/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

namespace logview4net.Controls
{
    partial class StackedPanel
    {
        private const int _padding = 5;

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
            this.SuspendLayout();
            // 
            // StackedPanel
            // 
            this.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.StackedPanel_ControlRemoved);
            this.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.StackedPanel_ControlAdded);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
