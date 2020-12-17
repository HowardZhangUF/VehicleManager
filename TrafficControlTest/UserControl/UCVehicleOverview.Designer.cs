namespace TrafficControlTest.UserControl
{
	partial class UcVehicleOverview
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
            this.lblTitle1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle2 = new System.Windows.Forms.Label();
            this.pnlVehicleInfo = new System.Windows.Forms.Panel();
            this.pnlMissionInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle1
            // 
            this.lblTitle1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle1.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTitle1.ForeColor = System.Drawing.Color.White;
            this.lblTitle1.Location = new System.Drawing.Point(3, 0);
            this.lblTitle1.Name = "lblTitle1";
            this.lblTitle1.Size = new System.Drawing.Size(394, 60);
            this.lblTitle1.TabIndex = 3;
            this.lblTitle1.Text = "    Vehicle Overview";
            this.lblTitle1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pnlMissionInfo, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTitle1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTitle2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlVehicleInfo, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 650);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // lblTitle2
            // 
            this.lblTitle2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle2.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTitle2.ForeColor = System.Drawing.Color.White;
            this.lblTitle2.Location = new System.Drawing.Point(3, 325);
            this.lblTitle2.Name = "lblTitle2";
            this.lblTitle2.Size = new System.Drawing.Size(394, 60);
            this.lblTitle2.TabIndex = 4;
            this.lblTitle2.Text = "    Mission Overview";
            this.lblTitle2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlVehicleInfo
            // 
            this.pnlVehicleInfo.AutoScroll = true;
            this.pnlVehicleInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVehicleInfo.Location = new System.Drawing.Point(3, 63);
            this.pnlVehicleInfo.Name = "pnlVehicleInfo";
            this.pnlVehicleInfo.Size = new System.Drawing.Size(394, 259);
            this.pnlVehicleInfo.TabIndex = 5;
            // 
            // pnlMissionInfo
            // 
            this.pnlMissionInfo.AutoScroll = true;
            this.pnlMissionInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMissionInfo.Location = new System.Drawing.Point(3, 388);
            this.pnlMissionInfo.Name = "pnlMissionInfo";
            this.pnlMissionInfo.Size = new System.Drawing.Size(394, 259);
            this.pnlMissionInfo.TabIndex = 6;
            // 
            // UcVehicleOverview
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "UcVehicleOverview";
            this.Size = new System.Drawing.Size(400, 650);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblTitle1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblTitle2;
        private System.Windows.Forms.Panel pnlMissionInfo;
        private System.Windows.Forms.Panel pnlVehicleInfo;
    }
}
