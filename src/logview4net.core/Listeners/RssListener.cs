/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;

namespace logview4net.Listeners
{
    /// <summary>
    /// RssListener monitors RSS feeds
    /// </summary>
    [Serializable]
    public class RssListener : ListenerBase
    {
        private bool _isRunning = false;

        /// <summary>
        /// Name of the file being monitored.
        /// </summary>
        protected string _address;

        /// <summary>
        /// UserName for http basic authentication 
        /// </summary>
        protected string _username;

        /// <summary>
        /// Password for http basic authentication 
        /// </summary>
        protected string _password;

        /// <summary>
        /// Time, in minutes, to pause between checking the file.
        /// </summary>
        protected int _pollInterval = 45;

        /// <summary>
        /// The thread that does the actual 'listening'.
        /// </summary>
        protected Thread _listenerThread;

        /// <summary>
        /// Whether or not to load the whole file when starting the listener thread.
        /// </summary>
        protected bool _onlyTail;

        /// <summary>
        /// The <see cref="StreamReader"/> used for readig the file.
        /// </summary>
        protected StreamReader _reader;

        private static Dictionary<string, DateTime> _feedData = null;

        private static string _feedDataFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                              "logview4net.rss.txt";

        private List<string> _newEvents = new List<string>();
        private DateTime _lastPubDate = new DateTime(1970, 3, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="FileListenerBase"/> class.
        /// </summary>
        public RssListener()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "RssListener()");
            IsRestartable = true;
        }

