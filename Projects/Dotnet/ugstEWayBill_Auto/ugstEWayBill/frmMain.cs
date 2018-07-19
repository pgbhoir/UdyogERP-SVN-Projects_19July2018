using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using uBaseForm;
using System.Collections;
using udSelectPop;
using DataAccess_Net;
using System.Diagnostics;

using System.Globalization;
using System.Threading;
using Udyog.Library.Common;
using Udyog.QRCode.Codec;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Configuration;


namespace ugstEWayBill
{
    public partial class frmMain : uBaseForm.FrmBaseForm
    {
        DataSet vDsCommon;
        DataAccess_Net.clsDataAccess oDataAccess;
        DataTable tblGSTIN;
        string vSqlStr = "";
        DataTable jsonDataTable = new DataTable();

        int selectedReccnt = 0;
        String cAppPId, cAppName;
        short vTimeOut = 50;

        string sqlconnstr = string.Empty;
        string authourl = string.Empty;
        string ewayurl = string.Empty;
        string ewaycancelurl = string.Empty;
        string authorization = string.Empty;
        string _nicuid = string.Empty;
        string _nicpwd = string.Empty;

        //string _gspid = "F066EA96863D4A68BB2944B2328DEB82";
        //string _gspPwd = "8C9D3661G75D3G46ADG9331G1B70E90BAA7D";

        string _gspid = "";         // Changed by Sachin N. S. on 10/04/2018 
        string _gspPwd = "";        // Changed by Sachin N. S. on 10/04/2018 

        string autho_token = string.Empty;
        DataTable GetData = new DataTable();
        
        public frmMain(string[] args)
        {
            InitializeComponent();
            DataAccess_Net.clsDataAccess oDataAccess;

            //this.pDisableCloseBtn = true;  /* close disable  */

            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "e Way Bill Generation";
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

            sqlconnstr = "Data Source=" + this.pServerName + ";Initial Catalog=" + this.pComDbnm + ";Uid=" + this.pUserId + ";Pwd=" + this.pPassword;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            this.pAppPath = Application.ExecutablePath;
            this.pAppPath = Path.GetDirectoryName(this.pAppPath);
            this.mthControlSet();

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.mthDsCommon();
            this.SetFormColor();
            this.txtTrnName.Text = "Sales";
            this.mInsertProcessIdRecord();

            string prodcode = VU_UDFS.GetDecProductCode(vDsCommon.Tables[0].Rows[0]["p1"].ToString().Trim());
            string prodcode1 = VU_UDFS.GetDecProductCode(vDsCommon.Tables[0].Rows[0]["p2"].ToString().Trim());
            prodcode = prodcode + "," + prodcode1;
            
            if (!prodcode.ToLower().Contains("udewaygen"))
            {
                this.btnGenEWB.Visible = false;
                this.btnCancel.Visible = false;
            }
            this.ReadUrlSettings();
            this.ReadClientLicense();       // Added by Sachin N. S. on 06/04/2018 
            this.Icon = this.pFrmIcon;
        }

        private void ReadUrlSettings()
        {
            authourl = ConfigurationManager.AppSettings["EWBAUTH"].ToString();
            ewayurl = ConfigurationManager.AppSettings["EWBGEN"].ToString();
            ewaycancelurl = ConfigurationManager.AppSettings["EWBCAN"].ToString();
            authorization = ConfigurationManager.AppSettings["EWBAUTHTYPE"].ToString();

            _nicuid = vDsCommon.Tables[0].Rows[0]["nicuid"].ToString().Trim();
            _nicpwd = VU_UDFS.dec(VU_UDFS.ondecrypt(vDsCommon.Tables[0].Rows[0]["nicpwd"].ToString().Trim()));

            //_gstinuid = "05AAACG2115R1ZN";
            //_gstinpwd = "abc123@@";
        }

        // Added by Sachin N. S. on 06/04/2018  -- Start
        private void ReadClientLicense()
        {
            ugstnHelper.clsUdyogGSTApiLicChker _LicReader = new ugstnHelper.clsUdyogGSTApiLicChker();

            string[] _licread = _LicReader.ChkClientLicense(this.pAppPath, vDsCommon.Tables["Company"].Rows[0]["Co_Name"].ToString().Trim(), "udewaygen");
            //string[] _licread = _LicReader.ChkClientLicense(this.pAppPath, "Udyog Software License Testing company", "udewaygen");       // To be Removed
            //MessageBox.Show("="+vDsCommon.Tables["Company"].Rows[0]["Co_Name"].ToString()+"=");
            //MessageBox.Show(_licread[0].ToString());
            //MessageBox.Show(_licread[1].ToString());
            _gspid = _licread[0].ToString();
            _gspPwd = _licread[1].ToString();
        }
        // Added by Sachin N. S. on 06/04/2018  -- End

        private void mthView()
        {
            dgvGSTIN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));

            vSqlStr = "Execute [USP_Ent_eWayBill_Bulk] " + (this.txtGSTIN.Text.Trim() != "" ? "'" + this.txtGSTIN.Text.Trim() + "'" : "''") + (this.txtPartyNm.Text.Trim() != "" ? ",'" + this.txtPartyNm.Text.Trim() + "'" : ",''");
            vSqlStr = vSqlStr + "," + (this.txtTrnName.Text.Trim() != "" ? "'" + this.txtTrnName.Text.Trim() + "'" : "''");
            if (this.txtFInvNo.Text.Trim().Length > 0 && this.txtTInvNo.Text.Trim().Length > 0)
            {
                vSqlStr = vSqlStr + ",'" + this.txtFInvNo.Text.Trim() + "','" + this.txtTInvNo.Text.Trim() + "'";
            }
            else
            {
                vSqlStr = vSqlStr + ",'',''";
            }
            vSqlStr = vSqlStr + ",'" + Convert.ToDateTime(vDsCommon.Tables[0].Rows[0]["sta_dt"]).ToString("MM/dd/yyyy") + "','" + Convert.ToDateTime(vDsCommon.Tables[0].Rows[0]["end_dt"]).ToString("MM/dd/yyyy") + "'";
            //vSqlStr = vSqlStr + ",'04/01/2017','03/31/2018'";       // /*To be Remove*/d
            //tblGSTIN = oDataAccess.GetDataTable(vSqlStr, null, 25);
            SqlConnection conn = new SqlConnection(sqlconnstr);
            SqlDataAdapter lda = new SqlDataAdapter(vSqlStr, conn);
            DataTable ldt = new DataTable();
            lda.Fill(ldt);
            tblGSTIN = ldt;

            this.dgvGSTIN.Columns.Clear();
            this.dgvGSTIN.DataSource = this.tblGSTIN;
            this.dgvGSTIN.Columns.Clear();

            System.Windows.Forms.DataGridViewCheckBoxColumn colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();


            colSelect.HeaderText = "Select";
            colSelect.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSelect.Width = 40;
            colSelect.Name = "colSelect";
            colSelect.ReadOnly = false;
            this.dgvGSTIN.Columns.Add(colSelect);


