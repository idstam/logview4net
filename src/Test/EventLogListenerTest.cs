/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

#if DEBUG
using System;
using System.Threading;
using logview4net.Viewers;
using logview4net.Listeners;
using System.Diagnostics;
using NUnit.Framework;
using System.Text;

namespace logview4net.test
{
	/// <summary>
	/// Summary description for EventLogListenerTest.
	/// </summary>
	[TestFixture]
	public class EventLogListenerTest
	{

		string _source = "logview4netSource";
		string _logName = "logview4netLog";
		IListener _listener;
		MockViewer _viewer;
		Session _session;
		private EventLog _eventLog;

		/// <summary>
		/// Creates a new <see cref="EventLogListenerTest"/> instance.
		/// </summary>
		public EventLogListenerTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		[SetUp]
		public void Initialize()
		{		
			int interval = 300;
			
			if(!EventLog.Exists(_logName, "."))
			{
                EventLog.CreateEventSource(new EventSourceCreationData(_source, _logName));
			}
			_eventLog = new EventLog(_logName, ".");
			_eventLog.Clear();
			_viewer = new MockViewer();
			_listener = new EventLogListenerMonitor(".", _logName, interval, true);
			_session = new Session(_listener, _viewer);
		}

		/// <summary>
		/// Clean up som resources when the test is done.
		/// </summary>
		[TearDown]
		public void CleanAfterTest()
		{
				_eventLog.Clear();
				EventLog.Delete(_logName);
		}

		/// <summary>
		/// Tests the listener when not using events to monitor the event log and there is NO activity.
		/// </summary>
		[Test]
		public void ListenTestWithNoActivity()
		{
			((EventLogListenerMonitor)_listener).useEvent(false);
			int numOfEntries = 5;
			for(int i = 0; i < numOfEntries; i++)
			{
				System.Diagnostics.EventLog.WriteEntry(_source, "Test_" + i.ToString() + " ".PadRight(20, '#'));
				
			}

			_session.Start();

			Thread.Sleep(2000);	

			_session.Stop();
			
			Assert.AreEqual(numOfEntries, _eventLog.Entries.Count);
			Assert.AreEqual(0, ((MockViewer)_session.Viewer).ReceivedData.Count);
		}

		/// <summary>
		/// Tests the listener when not using events to monitor the event log and there is activity.
		/// </summary>
		[Test]
		public void ListenTestWithActivity()
		{
			((EventLogListenerMonitor)_listener).useEvent(false);
			_session.Start();
			Thread.Sleep(200);

			int numOfEntries = 5;
			for(int i = 0; i < numOfEntries; i++)
			{
				System.Diagnostics.EventLog.WriteEntry(_source, "Test_" + i.ToString() + " ".PadRight(20, '#'));
				
			}
		
			//I needed a pause for the events to bubble through the system.
			Thread.Sleep(2000);	

			_session.Stop();
			
			Assert.AreEqual(numOfEntries, _eventLog.Entries.Count);
			Assert.AreEqual(numOfEntries, ((MockViewer)_session.Viewer).ReceivedData.Count);
		}
		
		/// <summary>
		/// Tests the listener when using events to monitor the event log and there is NO activity.
		/// </summary>
		[Test]
		public void EventsTestWithNoActivity()
		{
			((EventLogListenerMonitor)_listener).useEvent(true);
			int numOfEntries = 5;
			for(int i = 0; i < numOfEntries; i++)
			{
				System.Diagnostics.EventLog.WriteEntry(_source, "Test_" + i.ToString() + " ".PadRight(20, '#'));
				
			}

			_session.Start();

			//I needed a pause for the events to bubble through the system.
			for(int i = 0; i < 10 ; i++)
			{
				Thread.Sleep(200);	
			}

			_session.Stop();
			
			Assert.AreEqual(numOfEntries, _eventLog.Entries.Count);
			Assert.AreEqual(0, ((MockViewer)_session.Viewer).ReceivedData.Count);
		}

		/// <summary>
		/// Tests the listener when using events to monitor the event log and there is activity.
		/// </summary>
		[Test]
		public void EventsTestWithActivity()
		{
			((EventLogListenerMonitor)_listener).useEvent(true);
			_session.Start();
			Thread.Sleep(200);

			int numOfEntries = 5;
			for(int i = 0; i < numOfEntries; i++)
			{
				System.Diagnostics.EventLog.WriteEntry(_source, "Test_" + i.ToString() + " ".PadRight(20, '#'));
				
			}
		
			//I needed a pause for the events to bubble through the system.
			Thread.Sleep(2000);	
			System.Windows.Forms.Application.DoEvents();

			_session.Stop();
			
			Assert.AreEqual(numOfEntries, _eventLog.Entries.Count);
			Assert.AreEqual(numOfEntries, ((MockViewer)_session.Viewer).ReceivedData.Count);
		}

        /// <summary>
        /// Serializes the listener.
        /// </summary>
        [Test]
        public void SerializeTheListener()
        {
            EventLogListener original = new EventLogListener();
            original.MessagePrefix = "MP";
            original.AppendFieldNames = true;
            original.Host = "10.0.0.1";
            original.LogName = "LN";
            original.PollInterval = 100;

            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(original.GetType());
            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            xs.Serialize(sw, original);

            System.IO.StringReader sr = new System.IO.StringReader(sb.ToString());

            EventLogListener copy = (EventLogListener)xs.Deserialize(sr);

            Assert.AreEqual(original.MessagePrefix, copy.MessagePrefix);
            Assert.AreEqual(original.AppendFieldNames, copy.AppendFieldNames);
            Assert.AreEqual(original.Host, copy.Host);
            Assert.AreEqual(original.LogName, copy.LogName);
            Assert.AreEqual(original.PollInterval, copy.PollInterval);

            original.Dispose();
            copy.Dispose();
        }
        
	}
}
#endif