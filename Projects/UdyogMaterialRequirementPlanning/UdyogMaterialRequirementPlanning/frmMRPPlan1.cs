using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting;
using System.IO;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Printing.Core;
using DevExpress.Utils.Serializing.Helpers;
using DevExpress.XtraEditors.Repository;
using udAdditionalInfo;
using DataAccess_Net;           // Added by Sachin N. S. on 25/10/2019 for Bug-32914


namespace UdyogMaterialRequirementPlanning
{
    public partial class frmMRPPlan1 : Form
    {
        private DataTable PendingData;
        //DataAccess_Net.clsDataAccess oDataAccess;
        string BomNotDefined;
        string BommID; //ruchit
        DataAccess_Net.clsDataAccess oDataAccess;       // Added by Sachin N. S. on 25/10/2019 for Bug-32914
        string ItemName;        

        public frmMRPPlan1(DataTable pendingData)
        {
            InitializeComponent();
            PendingData = pendingData;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            this.label2.Visible = true;
            this.label2.Text = "Please wait...";
            this.Refresh();
            this.UpdateBomDetails();

            //    if (BomNotDefined.Trim().Length > 0)
            //    {
            //        DialogResult result = MessageBox.Show("BOM not attached to the item(s): " + BomNotDefined.Trim().Substring(1).Trim() +Environment.NewLine+ "Do you want to continue?", "", MessageBoxButtons.YesNo);
            //        if (result == DialogResult.No)
            //        {
            //            return;
            //        }
            //    }
            //    int Selected = 0;
            //    for (int i = 0; i < PendingData.Rows.Count; i++)
            //    {
            //        if (Convert.ToBoolean(PendingData.Rows[i]["Sel"]) == true)
            //            Selected++;
            //    }
            //    if (Selected == 0)
            //    {
            //        MessageBox.Show("Please select the entries.");
            //        return;
            //    }
            //    foreach (DataRow dr in PendingData.Rows)
            //    {
            //        if (Convert.ToBoolean(dr["Sel"]) == false)
            //        {
            //            dr.Delete();
            //        }
            //    }
            //    PendingData.AcceptChanges();
            //    frmMRPPlan2 f = new frmMRPPlan2(PendingData);
            //    f.Show();
            //    this.label2.Visible = false;
            //    this.Hide();
            //}

            //Ruchit Start 26974 Bug 26/10/2015
            if (BomNotDefined.Trim().Length > 0 && BommID.Trim().Length == 0) // no bom id items
            {
                // DialogResult result = MessageBox.Show("BOM not attached to the item(s): " + BomNotDefined.Trim().Substring(1).Trim() + Environment.NewLine + "Need BOM to Generate Indent or Workorder", "", MessageBoxButtons.OK); //Commented  by Suraj Kumawat for GST Date on 11-05-2017
                //DialogResult result = MessageBox.Show("BOM not attached to the Goods: " + BomNotDefined.Trim().Substring(1).Trim() + Environment.NewLine + "Need BOM to Generate Indent or Workorder", clsCommon.ApplName, MessageBoxButtons.OK, MessageBoxIcon.Question); //Added  by Suraj Kumawat for GST Date on 11-05-2017  //Commented by Prajakta B. on 24/04/2020 for Bug 33359
                DialogResult result = MessageBox.Show("BOM not attached to the "+ItemName.ToString()+" : " + BomNotDefined.Trim().Substring(1).Trim() + Environment.NewLine + "Need BOM to Generate Indent or Workorder", clsCommon.ApplName, MessageBoxButtons.OK, MessageBoxIcon.Question); //Added  by Suraj Kumawat for GST Date on 11-05-2017  //Modified by Prajakta B. on 24/04/2020 for Bug 33359
                if (result == DialogResult.OK)
                {
                    //Application.Exit();
                    return;
                }
            }
            else if (BomNotDefined.Trim().Length > 0 && BommID.Trim().Length != 0) //bom id + no bom id items
            {
                //DialogResult result = MessageBox.Show("BOM not attached to the item(s): " + BomNotDefined.Trim().Substring(1).Trim() + Environment.NewLine + "Do you want to continue with selected item(s)?", "", MessageBoxButtons.YesNo); // Commented by Suraj Kumawat for GST Date on 17-08-2017
                //DialogResult result = MessageBox.Show("BOM not attached to the Goods: " + BomNotDefined.Trim().Substring(1).Trim() + Environment.NewLine + "Do you want to continue with selected Goods?", clsCommon.ApplName, MessageBoxButtons.YesNo, MessageBoxIcon.Question); // Added by Suraj Kumawat for GST Date on 17-08-2017//Commented by Prajakta B. on 24/04/2020 for Bug 33359
                DialogResult result = MessageBox.Show("BOM not attached to the "+ ItemName.ToString()+": " + BomNotDefined.Trim().Substring(1).Trim() + Environment.NewLine + "Do you want to continue with selected "+ItemName.ToString()+"?", clsCommon.ApplName, MessageBoxButtons.YesNo, MessageBoxIcon.Question); // Added by Suraj Kumawat for GST Date on 17-08-2017//Modified by Prajakta B. on 24/04/2020 for Bug 33359
                if (result == DialogResult.No)
                {
                    //Application.Exit();
                    return;
                }
                else
                {
                    int Selected = 0;
                    for (int i = 0; i < PendingData.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(PendingData.Rows[i]["Sel"]) == true)
                            Selected++;
                    }
                    if (Selected == 0)
                    {
                        MessageBox.Show("Please select the entries.", clsCommon.ApplName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    foreach (DataRow dr in PendingData.Rows)
                    {
                        if (Convert.ToBoolean(dr["Sel"]) == false)
                        {
                            dr.Delete();
                        }
                    }
                    PendingData.AcceptChanges();
                    frmMRPPlan2 f = new frmMRPPlan2(PendingData);
                    f.Show();
                    this.label2.Visible = false;
                    this.Hide();
                }
            }
            else if (BomNotDefined.Trim().Length == 0 && BommID.Trim().Length != 0) // only bom id items
            {
                int Selected = 0;
                for (int i = 0; i < PendingData.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(PendingData.Rows[i]["Sel"]) == true)
                        Selected++;
                }
                if (Selected == 0)
                {
                    MessageBox.Show("Please select the entries.", clsCommon.ApplName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                foreach (DataRow dr in PendingData.Rows)
                {
                    if (Convert.ToBoolean(dr["Sel"]) == false)
                    {
                        dr.Delete();
                    }
                }
                PendingData.AcceptChanges();
                frmMRPPlan2 f = new frmMRPPlan2(PendingData);
                f.Show();
                this.label2.Visible = false;
                this.Hide();
            }
            else                                            //no entries    
            {
                int Selected = 0;
                for (int i = 0; i < PendingData.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(PendingData.Rows[i]["Sel"]) == true)
                        Selected++;
                }
                if (Selected == 0)
                {
                    MessageBox.Show("Please select the entries.", clsCommon.ApplName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        //Ruchit End 26974 Bug 26/10/2015

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clsCommon.DeleteProcessIdRecord();
            Application.Exit();
        }

        private void frmMRPPlan1_Load(object sender, EventArgs e)
        {
            //Added by Prajakta B. on 24/04/2020 for Bug 33359 Start
            string sqlstr = "Select it_heading from Vudyog..Co_mast where DbName='" + clsCommon.DbName + "' and compid=" + clsCommon.CompId.ToString(); 
            SqlConnection con = new SqlConnection(clsCommon.ConnStr);
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            if (con.State == ConnectionState.Closed)
                con.Open();

            ItemName = Convert.ToString(cmd.ExecuteScalar());
            //Added by Prajakta B. on 24/04/2020 for Bug 33359 End

            this.Text = clsCommon.ApplName;
            if (clsCommon.IconFile != null)
                this.Icon = new Icon(clsCommon.IconFile);

            this.label1.Text = "Order Entries";
            this.label2.Visible = false;
            //oDataAccess = new DataAccess_Net.clsDataAccess();     
            this.GridSetting();
        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            //if (gridView1.FocusedColumn.Name.ToUpper() == "COLSEL" || gridView1.FocusedColumn.Name.ToUpper() == "COLADJUSTQTY")
            if (gridView1.FocusedColumn.Name.ToUpper() == "COLSEL" || gridView1.FocusedColumn.Name.ToUpper() == "COLADJUSTQTY" || gridView1.FocusedColumn.Name.ToUpper() == "COLTRADEBUTTON")       //*** Changed by Sachin N. S. on 23/10/2019 for Bug-32914
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void UpdateBomDetails()
        {
            DataView view = PendingData.DefaultView;
            string SqlStr = string.Empty;
            BomNotDefined = string.Empty;
            BommID = string.Empty; //Ruchit
            view.RowFilter = "Sel = 1";
            // then get the distinct table...
            //DataTable distinctItem = view.ToTable("PendData", true,new string[]{"Item","It_code"});  //Commented by Priyanka B on 27042018 for Bugs 31390 & 31306
            DataTable distinctItem = view.ToTable("PendData", true, new string[] { "Item", "It_code", "BomId" });  //Modified by Priyanka B on 27042018 for Bugs 31390 & 31306


            for (int i = 0; i < distinctItem.Rows.Count; i++)
            {
                //Commented by Priyanka B on 27042018 for Bugs 31390 & 31306 Start
                /*
                //SqlStr = "Select Isnull(u_bomid1,'') as u_bomid1 From It_mast Where It_code=@It_code";  //Commented by Priyanka B on 27042018 for Bugs 31390 & 31306
                SqlStr = "Execute Get_MRP_PendingBOMData @It_code";  //Modified by Priyanka B on 27042018 for Bugs 31390 & 31306
                SqlConnection con = new SqlConnection(clsCommon.ConnStr);
                SqlCommand cmd = new SqlCommand(SqlStr, con);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@It_code",Convert.ToString(distinctItem.Rows[i]["It_code"]).Trim()));
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd.CommandTimeout = 60000;
                string BomId = Convert.ToString(cmd.ExecuteScalar()).Trim();
                */
                //Commented by Priyanka B on 27042018 for Bugs 31390 & 31306 End

                string BomId = distinctItem.Rows[i]["BomId"].ToString().Trim(); //Modified by Priyanka B on 27042018 for Bugs 31390 & 31306
                if (BomId == string.Empty)
                {
                    BomNotDefined += " ," + Convert.ToString(distinctItem.Rows[i]["Item"]).Trim();
                    DataRow[] rows = PendingData.Select("It_code=" + Convert.ToString(distinctItem.Rows[i]["It_code"]).Trim());
                    foreach (DataRow dr in rows)
                    {
                        dr["Sel"] = false;
                    }
                }
                //Ruchit Start 26974 Bug 26/10/2015   
                if (BomId.Trim().Length != 0)
                {
                    BommID += " ," + Convert.ToString(distinctItem.Rows[i]["Item"]).Trim();
                }
                //Ruchit End 26974 Bug 26/10/2015

                //Commented by Priyanka B on 27042018 for Bugs 31390 & 31306 Start
                /*if (con.State == ConnectionState.Open)
                    con.Close();
                    */
                //Commented by Priyanka B on 27042018 for Bugs 31390 & 31306 End
            }
            view.RowFilter = "";
        }

        private void GridSetting()
        {
            gridControl1.DataSource = PendingData;
            gridControl1.ForceInitialize();
            gridView1.PopulateColumns();
            gridView1.OptionsPrint.AutoWidth = false;
            gridControl1.MainView = gridView1;
            gridView1.OptionsView.ShowGroupPanel = false;

            gridView1.Columns[0].Width = 50;
            gridView1.Columns[0].Caption = "Select";
            gridView1.Columns[1].Caption = "Order No.";
            gridView1.Columns[1].Width = 100;
            gridView1.Columns[2].Caption = "Date";
            gridView1.Columns[2].Width = 75;

            gridView1.Columns[3].Caption = "Due Date";
            gridView1.Columns[3].Width = 75;
            // gridView1.Columns[4].Caption = "Goods"; // Added by Suraj Kumawat for GST date on 11-05-2017 --"Item";//Commented by Prajakta B. on 24/04/2020 for Bug 33359
            gridView1.Columns[4].Caption = ItemName; // Added by Suraj Kumawat for GST date on 11-05-2017 --"Item";//Modified by Prajakta B. on 24/04/2020 for Bug 33359
            gridView1.Columns[4].Width = 150;
            //Commented by Prajakta B. on 27/03/2020 for Bug 32929 Start
            //gridView1.Columns[5].Caption = "Quantity";
            //gridView1.Columns[5].Width = 60;
            //gridView1.Columns[6].Caption = "Adjust Qty";
            //gridView1.Columns[6].Width = 60;
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
            //Modified by Prajakta B. on 27/03/2020 for Bug 32929 Start
            //gridView1.Columns[5].Caption = "Goods Desc"; //Commented by Prajakta B. on 24/04/2020 for Bug 33359
            gridView1.Columns[5].Caption = ItemName+" Desc"; //Modified by Prajakta B. on 24/04/2020 for Bug 33359
            gridView1.Columns[5].Width = 180;
            gridView1.Columns[6].Caption = "Quantity";
            gridView1.Columns[6].Width = 60;
            gridView1.Columns[7].Caption = "Adjust Qty";
            gridView1.Columns[7].Width = 60;
            gridView1.Columns[7].Visible = false;
            gridView1.Columns[8].Caption = "Warehouse";
            gridView1.Columns[9].Visible = false;
            gridView1.Columns[10].Visible = false;
            gridView1.Columns[11].Visible = false;
            gridView1.Columns[12].Visible = false;
            gridView1.Columns["ItemBom"].Visible = false;  
            if (clsCommon.IsWareAppl == false)
                gridView1.Columns[8].Visible = false;
            //Modified by Prajakta B. on 27/03/2020 for Bug 32929 End

            //**** Added by Sachin N. S. on 22/10/2019 for Bug-32914 -- Start

            clsDataAccess._databaseName = clsCommon.DbName;
            clsDataAccess._serverName = clsCommon.ServerName;
            clsDataAccess._userID = clsCommon.User;
            clsDataAccess._password = clsCommon.Password;

            oDataAccess = new clsDataAccess();
            string[] cValidTran = clsCommon.Valid_Trans.Split(',');
            string _str1 = "";
            foreach(string _str in cValidTran)
            {
                string[] _cstr1 = _str.Split(':');
                _str1 += "'"+_cstr1[0].ToString().Trim()+"'";
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

            this.cboExport.Top = this.gridControl1.Top + this.gridControl1.Height + 2;
            this.btnExport.Top = this.gridControl1.Top + this.gridControl1.Height + 2;
            this.btnProceed.Top = this.gridControl1.Top + this.gridControl1.Height + 2;
            this.btnCancel.Top = this.gridControl1.Top + this.gridControl1.Height + 2;

            this.btnCancel.Left = this.gridControl1.Width - this.btnCancel.Width - 2;
            this.btnProceed.Left = this.btnCancel.Left - this.btnProceed.Width - 2;



            //GridColumn gridColumn = gridView1.Columns.AddVisible("TradeButton", string.Empty);
            //RepositoryItemButtonEdit repositoryItemButtonEdit = new RepositoryItemButtonEdit();
            //repositoryItemButtonEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            //repositoryItemButtonEdit.ButtonClick += RepositoryItemButtonEdit_ButtonClick;
            //gridControl1.RepositoryItems.Add(repositoryItemButtonEdit);
            //gridColumn.ColumnEdit = repositoryItemButtonEdit;
            //gridColumn.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            //**** Added by Sachin N. S. on 22/10/2019 for Bug-32914 -- End
        }

        //**** Added by Sachin N. S. on 22/10/2019 for Bug-32914 -- Start
        private void RepositoryItemButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            //clsDataAccess._databaseName = clsCommon.DbName;
            //clsDataAccess._serverName = clsCommon.ServerName;
            //clsDataAccess._userID = clsCommon.User;
            //clsDataAccess._password = clsCommon.Password;

            //oDataAccess = new clsDataAccess();
            //string cSql = "Select * from Lother Where e_code='SO' order by case when att_file=1 then 1 else 2 end, Serial, SubSerial ";
            //DataTable _dtLother = oDataAccess.GetDataTable(cSql, null, 30);

            //string csvString = String.Join(",", _dtLother.AsEnumerable().Select(x => (x.Field<string>("tbl_nm").ToString().Trim().ToUpper().Contains("MAIN") || x.Field<string>("tbl_nm").ToString().Trim().ToUpper().Contains("LMC") ? "A." : "B." ) +"[" + x.Field<string>("fld_nm").ToString().Trim() + "]").ToArray());

            //int savedRowIndex = gridView1.GetDataSourceRowIndex(gridView1.FocusedRowHandle);

            //int rowHandle = gridView1.GetRowHandle(savedRowIndex);

            //cSql = "Select " + csvString + " from somain a,soitem b Where a.Entry_ty='SO' and a.Tran_cd = "+ PendingData.Rows[rowHandle]["Tran_cd"].ToString() +" and b.itSerial = '"+ PendingData.Rows[rowHandle]["ItSerial"].ToString() + "' ";
            //DataSet _dsAddlInfo = oDataAccess.GetDataSet(cSql, null, 30);
            //_dsAddlInfo.Tables[0].TableName = "SODATA";


            ////udAdditionalInfo.udAdditionalInfo _udAdditionalInfo = new udAdditionalInfo.udAdditionalInfo();
            ////_udAdditionalInfo._commonDs = _dsAddlInfo;
            ////_udAdditionalInfo._EntryTy = "SO";
            ////_udAdditionalInfo._HdrDtl = "D";
            ////_udAdditionalInfo._TblName = "SODATA";
            //////_udAdditionalInfo._FilterRowId = PendingData.Rows[rowHandle]["ItemRowId"].ToString();
            ////_udAdditionalInfo.callAdditionalInfo();

            int savedRowIndex = gridView1.GetDataSourceRowIndex(gridView1.FocusedRowHandle);

            int rowHandle = gridView1.GetRowHandle(savedRowIndex);

            ueFrmAddlInfo _uefrmAddlInfo = new ueFrmAddlInfo();
            _uefrmAddlInfo.cEntTy = "SO";
            _uefrmAddlInfo.iTranCd = Convert.ToInt32(PendingData.Rows[rowHandle]["Tran_cd"]);
            _uefrmAddlInfo.cItSerial = PendingData.Rows[rowHandle]["ItSerial"].ToString();
            _uefrmAddlInfo.ShowDialog();

        }
        //**** Added by Sachin N. S. on 22/10/2019 for Bug-32914 -- End

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.cboExport.Text == "")
            {
                MessageBox.Show("Please select file type.", clsCommon.ApplName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.cboExport.Focus();
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            switch (cboExport.Text)
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
                    gridControl1.ExportToXls(sfd.FileName, _Options1);
                    break;
                case "EXCEL(XLSX)":
                    XlsxExportOptions _Options2 = new XlsxExportOptions();
                    _Options2.SheetName = sfd.FileName;
                    _Options2.ExportMode = XlsxExportMode.SingleFile;
                    gridControl1.ExportToXlsx(sfd.FileName, _Options2);
                    break;
                case "PDF":
                    PdfExportOptions _Options3 = new PdfExportOptions();
                    gridControl1.ExportToPdf(sfd.FileName);
                    break;
                case "RTF":
                    RtfExportOptions _Options4 = new RtfExportOptions();
                    _Options4.ExportMode = RtfExportMode.SingleFile;
                    gridControl1.ExportToRtf(sfd.FileName);
                    break;
                case "HTML":
                    RtfExportOptions _Options5 = new RtfExportOptions();
                    _Options5.ExportMode = RtfExportMode.SingleFilePageByPage;
                    gridControl1.ExportToHtml(sfd.FileName);
                    break;
            }
        }

        private void frmMRPPlan1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void frmMRPPlan1_FormClosing(object sender, FormClosingEventArgs e)
        {
            clsCommon.DeleteProcessIdRecord();
        }

    }
}
