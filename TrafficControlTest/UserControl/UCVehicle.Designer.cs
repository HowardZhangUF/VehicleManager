namespace TrafficControlTest.UserControl
{
	partial class UCVehicle
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
			this.lblVehiclePosition = new System.Windows.Forms.Label();
			this.lblVehicleTarget = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.lblVehicleToward = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.lblVehiclePath = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lblVehicleVelocity = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblVehicleMatch = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.lblVehicleBattery = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.lblVehicleLastUpdateTime = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.lblVehicleIntervenable = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.lblVehicleInterveneCommand = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.lblVehicleIntervening = new System.Windows.Forms.Label();
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
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleState, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.label5, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label6, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblVehiclePosition, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleTarget, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.label15, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleToward, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.label23, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblVehiclePath, 4, 2);
			this.tableLayoutPanel1.Controls.Add(this.label7, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleVelocity, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.label3, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleMatch, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.label14, 3, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleBattery, 4, 3);
			this.tableLayoutPanel1.Controls.Add(this.label29, 3, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleLastUpdateTime, 4, 5);
			this.tableLayoutPanel1.Controls.Add(this.label16, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleIntervenable, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.label28, 3, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleInterveneCommand, 4, 4);
			this.tableLayoutPanel1.Controls.Add(this.label25, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblVehicleIntervening, 2, 5);
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
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(850, 520);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.Location = new System.Drawing.Point(142, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "State :";
			// 
			// lblVehicleState
			// 
			this.lblVehicleState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleState.AutoSize = true;
			this.lblVehicleState.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleState.Location = new System.Drawing.Point(203, 10);
			this.lblVehicleState.Name = "lblVehicleState";
			this.lblVehicleState.Size = new System.Drawing.Size(144, 20);
			this.lblVehicleState.TabIndex = 1;
			this.lblVehicleState.Text = "Running";
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label5.Location = new System.Drawing.Point(119, 50);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(78, 20);
			this.label5.TabIndex = 4;
			this.label5.Text = "Position :";
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label6.Location = new System.Drawing.Point(131, 90);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(66, 20);
			this.label6.TabIndex = 5;
			this.label6.Text = "Target :";
			// 
			// lblVehiclePosition
			// 
			this.lblVehiclePosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehiclePosition.AutoSize = true;
			this.lblVehiclePosition.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehiclePosition.Location = new System.Drawing.Point(203, 50);
			this.lblVehiclePosition.Name = "lblVehiclePosition";
			this.lblVehiclePosition.Size = new System.Drawing.Size(144, 20);
			this.lblVehiclePosition.TabIndex = 9;
			this.lblVehiclePosition.Text = "(235, 122)";
			// 
			// lblVehicleTarget
			// 
			this.lblVehicleTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleTarget.AutoSize = true;
			this.lblVehicleTarget.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleTarget.Location = new System.Drawing.Point(203, 90);
			this.lblVehicleTarget.Name = "lblVehicleTarget";
			this.lblVehicleTarget.Size = new System.Drawing.Size(144, 20);
			this.lblVehicleTarget.TabIndex = 10;
			this.lblVehicleTarget.Text = "Goal8";
			// 
			// label15
			// 
			this.label15.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label15.AutoSize = true;
			this.label15.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label15.Location = new System.Drawing.Point(461, 50);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(76, 20);
			this.label15.TabIndex = 14;
			this.label15.Text = "Toward :";
			// 
			// lblVehicleToward
			// 
			this.lblVehicleToward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleToward.AutoSize = true;
			this.lblVehicleToward.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleToward.Location = new System.Drawing.Point(543, 50);
			this.lblVehicleToward.Name = "lblVehicleToward";
			this.lblVehicleToward.Size = new System.Drawing.Size(254, 20);
			this.lblVehicleToward.TabIndex = 19;
			this.lblVehicleToward.Text = "160 deg";
			// 
			// label23
			// 
			this.label23.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label23.AutoSize = true;
			this.label23.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label23.Location = new System.Drawing.Point(486, 90);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(51, 20);
			this.label23.TabIndex = 24;
			this.label23.Text = "Path :";
			// 
			// lblVehiclePath
			// 
			this.lblVehiclePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehiclePath.AutoSize = true;
			this.lblVehiclePath.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehiclePath.Location = new System.Drawing.Point(543, 90);
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
			this.label7.Location = new System.Drawing.Point(455, 10);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(82, 20);
			this.label7.TabIndex = 6;
			this.label7.Text = "Velocity :";
			// 
			// lblVehicleVelocity
			// 
			this.lblVehicleVelocity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleVelocity.AutoSize = true;
			this.lblVehicleVelocity.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleVelocity.Location = new System.Drawing.Point(543, 10);
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
			this.label3.Location = new System.Drawing.Point(131, 130);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(66, 20);
			this.label3.TabIndex = 2;
			this.label3.Text = "Match :";
			// 
			// lblVehicleMatch
			// 
			this.lblVehicleMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleMatch.AutoSize = true;
			this.lblVehicleMatch.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleMatch.Location = new System.Drawing.Point(203, 130);
			this.lblVehicleMatch.Name = "lblVehicleMatch";
			this.lblVehicleMatch.Size = new System.Drawing.Size(144, 20);
			this.lblVehicleMatch.TabIndex = 3;
			this.lblVehicleMatch.Text = "90 %";
			// 
			// label14
			// 
			this.label14.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label14.AutoSize = true;
			this.label14.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label14.Location = new System.Drawing.Point(464, 130);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(73, 20);
			this.label14.TabIndex = 22;
			this.label14.Text = "Battery :";
			// 
			// lblVehicleBattery
			// 
			this.lblVehicleBattery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleBattery.AutoSize = true;
			this.lblVehicleBattery.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleBattery.Location = new System.Drawing.Point(543, 130);
			this.lblVehicleBattery.Name = "lblVehicleBattery";
			this.lblVehicleBattery.Size = new System.Drawing.Size(254, 20);
			this.lblVehicleBattery.TabIndex = 23;
			this.lblVehicleBattery.Text = "50.9 %";
			// 
			// label29
			// 
			this.label29.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label29.AutoSize = true;
			this.label29.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label29.Location = new System.Drawing.Point(385, 210);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(152, 20);
			this.label29.TabIndex = 30;
			this.label29.Text = "Last Update Time :";
			// 
			// lblVehicleLastUpdateTime
			// 
			this.lblVehicleLastUpdateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleLastUpdateTime.AutoSize = true;
			this.lblVehicleLastUpdateTime.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleLastUpdateTime.Location = new System.Drawing.Point(543, 210);
			this.lblVehicleLastUpdateTime.Name = "lblVehicleLastUpdateTime";
			this.lblVehicleLastUpdateTime.Size = new System.Drawing.Size(254, 20);
			this.lblVehicleLastUpdateTime.TabIndex = 31;
			this.lblVehicleLastUpdateTime.Text = "2019/11/07 11:25:30.231";
			// 
			// label16
			// 
			this.label16.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label16.AutoSize = true;
			this.label16.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label16.Location = new System.Drawing.Point(88, 170);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(109, 20);
			this.label16.TabIndex = 15;
			this.label16.Text = "Intervenable :";
			// 
			// lblVehicleIntervenable
			// 
			this.lblVehicleIntervenable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleIntervenable.AutoSize = true;
			this.lblVehicleIntervenable.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleIntervenable.Location = new System.Drawing.Point(203, 170);
			this.lblVehicleIntervenable.Name = "lblVehicleIntervenable";
			this.lblVehicleIntervenable.Size = new System.Drawing.Size(144, 20);
			this.lblVehicleIntervenable.TabIndex = 21;
			this.lblVehicleIntervenable.Text = "True";
			// 
			// label28
			// 
			this.label28.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label28.AutoSize = true;
			this.label28.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label28.Location = new System.Drawing.Point(367, 170);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(170, 20);
			this.label28.TabIndex = 29;
			this.label28.Text = "Intervene Command :";
			// 
			// lblVehicleInterveneCommand
			// 
			this.lblVehicleInterveneCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleInterveneCommand.AutoSize = true;
			this.lblVehicleInterveneCommand.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleInterveneCommand.Location = new System.Drawing.Point(543, 170);
			this.lblVehicleInterveneCommand.Name = "lblVehicleInterveneCommand";
			this.lblVehicleInterveneCommand.Size = new System.Drawing.Size(254, 20);
			this.lblVehicleInterveneCommand.TabIndex = 28;
			this.lblVehicleInterveneCommand.Text = "Pausing";
			// 
			// label25
			// 
			this.label25.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label25.AutoSize = true;
			this.label25.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label25.Location = new System.Drawing.Point(95, 210);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(102, 20);
			this.label25.TabIndex = 26;
			this.label25.Text = "Intervening :";
			// 
			// lblVehicleIntervening
			// 
			this.lblVehicleIntervening.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVehicleIntervening.AutoSize = true;
			this.lblVehicleIntervening.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblVehicleIntervening.Location = new System.Drawing.Point(203, 210);
			this.lblVehicleIntervening.Name = "lblVehicleIntervening";
			this.lblVehicleIntervening.Size = new System.Drawing.Size(144, 20);
			this.lblVehicleIntervening.TabIndex = 27;
			this.lblVehicleIntervening.Text = "True";
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
		private System.Windows.Forms.Label lblVehicleMatch;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblVehiclePosition;
		private System.Windows.Forms.Label lblVehicleTarget;
		private System.Windows.Forms.Label lblVehicleVelocity;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label lblVehicleToward;
		private System.Windows.Forms.Label lblVehicleIntervenable;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label lblVehicleBattery;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label lblVehiclePath;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label lblVehicleIntervening;
		private System.Windows.Forms.Label lblVehicleInterveneCommand;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label lblVehicleLastUpdateTime;
		private System.Windows.Forms.Button btnRefreshVehicleState;
	}
}
