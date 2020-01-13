namespace TrafficControlTest.UserControl
{
	partial class UcSearch
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcSearch));
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnSearch = new System.Windows.Forms.Button();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.cbLimit = new System.Windows.Forms.ComboBox();
			this.dgvSearchResult = new System.Windows.Forms.DataGridView();
			this.dtpDateFilter = new TrafficControlTest.UserControl.DateTimePickerColorful();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvSearchResult)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.panel1.Controls.Add(this.tableLayoutPanel1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(850, 50);
			this.panel1.TabIndex = 1;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
			this.tableLayoutPanel1.Controls.Add(this.dtpDateFilter, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnSearch, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtSearch, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cbLimit, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(840, 50);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// btnSearch
			// 
			this.btnSearch.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnSearch.FlatAppearance.BorderSize = 0;
			this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
			this.btnSearch.Location = new System.Drawing.Point(403, 3);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(44, 44);
			this.btnSearch.TabIndex = 8;
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// txtSearch
			// 
			this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.txtSearch.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtSearch.ForeColor = System.Drawing.Color.White;
			this.txtSearch.Location = new System.Drawing.Point(3, 3);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(394, 43);
			this.txtSearch.TabIndex = 6;
			this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
			// 
			// cbLimit
			// 
			this.cbLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbLimit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.cbLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbLimit.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbLimit.ForeColor = System.Drawing.Color.White;
			this.cbLimit.FormattingEnabled = true;
			this.cbLimit.Location = new System.Drawing.Point(453, 7);
			this.cbLimit.Name = "cbLimit";
			this.cbLimit.Size = new System.Drawing.Size(144, 35);
			this.cbLimit.TabIndex = 7;
			this.cbLimit.SelectedIndexChanged += new System.EventHandler(this.cbLimit_SelectedIndexChanged);
			// 
			// dgvSearchResult
			// 
			this.dgvSearchResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvSearchResult.Location = new System.Drawing.Point(0, 50);
			this.dgvSearchResult.Name = "dgvSearchResult";
			this.dgvSearchResult.RowTemplate.Height = 27;
			this.dgvSearchResult.Size = new System.Drawing.Size(850, 500);
			this.dgvSearchResult.TabIndex = 2;
			// 
			// dtpDateFilter
			// 
			this.dtpDateFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.dtpDateFilter.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.dtpDateFilter.BorderColor = System.Drawing.Color.White;
			this.dtpDateFilter.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.dtpDateFilter.ForeTextColor = System.Drawing.Color.White;
			this.dtpDateFilter.Location = new System.Drawing.Point(603, 7);
			this.dtpDateFilter.Name = "dtpDateFilter";
			this.dtpDateFilter.Size = new System.Drawing.Size(234, 35);
			this.dtpDateFilter.TabIndex = 2;
			this.dtpDateFilter.ValueChanged += new System.EventHandler(this.dtpDateFilter_ValueChanged);
			// 
			// UcSearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.Controls.Add(this.dgvSearchResult);
			this.Controls.Add(this.panel1);
			this.Name = "UcSearch";
			this.Size = new System.Drawing.Size(850, 550);
			this.panel1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvSearchResult)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.ComboBox cbLimit;
		private System.Windows.Forms.DataGridView dgvSearchResult;
		private DateTimePickerColorful dtpDateFilter;
	}
}
