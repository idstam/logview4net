/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;

namespace logview4net.Listeners
{
	/// <summary>
    /// StdOutListener starts a process and monitors it's stdout and stderr
	/// </summary>
    [Serializable]
	public class StdOutListener : IConfigurableListener
	{
        private string _hash = Guid.NewGuid().ToString();
        public string Hash
        {
            get { return _hash; }
        }

        public bool IsStructured{ get{ return false;}}
        public bool IsRestartable
        {
            get { return false; }
        }

        public bool IsConfigured { get; set; }

        private bool _isRunning = false;        

		/// <summary>
		/// The <see cref="Session"/> this listener belongs to.
		/// </summary>
		protected Session _session;
		/// <summary>
		/// A string that will preceed this listeners messages in the viewer.
		/// </summary>
		protected string _messagePrefix;
		/// <summary>
		/// Name of the file being monitored.
		/// </summary>
		protected string _fileName;
        /// <summary>
        /// The arguments to use when starting the executable
        /// </summary>
        private string _args;
        /// <summary>
        /// The process that this listener starts (and listens to)
        /// </summary>
        protected Process _process = null;
        /// <summary>
		/// The <see cref="StreamReader"/> used for readig the stream.
		/// </summary>
		protected StreamReader _reader;

        /// <summary>
        /// If the proces that is watched should be killed when the listener stops.
        /// </summary>
        private bool _killOnStop = false;

		private ILog  _log = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="StdOutListener"/> class.
        /// </summary>
        public StdOutListener()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "StdOutListener()");
        }

		/// <summary>
        /// Creates a new <see cref="StdOutListener"/> instance.
		/// </summary>
		/// <param name="fileName">Name of the executable to monitor.</param>
		/// <param name="messagePrefix">A string that will preceed this listeners messages in the viewer.</param>
        public StdOutListener(string fileName, string messagePrefix, bool killOnExit)
		{
            if (_log.Enabled) _log.Debug(GetHashCode(), "StdOutListener(string, string)");

			_fileName = fileName;
			_messagePrefix = messagePrefix;
		    _killOnStop = killOnExit;
		}

		/// <summary>
		/// Gets or sets the name of the executable.
		/// </summary>
		/// <value></value>
		public string FileName
		{
			get{ return _fileName; }
			set{ _fileName = value; }
		}

        /// <summary>
        /// Gets or sets the arguments for the executable.
        /// </summary>
        /// <value>The args.</value>
        protected string Args
        {
            get { return _args; }
            set { _args = value; }
        }

		#region IListener Members

		/// <summary>
		/// Sets the session for this listener.
		/// </summary>
		/// <value></value>
		public Session Session
		{
			set
			{
                _session = value ;
			}
		}

		/// <summary>
		/// Gets or sets the string that will preceed this listeners messages in the viewer.
		/// </summary>
		/// <value></value>
		public string MessagePrefix
		{
			get
			{
				return _messagePrefix;
			}
			set
			{
				_messagePrefix = value;
			}
		}

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <returns></returns>
		public string GetConfiguration()
		{
            if (_log.Enabled) _log.Debug(GetHashCode(), "GetConfiguration");
            return ListenerHelper.SerializeListener(this);
        }

		/// <summary>
		/// Stops this instance.
		/// </summary>
		public void Stop()
		{
            if (_log.Enabled) _log.Debug(GetHashCode(), "Stop");

			if(_process != null)
			{
                if(_killOnStop)
                {
                    _process.Kill();
                }
			    _process.Dispose();
    		    _log.Info(GetHashCode(), "Ended polling log entries for " + _fileName);
			}

            _isRunning = false;
		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		public void Start()
		{
            _log.Info(GetHashCode(), "Starting " + _fileName + " Using prefix: " + _messagePrefix);
            _process = new Process();
            _process.Exited += new EventHandler(_process_Exited);
            _process.ErrorDataReceived += new DataReceivedEventHandler(_process_ErrorDataReceived);
            _process.OutputDataReceived += new DataReceivedEventHandler(_process_OutputDataReceived);

            var si = new ProcessStartInfo(_fileName, _args);
            
            _process.StartInfo = si;
            si.UseShellExecute = false;
            si.RedirectStandardError = true;
            si.RedirectStandardOutput = true;

            _process.Start();
		    _process.BeginErrorReadLine();
		    _process.BeginOutputReadLine();

            _isRunning = true;

		}

        void _process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _session.AddEvent(this, e.Data);
        }

        void _process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            _session.AddEvent(this, e.Data);
        }

        void _process_Exited(object sender, EventArgs e)
        {
            _session.AddEvent(this, _fileName + " has exited.");
            _process.Dispose();
        }

		/// <summary>
		/// Disposes this instance.
		/// </summary>
		public void Dispose()
		{
            if (_log.Enabled) _log.Debug(GetHashCode(), "Dispose");
            if (_isRunning)
            {
                Stop();
            }
		}


        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get { return _isRunning; }
        }

        /// <summary>
        /// Gets a new configurator.
        /// </summary>
        /// <returns></returns>
        [Obsolete("This method is going to be removed from the IListener interface", true)]
        public IListenerConfigurator GetNewConfigurator()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// True if this listener has no historic data.
        /// </summary>
        /// <returns></returns>
        public bool OnlyTail
        {
            get{ return true;}
            set{}
        }
        #endregion

        /// <summary>
        /// Gets the config value fields.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ListenerConfigField> GetConfigValueFields()
        {
            var ret = new Dictionary<string, ListenerConfigField>();

            var f = new ListenerConfigField();
            f.Name = "Prefix";
            f.MultiValueType = MultiValueTypes.None;
            ret.Add("prefix", f);

            f = new ListenerConfigField();
            f.Name = "Kill on stop";
            f.MultiValueType = MultiValueTypes.Check;
            ret.Add("kill_on_stop", f);

            f = new ListenerConfigField();
            f.Name = "BREAK";
            f.MultiValueType = MultiValueTypes.Linebreak;
            ret.Add("break1", f);

            f = new ListenerConfigField();
            f.Name = "File";
            f.MultiValueType = MultiValueTypes.None;
            f.Width = 45;
            ret.Add("file_name", f);

            f = new ListenerConfigField();
            f.Name = "...";
            f.MultiValueType = MultiValueTypes.FileOpenButton;
            ret.Add("file_open", f);

            return ret;
        }
        public List<string> GetMultiOptions(string name)
        {
        	throw new NotImplementedException();
        }      
        /// <summary>
        /// Gets the config value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string GetConfigValue(string name)
        {
            switch (name)
            {
                case "prefix":
                    return _messagePrefix;
                case "file_name":
                    return _fileName;
                case "kill_on_stop":
                    return _killOnStop.ToString();
                case "file_open":
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
        public string SetConfigValue(string name, string value)
        {
            string ret = null;
            switch (name)
            {
                case "prefix":
                    _messagePrefix = value;
                    break;
                case "file_name":
                    _fileName = value;
                    break;
                case "file_open":
                    _fileName = value;
                    break;
                case "kill_on_stop":
                    _killOnStop = bool.Parse(value);
                    break;
                default:
                    throw new NotImplementedException("This listener has no field named: " + name);
            }

            return ret;
        }

        public bool ShowTimestamp {get; set;}
        public string TimestampFormat { get; set; }

    }
}
