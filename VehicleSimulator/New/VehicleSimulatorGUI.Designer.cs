namespace VehicleSimulator.New
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
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VehicleSimulatorGUI));
			this.pnlTitle = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.lblIcon = new System.Windows.Forms.Label();
			this.btnMinimizeProgram = new System.Windows.Forms.Button();
			this.btnCloseProgram = new System.Windows.Forms.Button();
			this.pnlStatus = new System.Windows.Forms.Panel();
			this.pnlMenu = new System.Windows.Forms.Panel();
			this.btnMenuOfAbout = new System.Windows.Forms.Button();
			this.btnMenuOfSetting = new System.Windows.Forms.Button();
			this.btnMenuOfConsole = new System.Windows.Forms.Button();
			this.btnMenuOfSimulator = new System.Windows.Forms.Button();
			this.pnlContent = new System.Windows.Forms.Panel();
			this.ucContentOfAbout1 = new VehicleSimulator.New.UcContentOfAbout();
			this.ucContentOfSetting1 = new VehicleSimulator.New.UcContentOfSetting();
			this.ucContentOfConsole1 = new VehicleSimulator.New.UcContentOfConsole();
			this.ucContentOfSimulator1 = new VehicleSimulator.New.UcContentOfSimulator();
			this.pnlPaddingLeft = new System.Windows.Forms.Panel();
			this.pnlPaddingRight = new System.Windows.Forms.Panel();
			this.pnlPaddingTop = new System.Windows.Forms.Panel();
			this.pnlTitle.SuspendLayout();
			this.pnlMenu.SuspendLayout();
			this.pnlContent.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlTitle
			// 
			this.pnlTitle.Controls.Add(this.label2);
			this.pnlTitle.Controls.Add(this.lblIcon);
			this.pnlTitle.Controls.Add(this.btnMinimizeProgram);
			this.pnlTitle.Controls.Add(this.btnCloseProgram);
			this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTitle.Location = new System.Drawing.Point(0, 0);
			this.pnlTitle.Name = "pnlTitle";
			this.pnlTitle.Size = new System.Drawing.Size(800, 40);
			this.pnlTitle.TabIndex = 0;
			this.pnlTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrlTitle_MouseDown);
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Left;
			this.label2.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(40, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(400, 40);
			this.label2.TabIndex = 3;
			this.label2.Text = "Vehicle Simulator";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrlTitle_MouseDown);
			// 
			// lblIcon
			// 
			this.lblIcon.Dock = System.Windows.Forms.DockStyle.Left;
			this.lblIcon.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblIcon.ForeColor = System.Drawing.Color.White;
			this.lblIcon.Image = global::VehicleSimulator.Properties.Resources.icons8_adjust_32px;
			this.lblIcon.Location = new System.Drawing.Point(0, 0);
			this.lblIcon.Name = "lblIcon";
			this.lblIcon.Size = new System.Drawing.Size(40, 40);
			this.lblIcon.TabIndex = 2;
			this.lblIcon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrlTitle_MouseDown);
			// 
			// btnMinimizeProgram
			// 
			this.btnMinimizeProgram.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnMinimizeProgram.FlatAppearance.BorderSize = 0;
			this.btnMinimizeProgram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnMinimizeProgram.ForeColor = System.Drawing.Color.White;
			this.btnMinimizeProgram.Image = global::VehicleSimulator.Properties.Resources.icons8_minimize_window_32px;
			this.btnMinimizeProgram.Location = new System.Drawing.Point(720, 0);
			this.btnMinimizeProgram.Name = "btnMinimizeProgram";
			this.btnMinimizeProgram.Size = new System.Drawing.Size(40, 40);
			this.btnMinimizeProgram.TabIndex = 1;
			this.btnMinimizeProgram.UseVisualStyleBackColor = true;
			this.btnMinimizeProgram.Click += new System.EventHandler(this.btnMinimizeProgram_Click);
			// 
			// btnCloseProgram
			// 
			this.btnCloseProgram.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnCloseProgram.FlatAppearance.BorderSize = 0;
			this.btnCloseProgram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCloseProgram.ForeColor = System.Drawing.Color.White;
			this.btnCloseProgram.Image = global::VehicleSimulator.Properties.Resources.icons8_close_window_32px;
			this.btnCloseProgram.Location = new System.Drawing.Point(760, 0);
			this.btnCloseProgram.Name = "btnCloseProgram";
			this.btnCloseProgram.Size = new System.Drawing.Size(40, 40);
			this.btnCloseProgram.TabIndex = 0;
			this.btnCloseProgram.UseVisualStyleBackColor = true;
			this.btnCloseProgram.Click += new System.EventHandler(this.btnCloseProgram_Click);
			// 
			// pnlStatus
			// 
			this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlStatus.Location = new System.Drawing.Point(0, 560);
			this.pnlStatus.Name = "pnlStatus";
			this.pnlStatus.Size = new System.Drawing.Size(800, 40);
			this.pnlStatus.TabIndex = 1;
			// 
			// pnlMenu
			// 
			this.pnlMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
			this.pnlMenu.Controls.Add(this.btnMenuOfAbout);
			this.pnlMenu.Controls.Add(this.btnMenuOfSetting);
			this.pnlMenu.Controls.Add(this.btnMenuOfConsole);
			this.pnlMenu.Controls.Add(this.btnMenuOfSimulator);
			this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlMenu.Location = new System.Drawing.Point(20, 50);
			this.pnlMenu.Name = "pnlMenu";
			this.pnlMenu.Size = new System.Drawing.Size(760, 60);
			this.pnlMenu.TabIndex = 2;
			// 
			// btnMenuOfAbout
			// 
			this.btnMenuOfAbout.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnMenuOfAbout.FlatAppearance.BorderSize = 0;
			this.btnMenuOfAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnMenuOfAbout.ForeColor = System.Drawing.Color.White;
			this.btnMenuOfAbout.Image = global::VehicleSimulator.Properties.Resources.icons8_info_squared_32px;
			this.btnMenuOfAbout.Location = new System.Drawing.Point(255, 0);
			this.btnMenuOfAbout.Name = "btnMenuOfAbout";
			this.btnMenuOfAbout.Size = new System.Drawing.Size(85, 60);
			this.btnMenuOfAbout.TabIndex = 3;
			this.btnMenuOfAbout.Text = "About";
			this.btnMenuOfAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.btnMenuOfAbout.UseVisualStyleBackColor = true;
			this.btnMenuOfAbout.Click += new System.EventHandler(this.btnMenuOfAbout_Click);
			// 
			// btnMenuOfSetting
			// 
			this.btnMenuOfSetting.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnMenuOfSetting.FlatAppearance.BorderSize = 0;
			this.btnMenuOfSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnMenuOfSetting.ForeColor = System.Drawing.Color.White;
			this.btnMenuOfSetting.Image = global::VehicleSimulator.Properties.Resources.icons8_settings_32px;
			this.btnMenuOfSetting.Location = new System.Drawing.Point(170, 0);
			this.btnMenuOfSetting.Name = "btnMenuOfSetting";
			this.btnMenuOfSetting.Size = new System.Drawing.Size(85, 60);
			this.btnMenuOfSetting.TabIndex = 2;
			this.btnMenuOfSetting.Text = "Setting";
			this.btnMenuOfSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.btnMenuOfSetting.UseVisualStyleBackColor = true;
			this.btnMenuOfSetting.Click += new System.EventHandler(this.btnMenuOfSetting_Click);
			// 
			// btnMenuOfConsole
			// 
			this.btnMenuOfConsole.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnMenuOfConsole.FlatAppearance.BorderSize = 0;
			this.btnMenuOfConsole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnMenuOfConsole.ForeColor = System.Drawing.Color.White;
			this.btnMenuOfConsole.Image = global::VehicleSimulator.Properties.Resources.icons8_console_32px;
			this.btnMenuOfConsole.Location = new System.Drawing.Point(85, 0);
			this.btnMenuOfConsole.Name = "btnMenuOfConsole";
			this.btnMenuOfConsole.Size = new System.Drawing.Size(85, 60);
			this.btnMenuOfConsole.TabIndex = 1;
			this.btnMenuOfConsole.Text = "Console";
			this.btnMenuOfConsole.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.btnMenuOfConsole.UseVisualStyleBackColor = true;
			this.btnMenuOfConsole.Click += new System.EventHandler(this.btnMenuOfConsole_Click);
			// 
			// btnMenuOfSimulator
			// 
			this.btnMenuOfSimulator.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnMenuOfSimulator.FlatAppearance.BorderSize = 0;
			this.btnMenuOfSimulator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnMenuOfSimulator.ForeColor = System.Drawing.Color.White;
			this.btnMenuOfSimulator.Image = global::VehicleSimulator.Properties.Resources.icons8_heavy_vehicle_32px;
			this.btnMenuOfSimulator.Location = new System.Drawing.Point(0, 0);
			this.btnMenuOfSimulator.Name = "btnMenuOfSimulator";
			this.btnMenuOfSimulator.Size = new System.Drawing.Size(85, 60);
			this.btnMenuOfSimulator.TabIndex = 0;
			this.btnMenuOfSimulator.Text = "Simulator";
			this.btnMenuOfSimulator.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.btnMenuOfSimulator.UseVisualStyleBackColor = true;
			this.btnMenuOfSimulator.Click += new System.EventHandler(this.btnMenuOfSimulator_Click);
			// 
			// pnlContent
			// 
			this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
			this.pnlContent.Controls.Add(this.ucContentOfAbout1);
			this.pnlContent.Controls.Add(this.ucContentOfSetting1);
			this.pnlContent.Controls.Add(this.ucContentOfConsole1);
			this.pnlContent.Controls.Add(this.ucContentOfSimulator1);
			this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlContent.Location = new System.Drawing.Point(20, 110);
			this.pnlContent.Name = "pnlContent";
			this.pnlContent.Padding = new System.Windows.Forms.Padding(15);
			this.pnlContent.Size = new System.Drawing.Size(760, 450);
			this.pnlContent.TabIndex = 2;
			// 
			// ucContentOfAbout1
			// 
			this.ucContentOfAbout1.AutoScroll = true;
			this.ucContentOfAbout1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
			this.ucContentOfAbout1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucContentOfAbout1.Location = new System.Drawing.Point(15, 15);
			this.ucContentOfAbout1.Name = "ucContentOfAbout1";
			this.ucContentOfAbout1.Size = new System.Drawing.Size(730, 420);
			this.ucContentOfAbout1.TabIndex = 3;
			// 
			// ucContentOfSetting1
			// 
			this.ucContentOfSetting1.AutoScroll = true;
			this.ucContentOfSetting1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
			this.ucContentOfSetting1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucContentOfSetting1.Location = new System.Drawing.Point(15, 15);
			this.ucContentOfSetting1.Name = "ucContentOfSetting1";
			this.ucContentOfSetting1.Size = new System.Drawing.Size(730, 420);
			this.ucContentOfSetting1.TabIndex = 2;
			// 
			// ucContentOfConsole1
			// 
			this.ucContentOfConsole1.AutoScroll = true;
			this.ucContentOfConsole1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
			this.ucContentOfConsole1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucContentOfConsole1.Location = new System.Drawing.Point(15, 15);
			this.ucContentOfConsole1.Name = "ucContentOfConsole1";
			this.ucContentOfConsole1.Size = new System.Drawing.Size(730, 420);
			this.ucContentOfConsole1.TabIndex = 1;
			// 
			// ucContentOfSimulator1
			// 
			this.ucContentOfSimulator1.AutoScroll = true;
			this.ucContentOfSimulator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
			this.ucContentOfSimulator1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucContentOfSimulator1.Location = new System.Drawing.Point(15, 15);
			this.ucContentOfSimulator1.Name = "ucContentOfSimulator1";
			this.ucContentOfSimulator1.Size = new System.Drawing.Size(730, 420);
			this.ucContentOfSimulator1.TabIndex = 0;
			// 
			// pnlPaddingLeft
			// 
			this.pnlPaddingLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlPaddingLeft.Location = new System.Drawing.Point(0, 50);
			this.pnlPaddingLeft.Name = "pnlPaddingLeft";
			this.pnlPaddingLeft.Size = new System.Drawing.Size(20, 510);
			this.pnlPaddingLeft.TabIndex = 0;
			// 
			// pnlPaddingRight
			// 
			this.pnlPaddingRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlPaddingRight.Location = new System.Drawing.Point(780, 50);
			this.pnlPaddingRight.Name = "pnlPaddingRight";
			this.pnlPaddingRight.Size = new System.Drawing.Size(20, 510);
			this.pnlPaddingRight.TabIndex = 1;
			// 
			// pnlPaddingTop
			// 
			this.pnlPaddingTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlPaddingTop.Location = new System.Drawing.Point(0, 40);
			this.pnlPaddingTop.Name = "pnlPaddingTop";
			this.pnlPaddingTop.Size = new System.Drawing.Size(800, 10);
			this.pnlPaddingTop.TabIndex = 1;
			// 
			// VehicleSimulatorGUI
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
			this.ClientSize = new System.Drawing.Size(800, 600);
			this.Controls.Add(this.pnlContent);
			this.Controls.Add(this.pnlMenu);
			this.Controls.Add(this.pnlPaddingRight);
			this.Controls.Add(this.pnlPaddingLeft);
			this.Controls.Add(this.pnlPaddingTop);
			this.Controls.Add(this.pnlStatus);
			this.Controls.Add(this.pnlTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "VehicleSimulatorGUI";
			this.Text = "VehicleSimulatorGUI";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VehicleSimulatorGUI_FormClosing);
			this.Load += new System.EventHandler(this.VehicleSimulatorGUI_Load);
			this.pnlTitle.ResumeLayout(false);
			this.pnlMenu.ResumeLayout(false);
			this.pnlContent.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlTitle;
		private System.Windows.Forms.Panel pnlStatus;
		private System.Windows.Forms.Panel pnlMenu;
		private System.Windows.Forms.Panel pnlContent;
		private System.Windows.Forms.Panel pnlPaddingLeft;
		private System.Windows.Forms.Panel pnlPaddingRight;
		private System.Windows.Forms.Button btnCloseProgram;
		private System.Windows.Forms.Button btnMinimizeProgram;
		private System.Windows.Forms.Label lblIcon;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMenuOfSimulator;
        private System.Windows.Forms.Button btnMenuOfAbout;
        private System.Windows.Forms.Button btnMenuOfSetting;
        private System.Windows.Forms.Button btnMenuOfConsole;
        private UcContentOfAbout ucContentOfAbout1;
        private UcContentOfSetting ucContentOfSetting1;
        private UcContentOfConsole ucContentOfConsole1;
        private UcContentOfSimulator ucContentOfSimulator1;
        private System.Windows.Forms.Panel pnlPaddingTop;
    }
}