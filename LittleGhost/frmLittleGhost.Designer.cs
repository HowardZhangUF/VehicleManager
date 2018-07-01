namespace LittleGhost
{
    partial class frmMain
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
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                if (server != null)
                    server.Dispose();
                if (client != null)
                    client.Dispose();
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
            this.components = new System.ComponentModel.Container();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.pageServer = new System.Windows.Forms.TabPage();
            this.btnServerSendFakePath = new System.Windows.Forms.Button();
            this.btnServerSendFakeStatus = new System.Windows.Forms.Button();
            this.chkServerSendByBytes = new System.Windows.Forms.CheckBox();
            this.txtServerSendData = new System.Windows.Forms.TextBox();
            this.btnServerSend = new System.Windows.Forms.Button();
            this.cmbRemoteList = new System.Windows.Forms.ComboBox();
            this.btnListening = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.nmrServerPort = new System.Windows.Forms.NumericUpDown();
            this.pageClient = new System.Windows.Forms.TabPage();
            this.btnClientSendFakePath = new System.Windows.Forms.Button();
            this.btnClientSendFakeStatus = new System.Windows.Forms.Button();
            this.chkClientSendByBytes = new System.Windows.Forms.CheckBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtClientSendData = new System.Windows.Forms.TextBox();
            this.btnClientSend = new System.Windows.Forms.Button();
            this.txtClientIP = new System.Windows.Forms.TextBox();
            this.nmrClientPort = new System.Windows.Forms.NumericUpDown();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.tUpdateMessage = new System.Windows.Forms.Timer(this.components);
            this.tabControl.SuspendLayout();
            this.pageServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrServerPort)).BeginInit();
            this.pageClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrClientPort)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.pageServer);
            this.tabControl.Controls.Add(this.pageClient);
            this.tabControl.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(927, 180);
            this.tabControl.TabIndex = 0;
            // 
            // pageServer
            // 
            this.pageServer.Controls.Add(this.btnServerSendFakePath);
            this.pageServer.Controls.Add(this.btnServerSendFakeStatus);
            this.pageServer.Controls.Add(this.chkServerSendByBytes);
            this.pageServer.Controls.Add(this.txtServerSendData);
            this.pageServer.Controls.Add(this.btnServerSend);
            this.pageServer.Controls.Add(this.cmbRemoteList);
            this.pageServer.Controls.Add(this.btnListening);
            this.pageServer.Controls.Add(this.label1);
            this.pageServer.Controls.Add(this.nmrServerPort);
            this.pageServer.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.pageServer.Location = new System.Drawing.Point(4, 37);
            this.pageServer.Name = "pageServer";
            this.pageServer.Padding = new System.Windows.Forms.Padding(3);
            this.pageServer.Size = new System.Drawing.Size(919, 139);
            this.pageServer.TabIndex = 0;
            this.pageServer.Text = "Server";
            this.pageServer.UseVisualStyleBackColor = true;
            // 
            // btnServerSendFakePath
            // 
            this.btnServerSendFakePath.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnServerSendFakePath.Location = new System.Drawing.Point(696, 92);
            this.btnServerSendFakePath.Name = "btnServerSendFakePath";
            this.btnServerSendFakePath.Size = new System.Drawing.Size(125, 40);
            this.btnServerSendFakePath.TabIndex = 8;
            this.btnServerSendFakePath.Text = "Fake Path";
            this.btnServerSendFakePath.UseVisualStyleBackColor = true;
            this.btnServerSendFakePath.Click += new System.EventHandler(this.btnServerSendFakePath_Click);
            // 
            // btnServerSendFakeStatus
            // 
            this.btnServerSendFakeStatus.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnServerSendFakeStatus.Location = new System.Drawing.Point(565, 93);
            this.btnServerSendFakeStatus.Name = "btnServerSendFakeStatus";
            this.btnServerSendFakeStatus.Size = new System.Drawing.Size(125, 40);
            this.btnServerSendFakeStatus.TabIndex = 7;
            this.btnServerSendFakeStatus.Text = "Fake Status";
            this.btnServerSendFakeStatus.UseVisualStyleBackColor = true;
            this.btnServerSendFakeStatus.Click += new System.EventHandler(this.btnServerSendFakeStatus_Click);
            // 
            // chkServerSendByBytes
            // 
            this.chkServerSendByBytes.AutoSize = true;
            this.chkServerSendByBytes.Location = new System.Drawing.Point(434, 53);
            this.chkServerSendByBytes.Name = "chkServerSendByBytes";
            this.chkServerSendByBytes.Size = new System.Drawing.Size(192, 32);
            this.chkServerSendByBytes.TabIndex = 6;
            this.chkServerSendByBytes.Text = "Send By Bytes";
            this.chkServerSendByBytes.UseVisualStyleBackColor = true;
            // 
            // txtServerSendData
            // 
            this.txtServerSendData.Location = new System.Drawing.Point(11, 93);
            this.txtServerSendData.Name = "txtServerSendData";
            this.txtServerSendData.Size = new System.Drawing.Size(417, 40);
            this.txtServerSendData.TabIndex = 5;
            // 
            // btnServerSend
            // 
            this.btnServerSend.Location = new System.Drawing.Point(434, 93);
            this.btnServerSend.Name = "btnServerSend";
            this.btnServerSend.Size = new System.Drawing.Size(125, 40);
            this.btnServerSend.TabIndex = 4;
            this.btnServerSend.Text = "Send";
            this.btnServerSend.UseVisualStyleBackColor = true;
            this.btnServerSend.Click += new System.EventHandler(this.btnServerSend_Click);
            // 
            // cmbRemoteList
            // 
            this.cmbRemoteList.FormattingEnabled = true;
            this.cmbRemoteList.Location = new System.Drawing.Point(11, 52);
            this.cmbRemoteList.Name = "cmbRemoteList";
            this.cmbRemoteList.Size = new System.Drawing.Size(417, 35);
            this.cmbRemoteList.TabIndex = 3;
            // 
            // btnListening
            // 
            this.btnListening.Location = new System.Drawing.Point(194, 6);
            this.btnListening.Name = "btnListening";
            this.btnListening.Size = new System.Drawing.Size(234, 40);
            this.btnListening.TabIndex = 2;
            this.btnListening.UseVisualStyleBackColor = true;
            this.btnListening.Click += new System.EventHandler(this.btnListening_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 28);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port";
            // 
            // nmrServerPort
            // 
            this.nmrServerPort.Location = new System.Drawing.Point(68, 6);
            this.nmrServerPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nmrServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmrServerPort.Name = "nmrServerPort";
            this.nmrServerPort.Size = new System.Drawing.Size(120, 40);
            this.nmrServerPort.TabIndex = 0;
            this.nmrServerPort.Value = new decimal(new int[] {
            8051,
            0,
            0,
            0});
            // 
            // pageClient
            // 
            this.pageClient.Controls.Add(this.btnClientSendFakePath);
            this.pageClient.Controls.Add(this.btnClientSendFakeStatus);
            this.pageClient.Controls.Add(this.chkClientSendByBytes);
            this.pageClient.Controls.Add(this.btnConnect);
            this.pageClient.Controls.Add(this.txtClientSendData);
            this.pageClient.Controls.Add(this.btnClientSend);
            this.pageClient.Controls.Add(this.txtClientIP);
            this.pageClient.Controls.Add(this.nmrClientPort);
            this.pageClient.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.pageClient.Location = new System.Drawing.Point(4, 37);
            this.pageClient.Name = "pageClient";
            this.pageClient.Padding = new System.Windows.Forms.Padding(3);
            this.pageClient.Size = new System.Drawing.Size(919, 139);
            this.pageClient.TabIndex = 1;
            this.pageClient.Text = "Client";
            this.pageClient.UseVisualStyleBackColor = true;
            // 
            // btnClientSendFakePath
            // 
            this.btnClientSendFakePath.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnClientSendFakePath.Location = new System.Drawing.Point(691, 50);
            this.btnClientSendFakePath.Name = "btnClientSendFakePath";
            this.btnClientSendFakePath.Size = new System.Drawing.Size(125, 40);
            this.btnClientSendFakePath.TabIndex = 12;
            this.btnClientSendFakePath.Text = "Fake Path";
            this.btnClientSendFakePath.UseVisualStyleBackColor = true;
            this.btnClientSendFakePath.Click += new System.EventHandler(this.btnClientSendFakePath_Click);
            // 
            // btnClientSendFakeStatus
            // 
            this.btnClientSendFakeStatus.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnClientSendFakeStatus.Location = new System.Drawing.Point(560, 51);
            this.btnClientSendFakeStatus.Name = "btnClientSendFakeStatus";
            this.btnClientSendFakeStatus.Size = new System.Drawing.Size(125, 40);
            this.btnClientSendFakeStatus.TabIndex = 11;
            this.btnClientSendFakeStatus.Text = "Fake Status";
            this.btnClientSendFakeStatus.UseVisualStyleBackColor = true;
            this.btnClientSendFakeStatus.Click += new System.EventHandler(this.btnClientSendFakeStatus_Click);
            // 
            // chkClientSendByBytes
            // 
            this.chkClientSendByBytes.AutoSize = true;
            this.chkClientSendByBytes.Location = new System.Drawing.Point(560, 15);
            this.chkClientSendByBytes.Name = "chkClientSendByBytes";
            this.chkClientSendByBytes.Size = new System.Drawing.Size(192, 32);
            this.chkClientSendByBytes.TabIndex = 10;
            this.chkClientSendByBytes.Text = "Send By Bytes";
            this.chkClientSendByBytes.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(390, 7);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(164, 40);
            this.btnConnect.TabIndex = 9;
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtClientSendData
            // 
            this.txtClientSendData.Location = new System.Drawing.Point(6, 52);
            this.txtClientSendData.Name = "txtClientSendData";
            this.txtClientSendData.Size = new System.Drawing.Size(417, 40);
            this.txtClientSendData.TabIndex = 8;
            // 
            // btnClientSend
            // 
            this.btnClientSend.Location = new System.Drawing.Point(429, 52);
            this.btnClientSend.Name = "btnClientSend";
            this.btnClientSend.Size = new System.Drawing.Size(125, 40);
            this.btnClientSend.TabIndex = 7;
            this.btnClientSend.Text = "Send";
            this.btnClientSend.UseVisualStyleBackColor = true;
            this.btnClientSend.Click += new System.EventHandler(this.btnClientSend_Click);
            // 
            // txtClientIP
            // 
            this.txtClientIP.Location = new System.Drawing.Point(6, 6);
            this.txtClientIP.Name = "txtClientIP";
            this.txtClientIP.Size = new System.Drawing.Size(252, 40);
            this.txtClientIP.TabIndex = 6;
            this.txtClientIP.Text = "127.0.0.1";
            // 
            // nmrClientPort
            // 
            this.nmrClientPort.Location = new System.Drawing.Point(264, 7);
            this.nmrClientPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nmrClientPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmrClientPort.Name = "nmrClientPort";
            this.nmrClientPort.Size = new System.Drawing.Size(120, 40);
            this.nmrClientPort.TabIndex = 1;
            this.nmrClientPort.Value = new decimal(new int[] {
            8051,
            0,
            0,
            0});
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.BackColor = System.Drawing.SystemColors.InfoText;
            this.txtMessage.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtMessage.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.txtMessage.Location = new System.Drawing.Point(16, 198);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMessage.Size = new System.Drawing.Size(919, 252);
            this.txtMessage.TabIndex = 1;
            // 
            // tUpdateMessage
            // 
            this.tUpdateMessage.Enabled = true;
            this.tUpdateMessage.Interval = 20;
            this.tUpdateMessage.Tick += new System.EventHandler(this.tUpdateMessage_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 462);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.tabControl);
            this.Name = "frmMain";
            this.Text = "Little Ghost";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.pageServer.ResumeLayout(false);
            this.pageServer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrServerPort)).EndInit();
            this.pageClient.ResumeLayout(false);
            this.pageClient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrClientPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage pageServer;
        private System.Windows.Forms.TabPage pageClient;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nmrServerPort;
        private System.Windows.Forms.Button btnListening;
        private System.Windows.Forms.Button btnServerSend;
        private System.Windows.Forms.ComboBox cmbRemoteList;
        private System.Windows.Forms.TextBox txtServerSendData;
        private System.Windows.Forms.Timer tUpdateMessage;
        private System.Windows.Forms.TextBox txtClientIP;
        private System.Windows.Forms.NumericUpDown nmrClientPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtClientSendData;
        private System.Windows.Forms.Button btnClientSend;
        private System.Windows.Forms.CheckBox chkServerSendByBytes;
        private System.Windows.Forms.CheckBox chkClientSendByBytes;
        private System.Windows.Forms.Button btnServerSendFakePath;
        private System.Windows.Forms.Button btnServerSendFakeStatus;
        private System.Windows.Forms.Button btnClientSendFakePath;
        private System.Windows.Forms.Button btnClientSendFakeStatus;
    }
}

