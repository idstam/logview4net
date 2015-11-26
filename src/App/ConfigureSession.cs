/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

using logview4net.Controls;
using logview4net.core;
using logview4net.Listeners;
using logview4net.Viewers;

namespace logview4net
{
    /// <summary>
    /// This form configures a session
    /// </summary>
    public partial class ConfigureSession : Form
    {
        private textConfigurator _viewerConfigurator = new textConfigurator();
        private ILog _log = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ActionManager _actionManager = new ActionManager();

        private Session _session;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSession"/> class.
        /// </summary>
        public ConfigureSession()
        {
            InitializeComponent();
            BackColor = (Color)Logview4netSettings.Instance["BaseColor"];
            
            _session = new Session();
            _session.Viewer = _viewerConfigurator.Viewer;

            ClearListeners();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSession"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public ConfigureSession(Session session)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "ConfigureSession(Session)");

            InitializeComponent();
            BackColor = (Color)Logview4netSettings.Instance["BaseColor"];
            stacked.Controls.Clear();
            stacked.Controls.Add(headerPanel1);
            stacked.Controls.Add(hpActions);

            _session = session;
            _actionManager.LoadConfiguration((TextViewer)_session.Viewer);
             //_viewerConfigurator = new textConfigurator((textViewer) _session.Viewer);

            showConfiguration();
        }

        #endregion

        private void ConfigureSession_Load(object sender, EventArgs e)
        {
            
            headerPanel1.AddControl(_viewerConfigurator);
            headerPanel1.Text = "Viewer";
            headerPanel1.DeleteIsVisible = false;
            headerPanel1.ResizeHeight();

            _actionManager.LoadConfiguration(_viewerConfigurator.Viewer);
            _actionManager.SizeChanged += new EventHandler(_actionManager_SizeChanged);
            hpActions.AddControl(_actionManager);
            hpActions.Text = "User Interactions";
            
            
            hpActions.DeleteIsVisible = false;
            //hpActions.ResizeHeight();
            hpActions.Collapse();

        }

        void _actionManager_SizeChanged(object sender, EventArgs e)
        {
            hpActions.Expand();
        }

        

        private void btnAddListener_Click(object sender, EventArgs e)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "btnAddListener_Click");
            
            var c = (Control) getListenerConfigurator(cboListenerType.Text);

            if (c != null)
            {
                AddListenerConfiguratorControl(c);
                _session.AddListener(((IListenerConfigurator) c).Listener);
            }

