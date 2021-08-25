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
    public partial class frmInventory : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess vDataAccess;
        DataTable vTblMain, vTblManufact, vTblStkValConfig;
        string vSqlStr;
        short vTimeOut = 15;
        string vShowTips = "0";
        public frmInventory()
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            this.pFrmCaption = "Inventory Details";
        }

        private void frmInventory_Load(object sender, EventArgs e)
        {
            this.mthView();
            this.mthEnableDisableFormControls();
        }
        private void mthView()
        {
            this.mthBindClear();
            this.mthBindData();
            this.mthGridBind();
        }
        private void mthBindData()
        {
            if (this.pTblMain == null) { return; }
            if (this.pTblMain.Rows.Count == 0) { return; }
            if ((bool)this.pTblManufact.Rows[0]["BatchItem"]) { this.rdbGoods.Checked = true; this.rdbRun.Checked = false; } else { this.rdbGoods.Checked = false; this.rdbRun.Checked = true; }
            chkAutoBatch.Checked = ((bool)this.pTblManufact.Rows[0]["autobatch"] ? true : false);    //Divyang

            chkonLineBal.Checked =    ((bool)this.pTblMain.Rows[0]["It_BChk"]? true : false);
            chkNegBal.Checked =       ((bool)this.pTblMain.Rows[0]["Neg_ItBal"] ? true : false);
            chkPartyPrice.Checked =   ((bool)this.pTblMain.Rows[0]["It_Rate"] ? true : false);
            chkDisplayInStk.Checked = ((bool)this.pTblMain.Rows[0]["It_Stock"] ? true : false);

            this.nudQtyDeci.DataBindings.Add("Text", this.pTblMain, "Deci");
            this.nudRateDeci.DataBindings.Add("Text", this.pTblMain, "RateDeci");
        }
        private void mthGridBind()
        {
            dgvStkVal.AutoGenerateColumns = false;
            dgvStkVal.DataSource = this.pTblStkValConfig;
            dgvStkVal.Columns["colName"].DataPropertyName = "VName";
            dgvStkVal.Columns["colCond"].DataPropertyName = "VCond";
            dgvStkVal.Columns["colValuation"].DataPropertyName = "vType";
           // dgvStkVal.Columns["colOpStkAc"].DataPropertyName = "OP_AcName";
            //dgvStkVal.Columns["colClStkPLAc"].DataPropertyName = "ClP_AcName";
            //dgvStkVal.Columns["colClStkBsAc"].DataPropertyName = "ClB_AcName";
        }
        private void mthBindClear()
        {
            this.nudQtyDeci.DataBindings.Clear();
            this.lblRateDeci.DataBindings.Clear();
        }
        private void btnDone_Click(object sender, EventArgs e)
        {
            pTblStkValConfig.AcceptChanges();
            int i, count = pTblStkValConfig.Rows.Count, rcount = 0;
            for (i = 0; i < count; i++)
            {
                if (pTblStkValConfig.Rows[i]["vName"].ToString().Trim() == "")
                {
                    rcount++;
                }
            }
            if (rcount > 0) { MessageBox.Show("Blank name not allowed.."); return; }


            this.pTblMain.Rows[0]["It_BChk"] = chkonLineBal.Checked;
            this.pTblMain.Rows[0]["Neg_ItBal"]=chkNegBal.Checked ;
            this.pTblMain.Rows[0]["It_Rate"]=chkPartyPrice.Checked;
            this.pTblMain.Rows[0]["It_Stock"]=chkDisplayInStk.Checked;
            this.pTblManufact.Rows[0]["BatchItem"] = this.rdbGoods.Checked;
            this.pTblManufact.Rows[0]["BatchRun"] = this.rdbRun.Checked;
            this.pTblManufact.Rows[0]["autobatch"] = this.chkAutoBatch.Checked;

            this.Close();
        }
        public void mthEnableDisableFormControls()
        {
            bool vEnabled = true;
            
            if (this.pEditMode == false) { vEnabled = false;this.dgvStkVal.Columns["colName"].ReadOnly = true; }
            this.rdbGoods.Enabled = false;
            this.rdbRun.Enabled = false;
            //this.rdbRun.Checked = false;
            if (this.pEditMode == true)  //Divyang
            {
                if (chkAutoBatch.Checked) { this.rdbRun.Enabled = true; this.rdbGoods.Enabled = true; }    
                else { this.rdbRun.Enabled = false; this.rdbGoods.Enabled = false; }
            }
            this.chkAutoBatch.Enabled = vEnabled;        //Divyang
            chkonLineBal.Enabled = vEnabled;
            chkNegBal.Enabled = vEnabled;
            chkPartyPrice.Enabled = vEnabled;
            chkDisplayInStk.Enabled = vEnabled;
            this.nudQtyDeci.Enabled = vEnabled;
            this.nudRateDeci.Enabled = vEnabled;
            
        }
        public void mthControlSet()
        {
        }
        
        private void dgvStkVal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int vRowIndex = dgvStkVal.CurrentRow.Index;
                int vColIndex = dgvStkVal.CurrentCell.ColumnIndex;
                var selectedCell = dgvStkVal.SelectedCells[0];

                if ((e.Modifiers == Keys.Control && e.KeyCode == Keys.I) && (this.pEditMode == true || this.pAddMode == true))
                {
                    DataRow dr = this.pTblStkValConfig.NewRow();
                    this.pTblStkValConfig.Rows.Add(dr);
                    this.dgvStkVal.Refresh();
                    this.dgvStkVal.ClearSelection();
                    vRowIndex = this.dgvStkVal.Rows.Count - 1;
                    vColIndex = this.dgvStkVal.Columns.Count - 1;
                    this.dgvStkVal.Rows[vRowIndex].Cells[0].Selected = true;
                    this.dgvStkVal.CurrentCell = this.dgvStkVal.Rows[vRowIndex].Cells[0];
                }
                if ((e.Modifiers == Keys.Control && e.KeyCode == Keys.T) && (this.pEditMode == true || this.pAddMode == true))
                {
                    selectedCell = dgvStkVal.SelectedCells[0];
                    MenuItemClick(new MenuItem(string.Format("Remove")), new EventArgs());

                }
                if (e.KeyCode != Keys.F2 || selectedCell.ColumnIndex == 0 || selectedCell.ColumnIndex == 1 || this.pEditMode == false)
                {
                    return;
                }

                string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
                DataTable tblPop;

                if(selectedCell.ColumnIndex==2)
                {
                    vSqlStr = "Select StkValMth='Fifo' Union Select 'Lifo' Union Select 'Weighted Average'";
                    VForText = "Select Method Name";
                    vSearchCol = "StkValMth";
                    vDisplayColumnList = "StkValMth:Stock Valuation Metod";
                    vReturnCol = "StkValMth";
                }
                else
                {
                    vSqlStr = "Select  Ac_Name,MailName=(case when isNull(MailName,'')='' then Ac_Name else MailName end) ,Grp=[Group]  From Ac_Mast Order by Ac_Name";
                    
                    VForText = "Select Ledger Name";
                    vSearchCol = "Ac_Name";
                    //vSearchCol = "Ac_Name,MailName";
                    vDisplayColumnList = "Ac_Name:Ledger Name,MailName:Mail Name,Grp:Group";
                    vReturnCol = "Ac_Name";
                 }
                
                tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);

                DataView dvw = tblPop.DefaultView;
                
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
                    dgvStkVal.SelectedCells[0].Value = oSelectPop.pReturnArray[0].Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.pPApplText, MessageBoxButtons.OK);
            }

        }
        
        private void btnDone_Click1(object sender, EventArgs e)
        {
            this.Close();
        }
        private void uomCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       

        private void dgvStkVal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {

                if (e.ColumnIndex == this.dgvStkVal.Columns["colbtnCond"].Index)
                {
                    int vRowIndex = dgvStkVal.CurrentRow.Index, vColIndex = dgvStkVal.CurrentCell.ColumnIndex;
                    frmInventoryCond ofrmInventoryCond = new udCompanySetting.frmInventoryCond();
                    ofrmInventoryCond.pCond = dgvStkVal.CurrentRow.Cells["colCond"].Value.ToString();
                    ofrmInventoryCond.pAddMode = this.pAddMode;
                    ofrmInventoryCond.pEditMode = this.pEditMode;
                    ofrmInventoryCond.oDataAccess = this.oDataAccess;
                    ofrmInventoryCond.ShowDialog();
                    dgvStkVal.CurrentRow.Cells["colCond"].Value = ofrmInventoryCond.pCond;
                    this.dgvStkVal.Refresh();
                }
            }
            catch (Exception vExc)
            {
                MessageBox.Show(vExc.Message);
            }
        }

        private void dgvStkVal_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (this.pEditMode == true || this.pAddMode == true))
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Add", new System.EventHandler(this.MenuItemClick)));
                //  m.MenuItems.Add(new MenuItem("",new System.EventHandler(this.MenuItemClick),Shortcut.))
                int currentMouseOverRow = dgvStkVal.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    m.MenuItems.Add(new MenuItem(string.Format("Remove", currentMouseOverRow.ToString()), new System.EventHandler(this.MenuItemClick)));
                }

                m.Show(dgvStkVal, new Point(e.X, e.Y));


            }
        }
        private void MenuItemClick(Object sender, System.EventArgs e)
        {
            var m = (MenuItem)sender;

            if (this.pEditMode == true || this.pAddMode == true)
            {
                if (m.Text.ToString() == "Add")
                {

                    DataRow drchild = this.pTblStkValConfig.NewRow();
                    drchild["VName"] = "";
                    drchild["VType"] = "";
                    drchild["VCond"] = "";
                    //drchild["OP_AcName"] ="";
                    //drchild["ClP_AcName"] = "";
                    //drchild["ClB_AcName"] = "";

                    this.pTblStkValConfig.Rows.Add(drchild);
                    this.dgvStkVal.Refresh();
                }
                else if (m.Text.ToString() == "Remove")
                {
                    if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                  MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int selectedIndex = dgvStkVal.CurrentCell.RowIndex;
                        dgvStkVal.Rows.RemoveAt(selectedIndex);
                    }

                }
            }
            else
            {

            }
        }
        public DataTable pTblManufact
        {
            get { return vTblManufact; }
            set { vTblManufact = value; }
        }
        public DataTable pTblStkValConfig
        {
            get { return vTblStkValConfig; }
            set { vTblStkValConfig = value; }
        }

        private void dgvStkVal_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)     //Divyang
        {
            int vRowIndex, vcolIndex;
            string vcolNm = "";
            if (dgvStkVal.Rows.Count == 0)
            {
                return;
            }
            vRowIndex = dgvStkVal.CurrentRow.Index;
            vcolIndex = dgvStkVal.CurrentCell.ColumnIndex;
            vcolNm = dgvStkVal.Columns[vcolIndex].Name;

            if (this.pEditMode == true || this.pAddMode == true)
            {
                if (dgvStkVal.Rows.Count > 1)
                {
                    if (vcolNm == "colName")
                    {
                        string vNm = dgvStkVal.Rows[dgvStkVal.Rows.Count - 1].Cells["colName"].EditedFormattedValue.ToString().Trim();

                        int i, count = pTblStkValConfig.Rows.Count, rcount = 0;
                        for (i = 0; i < count - 1; i++)
                        {
                            if (pTblStkValConfig.Rows[i]["vName"].ToString().Trim() == vNm)
                            {
                                rcount++;
                            }
                        }
                        //if (pTblStkValConfig.Rows[i]["colName"].ToString().Trim() == vName)
                        if (rcount > 0)
                        {
                            MessageBox.Show("Duplicate Name not allowed..");
                            dgvStkVal.Focus();
                            dgvStkVal.Rows[e.RowIndex].Cells["colName"].Selected = true;
                            e.Cancel = true;
                        }

                    }
                }
            }
          }

        private void chkAutoBatch_CheckedChanged(object sender, EventArgs e)        //Divyang
        {
            if (chkAutoBatch.Checked)  {  this.rdbRun.Enabled = true; this.rdbGoods.Enabled = true;  }
            else { this.rdbRun.Enabled = false; this.rdbGoods.Enabled = false;  this.rdbRun.Checked = false; this.rdbGoods.Checked = false; }
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
