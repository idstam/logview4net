/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Diagnostics;

namespace logview4net
{
    public class ExceptionManager
    {
        public static Exception HandleException(int callerHash, Exception ex)
        {
            Logger.GetLogger("ExceptionManager").Error(callerHash, ex.Message + Environment.NewLine + ex.StackTrace);
            Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            return ex;
        }
    }
}