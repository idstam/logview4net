/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace logview4net.Listeners
{
    /// <summary>
    /// FileListener monitors log files.
    /// </summary>
    [Serializable]
    public class FileListener : ListenerBase
    {
        private bool _isRunning;
        
        private HexEncoder _hexEncoder = new HexEncoder();
        private ulong _address;
        /// <summary>
        /// Name of the file being monitored.
        /// </summary>
        protected string _fileName;

        /// <summary>
        /// Time, in milliseconds, to pause between checking the file.
        /// </summary>
        protected int _pollInterval = 3000;

        /// <summary>
        /// The thread that does the actual 'listening'.
        /// </summary>
        protected Thread _listenerThread;

        /// <summary>
        /// Whether or not to load the whole file when starting the listener thread.
        /// </summary>
        protected bool _onlyTail = true;

        protected string _encName = "Unicode";

        /// <summary>
        /// The <see cref="StreamReader"/> used for readig the file.
        /// </summary>
        protected StreamReader _reader;


        /// <summary>
        /// Initializes a new instance of the <see cref="FileListenerBase"/> class.
        /// </summary>
        public FileListener()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "FileListener()");
            IsRestartable = true;

        }

        /// <summary>
        /// Creates a new <see cref="FileListenerBase"/> instance.
        /// </summary>
        /// <param name="fileName">Name of the file to monitor.</param>
        /// <param name="pollInterval">Time, in milliseconds, to pause between checking the file.</param>
        /// <param name="messagePrefix">A string that will preceed this listeners messages in the viewer.</param>
        /// <param name="onlyTail">Whether or not to load the whole file when starting the listener thread.</param>
        public FileListener(string fileName, int pollInterval, string messagePrefix, bool onlyTail)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "FileListener(string, int, string, bool)");

            _fileName = fileName;
            _pollInterval = pollInterval;
            MessagePrefix = messagePrefix;
            _onlyTail = onlyTail;
            IsRestartable = true;
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value></value>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// Gets or sets the poll interval.
        /// </summary>
        /// <value></value>
        public int PollInterval
        {
            get { return _pollInterval; }
            set { _pollInterval = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [only tail].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [only tail]; otherwise, <c>false</c>.
        /// </value>
        public bool OnlyTail
        {
            get { return _onlyTail; }
            set { _onlyTail = value; }
        }

        public string EncName
        {
            get { return _encName; }
            set { _encName = value; }
        }

        /// <summary>
        /// This listeners working method. Does the tail checking of the file.
        /// Most of this code it taken from http://www.codeproject.com/csharp/tail.asp. Written by Taylor Wood
        /// </summary>
        private void tail()
        {
            
            if (_log.Enabled) _log.Debug(GetHashCode(), "tail");

            if (! File.Exists(_fileName))
            {
                _log.Warn(GetHashCode(), "Can't find " + _fileName);
            }
            else
            {
                _reader =
                    new StreamReader(new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite));
                _log.Debug(GetHashCode(), "Reader created");

                try
                {
                    using (_reader)
                    {
                        //start at the end of the file or at the beginning
                        long startAt = 0;
                        if (_onlyTail)
                        {
                            startAt = _reader.BaseStream.Length;
                        }
    
                        //_log.Debug("Reading file from pos: " + lastMaxOffset.ToString());
                        tailLoop(startAt);
                    }
                }
                catch(Exception ex)
                {
                    _log.Error(GetHashCode(), "There was an error polling file:" + _fileName + Environment.NewLine, ex);
                }
            }
        }


        //_log.Debug(GetHashCode(), "Breaking the tail loop");
        private void tailLoop(long startAt)
        {
            var lastMaxOffset = startAt;
            
            while (true) {
                if (_reader.BaseStream == null) {
                    break;
                }
                if (_reader.BaseStream.Length != lastMaxOffset) {
                    lastMaxOffset = readData(lastMaxOffset);
                }
            	else
            	{
            		_log.Debug(GetHashCode(), "No change in tailed file.");
            	}
                Thread.Sleep(_pollInterval);
                if (!_isRunning) {
                    break;
                }
            }
        }

        private long readData(long lastMaxOffset)
        {
            if(_encName == HexEncoder.EncName)
            {
                return readBytes(lastMaxOffset);
            }
            else
            {
                return readLines(lastMaxOffset);
            }
        }
        private long readLines(long lastMaxOffset)
        {
            _reader.BaseStream.Seek(lastMaxOffset, SeekOrigin.Begin);
            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                Session.AddEvent(this, line);
            }
            
            lastMaxOffset = _reader.BaseStream.Position;
            return lastMaxOffset;
        }
        private long readBytes(long lastMaxOffset)
        {
            _reader.BaseStream.Seek(lastMaxOffset, SeekOrigin.Begin);

            
            var buffer = new byte[_hexEncoder.BlockLength];
            var buffPos = 0;
            int i;

            i = _reader.BaseStream.ReadByte();
            while (i != -1)
            {
                buffer[buffPos] = (byte)i;
                buffPos++;
                if(buffPos == _hexEncoder.BlockLength)
                {
                    var l = _hexEncoder.GetHex(_address, buffer);
                    Session.AddEvent(this, l);
                    _address += (ulong)buffPos;
                    buffPos = 0;
                }
                i = _reader.BaseStream.ReadByte();
            }
            
            lastMaxOffset = _reader.BaseStream.Position;
            return lastMaxOffset;
        }
        #region IListener Members

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void Stop()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Stop");

            if (_listenerThread != null)
            {
                if (_reader != null)
                {
                    _reader.Close();
                }
                _listenerThread.Abort();

                _log.Info(GetHashCode(), "Ended polling log entries for " + _fileName);
            }

            _isRunning = false;
        }



        /// <summary>
        /// Starts this instance.
        /// </summary>
        public override void Start()
        {
            _log.Info(GetHashCode(), "Started polling log entries in " + _fileName + " using prefix: " + MessagePrefix);
            var ts = new ThreadStart(tail);
            _listenerThread = new Thread(ts);
            //_listenerThread.Priority = ThreadPriority.BelowNormal;
            _listenerThread.Start();

            _isRunning = true;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Dispose");
            if (_isRunning )
            {
                Stop();
            }
        }

        #endregion


        /// <summary>
        /// Gets the config value fields to build the congif gui.
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
            f.Name = "Poll intervall (ms)";
            f.MultiValueType = MultiValueTypes.None;
            ret.Add("poll_intervall", f);

            f = new ListenerConfigField();
            f.Name = "Start at end";
            f.MultiValueType = MultiValueTypes.Check;
            ret.Add("only_tail", f);

            f = new ListenerConfigField();
            f.Name = "Structured";
            f.MultiValueType = MultiValueTypes.Combo;
            ret.Add("structured", f);


            f = new ListenerConfigField();
            f.Name = "BREAK";
            f.MultiValueType = MultiValueTypes.Linebreak;
            ret.Add("break1", f);

            f = new ListenerConfigField();
            f.Name = "File";
            f.MultiValueType = MultiValueTypes.None;
            f.AlignTo = "prefix";
            f.Width = 45;
            ret.Add("file_name", f);

            f = new ListenerConfigField();
            f.Name = "...";
            f.MultiValueType = MultiValueTypes.FileOpenButton;
            ret.Add("file_open", f);

            //f = new ListenerConfigField();
            //f.Name = "BREAK";
            //f.MultiValueType = MultiValueTypes.Linebreak;
            //ret.Add("break1", f);

            f = new ListenerConfigField();
            f.Name = "Encoding";
            f.MultiValueType = MultiValueTypes.Combo;
            ret.Add("encoding", f);

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
                case "poll_intervall":
                    return _pollInterval.ToString();
                case "only_tail":
                    return _onlyTail.ToString();
                case "file_name":
                    return _fileName;
                case "file_open":
                    return null;
                case "encoding":
                    return _encName;
                case "structured":
                    return _structured;
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
                case "poll_intervall":
                    _pollInterval = ListenerHelper.GetSafeInt(value);
                    break;
                case "only_tail":
                    if(! bool.TryParse(value, out _onlyTail))
                    {
                        ret = "Please enter [true] or [false] here.";
                    }
                    break;
                case "file_name":
                    _fileName = value;
                    break;
                case "encoding":
                    _encName = value;
                    break;
                case "structured":
                    _structured = value;
                    break;
                case "file_open":
                    _fileName = value;
                    break;
                default:
                    throw new NotImplementedException("This listener has no field named: " + name);
            }
            
            return ret;
        }

    }
}
