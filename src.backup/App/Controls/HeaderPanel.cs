/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using logview4net.core;
using logview4net.Properties;

namespace logview4net.Controls
{
    public partial class HeaderPanel : UserControl
    {
        /// <summary>
        /// If the control is collapsed or not.
        /// </summary>
        protected bool IsCollapsed = false;
        /// <summary>
        /// The height of the control when is is expanded
        /// </summary>
        protected int ExpandedHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderPanel"/> class.
        /// </summary>
        public HeaderPanel()
        {
            InitializeComponent();
            if (Logview4netSettings.Instance == null) return;

            pnlControls.BackColor = (Color)Logview4netSettings.Instance["LightColor"];
            lblCaption.BackColor = (Color)Logview4netSettings.Instance["DarkColor"];
            btnSize.BackColor = (Color)Logview4netSettings.Instance["DarkColor"];
            btnDelete.BackColor = (Color)Logview4netSettings.Instance["DarkColor"];

        }

        /// <summary>
        /// Handles the Paint event of the HeaderPanel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        protected void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            var h = ClientRectangle.Height;
            var w = ClientRectangle.Width;

            var g = CreateGraphics();
            var p = new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark));
            g.DrawLine(p, 0, 0, w - 3, 0);
            g.DrawLine(p, 0, h - 3, w - 3, h - 3);
            g.DrawLine(p, 0, 0, 0, h - 3);
            g.DrawLine(p, w - 3, 0, w - 3, h - 3);
            g.DrawLine(p, 0, btnSize.Bottom, w - 3, btnSize.Bottom);
            //g.DrawLine(p, 0, btnSize.Bottom + 1, w - 3, btnSize.Bottom + 1);


            var p2 = new Pen(Color.FromArgb(100, Color.Black));
            g.DrawLine(p2, 1, h - 2, w - 2, h - 2);
            g.DrawLine(p2, w - 2, 1, w - 2, h - 3);

            p2 = new Pen(Color.FromArgb(50, Color.Black));
            g.DrawLine(p2, 2, h - 1, w - 1, h - 1);
            g.DrawLine(p2, w - 1, 2, w - 1, h - 2);

            p2 = new Pen(Color.FromArgb(25, Color.Black));
            g.DrawLine(p2, 3, h - 0, w - 0, h - 1);
            g.DrawLine(p2, w - 0, 3, w - 1, h - 1);

        }


        private void btnSize_Click(object sender, EventArgs e)
        {
            if (IsCollapsed)
            {
                Expand();
            }
            else
            {
                Collapse();
            }
        }

        /// <summary>
        /// Expands this instance.
        /// </summary>
        public void Expand()
        {
            ResizeHeight();
            btnSize.Image = Resources.arrow_in;
            pnlControls.Visible = true;
            IsCollapsed = false;
            Refresh();
            Parent.Refresh();
        }

        /// <summary>
        /// Collapses this instance.
        /// </summary>
        public void Collapse()
        {
            ExpandedHeight = Height;
            Height = 30;
            pnlControls.Visible = false;
            btnSize.Image = Resources.arrow_out;
            IsCollapsed = true;
            Refresh();

            if (Parent != null)
                Parent.Refresh();
        }

        private void btnSize_MouseDown(object sender, MouseEventArgs e)
        {
            if (!IsCollapsed)
            {
                ExpandedHeight = Height;
            }
        }

        /// <summary>
        /// </summary>
        /// <value></value>
        [Browsable(true)]
        public override string Text
        {
            get
            {
                return lblCaption.Text;
            }
            set
            {
                lblCaption.Text = value;
            }
        }

        /// <summary>
        /// Adds the control.
        /// </summary>
        /// <param name="control">The control.</param>
        public void AddControl(Control control)
        {
            pnlControls.Controls.Add(control);
            control.Visible = true;

            if (AutoSize)
            {
                resizeControl();
            }
        }

        /// <summary>
        /// Gets the child controls.
        /// </summary>
        /// <value>The child controls.</value>
        public ControlCollection ChildControls
        {
            get { return pnlControls.Controls; }
        }

        private void resizeControl()
        {
            Height = pnlControls.Height + 32;
            Width = pnlControls.Width + 5;
        }

        /// <summary>
        /// Resizes the height.
        /// </summary>
        public void ResizeHeight()
        {
            Height = pnlControls.Height + 32;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
            foreach (Control c in pnlControls.Controls)
            {
                c.Dispose();
            }
            foreach (Control c in Controls)
            {
                c.Dispose();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [delete is visible].
        /// </summary>
        /// <value><c>true</c> if [delete is visible]; otherwise, <c>false</c>.</value>
        public bool DeleteIsVisible
        {
            get
            {
                return _deleteIsVisible;
            }
            set
            {
                _deleteIsVisible = value;
                btnDelete.Visible = value;
                if (value)
                {
                    lblCaption.Width = btnDelete.Left - 1;
                }
                else
                {
                    lblCaption.Width = btnSize.Left - 1;
                }
            }
        }

        private void btnDelete_MouseEnter(object sender, EventArgs e)
        {

        }

        private void btnSize_MouseEnter(object sender, EventArgs e)
        {
            if (IsCollapsed)
            {

                toolTip1.SetToolTip(btnSize, "Maximize");
            }
            else
            {
                toolTip1.SetToolTip(btnSize, "Minimize");
            }
        }

    }
}
