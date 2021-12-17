/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using logview4net.core;
using logview4net.Listeners;
using logview4net.Properties;
using logview4net.Viewers;

namespace logview4net
{
    /// <summary>
    ///     Main form of the application
    /// </summary>
    public partial class App : Form
    {
        private readonly ILog _log;
        private string[] _args;
        private List<Session> _sessions = new List<Session>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="App" /> class.
        /// </summary>
        public App(string[] args)
        {
            _log = Logger.GetLogger("App");

            _args = args;

            AppDomain.CurrentDomain.UnhandledException +=
                CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;

            InitializeComponent();

            menuStrip1.BackColor = (Color) Logview4netSettings.Instance["BaseColor"];
        }

        /// <summary>
        ///     Gets or sets the sessions.
        /// </summary>
        /// <value>The sessions.</value>
        public List<Session> Sessions
        {
            get { return _sessions; }
            set { _sessions = value; }
        }

        private void StartListening()
        {
            _log.Debug(GetHashCode(), "StartListening");

            var cf = new ConfigureSession();
            var showConfigWindow = true;

            cf.LoadDefaultSettings();

            if (_args.Length > 0)
            {
                //If there is anything on the commandline clear the listeners from the default session.
                cf.ClearListeners();

                var fileNum = 0;
                foreach (var file in _args)
                {
                    _log.Debug(GetHashCode(), "Argument #" + fileNum + " " + file);

                    if (File.Exists(file))
                    {
                        var fi = new FileInfo(file);
                        if (fi.Extension == ".exe")
                        {
                            cf.Session.Listeners.Add(new StdOutListener(file, "StdOut", false));
                        }
                        else if (fi.Extension == ".l4n")
                        {
                            showConfigWindow = false;
                            cf.loadConfiguration(file);
                        }
                        else
                        {
                            cf.Session.Listeners.Add(new FileListener(file, 3, "File" + fileNum, true));
                        }
                    }
                    else if (Directory.Exists(file))
                    {
                        cf.Session.Listeners.Add(new FolderListener(file, "Folder", true, false));
                    }

                    fileNum++;
                }
                cf.showConfiguration();
                _args = new string[] {};
            }

            if (showConfigWindow)
            {
                cf.ShowDialog();
            }
            else
            {
                cf.DialogResult = DialogResult.OK;
            }

            if (cf.DialogResult == DialogResult.OK)
            {
                var f = new ViewerForm(cf.Session) {MdiParent = this};

                f.Show();
                f.WindowState = FormWindowState.Maximized;
                var session = f.Session;

                session.Start();
                _sessions.Add(session);
            }

            cf.Dispose();
        }

        private void EnableButtons()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "enableButtons");

