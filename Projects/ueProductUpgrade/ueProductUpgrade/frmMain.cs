using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;
using System.Threading;
using System.Reflection;
using System.Globalization;
using System.Diagnostics; //Added by sandeep for bug-18141 on 14-SEP-13
using System.IO;
using DataAccess_Net; //Added by sandeep for bug-18141 on 14-SEP-13
using uBaseForm;  //Added by sandeep for bug-18141 on 14-SEP-13
using ueconnect;
using Udyog.Library.Common;
using System.Text.RegularExpressions;

namespace ueProductUpgrade
{
    //public partial class frmMain : Form //commented by sandeep for bug-18141 on 14-SEP-13 
    public partial class frmMain : uBaseForm.FrmBaseForm //Added by sandeep for bug-18141 on 14-SEP-13 
    {
        ReadSetting readIni;
        SqlConnection conn;
        DataSet ds = new DataSet();
        clsConnect oConnect;

        string[] vu10Prod = new string[] { "VudyogSDK", "VudyogPRO", "VudyogENT", "VudyogSTD", "10USquare", "10iTAX", "USquare10", "iTAX10", "UdyogGST", "UdyogGSTSDK", "UdyogERP", "UdyogERPSDK", "UdyogERPSTD", "UdyogERPPRO", "UdyogERPENT", "UdyogERPSDK" }; //Added by Rupesh G. on 01/09/2017 

        //      string[] vu10Prod = new string[] { "VudyogSDK", "VudyogPRO", "VudyogENT", "VudyogSTD", "10USquare", "10iTAX", "USquare10", "iTAX10", "VudyogGST", "VudyogGSSDK" }; //Comment by Rupesh G. on 01/09/2017 
        ArrayList stringList = new ArrayList();
        string appPath = string.Empty;
        string MacId = string.Empty;
        string ErrorMsg = string.Empty;
        string RegProd = string.Empty;
        DataTable UpdateTbl;
        string LogFile = string.Empty;
        DataTable LogTable;
        ProgressBar pbar;
        Int32 totRecords = 0;
        Int32 unFetRecords = 0;
        float ratio;
        string vuMess = string.Empty;
        string ProcessStart = string.Empty;
        string ProcessEnd = string.Empty;
        //Added by sandeep for bug-18141 on 14-SEP-13-->Start 
        private string vLocCode = string.Empty;
        private DataSet dsCompany;
        DataAccess_Net.clsDataAccess oDataAccess;
        private String cAppPId, cAppName;
        int vFileNo;
        bool IsExitCalled;
        string vIE_LocCode;

        int ListViewItemCount;
        const int Timeout = 5000;

        bool _IgnoreStandardRule = false;               //Added by Shrikant S. on 05/05/2018 for Bug-31515
        bool _CheckRegisterMeFileExists = false;        //Added by Shrikant S. on 05/05/2018 for Bug-31515
        bool _InfFileExists = false;                    //Added by Shrikant S. on 05/05/2018 for Bug-31515

        DataTable optionTable = new DataTable();
        DataTable AddiModule = new DataTable();             //Added by Shrikant S. on 02/02/2019 for Registration
        string zipFileName = string.Empty;                  //Added by Shrikant S. on 04/02/2019 for Registration
        DataTable Complist = new DataTable();               //Added by Shrikant S. on 06/02/2019 for Registration
        string mudprodcode = string.Empty;                  //Added by Shrikant S. on 06/02/2019 for Registration
        string status = string.Empty;                       //Added by Shrikant S. on 06/02/2019 for Registration
        BackgroundWorker oWorker;                           //Added by Shrikant S. on 06/02/2019 for Registration
        int progressPercentage = 0;                         //Added by Shrikant S. on 06/02/2019 for Registration
        string ProdSrno = string.Empty;                     //Added by Shrikant S. on 01/04/2019 for Registration
        string LicenseKey = string.Empty;                   //Added by Shrikant S. on 01/04/2019 for Registration    
        int UpgradationCount = 0;                           //Added by Shrikant S. on 01/04/2019 for Registration    
        string mudshortcode = string.Empty;                 //Added by Shrikant S. on 01/04/2019 for Registration    
        string regshortcode = string.Empty;                 //Added by Shrikant S. on 01/04/2019 for Registration    
        string upgradeAppTitle = string.Empty;
        string upgradeProdcode = string.Empty;
        DataTable upgradeInfData = new DataTable();
        DataTable UpgradecompList = new DataTable();                //Added by Shrikant S. on 26/04/2019 for Registration 

        int regicompcnt = 0, regiusercnt = 0, totcompcnt = 0, totusercnt = 0, addicompcnt = 0, addiusercnt = 0; //Added by Shrikant S. on 06/02/2019 for Registration
        bool checkMefile = false;                   //Added by Shrikant S. on 06/02/2019 for Registration
        string _suptype = string.Empty;             //Added by Shrikant S. on 30/08/2019 for SDK Installer 

        List<string> LogFileList = new List<string>();

        //Added by sandeep for bug-18141 on 14-SEP-13-->End 
        #region Properties
        private string _CurrentApplication = string.Empty;
        public string CurrentApplication
        {
            get { return _CurrentApplication; }
            set { _CurrentApplication = value; }
        }
        private string _CurrentAppFile;
        public string CurrentAppFile
        {
            get { return _CurrentAppFile; }
        }
        private string _UpgradeAppFile;
        public string UpgradeAppFile
        {
            get { return _UpgradeAppFile; }
        }
        private string _UpgradeApplication = string.Empty;
        public string UpgradeApplication
        {
            get { return _UpgradeApplication; }
            set { _UpgradeApplication = value; }
        }

        #endregion
        //commented by sandeep for bug-18141 on 14-SEP-13-->S 
        //public frmMain()
        //{
        //    InitializeComponent();
        //}
        //commented by sandeep for bug-18141 on 14-SEP-13-->E 
        //added by sandeep for bug-18141 on 14-SEP-13---->start      
        public frmMain(string[] args)
        {
            InitializeComponent();
            //for (int i = 0; i < args.Length; i++)
            //{
            //    MessageBox.Show(i.ToString() + "=" + args[i]);
            //}

            ListViewItemCount = 0;
            this.pFrmCaption = "Product Upgrade";
            this.Text = this.pFrmCaption;
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];

            if (args[5] != "")
            {
                this.pPApplRange = args[5].ToString().Replace("^", "");
            }
            this.pAppUerName = args[6];
            Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
            this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt32(args[10]);

            this.pPApplCode = args[11];

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            //Commented by Shrikant S. on 02/02/2019 for Registration       //Start
            //dsCompany = new DataSet();
            //string strSQL = "select * from vudyog..Co_Mast where compid=" + this.pCompId;
            //dsCompany = oDataAccess.GetDataSet(strSQL, null, 20);
            //Commented by Shrikant S. on 02/02/2019 for Registration       //End

            this.mInsertProcessIdRecord();

            AddiModule.Columns.Add("Company", typeof(string));
            AddiModule.Columns.Add("AddiMod", typeof(string));
            AddiModule.Columns.Add("RowId", typeof(int));

            Complist.Columns.Add("compname", typeof(string));
            Complist.Columns.Add("addimodlist", typeof(string));
            Complist.Columns.Add("addimodcode", typeof(string));
            Complist.Columns.Add("selmodlist", typeof(string));
            Complist.Columns.Add("selmodcode", typeof(string));
            Complist.Columns.Add("selmainmodule", typeof(string));

