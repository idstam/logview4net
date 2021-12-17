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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace logview4net.Viewers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ActionConfiguratorGetFileName : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public ActionConfiguratorGetFileName()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Occurs when [file name changed].
        /// </summary>
        public event Action<string> FileNameChanged;

        private string _fileName = "";

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get { return _fileName; }
            set 
                { 
                    txtFileName.Text = value;
                }
        }

        /// <summary>
        /// Sets the title and file type filter
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filter"></param>
        public void Config(string title, string filter)
        {
            openFileDlg.Title = title;
            openFileDlg.Filter = filter;
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            openFileDlg.ShowDialog(this);
            txtFileName.Text = openFileDlg.FileName;

        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            _fileName = txtFileName.Text;
            if (FileNameChanged != null)
            {
                FileNameChanged(FileName);
            }
        }

        
    }
}
