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
			this.gluiCtrl1.Size = new System.Drawing.Size(774, 424);
			this.gluiCtrl1.TabIndex = 1;
			this.gluiCtrl1.Zoom = 10D;
			this.gluiCtrl1.LoadMapEvent += new GLUI.LoadMapEvent(this.gluiCtrl1_LoadMapEvent);
			// 
			// VehicleManagerGUI
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.gluiCtrl1);
			this.Name = "VehicleManagerGUI";
			this.Text = "VehicleManagerGUI";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VehicleManagerGUI_FormClosing);
			this.Load += new System.EventHandler(this.VehicleManagerGUI_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private GLUI.GLUICtrl gluiCtrl1;
	}
}