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
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
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
        private static List<string> _tempFiles = null;

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
            _tempFiles = new List<string>();
        }

        /// <summary>
        /// Cleans this instance.
        /// </summary>
        [TestFixtureTearDown]
        public void Clean()
        {
            foreach (var f in _tempFiles)
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
            var file = Path.GetTempFileName();
            _tempFiles.Add(file);
            return file;
        }
        
        /// <summary>
        /// Checks what happens if there is no file to monitor..
        /// </summary>
        [Test]
        public void NonExistingFile()
        {
            
            var v = new MockViewer();
            var f = new FileListener(@"c:\finns_inte.foo", 10, "foo", true);
            var s = new Session(f, v);
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
            var v = new MockViewer();
            var f = createFile();
            File.WriteAllText(f, "some random text");
            var fl = new FileListener(f, 3, "foo", false);
            var s = new Session(fl, v);
            s.Start();
            Application.DoEvents();
            Thread.Sleep(200);
            Application.DoEvents();
            Assert.IsTrue(v.ReceivedData.Count > 0);

            s.Stop();
        }

        /// <summary>
        /// Checks that existing data in a file is not shown when only tail is true
        /// </summary>
        [Test]
        public void DontShowExistingFileData()
        {
            var v = new MockViewer();
            var f = createFile();
            File.WriteAllText(f, "some random text");
            var fl = new FileListener(f, 3, "foo", true);
            var s = new Session(fl, v);
            s.Start();
            Application.DoEvents();
            Thread.Sleep(200);
            Application.DoEvents();

            Assert.AreEqual(0, v.ReceivedData.Count);

            s.Stop();
        }

        /// <summary>
        /// Checks that only new data in a file is shown when only tail is true
        /// </summary>
        [Test]
        public void ShowOnlyNewFileData()
        {
            var v = new MockViewer();
            var f = createFile();
            File.WriteAllText(f, "some random text");
            var fl = new FileListener(f, 3, "foo", true);
            var s = new Session(fl, v);
            s.Start();
            Thread.Sleep(200);
            Application.DoEvents();

            File.AppendAllText(f, "some more random text");
            Thread.Sleep(200);

            Application.DoEvents();

            Assert.AreEqual(1, v.ReceivedData.Count);

            s.Stop();
        }

        /// <summary>
        /// Checks that new and old data in a file is shown when only tail is false
        /// </summary>
        [Test]
        public void ShowNewAndOldFileData()
        {
            var v = new MockViewer();
            var f = createFile();
            File.WriteAllText(f, "some random text");
            var fl = new FileListener(f, 3, "foo", false);
            var s = new Session(fl, v);
            s.Start();

            Application.DoEvents();
            Thread.Sleep(200); //If this is to short the test will fail.
            Application.DoEvents();

            File.AppendAllText(f, "some more random text");
            Thread.Sleep(200); //If this is to short the test will fail.
            Application.DoEvents();

            Assert.AreEqual(2,v.ReceivedData.Count);

            s.Stop();
        }

        /// <summary>
        /// Serializes the listener.
        /// </summary>
        [Test]
        public void SerializeTheListener()
        {
            var original = new FileListener(@"c:\finns_inte.foo", 10, "foo", true);
            var xs = new XmlSerializer(original.GetType());
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            xs.Serialize(sw, original);

            var sr = new StringReader(sb.ToString());

            var copy = (FileListener)xs.Deserialize(sr);

            Assert.AreEqual(original.FileName, copy.FileName);
            Assert.AreEqual(original.MessagePrefix, copy.MessagePrefix);
            Assert.AreEqual(original.OnlyTail, copy.OnlyTail);
            Assert.AreEqual(original.PollInterval, copy.PollInterval);

            original.Dispose();
            copy.Dispose();
        }		
    }
    
}
