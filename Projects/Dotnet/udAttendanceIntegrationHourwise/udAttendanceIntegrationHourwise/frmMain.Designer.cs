namespace udAttendanceIntegrationHourwise
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLogout = new System.Windows.Forms.ToolStripButton();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.btnCalculator = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtGrade = new System.Windows.Forms.TextBox();
            this.btnGrade = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.btnCategory = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDesignation = new System.Windows.Forms.TextBox();
            this.btnDesignation = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.btnDepartment = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.btnLocation = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpfrom = new System.Windows.Forms.DateTimePicker();
            this.txtEmpName = new System.Windows.Forms.TextBox();
            this.dtpto = new System.Windows.Forms.DateTimePicker();
            this.btnEmpName = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddRec = new System.Windows.Forms.Button();
            this.lcProcessingMonth = new System.Windows.Forms.Label();
            this.btnProcess = new System.Windows.Forms.Button();
            this.txtProcMonth = new System.Windows.Forms.TextBox();
            this.btnMonth = new System.Windows.Forms.Button();
            this.lcProcessingYear = new System.Windows.Forms.Label();
            this.BtnMuster = new System.Windows.Forms.Button();
            this.txtProcYear = new System.Windows.Forms.TextBox();
            this.txtMuster = new System.Windows.Forms.TextBox();
            this.lcMusterType = new System.Windows.Forms.Label();
            this.btnProcYear = new System.Windows.Forms.Button();
            this.dgvAttendanceGrid = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnGetExcel = new System.Windows.Forms.Button();
            this.sbExcelFile = new System.Windows.Forms.Button();
            this.txtExcelFile = new System.Windows.Forms.TextBox();
            this.lcExcelFile = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendanceGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.btnEdit,
            this.btnCancel,
            this.toolStripSeparator3,
            this.btnLogout,
            this.btnHelp,
            this.btnCalculator,
            this.toolStripSeparator7,
            this.btnExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1184, 38);
            this.toolStrip1.TabIndex = 94;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = false;
            this.btnSave.Enabled = false;
            this.btnSave.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(50, 35);
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSave.ToolTipText = "Save(Ctrl+S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.AutoSize = false;
            this.btnEdit.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(50, 35);
            this.btnEdit.Text = "Edit";
            this.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEdit.ToolTipText = "Edit(Ctrl+E)";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = false;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 35);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCancel.ToolTipText = "Cancel(Ctrl+Z)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 38);
            // 
            // btnLogout
            // 
            this.btnLogout.AutoSize = false;
            this.btnLogout.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.Black;
            this.btnLogout.Image = ((System.Drawing.Image)(resources.GetObject("btnLogout.Image")));
            this.btnLogout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(50, 35);
            this.btnLogout.Text = "Exit";
            this.btnLogout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnLogout.ToolTipText = "Exit(Ctrl+F4)";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.AutoSize = false;
            this.btnHelp.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.btnHelp.ForeColor = System.Drawing.Color.Black;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(50, 35);
            this.btnHelp.Text = "Help";
            this.btnHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnHelp.ToolTipText = "Help";
            this.btnHelp.Visible = false;
            // 
            // btnCalculator
            // 
            this.btnCalculator.AutoSize = false;
            this.btnCalculator.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.btnCalculator.ForeColor = System.Drawing.Color.Black;
            this.btnCalculator.Image = ((System.Drawing.Image)(resources.GetObject("btnCalculator.Image")));
            this.btnCalculator.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCalculator.Name = "btnCalculator";
            this.btnCalculator.Size = new System.Drawing.Size(50, 35);
            this.btnCalculator.Text = "Calc";
            this.btnCalculator.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCalculator.ToolTipText = "Calculator";
            this.btnCalculator.Visible = false;
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 38);
            this.toolStripSeparator7.Visible = false;
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = false;
            this.btnExit.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(50, 35);
            this.btnExit.Text = "Logout";
            this.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExit.ToolTipText = "Exit";
            this.btnExit.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.chkSelectAll);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.dgvAttendanceGrid);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 523);
            this.panel1.TabIndex = 95;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtGrade);
            this.groupBox3.Controls.Add(this.btnGrade);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtCategory);
            this.groupBox3.Controls.Add(this.btnCategory);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtDesignation);
            this.groupBox3.Controls.Add(this.btnDesignation);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtDepartment);
            this.groupBox3.Controls.Add(this.btnDepartment);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.txtLocation);
            this.groupBox3.Controls.Add(this.btnLocation);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.dtpfrom);
            this.groupBox3.Controls.Add(this.txtEmpName);
            this.groupBox3.Controls.Add(this.dtpto);
            this.groupBox3.Controls.Add(this.btnEmpName);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(248, 62);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(787, 125);
            this.groupBox3.TabIndex = 100;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Filter";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(361, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 13);
            this.label8.TabIndex = 115;
            this.label8.Text = "Grade";
            // 
            // txtGrade
            // 
            this.txtGrade.Location = new System.Drawing.Point(462, 70);
            this.txtGrade.Name = "txtGrade";
            this.txtGrade.ReadOnly = true;
            this.txtGrade.Size = new System.Drawing.Size(193, 20);
            this.txtGrade.TabIndex = 114;
            // 
            // btnGrade
            // 
            this.btnGrade.Location = new System.Drawing.Point(661, 70);
            this.btnGrade.Name = "btnGrade";
            this.btnGrade.Size = new System.Drawing.Size(25, 20);
            this.btnGrade.TabIndex = 116;
            this.btnGrade.Click += new System.EventHandler(this.btnGrade_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 112;
            this.label7.Text = "Category";
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(107, 68);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.ReadOnly = true;
            this.txtCategory.Size = new System.Drawing.Size(193, 20);
            this.txtCategory.TabIndex = 111;
            // 
            // btnCategory
            // 
            this.btnCategory.Location = new System.Drawing.Point(306, 68);
            this.btnCategory.Name = "btnCategory";
            this.btnCategory.Size = new System.Drawing.Size(25, 20);
            this.btnCategory.TabIndex = 113;
            this.btnCategory.Click += new System.EventHandler(this.btnCategory_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 109;
            this.label6.Text = "Designation";
            // 
            // txtDesignation
            // 
            this.txtDesignation.Location = new System.Drawing.Point(107, 95);
            this.txtDesignation.Name = "txtDesignation";
            this.txtDesignation.ReadOnly = true;
            this.txtDesignation.Size = new System.Drawing.Size(193, 20);
            this.txtDesignation.TabIndex = 108;
            // 
            // btnDesignation
            // 
            this.btnDesignation.Location = new System.Drawing.Point(306, 95);
            this.btnDesignation.Name = "btnDesignation";
            this.btnDesignation.Size = new System.Drawing.Size(25, 20);
            this.btnDesignation.TabIndex = 110;
            this.btnDesignation.Click += new System.EventHandler(this.btnDesignation_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(361, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 106;
            this.label5.Text = "Department";
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(462, 45);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(193, 20);
            this.txtDepartment.TabIndex = 105;
            // 
            // btnDepartment
            // 
            this.btnDepartment.Location = new System.Drawing.Point(661, 45);
            this.btnDepartment.Name = "btnDepartment";
            this.btnDepartment.Size = new System.Drawing.Size(25, 20);
            this.btnDepartment.TabIndex = 107;
            this.btnDepartment.Click += new System.EventHandler(this.btnDepartment_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 103;
            this.label4.Text = "Location";
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(107, 45);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.ReadOnly = true;
            this.txtLocation.Size = new System.Drawing.Size(193, 20);
            this.txtLocation.TabIndex = 102;
            // 
            // btnLocation
            // 
            this.btnLocation.Location = new System.Drawing.Point(306, 45);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(25, 20);
            this.btnLocation.TabIndex = 104;
            this.btnLocation.Click += new System.EventHandler(this.btnLocation_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(206, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 17);
            this.label3.TabIndex = 101;
            this.label3.Text = "To";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(361, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 97;
            this.label1.Text = "Employee Name";
            // 
            // dtpfrom
            // 
            this.dtpfrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpfrom.Location = new System.Drawing.Point(107, 16);
            this.dtpfrom.Name = "dtpfrom";
            this.dtpfrom.Size = new System.Drawing.Size(96, 20);
            this.dtpfrom.TabIndex = 100;
            this.dtpfrom.ValueChanged += new System.EventHandler(this.dtpfrom_ValueChanged);
            // 
            // txtEmpName
            // 
            this.txtEmpName.Location = new System.Drawing.Point(462, 95);
            this.txtEmpName.Name = "txtEmpName";
            this.txtEmpName.ReadOnly = true;
            this.txtEmpName.Size = new System.Drawing.Size(193, 20);
            this.txtEmpName.TabIndex = 93;
            // 
            // dtpto
            // 
            this.dtpto.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpto.Location = new System.Drawing.Point(232, 16);
            this.dtpto.Name = "dtpto";
            this.dtpto.Size = new System.Drawing.Size(96, 20);
            this.dtpto.TabIndex = 99;
            this.dtpto.ValueChanged += new System.EventHandler(this.dtpto_ValueChanged);
            // 
            // btnEmpName
            // 
            this.btnEmpName.Location = new System.Drawing.Point(661, 95);
            this.btnEmpName.Name = "btnEmpName";
            this.btnEmpName.Size = new System.Drawing.Size(25, 20);
            this.btnEmpName.TabIndex = 97;
            this.btnEmpName.Click += new System.EventHandler(this.btnEmpName_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 17);
            this.label2.TabIndex = 98;
            this.label2.Text = "Date";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(7, 161);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAll.TabIndex = 92;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAddRec);
            this.groupBox2.Controls.Add(this.lcProcessingMonth);
            this.groupBox2.Controls.Add(this.btnProcess);
            this.groupBox2.Controls.Add(this.txtProcMonth);
            this.groupBox2.Controls.Add(this.btnMonth);
            this.groupBox2.Controls.Add(this.lcProcessingYear);
            this.groupBox2.Controls.Add(this.BtnMuster);
            this.groupBox2.Controls.Add(this.txtProcYear);
            this.groupBox2.Controls.Add(this.txtMuster);
            this.groupBox2.Controls.Add(this.lcMusterType);
            this.groupBox2.Controls.Add(this.btnProcYear);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(239, 152);
            this.groupBox2.TabIndex = 99;
            this.groupBox2.TabStop = false;
            // 
            // btnAddRec
            // 
            this.btnAddRec.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddRec.Location = new System.Drawing.Point(9, 113);
            this.btnAddRec.Name = "btnAddRec";
            this.btnAddRec.Size = new System.Drawing.Size(90, 24);
            this.btnAddRec.TabIndex = 94;
            this.btnAddRec.Text = "Add Record";
            this.btnAddRec.Visible = false;
            this.btnAddRec.Click += new System.EventHandler(this.btnAddRec_Click);
            // 
            // lcProcessingMonth
            // 
            this.lcProcessingMonth.Location = new System.Drawing.Point(7, 47);
            this.lcProcessingMonth.Name = "lcProcessingMonth";
            this.lcProcessingMonth.Size = new System.Drawing.Size(95, 13);
            this.lcProcessingMonth.TabIndex = 86;
            this.lcProcessingMonth.Text = "Processing Month";
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(107, 113);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(113, 24);
            this.btnProcess.TabIndex = 98;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // txtProcMonth
            // 
            this.txtProcMonth.Location = new System.Drawing.Point(108, 44);
            this.txtProcMonth.Name = "txtProcMonth";
            this.txtProcMonth.ReadOnly = true;
            this.txtProcMonth.Size = new System.Drawing.Size(100, 20);
            this.txtProcMonth.TabIndex = 87;
            // 
            // btnMonth
            // 
            this.btnMonth.Location = new System.Drawing.Point(210, 44);
            this.btnMonth.Name = "btnMonth";
            this.btnMonth.Size = new System.Drawing.Size(25, 20);
            this.btnMonth.TabIndex = 92;
            this.btnMonth.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // lcProcessingYear
            // 
            this.lcProcessingYear.Location = new System.Drawing.Point(6, 17);
            this.lcProcessingYear.Name = "lcProcessingYear";
            this.lcProcessingYear.Size = new System.Drawing.Size(93, 17);
            this.lcProcessingYear.TabIndex = 90;
            this.lcProcessingYear.Text = "Processing Year";
            // 
            // BtnMuster
            // 
            this.BtnMuster.Location = new System.Drawing.Point(209, 73);
            this.BtnMuster.Name = "BtnMuster";
            this.BtnMuster.Size = new System.Drawing.Size(25, 20);
            this.BtnMuster.TabIndex = 94;
            this.BtnMuster.Visible = false;
            // 
            // txtProcYear
            // 
            this.txtProcYear.Location = new System.Drawing.Point(107, 14);
            this.txtProcYear.Name = "txtProcYear";
            this.txtProcYear.ReadOnly = true;
            this.txtProcYear.Size = new System.Drawing.Size(100, 20);
            this.txtProcYear.TabIndex = 88;
            // 
            // txtMuster
            // 
            this.txtMuster.Location = new System.Drawing.Point(107, 73);
            this.txtMuster.Name = "txtMuster";
            this.txtMuster.ReadOnly = true;
            this.txtMuster.Size = new System.Drawing.Size(100, 20);
            this.txtMuster.TabIndex = 89;
            // 
            // lcMusterType
            // 
            this.lcMusterType.Location = new System.Drawing.Point(7, 73);
            this.lcMusterType.Name = "lcMusterType";
            this.lcMusterType.Size = new System.Drawing.Size(60, 13);
            this.lcMusterType.TabIndex = 91;
            this.lcMusterType.Text = "Muster Type";
            // 
            // btnProcYear
            // 
            this.btnProcYear.Location = new System.Drawing.Point(209, 13);
            this.btnProcYear.Name = "btnProcYear";
            this.btnProcYear.Size = new System.Drawing.Size(25, 20);
            this.btnProcYear.TabIndex = 93;
            this.btnProcYear.Click += new System.EventHandler(this.btnProcYear_Click);
            // 
            // dgvAttendanceGrid
            // 
            this.dgvAttendanceGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttendanceGrid.Location = new System.Drawing.Point(4, 197);
            this.dgvAttendanceGrid.Name = "dgvAttendanceGrid";
            this.dgvAttendanceGrid.RowHeadersVisible = false;
            this.dgvAttendanceGrid.Size = new System.Drawing.Size(1019, 322);
            this.dgvAttendanceGrid.TabIndex = 97;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDownload);
            this.groupBox1.Controls.Add(this.btnGetExcel);
            this.groupBox1.Controls.Add(this.sbExcelFile);
            this.groupBox1.Controls.Add(this.txtExcelFile);
            this.groupBox1.Controls.Add(this.lcExcelFile);
            this.groupBox1.Location = new System.Drawing.Point(248, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(787, 49);
            this.groupBox1.TabIndex = 95;
            this.groupBox1.TabStop = false;
            // 
            // btnDownload
            // 
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownload.Location = new System.Drawing.Point(527, 17);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(152, 21);
            this.btnDownload.TabIndex = 93;
            this.btnDownload.Text = "Download Default Format";
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnGetExcel
            // 
            this.btnGetExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGetExcel.Location = new System.Drawing.Point(462, 17);
            this.btnGetExcel.Name = "btnGetExcel";
            this.btnGetExcel.Size = new System.Drawing.Size(59, 20);
            this.btnGetExcel.TabIndex = 91;
            this.btnGetExcel.Text = "Upload";
            this.btnGetExcel.Click += new System.EventHandler(this.btnGetExcel_Click);
            // 
            // sbExcelFile
            // 
            this.sbExcelFile.Location = new System.Drawing.Point(431, 17);
            this.sbExcelFile.Name = "sbExcelFile";
            this.sbExcelFile.Size = new System.Drawing.Size(25, 20);
            this.sbExcelFile.TabIndex = 89;
            this.sbExcelFile.Click += new System.EventHandler(this.sbExcelFile_Click);
            // 
            // txtExcelFile
            // 
            this.txtExcelFile.Location = new System.Drawing.Point(107, 17);
            this.txtExcelFile.Name = "txtExcelFile";
            this.txtExcelFile.ReadOnly = true;
            this.txtExcelFile.Size = new System.Drawing.Size(319, 20);
            this.txtExcelFile.TabIndex = 88;
            // 
            // lcExcelFile
            // 
            this.lcExcelFile.AutoSize = true;
            this.lcExcelFile.Location = new System.Drawing.Point(6, 17);
            this.lcExcelFile.Name = "lcExcelFile";
            this.lcExcelFile.Size = new System.Drawing.Size(85, 13);
            this.lcExcelFile.TabIndex = 90;
            this.lcExcelFile.Text = "Select Excel File";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.editToolStripMenuItem,
            this.saveToolStripMenuItem1,
            this.cancelToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 38);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1184, 24);
            this.menuStrip1.TabIndex = 96;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.newToolStripMenuItem.Text = "New";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.cancelToolStripMenuItem.Text = "Cancel";
            this.cancelToolStripMenuItem.Click += new System.EventHandler(this.cancelToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 561);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendanceGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnCancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnLogout;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.DataGridView dgvAttendanceGrid;
        private System.Windows.Forms.Button BtnMuster;
        private System.Windows.Forms.TextBox txtMuster;
        private System.Windows.Forms.Label lcMusterType;
        private System.Windows.Forms.Button btnProcYear;
        private System.Windows.Forms.TextBox txtProcYear;
        private System.Windows.Forms.Label lcProcessingYear;
        private System.Windows.Forms.Button btnMonth;
        private System.Windows.Forms.TextBox txtProcMonth;
        private System.Windows.Forms.Label lcProcessingMonth;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.Button btnGetExcel;
        private System.Windows.Forms.Button sbExcelFile;
        private System.Windows.Forms.TextBox txtExcelFile;
        private System.Windows.Forms.Label lcExcelFile;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAddRec;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtGrade;
        private System.Windows.Forms.Button btnGrade;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Button btnCategory;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDesignation;
        private System.Windows.Forms.Button btnDesignation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.Button btnDepartment;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Button btnLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpfrom;
        private System.Windows.Forms.TextBox txtEmpName;
        private System.Windows.Forms.DateTimePicker dtpto;
        private System.Windows.Forms.Button btnEmpName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripButton btnHelp;
        private System.Windows.Forms.ToolStripButton btnCalculator;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    }
}

