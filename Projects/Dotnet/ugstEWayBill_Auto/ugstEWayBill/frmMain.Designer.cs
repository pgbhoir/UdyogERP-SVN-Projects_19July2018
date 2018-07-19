namespace ugstEWayBill
{
    partial class frmMain
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
            this.btnSelctAll = new System.Windows.Forms.Button();
            this.txtGrpId = new System.Windows.Forms.TextBox();
            this.btnTrnName = new System.Windows.Forms.Button();
            this.btnPartyNm = new System.Windows.Forms.Button();
            this.btnGSTIN = new System.Windows.Forms.Button();
            this.dgvGSTIN = new System.Windows.Forms.DataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.txtTrnName = new System.Windows.Forms.TextBox();
            this.txtGSTIN = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPartyNm = new System.Windows.Forms.TextBox();
            this.gbxFilter = new System.Windows.Forms.GroupBox();
            this.btnTInvNo = new System.Windows.Forms.Button();
            this.txtTInvNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnFInvNo = new System.Windows.Forms.Button();
            this.txtFInvNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbOutward = new System.Windows.Forms.RadioButton();
            this.rbInward = new System.Windows.Forms.RadioButton();
            this.btnGenJson = new System.Windows.Forms.Button();
            this.txtJsonFile = new System.Windows.Forms.TextBox();
            this.btnJsonFile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenEWB = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTIN)).BeginInit();
            this.gbxFilter.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelctAll
            // 
            this.btnSelctAll.Location = new System.Drawing.Point(7, 126);
            this.btnSelctAll.Name = "btnSelctAll";
            this.btnSelctAll.Size = new System.Drawing.Size(75, 21);
            this.btnSelctAll.TabIndex = 10;
            this.btnSelctAll.Text = "Select All";
            this.btnSelctAll.UseVisualStyleBackColor = true;
            this.btnSelctAll.Click += new System.EventHandler(this.btnSelctAll_Click);
            // 
            // txtGrpId
            // 
            this.txtGrpId.Location = new System.Drawing.Point(820, -7);
            this.txtGrpId.Name = "txtGrpId";
            this.txtGrpId.Size = new System.Drawing.Size(74, 20);
            this.txtGrpId.TabIndex = 31;
            this.txtGrpId.Visible = false;
            // 
            // btnTrnName
            // 
            this.btnTrnName.Location = new System.Drawing.Point(319, 17);
            this.btnTrnName.Name = "btnTrnName";
            this.btnTrnName.Size = new System.Drawing.Size(24, 23);
            this.btnTrnName.TabIndex = 5;
            this.btnTrnName.UseVisualStyleBackColor = true;
            this.btnTrnName.Click += new System.EventHandler(this.btnTrnName_Click);
            // 
            // btnPartyNm
            // 
            this.btnPartyNm.Location = new System.Drawing.Point(319, 44);
            this.btnPartyNm.Name = "btnPartyNm";
            this.btnPartyNm.Size = new System.Drawing.Size(24, 23);
            this.btnPartyNm.TabIndex = 7;
            this.btnPartyNm.UseVisualStyleBackColor = true;
            this.btnPartyNm.Click += new System.EventHandler(this.btnPartyNm_Click);
            // 
            // btnGSTIN
            // 
            this.btnGSTIN.Location = new System.Drawing.Point(651, 41);
            this.btnGSTIN.Name = "btnGSTIN";
            this.btnGSTIN.Size = new System.Drawing.Size(24, 23);
            this.btnGSTIN.TabIndex = 3;
            this.btnGSTIN.UseVisualStyleBackColor = true;
            this.btnGSTIN.Click += new System.EventHandler(this.btnGSTIN_Click);
            // 
            // dgvGSTIN
            // 
            this.dgvGSTIN.AllowUserToAddRows = false;
            this.dgvGSTIN.AllowUserToDeleteRows = false;
            this.dgvGSTIN.AllowUserToResizeRows = false;
            this.dgvGSTIN.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGSTIN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGSTIN.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect});
            this.dgvGSTIN.Location = new System.Drawing.Point(7, 158);
            this.dgvGSTIN.Name = "dgvGSTIN";
            this.dgvGSTIN.RowHeadersVisible = false;
            this.dgvGSTIN.Size = new System.Drawing.Size(907, 262);
            this.dgvGSTIN.TabIndex = 14;
            this.dgvGSTIN.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGSTIN_CellContentClick);
            this.dgvGSTIN.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGSTIN_CellValueChanged);
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "colSelect";
            this.colSelect.Name = "colSelect";
            this.colSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // txtTrnName
            // 
            this.txtTrnName.Location = new System.Drawing.Point(142, 18);
            this.txtTrnName.Name = "txtTrnName";
            this.txtTrnName.ReadOnly = true;
            this.txtTrnName.Size = new System.Drawing.Size(172, 20);
            this.txtTrnName.TabIndex = 4;
            this.txtTrnName.TextChanged += new System.EventHandler(this.txtTrnName_TextChanged);
            // 
            // txtGSTIN
            // 
            this.txtGSTIN.Location = new System.Drawing.Point(467, 44);
            this.txtGSTIN.Name = "txtGSTIN";
            this.txtGSTIN.ReadOnly = true;
            this.txtGSTIN.Size = new System.Drawing.Size(181, 20);
            this.txtGSTIN.TabIndex = 2;
            this.txtGSTIN.TextChanged += new System.EventHandler(this.txtGSTIN_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Transaction Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Party Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(366, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Party GSTIN";
            // 
            // txtPartyNm
            // 
            this.txtPartyNm.Location = new System.Drawing.Point(142, 45);
            this.txtPartyNm.Name = "txtPartyNm";
            this.txtPartyNm.ReadOnly = true;
            this.txtPartyNm.Size = new System.Drawing.Size(172, 20);
            this.txtPartyNm.TabIndex = 6;
            this.txtPartyNm.TextChanged += new System.EventHandler(this.txtPartyNm_TextChanged);
            // 
            // gbxFilter
            // 
            this.gbxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxFilter.Controls.Add(this.btnTInvNo);
            this.gbxFilter.Controls.Add(this.txtTInvNo);
            this.gbxFilter.Controls.Add(this.label5);
            this.gbxFilter.Controls.Add(this.btnFInvNo);
            this.gbxFilter.Controls.Add(this.txtFInvNo);
            this.gbxFilter.Controls.Add(this.label4);
            this.gbxFilter.Controls.Add(this.groupBox1);
            this.gbxFilter.Controls.Add(this.txtGrpId);
            this.gbxFilter.Controls.Add(this.btnTrnName);
            this.gbxFilter.Controls.Add(this.btnPartyNm);
            this.gbxFilter.Controls.Add(this.btnGSTIN);
            this.gbxFilter.Controls.Add(this.txtTrnName);
            this.gbxFilter.Controls.Add(this.txtPartyNm);
            this.gbxFilter.Controls.Add(this.txtGSTIN);
            this.gbxFilter.Controls.Add(this.label2);
            this.gbxFilter.Controls.Add(this.label1);
            this.gbxFilter.Controls.Add(this.label6);
            this.gbxFilter.Location = new System.Drawing.Point(4, 1);
            this.gbxFilter.Name = "gbxFilter";
            this.gbxFilter.Size = new System.Drawing.Size(907, 116);
            this.gbxFilter.TabIndex = 24;
            this.gbxFilter.TabStop = false;
            this.gbxFilter.Text = "Filter";
            this.gbxFilter.Enter += new System.EventHandler(this.gbxFilter_Enter);
            // 
            // btnTInvNo
            // 
            this.btnTInvNo.Location = new System.Drawing.Point(651, 68);
            this.btnTInvNo.Name = "btnTInvNo";
            this.btnTInvNo.Size = new System.Drawing.Size(24, 23);
            this.btnTInvNo.TabIndex = 42;
            this.btnTInvNo.UseVisualStyleBackColor = true;
            this.btnTInvNo.Click += new System.EventHandler(this.btnTInvNo_Click);
            // 
            // txtTInvNo
            // 
            this.txtTInvNo.Location = new System.Drawing.Point(467, 71);
            this.txtTInvNo.Name = "txtTInvNo";
            this.txtTInvNo.ReadOnly = true;
            this.txtTInvNo.Size = new System.Drawing.Size(181, 20);
            this.txtTInvNo.TabIndex = 41;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(366, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 40;
            this.label5.Text = "To Transaction No.";
            // 
            // btnFInvNo
            // 
            this.btnFInvNo.Location = new System.Drawing.Point(319, 73);
            this.btnFInvNo.Name = "btnFInvNo";
            this.btnFInvNo.Size = new System.Drawing.Size(24, 23);
            this.btnFInvNo.TabIndex = 38;
            this.btnFInvNo.UseVisualStyleBackColor = true;
            this.btnFInvNo.Click += new System.EventHandler(this.btnFInvNo_Click);
            // 
            // txtFInvNo
            // 
            this.txtFInvNo.Location = new System.Drawing.Point(142, 74);
            this.txtFInvNo.Name = "txtFInvNo";
            this.txtFInvNo.ReadOnly = true;
            this.txtFInvNo.Size = new System.Drawing.Size(172, 20);
            this.txtFInvNo.TabIndex = 37;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "FromTransaction No.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbOutward);
            this.groupBox1.Controls.Add(this.rbInward);
            this.groupBox1.Location = new System.Drawing.Point(769, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(138, 40);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // rbOutward
            // 
            this.rbOutward.AutoSize = true;
            this.rbOutward.Checked = true;
            this.rbOutward.Location = new System.Drawing.Point(5, 15);
            this.rbOutward.Name = "rbOutward";
            this.rbOutward.Size = new System.Drawing.Size(65, 17);
            this.rbOutward.TabIndex = 8;
            this.rbOutward.TabStop = true;
            this.rbOutward.Text = "Outward";
            this.rbOutward.UseVisualStyleBackColor = true;
            // 
            // rbInward
            // 
            this.rbInward.AutoSize = true;
            this.rbInward.Location = new System.Drawing.Point(77, 14);
            this.rbInward.Name = "rbInward";
            this.rbInward.Size = new System.Drawing.Size(57, 17);
            this.rbInward.TabIndex = 9;
            this.rbInward.Text = "Inward";
            this.rbInward.UseVisualStyleBackColor = true;
            // 
            // btnGenJson
            // 
            this.btnGenJson.Location = new System.Drawing.Point(556, 121);
            this.btnGenJson.Name = "btnGenJson";
            this.btnGenJson.Size = new System.Drawing.Size(89, 25);
            this.btnGenJson.TabIndex = 28;
            this.btnGenJson.Text = "Generate Json";
            this.btnGenJson.UseVisualStyleBackColor = true;
            this.btnGenJson.Click += new System.EventHandler(this.btnGenJson_Click);
            // 
            // txtJsonFile
            // 
            this.txtJsonFile.Location = new System.Drawing.Point(183, 125);
            this.txtJsonFile.Name = "txtJsonFile";
            this.txtJsonFile.Size = new System.Drawing.Size(337, 20);
            this.txtJsonFile.TabIndex = 11;
            // 
            // btnJsonFile
            // 
            this.btnJsonFile.Location = new System.Drawing.Point(526, 123);
            this.btnJsonFile.Name = "btnJsonFile";
            this.btnJsonFile.Size = new System.Drawing.Size(24, 23);
            this.btnJsonFile.TabIndex = 12;
            this.btnJsonFile.UseVisualStyleBackColor = true;
            this.btnJsonFile.Click += new System.EventHandler(this.btnJsonFile_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(101, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "Json File Path";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 439);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(921, 22);
            this.statusStrip1.TabIndex = 36;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Status";
            // 
            // btnCancel
            // 
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.Location = new System.Drawing.Point(774, 121);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(124, 25);
            this.btnCancel.TabIndex = 38;
            this.btnCancel.Text = "Cancel EWB Online";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenEWB
            // 
            this.btnGenEWB.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGenEWB.Location = new System.Drawing.Point(648, 121);
            this.btnGenEWB.Name = "btnGenEWB";
            this.btnGenEWB.Size = new System.Drawing.Size(124, 25);
            this.btnGenEWB.TabIndex = 37;
            this.btnGenEWB.Text = "Generate EWB Online";
            this.btnGenEWB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenEWB.UseVisualStyleBackColor = true;
            this.btnGenEWB.Click += new System.EventHandler(this.btnGenEWB_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 461);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenEWB);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnJsonFile);
            this.Controls.Add(this.txtJsonFile);
            this.Controls.Add(this.btnGenJson);
            this.Controls.Add(this.btnSelctAll);
            this.Controls.Add(this.dgvGSTIN);
            this.Controls.Add(this.gbxFilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "e Way Bill Generation";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTIN)).EndInit();
            this.gbxFilter.ResumeLayout(false);
            this.gbxFilter.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelctAll;
        private System.Windows.Forms.TextBox txtGrpId;
        private System.Windows.Forms.Button btnTrnName;
        private System.Windows.Forms.Button btnPartyNm;
        private System.Windows.Forms.Button btnGSTIN;
        private System.Windows.Forms.DataGridView dgvGSTIN;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.TextBox txtTrnName;
        private System.Windows.Forms.TextBox txtGSTIN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPartyNm;
        private System.Windows.Forms.GroupBox gbxFilter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbOutward;
        private System.Windows.Forms.RadioButton rbInward;
        private System.Windows.Forms.Button btnGenJson;
        private System.Windows.Forms.TextBox txtJsonFile;
        private System.Windows.Forms.Button btnJsonFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Button btnFInvNo;
        private System.Windows.Forms.TextBox txtFInvNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnTInvNo;
        private System.Windows.Forms.TextBox txtTInvNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenEWB;
    }
}

