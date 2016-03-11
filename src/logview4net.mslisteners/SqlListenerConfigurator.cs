/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace logview4net.Listeners
{
    /// <summary>
    /// This clas manages the configuration af a SqlListener
    /// </summary>
    public partial class SqlListenerConfigurator : UserControl, IListenerConfigurator
    {

        private ILog _log = Logger.GetLogger("logview4net.Listeners.SqlListenerConfigurator");
        private SqlListener _listener = new SqlListener();

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlListenerConfigurator"/> class.
        /// </summary>
        public SqlListenerConfigurator()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "SqlListenerConfigurator");
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlListenerConfigurator"/> class.
        /// </summary>
        /// <param name="listner">The listner.</param>
        public SqlListenerConfigurator(ListenerBase listner)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "SqlListenerConfigurator(IListener)");
            InitializeComponent();
            _listener = (SqlListener) listner;
            UpdateControls();

            txtServer.Enabled = false;
            txtUser.Enabled = false;
            txtPassword.Enabled = false;
            chkWinAuthentication.Enabled = false;
            cboDatabase.Enabled = false;
            cboTable.Enabled = false;
            cboColumn.Enabled = false;
            chkTail.Enabled = false;
        }

        #region IListenerConfigurator Members

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get { return "SQL Listener: " + _listener.MessagePrefix ; }
        }

        /// <summary>
        /// Gets or sets the configuration data for an implementation of this interface.
        /// </summary>
        /// <value></value>
        public string Configuration
        {
            get { return _listener.GetConfiguration(); }
            set
            {
                if (_log.Enabled) _log.Debug(GetHashCode(), "Configuration Set");
                var xs = new XmlSerializer(_listener.GetType());
                var sr = new StringReader(value);
                _listener = (SqlListener) xs.Deserialize(sr);

                UpdateControls();
            }
        }

        public void UpdateControls()
        {
            txtServer.Text = _listener.Server;
            txtUser.Text = _listener.User;
            txtPassword.Text = _listener.Password;
            chkWinAuthentication.Checked = _listener.WinAuthentication;
            txtPrefix.Text = _listener.MessagePrefix;
            cboDatabase.Text = _listener.Database;
            cboTable.Text = _listener.Table;
            cboColumn.Text = _listener.Column;
            txtIntervall.Text = _listener.Interval.ToString();
            chkTail.Checked = _listener.StartAtEnd;

            chkTimestamp.Checked = _listener.ShowTimestamp;
            txtFormat.Text = _listener.TimestampFormat == "" ? ListenerHelper.DefaultTimestampFormat : _listener.TimestampFormat;

        }

        /// <summary>
        /// Gets the listener for an implementation of this interface
        /// </summary>
        /// <value></value>
        public ListenerBase ListenerBase
        {
            get { return _listener; }
            set { _listener = (SqlListener) value; }
        }

        #endregion

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

        private void cboColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            _listener.Column = cboColumn.Text;
        }

        private void chkWinAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            _listener.WinAuthentication = chkWinAuthentication.Checked;
        }

        private void txtIntervall_TextChanged(object sender, EventArgs e)
        {
            int foo;
            if(int.TryParse(txtIntervall.Text, out foo))
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
            fillCombo((ComboBox) sender, "SELECT Name FROM master..sysdatabases ORDER BY Name");
            Cursor = Cursors.Default;
        }

        private void cboTable_DropDown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var changeDB = "use [" + cboDatabase.Text + "]; ";
            var sql = "SELECT TABLE_NAME as Name FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME";
            fillCombo((ComboBox) sender, changeDB + sql);
            Cursor = Cursors.Default;
        }

        private void cboColumn_DropDown(object sender, EventArgs e)
        {
            var changeDB = "use [" + cboDatabase.Text + "]; ";
            var sql = "SELECT COLUMN_NAME as Name FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + cboTable.Text + "'" +
                         " ORDER BY COLUMN_NAME";
            fillCombo((ComboBox) sender, changeDB + sql);
        }

        private void fillCombo(ComboBox cbo, string sql)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "fillCombo " + cbo.Name + " " + sql);

            var cn = getOpenConnection();
            if (cn == null)
            {
                return;
            }
            else
            {
                var da = new SqlDataAdapter(sql, cn);
                var ds = new DataSet();
                da.Fill(ds);
                cbo.DataSource = ds.Tables[0];
                cbo.DisplayMember = "Name";
                da.Dispose();
                cn.Dispose();
            }
        }

        private SqlConnection getOpenConnection()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "getOpenConnection");
            try
            {
                _log.Debug(GetHashCode(), "Creating db connection");
                var csb = new SqlConnectionStringBuilder();
                if (chkWinAuthentication.Checked)
                {
                    csb.IntegratedSecurity = true;
                }
                else
                {
                    csb.UserID = txtUser.Text;
                    ;
                    csb.Password = txtPassword.Text;
                }

                csb.DataSource = txtServer.Text;

                var cn = new SqlConnection(csb.ConnectionString);
                cn.Open();

                return cn;
            }
            catch (Exception ex)
            {
                _log.Debug(GetHashCode(), "Tried to open database.", ex);
                return null;
            }
        }

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