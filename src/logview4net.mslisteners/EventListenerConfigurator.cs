/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace logview4net.Listeners
{
    /// <summary>
    /// Summary description for UdpListenerConfigurator.
    /// </summary>
    public class EventLogListenerConfigurator : UserControl, IListenerConfigurator
    {

        private EventLogListener _listener = new EventLogListener();
        private Panel panel1;
        private TextBox txtPrefix;
        private Label label4;
        private CheckBox chkFieldNames;
        private ComboBox cboLogName;
        private TextBox txtIntervall;
        private Label label3;
        private Label label2;
        private TextBox txtHost;
        private Label label1;
        private ILog _log = Logger.GetLogger("logview4net.Listeners.EventLogListenerConfigurator");
        private CheckBox chkTimestamp;
        private TextBox txtFormat;
        private Label label5;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        /// <summary>
        /// Creates a new <see cref="EventLogListenerConfigurator"/> instance.
        /// </summary>
        public EventLogListenerConfigurator()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Create EventLogListenerConfigurator");

            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            _listener = new EventLogListener();
            _listener.Host = ".";
            _listener.LogName = "Application";
            _listener.PollInterval = 3000;
        }

        /// <summary>
        /// Creates a new <see cref="EventLogListenerConfigurator"/> instance.
        /// </summary>
        public EventLogListenerConfigurator(EventLogListener listener)
        {
            _listener = listener;
            if (_log.Enabled) _log.Debug(GetHashCode(), "Create EventLogListenerConfigurator(IListener)");

            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            UpdateControls();

            cboLogName.Enabled = false;
            txtHost.Enabled = false;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Dispose");
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkFieldNames = new System.Windows.Forms.CheckBox();
            this.cboLogName = new System.Windows.Forms.ComboBox();
            this.txtIntervall = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkTimestamp = new System.Windows.Forms.CheckBox();
            this.txtFormat = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkTimestamp);
            this.panel1.Controls.Add(this.txtFormat);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtPrefix);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.chkFieldNames);
            this.panel1.Controls.Add(this.cboLogName);
            this.panel1.Controls.Add(this.txtIntervall);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtHost);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(456, 79);
            this.panel1.TabIndex = 0;
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(37, 3);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(102, 20);
            this.txtPrefix.TabIndex = 23;
            this.txtPrefix.TextChanged += new System.EventHandler(this.txtPrefix_TextChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 18);
            this.label4.TabIndex = 22;
            this.label4.Text = "Prefix";
            // 
            // chkFieldNames
            // 
            this.chkFieldNames.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkFieldNames.Checked = true;
            this.chkFieldNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFieldNames.Location = new System.Drawing.Point(6, 30);
            this.chkFieldNames.Name = "chkFieldNames";
            this.chkFieldNames.Size = new System.Drawing.Size(130, 24);
            this.chkFieldNames.TabIndex = 21;
            this.chkFieldNames.Text = "Append field names";
            this.chkFieldNames.CheckedChanged += new System.EventHandler(this.chkFieldNames_CheckedChanged);
            // 
            // cboLogName
            // 
            this.cboLogName.Location = new System.Drawing.Point(332, 3);
            this.cboLogName.Name = "cboLogName";
            this.cboLogName.Size = new System.Drawing.Size(121, 21);
            this.cboLogName.TabIndex = 20;
            this.cboLogName.Text = "Application";
            this.cboLogName.DropDown += new System.EventHandler(this.cboLogName_DropDown_1);
            this.cboLogName.SelectedIndexChanged += new System.EventHandler(this.cboLogName_SelectedIndexChanged);
            // 
            // txtIntervall
            // 
            this.txtIntervall.Location = new System.Drawing.Point(223, 32);
            this.txtIntervall.Name = "txtIntervall";
            this.txtIntervall.Size = new System.Drawing.Size(43, 20);
            this.txtIntervall.TabIndex = 19;
            this.txtIntervall.Text = "3000";
            this.txtIntervall.TextChanged += new System.EventHandler(this.txtIntervall_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(134, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 18);
            this.label3.TabIndex = 18;
            this.label3.Text = "Poll intervall (ms)";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(274, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 18);
            this.label2.TabIndex = 17;
            this.label2.Text = "Log name";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(189, 4);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(77, 20);
            this.txtHost.TabIndex = 16;
            this.txtHost.Text = ".";
            this.txtHost.TextChanged += new System.EventHandler(this.txtHost_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(145, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 18);
            this.label1.TabIndex = 15;
            this.label1.Text = "Host IP";
            // 
            // chkTimestamp
            // 
            this.chkTimestamp.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkTimestamp.Location = new System.Drawing.Point(6, 54);
            this.chkTimestamp.Name = "chkTimestamp";
            this.chkTimestamp.Size = new System.Drawing.Size(79, 24);
            this.chkTimestamp.TabIndex = 26;
            this.chkTimestamp.Text = "Timestamp";
            this.chkTimestamp.CheckedChanged += new System.EventHandler(this.chkTimestamp_CheckedChanged);
            // 
            // txtFormat
            // 
            this.txtFormat.Enabled = false;
            this.txtFormat.Location = new System.Drawing.Point(158, 56);
            this.txtFormat.Name = "txtFormat";
            this.txtFormat.Size = new System.Drawing.Size(108, 20);
            this.txtFormat.TabIndex = 25;
            this.txtFormat.TextChanged += new System.EventHandler(this.txtFormat_TextChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(110, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 18);
            this.label5.TabIndex = 24;
            this.label5.Text = "Format";
            // 
            // EventLogListenerConfigurator
            // 
            this.Controls.Add(this.panel1);
            this.Name = "EventLogListenerConfigurator";
            this.Size = new System.Drawing.Size(456, 79);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ListenerBase getCurrentListener()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "getCurrentListener");
            return _listener;
        }


        private void loadConfiguration(XmlNode configuration)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "loadConfiguration(XmlNode)");

            //<listener type="EventLog" ip="127.0.0.1" name="Application" poll="3000" append="1" prefix="EvntApp" />
            cboLogName.Text = _listener.LogName;
            txtHost.Text = _listener.Host;
            chkFieldNames.Checked = _listener.AppendFieldNames;
            txtIntervall.Text = _listener.PollInterval.ToString();
            txtPrefix.Text = _listener.MessagePrefix;
        }


        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "btnRemove_Click");
            Parent.Controls.Remove(this);
        }

        private void txtHost_TextChanged(object sender, EventArgs e)
        {
            _listener.Host = txtHost.Text;
        }

        private void cboLogName_SelectedIndexChanged(object sender, EventArgs e)
        {
            _listener.LogName = cboLogName.Text;
        }

        private void txtIntervall_TextChanged(object sender, EventArgs e)
        {
            int foo;
            if (int.TryParse(txtIntervall.Text, out foo))
            {
                _listener.PollInterval = foo;
            }
        }

        private void chkFieldNames_CheckedChanged(object sender, EventArgs e)
        {
            _listener.AppendFieldNames = chkFieldNames.Checked;
        }

        private void txtPrefix_TextChanged(object sender, EventArgs e)
        {
            _listener.MessagePrefix = txtPrefix.Text;
            Text = txtPrefix.Text;
        }

        private void cboLogName_DropDown_1(object sender, EventArgs e)
        {
            var eventLogs = EventLog.GetEventLogs(txtHost.Text);
            foreach (var eventLog in eventLogs)
            {
                cboLogName.Items.Add(eventLog.Log);
            }
        }

        #region IListenerConfigurator members

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get { return "EventLog Listener: " + _listener.MessagePrefix; }
        }

        /// <summary>
        /// Gets or sets the configuration data for the event listener.
        /// </summary>
        /// <value></value>
        public string Configuration
        {
            get
            {
                if (_log.Enabled) _log.Debug(GetHashCode(), "Configuration Get");
                return _listener.GetConfiguration();
            }
            set
            {
                if (_log.Enabled) _log.Debug(GetHashCode(), "Configuration Set");

                var xs = new XmlSerializer(_listener.GetType());
                var sr = new StringReader(value);
                _listener = (EventLogListener) xs.Deserialize(sr);

                UpdateControls();
            }
        }

        public void UpdateControls()
        {
            txtHost.Text = _listener.Host;
            cboLogName.Text = _listener.LogName;
            txtIntervall.Text = _listener.PollInterval.ToString();
            txtPrefix.Text = _listener.MessagePrefix;
            chkFieldNames.Checked = _listener.AppendFieldNames;

            chkTimestamp.Checked = _listener.ShowTimestamp;
            txtFormat.Text = _listener.TimestampFormat == "" ? ListenerHelper.DefaultTimestampFormat : _listener.TimestampFormat;

        }

        /// <summary>
        /// Gets the listener that has been configured.
        /// </summary>
        /// <value></value>
        public ListenerBase ListenerBase
        {
            get { return getCurrentListener(); }
            set { _listener = (EventLogListener) value; }
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