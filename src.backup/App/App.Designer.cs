/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

namespace logview4net
{
    /// <summary>
    /// This is the main application class. It contains the main form.
    /// </summary>
    partial class App
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(App));
        	this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        	this.newSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.wordWrapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.hiddenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.sessionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.registerFiletypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.menuStrip1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// menuStrip1
        	// 
        	this.menuStrip1.BackColor = System.Drawing.Color.Lavender;
        	this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.newSessionToolStripMenuItem,
        	        	        	this.clearToolStripMenuItem,
        	        	        	this.wordWrapToolStripMenuItem,
        	        	        	this.pauseToolStripMenuItem,
        	        	        	this.hiddenToolStripMenuItem,
        	        	        	this.actionsToolStripMenuItem,
        	        	        	this.settingsToolStripMenuItem,
        	        	        	this.sessionsToolStripMenuItem,
        	        	        	this.helpToolStripMenuItem});
        	this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
        	this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        	this.menuStrip1.MdiWindowListItem = this.sessionsToolStripMenuItem;
        	this.menuStrip1.Name = "menuStrip1";
        	this.menuStrip1.Size = new System.Drawing.Size(831, 24);
        	this.menuStrip1.TabIndex = 3;
        	this.menuStrip1.Text = "menuStrip1";
        	// 
        	// newSessionToolStripMenuItem
        	// 
        	this.newSessionToolStripMenuItem.CheckOnClick = true;
        	this.newSessionToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.newSessionToolStripMenuItem.Name = "newSessionToolStripMenuItem";
        	this.newSessionToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
        	this.newSessionToolStripMenuItem.Text = "New Session";
        	this.newSessionToolStripMenuItem.Click += new System.EventHandler(this.ToolBarButtonClick);
        	// 
        	// clearToolStripMenuItem
        	// 
        	this.clearToolStripMenuItem.Image = global::logview4net.Properties.Resources.Document;
        	this.clearToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
        	this.clearToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
        	this.clearToolStripMenuItem.Text = "&Clear";
        	this.clearToolStripMenuItem.Click += new System.EventHandler(this.ToolBarButtonClick);
        	// 
        	// wordWrapToolStripMenuItem
        	// 
        	this.wordWrapToolStripMenuItem.CheckOnClick = true;
        	this.wordWrapToolStripMenuItem.Image = global::logview4net.Properties.Resources.arrow_undo;
        	this.wordWrapToolStripMenuItem.Name = "wordWrapToolStripMenuItem";
        	this.wordWrapToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
        	this.wordWrapToolStripMenuItem.Text = "Word wrap";
        	this.wordWrapToolStripMenuItem.Click += new System.EventHandler(this.ToolBarButtonClick);
        	// 
        	// pauseToolStripMenuItem
        	// 
        	this.pauseToolStripMenuItem.CheckOnClick = true;
        	this.pauseToolStripMenuItem.DoubleClickEnabled = true;
        	this.pauseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pauseToolStripMenuItem.Image")));
        	this.pauseToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
        	this.pauseToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
        	this.pauseToolStripMenuItem.Text = "Pause";
        	this.pauseToolStripMenuItem.Click += new System.EventHandler(this.ToolBarButtonClick);
        	// 
        	// hiddenToolStripMenuItem
        	// 
        	this.hiddenToolStripMenuItem.Name = "hiddenToolStripMenuItem";
        	this.hiddenToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
        	this.hiddenToolStripMenuItem.Text = "Hidden";
        	this.hiddenToolStripMenuItem.ToolTipText = "Show all hidden messages";
        	this.hiddenToolStripMenuItem.Click += new System.EventHandler(this.hiddenToolStripMenuItem_Click);
        	// 
        	// actionsToolStripMenuItem
        	// 
        	this.actionsToolStripMenuItem.Image = global::logview4net.Properties.Resources.AddTable;
        	this.actionsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
        	this.actionsToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
        	this.actionsToolStripMenuItem.Text = "Actions";
        	this.actionsToolStripMenuItem.Click += new System.EventHandler(this.ToolBarButtonClick);
        	// 
        	// settingsToolStripMenuItem
        	// 
        	this.settingsToolStripMenuItem.Image = global::logview4net.Properties.Resources.cog_edit;
        	this.settingsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
        	this.settingsToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
        	this.settingsToolStripMenuItem.Text = "Settings";
        	this.settingsToolStripMenuItem.Click += new System.EventHandler(this.ToolBarButtonClick);
        	// 
        	// sessionsToolStripMenuItem
        	// 
        	this.sessionsToolStripMenuItem.Image = global::logview4net.Properties.Resources.CascadeWindows;
        	this.sessionsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.sessionsToolStripMenuItem.Name = "sessionsToolStripMenuItem";
        	this.sessionsToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
        	this.sessionsToolStripMenuItem.Text = "Sessions";
        	// 
        	// helpToolStripMenuItem
        	// 
        	this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.aboutToolStripMenuItem,
        	        	        	this.documentationToolStripMenuItem,
        	        	        	this.checkForUpdatesToolStripMenuItem,
        	        	        	this.registerFiletypeToolStripMenuItem});
        	this.helpToolStripMenuItem.Image = global::logview4net.Properties.Resources.help;
        	this.helpToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
        	this.helpToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
        	this.helpToolStripMenuItem.Text = "Help";
        	// 
        	// aboutToolStripMenuItem
        	// 
        	this.aboutToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
        	this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
        	this.aboutToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
        	this.aboutToolStripMenuItem.Text = "&About";
        	this.aboutToolStripMenuItem.Click += new System.EventHandler(this.ToolBarButtonClick);
        	// 
        	// documentationToolStripMenuItem
        	// 
        	this.documentationToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
        	this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
        	this.documentationToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
        	this.documentationToolStripMenuItem.Text = "&Documentation";
        	this.documentationToolStripMenuItem.Click += new System.EventHandler(this.documentationToolStripMenuItem_Click_1);
        	// 
        	// checkForUpdatesToolStripMenuItem
        	// 
        	this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
        	this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
        	this.checkForUpdatesToolStripMenuItem.Text = "Check for updates";
        	this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
        	// 
        	// registerFiletypeToolStripMenuItem
        	// 
        	this.registerFiletypeToolStripMenuItem.Name = "registerFiletypeToolStripMenuItem";
        	this.registerFiletypeToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
        	this.registerFiletypeToolStripMenuItem.Text = "Register filetype";
        	this.registerFiletypeToolStripMenuItem.Click += new System.EventHandler(this.registerFiletypeToolStripMenuItem_Click);
        	// 
        	// App
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackColor = System.Drawing.Color.Khaki;
        	this.ClientSize = new System.Drawing.Size(831, 491);
        	this.Controls.Add(this.menuStrip1);
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.IsMdiContainer = true;
        	this.KeyPreview = true;
        	this.MainMenuStrip = this.menuStrip1;
        	this.Name = "App";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "logview4net";
        	this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.App_FormClosing);
        	this.Load += new System.EventHandler(this.App_Load);
        	this.MdiChildActivate += new System.EventHandler(this.App_MdiChildActivate);
        	this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.App_KeyUp);
        	this.menuStrip1.ResumeLayout(false);
        	this.menuStrip1.PerformLayout();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        
        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newSessionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sessionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordWrapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registerFiletypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hiddenToolStripMenuItem;
        
    }
}