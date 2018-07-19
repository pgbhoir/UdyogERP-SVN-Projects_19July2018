using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CustModAccUI.BLL;

namespace CustModAccUI.UI
{
    public partial class frmMenuTranRept : Form
    {
        cls_Gen_Mgr_CustModAccUI m_obj_CustModAccUI;
        DataTable dt1, dt2, dt3;

        bool isGrd = false;

        #region Properties
        private DataSet dsMain;

        public DataSet DsMain
        {
            get { return dsMain; }
            set { dsMain = value; }
        }

        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string ccomp;

        public string Ccomp
        {
            get { return ccomp; }
            set { ccomp = value; }
        }

        private bool addMode;

        public bool AddMode
        {
            get { return addMode; }
            set { addMode = value; }
        }

        private bool editMode;

        public bool EditMode
        {
            get { return editMode; }
            set { editMode = value; }
        }

        
        #endregion

        #region Private Methods
        private void CtrlForecolor()
        {
            grpMenu.ForeColor = Color.DarkOrchid;
            foreach (Control ctl in grpMenu.Controls)
            {
                ctl.ForeColor = SystemColors.ControlText;
            }
            lbl_mnu_total.ForeColor = Color.Crimson;

            grpTran.ForeColor = Color.DarkOrchid;
            foreach (Control ctl in grpTran.Controls)
            {
                ctl.ForeColor = SystemColors.ControlText;
            }
            lbl_tran_total.ForeColor = Color.Crimson;

            grpReport.ForeColor = Color.DarkOrchid;
            foreach (Control ctl in grpReport.Controls)
            {
                ctl.ForeColor = SystemColors.ControlText;
            }
            lbl_rpt_total.ForeColor = Color.Crimson;
        }

        private void EnableDisable(bool value)
        {
            foreach (DataGridViewColumn dcol in dgvMenu.Columns)
            {
                if (dcol.Index.Equals(0))
                    dcol.ReadOnly = true;
                else
                    dcol.ReadOnly = !value;
            }
            foreach (DataGridViewColumn dcol in dgvReport.Columns)
            {
                if (dcol.Index.Equals(0))
                    dcol.ReadOnly = true;
                else
                    dcol.ReadOnly = !value;
            }
            foreach (DataGridViewColumn dcol in dgvTran.Columns)
            {
                if (dcol.Index.Equals(0))
                    dcol.ReadOnly = true;
                else
                    dcol.ReadOnly = !value;
            }

            btn_mnuAdd.Enabled = value;
            btn_mnuDelete.Enabled = value;
            btn_rptAdd.Enabled = value;
            btn_rptDelete.Enabled = value;
            btn_tranAdd.Enabled = value;
            btn_tranDelete.Enabled = value;
            btnCancel.Enabled = value;
            btnDone.Enabled = value;
        }

