/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2010 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Drawing;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections;

namespace logview4net.Viewers
{
    /// <summary>
    /// These are the types of actions that the viewers should handle.
    /// New items MUST be inserted in priority order. 
    /// </summary>
    public enum ActionTypes
    {
        /// <summary>
        /// Ignores all events until an EndIgnoreBlock Event
        /// </summary>
        StartIgnoreBlock,
        /// <summary>
        /// Ends an ignore block
        /// </summary>
        EndIgnoreBlock,
        /// <summary>
        /// Hide an event with this action
        /// </summary>
        Ignore,
        /// <summary>
        /// Highlight an event with this action. The actions should have a color if its of this type.
        /// </summary>
        Highlight,
        /// <summary>
        /// Changes color on matching text in the event message.
        /// </summary>
        HighlightMatch,
        /// <summary>
        /// Show a message box with the event message.
        /// </summary>
        PopUp,
        /// <summary>
        /// Plays a sound when there is a match.
        /// </summary>
        PlaySound,
        /// <summary>
        /// Executes whatever commandline is in the action config
        /// </summary>        
        Hide,
        /// <summary>
        /// Stores a message in a cache that can be shown later, but doesn't display it right away.
        /// </summary>
        Execute
    }

    /// <summary>
    /// An Action is what the viewer should do with the event that matches 'Pattern'
    /// </summary>
    public class Action
    {
        private static ILog _log = Logger.GetLogger(typeof(Action));

        private Regex _regex = null;
        private string _pattern;
        private ActionTypes _actionType;
        private Color _color = Color.Orange;
        private Font _font = new Font(FontFamily.GenericMonospace, 9);
        private string _data = "";

        /// <summary>
        /// This is the cache of hidden messages.
        /// </summary>
        public ArrayList HideCache = new ArrayList();

        /// <summary>
        /// Creates a new <see cref="Action"/> instance.
        /// </summary>
        protected Action(string pattern, ActionTypes actionType)
        {
            _pattern = pattern;
            _actionType = actionType;

        }

        /// <summary>
        /// Creates a ignore action.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        public static Action CreateIgnoreAction(string pattern)
        {
            return CreateGenericEventAction(pattern, ActionTypes.Ignore);
        }

        /// <summary>
        /// Creates a hide action.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        public static Action CreateHideAction(string pattern)
        {
            return CreateGenericEventAction(pattern, ActionTypes.Hide);
        }

        /// <summary>
        /// Creates a popup action.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        public static Action CreatePopupAction(string pattern)
        {
            return CreateGenericEventAction(pattern, ActionTypes.PopUp);
        }
        /// <summary>
        /// Creates the highlight action.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static Action CreateHighlightAction(string pattern, Color color)
        {
            return CreateGenericEventAction(pattern, color, ActionTypes.Highlight);
        }

        /// <summary>
        /// Creates the highlight match action.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static Action CreateHighlightMatchAction(string pattern, Color color)
        {
            return CreateGenericEventAction(pattern, color, ActionTypes.HighlightMatch);
        }

        private static Action CreateGenericEventAction(string pattern, Color color, ActionTypes type)
        {
            var a = new Action(pattern, type);
            a.Color = color;
            return a;
        }

        private static Action CreateGenericEventAction(string pattern, ActionTypes type)
        {
            var a = new Action(pattern, type);
            return a;
        }

        private static Action CreateGenericEventAction(string pattern, string data, ActionTypes type)
        {
            var a = new Action(pattern, type);
            a.Data = data;
            return a;
        }

        /// <summary>
        /// Creates the generic event action.
        /// </summary>
        /// <param name="actionNode">The action node.</param>
        /// <returns></returns>
        public static Action CreateGenericEventAction(XmlNode actionNode)
        {
            try
            {
                var type = (ActionTypes)Enum.Parse(typeof(ActionTypes), actionNode.Attributes["type"].Value);
                var pattern = actionNode.Attributes["pattern"].Value;
                var data = actionNode.Attributes.GetNamedItem("data") == null ? "" : actionNode.Attributes["data"].Value;
                
                var color = Color.FromName(actionNode.Attributes["color"].Value);
                var fontName = actionNode.Attributes["font-name"].Value;
                var fontSize = Helpers.SafeFloatParse(actionNode.Attributes["font-size"].Value);

                FontStyle fontStyle;
                fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), actionNode.Attributes["font-style"].Value);

                var a = new Action(pattern, type);
                a.Font = new Font(fontName, fontSize, fontStyle);
                a.Color = color;
                a.Data = data;
                return a;
            }
            catch (Exception x)
            {
                ExceptionManager.HandleException(0, x);
                return null;
            }
        }


        /// <summary>
        /// Gets or sets the pattern.
        /// </summary>
        /// <value></value>
        public string Pattern
        {
            get { return _pattern; }
            set { _pattern = value; }
        }

        /// <summary>
        /// Gets or sets the action type.
        /// </summary>
        /// <value></value>
        public ActionTypes ActionType
        {
            get { return _actionType; }
            set { _actionType = value; }
        }

        /// <summary>
        /// Gets or sets the fore color of this action in the viewer.
        /// </summary>
        /// <value></value>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>The font.</value>
        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

        /// <summary>
        /// Gets or sets the action data.
        /// </summary>
        /// <value>The data.</value>
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }

        internal bool IsMatch(string data)
        {
            return getRegexp().IsMatch(data);
        }

        private Regex getRegexp()
        {
            lock (this)
            {
                if (_regex == null)
                {
                    _regex = new Regex(_pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
                }

                return _regex;
            }

        }
    }
}
