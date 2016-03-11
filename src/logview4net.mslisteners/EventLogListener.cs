/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Security;
using System.Collections.Generic;

namespace logview4net.Listeners
{
    /// <summary>
    /// Montors an event log on any reachable machine.
    /// </summary>
    public class EventLogListener : ListenerBase
    {
        /// <summary>
        /// This is the thread that does the actual 'listening'.
        /// </summary>
        protected Thread _listenerThread = null;

        /// <summary>
        /// The host name/ip of the machine hosting the event log that is to be monitored.
        /// </summary>
        protected string _host;

        /// <summary>
        /// The name of the event log to monitor.
        /// </summary>
        protected string _logname;

        /// <summary>
        /// The <see cref="Session"/> the contains this listener.
        /// </summary>
        protected Session _session = null;

        /// <summary>
        /// The <see ref="System.Diagnostics.EventLog"/> that is used if we monitor the event log using events.
        /// </summary>
        protected EventLog _eventLog;

        /// <summary>
        /// Copy of the last found log entry so that we can find the newer ones.
        /// </summary>
        protected EventLogEntry _lastEntry = null;

        /// <summary>
        /// The interval in milliseconds to sleep between the loopups in the event log.
        /// </summary>
        protected int _pollInterval;

        /// <summary>
        /// Whether or not to use events. Events only work on localhos.
        /// </summary>
        protected bool _useEvents = false;


        /// <summary>
        /// Whether or ot to preceed all data fields with their names when adding an event to the viewer.
        /// </summary>
        protected bool _appendFieldNames = true;

        /// <summary>
        /// Creates a new <see cref="EventLogListenerBase"/> instance.
        /// </summary>
        public EventLogListener()
        {
            _log = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            IsRestartable = true;
        }

        private EventLog CreateEventLog()
        {
            //_useEvents = (_host == "127.0.0.1" || _host == "localhost");				
            _log.Debug(GetHashCode(), "Creating new EventLog eventlog: " + _logname + " on " + _host);

            return new EventLog(_logname, _host);
        }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value></value>
        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        /// <summary>
        /// Gets or sets the name of the log.
        /// </summary>
        /// <value></value>
        public string LogName
        {
            get { return _logname; }
            set 
            {
                _log.Debug(GetHashCode(), "Setting Log Name to " + value);
                _logname = value; 
            }
        }

