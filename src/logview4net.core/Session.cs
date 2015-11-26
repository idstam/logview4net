/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

using logview4net.Listeners;
using logview4net.Viewers;

namespace logview4net
{
	/// <summary>
	/// This is the container for listeners (<see cref="IListener"/>) and viewers (<see cref="IViewer"/>).
	/// </summary>
	/// <remarks>
	/// A session can have a single viewer, multiple listeners and is represented by a tab in the current GUI implementation.
	/// </remarks>
	public class Session
	{
		/// <summary>
		/// The internal list of <see cref="logview4net.Listeners.IListener"/>
		/// </summary>
		private List<IListener> _listeners = new List<IListener>();


		private IViewer _viewer;
		private string _title = "";
		private bool _isRunning = false;
		public string Hash = Guid.NewGuid().ToString();
		/// <summary>
		/// Gets or sets a value indicating whether this instance is running.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is running; otherwise, <c>false</c>.
		/// </value>
		public bool IsRunning
		{
			get { return _isRunning; }
			set { _isRunning = value; }
		}


		/// <summary>
		/// Gets or sets the listeners.
		/// </summary>
		/// <value>The listeners.</value>
		public List<IListener> Listeners
		{
			get { return _listeners; }
			set { _listeners = value; }
		}

		/// <summary>
		/// The filename used for saving all events received by a session.
		/// </summary>
		
		public RollingFileAppender _rollingStorage = null; //Public to enable testing

		private ILog _log = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Creates a new <see cref="Session"/> instance.
		/// </summary>
		
		public Session()
		{
		}

		/// <summary>
		/// Creates a new <see cref="Session"/> instance.
		/// </summary>
		/// <param name="listener">A listener to be used by the session.</param>
		/// <param name="viewer">The viewer to be used by the session.</param>
		public Session(IListener listener, IViewer viewer)
		{
			_listeners.Add(listener);
			_viewer = viewer;
		}

		/// <summary>
		/// Clears the list of listeners.
		/// </summary>
		public void ClearListeners()
		{
			foreach (var l in _listeners)
			{
				l.Dispose();
			}
			_listeners = new List<IListener>();
		}

		/// <summary>
		/// Removes the specific listener from the listener list.
		/// </summary>
		/// <param name="listener">Listener.</param>
		public void RemoveListener(IListener listener)
		{
			IListener foo = null;
			foreach (var l in _listeners)
			{
				if (l.Hash == listener.Hash) foo = l;
			}

			_listeners.Remove(foo);

			if (listener != null)
			{
				listener.Dispose();
			}
		}

		/// <summary>
		/// Adds a listener to this session..
		/// </summary>
		/// <param name="listener">Listener.</param>
		public void AddListener(IListener listener)
		{
			listener.Session = this;
			_listeners.Add(listener);
		}

		/// <summary>
		/// Gets or sets the viewer to be used by the session.
		/// </summary>
		/// <value></value>
		public IViewer Viewer
		{
			get { return _viewer; }
			set { _viewer = value; }
		}


		private string getListenerPrefix(IListener sender)
		{
			var prefix = "";
			if (_viewer.ShowListenerPrefix)
			{
				prefix = sender.MessagePrefix + " - ";
			}
			return prefix;
		}
		/// <summary>
		/// Adds an event to the session. Will be handled by the session viewer.
		/// </summary>
		/// <param name="sender">The listener that 'found' the event.</param>
		/// <param name="data">The text to display.</param>
		public virtual void AddEvent(IListener sender, string data)
		{
			if (_log.Enabled) _log.Debug(GetHashCode(), "AddEvent(Ilistener, string)");
			WriteToLogFile(data);
			
			try
			{
				if (sender.ShowTimestamp)
				{
					_viewer.AddEvent(getListenerPrefix(sender) + DateTime.Now.ToString(sender.TimestampFormat) + " - " + data);
				}
				else
				{
					_viewer.AddEvent(getListenerPrefix(sender) + data);
				}
			}
			catch (Exception ex)
			{
				ExceptionManager.HandleException(GetHashCode(), ex);
			}
		}

