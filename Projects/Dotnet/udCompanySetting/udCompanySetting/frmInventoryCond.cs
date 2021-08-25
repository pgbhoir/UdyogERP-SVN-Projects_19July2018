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
using System.Xml;

namespace udCompanySetting
{
    public partial class frmInventoryCond : uBaseForm.FrmBaseForm
    {
        string vCond = "";
        DataAccess_Net.clsDataAccess vDataAccess;
        string vSqlStr;
        DataTable tblFlds;
        public frmInventoryCond()
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            this.pFrmCaption = "Condtion Details";
        }
        private void frmInventoryCond_Load(object sender, EventArgs e)
        {

            this.mthReadXml();

            if(this.pEditMode==false)
            {
                this.dgvFlds.Columns["colFldNm"].ReadOnly = true;
                this.dgvFlds.Columns["colDesc"].ReadOnly = true;
            }
        }
        private void dgvFlds_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void mthReadXml()
        {
            
            try
            {
                string vCondition, vDescrip, vTbl_Name;
                //MessageBox.Show(this.pCond);
                tblFlds = new DataTable();
                tblFlds.Columns.Add("condition", typeof(string));
                tblFlds.Columns.Add("descrip", typeof(string));
                tblFlds.Columns.Add("tbl_name", typeof(string));

                XmlDataDocument xmldoc = new XmlDataDocument();
                XmlNodeList xmlnode;
                //FileStream fs = new FileStream(this.pCond, FileMode.Open, FileAccess.Read);
                if (this.pCond != "")       //Divyang
                {
                    xmldoc.LoadXml(this.pCond);
                    xmlnode = xmldoc.GetElementsByTagName("tmpvcond");
                
                    foreach (XmlNode node in xmlnode)
                    {
                        vCondition = node.ChildNodes.Item(0).InnerText.Trim();
                        vDescrip = node.ChildNodes.Item(1).InnerText.Trim();
                        vTbl_Name = node.ChildNodes.Item(2).InnerText.Trim();
                        tblFlds.Rows.Add(vCondition, vDescrip, vTbl_Name);
                    }
                }
                else
                {
                    tblFlds.Rows.Add();
                }

                if(tblFlds.Rows.Count>0)
                {
                    dgvFlds.AutoGenerateColumns = false;
                    dgvFlds.DataSource = tblFlds;
                    dgvFlds.Columns["colFldNm"].DataPropertyName = "condition";
                    dgvFlds.Columns["colDesc"].DataPropertyName = "descrip";
                    dgvFlds.Columns["colTblNm"].DataPropertyName = "tbl_name";
                    //dgvFlds.Columns["colValuation"].DataPropertyName = "tbl_name";
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void mthCreateXml()
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                DataSet dS = new DataSet();
                dS.DataSetName = "VFPData";
                tblFlds.TableName = "tmpvcond";
                tblFlds.AcceptChanges();
                dS.Tables.Add(tblFlds);
                StringWriter sw = new StringWriter();
                tblFlds.WriteXml(sw, XmlWriteMode.IgnoreSchema);
                this.pCond = sw.ToString();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, this.pPApplText, MessageBoxButtons.OK);
            }
        }
        private void btnDone_Click(object sender, EventArgs e)
        {
            if (this.pEditMode) { this.mthCreateXml(); }
            this.Close();
        }
        private void dgvFlds_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.pEditMode == false) { return; }
            try
            {
               
                     int vRowIndex = dgvFlds.CurrentRow.Index;
                     int vColIndex = dgvFlds.CurrentCell.ColumnIndex;
                     var selectedCell = dgvFlds.SelectedCells[0];
                

                if ((e.Modifiers == Keys.Control && e.KeyCode == Keys.I) && (this.pEditMode == true || this.pAddMode == true))
                {
                    DataRow dr = tblFlds.NewRow();
                    this.tblFlds.Rows.Add(dr);
                    this.dgvFlds.Refresh();
                    this.dgvFlds.ClearSelection();
                    vRowIndex = this.dgvFlds.Rows.Count - 1;
                    vColIndex = this.dgvFlds.Columns.Count - 1;
                    this.dgvFlds.Rows[vRowIndex].Cells[0].Selected = true;
                    this.dgvFlds.CurrentCell = this.dgvFlds.Rows[vRowIndex].Cells[0];
                }
                if ((e.Modifiers == Keys.Control && e.KeyCode == Keys.T) && (this.pEditMode == true || this.pAddMode == true))
                {
                    selectedCell = dgvFlds.SelectedCells[0];
                    MenuItemClick(new MenuItem(string.Format("Remove")), new EventArgs());

                }
                if (e.KeyCode != Keys.F2 || selectedCell.ColumnIndex == 1)
                {
                    return;
                }

                string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
                DataTable tblPop=new DataTable();
                
                vSqlStr = "select tbl_alias = CAST('Main' as char(15)), a.[name] as tbl_name,b.[name] as fld_name from sysobjects a inner join syscolumns b on a.id=b.id where a.[name]= 'STKL_VW_MAIN' and a.xtype= 'V' and b.typestat = 2";
                vSqlStr = vSqlStr + " union all";
                vSqlStr = vSqlStr + " select tbl_alias = CAST('Item' as char(15)), a.[name] as tbl_name, b.[name] as fld_name from sysobjects a inner join syscolumns b on a.id= b.id where a.[name]= 'STKL_VW_ITEM' and a.xtype= 'V' and b.typestat = 2";
                vSqlStr = vSqlStr + " union all";
                vSqlStr = vSqlStr + " select tbl_alias = CAST('Item Master' as char(15)), a.[name] as tbl_name, b.[name] as fld_name from sysobjects a inner join syscolumns b on a.id= b.id where a.[name]= 'IT_MAST' and a.xtype= 'U' and b.typestat = 2";
                vSqlStr = vSqlStr + " order by a.[name], b.[name]";


                VForText = "Select Field Name";
                //vSearchCol = "fld_name,tbl_alias";
                vSearchCol = "fld_name";
                vDisplayColumnList = "fld_name:Field Name,tbl_alias:table Name";
                vReturnCol = "fld_name,tbl_alias,tbl_name";
                
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
                    dgvFlds.CurrentRow.Cells["colFldNm"].Value= oSelectPop.pReturnArray[0].Trim();
                    dgvFlds.CurrentRow.Cells["colTblNm"].Value = oSelectPop.pReturnArray[2].Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.pPApplText, MessageBoxButtons.OK);
            }
        }
        private void dgvFlds_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (this.pEditMode == true || this.pAddMode == true))
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Add", new System.EventHandler(this.MenuItemClick)));
                //  m.MenuItems.Add(new MenuItem("",new System.EventHandler(this.MenuItemClick),Shortcut.))
                int currentMouseOverRow = dgvFlds.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    m.MenuItems.Add(new MenuItem(string.Format("Remove", currentMouseOverRow.ToString()), new System.EventHandler(this.MenuItemClick)));
                }

                m.Show(dgvFlds, new Point(e.X, e.Y));


            }
        }
        private void MenuItemClick(Object sender, System.EventArgs e)
        {
            var m = (MenuItem)sender;

            if (this.pEditMode == true || this.pAddMode == true)
            {
                if (m.Text.ToString() == "Add")
                {
                    DataRow drchild = this.tblFlds.NewRow();
                    drchild["condition"] = "";
                    drchild["descrip"] = "";
                    drchild["tbl_name"] = "";
                    
                    this.tblFlds.Rows.Add(drchild);
                    //dgvFlds.DataSource = tblFlds;   //Divyang
                    this.dgvFlds.Refresh(); 
                }
                else if (m.Text.ToString() == "Remove")
                {
                    if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                  MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int selectedIndex = dgvFlds.CurrentCell.RowIndex;
                        dgvFlds.Rows.RemoveAt(selectedIndex);
                    }

                }
            }
            else
            {

            }
        }
        

        private void uomCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public string pCond
        {
            get { return vCond; }
            set { vCond = value; }
        }
        public DataAccess_Net.clsDataAccess oDataAccess
        {
            get { return vDataAccess; }
            set { vDataAccess = value; }
        }

        private void dgvFlds_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)   //Divyang
        {
            int vRowIndex, vcolIndex;
            string vcolNm = "";
            if (dgvFlds.Rows.Count == 0)
            {
                return;
            }
            vRowIndex = dgvFlds.CurrentRow.Index;
            vcolIndex = dgvFlds.CurrentCell.ColumnIndex;
            vcolNm = dgvFlds.Columns[vcolIndex].Name;

            if (this.pEditMode == true || this.pAddMode == true)
            {
                if (dgvFlds.Rows.Count > 1)
                {
                    if (vcolNm == "colDesc")
                    {
                        string vNm = dgvFlds.Rows[dgvFlds.Rows.Count - 1].Cells["colDesc"].EditedFormattedValue.ToString().Trim();

                        int i, count = tblFlds.Rows.Count, rcount = 0;
                        for (i = 0; i < count - 1; i++)
                        {
                            if (tblFlds.Rows[i]["descrip"].ToString().Trim() == vNm)
                            {
                                rcount++;
                            }
                        }
                        if (rcount > 0)
                        {
                            MessageBox.Show("Duplicate Description not allowed..");
                            dgvFlds.Focus();
                            dgvFlds.Rows[e.RowIndex].Cells["colDesc"].Selected = true;
                            e.Cancel = true;
                        }

                    }
                }
            }
        }
    }
}
