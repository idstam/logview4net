/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2011 Botond B. Balazs
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;

namespace logview4net.Listeners
{
    /// <summary>
    /// Configurator GUI for the MySQL listener
    /// </summary>
    public partial class MySqlListenerConfigurator : UserControl, IListenerConfigurator
    {
        private ILog _log = Logger.GetLogger("logview4net.Listeners.SqlListenerConfigurator");
        private MySqlListener _listener = new MySqlListener();

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlListenerConfigurator"/> class.
        /// </summary>
        public MySqlListenerConfigurator()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "MySqlListenerConfigurator");
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlListenerConfigurator"/> class.
        /// </summary>
        /// <param name="listenerBase">The listener to configure.</param>
        public MySqlListenerConfigurator(ListenerBase listenerBase)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "MySqlListenerConfigurator(IListener)");
            InitializeComponent();
            _listener = (MySqlListener)listenerBase;
            UpdateControls();

            txtServer.Enabled = false;
            txtPort.Enabled = false;
            txtUser.Enabled = false;
            txtPassword.Enabled = false;
            cboDatabase.Enabled = false;
            cboTable.Enabled = false;
            chkTail.Enabled = false;
        }

        #region IListenerConfigurator Members

        public string Caption
        {
            get { return "MySQL listener: " + _listener.MessagePrefix; }
        }

        public string Configuration
        {
            get { return _listener.GetConfiguration(); }
            set
            {
                if (_log.Enabled) _log.Debug(GetHashCode(), "Configuration Set");
                var xs = new XmlSerializer(_listener.GetType());
                var sr = new StringReader(value);
                _listener = (MySqlListener)xs.Deserialize(sr);

                UpdateControls();
            }
        }

        public ListenerBase ListenerBase
        {
            get { return _listener; }
            set { _listener = (MySqlListener)value; }
        }

        public void UpdateControls()
        {
            txtServer.Text = _listener.Server;
            txtUser.Text = _listener.User;
            txtPort.Text = _listener.Port.ToString();
            txtPassword.Text = _listener.Password;
            txtPrefix.Text = _listener.MessagePrefix;
            cboDatabase.Text = _listener.Database;
            cboTable.Text = _listener.Table;
            txtInterval.Text = _listener.Interval.ToString();
            chkTail.Checked = _listener.StartAtEnd;

            chkTimestamp.Checked = _listener.ShowTimestamp;
            txtFormat.Text = _listener.TimestampFormat == "" ? ListenerHelper.DefaultTimestampFormat : _listener.TimestampFormat;

        }

        #endregion

        #region Event Handlers

        private void txtPrefix_TextChanged(object sender, EventArgs e)
        {
            _listener.MessagePrefix = txtPrefix.Text;
            Text = txtPrefix.Text;
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {
            _listener.Server = txtServer.Text;
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            _listener.User = txtUser.Text;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            _listener.Password = txtPassword.Text;
        }

        private void cboDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            _listener.Database = cboDatabase.Text;
        }

        private void cboTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            _listener.Table = cboTable.Text;
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            uint port;
            if (uint.TryParse(txtPort.Text, out port))
            {
                _listener.Port = port;
            }
        }

        private void txtInterval_TextChanged(object sender, EventArgs e)
        {
            int foo;
            if (int.TryParse(txtInterval.Text, out foo))
            {
                _listener.Interval = foo;
            }
        }

        private void chkTail_CheckedChanged(object sender, EventArgs e)
        {
            _listener.StartAtEnd = chkTail.Checked;
        }

        private void cboDatabase_DropDown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            fillCombo((ComboBox)sender, "SHOW DATABASES;", "Database");
            Cursor = Cursors.Default;
        }

        private void cboTable_DropDown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var dbName = cboDatabase.Text;
            var sql = string.Format("use `{0}`; SHOW TABLES;", dbName);
            var columnName = string.Format("Tables_in_{0}", dbName);
            fillCombo((ComboBox)sender, sql, columnName);
            Cursor = Cursors.Default;
        }

        #endregion

        #region Helpers

        private void fillCombo(ComboBox cbo, string sql, string columnName)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "fillCombo " + cbo.Name + " " + sql + " " + columnName);

            var cn = getOpenConnection();
            if (cn == null)
            {
                return;
            }
            else
            {
                var da = new MySqlDataAdapter(sql, cn);
                var ds = new DataSet();
                da.Fill(ds);
                cbo.DataSource = ds.Tables[0];
                cbo.DisplayMember = columnName;
                da.Dispose();
                cn.Dispose();
            }
        }

        private MySqlConnection getOpenConnection()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "getOpenConnection");
            try
            {
                var csb = new MySqlConnectionStringBuilder();
                csb.UserID = txtUser.Text;
                csb.Password = txtPassword.Text;
                csb.Server = txtServer.Text;

                uint port;
                if (uint.TryParse(txtPort.Text, out port)) csb.Port = port;

                var cn = new MySqlConnection(csb.ConnectionString);
                cn.Open();

                return cn;
            }
            catch (Exception ex)
            {
                _log.Debug(GetHashCode(), "Tried to open database.", ex);
                return null;
            }
        }

        #endregion

        private void chkTimestamp_CheckedChanged(object sender, EventArgs e)
        {
            txtFormat.Enabled = chkTimestamp.Checked;
            _listener.ShowTimestamp = chkTimestamp.Checked;
        }

        private void txtFormat_TextChanged(object sender, EventArgs e)
        {
            _listener.TimestampFormat = txtFormat.Text;
        }
    }
}
