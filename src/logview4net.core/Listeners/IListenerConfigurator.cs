/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
namespace logview4net.Listeners
{
    /// <summary>
    /// This is the interface all listener configurators has to implement.
    /// </summary>
    public interface IListenerConfigurator
    {
        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        string Caption { get; }

        /// <summary>
        /// Gets or sets the configuration data for an implementation of this interface.
        /// </summary>
        string Configuration { get; set; }

        /// <summary>
        /// Gets the listener for an implementation of this interface
        /// </summary>
        ListenerBase ListenerBase { get; set; }

        void UpdateControls();
    }
}