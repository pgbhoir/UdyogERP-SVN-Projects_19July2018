namespace ugstEWB_ItMast_JSON
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
            this.txtJsonFile = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnStatus = new System.Windows.Forms.Button();
            this.btnItTyp = new System.Windows.Forms.Button();
            this.btnGrpNm = new System.Windows.Forms.Button();
            this.btnItNm = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.txtItTyp = new System.Windows.Forms.TextBox();
            this.txtGrpNm = new System.Windows.Forms.TextBox();
            this.txtItNm = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnJsonFile = new System.Windows.Forms.Button();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnSelctAll = new System.Windows.Forms.Button();
            this.dgvItDet = new System.Windows.Forms.DataGridView();
            this.btnJson = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.gbxFilter = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItDet)).BeginInit();
            this.gbxFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtJsonFile
            // 
            this.txtJsonFile.Location = new System.Drawing.Point(200, 100);
            this.txtJsonFile.Name = "txtJsonFile";
            this.txtJsonFile.Size = new System.Drawing.Size(582, 20);
            this.txtJsonFile.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(843, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 21);
            this.button1.TabIndex = 10;
            this.button1.Text = "Json File Name";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnStatus
            // 
            this.btnStatus.Location = new System.Drawing.Point(687, 44);
            this.btnStatus.Name = "btnStatus";
            this.btnStatus.Size = new System.Drawing.Size(24, 23);
            this.btnStatus.TabIndex = 8;
            this.btnStatus.UseVisualStyleBackColor = true;
            this.btnStatus.Visible = false;
            // 
            // btnItTyp
            // 
            this.btnItTyp.Location = new System.Drawing.Point(270, 19);
            this.btnItTyp.Name = "btnItTyp";
            this.btnItTyp.Size = new System.Drawing.Size(24, 23);
            this.btnItTyp.TabIndex = 3;
            this.btnItTyp.UseVisualStyleBackColor = true;
            this.btnItTyp.Click += new System.EventHandler(this.btnItTyp_Click);
            // 
            // btnGrpNm
            // 
            this.btnGrpNm.Location = new System.Drawing.Point(270, 46);
            this.btnGrpNm.Name = "btnGrpNm";
            this.btnGrpNm.Size = new System.Drawing.Size(24, 23);
            this.btnGrpNm.TabIndex = 9;
            this.btnGrpNm.UseVisualStyleBackColor = true;
            this.btnGrpNm.Click += new System.EventHandler(this.btnGrpNm_Click);
            // 
            // btnItNm
            // 
            this.btnItNm.Location = new System.Drawing.Point(687, 19);
            this.btnItNm.Name = "btnItNm";
            this.btnItNm.Size = new System.Drawing.Size(24, 23);
            this.btnItNm.TabIndex = 6;
            this.btnItNm.UseVisualStyleBackColor = true;
            this.btnItNm.Click += new System.EventHandler(this.btnItNm_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(510, 46);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(172, 20);
            this.txtStatus.TabIndex = 7;
            this.txtStatus.Visible = false;
            // 
            // txtItTyp
            // 
            this.txtItTyp.Location = new System.Drawing.Point(92, 20);
            this.txtItTyp.Name = "txtItTyp";
            this.txtItTyp.ReadOnly = true;
            this.txtItTyp.Size = new System.Drawing.Size(172, 20);
            this.txtItTyp.TabIndex = 2;
            // 
            // txtGrpNm
            // 
            this.txtGrpNm.Location = new System.Drawing.Point(92, 46);
            this.txtGrpNm.Name = "txtGrpNm";
            this.txtGrpNm.ReadOnly = true;
            this.txtGrpNm.Size = new System.Drawing.Size(172, 20);
            this.txtGrpNm.TabIndex = 8;
            // 
            // txtItNm
            // 
            this.txtItNm.Location = new System.Drawing.Point(510, 20);
            this.txtItNm.Name = "txtItNm";
            this.txtItNm.ReadOnly = true;
            this.txtItNm.Size = new System.Drawing.Size(172, 20);
            this.txtItNm.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(434, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Status";
            this.label3.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Group Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(435, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Product Name";
            // 
            // btnJsonFile
            // 
            this.btnJsonFile.Location = new System.Drawing.Point(786, 98);
            this.btnJsonFile.Name = "btnJsonFile";
            this.btnJsonFile.Size = new System.Drawing.Size(24, 23);
            this.btnJsonFile.TabIndex = 13;
            this.btnJsonFile.UseVisualStyleBackColor = true;
            this.btnJsonFile.Click += new System.EventHandler(this.btnJsonFile_Click);
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "colSelect";
            this.colSelect.Name = "colSelect";
            this.colSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // btnSelctAll
            // 
            this.btnSelctAll.Location = new System.Drawing.Point(12, 99);
            this.btnSelctAll.Name = "btnSelctAll";
            this.btnSelctAll.Size = new System.Drawing.Size(75, 21);
            this.btnSelctAll.TabIndex = 10;
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
            this.dgvItDet.Location = new System.Drawing.Point(12, 121);
            this.dgvItDet.Name = "dgvItDet";
            this.dgvItDet.RowHeadersVisible = false;
            this.dgvItDet.Size = new System.Drawing.Size(1126, 263);
            this.dgvItDet.TabIndex = 15;
            this.dgvItDet.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItDet_CellContentClick);
            this.dgvItDet.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItDet_CellValueChanged);
            // 
            // btnJson
            // 
            this.btnJson.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnJson.Location = new System.Drawing.Point(816, 95);
            this.btnJson.Name = "btnJson";
            this.btnJson.Size = new System.Drawing.Size(110, 25);
            this.btnJson.TabIndex = 14;
            this.btnJson.Text = "Generate Json";
            this.btnJson.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJson.UseVisualStyleBackColor = true;
            this.btnJson.Click += new System.EventHandler(this.btnJson_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Product Type";
            // 
            // gbxFilter
            // 
            this.gbxFilter.Controls.Add(this.btnStatus);
            this.gbxFilter.Controls.Add(this.btnItTyp);
            this.gbxFilter.Controls.Add(this.btnGrpNm);
            this.gbxFilter.Controls.Add(this.btnItNm);
            this.gbxFilter.Controls.Add(this.txtStatus);
            this.gbxFilter.Controls.Add(this.txtItNm);
            this.gbxFilter.Controls.Add(this.txtGrpNm);
            this.gbxFilter.Controls.Add(this.txtItTyp);
            this.gbxFilter.Controls.Add(this.label3);
            this.gbxFilter.Controls.Add(this.label1);
            this.gbxFilter.Controls.Add(this.label6);
            this.gbxFilter.Controls.Add(this.label2);
            this.gbxFilter.Location = new System.Drawing.Point(12, 12);
            this.gbxFilter.Name = "gbxFilter";
            this.gbxFilter.Size = new System.Drawing.Size(722, 82);
            this.gbxFilter.TabIndex = 37;
            this.gbxFilter.TabStop = false;
            this.gbxFilter.Text = "Filter";
            this.gbxFilter.Enter += new System.EventHandler(this.gbxFilter_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(116, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Json File Path";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 390);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtJsonFile);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnJsonFile);
            this.Controls.Add(this.btnSelctAll);
            this.Controls.Add(this.dgvItDet);
            this.Controls.Add(this.btnJson);
            this.Controls.Add(this.gbxFilter);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItDet)).EndInit();
            this.gbxFilter.ResumeLayout(false);
            this.gbxFilter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtJsonFile;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnStatus;
        private System.Windows.Forms.Button btnItTyp;
        private System.Windows.Forms.Button btnGrpNm;
        private System.Windows.Forms.Button btnItNm;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.TextBox txtItTyp;
        private System.Windows.Forms.TextBox txtGrpNm;
        private System.Windows.Forms.TextBox txtItNm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnJsonFile;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.Button btnSelctAll;
        private System.Windows.Forms.DataGridView dgvItDet;
        private System.Windows.Forms.Button btnJson;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbxFilter;
        private System.Windows.Forms.Label label4;
    }
}

