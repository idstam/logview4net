/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 *
 *
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Data.SqlClient;
using System.Threading;
using System.Collections.Generic;

namespace logview4net.Listeners
{
    /// <summary>
    /// This is the SQLListener it does a tail on a table in a SQL Server database table
    /// </summary>
    public class SqlListener :ListenerBase
    {
        private string _prefix = "MS-SQL";
        private bool _startAtEnd = true;
        private string _database;
        private string _server;
        private string _table;
        private string _user;
        private string _password;
        private string _column;
        private bool _winAuthentication;
        private int _interval = 3000;
        private Thread _listenerThread;


        #region SqlListener Properties

        /// <summary>
        /// Gets or sets a value indicating whether [start at end].
        /// </summary>
        /// <value><c>true</c> if [start at end]; otherwise, <c>false</c>.</value>
        public bool StartAtEnd
        {
            get
            {
                return _startAtEnd;
            }
            set
            {
                _startAtEnd = value;
            }
        }

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>The interval.</value>
        public int Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                _interval = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [win authentication].
        /// </summary>
        /// <value><c>true</c> if [win authentication]; otherwise, <c>false</c>.</value>
        public bool WinAuthentication
        {
            get
            {
                return _winAuthentication;
            }
            set
            {
                _winAuthentication = value;
            }
        }

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>The column.</value>
        public string Column
        {
            get
            {
                return _column;
            }
            set
            {
                _column = value;
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>The table.</value>
        public string Table
        {
            get
            {
                return _table;
            }
            set
            {
                _table = value;
            }
        }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>The server.</value>
        public string Server
        {
            get
            {
                return _server;
            }
            set
            {
                _server = value;
            }
        }


        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>The database.</value>
        public string Database
        {
            get
            {
                return _database;
            }
            set
            {
                _database = value;
            }
        }

        #endregion

        #region IListener Members

        public override Dictionary<string, ListenerConfigField> GetConfigValueFields()
        {
            return new Dictionary<string, ListenerConfigField>();
        }

        public override string SetConfigValue(string name, string value)
        {
            return null;
        }

        public override string GetConfigValue(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stops listening for the object implementing this interface.
        /// </summary>
        public override void Stop()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Stop");
            if (_listenerThread != null)
            {
                _listenerThread.Abort();
                _log.Info(GetHashCode(), "Stopped checking " + _table + " on " + _server + "/" + _database);
            }

            IsRunning = false;
        }

        /// <summary>
        /// Starts listening for the object implementing this interface.
        /// </summary>
        public override void Start()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Start");
            var ts = new ThreadStart(tail);
            _listenerThread = new Thread(ts);
            _listenerThread.Start();

            IsRunning = true;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            _log.Debug(GetHashCode(), "Dispoding a SQL Listener (nothing to dispose)");
        }

        #endregion

        private void tail()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "tail");
            object tailValue = null;
            
            try
            {
                _log.Info(GetHashCode(), "Initializing SQL Listener using prefix: " + _prefix);
                var csb = new SqlConnectionStringBuilder();
                if (_winAuthentication)
                {
                    csb.IntegratedSecurity = true;
                }
                else
                {
                    csb.UserID = _user;
                    csb.Password = _password;
                }

                csb.DataSource = _server;
                csb.InitialCatalog = _database;


                tailValue = getInitialTailValue(csb.ConnectionString);


                tailLoop(tailValue, csb.ConnectionString);
            }
            catch (Exception ex)
            {
                _log.Error(GetHashCode(), ex.Message, ex);
            }
        }

        object getInitialTailValue(string connectionString)
        {
            object tailValue;
            SqlConnection cn = null;
            SqlCommand cmd = null;

            using (cn = new SqlConnection(connectionString))
            {
                cn.Open();
                if (_startAtEnd)
                {
                    using (cmd = new SqlCommand("SELECT MAX(" + _column + ") FROM " + _table, cn))
                    {
                        tailValue = cmd.ExecuteScalar();
                        _log.Info(GetHashCode(), "Max " + _column + " = " + tailValue.ToString());
                    }
                }
                else
                {
                    tailValue = DBNull.Value;
                }
            }

            return tailValue;
        }


        void tailLoop(object tailValue, string connectionString)
        {
            SqlConnection cn;
            SqlCommand cmd;
            SqlDataReader dr;
			var tailColumnID = -1;
			
            while (true)
            {
                using (cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    var sql = "SELECT * FROM " + _table + "(NOLOCK) WHERE " + _column + " > @TailValue ORDER BY " + _column;
                    if (tailValue == DBNull.Value)
                    {
                        sql = "SELECT * FROM " + _table + "(NOLOCK) ORDER BY " + _column;
                    }
                    using (cmd = new SqlCommand(sql, cn))
                    {
                        if (tailValue != DBNull.Value)
                        {
                            cmd.Parameters.AddWithValue("@TailValue", tailValue);
                        }
                        dr = cmd.ExecuteReader();
                        if (tailColumnID == -1)
                        {
                            for (var i = 0; i < dr.FieldCount; i++)
                            {
                                if (dr.GetName(i).ToLower() == _column.ToLower())
                                {
                                    tailColumnID = i;
                                    _log.Debug(GetHashCode(), "Found TailColumn ID:" + tailColumnID.ToString());
                                    break;
                                }
                            }
                        }
                        while (dr.Read())
                        {
                            var message = "";
                            for (var i = 0; i < dr.FieldCount; i++)
                            {
                                message += dr.GetValue(i).ToString() + '\t';
                            }
                            Session.AddEvent(this, message);
                            tailValue = dr.GetValue(tailColumnID);
                        }
                    }
                }
                Thread.Sleep(_interval);
            }
        }

        #region IListener Members

        /// <summary>
        /// Gets a new configurator.
        /// </summary>
        /// <returns></returns>
        public IListenerConfigurator GetNewConfigurator()
        {
            return new SqlListenerConfigurator(this);
        }

        /// <summary>
        /// True if this listener has no historic data.
        /// </summary>
        /// <returns></returns>
        public bool OnlyTail
        {
            get
            {
                return _startAtEnd;
            }
            set
            {
                _startAtEnd = value;
            }
        }

        #endregion
    }
}