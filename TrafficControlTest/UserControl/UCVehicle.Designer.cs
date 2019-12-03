namespace TrafficControlTest.UserControl
{
	partial class UcVehicle
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
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.cbVehicleNameList = new System.Windows.Forms.ComboBox();
			this.btnRefreshVehicleState = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.lblVehicleState = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.lblVehicleLocation = new System.Windows.Forms.Label();
			this.lblVehicleTarget = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.lblVehiclePath = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lblVehicleVelocity = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblVehicleLocationScore = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.lblVehicleBatteryValue = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.lblVehicleLastUpdateTime = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.lblVehicleInterveneCommand = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblVehicleIpPort = new System.Windows.Forms.Label();
			this.lblVehicleAlarmMessage = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.lblVehicleMissionId = new System.Windows.Forms.Label();
			this.lblVehicleMapName = new System.Windows.Forms.Label();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 4;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 600F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.cbVehicleNameList, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.btnRefreshVehicleState, 2, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(850, 80);
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
			this.cbVehicleNameList.Location = new System.Drawing.Point(53, 23);
			this.cbVehicleNameList.Name = "cbVehicleNameList";
			this.cbVehicleNameList.Size = new System.Drawing.Size(594, 35);
			this.cbVehicleNameList.TabIndex = 3;
			this.cbVehicleNameList.SelectedIndexChanged += new System.EventHandler(this.cbVehicleNameList_SelectedIndexChanged);
			// 
			// btnRefreshVehicleState
			// 
			this.btnRefreshVehicleState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnRefreshVehicleState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnRefreshVehicleState.Location = new System.Drawing.Point(653, 23);
			this.btnRefreshVehicleState.Name = "btnRefreshVehicleState";
			this.btnRefreshVehicleState.Size = new System.Drawing.Size(144, 34);
			this.btnRefreshVehicleState.TabIndex = 4;
			this.btnRefreshVehicleState.Text = "Refresh";
			this.btnRefreshVehicleState.UseVisualStyleBackColor = true;
			this.btnRefreshVehicleState.Click += new System.EventHandler(this.btnRefreshVehicleState_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 6;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleState, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblVehiclePath, 4, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleLocationScore, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.label7, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleVelocity, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.label5, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleLocation, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.label6, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleTarget, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.label23, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.label14, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleBatteryValue, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.label3, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.label2, 3, 3);
			this.tableLayoutPanel1.Controls.Add(this.label4, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleIpPort, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleAlarmMessage, 4, 3);
			this.tableLayoutPanel1.Controls.Add(this.label29, 3, 6);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleLastUpdateTime, 4, 6);
			this.tableLayoutPanel1.Controls.Add(this.label28, 3, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleInterveneCommand, 4, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleMissionId, 4, 4);
			this.tableLayoutPanel1.Controls.Add(this.label9, 3, 4);
			this.tableLayoutPanel1.Controls.Add(this.label8, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleMapName, 2, 5);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 80);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 11;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(850, 520);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.Location = new System.Drawing.Point(112, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "State :";
			// 
			// lblVehicleState
			// 
			this.lblVehicleState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleState.AutoSize = true;
			this.lblVehicleState.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleState.Location = new System.Drawing.Point(173, 10);
			this.lblVehicleState.Name = "lblVehicleState";
			this.lblVehicleState.Size = new System.Drawing.Size(204, 20);
			this.lblVehicleState.TabIndex = 1;
			this.lblVehicleState.Text = "Running";
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label5.Location = new System.Drawing.Point(484, 10);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(83, 20);
			this.label5.TabIndex = 4;
			this.label5.Text = "Location :";
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label6.Location = new System.Drawing.Point(101, 50);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(66, 20);
			this.label6.TabIndex = 5;
			this.label6.Text = "Target :";
			// 
			// lblVehicleLocation
			// 
			this.lblVehicleLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleLocation.AutoSize = true;
			this.lblVehicleLocation.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleLocation.Location = new System.Drawing.Point(573, 10);
			this.lblVehicleLocation.Name = "lblVehicleLocation";
			this.lblVehicleLocation.Size = new System.Drawing.Size(254, 20);
			this.lblVehicleLocation.TabIndex = 9;
			this.lblVehicleLocation.Text = "(235, 122, 20.34)";
			// 
			// lblVehicleTarget
			// 
			this.lblVehicleTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleTarget.AutoSize = true;
			this.lblVehicleTarget.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleTarget.Location = new System.Drawing.Point(173, 50);
			this.lblVehicleTarget.Name = "lblVehicleTarget";
			this.lblVehicleTarget.Size = new System.Drawing.Size(204, 20);
			this.lblVehicleTarget.TabIndex = 10;
			this.lblVehicleTarget.Text = "Goal8";
			// 
			// label23
			// 
			this.label23.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label23.AutoSize = true;
			this.label23.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label23.Location = new System.Drawing.Point(516, 90);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(51, 20);
			this.label23.TabIndex = 24;
			this.label23.Text = "Path :";
			// 
			// lblVehiclePath
			// 
			this.lblVehiclePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehiclePath.AutoSize = true;
			this.lblVehiclePath.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehiclePath.Location = new System.Drawing.Point(573, 90);
			this.lblVehiclePath.Name = "lblVehiclePath";
			this.lblVehiclePath.Size = new System.Drawing.Size(254, 20);
			this.lblVehiclePath.TabIndex = 25;
			this.lblVehiclePath.Text = "(112,346)(234,1234)";
			// 
			// label7
			// 
			this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label7.Location = new System.Drawing.Point(485, 50);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(82, 20);
			this.label7.TabIndex = 6;
			this.label7.Text = "Velocity :";
			// 
			// lblVehicleVelocity
			// 
			this.lblVehicleVelocity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleVelocity.AutoSize = true;
			this.lblVehicleVelocity.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleVelocity.Location = new System.Drawing.Point(573, 50);
			this.lblVehicleVelocity.Name = "lblVehicleVelocity";
			this.lblVehicleVelocity.Size = new System.Drawing.Size(254, 20);
			this.lblVehicleVelocity.TabIndex = 11;
			this.lblVehicleVelocity.Text = "999 (mm/s)";
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label3.Location = new System.Drawing.Point(38, 130);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(129, 20);
			this.label3.TabIndex = 2;
			this.label3.Text = "Location Score :";
			// 
			// lblVehicleLocationScore
			// 
			this.lblVehicleLocationScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleLocationScore.AutoSize = true;
			this.lblVehicleLocationScore.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleLocationScore.Location = new System.Drawing.Point(173, 130);
			this.lblVehicleLocationScore.Name = "lblVehicleLocationScore";
			this.lblVehicleLocationScore.Size = new System.Drawing.Size(204, 20);
			this.lblVehicleLocationScore.TabIndex = 3;
			this.lblVehicleLocationScore.Text = "90 %";
			// 
			// label14
			// 
			this.label14.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label14.AutoSize = true;
			this.label14.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label14.Location = new System.Drawing.Point(45, 90);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(122, 20);
			this.label14.TabIndex = 22;
			this.label14.Text = "Battery Value :";
			// 
			// lblVehicleBatteryValue
			// 
			this.lblVehicleBatteryValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleBatteryValue.AutoSize = true;
			this.lblVehicleBatteryValue.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleBatteryValue.Location = new System.Drawing.Point(173, 90);
			this.lblVehicleBatteryValue.Name = "lblVehicleBatteryValue";
			this.lblVehicleBatteryValue.Size = new System.Drawing.Size(204, 20);
			this.lblVehicleBatteryValue.TabIndex = 23;
			this.lblVehicleBatteryValue.Text = "50.9 %";
			// 
			// label29
			// 
			this.label29.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label29.AutoSize = true;
			this.label29.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label29.Location = new System.Drawing.Point(415, 250);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(152, 20);
			this.label29.TabIndex = 30;
			this.label29.Text = "Last Update Time :";
			// 
			// lblVehicleLastUpdateTime
			// 
			this.lblVehicleLastUpdateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleLastUpdateTime.AutoSize = true;
			this.lblVehicleLastUpdateTime.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleLastUpdateTime.Location = new System.Drawing.Point(573, 250);
			this.lblVehicleLastUpdateTime.Name = "lblVehicleLastUpdateTime";
			this.lblVehicleLastUpdateTime.Size = new System.Drawing.Size(254, 20);
			this.lblVehicleLastUpdateTime.TabIndex = 31;
			this.lblVehicleLastUpdateTime.Text = "2019/11/07 11:25:30.231";
			// 
			// label28
			// 
			this.label28.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label28.AutoSize = true;
			this.label28.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label28.Location = new System.Drawing.Point(397, 210);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(170, 20);
			this.label28.TabIndex = 29;
			this.label28.Text = "Intervene Command :";
			// 
			// lblVehicleInterveneCommand
			// 
			this.lblVehicleInterveneCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleInterveneCommand.AutoSize = true;
			this.lblVehicleInterveneCommand.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleInterveneCommand.Location = new System.Drawing.Point(573, 210);
			this.lblVehicleInterveneCommand.Name = "lblVehicleInterveneCommand";
			this.lblVehicleInterveneCommand.Size = new System.Drawing.Size(254, 20);
			this.lblVehicleInterveneCommand.TabIndex = 28;
			this.lblVehicleInterveneCommand.Text = "Pausing";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label2.Location = new System.Drawing.Point(431, 130);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 20);
			this.label2.TabIndex = 32;
			this.label2.Text = "Alarm Message :";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label4.Location = new System.Drawing.Point(97, 170);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 20);
			this.label4.TabIndex = 33;
			this.label4.Text = "IP Port :";
			// 
			// lblVehicleIpPort
			// 
			this.lblVehicleIpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleIpPort.AutoSize = true;
			this.lblVehicleIpPort.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleIpPort.Location = new System.Drawing.Point(173, 170);
			this.lblVehicleIpPort.Name = "lblVehicleIpPort";
			this.lblVehicleIpPort.Size = new System.Drawing.Size(204, 20);
			this.lblVehicleIpPort.TabIndex = 34;
			this.lblVehicleIpPort.Text = "127.0.0.1:65536";
			// 
			// lblVehicleAlarmMessage
			// 
			this.lblVehicleAlarmMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleAlarmMessage.AutoSize = true;
			this.lblVehicleAlarmMessage.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleAlarmMessage.Location = new System.Drawing.Point(573, 130);
			this.lblVehicleAlarmMessage.Name = "lblVehicleAlarmMessage";
			this.lblVehicleAlarmMessage.Size = new System.Drawing.Size(254, 20);
			this.lblVehicleAlarmMessage.TabIndex = 35;
			this.lblVehicleAlarmMessage.Text = "Alarm Message";
			// 
			// label8
			// 
			this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label8.Location = new System.Drawing.Point(64, 210);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(103, 20);
			this.label8.TabIndex = 36;
			this.label8.Text = "Map Name :";
			// 
			// label9
			// 
			this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label9.Location = new System.Drawing.Point(464, 170);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(103, 20);
			this.label9.TabIndex = 37;
			this.label9.Text = "Mission ID :";
			// 
			// lblVehicleMissionId
			// 
			this.lblVehicleMissionId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleMissionId.AutoSize = true;
			this.lblVehicleMissionId.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleMissionId.Location = new System.Drawing.Point(573, 170);
			this.lblVehicleMissionId.Name = "lblVehicleMissionId";
			this.lblVehicleMissionId.Size = new System.Drawing.Size(254, 20);
			this.lblVehicleMissionId.TabIndex = 38;
			this.lblVehicleMissionId.Text = "20191114153022777";
			// 
			// lblVehicleMapName
			// 
			this.lblVehicleMapName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleMapName.AutoSize = true;
			this.lblVehicleMapName.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleMapName.Location = new System.Drawing.Point(173, 210);
			this.lblVehicleMapName.Name = "lblVehicleMapName";
			this.lblVehicleMapName.Size = new System.Drawing.Size(204, 20);
			this.lblVehicleMapName.TabIndex = 39;
			this.lblVehicleMapName.Text = "20191111.map";
			// 
			// UCVehicle
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.tableLayoutPanel2);
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "UCVehicle";
			this.Size = new System.Drawing.Size(850, 600);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.ComboBox cbVehicleNameList;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblVehicleState;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblVehicleLocationScore;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblVehicleLocation;
		private System.Windows.Forms.Label lblVehicleTarget;
		private System.Windows.Forms.Label lblVehicleVelocity;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label lblVehicleBatteryValue;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label lblVehiclePath;
		private System.Windows.Forms.Label lblVehicleInterveneCommand;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label lblVehicleLastUpdateTime;
		private System.Windows.Forms.Button btnRefreshVehicleState;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblVehicleIpPort;
		private System.Windows.Forms.Label lblVehicleAlarmMessage;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label lblVehicleMissionId;
		private System.Windows.Forms.Label lblVehicleMapName;
	}
}
