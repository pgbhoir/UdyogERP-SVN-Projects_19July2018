namespace ugstEWB_AcMast_JSON
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
            this.btnJson = new System.Windows.Forms.Button();
            this.gbxFilter = new System.Windows.Forms.GroupBox();
            this.chkCusSupp = new System.Windows.Forms.CheckBox();
            this.rbSupplier = new System.Windows.Forms.RadioButton();
            this.rbCustomer = new System.Windows.Forms.RadioButton();
            this.btnGrpNm = new System.Windows.Forms.Button();
            this.btnAcNm = new System.Windows.Forms.Button();
            this.txtGrpNm = new System.Windows.Forms.TextBox();
            this.txtAcNm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSelctAll = new System.Windows.Forms.Button();
            this.dgvItDet = new System.Windows.Forms.DataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnJsonFile = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtJsonFile = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gbxFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItDet)).BeginInit();
            this.SuspendLayout();
            // 
            // btnJson
            // 
            this.btnJson.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnJson.Location = new System.Drawing.Point(813, 91);
            this.btnJson.Name = "btnJson";
            this.btnJson.Size = new System.Drawing.Size(110, 25);
            this.btnJson.TabIndex = 10;
            this.btnJson.Text = "Generate Json";
            this.btnJson.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJson.UseVisualStyleBackColor = true;
            this.btnJson.Click += new System.EventHandler(this.btnJson_Click);
            // 
            // gbxFilter
            // 
            this.gbxFilter.Controls.Add(this.chkCusSupp);
            this.gbxFilter.Controls.Add(this.rbSupplier);
            this.gbxFilter.Controls.Add(this.rbCustomer);
            this.gbxFilter.Controls.Add(this.btnGrpNm);
            this.gbxFilter.Controls.Add(this.btnAcNm);
            this.gbxFilter.Controls.Add(this.txtGrpNm);
            this.gbxFilter.Controls.Add(this.txtAcNm);
            this.gbxFilter.Controls.Add(this.label1);
            this.gbxFilter.Controls.Add(this.label6);
            this.gbxFilter.Location = new System.Drawing.Point(9, 8);
            this.gbxFilter.Name = "gbxFilter";
            this.gbxFilter.Size = new System.Drawing.Size(914, 82);
            this.gbxFilter.TabIndex = 44;
            this.gbxFilter.TabStop = false;
            this.gbxFilter.Text = "Filter";
            // 
            // chkCusSupp
            // 
            this.chkCusSupp.AutoSize = true;
            this.chkCusSupp.Location = new System.Drawing.Point(161, 19);
            this.chkCusSupp.Name = "chkCusSupp";
            this.chkCusSupp.Size = new System.Drawing.Size(194, 17);
            this.chkCusSupp.TabIndex = 3;
            this.chkCusSupp.Text = "Display Both Customer and Supplier";
            this.chkCusSupp.UseVisualStyleBackColor = true;
            this.chkCusSupp.CheckedChanged += new System.EventHandler(this.chkCusSupp_CheckedChanged);
            // 
            // rbSupplier
            // 
            this.rbSupplier.AutoSize = true;
            this.rbSupplier.Location = new System.Drawing.Point(92, 19);
            this.rbSupplier.Name = "rbSupplier";
            this.rbSupplier.Size = new System.Drawing.Size(63, 17);
            this.rbSupplier.TabIndex = 2;
            this.rbSupplier.TabStop = true;
            this.rbSupplier.Text = "Supplier";
            this.rbSupplier.UseVisualStyleBackColor = true;
            this.rbSupplier.CheckedChanged += new System.EventHandler(this.rbSupplier_CheckedChanged);
            // 
            // rbCustomer
            // 
            this.rbCustomer.AutoSize = true;
            this.rbCustomer.Location = new System.Drawing.Point(22, 19);
            this.rbCustomer.Name = "rbCustomer";
            this.rbCustomer.Size = new System.Drawing.Size(69, 17);
            this.rbCustomer.TabIndex = 1;
            this.rbCustomer.TabStop = true;
            this.rbCustomer.Text = "Customer";
            this.rbCustomer.UseVisualStyleBackColor = true;
            this.rbCustomer.CheckedChanged += new System.EventHandler(this.rbCustomer_CheckedChanged);
            // 
            // btnGrpNm
            // 
            this.btnGrpNm.Location = new System.Drawing.Point(876, 17);
            this.btnGrpNm.Name = "btnGrpNm";
            this.btnGrpNm.Size = new System.Drawing.Size(24, 23);
            this.btnGrpNm.TabIndex = 5;
            this.btnGrpNm.UseVisualStyleBackColor = true;
            this.btnGrpNm.Click += new System.EventHandler(this.btnGrpNm_Click);
            // 
            // btnAcNm
            // 
            this.btnAcNm.Location = new System.Drawing.Point(360, 52);
            this.btnAcNm.Name = "btnAcNm";
            this.btnAcNm.Size = new System.Drawing.Size(24, 23);
            this.btnAcNm.TabIndex = 7;
            this.btnAcNm.UseVisualStyleBackColor = true;
            this.btnAcNm.Click += new System.EventHandler(this.btnAcNm_Click);
            // 
            // txtGrpNm
            // 
            this.txtGrpNm.Location = new System.Drawing.Point(466, 17);
            this.txtGrpNm.Name = "txtGrpNm";
            this.txtGrpNm.ReadOnly = true;
            this.txtGrpNm.Size = new System.Drawing.Size(406, 20);
            this.txtGrpNm.TabIndex = 4;
            // 
            // txtAcNm
            // 
            this.txtAcNm.Location = new System.Drawing.Point(92, 52);
            this.txtAcNm.Name = "txtAcNm";
            this.txtAcNm.ReadOnly = true;
            this.txtAcNm.Size = new System.Drawing.Size(265, 20);
            this.txtAcNm.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(392, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Group Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Party Name";
            // 
            // btnSelctAll
            // 
            this.btnSelctAll.Location = new System.Drawing.Point(9, 95);
            this.btnSelctAll.Name = "btnSelctAll";
            this.btnSelctAll.Size = new System.Drawing.Size(75, 21);
            this.btnSelctAll.TabIndex = 46;
            this.btnSelctAll.Text = "Select All";
            this.btnSelctAll.UseVisualStyleBackColor = true;
            this.btnSelctAll.Click += new System.EventHandler(this.btnSelctAll_Click);
            // 
            // dgvItDet
            // 
            this.dgvItDet.AllowUserToAddRows = false;
            this.dgvItDet.AllowUserToDeleteRows = false;
            this.dgvItDet.AllowUserToResizeRows = false;
            this.dgvItDet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItDet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItDet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect});
            this.dgvItDet.Location = new System.Drawing.Point(9, 117);
            this.dgvItDet.Name = "dgvItDet";
            this.dgvItDet.RowHeadersVisible = false;
            this.dgvItDet.Size = new System.Drawing.Size(1126, 263);
            this.dgvItDet.TabIndex = 11;
            this.dgvItDet.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItDet_CellValueChanged);
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "colSelect";
            this.colSelect.Name = "colSelect";
            this.colSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // btnJsonFile
            // 
            this.btnJsonFile.Location = new System.Drawing.Point(783, 94);
            this.btnJsonFile.Name = "btnJsonFile";
            this.btnJsonFile.Size = new System.Drawing.Size(24, 23);
            this.btnJsonFile.TabIndex = 9;
            this.btnJsonFile.UseVisualStyleBackColor = true;
            this.btnJsonFile.Click += new System.EventHandler(this.btnJsonFile_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(938, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 21);
            this.button1.TabIndex = 48;
            this.button1.Text = "Json File Name";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtJsonFile
            // 
            this.txtJsonFile.Location = new System.Drawing.Point(197, 96);
            this.txtJsonFile.Name = "txtJsonFile";
            this.txtJsonFile.Size = new System.Drawing.Size(582, 20);
            this.txtJsonFile.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(112, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 49;
            this.label4.Text = "Json File Path";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 383);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnJson);
            this.Controls.Add(this.gbxFilter);
            this.Controls.Add(this.btnSelctAll);
            this.Controls.Add(this.dgvItDet);
            this.Controls.Add(this.btnJsonFile);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtJsonFile);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.gbxFilter.ResumeLayout(false);
            this.gbxFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItDet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnJson;
        private System.Windows.Forms.GroupBox gbxFilter;
        private System.Windows.Forms.Button btnGrpNm;
        private System.Windows.Forms.Button btnAcNm;
        private System.Windows.Forms.TextBox txtGrpNm;
        private System.Windows.Forms.TextBox txtAcNm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSelctAll;
        private System.Windows.Forms.DataGridView dgvItDet;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.Button btnJsonFile;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtJsonFile;
        private System.Windows.Forms.RadioButton rbSupplier;
        private System.Windows.Forms.RadioButton rbCustomer;
        private System.Windows.Forms.CheckBox chkCusSupp;
        private System.Windows.Forms.Label label4;
    }
}

