/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;
using logview4net.Controls;
using logview4net.Listeners;

namespace logview4net.Viewers
{
    /// <summary>
    ///     Summary description for rtfLogViewer.
    /// </summary>
    [Serializable]
    public class TextViewer : UserControl, IViewer, ISerializable
    {
        private readonly ILog _log = Logger.GetLogger("logview4net.Viewers.textViewer");

        /// <summary>
        ///     This is where all events are stored while the view is paused.
        /// </summary>
        protected Queue<LogEvent> PauseCache = new Queue<LogEvent>();

        /// <summary>
        ///     The textbox containing the log events to be viewed.
        /// </summary>
        public AdvRichTextBox Txt;

        /// <summary>
        ///     The list of <see cref="Action" /> that are registered for the current <see cref="Session" />.
        /// </summary>
        protected List<Action> _actions = new List<Action>();

        private ToolStripMenuItem _addHideActionToolStripMenuItem;
        private ToolStripMenuItem _addHighlightActionToolStripMenuItem;
        private ToolStripMenuItem _addHighlightmatchActionToolStripMenuItem;

        private ToolStripMenuItem _addIgnoreActionToolStripMenuItem;
        private ToolStripMenuItem _addPopupActionToolStripMenuItem;
        private ToolStripMenuItem _blueToolStripMenuItem;
        private ToolStripMenuItem _blueToolStripMenuItem1;

        /// <summary>
        ///     The max amount of characters to have in the textbox
        /// </summary>
        protected int _bufferSize = 1024*100;

        /// <summary>
        ///     Whether or not to cache events when the viewer is pa
        /// </summary>
        protected bool _cacheOnPause = true;

        private ToolStripMenuItem _cyanToolStripMenuItem;

        private ToolStripMenuItem _cyanToolStripMenuItem1;
        private ToolStripMenuItem _findToolStripMenuItem;

        /// <summary>
        ///     Color of the text in the textbox.
        /// </summary>
        protected Color _foreColor;

        private int _lastProgressValue = -1;
        private ToolStripMenuItem _limeToolStripMenuItem;

        private ToolStripMenuItem _limeToolStripMenuItem1;
        private ToolStripMenuItem _magentaToolStripMenuItem;
        private ToolStripMenuItem _magentaToolStripMenuItem1;
        private ContextMenuStrip _mnuPop;
        private ToolStripMenuItem _orangeToolStripMenuItem;
        private ToolStripMenuItem _orangeToolStripMenuItem1;

        /// <summary>
        ///     Whether or not the viewer is paused
        /// </summary>
        protected bool _paused = false;

        private ProgressBar _progress;
        private ToolStripMenuItem _redToolStripMenuItem;
        private ToolStripMenuItem _redToolStripMenuItem1;
        private SaveFileDialog _saveFileDialog1;
        private ToolStripMenuItem _saveToFileToolStripMenuItem;
        private ToolStripMenuItem _showHiddenToolStripMenuItem;
        private ToolStripSeparator _toolStripSeparator1;

        /// <summary>
        ///     Whether or not the viewer does word wrapping
        /// </summary>
        protected bool _wordWrap = false;

        private ToolStripMenuItem _yellowToolStripMenuItem;
        private ToolStripMenuItem _yellowToolStripMenuItem1; // Scrolls to the right

        private IContainer components;

