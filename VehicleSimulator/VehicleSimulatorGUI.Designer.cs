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
			this.gluiCtrl1 = new GLUI.GLUICtrl();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tpMonitor = new System.Windows.Forms.TabPage();
			this.tpDebugMsg = new System.Windows.Forms.TabPage();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.chkRtxtDebugMsgAutoScroll = new System.Windows.Forms.CheckBox();
			this.chkVehicleSimulator = new System.Windows.Forms.CheckBox();
			this.rtxtDebugMessage = new System.Windows.Forms.RichTextBox();
			this.tpVehicleSettings = new System.Windows.Forms.TabPage();
			this.tabControl1.SuspendLayout();
			this.tpMonitor.SuspendLayout();
			this.tpDebugMsg.SuspendLayout();
			this.menuStrip1.SuspendLayout();
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
			this.tabControl1.Location = new System.Drawing.Point(12, 30);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Padding = new System.Drawing.Point(20, 5);
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1000, 528);
			this.tabControl1.TabIndex = 1;
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
			// 
			// rtxtDebugMessage
			// 
			this.rtxtDebugMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtxtDebugMessage.BackColor = System.Drawing.Color.Black;
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
			this.tpVehicleSettings.Location = new System.Drawing.Point(4, 29);
			this.tpVehicleSettings.Name = "tpVehicleSettings";
			this.tpVehicleSettings.Padding = new System.Windows.Forms.Padding(3);
			this.tpVehicleSettings.Size = new System.Drawing.Size(992, 495);
			this.tpVehicleSettings.TabIndex = 2;
			this.tpVehicleSettings.Text = "Vehicle Settings";
			this.tpVehicleSettings.UseVisualStyleBackColor = true;
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
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
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
	}
}

