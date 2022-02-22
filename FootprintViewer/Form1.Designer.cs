namespace FootprintViewer
{
	partial class Form1
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

		#region Windows Form 設計工具產生的程式碼

		/// <summary>
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{
			this.tbTimestamp = new System.Windows.Forms.TrackBar();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tpSetting = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.rtxtLog = new System.Windows.Forms.RichTextBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.btnSelectLogFile = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtMapFile = new System.Windows.Forms.TextBox();
			this.txtLogFile = new System.Windows.Forms.TextBox();
			this.dtpStart = new System.Windows.Forms.DateTimePicker();
			this.dtpEnd = new System.Windows.Forms.DateTimePicker();
			this.cbStart = new System.Windows.Forms.ComboBox();
			this.cbEnd = new System.Windows.Forms.ComboBox();
			this.btnSelectMapFile = new System.Windows.Forms.Button();
			this.btnLoadSetting = new System.Windows.Forms.Button();
			this.tpFootprint = new System.Windows.Forms.TabPage();
			this.panel3 = new System.Windows.Forms.Panel();
			this.gluiCtrl1 = new GLUI.GLUICtrl();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblCurrentTimestamp = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.tbTimestamp)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tpSetting.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tpFootprint.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbTimestamp
			// 
			this.tbTimestamp.Dock = System.Windows.Forms.DockStyle.Top;
			this.tbTimestamp.Location = new System.Drawing.Point(0, 0);
			this.tbTimestamp.Maximum = 1000;
			this.tbTimestamp.Name = "tbTimestamp";
			this.tbTimestamp.Size = new System.Drawing.Size(870, 45);
			this.tbTimestamp.TabIndex = 0;
			this.tbTimestamp.Value = 1;
			this.tbTimestamp.ValueChanged += new System.EventHandler(this.tbTimestamp_ValueChanged);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tpSetting);
			this.tabControl1.Controls.Add(this.tpFootprint);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Padding = new System.Drawing.Point(20, 10);
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(884, 561);
			this.tabControl1.TabIndex = 1;
			// 
			// tpSetting
			// 
			this.tpSetting.Controls.Add(this.tableLayoutPanel1);
			this.tpSetting.Location = new System.Drawing.Point(4, 36);
			this.tpSetting.Name = "tpSetting";
			this.tpSetting.Padding = new System.Windows.Forms.Padding(3);
			this.tpSetting.Size = new System.Drawing.Size(876, 521);
			this.tpSetting.TabIndex = 0;
			this.tpSetting.Text = "Setting";
			this.tpSetting.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 600F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.rtxtLog, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(870, 515);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// rtxtLog
			// 
			this.rtxtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtxtLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtxtLog.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.rtxtLog.Location = new System.Drawing.Point(603, 3);
			this.rtxtLog.Name = "rtxtLog";
			this.rtxtLog.ReadOnly = true;
			this.rtxtLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.rtxtLog.Size = new System.Drawing.Size(264, 509);
			this.rtxtLog.TabIndex = 0;
			this.rtxtLog.Text = "";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel2.Controls.Add(this.btnSelectLogFile, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.label4, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.label5, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.label6, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.txtMapFile, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.txtLogFile, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.dtpStart, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.dtpEnd, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.cbStart, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.cbEnd, 1, 5);
			this.tableLayoutPanel2.Controls.Add(this.btnSelectMapFile, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.btnLoadSetting, 0, 6);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 8;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(594, 509);
			this.tableLayoutPanel2.TabIndex = 1;
			// 
			// btnSelectLogFile
			// 
			this.btnSelectLogFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSelectLogFile.Location = new System.Drawing.Point(547, 53);
			this.btnSelectLogFile.Name = "btnSelectLogFile";
			this.btnSelectLogFile.Size = new System.Drawing.Size(44, 44);
			this.btnSelectLogFile.TabIndex = 13;
			this.btnSelectLogFile.Text = "...";
			this.btnSelectLogFile.UseVisualStyleBackColor = true;
			this.btnSelectLogFile.Click += new System.EventHandler(this.btnSelectLogFile_Click);
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 21);
			this.label1.TabIndex = 0;
			this.label1.Text = "Log File";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 114);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 21);
			this.label2.TabIndex = 1;
			this.label2.Text = "Start Date";
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 14);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(82, 21);
			this.label3.TabIndex = 2;
			this.label3.Text = "Map File";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 164);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(93, 21);
			this.label4.TabIndex = 3;
			this.label4.Text = "Start Time";
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 214);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(84, 21);
			this.label5.TabIndex = 4;
			this.label5.Text = "End Date";
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(3, 264);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(88, 21);
			this.label6.TabIndex = 5;
			this.label6.Text = "End Time";
			// 
			// txtMapFile
			// 
			this.txtMapFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMapFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtMapFile.Location = new System.Drawing.Point(113, 8);
			this.txtMapFile.Name = "txtMapFile";
			this.txtMapFile.ReadOnly = true;
			this.txtMapFile.Size = new System.Drawing.Size(428, 33);
			this.txtMapFile.TabIndex = 6;
			// 
			// txtLogFile
			// 
			this.txtLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtLogFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtLogFile.Location = new System.Drawing.Point(113, 58);
			this.txtLogFile.Name = "txtLogFile";
			this.txtLogFile.ReadOnly = true;
			this.txtLogFile.Size = new System.Drawing.Size(428, 33);
			this.txtLogFile.TabIndex = 7;
			// 
			// dtpStart
			// 
			this.dtpStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.dtpStart.CustomFormat = "";
			this.dtpStart.Location = new System.Drawing.Point(113, 108);
			this.dtpStart.Name = "dtpStart";
			this.dtpStart.Size = new System.Drawing.Size(428, 33);
			this.dtpStart.TabIndex = 8;
			// 
			// dtpEnd
			// 
			this.dtpEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.dtpEnd.CustomFormat = "";
			this.dtpEnd.Location = new System.Drawing.Point(113, 208);
			this.dtpEnd.Name = "dtpEnd";
			this.dtpEnd.Size = new System.Drawing.Size(428, 33);
			this.dtpEnd.TabIndex = 9;
			// 
			// cbStart
			// 
			this.cbStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbStart.FormattingEnabled = true;
			this.cbStart.Location = new System.Drawing.Point(113, 165);
			this.cbStart.Name = "cbStart";
			this.cbStart.Size = new System.Drawing.Size(428, 29);
			this.cbStart.TabIndex = 10;
			// 
			// cbEnd
			// 
			this.cbEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbEnd.FormattingEnabled = true;
			this.cbEnd.Location = new System.Drawing.Point(113, 265);
			this.cbEnd.Name = "cbEnd";
			this.cbEnd.Size = new System.Drawing.Size(428, 29);
			this.cbEnd.TabIndex = 11;
			// 
			// btnSelectMapFile
			// 
			this.btnSelectMapFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSelectMapFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSelectMapFile.Location = new System.Drawing.Point(547, 3);
			this.btnSelectMapFile.Name = "btnSelectMapFile";
			this.btnSelectMapFile.Size = new System.Drawing.Size(44, 44);
			this.btnSelectMapFile.TabIndex = 12;
			this.btnSelectMapFile.Text = "...";
			this.btnSelectMapFile.UseVisualStyleBackColor = true;
			this.btnSelectMapFile.Click += new System.EventHandler(this.btnSelectMapFile_Click);
			// 
			// btnLoadSetting
			// 
			this.btnLoadSetting.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnLoadSetting.Location = new System.Drawing.Point(3, 303);
			this.btnLoadSetting.Name = "btnLoadSetting";
			this.btnLoadSetting.Size = new System.Drawing.Size(104, 44);
			this.btnLoadSetting.TabIndex = 14;
			this.btnLoadSetting.Text = "Load";
			this.btnLoadSetting.UseVisualStyleBackColor = true;
			this.btnLoadSetting.Click += new System.EventHandler(this.btnLoadSetting_Click);
			// 
			// tpFootprint
			// 
			this.tpFootprint.Controls.Add(this.panel3);
			this.tpFootprint.Controls.Add(this.panel2);
			this.tpFootprint.Controls.Add(this.panel1);
			this.tpFootprint.Location = new System.Drawing.Point(4, 36);
			this.tpFootprint.Name = "tpFootprint";
			this.tpFootprint.Padding = new System.Windows.Forms.Padding(3);
			this.tpFootprint.Size = new System.Drawing.Size(876, 521);
			this.tpFootprint.TabIndex = 1;
			this.tpFootprint.Text = "Footprint";
			this.tpFootprint.UseVisualStyleBackColor = true;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.gluiCtrl1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(3, 3);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(870, 345);
			this.panel3.TabIndex = 5;
			// 
			// gluiCtrl1
			// 
			this.gluiCtrl1.AllowObjectMenu = true;
			this.gluiCtrl1.AllowUndoMenu = true;
			this.gluiCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gluiCtrl1.Location = new System.Drawing.Point(0, 0);
			this.gluiCtrl1.Name = "gluiCtrl1";
			this.gluiCtrl1.ShowAGVText = true;
			this.gluiCtrl1.ShowAxis = true;
			this.gluiCtrl1.ShowGrid = true;
			this.gluiCtrl1.ShowObjectText = true;
			this.gluiCtrl1.Size = new System.Drawing.Size(870, 345);
			this.gluiCtrl1.TabIndex = 2;
			this.gluiCtrl1.Zoom = 10D;
			// 
			// panel2
			// 
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(3, 348);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(870, 100);
			this.panel2.TabIndex = 4;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tbTimestamp);
			this.panel1.Controls.Add(this.lblCurrentTimestamp);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(3, 448);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(870, 70);
			this.panel1.TabIndex = 3;
			// 
			// lblCurrentTimestamp
			// 
			this.lblCurrentTimestamp.AutoSize = true;
			this.lblCurrentTimestamp.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.lblCurrentTimestamp.Location = new System.Drawing.Point(5, 48);
			this.lblCurrentTimestamp.Name = "lblCurrentTimestamp";
			this.lblCurrentTimestamp.Size = new System.Drawing.Size(155, 21);
			this.lblCurrentTimestamp.TabIndex = 1;
			this.lblCurrentTimestamp.Text = "1911/1/1 00:00:00";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 561);
			this.Controls.Add(this.tabControl1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.tbTimestamp)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tpSetting.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tpFootprint.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TrackBar tbTimestamp;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tpSetting;
		private System.Windows.Forms.TabPage tpFootprint;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.RichTextBox rtxtLog;
		private System.Windows.Forms.Label lblCurrentTimestamp;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtMapFile;
		private System.Windows.Forms.TextBox txtLogFile;
		private System.Windows.Forms.DateTimePicker dtpStart;
		private System.Windows.Forms.DateTimePicker dtpEnd;
		private System.Windows.Forms.ComboBox cbStart;
		private System.Windows.Forms.ComboBox cbEnd;
		private System.Windows.Forms.Button btnSelectLogFile;
		private System.Windows.Forms.Button btnSelectMapFile;
		private System.Windows.Forms.Button btnLoadSetting;
		private GLUI.GLUICtrl gluiCtrl1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel2;
	}
}

