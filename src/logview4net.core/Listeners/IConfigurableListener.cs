/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */
using System.Collections.Generic;

namespace logview4net.Listeners
{
    public interface IConfigurableListener: IListener
    {

        /// <summary>
        /// Gets the config value fields.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, ListenerConfigField> GetConfigValueFields();

        List<string> GetMultiOptions(string name);
        
        /// <summary>
        /// Sets the config value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        string SetConfigValue(string name, string value);

        /// <summary>
        /// Gets the config value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        string GetConfigValue(string name);
        
    }
}
