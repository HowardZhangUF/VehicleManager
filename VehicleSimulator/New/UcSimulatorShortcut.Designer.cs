namespace VehicleSimulator.New
{
    partial class UcSimulatorShortcut
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblSimulatorName = new System.Windows.Forms.Label();
			this.lblSimulatorLocation = new System.Windows.Forms.Label();
			this.lblSimulatorStatus = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
			this.tableLayoutPanel1.Controls.Add(this.lblSimulatorName, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblSimulatorLocation, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblSimulatorStatus, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(230, 70);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lblSimulatorName
			// 
			this.lblSimulatorName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSimulatorName.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.lblSimulatorName, 2);
			this.lblSimulatorName.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblSimulatorName.ForeColor = System.Drawing.Color.White;
			this.lblSimulatorName.Location = new System.Drawing.Point(3, 7);
			this.lblSimulatorName.Name = "lblSimulatorName";
			this.lblSimulatorName.Size = new System.Drawing.Size(143, 28);
			this.lblSimulatorName.TabIndex = 0;
			this.lblSimulatorName.Text = "Vehicle001";
			// 
			// lblSimulatorLocation
			// 
			this.lblSimulatorLocation.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblSimulatorLocation.AutoSize = true;
			this.lblSimulatorLocation.ForeColor = System.Drawing.Color.White;
			this.lblSimulatorLocation.Location = new System.Drawing.Point(125, 48);
			this.lblSimulatorLocation.Name = "lblSimulatorLocation";
			this.lblSimulatorLocation.Size = new System.Drawing.Size(102, 15);
			this.lblSimulatorLocation.TabIndex = 2;
			this.lblSimulatorLocation.Text = "(5555,23121,58)";
			// 
			// lblSimulatorStatus
			// 
			this.lblSimulatorStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSimulatorStatus.AutoSize = true;
			this.lblSimulatorStatus.ForeColor = System.Drawing.Color.White;
			this.lblSimulatorStatus.Location = new System.Drawing.Point(3, 48);
			this.lblSimulatorStatus.Name = "lblSimulatorStatus";
			this.lblSimulatorStatus.Size = new System.Drawing.Size(29, 15);
			this.lblSimulatorStatus.TabIndex = 1;
			this.lblSimulatorStatus.Text = "Idle";
			// 
			// UcSimulatorShortcut
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "UcSimulatorShortcut";
			this.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			this.Size = new System.Drawing.Size(250, 80);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblSimulatorName;
        private System.Windows.Forms.Label lblSimulatorLocation;
        private System.Windows.Forms.Label lblSimulatorStatus;
    }
}
