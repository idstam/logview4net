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


namespace logview4net.Listeners
{	
    /// <summary>
    /// Description of TcpListener.	
    /// </summary>
    public class TcpListenerBase : ListenerBase
    {
        private bool _isRunning = false;
        private int _port;
        private string _endpoint;
        private Thread _listenerThread = null;
        private System.Net.Sockets.TcpListener  _tcpListener = null;
        private string _encName ="Unicode";
        private HexEncoder _hexEncoder = new HexEncoder();
        /// <summary>
        /// A string that will preceed this listeners messages in the viewer.
        /// </summary>
        private ulong _address;

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpListenerBase"/> class.
        /// </summary>
        public TcpListenerBase()
        {
            _log = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            IsRestartable = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TcpListenerBase"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="port">The port.</param>
        /// <param name="messagePrefix">The message prefix.</param>
        public TcpListenerBase(string endpoint, int port, string messagePrefix)
        {
            _log = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            _port = port;	
            _endpoint = endpoint;
            MessagePrefix = messagePrefix;
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

            if(Session == null) throw new NullReferenceException("This IListener has no Session yet!");

            IPEndPoint remoteEndPoint;
            if(_endpoint == "ANY")
            {
                remoteEndPoint = new IPEndPoint(IPAddress.Any, _port);
            }
            else
            {
                remoteEndPoint = new IPEndPoint(IPAddress.Parse(_endpoint), _port);
            }
            
            var buffer = new Byte[256];
            string loggingEvent;

            try
            {
                _tcpListener = new System.Net.Sockets.TcpListener(remoteEndPoint);
                _tcpListener.Start();
                _log.Info(GetHashCode(), "Started listening for TCP on port " + _port.ToString() + " using prefix: " + MessagePrefix);

                while (true)
                {
                    var client = _tcpListener.AcceptTcpClient();

                    var sender = "";
                    if (Session.Viewer.ShowListenerPrefix)
                    {
                        sender = client.Client.RemoteEndPoint.ToString();
                    }
                   
                    var data = "";
                    var stream = client.GetStream();

                    int i;

                    if (_encName == HexEncoder.EncName)
                    {
                        data = _hexEncoder.GetHex(_address, buffer);
                    }
                    else
                    {
                        // Loop to receive all the data sent by the client.
                        while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            data += Encoding.GetEncoding(_encName).GetString(buffer, 0, i);
                        }
                    }
                    // Shutdown and end connection
                    client.Close();
                    loggingEvent = sender + " " + data;
                    Session.AddEvent(this, loggingEvent);
                }
            }
            catch (SocketException ex)
            {
                var foo = "(TCP ERROR) The TCP listener could not be started at port " + _port.ToString() + " " + Environment.NewLine + ex.Message;
                _log.Error(GetHashCode(), foo);
                Session.AddEvent(this, foo);
            }
            catch (ThreadAbortException)
            {
                var foo = "(TCP ERROR) TcpListener aborted.";
                _log.Error(GetHashCode(), foo);
                Session.AddEvent(this, foo);
            }
            catch (Exception e)
            {
                var foo = "(TCP ERROR) Internal error in TcpListener: " + e.Message + Environment.NewLine + e.StackTrace;
                _log.Error(GetHashCode(), foo);
                Session.AddEvent(this, foo);

            }
            finally
            {
                if (_tcpListener != null)
                {
                    _tcpListener.Stop();
                }
            }
    

        }

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
                if (_listenerThread != null)
                {
                    _listenerThread.Abort();
                }

                if (_tcpListener != null)
                {
                    _tcpListener.Stop();
                }

                _isRunning = false;
            }
            catch(Exception ex)
            {
                _log.Error(GetHashCode(), "Failed to stop the tcp listener.", ex);
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

        #region IListener Members

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

        
        public override Dictionary<string, ListenerConfigField> GetConfigValueFields()
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

        public override string GetConfigValue(string name)
        {
            switch (name)
            {
                case "prefix":
                    return MessagePrefix;
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
        public override string SetConfigValue(string name, string value)
        {
            string ret = null;
            switch (name)
            {
                case "prefix":
                    MessagePrefix = value;
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
    }
}
