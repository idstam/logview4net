/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace logview4net.Viewers
{
	/// <summary>
	/// Summary description for ActionConfigurator.
	/// </summary>
	public class ActionConfigurator : UserControl
	{
		private TextBox txtPattern;
		private ColorDialog colorDialog1;
		private ComboBox cboActionType;
		private Label lblActionInfo;
		private ToolTip toolTip1;
        private IContainer components;
        private Label label1;
        private Label label2;
        private bool _showHeaders = false;
        private Button btnFont;
        private FontDialog fontDlg;
        private TextBox txtExample;
		private Action _action;
        private ActionConfiguratorGetFileName getFileName;
		private bool _dirty = false;
        private TextViewer _viewer;

		/// <summary>
		/// Gets the active action.
		/// </summary>
		/// <value></value>
		public Action ActiveAction
		{
			get{ return _action; }
		}
		/// <summary>
		/// Creates a new <see cref="ActionConfigurator"/> instance.
		/// </summary>
        public ActionConfigurator(TextViewer viewer)
		{
            _viewer = viewer;
            _viewer.Txt.BackColorChanged += new EventHandler(txt_BackColorChanged);

            PreInitializeComponent();
            txtExample.BackColor = _viewer.Txt.BackColor;
		}

        /// <summary>
        /// Creates a new <see cref="ActionConfigurator"/> instance.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to configure.</param>
        /// <param name="viewer">The <see cref="TextViewer"/> connected.</param>
        public ActionConfigurator(Action action, TextViewer viewer)
        {
            _viewer = viewer;
            _viewer.Txt.BackColorChanged += new EventHandler(txt_BackColorChanged);
            
            PreInitializeComponent();
            _action = action;
            
            txtExample.BackColor = _viewer.Txt.BackColor;
        }

        void txt_BackColorChanged(object sender, EventArgs e)
        {
            txtExample.BackColor = (sender as RichTextBox).BackColor; 
        }

        private void PreInitializeComponent()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

            _action = Action.CreateHighlightAction("", Color.White);
            txtExample.Font = (Font)_viewer.Font.Clone();
            ShowHeaders = _showHeaders;
        }
        /// <summary>
        /// Toggles the visibility of headers fo the items in the control
        /// </summary>
        public bool ShowHeaders
        {
            get { return _showHeaders; }
            set {
                ToggleHeaders(value);
            }
        }

        private void ToggleHeaders(bool value)
        {
            _showHeaders = value;
            if (_showHeaders)
            {
                Height = 40;
            }
            else
            {
                Height = 27;
            }
            label1.Visible = _showHeaders;
            label2.Visible = _showHeaders;
        }




		private void populateControls()
		{
			txtPattern.Text = _action.Pattern;
			cboActionType.Text = _action.ActionType.ToString();
            txtExample.ForeColor = _action.Color;
            txtExample.Font = _action.Font;
            getFileName.FileName = _action.Data;

            getFileName.Visible = false;
            btnFont.Visible = false;
			switch(_action.ActionType)
			{
                case ActionTypes.Hide:
                    lblActionInfo.Visible = false;
                    txtExample.Visible = false;
                    break;
				case ActionTypes.Ignore:
					lblActionInfo.Visible = false;
                    txtExample.Visible = false;
					break;
                case ActionTypes.PopUp:
                    lblActionInfo.Visible = false;
                    txtExample.Visible = false;
                    break;
                case ActionTypes.PlaySound:
                    lblActionInfo.Visible = false;
                    txtExample.Visible = false;
                    getFileName.Config("Select the sound file to play.", "Wave Files(*.WAV)|*.WAV|All files (*.*)|*.*");
                    getFileName.Visible = true;

                    break;
                case ActionTypes.Execute:
                    lblActionInfo.Visible = false;
                    txtExample.Visible = false;
                    getFileName.Config("Select the file to execute.", "(*.EXE)|*.EXE|(*.CMD)|*.CMD|(*.BAT)|*.BAT|All files (*.*)|*.*");
                    getFileName.Visible = true;
                    break;
                default:
                    lblActionInfo.Visible = true;
                    txtExample.Visible = true;
                    btnFont.Visible = true;
                    break;
			}
		}


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.txtPattern = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.cboActionType = new System.Windows.Forms.ComboBox();
            this.lblActionInfo = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnFont = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fontDlg = new System.Windows.Forms.FontDialog();
            this.txtExample = new System.Windows.Forms.TextBox();
            this.getFileName = new logview4net.Viewers.ActionConfiguratorGetFileName();
            this.SuspendLayout();
            // 
            // txtPattern
            // 
            this.txtPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtPattern.Location = new System.Drawing.Point(3, 4);
            this.txtPattern.Name = "txtPattern";
            this.txtPattern.Size = new System.Drawing.Size(141, 20);
            this.txtPattern.TabIndex = 0;
            this.toolTip1.SetToolTip(this.txtPattern, "The string to macth in the event text.");
            this.txtPattern.TextChanged += new System.EventHandler(this.txtPattern_TextChanged);
            // 
            // cboActionType
            // 
            this.cboActionType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboActionType.Location = new System.Drawing.Point(147, 4);
            this.cboActionType.Name = "cboActionType";
            this.cboActionType.Size = new System.Drawing.Size(121, 21);
            this.cboActionType.TabIndex = 1;
            this.toolTip1.SetToolTip(this.cboActionType, "The action to take when there is a match.");
            this.cboActionType.SelectedIndexChanged += new System.EventHandler(this.cboActionType_SelectedIndexChanged);
            // 
            // lblActionInfo
            // 
            this.lblActionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblActionInfo.Location = new System.Drawing.Point(273, 6);
            this.lblActionInfo.Name = "lblActionInfo";
            this.lblActionInfo.Size = new System.Drawing.Size(36, 15);
            this.lblActionInfo.TabIndex = 2;
            // 
            // btnFont
            // 
            this.btnFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFont.Image = global::logview4net.Properties.Resources.style;
            this.btnFont.Location = new System.Drawing.Point(498, 0);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(25, 25);
            this.btnFont.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btnFont, "Edit font settings.");
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Pattern";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(144, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Action type";
            this.label2.Visible = false;
            // 
            // txtExample
            // 
            this.txtExample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtExample.BackColor = System.Drawing.Color.Black;
            this.txtExample.Location = new System.Drawing.Point(315, 3);
            this.txtExample.Name = "txtExample";
            this.txtExample.Size = new System.Drawing.Size(176, 20);
            this.txtExample.TabIndex = 6;
            this.txtExample.Text = "Example";
            // 
            // getFileName
            // 
            this.getFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.getFileName.BackColor = System.Drawing.Color.Transparent;
            this.getFileName.Location = new System.Drawing.Point(276, 1);
            this.getFileName.Name = "getFileName";
            this.getFileName.Size = new System.Drawing.Size(247, 25);
            this.getFileName.TabIndex = 7;
            this.getFileName.Visible = false;
            this.getFileName.FileNameChanged += new System.Action<string>(this.getFileName_FileNameChanged);
            // 
            // ActionConfigurator
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.getFileName);
            this.Controls.Add(this.lblActionInfo);
            this.Controls.Add(this.txtExample);
            this.Controls.Add(this.btnFont);
            this.Controls.Add(this.cboActionType);
            this.Controls.Add(this.txtPattern);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ActionConfigurator";
            this.Size = new System.Drawing.Size(525, 27);
            this.Load += new System.EventHandler(this.ActionConfigurator_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		/// <summary>
		/// Dirty turns true when the pattern changes.
		/// </summary>
		public bool Dirty 
		{
			get { return _dirty; }
		}

		private void ActionConfigurator_Load(object sender, EventArgs e)
		{
			fillEnumCombo(cboActionType, Type.GetType("logview4net.Viewers.ActionTypes"));
			populateControls();
		}

		private void fillEnumCombo(ComboBox cbo, Type enm)
		{
			cbo.Items.AddRange(Enum.GetNames(enm));
		}

		private void txtPattern_TextChanged(object sender, EventArgs e)
		{
			_action.Pattern = txtPattern.Text;
			_dirty = true;
		}

		private void cboActionType_SelectedIndexChanged(object sender, EventArgs e)
		{
			var selectedAction = ActionTypes.Highlight ;
			var ec = new EnumConverter(selectedAction.GetType());
			_action.ActionType  = (ActionTypes)ec.ConvertFrom(cboActionType.Text);
            populateControls();
		}

        private void btnFont_Click(object sender, EventArgs e)
        {
            fontDlg.ShowColor = true;
            fontDlg.Color = _action.Color;
            
            fontDlg.ShowDialog();

            txtExample.ForeColor = fontDlg.Color;
            txtExample.Font = fontDlg.Font;
            _action.Color = fontDlg.Color;
            _action.Font = fontDlg.Font;

            populateControls();
        }

        private void getFileName_FileNameChanged(string obj)
        {
            if ((!string.IsNullOrEmpty(getFileName.FileName)) && File.Exists(getFileName.FileName))
            {
                try
                {
                    _action.Data = getFileName.FileName;
                    _dirty = true;
                    if (_action.ActionType == ActionTypes.PlaySound)
                    {
                        var aex = new ActionExecutor(_action, _action.Pattern);
                        aex.Execute();
                    }
                }
                catch (Exception)
                {
                    if (_action.ActionType == ActionTypes.PlaySound)
                    {
                        var msg = string.Format("Woops. logview4net can not play that sound.{0}{0}Only PCM Wave files are supported.", Environment.NewLine);
                        MessageBox.Show(msg, "Sound error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        getFileName.FileName = "";
                    }
                }
            }
        }
	}
}
