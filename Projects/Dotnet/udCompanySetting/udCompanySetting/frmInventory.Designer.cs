namespace udCompanySetting
{
    partial class frmInventory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInventory));
            this.gbgen = new System.Windows.Forms.GroupBox();
            this.nudRateDeci = new System.Windows.Forms.NumericUpDown();
            this.lblRateDeci = new System.Windows.Forms.Label();
            this.nudQtyDeci = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.chkDisplayInStk = new System.Windows.Forms.CheckBox();
            this.chkPartyPrice = new System.Windows.Forms.CheckBox();
            this.chkNegBal = new System.Windows.Forms.CheckBox();
            this.chkonLineBal = new System.Windows.Forms.CheckBox();
            this.uomCancel = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.gbBatchProcess = new System.Windows.Forms.GroupBox();
            this.chkAutoBatch = new System.Windows.Forms.CheckBox();
            this.rdbGoods = new System.Windows.Forms.RadioButton();
            this.rdbRun = new System.Windows.Forms.RadioButton();
            this.gbStkVal = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvStkVal = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colbtnCond = new System.Windows.Forms.DataGridViewImageColumn();
            this.colValuation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOpStkAc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClStkPLAc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClStkBsAc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCond = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbgen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRateDeci)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQtyDeci)).BeginInit();
            this.gbBatchProcess.SuspendLayout();
            this.gbStkVal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStkVal)).BeginInit();
            this.SuspendLayout();
            // 
            // gbgen
            // 
            this.gbgen.Controls.Add(this.nudRateDeci);
            this.gbgen.Controls.Add(this.lblRateDeci);
            this.gbgen.Controls.Add(this.nudQtyDeci);
            this.gbgen.Controls.Add(this.label1);
            this.gbgen.Controls.Add(this.chkDisplayInStk);
            this.gbgen.Controls.Add(this.chkPartyPrice);
            this.gbgen.Controls.Add(this.chkNegBal);
            this.gbgen.Controls.Add(this.chkonLineBal);
            this.gbgen.Location = new System.Drawing.Point(12, 16);
            this.gbgen.Name = "gbgen";
            this.gbgen.Size = new System.Drawing.Size(585, 71);
            this.gbgen.TabIndex = 0;
            this.gbgen.TabStop = false;
            this.gbgen.Text = "General Details";
            // 
            // nudRateDeci
            // 
            this.nudRateDeci.Location = new System.Drawing.Point(520, 41);
            this.nudRateDeci.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudRateDeci.Name = "nudRateDeci";
            this.nudRateDeci.Size = new System.Drawing.Size(44, 20);
            this.nudRateDeci.TabIndex = 5;
            // 
            // lblRateDeci
            // 
            this.lblRateDeci.AutoSize = true;
            this.lblRateDeci.Location = new System.Drawing.Point(391, 41);
            this.lblRateDeci.Name = "lblRateDeci";
            this.lblRateDeci.Size = new System.Drawing.Size(114, 13);
            this.lblRateDeci.TabIndex = 25;
            this.lblRateDeci.Text = "Decimal Points in Rate";
            // 
            // nudQtyDeci
            // 
            this.nudQtyDeci.Location = new System.Drawing.Point(520, 19);
            this.nudQtyDeci.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudQtyDeci.Name = "nudQtyDeci";
            this.nudQtyDeci.Size = new System.Drawing.Size(44, 20);
            this.nudQtyDeci.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(391, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Decimal Points in Quantity";
            // 
            // chkDisplayInStk
            // 
            this.chkDisplayInStk.AutoSize = true;
            this.chkDisplayInStk.Location = new System.Drawing.Point(202, 41);
            this.chkDisplayInStk.Name = "chkDisplayInStk";
            this.chkDisplayInStk.Size = new System.Drawing.Size(161, 17);
            this.chkDisplayInStk.TabIndex = 4;
            this.chkDisplayInStk.Text = "Display Only In-Stock Goods";
            this.chkDisplayInStk.UseVisualStyleBackColor = true;
            this.chkDisplayInStk.Visible = false;
            // 
            // chkPartyPrice
            // 
            this.chkPartyPrice.AutoSize = true;
            this.chkPartyPrice.Location = new System.Drawing.Point(6, 41);
            this.chkPartyPrice.Name = "chkPartyPrice";
            this.chkPartyPrice.Size = new System.Drawing.Size(186, 17);
            this.chkPartyPrice.TabIndex = 3;
            this.chkPartyPrice.Text = "Display Only The Party\'s Price List";
            this.chkPartyPrice.UseVisualStyleBackColor = true;
            this.chkPartyPrice.Visible = false;
            // 
            // chkNegBal
            // 
            this.chkNegBal.AutoSize = true;
            this.chkNegBal.Location = new System.Drawing.Point(202, 19);
            this.chkNegBal.Name = "chkNegBal";
            this.chkNegBal.Size = new System.Drawing.Size(179, 17);
            this.chkNegBal.TabIndex = 1;
            this.chkNegBal.Text = "Goods Negative Balance Allow  ";
            this.chkNegBal.UseVisualStyleBackColor = true;
            // 
            // chkonLineBal
            // 
            this.chkonLineBal.AutoSize = true;
            this.chkonLineBal.Location = new System.Drawing.Point(6, 19);
            this.chkonLineBal.Name = "chkonLineBal";
            this.chkonLineBal.Size = new System.Drawing.Size(179, 17);
            this.chkonLineBal.TabIndex = 0;
            this.chkonLineBal.Text = "Goods On Line Balance Check  ";
            this.chkonLineBal.UseVisualStyleBackColor = true;
            // 
            // uomCancel
            // 
            this.uomCancel.Location = new System.Drawing.Point(645, 295);
            this.uomCancel.Name = "uomCancel";
            this.uomCancel.Size = new System.Drawing.Size(75, 23);
            this.uomCancel.TabIndex = 3;
            this.uomCancel.Text = "Cancel";
            this.uomCancel.UseVisualStyleBackColor = true;
            this.uomCancel.Click += new System.EventHandler(this.uomCancel_Click);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(729, 295);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 4;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // gbBatchProcess
            // 
            this.gbBatchProcess.Controls.Add(this.chkAutoBatch);
            this.gbBatchProcess.Controls.Add(this.rdbGoods);
            this.gbBatchProcess.Controls.Add(this.rdbRun);
            this.gbBatchProcess.Location = new System.Drawing.Point(610, 16);
            this.gbBatchProcess.Name = "gbBatchProcess";
            this.gbBatchProcess.Size = new System.Drawing.Size(194, 71);
            this.gbBatchProcess.TabIndex = 1;
            this.gbBatchProcess.TabStop = false;
            this.gbBatchProcess.Text = "Batch Processing Details";
            // 
            // chkAutoBatch
            // 
            this.chkAutoBatch.AutoSize = true;
            this.chkAutoBatch.Location = new System.Drawing.Point(6, 18);
            this.chkAutoBatch.Name = "chkAutoBatch";
            this.chkAutoBatch.Size = new System.Drawing.Size(147, 17);
            this.chkAutoBatch.TabIndex = 0;
            this.chkAutoBatch.Text = "Auto Batch Process Type";
            this.chkAutoBatch.UseVisualStyleBackColor = true;
            this.chkAutoBatch.CheckedChanged += new System.EventHandler(this.chkAutoBatch_CheckedChanged);
            // 
            // rdbGoods
            // 
            this.rdbGoods.AutoSize = true;
            this.rdbGoods.Location = new System.Drawing.Point(83, 41);
            this.rdbGoods.Name = "rdbGoods";
            this.rdbGoods.Size = new System.Drawing.Size(77, 17);
            this.rdbGoods.TabIndex = 2;
            this.rdbGoods.TabStop = true;
            this.rdbGoods.Text = "Goodswise";
            this.rdbGoods.UseVisualStyleBackColor = true;
            // 
            // rdbRun
            // 
            this.rdbRun.AutoSize = true;
            this.rdbRun.Location = new System.Drawing.Point(8, 41);
            this.rdbRun.Name = "rdbRun";
            this.rdbRun.Size = new System.Drawing.Size(65, 17);
            this.rdbRun.TabIndex = 1;
            this.rdbRun.TabStop = true;
            this.rdbRun.Text = "Running";
            this.rdbRun.UseVisualStyleBackColor = true;
            // 
            // gbStkVal
            // 
            this.gbStkVal.Controls.Add(this.label3);
            this.gbStkVal.Controls.Add(this.label2);
            this.gbStkVal.Controls.Add(this.label5);
            this.gbStkVal.Controls.Add(this.dgvStkVal);
            this.gbStkVal.Location = new System.Drawing.Point(12, 89);
            this.gbStkVal.Name = "gbStkVal";
            this.gbStkVal.Size = new System.Drawing.Size(792, 200);
            this.gbStkVal.TabIndex = 2;
            this.gbStkVal.TabStop = false;
            this.gbStkVal.Text = "Stock Valuation and Related Ledgers";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(141, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 15);
            this.label3.TabIndex = 48;
            this.label3.Text = "CTRL+T : Remove Line";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 15);
            this.label2.TabIndex = 47;
            this.label2.Text = "CTRL+I : Add Line";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(301, 177);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 15);
            this.label5.TabIndex = 46;
            this.label5.Text = "F2 : Select Valuation Type";
            // 
            // dgvStkVal
            // 
            this.dgvStkVal.AllowUserToAddRows = false;
            this.dgvStkVal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStkVal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colbtnCond,
            this.colValuation,
            this.colOpStkAc,
            this.colClStkPLAc,
            this.colClStkBsAc,
            this.colCond});
            this.dgvStkVal.Location = new System.Drawing.Point(9, 19);
            this.dgvStkVal.Name = "dgvStkVal";
            this.dgvStkVal.RowHeadersVisible = false;
            this.dgvStkVal.Size = new System.Drawing.Size(777, 153);
            this.dgvStkVal.TabIndex = 0;
            this.dgvStkVal.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStkVal_CellContentClick);
            this.dgvStkVal.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvStkVal_CellValidating);
            this.dgvStkVal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvStkVal_KeyDown);
            this.dgvStkVal.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvStkVal_MouseClick);
            // 
            // colName
            // 
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            // 
            // colbtnCond
            // 
            this.colbtnCond.HeaderText = "Condition";
            this.colbtnCond.Image = ((System.Drawing.Image)(resources.GetObject("colbtnCond.Image")));
            this.colbtnCond.Name = "colbtnCond";
            this.colbtnCond.ReadOnly = true;
            this.colbtnCond.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colbtnCond.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colValuation
            // 
            this.colValuation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colValuation.HeaderText = "Valuation Type";
            this.colValuation.Name = "colValuation";
            this.colValuation.ReadOnly = true;
            this.colValuation.Width = 120;
            // 
            // colOpStkAc
            // 
            this.colOpStkAc.HeaderText = "Opening Stock Account Posting";
            this.colOpStkAc.Name = "colOpStkAc";
            this.colOpStkAc.ReadOnly = true;
            this.colOpStkAc.Visible = false;
            this.colOpStkAc.Width = 250;
            // 
            // colClStkPLAc
            // 
            this.colClStkPLAc.HeaderText = "Closing Stock (P & L) Account Posting";
            this.colClStkPLAc.Name = "colClStkPLAc";
            this.colClStkPLAc.ReadOnly = true;
            this.colClStkPLAc.Visible = false;
            this.colClStkPLAc.Width = 250;
            // 
            // colClStkBsAc
            // 
            this.colClStkBsAc.HeaderText = "Closing Stock (B/S) Account Posting";
            this.colClStkBsAc.Name = "colClStkBsAc";
            this.colClStkBsAc.ReadOnly = true;
            this.colClStkBsAc.Visible = false;
            this.colClStkBsAc.Width = 250;
            // 
            // colCond
            // 
            this.colCond.HeaderText = "Condition";
            this.colCond.Name = "colCond";
            this.colCond.Visible = false;
            // 
            // frmInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 324);
            this.Controls.Add(this.gbStkVal);
            this.Controls.Add(this.gbBatchProcess);
            this.Controls.Add(this.uomCancel);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.gbgen);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInventory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmInventory";
            this.Load += new System.EventHandler(this.frmInventory_Load);
            this.gbgen.ResumeLayout(false);
            this.gbgen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRateDeci)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQtyDeci)).EndInit();
            this.gbBatchProcess.ResumeLayout(false);
            this.gbBatchProcess.PerformLayout();
            this.gbStkVal.ResumeLayout(false);
            this.gbStkVal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStkVal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbgen;
        private System.Windows.Forms.CheckBox chkDisplayInStk;
        private System.Windows.Forms.CheckBox chkPartyPrice;
        private System.Windows.Forms.CheckBox chkNegBal;
        private System.Windows.Forms.CheckBox chkonLineBal;
        private System.Windows.Forms.NumericUpDown nudRateDeci;
        private System.Windows.Forms.Label lblRateDeci;
        private System.Windows.Forms.NumericUpDown nudQtyDeci;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button uomCancel;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.GroupBox gbBatchProcess;
        private System.Windows.Forms.RadioButton rdbGoods;
        private System.Windows.Forms.RadioButton rdbRun;
        private System.Windows.Forms.GroupBox gbStkVal;
        private System.Windows.Forms.DataGridView dgvStkVal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkAutoBatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewImageColumn colbtnCond;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValuation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOpStkAc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClStkPLAc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClStkBsAc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCond;
    }
}