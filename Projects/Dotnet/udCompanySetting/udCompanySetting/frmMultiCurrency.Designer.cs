namespace udCompanySetting
{
    partial class frmMultiCurrency
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
            this.txtRtDiffAc = new System.Windows.Forms.TextBox();
            this.btnRtDiffAc = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.chkPtGdRnd = new System.Windows.Forms.CheckBox();
            this.chkPtTrnRnd = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkStGdRnd = new System.Windows.Forms.CheckBox();
            this.chkStTrnRnd = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDone = new System.Windows.Forms.Button();
            this.gbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.txtRtDiffAc);
            this.gbMain.Controls.Add(this.btnRtDiffAc);
            this.gbMain.Controls.Add(this.label4);
            this.gbMain.Controls.Add(this.chkPtGdRnd);
            this.gbMain.Controls.Add(this.chkPtTrnRnd);
            this.gbMain.Controls.Add(this.label6);
            this.gbMain.Controls.Add(this.label5);
            this.gbMain.Controls.Add(this.chkStGdRnd);
            this.gbMain.Controls.Add(this.chkStTrnRnd);
            this.gbMain.Controls.Add(this.label2);
            this.gbMain.Controls.Add(this.label1);
            this.gbMain.Location = new System.Drawing.Point(9, 10);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(485, 110);
            this.gbMain.TabIndex = 0;
            this.gbMain.TabStop = false;
            this.gbMain.Text = "Roundoff";
            // 
            // txtRtDiffAc
            // 
            this.txtRtDiffAc.Location = new System.Drawing.Point(171, 75);
            this.txtRtDiffAc.Name = "txtRtDiffAc";
            this.txtRtDiffAc.Size = new System.Drawing.Size(273, 20);
            this.txtRtDiffAc.TabIndex = 4;
            // 
            // btnRtDiffAc
            // 
            this.btnRtDiffAc.Location = new System.Drawing.Point(450, 73);
            this.btnRtDiffAc.Name = "btnRtDiffAc";
            this.btnRtDiffAc.Size = new System.Drawing.Size(27, 23);
            this.btnRtDiffAc.TabIndex = 5;
            this.btnRtDiffAc.UseVisualStyleBackColor = true;
            this.btnRtDiffAc.Click += new System.EventHandler(this.btnRtDiffAc_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 33;
            this.label4.Text = "Rate Difference A/c";
            // 
            // chkPtGdRnd
            // 
            this.chkPtGdRnd.AutoSize = true;
            this.chkPtGdRnd.Location = new System.Drawing.Point(249, 55);
            this.chkPtGdRnd.Name = "chkPtGdRnd";
            this.chkPtGdRnd.Size = new System.Drawing.Size(15, 14);
            this.chkPtGdRnd.TabIndex = 3;
            this.chkPtGdRnd.UseVisualStyleBackColor = true;
            // 
            // chkPtTrnRnd
            // 
            this.chkPtTrnRnd.AutoSize = true;
            this.chkPtTrnRnd.Location = new System.Drawing.Point(249, 37);
            this.chkPtTrnRnd.Name = "chkPtTrnRnd";
            this.chkPtTrnRnd.Size = new System.Drawing.Size(15, 14);
            this.chkPtTrnRnd.TabIndex = 1;
            this.chkPtTrnRnd.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(219, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "Purchases";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(157, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "Sales";
            // 
            // chkStGdRnd
            // 
            this.chkStGdRnd.AutoSize = true;
            this.chkStGdRnd.Location = new System.Drawing.Point(171, 55);
            this.chkStGdRnd.Name = "chkStGdRnd";
            this.chkStGdRnd.Size = new System.Drawing.Size(15, 14);
            this.chkStGdRnd.TabIndex = 2;
            this.chkStGdRnd.UseVisualStyleBackColor = true;
            // 
            // chkStTrnRnd
            // 
            this.chkStTrnRnd.AutoSize = true;
            this.chkStTrnRnd.Location = new System.Drawing.Point(171, 37);
            this.chkStTrnRnd.Name = "chkStTrnRnd";
            this.chkStTrnRnd.Size = new System.Drawing.Size(15, 14);
            this.chkStTrnRnd.TabIndex = 0;
            this.chkStTrnRnd.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Goodswise Round-Off";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Transactionwise Round-Off";
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(419, 125);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 1;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // frmMultiCurrency
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 154);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.gbMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMultiCurrency";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMultiCurrency";
            this.Load += new System.EventHandler(this.frmMultiCurrency_Load);
            this.gbMain.ResumeLayout(false);
            this.gbMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkPtGdRnd;
        private System.Windows.Forms.CheckBox chkPtTrnRnd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkStGdRnd;
        private System.Windows.Forms.CheckBox chkStTrnRnd;
        private System.Windows.Forms.Button btnRtDiffAc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.TextBox txtRtDiffAc;
    }
}