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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.dgvAGVInfo = new System.Windows.Forms.DataGridView();
			this.cbAGVList = new System.Windows.Forms.ComboBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvAGVInfo)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgvInfo
			// 
			this.dgvInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.dgvInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvInfo.Location = new System.Drawing.Point(7, 38);
			this.dgvInfo.Margin = new System.Windows.Forms.Padding(4);
			this.dgvInfo.Name = "dgvInfo";
			this.dgvInfo.RowTemplate.Height = 24;
			this.dgvInfo.Size = new System.Drawing.Size(650, 475);
			this.dgvInfo.TabIndex = 1;
			this.dgvInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvInfo_CellValueChanged);
			this.dgvInfo.DoubleClick += new System.EventHandler(this.DgvInfo_DoubleClick);
			// 
			// cmbSelectType
			// 
			this.cmbSelectType.FormattingEnabled = true;
			this.cmbSelectType.Location = new System.Drawing.Point(7, 7);
			this.cmbSelectType.Margin = new System.Windows.Forms.Padding(4);
			this.cmbSelectType.Name = "cmbSelectType";
			this.cmbSelectType.Size = new System.Drawing.Size(650, 23);
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
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.ItemSize = new System.Drawing.Size(40, 40);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(672, 568);
			this.tabControl1.TabIndex = 5;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.dgvAGVInfo);
			this.tabPage1.Controls.Add(this.cbAGVList);
			this.tabPage1.Location = new System.Drawing.Point(4, 44);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(664, 520);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "AGVs";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// dgvAGVInfo
			// 
			this.dgvAGVInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvAGVInfo.Location = new System.Drawing.Point(6, 50);
			this.dgvAGVInfo.Name = "dgvAGVInfo";
			this.dgvAGVInfo.RowTemplate.Height = 27;
			this.dgvAGVInfo.Size = new System.Drawing.Size(303, 311);
			this.dgvAGVInfo.TabIndex = 1;
			this.dgvAGVInfo.SelectionChanged += new System.EventHandler(this.dgvAGVInfo_SelectionChanged);
			// 
			// cbAGVList
			// 
			this.cbAGVList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbAGVList.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbAGVList.FormattingEnabled = true;
			this.cbAGVList.Location = new System.Drawing.Point(6, 6);
			this.cbAGVList.Name = "cbAGVList";
			this.cbAGVList.Size = new System.Drawing.Size(303, 38);
			this.cbAGVList.TabIndex = 0;
			this.cbAGVList.SelectedIndexChanged += new System.EventHandler(this.cbAGVList_SelectedIndexChanged);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.dgvInfo);
			this.tabPage2.Controls.Add(this.cmbSelectType);
			this.tabPage2.Location = new System.Drawing.Point(4, 44);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(664, 520);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Objects";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// frmTest
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1368, 608);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.GLUI);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "frmTest";
			this.Text = "GLUITest";
			this.Load += new System.EventHandler(this.frmTest_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvAGVInfo)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvInfo;
        private System.Windows.Forms.ComboBox cmbSelectType;
        private GLUI.GLUICtrl GLUI;
		private System.Windows.Forms.ToolStripStatusLabel tsslConnectStatus;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.DataGridView dgvAGVInfo;
		private System.Windows.Forms.ComboBox cbAGVList;
	}
}

