namespace VehicleSimulator
{
	partial class VehicleSimulatorGUI
	{
		/// <summary>
		/// 設計工具所需的變數。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清除任何使用中的資源。
		/// </summary>
		/// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 設計工具產生的程式碼

		/// <summary>
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.gluiCtrl1 = new GLUI.GLUICtrl();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tpMonitor = new System.Windows.Forms.TabPage();
			this.tpDebugMsg = new System.Windows.Forms.TabPage();
			this.chkRtxtDebugMsgAutoScroll = new System.Windows.Forms.CheckBox();
			this.chkVehicleSimulator = new System.Windows.Forms.CheckBox();
			this.rtxtDebugMessage = new System.Windows.Forms.RichTextBox();
			this.tpVehicleSettings = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtVehicleCycleTimes1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtVehiclePath1 = new System.Windows.Forms.TextBox();
			this.txtVehicleRotationSpeed1 = new System.Windows.Forms.TextBox();
			this.txtVehicleTranslationSpeed1 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtVehicleName1 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.btnAddVehicle = new System.Windows.Forms.Button();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.txtRemotePort = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtRemoteIP = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.btnConnectRemote = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.chkConsoleCommunicator = new System.Windows.Forms.CheckBox();
			this.cmenuRichTextboxProcess = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmenuItemClearRichTextbox = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1.SuspendLayout();
			this.tpMonitor.SuspendLayout();
			this.tpDebugMsg.SuspendLayout();
			this.tpVehicleSettings.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.cmenuRichTextboxProcess.SuspendLayout();
			this.SuspendLayout();
			// 
			// gluiCtrl1
			// 
			this.gluiCtrl1.AllowObjectMenu = true;
			this.gluiCtrl1.AllowUndoMenu = true;
			this.gluiCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gluiCtrl1.Location = new System.Drawing.Point(7, 7);
			this.gluiCtrl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.gluiCtrl1.Name = "gluiCtrl1";
			this.gluiCtrl1.ShowAxis = true;
			this.gluiCtrl1.ShowGrid = true;
			this.gluiCtrl1.Size = new System.Drawing.Size(978, 481);
			this.gluiCtrl1.TabIndex = 0;
			this.gluiCtrl1.Zoom = 10D;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tpMonitor);
			this.tabControl1.Controls.Add(this.tpDebugMsg);
			this.tabControl1.Controls.Add(this.tpVehicleSettings);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Location = new System.Drawing.Point(12, 30);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Padding = new System.Drawing.Point(20, 5);
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1000, 528);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tpMonitor
			// 
			this.tpMonitor.Controls.Add(this.gluiCtrl1);
			this.tpMonitor.Location = new System.Drawing.Point(4, 29);
			this.tpMonitor.Name = "tpMonitor";
			this.tpMonitor.Padding = new System.Windows.Forms.Padding(3);
			this.tpMonitor.Size = new System.Drawing.Size(992, 495);
			this.tpMonitor.TabIndex = 0;
			this.tpMonitor.Text = "Real-Time Monitor";
			this.tpMonitor.UseVisualStyleBackColor = true;
			// 
			// tpDebugMsg
			// 
			this.tpDebugMsg.Controls.Add(this.chkConsoleCommunicator);
			this.tpDebugMsg.Controls.Add(this.chkRtxtDebugMsgAutoScroll);
			this.tpDebugMsg.Controls.Add(this.chkVehicleSimulator);
			this.tpDebugMsg.Controls.Add(this.rtxtDebugMessage);
			this.tpDebugMsg.Location = new System.Drawing.Point(4, 29);
			this.tpDebugMsg.Name = "tpDebugMsg";
			this.tpDebugMsg.Padding = new System.Windows.Forms.Padding(3);
			this.tpDebugMsg.Size = new System.Drawing.Size(992, 495);
			this.tpDebugMsg.TabIndex = 1;
			this.tpDebugMsg.Text = "Debug Message";
			this.tpDebugMsg.UseVisualStyleBackColor = true;
			// 
			// chkRtxtDebugMsgAutoScroll
			// 
			this.chkRtxtDebugMsgAutoScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkRtxtDebugMsgAutoScroll.AutoSize = true;
			this.chkRtxtDebugMsgAutoScroll.Checked = true;
			this.chkRtxtDebugMsgAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkRtxtDebugMsgAutoScroll.Enabled = false;
			this.chkRtxtDebugMsgAutoScroll.Location = new System.Drawing.Point(891, 56);
			this.chkRtxtDebugMsgAutoScroll.Name = "chkRtxtDebugMsgAutoScroll";
			this.chkRtxtDebugMsgAutoScroll.Size = new System.Drawing.Size(95, 19);
			this.chkRtxtDebugMsgAutoScroll.TabIndex = 7;
			this.chkRtxtDebugMsgAutoScroll.Text = "Auto Scroll";
			this.chkRtxtDebugMsgAutoScroll.UseVisualStyleBackColor = true;
			// 
			// chkVehicleSimulator
			// 
			this.chkVehicleSimulator.AutoSize = true;
			this.chkVehicleSimulator.Checked = true;
			this.chkVehicleSimulator.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVehicleSimulator.Location = new System.Drawing.Point(6, 6);
			this.chkVehicleSimulator.Name = "chkVehicleSimulator";
			this.chkVehicleSimulator.Size = new System.Drawing.Size(132, 19);
			this.chkVehicleSimulator.TabIndex = 6;
			this.chkVehicleSimulator.Text = "Vehicle Simulator";
			this.chkVehicleSimulator.UseVisualStyleBackColor = true;
			this.chkVehicleSimulator.CheckedChanged += new System.EventHandler(this.chkVehicleSimulator_CheckedChanged);
			// 
			// rtxtDebugMessage
			// 
			this.rtxtDebugMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtxtDebugMessage.BackColor = System.Drawing.Color.Black;
			this.rtxtDebugMessage.ContextMenuStrip = this.cmenuRichTextboxProcess;
			this.rtxtDebugMessage.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.rtxtDebugMessage.ForeColor = System.Drawing.SystemColors.Control;
			this.rtxtDebugMessage.Location = new System.Drawing.Point(6, 81);
			this.rtxtDebugMessage.Name = "rtxtDebugMessage";
			this.rtxtDebugMessage.ReadOnly = true;
			this.rtxtDebugMessage.Size = new System.Drawing.Size(980, 408);
			this.rtxtDebugMessage.TabIndex = 5;
			this.rtxtDebugMessage.Text = "";
			// 
			// tpVehicleSettings
			// 
			this.tpVehicleSettings.Controls.Add(this.groupBox1);
			this.tpVehicleSettings.Location = new System.Drawing.Point(4, 29);
			this.tpVehicleSettings.Name = "tpVehicleSettings";
			this.tpVehicleSettings.Padding = new System.Windows.Forms.Padding(3);
			this.tpVehicleSettings.Size = new System.Drawing.Size(992, 495);
			this.tpVehicleSettings.TabIndex = 2;
			this.tpVehicleSettings.Text = "Vehicle Settings";
			this.tpVehicleSettings.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.LightSteelBlue;
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtVehicleCycleTimes1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtVehiclePath1);
			this.groupBox1.Controls.Add(this.txtVehicleRotationSpeed1);
			this.groupBox1.Controls.Add(this.txtVehicleTranslationSpeed1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtVehicleName1);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.btnAddVehicle);
			this.groupBox1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(20, 20);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(280, 238);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Vehicle Operation";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label5.Location = new System.Drawing.Point(20, 160);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(36, 15);
			this.label5.TabIndex = 24;
			this.label5.Text = "Path:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.Location = new System.Drawing.Point(20, 130);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 15);
			this.label1.TabIndex = 23;
			this.label1.Text = "Cycle Times:";
			// 
			// txtVehicleCycleTimes1
			// 
			this.txtVehicleCycleTimes1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtVehicleCycleTimes1.Location = new System.Drawing.Point(139, 127);
			this.txtVehicleCycleTimes1.Name = "txtVehicleCycleTimes1";
			this.txtVehicleCycleTimes1.Size = new System.Drawing.Size(120, 25);
			this.txtVehicleCycleTimes1.TabIndex = 22;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label2.Location = new System.Drawing.Point(20, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(44, 15);
			this.label2.TabIndex = 12;
			this.label2.Text = "Name:";
			// 
			// txtVehiclePath1
			// 
			this.txtVehiclePath1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtVehiclePath1.Location = new System.Drawing.Point(139, 157);
			this.txtVehiclePath1.Name = "txtVehiclePath1";
			this.txtVehiclePath1.Size = new System.Drawing.Size(120, 25);
			this.txtVehiclePath1.TabIndex = 21;
			// 
			// txtVehicleRotationSpeed1
			// 
			this.txtVehicleRotationSpeed1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtVehicleRotationSpeed1.Location = new System.Drawing.Point(139, 97);
			this.txtVehicleRotationSpeed1.Name = "txtVehicleRotationSpeed1";
			this.txtVehicleRotationSpeed1.Size = new System.Drawing.Size(120, 25);
			this.txtVehicleRotationSpeed1.TabIndex = 11;
			// 
			// txtVehicleTranslationSpeed1
			// 
			this.txtVehicleTranslationSpeed1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtVehicleTranslationSpeed1.Location = new System.Drawing.Point(139, 67);
			this.txtVehicleTranslationSpeed1.Name = "txtVehicleTranslationSpeed1";
			this.txtVehicleTranslationSpeed1.Size = new System.Drawing.Size(120, 25);
			this.txtVehicleTranslationSpeed1.TabIndex = 10;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label3.Location = new System.Drawing.Point(20, 70);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(113, 15);
			this.label3.TabIndex = 13;
			this.label3.Text = "Translation Speed:";
			// 
			// txtVehicleName1
			// 
			this.txtVehicleName1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtVehicleName1.Location = new System.Drawing.Point(139, 37);
			this.txtVehicleName1.Name = "txtVehicleName1";
			this.txtVehicleName1.Size = new System.Drawing.Size(120, 25);
			this.txtVehicleName1.TabIndex = 9;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label4.Location = new System.Drawing.Point(20, 100);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(97, 15);
			this.label4.TabIndex = 14;
			this.label4.Text = "Rotation Speed:";
			// 
			// btnAddVehicle
			// 
			this.btnAddVehicle.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnAddVehicle.Location = new System.Drawing.Point(63, 188);
			this.btnAddVehicle.Name = "btnAddVehicle";
			this.btnAddVehicle.Size = new System.Drawing.Size(197, 30);
			this.btnAddVehicle.TabIndex = 15;
			this.btnAddVehicle.Text = "Add";
			this.btnAddVehicle.UseVisualStyleBackColor = true;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.txtRemotePort);
			this.tabPage1.Controls.Add(this.label7);
			this.tabPage1.Controls.Add(this.txtRemoteIP);
			this.tabPage1.Controls.Add(this.label6);
			this.tabPage1.Controls.Add(this.btnConnectRemote);
			this.tabPage1.Location = new System.Drawing.Point(4, 29);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(992, 495);
			this.tabPage1.TabIndex = 3;
			this.tabPage1.Text = "Other Settings";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// txtRemotePort
			// 
			this.txtRemotePort.Location = new System.Drawing.Point(81, 67);
			this.txtRemotePort.Name = "txtRemotePort";
			this.txtRemotePort.Size = new System.Drawing.Size(100, 25);
			this.txtRemotePort.TabIndex = 4;
			this.txtRemotePort.Text = "8000";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(40, 70);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(35, 15);
			this.label7.TabIndex = 3;
			this.label7.Text = "Port:";
			// 
			// txtRemoteIP
			// 
			this.txtRemoteIP.Location = new System.Drawing.Point(81, 37);
			this.txtRemoteIP.Name = "txtRemoteIP";
			this.txtRemoteIP.Size = new System.Drawing.Size(100, 25);
			this.txtRemoteIP.TabIndex = 2;
			this.txtRemoteIP.Text = "127.0.0.1";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(40, 40);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(24, 15);
			this.label6.TabIndex = 1;
			this.label6.Text = "IP:";
			// 
			// btnConnectRemote
			// 
			this.btnConnectRemote.BackColor = System.Drawing.Color.LightPink;
			this.btnConnectRemote.Location = new System.Drawing.Point(44, 95);
			this.btnConnectRemote.Name = "btnConnectRemote";
			this.btnConnectRemote.Size = new System.Drawing.Size(138, 30);
			this.btnConnectRemote.TabIndex = 0;
			this.btnConnectRemote.Text = "Connect";
			this.btnConnectRemote.UseVisualStyleBackColor = false;
			this.btnConnectRemote.Click += new System.EventHandler(this.btnConnectRemote_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Location = new System.Drawing.Point(0, 561);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1024, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1024, 27);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(45, 23);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// chkConsoleCommunicator
			// 
			this.chkConsoleCommunicator.AutoSize = true;
			this.chkConsoleCommunicator.Checked = true;
			this.chkConsoleCommunicator.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkConsoleCommunicator.Location = new System.Drawing.Point(6, 31);
			this.chkConsoleCommunicator.Name = "chkConsoleCommunicator";
			this.chkConsoleCommunicator.Size = new System.Drawing.Size(162, 19);
			this.chkConsoleCommunicator.TabIndex = 8;
			this.chkConsoleCommunicator.Text = "Console Communicator";
			this.chkConsoleCommunicator.UseVisualStyleBackColor = true;
			this.chkConsoleCommunicator.CheckedChanged += new System.EventHandler(this.chkConsoleCommunicator_CheckedChanged);
			// 
			// cmenuRichTextboxProcess
			// 
			this.cmenuRichTextboxProcess.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.cmenuRichTextboxProcess.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenuItemClearRichTextbox});
			this.cmenuRichTextboxProcess.Name = "cmenuRichTextboxProcess";
			this.cmenuRichTextboxProcess.Size = new System.Drawing.Size(115, 28);
			// 
			// cmenuItemClearRichTextbox
			// 
			this.cmenuItemClearRichTextbox.Name = "cmenuItemClearRichTextbox";
			this.cmenuItemClearRichTextbox.Size = new System.Drawing.Size(175, 24);
			this.cmenuItemClearRichTextbox.Text = "Clear";
			this.cmenuItemClearRichTextbox.Click += new System.EventHandler(this.cmenuItemClearRichTextbox_Click);
			// 
			// VehicleSimulatorGUI
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1024, 583);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.tabControl1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "VehicleSimulatorGUI";
			this.Text = "Vehicle Simulator";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VehicleSimulatorGUI_FormClosing);
			this.Load += new System.EventHandler(this.VehicleSimulatorGUI_Load);
			this.tabControl1.ResumeLayout(false);
			this.tpMonitor.ResumeLayout(false);
			this.tpDebugMsg.ResumeLayout(false);
			this.tpDebugMsg.PerformLayout();
			this.tpVehicleSettings.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.cmenuRichTextboxProcess.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private GLUI.GLUICtrl gluiCtrl1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tpMonitor;
		private System.Windows.Forms.TabPage tpDebugMsg;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.CheckBox chkRtxtDebugMsgAutoScroll;
		private System.Windows.Forms.CheckBox chkVehicleSimulator;
		private System.Windows.Forms.RichTextBox rtxtDebugMessage;
		private System.Windows.Forms.TabPage tpVehicleSettings;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtVehicleName1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtVehicleTranslationSpeed1;
		private System.Windows.Forms.TextBox txtVehicleRotationSpeed1;
		private System.Windows.Forms.Button btnAddVehicle;
		private System.Windows.Forms.TextBox txtVehicleCycleTimes1;
		private System.Windows.Forms.TextBox txtVehiclePath1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TextBox txtRemotePort;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtRemoteIP;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnConnectRemote;
		private System.Windows.Forms.CheckBox chkConsoleCommunicator;
		private System.Windows.Forms.ContextMenuStrip cmenuRichTextboxProcess;
		private System.Windows.Forms.ToolStripMenuItem cmenuItemClearRichTextbox;
	}
}

