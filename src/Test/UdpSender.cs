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

namespace logview4net.test
{
	/// <summary>
	/// This class was made to enable testing if the <see cref="Listeners.UdpListener"/>
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

			for(int i = 0; i < _data.Length; i++)
			{
				Thread.Sleep(_interval);

				string fooText = _data[i];
				System.Net.Sockets.UdpClient udp = new System.Net.Sockets.UdpClient(_testEndpoint, _testPort);
				byte[] foo = System.Text.Encoding.Unicode.GetBytes(fooText);
				
				udp.Send(foo, foo.Length);

			}
		}
	}
}
#endif