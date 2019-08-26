namespace TrafficControlTest.UserInterface
{
	partial class VehicleManagerGUI
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.gluiCtrl1 = new GLUI.GLUICtrl();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.txtInterveneMovingBuffer = new System.Windows.Forms.TextBox();
			this.btnInterveneInsertMovingBuffer = new System.Windows.Forms.Button();
			this.btnInterveneRemoveMovingBuffer = new System.Windows.Forms.Button();
			this.btnIntervenePauseMoving = new System.Windows.Forms.Button();
			this.btnInterveneResumeMoving = new System.Windows.Forms.Button();
			this.cbVehicleList = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gluiCtrl1
			// 
			this.gluiCtrl1.AllowObjectMenu = true;
			this.gluiCtrl1.AllowUndoMenu = true;
			this.gluiCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gluiCtrl1.Location = new System.Drawing.Point(13, 13);
			this.gluiCtrl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.gluiCtrl1.Name = "gluiCtrl1";
			this.gluiCtrl1.ShowAxis = true;
			this.gluiCtrl1.ShowGrid = true;
			this.gluiCtrl1.Size = new System.Drawing.Size(646, 424);
			this.gluiCtrl1.TabIndex = 1;
			this.gluiCtrl1.Zoom = 10D;
			this.gluiCtrl1.LoadMapEvent += new GLUI.LoadMapEvent(this.gluiCtrl1_LoadMapEvent);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.LightSkyBlue;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtInterveneMovingBuffer, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnInterveneInsertMovingBuffer, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.btnInterveneRemoveMovingBuffer, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.btnIntervenePauseMoving, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.btnInterveneResumeMoving, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.cbVehicleList, 0, 1);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(666, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 7;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(244, 426);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(238, 67);
			this.label1.TabIndex = 0;
			this.label1.Text = "Intervene Command";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtInterveneMovingBuffer
			// 
			this.txtInterveneMovingBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtInterveneMovingBuffer.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtInterveneMovingBuffer.Location = new System.Drawing.Point(3, 112);
			this.txtInterveneMovingBuffer.Name = "txtInterveneMovingBuffer";
			this.txtInterveneMovingBuffer.Size = new System.Drawing.Size(238, 40);
			this.txtInterveneMovingBuffer.TabIndex = 1;
			this.txtInterveneMovingBuffer.Text = "-9000,7000";
			// 
			// btnInterveneInsertMovingBuffer
			// 
			this.btnInterveneInsertMovingBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnInterveneInsertMovingBuffer.Location = new System.Drawing.Point(3, 157);
			this.btnInterveneInsertMovingBuffer.Name = "btnInterveneInsertMovingBuffer";
			this.btnInterveneInsertMovingBuffer.Size = new System.Drawing.Size(238, 61);
			this.btnInterveneInsertMovingBuffer.TabIndex = 2;
			this.btnInterveneInsertMovingBuffer.Text = "Insert Moving Buffer";
			this.btnInterveneInsertMovingBuffer.UseVisualStyleBackColor = true;
			this.btnInterveneInsertMovingBuffer.Click += new System.EventHandler(this.btnInterveneInsertMovingBuffer_Click);
			// 
			// btnInterveneRemoveMovingBuffer
			// 
			this.btnInterveneRemoveMovingBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnInterveneRemoveMovingBuffer.Location = new System.Drawing.Point(3, 224);
			this.btnInterveneRemoveMovingBuffer.Name = "btnInterveneRemoveMovingBuffer";
			this.btnInterveneRemoveMovingBuffer.Size = new System.Drawing.Size(238, 61);
			this.btnInterveneRemoveMovingBuffer.TabIndex = 2;
			this.btnInterveneRemoveMovingBuffer.Text = "Remove Moving Buffer";
			this.btnInterveneRemoveMovingBuffer.UseVisualStyleBackColor = true;
			this.btnInterveneRemoveMovingBuffer.Click += new System.EventHandler(this.btnInterveneRemoveMovingBuffer_Click);
			// 
			// btnIntervenePauseMoving
			// 
			this.btnIntervenePauseMoving.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnIntervenePauseMoving.Location = new System.Drawing.Point(3, 291);
			this.btnIntervenePauseMoving.Name = "btnIntervenePauseMoving";
			this.btnIntervenePauseMoving.Size = new System.Drawing.Size(238, 61);
			this.btnIntervenePauseMoving.TabIndex = 2;
			this.btnIntervenePauseMoving.Text = "Pause Moving";
			this.btnIntervenePauseMoving.UseVisualStyleBackColor = true;
			this.btnIntervenePauseMoving.Click += new System.EventHandler(this.btnIntervenePauseMoving_Click);
			// 
			// btnInterveneResumeMoving
			// 
			this.btnInterveneResumeMoving.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnInterveneResumeMoving.Location = new System.Drawing.Point(3, 358);
			this.btnInterveneResumeMoving.Name = "btnInterveneResumeMoving";
			this.btnInterveneResumeMoving.Size = new System.Drawing.Size(238, 65);
			this.btnInterveneResumeMoving.TabIndex = 2;
			this.btnInterveneResumeMoving.Text = "Resume Moving";
			this.btnInterveneResumeMoving.UseVisualStyleBackColor = true;
			this.btnInterveneResumeMoving.Click += new System.EventHandler(this.btnInterveneResumeMoving_Click);
			// 
			// cbVehicleList
			// 
			this.cbVehicleList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbVehicleList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVehicleList.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.cbVehicleList.FormattingEnabled = true;
			this.cbVehicleList.Location = new System.Drawing.Point(3, 70);
			this.cbVehicleList.Name = "cbVehicleList";
			this.cbVehicleList.Size = new System.Drawing.Size(238, 35);
			this.cbVehicleList.TabIndex = 3;
			// 
			// VehicleManagerGUI
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(922, 450);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.gluiCtrl1);
			this.Name = "VehicleManagerGUI";
			this.Text = "VehicleManagerGUI";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VehicleManagerGUI_FormClosing);
			this.Load += new System.EventHandler(this.VehicleManagerGUI_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private GLUI.GLUICtrl gluiCtrl1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtInterveneMovingBuffer;
		private System.Windows.Forms.Button btnInterveneInsertMovingBuffer;
		private System.Windows.Forms.Button btnInterveneRemoveMovingBuffer;
		private System.Windows.Forms.Button btnIntervenePauseMoving;
		private System.Windows.Forms.Button btnInterveneResumeMoving;
		private System.Windows.Forms.ComboBox cbVehicleList;
	}
}