using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udBarcodePRN
{
    public partial class FilterForm : uBaseForm.FrmBaseForm
    {
        static public string s1;
        DataTable dt = new DataTable();
        public FilterForm()
        {
            InitializeComponent();
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            s1 = "";
            dataGridView1.AutoGenerateColumns = false;
          
               dt = Form1.dtFilterField;
            try
            {
                dt.Columns.Add("select", typeof(bool));
                foreach (DataRow dr in dt.Rows)
                {
                    dr[4] = false;
                }
            }
            catch(Exception ee)
            {

            }
           

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].DataPropertyName = "select";
            dataGridView1.Columns[1].DataPropertyName = "name";
            dataGridView1.Columns[2].DataPropertyName = "dis";
            dataGridView1.Columns[3].DataPropertyName = "table1";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // string s1=string.Empty;




            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string _tblprfix = "";
                string _stblname = row.Cells[3].Value.ToString();
                string _heading = row.Cells[2].Value.ToString();
                if (_stblname == "Header")
                {
                    _tblprfix = "H";
                }
                else if (_stblname == "LineItem")
                {
                    _tblprfix = "L";
                }
                else if (_stblname == "It_Mast")
                {
                    _tblprfix = "I";
                }

                try
                {
                    string s = row.Cells[0].Value.ToString();
                    if (s.ToLower() == "true")
                    {
                        s1 = s1 + "<<" + _tblprfix + "#" + row.Cells[1].Value.ToString() + "#" + _heading + ">>";
                    }

                }
                catch (Exception ee)
                {

                }

            }
            this.Close();
            //textBox1.Text = s1;
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //  MessageBox.Show("DataSource type BEFORE = " + dataGridView1.DataSource.GetType().ToString());

            DataView DV = new DataView(dt);

            DV.RowFilter = string.Format("name LIKE '%{0}%' or dis LIKE '%{0}%'", txtSearch.Text);

            dataGridView1.DataSource = DV;

            //   MessageBox.Show("DataSource type AFTER = " + dataGridView1.DataSource.GetType().ToString());

            //string select, name, dis, table1;
            //DataTable dtFiltRec = new DataTable();
            //dtFiltRec.Columns.Add("select", typeof(bool));
            //dtFiltRec.Columns.Add("name", typeof(string));
            //dtFiltRec.Columns.Add("dis", typeof(string));
            //dtFiltRec.Columns.Add("table1", typeof(string));
            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{

            //    select = row.Cells[0].Value.ToString();
            //    if (select == "True")
            //    {
            //        name = row.Cells[1].Value.ToString();
            //        dis = row.Cells[2].Value.ToString();
            //        table1 = row.Cells[3].Value.ToString();
            //        dtFiltRec.Rows.Add(select, name, dis, table1);
            //    }

            //    if (dtFiltRec.Rows.Count > 0)
            //    {
            //        foreach(DataRow dr in  dtFiltRec.Rows )
            //        {
            //            foreach (DataRow dr1 in dt.Rows)
            //            {
            //                if(dr1["name"].ToString().Trim()== dr["name"].ToString().Trim())
            //                {
            //                    dr1[0] = true;
            //                }
            //            }
            //        }

            //    }

            //}
            ////  DataTable dt = Form1.dtFilterField;

            //DataRow[] dtRow = dt.Select("name='" + txtSearch.Text.ToString() + "'");
            //DataTable dtSearchRec = new DataTable();
            //if (dtRow.Length > 0)
            //{
            //    dtSearchRec = dtRow.CopyToDataTable();
            //}
            //else
            //{
            //    dtSearchRec = dt;
            //}


            //dataGridView1.DataSource = dtSearchRec;
            //dataGridView1.Columns[0].DataPropertyName = "select";
            //dataGridView1.Columns[1].DataPropertyName = "name";
            //dataGridView1.Columns[2].DataPropertyName = "dis";
            //dataGridView1.Columns[3].DataPropertyName = "table1";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnLogout, e);
        }
    }
}
