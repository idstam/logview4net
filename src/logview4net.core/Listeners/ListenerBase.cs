/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace logview4net.Listeners
{
    /// <summary>
    /// The interface that has to be implemented by all listeners.
    /// As of this writing the implementers are:
    /// <see cref="UdpListenerBase"/>,
    /// <see cref="EventLogListener"/>,
    /// <see cref="FileListenerBase"/>.
    /// </summary>
    public abstract class ListenerBase
    {
        protected ILog _log = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        
        /// <summary>
        /// Gets the config value fields.
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, ListenerConfigField> GetConfigValueFields();


        public List<string> GetMultiOptions(string name)
        {
            switch (name.ToLowerInvariant())
            {
                case "encoding":
                    return GetEncodingOptions();
                case "structured":
                    return GetStructuredOptions();
                default:
                    throw new ArgumentException("No know MultiOption named " + name);
            }


        }
        public List<string> GetStructuredOptions()
        {
            var ret = new List<string>();
            ret.Add("n/a");
            ret.Add("json");
            //ret.Add("xml");
            //ret.Add("csv (,)");
            //ret.Add("csv (;)");
            //ret.Add("csv (TAB)");

            return ret;
        }

        public List<string> GetEncodingOptions()
        {
            var ret = new List<string>();
            ret.Add(HexEncoder.EncName);
            foreach (var t in Encoding.GetEncodings())
            {
                ret.Add(t.Name);
            }
            ret.Add("Unicode");
            ret.Sort();
            return ret;
        }

        /// <summary>
        /// Sets the config value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public abstract string SetConfigValue(string name, string value);

        /// <summary>
        /// Gets the config value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public abstract string GetConfigValue(string name);

        protected string _structured = "n/a";
  
        /// <summary>
        /// Indicates whether the listener has column headers that the viewer can ask for.
        /// </summary>
        public bool IsStructured => _structured != "n/a";

        public bool IsRestartable{get; set; }

        /// <summaBry>
        /// Sets the implementing objects <see cref="Session"/>
        /// </summary>
        [XmlIgnore]
        public Session Session { protected get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning { get; set; }

        /// <summary>
        /// The string that will preceed the implementing listeners messages in the viewer.
        /// </summary>
        public string MessagePrefix { set; get; }

        /// <summary>
        /// Stops listening for the object implementing this interface.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Starts listening for the object implementing this interface.
        /// </summary>
        public abstract void Start();



        /// <summary>
        /// Gets a new configurator.
        /// </summary>
        /// <returns></returns>
        //JSI public abstract IListenerConfigurator GetNewConfigurator();

        /// <summary>
        /// Gets the configuration node for this listener.
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        public virtual string GetConfiguration()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "GetConfiguration");
            return ListenerHelper.SerializeListener(this);
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// True if this listener has no historic data.
        /// </summary>
        /// <returns></returns>
        bool OnlyTail
        {
            get;
            set;
        }

        public string Hash { get; } = Guid.NewGuid().ToString();

        public bool IsConfigured { get; set; }

        public bool ShowTimestamp { get; set; }

        public string TimestampFormat { get; set; }

        public IListenerConfigurator GetNewConfigurator()
        {
            return new ListenerConfigurator(this, Session);
        }
    }
}