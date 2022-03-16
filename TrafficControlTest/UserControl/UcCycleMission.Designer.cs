namespace TrafficControlTest.UserControl
{
	partial class UcCycleMission
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
			this.label3 = new System.Windows.Forms.Label();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.cbVehicleStateList = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnAnalyzeMissionListString = new System.Windows.Forms.Button();
			this.btnStartCycle = new System.Windows.Forms.Button();
			this.btnStopCycle = new System.Windows.Forms.Button();
			this.dgvMissionList = new System.Windows.Forms.DataGridView();
			this.txtMissionListString = new TrafficControlTest.UserControl.TextBoxWithHint();
			this.btnStopVehicle = new System.Windows.Forms.Button();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvMissionList)).BeginInit();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.Dock = System.Windows.Forms.DockStyle.Top;
			this.label3.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(0, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(400, 60);
			this.label3.TabIndex = 5;
			this.label3.Text = "    Cycle Mission";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel2.Controls.Add(this.cbVehicleStateList, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 60);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(400, 40);
			this.tableLayoutPanel2.TabIndex = 7;
			// 
			// cbVehicleStateList
			// 
			this.cbVehicleStateList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbVehicleStateList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.cbVehicleStateList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVehicleStateList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbVehicleStateList.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbVehicleStateList.ForeColor = System.Drawing.Color.White;
			this.cbVehicleStateList.FormattingEnabled = true;
			this.cbVehicleStateList.Location = new System.Drawing.Point(43, 5);
			this.cbVehicleStateList.Name = "cbVehicleStateList";
			this.cbVehicleStateList.Size = new System.Drawing.Size(314, 30);
			this.cbVehicleStateList.TabIndex = 3;
			this.cbVehicleStateList.SelectedIndexChanged += new System.EventHandler(this.cbVehicleStateList_SelectedIndexChanged);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.tableLayoutPanel1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.ForeColor = System.Drawing.Color.White;
			this.panel1.Location = new System.Drawing.Point(0, 100);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(400, 550);
			this.panel1.TabIndex = 8;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.btnStopVehicle, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.btnAnalyzeMissionListString, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnStartCycle, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.btnStopCycle, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.dgvMissionList, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtMissionListString, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 550);
			this.tableLayoutPanel1.TabIndex = 9;
			// 
			// btnAnalyzeMissionListString
			// 
			this.btnAnalyzeMissionListString.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnAnalyzeMissionListString.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.btnAnalyzeMissionListString.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAnalyzeMissionListString.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnAnalyzeMissionListString.ForeColor = System.Drawing.Color.White;
			this.btnAnalyzeMissionListString.Location = new System.Drawing.Point(293, 23);
			this.btnAnalyzeMissionListString.Name = "btnAnalyzeMissionListString";
			this.btnAnalyzeMissionListString.Size = new System.Drawing.Size(84, 34);
			this.btnAnalyzeMissionListString.TabIndex = 25;
			this.btnAnalyzeMissionListString.Text = "Analyze";
			this.btnAnalyzeMissionListString.UseVisualStyleBackColor = true;
			this.btnAnalyzeMissionListString.Click += new System.EventHandler(this.btnAnalyzeMissionListString_Click);
			// 
			// btnStartCycle
			// 
			this.btnStartCycle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnStartCycle.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.btnStartCycle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStartCycle.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnStartCycle.ForeColor = System.Drawing.Color.White;
			this.btnStartCycle.Location = new System.Drawing.Point(23, 453);
			this.btnStartCycle.Name = "btnStartCycle";
			this.btnStartCycle.Size = new System.Drawing.Size(174, 34);
			this.btnStartCycle.TabIndex = 26;
			this.btnStartCycle.Text = "Start Cycle";
			this.btnStartCycle.UseVisualStyleBackColor = true;
			this.btnStartCycle.Click += new System.EventHandler(this.btnStartCycle_Click);
			// 
			// btnStopCycle
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.btnStopCycle, 2);
			this.btnStopCycle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnStopCycle.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.btnStopCycle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStopCycle.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnStopCycle.ForeColor = System.Drawing.Color.White;
			this.btnStopCycle.Location = new System.Drawing.Point(203, 453);
			this.btnStopCycle.Name = "btnStopCycle";
			this.btnStopCycle.Size = new System.Drawing.Size(174, 34);
			this.btnStopCycle.TabIndex = 27;
			this.btnStopCycle.Text = "Stop Cycle";
			this.btnStopCycle.UseVisualStyleBackColor = true;
			this.btnStopCycle.Click += new System.EventHandler(this.btnStopCycle_Click);
			// 
			// dgvMissionList
			// 
			this.dgvMissionList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.tableLayoutPanel1.SetColumnSpan(this.dgvMissionList, 3);
			this.dgvMissionList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvMissionList.Location = new System.Drawing.Point(23, 63);
			this.dgvMissionList.Name = "dgvMissionList";
			this.dgvMissionList.RowTemplate.Height = 27;
			this.dgvMissionList.Size = new System.Drawing.Size(354, 384);
			this.dgvMissionList.TabIndex = 28;
			// 
			// txtMissionListString
			// 
			this.txtMissionListString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMissionListString.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.tableLayoutPanel1.SetColumnSpan(this.txtMissionListString, 2);
			this.txtMissionListString.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtMissionListString.ForeColor = System.Drawing.Color.White;
			this.txtMissionListString.Location = new System.Drawing.Point(23, 23);
			this.txtMissionListString.Name = "txtMissionListString";
			this.txtMissionListString.Size = new System.Drawing.Size(264, 33);
			this.txtMissionListString.TabIndex = 24;
			// 
			// btnStopVehicle
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.btnStopVehicle, 3);
			this.btnStopVehicle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnStopVehicle.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.btnStopVehicle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStopVehicle.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnStopVehicle.ForeColor = System.Drawing.Color.White;
			this.btnStopVehicle.Location = new System.Drawing.Point(23, 493);
			this.btnStopVehicle.Name = "btnStopVehicle";
			this.btnStopVehicle.Size = new System.Drawing.Size(354, 34);
			this.btnStopVehicle.TabIndex = 29;
			this.btnStopVehicle.Text = "Stop Vehicle";
			this.btnStopVehicle.UseVisualStyleBackColor = true;
			this.btnStopVehicle.Click += new System.EventHandler(this.btnStopVehicle_Click);
			// 
			// UcCycleMission
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.tableLayoutPanel2);
			this.Controls.Add(this.label3);
			this.Name = "UcCycleMission";
			this.Size = new System.Drawing.Size(400, 650);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvMissionList)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.ComboBox cbVehicleStateList;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnAnalyzeMissionListString;
		private System.Windows.Forms.Button btnStartCycle;
		private System.Windows.Forms.Button btnStopCycle;
		private System.Windows.Forms.DataGridView dgvMissionList;
		private TextBoxWithHint txtMissionListString;
		private System.Windows.Forms.Button btnStopVehicle;
	}
}
