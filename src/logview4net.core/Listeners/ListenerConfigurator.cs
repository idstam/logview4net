/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace logview4net.Listeners
{
    /// <summary>
    /// This is a dynamic configurator for all listeners.
    /// </summary>
    public partial class ListenerConfigurator : UserControl, IListenerConfigurator
    {
        private ListenerBase _listener;
        private int _rowHeight = 23;
        private int _rowCount = 0;
        private int _nextLeft = 3;
        private int _nextTop = 0;
        private Session _session;

        

        /// <summary>
        /// Initializes a new instance of the <see cref="ListenerConfigurator"/> class.
        /// </summary>
        /// <param name="listener">The listener.</param>
        /// <param name="session">The current session.</param>
        public ListenerConfigurator(ListenerBase listener, Session session)
        {
            _session = session;
            InitializeComponent();
            _listener = listener;

            var fields = _listener.GetConfigValueFields();
            foreach (var name in fields.Keys)
            {
                AddField(name, fields[name]);
            }
            AddField("TimestampBreak", new ListenerConfigField() { Name = "Timestamp break", MultiValueType = MultiValueTypes.Linebreak });
            var tc = AddField("TimestampCheck", new ListenerConfigField() { Name = "Timestamp", MultiValueType = MultiValueTypes.Check, Value = "False" });
            AddField("TimestampFormat", new ListenerConfigField() { Name = "Format", MultiValueType = MultiValueTypes.None, Value = ListenerHelper.DefaultTimestampFormat, Width = 25 });
            setChangedValue(tc, _listener.ShowTimestamp.ToString());

            Height = Controls[Controls.Count - 1].Bottom + 5;

            btnRestart.Visible = _listener.IsRunning && _listener.IsRestartable;
        }

        private Control AddField(string name, ListenerConfigField f)
        {
            _nextTop = _rowCount * _rowHeight + 5;
            Control ret = null;

            var l = new Label();
            l.Enabled = !_listener.IsRunning;
            if ((f.MultiValueType != MultiValueTypes.Linebreak) &&
                (f.MultiValueType != MultiValueTypes.FileOpenButton) &&
                (f.MultiValueType != MultiValueTypes.FolderBrowserButton))
            {
                l.Text = f.Name;
                l.Left = _nextLeft;
                l.Top = _nextTop;
                l.TextAlign = ContentAlignment.MiddleLeft;
                l.AutoSize = true;
                Controls.Add(l);
                l.Visible = true;
                _nextLeft = l.Right + 3;
                if (f.AlignTo != "")
                {
                    if (Controls.ContainsKey(f.AlignTo))
                    {
                        _nextLeft = Controls[f.AlignTo].Left;
                    }
                }
            }
            switch (f.MultiValueType)
            {
                case MultiValueTypes.None:
                    var t = new TextBox();
                    t.Enabled = !_listener.IsRunning;
                    t.Name = name;
                    t.Left = _nextLeft;
                    t.Top = _nextTop;
                    t.Text = f.Value;
                    if (f.Width > -1)
                    {
                        t.Width = calculateWidth(f.Width, t.Font);
                    }
                    Controls.Add(t);
                    t.Visible = true;
                    t.AllowDrop = true;
                    t.TextChanged += new EventHandler(t_TextChanged);
                    t.DragEnter += new DragEventHandler(t_DragEnter);
                    t.DragDrop += new DragEventHandler(t_DragDrop);
                    _nextLeft = t.Right + 3;
                    ret = t;
                    break;
                case MultiValueTypes.Password:
                    var pt = new TextBox();
                    pt.Enabled = !_listener.IsRunning;
                    pt.Name = name;
                    pt.Left = _nextLeft;
                    pt.Top = _nextTop;
                    if (f.Width > -1)
                    {
                        pt.Width = calculateWidth(f.Width, pt.Font);
                    }
                    Controls.Add(pt);
                    pt.Visible = true;
                    pt.PasswordChar = '*';
                    pt.TextChanged += new EventHandler(t_TextChanged);
                    _nextLeft = pt.Right + 3;
                    ret = pt;
                    break;
                case MultiValueTypes.Combo:
                    var c = new ComboBox();
                    c.Enabled = !_listener.IsRunning;
                    c.Name = name;
                    c.Left = _nextLeft;
                    c.DropDownStyle = ComboBoxStyle.DropDownList;
                    c.Items.Clear();
                    c.Items.AddRange(_listener.GetMultiOptions(name).ToArray());
                    var selectedIndex = c.FindString(_listener.GetConfigValue(name));
                    c.SelectedIndex = selectedIndex;
                    c.Top = _nextTop;

                    Controls.Add(c);
                    c.Visible = true;
                    c.SelectedIndexChanged += new EventHandler(cbo_SelectedIndexChanged);
                    _nextLeft = c.Right + 3;
                    ret = c;
                    break;
                case MultiValueTypes.Options:
                    break;
                case MultiValueTypes.Linebreak:
                    _nextLeft = 4;
                    _rowCount++;
                    break;
                case MultiValueTypes.Check:
                    var chk = new CheckBox();
                    //chk.Enabled = !_listener.IsRunning;
                    chk.AutoSize = true;
                    chk.Name = name;
                    chk.Text = "";
                    chk.Left = _nextLeft;
                    chk.Top = _nextTop + 3;

                    chk.Height = l.Height;
                    Controls.Add(chk);
                    chk.Visible = true;
                    chk.CheckedChanged += new EventHandler(chk_CheckedChanged);
                    _nextLeft = chk.Right + 6;
                    ret = chk;
                    break;
                case MultiValueTypes.FileOpenButton:
                    var btnFO = new Button();
                    btnFO.Enabled = !_listener.IsRunning;
                    btnFO.Name = name;
                    btnFO.Text = "...";
                    btnFO.Width = 24;
                    btnFO.Height = 22;
                    btnFO.Left = _nextLeft;
                    btnFO.Top = _nextTop - 1;
                    btnFO.FlatStyle = FlatStyle.Standard;
                    Controls.Add(btnFO);
                    btnFO.Visible = true;
                    btnFO.Click += new EventHandler(btnFO_Click);
                    _nextLeft = btnFO.Right + 3;
                    ret = btnFO;
                    break;
                case MultiValueTypes.FolderBrowserButton:
                    var btnDir = new Button();
                    btnDir.Enabled = !_listener.IsRunning;
                    btnDir.Name = name;
                    btnDir.Text = "...";
                    btnDir.Width = 24;
                    btnDir.Height = 22;
                    btnDir.Left = _nextLeft;
                    btnDir.Top = _nextTop - 1;
                    btnDir.FlatStyle = FlatStyle.Standard;
                    btnDir.BackColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    Controls.Add(btnDir);
                    btnDir.Visible = true;
                    btnDir.Click += new EventHandler(btnDir_Click);
                    _nextLeft = btnDir.Right + 3;
                    ret = btnDir;
                    break;
                default:
                    break;
            }

            return ret;
        }

        void btnDir_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var pos = Controls.IndexOf(btn);
                folderBrowserDialog1.ShowDialog();
                var folderName = folderBrowserDialog1.SelectedPath;
                if (folderName != "")
                {
                    setChangedValue(btn, folderName);
                }
            }
        }

        private int calculateWidth(int charCount, Font f)
        {
            if (this.ParentForm == null) return 100; //This happens during testing.

            using (var g = CreateGraphics())
            {
                var foo = new string('X', charCount);
                return (int)g.MeasureString(foo, f).Width;
            }
        }


        void btnFO_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var pos = Controls.IndexOf(btn);
                openFileDialog1.FileName = "";
                openFileDialog1.ReadOnlyChecked = true;
                openFileDialog1.ShowReadOnly = true;
                openFileDialog1.CheckFileExists = false;
                openFileDialog1.ValidateNames = false;
                if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
                {
                    var fileName = openFileDialog1.FileName;
                    if (fileName != "")
                    {
                        setChangedValue(btn, fileName);
                    }
                }
            }
        }

        void chk_CheckedChanged(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;
            if (chk != null)
            {
                setChangedValue(chk, chk.Checked.ToString());
            }
        }

        void cbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbo = sender as ComboBox;
            if (cbo != null)
            {
                setChangedValue(cbo, cbo.Text);
            }
        }
        void t_TextChanged(object sender, EventArgs e)
        {
            var txt = sender as TextBox;
            if (txt != null)
            {
                setChangedValue(txt, txt.Text);
            }
        }
        void t_DragEnter(object sender, DragEventArgs e)
        {
        	if(e.Data.GetDataPresent(DataFormats.FileDrop))
        	{
        		e.Effect = DragDropEffects.Copy;
        	}
        	else
        	{
        		e.Effect = DragDropEffects.None;
        		
        	}
        }
		
        void t_DragDrop(object sender, DragEventArgs e)
        {
        	var FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
        	((TextBox)sender).Text = FileList[0];

        }
        void setChangedValue(Control ctl, string value)
        {
            if (ctl.Name == "TimestampCheck")
            {
                var ctls = Controls.Find("TimestampFormat", false);
                if (ctls.Length > 0)
                {
                    ctls[0].Enabled = bool.Parse(value);
                }
                _listener.ShowTimestamp = bool.Parse(value);
                return;
            }
            if (ctl.Name == "TimestampFormat")
            {
                _listener.TimestampFormat = value;
                return;
            }

            var error = _listener.SetConfigValue(ctl.Name, value);
            if (error != null)
            {
                err.SetIconAlignment(ctl, ErrorIconAlignment.MiddleLeft);
                err.SetError(ctl, error);
            }
            else
            {
                err.Clear();

                switch (ctl.Name)
                {
                    case "prefix":
                        Text = value;
                        break;
                }
            }
            UpdateControls();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateControls()
        {
            var configFields = _listener.GetConfigValueFields();
            foreach (var fieldName in configFields.Keys)
            {
                var f = configFields[fieldName];
                switch (f.MultiValueType)
                {
                    case MultiValueTypes.None:
                        Controls[fieldName].Text = _listener.GetConfigValue(fieldName);
                        break;
                    case MultiValueTypes.Password:
                        Controls[fieldName].Text = _listener.GetConfigValue(fieldName);
                        break;
                    case MultiValueTypes.Combo:
                        var cbox = Controls[fieldName] as ComboBox;
                        cbox.SelectedIndex = cbox.FindStringExact(_listener.GetConfigValue(fieldName));

                        break;
                    case MultiValueTypes.Options:
                        break;
                    case MultiValueTypes.Linebreak:
                        break;
                    case MultiValueTypes.FileOpenButton:
                        break;
                    case MultiValueTypes.Check:
                        var foo = true;

                        bool.TryParse(_listener.GetConfigValue(fieldName), out foo);

                        ((CheckBox)Controls[fieldName]).Checked = foo;

                        break;
                    default:
                        break;
                }
            }

            ((CheckBox)Controls["TimestampCheck"]).Checked = _listener.ShowTimestamp;
            ((TextBox)Controls["TimestampFormat"]).Text = _listener.TimestampFormat == "" ? ListenerHelper.DefaultTimestampFormat : _listener.TimestampFormat;
        }


        #region IListenerConfigurator Members

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get { return _listener.GetType().Name + ": " + _listener.MessagePrefix; }
        }

        /// <summary>
        /// Gets or sets the configuration data for this configurator.
        /// </summary>
        /// <value></value>
        public string Configuration
        {
            get
            {
                return _listener.GetConfiguration();
            }
            set
            {
                var xs = new XmlSerializer(_listener.GetType());
                var sr = new StringReader(value);
                _listener = (ListenerBase)xs.Deserialize(sr);
                _listener.IsConfigured = true;
                UpdateControls();
            }
        }

        /// <summary>
        /// Gets the listener for an implementation of this interface
        /// </summary>
        /// <value></value>
        public ListenerBase ListenerBase
        {
            get
            {
                return _listener;
            }
            set
            {
                _listener = value;
            }
        }

        #endregion

        private void btnRestart_Click(object sender, EventArgs e)
        {
            try
            {
                _session.AddEvent(_listener, "Restarting listener....");
                _listener.Stop();
                _listener.Start();
            }
            catch (Exception ex)
            {
                _session.AddEvent(_listener, ex.Message);
            }
        }
    }
}
