namespace ARvpn
{
    partial class MainForm
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
            this.btnParam = new System.Windows.Forms.Button();
            this.btnVpnConnect = new System.Windows.Forms.Button();
            this.cmbItems = new System.Windows.Forms.ComboBox();
            this.rtxtLog = new System.Windows.Forms.RichTextBox();
            this.prBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnParam
            // 
            this.btnParam.Location = new System.Drawing.Point(12, 39);
            this.btnParam.Name = "btnParam";
            this.btnParam.Size = new System.Drawing.Size(97, 23);
            this.btnParam.TabIndex = 2;
            this.btnParam.Text = "Настройки";
            this.btnParam.UseVisualStyleBackColor = true;
            this.btnParam.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnVpnConnect
            // 
            this.btnVpnConnect.Location = new System.Drawing.Point(134, 39);
            this.btnVpnConnect.Name = "btnVpnConnect";
            this.btnVpnConnect.Size = new System.Drawing.Size(111, 23);
            this.btnVpnConnect.TabIndex = 3;
            this.btnVpnConnect.Text = "Подключиться";
            this.btnVpnConnect.UseVisualStyleBackColor = true;
            this.btnVpnConnect.Click += new System.EventHandler(this.btnVpnConnect_Click);
            // 
            // cmbItems
            // 
            this.cmbItems.FormattingEnabled = true;
            this.cmbItems.Location = new System.Drawing.Point(12, 12);
            this.cmbItems.Name = "cmbItems";
            this.cmbItems.Size = new System.Drawing.Size(254, 21);
            this.cmbItems.TabIndex = 1;
            // 
            // rtxtLog
            // 
            this.rtxtLog.Location = new System.Drawing.Point(9, 68);
            this.rtxtLog.Name = "rtxtLog";
            this.rtxtLog.Size = new System.Drawing.Size(342, 130);
            this.rtxtLog.TabIndex = 4;
            this.rtxtLog.Text = "";
            // 
            // prBar
            // 
            this.prBar.Location = new System.Drawing.Point(266, 44);
            this.prBar.Name = "prBar";
            this.prBar.Size = new System.Drawing.Size(85, 14);
            this.prBar.TabIndex = 5;
            this.prBar.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 211);
            this.Controls.Add(this.prBar);
            this.Controls.Add(this.rtxtLog);
            this.Controls.Add(this.cmbItems);
            this.Controls.Add(this.btnVpnConnect);
            this.Controls.Add(this.btnParam);
            this.Name = "MainForm";
            this.Text = "Подключение VPN";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnParam;
        private System.Windows.Forms.Button btnVpnConnect;
        private System.Windows.Forms.ComboBox cmbItems;
        private System.Windows.Forms.RichTextBox rtxtLog;
        private System.Windows.Forms.ProgressBar prBar;
    }
}

