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
    public partial class frmSMS : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess vDataAccess;
        DataTable vTblMain;
        string vSqlStr;
        short vTimeOut = 15;
        public frmSMS()
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            this.pFrmCaption = "SMS Details";
        }

        private void frmSMS_Load(object sender, EventArgs e)
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

            chkSMSEnable.Checked = (bool)this.pTblMain.Rows[0]["SMS_Allowed"];
            this.txtGateway.DataBindings.Add("Text", this.pTblMain, "Gateway");
        }
        private void mthBindClear()
        {
            this.txtGateway.DataBindings.Clear();
        }
        public void mthEnableDisableFormControls()
        {
            bool vEnabled = true;
            if (this.pEditMode == false) { vEnabled = false; }
            chkSMSEnable.Enabled = vEnabled;
            this.txtGateway.ReadOnly = true;
            this.btnGateway.Enabled = vEnabled;
        }
        public void mthControlSet()
        {
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnGateway.Image = Image.FromFile(fName);
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

        private void btnGateway_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;

            vSqlStr = " SELECT SMSGTWAYNM,SMSSETID FROM SMSSETTINGMASTER ORDER BY SMSGTWAYNM ";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);

            DataView dvw = tblPop.DefaultView;
            //dvw.Sort = "ac_name";

            VForText = "Select SMS Setting";
            //vSearchCol = "Ac_Name,MailName";
            vSearchCol = "SMSGTWAYNM";
            vDisplayColumnList = "SMSGTWAYNM:SMS Gateway Name,SMSSETID:SMS Setting Id";
            vReturnCol = "SMSGTWAYNM,SMSSETID";
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
                this.txtGateway.Text = oSelectPop.pReturnArray[0].Trim();
                this.pTblMain.Rows[0]["Gateway"] =  oSelectPop.pReturnArray[0].Trim();
                this.pTblMain.Rows[0]["GatewayUserID"] = oSelectPop.pReturnArray[1].Trim();
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.pTblMain.Rows[0]["SMS_Allowed"] = chkSMSEnable.Checked;
            this.txtGateway.Focus();
            this.txtGateway.Refresh();
            this.Close();
        }
    }
}