            if (stacked.HorizontalScroll.Visible)
            {
                Width += 50;
            }
        }

        private void AddListenerConfiguratorControl(Control c)
        {
            c.TextChanged += new EventHandler(Configurator_TextChanged);
            
            var h = new HeaderPanel();
            h.Width = headerPanel1.Width;
            h.AddControl(c);
            h.Text = ((IListenerConfigurator) c).Caption;

            h.ResizeHeight();
            stacked.Controls.Add(h);
            
            if (_session.IsRunning)
            {
                //((Control) lc).Enabled = false;
            }

       }

        void Configurator_TextChanged(object sender, EventArgs e)
        {
            var ctrl = (Control)sender;
            var hp = (HeaderPanel)ctrl.Parent.Parent;

            if (sender is IListenerConfigurator)
            {
                var lc = (IListenerConfigurator)sender;
                hp.Text = lc.Caption;
            }
            
        }

        #region getListenerConfigurator

        private IListenerConfigurator getListenerConfigurator(string type)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "getListenerConfigurator");

            IListenerConfigurator ret = null;
            switch (type)
            {
                case "":
                    break;
                case "UdpListener":
                    ret = new ListenerConfigurator((IConfigurableListener)new UdpListener(), _session);
                    break;
                case "TcpListener":
                    ret = new ListenerConfigurator((IConfigurableListener)new TcpListener(), _session);
                    break;
                case "EventLogListener":
                    ret = new EventLogListenerConfigurator();
                    break;
                case "FileListener":
                    ret = new ListenerConfigurator((IConfigurableListener)new FileListener(), _session);
                    break;
                case "FolderListener":
                    ret = new ListenerConfigurator((IConfigurableListener)new FolderListener(), _session);
                    break;
                case "SqlListener":
                    ret = new SqlListenerConfigurator();
                    break;
                case "RssListener":
                    ret = new ListenerConfigurator((IConfigurableListener)new RssListener(), _session);
                    break;
                case "StdOutListener":
                    ret = new ListenerConfigurator((IConfigurableListener)new StdOutListener(), _session);
                    break;
                case "ComPortListener":
                    ret = new ListenerConfigurator((IConfigurableListener)new ComPortListener(), _session);
                    break;
                case "MySqlListener":
                    ret = new MySqlListenerConfigurator();
                    break;
                default:
                    throw new NotImplementedException(type);
            }
            return ret;
        }

        #endregion

        internal void LoadDefaultSettings()
        {
            if (string.IsNullOrEmpty(Settings1.Default.DefaultSettings.Trim()))
            {
                loadConfiguration(Application.StartupPath + @"\DefaultSession.xml");
            }
            else
            {
                loadConfiguration(Settings1.Default.DefaultSettings);
            }
        }

        #region loadConfiguration

        internal void showConfiguration()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "showConfiguration()");


            txtSessionTitle.Text = _session.Title;
            _viewerConfigurator.Viewer = (TextViewer)_session.Viewer;

            foreach (var listener in _session.Listeners)
            {
                if (_log.Enabled) _log.Debug(GetHashCode(), "Loading listener: " + listener.GetType().ToString());
                
                IListenerConfigurator lc;
                if (listener is IConfigurableListener)
                {
                    lc = new ListenerConfigurator((IConfigurableListener)listener, _session);
                }
                else
                {
                    lc = listener.GetNewConfigurator();
                }

                if (listener.IsConfigured)
                {
                    lc.UpdateControls();
                }
                else
                {
                    lc.Configuration = listener.GetConfiguration();
                }

                AddListenerConfiguratorControl((Control) lc);
            }

            if (stacked.Controls.Count > 2) btnOK.Enabled = true;
        }

        internal void loadConfiguration(string configurationFileName)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "loadConfiguration(string)");
            try
            {
                var doc = new XmlDocument();
                doc.Load(configurationFileName);

                loadConfiguration(doc);
            }
            catch (Exception x)
            {
                _log.Error(GetHashCode(), x.ToString());
            }
        }

        internal void loadConfiguration(XmlDocument configuration)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "loadConfiguration(XmlDocument)");

            stacked.Controls.Clear();
            stacked.Controls.Add(headerPanel1);
            hpActions.Collapse();
            stacked.Controls.Add(hpActions);

            _session = new Session();
            _viewerConfigurator.Viewer.Actions.Clear();
            _session.Viewer = _viewerConfigurator.Viewer;

            var e = configuration.SelectSingleNode("//session");
            txtSessionTitle.Text = e.Attributes["title"].Value;
            
            e = configuration.SelectSingleNode("//viewer");
            _viewerConfigurator.Configuration = e.InnerXml;

            var nodes = configuration.SelectNodes("//listener");

            foreach (XmlNode node in nodes)
            {
                var lc = getListenerConfigurator(node.FirstChild.Name);
                lc.Configuration = node.InnerXml;
                AddListenerConfiguratorControl((Control) lc);
                _session.AddListener(lc.Listener);
            }
            
            _actionManager.LoadConfiguration(_viewerConfigurator.Viewer); 

            if (stacked.Controls.Count > 2) btnOK.Enabled = true;
        }

        #endregion

        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        /// <value>The session.</value>
        public Session Session
        {
            get { return _session; }
            set { _session = value; }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "btnSave_Click");
            //saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            saveFileDialog1.DefaultExt = ".l4n";
            saveFileDialog1.ShowDialog(this);
            if (saveFileDialog1.FileName != "")
            {
                _session.SaveConfiguration(saveFileDialog1.FileName, _viewerConfigurator.Configuration);
            }
        }


        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "btnLoad_Click");

            openFileDialog1.ShowDialog(this);
            Cursor = Cursors.WaitCursor;
            loadConfiguration(openFileDialog1.FileName);
            Cursor = Cursors.Default;
        }

        private void txtSessionTitle_TextChanged(object sender, EventArgs e)
        {
            _session.Title = txtSessionTitle.Text;
        }


        private void stacked_ControlRemoved(object sender, ControlEventArgs e)
        {
            var hp = e.Control as HeaderPanel;
            if (hp != null)
            {
                foreach (Control c in hp.ChildControls)
                {
                    var ilc = c as IListenerConfigurator;
                    if (ilc != null)
                    {
                        _session.RemoveListener(ilc.Listener);
                    }
                }
            }
            Debug.WriteLine("Removed a listener");
        }

        internal void ClearListeners()
        {
            stacked.Controls.Clear();
            stacked.Controls.Add(headerPanel1);
            stacked.Controls.Add(hpActions);
           
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
        	_viewerConfigurator.ValidateSettings();

            if (chkDefault.Checked)
            {
                SaveDefaultSettings();
            }
        	DialogResult  = DialogResult.OK;
        	Close();
        }

        private void SaveDefaultSettings()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folder = Path.Combine(folder, "logview4net");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var file = Path.Combine(folder, "default.l4n");
            Settings1.Default.DefaultSettings = file;
            Settings1.Default.Save();

            _session.SaveConfiguration(file, _viewerConfigurator.Configuration);

        }

        private void stacked_Resize(object sender, EventArgs e)
        {
            stacked.RestackControls();
        }

    }
}
