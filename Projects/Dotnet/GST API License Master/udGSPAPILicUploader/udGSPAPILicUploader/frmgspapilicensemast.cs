using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using uBaseForm;
using DataAccess_Net;
//using ueconnect;
using System.Collections;
using System.Diagnostics;
using System.Linq;
  
//Added by Prajakta B on 20042018 
namespace udGSPAPILicUploader
{
    public partial class frmgspapilicensemast : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DateTime sysdate = DateTime.Now;
        string SqlStr = string.Empty;
        string vMainField = "GSTAPPNAME", vMainTblNm = "GSTAPILICMAST", vMainFldVal = "";
        string vExpression = string.Empty;
        String cAppPId, cAppName;
        string modName;
        Boolean cValid = true;
        //clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        DataTable _cmnDataTbl;

        public frmgspapilicensemast(string[] args)
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "GSP API License Master";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // To list only csv files, we need to add this filter
            openFileDialog.Filter = "|*.csv";
            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtPath.Text = openFileDialog.FileName;
            }
            else
            {
                return;
            }

            try
            {
                _cmnDataTbl = GetDataTableFromCSVFile(txtPath.Text);
                dgCSVData.DataSource = _cmnDataTbl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Import CSV File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static DataTable GetDataTableFromCSVFile(string csvfilePath)
        {
            DataTable csvData = new DataTable();
            using (TextFieldParser csvReader = new TextFieldParser(csvfilePath))
            {
                csvReader.SetDelimiters(new string[] { "," });
                csvReader.HasFieldsEnclosedInQuotes = true;

                //Read columns from CSV file, remove this line if columns not exits  
                string[] colFields = csvReader.ReadFields();

                foreach (string column in colFields)
                {
                    DataColumn datecolumn = new DataColumn(column);
                    datecolumn.AllowDBNull = true;
                    csvData.Columns.Add(datecolumn);
                }

                while (!csvReader.EndOfData)
                {
                    string[] fieldData = csvReader.ReadFields();
                    //Making empty value as null
                    for (int i = 0; i < fieldData.Length; i++)
                    {
                        if (fieldData[i] == "")
                        {
                            fieldData[i] = null;
                        }
                    }
                    csvData.Rows.Add(fieldData);
                }
            }
            return csvData;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt1 = _cmnDataTbl;
            //oDataAccess = new DataAccess_Net.clsDataAccess();

            if (string.IsNullOrEmpty(this.txtPath.Text))
            {
                MessageBox.Show("Import Folder path cannot be empty...!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DataTable sqldatatbl = new DataTable();
                string sqlstr = "select GSTAPPNAME,GSPUSERID,GSPPASSWD from GSTAPILICMAST";
                sqldatatbl = oDataAccess.GetDataTable(sqlstr, null, 20);

                if (dt1.Rows.Count > 0)
                {
                    var dt1Records = dt1.AsEnumerable().Select(e1 => new { Id = e1["App name"].ToString(), Name = e1["Client Id"].ToString(), secret = e1["Client Secret"].ToString() });

                    var sqltblRecords = sqldatatbl.AsEnumerable().Select(e2 => new { Id = e2["GSTAPPNAME"].ToString(), Name = e2["GSPUSERID"].ToString(), secret = e2["GSPPASSWD"].ToString() });

                    var extraRecords = dt1Records.Except(sqltblRecords);

                    if (extraRecords.Count() > 0)
                    {
                        foreach (var data in extraRecords)
                        {
                            string sqlquery = "set dateformat dmy Insert Into GSTAPILICMAST (GSTAPPNAME,GSPUSERID,GSPPASSWD,CREATEDON,CREATEDBY) Values('" + data.Id.ToString() + "','" + data.Name.ToString() + "','" + data.secret.ToString() + "','" + sysdate + "','" + pAppUerName + "')";
                            oDataAccess.ExecuteSQLStatement(sqlquery, null, 20, true);
                        }
                        MessageBox.Show("Record Saved Successfully....!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Record already Saved....!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    foreach (DataRow row in dt1.Rows)
                    {
                        string sqlquery = "set dateformat dmy Insert Into GSTAPILICMAST (GSTAPPNAME,GSPUSERID,GSPPASSWD,CREATEDON,CREATEDBY) Values('" + row[0].ToString() + "','" + row[1].ToString() + "','" + row[2].ToString() + "','" + sysdate + "','" + pAppUerName + "')";
                        oDataAccess.ExecuteSQLStatement(sqlquery, null, 20, true);
                    }
                    MessageBox.Show("Record Saved Successfully....!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            DataTable dt1 = _cmnDataTbl;
            //oDataAccess = new DataAccess_Net.clsDataAccess();

            DataTable sqldatatbl = new DataTable();
            string sqlstr = "select GSTAPPNAME,GSPUSERID,GSPPASSWD from GSTAPILICMAST";
            sqldatatbl = oDataAccess.GetDataTable(sqlstr, null, 20);
            
            if (string.IsNullOrEmpty(this.txtPath.Text))
            {
                this.Close();
            }
            else
            {
                if (dt1.Rows.Count > 0)
                {
                    var dt1Records = dt1.AsEnumerable().Select(e1 => new { Id = e1["App name"].ToString(), Name = e1["Client Id"].ToString(), secret = e1["Client Secret"].ToString() });

                    var sqltblRecords = sqldatatbl.AsEnumerable().Select(e2 => new { Id = e2["GSTAPPNAME"].ToString(), Name = e2["GSPUSERID"].ToString(), secret = e2["GSPPASSWD"].ToString() });

                    var extraRecords = dt1Records.Except(sqltblRecords);

                    if (extraRecords.Count() > 0)
                    {
                        if (MessageBox.Show("Record Not Saved. Do You want to Close...!", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.Close();
                        }
                        else
                        {
                            this.Activate();
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
        }

        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udGSPAPILicUploader.exe";
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

        private void frmgspapilicensemast_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            startupPath = Application.StartupPath;
            //oConnect = new clsConnect();

            GetInfo.iniFile ini = new GetInfo.iniFile(startupPath + "\\" + "Visudyog.ini");
            string appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
            //oConnect.InitProc("'" + startupPath + "'", appfile);
            DirectoryInfo dir = new DirectoryInfo(startupPath);
            Array totalFile = dir.GetFiles();
            string registerMePath = string.Empty;

            for (int i = 0; i < totalFile.Length; i++)
            {
                string fname = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                if (Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name).ToUpper().Contains("REGISTER.ME"))
                {
                    registerMePath = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                    break;
                }

            }
            if (registerMePath == string.Empty)
            {
                ServiceType = "";
            }
            else
            {
                //string[] objRegisterMe = (oConnect.ReadRegiValue(startupPath)).Split('^');
                //ServiceType = objRegisterMe[15].ToString().Trim();
            }
            this.mInsertProcessIdRecord();
            this.SetFormColor();

            string appPath = Application.ExecutablePath;

            appPath = Path.GetDirectoryName(appPath);

            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }
        }

        private void mcheckCallingApplication()
        {
            Process pProc;
            Boolean procExists = true;
            try
            {
                pProc = Process.GetProcessById(Convert.ToInt16(this.pPApplPID));
                String pName = pProc.ProcessName;
                string pName1 = this.pPApplName.Substring(0, this.pPApplName.IndexOf("."));
                if (pName.ToUpper() != pName1.ToUpper())
                {
                    procExists = false;
                }
            }
            catch (Exception)
            {
                procExists = false;

            }
            if (procExists == false)
            {
                MessageBox.Show("Can't proceed,Main Application " + this.pPApplText + " is closed", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
        }

        private void SetFormColor()
        {
            DataSet dsColor = new DataSet();
            Color myColor = Color.Coral;
            string strSQL;
            string colorCode = string.Empty;
            strSQL = "select vcolor from Vudyog..co_mast where compid =" + this.pCompId;
            dsColor = oDataAccess.GetDataSet(strSQL, null, 20);
            if (dsColor != null)
            {
                if (dsColor.Tables.Count > 0)
                {
                    dsColor.Tables[0].TableName = "ColorInfo";
                    colorCode = dsColor.Tables["ColorInfo"].Rows[0]["vcolor"].ToString();
                }
            }

            if (!string.IsNullOrEmpty(colorCode))
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                myColor = Color.FromArgb(Convert.ToInt32(colorCode.Trim()));
            }
            this.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);
        }

        private void frmgspapilicensemast_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }
    }
}
