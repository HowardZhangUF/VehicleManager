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
            ((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvInfo
            // 
            this.dgvInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInfo.Location = new System.Drawing.Point(12, 38);
            this.dgvInfo.Name = "dgvInfo";
            this.dgvInfo.RowTemplate.Height = 24;
            this.dgvInfo.Size = new System.Drawing.Size(502, 124);
            this.dgvInfo.TabIndex = 1;
            this.dgvInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvInfo_CellValueChanged);
            this.dgvInfo.DoubleClick += new System.EventHandler(this.DgvInfo_DoubleClick);
            // 
            // cmbSelectType
            // 
            this.cmbSelectType.FormattingEnabled = true;
            this.cmbSelectType.Location = new System.Drawing.Point(12, 12);
            this.cmbSelectType.Name = "cmbSelectType";
            this.cmbSelectType.Size = new System.Drawing.Size(502, 20);
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
            this.GLUI.Location = new System.Drawing.Point(520, 12);
            this.GLUI.Name = "GLUI";
            this.GLUI.ShowAxis = true;
            this.GLUI.ShowGrid = true;
            this.GLUI.Size = new System.Drawing.Size(150, 150);
            this.GLUI.TabIndex = 3;
            this.GLUI.Zoom = 10D;
            this.GLUI.GLDoubleClick += new System.EventHandler(this.GLUI_GLDoubleClick);
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 195);
            this.Controls.Add(this.GLUI);
            this.Controls.Add(this.cmbSelectType);
            this.Controls.Add(this.dgvInfo);
            this.Name = "frmTest";
            this.Text = "GLUITest";
            ((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvInfo;
        private System.Windows.Forms.ComboBox cmbSelectType;
        private GLUI.GLUICtrl GLUI;
    }
}

