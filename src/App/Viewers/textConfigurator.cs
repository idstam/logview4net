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
using System.Windows.Forms;
using System.Xml;
using logview4net.Controls;

namespace logview4net.Viewers
{
	/// <summary>
	/// Summary description for rtfViewerConfigurator.
	/// </summary>
	public class textConfigurator : UserControl
	{
		#region My variables

		private ArrayList action = new ArrayList();
		private TextViewer _viewer = null;
		private ILog _log = Logger.GetLogger("logview4net.Viewers.textConfigurator");

		#endregion

		#region Bills variables

		private TextBox txtExample;
		private Button btnFont;
		private ColorPicker cpForeColor;
		private Label label5;
		private Label label6;
		private ColorPicker cpBackColor;
		private FontDialog font;

		#endregion

		private ToolTip toolTip1;
		private CheckBox chkCache;
		private CheckBox chkListenerHeader;
		private Label label1;
		private NumericUpDown udBufferSize;
		private Label label3;
		private IContainer components;

		#region Constructors

		/// <summary>
		/// Creates a new <see cref="textConfigurator"/> instance.
		/// </summary>
		public textConfigurator()
		{
			_viewer = new TextViewer();
			PreInitializeComponent();
		}

		/// <summary>
		/// Creates a new <see cref="textConfigurator"/> instance.
		/// </summary>
		/// <param name="viewer">Viewer.</param>
		public textConfigurator(TextViewer viewer)
		{
			_viewer = viewer;
			PreInitializeComponent();

		}

		private void PreInitializeComponent()
		{
			InitializeComponent();

			cpForeColor.Items = new KnownColorCollection(KnownColorFilter.Web);
			cpBackColor.Items = new KnownColorCollection(KnownColorFilter.Web);
			_viewer.Dock = DockStyle.Fill;
			txtExample.Font = (Font)_viewer.TextFont.Clone();
			cboRolling.SelectedIndex = 0;
		}
		

