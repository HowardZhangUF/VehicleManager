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
			this.lblFormTitle = new System.Windows.Forms.Label();
			this.lblFormIcon = new System.Windows.Forms.Label();
			this.btnFormMinimize = new System.Windows.Forms.Button();
			this.btnFormClose = new System.Windows.Forms.Button();
			this.btnDisplayAbout = new System.Windows.Forms.Button();
			this.pnlBtmSide = new System.Windows.Forms.Panel();
			this.pnlLeftSide = new System.Windows.Forms.Panel();
			this.pnlLeftSideMarker = new System.Windows.Forms.Panel();
			this.btnDisplayManualControl = new System.Windows.Forms.Button();
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
			this.pnlAbout = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.pnlVehicleManualControl = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.pnlVehicleOverview = new System.Windows.Forms.Panel();
			this.pnlMap = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.pnlVehicle = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.pnlMission = new System.Windows.Forms.Panel();
			this.label6 = new System.Windows.Forms.Label();
			this.pnlSetting = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.pnlLog = new System.Windows.Forms.Panel();
			this.label8 = new System.Windows.Forms.Label();
			this.pnlLeftMainTop = new System.Windows.Forms.Panel();
			this.pnlLeftMainBottom = new System.Windows.Forms.Panel();
			this.pnlLeftMainLeft = new System.Windows.Forms.Panel();
			this.pnlLeftMainRight = new System.Windows.Forms.Panel();
			this.pnlRightMainTop = new System.Windows.Forms.Panel();
			this.pnlRightMainBottom = new System.Windows.Forms.Panel();
			this.pnlRightMainLeft = new System.Windows.Forms.Panel();
			this.pnlRightMainRight = new System.Windows.Forms.Panel();
			this.pnlConnection = new System.Windows.Forms.Panel();
			this.pnlConnectionTop = new System.Windows.Forms.Panel();
			this.pnlConnectionBottom = new System.Windows.Forms.Panel();
			this.pnlConnectionLeft = new System.Windows.Forms.Panel();
			this.pnlConnectionRight = new System.Windows.Forms.Panel();
			this.lblConnection = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.pnlTopSide.SuspendLayout();
			this.pnlBtmSide.SuspendLayout();
			this.pnlLeftSide.SuspendLayout();
			this.pnlTop.SuspendLayout();
			this.pnlRightMain.SuspendLayout();
			this.pnlLeftMain.SuspendLayout();
			this.pnlAbout.SuspendLayout();
			this.pnlVehicleManualControl.SuspendLayout();
			this.pnlMap.SuspendLayout();
			this.pnlVehicle.SuspendLayout();
			this.pnlMission.SuspendLayout();
			this.pnlSetting.SuspendLayout();
			this.pnlLog.SuspendLayout();
			this.pnlConnection.SuspendLayout();
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
			this.gluiCtrl1.Size = new System.Drawing.Size(846, 596);
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
			this.tableLayoutPanel1.Location = new System.Drawing.Point(149, 205);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 7;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(244, 439);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(238, 70);
			this.label1.TabIndex = 0;
			this.label1.Text = "Intervene Command";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtInterveneMovingBuffer
			// 
			this.txtInterveneMovingBuffer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtInterveneMovingBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtInterveneMovingBuffer.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtInterveneMovingBuffer.Location = new System.Drawing.Point(3, 115);
			this.txtInterveneMovingBuffer.Name = "txtInterveneMovingBuffer";
			this.txtInterveneMovingBuffer.Size = new System.Drawing.Size(238, 40);
			this.txtInterveneMovingBuffer.TabIndex = 1;
			this.txtInterveneMovingBuffer.Text = "-9000,7000";
			// 
			// btnInterveneInsertMovingBuffer
			// 
			this.btnInterveneInsertMovingBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnInterveneInsertMovingBuffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnInterveneInsertMovingBuffer.Location = new System.Drawing.Point(3, 160);
			this.btnInterveneInsertMovingBuffer.Name = "btnInterveneInsertMovingBuffer";
			this.btnInterveneInsertMovingBuffer.Size = new System.Drawing.Size(238, 64);
			this.btnInterveneInsertMovingBuffer.TabIndex = 2;
			this.btnInterveneInsertMovingBuffer.Text = "Insert Moving Buffer";
			this.btnInterveneInsertMovingBuffer.UseVisualStyleBackColor = true;
			this.btnInterveneInsertMovingBuffer.Click += new System.EventHandler(this.btnInterveneInsertMovingBuffer_Click);
			// 
			// btnInterveneRemoveMovingBuffer
			// 
			this.btnInterveneRemoveMovingBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnInterveneRemoveMovingBuffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnInterveneRemoveMovingBuffer.Location = new System.Drawing.Point(3, 230);
			this.btnInterveneRemoveMovingBuffer.Name = "btnInterveneRemoveMovingBuffer";
			this.btnInterveneRemoveMovingBuffer.Size = new System.Drawing.Size(238, 64);
			this.btnInterveneRemoveMovingBuffer.TabIndex = 2;
			this.btnInterveneRemoveMovingBuffer.Text = "Remove Moving Buffer";
			this.btnInterveneRemoveMovingBuffer.UseVisualStyleBackColor = true;
			this.btnInterveneRemoveMovingBuffer.Click += new System.EventHandler(this.btnInterveneRemoveMovingBuffer_Click);
			// 
			// btnIntervenePauseMoving
			// 
			this.btnIntervenePauseMoving.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnIntervenePauseMoving.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnIntervenePauseMoving.Location = new System.Drawing.Point(3, 300);
			this.btnIntervenePauseMoving.Name = "btnIntervenePauseMoving";
			this.btnIntervenePauseMoving.Size = new System.Drawing.Size(238, 64);
			this.btnIntervenePauseMoving.TabIndex = 2;
			this.btnIntervenePauseMoving.Text = "Pause Moving";
			this.btnIntervenePauseMoving.UseVisualStyleBackColor = true;
			this.btnIntervenePauseMoving.Click += new System.EventHandler(this.btnIntervenePauseMoving_Click);
			// 
			// btnInterveneResumeMoving
			// 
			this.btnInterveneResumeMoving.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnInterveneResumeMoving.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnInterveneResumeMoving.Location = new System.Drawing.Point(3, 370);
			this.btnInterveneResumeMoving.Name = "btnInterveneResumeMoving";
			this.btnInterveneResumeMoving.Size = new System.Drawing.Size(238, 66);
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
			this.cbVehicleList.Location = new System.Drawing.Point(3, 73);
			this.cbVehicleList.Name = "cbVehicleList";
			this.cbVehicleList.Size = new System.Drawing.Size(238, 35);
			this.cbVehicleList.TabIndex = 3;
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
			// pnlLeftSide
			// 
			this.pnlLeftSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.pnlLeftSide.Controls.Add(this.pnlLeftSideMarker);
			this.pnlLeftSide.Controls.Add(this.btnDisplayAbout);
			this.pnlLeftSide.Controls.Add(this.btnDisplayManualControl);
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
			// btnDisplayManualControl
			// 
			this.btnDisplayManualControl.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnDisplayManualControl.FlatAppearance.BorderSize = 0;
			this.btnDisplayManualControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDisplayManualControl.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplayManualControl.Image")));
			this.btnDisplayManualControl.Location = new System.Drawing.Point(0, 100);
			this.btnDisplayManualControl.Name = "btnDisplayManualControl";
			this.btnDisplayManualControl.Size = new System.Drawing.Size(50, 50);
			this.btnDisplayManualControl.TabIndex = 1;
			this.btnDisplayManualControl.UseVisualStyleBackColor = true;
			this.btnDisplayManualControl.Click += new System.EventHandler(this.btnDisplayManualControl_Click);
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
			this.pnlRightMain.Controls.Add(this.pnlLog);
			this.pnlRightMain.Controls.Add(this.pnlSetting);
			this.pnlRightMain.Controls.Add(this.pnlMission);
			this.pnlRightMain.Controls.Add(this.pnlVehicle);
			this.pnlRightMain.Controls.Add(this.pnlMap);
			this.pnlRightMain.Controls.Add(this.pnlRightMainRight);
			this.pnlRightMain.Controls.Add(this.pnlRightMainLeft);
			this.pnlRightMain.Controls.Add(this.pnlRightMainBottom);
			this.pnlRightMain.Controls.Add(this.pnlRightMainTop);
			this.pnlRightMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlRightMain.Location = new System.Drawing.Point(450, 100);
			this.pnlRightMain.Name = "pnlRightMain";
			this.pnlRightMain.Size = new System.Drawing.Size(850, 600);
			this.pnlRightMain.TabIndex = 8;
			// 
			// pnlLeftMain
			// 
			this.pnlLeftMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.pnlLeftMain.Controls.Add(this.pnlAbout);
			this.pnlLeftMain.Controls.Add(this.pnlVehicleManualControl);
			this.pnlLeftMain.Controls.Add(this.pnlVehicleOverview);
			this.pnlLeftMain.Controls.Add(this.pnlLeftMainRight);
			this.pnlLeftMain.Controls.Add(this.pnlLeftMainLeft);
			this.pnlLeftMain.Controls.Add(this.pnlLeftMainBottom);
			this.pnlLeftMain.Controls.Add(this.pnlLeftMainTop);
			this.pnlLeftMain.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlLeftMain.Location = new System.Drawing.Point(50, 50);
			this.pnlLeftMain.Name = "pnlLeftMain";
			this.pnlLeftMain.Size = new System.Drawing.Size(400, 650);
			this.pnlLeftMain.TabIndex = 6;
			// 
			// pnlAbout
			// 
			this.pnlAbout.Controls.Add(this.label3);
			this.pnlAbout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlAbout.Location = new System.Drawing.Point(2, 2);
			this.pnlAbout.Name = "pnlAbout";
			this.pnlAbout.Size = new System.Drawing.Size(396, 646);
			this.pnlAbout.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(30, 30);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(42, 15);
			this.label3.TabIndex = 0;
			this.label3.Text = "About";
			// 
			// pnlVehicleManualControl
			// 
			this.pnlVehicleManualControl.Controls.Add(this.label2);
			this.pnlVehicleManualControl.Controls.Add(this.tableLayoutPanel1);
			this.pnlVehicleManualControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlVehicleManualControl.Location = new System.Drawing.Point(2, 2);
			this.pnlVehicleManualControl.Name = "pnlVehicleManualControl";
			this.pnlVehicleManualControl.Size = new System.Drawing.Size(396, 646);
			this.pnlVehicleManualControl.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(30, 30);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(97, 15);
			this.label2.TabIndex = 0;
			this.label2.Text = "Vehicle Control";
			// 
			// pnlVehicleOverview
			// 
			this.pnlVehicleOverview.AutoScroll = true;
			this.pnlVehicleOverview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlVehicleOverview.Location = new System.Drawing.Point(2, 2);
			this.pnlVehicleOverview.Name = "pnlVehicleOverview";
			this.pnlVehicleOverview.Size = new System.Drawing.Size(396, 646);
			this.pnlVehicleOverview.TabIndex = 0;
			// 
			// pnlMap
			// 
			this.pnlMap.Controls.Add(this.gluiCtrl1);
			this.pnlMap.Controls.Add(this.label4);
			this.pnlMap.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMap.Location = new System.Drawing.Point(2, 2);
			this.pnlMap.Name = "pnlMap";
			this.pnlMap.Size = new System.Drawing.Size(846, 596);
			this.pnlMap.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(30, 30);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(33, 15);
			this.label4.TabIndex = 0;
			this.label4.Text = "Map";
			// 
			// pnlVehicle
			// 
			this.pnlVehicle.Controls.Add(this.label5);
			this.pnlVehicle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlVehicle.Location = new System.Drawing.Point(2, 2);
			this.pnlVehicle.Name = "pnlVehicle";
			this.pnlVehicle.Size = new System.Drawing.Size(846, 596);
			this.pnlVehicle.TabIndex = 1;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(30, 30);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(50, 15);
			this.label5.TabIndex = 0;
			this.label5.Text = "Vehicle";
			// 
			// pnlMission
			// 
			this.pnlMission.Controls.Add(this.label6);
			this.pnlMission.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMission.Location = new System.Drawing.Point(2, 2);
			this.pnlMission.Name = "pnlMission";
			this.pnlMission.Size = new System.Drawing.Size(846, 596);
			this.pnlMission.TabIndex = 1;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(30, 30);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(52, 15);
			this.label6.TabIndex = 0;
			this.label6.Text = "Mission";
			// 
			// pnlSetting
			// 
			this.pnlSetting.Controls.Add(this.label7);
			this.pnlSetting.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSetting.Location = new System.Drawing.Point(2, 2);
			this.pnlSetting.Name = "pnlSetting";
			this.pnlSetting.Size = new System.Drawing.Size(846, 596);
			this.pnlSetting.TabIndex = 1;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(30, 30);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(47, 15);
			this.label7.TabIndex = 0;
			this.label7.Text = "Setting";
			// 
			// pnlLog
			// 
			this.pnlLog.Controls.Add(this.label8);
			this.pnlLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlLog.Location = new System.Drawing.Point(2, 2);
			this.pnlLog.Name = "pnlLog";
			this.pnlLog.Size = new System.Drawing.Size(846, 596);
			this.pnlLog.TabIndex = 1;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(30, 30);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(30, 15);
			this.label8.TabIndex = 0;
			this.label8.Text = "Log";
			// 
			// pnlLeftMainTop
			// 
			this.pnlLeftMainTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlLeftMainTop.Location = new System.Drawing.Point(0, 0);
			this.pnlLeftMainTop.Name = "pnlLeftMainTop";
			this.pnlLeftMainTop.Size = new System.Drawing.Size(400, 2);
			this.pnlLeftMainTop.TabIndex = 0;
			// 
			// pnlLeftMainBottom
			// 
			this.pnlLeftMainBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlLeftMainBottom.Location = new System.Drawing.Point(0, 648);
			this.pnlLeftMainBottom.Name = "pnlLeftMainBottom";
			this.pnlLeftMainBottom.Size = new System.Drawing.Size(400, 2);
			this.pnlLeftMainBottom.TabIndex = 0;
			// 
			// pnlLeftMainLeft
			// 
			this.pnlLeftMainLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlLeftMainLeft.Location = new System.Drawing.Point(0, 2);
			this.pnlLeftMainLeft.Name = "pnlLeftMainLeft";
			this.pnlLeftMainLeft.Size = new System.Drawing.Size(2, 646);
			this.pnlLeftMainLeft.TabIndex = 0;
			// 
			// pnlLeftMainRight
			// 
			this.pnlLeftMainRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlLeftMainRight.Location = new System.Drawing.Point(398, 2);
			this.pnlLeftMainRight.Name = "pnlLeftMainRight";
			this.pnlLeftMainRight.Size = new System.Drawing.Size(2, 646);
			this.pnlLeftMainRight.TabIndex = 0;
			// 
			// pnlRightMainTop
			// 
			this.pnlRightMainTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlRightMainTop.Location = new System.Drawing.Point(0, 0);
			this.pnlRightMainTop.Name = "pnlRightMainTop";
			this.pnlRightMainTop.Size = new System.Drawing.Size(850, 2);
			this.pnlRightMainTop.TabIndex = 2;
			// 
			// pnlRightMainBottom
			// 
			this.pnlRightMainBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlRightMainBottom.Location = new System.Drawing.Point(0, 598);
			this.pnlRightMainBottom.Name = "pnlRightMainBottom";
			this.pnlRightMainBottom.Size = new System.Drawing.Size(850, 2);
			this.pnlRightMainBottom.TabIndex = 3;
			// 
			// pnlRightMainLeft
			// 
			this.pnlRightMainLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlRightMainLeft.Location = new System.Drawing.Point(0, 2);
			this.pnlRightMainLeft.Name = "pnlRightMainLeft";
			this.pnlRightMainLeft.Size = new System.Drawing.Size(2, 596);
			this.pnlRightMainLeft.TabIndex = 3;
			// 
			// pnlRightMainRight
			// 
			this.pnlRightMainRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlRightMainRight.Location = new System.Drawing.Point(848, 2);
			this.pnlRightMainRight.Name = "pnlRightMainRight";
			this.pnlRightMainRight.Size = new System.Drawing.Size(2, 596);
			this.pnlRightMainRight.TabIndex = 3;
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
			// pnlConnectionTop
			// 
			this.pnlConnectionTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlConnectionTop.Location = new System.Drawing.Point(0, 0);
			this.pnlConnectionTop.Name = "pnlConnectionTop";
			this.pnlConnectionTop.Size = new System.Drawing.Size(50, 5);
			this.pnlConnectionTop.TabIndex = 1;
			// 
			// pnlConnectionBottom
			// 
			this.pnlConnectionBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlConnectionBottom.Location = new System.Drawing.Point(0, 45);
			this.pnlConnectionBottom.Name = "pnlConnectionBottom";
			this.pnlConnectionBottom.Size = new System.Drawing.Size(50, 5);
			this.pnlConnectionBottom.TabIndex = 1;
			// 
			// pnlConnectionLeft
			// 
			this.pnlConnectionLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlConnectionLeft.Location = new System.Drawing.Point(0, 5);
			this.pnlConnectionLeft.Name = "pnlConnectionLeft";
			this.pnlConnectionLeft.Size = new System.Drawing.Size(5, 40);
			this.pnlConnectionLeft.TabIndex = 1;
			// 
			// pnlConnectionRight
			// 
			this.pnlConnectionRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlConnectionRight.Location = new System.Drawing.Point(45, 5);
			this.pnlConnectionRight.Name = "pnlConnectionRight";
			this.pnlConnectionRight.Size = new System.Drawing.Size(5, 40);
			this.pnlConnectionRight.TabIndex = 1;
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
			this.pnlBtmSide.ResumeLayout(false);
			this.pnlLeftSide.ResumeLayout(false);
			this.pnlTop.ResumeLayout(false);
			this.pnlRightMain.ResumeLayout(false);
			this.pnlLeftMain.ResumeLayout(false);
			this.pnlAbout.ResumeLayout(false);
			this.pnlAbout.PerformLayout();
			this.pnlVehicleManualControl.ResumeLayout(false);
			this.pnlVehicleManualControl.PerformLayout();
			this.pnlMap.ResumeLayout(false);
			this.pnlMap.PerformLayout();
			this.pnlVehicle.ResumeLayout(false);
			this.pnlVehicle.PerformLayout();
			this.pnlMission.ResumeLayout(false);
			this.pnlMission.PerformLayout();
			this.pnlSetting.ResumeLayout(false);
			this.pnlSetting.PerformLayout();
			this.pnlLog.ResumeLayout(false);
			this.pnlLog.PerformLayout();
			this.pnlConnection.ResumeLayout(false);
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
		private System.Windows.Forms.Button btnDisplayManualControl;
		private System.Windows.Forms.Button btnDisplayVehicleOverview;
		private System.Windows.Forms.Panel pnlLeftMain;
		private System.Windows.Forms.Panel pnlVehicleOverview;
		private System.Windows.Forms.Panel pnlVehicleManualControl;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel pnlAbout;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel pnlLog;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Panel pnlSetting;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Panel pnlMission;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel pnlVehicle;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Panel pnlMap;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel pnlLeftMainTop;
		private System.Windows.Forms.Panel pnlLeftMainRight;
		private System.Windows.Forms.Panel pnlLeftMainLeft;
		private System.Windows.Forms.Panel pnlLeftMainBottom;
		private System.Windows.Forms.Panel pnlRightMainRight;
		private System.Windows.Forms.Panel pnlRightMainLeft;
		private System.Windows.Forms.Panel pnlRightMainBottom;
		private System.Windows.Forms.Panel pnlRightMainTop;
		private System.Windows.Forms.Panel pnlConnection;
		private System.Windows.Forms.Panel pnlConnectionRight;
		private System.Windows.Forms.Panel pnlConnectionTop;
		private System.Windows.Forms.Panel pnlConnectionLeft;
		private System.Windows.Forms.Panel pnlConnectionBottom;
		private System.Windows.Forms.Label lblConnection;
	}
}