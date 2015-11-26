/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

namespace logview4net.Listeners
{
    partial class SqlListenerConfigurator
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkTail = new System.Windows.Forms.CheckBox();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboDatabase = new System.Windows.Forms.ComboBox();
            this.cboTable = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboColumn = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkWinAuthentication = new System.Windows.Forms.CheckBox();
            this.txtIntervall = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkTimestamp = new System.Windows.Forms.CheckBox();
            this.txtFormat = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkTail
            // 
            this.chkTail.Checked = true;
            this.chkTail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTail.Location = new System.Drawing.Point(400, 114);
            this.chkTail.Name = "chkTail";
            this.chkTail.Size = new System.Drawing.Size(81, 18);
            this.chkTail.TabIndex = 18;
            this.chkTail.Text = "Start at end";
            this.chkTail.CheckedChanged += new System.EventHandler(this.chkTail_CheckedChanged);
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(67, 7);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(60, 20);
            this.txtPrefix.TabIndex = 2;
            this.txtPrefix.Text = "MS-SQL";
            this.txtPrefix.TextChanged += new System.EventHandler(this.txtPrefix_TextChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 18);
            this.label4.TabIndex = 1;
            this.label4.Text = "Prefix";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Server";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(67, 33);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(153, 20);
            this.txtServer.TabIndex = 4;
            this.txtServer.TextChanged += new System.EventHandler(this.txtServer_TextChanged);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(67, 59);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(153, 20);
            this.txtUser.TabIndex = 6;
            this.txtUser.TextChanged += new System.EventHandler(this.txtUser_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "User";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(67, 85);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(153, 20);
            this.txtPassword.TabIndex = 8;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Password";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(226, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 18);
            this.label6.TabIndex = 10;
            this.label6.Text = "Database";
            // 
            // cboDatabase
            // 
            this.cboDatabase.FormattingEnabled = true;
            this.cboDatabase.Location = new System.Drawing.Point(328, 32);
            this.cboDatabase.Name = "cboDatabase";
            this.cboDatabase.Size = new System.Drawing.Size(153, 21);
            this.cboDatabase.TabIndex = 11;
            this.cboDatabase.DropDown += new System.EventHandler(this.cboDatabase_DropDown);
            this.cboDatabase.SelectedIndexChanged += new System.EventHandler(this.cboDatabase_SelectedIndexChanged);
            this.cboDatabase.TextChanged += new System.EventHandler(this.cboDatabase_SelectedIndexChanged);
            // 
            // cboTable
            // 
            this.cboTable.FormattingEnabled = true;
            this.cboTable.Location = new System.Drawing.Point(328, 59);
            this.cboTable.Name = "cboTable";
            this.cboTable.Size = new System.Drawing.Size(153, 21);
            this.cboTable.TabIndex = 13;
            this.cboTable.DropDown += new System.EventHandler(this.cboTable_DropDown);
            this.cboTable.SelectedIndexChanged += new System.EventHandler(this.cboTable_SelectedIndexChanged);
            this.cboTable.TextChanged += new System.EventHandler(this.cboTable_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(226, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 18);
            this.label7.TabIndex = 12;
            this.label7.Text = "Table";
            // 
            // cboColumn
            // 
            this.cboColumn.FormattingEnabled = true;
            this.cboColumn.Location = new System.Drawing.Point(328, 86);
            this.cboColumn.Name = "cboColumn";
            this.cboColumn.Size = new System.Drawing.Size(153, 21);
            this.cboColumn.TabIndex = 15;
            this.cboColumn.DropDown += new System.EventHandler(this.cboColumn_DropDown);
            this.cboColumn.SelectedIndexChanged += new System.EventHandler(this.cboColumn_SelectedIndexChanged);
            this.cboColumn.TextChanged += new System.EventHandler(this.cboColumn_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(226, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 18);
            this.label8.TabIndex = 14;
            this.label8.Text = "Max Column";
            // 
            // chkWinAuthentication
            // 
            this.chkWinAuthentication.Location = new System.Drawing.Point(67, 114);
            this.chkWinAuthentication.Name = "chkWinAuthentication";
            this.chkWinAuthentication.Size = new System.Drawing.Size(156, 18);
            this.chkWinAuthentication.TabIndex = 9;
            this.chkWinAuthentication.Text = "Windows Authentication";
            this.chkWinAuthentication.CheckedChanged += new System.EventHandler(this.chkWinAuthentication_CheckedChanged);
            // 
            // txtIntervall
            // 
            this.txtIntervall.Location = new System.Drawing.Point(328, 113);
            this.txtIntervall.Name = "txtIntervall";
            this.txtIntervall.Size = new System.Drawing.Size(42, 20);
            this.txtIntervall.TabIndex = 17;
            this.txtIntervall.Text = "3000";
            this.txtIntervall.TextChanged += new System.EventHandler(this.txtIntervall_TextChanged);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(226, 116);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 18);
            this.label9.TabIndex = 16;
            this.label9.Text = "Poll intervall (ms)";
            // 
            // chkTimestamp
            // 
            this.chkTimestamp.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkTimestamp.Location = new System.Drawing.Point(229, 138);
            this.chkTimestamp.Name = "chkTimestamp";
            this.chkTimestamp.Size = new System.Drawing.Size(87, 24);
            this.chkTimestamp.TabIndex = 29;
            this.chkTimestamp.Text = "Timestamp";
            this.chkTimestamp.CheckedChanged += new System.EventHandler(this.chkTimestamp_CheckedChanged);
            // 
            // txtFormat
            // 
            this.txtFormat.Enabled = false;
            this.txtFormat.Location = new System.Drawing.Point(373, 139);
            this.txtFormat.Name = "txtFormat";
            this.txtFormat.Size = new System.Drawing.Size(108, 20);
            this.txtFormat.TabIndex = 28;
            this.txtFormat.TextChanged += new System.EventHandler(this.txtFormat_TextChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(325, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 18);
            this.label5.TabIndex = 27;
            this.label5.Text = "Format";
            // 
            // SqlListenerConfigurator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkTimestamp);
            this.Controls.Add(this.txtFormat);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtIntervall);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.chkWinAuthentication);
            this.Controls.Add(this.cboColumn);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cboTable);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cboDatabase);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkTail);
            this.Controls.Add(this.txtPrefix);
            this.Controls.Add(this.label4);
            this.Name = "SqlListenerConfigurator";
            this.Size = new System.Drawing.Size(492, 167);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkTail;
        private System.Windows.Forms.TextBox txtPrefix;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboDatabase;
        private System.Windows.Forms.ComboBox cboTable;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboColumn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkWinAuthentication;
        private System.Windows.Forms.TextBox txtIntervall;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkTimestamp;
        private System.Windows.Forms.TextBox txtFormat;
        private System.Windows.Forms.Label label5;
    }
}
