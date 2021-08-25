namespace udCompanySetting
{
    partial class frmMultiUOM
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
            this.label8 = new System.Windows.Forms.Label();
            this.dgvmain = new System.Windows.Forms.DataGridView();
            this.colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nudNoUOM = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDone = new System.Windows.Forms.Button();
            this.uomCancel = new System.Windows.Forms.Button();
            this.gbMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvmain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNoUOM)).BeginInit();
            this.SuspendLayout();
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.label8);
            this.gbMain.Controls.Add(this.dgvmain);
            this.gbMain.Controls.Add(this.nudNoUOM);
            this.gbMain.Controls.Add(this.label1);
            this.gbMain.Location = new System.Drawing.Point(12, 12);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(219, 266);
            this.gbMain.TabIndex = 0;
            this.gbMain.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(9, 246);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(153, 15);
            this.label8.TabIndex = 50;
            this.label8.Text = "F2 : Select UOM Name";
            // 
            // dgvmain
            // 
            this.dgvmain.AllowUserToAddRows = false;
            this.dgvmain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvmain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colQty,
            this.colUOM});
            this.dgvmain.Location = new System.Drawing.Point(9, 40);
            this.dgvmain.Name = "dgvmain";
            this.dgvmain.RowHeadersVisible = false;
            this.dgvmain.Size = new System.Drawing.Size(197, 201);
            this.dgvmain.TabIndex = 23;
            this.dgvmain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvmain_KeyDown);
            // 
            // colQty
            // 
            this.colQty.HeaderText = "Quantity";
            this.colQty.Name = "colQty";
            // 
            // colUOM
            // 
            this.colUOM.HeaderText = "UOM";
            this.colUOM.Name = "colUOM";
            // 
            // nudNoUOM
            // 
            this.nudNoUOM.Location = new System.Drawing.Point(76, 14);
            this.nudNoUOM.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudNoUOM.Name = "nudNoUOM";
            this.nudNoUOM.Size = new System.Drawing.Size(65, 20);
            this.nudNoUOM.TabIndex = 22;
            this.nudNoUOM.ValueChanged += new System.EventHandler(this.nudNoUOM_ValueChanged);
            this.nudNoUOM.Validated += new System.EventHandler(this.nudNoUOM_Validated);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "No. of UOM";
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(156, 283);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 37;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // uomCancel
            // 
            this.uomCancel.Location = new System.Drawing.Point(74, 283);
            this.uomCancel.Name = "uomCancel";
            this.uomCancel.Size = new System.Drawing.Size(75, 23);
            this.uomCancel.TabIndex = 38;
            this.uomCancel.Text = "Cancel";
            this.uomCancel.UseVisualStyleBackColor = true;
            this.uomCancel.Click += new System.EventHandler(this.uomCancel_Click);
            // 
            // frmMultiUOM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 312);
            this.Controls.Add(this.uomCancel);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.gbMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMultiUOM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Multi UOM";
            this.Load += new System.EventHandler(this.frmMultiUOM_Load);
            this.gbMain.ResumeLayout(false);
            this.gbMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvmain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNoUOM)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.NumericUpDown nudNoUOM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvmain;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUOM;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button uomCancel;
        private System.Windows.Forms.Label label8;
    }
}