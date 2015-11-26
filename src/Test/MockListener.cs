/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

#if DEBUG
using System;
using System.Xml;

namespace logview4net.test
{
	/// <summary>
	/// This listener is used only for testing <see cref="Session"/> and the objects implementing <see cref="Viewers.IViewer" />.
	/// </summary>
	public class MockListener : Listeners.IListener
	{
        private bool _isRunning = false;
		/// <summary>
		/// A string that will preceed this listeners messages in the viewer.
		/// </summary>
		public string _messagePrefix="";
		/// <summary>
		/// The <see cref="Session"/> this listener belongs to.
		/// </summary>
		public Session _session;

		/// <summary>
		/// Creates a new <see cref="MockListener"/> instance.
		/// </summary>
		public MockListener()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <returns></returns>
		public string GetConfiguration()
		{
			XmlDocument doc = new XmlDocument();
			XmlNode configuration;
			configuration = doc.CreateElement("listener");

			XmlAttribute a = doc.CreateAttribute("type");
			a.Value = "Mock";
			configuration.Attributes.Append(a);

            return configuration.OuterXml ;

		}

		/// <summary>
		/// Adds an event to the current <see cref="Session"/>.
		/// </summary>
		/// <param name="message">Message.</param>
		public void AddEvent(string message)
		{
			_session.AddEvent(this, _messagePrefix + message);
		}

		#region IListener Members

		/// <summary>
		/// Not implemented.
		/// </summary>
		public logview4net.Viewers.IViewer Viewer
		{
			get
			{
				// TODO:  Add MockListener.Viewer getter implementation
				return null;
			}
			set
			{
				// TODO:  Add MockListener.Viewer setter implementation
			}
		}

		/// <summary>
		/// Not implemented
		/// </summary>
		public void Listen()
		{
			// TODO:  Add MockListener.Listen implementation
		}

		/// <summary>
		/// Not implemented
		/// </summary>
		public void Stop()
		{
            _isRunning = false;
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		public void Start()
		{
            _isRunning = true;
		}

		/// <summary>
		/// Sets the session.
		/// </summary>
		/// <value></value>
		public Session Session
		{
			set{ _session = value; }
		}
		/// <summary>
		/// Gets or sets the string that will preceed this listeners messages in the viewer.
		/// </summary>
		/// <value></value>
		public string MessagePrefix
		{
			set{_messagePrefix = value;}
			get{return _messagePrefix;}
		}

		/// <summary>
		/// Disposes this instance.
		/// </summary>
		public void Dispose()
		{
			this.Stop();
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

        #endregion
    }
}
#endif