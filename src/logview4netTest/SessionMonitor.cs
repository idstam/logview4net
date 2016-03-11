/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using logview4net.Listeners ;
using logview4net.Viewers;

namespace logview4net.test
{
	/// <summary>
	/// SessionMonitor is used when testing Session. It exposes some of the protected fields in Session for monitoring purposes.
	/// </summary>
	public class SessionMonitor: Session
	{
		/// <summary>
		/// Creates a new <see cref="SessionMonitor"/> instance.
		/// </summary>
		/// <param name="listenerBase">Listener.</param>
		/// <param name="viewer">Viewer.</param>
		public SessionMonitor(ListenerBase listenerBase, IViewer viewer):base(listenerBase, viewer)
		{
		}

		
	}
}