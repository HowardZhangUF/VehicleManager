namespace GLUITest
{
    partial class frmTest
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
			this.dgvInfo = new System.Windows.Forms.DataGridView();
			this.cmbSelectType = new System.Windows.Forms.ComboBox();
			this.GLUI = new GLUI.GLUICtrl();
			this.tsslConnectStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgvInfo
			// 
			this.dgvInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.dgvInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvInfo.Location = new System.Drawing.Point(16, 48);
			this.dgvInfo.Margin = new System.Windows.Forms.Padding(4);
			this.dgvInfo.Name = "dgvInfo";
			this.dgvInfo.RowTemplate.Height = 24;
			this.dgvInfo.Size = new System.Drawing.Size(669, 519);
			this.dgvInfo.TabIndex = 1;
			this.dgvInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvInfo_CellValueChanged);
			this.dgvInfo.DoubleClick += new System.EventHandler(this.DgvInfo_DoubleClick);
			// 
			// cmbSelectType
			// 
			this.cmbSelectType.FormattingEnabled = true;
			this.cmbSelectType.Location = new System.Drawing.Point(16, 15);
			this.cmbSelectType.Margin = new System.Windows.Forms.Padding(4);
			this.cmbSelectType.Name = "cmbSelectType";
			this.cmbSelectType.Size = new System.Drawing.Size(668, 23);
			this.cmbSelectType.TabIndex = 2;
			this.cmbSelectType.SelectedValueChanged += new System.EventHandler(this.CmbSelectType_SelectedValueChanged);
			// 
			// GLUI
			// 
			this.GLUI.AllowObjectMenu = true;
			this.GLUI.AllowUndoMenu = true;
			this.GLUI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.GLUI.Location = new System.Drawing.Point(693, 15);
			this.GLUI.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.GLUI.Name = "GLUI";
			this.GLUI.ShowAxis = true;
			this.GLUI.ShowGrid = true;
			this.GLUI.Size = new System.Drawing.Size(662, 552);
			this.GLUI.TabIndex = 3;
			this.GLUI.Zoom = 10D;
			this.GLUI.GLDoubleClick += new System.EventHandler(this.GLUI_GLDoubleClick);
			// 
			// tsslConnectStatus
			// 
			this.tsslConnectStatus.Image = global::GLUITest.Properties.Resources.circle_red;
			this.tsslConnectStatus.Name = "tsslConnectStatus";
			this.tsslConnectStatus.Size = new System.Drawing.Size(38, 20);
			this.tsslConnectStatus.Text = "0";
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslConnectStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 583);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1368, 25);
			this.statusStrip1.TabIndex = 4;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// frmTest
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1368, 608);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.GLUI);
			this.Controls.Add(this.cmbSelectType);
			this.Controls.Add(this.dgvInfo);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "frmTest";
			this.Text = "GLUITest";
			this.Load += new System.EventHandler(this.frmTest_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvInfo;
        private System.Windows.Forms.ComboBox cmbSelectType;
        private GLUI.GLUICtrl GLUI;
		private System.Windows.Forms.ToolStripStatusLabel tsslConnectStatus;
		private System.Windows.Forms.StatusStrip statusStrip1;
	}
}

