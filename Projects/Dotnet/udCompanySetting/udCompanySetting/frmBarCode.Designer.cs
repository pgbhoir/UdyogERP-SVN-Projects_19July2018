namespace udCompanySetting
{
    partial class frmBarCode
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
            this.dgvmain = new System.Windows.Forms.DataGridView();
            this.colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDefVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtType = new System.Windows.Forms.TextBox();
            this.btnType = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.nudColNo = new System.Windows.Forms.NumericUpDown();
            this.lblColNo = new System.Windows.Forms.Label();
            this.nudQrSize = new System.Windows.Forms.NumericUpDown();
            this.lblQrSize = new System.Windows.Forms.Label();
            this.chkText = new System.Windows.Forms.CheckBox();
            this.btnDone = new System.Windows.Forms.Button();
            this.gbMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvmain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQrSize)).BeginInit();
            this.SuspendLayout();
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.dgvmain);
            this.gbMain.Controls.Add(this.txtType);
            this.gbMain.Controls.Add(this.btnType);
            this.gbMain.Controls.Add(this.label4);
            this.gbMain.Controls.Add(this.nudColNo);
            this.gbMain.Controls.Add(this.lblColNo);
            this.gbMain.Controls.Add(this.nudQrSize);
            this.gbMain.Controls.Add(this.lblQrSize);
            this.gbMain.Controls.Add(this.chkText);
            this.gbMain.Location = new System.Drawing.Point(12, 12);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(428, 94);
            this.gbMain.TabIndex = 18;
            this.gbMain.TabStop = false;
            this.gbMain.Text = "Setting";
            // 
            // dgvmain
            // 
            this.dgvmain.AllowUserToAddRows = false;
            this.dgvmain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvmain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDesc,
            this.colDefVal});
            this.dgvmain.Location = new System.Drawing.Point(11, 72);
            this.dgvmain.Name = "dgvmain";
            this.dgvmain.RowHeadersVisible = false;
            this.dgvmain.Size = new System.Drawing.Size(404, 18);
            this.dgvmain.TabIndex = 40;
            this.dgvmain.Visible = false;
            // 
            // colDesc
            // 
            this.colDesc.HeaderText = "Description";
            this.colDesc.Name = "colDesc";
            this.colDesc.Width = 200;
            // 
            // colDefVal
            // 
            this.colDefVal.HeaderText = "Default Value";
            this.colDefVal.Name = "colDefVal";
            this.colDefVal.Width = 200;
            // 
            // txtType
            // 
            this.txtType.Location = new System.Drawing.Point(110, 17);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(268, 20);
            this.txtType.TabIndex = 39;
            // 
            // btnType
            // 
            this.btnType.Location = new System.Drawing.Point(383, 16);
            this.btnType.Name = "btnType";
            this.btnType.Size = new System.Drawing.Size(27, 23);
            this.btnType.TabIndex = 38;
            this.btnType.UseVisualStyleBackColor = true;
            this.btnType.Click += new System.EventHandler(this.btnType_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "Barcode Type";
            // 
            // nudColNo
            // 
            this.nudColNo.Location = new System.Drawing.Point(349, 43);
            this.nudColNo.Name = "nudColNo";
            this.nudColNo.Size = new System.Drawing.Size(49, 20);
            this.nudColNo.TabIndex = 22;
            this.nudColNo.Visible = false;
            // 
            // lblColNo
            // 
            this.lblColNo.AutoSize = true;
            this.lblColNo.Location = new System.Drawing.Point(264, 43);
            this.lblColNo.Name = "lblColNo";
            this.lblColNo.Size = new System.Drawing.Size(79, 13);
            this.lblColNo.TabIndex = 21;
            this.lblColNo.Text = "No. of Columns";
            this.lblColNo.Visible = false;
            // 
            // nudQrSize
            // 
            this.nudQrSize.Location = new System.Drawing.Point(110, 43);
            this.nudQrSize.Name = "nudQrSize";
            this.nudQrSize.Size = new System.Drawing.Size(49, 20);
            this.nudQrSize.TabIndex = 20;
            this.nudQrSize.Visible = false;
            // 
            // lblQrSize
            // 
            this.lblQrSize.AutoSize = true;
            this.lblQrSize.Location = new System.Drawing.Point(11, 43);
            this.lblQrSize.Name = "lblQrSize";
            this.lblQrSize.Size = new System.Drawing.Size(74, 13);
            this.lblQrSize.TabIndex = 19;
            this.lblQrSize.Text = "QR Code Size";
            this.lblQrSize.Visible = false;
            // 
            // chkText
            // 
            this.chkText.AutoSize = true;
            this.chkText.Location = new System.Drawing.Point(165, 43);
            this.chkText.Name = "chkText";
            this.chkText.Size = new System.Drawing.Size(93, 17);
            this.chkText.TabIndex = 14;
            this.chkText.Text = "Text Required";
            this.chkText.UseVisualStyleBackColor = true;
            this.chkText.Visible = false;
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(365, 110);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 37;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // frmBarCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 142);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.gbMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBarCode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBarCode";
            this.Load += new System.EventHandler(this.frmBarCode_Load);
            this.gbMain.ResumeLayout(false);
            this.gbMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvmain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQrSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.NumericUpDown nudQrSize;
        private System.Windows.Forms.Label lblQrSize;
        private System.Windows.Forms.CheckBox chkText;
        private System.Windows.Forms.NumericUpDown nudColNo;
        private System.Windows.Forms.Label lblColNo;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.Button btnType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.DataGridView dgvmain;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDefVal;
    }
}