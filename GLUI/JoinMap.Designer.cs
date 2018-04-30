namespace GLUI
{
    partial class JoinMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JoinMap));
            this.rbtnCenter = new System.Windows.Forms.RadioButton();
            this.rbtnMax = new System.Windows.Forms.RadioButton();
            this.rbtnMin = new System.Windows.Forms.RadioButton();
            this.btnRotateRight = new System.Windows.Forms.Button();
            this.btnRotateLeft = new System.Windows.Forms.Button();
            this.btnSelectRange = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.gboxSelectRange = new System.Windows.Forms.GroupBox();
            this.gboxTranslate = new System.Windows.Forms.GroupBox();
            this.rbtn1000mm = new System.Windows.Forms.RadioButton();
            this.rbtn1mm = new System.Windows.Forms.RadioButton();
            this.rbtn10mm = new System.Windows.Forms.RadioButton();
            this.rbtn100mm = new System.Windows.Forms.RadioButton();
            this.gboxRotate = new System.Windows.Forms.GroupBox();
            this.rbtn45deg = new System.Windows.Forms.RadioButton();
            this.rbtn01deg = new System.Windows.Forms.RadioButton();
            this.rbtn5deg = new System.Windows.Forms.RadioButton();
            this.rbtn1deg = new System.Windows.Forms.RadioButton();
            this.gboxSelectRange.SuspendLayout();
            this.gboxTranslate.SuspendLayout();
            this.gboxRotate.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbtnCenter
            // 
            this.rbtnCenter.AutoSize = true;
            this.rbtnCenter.Checked = true;
            this.rbtnCenter.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtnCenter.Location = new System.Drawing.Point(6, 21);
            this.rbtnCenter.Name = "rbtnCenter";
            this.rbtnCenter.Size = new System.Drawing.Size(132, 25);
            this.rbtnCenter.TabIndex = 11;
            this.rbtnCenter.TabStop = true;
            this.rbtnCenter.Text = "radioButton1";
            this.rbtnCenter.UseVisualStyleBackColor = true;
            // 
            // rbtnMax
            // 
            this.rbtnMax.AutoSize = true;
            this.rbtnMax.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtnMax.Location = new System.Drawing.Point(177, 21);
            this.rbtnMax.Name = "rbtnMax";
            this.rbtnMax.Size = new System.Drawing.Size(132, 25);
            this.rbtnMax.TabIndex = 12;
            this.rbtnMax.Text = "radioButton2";
            this.rbtnMax.UseVisualStyleBackColor = true;
            // 
            // rbtnMin
            // 
            this.rbtnMin.AutoSize = true;
            this.rbtnMin.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtnMin.Location = new System.Drawing.Point(177, 52);
            this.rbtnMin.Name = "rbtnMin";
            this.rbtnMin.Size = new System.Drawing.Size(132, 25);
            this.rbtnMin.TabIndex = 13;
            this.rbtnMin.Text = "radioButton3";
            this.rbtnMin.UseVisualStyleBackColor = true;
            // 
            // btnRotateRight
            // 
            this.btnRotateRight.Image = global::GLUI.Properties.Resources.RotateRight;
            this.btnRotateRight.Location = new System.Drawing.Point(184, 98);
            this.btnRotateRight.Name = "btnRotateRight";
            this.btnRotateRight.Size = new System.Drawing.Size(80, 80);
            this.btnRotateRight.TabIndex = 15;
            this.btnRotateRight.UseVisualStyleBackColor = true;
            this.btnRotateRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RotateBtnMouseDown);
            // 
            // btnRotateLeft
            // 
            this.btnRotateLeft.Image = global::GLUI.Properties.Resources.RotateLeft;
            this.btnRotateLeft.Location = new System.Drawing.Point(12, 98);
            this.btnRotateLeft.Name = "btnRotateLeft";
            this.btnRotateLeft.Size = new System.Drawing.Size(80, 80);
            this.btnRotateLeft.TabIndex = 14;
            this.btnRotateLeft.UseVisualStyleBackColor = true;
            this.btnRotateLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RotateBtnMouseDown);
            // 
            // btnSelectRange
            // 
            this.btnSelectRange.Image = global::GLUI.Properties.Resources.SelectRange;
            this.btnSelectRange.Location = new System.Drawing.Point(98, 12);
            this.btnSelectRange.Name = "btnSelectRange";
            this.btnSelectRange.Size = new System.Drawing.Size(80, 80);
            this.btnSelectRange.TabIndex = 9;
            this.btnSelectRange.UseVisualStyleBackColor = true;
            this.btnSelectRange.Click += new System.EventHandler(this.btnSelectRange_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Image = global::GLUI.Properties.Resources.Open;
            this.btnLoad.Location = new System.Drawing.Point(12, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(80, 80);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(598, 184);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 80);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDone
            // 
            this.btnDone.Image = ((System.Drawing.Image)(resources.GetObject("btnDone.Image")));
            this.btnDone.Location = new System.Drawing.Point(512, 184);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(80, 80);
            this.btnDone.TabIndex = 6;
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // btnUp
            // 
            this.btnUp.Image = global::GLUI.Properties.Resources.ArrowUp;
            this.btnUp.Location = new System.Drawing.Point(98, 98);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(80, 80);
            this.btnUp.TabIndex = 16;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TranslateBtnMouseDown);
            // 
            // btnLeft
            // 
            this.btnLeft.Image = global::GLUI.Properties.Resources.ArrowLeft;
            this.btnLeft.Location = new System.Drawing.Point(12, 184);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(80, 80);
            this.btnLeft.TabIndex = 17;
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TranslateBtnMouseDown);
            // 
            // btnDown
            // 
            this.btnDown.Image = global::GLUI.Properties.Resources.ArrowDown;
            this.btnDown.Location = new System.Drawing.Point(98, 184);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(80, 80);
            this.btnDown.TabIndex = 18;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TranslateBtnMouseDown);
            // 
            // btnRight
            // 
            this.btnRight.Image = global::GLUI.Properties.Resources.ArrowRight;
            this.btnRight.Location = new System.Drawing.Point(184, 184);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(80, 80);
            this.btnRight.TabIndex = 19;
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TranslateBtnMouseDown);
            // 
            // gboxSelectRange
            // 
            this.gboxSelectRange.Controls.Add(this.rbtnCenter);
            this.gboxSelectRange.Controls.Add(this.rbtnMax);
            this.gboxSelectRange.Controls.Add(this.rbtnMin);
            this.gboxSelectRange.Location = new System.Drawing.Point(184, 12);
            this.gboxSelectRange.Name = "gboxSelectRange";
            this.gboxSelectRange.Size = new System.Drawing.Size(494, 80);
            this.gboxSelectRange.TabIndex = 20;
            this.gboxSelectRange.TabStop = false;
            this.gboxSelectRange.Text = "groupBox1";
            // 
            // gboxTranslate
            // 
            this.gboxTranslate.Controls.Add(this.rbtn1000mm);
            this.gboxTranslate.Controls.Add(this.rbtn1mm);
            this.gboxTranslate.Controls.Add(this.rbtn10mm);
            this.gboxTranslate.Controls.Add(this.rbtn100mm);
            this.gboxTranslate.Location = new System.Drawing.Point(270, 183);
            this.gboxTranslate.Name = "gboxTranslate";
            this.gboxTranslate.Size = new System.Drawing.Size(236, 80);
            this.gboxTranslate.TabIndex = 21;
            this.gboxTranslate.TabStop = false;
            this.gboxTranslate.Text = "groupBox1";
            // 
            // rbtn1000mm
            // 
            this.rbtn1000mm.AutoSize = true;
            this.rbtn1000mm.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtn1000mm.Location = new System.Drawing.Point(6, 49);
            this.rbtn1000mm.Name = "rbtn1000mm";
            this.rbtn1000mm.Size = new System.Drawing.Size(68, 25);
            this.rbtn1000mm.TabIndex = 14;
            this.rbtn1000mm.Text = "1000";
            this.rbtn1000mm.UseVisualStyleBackColor = true;
            // 
            // rbtn1mm
            // 
            this.rbtn1mm.AutoSize = true;
            this.rbtn1mm.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtn1mm.Location = new System.Drawing.Point(6, 21);
            this.rbtn1mm.Name = "rbtn1mm";
            this.rbtn1mm.Size = new System.Drawing.Size(38, 25);
            this.rbtn1mm.TabIndex = 11;
            this.rbtn1mm.Text = "1";
            this.rbtn1mm.UseVisualStyleBackColor = true;
            // 
            // rbtn10mm
            // 
            this.rbtn10mm.AutoSize = true;
            this.rbtn10mm.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtn10mm.Location = new System.Drawing.Point(80, 21);
            this.rbtn10mm.Name = "rbtn10mm";
            this.rbtn10mm.Size = new System.Drawing.Size(48, 25);
            this.rbtn10mm.TabIndex = 12;
            this.rbtn10mm.Text = "10";
            this.rbtn10mm.UseVisualStyleBackColor = true;
            // 
            // rbtn100mm
            // 
            this.rbtn100mm.AutoSize = true;
            this.rbtn100mm.Checked = true;
            this.rbtn100mm.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtn100mm.Location = new System.Drawing.Point(164, 21);
            this.rbtn100mm.Name = "rbtn100mm";
            this.rbtn100mm.Size = new System.Drawing.Size(58, 25);
            this.rbtn100mm.TabIndex = 13;
            this.rbtn100mm.TabStop = true;
            this.rbtn100mm.Text = "100";
            this.rbtn100mm.UseVisualStyleBackColor = true;
            // 
            // gboxRotate
            // 
            this.gboxRotate.Controls.Add(this.rbtn45deg);
            this.gboxRotate.Controls.Add(this.rbtn01deg);
            this.gboxRotate.Controls.Add(this.rbtn5deg);
            this.gboxRotate.Controls.Add(this.rbtn1deg);
            this.gboxRotate.Location = new System.Drawing.Point(270, 98);
            this.gboxRotate.Name = "gboxRotate";
            this.gboxRotate.Size = new System.Drawing.Size(408, 80);
            this.gboxRotate.TabIndex = 22;
            this.gboxRotate.TabStop = false;
            this.gboxRotate.Text = "groupBox1";
            // 
            // rbtn45deg
            // 
            this.rbtn45deg.AutoSize = true;
            this.rbtn45deg.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtn45deg.Location = new System.Drawing.Point(242, 21);
            this.rbtn45deg.Name = "rbtn45deg";
            this.rbtn45deg.Size = new System.Drawing.Size(48, 25);
            this.rbtn45deg.TabIndex = 14;
            this.rbtn45deg.Text = "45";
            this.rbtn45deg.UseVisualStyleBackColor = true;
            // 
            // rbtn01deg
            // 
            this.rbtn01deg.AutoSize = true;
            this.rbtn01deg.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtn01deg.Location = new System.Drawing.Point(6, 21);
            this.rbtn01deg.Name = "rbtn01deg";
            this.rbtn01deg.Size = new System.Drawing.Size(53, 25);
            this.rbtn01deg.TabIndex = 11;
            this.rbtn01deg.Text = "0.1";
            this.rbtn01deg.UseVisualStyleBackColor = true;
            // 
            // rbtn5deg
            // 
            this.rbtn5deg.AutoSize = true;
            this.rbtn5deg.Checked = true;
            this.rbtn5deg.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtn5deg.Location = new System.Drawing.Point(164, 21);
            this.rbtn5deg.Name = "rbtn5deg";
            this.rbtn5deg.Size = new System.Drawing.Size(38, 25);
            this.rbtn5deg.TabIndex = 12;
            this.rbtn5deg.TabStop = true;
            this.rbtn5deg.Text = "5";
            this.rbtn5deg.UseVisualStyleBackColor = true;
            // 
            // rbtn1deg
            // 
            this.rbtn1deg.AutoSize = true;
            this.rbtn1deg.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtn1deg.Location = new System.Drawing.Point(94, 21);
            this.rbtn1deg.Name = "rbtn1deg";
            this.rbtn1deg.Size = new System.Drawing.Size(38, 25);
            this.rbtn1deg.TabIndex = 13;
            this.rbtn1deg.Text = "1";
            this.rbtn1deg.UseVisualStyleBackColor = true;
            // 
            // JoinMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 275);
            this.Controls.Add(this.gboxRotate);
            this.Controls.Add(this.gboxTranslate);
            this.Controls.Add(this.gboxSelectRange);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnRotateRight);
            this.Controls.Add(this.btnRotateLeft);
            this.Controls.Add(this.btnSelectRange);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDone);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "JoinMap";
            this.Text = "JoinMap";
            this.gboxSelectRange.ResumeLayout(false);
            this.gboxSelectRange.PerformLayout();
            this.gboxTranslate.ResumeLayout(false);
            this.gboxTranslate.PerformLayout();
            this.gboxRotate.ResumeLayout(false);
            this.gboxRotate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSelectRange;
        private System.Windows.Forms.RadioButton rbtnCenter;
        private System.Windows.Forms.RadioButton rbtnMax;
        private System.Windows.Forms.RadioButton rbtnMin;
        private System.Windows.Forms.Button btnRotateLeft;
        private System.Windows.Forms.Button btnRotateRight;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.GroupBox gboxSelectRange;
        private System.Windows.Forms.GroupBox gboxTranslate;
        private System.Windows.Forms.RadioButton rbtn1mm;
        private System.Windows.Forms.RadioButton rbtn10mm;
        private System.Windows.Forms.RadioButton rbtn100mm;
        private System.Windows.Forms.GroupBox gboxRotate;
        private System.Windows.Forms.RadioButton rbtn01deg;
        private System.Windows.Forms.RadioButton rbtn5deg;
        private System.Windows.Forms.RadioButton rbtn1deg;
        private System.Windows.Forms.RadioButton rbtn45deg;
        private System.Windows.Forms.RadioButton rbtn1000mm;
    }
}