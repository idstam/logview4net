/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Threading;
using logview4net.Listeners;
using logview4net.Viewers;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
		private MockListener _listenerBaseA;
		private MockListener _listenerBaseB;

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
			_listenerBaseA = new MockListener();
			_listenerBaseB = new MockListener();
			_session.AddListener(_listenerBaseA);
			_session.AddListener(_listenerBaseB);
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
			
			_listenerBaseA.AddEvent("a_1");
			_listenerBaseA.AddEvent("b_1");
			_listenerBaseA.AddEvent("a_2");
			_listenerBaseA.AddEvent("b_2");


            ClassicAssert.AreEqual(4, _viewer.ReceivedData.Count );
			

			_session.Stop();

		}

	}
}