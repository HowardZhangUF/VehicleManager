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
			this.pnlSubMenu = new System.Windows.Forms.Panel();
			this.btnRemoveSimulator = new System.Windows.Forms.Button();
			this.btnAddSimulator = new System.Windows.Forms.Button();
			this.pnlContent = new System.Windows.Forms.Panel();
			this.pnlSubMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlMenu
			// 
			this.pnlMenu.AutoScroll = true;
			this.pnlMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
			this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMenu.Location = new System.Drawing.Point(0, 0);
			this.pnlMenu.Name = "pnlMenu";
			this.pnlMenu.Size = new System.Drawing.Size(250, 390);
			this.pnlMenu.TabIndex = 2;
			// 
			// pnlSubMenu
			// 
			this.pnlSubMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
			this.pnlSubMenu.Controls.Add(this.btnRemoveSimulator);
			this.pnlSubMenu.Controls.Add(this.btnAddSimulator);
			this.pnlSubMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlSubMenu.Location = new System.Drawing.Point(0, 390);
			this.pnlSubMenu.Name = "pnlSubMenu";
			this.pnlSubMenu.Size = new System.Drawing.Size(250, 30);
			this.pnlSubMenu.TabIndex = 0;
			// 
			// btnRemoveSimulator
			// 
			this.btnRemoveSimulator.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnRemoveSimulator.FlatAppearance.BorderSize = 0;
			this.btnRemoveSimulator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnRemoveSimulator.Image = global::VehicleSimulator.Properties.Resources.icons8_trash_24px;
			this.btnRemoveSimulator.Location = new System.Drawing.Point(30, 0);
			this.btnRemoveSimulator.Name = "btnRemoveSimulator";
			this.btnRemoveSimulator.Size = new System.Drawing.Size(30, 30);
			this.btnRemoveSimulator.TabIndex = 1;
			this.btnRemoveSimulator.UseVisualStyleBackColor = true;
			this.btnRemoveSimulator.Click += new System.EventHandler(this.btnRemoveSimulator_Click);
			// 
			// btnAddSimulator
			// 
			this.btnAddSimulator.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnAddSimulator.FlatAppearance.BorderSize = 0;
			this.btnAddSimulator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAddSimulator.Image = global::VehicleSimulator.Properties.Resources.icons8_plus_math_24px;
			this.btnAddSimulator.Location = new System.Drawing.Point(0, 0);
			this.btnAddSimulator.Name = "btnAddSimulator";
			this.btnAddSimulator.Size = new System.Drawing.Size(30, 30);
			this.btnAddSimulator.TabIndex = 0;
			this.btnAddSimulator.UseVisualStyleBackColor = true;
			this.btnAddSimulator.Click += new System.EventHandler(this.btnAddSimulator_Click);
			// 
			// pnlContent
			// 
			this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
			this.pnlContent.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlContent.Location = new System.Drawing.Point(250, 0);
			this.pnlContent.Name = "pnlContent";
			this.pnlContent.Size = new System.Drawing.Size(470, 420);
			this.pnlContent.TabIndex = 0;
			// 
			// UcContentOfSimulator
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
			this.Controls.Add(this.pnlMenu);
			this.Controls.Add(this.pnlSubMenu);
			this.Controls.Add(this.pnlContent);
			this.Name = "UcContentOfSimulator";
			this.Size = new System.Drawing.Size(720, 420);
			this.pnlSubMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

        #endregion
        private System.Windows.Forms.Panel pnlMenu;
		private System.Windows.Forms.Panel pnlSubMenu;
		private System.Windows.Forms.Button btnAddSimulator;
		private System.Windows.Forms.Button btnRemoveSimulator;
		private System.Windows.Forms.Panel pnlContent;
	}
}
