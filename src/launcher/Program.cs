using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace logview4net
{
    static class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        [STAThread]
        public static void Main(string[] args)
        {
        	logview4net.core.Logview4netSettings.Instance = Settings1.Default;
            Application.EnableVisualStyles();
            Application.DoEvents();
            	
            Application.Run(new App(args));

        }
    }
}
