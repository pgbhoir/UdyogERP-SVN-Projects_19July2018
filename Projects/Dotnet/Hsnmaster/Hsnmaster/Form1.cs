using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Udyog.Library.Common;

namespace Hsnmaster
{
    public partial class frmHSNCodeMast : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DataSet dsMain;
        string SqlStr = string.Empty;
        string vMainField = "Recid", vMainTblNm = "HsnCode_vw", vMainFldVal = "";
        String cAppPId, cAppName;
        string startupPath = string.Empty;
        string ServiceType = string.Empty;
        private RegisterMeInfo m_info;
        DataTable childTbl = new DataTable();
        DataTable DtValidin = new DataTable();
        DataTable DeletedRec = new DataTable();
        int DeletedRecIndex = 0,save=0;
        double igst_o, cgst_o, sgst_o;
        public frmHSNCodeMast(string[] args)
        {
            InitializeComponent();
            this.pDisableCloseBtn = true;  /* close disable  */
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "HSN Code Master";
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
            DeletedRec.Columns.Add("PrimaryID", typeof(int));
        }

        private void FormSetting()
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Enabled = false;

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

        }

        private void GetStates()
        {

            DataTable ldt = new DataTable();
            string strSQL = "select gst_state_code as state_code,state From state Where Isnull(Gst_state_code,'')<>'' Order by State ";
            ldt = oDataAccess.GetDataSet(strSQL, null, 20).Tables[0];
            this.cboStateList.DataSource = ldt;
            this.cboStateList.DisplayMember = "State";
            this.cboStateList.ValueMember = "state_code";
            this.txtStateCode.Text = this.cboStateList.SelectedValue.ToString();
            //this.cboStateList.Items.Clear();
            //for (int i = 0; i < ldt.Rows.Count; i++)
            //{

            //    this.cboStateList.Items.Add(new { Text = ldt.Rows[i]["State"].ToString(), Value = ldt.Rows[i]["gst_state_code"].ToString() });
            //}             
        }

        private void GetChildRecords(string keyValue)
        {
            childTbl.Rows.Clear();
            string strSQL = "select * From hsncodemast Where Hsncode+'-'+state_code='" + keyValue + "' order by Activefrom";
            childTbl = oDataAccess.GetDataSet(strSQL, null, 20).Tables[0];
            this.dgvGSTRate.RefreshEdit();
            this.dgvGSTRate.Refresh();
        }
        private void AssignSourceToGrid()
        {
            dgvGSTRate.AutoGenerateColumns = false;
            dgvGSTRate.DataSource = childTbl;
            dgvGSTRate.Columns[0].DataPropertyName = "CGSTPer";
            dgvGSTRate.Columns[1].DataPropertyName = "SGSTPer";
            dgvGSTRate.Columns[2].DataPropertyName = "IGSTPer";
            dgvGSTRate.Columns[3].DataPropertyName = "ActiveFrom";
            dgvGSTRate.Columns[4].DataPropertyName = "PrimaryID";
            dgvGSTRate.Columns[5].DataPropertyName = "exempted";

        }
        private void SetMenuRights()
        {
            DataSet dsMenu = new DataSet();
            DataSet dsRights = new DataSet();
            this.pPApplRange = this.pPApplRange.Replace("^", "");
            string strSQL = "select padname,barname,range from com_menu where range =" + this.pPApplRange;
            dsMenu = oDataAccess.GetDataSet(strSQL, null, 20);
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
            dsRights = oDataAccess.GetDataSet(strSQL, null, 20);


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
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            //cAppName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            cAppName = "UdHSNCodeMast.exe";
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

        private string GetRegisterDetails()
        {
            string retvalue = string.Empty;

            UdyogRegister reg = new UdyogRegister();
            m_info = reg.RegistrationInfo;

            retvalue = m_info.CompanyName;
            retvalue = retvalue + "^" + m_info.InstallationAddress.Line1;
            retvalue = retvalue + "^" + m_info.InstallationAddress.Line2;
            retvalue = retvalue + "^" + m_info.InstallationAddress.Line3;
            retvalue = retvalue + "^" + m_info.InstallationAddress.Location;
            retvalue = retvalue + "^" + m_info.InstallationAddress.Zip;
            retvalue = retvalue + "^" + m_info.InstallationAddress.EMail;
            retvalue = retvalue + "^" + m_info.ServiceCenterCode;
            retvalue = retvalue + "^" + m_info.DBServerIP;
            retvalue = retvalue + "^" + m_info.DBServerName;
            retvalue = retvalue + "^" + m_info.ApplicationServerIP;
            retvalue = retvalue + "^" + m_info.ApplicationServerName;
            retvalue = retvalue + "^" + m_info.MACId;
            retvalue = retvalue + "^" + m_info.MaximumNumberOfCompaniesAllowed;
            retvalue = retvalue + "^" + m_info.MaximumNumberOfUsersAllowed;
            retvalue = retvalue + "^" + m_info.ServiceType;
            retvalue = retvalue + "^" + m_info.ProductId;

            return retvalue;
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
        private void mthEnableDisableFormControls()
        {
            if (!this.pAddMode && !this.pEditMode)
            {
                this.txtHSNCode.Enabled = false;
                this.txtStateCode.Enabled = false;
                this.cboStateList.Enabled = false;
                this.btnAdd.Visible = false;
                this.btnRemove.Visible = false;
                this.btnStateList.Visible = false;
                this.chkSelect.Enabled = false;
                this.dgvGSTRate.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
                this.chkSelect.Visible = false;
                //this.btnHSNCode.Enabled = false;
            }
            else
            {
                if (this.pAddMode)
                {
                    this.txtHSNCode.Enabled = true;
                    this.txtStateCode.Enabled = true;
                    this.chkSelect.Enabled = true;
                    this.chkSelect.Visible = true;
                    //this.btnHSNCode.Enabled = true;
                    this.dgvGSTRate.Columns[4].ReadOnly = false;
                }
                if (this.pEditMode)
                {
                    this.txtHSNCode.Enabled = false;
                    this.txtStateCode.Enabled = false;
                    this.chkSelect.Visible = false;
                    this.chkSelect.Enabled = false;
                    //this.btnHSNCode.Enabled = false;
                    this.dgvGSTRate.Columns[4].ReadOnly = true;
                }
                this.dgvGSTRate.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
                this.cboStateList.Enabled = true;
                this.btnRemove.Visible = true;
            }
        }

        private void mthBindClear()
        {
            this.txtHSNCode.DataBindings.Clear();
            this.txtStateCode.DataBindings.Clear();
            this.cboStateList.Text = string.Empty;
            this.txtValid.Text = "";
            this.cboStateList.DataSource = null;
            this.cboStateList.Items.Clear();
        }

        private void mthBindData()
        {
            if (dsMain.Tables.Count == 0)
            {
                return;
            }

            this.txtHSNCode.DataBindings.Add("Text", dsMain.Tables[0], "HSNCODE");
            this.txtStateCode.DataBindings.Add("Text", dsMain.Tables[0], "STATE_CODE");

            if (!this.pAddMode)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    //this.cboStateList.Items.Clear();
                    if (this.cboStateList.DataSource == null)
                        this.cboStateList.Items.Clear();

                    this.cboStateList.DataSource = null;
                    this.cboStateList.Items.Add(dsMain.Tables[0].Rows[0]["State"].ToString().Trim());
                    this.cboStateList.SelectedValue = dsMain.Tables[0].Rows[0]["State"].ToString().Trim();
                    this.cboStateList.SelectedIndex = 0;

                }
            }
            //this.cboStateList.Text = dsMain.Tables[0].Rows[0]["State"].ToString();

            //this.txtState.DataBindings.Add("Text", dsMain.Tables[0], "STATE");
        }
        private void mthView()
        {
            this.mthBindClear();
            this.mthBindData();

        }
        private void mthChkNavigationButton()
        {
            DataSet dsTemp = new DataSet();
            this.btnForward.Enabled = false;
            this.btnLast.Enabled = false;
            this.btnFirst.Enabled = false;
            this.btnBack.Enabled = false;
            this.btnLocate.Enabled = false;
            btnEdit.Enabled = false;
            Boolean vBtnAdd, vBtnEdit, vBtnDelete, vBtnPrint;
            if (ServiceType.ToUpper() != "VIEWER VERSION")//Added by Archana K. on 17/05/13 for Bug-7899
            {
                if (dsMain.Tables[0].Rows.Count == 0)
                {
                    if (this.pAddMode == false && this.pEditMode == false)
                    {
                        this.btnCancel.Enabled = false;
                        this.btnSave.Enabled = false;
                        //this.btnGradeName.Enabled = false;

                        vBtnAdd = true;
                        vBtnDelete = false;
                        vBtnEdit = false;
                        vBtnPrint = false;
                        this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
                    }
                    return;
                }
            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    vMainFldVal = dsMain.Tables[0].Rows[0]["Recid"].ToString().Trim();

                    SqlStr = "select Recid from " + vMainTblNm + " Where Recid>" + vMainFldVal;

                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, 20);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnForward.Enabled = true;
                        this.btnLast.Enabled = true;
                    }
                    SqlStr = "select Recid from " + vMainTblNm + " Where Recid<" + vMainFldVal;

                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, 20);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnBack.Enabled = true;
                        this.btnFirst.Enabled = true;
                    }
                }

            }

            vBtnAdd = false;
            vBtnDelete = false;
            vBtnEdit = false;
            vBtnPrint = false;

            if (ServiceType.ToUpper() != "VIEWER VERSION")
            {
                if (this.btnForward.Enabled == true || this.btnBack.Enabled == true || (this.pAddMode == false && this.pEditMode == false))
                {
                    vBtnAdd = true;
                    if (dsMain.Tables[0].Rows.Count > 0)
                    {
                        vBtnDelete = true;
                        vBtnEdit = true;
                        vBtnPrint = true;
                        //this.btnGradeName.Enabled = true;
                    }
                }
                this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
            }
        }
        private void mthChkAEDPButton(Boolean vBtnAdd, Boolean vBtnEdit, Boolean vBtnDelete, Boolean vBtnPrint)
        {
            this.btnNew.Enabled = false;
            this.btnEdit.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnPreview.Enabled = false;
            this.btnPrint.Enabled = false;
            this.btnLocate.Enabled = false;

            if (vBtnAdd && this.pAddButton)
            {
                this.btnNew.Enabled = true;
            }
            if (vBtnEdit && this.pEditButton)
            {
                this.btnEdit.Enabled = true;
            }
            if (vBtnDelete && this.pDeleteButton)
            {
                this.btnDelete.Enabled = true;
            }
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
            this.btnLogout.Enabled = false;
            this.btnEmail.Enabled = false;
            this.btnLocate.Enabled = false;
            if (this.btnNew.Enabled == false && this.btnEdit.Enabled == false)
            {
                this.btnSave.Enabled = true;
                this.btnCancel.Enabled = true;
            }
            if (this.btnSave.Enabled == false)
            {
                this.btnLogout.Enabled = true;
            }
        }

        private void frmHSNCodeMast_Load(object sender, EventArgs e)
        {
            dgvGSTRate.Columns[0].ReadOnly = true;
            dgvGSTRate.Columns[1].ReadOnly = true;
            dgvGSTRate.Columns[2].DisplayIndex = 0;

            this.pAddMode = false;
            this.pEditMode = false;
            this.FormSetting();
            this.SetMenuRights();
            startupPath = Application.StartupPath;
            //startupPath = @"E:\U3\VUDYOGGST\";
            GetInfo.iniFile ini = new GetInfo.iniFile(startupPath + "\\" + "Visudyog.ini");
            string appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);

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
                string[] objRegisterMe = (this.GetRegisterDetails()).Split('^');
            }
            this.btnLast.PerformClick();
            this.AssignSourceToGrid();
            this.mInsertProcessIdRecord();//Check Application open or not
            this.SetFormColor();
            string appPath = Application.ExecutablePath;

            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }

            string fName = appPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnHSNCode.Image = Image.FromFile(fName);
            }
        }
        private void GoToRecord(string cond)
        {
            //vMainFldVal = string.Empty;
            //if (dsMain.Tables[0].Rows.Count > 0)
            //    vMainFldVal = dsMain.Tables[0].Rows[0]["hsncode"].ToString() + "-" + dsMain.Tables[0].Rows[0]["state_code"].ToString();

            SqlStr = "Select top 1 * from " + vMainTblNm + " Where hsncode+'-'+state_code='" + cond + "'";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            this.mthView();
            this.mthChkNavigationButton();
            this.GetChildRecords(cond);
        }
        private void btnFirst_Click(object sender, EventArgs e)
        {
            vMainFldVal = string.Empty;
            if (dsMain.Tables[0].Rows.Count > 0)
                vMainFldVal = dsMain.Tables[0].Rows[0]["Recid"].ToString();

            SqlStr = "Select top 1 * from " + vMainTblNm + "  order by " + vMainField;
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            this.mthView();
            this.mthChkNavigationButton();

            string keyValue = string.Empty;
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                keyValue = dsMain.Tables[0].Rows[0]["hsncode"].ToString() + "-" + dsMain.Tables[0].Rows[0]["state_code"].ToString();
            }

            this.GetChildRecords(keyValue);
            this.AssignSourceToGrid();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            vMainFldVal = string.Empty;
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                vMainFldVal = dsMain.Tables[0].Rows[0]["Recid"].ToString();
            }
            //vMainFldVal = dsMain.Tables[0].Rows[0]["hsncode"].ToString() + "-" + dsMain.Tables[0].Rows[0]["state_code"].ToString();
            SqlStr = "Select top 1 * from hsncode_vw Where hsncode+'-'+state_code=(select Top 1 hsncode+'-'+state_code  from Hsncodemast where Recid <" + vMainFldVal + " order by " + vMainField + " desc)";

            //SqlStr = "Select top 1 * from " + vMainTblNm + " Where hsncode+'-'+state_code <'" + vMainFldVal + "' order by " + vMainField + " desc";
            //SqlStr = "Select top 1 * from " + vMainTblNm + " Where Recid <" + vMainFldVal + " order by " + vMainField + " desc";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

            string keyValue = string.Empty;
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                keyValue = dsMain.Tables[0].Rows[0]["hsncode"].ToString() + "-" + dsMain.Tables[0].Rows[0]["state_code"].ToString();
            }

            this.mthView();
            this.mthChkNavigationButton();
            this.GetChildRecords(keyValue);
            this.AssignSourceToGrid();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            vMainFldVal = string.Empty;
            if (dsMain.Tables[0].Rows.Count > 0)
                vMainFldVal = dsMain.Tables[0].Rows[0]["Recid"].ToString();

            SqlStr = "Select top 1 * from " + vMainTblNm + " Where Recid >" + vMainFldVal + " order by " + vMainField;
            //SqlStr = "Select top 1 * from hsncode_vw Where hsncode+'-'+state_code=(select Top 1 hsncode+'-'+state_code  from hsncode_vw where Recid >" + vMainFldVal + " order by " + vMainField + " desc)";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            this.mthView();
            this.mthChkNavigationButton();
            string keyValue = string.Empty;
            if (dsMain.Tables[0].Rows.Count > 0)
            {
                keyValue = dsMain.Tables[0].Rows[0]["hsncode"].ToString() + "-" + dsMain.Tables[0].Rows[0]["state_code"].ToString();
            }
            this.GetChildRecords(keyValue);
            this.AssignSourceToGrid();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (ServiceType.ToUpper() == "VIEWER VERSION")
            {
                this.btnNew.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCancel.Enabled = false;
                this.btnDelete.Enabled = false;
                //                this.groupBox1.Enabled = false;
                this.btnPreview.Enabled = false;
                this.btnPrint.Enabled = false;
            }
            else
                this.mthEnableDisableFormControls();

            SqlStr = "Select top 1 * from " + vMainTblNm + " Where hsncode+'-'+state_code in (Select Top 1 hsncode+'-'+state_code from " + vMainTblNm + " Order by hsncode desc,state_code desc )";
            dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

            this.mthView();
            this.mthChkNavigationButton();
            string keyValue = string.Empty;

            if (dsMain.Tables[0].Rows.Count > 0)
                keyValue = dsMain.Tables[0].Rows[0]["hsncode"].ToString() + "-" + dsMain.Tables[0].Rows[0]["state_code"].ToString();

            this.GetChildRecords(keyValue);

            if (dsMain.Tables[0].Rows.Count == 0)
            {
                this.btnEmail.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnEdit.Enabled = false;
            }
            this.AssignSourceToGrid();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            chkSelect.Checked = false;
            this.pAddMode = true;
            this.pEditMode = false;
            this.mthBindClear();
            this.GetStates();
            int Recid = 0;

            if (dsMain.Tables[0].Rows.Count > 0)
            {
                Recid = Convert.ToInt32(dsMain.Tables[0].Rows[0]["Recid"]);
                dsMain.Tables[0].Rows.RemoveAt(0);
                dsMain.Tables[0].AcceptChanges();
            }
            childTbl.Rows.Clear();
            Recid = Recid + 1;

            DataRow drCurrent;
            drCurrent = dsMain.Tables[0].NewRow();
            drCurrent["Recid"] = Recid;
            dsMain.Tables[0].Rows.Add(drCurrent);

            this.AddRecord();


            this.mthBindData();
            this.mthEnableDisableFormControls();
            //dsMain.Tables[0].Rows[0].BeginEdit();
            dsMain.Tables[0].Rows[0].EndEdit();

            this.mthChkNavigationButton();
            this.dgvGSTRate.RefreshEdit();
            this.dgvGSTRate.Refresh();
            this.txtHSNCode.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

            string state_code, hsn_code, s_date;
            s_date = null;
            foreach (DataRow item in childTbl.Rows)
            {
                try
                {
                    s_date = item["Activefrom"].ToString();
                }
                catch (Exception)
                {

                }

            }
            this.btnHSNCode.Focus();
            bool lccommit = true;
            this.Refresh();
            this.txtValid.Text = "";
            string vSaveString = string.Empty;

            if (!this.CheckValidation())
            {
                return;
            }
           
            //if (childTbl.Rows.Count > 0)
            //{
            //    string hsnid = "";
            //    foreach (DataRow dr in childTbl.Rows)
            //    {
            //        if (dr.RowState != DataRowState.Deleted)
            //        {
            //            if (hsnid == "")
            //            {
            //                hsnid = dr["hsnid"].ToString();
            //            }
            //            dr["HsnCode"] = this.txtHSNCode.Text;
            //            dr["State_Code"] = this.txtStateCode.Text;
            //            dr["hsnid"] = hsnid;
            //        }
            //    }
            //}

            try
            {
                foreach (DataRow item in childTbl.Rows)
                {
                    //   MessageBox.Show(item["exempted"].ToString());
                    SqlStr = string.Empty;
                    int updated = 0;
                    item.EndEdit();
                    switch (item.RowState)
                    {
                        case DataRowState.Added:
                            state_code = txtStateCode.Text.ToString();
                            hsn_code = txtHSNCode.Text.ToString();

                            int a = check_tr_save(state_code, hsn_code, s_date);
                            if (a == 1)
                            {
                                if (DtValidin.Rows.Count > 0 && DtValidin != null)
                                {
                                    int DupliChk = 0;
                                    DataTable Duplicate = new DataTable();
                                    string DupliCheck = "";
                                    string DuplicateRecMessage = "";
                                    foreach (DataRow StCode in DtValidin.Rows)
                                    {
                                        string Stcode = StCode["GSTSCode"].ToString();

                                        SqlStr = "Set dateformat mdy SELECT * from Hsncodemast where State_code ='" + Stcode + "' and Activefrom ='";
                                        SqlStr = SqlStr + Convert.ToDateTime(item["Activefrom"]).ToString("MM / dd / yyyy") + "' and hsnid=" + Convert.ToString(item["hsnid"]);
                                        DupliChk = oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, false);

                                        if (DupliChk != 0)
                                        {
                                            DupliCheck = DupliCheck + StCode[0].ToString() + ",";
                                        }
                                    }
                                    if (DupliCheck.Length > 0)
                                    {
                                        DupliCheck = DupliCheck.Substring(0, DupliCheck.Length - 1);
                                        SqlStr = "select [State] from [STATE] where GST_STATE_CODE IN (" + DupliCheck + ") Order By [STATE]";
                                        Duplicate = oDataAccess.GetDataTable(SqlStr, null, 20);

                                        int ix = 1;
                                        foreach (DataRow Dr in Duplicate.Rows)
                                        {
                                            DuplicateRecMessage = DuplicateRecMessage + Environment.NewLine + ix.ToString() + " ." + Dr["State"].ToString();
                                        }
                                    }

                                    if (DuplicateRecMessage == "")
                                    {
                                        foreach (DataRow StCode in DtValidin.Rows)
                                        {
                                            oDataAccess.BeginTransaction();
                                            string Stcode = StCode["GSTSCode"].ToString();
                                            SqlStr = string.Empty;
                                            item["Hsncode"] = this.txtHSNCode.Text;
                                            item["State_code"] = Stcode;
                                            //SqlStr = SqlStr + "Set dateformat mdy Insert Into Hsncodemast (HsnCode,State_code,SGSTPer,CGSTPer,IGSTPer,Activefrom,hsnid,exempted)";  //Commented by Priyanka B on 28122017 for Bug-30849
                                            SqlStr = SqlStr + "Set dateformat mdy Insert Into Hsncodemast (HsnCode,State_code,SGSTPer,CGSTPer,IGSTPer,Activefrom,hsnid,exempted,DataImport,DataExport)";  //Modified by Priyanka B on 28122017 for Bug-30849
                                            SqlStr = SqlStr + " Values ('" + Convert.ToString(item["Hsncode"]) + "','" + Convert.ToString(item["State_code"]) + "',"
                                                + Convert.ToString(item["SGSTPer"]) + "," + Convert.ToString(item["CGSTPer"]) + "," + Convert.ToString(item["IGSTPer"])
                                                    + ",'" + Convert.ToDateTime(item["Activefrom"]).ToString("MM/dd/yyyy") + "'," + Convert.ToString(item["hsnid"]) + ",'" + Convert.ToBoolean(item["exempted"])
                                                    + "','','')";  //Modified by Priyanka B on 28122017 for Bug-30849
                                            //+ "')";  //Commented by Priyanka B on 28122017 for Bug-30849
                                            updated = oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, false);
                                            item["PrimaryId"] = updated;
                                            oDataAccess.CommitTransaction();
                                        }
                                    }
                                    else
                                    {//1

                                        DialogResult Result = MessageBox.Show("HSN Code is already exists in the master for " + DuplicateRecMessage + Environment.NewLine + "Do you want to update..?", this.pPApplName, MessageBoxButtons.YesNo);
                                        if (Result == DialogResult.Yes)
                                        {
                                            oDataAccess.BeginTransaction();
                                            foreach (DataRow StCode in DtValidin.Rows)
                                            {
                                                string Stcode = StCode["GSTSCode"].ToString();

                                                SqlStr = "Set dateformat mdy SELECT * from Hsncodemast where HsnCode='" + this.txtHSNCode.Text + "' and State_code ='" + Stcode + "' and Activefrom ='";
                                                SqlStr = SqlStr + Convert.ToDateTime(item["Activefrom"]).ToString("MM / dd / yyyy") + "' and hsnid=" + Convert.ToString(item["hsnid"]);
                                                DupliChk = oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, false);

                                                if (DupliChk != 0)
                                                {
                                                    item["State_code"] = Stcode;
                                                    SqlStr = "Set dateformat mdy Update Hsncodemast set HsnCode='" + Convert.ToString(item["Hsncode"]) + "',State_code='" + Convert.ToString(item["State_code"])
                                                    + "',SGSTPer=" + Convert.ToString(item["SGSTPer"]) + ",CGSTPer=" + Convert.ToString(item["CGSTPer"]) + ",IGSTPer=" + Convert.ToString(item["IGSTPer"])
                                                    + ",Activefrom='" + Convert.ToDateTime(item["Activefrom"]).ToString("MM/dd/yyyy") + "',hsnid=" + Convert.ToString(item["hsnid"]) + ",exempted='" + Convert.ToBoolean(item["exempted"]) + "'"
                                                    + ",DataExport='',DataImport=''"  //Added by Priyanka B on 28122017 for Bug-30849
                                                    + " Where PrimaryId=" + DupliChk;
                                                    updated = oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                                                }
                                                else
                                                {//rrg
                                                    SqlStr = string.Empty;
                                                    item["Hsncode"] = this.txtHSNCode.Text;
                                                    item["State_code"] = Stcode;
                                                    //SqlStr = SqlStr + "Set dateformat mdy Insert Into Hsncodemast (HsnCode,State_code,SGSTPer,CGSTPer,IGSTPer,Activefrom,hsnid,exempted)";   //Commented by Priyanka B on 28122017 for Bug-30849
                                                    SqlStr = SqlStr + "Set dateformat mdy Insert Into Hsncodemast (HsnCode,State_code,SGSTPer,CGSTPer,IGSTPer,Activefrom,hsnid,exempted,DataImport,DataExport)";  //Modified by Priyanka B on 28122017 for Bug-30849
                                                    SqlStr = SqlStr + " Values ('" + Convert.ToString(item["Hsncode"]) + "','" + Convert.ToString(item["State_code"]) + "',"
                                                        + Convert.ToString(item["SGSTPer"]) + "," + Convert.ToString(item["CGSTPer"]) + "," + Convert.ToString(item["IGSTPer"])
                                                            + ",'" + Convert.ToDateTime(item["Activefrom"]).ToString("MM/dd/yyyy") + "'," + Convert.ToString(item["hsnid"]) + ",'" + Convert.ToBoolean(item["exempted"])
                                                    + "','','')";  //Modified by Priyanka B on 28122017 for Bug-30849
                                                    //+"')";  //Commented by Priyanka B on 28122017 for Bug-30849
                                                    updated = oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, false);
                                                    item["PrimaryId"] = updated;
                                                }
                                            }
                                            oDataAccess.CommitTransaction();
                                        }
                                        else
                                        {
                                            return;
                                        }
                                        //1
                                    }


                                }
                                else
                                {
                                    string Stcode = txtStateCode.Text;
                                    int DupliChk = 0;
                                    SqlStr = "Set dateformat mdy SELECT * from Hsncodemast where State_code ='" + Stcode + "' and Activefrom ='";
                                    SqlStr = SqlStr + Convert.ToDateTime(item["Activefrom"]).ToString("MM / dd / yyyy") + "' and hsnid=" + Convert.ToString(item["hsnid"]);
                                    DupliChk = oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, false);

                                    if (DupliChk == 0)
                                    {
                                        oDataAccess.BeginTransaction();
                                        SqlStr = string.Empty;
                                        item["Hsncode"] = this.txtHSNCode.Text;
                                        item["State_code"] = Stcode;
                                        //SqlStr = SqlStr + "Set dateformat mdy Insert Into Hsncodemast (HsnCode,State_code,SGSTPer,CGSTPer,IGSTPer,Activefrom,hsnid,exempted)";   //Commented by Priyanka B on 28122017 for Bug-30849
                                        SqlStr = "Set dateformat mdy Insert Into Hsncodemast (HsnCode,State_code,SGSTPer,CGSTPer,IGSTPer,Activefrom,hsnid,exempted,DataImport,DataExport)";   //Modified by Priyanka B on 28122017 for Bug-30849
                                        SqlStr = SqlStr + " Values ('" + Convert.ToString(item["Hsncode"]) + "','" + Convert.ToString(item["State_code"]) + "',"
                                            + Convert.ToString(item["SGSTPer"]) + "," + Convert.ToString(item["CGSTPer"]) + "," + Convert.ToString(item["IGSTPer"])
                                                + ",'" + Convert.ToDateTime(item["Activefrom"]).ToString("MM/dd/yyyy") + "'," + Convert.ToString(item["hsnid"]) + ",'" + Convert.ToBoolean(item["exempted"])
                                                + "','','')";  //Modified by Priyanka B on 28122017 for Bug-30849
                                                               //+"')";  //Commented by Priyanka B on 28122017 for Bug-30849
                                        updated = oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, false);
                                        item["PrimaryId"] = updated;
                                        SqlStr = " Update Hsn_master set DataExport='',DataImport='' Where Hsn_Code='" + Convert.ToString(item["Hsncode"]) + "' and hsn_id=" + Convert.ToString(item["hsnid"]);  //Added by Priyanka B on 28122017 for Bug-30849
                                        updated = oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                                        oDataAccess.CommitTransaction();
                                    }
                                    else
                                    {//2
                                        DialogResult Result = MessageBox.Show("HSN Code is already exists in the master..." + Environment.NewLine + "Do you want to update..?", this.pPApplName, MessageBoxButtons.YesNo);
                                        if (Result == DialogResult.Yes)
                                        {
                                            oDataAccess.BeginTransaction();
                                            item["State_code"] = Stcode;
                                            SqlStr = "Set dateformat mdy Update Hsncodemast set HsnCode='" + Convert.ToString(item["Hsncode"]) + "',State_code='" + Convert.ToString(item["State_code"])
                                            + "',SGSTPer=" + Convert.ToString(item["SGSTPer"]) + ",CGSTPer=" + Convert.ToString(item["CGSTPer"]) + ",IGSTPer=" + Convert.ToString(item["IGSTPer"])
                                            + ",Activefrom='" + Convert.ToDateTime(item["Activefrom"]).ToString("MM/dd/yyyy") + "',hsnid=" + Convert.ToString(item["hsnid"]) + ",exempted='" + Convert.ToBoolean(item["exempted"]) + "'"
                                            + ",DataExport='',DataImport=''"  //Added by Priyanka B on 28122017 for Bug-30849
                                            + " Where PrimaryId=" + DupliChk;
                                            SqlStr = SqlStr + " Update Hsn_master set DataExport='',DataImport='' Where Hsn_Code='" + Convert.ToString(item["Hsncode"]) + "' and hsn_id=" + Convert.ToString(item["hsnid"]);  //Added by Priyanka B on 28122017 for Bug-30849
                                            updated = oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                                            oDataAccess.CommitTransaction();
                                        }
                                        else
                                        {
                                            return;
                                        }



                                    }

                                }

                            }

                            else
                            {
                                MessageBox.Show("Transaction has been made against this HSN Code, Setting cannot be changed.",
                                  "HSN Code Master",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning // for Warning  
                                                         //MessageBoxIcon.Error // for Error 
                                                         //MessageBoxIcon.Information  // for Information
                                                         //MessageBoxIcon.Question // for Question
                                 );

                                return;
                            }
                            break;
                        case DataRowState.Deleted:
                            oDataAccess.BeginTransaction();
                            if (DeletedRec.Rows.Count > 0)
                            {
                                foreach (DataRow dr in DeletedRec.Rows)
                                {
                                    SqlStr = "Delete From Hsncodemast Where PrimaryId=" + Convert.ToString(dr["PrimaryId"]);
                                    updated = oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                                }
                                oDataAccess.CommitTransaction();
                            }
                            DeletedRec.Rows.Clear();
                            break;
                        case DataRowState.Detached:
                            oDataAccess.BeginTransaction();
                            break;
                        case DataRowState.Modified:
                            oDataAccess.BeginTransaction();
                            SqlStr = "Set dateformat mdy Update Hsncodemast set HsnCode='" + Convert.ToString(item["Hsncode"]) + "',State_code='" + Convert.ToString(item["State_code"])
                                + "',SGSTPer=" + Convert.ToString(item["SGSTPer"]) + ",CGSTPer=" + Convert.ToString(item["CGSTPer"]) + ",IGSTPer=" + Convert.ToString(item["IGSTPer"])
                                + ",Activefrom='" + Convert.ToDateTime(item["Activefrom"]).ToString("MM/dd/yyyy") + "',hsnid=" + Convert.ToString(item["hsnid"]) + ",exempted='" + Convert.ToBoolean(item["exempted"]) + "'"
                                + ",DataExport='',DataImport=''"  //Added by Priyanka B on 28122017 for Bug-30849
                                + " Where PrimaryId=" + Convert.ToString(item["PrimaryId"]);
                            SqlStr = SqlStr + " Update Hsn_master set DataExport='',DataImport='' Where Hsn_Code='" + Convert.ToString(item["Hsncode"]) + "' and hsn_id=" + Convert.ToString(item["hsnid"]);  //Added by Priyanka B on 28122017 for Bug-30849
                            updated = oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                            oDataAccess.CommitTransaction();
                            break;
                        case DataRowState.Unchanged:
                            //Added by Priyanka B on 08012018 for Bug-30849 Start
                            oDataAccess.BeginTransaction();
                            SqlStr = "Set dateformat mdy Update Hsncodemast set DataExport='',DataImport='' Where PrimaryId=" + Convert.ToString(item["PrimaryId"]);
                            SqlStr = SqlStr + " Update Hsn_master set DataExport='',DataImport='' Where Hsn_Code='" + Convert.ToString(item["Hsncode"]) + "' and hsn_id=" + Convert.ToString(item["hsnid"]);
                            updated = oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                            oDataAccess.CommitTransaction();
                            //Added by Priyanka B on 08012018 for Bug-30849 End
                            //lccommit = false;   //Commented by Priyanka B on 08012018 for Bug-30849
                            break;
                        default:
                            lccommit = false;
                            break;
                    }
                }
                if (lccommit == true)
                {
                    childTbl.AcceptChanges();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.pPApplName);
                oDataAccess.RollbackTransaction();
            }

            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();

            for (int i = 0; i < dgvGSTRate.RowCount; i++)
            {
                dgvGSTRate.Rows[i].DefaultCellStyle.BackColor = Color.White;
            }
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            check_tr_edit(txtStateCode.Text, txtHSNCode.Text);

            if (this.dsMain.Tables[0].Rows.Count <= 0)
            {
                return;
            }

            this.pAddMode = false;
            this.pEditMode = true;
            this.mthEnableDisableFormControls();
            dsMain.Tables[0].Rows[0].BeginEdit();
            this.mthChkNavigationButton();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Iscancel = true;

            dsMain.Tables[0].RejectChanges();
            childTbl.RejectChanges();
            string keyValue = string.Empty;

            //if (this.dsMain.Tables[0].Rows.Count != 0)
            //{
            //    dsMain.Tables[0].Rows[0].CancelEdit();
            //}

            //if (this.dsMain.Tables[0].Rows.Count == 1)
            //{
            //    dsMain.Tables[0].Rows[0].CancelEdit();
            //    if (this.pAddMode)
            //    {
            //        dsMain.Tables[0].Rows[0].Delete();
            //        this.btnLast.PerformClick();
            //    }
            //    if (this.pEditMode)
            //    {
            //        //this.txtRemarks.Enabled = false;

            //        vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            //        SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
            //        dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            //        this.mthView();
            //    }
            //}
            if (this.pAddMode || this.pEditMode)
            {
                SqlStr = "Select top 1 * from " + vMainTblNm + " Where hsncode+'-'+state_code in (Select Top 1 hsncode+'-'+state_code from " + vMainTblNm + " Order by hsncode desc,state_code desc )";
                dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    keyValue = dsMain.Tables[0].Rows[0]["hsncode"].ToString() + "-" + dsMain.Tables[0].Rows[0]["state_code"].ToString();
                }
            }
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthView();
            //this.pAddMode = false;
            //this.pEditMode = false;
            this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();
            this.GetChildRecords(keyValue);
            this.dgvGSTRate.DataSource = null;
            this.AssignSourceToGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int val = check_tr();
            if (val == 0)
            {
                MessageBox.Show("Transaction has been made against this HSN Code, Records cannot be Deleted.",
                  "HSN Code Master",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string vDelString = string.Empty;

                    vMainFldVal = string.Empty;
                    if (dsMain.Tables[0].Rows.Count > 0)
                        vMainFldVal = dsMain.Tables[0].Rows[0]["hsncode"].ToString() + "-" + dsMain.Tables[0].Rows[0]["state_code"].ToString();


                    vDelString = "Delete from Hsncodemast  Where  hsncode+'-'+state_code='" + this.txtHSNCode.Text.Trim() + '-' + this.txtStateCode.Text.Trim() + "'";
                    oDataAccess.ExecuteSQLStatement(vDelString, null, 20, true);
                    this.dsMain.Tables[0].Rows[0].Delete();
                    this.dsMain.Tables[0].AcceptChanges();

                    //if (this.btnForward.Enabled)
                    //{
                    //    SqlStr = "Select top 1 * from " + vMainTblNm + "  Where " + vMainField + ">'" + vMainFldVal + "' order by " + vMainField;
                    //    dsMain = oDataAccess.GetDataSet(SqlStr, null, 25);
                    //    vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                    //}
                    //else
                    //{
                    //    if (this.btnBack.Enabled)
                    //    {
                    //        SqlStr = "Select top 1 * from " + vMainTblNm + "  Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc";
                    //        dsMain = oDataAccess.GetDataSet(SqlStr, null, 25);
                    //        vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
                    //    }

                    //}
                    this.mthView();
                    this.mthChkNavigationButton();
                    this.btnLast_Click(sender, e);
                    this.dgvGSTRate.DataSource = null;
                    this.AssignSourceToGrid();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHSNCode_Click(object sender, EventArgs e)
        {
            if (this.pAddMode || this.pEditMode)
            {
                string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
                DataSet tDs = new DataSet();
                SqlStr = "Select HSN_CODE,HSN_Desc,hsn_id=CAST(hsn_id AS VARCHAR(10)) from hsn_master Where Isactive=1 and LEN(RTRIM(hsn_code)) in (2,4,6,8)  order by HSN_CODE,HSN_Desc";

                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
                DataView dvw = tDs.Tables[0].DefaultView;
                VForText = "Select HSN Code";
                vSearchCol = "HSN_CODE";
                vDisplayColumnList = "HSN_CODE:HSN Name,HSN_Desc:Description";
                vReturnCol = "HSN_CODE,hsn_id";
                udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
                oSelectPop.pdataview = dvw;
                oSelectPop.pformtext = VForText;
                oSelectPop.psearchcol = vSearchCol;

                oSelectPop.Icon = pFrmIcon;
                oSelectPop.pDisplayColumnList = vDisplayColumnList;
                oSelectPop.pRetcolList = vReturnCol;
                oSelectPop.ShowDialog();

                if (oSelectPop.pReturnArray != null)
                {
                    this.txtHSNCode.Text = oSelectPop.pReturnArray[0];
                    childTbl.Rows[0]["hsnid"] = Convert.ToInt32(oSelectPop.pReturnArray[1]);
                    childTbl.Rows[0]["hsncode"] = oSelectPop.pReturnArray[0];
                }
            }

            if (!this.pAddMode && !this.pEditMode)
            {
                string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
                DataSet tDs = new DataSet();
                //SqlStr = "Select a.HSNCODE,a.state_code,b.state,c.HSN_Desc from Hsncode_vw a Inner join State b on (a.state_code=b.gst_state_code) inner join hsn_master c on c.HSN_Code=a.HSNCODE Order by a.HSNCode,b.State";
                SqlStr = "select  distinct a.HSNCode, a.state_code,b.State,c.HSN_Desc,a.exempted from HSNCODEMAST a, STATE b ,HSN_Master c Where a.hsnid = c.hsn_id and a.State_code = b.Gst_State_Code and  c.Isactive=1 and LEN(RTRIM(c.hsn_code)) in (2,4,6,8) ";
                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

                DataView dvw = tDs.Tables[0].DefaultView;

                VForText = "Select HSN Code";
                vSearchCol = "HSNCode";
                vDisplayColumnList = "HSNCode:HSN Name,State_code:State Code,State:State,HSN_Desc: HSN Discription,exempted:Exempted";
                vReturnCol = "HSNCode,state_code,State";
                udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
                oSelectPop.pdataview = dvw;
                oSelectPop.pformtext = VForText;
                oSelectPop.psearchcol = vSearchCol;

                oSelectPop.Icon = pFrmIcon;
                oSelectPop.pDisplayColumnList = vDisplayColumnList;
                oSelectPop.pRetcolList = vReturnCol;
                oSelectPop.ShowDialog();
                //vMainFldVal = dsMain.Tables[0].Rows[0]["GradeNm"].ToString().Trim();


                if (oSelectPop.pReturnArray != null)
                {
                    this.txtHSNCode.Text = oSelectPop.pReturnArray[0];
                    this.txtStateCode.Text = oSelectPop.pReturnArray[1];

                    //this.txtState.Text = oSelectPop.pReturnArray[2];
                    //vMainFldVal = this.txtGradeName.Text.Trim();
                    //SqlStr = "Select top 1 * from " + vMainTblNm + " Where GradeNm='" + vMainFldVal + "' order by " + vMainField;
                    //dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

                }
                GoToRecord(this.txtHSNCode.Text.Trim() + "-" + this.txtStateCode.Text.Trim());
                this.dgvGSTRate.DataSource = null;
                this.AssignSourceToGrid();
                this.mthView();
                this.mthChkNavigationButton();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnNew.Enabled)
                btnNew_Click(this.btnNew, e);
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);

        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnExit, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
                btnDelete_Click(this.btnDelete, e);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSelect.Checked == true)
                this.btnStateList.Visible = true;
            else
                this.btnStateList.Visible = false;

            this.Refresh();
        }

        private void cboStateList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboStateList.SelectedValue != null)
                this.txtStateCode.Text = this.cboStateList.SelectedValue.ToString();

        }

        private bool CheckValidation()
        {
            if (this.txtStateCode.Text.Trim().Length <= 0)
            {
                MessageBox.Show("State code cannot be empty.", this.pPApplName);
                this.txtStateCode.Focus();
                return false;
            }

            if (this.txtHSNCode.Text.Trim().Length <= 0)
            {
                MessageBox.Show("HSN code cannot be empty.", this.pPApplName);
                this.txtHSNCode.Focus();
                return false;
            }

            for (int i = 0; i < dgvGSTRate.RowCount; i++)
            {

                if (Convert.ToDouble(dgvGSTRate.Rows[i].Cells["colSGST"].Value) > 100)
                {
                    MessageBox.Show("State GST Rate cannot be greater than 100", this.pPApplName);
                    this.dgvGSTRate.CurrentCell = this.dgvGSTRate.Rows[i].Cells["colSGST"];
                    return false;
                }
                if (Convert.ToDouble(dgvGSTRate.Rows[i].Cells["colCGST"].Value) > 100)
                {
                    MessageBox.Show("Central GST Rate cannot be greater than 100", this.pPApplName);
                    this.dgvGSTRate.CurrentCell = this.dgvGSTRate.Rows[i].Cells["colCGST"];
                    return false;
                }
                if (Convert.ToDouble(dgvGSTRate.Rows[i].Cells["colIGST"].Value) > 100)
                {
                    MessageBox.Show("Interstate GST Rate cannot be greater than 100", this.pPApplName);
                    this.dgvGSTRate.CurrentCell = this.dgvGSTRate.Rows[i].Cells["colIGST"];
                    return false;
                }
            }

            return true;
        }

        private void txtHSNCode_Validating(object sender, CancelEventArgs e)
        {

            if (this.pAddMode || this.pEditMode)
            {
                //if (this.txtHSNCode.Text.Trim().Length <= 0)
                //{
                //    MessageBox.Show("HSN Code cannot be empty.", this.pPApplName);
                //    e.Cancel = true;
                //}
                foreach (DataRow item in childTbl.Rows)
                {
                    item["Hsncode"] = this.txtHSNCode.Text;
                }
            }



        }
        private void AddRecord()
        {

            DataRow drchild = childTbl.NewRow();
            drchild["SGSTPer"] = 0;
            drchild["CGSTPer"] = 0;
            drchild["IGSTPer"] = 0;
            drchild["Activefrom"] = DateTime.Today;
            drchild["exempted"] = false;
            childTbl.Rows.Add(drchild);
            this.dgvGSTRate.Refresh();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.AddRecord();
        }

        private void dgvGSTRate_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.pEditMode == true)
            {


                if (dgvGSTRate.SelectedCells.Count > 0)
                {
                    int selectedrowindex = dgvGSTRate.SelectedCells[0].RowIndex;

                    DataGridViewRow selectedRow = dgvGSTRate.Rows[selectedrowindex];

                    string date = Convert.ToString(selectedRow.Cells[3].Value);

                    //// int result = check_tr_edit(txtStateCode.Text.ToString(), txtHSNCode.Text.ToString(), date);
                    // //   MessageBox.Show(date);
                    // if (result == 1)
                    // {
                    //     selectedRow.ReadOnly = true;
                    // }
                    // else
                    // {
                    //     selectedRow.ReadOnly = false;
                    // }
                }
            }
        }

        private void dgvGSTRate_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvGSTRate_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

            if (childTbl.Rows.Count > 1)
            {
                DeletedRec.Rows.Add();
                DeletedRec.Rows[DeletedRecIndex][0] = dgvGSTRate.Rows[this.dgvGSTRate.CurrentRow.Index].Cells[4].Value;
                DeletedRecIndex++;


                if (this.dgvGSTRate.CurrentRow.Index >= 0)
                {
                    if (dgvGSTRate.Rows[this.dgvGSTRate.CurrentRow.Index].DefaultCellStyle.BackColor != Color.LightPink)
                    {


                        this.dgvGSTRate.Rows.RemoveAt(this.dgvGSTRate.CurrentRow.Index);
                    }
                }
                //DeletedRec          
            }
        }
        private void txtHSNCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                this.txtHSNCode.CausesValidation = false;
                this.btnHSNCode.PerformClick();
            }
        }


        private void btnStateList_Click(object sender, EventArgs e)
        {
            if (this.txtStateCode.Text == "")
            {
                DialogResult Result = MessageBox.Show("State code can not be empty..", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cboStateList.Focus();
                return;
            }

            txtValid.Text = this.txtStateCode.Text;
            string sqlQuery = string.Empty;
            DataSet DS1;
            sqlQuery = "select [State],GST_State_Code from [State] where [State]<>''";
            DS1 = oDataAccess.GetDataSet(sqlQuery, null, 25);
            frmValidIn frmvalid = new frmValidIn();
            frmvalid.ds = DS1;
            frmvalid.getds(DS1, txtValid.Text.ToString().Trim());
            frmvalid.ShowDialog();
            txtValid.Text = frmvalid.validdata;
            DtValidin = frmvalid.DtValid;
            //dsMain.Tables[0].Rows[0]["State_Code"] = frmvalid.validdata;
        }

        public void cancel()
        {
            //Iscancel = true;

            dsMain.Tables[0].RejectChanges();
            childTbl.RejectChanges();
            string keyValue = string.Empty;

            //if (this.dsMain.Tables[0].Rows.Count != 0)
            //{
            //    dsMain.Tables[0].Rows[0].CancelEdit();
            //}

            //if (this.dsMain.Tables[0].Rows.Count == 1)
            //{
            //    dsMain.Tables[0].Rows[0].CancelEdit();
            //    if (this.pAddMode)
            //    {
            //        dsMain.Tables[0].Rows[0].Delete();
            //        this.btnLast.PerformClick();
            //    }
            //    if (this.pEditMode)
            //    {
            //        //this.txtRemarks.Enabled = false;

            //        vMainFldVal = dsMain.Tables[0].Rows[0][vMainField].ToString().Trim();
            //        SqlStr = "Select top 1 * from " + vMainTblNm + " Where " + vMainField + "='" + vMainFldVal + "'";
            //        dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);
            //        this.mthView();
            //    }
            //}
            if (this.pAddMode || this.pEditMode)
            {
                SqlStr = "Select top 1 * from " + vMainTblNm + " Where hsncode+'-'+state_code in (Select Top 1 hsncode+'-'+state_code from " + vMainTblNm + " Order by hsncode desc,state_code desc )";
                dsMain = oDataAccess.GetDataSet(SqlStr, null, 20);

                if (dsMain.Tables[0].Rows.Count > 0)
                {
                    keyValue = dsMain.Tables[0].Rows[0]["hsncode"].ToString() + "-" + dsMain.Tables[0].Rows[0]["state_code"].ToString();
                }
            }
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthView();
            //this.pAddMode = false;
            //this.pEditMode = false;
            this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();
            this.GetChildRecords(keyValue);
            this.dgvGSTRate.DataSource = null;
            this.AssignSourceToGrid();
        }

        private void dgvGSTRate_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {//Rupesh G. add 19-aug-2017
            
            double IGST, CGST, SGST;
            IGST = Convert.ToDouble(dgvGSTRate.Rows[e.RowIndex].Cells[2].Value);
            if (IGST == 0 || IGST==3 || IGST == 5 || IGST == 12 || IGST == 18 || IGST == 28)
            {
                if (this.pEditMode == true || this.pAddMode == true)
                {
                    if (e.ColumnIndex == 2)
                    {
                        CGST = IGST / 2;
                        SGST = IGST / 2;
                        dgvGSTRate.Rows[e.RowIndex].Cells[0].Value = CGST;
                        dgvGSTRate.Rows[e.RowIndex].Cells[1].Value = SGST;
                    }
                }
           
            }
            else
            {
                MessageBox.Show("IGST Rate should be like 0,3,5,12,18,28.", "HSN Code Master", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvGSTRate.Rows[e.RowIndex].Cells[2].Value= igst_o;
                dgvGSTRate.Rows[e.RowIndex].Cells[0].Value = cgst_o;
                dgvGSTRate.Rows[e.RowIndex].Cells[1].Value = sgst_o;
              

            }
            
        }

        private void dgvGSTRate_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
           igst_o = Convert.ToDouble(dgvGSTRate.Rows[e.RowIndex].Cells[2].Value);
           cgst_o = Convert.ToDouble(dgvGSTRate.Rows[e.RowIndex].Cells[0].Value);
           sgst_o = Convert.ToDouble(dgvGSTRate.Rows[e.RowIndex].Cells[1].Value);
           
        }

        private void check_tr_edit(string scode, string hsncode)
        {
            int count = 0;
            DataTable dt1 = new DataTable();
            string SqlStr = "select  state,hsncode,date from hsnmaster_tr_vw where state=" + scode + " and hsncode='" + hsncode + "'  ";



            dt1 = oDataAccess.GetDataSet(SqlStr, null, 20).Tables[0];

            for (int i = 0; i < dgvGSTRate.RowCount; i++)
            {
                int cn = 0;
                string date3 = Convert.ToString(dgvGSTRate.Rows[i].Cells[3].Value.ToString());
                DateTime oDate1 = Convert.ToDateTime(date3);
                foreach (DataRow dr in dt1.Rows)
                {
                    string state = dr[0].ToString().Trim();
                    string code = dr[1].ToString().Trim();
                    string date1 = dr[2].ToString().Trim();
                    DateTime oDate = Convert.ToDateTime(date1);
                    if (state == scode.Trim() && code == hsncode.Trim() && oDate >= oDate1)
                    {
                        cn++;
                    }

                }

                if (cn > 0)
                {
                    dgvGSTRate.Rows[i].ReadOnly = true;
                    dgvGSTRate.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;

                }
                else
                {
                    dgvGSTRate.Rows[i].ReadOnly = false;

                }




            }

            //foreach (DataRow dr in dt1.Rows)
            //{
            //    string state = dr[0].ToString().Trim();
            //    string code = dr[1].ToString().Trim();
            //    string date1 = dr[2].ToString().Trim();
            //    DateTime oDate = Convert.ToDateTime(date1);
            //    string date2;
            //    for (int i = 0; i < dgvGSTRate.RowCount; i++)
            //    {
            //        date2 = Convert.ToString(dgvGSTRate.Rows[i].Cells[3].Value.ToString());
            //        DateTime oDate1 = Convert.ToDateTime(date);
            //        MessageBox.Show("" + date2);
            //        if (state == scode.Trim() && code == hsncode.Trim() && oDate >= oDate1)
            //        {
            //            dgvGSTRate.Rows[i].
            //        }
            //    }
            //}


            //foreach (DataRow dr in dt1.Rows)
            //{

            //    string state = dr[0].ToString().Trim();
            //    string code = dr[1].ToString().Trim();
            //    string date1 = dr[2].ToString().Trim();
            //    DateTime oDate = Convert.ToDateTime(date1);
            //    // MessageBox.Show("" + oDate);
            //    DateTime oDate1 = Convert.ToDateTime(date);
            //    //MessageBox.Show("" + oDate1);
            //    if (state == scode.Trim() && code == hsncode.Trim() && oDate >= oDate1)
            //    {
            //        count++;
            //    }

            //}

            //if (count > 0)
            //{
            //    return 0;
            //}
            //else
            //{
            //    return 1;
            //}
        }

        private int check_tr_save(string scode, string hsncode, string date)
        {
            int count = 0;
            int cn = 0;
            DataTable dt1 = new DataTable();
            string SqlStr = "select  state,hsncode,date from hsnmaster_tr_vw where state=" + scode + " and hsncode='" + hsncode + "'  ";
            dt1 = oDataAccess.GetDataSet(SqlStr, null, 20).Tables[0];


            string date3 = Convert.ToString(date);
            DateTime oDate1 = Convert.ToDateTime(date3);

            foreach (DataRow dr in dt1.Rows)
            {
                string state = dr[0].ToString().Trim();
                string code = dr[1].ToString().Trim();
                string date1 = dr[2].ToString().Trim();
                DateTime oDate = Convert.ToDateTime(date1);
                if (state == scode.Trim() && code == hsncode.Trim() && oDate >= oDate1)
                {
                    cn++;
                }

            }


            if (cn > 0)
            {
                return 0;
            }
            else
            {
                return 1;

            }


        }

        private int check_tr()
        {
            int count = 0;
            DataTable dt1 = new DataTable();
            string SqlStr = "select distinct state,hsncode,date from hsnmaster_tr_vw";
            dt1 = oDataAccess.GetDataSet(SqlStr, null, 20).Tables[0];

            foreach (DataRow dr in dt1.Rows)
            {
                string state = dr[0].ToString().Trim();
                string code = dr[1].ToString().Trim();
                string date1 = Convert.ToDateTime(dr[2].ToString().Trim()).ToString("MM/dd/ yyyy");

                if (state == txtStateCode.Text.Trim() && code == txtHSNCode.Text.Trim())
                {
                    count++;
                }

            }

            if (count > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
