namespace GLUI
{
    partial class GLUICtrl
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.SharpGLCtrl = new SharpGL.OpenGLControl();
            ((System.ComponentModel.ISupportInitialize)(this.SharpGLCtrl)).BeginInit();
            this.SuspendLayout();
            // 
            // SharpGLCtrl
            // 
            this.SharpGLCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SharpGLCtrl.DrawFPS = true;
            this.SharpGLCtrl.Location = new System.Drawing.Point(0, 0);
            this.SharpGLCtrl.Name = "SharpGLCtrl";
            this.SharpGLCtrl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.SharpGLCtrl.RenderContextType = SharpGL.RenderContextType.NativeWindow;
            this.SharpGLCtrl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.SharpGLCtrl.Size = new System.Drawing.Size(150, 150);
            this.SharpGLCtrl.TabIndex = 0;
            this.SharpGLCtrl.OpenGLDraw += new SharpGL.RenderEventHandler(this.SharpGLCtrl_OpenGLDraw);
            this.SharpGLCtrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SharpGLCtrl_KeyDown);
            // 
            // GLUICtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SharpGLCtrl);
            this.Name = "GLUICtrl";
            ((System.ComponentModel.ISupportInitialize)(this.SharpGLCtrl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SharpGL.OpenGLControl SharpGLCtrl;
    }
}
