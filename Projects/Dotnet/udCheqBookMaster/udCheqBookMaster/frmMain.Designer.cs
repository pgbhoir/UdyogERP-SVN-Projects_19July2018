namespace udCheqBookMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.gbPeriod = new System.Windows.Forms.GroupBox();
            this.txtEndNo = new uNumericTextBox.cNumericTextBox();
            this.txtStartNo = new uNumericTextBox.cNumericTextBox();
            this.txtLeaftcnt = new uNumericTextBox.cNumericTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnTrnSeries = new System.Windows.Forms.Button();
            this.txtTrnSeries = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDtDeAct = new System.Windows.Forms.Label();
            this.dtpDeAct = new System.Windows.Forms.DateTimePicker();
            this.LblEndNo = new System.Windows.Forms.Label();
            this.LblStartNo = new System.Windows.Forms.Label();
            this.lblLeaftcnt = new System.Windows.Forms.Label();
            this.chkClosedBook = new System.Windows.Forms.CheckBox();
            this.btnLocNm = new System.Windows.Forms.Button();
            this.txtLocNm = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBankName = new System.Windows.Forms.TextBox();
            this.lblBankName = new System.Windows.Forms.Label();
            this.lblDtAct = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpActDt = new System.Windows.Forms.DateTimePicker();
            this.btnBankName = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnFirst = new System.Windows.Forms.ToolStripButton();
            this.btnBack = new System.Windows.Forms.ToolStripButton();
            this.btnForward = new System.Windows.Forms.ToolStripButton();
            this.btnLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEmail = new System.Windows.Forms.ToolStripButton();
            this.btnLocate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPreview = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExportPdf = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLogout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.btnCalculator = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvChequeDet = new System.Windows.Forms.DataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.gbPeriod.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChequeDet)).BeginInit();
            this.SuspendLayout();
            // 
            // gbPeriod
            // 
            this.gbPeriod.Controls.Add(this.txtEndNo);
            this.gbPeriod.Controls.Add(this.txtStartNo);
            this.gbPeriod.Controls.Add(this.txtLeaftcnt);
            this.gbPeriod.Controls.Add(this.button1);
            this.gbPeriod.Controls.Add(this.btnTrnSeries);
            this.gbPeriod.Controls.Add(this.txtTrnSeries);
            this.gbPeriod.Controls.Add(this.label2);
            this.gbPeriod.Controls.Add(this.btnRefresh);
            this.gbPeriod.Controls.Add(this.label1);
            this.gbPeriod.Controls.Add(this.lblDtDeAct);
            this.gbPeriod.Controls.Add(this.dtpDeAct);
            this.gbPeriod.Controls.Add(this.LblEndNo);
            this.gbPeriod.Controls.Add(this.LblStartNo);
            this.gbPeriod.Controls.Add(this.lblLeaftcnt);
            this.gbPeriod.Controls.Add(this.chkClosedBook);
            this.gbPeriod.Controls.Add(this.btnLocNm);
            this.gbPeriod.Controls.Add(this.txtLocNm);
            this.gbPeriod.Controls.Add(this.label10);
            this.gbPeriod.Controls.Add(this.txtBankName);
            this.gbPeriod.Controls.Add(this.lblBankName);
            this.gbPeriod.Controls.Add(this.lblDtAct);
            this.gbPeriod.Controls.Add(this.label3);
            this.gbPeriod.Controls.Add(this.dtpActDt);
            this.gbPeriod.Controls.Add(this.btnBankName);
            this.gbPeriod.Location = new System.Drawing.Point(7, 31);
            this.gbPeriod.Name = "gbPeriod";
            this.gbPeriod.Size = new System.Drawing.Size(880, 123);
            this.gbPeriod.TabIndex = 21;
            this.gbPeriod.TabStop = false;
            // 
            // txtEndNo
            // 
            this.txtEndNo.Location = new System.Drawing.Point(350, 67);
            this.txtEndNo.Name = "txtEndNo";
            this.txtEndNo.ReadOnly = true;
            this.txtEndNo.Size = new System.Drawing.Size(78, 20);
            this.txtEndNo.TabIndex = 12;
            this.txtEndNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtStartNo
            // 
            this.txtStartNo.Location = new System.Drawing.Point(67, 67);
            this.txtStartNo.MaxLength = 6;
            this.txtStartNo.Name = "txtStartNo";
            this.txtStartNo.Size = new System.Drawing.Size(78, 20);
            this.txtStartNo.TabIndex = 8;
            this.txtStartNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtStartNo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtStartNo_KeyUp);
            // 
            // txtLeaftcnt
            // 
            this.txtLeaftcnt.Location = new System.Drawing.Point(223, 67);
            this.txtLeaftcnt.MaxLength = 3;
            this.txtLeaftcnt.Name = "txtLeaftcnt";
            this.txtLeaftcnt.Size = new System.Drawing.Size(78, 20);
            this.txtLeaftcnt.TabIndex = 10;
            this.txtLeaftcnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLeaftcnt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtLeaftcnt_KeyUp);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(576, 92);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 25);
            this.button1.TabIndex = 21;
            this.button1.Text = "Cheque Details";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnTrnSeries
            // 
            this.btnTrnSeries.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnTrnSeries.Image = global::udCheqBookMaster.Properties.Resources.loc_on;
            this.btnTrnSeries.Location = new System.Drawing.Point(680, 66);
            this.btnTrnSeries.Name = "btnTrnSeries";
            this.btnTrnSeries.Size = new System.Drawing.Size(24, 20);
            this.btnTrnSeries.TabIndex = 19;
            this.btnTrnSeries.UseVisualStyleBackColor = true;
            this.btnTrnSeries.Click += new System.EventHandler(this.btnTrnSeries_Click);
            // 
            // txtTrnSeries
            // 
            this.txtTrnSeries.Location = new System.Drawing.Point(576, 66);
            this.txtTrnSeries.Name = "txtTrnSeries";
            this.txtTrnSeries.ReadOnly = true;
            this.txtTrnSeries.Size = new System.Drawing.Size(102, 20);
            this.txtTrnSeries.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(460, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Transaction Series";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(737, 38);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(136, 33);
            this.btnRefresh.TabIndex = 27;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(711, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(3, 118);
            this.label1.TabIndex = 113;
            // 
            // lblDtDeAct
            // 
            this.lblDtDeAct.AutoSize = true;
            this.lblDtDeAct.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDtDeAct.Location = new System.Drawing.Point(460, 41);
            this.lblDtDeAct.Name = "lblDtDeAct";
            this.lblDtDeAct.Size = new System.Drawing.Size(113, 13);
            this.lblDtDeAct.TabIndex = 15;
            this.lblDtDeAct.Text = "Book Deactivate From";
            // 
            // dtpDeAct
            // 
            this.dtpDeAct.CustomFormat = "dd/MM/yyyy";
            this.dtpDeAct.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDeAct.Location = new System.Drawing.Point(576, 37);
            this.dtpDeAct.Name = "dtpDeAct";
            this.dtpDeAct.Size = new System.Drawing.Size(101, 20);
            this.dtpDeAct.TabIndex = 16;
            // 
            // LblEndNo
            // 
            this.LblEndNo.AutoSize = true;
            this.LblEndNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblEndNo.Location = new System.Drawing.Point(303, 70);
            this.LblEndNo.Name = "LblEndNo";
            this.LblEndNo.Size = new System.Drawing.Size(46, 13);
            this.LblEndNo.TabIndex = 11;
            this.LblEndNo.Text = "End No.";
            // 
            // LblStartNo
            // 
            this.LblStartNo.AutoSize = true;
            this.LblStartNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblStartNo.Location = new System.Drawing.Point(15, 70);
            this.LblStartNo.Name = "LblStartNo";
            this.LblStartNo.Size = new System.Drawing.Size(49, 13);
            this.LblStartNo.TabIndex = 7;
            this.LblStartNo.Text = "Start No.";
            // 
            // lblLeaftcnt
            // 
            this.lblLeaftcnt.AutoSize = true;
            this.lblLeaftcnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLeaftcnt.Location = new System.Drawing.Point(147, 70);
            this.lblLeaftcnt.Name = "lblLeaftcnt";
            this.lblLeaftcnt.Size = new System.Drawing.Size(70, 13);
            this.lblLeaftcnt.TabIndex = 9;
            this.lblLeaftcnt.Text = "Leaflet Count";
            // 
            // chkClosedBook
            // 
            this.chkClosedBook.AutoSize = true;
            this.chkClosedBook.Location = new System.Drawing.Point(720, 12);
            this.chkClosedBook.Name = "chkClosedBook";
            this.chkClosedBook.Size = new System.Drawing.Size(154, 17);
            this.chkClosedBook.TabIndex = 26;
            this.chkClosedBook.Text = "Do not show Closed Books";
            this.chkClosedBook.UseVisualStyleBackColor = true;
            this.chkClosedBook.CheckedChanged += new System.EventHandler(this.chkClosedBook_CheckedChanged);
            // 
            // btnLocNm
            // 
            this.btnLocNm.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnLocNm.Image = global::udCheqBookMaster.Properties.Resources.loc_on;
            this.btnLocNm.Location = new System.Drawing.Point(423, 11);
            this.btnLocNm.Name = "btnLocNm";
            this.btnLocNm.Size = new System.Drawing.Size(24, 20);
            this.btnLocNm.TabIndex = 3;
            this.btnLocNm.UseVisualStyleBackColor = true;
            this.btnLocNm.Click += new System.EventHandler(this.btnLocNm_Click);
            // 
            // txtLocNm
            // 
            this.txtLocNm.Location = new System.Drawing.Point(101, 11);
            this.txtLocNm.Name = "txtLocNm";
            this.txtLocNm.ReadOnly = true;
            this.txtLocNm.Size = new System.Drawing.Size(319, 20);
            this.txtLocNm.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 11);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Location";
            // 
            // txtBankName
            // 
            this.txtBankName.Location = new System.Drawing.Point(101, 37);
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.ReadOnly = true;
            this.txtBankName.Size = new System.Drawing.Size(319, 20);
            this.txtBankName.TabIndex = 5;
            // 
            // lblBankName
            // 
            this.lblBankName.AutoSize = true;
            this.lblBankName.Location = new System.Drawing.Point(15, 37);
            this.lblBankName.Name = "lblBankName";
            this.lblBankName.Size = new System.Drawing.Size(63, 13);
            this.lblBankName.TabIndex = 4;
            this.lblBankName.Text = "Bank Name";
            // 
            // lblDtAct
            // 
            this.lblDtAct.AutoSize = true;
            this.lblDtAct.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDtAct.Location = new System.Drawing.Point(460, 13);
            this.lblDtAct.Name = "lblDtAct";
            this.lblDtAct.Size = new System.Drawing.Size(100, 13);
            this.lblDtAct.TabIndex = 13;
            this.lblDtAct.Text = "Book Activate From";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(74, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "*";
            // 
            // dtpActDt
            // 
            this.dtpActDt.CustomFormat = "dd/MM/yyyy";
            this.dtpActDt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpActDt.Location = new System.Drawing.Point(576, 11);
            this.dtpActDt.Name = "dtpActDt";
            this.dtpActDt.Size = new System.Drawing.Size(100, 20);
            this.dtpActDt.TabIndex = 14;
            this.dtpActDt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpActDt_KeyUp);
            this.dtpActDt.Leave += new System.EventHandler(this.dtpActDt_Leave);
            // 
            // btnBankName
            // 
            this.btnBankName.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnBankName.Image = global::udCheqBookMaster.Properties.Resources.loc_on;
            this.btnBankName.Location = new System.Drawing.Point(423, 37);
            this.btnBankName.Name = "btnBankName";
            this.btnBankName.Size = new System.Drawing.Size(24, 20);
            this.btnBankName.TabIndex = 6;
            this.btnBankName.UseVisualStyleBackColor = true;
            this.btnBankName.Click += new System.EventHandler(this.btnBankName_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFirst,
            this.btnBack,
            this.btnForward,
            this.btnLast,
            this.toolStripSeparator1,
            this.btnEmail,
            this.btnLocate,
            this.toolStripSeparator2,
            this.btnNew,
            this.btnSave,
            this.btnEdit,
            this.btnCancel,
            this.btnDelete,
            this.toolStripSeparator3,
            this.btnPreview,
            this.btnPrint,
            this.btnExportPdf,
            this.toolStripSeparator4,
            this.btnLogout,
            this.toolStripSeparator6,
            this.btnHelp,
            this.btnCalculator,
            this.toolStripSeparator7,
            this.btnExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(900, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnFirst
            // 
            this.btnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFirst.Image = ((System.Drawing.Image)(resources.GetObject("btnFirst.Image")));
            this.btnFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(23, 22);
            this.btnFirst.Text = "toolStripButton1";
            this.btnFirst.ToolTipText = "First";
            this.btnFirst.Visible = false;
            // 
            // btnBack
            // 
            this.btnBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnBack.Image = ((System.Drawing.Image)(resources.GetObject("btnBack.Image")));
            this.btnBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(23, 22);
            this.btnBack.Text = "toolStripButton2";
            this.btnBack.ToolTipText = "Back";
            this.btnBack.Visible = false;
            // 
            // btnForward
            // 
            this.btnForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnForward.Image = ((System.Drawing.Image)(resources.GetObject("btnForward.Image")));
            this.btnForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(23, 22);
            this.btnForward.Text = "toolStripButton3";
            this.btnForward.ToolTipText = "Forward";
            this.btnForward.Visible = false;
            // 
            // btnLast
            // 
            this.btnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLast.Image = ((System.Drawing.Image)(resources.GetObject("btnLast.Image")));
            this.btnLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(23, 22);
            this.btnLast.Text = "toolStripButton4";
            this.btnLast.ToolTipText = "Last";
            this.btnLast.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator1.Visible = false;
            // 
            // btnEmail
            // 
            this.btnEmail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEmail.Image = ((System.Drawing.Image)(resources.GetObject("btnEmail.Image")));
            this.btnEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(23, 22);
            this.btnEmail.Text = "toolStripButton5";
            this.btnEmail.ToolTipText = "Email(Ctrl+F)";
            this.btnEmail.Visible = false;
            // 
            // btnLocate
            // 
            this.btnLocate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLocate.Image = ((System.Drawing.Image)(resources.GetObject("btnLocate.Image")));
            this.btnLocate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLocate.Name = "btnLocate";
            this.btnLocate.Size = new System.Drawing.Size(23, 22);
            this.btnLocate.Text = "toolStripButton6";
            this.btnLocate.ToolTipText = "Locate";
            this.btnLocate.Visible = false;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Visible = false;
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(23, 22);
            this.btnNew.Text = "toolStripButton7";
            this.btnNew.ToolTipText = "New(Ctrl+N)";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Enabled = false;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.Text = "toolStripButton1";
            this.btnSave.ToolTipText = "Save(Ctrl+S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(23, 22);
            this.btnEdit.Text = "toolStripButton8";
            this.btnEdit.ToolTipText = "Edit(Ctrl+E)";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(23, 22);
            this.btnCancel.Text = "toolStripButton9";
            this.btnCancel.ToolTipText = "Cancel(Ctrl+Z)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.Text = "toolStripButton10";
            this.btnDelete.ToolTipText = "Delete(Ctrl+D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPreview
            // 
            this.btnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPreview.Enabled = false;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(23, 22);
            this.btnPreview.Text = "toolStripButton11";
            this.btnPreview.ToolTipText = "Preview(Ctrl+O)";
            // 
            // btnPrint
            // 
            this.btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrint.Enabled = false;
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(23, 22);
            this.btnPrint.Text = "Print(Ctrl+P)";
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExportPdf.Enabled = false;
            this.btnExportPdf.Image = ((System.Drawing.Image)(resources.GetObject("btnExportPdf.Image")));
            this.btnExportPdf.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(23, 22);
            this.btnExportPdf.Text = "PDF";
            this.btnExportPdf.ToolTipText = "Export To PDF(Ctrl+R)";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnLogout
            // 
            this.btnLogout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLogout.Image = ((System.Drawing.Image)(resources.GetObject("btnLogout.Image")));
            this.btnLogout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(23, 22);
            this.btnLogout.Text = "toolStripButton14";
            this.btnLogout.ToolTipText = "Exit(Ctrl+F4)";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator6.Visible = false;
            // 
            // btnHelp
            // 
            this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(23, 22);
            this.btnHelp.Text = "toolStripButton16";
            this.btnHelp.ToolTipText = "Help";
            this.btnHelp.Visible = false;
            // 
            // btnCalculator
            // 
            this.btnCalculator.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCalculator.Image = ((System.Drawing.Image)(resources.GetObject("btnCalculator.Image")));
            this.btnCalculator.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCalculator.Name = "btnCalculator";
            this.btnCalculator.Size = new System.Drawing.Size(23, 22);
            this.btnCalculator.Text = "toolStripButton17";
            this.btnCalculator.ToolTipText = "Calculator";
            this.btnCalculator.Visible = false;
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExit
            // 
            this.btnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(23, 22);
            this.btnExit.Text = "toolStripButton15";
            this.btnExit.ToolTipText = "Exit";
            this.btnExit.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvChequeDet);
            this.groupBox1.Location = new System.Drawing.Point(8, 192);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(879, 236);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            // 
            // dgvChequeDet
            // 
            this.dgvChequeDet.AllowUserToAddRows = false;
            this.dgvChequeDet.AllowUserToDeleteRows = false;
            this.dgvChequeDet.AllowUserToResizeRows = false;
            this.dgvChequeDet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvChequeDet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChequeDet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect});
            this.dgvChequeDet.Location = new System.Drawing.Point(11, 15);
            this.dgvChequeDet.Name = "dgvChequeDet";
            this.dgvChequeDet.RowHeadersVisible = false;
            this.dgvChequeDet.Size = new System.Drawing.Size(858, 210);
            this.dgvChequeDet.TabIndex = 20;
            this.dgvChequeDet.DoubleClick += new System.EventHandler(this.dgvChequeDet_DoubleClick);
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "";
            this.colSelect.Name = "colSelect";
            this.colSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(596, 182);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(150, 14);
            this.progressBar1.TabIndex = 23;
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(413, 169);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(41, 30);
            this.btn.TabIndex = 127;
            this.btn.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(762, 173);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 16);
            this.label4.TabIndex = 24;
            this.label4.Text = "Export To :";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(599, 163);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(47, 16);
            this.lblProgress.TabIndex = 22;
            this.lblProgress.Text = "label5";
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Image = global::udCheqBookMaster.Properties.Resources.icon1;
            this.btnExportExcel.Location = new System.Drawing.Point(842, 165);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(41, 30);
            this.btnExportExcel.TabIndex = 25;
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 442);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnExportExcel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.gbPeriod);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.gbPeriod.ResumeLayout(false);
            this.gbPeriod.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChequeDet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton btnFirst;
        private System.Windows.Forms.ToolStripButton btnBack;
        private System.Windows.Forms.GroupBox gbPeriod;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnTrnSeries;
        private System.Windows.Forms.TextBox txtTrnSeries;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDtDeAct;
        private System.Windows.Forms.DateTimePicker dtpDeAct;
        private System.Windows.Forms.Label LblEndNo;
        private System.Windows.Forms.Label LblStartNo;
        private System.Windows.Forms.Label lblLeaftcnt;
        private System.Windows.Forms.CheckBox chkClosedBook;
        private System.Windows.Forms.Button btnLocNm;
        private System.Windows.Forms.TextBox txtLocNm;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtBankName;
        private System.Windows.Forms.Label lblBankName;
        private System.Windows.Forms.Label lblDtAct;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpActDt;
        private System.Windows.Forms.Button btnBankName;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnForward;
        private System.Windows.Forms.ToolStripButton btnLast;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnEmail;
        private System.Windows.Forms.ToolStripButton btnLocate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnCancel;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnPreview;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.ToolStripButton btnExportPdf;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnLogout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton btnHelp;
        private System.Windows.Forms.ToolStripButton btnCalculator;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvChequeDet;
        private uNumericTextBox.cNumericTextBox txtEndNo;
        private uNumericTextBox.cNumericTextBox txtStartNo;
        private uNumericTextBox.cNumericTextBox txtLeaftcnt;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
    }
}

