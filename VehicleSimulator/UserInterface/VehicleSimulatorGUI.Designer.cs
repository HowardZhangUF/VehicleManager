namespace VehicleSimulator.UserInterface
{
	partial class VehicleSimulatorGUI
	{
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusLabelFill = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusLabelHostConnectState = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.menuHostConnection = new System.Windows.Forms.ToolStripMenuItem();
			this.menuHostIpPort = new System.Windows.Forms.ToolStripTextBox();
			this.menuHostConnect = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnVehicleSimulatorStartMove = new System.Windows.Forms.Button();
			this.txtVehicleSimulatorPath = new System.Windows.Forms.TextBox();
			this.btnVehicleSimulatorStopMove = new System.Windows.Forms.Button();
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// BottomToolStripPanel
			// 
			this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.BottomToolStripPanel.Name = "BottomToolStripPanel";
			this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// TopToolStripPanel
			// 
			this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.TopToolStripPanel.Name = "TopToolStripPanel";
			this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// RightToolStripPanel
			// 
			this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.RightToolStripPanel.Name = "RightToolStripPanel";
			this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// LeftToolStripPanel
			// 
			this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.LeftToolStripPanel.Name = "LeftToolStripPanel";
			this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// ContentPanel
			// 
			this.ContentPanel.Size = new System.Drawing.Size(318, 278);
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabelFill,
            this.statusLabelHostConnectState});
			this.statusStrip1.Location = new System.Drawing.Point(0, 426);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(800, 24);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusLabelFill
			// 
			this.statusLabelFill.Name = "statusLabelFill";
			this.statusLabelFill.Size = new System.Drawing.Size(744, 19);
			this.statusLabelFill.Spring = true;
			// 
			// statusLabelHostConnectState
			// 
			this.statusLabelHostConnectState.BackColor = System.Drawing.Color.LightPink;
			this.statusLabelHostConnectState.Name = "statusLabelHostConnectState";
			this.statusLabelHostConnectState.Size = new System.Drawing.Size(41, 19);
			this.statusLabelHostConnectState.Text = "Host";
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHostConnection});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(800, 27);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// menuHostConnection
			// 
			this.menuHostConnection.BackColor = System.Drawing.Color.LightPink;
			this.menuHostConnection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHostConnect,
            this.menuHostIpPort});
			this.menuHostConnection.Name = "menuHostConnection";
			this.menuHostConnection.Size = new System.Drawing.Size(136, 23);
			this.menuHostConnection.Text = "Host Connection";
			// 
			// menuHostIpPort
			// 
			this.menuHostIpPort.Name = "menuHostIpPort";
			this.menuHostIpPort.Size = new System.Drawing.Size(100, 27);
			this.menuHostIpPort.Text = "127.0.0.1:8000";
			// 
			// menuHostConnect
			// 
			this.menuHostConnect.Name = "menuHostConnect";
			this.menuHostConnect.Size = new System.Drawing.Size(216, 26);
			this.menuHostConnect.Text = "Connect";
			this.menuHostConnect.Click += new System.EventHandler(this.menuHostConnect_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.btnVehicleSimulatorStartMove, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtVehicleSimulatorPath, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnVehicleSimulatorStopMove, 1, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(407, 84);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(342, 123);
			this.tableLayoutPanel1.TabIndex = 4;
			// 
			// btnVehicleSimulatorStartMove
			// 
			this.btnVehicleSimulatorStartMove.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehicleSimulatorStartMove.Location = new System.Drawing.Point(203, 3);
			this.btnVehicleSimulatorStartMove.Name = "btnVehicleSimulatorStartMove";
			this.btnVehicleSimulatorStartMove.Size = new System.Drawing.Size(65, 117);
			this.btnVehicleSimulatorStartMove.TabIndex = 2;
			this.btnVehicleSimulatorStartMove.Text = "Move";
			this.btnVehicleSimulatorStartMove.UseVisualStyleBackColor = true;
			this.btnVehicleSimulatorStartMove.Click += new System.EventHandler(this.btnVehicleSimulatorStartMove_Click);
			// 
			// txtVehicleSimulatorPath
			// 
			this.txtVehicleSimulatorPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtVehicleSimulatorPath.Location = new System.Drawing.Point(3, 3);
			this.txtVehicleSimulatorPath.Multiline = true;
			this.txtVehicleSimulatorPath.Name = "txtVehicleSimulatorPath";
			this.txtVehicleSimulatorPath.Size = new System.Drawing.Size(194, 117);
			this.txtVehicleSimulatorPath.TabIndex = 0;
			// 
			// btnVehicleSimulatorStopMove
			// 
			this.btnVehicleSimulatorStopMove.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehicleSimulatorStopMove.Location = new System.Drawing.Point(274, 3);
			this.btnVehicleSimulatorStopMove.Name = "btnVehicleSimulatorStopMove";
			this.btnVehicleSimulatorStopMove.Size = new System.Drawing.Size(65, 117);
			this.btnVehicleSimulatorStopMove.TabIndex = 1;
			this.btnVehicleSimulatorStopMove.Text = "Stop";
			this.btnVehicleSimulatorStopMove.UseVisualStyleBackColor = true;
			this.btnVehicleSimulatorStopMove.Click += new System.EventHandler(this.btnVehicleSimulatorStopMove_Click);
			// 
			// VehicleSimulatorGUI
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "VehicleSimulatorGUI";
			this.Text = "VehicleSimulatorGUI";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VehicleSimulatorGUI_FormClosing);
			this.Load += new System.EventHandler(this.VehicleSimulatorGUI_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
		private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
		private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
		private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
		private System.Windows.Forms.ToolStripContentPanel ContentPanel;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel statusLabelFill;
		private System.Windows.Forms.ToolStripStatusLabel statusLabelHostConnectState;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuHostConnection;
		private System.Windows.Forms.ToolStripTextBox menuHostIpPort;
		private System.Windows.Forms.ToolStripMenuItem menuHostConnect;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox txtVehicleSimulatorPath;
		private System.Windows.Forms.Button btnVehicleSimulatorStopMove;
		private System.Windows.Forms.Button btnVehicleSimulatorStartMove;
	}
}