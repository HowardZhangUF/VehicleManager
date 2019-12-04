namespace TrafficControlTest.UserControl
{
	partial class UcVehicleApi
	{
		/// <summary> 
		/// 設計工具所需的變數。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清除任何使用中的資源。
		/// </summary>
		/// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 元件設計工具產生的程式碼

		/// <summary> 
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.cbLocalMapNameList = new System.Windows.Forms.ComboBox();
			this.btnVehicleGoto = new System.Windows.Forms.Button();
			this.btnVehicleStop = new System.Windows.Forms.Button();
			this.btnVehicleGotoPoint = new System.Windows.Forms.Button();
			this.btnVehicleDock = new System.Windows.Forms.Button();
			this.cbGoalNameList = new System.Windows.Forms.ComboBox();
			this.btnVehicleInsertMovingBuffer = new System.Windows.Forms.Button();
			this.btnVehicleRemoveMovingBuffer = new System.Windows.Forms.Button();
			this.btnVehiclePause = new System.Windows.Forms.Button();
			this.btnVehicleResume = new System.Windows.Forms.Button();
			this.btnVehicleRequestMapList = new System.Windows.Forms.Button();
			this.btnVehicleGetMap = new System.Windows.Forms.Button();
			this.btnVehicleChangeMap = new System.Windows.Forms.Button();
			this.btnVehicleUploadMap = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbRemoteMapNameList1 = new System.Windows.Forms.ComboBox();
			this.cbRemoteMapNameList2 = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.cbVehicleNameList = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.txtCoordinate1 = new TrafficControlTest.UserControl.TextBoxWithHint();
			this.txtCoordinate2 = new TrafficControlTest.UserControl.TextBoxWithHint();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(400, 60);
			this.label1.TabIndex = 0;
			this.label1.Text = "    Vehicle API";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.cbLocalMapNameList, 2, 16);
			this.tableLayoutPanel1.Controls.Add(this.btnVehicleGoto, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnVehicleStop, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.btnVehicleGotoPoint, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.btnVehicleDock, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.cbGoalNameList, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnVehicleInsertMovingBuffer, 1, 8);
			this.tableLayoutPanel1.Controls.Add(this.btnVehicleRemoveMovingBuffer, 1, 9);
			this.tableLayoutPanel1.Controls.Add(this.btnVehiclePause, 1, 10);
			this.tableLayoutPanel1.Controls.Add(this.btnVehicleResume, 1, 11);
			this.tableLayoutPanel1.Controls.Add(this.btnVehicleRequestMapList, 1, 14);
			this.tableLayoutPanel1.Controls.Add(this.btnVehicleGetMap, 1, 15);
			this.tableLayoutPanel1.Controls.Add(this.btnVehicleChangeMap, 1, 17);
			this.tableLayoutPanel1.Controls.Add(this.btnVehicleUploadMap, 1, 16);
			this.tableLayoutPanel1.Controls.Add(this.label2, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this.label4, 1, 13);
			this.tableLayoutPanel1.Controls.Add(this.cbRemoteMapNameList1, 2, 15);
			this.tableLayoutPanel1.Controls.Add(this.cbRemoteMapNameList2, 2, 17);
			this.tableLayoutPanel1.Controls.Add(this.txtCoordinate1, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtCoordinate2, 3, 8);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 19;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(379, 650);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// cbLocalMapNameList
			// 
			this.cbLocalMapNameList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbLocalMapNameList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.tableLayoutPanel1.SetColumnSpan(this.cbLocalMapNameList, 2);
			this.cbLocalMapNameList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLocalMapNameList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbLocalMapNameList.Font = new System.Drawing.Font("新細明體", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbLocalMapNameList.ForeColor = System.Drawing.Color.White;
			this.cbLocalMapNameList.FormattingEnabled = true;
			this.cbLocalMapNameList.Location = new System.Drawing.Point(133, 508);
			this.cbLocalMapNameList.Name = "cbLocalMapNameList";
			this.cbLocalMapNameList.Size = new System.Drawing.Size(223, 41);
			this.cbLocalMapNameList.TabIndex = 22;
			// 
			// btnVehicleGoto
			// 
			this.btnVehicleGoto.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehicleGoto.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
			this.btnVehicleGoto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVehicleGoto.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnVehicleGoto.ForeColor = System.Drawing.Color.White;
			this.btnVehicleGoto.Location = new System.Drawing.Point(23, 38);
			this.btnVehicleGoto.Name = "btnVehicleGoto";
			this.btnVehicleGoto.Size = new System.Drawing.Size(104, 34);
			this.btnVehicleGoto.TabIndex = 0;
			this.btnVehicleGoto.Text = "Goto";
			this.btnVehicleGoto.UseVisualStyleBackColor = true;
			this.btnVehicleGoto.Click += new System.EventHandler(this.btnVehicleGoto_Click);
			// 
			// btnVehicleStop
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.btnVehicleStop, 3);
			this.btnVehicleStop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehicleStop.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
			this.btnVehicleStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVehicleStop.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnVehicleStop.ForeColor = System.Drawing.Color.White;
			this.btnVehicleStop.Location = new System.Drawing.Point(23, 158);
			this.btnVehicleStop.Name = "btnVehicleStop";
			this.btnVehicleStop.Size = new System.Drawing.Size(333, 34);
			this.btnVehicleStop.TabIndex = 3;
			this.btnVehicleStop.Text = "Stop";
			this.btnVehicleStop.UseVisualStyleBackColor = true;
			this.btnVehicleStop.Click += new System.EventHandler(this.btnVehicleStop_Click);
			// 
			// btnVehicleGotoPoint
			// 
			this.btnVehicleGotoPoint.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehicleGotoPoint.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
			this.btnVehicleGotoPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVehicleGotoPoint.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnVehicleGotoPoint.ForeColor = System.Drawing.Color.White;
			this.btnVehicleGotoPoint.Location = new System.Drawing.Point(23, 78);
			this.btnVehicleGotoPoint.Name = "btnVehicleGotoPoint";
			this.btnVehicleGotoPoint.Size = new System.Drawing.Size(104, 34);
			this.btnVehicleGotoPoint.TabIndex = 1;
			this.btnVehicleGotoPoint.Text = "GotoPoint";
			this.btnVehicleGotoPoint.UseVisualStyleBackColor = true;
			this.btnVehicleGotoPoint.Click += new System.EventHandler(this.btnVehicleGotoPoint_Click);
			// 
			// btnVehicleDock
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.btnVehicleDock, 3);
			this.btnVehicleDock.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehicleDock.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
			this.btnVehicleDock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVehicleDock.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnVehicleDock.ForeColor = System.Drawing.Color.White;
			this.btnVehicleDock.Location = new System.Drawing.Point(23, 118);
			this.btnVehicleDock.Name = "btnVehicleDock";
			this.btnVehicleDock.Size = new System.Drawing.Size(333, 34);
			this.btnVehicleDock.TabIndex = 2;
			this.btnVehicleDock.Text = "Dock";
			this.btnVehicleDock.UseVisualStyleBackColor = true;
			this.btnVehicleDock.Click += new System.EventHandler(this.btnVehicleDock_Click);
			// 
			// cbGoalNameList
			// 
			this.cbGoalNameList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbGoalNameList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.tableLayoutPanel1.SetColumnSpan(this.cbGoalNameList, 2);
			this.cbGoalNameList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbGoalNameList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbGoalNameList.Font = new System.Drawing.Font("新細明體", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbGoalNameList.ForeColor = System.Drawing.Color.White;
			this.cbGoalNameList.FormattingEnabled = true;
			this.cbGoalNameList.Location = new System.Drawing.Point(133, 38);
			this.cbGoalNameList.Name = "cbGoalNameList";
			this.cbGoalNameList.Size = new System.Drawing.Size(223, 41);
			this.cbGoalNameList.TabIndex = 5;
			// 
			// btnVehicleInsertMovingBuffer
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.btnVehicleInsertMovingBuffer, 2);
			this.btnVehicleInsertMovingBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehicleInsertMovingBuffer.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
			this.btnVehicleInsertMovingBuffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVehicleInsertMovingBuffer.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnVehicleInsertMovingBuffer.ForeColor = System.Drawing.Color.White;
			this.btnVehicleInsertMovingBuffer.Location = new System.Drawing.Point(23, 233);
			this.btnVehicleInsertMovingBuffer.Name = "btnVehicleInsertMovingBuffer";
			this.btnVehicleInsertMovingBuffer.Size = new System.Drawing.Size(164, 34);
			this.btnVehicleInsertMovingBuffer.TabIndex = 6;
			this.btnVehicleInsertMovingBuffer.Text = "InsertMovinghBuffer";
			this.btnVehicleInsertMovingBuffer.UseVisualStyleBackColor = true;
			this.btnVehicleInsertMovingBuffer.Click += new System.EventHandler(this.btnVehicleInsertMovingBuffer_Click);
			// 
			// btnVehicleRemoveMovingBuffer
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.btnVehicleRemoveMovingBuffer, 3);
			this.btnVehicleRemoveMovingBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehicleRemoveMovingBuffer.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
			this.btnVehicleRemoveMovingBuffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVehicleRemoveMovingBuffer.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnVehicleRemoveMovingBuffer.ForeColor = System.Drawing.Color.White;
			this.btnVehicleRemoveMovingBuffer.Location = new System.Drawing.Point(23, 273);
			this.btnVehicleRemoveMovingBuffer.Name = "btnVehicleRemoveMovingBuffer";
			this.btnVehicleRemoveMovingBuffer.Size = new System.Drawing.Size(333, 34);
			this.btnVehicleRemoveMovingBuffer.TabIndex = 7;
			this.btnVehicleRemoveMovingBuffer.Text = "RemoveMovingBuffer";
			this.btnVehicleRemoveMovingBuffer.UseVisualStyleBackColor = true;
			this.btnVehicleRemoveMovingBuffer.Click += new System.EventHandler(this.btnVehicleRemoveMovingBuffer_Click);
			// 
			// btnVehiclePause
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.btnVehiclePause, 3);
			this.btnVehiclePause.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehiclePause.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
			this.btnVehiclePause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVehiclePause.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnVehiclePause.ForeColor = System.Drawing.Color.White;
			this.btnVehiclePause.Location = new System.Drawing.Point(23, 313);
			this.btnVehiclePause.Name = "btnVehiclePause";
			this.btnVehiclePause.Size = new System.Drawing.Size(333, 34);
			this.btnVehiclePause.TabIndex = 8;
			this.btnVehiclePause.Text = "Pause";
			this.btnVehiclePause.UseVisualStyleBackColor = true;
			this.btnVehiclePause.Click += new System.EventHandler(this.btnVehiclePause_Click);
			// 
			// btnVehicleResume
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.btnVehicleResume, 3);
			this.btnVehicleResume.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehicleResume.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
			this.btnVehicleResume.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVehicleResume.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnVehicleResume.ForeColor = System.Drawing.Color.White;
			this.btnVehicleResume.Location = new System.Drawing.Point(23, 353);
			this.btnVehicleResume.Name = "btnVehicleResume";
			this.btnVehicleResume.Size = new System.Drawing.Size(333, 34);
			this.btnVehicleResume.TabIndex = 9;
			this.btnVehicleResume.Text = "Resume";
			this.btnVehicleResume.UseVisualStyleBackColor = true;
			this.btnVehicleResume.Click += new System.EventHandler(this.btnVehicleResume_Click);
			// 
			// btnVehicleRequestMapList
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.btnVehicleRequestMapList, 3);
			this.btnVehicleRequestMapList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehicleRequestMapList.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
			this.btnVehicleRequestMapList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVehicleRequestMapList.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnVehicleRequestMapList.ForeColor = System.Drawing.Color.White;
			this.btnVehicleRequestMapList.Location = new System.Drawing.Point(23, 428);
			this.btnVehicleRequestMapList.Name = "btnVehicleRequestMapList";
			this.btnVehicleRequestMapList.Size = new System.Drawing.Size(333, 34);
			this.btnVehicleRequestMapList.TabIndex = 11;
			this.btnVehicleRequestMapList.Text = "RequestMapList";
			this.btnVehicleRequestMapList.UseVisualStyleBackColor = true;
			this.btnVehicleRequestMapList.Click += new System.EventHandler(this.btnVehicleRequestMapList_Click);
			// 
			// btnVehicleGetMap
			// 
			this.btnVehicleGetMap.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehicleGetMap.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
			this.btnVehicleGetMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVehicleGetMap.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnVehicleGetMap.ForeColor = System.Drawing.Color.White;
			this.btnVehicleGetMap.Location = new System.Drawing.Point(23, 468);
			this.btnVehicleGetMap.Name = "btnVehicleGetMap";
			this.btnVehicleGetMap.Size = new System.Drawing.Size(104, 34);
			this.btnVehicleGetMap.TabIndex = 12;
			this.btnVehicleGetMap.Text = "GetMap";
			this.btnVehicleGetMap.UseVisualStyleBackColor = true;
			this.btnVehicleGetMap.Click += new System.EventHandler(this.btnVehicleGetMap_Click);
			// 
			// btnVehicleChangeMap
			// 
			this.btnVehicleChangeMap.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehicleChangeMap.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
			this.btnVehicleChangeMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVehicleChangeMap.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnVehicleChangeMap.ForeColor = System.Drawing.Color.White;
			this.btnVehicleChangeMap.Location = new System.Drawing.Point(23, 548);
			this.btnVehicleChangeMap.Name = "btnVehicleChangeMap";
			this.btnVehicleChangeMap.Size = new System.Drawing.Size(104, 34);
			this.btnVehicleChangeMap.TabIndex = 13;
			this.btnVehicleChangeMap.Text = "ChangeMap";
			this.btnVehicleChangeMap.UseVisualStyleBackColor = true;
			this.btnVehicleChangeMap.Click += new System.EventHandler(this.btnVehicleChangeMap_Click);
			// 
			// btnVehicleUploadMap
			// 
			this.btnVehicleUploadMap.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnVehicleUploadMap.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
			this.btnVehicleUploadMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVehicleUploadMap.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnVehicleUploadMap.ForeColor = System.Drawing.Color.White;
			this.btnVehicleUploadMap.Location = new System.Drawing.Point(23, 508);
			this.btnVehicleUploadMap.Name = "btnVehicleUploadMap";
			this.btnVehicleUploadMap.Size = new System.Drawing.Size(104, 34);
			this.btnVehicleUploadMap.TabIndex = 14;
			this.btnVehicleUploadMap.Text = "UploadMap";
			this.btnVehicleUploadMap.UseVisualStyleBackColor = true;
			this.btnVehicleUploadMap.Click += new System.EventHandler(this.btnVehicleUploadMap_Click);
			// 
			// label2
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.label2, 3);
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(23, 5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(333, 30);
			this.label2.TabIndex = 17;
			this.label2.Text = "    Movement";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.label3, 3);
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(23, 200);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(333, 30);
			this.label3.TabIndex = 18;
			this.label3.Text = "    Intervene";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.label4, 3);
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(23, 395);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(333, 30);
			this.label4.TabIndex = 19;
			this.label4.Text = "    Map";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbRemoteMapNameList1
			// 
			this.cbRemoteMapNameList1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbRemoteMapNameList1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.tableLayoutPanel1.SetColumnSpan(this.cbRemoteMapNameList1, 2);
			this.cbRemoteMapNameList1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbRemoteMapNameList1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbRemoteMapNameList1.Font = new System.Drawing.Font("新細明體", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbRemoteMapNameList1.ForeColor = System.Drawing.Color.White;
			this.cbRemoteMapNameList1.FormattingEnabled = true;
			this.cbRemoteMapNameList1.Location = new System.Drawing.Point(133, 468);
			this.cbRemoteMapNameList1.Name = "cbRemoteMapNameList1";
			this.cbRemoteMapNameList1.Size = new System.Drawing.Size(223, 41);
			this.cbRemoteMapNameList1.TabIndex = 20;
			// 
			// cbRemoteMapNameList2
			// 
			this.cbRemoteMapNameList2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbRemoteMapNameList2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.tableLayoutPanel1.SetColumnSpan(this.cbRemoteMapNameList2, 2);
			this.cbRemoteMapNameList2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbRemoteMapNameList2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbRemoteMapNameList2.Font = new System.Drawing.Font("新細明體", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbRemoteMapNameList2.ForeColor = System.Drawing.Color.White;
			this.cbRemoteMapNameList2.FormattingEnabled = true;
			this.cbRemoteMapNameList2.Location = new System.Drawing.Point(133, 548);
			this.cbRemoteMapNameList2.Name = "cbRemoteMapNameList2";
			this.cbRemoteMapNameList2.Size = new System.Drawing.Size(223, 41);
			this.cbRemoteMapNameList2.TabIndex = 21;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel2.Controls.Add(this.cbVehicleNameList, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 60);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(400, 40);
			this.tableLayoutPanel2.TabIndex = 7;
			// 
			// cbVehicleNameList
			// 
			this.cbVehicleNameList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbVehicleNameList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.cbVehicleNameList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVehicleNameList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbVehicleNameList.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbVehicleNameList.ForeColor = System.Drawing.Color.White;
			this.cbVehicleNameList.FormattingEnabled = true;
			this.cbVehicleNameList.Location = new System.Drawing.Point(43, 3);
			this.cbVehicleNameList.Name = "cbVehicleNameList";
			this.cbVehicleNameList.Size = new System.Drawing.Size(314, 35);
			this.cbVehicleNameList.TabIndex = 3;
			this.cbVehicleNameList.SelectedIndexChanged += new System.EventHandler(this.cbVehicleNameList_SelectedIndexChanged);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.tableLayoutPanel1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 100);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(400, 550);
			this.panel1.TabIndex = 8;
			// 
			// txtCoordinate1
			// 
			this.txtCoordinate1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCoordinate1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.tableLayoutPanel1.SetColumnSpan(this.txtCoordinate1, 2);
			this.txtCoordinate1.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtCoordinate1.ForeColor = System.Drawing.Color.White;
			this.txtCoordinate1.Location = new System.Drawing.Point(133, 78);
			this.txtCoordinate1.Name = "txtCoordinate1";
			this.txtCoordinate1.Size = new System.Drawing.Size(223, 40);
			this.txtCoordinate1.TabIndex = 23;
			// 
			// txtCoordinate2
			// 
			this.txtCoordinate2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCoordinate2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.txtCoordinate2.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtCoordinate2.ForeColor = System.Drawing.Color.White;
			this.txtCoordinate2.Location = new System.Drawing.Point(193, 233);
			this.txtCoordinate2.Name = "txtCoordinate2";
			this.txtCoordinate2.Size = new System.Drawing.Size(163, 40);
			this.txtCoordinate2.TabIndex = 24;
			// 
			// UcVehicleApi
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.tableLayoutPanel2);
			this.Controls.Add(this.label1);
			this.Name = "UcVehicleApi";
			this.Size = new System.Drawing.Size(400, 650);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnVehicleGoto;
		private System.Windows.Forms.Button btnVehicleStop;
		private System.Windows.Forms.Button btnVehicleDock;
		private System.Windows.Forms.ComboBox cbGoalNameList;
		private System.Windows.Forms.Button btnVehicleInsertMovingBuffer;
		private System.Windows.Forms.Button btnVehicleRemoveMovingBuffer;
		private System.Windows.Forms.Button btnVehiclePause;
		private System.Windows.Forms.Button btnVehicleResume;
		private System.Windows.Forms.Button btnVehicleRequestMapList;
		private System.Windows.Forms.Button btnVehicleGetMap;
		private System.Windows.Forms.Button btnVehicleChangeMap;
		private System.Windows.Forms.Button btnVehicleUploadMap;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.ComboBox cbVehicleNameList;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnVehicleGotoPoint;
		private System.Windows.Forms.ComboBox cbRemoteMapNameList1;
		private System.Windows.Forms.ComboBox cbRemoteMapNameList2;
		private System.Windows.Forms.ComboBox cbLocalMapNameList;
		private TextBoxWithHint txtCoordinate1;
		private TextBoxWithHint txtCoordinate2;
	}
}
