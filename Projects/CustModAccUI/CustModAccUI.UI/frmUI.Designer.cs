namespace CustModAccUI.UI
{
    partial class frmUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUI));
            this.tlsMain = new System.Windows.Forms.ToolStrip();
            this.tlsbtnSave = new System.Windows.Forms.ToolStripButton();
            this.tlsbtnCancel = new System.Windows.Forms.ToolStripButton();
            this.tlsSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.tlsbtnSearch = new System.Windows.Forms.ToolStripButton();
            this.tlsbtnExit = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbl_macid = new System.Windows.Forms.Label();
            this.txt_macid = new System.Windows.Forms.TextBox();
            this.lbl_prodver = new System.Windows.Forms.Label();
            this.txt_prodver = new System.Windows.Forms.TextBox();
            this.lbl_date = new System.Windows.Forms.Label();
            this.txt_date = new System.Windows.Forms.TextBox();
            this.lbl_regcomp = new System.Windows.Forms.Label();
            this.txt_regcomp = new System.Windows.Forms.TextBox();
            this.btn_uploadxmlpath = new System.Windows.Forms.Button();
            this.lbl_uploadxml = new System.Windows.Forms.Label();
            this.txt_xmlpath = new System.Windows.Forms.TextBox();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.lbl_id = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_uploadscript = new System.Windows.Forms.Button();
            this.ColSrno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCompName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbl_remarks = new System.Windows.Forms.Label();
            this.lbl_apprby = new System.Windows.Forms.Label();
            this.txt_apprby = new System.Windows.Forms.TextBox();
            this.lbl_bugid = new System.Windows.Forms.Label();
            this.txt_bugid = new System.Windows.Forms.TextBox();
            this.lbl_poamt = new System.Windows.Forms.Label();
            this.txt_poamt = new System.Windows.Forms.TextBox();
            this.lbl_podate = new System.Windows.Forms.Label();
            this.txt_podate = new System.Windows.Forms.TextBox();
            this.txt_pono = new System.Windows.Forms.TextBox();
            this.lbl_pono = new System.Windows.Forms.Label();
            this.txt_remarks = new System.Windows.Forms.TextBox();
            this.tlsMain.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlsMain
            // 
            this.tlsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlsbtnSave,
            this.tlsbtnCancel,
            this.tlsSep1,
            this.tlsbtnSearch,
            this.tlsbtnExit});
            this.tlsMain.Location = new System.Drawing.Point(0, 0);
            this.tlsMain.Name = "tlsMain";
            this.tlsMain.Size = new System.Drawing.Size(809, 25);
            this.tlsMain.TabIndex = 0;
            this.tlsMain.Text = "toolStrip1";
            // 
            // tlsbtnSave
            // 
            this.tlsbtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlsbtnSave.Image = ((System.Drawing.Image)(resources.GetObject("tlsbtnSave.Image")));
            this.tlsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbtnSave.Name = "tlsbtnSave";
            this.tlsbtnSave.Size = new System.Drawing.Size(23, 22);
            // 
            // tlsbtnCancel
            // 
            this.tlsbtnCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlsbtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("tlsbtnCancel.Image")));
            this.tlsbtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbtnCancel.Name = "tlsbtnCancel";
            this.tlsbtnCancel.Size = new System.Drawing.Size(23, 22);
            // 
            // tlsSep1
            // 
            this.tlsSep1.Name = "tlsSep1";
            this.tlsSep1.Size = new System.Drawing.Size(6, 25);
            // 
            // tlsbtnSearch
            // 
            this.tlsbtnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlsbtnSearch.Image = ((System.Drawing.Image)(resources.GetObject("tlsbtnSearch.Image")));
            this.tlsbtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbtnSearch.Name = "tlsbtnSearch";
            this.tlsbtnSearch.Size = new System.Drawing.Size(23, 22);
            // 
            // tlsbtnExit
            // 
            this.tlsbtnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlsbtnExit.Image = ((System.Drawing.Image)(resources.GetObject("tlsbtnExit.Image")));
            this.tlsbtnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbtnExit.Name = "tlsbtnExit";
            this.tlsbtnExit.Size = new System.Drawing.Size(23, 22);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(785, 617);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl_macid);
            this.groupBox2.Controls.Add(this.txt_macid);
            this.groupBox2.Controls.Add(this.lbl_prodver);
            this.groupBox2.Controls.Add(this.txt_prodver);
            this.groupBox2.Controls.Add(this.lbl_date);
            this.groupBox2.Controls.Add(this.txt_date);
            this.groupBox2.Controls.Add(this.lbl_regcomp);
            this.groupBox2.Controls.Add(this.txt_regcomp);
            this.groupBox2.Controls.Add(this.btn_uploadxmlpath);
            this.groupBox2.Controls.Add(this.lbl_uploadxml);
            this.groupBox2.Controls.Add(this.txt_xmlpath);
            this.groupBox2.Controls.Add(this.txt_id);
            this.groupBox2.Controls.Add(this.lbl_id);
            this.groupBox2.Location = new System.Drawing.Point(10, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(764, 162);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            // 
            // lbl_macid
            // 
            this.lbl_macid.AutoSize = true;
            this.lbl_macid.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_macid.Location = new System.Drawing.Point(16, 139);
            this.lbl_macid.Name = "lbl_macid";
            this.lbl_macid.Size = new System.Drawing.Size(51, 13);
            this.lbl_macid.TabIndex = 49;
            this.lbl_macid.Text = "&MAC ID";
            // 
            // txt_macid
            // 
            this.txt_macid.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_macid.Location = new System.Drawing.Point(187, 134);
            this.txt_macid.Name = "txt_macid";
            this.txt_macid.Size = new System.Drawing.Size(393, 20);
            this.txt_macid.TabIndex = 48;
            // 
            // lbl_prodver
            // 
            this.lbl_prodver.AutoSize = true;
            this.lbl_prodver.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_prodver.Location = new System.Drawing.Point(16, 115);
            this.lbl_prodver.Name = "lbl_prodver";
            this.lbl_prodver.Size = new System.Drawing.Size(97, 13);
            this.lbl_prodver.TabIndex = 47;
            this.lbl_prodver.Text = "Product &Version";
            // 
            // txt_prodver
            // 
            this.txt_prodver.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_prodver.Location = new System.Drawing.Point(187, 111);
            this.txt_prodver.Name = "txt_prodver";
            this.txt_prodver.Size = new System.Drawing.Size(393, 20);
            this.txt_prodver.TabIndex = 46;
            // 
            // lbl_date
            // 
            this.lbl_date.AutoSize = true;
            this.lbl_date.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_date.Location = new System.Drawing.Point(16, 93);
            this.lbl_date.Name = "lbl_date";
            this.lbl_date.Size = new System.Drawing.Size(34, 13);
            this.lbl_date.TabIndex = 45;
            this.lbl_date.Text = "&Date";
            // 
            // txt_date
            // 
            this.txt_date.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_date.Location = new System.Drawing.Point(187, 88);
            this.txt_date.Name = "txt_date";
            this.txt_date.Size = new System.Drawing.Size(87, 20);
            this.txt_date.TabIndex = 44;
            // 
            // lbl_regcomp
            // 
            this.lbl_regcomp.AutoSize = true;
            this.lbl_regcomp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_regcomp.Location = new System.Drawing.Point(16, 69);
            this.lbl_regcomp.Name = "lbl_regcomp";
            this.lbl_regcomp.Size = new System.Drawing.Size(164, 13);
            this.lbl_regcomp.TabIndex = 43;
            this.lbl_regcomp.Text = "R&egistered Company Name";
            // 
            // txt_regcomp
            // 
            this.txt_regcomp.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_regcomp.Location = new System.Drawing.Point(187, 65);
            this.txt_regcomp.Name = "txt_regcomp";
            this.txt_regcomp.Size = new System.Drawing.Size(393, 20);
            this.txt_regcomp.TabIndex = 42;
            // 
            // btn_uploadxmlpath
            // 
            this.btn_uploadxmlpath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_uploadxmlpath.BackColor = System.Drawing.SystemColors.Control;
            this.btn_uploadxmlpath.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_uploadxmlpath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_uploadxmlpath.Image = ((System.Drawing.Image)(resources.GetObject("btn_uploadxmlpath.Image")));
            this.btn_uploadxmlpath.Location = new System.Drawing.Point(480, 40);
            this.btn_uploadxmlpath.Name = "btn_uploadxmlpath";
            this.btn_uploadxmlpath.Size = new System.Drawing.Size(26, 23);
            this.btn_uploadxmlpath.TabIndex = 41;
            this.btn_uploadxmlpath.UseVisualStyleBackColor = false;
            // 
            // lbl_uploadxml
            // 
            this.lbl_uploadxml.AutoSize = true;
            this.lbl_uploadxml.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_uploadxml.Location = new System.Drawing.Point(16, 45);
            this.lbl_uploadxml.Name = "lbl_uploadxml";
            this.lbl_uploadxml.Size = new System.Drawing.Size(59, 13);
            this.lbl_uploadxml.TabIndex = 40;
            this.lbl_uploadxml.Text = "&XML Path";
            // 
            // txt_xmlpath
            // 
            this.txt_xmlpath.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_xmlpath.Location = new System.Drawing.Point(187, 41);
            this.txt_xmlpath.Name = "txt_xmlpath";
            this.txt_xmlpath.Size = new System.Drawing.Size(287, 20);
            this.txt_xmlpath.TabIndex = 39;
            // 
            // txt_id
            // 
            this.txt_id.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_id.Location = new System.Drawing.Point(187, 17);
            this.txt_id.Name = "txt_id";
            this.txt_id.Size = new System.Drawing.Size(134, 20);
            this.txt_id.TabIndex = 38;
            // 
            // lbl_id
            // 
            this.lbl_id.AutoSize = true;
            this.lbl_id.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_id.Location = new System.Drawing.Point(16, 21);
            this.lbl_id.Name = "lbl_id";
            this.lbl_id.Size = new System.Drawing.Size(21, 13);
            this.lbl_id.TabIndex = 37;
            this.lbl_id.Text = "&ID";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_uploadscript);
            this.groupBox3.Controls.Add(this.dataGridView1);
            this.groupBox3.Location = new System.Drawing.Point(10, 181);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(764, 187);
            this.groupBox3.TabIndex = 50;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Company Details";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColSrno,
            this.ColCompName,
            this.ColSelect});
            this.dataGridView1.Location = new System.Drawing.Point(12, 42);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(740, 134);
            this.dataGridView1.TabIndex = 0;
            // 
            // btn_uploadscript
            // 
            this.btn_uploadscript.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_uploadscript.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_uploadscript.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_uploadscript.Location = new System.Drawing.Point(670, 13);
            this.btn_uploadscript.Name = "btn_uploadscript";
            this.btn_uploadscript.Size = new System.Drawing.Size(79, 23);
            this.btn_uploadscript.TabIndex = 1;
            this.btn_uploadscript.Text = "&Upload Script";
            this.btn_uploadscript.UseVisualStyleBackColor = true;
            // 
            // ColSrno
            // 
            this.ColSrno.HeaderText = "Sr. No.";
            this.ColSrno.Name = "ColSrno";
            this.ColSrno.ReadOnly = true;
            // 
            // ColCompName
            // 
            this.ColCompName.HeaderText = "Company Name";
            this.ColCompName.Name = "ColCompName";
            this.ColCompName.ReadOnly = true;
            // 
            // ColSelect
            // 
            this.ColSelect.HeaderText = "Select";
            this.ColSelect.Name = "ColSelect";
            this.ColSelect.ReadOnly = true;
            this.ColSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txt_remarks);
            this.groupBox4.Controls.Add(this.lbl_remarks);
            this.groupBox4.Controls.Add(this.lbl_apprby);
            this.groupBox4.Controls.Add(this.txt_apprby);
            this.groupBox4.Controls.Add(this.lbl_bugid);
            this.groupBox4.Controls.Add(this.txt_bugid);
            this.groupBox4.Controls.Add(this.lbl_poamt);
            this.groupBox4.Controls.Add(this.txt_poamt);
            this.groupBox4.Controls.Add(this.lbl_podate);
            this.groupBox4.Controls.Add(this.txt_podate);
            this.groupBox4.Controls.Add(this.txt_pono);
            this.groupBox4.Controls.Add(this.lbl_pono);
            this.groupBox4.Location = new System.Drawing.Point(10, 374);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(764, 231);
            this.groupBox4.TabIndex = 51;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Additional Details";
            // 
            // lbl_remarks
            // 
            this.lbl_remarks.AutoSize = true;
            this.lbl_remarks.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_remarks.Location = new System.Drawing.Point(16, 137);
            this.lbl_remarks.Name = "lbl_remarks";
            this.lbl_remarks.Size = new System.Drawing.Size(58, 13);
            this.lbl_remarks.TabIndex = 49;
            this.lbl_remarks.Text = "&Remarks";
            // 
            // lbl_apprby
            // 
            this.lbl_apprby.AutoSize = true;
            this.lbl_apprby.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_apprby.Location = new System.Drawing.Point(16, 115);
            this.lbl_apprby.Name = "lbl_apprby";
            this.lbl_apprby.Size = new System.Drawing.Size(81, 13);
            this.lbl_apprby.TabIndex = 47;
            this.lbl_apprby.Text = "A&pproved By";
            // 
            // txt_apprby
            // 
            this.txt_apprby.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_apprby.Location = new System.Drawing.Point(187, 111);
            this.txt_apprby.Name = "txt_apprby";
            this.txt_apprby.Size = new System.Drawing.Size(393, 20);
            this.txt_apprby.TabIndex = 46;
            // 
            // lbl_bugid
            // 
            this.lbl_bugid.AutoSize = true;
            this.lbl_bugid.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_bugid.Location = new System.Drawing.Point(16, 93);
            this.lbl_bugid.Name = "lbl_bugid";
            this.lbl_bugid.Size = new System.Drawing.Size(47, 13);
            this.lbl_bugid.TabIndex = 45;
            this.lbl_bugid.Text = "&Bug ID";
            // 
            // txt_bugid
            // 
            this.txt_bugid.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_bugid.Location = new System.Drawing.Point(187, 89);
            this.txt_bugid.Name = "txt_bugid";
            this.txt_bugid.Size = new System.Drawing.Size(393, 20);
            this.txt_bugid.TabIndex = 44;
            // 
            // lbl_poamt
            // 
            this.lbl_poamt.AutoSize = true;
            this.lbl_poamt.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_poamt.Location = new System.Drawing.Point(16, 69);
            this.lbl_poamt.Name = "lbl_poamt";
            this.lbl_poamt.Size = new System.Drawing.Size(71, 13);
            this.lbl_poamt.TabIndex = 43;
            this.lbl_poamt.Text = "PO A&mount";
            // 
            // txt_poamt
            // 
            this.txt_poamt.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_poamt.Location = new System.Drawing.Point(187, 65);
            this.txt_poamt.Name = "txt_poamt";
            this.txt_poamt.Size = new System.Drawing.Size(87, 20);
            this.txt_poamt.TabIndex = 42;
            // 
            // lbl_podate
            // 
            this.lbl_podate.AutoSize = true;
            this.lbl_podate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_podate.Location = new System.Drawing.Point(16, 45);
            this.lbl_podate.Name = "lbl_podate";
            this.lbl_podate.Size = new System.Drawing.Size(54, 13);
            this.lbl_podate.TabIndex = 40;
            this.lbl_podate.Text = "PO D&ate";
            // 
            // txt_podate
            // 
            this.txt_podate.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_podate.Location = new System.Drawing.Point(187, 41);
            this.txt_podate.Name = "txt_podate";
            this.txt_podate.Size = new System.Drawing.Size(87, 20);
            this.txt_podate.TabIndex = 39;
            // 
            // txt_pono
            // 
            this.txt_pono.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_pono.Location = new System.Drawing.Point(187, 17);
            this.txt_pono.Name = "txt_pono";
            this.txt_pono.Size = new System.Drawing.Size(287, 20);
            this.txt_pono.TabIndex = 38;
            // 
            // lbl_pono
            // 
            this.lbl_pono.AutoSize = true;
            this.lbl_pono.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_pono.Location = new System.Drawing.Point(16, 21);
            this.lbl_pono.Name = "lbl_pono";
            this.lbl_pono.Size = new System.Drawing.Size(46, 13);
            this.lbl_pono.TabIndex = 37;
            this.lbl_pono.Text = "PO N&o.";
            // 
            // txt_remarks
            // 
            this.txt_remarks.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_remarks.Location = new System.Drawing.Point(187, 137);
            this.txt_remarks.Multiline = true;
            this.txt_remarks.Name = "txt_remarks";
            this.txt_remarks.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_remarks.Size = new System.Drawing.Size(393, 88);
            this.txt_remarks.TabIndex = 50;
            // 
            // frmUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 660);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tlsMain);
            this.Name = "frmUI";
            this.Text = "frmUI";
            this.tlsMain.ResumeLayout(false);
            this.tlsMain.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tlsMain;
        private System.Windows.Forms.ToolStripButton tlsbtnSave;
        private System.Windows.Forms.ToolStripButton tlsbtnCancel;
        private System.Windows.Forms.ToolStripSeparator tlsSep1;
        private System.Windows.Forms.ToolStripButton tlsbtnSearch;
        private System.Windows.Forms.ToolStripButton tlsbtnExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbl_macid;
        private System.Windows.Forms.TextBox txt_macid;
        private System.Windows.Forms.Label lbl_prodver;
        private System.Windows.Forms.TextBox txt_prodver;
        private System.Windows.Forms.Label lbl_date;
        private System.Windows.Forms.TextBox txt_date;
        private System.Windows.Forms.Label lbl_regcomp;
        private System.Windows.Forms.TextBox txt_regcomp;
        private System.Windows.Forms.Button btn_uploadxmlpath;
        private System.Windows.Forms.Label lbl_uploadxml;
        private System.Windows.Forms.TextBox txt_xmlpath;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label lbl_id;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_uploadscript;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSrno;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCompName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColSelect;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lbl_remarks;
        private System.Windows.Forms.Label lbl_apprby;
        private System.Windows.Forms.TextBox txt_apprby;
        private System.Windows.Forms.Label lbl_bugid;
        private System.Windows.Forms.TextBox txt_bugid;
        private System.Windows.Forms.Label lbl_poamt;
        private System.Windows.Forms.TextBox txt_poamt;
        private System.Windows.Forms.Label lbl_podate;
        private System.Windows.Forms.TextBox txt_podate;
        private System.Windows.Forms.TextBox txt_pono;
        private System.Windows.Forms.Label lbl_pono;
        private System.Windows.Forms.TextBox txt_remarks;
    }
}