namespace udCompanySetting
{
    partial class frmSMS
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
            this.gbMain = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSMSEnable = new System.Windows.Forms.CheckBox();
            this.txtGateway = new System.Windows.Forms.TextBox();
            this.btnGateway = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.gbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.label1);
            this.gbMain.Controls.Add(this.chkSMSEnable);
            this.gbMain.Controls.Add(this.txtGateway);
            this.gbMain.Controls.Add(this.btnGateway);
            this.gbMain.Location = new System.Drawing.Point(9, 8);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(332, 93);
            this.gbMain.TabIndex = 0;
            this.gbMain.TabStop = false;
            this.gbMain.Text = "Setting";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "Select Gateway";
            // 
            // chkSMSEnable
            // 
            this.chkSMSEnable.AutoSize = true;
            this.chkSMSEnable.Location = new System.Drawing.Point(19, 21);
            this.chkSMSEnable.Name = "chkSMSEnable";
            this.chkSMSEnable.Size = new System.Drawing.Size(124, 17);
            this.chkSMSEnable.TabIndex = 0;
            this.chkSMSEnable.Text = "Enable Sms Sending";
            this.chkSMSEnable.UseVisualStyleBackColor = true;
            // 
            // txtGateway
            // 
            this.txtGateway.Location = new System.Drawing.Point(107, 53);
            this.txtGateway.Name = "txtGateway";
            this.txtGateway.Size = new System.Drawing.Size(185, 20);
            this.txtGateway.TabIndex = 1;
            // 
            // btnGateway
            // 
            this.btnGateway.Location = new System.Drawing.Point(298, 51);
            this.btnGateway.Name = "btnGateway";
            this.btnGateway.Size = new System.Drawing.Size(27, 23);
            this.btnGateway.TabIndex = 2;
            this.btnGateway.UseVisualStyleBackColor = true;
            this.btnGateway.Click += new System.EventHandler(this.btnGateway_Click);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(266, 107);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 1;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // frmSMS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 138);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.gbMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSMS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmSMS";
            this.Load += new System.EventHandler(this.frmSMS_Load);
            this.gbMain.ResumeLayout(false);
            this.gbMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.TextBox txtGateway;
        private System.Windows.Forms.Button btnGateway;
        private System.Windows.Forms.CheckBox chkSMSEnable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDone;
    }
}