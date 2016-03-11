/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Sockets;
using System.Text;

using logview4net.core;

namespace logview4net
{
    /// <summary>
    /// This interface is used for loggers.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Wther or not the logger is enabled
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Loggs the string p with debug severity
        /// </summary>
        /// <param name="p">The p.</param>
        void Debug(int callerHash, string p);

        /// <summary>
        /// Loggs the string p with INFO severity
        /// </summary>
        /// <param name="p">The p.</param>
        void Info(int callerHash, string p);

        /// <summary>
        /// Loggs the string p with ERROR severity
        /// </summary>
        /// <param name="foo">The foo.</param>
        void Error(int callerHash, string foo);

        /// <summary>
        /// Loggs the string p with WARN severity
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="ex">The ex.</param>
        void Warn(int callerHash, string p, Exception ex);

        /// <summary>
        /// Loggs the string p with debug severity
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="ex">The ex.</param>
        void Debug(int callerHash, string p, Exception ex);

        /// <summary>
        /// Loggs the string p with ERROR severity
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="ex">The ex.</param>
        void Error(int callerHash, string p, Exception ex);

        /// <summary>
        /// Loggs the string p with WARN severity
        /// </summary>
        /// <param name="p">The p.</param>
        void Warn(int callerHash, string p);
    }

    /// <summary>
    /// 
    /// </summary>
    public class Logger : ILog
    {
        #region Statics to get a named logger

        private static Dictionary<string, ILog> _loggers = new Dictionary<string, ILog>();
        private static UdpClient _udp = null;

        /// <summary>
        /// 
        /// </summary>
        public static int Port = 9898;

        private static bool _enabled = false;
        private static bool _hasReadEnabled = false;

        public bool Enabled
        {
            get
            {
                if (!_hasReadEnabled)
                {
                    _enabled = readEnabled();
                    _hasReadEnabled = true;
                }
                return _enabled;
            }
            set { _enabled = value; }
        }

        private bool readEnabled()
        {
            var ret = false;
            try
            {
            	ret = (bool)Logview4netSettings.Instance["Logger"];
            	Port = (int)Logview4netSettings.Instance["LogPort"];
            }
            catch(Exception ex)
            {
            	System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return ret;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static ILog GetLogger(string name)
        {
            if (_loggers.ContainsKey(name))
            {
                return _loggers[name];
            }
            else
            {
                var newLogger = new Logger(name);
                _loggers.Add(name, newLogger);
                return newLogger;
            }
        }

        private static void SendUDP(string s)
        {
            try
            {
                if (_udp == null)
                {
                    _udp = new UdpClient("127.0.0.1", Port);
                }
                var foo = Encoding.Unicode.GetBytes(s);

                _udp.Send(foo, foo.Length);
            }
            catch (Exception)
            {
                
            }
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ILog GetLogger(Type type)
        {
            return GetLogger(type.FullName);
        }

        #endregion

        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Logger(string name)
        {
            _name = name;
        }

        #region ILog Members

        /// <summary>
        /// Loggs the string p with debug severity.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="ex">The ex.</param>
        public void Debug(int callerHash, string p, Exception ex)
        {
            write(callerHash, "Debug", p + Environment.NewLine + ex.ToString() + Environment.NewLine + ex.StackTrace);
        }

        /// <summary>
        /// Loggs the string p with debug severity
        /// </summary>
        /// <param name="p">The p.</param>
        public void Debug(int callerHash, string p)
        {
            write(callerHash, "Debug", p);
        }

        /// <summary>
        /// Loggs the string p with INFO severity
        /// </summary>
        /// <param name="p">The p.</param>
        public void Info(int callerHash, string p)
        {
            write(callerHash, "Info", p);
        }

        /// <summary>
        /// Loggs the string p with INFO severity
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="ex">The ex.</param>
        public void Info(int callerHash, string p, Exception ex)
        {
            write(callerHash, "Info", p + Environment.NewLine + ex.ToString() + Environment.NewLine + ex.StackTrace );
        }

        /// <summary>
        /// Loggs the string p with WARN severity
        /// </summary>
        /// <param name="p">The p.</param>
        public void Warn(int callerHash, string p)
        {
            write(callerHash, "Warn", p);
        }

        /// <summary>
        /// Loggs the string p with WARN severity
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="ex">The ex.</param>
        public void Warn(int callerHash, string p, Exception ex)
        {
            write(callerHash, "Warn", p + Environment.NewLine + ex.ToString() + Environment.NewLine + ex.StackTrace);
        }

        /// <summary>
        /// Loggs the string p with ERROR severity
        /// </summary>
        /// <param name="foo">The foo.</param>
        public void Error(int callerHash, string foo)
        {
            write(callerHash, "Error", foo);
            ;
        }

        /// <summary>
        /// Loggs the string p with ERROR severity
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="ex">The ex.</param>
        public void Error(int callerHash, string p, Exception ex)
        {
            write(callerHash, "Error", p + Environment.NewLine + ex.ToString() + Environment.NewLine + ex.StackTrace);
        }


        private void write(int callerHash,  string level, string s)
        {
            if (Enabled)
            {
                var foo = level + " : " + _name + ":" + callerHash.ToString() + " : " + s;
                System.Diagnostics.Debug.WriteLine(foo);
                SendUDP(foo);
            }
        }


        #endregion
        
        
        
        
    }


}