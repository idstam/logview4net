/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Xml;

using logview4net.Listeners;
using System.Collections.Generic;
namespace logview4net.Viewers
{
	/// <summary>
	/// The interface that all viewers has to implement..
	/// </summary>
	public interface IViewer
	{
        event EventHandler HasHiddenMessagesEvent;
        bool HasHiddenMessages();
        void ShowAllHidden();

		/// <summary>
		/// Adds an event to the implemented viewer.
		/// </summary>
		/// <param name="message">The text to display</param>
		void AddEvent(string message, ListenerBase listenerBase);

        /// <summary>
        /// Adds a list of events to the implemented viewer.
        /// </summary>
        /// <param name="message">The text to display</param>
        void AddEvent(string prefix, List<string> message, ListenerBase listenerBase);

		void Clear();

        /// <summary>
        /// Whether or not a viewer should wrap lines
        /// </summary>
        bool WordWrap
        {
            get;
            set;
        }
		/// <summary>
		/// Pauses and un-pauses the viewer
		/// </summary>
		bool Paused
		{
			get;
			set;
		}

		/// <summary>
		/// Size of buffer in a viewer. Number of lines in a <see cref="textViewer"/>
		/// </summary>
		int BufferSize
		{
			get;
			set;
		}
		
		bool RemoveWhitespace {get; set;}
        bool ShowListenerPrefix { get; set; }

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <returns></returns>
		string GetConfiguration();

		string LogFile {get; set;}
		string LogRolling {get; set;}
		bool LogToFile {get; set;}
	}

    public class HasHiddenMessagesEventArgs : EventArgs
    {
        public bool HasHiddenMessages;
        public HasHiddenMessagesEventArgs(bool hasHiddenMessages):base()
        {
           
            HasHiddenMessages = hasHiddenMessages;
        }
    }
}
