/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System.Windows.Forms;

namespace logview4net.Controls
{
    /// <summary>
    /// This is a panel that aligns its controls to the left and stacks them on top of each other
    /// </summary>
    public partial class StackedPanel : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StackedPanel"/> class.
        /// </summary>
        public StackedPanel()
        {
            InitializeComponent();
        }

        private void StackedPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            RestackControls();
            
        }

        private void StackedPanel_ControlRemoved(object sender, ControlEventArgs e)
        {
            RestackControls();
        }
        /// <summary>
        /// Restacks the controls.
        /// </summary>
        public void RestackControls()
        {
            var lastBottom = _padding * -1;

            foreach (Control c in Controls)
            {
                c.Top = lastBottom + _padding;
                c.Left = 0;
                lastBottom = c.Bottom;
                c.Width = ClientRectangle.Width-3;
            }

        }

        /// <summary>
        /// Forces the control to invalidate its client area and immediately redraw itself and any child controls.
        /// </summary>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public override void Refresh()
        {
            RestackControls();
           base.Refresh();
        }

    }
}
