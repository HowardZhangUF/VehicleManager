namespace TrafficControlTest.UserControl
{
	partial class UCLog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCLog));
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnAddSearchPage = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.btnAddSearchPage);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(850, 50);
			this.panel1.TabIndex = 0;
			// 
			// btnAddSearchPage
			// 
			this.btnAddSearchPage.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnAddSearchPage.FlatAppearance.BorderSize = 0;
			this.btnAddSearchPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAddSearchPage.Image = ((System.Drawing.Image)(resources.GetObject("btnAddSearchPage.Image")));
			this.btnAddSearchPage.Location = new System.Drawing.Point(800, 0);
			this.btnAddSearchPage.Name = "btnAddSearchPage";
			this.btnAddSearchPage.Size = new System.Drawing.Size(50, 50);
			this.btnAddSearchPage.TabIndex = 0;
			this.btnAddSearchPage.UseVisualStyleBackColor = true;
			this.btnAddSearchPage.Click += new System.EventHandler(this.btnAddSearchPage_Click);
			// 
			// panel2
			// 
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 50);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(850, 550);
			this.panel2.TabIndex = 1;
			// 
			// UCLog
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "UCLog";
			this.Size = new System.Drawing.Size(850, 600);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnAddSearchPage;
	}
}
