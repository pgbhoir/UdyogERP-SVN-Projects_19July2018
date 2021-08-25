namespace udCompanySetting
{
    partial class frmGeneral
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtServ = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSat = new System.Windows.Forms.CheckBox();
            this.chkFri = new System.Windows.Forms.CheckBox();
            this.chkThu = new System.Windows.Forms.CheckBox();
            this.chkWed = new System.Windows.Forms.CheckBox();
            this.chkTue = new System.Windows.Forms.CheckBox();
            this.chkMon = new System.Windows.Forms.CheckBox();
            this.chkSun = new System.Windows.Forms.CheckBox();
            this.btnServ = new System.Windows.Forms.Button();
            this.btnLoc = new System.Windows.Forms.Button();
            this.txtLoc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nmdTime = new System.Windows.Forms.NumericUpDown();
            this.cmbTime = new System.Windows.Forms.ComboBox();
            this.chkTips = new System.Windows.Forms.CheckBox();
            this.btnDone = new System.Windows.Forms.Button();
            this.nudInvLen = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtItHeading = new System.Windows.Forms.TextBox();
            this.chkApproset = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkPTSupPost = new System.Windows.Forms.CheckBox();
            this.chkSTRate = new System.Windows.Forms.CheckBox();
            this.chkSTSupPost = new System.Windows.Forms.CheckBox();
            this.ChkPTLine = new System.Windows.Forms.CheckBox();
            this.chkPTRate = new System.Windows.Forms.CheckBox();
            this.chkSTLine = new System.Windows.Forms.CheckBox();
            this.chkShwWEFDt = new System.Windows.Forms.CheckBox();
            this.cmbDsb = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmdTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInvLen)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server Path";
            // 
            // txtServ
            // 
            this.txtServ.Enabled = false;
            this.txtServ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServ.Location = new System.Drawing.Point(94, 173);
            this.txtServ.Name = "txtServ";
            this.txtServ.Size = new System.Drawing.Size(259, 20);
            this.txtServ.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkSat);
            this.groupBox1.Controls.Add(this.chkFri);
            this.groupBox1.Controls.Add(this.chkThu);
            this.groupBox1.Controls.Add(this.chkWed);
            this.groupBox1.Controls.Add(this.chkTue);
            this.groupBox1.Controls.Add(this.chkMon);
            this.groupBox1.Controls.Add(this.chkSun);
            this.groupBox1.Location = new System.Drawing.Point(11, 105);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 63);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Weekly Holiday(s)";
            // 
            // chkSat
            // 
            this.chkSat.AutoSize = true;
            this.chkSat.Location = new System.Drawing.Point(236, 19);
            this.chkSat.Name = "chkSat";
            this.chkSat.Size = new System.Drawing.Size(68, 17);
            this.chkSat.TabIndex = 6;
            this.chkSat.Text = "Saturday";
            this.chkSat.UseVisualStyleBackColor = true;
            // 
            // chkFri
            // 
            this.chkFri.AutoSize = true;
            this.chkFri.Location = new System.Drawing.Point(160, 39);
            this.chkFri.Name = "chkFri";
            this.chkFri.Size = new System.Drawing.Size(54, 17);
            this.chkFri.TabIndex = 5;
            this.chkFri.Text = "Friday";
            this.chkFri.UseVisualStyleBackColor = true;
            // 
            // chkThu
            // 
            this.chkThu.AutoSize = true;
            this.chkThu.Location = new System.Drawing.Point(160, 19);
            this.chkThu.Name = "chkThu";
            this.chkThu.Size = new System.Drawing.Size(70, 17);
            this.chkThu.TabIndex = 4;
            this.chkThu.Text = "Thursday";
            this.chkThu.UseVisualStyleBackColor = true;
            // 
            // chkWed
            // 
            this.chkWed.AutoSize = true;
            this.chkWed.Location = new System.Drawing.Point(75, 39);
            this.chkWed.Name = "chkWed";
            this.chkWed.Size = new System.Drawing.Size(83, 17);
            this.chkWed.TabIndex = 3;
            this.chkWed.Text = "Wednesday";
            this.chkWed.UseVisualStyleBackColor = true;
            // 
            // chkTue
            // 
            this.chkTue.AutoSize = true;
            this.chkTue.Location = new System.Drawing.Point(74, 19);
            this.chkTue.Name = "chkTue";
            this.chkTue.Size = new System.Drawing.Size(67, 17);
            this.chkTue.TabIndex = 2;
            this.chkTue.Text = "Tuesday";
            this.chkTue.UseVisualStyleBackColor = true;
            // 
            // chkMon
            // 
            this.chkMon.AutoSize = true;
            this.chkMon.Location = new System.Drawing.Point(6, 38);
            this.chkMon.Name = "chkMon";
            this.chkMon.Size = new System.Drawing.Size(64, 17);
            this.chkMon.TabIndex = 1;
            this.chkMon.Text = "Monday";
            this.chkMon.UseVisualStyleBackColor = true;
            // 
            // chkSun
            // 
            this.chkSun.AutoSize = true;
            this.chkSun.Location = new System.Drawing.Point(6, 19);
            this.chkSun.Name = "chkSun";
            this.chkSun.Size = new System.Drawing.Size(62, 17);
            this.chkSun.TabIndex = 0;
            this.chkSun.Text = "Sunday";
            this.chkSun.UseVisualStyleBackColor = true;
            // 
            // btnServ
            // 
            this.btnServ.Location = new System.Drawing.Point(360, 172);
            this.btnServ.Name = "btnServ";
            this.btnServ.Size = new System.Drawing.Size(27, 23);
            this.btnServ.TabIndex = 4;
            this.btnServ.UseVisualStyleBackColor = true;
            this.btnServ.Click += new System.EventHandler(this.btnServ_Click);
            // 
            // btnLoc
            // 
            this.btnLoc.Location = new System.Drawing.Point(360, 198);
            this.btnLoc.Name = "btnLoc";
            this.btnLoc.Size = new System.Drawing.Size(27, 23);
            this.btnLoc.TabIndex = 7;
            this.btnLoc.UseVisualStyleBackColor = true;
            this.btnLoc.Click += new System.EventHandler(this.btnLoc_Click);
            // 
            // txtLoc
            // 
            this.txtLoc.Enabled = false;
            this.txtLoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoc.Location = new System.Drawing.Point(94, 199);
            this.txtLoc.Name = "txtLoc";
            this.txtLoc.Size = new System.Drawing.Size(259, 20);
            this.txtLoc.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Location";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 282);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Session Timeout";
            // 
            // nmdTime
            // 
            this.nmdTime.Location = new System.Drawing.Point(124, 282);
            this.nmdTime.Name = "nmdTime";
            this.nmdTime.Size = new System.Drawing.Size(59, 20);
            this.nmdTime.TabIndex = 14;
            // 
            // cmbTime
            // 
            this.cmbTime.FormattingEnabled = true;
            this.cmbTime.Location = new System.Drawing.Point(191, 282);
            this.cmbTime.Name = "cmbTime";
            this.cmbTime.Size = new System.Drawing.Size(67, 21);
            this.cmbTime.TabIndex = 15;
            // 
            // chkTips
            // 
            this.chkTips.AutoSize = true;
            this.chkTips.Location = new System.Drawing.Point(21, 313);
            this.chkTips.Name = "chkTips";
            this.chkTips.Size = new System.Drawing.Size(76, 17);
            this.chkTips.TabIndex = 16;
            this.chkTips.Text = "Show Tips";
            this.chkTips.UseVisualStyleBackColor = true;
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(309, 338);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 19;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // nudInvLen
            // 
            this.nudInvLen.Location = new System.Drawing.Point(124, 254);
            this.nudInvLen.Name = "nudInvLen";
            this.nudInvLen.Size = new System.Drawing.Size(59, 20);
            this.nudInvLen.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 258);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Invoice Field Length";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 228);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Item Heading";
            // 
            // txtItHeading
            // 
            this.txtItHeading.Location = new System.Drawing.Point(94, 225);
            this.txtItHeading.Name = "txtItHeading";
            this.txtItHeading.Size = new System.Drawing.Size(259, 20);
            this.txtItHeading.TabIndex = 9;
            // 
            // chkApproset
            // 
            this.chkApproset.AutoSize = true;
            this.chkApproset.Location = new System.Drawing.Point(217, 255);
            this.chkApproset.Name = "chkApproset";
            this.chkApproset.Size = new System.Drawing.Size(140, 17);
            this.chkApproset.TabIndex = 12;
            this.chkApproset.Text = "Not Edit After Approval?";
            this.chkApproset.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkPTSupPost);
            this.groupBox2.Controls.Add(this.chkSTRate);
            this.groupBox2.Controls.Add(this.chkSTSupPost);
            this.groupBox2.Controls.Add(this.ChkPTLine);
            this.groupBox2.Controls.Add(this.chkPTRate);
            this.groupBox2.Controls.Add(this.chkSTLine);
            this.groupBox2.Location = new System.Drawing.Point(11, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(342, 87);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Edit Details";
            // 
            // chkPTSupPost
            // 
            this.chkPTSupPost.AutoSize = true;
            this.chkPTSupPost.Location = new System.Drawing.Point(158, 65);
            this.chkPTSupPost.Name = "chkPTSupPost";
            this.chkPTSupPost.Size = new System.Drawing.Size(165, 17);
            this.chkPTSupPost.TabIndex = 5;
            this.chkPTSupPost.Text = "Purchase Supplywise Posting";
            this.chkPTSupPost.UseVisualStyleBackColor = true;
            this.chkPTSupPost.CheckedChanged += new System.EventHandler(this.chkPTSupPost_CheckedChanged);
            // 
            // chkSTRate
            // 
            this.chkSTRate.AutoSize = true;
            this.chkSTRate.Location = new System.Drawing.Point(6, 42);
            this.chkSTRate.Name = "chkSTRate";
            this.chkSTRate.Size = new System.Drawing.Size(124, 17);
            this.chkSTRate.TabIndex = 2;
            this.chkSTRate.Text = "Sales Rate of Goods";
            this.chkSTRate.UseVisualStyleBackColor = true;
            // 
            // chkSTSupPost
            // 
            this.chkSTSupPost.AutoSize = true;
            this.chkSTSupPost.Location = new System.Drawing.Point(6, 65);
            this.chkSTSupPost.Name = "chkSTSupPost";
            this.chkSTSupPost.Size = new System.Drawing.Size(146, 17);
            this.chkSTSupPost.TabIndex = 4;
            this.chkSTSupPost.Text = "Sales Supplywise Posting";
            this.chkSTSupPost.UseVisualStyleBackColor = true;
            // 
            // ChkPTLine
            // 
            this.ChkPTLine.AutoSize = true;
            this.ChkPTLine.Location = new System.Drawing.Point(158, 19);
            this.ChkPTLine.Name = "ChkPTLine";
            this.ChkPTLine.Size = new System.Drawing.Size(121, 17);
            this.ChkPTLine.TabIndex = 1;
            this.ChkPTLine.Text = "Purchase Line Total";
            this.ChkPTLine.UseVisualStyleBackColor = true;
            // 
            // chkPTRate
            // 
            this.chkPTRate.AutoSize = true;
            this.chkPTRate.Location = new System.Drawing.Point(158, 42);
            this.chkPTRate.Name = "chkPTRate";
            this.chkPTRate.Size = new System.Drawing.Size(143, 17);
            this.chkPTRate.TabIndex = 3;
            this.chkPTRate.Text = "Purchase Rate of Goods";
            this.chkPTRate.UseVisualStyleBackColor = true;
            // 
            // chkSTLine
            // 
            this.chkSTLine.AutoSize = true;
            this.chkSTLine.Location = new System.Drawing.Point(6, 19);
            this.chkSTLine.Name = "chkSTLine";
            this.chkSTLine.Size = new System.Drawing.Size(102, 17);
            this.chkSTLine.TabIndex = 0;
            this.chkSTLine.Text = "Sales Line Total";
            this.chkSTLine.UseVisualStyleBackColor = true;
            // 
            // chkShwWEFDt
            // 
            this.chkShwWEFDt.AutoSize = true;
            this.chkShwWEFDt.Location = new System.Drawing.Point(21, 344);
            this.chkShwWEFDt.Name = "chkShwWEFDt";
            this.chkShwWEFDt.Size = new System.Drawing.Size(210, 17);
            this.chkShwWEFDt.TabIndex = 18;
            this.chkShwWEFDt.Text = "Show W.E.F. Date in Price List  Master";
            this.chkShwWEFDt.UseVisualStyleBackColor = true;
            // 
            // cmbDsb
            // 
            this.cmbDsb.FormattingEnabled = true;
            this.cmbDsb.Items.AddRange(new object[] {
            "None",
            "Standard"});
            this.cmbDsb.Location = new System.Drawing.Point(219, 311);
            this.cmbDsb.Name = "cmbDsb";
            this.cmbDsb.Size = new System.Drawing.Size(100, 21);
            this.cmbDsb.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(121, 314);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Dashboard Setting";
            // 
            // frmGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 369);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbDsb);
            this.Controls.Add(this.chkShwWEFDt);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chkApproset);
            this.Controls.Add(this.txtItHeading);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudInvLen);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.chkTips);
            this.Controls.Add(this.cmbTime);
            this.Controls.Add(this.nmdTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnLoc);
            this.Controls.Add(this.txtLoc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnServ);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtServ);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGeneral";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmGeneral";
            this.Load += new System.EventHandler(this.frmGeneral_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmdTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInvLen)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServ;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkSat;
        private System.Windows.Forms.CheckBox chkFri;
        private System.Windows.Forms.CheckBox chkThu;
        private System.Windows.Forms.CheckBox chkWed;
        private System.Windows.Forms.CheckBox chkTue;
        private System.Windows.Forms.CheckBox chkMon;
        private System.Windows.Forms.CheckBox chkSun;
        private System.Windows.Forms.Button btnServ;
        private System.Windows.Forms.Button btnLoc;
        private System.Windows.Forms.TextBox txtLoc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nmdTime;
        private System.Windows.Forms.ComboBox cmbTime;
        private System.Windows.Forms.CheckBox chkTips;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.NumericUpDown nudInvLen;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtItHeading;
        private System.Windows.Forms.CheckBox chkApproset;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkPTSupPost;
        private System.Windows.Forms.CheckBox chkSTRate;
        private System.Windows.Forms.CheckBox chkSTSupPost;
        private System.Windows.Forms.CheckBox ChkPTLine;
        private System.Windows.Forms.CheckBox chkSTLine;
        private System.Windows.Forms.CheckBox chkPTRate;
        private System.Windows.Forms.CheckBox chkShwWEFDt;
        private System.Windows.Forms.ComboBox cmbDsb;
        private System.Windows.Forms.Label label6;
    }
}