namespace FootprintViewer
{
	partial class FootprintViewer
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
			this.cbYear1 = new System.Windows.Forms.ComboBox();
			this.cbMonth1 = new System.Windows.Forms.ComboBox();
			this.cbDay1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cbHour1 = new System.Windows.Forms.ComboBox();
			this.cbMinute1 = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.cbSecond1 = new System.Windows.Forms.ComboBox();
			this.cbSecond2 = new System.Windows.Forms.ComboBox();
			this.cbMinute2 = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cbHour2 = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.cbDay2 = new System.Windows.Forms.ComboBox();
			this.cbMonth2 = new System.Windows.Forms.ComboBox();
			this.cbYear2 = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.txtFootprintDirectory = new System.Windows.Forms.TextBox();
			this.btnBrowseFootprintDirectory = new System.Windows.Forms.Button();
			this.btnBrowseInsepctionResultDirectory = new System.Windows.Forms.Button();
			this.txtInspectionResultDirectory = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.btnBrowseMapPath = new System.Windows.Forms.Button();
			this.txtMapPath = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.cmbInspectionResultIntervals = new System.Windows.Forms.ComboBox();
			this.btnSetTimeInterval = new System.Windows.Forms.Button();
			this.btnSaveSettings = new System.Windows.Forms.Button();
			this.gluiCtrl1 = new GLUI.GLUICtrl();
			this.lbRobotID = new System.Windows.Forms.ListBox();
			this.btnReloadSettings = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cbYear1
			// 
			this.cbYear1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbYear1.FormattingEnabled = true;
			this.cbYear1.Location = new System.Drawing.Point(84, 87);
			this.cbYear1.Name = "cbYear1";
			this.cbYear1.Size = new System.Drawing.Size(65, 23);
			this.cbYear1.TabIndex = 16;
			// 
			// cbMonth1
			// 
			this.cbMonth1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMonth1.FormattingEnabled = true;
			this.cbMonth1.Location = new System.Drawing.Point(172, 87);
			this.cbMonth1.Name = "cbMonth1";
			this.cbMonth1.Size = new System.Drawing.Size(60, 23);
			this.cbMonth1.TabIndex = 17;
			// 
			// cbDay1
			// 
			this.cbDay1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDay1.FormattingEnabled = true;
			this.cbDay1.Location = new System.Drawing.Point(255, 87);
			this.cbDay1.Name = "cbDay1";
			this.cbDay1.Size = new System.Drawing.Size(60, 23);
			this.cbDay1.TabIndex = 18;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(155, 90);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(11, 15);
			this.label1.TabIndex = 19;
			this.label1.Text = "/";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(238, 90);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(11, 15);
			this.label2.TabIndex = 20;
			this.label2.Text = "/";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(412, 90);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(11, 15);
			this.label3.TabIndex = 21;
			this.label3.Text = ":";
			// 
			// cbHour1
			// 
			this.cbHour1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbHour1.FormattingEnabled = true;
			this.cbHour1.Location = new System.Drawing.Point(346, 87);
			this.cbHour1.Name = "cbHour1";
			this.cbHour1.Size = new System.Drawing.Size(60, 23);
			this.cbHour1.TabIndex = 22;
			// 
			// cbMinute1
			// 
			this.cbMinute1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMinute1.FormattingEnabled = true;
			this.cbMinute1.Location = new System.Drawing.Point(429, 87);
			this.cbMinute1.Name = "cbMinute1";
			this.cbMinute1.Size = new System.Drawing.Size(60, 23);
			this.cbMinute1.TabIndex = 24;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(495, 90);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(11, 15);
			this.label4.TabIndex = 23;
			this.label4.Text = ":";
			// 
			// cbSecond1
			// 
			this.cbSecond1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSecond1.FormattingEnabled = true;
			this.cbSecond1.Location = new System.Drawing.Point(512, 87);
			this.cbSecond1.Name = "cbSecond1";
			this.cbSecond1.Size = new System.Drawing.Size(60, 23);
			this.cbSecond1.TabIndex = 25;
			// 
			// cbSecond2
			// 
			this.cbSecond2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSecond2.FormattingEnabled = true;
			this.cbSecond2.Location = new System.Drawing.Point(512, 117);
			this.cbSecond2.Name = "cbSecond2";
			this.cbSecond2.Size = new System.Drawing.Size(60, 23);
			this.cbSecond2.TabIndex = 35;
			// 
			// cbMinute2
			// 
			this.cbMinute2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMinute2.FormattingEnabled = true;
			this.cbMinute2.Location = new System.Drawing.Point(429, 117);
			this.cbMinute2.Name = "cbMinute2";
			this.cbMinute2.Size = new System.Drawing.Size(60, 23);
			this.cbMinute2.TabIndex = 34;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(495, 120);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(11, 15);
			this.label5.TabIndex = 33;
			this.label5.Text = ":";
			// 
			// cbHour2
			// 
			this.cbHour2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbHour2.FormattingEnabled = true;
			this.cbHour2.Location = new System.Drawing.Point(346, 117);
			this.cbHour2.Name = "cbHour2";
			this.cbHour2.Size = new System.Drawing.Size(60, 23);
			this.cbHour2.TabIndex = 32;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(412, 120);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(11, 15);
			this.label6.TabIndex = 31;
			this.label6.Text = ":";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(238, 120);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(11, 15);
			this.label7.TabIndex = 30;
			this.label7.Text = "/";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(155, 120);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(11, 15);
			this.label8.TabIndex = 29;
			this.label8.Text = "/";
			// 
			// cbDay2
			// 
			this.cbDay2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDay2.FormattingEnabled = true;
			this.cbDay2.Location = new System.Drawing.Point(255, 117);
			this.cbDay2.Name = "cbDay2";
			this.cbDay2.Size = new System.Drawing.Size(60, 23);
			this.cbDay2.TabIndex = 28;
			// 
			// cbMonth2
			// 
			this.cbMonth2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMonth2.FormattingEnabled = true;
			this.cbMonth2.Location = new System.Drawing.Point(172, 117);
			this.cbMonth2.Name = "cbMonth2";
			this.cbMonth2.Size = new System.Drawing.Size(60, 23);
			this.cbMonth2.TabIndex = 27;
			// 
			// cbYear2
			// 
			this.cbYear2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbYear2.FormattingEnabled = true;
			this.cbYear2.Location = new System.Drawing.Point(84, 117);
			this.cbYear2.Name = "cbYear2";
			this.cbYear2.Size = new System.Drawing.Size(65, 23);
			this.cbYear2.TabIndex = 26;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(30, 90);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(48, 15);
			this.label9.TabIndex = 36;
			this.label9.Text = "Time1:";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(30, 120);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(48, 15);
			this.label10.TabIndex = 37;
			this.label10.Text = "Time2:";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(30, 60);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(182, 15);
			this.label11.TabIndex = 38;
			this.label11.Text = "Footprint Directory (VMLog):";
			// 
			// txtFootprintDirectory
			// 
			this.txtFootprintDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFootprintDirectory.Location = new System.Drawing.Point(218, 57);
			this.txtFootprintDirectory.Name = "txtFootprintDirectory";
			this.txtFootprintDirectory.Size = new System.Drawing.Size(725, 25);
			this.txtFootprintDirectory.TabIndex = 39;
			// 
			// btnBrowseFootprintDirectory
			// 
			this.btnBrowseFootprintDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseFootprintDirectory.Location = new System.Drawing.Point(949, 55);
			this.btnBrowseFootprintDirectory.Name = "btnBrowseFootprintDirectory";
			this.btnBrowseFootprintDirectory.Size = new System.Drawing.Size(75, 25);
			this.btnBrowseFootprintDirectory.TabIndex = 40;
			this.btnBrowseFootprintDirectory.Text = "...";
			this.btnBrowseFootprintDirectory.UseVisualStyleBackColor = true;
			this.btnBrowseFootprintDirectory.Click += new System.EventHandler(this.btnBrowseFootprintDirectory_Click);
			// 
			// btnBrowseInsepctionResultDirectory
			// 
			this.btnBrowseInsepctionResultDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseInsepctionResultDirectory.Location = new System.Drawing.Point(949, 143);
			this.btnBrowseInsepctionResultDirectory.Name = "btnBrowseInsepctionResultDirectory";
			this.btnBrowseInsepctionResultDirectory.Size = new System.Drawing.Size(75, 25);
			this.btnBrowseInsepctionResultDirectory.TabIndex = 44;
			this.btnBrowseInsepctionResultDirectory.Text = "...";
			this.btnBrowseInsepctionResultDirectory.UseVisualStyleBackColor = true;
			this.btnBrowseInsepctionResultDirectory.Click += new System.EventHandler(this.btnBrowseInsepctionResultDirectory_Click);
			// 
			// txtInspectionResultDirectory
			// 
			this.txtInspectionResultDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtInspectionResultDirectory.Location = new System.Drawing.Point(267, 145);
			this.txtInspectionResultDirectory.Name = "txtInspectionResultDirectory";
			this.txtInspectionResultDirectory.Size = new System.Drawing.Size(676, 25);
			this.txtInspectionResultDirectory.TabIndex = 43;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(31, 148);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(230, 15);
			this.label12.TabIndex = 42;
			this.label12.Text = "Inspection Result Directory (CIMLog):";
			// 
			// btnBrowseMapPath
			// 
			this.btnBrowseMapPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseMapPath.Location = new System.Drawing.Point(949, 25);
			this.btnBrowseMapPath.Name = "btnBrowseMapPath";
			this.btnBrowseMapPath.Size = new System.Drawing.Size(75, 25);
			this.btnBrowseMapPath.TabIndex = 47;
			this.btnBrowseMapPath.Text = "...";
			this.btnBrowseMapPath.UseVisualStyleBackColor = true;
			this.btnBrowseMapPath.Click += new System.EventHandler(this.btnBrowseMapPath_Click);
			// 
			// txtMapPath
			// 
			this.txtMapPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMapPath.Location = new System.Drawing.Point(102, 27);
			this.txtMapPath.Name = "txtMapPath";
			this.txtMapPath.Size = new System.Drawing.Size(841, 25);
			this.txtMapPath.TabIndex = 46;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(30, 30);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(66, 15);
			this.label13.TabIndex = 45;
			this.label13.Text = "Map Path:";
			// 
			// cmbInspectionResultIntervals
			// 
			this.cmbInspectionResultIntervals.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbInspectionResultIntervals.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbInspectionResultIntervals.FormattingEnabled = true;
			this.cmbInspectionResultIntervals.Location = new System.Drawing.Point(33, 175);
			this.cmbInspectionResultIntervals.Name = "cmbInspectionResultIntervals";
			this.cmbInspectionResultIntervals.Size = new System.Drawing.Size(991, 23);
			this.cmbInspectionResultIntervals.TabIndex = 48;
			this.cmbInspectionResultIntervals.SelectedIndexChanged += new System.EventHandler(this.cmbInspectionResultIntervals_SelectedIndexChanged);
			// 
			// btnSetTimeInterval
			// 
			this.btnSetTimeInterval.Location = new System.Drawing.Point(578, 86);
			this.btnSetTimeInterval.Name = "btnSetTimeInterval";
			this.btnSetTimeInterval.Size = new System.Drawing.Size(99, 52);
			this.btnSetTimeInterval.TabIndex = 49;
			this.btnSetTimeInterval.Text = "Set";
			this.btnSetTimeInterval.UseVisualStyleBackColor = true;
			this.btnSetTimeInterval.Click += new System.EventHandler(this.btnSetTimeInterval_Click);
			// 
			// btnSaveSettings
			// 
			this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveSettings.Location = new System.Drawing.Point(1030, 25);
			this.btnSaveSettings.Name = "btnSaveSettings";
			this.btnSaveSettings.Size = new System.Drawing.Size(148, 83);
			this.btnSaveSettings.TabIndex = 51;
			this.btnSaveSettings.Text = "Save Settings";
			this.btnSaveSettings.UseVisualStyleBackColor = true;
			this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
			// 
			// gluiCtrl1
			// 
			this.gluiCtrl1.AllowObjectMenu = true;
			this.gluiCtrl1.AllowUndoMenu = true;
			this.gluiCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gluiCtrl1.Location = new System.Drawing.Point(154, 203);
			this.gluiCtrl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.gluiCtrl1.Name = "gluiCtrl1";
			this.gluiCtrl1.ShowAxis = true;
			this.gluiCtrl1.ShowGrid = true;
			this.gluiCtrl1.Size = new System.Drawing.Size(1024, 438);
			this.gluiCtrl1.TabIndex = 50;
			this.gluiCtrl1.Zoom = 10D;
			// 
			// lbRobotID
			// 
			this.lbRobotID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lbRobotID.FormattingEnabled = true;
			this.lbRobotID.ItemHeight = 15;
			this.lbRobotID.Location = new System.Drawing.Point(29, 204);
			this.lbRobotID.Name = "lbRobotID";
			this.lbRobotID.Size = new System.Drawing.Size(120, 439);
			this.lbRobotID.TabIndex = 52;
			// 
			// btnReloadSettings
			// 
			this.btnReloadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReloadSettings.Location = new System.Drawing.Point(1030, 114);
			this.btnReloadSettings.Name = "btnReloadSettings";
			this.btnReloadSettings.Size = new System.Drawing.Size(148, 84);
			this.btnReloadSettings.TabIndex = 53;
			this.btnReloadSettings.Text = "Reload Settings";
			this.btnReloadSettings.UseVisualStyleBackColor = true;
			this.btnReloadSettings.Click += new System.EventHandler(this.btnReloadSettings_Click);
			// 
			// FootprintViewer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1195, 654);
			this.Controls.Add(this.btnReloadSettings);
			this.Controls.Add(this.lbRobotID);
			this.Controls.Add(this.btnSaveSettings);
			this.Controls.Add(this.gluiCtrl1);
			this.Controls.Add(this.btnSetTimeInterval);
			this.Controls.Add(this.cmbInspectionResultIntervals);
			this.Controls.Add(this.btnBrowseMapPath);
			this.Controls.Add(this.txtMapPath);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.btnBrowseInsepctionResultDirectory);
			this.Controls.Add(this.txtInspectionResultDirectory);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.btnBrowseFootprintDirectory);
			this.Controls.Add(this.txtFootprintDirectory);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.cbSecond2);
			this.Controls.Add(this.cbMinute2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.cbHour2);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.cbDay2);
			this.Controls.Add(this.cbMonth2);
			this.Controls.Add(this.cbYear2);
			this.Controls.Add(this.cbSecond1);
			this.Controls.Add(this.cbMinute1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cbHour1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbDay1);
			this.Controls.Add(this.cbMonth1);
			this.Controls.Add(this.cbYear1);
			this.Name = "FootprintViewer";
			this.Text = "Footprint Viewer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FootprintViewer_FormClosing);
			this.Load += new System.EventHandler(this.FootprintViewer_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ComboBox cbYear1;
		private System.Windows.Forms.ComboBox cbMonth1;
		private System.Windows.Forms.ComboBox cbDay1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbHour1;
		private System.Windows.Forms.ComboBox cbMinute1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbSecond1;
		private System.Windows.Forms.ComboBox cbSecond2;
		private System.Windows.Forms.ComboBox cbMinute2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cbHour2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox cbDay2;
		private System.Windows.Forms.ComboBox cbMonth2;
		private System.Windows.Forms.ComboBox cbYear2;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox txtFootprintDirectory;
		private System.Windows.Forms.Button btnBrowseFootprintDirectory;
		private System.Windows.Forms.Button btnBrowseInsepctionResultDirectory;
		private System.Windows.Forms.TextBox txtInspectionResultDirectory;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Button btnBrowseMapPath;
		private System.Windows.Forms.TextBox txtMapPath;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.ComboBox cmbInspectionResultIntervals;
		private System.Windows.Forms.Button btnSetTimeInterval;
		private GLUI.GLUICtrl gluiCtrl1;
		private System.Windows.Forms.Button btnSaveSettings;
		private System.Windows.Forms.ListBox lbRobotID;
		private System.Windows.Forms.Button btnReloadSettings;
	}
}