		#endregion

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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.txtExample = new System.Windows.Forms.TextBox();
			this.btnFont = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.font = new System.Windows.Forms.FontDialog();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.chkCache = new System.Windows.Forms.CheckBox();
			this.chkWhitespace = new System.Windows.Forms.CheckBox();
			this.chkListenerHeader = new System.Windows.Forms.CheckBox();
			this.chkUseLogFile = new System.Windows.Forms.CheckBox();
			this.btnLogFile = new System.Windows.Forms.Button();
			this.txtLogFile = new System.Windows.Forms.TextBox();
			this.cboRolling = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.saveFile = new System.Windows.Forms.SaveFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.udBufferSize = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.cpBackColor = new logview4net.Controls.ColorPicker();
			this.cpForeColor = new logview4net.Controls.ColorPicker();
			((System.ComponentModel.ISupportInitialize)(this.udBufferSize)).BeginInit();
			this.SuspendLayout();
			// 
			// txtExample
			// 
			this.txtExample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtExample.BackColor = System.Drawing.Color.Black;
			this.txtExample.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtExample.ForeColor = System.Drawing.Color.Lime;
			this.txtExample.Location = new System.Drawing.Point(475, 3);
			this.txtExample.Multiline = true;
			this.txtExample.Name = "txtExample";
			this.txtExample.Size = new System.Drawing.Size(148, 42);
			this.txtExample.TabIndex = 1;
			this.txtExample.Text = "Example";
			// 
			// btnFont
			// 
			this.btnFont.Image = global::logview4net.Properties.Resources.style;
			this.btnFont.Location = new System.Drawing.Point(434, 1);
			this.btnFont.Name = "btnFont";
			this.btnFont.Size = new System.Drawing.Size(25, 25);
			this.btnFont.TabIndex = 4;
			this.toolTip1.SetToolTip(this.btnFont, "Font settings");
			this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(24, 6);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(57, 18);
			this.label5.TabIndex = 16;
			this.label5.Text = "Fore color";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(226, 6);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(79, 18);
			this.label6.TabIndex = 18;
			this.label6.Text = "Back color";
			// 
			// chkCache
			// 
			this.chkCache.AutoSize = true;
			this.chkCache.Checked = true;
			this.chkCache.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCache.Location = new System.Drawing.Point(226, 34);
			this.chkCache.Name = "chkCache";
			this.chkCache.Size = new System.Drawing.Size(142, 17);
			this.chkCache.TabIndex = 19;
			this.chkCache.Text = "Cache events on pause.";
			this.toolTip1.SetToolTip(this.chkCache, "If this is checked events are stored in memory while the viewer is paused.\r\nIf it" +
		" is not checked events are discarded while the viewer is paused.");
			this.chkCache.UseVisualStyleBackColor = true;
			this.chkCache.CheckedChanged += new System.EventHandler(this.chkCache_CheckedChanged);
			// 
			// chkWhitespace
			// 
			this.chkWhitespace.AutoSize = true;
			this.chkWhitespace.Location = new System.Drawing.Point(27, 34);
			this.chkWhitespace.Name = "chkWhitespace";
			this.chkWhitespace.Size = new System.Drawing.Size(184, 17);
			this.chkWhitespace.TabIndex = 20;
			this.chkWhitespace.Text = "Remove surrounding white space";
			this.toolTip1.SetToolTip(this.chkWhitespace, "If this is checked any white space before or after a message will be removed.");
			this.chkWhitespace.UseVisualStyleBackColor = true;
			this.chkWhitespace.CheckedChanged += new System.EventHandler(this.ChkWhitespaceCheckedChanged);
			// 
			// chkListenerHeader
			// 
			this.chkListenerHeader.AutoSize = true;
			this.chkListenerHeader.Checked = true;
			this.chkListenerHeader.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkListenerHeader.Location = new System.Drawing.Point(27, 57);
			this.chkListenerHeader.Name = "chkListenerHeader";
			this.chkListenerHeader.Size = new System.Drawing.Size(128, 17);
			this.chkListenerHeader.TabIndex = 21;
			this.chkListenerHeader.Text = "Show listener header.";
			this.toolTip1.SetToolTip(this.chkListenerHeader, "If this is checked the listeners will add a header to each message.");
			this.chkListenerHeader.UseVisualStyleBackColor = true;
			this.chkListenerHeader.CheckedChanged += new System.EventHandler(this.chkListenerHeader_CheckedChanged);
			// 
			// chkUseLogFile
			// 
			this.chkUseLogFile.AutoSize = true;
			this.chkUseLogFile.Location = new System.Drawing.Point(27, 80);
			this.chkUseLogFile.Name = "chkUseLogFile";
			this.chkUseLogFile.Size = new System.Drawing.Size(82, 17);
			this.chkUseLogFile.TabIndex = 22;
			this.chkUseLogFile.Text = "Save to file:";
			this.toolTip1.SetToolTip(this.chkUseLogFile, "If this is checked all events are stored in a rolling log file.");
			this.chkUseLogFile.UseVisualStyleBackColor = true;
			this.chkUseLogFile.CheckedChanged += new System.EventHandler(this.ChkUseLogFileCheckedChanged);
			// 
			// btnLogFile
			// 
			this.btnLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLogFile.Enabled = false;
			this.btnLogFile.Image = global::logview4net.Properties.Resources.folder;
			this.btnLogFile.Location = new System.Drawing.Point(423, 74);
			this.btnLogFile.Name = "btnLogFile";
			this.btnLogFile.Size = new System.Drawing.Size(25, 25);
			this.btnLogFile.TabIndex = 28;
			this.toolTip1.SetToolTip(this.btnLogFile, "Filename");
			this.btnLogFile.Click += new System.EventHandler(this.BtnLogFileClick);
			// 
			// txtLogFile
			// 
			this.txtLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtLogFile.Enabled = false;
			this.txtLogFile.Location = new System.Drawing.Point(113, 78);
			this.txtLogFile.Name = "txtLogFile";
			this.txtLogFile.Size = new System.Drawing.Size(302, 20);
			this.txtLogFile.TabIndex = 24;
			this.toolTip1.SetToolTip(this.txtLogFile, "This is the base file name to use for storing all incoming events.");
			// 
			// cboRolling
			// 
			this.cboRolling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cboRolling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRolling.Enabled = false;
			this.cboRolling.FormattingEnabled = true;
			this.cboRolling.Items.AddRange(new object[] {
			"Daily",
			"Hourly",
			"100KB",
			"  1 MB",
			" 10MB"});
			this.cboRolling.Location = new System.Drawing.Point(502, 78);
			this.cboRolling.Name = "cboRolling";
			this.cboRolling.Size = new System.Drawing.Size(121, 21);
			this.cboRolling.TabIndex = 29;
			this.toolTip1.SetToolTip(this.cboRolling, "When to \'roll\' the log file.\r\nDaily and Hourly will give you a new file based on " +
		"time.\r\nThe other ones will give you a new file based on size.");
			this.cboRolling.SelectedIndexChanged += new System.EventHandler(this.CboRollingSelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(454, 81);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 27;
			this.label2.Text = "Rolling:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(25, 106);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(390, 18);
			this.label1.TabIndex = 30;
			this.label1.Text = "Max number of characters in the viewer.";
			// 
			// udBufferSize
			// 
			this.udBufferSize.Increment = new decimal(new int[] {
			10,
			0,
			0,
			0});
			this.udBufferSize.Location = new System.Drawing.Point(295, 104);
			this.udBufferSize.Maximum = new decimal(new int[] {
			2000000000,
			0,
			0,
			0});
			this.udBufferSize.Minimum = new decimal(new int[] {
			10,
			0,
			0,
			0});
			this.udBufferSize.Name = "udBufferSize";
			this.udBufferSize.Size = new System.Drawing.Size(120, 20);
			this.udBufferSize.TabIndex = 31;
			this.udBufferSize.Value = new decimal(new int[] {
			1000,
			0,
			0,
			0});
			this.udBufferSize.ValueChanged += new System.EventHandler(this.udBufferSize_ValueChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(421, 106);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(57, 18);
			this.label3.TabIndex = 32;
			this.label3.Text = "* 1000";
			// 
			// cpBackColor
			// 
			this.cpBackColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cpBackColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cpBackColor.Items = null;
			this.cpBackColor.Location = new System.Drawing.Point(287, 3);
			this.cpBackColor.Name = "cpBackColor";
			this.cpBackColor.Size = new System.Drawing.Size(128, 21);
			this.cpBackColor.TabIndex = 17;
			this.cpBackColor.SelectedIndexChanged += new System.EventHandler(this.cpBackColor_SelectedIndexChanged);
			// 
			// cpForeColor
			// 
			this.cpForeColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cpForeColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cpForeColor.Items = null;
			this.cpForeColor.Location = new System.Drawing.Point(83, 3);
			this.cpForeColor.Name = "cpForeColor";
			this.cpForeColor.Size = new System.Drawing.Size(137, 21);
			this.cpForeColor.TabIndex = 15;
			this.cpForeColor.SelectedIndexChanged += new System.EventHandler(this.cpForeColor_SelectedIndexChanged);
			// 
			// textConfigurator
			// 
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.label3);
			this.Controls.Add(this.udBufferSize);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cboRolling);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnLogFile);
			this.Controls.Add(this.txtLogFile);
			this.Controls.Add(this.chkListenerHeader);
			this.Controls.Add(this.chkWhitespace);
			this.Controls.Add(this.chkCache);
			this.Controls.Add(this.cpBackColor);
			this.Controls.Add(this.chkUseLogFile);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.cpForeColor);
			this.Controls.Add(this.btnFont);
			this.Controls.Add(this.txtExample);
			this.Controls.Add(this.label6);
			this.Name = "textConfigurator";
			this.Size = new System.Drawing.Size(626, 135);
			this.Load += new System.EventHandler(this.textConfigurator_Load);
			((System.ComponentModel.ISupportInitialize)(this.udBufferSize)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.SaveFileDialog saveFile;
		private System.Windows.Forms.ComboBox cboRolling;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnLogFile;
		private System.Windows.Forms.TextBox txtLogFile;
		private System.Windows.Forms.CheckBox chkUseLogFile;
		private System.Windows.Forms.CheckBox chkWhitespace;

		#endregion

		private void btnFont_Click(object sender, EventArgs e)
		{

			font.Font = (Font)_viewer.TextFont.Clone();

			font.ShowDialog();
			txtExample.Font = (Font)font.Font.Clone();
			_viewer.TextFont = (Font)font.Font.Clone();
		}

		private void textConfigurator_Load(object sender, EventArgs e)
		{
		}

		#region IViewerConfigurator members

		/// <summary>
		/// Gets the viewer for this configurator.
		/// </summary>
		/// <value></value>
		public TextViewer Viewer
		{
			get { return _viewer; }
			set
			{
				_viewer = value;
				cpForeColor.Text = _viewer.TextForeColor.Name;
				cpBackColor.Text = _viewer.TextBackColor.Name;
				var f = new Font(_viewer.Font.Name, _viewer.Font.Size);
				txtExample.Font = f;
				txtExample.ForeColor = _viewer.TextForeColor;
				txtExample.BackColor = _viewer.TextBackColor;
				chkCache.Checked = _viewer.CacheOnPause;
			}
		}

		/// <summary>
		/// Gets or sets the configuration data for this configurator.
		/// </summary>
		/// <value></value>
		public string Configuration
		{
			get { return _viewer.GetConfiguration(); }
			set
			{
				try
				{
					loadConfiguration(value);
				}
				catch (Exception ex)
				{
					_log.Warn(GetHashCode(), ex.ToString(), ex);
					throw;
				}
			}
		}

		#endregion

		private void loadConfiguration(string configuration)
		{
			var d = new XmlDocument();

			try
			{
				d.LoadXml(configuration);

				var foo = false;
				
				if(d.FirstChild.Attributes.GetNamedItem("useLog") != null)
				{
					bool.TryParse(d.FirstChild.Attributes["useLog"].InnerText, out foo);
				}
				chkUseLogFile.Checked = foo;
				_viewer.LogToFile = foo;
				
				if(d.FirstChild.Attributes.GetNamedItem("logFileName") != null)
				{
					txtLogFile.Text = d.FirstChild.Attributes["logFileName"].InnerText;
					_viewer.LogFile = d.FirstChild.Attributes["logFileName"].InnerText;
				}
				
				if(d.FirstChild.Attributes.GetNamedItem("logRoll") != null)
				{
					_viewer.LogRolling= d.FirstChild.Attributes["logRoll"].InnerText;
					var i = cboRolling.FindStringExact(_viewer.LogRolling);
					cboRolling.SelectedIndex = i;
				}

				
				if (d.FirstChild.Attributes.GetNamedItem("buffer") != null)
				{
					int intFoo;
					if (int.TryParse(d.FirstChild.Attributes["buffer"].InnerText, out intFoo))
					{
						_viewer.BufferSize = intFoo;
						intFoo /= 1000;
						if (udBufferSize.Maximum >= intFoo && udBufferSize.Minimum <= intFoo)
						{
							udBufferSize.Value = intFoo / 1000;
						}
						else
						{
							if (udBufferSize.Maximum < intFoo) udBufferSize.Value = udBufferSize.Maximum;
							if (udBufferSize.Minimum > intFoo) udBufferSize.Value = udBufferSize.Minimum;
						}
					}
					else
					{
						_viewer.BufferSize = 1000000;
					}

				}


				foo = true;
				if(d.FirstChild.Attributes.GetNamedItem("cacheOnPause") != null)
				{
					bool.TryParse(d.FirstChild.Attributes["cacheOnPause"].InnerText, out foo);
				}
				chkCache.Checked = foo;
				_viewer.CacheOnPause = foo;

				txtExample.ForeColor = Color.FromName(d.FirstChild.Attributes["forecolor"].InnerText);
				cpForeColor.Text = d.FirstChild.Attributes["forecolor"].InnerText;
				_viewer.TextForeColor = txtExample.ForeColor;

				txtExample.BackColor = Color.FromName(d.FirstChild.Attributes["backcolor"].InnerText);
				cpBackColor.Text = d.FirstChild.Attributes["backcolor"].InnerText;
				_viewer.TextBackColor = txtExample.BackColor;

				var f =
					new Font(d.FirstChild.Attributes["font"].InnerText,
							 Helpers.SafeFloatParse(d.FirstChild.Attributes["fontsize"].InnerText));
				txtExample.Font = f;
				_viewer.TextFont = f;

				var actions = d.SelectNodes("//action");

				foreach (XmlNode actionNode in actions)
				{
					_viewer.AddAction(Action.CreateGenericEventAction(actionNode));
				}
			}
			catch (Exception ex)
			{
				_log.Error(GetHashCode(), ex.ToString(), ex);
			}
		}

		private void cpForeColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cpForeColor.SelectedText.Length > 0)
			{
				txtExample.ForeColor = Color.FromName(cpForeColor.SelectedText);
				_viewer.TextForeColor = Color.FromName(cpForeColor.SelectedText);
			}
		}

		private void cpBackColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cpBackColor.SelectedText.Length > 0)
			{
				txtExample.BackColor = Color.FromName(cpBackColor.SelectedText);
				_viewer.TextBackColor = Color.FromName(cpBackColor.SelectedText);
			}
		}

		private void chkCache_CheckedChanged(object sender, EventArgs e)
		{
			_viewer.CacheOnPause = chkCache.Checked;
		}
		
		void ChkWhitespaceCheckedChanged(object sender, EventArgs e)
		{
			_viewer.RemoveWhitespace = chkWhitespace.Checked;     	
		}

		private void chkListenerHeader_CheckedChanged(object sender, EventArgs e)
		{
			_viewer.ShowListenerPrefix = chkListenerHeader.Checked;
		}
		
		void BtnLogFileClick(object sender, EventArgs e)
		{
			if(saveFile.ShowDialog() == DialogResult.OK)
			{
				txtLogFile.Text = saveFile.FileName;
				_viewer.LogFile = saveFile.FileName;
			}
		}
				
		void ChkUseLogFileCheckedChanged(object sender, EventArgs e)
		{
			txtLogFile.Enabled = chkUseLogFile.Checked;
			btnLogFile.Enabled = chkUseLogFile.Checked;
			cboRolling.Enabled = chkUseLogFile.Checked;
			_viewer.LogToFile = chkUseLogFile.Checked;
			
		}
		
		void CboRollingSelectedIndexChanged(object sender, EventArgs e)
		{
			_viewer.LogRolling = cboRolling.Text;
		}
		/// <summary>
		/// Validates the settings in the configurator.
		/// </summary>
		/// <returns>Trus if everything seems OK, False if there is a configuration error.</returns>
		public bool ValidateSettings()
		{
			if(chkUseLogFile.Checked && txtLogFile.Text == "")
			{
				chkUseLogFile.Checked = false;
			}
			return true;
		}

		private void udBufferSize_ValueChanged(object sender, EventArgs e)
		{

			if (udBufferSize.Value > int.MaxValue/1000)
			{
				_viewer.BufferSize = int.MaxValue;
			}
			else
			{
				_viewer.BufferSize = Convert.ToInt32(udBufferSize.Value * 1000);    
			}
			
		}
	}
}
