namespace TrafficControlTest
{
	partial class TrafficControlTestGUI
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
			this.gluiCtrl1 = new GLUI.GLUICtrl();
			this.SuspendLayout();
			// 
			// gluiCtrl1
			// 
			this.gluiCtrl1.AllowObjectMenu = true;
			this.gluiCtrl1.AllowUndoMenu = true;
			this.gluiCtrl1.Location = new System.Drawing.Point(13, 13);
			this.gluiCtrl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.gluiCtrl1.Name = "gluiCtrl1";
			this.gluiCtrl1.ShowAxis = true;
			this.gluiCtrl1.ShowGrid = true;
			this.gluiCtrl1.Size = new System.Drawing.Size(256, 227);
			this.gluiCtrl1.TabIndex = 0;
			this.gluiCtrl1.Zoom = 10D;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(282, 253);
			this.Controls.Add(this.gluiCtrl1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private GLUI.GLUICtrl gluiCtrl1;
	}
}

