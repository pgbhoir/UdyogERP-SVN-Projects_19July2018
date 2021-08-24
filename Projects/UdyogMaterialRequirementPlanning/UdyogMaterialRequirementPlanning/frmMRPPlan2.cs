using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;      // Added by Sachin N. S. on 25/10/2019 for Bug-32914
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid.Views.Base;       // Added by Sachin N. S. on 25/10/2019 for Bug-32914
using System.Data.SqlClient;
using DataAccess_Net;               // Added by Sachin N. S. on 25/10/2019 for Bug-32914
using System.IO;
using ueconnect;

namespace UdyogMaterialRequirementPlanning
{
    public partial class frmMRPPlan2 : Form
    {
        private DataTable PendingData;
        DataTable finalTable = new DataTable();
        DataTable RawMatTable = new DataTable();
        DataTable dtMrpLog = new DataTable("MrpLog_vw");
        DataAccess_Net.clsDataAccess oDataAccess;       // Added by Sachin N. S. on 25/10/2019 for Bug-32914
        clsConnect oConnect;        // Added by Sachin N. S. on 07/11/2019 for Bug-32925
        string serviceType = "Demo Version";        // Added by Sachin N. S. on 07/11/2019 for Bug-32925
        string ItemName;

        public frmMRPPlan2(DataTable pendingData)
        {
            InitializeComponent();
            PendingData = pendingData;
            PendingData.TableName = "PendData";
            //DataColumn colItem = new DataColumn("Item", typeof(string));
            //RawMatTable.Columns.Add(colItem);
            //DataColumn colrmItem = new DataColumn("rmItem", typeof(string));
            //RawMatTable.Columns.Add(colrmItem);
            //DataColumn colReq_Qty = new DataColumn("Req_qty", typeof(decimal));
            //RawMatTable.Columns.Add(colReq_Qty);

        }

        private void btnProceed_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clsCommon.DeleteProcessIdRecord();
            Application.Exit();
        }

        private void frmMRPPlan2_Load(object sender, EventArgs e)
        {
            this.label1.Text = "Order Entries:";
            if (clsCommon.IconFile != null)
                this.Icon = new Icon(clsCommon.IconFile);
            //Added by Prajakta B. on 23/04/2020 for Bug 33359   Start
            string sqlstr = "Select It_Heading from Vudyog..Co_mast where DbName='" + clsCommon.DbName + "' and compid=" + clsCommon.CompId.ToString();
            SqlConnection con = new SqlConnection(clsCommon.ConnStr);
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            if (con.State == ConnectionState.Closed)
                con.Open();
            ItemName = Convert.ToString(cmd.ExecuteScalar());
            //Added by Prajakta B. on 23/04/2020 for Bug 33359   End
            this.GridSetting();
            this.DoPlanning();
            this.SetMRPGrid();
            this.Text = clsCommon.ApplName;

            //***** Added by Sachin N. S. on 07/11/2019 for Bug-32925 -- Start
            string appPath = Application.StartupPath;

            DirectoryInfo dir = new DirectoryInfo(appPath);

            System.Linq.IOrderedEnumerable<FileInfo> totalFile = (from f in dir.GetFiles("*register.Me") orderby f.LastWriteTime descending select f);

            string m_registerMePath = string.Empty;

            oConnect = new clsConnect();
            oConnect.InitProc("'" + appPath + "'", clsCommon.pApplNm.Substring(0, clsCommon.pApplNm.Length - 4));
            
            if (totalFile.Count() > 0)
            {
                for (int i = 0; i < totalFile.Count(); i++)
                {
                    m_registerMePath = Path.GetFileName(totalFile.ElementAt(i).ToString());
                    string[] objRegisterMe = (oConnect.ReadRegiValue(appPath)).Split('^');
                    serviceType = objRegisterMe[15].ToString().Trim();
                    break;
                }
            }
            //***** Added by Sachin N. S. on 07/11/2019 for Bug-32925 -- End
        }


        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
        private void DoPlanning()
        {
            string SqlStr = string.Empty;
            string BomNotDefined = string.Empty;
            Double AdjustedQty = 0.0000;

            SqlConnection conn = new SqlConnection(clsCommon.ConnStr);
            //SqlCommand cmd = new SqlCommand("Select * From MrpLog Where 1=2", conn);  //Commented by Priyanka B on 27042018 for Bugs 31390 & 31306
            SqlCommand cmd = new SqlCommand("Select *,ItemBom='' From MrpLog Where 1=2", conn);  //Modified by Priyanka B on 27042018 for Bugs 31390 & 31306
            cmd.CommandTimeout = 60000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtMrpLog);
            DataColumn colsItemLvl = new DataColumn("ItemLvl", typeof(short));
            dtMrpLog.Columns.Add(colsItemLvl);

            DataView view = PendingData.DefaultView;

            view.RowFilter = "Sel = 1";
            // then get the distinct table...
            //DataTable distinctItem = view.ToTable("PendData", true, new string[] { "Item", "It_code" });  //Commented by Priyanka B on 27042018 for Bugs 31390 & 31306
            DataTable distinctItem = view.ToTable("PendData", true, new string[] { "Item", "It_code", "BomId", "ItemBom" });  //Modified by Priyanka B on 27042018 for Bugs 31390 & 31306

