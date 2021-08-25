namespace udCompanySetting
{
    partial class frmAccounts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccounts));
            this.gbMain = new System.Windows.Forms.GroupBox();
            this.chkCostCenter = new System.Windows.Forms.CheckBox();
            this.nudLvl = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.chkVenIndt = new System.Windows.Forms.CheckBox();
            this.chkInclBillDt = new System.Windows.Forms.CheckBox();
            this.chkOnlineBalChk = new System.Windows.Forms.CheckBox();
            this.chkAcWise = new System.Windows.Forms.CheckBox();
            this.chkPayAdj = new System.Windows.Forms.CheckBox();
            this.gbBalSheet = new System.Windows.Forms.GroupBox();
            this.btnCSBSAct = new System.Windows.Forms.Button();
            this.btnCSPLAct = new System.Windows.Forms.Button();
            this.btnOSAct = new System.Windows.Forms.Button();
            this.txtCSBSAct = new System.Windows.Forms.TextBox();
            this.txtCSPLAct = new System.Windows.Forms.TextBox();
            this.txtOSAct = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPlStAc = new System.Windows.Forms.Button();
            this.btnPlPtAc = new System.Windows.Forms.Button();
            this.btnBalStAc = new System.Windows.Forms.Button();
            this.btnBalPtAc = new System.Windows.Forms.Button();
            this.txtPlStAc = new System.Windows.Forms.TextBox();
            this.txtPlPtAc = new System.Windows.Forms.TextBox();
            this.txtBalStAc = new System.Windows.Forms.TextBox();
            this.txtBalPtAc = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnDone = new System.Windows.Forms.Button();
            this.gbMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLvl)).BeginInit();
            this.gbBalSheet.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.chkCostCenter);
            this.gbMain.Controls.Add(this.nudLvl);
            this.gbMain.Controls.Add(this.label1);
            this.gbMain.Controls.Add(this.chkVenIndt);
            this.gbMain.Controls.Add(this.chkInclBillDt);
            this.gbMain.Controls.Add(this.chkOnlineBalChk);
            this.gbMain.Controls.Add(this.chkAcWise);
            this.gbMain.Controls.Add(this.chkPayAdj);
            this.gbMain.Location = new System.Drawing.Point(8, 5);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(646, 93);
            this.gbMain.TabIndex = 0;
            this.gbMain.TabStop = false;
            this.gbMain.Text = "Setting";
            // 
            // chkCostCenter
            // 
            this.chkCostCenter.AutoSize = true;
            this.chkCostCenter.Location = new System.Drawing.Point(367, 64);
            this.chkCostCenter.Name = "chkCostCenter";
            this.chkCostCenter.Size = new System.Drawing.Size(160, 17);
            this.chkCostCenter.TabIndex = 7;
            this.chkCostCenter.Text = "Cost Centre non-expandable";
            this.chkCostCenter.UseVisualStyleBackColor = true;
            // 
            // nudLvl
            // 
            this.nudLvl.Location = new System.Drawing.Point(181, 63);
            this.nudLvl.Name = "nudLvl";
            this.nudLvl.Size = new System.Drawing.Size(65, 20);
            this.nudLvl.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Group Level No. For Final Reports";
            // 
            // chkVenIndt
            // 
            this.chkVenIndt.AutoSize = true;
            this.chkVenIndt.Location = new System.Drawing.Point(11, 42);
            this.chkVenIndt.Name = "chkVenIndt";
            this.chkVenIndt.Size = new System.Drawing.Size(202, 17);
            this.chkVenIndt.TabIndex = 3;
            this.chkVenIndt.Text = "Generate Vendor wise Indent in MRP";
            this.chkVenIndt.UseVisualStyleBackColor = true;
            // 
            // chkInclBillDt
            // 
            this.chkInclBillDt.AutoSize = true;
            this.chkInclBillDt.Location = new System.Drawing.Point(367, 42);
            this.chkInclBillDt.Name = "chkInclBillDt";
            this.chkInclBillDt.Size = new System.Drawing.Size(219, 17);
            this.chkInclBillDt.TabIndex = 4;
            this.chkInclBillDt.Text = "Include bill date in credit days calculation";
            this.chkInclBillDt.UseVisualStyleBackColor = true;
            // 
            // chkOnlineBalChk
            // 
            this.chkOnlineBalChk.AutoSize = true;
            this.chkOnlineBalChk.Location = new System.Drawing.Point(367, 19);
            this.chkOnlineBalChk.Name = "chkOnlineBalChk";
            this.chkOnlineBalChk.Size = new System.Drawing.Size(145, 17);
            this.chkOnlineBalChk.TabIndex = 2;
            this.chkOnlineBalChk.Text = "On Line Balance Check  ";
            this.chkOnlineBalChk.UseVisualStyleBackColor = true;
            // 
            // chkAcWise
            // 
            this.chkAcWise.AutoSize = true;
            this.chkAcWise.Location = new System.Drawing.Point(181, 19);
            this.chkAcWise.Name = "chkAcWise";
            this.chkAcWise.Size = new System.Drawing.Size(96, 17);
            this.chkAcWise.TabIndex = 1;
            this.chkAcWise.Text = "Account Wise ";
            this.chkAcWise.UseVisualStyleBackColor = true;
            // 
            // chkPayAdj
            // 
            this.chkPayAdj.AutoSize = true;
            this.chkPayAdj.Location = new System.Drawing.Point(11, 19);
            this.chkPayAdj.Name = "chkPayAdj";
            this.chkPayAdj.Size = new System.Drawing.Size(169, 17);
            this.chkPayAdj.TabIndex = 0;
            this.chkPayAdj.Text = "Manual Payment Adjustment   ";
            this.chkPayAdj.UseVisualStyleBackColor = true;
            this.chkPayAdj.CheckedChanged += new System.EventHandler(this.chkPayAdj_CheckedChanged);
            // 
            // gbBalSheet
            // 
            this.gbBalSheet.Controls.Add(this.btnCSBSAct);
            this.gbBalSheet.Controls.Add(this.btnCSPLAct);
            this.gbBalSheet.Controls.Add(this.btnOSAct);
            this.gbBalSheet.Controls.Add(this.txtCSBSAct);
            this.gbBalSheet.Controls.Add(this.txtCSPLAct);
            this.gbBalSheet.Controls.Add(this.txtOSAct);
            this.gbBalSheet.Controls.Add(this.label4);
            this.gbBalSheet.Controls.Add(this.label3);
            this.gbBalSheet.Controls.Add(this.label2);
            this.gbBalSheet.Controls.Add(this.btnPlStAc);
            this.gbBalSheet.Controls.Add(this.btnPlPtAc);
            this.gbBalSheet.Controls.Add(this.btnBalStAc);
            this.gbBalSheet.Controls.Add(this.btnBalPtAc);
            this.gbBalSheet.Controls.Add(this.txtPlStAc);
            this.gbBalSheet.Controls.Add(this.txtPlPtAc);
            this.gbBalSheet.Controls.Add(this.txtBalStAc);
            this.gbBalSheet.Controls.Add(this.txtBalPtAc);
            this.gbBalSheet.Controls.Add(this.label7);
            this.gbBalSheet.Controls.Add(this.label8);
            this.gbBalSheet.Controls.Add(this.label6);
            this.gbBalSheet.Controls.Add(this.label5);
            this.gbBalSheet.Location = new System.Drawing.Point(8, 103);
            this.gbBalSheet.Name = "gbBalSheet";
            this.gbBalSheet.Size = new System.Drawing.Size(646, 200);
            this.gbBalSheet.TabIndex = 1;
            this.gbBalSheet.TabStop = false;
            this.gbBalSheet.Text = "Balance Sheet Ledger Details";
            // 
            // btnCSBSAct
            // 
            this.btnCSBSAct.Location = new System.Drawing.Point(608, 69);
            this.btnCSBSAct.Name = "btnCSBSAct";
            this.btnCSBSAct.Size = new System.Drawing.Size(27, 23);
            this.btnCSBSAct.TabIndex = 5;
            this.btnCSBSAct.UseVisualStyleBackColor = true;
            this.btnCSBSAct.Click += new System.EventHandler(this.btnCSBSAct_Click);
            // 
            // btnCSPLAct
            // 
            this.btnCSPLAct.Location = new System.Drawing.Point(608, 45);
            this.btnCSPLAct.Name = "btnCSPLAct";
            this.btnCSPLAct.Size = new System.Drawing.Size(27, 23);
            this.btnCSPLAct.TabIndex = 3;
            this.btnCSPLAct.UseVisualStyleBackColor = true;
            this.btnCSPLAct.Click += new System.EventHandler(this.btnCSPLAct_Click);
            // 
            // btnOSAct
            // 
            this.btnOSAct.Location = new System.Drawing.Point(608, 21);
            this.btnOSAct.Name = "btnOSAct";
            this.btnOSAct.Size = new System.Drawing.Size(27, 23);
            this.btnOSAct.TabIndex = 1;
            this.btnOSAct.UseVisualStyleBackColor = true;
            this.btnOSAct.Click += new System.EventHandler(this.btnOSAct_Click);
            // 
            // txtCSBSAct
            // 
            this.txtCSBSAct.Location = new System.Drawing.Point(259, 71);
            this.txtCSBSAct.Name = "txtCSBSAct";
            this.txtCSBSAct.Size = new System.Drawing.Size(342, 20);
            this.txtCSBSAct.TabIndex = 4;
            // 
            // txtCSPLAct
            // 
            this.txtCSPLAct.Location = new System.Drawing.Point(259, 47);
            this.txtCSPLAct.Name = "txtCSPLAct";
            this.txtCSPLAct.Size = new System.Drawing.Size(342, 20);
            this.txtCSPLAct.TabIndex = 2;
            // 
            // txtOSAct
            // 
            this.txtOSAct.Location = new System.Drawing.Point(259, 23);
            this.txtOSAct.Name = "txtOSAct";
            this.txtOSAct.Size = new System.Drawing.Size(342, 20);
            this.txtOSAct.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(181, 13);
            this.label4.TabIndex = 62;
            this.label4.Text = "Closing Stock (B/S) Account Posting";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 13);
            this.label3.TabIndex = 61;
            this.label3.Text = "Closing Stock (P&&L) Account Posting";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 13);
            this.label2.TabIndex = 60;
            this.label2.Text = "Opening Stock Account Posting";
            // 
            // btnPlStAc
            // 
            this.btnPlStAc.Location = new System.Drawing.Point(608, 166);
            this.btnPlStAc.Name = "btnPlStAc";
            this.btnPlStAc.Size = new System.Drawing.Size(27, 23);
            this.btnPlStAc.TabIndex = 13;
            this.btnPlStAc.UseVisualStyleBackColor = true;
            this.btnPlStAc.Click += new System.EventHandler(this.btnPlStAc_Click);
            // 
            // btnPlPtAc
            // 
            this.btnPlPtAc.Location = new System.Drawing.Point(608, 142);
            this.btnPlPtAc.Name = "btnPlPtAc";
            this.btnPlPtAc.Size = new System.Drawing.Size(27, 23);
            this.btnPlPtAc.TabIndex = 11;
            this.btnPlPtAc.UseVisualStyleBackColor = true;
            this.btnPlPtAc.Click += new System.EventHandler(this.btnPlPtAc_Click);
            // 
            // btnBalStAc
            // 
            this.btnBalStAc.Location = new System.Drawing.Point(608, 118);
            this.btnBalStAc.Name = "btnBalStAc";
            this.btnBalStAc.Size = new System.Drawing.Size(27, 23);
            this.btnBalStAc.TabIndex = 9;
            this.btnBalStAc.UseVisualStyleBackColor = true;
            this.btnBalStAc.Click += new System.EventHandler(this.btnBalStAc_Click);
            // 
            // btnBalPtAc
            // 
            this.btnBalPtAc.Location = new System.Drawing.Point(608, 93);
            this.btnBalPtAc.Name = "btnBalPtAc";
            this.btnBalPtAc.Size = new System.Drawing.Size(27, 23);
            this.btnBalPtAc.TabIndex = 7;
            this.btnBalPtAc.UseVisualStyleBackColor = true;
            this.btnBalPtAc.Click += new System.EventHandler(this.btnBalPtAc_Click);
            // 
            // txtPlStAc
            // 
            this.txtPlStAc.Location = new System.Drawing.Point(259, 168);
            this.txtPlStAc.Name = "txtPlStAc";
            this.txtPlStAc.Size = new System.Drawing.Size(342, 20);
            this.txtPlStAc.TabIndex = 12;
            // 
            // txtPlPtAc
            // 
            this.txtPlPtAc.Location = new System.Drawing.Point(259, 144);
            this.txtPlPtAc.Name = "txtPlPtAc";
            this.txtPlPtAc.Size = new System.Drawing.Size(342, 20);
            this.txtPlPtAc.TabIndex = 10;
            // 
            // txtBalStAc
            // 
            this.txtBalStAc.Location = new System.Drawing.Point(259, 120);
            this.txtBalStAc.Name = "txtBalStAc";
            this.txtBalStAc.Size = new System.Drawing.Size(342, 20);
            this.txtBalStAc.TabIndex = 8;
            // 
            // txtBalPtAc
            // 
            this.txtBalPtAc.Location = new System.Drawing.Point(259, 95);
            this.txtBalPtAc.Name = "txtBalPtAc";
            this.txtBalPtAc.Size = new System.Drawing.Size(342, 20);
            this.txtBalPtAc.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 166);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(183, 13);
            this.label7.TabIndex = 52;
            this.label7.Text = "P && L - Sales Bill to come A/c Posting";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(228, 13);
            this.label8.TabIndex = 51;
            this.label8.Text = "Balance Sheet - Sales Bill to make A/c Posting";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(202, 13);
            this.label6.TabIndex = 50;
            this.label6.Text = "P && L - Purchase Bill to come A/c Posting";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(245, 13);
            this.label5.TabIndex = 49;
            this.label5.Text = "Balance sheet - Purchase Bill to come A/c Posting";
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(579, 309);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 2;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // frmAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 341);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.gbBalSheet);
            this.Controls.Add(this.gbMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAccounts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmAccounts";
            this.Load += new System.EventHandler(this.frmAccounts_Load);
            this.gbMain.ResumeLayout(false);
            this.gbMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLvl)).EndInit();
            this.gbBalSheet.ResumeLayout(false);
            this.gbBalSheet.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkVenIndt;
        private System.Windows.Forms.CheckBox chkInclBillDt;
        private System.Windows.Forms.CheckBox chkOnlineBalChk;
        private System.Windows.Forms.CheckBox chkAcWise;
        private System.Windows.Forms.CheckBox chkPayAdj;
        private System.Windows.Forms.GroupBox gbBalSheet;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.NumericUpDown nudLvl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPlStAc;
        private System.Windows.Forms.TextBox txtPlPtAc;
        private System.Windows.Forms.TextBox txtBalStAc;
        private System.Windows.Forms.TextBox txtBalPtAc;
        private System.Windows.Forms.Button btnPlStAc;
        private System.Windows.Forms.Button btnPlPtAc;
        private System.Windows.Forms.Button btnBalStAc;
        private System.Windows.Forms.Button btnBalPtAc;
        private System.Windows.Forms.CheckBox chkCostCenter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOSAct;
        private System.Windows.Forms.TextBox txtCSBSAct;
        private System.Windows.Forms.TextBox txtCSPLAct;
        private System.Windows.Forms.Button btnCSBSAct;
        private System.Windows.Forms.Button btnCSPLAct;
        private System.Windows.Forms.Button btnOSAct;
    }
}