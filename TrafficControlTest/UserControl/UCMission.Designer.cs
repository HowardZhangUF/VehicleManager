namespace TrafficControlTest.UserControl
{
	partial class UcMission
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
            this.components = new System.ComponentModel.Container();
            this.dgvMission = new System.Windows.Forms.DataGridView();
            this.cmenuDgvMission = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmenuItemRemoveMission = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMission)).BeginInit();
            this.cmenuDgvMission.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvMission
            // 
            this.dgvMission.AllowUserToAddRows = false;
            this.dgvMission.AllowUserToDeleteRows = false;
            this.dgvMission.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMission.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMission.Location = new System.Drawing.Point(0, 0);
            this.dgvMission.Name = "dgvMission";
            this.dgvMission.ReadOnly = true;
            this.dgvMission.RowHeadersWidth = 51;
            this.dgvMission.RowTemplate.Height = 27;
            this.dgvMission.Size = new System.Drawing.Size(850, 600);
            this.dgvMission.TabIndex = 0;
            this.dgvMission.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvMission_MouseDown);
            // 
            // cmenuDgvMission
            // 
            this.cmenuDgvMission.BackColor = System.Drawing.SystemColors.Control;
            this.cmenuDgvMission.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmenuDgvMission.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenuItemRemoveMission});
            this.cmenuDgvMission.Name = "cmenuDgvMission";
            this.cmenuDgvMission.Size = new System.Drawing.Size(211, 56);
            // 
            // cmenuItemRemoveMission
            // 
            this.cmenuItemRemoveMission.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.cmenuItemRemoveMission.ForeColor = System.Drawing.Color.White;
            this.cmenuItemRemoveMission.Name = "cmenuItemRemoveMission";
            this.cmenuItemRemoveMission.Size = new System.Drawing.Size(210, 24);
            this.cmenuItemRemoveMission.Text = "Remove Mission";
            this.cmenuItemRemoveMission.Click += new System.EventHandler(this.cmenuItemRemoveMission_Click);
            // 
            // UcMission
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
            this.Controls.Add(this.dgvMission);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "UcMission";
            this.Size = new System.Drawing.Size(850, 600);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMission)).EndInit();
            this.cmenuDgvMission.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvMission;
		private System.Windows.Forms.ContextMenuStrip cmenuDgvMission;
		private System.Windows.Forms.ToolStripMenuItem cmenuItemRemoveMission;
	}
}
