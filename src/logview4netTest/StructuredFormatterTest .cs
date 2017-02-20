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
    /// Summary description for StructuredFormatterTest .
    /// </summary>
    [TestFixture]
    public class StructuredFormatterTest
    {

        /// <summary>
        /// Creates a new <see cref="StructuredFormatterTest "/> instance.
        /// </summary>
        public StructuredFormatterTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Checks that existing data in a file is shown when only tail is false
        /// </summary>
        [Test]
        public void FormatSimpleJson()
        {


            string msg = "{\"component\": \"AutoCheckOrder\",\"environment\": \"BACKOFFICE\"}";
            string formattedMsg = "\r\n{\r\n\t\"component\": \"AutoCheckOrder\",\r\n\t\"environment\": \"BACKOFFICE\"\r\n}\r\n";
            var listener = new FileListener();
            listener.SetConfigValue("structured", "json");
            var f = new StructuredMessageFormatter(msg, listener);
            var foo = f.Message();

            Assert.AreEqual(formattedMsg, foo);


        }
	
    }
    
}
