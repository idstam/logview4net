using logview4net.Listeners;
using System;
using System.Collections.Generic;
using System.Text;

namespace logview4net.Viewers
{
    /// <summary>
    /// Formats a structured message in to a more readable form.
    /// </summary>
    public class StructuredMessageFormatter
    {
        private string _message;
        private string _format;

        /// <summary>
        /// Creates a message formatter.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="listener"></param>
        public StructuredMessageFormatter(string message, IConfigurableListener listener)
        {
            _message = message;
            _format = listener.IsStructured ? listener.GetConfigValue("structure") : "n/a";
        }

        /// <summary>
        /// The formatted message if the listener is a structured listener.
        /// </summary>
        /// <returns></returns>
        public string Message()
        {
            switch (_format.ToLowerInvariant())
            {
                case "json":
                    return asJson();
                default:
                    return _message; 
            }
        }

        private string asJson()
        {
            throw new NotImplementedException();
        }
    }
}
