namespace udCompanySetting
{
    partial class frmInventoryCond
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
            this.gbFlds = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvFlds = new System.Windows.Forms.DataGridView();
            this.colFldNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTblNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.uomCancel = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.gbFlds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFlds)).BeginInit();
            this.SuspendLayout();
            // 
            // gbFlds
            // 
            this.gbFlds.Controls.Add(this.label6);
            this.gbFlds.Controls.Add(this.dgvFlds);
            this.gbFlds.Controls.Add(this.label7);
            this.gbFlds.Controls.Add(this.label8);
            this.gbFlds.Location = new System.Drawing.Point(5, 10);
            this.gbFlds.Name = "gbFlds";
            this.gbFlds.Size = new System.Drawing.Size(454, 401);
            this.gbFlds.TabIndex = 28;
            this.gbFlds.TabStop = false;
            this.gbFlds.Text = "Condition Builder";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(135, 375);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(154, 15);
            this.label6.TabIndex = 51;
            this.label6.Text = "CTRL+T : Remove Line";
            // 
            // dgvFlds
            // 
            this.dgvFlds.AllowUserToAddRows = false;
            this.dgvFlds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFlds.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFldNm,
            this.colDesc,
            this.colTblNm});
            this.dgvFlds.Location = new System.Drawing.Point(9, 20);
            this.dgvFlds.Name = "dgvFlds";
            this.dgvFlds.RowHeadersVisible = false;
            this.dgvFlds.Size = new System.Drawing.Size(433, 350);
            this.dgvFlds.TabIndex = 23;
            this.dgvFlds.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFlds_CellContentClick);
            this.dgvFlds.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvFlds_CellValidating);
            this.dgvFlds.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvFlds_KeyDown);
            this.dgvFlds.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvFlds_MouseClick);
            // 
            // colFldNm
            // 
            this.colFldNm.HeaderText = "Field Name";
            this.colFldNm.Name = "colFldNm";
            this.colFldNm.ReadOnly = true;
            this.colFldNm.Width = 110;
            // 
            // colDesc
            // 
            this.colDesc.HeaderText = "Description";
            this.colDesc.Name = "colDesc";
            this.colDesc.Width = 320;
            // 
            // colTblNm
            // 
            this.colTblNm.HeaderText = "Table Name";
            this.colTblNm.Name = "colTblNm";
            this.colTblNm.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(7, 375);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 15);
            this.label7.TabIndex = 50;
            this.label7.Text = "CTRL+I : Add Line";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(295, 375);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(153, 15);
            this.label8.TabIndex = 49;
            this.label8.Text = "F2 : Select Field Name";
            // 
            // uomCancel
            // 
            this.uomCancel.Location = new System.Drawing.Point(303, 417);
            this.uomCancel.Name = "uomCancel";
            this.uomCancel.Size = new System.Drawing.Size(75, 23);
            this.uomCancel.TabIndex = 42;
            this.uomCancel.Text = "Cancel";
            this.uomCancel.UseVisualStyleBackColor = true;
            this.uomCancel.Click += new System.EventHandler(this.uomCancel_Click);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(384, 417);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 41;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // frmInventoryCond
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 446);
            this.Controls.Add(this.uomCancel);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.gbFlds);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInventoryCond";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmInventoryCond";
            this.Load += new System.EventHandler(this.frmInventoryCond_Load);
            this.gbFlds.ResumeLayout(false);
            this.gbFlds.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFlds)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFlds;
        private System.Windows.Forms.DataGridView dgvFlds;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button uomCancel;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFldNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTblNm;
    }
}