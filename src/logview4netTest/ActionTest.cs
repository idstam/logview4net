/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Xml;
using logview4net.Viewers;
using System.Drawing;
using Action = logview4net.Viewers.Action;

namespace logview4netTest
{
    [TestFixture]
    public class ActionTest
    {
        [Test]
        public void ActionFontSizeIsInteger()
        {
            var foo =
                "<viewer type=\"Text\" buffer=\"5000\" forecolor=\"Lime\" backcolor=\"Black\" font=\"Courier New\" fontsize=\"9\">" +
                "<action type=\"Highlight\" pattern=\"Document\" color=\"White\" font-name=\"Microsoft Sans Serif\" font-size=\"8\" font-style=\"Regular\" />" +
                "</viewer>";

            var xml = new XmlDocument();
            xml.LoadXml(foo);

            var n = xml.FirstChild.ChildNodes[0];
            var a =  Action.CreateGenericEventAction(n);

            var s = 8f;
            Assert.AreEqual(s, a.Font.Size);
        }

        /// <summary>
        /// Actions the font size is float.
        /// </summary>
        [Test]
        public void ActionFontSizeIsFloat()
        {
            var foo =
                "<viewer type=\"Text\" buffer=\"5000\" forecolor=\"Lime\" backcolor=\"Black\" font=\"Courier New\" fontsize=\"9\">" +
                "<action type=\"Highlight\" pattern=\"Document\" color=\"White\" font-name=\"Microsoft Sans Serif\" font-size=\"8.25\" font-style=\"Regular\" />" +
                "</viewer>";

            var xml = new XmlDocument();
            xml.LoadXml(foo);

            var n = xml.FirstChild.ChildNodes[0];
            var a = Action.CreateGenericEventAction(n);

            var s = 8.25f;
            Assert.AreEqual(s, a.Font.Size);
        }
    }
}
