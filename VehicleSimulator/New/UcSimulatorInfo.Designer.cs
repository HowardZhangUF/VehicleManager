namespace VehicleSimulator.New
{
    partial class UcSimulatorInfo
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
            this.dgvSimulatorInfo = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSimulatorInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSimulatorInfo
            // 
            this.dgvSimulatorInfo.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
            this.dgvSimulatorInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSimulatorInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSimulatorInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSimulatorInfo.Location = new System.Drawing.Point(10, 10);
            this.dgvSimulatorInfo.Name = "dgvSimulatorInfo";
            this.dgvSimulatorInfo.RowTemplate.Height = 27;
            this.dgvSimulatorInfo.Size = new System.Drawing.Size(450, 400);
            this.dgvSimulatorInfo.TabIndex = 0;
            this.dgvSimulatorInfo.SelectionChanged += new System.EventHandler(this.dgvSimulatorInfo_SelectionChanged);
            // 
            // UcSimulatorInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
            this.Controls.Add(this.dgvSimulatorInfo);
            this.Name = "UcSimulatorInfo";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(470, 420);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSimulatorInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSimulatorInfo;
    }
}
