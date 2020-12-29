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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.btnSimulatorStop = new System.Windows.Forms.Button();
			this.btnSimulatorMove = new System.Windows.Forms.Button();
			this.txtMoveTarget = new System.Windows.Forms.TextBox();
			this.btnSimulatorConnect = new System.Windows.Forms.Button();
			this.lblSimulatorName = new System.Windows.Forms.Label();
			this.txtHostIpPort = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.dgvSimulatorInfo)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgvSimulatorInfo
			// 
			this.dgvSimulatorInfo.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
			this.dgvSimulatorInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgvSimulatorInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.tableLayoutPanel1.SetColumnSpan(this.dgvSimulatorInfo, 2);
			this.dgvSimulatorInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvSimulatorInfo.Location = new System.Drawing.Point(0, 240);
			this.dgvSimulatorInfo.Margin = new System.Windows.Forms.Padding(0);
			this.dgvSimulatorInfo.Name = "dgvSimulatorInfo";
			this.dgvSimulatorInfo.RowTemplate.Height = 27;
			this.dgvSimulatorInfo.Size = new System.Drawing.Size(450, 160);
			this.dgvSimulatorInfo.TabIndex = 0;
			this.dgvSimulatorInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSimulatorInfo_CellValueChanged);
			this.dgvSimulatorInfo.SelectionChanged += new System.EventHandler(this.dgvSimulatorInfo_SelectionChanged);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.dgvSimulatorInfo, 0, 11);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 9);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 8);
			this.tableLayoutPanel1.Controls.Add(this.btnSimulatorStop, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.btnSimulatorMove, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.txtMoveTarget, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.btnSimulatorConnect, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblSimulatorName, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtHostIpPort, 1, 4);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 12;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(450, 400);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
			this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(3, 42);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(91, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Operation";
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Gray;
			this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 68);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(444, 1);
			this.panel1.TabIndex = 1;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.Gray;
			this.tableLayoutPanel1.SetColumnSpan(this.panel2, 2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 233);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(444, 1);
			this.panel2.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(3, 207);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(107, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "Information";
			// 
			// btnSimulatorStop
			// 
			this.btnSimulatorStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSimulatorStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSimulatorStop.ForeColor = System.Drawing.Color.White;
			this.btnSimulatorStop.Location = new System.Drawing.Point(3, 160);
			this.btnSimulatorStop.Name = "btnSimulatorStop";
			this.btnSimulatorStop.Size = new System.Drawing.Size(144, 30);
			this.btnSimulatorStop.TabIndex = 4;
			this.btnSimulatorStop.Text = "Stop";
			this.btnSimulatorStop.UseVisualStyleBackColor = true;
			this.btnSimulatorStop.Click += new System.EventHandler(this.btnSimulatorStop_Click);
			// 
			// btnSimulatorMove
			// 
			this.btnSimulatorMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSimulatorMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSimulatorMove.ForeColor = System.Drawing.Color.White;
			this.btnSimulatorMove.Location = new System.Drawing.Point(3, 120);
			this.btnSimulatorMove.Name = "btnSimulatorMove";
			this.btnSimulatorMove.Size = new System.Drawing.Size(144, 30);
			this.btnSimulatorMove.TabIndex = 5;
			this.btnSimulatorMove.Text = "Move";
			this.btnSimulatorMove.UseVisualStyleBackColor = true;
			this.btnSimulatorMove.Click += new System.EventHandler(this.btnSimulatorMove_Click);
			// 
			// txtMoveTarget
			// 
			this.txtMoveTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMoveTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
			this.txtMoveTarget.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtMoveTarget.ForeColor = System.Drawing.Color.White;
			this.txtMoveTarget.Location = new System.Drawing.Point(153, 121);
			this.txtMoveTarget.Name = "txtMoveTarget";
			this.txtMoveTarget.Size = new System.Drawing.Size(294, 28);
			this.txtMoveTarget.TabIndex = 6;
			// 
			// btnSimulatorConnect
			// 
			this.btnSimulatorConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSimulatorConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
			this.btnSimulatorConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSimulatorConnect.ForeColor = System.Drawing.Color.White;
			this.btnSimulatorConnect.Location = new System.Drawing.Point(3, 80);
			this.btnSimulatorConnect.Name = "btnSimulatorConnect";
			this.btnSimulatorConnect.Size = new System.Drawing.Size(144, 30);
			this.btnSimulatorConnect.TabIndex = 7;
			this.btnSimulatorConnect.Text = "Connect";
			this.btnSimulatorConnect.UseVisualStyleBackColor = false;
			this.btnSimulatorConnect.Click += new System.EventHandler(this.btnSimulatorConnect_Click);
			// 
			// lblSimulatorName
			// 
			this.lblSimulatorName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSimulatorName.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.lblSimulatorName, 2);
			this.lblSimulatorName.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblSimulatorName.ForeColor = System.Drawing.Color.White;
			this.lblSimulatorName.Location = new System.Drawing.Point(3, 6);
			this.lblSimulatorName.Name = "lblSimulatorName";
			this.lblSimulatorName.Size = new System.Drawing.Size(444, 28);
			this.lblSimulatorName.TabIndex = 8;
			this.lblSimulatorName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// txtHostIpPort
			// 
			this.txtHostIpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtHostIpPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
			this.txtHostIpPort.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtHostIpPort.ForeColor = System.Drawing.Color.White;
			this.txtHostIpPort.Location = new System.Drawing.Point(153, 81);
			this.txtHostIpPort.Name = "txtHostIpPort";
			this.txtHostIpPort.Size = new System.Drawing.Size(294, 28);
			this.txtHostIpPort.TabIndex = 9;
			// 
			// UcSimulatorInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "UcSimulatorInfo";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.Size = new System.Drawing.Size(470, 420);
			((System.ComponentModel.ISupportInitialize)(this.dgvSimulatorInfo)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSimulatorInfo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnSimulatorStop;
		private System.Windows.Forms.Button btnSimulatorMove;
		private System.Windows.Forms.TextBox txtMoveTarget;
		private System.Windows.Forms.Button btnSimulatorConnect;
		private System.Windows.Forms.Label lblSimulatorName;
		private System.Windows.Forms.TextBox txtHostIpPort;
	}
}
