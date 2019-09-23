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
			this.txtInterveneMovingBuffer = new System.Windows.Forms.TextBox();
			this.cbVehicleNameList = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.btnIntervenePauseMoving = new System.Windows.Forms.Button();
			this.btnInterveneRemoveMovingBuffer = new System.Windows.Forms.Button();
			this.btnInterveneInsertMovingBuffer = new System.Windows.Forms.Button();
			this.btnInterveneResumeMoving = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.lbGoalList = new System.Windows.Forms.ListBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
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
			// txtInterveneMovingBuffer
			// 
			this.txtInterveneMovingBuffer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtInterveneMovingBuffer.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtInterveneMovingBuffer.Location = new System.Drawing.Point(203, 23);
			this.txtInterveneMovingBuffer.Name = "txtInterveneMovingBuffer";
			this.txtInterveneMovingBuffer.Size = new System.Drawing.Size(143, 40);
			this.txtInterveneMovingBuffer.TabIndex = 1;
			this.txtInterveneMovingBuffer.Text = "-9000,7000";
			this.txtInterveneMovingBuffer.Visible = false;
			// 
			// cbVehicleNameList
			// 
			this.cbVehicleNameList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.cbVehicleNameList.Dock = System.Windows.Forms.DockStyle.Fill;
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
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 4;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
			this.tableLayoutPanel3.Controls.Add(this.label2, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.btnIntervenePauseMoving, 1, 3);
			this.tableLayoutPanel3.Controls.Add(this.txtInterveneMovingBuffer, 2, 1);
			this.tableLayoutPanel3.Controls.Add(this.btnInterveneRemoveMovingBuffer, 2, 2);
			this.tableLayoutPanel3.Controls.Add(this.btnInterveneInsertMovingBuffer, 1, 2);
			this.tableLayoutPanel3.Controls.Add(this.btnInterveneResumeMoving, 2, 3);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 480);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 5;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(400, 170);
			this.tableLayoutPanel3.TabIndex = 7;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label2.Location = new System.Drawing.Point(23, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(174, 30);
			this.label2.TabIndex = 3;
			this.label2.Text = "        Intervene";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnIntervenePauseMoving
			// 
			this.btnIntervenePauseMoving.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnIntervenePauseMoving.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnIntervenePauseMoving.Location = new System.Drawing.Point(23, 103);
			this.btnIntervenePauseMoving.Name = "btnIntervenePauseMoving";
			this.btnIntervenePauseMoving.Size = new System.Drawing.Size(174, 44);
			this.btnIntervenePauseMoving.TabIndex = 2;
			this.btnIntervenePauseMoving.Text = "Pause Moving";
			this.btnIntervenePauseMoving.UseVisualStyleBackColor = true;
			// 
			// btnInterveneRemoveMovingBuffer
			// 
			this.btnInterveneRemoveMovingBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnInterveneRemoveMovingBuffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnInterveneRemoveMovingBuffer.Location = new System.Drawing.Point(203, 53);
			this.btnInterveneRemoveMovingBuffer.Name = "btnInterveneRemoveMovingBuffer";
			this.btnInterveneRemoveMovingBuffer.Size = new System.Drawing.Size(174, 44);
			this.btnInterveneRemoveMovingBuffer.TabIndex = 2;
			this.btnInterveneRemoveMovingBuffer.Text = "Remove Moving Buffer";
			this.btnInterveneRemoveMovingBuffer.UseVisualStyleBackColor = true;
			// 
			// btnInterveneInsertMovingBuffer
			// 
			this.btnInterveneInsertMovingBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnInterveneInsertMovingBuffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnInterveneInsertMovingBuffer.Location = new System.Drawing.Point(23, 53);
			this.btnInterveneInsertMovingBuffer.Name = "btnInterveneInsertMovingBuffer";
			this.btnInterveneInsertMovingBuffer.Size = new System.Drawing.Size(174, 44);
			this.btnInterveneInsertMovingBuffer.TabIndex = 2;
			this.btnInterveneInsertMovingBuffer.Text = "Insert Moving Buffer";
			this.btnInterveneInsertMovingBuffer.UseVisualStyleBackColor = true;
			// 
			// btnInterveneResumeMoving
			// 
			this.btnInterveneResumeMoving.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnInterveneResumeMoving.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnInterveneResumeMoving.Location = new System.Drawing.Point(203, 103);
			this.btnInterveneResumeMoving.Name = "btnInterveneResumeMoving";
			this.btnInterveneResumeMoving.Size = new System.Drawing.Size(174, 44);
			this.btnInterveneResumeMoving.TabIndex = 2;
			this.btnInterveneResumeMoving.Text = "Resume Moving";
			this.btnInterveneResumeMoving.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lbGoalList, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 100);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 380);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.Location = new System.Drawing.Point(23, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(354, 30);
			this.label1.TabIndex = 0;
			this.label1.Text = "        Normal";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbGoalList
			// 
			this.lbGoalList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.lbGoalList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbGoalList.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lbGoalList.ForeColor = System.Drawing.Color.White;
			this.lbGoalList.FormattingEnabled = true;
			this.lbGoalList.ItemHeight = 20;
			this.lbGoalList.Location = new System.Drawing.Point(23, 53);
			this.lbGoalList.Name = "lbGoalList";
			this.lbGoalList.ScrollAlwaysVisible = true;
			this.lbGoalList.Size = new System.Drawing.Size(354, 274);
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
			this.tableLayoutPanel4.Location = new System.Drawing.Point(23, 333);
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
			this.Controls.Add(this.tableLayoutPanel3);
			this.Controls.Add(this.tableLayoutPanel2);
			this.Controls.Add(this.label3);
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "UCVehicleManualControl";
			this.Size = new System.Drawing.Size(400, 650);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtInterveneMovingBuffer;
		private System.Windows.Forms.ComboBox cbVehicleNameList;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnIntervenePauseMoving;
		private System.Windows.Forms.Button btnInterveneRemoveMovingBuffer;
		private System.Windows.Forms.Button btnInterveneInsertMovingBuffer;
		private System.Windows.Forms.Button btnInterveneResumeMoving;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lbGoalList;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
	}
}
