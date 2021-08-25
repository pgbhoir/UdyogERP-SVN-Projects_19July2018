using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using vunettofx;

namespace ITC_Mismatch_Details
{
    public partial class frmMismatch : Form
    {
        string file_path, file_name, extension;
        string dbname, s, auser, Product_code, Default_path, application_path, Company_startDate, Company_endDate, Entrey_ty, Trans_cd;
        private string Excel03Con;
        private string Excel07Con;
        string constring;
        SqlDataAdapter sda;
        DataTable dtExcel = new DataTable();
        DataTable dtEcxelData = new DataTable();
        DataTable dttemp = new DataTable();
        DataTable dtsame = new DataTable();
        DataSet amenddt = new DataSet();

        bool bb = false;
        string ss;
        DataGridViewCheckBoxColumn checkBoxColumn;
        DataTable dt;
        DataGridViewCheckBoxCell chkCell;
        static int num;
        CheckBox checkboxHeader;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                label1.Text = "Unselect All";
                dt = new DataTable();
                dt.Columns.Add("Entrey_ty");
                dt.Columns.Add("Trans_cd");
                dt.Columns.Add("true/false");
                for (int i = 0; i < dataGridView3.RowCount; i++)
                {
                    if (dataGridView3.Rows[i].Cells[7].Value.ToString() != "")
                    {
                        DataGridViewCell cell = dataGridView3.Rows[i].Cells[8];
                        chkCell = cell as DataGridViewCheckBoxCell;
                        chkCell.Value = false;
                        chkCell.FlatStyle = FlatStyle.Flat;
                        chkCell.Style.ForeColor = Color.DarkGray;
                        cell.ReadOnly = true;
                        dataGridView3.Rows[i].Cells[8].Value = false;
                    }
                    else
                    {

                        dataGridView3.Rows[i].Cells[8].Value = true;
                        dt.Rows.Add(dataGridView3.Rows[i].Cells[0].Value.ToString(), dataGridView3.Rows[i].Cells[1].Value.ToString(), dataGridView3.Rows[i].Cells[8].Value.ToString());

                    }
                }
            }
            else
            {
                label1.Text = "Select All";
                load();

            }


        }

        private void button3_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Entrey_ty");
            dt.Columns.Add("Trans_cd");
            int i = 0;
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[8];
                if ((Boolean)chk.Value == true)
                {
                    dt.Rows.Add(dataGridView3.Rows[i].Cells[0].Value.ToString(), dataGridView3.Rows[i].Cells[1].Value.ToString());
                }
                else
                {

                }
                i++;
            }


            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = null;

            foreach (DataRow dr in dt.Rows)
            {
                string Entrey_ty = dr["Entrey_ty"].ToString();
                string Trans_cd = dr["Trans_cd"].ToString();
                if (Entrey_ty == "S1")
                {

                    cmd = new SqlCommand("update SBMAIN set transtatus = '1' where Entry_ty='S1' and Tran_cd= '" + Trans_cd + "'", con);
                }
                if (Entrey_ty == "ST")
                {
                    cmd = new SqlCommand("update STMAIN set transtatus = '1' where Entry_ty='ST' and Tran_cd= '" + Trans_cd + "'", con);

                }
                if (Entrey_ty == "E1")
                {
                    cmd = new SqlCommand("update EPMAIN set transtatus = '1' where Entry_ty='E1' and Tran_cd= '" + Trans_cd + "'", con);

                }
                if (Entrey_ty == "PT")
                {
                    cmd = new SqlCommand("update PTMAIN set transtatus = '1' where Entry_ty='PT' and Tran_cd= '" + Trans_cd + "'", con);
                }


                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();

            }

            load();
            checkBox1.Checked = false;
            label1.Text = "Select All";
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int Xcor = dataGridView3.CurrentCellAddress.X;
            string ColumnName = dataGridView3.Columns[Xcor].Name;
            if (Xcor != 8)
            {
                try
                {
                    string val = dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    if (val == "Amend")
                    {
                        DialogResult dialogResult = MessageBox.Show("Amend your entry ?", "Miscmatch Reconcile", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (dataGridView3.SelectedCells.Count > 0)
                            {
                                int selectedrowindex = dataGridView3.SelectedCells[0].RowIndex;

                                DataGridViewRow selectedRow = dataGridView3.Rows[selectedrowindex];
                                Entrey_ty = Convert.ToString(selectedRow.Cells[0].Value);
                                Trans_cd = Convert.ToString(selectedRow.Cells[1].Value);
                                getamend_records(Entrey_ty, Trans_cd);

                            }

                            vunettofx.ClNetToFx netToFx = new vunettofx.ClNetToFx();
                            try
                            {
                                netToFx.PrNetToFx(this.dbname, this.Company_startDate, this.Company_endDate, this.auser, this.Default_path, this.application_path, @"C:\VudyogGST\Bmp\Icon_VudyogGST.ico", this.Product_code, "DO ToUeVoucher WITH '" + Entrey_ty + "', " + Trans_cd + ",Thisform.DataSessionId,.T.");
                            }
                            catch (Exception et)
                            {
                                MessageBox.Show(et.ToString());
                            }

                        }
                        else if (dialogResult == DialogResult.No)
                        {

                        }
                        load();

                        for (int i = 0; i < dataGridView3.RowCount; i++)
                        {
                            if (dataGridView3.Rows[i].Cells[0].Value.ToString() == Entrey_ty && dataGridView3.Rows[i].Cells[1].Value.ToString() == Trans_cd && dataGridView3.Rows[i].Cells[7].Value.ToString() == "")
                            {
                                amend_data(Entrey_ty);

                            }

                        }

                    }

                    if (val == "Accept")
                    {
                        SqlConnection con = new SqlConnection(constring);
                        SqlCommand cmd = null;
                        DialogResult dialogResult = MessageBox.Show("Accept your entry ?", "Miscmatch Reconcile", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {

                            int selectedrowindex = dataGridView3.SelectedCells[0].RowIndex;

                            DataGridViewRow selectedRow = dataGridView3.Rows[selectedrowindex];

                            Entrey_ty = Convert.ToString(selectedRow.Cells[0].Value);
                            Trans_cd = Convert.ToString(selectedRow.Cells[1].Value);

                            if (Entrey_ty == "S1")
                            {

                                cmd = new SqlCommand("update SBMAIN set transtatus = '1' where Entry_ty='S1' and Tran_cd= '" + Trans_cd + "'", con);
                            }
                            if (Entrey_ty == "ST")
                            {
                                cmd = new SqlCommand("update STMAIN set transtatus = '1' where Entry_ty='ST' and Tran_cd= '" + Trans_cd + "'", con);

                            }
                            if (Entrey_ty == "E1")
                            {
                                cmd = new SqlCommand("update EPMAIN set transtatus = '1' where Entry_ty='E1' and Tran_cd= '" + Trans_cd + "'", con);

                            }
                            if (Entrey_ty == "PT")
                            {
                                cmd = new SqlCommand("update PTMAIN set transtatus = '1' where Entry_ty='PT' and Tran_cd= '" + Trans_cd + "'", con);
                            }
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Entry Update");

                        }
                        else if (dialogResult == DialogResult.No)
                        {

                        }
                        load();
                    }

                }
                catch (Exception ee)
                {
                    MessageBox.Show("" + ee);
                }


            }




            //  checkSelection();
        }
        public void checkSelection()
        {
            int count = 0;
            int i = 0;
            count++;
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                string chk = dataGridView3.Rows[i].Cells[8].Value.ToString();

                if (chk == "True" && count == 1)
                {
                    count = 0;
                    label1.Text = "Unselect all";
                    checkBox1.Checked = true;
                }
                else
                {
                    label1.Text = "Select All";
                    //   checkBox1.Checked = false;
                }
                i++;
            }
            // MessageBox.Show(count.ToString());
            //if (count <=1)
            //{
            //   

            //}
            //else
            //{
            //    label1.Text = "Unselect all";
            //    checkBox1.Checked = true;
            //}

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void load()
        {


            checkBox1.Checked = false;
            dttemp.Columns.Clear();
            dttemp.Clear();
            dtsame.Columns.Clear();
            dtsame.Clear();
            dttemp.Columns.Add("Entry_ty", typeof(String));
            dttemp.Columns.Add("Tran_cd", typeof(String));
            dttemp.Columns.Add("Ac_Id", typeof(String));
            dttemp.Columns.Add("Date", typeof(String));
            dttemp.Columns.Add("Invoice_Number", typeof(String));
            dttemp.Columns.Add("party_nm", typeof(String));
            dttemp.Columns.Add("tot_examt", typeof(String));
            dttemp.Columns.Add("transtatus", typeof(String));
            dttemp.Columns.Add("transtatus1", typeof(String));
            constring = frmSelectFile.constring;
            dtsame.Columns.Add("InvoiceNo");

            if (extension == ".xls")
            {
                Excel03Con = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                Excel07Con = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";

                this.getRecordsToExcel(file_path, file_name, extension);
            }

            frmSelectFile s = new frmSelectFile();
            //    MessageBox.Show(frmSelectFile.Company_startDate);
            demo1();
            // demo3();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            // Resize the master DataGridView columns to fit the newly loaded data.
            //dataGridView3.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);


            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.AutoGenerateColumns = false;






            dataGridView3.Columns.RemoveAt(8);
            checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "";
            checkBoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            checkBoxColumn.Width = 30;
            checkBoxColumn.Name = "checkBoxColumn";
            dataGridView3.Columns.Insert(8, checkBoxColumn);
            checkBoxColumn.DisplayIndex = 8;



            for (int i = 0; i < dataGridView3.RowCount; i++)
            {
                if (dataGridView3.Rows[i].Cells[7].Value.ToString() != "")
                {
                    DataGridViewCell cell1 = dataGridView3.Rows[i].Cells[8];

                    chkCell = cell1 as DataGridViewCheckBoxCell;
                    chkCell.Value = false;
                    chkCell.FlatStyle = FlatStyle.Flat;
                    chkCell.Style.ForeColor = Color.DarkGray;

                    cell1.ReadOnly = true;

                }
                if (dataGridView3.Rows[i].Cells[7].Value.ToString() != "Amend")
                {
                    DataGridViewCell cell1 = dataGridView3.Rows[i].Cells[8];

                    chkCell = cell1 as DataGridViewCheckBoxCell;
                    chkCell.Value = false;
                    // chkCell.FlatStyle = FlatStyle.Flat;
                    //  chkCell.Style.ForeColor = Color.DarkGray;

                    // cell1.ReadOnly = true;

                }



            }
        }
        private void frmMismatch_Load(object sender, EventArgs e)
        {
            dataGridView3.AllowUserToAddRows = false;

            load();
            dataGridView3.AutoGenerateColumns = false;
            // add checkbox header
            Rectangle rect = dataGridView3.GetCellDisplayRectangle(8, -1, true);
            //  set checkbox header to center of header cell. +1 pixel to position correctly.
            rect.X = rect.Location.X + (rect.Width / 4);

            checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            checkboxHeader.Size = new Size(14, 14);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);

            dataGridView3.Controls.Add(checkboxHeader);
        }

        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxHeader.Checked)
            {
                label1.Text = "Unselect All";
                dt = new DataTable();
                dt.Columns.Add("Entrey_ty");
                dt.Columns.Add("Trans_cd");
                dt.Columns.Add("true/false");
                for (int i = 0; i < dataGridView3.RowCount; i++)
                {
                    if (dataGridView3.Rows[i].Cells[7].Value.ToString() != "")
                    {
                        DataGridViewCell cell = dataGridView3.Rows[i].Cells[8];
                        chkCell = cell as DataGridViewCheckBoxCell;
                        chkCell.Value = false;
                        chkCell.FlatStyle = FlatStyle.Flat;
                        chkCell.Style.ForeColor = Color.DarkGray;
                        cell.ReadOnly = true;
                        dataGridView3.Rows[i].Cells[8].Value = false;
                    }
                    else
                    {

                        dataGridView3.Rows[i].Cells[8].Value = true;
                        dt.Rows.Add(dataGridView3.Rows[i].Cells[0].Value.ToString(), dataGridView3.Rows[i].Cells[1].Value.ToString(), dataGridView3.Rows[i].Cells[8].Value.ToString());

                    }
                }
            }
            else
            {
                load();
            }

            dataGridView3.EndEdit();
        }
        public frmMismatch()
        {
            InitializeComponent();
        }

        public void getData(string file_path, string file_name, string extension)
        {
            this.file_path = file_path;
            this.file_name = file_name;
            this.extension = extension;
        }

        public void getData1(string dbname, string auser, string Product_code, string Default_path, string application_path, string Company_startDate, string Company_endDate)
        {

            this.dbname = dbname;
            this.auser = auser;
            this.Product_code = Product_code;
            this.Default_path = Default_path;
            this.application_path = application_path;
            this.Company_startDate = Company_startDate;
            this.Company_endDate = Company_endDate;

        }

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
                sheetName = dtExcel.Rows[0]["TABLE_NAME"].ToString();
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

                        //  dataGridView1.DataSource = dtEcxelData;

                        // dataGridView1.AutoResizeColumns();
                        // dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                        //DataTable dt1=  dt.AsEnumerable().Take(1).CopyToDataTable();
                        //  string name = dt1.Rows[0][2].ToString();

                    }
                }
            }
        }

        public void demo1()
        {


            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("select * from vw_mismatch ", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);


            sda.Fill(dttemp);
            demo2();
            dataGridView3.DataSource = dttemp;

            foreach (DataRow dr in dttemp.Rows)
            {
                int i = dttemp.Rows.IndexOf(dr);

                if (dr["transtatus"].ToString() == "0")
                {

                    //DataGridViewButtonCell a = new DataGridViewButtonCell();

                    //dataGridView3.Rows[i].Cells[7] = a;
                    dataGridView3.Rows[i].Cells[7].Value = "";
                    dataGridView3.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                    //DataGridViewCheckBoxCell a12= new DataGridViewCheckBoxCell();

                    //  dataGridView3.Rows[i].Cells[8] = a12;
                    //  dataGridView3.Rows[i].Cells[8].Value = "Accept11";
                }
                if (dr["transtatus"].ToString() == "2")
                {

                    DataGridViewButtonCell a = new DataGridViewButtonCell();
                    a.ToolTipText = "Amend";
                    dataGridView3.Rows[i].Cells[7] = a;
                    dataGridView3.Rows[i].Cells[7].Value = "Amend";
                    dataGridView3.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;

                }
                if (dr["transtatus"].ToString() == "1")
                {

                    dataGridView3.Rows[i].Cells[7].Value = "Accepted";
                    dataGridView3.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;

                }

            }



        }

        public void demo2()
        {
            foreach (DataRow dr1 in dtEcxelData.Rows)
            {
                foreach (DataRow dr in dttemp.Rows)
                {


                    if (dr1[0].ToString().Trim() == dr[4].ToString().Trim())
                    {
                        double name = double.Parse(dr1[4].ToString().Trim());
                        double name1 = double.Parse(dr[6].ToString().Trim());
                        dtsame.Rows.Add(dr1[0].ToString().Trim());
                        if (name != name1)
                        {


                            int i = dttemp.Rows.IndexOf(dr);
                            dttemp.Rows[i]["transtatus"] = "2";
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
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("EXEC proc_MultipleSelect '" + Entry_ty + "' ,'" + Tran_cd + "'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);


            sda.Fill(amenddt);

            //  dataGridView1.DataSource = amenddt.Tables[0];

            // dataGridView2.DataSource = amenddt.Tables[1];


            // dataGridView4.DataSource = amenddt.Tables[2];


        }

        public void amend_data(string Entrey_ty)
        {
            if (Entrey_ty == "ST")
            {
                DataTable dt = amenddt.Tables[0];
                DataTable dt1 = amenddt.Tables[1];
                DataTable dt2 = amenddt.Tables[2];
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
                    bulkCopy.DestinationTableName = "stmainam";
                    connection.Open();
                    // write the data in the "dataTable"
                    bulkCopy.WriteToServer(dt);
                    connection.Close();
                }
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
                    bulkCopy.DestinationTableName = "stitemam";
                    connection.Open();
                    // write the data in the "dataTable"
                    bulkCopy.WriteToServer(dt1);
                    connection.Close();
                }
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
                    bulkCopy.DestinationTableName = "stacdetam";
                    connection.Open();
                    // write the data in the "dataTable"
                    bulkCopy.WriteToServer(dt2);
                    connection.Close();
                }

            }//end if


        }




    }
}
