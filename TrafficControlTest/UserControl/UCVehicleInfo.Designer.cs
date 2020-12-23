namespace TrafficControlTest.UserControl
{
	partial class UcVehicleInfo
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcVehicleInfo));
			this.lblId = new System.Windows.Forms.Label();
			this.lblBattery = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblTarget = new System.Windows.Forms.Label();
			this.lblState = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.lblLocationScore = new System.Windows.Forms.Label();
			this.pnlBorder = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.pnlBorder.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblId
			// 
			this.lblId.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblId.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblId.Location = new System.Drawing.Point(7, 4);
			this.lblId.Name = "lblId";
			this.tableLayoutPanel1.SetRowSpan(this.lblId, 6);
			this.lblId.Size = new System.Drawing.Size(270, 36);
			this.lblId.TabIndex = 0;
			this.lblId.Text = "ID";
			this.lblId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblId.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// lblBattery
			// 
			this.lblBattery.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblBattery.AutoSize = true;
			this.lblBattery.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblBattery.ForeColor = System.Drawing.Color.White;
			this.lblBattery.Location = new System.Drawing.Point(307, 8);
			this.lblBattery.Name = "lblBattery";
			this.tableLayoutPanel1.SetRowSpan(this.lblBattery, 4);
			this.lblBattery.Size = new System.Drawing.Size(70, 15);
			this.lblBattery.TabIndex = 1;
			this.lblBattery.Text = "100.00 %";
			this.lblBattery.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.lblBattery.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.lblId, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblBattery, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblLocationScore, 2, 4);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(4);
			this.tableLayoutPanel1.RowCount = 10;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(388, 68);
			this.tableLayoutPanel1.TabIndex = 5;
			this.tableLayoutPanel1.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lblTarget);
			this.panel1.Controls.Add(this.lblState);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(7, 43);
			this.panel1.Name = "panel1";
			this.tableLayoutPanel1.SetRowSpan(this.panel1, 4);
			this.panel1.Size = new System.Drawing.Size(270, 18);
			this.panel1.TabIndex = 0;
			this.panel1.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// lblTarget
			// 
			this.lblTarget.AutoSize = true;
			this.lblTarget.Dock = System.Windows.Forms.DockStyle.Left;
			this.lblTarget.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblTarget.ForeColor = System.Drawing.Color.LightGray;
			this.lblTarget.Location = new System.Drawing.Point(45, 0);
			this.lblTarget.Name = "lblTarget";
			this.lblTarget.Size = new System.Drawing.Size(56, 20);
			this.lblTarget.TabIndex = 0;
			this.lblTarget.Text = "Target";
			this.lblTarget.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// lblState
			// 
			this.lblState.AutoSize = true;
			this.lblState.Dock = System.Windows.Forms.DockStyle.Left;
			this.lblState.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblState.ForeColor = System.Drawing.Color.LightGray;
			this.lblState.Location = new System.Drawing.Point(0, 0);
			this.lblState.Name = "lblState";
			this.lblState.Size = new System.Drawing.Size(45, 20);
			this.lblState.TabIndex = 0;
			this.lblState.Text = "State";
			this.lblState.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// panel2
			// 
			this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
			this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(283, 7);
			this.panel2.Name = "panel2";
			this.tableLayoutPanel1.SetRowSpan(this.panel2, 4);
			this.panel2.Size = new System.Drawing.Size(18, 18);
			this.panel2.TabIndex = 2;
			this.panel2.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// panel3
			// 
			this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
			this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(283, 31);
			this.panel3.Name = "panel3";
			this.tableLayoutPanel1.SetRowSpan(this.panel3, 4);
			this.panel3.Size = new System.Drawing.Size(18, 18);
			this.panel3.TabIndex = 3;
			this.panel3.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// lblLocationScore
			// 
			this.lblLocationScore.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblLocationScore.AutoSize = true;
			this.lblLocationScore.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblLocationScore.Location = new System.Drawing.Point(307, 32);
			this.lblLocationScore.Name = "lblLocationScore";
			this.tableLayoutPanel1.SetRowSpan(this.lblLocationScore, 4);
			this.lblLocationScore.Size = new System.Drawing.Size(70, 15);
			this.lblLocationScore.TabIndex = 4;
			this.lblLocationScore.Text = "100.00 %";
			this.lblLocationScore.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// pnlBorder
			// 
			this.pnlBorder.BackColor = System.Drawing.Color.White;
			this.pnlBorder.Controls.Add(this.tableLayoutPanel1);
			this.pnlBorder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlBorder.Location = new System.Drawing.Point(4, 4);
			this.pnlBorder.Name = "pnlBorder";
			this.pnlBorder.Padding = new System.Windows.Forms.Padding(2);
			this.pnlBorder.Size = new System.Drawing.Size(392, 72);
			this.pnlBorder.TabIndex = 2;
			// 
			// UcVehicleInfo
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.Controls.Add(this.pnlBorder);
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "UcVehicleInfo";
			this.Padding = new System.Windows.Forms.Padding(4);
			this.Size = new System.Drawing.Size(400, 80);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.pnlBorder.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblId;
		private System.Windows.Forms.Label lblBattery;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label lblLocationScore;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblTarget;
		private System.Windows.Forms.Label lblState;
		private System.Windows.Forms.Panel pnlBorder;
	}
}
