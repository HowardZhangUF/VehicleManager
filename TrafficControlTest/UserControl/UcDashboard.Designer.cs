namespace TrafficControlTest.UserControl
{
	partial class UcDashboard
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea9 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend9 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.chart4 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.chart3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.ucDailyMissionCount1 = new TrafficControlTest.UserControl.UcDailyMissionCount();
			this.ucDailyMissionAverageCost1 = new TrafficControlTest.UserControl.UcDailyMissionAverageCost();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chart4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chart3)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 9;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.Controls.Add(this.chart2, 5, 1);
			this.tableLayoutPanel1.Controls.Add(this.chart4, 5, 3);
			this.tableLayoutPanel1.Controls.Add(this.chart3, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.ucDailyMissionCount1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.ucDailyMissionAverageCost1, 3, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(850, 600);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// chart2
			// 
			chartArea7.Name = "ChartArea1";
			this.chart2.ChartAreas.Add(chartArea7);
			this.tableLayoutPanel1.SetColumnSpan(this.chart2, 3);
			this.chart2.Dock = System.Windows.Forms.DockStyle.Fill;
			legend7.Name = "Legend1";
			this.chart2.Legends.Add(legend7);
			this.chart2.Location = new System.Drawing.Point(433, 13);
			this.chart2.Name = "chart2";
			series7.ChartArea = "ChartArea1";
			series7.Legend = "Legend1";
			series7.Name = "Series1";
			this.chart2.Series.Add(series7);
			this.chart2.Size = new System.Drawing.Size(404, 279);
			this.chart2.TabIndex = 0;
			this.chart2.Text = "chart2";
			// 
			// chart4
			// 
			chartArea8.Name = "ChartArea1";
			this.chart4.ChartAreas.Add(chartArea8);
			this.tableLayoutPanel1.SetColumnSpan(this.chart4, 3);
			this.chart4.Dock = System.Windows.Forms.DockStyle.Fill;
			legend8.Name = "Legend1";
			this.chart4.Legends.Add(legend8);
			this.chart4.Location = new System.Drawing.Point(433, 308);
			this.chart4.Name = "chart4";
			series8.ChartArea = "ChartArea1";
			series8.Legend = "Legend1";
			series8.Name = "Series1";
			this.chart4.Series.Add(series8);
			this.chart4.Size = new System.Drawing.Size(404, 279);
			this.chart4.TabIndex = 1;
			this.chart4.Text = "chart4";
			// 
			// chart3
			// 
			chartArea9.Name = "ChartArea1";
			this.chart3.ChartAreas.Add(chartArea9);
			this.tableLayoutPanel1.SetColumnSpan(this.chart3, 3);
			this.chart3.Dock = System.Windows.Forms.DockStyle.Fill;
			legend9.Name = "Legend1";
			this.chart3.Legends.Add(legend9);
			this.chart3.Location = new System.Drawing.Point(13, 308);
			this.chart3.Name = "chart3";
			series9.ChartArea = "ChartArea1";
			series9.Legend = "Legend1";
			series9.Name = "Series1";
			this.chart3.Series.Add(series9);
			this.chart3.Size = new System.Drawing.Size(404, 279);
			this.chart3.TabIndex = 3;
			this.chart3.Text = "chart3";
			// 
			// ucDailyMissionCount1
			// 
			this.ucDailyMissionCount1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.ucDailyMissionCount1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucDailyMissionCount1.Location = new System.Drawing.Point(13, 13);
			this.ucDailyMissionCount1.mDate = new System.DateTime(((long)(0)));
			this.ucDailyMissionCount1.mFailedCount = 0;
			this.ucDailyMissionCount1.mSuccessedCount = 0;
			this.ucDailyMissionCount1.Name = "ucDailyMissionCount1";
			this.ucDailyMissionCount1.Size = new System.Drawing.Size(194, 279);
			this.ucDailyMissionCount1.TabIndex = 4;
			// 
			// ucDailyMissionAverageCost1
			// 
			this.ucDailyMissionAverageCost1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.ucDailyMissionAverageCost1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucDailyMissionAverageCost1.Location = new System.Drawing.Point(223, 13);
			this.ucDailyMissionAverageCost1.mAverageCostInSec = 0D;
			this.ucDailyMissionAverageCost1.mDate = new System.DateTime(((long)(0)));
			this.ucDailyMissionAverageCost1.mSuccessedMissionCount = 0;
			this.ucDailyMissionAverageCost1.Name = "ucDailyMissionAverageCost1";
			this.ucDailyMissionAverageCost1.Size = new System.Drawing.Size(194, 279);
			this.ucDailyMissionAverageCost1.TabIndex = 5;
			// 
			// UcDashboard
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "UcDashboard";
			this.Size = new System.Drawing.Size(850, 600);
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chart4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chart3)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart4;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart3;
		private UcDailyMissionCount ucDailyMissionCount1;
		private UcDailyMissionAverageCost ucDailyMissionAverageCost1;
	}
}
