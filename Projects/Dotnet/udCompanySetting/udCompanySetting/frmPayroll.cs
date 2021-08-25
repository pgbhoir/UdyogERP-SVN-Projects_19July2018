using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udCompanySetting
{
    public partial class frmPayroll : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess vDataAccess;
        DataTable vTblMain;
        string vSqlStr;
        short vTimeOut = 15;

        public frmPayroll()
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            this.pFrmCaption = "Payroll Details";
        }

        private void frmPayroll_Load(object sender, EventArgs e)
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
         
            chkEmpCode.Checked = (this.pTblMain.Rows[0]["GenManEcd"].ToString() == "True" ? true : false);
            
        }
        private void mthBindClear()
        {
          
        }
        public void mthControlSet()
        {
            
        }
        private void btnDone_Click(object sender, EventArgs e)
        {
            if (this.pEditMode == false) { this.Close(); }
            this.pTblMain.Rows[0]["GenManEcd"] = this.chkEmpCode.Checked;
            this.Close();
        }
        public void mthEnableDisableFormControls()
        {
            bool vEnabled = true;
            if (this.pEditMode == false) { vEnabled = false; }
            this.chkEmpCode.Enabled = vEnabled;
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
