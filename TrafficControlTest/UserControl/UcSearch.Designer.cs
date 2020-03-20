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
			this.panel2 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.cbHourFilterStart = new System.Windows.Forms.ComboBox();
			this.cbSearchCondition = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cbHourFilterEnd = new System.Windows.Forms.ComboBox();
			this.dtpDateFilterEnd = new TrafficControlTest.UserControl.DateTimePickerColorful();
			this.dtpDateFilterStart = new TrafficControlTest.UserControl.DateTimePickerColorful();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvSearchResult)).BeginInit();
			this.panel2.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
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
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.btnSearch, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtSearch, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cbLimit, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(600, 50);
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
			// 
			// dgvSearchResult
			// 
			this.dgvSearchResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvSearchResult.Location = new System.Drawing.Point(0, 100);
			this.dgvSearchResult.Name = "dgvSearchResult";
			this.dgvSearchResult.RowTemplate.Height = 27;
			this.dgvSearchResult.Size = new System.Drawing.Size(850, 450);
			this.dgvSearchResult.TabIndex = 2;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.tableLayoutPanel2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 50);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(850, 50);
			this.panel2.TabIndex = 3;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 6;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 205F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 205F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
			this.tableLayoutPanel2.Controls.Add(this.cbHourFilterStart, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.dtpDateFilterEnd, 4, 0);
			this.tableLayoutPanel2.Controls.Add(this.cbSearchCondition, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.label1, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.cbHourFilterEnd, 5, 0);
			this.tableLayoutPanel2.Controls.Add(this.dtpDateFilterStart, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(850, 50);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// cbHourFilterStart
			// 
			this.cbHourFilterStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbHourFilterStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.cbHourFilterStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbHourFilterStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbHourFilterStart.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbHourFilterStart.ForeColor = System.Drawing.Color.White;
			this.cbHourFilterStart.FormattingEnabled = true;
			this.cbHourFilterStart.Location = new System.Drawing.Point(388, 7);
			this.cbHourFilterStart.Name = "cbHourFilterStart";
			this.cbHourFilterStart.Size = new System.Drawing.Size(99, 35);
			this.cbHourFilterStart.TabIndex = 13;
			// 
			// cbSearchCondition
			// 
			this.cbSearchCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbSearchCondition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.cbSearchCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSearchCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbSearchCondition.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbSearchCondition.ForeColor = System.Drawing.Color.White;
			this.cbSearchCondition.FormattingEnabled = true;
			this.cbSearchCondition.Location = new System.Drawing.Point(3, 7);
			this.cbSearchCondition.Name = "cbSearchCondition";
			this.cbSearchCondition.Size = new System.Drawing.Size(174, 35);
			this.cbSearchCondition.TabIndex = 8;
			this.cbSearchCondition.SelectedIndexChanged += new System.EventHandler(this.cbSearchCondition_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(497, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 24);
			this.label1.TabIndex = 11;
			this.label1.Text = "To";
			// 
			// cbHourFilterEnd
			// 
			this.cbHourFilterEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbHourFilterEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.cbHourFilterEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbHourFilterEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbHourFilterEnd.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbHourFilterEnd.ForeColor = System.Drawing.Color.White;
			this.cbHourFilterEnd.FormattingEnabled = true;
			this.cbHourFilterEnd.Location = new System.Drawing.Point(748, 7);
			this.cbHourFilterEnd.Name = "cbHourFilterEnd";
			this.cbHourFilterEnd.Size = new System.Drawing.Size(99, 35);
			this.cbHourFilterEnd.TabIndex = 10;
			// 
			// dtpDateFilterEnd
			// 
			this.dtpDateFilterEnd.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.dtpDateFilterEnd.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.dtpDateFilterEnd.BorderColor = System.Drawing.Color.White;
			this.dtpDateFilterEnd.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.dtpDateFilterEnd.ForeTextColor = System.Drawing.Color.White;
			this.dtpDateFilterEnd.Location = new System.Drawing.Point(543, 7);
			this.dtpDateFilterEnd.Name = "dtpDateFilterEnd";
			this.dtpDateFilterEnd.Size = new System.Drawing.Size(199, 35);
			this.dtpDateFilterEnd.TabIndex = 12;
			// 
			// dtpDateFilterStart
			// 
			this.dtpDateFilterStart.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.dtpDateFilterStart.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.dtpDateFilterStart.BorderColor = System.Drawing.Color.White;
			this.dtpDateFilterStart.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.dtpDateFilterStart.ForeTextColor = System.Drawing.Color.White;
			this.dtpDateFilterStart.Location = new System.Drawing.Point(183, 7);
			this.dtpDateFilterStart.Name = "dtpDateFilterStart";
			this.dtpDateFilterStart.Size = new System.Drawing.Size(199, 35);
			this.dtpDateFilterStart.TabIndex = 9;
			// 
			// UcSearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.Controls.Add(this.dgvSearchResult);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "UcSearch";
			this.Size = new System.Drawing.Size(850, 550);
			this.panel1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvSearchResult)).EndInit();
			this.panel2.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.ComboBox cbLimit;
		private System.Windows.Forms.DataGridView dgvSearchResult;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.ComboBox cbHourFilterStart;
		private DateTimePickerColorful dtpDateFilterEnd;
		private System.Windows.Forms.ComboBox cbSearchCondition;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbHourFilterEnd;
		private DateTimePickerColorful dtpDateFilterStart;
	}
}
