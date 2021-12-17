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

namespace logview4net
{
    public class Helpers
    {
        public static float SafeFloatParse(string val)
        {
            float ret = 0;

            try
            {
                ret = float.Parse(val);
                return ret;
            }
            catch
            {
                ret = (float)XmlConvert.ToDouble(val);
                return ret;
            }
        }

        public static bool SafeBoolParse(string val)
        {
        
            try
            {
                var ret = false;
                if (bool.TryParse(val, out ret))
                {
                    return ret;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }
    }
}
