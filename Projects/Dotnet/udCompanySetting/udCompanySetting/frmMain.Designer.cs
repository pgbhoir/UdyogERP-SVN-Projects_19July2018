namespace udCompanySetting
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
            this.btnGen = new System.Windows.Forms.Button();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btnLogout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLast = new System.Windows.Forms.ToolStripButton();
            this.btnForward = new System.Windows.Forms.ToolStripButton();
            this.btnBack = new System.Windows.Forms.ToolStripButton();
            this.btnFirst = new System.Windows.Forms.ToolStripButton();
            this.gbMain = new System.Windows.Forms.GroupBox();
            this.btnIRN = new System.Windows.Forms.Button();
            this.btnBarCode = new System.Windows.Forms.Button();
            this.btnMultiCurr = new System.Windows.Forms.Button();
            this.btnNIC = new System.Windows.Forms.Button();
            this.btnPayroll = new System.Windows.Forms.Button();
            this.btnMultiUOM = new System.Windows.Forms.Button();
            this.btnInv = new System.Windows.Forms.Button();
            this.btnAcc = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSMS = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.gbMain.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGen
            // 
            this.btnGen.Location = new System.Drawing.Point(6, 19);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(122, 23);
            this.btnGen.TabIndex = 0;
            this.btnGen.Text = "General";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Enabled = false;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.cancelToolStripMenuItem.Text = "Cancel";
            this.cancelToolStripMenuItem.Click += new System.EventHandler(this.cancelToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Enabled = false;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.newToolStripMenuItem.Text = "New";
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
            this.menuStrip1.Size = new System.Drawing.Size(434, 24);
            this.menuStrip1.TabIndex = 69;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // btnLogout
            // 
            this.btnLogout.AutoSize = false;
            this.btnLogout.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 38);
            // 
            // btnDelete
            // 
            this.btnDelete.AutoSize = false;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(50, 35);
            this.btnDelete.Text = "Delete";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDelete.ToolTipText = "Delete(Ctrl+D)";
            this.btnDelete.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = false;
            this.btnCancel.Enabled = false;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // btnEdit
            // 
            this.btnEdit.AutoSize = false;
            this.btnEdit.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // btnSave
            // 
            this.btnSave.AutoSize = false;
            this.btnSave.Enabled = false;
            this.btnSave.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // btnNew
            // 
            this.btnNew.AutoSize = false;
            this.btnNew.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.ForeColor = System.Drawing.Color.Black;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(50, 35);
            this.btnNew.Text = "New";
            this.btnNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnNew.ToolTipText = "New(Ctrl+N)";
            this.btnNew.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 38);
            this.toolStripSeparator1.Visible = false;
            // 
            // btnLast
            // 
            this.btnLast.AutoSize = false;
            this.btnLast.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLast.ForeColor = System.Drawing.Color.Black;
            this.btnLast.Image = ((System.Drawing.Image)(resources.GetObject("btnLast.Image")));
            this.btnLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(50, 35);
            this.btnLast.Text = "Last";
            this.btnLast.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnLast.ToolTipText = "Last";
            this.btnLast.Visible = false;
            // 
            // btnForward
            // 
            this.btnForward.AutoSize = false;
            this.btnForward.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnForward.ForeColor = System.Drawing.Color.Black;
            this.btnForward.Image = ((System.Drawing.Image)(resources.GetObject("btnForward.Image")));
            this.btnForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(50, 35);
            this.btnForward.Text = "Next";
            this.btnForward.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnForward.ToolTipText = "Next";
            this.btnForward.Visible = false;
            // 
            // btnBack
            // 
            this.btnBack.AutoSize = false;
            this.btnBack.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.Color.Black;
            this.btnBack.Image = ((System.Drawing.Image)(resources.GetObject("btnBack.Image")));
            this.btnBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(50, 35);
            this.btnBack.Text = "Prev";
            this.btnBack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnBack.ToolTipText = "Prev";
            this.btnBack.Visible = false;
            // 
            // btnFirst
            // 
            this.btnFirst.AutoSize = false;
            this.btnFirst.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFirst.ForeColor = System.Drawing.Color.Black;
            this.btnFirst.Image = ((System.Drawing.Image)(resources.GetObject("btnFirst.Image")));
            this.btnFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(50, 35);
            this.btnFirst.Text = "First";
            this.btnFirst.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnFirst.ToolTipText = "First";
            this.btnFirst.Visible = false;
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.btnSMS);
            this.gbMain.Controls.Add(this.btnIRN);
            this.gbMain.Controls.Add(this.btnBarCode);
            this.gbMain.Controls.Add(this.btnMultiCurr);
            this.gbMain.Controls.Add(this.btnNIC);
            this.gbMain.Controls.Add(this.btnPayroll);
            this.gbMain.Controls.Add(this.btnMultiUOM);
            this.gbMain.Controls.Add(this.btnInv);
            this.gbMain.Controls.Add(this.btnAcc);
            this.gbMain.Controls.Add(this.btnGen);
            this.gbMain.Location = new System.Drawing.Point(11, 45);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(412, 138);
            this.gbMain.TabIndex = 0;
            this.gbMain.TabStop = false;
            this.gbMain.Text = "Setting";
            this.gbMain.Enter += new System.EventHandler(this.gbMain_Enter);
            // 
            // btnIRN
            // 
            this.btnIRN.Location = new System.Drawing.Point(280, 77);
            this.btnIRN.Name = "btnIRN";
            this.btnIRN.Size = new System.Drawing.Size(122, 23);
            this.btnIRN.TabIndex = 8;
            this.btnIRN.Text = "IRN Credentials";
            this.btnIRN.UseVisualStyleBackColor = true;
            this.btnIRN.Click += new System.EventHandler(this.btnIRN_Click);
            // 
            // btnBarCode
            // 
            this.btnBarCode.Location = new System.Drawing.Point(280, 19);
            this.btnBarCode.Name = "btnBarCode";
            this.btnBarCode.Size = new System.Drawing.Size(122, 23);
            this.btnBarCode.TabIndex = 2;
            this.btnBarCode.Text = "Barcode";
            this.btnBarCode.UseVisualStyleBackColor = true;
            this.btnBarCode.Click += new System.EventHandler(this.btnBarCode_Click);
            // 
            // btnMultiCurr
            // 
            this.btnMultiCurr.Location = new System.Drawing.Point(147, 48);
            this.btnMultiCurr.Name = "btnMultiCurr";
            this.btnMultiCurr.Size = new System.Drawing.Size(122, 23);
            this.btnMultiCurr.TabIndex = 4;
            this.btnMultiCurr.Text = "Multiple Currency";
            this.btnMultiCurr.UseVisualStyleBackColor = true;
            this.btnMultiCurr.Click += new System.EventHandler(this.btnMultiCurr_Click);
            // 
            // btnNIC
            // 
            this.btnNIC.Location = new System.Drawing.Point(280, 48);
            this.btnNIC.Name = "btnNIC";
            this.btnNIC.Size = new System.Drawing.Size(122, 23);
            this.btnNIC.TabIndex = 5;
            this.btnNIC.Text = "NIC Credentials";
            this.btnNIC.UseVisualStyleBackColor = true;
            this.btnNIC.Click += new System.EventHandler(this.btnNIC_Click);
            // 
            // btnPayroll
            // 
            this.btnPayroll.Location = new System.Drawing.Point(147, 77);
            this.btnPayroll.Name = "btnPayroll";
            this.btnPayroll.Size = new System.Drawing.Size(122, 23);
            this.btnPayroll.TabIndex = 7;
            this.btnPayroll.Text = "Payroll";
            this.btnPayroll.UseVisualStyleBackColor = true;
            this.btnPayroll.Click += new System.EventHandler(this.btnPayroll_Click);
            // 
            // btnMultiUOM
            // 
            this.btnMultiUOM.Location = new System.Drawing.Point(6, 77);
            this.btnMultiUOM.Name = "btnMultiUOM";
            this.btnMultiUOM.Size = new System.Drawing.Size(122, 23);
            this.btnMultiUOM.TabIndex = 6;
            this.btnMultiUOM.Text = "Mulitiple UOM";
            this.btnMultiUOM.UseVisualStyleBackColor = true;
            this.btnMultiUOM.Click += new System.EventHandler(this.btnMultiUOM_Click);
            // 
            // btnInv
            // 
            this.btnInv.Location = new System.Drawing.Point(6, 48);
            this.btnInv.Name = "btnInv";
            this.btnInv.Size = new System.Drawing.Size(122, 23);
            this.btnInv.TabIndex = 3;
            this.btnInv.Text = "Inventory";
            this.btnInv.UseVisualStyleBackColor = true;
            this.btnInv.Click += new System.EventHandler(this.btnInv_Click);
            // 
            // btnAcc
            // 
            this.btnAcc.Location = new System.Drawing.Point(147, 19);
            this.btnAcc.Name = "btnAcc";
            this.btnAcc.Size = new System.Drawing.Size(122, 23);
            this.btnAcc.TabIndex = 1;
            this.btnAcc.Text = "Accounts";
            this.btnAcc.UseVisualStyleBackColor = true;
            this.btnAcc.Click += new System.EventHandler(this.btnAcc_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFirst,
            this.btnBack,
            this.btnForward,
            this.btnLast,
            this.toolStripSeparator1,
            this.btnNew,
            this.btnSave,
            this.btnEdit,
            this.btnCancel,
            this.btnDelete,
            this.toolStripSeparator3,
            this.btnLogout});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(434, 38);
            this.toolStrip1.TabIndex = 70;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSMS
            // 
            this.btnSMS.Location = new System.Drawing.Point(6, 106);
            this.btnSMS.Name = "btnSMS";
            this.btnSMS.Size = new System.Drawing.Size(122, 23);
            this.btnSMS.TabIndex = 9;
            this.btnSMS.Text = "SMS";
            this.btnSMS.UseVisualStyleBackColor = true;
            this.btnSMS.Click += new System.EventHandler(this.btnSMS_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 189);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.gbMain);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Company Setting";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.gbMain.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGen;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripButton btnLogout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnCancel;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnLast;
        private System.Windows.Forms.ToolStripButton btnForward;
        private System.Windows.Forms.ToolStripButton btnBack;
        private System.Windows.Forms.ToolStripButton btnFirst;
        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Button btnAcc;
        private System.Windows.Forms.Button btnMultiCurr;
        private System.Windows.Forms.Button btnNIC;
        private System.Windows.Forms.Button btnPayroll;
        private System.Windows.Forms.Button btnMultiUOM;
        private System.Windows.Forms.Button btnInv;
        private System.Windows.Forms.Button btnIRN;
        private System.Windows.Forms.Button btnBarCode;
        private System.Windows.Forms.Button btnSMS;
    }
}

