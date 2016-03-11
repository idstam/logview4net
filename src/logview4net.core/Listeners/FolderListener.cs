/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace logview4net.Listeners
{
    /// <summary>
    /// FileListener monitors log files.
    /// </summary>
    [Serializable]
    public class FolderListener : ListenerBase
    {
        private FileSystemWatcher _watcher = null;
        private bool _isRunning = false;
        /// <summary>
        /// This list (_fileLengths) is used for filenames that existed in the folder when the listener was started. This way I can 
        /// check if the length of the files has changed without having an open reader for each file. As soon as (if) 
        /// a file lengths is changed I create a new reader and put it in the _fileReadersList.
        /// </summary>
        private SortedList<string, long> _fileLengths = new SortedList<string, long>();

        private SortedList<string, StreamReader> _fileReaders = new SortedList<string, StreamReader>();

        /// <summary>
        /// Name of the file being monitored.
        /// </summary>
        protected string _folderName;

        /// <summary>
        /// Whether or not to add the file name to the message prefix
        /// </summary>
        private bool _fileNamePrefix;
        
        /// <summary>
        /// Whether or not to add the short file name to the message prefix
        /// </summary>
        private bool _shortFileNamePrefix;
        private bool _showNoData;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderListenerBase"/> class.
        /// </summary>
        public FolderListener()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "FolderListener()");
            IsRestartable = true;
        }

        /// <summary>
        /// Creates a new <see cref="FileListenerBase"/> instance.
        /// </summary>
        /// <param name="folderName">Name of the folder to monitor.</param>
        /// <param name="messagePrefix">A string that will preceed this listeners messages in the viewer.</param>
        /// <param name="onlyTail">Whether or not to load the whole existing files when starting the listener thread.</param>
        /// <param name="fileNamePrefix">Whether or not to prefix messages with the filename it came from.</param>
        public FolderListener(string folderName, string messagePrefix, bool fileNamePrefix, bool shortFileNamePrefix)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "FolderListener(string, int, string, bool, bool)");
            _folderName = folderName;
            MessagePrefix = messagePrefix;
            _fileNamePrefix = fileNamePrefix;
            _shortFileNamePrefix = shortFileNamePrefix;
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value></value>
        public string FolderName
        {
            get { return _folderName; }
            set { _folderName = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [file name prefix].
        /// </summary>
        /// <value><c>true</c> if [file name prefix]; otherwise, <c>false</c>.</value>
        public bool FileNamePrefix
        {
            get { return _fileNamePrefix; }
            set { _fileNamePrefix = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [short file name prefix].
        /// </summary>
        /// <value><c>true</c> if [short file name prefix]; otherwise, <c>false</c>.</value>
        public bool ShortFileNamePrefix
        {
            get { return _shortFileNamePrefix; }
            set { _shortFileNamePrefix = value; }
        }

        public bool ShowNoData
        {
            get { return _showNoData; }
            set { _showNoData = value; }
        }

        #region IListener Members

        /// <summary>
        /// Gets the reader.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        protected StreamReader GetReader(string fileName)
        {
            if (_fileReaders.ContainsKey(fileName))
            {
                return _fileReaders[fileName];
            }
            else
            {

                StreamReader ret = null;
                try
                {
                    ret = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite));
                    _fileReaders.Add(fileName, ret);
                }
                catch (Exception ex)
                {
                    Session.AddEvent(this, "Error when opening: " + fileName + Environment.NewLine + ex.ToString());
                }
                return ret;
            }
        }

        private void closeAllReaders()
        {
            foreach (var sr in _fileReaders.Values)
            {
                sr.Close();
                sr.Dispose();
            }
        }


        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void Stop()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Stop");

            if (_isRunning)
            {
                _watcher.EnableRaisingEvents = false;
                _isRunning = false;
                closeAllReaders();
                _watcher.Dispose();
                _watcher = null;
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public override void Start()
        {
            _log.Info(GetHashCode(), "Started polling log entries in " + _folderName );

            if (_watcher == null)
            {
                _watcher = new FileSystemWatcher(_folderName);
                _watcher.Created += new FileSystemEventHandler(watcher_Created);
                _watcher.Changed += new FileSystemEventHandler(watcher_Changed);
                _watcher.Deleted += new FileSystemEventHandler(_watcher_Deleted);

                GetInitialFileData();
            }

            _isRunning = true;

            _watcher.EnableRaisingEvents = true;
            _isRunning = true;
        }

        private void GetInitialFileData()
        {
            foreach (var f in Directory.GetFiles(_folderName))
            {
                if (!_showNoData)
                {
                    //Store existing file lengts so that all of the files doesn't have to be read.
                    var fi = new FileInfo(f);
                    _fileLengths.Add(f, fi.Length);
                }
                Session.AddEvent(this, "File found: (" + GetShortFileName(f) + ")" + f);
            }
        }


        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var line = "";
            var linePrefix = GetEventPrefix(e);
            if (_showNoData)
            {
                Session.AddEvent(this, "File changed: (" + GetShortFileName(e.FullPath) + ")" + e.FullPath);
            }
            else
            {
                var r = GetReader(e.FullPath);
                if (r == null) return;  //Something failed when getting the stream. There should be an error message on the screen.

                r.BaseStream.Seek(_fileLengths[e.FullPath], SeekOrigin.Begin);
                
                var lines = new List<string>();
                while ((line = r.ReadLine()) != null)
                {
                    line = linePrefix  + line;

                    lines.Add(line);

                }
                Session.AddEvent(this, lines);
                _fileLengths[e.FullPath] = r.BaseStream.Position;
            }
        }

        private string GetEventPrefix(FileSystemEventArgs e)
        {
            var line = "";
            if (_fileNamePrefix)
            {
                line = e.Name + " : ";
            }
            if (_shortFileNamePrefix)
            {
                line = GetShortFileName(e.FullPath) + " : ";
            }
            return line;
        }

        private void watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (File.Exists(e.FullPath)) //Make sure it isn't a folder
            {
                var fi = new FileInfo(e.FullPath);
                if (_fileLengths.ContainsKey(e.FullPath))
                {
                    _fileLengths.Remove(e.FullPath);
                }
                _fileLengths.Add(e.FullPath, 0);

                Session.AddEvent(this, "File created: (" + GetShortFileName(e.FullPath) + ")" + e.FullPath);
            }
        }

        private void _watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (_fileLengths.ContainsKey(e.FullPath))
            {
                _fileLengths.Remove(e.FullPath);
                Session.AddEvent(this, "File deleted: (" + GetShortFileName(e.FullPath) + ")" + e.FullPath);
            }
            
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Dispose");
            if (_isRunning)
            {
                Stop(); 
            }
        }

        /// <summary>
        /// True if this listener has no historic data.
        /// </summary>
        /// <returns></returns>
        public bool OnlyTail
        {
            get { return true; }
            set { }
        }

        #endregion  IListener Members

        /// <summary>
        /// Gets the config value fields.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, ListenerConfigField> GetConfigValueFields()
        {
            var ret = new Dictionary<string, ListenerConfigField>();

            var f = new ListenerConfigField();
            f.Name = "Prefix";
            f.MultiValueType = MultiValueTypes.None;
            ret.Add("prefix", f);

            f = new ListenerConfigField();
            f.Name = "Add filename to prefix";
            f.MultiValueType = MultiValueTypes.Check;
            ret.Add("filename_prefix", f);

            f = new ListenerConfigField();
            f.Name = "Add short filename to prefix";
            f.MultiValueType = MultiValueTypes.Check;
            ret.Add("short_filename_prefix", f);

            f = new ListenerConfigField();
            f.Name = "Show no file content";
            f.MultiValueType = MultiValueTypes.Check;
            ret.Add("show_no_data", f);

            f = new ListenerConfigField();
            f.Name = "BREAK";
            f.MultiValueType = MultiValueTypes.Linebreak;
            ret.Add("break1", f);

            f = new ListenerConfigField();
            f.Name = "Folder";
            f.MultiValueType = MultiValueTypes.None;
            f.Width = 45;
            f.AlignTo = "prefix";
            ret.Add("folder_name", f);

            f = new ListenerConfigField();
            f.Name = "...";
            f.MultiValueType = MultiValueTypes.FolderBrowserButton;
            ret.Add("folder_open", f);

            //f = new ListenerConfigField();
            //f.Name = "BREAK";
            //f.MultiValueType = MultiValueTypes.Linebreak;
            //ret.Add("break1", f);

            return ret;
        }

        /// <summary>
        /// Gets the config value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public override string GetConfigValue(string name)
        {
            switch (name)
            {
                case "prefix":
                    return MessagePrefix;
                case "filename_prefix":
                    return _fileNamePrefix.ToString();
                case "short_filename_prefix":
                    return _shortFileNamePrefix.ToString();
                case "show_no_data":
                    return _showNoData.ToString();
                case "folder_name":
                    return _folderName;
                case "folder_open":
                    return null;
                default:
                    throw new NotImplementedException("This listener has no field named: " + name);
            }
        }
        /// <summary>
        /// Sets the config value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override string SetConfigValue(string name, string value)
        {
            string ret = null;
            switch (name)
            {
                case "prefix":
                    MessagePrefix = value;
                    break;
                case "filename_prefix":
                    if (!bool.TryParse(value, out _fileNamePrefix))
                    {
                        ret = "Please enter [true] or [false] here.";
                    }
                    break;
                case "short_filename_prefix":
                    if (!bool.TryParse(value, out _shortFileNamePrefix))
                    {
                        ret = "Please enter [true] or [false] here.";
                    }
                    break;
                case "show_no_data":
                    if (!bool.TryParse(value, out _showNoData))
                    {
                        ret = "Please enter [true] or [false] here.";
                    }
                    break;
                case "folder_name":
                    _folderName = value;
                    break;
                case "folder_open":
                    _folderName = value;
                    break;
                default:
                    throw new NotImplementedException("This listener has no field named: " + name);
            }

            return ret;
        }
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName([MarshalAs(UnmanagedType.LPTStr)]string path,[MarshalAs(UnmanagedType.LPTStr)]StringBuilder shortPath,int shortPathLength );
        private string GetShortFileName(string fileName)
        {
            var shortPath = new StringBuilder(255);
            GetShortPathName(fileName, shortPath, shortPath.Capacity);
            return Path.GetFileName(shortPath.ToString()).PadRight(12, ' ');
        }
    }
}
