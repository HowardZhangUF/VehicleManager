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
			this.gluiCtrl1 = new GLUI.GLUICtrl();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.txtInterveneMovingBuffer = new System.Windows.Forms.TextBox();
			this.btnInterveneInsertMovingBuffer = new System.Windows.Forms.Button();
			this.btnInterveneRemoveMovingBuffer = new System.Windows.Forms.Button();
			this.btnIntervenePauseMoving = new System.Windows.Forms.Button();
			this.btnInterveneResumeMoving = new System.Windows.Forms.Button();
			this.cbVehicleList = new System.Windows.Forms.ComboBox();
			this.pnlTopSide = new System.Windows.Forms.Panel();
			this.btnAbout = new System.Windows.Forms.Button();
			this.lblFormTitle = new System.Windows.Forms.Label();
			this.lblFormIcon = new System.Windows.Forms.Label();
			this.btnFormMinimize = new System.Windows.Forms.Button();
			this.btnFormClose = new System.Windows.Forms.Button();
			this.pnlBtmSide = new System.Windows.Forms.Panel();
			this.pnlLeftSide = new System.Windows.Forms.Panel();
			this.btnDisplayManualControl = new System.Windows.Forms.Button();
			this.btnDisplayPnlLeftMain = new System.Windows.Forms.Button();
			this.btnDisplaySetting = new System.Windows.Forms.Button();
			this.pnlLeftMain = new System.Windows.Forms.Panel();
			this.pnlLeftSideMarker = new System.Windows.Forms.Panel();
			this.pnlTop = new System.Windows.Forms.Panel();
			this.pnlTopMarker = new System.Windows.Forms.Panel();
			this.btnDisplayLog = new System.Windows.Forms.Button();
			this.btnDisplayMission = new System.Windows.Forms.Button();
			this.btnDisplayVehicle = new System.Windows.Forms.Button();
			this.btnDisplayMap = new System.Windows.Forms.Button();
			this.pnlRightMain = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1.SuspendLayout();
			this.pnlTopSide.SuspendLayout();
			this.pnlLeftSide.SuspendLayout();
			this.pnlLeftMain.SuspendLayout();
			this.pnlTop.SuspendLayout();
			this.pnlRightMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// gluiCtrl1
			// 
			this.gluiCtrl1.AllowObjectMenu = true;
			this.gluiCtrl1.AllowUndoMenu = true;
			this.gluiCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gluiCtrl1.Location = new System.Drawing.Point(0, 0);
			this.gluiCtrl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.gluiCtrl1.Name = "gluiCtrl1";
			this.gluiCtrl1.ShowAxis = true;
			this.gluiCtrl1.ShowGrid = true;
			this.gluiCtrl1.Size = new System.Drawing.Size(606, 600);
			this.gluiCtrl1.TabIndex = 1;
			this.gluiCtrl1.Zoom = 10D;
			this.gluiCtrl1.LoadMapEvent += new GLUI.LoadMapEvent(this.gluiCtrl1_LoadMapEvent);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtInterveneMovingBuffer, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnInterveneInsertMovingBuffer, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.btnInterveneRemoveMovingBuffer, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.btnIntervenePauseMoving, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.btnInterveneResumeMoving, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.cbVehicleList, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(606, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 7;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(244, 600);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(238, 102);
			this.label1.TabIndex = 0;
			this.label1.Text = "Intervene Command";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtInterveneMovingBuffer
			// 
			this.txtInterveneMovingBuffer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtInterveneMovingBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtInterveneMovingBuffer.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtInterveneMovingBuffer.Location = new System.Drawing.Point(3, 147);
			this.txtInterveneMovingBuffer.Name = "txtInterveneMovingBuffer";
			this.txtInterveneMovingBuffer.Size = new System.Drawing.Size(238, 40);
			this.txtInterveneMovingBuffer.TabIndex = 1;
			this.txtInterveneMovingBuffer.Text = "-9000,7000";
			// 
			// btnInterveneInsertMovingBuffer
			// 
			this.btnInterveneInsertMovingBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnInterveneInsertMovingBuffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnInterveneInsertMovingBuffer.Location = new System.Drawing.Point(3, 192);
			this.btnInterveneInsertMovingBuffer.Name = "btnInterveneInsertMovingBuffer";
			this.btnInterveneInsertMovingBuffer.Size = new System.Drawing.Size(238, 96);
			this.btnInterveneInsertMovingBuffer.TabIndex = 2;
			this.btnInterveneInsertMovingBuffer.Text = "Insert Moving Buffer";
			this.btnInterveneInsertMovingBuffer.UseVisualStyleBackColor = true;
			this.btnInterveneInsertMovingBuffer.Click += new System.EventHandler(this.btnInterveneInsertMovingBuffer_Click);
			// 
			// btnInterveneRemoveMovingBuffer
			// 
			this.btnInterveneRemoveMovingBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnInterveneRemoveMovingBuffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnInterveneRemoveMovingBuffer.Location = new System.Drawing.Point(3, 294);
			this.btnInterveneRemoveMovingBuffer.Name = "btnInterveneRemoveMovingBuffer";
			this.btnInterveneRemoveMovingBuffer.Size = new System.Drawing.Size(238, 96);
			this.btnInterveneRemoveMovingBuffer.TabIndex = 2;
			this.btnInterveneRemoveMovingBuffer.Text = "Remove Moving Buffer";
			this.btnInterveneRemoveMovingBuffer.UseVisualStyleBackColor = true;
			this.btnInterveneRemoveMovingBuffer.Click += new System.EventHandler(this.btnInterveneRemoveMovingBuffer_Click);
			// 
			// btnIntervenePauseMoving
			// 
			this.btnIntervenePauseMoving.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnIntervenePauseMoving.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnIntervenePauseMoving.Location = new System.Drawing.Point(3, 396);
			this.btnIntervenePauseMoving.Name = "btnIntervenePauseMoving";
			this.btnIntervenePauseMoving.Size = new System.Drawing.Size(238, 96);
			this.btnIntervenePauseMoving.TabIndex = 2;
			this.btnIntervenePauseMoving.Text = "Pause Moving";
			this.btnIntervenePauseMoving.UseVisualStyleBackColor = true;
			this.btnIntervenePauseMoving.Click += new System.EventHandler(this.btnIntervenePauseMoving_Click);
			// 
			// btnInterveneResumeMoving
			// 
			this.btnInterveneResumeMoving.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnInterveneResumeMoving.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnInterveneResumeMoving.Location = new System.Drawing.Point(3, 498);
			this.btnInterveneResumeMoving.Name = "btnInterveneResumeMoving";
			this.btnInterveneResumeMoving.Size = new System.Drawing.Size(238, 99);
			this.btnInterveneResumeMoving.TabIndex = 2;
			this.btnInterveneResumeMoving.Text = "Resume Moving";
			this.btnInterveneResumeMoving.UseVisualStyleBackColor = true;
			this.btnInterveneResumeMoving.Click += new System.EventHandler(this.btnInterveneResumeMoving_Click);
			// 
			// cbVehicleList
			// 
			this.cbVehicleList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.cbVehicleList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbVehicleList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVehicleList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbVehicleList.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbVehicleList.ForeColor = System.Drawing.Color.White;
			this.cbVehicleList.FormattingEnabled = true;
			this.cbVehicleList.Location = new System.Drawing.Point(3, 105);
			this.cbVehicleList.Name = "cbVehicleList";
			this.cbVehicleList.Size = new System.Drawing.Size(238, 35);
			this.cbVehicleList.TabIndex = 3;
			// 
			// pnlTopSide
			// 
			this.pnlTopSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
			this.pnlTopSide.Controls.Add(this.btnAbout);
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
			// btnAbout
			// 
			this.btnAbout.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnAbout.FlatAppearance.BorderSize = 0;
			this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAbout.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnAbout.Image = ((System.Drawing.Image)(resources.GetObject("btnAbout.Image")));
			this.btnAbout.Location = new System.Drawing.Point(1060, 0);
			this.btnAbout.Name = "btnAbout";
			this.btnAbout.Size = new System.Drawing.Size(140, 50);
			this.btnAbout.TabIndex = 7;
			this.btnAbout.Text = "  About";
			this.btnAbout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnAbout.UseVisualStyleBackColor = true;
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
			// pnlBtmSide
			// 
			this.pnlBtmSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
			this.pnlBtmSide.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBtmSide.Location = new System.Drawing.Point(0, 700);
			this.pnlBtmSide.Name = "pnlBtmSide";
			this.pnlBtmSide.Size = new System.Drawing.Size(1300, 50);
			this.pnlBtmSide.TabIndex = 4;
			// 
			// pnlLeftSide
			// 
			this.pnlLeftSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.pnlLeftSide.Controls.Add(this.btnDisplayManualControl);
			this.pnlLeftSide.Controls.Add(this.btnDisplayPnlLeftMain);
			this.pnlLeftSide.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlLeftSide.Location = new System.Drawing.Point(0, 50);
			this.pnlLeftSide.Name = "pnlLeftSide";
			this.pnlLeftSide.Size = new System.Drawing.Size(50, 650);
			this.pnlLeftSide.TabIndex = 5;
			// 
			// btnDisplayManualControl
			// 
			this.btnDisplayManualControl.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnDisplayManualControl.FlatAppearance.BorderSize = 0;
			this.btnDisplayManualControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDisplayManualControl.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplayManualControl.Image")));
			this.btnDisplayManualControl.Location = new System.Drawing.Point(0, 50);
			this.btnDisplayManualControl.Name = "btnDisplayManualControl";
			this.btnDisplayManualControl.Size = new System.Drawing.Size(50, 50);
			this.btnDisplayManualControl.TabIndex = 1;
			this.btnDisplayManualControl.UseVisualStyleBackColor = true;
			this.btnDisplayManualControl.Click += new System.EventHandler(this.btnDisplayManualControl_Click);
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
			// pnlLeftMain
			// 
			this.pnlLeftMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.pnlLeftMain.Controls.Add(this.pnlLeftSideMarker);
			this.pnlLeftMain.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlLeftMain.Location = new System.Drawing.Point(50, 50);
			this.pnlLeftMain.Name = "pnlLeftMain";
			this.pnlLeftMain.Size = new System.Drawing.Size(400, 650);
			this.pnlLeftMain.TabIndex = 6;
			// 
			// pnlLeftSideMarker
			// 
			this.pnlLeftSideMarker.BackColor = System.Drawing.Color.Aqua;
			this.pnlLeftSideMarker.Location = new System.Drawing.Point(0, 0);
			this.pnlLeftSideMarker.Name = "pnlLeftSideMarker";
			this.pnlLeftSideMarker.Size = new System.Drawing.Size(4, 650);
			this.pnlLeftSideMarker.TabIndex = 4;
			// 
			// pnlTop
			// 
			this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
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
			this.pnlRightMain.Controls.Add(this.gluiCtrl1);
			this.pnlRightMain.Controls.Add(this.tableLayoutPanel1);
			this.pnlRightMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlRightMain.Location = new System.Drawing.Point(450, 100);
			this.pnlRightMain.Name = "pnlRightMain";
			this.pnlRightMain.Size = new System.Drawing.Size(850, 600);
			this.pnlRightMain.TabIndex = 8;
			// 
			// VehicleManagerGUI
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.ClientSize = new System.Drawing.Size(1300, 750);
			this.Controls.Add(this.pnlRightMain);
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
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.pnlTopSide.ResumeLayout(false);
			this.pnlLeftSide.ResumeLayout(false);
			this.pnlLeftMain.ResumeLayout(false);
			this.pnlTop.ResumeLayout(false);
			this.pnlRightMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private GLUI.GLUICtrl gluiCtrl1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtInterveneMovingBuffer;
		private System.Windows.Forms.Button btnInterveneInsertMovingBuffer;
		private System.Windows.Forms.Button btnInterveneRemoveMovingBuffer;
		private System.Windows.Forms.Button btnIntervenePauseMoving;
		private System.Windows.Forms.Button btnInterveneResumeMoving;
		private System.Windows.Forms.ComboBox cbVehicleList;
		private System.Windows.Forms.Panel pnlTopSide;
		private System.Windows.Forms.Panel pnlBtmSide;
		private System.Windows.Forms.Panel pnlLeftSide;
		private System.Windows.Forms.Panel pnlLeftMain;
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
		private System.Windows.Forms.Button btnAbout;
		private System.Windows.Forms.Button btnDisplayVehicle;
		private System.Windows.Forms.Button btnDisplayManualControl;
	}
}