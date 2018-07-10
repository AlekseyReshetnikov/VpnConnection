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
            this.SuspendLayout();
            // 
            // btnParam
            // 
            this.btnParam.Location = new System.Drawing.Point(32, 58);
            this.btnParam.Name = "btnParam";
            this.btnParam.Size = new System.Drawing.Size(229, 23);
            this.btnParam.TabIndex = 2;
            this.btnParam.Text = "Настройки";
            this.btnParam.UseVisualStyleBackColor = true;
            this.btnParam.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnVpnConnect
            // 
            this.btnVpnConnect.Location = new System.Drawing.Point(32, 99);
            this.btnVpnConnect.Name = "btnVpnConnect";
            this.btnVpnConnect.Size = new System.Drawing.Size(229, 23);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 134);
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
    }
}

