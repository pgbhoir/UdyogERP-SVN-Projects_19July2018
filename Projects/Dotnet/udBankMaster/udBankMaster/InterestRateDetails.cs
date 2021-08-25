using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace udBankMaster
{


    public partial class InterestRateDetails : uBaseForm.FrmBaseForm
    {
        DataTable vTblMain;
        int ac_id = 0;
        static int acid;
        DataTable childTbl;
        string SqlStr;
        DataAccess_Net.clsDataAccess oDataAccess;
        int days, ecount;
        DateTime sta_dt, end_dt;
        DateTime stdt, endt, selectdate;
        bool flag_dt;
        static public DataTable dtRate = new DataTable();
        public InterestRateDetails()
        {
            InitializeComponent();
            this.pDisableCloseBtn = true;
        }

        private void InterestRateDetails_Load(object sender, EventArgs e)
        {
            sta_dt = new DateTime();
            end_dt = new DateTime();
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            dataGridView1.AutoGenerateColumns = false;

            if (dtRate.Rows.Count == 0 || ac_id != acid)
            {
                SqlStr = "select intFrom as 'from',intTo as 'to',intRate as 'roi',RateperDays AS pnd from InterestRateDetail where ac_id=" + ac_id + "";
                childTbl = new DataTable();
                childTbl = oDataAccess.GetDataTable(SqlStr, null, 20);

                if (childTbl.Rows.Count == 0)
                {
                    SqlStr = "select sta_dt as 'from',end_dt 'to', 0.00'roi',DATEDIFF(day, sta_dt,end_dt)+1 AS pnd from vudyog..co_mast where compid=" + this.pCompId + "";
                    childTbl = new DataTable();
                    childTbl = oDataAccess.GetDataTable(SqlStr, null, 20);

                }
                dataGridView1.DataSource = childTbl;
                dataGridView1.Columns[0].DataPropertyName = "from";
                dataGridView1.Columns[1].DataPropertyName = "to";
                dataGridView1.Columns[2].DataPropertyName = "roi";
                dataGridView1.Columns[3].DataPropertyName = "pnd";
            }
            else
            {
                childTbl = dtRate;
                dataGridView1.DataSource = childTbl;
                dataGridView1.Columns[0].DataPropertyName = "from";
                dataGridView1.Columns[1].DataPropertyName = "to";
                dataGridView1.Columns[2].DataPropertyName = "roi";
                dataGridView1.Columns[3].DataPropertyName = "pnd";
            }

            SqlStr = "select sta_dt as 'from',end_dt 'to', 0.00'roi',DATEDIFF(day, sta_dt,end_dt)+1 AS pnd from vudyog..co_mast where compid=" + this.pCompId + "";
            DataTable childTbl1 = new DataTable();
            childTbl1 = oDataAccess.GetDataTable(SqlStr, null, 20);
            foreach (DataRow dr in childTbl1.Rows)
            {
                sta_dt = DateTime.Parse(dr["from"].ToString());
                end_dt = DateTime.Parse(dr["to"].ToString());
                days = Int32.Parse(dr["pnd"].ToString());
            }

            if (this.pEditMode == true || this.pAddMode == true)
            {
                dataGridView1.Columns[0].ReadOnly = false;
                dataGridView1.Columns[1].ReadOnly = false;
                dataGridView1.Columns[2].ReadOnly = false;
            }
            else
            {

                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;


            }
            acid = ac_id;
        }

        public int Pac_id
        {
            get
            {
                return ac_id;
            }
            set
            {
                ac_id = value;
            }
        }

        public DataTable pTblMain
        {
            get
            {
                return vTblMain;
            }
            set
            {
                vTblMain = value;
            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Add", new System.EventHandler(this.MenuItemClick)));

                int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    m.MenuItems.Add(new MenuItem(string.Format("Remove", currentMouseOverRow.ToString()), new System.EventHandler(this.MenuItemClick)));
                }

                m.Show(dataGridView1, new Point(e.X, e.Y));


            }
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                decimal i;

                if (!decimal.TryParse(Convert.ToString(e.FormattedValue), out i))
                {
                    e.Cancel = true;
                    MessageBox.Show("Please enter valid Rate Of Interest", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {

                }
            }
            if (e.ColumnIndex == 0)
            {

                try
                {
                    stdt = Convert.ToDateTime(e.FormattedValue.ToString());
                    endt = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[1].FormattedValue.ToString());


                    if (stdt >= endt)
                    {
                        MessageBox.Show("From Date Must be less than To Date");
                        e.Cancel = true;
                    }
                    else
                    {
                        var days = (endt - stdt).Days + 1;
                        dataGridView1.Rows[e.RowIndex].Cells[3].Value = days;
                    }


                }
                catch (Exception ee)
                {

                }
            }
            if (e.ColumnIndex == 1)
            {

                try
                {

                    stdt = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[0].FormattedValue.ToString());
                    endt = Convert.ToDateTime(e.FormattedValue.ToString());
                    if (stdt >= endt)
                    {
                        MessageBox.Show("From Date Must be less than To Date");
                        e.Cancel = true;
                    }
                    else
                    {
                        var days = (endt - stdt).Days + 1;
                        dataGridView1.Rows[e.RowIndex].Cells[3].Value = days;
                    }


                }
                catch (Exception ee)
                {

                }
            }



        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("from", typeof(DateTime));
            dt.Columns.Add("to", typeof(DateTime));
            dt.Columns.Add("roi", typeof(decimal));
            dt.Columns.Add("pnd", typeof(int));

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    dr[j] = row.Cells[j].Value;
                }

                dt.Rows.Add(dr);
            }

            dtRate = dt;
            DataRow[] result = dtRate.Select("roi=0");
            if (result.Length > 0 && (this.pEditMode == true || this.pAddMode == true))
            {
                MessageBox.Show("Empty Rate of Interest Not Allowed..", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                this.Close();
            }


        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("from", typeof(DateTime));
            dt.Columns.Add("to", typeof(DateTime));
            dt.Columns.Add("roi", typeof(decimal));
            dt.Columns.Add("pnd", typeof(int));

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    dr[j] = row.Cells[j].Value;
                }

                dt.Rows.Add(dr);
            }

            dtRate = dt;
            //DataRow[] result = dtRate.Select("roi=0");
            //if (result.Length > 0 && (this.pEditMode == true || this.pAddMode == true))
            //{
            //    MessageBox.Show("Empty Rate of Interest Not Allowed..", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //}
            //else
            //{
                this.Close();
            //}

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                try
                {
                    string tempVal = string.Format("{0:N2}", Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()));
                    decimal a = decimal.Parse(tempVal);
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = a;
                }
                catch (Exception ee)
                {

                }
            }

        }

        private void MenuItemClick(Object sender, System.EventArgs e)
        {
            var m = (MenuItem)sender;

            if (this.pEditMode == true || this.pAddMode == true)
            {
                if (m.Text.ToString() == "Add")
                {
                    DataRow drchild = childTbl.NewRow();                   
                    drchild["from"] = sta_dt;
                    drchild["to"] = end_dt;
                    drchild["roi"] = "0.00";
                    drchild["pnd"] = days;
                    childTbl.Rows.Add(drchild);
                    this.dataGridView1.Refresh();
                }
                else if (m.Text.ToString() == "Remove")
                {
                    int selectedIndex = dataGridView1.CurrentCell.RowIndex;
                    dataGridView1.Rows.RemoveAt(selectedIndex);
                }
            }
            else
            {

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
