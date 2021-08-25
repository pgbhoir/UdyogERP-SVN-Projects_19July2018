using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udAddInfoMaster
{
    public partial class frmQuery : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        string vQuery;
        string SqlStr;
        short vTimeOut = 15;
        string vSerachFld, vRtnFld, vExcleFld, vFldLst;
        DataTable tblMain;
        public frmQuery()
        {
            InitializeComponent();
        }

        private void frmQuery_Load(object sender, EventArgs e)
        {
            string vvQuery = this.pQuery;
           if (vvQuery != ""  &&  vvQuery.IndexOf("{")>-1)    //Divyang
           //if (vvQuery != "" )
            {
                
                this.txtQuery.Text = vvQuery.Substring(0, vvQuery.IndexOf("{"));  
                //this.txtQuery.Text = vvQuery.Substring(0, Math.Max(0, vvQuery.IndexOf("{")));
                vvQuery = vvQuery.Replace(this.txtQuery.Text, "");        
                this.txtpara.Text = vvQuery;

                //vEntryList = SqlStr.Split('');
                string[] vPara = this.txtpara.Text.Replace("{", "").Replace("}", "").Split('#');
                vSerachFld = vPara[0];
                vRtnFld = vPara[1];
                vExcleFld = vPara[2];
                vFldLst = vPara[3];
                vSerachFld = vSerachFld.Replace("{", "");
                vFldLst = vFldLst.Replace("}", "");


                this.mthGridSet();
            }
            else    //Divyang
            {
                this.txtQuery.Text = vvQuery;

            }
        }

        private void mthParaUpdate()
        {
            this.dgvMain.Refresh();
            if (dgvMain.Rows.Count == 0) { return; }
            vSerachFld = ""; vRtnFld = "";vExcleFld = "";vFldLst = "";

            foreach(DataGridViewRow dgvr in dgvMain.Rows)
            {
                vSerachFld = vSerachFld + (dgvr.Cells["colSearchFld"].Value.ToString() == "True" ? "+" + dgvr.Cells["colFldLst"].Value : "");
                vRtnFld = vRtnFld + (dgvr.Cells["ColRetFld"].Value.ToString() == "True" ? "+" + dgvr.Cells["colFldLst"].Value : "");
                vExcleFld = vExcleFld + (dgvr.Cells["colExclFld"].Value.ToString() == "True" ? "+" + dgvr.Cells["colFldLst"].Value : "");
                vFldLst = vFldLst  + (dgvr.Cells["colFldLst"].Value.ToString() != "" ? "," + dgvr.Cells["colFldLst"].Value : "");
                vFldLst= vFldLst+ (dgvr.Cells["colCaption"].Value.ToString() != "" ?":"+ dgvr.Cells["colCaption"].Value : ":"+ dgvr.Cells["colFldLst"].Value.ToString());
            }
            vSerachFld =(vSerachFld!=""? vSerachFld.Substring(1):"");
            vRtnFld =   (vRtnFld!=""?vRtnFld.Substring(1):"");
            vExcleFld = (vExcleFld!=""?vExcleFld.Substring(1):"");
            vFldLst =   (vFldLst!=""?vFldLst.Substring(1):"");
            this.txtpara.Text = "{" + vSerachFld + "#" + vRtnFld + "#" + vExcleFld + "#" + vFldLst + "}";
        }
       
        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {

            DataRow[] dr = tblMain.Select("SearchFld='"+true+"'");
            if (dr.Length == 0) { MessageBox.Show("There should be atleast one Search Field.");  return; }

            DataRow[] dr1 = tblMain.Select("RetFld='" + true + "'");
            if (dr.Length == 0) { MessageBox.Show("There should be atleast one Return  Field."); return; }

            this.mthParaUpdate();
            this.pQuery = this.txtQuery.Text.Trim() + " " + this.txtpara.Text.Trim();
            this.Close();
        }

        private void btnChkQuery_Click(object sender, EventArgs e)
        {
            if (this.txtQuery.Text != "")
            {
                this.mthGridSet();
            }
        }

        private void dgvMain_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvMain.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvMain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.mthParaUpdate();
        }
        
        private void dgvMain_CellContentClick(object sender, DataGridViewCellEventArgs e)           //Divyang
        {
            if (dgvMain.Columns.IndexOf(dgvMain.Columns["ColRetFld"]) == e.ColumnIndex)
            {
                int currentcolumnindex = e.ColumnIndex;
                int currentrowindex = e.RowIndex; 
                foreach (DataGridViewRow dr in dgvMain.Rows)            //Divyang
                {
                    dr.Cells[currentcolumnindex].Value = false;
                }
                dgvMain.CurrentRow.Cells["ColRetFld"].Value = true;
            }
            if (dgvMain.Columns.IndexOf(dgvMain.Columns["colExclFld"]) == e.ColumnIndex)
            {
                int currentcolumnindex = e.ColumnIndex;
                int currentrowindex = e.RowIndex;
                foreach (DataGridViewRow dr in dgvMain.Rows)            
                {
                    if (dgvMain.CurrentRow.Cells["colSearchFld"].Value.ToString() == "True"   && dgvMain.CurrentRow.Cells["colExclFld"].Value.ToString() == "True")
                    {
                        MessageBox.Show("You have selected this Field for Search, cannot Exclude.");
                        //dr.Cells["colExclFld"].Value = false;
                        dgvMain.CurrentRow.Cells["colExclFld"].Value = false;
                        this.dgvMain.RefreshEdit();
                        return;
                    }
                }
               
            }

        }

        private void mthGridSet()
        {
            
            DataTable tblColList = new DataTable();
            try
            {
                tblColList = oDataAccess.GetDataTable(this.txtQuery.Text, null, vTimeOut);
                string vColHead;
                string[] vArFldLst;
                if (vFldLst ==null) { vFldLst = ""; } if (vSerachFld == null) { vSerachFld = ""; } if (vRtnFld == null) { vRtnFld = ""; } if (vExcleFld == null) { vExcleFld = ""; }


                vArFldLst = vFldLst.Split(',');
                tblMain = new DataTable();
                tblMain.Columns.Add("FldList", typeof(string));
                tblMain.Columns.Add("SearchFld", typeof(bool));
                tblMain.Columns.Add("RetFld", typeof(bool));
                tblMain.Columns.Add("ExclFld", typeof(bool));
                tblMain.Columns.Add("Caption", typeof(string));
                foreach (DataColumn col in tblColList.Columns)
                {
                    DataRow dr = tblMain.NewRow();
                    dr["FldList"] = col.ColumnName;
                    dr["SearchFld"] = (vSerachFld.ToUpper().IndexOf(col.ColumnName.ToUpper()) > -1 ? true : false);
                    dr["RetFld"] = (vRtnFld.ToUpper().IndexOf(col.ColumnName.ToUpper()) > -1 ? true : false);
                    dr["ExclFld"] = (vExcleFld.ToUpper().IndexOf(col.ColumnName.ToUpper()) > -1 ? true : false);
                    for (int i = 0; i <= vArFldLst.Length - 1; i++)
                    {
                        if (vArFldLst[i].ToUpper().Contains(col.ColumnName.ToUpper()))
                        {
                            vColHead = vArFldLst[i];
                            vColHead = vColHead.ToUpper().Substring(vColHead.ToUpper().IndexOf(":") + 1);
                            dr["Caption"] = vColHead;
                        }

                    }
                    tblMain.Rows.Add(dr);
                }
                dgvMain.AutoGenerateColumns = false;
                dgvMain.DataSource = tblMain;
                dgvMain.Columns["colFldLst"].DataPropertyName = "FldList";
                dgvMain.Columns["colSearchFld"].DataPropertyName = "SearchFld";
                dgvMain.Columns["ColRetFld"].DataPropertyName = "RetFld";
                dgvMain.Columns["colExclFld"].DataPropertyName = "ExclFld";
                dgvMain.Columns["colCaption"].DataPropertyName = "Caption";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        
        public DataAccess_Net.clsDataAccess pDataAccess
        {
            get { return oDataAccess; }
            set { oDataAccess= value; }
        } 
        public string pQuery
        {
            get { return vQuery; }
            set { vQuery = value;}
        }
    }
}
