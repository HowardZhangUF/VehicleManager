namespace VehicleSimulator
{
	partial class UcContentOfConsole
	{
		/// <summary> 
		/// 設計工具所需的變數。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清除任何使用中的資源。
		/// </summary>
		/// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 元件設計工具產生的程式碼

		/// <summary> 
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent() {
			this.dgvConsole = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dgvConsole)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvConsole
			// 
			this.dgvConsole.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
			this.dgvConsole.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvConsole.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvConsole.Location = new System.Drawing.Point(0, 0);
			this.dgvConsole.Name = "dgvConsole";
			this.dgvConsole.RowTemplate.Height = 27;
			this.dgvConsole.Size = new System.Drawing.Size(720, 420);
			this.dgvConsole.TabIndex = 0;
			// 
			// UcContentOfConsole
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
			this.Controls.Add(this.dgvConsole);
			this.Name = "UcContentOfConsole";
			this.Size = new System.Drawing.Size(720, 420);
			((System.ComponentModel.ISupportInitialize)(this.dgvConsole)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvConsole;
	}
}
