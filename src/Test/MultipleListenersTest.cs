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
using logview4net.Listeners;
using logview4net.Viewers;
using System.IO;
using NUnit.Framework;

namespace logview4net.test
{
	/// <summary>
	/// Summary description for MultipleListenersTest.
	/// </summary>
	[TestFixture]
	public class MultipleListenersTest
	{
		
		private MockViewer _viewer;
		private Session _session;
		private MockListener _listenerA;
		private MockListener _listenerB;

		/// <summary>
		/// Creates a new <see cref="MultipleListenersTest"/> instance.
		/// </summary>
		public MultipleListenersTest()
		{
		}


		/// <summary>
		/// Initializes the test.
		/// </summary>
		[SetUp]
		public void InitializeTest()
		{
			_session = new Session();
			_listenerA = new MockListener();
			_listenerB = new MockListener();
			_session.AddListener(_listenerA);
			_session.AddListener(_listenerB);
			_viewer = new MockViewer();
			_session.Viewer = _viewer;

		}

		/// <summary>
		/// Not implemented
		/// </summary>
		[TearDown]
		public void CleanAfterTest()
		{

		}	

		/// <summary>
		/// Checks that a session receives all messages from multiple listeners.
		/// </summary>
		[Test]
		public void ListenTest()
		{

			_session.Start();
			
			_listenerA.AddEvent("a_1");
			_listenerA.AddEvent("b_1");
			_listenerA.AddEvent("a_2");
			_listenerA.AddEvent("b_2");

				
			Assert.AreEqual(4, _viewer.ReceivedData.Count );
			

			_session.Stop();

		}

	}
}
#endif