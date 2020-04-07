namespace TrafficControlTest.UserControl
{
	partial class UcSystemOverview
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
			this.pnlConnection = new System.Windows.Forms.Panel();
			this.lblConnection = new System.Windows.Forms.Label();
			this.pnlConnectionRight = new System.Windows.Forms.Panel();
			this.pnlConnectionLeft = new System.Windows.Forms.Panel();
			this.pnlConnectionBottom = new System.Windows.Forms.Panel();
			this.pnlConnectionTop = new System.Windows.Forms.Panel();
			this.pnlConnection.SuspendLayout();
			this.SuspendLayout();
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
			this.pnlConnection.TabIndex = 1;
			// 
			// lblConnection
			// 
			this.lblConnection.BackColor = System.Drawing.Color.DarkRed;
			this.lblConnection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblConnection.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblConnection.ForeColor = System.Drawing.Color.White;
			this.lblConnection.Location = new System.Drawing.Point(5, 5);
			this.lblConnection.Name = "lblConnection";
			this.lblConnection.Size = new System.Drawing.Size(40, 40);
			this.lblConnection.TabIndex = 2;
			this.lblConnection.Text = "0";
			this.lblConnection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlConnectionRight
			// 
			this.pnlConnectionRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlConnectionRight.Location = new System.Drawing.Point(45, 5);
			this.pnlConnectionRight.Name = "pnlConnectionRight";
			this.pnlConnectionRight.Size = new System.Drawing.Size(5, 40);
			this.pnlConnectionRight.TabIndex = 1;
			// 
			// pnlConnectionLeft
			// 
			this.pnlConnectionLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlConnectionLeft.Location = new System.Drawing.Point(0, 5);
			this.pnlConnectionLeft.Name = "pnlConnectionLeft";
			this.pnlConnectionLeft.Size = new System.Drawing.Size(5, 40);
			this.pnlConnectionLeft.TabIndex = 1;
			// 
			// pnlConnectionBottom
			// 
			this.pnlConnectionBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlConnectionBottom.Location = new System.Drawing.Point(0, 45);
			this.pnlConnectionBottom.Name = "pnlConnectionBottom";
			this.pnlConnectionBottom.Size = new System.Drawing.Size(50, 5);
			this.pnlConnectionBottom.TabIndex = 1;
			// 
			// pnlConnectionTop
			// 
			this.pnlConnectionTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlConnectionTop.Location = new System.Drawing.Point(0, 0);
			this.pnlConnectionTop.Name = "pnlConnectionTop";
			this.pnlConnectionTop.Size = new System.Drawing.Size(50, 5);
			this.pnlConnectionTop.TabIndex = 1;
			// 
			// UcSystemOverview
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
			this.Controls.Add(this.pnlConnection);
			this.Name = "UcSystemOverview";
			this.Size = new System.Drawing.Size(1300, 50);
			this.pnlConnection.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlConnection;
		private System.Windows.Forms.Label lblConnection;
		private System.Windows.Forms.Panel pnlConnectionRight;
		private System.Windows.Forms.Panel pnlConnectionLeft;
		private System.Windows.Forms.Panel pnlConnectionBottom;
		private System.Windows.Forms.Panel pnlConnectionTop;
	}
}
