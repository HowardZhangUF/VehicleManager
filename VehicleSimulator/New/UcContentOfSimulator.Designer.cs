namespace VehicleSimulator.New
{
	partial class UcContentOfSimulator
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
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.ucSimulatorInfo1 = new VehicleSimulator.New.UcSimulatorInfo();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMenu
            // 
            this.pnlMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(250, 420);
            this.pnlMenu.TabIndex = 2;
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
            this.pnlContent.Controls.Add(this.ucSimulatorInfo1);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(250, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(470, 420);
            this.pnlContent.TabIndex = 3;
            // 
            // ucSimulatorInfo1
            // 
            this.ucSimulatorInfo1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
            this.ucSimulatorInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSimulatorInfo1.Location = new System.Drawing.Point(0, 0);
            this.ucSimulatorInfo1.Name = "ucSimulatorInfo1";
            this.ucSimulatorInfo1.Padding = new System.Windows.Forms.Padding(5);
            this.ucSimulatorInfo1.Size = new System.Drawing.Size(470, 420);
            this.ucSimulatorInfo1.TabIndex = 0;
            // 
            // UcContentOfSimulator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlMenu);
            this.Name = "UcContentOfSimulator";
            this.Size = new System.Drawing.Size(720, 420);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        #endregion
        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Panel pnlContent;
        private UcSimulatorInfo ucSimulatorInfo1;
    }
}
