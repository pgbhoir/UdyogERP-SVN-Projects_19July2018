using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udCompanySetting
{
    public partial class frmBarCode : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess vDataAccess;
        DataTable vTblMain,vTblCo_Set;
        string vSqlStr;
        short vTimeOut = 15;
        public frmBarCode()
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            this.pFrmCaption = "Barcode Details";
        }

        private void frmBarCode_Load(object sender, EventArgs e)
        {
            this.mthControlSet();
            this.mthView();
            this.mthEnableDisableFormControls();
        }
        private void mthView()
        {

            this.mthBindClear();
            this.mthBindData();
        }
        private void mthBindData()
        {
            if (this.pTblCo_Set == null) { return; }
            if (this.pTblCo_Set.Rows.Count == 0) { return; }
            DataTable tblBarType = new DataTable();
            vSqlStr = "Select BarCodeId,BarCodeNm From BarCodeTypeMast Where BarCodeId='" + this.pTblCo_Set.Rows[0]["BarCodeId"].ToString().Trim() + "'";
            tblBarType = oDataAccess.GetDataTable(vSqlStr, null, vTimeOut);
            if (tblBarType.Rows.Count > 0)  //Divyang
            {
                this.txtType.Text = tblBarType.Rows[0]["BarCodeNm"].ToString().Trim();
                chkText.Checked = (bool)this.pTblCo_Set.Rows[0]["BCTextReqd"];
                this.nudColNo.DataBindings.Add("Text", this.pTblCo_Set, "BC_Col");
                this.nudQrSize.DataBindings.Add("Text", this.pTblCo_Set, "BC_Size");
            }
        }
        private void mthBindClear()
        {

        }
        public void mthControlSet()
        {
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnType.Image = Image.FromFile(fName);
            }
            if (this.pTblCo_Set.Rows.Count == 0)
            {
                DataRow dr = this.pTblCo_Set.NewRow();
                this.pTblCo_Set.Rows.Add(dr);
                this.pTblCo_Set.Rows[0]["BarCodeId"] = "";
                this.pTblCo_Set.Rows[0]["BCFontNm"] = "";
                this.pTblCo_Set.Rows[0]["BCTextReqd"] = false;
                this.pTblCo_Set.Rows[0]["BC_Col"] = 0;
                this.pTblCo_Set.Rows[0]["BC_Size"] = 0;
                this.pTblCo_Set.Rows[0]["BCVer"] = 0;
            }
        }
        private void btnType_Click(object sender, EventArgs e)
        {
            DataTable tblBarCodeMast = new DataTable();
            vSqlStr = "select top 1 Entry_ty,Head_nm from BarCodeMast";
            tblBarCodeMast = oDataAccess.GetDataTable(vSqlStr, null, vTimeOut);
            if (tblBarCodeMast.Rows.Count > 0)
            {
                MessageBox.Show("Cannot change the Barcode Type as Barcode have already been created.");
                return;
            }


            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            vSqlStr = "select BarCodeId,BarCodeNm from barcodetypemast order by BarcodeNm";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);

            DataView dvw = tblPop.DefaultView;
            VForText = "Select Bar Code Name";
            vSearchCol = "BarCodeNm";
            vDisplayColumnList = "BarCodeNm:Bar Code Name";
            vReturnCol = "BarCodeNm,BarCodeId";
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
                this.txtType.Text = oSelectPop.pReturnArray[0].Trim();
                // this.pTblMain.Rows[0]["fcRateDiff"] = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;

            }
            this.mthEnableDisableFormControls();
        }
        
        private void btnDone_Click(object sender, EventArgs e)
        {
            if (this.txtType.Text == "") {this.Close(); }
            if (this.pEditMode)
            {
                DataTable tblBarType = new DataTable();
                vSqlStr = "Select * From barcodetypemast Where   BarCodeNm='" + this.txtType.Text.Trim() + "'";
                tblBarType = oDataAccess.GetDataTable(vSqlStr, null, vTimeOut);
                if (tblBarType.Rows.Count > 0)
                {
                    this.pTblCo_Set.Rows[0]["BarCodeId"] = tblBarType.Rows[0]["BarCodeId"];
                    this.pTblCo_Set.Rows[0]["BCFontNm"] = tblBarType.Rows[0]["BCFontNm_HR"];
                }
                if (this.txtType.Text == "Type 1 (Code 128)")
                {
                    this.pTblCo_Set.Rows[0]["BCTextReqd"] = this.chkText.Checked;
                }
                this.pTblCo_Set.AcceptChanges();
            }
            this.Close();
        }
        public void mthEnableDisableFormControls()
        {
            bool vEnabled = true;
            if (this.pEditMode == false) { vEnabled = false; }
            this.txtType.ReadOnly = true;
            this.btnType.Enabled = vEnabled;
            chkText.Enabled=vEnabled;
            this.nudColNo.Enabled = vEnabled;
            this.nudQrSize.Enabled = vEnabled;

            if (this.txtType.Text == "Type 1 (Code 128)")
            {
                lblQrSize.Visible = false;
                nudQrSize.Visible = false;
                this.chkText.Visible = true;
                lblColNo.Visible = false;
                nudColNo.Visible = false;
                this.chkText.Left = 11;
                this.chkText.Top = 45;

            }
            if (this.txtType.Text == "PDF 417")
            {
                lblQrSize.Visible = false;
                nudQrSize.Visible = false;
                this.chkText.Visible = false;
                lblColNo.Visible = true;
                nudColNo.Visible = true;
                this.lblColNo.Left = 11;
                this.lblColNo.Top = 45;
                this.nudColNo.Left = 110;
                this.nudColNo.Top = 45;
            }
            if (this.txtType.Text == "QR Code")
            {
                lblQrSize.Visible = true;
                nudQrSize.Visible = true;
                this.chkText.Visible = false;
                lblColNo.Visible = false;
                nudColNo.Visible = false;
                this.lblQrSize.Left = 11;
                this.lblQrSize.Top = 45;
                this.nudQrSize.Left = 110;
                this.nudQrSize.Top = 45;
            }

        }
        public DataAccess_Net.clsDataAccess oDataAccess
        {
            get { return vDataAccess; }
            set { vDataAccess = value; }
        }
        public DataTable pTblMain
        {
            get { return vTblMain; }
            set { vTblMain = value; }
        }
        public DataTable pTblCo_Set
        {
            get { return vTblCo_Set; }
            set { vTblCo_Set = value; }
        }
        
       
    }
}
