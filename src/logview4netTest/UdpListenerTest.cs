/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using logview4net.Listeners;
using logview4net.Viewers;
using NUnit.Framework;
using System.Text;

namespace logview4net.test
{
	/// <summary>
	/// Summary description for UdpListenerTest.
	/// </summary>
	[TestFixture]
	public class UdpListenerTest
	{
		private string _testEndpoint = "127.0.0.1";
		private int _testPort = 9999;
		private MockViewer _viewer;
		private Session _session;
		private UdpListener _listener;

		/// <summary>
		/// Creates a new <see cref="UdpListenerTest"/> instance.
		/// </summary>
		public UdpListenerTest()
		{
		}


		/// <summary>
		/// Initializes the test.
		/// </summary>
		[SetUp]
		public void InitializeTest()
		{
			_viewer = new MockViewer();
			_listener = new UdpListener(_testEndpoint, _testPort, "");
			_session = new Session(_listener, _viewer);

		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		[TearDown]
		public void CleanAfterTest()
		{
            _session.Stop();
		}	

		/// <summary>
		/// Tests if the <see cref="UdpListenerBase"/> receives the data sent.
		/// </summary>
		[Test]
		public void ListenTest()
		{
					
			
			_session.Start();
			//Thread.Sleep(1000);
		    _viewer.ShowListenerPrefix = true;

			var messages = new string[] {"johan", "testar"};
			var us = new UdpSender(_testEndpoint, _testPort,  messages, 200);
			us.Start();
			
			Thread.Sleep(500); //To let the message go through the network stack
	
			Assert.AreEqual(2, _viewer.ReceivedData.Count );

			for(var i = 0; i < messages.Length; i++)
			{
				Assert.AreEqual(" - 127.0.0.1 " + messages[i], _viewer.ReceivedData[i].ToString(), "Messages diff at no: " + i.ToString());
			}
			

			_session.Stop();
		}

        /// <summary>
        /// Serializes the listener.
        /// </summary>
        [Test]
        public void SerializeTheListener()
        {
            var original = new UdpListener("10.0.0.1", 888, "MP");
            original.MessagePrefix = "MP";
            var xs = new XmlSerializer(original.GetType());
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            xs.Serialize(sw, original);

            var sr = new StringReader(sb.ToString());

            var copy = (UdpListener)xs.Deserialize(sr);

            Assert.AreEqual(original.Endpoint, copy.Endpoint);
            Assert.AreEqual(original.MessagePrefix, copy.MessagePrefix);
            Assert.AreEqual(original.Port, copy.Port);

            original.Dispose();
            copy.Dispose();
        }

	}
}