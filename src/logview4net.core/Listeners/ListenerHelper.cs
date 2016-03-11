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
using System.Xml;
using System.Xml.Serialization;

namespace logview4net.Listeners
{
    public class ListenerHelper
    {
        public static string DefaultTimestampFormat = "yyyy-MM-dd HH:mm:ss:fff";

        public static string SerializeListener(object listener)
        {
            var xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Encoding = Encoding.UTF8;

            var xs = new XmlSerializer(listener.GetType());

            var sb = new StringBuilder();
            var xw = XmlWriter.Create(sb, xws);

            xs.Serialize(xw, listener);

            return sb.ToString();
        }

        public static int GetSafeInt(string number)
        {
            var foo = 0;
            int.TryParse(number, out foo);
            return foo;
        }



    }
}