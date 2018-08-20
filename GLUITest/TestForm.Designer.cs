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
			this.tsslConnectStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnChangeMap = new System.Windows.Forms.Button();
			this.btnUploadMapToAGV = new System.Windows.Forms.Button();
			this.btnRequestMapList = new System.Windows.Forms.Button();
			this.dgvAGVInfo = new System.Windows.Forms.DataGridView();
			this.cbAGVList = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.GLUI = new GLUI.GLUICtrl();
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
			this.dgvInfo.Location = new System.Drawing.Point(7, 53);
			this.dgvInfo.Margin = new System.Windows.Forms.Padding(4);
			this.dgvInfo.Name = "dgvInfo";
			this.dgvInfo.RowTemplate.Height = 24;
			this.dgvInfo.Size = new System.Drawing.Size(791, 493);
			this.dgvInfo.TabIndex = 1;
			this.dgvInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvInfo_CellValueChanged);
			this.dgvInfo.DoubleClick += new System.EventHandler(this.DgvInfo_DoubleClick);
			// 
			// cmbSelectType
			// 
			this.cmbSelectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSelectType.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cmbSelectType.FormattingEnabled = true;
			this.cmbSelectType.Location = new System.Drawing.Point(7, 7);
			this.cmbSelectType.Margin = new System.Windows.Forms.Padding(4);
			this.cmbSelectType.Name = "cmbSelectType";
			this.cmbSelectType.Size = new System.Drawing.Size(791, 38);
			this.cmbSelectType.TabIndex = 2;
			this.cmbSelectType.SelectedValueChanged += new System.EventHandler(this.CmbSelectType_SelectedValueChanged);
			// 
			// tsslConnectStatus
			// 
			this.tsslConnectStatus.Image = global::GLUITest.Properties.Resources.CircleRed;
			this.tsslConnectStatus.Name = "tsslConnectStatus";
			this.tsslConnectStatus.Size = new System.Drawing.Size(38, 20);
			this.tsslConnectStatus.Text = "0";
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslConnectStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 616);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1368, 25);
			this.statusStrip1.TabIndex = 4;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.ItemSize = new System.Drawing.Size(40, 40);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(863, 601);
			this.tabControl1.TabIndex = 5;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.btnChangeMap);
			this.tabPage1.Controls.Add(this.btnUploadMapToAGV);
			this.tabPage1.Controls.Add(this.btnRequestMapList);
			this.tabPage1.Controls.Add(this.dgvAGVInfo);
			this.tabPage1.Controls.Add(this.cbAGVList);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Location = new System.Drawing.Point(4, 44);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(855, 553);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "AGVs";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label2.Location = new System.Drawing.Point(433, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(153, 20);
			this.label2.TabIndex = 6;
			this.label2.Text = "Map Synchronize";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.Location = new System.Drawing.Point(129, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(135, 20);
			this.label1.TabIndex = 5;
			this.label1.Text = "AGVs Monitor";
			// 
			// btnChangeMap
			// 
			this.btnChangeMap.Location = new System.Drawing.Point(408, 163);
			this.btnChangeMap.Name = "btnChangeMap";
			this.btnChangeMap.Size = new System.Drawing.Size(202, 53);
			this.btnChangeMap.TabIndex = 4;
			this.btnChangeMap.Text = "Change Map";
			this.btnChangeMap.UseVisualStyleBackColor = true;
			this.btnChangeMap.Click += new System.EventHandler(this.btnChangeMap_Click);
			// 
			// btnUploadMapToAGV
			// 
			this.btnUploadMapToAGV.Location = new System.Drawing.Point(408, 104);
			this.btnUploadMapToAGV.Name = "btnUploadMapToAGV";
			this.btnUploadMapToAGV.Size = new System.Drawing.Size(202, 53);
			this.btnUploadMapToAGV.TabIndex = 3;
			this.btnUploadMapToAGV.Text = "Upload Map To AGV";
			this.btnUploadMapToAGV.UseVisualStyleBackColor = true;
			this.btnUploadMapToAGV.Click += new System.EventHandler(this.btnUploadMapToAGV_Click);
			// 
			// btnRequestMapList
			// 
			this.btnRequestMapList.Location = new System.Drawing.Point(408, 45);
			this.btnRequestMapList.Name = "btnRequestMapList";
			this.btnRequestMapList.Size = new System.Drawing.Size(202, 53);
			this.btnRequestMapList.TabIndex = 2;
			this.btnRequestMapList.Text = "Request Map";
			this.btnRequestMapList.UseVisualStyleBackColor = true;
			this.btnRequestMapList.Click += new System.EventHandler(this.btnRequestMapList_Click);
			// 
			// dgvAGVInfo
			// 
			this.dgvAGVInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvAGVInfo.Location = new System.Drawing.Point(45, 89);
			this.dgvAGVInfo.Name = "dgvAGVInfo";
			this.dgvAGVInfo.RowTemplate.Height = 27;
			this.dgvAGVInfo.Size = new System.Drawing.Size(303, 246);
			this.dgvAGVInfo.TabIndex = 1;
			this.dgvAGVInfo.SelectionChanged += new System.EventHandler(this.dgvAGVInfo_SelectionChanged);
			// 
			// cbAGVList
			// 
			this.cbAGVList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbAGVList.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbAGVList.FormattingEnabled = true;
			this.cbAGVList.Location = new System.Drawing.Point(45, 45);
			this.cbAGVList.Name = "cbAGVList";
			this.cbAGVList.Size = new System.Drawing.Size(303, 38);
			this.cbAGVList.TabIndex = 0;
			this.cbAGVList.SelectedIndexChanged += new System.EventHandler(this.cbAGVList_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label3.Location = new System.Drawing.Point(393, 30);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(232, 201);
			this.label3.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label4.Location = new System.Drawing.Point(30, 30);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(333, 320);
			this.label4.TabIndex = 8;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.dgvInfo);
			this.tabPage2.Controls.Add(this.cmbSelectType);
			this.tabPage2.Location = new System.Drawing.Point(4, 44);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(805, 553);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Map Objects";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tabPage3
			// 
			this.tabPage3.Location = new System.Drawing.Point(4, 44);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(805, 553);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Dispatch System";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// GLUI
			// 
			this.GLUI.AllowObjectMenu = true;
			this.GLUI.AllowUndoMenu = true;
			this.GLUI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.GLUI.ContextMenuStripMode = true;
			this.GLUI.Location = new System.Drawing.Point(882, 24);
			this.GLUI.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.GLUI.Name = "GLUI";
			this.GLUI.ShowAxis = true;
			this.GLUI.ShowGrid = true;
			this.GLUI.Size = new System.Drawing.Size(473, 585);
			this.GLUI.TabIndex = 6;
			this.GLUI.Zoom = 10D;
			// 
			// frmTest
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1368, 641);
			this.Controls.Add(this.GLUI);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.statusStrip1);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "frmTest";
			this.Text = "GLUITest";
			this.Load += new System.EventHandler(this.frmTest_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvAGVInfo)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvInfo;
        private System.Windows.Forms.ComboBox cmbSelectType;
		private System.Windows.Forms.ToolStripStatusLabel tsslConnectStatus;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.DataGridView dgvAGVInfo;
		private System.Windows.Forms.ComboBox cbAGVList;
		private System.Windows.Forms.Button btnRequestMapList;
		private System.Windows.Forms.Button btnUploadMapToAGV;
		private System.Windows.Forms.Button btnChangeMap;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private GLUI.GLUICtrl GLUI;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
	}
}

