namespace TrafficControlTest.UserControl
{
	partial class UCVehicleManualControl
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
			this.cbVehicleNameList = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lbGoalList = new System.Windows.Forms.ListBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
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
			this.label3.TabIndex = 4;
			this.label3.Text = "    Vehicle Manual Control";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			this.tableLayoutPanel2.TabIndex = 6;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
			this.tableLayoutPanel1.Controls.Add(this.lbGoalList, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 100);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 550);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// lbGoalList
			// 
			this.lbGoalList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.lbGoalList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbGoalList.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lbGoalList.ForeColor = System.Drawing.Color.White;
			this.lbGoalList.FormattingEnabled = true;
			this.lbGoalList.ItemHeight = 20;
			this.lbGoalList.Location = new System.Drawing.Point(23, 23);
			this.lbGoalList.Name = "lbGoalList";
			this.lbGoalList.ScrollAlwaysVisible = true;
			this.lbGoalList.Size = new System.Drawing.Size(354, 454);
			this.lbGoalList.TabIndex = 1;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.Controls.Add(this.button1, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.button2, 1, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(23, 483);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(354, 44);
			this.tableLayoutPanel4.TabIndex = 5;
			// 
			// button1
			// 
			this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point(3, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(171, 38);
			this.button1.TabIndex = 5;
			this.button1.Text = "Goto";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Location = new System.Drawing.Point(180, 3);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(171, 38);
			this.button2.TabIndex = 4;
			this.button2.Text = "Stop";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// UCVehicleManualControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.tableLayoutPanel2);
			this.Controls.Add(this.label3);
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "UCVehicleManualControl";
			this.Size = new System.Drawing.Size(400, 650);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbVehicleNameList;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ListBox lbGoalList;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
	}
}
