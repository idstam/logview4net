/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

namespace logview4net
{
    partial class ConfigureSession
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureSession));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAddListener = new System.Windows.Forms.Button();
            this.cboListenerType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSessionTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.stacked = new logview4net.Controls.StackedPanel();
            this.hpActions = new logview4net.Controls.HeaderPanel();
            this.headerPanel1 = new logview4net.Controls.HeaderPanel();
            this.chkDefault = new System.Windows.Forms.CheckBox();
            this.stacked.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Image = global::logview4net.Properties.Resources.tick;
            this.btnOK.Location = new System.Drawing.Point(595, 420);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(71, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "D&one";
            this.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Image = global::logview4net.Properties.Resources.cog_go;
            this.btnLoad.Location = new System.Drawing.Point(12, 419);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(71, 25);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load";
            this.btnLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Image = global::logview4net.Properties.Resources.cog_edit;
            this.btnSave.Location = new System.Drawing.Point(89, 419);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(71, 25);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddListener
            // 
            this.btnAddListener.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddListener.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddListener.Image = global::logview4net.Properties.Resources.add;
            this.btnAddListener.Location = new System.Drawing.Point(641, 12);
            this.btnAddListener.Name = "btnAddListener";
            this.btnAddListener.Size = new System.Drawing.Size(25, 25);
            this.btnAddListener.TabIndex = 6;
            this.btnAddListener.UseVisualStyleBackColor = true;
            this.btnAddListener.Click += new System.EventHandler(this.btnAddListener_Click);
            // 
            // cboListenerType
            // 
            this.cboListenerType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboListenerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboListenerType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboListenerType.Items.AddRange(new object[] {
            "EventLogListener",
            "FileListener",
            "FolderListener",
            "MySqlListener",
            "RssListener",
            "SqlListener",
            "StdOutListener",
            "ComPortListener",
            "TcpListener",
            "UdpListener"});
            this.cboListenerType.Location = new System.Drawing.Point(496, 11);
            this.cboListenerType.Name = "cboListenerType";
            this.cboListenerType.Size = new System.Drawing.Size(139, 28);
            this.cboListenerType.Sorted = true;
            this.cboListenerType.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(361, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 25);
            this.label1.TabIndex = 13;
            this.label1.Text = "Listeners to add";
            // 
            // txtSessionTitle
            // 
            this.txtSessionTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSessionTitle.Location = new System.Drawing.Point(113, 11);
            this.txtSessionTitle.Name = "txtSessionTitle";
            this.txtSessionTitle.Size = new System.Drawing.Size(193, 26);
            this.txtSessionTitle.TabIndex = 15;
            this.txtSessionTitle.TextChanged += new System.EventHandler(this.txtSessionTitle_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "Session title";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "logview4net files|*.l4n";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "logview4net files|*.l4n|XML files|*.xml|All files|*.*";
            // 
            // stacked
            // 
            this.stacked.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stacked.AutoScroll = true;
            this.stacked.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.stacked.Controls.Add(this.hpActions);
            this.stacked.Controls.Add(this.headerPanel1);
            this.stacked.Location = new System.Drawing.Point(12, 51);
            this.stacked.Name = "stacked";
            this.stacked.Size = new System.Drawing.Size(657, 362);
            this.stacked.TabIndex = 14;
            this.stacked.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.stacked_ControlRemoved);
            this.stacked.Resize += new System.EventHandler(this.stacked_Resize);
            // 
            // hpActions
            // 
            this.hpActions.AllowDrop = true;
            this.hpActions.AutoSize = true;
            this.hpActions.BackColor = System.Drawing.Color.Transparent;
            this.hpActions.DeleteIsVisible = true;
            this.hpActions.Location = new System.Drawing.Point(0, 0);
            this.hpActions.Name = "hpActions";
            this.hpActions.Size = new System.Drawing.Size(197, 291);
            this.hpActions.TabIndex = 1;
            // 
            // headerPanel1
            // 
            this.headerPanel1.AllowDrop = true;
            this.headerPanel1.BackColor = System.Drawing.Color.Transparent;
            this.headerPanel1.DeleteIsVisible = true;
            this.headerPanel1.Location = new System.Drawing.Point(0, 296);
            this.headerPanel1.Name = "headerPanel1";
            this.headerPanel1.Size = new System.Drawing.Size(197, 110);
            this.headerPanel1.TabIndex = 0;
            // 
            // chkDefault
            // 
            this.chkDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDefault.AutoSize = true;
            this.chkDefault.Location = new System.Drawing.Point(424, 424);
            this.chkDefault.Name = "chkDefault";
            this.chkDefault.Size = new System.Drawing.Size(165, 17);
            this.chkDefault.TabIndex = 17;
            this.chkDefault.Text = "Use these settings as default.";
            this.chkDefault.UseVisualStyleBackColor = true;
            // 
            // ConfigureSession
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(678, 452);
            this.Controls.Add(this.chkDefault);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSessionTitle);
            this.Controls.Add(this.stacked);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboListenerType);
            this.Controls.Add(this.btnAddListener);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "ConfigureSession";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConfigureSession";
            this.Load += new System.EventHandler(this.ConfigureSession_Load);
            this.stacked.ResumeLayout(false);
            this.stacked.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private logview4net.Controls.HeaderPanel headerPanel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAddListener;
        private System.Windows.Forms.ComboBox cboListenerType;
        private System.Windows.Forms.Label label1;
        private logview4net.Controls.StackedPanel stacked;
        private System.Windows.Forms.TextBox txtSessionTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
        private logview4net.Controls.HeaderPanel hpActions;
        private System.Windows.Forms.CheckBox chkDefault;



    }
}