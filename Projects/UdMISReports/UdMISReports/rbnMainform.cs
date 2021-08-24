using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraWaitForm;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraPivotGrid;
using DevExpress.LookAndFeel;
using System.Text.RegularExpressions;
using System.Xml;
using DevExpress.XtraCharts;
using System.Diagnostics;
using DevExpress.Printing.ExportHelpers;
using DevExpress.Export;
using Microsoft.Office.Interop.Excel;


namespace UdMISReports
{
    public partial class rbnMainform : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private SqlConnection oconn;
        private System.Data.DataTable _SettingTbl = new System.Data.DataTable("ColDetView");
        private System.Data.DataTable _CompanyDetails;
        private string _currentLayout = string.Empty;

        private string ReportName = string.Empty;
        private string MainIcon = string.Empty;
        private int filterFieldCount = 0;
        private int maxfilterFieldCount = 20;
        private string cAppPId = string.Empty;
        private string cAppName = "UdMISReports.exe";
        private string reportCaption = "";
        Workbook workbook;


        #region public Properties
        private int _CompId;
        

        public int CompId
        {
            get
            {
                return _CompId;
            }

            set
            {
                _CompId = value;
            }
        }

        private string _CompDbNm;
        public string CompDbNm
        {
            get
            {
                return _CompDbNm;
            }

            set
            {
                _CompDbNm = value;
            }
        }

        private string _ServerName;
        public string ServerName
        {
            get
            {
                return _ServerName;
            }

            set
            {
                _ServerName = value;
            }
        }

        private string _SQLUserId;
        public string SQLUserId
        {
            get
            {
                return _SQLUserId;
            }

            set
            {
                _SQLUserId = value;
            }
        }

