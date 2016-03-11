/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Reflection;
using logview4net.Viewers;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using Action = logview4net.Viewers.Action;
using logview4net.Listeners;

namespace logview4net.test
{
	/// <summary>
	/// Summary description for MockViewer.
	/// </summary>
	public class MockViewer : IViewer
	{
		/// <summary>
		/// Whether or not clear has been called on this instance.
		/// </summary>
        private bool _clearCalled;

        /// <summary>
        /// Idicate if Clear was called.
        /// </summary>
        public bool ClearCalled
        {
            get { return _clearCalled; }
            set { _clearCalled = value; }
        }
		
        public bool RemoveWhitespace{ get; set;}
		/// <summary>
		/// Exposed to monitor the data received by this viewer.
		/// </summary>
        public List<string> ReceivedData = new List<string>();
		private int _bufferSize;
		
		private ILog _log;

		/// <summary>
		/// Creates a new <see cref="MockViewer"/> instance.
		/// </summary>
		public MockViewer()
		{
			//
			// TODO: Add constructor logic here
			//
			_log = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		}
		#region IViewer Members

		/// <summary>
		/// Adds an event to this viewer
		/// </summary>
		/// <param name="message">Message.</param>
		public void AddEvent(string message, ListenerBase listener)
		{
            _log.Debug(GetHashCode(), message);

			ReceivedData.Add( message);
		}

        /// <summary>
        /// Adds a list of events to this viewer
        /// </summary>
        public void AddEvent(string prefix, List<string> lines, ListenerBase listenerBase)
        {
            ReceivedData.AddRange(lines);
        }
        
        /// <summary>
		/// Adds an event to this viewer
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="action">Action.</param>
		public void AddEvent(string message, List<Action> action)
		{
            _log.Debug(GetHashCode(), action + ":" + message);
			ReceivedData.Add(message);
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			ClearCalled = true;
		}

		/// <summary>
		/// Gets or sets the size of the buffer.
		/// </summary>
		/// <value></value>
		public int BufferSize
		{
			get{ return  _bufferSize; }
			set{ _bufferSize = value; }
		}

		/// <summary>
		/// Not implemented
		/// </summary>
		public void Pause()
		{
		}

        /// <summary>
        /// Wether or not a viewer should wrap lines
        /// </summary>
        /// <value></value>
        public bool WordWrap
        {
			get{ return true; }
			set {}
		}

		/// <summary>
		/// Adds an actin to this Viewer
		/// </summary>
		/// <param name="action">Action.</param>
		public void AddAction(Action action)
		{
		}

        /// <summary>
        /// Empty method just to implement the interface
        /// </summary>
        /// <param name="action"></param>
        public void RemoveAction(Action action)
        {
            
        }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MockViewer"/> is paused.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if paused; otherwise, <c>false</c>.
		/// </value>
		public bool Paused
		{
			get{ return true; }
			set {}
		}

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <returns></returns>
		public string GetConfiguration()
		{
            return "";
		}

        /// <summary>
        /// Empty method just to implement the interface
        /// </summary>
        public List<Action> Actions
        {
            get { return null; }
        }
        #endregion

        public event EventHandler HasHiddenMessagesEvent;

        public bool HasHiddenMessages()
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// Rasise the HasHiddenMessagesEvent event
		/// </summary>
		/// <param name="hasHiddenMessages"></param>
		public void RaiseHasHiddenMessages(bool hasHiddenMessages)
		{
			if (HasHiddenMessagesEvent != null)
			{
				var e = new HasHiddenMessagesEventArgs(hasHiddenMessages);
				HasHiddenMessagesEvent(this, e);
			}
		}

		
        public void ShowAllHidden()
        {
            throw new NotImplementedException();
        }
		
		public bool ShowListenerPrefix { get; set; }
		
		public string LogFile {get; set;}
		public string LogRolling {get; set;}
		public bool LogToFile {get; set;}
    }
}