            oWorker = new BackgroundWorker();
            oWorker.DoWork += new DoWorkEventHandler(oWorker_DoWork);
            oWorker.ProgressChanged += new ProgressChangedEventHandler(oWorker_ProgressChanged);
            oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(oWorker_RunWorkerCompleted);
            oWorker.WorkerReportsProgress = true;
            oWorker.WorkerSupportsCancellation = true;

        }
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "ueProductUpgrade.EXE";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "','" + this.pPApplName + "'," + this.pPApplPID.ToString() + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, Timeout, true);
        }
        private void mDeleteProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            oDataAccess.ExecuteSQLStatement(sqlstr, null, Timeout, true);
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
                MessageBox.Show("Can't proceed, Main Application " + this.pPApplText + " is closed", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                IsExitCalled = true;
                Application.Exit();
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mDeleteProcessIdRecord();
        }

        //added by sandeep for bug-18141 on 14-SEP-13-->End


        #region Method to Implement
        public void SetNavigationVisibility()
        {
            if (this.tabControl1.SelectedIndex == 0)
            {
                this.btnBack.Enabled = false;
                this.btnNext.Enabled = true;
                this.btnFinish.Enabled = false;
            }
            else if (this.tabControl1.SelectedIndex == (this.tabControl1.TabCount - 1))
            {
                this.btnBack.Enabled = true;
                this.btnNext.Enabled = false;
                this.btnFinish.Enabled = true;
            }
            else
            {
                this.btnBack.Enabled = true;
                this.btnNext.Enabled = true;
                this.btnFinish.Enabled = false;
            }
            this.Refresh();
        }
        public void SetImages()
        {
            readIni = new ReadSetting(appPath);
            //this.Icon = new Icon(appPath + "\\BMP\\UEICON.ICO");  //Commented by Priyanka B on 16/05/2017
            //this.Icon = new Icon(appPath + "\\BMP\\ICON_VUDYOGGST.ICO");  //Added by Priyanka B on 16/05/2017

            foreach (string fileName in Directory.GetFiles(appPath + "\\BMP\\"))
            {
                FileInfo f = new FileInfo(fileName);
                if (f.Name.ToUpper().IndexOf("_" + readIni.AppFile.ToUpper() + ".JPG") > 0)
                    this.picImage.Image = Image.FromFile(f.FullName);
            }
            //_CurrentApplication = readIni.AppTitle;       //Commented By Shrikant S. on 18/10/2012 
            _CurrentAppFile = readIni.AppFile;
            _CurrentApplication = readIni.AppTitle;
        }
        public string GetCurrentAppName(string appName)      //Added By Shrikant S. on 18/10/2012 
        {
            string result = string.Empty;
            try
            {
                conn = new SqlConnection(readIni.ConnectionString);
                conn.Open();
                
                SqlCommand lcmd = new SqlCommand("Select Top 1 AppDesc From ApplicationDet Where LTrim(RTrim(AppName))='" + appName.Trim() + "'", conn);
                result = (string)lcmd.ExecuteScalar();
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        public void SetStyleToGrid()
        {
            dgvComp1.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvComp2.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvProduct.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvProductUpgrade.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        public void BindData()
        {
            conn = new SqlConnection(readIni.ConnectionString);
            string sqlStr = string.Empty;
            //sqlStr = "Select CompId,Co_Name,Passroute=rtrim(convert(varchar(250),Passroute)),Passroute1=rtrim(convert(varchar(250),Passroute1)),add1,add2,add3,area,zone,city,zip,state,country,email,dbname,foldername,sta_dt,end_dt From vudyog..Co_Mast Where Enddir='' and com_type='' and compId=" + this.pCompId.ToString();        //Commented by Shrikant S. on 02/02/2019 for Registration
            sqlStr = "Select CompId,Co_Name,Passroute=rtrim(convert(varchar(max),Passroute)),Passroute1=rtrim(convert(varchar(max),Passroute1)),add1,add2,add3,area,zone,city,zip,state,country,email,dbname,foldername,sta_dt,end_dt,gstin From vudyog..Co_Mast Where Enddir='' and com_type='' and Convert(Varchar(max),ProdCode)=@ProdCode  ";        //Added by Shrikant S. on 02/02/2019 for Registration
            SqlCommand lcmd = new SqlCommand(sqlStr, conn);
            lcmd.Parameters.Add(new SqlParameter("@ProdCode", oConnect.RetAppEnc(this.CurrentAppFile)));
            SqlDataAdapter da = new SqlDataAdapter(lcmd);
            ds = new DataSet();
            da.Fill(ds, "Co_Mast_vw");

            dgvProduct.AutoGenerateColumns = false;                     //Added by Shrikant S. on 27/04/2019 for Registration
            dgvProductUpgrade.AutoGenerateColumns = false;              //Added by Shrikant S. on 27/04/2019 for Registration
            dgvComp1.AutoGenerateColumns = false;                       //Added by Shrikant S. on 27/04/2019 for Registration
            dgvComp2.AutoGenerateColumns = false;                       //Added by Shrikant S. on 27/04/2019 for Registration

            dgvComp1.DataSource = ds.Tables["Co_Mast_vw"];
            dgvComp1.Columns[0].DataPropertyName = "Co_Name";
            dgvComp1.Columns[1].DataPropertyName = "CompId";

            UpgradecompList = ds.Tables["Co_Mast_vw"].Copy();           // Added by Shrikant S. on 26/04/2019 for Registration
            dgvComp2.DataSource = UpgradecompList;                         // Added by Shrikant S. on 26/04/2019 for Registration
            dgvComp2.Columns[0].DataPropertyName = "Co_Name";
            dgvComp2.Columns[1].DataPropertyName = "CompId";                // Added by Shrikant S. on 26/04/2019 for Registration
            GetProd();
            GetCompanyProducts();



            dgvProduct.DataSource = ds.Tables["cProduct_vw"];
            dgvProduct.Columns[0].DataPropertyName = "Sel";
            dgvProduct.Columns[1].DataPropertyName = "CProductName";

            dgvProductUpgrade.DataSource = ds.Tables["Product_vw"];
            dgvProductUpgrade.Columns[0].DataPropertyName = "Sel";
            dgvProductUpgrade.Columns[1].DataPropertyName = "cProdName";
            dgvProductUpgrade.Columns[2].DataPropertyName = "isDefault";            //Added by Shrikant S. on 05/02/2019 for Registration 
            dgvProductUpgrade.Columns[2].Visible = false;                           //Added by Shrikant S. on 05/02/2019 for Registration 
            dgvProductUpgrade.Columns[3].DataPropertyName = "FoundInInf";            //Added by Shrikant S. on 05/02/2019 for Registration 
            dgvProductUpgrade.Columns[3].Visible = false;                           //Added by Shrikant S. on 05/02/2019 for Registration 
            dgvProductUpgrade.Columns[4].DataPropertyName = "cModDep";
            dgvProductUpgrade.Columns[4].Visible = false;
            dgvProductUpgrade.Columns[5].DataPropertyName = "cCmbNotAlwd";
            dgvProductUpgrade.Columns[6].DataPropertyName = "cProdName";
            dgvProductUpgrade.Columns[7].DataPropertyName = "cProdCode";
            dgvProductUpgrade.Columns[8].DataPropertyName = "cMainProdCode";
            dgvProductUpgrade.Columns[9].DataPropertyName = "CurrSel";
            dgvProductUpgrade.Columns[9].Visible = false;
            SetDefaultProduct();
        }

        public void SetDefaultProduct()
        {
            for (int i = 0; i < ds.Tables["Product_vw"].Rows.Count; i++)
            {
                for (int j = 0; j < ds.Tables["cProduct_vw"].Rows.Count; j++)
                {
                    if (ds.Tables["Product_vw"].Rows[i]["cProdCode"].ToString() == ds.Tables["cProduct_vw"].Rows[j]["cProduct"].ToString() && ds.Tables["Product_vw"].Rows[i]["CompId"].ToString() == ds.Tables["cProduct_vw"].Rows[j]["CompId"].ToString())
                    {
                        ds.Tables["Product_vw"].Rows[i]["Sel"] = true;
                        ds.Tables["Product_vw"].Rows[i]["CurrSel"] = true;
                    }
                }
            }
        }

        public string GetProductDescription(string Prodcode)
        {

            string retValue = string.Empty;

            if (vu10Prod.Contains(this.CurrentAppFile))
            {
                for (int i = 0; i < ds.Tables["Product_vw"].Rows.Count; i++)
                {
                    if (ds.Tables["Product_vw"].Rows[i]["cProdCode"].ToString().Trim() == Prodcode.Trim())      // 26/04/2014 trim added to condition by Shrikant S.
                    {
                        retValue = ds.Tables["Product_vw"].Rows[i]["cProdName"].ToString();
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < ds.Tables["Product_vw"].Rows.Count; i++)
                {
                    if (ds.Tables["Product_vw"].Rows[i]["cMainProdCode"].ToString() == Prodcode)
                    {
                        retValue = ds.Tables["Product_vw"].Rows[i]["cProdCode"].ToString();
                        break;
                    }
                }
            }
            return retValue;
        }
        public void GetProd()
        {
            conn = new SqlConnection(readIni.ConnectionString);
            string sqlStr = string.Empty;
            //sqlStr = "Select Sel=convert(bit,0),a.Enc1,a.Enc2,EValue=space(2000),cProdName=space(100),cProdCode=space(10),cCmbNotAlwd=space(250),cModDep=space(250),cMainProdCode=space(250),nProdType =convert(bit,0),b.CompId  From Vudyog..ModuleMast a, Vudyog..co_Mast b Where a.Enc1=@pEnc and b.enddir='' and b.compid=" + this.pCompId.ToString();        //Commented by Shrikant S. on 02/02/2019 for Registration
            sqlStr = "Select Sel=convert(bit,0),a.Enc1,a.Enc2,EValue=space(2000),cProdName=space(100),cProdCode=space(10),cCmbNotAlwd=space(250),cModDep=space(250),cMainProdCode=space(250),nProdType =convert(bit,0),b.CompId,ModOrder=Space(100),isDefault=convert(Bit,0),FoundInInf=convert(Bit,0),currsel=convert(Bit,0)  From Vudyog..ModuleMast a, Vudyog..co_Mast b Where a.Enc1=@pEnc and b.enddir='' and Convert(Varchar(max),b.ProdCode)=@ProdCode ";          //Added by Shrikant S. on 02/02/2019 for Registration

            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sqlStr, conn);
            cmd.Parameters.Add(new SqlParameter("@ProdCode", oConnect.RetAppEnc(this.CurrentAppFile)));
            da.SelectCommand = cmd;
            //Added by Shrikant S. on 02/04/2019 for Registration       //Start
            if (this.CurrentAppFile != _UpgradeAppFile)
            {
                string sqlSelect = "Select Top 1 UpgradeTo From Vudyog..ApplicationDet Where AppName='" + _UpgradeAppFile + "'";
                SqlConnection con = new SqlConnection(readIni.ConnectionString);
                SqlCommand sqlcmd = new SqlCommand(sqlSelect, con);
                con.Open();
                string retvalue = Convert.ToString(sqlcmd.ExecuteScalar());
                con.Close();
                cmd.Parameters.Add("@pEnc", SqlDbType.VarChar).Value = VU_UDFS.NewENCRY(VU_UDFS.enc(VU_UDFS.NewDECRY(VU_UDFS.HexDecimalToASCII(retvalue), "Ud*yog+1993Prod")), "Ud*yog+1993");
            }
            else
            {
                //Added by Shrikant S. on 02/04/2019 for Registration       //End
                cmd.Parameters.Add("@pEnc", SqlDbType.VarChar).Value = oConnect.RetAppEnc(this.CurrentAppFile);       //Commented by Shrikant S. on 02/04/2019 for Registration
            }                                                       //Added by Shrikant S. on 02/04/2019 for Registration       

            da.Fill(ds, "Product_vw");
            for (int i = 0; i < ds.Tables["Product_vw"].Rows.Count; i++)
            {
                ds.Tables["Product_vw"].Rows[i]["Evalue"] = oConnect.RetModuleDec(ds.Tables["Product_vw"].Rows[i]["Enc2"].ToString());

                //for (int j = 1; j <= 5; j++)          //Commented by Shrikant S. on 04/02/2019 for Registration
                for (int j = 1; j <= 7; j++)            //Added by Shrikant S. on 04/02/2019 for Registration    
                {
                    int firstKeyIndex = ds.Tables["Product_vw"].Rows[i]["Evalue"].ToString().IndexOf("~*" + j.ToString() + "*~");

                    if (firstKeyIndex < 0)                  //Added by Shrikant S. on 19/06/2019 for Ugst to ERPENT
                        continue;

                    int secondKeyIndex = ds.Tables["Product_vw"].Rows[i]["Evalue"].ToString().IndexOf("~*" + j.ToString() + "*~", firstKeyIndex + 1);
                    string lstring = ds.Tables["Product_vw"].Rows[i]["Evalue"].ToString().Substring(firstKeyIndex + 5, secondKeyIndex - firstKeyIndex - 5);
                    switch (j)
                    {
                        case 1:
                            ds.Tables["Product_vw"].Rows[i]["cProdCode"] = lstring.Trim();
                            break;
                        case 2:
                            ds.Tables["Product_vw"].Rows[i]["cProdName"] = lstring.Trim();

                            break;
                        case 3:
                            ds.Tables["Product_vw"].Rows[i]["cModDep"] = lstring.Trim();
                            break;
                        case 4:
                            switch (ds.Tables["Product_vw"].Rows[i]["cProdCode"].ToString())
                            {
                                case "phrmretlr":
                                    lstring = lstring.Trim() + "," + "eautomob";
                                    break;
                                case "eautomob":
                                    lstring = lstring.Trim() + "," + "phrmretlr";
                                    break;
                                default:
                                    break;
                            }
                            ds.Tables["Product_vw"].Rows[i]["cCmbNotAlwd"] = lstring.Trim();

                            break;
                        case 5:
                            ds.Tables["Product_vw"].Rows[i]["cMainProdCode"] = lstring.Trim();
                            break;
                        //Added by Shrikant S. on 04/02/2019 for Registration       //Start
                        case 6:
                            ds.Tables["Product_vw"].Rows[i]["ModOrder"] = lstring.Trim();
                            break;
                        case 7:
                            ds.Tables["Product_vw"].Rows[i]["isDefault"] = (lstring.Trim().Length <= 0 || lstring.Trim() == ".F." ? false : true);
                            break;
                            //Added by Shrikant S. on 04/02/2019 for Registration       //End
                    }
                }
            }
        }
        private void GetCompanyProducts()
        {

            DataTable dtcProducts = new DataTable();
            dtcProducts.TableName = "cProduct_vw";
            ds.Tables.Add(dtcProducts);
            DataColumn sel = dtcProducts.Columns.Add("Sel", typeof(System.Boolean));
            DataColumn dccompId = dtcProducts.Columns.Add("CompId", typeof(int));
            DataColumn dcProduct = dtcProducts.Columns.Add("CProduct", typeof(System.String));
            DataColumn dcProductName = dtcProducts.Columns.Add("CProductName", typeof(System.String));
            DataRow dr;

            string cProduct = string.Empty;
            string cprod = string.Empty;
            string productName = string.Empty;

            for (int i = 0; i < ds.Tables["Co_mast_vw"].Rows.Count; i++)
            {
                if (!vu10Prod.Contains(this.CurrentAppFile))
                {
                    cProduct = Utilities.GetDecProductCode(ds.Tables["Co_mast_vw"].Rows[i]["Passroute"].ToString());
                    int j = 0;
                    //while (j + 5 <= cProduct.Length)          //Commented by Shrikant S. on 13/02/2019 for Registration
                    for (int d = 0; d < ds.Tables["Product_vw"].Rows.Count; d++)        //Added by Shrikant S. on 13/02/2019 for Registration
                    {
                        if (ds.Tables["Product_vw"].Rows[d]["compid"].ToString().Trim() == ds.Tables["Co_mast_vw"].Rows[i]["compid"].ToString().Trim())
                        {
                            if (cProduct.Contains(ds.Tables["Product_vw"].Rows[d]["cMainProdCode"].ToString().Trim()))
                            {
                                //cprod = cProduct.Substring(j, 5);
                                cprod = ds.Tables["Product_vw"].Rows[d]["cMainProdCode"].ToString().Trim();
                                dr = dtcProducts.NewRow();
                                dr["Sel"] = true;
                                dr["Compid"] = ds.Tables["Co_mast_vw"].Rows[i]["CompId"];
                                dr["cProduct"] = cprod;
                                //switch (cprod)
                                //{

                                //    case "vuent":
                                //        productName = "Financial A/c";
                                //        break;
                                //    case "vupro":
                                //        productName = "Vat";
                                //        break;
                                //    case "vuexc":
                                //        productName = "Excise Manufacturing";
                                //        break;
                                //    case "vuexp":
                                //        productName = "Export";
                                //        break;
                                //    case "vuinv":
                                //        productName = "Inventory";
                                //        break;
                                //    case "vuord":
                                //        productName = "Order Processing";
                                //        break;
                                //    case "vubil":
                                //        productName = "Special Billing";
                                //        break;
                                //    case "vutex":
                                //        productName = "Excise Trading";
                                //        break;
                                //    case "vuser":
                                //        productName = "Service Tax";
                                //        break;
                                //    case "vuisd":
                                //        productName = "Input Service Distributor";
                                //        break;
                                //    case "vumcu":
                                //        productName = "Multi-Currency";
                                //        break;
                                //    case "vutds":
                                //        productName = "TDS";
                                //        break;
                                //}
                                dr["cProductName"] = ds.Tables["Product_vw"].Rows[d]["cProdName"].ToString().Trim();
                                dtcProducts.Rows.Add(dr);
                            }
                        }
                    }
                }
                else
                {
                    cProduct = Utilities.GetDecProductCode(ds.Tables["Co_mast_vw"].Rows[i]["Passroute1"].ToString());
                    //cProduct = Utilities.GetDecProductCode(ds.Tables["Co_mast_vw"].Rows[i]["Passroute"].ToString() + (ds.Tables["Co_mast_vw"].Rows[i]["Passroute1"].ToString().Length>0?"," :"") + ds.Tables["Co_mast_vw"].Rows[i]["Passroute1"].ToString());

                    if (cProduct.Trim().Length != 0)
                    {
                        string[] prod = cProduct.Split(',');
                        foreach (string xprod in prod)
                        {
                            string ProdDesc = string.Empty;
                            ProdDesc = GetProductDescription(xprod);
                            if (ProdDesc.Length != 0)
                            {
                                dr = dtcProducts.NewRow();
                                dr["Sel"] = true;
                                dr["Compid"] = ds.Tables["Co_mast_vw"].Rows[i]["CompId"];
                                dr["cProduct"] = xprod;
                                dr["cProductName"] = GetProductDescription(xprod);
                                dtcProducts.Rows.Add(dr);
                            }
                        }

                    }
                }
            }


        }
        private string GenerateProductXml()
        {
            DataView dv = new DataView();
            string filename = txtCo_name.Text.Trim() + "-" + txtPartnerCode.Text.Trim() + "-UpgradeProducts.xml";

            using (XmlWriter writer = XmlWriter.Create(filename))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("companydata");
                writer.WriteElementString("upgradefrom", CurrentApplication);
                writer.WriteElementString("upgradeto", UpgradeApplication);
                for (int i = 0; i < UpgradecompList.Rows.Count; i++)
                {
                    writer.WriteStartElement("company" + (i + 1).ToString());
                    writer.WriteElementString("name", UpgradecompList.Rows[i]["Co_name"].ToString().Trim());
                    writer.WriteStartElement("products");
                    dv = ds.Tables["Product_vw"].DefaultView;
                    dv.RowFilter = "Sel=1 and CompId=" + UpgradecompList.Rows[i]["CompId"].ToString().Trim();
                    for (int j = 0; j < dv.Count; j++)
                    {
                        DataRow[] drs = ds.Tables["cProduct_vw"].Select("CompId=" + UpgradecompList.Rows[i]["CompId"].ToString().Trim() + " and cProduct='" + dv[j]["cProdCode"].ToString().Trim() + "'");
                        if (drs.Length == 0)
                            writer.WriteElementString("addproduct", dv[j]["cProdName"].ToString().Trim());
                        else
                            writer.WriteElementString("product", dv[j]["cProdName"].ToString().Trim());
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                btnFinish.Enabled = false;
                btnBack.Enabled = false;
                //MessageBox.Show("Xml generated successfully....", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.Close();
            }
            return filename;
        }
        private bool CheckRegisterMe()
        {
            DirectoryInfo dir = new DirectoryInfo(appPath);
            //Array totalFile = dir.GetFiles("*register.Me");

            System.Linq.IOrderedEnumerable<FileInfo> totalFile = (from f in dir.GetFiles("*register.Me") orderby f.LastWriteTime descending select f);

            string m_registerMePath = string.Empty;
            if (totalFile.Count() > 1)
            {
                for (int i = 0; i < totalFile.Count(); i++)
                {
                    m_registerMePath = Path.GetFileName(totalFile.ElementAt(i).ToString());
                    this.ReadRegisterMe(m_registerMePath);
                    if (!this.CheckRegisterMeValidation())
                    {
                        //string renamefile = Path.GetFileNameWithoutExtension(totalFile.ElementAt(i).ToString()) + "_" + regshortcode.Substring(3, 3)+ DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".Me";
                        //File.Move(m_registerMePath, renamefile);
                        stringList.Clear();
                        m_registerMePath = string.Empty;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int j = 0; j < totalFile.Count(); j++)
                {
                    //Added by Shrikant S. on 05/04/2019 for Registration           //Start
                    m_registerMePath = Path.GetFileName(totalFile.ElementAt(j).ToString());
                    _CheckRegisterMeFileExists = true;
                    this.ReadRegisterMe(m_registerMePath);
                    break;
                    //Added by Shrikant S. on 05/04/2019 for Registration           //End

                    //Commented by Shrikant S. on 05/04/2019 for Registration           //Start
                    //if (Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name).ToUpper().Contains("REGISTER.ME"))
                    //{
                    //    m_registerMePath = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                    //    _CheckRegisterMeFileExists = true;              //Added by Shrikant S. on 07/05/2018 for Bug-31515
                    //    break;
                    //}
                    //Commented by Shrikant S. on 05/04/2019 for Registration           //End

                }

            }
            if (m_registerMePath == string.Empty)
            {
                ErrorMsg = "Regiser.me file not found.";
                return false;
            }
            return true;
        }
        private void GenerateDefaultValue()
        {
            txtDBIP.Text = GetMachineDetails.IpAddress();
            txtDBName.Text = GetMachineDetails.MachineName();
            txtAppIP.Text = txtDBIP.Text;
            txtAppName.Text = txtDBName.Text;
            txtProductVer.Text = this.UpgradeAppFile;
            MacId = Utilities.ReverseString(GetMachineDetails.ProcessorId().Trim());
            if (this.CurrentAppFile == "VudyogSDK" || this.CurrentAppFile == "UdyogERPSDK")
            {
                cboServiceVer.Items.Add("Developer Version");
            }
            else
            {
                cboServiceVer.Items.Add("Client Version");
                cboServiceVer.Items.Add("Support Version");
                cboServiceVer.Items.Add("Marketing Version");
                cboServiceVer.Items.Add("Educational Version");
                cboServiceVer.Items.Add("Developer Version");
            }
            txtNoComp.Text = ds.Tables["Co_mast_vw"].Rows.Count.ToString();
            cboServiceVer.SelectedIndex = 0;
            if (cboServiceVer.Text != "Client Version")
            {
                txtUsers.Text = "1";
                txtNoComp.Text = "1";
            }
        }
        private bool CheckValidation()
        {
            if (tabControl1.TabPages.Count == 4)
            {
                if (tabControl1.TabPages.Contains(this.tabPage4))           // Added by Shrikant S. on 04/11/2019 for Bug-32956
                {
                    if (txtCo_name.TextLength == 0)
                    {
                        MessageBox.Show("Please select the main company.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCo_name.Focus();
                        return false;
                    }

                    for (int i = 0; i < UpgradecompList.Rows.Count; i++)
                    {
                        if (UpgradecompList.Rows[i]["co_name"].ToString().Trim().Length <= 0)
                        {
                            MessageBox.Show("Company name cannot be empty.", vuMess, MessageBoxButtons.OK);
                            return false;
                        }
                    }
                    //Commented by Shrikant S. on 23/07/2019 for Registration   //Start
                    ////Added by Shrikant S. on 02/05/2019 for Registration       //Start
                    //if (txtPartnerCode.TextLength == 0)
                    //{
                    //    this.txtPartnerCode.Text = "UM0063";
                    //}
                    ////Added by Shrikant S. on 02/05/2019 for Registration       //End
                    //Commented by Shrikant S. on 23/07/2019 for Registration   //End

                    //UnCommented by Shrikant S. on 23/07/2019 for Registration   //Start
                    //Commented by Shrikant S. on 14/02/2018 for Registration       //Start
                    if (txtPartnerCode.TextLength == 0)
                    {
                        MessageBox.Show("Please fill partner code.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtPartnerCode.Focus();
                        return false;
                    }
                    if (txtUsers.TextLength == 0)
                    {
                        MessageBox.Show("Please fill no. of users.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtUsers.Focus();
                        return false;
                    }
                    //Commented by Shrikant S. on 14/02/2018 for Registration       //End
                    //UnCommented by Shrikant S. on 23/07/2019 for Registration   //End

                    if (txtEmail.Text.Length != 0)
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text.Trim(), "^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$"))
                        {
                            MessageBox.Show("Email field is not in correct format!", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtEmail.Focus();
                            return false;
                        }
                    }

                    if (txtNoComp.TextLength == 0)
                    {
                        MessageBox.Show("Please fill no. of companies.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtNoComp.Focus();
                        return false;
                    }

                }
            }

            //Added by Shrikant S. on 18/02/2019 for Registration           // Start
            if (this.Contains(this.tabPage5))
            {
                // Added by Shrikant S. on 26/04/2019 for Registration      //Start
                if (this.cboServiceVer.Text.Trim().ToUpper() == "SUPPORT VERSION" || this.cboServiceVer.Text.Trim().ToUpper() == "MARKETING VERSION" || this.cboServiceVer.Text.Trim().ToUpper() == "DEVELOPER VERSION" || this.cboServiceVer.Text.Trim().ToUpper() == "EDUCATIONAL VERSION")
                {
                    regicompcnt = 1;
                    regiusercnt = 1;
                    totcompcnt = 1;
                    totusercnt = 1;
                }
                else
                {
                    regicompcnt = int.Parse(this.txtregicompcnt.Text);
                    regiusercnt = int.Parse(this.txtregiusercnt.Text);
                    totcompcnt = int.Parse(this.txttotcompcnt.Text);
                    totusercnt = int.Parse(this.txttotusercnt.Text);
                }
                // Added by Shrikant S. on 26/04/2019 for Registration      //End

                //addicompcnt = int.Parse(this.txtaddicompcnt.Text);
                addicompcnt = Convert.ToInt32(UpgradecompList.Compute("Count(Compid)", "CompId < 0"));
                addiusercnt = int.Parse(this.txtaddiusercnt.Text);
            }
            //Added by Shrikant S. on 18/02/2019 for Registration           // End
            return true;
        }
        //Added by Shrikant S. on 02/02/2019 for Registration       //Start
        private int Update_manufact()
        {
            int retvalue = 1;
            DataRow[] comprow = ds.Tables["Co_mast_vw"].Select("Co_name='" + stringList[0].ToString().ToUpper() + "'");
            if (comprow.Count() > 0)
            {
                string sqlStr = "Update " + comprow[0]["Dbname"].ToString().Trim() + "..manufact set constbusi='" + this.txtnatbusi.Text.Trim() + "',prodmfg='" + this.txtprodmfg.Text.Trim() + "' ";
                SqlConnection con = new SqlConnection(readIni.ConnectionString);
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                this.ConnOpen();
                retvalue = Convert.ToInt32(cmd.ExecuteNonQuery());
                this.ConnClose();
            }
            return retvalue;
        }
        private void ReadAdditionalModuleFromDatabase(byte[] reqBufferData, byte[] reqBufferUpgradeData)
        {
            XmlDocument doc = new XmlDocument();
            MemoryStream ms = new MemoryStream(reqBufferData, 0, reqBufferData.Length);
            doc.Load(ms);

            foreach (XmlNode node in doc.SelectNodes("/VFPData/curtemp/modules"))
            {
                string companyname = string.Empty;
                string prodlist = string.Empty;

                if (node.HasChildNodes)
                {
                    foreach (XmlNode compnode in node)
                    {
                        companyname = compnode.Attributes[0].Value;

                        if (compnode.HasChildNodes)
                        {
                            foreach (XmlNode modulenode in compnode)
                            {
                                prodlist = prodlist + modulenode.InnerText + ",";
                            }
                            DataRow row = Complist.NewRow();
                            row["compname"] = companyname;
                            row["addimodlist"] = prodlist;
                            Complist.Rows.Add(row);
                            prodlist = string.Empty;
                        }
                    }
                    //break;
                }

            }
            doc = new XmlDocument();
            ms = new MemoryStream(reqBufferUpgradeData, 0, reqBufferUpgradeData.Length);
            doc.Load(ms);

            int compnodecount = 1;
            foreach (XmlNode node in doc.SelectNodes("/companydata"))
            {
                string companyname = string.Empty;
                string prodlist = string.Empty;

                if (node.HasChildNodes)
                {
                    foreach (XmlNode compnode in node)
                    {

                        if (compnode.Name == "company" + compnodecount.ToString())
                        {
                            if (compnode.HasChildNodes)
                            {
                                foreach (XmlNode comp in compnode)
                                {
                                    if (comp.Name == "name")
                                    {
                                        companyname = comp.InnerText;
                                        compnodecount++;
                                    }

                                    if (comp.Name == "products")
                                    {
                                        foreach (XmlNode modulenode in comp)
                                        {
                                            prodlist = prodlist + modulenode.InnerText + ",";
                                        }
                                    }
                                }


                            }
                            string filt = string.Format("compname='{0}'", companyname);
                            DataRow[] compRow = Complist.Select(filt);
                            if (compRow.Count() > 0)
                            {
                                compRow[0]["selmodlist"] = prodlist;
                            }
                            else
                            {
                                DataRow row = Complist.NewRow();
                                row["compname"] = companyname;
                                row["addimodlist"] = string.Empty;
                                row["selmodlist"] = prodlist;
                                Complist.Rows.Add(row);
                            }
                            prodlist = string.Empty;
                        }
                    }
                    //break;
                }
            }
            Complist.AcceptChanges();
        }
        private void GetModules()
        {
            conn = new SqlConnection(readIni.ConnectionString);
            
            string sqlStr = string.Empty;
            //sqlStr = "Select Sel=convert(bit,0),a.Enc1,a.Enc2,EValue=space(2000),cProdName=space(100),cProdCode=space(10),cCmbNotAlwd=space(250),cModDep=space(250),cMainProdCode=space(250),nProdType =convert(bit,0),b.CompId,ModOrder=Space(100),isDefault=convert(Bit,0)  From Vudyog..ModuleMast a, Vudyog..co_Mast b Where a.Enc1=@pEnc and b.enddir='' ";
            sqlStr = "Select Sel=convert(bit,0),a.Enc1,a.Enc2,EValue=space(2000),cProdName=space(100),cProdCode=space(10),cCmbNotAlwd=space(250),cModDep=space(250),cMainProdCode=space(250),nProdType =convert(bit,0),CompId=0,ModOrder=Space(100),isDefault=convert(Bit,0)  From Vudyog..ModuleMast a Where a.Enc1=@pEnc ";

            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sqlStr, conn);
            da.SelectCommand = cmd;

            //cmd.Parameters.Add("@pEnc", SqlDbType.VarChar).Value = oConnect.RetAppEnc(this.CurrentAppFile);
            if (this.CurrentAppFile != _UpgradeAppFile)
                cmd.Parameters.Add("@pEnc", SqlDbType.VarChar).Value = VU_UDFS.NewENCRY(VU_UDFS.enc(upgradeProdcode), "Ud*yog+1993");
            else
                cmd.Parameters.Add("@pEnc", SqlDbType.VarChar).Value = oConnect.RetAppEnc(this.CurrentAppFile);


            da.Fill(ds, "Module_vw");
            for (int i = 0; i < ds.Tables["Module_vw"].Rows.Count; i++)
            {
                ds.Tables["Module_vw"].Rows[i]["Evalue"] = oConnect.RetModuleDec(ds.Tables["Module_vw"].Rows[i]["Enc2"].ToString());

                for (int j = 1; j <= 7; j++)
                {
                    int firstKeyIndex = ds.Tables["Module_vw"].Rows[i]["Evalue"].ToString().IndexOf("~*" + j.ToString() + "*~");

                    if (firstKeyIndex < 0)                  //Added by Shrikant S. on 19/06/2019 for ugst to uerpent 
                        continue;

                    int secondKeyIndex = ds.Tables["Module_vw"].Rows[i]["Evalue"].ToString().IndexOf("~*" + j.ToString() + "*~", firstKeyIndex + 1);
                    string lstring = ds.Tables["Module_vw"].Rows[i]["Evalue"].ToString().Substring(firstKeyIndex + 5, secondKeyIndex - firstKeyIndex - 5);
                    switch (j)
                    {
                        case 1:
                            ds.Tables["Module_vw"].Rows[i]["cProdCode"] = lstring.Trim();
                            break;
                        case 2:
                            ds.Tables["Module_vw"].Rows[i]["cProdName"] = lstring.Trim();
                            break;
                        case 3:
                            ds.Tables["Module_vw"].Rows[i]["cModDep"] = lstring.Trim();
                            break;
                        case 4:
                            ds.Tables["Module_vw"].Rows[i]["cCmbNotAlwd"] = lstring.Trim();
                            break;
                        case 5:
                            ds.Tables["Module_vw"].Rows[i]["cMainProdCode"] = lstring.Trim();
                            break;
                        case 6:
                            ds.Tables["Module_vw"].Rows[i]["ModOrder"] = lstring.Trim();
                            break;
                        case 7:
                            ds.Tables["Module_vw"].Rows[i]["isDefault"] = (lstring.Trim().Length <= 0 || lstring.Trim() == ".F." ? false : true);
                            break;
                    }
                }
            }
        }
        private void GetAdditionalModules()
        {
            this.GetModules();
            int RowId = 0;
            for (int i = 0; i < UpgradecompList.Rows.Count; i++)
            {
                if (UpgradecompList.Rows[i]["Co_name"].ToString().Trim().ToUpper() == stringList[0].ToString().ToUpper())
                {
                    string selectedModules = Utilities.GetDecProductCode(UpgradecompList.Rows[i]["Passroute"].ToString()) + Utilities.GetDecProductCode(UpgradecompList.Rows[i]["Passroute1"].ToString());
                    DataRow[] ModuleRows = ds.Tables["Module_vw"].Select("isDefault=False");
                    for (int k = 0; k < ModuleRows.Count(); k++)
                    {
                        bool _cProdCodeExists = selectedModules.Contains(ModuleRows[k]["cProdCode"].ToString().Trim());
                        //if (_cProdCodeExists && Convert.ToBoolean(ModuleRows[k]["isDefault"]) == false)
                        if (_cProdCodeExists)
                        {
                            RowId++;
                            DataRow row = AddiModule.NewRow();
                            row["Company"] = UpgradecompList.Rows[i]["Co_name"].ToString().Trim();
                            row["AddiMod"] = ModuleRows[k]["cProdName"].ToString().Trim();
                            row["RowId"] = RowId;
                            AddiModule.Rows.Add(row);
                        }
                    }
                    ModuleRows = ds.Tables["Product_vw"].Select("CompId=" + UpgradecompList.Rows[i]["Compid"].ToString().Trim() + " and isDefault=False and Sel=True");
                    for (int k = 0; k < ModuleRows.Count(); k++)
                    {
                        string filtcond = string.Format("Company='{0}' and AddiMod='{1}'", this.EscapeSpecialCharacters(ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim()), this.EscapeSpecialCharacters(ModuleRows[k]["cProdName"].ToString().Trim()));
                        int moduleCount = Convert.ToInt32(AddiModule.Compute("Count(Company)", filtcond));
                        if (moduleCount <= 0)
                        {
                            RowId++;
                            DataRow row = AddiModule.NewRow();
                            row["Company"] = ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim();
                            row["AddiMod"] = ModuleRows[k]["cProdName"].ToString().Trim();
                            row["RowId"] = RowId;
                            AddiModule.Rows.Add(row);
                        }
                    }
                    ////Added by Shrikant S. on 02/05/2019 for Registration       //Start
                    //string filter = string.Format("Company='{0}'", this.EscapeSpecialCharacters(ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim()));
                    //int cmpcount= Convert.ToInt32(AddiModule.Compute("Count(Company)", filter));
                    //if (cmpcount <= 0)
                    //{
                    //    RowId++;
                    //    DataRow row = AddiModule.NewRow();
                    //    row["Company"] = ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim();
                    //    row["AddiMod"] = string.Empty;
                    //    row["RowId"] = RowId;
                    //    AddiModule.Rows.Add(row);
                    //}
                    ////Added by Shrikant S. on 02/05/2019 for Registration       //End

                }
            }
            for (int i = 0; i < UpgradecompList.Rows.Count; i++)
            {
                if (UpgradecompList.Rows[i]["Co_name"].ToString().Trim().ToUpper() != stringList[0].ToString().ToUpper())
                {
                    string selectedModules = Utilities.GetDecProductCode(UpgradecompList.Rows[i]["Passroute"].ToString().Trim()) + Utilities.GetDecProductCode(UpgradecompList.Rows[i]["Passroute1"].ToString().Trim());
                    DataRow[] ModuleRows = ds.Tables["Module_vw"].Select("isDefault=False");
                    for (int k = 0; k < ModuleRows.Count(); k++)
                    {
                        bool _cProdCodeExists = selectedModules.Contains(ModuleRows[k]["cProdCode"].ToString().Trim());
                        //if (_cProdCodeExists && Convert.ToBoolean(ModuleRows[k]["isDefault"]) == false)
                        if (_cProdCodeExists)
                        {
                            RowId++;
                            DataRow row = AddiModule.NewRow();
                            row["Company"] = UpgradecompList.Rows[i]["Co_name"].ToString().Trim();
                            row["AddiMod"] = ModuleRows[k]["cProdName"].ToString().Trim();
                            row["RowId"] = RowId;
                            AddiModule.Rows.Add(row);
                        }
                    }
                    ModuleRows = ds.Tables["Product_vw"].Select("CompId=" + UpgradecompList.Rows[i]["Compid"].ToString().Trim() + " and isDefault=False and Sel=True");
                    for (int k = 0; k < ModuleRows.Count(); k++)
                    {
                        string filtcond = string.Format("Company='{0}' and AddiMod='{1}'", this.EscapeSpecialCharacters(UpgradecompList.Rows[i]["Co_name"].ToString().Trim()), this.EscapeSpecialCharacters(ModuleRows[k]["cProdName"].ToString().Trim()));
                        int moduleCount = Convert.ToInt32(AddiModule.Compute("Count(Company)", filtcond));
                        if (moduleCount <= 0)
                        {
                            RowId++;
                            DataRow row = AddiModule.NewRow();
                            row["Company"] = UpgradecompList.Rows[i]["Co_name"].ToString().Trim();
                            row["AddiMod"] = ModuleRows[k]["cProdName"].ToString().Trim();
                            row["RowId"] = RowId;
                            AddiModule.Rows.Add(row);
                        }
                    }

                    ////Added by Shrikant S. on 02/05/2019 for Registration       //Start
                    //string filter = string.Format("Company='{0}'", this.EscapeSpecialCharacters(UpgradecompList.Rows[i]["Co_name"].ToString().Trim()));
                    //int cmpcount = Convert.ToInt32(AddiModule.Compute("Count(Company)", filter));
                    //if (cmpcount <= 0)
                    //{
                    //    RowId++;
                    //    DataRow row = AddiModule.NewRow();
                    //    row["Company"] = ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim();
                    //    row["AddiMod"] = string.Empty;
                    //    row["RowId"] = RowId;
                    //    AddiModule.Rows.Add(row);
                    //}
                    ////Added by Shrikant S. on 02/05/2019 for Registration       //End
                }
            }
        }

        private void GetAdditionalModules_Old()
        {
            this.GetModules();
            int RowId = 0;
            for (int i = 0; i < ds.Tables["Co_mast_vw"].Rows.Count; i++)
            {
                if (ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim().ToUpper() == stringList[0].ToString().ToUpper())
                {
                    string selectedModules = Utilities.GetDecProductCode(ds.Tables["Co_mast_vw"].Rows[i]["Passroute"].ToString()) + Utilities.GetDecProductCode(ds.Tables["Co_mast_vw"].Rows[i]["Passroute1"].ToString());
                    DataRow[] ModuleRows = ds.Tables["Module_vw"].Select("isDefault=False");
                    for (int k = 0; k < ModuleRows.Count(); k++)
                    {
                        bool _cProdCodeExists = selectedModules.Contains(ModuleRows[k]["cProdCode"].ToString().Trim());
                        //if (_cProdCodeExists && Convert.ToBoolean(ModuleRows[k]["isDefault"]) == false)
                        if (_cProdCodeExists)
                        {
                            RowId++;
                            DataRow row = AddiModule.NewRow();
                            row["Company"] = ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim();
                            row["AddiMod"] = ModuleRows[k]["cProdName"].ToString().Trim();
                            row["RowId"] = RowId;
                            AddiModule.Rows.Add(row);
                        }
                    }
                    ModuleRows = ds.Tables["Product_vw"].Select("CompId=" + ds.Tables["Co_mast_vw"].Rows[i]["Compid"].ToString().Trim() + " and isDefault=False and Sel=True");
                    for (int k = 0; k < ModuleRows.Count(); k++)
                    {
                        string filtcond = string.Format("Company='{0}' and AddiMod='{1}'", this.EscapeSpecialCharacters(ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim()), this.EscapeSpecialCharacters(ModuleRows[k]["cProdName"].ToString().Trim()));
                        int moduleCount = Convert.ToInt32(AddiModule.Compute("Count(Company)", filtcond));
                        if (moduleCount <= 0)
                        {
                            RowId++;
                            DataRow row = AddiModule.NewRow();
                            row["Company"] = ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim();
                            row["AddiMod"] = ModuleRows[k]["cProdName"].ToString().Trim();
                            row["RowId"] = RowId;
                            AddiModule.Rows.Add(row);
                        }
                    }
                }
            }
            for (int i = 0; i < ds.Tables["Co_mast_vw"].Rows.Count; i++)
            {
                if (ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim().ToUpper() != stringList[0].ToString().ToUpper())
                {
                    string selectedModules = Utilities.GetDecProductCode(ds.Tables["Co_mast_vw"].Rows[i]["Passroute"].ToString().Trim()) + Utilities.GetDecProductCode(ds.Tables["Co_mast_vw"].Rows[i]["Passroute1"].ToString().Trim());
                    DataRow[] ModuleRows = ds.Tables["Module_vw"].Select("isDefault=False");
                    for (int k = 0; k < ModuleRows.Count(); k++)
                    {
                        bool _cProdCodeExists = selectedModules.Contains(ModuleRows[k]["cProdCode"].ToString().Trim());
                        //if (_cProdCodeExists && Convert.ToBoolean(ModuleRows[k]["isDefault"]) == false)
                        if (_cProdCodeExists)
                        {
                            RowId++;
                            DataRow row = AddiModule.NewRow();
                            row["Company"] = ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim();
                            row["AddiMod"] = ModuleRows[k]["cProdName"].ToString().Trim();
                            row["RowId"] = RowId;
                            AddiModule.Rows.Add(row);
                        }
                    }
                    ModuleRows = ds.Tables["Product_vw"].Select("CompId=" + ds.Tables["Co_mast_vw"].Rows[i]["Compid"].ToString().Trim() + " and isDefault=False and Sel=True");
                    for (int k = 0; k < ModuleRows.Count(); k++)
                    {
                        string filtcond = string.Format("Company='{0}' and AddiMod='{1}'", this.EscapeSpecialCharacters(ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim()), this.EscapeSpecialCharacters(ModuleRows[k]["cProdName"].ToString().Trim()));
                        int moduleCount = Convert.ToInt32(AddiModule.Compute("Count(Company)", filtcond));
                        if (moduleCount <= 0)
                        {
                            RowId++;
                            DataRow row = AddiModule.NewRow();
                            row["Company"] = ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim();
                            row["AddiMod"] = ModuleRows[k]["cProdName"].ToString().Trim();
                            row["RowId"] = RowId;
                            AddiModule.Rows.Add(row);
                        }
                    }
                }
            }
        }
        //Added by Shrikant S. on 02/02/2019 for Registration       //End
        private string GenerateCompanyXml()
        {
            string filename = string.Empty;
            string setvalue = string.Empty;
            filename = txtCo_name.Text.Trim() + "-" + txtPartnerCode.Text.Trim() + ".xml";
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.GetEncoding(1252),
                OmitXmlDeclaration = false
            };

            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("VFPData");
                writer.WriteStartElement("curtemp");
                writer.WriteElementString("co_name", stringList[0].ToString());
                writer.WriteElementString("add1", stringList[1].ToString());
                writer.WriteElementString("add2", stringList[2].ToString());
                writer.WriteElementString("add3", stringList[3].ToString());
                writer.WriteElementString("area", stringList[4].ToString());
                writer.WriteElementString("zone", stringList[5].ToString());
                writer.WriteElementString("city", stringList[6].ToString());
                writer.WriteElementString("zip", stringList[7].ToString());

                writer.WriteElementString("state", stringList[8].ToString());
                writer.WriteElementString("country", stringList[9].ToString());

                //Commented by Shrikant S. on 23/07/2019 for Old Registration       //Start
                ////Added by Shrikant S. on 30/04/2019 for Registration       // Start
                //this.Invoke(new Action(() => setvalue = this.txtgstin.Text));
                //writer.WriteElementString("gstin", setvalue);

                //this.Invoke(new Action(() => setvalue = this.txtgstreg.Text));
                //writer.WriteElementString("gstreg", setvalue);
                ////Added by Shrikant S. on 30/04/2019 for Registration       // End
                //Commented by Shrikant S. on 23/07/2019 for Old Registration       //End


                writer.WriteElementString("phone", stringList[13].ToString());

                this.Invoke(new Action(() => setvalue = cboServiceVer.Text));
                writer.WriteElementString("servicetype", setvalue);

                this.Invoke(new Action(() => setvalue = txtUsers.Text.Trim()));
                setvalue = (totusercnt > 0 ? totusercnt.ToString() : setvalue);
                writer.WriteElementString("noofuser", setvalue);

                this.Invoke(new Action(() => setvalue = txtNoComp.Text.Trim()));
                setvalue = (totcompcnt > 0 ? totcompcnt.ToString() : setvalue);
                writer.WriteElementString("noofco", setvalue);


                this.Invoke(new Action(() => setvalue = txtContact.Text.Trim()));
                writer.WriteElementString("contact", setvalue);

                this.Invoke(new Action(() => setvalue = txtEmail.Text.Trim()));
                writer.WriteElementString("email", setvalue);

                writer.WriteElementString("product", oConnect.RetProduct().Trim().Replace(",", ""));
                var versionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(appPath, this.CurrentAppFile + ".exe"));
                string version = versionInfo.FileVersion;
                version = version + (version.Count(x => x == '.') == 2 ? ".0" : "");
                writer.WriteElementString("version", version);

                int j = 1;

                //Commented by Shrikant S. on 27/04/2019 for Registration       //Start
                //for (int i = 0; i < ds.Tables["Co_mast_vw"].Rows.Count; i++)
                //{
                //    if (ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim().ToUpper() != stringList[0].ToString().ToUpper())
                //    {
                //        writer.WriteElementString("company" + (j++).ToString(), ds.Tables["Co_mast_vw"].Rows[i]["Co_name"].ToString().Trim());
                //    }
                //}
                //Commented by Shrikant S. on 27/04/2019 for Registration       //End

                //Commented by Shrikant S. on 27/04/2019 for Registration       //Start
                for (int i = 0; i < UpgradecompList.Rows.Count; i++)
                {
                    if (UpgradecompList.Rows[i]["Co_name"].ToString().Trim().ToUpper() != stringList[0].ToString().ToUpper())
                    {
                        writer.WriteElementString("company" + (j++).ToString(), UpgradecompList.Rows[i]["Co_name"].ToString().Trim());
                    }
                }
                //Commented by Shrikant S. on 27/04/2019 for Registration       //End

                this.Invoke(new Action(() => setvalue = txtPartnerCode.Text.Trim()));
                writer.WriteElementString("prtnrcd", setvalue);
                writer.WriteElementString("macid", MacId);

                this.Invoke(new Action(() => setvalue = txtDBIP.Text.Trim()));
                writer.WriteElementString("dbsrvip", setvalue);

                this.Invoke(new Action(() => setvalue = txtDBName.Text.Trim()));
                writer.WriteElementString("dbsrvnm", setvalue);

                this.Invoke(new Action(() => setvalue = txtAppIP.Text.Trim()));
                writer.WriteElementString("apsrvip", setvalue);

                this.Invoke(new Action(() => setvalue = txtAppName.Text.Trim()));
                writer.WriteElementString("apsrvnm", setvalue);

                writer.WriteElementString("prodversn", oConnect.RetShortCode("UdProdShortCode").Trim());
                writer.WriteElementString("prodsrno", ProdSrno);                                        //Changed by Shrikant S. on 01/04/2019 for Registration
                writer.WriteElementString("lickeyno", LicenseKey);                                      //Changed by Shrikant S. on 01/04/2019 for Registration
                                                                                                        //Added by Shrikant S. on 02/02/2019 for Registration       //Start

                //Commented by Shrikant S. on 23/07/2019 for Old Registration Process       //Start
                //this.Invoke(new Action(() => setvalue = txtnatbusi.Text.Trim()));
                //writer.WriteElementString("natbusi", setvalue);

                //writer.WriteElementString("reg_type", "Upgrade");                 //09/04/2019 
                //writer.WriteElementString("upgradeto", _UpgradeApplication);

                //writer.WriteElementString("partner_ucode", "");
                //writer.WriteElementString("customer_scode", "");
                //this.Invoke(new Action(() => setvalue = txtprodmfg.Text.Trim()));
                //writer.WriteElementString("prodmfg", setvalue);
                //writer.WriteStartElement("modules");


                //DataTable moddet = AddiModule.DefaultView.ToTable(true, "Company");
                //DataRow[] maincompRow = moddet.Select("company='" + stringList[0].ToString().Trim() + "'");
                //int compNo = 0;
                //if (maincompRow.Count() <= 0)
                //{
                //    compNo++;
                //}

                //for (int m = 0; m < moddet.Rows.Count; m++)
                //{
                //    writer.WriteStartElement("company" + (compNo + 1).ToString());
                //    writer.WriteAttributeString("name", moddet.Rows[m]["Company"].ToString());
                //    DataRow[] rows = AddiModule.Select("Company='" + moddet.Rows[m]["Company"].ToString().Trim() + "'");
                //    int k = 0;
                //    foreach (var item in rows)
                //    {
                //        k++;
                //        writer.WriteElementString("m" + k.ToString(), item["AddiMod"].ToString().Trim());
                //    }
                //    writer.WriteEndElement();
                //    compNo++;
                //}


                //writer.WriteEndElement();
                //Commented by Shrikant S. on 23/07/2019 for Old Registration Process       //End



                //Added by Shrikant S. on 02/02/2019 for Registration       //End
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            return filename;
        }


        private void ReadRegisterMe(string fileName)
        {
            string[] objRegisterMe = (oConnect.ReadRegiValue(appPath)).Split('^');
            //Co_Name,Add1,Add2,Add3,area,zone,city,zip,state,country,email
            stringList.Add(objRegisterMe[0].ToString().Trim());
            stringList.Add(objRegisterMe[1].ToString().Trim());
            stringList.Add(objRegisterMe[2].ToString().Trim());
            stringList.Add(objRegisterMe[3].ToString().Trim());
            stringList.Add("");
            stringList.Add("");
            stringList.Add(objRegisterMe[4].ToString().Trim());
            stringList.Add(objRegisterMe[5].ToString().Trim());
            stringList.Add("");
            stringList.Add("");
            stringList.Add(objRegisterMe[6].ToString().Trim());
            stringList.Add("");
            stringList.Add("");
            stringList.Add(objRegisterMe[17].ToString().Trim());

            txtCo_name.Text = objRegisterMe[0].ToString().Trim();
            txtPartnerCode.Text = objRegisterMe[7].ToString().Trim();
            txtDBIP.Text = objRegisterMe[8].ToString().Trim();
            txtDBName.Text = objRegisterMe[9].ToString().Trim();
            txtAppIP.Text = objRegisterMe[10].ToString().Trim();
            txtAppName.Text = objRegisterMe[11].ToString().Trim();
            MacId = Utilities.ReverseString(objRegisterMe[12].ToString());
            txtNoComp.Text = objRegisterMe[13].ToString().Trim();
            txtUsers.Text = objRegisterMe[14].ToString().Trim();
            cboServiceVer.Text = objRegisterMe[15].ToString().Trim();

            ProdSrno = objRegisterMe[18].ToString().Trim();                      //Added by Shrikant S. on 01/04/2019 for Registration
            LicenseKey = objRegisterMe[19].ToString().Trim();                    //Added by Shrikant S. on 01/04/2019 for Registration
            regshortcode = objRegisterMe[16].ToString().Replace(",", "").Trim();

            //Commented by Shrikant S. on 13/02/2019 for Registration       //Start
            //if (objRegisterMe[16].ToString().IndexOf("PRO") > 0)
            //    RegProd = "VudyogPRO";
            //else if (objRegisterMe[16].ToString().IndexOf("STD") > 0)
            //    RegProd = "VudyogSTD";
            //else if (objRegisterMe[16].ToString().IndexOf("ENT") > 0)
            //    RegProd = "VudyogENT";
            //else if (objRegisterMe[16].ToString().IndexOf("SDK") > 0)
            //    RegProd = "VudyogSDK";
            //else if (objRegisterMe[16].ToString().IndexOf("10US") > 0)
            //    RegProd = "10USQUARE";
            //else if (objRegisterMe[16].ToString().IndexOf("10IT") > 0)
            //    RegProd = "10ITAX";
            //else if (objRegisterMe[16].ToString().IndexOf("GST") > 0)       // Added by Shrikant S. on 14/03/2018 
            //    RegProd = "VudyogGST";                                      // Added by Shrikant S. on 14/03/2018  
            //Commented by Shrikant S. on 13/02/2019 for Registration       //End  

            if (totcompcnt == 0)
            {
                this.txtregicomp.Text = objRegisterMe[0].ToString().Trim();
                this.txtregicompcnt.Text = objRegisterMe[13].ToString().Trim();
                this.txtregiusercnt.Text = objRegisterMe[14].ToString().Trim();

                this.txttotcompcnt.Text = objRegisterMe[13].ToString().Trim();
                this.txttotusercnt.Text = objRegisterMe[14].ToString().Trim();


                if (totcompcnt != int.Parse(this.txtNoComp.Text) && totcompcnt > 0)
                {
                    this.txtNoComp.Text = totcompcnt.ToString();
                }
                if (totusercnt != int.Parse(this.txttotusercnt.Text) && totusercnt > 0)
                {
                    this.txtUsers.Text = totusercnt.ToString();
                }
            }
            // Added by Shrikant S. on 26/04/2019 for Registration      //Start
            if (this.cboServiceVer.Text.Trim().ToUpper() == "SUPPORT VERSION" || this.cboServiceVer.Text.Trim().ToUpper() == "MARKETING VERSION" || this.cboServiceVer.Text.Trim().ToUpper() == "DEVELOPER VERSION" || this.cboServiceVer.Text.Trim().ToUpper() == "EDUCATIONAL VERSION")
            {
                this.txtregicompcnt.Text = "Unlimited";
                this.txtregiusercnt.Text = "Unlimited";

                this.txttotcompcnt.Text = "Unlimited";
                this.txttotusercnt.Text = "Unlimited";

                this.txtaddicompcnt.Enabled = false;
                this.txtaddiusercnt.Enabled = false;
            }
            // Added by Shrikant S. on 26/04/2019 for Registration      //End
        }

        private bool ExtractZipFiles()
        {
            if (Directory.Exists(appPath + "\\Database\\"))
            {
                Thread.Sleep(10);
                //pbar.Show();
                //pbar.ShowProgress("Extracting Zip Files...", 0);

                //Added by Shrikant S. on 14/03/2018        //Start
                //UdyoyZipUnZipUtility file = new UdyoyZipUnZipUtility();

                UdyogZipUnzip.UdyoyZipUnZipUtility file = new UdyogZipUnzip.UdyoyZipUnZipUtility();

                //Commented by Shrikant S. on 04/02/2019 for Registration       //Start
                //if (File.Exists(Path.Combine(appPath + "\\Database\\", "GST.Zip")))
                //{
                //    file.UdyogUnzip(Path.Combine(appPath + "\\Database\\", "GST.Zip"), Path.Combine(appPath + "\\Database\\", "GST"), "GST.Dat", "");
                //    if (!RestoreSqlData(appPath + "\\Database\\GST\\", "GST"))
                //    {
                //        ErrorMsg = "Error occured while restoring Database GST";
                //        return false;
                //    }
                //}
                //Commented by Shrikant S. on 04/02/2019 for Registration       //End

                //Added by Shrikant S. on 04/02/2019 for Registration           //Start
                if (Utilities.InList(this.CurrentAppFile, new string[] { "UdyogGST", "UdyogGSTSDK" }))
                {
                    zipFileName = "GST";
                }
                else
                {
                    if (Utilities.InList(this.CurrentAppFile, new string[] { "UdyogERP", "UdyogERPSDK", "UdyogERPSTD", "UdyogERPPRO", "UdyogERPENT", "UdyogERPSDK" }))
                    {
                        zipFileName = "UERP";
                    }
                }
                if (File.Exists(Path.Combine(appPath + "\\Database\\", zipFileName + ".Zip")))
                {
                    file.UdyogUnzip(Path.Combine(appPath + "\\Database\\", zipFileName + ".Zip"), Path.Combine(appPath + "\\Database\\", zipFileName), zipFileName + ".Dat", "");
                    if (!RestoreSqlData(appPath + "\\Database\\" + zipFileName + "\\", zipFileName))
                    {
                        ErrorMsg = "Error occured while restoring Database " + zipFileName;
                        return false;
                    }
                }
                else
                {
                    ErrorMsg = "Zip file '" + zipFileName + "' not found in database folder.";
                    return false;
                }
                //Added by Shrikant S. on 04/02/2019 for Registration           //End


                //Added by Shrikant S. on 14/03/2018        //End
                optionTable = this.GetOptionDetails();      //Added by Shrikant S. on 16/05/2018 for Bug-31515

                //Commented by Shrikant S. on 14/03/2018        //Start
                //if (File.Exists(Path.Combine(appPath + "\\Database\\", "Neio.Zip")))
                //{
                //    file.UdyogUnzip(Path.Combine(appPath + "\\Database\\", "Neio.Zip"), Path.Combine(appPath + "\\Database\\", "Neio"), "Neio.Dat", "");
                //    if (!RestoreSqlData(appPath + "\\Database\\Neio\\", "Neio"))        //Added by Shrikant S. on 26/08/2014 for Bug-23814  
                //    //if (!RestoreSqlData(Path.Combine(appPath + "\\Database\\Neio\\", "Neio.Dat"), "Neio"))    ////Commented by Shrikant S. on 26/08/2014 for Bug-23814
                //    {
                //        ErrorMsg = "Error occured while restoring Database Neio";
                //        //MessageBox.Show("Error occured while restoring Database Neio", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        return false;
                //    }
                //}
                //if (File.Exists(Path.Combine(appPath + "\\Database\\", "Nxio.Zip")))
                //{
                //    file.UdyogUnzip(Path.Combine(appPath + "\\Database\\", "Nxio.Zip"), Path.Combine(appPath + "\\Database\\", "Nxio"), "Nxio.Dat", "");
                //    if (!RestoreSqlData(appPath + "\\Database\\Nxio\\", "Nxio"))             //Added by Shrikant S. on 26/08/2014 for Bug-23814
                //    //if (!RestoreSqlData(Path.Combine(appPath + "\\Database\\Nxio\\", "Nxio.Dat"), "Nxio"))    ////Commented by Shrikant S. on 26/08/2014 for Bug-23814
                //    {
                //        //MessageBox.Show("Error occured while restoring Database Nxio", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        ErrorMsg = "Error occured while restoring Database Nxio";
                //        return false;
                //    }
                //}
                //Commented by Shrikant S. on 14/03/2018        //End
            }
            return true;
        }
        private bool RestoreSqlData(string fileFullPath, string dbName)
        {
            // Commented by Shrikant S. on 26/08/2014 for Bug-23814     //Start
            //try
            //{
            //    conn = new SqlConnection(readIni.ConnectionString);
            //    conn.Open();
            //    string strText = string.Empty;
            //    strText = "If Not Exists( Select [Name] From Master..SysDatabases Where [Name]='" + dbName + "') Begin Create Database " + dbName + "  ";
            //    strText = strText + " Restore Database " + dbName + " From Disk=N'" + fileFullPath + "' With Move '" + dbName + "' To '" + appPath + "\\Database\\" + dbName + ".mdf" + "', Move '" + dbName + "_Log' To '" + appPath + "\\Database\\" + dbName + ".ldf" + "',replace  End";
            //    SqlCommand cmd = new SqlCommand(strText, conn);
            //    cmd.CommandTimeout = 1500;
            //    cmd.ExecuteNonQuery();
            //    conn.Close();
            //}
            // Commented by Shrikant S. on 26/08/2014 for Bug-23814     //End
            // Added by Shrikant S. on 26/08/2014 for Bug-23814     //Start
            try
            {
                string mdfFilenm = string.Empty;
                string ldfFilenm = string.Empty;
                string restorePath = string.Empty;

                conn = new SqlConnection(readIni.ConnectionString);
                conn.Open();
                string strText = string.Empty;
                //strText = "If Not Exists( Select [Name] From Master..SysDatabases Where [Name]='" + dbName + "') Begin Create Database " + dbName + " End  ";     //Commented by Shrikant S. on 04/07/2019
                strText = "Select [Name] as DBNAME From Master..SysDatabases Where [Name]='" + dbName + "'";       //Added by Shrikant S. on 04/07/2019
                SqlCommand cmd = new SqlCommand(strText, conn);
                SqlDataAdapter sqlda = new SqlDataAdapter(cmd);
                DataTable lsqltbl = new DataTable();
                sqlda.Fill(lsqltbl);
                int dbExists = (lsqltbl.Rows.Count > 0 ? 1 : 0);                               //Added by Shrikant S. on 04/07/2019
                //Added by Shrikant S. on 04/07/2019        //Start
                if (dbExists <= 0)
                {
                    strText = "Create Database " + dbName;
                    cmd.CommandText = strText;
                    cmd.ExecuteNonQuery();
                    //Added by Shrikant S. on 04/07/2019        //End

                    restorePath = (readIni.ItaxPath.Length > 0 ? readIni.ItaxPath : fileFullPath);

                    strText = "RESTORE FILELISTONLY FROM DISK = N'" + Path.Combine(restorePath, dbName) + ".Dat'";
                    cmd.CommandText = strText;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sqlda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        mdfFilenm = dt.Rows[0]["LogicalName"].ToString();
                        ldfFilenm = dt.Rows[1]["LogicalName"].ToString();
                    }
                    strText = "RESTORE DATABASE " + dbName + " From Disk=N'" + Path.Combine(restorePath, dbName) + ".Dat' with Move N'" + mdfFilenm + "' To N'" + Path.Combine(restorePath, dbName) + ".mdf',";
                    strText = strText + " " + "Move N'" + ldfFilenm + "' To N'" + Path.Combine(restorePath, dbName) + ".ldf',replace";
                    cmd.CommandText = strText;
                    //strText = strText + " Restore Database " + dbName + " From Disk=N'" + fileFullPath + "' With Move '" + dbName + "' To '" + appPath + "\\Database\\" + dbName + ".mdf" + "', Move '" + dbName + "_Log' To '" + appPath + "\\Database\\" + dbName + ".ldf" + "',replace  End";
                    cmd.CommandTimeout = 1500;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            // Added by Shrikant S. on 26/08/2014 for Bug-23814     //End
            catch (Exception ex)
            {
                conn.Close();
                return false;
            }
            return true;
        }
        private string GetProductInfo(string appName)
        {
            string returnVal = string.Empty;
            string cSql = "Select top 1 *,Upgradeto1=Convert(varchar(100),'') From Vudyog..ApplicationDet Where AppName='" + appName + "'";
            DataTable _appdt = oDataAccess.GetDataTable(cSql, null, 50);
            for (int i = 0; i < _appdt.Rows.Count; i++)
            {
                _appdt.Rows[i]["Upgradeto1"] = VU_UDFS.NewDECRY(VU_UDFS.HexDecimalToASCII(_appdt.Rows[i]["ProdVer"].ToString()), "Ud*yog+1993Prod2");
                returnVal = VU_UDFS.NewDECRY(VU_UDFS.HexDecimalToASCII(_appdt.Rows[i]["ProdVer"].ToString()), "Ud*yog+1993Prod2");
            }
            return returnVal;
            //Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("ueProductUpgrade.Proddetail.xml");
            //DataSet lds = new DataSet();
            //lds.ReadXml(s, XmlReadMode.Auto);
            //return lds.Tables[0];

        }
        private bool WriteConfigFile(string newAppName)
        {
            try
            {
                GetInfo.iniFile ini = new GetInfo.iniFile(Path.Combine(appPath, "Visudyog.ini"));
                ini.IniWriteValue("Settings", "Title", this.lblUpgradeApp1.Text);
                ini.IniWriteValue("Settings", "xfile", newAppName + ".exe");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = "Issue occured while writing the ini file. " + ex.Message;
                return false;
            }
        }
        private void ReadInfFile()
        {
            //this.ShowProcess("Reading Inf File..");
            this.status = "Reading Inf File...";
            this.oWorker.ReportProgress(this.progressPercentage);
            StringReader sr = new StringReader(oConnect.RetIniTable(appPath));
            ds.ReadXml(sr);

            if (_IgnoreStandardRule == true)
            {
                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    DataRow row = ds.Tables["CustInfo_vw"].NewRow();
                    row["clientnm"] = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString();
                    row["macid"] = GetMachineDetails.ProcessorId();
                    row["prodnm"] = this.lblUpgradeApp1.Text;
                    row["mainprodcd"] = _CurrentAppFile;
                    row["prodcd"] = "";
                    row["zip"] = "";
                    row["featureid"] = "";
                    row["vatstates"] = "";
                    row["addprodcd"] = "";
                    ds.Tables["CustInfo_vw"].Rows.Add(row);
                }
            }
        }
        public bool GetFeatures()
        {
            string sqlStr = string.Empty;
            //this.ShowProcess("Genearating Features..");
            this.status = "Please wait... Getting Features..";
            this.oWorker.ReportProgress(this.progressPercentage);
            Thread.Sleep(10);
            try
            {
                ////**** To be Removed -- Start
                //DataTable _dtTemp = new DataTable();
                //sqlStr = "Select [name] from sys.Tables where [name] = 'ProdDetail1'";
                //SqlDataAdapter lda1 = new SqlDataAdapter();
                //SqlCommand cmd2 = new SqlCommand(sqlStr, conn);
                //lda1.SelectCommand = cmd2;
                //lda1.Fill(_dtTemp);

                //if (_dtTemp.Rows.Count <= 0)
                //{
                //    sqlStr = "Select CompId=Convert(int,0), vcEnc=convert(Varchar(5000),'') "
                //    + ",OptionType=Convert(Varchar(254),''),featureid=Convert(Varchar(254),''),sfeatureid=Convert(Varchar(254),'') "
                //    + " ,ProdCode=Convert(Varchar(254),''),prodver=Convert(Varchar(254),''),servicever=Convert(Varchar(254),'') "
                //    + " ,fType=Convert(Varchar(254),''),OptionName=Convert(Varchar(254),''),IntFeatureId=Convert(Int,0) into ProdDetail1 From Vudyog..ProdDetail where 1=2 ";

                //    SqlCommand cmd1 = new SqlCommand(sqlStr, conn);
                //    this.ConnOpen();
                //    int retvalue = Convert.ToInt32(cmd1.ExecuteNonQuery());
                //    this.ConnClose();

                //    //lda.SelectCommand = cmd;
                //    //lda.Fill(ds, "ProdDetail1");

                //    //**** To be Removed -- End

                sqlStr = "Select CompId=Convert(int,0), Enc=Convert(Varchar(254),Enc),PEnc=Convert(Varchar(254),PEnc),vcEnc=convert(Varchar(5000),'') "
                + ",OptionType=Convert(Varchar(254),''),featureid=Convert(Varchar(254),''),sfeatureid=Convert(Varchar(254),'') "
                + " ,ProdCode=Convert(Varchar(254),''),prodver=Convert(Varchar(254),''),servicever=Convert(Varchar(254),'') "
                + " ,fType=Convert(Varchar(254),''),OptionName=Convert(Varchar(254),''),IntFeatureId=Convert(Int,0) From Vudyog..ProdDetail ";
                SqlDataAdapter lda = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                lda.SelectCommand = cmd;
                lda.Fill(ds, "FeatureDet_vw");


                for (int i = 0; i < ds.Tables["FeatureDet_vw"].Rows.Count; i++)
                {
                    ds.Tables["FeatureDet_vw"].Rows[i]["vcEnc"] = oConnect.RetFeatureDec(ds.Tables["FeatureDet_vw"].Rows[i]["Enc"].ToString());
                    for (int j = 0; j < 8; j++)
                    {
                        int firstKeyIndex = (j == 0 ? 0 : ds.Tables["FeatureDet_vw"].Rows[i]["vcEnc"].ToString().IndexOf("~*" + (j - 1).ToString() + "*~"));
                        int secondKeyIndex = ds.Tables["FeatureDet_vw"].Rows[i]["vcEnc"].ToString().IndexOf("~*" + j.ToString() + "*~", firstKeyIndex + 1);
                        string lstring = string.Empty;
                        if (j == 0)
                            lstring = ds.Tables["FeatureDet_vw"].Rows[i]["vcEnc"].ToString().Substring(firstKeyIndex, secondKeyIndex);
                        else
                            lstring = ds.Tables["FeatureDet_vw"].Rows[i]["vcEnc"].ToString().Substring(firstKeyIndex + 5, secondKeyIndex - firstKeyIndex - 5);
                        switch (j)
                        {
                            case 0:
                                ds.Tables["FeatureDet_vw"].Rows[i]["OptionType"] = lstring;
                                break;
                            case 1:
                                ds.Tables["FeatureDet_vw"].Rows[i]["featureid"] = lstring;
                                ds.Tables["FeatureDet_vw"].Rows[i]["IntFeatureId"] = Convert.ToInt32(lstring);
                                break;
                            case 2:
                                ds.Tables["FeatureDet_vw"].Rows[i]["sfeatureid"] = lstring;
                                break;
                            case 3:
                                ds.Tables["FeatureDet_vw"].Rows[i]["ProdCode"] = lstring;
                                break;
                            case 4:
                                ds.Tables["FeatureDet_vw"].Rows[i]["prodver"] = lstring;
                                break;
                            case 5:
                                ds.Tables["FeatureDet_vw"].Rows[i]["servicever"] = lstring;
                                break;
                            case 6:
                                ds.Tables["FeatureDet_vw"].Rows[i]["fType"] = lstring;
                                break;
                            case 7:
                                ds.Tables["FeatureDet_vw"].Rows[i]["OptionName"] = lstring;
                                break;
                        }
                    }
                }
                ////**** To be Removed -- Start
                //sqlStr = "Insert Into ProdDetail1 Values(" + ds.Tables["FeatureDet_vw"].Rows[i]["compid"].ToString() + ",'" + ds.Tables["FeatureDet_vw"].Rows[i]["vcEnc"].ToString().Replace("'", "''") + "','" + ds.Tables["FeatureDet_vw"].Rows[i]["OptionType"].ToString() + "','" + ds.Tables["FeatureDet_vw"].Rows[i]["featureid"].ToString() + "','" + ds.Tables["FeatureDet_vw"].Rows[i]["sfeatureid"].ToString() + "','" + ds.Tables["FeatureDet_vw"].Rows[i]["ProdCode"].ToString() + "','" + ds.Tables["FeatureDet_vw"].Rows[i]["prodver"].ToString() + "','" + ds.Tables["FeatureDet_vw"].Rows[i]["servicever"].ToString() + "','" + ds.Tables["FeatureDet_vw"].Rows[i]["fType"].ToString() + "','" + ds.Tables["FeatureDet_vw"].Rows[i]["OptionName"].ToString().Replace("'", "''") + "'," + ds.Tables["FeatureDet_vw"].Rows[i]["IntFeatureId"].ToString() + ")";
                //        SqlCommand cmd3 = new SqlCommand(sqlStr, conn);
                //        this.ConnOpen();
                //        retvalue = Convert.ToInt32(cmd3.ExecuteNonQuery());
                //        this.ConnClose();
                //    }
                //}
                //else
                //{
                //    sqlStr = "Select * from ProdDetail1 ";
                //    SqlDataAdapter lda = new SqlDataAdapter();
                //    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                //    lda.SelectCommand = cmd;
                //    lda.Fill(ds, "FeatureDet_vw");

                //}
                ////**** To be Removed -- End

            }
            catch (Exception ex)
            {
                ErrorMsg = "Error Occured while retrieving features." + Environment.NewLine + ex.Message;
                return false;
            }
            //ds.Tables["FeatureDet_vw"].WriteXml("d:\\feature.xml");
            return true;
        }

        private bool CheckRegisterMeValidation()
        {
            //this.CheckRegisterMe();               //Commented by Shrikant S. on 19/02/2019 for Registration
            if (Utilities.ReverseString(MacId) != GetMachineDetails.ProcessorId() || mudshortcode != regshortcode)
            {
                ErrorMsg = "Please check, invalid Register.me file. ";
                return false;
            }
            return true;
        }

        private bool CheckIniValidation()
        {
            if (ds.Tables["CustInfo_vw"].Rows.Count > 0)
            {
                if (ds.Tables["CustInfo_vw"].Rows[0]["macid"].ToString() != GetMachineDetails.ProcessorId())
                {
                    ErrorMsg = "Please check, invalid Info.inf file. ";
                    return false;
                }
            }
            else
            {
                ErrorMsg = "Info.inf file not found...!!! ";
                return false;
            }
            return true;
        }
        //Added by Shrikant S. on 16/05/2018 for Bug-31515      //Start

        private DataTable GetOptionDetails()
        {
            string strSQL = "select [Group] as para1,[desc] as para2,[rep_nm] as para3,rtrim([Group])+rtrim([desc])+rtrim([rep_nm]) as para4,upper(Rtrim([Group]))+upper(Rtrim([Desc]))+upper(Rtrim([Rep_nm])) as featureNm,'REPORT' as FeatureTy from " + zipFileName + "..R_STATUS ";
            strSQL = strSQL + " UNION ALL Select [padname] as para1,[barname] as para2,rtrim(convert(varchar(10),[range])) as para3,rtrim([padname])+rtrim([barname]) as para4,upper(Rtrim(Padname))+upper(Rtrim(Barname)) as featureNm,'MENU' as FeatureTy from  " + zipFileName + "..com_menu ";
            strSQL = strSQL + " UNION ALL Select [Entry_ty] as para1,para2='',para3='',para4=[Entry_ty],upper(Rtrim(Entry_ty)) as featureNm,'TRANSACTION' as FeatureTy from  " + zipFileName + "..Lcode ";
            return oDataAccess.GetDataSet(strSQL, null, 20).Tables[0];
        }

        private string EscapeSpecialCharacters(string filterValue)
        {
            StringBuilder sb = new StringBuilder(filterValue.Length);
            for (int i = 0; i < filterValue.Length; i++)
            {
                char c = filterValue[i];
                switch (c)
                {
                    //case ']':
                    //case '[':
                    //case '%':
                    case '*':
                        sb.Append("[" + c + "]");
                        break;
                    case '\'':
                        sb.Append("''");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
        //Added by Shrikant S. on 16/05/2018 for Bug-31515      //End

        //Added by Shrikant S. on 04/02/2019 for Registration       //Start


        void oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // The background process is complete. We need to inspect
            // our response to see if an error occurred, a cancel was
            // requested or if we completed successfully.  
            if (e.Cancelled)
            {
                lblStatus.Text = "Task Cancelled.";
                progressPercentage = 0;
            }

            // Check to see if an error occurred in the background process.

            else if (e.Error != null)
            {
                lblStatus.Text = "Error while performing background operation.";
            }
            else
            {
                // Everything completed normally.
                lblStatus.Text = "Task Completed...";
                progressPercentage = 100;
                this.btnCancel.Text = "&Close";
            }

            //Change the status of the buttons on the UI accordingly
            //btnStartAsyncOperation.Enabled = true;
            //btnCancel.Enabled = false;
        }
        void oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // This function fires on the UI thread so it's safe to edit
            // the UI control directly, no funny business with Control.Invoke :)
            // Update the progressBar with the integer supplied to us from the
            // ReportProgress() function.  

            //progressBar1.Value = e.ProgressPercentage;
            Thread.Sleep(10);
            if (lblStatus.Visible == false)
            {
                lblStatus.Visible = true;
            }
            lblStatus.Text = this.status + Environment.NewLine + "Percentage completed: " + e.ProgressPercentage.ToString() + " % ";

            progressPercentage = e.ProgressPercentage;
        }

        void oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //upgradeProdcode = string.Empty;         //Added by Shrikant S. on 05/04/2019 for Registration            
            UpgradationCount = 0;                   //Added by Shrikant S. on 03/04/2019 for Registration            
            //Added by Shrikant S. on 07/05/2018 for bug-31515      && Start
            //if (_IgnoreStandardRule == true)                  //Commented by Shrikant S. on 04/04/2019 
            //{
            //    this.rdoProductUpgrade.Checked = true;
            //    //this.Refresh();
            //}
            //Added by Shrikant S. on 07/05/2018 for bug-31515      && End

            if (rdoProductUpgrade.Checked == true)
            {
                //lblStatus.Visible = true;
                Thread.Sleep(10);

                this.status = "Please wait... Checking Registration";
                this.progressPercentage = 5;
                this.oWorker.ReportProgress(progressPercentage);
                if (this.CheckRegisterMe())               //Commented by Shrikant S. on 19/02/2019 for Registration
                //if (this.checkMefile)                        //Added by Shrikant S. on 19/02/2019 for Registration
                {
                    if (!CheckRegisterMeValidation())
                    {
                        this.CancelProcess();
                        //lblStatus.Visible = false;
                        MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                }
                else
                {
                    if (_IgnoreStandardRule == false)
                    {
                        this.CancelProcess();
                        //lblStatus.Visible = false;
                        MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                }
                //this.ReadInfFile();           //Commented by Shrikant S. on 19/02/2019 for Registration

                if (!this.CheckIniValidation())
                {
                    this.CancelProcess();
                    //lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                this.status = "Extracting Zip file";
                this.progressPercentage = 10;
                this.oWorker.ReportProgress(this.progressPercentage);
                //ExtractZipFiles();        //Commented by Shrikant S. on 15/03/2018 

                if (!ExtractZipFiles())
                {
                    this.CancelProcess();
                    //lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }


                //if (!this.ExecuteUpdates())
                //{
                //    lblStatus.Visible = false;
                //    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    this.Close();
                //    return;
                //}

                this.status = "Please wait... Checking Features";
                this.progressPercentage = 20;
                this.oWorker.ReportProgress(progressPercentage);
                if (!this.GetFeatures())
                {
                    this.CancelProcess();
                    //lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                //Added by Shrikant S. on 07/05/2018 for Bug-31515          //Start
                if (_IgnoreStandardRule == true)
                {
                    this.UpdateInfInfo();
                }
                //Added by Shrikant S. on 07/05/2018 for Bug-31515          //End

                //this.status = "Please wait... Getting Features from Default Database: " + zipFileName;
                //this.progressPercentage = 30;
                //this.oWorker.ReportProgress(progressPercentage);
                //if (!this.GetExistingFeaturesFromDefaultDatabase(zipFileName))
                //{
                //    this.btnCancel.PerformClick();
                //    lblStatus.Visible = false;
                //    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    this.Close();
                //    return;
                //}

                this.status = "Please wait... Checking modules";
                this.progressPercentage = 40;
                this.oWorker.ReportProgress(progressPercentage);

                //if (!this.DoProductUpgrade())                 //Commented by Shrikant S. on 04/02/2018 for Registration 
                if (!this.DoProductUpgrade_New())               //Added by Shrikant S. on 04/02/2018 for Registration     
                {
                    this.CancelProcess();
                    //lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                this.status = "Please wait... Generating features";
                this.progressPercentage = 50;
                this.oWorker.ReportProgress(progressPercentage);

                //if (!this.ExecuteScripts())               //Commented by Shrikant S. on 04/02/2018 for Registration 
                if (!this.ExecuteScripts_New())                 //Added by Shrikant S. on 04/02/2018 for Registration  
                {
                    this.CancelProcess();
                    //lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                this.progressPercentage = 70;
                this.oWorker.ReportProgress(progressPercentage);
                //if (!this.InsertFeature())             //Commented by Shrikant S. on 04/02/2018 for Registration 
                if (!this.InsertFeature_New())          //Added by Shrikant S. on 04/02/2018 for Registration     
                {
                    this.CancelProcess();
                    //lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                this.status = "Please wait... Updating modules";
                this.progressPercentage = 90;
                this.oWorker.ReportProgress(progressPercentage);
                //if (!this.DoMenuUpdate())                 //Commented by Shrikant S. on 04/02/2018 for Registration 
                if (!this.DoMenuUpdate_New())                    //Added by Shrikant S. on 04/02/2018 for Registration     
                {
                    this.CancelProcess();
                    //lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                if (this._CurrentAppFile != this._UpgradeAppFile)
                {
                    if (!this.UpdateRolesRights())
                    {
                        MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                }

                this.progressPercentage = 95;
                this.oWorker.ReportProgress(progressPercentage);
                if (!this.Update_Request(2))
                {
                    this.CancelProcess();
                    //lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                this.progressPercentage = 97;
                this.oWorker.ReportProgress(progressPercentage);
                if (this.CurrentAppFile != _UpgradeAppFile)
                {
                    if (!this.WriteConfigFile(_UpgradeAppFile))
                    {
                        MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                }
                this.status = "";
                this.oWorker.ReportProgress(100);
                string msg = string.Empty;
                if (UpgradationCount > 0)
                    msg = "Upgradation done ..." + Environment.NewLine;

                if (LogFileList.Count() > 0)
                    msg = msg + "Please check the log file in Companyfolder under Upgrade_Log folder. File(s) are as follows :" + Environment.NewLine;

                for (int i = 0; i < LogFileList.Count(); i++)
                {
                    msg = msg + LogFileList.ElementAt(i) + Environment.NewLine;
                }
                if (msg.Length > 0)
                    MessageBox.Show(msg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.None);

                oWorker.Dispose();
                //this.Close();
                //this.Dispose();
            }
            else
            {
                string fileNames = string.Empty;
                if (this.CurrentAppFile.Trim() == this.UpgradeAppFile.Trim())
                {

                    //Added by Shrikant S. on 06/02/2019 for Registration           //Start
                    if (ds.Tables["Product_vw"] != null)
                    {
                        DataRow[] defaProducts = ds.Tables["Product_vw"].Select("Sel=True and isDefault=True");
                        if (defaProducts.Count() > 0 && this.CheckRegisterMe())                   //Commented by Shrikant S. on 19/02/2019 for Registration
                                                                                                  //if (defaProducts.Count() > 0 && this.checkMefile)                           //Added by Shrikant S. on 19/02/2019 for Registration
                        {
                            this.status = "Please wait... Extracting zip file";
                            this.progressPercentage = 10;
                            this.oWorker.ReportProgress(progressPercentage);
                            if (!ExtractZipFiles())
                            {
                                this.CancelProcess();
                                //lblStatus.Visible = false;
                                MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                                return;
                            }
                            this.status = "Please wait... Checking Registration";
                            this.progressPercentage = 15;
                            this.oWorker.ReportProgress(progressPercentage);
                            if (this.CheckRegisterMe())               //Commented by Shrikant S. on 19/02/2019 for Registration
                                                                      //if (this.checkMefile)                        //Added by Shrikant S. on 19/02/2019 for Registration
                            {
                                if (!CheckRegisterMeValidation())
                                {
                                    this.CancelProcess();
                                    //lblStatus.Visible = false;
                                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Close();
                                    return;
                                }
                            }
                            else
                            {
                                if (_IgnoreStandardRule == false)
                                {
                                    this.CancelProcess();
                                    //lblStatus.Visible = false;
                                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Close();
                                    return;
                                }
                            }
                            //this.ReadInfFile();                //Commented by Shrikant S. on 19/02/2019 for Registration

                            if (!this.CheckIniValidation())
                            {
                                this.CancelProcess();
                                //lblStatus.Visible = false;
                                MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                                return;
                            }
                            this.status = "Please wait... Checking Features";
                            this.progressPercentage = 20;
                            this.oWorker.ReportProgress(progressPercentage);
                            if (!this.GetFeatures())
                            {
                                this.CancelProcess();
                                //lblStatus.Visible = false;
                                MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                                return;
                            }


                            if (_IgnoreStandardRule == true)
                            {
                                this.UpdateInfInfo();
                            }
                            //this.status = "Please wait... Getting Features from Default Database: "+zipFileName;
                            //this.progressPercentage = 30;
                            //this.oWorker.ReportProgress(progressPercentage);
                            //if (!this.GetExistingFeaturesFromDefaultDatabase(zipFileName))
                            //{
                            //    this.btnCancel.PerformClick();
                            //    lblStatus.Visible = false;
                            //    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //    this.Close();
                            //    return;
                            //}

                            this.status = "Please wait... Checking modules";
                            this.progressPercentage = 40;
                            this.oWorker.ReportProgress(progressPercentage);
                            if (!this.DoProductUpgrade_DefaultModule())
                            {
                                this.CancelProcess();
                                //lblStatus.Visible = false;
                                MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                                return;
                            }
                            this.status = "Please wait... Generating features";
                            this.progressPercentage = 50;
                            this.oWorker.ReportProgress(progressPercentage);
                            if (!this.ExecuteScripts_New())
                            {
                                this.CancelProcess();
                                //lblStatus.Visible = false;
                                MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                                return;
                            }
                            this.progressPercentage = 70;
                            this.oWorker.ReportProgress(progressPercentage);
                            if (!this.InsertFeature_DefaultModule())
                            {
                                this.CancelProcess();
                                //lblStatus.Visible = false;
                                MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                                return;
                            }
                            this.status = "Please wait... Updating modules";
                            this.progressPercentage = 90;
                            this.oWorker.ReportProgress(progressPercentage);
                            if (!this.DoMenuUpdate_DefaultModule())
                            {
                                this.CancelProcess();
                                //lblStatus.Visible = false;
                                MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                                return;
                            }

                            this.status = "Please wait... Generating xml for additional module";
                            this.oWorker.ReportProgress(95);
                            string msg = string.Empty;
                            if (UpgradationCount > 0)
                            {
                                msg = "Upgradation done for existing module/default modules/licensed modules which are selected..." + Environment.NewLine + "Please check the log file in Companyfolder under Upgrade_Log folder. File(s) are as follows :" + Environment.NewLine;
                                for (int i = 0; i < LogFileList.Count(); i++)
                                {
                                    msg = msg + LogFileList.ElementAt(i) + Environment.NewLine;
                                }
                                MessageBox.Show(msg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.None);
                            }
                        }

                    }
                }
                this.status = "Please wait... Generating xml for additional module";
                this.oWorker.ReportProgress(99);

                this.GetAdditionalModules();

                this.oWorker.ReportProgress(100);

                fileNames = GenerateCompanyXml();

                string upgradFile = GenerateProductXml();

                this.Update_manufact();
                if (!this.Update_Request(1, Path.Combine(appPath, fileNames), Path.Combine(appPath, upgradFile)))
                {
                    this.CancelProcess();
                    //lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                ////***** Commented by Sachin N. S. on 13/02/2020 for Bug- -- Start
                //if (File.Exists(upgradFile))
                //    File.Delete(upgradFile);
                ////***** Commented by Sachin N. S. on 13/02/2020 for Bug- -- End
                //Added by Shrikant S. on 06/02/2019 for Registration           //End

                //fileNames = fileNames + Environment.NewLine + GenerateProductXml();      //Commented by Shrikant S. on 04/02/2019 for Registration
                //fileNames = fileNames + Environment.NewLine + "Xml file generated successfully for upgradation in main folder .";
                fileNames = fileNames + Environment.NewLine + upgradFile + Environment.NewLine + "Xml file generated successfully for upgradation in main folder.";    // Added by Sachin N. S. on 13/02/2020 for Bug-
                this.status = fileNames;
                this.oWorker.ReportProgress(100);
                MessageBox.Show(fileNames, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.Close();
                //this.Dispose();

            }

            //// The sender is the BackgroundWorker object we need it to
            //// report progress and check for cancellation.
            ////NOTE : Never play with the UI thread here...
            //for (int i = 0; i < 100; i++)
            //{
            //    Thread.Sleep(100);

            //    // Periodically report progress to the main thread so that it can
            //    // update the UI.  In most cases you'll just need to send an
            //    // integer that will update a ProgressBar                    
            //    oWorker.ReportProgress(i);
            //    // Periodically check if a cancellation request is pending.
            //    // If the user clicks cancel the line
            //    // m_AsyncWorker.CancelAsync(); if ran above.  This
            //    // sets the CancellationPending to true.
            //    // You must check this flag in here and react to it.
            //    // We react to it by setting e.Cancel to true and leaving
            //    if (oWorker.CancellationPending)
            //    {
            //        // Set the e.Cancel flag so that the WorkerCompleted event
            //        // knows that the process was cancelled.
            //        e.Cancel = true;
            //        oWorker.ReportProgress(0);
            //        return;
            //    }
            //}

            ////Report 100% completion on operation completed
            //oWorker.ReportProgress(100);
        }
        public bool UpdateRolesRights()
        {
            string sqlStr = string.Empty;
            this.status = "Please wait... Updating user rights..";
            this.oWorker.ReportProgress(this.progressPercentage);
            try
            {
                string cRights = Utilities.GetDecoder("IYCYDYPYVY", false).Trim();
                string cRoles = Utilities.GetDecoder("ADMINISTRATOR", true).Trim();

                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    string compRights = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim().Replace("'", "''") + "[" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["sta_dt"]).Year.ToString() + "-" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["end_dt"]).Year.ToString() + "]";
                    string dbName = ds.Tables["Co_mast_vw"].Rows[k]["dbname"].ToString().Trim();
                    sqlStr = "Update Vudyog..RolesRights set ProdCode=Convert(varbinary(max),@uProdcode) Where dbo.func_decoder(LTRIM(RTRIM(company)),'T') = '" + compRights + "' and prodcode=Convert(varbinary(max),@Prodcode)";
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd.CommandTimeout = 0;
                    SqlParameter newprodcode = new SqlParameter("@uProdcode", SqlDbType.VarChar);
                    newprodcode.Value = VU_UDFS.NewENCRY(VU_UDFS.enc(upgradeProdcode), "Ud*yog+1993").ToString();
                    cmd.Parameters.Add(newprodcode);
                    SqlParameter prodcode = new SqlParameter("@Prodcode", SqlDbType.VarChar);
                    prodcode.Value = VU_UDFS.NewENCRY(VU_UDFS.enc(mudprodcode), "Ud*yog+1993").ToString();
                    cmd.Parameters.Add(prodcode);
                    this.ConnOpen();
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();
                    sqlStr = " insert into Vudyog..rolesrights(padname,barname,[range],company,rights,user_roles,Prodcode) Select PadName,Barname,[Range],dbo.func_decoder(ltrim(rtrim(@compForrights)),'F'), @rights, @roles,Convert(varbinary(max),@Product) From " + dbName + "..Com_Menu ";
                    sqlStr = sqlStr + " " + " Where rtrim(upper(padname))+rtrim(upper(barname)) Not in (Select rtrim(upper(padname))+rtrim(upper(barname)) From Vudyog..rolesrights WITH(NOLOCK) Where user_roles=@roles  and dbo.func_decoder(ltrim(rtrim(Company)),'T')=@compForrights )";

                    SqlParameter companyRights = new SqlParameter("@compForrights", SqlDbType.VarChar);
                    companyRights.Value = compRights;
                    cmd.Parameters.Add(companyRights);

                    SqlParameter rRights = new SqlParameter("@rights", SqlDbType.VarChar);
                    rRights.Value = cRights;
                    cmd.Parameters.Add(rRights);

                    SqlParameter rRoles = new SqlParameter("@roles", SqlDbType.VarChar);
                    rRoles.Value = cRoles;
                    cmd.Parameters.Add(rRoles);

                    SqlParameter product = new SqlParameter("@Product", SqlDbType.VarChar);
                    product.Value = VU_UDFS.NewENCRY(VU_UDFS.enc(upgradeProdcode), "Ud*yog+1993").ToString();
                    cmd.Parameters.Add(product);

                    cmd.CommandText = sqlStr;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    this.ConnClose();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Error Occured while updating user rights." + Environment.NewLine + ex.Message;
                return false;
            }

            return true;
        }
        public bool GetExistingFeaturesFromCompanyDatabase(string companyDBName)
        {
            if (ds.Tables["ExFeature_vw"] != null)
            {
                ds.Tables["ExFeature_vw"].Rows.Clear();
            }

            string sqlStr = string.Empty;
            //this.ShowProcess("Genearating Features..");
            this.status = "Please wait... Getting Existing Features..";
            this.oWorker.ReportProgress(this.progressPercentage);
            try
            {
                sqlStr = "Select upper(Rtrim(Padname))+upper(Rtrim(Barname)) as featureNm,'MENU' as FeatureTy,enc=convert(varchar(max),newrange),Fenc=Convert(varchar(max),'') From " + companyDBName + "..Com_menu Where Isnull([newRange],0x)<>0x  ";
                sqlStr = sqlStr + " " + "Union all Select upper(Rtrim(Entry_ty)) as featureNm,'TRANSACTION' as FeatureTy,enc=convert(varchar(max),cd),Fenc=Convert(varchar(max),'') From " + companyDBName + "..Lcode Where Isnull([cd],0x)<>0x  ";
                sqlStr = sqlStr + " " + "Union all Select upper(Rtrim([Group]))+upper(Rtrim([Desc]))+upper(Rtrim([Rep_nm])) as featureNm,'REPORT' as FeatureTy,enc=convert(varchar(max),newgroup),Fenc=Convert(varchar(max),'') From " + companyDBName + "..R_status Where Isnull([newgroup],0x)<>0x  ";
                SqlDataAdapter lda = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                lda.SelectCommand = cmd;
                lda.Fill(ds, "ExFeature_vw");


            }
            catch (Exception ex)
            {
                ErrorMsg = "Error Occured while retrieving existing features." + Environment.NewLine + ex.Message;
                return false;
            }

            return true;
        }

        //public bool GetExistingFeaturesFromDefaultDatabase(string companyDBName)
        //{
        //    if (ds.Tables["DefaFeature_vw"] != null)
        //    {
        //        ds.Tables["DefaFeature_vw"].Rows.Clear();
        //    }

        //    string sqlStr = string.Empty;
        //    //this.ShowProcess("Genearating Features..");
        //    this.status = "Please wait... Getting Default Existing Features..";
        //    this.oWorker.ReportProgress(this.progressPercentage);
        //    try
        //    {
        //        sqlStr = "Select upper(Rtrim(Padname))+upper(Rtrim(Barname)) as featureNm,'MENU' as FeatureTy From " + companyDBName + "..Com_menu Where Isnull([newRange],0x)<>0x  ";
        //        sqlStr = sqlStr + " " + "Union all Select upper(Rtrim(Entry_ty)) as featureNm,'TRANSACTION' as FeatureTy From " + companyDBName + "..Lcode Where Isnull([cd],0x)<>0x  ";
        //        sqlStr = sqlStr + " " + "Union all Select upper(Rtrim([Group]))+upper(Rtrim([Desc]))+upper(Rtrim([Rep_nm])) as featureNm,'REPORT' as FeatureTy From " + companyDBName + "..R_status Where Isnull([newgroup],0x)<>0x  ";
        //        SqlDataAdapter lda = new SqlDataAdapter();
        //        SqlCommand cmd = new SqlCommand(sqlStr, conn);
        //        lda.SelectCommand = cmd;
        //        lda.Fill(ds, "DefaFeature_vw");

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMsg = "Error Occured while retrieving default existing features." + Environment.NewLine + ex.Message;
        //        return false;
        //    }
        //    return true;
        //}

        private void Update_modulecode()
        {
            for (int i = 0; i < Complist.Rows.Count; i++)
            {
                string _addimodulelist = Complist.Rows[i]["addimodlist"].ToString();
                string[] _addmodarray = _addimodulelist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string _addimodcodelist = string.Empty;

                string _upgmodulelist = Complist.Rows[i]["selmodlist"].ToString();
                string[] _upgmodarray = _upgmodulelist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string _upgmodcodelist = string.Empty;
                string _upgmainmodule = string.Empty;


                for (int j = 0; j < _addmodarray.Length; j++)
                {
                    DataRow[] modRows = ds.Tables["Module_vw"].Select("Trim(cProdName)='" + _addmodarray[j].Trim() + "' And isDefault=False");
                    if (modRows.Length > 0)
                    {
                        foreach (var item in modRows)
                        {
                            _addimodcodelist = _addimodcodelist + item["cProdCode"].ToString() + ",";
                        }
                    }
                }
                Complist.Rows[i]["addimodcode"] = _addimodcodelist;

                for (int j = 0; j < _upgmodarray.Length; j++)
                {
                    DataRow[] modRows = ds.Tables["Module_vw"].Select("Trim(cProdName)='" + _upgmodarray[j].Trim() + "' ");
                    if (modRows.Length > 0)
                    {
                        foreach (var item in modRows)
                        {
                            _upgmodcodelist = _upgmodcodelist + item["cProdCode"].ToString() + ",";
                            _upgmainmodule = _upgmainmodule + (!_upgmainmodule.Contains(item["cMainProdCode"].ToString()) ? item["cMainProdCode"].ToString() + "," : "");
                        }
                    }
                }
                Complist.Rows[i]["selmodcode"] = _upgmodcodelist;
                Complist.Rows[i]["selmainmodule"] = _upgmainmodule;
            }
        }
        private bool Update_Request(int requestType, string filename = "", string upgradefile = "")
        {
            try
            {
                conn = new SqlConnection(readIni.ConnectionString);
                string sqlStr = string.Empty; SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                switch (requestType)
                {
                    case 1:
                        if (filename.Length <= 0)
                            throw new Exception("FileName is blank");
                        sqlStr = "if Not Exists(Select Top 1 ReqTime From UpdateRequest) ";
                        sqlStr = sqlStr + " Begin Insert Into UpdateRequest(RequestData,ReqTime,ReqprodUpd) ";
                        sqlStr = sqlStr + " SELECT *,getdate(),'' FROM OPENROWSET( BULK '" + filename + "',SINGLE_BLOB) AS x  ";
                        sqlStr = sqlStr + " update UpdateRequest set ReqprodUpd=a.Bulkcolumn from(SELECT * FROM OPENROWSET(BULK '" + upgradefile + "', SINGLE_BLOB) AS x) a end";
                        sqlStr = sqlStr + " Else Begin  ";
                        sqlStr = sqlStr + " update UpdateRequest set RequestData=a.Bulkcolumn,ReqTime=a.ReqTime from (SELECT *,getdate() as ReqTime FROM OPENROWSET( BULK '" + filename + "', SINGLE_BLOB) AS x) a";
                        sqlStr = sqlStr + " update UpdateRequest set ReqprodUpd=a.Bulkcolumn from (SELECT * FROM OPENROWSET( BULK '" + upgradefile + "', SINGLE_BLOB) AS x) a";
                        sqlStr = sqlStr + " End";
                        cmd.CommandText = sqlStr;
                        this.ConnOpen();
                        cmd.ExecuteNonQuery();
                        this.ConnClose();
                        break;
                    case 2:
                        sqlStr = "Update UpdateRequest set RequestData='',ReqTime=getdate(),ReqprodUpd=''";
                        cmd.CommandText = sqlStr;
                        this.ConnOpen();
                        cmd.ExecuteNonQuery();
                        this.ConnClose();
                        break;
                    case 3:
                        sqlStr = "Select * From UpdateRequest";
                        cmd.CommandText = sqlStr;
                        DataTable reqtbl = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(reqtbl);
                        if (reqtbl.Rows.Count > 0)
                        {
                            //byte[] bytBuffer = Encoding.GetEncoding(1252).GetBytes(reqtbl.Rows[0]["RequestData"].ToString());
                            byte[] bytBuffer = Encoding.GetEncoding(1252).GetBytes(reqtbl.Rows[0]["RequestData"].ToString());
                            byte[] bytBufferUpgrade = Encoding.GetEncoding(1252).GetBytes(reqtbl.Rows[0]["ReqprodUpd"].ToString());
                            this.ReadAdditionalModuleFromDatabase(bytBuffer, bytBufferUpgrade);
                        }
                        else
                        {
                            throw new Exception("Request not found in UpdateRequest Table");
                        }
                        break;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private bool DoProductUpgrade_DefaultModule()
        {
            UpdateTbl = new DataTable("Update_vw");
            DataRow[] ldrs;
            DataRow updateRow;
            UpdateTbl.Columns.Add("CompId", typeof(System.Int32));
            UpdateTbl.Columns.Add("Co_name", typeof(System.String));
            UpdateTbl.Columns.Add("OptType", typeof(System.String));
            UpdateTbl.Columns.Add("SqlQuery", typeof(System.String));
            UpdateTbl.Columns.Add("encValue", typeof(System.String));
            UpdateTbl.Columns.Add("OptName", typeof(System.String));
            UpdateTbl.Columns.Add("OptUpdate", typeof(int));

            //if (ds.Tables["Product_vw"] != null)
            //{
            //    ds.Tables["Product_vw"].Clear();
            //}
            //if (ds.Tables["cProduct_vw"] != null)
            //{
            //    ds.Tables["cProduct_vw"].Clear();
            //}

            string[] lProducts;
            try
            {
                //this.ShowProcess("Genearating Scripts..");
                this.status = "Please wait... Generating Scripts..";
                this.oWorker.ReportProgress(this.progressPercentage);
                Thread.Sleep(10);
                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    if (this.CurrentAppFile == _UpgradeAppFile)     // Added by Sachin N. S. on 26/12/2019 for Bug-32837
                    {
                        if (!this.GetExistingFeaturesFromCompanyDatabase(ds.Tables["Co_mast_vw"].Rows[k]["dbname"].ToString().Trim()))
                        {
                            throw new Exception();
                        }
                    }
                    //DataRow[] CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + stringList[0] + "'");     //Commented by Shrikant S. on 23/07/2019 for Old Registration 
                    DataRow[] CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim() + "'");       //Added by Shrikant S. on 23/07/2019 for Old Registration 
                    for (int i = 0; i < CustInfoRows.Count(); i++)
                    {
                        string defaData = zipFileName;
                        //string compRights = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim().Replace("'", "''") + "(" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["sta_dt"]).Year.ToString() + "-" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["end_dt"]).Year.ToString() + ")[ADMINISTRATOR]";         //Commented by Shrikant S. on 03/06/2019
                        string compRights = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim().Replace("'", "''") + "[" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["sta_dt"]).Year.ToString() + "-" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["end_dt"]).Year.ToString() + "]";                          //Added by Shrikant S. on 03/06/2019

                        string prodlist = CustInfoRows[i]["ProdCd"].ToString() + "," + CustInfoRows[i]["addProdCd"].ToString();
                        lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        DataRow[] selectedDefaRows = ds.Tables["Product_vw"].Select("Sel=True And Compid=" + ds.Tables["Co_mast_vw"].Rows[k]["Compid"].ToString());
                        string selectedModuleList = string.Empty;
                        foreach (var item in selectedDefaRows)
                        {
                            if (lProducts.Contains(item["cMainProdCode"].ToString()))
                            {
                                selectedModuleList = selectedModuleList + (selectedModuleList.Contains(item["cMainProdCode"].ToString().Trim()) ? "" : item["cMainProdCode"].ToString() + ",");
                            }
                        }
                        lProducts = selectedModuleList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        string lstProducts = string.Empty;
                        for (int l = 0; l < lProducts.Count(); l++)
                        {
                            lstProducts = lstProducts + "'" + lProducts[l] + "',";
                        }
                        lstProducts = (lstProducts.Length != 0 ? lstProducts.Substring(0, lstProducts.Length - 1) : "");

                        ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + mudprodcode + "' and servicever='" + cboServiceVer.Text.ToUpper().Trim() + "'");      //Added by Shrikant S. on 06/02/2019 for Registration

                        //ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + RegProd + "' and servicever='" + cboServiceVer.Text.ToUpper().Trim() + "'");    //Commented by Shrikant S. on 06/02/2019 for Registration
                        //ds.Tables["FeatureDet_vw"].WriteXml("d:\\tst.xml");
                        foreach (DataRow ldr in ldrs)
                        {
                            string optioncond = string.Format("FeatureNm='{0}' and FeatureTy='{1}'", this.EscapeSpecialCharacters(ldr["OptionName"].ToString().Trim().ToUpper()), ldr["OptionType"].ToString().Trim().ToUpper());

                            DataRow[] ExistsRow = ds.Tables["ExFeature_vw"].Select(optioncond);
                            if (this.CurrentAppFile == this.UpgradeAppFile)
                            {
                                if (ExistsRow.Count() > 0)
                                {
                                    continue;
                                }
                            }
                            ExistsRow = optionTable.Select(optioncond);
                            if (ExistsRow.Count() <= 0)
                            {
                                continue;
                            }

                            updateRow = UpdateTbl.NewRow();
                            updateRow["CompId"] = ds.Tables["Co_mast_vw"].Rows[k]["CompId"];
                            updateRow["Co_Name"] = ds.Tables["Co_mast_vw"].Rows[k]["Co_Name"];
                            updateRow["OptType"] = ldr["OptionType"];
                            updateRow["OptName"] = ldr["OptionName"].ToString();
                            updateRow["OptUpdate"] = 0;
                            if (this.CurrentAppFile != _UpgradeAppFile)
                            {
                                updateRow["OptUpdate"] = 1;
                            }

                            if (_IgnoreStandardRule == true)
                            {
                                //string mudprodcode = VU_UDFS.dec(VU_UDFS.NewDECRY(oConnect.RetAppEnc(this.CurrentAppFile), "Ud*yog+1993"));       //Commented by Shrikant S. on 06/02/2019 for Registration
                                string cond = string.Format("Para4='{0}'", this.EscapeSpecialCharacters(ldr["OptionName"].ToString().Trim()));
                                try
                                {
                                    if (mudprodcode == "uERPSDK")
                                    {
                                        DataRow[] optionRow = optionTable.Select(cond);
                                        if (optionRow.Count() > 0)
                                        {
                                            switch (ldr["OptionType"].ToString())
                                            {
                                                case "REPORT":
                                                    updateRow["encValue"] = oConnect.RetEncValue(mudprodcode + "<~*0*~>" + optionRow[0]["para1"].ToString().Trim() + "<~*1*~>" + optionRow[0]["para2"].ToString().Trim() + "<~*2*~>" + optionRow[0]["para3"].ToString().Trim(), "Udencyogprod");
                                                    break;
                                                case "TRANSACTION":
                                                    updateRow["encValue"] = oConnect.RetEncValue(mudprodcode + "<~*0*~>" + optionRow[0]["para1"].ToString().Trim(), "Udencyogprod");
                                                    break;
                                                case "MENU":
                                                    updateRow["encValue"] = oConnect.RetEncValue(mudprodcode + "<~*0*~>" + optionRow[0]["para1"].ToString().Trim() + "<~*1*~>" + optionRow[0]["para2"].ToString().Trim() + "<~*2*~>" + optionRow[0]["para3"].ToString().Trim(), "Udencyogprod");
                                                    break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        updateRow["encValue"] = oConnect.RetEncValue(ldr["FeatureId"].ToString().Trim() + "~*0*~" + ldr["sfeatureid"].ToString().Trim() + "~*1*~" + ldr["OptionName"].ToString().Trim() + "~*2*~", "Udencyogprod");
                                    }
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                            else
                            {
                                updateRow["encValue"] = oConnect.RetEncValue(ldr["FeatureId"].ToString().Trim() + "~*0*~" + ldr["sfeatureid"].ToString().Trim() + "~*1*~" + ldr["OptionName"].ToString().Trim() + "~*2*~", "Udencyogprod");
                            }
                            //updateRow["SqlQuery"] = this.GenInsertUpdate(ds.Tables["Co_mast_vw"].Rows[k]["dbname"].ToString().Trim(), ldr[4].ToString(), ldr[11].ToString().Replace("'", "''"), defaData, compRights);                                            //Commented by Shrikant S. on 19/04/2019 for Registration
                            updateRow["SqlQuery"] = this.GenInsertUpdate(ds.Tables["Co_mast_vw"].Rows[k]["dbname"].ToString().Trim(), ldr[4].ToString(), ldr[11].ToString().Replace("'", "''"), defaData, compRights, Convert.ToInt32(updateRow["OptUpdate"]));     //Added by Shrikant S. on 19/04/2019 for Registration
                            UpdateTbl.Rows.Add(updateRow);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                return false;
            }

            return true;
        }

        private bool DoMenuUpdate_DefaultModule()
        {
            if (_IgnoreStandardRule == true)
                return true;

            SqlCommand lcmd;
            string sqlStr = string.Empty;
            string[] lProducts;
            DataRow[] ldrs;
            try
            {
                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    string companyName = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim();
                    string companyDB = ds.Tables["Co_mast_vw"].Rows[k]["DBName"].ToString().ToUpper().Trim();

                    //DataRow[] CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + stringList[0] + "'");     //Commented by Shrikant S. on 23/07/2019 for Old Registration Process
                    DataRow[] CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + companyName + "'");       //Added by Shrikant S. on 23/07/2019 for Old Registration Process
                    for (int i = 0; i < CustInfoRows.Count(); i++)
                    {

                        //pbar.ShowProgress("Inserting Features for company:" + companyName, 0);
                        //this.ShowProcess("Updating menus...");
                        this.status = "Updating menus for Company :" + companyName;
                        this.oWorker.ReportProgress(this.progressPercentage);

                        try
                        {
                            sqlStr = "Update " + companyDB + "..Com_menu Set LabKey=''";
                            lcmd = new SqlCommand(sqlStr, conn);
                            this.ConnOpen();
                            lcmd.ExecuteNonQuery();
                            this.ConnClose();
                        }
                        catch
                        {
                            MessageBox.Show("Unable to update Menu Table." + Environment.NewLine + "Please contact your software vendor.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        lProducts = CustInfoRows[i]["ProdCd"].ToString().Split(',');
                        //string lstProducts = string.Empty;
                        //for (int l = 0; l < lProducts.Count(); l++)
                        //{
                        //    lstProducts = lstProducts + "'" + lProducts[l] + "',";
                        //}

                        DataRow[] selectedDefaRows = ds.Tables["Product_vw"].Select("Sel=True And CompId=" + ds.Tables["Co_mast_vw"].Rows[k]["CompId"].ToString());
                        string selectedModuleList = string.Empty;
                        foreach (var item in selectedDefaRows)
                        {
                            if (lProducts.Contains(item["cMainProdCode"].ToString()))
                            {
                                selectedModuleList = selectedModuleList + (selectedModuleList.Contains(item["cMainProdCode"].ToString().Trim()) ? "" : item["cMainProdCode"].ToString() + ",");
                            }
                        }
                        lProducts = selectedModuleList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        string lstProducts = string.Empty;
                        for (int l = 0; l < lProducts.Count(); l++)
                        {
                            lstProducts = lstProducts + "'" + lProducts[l] + "',";
                        }

                        lstProducts = (lstProducts.Length != 0 ? lstProducts.Substring(0, lstProducts.Length - 1) : "");

                        ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + mudprodcode + "' and servicever='NORMAL' and fType='PREMIUM' and OptionType='MENU'");         //Added by Shrikant S. on 06/02/2019 for Registration
                        //ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + RegProd + "' and servicever='NORMAL' and fType='PREMIUM' and OptionType='MENU'");       //Commented by Shrikant S. on 06/02/2019 for Registration

                        foreach (DataRow ldr in ldrs)
                        {
                            //pbar.ShowProgress("Inserting Features for company:" + companyName, Convert.ToInt32(j * ratio)); 
                            sqlStr = "If Exists (Select Top 1 enc From " + companyDB + "..Clientfeature Where Enc=@encValue) ";
                            sqlStr = sqlStr + " " + "Begin";
                            sqlStr = sqlStr + " " + "Update " + companyDB + "..Com_Menu set LabKey='SPREMIUM' Where ltrim(rtrim(Padname))+ltrim(rtrim(Barname))='" + ldr["OptionName"].ToString().Trim() + "'";
                            sqlStr = sqlStr + " " + "End";
                            sqlStr = sqlStr + " " + "Else";
                            sqlStr = sqlStr + " " + "Begin";
                            sqlStr = sqlStr + " " + "Update " + companyDB + "..Com_Menu set LabKey='UPREMIUM' Where ltrim(rtrim(Padname))+ltrim(rtrim(Barname))='" + ldr["OptionName"].ToString().Trim() + "'";
                            sqlStr = sqlStr + " " + "End";

                            lcmd = new SqlCommand(sqlStr, conn);
                            SqlParameter param = lcmd.Parameters.Add("@encValue", SqlDbType.NVarChar);
                            param.Value = oConnect.RetEncValue(txtCo_name.Text.Trim() + "~*0*~" + ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim() + "~*1*~" + ldr["FeatureId"].ToString().Trim() + "~*2*~" + companyName, ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim());

                            this.ConnOpen();
                            lcmd.ExecuteNonQuery();
                            this.ConnClose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Error occured while updating menu." + Environment.NewLine + ex.Message;
                return false;
            }
            return true;
        }
        private bool InsertFeature_DefaultModule()
        {

            //ds.WriteXml("D:\\ds.xml");       // Added by Sachin N. S. on 30/11/2019 for Bug-32837
            //MessageBox.Show("Testing - InsertFeature_DefaultModule - 1");
            SqlCommand lcmd;
            string sqlStr = string.Empty;

            try
            {

                //MessageBox.Show("Testing - InsertFeature_DefaultModule - 2");

                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    string companyName = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim();
                    string companyDB = ds.Tables["Co_mast_vw"].Rows[k]["DBName"].ToString().ToUpper().Trim();
                    //DataRow[] CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + stringList[0] + "'");     //Commented by Shrikant S. on 23/07/2019 for Old Registration Process
                    DataRow[] CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + companyName + "'");       //Added by Shrikant S. on 23/07/2019 for Old Registration Process
                    for (int i = 0; i < CustInfoRows.Count(); i++)
                    {
                        string prodlist = string.Empty;
                        string[] lProducts;
                        string selectedModuleList = string.Empty;
                        string selectedAddiModuleList = string.Empty;
                        DataRow[] selectedDefaRows;

                        //pbar.ShowProgress("Inserting Features for company:" + companyName, 0);
                        this.status = "Updating feature for company:" + companyName;
                        this.oWorker.ReportProgress(this.progressPercentage);

                        if (_IgnoreStandardRule == false)                    // Added by Shrikant S. on 07/05/2018 for Bug-31515
                        {
                            //Commented by Shrikant S. on 01/08/2019 for Old Registration Process       //Start
                            //sqlStr = "Delete From " + companyDB + "..ClientFeature";
                            //lcmd = new SqlCommand(sqlStr, conn);
                            //this.ConnOpen();
                            //lcmd.ExecuteNonQuery();
                            //this.ConnClose();
                            //Commented by Shrikant S. on 01/08/2019 for Old Registration Process       //End

                            //string[] lfeatureIds = CustInfoRows[i]["FeatureId"].ToString().Split(',');        //Commented by Shrikant S. on 07/02/2019 for Registration       

                            //Added by Shrikant S. on 07/02/2019 for Registration           //Start
                            prodlist = CustInfoRows[i]["ProdCd"].ToString() + "," + CustInfoRows[i]["addProdCd"].ToString();
                            lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            selectedDefaRows = ds.Tables["Product_vw"].Select("Sel=True And Compid=" + ds.Tables["Co_mast_vw"].Rows[k]["Compid"].ToString());

                            foreach (var item in selectedDefaRows)
                            {
                                if (lProducts.Contains(item["cMainProdCode"].ToString()))
                                {
                                    selectedModuleList = selectedModuleList + (selectedModuleList.Contains(item["cMainProdCode"].ToString().Trim()) ? "" : item["cMainProdCode"].ToString() + ",");
                                }
                            }
                            lProducts = selectedModuleList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            string lstProducts = string.Empty;
                            for (int l = 0; l < lProducts.Count(); l++)
                            {
                                lstProducts = lstProducts + "'" + lProducts[l] + "',";
                            }
                            lstProducts = (lstProducts.Length != 0 ? lstProducts.Substring(0, lstProducts.Length - 1) : "");
                            DataRow[] lfeatureIds = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + mudprodcode + "' and servicever='" + cboServiceVer.Text.ToUpper().Trim() + "'");
                            //Added by Shrikant S. on 06/02/2019 for Registration       //End


                            for (int j = 0; j < lfeatureIds.Length; j++)
                            {
                                //pbar.ShowProgress("Inserting Features for company:" + companyName, Convert.ToInt32(j * ratio)); 
                                sqlStr = "If Not Exists (Select top 1 enc From " + companyDB + "..Clientfeature Where Enc=@encValue)  Begin Insert Into " + companyDB + "..ClientFeature (enc) values (@encValue) End";
                                lcmd = new SqlCommand(sqlStr, conn);
                                SqlParameter param = lcmd.Parameters.Add("@encValue", SqlDbType.NVarChar);
                                param.Value = oConnect.RetEncValue(txtCo_name.Text.Trim() + "~*0*~" + ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim() + "~*1*~" + lfeatureIds[j]["FeatureId"].ToString().Trim().PadLeft(10, '0') + "~*2*~" + companyName, ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim());

                                this.ConnOpen();
                                lcmd.ExecuteNonQuery();
                                this.ConnClose();

                            }
                        }
                        prodlist = CustInfoRows[i]["ProdCd"].ToString();
                        prodlist = string.Join(",", prodlist.Split(',').Distinct().ToArray());

                        lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        selectedDefaRows = ds.Tables["Product_vw"].Select("Sel=True And Compid=" + ds.Tables["Co_mast_vw"].Rows[k]["Compid"].ToString());

                        foreach (var item in selectedDefaRows)
                        {
                            if (lProducts.Contains(item["cMainProdcode"].ToString()))
                            {
                                selectedModuleList = selectedModuleList + item["cMainProdcode"].ToString() + ",";
                            }
                        }
                        selectedModuleList = (selectedModuleList.Length != 0 ? selectedModuleList.Substring(0, selectedModuleList.Length - 1) : "");

                        prodlist = CustInfoRows[i]["AddProdCd"].ToString();
                        prodlist = string.Join(",", prodlist.Split(',').Distinct().ToArray());
                        lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in selectedDefaRows)
                        {
                            if (lProducts.Contains(item["cProdcode"].ToString()))
                            {
                                selectedAddiModuleList = selectedAddiModuleList + item["cProdcode"].ToString() + ",";
                            }
                        }
                        selectedAddiModuleList = (selectedAddiModuleList.Length != 0 ? selectedAddiModuleList.Substring(0, selectedAddiModuleList.Length - 1) : "");

                        //Added by Shrikant S. on 03/04/2019 for Registration       //Start
                        if (!this.UpdateLotherDcMastRecords(companyDB))
                        {
                            return false;
                        }
                        if (!this.UpdateIndustryScript(companyDB, selectedModuleList, selectedAddiModuleList))
                        {
                            return false;
                        }
                        //Added by Shrikant S. on 03/04/2019 for Registration       //End
                        DoCompanyProductUpgrade(selectedModuleList, selectedAddiModuleList, "", " Compid= " + ds.Tables["Co_mast_vw"].Rows[k]["CompId"].ToString());
                    }
                }
                //MessageBox.Show("Testing - InsertFeature_DefaultModule - 3");
            }
            catch (Exception ex)
            {
                ErrorMsg = "Error occured while updating features." + Environment.NewLine + ex.Message;
                return false;
            }

            //MessageBox.Show("Testing - InsertFeature_DefaultModule - 4");
            //pbar.Dispose();
            return true;
        }

        private bool DoProductUpgrade_New()
        {
            UpdateTbl = new DataTable("Update_vw");
            DataRow[] ldrs;
            DataRow updateRow;
            UpdateTbl.Columns.Add("CompId", typeof(System.Int32));
            UpdateTbl.Columns.Add("Co_name", typeof(System.String));
            UpdateTbl.Columns.Add("OptType", typeof(System.String));
            UpdateTbl.Columns.Add("SqlQuery", typeof(System.String));
            UpdateTbl.Columns.Add("encValue", typeof(System.String));
            UpdateTbl.Columns.Add("OptName", typeof(System.String));
            UpdateTbl.Columns.Add("OptUpdate", typeof(int));

            //if (ds.Tables["Product_vw"] != null)
            //{
            //    ds.Tables["Product_vw"].Clear();
            //}
            //if (ds.Tables["cProduct_vw"] != null)
            //{
            //    ds.Tables["cProduct_vw"].Clear();
            //}
            if (ds.Tables["Module_vw"] != null)
            {
                ds.Tables["Module_vw"].Clear();
            }
            this.GetModules();
            this.Update_Request(3);
            this.Update_modulecode();

            //MessageBox.Show("DoPrductUpgrade_New -- 1");

            string[] lProducts;
            try
            {
                //this.ShowProcess("Genearating Scripts..");
                this.status = "Generating Scripts...";
                //Thread.Sleep(10);
                this.oWorker.ReportProgress(this.progressPercentage);

                // ///****** To be removed -- Sachin -- Start
                // //MessageBox.Show("DoPrductUpgrade_New -- 2");

                // ds.WriteXml("D:\\DataSet.xml");

                // foreach (DataTable _dttt in ds.Tables)
                // {
                //     _dttt.WriteXml("D:\\" + _dttt.TableName.Trim() + ".xml");
                // }

                // upgradeInfData.TableName = "upgradeInfData";
                // upgradeInfData.WriteXml("d:\\upgradeInfData.XML");
                ///////****** To be removed -- Sachin -- End

                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    if (this.CurrentAppFile == _UpgradeAppFile)     // Added by Sachin N. S. on 26/12/2019 for Bug-32837
                    {
                        if (!this.GetExistingFeaturesFromCompanyDatabase(ds.Tables["Co_mast_vw"].Rows[k]["dbname"].ToString().Trim()))
                        {
                            throw new Exception();
                        }
                    }
                    //MessageBox.Show("this.CurrentAppFile : " + this.CurrentAppFile);
                    //MessageBox.Show("_UpgradeAppFile : " + _UpgradeAppFile);
                    //MessageBox.Show("upgradeProdcode : " + upgradeProdcode);
                    //MessageBox.Show("mudprodcode : " + mudprodcode);

                    DataRow[] CustInfoRows;
                    if (this.CurrentAppFile != _UpgradeAppFile)
                        CustInfoRows = upgradeInfData.Select("clientnm='" + ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim() + "'");       //Added by Shrikant S. on 23/07/2019 for Old Registration Process
                                                                                                                                                       //CustInfoRows = upgradeInfData.Select("clientnm='" + stringList[0] + "'");     //Commented by Shrikant S. on 23/07/2019 for Old Registration Process
                    else
                    {
                        //CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim() + "'");         //Added by Shrikant S. on 23/07/2019 for Old Registration Process    
                        CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim() + "' and mainprodcd='" + mudprodcode + "' ");          // Changed by Sachin N. S. on 17/12/2019 for Bug-32837
                        //CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + stringList[0] + "'");       //Commented by Shrikant S. on 23/07/2019 for Old Registration Process
                        //DataRow[] custaddimodrows = Complist.Select("compname='" + ds.Tables["Co_mast_vw"].Rows[k].ToString() + "'");
                        //if (custaddimodrows.Length <= 0)
                        //    continue;
                    }

                    //MessageBox.Show("DoPrductUpgrade_New -- 2.1");

                    for (int i = 0; i < CustInfoRows.Count(); i++)
                    {
                        string defaData = zipFileName;
                        //string compRights = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim().Replace("'", "''") + "(" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["sta_dt"]).Year.ToString() + "-" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["end_dt"]).Year.ToString() + ")[ADMINISTRATOR]";         //Commented by Shrikant S. on 03/06/2019
                        string compRights = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim().Replace("'", "''") + "[" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["sta_dt"]).Year.ToString() + "-" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["end_dt"]).Year.ToString() + "]";                          //Added by Shrikant S. on 03/06/2019

                        string lstProducts = string.Empty;
                        string selectedModuleList = string.Empty;
                        string selectedAddiModuleList = string.Empty;
                        string prodlist = string.Empty;

                        string filt = string.Format("compname='{0}' ", ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim());
                        DataRow[] comps = Complist.Select(filt);
                        if (comps.Count() > 0)
                        {
                            prodlist = CustInfoRows[i]["ProdCd"].ToString() + "," + CustInfoRows[i]["AddProdCd"].ToString();
                            lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            string[] seleModules = comps[0]["selmainmodule"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string item in seleModules)
                            {
                                if (lProducts.Contains(item))
                                {
                                    selectedModuleList = selectedModuleList + item + ",";
                                }
                            }
                            selectedModuleList = (selectedModuleList.Length != 0 ? selectedModuleList.Substring(0, selectedModuleList.Length - 1) : "");

                            seleModules = comps[0]["selmodcode"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string item in seleModules)
                            {
                                if (lProducts.Contains(item))
                                {
                                    selectedAddiModuleList = selectedAddiModuleList + item + ",";
                                }
                            }
                            selectedAddiModuleList = (selectedAddiModuleList.Length != 0 ? selectedAddiModuleList.Substring(0, selectedAddiModuleList.Length - 1) : "");
                        }
                        else
                        {
                            prodlist = CustInfoRows[i]["ProdCd"].ToString();
                            lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            //DataRow[] selectedDefaRows = ds.Tables["cProduct_vw"].Select("Sel=True And Compid=" + ds.Tables["Co_mast_vw"].Rows[k]["Compid"].ToString());

                            DataRow[] selectedDefaRows = ds.Tables["Product_vw"].Select("Sel=True And Compid=" + ds.Tables["Co_mast_vw"].Rows[k]["Compid"].ToString());


                            foreach (var item in selectedDefaRows)
                            {
                                if (lProducts.Contains(item["cProdCode"].ToString()))
                                {
                                    selectedModuleList = selectedModuleList + item["cProdCode"].ToString() + ",";
                                }
                                if (lProducts.Contains(item["cMainProdCode"].ToString()))
                                {
                                    selectedModuleList = selectedModuleList + item["cMainProdCode"].ToString() + ",";
                                }
                            }
                            //selectedModuleList = (selectedModuleList.Length != 0 ? selectedModuleList.Substring(0, selectedModuleList.Length - 1) : "");

                            prodlist = CustInfoRows[i]["AddProdCd"].ToString();
                            lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var item in selectedDefaRows)
                            {
                                if (lProducts.Contains(item["cProdCode"].ToString()))
                                {
                                    selectedAddiModuleList = selectedAddiModuleList + item["cProdCode"].ToString() + ",";
                                }
                            }
                            selectedAddiModuleList = (selectedAddiModuleList.Length != 0 ? selectedAddiModuleList.Substring(0, selectedAddiModuleList.Length - 1) : "");
                        }
                        //prodlist = selectedModuleList + selectedAddiModuleList;
                        prodlist = selectedModuleList + (selectedAddiModuleList == "" ? "" : ",") + selectedAddiModuleList;     // Changed by Sachin N. S. on 03/01/2019 for Bug-32837
                        lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int l = 0; l < lProducts.Count(); l++)
                        {
                            lstProducts = lstProducts + "'" + lProducts[l] + "',";
                        }
                        lstProducts = (lstProducts.Length != 0 ? lstProducts.Substring(0, lstProducts.Length - 1) : "");

                        //MessageBox.Show("DoPrductUpgrade_New -- 2.2");

                        //ds.Tables["FeatureDet_vw"].WriteXml("D:\\FeatureDet_vw.xml");

                        //MessageBox.Show("DoPrductUpgrade_New -- 2.2.1");

                        if (this.CurrentAppFile != _UpgradeAppFile)
                            ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + upgradeProdcode + "' and servicever='" + cboServiceVer.Text.ToUpper().Trim() + "'");      //Added by Shrikant S. on 06/02/2019 for Registration
                        else
                            ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + mudprodcode + "' and servicever='" + cboServiceVer.Text.ToUpper().Trim() + "'");      //Added by Shrikant S. on 06/02/2019 for Registration

                        //// **** To be removed -- Sachin - Start
                        ////MessageBox.Show("DoPrductUpgrade_New -- 2.2.2");
                        //if (ldrs != null)
                        //{
                        //    if (ldrs.Count() > 0)
                        //    {
                        //        DataTable _dtttt = ds.Tables["FeatureDet_vw"].Copy();
                        //        _dtttt.Clear();

                        //        foreach (DataRow row in ldrs)
                        //        {
                        //            _dtttt.ImportRow(row);
                        //        }
                        //        _dtttt.TableName = "Featurefilter";
                        //        _dtttt.WriteXml("D:\\Featurefilter.xml");
                        //    }
                        //}
                        ////MessageBox.Show("DoPrductUpgrade_New -- 2.2.3");
                        //// **** To be removed -- Sachin - End

                        //ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + mudprodcode + "' and servicever='" + cboServiceVer.Text.ToUpper().Trim() + "'");      //Added by Shrikant S. on 06/02/2019 for Registration
                        //ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + RegProd + "' and servicever='" + cboServiceVer.Text.ToUpper().Trim() + "'");    //Commented by Shrikant S. on 06/02/2019 for Registration

                        //***** Added by Sachin N. S. on 31/12/2019 for Bug-32837 -- Start
                        if (ldrs != null)
                        {
                            if (ldrs.Count() > 0)
                            {
                                //***** Added by Sachin N. S. on 31/12/2019 for Bug-32837 -- End
                                //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4");

                                foreach (DataRow ldr in ldrs)
                                {

                                    //if (ldr[3].ToString().Trim() == "0000018861" && ldr[2].ToString().Trim() == "REPORT")
                                    //{
                                    //    int A = 1;
                                    //}


                                    //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.1");
                                    string optioncond = string.Format("FeatureNm='{0}' and FeatureTy='{1}'", this.EscapeSpecialCharacters(ldr["OptionName"].ToString().Trim().ToUpper()), ldr["OptionType"].ToString().Trim().ToUpper());
                                    //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.1.1");
                                    //***** Changed by Sachin N. S. on 31/12/2019 for Bug-32837 -- Start
                                    DataRow[] ExistsRow = null;
                                    if (ds.Tables["ExFeature_vw"] != null)
                                    {
                                        if (ds.Tables["ExFeature_vw"].Rows.Count > 0)
                                            ExistsRow = ds.Tables["ExFeature_vw"].Select(optioncond);
                                    }
                                    //DataRow[] ExistsRow = ds.Tables["ExFeature_vw"].Select(optioncond);
                                    //***** Changed by Sachin N. S. on 31/12/2019 for Bug-32837 -- End
                                    //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.1.2");
                                    if (this.CurrentAppFile == this.UpgradeAppFile)
                                    {
                                        //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.1.3");
                                        if (ExistsRow != null)      // Added by Sachin N. S. on 31/12/2019 for Bug-32837
                                        {
                                            //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.1.4");
                                            if (ExistsRow.Count() > 0)
                                            {
                                                //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.1.5");
                                                continue;
                                            }
                                            //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.1.6");
                                        }
                                        //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.1.7");
                                    }
                                    //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.2");

                                    if (this.CurrentAppFile == this.UpgradeAppFile)     // Added by Sachin N. S. on 31/12/2019 for Bug-32837
                                    {
                                        ExistsRow = optionTable.Select(optioncond);
                                        if (ExistsRow != null)      // Added by Sachin N. S. on 31/12/2019 for Bug-32837
                                        {
                                            if (ExistsRow.Count() <= 0)
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.3");
                                    ExistsRow = UpdateTbl.Select(string.Format("OptName='{0}' and CompId=" + ds.Tables["Co_mast_vw"].Rows[k]["CompId"].ToString(), this.EscapeSpecialCharacters(ldr["OptionName"].ToString().Trim().ToUpper())));
                                    if (ExistsRow != null)      // Added by Sachin N. S. on 31/12/2019 for Bug-32837
                                    {
                                        if (ExistsRow.Count() > 0)
                                        {
                                            continue;
                                        }
                                    }
                                    //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.4");
                                    updateRow = UpdateTbl.NewRow();
                                    updateRow["CompId"] = ds.Tables["Co_mast_vw"].Rows[k]["CompId"];
                                    updateRow["Co_Name"] = ds.Tables["Co_mast_vw"].Rows[k]["Co_Name"];
                                    updateRow["OptType"] = ldr["OptionType"];
                                    updateRow["OptName"] = ldr["OptionName"].ToString();
                                    updateRow["OptUpdate"] = 0;
                                    if (this.CurrentAppFile != _UpgradeAppFile)
                                    {
                                        updateRow["OptUpdate"] = 1;
                                    }

                                    //if (ldr["OptionName"].ToString().Trim() == "PURCHASETRANSACTIONSPURCHASE")
                                    //{
                                    //    int a = 1;
                                    //}

                                    //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.2");


                                    if (_IgnoreStandardRule == true)
                                    {

                                        //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.3");

                                        //string mudprodcode = VU_UDFS.dec(VU_UDFS.NewDECRY(oConnect.RetAppEnc(this.CurrentAppFile), "Ud*yog+1993"));       //Commented by Shrikant S. on 06/02/2019 for Registration
                                        string cond = string.Format("Para4='{0}'", this.EscapeSpecialCharacters(ldr["OptionName"].ToString().Trim()));
                                        try
                                        {
                                            DataRow[] optionRow = optionTable.Select(cond);
                                            if (optionRow != null)      // Added by Sachin N. S. on 31/12/2019 for Bug-32837
                                            {
                                                if (optionRow.Count() > 0)
                                                {
                                                    switch (ldr["OptionType"].ToString())
                                                    {
                                                        case "REPORT":
                                                            updateRow["encValue"] = oConnect.RetEncValue(mudprodcode + "<~*0*~>" + optionRow[0]["para1"].ToString().Trim() + "<~*1*~>" + optionRow[0]["para2"].ToString().Trim() + "<~*2*~>" + optionRow[0]["para3"].ToString().Trim(), "Udencyogprod");
                                                            break;
                                                        case "TRANSACTION":
                                                            updateRow["encValue"] = oConnect.RetEncValue(mudprodcode + "<~*0*~>" + optionRow[0]["para1"].ToString().Trim(), "Udencyogprod");
                                                            break;
                                                        case "MENU":
                                                            updateRow["encValue"] = oConnect.RetEncValue(mudprodcode + "<~*0*~>" + optionRow[0]["para1"].ToString().Trim() + "<~*1*~>" + optionRow[0]["para2"].ToString().Trim() + "<~*2*~>" + optionRow[0]["para3"].ToString().Trim(), "Udencyogprod");
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            throw;
                                        }

                                        //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.4");
                                    }
                                    else
                                    {
                                        //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.5");
                                        updateRow["encValue"] = oConnect.RetEncValue(ldr["FeatureId"].ToString().Trim() + "~*0*~" + ldr["sfeatureid"].ToString().Trim() + "~*1*~" + ldr["OptionName"].ToString().Trim() + "~*2*~", "Udencyogprod");
                                        //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.6");
                                    }

                                    //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.7");

                                    //updateRow["SqlQuery"] = this.GenInsertUpdate(ds.Tables["Co_mast_vw"].Rows[k]["dbname"].ToString().Trim(), ldr[4].ToString(), ldr[11].ToString().Replace("'", "''"), defaData, compRights);                        //Commented by Shrikant S. on 19/04/2019 for Registration
                                    updateRow["SqlQuery"] = this.GenInsertUpdate(ds.Tables["Co_mast_vw"].Rows[k]["dbname"].ToString().Trim(), ldr[4].ToString(), ldr[11].ToString().Replace("'", "''"), defaData, compRights, Convert.ToInt32(updateRow["OptUpdate"]));     //Added by Shrikant S. on 19/04/2019 for Registration     
                                    //updateRow["SqlQuery"] = this.GenInsertUpdate(ds.Tables["Co_mast_vw"].Rows[k]["dbname"].ToString().Trim(), ldr[4].ToString(), ldr[10].ToString().Replace("'", "''"), defaData, compRights, Convert.ToInt32(updateRow["OptUpdate"]));     //Added by Shrikant S. on 19/04/2019 for Registration       // To be Removed
                                    UpdateTbl.Rows.Add(updateRow);

                                    //MessageBox.Show("DoPrductUpgrade_New -- 2.2.4.8");
                                }

                                //MessageBox.Show("DoPrductUpgrade_New -- 2.2.5");

                            }       //***** Added by Sachin N. S. on 31/12/2019 for Bug-32837
                        }           //***** Added by Sachin N. S. on 31/12/2019 for Bug-32837
                    }
                }
                //UpdateTbl.WriteXml("D:\\UpdateTbl.xml");
                //MessageBox.Show("DoPrductUpgrade_New -- 3");
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                return false;
            }

            //ds.Tables["FeatureDet_vw"].WriteXml("D:\\FeatureDet_vw.xml");
            //MessageBox.Show("DoPrductUpgrade_New -- 4");

            return true;
        }
        private bool ExecuteScripts_New()
        {
            //MessageBox.Show("Testing - ExecuteScripts_New - 1");

            string companyFolder = string.Empty;
            string LogFile = string.Empty;
            SqlParameter param;
            SqlCommand lcmd;
            string SQL = string.Empty;
            int result = 0;

            //MessageBox.Show("Testing - ExecuteScripts_New - 2");

            LogTable = new DataTable("Log_vw");

            LogTable.Columns.Add("Co_name", typeof(System.String));
            LogTable.Columns.Add("OptType", typeof(System.String));
            LogTable.Columns.Add("OptName", typeof(System.String));
            LogTable.Columns.Add("LogDesc", typeof(System.String));
            //UpdateTbl.WriteXml("d:\\scripts.xml");
            DataView ldv = UpdateTbl.DefaultView;

            //MessageBox.Show("Testing - ExecuteScripts_New - 3");

            if (UpdateTbl != null)
            {
                UpdateTbl = null;
            }

            try
            {

                //MessageBox.Show("this.CurrentAppFile : " + this.CurrentAppFile);
                //MessageBox.Show("_UpgradeAppFile : " + _UpgradeAppFile);

                //ds.Tables["CustInfo_vw"].WriteXml("d:\\CustInfo_vw.xml");

                //MessageBox.Show("Testing - ExecuteScripts_New - 4");

                for (int i = 0; i < ds.Tables["CO_mast_vw"].Rows.Count; i++)
                {
                    DataRow[] CustInfoRows;
                    if (this.CurrentAppFile != _UpgradeAppFile)
                        CustInfoRows = upgradeInfData.Select("clientnm='" + ds.Tables["CO_mast_vw"].Rows[i]["co_name"].ToString().Trim() + "'");       //Added by Shrikant S. on 23/07/2019 for Old Registration Process
                                                                                                                                                       //CustInfoRows = upgradeInfData.Select("clientnm='" + stringList[0] + "'");     //Commented by Shrikant S. on 23/07/2019 for Old Registration Process
                    else
                        CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + ds.Tables["CO_mast_vw"].Rows[i]["co_name"].ToString().Trim() + "'");
                    //CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + stringList[0] + "'");       //Commented by Shrikant S. on 23/07/2019 for Old Registration Process

                    for (int k = 0; k < CustInfoRows.Count(); k++)
                    {

                        totRecords = 0;
                        unFetRecords = 0;

                        ProcessStart = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                        companyFolder = ds.Tables["CO_mast_vw"].Rows[i]["foldername"].ToString().Trim();
                        CreateLogFile(ds.Tables["CO_mast_vw"].Rows[i]["co_name"].ToString().Trim(), companyFolder);

                        //ldv.RowFilter = "Co_name='" + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim()+"'";  //Commented by Prajakta B. on 12032018 for Bug 31351
                        //ldv.RowFilter = "Trim(Co_name)='" + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim().Replace("'", "''") + "'";   //Added by Prajakta B. on 12032018 for Bug 31351
                        ldv.RowFilter = "CompId=" + ds.Tables["CO_mast_vw"].Rows[i]["compid"].ToString();   //Added by Prajakta B. on 12032018 for Bug 31351

                        ldv.Sort = "Compid,Co_name,OptType";
                        totRecords = ldv.ToTable().Rows.Count;


                        //pbar.ShowProgress("Executing Features for company: " + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim(), 0);
                        //this.ShowProcess("Updating features for :" + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim());

                        this.status = "Updating features for company: " + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim();
                        this.oWorker.ReportProgress(this.progressPercentage);

                        //this.ConnOpen();


                        for (int j = 0; j < ldv.ToTable().Rows.Count; j++)
                        {
                            try
                            {
                                //pbar.ShowProgress("Executing Features for company: " + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim(), Convert.ToInt32(j * ratio));

                                SQL = ldv.ToTable().Rows[j]["SqlQuery"].ToString();


                                lcmd = new SqlCommand(SQL, conn);
                                if (this.CurrentAppFile != _UpgradeAppFile)
                                {
                                    lcmd.Parameters.Add("@pEnc", SqlDbType.VarChar).Value = VU_UDFS.NewENCRY(VU_UDFS.enc(upgradeProdcode), "Ud*yog+1993");
                                }
                                else
                                {
                                    lcmd.Parameters.Add("@pEnc", SqlDbType.VarChar).Value = VU_UDFS.NewENCRY(VU_UDFS.enc(mudprodcode), "Ud*yog+1993");
                                }

                                param = lcmd.Parameters.Add("@encValue", SqlDbType.VarChar);
                                param.Value = ldv.ToTable().Rows[j]["encValue"].ToString().Trim();

                                this.ConnOpen();
                                //int result = (int)lcmd.ExecuteScalar();
                                SqlDataReader dr = lcmd.ExecuteReader();
                                int resultID = dr.GetOrdinal("ret");
                                int ErrorMsgID = dr.GetOrdinal("ErrorMsg");
                                while (dr.Read())
                                {
                                    result = (int)dr[resultID];
                                    string ErrorMsg = (string)dr[ErrorMsgID];
                                }
                                dr.Close();
                                this.ConnClose();
                                switch (result)
                                {
                                    case 0:
                                        unFetRecords = unFetRecords + 1;
                                        throw new Exception("Option already exists.");
                                        break;
                                    case 1:
                                        unFetRecords = unFetRecords + 1;
                                        throw new Exception("Option not available in current Zip file. Kindly get new gst zip files.");
                                        break;
                                    case 2:
                                        throw new Exception("Feature Updated Sucessfully.");
                                        break;
                                    case 4:
                                        throw new Exception("Feature Updated Sucessfully.");
                                        break;
                                    case 5:
                                        unFetRecords = unFetRecords + 1;
                                        throw new Exception("Issue occured in execution." + Environment.NewLine + ErrorMsg);
                                    default:
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                //unFetRecords = unFetRecords + 1;
                                if (result == 5)
                                {
                                    //dr.Close();
                                    this.ConnClose();
                                }
                                DataRow ldr = LogTable.NewRow();
                                ldr["co_name"] = ds.Tables["CO_mast_vw"].Rows[i]["co_name"].ToString().Trim();
                                ldr["OptType"] = ldv.ToTable().Rows[j]["OptType"].ToString();
                                ldr["OptName"] = ldv.ToTable().Rows[j]["OptName"].ToString();
                                ldr["LogDesc"] = ex.Message;
                                LogTable.Rows.Add(ldr);
                                ldr = null;
                            }
                        }
                        this.ConnClose();
                        ldv.RowFilter = "";
                        ProcessEnd = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                        if ((totRecords - unFetRecords) > 0)                        //Added by Shrikant S. on 03/04/2019 for Registration
                            UpgradationCount++;                                     //Added by Shrikant S. on 03/04/2019 for Registration    

                        WriteLog(ds.Tables["CO_mast_vw"].Rows[i]["co_name"].ToString().Trim(), 2);
                        ClearTables();

                    }
                }

                //MessageBox.Show("Testing - ExecuteScripts_New - 4");
            }
            catch (Exception ex)
            {
                ErrorMsg = "Error occured while updating features." + Environment.NewLine + ex.Message;
                return false;
            }

            //MessageBox.Show("Testing - ExecuteScripts_New - 5");

            return true;
        }
        private bool DoMenuUpdate_New()
        {
            if (_IgnoreStandardRule == true)
                return true;

            SqlCommand lcmd;
            string sqlStr = string.Empty;
            string[] lProducts;
            DataRow[] ldrs;
            try
            {
                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    string companyName = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim();
                    string companyDB = ds.Tables["Co_mast_vw"].Rows[k]["DBName"].ToString().ToUpper().Trim();

                    DataRow[] CustInfoRows;
                    if (this.CurrentAppFile != _UpgradeAppFile)
                        CustInfoRows = upgradeInfData.Select("clientnm='" + companyName + "'");       // Added by Shrikant S. on 23/07/2019 for Old Registration Process
                    //CustInfoRows = upgradeInfData.Select("clientnm='" + stringList[0] + "'");         // Commented by Shrikant S. on 23/07/2019 for Old Registration Process
                    else
                        CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + companyName + "'");     // Added by Shrikant S. on 23/07/2019 for Old Registration Process
                    //CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + stringList[0] + "'");       // Commented by Shrikant S. on 23/07/2019 for Old Registration Process

                    for (int i = 0; i < CustInfoRows.Count(); i++)
                    {

                        //pbar.ShowProgress("Inserting Features for company:" + companyName, 0);
                        //this.ShowProcess("Updating menus...");
                        this.status = "Updating menus for company:" + companyName;
                        this.oWorker.ReportProgress(this.progressPercentage);
                        try
                        {
                            sqlStr = "Update " + companyDB + "..Com_menu Set LabKey=''";
                            lcmd = new SqlCommand(sqlStr, conn);
                            this.ConnOpen();
                            lcmd.ExecuteNonQuery();
                            this.ConnClose();
                        }
                        catch
                        {
                            MessageBox.Show("Unable to update Menu Table." + Environment.NewLine + "Please contact your software vendor.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        //lProducts = CustInfoRows[i]["ProdCd"].ToString().Split(',');
                        //string lstProducts = string.Empty;
                        //for (int l = 0; l < lProducts.Count(); l++)
                        //{
                        //    lstProducts = lstProducts + "'" + lProducts[l] + "',";
                        //}
                        //lstProducts = (lstProducts.Length != 0 ? lstProducts.Substring(0, lstProducts.Length - 1) : "");

                        string selectedModuleList = string.Empty;
                        string selectedAddiModuleList = string.Empty;
                        string lstProducts = string.Empty;
                        string prodlist = string.Empty;

                        string cProduct = Utilities.GetDecProductCode(ds.Tables["Co_mast_vw"].Rows[k]["Passroute"].ToString());
                        int j = 0;
                        //while (j + 5 <= cProduct.Length)
                        //{
                        //    prodlist = prodlist + cProduct.Substring(j, 5) + ",";
                        //    j = j + 5;
                        //}
                        for (int a = 0; a < ds.Tables["Module_vw"].Rows.Count; a++)
                        {
                            if (cProduct.Contains(ds.Tables["Module_vw"].Rows[a]["cMainProdCode"].ToString().Trim()))
                            {
                                prodlist = prodlist + (prodlist.Contains(ds.Tables["Module_vw"].Rows[a]["cMainProdCode"].ToString().Trim()) ? "" : ds.Tables["Module_vw"].Rows[a]["cMainProdCode"].ToString().Trim() + ",");
                            }
                        }
                        //DataRow[] selectedProds = ds.Tables["cProduct_vw"].Select("CompId=" + ds.Tables["Co_mast_vw"].Rows[k]["CompId"].ToString());
                        //for (int n = 0; n < selectedProds.Count(); n++)
                        //{
                        //    prodlist = prodlist + selectedProds[n]["cProduct"].ToString() + ",";
                        //}

                        lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int l = 0; l < lProducts.Count(); l++)
                        {
                            lstProducts = lstProducts + "'" + lProducts[l] + "',";
                        }
                        lstProducts = (lstProducts.Length != 0 ? lstProducts.Substring(0, lstProducts.Length - 1) : "");

                        ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + ((this.CurrentAppFile != _UpgradeAppFile) ? upgradeProdcode : mudprodcode) + "' and servicever='NORMAL' and fType='PREMIUM' and OptionType='MENU'");         //Added by Shrikant S. on 30/05/2019 for Registration   
                        //ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + mudprodcode + "' and servicever='NORMAL' and fType='PREMIUM' and OptionType='MENU'");         //Added by Shrikant S. on 06/02/2019 for Registration
                        //ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + RegProd + "' and servicever='NORMAL' and fType='PREMIUM' and OptionType='MENU'");       //Commented by Shrikant S. on 06/02/2019 for Registration

                        foreach (DataRow ldr in ldrs)
                        {
                            DataRow[] freeFeatureRows = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + ((this.CurrentAppFile != _UpgradeAppFile) ? upgradeProdcode : mudprodcode) + "' and servicever='NORMAL' and fType='Free' and OptionType='MENU' and OptionName='" + ldr["OptionName"].ToString().Trim() + "'");         //Added by Shrikant S. on 30/05/2019 for Registration    
                            if (freeFeatureRows.Length > 0)
                                continue;

                            //pbar.ShowProgress("Inserting Features for company:" + companyName, Convert.ToInt32(j * ratio)); 
                            sqlStr = "If Exists (Select Top 1 enc From " + companyDB + "..Clientfeature Where Enc=@encValue) ";
                            sqlStr = sqlStr + " " + "Begin";
                            sqlStr = sqlStr + " " + "Update " + companyDB + "..Com_Menu set LabKey='SPREMIUM' Where ltrim(rtrim(Padname))+ltrim(rtrim(Barname))='" + ldr["OptionName"].ToString().Trim() + "'";
                            sqlStr = sqlStr + " " + "End";
                            sqlStr = sqlStr + " " + "Else";
                            sqlStr = sqlStr + " " + "Begin";
                            sqlStr = sqlStr + " " + "Update " + companyDB + "..Com_Menu set LabKey='UPREMIUM' Where ltrim(rtrim(Padname))+ltrim(rtrim(Barname))='" + ldr["OptionName"].ToString().Trim() + "'";
                            sqlStr = sqlStr + " " + "End";

                            lcmd = new SqlCommand(sqlStr, conn);
                            SqlParameter param = lcmd.Parameters.Add("@encValue", SqlDbType.NVarChar);

                            param.Value = oConnect.RetEncValue(txtCo_name.Text.Trim() + "~*0*~" + ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim() + "~*1*~" + ldr["FeatureId"].ToString().Trim() + "~*2*~" + companyName, ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim());

                            this.ConnOpen();
                            lcmd.ExecuteNonQuery();
                            this.ConnClose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Error occured while updating menu." + Environment.NewLine + ex.Message;
                return false;
            }
            return true;
        }

        private bool InsertFeature_New()
        {
            SqlCommand lcmd;
            string sqlStr = string.Empty;
            //MessageBox.Show("Testing - InsertFeature_New - 1");

            try
            {
                //MessageBox.Show("Testing - InsertFeature_New - 2");
                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    string companyName = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim();
                    string companyDB = ds.Tables["Co_mast_vw"].Rows[k]["DBName"].ToString().ToUpper().Trim();
                    DataRow[] CustInfoRows;
                    if (this.CurrentAppFile != _UpgradeAppFile)
                        CustInfoRows = upgradeInfData.Select("clientnm='" + companyName + "'");       //Added by Shrikant S. on 23/07/2019 for Old Registration Process
                    //CustInfoRows = upgradeInfData.Select("clientnm='" + stringList[0] + "'");         //Commeneted by Shrikant S. on 23/07/2019 for Old Registration Process
                    else
                        CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + companyName + "'");     //Added by Shrikant S. on 23/07/2019 for Old Registration Process
                    //CustInfoRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + stringList[0] + "'");       //Commeneted by Shrikant S. on 23/07/2019 for Old Registration Process      

                    for (int i = 0; i < CustInfoRows.Count(); i++)
                    {
                        string selectedModuleList = string.Empty;
                        string selectedAddiModuleList = string.Empty;

                        this.status = "Updating feature for:" + companyName;
                        this.oWorker.ReportProgress(this.progressPercentage);

                        string prodlist = string.Empty;
                        string lstProducts = string.Empty;

                        prodlist = Utilities.GetDecProductCode(ds.Tables["Co_mast_vw"].Rows[k]["Passroute"].ToString());


                        for (int a = 0; a < ds.Tables["Module_vw"].Rows.Count; a++)
                        {
                            if (prodlist.Contains(ds.Tables["Module_vw"].Rows[a]["cMainProdCode"].ToString().Trim()))
                            {
                                selectedModuleList = selectedModuleList + (selectedModuleList.Contains(ds.Tables["Module_vw"].Rows[a]["cMainProdCode"].ToString().Trim()) ? "" : ds.Tables["Module_vw"].Rows[a]["cMainProdCode"].ToString().Trim() + ",");
                            }
                        }

                        selectedModuleList = (selectedModuleList.Length != 0 ? selectedModuleList.Substring(0, selectedModuleList.Length - 1) : "");

                        prodlist = Utilities.GetDecProductCode(ds.Tables["Co_mast_vw"].Rows[k]["Passroute1"].ToString());
                        string[] lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int t = 0; t < lProducts.Length; t++)
                        {
                            selectedAddiModuleList = selectedAddiModuleList + lProducts[t].ToString() + ",";
                        }

                        selectedAddiModuleList = (selectedAddiModuleList.Length != 0 ? selectedAddiModuleList.Substring(0, selectedAddiModuleList.Length - 1) : "");


                        if (_IgnoreStandardRule == false)                    // Added by Shrikant S. on 07/05/2018 for Bug-31515
                        {
                            //Commented by Shrikant S. on 01/08/2019 for Old Registration Process       //Start
                            //sqlStr = "Delete From " + companyDB + "..ClientFeature";
                            //lcmd = new SqlCommand(sqlStr, conn);
                            //this.ConnOpen();
                            //lcmd.ExecuteNonQuery();
                            //this.ConnClose();
                            //Commented by Shrikant S. on 01/08/2019 for Old Registration Process       //End

                            //string[] lfeatureIds = CustInfoRows[i]["FeatureId"].ToString().Split(',');

                            string filt = string.Format("compname='{0}'", ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim());
                            DataRow[] comps = Complist.Select(filt);
                            if (comps.Count() > 0)
                            {
                                selectedModuleList = string.Empty;
                                selectedAddiModuleList = string.Empty;
                                prodlist = CustInfoRows[i]["ProdCd"].ToString() + "," + CustInfoRows[i]["AddProdCd"].ToString();
                                lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                string[] seleModules = comps[0]["selmainmodule"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                foreach (string item in seleModules)
                                {
                                    if (lProducts.Contains(item))
                                    {
                                        selectedModuleList = selectedModuleList + item + ",";
                                    }
                                }
                                selectedModuleList = (selectedModuleList.Length != 0 ? selectedModuleList.Substring(0, selectedModuleList.Length - 1) : "");

                                seleModules = comps[0]["selmodcode"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                foreach (string item in seleModules)
                                {
                                    if (lProducts.Contains(item))
                                    {
                                        selectedAddiModuleList = selectedAddiModuleList + item + ",";
                                    }
                                }
                                selectedAddiModuleList = (selectedAddiModuleList.Length != 0 ? selectedAddiModuleList.Substring(0, selectedAddiModuleList.Length - 1) : "");
                            }

                            //prodlist = selectedModuleList + selectedAddiModuleList;
                            prodlist = selectedModuleList + (selectedAddiModuleList == "" ? "" : ",") + selectedAddiModuleList;     // Changed by Sachin N. S. on 03/01/2019 for Bug-32837
                            lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int l = 0; l < lProducts.Count(); l++)
                            {
                                lstProducts = lstProducts + "'" + lProducts[l] + "',";
                            }
                            lstProducts = (lstProducts.Length != 0 ? lstProducts.Substring(0, lstProducts.Length - 1) : "");

                            DataRow[] lfeatureIds;
                            if (this.CurrentAppFile != _UpgradeAppFile)
                                lfeatureIds = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + upgradeProdcode + "' and servicever='" + cboServiceVer.Text.ToUpper().Trim() + "'");
                            else
                                lfeatureIds = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + mudprodcode + "' and servicever='" + cboServiceVer.Text.ToUpper().Trim() + "'");

                            for (int j = 0; j < lfeatureIds.Length; j++)
                            {
                                //pbar.ShowProgress("Inserting Features for company:" + companyName, Convert.ToInt32(j * ratio)); 
                                sqlStr = "If Not Exists (Select top 1 enc From " + companyDB + "..Clientfeature Where Enc=@encValue)  Begin Insert Into " + companyDB + "..ClientFeature (enc) values (@encValue) End";
                                lcmd = new SqlCommand(sqlStr, conn);
                                SqlParameter param = lcmd.Parameters.Add("@encValue", SqlDbType.NVarChar);
                                param.Value = oConnect.RetEncValue(txtCo_name.Text.Trim() + "~*0*~" + ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim() + "~*1*~" + lfeatureIds[j]["featureId"].ToString().Trim().PadLeft(10, '0') + "~*2*~" + companyName, ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim());

                                this.ConnOpen();
                                lcmd.ExecuteNonQuery();
                                this.ConnClose();

                            }
                        }

                        //Added by Shrikant S. on 03/04/2019 for Registration       //Start
                        if (!this.UpdateLotherDcMastRecords(companyDB))
                        {
                            return false;
                        }
                        if (!this.UpdateIndustryScript(companyDB, selectedModuleList, selectedAddiModuleList))
                        {
                            return false;
                        }
                        //Added by Shrikant S. on 03/04/2019 for Registration       //End

                        DoCompanyProductUpgrade(selectedModuleList, selectedAddiModuleList, "", " Compid= " + ds.Tables["Co_mast_vw"].Rows[k]["CompId"].ToString());


                    }
                }

                //MessageBox.Show("Testing - InsertFeature_New - 3");

            }
            catch (Exception ex)
            {
                ErrorMsg = "Error occured while updating features." + Environment.NewLine + ex.Message;
                return false;
            }

            //MessageBox.Show("Testing - InsertFeature_New - 4");

            //pbar.Dispose();
            return true;

        }
        private bool UpdateIndustryScript(string companyDBName, string selectedModules, string selectedaddimodules)
        {
            SqlCommand lcmd;
            SqlDataAdapter lda;
            string connString = readIni.ConnectionString.Replace("VUDYOG", companyDBName);
            SqlConnection con = new SqlConnection(connString);
            string sqlStr = "Select * From Vudyog..AddiScriptDet";
            string scriptFileName = string.Empty;
            try
            {
                lcmd = new SqlCommand(sqlStr, con);
                lda = new SqlDataAdapter(lcmd);
                DataTable ldt = new DataTable();
                lda.Fill(ldt);
                for (int i = 0; i < ldt.Rows.Count; i++)
                {
                    DataRow[] selectedProds = ds.Tables["Product_vw"].Select("Sel=True");
                    for (int j = 0; j < selectedProds.Count(); j++)
                    {
                        if (selectedaddimodules.Contains(selectedProds[j]["cProdCode"].ToString().Trim()))
                        {
                            if (ldt.Rows[i]["ProdName"].ToString().Trim() == selectedProds[j]["cProdName"].ToString().Trim())
                            {
                                scriptFileName = Path.Combine(this.appPath, ldt.Rows[i]["ScrFileNm"].ToString().Trim());
                                if (File.Exists(scriptFileName))
                                {
                                    string fileContent = System.IO.File.ReadAllText(scriptFileName);
                                    if (fileContent.Trim().Length > 0)
                                    {
                                        con.Open();
                                        lcmd.CommandText = fileContent;
                                        lcmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Error occured while updating industry script file: " + scriptFileName + Environment.NewLine + ex.Message;
                return false;
            }
            return true;
        }
        private bool UpdateLotherDcMastRecords(string companyDbName)
        {
            SqlCommand lcmd;
            string sqlStr = " Execute " + companyDbName + "..USP_update_Lother_dcmast '" + zipFileName + "'";

            try
            {
                lcmd = new SqlCommand(sqlStr, conn);
                this.ConnOpen();
                lcmd.ExecuteNonQuery();
                this.ConnClose();
            }
            catch (Exception ex)
            {
                ErrorMsg = "Error occured while updating Lother/DcMast." + Environment.NewLine + ex.Message;
                return false;
            }
            return true;
        }
        //Added by Shrikant S. on 04/02/2019 for Registration       //End

        private bool DoProductUpgrade()
        {
            UpdateTbl = new DataTable("Update_vw");
            DataRow[] ldrs;
            DataRow updateRow;
            UpdateTbl.Columns.Add("CompId", typeof(System.Int32));
            UpdateTbl.Columns.Add("Co_name", typeof(System.String));
            UpdateTbl.Columns.Add("OptType", typeof(System.String));
            UpdateTbl.Columns.Add("SqlQuery", typeof(System.String));
            UpdateTbl.Columns.Add("encValue", typeof(System.String));
            UpdateTbl.Columns.Add("OptName", typeof(System.String));
            UpdateTbl.Columns.Add("OptUpdate", typeof(int));

            if (ds.Tables["Product_vw"] != null)
            {
                ds.Tables["Product_vw"].Clear();
            }
            if (ds.Tables["cProduct_vw"] != null)
            {
                ds.Tables["cProduct_vw"].Clear();
            }
            string[] lProducts;
            try
            {
                //GetFeatures();

                //pbar.Show();
                //this.ShowProcess("Genearating Scripts..");
                this.status = "Please wait... Generating Scripts...";
                //Thread.Sleep(10);
                this.oWorker.ReportProgress(this.progressPercentage);

                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    for (int i = 0; i < ds.Tables["CustInfo_vw"].Rows.Count; i++)
                    {
                        //string defaData = (ds.Tables["CustInfo_vw"].Rows[i]["Prodcd"].ToString().IndexOf("vutex") >= 0 ? "NXIO" : "NEIO");        //Commented by Shrikant S. on 14/03/2018 
                        //string defaData = "GST";      //Added by Shrikant S. on 14/03/2018        //Commented by Shrikant S. on 04/02/2019 for Registration
                        string defaData = zipFileName;      //Added by Shrikant S. on 04/02/2019 for Registration
                        //string compRights = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().ToUpper().Trim() + "(" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["sta_dt"]).Year.ToString() + "-" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["end_dt"]).Year.ToString() + ")[ADMINISTRATOR]"; //Commented by Prajakta B. on 15032018 for Bug 31351
                        string compRights = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim().Replace("'", "''") + "(" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["sta_dt"]).Year.ToString() + "-" + Convert.ToDateTime(ds.Tables["Co_mast_vw"].Rows[k]["end_dt"]).Year.ToString() + ")[ADMINISTRATOR]"; //Added by Prajakta B.on 15032018 for Bug 31351
                        if (ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().ToUpper().Trim() == ds.Tables["CustInfo_vw"].Rows[i]["clientnm"].ToString().ToUpper().Trim())
                        {

                            //lProducts = ds.Tables["CustInfo_vw"].Rows[i]["ProdCd"].ToString().Split(',');         //Commented by Shrikant S. on 11/08/2014 for Bug-23814
                            string prodlist = ds.Tables["CustInfo_vw"].Rows[i]["ProdCd"].ToString() + "," + ds.Tables["CustInfo_vw"].Rows[i]["addProdCd"].ToString();       //Added by Shrikant S. on 11/08/2014 for Bug-23814
                            lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);              //Added by Shrikant S. on 11/08/2014 for Bug-23814

                            string lstProducts = string.Empty;
                            for (int l = 0; l < lProducts.Count(); l++)
                            {
                                lstProducts = lstProducts + "'" + lProducts[l] + "',";
                            }
                            lstProducts = (lstProducts.Length != 0 ? lstProducts.Substring(0, lstProducts.Length - 1) : "");
                            ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + RegProd + "' and servicever='" + cboServiceVer.Text.ToUpper().Trim() + "'");
                            foreach (DataRow ldr in ldrs)
                            {
                                updateRow = UpdateTbl.NewRow();
                                updateRow["CompId"] = ds.Tables["Co_mast_vw"].Rows[k]["CompId"];
                                updateRow["Co_Name"] = ds.Tables["Co_mast_vw"].Rows[k]["Co_Name"];
                                updateRow["OptType"] = ldr["OptionType"];
                                updateRow["OptName"] = ldr["OptionName"].ToString();
                                updateRow["OptUpdate"] = 0;
                                if (this.CurrentAppFile != _UpgradeAppFile)
                                {
                                    updateRow["OptUpdate"] = 1;
                                }

                                // Added by Shrikant S. on 15/05/2018 for Bug-31515     //Start
                                if (_IgnoreStandardRule == true)
                                {
                                    string cond = string.Format("Para4='{0}'", this.EscapeSpecialCharacters(ldr["OptionName"].ToString().Trim()));
                                    //if (ldr["OptionName"].ToString().Trim() == "PURCHASETRANSACTIONSLETTEROFCREDIT")
                                    //{
                                    //    int a = 1;
                                    //}
                                    try
                                    {
                                        DataRow[] optionRow = optionTable.Select(cond);
                                        if (optionRow.Count() > 0)
                                        {
                                            switch (ldr["OptionType"].ToString())
                                            {
                                                case "REPORT":
                                                    updateRow["encValue"] = oConnect.RetEncValue(mudprodcode + "<~*0*~>" + optionRow[0]["para1"].ToString().Trim() + "<~*1*~>" + optionRow[0]["para2"].ToString().Trim() + "<~*2*~>" + optionRow[0]["para3"].ToString().Trim(), "Udencyogprod");
                                                    break;
                                                case "TRANSACTION":
                                                    updateRow["encValue"] = oConnect.RetEncValue(mudprodcode + "<~*0*~>" + optionRow[0]["para1"].ToString().Trim(), "Udencyogprod");
                                                    break;
                                                case "MENU":
                                                    updateRow["encValue"] = oConnect.RetEncValue(mudprodcode + "<~*0*~>" + optionRow[0]["para1"].ToString().Trim() + "<~*1*~>" + optionRow[0]["para2"].ToString().Trim() + "<~*2*~>" + optionRow[0]["para3"].ToString().Trim(), "Udencyogprod");
                                                    break;
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }
                                else
                                {
                                    updateRow["encValue"] = oConnect.RetEncValue(ldr["FeatureId"].ToString().Trim() + "~*0*~" + ldr["sfeatureid"].ToString().Trim() + "~*1*~" + ldr["OptionName"].ToString().Trim() + "~*2*~", "Udencyogprod");
                                }
                                // Added by Shrikant S. on 15/05/2018 for Bug-31515     //Start
                                //updateRow["encValue"] = oConnect.RetEncValue(ldr["FeatureId"].ToString().Trim() + "~*0*~" + ldr["sfeatureid"].ToString().Trim() + "~*1*~" + ldr["OptionName"].ToString().Trim() + "~*2*~", "Udencyogprod");     //Commented by Shrikant S. on 15/05/2018 for Bug-31515     

                                // updateRow["SqlQuery"] = this.GenInsertUpdate(ds.Tables["Co_mast_vw"].Rows[k]["dbname"].ToString().Trim(), ldr[4].ToString(), ldr[11].ToString(), defaData, compRights);  // Commented By Prajakta B. on 15032018 for Bug 31351
                                updateRow["SqlQuery"] = this.GenInsertUpdate(ds.Tables["Co_mast_vw"].Rows[k]["dbname"].ToString().Trim(), ldr[4].ToString(), ldr[11].ToString().Replace("'", "''"), defaData, compRights, Convert.ToInt32(updateRow["OptUpdate"]));  // Added By Prajakta B. on 15032018 for Bug 31351
                                UpdateTbl.Rows.Add(updateRow);
                            }
                        }
                    }
                }
                //pbar.Dispose();
            }
            catch (Exception ex)
            {
                //pbar.Dispose();
                this.ErrorMsg = ex.Message;
                return false;
            }
            return true;
        }

        private string GenInsertUpdate(string companyDB, string optType, string optValue, string defaultDataNm, string compforRights, int updateType)
        {
            string retStr = string.Empty;
            string cRights = string.Empty;
            string cRoles = string.Empty;

            switch (optType)
            {
                case "MENU":
                    cRights = Utilities.GetDecoder("IYCYDYPYVY", false).Trim();
                    cRoles = Utilities.GetDecoder("ADMINISTRATOR", true).Trim();
                    retStr = "Execute Usp_Int_GenFeature '" + companyDB + "','" + defaultDataNm + "','" + optType + "','" + optValue + "',@encValue,'" + compforRights + "','" + cRights + "','" + cRoles + "'" + (updateType == 1 ? ",1" : ",0");//+ ",@pEnc";
                    break;
                case "TRANSACTION":
                    retStr = "Execute Usp_Int_GenFeature '" + companyDB + "','" + defaultDataNm + "','" + optType + "','" + optValue + "',@encValue,'" + compforRights + "','" + cRights + "','" + cRoles + "'" + (updateType == 1 ? ",1" : ",0");//+ ",@pEnc";
                    break;
                case "REPORT":
                    retStr = "Execute Usp_Int_GenFeature '" + companyDB + "','" + defaultDataNm + "','" + optType + "','" + optValue + "',@encValue,'" + compforRights + "','" + cRights + "','" + cRoles + "'" + (updateType == 1 ? ",1" : ",0");//+ ",@pEnc";
                    break;
            }
            return retStr;
        }

        private bool ExecuteScripts()
        {
            string companyFolder = string.Empty;
            string LogFile = string.Empty;
            SqlParameter param;
            SqlCommand lcmd;
            string SQL = string.Empty;
            int result = 0;
            LogTable = new DataTable("Log_vw");

            LogTable.Columns.Add("Co_name", typeof(System.String));
            LogTable.Columns.Add("OptType", typeof(System.String));
            LogTable.Columns.Add("OptName", typeof(System.String));
            LogTable.Columns.Add("LogDesc", typeof(System.String));
            //UpdateTbl.WriteXml("d:\\abcd_160518.xml");
            DataView ldv = UpdateTbl.DefaultView;

            if (UpdateTbl != null)
            {
                UpdateTbl = null;
            }

            //pbar.Show();
            try
            {
                for (int i = 0; i < ds.Tables["CO_mast_vw"].Rows.Count; i++)
                {
                    for (int k = 0; k < ds.Tables["CustInfo_vw"].Rows.Count; k++)
                    {
                        if (ds.Tables["CO_mast_vw"].Rows[i]["Co_name"].ToString().ToUpper().Trim() == ds.Tables["CustInfo_vw"].Rows[k]["clientnm"].ToString().ToUpper().Trim())
                        {

                            totRecords = 0;
                            unFetRecords = 0;

                            ProcessStart = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                            companyFolder = ds.Tables["CO_mast_vw"].Rows[i]["foldername"].ToString().Trim();
                            CreateLogFile(ds.Tables["CO_mast_vw"].Rows[i]["co_name"].ToString().Trim(), companyFolder);

                            //ldv.RowFilter = "Co_name='" + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim()+"'";  //Commented by Prajakta B. on 12032018 for Bug 31351
                            ldv.RowFilter = "Co_name='" + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim().Replace("'", "''") + "'";   //Added by Prajakta B. on 12032018 for Bug 31351

                            ldv.Sort = "Co_name,OptType";
                            totRecords = ldv.ToTable().Rows.Count;

                            //pbar.ShowProgress("Executing Features for company: " + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim(), 0);
                            //this.ShowProcess("Updating features for :" + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim());
                            this.status = "Updating features for company: " + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim();
                            this.oWorker.ReportProgress(this.progressPercentage);

                            for (int j = 0; j < ldv.ToTable().Rows.Count; j++)
                            {
                                try
                                {
                                    //pbar.ShowProgress("Executing Features for company: " + ds.Tables["CO_mast_vw"].Rows[i]["Co_Name"].ToString().Trim(), Convert.ToInt32(j * ratio));

                                    SQL = ldv.ToTable().Rows[j]["SqlQuery"].ToString();

                                    lcmd = new SqlCommand(SQL, conn);
                                    param = lcmd.Parameters.Add("@encValue", SqlDbType.VarChar);
                                    param.Value = ldv.ToTable().Rows[j]["encValue"].ToString().Trim();

                                    this.ConnOpen();
                                    //int result = (int)lcmd.ExecuteScalar();
                                    SqlDataReader dr = lcmd.ExecuteReader();
                                    int resultID = dr.GetOrdinal("ret");
                                    int ErrorMsgID = dr.GetOrdinal("ErrorMsg");
                                    while (dr.Read())
                                    {
                                        result = (int)dr[resultID];
                                        string ErrorMsg = (string)dr[ErrorMsgID];
                                    }
                                    dr.Close();
                                    this.ConnClose();
                                    switch (result)
                                    {
                                        case 0:
                                            unFetRecords = unFetRecords + 1;
                                            throw new Exception("Option already exists.");
                                            break;
                                        case 1:
                                            unFetRecords = unFetRecords + 1;
                                            throw new Exception("Option not available in current Zip file. Kindly get new gst zip files.");
                                            break;
                                        case 2:
                                            throw new Exception("Feature Updated Sucessfully.");
                                            break;
                                        case 4:
                                            throw new Exception("Feature Updated Sucessfully.");
                                            break;
                                        case 5:
                                            unFetRecords = unFetRecords + 1;
                                            throw new Exception("Issue occured in execution." + Environment.NewLine + ErrorMsg);
                                        default:
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //unFetRecords = unFetRecords + 1;
                                    if (result == 5)
                                    {
                                        //dr.Close();
                                        this.ConnClose();
                                    }
                                    DataRow ldr = LogTable.NewRow();
                                    ldr["co_name"] = ds.Tables["CO_mast_vw"].Rows[i]["co_name"].ToString().Trim();
                                    ldr["OptType"] = ldv.ToTable().Rows[j]["OptType"].ToString();
                                    ldr["OptName"] = ldv.ToTable().Rows[j]["OptName"].ToString();
                                    ldr["LogDesc"] = ex.Message;
                                    LogTable.Rows.Add(ldr);
                                    ldr = null;
                                }
                            }
                            this.ConnClose();
                            ldv.RowFilter = "";
                            ProcessEnd = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                            WriteLog(ds.Tables["CO_mast_vw"].Rows[i]["co_name"].ToString().Trim(), 2);
                            ClearTables();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Error occured while updating features." + Environment.NewLine + ex.Message;
                return false;
            }
            return true;
        }

        private void CreateLogFile(string companyName, string companyFolder)
        {
            if (!System.IO.Directory.Exists(appPath + "\\" + companyFolder + "\\" + "Upgrade_Log"))        // Added By Shrikant S. on 31/03/2012 for Bug-3228
            {
                System.IO.Directory.CreateDirectory(appPath + "\\" + companyFolder + "\\" + "Upgrade_Log");
            }

            //LogFile = appPath + "\\" + companyFolder + "\\Upgrade_Log\\Upgrade_Log_" + companyName + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".uLog";        // Commented by Sachin N. S. on 28/01/2020 for Bug-32837
            string _compNm = Regex.Replace(companyName, @"[\\/:*?<>|""]", "_");     // Added by Sachin N. S. on 28/01/2020 for Bug-32837
            LogFile = appPath + "\\" + companyFolder + "\\Upgrade_Log\\Upgrade_Log_" + _compNm + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".uLog";      // Added by Sachin N. S. on 28/01/2020 for Bug-32837
            if (!File.Exists(LogFile))
            {
                WriteLog(companyName, 1);
                LogFileList.Add(LogFile);
            }
        }

        private void WriteLog(string company, int type)
        {
            int xWidth = 200;
            //this.ShowProcess("Writing Log for " + company);

            switch (type)
            {
                case 1:
                    using (FileStream file = new FileStream(LogFile, FileMode.Append, FileAccess.Write))
                    {
                        StreamWriter streamWriter = new StreamWriter(file);
                        streamWriter.WriteLine("".PadRight(xWidth, '='));
                        int x = (xWidth - ("Auto Generated Log For:").Length - company.Length) / 2;
                        streamWriter.WriteLine(" ".PadLeft(x) + "Auto Generated Log For:" + company);
                        streamWriter.Close();
                    }
                    break;
                case 2:
                    using (FileStream file = new FileStream(LogFile, FileMode.Append, FileAccess.Write))
                    {
                        StreamWriter streamWriter = new StreamWriter(file);
                        streamWriter.WriteLine("".PadRight(xWidth, '='));
                        streamWriter.WriteLine(" ".PadLeft((xWidth - ("Log Summary").Length) / 2) + "Log Summary");
                        streamWriter.WriteLine("".PadRight(xWidth, '='));

                        streamWriter.WriteLine("Process Start:" + ProcessStart);
                        streamWriter.WriteLine("Total Features:" + totRecords.ToString());
                        streamWriter.WriteLine("Total Updated:" + (totRecords - unFetRecords).ToString());
                        streamWriter.WriteLine("Total Not Updated:" + unFetRecords.ToString());
                        streamWriter.WriteLine("Process End:" + ProcessEnd);

                        if (LogTable.Rows.Count > 0)
                        {
                            DataView ldv = LogTable.DefaultView;
                            ldv.Sort = "LogDesc";
                            streamWriter.WriteLine("".PadRight(xWidth, '='));
                            streamWriter.WriteLine(" ".PadLeft((xWidth - ("Log Details").Length) / 2) + "Log Details");
                            streamWriter.WriteLine("".PadRight(xWidth, '='));
                            streamWriter.WriteLine(("Option Type").PadRight(15, ' ') + ("Option Name").PadRight(150, ' ') + ("Description").PadRight(55, ' '));
                            for (int i = 0; i < ldv.Count; i++)
                            {
                                streamWriter.WriteLine((ldv[i]["OptType"].ToString().Trim()).PadRight(15, ' ') + (ldv[i]["OptName"].ToString().Trim()).PadRight(150, ' ') + (ldv[i]["LogDesc"].ToString().Trim()).PadRight(55, ' '));
                                //streamWriter.WriteLine((LogTable.Rows[i]["OptType"].ToString().Trim()).PadRight(15, ' ') + (LogTable.Rows[i]["OptName"].ToString().Trim()).PadRight(55, ' ') + (LogTable.Rows[i]["LogDesc"].ToString().Trim()).PadRight(55, ' '));
                            }
                        }
                        streamWriter.Close();
                    }
                    break;
            }
        }
        private void ClearTables()
        {
            if (LogTable != null)
                LogTable.Clear();
        }
        private bool InsertFeature()
        {
            SqlCommand lcmd;
            string sqlStr = string.Empty;
            //pbar.Show();
            try
            {
                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    string companyName = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim();
                    string companyDB = ds.Tables["Co_mast_vw"].Rows[k]["DBName"].ToString().ToUpper().Trim();
                    for (int i = 0; i < ds.Tables["CustInfo_vw"].Rows.Count; i++)
                    {
                        if (companyName.ToUpper() == ds.Tables["CustInfo_vw"].Rows[i]["clientnm"].ToString().ToUpper().Trim())
                        {
                            //pbar.ShowProgress("Inserting Features for company:" + companyName, 0);
                            //this.ShowProcess("Updating feature for:" + companyName);
                            this.status = "Updating feature for:" + companyName;
                            this.oWorker.ReportProgress(this.progressPercentage);

                            if (_IgnoreStandardRule == false)                    // Added by Shrikant S. on 07/05/2018 for Bug-31515
                            {
                                sqlStr = "Delete From " + companyDB + "..ClientFeature";
                                lcmd = new SqlCommand(sqlStr, conn);
                                this.ConnOpen();
                                lcmd.ExecuteNonQuery();
                                this.ConnClose();

                                string[] lfeatureIds = ds.Tables["CustInfo_vw"].Rows[i]["FeatureId"].ToString().Split(',');

                                for (int j = 0; j < lfeatureIds.Length; j++)
                                {
                                    //pbar.ShowProgress("Inserting Features for company:" + companyName, Convert.ToInt32(j * ratio)); 
                                    sqlStr = "If Not Exists (Select top 1 enc From " + companyDB + "..Clientfeature Where Enc=@encValue)  Begin Insert Into " + companyDB + "..ClientFeature (enc) values (@encValue) End";
                                    lcmd = new SqlCommand(sqlStr, conn);
                                    SqlParameter param = lcmd.Parameters.Add("@encValue", SqlDbType.NVarChar);
                                    param.Value = oConnect.RetEncValue(txtCo_name.Text.Trim() + "~*0*~" + ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim() + "~*1*~" + lfeatureIds[j].Trim().PadLeft(10, '0') + "~*2*~" + companyName, ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim());

                                    this.ConnOpen();
                                    lcmd.ExecuteNonQuery();
                                    this.ConnClose();

                                }
                            }
                            //Added by Shrikant S. on 03/04/2019 for Registration       //Start
                            if (!this.UpdateLotherDcMastRecords(companyDB))
                            {
                                return false;
                            }
                            //Added by Shrikant S. on 03/04/2019 for Registration       //End

                            DoCompanyProductUpgrade(ds.Tables["CustInfo_vw"].Rows[i]["Prodcd"].ToString().Trim(), ds.Tables["CustInfo_vw"].Rows[i]["AddProdCd"].ToString().Trim(), ds.Tables["CustInfo_vw"].Rows[i]["VatStates"].ToString().Trim(), " Compid= " + ds.Tables["Co_mast_vw"].Rows[k]["CompId"].ToString());

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Error occured while updating features." + Environment.NewLine + ex.Message;
                return false;
            }
            //pbar.Dispose();
            return true;
        }
        private void ConnOpen()
        {
            if (this.conn != null)
            {
                if (this.conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
            }
            else
            {
                this.conn = new SqlConnection(readIni.ConnectionString);
                this.conn.Open();
            }
        }
        private void ConnClose()
        {
            if (this.conn != null)
            {
                if (this.conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void DoCompanyProductUpgrade(string companyProd, string compAddProd, string compVatStates, string cond)
        {
            SqlCommand lcmd;
            string sqlStr = string.Empty;
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            //MessageBox.Show("Testing -1");

            //this.ShowProcess("Updating products...");
            this.status = "Updating products...";
            this.oWorker.ReportProgress(this.progressPercentage);

            // ****** Added by Sachin N. S. on 27/12/2019 for Bug-32837 -- Start
            sqlStr = "Select Top 1 Co_name,dbname from co_mast Where " + cond + " and enddir<>'' ";
            lcmd = new SqlCommand(sqlStr, conn);
            SqlDataAdapter da = new SqlDataAdapter(lcmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            //MessageBox.Show("Testing -2");

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    sqlStr = "";
                    foreach (DataRow _dr in dt.Rows)
                    {
                        sqlStr += " Update Co_mast set Passroute=Convert(Varbinary(max),@Pass), Passroute1=Convert(Varbinary(max),@Pass1),Prodcode=Convert(Varbinary(250),@Prodcode) Where CO_Name='" + _dr["Co_Name"].ToString().Trim() + "' and dbName='" + _dr["DbName"].ToString().Trim() + "' ";
                    }

                    lcmd = new SqlCommand(sqlStr, conn);
                    SqlParameter param11 = lcmd.Parameters.Add("@Pass", SqlDbType.VarChar);
                    param11.Value = Utilities.GetEncProductCode(companyProd);

                    SqlParameter param21 = lcmd.Parameters.Add("@Pass1", SqlDbType.VarChar);
                    param21.Value = Utilities.GetEncProductCode(compAddProd);
                    this.ConnOpen();
                    lcmd.ExecuteNonQuery();
                    this.ConnClose();
                }
            }
            // ****** Added by Sachin N. S. on 27/12/2019 for Bug-32837 -- End

            //MessageBox.Show("Testing -3");

            //sqlStr = "Update Co_mast set Passroute=Convert(Varbinary(250),@Pass), Passroute1=Convert(Varbinary(250),@Pass1),Prodcode=Convert(Varbinary(250),@Prodcode),Vatstates=@vatStates Where " + cond;       //Commented by Shrikant S. on 02/02/2019 for Registration
            sqlStr = "Update Co_mast set Passroute=Convert(Varbinary(max),@Pass), Passroute1=Convert(Varbinary(max),@Pass1),Prodcode=Convert(Varbinary(250),@Prodcode) Where " + cond;                              //Added by Shrikant S. on 02/02/2019 for Registration

            if (this.CurrentAppFile != _UpgradeAppFile)
            {
                sqlStr = sqlStr + " Update [User] set ProdCode=Convert(Varbinary(250),@Prodcode) Where ProdCode=Convert(Varbinary(250),@PrevProdcode) ";
                sqlStr = sqlStr + " Update [UserRoles] set ProdCode=Convert(Varbinary(250),@Prodcode) Where ProdCode=Convert(Varbinary(250),@PrevProdcode) ";
            }


            //MessageBox.Show("Testing -4");

            lcmd = new SqlCommand(sqlStr, conn);
            SqlParameter param1 = lcmd.Parameters.Add("@Pass", SqlDbType.VarChar);
            param1.Value = Utilities.GetEncProductCode(companyProd);

            SqlParameter param2 = lcmd.Parameters.Add("@Pass1", SqlDbType.VarChar);
            param2.Value = Utilities.GetEncProductCode(compAddProd);

            SqlParameter param3 = lcmd.Parameters.Add("@Prodcode", SqlDbType.VarChar);
            if (this.CurrentAppFile != _UpgradeAppFile)
            {
                param3.Value = VU_UDFS.NewENCRY(VU_UDFS.enc(upgradeProdcode), "Ud*yog+1993");
                SqlParameter param4 = lcmd.Parameters.Add("@PrevProdcode", SqlDbType.VarChar);
                param4.Value = oConnect.RetAppEnc(this.CurrentAppFile);
            }
            else
                param3.Value = oConnect.RetAppEnc(this.CurrentAppFile);


            this.ConnOpen();
            lcmd.ExecuteNonQuery();
            this.ConnClose();

            //MessageBox.Show("Testing -5");
        }

        private bool DoMenuUpdate()
        {
            if (_IgnoreStandardRule == true)                    // Added by Shrikant S. on 07/05/2018 for Bug-31515
                return true;                                    // Added by Shrikant S. on 07/05/2018 for Bug-31515

            SqlCommand lcmd;
            string sqlStr = string.Empty;
            string[] lProducts;
            DataRow[] ldrs;
            try
            {
                for (int k = 0; k < ds.Tables["Co_mast_vw"].Rows.Count; k++)
                {
                    string companyName = ds.Tables["Co_mast_vw"].Rows[k]["Co_name"].ToString().Trim();
                    string companyDB = ds.Tables["Co_mast_vw"].Rows[k]["DBName"].ToString().ToUpper().Trim();
                    for (int i = 0; i < ds.Tables["CustInfo_vw"].Rows.Count; i++)
                    {
                        if (companyName.ToUpper() == ds.Tables["CustInfo_vw"].Rows[i]["clientnm"].ToString().ToUpper().Trim())
                        {
                            //pbar.ShowProgress("Inserting Features for company:" + companyName, 0);
                            //this.ShowProcess("Updating menus...");
                            this.status = "Updating menus for company:" + companyName;
                            this.oWorker.ReportProgress(this.progressPercentage);
                            try
                            {
                                sqlStr = "Update " + companyDB + "..Com_menu Set LabKey=''";
                                lcmd = new SqlCommand(sqlStr, conn);
                                this.ConnOpen();
                                lcmd.ExecuteNonQuery();
                                this.ConnClose();
                            }
                            catch
                            {
                                MessageBox.Show("Unable to update Menu Table." + Environment.NewLine + "Please contact your software vendor.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            lProducts = ds.Tables["CustInfo_vw"].Rows[i]["ProdCd"].ToString().Split(',');
                            string lstProducts = string.Empty;
                            for (int l = 0; l < lProducts.Count(); l++)
                            {
                                lstProducts = lstProducts + "'" + lProducts[l] + "',";
                            }
                            lstProducts = (lstProducts.Length != 0 ? lstProducts.Substring(0, lstProducts.Length - 1) : "");
                            ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + RegProd + "' and servicever='NORMAL' and fType='PREMIUM' and OptionType='MENU'");

                            foreach (DataRow ldr in ldrs)
                            {
                                //pbar.ShowProgress("Inserting Features for company:" + companyName, Convert.ToInt32(j * ratio)); 
                                sqlStr = "If Exists (Select Top 1 enc From " + companyDB + "..Clientfeature Where Enc=@encValue) ";
                                sqlStr = sqlStr + " " + "Begin";
                                sqlStr = sqlStr + " " + "Update " + companyDB + "..Com_Menu set LabKey='SPREMIUM' Where ltrim(rtrim(Padname))+ltrim(rtrim(Barname))='" + ldr["OptionName"].ToString().Trim() + "'";
                                sqlStr = sqlStr + " " + "End";
                                sqlStr = sqlStr + " " + "Else";
                                sqlStr = sqlStr + " " + "Begin";
                                sqlStr = sqlStr + " " + "Update " + companyDB + "..Com_Menu set LabKey='UPREMIUM' Where ltrim(rtrim(Padname))+ltrim(rtrim(Barname))='" + ldr["OptionName"].ToString().Trim() + "'";
                                sqlStr = sqlStr + " " + "End";

                                lcmd = new SqlCommand(sqlStr, conn);
                                SqlParameter param = lcmd.Parameters.Add("@encValue", SqlDbType.NVarChar);
                                param.Value = oConnect.RetEncValue(txtCo_name.Text.Trim() + "~*0*~" + ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim() + "~*1*~" + ldr["FeatureId"].ToString().Trim() + "~*2*~" + companyName, ds.Tables["CustInfo_vw"].Rows[i]["macid"].ToString().Trim());

                                this.ConnOpen();
                                lcmd.ExecuteNonQuery();
                                this.ConnClose();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Error occured while updating menu." + Environment.NewLine + ex.Message;
                return false;
            }
            return true;
        }

        public bool ExecuteUpdates()
        {

            UtilityUpdate runUpdates = new UtilityUpdate(readIni.ConnectionString);
            runUpdates.ExecuteSystemUpdates(Assembly.GetExecutingAssembly().GetManifestResourceStream("ueProductUpgrade.Updates.xml"));
            this.ShowProcess("Executing system updates...");
            //this.status="Executing system updates...";
            for (int i = 0; i < ds.Tables["Co_Mast_vw"].Rows.Count; i++)
            {
                this.ShowProcess("Executing Company updates for :" + ds.Tables["Co_Mast_vw"].Rows[i]["Co_Name"].ToString().Trim());
                runUpdates.ExecuteCompanyUpdates(ds.Tables["Co_Mast_vw"].Rows[i]["Dbname"].ToString().Trim(), Assembly.GetExecutingAssembly().GetManifestResourceStream("ueProductUpgrade.Updates.xml"));
            }
            return true;
        }

        private void UpdateInfInfo()
        {
            //for (int i = 0; i < ds.Tables["Co_mast_vw"].Rows.Count; i++)
            //{
            //DataRow[] allProdList = ds.Tables["Product_vw"].Select("Sel=1 and CompId=" + ds.Tables["Co_mast_vw"].Rows[i]["CompId"].ToString());
            DataRow[] allProdList = ds.Tables["Product_vw"].Select("Sel=1");
            string _featlist = string.Empty;
            string _mainprdlist = string.Empty;
            string _addprdlist = string.Empty;
            foreach (DataRow item in allProdList)
            {
                if (item["cProdCode"].ToString().Trim() != item["cMainProdCode"].ToString().Trim())
                {
                    //_mainprdlist = _mainprdlist + "'" + item["cMainProdCode"].ToString().Trim() + "',";
                    _mainprdlist = _mainprdlist + (!_mainprdlist.Contains(item["cMainProdCode"].ToString().Trim()) ? "'" + item["cMainProdCode"].ToString().Trim() + "'," : "");
                }
                _addprdlist = _addprdlist + (!_addprdlist.Contains(item["cProdCode"].ToString().Trim()) ? "'" + item["cProdCode"].ToString().Trim() + "'," : "");
                //ldrs = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + RegProd + "' and servicever='" + cboServiceVer.Text.ToUpper().Trim() + "'");
            }
            string lstProducts = _mainprdlist + _addprdlist.Substring(0, _addprdlist.Length - 1);
            DataRow[] allfeatList = ds.Tables["FeatureDet_vw"].Select(" ProdCode In (" + lstProducts + ") and Prodver='" + RegProd + "' and servicever='" + cboServiceVer.Text.ToUpper().Trim() + "'");     //27/04/2019
                                                                                                                                                                                                            //var prodList = (from DataRow dRow in ds.Tables["FeatureDet_vw"].Rows select new { col1 = dRow["ProdVer"]}).Distinct();
            foreach (var feature in allfeatList)
            {
                //_featlist = _featlist + (!_featlist.Contains(feature["IntFeatureId"].ToString().Trim()) ? feature["IntFeatureId"].ToString() + "," : "");
                _featlist = _featlist + feature["IntFeatureId"].ToString().Trim() + ",";
            }
            _featlist = _featlist.Substring(0, _featlist.Length - 1);
            _mainprdlist = _mainprdlist.Substring(0, _mainprdlist.Length - 1);
            _addprdlist = _addprdlist.Substring(0, _addprdlist.Length - 1);
            //DataRow[] _InfRows = ds.Tables["CustInfo_vw"].Select("clientnm='" + ds.Tables["Co_mast_vw"].Rows[]["Co_Name"].ToString() + "'");
            DataRow[] _InfRows = ds.Tables["CustInfo_vw"].Select();
            if (_InfRows.Count() > 0)
            {
                for (int i = 0; i < _InfRows.Length; i++)
                {
                    _InfRows[i]["prodcd"] = _mainprdlist.Replace("'", "");
                    _InfRows[i]["featureid"] = _featlist;
                    _InfRows[i]["addprodcd"] = _addprdlist.Replace("'", "");
                }

            }
            //}
        }
        private void ShowProcess(string processMsg)
        {
            lblStatus.Text = "Please wait..." + Environment.NewLine + processMsg;
            lblStatus.Refresh();
        }

        private string GetRequestDetails()
        {
            conn = new SqlConnection(readIni.ConnectionString);
            string sqlStr = string.Empty;
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            sqlStr = "Select * From UpdateRequest";
            cmd.CommandText = sqlStr;
            DataTable reqtbl = new DataTable();
            da.SelectCommand = cmd;
            da.Fill(reqtbl);
            if (reqtbl.Rows.Count <= 0)
                return string.Empty;

            return reqtbl.Rows[0]["Reqprodupd"].ToString();
        }

        private string GetExtractString(string extractFrom, string tag)
        {
            Regex regex = new Regex("<" + tag + ">(.*?)</" + tag + ">");
            var str = regex.Match(extractFrom);
            return str.Groups[1].ToString();
        }

        private void GetProductCode(string appName)
        {
            string returnVal = string.Empty;
            //string cSql = "Select top 1 *,Upgradeto1=Convert(varchar(100),'') From Vudyog..ApplicationDet Where AppName='"+appName+"'";
            string cSql = "Select top 1 *,Upgradeto1=Convert(varchar(100),'') From Vudyog..ApplicationDet Where AppDesc='" + appName + "'";
            DataTable _appdt = oDataAccess.GetDataTable(cSql, null, 50);
            for (int i = 0; i < _appdt.Rows.Count; i++)
            {
                _appdt.Rows[i]["Upgradeto1"] = VU_UDFS.NewDECRY(VU_UDFS.HexDecimalToASCII(_appdt.Rows[i]["Upgradeto"].ToString()), "Ud*yog+1993Prod");
                upgradeProdcode = VU_UDFS.NewDECRY(VU_UDFS.HexDecimalToASCII(_appdt.Rows[i]["Upgradeto"].ToString()), "Ud*yog+1993Prod");
                upgradeAppTitle = _appdt.Rows[i]["Appdesc"].ToString().Trim();
                UpgradeApplication = _appdt.Rows[i]["Appdesc"].ToString().Trim();
                this.lblUpgradeApp1.Text = _appdt.Rows[i]["Appdesc"].ToString().Trim();
                _UpgradeAppFile = _appdt.Rows[i]["AppName"].ToString().Trim();
            }

        }

        private bool CheckUpgradeRegisterMe(string productName)
        {
            string prodInfo = this.GetProductInfo(productName);
            //DataRow[] prodrow = prodInfo.Select("AppName='"+ productName + "'");

            DirectoryInfo dir = new DirectoryInfo(appPath);
            //Array totalFile = dir.GetFiles("*register.Me");

            System.Linq.IOrderedEnumerable<FileInfo> totalFile = (from f in dir.GetFiles("*register.Me") orderby f.LastWriteTime descending select f);

            string m_registerMeFile = string.Empty;
            if (totalFile.Count() > 1)
            {
                for (int i = 0; i < totalFile.Count(); i++)
                {
                    m_registerMeFile = Path.GetFileName(totalFile.ElementAt(i).ToString());
                    ReadRegisterMefile oread = new ReadRegisterMefile();
                    string[,] filearr = oread.ReadFile(m_registerMeFile);
                    if (filearr[8, 1].Trim() != prodInfo.Trim())
                    {
                        m_registerMeFile = string.Empty;
                        continue;
                    }
                    else
                    {
                        if (Utilities.ReverseString(filearr[23, 1].Trim()) != GetMachineDetails.ProcessorId())
                        {
                            m_registerMeFile = string.Empty;
                            continue;
                        }
                        else
                        {
                            break;
                        }

                    }
                }
            }
            else
            {
                for (int j = 0; j < totalFile.Count(); j++)
                {
                    //Added by Shrikant S. on 05/04/2019 for Registration           //Start
                    m_registerMeFile = Path.GetFileName(totalFile.ElementAt(j).ToString());
                    _CheckRegisterMeFileExists = true;
                    ReadRegisterMefile oread = new ReadRegisterMefile();
                    string[,] filearr = oread.ReadFile(m_registerMeFile);
                    if (filearr[8, 1].Trim() != prodInfo.Trim())
                    {
                        m_registerMeFile = string.Empty;
                        continue;
                    }
                    else
                    {
                        if (Utilities.ReverseString(filearr[23, 1].Trim()) != GetMachineDetails.ProcessorId())
                        {
                            m_registerMeFile = string.Empty;
                            continue;
                        }
                        else
                        {
                            break;
                        }

                    }
                }

            }
            if (m_registerMeFile == string.Empty && _IgnoreStandardRule != true)
            {
                ErrorMsg = "As per the last request of product upgradation, Regiser.me file not found for product " + productName;
                return false;
            }
            return true;
        }

        private bool CheckUpgradeInfFile(string productName)
        {
            DirectoryInfo dir = new DirectoryInfo(appPath);

            System.Linq.IOrderedEnumerable<FileInfo> totalFile = (from f in dir.GetFiles("*Info.Inf") orderby f.LastWriteTime descending select f);

            string mInfFile = string.Empty;
            if (totalFile.Count() > 1)
            {
                for (int i = 0; i < totalFile.Count(); i++)
                {
                    mInfFile = Path.GetFullPath(totalFile.ElementAt(i).ToString());
                    ueProductUpgrade.ReadInfFile oInf = new ueProductUpgrade.ReadInfFile();
                    upgradeInfData = oInf.ReadFile(mInfFile);
                    if (upgradeInfData.Rows.Count > 0)
                    {
                        if (upgradeInfData.Rows[0]["prodnm"].ToString().Trim() != UpgradeApplication.Trim())
                        {
                            mInfFile = string.Empty;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        mInfFile = string.Empty;
                    }
                }
            }
            else
            {
                for (int i = 0; i < totalFile.Count(); i++)
                {
                    mInfFile = Path.GetFullPath(totalFile.ElementAt(i).ToString());
                    ueProductUpgrade.ReadInfFile oInf = new ueProductUpgrade.ReadInfFile();
                    upgradeInfData = oInf.ReadFile(mInfFile);

                    if (upgradeInfData.Rows.Count > 0)
                    {
                        if (upgradeInfData.Rows[0]["prodnm"].ToString().Trim() != UpgradeApplication.Trim())
                        {
                            mInfFile = string.Empty;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        mInfFile = string.Empty;
                    }

                }

            }
            if (mInfFile == string.Empty && _IgnoreStandardRule != true)
            {
                upgradeInfData.Rows.Clear();
                ErrorMsg = "As per the last request of product upgradation, Inf file not found for product " + productName;
                return false;
            }
            return true;
        }

        #endregion
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = this.tabControl1.SelectedIndex - 1;
            SetNavigationVisibility();
            this.tabControl1.Refresh();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = this.tabControl1.SelectedIndex + 1;
            SetNavigationVisibility();
            this.tabControl1.Refresh();
            // Added by Shrikant S. on 27/04/2019 for Registration      //Start
            if (this.rdoGenxml.Checked == true)
            {
                if (this.tabControl1.SelectedTab.Name == "tabPage5")
                {
                    this.txtaddicompcnt.Text = Convert.ToInt32(UpgradecompList.Compute("Count(CompId)", "CompId<0")).ToString();
                    if (this.txtregicompcnt.Text != "Unlimited")
                        this.txttotcompcnt.Text = (int.Parse(this.txtregicompcnt.Text) + int.Parse(this.txtaddicompcnt.Text)).ToString();
                }
            }
            // Added by Shrikant S. on 27/04/2019 for Registration      //End
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinish_Click(object sender, EventArgs e)
        {
            //this.lblStatus.Height= this.lblStatus.Height+20;
            //this.lblStatus.BringToFront();
            //Thread.Sleep(10);

            //Added by Shrikant S. on 14/02/2019 for Registration           //Start
            if (this.rdoGenxml.Checked == true)
            {
                if (!this.CheckValidation())
                {
                    return;
                }
            }
            //Added by Shrikant S. on 14/02/2019 for Registration           //End

            //Added by Shrikant S. on 08/04/2019 for Registration           //Start
            if (this.rdoProductUpgrade.Checked == true)
            {
                string reqString = this.GetRequestDetails();
                if (reqString.Length <= 0)
                {
                    MessageBox.Show("Please generate the xml to upgrade first then run upgrade product.", this.CurrentApplication, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                _UpgradeApplication = this.GetExtractString(reqString, "upgradeto");
                if (_UpgradeApplication.Trim() != this.CurrentApplication.Trim())
                {
                    this.GetProductCode(_UpgradeApplication);
                    if (!this.CheckUpgradeRegisterMe(_UpgradeAppFile))
                    {
                        MessageBox.Show(ErrorMsg, this.CurrentApplication, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }

                    if (!this.CheckUpgradeInfFile(_UpgradeAppFile))
                    {
                        MessageBox.Show(ErrorMsg, this.CurrentApplication, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                }
            }
            //Added by Shrikant S. on 08/04/2019 for Registration           //End

            this.btnBack.Enabled = false;
            this.btnFinish.Enabled = false;

            oWorker.RunWorkerAsync();
        }

        //private void btnFinish_1_Click(object sender, EventArgs e)
        //{
        //    //this.lblStatus.Height = this.lblStatus.Height + 20;

        //    this.lblStatus.BringToFront();
        //    Thread.Sleep(10);
        //    this.btnFinish.Enabled = false;
        //    oWorker.RunWorkerAsync();

        //    //Added by Shrikant S. on 07/05/2018 for bug-31515      && Start
        //    if (_IgnoreStandardRule == true)
        //    {
        //        this.rdoProductUpgrade.Checked = true;
        //        this.Refresh();
        //    }
        //    //Added by Shrikant S. on 07/05/2018 for bug-31515      && End

        //    if (rdoProductUpgrade.Checked == true)
        //    {
        //        lblStatus.Visible = true;
        //        Thread.Sleep(10);
        //        this.ShowProcess("Extracting Zip files");
        //        //ExtractZipFiles();        //Commented by Shrikant S. on 15/03/2018 
        //        if (!ExtractZipFiles())
        //        {
        //            lblStatus.Visible = false;
        //            MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            this.Close();
        //            return;
        //        }
        //        if (this.CheckRegisterMe())               //Commented by Shrikant S. on 19/02/2019 for Registration
        //        //if (this.checkMefile)                        //Added by Shrikant S. on 19/02/2019 for Registration
        //        {
        //            if (!CheckRegisterMeValidation())
        //            {
        //                lblStatus.Visible = false;
        //                MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                this.Close();
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            if (_IgnoreStandardRule == false)
        //            {
        //                MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                this.Close();
        //                return;
        //            }
        //        }
        //        this.ReadInfFile();

        //        if (!this.CheckIniValidation())
        //        {
        //            lblStatus.Visible = false;
        //            MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            this.Close();
        //            return;
        //        }
        //        //if (!this.ExecuteUpdates())
        //        //{
        //        //    lblStatus.Visible = false;
        //        //    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        //    this.Close();
        //        //    return;
        //        //}
        //        if (!this.GetFeatures())
        //        {
        //            lblStatus.Visible = false;
        //            MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            this.Close();
        //            return;
        //        }
        //        //Added by Shrikant S. on 07/05/2018 for Bug-31515          //Start
        //        if (_IgnoreStandardRule == true)
        //        {
        //            this.UpdateInfInfo();
        //        }
        //        //Added by Shrikant S. on 07/05/2018 for Bug-31515          //End

        //        //if (!this.DoProductUpgrade())             //Commented by Shrikant S. on 04/02/2018 for Registration 
        //        if (!this.DoProductUpgrade_New())               //Added by Shrikant S. on 04/02/2018 for Registration     
        //        {
        //            lblStatus.Visible = false;
        //            MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            this.Close();
        //            return;
        //        }
        //        //if (!this.ExecuteScripts())               //Commented by Shrikant S. on 04/02/2018 for Registration 
        //        if (!this.ExecuteScripts_New())                 //Added by Shrikant S. on 04/02/2018 for Registration  
        //        {
        //            lblStatus.Visible = false;
        //            MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            this.Close();
        //            return;
        //        }
        //        //if (!this.InsertFeature())             //Commented by Shrikant S. on 04/02/2018 for Registration 
        //        if (!this.InsertFeature_New())          //Added by Shrikant S. on 04/02/2018 for Registration     
        //        {
        //            lblStatus.Visible = false;
        //            MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            this.Close();
        //            return;
        //        }
        //        //if (!this.DoMenuUpdate())                 //Commented by Shrikant S. on 04/02/2018 for Registration 
        //        if (!this.DoMenuUpdate_New())                    //Added by Shrikant S. on 04/02/2018 for Registration     
        //        {
        //            lblStatus.Visible = false;
        //            MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            this.Close();
        //            return;
        //        }
        //        if (!this.Update_Request(2))
        //        {
        //            lblStatus.Visible = false;
        //            MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            this.Close();
        //            return;
        //        }
        //        MessageBox.Show("Upgradation done ..." + Environment.NewLine + "Please check the log file in Companyfolder under Upgrade_Log folder", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.None);
        //        this.Close();
        //    }
        //    else
        //    {

        //        string fileNames = string.Empty;
        //        //Added by Shrikant S. on 06/02/2019 for Registration           //Start
        //        if (ds.Tables["Product_vw"] != null)
        //        {
        //            DataRow[] defaProducts = ds.Tables["Product_vw"].Select("Sel=True and isDefault=True");
        //            if (defaProducts.Count() > 0 && this.CheckRegisterMe())               //Commented by Shrikant S. on 19/02/2019 for Registration
        //            //if (defaProducts.Count() > 0 && this.checkMefile)                       //Added by Shrikant S. on 19/02/2019 for Registration
        //            {
        //                lblStatus.Visible = true;
        //                Thread.Sleep(10);
        //                this.lblStatus.Text = "Please wait... Updating default modules";
        //                if (!ExtractZipFiles())
        //                {
        //                    lblStatus.Visible = false;
        //                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    this.Close();
        //                    return;
        //                }

        //                if (this.CheckRegisterMe())               //Commented by Shrikant S. on 19/02/2019 for Registration
        //                //if (this.checkMefile)                        //Added by Shrikant S. on 19/02/2019 for Registration
        //                {
        //                    if (!CheckRegisterMeValidation())
        //                    {
        //                        lblStatus.Visible = false;
        //                        MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                        this.Close();
        //                        return;
        //                    }
        //                }
        //                else
        //                {
        //                    if (_IgnoreStandardRule == false)
        //                    {
        //                        MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                        this.Close();
        //                        return;
        //                    }
        //                }
        //                this.ReadInfFile();

        //                if (!this.CheckIniValidation())
        //                {
        //                    lblStatus.Visible = false;
        //                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    this.Close();
        //                    return;
        //                }
        //                if (!this.GetFeatures())
        //                {
        //                    lblStatus.Visible = false;
        //                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    this.Close();
        //                    return;
        //                }
        //                if (_IgnoreStandardRule == true)
        //                {
        //                    this.UpdateInfInfo();
        //                }

        //                if (!this.DoProductUpgrade_DefaultModule())
        //                {
        //                    lblStatus.Visible = false;
        //                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    this.Close();
        //                    return;
        //                }
        //                if (!this.ExecuteScripts_New())
        //                {
        //                    lblStatus.Visible = false;
        //                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    this.Close();
        //                    return;
        //                }
        //                if (!this.InsertFeature_DefaultModule())
        //                {
        //                    lblStatus.Visible = false;
        //                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    this.Close();
        //                    return;
        //                }
        //                if (!this.DoMenuUpdate_DefaultModule())
        //                {
        //                    lblStatus.Visible = false;
        //                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    this.Close();
        //                    return;
        //                }
        //                MessageBox.Show("Upgradation done ..." + Environment.NewLine + "Please check the log file in Companyfolder under Upgrade_Log folder", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.None);
        //                lblStatus.Text = "Pleas wait... Generating xml for additional module";
        //            }

        //        }

        //        this.GetAdditionalModules();

        //        fileNames = GenerateCompanyXml();
        //        if (!this.Update_Request(1, fileNames))
        //        {
        //            lblStatus.Visible = false;
        //            MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            this.Close();
        //            return;
        //        }
        //        //Added by Shrikant S. on 06/02/2019 for Registration           //End

        //        //fileNames = fileNames + Environment.NewLine + GenerateProductXml();      //Commented by Shrikant S. on 04/02/2019 for Registration
        //        fileNames = fileNames + Environment.NewLine + "Xml file generated successfully in main folder.";
        //        MessageBox.Show(fileNames, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        this.Close();
        //    }
        //}
        private void CancelProcess()
        {

            if (oWorker != null)
            {
                if (oWorker.IsBusy)
                {
                    // Notify the worker thread that a cancel has been requested.
                    // The cancel will not actually happen until the thread in the
                    // DoWork checks the m_oWorker.CancellationPending flag. 
                    oWorker.CancelAsync();
                }
            }
        }

        private void AddCompany(object sender, EventArgs e)
        {
            int compid = Convert.ToInt32(this.dgvComp2.CurrentRow.Cells["CompId"].Value);
            object calc = this.UpgradecompList.Compute("Min(CompId)", "CompId<0");
            int mincompid = 0;
            mincompid = (calc == DBNull.Value ? mincompid : (int.TryParse(Convert.ToString(calc), out mincompid)) ? mincompid : 0);

            DataRow row = this.UpgradecompList.NewRow();
            row["CompId"] = mincompid - 1;
            this.UpgradecompList.Rows.Add(row);
            this.UpgradecompList.AcceptChanges();
            DataRow[] prodRows = ds.Tables["Product_vw"].DefaultView.ToTable(true).Select("CompId=" + compid.ToString(), "");
            foreach (DataRow item in prodRows)
            {
                item["Sel"] = (Convert.ToBoolean(item["isDefault"]) || Convert.ToBoolean(item["FoundInInf"]));
                item["CompId"] = row["CompId"];
                DataRow prodrow = this.ds.Tables["Product_vw"].NewRow();
                prodrow.ItemArray = item.ItemArray;

                this.ds.Tables["Product_vw"].Rows.Add(prodrow);
            }
            this.ds.Tables["Product_vw"].AcceptChanges();
            this.dgvComp2.Refresh();
            this.dgvProductUpgrade.Refresh();
            this.dgvComp2_SelectionChanged(sender, e);

        }
        private void RemoveCompany(object sender, EventArgs e)
        {
            int compid = Convert.ToInt32(this.dgvComp2.CurrentRow.Cells["CompId"].Value);
            DataRow[] prodRows = this.ds.Tables["Product_vw"].Select("CompId=" + compid.ToString(), "");
            foreach (DataRow item in prodRows)
            {
                item.Delete();
            }
            this.ds.Tables["Product_vw"].AcceptChanges();

            DataRow[] comprow = UpgradecompList.Select("CompId=" + compid.ToString(), "");
            foreach (DataRow item in comprow)
            {
                item.Delete();
            }
            UpgradecompList.AcceptChanges();
            this.dgvComp2.Refresh();
            this.dgvProductUpgrade.Refresh();
            this.dgvComp2_SelectionChanged(sender, e);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //DialogResult ans = new DialogResult();
            //ans = MessageBox.Show("Do you really want to Cancel?", this.CurrentApplication, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (ans == DialogResult.Yes)
            //    Application.Exit();
            this.CancelProcess();

            this.Dispose();
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                //appPath = "C:\\UdyogERP";           // To be Removed
                appPath = Application.StartupPath;
                SetStyleToGrid();
                SetImages();
                SetNavigationVisibility();

                lblCurrApp.Text = _CurrentApplication;
                lblUpgradeApp1.Text = _CurrentApplication;
                _UpgradeAppFile = this.CurrentAppFile;
                _UpgradeApplication = _CurrentApplication;                      //Added by Shrikant S. on 26/04/2019 for Registration
                this.label32.Text = "Default Modules for " + _UpgradeAppFile;            //Added by Shrikant S. on 02/04/2019 for Registration
                if (!Utilities.InList(this.CurrentAppFile, vu10Prod))
                {
                    MessageBox.Show("This application will only run in VU-10 products.", CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }

                oConnect = new clsConnect();
                //oConnect.InitProc(appPath, this.CurrentAppFile);          //Commented by Shrikant S. on 13/08/2014 for Bug-23814  
                oConnect.InitProc("'" + appPath + "'", this.CurrentAppFile);    //Added by Shrikant S. on 13/08/2014 for Bug-23814  
                mudprodcode = VU_UDFS.dec(VU_UDFS.NewDECRY(oConnect.RetAppEnc(this.CurrentAppFile), "Ud*yog+1993"));     //Added by Shrikant S. on 06/02/2019 for Registration
                mudshortcode = oConnect.RetProduct().Replace(",", "").Trim();                                   //Added by Shrikant S. on 05/04/2019 for Registration

                BindData();
                tabControl1.TabPages.Remove(tabPage4);

                //Added by Shrikant S. on 07/05/2018 for Bug-31515          //Start
                checkMefile = this.CheckRegisterMe();

                if (!checkMefile)
                {
                    tabControl1.TabPages.Remove(tabPage5);
                }
                //Commented by Shrikant S. on 01/04/2019 for Registration       //Start
                //if ((CurrentAppFile == "UdyogERPSDK" || CurrentAppFile == "UdyogGSTSDK") && !_CheckRegisterMeFileExists)
                //{
                //    RegProd = _UpgradeAppFile;
                //    this.cboServiceVer.Text = "NORMAL";
                //    RegProd = "VudyogGST";
                //    _IgnoreStandardRule = true;
                //    this.groupBox1.Visible = false;
                //    this.btnApplication.Visible = false;
                //}
                //Commented by Shrikant S. on 01/04/2019 for Registration       //End

                //Added by Shrikant S. on 26/04/2019 for Registration       // Start
                if (this.cboServiceVer.Text.Trim().ToUpper() == "SUPPORT VERSION" || this.cboServiceVer.Text.Trim().ToUpper() == "MARKETING VERSION" || this.cboServiceVer.Text.Trim().ToUpper() == "DEVELOPER VERSION" || this.cboServiceVer.Text.Trim().ToUpper() == "EDUCATIONAL VERSION")
                {
                    RegProd = mudprodcode;
                    //this.cboServiceVer.Text = "NORMAL";           //Commented by Shrikant S. on 30/08/2019 for SDK Installer        
                    this.cboServiceVer.Text = "DEMO";               //Added by Shrikant S. on 30/08/2019 for SDK Installer        
                    _IgnoreStandardRule = true;
                }
                //Added by Shrikant S. on 26/04/2019 for Registration       // End

                foreach (string fileName in Directory.GetFiles(appPath + "\\BMP\\"))
                {
                    FileInfo f = new FileInfo(fileName);
                    if (f.Name.ToUpper().IndexOf("_" + mudprodcode.ToUpper() + ".JPG") > 0 || f.Name.ToUpper().IndexOf("_" + mudprodcode.ToUpper() + ".PNG") > 0)
                        this.picImage.Image = Image.FromFile(f.FullName);
                }
                //Added by Shrikant S. on 07/05/2018 for Bug-31515          //End

                //if (!this.CheckUpgradeInfFile(_UpgradeAppFile))           //code should be commented used for reading inf file.
                //{
                //    MessageBox.Show(ErrorMsg, this.CurrentApplication, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                //    return;
                //}


                if (!this.CheckRegisterMe())          //Commented by Shrikant S. on 07/05/2018 for Bug-31515    
                                                      //if (!checkMefile)                       //Added by Shrikant S. on 07/05/2018 for Bug-31515
                {
                    if (!_IgnoreStandardRule)               //Added by Shrikant S. on 07/05/2018 for Bug-31515
                    {
                        tabControl1.TabPages.Add(tabPage4);
                        GenerateDefaultValue();
                    }
                }
                else
                {
                    this.ReadInfFile();                          //Added by Shrikant S. on 19/02/2019 for Registration
                                                                 //Added by Shrikant S. on 07/05/2018 for Bug-31515      //Start
                    if (ErrorMsg.Length > 0)
                    {
                        MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    //Added by Shrikant S. on 07/05/2018 for Bug-31515      //End


                }

                lblStatus.Visible = true;
                lblStatus.Text = "Creating System Objects...";
                this.Refresh();
                if (!this.ExecuteUpdates())
                {

                    lblStatus.Visible = false;
                    MessageBox.Show(ErrorMsg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                lblStatus.Visible = false;
                //Commented by Shrikant S. on 07/05/2018 for Bug-31515      // Start
                ////Added by Shrikant S. on 26/10/2012 for Bug-5849       //Start
                //_CurrentApplication = this.GetCurrentAppName(_CurrentAppFile);
                //lblCurrApp.Text = _CurrentApplication;
                //lblUpgradeApp1.Text = _CurrentApplication;
                //_UpgradeAppFile = this.CurrentAppFile;
                ////Added by Shrikant S. on 26/10/2012 for Bug-5849       //End
                //Commented by Shrikant S. on 07/05/2018 for Bug-31515      // End

                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnApplication_Click(object sender, EventArgs e)
        {
            string prevApp = _UpgradeAppFile;
            //Added by Shrikant S. on 02/02/2019 for Registration       //Start
            udSelectPop.SELECTPOPUP oselect = new udSelectPop.SELECTPOPUP();
            string xcFldList = "AppName,AppDesc,Upgradeto1";
            string xcDispCol = "AppName:Application Name,AppDesc:Application Description";
            string xcRetCol = "AppName,AppDesc,Upgradeto1";
            //string cSql = "Select * From Vudyog..ApplicationDet Where Display<>0 and Deactive<>1";
            string cSql = "Select *,Upgradeto1=Convert(varchar(100),''),Upgradewith1=Convert(varchar(2000),'') From Vudyog..ApplicationDet Where Deactive<>1";
            DataTable _appdt = oDataAccess.GetDataTable(cSql, null, 50);
            for (int i = 0; i < _appdt.Rows.Count; i++)
            {
                _appdt.Rows[i]["Upgradeto1"] = VU_UDFS.NewDECRY(VU_UDFS.HexDecimalToASCII(_appdt.Rows[i]["Upgradeto"].ToString()), "Ud*yog+1993Prod");
                _appdt.Rows[i]["Upgradewith1"] = VU_UDFS.NewDECRY(VU_UDFS.HexDecimalToASCII(_appdt.Rows[i]["Upgradewith"].ToString()), "Ud*yog+1993Prod2");
            }
            DataRow[] _currappRow = _appdt.Select("Appname='" + this.CurrentAppFile + "'");
            //DataRow[] _currappRow = _appdt.Select("Upgradeto1='" + _UpgradeAppFile + "'");
            string[] upgradeto = null;
            if (_currappRow.Count() > 0)
            {
                //upgradeto = _currappRow[0]["UpdateTo"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                upgradeto = _currappRow[0]["Upgradewith1"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            for (int i = 0; i < _appdt.Rows.Count; i++)
            {
                bool updtfound = false;
                for (int j = 0; j < upgradeto.Length; j++)
                {
                    //if (_appdt.Rows[i]["AppName"].ToString().Trim() == upgradeto[j].ToString().Trim())
                    if (_appdt.Rows[i]["Upgradeto1"].ToString().Trim() == upgradeto[j].ToString().Trim())
                    {
                        updtfound = true;
                        break;
                    }
                }
                _appdt.Rows[i]["Display"] = updtfound;
            }

            DataView _dvw = _appdt.DefaultView;
            _dvw.RowFilter = "Display=True";
            oselect.pdataview = _dvw;
            oselect.pformtext = "Select Upgrade Version";
            oselect.psearchcol = xcFldList;
            oselect.pDisplayColumnList = xcDispCol;
            oselect.pRetcolList = xcRetCol;
            oselect.Icon = this.Icon;
            oselect.ShowDialog();
            if (oselect.pReturnArray != null)
            {
                lblUpgradeApp1.Text = oselect.pReturnArray[1].ToString();
                _UpgradeApplication = oselect.pReturnArray[1].ToString();
                _UpgradeAppFile = oselect.pReturnArray[0].ToString();
                //_UpgradeAppFile = oselect.pReturnArray[2].ToString();
                this.label32.Text = "Default Modules for " + _UpgradeAppFile;            //Added by Shrikant S. on 02/04/2019 for Registration
                upgradeAppTitle = oselect.pReturnArray[1].ToString();                   //Added by Shrikant S. on 05/04/2019 for Registration

                DataRow[] uprow = _appdt.Select("AppName='" + _UpgradeAppFile + "'");
                if (uprow.Count() > 0)
                {
                    upgradeProdcode = uprow[0]["Upgradeto1"].ToString().Trim();
                }
            }
            //Added by Shrikant S. on 02/02/2019 for Registration       //End


            if (prevApp.Length > 0 && _UpgradeAppFile.Length > 0 && prevApp.Trim() != _UpgradeAppFile.Trim())
            {
                ds.Tables["Product_vw"].Rows.Clear();
                this.GetProd();
                this.SetDefaultProduct();
                this.dgvComp2_SelectionChanged(sender, e);
            }



            //Commented by Shrikant S. on 02/02/2019 for Registration       //Start
            //conn = new SqlConnection(readIni.ConnectionString);
            //frmSearchForm objSearch = new frmSearchForm(conn, "Select Application", "ApplicationDet", "AppName,AppDesc,baseversion", "AppName:Name,AppDesc:Description,baseversion:Base Version", "", "AppName,AppDesc"); //Added by Rupesh G. on 01/09/2017 
            //if (objSearch.ShowDialog() == DialogResult.OK)
            //{
            //    lblUpgradeApp1.Text = objSearch.ReturnString[1].ToString();
            //    _UpgradeApplication = objSearch.ReturnString[1].ToString();
            //    _UpgradeAppFile = objSearch.ReturnString[0].ToString();
            //    objSearch = null;
            //}
            //Commented by Shrikant S. on 02/02/2019 for Registration       //End


            //comment Rupesh G. on 01/09/2017 
            //conn = new SqlConnection(readIni.ConnectionString);
            //frmSearchForm objSearch = new frmSearchForm(conn, "Select Application", "ApplicationDet", "AppName,AppDesc", "AppName:Name,AppDesc:Description", "", "AppName,AppDesc");
            //if (objSearch.ShowDialog() == DialogResult.OK)
            //{
            //    lblUpgradeApp1.Text = objSearch.ReturnString[1].ToString();
            //    _UpgradeApplication = objSearch.ReturnString[1].ToString();
            //    _UpgradeAppFile = objSearch.ReturnString[0].ToString();
            //    objSearch = null;
            //}
        }


        private void dgvComp1_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvComp1.SelectedRows.Count > 0)
            {
                ds.Tables["cProduct_vw"].DefaultView.RowFilter = "";
                ds.Tables["cProduct_vw"].DefaultView.RowFilter = "CompId=" + dgvComp1.SelectedRows[0].Cells["txtCompId"].Value.ToString();
                //dgvProduct.Refresh();
            }
            else
            {
                ds.Tables["cProduct_vw"].DefaultView.RowFilter = "";
                ds.Tables["cProduct_vw"].DefaultView.RowFilter = "CompId=" + dgvComp1.Rows[0].Cells["txtCompId"].Value.ToString();
                //dgvProduct.Refresh();
            }
        }

        private void dgvComp2_SelectionChanged(object sender, EventArgs e)
        {
            string[] lProducts = null;
            dgvProductUpgrade.Refresh();
            if (ds.Tables.Contains("CustInfo_vw"))
            {
                DataRow[] infrows = ds.Tables["CustInfo_vw"].Select("Clientnm='" + stringList[0] + "'");

                if (infrows.Length > 0)
                {
                    string prodlist = infrows[0]["ProdCd"].ToString() + "," + infrows[0]["addProdCd"].ToString();
                    lProducts = prodlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int d = 0; d < ds.Tables["Product_vw"].Rows.Count; d++)
                    {
                        if (lProducts.Contains(ds.Tables["Product_vw"].Rows[d]["cProdCode"].ToString().Trim()))
                        {
                            ds.Tables["Product_vw"].Rows[d]["FoundInInf"] = true;
                        }
                    }
                }
            }

            if (dgvComp2.SelectedRows.Count > 0)
            {
                ds.Tables["Product_vw"].DefaultView.RowFilter = "";
                ds.Tables["Product_vw"].DefaultView.RowFilter = "CompId=" + dgvComp2.SelectedRows[0].Cells["CompId"].Value.ToString();
                for (int i = 0; i < dgvProductUpgrade.Rows.Count; i++)
                {
                    //if (dgvProductUpgrade.Rows[i].Cells["Sel"].Value.ToString() == "True")
                    if (dgvProductUpgrade.Rows[i].Cells["currSel"].Value.ToString() == "True")
                    {
                        dgvProductUpgrade.Rows[i].ReadOnly = true;
                        dgvProductUpgrade.Rows[i].DefaultCellStyle.BackColor = Color.Cyan;
                    }
                    //Added by Shrikant S. on 05/02/2019 for Registration   //Start
                    //if (dgvProductUpgrade.Rows[i].Cells["Default"].Value.ToString() == "True" && dgvProductUpgrade.Rows[i].Cells["chkSel2"].Value.ToString() == "False" && dgvProductUpgrade.Rows[i].Cells["currsel"].Value.ToString() == "False")
                    if (dgvProductUpgrade.Rows[i].Cells["Default"].Value.ToString() == "True" && dgvProductUpgrade.Rows[i].Cells["currsel"].Value.ToString() == "False")
                    {
                        dgvProductUpgrade.Rows[i].DefaultCellStyle.BackColor = Color.Pink;
                    }
                    if (ds.Tables.Contains("CustInfo_vw"))
                    {
                        if (dgvProductUpgrade.Rows[i].Cells["Default"].Value.ToString() == "False" && dgvProductUpgrade.Rows[i].Cells["FoundInInf"].Value.ToString() == "True")
                        {
                            dgvProductUpgrade.Rows[i].DefaultCellStyle.BackColor = Color.Violet;
                        }
                    }
                    //Added by Shrikant S. on 05/02/2019 for Registration   //End

                }
                if (dgvProductUpgrade.Rows.Count > 0)
                {
                    dgvProductUpgrade.Sort(this.dgvProductUpgrade.Columns["cModDep"], ListSortDirection.Ascending);
                    dgvProductUpgrade.Rows[0].Selected = true;
                    dgvProductUpgrade.FirstDisplayedScrollingRowIndex = 0;
                    dgvProductUpgrade.Refresh();
                }
            }
            else
            {
                ds.Tables["Product_vw"].DefaultView.RowFilter = "";
                ds.Tables["Product_vw"].DefaultView.RowFilter = "CompId=" + dgvComp2.Rows[0].Cells["CompId"].Value.ToString();
                for (int i = 0; i < dgvProductUpgrade.Rows.Count; i++)
                {
                    if (dgvProductUpgrade.Rows[i].Cells["chkSel2"].Value.ToString() == "True")
                    {
                        dgvProductUpgrade.Rows[i].ReadOnly = true;
                    }
                }
                dgvProductUpgrade.Sort(this.dgvProductUpgrade.Columns["cModDep"], ListSortDirection.Ascending);
                //dgvProductUpgrade.Rows[0].Selected = true;            //Commenetd by Shrikant S. on 02/02/2019 for Registration
                //dgvProductUpgrade.FirstDisplayedScrollingRowIndex = 0;    //Commenetd by Shrikant S. on 02/02/2019 for Registration
                dgvProductUpgrade.Refresh();
            }
        }
        private void dgvProductUpgrade_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProductUpgrade.Columns[e.ColumnIndex].Name == "chkSel2")
            {
                if (dgvProductUpgrade.CurrentRow.Cells["chkSel2"].ReadOnly == false)
                {
                    DataRow[] dr = ds.Tables["Product_vw"].Select("CompId=" + dgvComp2.SelectedRows[0].Cells["CompId"].Value.ToString() + " and cProdCode='" + dgvProductUpgrade.CurrentRow.Cells["cProdCode"].Value.ToString() + "'");
                    string moduleDep = dgvProductUpgrade.CurrentRow.Cells["cModDep"].Value.ToString();
                    string moduleNotAllowed = dgvProductUpgrade.CurrentRow.Cells["cCmbNotAlwd"].Value.ToString();
                    string currProd = dgvProductUpgrade.CurrentRow.Cells["cProdName"].Value.ToString();
                    int currIndex = dgvProductUpgrade.CurrentRow.Index;
                    string productName = string.Empty;
                    string prodNotAllowed = string.Empty;
                    bool setValue = true;

                    switch (dgvProductUpgrade.CurrentRow.Cells["chkSel2"].Value.ToString())
                    {
                        case "True":
                            string compid = dgvComp2.SelectedRows[0].Cells["CompId"].Value.ToString();
                            //Added by Shrikant S. on 03/06/2019            //Start
                            string cCProdCode = dgvProductUpgrade.CurrentRow.Cells["cProdCode"].Value.ToString();
                            DataRow[] _tallyRows = ds.Tables["Product_vw"].Select("CompId=" + compid + " and sel=True and cModDep Like '%" + cCProdCode + "%' and Len(Trim(cModDep))>0");
                            string _ProdRemv = string.Empty;
                            for (int i = 0; i < _tallyRows.Length; i++)
                            {
                                string _ccModDep = _tallyRows[i]["cModDep"].ToString().Trim();
                                int _ProdFnd = 0;
                                do
                                {
                                    string _ccModDep1 = (_ccModDep.IndexOf(',') > 0 ? _ccModDep.Substring(0, _ccModDep.IndexOf(',') - 1) : _ccModDep);
                                    _ccModDep = (_ccModDep.IndexOf(',') > 0 ? _ccModDep.Substring(_ccModDep.IndexOf(',') + 1) : string.Empty);

                                    if (_ccModDep1.Contains("+") && _ccModDep1.Contains(cCProdCode))
                                    {
                                        _ProdFnd = 1;
                                    }
                                    if (_ProdFnd != 1 && _ccModDep1 != cCProdCode)
                                    {
                                        if (!_ProdRemv.Contains(_ccModDep1))
                                        {
                                            DataRow[] childRows = ds.Tables["Product_vw"].Select("CompId=" + compid + " and sel=True and Trim(cProdCode) ='" + _ccModDep + "' ");
                                            if (childRows.Length > 0)
                                            {
                                                _ProdFnd = 2;
                                            }
                                        }
                                    }
                                    if (_ccModDep.Length <= 0)
                                    {
                                        break;
                                    }
                                } while (true);
                                if (_ProdFnd != 2)
                                {
                                    _ProdRemv = (_ProdRemv.Length > 0 ? _ProdRemv.Trim() + "," : "") + "[" + _tallyRows[i]["cProdCode"].ToString().Trim() + "]";
                                }

                            }
                            //Added by Shrikant S. on 03/06/2019            //End
                            if (_ProdRemv.Length != 0)
                            {
                                string unselectProd = dr.ElementAt(0)["cProdCode"].ToString();
                                DialogResult ans = new DialogResult();
                                string Msg = "some of the dependent modules are selected." + Environment.NewLine;
                                Msg = Msg + "Un-selecting the main module will unselect the depending modules." + Environment.NewLine;
                                Msg = Msg + "Would you like to continue ?";
                                ans = MessageBox.Show(Msg, this.CurrentApplication, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (ans == DialogResult.Yes)
                                {
                                    for (int i = 0; i < dgvProductUpgrade.RowCount; i++)
                                    {
                                        if (_ProdRemv.Contains("[" + dgvProductUpgrade.Rows[i].Cells["cProdCode"].Value.ToString() + "]") || _ProdRemv.Contains("[" + dgvProductUpgrade.Rows[i].Cells["cModDep"].Value.ToString() + "]"))
                                        {
                                            dgvProductUpgrade.Rows[i].Cells["chkSel2"].Value = false;
                                        }
                                        //if (dgvProductUpgrade.Rows[i].Cells["chkSel2"].Value.ToString() == "True")
                                        //{
                                        //    string DependencyMod = dgvProductUpgrade.Rows[i].Cells["cModDep"].Value.ToString();
                                        //    if (DependencyMod.Length != 0)
                                        //    {
                                        //        if (DependencyMod.Contains(unselectProd))
                                        //        {
                                        //            dgvProductUpgrade.Rows[i].Cells["chkSel2"].Value = false;
                                        //        }
                                        //    }
                                        //}
                                    }
                                    setValue = false;
                                }
                                dr.ElementAt(0)["Sel"] = setValue;
                                dgvProductUpgrade.Rows[currIndex].Selected = true;
                                dgvProductUpgrade.CurrentRow.Cells["chkSel2"].Value = setValue;
                                //dgvProductUpgrade.CurrentRow.Cells["currsel"].Value = setValue;

                            }
                            else
                            {
                                setValue = false;
                                dr.ElementAt(0)["Sel"] = setValue;
                                dgvProductUpgrade.Rows[currIndex].Selected = true;
                                dgvProductUpgrade.CurrentRow.Cells["chkSel2"].Value = setValue;
                            }
                            break;
                        case "False":
                            if (moduleDep.Trim().Length != 0)
                            {

                                int cnt = 0;
                                for (int i = 0; i < dgvProductUpgrade.Rows.Count; i++)
                                {
                                    if (moduleDep.Contains(dgvProductUpgrade.Rows[i].Cells["cProdCode"].Value.ToString()))
                                    {
                                        if (moduleDep.Contains("+"))
                                        {
                                            string[] depprods = moduleDep.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                                            int seledepprodcount = 0;
                                            string depmodname = string.Empty;
                                            for (int k = 0; k < depprods.Length; k++)
                                            {
                                                for (int j = 0; j < dgvProductUpgrade.Rows.Count; j++)
                                                {
                                                    if (dgvProductUpgrade.Rows[j].Cells["cProdCode"].Value.ToString().Trim() == depprods[k].Trim())
                                                    {
                                                        depmodname = depmodname + dgvProductUpgrade.Rows[j].Cells["cProdName"].Value.ToString().Trim() + " + ";
                                                        if (Convert.ToBoolean(dgvProductUpgrade.Rows[j].Cells["chkSel2"].Value) == true)
                                                        {
                                                            seledepprodcount++;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (depprods.Length != seledepprodcount)
                                            {
                                                productName = depmodname.Left(depmodname.Length - 3);
                                                cnt = 1;
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            if (dgvProductUpgrade.Rows[i].Cells["chkSel2"].Value.ToString() == "True")
                                            {
                                                productName = string.Empty;
                                                break;
                                            }
                                        }
                                        cnt = cnt + 1;
                                        productName = productName + (cnt == 1 ? "" : ", " + Environment.NewLine) + dgvProductUpgrade.Rows[i].Cells["cProdName"].Value.ToString();
                                    }
                                }
                                if (productName.Trim().Length != 0)
                                {
                                    string Msg = currProd + " product can be selected only if";
                                    Msg = Msg + (cnt == 1 ? " the following module is selected: " : " either of the following modules are selected: ") + Environment.NewLine + productName;
                                    MessageBox.Show(Msg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    setValue = false;
                                }
                                dr.ElementAt(0)["Sel"] = setValue;
                                dgvProductUpgrade.Rows[currIndex].Selected = true;
                                dgvProductUpgrade.CurrentRow.Cells["chkSel2"].Value = setValue;
                                //dgvProductUpgrade.CurrentRow.Cells["currsel"].Value = setValue;
                            }
                            if (moduleNotAllowed.Trim().Length != 0 && setValue == true)
                            {
                                for (int i = 0; i < dgvProductUpgrade.Rows.Count; i++)
                                {
                                    if (moduleNotAllowed.Contains(dgvProductUpgrade.Rows[i].Cells["cMainProdCode"].Value.ToString()) && dgvProductUpgrade.Rows[i].Cells["chkSel2"].Value.ToString() == "True")
                                    {
                                        prodNotAllowed = prodNotAllowed + ", " + dgvProductUpgrade.Rows[i].Cells["cProdName"].Value.ToString();
                                    }
                                }
                                if (prodNotAllowed.Trim().Length != 0)
                                {
                                    string Msg = "'" + currProd + "' product not allowed with selected combination.";
                                    MessageBox.Show(Msg, CurrentAppFile, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    setValue = false;
                                }
                                dr.ElementAt(0)["Sel"] = setValue;
                                dgvProductUpgrade.Rows[currIndex].Selected = true;
                                dgvProductUpgrade.CurrentRow.Cells["chkSel2"].Value = setValue;
                                //dgvProductUpgrade.CurrentRow.Cells["currsel"].Value = setValue;
                            }
                            //Added by Shrikant S. on 27/04/2019 for Registration       //Start
                            if (moduleDep.Trim().Length == 0 && setValue == true)
                            {
                                dr.ElementAt(0)["Sel"] = setValue;
                                dgvProductUpgrade.Rows[currIndex].Selected = true;
                                dgvProductUpgrade.CurrentRow.Cells["chkSel2"].Value = setValue;
                                //dgvProductUpgrade.CurrentRow.Cells["currsel"].Value = setValue;
                            }
                            //Added by Shrikant S. on 27/04/2019 for Registration       //End

                            break;
                    }
                    dgvProductUpgrade.RefreshEdit();
                }
            }
        }



        private void btnCompany_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection(readIni.ConnectionString);
            frmSearchForm objSearch = new frmSearchForm(ds.Tables["Co_Mast_vw"], "Select company", "Co_Name", "Co_Name:Company Name", "Compid,passroute,passroute1,Add1,Add2,Add3,area,zone,city,zip,state,country,email,sta_dt,end_dt,foldername,gstin", "Co_Name,Add1,Add2,Add3,area,zone,city,zip,state,country,email,dbname,gstin");
            if (objSearch.ShowDialog() == DialogResult.OK)
            {

                txtCo_name.Text = objSearch.ReturnString[0].ToString().Trim();
                //txtContact.Text = objSearch.ReturnString[9].ToString().Trim();            //Commented by Shrikant S. on 01/04/2019 for Registration
                txtContact.Text = string.Empty;              //Added by Shrikant S. on 01/04/2019 for Registration
                txtEmail.Text = objSearch.ReturnString[10].ToString().Trim();

                //Co_Name,Add1,Add2,Add3,area,zone,city,zip,state,country,email,dbname,gstin
                stringList.Add(objSearch.ReturnString[0].ToString().Trim());
                stringList.Add(objSearch.ReturnString[1].ToString().Trim());
                stringList.Add(objSearch.ReturnString[2].ToString().Trim());
                stringList.Add(objSearch.ReturnString[3].ToString().Trim());
                stringList.Add(objSearch.ReturnString[4].ToString().Trim());
                stringList.Add(objSearch.ReturnString[5].ToString().Trim());
                stringList.Add(objSearch.ReturnString[6].ToString().Trim());
                stringList.Add(objSearch.ReturnString[7].ToString().Trim());
                stringList.Add(objSearch.ReturnString[8].ToString().Trim());
                stringList.Add(objSearch.ReturnString[9].ToString().Trim());
                stringList.Add("");
                stringList.Add("");
                stringList.Add("");
                stringList.Add("");
                stringList.Add(objSearch.ReturnString[10].ToString().Trim());
                stringList.Add("");
                stringList.Add("");

                this.txtgstin.Text = objSearch.ReturnString[12].ToString().Trim();          //Added by Shrikant S.on 30/04/2019 for registration
                this.txtgstreg.Text = (this.txtgstin.Text.Length > 0 ? "Registered" : "Un-registered");

                SqlDataAdapter da = new SqlDataAdapter("Select * From " + objSearch.ReturnString[11].ToString().Trim() + "..Manufact", conn);
                DataTable coaddi = new DataTable();
                da.Fill(coaddi);
                txtnatbusi.Text = coaddi.Rows[0]["constbusi"].ToString().Trim();
                txtprodmfg.Text = coaddi.Rows[0]["ProdMfg"].ToString().Trim();


            }

            objSearch = null;
        }

        private void cboServiceVer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboServiceVer.Text != "Client Version")
            {
                txtUsers.Text = "1";
                txtNoComp.Text = "1";
            }
        }

        private void rdoProductUpgrade_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoProductUpgrade.Checked == true)
            {
                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Remove(tabPage3);
                if (!this.CheckRegisterMe())          //Commented by Shrikant S. on 19/02/2019 for Registration
                                                      //if (!this.checkMefile)              //Added by Shrikant S. on 19/02/2019 for Registration    
                    tabControl1.TabPages.Remove(tabPage4);

                this.btnBack.Enabled = false;
                this.btnNext.Enabled = false;
                this.btnFinish.Enabled = true;

            }
            else
            {
                tabControl1.TabPages.Add(tabPage2);
                tabControl1.TabPages.Add(tabPage3);

                if (!this.CheckRegisterMe())               //Commented by Shrikant S. on 19/02/2019 for Registration
                //if (!this.checkMefile)                //Added by Shrikant S. on 19/02/2019 for Registration    
                {
                    tabControl1.TabPages.Add(tabPage4);
                    GenerateDefaultValue();
                }
                this.SetNavigationVisibility();
            }
        }

        private void txtUsers_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {

        }

        private void btnnatbusi_Click(object sender, EventArgs e)
        {
            DataTable natureBusi = new DataTable();
            natureBusi.Columns.Add("natbusi", typeof(string));
            natureBusi.Rows.Add("");
            natureBusi.Rows.Add("Any person registered under CGST/SGST/UTGST act");
            natureBusi.Rows.Add("AOP");
            natureBusi.Rows.Add("Arbitral Tribunal");
            natureBusi.Rows.Add("Artist");
            natureBusi.Rows.Add("Author");
            natureBusi.Rows.Add("Banking Company");
            natureBusi.Rows.Add("Casual Taxable person");
            natureBusi.Rows.Add("Co-operative Society established under any law");
            natureBusi.Rows.Add("Director");
            natureBusi.Rows.Add("Factory registered under the Factories Act, 1948");
            natureBusi.Rows.Add("Financial Institution");
            natureBusi.Rows.Add("Firm of Advocates");
            natureBusi.Rows.Add("Goods Transport Agency");
            natureBusi.Rows.Add("Government");
            natureBusi.Rows.Add("HUF");
            natureBusi.Rows.Add("Importer as defined under clause (26) of section 2 of the Customs Act, 1962");
            natureBusi.Rows.Add("Individual");
            natureBusi.Rows.Add("Individual Advocate");
            natureBusi.Rows.Add("Insurance Agent");
            natureBusi.Rows.Add("Insurance Company");
            natureBusi.Rows.Add("Local Authority");
            natureBusi.Rows.Add("Music Company");
            natureBusi.Rows.Add("Music Composer");
            natureBusi.Rows.Add("Non - Assessee online recipient");
            natureBusi.Rows.Add("Non Banking Financial Company");
            natureBusi.Rows.Add("Other");
            natureBusi.Rows.Add("Partnership Firm (including limited liability partnerships)");
            natureBusi.Rows.Add("Photographer");
            natureBusi.Rows.Add("Private Limited Company");
            natureBusi.Rows.Add("Producer");
            natureBusi.Rows.Add("Public Limited Company");
            natureBusi.Rows.Add("Publisher");
            natureBusi.Rows.Add("Recovery Agent");
            natureBusi.Rows.Add("Rent a Cab operator");
            natureBusi.Rows.Add("Society registered under the Societies Registration Act, 1860");
            natureBusi.Rows.Add("Taxi driver");
            //frmSearchForm objSearch = new frmSearchForm(natureBusi, "Select Nature of business", "natbusi", "natbusi:Nature of business", "", "natbusi");
            //if (objSearch.ShowDialog() == DialogResult.OK)
            //{

            //}

            udSelectPop.SELECTPOPUP popup = new udSelectPop.SELECTPOPUP();
            popup.psearchcol = "natbusi";
            popup.pdataview = natureBusi.DefaultView;
            popup.pformtext = "Select Nature of Business";
            popup.psearchcol = "natbusi";
            popup.pDisplayColumnList = "natbusi:Nature of Business";
            popup.pRetcolList = "natbusi";
            popup.Icon = this.Icon;
            popup.ShowDialog();
            if (popup.pReturnArray != null)
            {
                this.txtnatbusi.Text = popup.pReturnArray[0].ToString();
            }
        }

        private void dgvComp2_MouseClick(object sender, MouseEventArgs e)
        {
            //Commented by Shrikant S. on 16/09/2019 for Bug-32842      // Start
            //if (e.Button == MouseButtons.Right)
            //{
            //    ContextMenu m = new ContextMenu();
            //    m.MenuItems.Add(new MenuItem("Add Company", new EventHandler(AddCompany)));
            //    m.MenuItems.Add(new MenuItem("Remove Company", new EventHandler(RemoveCompany)));
            //    int currentMouseOverRow = dgvComp2.HitTest(e.X, e.Y).RowIndex;

            //    m.Show(dgvComp2, new Point(e.X, e.Y));
            //}
            //Commented by Shrikant S. on 16/09/2019 for Bug-32842      // End
        }

        private void dgvComp2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvComp2.Columns[e.ColumnIndex].Name == "colconame")
            {
                if (e.FormattedValue.ToString().Trim().Length <= 0)
                {
                    MessageBox.Show("Company name cannot be ;.", vuMess, MessageBoxButtons.OK);
                    e.Cancel = true;
                }
            }
        }
        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void txtaddicompcnt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete);
        }

        private void txtaddiusercnt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete);
        }

        private void txtaddicompcnt_Validated(object sender, EventArgs e)
        {
            this.txttotcompcnt.Text = (int.Parse(this.txtregicompcnt.Text) + int.Parse(this.txtaddicompcnt.Text)).ToString();
        }

        private void txtaddiusercnt_Validated(object sender, EventArgs e)
        {
            this.txttotusercnt.Text = (int.Parse(this.txtregiusercnt.Text) + int.Parse(this.txtaddiusercnt.Text)).ToString();
        }

        private void txtNoComp_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete);
        }


    }
}

