/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

#if DEBUG
using System;
using logview4net;
using System.Threading;
using NUnit.Framework;

namespace logview4net.test
{
	/// <summary>
	/// Summary description for SessionTest.
	/// </summary>
	[TestFixture]
	public class SessionTest
	{
		/// <summary>
		/// Creates a new <see cref="SessionTest"/> instance.
		/// </summary>
		public SessionTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Test that a newly creates Session object is in an expected state
		/// </summary>
		[Test]
		public void Contructor_aTest()
		{
			Session s = new Session();
			Assert.IsNull(s.Viewer);
			Assert.AreEqual("", s.Title);
		}

		/// <summary>
		/// Test that a newly created Session object is in an expected state
		/// </summary>
		[Test]
		public void Contructor_bTest()
		{
			Viewers.IViewer viewer = new MockViewer();
			Listeners.IListener listener = new MockListener();
			Session s = new Session(listener, viewer);
			
			Assert.AreEqual("", s.Title);
			Assert.AreSame(viewer, s.Viewer);
			
		}

		/// <summary>
		/// Setts the title for the session.
		/// </summary>
		[Test]
		public void TitleTest()
		{
			Session s = new Session();
			string title = "Foo";
			s.Title = title;
			Assert.AreEqual(title, s.Title);
		}
		/// <summary>
		/// Try to add a viewer to the session.
		/// </summary>
		[Test]
		public void ViewerTest()
		{
			Viewers.IViewer viewer = new MockViewer();
			Session s = new Session();
			s.Viewer = viewer;
			
			Assert.AreSame(viewer, s.Viewer);
		}

		/// <summary>
		/// Try to add a listener.
		/// </summary>
		[Test]
		public void ListenerTest()
		{
			Listeners.IListener listener = new MockListener();
			Session s = new Session();
			
			s.AddListener(listener);
			Assert.AreEqual(1, s.Listeners.Count);
		}

		/// <summary>
		/// Check that the session creates a log file when it's told to.
		/// </summary>
		[Test]
		public void LogFileCreated()
		{
			
			Viewers.IViewer viewer = new MockViewer();
			Listeners.IListener listener = new MockListener();
			SessionMonitor s = new SessionMonitor(listener, viewer,  "foo_log.txt");
			
			s.Start();
			s.AddEvent(listener, "Some text");
			s.Stop();

			Assert.IsTrue(System.IO.File.Exists(s.logFileName));

			System.IO.File.Delete(s.logFileName);
		}

		/// <summary>
		/// Check that the session doesn't create a log file if it's not told to.
		/// </summary>
		[Test]
		public void LogFileNotCreated()
		{
			Listeners.IListener listener = new MockListener();
			Viewers.IViewer viewer = new MockViewer();

			SessionMonitor s = new SessionMonitor(listener, viewer);
			
			s.Start();
			s.AddEvent(listener, "Some text");
			s.Stop();

			Assert.IsNull(s.logFileName);
			
		}

		/// <summary>
		/// Check that the XML from .ToXML can be used to configure a session.
		/// </summary>
		[Test]
		public void TestToXML()
		{
			Listeners.IListener l = new Listeners.UdpListener("127.0.0.1", 8080, "");
			Viewers.IViewer v = new Viewers.textViewer();
			SessionMonitor a = new SessionMonitor(l, v);

			ConfigureSession c = new ConfigureSession(a);
			
			Session b = c.Session;

			string xml_a = a.ToXml().InnerXml;
			string xml_b = b.ToXml().InnerXml;

			Assert.AreEqual(xml_a, xml_b);
			
		}
		
	}
}
#endif