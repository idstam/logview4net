/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

#if DEBUG
using System;
using System.IO;
using System.Threading;
using logview4net.Listeners;
using logview4net.Viewers;
using NUnit.Framework;
using System.Text;

namespace logview4net.test
{
	/// <summary>
	/// Summary description for FileListenerTest.
	/// </summary>
	[TestFixture]
	public class FileListenerTest
	{
        private static System.Collections.Generic.List<string> _tempFiles = null;

		/// <summary>
		/// Creates a new <see cref="FileListenerTest"/> instance.
		/// </summary>
		public FileListenerTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}


        /// <summary>
        /// Setups the fixture.
        /// </summary>
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            _tempFiles = new System.Collections.Generic.List<string>();
        }

        /// <summary>
        /// Cleans this instance.
        /// </summary>
        [TestFixtureTearDown]
        public void Clean()
        {
            foreach (string f in _tempFiles)
            {
                if (File.Exists(f))
                {
                    File.Delete(f);
                }
            }

        }

        /// <summary>
        /// Creates a temporary file and returns it's file name. 
        /// The file name will be stored in a list so that TestFixtureTearDown can delete them all
        /// </summary>
        /// <returns></returns>
        private static string createFile()
        {
            string file = Path.GetTempFileName();
            _tempFiles.Add(file);
            return file;
        }
        
        /// <summary>
		/// Checks what happens if there is no file to monitor..
		/// </summary>
		[Test]
		public void NonExistingFile()
		{
			
			MockViewer v = new MockViewer();
			FileListener f = new FileListener(@"c:\finns_inte.foo", 10, "foo", true);
			Session s = new Session(f, v);
			s.Start();
			Thread.Sleep(500);
			s.Stop();
		}


        /// <summary>
        /// Checks that existing data in a file is shown when only tail is false
        /// </summary>
        [Test]
        public void ShowExistingFileData()
        {
            MockViewer v = new MockViewer();
            string f = createFile();
            File.WriteAllText(f, "some random text");
            FileListener fl = new FileListener(f, 3, "foo", false);
            Session s = new Session(fl, v);
            s.Start();
            System.Windows.Forms.Application.DoEvents();
            Thread.Sleep(200);
            System.Windows.Forms.Application.DoEvents();
            Assert.IsTrue(v.ReceivedData.Count > 0);

            s.Stop();
        }

        /// <summary>
        /// Checks that existing data in a file is not shown when only tail is true
        /// </summary>
        [Test]
        public void DontShowExistingFileData()
        {
            MockViewer v = new MockViewer();
            string f = createFile();
            File.WriteAllText(f, "some random text");
            FileListener fl = new FileListener(f, 3, "foo", true);
            Session s = new Session(fl, v);
            s.Start();
            System.Windows.Forms.Application.DoEvents();
            Thread.Sleep(200);
            System.Windows.Forms.Application.DoEvents();

            Assert.AreEqual(0, v.ReceivedData.Count);

            s.Stop();
        }

        /// <summary>
        /// Checks that only new data in a file is shown when only tail is true
        /// </summary>
        [Test]
        public void ShowOnlyNewFileData()
        {
            MockViewer v = new MockViewer();
            string f = createFile();
            File.WriteAllText(f, "some random text");
            FileListener fl = new FileListener(f, 3, "foo", true);
            Session s = new Session(fl, v);
            s.Start();
            Thread.Sleep(200);
            System.Windows.Forms.Application.DoEvents();

            File.AppendAllText(f, "some more random text");
            Thread.Sleep(200);

            System.Windows.Forms.Application.DoEvents();

            Assert.AreEqual(1, v.ReceivedData.Count);

            s.Stop();
        }

        /// <summary>
        /// Checks that new and old data in a file is shown when only tail is false
        /// </summary>
        [Test]
        public void ShowNewAndOldFileData()
        {
            MockViewer v = new MockViewer();
            string f = createFile();
            File.WriteAllText(f, "some random text");
            FileListener fl = new FileListener(f, 3, "foo", false);
            Session s = new Session(fl, v);
            s.Start();

            System.Windows.Forms.Application.DoEvents();
            Thread.Sleep(200); //If this is to short the test will fail.
            System.Windows.Forms.Application.DoEvents();

            File.AppendAllText(f, "some more random text");
            Thread.Sleep(200); //If this is to short the test will fail.
            System.Windows.Forms.Application.DoEvents();

            Assert.AreEqual(2,v.ReceivedData.Count);

            s.Stop();
        }

        /// <summary>
        /// Serializes the listener.
        /// </summary>
        [Test]
        public void SerializeTheListener()
        {
            FileListener original = new FileListener(@"c:\finns_inte.foo", 10, "foo", true);
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(original.GetType());
            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            xs.Serialize(sw, original);

            System.IO.StringReader sr = new System.IO.StringReader(sb.ToString());

            FileListener copy = (FileListener)xs.Deserialize(sr);

            Assert.AreEqual(original.FileName, copy.FileName);
            Assert.AreEqual(original.MessagePrefix, copy.MessagePrefix);
            Assert.AreEqual(original.OnlyTail, copy.OnlyTail);
            Assert.AreEqual(original.PollInterval, copy.PollInterval);

            original.Dispose();
            copy.Dispose();
        }		
	}
	
}
#endif