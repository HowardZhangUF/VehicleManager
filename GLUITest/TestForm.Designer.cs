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
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabelLogIn = new System.Windows.Forms.ToolStripStatusLabel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.lblMapName = new System.Windows.Forms.Label();
			this.lblMapHash = new System.Windows.Forms.Label();
			this.lblMapLastEditTime = new System.Windows.Forms.Label();
			this.btnClearMap = new System.Windows.Forms.Button();
			this.btnLoadMap = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnChangeMap = new System.Windows.Forms.Button();
			this.btnUploadMapToAGV = new System.Windows.Forms.Button();
			this.btnRequestMapList = new System.Windows.Forms.Button();
			this.dgvAGVInfo = new System.Windows.Forms.DataGridView();
			this.cbAGVList = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.GLUI = new GLUI.GLUICtrl();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItemLogIn = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvAGVInfo)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgvInfo
			// 
			this.dgvInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvInfo.Location = new System.Drawing.Point(7, 53);
			this.dgvInfo.Margin = new System.Windows.Forms.Padding(4);
			this.dgvInfo.Name = "dgvInfo";
			this.dgvInfo.RowTemplate.Height = 24;
			this.dgvInfo.Size = new System.Drawing.Size(841, 493);
			this.dgvInfo.TabIndex = 1;
			this.dgvInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvInfo_CellValueChanged);
			this.dgvInfo.DoubleClick += new System.EventHandler(this.DgvInfo_DoubleClick);
			// 
			// cmbSelectType
			// 
			this.cmbSelectType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbSelectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSelectType.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cmbSelectType.FormattingEnabled = true;
			this.cmbSelectType.Location = new System.Drawing.Point(7, 7);
			this.cmbSelectType.Margin = new System.Windows.Forms.Padding(4);
			this.cmbSelectType.Name = "cmbSelectType";
			this.cmbSelectType.Size = new System.Drawing.Size(841, 38);
			this.cmbSelectType.TabIndex = 2;
			this.cmbSelectType.SelectedValueChanged += new System.EventHandler(this.CmbSelectType_SelectedValueChanged);
			// 
			// tsslConnectStatus
			// 
			this.tsslConnectStatus.Image = global::GLUITest.Properties.Resources.CircleRed;
			this.tsslConnectStatus.Name = "tsslConnectStatus";
			this.tsslConnectStatus.Size = new System.Drawing.Size(38, 23);
			this.tsslConnectStatus.Text = "0";
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslConnectStatus,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelLogIn});
			this.statusStrip1.Location = new System.Drawing.Point(0, 627);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1368, 28);
			this.statusStrip1.TabIndex = 4;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(1258, 23);
			this.toolStripStatusLabel1.Spring = true;
			// 
			// toolStripStatusLabelLogIn
			// 
			this.toolStripStatusLabelLogIn.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.toolStripStatusLabelLogIn.Name = "toolStripStatusLabelLogIn";
			this.toolStripStatusLabelLogIn.Size = new System.Drawing.Size(57, 23);
			this.toolStripStatusLabelLogIn.Text = "Log In";
			this.toolStripStatusLabelLogIn.Click += new System.EventHandler(this.toolStripStatusLabelLogIn_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.ItemSize = new System.Drawing.Size(40, 40);
			this.tabControl1.Location = new System.Drawing.Point(12, 26);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(863, 601);
			this.tabControl1.TabIndex = 5;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.lblMapName);
			this.tabPage1.Controls.Add(this.lblMapHash);
			this.tabPage1.Controls.Add(this.lblMapLastEditTime);
			this.tabPage1.Controls.Add(this.btnClearMap);
			this.tabPage1.Controls.Add(this.btnLoadMap);
			this.tabPage1.Controls.Add(this.label8);
			this.tabPage1.Controls.Add(this.label7);
			this.tabPage1.Controls.Add(this.label6);
			this.tabPage1.Controls.Add(this.label5);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.btnChangeMap);
			this.tabPage1.Controls.Add(this.btnUploadMapToAGV);
			this.tabPage1.Controls.Add(this.btnRequestMapList);
			this.tabPage1.Controls.Add(this.dgvAGVInfo);
			this.tabPage1.Controls.Add(this.cbAGVList);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.label9);
			this.tabPage1.Location = new System.Drawing.Point(4, 44);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(855, 553);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "AGVs";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// lblMapName
			// 
			this.lblMapName.AutoSize = true;
			this.lblMapName.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblMapName.Location = new System.Drawing.Point(496, 65);
			this.lblMapName.Name = "lblMapName";
			this.lblMapName.Size = new System.Drawing.Size(27, 15);
			this.lblMapName.TabIndex = 18;
			this.lblMapName.Text = "----";
			// 
			// lblMapHash
			// 
			this.lblMapHash.AutoSize = true;
			this.lblMapHash.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblMapHash.Location = new System.Drawing.Point(496, 95);
			this.lblMapHash.Name = "lblMapHash";
			this.lblMapHash.Size = new System.Drawing.Size(27, 15);
			this.lblMapHash.TabIndex = 17;
			this.lblMapHash.Text = "----";
			// 
			// lblMapLastEditTime
			// 
			this.lblMapLastEditTime.AutoSize = true;
			this.lblMapLastEditTime.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblMapLastEditTime.Location = new System.Drawing.Point(496, 125);
			this.lblMapLastEditTime.Name = "lblMapLastEditTime";
			this.lblMapLastEditTime.Size = new System.Drawing.Size(27, 15);
			this.lblMapLastEditTime.TabIndex = 16;
			this.lblMapLastEditTime.Text = "----";
			// 
			// btnClearMap
			// 
			this.btnClearMap.Location = new System.Drawing.Point(601, 164);
			this.btnClearMap.Name = "btnClearMap";
			this.btnClearMap.Size = new System.Drawing.Size(202, 53);
			this.btnClearMap.TabIndex = 14;
			this.btnClearMap.Text = "Clear Map";
			this.btnClearMap.UseVisualStyleBackColor = true;
			this.btnClearMap.Click += new System.EventHandler(this.btnClearMap_Click);
			// 
			// btnLoadMap
			// 
			this.btnLoadMap.Location = new System.Drawing.Point(393, 164);
			this.btnLoadMap.Name = "btnLoadMap";
			this.btnLoadMap.Size = new System.Drawing.Size(202, 53);
			this.btnLoadMap.TabIndex = 13;
			this.btnLoadMap.Text = "Load Map";
			this.btnLoadMap.UseVisualStyleBackColor = true;
			this.btnLoadMap.Click += new System.EventHandler(this.btnLoadMap_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label8.Location = new System.Drawing.Point(393, 20);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(150, 20);
			this.label8.TabIndex = 12;
			this.label8.Text = "Map Information";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label7.Location = new System.Drawing.Point(393, 125);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(97, 15);
			this.label7.TabIndex = 11;
			this.label7.Text = "Last Edit Time:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label6.Location = new System.Drawing.Point(393, 95);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(39, 15);
			this.label6.TabIndex = 10;
			this.label6.Text = "Hash:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label5.Location = new System.Drawing.Point(393, 65);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(44, 15);
			this.label5.TabIndex = 9;
			this.label5.Text = "Name:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.Location = new System.Drawing.Point(45, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(135, 20);
			this.label1.TabIndex = 5;
			this.label1.Text = "AGVs Monitor";
			// 
			// btnChangeMap
			// 
			this.btnChangeMap.Location = new System.Drawing.Point(601, 282);
			this.btnChangeMap.Name = "btnChangeMap";
			this.btnChangeMap.Size = new System.Drawing.Size(202, 53);
			this.btnChangeMap.TabIndex = 4;
			this.btnChangeMap.Text = "Change Map";
			this.btnChangeMap.UseVisualStyleBackColor = true;
			this.btnChangeMap.Click += new System.EventHandler(this.btnChangeMap_Click);
			// 
			// btnUploadMapToAGV
			// 
			this.btnUploadMapToAGV.Location = new System.Drawing.Point(601, 223);
			this.btnUploadMapToAGV.Name = "btnUploadMapToAGV";
			this.btnUploadMapToAGV.Size = new System.Drawing.Size(202, 53);
			this.btnUploadMapToAGV.TabIndex = 3;
			this.btnUploadMapToAGV.Text = "Upload Map To AGV";
			this.btnUploadMapToAGV.UseVisualStyleBackColor = true;
			this.btnUploadMapToAGV.Click += new System.EventHandler(this.btnUploadMapToAGV_Click);
			// 
			// btnRequestMapList
			// 
			this.btnRequestMapList.Location = new System.Drawing.Point(393, 223);
			this.btnRequestMapList.Name = "btnRequestMapList";
			this.btnRequestMapList.Size = new System.Drawing.Size(202, 112);
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
			// label4
			// 
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label4.Location = new System.Drawing.Point(30, 30);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(333, 320);
			this.label4.TabIndex = 8;
			// 
			// label9
			// 
			this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.label9.Location = new System.Drawing.Point(378, 30);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(440, 320);
			this.label9.TabIndex = 15;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.dgvInfo);
			this.tabPage2.Controls.Add(this.cmbSelectType);
			this.tabPage2.Location = new System.Drawing.Point(4, 44);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(855, 553);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Map Objects";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tabPage3
			// 
			this.tabPage3.Location = new System.Drawing.Point(4, 44);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(855, 553);
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
			this.GLUI.Location = new System.Drawing.Point(882, 38);
			this.GLUI.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.GLUI.Name = "GLUI";
			this.GLUI.ShowAxis = true;
			this.GLUI.ShowGrid = true;
			this.GLUI.Size = new System.Drawing.Size(473, 585);
			this.GLUI.TabIndex = 6;
			this.GLUI.Zoom = 10D;
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemLogIn});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1368, 28);
			this.menuStrip1.TabIndex = 7;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// toolStripMenuItemLogIn
			// 
			this.toolStripMenuItemLogIn.Name = "toolStripMenuItemLogIn";
			this.toolStripMenuItemLogIn.Size = new System.Drawing.Size(65, 24);
			this.toolStripMenuItemLogIn.Text = "Log In";
			this.toolStripMenuItemLogIn.Click += new System.EventHandler(this.logInToolStripMenuItem_Click);
			// 
			// frmTest
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1368, 655);
			this.Controls.Add(this.GLUI);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
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
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
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
		private GLUI.GLUICtrl GLUI;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button btnClearMap;
		private System.Windows.Forms.Button btnLoadMap;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label lblMapLastEditTime;
		private System.Windows.Forms.Label lblMapName;
		private System.Windows.Forms.Label lblMapHash;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelLogIn;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLogIn;
	}
}

