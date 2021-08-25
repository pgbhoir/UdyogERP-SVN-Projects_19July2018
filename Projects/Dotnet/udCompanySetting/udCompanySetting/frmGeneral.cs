using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GetInfo;
using System.IO;

namespace udCompanySetting
{
    public partial class frmGeneral :  uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess vDataAccess;
        DataTable vTblMain;
        string vSqlStr;
        short vTimeOut = 15;
        string vHolidays="";
        //string vShowTips = "0";
        string vShowTips;
        public frmGeneral()
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            this.pFrmCaption = "General Details";
        }
        private void frmGeneral_Load(object sender, EventArgs e)
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
            vHolidays = this.pTblMain.Rows[0]["Holi_Day"].ToString();
            chkSun.Checked = (vHolidays.ToUpper().IndexOf("SUNDAY") > -1 ? true : false);
            chkMon.Checked = (vHolidays.ToUpper().IndexOf("MONDAY") > -1 ? true : false);
            chkTue.Checked = (vHolidays.ToUpper().IndexOf("TUESDAY") > -1 ? true : false);
            chkWed.Checked = (vHolidays.ToUpper().IndexOf("WEDNESDAY") > -1 ? true : false);
            chkThu.Checked = (vHolidays.ToUpper().IndexOf("THURSDAY") > -1 ? true : false);
            chkFri.Checked = (vHolidays.ToUpper().IndexOf("FRIDAY") > -1 ? true : false);
            chkSat.Checked = (vHolidays.ToUpper().IndexOf("SATURDAY") > -1 ? true : false);
            this.txtServ.DataBindings.Add("Text", this.pTblMain, "Dir_Nm");
            this.nudInvLen.DataBindings.Add("Text", this.pTblMain, "Inv_Length");
            this.txtItHeading.DataBindings.Add("Text", this.pTblMain, "It_Heading");    //Divyang
            chkApproset.Checked = (this.pTblMain.Rows[0]["approset"].ToString() == "True" ? true : false);  //Divyang
            //this.cmbDsb.DataBindings.Add("Text", this.pTblMain, "DISP_DSB");  //Divyang
            cmbDsb.Text = (pTblMain.Rows[0]["DISP_DSB"].ToString().Trim() == "N" ? "None" :  "Standard" );
            chkSTLine.Checked = (this.pTblMain.Rows[0]["sgro_op"].ToString() == "True" ? true : false);  //Divyang
            ChkPTLine.Checked = (this.pTblMain.Rows[0]["pgro_op"].ToString() == "True" ? true : false);  //Divyang
            chkSTRate.Checked = (this.pTblMain.Rows[0]["srate_op"].ToString() == "True" ? true : false);  //Divyang
            chkPTRate.Checked = (this.pTblMain.Rows[0]["prate_op"].ToString() == "True" ? true : false);  //Divyang
            chkSTSupPost.Checked = (this.pTblMain.Rows[0]["s_item"].ToString() == "True" ? true : false);  //Divyang
            chkPTSupPost.Checked = (this.pTblMain.Rows[0]["p_item"].ToString() == "True" ? true : false);  //Divyang
            vSqlStr = "Select Loc_Desc,Loc_Code from " + pTblMain.Rows[0]["DbName"].ToString().Trim() + "..Loc_Master Where Loc_Code='" + pTblMain.Rows[0]["IE_LocCode"] + "'";
            DataTable tblLoc = new DataTable();
            tblLoc = oDataAccess.GetDataTable(vSqlStr, null, vTimeOut);
            if(tblLoc.Rows.Count>0) { txtLoc.Text = (tblLoc.Rows[0]["Loc_Desc"] != null ? tblLoc.Rows[0]["Loc_Desc"].ToString() : ""); }
            string vSessTime = pTblMain.Rows[0]["SessTime"].ToString();
            if (vSessTime.IndexOf("-") > 0)
            {
                nmdTime.Value =Convert.ToDecimal(vSessTime.Substring(0, vSessTime.IndexOf("-")));
                cmbTime.Text = vSessTime.Substring(vSessTime.IndexOf("-")+1);
            }
            chkTips.Checked = (this.pShowTips == "1" ? true : false);
            chkShwWEFDt.Checked = (this.pTblMain.Rows[0]["LAPLWEFDT"].ToString() == "True" ? true : false);     // Added by Sachin N. S. on 10/07/2020 for Bug-33581

        }
        private void mthBindClear()
        {
            this.nudInvLen.DataBindings.Clear();
            this.txtServ.DataBindings.Clear();
            this.txtItHeading.DataBindings.Clear(); //Divyang
        }
        private void btnDone_Click(object sender, EventArgs e)
        {
            vHolidays = ",";
            vHolidays = vHolidays + (chkSun.Checked ? "Sunday" : "");
            vHolidays = vHolidays + (chkMon.Checked ? ",Monday":"");
            vHolidays = vHolidays + (chkTue.Checked ? ",Tuesday" : "");
            vHolidays = vHolidays + (chkWed.Checked ? ",Wednesday" : "");
            vHolidays = vHolidays + (chkThu.Checked ? ",Thursday" : "");
            vHolidays = vHolidays + (chkFri.Checked ? ",Friday" : "");
            vHolidays = vHolidays + (chkSat.Checked ? ",Saturday" : "");
            vHolidays = vHolidays.Substring(1, vHolidays.Length - 1);
            this.pTblMain.Rows[0]["Holi_Day"] = vHolidays;
            this.pTblMain.Rows[0]["It_heading"] = this.txtItHeading.Text.Trim();  //Divyang
            this.pTblMain.Rows[0]["approset"] = this.chkApproset.Checked;  //Divyang
            this.pTblMain.Rows[0]["DISP_DSB"] = this.cmbDsb.Text.Substring(0,1);  //Divyang
            this.pTblMain.Rows[0]["sgro_op"] = this.chkSTLine.Checked;  //Divyang
            this.pTblMain.Rows[0]["pgro_op"] = this.ChkPTLine.Checked;  //Divyang
            this.pTblMain.Rows[0]["srate_op"] = this.chkSTRate.Checked;  //Divyang
            this.pTblMain.Rows[0]["prate_op"] = this.chkPTRate.Checked;  //Divyang
            this.pTblMain.Rows[0]["s_item"] = this.chkSTSupPost.Checked;  //Divyang
            this.pTblMain.Rows[0]["p_item"] = this.chkPTSupPost.Checked;  //Divyang
            this.pTblMain.Rows[0]["LAplWEFDt"] = this.chkShwWEFDt.Checked;  // Added by Sachin N. S. on 10/07/2020 for Bug-33581
            vSqlStr = "Select Loc_Code from " + pTblMain.Rows[0]["DbName"].ToString().Trim() + "..Loc_Master Where Loc_Desc='" + this.txtLoc.Text.Trim() + "'";
            DataTable tblLoc = new DataTable();
            tblLoc = oDataAccess.GetDataTable(vSqlStr, null, vTimeOut);
            if (tblLoc.Rows.Count > 0) { this.pTblMain.Rows[0]["IE_LocCode"] = (tblLoc.Rows[0]["Loc_Code"] != null ? tblLoc.Rows[0]["Loc_Code"].ToString() : ""); }
            if (nmdTime.Value > 0 && cmbTime.Text.Trim() == "") { MessageBox.Show("Please Select Session TimeOut");this.cmbTime.Focus(); return; }
            pTblMain.Rows[0]["SessTime"] = (nmdTime.Value > 0 ? nmdTime.Value.ToString().Trim() + "-" + cmbTime.Text : "");
            this.pShowTips= (chkTips.Checked? "1" : "0");
            this.Close();
        }
        public void mthEnableDisableFormControls()
        {
            bool vEnabled = true;
            if (this.pEditMode == false) { vEnabled = false; }
            chkSun.Enabled=vEnabled;
            chkMon.Enabled = vEnabled;
            chkTue.Enabled = vEnabled;
            chkWed.Enabled = vEnabled;
            chkThu.Enabled = vEnabled;
            chkFri.Enabled = vEnabled;
            chkSat.Enabled = vEnabled;
            this.txtServ.Enabled = vEnabled;
            this.nudInvLen.Enabled=vEnabled;
            this.txtItHeading.Enabled = vEnabled;       //Divyang
            chkApproset.Enabled = vEnabled; //Divyang
            cmbDsb.Enabled = vEnabled; //Divyang
            chkSTLine.Enabled = vEnabled;
            ChkPTLine.Enabled = vEnabled;
            chkSTRate.Enabled = vEnabled;
            chkPTRate.Enabled = vEnabled;
            chkSTSupPost.Enabled = vEnabled;
            chkPTSupPost.Enabled = vEnabled;
            this.txtLoc.ReadOnly =true ;
            nmdTime.Enabled = vEnabled;
            cmbTime.Enabled = vEnabled;
            chkTips.Enabled = vEnabled;
            chkShwWEFDt.Enabled = vEnabled;     // Added by Sachin N. S. on 10/07/2020 for Bug-33581

            this.btnLoc.Enabled = vEnabled;
            this.btnServ.Enabled = vEnabled;
        }
        public void mthControlSet()
        {
            cmbTime.Items.Add("Min");
            cmbTime.Items.Add("Hrs");
            cmbTime.Text= "Hrs";
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnServ.Image = Image.FromFile(fName);
                this.btnLoc.Image = Image.FromFile(fName);
            }
        }
       
        private void btnServ_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vFBD = new FolderBrowserDialog();

            if (vFBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtServ.Text = vFBD.SelectedPath+@"\"; 
            }

        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;

            vSqlStr = "Select Loc_Desc,Loc_Code from " + pTblMain.Rows[0]["DbName"].ToString().Trim() + "..Loc_Master Order By Loc_Desc";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);

            DataView dvw = tblPop.DefaultView;
            //dvw.Sort = "ac_name";

            VForText = "Select Location Name";
            vSearchCol = "Loc_Desc,Loc_Code";
            vDisplayColumnList = "Loc_Desc,Loc_Desc";
            vReturnCol = "Loc_Desc";
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
                this.txtLoc.Text = oSelectPop.pReturnArray[0];
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
        public string pShowTips
        {
            get { return vShowTips; }
            set { vShowTips = value; }
        }

        private void chkPTSupPost_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
