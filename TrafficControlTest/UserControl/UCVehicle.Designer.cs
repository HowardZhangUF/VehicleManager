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
            this.dgvVehicleInfo = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvVehicleControl = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicleInfo)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicleControl)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvVehicleInfo
            // 
            this.dgvVehicleInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVehicleInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVehicleInfo.Location = new System.Drawing.Point(3, 3);
            this.dgvVehicleInfo.Name = "dgvVehicleInfo";
            this.dgvVehicleInfo.RowTemplate.Height = 27;
            this.dgvVehicleInfo.Size = new System.Drawing.Size(844, 294);
            this.dgvVehicleInfo.TabIndex = 9;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dgvVehicleInfo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvVehicleControl, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(850, 600);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // dgvVehicleControl
            // 
            this.dgvVehicleControl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVehicleControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVehicleControl.Location = new System.Drawing.Point(3, 303);
            this.dgvVehicleControl.Name = "dgvVehicleControl";
            this.dgvVehicleControl.RowTemplate.Height = 27;
            this.dgvVehicleControl.Size = new System.Drawing.Size(844, 294);
            this.dgvVehicleControl.TabIndex = 10;
            // 
            // UcVehicle
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "UcVehicle";
            this.Size = new System.Drawing.Size(850, 600);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicleInfo)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicleControl)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.DataGridView dgvVehicleInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgvVehicleControl;
    }
}
