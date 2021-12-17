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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace logview4net.Viewers
{
    public partial class ViewerForm : Form
    {
        private Session _session;


        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        public Session Session
        {
            get { return _session; }
            set { _session = value; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ViewerForm"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public ViewerForm(Session session)
        {
            InitializeComponent();

            _session = session;

            Controls.Add(_session.Viewer as Control);    

            Text = _session.Title;
            _session.Viewer.HasHiddenMessagesEvent += new EventHandler(Viewer_HasHiddenMessages);
        }

        void Viewer_HasHiddenMessages(object sender, EventArgs e)
        {
            var he = (HasHiddenMessagesEventArgs)e;
            ((App)MdiParent).HasHiddenMessages(he.HasHiddenMessages, _session);
        }

        private void ViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            var a = (App)MdiParent;
            _session.Stop();
            a.Sessions.Remove(_session);
        }



    }
}