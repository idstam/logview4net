/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

#if DEBUG
using System;
using logview4net.Listeners ;
using logview4net.Viewers;

namespace logview4net.test
{
	/// <summary>
	/// SessionMonitor is used when testing Session. It exposes some of the protected fields in Session for monitoring purposes.
	/// </summary>
	public class SessionMonitor: logview4net.Session
	{
		/// <summary>
		/// Creates a new <see cref="SessionMonitor"/> instance.
		/// </summary>
		/// <param name="listener">Listener.</param>
		/// <param name="viewer">Viewer.</param>
		public SessionMonitor(IListener listener, IViewer viewer):base(listener, viewer)
		{
		}

		/// <summary>
		/// Creates a new <see cref="SessionMonitor"/> instance.
		/// </summary>
		/// <param name="listener">Listener.</param>
		/// <param name="viewer">Viewer.</param>
		/// <param name="logFileName">Name of the log file.</param>
		public SessionMonitor(IListener listener, IViewer viewer, string logFileName):base(listener, viewer, logFileName)
		{
		}
		
		/// <summary>
		/// Gets the name of the log file.
		/// </summary>
		/// <value></value>
		public string logFileName
		{
			get{ return _logFileName;}
		}
	}
}
#endif