		/// <summary>
		/// Adds an event to the session. Will be handled by the session viewer.
		/// </summary>
		/// <param name="listener">The listener that 'found' the event.</param>
		/// <param name="lines">The text to display.</param>
		public virtual void AddEvent(IListener listener, List<string> lines)
		{
			
			try
			{
				
				if (listener.ShowTimestamp)
				{
					var lst = new List<string>();
					foreach (var line in lines)
					{
						WriteToLogFile(line);
						lst.Add(DateTime.Now.ToString(listener.TimestampFormat) + " - " + line);
					}
					_viewer.AddEvent(getListenerPrefix(listener), lst);
				}
				else
				{
					foreach (var line in lines)
					{
						WriteToLogFile(line);
					}

					_viewer.AddEvent(getListenerPrefix(listener), lines);
				}

				
			}
			catch (Exception ex)
			{
				throw ExceptionManager.HandleException(GetHashCode(), ex);
			}
		}

		/// <summary>
		/// Gets or sets the title of this session.
		/// </summary>
		/// <value></value>
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		public void Start()
		{
			foreach (var listener in _listeners)
			{
				if (! listener.IsRunning)
				{
					listener.Session = this;
					listener.Start();
				}
			}

			IsRunning = true;
		}
		


		/// <summary>
		/// Stops this instance.
		/// </summary>
		public void Stop()
		{
			foreach (var listener in _listeners)
			{
				listener.Stop();
				listener.Dispose();
			}
			_listeners.Clear();
			
			IsRunning = false;
		}

		/// <summary>
		/// Returns this sessions configuration data.
		/// </summary>
		/// <returns></returns>
		public XmlDocument ToXml()
		{
			//Create Session (root) node
			var configuration = new XmlDocument();
			var pi =
				configuration.CreateProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\" ");
			configuration.AppendChild(pi);

			var root = configuration.CreateElement("session");
			configuration.AppendChild(root);
			var attrib = configuration.CreateAttribute("title");
			attrib.Value = _title;
			root.Attributes.Append(attrib);

			//Append listener nodes
			foreach (var lc in _listeners)
			{
				var listener = configuration.CreateElement("listener");
				listener.InnerXml = lc.GetConfiguration();

				root.AppendChild(listener);
			}

			//Append Viewer node
			var viewer = configuration.CreateElement("viewer");
			viewer.InnerXml = _viewer.GetConfiguration();
			root.AppendChild(viewer);

			return configuration;
		}

		public void SaveConfiguration(string fileName, string viewerConfiguration)
		{
			if (_log.Enabled) _log.Debug(GetHashCode(), "saveConfiguration");

			//Create Session (root) node
			var configuration = new XmlDocument();
			var pi =
				configuration.CreateProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\" ");
			configuration.AppendChild(pi);

			var root = configuration.CreateElement("session");
			configuration.AppendChild(root);
			var attrib = configuration.CreateAttribute("title");
			attrib.Value = _title;
			root.Attributes.Append(attrib);

			//Append listener nodes
			foreach (var l in Listeners)
			{
				var listener = configuration.CreateElement("listener");
				listener.InnerXml = l.GetConfiguration();
				root.AppendChild(listener);
			}

			//Append Viewer node
			var viewer = configuration.CreateElement("viewer");

			viewer.InnerXml = viewerConfiguration;
			root.AppendChild(viewer);

			configuration.Save(fileName);
		}
		
		private void WriteToLogFile(string msg)
		{
			if(_rollingStorage == null)
			{
				_rollingStorage = new RollingFileAppender(_viewer.LogFile, _viewer.LogRolling, _viewer.LogToFile);
			}
			_rollingStorage.Write(msg);
		}
		
		/// <summary>
		/// Cleans up this sessions resources
		/// </summary>
		~Session()
		{
			
		}
	}
}