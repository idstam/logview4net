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
using System.Xml;
using logview4net.Properties;

namespace logview4net.Viewers
{
    /// <summary>
    /// Manages alla actions for a viewer in a session.
    /// </summary>
    public partial class ActionManager : UserControl
    {
        private ActionConfigurator _newAction;
        private Button _newButton;
        
        private TextViewer _viewer;
        
        /// <summary>
        /// Ceates an ActionManager
        /// </summary>
        public ActionManager()
        {
            InitializeComponent();            
        }
        
        //currentSession.Viewer.AddAction(a.ActiveAction);

        private void DirtyAction()
        {
        	if(_newAction.Dirty)
        	{
        		var msg = "The last action you created is not saved.\n\nDo you want to save it?";
        		if(MessageBox.Show(msg, "Save Action", MessageBoxButtons.YesNo) == DialogResult.Yes)
        		{
        			add_Click(_newButton, null);
        		}
        	}
        }
        /// <summary>
        /// Loads the configuration for the ActionManager
        /// </summary>
        /// <param name="viewer"></param>
        public void LoadConfiguration(TextViewer viewer)
        {	
            _viewer = viewer;
            
            Controls.Clear();

            //Loop through all actions and add thier configurators to controls
            //Also add 'Add' and 'Delete' buttons
            foreach (var a in _viewer.Actions)
            {
                var ac = new ActionConfigurator(a, _viewer);
                Controls.Add(ac);

                if (Controls.Count == 1)
                {
                    ac.ShowHeaders = true;
                }
                else
                {
                    ac.Top = Controls[Controls.Count - 2].Bottom + 3;
                    ac.ShowHeaders = false;
                }
                
                ac.Visible = true;
                
                //Delete button
                var b = new Button();
                Controls.Add(b);
                b.Top = ac.Bottom - 27;
                b.Left = ac.Left + ac.Width + 3;
                b.Size = new Size(25, 25);
                b.Visible = true;
                b.Tag = ac;
                b.Image = Resources.cross;
                b.Click += new EventHandler(delete_Click);
                toolTip1.SetToolTip(b, "Remove action from session.");
                if ((b.Right +7) > ClientRectangle.Width)
                {
                    var foo = (b.Right + 7) - ClientRectangle.Width;
                    Width += foo;
                }

            }
            
            //This is the configurator for a new action
            var ac1 = new ActionConfigurator(_viewer);
            _newAction = ac1;
            Controls.Add(ac1);
            ac1.ShowHeaders = false;
            if (Controls.Count < 2)
            {
                ac1.Top = 12;
                ac1.ShowHeaders = true;
            }
            else
            {
                ac1.Top = Controls[Controls.Count - 2].Bottom + 12;
            }
            ac1.Visible = true;

            var c = new Button();
            _newButton = c;
            Controls.Add(c);
            c.Top = ac1.Bottom - 27;
            c.Left = ac1.Left + ac1.Width + 3;
            c.Size = new Size(25 , 25);
            toolTip1.SetToolTip(c, "Add action to session.");
            c.Visible = true;
            c.Tag = ac1;
            c.Image = Resources.add;
            c.Click +=new EventHandler(add_Click);

            Invalidate();
            
        }

        void delete_Click(object sender, EventArgs e)
        {
            var b = (Button)sender;
            var a = (ActionConfigurator)b.Tag;
            _viewer.RemoveAction(a.ActiveAction);

            LoadConfiguration(_viewer);
        }

        void add_Click(object sender, EventArgs e)
        {
            var b = (Button)sender;
            var a = (ActionConfigurator)b.Tag;
            _viewer.AddAction(a.ActiveAction);

            LoadConfiguration(_viewer);
        }

        private void ActionManager_Paint(object sender, PaintEventArgs e)
        {
            
            
            if (Controls.Count > 2)
            {
                var foo = Controls[Controls.Count - 1].Top - 8;
                CreateGraphics().DrawLine(new Pen(Color.Black, 1), ClientRectangle.Left, foo, ClientRectangle.Right, foo);

                var g = CreateGraphics();

                g.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), ClientRectangle.Left, foo, ClientRectangle.Right, foo);
                g.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark), 1), ClientRectangle.Left, foo + 1, ClientRectangle.Right, foo + 1);

            }

        }

        
        void ActionManagerLoad(object sender, EventArgs ea)
        {
        	ParentForm.FormClosing += delegate(object form, FormClosingEventArgs e) { DirtyAction(); };
        }


    }
}
