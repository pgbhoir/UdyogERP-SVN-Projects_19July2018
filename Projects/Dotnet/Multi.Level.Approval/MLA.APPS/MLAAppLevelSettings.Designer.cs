namespace MLA.APPS
{
    partial class MLAAppLevelSettings
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MLAAppLevelSettings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deTooltip = new DevExpress.Utils.DefaultToolTipController(this.components);
            this.btnGetData = new System.Windows.Forms.Button();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkPasslevel = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.cboAppLevel = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtgrdvwAppLevel = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treevwLevels = new System.Windows.Forms.TreeView();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.treevwNodes = new System.Windows.Forms.TreeView();
            this.btnMainCancel = new System.Windows.Forms.Button();
            this.btnMainSave = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lkeditInitiator = new DevExpress.XtraEditors.LookUpEdit();
            this.lkeditUserRoles = new DevExpress.XtraEditors.LookUpEdit();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.imagelist = new System.Windows.Forms.ImageList(this.components);
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdvwAppLevel)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeditInitiator.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeditUserRoles.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lkeditInitiator);
            this.groupBox1.Controls.Add(this.btnGetData);
            this.groupBox1.Controls.Add(this.chkActive);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.dtgrdvwAppLevel);
            this.groupBox1.Controls.Add(this.treevwLevels);
            this.groupBox1.Controls.Add(this.pictureBox3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.treevwNodes);
            this.groupBox1.Location = new System.Drawing.Point(9, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(838, 441);
            this.deTooltip.SetSuperTip(this.groupBox1, null);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "   Approval Level Workflow   ";
            // 
            // deTooltip
            // 
            // 
            // 
            // 
            this.deTooltip.DefaultController.Appearance.Options.UseTextOptions = true;
            this.deTooltip.DefaultController.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.deTooltip.DefaultController.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.deTooltip.DefaultController.ToolTipLocation = DevExpress.Utils.ToolTipLocation.BottomCenter;
            // 
            // btnGetData
            // 
            this.btnGetData.Image = global::MLA.APPS.Properties.Resources.downloads_icon;
            this.btnGetData.Location = new System.Drawing.Point(482, 57);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(32, 24);
            this.deTooltip.SetSuperTip(this.btnGetData, null);
            this.btnGetData.TabIndex = 38;
            this.btnGetData.Tag = "Add";
            this.toolTip.SetToolTip(this.btnGetData, "Click for get Data");
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkActive.Location = new System.Drawing.Point(581, 62);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.deTooltip.SetSuperTip(this.chkActive, null);
            this.chkActive.TabIndex = 34;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            this.chkActive.Click += new System.EventHandler(this.chkActive_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(202, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.deTooltip.SetSuperTip(this.label6, null);
            this.label6.TabIndex = 32;
            this.label6.Text = "Initiator";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MLA.APPS.Properties.Resources.Increase_icon;
            this.pictureBox1.Location = new System.Drawing.Point(771, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 25);
            this.deTooltip.SetSuperTip(this.pictureBox1, null);
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(646, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 13);
            this.deTooltip.SetSuperTip(this.label4, null);
            this.label4.TabIndex = 16;
            this.label4.Text = "Treeview Display";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkPasslevel);
            this.groupBox2.Controls.Add(this.lkeditUserRoles);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.btnUpdate);
            this.groupBox2.Controls.Add(this.cboAppLevel);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(197, 91);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(444, 128);
            this.deTooltip.SetSuperTip(this.groupBox2, null);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Approval Level Input Details  ";
            // 
            // chkPasslevel
            // 
            this.chkPasslevel.AutoSize = true;
            this.chkPasslevel.Location = new System.Drawing.Point(127, 70);
            this.chkPasslevel.Name = "chkPasslevel";
            this.chkPasslevel.Size = new System.Drawing.Size(15, 14);
            this.deTooltip.SetSuperTip(this.chkPasslevel, null);
            this.chkPasslevel.TabIndex = 27;
            this.chkPasslevel.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.deTooltip.SetSuperTip(this.label5, null);
            this.label5.TabIndex = 26;
            this.label5.Text = "Pass Prev. Level";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::MLA.APPS.Properties.Resources.cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(223, 99);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(66, 23);
            this.deTooltip.SetSuperTip(this.btnCancel, null);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = " Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::MLA.APPS.Properties.Resources.delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(25, 99);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 23);
            this.deTooltip.SetSuperTip(this.btnDelete, null);
            this.btnDelete.TabIndex = 24;
            this.btnDelete.Text = "Remove";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.btnDelete, "Click for Delete Approve Level");
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::MLA.APPS.Properties.Resources.save;
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdate.Location = new System.Drawing.Point(304, 99);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(66, 23);
            this.deTooltip.SetSuperTip(this.btnUpdate, null);
            this.btnUpdate.TabIndex = 23;
            this.btnUpdate.Tag = "Add";
            this.btnUpdate.Text = "Assign";
            this.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.btnUpdate, "Click for Add Approval Level");
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // cboAppLevel
            // 
            this.cboAppLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAppLevel.FormattingEnabled = true;
            this.cboAppLevel.Location = new System.Drawing.Point(127, 19);
            this.cboAppLevel.Name = "cboAppLevel";
            this.cboAppLevel.Size = new System.Drawing.Size(253, 21);
            this.deTooltip.SetSuperTip(this.cboAppLevel, null);
            this.cboAppLevel.TabIndex = 21;
            this.cboAppLevel.SelectedIndexChanged += new System.EventHandler(this.cboAppLevel_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.deTooltip.SetSuperTip(this.label3, null);
            this.label3.TabIndex = 20;
            this.label3.Text = "Role / User ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.deTooltip.SetSuperTip(this.label2, null);
            this.label2.TabIndex = 19;
            this.label2.Text = "Approval Level";
            // 
            // dtgrdvwAppLevel
            // 
            this.dtgrdvwAppLevel.AllowUserToAddRows = false;
            this.dtgrdvwAppLevel.AllowUserToDeleteRows = false;
            this.dtgrdvwAppLevel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgrdvwAppLevel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column4,
            this.Column3,
            this.Column5,
            this.Column6});
            this.dtgrdvwAppLevel.ContextMenuStrip = this.contextMenuStrip;
            this.dtgrdvwAppLevel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dtgrdvwAppLevel.Location = new System.Drawing.Point(197, 230);
            this.dtgrdvwAppLevel.MultiSelect = false;
            this.dtgrdvwAppLevel.Name = "dtgrdvwAppLevel";
            this.dtgrdvwAppLevel.ReadOnly = true;
            this.dtgrdvwAppLevel.RowHeadersVisible = false;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.dtgrdvwAppLevel.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dtgrdvwAppLevel.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dtgrdvwAppLevel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgrdvwAppLevel.Size = new System.Drawing.Size(444, 201);
            this.deTooltip.SetSuperTip(this.dtgrdvwAppLevel, null);
            this.dtgrdvwAppLevel.TabIndex = 14;
            this.dtgrdvwAppLevel.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgrdvwAppLevel_CellContentDoubleClick);
            this.dtgrdvwAppLevel.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.dtgrdvwAppLevel_CellToolTipTextNeeded);
            this.dtgrdvwAppLevel.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtgrdvwAppLevel_ColumnHeaderMouseClick);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Applevel";
            this.Column1.HeaderText = "Approval Level";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.ToolTipText = "Right click for Edit / Delete \\n OR \\n Double click on gridline for Edit";
            this.Column1.Width = 110;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "User_Role";
            this.Column2.HeaderText = "Role / User";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 110;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 50;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "type";
            this.Column3.HeaderText = "Type";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 80;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "bypasslevel";
            this.Column5.HeaderText = "Pass Prev. Level";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 84;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "Id";
            this.Column6.HeaderText = "ID";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Visible = false;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(118, 48);
            this.deTooltip.SetSuperTip(this.contextMenuStrip, null);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = global::MLA.APPS.Properties.Resources.edit;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::MLA.APPS.Properties.Resources.delete;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem.Text = "Remove";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // treevwLevels
            // 
            this.treevwLevels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treevwLevels.Location = new System.Drawing.Point(649, 58);
            this.treevwLevels.Name = "treevwLevels";
            this.treevwLevels.Size = new System.Drawing.Size(174, 373);
            this.deTooltip.SetSuperTip(this.treevwLevels, null);
            this.treevwLevels.TabIndex = 13;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::MLA.APPS.Properties.Resources.menu_item_icon;
            this.pictureBox3.Location = new System.Drawing.Point(72, 28);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(24, 25);
            this.deTooltip.SetSuperTip(this.pictureBox3, null);
            this.pictureBox3.TabIndex = 12;
            this.pictureBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.deTooltip.SetSuperTip(this.label1, null);
            this.label1.TabIndex = 11;
            this.label1.Text = "Entities";
            // 
            // treevwNodes
            // 
            this.treevwNodes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treevwNodes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.treevwNodes.HideSelection = false;
            this.treevwNodes.Location = new System.Drawing.Point(11, 58);
            this.treevwNodes.Name = "treevwNodes";
            this.treevwNodes.Size = new System.Drawing.Size(174, 373);
            this.deTooltip.SetSuperTip(this.treevwNodes, null);
            this.treevwNodes.TabIndex = 10;
            this.treevwNodes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treevwNodes_AfterSelect);
            // 
            // btnMainCancel
            // 
            this.btnMainCancel.Image = global::MLA.APPS.Properties.Resources.Action_cancel_icon;
            this.btnMainCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMainCancel.Location = new System.Drawing.Point(753, 457);
            this.btnMainCancel.Name = "btnMainCancel";
            this.btnMainCancel.Size = new System.Drawing.Size(77, 24);
            this.deTooltip.SetSuperTip(this.btnMainCancel, null);
            this.btnMainCancel.TabIndex = 34;
            this.btnMainCancel.Text = "Cancel   ";
            this.btnMainCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMainCancel.UseVisualStyleBackColor = true;
            this.btnMainCancel.Click += new System.EventHandler(this.btnMainCancel_Click);
            // 
            // btnMainSave
            // 
            this.btnMainSave.Image = global::MLA.APPS.Properties.Resources.Ok_icon;
            this.btnMainSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMainSave.Location = new System.Drawing.Point(656, 457);
            this.btnMainSave.Name = "btnMainSave";
            this.btnMainSave.Size = new System.Drawing.Size(77, 24);
            this.deTooltip.SetSuperTip(this.btnMainSave, null);
            this.btnMainSave.TabIndex = 33;
            this.btnMainSave.Text = "Save    ";
            this.btnMainSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMainSave.UseVisualStyleBackColor = true;
            this.btnMainSave.Click += new System.EventHandler(this.btnMainSave_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 490);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(858, 22);
            this.deTooltip.SetSuperTip(this.statusStrip, null);
            this.statusStrip.TabIndex = 35;
            this.statusStrip.Text = ".";
            // 
            // lkeditInitiator
            // 
            this.lkeditInitiator.EditValue = "";
            this.lkeditInitiator.Location = new System.Drawing.Point(246, 60);
            this.lkeditInitiator.Name = "lkeditInitiator";
            this.lkeditInitiator.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkeditInitiator.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("user", "User Id.", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("user_name", "Name", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("designation", "Designation", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dept", "Department", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("email", "Email Id.", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.lkeditInitiator.Properties.PopupFormWidth = 400;
            this.lkeditInitiator.Properties.PopupWidth = 400;
            this.lkeditInitiator.Size = new System.Drawing.Size(230, 20);
            this.lkeditInitiator.TabIndex = 39;
            this.lkeditInitiator.ToolTip = "F4 to show or Hide the Dropdown list";
            this.lkeditInitiator.ToolTipController = this.deTooltip.DefaultController;
            this.lkeditInitiator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.lkeditInitiator.ToolTipTitle = "Information";
            // 
            // lkeditUserRoles
            // 
            this.lkeditUserRoles.Location = new System.Drawing.Point(127, 44);
            this.lkeditUserRoles.Name = "lkeditUserRoles";
            this.lkeditUserRoles.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkeditUserRoles.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("type", "Type", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("user_role", "User / Role", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("user_name", "Name", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("designation", "Designation", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dept", "Department", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("email", "Email Id.", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.lkeditUserRoles.Properties.PopupFormWidth = 400;
            this.lkeditUserRoles.Properties.PopupWidth = 400;
            this.lkeditUserRoles.Size = new System.Drawing.Size(253, 20);
            this.lkeditUserRoles.TabIndex = 37;
            this.toolTip.SetToolTip(this.lkeditUserRoles, "F4 to show or Hide the Dropdown list");
            this.lkeditUserRoles.ToolTip = "F4 to show or Hide the Dropdown list";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(10, 17);
            this.statusLabel.Text = ".";
            // 
            // imagelist
            // 
            this.imagelist.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagelist.ImageStream")));
            this.imagelist.TransparentColor = System.Drawing.Color.Transparent;
            this.imagelist.Images.SetKeyName(0, "user-icon.png");
            this.imagelist.Images.SetKeyName(1, "menu-item-icon16.png");
            this.imagelist.Images.SetKeyName(2, "users-icon.png");
            this.imagelist.Images.SetKeyName(3, "Increase-icon.png");
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.Size = new System.Drawing.Size(300, 300);
            this.xtraTabControl1.TabIndex = 0;
            // 
            // MLAAppLevelSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 512);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnMainCancel);
            this.Controls.Add(this.btnMainSave);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MLAAppLevelSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.deTooltip.SetSuperTip(this, null);
            this.Text = "Approval Level Workflow Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MLAAppLevelSettings_FormClosing);
            this.Load += new System.EventHandler(this.MLAAppLevelSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdvwAppLevel)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeditInitiator.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeditUserRoles.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treevwNodes;
        private System.Windows.Forms.DataGridView dtgrdvwAppLevel;
        private System.Windows.Forms.TreeView treevwLevels;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboAppLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.ImageList imagelist;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnMainCancel;
        private System.Windows.Forms.Button btnMainSave;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkPasslevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewImageColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.LookUpEdit lkeditUserRoles;
        private DevExpress.XtraEditors.LookUpEdit lkeditInitiator;
        private DevExpress.Utils.DefaultToolTipController deTooltip;
        private System.Windows.Forms.Button btnGetData;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
    }
}