namespace TrafficControlTest.UserControl
{
	partial class UcSimpleLog
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
			this.dgvSimpleLog = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dgvSimpleLog)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvSimpleLog
			// 
			this.dgvSimpleLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSimpleLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvSimpleLog.Location = new System.Drawing.Point(0, 0);
			this.dgvSimpleLog.Name = "dgvSimpleLog";
			this.dgvSimpleLog.RowTemplate.Height = 27;
			this.dgvSimpleLog.Size = new System.Drawing.Size(850, 250);
			this.dgvSimpleLog.TabIndex = 0;
			// 
			// UCSimpleLog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.Controls.Add(this.dgvSimpleLog);
			this.Name = "UCSimpleLog";
			this.Size = new System.Drawing.Size(850, 250);
			((System.ComponentModel.ISupportInitialize)(this.dgvSimpleLog)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvSimpleLog;
	}
}
