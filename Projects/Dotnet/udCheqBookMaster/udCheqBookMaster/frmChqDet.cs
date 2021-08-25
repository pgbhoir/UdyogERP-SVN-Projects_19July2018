using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using uBaseForm;
using System.Collections;
using udSelectPop;
using DataAccess_Net;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using ueconnect;
using GetInfo;
using uNumericTextBox;
namespace udCheqBookMaster
{
    public partial class frmChqDet : uBaseForm.FrmBaseForm
    {
        DataSet vDsCommon;
        DataAccess_Net.clsDataAccess oDataAccess;
        string ServiceType = string.Empty;
        string vSqlStr = string.Empty, vBook_Id = string.Empty;
        short vTimeOut = 50;
        String cAppPId, cAppName;
        DataTable vTblMain;

        DataTable tblChqDet;
        CheckBox checkboxHeader;
        public frmChqDet()
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
        }

        private void frmChqDet_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            DataSet vDsCommon;
            this.pAppPath = Application.ExecutablePath;
            this.pAppPath = Path.GetDirectoryName(this.pAppPath);
            this.mthControlSet();

            this.btnEdit.Enabled = false;
            this.btnExit.Enabled = false;

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.mthDsCommon();
            this.SetFormColor();
            this.SetMenuRights();
            this.mInsertProcessIdRecord();

            clsConnect oConnect;
            string startupPath = Application.StartupPath;



