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
    public partial class frmAccounts : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess vDataAccess;
        DataTable vTblMain;
        string vSqlStr;
        short vTimeOut = 15;
        string vShowTips = "0";

        public frmAccounts()
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            this.pFrmCaption = "Account Details";
        }

        private void frmAccounts_Load(object sender, EventArgs e)
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
            
            chkPayAdj.Checked = (this.pTblMain.Rows[0]["Allo_Op"].ToString() == "True" ? true : false);
            chkAcWise.Checked = (this.pTblMain.Rows[0]["Acc_Adj"].ToString() == "True" ? true : false);
            chkOnlineBalChk.Checked = (this.pTblMain.Rows[0]["Ac_BChk"].ToString() == "True" ? true : false);
            chkVenIndt.Checked = (this.pTblMain.Rows[0]["lVendMRP"].ToString() == "True" ? true : false);
            chkInclBillDt.Checked = (this.pTblMain.Rows[0]["Acc_CrDys"].ToString() == "True" ? true : false);
            chkCostCenter.Checked = (this.pTblMain.Rows[0]["CostExpand"].ToString() == "True" ? true : false);

            this.nudLvl.DataBindings.Add("Text", this.pTblMain, "Grp_lv");
            this.txtBalPtAc.DataBindings.Add("Text", this.pTblMain, "BalPTBook");
            this.txtBalStAc.DataBindings.Add("Text", this.pTblMain, "BalSTBook");
            this.txtPlPtAc.DataBindings.Add("Text", this.pTblMain, "PalPTBook");
            this.txtPlStAc.DataBindings.Add("Text", this.pTblMain, "PalSTBook");
            this.txtOSAct.DataBindings.Add("Text", this.pTblMain, "OP_AcName");
            this.txtCSPLAct.DataBindings.Add("Text", this.pTblMain, "ClP_AcName");
            this.txtCSBSAct.DataBindings.Add("Text", this.pTblMain, "ClB_AcName");
        }
        private void mthBindClear()
        {
            this.nudLvl.DataBindings.Clear();
            this.txtBalPtAc.DataBindings.Clear();
            this.txtBalStAc.DataBindings.Clear();
            this.txtPlPtAc.DataBindings.Clear();
            this.txtPlStAc.DataBindings.Clear();
            this.txtOSAct.DataBindings.Clear();
            this.txtCSPLAct.DataBindings.Clear();
            this.txtCSBSAct.DataBindings.Clear();
        }
        private void btnDone_Click(object sender, EventArgs e)
        {
            if (this.pEditMode)
            {
                //this.pTblMain.Rows[0]["Holi_Day"] = vHolidays;
                this.pTblMain.Rows[0]["Allo_Op"] = chkPayAdj.Checked;
                this.pTblMain.Rows[0]["Acc_Adj"] = chkAcWise.Checked;
                this.pTblMain.Rows[0]["Ac_BChk"] = chkOnlineBalChk.Checked;
                this.pTblMain.Rows[0]["lVendMRP"] = chkVenIndt.Checked;
                this.pTblMain.Rows[0]["Acc_CrDys"] = chkInclBillDt.Checked;
                this.pTblMain.Rows[0]["CostExpand"] = chkCostCenter.Checked;
            }
            this.Close();
        }
        public void mthControlSet()
        {
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnBalPtAc.Image = Image.FromFile(fName);
                this.btnBalStAc.Image = Image.FromFile(fName);
                this.btnPlPtAc.Image = Image.FromFile(fName);
                this.btnPlStAc.Image = Image.FromFile(fName);
                this.btnOSAct.Image = Image.FromFile(fName);
                this.btnCSPLAct.Image = Image.FromFile(fName);
                this.btnCSBSAct.Image = Image.FromFile(fName);
            }
        }
        public void mthEnableDisableFormControls()
        {
            bool vEnabled = true;
            if (this.pEditMode == false) { vEnabled = false; }
            chkPayAdj.Enabled = vEnabled;
            chkAcWise.Enabled = vEnabled;
            chkOnlineBalChk.Enabled = vEnabled;
            chkVenIndt.Enabled = vEnabled;
            chkInclBillDt.Enabled = vEnabled;
            chkCostCenter.Enabled = vEnabled;

            this.nudLvl.Enabled = vEnabled;
            this.txtBalPtAc.Enabled = vEnabled;
            this.txtBalStAc.Enabled = vEnabled;
            this.txtPlPtAc.Enabled = vEnabled;
            this.txtPlStAc.Enabled = vEnabled;
            this.txtOSAct.Enabled = vEnabled;
            this.txtCSPLAct.Enabled = vEnabled;
            this.txtCSBSAct.Enabled = vEnabled;

            this.btnBalPtAc.Enabled = vEnabled;
            this.btnBalStAc.Enabled = vEnabled;
            this.btnPlPtAc.Enabled = vEnabled;
            this.btnPlStAc.Enabled = vEnabled;
            this.btnOSAct.Enabled = vEnabled;
            this.btnCSPLAct.Enabled = vEnabled;
            this.btnCSBSAct.Enabled = vEnabled;

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

        private void btnBalPtAc_Click(object sender, EventArgs e)
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
                //this.txtBalPtAc.Text = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                //this.pTblMain.Rows[0]["BalPTBook"] = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                this.txtBalPtAc.Text = oSelectPop.pReturnArray[0].Trim();
                this.pTblMain.Rows[0]["BalPTBook"] =oSelectPop.pReturnArray[0].Trim();

            }
        }

        private void btnBalStAc_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;

            vSqlStr = "Select  Ac_Name,MailName=(case when isNull(MailName,'')='' then Ac_Name else MailName end) ,Grp=[Group]  From Ac_Mast Order by Ac_Name";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);

            DataView dvw = tblPop.DefaultView;
            //dvw.Sort = "ac_name";

            VForText = "Select Ledger Name";
            vSearchCol = "Ac_Name";
            //vSearchCol = "Ac_Name,MailName";
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
                //this.txtBalStAc.Text = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                //this.pTblMain.Rows[0]["BalSTBook"] = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;

                this.txtBalStAc.Text = oSelectPop.pReturnArray[0].Trim();
                this.pTblMain.Rows[0]["BalSTBook"] = oSelectPop.pReturnArray[0].Trim();
            }
        }

        private void btnPlPtAc_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;

            vSqlStr = "Select  Ac_Name,MailName=(case when isNull(MailName,'')='' then Ac_Name else MailName end) ,Grp=[Group]  From Ac_Mast Order by Ac_Name";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);

            DataView dvw = tblPop.DefaultView;
            //dvw.Sort = "ac_name";

            VForText = "Select Ledger Name";
            vSearchCol = "Ac_Name";
            //vSearchCol = "Ac_Name,MailName";
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
                //this.txtPlPtAc.Text = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                //this.pTblMain.Rows[0]["PaLPTBOOK"] = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                this.txtPlPtAc.Text = oSelectPop.pReturnArray[0].Trim();
                this.pTblMain.Rows[0]["PaLPTBOOK"] = oSelectPop.pReturnArray[0].Trim();
            }
        }

        private void btnPlStAc_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;

            vSqlStr = "Select  Ac_Name,MailName=(case when isNull(MailName,'')='' then Ac_Name else MailName end) ,Grp=[Group]  From Ac_Mast Order by Ac_Name";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);

            DataView dvw = tblPop.DefaultView;
            //dvw.Sort = "ac_name";

            VForText = "Select Ledger Name";
            vSearchCol = "Ac_Name";
            //vSearchCol = "Ac_Name,MailName";
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
                //this.txtPlStAc.Text = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                //this.pTblMain.Rows[0]["PaLSTBOOK"] = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                this.txtPlStAc.Text = oSelectPop.pReturnArray[0].Trim();
                this.pTblMain.Rows[0]["PaLSTBOOK"] = oSelectPop.pReturnArray[0].Trim();
            }

        }

        private void chkPayAdj_CheckedChanged(object sender, EventArgs e)   //Divyang
        {
            if (chkPayAdj.Checked)
            {
                chkAcWise.Enabled = true;
            }
            else
            {
                chkAcWise.Enabled = false;
                chkAcWise.Checked = false;
            }
        }

        private void btnOSAct_Click(object sender, EventArgs e)
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
                //this.txtBalPtAc.Text = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                //this.pTblMain.Rows[0]["BalPTBook"] = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                this.txtOSAct.Text = oSelectPop.pReturnArray[0].Trim();
                this.pTblMain.Rows[0]["OP_AcName"] = oSelectPop.pReturnArray[0].Trim();

            }
        }

        private void btnCSPLAct_Click(object sender, EventArgs e)
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
                //this.txtBalPtAc.Text = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                //this.pTblMain.Rows[0]["BalPTBook"] = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                this.txtCSPLAct.Text = oSelectPop.pReturnArray[0].Trim();
                this.pTblMain.Rows[0]["ClP_AcName"] = oSelectPop.pReturnArray[0].Trim();

            }
        }

        private void btnCSBSAct_Click(object sender, EventArgs e)
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
                //this.txtBalPtAc.Text = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                //this.pTblMain.Rows[0]["BalPTBook"] = (char)34 + oSelectPop.pReturnArray[0].Trim() + (char)34;
                this.txtCSBSAct.Text = oSelectPop.pReturnArray[0].Trim();
                this.pTblMain.Rows[0]["ClB_AcName"] = oSelectPop.pReturnArray[0].Trim();

            }
        }
    }
}
