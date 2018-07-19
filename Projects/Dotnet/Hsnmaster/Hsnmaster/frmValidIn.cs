using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hsnmaster
{
    public partial class frmValidIn : Form
    {
        public DataSet ds;
        public string validdata = string.Empty,actstr=string.Empty;
        public string[] InpValid, asd1;
        public DataTable DtValid = new DataTable();
        public frmValidIn()
        {
            InitializeComponent();
        }

        public void getds(DataSet ds2,string InValid)
        {
            InpValid = InValid.Split('/');
            validdata = InValid;
            int i=0;
            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
               listView1.Items.Add(dr["State"].ToString());
               //asd1 = InpValid.Where(n => n == dr["GST_State_Code"].ToString()).ToArray();
               
                foreach (string lst in InpValid)
                {
                    if (dr["GST_State_Code"].ToString().Trim() == lst.Trim() && lst.Trim()!="")
                    {
                        listView1.Items[i].Checked = true;
                    }
                }
                i =i+1;
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
             DtValid.Columns.AddRange(new DataColumn[1] { new DataColumn("GSTSCode") }); 
            string data=string.Empty;            
            foreach (ListViewItem it in listView1.Items)
            {              
                if (it.Checked)
                {
                    DataRow[] drflt =ds.Tables[0].Select("State='"+it.Text+"'");
                    DtValid.Rows.Add(drflt[0].ItemArray[1].ToString());
                    data = data + drflt[0].ItemArray[1].ToString() + "/" ;
                }
            }
            
            validdata = data;
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
