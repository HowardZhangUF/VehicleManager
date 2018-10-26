﻿namespace TrafficControlTest
{
	partial class TrafficControlTestGUI
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
			this.chkRtxtDebugMsgAutoScroll = new System.Windows.Forms.CheckBox();
			this.chkDebugMessage1 = new System.Windows.Forms.CheckBox();
			this.rtxtDebugMessage = new System.Windows.Forms.RichTextBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemLoadMap = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.chkDebugMessage3 = new System.Windows.Forms.CheckBox();
			this.chkDebugMessage2 = new System.Windows.Forms.CheckBox();
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
			this.gluiCtrl1.LoadMapEvent += new GLUI.LoadMapEvent(this.gluiCtrl1_LoadMapEvent);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tpMonitor);
			this.tabControl1.Controls.Add(this.tpDebugMsg);
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
			this.tpDebugMsg.Controls.Add(this.chkDebugMessage2);
			this.tpDebugMsg.Controls.Add(this.chkDebugMessage3);
			this.tpDebugMsg.Controls.Add(this.chkRtxtDebugMsgAutoScroll);
			this.tpDebugMsg.Controls.Add(this.chkDebugMessage1);
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
			this.chkRtxtDebugMsgAutoScroll.Location = new System.Drawing.Point(891, 56);
			this.chkRtxtDebugMsgAutoScroll.Name = "chkRtxtDebugMsgAutoScroll";
			this.chkRtxtDebugMsgAutoScroll.Size = new System.Drawing.Size(95, 19);
			this.chkRtxtDebugMsgAutoScroll.TabIndex = 2;
			this.chkRtxtDebugMsgAutoScroll.Text = "Auto Scroll";
			this.chkRtxtDebugMsgAutoScroll.UseVisualStyleBackColor = true;
			// 
			// chkDebugMessage1
			// 
			this.chkDebugMessage1.AutoSize = true;
			this.chkDebugMessage1.Location = new System.Drawing.Point(6, 6);
			this.chkDebugMessage1.Name = "chkDebugMessage1";
			this.chkDebugMessage1.Size = new System.Drawing.Size(129, 19);
			this.chkDebugMessage1.TabIndex = 1;
			this.chkDebugMessage1.Text = "Debug Message 1";
			this.chkDebugMessage1.UseVisualStyleBackColor = true;
			// 
			// rtxtDebugMessage
			// 
			this.rtxtDebugMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtxtDebugMessage.BackColor = System.Drawing.Color.Black;
			this.rtxtDebugMessage.Location = new System.Drawing.Point(6, 81);
			this.rtxtDebugMessage.Name = "rtxtDebugMessage";
			this.rtxtDebugMessage.Size = new System.Drawing.Size(980, 408);
			this.rtxtDebugMessage.TabIndex = 0;
			this.rtxtDebugMessage.Text = "";
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1024, 27);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// menuFile
			// 
			this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLoadMap});
			this.menuFile.Name = "menuFile";
			this.menuFile.Size = new System.Drawing.Size(45, 23);
			this.menuFile.Text = "File";
			// 
			// menuItemLoadMap
			// 
			this.menuItemLoadMap.Name = "menuItemLoadMap";
			this.menuItemLoadMap.Size = new System.Drawing.Size(181, 26);
			this.menuItemLoadMap.Text = "Load Map";
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Location = new System.Drawing.Point(0, 561);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1024, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// chkDebugMessage3
			// 
			this.chkDebugMessage3.AutoSize = true;
			this.chkDebugMessage3.Location = new System.Drawing.Point(6, 56);
			this.chkDebugMessage3.Name = "chkDebugMessage3";
			this.chkDebugMessage3.Size = new System.Drawing.Size(129, 19);
			this.chkDebugMessage3.TabIndex = 3;
			this.chkDebugMessage3.Text = "Debug Message 3";
			this.chkDebugMessage3.UseVisualStyleBackColor = true;
			// 
			// chkDebugMessage2
			// 
			this.chkDebugMessage2.AutoSize = true;
			this.chkDebugMessage2.Location = new System.Drawing.Point(6, 31);
			this.chkDebugMessage2.Name = "chkDebugMessage2";
			this.chkDebugMessage2.Size = new System.Drawing.Size(129, 19);
			this.chkDebugMessage2.TabIndex = 4;
			this.chkDebugMessage2.Text = "Debug Message 2";
			this.chkDebugMessage2.UseVisualStyleBackColor = true;
			// 
			// TrafficControlTestGUI
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1024, 583);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "TrafficControlTestGUI";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TrafficControlTestGUI_FormClosing);
			this.Load += new System.EventHandler(this.TrafficControlTestGUI_Load);
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
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuFile;
		private System.Windows.Forms.ToolStripMenuItem menuItemLoadMap;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.RichTextBox rtxtDebugMessage;
		private System.Windows.Forms.CheckBox chkDebugMessage1;
		private System.Windows.Forms.CheckBox chkRtxtDebugMsgAutoScroll;
		private System.Windows.Forms.CheckBox chkDebugMessage2;
		private System.Windows.Forms.CheckBox chkDebugMessage3;
	}
}

