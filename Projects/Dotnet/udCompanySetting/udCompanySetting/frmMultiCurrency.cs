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
    public partial class frmMultiCurrency : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess vDataAccess;
        DataTable vTblMain;
        string vSqlStr;
        short vTimeOut = 15;
        public frmMultiCurrency()
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            this.pFrmCaption = "Multi Currency Details";
        }

        private void frmMultiCurrency_Load(object sender, EventArgs e)
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
            if (this.pTblMain == null) { return; }
            if (this.pTblMain.Rows.Count == 0) { return; }
            
            chkStTrnRnd.Checked = (bool)this.pTblMain.Rows[0]["fcsNet_op"];
            chkStGdRnd.Checked = (bool)this.pTblMain.Rows[0]["fcsINet_op"];
            chkPtTrnRnd.Checked = (bool)this.pTblMain.Rows[0]["fcPNet_op"];
            chkPtGdRnd.Checked = (bool)this.pTblMain.Rows[0]["fcPInet_op"];
            this.txtRtDiffAc.DataBindings.Add("Text", this.pTblMain, "fcRateDiff");
        }
        private void mthBindClear()
        {
            this.txtRtDiffAc.DataBindings.Clear();
        }
        private void btnDone_Click(object sender, EventArgs e)
        {
            this.pTblMain.Rows[0]["fcsNet_op"] = chkStTrnRnd.Checked;
            this.pTblMain.Rows[0]["fcsINet_op"] = chkStGdRnd.Checked;
            this.pTblMain.Rows[0]["fcPNet_op"] = chkPtTrnRnd.Checked;
            this.pTblMain.Rows[0]["fcPInet_op"] = chkPtGdRnd.Checked;
            this.txtRtDiffAc.Focus();
            this.txtRtDiffAc.Refresh();
            this.Close();
        }
        public void mthEnableDisableFormControls()
        {
            bool vEnabled = true;
            if (this.pEditMode == false) { vEnabled = false; }
            chkStTrnRnd.Enabled = vEnabled;
            chkStGdRnd.Enabled = vEnabled;
            chkPtTrnRnd.Enabled = vEnabled;
            chkPtGdRnd.Enabled = vEnabled;
            this.txtRtDiffAc.ReadOnly = true;
            this.btnRtDiffAc.Enabled = vEnabled;
        }
        public void mthControlSet()
        {   
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnRtDiffAc.Image = Image.FromFile(fName);
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

        private void btnRtDiffAc_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;

            vSqlStr = "Select  Ac_Name,MailName=(case when isNull(MailName,'')='' then Ac_Name else MailName end) ,Grp=[Group]  From Ac_Mast Order by Ac_Name";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);

            DataView dvw = tblPop.DefaultView;
            //dvw.Sort = "ac_name";

            VForText = "Select Ledger Name";
            //vSearchCol = "Ac_Name,MailName";
            vSearchCol = "Ac_Name";
            vDisplayColumnList = "Ac_Name:Ledger Name,MailName:Mail Name,Grp:Group";
            vReturnCol = "Ac_Name";
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
                if (oSelectPop.pReturnArray[0].Trim() == "")
                {
                    this.txtRtDiffAc.Text = "";
                    this.pTblMain.Rows[0]["fcRateDiff"] =  oSelectPop.pReturnArray[0].Trim();
                }
                else
                {
                    this.txtRtDiffAc.Text = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                    this.pTblMain.Rows[0]["fcRateDiff"] = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                }
                //this.pTblMain.Rows[0]["fcRateDiff"] = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
            }
        }
    }
}
