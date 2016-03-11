/*
 * Created by SharpDevelop.
 * User: johan
 * Date: 2012-10-31
 * Time: 09:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using logview4net;
using logview4net.test;
using NUnit.Framework;

namespace logview4netTest
{
	/// <summary>
	/// Description of RollingFileAppenderTest.
	/// </summary>
	[TestFixture]
	public class RollingFileAppenderTest
	{
		
		private string _folder = null;
		
		[SetUp]
		public void SetUp()
		{
			_folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(_folder);
			
		}
		
		[TearDown]
		public void TearDown()
		{
			try
			{
				if(_folder != null)
				{
					//Delete the test folder
					Directory.Delete(_folder, true);
				}
			}
			catch{}
		}
		
		[Test]
		public void DisabledRollingTest()
		{
			var v = new MockViewer();
			var l = new MockListener();
			
			v.LogFile = Path.Combine(_folder, "logview4net.test.txt");
			v.LogRolling = "100 KB";
			v.LogToFile = false;
			var s = new Session(l, v);
			
			s.AddEvent(l, "store log test");
			
			Assert.IsFalse(File.Exists(v.LogFile));	
			
		}


		[Test]
		public void WriteRollingTest()
		{
			var v = new MockViewer();
			var l = new MockListener();
			
			v.LogFile = Path.Combine(_folder, "logview4net.test.txt");
			v.LogRolling = "100 KB";
			v.LogToFile = true;
			var s = new Session(l, v);
			
			s.AddEvent(l, "store log test");
			
			Assert.IsTrue(File.Exists(v.LogFile));	
			
		}		

		[Test]
		public void SizeRollingTest()
		{
			var v = new MockViewer();
			var l = new MockListener();
			
			v.LogFile = Path.Combine(_folder, "logview4net.test.txt");
			v.LogRolling = "100 KB";
			v.LogToFile = true;
			var s = new Session(l, v);
			
			s.AddEvent(l, "store log test");
			
			Assert.IsTrue(File.Exists(v.LogFile), "SizeRollingTest A");	
			Assert.IsFalse(File.Exists(v.LogFile + ".1"), "SizeRollingTest B");	
			
			s.AddEvent(l, new String('X', 100 * 1024));

			Assert.IsTrue(File.Exists(v.LogFile), "SizeRollingTest D");	
			Assert.IsFalse(File.Exists(v.LogFile + ".1"), "SizeRollingTest E");	

			s.AddEvent(l, new String('X', 10 * 1024));

			Assert.IsTrue(File.Exists(v.LogFile), "SizeRollingTest G");	
			Assert.IsTrue(File.Exists(v.LogFile + ".1"), "SizeRollingTest H");	

			s.AddEvent(l, new String('X', 100 * 1024));
			s.AddEvent(l, new String('X', 10 * 1024));
			Assert.IsTrue(File.Exists(v.LogFile + ".2"), "SizeRollingTest I");	
		}		

		[Test]
		public void DateRollingTest()
		{
			var v = new MockViewer();
			var l = new MockListener();
			
			v.LogFile = Path.Combine(_folder, "logview4net.test.txt");
			v.LogRolling = "Daily";
			v.LogToFile = true;
			var s = new Session(l, v);
			
			s.AddEvent(l, "store log test");
			
			s._rollingStorage._lastDatePart = "foo";
			s.AddEvent(l, "store log test");
			Assert.IsTrue(File.Exists(v.LogFile + "." + DateTime.Now.ToString("yyyyMMdd")));

			s._rollingStorage._lastDatePart = "foo";
			s.AddEvent(l, "store log test");
			Assert.IsTrue(File.Exists(v.LogFile + "." + DateTime.Now.ToString("yyyyMMdd") + ".1"));
		}		


	}
}
