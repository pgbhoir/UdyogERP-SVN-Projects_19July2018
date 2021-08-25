using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udBarcodeDataTran
{
    public partial class frmMain : uBaseForm.FrmBaseForm
    {
        string SqlStr = string.Empty, tblname, behaviour;
        string[] parts;
        string vEntry_ty = "", vTran_cd = "";
        short vTimeOut = 50;
        string FldName = "";
        string prn;
        string prnString;
        string prodcode = "", prodcode1 = "";
        DataTable tblBarNm;
        DataTable tblBarcodeData;
        DataTable dtMain;
        DataTable dtItem;
        DataTable dtSerialize;
        DataSet vDsCommon;                              
        DataAccess_Net.clsDataAccess oDataAccess;
        DataTable dtControls;
        DataTable dtRecord;
        DataTable dtSharePrintName;

        string barcodeName;
        String cAppPId, cAppName;

        public frmMain(string[] args)
        {
            //InitializeComponent();

            this.pDisableCloseBtn = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "BarcodePrinting";  
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

            vEntry_ty = args[12];       //14
            vTran_cd = args[13];       //15


            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

        }

       

        private void frmMain_Load(object sender, EventArgs e)
        {


            try
            {
                DataTable dtPrinter = new DataTable();
                dtPrinter = getPrinterName();
                if (dtPrinter.Rows.Count > 0)
                {
                    DataRow[] result = dtPrinter.Select("Sharename <> ''");

                    dtSharePrintName = new DataTable();
                    dtSharePrintName = result.CopyToDataTable();

                    foreach (DataRow dr in dtSharePrintName.Rows)
                    {
                        ddlPrinterName.Items.Add(dr[0].ToString());
                    }

                    DataRow[] result1 = dtSharePrintName.Select("Default=True");
                    if (result1.Length > 0)
                    {

                        ddlPrinterName.SelectedItem = result1[0][0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
            }

            this.mInsertProcessIdRecord();
            this.mthDsCommon();

            


            

            SqlStr = "EXECUTE [USP_REP_Barcode_Data] " + "'" + vEntry_ty + "'," + "'" + vTran_cd + "'";
            tblBarcodeData = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

       
            this.tabPage1.Text = "HeaderWise";
            this.tabPage2.Text = "DetailWise";
            this.tabPage3.Text = "SerializeWise";

            this.mthGridBind_Main();
            this.mthGridBind_Item();
            this.mthGridBind_Serialize();


            AddHeaderCheckBox();
            HeaderCheckBox.MouseClick += new MouseEventHandler(HeaderCheckBox_MouseClick);
            HeaderCheckBoxI.MouseClick += new MouseEventHandler(HeaderCheckBox_MouseClick);
            HeaderCheckBoxS.MouseClick += new MouseEventHandler(HeaderCheckBox_MouseClick);

            if (tabControl1.TabPages.Count == 0)
            {
                MessageBox.Show("No records Found of Barcode", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
            }

            
        }

        private void mthGridBind_Main()
        {
            DataRow[] Result;
            Result = tblBarcodeData.Select("Header='Headerwise'");
            if (Result.Length > 0)
            {
                dtMain = Result.CopyToDataTable();
            }
            else
            { 
                tabControl1.TabPages.Remove(tabPage1);
            }
                this.dgvMain.DataSource = this.dtMain;
                this.dgvMain.ColumnHeadersHeight = 35;
                //this.dgvMain.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
                this.dgvMain.Columns.Clear();

                System.Windows.Forms.DataGridViewCheckBoxColumn colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
                colSelect.HeaderText = "Select";
                colSelect.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                colSelect.Width = 40;
                colSelect.Name = "colSelect";
                colSelect.ReadOnly = false;
                colSelect.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvMain.Columns.Add(colSelect);

                System.Windows.Forms.DataGridViewTextBoxColumn colPartyNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colPartyNm.HeaderText = "Party Name";
                colPartyNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                colPartyNm.Width = 160;
                colPartyNm.Name = "colPartyNm";
                colPartyNm.ReadOnly = true;
                colPartyNm.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvMain.Columns.Add(colPartyNm);

                System.Windows.Forms.DataGridViewTextBoxColumn colInvNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colInvNO.HeaderText = "Transaction No.";
                colInvNO.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                colInvNO.Width = 110;
                colInvNO.Name = "colInvNO";
                colInvNO.ReadOnly = true;
                colInvNO.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvMain.Columns.Add(colInvNO);

                System.Windows.Forms.DataGridViewTextBoxColumn colTranCd = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colTranCd.HeaderText = "Tran_cd";
                colTranCd.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                colTranCd.Width = 100;
                colTranCd.Name = "colTranCd";
                colTranCd.ReadOnly = true;
                colTranCd.Visible = false;
                this.dgvMain.Columns.Add(colTranCd);

                System.Windows.Forms.DataGridViewTextBoxColumn colAcID = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colAcID.HeaderText = "AC_ID";
                colAcID.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                colAcID.Width = 100;
                colAcID.Name = "colAcID";
                colAcID.ReadOnly = true;
                colAcID.Visible = false;
                this.dgvMain.Columns.Add(colAcID);

                dgvMain.Columns["colSelect"].DataPropertyName = "sel";
                dgvMain.Columns["colPartyNm"].DataPropertyName = "party_nm";
                dgvMain.Columns["colInvNO"].DataPropertyName = "inv_no";
                dgvMain.Columns["colTranCd"].DataPropertyName = "Tran_cd";
                dgvMain.Columns["colAcID"].DataPropertyName = "code";
            
        }

        private void mthGridBind_Item()
        {
            DataRow[] result;
            result = tblBarcodeData.Select("Header='Itemwise'");
            if (result.Length > 0)
            {
                dtItem = result.CopyToDataTable();
            }
            else
            {
                tabControl1.TabPages.Remove(tabPage2);
            }
            this.dgvItem.AutoGenerateColumns = false;
                this.dgvItem.DataSource = this.dtItem;
                this.dgvItem.Columns.Clear();

                System.Windows.Forms.DataGridViewCheckBoxColumn colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
                colSelect.HeaderText = "Select";
                colSelect.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                colSelect.Width = 40;
                colSelect.Name = "colSelect";
                colSelect.ReadOnly = false;
                colSelect.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvItem.Columns.Add(colSelect);

                System.Windows.Forms.DataGridViewTextBoxColumn colInvNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colInvNO.HeaderText = "Transaction No.";
                colInvNO.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                colInvNO.Width = 110;
                colInvNO.Name = "colInvNO";
                colInvNO.ReadOnly = true;
                colInvNO.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvItem.Columns.Add(colInvNO);

                System.Windows.Forms.DataGridViewTextBoxColumn colItemNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colItemNO.HeaderText = "Item No.";
                colItemNO.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                colItemNO.Width = 40;
                colItemNO.Name = "colItemNO";
                colItemNO.ReadOnly = true;
                colItemNO.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvItem.Columns.Add(colItemNO);

                System.Windows.Forms.DataGridViewTextBoxColumn colItemNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colItemNm.HeaderText = "Item Name";
                colItemNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                colItemNm.Width = 140;
                colItemNm.Name = "colItemNm";
                colItemNm.ReadOnly = true;
                colItemNm.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvItem.Columns.Add(colItemNm);

                System.Windows.Forms.DataGridViewTextBoxColumn colTranCd = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colTranCd.HeaderText = "Tran_cd";
                colTranCd.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                colTranCd.Width = 100;
                colTranCd.Name = "colTranCd";
                colTranCd.ReadOnly = true;
                colTranCd.Visible = false;
                this.dgvItem.Columns.Add(colTranCd);

                System.Windows.Forms.DataGridViewTextBoxColumn colITCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colITCode.HeaderText = "Item Code";
                colITCode.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                colITCode.Width = 100;
                colITCode.Name = "colITCode";
                colITCode.ReadOnly = true;
                colITCode.Visible = false;
                this.dgvItem.Columns.Add(colITCode);

                System.Windows.Forms.DataGridViewTextBoxColumn colItSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colItSerial.HeaderText = "Item Serial";
                colItSerial.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                colItSerial.Width = 100;
                colItSerial.Name = "colItSerial";
                colItSerial.ReadOnly = true;
                colItSerial.Visible = false;
                this.dgvItem.Columns.Add(colItSerial);

                dgvItem.Columns["colSelect"].DataPropertyName = "sel";
                dgvItem.Columns["colInvNO"].DataPropertyName = "inv_no";
                dgvItem.Columns["colItemNO"].DataPropertyName = "item_no";
                dgvItem.Columns["colItemNm"].DataPropertyName = "ItemName";
                dgvItem.Columns["colTranCd"].DataPropertyName = "Tran_cd";
                dgvItem.Columns["colITCode"].DataPropertyName = "code";
                dgvItem.Columns["colItSerial"].DataPropertyName = "itserial";
            
        }

       

        private void mthGridBind_Serialize()
        {
            DataRow[] Result;
            Result = tblBarcodeData.Select("Header='Serialize'");
            if (Result.Length > 0)
            {
                dtSerialize = Result.CopyToDataTable();
            }
            else
            {
                tabControl1.TabPages.Remove(tabPage3);
            }
            this.dgvSerialize.AutoGenerateColumns = false;
            this.dgvSerialize.DataSource = this.dtSerialize;
            this.dgvSerialize.Columns.Clear();

            System.Windows.Forms.DataGridViewCheckBoxColumn colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            colSelect.HeaderText = "Select";
            colSelect.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSelect.Width = 40;
            colSelect.Name = "colSelect";
            colSelect.ReadOnly = false;
            colSelect.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvSerialize.Columns.Add(colSelect);

            System.Windows.Forms.DataGridViewTextBoxColumn colInvNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colInvNO.HeaderText = "Transaction No.";
            colInvNO.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colInvNO.Width = 110;
            colInvNO.Name = "colInvNO";
            colInvNO.ReadOnly = true;
            colInvNO.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvSerialize.Columns.Add(colInvNO);

            System.Windows.Forms.DataGridViewTextBoxColumn colItemNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItemNO.HeaderText = "Item No.";
            colItemNO.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colItemNO.Width = 40;
            colItemNO.Name = "colItemNO";
            colItemNO.ReadOnly = true;
            colItemNO.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvSerialize.Columns.Add(colItemNO);

            System.Windows.Forms.DataGridViewTextBoxColumn colItemNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItemNm.HeaderText = "Item Name";
            colItemNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colItemNm.Width = 140;
            colItemNm.Name = "colItemNm";
            colItemNm.ReadOnly = true;
            colItemNm.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvSerialize.Columns.Add(colItemNm);

            System.Windows.Forms.DataGridViewTextBoxColumn colSerialNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSerialNO.HeaderText = "Serial No.";
            colSerialNO.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSerialNO.Width = 140;
            colSerialNO.Name = "colSerialNO";
            colSerialNO.ReadOnly = true;
            colSerialNO.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvSerialize.Columns.Add(colSerialNO);

            System.Windows.Forms.DataGridViewTextBoxColumn colTranCd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTranCd.HeaderText = "Tran_cd";
            colTranCd.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colTranCd.Width = 100;
            colTranCd.Name = "colTranCd";
            colTranCd.ReadOnly = true;
            colTranCd.Visible = false;
            this.dgvSerialize.Columns.Add(colTranCd);

            System.Windows.Forms.DataGridViewTextBoxColumn colSTranCd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSTranCd.HeaderText = "STran_cd";
            colSTranCd.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSTranCd.Width = 100;
            colSTranCd.Name = "colSTranCd";
            colSTranCd.ReadOnly = true;
            colSTranCd.Visible = false;
            this.dgvSerialize.Columns.Add(colSTranCd);

            System.Windows.Forms.DataGridViewTextBoxColumn colITCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colITCode.HeaderText = "Item Code";
            colITCode.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colITCode.Width = 100;
            colITCode.Name = "colITCode";
            colITCode.ReadOnly = true;
            colITCode.Visible = false;
            this.dgvSerialize.Columns.Add(colITCode);

            System.Windows.Forms.DataGridViewTextBoxColumn colItSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItSerial.HeaderText = "Item Serial";
            colItSerial.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colItSerial.Width = 100;
            colItSerial.Name = "colItSerial";
            colItSerial.ReadOnly = true;
            colItSerial.Visible = false;
            this.dgvSerialize.Columns.Add(colItSerial);

            dgvSerialize.Columns["colSelect"].DataPropertyName = "sel";
            dgvSerialize.Columns["colInvNO"].DataPropertyName = "inv_no";
            dgvSerialize.Columns["colItemNO"].DataPropertyName = "item_no";
            dgvSerialize.Columns["colItemNm"].DataPropertyName = "ItemName";
            dgvSerialize.Columns["colSerialNO"].DataPropertyName = "SERIALNO";
            dgvSerialize.Columns["colTranCd"].DataPropertyName = "Tran_cd";
            dgvSerialize.Columns["colSTranCd"].DataPropertyName = "stran_cd";
            dgvSerialize.Columns["colITCode"].DataPropertyName = "code";
            dgvSerialize.Columns["colItSerial"].DataPropertyName = "itserial";


        }

        
        private void btnHeaderNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select Barcode_Nm as 'BarcodeName',M_T=(case when M_T='M' then 'Master' else 'Transaction' end) ,name as 'Name',tblname,behaviour,ffld,prn from BARCODE_SETTING where behaviour='"+vEntry_ty+"' ";

            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            if (tDs.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Label-Barcode Setting not found for this Transaction.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select Barcode Name";
            vSearchCol = "BarcodeName";
            vDisplayColumnList = "BarcodeName:Barcode Name";
            vReturnCol = "BarcodeName,M_T,Name,tblname,behaviour,ffld,prn";
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
                //btnPrint.Enabled = true;
                txtBHeaderNm.Text = oSelectPop.pReturnArray[0];
               // txtMT.Text = oSelectPop.pReturnArray[1];
                //txtName.Text = oSelectPop.pReturnArray[2];
                tblname = oSelectPop.pReturnArray[3];
                behaviour = oSelectPop.pReturnArray[4];
                //ffld = oSelectPop.pReturnArray[5];
                prn = oSelectPop.pReturnArray[6];

            }

        }

        private void btnBDetailNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select Barcode_Nm as 'BarcodeName',M_T=(case when M_T='M' then 'Master' else 'Transaction' end) ,name as 'Name',tblname,behaviour,ffld,prn from BARCODE_SETTING where behaviour='" + vEntry_ty + "' ";

            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            if (tDs.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Label-Barcode Setting not found for this Transaction.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select Barcode Name";
            vSearchCol = "BarcodeName";
            vDisplayColumnList = "BarcodeName:Barcode Name";
            vReturnCol = "BarcodeName,M_T,Name,tblname,behaviour,ffld,prn";
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
                //btnPrint.Enabled = true;
                txtBDetailNm.Text = oSelectPop.pReturnArray[0];
                // txtMT.Text = oSelectPop.pReturnArray[1];
                //txtName.Text = oSelectPop.pReturnArray[2];
                tblname = oSelectPop.pReturnArray[3];
                behaviour = oSelectPop.pReturnArray[4];
                //ffld = oSelectPop.pReturnArray[5];
                prn = oSelectPop.pReturnArray[6];

            }
        }

        private void btnBSerializeNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select Barcode_Nm as 'BarcodeName',M_T=(case when M_T='M' then 'Master' else 'Transaction' end) ,name as 'Name',tblname,behaviour,ffld,prn from BARCODE_SETTING where behaviour='" + vEntry_ty + "' ";

            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);

            if (tDs.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Label-Barcode Setting not found for this Transaction.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select Barcode Name";
            vSearchCol = "BarcodeName";
            vDisplayColumnList = "BarcodeName:Barcode Name";
            vReturnCol = "BarcodeName,M_T,Name,tblname,behaviour,ffld,prn";
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
                //btnPrint.Enabled = true;
                txtBSerializeNm.Text = oSelectPop.pReturnArray[0];
                // txtMT.Text = oSelectPop.pReturnArray[1];
                //txtName.Text = oSelectPop.pReturnArray[2];
                tblname = oSelectPop.pReturnArray[3];
                behaviour = oSelectPop.pReturnArray[4];
                //ffld = oSelectPop.pReturnArray[5];
                prn = oSelectPop.pReturnArray[6];

            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.tabControl1.SelectedIndex == 0 && this.txtBHeaderNm.Text.Trim() == "")                //Validation
                {
                    MessageBox.Show("Select "+label1.Text.Trim(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (this.tabControl1.SelectedIndex == 1 && this.txtBDetailNm.Text.Trim() == "")                //Validation
                {
                    MessageBox.Show("Select " + label2.Text.Trim(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (this.tabControl1.SelectedIndex == 2 && this.txtBSerializeNm.Text.Trim() == "")                //Validation
                {
                    MessageBox.Show("Select " + label3.Text.Trim(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }


                if (ddlPrinterName.SelectedItem.ToString() != "")
                {
                    try
                    {


                        string s1 = "";
                        string s = "";
                        int i = 0;
                        string field = "";
                        DataTable dd = dtControls;

                        //if (dd.Rows.Count > 0)
                        //{


                        //var distinctRows = (from DataRow dRow in dd.Rows
                        //                    select dRow["tblname"]).Distinct();

                        //var distinctRows1 = (from DataRow dRow in dd.Rows
                        //                     select dRow["behaviour"]).Distinct();

                        string[] tablename = new string[3];
                        if (behaviour != "" && behaviour != "IM")
                        {
                            tablename[0] = behaviour + "MAIN";
                            tablename[1] = behaviour + "ITEM";
                            if (this.tabControl1.SelectedIndex==2 && this.txtBSerializeNm.Text.Trim() != "")
                            {
                                tablename[2] = "IT_SRSTK";
                            }
                            else
                            {
                                tablename[2] = "";
                            }
                        }
                        else
                        {
                            tablename[0] = "IT_MAST";

                        }
                        string[] distinctRows1 = new string[1];
                        distinctRows1[0] = behaviour;

                        ////
                        foreach (string str in tablename)
                        {
                            string sQuery1;

                            sQuery1 = "SELECT COLUMN_NAME=('" + str + ".['+COLUMN_NAME+']'),alise=('" + str + "_'+COLUMN_NAME)  FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + str + "'";
                            DataTable dtFld1 = new DataTable();
                            dtFld1 = oDataAccess.GetDataTable(sQuery1, null, 20);
                            foreach (DataRow dr in dtFld1.Rows)
                            {
                                field = field + "," + dr[0].ToString() + " as " + dr[1].ToString();
                            }
                        }

                        field = field.Substring(1);
                        ////


                        string sQuery = " set dateformat dmy select " + field + " from ";
                        int i1 = 0;
                        foreach (string str in tablename)
                        {
                            if (str != null)
                            {
                                if (i1 == 0)
                                {
                                    sQuery = sQuery + str;
                                }
                                else if (i1==1)
                                {
                                    sQuery = sQuery + " inner join " + str;
                                }

                                i1++;
                            }

                        }

                        foreach (string str in distinctRows1)
                        {
                            if (str != "" && str != "IM" && i1 == 3)
                            {
                                sQuery = sQuery + " on " + behaviour + "MAIN.entry_ty=" + behaviour + "ITEM.entry_ty and " + behaviour + "MAIN.tran_cd=" + behaviour + "ITEM.Tran_cd ";
                            }
                           
                            i1++;

                        }

                        if (i1 == 4 && txtBSerializeNm.Text.Trim() != "")
                        {
                            //inner join It_SrStk S on s.InEntry_ty = OPITEM.entry_ty and s.InTran_cd = OPITEM.Tran_cd and s.InItserial = OPITEM.itserial
                            sQuery = sQuery + " left join IT_SRSTK on IT_SRSTK.InEntry_ty=" + behaviour + "ITEM.entry_ty and IT_SRSTK.InTran_cd=" + behaviour + "ITEM.Tran_cd and IT_SRSTK.InItserial=" + behaviour + "ITEM.itserial";
                        }

                        //Commented by Rupesh G. on 25/03/2019 for Bug no 32392       //Start
                        //if (s.Length > 0)
                        //{
                        //Commented by Rupesh G. on 25/03/2019 for Bug no 32392       //END

                        //Added by Shrikant S. on 29/12/2018 for Installer 2.1.0        //Start
                        //string barquery = "select Barcode_Nm,BC_HD,b.Entry_ty,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm))";
                        //    barquery = barquery + "Inner join BARCODEMAST c on(c.Entry_ty = b.Entry_ty)";

                        //Added by Rupesh G. on 25/03/2019 for Bug no 32392        //Start
                        string  M_T, barquery = "";
                        //bname = txtBarcode.Text.ToString().Trim();    //Divyang
                        //string[] bname = new string[3];
                        //string[] vBC_HD = new string[3];

                        //bname[0] = this.txtBHeaderNm.Text.Trim();
                        //vBC_HD[0] = "H";
                        //bname[1] = this.txtBDetailNm.Text.Trim();
                        //vBC_HD[1] = "D";
                        //bname[2] = this.txtBSerializeNm.Text.Trim();
                        //vBC_HD[2] = "S";

                        string BarNm = "";

                        if (this.tabControl1.SelectedIndex == 0)
                        {
                            BarNm = this.txtBHeaderNm.Text.Trim();

                            barquery = "select Barcode_Nm,BC_HD,b.Entry_ty,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm))";
                            barquery = barquery + "Inner join BARCODEMAST c on(c.Entry_ty = b.Entry_ty)";
                            barquery = barquery + "where Barcode_Nm = '" + BarNm + "' and BC_HD ='H' ";
                        }
                        else if (this.tabControl1.SelectedIndex == 1)
                        {
                            BarNm = this.txtBDetailNm.Text.Trim();

                            barquery = "select Barcode_Nm,BC_HD,b.Entry_ty,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm))";
                            barquery = barquery + "Inner join BARCODEMAST c on(c.Entry_ty = b.Entry_ty)";
                            barquery = barquery + "where Barcode_Nm = '" + BarNm + "' and BC_HD ='D' ";
                        }
                        else if (this.tabControl1.SelectedIndex == 2)
                        {
                            BarNm = this.txtBSerializeNm.Text.Trim();

                            barquery = "select Barcode_Nm,BC_HD,b.Entry_ty,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm))";
                            barquery = barquery + "Inner join BARCODEMAST c on(c.Entry_ty = b.Entry_ty)";
                            barquery = barquery + "where Barcode_Nm = '" + BarNm + "' and BC_HD ='S' ";
                        }



                        M_T = "Transaction";
                        //int ii = 0;
                        //DataTable barTbl = new DataTable();
                        //foreach (string strBarNm in bname)
                        //{

                        //    //barquery = "select Barcode_Nm,BC_HD,b.Entry_ty,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm))";
                        //    //barquery = barquery + "Inner join BARCODEMAST c on(c.Entry_ty = b.Entry_ty)";
                        //    //barquery = barquery + "where Barcode_Nm = '" + strBarNm + "'  " ;
                        //        if(ii==0)
                        //        {
                        //            barquery = "select Barcode_Nm,BC_HD,b.Entry_ty,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm))";
                        //            barquery = barquery + "Inner join BARCODEMAST c on(c.Entry_ty = b.Entry_ty)";
                        //            barquery = barquery + "where Barcode_Nm = '" + strBarNm + "' and BC_HD ='H' ";
                        //        }
                        //        else if(ii==1)
                        //        {
                        //            barquery = barquery + "union select Barcode_Nm,BC_HD,b.Entry_ty,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm))";
                        //            barquery = barquery + "Inner join BARCODEMAST c on(c.Entry_ty = b.Entry_ty)";
                        //            barquery = barquery + "where Barcode_Nm = '" + strBarNm + "' and BC_HD ='D' ";
                        //        }
                        //        else if (ii == 2)
                        //        {
                        //            barquery = barquery + "union select Barcode_Nm,BC_HD,b.Entry_ty,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm))";
                        //            barquery = barquery + "Inner join BARCODEMAST c on(c.Entry_ty = b.Entry_ty)";
                        //            barquery = barquery + "where Barcode_Nm = '" + strBarNm + "' and BC_HD ='S' ";
                        //        }

                        //        ii++;

                        
                        //     //barTbl = oDataAccess.GetDataTable(barquery, null, 20);


                        //}

                        //barTbl = oDataAccess.GetDataTable(barquery, null, 20);


                        //if (M_T == "Master")
                        //{
                        //    barquery = "select Barcode_Nm, BC_HD, b.Code,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a Inner Join MASTCODE b on(rtrim(a.[name]) = rtrim(b.Name))";
                        //    barquery = barquery + " Inner join BARCODEMAST c on(c.Entry_ty = b.Code)";
                        //    barquery = barquery + "where Barcode_Nm = '" + bname + "'";

                        //}
                        //else if (M_T == "Transaction")
                        //{
                        //    if (!prodcode.ToLower().Contains("efabric"))         //Divyang
                        //    {
                        //        //Commented by Priyanka B on 05032020 for Bug-33346 Start
                        //        barquery = "select Barcode_Nm,BC_HD,b.Entry_ty,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm))";
                        //        barquery = barquery + "Inner join BARCODEMAST c on(c.Entry_ty = b.Entry_ty)";
                        //        barquery = barquery + "where Barcode_Nm = '" + bname + "'";
                        //        //Commented by Priyanka B on 05032020 for Bug-33346 End
                        //    }
                        //    else //Divyang
                        //    {
                        //        //Modified by Priyanka B on 05032020 for Bug-33346 Start
                        //        barquery = "select top 1 * from (select Barcode_Nm,BC_HD,b.Entry_ty,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a ";
                        //        barquery = barquery + " Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm)) ";
                        //        barquery = barquery + " Inner join BARCODEMAST c on(c.Entry_ty = b.Entry_ty) ";
                        //        barquery = barquery + " where Barcode_Nm = '" + bname + "'";
                        //        barquery = barquery + " union ";
                        //        barquery = barquery + " select Barcode_Nm, BC_HD = 'D', b.Entry_ty,Tbl_Nm1 = (case when b.bcode_nm = '' then b.entry_ty else b.bcode_nm end +'ITEM'),d.Fld_Nm1,d.Fld_Nm2 from BARCODE_SETTING a ";
                        //        barquery = barquery + " Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm)) ";
                        //        barquery = barquery + " Inner join UOMBCMAST d on(d.Entry_ty = b.Entry_ty) ";
                        //        barquery = barquery + " where Barcode_Nm = '" + bname + "'";
                        //        barquery = barquery + ") aa";
                        //        //Modified by Priyanka B on 05032020 for Bug-33346 End
                        //    }                                   //Divyang
                        //}
                        ////Added by Rupesh G. on 25/03/2019 for Bug no 32392       //End

                        sQuery = sQuery.ToUpper();
                        DataTable barTbl = oDataAccess.GetDataTable(barquery, null, 20);

                        

                        if (barTbl.Rows.Count > 0)
                        {
                            //sQuery = sQuery + " inner join BarcodeTran Bar on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd and Bar.Itserial= " + behaviour + "Item.Itserial  )";

                            foreach (DataRow dr in barTbl.Rows)
                            {
                                string vBC_HD = dr["BC_HD"].ToString().Trim();
                               // sQuery = sQuery + " inner join BarcodeTran Bar on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd and Bar.Itserial= " + behaviour + "Item.Itserial  )";
                                //switch (barTbl.Rows[0]["BC_HD"].ToString().Trim())
                                switch (vBC_HD)
                                {
                                   

                                    case "H":
                                        //Commented by Rupesh G. on 25/03/2019 for Bug no 32392       //Start
                                        //sQuery = sQuery.Replace(barTbl.Rows[0]["Tbl_Nm1"].ToString().Trim().ToUpper() + ".[" + barTbl.Rows[0]["Fld_Nm1"].ToString().Trim() + "]", "Bar." + barTbl.Rows[0]["Fld_Nm2"].ToString().Trim());
                                        //sQuery = sQuery + " inner join BarcodeTran Bar on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd )";
                                        //Commented by Rupesh G. on 25/03/2019 for Bug no 32392       //END


                                        sQuery = sQuery.Replace(dr["Tbl_Nm1"].ToString().Trim().ToUpper() + ".[" + dr["Fld_Nm1"].ToString().Trim().ToUpper() + "]", "Bar." + dr["Fld_Nm2"].ToString().Trim());
                                        sQuery = sQuery + " inner join BarcodeTran Bar on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd  and bar.ItSerial='' )";
                                        sQuery = sQuery.Replace("SET DATEFORMAT DMY SELECT", "SET DATEFORMAT DMY SELECT top 1 ");

                                        //Added by Rupesh G. on 25/03/2019 for Bug no 32392        //END

                                        break;
                                    case "D":
                                        //Commented by Priyanka B on 05032020 for Bug-33346 Start
                                        //sQuery = sQuery.Replace(barTbl.Rows[0]["Tbl_Nm1"].ToString().Trim().ToUpper() + ".[" + barTbl.Rows[0]["Fld_Nm1"].ToString().Trim().ToUpper() + "]", "Bar." + barTbl.Rows[0]["Fld_Nm2"].ToString().Trim());
                                        //sQuery = sQuery + " inner join BarcodeTran Bar on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd and Bar.Itserial= " + behaviour + "Item.Itserial)";
                                        //Commented by Priyanka B on 05032020 for Bug-33346 End

                                        //Modified by Priyanka B on 05032020 for Bug-33346 Start
                                        string sq = sQuery.Replace(dr["Tbl_Nm1"].ToString().Trim().ToUpper() + ".[" + dr["Fld_Nm1"].ToString().Trim().ToUpper() + "]", "Bar." + dr["Fld_Nm2"].ToString().Trim());
                                        sQuery = sQuery.Replace(dr["Tbl_Nm1"].ToString().Trim().ToUpper() + ".[" + dr["Fld_Nm1"].ToString().Trim().ToUpper() + "]", "Bar." + dr["Fld_Nm2"].ToString().Trim());
                                        sQuery = sQuery + " inner join BarcodeTran Bar on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd and Bar.Itserial= " + behaviour + "Item.Itserial and bar.ITran_cd=0)";
                                        //if (prodcode.ToLower().Contains("efabric"))         //Divyang
                                        //{
                                        //    sQuery = sQuery + " Union all ";
                                        //    sQuery = sQuery + sq.Replace("SET DATEFORMAT DMY", " ");
                                        //    sQuery = sQuery + " inner join BarcodeDet Bar on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd and Bar.Itserial= " + behaviour + "Item.Itserial  and bar.ITran_cd=0)";
                                        //    //Modified by Priyanka B on 05032020 for Bug-33346 End
                                        //}                                       //Divyang
                                        break;
                                    case "S":

                                        sQuery = sQuery.Replace(dr["Tbl_Nm1"].ToString().Trim().ToUpper() + ".[" + dr["Fld_Nm1"].ToString().Trim().ToUpper() + "]", "Bar." + dr["Fld_Nm2"].ToString().Trim());
                                        sQuery = sQuery + " inner join BarcodeTran Bar on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd and Bar.Itserial= " + behaviour + "Item.Itserial and bar.ITran_cd=It_SrStk.iTran_cd )";
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        //Added by Shrikant S. on 29/12/2018 for Installer 2.1.0        //End


                        string vWhrCond = "";
                        string strItserial = "", strITran_cd = "";
                        int iCount = 0;
                        if (this.tabControl1.SelectedIndex == 0)
                        {
                            foreach (DataGridViewRow dgvM in dgvMain.Rows)
                            {
                                if (dgvM.Cells["colSelect"].Value.ToString() == "1")
                                {
                                    string vvTrancd = dgvM.Cells["colTranCd"].Value.ToString().Trim();
                                    vWhrCond = behaviour + "MAIN" + ".Tran_cd=" + vvTrancd;
                                }
                            }
                        }
                        else if (this.tabControl1.SelectedIndex == 1)
                        {
                            foreach (DataGridViewRow dgvD in dgvItem.Rows)
                            {
                                if (dgvD.Cells["colSelect"].Value.ToString() == "1")
                                {
                                    
                                    string vvITSERIAL = dgvD.Cells["colItSerial"].Value.ToString().Trim();
                                    if (iCount == 0)
                                    {
                                        strItserial = strItserial + "('" + vvITSERIAL + "'";
                                    }
                                    else
                                    {
                                        strItserial = strItserial + ",'" + vvITSERIAL + "'";
                                    }
                                    iCount++;
                                }
                            }
                            strItserial = strItserial + ")";
                            vWhrCond = behaviour + "Item" + ".Tran_cd=" + vTran_cd + " and " + behaviour + "Item.Itserial in " + strItserial;

                        }
                        else if (this.tabControl1.SelectedIndex == 2)
                        {
                            foreach (DataGridViewRow dgvS in dgvSerialize.Rows)
                            {
                                if (dgvS.Cells["colSelect"].Value.ToString() == "1")
                                {

                                    string vvITSERIAL = dgvS.Cells["colItSerial"].Value.ToString().Trim();
                                    if (iCount == 0)
                                    {
                                        strItserial = strItserial + "('" + vvITSERIAL + "'";
                                    }
                                    else
                                    {
                                        strItserial = strItserial + ",'" + vvITSERIAL + "'";
                                    }
                                    //iCount++;
                                    string vvITranCd = dgvS.Cells["colSTranCd"].Value.ToString().Trim();
                                    if (iCount == 0)
                                    {
                                        strITran_cd = strITran_cd + "('" + vvITranCd + "'";
                                    }
                                    else
                                    {
                                        strITran_cd = strITran_cd + ",'" + vvITranCd + "'";
                                    }
                                    iCount++;
                                }
                                    
                            }
                            strItserial = strItserial + ")";
                            strITran_cd = strITran_cd + ")";
                            vWhrCond = behaviour + "Item" + ".Tran_cd=" + vTran_cd + " and It_SrStk.InItserial in " + strItserial+ " and It_SrStk.iTran_cd in "+strITran_cd;

                        }



                            //Added by Rupesh G. on 25/03/2019 for Bug no  32392      //Start

                        if (vWhrCond.Length > 0)
                        {
                            //Added by Rupesh G. on 25/03/2019 for Bug no  32392      //END
                            sQuery = sQuery + " where " + vWhrCond;
                        }

                        dtRecord = new DataTable();
                        dtRecord = oDataAccess.GetDataTable(sQuery, null, 20);
                        //   }


                        ////////////////***********************************
                        genratePRN();
                        ///////////////************************************

                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("" + ee.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Select Printer name!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

            }
            catch (Exception ee)
            {
                MessageBox.Show("Select Printer name!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void genratePRN()
        {

             DataRow[] printerShareName = dtSharePrintName.Select("name='" + ddlPrinterName.SelectedItem.ToString().Trim() + "'");         //Divyang

            if (printerShareName.Length > 0)
            {
                string printSName = printerShareName[0][4].ToString();
            if (dtRecord.Rows.Count == 0)
            {
                MessageBox.Show("Records not found...!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                try
                {
                    string fldname, rl;
                    int sp, ep;
                    string txtPrn = "";

                    //  string[] delimiters = new string[] { "<<", ">>" };
                    //parts = ffld.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    foreach (DataRow dr in dtRecord.Rows)
                    {

                        string prn1 = prn;

                        string[] var = prn.Split('\n');

                        foreach (string str in var)
                        {
                            string[] delimiters = new string[] { "@@" };
                            parts = str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string str1 in parts)
                            {

                                if (str1.StartsWith("<<") && str1.EndsWith(">>"))
                                {
                                    string[] delimiters1 = new string[] { "<<", ">>" };
                                    parts = str1.Split(delimiters1, StringSplitOptions.RemoveEmptyEntries);

                                    fldname = parts[0].ToString();

                                    string[] delimiters12 = fldname.Split('#');

                                    if (delimiters12[0] == "H")
                                    {
                                        fldname = dr[behaviour + "Main_" + delimiters12[1].ToString()].ToString();
                                    }
                                    else if (delimiters12[0] == "L")
                                    {
                                        fldname = dr[behaviour + "Item_" + delimiters12[1].ToString()].ToString();
                                    }
                                    else if (delimiters12[0] == "S")
                                    {
                                        fldname = dr["IT_SRSTK_" + delimiters12[1].ToString()].ToString();
                                    }
                                    else if (delimiters12[0] == "I")
                                    {
                                        // MessageBox.Show(delimiters12[1].ToString());
                                        //     MessageBox.Show(dr["IT_MAST_" + delimiters12[1].ToString()].ToString());
                                        fldname = dr["IT_MAST_" + delimiters12[1].ToString()].ToString();
                                    }

                                    rl = parts[1].ToString();

                                    sp = Int32.Parse(parts[2].ToString()) - 1;

                                    ep = Int32.Parse(parts[3].ToString());


                                    fldname = fldname.Trim();
                                    if (rl == "R")
                                    {
                                        char[] array = fldname.ToCharArray();
                                        Array.Reverse(array);
                                        fldname = new string(array);

                                    }

                                    int i = fldname.Length - sp;
                                    if (i >= ep)
                                    {
                                        fldname = fldname.Substring(sp, ep);
                                    }

                                    else
                                    {
                                        fldname = fldname.Substring(sp, i);
                                    }

                                    prn1 = prn1.Replace("@@" + str1 + "@@", fldname);

                                }
                            }
                        }
                        txtPrn = txtPrn + prn1 + "\n";
                    }

                    string fname = "";
                    //string fname = getFileName(txtBarcode.Text.ToString());
                    if (this.tabControl1.SelectedIndex == 0)
                    {
                         fname = getFileName(txtBHeaderNm.Text.ToString());
                    }
                    else if (this.tabControl1.SelectedIndex == 1)
                    {
                        fname = getFileName(txtBDetailNm.Text.ToString());
                    }
                    else if (this.tabControl1.SelectedIndex == 2)
                    {
                        fname = getFileName(txtBSerializeNm.Text.ToString());
                    }
                        //string fname = getFileName("Bar");       

                    string filepath = Application.StartupPath + "\\_BAT\\_PRN";

                    using (FileStream stream = new FileStream(filepath + "\\" + fname + ".prn", FileMode.Create))
                    using (TextWriter writer1 = new StreamWriter(stream))
                    {
                        writer1.WriteLine("");
                    }

                    FileStream fs1 = new FileStream(filepath + "\\" + fname + ".prn", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(fs1);

                    writer.Write(txtPrn);
                    writer.Close();

                    ////Code genrate batch file
                    string filepathBat = Application.StartupPath;
                    string txtBat = "";

                    //txtBat = txtBat + "set MyPath=%cd%" + Environment.NewLine;
                    //txtBat = txtBat + "echo %MyPath%" + Environment.NewLine;
                    //txtBat = txtBat + "set PCName=%ComputerName%" + Environment.NewLine + Environment.NewLine;
                    //txtBat = txtBat + "FOR /F \"tokens=2* delims==\" %%A in (" + Environment.NewLine;
                    //txtBat = txtBat + "  'wmic printer where \"default=True\" get sharename /value'" + Environment.NewLine;
                    //txtBat = txtBat + "  ) do SET DefaultPrinter=%%A" + Environment.NewLine;
                    //txtBat = txtBat + "set PrinterShareName=%DefaultPrinter%" + Environment.NewLine + Environment.NewLine;
                    //txtBat = txtBat + "Copy  \"%MyPath%\\_PRN\\" + fname + ".prn\" " + "\\" + "\\" + "%PCName%\\"+ printSName + "" + Environment.NewLine;
                    //txtBat = txtBat + "PAUSE";








                    // txtBat = txtBat + "set MyPath=%cd%" + Environment.NewLine;
                    // txtBat = txtBat + "echo %MyPath%" + Environment.NewLine;

                    txtBat = txtBat + "@ECHO OFF" + Environment.NewLine;
                    txtBat = txtBat + "SET MyPath=%~dp0" + Environment.NewLine;
                    txtBat = txtBat + "SET MyPath=%MyPath:~0,-1%" + Environment.NewLine;

                    txtBat = txtBat + "set PCName=%ComputerName%" + Environment.NewLine + Environment.NewLine;
                    txtBat = txtBat + "FOR /F \"tokens=2* delims==\" %%A in (" + Environment.NewLine;
                    txtBat = txtBat + "  'wmic printer where \"default=True\" get sharename /value'" + Environment.NewLine;
                    txtBat = txtBat + "  ) do SET DefaultPrinter=%%A" + Environment.NewLine;
                    txtBat = txtBat + "set PrinterShareName=%DefaultPrinter%" + Environment.NewLine + Environment.NewLine;
                     txtBat = txtBat + "Copy  \"%MyPath%\\_PRN\\" + fname + ".prn\" " + "\\" + "\\" + "%PCName%\\" + printSName + "" + Environment.NewLine;        //Divyang


                    //txtBat = txtBat + "Copy  \"%MyPath%\\_PRN\\" + fname + ".prn\" " + "\\" + "\\" + "%PCName%\\%PrinterShareName%" + Environment.NewLine;
                    FileStream fsBat = new FileStream(filepathBat + "\\_BAT\\" + fname + ".bat", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter writerBat = new StreamWriter(fsBat);

                    writerBat.Write(txtBat);
                    writerBat.Close();


                    ////Code genrate batch file
                    int printCopy = Int32.Parse(txtUpDown.Value.ToString());    //Divyang
                    //int printCopy = Int32.Parse("1");

                    for (int i = 1; i <= printCopy; i++)
                    {
                        string filepath1 = filepathBat + @"\_BAT\" + fname + ".bat";

                        System.Diagnostics.Process.Start(filepath1);
                    }


                    //if (File.Exists(filepathBat + "\\"+ fname + ".bat"))
                    //{
                    //    File.Delete(filepathBat + "\\"+ fname + ".bat");
                    //}
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());
                }

            }
              } //Divyang


        }
        private string getFileName(string fname)
        {
            string filename;
            string date = DateTime.Now.ToString("dd/MM/yy") + "_" + DateTime.Now.ToString("hh:mm:ss tt");
            filename = fname + date;
            filename = filename.Replace(".", "").Replace("-", "").Replace("/", "").Replace(":", "").Replace("__", "_").Replace(" ", "");
            return filename;
        }

        private void dgvItem_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            udGridViewColSearch.clsGridViewColSearch vGridViewColSearch = new udGridViewColSearch.clsGridViewColSearch();
            vGridViewColSearch.mthDgvSearch(dtItem, this.Icon, dgvItem, e);
        }

        private void dgvSerialize_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            udGridViewColSearch.clsGridViewColSearch vGridViewColSearch = new udGridViewColSearch.clsGridViewColSearch();
            vGridViewColSearch.mthDgvSearch(dtSerialize, this.Icon, dgvSerialize, e);
        }

        CheckBox HeaderCheckBox = null;
        CheckBox HeaderCheckBoxI = null;
        CheckBox HeaderCheckBoxS = null;
        bool IsHeaderCheckBoxClicked = false;

        private void AddHeaderCheckBox()
        {
            HeaderCheckBox = new CheckBox();
            Point headerCellLocation = this.dgvMain.GetCellDisplayRectangle(0, -1, true).Location;
            HeaderCheckBox.Location = new Point(headerCellLocation.X + 14, headerCellLocation.Y + 17);
            HeaderCheckBox.Size = new Size(14, 14);
            HeaderCheckBox.Checked = true;

            HeaderCheckBoxI = new CheckBox();
            Point headerCellLocationI = this.dgvItem.GetCellDisplayRectangle(0, -1, true).Location;
            HeaderCheckBoxI.Location = new Point(headerCellLocation.X + 14, headerCellLocation.Y + 17);
            HeaderCheckBoxI.Size = new Size(14, 14);
            HeaderCheckBoxI.Checked = true;

            HeaderCheckBoxS = new CheckBox();
            Point headerCellLocationS = this.dgvSerialize.GetCellDisplayRectangle(0, -1, true).Location;
            HeaderCheckBoxS.Location = new Point(headerCellLocation.X + 14, headerCellLocation.Y + 17);
            HeaderCheckBoxS.Size = new Size(14, 14);
            HeaderCheckBoxS.Checked = true;


            this.dgvMain.Controls.Add(HeaderCheckBox);
            this.dgvItem.Controls.Add(HeaderCheckBoxI);
            this.dgvSerialize.Controls.Add(HeaderCheckBoxS);
        }

        private void HeaderCheckBoxClick(CheckBox HCheckBox)
        {
            if (this.tabControl1.SelectedIndex == 0)
            {
                IsHeaderCheckBoxClicked = true;

                foreach (DataGridViewRow Row in dgvMain.Rows)
                    ((DataGridViewCheckBoxCell)Row.Cells["colSelect"]).Value = HCheckBox.Checked;

                dgvMain.RefreshEdit();
                IsHeaderCheckBoxClicked = false;
            }
            else if (this.tabControl1.SelectedIndex == 1)
            {
                IsHeaderCheckBoxClicked = true;

                foreach (DataGridViewRow Row in dgvItem.Rows)
                    ((DataGridViewCheckBoxCell)Row.Cells["colSelect"]).Value = HCheckBox.Checked;

                dgvItem.RefreshEdit();
                IsHeaderCheckBoxClicked = false;
            }
            else if (this.tabControl1.SelectedIndex == 2)
            {
                IsHeaderCheckBoxClicked = true;

                foreach (DataGridViewRow Row in dgvSerialize.Rows)
                    ((DataGridViewCheckBoxCell)Row.Cells["colSelect"]).Value = HCheckBox.Checked;

                dgvSerialize.RefreshEdit();
                IsHeaderCheckBoxClicked = false;
            }


        }

        private void HeaderCheckBox_MouseClick(Object sender, MouseEventArgs e)
        {
            HeaderCheckBoxClick((CheckBox)sender);
        }

        private void dgvMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dgvM in dgvMain.Rows)
            {
                if ((bool)dgvM.Cells["colSelect"].EditedFormattedValue == false)
                {
                    HeaderCheckBox.Checked = false;   
                }
            }
        }

        private void dgvItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dgvI in dgvItem.Rows)
            {
                if ((bool)dgvI.Cells["colSelect"].EditedFormattedValue == false)
                {
                    HeaderCheckBoxI.Checked = false;
                }
            }
        }

        private void dgvSerialize_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dgvS in dgvSerialize.Rows)
            {
                if ((bool)dgvS.Cells["colSelect"].EditedFormattedValue == false)
                {
                    HeaderCheckBoxS.Checked = false;
                }
            }
        }

        private DataTable getPrinterName()
        {
            DataTable dtPrinter = new DataTable();
            dtPrinter.Columns.Add("name");
            dtPrinter.Columns.Add("Status");
            dtPrinter.Columns.Add("Default");
            dtPrinter.Columns.Add("Network");
            dtPrinter.Columns.Add("Sharename");

            var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
            foreach (var printer in printerQuery.Get())
            {
                try
                {
                    var name = printer.GetPropertyValue("Name");
                    var status = printer.GetPropertyValue("Status");
                    var isDefault = printer.GetPropertyValue("Default");
                    var isNetworkPrinter = printer.GetPropertyValue("Network");
                    var sname = printer.GetPropertyValue("Sharename");

                    dtPrinter.Rows.Add(name, status, isDefault, isNetworkPrinter, sname);


                }
                catch (Exception ee)
                {

                }
            }
            return dtPrinter;
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

       

        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udBarcodePrinting.exe";
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

        private void btnLogout_Click_3(object sender, EventArgs e)
        {
            this.Close();
            mDeleteProcessIdRecord();
        }
    }
}
