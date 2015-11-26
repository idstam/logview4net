/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System.Collections.Generic;
using System;
namespace logview4net.Listeners
{
    /// <summary>
    /// The interface that has to be implemented by all listeners.
    /// As of this writing the implementers are:
    /// <see cref="UdpListener"/>,
    /// <see cref="EventLogListener"/>,
    /// <see cref="FileListener"/>.
    /// </summary>
    public interface IListener
    {
    	/// <summary>
        /// Indicates whether the listener has column headers that the viewer can ask for.
        /// </summary>
    	bool HasColumnHeaders {get;}
    	
        bool IsRestartable{get;}

        /// <summaBry>
        /// Sets the implementing objects <see cref="Session"/>
        /// </summary>
        Session Session { set; }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        bool IsRunning { get; }

        /// <summary>
        /// The string that will preceed the implementing listeners messages in the viewer.
        /// </summary>
        string MessagePrefix { set; get; }

        /// <summary>
        /// Stops listening for the object implementing this interface.
        /// </summary>
        void Stop();

        /// <summary>
        /// Starts listening for the object implementing this interface.
        /// </summary>
        void Start();

        

        /// <summary>
        /// Gets a new configurator.
        /// </summary>
        /// <returns></returns>
        IListenerConfigurator GetNewConfigurator();
        /// <summary>
        /// Gets the configuration node for this listener.
        /// </summary>
        /// <returns></returns>
        string GetConfiguration();

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        void Dispose();

        /// <summary>
        /// True if this listener has no historic data.
        /// </summary>
        /// <returns></returns>
        bool OnlyTail
        {
            get;
            set;
        }

        string Hash
        {
            get;
        }

        bool IsConfigured { get; set; }

        bool ShowTimestamp { get; set; }

        string TimestampFormat { get; set; }
    }
}