        private string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }

            set
            {
                _Password = value;
            }
        }

        private string _MenuRange;
        public string MenuRange
        {
            get
            {
                return _MenuRange;
            }

            set
            {
                _MenuRange = value;
            }
        }

        private string _UserName;
        public string UserName
        {
            get
            {
                return _UserName;
            }

            set
            {
                _UserName = value;
            }
        }

        private string _ApplText;
        public string ApplText
        {
            get
            {
                return _ApplText;
            }

            set
            {
                _ApplText = value;
            }
        }

        private string _ApplName;
        public string ApplName
        {
            get
            {
                return _ApplName;
            }

            set
            {
                _ApplName = value;
            }
        }

        private double _ApplPId;
        public double ApplPId
        {
            get
            {
                return _ApplPId;
            }

            set
            {
                _ApplPId = value;
            }
        }

        private string _ApplCode;
        public string ApplCode
        {
            get
            {
                return _ApplCode;
            }

            set
            {
                _ApplCode = value;
            }
        }

        private string _ProdCode;
        public string ProdCode
        {
            get
            {
                return _ProdCode;
            }

            set
            {
                _ProdCode = value;
            }
        }

        private string _DefaPath;
        public string DefaPath
        {
            get
            {
                return _DefaPath;
            }

            set
            {
                _DefaPath = value;
            }
        }

        private string _AppPath;
        public string AppPath
        {
            get
            {
                return _AppPath;
            }

            set
            {
                _AppPath = value;
            }
        }

        private string _CoSta_dt;
        public string CoSta_dt
        {
            get
            {
                return _CoSta_dt;
            }

            set
            {
                _CoSta_dt = value;
            }
        }

        private string _CoEnd_dt;
        public string CoEnd_dt
        {
            get
            {
                return _CoEnd_dt;
            }

            set
            {
                _CoEnd_dt = value;
            }
        }

        private string _ReportId;
        public string ReportId
        {
            get
            {
                return _ReportId;
            }

            set
            {
                _ReportId = value;
            }
        }

        private string _StringValues;
        public string StringValues
        {
            get
            {
                return _StringValues;
            }

            set
            {
                _StringValues = value;
            }
        }

        private string _NumericValues;
        public string NumericValues
        {
            get
            {
                return _NumericValues;
            }

            set
            {
                _NumericValues = value;
            }
        }

        private string _DateTimeValues;
        public string DateTimeValues
        {
            get
            {
                return _DateTimeValues;
            }

            set
            {
                _DateTimeValues = value;
            }
        }

        private string _SqlConnString;
        public string SqlConnString
        {
            get
            {
                return _SqlConnString;
            }

            set
            {
                _SqlConnString = value;
            }
        }

        private System.Data.DataTable _gridSource;
        public System.Data.DataTable GridSource
        {
            get
            {
                return _gridSource;
            }

            set
            {
                _gridSource = value;
            }
        }
        #endregion




        public rbnMainform(string[] args)
        {
            InitializeComponent();
            string[] argument = null;
            argument = args[0].Split('|');
            this.CompId = Convert.ToInt16(args[1]);
            this.CompDbNm = args[2].Replace("<*#*>", " ");
            this.ServerName = args[3].Replace("<*#*>", " ");
            this.SQLUserId = args[4].Replace("<*#*>", " ");
            this.Password = args[5].Replace("<*#*>", " ");
            this.MenuRange = args[6].Replace("<*#*>", " ");
            this.UserName = args[7].Replace("<*#*>", " ");
            MainIcon = args[8].Replace("<*#*>", " ");
            this.ApplText = args[9].Replace("<*#*>", " ");
            this.ApplName = args[10].Replace("<*#*>", " ");
            this.ApplPId = Convert.ToDouble(args[11]);
            this.ApplCode = args[12].Replace("<*#*>", " ");
            ProdCode = args[13].Replace("<*#*>", " ");
            DefaPath = args[14].Replace("<*#*>", " ");
            AppPath = args[15].Replace("<*#*>", " ");
            CoSta_dt = args[16].Replace("<*#*>", " ");
            CoEnd_dt = args[17].Replace("<*#*>", " ");
            this.Icon = new System.Drawing.Icon(MainIcon);
            ReportId = argument[0].ToString();
            StringValues = argument[1].ToString();
            NumericValues = argument[2].ToString();
            DateTimeValues = argument[3].ToString();
            SqlConnString = "Data Source=" + this.ServerName + ";Initial Catalog=" + this.CompDbNm + ";Uid=" + this.SQLUserId + ";Pwd=" + this.Password;
            this.bsiUserName.Caption = this.bsiUserName.Caption + this.UserName;
            this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2;
            this.splitContainerControl2.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
        }
        #region Private Methods
        private void RefreshScreen()
        {
            SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
            SplashScreenManager.Default.SetWaitFormDescription("Loading setting...");
            Thread.Sleep(1);
            this.RefreshSetting();
            this.pgMain.Fields.Clear();
            try
            {

                SplashScreenManager.Default.SetWaitFormDescription("Applying setting...");
                Thread.Sleep(1);
                this.AddFieldsToPivot();
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");
                this.pgMain.LookAndFeel.SetSkinStyle("Office 2013 Light Gray");
                LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                System.Windows.Forms.Application.Exit();
            }
            SplashScreenManager.CloseForm();
        }
        private string[] SpiltArguments(string Value)
        {
            Value = Value.Replace("][", "|");
            Value = Value.Replace("[", "");
            Value = Value.Replace("]", "");
            Value = Value.Replace("=", "|");
            return Value.Split(new char[] { '|' });
        }
        private System.Data.DataTable GetCompanyDetails()
        {
            _CompanyDetails = new System.Data.DataTable();
            SqlConnection oconn = new SqlConnection(SqlConnString);
            SqlDataAdapter lda = new SqlDataAdapter("Select Top 1 * From Vudyog..Co_mast Where CompId=" + this.CompId, oconn);
            lda.Fill(_CompanyDetails);
            return _CompanyDetails;
        }
        private void GetDataSource(string reportId)
        {
            DataSet lds = new DataSet();
            System.Data.DataTable ldt = new System.Data.DataTable();
            oconn = new SqlConnection(this.SqlConnString);
            #region Fetching Report Details 
            SqlCommand cmd = new SqlCommand("Select Top 1 * From UBIReportMast Where ReportId=@ReportId", oconn);
            cmd.Parameters.Add(new SqlParameter("@ReportId", reportId));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(lds, "ReportView");
            if (lds.Tables["ReportView"].Rows.Count <= 0)
            {
                throw new Exception("Report not found in UBIReportMast");
            }
            ReportName = lds.Tables["ReportView"].Rows[0]["ReportNm"].ToString();

            cmd.CommandText = "Select QueryId,SqlQuery From UBIQueryMast Where ReportId=@ReportId";
            da.Fill(lds, "QueryView");
            if (lds.Tables["QueryView"].Rows.Count <= 0)
            {
                throw new Exception("Query not found in UBIQueryMast");
            }
            cmd.CommandText = "Select *,DataType=Convert(Varchar(100),'') From UBIColDet Where ReportId=@ReportId";
            da.Fill(_SettingTbl);

            cmd.Parameters.Clear();

            string sqlQuery = lds.Tables["QueryView"].Rows[0]["SqlQuery"].ToString();
            cmd.CommandText = sqlQuery.Replace("{", "").Replace("}", "");
            cmd.CommandTimeout = 0;
            #endregion

            #region Retrieving Parameters received  
            List<string> paraReceived = new List<string>();
            if (this.StringValues.Length > 0)
            {
                string[] strArgs = this.SpiltArguments(this.StringValues);
                for (int i = 0; i < strArgs.Length; i = i + 2)
                {
                    cmd.Parameters.Add(new SqlParameter("@" + strArgs[i].ToString(), strArgs[i + 1].ToString()));
                    paraReceived.Add("@" + strArgs[i].ToString());

                }
            }
            if (this.NumericValues.Length > 0)
            {
                string[] strArgs = this.SpiltArguments(this.NumericValues);
                for (int i = 0; i < strArgs.Length; i = i + 2)
                {
                    cmd.Parameters.Add(new SqlParameter("@" + strArgs[i].ToString(), strArgs[i + 1].ToString()));
                    paraReceived.Add("@" + strArgs[i].ToString());
                }
            }
            if (this.DateTimeValues.Length > 0)
            {
                string[] strArgs = this.SpiltArguments(this.DateTimeValues);
                for (int i = 0; i < strArgs.Length; i = i + 2)
                {
                    cmd.Parameters.Add(new SqlParameter("@" + strArgs[i].ToString(), strArgs[i + 1].ToString()));
                    paraReceived.Add("@" + strArgs[i].ToString());
                }
            }


            var regex = new Regex("{(.*?)}", RegexOptions.IgnoreCase);
            var matches = regex.Matches(sqlQuery);
            List<string> paraFromQuery = new List<string>();

            paraFromQuery = matches.Cast<Match>().Select(m => m.Value).Distinct().ToList();

            if (paraReceived.Count != paraFromQuery.Count)
            {
                throw new Exception("Number of parameters passed and defined in the query are not matching.");
            }
            #endregion


            da.Fill(ldt);

            this.GridSource = ldt;
        }
        private void RefreshSetting()
        {
            this.pgMain.OptionsView.ShowColumnGrandTotals = true;
            this.pgMain.OptionsView.ShowColumnTotals = true;
            this.pgMain.OptionsView.ShowColumnHeaders = true;
            this.pgMain.OptionsView.ShowHorzLines = true;
            this.pgMain.OptionsView.ShowVertLines = true;
            this.pgMain.OptionsView.ShowRowGrandTotals = true;
            this.pgMain.OptionsView.ShowRowHeaders = true;
            this.pgMain.OptionsView.ShowRowTotals = true;
            this.pgMain.OptionsView.ShowDataHeaders = true;
            this.pgMain.OptionsView.ShowFilterHeaders = true;

            this.bciShowColGrandTotal.Checked = this.pgMain.OptionsView.ShowColumnGrandTotals;
            this.bciShowColTotal.Checked = this.pgMain.OptionsView.ShowColumnTotals;
            this.bciShowColHeader.Checked = this.pgMain.OptionsView.ShowColumnHeaders;
            this.bciShowHoriLines.Checked = this.pgMain.OptionsView.ShowHorzLines;
            this.bciShowVertLines.Checked = this.pgMain.OptionsView.ShowVertLines;
            this.bciShowRowGrandTotal.Checked = this.pgMain.OptionsView.ShowRowGrandTotals;
            this.bciShowRowHeader.Checked = this.pgMain.OptionsView.ShowRowHeaders;
            this.bciShowRowTotal.Checked = this.pgMain.OptionsView.ShowRowTotals;
            this.bciShowDataHeader.Checked = this.pgMain.OptionsView.ShowDataHeaders;
            this.bciShowFilterHeader.Checked = this.pgMain.OptionsView.ShowFilterHeaders;
        }
        private void LoadInitSetting()
        {
            this.bciShowColGrandTotal.Checked = this.pgMain.OptionsView.ShowColumnGrandTotals;
            this.bciShowColTotal.Checked = this.pgMain.OptionsView.ShowColumnTotals;
            this.bciShowColHeader.Checked = this.pgMain.OptionsView.ShowColumnHeaders;
            this.bciShowHoriLines.Checked = this.pgMain.OptionsView.ShowHorzLines;
            this.bciShowVertLines.Checked = this.pgMain.OptionsView.ShowVertLines;
            this.bciShowRowGrandTotal.Checked = this.pgMain.OptionsView.ShowRowGrandTotals;
            this.bciShowRowHeader.Checked = this.pgMain.OptionsView.ShowRowHeaders;
            this.bciShowRowTotal.Checked = this.pgMain.OptionsView.ShowRowTotals;
            this.bciShowDataHeader.Checked = this.pgMain.OptionsView.ShowDataHeaders;
            this.bciShowFilterHeader.Checked = this.pgMain.OptionsView.ShowFilterHeaders;

            this.bciShowColGrandTotal.CheckedChanged += new ItemClickEventHandler(this.CheckBarItem_CheckedChanged);
            this.bciShowColTotal.CheckedChanged += new ItemClickEventHandler(this.CheckBarItem_CheckedChanged);
            this.bciShowColHeader.CheckedChanged += new ItemClickEventHandler(this.CheckBarItem_CheckedChanged);
            this.bciShowHoriLines.CheckedChanged += new ItemClickEventHandler(this.CheckBarItem_CheckedChanged);
            this.bciShowVertLines.CheckedChanged += new ItemClickEventHandler(this.CheckBarItem_CheckedChanged);
            this.bciShowRowGrandTotal.CheckedChanged += new ItemClickEventHandler(this.CheckBarItem_CheckedChanged);
            this.bciShowRowHeader.CheckedChanged += new ItemClickEventHandler(this.CheckBarItem_CheckedChanged);
            this.bciShowRowTotal.CheckedChanged += new ItemClickEventHandler(this.CheckBarItem_CheckedChanged);
            this.bciShowDataHeader.CheckedChanged += new ItemClickEventHandler(this.CheckBarItem_CheckedChanged);
            this.bciShowFilterHeader.CheckedChanged += new ItemClickEventHandler(this.CheckBarItem_CheckedChanged);

            this.bbiExportToCSV.ItemClick += new ItemClickEventHandler(this.Export_ItemClicked);
            this.bbiExportToHTML.ItemClick += new ItemClickEventHandler(this.Export_ItemClicked);
            this.bbiExportToPDF.ItemClick += new ItemClickEventHandler(this.Export_ItemClicked);
            this.bbiExportToRTF.ItemClick += new ItemClickEventHandler(this.Export_ItemClicked);
            this.bbiExportToText.ItemClick += new ItemClickEventHandler(this.Export_ItemClicked);
            this.bbiExportToXls.ItemClick += new ItemClickEventHandler(this.Export_ItemClicked);
            this.bbiExportToXlsx.ItemClick += new ItemClickEventHandler(this.Export_ItemClicked);

            this.bbiEmailToCSV.ItemClick += new ItemClickEventHandler(this.EmailToFile);
            this.bbiEmailToPdf.ItemClick += new ItemClickEventHandler(this.EmailToFile);
            this.bbiEmailToRTF.ItemClick += new ItemClickEventHandler(this.EmailToFile);
            this.bbiEmailToText.ItemClick += new ItemClickEventHandler(this.EmailToFile);
            this.bbiEmailToXls.ItemClick += new ItemClickEventHandler(this.EmailToFile);
            this.bbiEmailToXlsx.ItemClick += new ItemClickEventHandler(this.EmailToFile);

            this.pgMain.OptionsBehavior.HorizontalScrolling = PivotGridScrolling.Control;
            

        }
        private void AddFieldsToPivot()
        {
            System.Data.DataTable columnDetails = new System.Data.DataTable();
            columnDetails = _SettingTbl.Clone();

            for (int i = 0; i < this.GridSource.Columns.Count; i++)
            {
                DataRow row = columnDetails.NewRow();
                row["ReportId"] = this.ReportId;
                row["ColNm"] = this.GridSource.Columns[i].ColumnName.ToString().Trim();
                row["ColCap"] = this.GridSource.Columns[i].ColumnName.ToString().Trim();
                row["ColType"] = "Filter";
                row["ColDispWid"] = 125;
                row["ColOrder"] = i;
                row["IsActive"] = true;
                row["DataType"] = this.GridSource.Columns[i].DataType.Name.ToString();
                columnDetails.Rows.Add(row);
            }
            if (_SettingTbl.Rows.Count > 0)
            {
                for (int i = 0; i < this._SettingTbl.Rows.Count; i++)
                {
                    DataRow[] rows = columnDetails.Select("ReportId='" + this.ReportId + "' And Trim(ColNm)='" + this._SettingTbl.Rows[i]["ColNm"].ToString().Trim() + "'");
                    if (rows.Count() == 1)
                    {
                        rows[0]["ColCap"] = this._SettingTbl.Rows[i]["ColCap"];
                        rows[0]["ColType"] = this._SettingTbl.Rows[i]["ColType"];
                        rows[0]["ColDispWid"] = this._SettingTbl.Rows[i]["ColDispWid"];
                        rows[0]["ColOrder"] = this._SettingTbl.Rows[i]["ColOrder"];
                        rows[0]["IsActive"] = this._SettingTbl.Rows[i]["IsActive"];
                        rows[0]["colFormat"] = this._SettingTbl.Rows[i]["colFormat"];
                    }
                }
            }
            DataView ldv = columnDetails.DefaultView;
            ldv.Sort = "ColOrder,ColCap";
            ldv.RowFilter = "IsActive=1";

            for (int i = 0; i < ldv.Count; i++)
            {
                PivotGridField fld = null;
                switch (ldv[i]["ColType"].ToString().Trim().ToLower())
                {
                    case "filter":

                        fld = new PivotGridField(ldv[i]["ColNm"].ToString(), PivotArea.FilterArea);
                        if (filterFieldCount + 1 > maxfilterFieldCount)
                        {
                            fld.Visible = false;
                            fld.Options.ShowInCustomizationForm = true;
                        }
                        filterFieldCount++;
                        break;
                    case "data":
                        fld = new PivotGridField(ldv[i]["ColNm"].ToString(), PivotArea.DataArea);
                        break;
                    case "row":
                        fld = new PivotGridField(ldv[i]["ColNm"].ToString(), PivotArea.RowArea);
                        break;
                    case "column":
                        fld = new PivotGridField(ldv[i]["ColNm"].ToString(), PivotArea.ColumnArea);
                        break;
                    default:
                        break;
                }
                if (fld != null)
                {
                    fld.Name = "field" + ldv[i]["ColCap"].ToString();
                    fld.Caption = ldv[i]["ColCap"].ToString();
                    fld.Width = Convert.ToInt32(ldv[i]["ColDispWid"]);
                    
                    //Valueformat formats values within the Column Header Area and Row Header Area;
                    //CellFormat formats values within the Data Area;
                    //TotalCellFormat formats totals;
                    //GrandTotalCellFormat formats grand totals.;

                    switch (ldv[i]["DataType"].ToString().ToLower().Trim())
                    {
                        case "int32":
                        case "int64":
                        case "int":
                            fld.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;

                            break;
                        case "numeric":
                        case "decimal":

                            fld.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            fld.ValueFormat.FormatString = "#0.00";
                            fld.CellFormat.FormatString = "#0.00";
                            fld.TotalCellFormat.FormatString = "#0.00";
                            fld.GrandTotalCellFormat.FormatString = "#0.00";

                            if (ldv[i]["colFormat"].ToString().Trim().Length > 0)
                                fld.ValueFormat.FormatString = ldv[i]["colFormat"].ToString().Trim();

                            fld.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            if (ldv[i]["colFormat"].ToString().Trim().Length > 0)
                                fld.CellFormat.FormatString = ldv[i]["colFormat"].ToString().Trim();

                            fld.TotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            if (ldv[i]["colFormat"].ToString().Trim().Length > 0)
                                fld.TotalCellFormat.FormatString = ldv[i]["colFormat"].ToString().Trim();

                            fld.GrandTotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            if (ldv[i]["colFormat"].ToString().Trim().Length > 0)
                                fld.GrandTotalCellFormat.FormatString = ldv[i]["colFormat"].ToString().Trim();

                            break;
                        case "datetime":
                        case "date":
                            fld.ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                            if (ldv[i]["colFormat"].ToString().Trim().Length > 0)
                                fld.ValueFormat.FormatString = ldv[i]["colFormat"].ToString().Trim();
                            else
                                fld.ValueFormat.FormatString = "dd/MM/yyyy";


                            break;
                        default:
                            break;
                    }
                    this.pgMain.Fields.Add(fld);
                }
            }
            if (filterFieldCount > maxfilterFieldCount)
            {
                this.pgMain.FieldsCustomization();
            }
        }
        private string GetDefaultLayout()
        {
            System.Data.DataTable ldt = new System.Data.DataTable();
            SqlConnection oconn = new SqlConnection(SqlConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = oconn;
            cmd.CommandText = "Select Top 1 LayoutNm From UBIRepLayout Where ReportId=@ReportId and Username=@Username and isDefault=1";
            cmd.Parameters.Add(new SqlParameter("@ReportId", ReportId));
            cmd.Parameters.Add(new SqlParameter("@Username", UserName));
            SqlDataAdapter lda = new SqlDataAdapter(cmd);
            lda.Fill(ldt);
            return (ldt.Rows.Count > 0 ? ldt.Rows[0]["LayoutNm"].ToString() : "");
        }
        private int SaveLayout(bool addMode, bool editMode, bool isDefault)
        {
            bool addmode = addMode;
            bool editmode = editMode;
            int result = 0;
            XmlDocument xmlDocument = null;
            try
            {
                //workspaceManager1.CaptureWorkspace(_currentLayout, true);
                string fileName = Path.Combine(Path.GetTempPath(), UserName + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss")) + ".xml";

                //workspaceManager1.SaveWorkspace(_currentLayout, fileName, true);
                //workspaceManager1.Dispose();

                this.pgMain.SaveLayoutToXml(fileName);
                xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);

                SqlConnection oconn = new SqlConnection(SqlConnString);
                oconn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = oconn;
                if (addmode)
                {
                    cmd.CommandText = "Insert Into UBIRepLayout (ReportId,LayoutNm,UserName,LayoutData,IsDefault,SkinName) Values (@ReportId,@LayoutNm,@UserName,convert(varbinary(max),@LayoutData),@IsDefault,@SkinName)";
                    cmd.Parameters.Add(new SqlParameter("@ReportId", ReportId));
                    cmd.Parameters.Add(new SqlParameter("@LayoutNm", _currentLayout));
                    cmd.Parameters.Add(new SqlParameter("@UserName", UserName));
                    cmd.Parameters.Add(new SqlParameter("@LayoutData", xmlDocument.OuterXml.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@IsDefault", isDefault));
                    cmd.Parameters.Add(new SqlParameter("@SkinName", DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName));
                }
                else
                {
                    cmd.CommandText = "Update UBIRepLayout set LayoutData=convert(varbinary(max),@LayoutData), IsDefault=@IsDefault,SkinName=@SkinName Where ReportId=@ReportId and LayoutNm=@LayoutNm and UserName=@UserName ";
                    cmd.Parameters.Add(new SqlParameter("@ReportId", ReportId));
                    cmd.Parameters.Add(new SqlParameter("@LayoutNm", _currentLayout));
                    cmd.Parameters.Add(new SqlParameter("@UserName", UserName));
                    cmd.Parameters.Add(new SqlParameter("@LayoutData", xmlDocument.OuterXml.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@IsDefault", isDefault));
                    cmd.Parameters.Add(new SqlParameter("@SkinName", DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName));
                }
                result = cmd.ExecuteNonQuery();
                oconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.ApplText, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }
        private int DeleteLayout(string layoutName)
        {
            int reccount = 0;
            try
            {
                System.Data.DataTable ldt = new System.Data.DataTable();
                SqlConnection oconn = new SqlConnection(SqlConnString);
                SqlCommand cmd = new SqlCommand();
                oconn.Open();
                cmd.Connection = oconn;
                cmd.CommandText = "Delete From UBIRepLayout Where ReportId=@ReportId and LayoutNm=@LayoutNm and Username=@Username";
                cmd.Parameters.Add(new SqlParameter("@ReportId", ReportId));
                cmd.Parameters.Add(new SqlParameter("@LayoutNm", layoutName));
                cmd.Parameters.Add(new SqlParameter("@Username", UserName));
                reccount = cmd.ExecuteNonQuery();
                if (_currentLayout == layoutName)
                {
                    this.RefreshScreen();
                }
                oconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.ApplText, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return reccount;
        }
        private void ApplyLayout(string layoutName)
        {
            System.Data.DataTable ldt = new System.Data.DataTable();
            SqlConnection oconn = new SqlConnection(SqlConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = oconn;
            cmd.CommandText = "Select Top 1 LayoutData,SkinName From UBIRepLayout Where ReportId=@ReportId and LayoutNm=@LayoutNm and Username=@Username";
            cmd.Parameters.Add(new SqlParameter("@ReportId", ReportId));
            cmd.Parameters.Add(new SqlParameter("@LayoutNm", layoutName));
            cmd.Parameters.Add(new SqlParameter("@Username", UserName));
            SqlDataAdapter lda = new SqlDataAdapter(cmd);
            lda.Fill(ldt);

            {
                XmlDocument xmlDocument = new XmlDocument();
                byte[] myArray = (System.Byte[])ldt.Rows[0]["LayoutData"];
                MemoryStream fs = new MemoryStream(myArray);

                xmlDocument.Load(fs);
                string fileName = Path.Combine(Path.GetTempPath(), UserName + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss")) + ".xml";
                xmlDocument.Save(fileName);
                this.pgMain.RestoreLayoutFromXml(fileName, DevExpress.Utils.OptionsLayoutBase.FullLayout);
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(ldt.Rows[0]["SkinName"].ToString().Trim());
                this.pgMain.LookAndFeel.SetSkinStyle(ldt.Rows[0]["SkinName"].ToString().Trim());
                LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
                File.Delete(fileName);
            }

        }
        private void Export_ItemClicked(object sender, ItemClickEventArgs e)
        {
            string fileName = string.Empty;
            switch (e.Item.Tag.ToString().Trim())
            {
                case "CSV":
                    fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "CSV File|*.csv", string.Empty);
                    if (fileName.Length > 0)
                    {
                        SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                        Thread.Sleep(1);
                        this.pgMain.ExportToCsv(fileName);
                        SplashScreenManager.CloseForm();
                        this.OpenExportedFile(fileName);
                    }
                    break;
                case "HTML":
                    fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "HTML File|*.html", string.Empty);
                    if (fileName.Length > 0)
                    {
                        SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                        Thread.Sleep(1);
                        this.pgMain.ExportToHtml(fileName);
                        SplashScreenManager.CloseForm();
                        this.OpenExportedFile(fileName);
                    }
                    break;
                case "PDF":
                    fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "PDF File|*.pdf", string.Empty);
                    if (fileName.Length > 0)
                    {
                        SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                        Thread.Sleep(1);
                        this.pgMain.ExportToPdf(fileName);
                        SplashScreenManager.CloseForm();
                        this.OpenExportedFile(fileName);

                    }
                    break;
                case "XLS":
                    fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "Microsoft Excel Workbook(*.xls)|*.xls", string.Empty);
                    if (fileName.Length > 0)
                    {
                        SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                        Thread.Sleep(1);
                        var pivotExportOptions = new DevExpress.XtraPivotGrid.PivotXlsExportOptions();
                        //pivotExportOptions.ExportType = DevExpress.Export.ExportType.Default;
                        pivotExportOptions.ExportType = DevExpress.Export.ExportType.Default;
                        pivotExportOptions.AllowGrouping = DevExpress.Utils.DefaultBoolean.False;
                        pivotExportOptions.AllowFixedColumnHeaderPanel = DevExpress.Utils.DefaultBoolean.False;
                        pivotExportOptions.AllowLookupValues = DevExpress.Utils.DefaultBoolean.True;
                        pivotExportOptions.CustomizeSheetHeader += pivotExportOptions_CustomizeSheetHeader;         //Added by Shrikant S. on 05/03/2019 for AU 2.1.1
                        this.pgMain.ExportToXls(fileName, pivotExportOptions);

                        this.WriteHeaders(fileName);                                //Added by Shrikant S. on 06/03/2019 for AU 2.1.1
                        SplashScreenManager.CloseForm();
                        this.OpenExportedFile(fileName);
                    }
                    break;
                case "XLSX":
                    fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "Microsoft Excel Workbook(*.xlsx)|*.xlsx", string.Empty);
                    if (fileName.Length > 0)
                    {
                        SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                        Thread.Sleep(1);
                        var pivotExportOptions = new DevExpress.XtraPivotGrid.PivotXlsxExportOptions();
                        //pivotExportOptions.ExportType = DevExpress.Export.ExportType.Default;
                        pivotExportOptions.ExportType = DevExpress.Export.ExportType.Default;
                        pivotExportOptions.AllowGrouping = DevExpress.Utils.DefaultBoolean.False;
                        pivotExportOptions.AllowFixedColumnHeaderPanel= DevExpress.Utils.DefaultBoolean.False;
                        pivotExportOptions.AllowLookupValues= DevExpress.Utils.DefaultBoolean.True;
                        pivotExportOptions.CustomizeSheetHeader += pivotExportOptions_CustomizeSheetHeader;         //Added by Shrikant S. on 05/03/2019 for AU 2.1.1

                        this.pgMain.ExportToXlsx(fileName, pivotExportOptions);
                        this.WriteHeaders(fileName);                                //Added by Shrikant S. on 06/03/2019 for AU 2.1.1
                        SplashScreenManager.CloseForm();
                        this.OpenExportedFile(fileName);
                    }
                    break;
                case "RTF":
                    fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "RTF File|*.rtf", string.Empty);
                    if (fileName.Length > 0)
                    {
                        SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                        Thread.Sleep(1);
                        this.pgMain.ExportToRtf(fileName);
                        SplashScreenManager.CloseForm();
                        this.OpenExportedFile(fileName);
                    }
                    break;
                case "TXT":
                    fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "Text File|*.txt", string.Empty);
                    if (fileName.Length > 0)
                    {
                        SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                        Thread.Sleep(1);
                        this.pgMain.ExportToText(fileName);
                        SplashScreenManager.CloseForm();
                        this.OpenExportedFile(fileName);
                    }
                    break;
            }

        }
        //Added by Shrikant S. on 06/03/2019 for AU 2.1.1           //Start
        void pivotExportOptions_CustomizeSheetHeader(DevExpress.Export.ContextEventArgs e)
        {
            // Create a new row. 
            CellObject row = new CellObject();
            // Specify row values. 
            row.Value = this.Text;
            // Specify row formatting. 
            XlFormattingObject rowFormatting = new XlFormattingObject();
            rowFormatting.Font = new XlCellFont { Bold = true, Size = 12 };
            rowFormatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Left, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
            row.Formatting = rowFormatting;
            // Add the created row to the output document. 
            e.ExportContext.AddRow(new[] { row });
            //// Add an empty row to the output document. 
            //e.ExportContext.AddRow();
            //// Merge cells of two new rows.  
            //e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 0), new DevExpress.Export.Xl.XlCellPosition(5, 1)));
        }
        private void WriteHeaders(string xlsxFileName)
        {
            List<PivotGridField> fldList = this.pgMain.GetFieldsByArea(PivotArea.RowArea);

            Microsoft.Office.Interop.Excel.Application oXL = null;
            Microsoft.Office.Interop.Excel.Workbook oWB = null;
            Microsoft.Office.Interop.Excel.Worksheet oSheet = null;

            try
            {
                oXL = new Microsoft.Office.Interop.Excel.Application();
                oWB = oXL.Workbooks.Open(xlsxFileName, ReadOnly:false, Editable:true);
                oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oWB.Worksheets.Item[1];
                for (int i = 0; i < fldList.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.Range row1 = oSheet.Rows.Cells[3, i+1];
                    row1.Value = fldList[i].Caption;
                    
                }
                oWB.Save();

                oXL.Application.ActiveWorkbook.Save();
                oXL.Application.Quit();
                oXL.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oWB);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
 
        }
        //Added by Shrikant S. on 06/03/2019 for AU 2.1.1           //End


        private void OpenExportedFile(string fileName)
        {
            if (MessageBox.Show("Do you want to open this file?", "Export To...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    process.Start();
                }
                catch
                {
                    MessageBox.Show(this, "Cannot find an application on your system suitable for openning the file with exported data.",System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private string ExportTo(string exportTo, string dialogTitle, string filterFile, string directoryPath)
        {
            string filename = string.Empty;
            bool ans = false;
            string filePath = directoryPath;
            if (directoryPath.Length > 0)
            {
                ans = true;
                filename = Path.GetFileName(filePath);
            }
            else
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Title = dialogTitle;
                dlg.InitialDirectory = (this.AppPath);
                dlg.DefaultExt = exportTo;
                //dlg.CheckFileExists = true;
                dlg.FileName = this.Text;               //Added by Shrikant S. on 05/03/2019 for AU 2.1.1
                dlg.Filter = filterFile;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ans = true;
                    filename = Path.GetFileName(dlg.FileName);
                    filePath = Path.GetFullPath(dlg.FileName);
                }
            }
            if (ans)
            {

                filePath = filePath.Substring(0, filePath.Length - filename.Length);
                string[] spChar = new string[] { "~", "#", "%", "&", "*", ":", "<", ">", "{", "}", "?", "/", "\\", "|" };
                for (int i = 0; i < spChar.Length; i++)
                {
                    filename = filename.Replace(spChar[i], "_");
                }
                filename = Path.Combine(filePath, filename);
            }
            return filename;
        }
        private void EmailToFile(object sender, ItemClickEventArgs e)
        {
            DialogResult DR = MessageBox.Show("Do you want to send e-mail?", "Send File", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (DR == DialogResult.Yes)
            {
                try
                {
                    string fileName = string.Empty;
                    Stream fileStream = new System.IO.MemoryStream();
                    switch (e.Item.Tag.ToString().Trim())
                    {
                        case "CSV":
                            fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "CSV File|*.csv", Path.ChangeExtension(Path.GetTempPath() + ReportName, ".csv"));
                            if (fileName.Length > 0)
                            {
                                SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                                Thread.Sleep(1);
                                this.pgMain.ExportToCsv(fileName);
                                SplashScreenManager.CloseForm();
                            }
                            break;
                        case "HTML":
                            fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "HTML File|*.html", Path.ChangeExtension(Path.GetTempPath() + ReportName, ".html"));
                            if (fileName.Length > 0)
                            {
                                SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                                Thread.Sleep(1);
                                this.pgMain.ExportToHtml(fileName);
                                SplashScreenManager.CloseForm();
                            }
                            break;
                        case "PDF":
                            fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "PDF File|*.pdf", Path.ChangeExtension(Path.GetTempPath() + ReportName, ".pdf"));
                            if (fileName.Length > 0)
                            {
                                SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                                Thread.Sleep(1);
                                this.pgMain.ExportToPdf(fileName);
                                SplashScreenManager.CloseForm();
                            }
                            break;
                        case "XLS":
                            fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "Microsoft Excel Workbook(*.xls)|*.xls", Path.ChangeExtension(Path.GetTempPath() + ReportName, ".xls"));
                            if (fileName.Length > 0)
                            {
                                SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                                Thread.Sleep(1);
                                var pivotExportOptions = new DevExpress.XtraPivotGrid.PivotXlsExportOptions();
                                //pivotExportOptions.ExportType = DevExpress.Export.ExportType.Default;
                                pivotExportOptions.ExportType = DevExpress.Export.ExportType.Default;
                                pivotExportOptions.AllowGrouping = DevExpress.Utils.DefaultBoolean.False;
                                pivotExportOptions.AllowFixedColumnHeaderPanel = DevExpress.Utils.DefaultBoolean.False;
                                pivotExportOptions.AllowLookupValues = DevExpress.Utils.DefaultBoolean.True;
                                pivotExportOptions.CustomizeSheetHeader += pivotExportOptions_CustomizeSheetHeader;         //Added by Shrikant S. on 05/03/2019 for AU 2.1.1
                                this.pgMain.ExportToXls(fileName, pivotExportOptions);

                                this.WriteHeaders(fileName);                                //Added by Shrikant S. on 06/03/2019 for AU 2.1.1
                                SplashScreenManager.CloseForm();
                            }
                            break;
                        case "XLSX":
                            fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "Microsoft Excel Workbook(*.xlsx)|*.xlsx", Path.ChangeExtension(Path.GetTempPath() + ReportName, ".xlsx"));
                            if (fileName.Length > 0)
                            {
                                SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                                Thread.Sleep(1);
                                var pivotExportOptions = new DevExpress.XtraPivotGrid.PivotXlsxExportOptions();
                                pivotExportOptions.ExportType = DevExpress.Export.ExportType.Default;
                                pivotExportOptions.AllowGrouping = DevExpress.Utils.DefaultBoolean.False;
                                pivotExportOptions.AllowFixedColumnHeaderPanel = DevExpress.Utils.DefaultBoolean.False;
                                pivotExportOptions.AllowLookupValues = DevExpress.Utils.DefaultBoolean.True;
                                pivotExportOptions.CustomizeSheetHeader += pivotExportOptions_CustomizeSheetHeader;         //Added by Shrikant S. on 05/03/2019 for AU 2.1.1

                                this.pgMain.ExportToXlsx(fileName, pivotExportOptions);
                                this.WriteHeaders(fileName);                                //Added by Shrikant S. on 06/03/2019 for AU 2.1.1

                                SplashScreenManager.CloseForm();
                            }
                            break;
                        case "RTF":
                            fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "RTF File|*.rtf", Path.ChangeExtension(Path.GetTempPath() + ReportName, ".rtf"));
                            if (fileName.Length > 0)
                            {
                                SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                                Thread.Sleep(1);
                                this.pgMain.ExportToRtf(fileName);
                                SplashScreenManager.CloseForm();
                            }
                            break;
                        case "TXT":
                            fileName = this.ExportTo(e.Item.Tag.ToString().Trim(), "Save File", "Text File|*.txt", Path.ChangeExtension(Path.GetTempPath() + ReportName, ".txt"));
                            if (fileName.Length > 0)
                            {
                                SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
                                Thread.Sleep(1);
                                this.pgMain.ExportToText(fileName);
                                SplashScreenManager.CloseForm();
                            }
                            break;
                    }
                    if (fileName.Length > 0)
                    {
                        string defaSetting = this.GetDefaultEmailSetting();

                        if (defaSetting != "SMTP")
                        {
                            try
                            {
                                var oApp = new Microsoft.Office.Interop.Outlook.Application();
                                Microsoft.Office.Interop.Outlook._NameSpace ns = oApp.GetNamespace("MAPI");
                                //var f = ns.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderInbox);
                                //Thread.Sleep(5000); // a bit of startup grace time.
                                
                                var mailItem = (Microsoft.Office.Interop.Outlook._MailItem)oApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                                mailItem.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML;
                                mailItem.Subject = "Reports : " + ReportName;
                                StringBuilder mailbody = new StringBuilder();


                                mailItem.HTMLBody = "<html><body> Dear Sir / Madam, <br> <br> Please Find the Attached Report:" + ReportName + ".</b> <br><br><br> Thanks & Regards,<br> " + UserName + "</body></html>";
                                mailItem.To = "";
                                mailItem.Attachments.Add(fileName, Microsoft.Office.Interop.Outlook.OlAttachmentType.olByValue, 1, Path.GetFileName(fileName));
                                mailItem.Display(true);
                                //mailItem.Send();
                            }
                            catch (Exception)
                            {
                                defaSetting = "SMTP";
                            }
                        }
                        if (defaSetting == "SMTP")
                        {
                            frmSendMail frmsend = new frmSendMail(SqlConnString, fileName, ReportName, UserName);
                            frmsend.LookAndFeel.SetSkinStyle(this.pgMain.LookAndFeel.SkinName);
                            frmsend.ShowDialog();
                        }


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Mail Sending failed, please send once again. \r\n The Error Is: " + ex.Message, this.ApplText, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                }
            }
        }

        private string GetDefaultEmailSetting()
        {
            string retvalue = string.Empty;
            System.Data.DataTable ldt = new System.Data.DataTable();
            SqlConnection oconn = new SqlConnection(SqlConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = oconn;
            cmd.CommandText = "select * from emailsettings";
            SqlDataAdapter lda = new SqlDataAdapter(cmd);
            lda.Fill(ldt);

            if (ldt.Columns.Contains("IsSMTPDefa"))
            {
                retvalue=((ldt.Rows.Count > 0 ?Convert.ToBoolean(ldt.Rows[0]["IsSMTPDefa"]) : false)?"SMTP":"");
            }
            return retvalue;
        }

        private void CheckBarItem_CheckedChanged(object sender, EventArgs e)
        {
            this.pgMain.OptionsView.ShowColumnGrandTotals = this.bciShowColGrandTotal.Checked;
            this.pgMain.OptionsView.ShowColumnTotals = this.bciShowColTotal.Checked;
            this.pgMain.OptionsView.ShowColumnHeaders = this.bciShowColHeader.Checked;
            this.pgMain.OptionsView.ShowHorzLines = this.bciShowHoriLines.Checked;
            this.pgMain.OptionsView.ShowVertLines = this.bciShowVertLines.Checked;
            this.pgMain.OptionsView.ShowRowGrandTotals = this.bciShowRowGrandTotal.Checked;
            this.pgMain.OptionsView.ShowRowHeaders = this.bciShowRowHeader.Checked;
            this.pgMain.OptionsView.ShowRowTotals = this.bciShowRowTotal.Checked;
            this.pgMain.OptionsView.ShowDataHeaders = this.bciShowDataHeader.Checked;
            this.pgMain.OptionsView.ShowFilterHeaders = this.bciShowFilterHeader.Checked;
        }
        #endregion

        private void rbnMainform_Load(object sender, EventArgs e)
        {
            Screen scr = Screen.FromControl(this);
            this.Width = scr.WorkingArea.Width;
            this.Height = scr.WorkingArea.Height;

            SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
            SplashScreenManager.Default.SetWaitFormDescription("Loading setting...");
            Thread.Sleep(1);
            try
            {
                this.LoadInitSetting();
                this.GetCompanyDetails();

                SplashScreenManager.Default.SetWaitFormDescription("Fetching Data...");
                Thread.Sleep(1);
                this.GetDataSource(ReportId);

                this.pgMain.DataSource = this.GridSource;

                pgMain.OptionsChartDataSource.ProvideDataByColumns = true;
                pgMain.OptionsChartDataSource.SelectionOnly = true;
                pgMain.OptionsChartDataSource.ProvideColumnGrandTotals = true;
                pgMain.OptionsChartDataSource.ProvideRowGrandTotals = true;
                chartMain.CrosshairOptions.ShowArgumentLine = false;
                chartMain.DataSource = this.pgMain;

                ViewType[] restrictedTypes = new ViewType[] { ViewType.PolarArea, ViewType.PolarLine, ViewType.ScatterPolarLine, ViewType.SideBySideGantt, ViewType.Bubble,
                ViewType.SideBySideRangeBar, ViewType.RangeBar, ViewType.Gantt, ViewType.PolarPoint, ViewType.Stock, ViewType.CandleStick,
                ViewType.SideBySideFullStackedBar, ViewType.SideBySideFullStackedBar3D, ViewType.SideBySideStackedBar, ViewType.SideBySideStackedBar3D };
                foreach (ViewType type in Enum.GetValues(typeof(ViewType)))
                {
                    if (Array.IndexOf<ViewType>(restrictedTypes, type) >= 0) continue;
                    comboChartType.Properties.Items.Add(type);
                }
                comboChartType.SelectedItem = ViewType.StackedBar;


                bsiRecords.Caption = "Records : " + this.GridSource.Rows.Count.ToString();
                this.Text = ReportName + " - " + _CompanyDetails.Rows[0]["Co_name"].ToString().Trim() + "(" + CoSta_dt.Substring(6, 4) + "-" + CoEnd_dt.Substring(6, 4) + ")";

                SplashScreenManager.Default.SetWaitFormDescription("Applying setting...");
                Thread.Sleep(1);
                this.AddFieldsToPivot();
                _currentLayout = GetDefaultLayout();
                if (_currentLayout != string.Empty)
                {
                    this.ApplyLayout(_currentLayout);
                }
                this.splitContainerControl2.SplitterPosition = (this.Width / 2);

                this.splitContainerControl1.Height = this.Width - this.ribbonControl.Height - this.ribbonStatusBar1.Height;

                this.pgMain.OptionsMenu.EnableFormatRulesMenu = true;

                this.mInsertProcessIdRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.ApplText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Windows.Forms.Application.Exit();
            }
            SplashScreenManager.CloseForm();

        }

        private void bbiExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MessageBox.Show(this, "Do you really want to exit?", this.ApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                this.mDeleteProcessIdRecord();
                System.Windows.Forms.Application.Exit();
            }
        }

        private void srgbiSkin_GalleryItemClick(object sender, DevExpress.XtraBars.Ribbon.GalleryItemClickEventArgs e)
        {
            this.pgMain.LookAndFeel.SetSkinStyle(e.Item.Caption);
            LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
        }

        private void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.pgMain.ShowPrintPreview();
        }

        private void bbiPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.pgMain.Print();
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(frmProgress), true, true, false);
            SplashScreenManager.Default.SetWaitFormDescription("Loading setting...");
            Thread.Sleep(1);
            this.RefreshSetting();
            this.pgMain.Fields.Clear();
            this._SettingTbl.Rows.Clear();
            this.GridSource = null;
            try
            {
                SplashScreenManager.Default.SetWaitFormDescription("Fetching Data...");
                Thread.Sleep(1);
                this.GetDataSource(ReportId);

                pgMain.DataSource = this.GridSource;
                SplashScreenManager.Default.SetWaitFormDescription("Applying setting...");
                Thread.Sleep(1);
                this.AddFieldsToPivot();
                if (_currentLayout != string.Empty)
                {
                    this.ApplyLayout(_currentLayout);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                System.Windows.Forms.Application.Exit();
            }
            SplashScreenManager.CloseForm();
        }

        private void bbiSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            bool addmode = false;
            bool editmode = false;
            bool isDefault = false;
            if (_currentLayout != string.Empty)
            {
                DialogResult result = MessageBox.Show("Do you want to SaveAs the layout?", this.ApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    addmode = true;
                    _currentLayout = string.Empty;
                }
                else
                    editmode = true;
            }
            else
                addmode = true;

            frmLayout frm = new frmLayout(SqlConnString, UserName, ReportId, addmode, editmode, _currentLayout);
            frm.Icon = this.Icon;
            frm.Text = "Save Layout";
            frm.LookAndFeel.SetSkinStyle(this.pgMain.LookAndFeel.SkinName);
            frm.ShowDialog();
            _currentLayout = frm.LayoutName;

            if (_currentLayout != string.Empty)
                this.SaveLayout(addmode, editmode, frm.IsDefault);
        }

        private void bbiOpen_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmLayout frm = new frmLayout(SqlConnString, UserName, ReportId);
            frm.Icon = this.Icon;
            if (frm.LayoutCount > 0)
            {
                frm.LookAndFeel.SetSkinStyle(this.pgMain.LookAndFeel.SkinName);
                frm.ShowDialog();
            }
            _currentLayout = frm.LayoutName;
            //string _layoutSelected = string.Empty;
            //_layoutSelected = frm.LayoutName;
            if (_currentLayout.Trim().Length > 0)
            {
                this.ApplyLayout(_currentLayout);
            }
        }

        private void bbiClear_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (PivotGridField field in pgMain.Fields)
            {
                field.TopValueCount = field.GetUniqueValues().Length;
            }
            if (this.seTop.Visible == true)
                this.bbiTopNValues.PerformClick();
            this.RefreshScreen();
        }

        private void bbiDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmLayout frm = new frmLayout(SqlConnString, UserName, ReportId);
            frm.Icon = this.Icon;
            if (frm.LayoutCount > 0)
            {
                frm.LookAndFeel.SetSkinStyle(this.pgMain.LookAndFeel.SkinName);
                frm.ShowDialog();
            }
            string _layoutSelected = string.Empty;
            _layoutSelected = frm.LayoutName;

            if (_layoutSelected.Trim().Length > 0)
            {
                DialogResult result = MessageBox.Show("Do you want to Delete the selected Layout?", this.ApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (this.DeleteLayout(_layoutSelected) > 0)
                    {
                        MessageBox.Show("Layout deleted successfully.", this.ApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void bbiChart_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
            this.splitContainerControl2.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2;
            this.splitContainerControl3.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2;
            this.splitContainerControl3.Visible = true;
            this.splitContainerControl3.Refresh();
            this.Refresh();
        }

        private void pgMain_Click(object sender, EventArgs e)
        {

        }

        private void bbiBoth_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
            this.splitContainerControl2.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
            this.splitContainerControl3.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
            this.splitContainerControl3.Visible = true;
            this.splitContainerControl3.Refresh();
            this.splitContainerControl3.SplitterPosition = this.splitContainerControl3.Width / 2;
            this.Refresh();
        }

        private void bbiData_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
            this.splitContainerControl2.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            this.splitContainerControl3.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            if (this.splitContainerControl2.PanelVisibility == DevExpress.XtraEditors.SplitPanelVisibility.Panel1)
            {
                if (this.seTop.Visible == false)
                {
                    this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2;
                    //this.splitContainerControl3.Visible = false;
                    //this.splitContainerControl3.Refresh();
                }
                else
                {
                    this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
                    //this.splitContainerControl3.Visible = true;
                    //this.splitContainerControl3.Refresh();
                }
            }
            else
            {
                this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
            }
            this.Refresh();
        }

        private void bbiTopNValues_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.splitContainerControl2.PanelVisibility != DevExpress.XtraEditors.SplitPanelVisibility.Panel2)
            {
                if (this.seTop.Visible == false)
                {
                    this.lblfldValue.Visible = true;
                    this.lblShowTop.Visible = true;
                    this.icbField.Visible = true;
                    this.seTop.Visible = true;
                    if (this.splitContainerControl2.PanelVisibility == DevExpress.XtraEditors.SplitPanelVisibility.Panel1)
                    {
                        this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
                        this.splitContainerControl3.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
                    }
                    InitComboBoxes();
                    icbField.SelectedIndex = icbField.Properties.Items.Count - 1;
                    this.pgMain.OptionsCustomization.AllowFilterBySummary = false;
                    this.SetFieldTop();
                }
                else
                {
                    this.lblfldValue.Visible = false;
                    this.lblShowTop.Visible = false;
                    this.icbField.Visible = false;
                    this.seTop.Visible = false;
                    if (this.splitContainerControl2.PanelVisibility != DevExpress.XtraEditors.SplitPanelVisibility.Both)
                        this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2;
                    else
                        this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
                }
            }
            else
            {
                this.lblfldValue.Visible = false;
                this.lblShowTop.Visible = false;
                this.icbField.Visible = false;
                this.seTop.Visible = false;

            }
        }

        private void comboChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            chartMain.SeriesTemplate.ChangeView((ViewType)comboChartType.SelectedItem);
            //if (chartMain.SeriesTemplate.Label != null)
            //{
            //    chartMain.SeriesTemplate.LabelsVisibility = checkShowPointLabels.Checked ?
            //        DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
            //    chartMain.CrosshairEnabled = checkShowPointLabels.Checked ?
            //        DevExpress.Utils.DefaultBoolean.False : DevExpress.Utils.DefaultBoolean.True;
            //    chartMain.Enabled = true;
            //}
            //else
            //{
            //    checkShowPointLabels.Enabled = false;
            //}

            chartMain.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            if (chartMain.Diagram is Diagram3D)
            {
                Diagram3D diagram = (Diagram3D)chartMain.Diagram;
                diagram.RuntimeRotation = true;
                diagram.RuntimeZooming = true;
                diagram.RuntimeScrolling = true;
            }
            foreach (DevExpress.XtraCharts.Series series in chartMain.Series)
                UpdateSeriesTransparency(series.View);
            UpdateSeriesTransparency(chartMain.SeriesTemplate.View);
        }
        void UpdateSeriesTransparency(SeriesViewBase seriesView)
        {
            ISupportTransparency supportTransparency = seriesView as ISupportTransparency;
            if (supportTransparency != null)
            {
                if ((seriesView is AreaSeriesView) || (seriesView is Area3DSeriesView)
                    || (seriesView is RadarAreaSeriesView) || (seriesView is Bar3DSeriesView))
                    supportTransparency.Transparency = 135;
                else
                    supportTransparency.Transparency = 0;
            }
        }

        private void icbField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (icbField.Visible == true)
            {
                pgMain.BeginUpdate();
                foreach (PivotGridField field in pgMain.Fields)
                {
                    //field.Area = PivotArea.RowArea;
                    //bool IsVisible = field == (PivotGridField)icbField.EditValue;
                    //field.Visible = IsVisible;
                    if (field == (PivotGridField)icbField.EditValue)
                    {
                        seTop.Properties.MaxValue = field.GetUniqueValues().Length;
                        SetFieldTop();
                    }
                    //if (IsVisible)
                    //{
                    //    seTop.Properties.MaxValue = field.GetUniqueValues().Length;
                    //    SetFieldTop();
                    //}
                }
                pgMain.EndUpdate();
            }
        }

        private void seTop_EditValueChanged(object sender, EventArgs e)
        {
            if (seTop.Visible == true)
            {
                SetFieldTop();
            }
        }
        void SetFieldTop()
        {
            PivotGridField field = icbField.EditValue as PivotGridField;
            if (field == null) return;

            field.TopValueCount = Convert.ToInt32(seTop.Value);
            //field.TopValueShowOthers = ceTopValuesShowOthers.Checked;
            this.pgMain.RefreshData();
        }
        void InitComboBoxes()
        {
            icbField.Properties.Items.Clear();
            foreach (PivotGridField field in pgMain.Fields)
                if (field.Area == PivotArea.RowArea)
                    icbField.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(field.Caption.ToString(), field, -1));

            //icbField.EditValue = fieldSalesPerson.SortBySummaryInfo.Field;
        }

        private void rbnMainform_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mDeleteProcessIdRecord();
        }
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.ApplCode + "','" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "','" + this.ApplName + "'," + this.ApplPId.ToString() + ",'" + this.ApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            try
            {
                SqlConnection oconn = new SqlConnection(SqlConnString);
                oconn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = oconn;
                cmd.CommandText = sqlstr;

                cmd.ExecuteNonQuery();
                oconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.ApplText, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void mDeleteProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.ApplName + "' and pApplId=" + this.ApplPId + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            try
            {
                SqlConnection oconn = new SqlConnection(SqlConnString);
                oconn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = oconn;
                cmd.CommandText = sqlstr;

                cmd.ExecuteNonQuery();
                oconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.ApplText, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
