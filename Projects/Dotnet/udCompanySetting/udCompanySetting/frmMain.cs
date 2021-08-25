using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Udyog.Library.Common;

namespace udCompanySetting
{
    public partial class frmMain : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DataTable vTblMain = new DataTable();
        string SqlStr = string.Empty;
        string prodcode = "", prodcode1 = "";
        DataSet vDsCommon;
        DataSet dsMain = new DataSet();
        string vMainField = "CompId", vTblMainNm = "Co_Mast", vMainFldVal = "", vOrdFld = "";
        DataTable vTblManufact, vTblStkValConfig, vTblCo_Set;
        String cAppPId, cAppName;
        string vErrMsg = string.Empty;
        short vTimeOut = 15;

        string ServiceType = "";
        string vShowTips = "";
        public frmMain(string[] args)
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            this.pFrmCaption = "Company Setting";
            this.pPApplPID = 0;

            this.pPara = args;
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;


            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.SetMenuRights();
            this.mInsertProcessIdRecord();
            this.SetFormColor();
            this.mthDsCommon();

            string appPath = Application.ExecutablePath;
            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }


             prodcode = VU_UDFS.GetDecProductCode(vDsCommon.Tables[0].Rows[0]["p1"].ToString().Trim());
             prodcode1 = VU_UDFS.GetDecProductCode(vDsCommon.Tables[0].Rows[0]["p2"].ToString().Trim());
            prodcode = prodcode + "," + prodcode1;


        

            this.mthControlSet();
            this.mthCreateTbls();
        }
        private void mthCreateTbls()
        {
            SqlStr = "Select * From vudyog..Co_Mast Where CompId="+this.pCompId;
            vTblMain = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            SqlStr = "Select * From Manufact";
            vTblManufact = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            SqlStr = "Select * From StkValConfig";
            vTblStkValConfig = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            SqlStr = "Select * From Co_Set";
            vTblCo_Set = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.mthEdit();
            this.pEditMode = true;
            
        }
        private void mthEdit()
        {
            if (this.vTblMain.Rows.Count <= 0)
            {
                return;
            }
            this.pAddMode = false;
            this.pEditMode = true;
            this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();
         }
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.mthChkSaveValidation(ref vErrMsg);
            if (vErrMsg != "") { MessageBox.Show(vErrMsg, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            this.mthSave();
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();
        }
        private void mthSave()
        {
            string vErrMsg = "";
            string vSaveString = string.Empty;
            string[] vEntryList = null;
            DataTable tblNm = new DataTable();
            try
            {
                oDataAccess.BeginTransaction();

                vSaveString = string.Empty;
                this.mSaveCommandString(ref vSaveString, "#passroute##prodcode##passroute1##GatewayPwd##Industry##ModDet##coFormdt#", "#CompId#",vTblMain,"vudyog..Co_Mast");
                oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);


                vSaveString = string.Empty;
                this.mSaveCommandString(ref vSaveString, "", "", vTblManufact, "Manufact");
                oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);

                vSaveString = string.Empty;
                this.mSaveCommandString(ref vSaveString, "", "", vTblCo_Set, "Co_Set");
                if (vSaveString != "")
                {
                    oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
                    this.pAddMode = false;
                }
                

                SqlStr = "Delete From StkValConfig";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                foreach(DataRow dr in vTblStkValConfig.Rows)
                {
                    vSaveString = string.Empty;
                    this.mthSave_InsertString(dr, ref vSaveString, "", "", vTblStkValConfig, "StkValConfig");
                    oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
                }

                oDataAccess.CommitTransaction();
                MessageBox.Show("Please re-login the application for the changes to be effected...", this.pPApplText);   //Divyang
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                oDataAccess.RollbackTransaction();
            }
        }
        private void mthSave_InsertString(DataRow dr, ref string vSaveString, string vkeyField, string vIgnoreColumns, DataTable tblUpdt, string tblUpdtNm)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;

            /*Identity Columns--->*/
            DataSet dsData = new DataSet();
            string sqlstr = "select c.name as colName from sys.objects o inner join sys.Columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            sqlstr = sqlstr + " and o.name='" + tblUpdtNm + " ' ";
            dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#" + dr1["colName"].ToString().Trim() + "#";
            }

            /*<---Identity Columns--->*/
            if (1 == 1)
            {
                vSaveString = "Set DateFormat dmy insert into " + tblUpdtNm;
                tblUpdt.AcceptChanges();
                foreach (DataColumn dtc1 in tblUpdt.Columns)
                {
                    if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0 && vIgnoreColumns.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                    {
                        if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                        if (string.IsNullOrEmpty(vfldValList) == false) { vfldValList = vfldValList + ","; }
                        vfldList = vfldList + "[" + dtc1.ToString().Trim() + "]";
                        vfldVal = dr[dtc1.ToString().Trim()].ToString().Trim();

                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal.Replace("'","''") + "'";
                        }
                        if ((dtc1.DataType.Name.ToLower() == "decimal" || dtc1.DataType.Name.ToLower() == "int16" || dtc1.DataType.Name.ToLower() == "int32") && string.IsNullOrEmpty(vfldVal))
                        {
                            vfldVal = "0";
                        }
                        if (dtc1.DataType.Name.ToLower() == "datetime")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        if (dtc1.DataType.Name.ToLower() == "boolean")
                        {
                            if (vfldVal.ToLower() == "true")
                            {
                                vfldVal = "1";
                            }
                            else
                            {
                                vfldVal = "0";
                            }

                        }
                        vfldValList = vfldValList + vfldVal;
                    }
                }

                vSaveString = vSaveString + " (" + vfldList + ") Values (" + vfldValList + ")";
            }

        }
        private void mSaveCommandString(ref string vSaveString, string vIgnoreFld, string vkeyField,DataTable tblUpdt,string tblUpdtNm)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;

            /*Identity Columns--->*/
            DataSet dsData = new DataSet();
            SqlStr = "select c.name as ColName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            SqlStr = SqlStr + " and o.name='" + tblUpdtNm + " ' ";
            dsData = oDataAccess.GetDataSet(SqlStr, null, 20);
            
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#" + dr1["ColName"].ToString().Trim() + "#";
            }

            if (tblUpdt.Rows.Count == 0)
            {
                return;
            }

            //if (vTblCo_Set.Rows[0]["BarCodeId"].ToString().Trim() == "" && tblUpdtNm=="Co_Set")
            //{
            //    this.pAddMode = true;
            //}

            /*<---Identity Columns--->*/
            if (this.pAddMode == true)
            {
                vSaveString = "Set DateFormat dmy insert into " + tblUpdtNm;
                tblUpdt.AcceptChanges();
                foreach (DataColumn dtc1 in tblUpdt.Columns)
                {
                    if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0 && vIgnoreFld.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                    {
                        if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                        if (string.IsNullOrEmpty(vfldValList) == false) { vfldValList = vfldValList + ","; }
                        vfldList = vfldList + dtc1.ToString().Trim();
                        vfldVal = tblUpdt.Rows[0][dtc1.ToString().Trim()].ToString().Trim();

                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal.Replace("'","''") + "'";
                        }
                        if ((dtc1.DataType.Name.ToLower() == "decimal" || dtc1.DataType.Name.ToLower() == "int16" || dtc1.DataType.Name.ToLower() == "int32") && string.IsNullOrEmpty(vfldVal))
                        {
                            vfldVal = "0";
                        }
                        if (dtc1.DataType.Name.ToLower() == "datetime")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        if (dtc1.DataType.Name.ToLower() == "boolean")
                        {
                            if (vfldVal.ToLower() == "true")
                            {
                                vfldVal = "1";
                            }
                            else
                            {
                                vfldVal = "0";
                            }

                        }
                        vfldValList = vfldValList + vfldVal;
                    }
                }

                vSaveString = vSaveString + " (" + vfldList + ") Values (" + vfldValList + ")";
            }
            if (this.pEditMode == true)
            {
                vSaveString = "Set DateFormat dmy Update " + tblUpdtNm + "  Set ";
                string vWhereCondn = string.Empty;

                foreach (DataColumn dtc1 in tblUpdt.Columns)
                {
                    if (vIgnoreFld.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                    {
                        vfldVal = tblUpdt.Rows[0][dtc1.ToString().Trim()].ToString().Trim();
                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal.Replace("'", "''") + "'";
                        }
                        if (dtc1.DataType.Name.ToLower() == "datetime")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        if (dtc1.DataType.Name.ToLower() == "boolean")
                        {
                            if (vfldVal.ToLower() == "true")
                            {
                                vfldVal = "1";
                            }
                            else
                            {
                                vfldVal = "0";
                            }

                        }

                        if ((vkeyField.ToLower().IndexOf("#" + dtc1.ToString().Trim().ToLower() + "#") > -1))
                        //if (Array.Exists(vArrkeyField, element => element == dtc1.ToString().Trim().ToLower()) == true)
                        {
                            if (string.IsNullOrEmpty(vWhereCondn) == false) { vWhereCondn = vWhereCondn + " and "; } else { vWhereCondn = " Where "; }
                            vWhereCondn = vWhereCondn + dtc1.ToString().Trim() + " = ";
                            vfldVal = tblUpdt.Rows[0][dtc1.ToString().Trim()].ToString().Trim();
                            if (dtc1.DataType.Name.ToLower() == "string")
                            {
                                if(dtc1.ColumnName == "PunchLn")
                                {
                                    MessageBox.Show("Hi");
                                }
                                vfldVal = "'" + vfldVal.Replace("'","''") + "'";
                            }
                            if (dtc1.DataType.Name.ToLower() == "datetime")
                            {
                                vfldVal = "'" + vfldVal + "'";
                            }
                            if (dtc1.DataType.Name.ToLower() == "boolean")
                            {
                                if (vfldVal.ToLower() == "true")
                                {
                                    vfldVal = "1";
                                }
                                else
                                {
                                    vfldVal = "0";
                                }

                            }
                            vWhereCondn = vWhereCondn + vfldVal;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                            vfldList = vfldList + dtc1.ToString().Trim() + " = ";
                            vfldList = vfldList + vfldVal;
                        }
                    }
                } //For Loop EditMode
                vSaveString = vSaveString + vfldList + vWhereCondn;

            }
        }
        private void mthChkSaveValidation(ref string vErrMsg)
        {

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.mthCreateTbls();
            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();
            
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            string startupPath = Application.StartupPath;
            GetInfo.iniFile ini = new GetInfo.iniFile(startupPath + "\\" + "Visudyog.ini");
            vShowTips = ini.IniReadValue("Defaults", "ShowIntroForm");
            
            
            frmGeneral oFrmGeneral = new udCompanySetting.frmGeneral();
            oFrmGeneral.pTblMain = this.vTblMain;
            oFrmGeneral.oDataAccess = this.oDataAccess;
            oFrmGeneral.pAppPath = this.pAppPath;
            oFrmGeneral.pPApplText=this.pPApplText;
            oFrmGeneral.pAddMode = this.pAddMode;
            oFrmGeneral.pEditMode = this.pEditMode;
            oFrmGeneral.pShowTips = this.vShowTips; //Divyang
            oFrmGeneral.Icon = this.Icon;
            oFrmGeneral.ShowDialog();
            vShowTips = oFrmGeneral.pShowTips;
            ini.IniWriteValue("Defaults", "ShowIntroForm", vShowTips);
        }
        private void btnInv_Click(object sender, EventArgs e)
        {
            frmInventory oFrmInventory = new frmInventory();
            oFrmInventory.pTblMain = this.vTblMain;
            oFrmInventory.oDataAccess = this.oDataAccess;
            oFrmInventory.pAppPath = this.pAppPath;
            oFrmInventory.pPApplText = this.pPApplText;
            oFrmInventory.pTblManufact = vTblManufact;
            oFrmInventory.pTblStkValConfig = vTblStkValConfig;
            oFrmInventory.pAddMode = this.pAddMode;
            oFrmInventory.pEditMode = this.pEditMode;
            oFrmInventory.Icon = this.Icon;
            oFrmInventory.ShowDialog();
        }
        private void btnMultiUOM_Click(object sender, EventArgs e)
        {
            if (!prodcode.ToLower().Contains("multiuom"))
            {
                MessageBox.Show("Multi UOM Module is not selected...", this.pPApplText);
                this.btnMultiUOM.Enabled = false;
                return;
            }

            frmMultiUOM ofrmMultiUOM = new frmMultiUOM();
            ofrmMultiUOM.pTblMain = this.vTblMain;
            ofrmMultiUOM.oDataAccess = this.oDataAccess;
            ofrmMultiUOM.pAppPath = this.pAppPath;
            ofrmMultiUOM.pPApplText = this.pPApplText;
            ofrmMultiUOM.pAddMode = this.pAddMode;
            ofrmMultiUOM.pEditMode = this.pEditMode;
            ofrmMultiUOM.Icon = this.Icon;
            ofrmMultiUOM.ShowDialog();
        }
        private void btnAcc_Click(object sender, EventArgs e)
        {
            frmAccounts oFrmAccounts = new udCompanySetting.frmAccounts();
            oFrmAccounts.pTblMain = this.vTblMain;
            oFrmAccounts.oDataAccess = this.oDataAccess;
            oFrmAccounts.pAppPath = this.pAppPath;
            oFrmAccounts.pAddMode = this.pAddMode;
            oFrmAccounts.pEditMode = this.pEditMode;
            oFrmAccounts.Icon = this.Icon;
            oFrmAccounts.ShowDialog();
        }
        private void btnMultiCurr_Click(object sender, EventArgs e)
        {
            if (!prodcode.ToLower().Contains("vumcu"))
            {
                MessageBox.Show("Multi Currency Module is not selected...", this.pPApplText);
                this.btnMultiCurr.Enabled = false;
                return;
            }

            frmMultiCurrency oFrmGeneral = new udCompanySetting.frmMultiCurrency();
            oFrmGeneral.pTblMain = this.vTblMain;
            oFrmGeneral.oDataAccess = this.oDataAccess;
            oFrmGeneral.pAppPath = this.pAppPath;
            oFrmGeneral.pAddMode = this.pAddMode;
            oFrmGeneral.pEditMode = this.pEditMode;
            oFrmGeneral.Icon = this.Icon;
            oFrmGeneral.ShowDialog();
        }
        private void btnPayroll_Click(object sender, EventArgs e)
        {
            frmPayroll oFrmPayroll = new frmPayroll();
            oFrmPayroll.pTblMain = this.vTblMain;
            oFrmPayroll.oDataAccess = this.oDataAccess;
            oFrmPayroll.pAppPath = this.pAppPath;
            oFrmPayroll.pAddMode = this.pAddMode;
            oFrmPayroll.pEditMode = this.pEditMode;
            oFrmPayroll.Icon = this.Icon;
            oFrmPayroll.ShowDialog();
        }
        private void btnBarCode_Click(object sender, EventArgs e)
        {
            frmBarCode oFrmBarCode = new frmBarCode();
            oFrmBarCode.pTblMain = this.vTblMain;
            oFrmBarCode.pTblCo_Set = this.vTblCo_Set;
            oFrmBarCode.oDataAccess = this.oDataAccess;
            oFrmBarCode.pAppPath = this.pAppPath;
            oFrmBarCode.pAddMode = this.pAddMode;
            oFrmBarCode.pEditMode = this.pEditMode;
            oFrmBarCode.Icon = this.Icon;
            oFrmBarCode.ShowDialog();
            this.vTblCo_Set = oFrmBarCode.pTblCo_Set;
        }
        private void btnNIC_Click(object sender, EventArgs e)
        {
            if (!prodcode.ToLower().Contains("udewaygen"))
            {
                MessageBox.Show("E-WayBill Auto generation Module is not selected...", this.pPApplText);
                this.btnNIC.Enabled = false;
                return;
            }

            frmNIC ofrmNIC = new frmNIC();
            ofrmNIC.pTblMain = this.vTblMain;
            ofrmNIC.oDataAccess = this.oDataAccess;
            ofrmNIC.pAppPath = this.pAppPath;
            ofrmNIC.pAddMode = this.pAddMode;
            ofrmNIC.pEditMode = this.pEditMode;
            ofrmNIC.Icon = this.Icon;
            ofrmNIC.ShowDialog();
        }

        private void btnIRN_Click(object sender, EventArgs e)
        {
            if (!prodcode.ToLower().Contains("udewaygen"))
            {
                MessageBox.Show("E-WayBill Auto generation Module is not selected...", this.pPApplText);
                this.btnIRN.Enabled = false;
                return;
            }

            frmIRN oFrmIRN = new frmIRN();
            oFrmIRN.pTblMain = this.vTblMain;
            oFrmIRN.oDataAccess = this.oDataAccess;
            oFrmIRN.pAppPath = this.pAppPath;
            oFrmIRN.pAddMode = this.pAddMode;
            oFrmIRN.pEditMode = this.pEditMode;
            oFrmIRN.Icon = this.Icon;
            oFrmIRN.ShowDialog();
        }
        public void mthEnableDisableFormControls()
        {

        }
        public void mthControlSet()
        {
            //this.ntxtCrDays.pAllowNegative = false;
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                //this.btnGen.Image = Image.FromFile(fName);
                //this.btnInv.Image = Image.FromFile(fName);
                //this.btnMultiUOM.Image = Image.FromFile(fName);
                //this.btnAcc.Image = Image.FromFile(fName);
                //this.btnMultiCurr.Image = Image.FromFile(fName);
                //this.btnBarCode.Image = Image.FromFile(fName);
                //this.btnNIC.Image = Image.FromFile(fName);
            }
            
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
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
                btnLogout_Click(this.btnLogout, e);
        }

        private void gbMain_Enter(object sender, EventArgs e)
        {

        }

        private void btnSMS_Click(object sender, EventArgs e)
        {
            if (!prodcode.ToLower().Contains("udsmsmod"))
            {
                MessageBox.Show("SMS Module is not selected...",this.pPApplText);
                this.btnSMS.Enabled = false;
                return;
            }

            frmSMS oFrmSMS = new udCompanySetting.frmSMS();
            oFrmSMS.pTblMain = this.vTblMain;
            oFrmSMS.oDataAccess = this.oDataAccess;
            oFrmSMS.pAppPath = this.pAppPath;
            oFrmSMS.pAddMode = this.pAddMode;
            oFrmSMS.pEditMode = this.pEditMode;
            oFrmSMS.Icon = this.Icon;
            oFrmSMS.ShowDialog();
        }

        private void SetMenuRights()
        {
            DataSet dsMenu = new DataSet();
            DataSet dsRights = new DataSet();
            this.pPApplRange = this.pPApplRange.Replace("^", "");
            SqlStr = "select padname,barname,range from com_menu where range =" + this.pPApplRange;
            dsMenu = oDataAccess.GetDataSet(SqlStr, null, 20);
            if (dsMenu != null)
            {
                if (dsMenu.Tables[0].Rows.Count > 0)
                {
                    string padName = "";
                    string barName = "";
                    padName = dsMenu.Tables[0].Rows[0]["padname"].ToString();
                    barName = dsMenu.Tables[0].Rows[0]["barname"].ToString();
                    SqlStr = "select padname,barname,dbo.func_decoder(rights,'F') as rights from ";
                    SqlStr += "userrights where padname ='" + padName.Trim() + "' and barname ='" + barName + "' and range = " + this.pPApplRange;
                    SqlStr += "and dbo.func_decoder([user],'T') ='" + this.pAppUerName.Trim() + "'";

                }
            }
            dsRights = oDataAccess.GetDataSet(SqlStr, null, 20);


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

        private void SetFormColor()
        {
            DataSet dsColor = new DataSet();
            Color myColor = Color.Coral;

            string colorCode = string.Empty;
            SqlStr = "select vcolor from Vudyog..co_mast where compid =" + this.pCompId;
            dsColor = oDataAccess.GetDataSet(SqlStr, null, 20);
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
        private void mthChkNavigationButton()
        {
            if (ServiceType.ToUpper() != "VIEWER VERSION")
            {
                if (this.pEditMode)
                {
                    this.btnSave.Enabled = true;
                    this.btnCancel.Enabled = true;
                    this.btnEdit.Enabled = false;
                    this.btnLogout.Enabled = false;
                }
                else
                {
                    this.btnSave.Enabled = false;
                    this.btnCancel.Enabled = false;
                    this.btnEdit.Enabled = true;
                    this.btnLogout.Enabled = true;
                }
            }
        }
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();

            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udCompanySetting.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            SqlStr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
        }
        private void mDeleteProcessIdRecord()
        {
            if (string.IsNullOrEmpty(this.pPApplName) || this.pPApplPID == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))
            {
                return;
            }
            DataSet dsData = new DataSet();

            SqlStr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
        }

        private void mthDsCommon()
        {
            string vL_Yn = "";
            vDsCommon = new DataSet();
            DataTable company = new DataTable();
            company.TableName = "company";
            SqlStr = "Select *,p1=rtrim(convert(varchar(max),Passroute)),p2=rtrim(convert(varchar(max),Passroute1)) From vudyog..Co_Mast where CompId=" + this.pCompId.ToString();
            company = oDataAccess.GetDataTable(SqlStr, null, 25);
            if (vL_Yn == "")
            {
                vL_Yn = ((DateTime)company.Rows[0]["Sta_Dt"]).Year.ToString().Trim() + "-" + ((DateTime)company.Rows[0]["End_Dt"]).Year.ToString().Trim();
            }
            vDsCommon.Tables.Add(company);
            vDsCommon.Tables[0].TableName = "company";

            DataTable tblCoAdditional = new DataTable();
            tblCoAdditional.TableName = "coadditional";
            SqlStr = "Select Top 1 * From Manufact";
            tblCoAdditional = oDataAccess.GetDataTable(SqlStr, null, 25);
            vDsCommon.Tables.Add(tblCoAdditional);
            vDsCommon.Tables[1].TableName = "coadditional";

            DataTable tblCoset = new DataTable();
            tblCoset.TableName = "CompanySetting";
            SqlStr = "Select Top 1 * From co_set";
            tblCoAdditional = oDataAccess.GetDataTable(SqlStr, null, 25);
            vDsCommon.Tables.Add(tblCoset);
            vDsCommon.Tables[2].TableName = "CompanySetting";
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
                //MessageBox.Show("Can't proceed,Main Application " + this.pPApplText + " is closed", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //Application.Exit();
            }
        }
    }
}
