/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using logview4net.Listeners;

namespace logview4net.test
{
	/// <summary>
	/// This class was made to enable testing if the <see cref="UdpListenerBase"/>
	/// </summary>
	public class UdpSender
	{
		private string[] _data;
		private int _interval;
		private string _testEndpoint ;
		private int _testPort ;

		/// <summary>
		/// Creates a new <see cref="UdpSender"/> instance.
		/// </summary>
		/// <param name="endpoint">The IP to send UDP traffic to</param>
		/// <param name="port">The IPport to use</param>
		/// <param name="data">The data to send. There will be one message for each string in the array.</param>
		/// <param name="interval">How many milliseconds to pause between each message.</param>
		public UdpSender(string endpoint, int port, string[] data, int interval)
		{
			_testEndpoint = endpoint;
			_testPort = port;
			_data = data;
			_interval = interval;

		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		public void Start()
		{

			for(var i = 0; i < _data.Length; i++)
			{
				Thread.Sleep(_interval);

				var fooText = _data[i];
				var udp = new UdpClient(_testEndpoint, _testPort);
				var foo = Encoding.Unicode.GetBytes(fooText);
				
				udp.Send(foo, foo.Length);

			}
		}
	}
}