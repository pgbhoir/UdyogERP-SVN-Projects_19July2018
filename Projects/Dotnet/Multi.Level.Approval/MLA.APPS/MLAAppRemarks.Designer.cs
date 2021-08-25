namespace MLA.APPS
{
    partial class MLAAppRemarks
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
            this.memRemarks = new DevExpress.XtraEditors.MemoEdit();
            this.btnOk = new DevExpress.XtraEditors.ButtonEdit();
            this.btnCancel = new DevExpress.XtraEditors.ButtonEdit();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.memRemarks.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // memRemarks
            // 
            this.memRemarks.Location = new System.Drawing.Point(3, 8);
            this.memRemarks.Name = "memRemarks";
            this.memRemarks.Size = new System.Drawing.Size(380, 155);
            this.memRemarks.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.EditValue = "Ok";
            this.btnOk.Location = new System.Drawing.Point(234, 170);
            this.btnOk.Name = "btnOk";
            this.btnOk.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Ok", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.btnOk.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.btnOk.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.btnOk.Size = new System.Drawing.Size(72, 20);
            this.btnOk.TabIndex = 1;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(312, 170);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Cancel", -1, true, true, false, DevExpress.Utils.HorzAlignment.Center, null)});
            this.btnCancel.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.btnCancel.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.btnCancel.Size = new System.Drawing.Size(72, 20);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // MLAAppRemarks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 198);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.memRemarks);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MLAAppRemarks";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Comments";
            this.Load += new System.EventHandler(this.MLAAppRemarks_Load);
            ((System.ComponentModel.ISupportInitialize)(this.memRemarks.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit memRemarks;
        private DevExpress.XtraEditors.ButtonEdit btnOk;
        private DevExpress.XtraEditors.ButtonEdit btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}