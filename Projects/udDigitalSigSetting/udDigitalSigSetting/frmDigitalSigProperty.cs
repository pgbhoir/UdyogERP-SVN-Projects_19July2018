using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace udDigitalSigSetting
{
    public partial class frmDigitalSigProperty : uBaseForm.FrmBaseForm
    {
      public  DataTable dtDigSigProperty;
        public frmDigitalSigProperty()
        {
            InitializeComponent();
             this.pDisableCloseBtn = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void frmDigitalSigProperty_Load(object sender, EventArgs e)
        {
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.DataSource = dtDigSigProperty;
            dataGridView2.Columns["colfldnm1"].DataPropertyName = "fldnm";
            dataGridView2.Columns["colPrifix1"].DataPropertyName = "prefix";
            dataGridView2.Columns["colSufix1"].DataPropertyName = "sufix";
            dataGridView2.Columns["colfldval1"].DataPropertyName = "fldval";
            dataGridView2.Columns["colprint1"].DataPropertyName = "fldprint";
            dataGridView2.Columns["colnewline1"].DataPropertyName = "newline";
            dataGridView2.Columns["colSerial1"].DataPropertyName = "Serial";

            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //  dataGridView1.AutoGenerateColumns = false;
            // dataGridView2.DataSource = frmDigitalSigSetting.dtDigProperty;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dataGridView2.EndEdit();
            dtDigSigProperty.AcceptChanges();
            this.Close();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnLogout, e);
        }
    }
}
