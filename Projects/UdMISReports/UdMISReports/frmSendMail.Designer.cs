namespace UdMISReports
{
    partial class frmSendMail
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
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip4 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem4 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip5 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem5 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSendMail));
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.lblFrom = new DevExpress.XtraEditors.LabelControl();
            this.txtSubject = new DevExpress.XtraEditors.TextEdit();
            this.lblSubject = new DevExpress.XtraEditors.LabelControl();
            this.txtFrom = new DevExpress.XtraEditors.TextEdit();
            this.txtBCC = new DevExpress.XtraEditors.TextEdit();
            this.lblBCC = new DevExpress.XtraEditors.LabelControl();
            this.txtCC = new DevExpress.XtraEditors.TextEdit();
            this.lblCC = new DevExpress.XtraEditors.LabelControl();
            this.txtTO = new DevExpress.XtraEditors.TextEdit();
            this.lblTO = new DevExpress.XtraEditors.LabelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtMailBody = new System.Windows.Forms.TextBox();
            this.lblAttachmentFileName = new DevExpress.XtraEditors.LabelControl();
            this.lblAttachment = new DevExpress.XtraEditors.LabelControl();
            this.txtSend = new DevExpress.XtraEditors.SimpleButton();
            this.txtCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubject.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBCC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.Appearance.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panelControl2.Appearance.Options.UseBackColor = true;
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.panelControl2.Controls.Add(this.lblFrom);
            this.panelControl2.Controls.Add(this.txtSubject);
            this.panelControl2.Controls.Add(this.lblSubject);
            this.panelControl2.Controls.Add(this.txtFrom);
            this.panelControl2.Controls.Add(this.txtBCC);
            this.panelControl2.Controls.Add(this.lblBCC);
            this.panelControl2.Controls.Add(this.txtCC);
            this.panelControl2.Controls.Add(this.lblCC);
            this.panelControl2.Controls.Add(this.txtTO);
            this.panelControl2.Controls.Add(this.lblTO);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(583, 119);
            this.panelControl2.TabIndex = 5;
            // 
            // lblFrom
            // 
            this.lblFrom.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblFrom.Location = new System.Drawing.Point(6, 75);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 16);
            this.lblFrom.TabIndex = 6;
            this.lblFrom.Text = "From :";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(123, 96);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtSubject.Properties.Appearance.Options.UseFont = true;
            this.txtSubject.Size = new System.Drawing.Size(453, 20);
            this.txtSubject.TabIndex = 9;
            // 
            // lblSubject
            // 
            this.lblSubject.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblSubject.Location = new System.Drawing.Point(6, 96);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(58, 16);
            this.lblSubject.TabIndex = 8;
            this.lblSubject.Text = "Subject :";
            // 
            // txtFrom
            // 
            this.txtFrom.Enabled = false;
            this.txtFrom.Location = new System.Drawing.Point(123, 73);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtFrom.Properties.Appearance.Options.UseFont = true;
            this.txtFrom.Size = new System.Drawing.Size(453, 20);
            toolTipItem2.Text = "you can\'t Edit";
            superToolTip2.Items.Add(toolTipItem2);
            this.txtFrom.SuperTip = superToolTip2;
            this.txtFrom.TabIndex = 7;
            this.txtFrom.ToolTip = "you can\'t Edit";
            // 
            // txtBCC
            // 
            this.txtBCC.Location = new System.Drawing.Point(123, 51);
            this.txtBCC.Name = "txtBCC";
            this.txtBCC.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtBCC.Properties.Appearance.Options.UseFont = true;
            this.txtBCC.Size = new System.Drawing.Size(453, 20);
            toolTipItem3.Text = "Enter mail id\'s by separating with \";\" \r\nEx: a@b.com\r\nEx:a@b.com;c@d.net";
            superToolTip3.Items.Add(toolTipItem3);
            this.txtBCC.SuperTip = superToolTip3;
            this.txtBCC.TabIndex = 5;
            this.txtBCC.ToolTip = "Please Enter one or more with \";\"";
            // 
            // lblBCC
            // 
            this.lblBCC.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblBCC.Location = new System.Drawing.Point(6, 53);
            this.lblBCC.Name = "lblBCC";
            this.lblBCC.Size = new System.Drawing.Size(33, 16);
            this.lblBCC.TabIndex = 4;
            this.lblBCC.Text = "BCC :";
            // 
            // txtCC
            // 
            this.txtCC.Location = new System.Drawing.Point(123, 28);
            this.txtCC.Name = "txtCC";
            this.txtCC.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtCC.Properties.Appearance.Options.UseFont = true;
            this.txtCC.Size = new System.Drawing.Size(453, 20);
            toolTipItem4.Text = "Enter mail id\'s by separating with \";\"  \r\nEx: a@b.com\r\nEx:a@b.com;c@d.net";
            superToolTip4.Items.Add(toolTipItem4);
            this.txtCC.SuperTip = superToolTip4;
            this.txtCC.TabIndex = 3;
            this.txtCC.ToolTip = "Please Enter one or more with \";\"";
            // 
            // lblCC
            // 
            this.lblCC.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblCC.Location = new System.Drawing.Point(6, 29);
            this.lblCC.Name = "lblCC";
            this.lblCC.Size = new System.Drawing.Size(25, 16);
            this.lblCC.TabIndex = 2;
            this.lblCC.Text = "CC :";
            // 
            // txtTO
            // 
            this.txtTO.Location = new System.Drawing.Point(123, 5);
            this.txtTO.Name = "txtTO";
            this.txtTO.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtTO.Properties.Appearance.Options.UseFont = true;
            this.txtTO.Size = new System.Drawing.Size(453, 20);
            toolTipItem5.Text = "Enter mail id\'s by separating with \";\" \r\nEx: a@b.com\r\nEx:a@b.com;c@d.net";
            superToolTip5.Items.Add(toolTipItem5);
            this.txtTO.SuperTip = superToolTip5;
            this.txtTO.TabIndex = 1;
            this.txtTO.ToolTip = "Please Enter one or more with \";\"";
            // 
            // lblTO
            // 
            this.lblTO.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblTO.Location = new System.Drawing.Point(6, 8);
            this.lblTO.Name = "lblTO";
            this.lblTO.Size = new System.Drawing.Size(25, 16);
            this.lblTO.TabIndex = 0;
            this.lblTO.Text = "TO :";
            // 
            // panelControl3
            // 
            this.panelControl3.Appearance.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panelControl3.Appearance.Options.UseBackColor = true;
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.panelControl3.Controls.Add(this.txtSend);
            this.panelControl3.Controls.Add(this.txtCancel);
            this.panelControl3.Controls.Add(this.pictureBox1);
            this.panelControl3.Controls.Add(this.txtMailBody);
            this.panelControl3.Controls.Add(this.lblAttachmentFileName);
            this.panelControl3.Controls.Add(this.lblAttachment);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 119);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(583, 242);
            this.panelControl3.TabIndex = 6;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(123, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // txtMailBody
            // 
            this.txtMailBody.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMailBody.Location = new System.Drawing.Point(6, 40);
            this.txtMailBody.Multiline = true;
            this.txtMailBody.Name = "txtMailBody";
            this.txtMailBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMailBody.Size = new System.Drawing.Size(570, 162);
            this.txtMailBody.TabIndex = 2;
            // 
            // lblAttachmentFileName
            // 
            this.lblAttachmentFileName.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblAttachmentFileName.Location = new System.Drawing.Point(145, 9);
            this.lblAttachmentFileName.Name = "lblAttachmentFileName";
            this.lblAttachmentFileName.Size = new System.Drawing.Size(49, 13);
            this.lblAttachmentFileName.TabIndex = 1;
            this.lblAttachmentFileName.Text = "filename";
            // 
            // lblAttachment
            // 
            this.lblAttachment.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblAttachment.Location = new System.Drawing.Point(6, 6);
            this.lblAttachment.Name = "lblAttachment";
            this.lblAttachment.Size = new System.Drawing.Size(87, 16);
            this.lblAttachment.TabIndex = 0;
            this.lblAttachment.Text = "Attachment :";
            // 
            // txtSend
            // 
            this.txtSend.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtSend.Appearance.Options.UseFont = true;
            this.txtSend.Location = new System.Drawing.Point(415, 208);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(75, 23);
            this.txtSend.TabIndex = 5;
            this.txtSend.Text = "Send";
            this.txtSend.Click += new System.EventHandler(this.txtSend_Click);
            // 
            // txtCancel
            // 
            this.txtCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtCancel.Appearance.Options.UseFont = true;
            this.txtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.txtCancel.Location = new System.Drawing.Point(496, 208);
            this.txtCancel.Name = "txtCancel";
            this.txtCancel.Size = new System.Drawing.Size(75, 23);
            this.txtCancel.TabIndex = 6;
            this.txtCancel.Text = "Cancel";
            this.txtCancel.Click += new System.EventHandler(this.txtCancel_Click);
            // 
            // frmSendMail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 361);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSendMail";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Send Mail";
            this.Load += new System.EventHandler(this.frmSendMail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubject.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBCC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl lblFrom;
        private DevExpress.XtraEditors.TextEdit txtSubject;
        private DevExpress.XtraEditors.LabelControl lblSubject;
        private DevExpress.XtraEditors.TextEdit txtFrom;
        private DevExpress.XtraEditors.TextEdit txtBCC;
        private DevExpress.XtraEditors.LabelControl lblBCC;
        private DevExpress.XtraEditors.TextEdit txtCC;
        private DevExpress.XtraEditors.LabelControl lblCC;
        private DevExpress.XtraEditors.TextEdit txtTO;
        private DevExpress.XtraEditors.LabelControl lblTO;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton txtSend;
        private DevExpress.XtraEditors.SimpleButton txtCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtMailBody;
        private DevExpress.XtraEditors.LabelControl lblAttachmentFileName;
        private DevExpress.XtraEditors.LabelControl lblAttachment;
    }
}