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
using System.Xml;

using logview4net.core;

namespace logview4net.Viewers
{
    /// <summary>
    /// Form used to manage actions for a running session.
    /// </summary>
    public partial class ManageActions : Form
    {
        /// <summary>
        /// Creates a <c ref="ManageActions" />
        /// </summary>
        /// <param name="viewer"></param>
        public ManageActions(TextViewer viewer)
        {
            InitializeComponent();
            BackColor = (Color)Logview4netSettings.Instance["BaseColor"];

            actionManager.LoadConfiguration(viewer);
        }

        private void actionManager_Resize(object sender, EventArgs e)
        {
            if (actionManager.Width  > ClientRectangle.Width)
            {
                var foo = actionManager.Width - ClientRectangle.Width;
                Width += foo;
            }
        }
    }
}