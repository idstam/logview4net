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
using System.Text;
using System.Xml.Serialization;
using NUnit.Framework;
using logview4net.Listeners;
using NUnit.Framework.Legacy;

namespace logview4net.test
{
    /// <summary>
    /// Tests for the SQL Listener
    /// </summary>
    [TestFixture]
    public class SqlListenerTest
    {


        /// <summary>
        /// Serializes the listener.
        /// </summary>
        [Test]
        public void SerializeTheListener()
        {
            var original = new SqlListener();
            original.MessagePrefix = "MP";
            original.Column = "C";
            original.Database = "D";
            original.Interval = 100;
            original.Password = "P";
            original.Server = "S";
            original.StartAtEnd = true;
            original.Table = "T";
            original.User = "U";
            original.WinAuthentication = true;
            
            var xs = new XmlSerializer(original.GetType());
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            xs.Serialize(sw, original);

            var sr = new StringReader(sb.ToString());

            var copy = (SqlListener)xs.Deserialize(sr);

            ClassicAssert.AreEqual(original.MessagePrefix , copy.MessagePrefix);
            ClassicAssert.AreEqual(original.Column , copy.Column);
            ClassicAssert.AreEqual(original.Database , copy.Database);
            ClassicAssert.AreEqual(original.Interval , copy.Interval);
            ClassicAssert.AreEqual(original.Password , copy.Password);
            ClassicAssert.AreEqual(original.Server , copy.Server);
            ClassicAssert.AreEqual(original.StartAtEnd , copy.StartAtEnd);
            ClassicAssert.AreEqual(original.Table , copy.Table);
            ClassicAssert.AreEqual(original.User , copy.User);
            ClassicAssert.AreEqual(original.WinAuthentication, copy.WinAuthentication);

            original.Dispose();
            copy.Dispose();
        }
        
    }
}