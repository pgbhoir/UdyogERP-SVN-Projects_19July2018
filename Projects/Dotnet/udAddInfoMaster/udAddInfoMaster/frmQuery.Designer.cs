namespace udAddInfoMaster
{
    partial class frmQuery
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
            this.txtpara = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnChkQuery = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.colFldLst = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSearchFld = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colRetFld = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colExclFld = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCaption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.txtpara);
            this.gbMain.Controls.Add(this.label2);
            this.gbMain.Controls.Add(this.btnChkQuery);
            this.gbMain.Controls.Add(this.label1);
            this.gbMain.Controls.Add(this.dgvMain);
            this.gbMain.Controls.Add(this.txtQuery);
            this.gbMain.Location = new System.Drawing.Point(11, 7);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(669, 403);
            this.gbMain.TabIndex = 3;
            this.gbMain.TabStop = false;
            // 
            // txtpara
            // 
            this.txtpara.Location = new System.Drawing.Point(9, 323);
            this.txtpara.Multiline = true;
            this.txtpara.Name = "txtpara";
            this.txtpara.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtpara.Size = new System.Drawing.Size(650, 69);
            this.txtpara.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 292);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(650, 28);
            this.label2.TabIndex = 7;
            this.label2.Text = "Parameter :-  { SearchFieldList    #    ReturnField    [ #    ExcludeField    #  " +
    "  FieldCAption ] }";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnChkQuery
            // 
            this.btnChkQuery.Location = new System.Drawing.Point(279, 110);
            this.btnChkQuery.Name = "btnChkQuery";
            this.btnChkQuery.Size = new System.Drawing.Size(103, 23);
            this.btnChkQuery.TabIndex = 6;
            this.btnChkQuery.Text = "Check Query";
            this.btnChkQuery.UseVisualStyleBackColor = true;
            this.btnChkQuery.Click += new System.EventHandler(this.btnChkQuery_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(9, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(650, 28);
            this.label1.TabIndex = 5;
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFldLst,
            this.colSearchFld,
            this.colRetFld,
            this.colExclFld,
            this.colCaption});
            this.dgvMain.Location = new System.Drawing.Point(9, 139);
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.RowHeadersVisible = false;
            this.dgvMain.Size = new System.Drawing.Size(650, 150);
            this.dgvMain.TabIndex = 4;
            this.dgvMain.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMain_CellContentClick);
            this.dgvMain.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMain_CellValueChanged);
            this.dgvMain.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvMain_CurrentCellDirtyStateChanged);
            // 
            // colFldLst
            // 
            this.colFldLst.HeaderText = "Field List";
            this.colFldLst.Name = "colFldLst";
            this.colFldLst.ReadOnly = true;
            this.colFldLst.Width = 150;
            // 
            // colSearchFld
            // 
            this.colSearchFld.HeaderText = "Search Filed (*)";
            this.colSearchFld.Name = "colSearchFld";
            // 
            // colRetFld
            // 
            this.colRetFld.HeaderText = "Return Field (*)";
            this.colRetFld.Name = "colRetFld";
            // 
            // colExclFld
            // 
            this.colExclFld.HeaderText = "Exclude Field";
            this.colExclFld.Name = "colExclFld";
            // 
            // colCaption
            // 
            this.colCaption.FillWeight = 200F;
            this.colCaption.HeaderText = "Caption";
            this.colCaption.Name = "colCaption";
            this.colCaption.Width = 200;
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(9, 14);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtQuery.Size = new System.Drawing.Size(650, 91);
            this.txtQuery.TabIndex = 3;
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(169, 416);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(103, 23);
            this.btnDone.TabIndex = 9;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(350, 416);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(103, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 441);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.gbMain);
            this.Name = "frmQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmQuery";
            this.Load += new System.EventHandler(this.frmQuery_Load);
            this.gbMain.ResumeLayout(false);
            this.gbMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvMain;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.Button btnChkQuery;
        private System.Windows.Forms.TextBox txtpara;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFldLst;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSearchFld;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colRetFld;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colExclFld;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCaption;
    }
}