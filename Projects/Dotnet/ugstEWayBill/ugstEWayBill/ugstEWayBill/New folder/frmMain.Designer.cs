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
            this.btnJson = new System.Windows.Forms.Button();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbOutward = new System.Windows.Forms.RadioButton();
            this.rbInward = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.txtJsonFile = new System.Windows.Forms.TextBox();
            this.btnJsonFile = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTIN)).BeginInit();
            this.gbxFilter.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelctAll
            // 
            this.btnSelctAll.Location = new System.Drawing.Point(7, 95);
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
            this.btnTrnName.Location = new System.Drawing.Point(684, 20);
            this.btnTrnName.Name = "btnTrnName";
            this.btnTrnName.Size = new System.Drawing.Size(24, 23);
            this.btnTrnName.TabIndex = 5;
            this.btnTrnName.UseVisualStyleBackColor = true;
            this.btnTrnName.Click += new System.EventHandler(this.btnTrnName_Click);
            // 
            // btnPartyNm
            // 
            this.btnPartyNm.Location = new System.Drawing.Point(270, 46);
            this.btnPartyNm.Name = "btnPartyNm";
            this.btnPartyNm.Size = new System.Drawing.Size(24, 23);
            this.btnPartyNm.TabIndex = 7;
            this.btnPartyNm.UseVisualStyleBackColor = true;
            this.btnPartyNm.Click += new System.EventHandler(this.btnPartyNm_Click);
            // 
            // btnJson
            // 
            this.btnJson.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnJson.Location = new System.Drawing.Point(796, 91);
            this.btnJson.Name = "btnJson";
            this.btnJson.Size = new System.Drawing.Size(113, 25);
            this.btnJson.TabIndex = 13;
            this.btnJson.Text = "Generate EWB";
            this.btnJson.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJson.UseVisualStyleBackColor = true;
            this.btnJson.Click += new System.EventHandler(this.btnJson_Click);
            // 
            // btnGSTIN
            // 
            this.btnGSTIN.Location = new System.Drawing.Point(270, 22);
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
            this.dgvGSTIN.Location = new System.Drawing.Point(7, 117);
            this.dgvGSTIN.Name = "dgvGSTIN";
            this.dgvGSTIN.RowHeadersVisible = false;
            this.dgvGSTIN.Size = new System.Drawing.Size(907, 275);
            this.dgvGSTIN.TabIndex = 14;
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
            this.txtTrnName.Location = new System.Drawing.Point(506, 22);
            this.txtTrnName.Name = "txtTrnName";
            this.txtTrnName.ReadOnly = true;
            this.txtTrnName.Size = new System.Drawing.Size(172, 20);
            this.txtTrnName.TabIndex = 4;
            this.txtTrnName.TextChanged += new System.EventHandler(this.txtTrnName_TextChanged);
            // 
            // txtGSTIN
            // 
            this.txtGSTIN.Location = new System.Drawing.Point(92, 22);
            this.txtGSTIN.Name = "txtGSTIN";
            this.txtGSTIN.ReadOnly = true;
            this.txtGSTIN.Size = new System.Drawing.Size(172, 20);
            this.txtGSTIN.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(385, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Transaction Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Party Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Party GSTIN";
            // 
            // txtPartyNm
            // 
            this.txtPartyNm.Location = new System.Drawing.Point(92, 46);
            this.txtPartyNm.Name = "txtPartyNm";
            this.txtPartyNm.ReadOnly = true;
            this.txtPartyNm.Size = new System.Drawing.Size(172, 20);
            this.txtPartyNm.TabIndex = 6;
            // 
            // gbxFilter
            // 
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
            this.gbxFilter.Location = new System.Drawing.Point(7, 8);
            this.gbxFilter.Name = "gbxFilter";
            this.gbxFilter.Size = new System.Drawing.Size(907, 82);
            this.gbxFilter.TabIndex = 24;
            this.gbxFilter.TabStop = false;
            this.gbxFilter.Text = "Filter";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbOutward);
            this.groupBox1.Controls.Add(this.rbInward);
            this.groupBox1.Location = new System.Drawing.Point(798, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(77, 57);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // rbOutward
            // 
            this.rbOutward.AutoSize = true;
            this.rbOutward.Checked = true;
            this.rbOutward.Location = new System.Drawing.Point(6, 14);
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
            this.rbInward.Location = new System.Drawing.Point(6, 36);
            this.rbInward.Name = "rbInward";
            this.rbInward.Size = new System.Drawing.Size(57, 17);
            this.rbInward.TabIndex = 9;
            this.rbInward.Text = "Inward";
            this.rbInward.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(180, 93);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 21);
            this.button1.TabIndex = 28;
            this.button1.Text = "Json File Name";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // txtJsonFile
            // 
            this.txtJsonFile.Location = new System.Drawing.Point(261, 94);
            this.txtJsonFile.Name = "txtJsonFile";
            this.txtJsonFile.Size = new System.Drawing.Size(494, 20);
            this.txtJsonFile.TabIndex = 11;
            // 
            // btnJsonFile
            // 
            this.btnJsonFile.Location = new System.Drawing.Point(758, 92);
            this.btnJsonFile.Name = "btnJsonFile";
            this.btnJsonFile.Size = new System.Drawing.Size(24, 23);
            this.btnJsonFile.TabIndex = 12;
            this.btnJsonFile.UseVisualStyleBackColor = true;
            this.btnJsonFile.Click += new System.EventHandler(this.btnJsonFile_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.Location = new System.Drawing.Point(99, 91);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 25);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "Cancel EWB";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 407);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnJsonFile);
            this.Controls.Add(this.txtJsonFile);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSelctAll);
            this.Controls.Add(this.dgvGSTIN);
            this.Controls.Add(this.gbxFilter);
            this.Controls.Add(this.btnJson);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTIN)).EndInit();
            this.gbxFilter.ResumeLayout(false);
            this.gbxFilter.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelctAll;
        private System.Windows.Forms.TextBox txtGrpId;
        private System.Windows.Forms.Button btnTrnName;
        private System.Windows.Forms.Button btnPartyNm;
        private System.Windows.Forms.Button btnJson;
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtJsonFile;
        private System.Windows.Forms.Button btnJsonFile;
        private System.Windows.Forms.Button btnCancel;
    }
}

