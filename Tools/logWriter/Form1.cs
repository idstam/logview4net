/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Security.AccessControl;
using System.Net.Sockets;

namespace logWriter
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox chkUdp;
		private System.Windows.Forms.CheckBox chkFile;
		private System.Windows.Forms.CheckBox chkEventLog;
		private System.Windows.Forms.CheckBox chkUDP_2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox txtForUdp8080;
		private System.ComponentModel.IContainer components;
        private NumericUpDown repeatCount;


        private FileStream _fs = null;
        private CheckBox chkTcp;
        private CheckBox chkUdpAny;
        private TextBox hostnameUdpAny;
        private TextBox portUdpAny;
        private GroupBox groupBoxPrefix;
        private CheckBox checkBoxPrefixCounter;
        private CheckBox checkBoxPrefixTime;
        private CheckBox checkBoxPrefixType;
        private ListBox listBoxEncoding;
        private Label labelEncoding;

	    private long ctr = 0;

		public Form1()
		{
			InitializeComponent();
            listBoxEncoding.SelectedIndex = 1;  // Unicode
            label1_Update();
        }

        public Form1(string msg)
        {
            InitializeComponent();
            label1_Update();
            txtForUdp8080.Text = msg;
            chkUdp.Checked = true;
            button1_Click(null, null);
        }
        
        /// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.chkUdp = new System.Windows.Forms.CheckBox();
            this.chkFile = new System.Windows.Forms.CheckBox();
            this.chkEventLog = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.chkUDP_2 = new System.Windows.Forms.CheckBox();
            this.txtForUdp8080 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.repeatCount = new System.Windows.Forms.NumericUpDown();
            this.chkTcp = new System.Windows.Forms.CheckBox();
            this.chkUdpAny = new System.Windows.Forms.CheckBox();
            this.hostnameUdpAny = new System.Windows.Forms.TextBox();
            this.portUdpAny = new System.Windows.Forms.TextBox();
            this.groupBoxPrefix = new System.Windows.Forms.GroupBox();
            this.checkBoxPrefixType = new System.Windows.Forms.CheckBox();
            this.checkBoxPrefixTime = new System.Windows.Forms.CheckBox();
            this.checkBoxPrefixCounter = new System.Windows.Forms.CheckBox();
            this.listBoxEncoding = new System.Windows.Forms.ListBox();
            this.labelEncoding = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repeatCount)).BeginInit();
            this.groupBoxPrefix.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1000;
            this.trackBar1.Location = new System.Drawing.Point(8, 8);
            this.trackBar1.Maximum = 10000;
            this.trackBar1.Minimum = 10;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(616, 45);
            this.trackBar1.SmallChange = 10;
            this.trackBar1.TabIndex = 0;
            this.trackBar1.TickFrequency = 250;
            this.trackBar1.Value = 1000;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // chkUdp
            // 
            this.chkUdp.Location = new System.Drawing.Point(24, 56);
            this.chkUdp.Name = "chkUdp";
            this.chkUdp.Size = new System.Drawing.Size(200, 24);
            this.chkUdp.TabIndex = 1;
            this.chkUdp.Text = "Send to UDP @ 8080";
            this.chkUdp.CheckedChanged += new System.EventHandler(this.chkUdp_CheckedChanged);
            // 
            // chkFile
            // 
            this.chkFile.Location = new System.Drawing.Point(24, 80);
            this.chkFile.Name = "chkFile";
            this.chkFile.Size = new System.Drawing.Size(432, 24);
            this.chkFile.TabIndex = 2;
            this.chkFile.Text = "Write to C:\\temp\\loglistenertest.txt";
            // 
            // chkEventLog
            // 
            this.chkEventLog.Location = new System.Drawing.Point(24, 104);
            this.chkEventLog.Name = "chkEventLog";
            this.chkEventLog.Size = new System.Drawing.Size(200, 24);
            this.chkEventLog.TabIndex = 3;
            this.chkEventLog.Text = "Write to Application event log";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(648, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(759, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Run";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkUDP_2
            // 
            this.chkUDP_2.Location = new System.Drawing.Point(24, 128);
            this.chkUDP_2.Name = "chkUDP_2";
            this.chkUDP_2.Size = new System.Drawing.Size(200, 24);
            this.chkUDP_2.TabIndex = 6;
            this.chkUDP_2.Text = "Send to UDP @ 8081";
            // 
            // txtForUdp8080
            // 
            this.txtForUdp8080.Location = new System.Drawing.Point(24, 289);
            this.txtForUdp8080.Multiline = true;
            this.txtForUdp8080.Name = "txtForUdp8080";
            this.txtForUdp8080.Size = new System.Drawing.Size(541, 108);
            this.txtForUdp8080.TabIndex = 7;
            this.txtForUdp8080.Text = "Msg A";
            this.txtForUdp8080.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(759, 53);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Send";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // repeatCount
            // 
            this.repeatCount.Location = new System.Drawing.Point(618, 56);
            this.repeatCount.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.repeatCount.Name = "repeatCount";
            this.repeatCount.Size = new System.Drawing.Size(120, 20);
            this.repeatCount.TabIndex = 9;
            this.repeatCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkTcp
            // 
            this.chkTcp.Location = new System.Drawing.Point(24, 152);
            this.chkTcp.Name = "chkTcp";
            this.chkTcp.Size = new System.Drawing.Size(200, 24);
            this.chkTcp.TabIndex = 10;
            this.chkTcp.Text = "Send to TCP @ 9999";
            // 
            // chkUdpAny
            // 
            this.chkUdpAny.Location = new System.Drawing.Point(24, 176);
            this.chkUdpAny.Name = "chkUdpAny";
            this.chkUdpAny.Size = new System.Drawing.Size(253, 24);
            this.chkUdpAny.TabIndex = 11;
            this.chkUdpAny.Text = "Send to UDP @";
            // 
            // hostnameUdpAny
            // 
            this.hostnameUdpAny.Location = new System.Drawing.Point(125, 178);
            this.hostnameUdpAny.Name = "hostnameUdpAny";
            this.hostnameUdpAny.Size = new System.Drawing.Size(90, 20);
            this.hostnameUdpAny.TabIndex = 12;
            this.hostnameUdpAny.Text = "127.0.0.1";
            this.hostnameUdpAny.TextChanged += new System.EventHandler(this.hostnameUdpAny_TextChanged);
            // 
            // portUdpAny
            // 
            this.portUdpAny.Location = new System.Drawing.Point(220, 178);
            this.portUdpAny.Name = "portUdpAny";
            this.portUdpAny.Size = new System.Drawing.Size(50, 20);
            this.portUdpAny.TabIndex = 13;
            this.portUdpAny.Text = "8080";
            this.portUdpAny.TextChanged += new System.EventHandler(this.portUdpAny_TextChanged);
            // 
            // groupBoxPrefix
            // 
            this.groupBoxPrefix.Controls.Add(this.checkBoxPrefixCounter);
            this.groupBoxPrefix.Controls.Add(this.checkBoxPrefixTime);
            this.groupBoxPrefix.Controls.Add(this.checkBoxPrefixType);
            this.groupBoxPrefix.Location = new System.Drawing.Point(618, 98);
            this.groupBoxPrefix.Name = "groupBoxPrefix";
            this.groupBoxPrefix.Size = new System.Drawing.Size(216, 100);
            this.groupBoxPrefix.TabIndex = 14;
            this.groupBoxPrefix.TabStop = false;
            this.groupBoxPrefix.Text = "Prefix";
            // 
            // checkBoxPrefixType
            // 
            this.checkBoxPrefixType.AutoSize = true;
            this.checkBoxPrefixType.Checked = true;
            this.checkBoxPrefixType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPrefixType.Location = new System.Drawing.Point(7, 20);
            this.checkBoxPrefixType.Name = "checkBoxPrefixType";
            this.checkBoxPrefixType.Size = new System.Drawing.Size(50, 17);
            this.checkBoxPrefixType.TabIndex = 0;
            this.checkBoxPrefixType.Text = "Type";
            this.checkBoxPrefixType.UseVisualStyleBackColor = true;
            // 
            // checkBoxPrefixTime
            // 
            this.checkBoxPrefixTime.AutoSize = true;
            this.checkBoxPrefixTime.Checked = true;
            this.checkBoxPrefixTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPrefixTime.Location = new System.Drawing.Point(7, 44);
            this.checkBoxPrefixTime.Name = "checkBoxPrefixTime";
            this.checkBoxPrefixTime.Size = new System.Drawing.Size(49, 17);
            this.checkBoxPrefixTime.TabIndex = 1;
            this.checkBoxPrefixTime.Text = "Time";
            this.checkBoxPrefixTime.UseVisualStyleBackColor = true;
            // 
            // checkBoxPrefixCounter
            // 
            this.checkBoxPrefixCounter.AutoSize = true;
            this.checkBoxPrefixCounter.Checked = true;
            this.checkBoxPrefixCounter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPrefixCounter.Location = new System.Drawing.Point(7, 68);
            this.checkBoxPrefixCounter.Name = "checkBoxPrefixCounter";
            this.checkBoxPrefixCounter.Size = new System.Drawing.Size(63, 17);
            this.checkBoxPrefixCounter.TabIndex = 2;
            this.checkBoxPrefixCounter.Text = "Counter";
            this.checkBoxPrefixCounter.UseVisualStyleBackColor = true;
            // 
            // listBoxEncoding
            // 
            this.listBoxEncoding.FormattingEnabled = true;
            this.listBoxEncoding.Items.AddRange(new object[] {
            "Ascii",
            "Unicode (UTF16)",
            "Unicode (UTF16) Big Endian",
            "UTF7",
            "UTF8",
            "UTF32"});
            this.listBoxEncoding.Location = new System.Drawing.Point(676, 204);
            this.listBoxEncoding.Name = "listBoxEncoding";
            this.listBoxEncoding.Size = new System.Drawing.Size(158, 82);
            this.listBoxEncoding.TabIndex = 15;
            // 
            // labelEncoding
            // 
            this.labelEncoding.AutoSize = true;
            this.labelEncoding.Location = new System.Drawing.Point(618, 204);
            this.labelEncoding.Name = "labelEncoding";
            this.labelEncoding.Size = new System.Drawing.Size(52, 13);
            this.labelEncoding.TabIndex = 16;
            this.labelEncoding.Text = "Encoding";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(856, 409);
            this.Controls.Add(this.labelEncoding);
            this.Controls.Add(this.listBoxEncoding);
            this.Controls.Add(this.groupBoxPrefix);
            this.Controls.Add(this.portUdpAny);
            this.Controls.Add(this.hostnameUdpAny);
            this.Controls.Add(this.chkUdpAny);
            this.Controls.Add(this.chkTcp);
            this.Controls.Add(this.repeatCount);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtForUdp8080);
            this.Controls.Add(this.chkUDP_2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkEventLog);
            this.Controls.Add(this.chkFile);
            this.Controls.Add(this.chkUdp);
            this.Controls.Add(this.trackBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repeatCount)).EndInit();
            this.groupBoxPrefix.ResumeLayout(false);
            this.groupBoxPrefix.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{
            

			Application.Run(new Form1());
		}

        private void label1_Update()
        {
            label1.Text = trackBar1.Value.ToString() + " ms";
        }

        private void trackBar1_Scroll(object sender, System.EventArgs e)
		{
            label1_Update();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if(button1.Text == "Run")
			{
				button1.Text = "Stop";
				timer1.Interval = trackBar1.Value;
				timer1.Enabled = true;
			}
			else
			{
				button1.Text = "Run";
				timer1.Enabled = false;
			}
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			send();			
		}

		private void chkUdp_CheckedChanged(object sender, System.EventArgs e)
		{
		}

		private void send()
		{
			if(chkUdp.Checked)
			{
                sendUDP(chkUdp, "127.0.0.1", 8080);
			}
			if(chkUDP_2.Checked)
			{
                sendUDP(chkUDP_2, "127.0.0.1", 8081);
			}
            if ((chkUdpAny.Checked) && (udpAny_Validate()))
            {
                string  hostname = hostnameUdpAny.Text.Trim();
                string  portStr = portUdpAny.Text.Trim();
                int     port = int.Parse(portStr);
                sendUDP(chkUdpAny, hostname, port);
            }
            if (chkTcp.Checked)
            {
                sendTCP(9999);
            }
            if (chkEventLog.Checked)
			{
				sendEventLog();
			}

            if (chkFile.Checked)
            {
                if (File.Exists(@"C:\temp\loglistenertest.txt"))
                {
                    _fs.WriteByte(99);
                    _fs.WriteByte(13);
                    _fs.WriteByte(10);
                    _fs.Flush();
                }
                else
                {
                    _fs = File.Open(@"C:\temp\loglistenertest.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                }
            }
            ctr++;

            Console.WriteLine(getMsg("StdOut"));
            

		}

        private void sendTCP(int port)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                TcpClient client = new TcpClient("localhost", port);

                // Translate the passed message and store it as a Byte array.
                System.Text.Encoding encoding = getEncoding();
                Byte[] data = encoding.GetBytes(txtForUdp8080.Text);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", txtForUdp8080.Text);

/*
                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
 */ 
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            
        }
        private void sendUDP(CheckBox checkBox, string hostname, int port)
		{
            sendUDP(checkBox, hostname, port, getMsg("UDP"));

		}
        private void sendUDP(CheckBox checkBox, string hostname, int port, string message)
		{
            bool no_error = true;
            try
            {
                System.Net.Sockets.UdpClient udp = new System.Net.Sockets.UdpClient(hostname, port);
                byte[] foo = MsgToData(message);

                udp.Send(foo, foo.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
                no_error = false;
            }

            checkBox.BackColor = no_error ? CheckBox.DefaultBackColor : Color.Red;
        }

		private string getMsg(string type)
		{
            string msg = "";
            if (checkBoxPrefixType.Checked)
                msg += type + " ";
            if (checkBoxPrefixTime.Checked)
                msg += "@" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("000") + " ";
            if (checkBoxPrefixCounter.Checked)
                msg += "{{" + ctr.ToString() + "}} ";

            msg += txtForUdp8080.Text;
            return msg;
        }

        private System.Text.Encoding getEncoding()
        {
            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            if (listBoxEncoding.SelectedItem == "Ascii")
            {
                encoding = System.Text.Encoding.ASCII;
            }
            else if (listBoxEncoding.SelectedItem == "UTF7")
            {
                encoding = System.Text.Encoding.UTF7;
            }
            else if (listBoxEncoding.SelectedItem == "UTF8")
            {
                encoding = System.Text.Encoding.UTF8;
            }
            else if (listBoxEncoding.SelectedItem == "UTF32")
            {
                encoding = System.Text.Encoding.UTF32;
            }
            else if (listBoxEncoding.SelectedItem.ToString().Contains("Big"))
            {
                encoding = System.Text.Encoding.BigEndianUnicode;
            }
            else
            {
                encoding = System.Text.Encoding.Unicode;
            }

            return encoding;
        }

        private byte[] MsgToData(string msg)
        {
            System.Text.Encoding encoding = getEncoding();

            Byte[] data = encoding.GetBytes(msg);
            return data;
        }

		private void sendEventLog()
		{
			System.Diagnostics.EventLog.WriteEntry("Application", getMsg("EventLog"));
		}

		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
            for (int i = 0; i < repeatCount.Value; i++)
            {
                send();
            }
		}

        private static bool hostname_port_Validate(string hostname, int port)
        {
            bool  no_error = true;
            try {
                // When hostname is not recognized as an ip address,
                //  UdpClient take it as an hostname and
                //  try to resolve it using network.
                // So it could take some times.
                // So I do not use it.
//                System.Net.Sockets.UdpClient udp = new System.Net.Sockets.UdpClient(hostname, port);

                // Parse accepts many things as ip address :
                // 127.0.0    127.0    127    6543
                System.Net.IPAddress ipadd;
                no_error = System.Net.IPAddress.TryParse(hostname, out ipadd);

                if (no_error)
                {
                    System.Net.Sockets.UdpClient udp = new System.Net.Sockets.UdpClient(hostname, port);
                }
            }
            catch(Exception)
            {
                no_error = false;
            }

            return no_error;
        }

        private static bool hostname_Validate(string hostname)
        {
            return  hostname_port_Validate(hostname, 8080);
        }

        private static int port_Validate(string portStr)
        {
            int    port = -1;
            bool   result = int.TryParse(portStr, out port);
            if ((result == false) || (port < 0) || (hostname_port_Validate("127.0.0.1", port) != true))
            {
                port = -1;
            }

            return  port;
        }

        private bool udpAny_Validate()
        {
            bool no_error = hostnameUdpAny_Validate();
            no_error = portUdpAny_Validate() && no_error;

            return no_error;
        }

        private bool hostnameUdpAny_Validate()
        {
            bool no_error = true;

            if (hostname_Validate(hostnameUdpAny.Text.Trim()) == false)
            {
                hostnameUdpAny.BackColor = Color.Red;
                no_error = false;
            }
            else
            {
                hostnameUdpAny.BackColor = TextBox.DefaultBackColor;
            }

            return no_error;
        }

        private void hostnameUdpAny_TextChanged(object sender, EventArgs e)
        {
            hostnameUdpAny_Validate();
        }

        private bool portUdpAny_Validate()
        {
            bool no_error = true;

            if (port_Validate(portUdpAny.Text.Trim()) < 0)
            {
                portUdpAny.BackColor = Color.Red;
                no_error = false;
            }
            else
            {
                portUdpAny.BackColor = TextBox.DefaultBackColor;
            }

            return no_error;
        }

        private void portUdpAny_TextChanged(object sender, EventArgs e)
        {
            portUdpAny_Validate();
        }
	}
}