            System.Windows.Forms.DataGridViewTextBoxColumn colEWAYN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEWAYN.HeaderText = "eWAY Bill No.";
            colEWAYN.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colEWAYN.Width = 100;
            colEWAYN.Name = "colEWAYN";
            colEWAYN.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colEWAYN);


            System.Windows.Forms.DataGridViewTextBoxColumn colEWAYDt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEWAYDt.HeaderText = "eWAY Bill Dt.";
            colEWAYDt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colEWAYDt.Width = 100;
            colEWAYDt.Name = "colEWAYDt";
            colEWAYDt.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colEWAYDt);


            System.Windows.Forms.DataGridViewTextBoxColumn colSuppTyp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSuppTyp.HeaderText = "Supply Type";
            colSuppTyp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSuppTyp.Width = 90;
            colSuppTyp.Name = "colSuppTyp";
            colSuppTyp.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colSuppTyp);


            System.Windows.Forms.DataGridViewTextBoxColumn colSubTyp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSubTyp.HeaderText = "EWB Sub Supply Type";
            colSubTyp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSubTyp.Width = 100;
            colSubTyp.Name = "colSubTyp";
            colSubTyp.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colSubTyp);


            System.Windows.Forms.DataGridViewTextBoxColumn colDocTyp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDocTyp.HeaderText = "Doc. Type";
            colDocTyp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colDocTyp.Width = 100;
            colDocTyp.Name = "colDocTyp";
            colDocTyp.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colDocTyp);


            System.Windows.Forms.DataGridViewTextBoxColumn colDocNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDocNo.HeaderText = "Doc. No.";
            colDocNo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colDocNo.Width = 120;
            colDocNo.Name = "colDocNo";
            colDocNo.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colDocNo);


            System.Windows.Forms.DataGridViewTextBoxColumn colDocDt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDocDt.HeaderText = "Doc. Dt.";
            colDocDt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colDocDt.Width = 100;
            colDocDt.Name = "colDocDt";
            colDocDt.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colDocDt);


            System.Windows.Forms.DataGridViewTextBoxColumn colGSTIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colGSTIN.HeaderText = "GSTIN";
            colGSTIN.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colGSTIN.Width = 120;
            colGSTIN.Name = "colGSTIN";
            colGSTIN.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colGSTIN);


            System.Windows.Forms.DataGridViewTextBoxColumn colLgNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLgNm.HeaderText = "Legal Name of Business";
            colLgNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colLgNm.Width = 145;
            colLgNm.Name = "colLgNm";
            colLgNm.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colLgNm);


            System.Windows.Forms.DataGridViewTextBoxColumn colConCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colConCity.HeaderText = "Place of Consignor";
            colConCity.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colConCity.Width = 100;
            colConCity.Name = "colConCity";
            colConCity.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colConCity);


            System.Windows.Forms.DataGridViewTextBoxColumn colConState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colConState.HeaderText = "State of Consignor";
            colConState.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colConState.Width = 145;
            colConState.Name = "colConState";
            colConState.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colConState);


            System.Windows.Forms.DataGridViewTextBoxColumn colTranDocNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTranDocNo.HeaderText = "Trans. Doc.(LR) No.";
            colTranDocNo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colTranDocNo.Width = 140;
            colTranDocNo.Name = "colTranDocNo";
            colTranDocNo.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colTranDocNo);


            System.Windows.Forms.DataGridViewTextBoxColumn colTranDocDt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTranDocDt.HeaderText = "Trans Doc.(LR) Dt.";
            colTranDocDt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colTranDocDt.Width = 140;
            colTranDocDt.Name = "colTranDocDt";
            colTranDocDt.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colTranDocDt);


            System.Windows.Forms.DataGridViewTextBoxColumn colTranMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTranMode.HeaderText = "Trans. Mode";
            colTranMode.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colTranMode.Width = 140;
            colTranMode.Name = "colTranMode";
            colTranMode.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colTranMode);


            System.Windows.Forms.DataGridViewTextBoxColumn colAproxDis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colAproxDis.HeaderText = "Approx Distance";
            colAproxDis.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colAproxDis.Width = 145;
            colAproxDis.Name = "colAproxDis";
            colAproxDis.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colAproxDis);


            dgvGSTIN.Columns["colSelect"].DataPropertyName = "sel";
            dgvGSTIN.Columns["colSuppTyp"].DataPropertyName = "SuppTyp";
            dgvGSTIN.Columns["colSubTyp"].DataPropertyName = "SubTyp";
            dgvGSTIN.Columns["colDocTyp"].DataPropertyName = "DocTyp";
            dgvGSTIN.Columns["colDocNo"].DataPropertyName = "DocNo";
            dgvGSTIN.Columns["colDocDt"].DataPropertyName = "DocDt";
            dgvGSTIN.Columns["colGSTIN"].DataPropertyName = "GSTIN";
            dgvGSTIN.Columns["colLgNm"].DataPropertyName = "LgNm";
            dgvGSTIN.Columns["colConCity"].DataPropertyName = "ConCity";
            dgvGSTIN.Columns["colConState"].DataPropertyName = "ConState";
            dgvGSTIN.Columns["colTranDocNo"].DataPropertyName = "TranDocNo";
            dgvGSTIN.Columns["colTranDocDt"].DataPropertyName = "TranDocDt";
            dgvGSTIN.Columns["colTranMode"].DataPropertyName = "TranMode";
            dgvGSTIN.Columns["colAproxDis"].DataPropertyName = "AproxDis";
            dgvGSTIN.Columns["colEWAYN"].DataPropertyName = "eWAYN";
            dgvGSTIN.Columns["colEWAYDt"].DataPropertyName = "eWAYDt";


        }

        private void mthControlSet()
        {
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnPartyNm.Image = Image.FromFile(fName);
                this.btnGSTIN.Image = Image.FromFile(fName);
                this.btnTrnName.Image = Image.FromFile(fName);
                this.btnFInvNo.Image = Image.FromFile(fName);
                this.btnTInvNo.Image = Image.FromFile(fName);
            }
            fName = this.pAppPath + @"\bmp\loc-on.gif";
            if (File.Exists(fName) == true)
            {
                //this.btnGenEWB.Image = Image.FromFile(fName);
            }
            fName = this.pAppPath + @"\bmp\report_rtf.gif";
            if (File.Exists(fName) == true)
            {
                this.btnJsonFile.Image = Image.FromFile(fName);
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
            vSqlStr = "Select *,p1=rtrim(convert(varchar(max),Passroute)),p2=rtrim(convert(varchar(max),Passroute1)) From vudyog..Co_Mast where CompId=" + this.pCompId.ToString();
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

            DataTable tblCoset = new DataTable();
            tblCoset.TableName = "CompanySetting";
            vSqlStr = "Select Top 1 * From co_set";
            tblCoAdditional = oDataAccess.GetDataTable(vSqlStr, null, 25);
            vDsCommon.Tables.Add(tblCoset);
            vDsCommon.Tables[2].TableName = "CompanySetting";
        }

        private void btnGSTIN_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;

            vSqlStr = "Execute [USP_Ent_eWayBill_Filter_PartyGstin] '" + this.txtTrnName.Text.ToString().Trim() + "','" + this.txtPartyNm.Text.ToString().Trim() + "'";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 20);

            DataView dvw = tblPop.DefaultView;
            dvw.Sort = "gstin";

            VForText = "Select GSTIN";
            vSearchCol = "GSTIN";

            vDisplayColumnList = "GSTIN:GSTIN";
            vReturnCol = "GSTIN";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.Icon = this.pFrmIcon;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtGSTIN.Text = oSelectPop.pReturnArray[0];
                selectedReccnt = 0;
                btnSelctAll.Text = "Select All";
                this.mthView();
            }
        }

        private void btnPartyNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;

            vSqlStr = "Execute [USP_Ent_eWayBill_Filter_PartyName] '" + this.txtTrnName.Text.ToString().Trim() + "','" + this.txtGSTIN.Text.ToString().Trim() + "'";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);

            DataView dvw = tblPop.DefaultView;
            dvw.Sort = "ac_name";

            VForText = "Select Party Name";
            vSearchCol = "AC_Name+Mailname";
            vDisplayColumnList = "AC_Name:Party Name,MailName:Mailing Name";
            vReturnCol = "AC_Name";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.Icon = this.pFrmIcon;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtPartyNm.Text = oSelectPop.pReturnArray[0];
                selectedReccnt = 0;
                btnSelctAll.Text = "Select All";
                this.mthView();
            }


        }
        private void Update_apilog(string logType, DateTime loggedTime, string loggedUser, string logText, string requestID)
        {
            vSqlStr = "Insert Into Eway_Log(logType,loggedTime,loggedUser,respStatus,requestId) Values (@logType,@loggedTime,@loggedUser,@logText,@requestID)";
            //oDataAccess.ExecuteSQLStatement(vSqlStr, null, 0, true);
            SqlConnection conn = new SqlConnection(sqlconnstr);
            SqlCommand cmd = new SqlCommand(vSqlStr, conn);

            cmd.Parameters.Add("@logType", SqlDbType.VarChar).Value = logType;
            cmd.Parameters.Add("@loggedTime", SqlDbType.DateTime).Value = loggedTime;
            cmd.Parameters.Add("@loggedUser", SqlDbType.VarChar).Value = loggedUser;
            cmd.Parameters.Add("@logText", SqlDbType.Text).Value = logText;
            cmd.Parameters.Add("@requestID", SqlDbType.VarChar).Value = requestID;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        private void ShowProcess(string processMsg)
        {
            this.lblStatus.Text = processMsg;
            this.statusStrip1.Refresh();
        }
        private void btnJson_Click(object sender, EventArgs e)
        {

        }
        private void Generate_EWB_Json(string vEntry_Ty, string vInv_No,string vInvDate, ref StringBuilder JsonString)
        {
            DataTable tblMain;
            vSqlStr = "Execute [USP_Ent_eWayBill_JsonDet_Main_API] '" + vEntry_Ty + "','" + vInv_No.Trim() + "','"+ vInvDate.Trim()+"'," + this.pCompId.ToString().Trim();
            //tblMain = oDataAccess.GetDataTable(vSqlStr, null, 25);
            SqlConnection conn = new SqlConnection(sqlconnstr);
            SqlDataAdapter lda = new SqlDataAdapter(vSqlStr, conn);
            DataTable ldt = new DataTable();
            lda.Fill(ldt);
            tblMain = ldt;

            string vIgnoreColumn = "<Sel><State_Name><GrpNm><supplyType1><subsupplyType1><docType1><transMode1><userGstin><genMode>";
            if (tblMain.Rows.Count > 0)
            {
                String vVal = "";
                for (int j = 0; j < tblMain.Columns.Count; j++)
                {
                    if (vIgnoreColumn.IndexOf("<" + tblMain.Columns[j].ColumnName.ToString() + ">") == -1)
                    {
                        if (j < tblMain.Columns.Count - 1)
                        {
                            vVal = tblMain.Rows[0][j].ToString().Trim();
                            if (tblMain.Columns[j].ColumnName.ToString() == "docDate" || tblMain.Columns[j].ColumnName.ToString() == "transDocDate")
                            {
                                vVal = vVal.Replace("-", "/");
                            }
                            if (tblMain.Columns[j].DataType.ToString() == "System.String")
                            {
                                JsonString.Append("\t\"" + tblMain.Columns[j].ColumnName.ToString() + "\":" + "\"" + vVal + "\","); ;
                            }
                            else
                            {
                                JsonString.Append("\t\"" + tblMain.Columns[j].ColumnName.ToString() + "\":" + vVal + ","); ;
                            }
                            JsonString.Append("\n");
                        }
                        else if (j == tblMain.Columns.Count - 1)
                        {
                            vVal = tblMain.Rows[0][j].ToString().Trim();
                            if (tblMain.Columns[j].DataType.ToString() == "System.String")
                            {
                                JsonString.Append("\t\"" + tblMain.Columns[j].ColumnName.ToString() + "\":" + "\"" + vVal + "\","); ;
                            }
                            else
                            {
                                JsonString.Append("\t\"" + tblMain.Columns[j].ColumnName.ToString() + "\":" + vVal+ ",");
                            }
                            JsonString.Append("\n");
                        }
                    }
                }
            }

            vIgnoreColumn = "<itemNo>";
            DataTable tblItem;
            vSqlStr = "Execute [USP_Ent_eWayBill_JsonDet_Item_API] '" + vEntry_Ty + "','" + vInv_No.Trim() + "','"+ vInvDate+"'";
            //tblItem = oDataAccess.GetDataTable(vSqlStr, null, 25);
            lda = new SqlDataAdapter(vSqlStr, conn);
            ldt = new DataTable();
            lda.Fill(ldt);
            tblItem = ldt;

            JsonString.Append("\t\"itemList\":");

            if (tblItem.Rows.Count > 0)
            {
                JsonString.Append("[\n");
                for (int i = 0; i < tblItem.Rows.Count; i++)
                {

                    JsonString.Append("\t\t{\n");
                    for (int j = 0; j < tblItem.Columns.Count; j++)
                    {
                        if (vIgnoreColumn.IndexOf("<" + tblItem.Columns[j].ColumnName.ToString() + ">") == -1)
                        {

                            if (j < tblItem.Columns.Count - 1)
                            {
                                if (tblItem.Columns[j].DataType.ToString() == "System.String" && tblItem.Columns[j].ColumnName.ToString().ToLower() == "hsncode")
                                {
                                    JsonString.Append("\t\t\t\"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + "" + tblItem.Rows[i][j].ToString().Trim() + ",");
                                }
                                else if (tblItem.Columns[j].DataType.ToString() == "System.String")
                                {
                                    JsonString.Append("\t\t\t\"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + "\"" + tblItem.Rows[i][j].ToString().Trim() + "\",");
                                }
                                else
                                {
                                    JsonString.Append("\t\t\t\"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + tblItem.Rows[i][j].ToString().Trim() + ",");
                                }
                                JsonString.Append("\n");
                            }
                            else if (j == tblItem.Columns.Count - 1)
                            {
                                if (tblItem.Columns[j].DataType.ToString() == "System.String" && tblItem.Columns[j].ColumnName.ToString().ToLower() == "hsncode")
                                {
                                    JsonString.Append("\t\t\t\"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + "" + tblItem.Rows[i][j].ToString().Trim() + ",");
                                }
                                else if (tblItem.Columns[j].DataType.ToString() == "System.String")
                                {
                                    JsonString.Append("\t\t\t\"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + "\"" + tblItem.Rows[i][j].ToString().Trim() + "\"");
                                }
                                else
                                {
                                    JsonString.Append("\t\t\t\"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + "" + tblItem.Rows[i][j].ToString().Trim());
                                }

                                JsonString.Append("\n");
                            }
                        }
                    }
                    if (i < tblItem.Rows.Count - 1)
                    {
                        JsonString.Append("\t\t},");
                        JsonString.Append("\n");
                    }
                    else
                    {
                        JsonString.Append("\t\t}");
                        JsonString.Append("\n");
                    }
                }
                JsonString.Append("\t]");

            }

        }

        private void mthGenJson(string vEntry_Ty, string vInv_No,string vInvDate, ref StringBuilder JsonString)
        {
            DataTable tblMain;
            vSqlStr = "Execute [USP_Ent_eWayBill_JsonDet_Main] '" + vEntry_Ty + "','" + vInv_No.Trim() + "','"+ vInvDate.Trim()+"'," + this.pCompId.ToString().Trim();

            //tblMain = oDataAccess.GetDataTable(vSqlStr, null, 25);
            SqlConnection conn = new SqlConnection(sqlconnstr);
            SqlDataAdapter lda = new SqlDataAdapter(vSqlStr, conn);
            DataTable ldt = new DataTable();
            lda.Fill(ldt);
            tblMain = ldt;

            string vIgnoreColumn = "<Sel><State_Name><GrpNm><supplyType1><subsupplyType1><docType1><transMode1>";
            if (tblMain.Rows.Count > 0)
            {
                String vVal = "";
                for (int j = 0; j < tblMain.Columns.Count; j++)
                {
                    if (vIgnoreColumn.IndexOf("<" + tblMain.Columns[j].ColumnName.ToString() + ">") == -1)
                    {
                        if (j < tblMain.Columns.Count - 1)
                        {
                            vVal = tblMain.Rows[0][j].ToString().Trim();
                            if (tblMain.Columns[j].ColumnName.ToString() == "docDate" || tblMain.Columns[j].ColumnName.ToString() == "transDocDate")
                            {
                                vVal = vVal.Replace("-", "/");
                            }
                            if (tblMain.Columns[j].DataType.ToString() == "System.String")
                            {
                                JsonString.Append("          \"" + tblMain.Columns[j].ColumnName.ToString() + "\":" + "\"" + vVal + "\",");
                            }
                            else
                            {
                                JsonString.Append("          \"" + tblMain.Columns[j].ColumnName.ToString() + "\":" + vVal + ",");
                            }
                            JsonString.Append("\n");
                        }
                        else if (j == tblMain.Columns.Count - 1)
                        {
                            vVal = tblMain.Rows[0][j].ToString().Trim();
                            if (tblMain.Columns[j].DataType.ToString() == "System.String")
                            {
                                JsonString.Append("          \"" + tblMain.Columns[j].ColumnName.ToString() + "\":" + "\"" + vVal + "\",");
                            }
                            else
                            {
                                JsonString.Append("          \"" + tblMain.Columns[j].ColumnName.ToString() + "\":" + vVal + ",");
                            }
                            JsonString.Append("\n");
                        }
                    }
                }

            }
            DataTable tblItem;
            vSqlStr = "Execute [USP_Ent_eWayBill_JsonDet_Item] '" + vEntry_Ty + "','" + vInv_No.Trim() + "','"+ vInvDate.Trim()+"'";
            //tblItem = oDataAccess.GetDataTable(vSqlStr, null, 25);
            lda = new SqlDataAdapter(vSqlStr, conn);
            ldt = new DataTable();
            lda.Fill(ldt);
            tblItem = ldt;

            JsonString.Append("               \"itemList\":");
            

            if (tblItem.Rows.Count > 0)
            {
                JsonString.Append("[\n");
                for (int i = 0; i < tblItem.Rows.Count; i++)
                {
                    JsonString.Append("               {\n");
                    for (int j = 0; j < tblItem.Columns.Count; j++)
                    {

                        if (j < tblItem.Columns.Count - 1)
                        {
                            if (tblItem.Columns[j].DataType.ToString() == "System.String" && tblItem.Columns[j].ColumnName.ToString().ToLower() == "hsncode")
                            {
                                JsonString.Append("                 \"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + "" + tblItem.Rows[i][j].ToString().Trim() + ",");
                            }
                            else if (tblItem.Columns[j].DataType.ToString() == "System.String")
                            {
                                JsonString.Append("                 \"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + "\"" + tblItem.Rows[i][j].ToString().Trim() + "\",");
                            }
                            else
                            {
                                JsonString.Append("                 \"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + tblItem.Rows[i][j].ToString().Trim() + ",");
                            }
                            JsonString.Append("\n");
                        }
                        else if (j == tblItem.Columns.Count - 1)
                        {
                            if (tblItem.Columns[j].DataType.ToString() == "System.String" && tblItem.Columns[j].ColumnName.ToString().ToLower() == "hsncode")
                            {
                                JsonString.Append("                 \"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + "" + tblItem.Rows[i][j].ToString().Trim() + ",");
                            }
                            else if (tblItem.Columns[j].DataType.ToString() == "System.String")
                            {
                                JsonString.Append("                 \"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + "\"" + tblItem.Rows[i][j].ToString().Trim() + "\"");
                            }
                            else
                            {
                                JsonString.Append("                 \"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + "" + tblItem.Rows[i][j].ToString().Trim());
                            }

                            JsonString.Append("\n");
                        }
                    }
                    if (i < tblItem.Rows.Count - 1)
                    {
                        JsonString.Append("               },");
                        JsonString.Append("\n");
                    }
                    else
                    {
                        JsonString.Append("               }");
                        JsonString.Append("\n");
                    }

                }
                JsonString.Append("               ]");

            }
        }

        public string DataTableToJsonObj(ref StringBuilder JsonString, DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString().Trim() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString().Trim() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                JsonString.Append("}");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }

        public string DataTableToJsonObj_tbl(ref StringBuilder JsonString, DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            //StringBuilder JsonString = new StringBuilder();
            //JsonString.Append("{\"billLists\": ");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString().Trim() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString().Trim() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                JsonString.Append("}");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }

        public string DataTableToJSON(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            return "";
        }

        private void btnJsonFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vFolderBrowserDialog1 = new FolderBrowserDialog();
            vFolderBrowserDialog1.ShowDialog();
            txtJsonFile.Text = "";
            txtJsonFile.Text = vFolderBrowserDialog1.SelectedPath + txtJsonFile.Text;
        }

        private void btnTrnName_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            //vSqlStr = "SELECT CODE_NM='SALES' UNION SELECT CODE_NM='PURCHASE' UNION SELECT CODE_NM='LABOUR JOB ISSUE[IV]' UNION SELECT CODE_NM='LABOUR JOB ISSUE[V]' UNION SELECT CODE_NM='LABOUR JOB RECEIPT[IV]' UNION SELECT CODE_NM='LABOUR JOB RECEIPT[V]' UNION SELECT CODE_NM='CREDIT NOTE' UNION SELECT CODE_NM='SALES RETURN' UNION SELECT CODE_NM='DELIVERY CHALLAN'";
            vSqlStr = "SELECT CODE_NM='SALES' UNION SELECT CODE_NM='PURCHASE' UNION SELECT CODE_NM='LABOUR JOB ISSUE[IV]' UNION SELECT CODE_NM='LABOUR JOB ISSUE[V]' UNION SELECT CODE_NM='LABOUR JOB RECEIPT[IV]' UNION SELECT CODE_NM='LABOUR JOB RECEIPT[V]' UNION SELECT CODE_NM='CREDIT NOTE' UNION SELECT CODE_NM='SALES RETURN' UNION SELECT CODE_NM='DELIVERY CHALLAN' UNION SELECT CODE_NM='GOODS RECEIPT'";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 20);

            DataView dvw = tblPop.DefaultView;

            VForText = "Select Transaction Name";
            vSearchCol = "Code_Nm";
            vDisplayColumnList = "Code_Nm:Transaction Name";
            vReturnCol = "Code_Nm";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.Icon = this.pFrmIcon;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtTrnName.Text = oSelectPop.pReturnArray[0];
                selectedReccnt = 0;
                btnSelctAll.Text = "Select All";
                this.mthView();
                this.txtPartyNm.Text = "";
                this.txtGSTIN.Text = "";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string[] ApiLicenseFile = Directory.GetFiles(Path.Combine(this.pAppPath), "*API Lic.Licx");
            string requestid = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
            string usergstin = Convert.ToString(vDsCommon.Tables[0].Rows[0]["gstin"]).Trim();
            if (usergstin.Trim().Length <= 0)
            {
                MessageBox.Show("GSTIN of Company cannot be Empty...!!!");
                this.ShowProcess("Ready");
                return;
            }
            if (ApiLicenseFile.Length == 0)
            {
                MessageBox.Show("License file of EWB is not found. Cannot continue...!!!");
                this.ShowProcess("Ready");
                return;
            }

            if (_gspid == null || _gspPwd == null || _gspid == string.Empty || _gspPwd == string.Empty)
            {
                MessageBox.Show("License file of EWB is not valid. Cannot continue...!!!");
                this.ShowProcess("Ready");
                return;
            }

            if (_nicuid.Trim().Length <= 0 || _nicpwd.Trim().Length <= 0)
            {
                MessageBox.Show("Please enter GSP User Id/Password in NIC credentials in Control Centre.");
                this.ShowProcess("Ready");
                return;
            }
           
            string jsonstrval = this.CancelJsonString();

            if (jsonstrval == string.Empty)
            {
                return;
            }
            
            //usergstin = _nicuid;          //Commented by Shrikant S. on 20/04/2018    

            this.ShowProcess("Authenticating");

            


            ugstnHelper.clsUdyogGSTINApi oautho = new ugstnHelper.clsUdyogGSTINApi();
            autho_token = oautho.GetAuthentication(authourl, _gspid, _gspPwd);

            this.ShowProcess("Writing Log");
            if (autho_token.Contains("Issue occured,"))
            {
                MessageBox.Show(autho_token);
                this.Update_apilog("Authentication", DateTime.Now, this.pAppUerName, autho_token, string.Empty);
                return;
            }
            else
            {
                this.Update_apilog("Authentication", DateTime.Now, this.pAppUerName, "Successful", string.Empty);
            }

            this.Update_apilog("EWB-Cancel-Request", DateTime.Now, this.pAppUerName, jsonstrval, requestid);

            string ewaybillDetail = oautho.CancelEwayBills(_nicuid, _nicpwd, ewaycancelurl,"Bearer "+autho_token, usergstin, requestid, jsonstrval);
            

            if (ewaybillDetail.Contains("Issue occured"))
            {
                string issueStr = ewaybillDetail.Replace("Issue occured, ", "");
                if (issueStr.Contains("Invalid Token") || issueStr.Contains("Token Expired"))
                {
                    autho_token = string.Empty;
                    this.btnCancel.PerformClick();
                    return;
                }
                if (issueStr.Substring(issueStr.Length - 1, 1) == ",")
                {
                    issueStr = issueStr.Substring(0, issueStr.Length - 1);
                    issueStr = this.GetErrorList(issueStr);
                }
                else
                {
                    issueStr = this.GetErrorList(issueStr);
                }
                this.Update_apilog("EWB-Cancel-Response", DateTime.Now, this.pAppUerName, issueStr, requestid);
                MessageBox.Show(issueStr, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.ShowProcess("Ready");
                return;
            }
            this.ShowProcess("Updating e-way bills");
            DataTable ewayTable = new DataTable();

            DataColumn ewaybillNo = new DataColumn("ewayBillNo", typeof(string));
            ewayTable.Columns.Add(ewaybillNo);
            DataColumn cancelDate = new DataColumn("cancelDate", typeof(string));
            ewayTable.Columns.Add(cancelDate);

            JObject ewbdetails = JObject.Parse(ewaybillDetail);
            CancelEwayList ewayResult = JsonConvert.DeserializeObject<CancelEwayList>(ewbdetails.SelectToken("result").ToString());
            DataRow row = ewayTable.NewRow();
            row["ewayBillNo"] = ewayResult.ewayBillNo;
            row["cancelDate"] = ewayResult.cancelDate;
            ewayTable.Rows.Add(row);

            for (int i = 0; i < ewayTable.Rows.Count; i++)
            {
                DataRow[] selectRows = this.tblGSTIN.Select("Sel='True'");

                if (selectRows.Count() > 0)
                {
                    foreach (DataRow item in selectRows)
                    {
                        item["EWAYN"] = "Cancelled";
                        item["EWAYDT"] = ""; //Convert.ToDateTime("01/01/1900");

                        QRCodeEncoder oQRcode = new QRCodeEncoder();
                        oQRcode.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                        oQRcode.QRCodeScale = 4;
                        oQRcode.QRCodeVersion = 7;
                        oQRcode.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                        if (vDsCommon.Tables[2].Columns.Contains("BC_Size") && vDsCommon.Tables[2].Rows.Count > 0)
                        {
                            oQRcode.QRCodeScale = Convert.ToInt16(vDsCommon.Tables[2].Rows[0]["BC_Size"]);
                        }

                        Bitmap bmp = oQRcode.Encode("");
                        byte[] bytes = (byte[])TypeDescriptor.GetConverter(bmp).ConvertTo(bmp, typeof(byte[]));

                        vSqlStr = "set dateformat dmy Execute Usp_ent_Update_EBN @Docno,@EBN,@EWBDT,@VALID_UPTO,@EBNQR";

                        SqlConnection conn = new SqlConnection(sqlconnstr);
                        SqlCommand cmd = new SqlCommand(vSqlStr, conn);

                        cmd.Parameters.Add("@Docno", SqlDbType.VarChar).Value = Convert.ToString(item["Docno"]);
                        cmd.Parameters.Add("@EBN", SqlDbType.VarChar).Value = Convert.ToString(ewayTable.Rows[i]["ewayBillNo"]);
                        cmd.Parameters.Add("@EWBDT", SqlDbType.VarChar).Value = "01/01/1900";
                        cmd.Parameters.Add("@VALID_UPTO", SqlDbType.VarChar).Value = "01/01/1900";
                        cmd.Parameters.Add("@EBNQR", SqlDbType.Image).Value = bytes;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        item["Sel"] = false;
                    }
                    
                }

            }
            if (ewayTable.Rows.Count > 0)
            {
                this.dgvGSTIN.RefreshEdit();
                this.dgvGSTIN.Refresh();
            }
            MessageBox.Show("EWB cancelled successfully.");
        }

        private void txtTrnName_TextChanged(object sender, EventArgs e)
        {
            this.txtFInvNo.Text = string.Empty;
            this.txtTInvNo.Text = string.Empty;
            this.txtPartyNm.Text = string.Empty;
            this.txtGSTIN.Text = string.Empty;
            this.mthView();
            GetData = this.tblGSTIN;
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {

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
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "ugstEWayBill.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set Dateformat DMY insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
        }

        private void btnSelctAll_Click(object sender, EventArgs e)
        {

            if (dgvGSTIN.Rows.Count > 0)
            {
                if (btnSelctAll.Text == "Select All")
                {
                    for (int i = 0; i < dgvGSTIN.Rows.Count; i++)
                    {
                        dgvGSTIN.Rows[i].Cells["colSelect"].Value = true;
                    }
                    btnSelctAll.Text = "DeSelect All";
                }
                else
                {
                    for (int i = 0; i < dgvGSTIN.Rows.Count; i++)
                    {
                        dgvGSTIN.Rows[i].Cells["colSelect"].Value = false;
                    }
                    btnSelctAll.Text = "Select All";
                }
            }
        }

        private void dgvGSTIN_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGSTIN.Rows.Count > 0)
            {
                if (e.ColumnIndex == 0)
                {
                    if (Convert.ToBoolean(dgvGSTIN.Rows[e.RowIndex].Cells["colSelect"].Value.ToString()) == true)
                        selectedReccnt++;
                    else
                        selectedReccnt--;
                }
            }
        }

        private void txtPartyNm_TextChanged(object sender, EventArgs e)
        {
            this.mthView();
        }

        private void txtGSTIN_TextChanged(object sender, EventArgs e)
        {
            this.mthView();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

        private void btnGenJson_Click(object sender, EventArgs e)
        {
            this.ShowProcess("Generating Json Data");
            string vErrMsg = "";
            mcheckCallingApplication();
            if (this.txtJsonFile.Text == "")
            {
                MessageBox.Show("Please select the json File Path", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.btnJsonFile_Click(sender, e);
                this.btnGenJson.PerformClick();
            }
            else
            {
                if (selectedReccnt > 0)
                {
                    string v = "";
                    int vSelectedRec = 0;
                    StringBuilder JsonString = new StringBuilder();
                    JsonString.Append("{ \n  \"version\": " + "\"1.0.0123\",");
                    JsonString.Append("\n     \"billLists\":");
                    JsonString.Append("[");
                    foreach (DataRow dr in tblGSTIN.Rows)
                    {
                        if ((Boolean)dr["Sel"] == true)
                        {
                            this.mthValidation(dr, ref vErrMsg);
                            if (vErrMsg == "")
                            {
                                JsonString.Append("{\n");
                                this.mthGenJson(this.txtTrnName.Text.ToString().Trim(), dr["DocNo"].ToString(),Convert.ToDateTime(dr["DocDt"]).ToString("MM/dd/yyyy"), ref JsonString);
                                JsonString.Append("\n},\n");
                            }
                            else
                            {
                                MessageBox.Show("Please rectify the below points and then generate the EWB." + Environment.NewLine + vErrMsg, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;

                            }
                        }
                    }
                    if (JsonString.ToString().EndsWith(",\n"))
                        JsonString.Remove(JsonString.Length - 2, 2);

                    JsonString.Append("\n]}");
                    v = JsonString.ToString();

                    string fName = @"\ugstEWayBill" + "_" + DateTime.Now.Date.ToString("dd/MM/yy") + "_" + DateTime.Now.ToString("hh:mm:ss tt");
                    fName = fName.Replace(".", "").Replace("-", "").Replace("/", "").Replace(":", "").Replace(" ", "") + ".Json";
                    fName = this.txtJsonFile.Text + fName;


                    System.IO.File.WriteAllText(fName, v);

                    MessageBox.Show("JSON file has been generated at the below location : \n" + fName.ToString(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    selectedReccnt = 0;
                    this.mthView();

                }
                else
                {
                    MessageBox.Show("Please select atleast one row of data to generate JSON file.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.ShowProcess("Ready");
                    return;
                }

            }
            this.ShowProcess("Ready");
        }
        private void mthValidation(DataRow dr, ref string vErrMsg)
        {
            if (dr["U_VehNo"].ToString().Trim().IndexOf(" ") > 0)
            {
                vErrMsg = vErrMsg + "Please remove space from [Vehicle Number] in Additional Info/Invoice Details in Invoice/Transaction number: " + dr["docNo"].ToString() + "\n";
            }
            if (dr["U_VehNo"].ToString().Trim() == "" && dr["transporterId"].ToString().Trim() == "" && dr["transporterName"].ToString().Trim() == "")
            {
                vErrMsg = vErrMsg + "Please Enter Vehicle Number or Transporter Id or Trnsporter Name in " + dr["docNo"].ToString() + "\n"; ;
            }
            if (dr["PartyPincode"].ToString().Trim() == "" || dr["PartyPincode"].ToString().Trim().Length < 6)
            {
                vErrMsg = vErrMsg + "Please enter valid [PIN/ZIP] in  Account Master  for " + dr["LgNm"].ToString() + "\n"; ;
            }

            if (dr["TranMode"].ToString().Trim() != string.Empty)
            {
                string[] transMode = new string[] { "Road", "Rail", "Air", "Ship" };
                if (!transMode.Contains(dr["TranMode"].ToString().Trim()))
                {
                    vErrMsg = vErrMsg + "Please enter mode of Transport as Road/Rail/Air/Ship in Invoice/Transaction number:" + dr["docNo"].ToString() + "\n";
                }

            }


            if (dr["TranMode"].ToString().Trim() == "Rail" || dr["TranMode"].ToString().Trim() == "Air" || dr["TranMode"].ToString().Trim() == "Ship")
            {
                if (dr["transDocNo"].ToString().Trim().Length <= 0 || Convert.ToInt32(Convert.ToDateTime(dr["transDocDate"]).Year) <= 1900)
                    vErrMsg = vErrMsg + "Please enter Trans. Doc.(LR) No. and Trans. Doc.(LR) Date. in Additional Info/Invoice Details in Invoice/Transaction number:" + dr["docNo"].ToString() + "\n";
            }
            string vComPin = vDsCommon.Tables["Company"].Rows[0]["zip"].ToString().Trim();
            if (vComPin == "" || vComPin.Length < 6)
            {
                vErrMsg = vErrMsg + "Please enter valid [PIN/ZIP] in Company Master";
            }
            string vmailname = vDsCommon.Tables["Company"].Rows[0]["Mailname"].ToString().Trim();
            if (vmailname.Length > 100)
            {
                vErrMsg = vErrMsg + "Mailing name in Company Master should have length upto 100 characters";
            }
            //if (dr["TranMode"].ToString().Trim() == "Road" || dr["TranMode"].ToString().Trim() == "Rail")
            //{
                if ((decimal)dr["AproxDis"] == 0)
                {
                    vErrMsg = vErrMsg + "Please enter [Approx Distance in km (EWB)] in Additional Info/Invoice Details in Invoice/Transaction number :" + dr["docNo"].ToString();
                }
            //}
        }
        //private void test()
        //{
        //    var file = File.OpenText("d:\\test.json");
        //    string jsonstrval = file.ReadToEnd();
        //    var jsonObject = JObject.Parse(jsonstrval);
        //    string status = (string)jsonObject.SelectToken("error")["errorCodes"];
        //    MessageBox.Show(status);
        //}

        private void btnGenEWB_Click(object sender, EventArgs e)
        {
            string[] ApiLicenseFile = Directory.GetFiles(Path.Combine(this.pAppPath), "*API Lic.Licx");
            string requestid = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
            string usergstin = Convert.ToString(vDsCommon.Tables[0].Rows[0]["gstin"]).Trim();
            if (usergstin.Trim().Length <= 0)
            {
                MessageBox.Show("GSTIN of Company cannot be Empty...!!!");
                this.ShowProcess("Ready");
                return;
            }

            if (ApiLicenseFile.Length == 0)
            {
                MessageBox.Show("License file of EWB is not found. Cannot continue...!!!");
                this.ShowProcess("Ready");
                return;
            }


            if (_gspid == null || _gspPwd == null || _gspid == string.Empty || _gspPwd == string.Empty)
            {
                MessageBox.Show("License file of EWB is not valid. Cannot continue...!!!");
                this.ShowProcess("Ready");
                return;
            }
            if (_nicuid.Trim().Length <= 0 || _nicpwd.Trim().Length <= 0)
            {
                MessageBox.Show("Please enter GSP User Id/Password in NIC credentials in Control Centre.");
                this.ShowProcess("Ready");
                return;
            }
            
            
            //usergstin = _nicuid;      Commented by Shrikant S. on 20/04/2018 

            this.ShowProcess("Generating Json Data");
            string jsonstrval = this.WriteJsonString();
            if (jsonstrval == string.Empty)
            {
                return;
            }
            //System.IO.File.WriteAllText(Path.Combine(Application.StartupPath, "jsonval_" + requestid + ".json"), jsonstrval);
            //isDemo = true;
            //if (isDemo == true)
            //{
            //    var file = File.OpenText(Path.Combine(Application.StartupPath, "EwayBillTest.json"));
            //    jsonstrval = file.ReadToEnd();
            //}
            ugstnHelper.clsUdyogGSTINApi oautho = new ugstnHelper.clsUdyogGSTINApi();

            if (autho_token == string.Empty)
            {
                this.ShowProcess("Authenticating");
                autho_token = oautho.GetAuthentication(authourl, _gspid, _gspPwd);
                if (autho_token.Contains("Issue occured,"))
                {
                    string issueStr = autho_token.Replace("Issue occured, ", "");
                    MessageBox.Show(issueStr);
                    this.Update_apilog("Authentication", DateTime.Now, this.pAppUerName, issueStr, string.Empty);
                    this.ShowProcess("Ready");
                    autho_token = string.Empty;             //Added by Shrikant S. on 20/04/2018
                    return;
                }
                else
                {
                    this.Update_apilog("Authentication", DateTime.Now, this.pAppUerName, "Successful", string.Empty);
                }
                this.ShowProcess("Requesting ewb for requestid: " + requestid);
            }
            this.Update_apilog("EWB-Request", DateTime.Now, this.pAppUerName, jsonstrval, requestid);

            string ewaybillDetail = oautho.GetEwayBillDetails(_nicuid, _nicpwd, ewayurl, "Bearer " + autho_token, usergstin, requestid, jsonstrval);
            
            if (ewaybillDetail.Contains("Issue occured, "))
            {
                string issueStr = ewaybillDetail.Replace("Issue occured, ", "");
                if (issueStr.Contains("Invalid Token") || issueStr.Contains("Token Expired"))
                {
                    autho_token = string.Empty;
                    this.btnGenEWB.PerformClick();
                    return;
                }
                if (issueStr.Substring(issueStr.Length - 1, 1) == ",")
                {
                    issueStr = issueStr.Substring(0, issueStr.Length - 1);
                    issueStr = this.GetErrorList(issueStr);
                }
                this.Update_apilog("EWB-Response", DateTime.Now, this.pAppUerName, issueStr, requestid);
                MessageBox.Show(issueStr, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.ShowProcess("Ready");
                autho_token = string.Empty;             //Added by Shrikant S. on 20/04/2018
                return;
            }
            this.ShowProcess("Received ewb response for requestid: " + requestid);

            DataTable ewayTable = new DataTable();
            DataColumn ewaybillNo = new DataColumn("ewaybillNo", typeof(string));
            ewayTable.Columns.Add(ewaybillNo);
            DataColumn ewaybillDate = new DataColumn("ewaybillDate", typeof(string));
            ewayTable.Columns.Add(ewaybillDate);
            DataColumn valid_upto = new DataColumn("validupto", typeof(string));
            ewayTable.Columns.Add(valid_upto);
            this.ShowProcess("Updating EWB details ");

            JObject ewbdetails = JObject.Parse(ewaybillDetail);

            EwayList ewayResult = JsonConvert.DeserializeObject<EwayList>(ewbdetails.SelectToken("result").ToString());
            DataRow row = ewayTable.NewRow();
            row["ewaybillNo"] = ewayResult.ewayBillNo;
            row["ewaybillDate"] = ewayResult.ewayBillDate;
            row["validupto"] = ewayResult.validUpto;
            ewayTable.Rows.Add(row);

            this.ShowProcess("Updating EWB QC Code");
            for (int i = 0; i < ewayTable.Rows.Count; i++)
            {
                DataRow[] selectRows = this.tblGSTIN.Select("Sel='True'");

                if (selectRows.Count() > 0)
                {
                    foreach (DataRow item in selectRows)
                    {
                        item["EWAYN"] = Convert.ToString(ewayTable.Rows[i]["ewaybillNo"]);
                        item["EWAYDT"] = ewayTable.Rows[i]["ewaybillDate"];

                        QRCodeEncoder oQRcode = new QRCodeEncoder();
                        oQRcode.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                        oQRcode.QRCodeScale = 4;
                        oQRcode.QRCodeVersion = 7;
                        oQRcode.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                        if (vDsCommon.Tables[2].Columns.Contains("BC_Size") && vDsCommon.Tables[2].Rows.Count > 0)
                        {
                            oQRcode.QRCodeScale = Convert.ToInt16(vDsCommon.Tables[2].Rows[0]["BC_Size"]);
                        }

                        Bitmap bmp = oQRcode.Encode(Convert.ToString(ewayTable.Rows[i]["ewaybillNo"]));
                        byte[] bytes = (byte[])TypeDescriptor.GetConverter(bmp).ConvertTo(bmp, typeof(byte[]));


                        vSqlStr = "set dateformat dmy Execute Usp_ent_Update_EBN @Docno,@EBN,@EWBDT,@EWB_VALID,@EBNQR";

                        SqlConnection conn = new SqlConnection(sqlconnstr);
                        SqlCommand cmd = new SqlCommand(vSqlStr, conn);

                        cmd.Parameters.Add("@Docno", SqlDbType.VarChar).Value = Convert.ToString(item["Docno"]);
                        cmd.Parameters.Add("@EBN", SqlDbType.VarChar).Value = Convert.ToString(ewayTable.Rows[i]["ewaybillNo"]);
                        cmd.Parameters.Add("@EWBDT", SqlDbType.VarChar).Value = Convert.ToString(ewayTable.Rows[i]["ewaybillDate"]);
                        cmd.Parameters.Add("@EWB_VALID", SqlDbType.VarChar).Value = Convert.ToString(ewayTable.Rows[i]["validupto"]);
                        cmd.Parameters.Add("@EBNQR", SqlDbType.Image).Value = bytes;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        item["Sel"] = false;
                    }
                    
                }
                this.ShowProcess("Updating EWB completed");

            }
            if (ewayTable.Rows.Count > 0)
            {
                this.dgvGSTIN.RefreshEdit();
                this.dgvGSTIN.Refresh();
            }
            MessageBox.Show("EWB generated successfully.");
            this.ShowProcess("Ready");
        }
        private string CancelJsonString()
        {
            DataRow[] SelectedRows = null;
            SelectedRows = this.tblGSTIN.Select("Sel='True'");
            if (SelectedRows.Count() > 1)
            {
                MessageBox.Show("Cannot select more than one Record.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.ShowProcess("Ready");
                return string.Empty;
            }
            SelectedRows = null;
            SelectedRows = this.tblGSTIN.Select("Sel='True' and ewayn<>''");

            //if (this.isDemo == true)
            //    SelectedRows = this.tblGSTIN.Select("Sel='True' ");
            //else
            //    SelectedRows = this.tblGSTIN.Select("Sel='True' and ewayn<>''");

            if (SelectedRows.Count() <= 0)
            {
                MessageBox.Show("Please select atleast one Record of EWB(generated) for cancellation of E-way bill ", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.ShowProcess("Ready");
                return string.Empty;
            }
            if (SelectedRows.Count() > 1)
            {
                MessageBox.Show("Cannot select more than one Record.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.ShowProcess("Ready");
                return string.Empty;
            }
            StringBuilder JsonString = new StringBuilder();

            foreach (DataRow row in SelectedRows)
            {
                for (int i = 0; i < SelectedRows.Count(); i++)
                {
                    this.ShowProcess("Collecting Data for Json of Transaction No.: " + SelectedRows[i]["DocNo"].ToString());
                    JsonString.Append("{");
                    JsonString.Append("\n\"" + "ewbNo" + "\": " + SelectedRows[i]["ewayn"].ToString() + ",");
                    JsonString.Append("\n\"" + "cancelRsnCode" + "\": 2,");
                    JsonString.Append("\n\"" + "cancelRmrk" + "\": \""+"Cancelled the order"+"\"");
                    JsonString.Append("\n}" + (i == SelectedRows.Count() - 1 ? "" : ","));
                }
            }

            return JsonString.ToString();

        }
        private string WriteJsonString()
        {
            string vErrMsg = "";
            DataRow[] SelectedRows = this.tblGSTIN.Select("Sel=1");

            if (SelectedRows.Count() <= 0)
            {
                MessageBox.Show("Please select atleast one Record", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.ShowProcess("Ready");
                return string.Empty;
            }
            if (SelectedRows.Count() > 1)
            {
                MessageBox.Show("Cannot select more than one Record.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.ShowProcess("Ready");
                return string.Empty;
            }
            StringBuilder JsonString = new StringBuilder();


            foreach (DataRow row in SelectedRows)
            {
                this.ShowProcess("Collecting Data for Json of Transaction No.: " + row["DocNo"].ToString());
                this.mthValidation(row, ref vErrMsg);
                if (vErrMsg == "")
                {
                    JsonString.Append("{\n");
                    this.Generate_EWB_Json(this.txtTrnName.Text.ToString().Trim(), row["DocNo"].ToString(),Convert.ToDateTime(row["DocDt"]).ToString("MM/dd/yyyy"), ref JsonString);
                    JsonString.Append("\n}");
                }
                else
                {
                    MessageBox.Show("Please rectify the below points and then generate the EWB." + Environment.NewLine + vErrMsg, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return string.Empty;
                }
            }

            return JsonString.ToString();
        }
        private string GetErrorList(string errorCode)
        {
            string errorDesc = string.Empty;
            try
            {
                vSqlStr = "Select ErrorDesc From EWBErrorDetail Where ErrorCode in ("+ errorCode + ")";

                SqlConnection conn = new SqlConnection(sqlconnstr);
                SqlCommand cmd = new SqlCommand(vSqlStr, conn);
                //cmd.Parameters.Add("@ErrorCode", SqlDbType.VarChar).Value = errorCode ;

                SqlDataAdapter lda = new SqlDataAdapter(cmd);
                DataSet lds = new DataSet();
                lda.Fill(lds);
                for (int i = 0; i < lds.Tables[0].Rows.Count; i++)
                {
                    errorDesc = errorDesc + lds.Tables[0].Rows[i]["ErrorDesc"].ToString()+"\n";
                }
                //conn.Open();
                //errorDesc = Convert.ToString(cmd.ExecuteScalar());
                //conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return errorDesc;
        }
        private void gbxFilter_Enter(object sender, EventArgs e)
        {

        }

        private void btnFInvNo_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            //string tbl_nm = string.Empty;

            //switch (this.txtTrnName.Text.Trim().ToUpper())
            //{
            //    case "SALES":
            //        tbl_nm = "STMAIN";
            //        break;
            //    case "PURCHASE":
            //        tbl_nm = "PTMAIN";
            //        break;
            //    case "LABOUR JOB ISSUE[IV]":
            //    case "LABOUR JOB ISSUE[V]":
            //        tbl_nm = "IIMAIN";
            //        break;
            //    case "LABOUR JOB RECEIPT[IV]":
            //    case "LABOUR JOB RECEIPT[V]":
            //        tbl_nm = "IRMAIN";
            //        break;
            //    case "CREDIT NOTE":
            //        tbl_nm = "CNMAIN";
            //        break;
            //    case "SALES RETURN":
            //        tbl_nm = "SRMAIN";
            //        break;
            //    case "DELIVERY CHALLAN":
            //        tbl_nm = "DCMAIN";
            //        break;
            //    default:
            //        break;
            //}
            //vSqlStr = "Select Inv_no=convert(varchar(20),'') Union all Select Inv_no From " + tbl_nm + " Order by Inv_no";
            //tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);

            string filterstr = " 1=1 ";

            if (this.txtPartyNm.Text.Trim().Length > 0)
            {
                filterstr = filterstr + string.Format(" and Partynm='{0}' ", this.txtPartyNm.Text.Trim().Replace("'", "''"));
            }
            if (this.txtGSTIN.Text.Trim().Length > 0)
            {
                filterstr = filterstr + string.Format(" and gstin='{0}' ", this.txtGSTIN.Text.Trim());
            }
            DataRow[] InvRows = this.GetData.Select(filterstr);

            tblPop = new DataTable();
            tblPop.Columns.Add("Inv_no", typeof(string));
            foreach (DataRow item in InvRows)
            {
                DataRow row = tblPop.NewRow();
                row["Inv_no"] = item["DocNo"];
                tblPop.Rows.Add(row);
            }


            DataView dvw = tblPop.DefaultView;
            dvw.Sort = "Inv_no";

            VForText = "Select Document No.";
            vSearchCol = "Inv_no";
            vDisplayColumnList = "Inv_no:Document No.";
            vReturnCol = "Inv_no";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.Icon = this.pFrmIcon;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtFInvNo.Text = oSelectPop.pReturnArray[0];
                if (this.txtTInvNo.Text.Trim().Length <= 0)
                {
                    this.txtTInvNo.Text = oSelectPop.pReturnArray[0];
                }
                this.mthView();
            }
        }

        private void btnTInvNo_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            //string tbl_nm = string.Empty;

            //switch (this.txtTrnName.Text.Trim().ToUpper())
            //{
            //    case "SALES":
            //        tbl_nm = "STMAIN";
            //        break;
            //    case "PURCHASE":
            //        tbl_nm = "PTMAIN";
            //        break;
            //    case "LABOUR JOB ISSUE[IV]":
            //    case "LABOUR JOB ISSUE[V]":
            //        tbl_nm = "IIMAIN";
            //        break;
            //    case "LABOUR JOB RECEIPT[IV]":
            //    case "LABOUR JOB RECEIPT[V]":
            //        tbl_nm = "IRMAIN";
            //        break;
            //    case "CREDIT NOTE":
            //        tbl_nm = "CNMAIN";
            //        break;
            //    case "SALES RETURN":
            //        tbl_nm = "SRMAIN";
            //        break;
            //    case "DELIVERY CHALLAN":
            //        tbl_nm = "DCMAIN";
            //        break;
            //    default:
            //        break;
            //}
            //vSqlStr = "Select Inv_no=convert(varchar(20),'') Union all Select Inv_no From " + tbl_nm + " Order by Inv_no";
            //tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);

            string filterstr = " 1=1 ";

            if (this.txtPartyNm.Text.Trim().Length > 0)
            {
                filterstr = filterstr + string.Format(" and Partynm='{0}' ", this.txtPartyNm.Text.Trim().Replace("'", "''"));
            }
            if (this.txtGSTIN.Text.Trim().Length > 0)
            {
                filterstr = filterstr + string.Format(" and gstin='{0}' ", this.txtGSTIN.Text.Trim());
            }

            DataRow[] InvRows = this.GetData.Select(filterstr);

            tblPop = new DataTable();
            tblPop.Columns.Add("Inv_no", typeof(string));
            foreach (DataRow item in InvRows)
            {
                DataRow row = tblPop.NewRow();
                row["Inv_no"] = item["DocNo"];
                tblPop.Rows.Add(row);
            }

            DataView dvw = tblPop.DefaultView;
            dvw.Sort = "Inv_no";

            VForText = "Select Document No.";
            vSearchCol = "Inv_no";
            vDisplayColumnList = "Inv_no:Document No.";
            vReturnCol = "Inv_no";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.Icon = this.pFrmIcon;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtTInvNo.Text = oSelectPop.pReturnArray[0];
                this.mthView();
            }
        }

        private void dgvGSTIN_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
        }
    }
}
