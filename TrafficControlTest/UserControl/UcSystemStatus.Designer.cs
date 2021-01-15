namespace TrafficControlTest.UserControl
{
	partial class UcSystemStatus
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
			this.btnLockPanel = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.btnExportLog = new System.Windows.Forms.Button();
			this.tlpSystem = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel2.SuspendLayout();
			this.tlpSystem.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnLockPanel
			// 
			this.btnLockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnLockPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLockPanel.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnLockPanel.Location = new System.Drawing.Point(3, 3);
			this.btnLockPanel.Name = "btnLockPanel";
			this.btnLockPanel.Size = new System.Drawing.Size(194, 44);
			this.btnLockPanel.TabIndex = 22;
			this.btnLockPanel.Text = "Locked";
			this.btnLockPanel.UseVisualStyleBackColor = true;
			this.btnLockPanel.Click += new System.EventHandler(this.btnLockPanel_Click);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoScroll = true;
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.btnLockPanel, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.btnExportLog, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 5);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(819, 50);
			this.tableLayoutPanel2.TabIndex = 24;
			// 
			// btnExportLog
			// 
			this.btnExportLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnExportLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnExportLog.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnExportLog.Location = new System.Drawing.Point(203, 3);
			this.btnExportLog.Name = "btnExportLog";
			this.btnExportLog.Size = new System.Drawing.Size(194, 44);
			this.btnExportLog.TabIndex = 23;
			this.btnExportLog.Text = "Export Log";
			this.btnExportLog.UseVisualStyleBackColor = true;
			this.btnExportLog.Click += new System.EventHandler(this.btnExportLog_Click);
			// 
			// tlpSystem
			// 
			this.tlpSystem.AutoScroll = true;
			this.tlpSystem.ColumnCount = 2;
			this.tlpSystem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 450F));
			this.tlpSystem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpSystem.Controls.Add(this.panel1, 0, 1);
			this.tlpSystem.Controls.Add(this.label1, 0, 0);
			this.tlpSystem.Dock = System.Windows.Forms.DockStyle.Top;
			this.tlpSystem.Location = new System.Drawing.Point(5, 55);
			this.tlpSystem.Name = "tlpSystem";
			this.tlpSystem.RowCount = 3;
			this.tlpSystem.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tlpSystem.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
			this.tlpSystem.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpSystem.Size = new System.Drawing.Size(819, 55);
			this.tlpSystem.TabIndex = 25;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.tlpSystem.SetColumnSpan(this.panel1, 2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 53);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(813, 1);
			this.panel1.TabIndex = 0;
			// 
			// label1
			// 
			this.tlpSystem.SetColumnSpan(this.label1, 2);
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("新細明體", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(813, 50);
			this.label1.TabIndex = 1;
			this.label1.Text = "System";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// UcSystemStatus
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.AutoSize = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.Controls.Add(this.tlpSystem);
			this.Controls.Add(this.tableLayoutPanel2);
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "UcSystemStatus";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Size = new System.Drawing.Size(829, 579);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tlpSystem.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button btnLockPanel;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button btnExportLog;
		private System.Windows.Forms.TableLayoutPanel tlpSystem;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
	}
}
