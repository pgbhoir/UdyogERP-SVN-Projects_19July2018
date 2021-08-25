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
    public partial class frmMultiUOM :  uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess vDataAccess;
        DataTable vTblMain;
        string vSqlStr;
        short vTimeOut = 15;
        string vShowTips = "0";
        Decimal vOrgNuUOM = 0;
        DataTable tblUOM;
        bool vFormLoad = true;
        public frmMultiUOM()
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            this.pFrmCaption = "Multi UOM Details";
        }

        private void frmMultiUOM_Load(object sender, EventArgs e)
        {
            this.mthControlSet();
            this.mthGridSet();
            vFormLoad = false;
            this.mthEnableDisableFormControls();
        }
        private void mthControlSet()
        {
            if ((decimal)this.pTblMain.Rows[0]["uom_no"] > 0) { nudNoUOM.Value = (decimal)this.pTblMain.Rows[0]["uom_no"]; vOrgNuUOM = nudNoUOM.Value; }

            tblUOM = new DataTable();
            tblUOM.Columns.Add("colQty", typeof(string));
            tblUOM.Columns.Add("colUOM", typeof(string));
            //string[] vArUOM1 = { "" };  
            string[] vArUOM1 = new string[0]; //Divyang
            //if (this.pTblMain.Rows[0]["UOM_Desc"] != null)   //Divyang
            if (this.pTblMain.Rows[0]["UOM_Desc"].ToString().Trim() != "")
            {
                vArUOM1 = this.pTblMain.Rows[0]["UOM_Desc"].ToString().Split(';');
            }

            for (int i = 0; i <= vArUOM1.Length - 1; i++)
            {
                DataRow dr = tblUOM.NewRow();
                dr["colQty"] = vArUOM1[i].Substring(0, vArUOM1[i].IndexOf(":")).Trim();
                dr["colUOM"] = vArUOM1[i].Substring(vArUOM1[i].IndexOf(":") + 1).Trim();
                tblUOM.Rows.Add(dr);
            }
        }
        private void mthGridSet()
        {
            dgvmain.AutoGenerateColumns = false;
            dgvmain.DataSource = tblUOM;
            dgvmain.Columns["colQty"].DataPropertyName = "colQty";
            dgvmain.Columns["colUOM"].DataPropertyName = "colUOM";
        }
       
        private void nudNoUOM_ValueChanged(object sender, EventArgs e)
        {
            if (nudNoUOM.Value==0) {MessageBox.Show("No. of UOM Quantity should be Greater than or equal to 1.", this.pPApplText, MessageBoxButtons.OK); this.nudNoUOM.Value = 1;  return; }
            if ((int)nudNoUOM.Value == this.dgvmain.Rows.Count || vFormLoad) { return; }
            if ((int)nudNoUOM.Value > this.dgvmain.Rows.Count)
            {
                for (int i = this.dgvmain.Rows.Count; i < (int)nudNoUOM.Value; i++)
                {
                    DataRow dr = tblUOM.NewRow();
                    dr["colQty"] = "QTY" +(i>0? i.ToString().Trim():"");
                    dr["colUOM"] = "";
                    tblUOM.Rows.Add(dr);
                }
            }
            if ((int)nudNoUOM.Value < this.dgvmain.Rows.Count)
            {
                for (int i = this.dgvmain.Rows.Count; i > (int)nudNoUOM.Value; i--)
                {
                    vSqlStr = "Select UOM_Desc From lCode Where Ext_Vou = 0 and bCode_Nm = '' and isNull(UOM_Desc,'')<> '' and UOM_Desc like '%"+ tblUOM.Rows[i-1]["colQty"].ToString().Trim() + ":%'";
                    DataTable tblLcodeChk = new DataTable();
                    tblLcodeChk = oDataAccess.GetDataTable(vSqlStr, null, vTimeOut);
                    if(tblLcodeChk.Rows.Count>0)
                    {
                        MessageBox.Show("Multi UOM is activated in some transactions. Cannot decrease the No. of UOM Quantity field.", this.pPApplText, MessageBoxButtons.OK);
                        nudNoUOM.Value = nudNoUOM.Value + 1;
                        return;
                    }
                    tblUOM.Rows.RemoveAt(i-1);
                }
            }

        }

        private void nudNoUOM_Validated(object sender, EventArgs e)
        {
           
        }
        
        private void dgvmain_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.pEditMode == false) { return; }
            try
            {
                if (dgvmain.Rows.Count > 0)
                {
                    var selectedCell = dgvmain.SelectedCells[0];

                    if (e.KeyCode != Keys.F2 || selectedCell.ColumnIndex == 0)
                    //if (e.KeyCode != Keys.F2 || selectedCell.ColumnIndex == 0 || (this.pEditMode == false && this.pAddMode == false))
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
                string vWhrCond = "";
                foreach(DataRow dr in tblUOM.Rows)
                {
                    vWhrCond =   vWhrCond +"," + "'" + dr["colUOM"].ToString().Trim() + "'";
                }
                vWhrCond = (vWhrCond.Length > 1 ?" Where u_uom not in ("+vWhrCond.Substring(1)+")" : vWhrCond);
                
                string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
                DataTable tblPop;

                vSqlStr = "Select u_uom=Upper(u_uom) From Uom " + vWhrCond + " order by u_uom";
                tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);

                DataView dvw = tblPop.DefaultView;
                //dvw.Sort = "ac_name";

                VForText = "Select Unit of Measure";
                vSearchCol = "u_uom";
                vDisplayColumnList = "u_uom:Unit of Measure";
                vReturnCol = "u_uom";
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
                    dgvmain.SelectedCells[0].Value=oSelectPop.pReturnArray[0].Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.pPApplText, MessageBoxButtons.OK);
            }
        }
        private void btnDone_Click(object sender, EventArgs e)
        {
            if (this.pEditMode == false) { this.Close(); return; }

            int i, count = tblUOM.Rows.Count, rcount = 0;
            for (i = 0; i < count; i++)
            {
                if (tblUOM.Rows[i]["colUOM"].ToString().Trim() == "")
                {
                    rcount++;
                }
            }
            if (rcount > 0) { MessageBox.Show("Enter valid Unit of Measurement."); return; }

            this.pTblMain.Rows[0]["UOM_Desc"] = "";
            foreach (DataRow dr in tblUOM.Rows)
            {
                this.pTblMain.Rows[0]["UOM_Desc"] = this.pTblMain.Rows[0]["UOM_Desc"] + ";" + dr["colQty"].ToString() + ":" + dr["colUOM"].ToString();
            }
            if (tblUOM.Rows.Count > 0)
            {
                this.pTblMain.Rows[0]["UOM_Desc"] = this.pTblMain.Rows[0]["UOM_Desc"].ToString().Substring(1);
                this.pTblMain.Rows[0]["UOM_No"] = (int)nudNoUOM.Value;
            }
            this.Close();
        }
        private void uomCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void mthEnableDisableFormControls()
        {
            bool vEnabled = true;
            if (this.pEditMode == false) { vEnabled = false;this.dgvmain.ReadOnly = true; }
            this.nudNoUOM.Enabled = vEnabled;
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