            //tbClear.Visible = foo;
            //tbActions.Visible = foo;
            //tbClose.Visible = foo;
            //tbPause.Visible = foo;
            //tbSettings.Visible = foo;
        }


        private void ToolBarButtonClick(object sender, EventArgs e)
        {
            var b = sender as ToolStripMenuItem;

            if (_log.Enabled) _log.Debug(GetHashCode(), "ToolBarButtonClick " + b.Name);

            if (_sessions.Count > 0)
            {
                var currentSession = ((ViewerForm) ActiveMdiChild).Session;

                switch (b.Name)
                {
                    case "clearToolStripMenuItem":
                        currentSession.Viewer.Clear();
                        break;
                    case "settingsToolStripMenuItem":
                        var f = new ConfigureSession(currentSession);
                        f.ShowDialog(this);
                        f.Dispose();
                        ActiveMdiChild.Text = currentSession.Title;
                        break;
                    case "pauseToolStripMenuItem":
                        currentSession.Viewer.Paused = !currentSession.Viewer.Paused;
                        UpdatePauseButton();
                        break;
                    case "actionsToolStripMenuItem":
                        var ma = new ManageActions((TextViewer) currentSession.Viewer);
                        ma.ShowDialog(this);
                        ma.Dispose();
                        break;
                    case "wordWrapToolStripMenuItem":
                        currentSession.Viewer.WordWrap = !currentSession.Viewer.WordWrap;
                        UpdateWordWrapButton();
                        break;
                }
            }

            switch (b.Name)
            {
                case "newSessionToolStripMenuItem":
                    StartListening();
                    EnableButtons();
                    break;
                case "aboutToolStripMenuItem":
                    var a = new About();
                    a.ShowDialog();
                    break;
            }
        }

        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            _log.Error(GetHashCode(), "Application_ThreadException", e.Exception);
        }


        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var terminating = e.IsTerminating ? "The application is forced to stop." : "";

            _log.Error(GetHashCode(), "Unhandled system error. " + terminating, (Exception) e.ExceptionObject);
        }

        private void App_Load(object sender, EventArgs e)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "App_Load");

            var foo = new ToolStripMenuItem("Vertically", Resources.ArrangeWindows)
            {
                ImageTransparentColor = Color.Magenta
            };
            foo.Click += mnuArrangeVerticallyClick;
            sessionsToolStripMenuItem.DropDownItems.Insert(0, foo);

            foo = new ToolStripMenuItem("Side by side", Resources.ArrangeSideBySide)
            {
                ImageTransparentColor = Color.Magenta
            };
            foo.Click += mnuArrangeHorizontallyClick;
            sessionsToolStripMenuItem.DropDownItems.Insert(0, foo);

            foo = new ToolStripMenuItem("Tabbed", Resources.ArrangeSideBySide) {ImageTransparentColor = Color.Magenta};
            foo.Click += mnuTabbedInterfaceClick;
            sessionsToolStripMenuItem.DropDownItems.Insert(0, foo);

            StartListening();

            EnableButtons();


            LoadFormPosition();

            var upd = new Updater();
            upd.CheckUpdates();
        }

        private void mnuArrangeVerticallyClick(object sender, EventArgs e)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "mnuArrangeVerticallyClick");

            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void mnuArrangeHorizontallyClick(object sender, EventArgs e)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "mnuArrangeHorizontallyClick");

            LayoutMdi(MdiLayout.TileVertical);
        }

        private void mnuTabbedInterfaceClick(object sender, EventArgs e)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "mnuTabbedInterfaceClick");
        }

        private void App_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "App_FormClosing");

            SaveFormPosition();

            foreach (var s in _sessions)
            {
                s.Stop();
            }
            Process.GetCurrentProcess().Kill();
        }

        private void SaveFormPosition()
        {
            if(WindowState == FormWindowState.Minimized) return;

            Settings1.Default.FormPos = new Point(Left, Top);
            Settings1.Default.FormSize = Size;
            Settings1.Default.FormState = WindowState;
            Settings1.Default.Save();
        }

        private void LoadFormPosition()
        {
            if (Settings1.Default.FormSize.Width < 10) return;

            if (Settings1.Default.FormState == FormWindowState.Minimized) return;
            
            Left = Settings1.Default.FormPos.X;
            Top = Settings1.Default.FormPos.Y;            
            Size = Settings1.Default.FormSize;
            WindowState = Settings1.Default.FormState;
        }

        private void App_MdiChildActivate(object sender, EventArgs e)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "App_MdiChildActivate");

            try
            {
                var vf = (ViewerForm) ActiveMdiChild;
                pauseToolStripMenuItem.Checked = vf.Session.Viewer.Paused;
                hiddenToolStripMenuItem.Enabled = vf.Session.Viewer.HasHiddenMessages();
                UpdatePauseButton();
            }
            catch (Exception ex)
            {
                _log.Error(GetHashCode(), ex.ToString());
            }
        }

        private void UpdatePauseButton()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "UpdatePauseButton");

            pauseToolStripMenuItem.Image = pauseToolStripMenuItem.Checked ? Resources.Play : Resources.Pause;

            pauseToolStripMenuItem.Text = pauseToolStripMenuItem.Checked ? "Run" : "Pause";
        }

        private void UpdateWordWrapButton()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "UpdateWordWrapButton");

            wordWrapToolStripMenuItem.Image = wordWrapToolStripMenuItem.Checked ? Resources.arrow_right : Resources.arrow_undo;
            wordWrapToolStripMenuItem.Text = wordWrapToolStripMenuItem.Checked ? "No Wrap" : "Word Wrap";
        }

        private void documentationToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            const string helpPath = "http://logview4net.com";
            Process.Start(helpPath);
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var u = new Updater(true);
            u.CheckUpdatesFromGui();
        }

        private void registerFiletypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var msg = "Do you want to associate the .l4n file extension with logview4net?";
            if (MessageBox.Show(msg, "Register filetype", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
            {
                if (!RegisterFiletype.Register("logview4net", ".l4n", "A viewer for application logs.",
                    Application.ExecutablePath))
                {
                    MessageBox.Show("The file type registration failed.", "Register filetype", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void App_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                if (menuStrip1.Visible)
                {
                    menuStrip1.Visible = false;
                    FormBorderStyle = FormBorderStyle.None;
                }
                else
                {
                    menuStrip1.Visible = true;
                    FormBorderStyle = FormBorderStyle.Sizable;
                }
            }
        }

        internal void HasHiddenMessages(bool p, Session session)
        {
            foreach (ViewerForm f in MdiChildren)
            {
                if (f.Session.Hash == session.Hash)
                {
                    hiddenToolStripMenuItem.Enabled = p;
                }
            }
        }

        private void hiddenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var vf = (ViewerForm) ActiveMdiChild;
            vf.Session.Viewer.ShowAllHidden();
        }
    }
}