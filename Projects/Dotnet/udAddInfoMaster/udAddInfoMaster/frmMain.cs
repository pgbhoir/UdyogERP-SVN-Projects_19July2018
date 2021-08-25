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
using udValidinTran;

namespace udAddInfoMaster
{
    public partial class frmMain : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        DataTable vTblMain = new DataTable();
        string SqlStr = string.Empty;
        DataSet vDsCommon;
        DataSet dsMain = new DataSet();
        string vMainField = "Head_Nm", vMainField1 = "e_Code", vTblMainNm = "lOther", vMainFldVal = "", vMainFldVal1 = "", vOrdFld = "Head_Nm,e_Code";
        String cAppPId, cAppName;

       

        string vErrMsg = string.Empty;
        short vTimeOut = 15;
        string ServiceType = "";
        string vE_Code = "", vBode_Nm = "", vTrnTypeCode = "";
        string vWhn_Con = "", vVal_Con = "", vDefa_Val = "", vInter_Use = "", vVal_Err = "",vMandatory="";
        string vOrgTblNm = "";
        public frmMain(string[] args)
        {
            this.pDisableCloseBtn = true;

            InitializeComponent();

            this.pFrmCaption = "Additional Info Master";
            this.pPApplPID = 0;

            this.pPara = args;
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            //Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            //this.pFrmIcon = MainIcon;
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

            

            string appPath = Application.ExecutablePath;
            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }
            this.mthControlSet();
            
            this.btnLast_Click(sender, e);
            

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = vTblMain.Rows[0][vMainField].ToString().Trim();
            vMainFldVal1 = vTblMain.Rows[0][vMainField1].ToString().Trim();
            SqlStr = "Select top 1 l.Code_Nm,m.* from " + vTblMainNm + " m Join lCode l on (m.e_Code=l.Entry_Ty) Where e_Code<>'I2' order by rtrim(" + vMainField + ")+rtrim(m." + vMainField1 + ")";
            vTblMain = oDataAccess.GetDataTable(SqlStr, null, 20);

            this.mthView();
            this.mthChkNavigationButton();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();

            vMainFldVal = vTblMain.Rows[0][vMainField].ToString().Trim();
            vMainFldVal1 = vTblMain.Rows[0][vMainField1].ToString().Trim();
            SqlStr = "Select top 1 l.Code_Nm,m.* from " + vTblMainNm + " m Join lCode l on (m.e_Code=l.Entry_Ty) Where  e_Code<>'I2' and rtrim(" + vMainField + ")+rtrim(m." + vMainField1 + ")<'" + vMainFldVal + vMainFldVal1 + "' order by rtrim(" + vMainField + ")+rtrim(m." + vMainField1 + ") desc";
            vTblMain = oDataAccess.GetDataTable(SqlStr, null, 20);

