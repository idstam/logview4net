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
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using logview4net.core;

namespace logview4net
{
    /// <summary>
    /// This is the About form.
    /// It shows the current version and has a link to the project pages as SourceForge.
    /// </summary>
    public class About : Form
    {
        private LinkLabel linkLabel1;
        private Label label1;
        private Button btnOK;
        private Label label2;
        private LinkLabel linkLabel2;
        private Label label3;
        private Label label4;
        private LinkLabel linkLabel3;
        private Label lblLogInfo;
        private ILog _log = Logger.GetLogger(typeof (About));
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        /// <summary>
        /// Creates a new <see cref="About"/> instance.
        /// </summary>
        public About()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            BackColor = (Color)Logview4netSettings.Instance["BaseColor"];
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	var resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
        	this.linkLabel1 = new System.Windows.Forms.LinkLabel();
        	this.label1 = new System.Windows.Forms.Label();
        	this.btnOK = new System.Windows.Forms.Button();
        	this.label2 = new System.Windows.Forms.Label();
        	this.linkLabel2 = new System.Windows.Forms.LinkLabel();
        	this.label3 = new System.Windows.Forms.Label();
        	this.label4 = new System.Windows.Forms.Label();
        	this.linkLabel3 = new System.Windows.Forms.LinkLabel();
        	this.lblLogInfo = new System.Windows.Forms.Label();
        	this.SuspendLayout();
        	// 
        	// linkLabel1
        	// 
        	this.linkLabel1.Location = new System.Drawing.Point(162, 42);
        	this.linkLabel1.Name = "linkLabel1";
        	this.linkLabel1.Size = new System.Drawing.Size(246, 21);
        	this.linkLabel1.TabIndex = 0;
        	this.linkLabel1.TabStop = true;
        	this.linkLabel1.Text = "http://sourceforge.net/projects/logview4net/";
        	this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
        	// 
        	// label1
        	// 
        	this.label1.Location = new System.Drawing.Point(6, 42);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(153, 21);
        	this.label1.TabIndex = 1;
        	this.label1.Text = "Project page at Sourceforge";
        	// 
        	// btnOK
        	// 
        	this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.btnOK.BackColor = System.Drawing.Color.Transparent;
        	this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.btnOK.Image = global::logview4net.Properties.Resources.tick;
        	this.btnOK.Location = new System.Drawing.Point(329, 153);
        	this.btnOK.Name = "btnOK";
        	this.btnOK.Size = new System.Drawing.Size(56, 23);
        	this.btnOK.TabIndex = 2;
        	this.btnOK.Text = "&OK";
        	this.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
        	this.btnOK.UseVisualStyleBackColor = false;
        	// 
        	// label2
        	// 
        	this.label2.Location = new System.Drawing.Point(6, 84);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(153, 21);
        	this.label2.TabIndex = 4;
        	this.label2.Text = "Mail to the developer(s)";
        	// 
        	// linkLabel2
        	// 
        	this.linkLabel2.Location = new System.Drawing.Point(162, 84);
        	this.linkLabel2.Name = "linkLabel2";
        	this.linkLabel2.Size = new System.Drawing.Size(246, 21);
        	this.linkLabel2.TabIndex = 3;
        	this.linkLabel2.TabStop = true;
        	this.linkLabel2.Text = "mailto:johan@logview4net.com";
        	this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
        	// 
        	// label3
        	// 
        	this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.label3.Location = new System.Drawing.Point(6, 9);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(391, 21);
        	this.label3.TabIndex = 5;
        	this.label3.Text = "logview4net";
        	// 
        	// label4
        	// 
        	this.label4.Location = new System.Drawing.Point(6, 63);
        	this.label4.Name = "label4";
        	this.label4.Size = new System.Drawing.Size(153, 21);
        	this.label4.TabIndex = 7;
        	this.label4.Text = "Project home page";
        	// 
        	// linkLabel3
        	// 
        	this.linkLabel3.Location = new System.Drawing.Point(162, 63);
        	this.linkLabel3.Name = "linkLabel3";
        	this.linkLabel3.Size = new System.Drawing.Size(246, 21);
        	this.linkLabel3.TabIndex = 6;
        	this.linkLabel3.TabStop = true;
        	this.linkLabel3.Text = "http://logview4net.com";
        	this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
        	// 
        	// lblLogInfo
        	// 
        	this.lblLogInfo.AutoSize = true;
        	this.lblLogInfo.Location = new System.Drawing.Point(6, 158);
        	this.lblLogInfo.Name = "lblLogInfo";
        	this.lblLogInfo.Size = new System.Drawing.Size(35, 13);
        	this.lblLogInfo.TabIndex = 8;
        	this.lblLogInfo.Text = "label5";
        	this.lblLogInfo.Visible = false;
        	// 
        	// About
        	// 
        	this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        	this.BackColor = System.Drawing.Color.Khaki;
        	this.ClientSize = new System.Drawing.Size(397, 188);
        	this.Controls.Add(this.lblLogInfo);
        	this.Controls.Add(this.label4);
        	this.Controls.Add(this.linkLabel3);
        	this.Controls.Add(this.label3);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.linkLabel2);
        	this.Controls.Add(this.btnOK);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.linkLabel1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.Name = "About";
        	this.ShowInTaskbar = false;
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "About";
        	this.Load += new System.EventHandler(this.About_Load);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(((LinkLabel) sender).Text);
        }

        private void About_Load(object sender, EventArgs e)
        {
            var an = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            label3.Text = "logview4net " + an.Version.Major.ToString() + "." + an.Version.Minor.ToString();

            lblLogInfo.Visible = _log.Enabled;
            lblLogInfo.Text = "Logging is done via UDP on port: " + Logger.Port.ToString();
        }
    }
}