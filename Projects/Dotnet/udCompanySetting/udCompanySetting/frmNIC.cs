using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Udyog.Library.Common;
namespace udCompanySetting
{
    public partial class frmNIC : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess vDataAccess;
        DataTable vTblMain;
        string vSqlStr;
        short vTimeOut = 15;
        public frmNIC()
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            this.pFrmCaption = "NIC Credential";
        }

        private void frmNIC_Load(object sender, EventArgs e)
        {
            this.mthControlSet();
            this.mthView();
            this.mthControlSet();
            this.mthEnableDisableFormControls();
        }
        private void mthBindData()
        {
            this.txtUserId.DataBindings.Add("Text", this.pTblMain, "NICUid");
            this.txtPassword.Text = VU_UDFS.NewDECRY(VU_UDFS.HexDecimalToASCII(this.pTblMain.Rows[0]["NicPwd"].ToString().Trim()),"Ud*yog+1993NIC");
        }
        private void mthBindClear()
        {
            this.txtUserId.DataBindings.Clear();
        }
        private void mthView()
        {
            this.mthBindData();
        }
        public void mthControlSet()
        {
            this.txtUserId.Enabled = true;
        }
        public void mthEnableDisableFormControls()
        {
            bool vEnabled = true;
            if (this.pEditMode == false) { vEnabled = false;}
            this.txtUserId.Enabled = vEnabled;
            this.txtPassword.Enabled = vEnabled;
        }
        private void btnDone_Click(object sender, EventArgs e)
        {
            if (this.pEditMode == false) { this.Close(); }
            if (this.txtUserId.Text.Trim() != "" && this.txtPassword.Text.Trim() == "") { MessageBox.Show("Password cannot be blank.."); return; }    //Divyang
            if (this.txtUserId.Text.Trim() == "" && this.txtPassword.Text.Trim() != "") { MessageBox.Show("Username cannot be blank.."); return; }    //Divyang
            if (this.txtPassword.Text.Trim() != "")
            {
                this.pTblMain.Rows[0]["NicPwd"] = VU_UDFS.ASCIIToHexDecimal(VU_UDFS.NewENCRY(this.txtPassword.Text.Trim(), "Ud*yog+1993NIC")).Substring(0, 14);
            }
            Close();
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
    }
}
