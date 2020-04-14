namespace TrafficControlTest.UserControl
{
	partial class UcSetting
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
			this.dgvSettings = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dgvSettings)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvSettings
			// 
			this.dgvSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvSettings.Location = new System.Drawing.Point(0, 0);
			this.dgvSettings.Name = "dgvSettings";
			this.dgvSettings.RowTemplate.Height = 27;
			this.dgvSettings.Size = new System.Drawing.Size(850, 600);
			this.dgvSettings.TabIndex = 0;
			this.dgvSettings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSettings_CellValueChanged);
			// 
			// UcSetting
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.Controls.Add(this.dgvSettings);
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "UcSetting";
			this.Size = new System.Drawing.Size(850, 600);
			((System.ComponentModel.ISupportInitialize)(this.dgvSettings)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvSettings;
	}
}
