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
			this.lblId = new System.Windows.Forms.Label();
			this.lblBattery = new System.Windows.Forms.Label();
			this.pnlTop = new System.Windows.Forms.Panel();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.pnlLeft = new System.Windows.Forms.Panel();
			this.pnlRight = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.pnlSecondTop = new System.Windows.Forms.Panel();
			this.pnlSecondBottom = new System.Windows.Forms.Panel();
			this.pnlSecondRight = new System.Windows.Forms.Panel();
			this.pnlSecondLeft = new System.Windows.Forms.Panel();
			this.pnlThirdTop = new System.Windows.Forms.Panel();
			this.pnlThirdBottom = new System.Windows.Forms.Panel();
			this.pnlThirdLeft = new System.Windows.Forms.Panel();
			this.pnlThirdRight = new System.Windows.Forms.Panel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblState = new System.Windows.Forms.Label();
			this.lblTarget = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblId
			// 
			this.lblId.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblId.Font = new System.Drawing.Font("新細明體", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblId.Location = new System.Drawing.Point(3, 0);
			this.lblId.Name = "lblId";
			this.lblId.Size = new System.Drawing.Size(274, 50);
			this.lblId.TabIndex = 0;
			this.lblId.Text = "ID";
			this.lblId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblId.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// lblBattery
			// 
			this.lblBattery.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblBattery.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblBattery.ForeColor = System.Drawing.Color.White;
			this.lblBattery.Location = new System.Drawing.Point(283, 0);
			this.lblBattery.Name = "lblBattery";
			this.lblBattery.Size = new System.Drawing.Size(94, 50);
			this.lblBattery.TabIndex = 1;
			this.lblBattery.Text = "100 %";
			this.lblBattery.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.lblBattery.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// pnlTop
			// 
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(0, 0);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(400, 4);
			this.pnlTop.TabIndex = 2;
			// 
			// pnlBottom
			// 
			this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBottom.Location = new System.Drawing.Point(0, 96);
			this.pnlBottom.Name = "pnlBottom";
			this.pnlBottom.Size = new System.Drawing.Size(400, 4);
			this.pnlBottom.TabIndex = 3;
			// 
			// pnlLeft
			// 
			this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlLeft.Location = new System.Drawing.Point(0, 4);
			this.pnlLeft.Name = "pnlLeft";
			this.pnlLeft.Size = new System.Drawing.Size(4, 92);
			this.pnlLeft.TabIndex = 4;
			// 
			// pnlRight
			// 
			this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlRight.Location = new System.Drawing.Point(396, 4);
			this.pnlRight.Name = "pnlRight";
			this.pnlRight.Size = new System.Drawing.Size(4, 92);
			this.pnlRight.TabIndex = 4;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.lblId, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblBattery, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(380, 50);
			this.tableLayoutPanel1.TabIndex = 5;
			this.tableLayoutPanel1.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// pnlSecondTop
			// 
			this.pnlSecondTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSecondTop.Location = new System.Drawing.Point(4, 4);
			this.pnlSecondTop.Name = "pnlSecondTop";
			this.pnlSecondTop.Size = new System.Drawing.Size(392, 2);
			this.pnlSecondTop.TabIndex = 7;
			// 
			// pnlSecondBottom
			// 
			this.pnlSecondBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlSecondBottom.Location = new System.Drawing.Point(4, 94);
			this.pnlSecondBottom.Name = "pnlSecondBottom";
			this.pnlSecondBottom.Size = new System.Drawing.Size(392, 2);
			this.pnlSecondBottom.TabIndex = 8;
			// 
			// pnlSecondRight
			// 
			this.pnlSecondRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlSecondRight.Location = new System.Drawing.Point(394, 6);
			this.pnlSecondRight.Name = "pnlSecondRight";
			this.pnlSecondRight.Size = new System.Drawing.Size(2, 88);
			this.pnlSecondRight.TabIndex = 8;
			// 
			// pnlSecondLeft
			// 
			this.pnlSecondLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlSecondLeft.Location = new System.Drawing.Point(4, 6);
			this.pnlSecondLeft.Name = "pnlSecondLeft";
			this.pnlSecondLeft.Size = new System.Drawing.Size(2, 88);
			this.pnlSecondLeft.TabIndex = 8;
			// 
			// pnlThirdTop
			// 
			this.pnlThirdTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlThirdTop.Location = new System.Drawing.Point(6, 6);
			this.pnlThirdTop.Name = "pnlThirdTop";
			this.pnlThirdTop.Size = new System.Drawing.Size(388, 4);
			this.pnlThirdTop.TabIndex = 9;
			// 
			// pnlThirdBottom
			// 
			this.pnlThirdBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlThirdBottom.Location = new System.Drawing.Point(6, 90);
			this.pnlThirdBottom.Name = "pnlThirdBottom";
			this.pnlThirdBottom.Size = new System.Drawing.Size(388, 4);
			this.pnlThirdBottom.TabIndex = 10;
			// 
			// pnlThirdLeft
			// 
			this.pnlThirdLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlThirdLeft.Location = new System.Drawing.Point(6, 10);
			this.pnlThirdLeft.Name = "pnlThirdLeft";
			this.pnlThirdLeft.Size = new System.Drawing.Size(4, 80);
			this.pnlThirdLeft.TabIndex = 10;
			// 
			// pnlThirdRight
			// 
			this.pnlThirdRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlThirdRight.Location = new System.Drawing.Point(390, 10);
			this.pnlThirdRight.Name = "pnlThirdRight";
			this.pnlThirdRight.Size = new System.Drawing.Size(4, 80);
			this.pnlThirdRight.TabIndex = 10;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(10, 60);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(380, 30);
			this.tableLayoutPanel2.TabIndex = 6;
			this.tableLayoutPanel2.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lblTarget);
			this.panel1.Controls.Add(this.lblState);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(274, 24);
			this.panel1.TabIndex = 0;
			this.panel1.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// lblState
			// 
			this.lblState.AutoSize = true;
			this.lblState.Dock = System.Windows.Forms.DockStyle.Left;
			this.lblState.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblState.ForeColor = System.Drawing.Color.LightGray;
			this.lblState.Location = new System.Drawing.Point(0, 0);
			this.lblState.Name = "lblState";
			this.lblState.Size = new System.Drawing.Size(55, 24);
			this.lblState.TabIndex = 0;
			this.lblState.Text = "State";
			this.lblState.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// lblTarget
			// 
			this.lblTarget.AutoSize = true;
			this.lblTarget.Dock = System.Windows.Forms.DockStyle.Left;
			this.lblTarget.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblTarget.ForeColor = System.Drawing.Color.LightGray;
			this.lblTarget.Location = new System.Drawing.Point(55, 0);
			this.lblTarget.Name = "lblTarget";
			this.lblTarget.Size = new System.Drawing.Size(69, 24);
			this.lblTarget.TabIndex = 0;
			this.lblTarget.Text = "Target";
			this.lblTarget.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
			// 
			// UcVehicleInfo
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
			this.Controls.Add(this.tableLayoutPanel2);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.pnlThirdRight);
			this.Controls.Add(this.pnlThirdLeft);
			this.Controls.Add(this.pnlThirdBottom);
			this.Controls.Add(this.pnlThirdTop);
			this.Controls.Add(this.pnlSecondLeft);
			this.Controls.Add(this.pnlSecondRight);
			this.Controls.Add(this.pnlSecondBottom);
			this.Controls.Add(this.pnlSecondTop);
			this.Controls.Add(this.pnlRight);
			this.Controls.Add(this.pnlLeft);
			this.Controls.Add(this.pnlBottom);
			this.Controls.Add(this.pnlTop);
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "UcVehicleInfo";
			this.Size = new System.Drawing.Size(400, 100);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblId;
		private System.Windows.Forms.Label lblBattery;
		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Panel pnlBottom;
		private System.Windows.Forms.Panel pnlLeft;
		private System.Windows.Forms.Panel pnlRight;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel pnlSecondTop;
		private System.Windows.Forms.Panel pnlSecondBottom;
		private System.Windows.Forms.Panel pnlSecondRight;
		private System.Windows.Forms.Panel pnlSecondLeft;
		private System.Windows.Forms.Panel pnlThirdTop;
		private System.Windows.Forms.Panel pnlThirdBottom;
		private System.Windows.Forms.Panel pnlThirdLeft;
		private System.Windows.Forms.Panel pnlThirdRight;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblTarget;
		private System.Windows.Forms.Label lblState;
	}
}
