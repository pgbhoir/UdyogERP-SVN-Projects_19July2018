using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Udyog.Library.Common;

using ITC_Mismatch_DetailsProperties;

namespace ITC_Mismatch_Details
{
    public partial class frmSelectFile : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        string file_path, file_name, extension;
        public static string constring;
        String cAppPId, cAppName;
        static public string Company_db, Company_startDate, Company_endDate, application_user, Default_path, application_path, Main_icon, Product_code, Company_State;
        private string Excel03Con;
        private string Excel07Con;
        PubClassProp objProperties = new PubClassProp();

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        DataTable dtExcel = new DataTable();

        private void frmSelectFile_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

        private void frmSelectFile_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            mInsertProcessIdRecord();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string file_type = string.Empty;
            if (textBox1.Text == "" && textBox2.Text == "")
            {
                //MessageBox.Show("Select Your Excel File", "ITC Mismatch Details", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);  //Commented by Priyanka B on 17/05/2017
                MessageBox.Show("Please Select File!", "ITC Mismatch Details", MessageBoxButtons.OK, MessageBoxIcon.Error);  //Added by Priyanka B on 17/05/2017
            }
            else
            {
                if (radioButton1.Checked == true)
                {
                    file_type = "Inward";
                }
                if (radioButton2.Checked == true)
                {
                    file_type = "Outward";
                }

                try
                {
                    frmMismatchDetails frm = new frmMismatchDetails();

                    frm.getData(file_path, file_name, extension, file_type);
                    //  frm.getData1(pComDbnm, pAppUerName, Product_code, Default_path, application_path, Company_startDate, Company_endDate);   //Commented by Priyanka B on 12/05/2017

                    frm.getData1(pComDbnm, pAppUerName, Product_code, Default_path, application_path, Company_startDate, Company_endDate, Main_icon, Company_State);   //Added by Priyanka B on 12/05/2017
                    frm.ShowDialog();

                    this.Dispose();



                }
                catch (Exception ess)
                {
                    MessageBox.Show(ess.Message.ToString(), "ITC Mismatch Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public frmSelectFile()
        {

        }
        public frmSelectFile(string[] args)
        {
            // MessageBox.Show("enter 1");
            InitializeComponent();
            pCompId = Convert.ToInt16(args[0]);
            //this.pFrmCaption = "ITC Mismatch Reconciliation";   //Commented by Prajakta B. on 16082018 for ERP 2.0.0.0 Installer
            this.pFrmCaption = "Mismatch Reconcile";             //Added by Prajakta B. on 16082018 for ERP 2.0.0.0 Installer
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            Main_icon = args[7].Replace("<*#*>", " "); //Added by Priyanka B on 12/05/2017
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];
            //  MessageBox.Show("enter 2");
            try
            {
                //     MessageBox.Show("enter 3");
                Product_code = args[12];
                Default_path = args[13];
                application_path = args[14];
                Company_startDate = args[15];
                Company_endDate = args[16];
                Company_State = args[17].Trim().ToString();
                //  MessageBox.Show("" + args[17].Trim().ToString());
            }
            catch (Exception)
            {
                // MessageBox.Show("enter 4");
            }
            constring = @"Data Source=" + pServerName + ";Initial Catalog=" + pComDbnm + ";User id = " + pUserId + ";password=" + pPassword + "";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //openFileDialog1.ShowDialog();  //Commented by Priyanka B on 15/05/2017

                //Added by Priyanka B on 15/05/2017 Start
                openFileDialog1.FileName = "";
                DialogResult dres = openFileDialog1.ShowDialog();
                if (dres == DialogResult.OK && openFileDialog1.FileName != string.Empty)
                {
                    //Added by Priyanka B on 15/05/2017 End
                    string filePath = openFileDialog1.FileName;
                    string extension = Path.GetExtension(filePath);
                    string file_name = Path.GetFileName(filePath);
                    file_path = filePath;
                    this.extension = extension;
                    this.file_name = file_name;
                    if (extension.ToLower() != ".json")
                    //  if (extension.ToLower() != ".xls" && extension.ToLower() != ".json")
                    {
                        MessageBox.Show("Please select the valid JSON file with the .json extension.", "ITC Mismatch Details", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // MessageBox.Show("Please select the valid excel file with the .xls extension or JSON file with the .json extension.", "ITC Mismatch Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox1.Text = "";
                        textBox2.Text = "";
                    }
                    else
                    {
                        textBox1.Text = file_name.ToString();
                        textBox2.Text = extension.ToString();
                    }
                }

                //Added by Priyanka B on 17/05/2017 Start
                if (extension.ToLower() == ".xls")
                {
                    //Excel03Con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                    Excel03Con = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                    Excel07Con = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";

                    this.getRecordsToExcel(file_path, file_name, extension);
                }

                //Added by Priyanka B on 17/05/2017 End
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        //Added by Priyanka B on 17/05/2017 Start

        public void getRecordsToExcel(string file_path, string file_name, string extension)
        {
            // label2.Text = file_name;
            string filePath = file_path;
            string extension1 = Path.GetExtension(filePath);
            string header = "YES"; /*rtobtnHeaderYes.Checked ? "YES" : "NO";*/
            string filename = Path.GetFileName(filePath);

            string conStr = string.Empty, sheetName = string.Empty;

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
                //dtExcel = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                if (dtExcel != null)
                {
                    if (dtExcel.Rows.Count < 1)
                        throw new Exception("Error: Could not determine the name of the first worksheet.");
                    else if (dtExcel.Rows.Count > 1)
                        throw new Exception("Selected Excel File cannot have more than one Worksheet.");// Found other than Sheet1.\nPlease specify correct sheet name.");
                }
                //sheetName = dtExcel.Rows[0].Field<string>("TABLE_NAME").ToString();                

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
                        objProperties.DtExcelData = new DataTable();
                        cmd.CommandText = "SELECT * From [" + sheetName + "]";
                        cmd.Connection = cn;
                        cn.Open();
                        oda.SelectCommand = cmd;
                        oda.Fill(objProperties.DtExcelData);
                        cn.Close();

                        if (objProperties.DtExcelData.Columns.Count < 12 && objProperties.DtExcelData.Columns[0].ColumnName == "F1")
                            throw new Exception("Selected Excel File is blank.");
                        else if (objProperties.DtExcelData.Columns.Count < 12 && objProperties.DtExcelData.Columns[0].ColumnName != "F1")
                            throw new Exception("Selected Excel File Format is not proper.\nExcel File should have only one Worksheet and with the desired columns.");
                        //   else if (dtExcel.Rows.Count > 1)
                        //     throw new Exception("Selected Excel File cannot have more than one Worksheet.");// Found other than Sheet1.\nPlease specify correct sheet name.");
                    }
                }
            }
        }
        //Added by Priyanka B on 17/05/2017 End

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    if (textBox1.Text == "" && textBox2.Text == "")
        //    {
        //        MessageBox.Show("Select Your Excel File", "ITC Mismatch Details", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
        //    }
        //    else
        //    {
        //        frmMismatch frm = new frmMismatch();
        //        frm.getData(file_path, file_name, extension);
        //        frm.getData1(pComDbnm, pAppUerName, Product_code, Default_path, application_path, Company_startDate, Company_endDate);
        //        frm.ShowDialog();
        //        this.Close();

        //    }
        //}

        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            //cAppName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            cAppName = "ITC_Mismatch_Details.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }

        private void mDeleteProcessIdRecord()
        {
            if (string.IsNullOrEmpty(this.pPApplName) || this.pPApplPID == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))
            {
                return;
            }
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
    }
}
