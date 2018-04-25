namespace GLUITest
{
    partial class frmTest
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
            this.GLUI = new GLUI.GLUICtrl();
            this.SuspendLayout();
            // 
            // GLUI
            // 
            this.GLUI.AllowObjectMenu = true;
            this.GLUI.AllowUndoMenu = true;
            this.GLUI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GLUI.Location = new System.Drawing.Point(12, 12);
            this.GLUI.Name = "GLUI";
            this.GLUI.ShowAxis = true;
            this.GLUI.ShowGrid = true;
            this.GLUI.Size = new System.Drawing.Size(260, 237);
            this.GLUI.TabIndex = 0;
            this.GLUI.Zoom = 10D;
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.GLUI);
            this.Name = "frmTest";
            this.Text = "GLUITest";
            this.ResumeLayout(false);

        }

        #endregion

        private GLUI.GLUICtrl GLUI;
    }
}

