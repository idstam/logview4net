/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using logview4net.Listeners;
using System.Diagnostics;
using NUnit.Framework;
using System.Text;
using NUnit.Framework.Legacy;

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
		ListenerBase _listenerBase;
		MockViewer _viewer;
		Session _session;
		private EventLog _eventLog;
		

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		[SetUp]
		public void Initialize()
		{		
			var interval = 300;
			
			if(!EventLog.Exists(_logName, "."))
			{
                EventLog.CreateEventSource(new EventSourceCreationData(_source, _logName));
			}
			_eventLog = new EventLog(_logName, ".");
			_eventLog.Clear();
			_viewer = new MockViewer();
			_listenerBase = new EventLogListenerMonitor(".", _logName, interval, true);
			_session = new Session(_listenerBase, _viewer);
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
			((EventLogListenerMonitor)_listenerBase).useEvent(false);
			var numOfEntries = 5;
			for(var i = 0; i < numOfEntries; i++)
			{
				EventLog.WriteEntry(_source, "Test_" + i.ToString() + " ".PadRight(20, '#'));
				
			}

			_session.Start();

			Thread.Sleep(2000);	

			_session.Stop();
			
			Assert.That(numOfEntries,Is.EqualTo( _eventLog.Entries.Count));
			Assert.That(0,Is.EqualTo(((MockViewer)_session.Viewer).ReceivedData.Count));
		}

		/// <summary>
		/// Tests the listener when not using events to monitor the event log and there is activity.
		/// </summary>
		[Test]
		public void ListenTestWithActivity()
		{
			((EventLogListenerMonitor)_listenerBase).useEvent(false);

			_session.Start();
			Thread.Sleep(200);

			var numOfEntries = 5;
			for(var i = 0; i < numOfEntries; i++)
			{
				EventLog.WriteEntry(_source, "Test_" + i.ToString() + " ".PadRight(20, '#'));
				
			}
		
			//I needed a pause for the events to bubble through the system.
			Thread.Sleep(8000);	

			_session.Stop();

            ClassicAssert.AreEqual(numOfEntries, _eventLog.Entries.Count);
            ClassicAssert.AreEqual(numOfEntries, ((MockViewer)_session.Viewer).ReceivedData.Count);
		}
		
		/// <summary>
		/// Tests the listener when using events to monitor the event log and there is NO activity.
		/// </summary>
		[Test]
		public void EventsTestWithNoActivity()
		{
			((EventLogListenerMonitor)_listenerBase).useEvent(true);
			var numOfEntries = 5;
			for(var i = 0; i < numOfEntries; i++)
			{
				EventLog.WriteEntry(_source, "Test_" + i.ToString() + " ".PadRight(20, '#'));
				
			}

			_session.Start();

			//I needed a pause for the events to bubble through the system.
			for(var i = 0; i < 10 ; i++)
			{
				Thread.Sleep(200);	
			}

			_session.Stop();

            ClassicAssert.AreEqual(numOfEntries, _eventLog.Entries.Count);
            ClassicAssert.AreEqual(0, ((MockViewer)_session.Viewer).ReceivedData.Count);
		}

		/// <summary>
		/// Tests the listener when using events to monitor the event log and there is activity.
		/// </summary>
		[Test]
		public void EventsTestWithActivity()
		{
			((EventLogListenerMonitor)_listenerBase).useEvent(true);
			_session.Start();
			Thread.Sleep(200);

			var numOfEntries = 5;
			for(var i = 0; i < numOfEntries; i++)
			{
				EventLog.WriteEntry(_source, "Test_" + i.ToString() + " ".PadRight(20, '#'));
				
			}
		
			//I needed a pause for the events to bubble through the system.
			Thread.Sleep(5000);	
			Application.DoEvents();

			_session.Stop();

            ClassicAssert.AreEqual(numOfEntries, _eventLog.Entries.Count);
            ClassicAssert.AreEqual(numOfEntries, ((MockViewer)_session.Viewer).ReceivedData.Count);
		}

        /// <summary>
        /// Serializes the listener.
        /// </summary>
        [Test]
        public void SerializeTheListener()
        {
            var original = new EventLogListener();
            original.MessagePrefix = "MP";
            original.AppendFieldNames = true;
            original.Host = "10.0.0.1";
            original.LogName = "LN";
            original.PollInterval = 100;

            var xs = new XmlSerializer(original.GetType());
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            xs.Serialize(sw, original);

            var sr = new StringReader(sb.ToString());

            var copy = (EventLogListener)xs.Deserialize(sr);

            ClassicAssert.AreEqual(original.MessagePrefix, copy.MessagePrefix);
            ClassicAssert.AreEqual(original.AppendFieldNames, copy.AppendFieldNames);
            ClassicAssert.AreEqual(original.Host, copy.Host);
            ClassicAssert.AreEqual(original.LogName, copy.LogName);
            ClassicAssert.AreEqual(original.PollInterval, copy.PollInterval);

            original.Dispose();
            copy.Dispose();
        }
        
	}
}