            this.mthView();
            this.mthChkNavigationButton();
        }
        private void btnForward_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            vMainFldVal = vTblMain.Rows[0][vMainField].ToString().Trim();
            vMainFldVal1 = vTblMain.Rows[0][vMainField1].ToString().Trim();
            SqlStr = "Select top 1  l.Code_Nm,m.* from " + vTblMainNm + "  m Join lCode l on (m.e_Code=l.Entry_Ty) Where e_Code<>'I2' and rtrim(" + vMainField + ")+rtrim(m." + vMainField1 + ")>'" + vMainFldVal + vMainFldVal1 + "' order by rtrim(" + vMainField + ")+rtrim(m." + vMainField1 + ")";
            vTblMain = oDataAccess.GetDataTable(SqlStr, null, 20);

            this.mthView();

            this.mthChkNavigationButton();
        }
        private void btnLast_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();

            this.pAddMode = false;
            this.pEditMode = false;
            //if (ServiceType.ToUpper() == "VIEWER VERSION")
            //{
            //    this.btnNew.Enabled = false;
            //    this.btnEdit.Enabled = false;
            //    this.btnCancel.Enabled = false;
            //    this.btnDelete.Enabled = false;
            //}
            //else
            this.mthEnableDisableFormControls();

            DataSet dsTemp = new DataSet();
            SqlStr = "select top 1  " + vMainField + " as Col," + vMainField1 + " as Col1 from " + vTblMainNm + " m Join lCode l on (m.e_Code=l.Entry_Ty) Where e_Code<>'I2' order by  rtrim(" + vMainField + ")+rtrim(" + vMainField1 + ") desc";
            dsTemp = oDataAccess.GetDataSet(SqlStr, null, 20);
            vMainFldVal = "";
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["Col1"].ToString()) == false)
                {
                    vMainFldVal = dsTemp.Tables[0].Rows[0]["Col"].ToString().Trim();
                    vMainFldVal1 = dsTemp.Tables[0].Rows[0]["Col1"].ToString().Trim();
                }
            }
            SqlStr = "Select top 1 l.Code_Nm,m.* from " + vTblMainNm + " m Join lCode l on (m.e_Code=l.Entry_Ty) Where e_Code<>'I2' and " + vMainField + "='" + vMainFldVal + "' and m." + vMainField1 + "='" + vMainFldVal1 + "'";
            vTblMain = oDataAccess.GetDataTable(SqlStr, null, 20);

            this.mthView();
            this.mthChkNavigationButton();
            if (vTblMain.Rows.Count == 0)
            {
                this.btnDelete.Enabled = false;
                this.btnEdit.Enabled = false;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnNew.Enabled)
                btnNew_Click(this.btnNew, e);
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.pAddMode = true;
            this.pEditMode = false;

            this.mthNew(sender, e);

            this.mthChkNavigationButton();
            //this.txtTrnType.Focus();
            this.rbTrn.Focus();
        }
        private void mthNew(object sender, EventArgs e)
        {
            this.mthBindClear();
            if (vTblMain.Rows.Count > 0)
            {
                vTblMain.Rows.RemoveAt(0);
                vTblMain.AcceptChanges();
            }

            DataRow drCurrent;
            drCurrent = vTblMain.NewRow();
            drCurrent["InPickup"] = false;
            drCurrent["InGrid"] = false;
            drCurrent["Disp_MIS"] = false;
            drCurrent["Data_Ty"] = "Varchar";
            drCurrent["Fld_Wid"] = 10;
            drCurrent["Fld_Dec"] = 0;

            drCurrent["Whn_Con"] = "";
            drCurrent["Val_Con"] = "";
            drCurrent["Defa_Val"] = "";
            drCurrent["Inter_Use"] = "";
            drCurrent["Val_Err"] = "";
            drCurrent["Mandatory"] = "";

            drCurrent["Lines"] = false;
            drCurrent["Type"] = "T";
            drCurrent["Att_File"] = false;

            vTblMain.Rows.Add(drCurrent);

            this.mthBindData();
            this.mthEnableDisableFormControls();
            vTblMain.Rows[0].BeginEdit();

            this.txtFldNm.CharacterCasing = CharacterCasing.Upper;

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.mthEdit();

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
            vTblMain.Rows[0].BeginEdit();
            this.mthChkNavigationButton();
            //this.txtTrnType.Focus();
            this.txtHeadNm.Focus();   //Divyang
            this.mth_rbValid(); //Divyang
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            vErrMsg = "";
            this.label1.Focus();
            this.mcheckCallingApplication();
            this.mthChkSaveValidation(ref vErrMsg);
            if (vErrMsg != "") { MessageBox.Show(vErrMsg, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            this.Refresh();
            this.mthSave();

            this.mthChkNavigationButton();
        }
        private void mthSave()
        {
            string vSaveString = string.Empty;
            string[] vEntryList = null;
            DataTable tblNm = new DataTable();

            
            if (nudOrder.Value == 0)
            {
                SqlStr = "Select Serial=max(isNull(Serial,0))+1 From Lother Where Entry_Ty='" + vE_Code + "' and Att_File=" + (rbHeader.Checked ? "1" : "0");
                DataTable tblOrder = new DataTable();
                tblOrder = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                this.nudOrder.Value= (tblOrder.Rows.Count > 0 ?(decimal) tblOrder.Rows[0]["Serial"] : 1);
                //this.nudSubOrder.Value = 0;
            }
            if (nudSubOrder.Value == 0)
            {
                SqlStr = "Select SubSerial=isnull(max(SubSerial),0)+1 From Lother Where e_Code='" + vE_Code + "' and Fld_Nm='" + this.txtFldNm.Text.Trim()+"'";
                DataTable tblOrder = new DataTable();
                tblOrder = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                this.nudSubOrder.Value = (tblOrder.Rows.Count > 0 ? (decimal)tblOrder.Rows[0]["SubSerial"] : 1);
                //this.nudSubOrder.Value = 0;
            }
            vTblMain.Rows[0]["e_Code"] = vE_Code;
            
            this.label1.Focus();

            vTblMain.Rows[0]["User_Name"] = this.pAppUerName;
            //vTblMain.Rows[0]["sysDate"] = DateTime.Parse(DateTime.Now.Date.ToString().Substring(0, 10)).ToString("dd/MM/yyyy");
            vTblMain.Rows[0]["sysDate"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").ToString();
            vTblMain.Rows[0]["compid"] = this.pCompId.ToString();
            

                        
            
            vTblMain.Rows[0]["Att_File"] = (rbHeader.Checked ? true : false);
            vTblMain.Rows[0]["Type"] = (rbMast.Checked ? "M" : "T");

            vTblMain.Rows[0]["InPickup"] = (chkDispInPickup.Checked ? true : false);
            vTblMain.Rows[0]["InGrid"] = (chkDispInHeader.Checked ? true : false);
            vTblMain.Rows[0]["Disp_MIS"] = (chkDispInMIS.Checked ? true : false);
            vTblMain.Rows[0]["Lines"] = (ChkNewLine.Checked ? true : false);

            vTblMain.Rows[0]["Whn_Con"] = vWhn_Con;
            vTblMain.Rows[0]["Val_Con"] = vVal_Con;
            vTblMain.Rows[0]["Defa_Val"] = vDefa_Val;
            vTblMain.Rows[0]["Inter_Use"] = vInter_Use;
            vTblMain.Rows[0]["Val_Err"] = vVal_Err;
            vTblMain.Rows[0]["Mandatory"] = vMandatory;

            vTblMain.Rows[0]["filtcond"] = this.txtPopupQuery.Text; //Divyang

            vTblMain.Rows[0].AcceptChanges();
            vTblMain.Rows[0].EndEdit();
            

            string[] vArrkeyField = { "e_code", "fld_nm" };
            //vSaveString = "";
            //this.mSaveCommandString(ref vSaveString,"#Entry_Ty#,#Fld_Nm)#", "#Code_Nm#", vArrkeyField);
            //oDataAccess.ExecuteSQLStatement(vSaveString, null, 20, true);
            SqlStr = vE_Code + " " + this.txtValidIn.Text;

            vEntryList = SqlStr.Split(' ');

            for (int i = 0; i <= vEntryList.Length - 2; i++)
            {
                vSaveString = "";
                vTblMain.Rows[0]["E_Code"] = vEntryList[i];
                SqlStr = "Select m.Head_Nm,TrNName=(Case When isNull(l.Code_Nm,'')='' Then m.E_Code Else l.Code_Nm End),TblNm=(Case When bCode_Nm='' Then l.Entry_Ty Else bCode_Nm End) From Lother m Left Join lCode l on (m.E_Code=l.Entry_Ty) Where m.E_Code='" + vEntryList[i] + "' and Fld_Nm='" + this.txtFldNm.Text.Trim() + "'";
                DataTable tblDuplicate = new DataTable();
                tblDuplicate = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                if (this.pAddMode && tblDuplicate.Rows.Count > 0) { MessageBox.Show("Record already Exists for Transaction Name='" + tblDuplicate.Rows[0]["TrNName"].ToString().Trim() + " and Field Name='" + this.txtFldNm.Text.Trim() + "'"); return; }
                else
                {
                    this.mSaveCommandString(ref vSaveString, "#E_Code#,#Fld_Nm)#", "#Code_Nm#ModuleCode#Prodcode#", vArrkeyField);
                    oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);

                    if (this.pAddMode)
                    {
                        if (this.rbTrn.Checked)
                        {
                            SqlStr = "Select tblName=(Case When Ext_Vou=0 Then Entry_Ty Else bCode_Nm End)+'" + (rbHeader.Checked ? "Main" : "Item") + "' From LCode Where Entry_Ty='" + vEntryList[i] + "'";
                        }
                        else
                        {
                            SqlStr = "Select tblName=filename From MASTCODE Where code='" + vEntryList[i] + "'";
                        }                
                        tblNm = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                        string vDefault= ((cmbType.Text == "Varchar" || cmbType.Text == "Text" || cmbType.Text == "Datetime") ? "''''" : "0");
                        string vDecimal = (cmbType.Text == "Decimal" ? nudDecimal.Value.ToString().Trim() : "");
                        string vWidth = ((cmbType.Text == "Varchar" || cmbType.Text == "Decimal") ? nudWidth.Value.ToString().Trim() : "");
                        if (vWidth != "")
                        {
                            vWidth = "("+vWidth ;
                            vWidth= vWidth+(vDecimal!=""?","+vDecimal:"");
                            vWidth = vWidth + ")";
                        }
                        
                        if (this.txtTblNm.Text.Trim() == "STMainAdd")
                        {
                            tblNm.Rows[0]["tblName"] = this.txtTblNm.Text.Trim();
                        }

                        SqlStr = "Execute Add_Columns '" + tblNm.Rows[0]["tblName"].ToString().Trim() + "','" + this.txtFldNm.Text.Trim() +" "+cmbType.Text+" "+vWidth +" Default "+vDefault+" With Values'";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);

                        SqlStr = "Select Fld_Nm from lOther Where Fld_Nm = 'AmendDate' and e_code ='" + vE_Code + "'";
                        DataTable tblChkAmFld = new DataTable();
                        tblChkAmFld = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                        if(tblChkAmFld.Rows.Count>0)
                        {
                            SqlStr = "Execute Add_Columns '" + tblNm.Rows[0]["tblName"].ToString().Trim() +"AM"+ "','" + this.txtFldNm.Text.Trim() + " " + cmbType.Text + " " + vWidth + " Default " + vDefault + " With Values'";
                            oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                        }

                    }
                    else if(this.pEditMode)
                    {
                        
                        if (this.rbTrn.Checked)
                        {
                            SqlStr = "Select tblName=(Case When Ext_Vou=0 Then Entry_Ty Else bCode_Nm End)+'" + (rbHeader.Checked ? "Main" : "Item") + "' From LCode Where Entry_Ty='" + vEntryList[i] + "'";
                        }
                        else
                        {
                            SqlStr = "Select tblName=filename From MASTCODE Where code='" + vEntryList[i] + "'";
                        }
                        tblNm = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

                        SqlStr = "select * from syscolumns inner join sysobjects on (syscolumns.id=sysobjects.id) where syscolumns.name='" + vTblMain.Rows[0]["fld_nm"] + "' and sysobjects.name='" + tblNm.Rows[0]["tblName"].ToString().Trim() + "' ";
                        DataTable vTblChkFld = new DataTable();
                        vTblChkFld = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

                        if (vTblChkFld.Rows.Count == 0)
                        {

                            string vDefault = ((cmbType.Text == "Varchar" || cmbType.Text == "Text" || cmbType.Text == "Datetime") ? "''''" : "0");
                            string vDecimal = (cmbType.Text == "Decimal" ? nudDecimal.Value.ToString().Trim() : "");
                            string vWidth = ((cmbType.Text == "Varchar" || cmbType.Text == "Decimal") ? nudWidth.Value.ToString().Trim() : "");
                            if (vWidth != "")
                            {
                                vWidth = "(" + vWidth;
                                vWidth = vWidth + (vDecimal != "" ? "," + vDecimal : "");
                                vWidth = vWidth + ")";
                            }

                            if (this.txtTblNm.Text.Trim() == "STMainAdd")
                            {
                                tblNm.Rows[0]["tblName"] = this.txtTblNm.Text.Trim();
                            }

                            SqlStr = "Execute Add_Columns '" + tblNm.Rows[0]["tblName"].ToString().Trim() + "','" + this.txtFldNm.Text.Trim() + " " + cmbType.Text + " " + vWidth + " Default " + vDefault + " With Values'";
                            oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);

                            SqlStr = "Select Fld_Nm from lOther Where Fld_Nm = 'AmendDate' and e_code ='" + vE_Code + "'";
                            DataTable tblChkAmFld = new DataTable();
                            tblChkAmFld = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                            if (tblChkAmFld.Rows.Count > 0)
                            {
                                SqlStr = "Execute Add_Columns '" + tblNm.Rows[0]["tblName"].ToString().Trim() + "AM" + "','" + this.txtFldNm.Text.Trim() + " " + cmbType.Text + " " + vWidth + " Default " + vDefault + " With Values'";
                                oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                            }
                        }
                    }

                }
            }


            vTblMain.Rows[0]["E_Code"] = vE_Code;
            vMainFldVal = vTblMain.Rows[0][vMainField].ToString().Trim();
            vMainFldVal1 = vTblMain.Rows[0][vMainField1].ToString().Trim();
            if (this.rbTrn.Checked)
            {
                SqlStr = "Select top 1 l.Code_Nm,m.* from " + vTblMainNm + " m left Join lCode l on (m.E_Code=l.Entry_Ty) Where " + vMainField + "='" + vMainFldVal + "' and m." + vMainField1 + "='" + vMainFldVal1 + "'";
            }
            else
            {
                SqlStr = "Select top 1 l.Name as Code_Nm,m.* from lOther m left Join MASTCODE l on (m.E_Code=l.Code) Where " + vMainField + "='" + vMainFldVal + "' and m." + vMainField1 + "='" + vMainFldVal1 + "'";
            }
            //SqlStr = "Select top 1 l.Code_Nm,m.* from " + vTblMainNm + " m Join lCode l on (m.Entry_Ty=l.Entry_Ty)  order by rtrim(" + vMainField + ")+rtrim(l." + vMainField1 + ")";
            vTblMain = oDataAccess.GetDataTable(SqlStr, null, 20);

            this.pAddMode = false;
            this.pEditMode = false;
            this.mthEnableDisableFormControls();
            this.mthView();

        }
        private void mSaveCommandString(ref string vSaveString, string vkeyField, string vIgnoreFld, string[] vArrkeyField)
        {
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;

            /*Identity Columns--->*/
            DataSet dsData = new DataSet();
            SqlStr = "select c.name as ColName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            SqlStr = SqlStr + " and o.name='" + vTblMainNm + " ' ";
            dsData = oDataAccess.GetDataSet(SqlStr, null, 20);
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#" + dr1["ColName"].ToString().Trim() + "#";
            }

            /*<---Identity Columns--->*/
            if (this.pAddMode == true)
            {
                vSaveString = "Set DateFormat dmy insert into " + vTblMainNm;
                vTblMain.AcceptChanges();
                foreach (DataColumn dtc1 in vTblMain.Columns)
                {
                    if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0 && vIgnoreFld.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                    {
                        if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                        if (string.IsNullOrEmpty(vfldValList) == false) { vfldValList = vfldValList + ","; }
                        vfldList = vfldList + dtc1.ToString().Trim();
                        vfldVal = vTblMain.Rows[0][dtc1.ToString().Trim()].ToString().Trim();

                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal + "'";
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

                SqlStr = "select * from lother where e_code='" + vTblMain.Rows[0]["e_code"].ToString().Trim() + "' and fld_nm='" + vTblMain.Rows[0]["fld_nm"].ToString().Trim() + "'";
                DataTable tblValidIn = new DataTable();
                tblValidIn = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

                if (tblValidIn.Rows.Count > 0)
                {
                    vSaveString = "Set DateFormat dmy Update " + vTblMainNm + "  Set ";
                    string vWhereCondn = string.Empty;
                    foreach (DataColumn dtc1 in vTblMain.Columns)
                    {
                        if (vIgnoreFld.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                        {
                            vfldVal = vTblMain.Rows[0][dtc1.ToString().Trim()].ToString().Trim();
                            if (dtc1.DataType.Name.ToLower() == "string")
                            {
                                vfldVal = "'" + vfldVal + "'";
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

                            //if ((vkeyField.ToLower().IndexOf("#" + dtc1.ToString().Trim().ToLower() + "#") > -1))
                            if (Array.Exists(vArrkeyField, element => element == dtc1.ToString().Trim().ToLower()) == true)
                            {
                                if (string.IsNullOrEmpty(vWhereCondn) == false) { vWhereCondn = vWhereCondn + " and "; } else { vWhereCondn = " Where "; }
                                vWhereCondn = vWhereCondn + dtc1.ToString().Trim() + " = ";
                                vfldVal = vTblMain.Rows[0][dtc1.ToString().Trim()].ToString().Trim();
                                if (dtc1.DataType.Name.ToLower() == "string")
                                {
                                    vfldVal = "'" + vfldVal + "'";
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
                else    //ValidIn in edit
                {
                    vSaveString = "Set DateFormat dmy insert into " + vTblMainNm;
                    vTblMain.AcceptChanges();
                    foreach (DataColumn dtc1 in vTblMain.Columns)
                    {
                        if (vIdentityFields.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0 && vIgnoreFld.IndexOf("#" + dtc1.ToString().Trim() + "#") < 0)
                        {
                            if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                            if (string.IsNullOrEmpty(vfldValList) == false) { vfldValList = vfldValList + ","; }
                            vfldList = vfldList + dtc1.ToString().Trim();
                            vfldVal = vTblMain.Rows[0][dtc1.ToString().Trim()].ToString().Trim();

                            if (dtc1.DataType.Name.ToLower() == "string")
                            {
                                vfldVal = "'" + vfldVal + "'";
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
        }
        private void mthChkSaveValidation(ref string vValid)
        {
            if (string.IsNullOrEmpty(this.txtTrnType.Text.Trim())) { vValid = "Empty Transaction Type not allowed!"; this.txtTrnType.Focus(); return; }
            if (string.IsNullOrEmpty(this.txtHeadNm.Text.Trim())) { vValid = "Empty Heading name not allowed!"; this.txtHeadNm.Focus(); return; }
            if (string.IsNullOrEmpty(this.txtFldNm.Text.Trim())) { vValid = "Empty Field name not allowed!"; this.txtFldNm.Focus(); return; }


            if (this.nudOrder.Value==0)
            {
                SqlStr = "Select SrNo=isNull(Max(Serial),0)+1 From Lother Where e_Code='" + vE_Code+"' and Att_File="+(rbHeader.Checked?"1":"0");
                DataTable tblSrNo = new DataTable();
                tblSrNo = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                this.nudOrder.Value =(decimal) tblSrNo.Rows[0]["SrNo"];
            }
            if (this.nudSubOrder.Value == 0)
            {   
                this.nudSubOrder.Value =1;
            }
            vTblMain.Rows[0]["Serial"] = this.nudOrder.Text;
            vTblMain.Rows[0]["SubSerial"] = this.nudSubOrder.Text;

            DataTable tblReservedWord = new DataTable();
            SqlStr = "Select kWord from vudyog..RsvKWords order by kword";
            tblReservedWord = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            foreach (DataRow dr in tblReservedWord.Rows)
            {
                if (dr["kWord"].ToString().Trim().ToUpper() == this.txtFldNm.Text.Trim().ToUpper()) { vErrMsg = "Reserve Keywords '"+ dr["kWord"].ToString().Trim()+"' Not Allowed.";this.txtFldNm.Focus();return; }
            }

            if (this.pAddMode && (this.rbMast.Checked))
            {
                SqlStr="Select a.Name from SysObjects a inner Join SysColumns b where a.Id = b.Id and a.Name = '"+this.txtTblNm.Text+"'";
                DataTable tmpColCount = new DataTable();
                
                if (tmpColCount.Rows.Count > 254) { vErrMsg = "Column Exceeds more than Table Limit!! " + "\n"+ "Could Not Generate Column";  return; }
            }
            
            if (this.pAddMode) { vErrMsg = this.FuncAddTableCreation(); if (vErrMsg != "") { return; } }
//            if (this.pAddMode) { vErrMsg = this.functAmendmentTableCreation(); if (vErrMsg != "") { return; } }
            if (this.pAddMode && this.rbTrn.Checked && this.txtTblNm.Text.IndexOf("Add")<=0) { this.txtTblNm.Text = (this.rbHeader.Checked ? "lmc" : "lIt"); }
            vTblMain.Rows[0]["Tbl_Nm"] = this.txtTblNm.Text;

            //Field Structure Validation    
            //Same Field Validation

        }
        private string FuncAddTableCreation()
        {   
            SqlStr = "Select SUM(Max_Length) as MaxLength from sys.columns where Object_Id = Object_Id('"+this.txtTblNm.Text.Trim()+"')";
            DataTable tblMaxLngth = new DataTable();
            tblMaxLngth = oDataAccess.GetDataTable(SqlStr,null,vTimeOut);
            int vMaxLength = (int)tblMaxLngth.Rows[0]["MaxLength"]; //Add Divyang
            //if(tblMaxLngth.Rows.Count> 8060)  //Comm Divyang
            if (vMaxLength > 8060)  //Add Divyang
            {
                string vWarningMsg = "Warning : ";
                vWarningMsg = vWarningMsg + "\n" + "The total row size in Sql is exceeding the maximum row size limit of 8060 bytes in table " + this.txtTblNm.Text;
                vWarningMsg = vWarningMsg + "\n" + "INSERT or UPDATE to this table will fail if the resulting row exceeds ";
                vWarningMsg = vWarningMsg + "\n" + "the size limit.";
                vWarningMsg = vWarningMsg + "\n" + "Do you want to create the field in an additional table? ";
                vWarningMsg = vWarningMsg + "\n" + "'YES' will create the field in new table else in the main table.";
                DialogResult vResult = MessageBox.Show(vWarningMsg, this.pPApplText, MessageBoxButtons.YesNo);
                if(vResult==DialogResult.Yes) 
                {
                    try
                    {
                        //Add table Creation Start
                        string vAddtblNm = vOrgTblNm + "Add";
                        SqlStr = "IF NOT EXISTS(SELECT [NAME] FROM SYS.TABLES WHERE [NAME] = '" + vAddtblNm + "')";
                        SqlStr = SqlStr + " BEGIN ";
                        SqlStr = SqlStr + " CREATE TABLE " + vAddtblNm + " ( ";
                        SqlStr = SqlStr + "     [ENTRY_TY] [varchar](2) NOT NULL, ";
                        SqlStr = SqlStr + "[TRAN_CD] [int] NOT NULL ";
                        SqlStr = SqlStr + ") ON [PRIMARY] ";
                        SqlStr = SqlStr + " END ";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);

                        SqlStr = "IF NOT EXISTS(SELECT [NAME] FROM SYS.OBJECTS WHERE TYPE='F' AND [NAME] = 'FK_" + vAddtblNm + "_" + vOrgTblNm + "') ";
                        SqlStr = SqlStr+"\n" + "BEGIN ";
                        SqlStr = SqlStr + "	ALTER TABLE [dbo].[" + vAddtblNm + "]  WITH CHECK ADD  CONSTRAINT [FK_" + vAddtblNm + "_"+ vOrgTblNm + "] FOREIGN KEY([TRAN_CD]) " ;
                        SqlStr = SqlStr + "		REFERENCES [dbo].[" + this.txtTblNm.Text.Trim() + "] ([Tran_cd]) ";
                        SqlStr = SqlStr + "END ";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);

                        SqlStr = "IF NOT EXISTS(SELECT [NAME] FROM SYS.OBJECTS WHERE TYPE='F' AND [NAME] = 'FK_" + vAddtblNm + "_" + vOrgTblNm + "') ";
                        SqlStr = SqlStr + "\n" + "BEGIN ";
                        SqlStr = SqlStr + "	ALTER TABLE [dbo].[" + vAddtblNm + "]  CHECK CONSTRAINT [FK_" + vAddtblNm + "_" + vOrgTblNm + "]";
                        SqlStr = SqlStr + " END ";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);

                        this.functAmendmentTableCreation();

                    }
                    catch (Exception ex)
                    {
                        vErrMsg = ex.Message;
                        return vErrMsg;
                    }
                    this.txtTblNm.Text = this.txtTblNm.Text + "Add";


                }//if(vResult==DialogResult.Yes)
            }
            return vErrMsg;
        }
        private string functAmendmentTableCreation()
        {
            string vAmblNm = "";
            
            SqlStr ="Select Fld_Nm from lOther Where Fld_Nm = 'AmendDate' and e_code ='"+vE_Code+"'";
            DataTable tblChkAmFld = new DataTable();
            tblChkAmFld = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
            string vTxtTblNm = this.txtTblNm.Text.Trim().ToUpper(); //Divyang
            if (vTxtTblNm.IndexOf("ADD") > -1)  //Divyang
            {
                 vAmblNm = this.txtTblNm.Text + "Am";
                vOrgTblNm = vOrgTblNm + "Am";
            }
            else
            {
                 vAmblNm = vOrgTblNm + "Am";
                //vOrgTblNm = vOrgTblNm + "Am";
            }
             //vAmblNm = vOrgTblNm + "Am";    //Comm Divyang
            if (tblChkAmFld.Rows.Count > 0)
            {
                SqlStr = "IF NOT EXISTS(SELECT [NAME] FROM SYS.TABLES WHERE [NAME] = '" + vAmblNm+"')";
                SqlStr = SqlStr + " BEGIN ";
                SqlStr = SqlStr + "     CREATE TABLE " + vAmblNm + "( ";
                SqlStr = SqlStr + "	[ENTRY_TY] [varchar](2) NOT NULL, ";
                SqlStr = SqlStr + "	[TRAN_CD] [int] NOT NULL ";
                SqlStr = SqlStr + "	) ON [PRIMARY] ";
                SqlStr = SqlStr + " END ";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);

                SqlStr = "IF NOT EXISTS(SELECT [NAME] FROM SYS.OBJECTS WHERE TYPE='F' AND [NAME] = 'FK_" + vAmblNm+"_" + vOrgTblNm+ "') ";
                SqlStr = SqlStr + " BEGIN " ;
                SqlStr = SqlStr + "	ALTER TABLE [dbo].[" + vAmblNm + "]  WITH CHECK ADD  CONSTRAINT [FK_" + vAmblNm + "_" + vOrgTblNm+ "] FOREIGN KEY([TRAN_CD]) " ;
                //SqlStr = SqlStr + "		REFERENCES [dbo].[" + vAmblNm + "] ([Tran_cd]) " ;
                SqlStr = SqlStr + "		REFERENCES [dbo].[" + vOrgTblNm + "] ([Tran_cd]) "; //Divyang
                SqlStr = SqlStr + " END ";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);

                SqlStr = "IF NOT EXISTS(SELECT [NAME] FROM SYS.OBJECTS WHERE TYPE='F' AND [NAME] = 'FK_" + vAmblNm + "_" + vOrgTblNm + "') ";
                SqlStr = SqlStr + " BEGIN ";
                SqlStr = SqlStr + "	    ALTER TABLE [dbo].[" + vAmblNm + "] CHECK CONSTRAINT [FK_" + vAmblNm + "_" + vOrgTblNm + "] " ;
                SqlStr = SqlStr + " END ";

            }
            return vErrMsg;
        }
        private void txtFldNm_KeyPress(object sender, KeyPressEventArgs e)
        {
            int vAsccii = ((int)e.KeyChar);
            if ((funcBetween(vAsccii, 48, 57) == false && funcBetween(vAsccii, 97, 122) == false && funcBetween(vAsccii, 65, 90) == false && (vAsccii != 22 || vAsccii != 127 || vAsccii != 13 || vAsccii != 4 || vAsccii != 19 || vAsccii != 7 || vAsccii != 9)) == true && (vAsccii == 95 || vAsccii == 8) ==false)
            { e.Handled = true; }
        }
        private bool funcBetween(int vVal, int vMinVal, int vMaxVal)
        {
            bool vResult = false;
            if (vVal >= vMinVal && vVal <= vMaxVal) { vResult = true; }
            return vResult;
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.pAddMode = false;
            this.pEditMode = false;
            //Iscancel = true;
            this.mthCancel(sender, e);
            this.mthChkNavigationButton();
            this.btnLast_Click(sender, e);
        }
        private void mthCancel(object sender, EventArgs e)
        {
            if (this.vTblMain.Rows.Count != 0)
            {
                vTblMain.Rows[0].CancelEdit();
            }

            this.mthEnableDisableFormControls();
            if (this.vTblMain.Rows.Count == 1)
            {
                vTblMain.Rows[0].CancelEdit();
                if (this.pAddMode)
                {
                    vTblMain.Rows[0].Delete();
                    this.btnLast_Click(sender, e);
                }
                if (this.pEditMode)
                {

                    vMainFldVal = vTblMain.Rows[0][vMainField].ToString().Trim();
                    SqlStr = "Select top 1 * from " + vTblMainNm + " Where " + vMainField + "='" + vMainFldVal + "'";
                    vTblMain = oDataAccess.GetDataTable(SqlStr, null, 20);
                    this.mthView();
                }
            }
            this.pAddMode = false;
            this.pEditMode = false;

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
                btnDelete_Click(this.btnDelete, e);
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.mcheckCallingApplication();
            this.mthDelete();
        }
        private void mthDelete()
        {
            if (this.vTblMain.Rows.Count <= 0)
            {
                return;
            }

            if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string vDelString = string.Empty;
                vMainFldVal = vTblMain.Rows[0][vMainField].ToString().Trim();
                string vFldval = vTblMain.Rows[0]["fld_nm"].ToString().Trim();
                string vFldTbl = vTblMain.Rows[0]["tbl_nm"].ToString().Trim();

                SqlStr = "Select tblName = (Case When Ext_Vou = 0 Then Entry_Ty Else bCode_Nm End) From LCode Where Entry_Ty = '" + vE_Code + "' ";
                DataTable vTblBcode = new DataTable();
                vTblBcode = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                string bCode = vTblBcode.Rows[0]["tblName"].ToString().Trim();
                if (vFldTbl.ToLower()=="lmc") { vFldTbl = bCode+"Main";}
                if (vFldTbl.ToLower() == "lit") { vFldTbl = bCode + "Item"; }


                SqlStr = " SELECT A.FLD_NM,B.bcode_nm,B.code_nm,A.e_code,b.entry_ty,D.Entry_ty,C.E_CODE FROM LOTHER A ";
                SqlStr = SqlStr + "inner JOIN LCODE B ON(A.e_code = B.Entry_ty)  LEFT OUTER JOIN LOTHER C ON (C.FLD_NM = A.FLD_NM)";
                SqlStr = SqlStr + " LEFT JOIN LCODE D ON(A.e_code = D.Entry_ty AND(B.Entry_ty = D.bcode_nm OR B.bcode_nm = D.bcode_nm)) ";
                SqlStr = SqlStr + "WHERE A.FLD_NM = '"+vFldval+"'  and  a.e_code = '"+vE_Code+"' ";
                DataTable vTblDblFld = new DataTable();
                vTblDblFld = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);


                // vDelString = "Delete from " + vTblMainNm + "  Where dept=" + vTblMain.Rows[0]["dept"].ToString();
                vDelString = "Delete from " + vTblMainNm + "  Where head_nm='" +vMainFldVal+ "' and e_code ='"+vE_Code+"' ";
                oDataAccess.ExecuteSQLStatement(vDelString, null, 20, true);
                if (vTblDblFld.Rows.Count < 2)
                {
                    SqlStr = " select consnm=name from sysobjects where id in (select syscolumns.cdefault from syscolumns inner join sysobjects on (syscolumns.id=sysobjects.id) where syscolumns.name='" + vFldval + "' and sysobjects.name='" + vFldTbl + "') ";
                    DataTable vTblConst = new DataTable();
                    vTblConst = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                    if (vTblConst.Rows.Count > 0)
                    {
                        string vConstString = vTblConst.Rows[0]["consnm"].ToString().Trim();

                        string vDelConstStr = "alter table " + vFldTbl + " drop " + vConstString + " ";
                        oDataAccess.ExecuteSQLStatement(vDelConstStr, null, 20, true);
                    }
                    string vDelFldStr = " alter table " + vFldTbl + " Drop column " + vFldval + " ";
                    oDataAccess.ExecuteSQLStatement(vDelFldStr, null, 20, true);
                    //}

                    SqlStr = "Select Fld_Nm from lOther Where Fld_Nm = 'AmendDate' and e_code ='" + vE_Code + "'";          //Amednment Table Delete
                    DataTable tblChkAmFld = new DataTable();
                    tblChkAmFld = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                    if (tblChkAmFld.Rows.Count > 0)
                    {
                        SqlStr = " select consnm=name from sysobjects where id in (select syscolumns.cdefault from syscolumns inner join sysobjects on (syscolumns.id=sysobjects.id) where syscolumns.name='" + vFldval + "' and sysobjects.name='" + vFldTbl + "am" + "') ";
                        DataTable vTblConstAm = new DataTable();
                        vTblConstAm = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                        if (vTblConstAm.Rows.Count > 0)
                        {
                            string vConstString = vTblConstAm.Rows[0]["consnm"].ToString().Trim();

                            string vDelConstStr = "alter table " + vFldTbl + "am" + " drop " + vConstString + " ";
                            oDataAccess.ExecuteSQLStatement(vDelConstStr, null, 20, true);
                        }
                        string vDelFldStrAm = " alter table " + vFldTbl + "am" + " Drop column " + vFldval + " ";
                        oDataAccess.ExecuteSQLStatement(vDelFldStrAm, null, 20, true);
                    }
                }
                


                this.vTblMain.Rows[0].Delete();
                this.vTblMain.AcceptChanges();
                
                if (this.btnForward.Enabled)
                {
                    //SqlStr = "Select top 1 * from " + vTblMainNm + "  Where " + vMainField + "='" + vMainFldVal + "' order by " + vMainField;
                    SqlStr = "Select top 1 Code_Nm='" + this.txtTrnType.Text + "',* from " + vTblMainNm + "   order by " + vMainField;
                    vTblMain = oDataAccess.GetDataTable(SqlStr, null, 25);
                    vMainFldVal = vTblMain.Rows[0][vMainField].ToString().Trim();
                }
                else
                {
                    if (this.btnBack.Enabled)
                    {
                        //SqlStr = "Select top 1 * from " + vTblMainNm + "  Where " + vMainField + "<'" + vMainFldVal + "' order by " + vMainField + " desc";
                        SqlStr = "Select top 1 Code_Nm='" + this.txtTrnType.Text + "',* from " + vTblMainNm + "   order by " + vMainField + " desc";
                        vTblMain = oDataAccess.GetDataTable(SqlStr, null, 25);
                        vMainFldVal = vTblMain.Rows[0][vMainField].ToString().Trim();
                    }

                }
                this.mthView();
                this.mthChkNavigationButton();
            }

        }

        private void mthView()
        {
            this.mthBindClear();
            this.mthBindData();

        }
        private void mthBindData()
        {
            if (vTblMain == null)
            {
                return;
            } else if (vTblMain.Rows.Count == 0) { return; }

            vE_Code = vTblMain.Rows[0]["e_Code"].ToString();
            this.txtTblNm.DataBindings.Add("Text", vTblMain, "Tbl_Nm");
            this.txtFldNm.DataBindings.Add("Text", vTblMain, "Fld_Nm");
            this.cmbType.DataBindings.Add("Text", vTblMain, "Data_Ty");
            this.nudWidth.DataBindings.Add("Text", vTblMain, "Fld_Wid");
            this.nudDecimal.DataBindings.Add("Text", vTblMain, "Fld_Dec");
            //this.txtPopupQuery.DataBindings.Add("Text", vTblMain, "Filt_Cond");
            this.txtValidIn.DataBindings.Add("Text", vTblMain, "Validity");
            this.txtRemark.DataBindings.Add("Text", vTblMain, "Remarks");
            this.txtTrnType.DataBindings.Add("Text", vTblMain, "Code_Nm");
            this.txtHeadNm.DataBindings.Add("Text", vTblMain, "Head_Nm");
            this.txtPopupQuery.DataBindings.Add("Text", vTblMain, "FiltCond");
            this.nudOrder.DataBindings.Add("Text", vTblMain, "Serial");
            this.nudSubOrder.DataBindings.Add("Text", vTblMain, "SubSerial");
            
            this.txtFormDesc.DataBindings.Add("Text", vTblMain, "FormDesc");
            this.txtFormHeading.DataBindings.Add("Text", vTblMain, "Heading");
            this.nudFormOrder.DataBindings.Add("Text", vTblMain, "FormNo");

            
            chkDispInPickup.Checked = (vTblMain.Rows[0]["InPickup"].ToString().Trim() == "False" ? false : true);
            chkDispInHeader.Checked = (vTblMain.Rows[0]["InGrid"].ToString().Trim() == "False" ? false : true);
            chkDispInMIS.Checked = (vTblMain.Rows[0]["Disp_MIS"].ToString().Trim() == "False" ? false : true);
            ChkNewLine.Checked = (vTblMain.Rows[0]["Lines"].ToString().Trim() == "False" ? false : true);

            chkWhen.Checked = (vTblMain.Rows[0]["Whn_Con"].ToString().Trim() == "" ? false : true);
            chkValid.Checked = (vTblMain.Rows[0]["Val_Con"].ToString().Trim() == "" ? false : true);
            chkDefault.Checked = (vTblMain.Rows[0]["Defa_Val"].ToString().Trim() == "" ? false : true);
            chkInternal.Checked = (vTblMain.Rows[0]["Inter_Use"].ToString().Trim() == "" ? false : true);
            chkError.Checked =     (vTblMain.Rows[0]["Val_Err"].ToString().Trim()   == "" ? false : true);
            chkMandatory.Checked = (vTblMain.Rows[0]["Mandatory"].ToString().Trim() == "" ? false : true);

            rbMast.Checked = (vTblMain.Rows[0]["Type"].ToString().Trim() == "M" ? true : false);
            rbTrn.Checked = (vTblMain.Rows[0]["Type"].ToString().Trim() == "T" ? true : false);
            rbDetail.Checked = (vTblMain.Rows[0]["att_file"].ToString().Trim() == "False" ? true : false);
            rbHeader.Checked = (vTblMain.Rows[0]["att_file"].ToString().Trim() == "True" ? true : false);
        }
        private void mthBindClear()
        {
            this.txtTblNm.DataBindings.Clear();
            this.txtFldNm.DataBindings.Clear();
            this.cmbType.DataBindings.Clear();
            this.nudWidth.DataBindings.Clear();
            this.nudDecimal.DataBindings.Clear();
            this.txtPopupQuery.DataBindings.Clear();
            this.txtValidIn.DataBindings.Clear();
            this.txtRemark.DataBindings.Clear();
            this.txtTrnType.DataBindings.Clear();
            this.txtHeadNm.DataBindings.Clear();
            this.txtPopupQuery.DataBindings.Clear();
            this.nudOrder.DataBindings.Clear();
            this.nudSubOrder.DataBindings.Clear();
            this.txtFormDesc.DataBindings.Clear();
            this.txtFormHeading.DataBindings.Clear();
            this.nudFormOrder.DataBindings.Clear();
        }
        private void chkWhen_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(vColVal);
            
            udTextEdit.cudTextEdit oudTextEdit = new udTextEdit.cudTextEdit();
            oudTextEdit.pAddMode = this.pAddMode;
            oudTextEdit.pEditMode = this.pEditMode;
            oudTextEdit.pICon = this.Icon;
            oudTextEdit.pTextVal = vWhn_Con;
            oudTextEdit.pFrmCaption = "When Condition ";
            oudTextEdit.mthCallTextEdit();
            vWhn_Con = oudTextEdit.pTextVal;
            vWhn_Con = vWhn_Con.Replace(@"'", @"''");
            this.chkWhen.Checked = (vWhn_Con == "" ? false : true);            
        }
        private void chkValid_Click(object sender, EventArgs e)
        {
            udTextEdit.cudTextEdit oudTextEdit = new udTextEdit.cudTextEdit();
            oudTextEdit.pAddMode = this.pAddMode;
            oudTextEdit.pEditMode = this.pEditMode;
            oudTextEdit.pICon = this.Icon;
            oudTextEdit.pTextVal = vVal_Con;
            oudTextEdit.pFrmCaption = "Valid Condition ";
            oudTextEdit.mthCallTextEdit();
            vVal_Con = oudTextEdit.pTextVal;
            vVal_Con = vVal_Con.Replace(@"'", @"''");
            this.chkValid.Checked = (vVal_Con == "" ? false : true);
        }
        private void chkMandatory_Click(object sender, EventArgs e)
        {
            udTextEdit.cudTextEdit oudTextEdit = new udTextEdit.cudTextEdit();
            oudTextEdit.pAddMode = this.pAddMode;
            oudTextEdit.pEditMode = this.pEditMode;
            oudTextEdit.pICon = this.Icon;
            oudTextEdit.pTextVal =vMandatory ;
            oudTextEdit.pFrmCaption = "Mandatory Condition ";
            oudTextEdit.mthCallTextEdit();
            vMandatory = oudTextEdit.pTextVal;
            vMandatory = vMandatory.Replace(@"'", @"''");
            this.chkMandatory.Checked = (vMandatory == "" ? false : true);
        }
        private void chkInternal_Click(object sender, EventArgs e)
        {
            udTextEdit.cudTextEdit oudTextEdit = new udTextEdit.cudTextEdit();
            oudTextEdit.pAddMode = this.pAddMode;
            oudTextEdit.pEditMode = this.pEditMode;
            oudTextEdit.pICon = this.Icon;
            oudTextEdit.pTextVal = vInter_Use;
            oudTextEdit.pFrmCaption = "Internal Use Condition ";
            oudTextEdit.mthCallTextEdit();
            vInter_Use = oudTextEdit.pTextVal;
            vInter_Use = vInter_Use.Replace(@"'", @"''");
            this.chkInternal.Checked = (vInter_Use == "" ? false : true);
        }
        private void chkError_Click(object sender, EventArgs e)
        {
            udTextEdit.cudTextEdit oudTextEdit = new udTextEdit.cudTextEdit();
            oudTextEdit.pAddMode = this.pAddMode;
            oudTextEdit.pEditMode = this.pEditMode;
            oudTextEdit.pICon = this.Icon;
            oudTextEdit.pTextVal = vVal_Err;
            oudTextEdit.pFrmCaption = "Error Message ";
            oudTextEdit.mthCallTextEdit();
            vVal_Err = oudTextEdit.pTextVal;
            vVal_Err = vVal_Err.Replace(@"'", @"''");
            this.chkError.Checked = (vVal_Err == "" ? false : true);
        }

        private void btnHeadNm_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication();
            string VForText = string.Empty, vSearchCol = string.Empty, SqlStr = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblTrn = new DataTable();
            //SqlStr = "select dept,add1,add2,add3,phone,validity from " + vTblMainNm + " order by " + vMainField;
            //SqlStr = "Select l.Entry_Ty,m.Head_Nm,m.Fld_Nm,TrnNm=l.Code_Nm From lOther m Left Join (Select Entry_Ty,Code_Nm From lCode union Select Code,[Name] From MastCode) l on (m.e_Code=l.Entry_TY) order by Head_Nm,l.Code_Nm";
            SqlStr = "Select l.Entry_Ty,m.Head_Nm,m.Fld_Nm,TrnNm=l.Code_Nm From lOther m Left Join (Select Entry_Ty,Code_Nm From lCode union Select Code,[Name] From MastCode) l on (m.e_Code=l.Entry_TY) where  code_nm is not null  ";
            tblTrn = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            DataView dvw = tblTrn.DefaultView;
            VForText = "Select Heading Name";
            //vSearchCol = "Head_Nm,TrnNm,Entry_Ty,Fld_Nm";
            vSearchCol = "Head_Nm";
            //vDisplayColumnList = "Head_Nm:Heading Name,TrnNm:Transaction Name,Entry_Ty:Type,Fld_Nm:Field Name";
            vDisplayColumnList = "Entry_Ty:Type,Head_Nm:Heading Name,Fld_Nm:Field Name,TrnNm:Transaction Name";
            //vReturnCol = "Head_Nm,TrnNm,Entry_Ty,Fld_Nm";
            vReturnCol = "Entry_Ty,Head_Nm,Fld_Nm,TrnNm";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            //vMainFldVal = vTblMain.Rows[0]["dept"].ToString().Trim();
            string vFldNm = "";
            if (oSelectPop.pReturnArray != null)
            {
                this.txtHeadNm.Text = oSelectPop.pReturnArray[1];
                this.txtTrnType.Text = vFldNm = oSelectPop.pReturnArray[3];
                vE_Code = oSelectPop.pReturnArray[0];
                vFldNm = oSelectPop.pReturnArray[2];

            }
            if (vFldNm == "") { return; } // Divyang
            SqlStr = "Select Top 1 Code_Nm='" + this.txtTrnType.Text + "',* From lOther Where e_Code='" + vE_Code + "' and Head_Nm='" + this.txtHeadNm.Text + "' and Fld_Nm='" + vFldNm + "'";
            vTblMain = oDataAccess.GetDataTable(SqlStr, null, 20);
            this.mthView();
            this.mthChkNavigationButton();
        }
        private void btnTrnType_Click(object sender, EventArgs e)
        {
            
            mcheckCallingApplication();
            string VForText = string.Empty, vSearchCol = string.Empty, SqlStr = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblTrn = new DataTable();
            if (rbTrn.Checked) { SqlStr = "Select TrnNm=l.Code_Nm,BehNm=isNull(l1.Code_Nm,''),l.Entry_Ty,bCode_Nm=(Case When isnull(l.bCode_Nm,'')='' Then l.Entry_Ty Else l.bCode_Nm End),Tbl_Nm='StMain' From lCode l Left Join lCode l1 on (l.bCode_Nm=l1.Entry_Ty) Order By TrnNm"; }
            else { SqlStr= "Select TrnNm = Name,BehNm = Name,Entry_Ty = Code,bCode_Nm = Code,Tbl_Nm =[FileName] From MastCode Order by TrnNm";}
            tblTrn = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

            DataView dvw = tblTrn.DefaultView;
            //VForText = "Select Transaction/Master Name";  //Comm by Divyang
            VForText = "Select " +label2.Text ;     //Added by Divyang
            vSearchCol = "TrnNm";
            vDisplayColumnList = "TrnNm:Name,BehNm:Behaviour Name";
            vReturnCol = "TrnNm,Entry_Ty,bCode_Nm,Tbl_Nm";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();

            if (oSelectPop.pReturnArray != null)
            {
                this.txtTrnType.Text = oSelectPop.pReturnArray[0];
                vE_Code = oSelectPop.pReturnArray[1];
                vBode_Nm = oSelectPop.pReturnArray[2];
                this.txtTblNm.Text= oSelectPop.pReturnArray[3];
            }
            if(rbTrn.Checked) {this.txtTblNm.Text = vBode_Nm + (rbHeader.Checked ? "Main" : "Item");}
            vOrgTblNm = this.txtTblNm.Text;
        }

        private void txtHeadNm_Leave(object sender, EventArgs e)
            {
            this.nudOrder.Value = 0;
            this.nudSubOrder.Value = 0;

            if (txtHeadNm.Text != "")
            {
                if (nudOrder.Value == 0)
                {
                    SqlStr = "Select Serial=max(isNull(Serial,0))+1 From Lother Where e_code='" + vE_Code + "' and Att_File=" + (rbHeader.Checked ? "1" : "0");
                    DataTable tblOrder = new DataTable();
                    tblOrder = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                    if (tblOrder.Rows[0]["Serial"].ToString().Trim() == "")
                    {
                        this.nudOrder.Value = 1;
                    }
                    else
                    { this.nudOrder.Value = (tblOrder.Rows.Count > 0 ? (decimal)tblOrder.Rows[0]["Serial"] : 1); }
                    //this.nudSubOrder.Value = 0;
                }
                if (nudSubOrder.Value == 0)
                {
                    SqlStr = "Select SubSerial=isnull(max(SubSerial),0)+1 From Lother Where e_Code='" + vE_Code + "' and Fld_Nm='" + this.txtFldNm.Text.Trim() + "'";
                    DataTable tblOrder = new DataTable();
                    tblOrder = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);
                    this.nudSubOrder.Value = (tblOrder.Rows.Count > 0 ? (decimal)tblOrder.Rows[0]["SubSerial"] : 1);
                    //this.nudSubOrder.Value = 0;
                }
            }
            if (txtHeadNm.Text != "")
            {
                string vTxtHeadNm = this.txtHeadNm.Text.Trim();
                if (vTxtHeadNm.IndexOf("'") > -1 || vTxtHeadNm.IndexOf("\"") > -1)
                {
                    MessageBox.Show("Quotation are Not Allowed in Heading Name"); this.txtHeadNm.Focus(); return;
                }
            }
        }

        private void cmbType_Leave(object sender, EventArgs e)          //Divyang
        {
            if (cmbType.Text == "Varchar") { this.nudDecimal.Enabled = false; this.nudWidth.Enabled = true; this.nudWidth.Value = 10; }
            if (cmbType.Text == "Decimal") { this.nudDecimal.Enabled = true; this.nudWidth.Enabled = true; this.nudWidth.Value = 10; }
            if (cmbType.Text == "Text") { this.nudDecimal.Enabled = false; this.nudWidth.Enabled = false; this.nudWidth.Value = 16; }
            if (cmbType.Text == "Datetime") { this.nudDecimal.Enabled = false; this.nudWidth.Enabled = false; this.nudWidth.Value = 8; }
            if (cmbType.Text == "Bit") { this.nudDecimal.Enabled = false; this.nudWidth.Enabled = false; this.nudWidth.Value = 1; }
        }

        private void txtFldNm_Leave(object sender, EventArgs e)
        {
        
        }

        private void mth_rbValid()
        {
            vWhn_Con = vTblMain.Rows[0]["whn_con"].ToString().Trim();
            vMandatory = vTblMain.Rows[0]["Mandatory"].ToString().Trim();
            vVal_Err = vTblMain.Rows[0]["Val_Err"].ToString().Trim();
            vVal_Con = vTblMain.Rows[0]["Val_Con"].ToString().Trim();
            vInter_Use = vTblMain.Rows[0]["Inter_Use"].ToString().Trim();
            vDefa_Val = vTblMain.Rows[0]["Defa_Val"].ToString().Trim();
        }

        private void rbMast_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbMast.Checked) { this.label2.Text = "Master Type";  this.label10.Text = "Valid in Master"; }
            else  { this.label2.Text = "Transaction Type";   this.label10.Text = "Valid in Transaction";  }

            if (this.rbMast.Checked && this.rbHeader.Checked) { this.chkDispInPickup.Enabled = false; this.chkDispInHeader.Enabled = false; }
            if (this.rbTrn.Checked && this.rbHeader.Checked) { this.chkDispInPickup.Enabled = true; this.chkDispInHeader.Enabled = true; this.chkDispInHeader.Text = "Display in Header"; }
            if (this.rbMast.Checked && this.rbDetail.Checked) { this.chkDispInPickup.Enabled = false; this.chkDispInHeader.Enabled = true; this.chkDispInHeader.Text = "Display in Grid"; }
            if (this.rbTrn.Checked && this.rbDetail.Checked) { this.chkDispInPickup.Enabled = true; this.chkDispInHeader.Enabled = true; this.chkDispInHeader.Text = "Display in Grid"; }
        }

        private void rbHeader_CheckedChanged(object sender, EventArgs e)
        {
            if (this.pAddMode == true || this.pEditMode == true)
            {
                if (this.rbMast.Checked && this.rbHeader.Checked) { this.chkDispInPickup.Enabled = false; this.chkDispInHeader.Enabled = false; }
                if (this.rbTrn.Checked && this.rbHeader.Checked) { this.chkDispInPickup.Enabled = true; this.chkDispInHeader.Enabled = true; this.chkDispInHeader.Text = "Display in Header"; }
                if (this.txtTrnType.Text != "") { if (rbTrn.Checked) { this.txtTblNm.Text = vBode_Nm + (rbHeader.Checked ? "Main" : "Item"); } }
                vOrgTblNm = this.txtTblNm.Text;
            }
        }

        private void rbDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (this.pAddMode == true || this.pEditMode == true)
            {
                if (this.rbMast.Checked && this.rbDetail.Checked) { this.chkDispInPickup.Enabled = false; this.chkDispInHeader.Enabled = true; this.chkDispInHeader.Text = "Display in Grid"; }
                if (this.rbTrn.Checked && this.rbDetail.Checked) { this.chkDispInPickup.Enabled = true; this.chkDispInHeader.Enabled = true; this.chkDispInHeader.Text = "Display in Grid"; }
                if (this.txtTrnType.Text != "") { if (rbTrn.Checked) { this.txtTblNm.Text = vBode_Nm + (rbHeader.Checked ? "Main" : "Item"); } }
            }
        }

        private void txtHeadNm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void btnValidIn_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet DS1;
                if (rbTrn.Checked)
                {
                    SqlStr = "select code_nm,entry_ty from lcode union select name as code_nm,code as entry_ty from MastCode where DeptApp=1 order by code_nm";
                }
                else
                {
                    SqlStr = "select name as code_nm,code as entry_ty from MastCode order by code_nm";
                }
                DS1 = oDataAccess.GetDataSet(SqlStr, null, 25);
                frmValidInTran frmvalid = new frmValidInTran();

                frmvalid.ds = DS1;
                frmvalid.getds(DS1, txtValidIn.Text.ToString().Trim());
                frmvalid.Icon = this.pFrmIcon;
                frmvalid.ShowDialog();
                txtValidIn.Text = frmvalid.validdata;
                vTblMain.Rows[0]["validity"] = frmvalid.validdata;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Source : " + ex.Source.ToString() + "\nMessage : " + ex.Message.ToString() + "\nTargetSite : " + ex.TargetSite.ToString());
            }
        }

        private void btnpopupQuery_Click(object sender, EventArgs e)
        {
            
            if (this.txtPopupQuery.Text.ToUpper().IndexOf("SELECT") > -1 && this.txtPopupQuery.Text.ToUpper().IndexOf("FROM") > -1)
            {
                frmQuery oFrmQury = new frmQuery();
                oFrmQury.pQuery = this.txtPopupQuery.Text;
                oFrmQury.pDataAccess = oDataAccess;
                oFrmQury.ShowDialog();
                this.txtPopupQuery.Text = oFrmQury.pQuery;
            }
        }

        private void chkDefault_Click(object sender, EventArgs e)
        {
            udTextEdit.cudTextEdit oudTextEdit = new udTextEdit.cudTextEdit();
            oudTextEdit.pAddMode = this.pAddMode;
            oudTextEdit.pEditMode = this.pEditMode;
            oudTextEdit.pICon = this.Icon;
            oudTextEdit.pTextVal = vDefa_Val;
            oudTextEdit.pFrmCaption = "Default Value ";
            oudTextEdit.mthCallTextEdit();
            vDefa_Val = oudTextEdit.pTextVal;
            vDefa_Val = vDefa_Val.Replace(@"'", @"''");
            this.chkDefault.Checked = (vDefa_Val == "" ? false : true);
        }
        
        private void mthEnableDisableFormControls()
        {
            bool vEnabled = true;
            this.rbDetail.Enabled = false;
            this.rbHeader.Enabled = false;
            this.rbTrn.Enabled = false;
            this.rbMast.Enabled = false;
            this.btnTrnType.Enabled = false;
            this.txtFldNm.Enabled = false;
            this.txtTrnType.Enabled = false;
                    
            if (this.pAddMode)
            {
                this.rbDetail.Enabled = true;
                this.rbHeader.Enabled = true;
                this.rbTrn.Enabled = true;
                this.rbMast.Enabled = true;
                this.btnTrnType.Enabled = true;
                this.btnHeadNm.Enabled = false;
                this.txtFldNm.Enabled = true;
                this.txtHeadNm.Focus();
            }
            else if (this.pEditMode)
            {
                this.btnHeadNm.Enabled = false;
                this.txtHeadNm.Focus();
                this.txtTrnType.Enabled = false;
            }
            else
            {
                this.btnHeadNm.Enabled = true;
                vEnabled = false;

            }
            this.txtFldNm.Enabled = vEnabled;
            this.chkDispInMIS.Enabled = vEnabled;  
            this.cmbType.Enabled = vEnabled;
            this.nudWidth.Enabled = vEnabled;
            this.nudDecimal.Enabled = vEnabled;
            this.txtPopupQuery.Enabled = vEnabled;
            this.txtValidIn.Enabled = vEnabled;
            this.txtRemark.Enabled = vEnabled;
            
            this.txtHeadNm.Enabled = vEnabled;
            this.txtFormDesc.Enabled = vEnabled;
            this.txtFormHeading.Enabled = vEnabled;
            this.nudOrder.Enabled = vEnabled;
            this.nudSubOrder.Enabled = vEnabled;
            this.nudFormOrder.Enabled = vEnabled;

            this.btnpopupQuery.Enabled = vEnabled;
            this.btnValidIn.Enabled = vEnabled;
            this.btnTrnType.Enabled = vEnabled;
            this.chkDispInPickup.Enabled = vEnabled;
            this.chkMandatory.Enabled = vEnabled;
            this.chkDispInHeader.Enabled = vEnabled;

            this.chkWhen.Enabled = vEnabled;
            this.chkValid.Enabled = vEnabled;
            this.chkDefault.Enabled = vEnabled;
            this.chkInternal.Enabled = vEnabled;
            this.chkError.Enabled = vEnabled;
            this.ChkNewLine.Enabled = vEnabled;

            //if (this.pEditMode == true && this.chkDeAct.Checked == true) { this.dtpDeActFrom.Enabled = true; }
        }
        public void mthControlSet()
        {
            this.cmbType.Items.Add("Varchar");
            this.cmbType.Items.Add("Decimal");
            this.cmbType.Items.Add("Bit");
            this.cmbType.Items.Add("Datetime");
            this.cmbType.Items.Add("Text");

            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnTrnType.Image = Image.FromFile(fName);
                this.btnValidIn.Image = Image.FromFile(fName);
                this.btnHeadNm.Image = Image.FromFile(fName);
                this.btnpopupQuery.Image = Image.FromFile(fName);
            }

        }
        private void mthChkNavigationButton()
        {
            DataSet dsTemp = new DataSet();
            this.btnForward.Enabled = false;
            this.btnLast.Enabled = false;
            this.btnFirst.Enabled = false;
            this.btnBack.Enabled = false;
            btnEdit.Enabled = false;
            Boolean vBtnAdd, vBtnEdit, vBtnDelete, vBtnPrint;
            if (ServiceType.ToUpper() != "VIEWER VERSION")
            {
                if (vTblMain.Rows.Count == 0)
                {
                    if (this.pAddMode == false && this.pEditMode == false)
                    {
                        this.btnCancel.Enabled = false;
                        this.btnSave.Enabled = false;

                        vBtnAdd = true;
                        vBtnDelete = false;
                        vBtnEdit = false;
                        vBtnPrint = false;
                        //this.btnFirst.Enabled = true;   //Added by Divyang
                        //this.btnBack.Enabled = true;    //Added by Divyang

                        this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
                    }
                    return;
                }
            }
            if (this.pAddMode == false && this.pEditMode == false)
            {
                if (vTblMain.Rows.Count > 0)
                {
                    vMainFldVal = vTblMain.Rows[0][vMainField].ToString().Trim();
                    vMainFldVal1 = vTblMain.Rows[0][vMainField1].ToString().Trim();
                    SqlStr = "select " + vMainField + "+" + vMainField1 + " from " + vTblMainNm + " m inner Join lCode l on (m.e_Code=l.Entry_Ty) Where " + vMainField + "+m." + vMainField1 + ">'" + vMainFldVal + vMainFldVal1 + "'";

                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, 20);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnForward.Enabled = true;
                        this.btnLast.Enabled = true;
                    }
                    SqlStr = "select " + vMainField + "+" + vMainField1 + " from " + vTblMainNm + " m inner Join lCode l on (m.e_Code=l.Entry_Ty)  Where " + vMainField + "+" + vMainField1 + "<'" + vMainFldVal + vMainFldVal1 + "'";

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
                    if (vTblMain.Rows.Count > 0)
                    {
                        vBtnDelete = true;
                        vBtnEdit = true;
                        vBtnPrint = true;
                        this.btnValidIn.Enabled = false;


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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnLogout, e);
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void mcheckCallingApplication()/*Added Rup 07/03/2011*/
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
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();

            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udAddInfoMaster.exe";
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
    }
}