        /// <summary>
        /// Creates a new <see cref="FileListenerBase"/> instance.
        /// </summary>
        /// <param name="address">URL for the feed to monitor.</param>
        /// <param name="pollInterval">Time, in milliseconds, to pause between checking the file.</param>
        /// <param name="messagePrefix">A string that will preceed this listeners messages in the viewer.</param>
        /// <param name="onlyTail">Whether or not to load the whole file when starting the listener thread.</param>
        public RssListener(string address, int pollInterval, string messagePrefix, bool onlyTail)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "RssListener(string, int, string, bool)");

            _address = address;
            _pollInterval = pollInterval;
            MessagePrefix = messagePrefix;
            _onlyTail = onlyTail;
            IsRestartable = true;
        }

        private DateTime getLastFeedCheck(string url)
        {
            if (_feedData == null)
            {
                loadFeedData();
            }

            if (!_feedData.ContainsKey(url))
            {
                _feedData.Add(url, DateTime.MinValue);
            }

            return _feedData[url];
        }

        private void loadFeedData()
        {
            _feedData = new Dictionary<string, DateTime>();

            if (File.Exists(_feedDataFile))
            {
                var feeds = File.ReadAllLines(_feedDataFile);
                foreach (var f in feeds)
                {
                    _feedData.Add(f.Split('|')[0], DateTime.Parse(f.Split('|')[1]));
                }
            }
        }

        /// <summary>
        /// Gets or sets the URL of the feed
        /// </summary>
        /// <value></value>
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// Gets or sets the poll interval.
        /// </summary>
        /// <value></value>
        public int PollInterval
        {
            get { return _pollInterval; }
            set { _pollInterval = value; }
        }


        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [only tail].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [only tail]; otherwise, <c>false</c>.
        /// </value>
        public bool OnlyTail
        {
            get { return _onlyTail; }
            set { _onlyTail = value; }
        }

        /// <summary>
        /// This listeners working method. Does the tail checking of the feed.
        /// </summary>
        protected void tail()
        {
            if (_onlyTail)
            {
                _lastPubDate = DateTime.Now;
            }
            else
            {
                _lastPubDate = getLastFeedCheck(_address);
            }

            var wc = InitializeWebClient();

            if (_pollInterval < 1)
            {
                _pollInterval = 1;
            }

            try
            {
                while (_isRunning)
                {
                    var x = new XmlDocument();
                    var checkDate = DateTime.Now;
                    #region Some debug logging
                    _log.Debug(GetHashCode(), "Checking feed: " + _address);
                    _log.Debug(GetHashCode(), "Check time: " + checkDate.ToShortDateString() + ":" + checkDate.ToShortTimeString());
                    _log.Debug(GetHashCode(), "_lastPubDate: " + _lastPubDate.ToShortDateString() + ":" + _lastPubDate.ToShortTimeString());
                    #endregion
                    //x.LoadXml(wc.DownloadString(_address));
                    x.Load(_address);
                    SelectAndRunFeedManager(x);

                    _lastPubDate = checkDate;
                    Thread.Sleep(_pollInterval * 1000 * 60);
                }
            }
            catch (Exception ex)
            {
                Session.AddEvent(this, ex.Message);
            }
        }

        private WebClient InitializeWebClient()
        {
            //Creating an instance of a credential cache, 
            //and passing the username and password to it
            var credCache = new CredentialCache();
            credCache.Add(new Uri(_address), "Basic", new NetworkCredential(_username, _password));
            var wc = new WebClient();
            wc.Credentials = credCache;
            return wc;
        }

        private void SelectAndRunFeedManager(XmlDocument x)
        {
            var channelNodes = x.SelectNodes("//channel");
            if (channelNodes.Count > 0) //This looks like an RSS-feed
            {
                CheckRssFeed(channelNodes);
            }
            else //Try Atom instead
            {
                CheckAtomFeed(x);
            }
        }

        private void CheckAtomFeed(XmlDocument x)
        {
            XmlNode c = x.DocumentElement;
            var nsmgr = new XmlNamespaceManager(x.NameTable);
            var nameSpaceURI = c.Attributes["xmlns"].Value;
            nsmgr.AddNamespace("foo", nameSpaceURI);

            //Check if 'modified' is later than last time we checked this feed
            //if not skip the loop
            var modified = DateTime.Parse(c.SelectSingleNode("./foo:modified", nsmgr).InnerText);
            if (modified >= _lastPubDate)
            {
                addEvent("Feed title: " + c.SelectSingleNode("./foo:title", nsmgr).InnerText);

                var items = c.SelectNodes("./foo:entry", nsmgr);
                foreach (XmlNode i in items)
                {
                    modified = DateTime.Parse(i.SelectSingleNode("./foo:modified", nsmgr).InnerText);
                    //Check if 'modified' is later than last time we checked this feed
                    if (modified >= _lastPubDate)
                    {
                        addEvent("Title: " + i.SelectSingleNode("./foo:title", nsmgr).InnerText);
                        var description =
                            wordWrap(Regex.Replace(i.SelectSingleNode("./foo:content", nsmgr).InnerText, "<[^>]*>", " "));
                        description += Environment.NewLine + "-----------------------------------";
                        addEvent(description);
                    }


                    flushEvents();
                }
            }
        }

        private void CheckRssFeed(XmlNodeList channelNodes)
        {
            //Check if pubDate is later than last time we checked this feed
            DateTime modified;
            string title;
            //if not skip the loop
            foreach (XmlNode c in channelNodes)
            {
                addEvent("Feed title: " + c.SelectSingleNode("./title").InnerText);
                var items = c.SelectNodes("./item");
                for (var i_pos = items.Count - 1; i_pos >= 0; i_pos--)
                {
                    var i = items[i_pos];
                    title = i.SelectSingleNode("./title").InnerText;
                    //Check if pubDate is later than last time we checked this feed
                    modified = DateTime.Parse(i.SelectSingleNode("./pubDate").InnerText);
                    _log.Debug(GetHashCode(), "Checking item " + title + " modified: " + modified.ToShortDateString() + ":" +
                               modified.ToShortTimeString());
                    if (modified >= _lastPubDate)
                    {
                        //if it is then make this the new latest pubDate
                        //else skip the article

                        addEvent("Title: " + title + " modified: " + modified.ToShortDateString() + ":" +
                                 modified.ToShortTimeString());
                        //string description = wordWrap(Regex.Replace(i.SelectSingleNode("./description").InnerText, "<[^>]*>", " "));
                        var description = wordWrap(i.SelectSingleNode("./description").InnerText);
                        description += Environment.NewLine + "-----------------------------------";
                        addEvent(description);

                        flushEvents();
                    }
                }
            }
        }

        private void addEvent(string text)
        {
            _newEvents.Add(text);
        }

        private void flushEvents()
        {
            lock (Session)
            {
                foreach (var s in _newEvents)
                {
                    Session.AddEvent(this, s);
                }
                _newEvents.Clear();
            }
        }

        private string wordWrap(string text)
        {
            return HttpUtility.HtmlDecode(text);
        }

        #region IListener Members

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void Stop()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Stop");

            if (_listenerThread != null)
            {
                if (_reader != null)
                {
                    _reader.Close();
                }
                _listenerThread.Abort();

                _log.Info(GetHashCode(), "Ended polling log entries for " + _address);
            }

            _isRunning = false;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public override void Start()
        {
            _log.Info(GetHashCode(), "Started polling log entries in " + _address + " using prefix: " + MessagePrefix);


            var ts = new ThreadStart(tail);
            _listenerThread = new Thread(ts);
            //_listenerThread.Priority = ThreadPriority.BelowNormal;
            _listenerThread.Start();

            _isRunning = true;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Dispose");
            if (_isRunning)
            {
                Stop();
            }
        }

        #endregion


        /// <summary>
        /// Gets the config value fields.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, ListenerConfigField> GetConfigValueFields()
        {
            var ret = new Dictionary<string, ListenerConfigField>();

            var f = new ListenerConfigField();
            f.Name = "Prefix";
            f.MultiValueType = MultiValueTypes.None;
            ret.Add("prefix", f);

            f = new ListenerConfigField();
            f.Name = "Poll intervall (m)";
            f.MultiValueType = MultiValueTypes.None;
            ret.Add("poll_intervall", f);

            f = new ListenerConfigField();
            f.Name = "Start at end";
            f.MultiValueType = MultiValueTypes.Check;
            ret.Add("only_tail", f);

            f = new ListenerConfigField();
            f.Name = "BREAK";
            f.MultiValueType = MultiValueTypes.Linebreak;
            ret.Add("break1", f);

            f = new ListenerConfigField();
            f.Name = "Url";
            f.MultiValueType = MultiValueTypes.None;
            f.Width = 45;
            f.AlignTo = "prefix";
            ret.Add("url", f);

            f = new ListenerConfigField();
            f.Name = "BREAK";
            f.MultiValueType = MultiValueTypes.Linebreak;
            ret.Add("break2", f);

            f = new ListenerConfigField();
            f.Name = "User";
            f.MultiValueType = MultiValueTypes.None;
            f.Width = 32;
            f.AlignTo = "prefix";
            ret.Add("username", f);

            f = new ListenerConfigField();
            f.Name = "Password";
            f.MultiValueType = MultiValueTypes.Password;
            f.Width = 16;
            ret.Add("password", f);

            return ret;
        }
        /// <summary>
        /// Gets the config value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public override string GetConfigValue(string name)
        {
            switch (name)
            {
                case "prefix":
                    return MessagePrefix;
                case "poll_intervall":
                    return _pollInterval.ToString();
                case "only_tail":
                    return _onlyTail.ToString();
                case "url":
                    return _address;
                case "username":
                    return _username;
                case "password":
                    return _password;
                default:
                    throw new NotImplementedException("This listener has no field named: " + name);
            }
        }
        /// <summary>
        /// Sets the config value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override string SetConfigValue(string name, string value)
        {
            string ret = null;
            switch (name)
            {
                case "prefix":
                    MessagePrefix = value;
                    break;
                case "poll_intervall":
                    _pollInterval = ListenerHelper.GetSafeInt(value);
                    break;
                case "only_tail":
                    if (!bool.TryParse(value, out _onlyTail))
                    {
                        ret = "Please enter [true] or [false] here.";
                    }
                    break;
                case "url":
                    _address = value;
                    break;
                case "password":
                    _password = value;
                    break;
                case "username":
                    _username = value;
                    break;
                default:
                    throw new NotImplementedException("This listener has no field named: " + name);
            }

            return ret;
        }
    }
}
