/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace logview4net.Viewers
{
    /// <summary>
    /// Represents a received event.
    /// </summary>
    public class LogEvent
    {
        /// <summary>
        /// The text of the event
        /// </summary>
        public string Message;

        /// <summary>
        /// The actions associated with the event.
        /// </summary>
        public List<Action> Actions = new List<Action>();

        public string Structure{ get; set; }
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="actions"></param>
        /// <param name="getActions"></param>
        /// <param name="structure"></param>
        public LogEvent(string message, List<Action> actions, bool getActions, string structure)
        {
            Structure = structure;

            Message = message;
            if (getActions)
            {
                Actions = ViewerUtils.GetActions(actions, Message);
            }
            else
            {
                Actions = actions;
            }
        }
    
        /// <summary>
        /// Contructor
        /// </summary>
        public LogEvent()
        {
            Message = "";
            Actions = new List<Action>();
        }

    }
}
