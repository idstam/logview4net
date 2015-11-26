/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

#if DEBUG
using System;
using logview4net.Listeners;
using logview4net.Viewers;

namespace logview4net.test
{
	/// <summary>
	/// Summary description for EventLogListenerTestClass.
	/// </summary>
	public class EventLogListenerMonitor : EventLogListener
	{
		/// <summary>
		/// Creates a new <see cref="EventLogListenerMonitor"/> instance.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <param name="logName">Name of the log.</param>
		/// <param name="interval">Interval.</param>
		/// <param name="appendFieldNames">Append field names.</param>
		public EventLogListenerMonitor(string host, string logName, int interval, bool appendFieldNames) : base()
		{
			base._host = host;
			base._logname = logName;
			base._pollInterval = interval;
			base._appendFieldNames = appendFieldNames;
		}
		/// <summary>
		/// Sets whether or not to use events for the monitoring.
		/// </summary>
		/// <param name="foo">Foo.</param>
		public void useEvent(bool foo)
		{
			_useEvents = foo;
		}

	}
}
#endif