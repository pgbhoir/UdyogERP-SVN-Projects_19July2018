using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLA.APPS
{
    public partial class MLAAppRemarks : Form
    {
        private string remarks;
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        public MLAAppRemarks()
        {
            InitializeComponent();
        }

        private string btnstatus = "cancel";

        private void MLAAppRemarks_Load(object sender, EventArgs e)
        {
            memRemarks.Text = Remarks;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (memRemarks.Text == string.Empty)
            {
                errorProvider.SetError(memRemarks, "Details are required");
                return;
            }

            Remarks = memRemarks.Text;
            btnstatus = "ok";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            btnstatus = "cancel";
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (btnstatus == "cancel")
            {
                var dr = MessageBox.Show("Discard the changes ?\n\n" +
                                "This action will not save comments...!!!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.No)
                    e.Cancel = true;
            }
        }
     

     
    }
}
