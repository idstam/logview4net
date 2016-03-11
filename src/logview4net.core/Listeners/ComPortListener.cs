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
using System.Text;
using System.Threading;

namespace logview4net.Listeners
{
	/// <summary>
    /// ComPortListener a listener for COM-ports
	/// </summary>
    [Serializable]
	public class ComPortListener : ListenerBase
	{
		private Thread _listenerThread = null;
        private string _encName ="Unicode";

        private bool _isRunning = false;        

		/// <summary>
		/// Name of the file being monitored.
		/// </summary>
		protected string _portName;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ComPortListenerBase"/> class.
        /// </summary>
        public ComPortListener()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "ComPortListener()");
            IsRestartable = false;
        }

		/// <summary>
        /// Creates a new <see cref="ComPortListenerBase"/> instance.
		/// </summary>
		/// <param name="fileName">Name of the executable to monitor.</param>
		/// <param name="messagePrefix">A string that will preceed this listeners messages in the viewer.</param>
        public ComPortListener(string portName, string messagePrefix, bool killOnExit)
		{
            if (_log.Enabled) _log.Debug(GetHashCode(), "ComPortListener(string, string)");

			_portName = portName;
			MessagePrefix = messagePrefix;
		    _killOnStop = killOnExit;
		}

		/// <summary>
		/// Gets or sets the name of the executable.
		/// </summary>
		/// <value></value>
		public string PortName
		{
			get{ return _portName; }
			set{ _portName = value; }
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
		/// Starts this instance.
		/// </summary>
		public override void Start()
		{
            if (_log.Enabled) _log.Debug(GetHashCode(), "Start");

			var ts = new ThreadStart(Listen);
			_listenerThread = new Thread(ts);
			_listenerThread.Start();

            _isRunning = true;
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		public override void Stop()
		{
            if (_log.Enabled) _log.Debug(GetHashCode(), "Stop");

			try
			{

				/*
                if (_udpClient != null)
                {
                    _udpClient.Close();
                    _udpClient = null;
                    
                }

                if (_listenerThread != null)
                {
                    _listenerThread.Abort();
                }

				*/
                _isRunning = false;
			}
			catch(Exception ex)
			{
                _log.Error(GetHashCode(), "Failed to stop the udp listener.", ex);
			}
            
		}

		protected void Listen()
		{
			
		}
        void _process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Session.AddEvent(this, e.Data);
        }

        void _process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Session.AddEvent(this, e.Data);
        }

        void _process_Exited(object sender, EventArgs e)
        {
            Session.AddEvent(this, _portName + " has exited.");
            _process.Dispose();
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
            get{ return true;}
            set{}
        }
        #endregion

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
            f.Name = "Kill on stop";
            f.MultiValueType = MultiValueTypes.Check;
            ret.Add("kill_on_stop", f);

            f = new ListenerConfigField();
            f.Name = "BREAK";
            f.MultiValueType = MultiValueTypes.Linebreak;
            ret.Add("break1", f);

            f = new ListenerConfigField();
            f.Name = "Port";
            f.MultiValueType = MultiValueTypes.None;
            f.Width = 45;
            ret.Add("port_name", f);

            f = new ListenerConfigField();
            f.Name = "Encoding";
            f.MultiValueType = MultiValueTypes.Combo ;
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
                case "port_name":
                    return _portName;
                case "kill_on_stop":
                    return _killOnStop.ToString();
                case "encoding":
                   return _encName; 
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
                case "port_name":
                    _portName = value;
                    break;
                case "kill_on_stop":
                    _killOnStop = bool.Parse(value);
                    break;
                   case "encoding":
                    _encName = value;
                    break;
                default:
                    throw new NotImplementedException("This listener has no field named: " + name);
            }

            return ret;
        }
    }
}
