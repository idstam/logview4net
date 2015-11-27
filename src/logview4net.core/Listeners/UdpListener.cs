/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics; 


namespace logview4net.Listeners
{	
    /// <summary>
    /// Description of UdpListener.	
    /// </summary>
    public class UdpListener : IConfigurableListener
    {
        private bool _isRunning = false;
        private HexEncoder _hexEncoder;
        private ulong _address = 0;
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

        private int _port;
        private string _endpoint;
        private Session _session;
        private Thread _listenerThread = null;
        private ILog _log;
        private UdpClient _udpClient = null;
        private string _encName ="Unicode";
        
        /// <summary>
        /// A string that will preceed this listeners messages in the viewer.
        /// </summary>
        private string _messagePrefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpListener"/> class.
        /// </summary>
        public UdpListener()
        {
            _log = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="UdpListener"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="port">The port.</param>
        /// <param name="messagePrefix">The message prefix.</param>
        public UdpListener(string endpoint, int port, string messagePrefix)
        {
            _log = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            _port = port;	
            _endpoint = endpoint;
            _messagePrefix = messagePrefix;
        }
        
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        public string GetConfiguration()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "getConfiguration");
            return ListenerHelper.SerializeListener(this);

        }

        /// <summary>
        /// Gets or sets the endpoint.
        /// </summary>
        /// <value></value>
        public string Endpoint
        {
            get{ return _endpoint; }
            set{ _endpoint = value; }
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value></value>
        public int Port
        {
            get{ return _port; }
            set{ _port = value; }
        }

        public string EncName
        {
            get{return _encName;}
            set{_encName = value;}
        }
        /// <summary>
        /// This is where all the actual 'listening' is done.
        /// </summary>
        protected void Listen()
        {

            if(_session == null) throw new NullReferenceException("This IListener has no Session yet!");

            IPEndPoint remoteEndPoint;
            if(_endpoint == "ANY")
            {
                remoteEndPoint = new IPEndPoint(IPAddress.Any, _port);
            }
            else
            {
                remoteEndPoint = new IPEndPoint(IPAddress.Parse(_endpoint), _port);
            }
            
            byte[] buffer;
            string loggingEvent;
            _hexEncoder = new HexEncoder();
            try
            {
                _udpClient = new UdpClient(_port);
                
                _log.Info(GetHashCode(), "Started listening for UDP on port " + _port.ToString() + " using prefix: " + _messagePrefix);

                while (true)
                {
                        buffer = _udpClient.Receive(ref remoteEndPoint);

                        var sender = "";
                        if (_session.Viewer.ShowListenerPrefix)
                        {
                            sender = remoteEndPoint.Address.ToString() + " ";
                        }

                        if (_encName == HexEncoder.EncName)
                        {
                            loggingEvent = sender + _hexEncoder.GetHex(_address, buffer);
                            _address += (ulong)buffer.Length;
                        }
                        else
                        {
                            loggingEvent = sender + Encoding.GetEncoding(_encName).GetString(buffer);
                        }
                        _session.AddEvent(this, loggingEvent);

                }
            }
            catch (SocketException ex)
            {
                var foo = "(UDP ERROR) The UDP listener could not be started at port " + _port.ToString() + " " + Environment.NewLine + ex.Message;
                _log.Error(GetHashCode(), foo);
                _session.AddEvent(this, foo);
            }
            catch (ThreadAbortException)
            {
                var foo = "(UDP) UdpListener thread aborted.";
                _log.Error(GetHashCode(), foo);
                _session.AddEvent(this, foo);
            }
            catch (Exception e)
            {
                var foo = "(UDP ERROR) Internal error in UdpListener: " + e.Message + Environment.NewLine + e.StackTrace;
                _log.Error(GetHashCode(), foo);
                _session.AddEvent(this, foo);

            }
            finally
            {
                if (_udpClient != null)
                {
                    _udpClient.Close();
                }
            }
    

        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
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
        public void Stop()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Stop");

            try
            {

                if (_udpClient != null)
                {
                    _udpClient.Close();
                    _udpClient = null;
                    
                }

                if (_listenerThread != null)
                {
                    _listenerThread.Abort();
                }


                _isRunning = false;
            }
            catch(Exception ex)
            {
                _log.Error(GetHashCode(), "Failed to stop the udp listener.", ex);
            }
            
        }

        /// <summary>
        /// Sets the session.
        /// </summary>
        /// <value></value>
        public Session Session
        {
            set
            {
                if (_log.Enabled) _log.Debug(GetHashCode(), "Session set");
                Debug.Assert(value != null, "Session can not be null here.");
                _session = value; 
            }
        }
        /// <summary>
        /// Gets or sets the string that will preceed this listeners messages in the viewer.
        /// </summary>
        /// <value></value>
        public string MessagePrefix
        {
            set{_messagePrefix = value;}
            get{
                return _messagePrefix;
            }
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

        #region IListener Members


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
        [Obsolete("This method is going to be removed from the IListener interface", true )]
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
            get { return true; }
            set { }
        }

        #endregion

        
        public Dictionary<string, ListenerConfigField> GetConfigValueFields()
        {
            var ret = new Dictionary<string, ListenerConfigField>();

            var f = new ListenerConfigField();
                f.Name = "Prefix";
                f.MultiValueType = MultiValueTypes.None;
                ret.Add("prefix", f);

            f = new ListenerConfigField();
                f.Name = "Sender IP";
                f.MultiValueType = MultiValueTypes.None;
                ret.Add( "sender_ip", f);

                f = new ListenerConfigField();
                f.Name = "Port";
                f.MultiValueType = MultiValueTypes.None;
                ret.Add("port", f);
                
                //f = new ListenerConfigField();
                //f.Name = "BREAK";
                //f.MultiValueType = MultiValueTypes.Linebreak;
                //ret.Add("break1", f);

                f = new ListenerConfigField();
                f.Name = "Encoding";
                f.MultiValueType = MultiValueTypes.Combo ;
                ret.Add("encoding", f);
                

                return ret;
        }
        public List<string> GetMultiOptions(string name)
        {
            var ret = new List<string>();
            ret.Add(HexEncoder.EncName);
            foreach(var t in Encoding.GetEncodings())
            {
                ret.Add(t.Name);
            }
            ret.Add("Unicode");
            
            return ret;
        }
        public string GetConfigValue(string name)
        {
            switch (name)
            {
                case "prefix":
                    return _messagePrefix;
                case "sender_ip":
                    return _endpoint;
                case "port":
                    return _port.ToString();
                case "encoding":
                   return _encName; 
                default:
                    throw new NotImplementedException("This listener has no field named: " + name);
            }
        }
        public string SetConfigValue(string name, string value)
        {
            string ret = null;
            switch (name)
            {
                case "prefix":
                    _messagePrefix = value;
                    break;
                case "sender_ip":
                    _endpoint = value;
                    break;
                case "port":
                    if(! int.TryParse(value,out _port))
                    {
                        ret ="Please enter an integer here.";
                    }
                    
                    break;
                   case "encoding":
                    _encName = value;
                    break;
                default:
                    throw new NotImplementedException("This listener has no field named: " + name);
            }
            return ret;
        }
        public bool ShowTimestamp { get; set; }
        public string TimestampFormat { get; set; }
    }
}
