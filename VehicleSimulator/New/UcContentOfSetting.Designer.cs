namespace VehicleSimulator.New
{
	partial class UcContentOfSetting
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
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblMapFileFolderDirectory = new System.Windows.Forms.Label();
            this.btnSelectMapFileFolderDirectory = new System.Windows.Forms.Button();
            this.dgvMapFileList = new System.Windows.Forms.DataGridView();
            this.pbMapPreview = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMapFileList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMapPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblMapFileFolderDirectory, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectMapFileFolderDirectory, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.dgvMapFileList, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.pbMapPreview, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(720, 411);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Map File:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 4);
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Map";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(714, 1);
            this.panel1.TabIndex = 1;
            // 
            // lblMapFileFolderDirectory
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.lblMapFileFolderDirectory, 2);
            this.lblMapFileFolderDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMapFileFolderDirectory.ForeColor = System.Drawing.Color.White;
            this.lblMapFileFolderDirectory.Location = new System.Drawing.Point(103, 36);
            this.lblMapFileFolderDirectory.Name = "lblMapFileFolderDirectory";
            this.lblMapFileFolderDirectory.Size = new System.Drawing.Size(584, 25);
            this.lblMapFileFolderDirectory.TabIndex = 2;
            this.lblMapFileFolderDirectory.Text = "C:\\Users\\User\\Desktop";
            this.lblMapFileFolderDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSelectMapFileFolderDirectory
            // 
            this.btnSelectMapFileFolderDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectMapFileFolderDirectory.FlatAppearance.BorderSize = 0;
            this.btnSelectMapFileFolderDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectMapFileFolderDirectory.Image = global::VehicleSimulator.Properties.Resources.icons8_folder_24px;
            this.btnSelectMapFileFolderDirectory.Location = new System.Drawing.Point(693, 39);
            this.btnSelectMapFileFolderDirectory.Name = "btnSelectMapFileFolderDirectory";
            this.btnSelectMapFileFolderDirectory.Size = new System.Drawing.Size(24, 19);
            this.btnSelectMapFileFolderDirectory.TabIndex = 3;
            this.btnSelectMapFileFolderDirectory.UseVisualStyleBackColor = true;
            // 
            // dgvMapFileList
            // 
            this.dgvMapFileList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.dgvMapFileList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMapFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel1.SetColumnSpan(this.dgvMapFileList, 2);
            this.dgvMapFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMapFileList.Location = new System.Drawing.Point(3, 89);
            this.dgvMapFileList.Name = "dgvMapFileList";
            this.dgvMapFileList.RowTemplate.Height = 27;
            this.dgvMapFileList.Size = new System.Drawing.Size(294, 294);
            this.dgvMapFileList.TabIndex = 4;
            // 
            // pbMapPreview
            // 
            this.pbMapPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.tableLayoutPanel1.SetColumnSpan(this.pbMapPreview, 2);
            this.pbMapPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbMapPreview.Location = new System.Drawing.Point(303, 89);
            this.pbMapPreview.Name = "pbMapPreview";
            this.pbMapPreview.Size = new System.Drawing.Size(414, 294);
            this.pbMapPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMapPreview.TabIndex = 5;
            this.pbMapPreview.TabStop = false;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(3, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Map Folder:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(303, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Map Preview:";
            // 
            // UcContentOfSetting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UcContentOfSetting";
            this.Size = new System.Drawing.Size(720, 420);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMapFileList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMapPreview)).EndInit();
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblMapFileFolderDirectory;
        private System.Windows.Forms.Button btnSelectMapFileFolderDirectory;
        private System.Windows.Forms.DataGridView dgvMapFileList;
        private System.Windows.Forms.PictureBox pbMapPreview;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}