            this.mthView();
            this.mthEnableDisableFormControls();
            lblProgress.Visible = false;
            progressBar1.Visible = false;
        }
        private void mthView()
        {


            if (ServiceType.ToUpper() == "VIEWER VERSION")
            {
                //this.btnNew.Enabled = false;
                //this.btnEdit.Enabled = false;
                //this.btnCancel.Enabled = false;
                //this.btnDelete.Enabled = false;
                //this.btnPreview.Enabled = false;
                //this.btnPrint.Enabled = false;

            }
            else
            {
                this.mthGrdRefresh();
                this.mthEnableDisableFormControls();
                this.mthChkNavigationButton();

            }

            this.dgvChequeDet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));



        }
        private void mthGrdRefresh()
        {

            //vSqlStr = "Execute [USP_Ent_Cheque_Det] " + this.pBook_Id;

            //tblChqDet = oDataAccess.GetDataTable(vSqlStr, null, 25);

            this.dgvChequeDet.Columns.Clear();
            this.dgvChequeDet.DataSource = this.pTblMain;
            this.dgvChequeDet.Columns.Clear();

            System.Windows.Forms.DataGridViewCheckBoxColumn colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            //this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            colSelect.HeaderText = "";
            colSelect.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSelect.Width = 40;
            colSelect.Name = "colSelect";
         //   colSelect.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colSelect);

            System.Windows.Forms.DataGridViewTextBoxColumn colChqNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colChqNo.HeaderText = "Cheque No.";
            colChqNo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colChqNo.Width = 100;
            colChqNo.Name = "colChqNo";
            colChqNo.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colChqNo);

            DataGridViewComboBoxColumn colStatus = new DataGridViewComboBoxColumn();
            colStatus.HeaderText = "Cheque Status";
            colStatus.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colStatus.Width = 100;
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Items.Add("Available");
            colStatus.Items.Add("Dishonor/Cancelled");
            colStatus.Items.Add("Issued");
            colStatus.Items.Add("PDC");
            colStatus.Items.Add("Reconciled");
          //  colStatus.Items.Add("Reco.Pending");//Comment Rupesh G. on 18 june 2018
            this.dgvChequeDet.Columns.Add(colStatus);

            System.Windows.Forms.DataGridViewTextBoxColumn colTrnNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTrnNm.HeaderText = "Transaction Name";
            colTrnNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colTrnNm.Width = 130;
            colTrnNm.Name = "colTrnNm";
            colTrnNm.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colTrnNm);

            System.Windows.Forms.DataGridViewTextBoxColumn colPartyNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPartyNm.HeaderText = "Party Name";
            colPartyNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colPartyNm.Width = 130;
            colPartyNm.Name = "colPartyNm";
            colPartyNm.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colPartyNm);



            dgvChequeDet.Columns["colSelect"].DataPropertyName = "sel";
            dgvChequeDet.Columns["colChqNo"].DataPropertyName = "Chq_No";
            dgvChequeDet.Columns["colStatus"].DataPropertyName = "Status";
            dgvChequeDet.Columns["colTrnNm"].DataPropertyName = "TrnNm";
            dgvChequeDet.Columns["colPartyNm"].DataPropertyName = "PartyNm";

            dgvChequeDet.Refresh();

            //rrg
            Rectangle rect = dgvChequeDet.GetCellDisplayRectangle(0, -1, true);
            rect.Y = 4;
            rect.X = 16;
            checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";

            checkboxHeader.Size = new Size(15, 15);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);
           
            dgvChequeDet.Controls.Add(checkboxHeader);

            //rrg
        }
        private void mthChkNavigationButton()
        {
            this.btnNew.Enabled = false;
            this.btnEdit.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;

            if (ServiceType.ToUpper() != "VIEWER VERSION")
            {
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    if (this.pAddButton)
                    {
                        this.btnNew.Enabled = true;
                    }
                    if (this.pEditButton)
                    {
                        //if (this.txtBankName.Text == "")
                        //{
                        //    this.btnEdit.Enabled = false;
                        //}
                        //else
                        //{
                        //    this.btnEdit.Enabled = true;
                        //}

                    }
                    if (this.pDeleteButton)
                    {
                        //this.btnDelete.Enabled = true;
                    }
                    btnLogout.Enabled = true;

                }
                else
                {
                    this.btnSave.Enabled = true;
                    this.btnCancel.Enabled = true;
                    //btnLogout.Enabled = false;
                }
                //if (this.dsGrd.Tables[0].Rows.Count == 0 && this.pAddMode == false && this.pEditMode == false)
                //{
                //    this.btnEdit.Enabled = false;
                //    this.btnDelete.Enabled = false;
                //}
            }
        }
        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
            if (this.pEditMode)
            {
                vEnabled = true;
                this.dgvChequeDet.Columns["colStatus"].ReadOnly = false;
             //   this.dgvChequeDet.Columns["colSelect"].ReadOnly = false;
             

            }

            //this.txtLeaftcnt.Enabled = vEnabled;
            //this.txtStartNo.Enabled = vEnabled;
            //this.dtpActDt.Enabled = vEnabled;
            //this.dtpDeAct.Enabled = vEnabled;
            //this.btnTrnSeries.Enabled = vEnabled;

        }

        private void mthControlSet()
        {
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                //this.btnLocNm.Image = Image.FromFile(fName);
                //this.btnBankName.Image = Image.FromFile(fName);
            }


        }
        private void SetFormColor()
        {
            string colorCode = string.Empty;
            Color myColor = Color.Coral;

            if (vDsCommon.Tables["company"].Rows.Count > 0)
            {
                colorCode = vDsCommon.Tables["company"].Rows[0]["vcolor"].ToString();
            }

            if (vDsCommon.Tables["company"].Rows.Count > 0)
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                myColor = Color.FromArgb(Convert.ToInt32(colorCode.Trim()));
            }
            this.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);
            foreach (Control ctrl in this.Controls)
            {
                //ctrl.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);
            }


        }
        private void mthDsCommon()
        {
            string vL_Yn = "";
            vDsCommon = new DataSet();
            DataTable company = new DataTable();
            company.TableName = "company";
            vSqlStr = "Select * From vudyog..Co_Mast where CompId=" + this.pCompId.ToString();
            company = oDataAccess.GetDataTable(vSqlStr, null, 25);
            if (vL_Yn == "")
            {
                vL_Yn = ((DateTime)company.Rows[0]["Sta_Dt"]).Year.ToString().Trim() + "-" + ((DateTime)company.Rows[0]["End_Dt"]).Year.ToString().Trim();
            }
            vDsCommon.Tables.Add(company);
            vDsCommon.Tables[0].TableName = "company";
            DataTable tblCoAdditional = new DataTable();
            tblCoAdditional.TableName = "coadditional";
            vSqlStr = "Select Top 1 * From Manufact";
            tblCoAdditional = oDataAccess.GetDataTable(vSqlStr, null, 25);
            vDsCommon.Tables.Add(tblCoAdditional);
            vDsCommon.Tables[1].TableName = "coadditional";

        }

        private void mthBindClear()
        {
            //this.txtBankName.Text = "";
            //this.txtLocNm.Text = "";
            //this.txtLeaftcnt.Text = "";
            //this.txtStartNo.Text = "";
            //this.txtEndNo.Text = "";
            //this.txtTrnSeries.Text = "";

        }
        private void SetMenuRights()
        {
            DataSet dsMenu = new DataSet();
            DataSet dsRights = new DataSet();
            this.pPApplRange = this.pPApplRange.Replace("^", "");
            string strSQL = "select padname,barname,range from com_menu where range =" + this.pPApplRange;
            dsMenu = oDataAccess.GetDataSet(strSQL, null, vTimeOut);
            if (dsMenu != null)
            {
                if (dsMenu.Tables[0].Rows.Count > 0)
                {
                    string padName = "";
                    string barName = "";
                    padName = dsMenu.Tables[0].Rows[0]["padname"].ToString();
                    barName = dsMenu.Tables[0].Rows[0]["barname"].ToString();
                    strSQL = "select padname,barname,dbo.func_decoder(rights,'F') as rights from ";
                    strSQL += "userrights where padname ='" + padName.Trim() + "' and barname ='" + barName + "' and range = " + this.pPApplRange;
                    strSQL += "and dbo.func_decoder([user],'T') ='" + this.pAppUerName.Trim() + "'";

                }
            }
            dsRights = oDataAccess.GetDataSet(strSQL, null, vTimeOut);


            if (dsRights != null)
            {
                string rights = dsRights.Tables[0].Rows[0]["rights"].ToString();
                int len = rights.Length;
                string newString = "";
                ArrayList rArray = new ArrayList();

                while (len > 2)
                {
                    newString = rights.Substring(0, 2);
                    rights = rights.Substring(2);
                    rArray.Add(newString);
                    len = rights.Length;
                }
                rArray.Add(rights);

                this.pAddButton = (rArray[0].ToString().Trim() == "IY" ? true : false);
                this.pEditButton = (rArray[1].ToString().Trim() == "CY" ? true : false);
                this.pDeleteButton = (rArray[2].ToString().Trim() == "DY" ? true : false);
                this.pPrintButton = (rArray[3].ToString().Trim() == "PY" ? true : false);
            }

        }
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string vSqlStr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udCheueManagementMaster.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            vSqlStr = "Set Dateformat DMY insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";  //Modified by Priyanka B on 14022018 for Bug-31240
            oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string getFileName(string fname)
        {
            string DfilePath, filename, FilePath;
            string date = DateTime.Now.ToString("dd/MM/yy") + "_" + DateTime.Now.ToString("hh:mm:ss tt");

            FolderBrowserDialog vFolderBrowserDialog1 = new FolderBrowserDialog();
            vFolderBrowserDialog1.ShowDialog();
            DfilePath = vFolderBrowserDialog1.SelectedPath;

            progressBar1.Value = 0;
            progressBar1.Value = 10;
            lblProgress.Visible = true;
            progressBar1.Visible = true;

            filename = fname + date;
            filename = filename.Replace(".", "").Replace("-", "").Replace("/", "").Replace(":", "").Replace("__", "_").Replace(" ", "");
            //  FilePath = DfilePath + filename+".xls";
            FilePath = DfilePath + filename;
            FilePath = FilePath.Replace("\\\\", "\\");
            return FilePath;
        }

        public void ExportToExcel(DataTable dt_record, string excelFilename)
        {
            try
            {
                lblProgress.Text = "Opening Excel Application.....";
                progressBar1.Value = 30;
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

                progressBar1.Value = 40;
                Microsoft.Office.Interop.Excel.Workbook wBook;

                progressBar1.Value = 50;
                Microsoft.Office.Interop.Excel.Worksheet wSheet;

                progressBar1.Value = 60;
                wBook = excel.Workbooks.Add(System.Reflection.Missing.Value);

                progressBar1.Value = 70;
                wSheet = (Microsoft.Office.Interop.Excel.Worksheet)wBook.ActiveSheet;

                progressBar1.Value = 80;
                System.Data.DataTable dt = dt_record;
                System.Data.DataColumn dc = new DataColumn();

                progressBar1.Value = 90;
                int colIndex = 0;
                float rowIndex = 1;
                progressBar1.Value = 100;

                foreach (DataColumn dcol in dt.Columns)
                {
                    colIndex = colIndex + 1;
                    excel.Cells[rowIndex, colIndex] = dcol.ColumnName;
                }

                progressBar1.Value = 0;
                rowIndex += 1;
                float nrow = 1;
                excel.Columns.NumberFormat = "@";
                foreach (DataRow drow in dt.Rows)
                {
                    colIndex = 0;
                    foreach (DataColumn dcol in dt.Columns)
                    {
                        colIndex = colIndex + 1;
                        excel.Cells[rowIndex, colIndex] = drow[dcol.ColumnName].ToString();

                    }
                    float aa = nrow == dt.Rows.Count ? 100.00f : (nrow / dt.Rows.Count) * 100;

                    progressBar1.Value = (Int32)aa;
                    progressBar1.Refresh();
                    progressBar1.Refresh();
                    lblProgress.Text = "Read Records: " + nrow.ToString() + "/" + dt.Rows.Count;

                    nrow = nrow + 1;
                    rowIndex = rowIndex + 1;
                }
                progressBar1.Value = 100;
                progressBar1.Refresh();



                object misValue = System.Reflection.Missing.Value;
                excel.Columns.AutoFit();
                excel.ActiveWorkbook.SaveAs(excelFilename + ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                excel.ActiveWorkbook.Saved = true;

                progressBar1.Visible = false;
                lblProgress.Visible = false;

                MessageBox.Show("Your excel file exported successfully at " + excelFilename + ".xls", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                MessageBox.Show("Excel Application not available", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                lblProgress.Visible = false;
            }
        }

        public DataTable getGridRecord()
        {
            string[] selectedColumns = new[] { "Chq_No", "Status", "TrnNm", "PartyNm" };

            DataTable dtMainRecord = pTblMain;
            DataRow[] result = dtMainRecord.Select("Sel=1");

            if (result.Length > 0)
            {
                dtMainRecord = result.CopyToDataTable();
                DataTable dt = new DataView(dtMainRecord).ToTable(false, selectedColumns);

                dt.Columns["Chq_No"].ColumnName = "Cheque Number";
                dt.Columns["Status"].ColumnName = "Status";
                dt.Columns["TrnNm"].ColumnName = "Transaction Name";
                dt.Columns["PartyNm"].ColumnName = "Party Name";
                
                return dt;
            }
            else
            {
                return null;
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            int count = 0;
            CheckBox headerBox = ((CheckBox)dgvChequeDet.Controls.Find("checkboxHeader", true)[0]);

            string status = headerBox.CheckState.ToString();

            if (status == "Checked")
            {
                foreach (DataRow row in pTblMain.Rows)
                {
                    if (row["Sel"].ToString() == "False")
                    {
                        row["sel"] = headerBox.Checked;
                    }
                }
            }
            else
            {
                foreach (DataRow row in pTblMain.Rows)
                {
                    if (row["Sel"].ToString() == "True")
                    {
                        row["sel"] = headerBox.Checked;
                    }
                }
            }

        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            int count = dgvChequeDet.RowCount;
            string file_path;

            if (count != 0)
            {
                DataTable dtRecord = getGridRecord();
                if (dtRecord != null)
                {
                    file_path = getFileName("\\ChequeBookDetails_");
                    progressBar1.Value = 20;
                    ExportToExcel(dtRecord, file_path);


                }
                else
                {
                    MessageBox.Show("Please select atleast one record.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }

            }
            else
            {
                MessageBox.Show("Record not found to export", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        public string pBook_Id
        {
            get
            {
                return vBook_Id;
            }
            set
            {
                vBook_Id = value;
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

    }
}