        /// <summary>
        ///     Creates a new <see cref="TextViewer" /> instance.
        /// </summary>
        public TextViewer()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "textViewer()");
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            _foreColor = Txt.ForeColor;
            // TODO: Add any initialization after the InitializeComponent call
        }


        /// <summary>
        ///     Gets or sets the fore color.
        /// </summary>
        /// <value></value>
        public Color TextForeColor
        {
            get { return Txt.ForeColor; }
            set { Txt.ForeColor = value; }
        }

        /// <summary>
        ///     Gets or sets the back color.
        /// </summary>
        /// <value></value>
        public Color TextBackColor
        {
            get { return Txt.BackColor; }
            set { Txt.BackColor = value; }
        }

        /// <summary>
        ///     Gets or sets the font.
        /// </summary>
        /// <value></value>
        public Font TextFont
        {
            get { return Txt.Font; }
            set { Txt.Font = value; }
        }

        /// <summary>
        ///     Whether or not to remove surrounding white space.
        /// </summary>
        public bool RemoveWhitespace { get; set; }

        /// <summary>
        ///     Whether or not to show the listener prefix
        /// </summary>
        public bool ShowListenerPrefix { get; set; }

        /// <summary>
        ///     Gets or sets the size of the buffer.
        /// </summary>
        /// <value></value>
        public int BufferSize
        {
            get { return _bufferSize; }
            set { _bufferSize = value; }
        }

        /// <summary>
        ///     The filename for saving events to disk
        /// </summary>
        public string LogFile { get; set; }

        /// <summary>
        ///     When to 'roll' the log file
        /// </summary>
        public string LogRolling { get; set; }

        /// <summary>
        ///     Wether or not to save events to disk.
        /// </summary>
        public bool LogToFile { get; set; }

        /// <summary>
        ///     Displays all hidden messages.
        /// </summary>
        public void ShowAllHidden()
        {
            foreach (Action a in Actions)
            {
                if (a.ActionType == ActionTypes.Hide && a.HideCache.Count > 0)
                {
                    foreach (string msg in a.HideCache)
                    {
                        var le = new LogEvent(msg, Actions, true);
                        InvokedAddEvent(le, true);
                    }
                    a.HideCache.Clear();
                }
            }
            RaiseHasHiddenMessages(HasHiddenMessages());
        }

        /// <summary>
        ///     An event used to toggle the 'Hidden' button in the toolbar
        /// </summary>
        public event EventHandler HasHiddenMessagesEvent;

        /// <summary>
        ///     Clean up any resources being used.
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

        private void EnforceBufferSize()
        {
            Txt.SelectionStart = Txt.TextLength;
            if (Txt.SelectionStart > BufferSize)
            {
                int toRemove = BufferSize / 10 ;
                //Txt.BeginUpdate();
                Txt.SelectionStart = 0;
                Txt.SelectionLength = toRemove;
                Txt.SelectedText = "";
                //Txt.EndUpdate();
                EnforceBufferSize();
                _log.Debug(GetHashCode(), "Truncated viewer");
            }
            Txt.SelectionStart = int.MaxValue - 1;
            ParentForm.Text = Txt.TextLength.ToString(CultureInfo.InvariantCulture);
        }

        private void SetPaused(bool paused)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "SetPaused");

            if (paused)
            {
                var a = new List<Action>();
                a.Add(Action.CreateHighlightAction("", Color.Silver));
                AddEvent(new LogEvent("----- PAUSED -----", a, false));
            }
            _paused = paused;

            if (!paused)
            {
                Txt.BeginUpdate();
                int pos = 0;
                _progress.Visible = true;
                int max = PauseCache.Count;
                while (PauseCache.Count > 0)
                {
                    pos++;
                    SetProgress(pos, max);

                    InvokedAddEvent(PauseCache.Dequeue(), false);
                }
                var a = new List<Action>();
                a.Add(Action.CreateHighlightAction("", Color.Silver));
                AddEvent(new LogEvent("----- UNPAUSED -----", a, false));
                Txt.EndUpdate();
                Refresh();
                Application.DoEvents();
                _progress.Visible = false;
            }
        }

        private void SetWordWrap(bool wordWrap)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "SetWordWrap");

            _wordWrap = wordWrap;

            Txt.WordWrap = wordWrap;
        }

        private void mnuPop_Opening(object sender, CancelEventArgs e)
        {
            bool foo = (Txt.SelectedText.Length > 0);
            _addIgnoreActionToolStripMenuItem.Visible = foo;
            _addPopupActionToolStripMenuItem.Visible = foo;
            _addHighlightActionToolStripMenuItem.Visible = foo;
            _addHighlightmatchActionToolStripMenuItem.Visible = foo;
            _addHideActionToolStripMenuItem.Visible = foo;
            _toolStripSeparator1.Visible = foo;

            if (_showHiddenToolStripMenuItem.HasDropDownItems)
            {
                _showHiddenToolStripMenuItem.DropDownItems.Clear();
            }
            foreach (Action a in Actions)
            {
                if (a.ActionType == ActionTypes.Hide && a.HideCache.Count > 0)
                {
                    ToolStripItem itm = _showHiddenToolStripMenuItem.DropDownItems.Add(a.Pattern);
                    itm.Tag = a;
                    itm.Click += mnuHideAction_Click;
                }
            }
            _showHiddenToolStripMenuItem.Enabled = _showHiddenToolStripMenuItem.HasDropDownItems;
        }

        private void mnuHideAction_Click(object sender, EventArgs e)
        {
            var itm = (ToolStripItem) sender;
            var a = (Action) itm.Tag;
            foreach (string msg in a.HideCache)
            {
                var le = new LogEvent(msg, Actions, true);
                InvokedAddEvent(le, true);
            }
            a.HideCache.Clear();
            RaiseHasHiddenMessages(HasHiddenMessages());
        }

        private void addIgnoreActionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Action a = Action.CreateIgnoreAction(Txt.SelectedText);
            AddAction(a);
        }

        private void addHideActionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Action a = Action.CreateHideAction(Txt.SelectedText);
            AddAction(a);
        }

        private void addPopupActionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Action a = Action.CreatePopupAction(Txt.SelectedText);
            AddAction(a);
        }

        private void colorHighlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mi = sender as ToolStripMenuItem;
            string colorName = mi.Text.Replace("&", "");
            Color c = Color.FromName(colorName);

            Action a = Action.CreateHighlightAction(Txt.SelectedText, c);
            AddAction(a);
        }

        private void colorHighlightMatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mi = sender as ToolStripMenuItem;
            string colorName = mi.Text.Replace("&", "");
            Color c = Color.FromName(colorName);

            Action a = Action.CreateHighlightMatchAction(Txt.SelectedText, c);
            AddAction(a);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFindDialog();
        }

        /// <summary>
        ///     Shows the find dialog.
        /// </summary>
        public void ShowFindDialog()
        {
            var sf = new SearchForm(Txt);
            sf.Show(this);
            sf.TopMost = true;
        }

        private void txt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.F))
            {
                ShowFindDialog();
            }
        }

        private void restartSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Session s = ((ViewerForm) ParentForm).Session;

            foreach (IListener l in s.Listeners)
            {
                l.Stop();
                l.Start();
            }
        }

        /// <summary>
        ///     Rasise the HasHiddenMessagesEvent event
        /// </summary>
        /// <param name="hasHiddenMessages"></param>
        public void RaiseHasHiddenMessages(bool hasHiddenMessages)
        {
            if (HasHiddenMessagesEvent != null)
            {
                var e = new HasHiddenMessagesEventArgs(hasHiddenMessages);
                HasHiddenMessagesEvent(this, e);
            }
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "saveToFileToolStripMenuItem_Click");
            //saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //            saveFileDialog1.DefaultExt = ".txt";
            _saveFileDialog1.ShowDialog(this);
            if (_saveFileDialog1.FileName != "")
            {
                if (_saveFileDialog1.FilterIndex == 2)
                {
                    Txt.SaveFile(_saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                }
                else
                {
                    Txt.SaveFile(_saveFileDialog1.FileName, RichTextBoxStreamType.UnicodePlainText);
                }
            }
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //<viewer type = "Text" buffer="5000" forecolor="Lime" backcolor="Black" font="Courier New" fontsize="9" >

            info.AddValue("buffer", _bufferSize);
            info.AddValue("forecolor", Txt.ForeColor.Name);
            info.AddValue("backcolor", Txt.BackColor);
            info.AddValue("font", Txt.Font.Name);
            info.AddValue("fontsize", Txt.Font.Size);
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._mnuPop = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._addIgnoreActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._addHideActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._addPopupActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._addHighlightActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._cyanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._limeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._magentaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._orangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._redToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._yellowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._addHighlightmatchActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._blueToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._cyanToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._limeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._magentaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._orangeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._redToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._yellowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._showHiddenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._saveToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._progress = new System.Windows.Forms.ProgressBar();
            this._saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.Txt = new AdvRichTextBox();
            this._mnuPop.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuPop
            // 
            this._mnuPop.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this._addIgnoreActionToolStripMenuItem,
                this._addHideActionToolStripMenuItem,
                this._addPopupActionToolStripMenuItem,
                this._addHighlightActionToolStripMenuItem,
                this._addHighlightmatchActionToolStripMenuItem,
                this._toolStripSeparator1,
                this._findToolStripMenuItem,
                this._showHiddenToolStripMenuItem,
                this._saveToFileToolStripMenuItem
            });
            this._mnuPop.Name = "_mnuPop";
            this._mnuPop.Size = new System.Drawing.Size(223, 186);
            this._mnuPop.Opening += new System.ComponentModel.CancelEventHandler(this.mnuPop_Opening);
            // 
            // addIgnoreActionToolStripMenuItem
            // 
            this._addIgnoreActionToolStripMenuItem.Name = "addIgnoreActionToolStripMenuItem";
            this._addIgnoreActionToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._addIgnoreActionToolStripMenuItem.Text = "Add &Ignore action";
            this._addIgnoreActionToolStripMenuItem.Click +=
                new System.EventHandler(this.addIgnoreActionToolStripMenuItem_Click);
            // 
            // addHideActionToolStripMenuItem
            // 
            this._addHideActionToolStripMenuItem.Name = "addHideActionToolStripMenuItem";
            this._addHideActionToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._addHideActionToolStripMenuItem.Text = "Add Hide action";
            this._addHideActionToolStripMenuItem.Click +=
                new System.EventHandler(this.addHideActionToolStripMenuItem_Click);
            // 
            // addPopupActionToolStripMenuItem
            // 
            this._addPopupActionToolStripMenuItem.Name = "addPopupActionToolStripMenuItem";
            this._addPopupActionToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._addPopupActionToolStripMenuItem.Text = "Add &Popup action";
            this._addPopupActionToolStripMenuItem.Click +=
                new System.EventHandler(this.addPopupActionToolStripMenuItem_Click);
            // 
            // addHighlightActionToolStripMenuItem
            // 
            this._addHighlightActionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this._blueToolStripMenuItem,
                this._cyanToolStripMenuItem,
                this._limeToolStripMenuItem,
                this._magentaToolStripMenuItem,
                this._orangeToolStripMenuItem,
                this._redToolStripMenuItem,
                this._yellowToolStripMenuItem
            });
            this._addHighlightActionToolStripMenuItem.Name = "addHighlightActionToolStripMenuItem";
            this._addHighlightActionToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._addHighlightActionToolStripMenuItem.Text = "Add &Highlight action";
            // 
            // blueToolStripMenuItem
            // 
            this._blueToolStripMenuItem.Name = "blueToolStripMenuItem";
            this._blueToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this._blueToolStripMenuItem.Text = "&Blue";
            this._blueToolStripMenuItem.Click += new System.EventHandler(this.colorHighlightToolStripMenuItem_Click);
            // 
            // cyanToolStripMenuItem
            // 
            this._cyanToolStripMenuItem.Name = "cyanToolStripMenuItem";
            this._cyanToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this._cyanToolStripMenuItem.Text = "&Cyan";
            this._cyanToolStripMenuItem.Click += new System.EventHandler(this.colorHighlightToolStripMenuItem_Click);
            // 
            // limeToolStripMenuItem
            // 
            this._limeToolStripMenuItem.Name = "limeToolStripMenuItem";
            this._limeToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this._limeToolStripMenuItem.Text = "&Lime";
            this._limeToolStripMenuItem.Click += new System.EventHandler(this.colorHighlightToolStripMenuItem_Click);
            // 
            // magentaToolStripMenuItem
            // 
            this._magentaToolStripMenuItem.Name = "magentaToolStripMenuItem";
            this._magentaToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this._magentaToolStripMenuItem.Text = "&Magenta";
            this._magentaToolStripMenuItem.Click += new System.EventHandler(this.colorHighlightToolStripMenuItem_Click);
            // 
            // orangeToolStripMenuItem
            // 
            this._orangeToolStripMenuItem.Name = "orangeToolStripMenuItem";
            this._orangeToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this._orangeToolStripMenuItem.Text = "&Orange";
            this._orangeToolStripMenuItem.Click += new System.EventHandler(this.colorHighlightToolStripMenuItem_Click);
            // 
            // redToolStripMenuItem
            // 
            this._redToolStripMenuItem.Name = "redToolStripMenuItem";
            this._redToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this._redToolStripMenuItem.Text = "&Red";
            this._redToolStripMenuItem.Click += new System.EventHandler(this.colorHighlightToolStripMenuItem_Click);
            // 
            // yellowToolStripMenuItem
            // 
            this._yellowToolStripMenuItem.Name = "yellowToolStripMenuItem";
            this._yellowToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this._yellowToolStripMenuItem.Text = "&Yellow";
            this._yellowToolStripMenuItem.Click += new System.EventHandler(this.colorHighlightToolStripMenuItem_Click);
            // 
            // addHighlightmatchActionToolStripMenuItem
            // 
            this._addHighlightmatchActionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem
                []
            {
                this._blueToolStripMenuItem1,
                this._cyanToolStripMenuItem1,
                this._limeToolStripMenuItem1,
                this._magentaToolStripMenuItem1,
                this._orangeToolStripMenuItem1,
                this._redToolStripMenuItem1,
                this._yellowToolStripMenuItem1
            });
            this._addHighlightmatchActionToolStripMenuItem.Name = "addHighlightmatchActionToolStripMenuItem";
            this._addHighlightmatchActionToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._addHighlightmatchActionToolStripMenuItem.Text = "Add Highlight &match action";
            // 
            // blueToolStripMenuItem1
            // 
            this._blueToolStripMenuItem1.Name = "blueToolStripMenuItem1";
            this._blueToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this._blueToolStripMenuItem1.Text = "&Blue";
            this._blueToolStripMenuItem1.Click +=
                new System.EventHandler(this.colorHighlightMatchToolStripMenuItem_Click);
            // 
            // cyanToolStripMenuItem1
            // 
            this._cyanToolStripMenuItem1.Name = "cyanToolStripMenuItem1";
            this._cyanToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this._cyanToolStripMenuItem1.Text = "&Cyan";
            this._cyanToolStripMenuItem1.Click +=
                new System.EventHandler(this.colorHighlightMatchToolStripMenuItem_Click);
            // 
            // limeToolStripMenuItem1
            // 
            this._limeToolStripMenuItem1.Name = "limeToolStripMenuItem1";
            this._limeToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this._limeToolStripMenuItem1.Text = "&Lime";
            this._limeToolStripMenuItem1.Click +=
                new System.EventHandler(this.colorHighlightMatchToolStripMenuItem_Click);
            // 
            // magentaToolStripMenuItem1
            // 
            this._magentaToolStripMenuItem1.Name = "magentaToolStripMenuItem1";
            this._magentaToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this._magentaToolStripMenuItem1.Text = "&Magenta";
            this._magentaToolStripMenuItem1.Click +=
                new System.EventHandler(this.colorHighlightMatchToolStripMenuItem_Click);
            // 
            // orangeToolStripMenuItem1
            // 
            this._orangeToolStripMenuItem1.Name = "orangeToolStripMenuItem1";
            this._orangeToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this._orangeToolStripMenuItem1.Text = "&Orange";
            this._orangeToolStripMenuItem1.Click +=
                new System.EventHandler(this.colorHighlightMatchToolStripMenuItem_Click);
            // 
            // redToolStripMenuItem1
            // 
            this._redToolStripMenuItem1.Name = "redToolStripMenuItem1";
            this._redToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this._redToolStripMenuItem1.Text = "&Red";
            this._redToolStripMenuItem1.Click += new System.EventHandler(this.colorHighlightMatchToolStripMenuItem_Click);
            // 
            // yellowToolStripMenuItem1
            // 
            this._yellowToolStripMenuItem1.Name = "yellowToolStripMenuItem1";
            this._yellowToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this._yellowToolStripMenuItem1.Text = "&Yellow";
            this._yellowToolStripMenuItem1.Click +=
                new System.EventHandler(this.colorHighlightMatchToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this._toolStripSeparator1.Name = "toolStripSeparator1";
            this._toolStripSeparator1.Size = new System.Drawing.Size(219, 6);
            // 
            // findToolStripMenuItem
            // 
            this._findToolStripMenuItem.Name = "findToolStripMenuItem";
            this._findToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._findToolStripMenuItem.Text = "&Find";
            this._findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // showHiddenToolStripMenuItem
            // 
            this._showHiddenToolStripMenuItem.Enabled = false;
            this._showHiddenToolStripMenuItem.Name = "showHiddenToolStripMenuItem";
            this._showHiddenToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._showHiddenToolStripMenuItem.Text = "Show hidden";
            // 
            // saveToFileToolStripMenuItem
            // 
            this._saveToFileToolStripMenuItem.Name = "saveToFileToolStripMenuItem";
            this._saveToFileToolStripMenuItem.ShortcutKeys =
                ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this._saveToFileToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._saveToFileToolStripMenuItem.Text = "Save to file ...";
            this._saveToFileToolStripMenuItem.Click += new System.EventHandler(this.saveToFileToolStripMenuItem_Click);
            // 
            // progress
            // 
            this._progress.BackColor = System.Drawing.Color.Black;
            this._progress.Dock = System.Windows.Forms.DockStyle.Top;
            this._progress.Location = new System.Drawing.Point(0, 0);
            this._progress.Maximum = 400;
            this._progress.Name = "_progress";
            this._progress.Size = new System.Drawing.Size(372, 23);
            this._progress.TabIndex = 1;
            this._progress.Visible = false;
            // 
            // saveFileDialog1
            // 
            this._saveFileDialog1.Filter = "Text files (*.txt)|*.txt|Rich text files (*.rtf)|*.rtf";
            // 
            // txt
            // 
            this.Txt.BackColor = System.Drawing.Color.Black;
            this.Txt.ContextMenuStrip = this._mnuPop;
            this.Txt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Txt.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Txt.ForeColor = System.Drawing.Color.Lime;
            this.Txt.HideSelection = false;
            this.Txt.Location = new System.Drawing.Point(0, 0);
            this.Txt.Name = "Txt";
            this.Txt.SelectionAlignment = TextAlign.Left;
            this.Txt.Size = new System.Drawing.Size(372, 273);
            this.Txt.TabIndex = 0;
            this.Txt.Text = "";
            this.Txt.WordWrap = false;
            this.Txt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_KeyUp);
            // 
            // textViewer
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this._progress);
            this.Controls.Add(this.Txt);
            this.Name = "TextViewer";
            this.Size = new System.Drawing.Size(372, 273);
            this._mnuPop.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        #region IViewer Members

        /// <summary>
        ///     List of actions for the listener.
        /// </summary>
        public List<Action> Actions
        {
            get { return _actions; }
        }

        /// <summary>
        ///     Whether or not to cache events when the viewer is paused
        /// </summary>
        public bool CacheOnPause
        {
            get { return _cacheOnPause; }
            set { _cacheOnPause = value; }
        }

        /// <summary>
        ///     Adds a list of events to this viewer
        /// </summary>
        public void AddEvent(string prefix, List<string> lines, IListener listener)
        {
            var a = new AddEventsAsynch(InvokedAddEvents);
            Invoke(a, new object[] {prefix, lines});
        }

        /// <summary>
        ///     Adds an event to show
        /// </summary>
        /// <param name="message">The message to show.</param>
        /// <param name="listener">The listener that received the data</param>
        public void AddEvent(string message, IListener listener)
        {
            var le = new LogEvent(message, _actions, true);
            try
            {
                AddEvent(le);
            }
            catch (Exception ex)
            {
                throw ExceptionManager.HandleException(GetHashCode(), ex);
            }
        }

        /// <summary>
        ///     Indicates the existence of hidden messages.
        /// </summary>
        /// <returns></returns>
        public bool HasHiddenMessages()
        {
            bool hasMessages = false;
            foreach (Action a in Actions)
            {
                if (a.ActionType == ActionTypes.Hide && a.HideCache.Count > 0)
                {
                    hasMessages = true;
                    break;
                }
            }
            return hasMessages;
        }

        /// <summary>
        ///     Clears this instance.
        /// </summary>
        public void Clear()
        {
            Txt.Text = "";
        }


        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="TextViewer" /> is paused.
        /// </summary>
        /// <value>
        ///     <c>true</c> if paused; otherwise, <c>false</c>.
        /// </value>
        public bool Paused
        {
            get { return _paused; }
            set { SetPaused(value); }
        }

        /// <summary>
        ///     Wether or not to wordwrap
        /// </summary>
        public bool WordWrap
        {
            get { return _wordWrap; }
            set { SetWordWrap(value); }
        }

        /// <summary>
        ///     Gets the configuration.
        /// </summary>
        /// <returns></returns>
        public string GetConfiguration()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "GetConfiguration");
            var x = new XmlDocument();
            //<viewer type = "Text" buffer="5000" forecolor="Lime" backcolor="Black" font="Courier New" fontsize="9" >
            XmlElement viewer = x.CreateElement("viewer");
            x.AppendChild(viewer);

            XmlAttribute a = x.CreateAttribute("type");
            a.InnerText = "Text";
            viewer.Attributes.Append(a);

            a = x.CreateAttribute("buffer");
            a.InnerText = _bufferSize.ToString();
            viewer.Attributes.Append(a);

            a = x.CreateAttribute("forecolor");
            a.InnerText = Txt.ForeColor.Name;
            viewer.Attributes.Append(a);

            a = x.CreateAttribute("backcolor");
            a.InnerText = Txt.BackColor.Name;
            viewer.Attributes.Append(a);

            a = x.CreateAttribute("font");
            a.InnerText = Txt.Font.Name;
            viewer.Attributes.Append(a);

            a = x.CreateAttribute("fontsize");
            a.InnerText = Txt.Font.Size.ToString(CultureInfo.InvariantCulture);
            viewer.Attributes.Append(a);

            a = x.CreateAttribute("cacheOnPause");
            a.InnerText = _cacheOnPause.ToString();
            viewer.Attributes.Append(a);

            a = x.CreateAttribute("logFileName");
            a.InnerText = LogFile;
            viewer.Attributes.Append(a);

            a = x.CreateAttribute("logRoll");
            a.InnerText = LogRolling;
            viewer.Attributes.Append(a);

            a = x.CreateAttribute("useLog");
            a.InnerText = LogToFile.ToString();
            viewer.Attributes.Append(a);

            foreach (Action action in _actions)
            {
                XmlNode actionNode = x.CreateElement("action");
                a = x.CreateAttribute("type");
                a.Value = action.ActionType.ToString();
                actionNode.Attributes.Append(a);

                a = x.CreateAttribute("pattern");
                a.Value = action.Pattern;
                actionNode.Attributes.Append(a);

                a = x.CreateAttribute("data");
                a.Value = action.Data;
                actionNode.Attributes.Append(a);

                a = CreateActionFormatAttributes(action, actionNode);

                viewer.AppendChild(actionNode);
            }

            return x.OuterXml;
        }

        private void AddEvent(LogEvent le)
        {
            var a = new AddEventAsynch(InvokedAddEvent);
            Invoke(a, new object[] {le, false});
        }

        /// <summary>
        ///     Adds the event.
        /// </summary>
        private void InvokedAddEvents(string prefix, List<string> lines)
        {
            if (! _paused && lines.Count > 100)
            {
                _progress.Visible = true;

                Refresh();
                Application.DoEvents();

                Txt.BeginUpdate();
            }

            int pos = 0;

            foreach (var line in lines)
            {
                var le = new LogEvent(prefix + line, _actions, true);
                pos++;
                SetProgress(pos, lines.Count);
                try
                {
                    InvokedAddEvent(le, false);
                }
                catch (Exception ex)
                {
                    throw ExceptionManager.HandleException(GetHashCode(), ex);
                }
            }

            if (! _paused && lines.Count > 100)
            {
                Txt.EndUpdate();
                Refresh();
                Application.DoEvents();
            }
            ResetProgress();
        }

        private void ResetProgress()
        {
            _progress.Visible = false;
            _lastProgressValue = -1;
            _progress.Value = 0;
        }

        private void SetProgress(int current, int max)
        {
            if (! _paused && max > 100)
            {
                if (!_progress.Visible)
                {
                    _progress.Visible = true;
                }
                float fvalue = current/(float) max;
                var value = (int) (fvalue*_progress.Maximum);
                if (value != _lastProgressValue)
                {
                    if (value > _progress.Maximum) value = _progress.Maximum;
                    _progress.Value = value;
                    Text = value.ToString();
                    Refresh();
                    Application.DoEvents();
                    _lastProgressValue = value;
                }
            }
        }

        /// <summary>
        ///     Adds the event.
        /// </summary>
        private void InvokedAddEvent(LogEvent le, bool forceShow)
        {
            if (RemoveWhitespace)
            {
                le.Message = le.Message.Trim();
            }

            if (_paused)
            {
                if (_cacheOnPause)
                {
                    PauseCache.Enqueue(le);
                }
                return;
            }

            try
            {
                var reason = ViewerUtils.IgnoreReasons.DoNotIgnore;
                var ignoreMessage = false;
                if (!forceShow)
                {
                    ignoreMessage = ViewerUtils.IgnoreEvent(le, out reason);
                }

                if (ignoreMessage)
                {
                    switch (reason)
                    {
                        case ViewerUtils.IgnoreReasons.StartedBlock:
                            Txt.AppendText("----- STARTED IGNORING -----" + Environment.NewLine);
                            break;
                        case ViewerUtils.IgnoreReasons.EndedBlock:
                            Txt.AppendText("----- ENDED IGNORING -----" + Environment.NewLine);
                            break;
                        case ViewerUtils.IgnoreReasons.Hide:
                            //The message was cached in ViewerUtils.IgnoreEvent
                            RaiseHasHiddenMessages(HasHiddenMessages());
                            break;
                    }
                }

                if (ignoreMessage) return;
                
                if (le.Actions.Count > 0)
                {
                    Txt.SelectionStart = int.MaxValue - 1;
                    int selectionStart = Txt.SelectionStart;
                    Txt.SelectedText = le.Message + Environment.NewLine;
                    //txt.AppendText(le.Message + Environment.NewLine);

                    foreach (Action action in le.Actions)
                    {
                        switch (action.ActionType)
                        {
                            case ActionTypes.Highlight:
                                ExecHighlight(action, le, selectionStart);
                                break;
                            case ActionTypes.HighlightMatch:
                                ExecHighlightMatch(action, le, selectionStart);
                                break;
                            case ActionTypes.PopUp:
                                ViewerUtils.ExecuteNonViewerAction(action, le.Message);
                                break;
                            case ActionTypes.PlaySound:
                                ViewerUtils.ExecuteNonViewerAction(action, le.Message);
                                break;
                            case ActionTypes.Execute:
                                ViewerUtils.ExecuteNonViewerAction(action, le.Message);
                                break;
                        }
                    }
                }
                else
                {
                    Txt.SelectionStart = int.MaxValue - 1;
                    Txt.SelectedText = le.Message + Environment.NewLine;
                }

                EnforceBufferSize();

                RaiseHasHiddenMessages(HasHiddenMessages());
            }
            catch (ObjectDisposedException ex)
            {
                _log.Warn(GetHashCode(), ex.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw ExceptionManager.HandleException(GetHashCode(), ex);
            }
        }

        private void ExecHighlightMatch(Action action, LogEvent le, int selectionStart)
        {
            int textLength = Txt.SelectionStart;
            int l = le.Message.Length;
            int curPos = 0;
            int startPos = selectionStart;
            int foundPos = 0;
            _foreColor = Txt.ForeColor;
            Font f = Txt.Font;

            while (foundPos >= 0)
            {
                foundPos = le.Message.IndexOf(action.Pattern, curPos, StringComparison.Ordinal);

                if (foundPos == -1)
                {
                    break;
                }

                Txt.Select(startPos + foundPos, action.Pattern.Length);
                Txt.SelectionColor = action.Color;
                Txt.SelectionFont = action.Font;

                curPos = foundPos + action.Pattern.Length;
            }

            Txt.Select(textLength, 0);
            Txt.SelectionColor = _foreColor;
            Txt.SelectionFont = f;
        }

        private void ExecHighlight(Action action, LogEvent le, int selectionStart)
        {
            _foreColor = Txt.ForeColor;
            Font f = Txt.Font;

            int l = le.Message.Length + 1;
            int pos = selectionStart - l;

            Txt.Select(selectionStart, l);

            Txt.SelectionColor = action.Color;
            Txt.SelectionFont = action.Font;

            Txt.SelectionStart = int.MaxValue;
            Txt.SelectionColor = _foreColor;
            Txt.SelectionFont = f;
        }


        /// <summary>
        ///     Adds an action to the list in this viewer.
        /// </summary>
        /// <param name="action">Action.</param>
        internal void AddAction(Action action)
        {
            if (action == null)
            {
                _log.Warn(GetHashCode(), "Got a null action. Error in configuration?");
                return;
            }
            bool added = false;

            _log.Info(GetHashCode(), "Adding Action: " + action.ActionType + " " + action.Pattern);

            for (int i = 0; i < _actions.Count; i++)
            {
                if ((String.Compare(action.Pattern, _actions[i].Pattern, StringComparison.Ordinal) <= 0) &&
                    (action.ActionType <= _actions[i].ActionType))
                {
                    _actions.Insert(i, action);
                    added = true;
                    break;
                }
            }
            if (!added)
            {
                _actions.Add(action);
            }
        }

        /// <summary>
        ///     removes an action from the list in this viewer.
        /// </summary>
        /// <param name="action">Action.</param>
        internal void RemoveAction(Action action)
        {
            _log.Info(GetHashCode(), "Removed Action: " + action.ActionType + " " + action.Pattern);
            _actions.Remove(action);
        }

        private static XmlAttribute CreateActionFormatAttributes(Action action, XmlNode actionNode)
        {
            Debug.Assert(actionNode.Attributes != null, "actionNode.Attributes != null");

            XmlDocument x = actionNode.OwnerDocument;

            Debug.Assert(x != null, "x != null");
            XmlAttribute a = x.CreateAttribute("color");
            a.Value = action.Color.Name;

            actionNode.Attributes.Append(a);

            a = x.CreateAttribute("font-name");
            a.Value = action.Font.Name;
            actionNode.Attributes.Append(a);

            a = x.CreateAttribute("font-size");
            a.Value = action.Font.Size.ToString();
            actionNode.Attributes.Append(a);

            a = x.CreateAttribute("font-style");
            a.Value = action.Font.Style.ToString();
            actionNode.Attributes.Append(a);

            return a;
        }

        #endregion

        private delegate void AddEventAsynch(LogEvent le, bool forceShow);

        private delegate void AddEventsAsynch(string prefix, List<string> lines);
    }
}