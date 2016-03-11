/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using logview4net;
using System.Threading;
using logview4net.Listeners;
using logview4net.Viewers;
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
			var s = new Session();
			Assert.IsNull(s.Viewer);
			Assert.AreEqual("", s.Title);
		}

		/// <summary>
		/// Test that a newly created Session object is in an expected state
		/// </summary>
		[Test]
		public void Contructor_bTest()
		{
			IViewer viewer = new MockViewer();
			ListenerBase listenerBase = new MockListener();
			var s = new Session(listenerBase, viewer);
			
			Assert.AreEqual("", s.Title);
			Assert.AreSame(viewer, s.Viewer);
			
		}

		/// <summary>
		/// Setts the title for the session.
		/// </summary>
		[Test]
		public void TitleTest()
		{
			var s = new Session();
			var title = "Foo";
			s.Title = title;
			Assert.AreEqual(title, s.Title);
		}
		/// <summary>
		/// Try to add a viewer to the session.
		/// </summary>
		[Test]
		public void ViewerTest()
		{
			IViewer viewer = new MockViewer();
			var s = new Session();
			s.Viewer = viewer;
			
			Assert.AreSame(viewer, s.Viewer);
		}

		/// <summary>
		/// Try to add a listener.
		/// </summary>
		[Test]
		public void ListenerTest()
		{
			ListenerBase listenerBase = new MockListener();
			var s = new Session();
			
			s.AddListener(listenerBase);
			Assert.AreEqual(1, s.Listeners.Count);
		}



		/// <summary>
		/// Check that the XML from .ToXML can be used to configure a session.
		/// </summary>
		[Test]
		public void TestToXML()
		{
			ListenerBase l = new UdpListener("127.0.0.1", 8080, "");
			IViewer v = new TextViewer();
			var a = new SessionMonitor(l, v);

			var c = new ConfigureSession(a);
			
			var b = c.Session;

			var xml_a = a.ToXml().InnerXml;
			var xml_b = b.ToXml().InnerXml;

			Assert.AreEqual(xml_a, xml_b);
			
		}
		
	}
}