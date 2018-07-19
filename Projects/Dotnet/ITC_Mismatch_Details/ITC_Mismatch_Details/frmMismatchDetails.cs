using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using System.Windows.Forms;
using udclsDGVDateTimePicker;
using vunettofx;
using ITC_Mismatch_DetailsProperties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ITC_Mismatch_Details
{
    public partial class frmMismatchDetails : Form
    {
        string file_path, file_name, extension, file_type, viewname;
        string dbname, s, auser, Product_code, Default_path, application_path, Company_startDate, Company_endDate, Companystate, Entrey_ty, Trans_cd;
        string iconpath; //Added by Priyanka B on 12/05/2017
        private string Excel03Con;
        private string Excel07Con;
        DataTable dtMismatchSupplier = new DataTable();
        DataTable dtMismatchReceiver = new DataTable();
        DataTable dtExcel = new DataTable();
        static DataTable dtEcxelData = new DataTable();
        static DataTable chkdt = new DataTable();
        DataTable dt = new DataTable();
        DataSet amenddt = new DataSet();
        string constring;
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        SqlDataReader dr;
        CheckBox checkboxHeader;
        DataGridViewCheckBoxColumn checkBoxColumn;
        DataGridViewCheckBoxCell chkCell;

        static DataTable dt11 = new DataTable();
        static DataTable getdate = new DataTable();

        int cn = 0;
        int cn1 = 0;
        int i = 0, selectCnt = 0;
        int rowindex = 0;
        string cboStatusValue, status_; //Added by Priyanka B on 15/05/2017
        bool chkAccept = false;  //Added by Priyanka B on 15/05/2017
        //PubClassProp objProperties = new PubClassProp();
        DataTable dd = new DataTable();
        DataTable tempdt = new DataTable();
        string ColumnName = "";
        int Xcor;
        int count1 = 0, chkcount;

        int ii, jj;
        string val, num, samt, csamt, rt, txval, camt, inv_typ, pos, idt, rchrg, inum, chksum, cfs, ctin, fp, gstin;
        public void load1()
        {
            bindGridSupplier();

            bindGridReceiver();

            accept();

            GridSupplierCheckbox();

            setdate();

            resize();

            countRecords();



            foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;

            }
            foreach (DataGridViewColumn dgvc in dataGridView3.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


            int ii;
            if (dataGridView1.Columns.Count > 15)
            {
                for (ii = dataGridView1.Columns.Count; ii <= dataGridView1.Columns.Count; ii++)
                {
                    dataGridView1.Columns.RemoveAt(ii - 1);
                }
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                foreach (DataGridViewRow row1 in dataGridView1.Rows)
                {
                    if (row1.Cells[0].Value.ToString() == "" || row1.Cells[2].Value.ToString() == "" || row1.Cells[3].Value.ToString() == "" || row1.Cells[5].Value.ToString() == "")
                    {
                        dataGridView1.Rows.Remove(row1);
                    }
                }

            }

            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Frozen = true;

            countRecords();

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataTable drec = new DataTable();
            drec.Columns.Add("Taxable value", typeof(decimal));
            drec.Columns.Add("IGST Rate", typeof(decimal));
            drec.Columns.Add("IGST Amount", typeof(decimal));
            drec.Columns.Add("CGST Rate", typeof(decimal));
            drec.Columns.Add("CGST Amount", typeof(decimal));
            drec.Columns.Add("SGST Rate", typeof(decimal));
            drec.Columns.Add("SGST Amount", typeof(decimal));


            if (e.RowIndex >= 0)//add Rupesh G. 24-10-2017 Bug no.30765
            {
                Xcor = dataGridView1.CurrentCellAddress.X;
                ColumnName = dataGridView1.Columns[Xcor].Name;
                status_ = dataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString();

                if (ColumnName == "Status")
                {
                    try
                    {
                        string val = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();


                        if (val == "Amend")
                        {


                            DialogResult dialogResult = MessageBox.Show("Amend your entry ?", "Miscmatch Reconcile", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                DataTable dttemp1 = new DataTable();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("Entrey_ty");
                                dt.Columns.Add("Trans_cd");
                                dt.Columns.Add("Trans_cd1");
                                string Entrey_ty = null;
                                string Trans_cd = null;
                                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

                                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                                string invno = Convert.ToString(selectedRow.Cells[2].Value).Trim();
                                string invdate = Convert.ToString(selectedRow.Cells[3].Value).Trim();
                                string hsncode = Convert.ToString(selectedRow.Cells[4].Value).Trim();
                                //Rupesh G. Comment 15 SEP 2017
                                //string sqlquery = "select entry_ty,Tran_cd,[Bill/Trans. No.] from " + viewname + "  where [Bill/Trans. No.]='" + invno + "'  and [HSN/SAC]='" + hsncode + "' ";
                                //Rupesh G. Add 15 SEP 2017
                                string sqlquery = "select entry_ty,Tran_cd,[Bill/Trans. No.] from " + viewname + "  where [Bill/Trans. No.]='" + invno + "' ";
                                cmd = new SqlCommand(sqlquery, con);
                                cmd.CommandType = CommandType.Text;
                                sda = new SqlDataAdapter(cmd);
                                sda.Fill(dttemp1);
                                foreach (DataRow dr in dttemp1.Rows)
                                {
                                    dt.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                                }
                                dttemp1.Clear();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    Entrey_ty = dr["Entrey_ty"].ToString();
                                    Trans_cd = dr["Trans_cd"].ToString();
                                }
                                //  MessageBox.Show("" + Entrey_ty);
                                //MessageBox.Show("" + Trans_cd);
                                getamend_records(Entrey_ty, Trans_cd);
                                vunettofx.ClNetToFx netToFx = new vunettofx.ClNetToFx();
                                try
                                {

                                    //netToFx.PrNetToFx(this.dbname, this.Company_startDate, this.Company_endDate, this.auser, this.Default_path, this.application_path, @"C:\VudyogGST\Bmp\Icon_VudyogGST.ico", this.Product_code, "DO ToUeVoucher WITH '" + Entrey_ty + "', " + Trans_cd + ",Thisform.DataSessionId,.T.");  //Commented by Priyanka B on 16/05/2017
                                    netToFx.PrNetToFx(this.dbname, this.Company_startDate, this.Company_endDate,
                                        this.auser, this.Default_path, this.application_path, this.iconpath,
                                        this.Product_code, "DO ToUeVoucher WITH '" + Entrey_ty + "', " + Trans_cd + ",Thisform.DataSessionId,.T.");  //Added by Priyanka B on 16/05/2017

                                }
                                catch (Exception et)
                                {
                                    MessageBox.Show(et.ToString());
                                }
                                //   
                                //   pageLoad();


                                // load1();

                                //edit last update
                                //if (Entrey_ty == "GC" || Entrey_ty == "C6")
                                //{
                                //    Entrey_ty = "CN";
                                //}
                                //if (Entrey_ty == "GD" || Entrey_ty == "D6")
                                //{
                                //    Entrey_ty = "DN";
                                //}
                                //     MessageBox.Show(selectedRow.Cells["status"].Value.ToString());

                                string taxval, irate, iamt, crate, camt, srate, samt;
                                taxval = Convert.ToDecimal(selectedRow.Cells["Taxable value"].Value).ToString("F");
                                irate = Convert.ToDecimal(selectedRow.Cells["IGST Rate"].Value).ToString("F");
                                iamt = Convert.ToDecimal(selectedRow.Cells["IGST Amount"].Value).ToString("F");
                                crate = Convert.ToDecimal(selectedRow.Cells["CGST Rate"].Value).ToString("F");
                                camt = Convert.ToDecimal(selectedRow.Cells["CGST Amount"].Value).ToString("F");
                                srate = Convert.ToDecimal(selectedRow.Cells["SGST Rate"].Value).ToString("F");
                                samt = Convert.ToDecimal(selectedRow.Cells["SGST Amount"].Value).ToString("F");

                                int count = 0;
                                RecGridFill();
                                foreach (DataGridViewRow dr in dataGridView3.Rows)
                                {
                                    drec.Rows.Add(Convert.ToDecimal(dr.Cells[2].Value).ToString("F"), Convert.ToDecimal(dr.Cells[3].Value).ToString("F"), Convert.ToDecimal(dr.Cells[4].Value).ToString("F"), Convert.ToDecimal(dr.Cells[5].Value).ToString("F"), Convert.ToDecimal(dr.Cells[6].Value).ToString("F"), Convert.ToDecimal(dr.Cells[7].Value).ToString("F"), Convert.ToDecimal(dr.Cells[8].Value).ToString("F"));
                                }
                                foreach (DataRow dr in drec.Rows)
                                {

                                    if (taxval != dr[0].ToString() || irate != dr[1].ToString() || iamt != dr[2].ToString() || crate != dr[3].ToString() || camt != dr[4].ToString() || srate != dr[5].ToString() || samt != dr[6].ToString())
                                    {
                                        count++;
                                    }

                                }
                                if (count == 0)
                                {


                                    selectedRow.Cells["status"].Value = "To Be Accept";
                                }
                                //for (int i = 0; i < dataGridView1.RowCount; i++)
                                //{
                                //    //Rupesh G. Comment 15 SEP 2017
                                //    //  if (dataGridView1.Rows[i].Cells[2].Value.ToString().Trim() == invno.Trim() && dataGridView1.Rows[i].Cells[4].Value.ToString().Trim() == hsncode.Trim() && dataGridView1.Rows[i].Cells[12].Value.ToString() == "To Be Accept")
                                //    if (dataGridView1.Rows[i].Cells[2].Value.ToString().Trim() == invno.Trim() && dataGridView1.Rows[i].Cells[12].Value.ToString() == "To Be Accept")
                                //    {


                                if (dataGridView1.Rows[selectedrowindex].Cells[2].Value.ToString().Trim() == invno.Trim() && dataGridView1.Rows[selectedrowindex].Cells[12].Value.ToString() == "To Be Accept")
                                {
                                    DataGridViewTextBoxCell a = new DataGridViewTextBoxCell();
                                    a.ToolTipText = "To Be Accept";
                                    dataGridView1.Rows[selectedrowindex].Cells[12] = a;

                                    dataGridView1.Rows[selectedrowindex].Cells[12].Value = "To Be Accept";
                                    dataGridView1.Rows[selectedrowindex].DefaultCellStyle.BackColor = Color.LightYellow;
                                    dataGridView1.Rows[selectedrowindex].Cells[14].Value = Convert.ToDateTime(dataGridView1.Rows[selectedrowindex].Cells[3].Value).ToString("dd/MM/yyyy");

                                    DataGridViewCell cell1 = dataGridView1.Rows[selectedrowindex].Cells[13];
                                    chkCell = cell1 as DataGridViewCheckBoxCell;
                                    chkCell.Value = false;
                                    //chkCell.FlatStyle = FlatStyle.Flat;
                                    //chkCell.Style.ForeColor = Color.White;//
                                    chkCell.FlatStyle = FlatStyle.Flat;
                                    chkCell.Style.BackColor = Color.White;
                                    cell1.ReadOnly = false;

                                    dataGridView1.Rows[selectedrowindex].Cells[14].ReadOnly = false;
                                    checkBox1.Enabled = true;
                                    button3.Enabled = true;
                                    //    MessageBox.Show(Entrey_ty);
                                    //  amend_data(Entrey_ty);//rupesh Comment stored data in amend table
                                    // selectedRow.Cells["status"].Value = "To Be Accept";


                                }

                                //}
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ww)
                    {
                        MessageBox.Show("" + ww.ToString());
                    }
                }
                else if (ColumnName == "Accept Date")
                {
                    // MessageBox.Show("hello");
                }
                else if (Xcor == 13)
                {
                    //string val = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    //MessageBox.Show(val.ToString());
                    //DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    //foreach (DataGridViewRow dr in dataGridView1.Rows)
                }
                else
                {
                    RecGridFill();
                }
                //dataGridView1.Columns[6].DefaultCellStyle.Format = "0.00"; //
                //dataGridView3.Columns[0].DefaultCellStyle.Format = "dd-MM-yyyy";

                dataGridView3.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView3.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView3.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView3.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView3.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView3.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView3.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                foreach (DataGridViewColumn dc in dataGridView3.Columns)
                {
                    dc.ReadOnly = true;
                }
                foreach (DataGridViewColumn dc in dataGridView1.Columns)
                {
                    if (dc.Name == "Status")
                        dc.ReadOnly = true;
                }
                dataGridView3.Columns[0].Width = 150;
                dataGridView3.Columns[1].Width = 150;
                dataGridView3.Columns[2].Width = 150;
                dataGridView3.Columns[3].Width = 150;
                dataGridView3.Columns[4].Width = 150;
                dataGridView3.Columns[5].Width = 150;
                dataGridView3.Columns[6].Width = 150;
                dataGridView3.Columns[7].Width = 150;
                dataGridView3.Columns[8].Width = 150;

                //Added by Priyanka B on 02/06/2017 Start
                RecGridFill();
                RecGridBackColor();

                //Added by Priyanka B on 02/06/2017 End

                //if (status_ == "Entry Not Found")
                //{
                //    tempdt.Rows.Clear();
                //    dataGridView3.DataSource = tempdt;
                //    dataGridView3.AllowUserToAddRows = false;
                //    dataGridView3.AutoGenerateColumns = false;
                //}
            }
            //dataGridView3.Rows[0].Cells[3].Value = "28.00";

        }

        //Added by Priyanka B on 02/06/2017 Start
        private void RecGridFill()
        {



            tempdt = new DataTable();

            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

                string invno = Convert.ToString(selectedRow.Cells[2].Value).Trim();
                string invdate = Convert.ToString(selectedRow.Cells[3].Value).Trim();
                string hsncode = Convert.ToString(selectedRow.Cells[4].Value).Trim();
                //Rupesh G. Comment 15 SEP 2017
                //  string sqlquery = "select [Bill/Trans. Date],[HSN/SAC],[Taxable Value] as 'Taxable value\n(as per line item)' ,[IGST Rate],[IGST Amount],[CGST Rate],[CGST Amount],[SGST Rate],[SGST Amount] from " + viewname + "  where [Bill/Trans. No.]='" + invno + "'  and [HSN/SAC]='" + hsncode + "' ";
                string sqlquery = "select [Bill/Trans. Date],[HSN/SAC],[Taxable Value] as 'Taxable value\n(as per line item)' ,[IGST Rate],[IGST Amount],[CGST Rate],[CGST Amount],[SGST Rate],[SGST Amount],LineRule,GSTRATE,GSTSTATE,state from " + viewname + "  where [Bill/Trans. No.]='" + invno + "'   ";
                cmd = new SqlCommand(sqlquery, con);
                cmd.CommandType = CommandType.Text;
                sda = new SqlDataAdapter(cmd);
                sda.Fill(tempdt);
                if (status_ == "Entry Not Found")

                {

                    tempdt.Rows.Clear();//
                }


                dataGridView3.DataSource = tempdt;
                dataGridView3.AllowUserToAddRows = false;
                dataGridView3.AutoGenerateColumns = false;
            }
            try
            {


                foreach (DataGridViewRow dr in dataGridView3.Rows)
                {
                    if (dr.Cells["LineRule"].Value.ToString().Trim() == "Exempted")
                    {

                        if (dr.Cells["GSTSTATE"].Value.ToString().Trim() != dr.Cells["state"].Value.ToString().Trim())
                        {
                            dr.Cells["IGST Rate"].Value = dr.Cells["GSTRATE"].Value.ToString();
                        }
                        else
                        {
                            dr.Cells["CGST Rate"].Value = decimal.Parse(dr.Cells["GSTRATE"].Value.ToString()) / 2;
                            dr.Cells["SGST Rate"].Value = decimal.Parse(dr.Cells["GSTRATE"].Value.ToString()) / 2;
                        }

                    }
                }
                dataGridView3.Columns["GSTSTATE"].Visible = false;
                dataGridView3.Columns["GSTRATE"].Visible = false;
                dataGridView3.Columns["state"].Visible = false;
            }
            catch (Exception e)
            {

            }

        }

        private void RecGridBackColor()
        {
            int selectedrowindex1 = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow1 = dataGridView1.Rows[selectedrowindex1];

            //   MessageBox.Show(selectedRow1.Cells[2].Value.ToString());
            foreach (DataGridViewRow dr in dataGridView3.Rows)
            {

                //MessageBox.Show((Convert.ToDecimal(dr.Cells[6].Value)).ToString());
                //MessageBox.Show(Convert.ToDecimal(selectedRow1.Cells[9].Value).ToString("F"));
                if (ColumnName != "Status" || ColumnName != "checkBoxColumn")
                {
                    if (Convert.ToDecimal(selectedRow1.Cells[5].Value).ToString("F") != Convert.ToDecimal(dr.Cells[2].Value).ToString("F"))
                    {
                        // MessageBox.Show(dr.Index.ToString());

                        //  dataGridView3.Columns[2].DefaultCellStyle.BackColor = Color.LightPink;

                        DataGridViewCell cell = dataGridView3.Rows[dr.Index].Cells[2];
                        cell.Style.BackColor = Color.LightPink;
                    }
                    if (Convert.ToDecimal(selectedRow1.Cells[6].Value).ToString("F") != Convert.ToDecimal(dr.Cells[3].Value).ToString("F"))
                    {
                        // dataGridView3.Columns[3].DefaultCellStyle.BackColor = Color.LightPink;
                        DataGridViewCell cell = dataGridView3.Rows[dr.Index].Cells[3];
                        cell.Style.BackColor = Color.LightPink;
                    }
                    if (Convert.ToDecimal(selectedRow1.Cells[7].Value).ToString("F") != Convert.ToDecimal(dr.Cells[4].Value).ToString("F"))
                    {
                        //   dataGridView3.Columns[4].DefaultCellStyle.BackColor = Color.LightPink;
                        DataGridViewCell cell = dataGridView3.Rows[dr.Index].Cells[4];
                        cell.Style.BackColor = Color.LightPink;
                    }
                    if (Convert.ToDecimal(selectedRow1.Cells[8].Value).ToString("F") != Convert.ToDecimal(dr.Cells[5].Value).ToString("F"))
                    {
                        // dataGridView3.Columns[5].DefaultCellStyle.BackColor = Color.LightPink;
                        DataGridViewCell cell = dataGridView3.Rows[dr.Index].Cells[5];
                        cell.Style.BackColor = Color.LightPink;
                    }
                    if (Convert.ToDecimal(selectedRow1.Cells[9].Value).ToString("F") != Convert.ToDecimal(dr.Cells[6].Value).ToString("F"))
                    {
                        // dataGridView3.Columns[6].DefaultCellStyle.BackColor = Color.LightPink;
                        DataGridViewCell cell = dataGridView3.Rows[dr.Index].Cells[6];
                        cell.Style.BackColor = Color.LightPink;
                    }
                    if (Convert.ToDecimal(selectedRow1.Cells[10].Value).ToString("F") != Convert.ToDecimal(dr.Cells[7].Value).ToString("F"))
                    {
                        //  dataGridView3.Columns[7].DefaultCellStyle.BackColor = Color.LightPink;
                        DataGridViewCell cell = dataGridView3.Rows[dr.Index].Cells[7];
                        cell.Style.BackColor = Color.LightPink;
                    }
                    if (Convert.ToDecimal(selectedRow1.Cells[11].Value).ToString("F") != Convert.ToDecimal(dr.Cells[8].Value).ToString("F"))
                    {
                        // dataGridView3.Columns[8].DefaultCellStyle.BackColor = Color.LightPink;
                        DataGridViewCell cell = dataGridView3.Rows[dr.Index].Cells[8];
                        cell.Style.BackColor = Color.LightPink;
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {




        }





        //Added by Priyanka B on 02/06/2017 End

        public frmMismatchDetails()
        {
            InitializeComponent();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                {
                    //  comboBox1.SelectedItem = "All";

                    label3.Text = "Unselect All";
                    //  dt = new DataTable();
                    //  dt.Columns.Add("Entrey_ty");
                    //  dt.Columns.Add("Trans_cd");
                    //  dt.Columns.Add("true/false");
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[12].Value.ToString().Trim() != "To Be Accept")
                        {
                            DataGridViewCell cell = dataGridView1.Rows[i].Cells[13];
                            chkCell = cell as DataGridViewCheckBoxCell;
                            chkCell.Value = false;
                            chkCell.FlatStyle = FlatStyle.Flat;
                            chkCell.Style.ForeColor = Color.DarkGray;
                            cell.ReadOnly = true;

                            //   dataGridView3.Rows[i].Cells[8].Value = false;
                        }
                        else
                        {

                            dataGridView1.Rows[i].Cells[13].Value = true;
                            //  dt.Rows.Add(dataGridView3.Rows[i].Cells[0].Value.ToString(), dataGridView3.Rows[i].Cells[1].Value.ToString(), dataGridView3.Rows[i].Cells[8].Value.ToString());

                        }
                    }

                }
                else
                {
                    label3.Text = "Select All";
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[12].Value.ToString().Trim() == "To Be Accept")
                        {
                            DataGridViewCell cell = dataGridView1.Rows[i].Cells[13];
                            chkCell = cell as DataGridViewCheckBoxCell;
                            chkCell.Value = false;
                            chkCell.FlatStyle = FlatStyle.Flat;
                            chkCell.Style.ForeColor = Color.DarkGray;
                            cell.ReadOnly = false;

                            //   dataGridView3.Rows[i].Cells[8].Value = false;
                        }

                    }

                    //  load1();
                    //   pageLoad();
                    //setdate();

                }
                //  search_status();rrg comment

                //   SetAcceptVisibility();  //Commented by Priyanka B on 17/05/2017 Visibility
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                MessageBox.Show(ex.Message.ToString() + Environment.NewLine + ex.Source.ToString());
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            string accept_date = null;
            DataTable dtaccept = new DataTable();
            DataTable dt = new DataTable();
            dt.Columns.Add("Entrey_ty");
            dt.Columns.Add("Trans_cd");
            dt.Columns.Add("Trans_cd1");
            dt.Columns.Add("date");
            int i = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[13];
                if ((Boolean)chk.Value == true)
                {

                    string invno = dataGridView1.Rows[i].Cells[2].Value.ToString().Trim();
                    string invdate = dataGridView1.Rows[i].Cells[3].Value.ToString().Trim();
                    string hsn = dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                    accept_date = dataGridView1.Rows[i].Cells[14].Value.ToString();
                    string sqlquery = "select entry_ty,Tran_cd,[Bill/Trans. No.] from " + viewname + "  where [Bill/Trans. No.]='" + invno + "'  ";
                    cmd = new SqlCommand(sqlquery, con);
                    cmd.CommandType = CommandType.Text;
                    sda = new SqlDataAdapter(cmd);
                    sda.Fill(dtaccept);
                    foreach (DataRow dr in dtaccept.Rows)
                    {
                        dt.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), accept_date);

                    }
                    dtaccept.Clear();


                }
                else
                {
                    // MessageBox.Show("false");
                }
                i++;
            }

            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string Entrey_ty = dr["Entrey_ty"].ToString();
                    string Trans_cd = dr["Trans_cd"].ToString();
                    string date = dr["date"].ToString();
                    if (Entrey_ty == "S1")
                    {

                        cmd = new SqlCommand("update SBMAIN set transtatus = '1', u_cldt=Convert(datetime,'" + date + "', 103) where Entry_ty='S1' and Tran_cd= '" + Trans_cd + "'", con);
                    }
                    if (Entrey_ty == "ST")
                    {
                        cmd = new SqlCommand("update STMAIN set transtatus = '1',u_cldt=Convert(datetime,'" + date + "', 103) where Entry_ty='ST' and Tran_cd= '" + Trans_cd + "'", con);

                    }
                    if (Entrey_ty == "E1")
                    {
                        cmd = new SqlCommand("update EPMAIN set transtatus = '1',u_cldt=Convert(datetime,'" + date + "', 103) where Entry_ty='E1' and Tran_cd= '" + Trans_cd + "'", con);

                    }
                    if (Entrey_ty == "PT" || Entrey_ty == "P1")
                    {
                        cmd = new SqlCommand("update PTMAIN set transtatus = '1',u_cldt=Convert(datetime,'" + date + "', 103) where Entry_ty='" + Entrey_ty + "' and Tran_cd= '" + Trans_cd + "'", con);
                    }
                    if (Entrey_ty == "GC" || Entrey_ty == "C6")
                    {
                        cmd = new SqlCommand("update CNMAIN set transtatus = '1' ,u_cldt=Convert(datetime,'" + date + "', 103)  where Entry_ty='" + Entrey_ty + "' and Tran_cd= '" + Trans_cd + "'", con);
                    }

                    //RRG NEW 
                    if (Entrey_ty == "GD" || Entrey_ty == "D6")
                    {
                        cmd = new SqlCommand("update DNMAIN set transtatus = '1' ,u_cldt=Convert(datetime,'" + date + "', 103)  where Entry_ty='" + Entrey_ty + "' and Tran_cd= '" + Trans_cd + "'", con);
                    }
                    if (Entrey_ty == "PR")
                    {
                        cmd = new SqlCommand("update PRMAIN set transtatus = '1' ,u_cldt=Convert(datetime,'" + date + "', 103)  where Entry_ty='" + Entrey_ty + "' and Tran_cd= '" + Trans_cd + "'", con);
                    }
                    if (Entrey_ty == "SR")
                    {
                        cmd = new SqlCommand("update SRMAIN set transtatus = '1' ,u_cldt=Convert(datetime,'" + date + "', 103)  where Entry_ty='" + Entrey_ty + "' and Tran_cd= '" + Trans_cd + "'", con);
                    }
                    if (Entrey_ty == "J6")
                    {
                        cmd = new SqlCommand("update JVMAIN set transtatus = '1' ,u_cldt=Convert(datetime,'" + date + "', 103)  where Entry_ty='" + Entrey_ty + "' and Tran_cd= '" + Trans_cd + "'", con);
                    }
                    //RRG END

                    con.Open();

                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception dd)
            {
                MessageBox.Show(dd.ToString());
            }
            ////chk code
            if (checkBox1.Checked)
            {


                string searchValue = "To Be Accept";



                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


                try
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[12].Value.ToString().Equals(searchValue))
                        {
                            row.Selected = true;
                        }
                        else
                        {
                            row.Selected = false;
                            //    row.Visible = false;
                        }

                    }
                }
                catch (Exception exc)
                {

                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Selected == true)
                    {
                        try
                        {
                            int selectedrowindex = row.Index;
                            //  int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                            dataGridView1.Rows[selectedrowindex].Cells[12].Value = "Accepted";
                            dataGridView1.Rows[selectedrowindex].DefaultCellStyle.BackColor = Color.LightGreen;
                            //accept();
                            if (dataGridView1.Rows[selectedrowindex].Cells[12].Value.ToString() != "To Be Accept")
                            {
                                DataGridViewCell cell1 = dataGridView1.Rows[selectedrowindex].Cells[13];

                                chkCell = cell1 as DataGridViewCheckBoxCell;
                                chkCell.Value = false;
                                chkCell.FlatStyle = FlatStyle.Flat;
                                chkCell.Style.BackColor = Color.DarkGray;

                                cell1.ReadOnly = true;
                                dataGridView1.Rows[selectedrowindex].Cells[14].ReadOnly = true;
                            }
                            else
                            {
                                DataGridViewCell cell1 = dataGridView1.Rows[selectedrowindex].Cells[13];

                                chkCell = cell1 as DataGridViewCheckBoxCell;
                                chkCell.Value = false;
                                //chkCell.FlatStyle = FlatStyle.Flat;
                                //chkCell.Style.ForeColor = Color.White;//
                                chkCell.FlatStyle = FlatStyle.Flat;
                                chkCell.Style.BackColor = Color.White;
                                cell1.ReadOnly = false;

                            }
                            //GridSupplierCheckbox();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        row.Selected = false;
                    }
                }
                checkBox1.Enabled = false;
                button3.Enabled = false;
                checkBox1.Checked = false;
                label3.Text = "Select All";


            }
            else
            {
                try
                {

                    int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                    dataGridView1.Rows[selectedrowindex].Cells[12].Value = "Accepted";
                    dataGridView1.Rows[selectedrowindex].DefaultCellStyle.BackColor = Color.LightGreen;
                    //accept();
                    if (dataGridView1.Rows[selectedrowindex].Cells[12].Value.ToString() != "To Be Accept")
                    {
                        DataGridViewCell cell1 = dataGridView1.Rows[selectedrowindex].Cells[13];

                        chkCell = cell1 as DataGridViewCheckBoxCell;
                        chkCell.Value = false;
                        chkCell.FlatStyle = FlatStyle.Flat;
                        chkCell.Style.BackColor = Color.DarkGray;

                        cell1.ReadOnly = true;
                        dataGridView1.Rows[selectedrowindex].Cells[14].ReadOnly = true;


                    }
                    else
                    {
                        DataGridViewCell cell1 = dataGridView1.Rows[selectedrowindex].Cells[13];

                        chkCell = cell1 as DataGridViewCheckBoxCell;
                        chkCell.Value = false;
                        //chkCell.FlatStyle = FlatStyle.Flat;
                        //chkCell.Style.ForeColor = Color.White;//
                        chkCell.FlatStyle = FlatStyle.Flat;
                        chkCell.Style.BackColor = Color.White;
                        cell1.ReadOnly = false;

                    }
                    //GridSupplierCheckbox();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                // row.Selected = false;
            }
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
        }
        public void getRecordsToJSON(string filepath)
        {

            try
            {

                using (StreamReader r = new StreamReader(filepath))
                {
                    string json = r.ReadToEnd();

                    demo1(json);
                    //List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
                }
            }
            catch (Exception eeee)
            {
                MessageBox.Show(eeee.ToString());
            }
        }
        public void demo1(string json1)
        {
            try
            {
                dt.Columns.Clear();
                dt.Rows.Clear();
                //dtEcxelData.Columns.Clear();
                dtEcxelData.Rows.Clear();

                dt.Columns.Add("b2b__inv__val");
                dt.Columns.Add("b2b__inv__itms__num");
                dt.Columns.Add("b2b__inv__itms__itm_det__samt");
                dt.Columns.Add("b2b__inv__itms__itm_det__csamt");
                dt.Columns.Add("b2b__inv__itms__itm_det__rt");
                dt.Columns.Add("b2b__inv__itms__itm_det__txval");
                dt.Columns.Add("b2b__inv__itms__itm_det__camt");
                dt.Columns.Add("b2b__inv__inv_typ");
                dt.Columns.Add("b2b__inv__pos");
                dt.Columns.Add("b2b__inv__idt");
                dt.Columns.Add("b2b__inv__rchrg");
                dt.Columns.Add("b2b__inv__inum");
                dt.Columns.Add("b2b__inv__chksum");
                dt.Columns.Add("b2b__cfs");
                dt.Columns.Add("b2b__ctin");
                dt.Columns.Add("fp");
                dt.Columns.Add("gstin");

                //var json = @"{""b2b"":[{""inv"":[{""val"":337079,""itms"":[{""num"":1,""itm_det"":{""samt"":25709.4,""rt"":18,""txval"":285660,""camt"":25709.4}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""11-07-2017"",""rchrg"":""N"",""inum"":""MSL/17-18/193"",""chksum"":""dbb1b75a829b7f92d8a97930f6a18424d81bf18ab97a3e6b82b509afbc8cd341""},{""val"":242632,""itms"":[{""num"":1,""itm_det"":{""samt"":18505.8,""rt"":18,""txval"":205620,""camt"":18505.8}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""05-07-2017"",""rchrg"":""N"",""inum"":""MSL/17-18/81"",""chksum"":""b66164d755dd0f9631419b9735d67e0f0301e4c2216a2f1cb7ab68c687914289""}],""cfs"":""N"",""ctin"":""20AABCM4621M1ZR""},{""inv"":[{""val"":13160,""itms"":[{""num"":1,""itm_det"":{""samt"":1439.37,""rt"":28,""txval"":10281.24,""camt"":1439.37}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""11-07-2017"",""rchrg"":""N"",""inum"":""3707IN200000017"",""chksum"":""ef9c052b261a13fc602dc95427af64af39b01b04e08476a3cd3f05fe6fc206d4""}],""cfs"":""Y"",""ctin"":""20AAACC2659Q1ZK""},{""inv"":[{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""02-07-2017"",""rchrg"":""N"",""inum"":""N237200100000046"",""chksum"":""2e4c34a1845bd04557b804622d229d34cb848c1a1bd9357d1889bb49b79c0c89""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""05-07-2017"",""rchrg"":""N"",""inum"":""N237200100000381"",""chksum"":""d2b9a36436fef31b1838a03a84ec466f8f209dd1207cefe0d34daa80d727b1d3""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""06-07-2017"",""rchrg"":""N"",""inum"":""N237200100000476"",""chksum"":""eeb279cde65c19a2504e1af709846d967822c5011d98ba5c23822e3f67ab6651""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""07-07-2017"",""rchrg"":""N"",""inum"":""N237200100000659"",""chksum"":""05bc4ae7b1991e6202cc05ed0d699a00681ad0a61fca2290ec4d5ab395423a2a""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""07-07-2017"",""rchrg"":""N"",""inum"":""N237200100000689"",""chksum"":""fad1acf83894f1ce91ebc8e91c775f8299efaf0c9020e5af468a33f1b4a768a3""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""08-07-2017"",""rchrg"":""N"",""inum"":""N237200100000807"",""chksum"":""2d051d98966bb12491f8ac9f00d32ed1b5977c286b37c5e738f20433a25c6656""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""08-07-2017"",""rchrg"":""N"",""inum"":""N237200100000848"",""chksum"":""1fd3f3ee0626ddd053f171e894a5e6b617eec13d47b6f2f1086be671e484a9da""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""08-07-2017"",""rchrg"":""N"",""inum"":""N237200100000860"",""chksum"":""b1c8525755c09de9cf2e2a916de89af2ae5f6ac06e24f53a7e991566e6fc58fc""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""09-07-2017"",""rchrg"":""N"",""inum"":""N237200100001005"",""chksum"":""2b940c77aece490800d25ea74fc19cd2b2605ba1212fc2c49f998edfa16d2f38""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""10-07-2017"",""rchrg"":""N"",""inum"":""N237200100001032"",""chksum"":""68e3636e8c9352282e0c759aa5372f0bfb0b3e31afb002e2d858938ceca27ff4""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""11-07-2017"",""rchrg"":""N"",""inum"":""N237200100001196"",""chksum"":""6a5cc2105ad74a0e92008edf8b73a4b5efc946bd63e23c16ba999924e8494416""},{""val"":166320,""itms"":[{""num"":1,""itm_det"":{""samt"":18191.32,""csamt"":0,""rt"":28,""txval"":129938,""camt"":18191.32}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""13-07-2017"",""rchrg"":""N"",""inum"":""N237200100001457"",""chksum"":""d1541967c2690f46a815cceb81de982d18b5a1e07e639483229756ec13546f78""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""13-07-2017"",""rchrg"":""N"",""inum"":""N237200100001467"",""chksum"":""dfd10b35bef553930827247ac82d192ac3c21892f0fa57614e82036ef3a0e57e""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""13-07-2017"",""rchrg"":""N"",""inum"":""N237200100001481"",""chksum"":""3a5c10ebe12eee2bf61ef3c31d0027f4672f57938136d3b9dc904458594b1f9e""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""13-07-2017"",""rchrg"":""N"",""inum"":""N237200100001485"",""chksum"":""b90cc008f346afb269239e6d19a32d5227053c6d1d6c33b7e6e0cc199e61c040""},{""val"":154980,""itms"":[{""num"":1,""itm_det"":{""samt"":16950.92,""csamt"":0,""rt"":28,""txval"":121078,""camt"":16950.92}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""14-07-2017"",""rchrg"":""N"",""inum"":""N237200100001553"",""chksum"":""f4f792a2c57c6afdd0132879c177b677d4f88fa05dbf05dc1bd0b8ac843009aa""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""15-07-2017"",""rchrg"":""N"",""inum"":""N237200100001635"",""chksum"":""01cd84ff43992fb8790c6c4a70b39c98b1738f02886ecfeadb7af8d619300f41""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""15-07-2017"",""rchrg"":""N"",""inum"":""N237200100001638"",""chksum"":""d475789c6b6fb022c183d5bad9bd7df946654eb24d293851c1e969feb22c03c5""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""15-07-2017"",""rchrg"":""N"",""inum"":""N237200100001643"",""chksum"":""93581d9ea750aea98944f087ee4ecf72bd9ef0aaf230d5515074d1e889429847""},{""val"":166320,""itms"":[{""num"":1,""itm_det"":{""samt"":18191.32,""csamt"":0,""rt"":28,""txval"":129938,""camt"":18191.32}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""16-07-2017"",""rchrg"":""N"",""inum"":""N237200100001821"",""chksum"":""00955a606ed7e28ab84c0f22a2ebc48a04565e38608f2a982deb855f639a5025""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""16-07-2017"",""rchrg"":""N"",""inum"":""N237200100001822"",""chksum"":""d1a2ff4852f83778a799d01a4bc12d9756c0352fd6524edd0459400c15559dad""},{""val"":166320,""itms"":[{""num"":1,""itm_det"":{""samt"":18191.32,""csamt"":0,""rt"":28,""txval"":129938,""camt"":18191.32}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""16-07-2017"",""rchrg"":""N"",""inum"":""N237200100001880"",""chksum"":""85679995f420b3f94907e7649f9a6b1a92ec7854a616bdd5b4ac4cbb9a9ec43b""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""16-07-2017"",""rchrg"":""N"",""inum"":""N237200100001897"",""chksum"":""3df7c13f1616079e341b210aec50aa6232955522cd73c01664443f846214421c""},{""val"":166320,""itms"":[{""num"":1,""itm_det"":{""samt"":18191.32,""csamt"":0,""rt"":28,""txval"":129938,""camt"":18191.32}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""17-07-2017"",""rchrg"":""N"",""inum"":""N237200100001939"",""chksum"":""9ffa1b1b44d11748277d889c3b79aac15101cd8bc5ce22548eb3aa34db3cd897""},{""val"":166320,""itms"":[{""num"":1,""itm_det"":{""samt"":18191.32,""csamt"":0,""rt"":28,""txval"":129938,""camt"":18191.32}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""17-07-2017"",""rchrg"":""N"",""inum"":""N237200100002088"",""chksum"":""7bcb4731d86c8cebb99e57e8c0c1df590faa36f6dd3355f6fcb119c2c82712dc""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""18-07-2017"",""rchrg"":""N"",""inum"":""N237200100002138"",""chksum"":""5132ee97df2943621f848332de606007377814188f5f42907ad1b53462ab4712""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""18-07-2017"",""rchrg"":""N"",""inum"":""N237200100002144"",""chksum"":""4240779ad9e2a0acefaf0540f891b46ce2629f4fbefc4d4985b64b26b662f1bf""},{""val"":117120,""itms"":[{""num"":1,""itm_det"":{""samt"":12810,""csamt"":0,""rt"":28,""txval"":91500,""camt"":12810}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""19-07-2017"",""rchrg"":""N"",""inum"":""N237200100002233"",""chksum"":""d657caed88ae726393e53a92a17f602616d70968a7a6b4f56b290f8cdc171012""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""19-07-2017"",""rchrg"":""N"",""inum"":""N237200100002337"",""chksum"":""8fb925c3bf5fddfe4220ef193206d1c648e85369bfe4891a76fe5403d89573f5""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""20-07-2017"",""rchrg"":""N"",""inum"":""N237200100002363"",""chksum"":""bdbea39119f6ffc48b35b639498822e86579320249deb2674c83e36a22039411""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""20-07-2017"",""rchrg"":""N"",""inum"":""N237200100002424"",""chksum"":""a1985a3634cbb756aee8be82113c96d22862c69f143f34692c07c83d4ebbdd37""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""20-07-2017"",""rchrg"":""N"",""inum"":""N237200100002463"",""chksum"":""e6c6a2125afc925be05145f0301d867626514e8ef87a902baa70682b2895efec""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""20-07-2017"",""rchrg"":""N"",""inum"":""N237200100002471"",""chksum"":""a308c1c3ded6770713f5570121b92966a8a581735e4182012b72f8b0e33dfe2f""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""20-07-2017"",""rchrg"":""N"",""inum"":""N237200100002488"",""chksum"":""78a02f2291679ebe7a3537a75d69008a1655cddd608f7fa909b67dec9c53a470""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""20-07-2017"",""rchrg"":""N"",""inum"":""N237200100002501"",""chksum"":""aef948056a4a0fba7b51b4f09b561fadc149e1515bdc40f6ccbd98464659d3e2""},{""val"":166320,""itms"":[{""num"":1,""itm_det"":{""samt"":18191.32,""csamt"":0,""rt"":28,""txval"":129938,""camt"":18191.32}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""20-07-2017"",""rchrg"":""N"",""inum"":""N237200100002505"",""chksum"":""9f673e7eb5cb3254210ee194049a00165114adbd4667fae81274cdb702ed62a6""},{""val"":166320,""itms"":[{""num"":1,""itm_det"":{""samt"":18191.32,""csamt"":0,""rt"":28,""txval"":129938,""camt"":18191.32}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""20-07-2017"",""rchrg"":""N"",""inum"":""N237200100002526"",""chksum"":""33a552d3dfe777e669d23414116d5c62252d4b14e3f22ecdb9687b20653c4197""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""22-07-2017"",""rchrg"":""N"",""inum"":""N237200100002718"",""chksum"":""70dd482a5bf15b9842e020b5739155eeaa09a257a8c675b1b3b5225714f154c5""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""22-07-2017"",""rchrg"":""N"",""inum"":""N237200100002719"",""chksum"":""cec96d73441ff6e023c96a234f5704b66c5149181fd516784f773d6630897615""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""22-07-2017"",""rchrg"":""N"",""inum"":""N237200100002739"",""chksum"":""23f3d484d584cb1f430d87fcb8a6cdc5dfe309c7a608bd991efd22df63f21b46""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""23-07-2017"",""rchrg"":""N"",""inum"":""N237200100002843"",""chksum"":""2b59ed0686e2e8825c4e88ada11524a5b031925623a6d4b3693b44a9f6e049b0""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""26-07-2017"",""rchrg"":""N"",""inum"":""N237200100002954"",""chksum"":""74ee546ed3d1a76b6559cc96c3e450c5eb9825bb2b4fa5a4f36e60aa9cd5301d""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""26-07-2017"",""rchrg"":""N"",""inum"":""N237200100002956"",""chksum"":""36a1be1bb6a2be88e2ebc08708cf2c7add9ffc46bbab708f2b982d93c58089d5""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""26-07-2017"",""rchrg"":""N"",""inum"":""N237200100003010"",""chksum"":""f6e1126bfdd2ceac549a51badd06dc15f9565d18d025b496312e94417d0b6bef""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""27-07-2017"",""rchrg"":""N"",""inum"":""N237200100003068"",""chksum"":""0c55ae3dcdc214eb64bba4aae4992e58fca41975d8edad51daed038c7a742952""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""27-07-2017"",""rchrg"":""N"",""inum"":""N237200100003159"",""chksum"":""8e14dc06938f647cfbe18992e9fab936cc3e594fb889e59892f06a250e71f10c""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""28-07-2017"",""rchrg"":""N"",""inum"":""N237200100003175"",""chksum"":""13fa7566bb3eb3b71a83b69ea9e58c0af58f521f8dfda97301a899416d2c2be0""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""28-07-2017"",""rchrg"":""N"",""inum"":""N237200100003192"",""chksum"":""83599fa21d8422e16122da26cc0acfcd484bf3fe5635c57972f3682bb63467a4""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""29-07-2017"",""rchrg"":""N"",""inum"":""N237200100003289"",""chksum"":""be5fd5b68d0975e5e2e393f8db4c357be43aae34fae4322af1dad7b1c76a4100""},{""val"":153720,""itms"":[{""num"":1,""itm_det"":{""samt"":16813.16,""csamt"":0,""rt"":28,""txval"":120094,""camt"":16813.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""30-07-2017"",""rchrg"":""N"",""inum"":""N237200100003460"",""chksum"":""fb1100cedc16341fbe609bf2514fa8cc90c2ab5f4fcdc3d3aff5ca3e9b12f6c7""},{""val"":166320,""itms"":[{""num"":1,""itm_det"":{""samt"":18191.32,""csamt"":0,""rt"":28,""txval"":129938,""camt"":18191.32}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""31-07-2017"",""rchrg"":""N"",""inum"":""N237200100003703"",""chksum"":""456a82c5cc90daccb2c4cc6f5453ad39a450fee8263c43e7152b9a33d80fb89f""}],""cfs"":""N"",""ctin"":""20AAACT1507C1ZB""},{""inv"":[{""val"":195000,""itms"":[{""num"":1,""itm_det"":{""samt"":21328.12,""csamt"":0,""rt"":28,""txval"":152343.74,""camt"":21328.12}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""08-07-2017"",""rchrg"":""N"",""inum"":""8216103683"",""chksum"":""2ae107fee563f2db4ad1a7df3dc61b4cc96842313079466c9685ad9a5406b43b""},{""val"":194400,""itms"":[{""num"":1,""itm_det"":{""samt"":21262.5,""csamt"":0,""rt"":28,""txval"":151875,""camt"":21262.5}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""25-07-2017"",""rchrg"":""N"",""inum"":""8216104230"",""chksum"":""94be3698a747374e54c69776c0211772d1e59d7b2bfcb8b67fbab65bd3420190""},{""val"":32900,""itms"":[{""num"":1,""itm_det"":{""samt"":3598.44,""csamt"":0,""rt"":28,""txval"":25703.12,""camt"":3598.44}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""05-07-2017"",""rchrg"":""N"",""inum"":""8219101174"",""chksum"":""28f6e4a5aceb2dcadd766590b551679be5ecbe436d150de975250d2aa82bfbad""},{""val"":35100,""itms"":[{""num"":1,""itm_det"":{""samt"":3839.06,""csamt"":0,""rt"":28,""txval"":27421.88,""camt"":3839.06}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""05-07-2017"",""rchrg"":""N"",""inum"":""8219101175"",""chksum"":""0a38c5493f519f56682df98e172f10f309f62e455618ecb0dbd48ae393274062""},{""val"":33100,""itms"":[{""num"":1,""itm_det"":{""samt"":3620.31,""csamt"":0,""rt"":28,""txval"":25859.38,""camt"":3620.31}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""17-07-2017"",""rchrg"":""N"",""inum"":""8219101691"",""chksum"":""e892d08d2f8a565a345e4281d605254d280ab23c3ab855a727c7f2dfd94595dd""},{""val"":70600,""itms"":[{""num"":1,""itm_det"":{""samt"":7721.87,""csamt"":0,""rt"":28,""txval"":55156.24,""camt"":7721.87}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""17-07-2017"",""rchrg"":""N"",""inum"":""8219101705"",""chksum"":""371a1adf0875ed8e46169bae463b8a93d2dd14575875afec556c7b703b7ea085""},{""val"":70600,""itms"":[{""num"":1,""itm_det"":{""samt"":7721.87,""csamt"":0,""rt"":28,""txval"":55156.24,""camt"":7721.87}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""18-07-2017"",""rchrg"":""N"",""inum"":""8219101779"",""chksum"":""11569dc1a3ba581a959b4c584b55893da23141ad0b7a169af87fec6fd5cbd750""},{""val"":178000,""itms"":[{""num"":1,""itm_det"":{""samt"":19468.75,""csamt"":0,""rt"":28,""txval"":139062.5,""camt"":19468.75}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""16-07-2017"",""rchrg"":""N"",""inum"":""8979000054"",""chksum"":""dc81b93b8bccee86f7efc50d2302a150ed27bf7c060bac59d04857de00e4fc37""}],""cfs"":""Y"",""ctin"":""20AAACL6442L1ZO""},{""inv"":[{""val"":133560.32,""itms"":[{""num"":1,""itm_det"":{""samt"":14608.16,""rt"":28,""txval"":104344,""camt"":14608.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""22-07-2017"",""rchrg"":""N"",""inum"":""S6J/17/P/18"",""chksum"":""3248a6b85290a20a5aa4c60fd7b1ddbd512be60a5323b9a59d2f5e77081782d9""},{""val"":127200,""itms"":[{""num"":1,""itm_det"":{""samt"":13912.5,""rt"":28,""txval"":99375,""camt"":13912.5}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""15-07-2017"",""rchrg"":""N"",""inum"":""S6J/17/P/4"",""chksum"":""477ad875c449ca36f6271ef64ebfc91394f2c1944652677fb3e0dc5d45802943""}],""cfs"":""Y"",""ctin"":""20AACCS8796G1Z5""},{""inv"":[{""val"":947211.1,""itms"":[{""num"":1,""itm_det"":{""samt"":72244.91,""rt"":18,""txval"":802721.26,""camt"":72244.91}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""06-07-2017"",""rchrg"":""N"",""inum"":""SI/JS1/18/000037"",""chksum"":""154caf784929d0628395d427881a9131cdabb1dd70cbc8240a38d36c843d4b07""},{""val"":994689.64,""itms"":[{""num"":1,""itm_det"":{""samt"":75866.16,""rt"":18,""txval"":842957.34,""camt"":75866.16}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""13-07-2017"",""rchrg"":""N"",""inum"":""SI/JS1/18/000149"",""chksum"":""7e0bb48827e18cb31e5e978f6454d69245a05bc205f930b1b78456df5557768f""},{""val"":1069293.4,""itms"":[{""num"":1,""itm_det"":{""samt"":81556.28,""rt"":18,""txval"":906180.86,""camt"":81556.28}}],""inv_typ"":""R"",""pos"":""20"",""idt"":""17-07-2017"",""rchrg"":""N"",""inum"":""SI/JS1/18/000220"",""chksum"":""e8116b274cf86105aa3d3823bf63d7e730b3861338f67aefb3f85fff90ee5195""}],""cfs"":""Y"",""ctin"":""20AAHCP6557H1ZE""}],""fp"":""072017"",""cdn"":[],""gstin"":""20AIMPS1513Q1Z1""}";
                //  var json = @"{""b2b"":[],""fp"":""072017"",""cdn"":[],""gstin"":""20AAXFM1088M1ZS""}";
                var json = json1;
                JToken token = JToken.Parse(json);

                int a = token.SelectToken("b2b").Count();


                for (ii = 0; ii < a; ii++)
                {
                    jj = 0;

                    Newtonsoft.Json.Linq.JArray men = (Newtonsoft.Json.Linq.JArray)token.SelectToken("b2b[" + ii + "].inv");



                    foreach (Newtonsoft.Json.Linq.JToken m in men)
                    {


                        try
                        {
                            val = m["val"].ToString();
                        }
                        catch (Exception ee)
                        {
                            val = "";
                        }



                        Newtonsoft.Json.Linq.JArray men1 = (Newtonsoft.Json.Linq.JArray)token.SelectToken("b2b[" + ii + "].inv[" + jj + "].itms");
                        foreach (Newtonsoft.Json.Linq.JToken m1 in men1)
                        {
                            // MessageBox.Show(m1["num"].ToString());

                            try
                            {
                                num = m1["num"].ToString();
                            }
                            catch (Exception ee)
                            {
                                num = "";
                            }

                            //MessageBox.Show(m1["itm_det"]["samt"].ToString());

                            try
                            {
                                samt = m1["itm_det"]["samt"].ToString();
                            }
                            catch (Exception ee)
                            {
                                samt = "0";
                            }

                            // MessageBox.Show(m1["itm_det"]["csamt"].ToString());
                            try
                            {
                                csamt = m1["itm_det"]["csamt"].ToString();
                            }
                            catch (Exception eee)
                            {
                                csamt = "0";
                            }
                            //   MessageBox.Show(m1["itm_det"]["rt"].ToString());
                            try
                            {
                                rt = m1["itm_det"]["rt"].ToString();
                            }
                            catch (Exception eee)
                            {
                                rt = "0";
                            }

                            // MessageBox.Show(m1["itm_det"]["txval"].ToString());
                            try
                            {
                                txval = m1["itm_det"]["txval"].ToString();
                            }
                            catch (Exception eee)
                            {
                                txval = "0";
                            }
                            //   MessageBox.Show(m1["itm_det"]["camt"].ToString());
                            try
                            {
                                camt = m1["itm_det"]["camt"].ToString();
                            }
                            catch (Exception eee)
                            {
                                camt = "0";
                            }

                        }

                        // MessageBox.Show(m["inv_typ"].ToString());
                        try
                        {
                            inv_typ = m["inv_typ"].ToString();
                        }
                        catch (Exception eee)
                        {
                            inv_typ = "";
                        }
                        // MessageBox.Show(m["pos"].ToString());
                        try
                        {
                            pos = m["pos"].ToString();
                        }
                        catch (Exception eee)
                        {
                            pos = "";
                        }
                        //  MessageBox.Show(m["idt"].ToString());
                        try
                        {
                            idt = m["idt"].ToString();
                        }
                        catch (Exception eee)
                        {
                            idt = "";
                        }
                        //  MessageBox.Show(m["rchrg"].ToString());
                        try
                        {
                            rchrg = m["rchrg"].ToString();
                        }
                        catch (Exception eee)
                        {
                            rchrg = "";
                        }
                        //  MessageBox.Show(m["inum"].ToString());
                        try
                        {
                            inum = m["inum"].ToString();
                        }
                        catch (Exception eee)
                        {
                            inum = "";
                        }
                        //  MessageBox.Show(m["chksum"].ToString());
                        try
                        {
                            chksum = m["chksum"].ToString();
                        }
                        catch (Exception eee)
                        {
                            chksum = "";
                        }

                        Newtonsoft.Json.Linq.JObject o = Newtonsoft.Json.Linq.JObject.Parse(json);

                        try
                        {
                            cfs = o.SelectToken("b2b[" + ii + "].cfs").ToString();
                        }
                        catch (Exception ee)
                        {
                            cfs = "";
                        }

                        try
                        {
                            ctin = o.SelectToken("b2b[" + ii + "].ctin").ToString();
                        }
                        catch (Exception ee)
                        {
                            ctin = "";
                        }
                        //MessageBox.Show(cfs.ToString());
                        //MessageBox.Show(ctin.ToString());

                        // MessageBox.Show(o.SelectToken("fp").ToString());
                        try
                        {
                            fp = o.SelectToken("fp").ToString();
                        }
                        catch (Exception ee)
                        {
                            fp = "";
                        }
                        // MessageBox.Show(o.SelectToken("gstin").ToString());
                        try
                        {
                            gstin = o.SelectToken("gstin").ToString();
                        }
                        catch (Exception ee)
                        {
                            gstin = "";
                        }
                        jj++;
                        dt.Rows.Add(val, num, samt, csamt, rt, txval, camt, inv_typ, pos, idt, rchrg, inum, chksum, cfs, ctin, fp, gstin);

                        decimal rt0 = decimal.Parse(rt);
                        decimal rt1 = decimal.Parse(rt) / 2;
                        decimal igst = decimal.Parse(csamt);
                        decimal cgst = decimal.Parse(camt);
                        decimal sgst = decimal.Parse(samt);
                        if ((cgst > 0 || sgst > 0) && igst <= 0)
                        {
                            rt0 = 0;
                        }
                        else if (cgst <= 0 && sgst <= 0)
                        {
                            rt1 = 0;
                        }
                        //  dtEcxelData.Rows.Add(ctin,"",inum,idt,"", txval,decimal.Parse( rt), decimal.Parse(csamt), rt1, decimal.Parse(camt), rt1, decimal.Parse(samt));

                        dtEcxelData.Rows.Add(ctin, "", inum, idt, "", txval, rt0, igst, rt1, cgst, rt1, sgst);
                    }

                }

            }
            catch (Exception eeee)
            {
                ShowWaitingBox waiting = new ShowWaitingBox("Title Text", "Please wait Data Processing....");
                waiting.Stop();
                // this.Dispose();
                //  MessageBox.Show("hh");
            }

        }

        public void pageLoad()
        {
            this.MaximizeBox = false;
            //   this.MinimizeBox = false;         
            dtEcxelData.Clear();
            dtEcxelData.Columns.Clear();
            dtEcxelData.Columns.Add("Supplier's GSTIN", typeof(string));
            dtEcxelData.Columns.Add("Supplier's Name", typeof(string));
            dtEcxelData.Columns.Add("Bill/Trans. No.", typeof(string));
            dtEcxelData.Columns.Add("Bill/Trans. Date", typeof(string));
            dtEcxelData.Columns.Add("HSN/SAC", typeof(string));
            dtEcxelData.Columns.Add("Taxable value", typeof(decimal));
            dtEcxelData.Columns.Add("IGST Rate", typeof(decimal));
            dtEcxelData.Columns.Add("IGST Amount", typeof(decimal));

            dtEcxelData.Columns.Add("CGST Rate", typeof(decimal));
            dtEcxelData.Columns.Add("CGST Amount", typeof(decimal));
            dtEcxelData.Columns.Add("SGST Rate", typeof(decimal));
            dtEcxelData.Columns.Add("SGST Amount", typeof(decimal));



            constring = frmSelectFile.constring;
            con = new SqlConnection(constring);

            try
            {
                //Commented by Priyanka B on 17/05/2017 Start
                if (extension.ToLower() == ".xls")
                {
                    //Excel03Con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                    Excel03Con = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                    Excel07Con = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";

                    this.getRecordsToExcel(file_path, file_name, extension);
                    //Commented by Priyanka B on 17/05/2017 End
                    bindGridSupplier();
                    bindGridReceiver();
                    accept();
                    GridSupplierCheckbox();
                    setdate();
                    resize();
                    countRecords();

                    foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
                    {
                        dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;

                    }
                    foreach (DataGridViewColumn dgvc in dataGridView3.Columns)
                    {
                        dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    //comboBox1.SelectedIndex = 0;
                }
                else if (extension.ToLower() == ".json")
                {
                    this.getRecordsToJSON(file_path);
                    bindGridSupplier();
                    bindGridReceiver();
                    accept();
                    GridSupplierCheckbox();
                    setdate();
                    resize();
                    countRecords();

                    foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
                    {
                        dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;

                    }
                    foreach (DataGridViewColumn dgvc in dataGridView3.Columns)
                    {
                        dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
            }
            catch (Exception e)
            {
                //  MessageBox.Show(e.Message);
                throw new Exception(e.Message.ToString());
            }

            //Commented by Priyanka B on 17/05/2017  Start
            //try
            //{


            //    bindGridSupplier();

            //    bindGridReceiver();

            //    accept();

            //    GridSupplierCheckbox();
            //    setdate();
            //    resize();
            //    countRecords();
            //    foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
            //    {
            //        dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;

            //    }
            //    foreach (DataGridViewColumn dgvc in dataGridView3.Columns)
            //    {
            //        dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            //    }
            //    //comboBox1.SelectedIndex = 0;

            //}
            //catch (Exception f)
            //{
            //    MessageBox.Show(f.Message.ToString());
            //}

            //Commented by Priyanka B on 17/05/2017  End
            int i;
            if (dataGridView1.Columns.Count > 15)
            {
                for (i = dataGridView1.Columns.Count; i <= dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns.RemoveAt(i - 1);
                }
            }



            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{

            //    foreach (DataGridViewRow row1 in dataGridView1.Rows)
            //    {
            //        if (row1.Cells[0].Value.ToString() == "" || row1.Cells[1].Value.ToString() == "" || row1.Cells[2].Value.ToString() == "" || row1.Cells[3].Value.ToString() == "" || row1.Cells[4].Value.ToString() == "" || row1.Cells[5].Value.ToString() == "")
            //        {
            //            dataGridView1.Rows.Remove(row1);
            //        }
            //    }
            //}

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                foreach (DataGridViewRow row1 in dataGridView1.Rows)
                {
                    if (row1.Cells[0].Value.ToString() == "" || row1.Cells[2].Value.ToString() == "" || row1.Cells[3].Value.ToString() == "" || row1.Cells[5].Value.ToString() == "")
                    {
                        dataGridView1.Rows.Remove(row1);
                    }
                }

            }

            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Frozen = true;

            countRecords();


        }

        public void countRecords()
        {
            int count = 0;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                count++;
            }
            label6.Text = "Records Count =" + count;
        }

        public void resize()
        {
            panel1.Size = new Size(this.Width, 55);
            dataGridView1.Size = new Size(this.Width - 30, this.Height / 2 - 100);
            dataGridView3.Size = new Size(this.Width - 30, this.Height / 2 - 100);
            button3.Location = new Point(this.Width - 100, 35 + 30);
            checkBox1.Location = new Point(this.Width - 120, 40 + 30);
            label3.Location = new Point(this.Width - 200, 40 + 30);
            label6.Location = new Point(this.Width - 250, 20);
            button1.Location = new Point(this.Width - 100, 10);


            DataGridViewColumn column = dataGridView1.Columns[1]; // column[1] selects the required column 

            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // sets the AutoSizeMode of column defined in previous line
            DataGridViewColumn column1 = dataGridView1.Columns[12]; // column[1] selects the required column 

            column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[4].Width = 70;

            dataGridView1.Columns[12].Width = 80;
            dataGridView1.Columns[14].Width = 86;

            dataGridView3.Location = new Point(7, dataGridView1.Height + 150);
            label2.Location = new Point(7, dataGridView1.Height + 130);

            dataGridView3.Columns[0].Width = 150;
            dataGridView3.Columns[1].Width = 150;
            dataGridView3.Columns[2].Width = 150;
            dataGridView3.Columns[3].Width = 150;
            dataGridView3.Columns[4].Width = 150;
            dataGridView3.Columns[5].Width = 150;
            dataGridView3.Columns[6].Width = 150;
            dataGridView3.Columns[7].Width = 150;
            dataGridView3.Columns[8].Width = 150;
            label5.Location = new Point(this.Width / 2 - label5.Width / 2, 20);

            panel2.Location = new Point(this.Width - 620, 44);

            dataGridView1.AllowUserToResizeColumns = true;
        }

        public void setdate()
        {

            try
            {
                dt11 = new DataTable();
                getdate = new DataTable();
                dt11.Columns.Add("Entrey_ty");
                dt11.Columns.Add("Trans_cd");

                string entry_ty = "\'\'";
                string Tran_cd = "0";
                string sqlQuery = "";
                //DataTable dttemp1 = new DataTable();
                //MessageBox.Show(dtEcxelData.Rows.Count.ToString());
                //   dataGridView2.DataSource = dtEcxelData;

                foreach (DataRow dt in dtEcxelData.Rows)
                {
                    try
                    {
                        DataTable dttemp1 = new DataTable();

                        string invno = Convert.ToString(dt[2].ToString()).Trim();
                        string invdate = Convert.ToString(dt[3].ToString()).Trim();
                        string hsncode = Convert.ToString(dt[4].ToString()).Trim();
                        //Rupesh G. Comment 15 SEP.2017
                        //string sqlquery = "select entry_ty,Tran_cd,[Bill/Trans. No.] from " + viewname + "  where [Bill/Trans. No.]='" + invno + "'  and [HSN/SAC]='" + hsncode + "' ";
                        string sqlquery = "select entry_ty,Tran_cd,[Bill/Trans. No.] from " + viewname + "  where [Bill/Trans. No.]='" + invno + "'  ";
                        cmd = new SqlCommand(sqlquery, con);
                        cmd.CommandType = CommandType.Text;
                        sda = new SqlDataAdapter(cmd);
                        sda.Fill(dttemp1);

                        if (dttemp1.Rows.Count > 0)
                        {
                            dt11.Rows.Add(dttemp1.Rows[0][0].ToString(), dttemp1.Rows[0][1].ToString());
                        }

                        // dataGridView3.DataSource = dt11;
                    }
                    catch (Exception ee)
                    {
                        //MessageBox.Show(ee.ToString());
                    }
                    finally
                    {
                        //dttemp1.Clear();
                    }
                }

                foreach (DataRow dr in dt11.Rows)
                {
                    entry_ty = entry_ty + "," + "'" + dr[0].ToString() + "'";
                    Tran_cd = Tran_cd + "," + dr[1].ToString();
                }
                sqlQuery = "select entry_ty,Tran_cd,  Convert(varchar,u_cldt,103),inv_no from stmain where entry_ty in (" + entry_ty + ") and Tran_cd in (" + Tran_cd + ") union all " +
                    "select entry_ty,Tran_cd,  Convert(varchar,u_cldt,103),inv_no from sbmain where entry_ty in (" + entry_ty + ") and Tran_cd in (" + Tran_cd + ") union all " +
                    "select entry_ty,Tran_cd,  Convert(varchar,u_cldt,103),pinvno from ptmain where entry_ty in (" + entry_ty + ") and Tran_cd in (" + Tran_cd + ") union all " +
                    "select entry_ty, Tran_cd, Convert(varchar,u_cldt,103),pinvno from epmain where entry_ty in (" + entry_ty + ") and Tran_cd in (" + Tran_cd + ") union all " +
                    "select entry_ty, Tran_cd, Convert(varchar,u_cldt,103),inv_no from cnmain where entry_ty in (" + entry_ty + ") and Tran_cd in (" + Tran_cd + ")union all " +
                    "select entry_ty, Tran_cd, Convert(varchar,u_cldt,103),inv_no from dnmain where entry_ty in (" + entry_ty + ") and Tran_cd in (" + Tran_cd + ")union all " +
                    "select entry_ty, Tran_cd, Convert(varchar,u_cldt,103),inv_no from prmain where entry_ty in (" + entry_ty + ") and Tran_cd in (" + Tran_cd + ")union all " +
                    "select entry_ty, Tran_cd, Convert(varchar,u_cldt,103),pinvno from jvmain where entry_ty in (" + entry_ty + ") and Tran_cd in (" + Tran_cd + ")union all " +
                    "select entry_ty, Tran_cd, Convert(varchar,u_cldt,103),inv_no from srmain where entry_ty in (" + entry_ty + ") and Tran_cd in (" + Tran_cd + ") ";

                try
                {
                    cmd = new SqlCommand(sqlQuery, con);

                    cmd.CommandType = CommandType.Text;
                    sda = new SqlDataAdapter(cmd);
                    sda.Fill(getdate);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Column name invalid");
                }


                int count1 = 0;

                DataTable dd = new DataTable();
                dd.Columns.Clear();
                dd.Columns.Add("inv");
                dd.Columns.Add("date");
                dd.Columns.Add("index");
                dd.Columns.Add("Remark");

                foreach (DataRow dr1 in dtEcxelData.Rows)
                {
                    foreach (DataRow dr in chkdt.Rows)
                    {
                        //Rupesh G. Comment 15 SEP
                        //  if ((dr1[0].ToString().Trim() == dr[2].ToString().Trim() || dr1[1].ToString().Trim() == dr[3].ToString().Trim()) && (dr1[2].ToString().Trim() == dr[4].ToString().Trim() && Convert.ToDateTime(dr1[3].ToString()).ToString("dd/MM/yyyy").Trim() == Convert.ToDateTime(dr[5].ToString()).ToString("dd/MM/yyyy").Trim() && dr1[4].ToString().Trim() == dr[6].ToString().Trim()))

                        if ((dr1[0].ToString().Trim() == dr[2].ToString().Trim() || dr1[1].ToString().Trim() == dr[3].ToString().Trim()) && (dr1[2].ToString().Trim() == dr[4].ToString().Trim() && Convert.ToDateTime(dr1[3].ToString()).ToString("dd/MM/yyyy").Trim() == Convert.ToDateTime(dr[5].ToString()).ToString("dd/MM/yyyy").Trim()))
                        {
                            foreach (DataRow d in getdate.Rows)
                            {
                                if (dr[0].ToString().Trim() == d[0].ToString().Trim() && dr[1].ToString().Trim() == d[1].ToString().Trim() && dr[4].ToString().Trim() == d[3].ToString().Trim())
                                {
                                    string remark = dataGridView1.Rows[count1].Cells[12].Value.ToString().Trim();
                                    dd.Rows.Add(dr1[2].ToString(), d[2].ToString(), count1.ToString(), remark);

                                    if (remark == "Amend")
                                    {
                                        dataGridView1.Rows[count1].Cells[14].Value = null;
                                    }
                                    else
                                            if (remark == "Accepted")
                                    {
                                        DateTime date = Convert.ToDateTime(d[2]);
                                        dataGridView1.Rows[count1].Cells[14].Value = date.ToString("dd/MM/yyyy");
                                    }
                                    else if (remark == "To Be Accept")
                                    {

                                        DateTime date = Convert.ToDateTime(dr1[3]);

                                        dataGridView1.Rows[count1].Cells[14].Value = date.ToString("dd/MM/yyyy");
                                        // dataGridView1.Rows[count1].Cells[14].Value = dr1[3].ToString();

                                    }

                                }
                            }
                        }
                    }
                    count1++;
                }



                //int c = 0;
                //foreach (DataRow dr1 in dtEcxelData.Rows)
                //{
                //    foreach (DataRow dr in chkdt.Rows)
                //    {


                //        if ((dr1[2].ToString().Trim() == dr[4].ToString().Trim() && dr1[3].ToString().Trim() == dr[5].ToString().Trim()))
                //        {

                //            foreach (DataRow d in getdate.Rows)
                //            {


                //                if (dr[0].ToString().Trim() == d[0].ToString().Trim() && dr[1].ToString().Trim() == d[1].ToString().Trim() && dr[4].ToString().Trim() == d[3].ToString().Trim())
                //                {
                //                    //MessageBox.Show(dr[4].ToString());
                //                    //   MessageBox.Show(d[3].ToString());
                //                    string remark = dataGridView1.Rows[c].Cells[12].Value.ToString().Trim();

                //                    if (remark == "Accepted" || remark == "To Be Accept")
                //                    {


                //                        try
                //                        {
                //                            //  MessageBox.Show(c.ToString());
                //                            if (remark == "Amend")
                //                            {
                //                                dataGridView1.Rows[c].Cells[14].Value = null;
                //                            }
                //                            else
                //                            if (remark == "Accepted")
                //                            {

                //                                dataGridView1.Rows[c].Cells[14].Value = d[2].ToString().Trim();
                //                            }
                //                            else if (remark == "To Be Accept")
                //                            {

                //                                DateTime date = (DateTime)dr1[3];

                //                                dataGridView1.Rows[c].Cells[14].Value = date.ToString("dd/MM/yyyy");
                //                            }


                //                        }
                //                        catch (Exception ed)
                //                        {
                //                            MessageBox.Show("" + ed.ToString());
                //                        }
                //                    }
                //                    c++;

                //                }
                //            }

                //        }


                //    }

                //}

            }
            catch (Exception ee)
            {

            }

        }

        private void frmMismatchDetails_Load(object sender, EventArgs e)
        {

            dataGridView1.Visible = false;
            ShowWaitingBox waiting = new ShowWaitingBox("Title Text", "Please wait Data Processing....");
            waiting.Start();
            pageLoad();
            foreach (DataGridViewColumn dc in dataGridView1.Columns)
            {
                if (dc.Index.Equals(0) || dc.Index.Equals(1) || dc.Index.Equals(2) || dc.Index.Equals(3) || dc.Index.Equals(4) || dc.Index.Equals(5) || dc.Index.Equals(6) || dc.Index.Equals(7) || dc.Index.Equals(8) || dc.Index.Equals(9) || dc.Index.Equals(10) || dc.Index.Equals(11))
                {
                    dc.ReadOnly = true;
                }
            }

            waiting.Stop();

            if (dataGridView1.Rows.Count <= 0)
            {
                this.Dispose();
                MessageBox.Show("Selected JSON File Format is not proper.", "ITC Mismatch Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            dataGridView1.Visible = true;
            panel1.Visible = true;
            panel2.Visible = true;
            panel3.Visible = true;
            textBox1.Visible = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            //SetVisibility();  //Commented by Priyanka B on 17/05/2017 Visibility
            //SetAcceptVisibility(); //Commented by Priyanka B on 17/05/2017 Visibility
            if (chkcount <= 0)
            {
                checkBox1.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int Xcor = dataGridView1.CurrentCellAddress.X;

                string ColumnName = dataGridView1.Columns[Xcor].Name;
                if (ColumnName == "AcceptDate")
                {
                    string accept_date = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    string invoice_date = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    string date_period = Convert.ToDateTime(invoice_date).AddDays(180).ToString();

                    if (Convert.ToDateTime(invoice_date) > Convert.ToDateTime(accept_date) || Convert.ToDateTime(accept_date) > Convert.ToDateTime(date_period))
                    {
                        //Commented by Priyanka B on 12/05/2017 Start
                        //MessageBox.Show("Accept Date Invalid.\n1. Accept Date is greater than Bill/Trans. Date.\n2.Accept Date period is 180 days from transaction date.", "ITC Mismatch Reconciliation", MessageBoxButtons.OK,
                        //MessageBoxIcon.Information);
                        //Commented by Priyanka B on 12/05/2017 End

                        //Added by Priyanka B on 12/05/2017 Start
                        MessageBox.Show("Accept Date Invalid.\n1. Accept Date should be greater than Bill/Trans. Date.\n2.Accept Date period should not be exceed more than 180 days from transaction date.", "ITC Mismatch Reconciliation", MessageBoxButtons.OK,
        MessageBoxIcon.Information);
                        //Added by Priyanka B on 12/05/2017 End
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Convert.ToDateTime(invoice_date).ToString("dd/MM/yyyy");
                    }
                }
            }
            catch (Exception eq)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //pageLoad();
            //search_status();

          
            try
            {
                string searchValue = comboBox1.SelectedItem.ToString();
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[12].Value.ToString().Equals(searchValue))
                    {
                        row.Selected = true;
                    }
                    else
                    {
                        row.Selected = false;
                        //    row.Visible = false;
                    }

                }
            }
            catch (Exception exc)
            {

            }
            //SetVisibility(); //Commented by Priyanka B on 17/05/2017 Visibility
        }

        //Added by Priyanka B on 15/05/2017  Start
        public void SetVisibility()
        {
            int cnt = 0;
            cboStatusValue = "ALL";//comboBox1.SelectedItem.ToString().Trim().ToUpper();
            if (dataGridView1.Rows.Count != 0)
            {
                if (cboStatusValue == "ALL")
                {
                    foreach (DataGridViewRow dr in dataGridView1.Rows)
                    {
                        if (dr.Cells[12].Value.ToString().Trim().ToUpper() == "TO BE ACCEPT")
                        {
                            cnt++;
                            break;
                        }
                    }
                    if (cnt != 0)
                    {
                        label3.Visible = true;
                        checkBox1.Visible = true;
                        button3.Visible = true;
                        label3.Text = "Select All";
                        checkBox1.Checked = false;

                    }
                    else
                    {
                        label3.Visible = false;
                        checkBox1.Visible = false;
                        button3.Visible = false;
                    }
                }
                else if (cboStatusValue == "TO BE ACCEPT")
                {
                    label3.Visible = true;
                    checkBox1.Visible = true;
                    button3.Visible = true;
                    label3.Text = "Select All";
                    checkBox1.Checked = false;
                }
                else
                {
                    label3.Visible = false;
                    checkBox1.Visible = false;
                    button3.Visible = false;
                }
            }
            else
            {
                label3.Visible = false;
                checkBox1.Visible = false;
                button3.Visible = false;
            }
        }

        public void SetAcceptVisibility()
        {
            chkAccept = checkBox1.Checked;
            if (chkAccept == true)
                button3.Enabled = true;
            else
                button3.Enabled = false;
        }
        //Added by Priyanka B on 15/05/2017  End

        public void search_status()
        {

            int count = 0;
            //Added by Priyanka B on 12/05/2017 Start
            string status = string.Empty;

            try
            {
                //MessageBox.Show(comboBox1.SelectedItem.ToString());
                status = comboBox1.SelectedItem.ToString();

            }
            catch (Exception ex)
            {

            }


            if (status != "All")
            ////Added by Priyanka B on 12/05/2017 End

            //Commented by Priyanka B on 12/05/2017 Start
            //    status = comboBox1.SelectedItem.ToString();
            //if (status != "All")
            //Commented by Priyanka B on 12/05/2017 End
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {

                    if (dr.Cells[12].Value.ToString() != status)
                    {
                        count++;
                    }
                    //else
                    //{
                    //    MessageBox.Show(dr.Index.ToString());
                    //}
                }




                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    if (dr.Cells[12].Value.ToString() != status)
                    {
                        dataGridView1.Rows.Remove(dr);
                    }
                }

            }

            countRecords();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cn = 0;
            cn1 = 0;
            this.Dispose();

        }

        private void frmMismatchDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            cn = 0;
            cn1 = 0;
            //dtEcxelData.Clear();
            //dataGridView1.Columns.Clear();
            //dtMismatchReceiver.Clear();
            //dataGridView3.Columns.Clear();
            //dataGridView1.DataSource = dtEcxelData;
            //dataGridView3.DataSource = dtMismatchReceiver;

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            RecGridFill();
            try
            {
                RecGridBackColor();
            }
            catch (Exception de)
            {

            }

            //if(e.ColumnIndex==3)
            //{

            ////  MessageBox.Show( Convert.ToDateTime( e.Value).ToString("dd/MM/yyyy"));
            //     // e.CellStyle.Format = "dd/MM/yyyy";
            //   dataGridView1[count1, 3].Value = Convert.ToDateTime(e.Value).ToString("dd/MM/yyyy");

            //    e.FormattingApplied = true;
            //    count1++;
            //}
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            RecGridFill();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    //string val = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        //    //MessageBox.Show(val.ToString());
        //    int Xcor = dataGridView1.CurrentCellAddress.X;
        //    if (Xcor == 13)
        //    {
        //        //DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
        //        //ch1 = (DataGridViewCheckBoxCell)dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[Xcor];

        //        //if (ch1.Value == null)
        //        //    ch1.Value = false;
        //        //switch (ch1.Value.ToString())
        //        //{
        //        //    case "True":
        //        //        ch1.Value = false;
        //        //        break;
        //        //    case "False":
        //        //        ch1.Value = true;
        //        //        break;
        //        //}
        //        int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
        //        foreach (DataGridViewRow dr in dataGridView1.Rows)
        //        {
        //            if ((bool)dr.Cells[Xcor].Value == true)
        //            {
        //                i++;
        //                selectCnt++;
        //                //label3.Visible = true;
        //                //checkBox1.Visible = true;
        //                //button3.Enabled = true;
        //                //label3.Text = "Select All";
        //                //checkBox1.Checked = false;
        //                break;
        //            }
        //        }
        //        //foreach (DataGridViewRow dr in dataGridView1.Rows)
        //        //{
        //        //    if ((bool)dr.Cells[Xcor].Value == true)
        //        //    {
        //        //        selectCnt++;
        //        //        //DataTable dt=dtEcxelData.Select()
        //        //    }
        //        //}
        //        if (i == 0)
        //        {
        //            button3.Enabled = false;
        //        }
        //        else
        //        {
        //            //if (selectCnt == i)
        //            //{

        //            //}
        //            button3.Enabled = true;
        //            i = 0;
        //        }
        //    }
        //}

        public void search_Records()
        {
            int count = 0;

            DataTable dt1 = new DataTable();
            dt1 = dtEcxelData;


            try
            {

                string GSTIN = "[" + dtEcxelData.Columns[0].ColumnName.ToString() + "]";
                string SNAME = "[" + dtEcxelData.Columns[1].ColumnName.ToString() + "]";
                string INVOICE = "[" + dtEcxelData.Columns[2].ColumnName.ToString() + "]";
                string INVOICEDT = "[" + dtEcxelData.Columns[3].ColumnName.ToString() + "]";
                string HSN = "[" + dtEcxelData.Columns[4].ColumnName.ToString() + "]";
                string tax = "[" + dtEcxelData.Columns[5].ColumnName.ToString() + "]";

                try
                {

                    // dtEcxelData = dtEcxelData.Select(" " + GSTIN + " LIKE '%" + textBox1.Text.ToString() + "%' or " + SNAME + " LIKE '%" + textBox1.Text.ToString() + "%' or Convert(" + INVOICE + ",'System.String') LIKE '%" + textBox1.Text.ToString() + "%' or Convert(" + HSN + ",'System.String') LIKE '%" + textBox1.Text.ToString() + "%' or Convert(" + INVOICEDT + ",'System.String') LIKE '%" + textBox1.Text.ToString() + "%' or Convert(" + tax + ",'System.String') LIKE '%" + textBox1.Text.ToString() + "%'").CopyToDataTable();
                    dtEcxelData = dtEcxelData.Select(" " + GSTIN + " = '" + textBox1.Text.ToString() + "' or " + SNAME + " = '" + textBox1.Text.ToString() + "' or Convert(" + INVOICE + ",'System.String') = '" + textBox1.Text.ToString() + "' or Convert(" + HSN + ",'System.String') = '" + textBox1.Text.ToString() + "' or Convert(" + INVOICEDT + ",'System.String') = '" + textBox1.Text.ToString() + "'").CopyToDataTable();



                    //  dtEcxelData = dtEcxelData.Select(" " + GSTIN + " LIKE '%" + textBox1.Text.ToString() + "%' or " + SNAME + " LIKE '%" + textBox1.Text.ToString() + "%'  or " + INVOICE + " LIKE '%" + textBox1.Text.ToString() + "%'  or " + HSN + " LIKE '%" + textBox1.Text.ToString() + "%'  or Convert(" + tax + ",'System.String') LIKE '%" + textBox1.Text.ToString() + "%' or Convert(" + INVOICEDT + ",'System.String') LIKE '%" + textBox1.Text.ToString() + "%' ").CopyToDataTable();

                }
                catch (Exception e)
                {

                }
                dataGridView1.DataSource = dtEcxelData;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AutoGenerateColumns = false;

                if (cn == 0)
                {
                    DataGridViewTextBoxColumn BoxColumn = new DataGridViewTextBoxColumn();
                    BoxColumn.HeaderText = "";
                    BoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    BoxColumn.Name = "Status";
                    BoxColumn.HeaderText = "Status";
                    dataGridView1.Columns.Insert(12, BoxColumn);
                    BoxColumn.DisplayIndex = 12;
                    cn++;

                }
                GridSupplierChk();
                demo2();




                dtEcxelData = dt1;
            }

            catch (Exception)
            {

            }
            countRecords();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchValue = textBox1.Text.ToString();

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            if (searchValue != "")
            {
                try
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[0].Value.ToString().Equals(searchValue) || row.Cells[1].Value.ToString().Trim().Equals(searchValue.Trim()) || row.Cells[2].Value.ToString().Equals(searchValue) || row.Cells[3].Value.ToString().Equals(searchValue))
                        {
                            row.Selected = true;
                        }
                        else
                        {
                            row.Selected = false;
                            //    row.Visible = false;
                        }

                    }
                }
                catch (Exception exc)
                {

                }
            }

            //try
            //{
            //    if (dataGridView1.Rows.Count != 0) //Added by Priyanka B on 15/05/2017
            //    {
            //        if (textBox1.Text == "" || textBox1.Text == null)
            //        {
            //            pageLoad();

            //            if (comboBox1.SelectedIndex.ToString() != "-1")
            //            {
            //                search_status();
            //            }
            //        }
            //        else
            //        {
            //            search_Records();
            //            //setdate();
            //            bindGridReceiver();
            //            accept();
            //            GridSupplierCheckbox();
            //            setdate();
            //            resize();
            //            countRecords();
            //            foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
            //            {
            //                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;

            //            }
            //            foreach (DataGridViewColumn dgvc in dataGridView3.Columns)
            //            {
            //                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            //            }
            //        }
            //        //SetVisibility();  //Added by Priyanka B on 15/05/2017  //Commented by Priyanka B on 17/05/2017 Visibility
            //    }
            //    //else
            //    //{
            //    //    MessageBox.Show("No Records Found!", "Error", MessageBoxButtons.OK);
            //    //}

            //}
            //catch (Exception ex)
            //{
            //    //MessageBox.Show(ex.Message.ToString());
            //    MessageBox.Show(ex.Message.ToString() + Environment.NewLine + ex.Source.ToString() + Environment.NewLine + ex.StackTrace.ToString());
            //}

        }


        public void getData(string file_path, string file_name, string extension, string file_type)
        {
            this.file_path = file_path;
            this.file_name = file_name;
            this.extension = extension;
            this.file_type = file_type;
            if (this.file_type == "Inward")
            {
                viewname = "vw_mismatch_Details";
            }
            else
            {
                viewname = "vw_mismatch_outward";
            }
        }

        //public void getData1(string dbname, string auser, string Product_code, string Default_path, string application_path, string Company_startDate, string Company_endDate) //Commented by Priyanka B on 12/05/2017
        public void getData1(string dbname, string auser, string Product_code, string Default_path, string application_path, string Company_startDate, string Company_endDate, string iconpath, string company_state) //Added by Priyanka B on 12/05/2017
        {

            this.dbname = dbname;
            this.auser = auser;
            this.Product_code = Product_code;
            this.Default_path = Default_path;
            this.application_path = application_path;
            this.Company_startDate = Company_startDate;
            this.Company_endDate = Company_endDate;
            this.Companystate = company_state;
            //Added by Priyanka B on 12/05/2017 Start
            this.iconpath = iconpath;
            Icon MainIcon = new System.Drawing.Icon(this.iconpath);
            this.Icon = MainIcon;
            //Added by Priyanka B on 12/05/2017 End
        }

        //Commented by Priyanka B on 17/05/2017 Start

        public void getRecordsToExcel(string file_path, string file_name, string extension)
        {
            // label2.Text = file_name;
            string filePath = file_path;
            string extension1 = Path.GetExtension(filePath);
            string header = "YES"; /*rtobtnHeaderYes.Checked ? "YES" : "NO";*/
            string filename = Path.GetFileName(filePath);

            string conStr, sheetName;
            conStr = string.Empty;
            switch (extension1)
            {
                case ".xls": //Excel 97-03
                    conStr = string.Format(Excel03Con, filePath, header);
                    break;
                case ".xlsx": //Excel 07
                    conStr = string.Format(Excel07Con, filePath, header);
                    break;
            }
            //Get the name of the First Sheet.

            OleDbConnection con = new OleDbConnection(conStr);

            using (OleDbCommand cmd = new OleDbCommand())
            {
                cmd.Connection = con;
                con.Open();

                dtExcel = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                //Commented by Priyanka B on 16/05/2017 Start
                //if (dtExcel != null)
                //{
                //    if (dtExcel.Rows.Count < 1)
                //        throw new Exception("Error: Could not determine the name of the first worksheet.");
                //    else if (dtExcel.Rows.Count > 1)
                //        throw new Exception("Selected Excel File cannot have more than one Worksheet.");// Found other than Sheet1.\nPlease specify correct sheet name.");
                //}
                //Commented by Priyanka B on 16/05/2017 End

                sheetName = dtExcel.Rows[0]["TABLE_NAME"].ToString().ToUpper().Trim();
                con.Close();
            }

            using (OleDbConnection cn = new OleDbConnection(conStr))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    using (OleDbDataAdapter oda = new OleDbDataAdapter())
                    {
                        dtEcxelData = new DataTable();
                        cmd.CommandText = "SELECT * From [" + sheetName + "]";
                        cmd.Connection = cn;
                        cn.Open();
                        oda.SelectCommand = cmd;
                        oda.Fill(dtEcxelData);
                        cn.Close();

                        //if (dtEcxelData.Columns.Count == 1 && dtEcxelData.Columns[0].ColumnName == "F1")
                        //  throw new Exception("Excel Sheet is blank.");
                    }
                }
            }
        }
        //Commented by Priyanka B On 17/05/2017 End

        public void bindGridSupplier()
        {
            foreach (DataRow dr in dtEcxelData.Rows)
            {

                if (dr[3].ToString() != "" && dr[3] != null)
                {
                    dr[3] = Convert.ToDateTime(dr[3]).ToString("dd/MM/yyyy");
                }


            }
            dataGridView1.DataSource = dtEcxelData;

            //dataGridView1.DataSource = dtEcxelData.DefaultView;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoGenerateColumns = false;

            if (cn == 0)
            {
                DataGridViewTextBoxColumn BoxColumn = new DataGridViewTextBoxColumn();
                BoxColumn.HeaderText = "";
                BoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                BoxColumn.Name = "Status";
                BoxColumn.HeaderText = "Status";
                //dataGridView1.Columns.Insert(dataGridView1.Columns.Count + 1, BoxColumn);
                dataGridView1.Columns.Insert(12, BoxColumn);
                BoxColumn.DisplayIndex = 12;
                cn++;
            }
            GridSupplierChk();
            // if (file_type.Equals("Inward"))
            // {
            //     GridSupplierChk();
            // }
            //else
            // {
            //    // GridSupplierChk1();
            //     // MessageBox.Show("error");
            // }
            demo2();
        }

        public void GridSupplierChk()
        {
            // MessageBox.Show(Companystate);
            int i = 0;
            chkdt = new DataTable();
            string sql = null;
            // sql = " execute SP_mismatch_Details " + Companystate + "," + viewname;
            sql = "select * from " + viewname;
            // MessageBox.Show(sql.ToString());
            cmd = new SqlCommand(sql, con);
            // cmd = new SqlCommand("select * from  " + viewname, con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            sda.Fill(chkdt);

            //Add Supplier Name if Supplier name is blank
            foreach (DataRow drr in dtEcxelData.Rows)
            {

                foreach (DataRow drr1 in chkdt.Rows)
                {
                    if (drr[2].ToString().Trim() == drr1[4].ToString().Trim())
                    {
                        if (drr[1].ToString() == "")
                        {
                            drr[1] = drr1[3].ToString();

                        }
                    }
                }
            }

            dd = new DataTable();
            dd.Clear();
            dd = dtEcxelData;

            foreach (DataRow dr1 in dd.Rows)  //Excel Data
            {
                //MessageBox.Show(dr1[0].ToString());
                int count = 0;

                foreach (DataRow dr in chkdt.Rows)  //View Data
                {

                    //MessageBox.Show("Excel Data Date :- " + Convert.ToDateTime(dr1[3].ToString()).ToString("dd/MM/yyyy").Trim()
                    //    + "\nView Data Date :- " + Convert.ToDateTime(dr[5].ToString()).ToString("dd/MM/yyyy").Trim());

                    //Rupesh G.Comment 15 SEP 2017
                    //if ((dr1[0].ToString().Trim() == dr[2].ToString().Trim() || dr1[1].ToString().Trim() == dr[3].ToString().Trim())
                    //    && (dr1[2].ToString().Trim() == dr[4].ToString().Trim() &&
                    //    Convert.ToDateTime(dr1[3].ToString()).ToString("dd/MM/yyyy").Trim() == Convert.ToDateTime(dr[5].ToString()).ToString("dd/MM/yyyy").Trim()
                    //    && dr1[4].ToString().Trim() == dr[6].ToString().Trim()))

                    //Rupesh G.Add 15 SEP 2017 
                    //26 oct 2017
                    if (dr1[0].ToString().Trim() == dr[2].ToString().Trim() && dr1[1].ToString().Trim() == dr[3].ToString().Trim()
                       && (dr1[2].ToString().Trim() == dr[4].ToString().Trim() &&
                       Convert.ToDateTime(dr1[3].ToString()).ToString("dd/MM/yyyy").Trim() == Convert.ToDateTime(dr[5].ToString()).ToString("dd/MM/yyyy").Trim()
                      ))

                    {
                        count++;
                        break;

                        //  MessageBox.Show("Accept");

                        //double name = double.Parse(dr1[4].ToString().Trim());
                        //double name1 = double.Parse(dr[6].ToString().Trim());
                        //dtsame.Rows.Add(dr1[0].ToString().Trim());
                        //if (name != name1)
                        //{


                        //    int i = dttemp.Rows.IndexOf(dr);
                        //    dttemp.Rows[i]["transtatus"] = "2";
                        //}
                    }


                }
                if (count == 0)
                {
                    //DataGridViewButtonCell a = new DataGridViewButtonCell();
                    //a.ToolTipText = "Remark";
                    // dataGridView1.Rows[i].Cells[12] = a;
                    dataGridView1.Rows[i].Cells[12].Value = "Entry Not Found";
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightSteelBlue;
                }
                else
                {
                    //DataGridViewButtonCell a = new DataGridViewButtonCell();
                    //a.ToolTipText = "Remark";
                    //  dataGridView1.Rows[i].Cells[12] = a;
                    dataGridView1.Rows[i].Cells[12].Value = "";
                    dataGridView1.Rows[i].Cells[12].Value = "To Be Accept";
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightYellow;
                }

                i++;
                //dd.Rows[0].Delete();
            }

            // dataGridView2.DataSource = getdate;
            //  MessageBox.Show(""+getdate.Rows[0][0]);


        }

        public void GridSupplierCheckbox()
        {
            chkcount = 0;
            if (cn1 == 0)
            {

                checkBoxColumn = new DataGridViewCheckBoxColumn();
                checkBoxColumn.HeaderText = "";
                checkBoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                checkBoxColumn.Width = 30;
                checkBoxColumn.Name = "checkBoxColumn";
                dataGridView1.Columns.Insert(13, checkBoxColumn);
                checkBoxColumn.DisplayIndex = 13;

                //MicrosoftDateTimePicker d = new MicrosoftDateTimePicker();
                MicrosoftDateTimePicker d = new MicrosoftDateTimePicker();
                d.HeaderText = "Accept Date";
                d.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


                d.Name = "AcceptDate";
                dataGridView1.Columns.Insert(14, d);
                d.DisplayIndex = 14;
                cn1++;

            }
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[12].Value.ToString() != "To Be Accept")
                {
                    DataGridViewCell cell1 = dataGridView1.Rows[i].Cells[13];

                    chkCell = cell1 as DataGridViewCheckBoxCell;
                    chkCell.Value = false;
                    chkCell.FlatStyle = FlatStyle.Flat;
                    chkCell.Style.BackColor = Color.DarkGray;

                    cell1.ReadOnly = true;
                    dataGridView1.Rows[i].Cells[14].ReadOnly = true;


                }
                else
                {
                    DataGridViewCell cell1 = dataGridView1.Rows[i].Cells[13];

                    chkCell = cell1 as DataGridViewCheckBoxCell;
                    chkCell.Value = false;
                    //chkCell.FlatStyle = FlatStyle.Flat;
                    //chkCell.Style.ForeColor = Color.White;//
                    chkCell.FlatStyle = FlatStyle.Flat;
                    chkCell.Style.BackColor = Color.White;
                    cell1.ReadOnly = false;
                    chkcount++;
                }



            }
            // // add checkbox header
            // Rectangle rect = dataGridView1.GetCellDisplayRectangle(13, -1, true);
            // //  set checkbox header to center of header cell. +1 pixel to position correctly.
            // rect.X = rect.Location.X + (rect.Width / 4);

            // checkboxHeader = new CheckBox();
            // checkboxHeader.Name = "checkboxHeader";
            // checkboxHeader.Size = new Size(14, 14);
            // checkboxHeader.Location = rect.Location;
            //// checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);

            // dataGridView1.Controls.Add(checkboxHeader);

        }

        public void bindGridReceiver()
        {

            dtMismatchReceiver.Columns.Clear();
            dtMismatchReceiver.Clear();
            dtMismatchReceiver.Columns.Add("Invoice /Debit Date");
            dtMismatchReceiver.Columns.Add("HSN/SAC");
            dtMismatchReceiver.Columns.Add("Taxable value\n(as per line item)");
            dtMismatchReceiver.Columns.Add("IGST Rate");
            dtMismatchReceiver.Columns.Add("IGST Amount");
            dtMismatchReceiver.Columns.Add("CGST Rate");
            dtMismatchReceiver.Columns.Add("CGST Amount");
            dtMismatchReceiver.Columns.Add("SGST Rate");
            dtMismatchReceiver.Columns.Add("SGST Amount");
            dataGridView3.DataSource = dtMismatchReceiver;

        }

        public void demo2()
        {

            dataGridView1.RowHeadersVisible = false;
            dataGridView3.RowHeadersVisible = false;

            dataGridView1.Columns[0].Width = 120;
            dataGridView1.Columns[1].Width = 120;
            dataGridView1.Columns[5].DefaultCellStyle.Format = "0.00";
            dataGridView1.Columns[6].DefaultCellStyle.Format = "0.00";
            dataGridView1.Columns[7].DefaultCellStyle.Format = "0.00";
            dataGridView1.Columns[8].DefaultCellStyle.Format = "0.00";
            dataGridView1.Columns[9].DefaultCellStyle.Format = "0.00";
            dataGridView1.Columns[10].DefaultCellStyle.Format = "0.00";
            dataGridView1.Columns[11].DefaultCellStyle.Format = "0.00";

            dataGridView3.Columns[2].DefaultCellStyle.Format = "0.00";
            dataGridView3.Columns[3].DefaultCellStyle.Format = "0.00";
            dataGridView3.Columns[4].DefaultCellStyle.Format = "0.00";
            dataGridView3.Columns[5].DefaultCellStyle.Format = "0.00";
            dataGridView3.Columns[6].DefaultCellStyle.Format = "0.00";
            dataGridView3.Columns[7].DefaultCellStyle.Format = "0.00";
            dataGridView3.Columns[8].DefaultCellStyle.Format = "0.00";
            //dataGridView1.Columns[3].DefaultCellStyle.Format = "dd/MM/yyyy";
            //  dataGridView1.Columns[14].DefaultCellStyle.Format = "dd-MM-yyyy";


            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            DataTable ddd = chkdt;
            int i = 0;
            foreach (DataRow dr1 in dtEcxelData.Rows)
            {
                foreach (DataRow dr in chkdt.Rows)
                {
                    //rrg
                    //  MessageBox.Show(dr1[3].ToString().Trim()+"--------"+ dr[5].ToString().Trim());
                    //Rupesh G Comment 15 Sep 2017
                    //   if ((dr1[0].ToString().Trim() == dr[2].ToString().Trim() || dr1[1].ToString().Trim() == dr[3].ToString().Trim()) && (dr1[2].ToString().Trim() == dr[4].ToString().Trim() && Convert.ToDateTime(dr1[3]).ToString("dd/MM/yyyy").Trim() == Convert.ToDateTime(dr[5]).ToString("dd/MM/yyyy").Trim() && dr1[4].ToString().Trim() == dr[6].ToString().Trim()))

                    if ((dr1[0].ToString().Trim() == dr[2].ToString().Trim() || dr1[1].ToString().Trim() == dr[3].ToString().Trim()) && (dr1[2].ToString().Trim() == dr[4].ToString().Trim() && Convert.ToDateTime(dr1[3]).ToString("dd/MM/yyyy").Trim() == Convert.ToDateTime(dr[5]).ToString("dd/MM/yyyy").Trim()))
                    {
                        string na = dr1[5].ToString().Trim();
                        double name = double.Parse(dr1[5].ToString().Trim());
                        double name1 = double.Parse(dr[7].ToString().Trim());

                        double ch2 = double.Parse(dr1[6].ToString().Trim());
                        double ch22 = double.Parse(dr[8].ToString().Trim());

                        double ch3 = double.Parse(dr1[7].ToString().Trim());
                        double ch33 = double.Parse(dr[9].ToString().Trim());

                        double ch4 = double.Parse(dr1[8].ToString().Trim());
                        double ch44 = double.Parse(dr[10].ToString().Trim());

                        double ch5 = double.Parse(dr1[9].ToString().Trim());
                        double ch55 = double.Parse(dr[11].ToString().Trim());

                        double ch6 = double.Parse(dr1[10].ToString().Trim());
                        double ch66 = double.Parse(dr[12].ToString().Trim());

                        double ch7 = double.Parse(dr1[11].ToString().Trim());
                        double ch77 = double.Parse(dr[13].ToString().Trim());



                        if (name == 0)
                        {
                            name = 0.00;
                        }
                        if (name1 == 0)
                        {
                            name1 = 0.00;
                        }

                        //  dtsame.Rows.Add(dr1[0].ToString().Trim());
                        if (name != name1 || ch2 != ch22 || ch3 != ch33 || ch4 != ch44 || ch5 != ch55 || ch6 != ch66 || ch7 != ch77)
                        {


                            //int i = chkdt.Rows.IndexOf(dr1);

                            DataGridViewButtonCell a = new DataGridViewButtonCell();
                            a.ToolTipText = "Amend";
                            dataGridView1.Rows[i].Cells[12] = a;
                            dataGridView1.Rows[i].Cells[12].Value = "Amend";
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                        }
                    }
                }
                i++;
            }
        }

        public void accept()
        {
            DataTable dtaccept = new DataTable();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                // MessageBox.Show(dataGridView1.Rows[i].Cells[12].Value.ToString());
                if (dataGridView1.Rows[i].Cells[12].Value.ToString() == "To Be Accept")
                {

                    string invno = dataGridView1.Rows[i].Cells[2].Value.ToString().Trim();
                    string invdate = dataGridView1.Rows[i].Cells[3].Value.ToString().Trim();
                    string hsn = dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                    //Rupesh G Comment 15 SEP 2017
                    //string sqlquery = "select transtatus from " + viewname + "  where [Bill/Trans. No.]='" + invno + "'  and [HSN/SAC]='" + hsn + "' ";
                    string sqlquery = "select transtatus from " + viewname + "  where [Bill/Trans. No.]='" + invno + "'   ";
                    cmd = new SqlCommand(sqlquery, con);
                    cmd.CommandType = CommandType.Text;
                    sda = new SqlDataAdapter(cmd);
                    sda.Fill(dtaccept);

                    foreach (DataRow drr in dtaccept.Rows)
                    {
                        if (drr[0].ToString() == "0")
                        {
                            DataGridViewTextBoxCell a = new DataGridViewTextBoxCell();
                            a.ToolTipText = "To Be Accept";
                            dataGridView1.Rows[i].Cells[12] = a;

                            dataGridView1.Rows[i].Cells[12].Value = "To Be Accept";
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightYellow;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[12].Value = "Accepted";
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;

                            //try
                            //{
                            //    for (int i1 = 0; i1 < dataGridView1.RowCount; i1++)
                            //    {
                            //        if (dataGridView1.Rows[i1].Cells[12].Value.ToString() == "Accepted")
                            //    {


                            //        DataGridViewCell cell = dataGridView1.Rows[i1].Cells[13];
                            //        chkCell = cell as DataGridViewCheckBoxCell;
                            //        chkCell.Value = false;
                            //        chkCell.FlatStyle = FlatStyle.Flat;
                            //        chkCell.Style.BackColor = Color.DarkGray;
                            //        cell.ReadOnly = true;
                            //    }

                            //    //    dataGridView1.Rows[i].Cells[13].ReadOnly = true;
                            //    //dataGridView1.Rows[i].Cells[13].Value = false;
                            //    //    dataGridView1.Rows[i].Cells[13].DefaultCellStyle.BackColor = Color.LightGreen;
                            //}
                            //}
                            //catch(Exception)
                            //{

                            //}


                        }
                    }
                }
            }
        }

        public void getamend_records(string entry_ty, string trancd)
        {
            amenddt.Clear();
            amenddt.Reset();
            string Entry_ty = entry_ty;
            int Tran_cd = int.Parse(trancd);
            //    MessageBox.Show("" + Entry_ty);
            //     MessageBox.Show("" + Tran_cd);

            SqlConnection con = new SqlConnection(constring);
            string sql = "EXEC proc_MultipleSelect '" + Entry_ty + "' ,'" + Tran_cd + "'";
       
            // MessageBox.Show(sql);
          
            SqlCommand cmd = new SqlCommand("EXEC proc_MultipleSelect '" + Entry_ty + "' ,'" + Tran_cd + "'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            sda.Fill(amenddt);
        }

        public void amend_data(string Entrey_ty)
        {


            //  MessageBox.Show("enter 1");
            string main = null, item = null, acdet = null, ent = null;
            string[] arr1 = null;
            DataTable dtbcode = new DataTable();
            DataTable findtabledt = new DataTable();
            string sqlquery = " select bcode_nm from LCODE where Entry_ty = '" + Entrey_ty + "'";
            // MessageBox.Show(sqlquery);
            cmd = new SqlCommand(sqlquery, con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            sda.Fill(dtbcode);
            string bcode = null;
            foreach (DataRow dr in dtbcode.Rows)
            {
                bcode = dr[0].ToString();
            }
            // MessageBox.Show(bcode.ToString());

            if (bcode.Trim() != "")
            {
                foreach (DataRow dr in dtbcode.Rows)
                {

                    string et = dr[0].ToString();


                    main = et + "mainam".Trim();
                    item = et + "itemam".Trim();
                    acdet = et + "acdetam".Trim();



                    arr1 = new string[] { main, item, acdet };
                    foreach (string aa in arr1)
                    {
                        //     MessageBox.Show("bcode ====" + aa);
                    }
                    ent = et;

                }

            }

            else
            {
                // MessageBox.Show("enter 2");




                main = Entrey_ty + "mainam".Trim();
                item = Entrey_ty + "itemam".Trim();
                acdet = Entrey_ty + "acdetam".Trim();

                // MessageBox.Show("" + main);
                //MessageBox.Show("" + item);
                //MessageBox.Show("" + acdet);

                arr1 = new string[] { main, item, acdet };
                ent = Entrey_ty;
                // rrg MessageBox.Show("" + ent);
                foreach (string aa in arr1)
                {
                    // rrg   MessageBox.Show("" + aa);
                }
            }
            DataTable dt = amenddt.Tables[0];
            DataTable dt1 = amenddt.Tables[1];
            DataTable dt2 = amenddt.Tables[2];

            foreach (string aa in arr1)
            {
                //  MessageBox.Show(aa);
                findtabledt.Clear();
                findtabledt.Columns.Clear();
                string sqlquery1 = "select * from sysobjects where xtype='u' and name='" + aa + "'";
                //  MessageBox.Show(sqlquery1);
                cmd = new SqlCommand(sqlquery1, con);
                cmd.CommandType = CommandType.Text;
                sda = new SqlDataAdapter(cmd);
                sda.Fill(findtabledt);
                int count = findtabledt.Rows.Count;
                //   MessageBox.Show(count.ToString());
                string extable = null;

                //  extable = string.Concat(aa.Reverse().Skip(2).Reverse()); Rupesh G. Comment 16 SEP 2017
                extable = aa.Remove(aa.Length - 2);
                //  MessageBox.Show("newtable=" + aa);
                //   MessageBox.Show("existable=" + extable);
                if (findtabledt.Rows.Count > 0)
                {

                }
                else
                {
                    try
                    {
                        string sqlquery12 = "select *  into " + aa + " from " + extable + "  where 1=2";

                        //   MessageBox.Show(sqlquery12);
                        cmd = new SqlCommand(sqlquery12, con);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ee)
                    {
                        //   MessageBox.Show("1" + ee.ToString());
                    }
                }
                try
                {
                    if (aa == ent + "mainam".Trim())
                    {
                        using (SqlConnection connection = new SqlConnection(constring))
                        {

                            SqlBulkCopy bulkCopy =
                            new SqlBulkCopy
                            (
                            connection,
                            SqlBulkCopyOptions.TableLock |
                            SqlBulkCopyOptions.FireTriggers |
                            SqlBulkCopyOptions.UseInternalTransaction,
                            null
                            );
                            // set the destination table name
                            bulkCopy.DestinationTableName = main;
                            connection.Open();
                            // write the data in the "dataTable"
                            bulkCopy.WriteToServer(dt);
                            connection.Close();
                        }
                    }
                    if (aa == ent + "itemam".Trim())
                    {
                        using (SqlConnection connection = new SqlConnection(constring))
                        {
                            SqlBulkCopy bulkCopy =
                            new SqlBulkCopy
                            (
                            connection,
                            SqlBulkCopyOptions.TableLock |
                            SqlBulkCopyOptions.FireTriggers |
                            SqlBulkCopyOptions.UseInternalTransaction,
                            null
                            );
                            // set the destination table name
                            bulkCopy.DestinationTableName = item;
                            connection.Open();
                            // write the data in the "dataTable"
                            bulkCopy.WriteToServer(dt1);
                            connection.Close();
                        }
                    }
                    if (aa == ent + "acdetam".Trim())
                    {
                        using (SqlConnection connection = new SqlConnection(constring))
                        {

                            SqlBulkCopy bulkCopy =
                            new SqlBulkCopy
                            (
                            connection,
                            SqlBulkCopyOptions.TableLock |
                            SqlBulkCopyOptions.FireTriggers |
                            SqlBulkCopyOptions.UseInternalTransaction,
                            null
                            );
                            // set the destination table name
                            bulkCopy.DestinationTableName = acdet;
                            connection.Open();
                            // write the data in the "dataTable"
                            bulkCopy.WriteToServer(dt2);
                            connection.Close();
                        }
                    }
                }
                catch (Exception eee)
                {
                    //   MessageBox.Show("2" + eee.ToString());
                }
            }

        }
    }
}
