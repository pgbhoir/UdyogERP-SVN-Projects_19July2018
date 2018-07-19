namespace CustModAccUI.UI
{
    partial class frmMenuTranRept
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMenuTranRept));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpReport = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbl_rpt_total = new System.Windows.Forms.Label();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.colRptSrno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rep_nm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpTran = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_tran_total = new System.Windows.Forms.Label();
            this.dgvTran = new System.Windows.Forms.DataGridView();
            this.colTranSrno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entry_ty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code_nm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpMenu = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_mnu_total = new System.Windows.Forms.Label();
            this.dgvMenu = new System.Windows.Forms.DataGridView();
            this.colMnuSrno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.padname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.btn_rptDelete = new System.Windows.Forms.Button();
            this.btn_rptAdd = new System.Windows.Forms.Button();
            this.btn_tranDelete = new System.Windows.Forms.Button();
            this.btn_tranAdd = new System.Windows.Forms.Button();
            this.btn_mnuDelete = new System.Windows.Forms.Button();
            this.btn_mnuAdd = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.grpReport.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.grpTran.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTran)).BeginInit();
            this.grpMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnDone);
            this.groupBox1.Controls.Add(this.grpReport);
            this.groupBox1.Controls.Add(this.grpTran);
            this.groupBox1.Controls.Add(this.grpMenu);
            this.groupBox1.Location = new System.Drawing.Point(12, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(663, 608);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // grpReport
            // 
            this.grpReport.Controls.Add(this.panel3);
            this.grpReport.Controls.Add(this.btn_rptDelete);
            this.grpReport.Controls.Add(this.btn_rptAdd);
            this.grpReport.Controls.Add(this.dgvReport);
            this.grpReport.Location = new System.Drawing.Point(9, 388);
            this.grpReport.Name = "grpReport";
            this.grpReport.Size = new System.Drawing.Size(644, 189);
            this.grpReport.TabIndex = 2;
            this.grpReport.TabStop = false;
            this.grpReport.Text = "Report";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.lbl_rpt_total);
            this.panel3.Location = new System.Drawing.Point(12, 165);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(619, 18);
            this.panel3.TabIndex = 6;
            // 
            // lbl_rpt_total
            // 
            this.lbl_rpt_total.AutoSize = true;
            this.lbl_rpt_total.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbl_rpt_total.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_rpt_total.ForeColor = System.Drawing.Color.Crimson;
            this.lbl_rpt_total.Location = new System.Drawing.Point(3, 0);
            this.lbl_rpt_total.Name = "lbl_rpt_total";
            this.lbl_rpt_total.Size = new System.Drawing.Size(19, 13);
            this.lbl_rpt_total.TabIndex = 3;
            this.lbl_rpt_total.Text = "00";
            this.lbl_rpt_total.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AllowUserToResizeRows = false;
            this.dgvReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRptSrno,
            this.group,
            this.desc,
            this.rep_nm});
            this.dgvReport.Location = new System.Drawing.Point(12, 35);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvReport.Size = new System.Drawing.Size(618, 132);
            this.dgvReport.TabIndex = 2;
            this.dgvReport.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvReport_RowValidating);
            this.dgvReport.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvReport_EditingControlShowing);
            this.dgvReport.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvReport_KeyDown);
            // 
            // colRptSrno
            // 
            this.colRptSrno.DataPropertyName = "srno";
            this.colRptSrno.FillWeight = 60.9137F;
            this.colRptSrno.HeaderText = "Sr. No.";
            this.colRptSrno.Name = "colRptSrno";
            this.colRptSrno.ReadOnly = true;
            // 
            // group
            // 
            this.group.DataPropertyName = "desc1";
            this.group.FillWeight = 113.0288F;
            this.group.HeaderText = "Group";
            this.group.Name = "group";
            // 
            // desc
            // 
            this.desc.DataPropertyName = "desc2";
            this.desc.FillWeight = 113.0288F;
            this.desc.HeaderText = "Description";
            this.desc.Name = "desc";
            // 
            // rep_nm
            // 
            this.rep_nm.DataPropertyName = "desc3";
            this.rep_nm.FillWeight = 113.0288F;
            this.rep_nm.HeaderText = "Report Name";
            this.rep_nm.Name = "rep_nm";
            // 
            // grpTran
            // 
            this.grpTran.Controls.Add(this.panel2);
            this.grpTran.Controls.Add(this.btn_tranDelete);
            this.grpTran.Controls.Add(this.btn_tranAdd);
            this.grpTran.Controls.Add(this.dgvTran);
            this.grpTran.Location = new System.Drawing.Point(9, 198);
            this.grpTran.Name = "grpTran";
            this.grpTran.Size = new System.Drawing.Size(644, 189);
            this.grpTran.TabIndex = 1;
            this.grpTran.TabStop = false;
            this.grpTran.Text = "Transaction";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.lbl_tran_total);
            this.panel2.Location = new System.Drawing.Point(12, 166);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(619, 18);
            this.panel2.TabIndex = 5;
            // 
            // lbl_tran_total
            // 
            this.lbl_tran_total.AutoSize = true;
            this.lbl_tran_total.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbl_tran_total.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_tran_total.ForeColor = System.Drawing.Color.Crimson;
            this.lbl_tran_total.Location = new System.Drawing.Point(3, 0);
            this.lbl_tran_total.Name = "lbl_tran_total";
            this.lbl_tran_total.Size = new System.Drawing.Size(19, 13);
            this.lbl_tran_total.TabIndex = 3;
            this.lbl_tran_total.Text = "00";
            this.lbl_tran_total.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dgvTran
            // 
            this.dgvTran.AllowUserToAddRows = false;
            this.dgvTran.AllowUserToDeleteRows = false;
            this.dgvTran.AllowUserToResizeRows = false;
            this.dgvTran.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTran.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTran.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTranSrno,
            this.entry_ty,
            this.code_nm});
            this.dgvTran.Location = new System.Drawing.Point(12, 35);
            this.dgvTran.Name = "dgvTran";
            this.dgvTran.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTran.Size = new System.Drawing.Size(618, 132);
            this.dgvTran.TabIndex = 2;
            this.dgvTran.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvTran_RowValidating);
            this.dgvTran.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvTran_EditingControlShowing);
            this.dgvTran.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTran_KeyDown);
            // 
            // colTranSrno
            // 
            this.colTranSrno.DataPropertyName = "srno";
            this.colTranSrno.FillWeight = 45.68528F;
            this.colTranSrno.HeaderText = "Sr. No.";
            this.colTranSrno.Name = "colTranSrno";
            this.colTranSrno.ReadOnly = true;
            // 
            // entry_ty
            // 
            this.entry_ty.DataPropertyName = "desc1";
            this.entry_ty.FillWeight = 127.1574F;
            this.entry_ty.HeaderText = "Transaction Type";
            this.entry_ty.Name = "entry_ty";
            // 
            // code_nm
            // 
            this.code_nm.DataPropertyName = "desc2";
            this.code_nm.FillWeight = 127.1574F;
            this.code_nm.HeaderText = "Transaction Name";
            this.code_nm.Name = "code_nm";
            // 
            // grpMenu
            // 
            this.grpMenu.Controls.Add(this.panel1);
            this.grpMenu.Controls.Add(this.btn_mnuDelete);
            this.grpMenu.Controls.Add(this.btn_mnuAdd);
            this.grpMenu.Controls.Add(this.dgvMenu);
            this.grpMenu.Location = new System.Drawing.Point(9, 9);
            this.grpMenu.Name = "grpMenu";
            this.grpMenu.Size = new System.Drawing.Size(644, 189);
            this.grpMenu.TabIndex = 0;
            this.grpMenu.TabStop = false;
            this.grpMenu.Text = "Menu";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lbl_mnu_total);
            this.panel1.Location = new System.Drawing.Point(12, 166);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(619, 18);
            this.panel1.TabIndex = 4;
            // 
            // lbl_mnu_total
            // 
            this.lbl_mnu_total.AutoSize = true;
            this.lbl_mnu_total.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbl_mnu_total.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_mnu_total.ForeColor = System.Drawing.Color.Crimson;
            this.lbl_mnu_total.Location = new System.Drawing.Point(3, 0);
            this.lbl_mnu_total.Name = "lbl_mnu_total";
            this.lbl_mnu_total.Size = new System.Drawing.Size(19, 13);
            this.lbl_mnu_total.TabIndex = 3;
            this.lbl_mnu_total.Text = "00";
            this.lbl_mnu_total.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dgvMenu
            // 
            this.dgvMenu.AllowUserToAddRows = false;
            this.dgvMenu.AllowUserToDeleteRows = false;
            this.dgvMenu.AllowUserToResizeRows = false;
            this.dgvMenu.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMenu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMenu.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMnuSrno,
            this.padname,
            this.barname});
            this.dgvMenu.Location = new System.Drawing.Point(12, 35);
            this.dgvMenu.Name = "dgvMenu";
            this.dgvMenu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvMenu.Size = new System.Drawing.Size(618, 132);
            this.dgvMenu.TabIndex = 2;
            this.dgvMenu.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvMenu_RowValidating);
            this.dgvMenu.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvMenu_EditingControlShowing);
            this.dgvMenu.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvMenu_KeyDown);
            // 
            // colMnuSrno
            // 
            this.colMnuSrno.DataPropertyName = "srno";
            this.colMnuSrno.FillWeight = 45.68528F;
            this.colMnuSrno.HeaderText = "Sr. No.";
            this.colMnuSrno.Name = "colMnuSrno";
            this.colMnuSrno.ReadOnly = true;
            // 
            // padname
            // 
            this.padname.DataPropertyName = "desc1";
            this.padname.FillWeight = 127.1573F;
            this.padname.HeaderText = "Padname";
            this.padname.Name = "padname";
            // 
            // barname
            // 
            this.barname.DataPropertyName = "desc2";
            this.barname.FillWeight = 127.1573F;
            this.barname.HeaderText = "Barname";
            this.barname.Name = "barname";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Image = global::CustModAccUI2.UI.Properties.Resources.Cancel_1;
            this.btnCancel.Location = new System.Drawing.Point(627, 579);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(26, 23);
            this.btnCancel.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnCancel, "Cancel (Ctrl+Z)");
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDone
            // 
            this.btnDone.BackColor = System.Drawing.SystemColors.Control;
            this.btnDone.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnDone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDone.Image = global::CustModAccUI2.UI.Properties.Resources.done_square;
            this.btnDone.Location = new System.Drawing.Point(595, 579);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(26, 23);
            this.btnDone.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnDone, "Done (Ctrl+O)");
            this.btnDone.UseVisualStyleBackColor = false;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // btn_rptDelete
            // 
            this.btn_rptDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btn_rptDelete.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_rptDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_rptDelete.Image = global::CustModAccUI2.UI.Properties.Resources.delete;
            this.btn_rptDelete.Location = new System.Drawing.Point(602, 9);
            this.btn_rptDelete.Name = "btn_rptDelete";
            this.btn_rptDelete.Size = new System.Drawing.Size(26, 23);
            this.btn_rptDelete.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btn_rptDelete, "Delete (Alt+D)");
            this.btn_rptDelete.UseVisualStyleBackColor = false;
            this.btn_rptDelete.Click += new System.EventHandler(this.btn_rptDelete_Click);
            // 
            // btn_rptAdd
            // 
            this.btn_rptAdd.BackColor = System.Drawing.SystemColors.Control;
            this.btn_rptAdd.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_rptAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_rptAdd.Image = global::CustModAccUI2.UI.Properties.Resources._new;
            this.btn_rptAdd.Location = new System.Drawing.Point(569, 9);
            this.btn_rptAdd.Name = "btn_rptAdd";
            this.btn_rptAdd.Size = new System.Drawing.Size(26, 23);
            this.btn_rptAdd.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btn_rptAdd, "Add (Alt+A)");
            this.btn_rptAdd.UseVisualStyleBackColor = false;
            this.btn_rptAdd.Click += new System.EventHandler(this.btn_rptAdd_Click);
            // 
            // btn_tranDelete
            // 
            this.btn_tranDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btn_tranDelete.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_tranDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_tranDelete.Image = global::CustModAccUI2.UI.Properties.Resources.delete;
            this.btn_tranDelete.Location = new System.Drawing.Point(602, 9);
            this.btn_tranDelete.Name = "btn_tranDelete";
            this.btn_tranDelete.Size = new System.Drawing.Size(26, 23);
            this.btn_tranDelete.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btn_tranDelete, "Delete (Alt+D)");
            this.btn_tranDelete.UseVisualStyleBackColor = false;
            this.btn_tranDelete.Click += new System.EventHandler(this.btn_tranDelete_Click);
            // 
            // btn_tranAdd
            // 
            this.btn_tranAdd.BackColor = System.Drawing.SystemColors.Control;
            this.btn_tranAdd.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_tranAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_tranAdd.Image = global::CustModAccUI2.UI.Properties.Resources._new;
            this.btn_tranAdd.Location = new System.Drawing.Point(569, 9);
            this.btn_tranAdd.Name = "btn_tranAdd";
            this.btn_tranAdd.Size = new System.Drawing.Size(26, 23);
            this.btn_tranAdd.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btn_tranAdd, "Add (Alt+A)");
            this.btn_tranAdd.UseVisualStyleBackColor = false;
            this.btn_tranAdd.Click += new System.EventHandler(this.btn_tranAdd_Click);
            // 
            // btn_mnuDelete
            // 
            this.btn_mnuDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btn_mnuDelete.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_mnuDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_mnuDelete.Image = global::CustModAccUI2.UI.Properties.Resources.delete;
            this.btn_mnuDelete.Location = new System.Drawing.Point(602, 9);
            this.btn_mnuDelete.Name = "btn_mnuDelete";
            this.btn_mnuDelete.Size = new System.Drawing.Size(26, 23);
            this.btn_mnuDelete.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btn_mnuDelete, "Delete (Alt+D)");
            this.btn_mnuDelete.UseVisualStyleBackColor = false;
            this.btn_mnuDelete.Click += new System.EventHandler(this.btn_mnuDelete_Click);
            // 
            // btn_mnuAdd
            // 
            this.btn_mnuAdd.BackColor = System.Drawing.SystemColors.Control;
            this.btn_mnuAdd.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_mnuAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_mnuAdd.Image = global::CustModAccUI2.UI.Properties.Resources._new;
            this.btn_mnuAdd.Location = new System.Drawing.Point(569, 9);
            this.btn_mnuAdd.Name = "btn_mnuAdd";
            this.btn_mnuAdd.Size = new System.Drawing.Size(26, 23);
            this.btn_mnuAdd.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btn_mnuAdd, "Add (Alt+A)");
            this.btn_mnuAdd.UseVisualStyleBackColor = false;
            this.btn_mnuAdd.Click += new System.EventHandler(this.btn_mnuAdd_Click);
            // 
            // frmMenuTranRept
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 617);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMenuTranRept";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menus/Transactions/Reports";
            this.Load += new System.EventHandler(this.frmMenuTranRept_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMenuTranRept_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.grpReport.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.grpTran.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTran)).EndInit();
            this.grpMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMenu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grpMenu;
        private System.Windows.Forms.DataGridView dgvMenu;
        private System.Windows.Forms.Button btn_mnuAdd;
        private System.Windows.Forms.Button btn_mnuDelete;
        private System.Windows.Forms.GroupBox grpTran;
        private System.Windows.Forms.Button btn_tranDelete;
        private System.Windows.Forms.Button btn_tranAdd;
        private System.Windows.Forms.DataGridView dgvTran;
        private System.Windows.Forms.GroupBox grpReport;
        private System.Windows.Forms.Button btn_rptDelete;
        private System.Windows.Forms.Button btn_rptAdd;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lbl_mnu_total;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRptSrno;
        private System.Windows.Forms.DataGridViewTextBoxColumn group;
        private System.Windows.Forms.DataGridViewTextBoxColumn desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn rep_nm;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbl_tran_total;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lbl_rpt_total;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranSrno;
        private System.Windows.Forms.DataGridViewTextBoxColumn entry_ty;
        private System.Windows.Forms.DataGridViewTextBoxColumn code_nm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMnuSrno;
        private System.Windows.Forms.DataGridViewTextBoxColumn padname;
        private System.Windows.Forms.DataGridViewTextBoxColumn barname;
    }
}