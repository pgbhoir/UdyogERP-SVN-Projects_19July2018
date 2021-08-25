namespace udCompanySetting
{
    partial class frmPayroll
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
            this.chkEmpCode = new System.Windows.Forms.CheckBox();
            this.btnDone = new System.Windows.Forms.Button();
            this.gbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.chkEmpCode);
            this.gbMain.Location = new System.Drawing.Point(12, 12);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(260, 45);
            this.gbMain.TabIndex = 27;
            this.gbMain.TabStop = false;
            this.gbMain.Text = "Setting";
            // 
            // chkEmpCode
            // 
            this.chkEmpCode.AutoSize = true;
            this.chkEmpCode.Location = new System.Drawing.Point(6, 19);
            this.chkEmpCode.Name = "chkEmpCode";
            this.chkEmpCode.Size = new System.Drawing.Size(170, 17);
            this.chkEmpCode.TabIndex = 1;
            this.chkEmpCode.Text = "Enter employee code manually";
            this.chkEmpCode.UseVisualStyleBackColor = true;
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(197, 63);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 36;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // frmPayroll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 92);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.gbMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPayroll";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "demPayroll";
            this.Load += new System.EventHandler(this.frmPayroll_Load);
            this.gbMain.ResumeLayout(false);
            this.gbMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.CheckBox chkEmpCode;
        private System.Windows.Forms.Button btnDone;
    }
}