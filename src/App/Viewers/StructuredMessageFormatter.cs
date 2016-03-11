using logview4net.Listeners;
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
        public StructuredMessageFormatter(string message, ListenerBase listener)
        {
            _message = message;
            _format = listener.IsStructured ? listener.GetConfigValue("structured") : "n/a";
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
            int curlyCount = 0;
            bool inString = false;
            var ret = new StringBuilder();
            foreach(var c in _message)
            {
                if (!inString)
                {
                    if (c == '{')
                    {
                        curlyCount++;
                        ret.AppendLine();
                        ret.AppendLine("{");
                        ret.Append(new string('\t', curlyCount));
                        continue;
                    }
                    if (c == '}')
                    {
                        curlyCount--;
                        ret.AppendLine();
                        ret.Append(new string('\t', curlyCount));
                        ret.Append("}");
                        ret.AppendLine();
                        ret.Append(new string('\t', curlyCount));
                        
                        continue;
                    }
                    if (c == ',')
                    {
                        ret.Append(",");
                        ret.AppendLine();
                        ret.Append(new string('\t', curlyCount));
                        continue;
                    }
                    ret.Append(c);

                }
                else
                {
                    ret.Append(c);
                }
                if (c == '"')
                {
                    inString = !inString;
                }

            }

            return ret.ToString();
        }
    }
}
