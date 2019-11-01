namespace TrafficControlTest.UserInterface
{
	partial class VehicleManagerGUI
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VehicleManagerGUI));
			this.pnlTopSide = new System.Windows.Forms.Panel();
			this.lblFormTitle = new System.Windows.Forms.Label();
			this.lblFormIcon = new System.Windows.Forms.Label();
			this.btnFormMinimize = new System.Windows.Forms.Button();
			this.btnFormClose = new System.Windows.Forms.Button();
			this.btnDisplayAbout = new System.Windows.Forms.Button();
			this.pnlBtmSide = new System.Windows.Forms.Panel();
			this.pnlDisplayPnlBtm = new System.Windows.Forms.Panel();
			this.btnDisplayPnlBtm = new System.Windows.Forms.Button();
			this.pnlConnection = new System.Windows.Forms.Panel();
			this.lblConnection = new System.Windows.Forms.Label();
			this.pnlConnectionRight = new System.Windows.Forms.Panel();
			this.pnlConnectionLeft = new System.Windows.Forms.Panel();
			this.pnlConnectionBottom = new System.Windows.Forms.Panel();
			this.pnlConnectionTop = new System.Windows.Forms.Panel();
			this.pnlLeftSide = new System.Windows.Forms.Panel();
			this.pnlLeftSideMarker = new System.Windows.Forms.Panel();
			this.btnDisplayVehicleManualControl = new System.Windows.Forms.Button();
			this.btnDisplayVehicleOverview = new System.Windows.Forms.Button();
			this.btnDisplayPnlLeftMain = new System.Windows.Forms.Button();
			this.btnDisplaySetting = new System.Windows.Forms.Button();
			this.pnlTop = new System.Windows.Forms.Panel();
			this.pnlTopMarker = new System.Windows.Forms.Panel();
			this.btnDisplayLog = new System.Windows.Forms.Button();
			this.btnDisplayMission = new System.Windows.Forms.Button();
			this.btnDisplayVehicle = new System.Windows.Forms.Button();
			this.btnDisplayMap = new System.Windows.Forms.Button();
			this.pnlRightMain = new System.Windows.Forms.Panel();
			this.pnlLeftMain = new System.Windows.Forms.Panel();
			this.pnlBtm = new System.Windows.Forms.Panel();
			this.ucLog1 = new TrafficControlTest.UserControl.UCLog();
			this.ucSetting1 = new TrafficControlTest.UserControl.UCSetting();
			this.ucMission1 = new TrafficControlTest.UserControl.UCMission();
			this.ucVehicle1 = new TrafficControlTest.UserControl.UCVehicle();
			this.ucMap1 = new TrafficControlTest.UserControl.UCMap();
			this.ucSimpleLog1 = new TrafficControlTest.UserControl.UCSimpleLog();
			this.ucAbout1 = new TrafficControlTest.UserControl.UCAbout();
			this.ucVehicleManualControl1 = new TrafficControlTest.UserControl.UCVehicleManualControl();
			this.ucVehicleOverview1 = new TrafficControlTest.UserControl.UCVehicleOverview();
			this.pnlTopSide.SuspendLayout();
			this.pnlBtmSide.SuspendLayout();
			this.pnlDisplayPnlBtm.SuspendLayout();
			this.pnlConnection.SuspendLayout();
			this.pnlLeftSide.SuspendLayout();
			this.pnlTop.SuspendLayout();
			this.pnlRightMain.SuspendLayout();
			this.pnlLeftMain.SuspendLayout();
			this.pnlBtm.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlTopSide
			// 
			this.pnlTopSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
			this.pnlTopSide.Controls.Add(this.lblFormTitle);
			this.pnlTopSide.Controls.Add(this.lblFormIcon);
			this.pnlTopSide.Controls.Add(this.btnFormMinimize);
			this.pnlTopSide.Controls.Add(this.btnFormClose);
			this.pnlTopSide.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTopSide.Location = new System.Drawing.Point(0, 0);
			this.pnlTopSide.Name = "pnlTopSide";
			this.pnlTopSide.Size = new System.Drawing.Size(1300, 50);
			this.pnlTopSide.TabIndex = 3;
			// 
			// lblFormTitle
			// 
			this.lblFormTitle.Font = new System.Drawing.Font("新細明體", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFormTitle.Location = new System.Drawing.Point(50, 0);
			this.lblFormTitle.Name = "lblFormTitle";
			this.lblFormTitle.Size = new System.Drawing.Size(700, 50);
			this.lblFormTitle.TabIndex = 6;
			this.lblFormTitle.Text = "Vehicle Manager";
			this.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblFormIcon
			// 
			this.lblFormIcon.Dock = System.Windows.Forms.DockStyle.Left;
			this.lblFormIcon.Font = new System.Drawing.Font("新細明體", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblFormIcon.Image = ((System.Drawing.Image)(resources.GetObject("lblFormIcon.Image")));
			this.lblFormIcon.Location = new System.Drawing.Point(0, 0);
			this.lblFormIcon.Name = "lblFormIcon";
			this.lblFormIcon.Size = new System.Drawing.Size(50, 50);
			this.lblFormIcon.TabIndex = 5;
			this.lblFormIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnFormMinimize
			// 
			this.btnFormMinimize.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnFormMinimize.FlatAppearance.BorderSize = 0;
			this.btnFormMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnFormMinimize.Image = ((System.Drawing.Image)(resources.GetObject("btnFormMinimize.Image")));
			this.btnFormMinimize.Location = new System.Drawing.Point(1200, 0);
			this.btnFormMinimize.Name = "btnFormMinimize";
			this.btnFormMinimize.Size = new System.Drawing.Size(50, 50);
			this.btnFormMinimize.TabIndex = 3;
			this.btnFormMinimize.UseVisualStyleBackColor = true;
			this.btnFormMinimize.Click += new System.EventHandler(this.btnFormMinimize_Click);
			// 
			// btnFormClose
			// 
			this.btnFormClose.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnFormClose.FlatAppearance.BorderSize = 0;
			this.btnFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnFormClose.Image = ((System.Drawing.Image)(resources.GetObject("btnFormClose.Image")));
			this.btnFormClose.Location = new System.Drawing.Point(1250, 0);
			this.btnFormClose.Name = "btnFormClose";
			this.btnFormClose.Size = new System.Drawing.Size(50, 50);
			this.btnFormClose.TabIndex = 2;
			this.btnFormClose.Text = " ";
			this.btnFormClose.UseVisualStyleBackColor = true;
			this.btnFormClose.Click += new System.EventHandler(this.btnFormClose_Click);
			// 
			// btnDisplayAbout
			// 
			this.btnDisplayAbout.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnDisplayAbout.FlatAppearance.BorderSize = 0;
			this.btnDisplayAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDisplayAbout.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnDisplayAbout.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplayAbout.Image")));
			this.btnDisplayAbout.Location = new System.Drawing.Point(0, 600);
			this.btnDisplayAbout.Name = "btnDisplayAbout";
			this.btnDisplayAbout.Size = new System.Drawing.Size(50, 50);
			this.btnDisplayAbout.TabIndex = 7;
			this.btnDisplayAbout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDisplayAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnDisplayAbout.UseVisualStyleBackColor = true;
			this.btnDisplayAbout.Click += new System.EventHandler(this.btnDisplayAbout_Click);
			// 
			// pnlBtmSide
			// 
			this.pnlBtmSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
			this.pnlBtmSide.Controls.Add(this.pnlConnection);
			this.pnlBtmSide.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBtmSide.Location = new System.Drawing.Point(0, 700);
			this.pnlBtmSide.Name = "pnlBtmSide";
			this.pnlBtmSide.Size = new System.Drawing.Size(1300, 50);
			this.pnlBtmSide.TabIndex = 4;
			// 
			// pnlDisplayPnlBtm
			// 
			this.pnlDisplayPnlBtm.Controls.Add(this.btnDisplayPnlBtm);
			this.pnlDisplayPnlBtm.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlDisplayPnlBtm.Location = new System.Drawing.Point(800, 0);
			this.pnlDisplayPnlBtm.Name = "pnlDisplayPnlBtm";
			this.pnlDisplayPnlBtm.Size = new System.Drawing.Size(50, 50);
			this.pnlDisplayPnlBtm.TabIndex = 1;
			// 
			// btnDisplayPnlBtm
			// 
			this.btnDisplayPnlBtm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnDisplayPnlBtm.FlatAppearance.BorderSize = 0;
			this.btnDisplayPnlBtm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDisplayPnlBtm.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplayPnlBtm.Image")));
			this.btnDisplayPnlBtm.Location = new System.Drawing.Point(0, 0);
			this.btnDisplayPnlBtm.Name = "btnDisplayPnlBtm";
			this.btnDisplayPnlBtm.Size = new System.Drawing.Size(50, 50);
			this.btnDisplayPnlBtm.TabIndex = 0;
			this.btnDisplayPnlBtm.UseVisualStyleBackColor = true;
			this.btnDisplayPnlBtm.Click += new System.EventHandler(this.btnDisplayPnlBtm_Click);
			// 
			// pnlConnection
			// 
			this.pnlConnection.Controls.Add(this.lblConnection);
			this.pnlConnection.Controls.Add(this.pnlConnectionRight);
			this.pnlConnection.Controls.Add(this.pnlConnectionLeft);
			this.pnlConnection.Controls.Add(this.pnlConnectionBottom);
			this.pnlConnection.Controls.Add(this.pnlConnectionTop);
			this.pnlConnection.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlConnection.Location = new System.Drawing.Point(1250, 0);
			this.pnlConnection.Name = "pnlConnection";
			this.pnlConnection.Size = new System.Drawing.Size(50, 50);
			this.pnlConnection.TabIndex = 0;
			// 
			// lblConnection
			// 
			this.lblConnection.BackColor = System.Drawing.Color.DarkRed;
			this.lblConnection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblConnection.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblConnection.Location = new System.Drawing.Point(5, 5);
			this.lblConnection.Name = "lblConnection";
			this.lblConnection.Size = new System.Drawing.Size(40, 40);
			this.lblConnection.TabIndex = 2;
			this.lblConnection.Text = "0";
			this.lblConnection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlConnectionRight
			// 
			this.pnlConnectionRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlConnectionRight.Location = new System.Drawing.Point(45, 5);
			this.pnlConnectionRight.Name = "pnlConnectionRight";
			this.pnlConnectionRight.Size = new System.Drawing.Size(5, 40);
			this.pnlConnectionRight.TabIndex = 1;
			// 
			// pnlConnectionLeft
			// 
			this.pnlConnectionLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlConnectionLeft.Location = new System.Drawing.Point(0, 5);
			this.pnlConnectionLeft.Name = "pnlConnectionLeft";
			this.pnlConnectionLeft.Size = new System.Drawing.Size(5, 40);
			this.pnlConnectionLeft.TabIndex = 1;
			// 
			// pnlConnectionBottom
			// 
			this.pnlConnectionBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlConnectionBottom.Location = new System.Drawing.Point(0, 45);
			this.pnlConnectionBottom.Name = "pnlConnectionBottom";
			this.pnlConnectionBottom.Size = new System.Drawing.Size(50, 5);
			this.pnlConnectionBottom.TabIndex = 1;
			// 
			// pnlConnectionTop
			// 
			this.pnlConnectionTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlConnectionTop.Location = new System.Drawing.Point(0, 0);
			this.pnlConnectionTop.Name = "pnlConnectionTop";
			this.pnlConnectionTop.Size = new System.Drawing.Size(50, 5);
			this.pnlConnectionTop.TabIndex = 1;
			// 
			// pnlLeftSide
			// 
			this.pnlLeftSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.pnlLeftSide.Controls.Add(this.pnlLeftSideMarker);
			this.pnlLeftSide.Controls.Add(this.btnDisplayAbout);
			this.pnlLeftSide.Controls.Add(this.btnDisplayVehicleManualControl);
			this.pnlLeftSide.Controls.Add(this.btnDisplayVehicleOverview);
			this.pnlLeftSide.Controls.Add(this.btnDisplayPnlLeftMain);
			this.pnlLeftSide.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlLeftSide.Location = new System.Drawing.Point(0, 50);
			this.pnlLeftSide.Name = "pnlLeftSide";
			this.pnlLeftSide.Size = new System.Drawing.Size(50, 650);
			this.pnlLeftSide.TabIndex = 5;
			// 
			// pnlLeftSideMarker
			// 
			this.pnlLeftSideMarker.BackColor = System.Drawing.Color.Aqua;
			this.pnlLeftSideMarker.Location = new System.Drawing.Point(0, 0);
			this.pnlLeftSideMarker.Name = "pnlLeftSideMarker";
			this.pnlLeftSideMarker.Size = new System.Drawing.Size(4, 650);
			this.pnlLeftSideMarker.TabIndex = 4;
			// 
			// btnDisplayVehicleManualControl
			// 
			this.btnDisplayVehicleManualControl.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnDisplayVehicleManualControl.FlatAppearance.BorderSize = 0;
			this.btnDisplayVehicleManualControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDisplayVehicleManualControl.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplayVehicleManualControl.Image")));
			this.btnDisplayVehicleManualControl.Location = new System.Drawing.Point(0, 100);
			this.btnDisplayVehicleManualControl.Name = "btnDisplayVehicleManualControl";
			this.btnDisplayVehicleManualControl.Size = new System.Drawing.Size(50, 50);
			this.btnDisplayVehicleManualControl.TabIndex = 1;
			this.btnDisplayVehicleManualControl.UseVisualStyleBackColor = true;
			this.btnDisplayVehicleManualControl.Click += new System.EventHandler(this.btnDisplayVehicleManualControl_Click);
			// 
			// btnDisplayVehicleOverview
			// 
			this.btnDisplayVehicleOverview.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnDisplayVehicleOverview.FlatAppearance.BorderSize = 0;
			this.btnDisplayVehicleOverview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDisplayVehicleOverview.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplayVehicleOverview.Image")));
			this.btnDisplayVehicleOverview.Location = new System.Drawing.Point(0, 50);
			this.btnDisplayVehicleOverview.Name = "btnDisplayVehicleOverview";
			this.btnDisplayVehicleOverview.Size = new System.Drawing.Size(50, 50);
			this.btnDisplayVehicleOverview.TabIndex = 2;
			this.btnDisplayVehicleOverview.UseVisualStyleBackColor = true;
			this.btnDisplayVehicleOverview.Click += new System.EventHandler(this.btnDisplayVehicleOverview_Click);
			// 
			// btnDisplayPnlLeftMain
			// 
			this.btnDisplayPnlLeftMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnDisplayPnlLeftMain.FlatAppearance.BorderSize = 0;
			this.btnDisplayPnlLeftMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDisplayPnlLeftMain.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplayPnlLeftMain.Image")));
			this.btnDisplayPnlLeftMain.Location = new System.Drawing.Point(0, 0);
			this.btnDisplayPnlLeftMain.Name = "btnDisplayPnlLeftMain";
			this.btnDisplayPnlLeftMain.Size = new System.Drawing.Size(50, 50);
			this.btnDisplayPnlLeftMain.TabIndex = 0;
			this.btnDisplayPnlLeftMain.UseVisualStyleBackColor = true;
			this.btnDisplayPnlLeftMain.Click += new System.EventHandler(this.btnDisplayPnlLeftMain_Click);
			// 
			// btnDisplaySetting
			// 
			this.btnDisplaySetting.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnDisplaySetting.FlatAppearance.BorderSize = 0;
			this.btnDisplaySetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDisplaySetting.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnDisplaySetting.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplaySetting.Image")));
			this.btnDisplaySetting.Location = new System.Drawing.Point(450, 0);
			this.btnDisplaySetting.Name = "btnDisplaySetting";
			this.btnDisplaySetting.Size = new System.Drawing.Size(150, 50);
			this.btnDisplaySetting.TabIndex = 3;
			this.btnDisplaySetting.Text = "  Setting";
			this.btnDisplaySetting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDisplaySetting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnDisplaySetting.UseVisualStyleBackColor = true;
			this.btnDisplaySetting.Click += new System.EventHandler(this.btnDisplaySetting_Click);
			// 
			// pnlTop
			// 
			this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.pnlTop.Controls.Add(this.pnlDisplayPnlBtm);
			this.pnlTop.Controls.Add(this.pnlTopMarker);
			this.pnlTop.Controls.Add(this.btnDisplayLog);
			this.pnlTop.Controls.Add(this.btnDisplaySetting);
			this.pnlTop.Controls.Add(this.btnDisplayMission);
			this.pnlTop.Controls.Add(this.btnDisplayVehicle);
			this.pnlTop.Controls.Add(this.btnDisplayMap);
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(450, 50);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(850, 50);
			this.pnlTop.TabIndex = 7;
			// 
			// pnlTopMarker
			// 
			this.pnlTopMarker.BackColor = System.Drawing.Color.Aqua;
			this.pnlTopMarker.Location = new System.Drawing.Point(0, 0);
			this.pnlTopMarker.Name = "pnlTopMarker";
			this.pnlTopMarker.Size = new System.Drawing.Size(850, 4);
			this.pnlTopMarker.TabIndex = 3;
			// 
			// btnDisplayLog
			// 
			this.btnDisplayLog.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnDisplayLog.FlatAppearance.BorderSize = 0;
			this.btnDisplayLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDisplayLog.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnDisplayLog.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplayLog.Image")));
			this.btnDisplayLog.Location = new System.Drawing.Point(600, 0);
			this.btnDisplayLog.Name = "btnDisplayLog";
			this.btnDisplayLog.Size = new System.Drawing.Size(120, 50);
			this.btnDisplayLog.TabIndex = 4;
			this.btnDisplayLog.Text = "  Log";
			this.btnDisplayLog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDisplayLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnDisplayLog.UseVisualStyleBackColor = true;
			this.btnDisplayLog.Click += new System.EventHandler(this.btnDisplayLog_Click);
			// 
			// btnDisplayMission
			// 
			this.btnDisplayMission.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnDisplayMission.FlatAppearance.BorderSize = 0;
			this.btnDisplayMission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDisplayMission.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnDisplayMission.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplayMission.Image")));
			this.btnDisplayMission.Location = new System.Drawing.Point(290, 0);
			this.btnDisplayMission.Name = "btnDisplayMission";
			this.btnDisplayMission.Size = new System.Drawing.Size(160, 50);
			this.btnDisplayMission.TabIndex = 2;
			this.btnDisplayMission.Text = "  Mission";
			this.btnDisplayMission.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDisplayMission.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnDisplayMission.UseVisualStyleBackColor = true;
			this.btnDisplayMission.Click += new System.EventHandler(this.btnDisplayMission_Click);
			// 
			// btnDisplayVehicle
			// 
			this.btnDisplayVehicle.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnDisplayVehicle.FlatAppearance.BorderSize = 0;
			this.btnDisplayVehicle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDisplayVehicle.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnDisplayVehicle.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplayVehicle.Image")));
			this.btnDisplayVehicle.Location = new System.Drawing.Point(130, 0);
			this.btnDisplayVehicle.Name = "btnDisplayVehicle";
			this.btnDisplayVehicle.Size = new System.Drawing.Size(160, 50);
			this.btnDisplayVehicle.TabIndex = 1;
			this.btnDisplayVehicle.Text = "  Vehicle";
			this.btnDisplayVehicle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDisplayVehicle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnDisplayVehicle.UseVisualStyleBackColor = true;
			this.btnDisplayVehicle.Click += new System.EventHandler(this.btnDisplayVehicle_Click);
			// 
			// btnDisplayMap
			// 
			this.btnDisplayMap.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnDisplayMap.FlatAppearance.BorderSize = 0;
			this.btnDisplayMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDisplayMap.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnDisplayMap.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplayMap.Image")));
			this.btnDisplayMap.Location = new System.Drawing.Point(0, 0);
			this.btnDisplayMap.Name = "btnDisplayMap";
			this.btnDisplayMap.Size = new System.Drawing.Size(130, 50);
			this.btnDisplayMap.TabIndex = 0;
			this.btnDisplayMap.Text = "  Map";
			this.btnDisplayMap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDisplayMap.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnDisplayMap.UseVisualStyleBackColor = true;
			this.btnDisplayMap.Click += new System.EventHandler(this.btnDisplayMap_Click);
			// 
			// pnlRightMain
			// 
			this.pnlRightMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.pnlRightMain.Controls.Add(this.ucLog1);
			this.pnlRightMain.Controls.Add(this.ucSetting1);
			this.pnlRightMain.Controls.Add(this.ucMission1);
			this.pnlRightMain.Controls.Add(this.ucVehicle1);
			this.pnlRightMain.Controls.Add(this.ucMap1);
			this.pnlRightMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlRightMain.Location = new System.Drawing.Point(450, 100);
			this.pnlRightMain.Name = "pnlRightMain";
			this.pnlRightMain.Size = new System.Drawing.Size(850, 350);
			this.pnlRightMain.TabIndex = 8;
			// 
			// pnlLeftMain
			// 
			this.pnlLeftMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.pnlLeftMain.Controls.Add(this.ucAbout1);
			this.pnlLeftMain.Controls.Add(this.ucVehicleManualControl1);
			this.pnlLeftMain.Controls.Add(this.ucVehicleOverview1);
			this.pnlLeftMain.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlLeftMain.Location = new System.Drawing.Point(50, 50);
			this.pnlLeftMain.Name = "pnlLeftMain";
			this.pnlLeftMain.Size = new System.Drawing.Size(400, 650);
			this.pnlLeftMain.TabIndex = 6;
			// 
			// pnlBtm
			// 
			this.pnlBtm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.pnlBtm.Controls.Add(this.ucSimpleLog1);
			this.pnlBtm.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBtm.Location = new System.Drawing.Point(450, 450);
			this.pnlBtm.Name = "pnlBtm";
			this.pnlBtm.Size = new System.Drawing.Size(850, 250);
			this.pnlBtm.TabIndex = 5;
			// 
			// ucLog1
			// 
			this.ucLog1.AutoScroll = true;
			this.ucLog1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.ucLog1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucLog1.ForeColor = System.Drawing.Color.White;
			this.ucLog1.Location = new System.Drawing.Point(0, 0);
			this.ucLog1.Name = "ucLog1";
			this.ucLog1.Size = new System.Drawing.Size(850, 350);
			this.ucLog1.TabIndex = 4;
			// 
			// ucSetting1
			// 
			this.ucSetting1.AutoScroll = true;
			this.ucSetting1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.ucSetting1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucSetting1.ForeColor = System.Drawing.Color.White;
			this.ucSetting1.Location = new System.Drawing.Point(0, 0);
			this.ucSetting1.Name = "ucSetting1";
			this.ucSetting1.Size = new System.Drawing.Size(850, 350);
			this.ucSetting1.TabIndex = 3;
			// 
			// ucMission1
			// 
			this.ucMission1.AutoScroll = true;
			this.ucMission1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.ucMission1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucMission1.ForeColor = System.Drawing.Color.White;
			this.ucMission1.Location = new System.Drawing.Point(0, 0);
			this.ucMission1.Name = "ucMission1";
			this.ucMission1.Size = new System.Drawing.Size(850, 350);
			this.ucMission1.TabIndex = 2;
			this.ucMission1.TableBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.ucMission1.TableEvenRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.ucMission1.TableEvenRowExecutingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
			this.ucMission1.TableGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(86)))), ((int)(((byte)(86)))));
			this.ucMission1.TableHeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(122)))), ((int)(((byte)(233)))));
			this.ucMission1.TableHeaderForeColor = System.Drawing.Color.White;
			this.ucMission1.TableOddRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.ucMission1.TableOddRowExecutingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(128)))), ((int)(((byte)(76)))));
			this.ucMission1.TableRowForeColor = System.Drawing.Color.White;
			// 
			// ucVehicle1
			// 
			this.ucVehicle1.AutoScroll = true;
			this.ucVehicle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.ucVehicle1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucVehicle1.ForeColor = System.Drawing.Color.White;
			this.ucVehicle1.Location = new System.Drawing.Point(0, 0);
			this.ucVehicle1.Name = "ucVehicle1";
			this.ucVehicle1.Size = new System.Drawing.Size(850, 350);
			this.ucVehicle1.TabIndex = 1;
			// 
			// ucMap1
			// 
			this.ucMap1.AutoScroll = true;
			this.ucMap1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.ucMap1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucMap1.ForeColor = System.Drawing.Color.White;
			this.ucMap1.Location = new System.Drawing.Point(0, 0);
			this.ucMap1.Name = "ucMap1";
			this.ucMap1.Size = new System.Drawing.Size(850, 350);
			this.ucMap1.TabIndex = 0;
			// 
			// ucSimpleLog1
			// 
			this.ucSimpleLog1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.ucSimpleLog1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucSimpleLog1.Location = new System.Drawing.Point(0, 0);
			this.ucSimpleLog1.Maximum = 200;
			this.ucSimpleLog1.Name = "ucSimpleLog1";
			this.ucSimpleLog1.OrderAscending = false;
			this.ucSimpleLog1.Size = new System.Drawing.Size(850, 250);
			this.ucSimpleLog1.TabIndex = 0;
			this.ucSimpleLog1.TableBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.ucSimpleLog1.TableEvenRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.ucSimpleLog1.TableExceptionRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
			this.ucSimpleLog1.TableGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(86)))), ((int)(((byte)(86)))));
			this.ucSimpleLog1.TableHeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(122)))), ((int)(((byte)(233)))));
			this.ucSimpleLog1.TableHeaderForeColor = System.Drawing.Color.White;
			this.ucSimpleLog1.TableOddRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.ucSimpleLog1.TableRowForeColor = System.Drawing.Color.White;
			// 
			// ucAbout1
			// 
			this.ucAbout1.AutoScroll = true;
			this.ucAbout1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.ucAbout1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucAbout1.ForeColor = System.Drawing.Color.White;
			this.ucAbout1.Location = new System.Drawing.Point(0, 0);
			this.ucAbout1.Name = "ucAbout1";
			this.ucAbout1.Size = new System.Drawing.Size(400, 650);
			this.ucAbout1.TabIndex = 2;
			// 
			// ucVehicleManualControl1
			// 
			this.ucVehicleManualControl1.AutoScroll = true;
			this.ucVehicleManualControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.ucVehicleManualControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucVehicleManualControl1.ForeColor = System.Drawing.Color.White;
			this.ucVehicleManualControl1.Location = new System.Drawing.Point(0, 0);
			this.ucVehicleManualControl1.Name = "ucVehicleManualControl1";
			this.ucVehicleManualControl1.Size = new System.Drawing.Size(400, 650);
			this.ucVehicleManualControl1.TabIndex = 1;
			// 
			// ucVehicleOverview1
			// 
			this.ucVehicleOverview1.AutoScroll = true;
			this.ucVehicleOverview1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.ucVehicleOverview1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucVehicleOverview1.ForeColor = System.Drawing.Color.White;
			this.ucVehicleOverview1.Location = new System.Drawing.Point(0, 0);
			this.ucVehicleOverview1.Name = "ucVehicleOverview1";
			this.ucVehicleOverview1.Size = new System.Drawing.Size(400, 650);
			this.ucVehicleOverview1.TabIndex = 0;
			this.ucVehicleOverview1.DoubleClickOnVehicleInfo += new TrafficControlTest.UserControl.UCVehicleOverview.EventHandlerString(this.ucVehicleOverview1_DoubleClickOnVehicleInfo);
			// 
			// VehicleManagerGUI
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.ClientSize = new System.Drawing.Size(1300, 750);
			this.Controls.Add(this.pnlRightMain);
			this.Controls.Add(this.pnlBtm);
			this.Controls.Add(this.pnlTop);
			this.Controls.Add(this.pnlLeftMain);
			this.Controls.Add(this.pnlLeftSide);
			this.Controls.Add(this.pnlBtmSide);
			this.Controls.Add(this.pnlTopSide);
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.Name = "VehicleManagerGUI";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "VehicleManagerGUI";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VehicleManagerGUI_FormClosing);
			this.Load += new System.EventHandler(this.VehicleManagerGUI_Load);
			this.pnlTopSide.ResumeLayout(false);
			this.pnlBtmSide.ResumeLayout(false);
			this.pnlDisplayPnlBtm.ResumeLayout(false);
			this.pnlConnection.ResumeLayout(false);
			this.pnlLeftSide.ResumeLayout(false);
			this.pnlTop.ResumeLayout(false);
			this.pnlRightMain.ResumeLayout(false);
			this.pnlLeftMain.ResumeLayout(false);
			this.pnlBtm.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel pnlTopSide;
		private System.Windows.Forms.Panel pnlBtmSide;
		private System.Windows.Forms.Panel pnlLeftSide;
		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Panel pnlRightMain;
		private System.Windows.Forms.Button btnDisplayPnlLeftMain;
		private System.Windows.Forms.Button btnDisplaySetting;
		private System.Windows.Forms.Button btnFormMinimize;
		private System.Windows.Forms.Button btnFormClose;
		private System.Windows.Forms.Button btnDisplayMission;
		private System.Windows.Forms.Button btnDisplayMap;
		private System.Windows.Forms.Panel pnlTopMarker;
		private System.Windows.Forms.Panel pnlLeftSideMarker;
		private System.Windows.Forms.Button btnDisplayLog;
		private System.Windows.Forms.Label lblFormIcon;
		private System.Windows.Forms.Label lblFormTitle;
		private System.Windows.Forms.Button btnDisplayAbout;
		private System.Windows.Forms.Button btnDisplayVehicle;
		private System.Windows.Forms.Button btnDisplayVehicleManualControl;
		private System.Windows.Forms.Button btnDisplayVehicleOverview;
		private System.Windows.Forms.Panel pnlConnection;
		private System.Windows.Forms.Panel pnlConnectionRight;
		private System.Windows.Forms.Panel pnlConnectionTop;
		private System.Windows.Forms.Panel pnlConnectionLeft;
		private System.Windows.Forms.Panel pnlConnectionBottom;
		private System.Windows.Forms.Label lblConnection;
		private System.Windows.Forms.Panel pnlLeftMain;
		private UserControl.UCVehicleOverview ucVehicleOverview1;
		private UserControl.UCVehicleManualControl ucVehicleManualControl1;
		private UserControl.UCAbout ucAbout1;
		private UserControl.UCMap ucMap1;
		private UserControl.UCLog ucLog1;
		private UserControl.UCSetting ucSetting1;
		private UserControl.UCMission ucMission1;
		private UserControl.UCVehicle ucVehicle1;
		private System.Windows.Forms.Panel pnlBtm;
		private UserControl.UCSimpleLog ucSimpleLog1;
		private System.Windows.Forms.Panel pnlDisplayPnlBtm;
		private System.Windows.Forms.Button btnDisplayPnlBtm;
	}
}