        private void Delete(DataGridView ctrlobj)
        {
            if (ctrlobj.SelectedCells.Count > 0)
            {
                if (ctrlobj.Name == "dgvMenu")
                {
                    DialogResult nRes = MessageBox.Show("Delete Menu?", m_obj_CustModAccUI.Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (nRes == DialogResult.No)
                        return;
                    dt1.DefaultView.RowFilter = "optiontype='MENU' and ccomp='" + Ccomp.ToString().Trim() + "'";
                    dt1.DefaultView.Delete(ctrlobj.SelectedCells[0].RowIndex);
                    ctrlobj.DataSource = dt1;

                    if (dt1.Select("optiontype='MENU' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length > 0)
                        lbl_mnu_total.Text = "Total Menus : " + dt1.Select("optiontype='MENU' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length.ToString().Trim();
                    else
                        lbl_mnu_total.Text = "Total Menus : 0";
                    isGrd = true;
                }
                else if (ctrlobj.Name == "dgvTran")
                {
                    DialogResult nRes = MessageBox.Show("Delete Transaction?", m_obj_CustModAccUI.Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (nRes == DialogResult.No)
                        return;
                    dt2.DefaultView.RowFilter = "optiontype='TRAN' and ccomp='" + Ccomp.ToString().Trim() + "'";
                    dt2.DefaultView.Delete(ctrlobj.SelectedCells[0].RowIndex);
                    ctrlobj.DataSource = dt2;

                    if (dt2.Select("optiontype='TRAN' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length > 0)
                        lbl_tran_total.Text = "Total Transactions : " + dt2.Select("optiontype='TRAN' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length.ToString().Trim();
                    else
                        lbl_tran_total.Text = "Total Transactions : 0";
                    isGrd = true;
                }
                else if (ctrlobj.Name == "dgvReport")
                {
                    DialogResult nRes = MessageBox.Show("Delete Report?", m_obj_CustModAccUI.Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (nRes == DialogResult.No)
                        return;
                    dt3.DefaultView.RowFilter = "optiontype='REPORT' and ccomp='" + Ccomp.ToString().Trim() + "'";
                    dt3.DefaultView.Delete(ctrlobj.SelectedCells[0].RowIndex);
                    ctrlobj.DataSource = dt3;

                    if (dt3.Select("optiontype='REPORT' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length > 0)
                        lbl_rpt_total.Text = "Total Reports : " + dt3.Select("optiontype='REPORT' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length.ToString().Trim();
                    else
                        lbl_rpt_total.Text = "Total Reports : 0";
                    isGrd = true;
                }
                ctrlobj.Refresh();
            }
        }

        private void Add(DataGridView ctrlobj)
        {
            if (ctrlobj.Name == "dgvMenu")
            {
                int srno = dt1.Select("optiontype='MENU' and ccomp='" + Ccomp.ToString().Trim() + "'").Length;
                DataRow dr1 = dt1.NewRow();
                dr1["srno"] = srno + 1;
                dr1["fk_id"] = Id;
                dr1["ccomp"] = Ccomp.ToString().Trim();
                dr1["optiontype"] = "MENU";
                dt1.Rows.Add(dr1);
                dt1.DefaultView.RowFilter = "optiontype='MENU' and ccomp='" + Ccomp.ToString().Trim() + "'";
                ctrlobj.DataSource = dt1;

                if (dt1.Select("optiontype='MENU' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length > 0)
                    lbl_mnu_total.Text = "Total Menus : " + dt1.Select("optiontype='MENU' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length.ToString().Trim();
                else
                    lbl_mnu_total.Text = "Total Menus : 0";
                isGrd = true;
                ctrlobj.CurrentCell = ctrlobj.Rows[srno].Cells[1];
            }
            else if (ctrlobj.Name == "dgvTran")
            {
                int srno = dt2.Select("optiontype='TRAN' and ccomp='" + Ccomp.ToString().Trim() + "'").Length;
                DataRow dr1 = dt2.NewRow();
                dr1["srno"] = srno + 1;
                dr1["fk_id"] = Id;
                dr1["ccomp"] = Ccomp.ToString().Trim();
                dr1["optiontype"] = "TRAN";
                dt2.Rows.Add(dr1);
                dt2.DefaultView.RowFilter = "optiontype='TRAN' and ccomp='" + Ccomp.ToString().Trim() + "'";
                ctrlobj.DataSource = dt2;

                if (dt2.Select("optiontype='TRAN' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length > 0)
                    lbl_tran_total.Text = "Total Transactions : " + dt2.Select("optiontype='TRAN' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length.ToString().Trim();
                else
                    lbl_tran_total.Text = "Total Transactions : 0";
                isGrd = true;
                ctrlobj.CurrentCell = ctrlobj.Rows[srno].Cells[1];
            }
            else if (ctrlobj.Name == "dgvReport")
            {
                int srno = dt3.Select("optiontype='REPORT' and ccomp='" + Ccomp.ToString().Trim() + "'").Length;
                DataRow dr1 = dt3.NewRow();
                dr1["srno"] = srno + 1;
                dr1["fk_id"] = Id;
                dr1["ccomp"] = Ccomp.ToString().Trim();
                dr1["optiontype"] = "REPORT";
                dt3.Rows.Add(dr1);
                dt3.DefaultView.RowFilter = "optiontype='REPORT' and ccomp='" + Ccomp.ToString().Trim() + "'";
                ctrlobj.DataSource = dt3;

                if (dt3.Select("optiontype='REPORT' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length > 0)
                    lbl_rpt_total.Text = "Total Reports : " + dt3.Select("optiontype='REPORT' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length.ToString().Trim();
                else
                    lbl_rpt_total.Text = "Total Reports : 0";
                isGrd = true;
                ctrlobj.CurrentCell = ctrlobj.Rows[srno].Cells[1];
            }
            ctrlobj.Refresh();
        }
        #endregion

        public frmMenuTranRept(string ccomp_nm,string connString,string username,int range,string IcoPath,string Vumess)
        {
            Ccomp = ccomp_nm.ToString().Trim();
            m_obj_CustModAccUI = new cls_Gen_Mgr_CustModAccUI(connString, username,range);
            InitializeComponent();
            Icon ico = new Icon(IcoPath);
            this.Icon = ico;
            m_obj_CustModAccUI.Vumess = Vumess;
        }

        private void frmMenuTranRept_Load(object sender, EventArgs e)
        {
            try
            {
                int i;
                //MENU GRID
                dgvMenu.AutoGenerateColumns = false;
                dgvMenu.DataMember = DsMain.Tables["subdetailtbl"].TableName;   //"Table";
                dt1 = DsMain.Tables["subdetailtbl"].Copy();
                dt1.DefaultView.RowFilter = "optiontype='MENU' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'";
                i = 1;
                foreach (DataRow dr in dt1.Select("optiontype='MENU' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'"))
                {
                    dr["srno"] = i;
                    i++;
                }
                dgvMenu.DataSource = dt1;
                dgvMenu.Refresh();

                if (dt1.Select("optiontype='MENU' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length > 0)
                    lbl_mnu_total.Text = "Total Menus : " + dt1.Select("optiontype='MENU' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length.ToString().Trim();
                else
                    lbl_mnu_total.Text = "Total Menus : 0";

                //TRANSACTION GRID
                dgvTran.AutoGenerateColumns = false;
                dgvTran.DataMember = DsMain.Tables["subdetailtbl"].TableName;  //"Table1";
                dt2 = DsMain.Tables["subdetailtbl"].Copy();
                dt2.DefaultView.RowFilter = "optiontype='TRAN' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'";
                i = 1;
                foreach (DataRow dr in dt2.Select("optiontype='TRAN' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'"))
                {
                    dr["srno"] = i;
                    i++;
                }
                dgvTran.DataSource = dt2;
                dgvTran.Refresh();

                if (dt2.Select("optiontype='TRAN' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length > 0)
                    lbl_tran_total.Text = "Total Transactions : " + dt2.Select("optiontype='TRAN' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length.ToString().Trim();
                else
                    lbl_tran_total.Text = "Total Transactions : 0";

                //REPORT GRID
                dgvReport.AutoGenerateColumns = false;
                dgvReport.DataMember = DsMain.Tables["subdetailtbl"].TableName;  //"Table2";
                dt3 = DsMain.Tables["subdetailtbl"].Copy();
                dt3.DefaultView.RowFilter = "optiontype='REPORT' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'";
                i = 1;
                foreach (DataRow dr in dt3.Select("optiontype='REPORT' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'"))
                {
                    dr["srno"] = i;
                    i++;
                }
                dgvReport.DataSource = dt3;
                dgvReport.Refresh();

                if (dt3.Select("optiontype='REPORT' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length > 0)
                    lbl_rpt_total.Text = "Total Reports : " + dt3.Select("optiontype='REPORT' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'").Length.ToString().Trim();
                else
                    lbl_rpt_total.Text = "Total Reports : 0";

                CtrlForecolor();
                if (AddMode == false && EditMode == false)
                {
                    EnableDisable(false);
                    btnCancel.Enabled = true;
                    toolTip1.SetToolTip(btnCancel, "Close (Ctrl+Z)");
                }
                else
                {
                    EnableDisable(true);
                    toolTip1.SetToolTip(btnCancel, "Cancel (Ctrl+Z)");
                }
                this.Text = this.Text + " - [" + Ccomp.ToString().ToUpper().Trim() + "]";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btn_mnuAdd_Click(object sender, EventArgs e)
        {
            Add(dgvMenu);
        }

        private void btn_mnuDelete_Click(object sender, EventArgs e)
        {
            Delete(dgvMenu);
        }

        private void btn_tranDelete_Click(object sender, EventArgs e)
        {
            Delete(dgvTran);
        }

        private void btn_rptDelete_Click(object sender, EventArgs e)
        {
            Delete(dgvReport);
        }

        private void btn_tranAdd_Click(object sender, EventArgs e)
        {
            Add(dgvTran);
        }

        private void btn_rptAdd_Click(object sender, EventArgs e)
        {
            Add(dgvReport);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {
                bool isFound = false;
                if (AddMode == true || EditMode == true)
                {
                    for (int i = 0; i < dgvTran.Rows.Count; i++)
                    {
                        if (dgvTran.Rows[i].Cells["entry_ty"].Value.ToString().ToUpper().Trim() != string.Empty)
                        {
                            if (dgvTran.Rows[i].Cells["entry_ty"].Value.ToString().ToUpper().Trim().StartsWith("X") || dgvTran.Rows[i].Cells["entry_ty"].Value.ToString().ToUpper().Trim().StartsWith("Y") || dgvTran.Rows[i].Cells["entry_ty"].Value.ToString().ToUpper().Trim().StartsWith("Z"))
                            { }
                            else
                            {
                                isFound = true;
                                break;
                            }
                        }
                    }
                     
                    if (isFound == true)
                    {
                        DialogResult x;
                        x = MessageBox.Show("Want to continue with the transaction type enetered by you?", m_obj_CustModAccUI.Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (x == DialogResult.No)
                            return;
                        isFound = false;
                    }
                }

                if (AddMode == true)
                {
                    foreach (DataRow dr in DsMain.Tables["subdetailtbl"].Select("ccomp='" + Ccomp.ToString().ToUpper().Trim() + "' and optiontype <> ''"))// and desc1 <> '' and desc2 <> ''"))//or desc3 <> ''"))
                        dr.Delete();
                }
                else if (EditMode == true)
                {
                    foreach (DataRow dr in DsMain.Tables["subdetailtbl"].Select("ccomp='" + Ccomp.ToString().ToUpper().Trim() + "' and (optiontype <> '' or desc1 <> '' or desc2 <> '' or desc3 <> '')"))
                        dr.Delete();
                }

                foreach (DataRow dr in dt1.Select("optiontype='MENU' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'"))
                {
                    if (dr["desc1"].ToString().ToUpper().Trim() == string.Empty && dr["desc2"].ToString().ToUpper().Trim() == string.Empty)
                        continue;
                    DataRow newRow = DsMain.Tables["subdetailtbl"].NewRow();
                    newRow["srno"] = dr["srno"];
                    newRow["id"] = dr["id"];
                    newRow["fk_id"] = dr["fk_id"];
                    newRow["ccomp"] = dr["ccomp"];
                    newRow["optiontype"] = dr["optiontype"];
                    newRow["desc1"] = dr["desc1"];
                    newRow["desc2"] = dr["desc2"];
                    newRow["desc3"] = dr["desc3"];
                    DsMain.Tables["subdetailtbl"].Rows.Add(newRow);
                }
                foreach (DataRow dr in dt2.Select("optiontype='TRAN' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'"))
                {
                    if (dr["desc1"].ToString().ToUpper().Trim() == string.Empty && dr["desc2"].ToString().ToUpper().Trim() == string.Empty)
                        continue;
                    DataRow newRow = DsMain.Tables["subdetailtbl"].NewRow();
                    newRow["srno"] = dr["srno"];
                    newRow["id"] = dr["id"];
                    newRow["fk_id"] = dr["fk_id"];
                    newRow["ccomp"] = dr["ccomp"];
                    newRow["optiontype"] = dr["optiontype"];
                    newRow["desc1"] = dr["desc1"];
                    newRow["desc2"] = dr["desc2"];
                    newRow["desc3"] = dr["desc3"];
                    DsMain.Tables["subdetailtbl"].Rows.Add(newRow);
                }
                foreach (DataRow dr in dt3.Select("optiontype='REPORT' and ccomp='" + Ccomp.ToString().ToUpper().Trim() + "'"))
                {
                    if (dr["desc1"].ToString().ToUpper().Trim() == string.Empty && dr["desc2"].ToString().ToUpper().Trim() == string.Empty && dr["desc3"].ToString().ToUpper().Trim() == string.Empty)
                        continue;
                    DataRow newRow = DsMain.Tables["subdetailtbl"].NewRow();
                    newRow["srno"] = dr["srno"];
                    newRow["id"] = dr["id"];
                    newRow["fk_id"] = dr["fk_id"];
                    newRow["ccomp"] = dr["ccomp"];
                    newRow["optiontype"] = dr["optiontype"];
                    newRow["desc1"] = dr["desc1"];
                    newRow["desc2"] = dr["desc2"];
                    newRow["desc3"] = dr["desc3"];
                    DsMain.Tables["subdetailtbl"].Rows.Add(newRow);
                }

                dgvMenu.DataSource = DsMain.Tables["subdetailtbl"];
                dgvMenu.Refresh();

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (isGrd == true)
            {
                DialogResult nRes = MessageBox.Show("Discard the Changes?", m_obj_CustModAccUI.Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (nRes == DialogResult.No)
                    return;
                else
                    Close();
            }
            else
                Close();
        }
       
        private void dgvMenu_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (AddMode == true || EditMode == true)
            {
                foreach (DataGridViewRow row in dgvMenu.Rows)
                {
                    if (row.Index != e.RowIndex & !row.IsNewRow)
                    {
                        //if (dgvMenu.CurrentRow.Cells["padname"].Value.ToString().ToUpper().Trim() != string.Empty && dgvMenu.CurrentRow.Cells["barname"].Value.ToString().ToUpper().Trim() != string.Empty)
                        //{
                            if (row.Cells["padname"].Value.ToString().ToUpper().Trim() == dgvMenu.CurrentRow.Cells["padname"].Value.ToString().ToUpper().Trim() && row.Cells["barname"].Value.ToString().ToUpper().Trim() == dgvMenu.CurrentRow.Cells["barname"].Value.ToString().ToUpper().Trim())
                            {
                                dgvMenu.Rows[e.RowIndex].ErrorText = "Duplicate menu not allowed";
                                e.Cancel = true;
                                return;
                            }
                        //}
                    }
                }
                dgvMenu.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }

        private void dgvTran_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (AddMode == true || EditMode == true)
            {
                foreach (DataGridViewRow row in dgvTran.Rows)
                {
                    if (row.Index != e.RowIndex & !row.IsNewRow)
                    {
                        //if (dgvTran.CurrentRow.Cells["entry_ty"].Value.ToString().ToUpper().Trim() != string.Empty && dgvTran.CurrentRow.Cells["code_nm"].Value.ToString().ToUpper().Trim() != string.Empty)
                        //{
                            if (row.Cells["entry_ty"].Value.ToString().ToUpper().Trim() == dgvTran.CurrentRow.Cells["entry_ty"].Value.ToString().ToUpper().Trim() && row.Cells["code_nm"].Value.ToString().ToUpper().Trim() == dgvTran.CurrentRow.Cells["code_nm"].Value.ToString().ToUpper().Trim())
                            {
                                dgvTran.Rows[e.RowIndex].ErrorText = "Duplicate transaction not allowed";
                                e.Cancel = true;
                                return;
                            }
                        //}
                        //if (dgvTran.CurrentRow.Cells["entry_ty"].Value.ToString().ToUpper().Trim() != string.Empty)
                        //{
                        //    if (dgvTran.CurrentRow.Cells["entry_ty"].Value.ToString().ToUpper().Trim().StartsWith("X") || dgvTran.CurrentRow.Cells["entry_ty"].Value.ToString().ToUpper().Trim().StartsWith("Y") || dgvTran.CurrentRow.Cells["entry_ty"].Value.ToString().ToUpper().Trim().StartsWith("Z"))
                        //    { }
                        //    else
                        //    {
                        //        DialogResult x;
                        //        x = MessageBox.Show("Want to continue with the transaction type '" + dgvTran.CurrentRow.Cells["entry_ty"].Value.ToString().ToUpper().Trim() + "' ?", m_obj_CustModAccUI.Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        //        if (x == DialogResult.No)
                        //        {
                        //            //dgvTran.Rows[e.RowIndex].ErrorText = "Invalid Transaction Type";
                        //            e.Cancel = true;
                        //            return;
                        //        }
                        //    }
                        //}
                    }
                    //if (row.Index == 0 && e.RowIndex == 0 && !row.IsNewRow)
                    //{
                    //    if (dgvTran.CurrentRow.Cells["entry_ty"].Value.ToString().ToUpper().Trim() != string.Empty)
                    //    {
                    //        if (dgvTran.CurrentRow.Cells["entry_ty"].Value.ToString().ToUpper().Trim().StartsWith("X") || dgvTran.CurrentRow.Cells["entry_ty"].Value.ToString().ToUpper().Trim().StartsWith("Y") || dgvTran.CurrentRow.Cells["entry_ty"].Value.ToString().ToUpper().Trim().StartsWith("Z"))
                    //        { }
                    //        else
                    //        {
                    //            DialogResult x;
                    //            x = MessageBox.Show("Want to continue with the transaction type '" + dgvTran.CurrentRow.Cells["entry_ty"].Value.ToString().ToUpper().Trim() + "' ?", m_obj_CustModAccUI.Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //            if (x == DialogResult.No)
                    //            {
                    //               // dgvTran.Rows[e.RowIndex].ErrorText = "Invalid Transaction Type";
                    //                e.Cancel = true;
                    //                return;
                    //            }
                    //        }
                    //    }
                    //}
                }
                dgvTran.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }

        private void dgvReport_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (AddMode == true || EditMode == true)
            {
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (row.Index != e.RowIndex & !row.IsNewRow)
                    {
                        //if (dgvReport.CurrentRow.Cells["group"].Value.ToString().ToUpper().Trim() != string.Empty && dgvReport.CurrentRow.Cells["desc"].Value.ToString().ToUpper().Trim() != string.Empty && dgvReport.CurrentRow.Cells["rep_nm"].Value.ToString().ToUpper().Trim() != string.Empty)
                        //{
                            if (row.Cells["group"].Value.ToString().ToUpper().Trim() == dgvReport.CurrentRow.Cells["group"].Value.ToString().ToUpper().Trim() && row.Cells["desc"].Value.ToString().ToUpper().Trim() == dgvReport.CurrentRow.Cells["desc"].Value.ToString().ToUpper().Trim() && row.Cells["rep_nm"].Value.ToString().ToUpper().Trim() == dgvReport.CurrentRow.Cells["rep_nm"].Value.ToString().ToUpper().Trim())
                            {
                                dgvReport.Rows[e.RowIndex].ErrorText = "Duplicate report not allowed";
                                e.Cancel = true;
                                return;
                            }
                        //}
                    }
                }
                dgvReport.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }

        private void frmMenuTranRept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O)
            {
                btnDone.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.Z)
            {
                btnCancel.PerformClick();
            }
        }

        private void dgvMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.A)
            {
                btn_mnuAdd.PerformClick();
            }
            if (e.Alt && e.KeyCode == Keys.D)
            {
                btn_mnuDelete.PerformClick();
            }
        }

        private void dgvTran_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.A)
            {
               btn_tranAdd.PerformClick();
            }
            if (e.Alt && e.KeyCode == Keys.D)
            {
                btn_tranDelete.PerformClick();
            }
        }

        private void dgvReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.A)
            {
                btn_rptAdd.PerformClick();
            }
            if (e.Alt && e.KeyCode == Keys.D)
            {
                btn_rptDelete.PerformClick();
            }
        }
       
        private void dgvMenu_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox)
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
        }

        private void dgvTran_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox)
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
        }

        private void dgvReport_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox)
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
        }
    }
}