            for (int i = 0; i < distinctItem.Rows.Count; i++)
            {
                //AdjustedQty = Convert.ToDouble(PendingData.Compute("Sum(AdjustQty)", "Sel=True and It_code=" + Convert.ToString(distinctItem.Rows[i]["It_code"]).Trim()));  //Commented by Priyanka B on 27042018 for Bugs 31390 & 31306
                AdjustedQty = Convert.ToDouble(PendingData.Compute("Sum(AdjustQty)", "Sel=True and It_code=" + Convert.ToString(distinctItem.Rows[i]["It_code"]).Trim() + " and BomId=" + distinctItem.Rows[i]["BomId"].ToString().Trim()));  //Modified by Priyanka B on 27042018 for Bugs 31390 & 31306

                conn = new SqlConnection(clsCommon.ConnStr);
                // cmd = new SqlCommand("Execute USP_MRP_PLANNING @Item,@Qty,@Warehouse", conn); // commented by Suraj Kumawat for bug-29249 
                //cmd = new SqlCommand("Execute USP_MRP_PLANNING @Item,@Qty,@Warehouse,@Wharehouseapplicable", conn);  //Commented by Priyanka B on 27042018 for Bugs 31390 & 31306
                cmd = new SqlCommand("Execute USP_MRP_PLANNING @Item,@Qty,@Warehouse,@Wharehouseapplicable,@BomId", conn);  //Modified by Priyanka B on 27042018 for Bugs 31390 & 31306
                cmd.Parameters.Add(new SqlParameter("@Item", Convert.ToString(distinctItem.Rows[i]["Item"]).Trim()));
                cmd.Parameters.Add(new SqlParameter("@Qty", AdjustedQty));
                cmd.Parameters.Add(new SqlParameter("@Warehouse", clsCommon.Warehouse));
                cmd.Parameters.Add(new SqlParameter("@Wharehouseapplicable", clsCommon.Wharehouseapplicable)); // added by Sruaj Kumawat for Bug-29249 
                cmd.Parameters.Add(new SqlParameter("@BomId", distinctItem.Rows[i]["BomId"].ToString().Trim())); //Added by Priyanka B on 27042018 for Bugs 31390 & 31306
                cmd.CommandTimeout = 60000; 
                da = new SqlDataAdapter(cmd);
                DataTable ldt = new DataTable();
                da.Fill(ldt);


                if (finalTable.Rows.Count == 0)
                {
                    finalTable = ldt.Clone();
                    finalTable.TableName = "FinalData";
                    RawMatTable = ldt.Clone();
                }
                foreach (DataRow ldr in ldt.Rows)
                {
                    bool itemFound = false;
                    for (int k = 0; k < finalTable.Rows.Count; k++)
                    {
                        if (finalTable.Rows[k]["rmitem"].ToString().Trim() == ldr["rmitem"].ToString().Trim())
                        {
                            itemFound = true;
                            finalTable.Rows[k]["req_qty"] = Convert.ToDecimal(finalTable.Rows[k]["req_qty"]) + Convert.ToDecimal(ldr["Req_qty"]);
                            //finalTable.Rows[k]["indent_qty"] = Convert.ToDecimal(0) : Convert.ToDecimal(finalTable.Rows[k]["req_qty"]) - Convert.ToDecimal(finalTable.Rows[k]["stock_avl"]);
                            decimal IndentVal = Convert.ToDecimal(finalTable.Rows[k]["req_qty"]) - Convert.ToDecimal(finalTable.Rows[k]["stock_avl"]);
                            finalTable.Rows[k]["indent_qty"] = (IndentVal < 0 ? Convert.ToDecimal(0) : IndentVal);
                            break;
                        }
                    }
                    if (itemFound == false)
                    {
                        finalTable.ImportRow(ldr);
                    }
                    RawMatTable.ImportRow(ldr);
                }

                for (int m = 0; m < PendingData.Rows.Count; m++)
                {
                    if (Convert.ToString(distinctItem.Rows[i]["It_code"]).Trim() ==Convert.ToString(PendingData.Rows[m]["It_code"]).Trim())
                    {
                        DataRow LogRow = dtMrpLog.NewRow();
                        LogRow["Item"] = distinctItem.Rows[i]["Item"];
                        LogRow["It_code"] = distinctItem.Rows[i]["It_code"];
                        LogRow["Ware_nm"] = clsCommon.Warehouse;
                        LogRow["Orderno"] = PendingData.Rows[m]["Inv_no"];
                        LogRow["Qty"] = PendingData.Rows[m]["AdjustQty"];
                        LogRow["Entry_ty"] = PendingData.Rows[m]["Entry_ty"];
                        LogRow["Tran_cd"] = PendingData.Rows[m]["Tran_cd"];
                        LogRow["Itserial"] = PendingData.Rows[m]["Itserial"];
                        LogRow["ItemLvl"] = 0; 
                        LogRow["rEntry_ty"] = "";
                        LogRow["itref_tran"] = 0;
                        LogRow["rItserial"] = "";
                        LogRow["rIt_code"] = distinctItem.Rows[i]["It_code"];
                        LogRow["rqty"] = PendingData.Rows[m]["AdjustQty"];
                        LogRow["ItemBom"] = distinctItem.Rows[i]["ItemBom"].ToString().Trim();  //Added by Priyanka B on 27042018 for Bugs 31390 & 31306
                        dtMrpLog.Rows.Add(LogRow);

                        
                        foreach (DataRow ldr in ldt.Rows)
                        {
                            LogRow = dtMrpLog.NewRow();
                            LogRow["Item"] = distinctItem.Rows[i]["Item"];
                            LogRow["It_code"] = distinctItem.Rows[i]["It_code"]; 
                            LogRow["Ware_nm"] = clsCommon.Warehouse;
                            LogRow["Orderno"] = PendingData.Rows[m]["Inv_no"];
                            LogRow["Qty"] = PendingData.Rows[m]["AdjustQty"];
                            LogRow["Entry_ty"] = PendingData.Rows[m]["Entry_ty"];
                            LogRow["Tran_cd"] = PendingData.Rows[m]["Tran_cd"];
                            LogRow["Itserial"] = PendingData.Rows[m]["Itserial"];
                            LogRow["ItemLvl"] = (Convert.ToBoolean(ldr["IsSubBOM"])?1:2);
                            LogRow["rEntry_ty"] = "";
                            LogRow["itref_tran"] = 0;
                            LogRow["rItserial"] = "";
                            LogRow["rIt_code"] = ldr["It_code"]; ;
                            LogRow["rqty"] = ldr["Req_qty"];
                            LogRow["ItemBom"] = distinctItem.Rows[i]["ItemBom"].ToString().Trim();  //Added by Priyanka B on 27042018 for Bugs 31390 & 31306
                            dtMrpLog.Rows.Add(LogRow);
                        }
                    }
                    
                }

            }
        }
        private void SetMRPGrid()
        {
            gridControl2.DataSource = finalTable;
            gridControl2.ForceInitialize();
            gridView2.PopulateColumns();
            gridView2.OptionsPrint.AutoWidth = false;
            gridControl2.MainView = gridView2;
            gridView2.OptionsView.ShowGroupPanel = false;
            
            //gridView2.Columns["Item"].GroupIndex = 0;
            gridView2.Columns["It_code"].Visible = false;
            gridView2.Columns["IsSubBOM"].Visible = false;
            gridView2.Columns["pItem"].Visible = false;
            gridView2.Columns["order_qty"].Visible = false;
            gridView2.Columns["Item"].Visible = false;
            gridView2.Columns["pit_code"].Visible = false;
            gridView2.Columns["it_desc"].Visible = false;         

            gridView2.Columns["rmitem"].Caption = "Raw Material";
            //Commented by Prajakta B. on 27/03/2020 for Bug 32929 Start
            //gridView2.Columns[2].Caption = "Required Qty";
            //gridView2.Columns[3].Caption = "Stock Available";
            //gridView2.Columns[4].Caption = "Pending Order";
            //gridView2.Columns[5].Caption = "Reorder Level";
            //gridView2.Columns[6].Caption = "Pending Work Order";
            //gridView2.Columns[7].Caption = "Indent Qty";
            //gridView2.Columns[8].Caption = "Material Type";
            //Commented by Prajakta B. on 27/03/2020 for Bug 32929 End
            //Modified by Prajakta B. on 27/03/2020 for Bug 32929 Start
            gridView2.Columns["rmit_desc"].Caption = "Raw Material Desc";  
            gridView2.Columns["req_qty"].Caption = "Required Qty";
            gridView2.Columns["stock_avl"].Caption = "Stock Available";
            gridView2.Columns["pend_order"].Caption = "Pending Order";
            gridView2.Columns["reorder"].Caption = "Reorder Level";
            gridView2.Columns["pending_wo"].Caption = "Pending Work Order";
            gridView2.Columns["indent_qty"].Caption = "Indent Qty";
            gridView2.Columns["mtype"].Caption = "Material Type";
            //Modified by Prajakta B. on 27/03/2020 for Bug 32929 End
            gridView2.Columns["ware_nm"].Caption = "Warehouse";  // added by Suraj Kumawat for Bug-29249 
            if (clsCommon.IsWareAppl == false)
                gridView2.Columns["ware_nm"].Visible= false;
            gridView2.Columns["ItemBom"].Visible = false;  //Added by Priyanka B on 02052018 for Bug-30938

            gridView2.ExpandAllGroups();
            
            gridControl3.DataSource = RawMatTable;
            gridControl3.ForceInitialize();
            //gridView3.PopulateColumns();
            gridView3.OptionsPrint.AutoWidth = false;
            gridControl3.MainView = gridView3;
            //gridView3.Columns[0].Caption = "Goods";//Changes has been done by Suraj Kumawat for GST Date on 11-05-2017  //Commented by Prajakta B. on 23/04/2020 for Bug 33359
            gridView3.Columns[0].Caption = ItemName;//Modified by Prajakta B. on 23/04/2020 for Bug 33359 
            gridView3.Columns[1].Caption = "Raw Material";
            //Commented by Prajakta B. on 27/03/2020 For Bug 32929  Start
            gridView3.Columns[2].Caption = "Required Qty";
            //gridView3.Columns[3].Caption = "Goods-BOMID";  //Added by Priyanka B on 27042018 for Bugs 31390 & 31306  //Commented by Prajakta B. on 24/04/2020 for Bug 33359
            gridView3.Columns[3].Caption = ItemName.ToString()+"-BOMID";  //Added by Priyanka B on 27042018 for Bugs 31390 & 31306//Modified by Prajakta B. on 24/04/2020 for Bug 33359
            //Commented by Prajakta B. on 27 / 03 / 2020 For Bug 32929  End
            //Modified by Prajakta B. on 27/03/2020 For Bug 32929  Start
            //gridView3.Columns[2].Caption = "Raw Material Desc";
            //gridView3.Columns[3].Caption = "Required Qty";
            //gridView3.Columns[4].Caption = "Goods-BOMID";
            //Modified by Prajakta B. on 27/03/2020 For Bug 32929  End

            gridView3.Columns[0].FieldName = "Item";
            gridView3.Columns[1].FieldName = "rmitem";
            //Commented by Prajakta B. on 27/03/2020 For Bug 32929  Start
            gridView3.Columns[2].FieldName = "req_qty";
            gridView3.Columns[3].FieldName = "ItemBom";  //Added by Priyanka B on 27042018 for Bugs 31390 & 31306
            //Commented by Prajakta B. on 27/03/2020 For Bug 32929  End
            //Modified by Prajakta B. on 27/03/2020 For Bug 32929  Start
            //gridView3.Columns[2].FieldName = "rmit_desc";
            //gridView3.Columns[3].FieldName = "req_qty";
            //gridView3.Columns[4].FieldName = "ItemBom";
            //Modified by Prajakta B. on 27/03/2020 For Bug 32929  Start
            //gridView3.Columns[0].GroupIndex = 0;  //Commented by Priyanka B on 27042018 for Bugs 31390 & 31306
            //Added by Priyanka B on 27042018 for Bugs 31390 & 31306 Start
            gridView3.Columns[3].GroupIndex = 0; 
            gridView3.Columns[0].Visible = false;
            gridView3.Columns[3].Visible = false;
            //Added by Priyanka B on 27042018 for Bugs 31390 & 31306 End
            gridView3.ExpandAllGroups();
            //gridView2.OptionsView.ShowGroupPanel = false
           // gridView1.Columns[9].Caption = "Warehouse";
        }

        private void GridSetting()
        {
            gridControl1.DataSource = PendingData;
            gridControl1.ForceInitialize();
            gridView1.PopulateColumns();
            gridView1.OptionsPrint.AutoWidth = false;
            gridControl1.MainView = gridView1;
            gridView1.OptionsView.ShowGroupPanel = false;

            gridView1.Columns[0].Width = 40;
            gridView1.Columns[1].Caption = "Order No.";
            gridView1.Columns[1].Width = 80;
            gridView1.Columns[2].Caption = "Date";
            gridView1.Columns[2].Width = 80;

            gridView1.Columns[3].Caption = "Due Date";
            gridView1.Columns[3].Width = 80;
            // gridView1.Columns[4].Caption = "Goods"; /// Changes has been done by Suraj Kumawat for GST Date on 11-05-2017 //Commented by Prajakta B. on 23/04/2020 for Bug 33359
            gridView1.Columns[4].Caption = ItemName; ///Modified by Prajakta B. on 23/04/2020 for Bug 33359
            gridView1.Columns[4].Width = 180;
            //Commented by Prajakta B. on 27/03/2020 for Bug 32929 Start
            //gridView1.Columns[5].Caption = "Quantity";
            //gridView1.Columns[5].Width = 100;
            //gridView1.Columns[6].Caption = "Adjust Qty";
            //gridView1.Columns[6].Width = 100;
            //gridView1.Columns[6].Visible = false;
            //gridView1.Columns[7].Caption = "Warehouse";
            //gridView1.Columns[8].Visible = false;
            //gridView1.Columns[9].Visible = false;
            //gridView1.Columns[10].Visible = false;
            //gridView1.Columns[11].Visible = false;
            //gridView1.Columns["ItemBom"].Visible = false;  //Added by Priyanka B on 02052018 for Bug-30938
            //if (clsCommon.IsWareAppl == false)
            //    gridView1.Columns[7].Visible = false;
            //Commented by Prajakta B. on 27/03/2020 for Bug 32929 End
            //Modified By Prajakta B. on 27/03/2020 for Bug 32929 Start
            //gridView1.Columns[5].Caption = "Goods Desc";//Commented by Prajakta B. on 24/04/2020 for Bug 33359
            gridView1.Columns[5].Caption = ItemName.ToString().Trim()+" Desc";  //Modified by Prajakta B. on 24/04/2020 for Bug 33359+
            gridView1.Columns[5].Width = 200;
            gridView1.Columns[6].Caption = "Quantity";
            gridView1.Columns[6].Width = 100;
            gridView1.Columns[7].Caption = "Adjust Qty";
            gridView1.Columns[7].Width = 100;
            gridView1.Columns[7].Visible = false;
            gridView1.Columns[8].Caption = "Warehouse";
            gridView1.Columns[9].Visible = false;
            gridView1.Columns[10].Visible = false;
            gridView1.Columns[11].Visible = false;
            gridView1.Columns[12].Visible = false;
            gridView1.Columns["ItemBom"].Visible = false;  //Added by Priyanka B on 02052018 for Bug-30938
            if (clsCommon.IsWareAppl == false)
                gridView1.Columns[8].Visible = false;
            //Modified By Prajakta B. on 27/03/2020 for Bug 32929 End
            //**** Added by Sachin N. S. on 22/10/2019 for Bug-32914 -- Start

            clsDataAccess._databaseName = clsCommon.DbName;
            clsDataAccess._serverName = clsCommon.ServerName;
            clsDataAccess._userID = clsCommon.User;
            clsDataAccess._password = clsCommon.Password;

            oDataAccess = new clsDataAccess();
            string[] cValidTran = clsCommon.Valid_Trans.Split(',');
            string _str1 = "";
            foreach (string _str in cValidTran)
            {
                string[] _cstr1 = _str.Split(':');
                _str1 += "'" + _cstr1[0].ToString().Trim() + "'";
            }
            string cSql = "Select * from Lother Where e_code in (" + _str1 + ") order by e_code,case when att_file=1 then 1 else 2 end, Serial, SubSerial ";
            DataTable _dtLother = oDataAccess.GetDataTable(cSql, null, 30);

            int _iCol = 15;
            foreach (DataRow _dr in _dtLother.Rows)
            {
                foreach (GridColumn _gc in gridView1.Columns)
                {
                    if (_gc.FieldName == _dr["Fld_nm"].ToString().Trim())
                    {
                        _gc.Caption = _dr["Head_Nm"].ToString();
                        _gc.Width = 100;
                        break;
                    }
                }
            }

            gridView1.HorzScrollVisibility = ScrollVisibility.Always;

            //**** Added by Sachin N. S. on 22/10/2019 for Bug-32914 -- End

        }

        private void gridView2_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                bool IsBom = Convert.ToBoolean(gridView2.GetRowCellValue(e.RowHandle, gridView2.Columns["IsSubBOM"]));
                if (IsBom)
                {
                    e.Appearance.BackColor = Color.Salmon;
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.cboExport.Text == "")
            {
                MessageBox.Show("Please select file type.", clsCommon.ApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cboExport.Focus();
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            switch (cboExport.Text.Trim())
            {
                case "EXCEL(XLS)":
                    sfd.Title = "Save as Excel";
                    sfd.DefaultExt = "xls";
                    sfd.Filter = "*.xls|*.xls";
                    break;
                case "EXCEL(XLSX)":
                    sfd.Title = "Save as Excel";
                    sfd.DefaultExt = "xlsx";
                    sfd.Filter = "*.xlsx|*.xlsx";
                    break;
                case "PDF":
                    sfd.Title = "Save as PDF";
                    sfd.DefaultExt = "pdf";
                    sfd.Filter = "*.pdf|*.pdf";
                    break;
                case "RTF":
                    sfd.Title = "Save as rtf";
                    sfd.DefaultExt = "rtf";
                    sfd.Filter = "*.rtf|*.rtf";
                    break;
                case "HTML":
                    sfd.Title = "Save as html";
                    sfd.DefaultExt = "html";
                    sfd.Filter = "*.html|*.html";
                    break;
            }

            sfd.FileName = "ExportDataGrid";

            if (sfd.ShowDialog() != DialogResult.OK)
                return;


            switch (cboExport.Text)
            {
                case "EXCEL(XLS)":

                    XlsExportOptions _Options1 = new XlsExportOptions();
                    _Options1.SheetName = sfd.FileName;
                    _Options1.ExportMode = XlsExportMode.SingleFile;
                    gridControl2.ExportToXls(sfd.FileName, _Options1);
                    break;
                case "EXCEL(XLSX)":
                    XlsxExportOptions _Options2 = new XlsxExportOptions();
                    _Options2.SheetName = sfd.FileName;
                    _Options2.ExportMode = XlsxExportMode.SingleFile;
                    gridControl2.ExportToXlsx(sfd.FileName, _Options2);
                    break;
                case "PDF":
                    PdfExportOptions _Options3 = new PdfExportOptions();
                    gridControl2.ExportToPdf(sfd.FileName);
                    break;
                case "RTF":
                    RtfExportOptions _Options4 = new RtfExportOptions();
                    _Options4.ExportMode = RtfExportMode.SingleFile;
                    gridControl2.ExportToRtf(sfd.FileName);
                    break;
                case "HTML":
                    RtfExportOptions _Options5 = new RtfExportOptions();
                    _Options5.ExportMode = RtfExportMode.SingleFilePageByPage;
                    gridControl2.ExportToHtml(sfd.FileName);
                    break;
            }
        }

        private void btnWorkOrder_Click(object sender, EventArgs e)
        {
            //***** Added by Sachin N. S. on 07/11/2019 for Bug-32925 -- Start
            if (serviceType == "Support Version")
            {
                int day = DateTime.Now.Day;
                List<int> _lstDay = new List<int>();
                _lstDay.AddRange(new int[] { 1, 2, 3, 5, 28 });

                if (_lstDay.Contains(day) == false)
                {
                    MessageBox.Show("Purchase Indent will not be generated as user can do entry only on day 1,2,3,5 or 28.", clsCommon.ApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            //***** Added by Sachin N. S. on 07/11/2019 for Bug-32925 -- End


            //   this.btnWorkOrder.Enabled = false;
            DataTable WorkOrderTable = new DataTable();
            int Reccount = 0;
            DataView view = PendingData.DefaultView;
            view.RowFilter = "Sel=True";

            //SqlConnection conn = new SqlConnection(clsCommon.ConnStr);
            //SqlCommand cmd = new SqlCommand("Select * From MrpLog Where 1=2", conn);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataTable dtMrpLog = new DataTable("MrpLog_vw");
            //da.Fill(dtMrpLog);

            //for (int i = 0; i < view.Count; i++)
            //{
            //    DataRow LogRow = dtMrpLog.NewRow();
            //    LogRow["Item"] = view[i]["Item"];
            //    LogRow["It_code"] = view[i]["It_code"];
            //    LogRow["Ware_nm"] = clsCommon.Warehouse;
            //    LogRow["Orderno"] = view[i]["Inv_no"];
            //    LogRow["Qty"] = view[i]["AdjustQty"];
            //    LogRow["Entry_ty"] = view[i]["Entry_ty"];
            //    LogRow["Tran_cd"] = view[i]["Tran_cd"];
            //    LogRow["Itserial"] = view[i]["Itserial"];
            //    LogRow["rEntry_ty"] = "";
            //    LogRow["itref_tran"] = 0;
            //    LogRow["rItserial"] = "";
            //    LogRow["rIt_code"] = 0;
            //    LogRow["rqty"] = 0;
            //    dtMrpLog.Rows.Add(LogRow);
            //}

            WorkOrderTable = this.GetWorkOrderTable();
            Reccount = WorkOrderTable.Rows.Count;
            if (Reccount <= 0)
            {
                MessageBox.Show("No records found to generate the work order.", clsCommon.ApplName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //frmMRPPlan3 frm = new frmMRPPlan3(view.ToTable(),dtMrpLog, (this.rdoMultiple.Checked ? 2 : 1), "WK", "Generate Work Order Entries:");
            frmMRPPlan3 frm = new frmMRPPlan3(WorkOrderTable, dtMrpLog, 2, "WK", "Generate Work Order Entries:");     
            frm.ShowDialog();
            if (frm.IsUpdated == true)
            {
                this.btnWorkOrder.Enabled = false;
                this.btnIndent.Enabled = false;
            }
            frm = null;
        }

        private void btnIndent_Click(object sender, EventArgs e)
        {
            //***** Added by Sachin N. S. on 07/11/2019 for Bug-32925 -- Start
            if (serviceType == "Support Version")
            {
                int day = DateTime.Now.Day;
                List<int> _lstDay = new List<int>();
                _lstDay.AddRange(new int[] { 1, 2, 3, 5, 28 });

                if (_lstDay.Contains(day) == false)
                {
                    MessageBox.Show("Purchase Indent will not be generated as user can do entry only on day 1,2,3,5 or 28.", clsCommon.ApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            //***** Added by Sachin N. S. on 07/11/2019 for Bug-32925 -- End

            //this.btnIndent.Enabled = false;
            int Reccount = 0;
            DataView view = PendingData.DefaultView;
            view.RowFilter = "Sel=True";

            //SqlConnection conn = new SqlConnection(clsCommon.ConnStr);
            //SqlCommand cmd = new SqlCommand("Select * From MrpLog Where 1=2", conn);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataTable dtMrpLog = new DataTable("MrpLog_vw");
            //da.Fill(dtMrpLog);

            //for (int i = 0; i < view.Count; i++)
            //{
            //    DataRow LogRow = dtMrpLog.NewRow();
            //    LogRow["Item"] = view[i]["Item"];
            //    LogRow["It_code"] = view[i]["It_code"];
            //    LogRow["Ware_nm"] = clsCommon.Warehouse;
            //    LogRow["Orderno"] = view[i]["Inv_no"];
            //    LogRow["Qty"] = view[i]["AdjustQty"];
            //    LogRow["Entry_ty"] = view[i]["Entry_ty"];
            //    LogRow["Tran_cd"] = view[i]["Tran_cd"];
            //    LogRow["Itserial"] = view[i]["Itserial"];
            //    LogRow["rEntry_ty"] = "";
            //    LogRow["itref_tran"] = 0;
            //    LogRow["rItserial"] = "";
            //    LogRow["rIt_code"] = 0;
            //    LogRow["rqty"] = 0;
            //    dtMrpLog.Rows.Add(LogRow);
            //}

            DataTable IndentTable = this.GetIndentTable();
            //ruchit start bug 26974
            Reccount = IndentTable.Rows.Count;

            if (Reccount <= 0)
            {
                MessageBox.Show("Stock available cannot generate the Indent Order.", clsCommon.ApplName, MessageBoxButtons.OK, MessageBoxIcon.Information);//Ruchit
                return;
            }
            //ruchit end bug 26974
            else
            {
                //frmMRPPlan3 frm = new frmMRPPlan3(view.ToTable(),null, (this.rdoMultiple.Checked?2:1), "PD","Generate Purchase Indent Entries:");
                frmMRPPlan3 frm = new frmMRPPlan3(IndentTable, dtMrpLog, 2, "PD", "Generate Purchase Indent Entries:");
                frm.ShowDialog();
                if (frm.IsUpdated == true)
                {
                    this.btnIndent.Enabled = false;
                    this.btnWorkOrder.Enabled = false;
                }
                frm = null;
                view.RowFilter = "";
            } //ruchit bug 26974
        }
        private DataTable GetWorkOrderTable()
        {
            DataTable WorkOrderTable = new DataTable();
            DataColumn colItem = new DataColumn("Item", typeof(string));
            WorkOrderTable.Columns.Add(colItem);
            //Added by Prajakta B. on 27/03/2020 for Bug 32929  Start
            DataColumn colIt_Desc = new DataColumn("it_desc", typeof(string));
            WorkOrderTable.Columns.Add(colIt_Desc);
            //Added by Prajakta B. on 27/03/2020 for Bug 32929  End
            DataColumn colItemCode = new DataColumn("It_code", typeof(int));
            WorkOrderTable.Columns.Add(colItemCode);
            DataColumn colQty = new DataColumn("Qty", typeof(decimal));
            WorkOrderTable.Columns.Add(colQty);
            DataColumn colItemLvl = new DataColumn("ItemLvl", typeof(short));
            WorkOrderTable.Columns.Add(colItemLvl);
            DataColumn colWare_nm = new DataColumn("Ware_nm", typeof(string));
            WorkOrderTable.Columns.Add(colWare_nm);

            DataView view = PendingData.DefaultView;
            view.RowFilter = "Sel=True";


            DataTable distinctItem = view.ToTable("PendData", true, new string[] { "Item","it_desc", "It_code", "Ware_nm" });
            for (int i = 0; i < distinctItem.Rows.Count; i++)
            {
                decimal ItemQty = Convert.ToDecimal(PendingData.Compute("sum(AdjustQty)", "Sel=True and It_code=" + distinctItem.Rows[i]["it_code"].ToString().Trim() + " and Ware_nm='" + distinctItem.Rows[i]["Ware_nm"].ToString().Trim() + "'"));
                DataRow dr = WorkOrderTable.NewRow();
                dr["Qty"] = ItemQty;
                dr["Item"] = distinctItem.Rows[i]["Item"].ToString().Trim();
                dr["it_desc"] = distinctItem.Rows[i]["it_desc"].ToString().Trim();
                dr["It_code"] = distinctItem.Rows[i]["It_code"];
                dr["Ware_nm"] = distinctItem.Rows[i]["Ware_nm"].ToString().Trim();
                dr["ItemLvl"] = 0;      //Base Finished Items
                WorkOrderTable.Rows.Add(dr);
            }

            for (int i = 0; i < finalTable.Rows.Count; i++)
            {
                if (Convert.ToDecimal(finalTable.Rows[i]["req_qty"]) > 0 && Convert.ToBoolean(finalTable.Rows[i]["IsSubBOM"]) == true)
                {
                    DataRow dr = WorkOrderTable.NewRow();
                    dr["Qty"] = finalTable.Rows[i]["req_qty"];
                    dr["Item"] = finalTable.Rows[i]["rmItem"];
                    dr["it_desc"] = finalTable.Rows[i]["rmit_desc"];//Added by Prajakta B. on 27/03/2020 for Bug 32929
                    dr["It_code"] = finalTable.Rows[i]["It_code"];
                    dr["Ware_nm"] = finalTable.Rows[i]["Ware_nm"].ToString().Trim();
                    dr["ItemLvl"] = (Convert.ToBoolean(finalTable.Rows[i]["IsSubBOM"]) == true ? 1 : 2);      //Base Finished Items
                    WorkOrderTable.Rows.Add(dr);
                }
            }
            //this.btnIndent.Enabled = false;
            var LogRows = dtMrpLog.Select("Itemlvl=2");
            foreach (var row in LogRows)
            {
                row.Delete();
            }
            int subbomCnt = Convert.ToInt32(WorkOrderTable.Compute("count(Item)", "ItemLvl=1"));
            if (subbomCnt > 0)
            {
                //DialogResult ans = MessageBox.Show("Do you want to generate the Work Order for items having sub BOM?", clsCommon.ApplName, MessageBoxButtons.YesNo, MessageBoxIcon.Question); // Commented by Suraj Kumawat date on 11-05-2017 
                //DialogResult ans = MessageBox.Show("Do you want to generate the Work Order for Goods having sub BOM?", clsCommon.ApplName, MessageBoxButtons.YesNo, MessageBoxIcon.Question); // Changes done by suraj Kumawat for GST Date on 11-05-2017   //Commented by Priyanka B on 27042018 for Bugs 31390 & 31306
                //DialogResult ans = MessageBox.Show("Do you want to generate the Work Order for Goods having sub BOM?", clsCommon.ApplName, MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2); // Changes done by suraj Kumawat for GST Date on 11-05-2017   //Modified by Priyanka B on 27042018 for Bugs 31390 & 31306//Commented by Prajakta B. on 24/04/2020 for Bug 33359
                DialogResult ans = MessageBox.Show("Do you want to generate the Work Order for "+ItemName.ToString().Trim()+ " having sub BOM?", clsCommon.ApplName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2); // Changes done by suraj Kumawat for GST Date on 11-05-2017   //Modified by Priyanka B on 27042018 for Bugs 31390 & 31306//Modified by Prajakta B. on 24/04/2020 for Bug 33359
                if (ans == DialogResult.No)
                {
                    var Rows = WorkOrderTable.Select("Itemlvl=1");
                    foreach (var row in Rows)
                    {
                        row.Delete();
                    }
                    LogRows = dtMrpLog.Select("Itemlvl=1");
                    foreach (var row in LogRows)
                    {
                        row.Delete();
                    }
                }
            }
            return WorkOrderTable;
        }

        private DataTable GetIndentTable()
        {
            DataTable IndentTbl = new DataTable();
            DataColumn colItem = new DataColumn("Item", typeof(string));
            IndentTbl.Columns.Add(colItem);
            //Added by Prajakta B. on 27/03/2020 for Bug 32929 Start
            DataColumn colIt_desc = new DataColumn("it_desc", typeof(string));
            IndentTbl.Columns.Add(colIt_desc);
            //Added by Prajakta B. on 27/03/2020 for Bug 32929 End
            DataColumn colItemCode = new DataColumn("It_code", typeof(int));
            IndentTbl.Columns.Add(colItemCode);
            DataColumn colQty = new DataColumn("Qty", typeof(decimal));
            IndentTbl.Columns.Add(colQty);
            DataColumn colItemLvl = new DataColumn("ItemLvl", typeof(short));
            IndentTbl.Columns.Add(colItemLvl);
            DataColumn colWare_nm = new DataColumn("Ware_nm", typeof(string));
            IndentTbl.Columns.Add(colWare_nm);

            DataView view = PendingData.DefaultView;
            view.RowFilter = "Sel=True";

            // DialogResult ans = MessageBox.Show("Do you want to Generate the Indent for Finished Items?", clsCommon.ApplName, MessageBoxButtons.YesNo, MessageBoxIcon.Question); //Commented by Suraj Kumawat for GST Date on 11-05-2017 
            //DialogResult ans = MessageBox.Show("Do you want to Generate the Indent for Finished Goods?", clsCommon.ApplName, MessageBoxButtons.YesNo, MessageBoxIcon.Question); // Changes done by Suraj Kumawat for GST Date on 11-05-2017  //Commented by Priyanka B for Bugs 31390 & 31306 //Commented by Prajakta B. on 24/04/2020 for Bug 33359
            DialogResult ans = MessageBox.Show("Do you want to Generate the Indent for Finished "+ItemName.ToString().Trim()+"?", clsCommon.ApplName, MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2); // Changes done by Suraj Kumawat for GST Date on 11-05-2017  //Modified by Priyanka B for Bugs 31390 & 31306//Modified by Prajakta B. on 24/04/2020 for Bug 33359
            if (ans == DialogResult.Yes)
            {
                DataTable distinctItem = view.ToTable("PendData", true, new string[] { "Item","it_desc", "It_code", "Ware_nm" });
                for (int i = 0; i < distinctItem.Rows.Count; i++)
                {
                    decimal ItemQty = Convert.ToDecimal(PendingData.Compute("sum(AdjustQty)", "Sel=True and It_code=" + distinctItem.Rows[i]["it_code"].ToString().Trim() + " and Ware_nm='" + distinctItem.Rows[i]["Ware_nm"].ToString().Trim() + "'"));
                    DataRow dr = IndentTbl.NewRow();
                    dr["Qty"] = ItemQty;
                    dr["Item"] = distinctItem.Rows[i]["Item"].ToString().Trim();
                    dr["it_desc"] = distinctItem.Rows[i]["it_desc"].ToString().Trim();
                    dr["It_code"] = distinctItem.Rows[i]["It_code"];
                    dr["Ware_nm"] = distinctItem.Rows[i]["Ware_nm"].ToString().Trim();
                    dr["ItemLvl"] = 0;      //Base Finished Items
                    IndentTbl.Rows.Add(dr);
                }

                var LogRows = dtMrpLog.Select("Itemlvl=1 Or Itemlvl=2");

                foreach (var row in LogRows)
                {
                    row.Delete();
                }

                //this.btnWorkOrder.Enabled = false;
                return IndentTbl;
            }
            else
            {
                var LogRows = dtMrpLog.Select("Itemlvl=0");
                foreach (var row in LogRows)
                {
                    row.Delete();
                }
            }
            for (int i = 0; i < finalTable.Rows.Count; i++)
            {
                if (Convert.ToDecimal(finalTable.Rows[i]["Indent_qty"]) > 0)
                {
                    DataRow dr = IndentTbl.NewRow();
                    dr["Qty"] = finalTable.Rows[i]["Indent_qty"];
                    dr["Item"] = finalTable.Rows[i]["rmItem"];
                    dr["it_desc"] = finalTable.Rows[i]["rmit_desc"];      
                    dr["It_code"] = finalTable.Rows[i]["It_code"];
                    dr["Ware_nm"] = finalTable.Rows[i]["Ware_nm"].ToString().Trim();
                    dr["ItemLvl"] = (Convert.ToBoolean(finalTable.Rows[i]["IsSubBOM"]) == true ? 1 : 2);      //Base Finished Items
                    IndentTbl.Rows.Add(dr);
                }
            }
            int subbomCnt = Convert.ToInt32(IndentTbl.Compute("count(Item)", "ItemLvl=1"));
            if (subbomCnt > 0)
            {
                // ans = MessageBox.Show("Do you want to generate the Indent for items having sub BOM?", clsCommon.ApplName, MessageBoxButtons.YesNo, MessageBoxIcon.Question); // Commented by Suraj Kumawat for GST Date on 11-05-2017 
                //ans = MessageBox.Show("Do you want to generate the Indent for Goods having sub BOM?", clsCommon.ApplName, MessageBoxButtons.YesNo, MessageBoxIcon.Question); // Changes done by Suraj Kumawat for GST Date on 11-05-2017   //Commented by Priyanka B for Bugs 31390 & 31306
                //ans = MessageBox.Show("Do you want to generate the Indent for Goods having sub BOM?", clsCommon.ApplName, MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2); // Changes done by Suraj Kumawat for GST Date on 11-05-2017   //Modified by Priyanka B for Bugs 31390 & 31306//Commented by Prajakta B. on 24/04/2020 for Bug 33359
                ans = MessageBox.Show("Do you want to generate the Indent for "+ItemName.ToString().Trim()+ " having sub BOM?", clsCommon.ApplName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2); // Changes done by Suraj Kumawat for GST Date on 11-05-2017   //Modified by Priyanka B for Bugs 31390 & 31306 //Modified by Prajakta B. on 24/04/2020 for Bug 33359
                if (ans == DialogResult.No)
                {
                    var Rows = IndentTbl.Select("Itemlvl=1");
                    foreach (var row in Rows)
                    {
                        row.Delete();
                    }
                    var LogRows = dtMrpLog.Select("Itemlvl=1");
                    foreach (var row in LogRows)
                    {
                        row.Delete();
                    }
                }
                else
                {
                    //this.btnWorkOrder.Enabled = false;
                    var SubBOMRows = finalTable.Select("IsSubBOM=True", "");

                    foreach (var row in SubBOMRows)
                    {
                        var RawItemRows = finalTable.Select("pit_code=" + row["It_code"].ToString());
                        foreach (var RawItemRow in RawItemRows)
                        {
                            var childRows = IndentTbl.Select("Itemlvl=2 and It_code=" + RawItemRow["It_code"].ToString());
                            foreach (var childRow in childRows)
                            {
                                childRow.Delete();
                            }

                            var LogRows = dtMrpLog.Select("Itemlvl=2 and rIt_code=" + RawItemRow["It_code"].ToString());
                            foreach (var lrow in LogRows)
                            {
                                lrow.Delete();
                            }
                        }
                    }
                }
            }

            return IndentTbl;
        }

        private void gridView3_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void frmMRPPlan2_FormClosing(object sender, FormClosingEventArgs e)
        {
            clsCommon.DeleteProcessIdRecord();
        }

        private void rdoSingle_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
