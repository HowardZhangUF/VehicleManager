namespace TrafficControlTest.UserControl
{
	partial class UcAutomaticDoor
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
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.dgvAutomaticDoorControl = new System.Windows.Forms.DataGridView();
			this.dgvAutomaticDoorInfo = new System.Windows.Forms.DataGridView();
			this.cmenuDgvAutomaticDoorInfo = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmenuItemOpenAutomaticDoor = new System.Windows.Forms.ToolStripMenuItem();
			this.cmenuItemCloseAutomaticDoor = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvAutomaticDoorControl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvAutomaticDoorInfo)).BeginInit();
			this.cmenuDgvAutomaticDoorInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.dgvAutomaticDoorControl, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.dgvAutomaticDoorInfo, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(850, 600);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// dgvAutomaticDoorControl
			// 
			this.dgvAutomaticDoorControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvAutomaticDoorControl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvAutomaticDoorControl.Location = new System.Drawing.Point(3, 303);
			this.dgvAutomaticDoorControl.Name = "dgvAutomaticDoorControl";
			this.dgvAutomaticDoorControl.RowTemplate.Height = 27;
			this.dgvAutomaticDoorControl.Size = new System.Drawing.Size(844, 294);
			this.dgvAutomaticDoorControl.TabIndex = 1;
			// 
			// dgvAutomaticDoorInfo
			// 
			this.dgvAutomaticDoorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvAutomaticDoorInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvAutomaticDoorInfo.Location = new System.Drawing.Point(3, 3);
			this.dgvAutomaticDoorInfo.Name = "dgvAutomaticDoorInfo";
			this.dgvAutomaticDoorInfo.RowTemplate.Height = 27;
			this.dgvAutomaticDoorInfo.Size = new System.Drawing.Size(844, 294);
			this.dgvAutomaticDoorInfo.TabIndex = 0;
			this.dgvAutomaticDoorInfo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvAutomaticDoorInfo_MouseDown);
			// 
			// cmenuDgvAutomaticDoorInfo
			// 
			this.cmenuDgvAutomaticDoorInfo.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.cmenuDgvAutomaticDoorInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenuItemOpenAutomaticDoor,
            this.cmenuItemCloseAutomaticDoor});
			this.cmenuDgvAutomaticDoorInfo.Name = "cmenuDgvAutomaticDoorInfo";
			this.cmenuDgvAutomaticDoorInfo.Size = new System.Drawing.Size(117, 52);
			// 
			// cmenuItemOpenAutomaticDoor
			// 
			this.cmenuItemOpenAutomaticDoor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.cmenuItemOpenAutomaticDoor.ForeColor = System.Drawing.Color.White;
			this.cmenuItemOpenAutomaticDoor.Name = "cmenuItemOpenAutomaticDoor";
			this.cmenuItemOpenAutomaticDoor.Size = new System.Drawing.Size(210, 24);
			this.cmenuItemOpenAutomaticDoor.Text = "Open";
			this.cmenuItemOpenAutomaticDoor.Click += new System.EventHandler(this.cmenuItemOpenAutomaticDoor_Click);
			// 
			// cmenuItemCloseAutomaticDoor
			// 
			this.cmenuItemCloseAutomaticDoor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.cmenuItemCloseAutomaticDoor.ForeColor = System.Drawing.Color.White;
			this.cmenuItemCloseAutomaticDoor.Name = "cmenuItemCloseAutomaticDoor";
			this.cmenuItemCloseAutomaticDoor.Size = new System.Drawing.Size(210, 24);
			this.cmenuItemCloseAutomaticDoor.Text = "Close";
			this.cmenuItemCloseAutomaticDoor.Click += new System.EventHandler(this.cmenuItemCloseAutomaticDoor_Click);
			// 
			// UcAutomaticDoor
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.Controls.Add(this.tableLayoutPanel1);
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "UcAutomaticDoor";
			this.Size = new System.Drawing.Size(850, 600);
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvAutomaticDoorControl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvAutomaticDoorInfo)).EndInit();
			this.cmenuDgvAutomaticDoorInfo.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.DataGridView dgvAutomaticDoorInfo;
		private System.Windows.Forms.DataGridView dgvAutomaticDoorControl;
		private System.Windows.Forms.ContextMenuStrip cmenuDgvAutomaticDoorInfo;
		private System.Windows.Forms.ToolStripMenuItem cmenuItemOpenAutomaticDoor;
		private System.Windows.Forms.ToolStripMenuItem cmenuItemCloseAutomaticDoor;
	}
}
