/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2011 Botond B. Balazs
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;

namespace logview4net.Listeners
{
    /// <summary>
    /// A listener for MySQL database tables
    /// </summary>
    public class MySqlListener : IListener
    {
        /// <summary>
        /// The thread the listener function (tail()) will run on
        /// </summary>
        private Thread _listenerThread;

        /// <summary>
        /// The log this listener will use if logging is enabled
        /// </summary>
        private ILog _log = Logger.GetLogger("logview4net.Listeners.MySqlListener");

        
        /// <summary>
        /// Indicates whether the listener is running
        /// </summary>
        private bool _isRunning = false;

        /// <summary>
        /// Indicates whether the listener has column headers that the viewer can ask for.
        /// </summary>
        public bool HasColumnHeaders{ get{ return true;}}
        
        /// <summary>
        /// Gets or sets the name of the MySQL host the listener connects to
        /// </summary>        
        public string Server { get; set; }

        private uint _port = 3306;
        /// <summary>
        /// Gets or sets the port to use when connecting to the MySQL host
        /// </summary>
        public uint Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        /// Gets or sets the user ID to use when connecting to the MySQL host
        /// </summary>
        public string User { get; set; }        

        /// <summary>
        /// Gets or sets the password to use when connecting to the MySQL host
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the database to use when connecting to the MySQL host
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the table to listen on
        /// </summary>
        public string Table { get; set; }

        private bool _startAtEnd = true;
        /// <summary>
        /// Gets or sets a value determining whether only new or all rows are returned
        /// when the listener starts.
        /// </summary>
        public bool StartAtEnd 
        {
            get { return _startAtEnd; }
            set { _startAtEnd = value; }
        }


        /// <summary>
        /// Gets or sets the prefix to be attached to messages from this listener
        /// when displaying them
        /// </summary>
        public string MessagePrefix { get; set; }

        /// <summary>
        /// Gets a value determining whether the listener can be restarted
        /// </summary>
        public bool IsRestartable 
        {
            get { return true; } 
        }

        private Session _session;
        /// <summary>
        /// Sets the <see cref="Session"/> this listener belongs to
        /// </summary>
        public Session Session 
        {
            set { _session = value; } 
        }

        /// <summary>
        /// Gets a value indicating whether the listener is running
        /// </summary>
        public bool IsRunning 
        {
            get { return _isRunning; }
        }

        private string _hash = Guid.NewGuid().ToString();
        /// <summary>
        /// Gets a value that uniquely identifies this listener instance
        /// </summary>
        public string Hash 
        { 
            get { return _hash; } 
        }

        /// <summary>
        /// Gets or sets a value determining whether the listener has been configured
        /// </summary>
        public bool IsConfigured { get; set; }

        private int _pollInterval = 3000;
        /// <summary>
        /// Gets or sets the timeout, in milliseconds, after polling the database
        /// </summary>
        public int Interval
        {
            get { return _pollInterval; }
            set { _pollInterval = value; }
        }

        /// <summary>
        /// True if this listener has no historic data.
        /// </summary>
        public bool OnlyTail
        {
            get { return _startAtEnd; }
            set { _startAtEnd = value; }
        }

        /// <summary>
        /// Runs on a separate thread, queries the database for new rows, and adds the corresponding
        /// events to the <see cref="Session"/> if needed.
        /// </summary>
        private void tail()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "tail");

            MySqlConnection conn = null;
            MySqlCommand noLockCommand = null;
            MySqlCommand countCommand = null;
            MySqlCommand selectCommand = null;

            try
            {
                conn = new MySqlConnection(GetConnectionString());
                conn.Open();

                noLockCommand = new MySqlCommand("SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED", conn);
                countCommand = new MySqlCommand(string.Format("SELECT COUNT(*) FROM `{0}`", Table), conn);
                selectCommand = null;

                noLockCommand.ExecuteNonQuery();
                var lastRow = StartAtEnd ? (Int64)countCommand.ExecuteScalar() : 0;

                while (true)
                {
                    var newLastRow = (Int64)countCommand.ExecuteScalar();

                    if (newLastRow > lastRow) // check if there are new rows
                    {
                        selectCommand = new MySqlCommand(
                            string.Format("SELECT * FROM `{0}` LIMIT @offset, @length", Table), conn);
                        selectCommand.Parameters.AddWithValue("@offset", lastRow);
                        selectCommand.Parameters.AddWithValue("@length", newLastRow - lastRow);                        
                        var reader = selectCommand.ExecuteReader();
                        lastRow = newLastRow;

                        while (reader.Read())
                        {
                            var sb = new StringBuilder();
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                sb.AppendFormat(string.Format("{0} = {1} ", reader.GetName(i), reader.GetString(i)));
                            }
                            _session.AddEvent(this, sb.ToString());
                            
                        }

                        reader.Close();
                        selectCommand.Dispose();
                    }

                    Thread.Sleep(Interval);
                }

            }
            catch (Exception ex)
            {
                _log.Error(GetHashCode(), ex.Message, ex);
            }
            finally
            {
                if (noLockCommand != null) noLockCommand.Dispose();
                if (countCommand != null) countCommand.Dispose();
                if (selectCommand != null) selectCommand.Dispose();
                if (conn != null) conn.Dispose();
            }
        }

        /// <summary>
        /// Builds a connection string from the Server, Port, User, Password and Database properties
        /// </summary>
        /// <returns>A MySQL connection string</returns>
        private string GetConnectionString()
        {
            var csb = new MySqlConnectionStringBuilder();
            csb.Server = Server;
            csb.Port = Port;
            csb.UserID = User;
            csb.Password = Password;
            csb.Database = Database;
            return csb.ConnectionString;
        }

        /// <summary>
        /// Gets the configuration node for this listener.
        /// </summary>
        /// <returns></returns>
        public string GetConfiguration()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "GetConfiguration");
            return ListenerHelper.SerializeListener(this);
        }

        /// <summary>
        /// Stops the listener
        /// </summary>
        public void Stop()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Stop");
            if (_listenerThread != null)
            {
                _listenerThread.Abort();
                _log.Info(GetHashCode(), "Stopped checking " + Table + " on " + Server + "/" + Database);
            }

            _isRunning = false;
        }

        /// <summary>
        /// Starts the listener
        /// </summary>
        public void Start()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Start");
            var ts = new ThreadStart(tail);
            _listenerThread = new Thread(ts);
            _listenerThread.Start();

            _isRunning = true;
        }

        /// <summary>
        /// Gets a new configurator.
        /// </summary>
        /// <returns></returns>
        public IListenerConfigurator GetNewConfigurator()
        {
            return new MySqlListenerConfigurator(this);
        }

        /// <summary>
        /// Disposes of the listener
        /// </summary>
        public void Dispose()
        {
            _log.Debug(GetHashCode(), "Disposing a MySQL Listener (nothing to dispose)");
        }
        public bool ShowTimestamp { get; set; }
        public string TimestampFormat { get; set; }

    }
}