        /// <summary>
        /// Gets or sets the poll interval.
        /// </summary>
        /// <value></value>
        public int PollInterval
        {
            get { return _pollInterval; }
            set
            {
                _log.Debug(GetHashCode(), "Setting poll intervall to " + value.ToString());
                _pollInterval = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to append field names.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [append field names]; otherwise, <c>false</c>.
        /// </value>
        public bool AppendFieldNames
        {
            get { return _appendFieldNames; }
            set { _appendFieldNames = value; }
        }


        /// <summary>
        /// This is where the actual monitoring of the event log takes place if we have choosen not to use events.
        /// </summary>
        protected void Listen()
        {
            _log.Debug(GetHashCode(), "Listen");

            if (!ValidatePermissionsAndInit())
            {
                return;
            }
            
            //Loop for ever. The thread running this method will be aborted when we don't want to monitor
            //the event log any more.
            do
            {
                try
                {
                    if(_eventLog == null)
                    {
                        _eventLog = new EventLog(_logname, _host);
                    }

                    if (_eventLog.Entries.Count > 0)
                    {
                        var thisLastEntry = _eventLog.Entries[_eventLog.Entries.Count - 1];

                        if (_lastEntry == null || _lastEntry.TimeGenerated != thisLastEntry.TimeGenerated)
                        {                           
                            //Loop through all entries until we find 'currentEntry' 
                            //Show them all and store the last one as 'currentEntry'
                            var newEntries = new Stack();
                            for (var i = _eventLog.Entries.Count - 1; i >= 0; i--)
                            {
                                var currentEntry = _eventLog.Entries[i];
                                if (_lastEntry == null || _lastEntry.Message != currentEntry.Message)
                                {
                                    _log.Debug(GetHashCode(), "Push " + i.ToString());
                                    newEntries.Push(currentEntry);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            var events = new List<string>();

                            while (newEntries.Count > 0)
                            {
                                _log.Debug(GetHashCode(), "Pop " + newEntries.Count.ToString());
                                events.Add(getEventString((EventLogEntry)newEntries.Pop()));
                            }
                            _session.AddEvent(this, events);
                            _lastEntry = thisLastEntry;
                            
                        }
                        else
                        {
                            _log.Info(GetHashCode(), "There are no new events in " + _logname + " on " + _host);
                        }
                    }
                    else
                    {
                        _log.Info(GetHashCode(), "There are no events at all in " + _logname + " on " + _host);
                    }

                    //I have to clear the event log object and re-create it in the beginning of
                    //the loop. Otherwise I don't get any new data from the underlying event log.
                    _eventLog.Dispose();
                    _eventLog = null;

                    Thread.Sleep(_pollInterval);
                }
                catch (SecurityException x)
                {
                    _log.Warn(GetHashCode(),"A Security Exception was thrown when reading " + _logname + " on " + _host,  x);
                    _session.AddEvent(this, "A Security Exception was thrown when reading " + _logname + " on " + _host +
                        Environment.NewLine + "You might not have the necessary permissions.");

                    return;
                }
                catch (Exception x)
                {
                    _log.Error(GetHashCode(), "EventLogListener loop error when reading " + _logname + " on " + _host, x);
                }
                finally
                {
                    if(_eventLog != null)
                    {
                        _eventLog.Dispose();
                        _eventLog = null;
                    }
                }
            } while (true);
        }

        private bool ValidatePermissionsAndInit()
        {
            try
            {

                //If there are events in the log; store some unique id from the last event
                if (_eventLog.Entries.Count > 0)
                {
                    _lastEntry = _eventLog.Entries[_eventLog.Entries.Count - 1];
                }
                return true;
            }
            catch (SecurityException x)
            {
                _log.Warn(GetHashCode(), "A Security Exception was thrown when reading " + _logname + " on " + _host, x);
                _session.AddEvent(this, "A Security Exception was thrown when opening " + _logname + " on " + _host +
                    Environment.NewLine + "You might not have the necessary permissions.");
                
                return false;
            }
        }

        public override Dictionary<string, ListenerConfigField> GetConfigValueFields()
        {
            return new Dictionary<string, ListenerConfigField>();
        }

        public override string SetConfigValue(string name, string value)
        {
            return null;
        }

        public override string GetConfigValue(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ends the monitoring of the event log.
        /// </summary>
        public override void Stop()
        {
            _log.Debug(GetHashCode(), "Stop");

            if (_useEvents)
            {
                _eventLog.EnableRaisingEvents = false;
            }
            else
            {
                try
                {
                    if (_listenerThread != null) _listenerThread.Abort();
                }
                catch (Exception ex)
                {
                    _log.Debug(GetHashCode(), "Failed to stop event log listener thread.", ex);
                }
            }
            IsRunning = false;
            _log.Info(GetHashCode(), "Ended receiving/polling event entries for " + _logname + " on " + _host);
        }

        /// <summary>
        /// Starts monitoring of the event log.
        /// </summary>
        public override void Start()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Start");

            if (_eventLog == null) _eventLog = CreateEventLog();

            _useEvents = (_host == "127.0.0.1" || _host == "localhost" || _host == ".");
            if (_useEvents)
            {
                _log.Info(GetHashCode(), "Started receiving event entries for " + _logname + " on " + _host + " using prefix: " +
                          MessagePrefix);
                _eventLog.EntryWritten += new EntryWrittenEventHandler(_eventLog_EntryWritten);
                _eventLog.EnableRaisingEvents = true;
            }
            else
            {
                _log.Info(GetHashCode(), "Started polling event entries for " + _logname + " on " + _host + " using prefix: " +
                          MessagePrefix);
                var ts = new ThreadStart(Listen);
                _listenerThread = new Thread(ts);
                _listenerThread.Priority = ThreadPriority.BelowNormal;
                _listenerThread.Start();
            }

            IsRunning = true;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Dispose");
            Stop();
        }

        /// <summary>
        /// This is the event handler thas is run when we use events to monitor a local event log
        /// </summary>
        /// <param name="sender">The <see cref="EventLog" /> that initiated the event. </param>
        /// <param name="e"> <see cret="EntryWrittenEventArgs" /> </param>
        private void _eventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            _session.AddEvent(this, getEventString(e.Entry));
        }

        private string getEventString(EventLogEntry entry)
        {
            string loggingEvent;
            if (_appendFieldNames)
            {
                loggingEvent = "Message: " + entry.Message;
                loggingEvent += "\tCategory: " + entry.Category;
                loggingEvent += "\tEntryType: " + entry.EntryType.ToString();
                loggingEvent += "\tEventID: " + entry.InstanceId.ToString();
                loggingEvent += "\tSource: " + entry.Source;
                loggingEvent += "\tTimeWritten: " + entry.TimeWritten.ToLongTimeString();
                loggingEvent += "\tUserName: " + entry.UserName;
            }
            else
            {
                loggingEvent = entry.Message;
                loggingEvent += "\t" + entry.Category;
                loggingEvent += "\t" + entry.EntryType.ToString();
                loggingEvent += "\t" + entry.InstanceId.ToString();
                loggingEvent += "\t" + entry.Source;
                loggingEvent += "\t" + entry.TimeWritten.ToLongTimeString();
                loggingEvent += "\t" + entry.UserName;
            }

            return loggingEvent;
            
        }

        #region IListener Members

        /// <summary>
        /// Gets a new configurator.
        /// </summary>
        /// <returns></returns>
        public IListenerConfigurator GetNewConfigurator()
        {
            return new EventLogListenerConfigurator(this);
        }

        /// <summary>
        /// True if this listener has no historic data.
        /// </summary>
        /// <returns></returns>
        public bool OnlyTail
        {
            get { return true; }
            set { }
        }
        #endregion
    }
}