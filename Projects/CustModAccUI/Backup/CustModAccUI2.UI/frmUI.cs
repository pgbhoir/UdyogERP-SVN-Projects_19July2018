using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Web;
using System.Diagnostics;

using CustModAccUI.BLL;

namespace CustModAccUI.UI
{
    public partial class frmUI : Form
    {
        #region Object Creations
        cls_Gen_Mgr_Read_XML m_obj_Read_XML = new cls_Gen_Mgr_Read_XML();
        cls_Gen_Mgr_Upload_Script m_obj_Upload_Script;
        cls_Gen_Mgr_CustModAccUI m_obj_CustModAccUI;
        AutoCompleteStringCollection autocomplete = new AutoCompleteStringCollection();
        cls_Gen_Mgr_ConnectVFP m_obj_connectVFP;
        #endregion

        #region Variable declaration
        private bool AddMode = false;
        private bool EditMode = false;
        string strRights = string.Empty, strAdd = string.Empty, strEdit = string.Empty, strDelete = string.Empty, strPrint = string.Empty, strView = string.Empty;
        bool isClicked = false;
        int cntClick = 0;
        #endregion

        private DataSet dsMain;

        public DataSet DsMain
        {
            get { return dsMain; }
            set { dsMain = value; }
        }

        private DataSet dsSearch;

        public DataSet DsSearch
        {
            get { return dsSearch; }
            set { dsSearch = value; }
        }

        public frmUI(string connString, string vfpconnString, string username, int range, string APath, string IcoPath, string Vumess)
        {
            //m_obj_Upload_Script = new cls_Gen_Mgr_Upload_Script(connString);  //Commented by Priyanka on 31122013
            m_obj_Upload_Script = new cls_Gen_Mgr_Upload_Script(connString, Vumess);  //Added by Priyanka on 31122013
            m_obj_CustModAccUI = new cls_Gen_Mgr_CustModAccUI(connString, username, range);
            m_obj_connectVFP = new cls_Gen_Mgr_ConnectVFP(vfpconnString);
            m_obj_CustModAccUI.APath = APath.ToString().Trim();
            m_obj_CustModAccUI.IcoPath = IcoPath.ToString().Trim();
            m_obj_CustModAccUI.Vumess = Vumess.ToString().Trim();
            InitializeComponent();
            Icon ico = new Icon(IcoPath);
            this.Icon = ico;
        }

        #region Other Methods
        private void BindData()
        {
            txt_id.Text = m_obj_CustModAccUI.Id;
            dtp_date.Value = m_obj_CustModAccUI.Date;
            txt_rcomp.Text = m_obj_CustModAccUI.Rcomp;
            txt_prodname.Text = m_obj_CustModAccUI.Prodname;
            txt_prodver.Text = m_obj_CustModAccUI.Prodver;
            txt_rmacid.Text = m_obj_CustModAccUI.Rmacid;            
            txt_bug.Text = m_obj_CustModAccUI.Bug;
            txt_pono.Text = m_obj_CustModAccUI.Pono;
            dtp_podate.Value = m_obj_CustModAccUI.Podate;
            txt_poamt.Text = m_obj_CustModAccUI.Poamt.ToString();
            txt_apprby.Text = m_obj_CustModAccUI.Apprby;
            txt_remarks.Text = m_obj_CustModAccUI.Remarks;
        }

        private void EnableDisableFields(bool value)
        {
            dtp_date.Enabled = !value;
            txt_pono.ReadOnly = value;
            txt_poamt.ReadOnly = value;
            dtp_podate.Enabled = !value;
            txt_bug.ReadOnly = value;
            txt_apprby.ReadOnly = value;
            txt_remarks.ReadOnly = value;
            btn_uploadxmlpath.Enabled = !value;
            btn_uploadscript.Enabled = !value;
            btnGenerateExe.Enabled = !value;
            dgvCompany.ReadOnly = value;
            foreach (DataGridViewColumn dcol in dgvCompany.Columns)
            {
                if (dcol.Index.Equals(0))
                    dcol.ReadOnly = false;
                else
                    dcol.ReadOnly = true;
            }
        }

        private void CtrlBackColor()
        {
            txt_pono.BackColor = Color.White;
            txt_poamt.BackColor = Color.White;
            txt_bug.BackColor = Color.White;
            txt_apprby.BackColor = Color.White;
            txt_remarks.BackColor = Color.White;

            groupBox3.ForeColor = Color.DarkOrchid;
            foreach (Control ctl in groupBox3.Controls)
                ctl.ForeColor = SystemColors.ControlText;

            groupBox4.ForeColor = Color.DarkOrchid;
            foreach (Control ctl in groupBox4.Controls)
                ctl.ForeColor = SystemColors.ControlText;
            btn_uploadscript.ForeColor = Color.DeepSkyBlue;
        }

        private void BindXMLData()
        {
            txt_rcomp.Text = m_obj_Read_XML.Regcomp;
            txt_prodname.Text = m_obj_Read_XML.Prodname;
            txt_prodver.Text = m_obj_Read_XML.Prodver;
            txt_rmacid.Text = m_obj_Read_XML.Macid;
        }

        private void ClearFields()
        {
            //txt_id.Clear();
            dtp_date.Value = DateTime.Now;
            txt_rcomp.Clear();
            txt_prodname.Clear();
            txt_prodver.Clear();
            txt_rmacid.Clear();
            txt_bug.Clear();
            txt_pono.Clear();
            dtp_podate.Value = DateTime.Now;
            txt_poamt.Clear();
            txt_apprby.Clear();
            txt_remarks.Clear();
        }

        private void Binding()
        {
            m_obj_CustModAccUI.Id = txt_id.Text.ToString().Trim();
            m_obj_CustModAccUI.Date = dtp_date.Value;
            m_obj_CustModAccUI.Rcomp = txt_rcomp.Text.ToString().Trim();
            m_obj_CustModAccUI.Prodname = txt_prodname.Text.ToString().Trim();
            m_obj_CustModAccUI.Prodver = txt_prodver.Text.ToString().Trim();
            m_obj_CustModAccUI.Rmacid = txt_rmacid.Text.ToString().Trim();            
            m_obj_CustModAccUI.Bug = txt_bug.Text.ToString().Trim();
            m_obj_CustModAccUI.Pono = txt_pono.Text.ToString().Trim();
            m_obj_CustModAccUI.Podate = dtp_podate.Value;
            m_obj_CustModAccUI.Poamt = txt_poamt.Text == string.Empty ? Convert.ToDecimal(0.00) : Convert.ToDecimal(txt_poamt.Text);
            m_obj_CustModAccUI.Apprby = txt_apprby.Text.ToString().Trim();
            m_obj_CustModAccUI.Remarks = txt_remarks.Text.ToString().Trim();
            if (dgvCompany.Rows.Count > 0)
                m_obj_CustModAccUI.DsDetail = m_obj_CustModAccUI.DsMain;
            else
                m_obj_CustModAccUI.DsDetail = null;
        }

        private bool Validations()
        {
            bool isValid = true;

            if (txt_id.Text == string.Empty)
            {
                errorProvider1.SetError(txt_id, lbl_id.Text.ToString().Trim() + " cannot be blank.");
                isValid = false;
            }
            if (dtp_date.Text == string.Empty)
            {
                errorProvider1.SetError(dtp_date, lbl_date.Text.ToString().Trim() + " cannot be blank.");
                isValid = false;
            }            
            if (txt_rcomp.Text == string.Empty)
            {
                errorProvider1.SetError(txt_rcomp, lbl_rcomp.Text.ToString().Trim() + " cannot be blank.");
                isValid = false;
            }
            if (txt_rmacid.Text == string.Empty)
            {
                errorProvider1.SetError(txt_rmacid, lbl_rmacid.Text.ToString().Trim() + " cannot be blank.");
                isValid = false;
            }
            if (txt_prodname.Text == string.Empty)
            {
                errorProvider1.SetError(txt_prodname, lbl_prodname.Text.ToString().Trim() + " cannot be blank.");
                isValid = false;
            }
            if (txt_prodver.Text == string.Empty)
            {
                errorProvider1.SetError(txt_prodver, lbl_prodver.Text.ToString().Trim() + " cannot be blank.");
                isValid = false;
            }
            if (dgvCompany.Rows.Count == 0)
            {
                errorProvider1.SetError(dgvCompany, groupBox3.Text.ToString().Trim() + " cannot be blank.");
                isValid = false;
            }
            return isValid;
        }

        private void UserRights(string strRights)
        {
            string strChar = string.Empty;
            int charindex = 0;
            while (strRights.Length > charindex)
            {
                
                strChar = strRights.Substring(charindex, 2);

                switch (strChar)
                {
                    case "IY":
                        tlsbtnAdd.Enabled = true;
                        tlsbtnCopy.Enabled = true;
                        strAdd = "ADD";
                        break;
                    case "CY":
                        tlsbtnEdit.Enabled = true;
                        strEdit = "EDIT";
                        break;
                    case "DY":
                        tlsbtnDelete.Enabled = true;
                        strDelete = "DELETE";
                        break;
                    case "PY":
                        btnGenerateExe.Enabled = true;
                        strPrint = "PRINT";
                        break;
                    case "VY":
                        strView = "VIEW";
                        break;
                    default:
                        if (charindex == 0 && strRights.Substring(charindex, 2) == "  ")
                        {
                            tlsbtnAdd.Enabled = false;
                            tlsbtnCopy.Enabled = false;
                            strAdd = string.Empty;
                        }
                        if (charindex == 2 && strRights.Substring(charindex, 2) == "  ")
                        {
                            tlsbtnEdit.Enabled = false;
                            strEdit = string.Empty;
                        }
                        if (charindex == 4 && strRights.Substring(charindex, 2) == "  ")
                        {
                            tlsbtnDelete.Enabled = false;
                            strDelete = string.Empty;
                        }
                        if (charindex == 6 && strRights.Substring(charindex, 2) == "  ")
                        {
                            btnGenerateExe.Enabled = false;
                            strPrint = string.Empty;
                        }
                        if (charindex == 8 && strRights.Substring(charindex, 2) == "  ") 
                        {
                            strView = string.Empty;
                        }
                        break;
                }
                charindex += 2;
            }
        }

        private void EnableDisableBtns(bool value)
        {
            tlsbtnAdd.Enabled = value;
            tlsbtnEdit.Enabled = value;
            tlsbtnDelete.Enabled = value;
            tlsbtnCopy.Enabled = value;
            tlsbtnSave.Enabled = value;
            tlsbtnCancel.Enabled = value;
            tlsbtnSearch.Enabled = value;
            tlsbtnExit.Enabled = value;
        }
        #endregion

        private void frmUI_Load(object sender, EventArgs e)
        {
            m_obj_CustModAccUI.SelectUserRights();
            DsSearch = m_obj_CustModAccUI.DsSearch;
            if(DsSearch.Tables[0].Rows.Count > 0)
                strRights = m_obj_CustModAccUI.DsSearch.Tables[0].Rows[0]["Rights"].ToString();
            UserRights(strRights);
            EnableDisableBtns(false);
            if (string.IsNullOrEmpty(strAdd))
            {
                tlsbtnAdd.Enabled = false;
                //tlsbtnCopy.Enabled = false;
            }
            else
            {
                tlsbtnAdd.Enabled = true;
                //tlsbtnCopy.Enabled = true;
            }
            tlsbtnSearch.Enabled = true;
            tlsbtnExit.Enabled = true;
            EnableDisableFields(true);
            ClearFields();
            CtrlBackColor();
            //txt_apprby.AutoCompleteCustomSource = autocomplete;
            chkSelectAll.Tag = "";
        }

        private void tlsbtnEdit_Click(object sender, EventArgs e)
        {
            AddMode = false;
            EditMode = true;
            UserRights(strRights);
            EnableDisableBtns(false);
            tlsbtnSave.Enabled = true;
            tlsbtnCancel.Enabled = true;
            EnableDisableFields(false);
            btn_uploadxmlpath.Enabled = false;
            btnGenerateExe.Enabled = false;

            chkSelectAll.Enabled = true;
            chkSelectAll.Checked = false;

            chkSelectAll.Tag = "";
        }        

        private void btn_uploadxmlpath_Click(object sender, EventArgs e)
        {
            try
            {
                string currpath = System.Environment.CurrentDirectory.ToString();
                //MessageBox.Show(System.Environment.CurrentDirectory, "Before");
                //MessageBox.Show(m_obj_CustModAccUI.APath, "Before");
                openFD.Title = "Select XML";
                openFD.FileName = "";
                openFD.Filter = "XML|*.xml";

                if (openFD.ShowDialog() == DialogResult.Cancel)
                    return;
                else
                {
                    System.Environment.CurrentDirectory = currpath;
                    //MessageBox.Show(System.Environment.CurrentDirectory, "After");
                    //MessageBox.Show(m_obj_CustModAccUI.APath, "After");
                    m_obj_Read_XML.Xmlpath = openFD.FileName;
                    m_obj_Read_XML.DsMain = m_obj_CustModAccUI.DsMain;
                    if (m_obj_Read_XML.DsMain.Tables["detailtbl"].Rows.Count > 0)
                    {
                        DialogResult res = MessageBox.Show("XML Data already found!!\nWant to Clear the existing data and Upload it again?", m_obj_CustModAccUI.Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (res == DialogResult.Yes)
                            m_obj_CustModAccUI.DsMain.Tables["detailtbl"].Rows.Clear();
                        else
                            return;
                    }
                    m_obj_Read_XML.Readxml();
                    m_obj_CustModAccUI.DsMain = m_obj_Read_XML.DsMain;
                    ClearFields();
                    BindXMLData();
                    dgvCompany.AutoGenerateColumns = false;
                    dgvCompany.DataMember = m_obj_CustModAccUI.DsMain.Tables["detailtbl"].TableName;
                    dgvCompany.DataSource = m_obj_CustModAccUI.DsMain;

                    foreach (DataGridViewColumn dcol in dgvCompany.Columns)
                    {
                        if (dcol.Index.Equals(0))
                            dcol.ReadOnly = false;
                        else
                            dcol.ReadOnly = true;
                    }
                    chkSelectAll.Enabled = true;
                    chkSelectAll.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }       

        private void btn_uploadscript_Click(object sender, EventArgs e)
        {
            string currpath = System.Environment.CurrentDirectory.ToString();
            //MessageBox.Show(System.Environment.CurrentDirectory, "Before");
            //MessageBox.Show(m_obj_CustModAccUI.APath, "Before");
            bool isChecked = false;
            m_obj_Upload_Script.Ccomp = "";
            try
            {
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    if ((bool)dgvCompany.Rows[i].Cells[0].Value == true)
                    {
                        m_obj_Upload_Script.Ccomp = m_obj_Upload_Script.Ccomp == string.Empty || m_obj_Upload_Script.Ccomp == null ? dgvCompany.Rows[i].Cells[2].Value.ToString() : m_obj_Upload_Script.Ccomp + "," + dgvCompany.Rows[i].Cells[2].Value.ToString();
                        isChecked = true;
                    }
                }
                if (!isChecked)
                {
                    MessageBox.Show("Please select the company from the company list for which you want to upload the script!!", m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                openFD.Title = "Select SQL Script File...";
                openFD.Multiselect = true;
                openFD.FileName = "";
                openFD.Filter = "SQL|*.sql";

                if (openFD.ShowDialog() == DialogResult.Cancel)
                    return;
                else
                {
                    System.Environment.CurrentDirectory = currpath;
                    //MessageBox.Show(System.Environment.CurrentDirectory, "After");
                    //MessageBox.Show(m_obj_CustModAccUI.APath, "After");
                    string oldscript = "", newscript = "", sql_script = "", sel_script = "", warmsg = "", newscript1 = "";
                    oldscript = " SELECT * INTO #CUSTCOMMENU FROM COM_MENU WHERE 1=2 SELECT * INTO #CUSTTRANCODE FROM LCODE WHERE 1=2 SELECT * INTO #CUSTRSTATUS FROM R_STATUS WHERE 1=2 ";
                    foreach (string filename in openFD.FileNames)
                    {
                        newscript = newscript + " " + m_obj_Read_XML.ReadSQLFile(filename);
                        if (!string.IsNullOrEmpty(m_obj_Read_XML.WarMsg))
                        //|| string.IsNullOrEmpty(newscript.Trim()))
                        {
                            if (!string.IsNullOrEmpty(m_obj_Read_XML.WarMsg))
                            {
                                warmsg = !string.IsNullOrEmpty(warmsg)
                                    ? warmsg + "\nWarning : Please check the " + filename.Trim() + " file" + "\n\n" + m_obj_Read_XML.WarMsg
                                    : "Warning : Please check the " + filename.Trim() + " file" + "\n\n" + m_obj_Read_XML.WarMsg;
                                newscript = "";
                            }
                        }
                        else
                        {
                            newscript1 = newscript1 + " " + newscript;
                        }
                    }
                    
                    sql_script = oldscript + " " + newscript1;
                    sel_script = " SELECT * FROM #CUSTCOMMENU SELECT * FROM #CUSTTRANCODE SELECT * FROM #CUSTRSTATUS ";
                    sql_script = sql_script + sel_script;
                    m_obj_Upload_Script.DsMain = m_obj_CustModAccUI.DsMain;
                    m_obj_Upload_Script.Id = m_obj_CustModAccUI.Id;
                    m_obj_Upload_Script.Select(sql_script);
                    if (!string.IsNullOrEmpty(warmsg))
                    {
                        MessageBox.Show(warmsg.Trim(), m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //return;
                    }
                    //Commented by Priyanka on 31122013 start
                    //if (!string.IsNullOrEmpty(m_obj_Upload_Script.WarMsg))
                      //  MessageBox.Show(m_obj_Upload_Script.WarMsg.ToString().Trim(), m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //Commented by Priyanka on 31122013 end
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void tlsbtnExit_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure to exit?", m_obj_CustModAccUI.Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (res == DialogResult.Yes)
            {
                Dispose(true);
                Close();
            }
        }        

        private void tlsbtnAdd_Click(object sender, EventArgs e)
        {
            AddMode = true;
            EditMode = false;
            UserRights(strRights);
            EnableDisableBtns(false);
            //tlsbtnAdd.Enabled = false;
            //tlsbtnEdit.Enabled = false;
            //tlsbtnDelete.Enabled = false;
            tlsbtnSave.Enabled = true;
            tlsbtnCancel.Enabled = true;
            //tlsbtnExit.Enabled = false;
            //tlsbtnSearch.Enabled = false;
            //tlsbtnCopy.Enabled = false;
            ClearFields();
            dgvCompany.DataSource = null;
            EnableDisableFields(false);
            btnGenerateExe.Enabled = false;
            m_obj_CustModAccUI.GenerateDb();
            txt_id.Text = m_obj_CustModAccUI.Id.ToString().Trim();
            txt_id.Focus();

            chkSelectAll.Tag = "";
        }
       
        private void tlsbtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure to delete this entry?", m_obj_CustModAccUI.Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (res == DialogResult.Yes)
            {
                try
                {
                    m_obj_CustModAccUI.Delete();
                    MessageBox.Show("Entry Deleted", m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    AddMode = false;
                    EditMode = false;
                    UserRights(strRights);
                    EnableDisableBtns(false);
                    if (string.IsNullOrEmpty(strAdd))
                    {
                        tlsbtnAdd.Enabled = false;
                        //tlsbtnCopy.Enabled = false;
                    }
                    else
                    {
                        tlsbtnAdd.Enabled = true;
                        //tlsbtnCopy.Enabled = true;
                    }

                    //tlsbtnAdd.Enabled = true;
                    //tlsbtnEdit.Enabled = false;
                    //tlsbtnDelete.Enabled = false;
                    //tlsbtnSave.Enabled = false;
                    //tlsbtnCancel.Enabled = false;
                    tlsbtnExit.Enabled = true;
                    tlsbtnSearch.Enabled = true;
                    //tlsbtnCopy.Enabled = false;
                    txt_id.Clear();
                    dgvCompany.DataSource = null;
                    ClearFields();
                    EnableDisableFields(true);
                    errorProvider1.Clear();

                    chkSelectAll.Enabled = false;
                    chkSelectAll.Checked = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Entry cannot be Deleted", m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void tlsbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider1.Clear();
                if (Validations() == false)
                    return;
                errorProvider1.Clear();

                Binding();

                if (AddMode == true)
                    m_obj_CustModAccUI.Insert();
                else
                    if (EditMode == true)
                        m_obj_CustModAccUI.Update();

                MessageBox.Show("Entry Saved", m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                if (AddMode == true)
                    m_obj_CustModAccUI.DsMain.Dispose();

                AddMode = false;
                EditMode = false;
                UserRights(strRights);
                EnableDisableBtns(false);
                if (string.IsNullOrEmpty(strAdd))
                {
                    tlsbtnAdd.Enabled = false;
                    //tlsbtnCopy.Enabled = false;
                }
                else
                {
                    tlsbtnAdd.Enabled = true;
                    //tlsbtnCopy.Enabled = true;
                }
                //if (string.IsNullOrEmpty(strEdit))
                //    tlsbtnEdit.Enabled = false;
                //else
                //    tlsbtnEdit.Enabled = true;
                //if (string.IsNullOrEmpty(strDelete))
                //    tlsbtnDelete.Enabled = false;
                //else
                //    tlsbtnDelete.Enabled = true;
                //if (string.IsNullOrEmpty(strPrint))
                //    btnGenerateExe.Enabled = false;
                //else
                //    btnGenerateExe.Enabled = true;

                //tlsbtnAdd.Enabled = true;
                //tlsbtnEdit.Enabled = false;
                //tlsbtnDelete.Enabled = false;
                //tlsbtnSave.Enabled = false;
                //tlsbtnCancel.Enabled = false;
                tlsbtnExit.Enabled = true;
                tlsbtnSearch.Enabled = true;
                //tlsbtnCopy.Enabled = false;
                txt_id.Clear();
                dgvCompany.DataSource = null;
                ClearFields();
                EnableDisableFields(true);
                errorProvider1.Clear();
                txt_id.Focus();

                chkSelectAll.Enabled = false;
                chkSelectAll.Checked = false;
                chkSelectAll.Text = "Select All";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Entry cannot be Saved", m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void tlsbtnCancel_Click(object sender, EventArgs e)
        {
            AddMode = false;
            EditMode = false;
            UserRights(strRights);
            EnableDisableBtns(false);
            if (string.IsNullOrEmpty(strAdd))
            {
                tlsbtnAdd.Enabled = false;
                //tlsbtnCopy.Enabled = false;
            }
            else
            {
                tlsbtnAdd.Enabled = true;
                //tlsbtnCopy.Enabled = true;
            }
            //if (string.IsNullOrEmpty(strEdit))
            //    tlsbtnEdit.Enabled = false;
            //else
            //    tlsbtnEdit.Enabled = true;
            //if (string.IsNullOrEmpty(strDelete))
            //    tlsbtnDelete.Enabled = false;
            //else
            //    tlsbtnDelete.Enabled = true;
            //if (string.IsNullOrEmpty(strPrint))
            //    btnGenerateExe.Enabled = false;
            //else
            //    btnGenerateExe.Enabled = true;
            //tlsbtnAdd.Enabled = true;
            //tlsbtnEdit.Enabled = false;
            //tlsbtnDelete.Enabled = false;
            //tlsbtnSave.Enabled = false;
            //tlsbtnCancel.Enabled = false;
            tlsbtnExit.Enabled = true;
            tlsbtnSearch.Enabled = true;
            //tlsbtnCopy.Enabled = false;
            txt_id.Clear();
            dgvCompany.DataSource = null;
            ClearFields();
            EnableDisableFields(true);
            errorProvider1.Clear();
            txt_id.Focus();

            chkSelectAll.Enabled = false;
            chkSelectAll.Checked = false;
            chkSelectAll.Text = "Select All";
        }

        private void tlsbtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                AddMode = false;
                EditMode = false;
                m_obj_CustModAccUI.SearchById();
                frmSearch m_obj_frmsearch = new frmSearch(m_obj_CustModAccUI.ConnString,m_obj_CustModAccUI.User_name,m_obj_CustModAccUI.Range);
                //m_obj_frmsearch.SeachValidate(m_obj_CustModAccUI.DsSearch.Tables[0], "Id+Rcomp", "Id", "Id:Id,Rcomp:Registered Company");
                string col = "";
                foreach (DataColumn dcol in m_obj_CustModAccUI.DsSearch.Tables[0].Columns)
                    col = col == string.Empty ? "[" + dcol.ColumnName.ToString().Trim() + "]" : col + "+[" + dcol.ColumnName.ToString().Trim() + "]";
                m_obj_frmsearch.SeachValidate(m_obj_CustModAccUI.DsSearch.Tables[0], col, "Id", "");
                if (m_obj_CustModAccUI.DsSearch.Tables[0].Rows.Count > 0)
                {
                    m_obj_frmsearch.ShowDialog();
                    if (m_obj_frmsearch.DialogResult == DialogResult.OK || m_obj_frmsearch.oSelectedRow != null)
                    {
                        m_obj_CustModAccUI.Id = m_obj_frmsearch.oSelectedRow == null ? "" : (string)m_obj_frmsearch.oSelectedRow["Id"];
                        if (m_obj_CustModAccUI.Id == string.Empty)
                            return;

                        m_obj_CustModAccUI.Select();

                        ClearFields();
                        BindData();
                        dgvCompany.AutoGenerateColumns = false;
                        dgvCompany.Columns[2].DataPropertyName = "ccomp";

                        dgvCompany.DataSource = m_obj_CustModAccUI.DsMain.Tables["detailtbl"];
                        for (int i = 0; i < m_obj_CustModAccUI.DsMain.Tables["detailtbl"].Rows.Count; i++)
                            dgvCompany.Rows[i].Cells[1].Value = i + 1;

                        UserRights(strRights);
                        EnableDisableBtns(true);
                        EnableDisableFields(true);
                        if (string.IsNullOrEmpty(strAdd))
                        {
                            tlsbtnAdd.Enabled = false;
                            tlsbtnCopy.Enabled = false;
                        }
                        else
                        {
                            tlsbtnAdd.Enabled = true;
                            tlsbtnCopy.Enabled = true;
                        }
                        if (string.IsNullOrEmpty(strEdit))
                            tlsbtnEdit.Enabled = false;
                        else
                            tlsbtnEdit.Enabled = true;
                        if (string.IsNullOrEmpty(strDelete))
                            tlsbtnDelete.Enabled = false;
                        else
                            tlsbtnDelete.Enabled = true;
                        if (string.IsNullOrEmpty(strPrint))
                            btnGenerateExe.Enabled = false;
                        else
                            btnGenerateExe.Enabled = true;
                        //tlsbtnAdd.Enabled = true;
                        //tlsbtnEdit.Enabled = true;
                        //tlsbtnDelete.Enabled = true;
                        tlsbtnSave.Enabled = false;
                        tlsbtnCancel.Enabled = false;
                        //tlsbtnCopy.Enabled = true;
                        //btnGenerateExe.Enabled = true;
                        txt_id.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("No Records found!!", m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void dgvCompany_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCompany.Columns[e.ColumnIndex].Name.ToString().ToUpper() == "SHOW" && dgvCompany.CurrentCell is DataGridViewLinkCell)
            {
                frmMenuTranRept m_MenuTranRept = new frmMenuTranRept(dgvCompany.CurrentRow.Cells[2].Value.ToString().Trim(),m_obj_CustModAccUI.ConnString,m_obj_CustModAccUI.User_name,m_obj_CustModAccUI.Range,m_obj_CustModAccUI.IcoPath,m_obj_CustModAccUI.Vumess);
                m_MenuTranRept.DsMain = m_obj_CustModAccUI.DsMain;
                m_MenuTranRept.Id = m_obj_CustModAccUI.Id.ToString().Trim();
                m_MenuTranRept.Ccomp = dgvCompany.CurrentRow.Cells[2].Value.ToString().Trim();
                m_MenuTranRept.AddMode = AddMode;
                m_MenuTranRept.EditMode = EditMode;
                m_MenuTranRept.ShowDialog();
            }

            if (dgvCompany.Columns[e.ColumnIndex].Name.ToString().ToUpper() == "SELECT" && dgvCompany.CurrentCell is DataGridViewCheckBoxCell)
            {
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    if ((bool)dgvCompany.Rows[i].Cells[0].EditedFormattedValue)
                        cntClick++;
                }
                if (cntClick == dgvCompany.Rows.Count)
                {
                    chkSelectAll.Checked = true;
                    cntClick = 0;
                }
                else
                {
                    chkSelectAll.Tag = "AUTO";
                    chkSelectAll.Checked = false;
                    cntClick = 0;
                }
            }
        }

        //private void txt_apprby_Leave(object sender, EventArgs e)
        //{
        //    if(txt_apprby.ReadOnly == false)
        //        autocomplete.Add(txt_apprby.Text.ToString().Trim());
        //}

        private void frmUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.A)
                tlsbtnAdd.PerformClick();
            if (e.Alt && e.KeyCode == Keys.E)
                tlsbtnEdit.PerformClick();
            if (e.Alt && e.KeyCode == Keys.D)
                tlsbtnDelete.PerformClick();
            if (e.Alt && e.KeyCode == Keys.C)
                tlsbtnCopy.PerformClick();
            if (e.Alt && e.KeyCode == Keys.S)
                tlsbtnSave.PerformClick();
            if (e.Alt && e.KeyCode == Keys.Z)
                tlsbtnCancel.PerformClick();
            if (e.Alt && e.KeyCode == Keys.F4)
                tlsbtnExit.PerformClick();
            if (e.KeyCode == Keys.F2)
                tlsbtnSearch.PerformClick();
            if (e.Alt && e.KeyCode == Keys.G)
                btnGenerateExe.PerformClick();
        }

        private void tlsbtnCopy_Click(object sender, EventArgs e)
        {
            AddMode = true;
            EditMode = false;
            UserRights(strRights);
            EnableDisableBtns(false);
            //tlsbtnAdd.Enabled = false;
            //tlsbtnEdit.Enabled = false;
            //tlsbtnDelete.Enabled = false;
            tlsbtnSave.Enabled = true;
            tlsbtnCancel.Enabled = true;
            //tlsbtnExit.Enabled = false;
            //tlsbtnSearch.Enabled = false;
            //tlsbtnCopy.Enabled = false;
            EnableDisableFields(false);
            btnGenerateExe.Enabled = false;
            DsMain = m_obj_CustModAccUI.DsMain;
            m_obj_CustModAccUI.GenerateDb();
            txt_id.Text = m_obj_CustModAccUI.Id.ToString().Trim();
            foreach (DataRow dr in DsMain.Tables["maintbl"].Rows)
            {
                dr["id"] = txt_id.Text.ToString().Trim();
                DsMain.Tables["maintbl"].AcceptChanges();
            }
            foreach (DataRow dr in DsMain.Tables["subdetailtbl"].Rows)
            {
                dr["fk_id"] = txt_id.Text.ToString().Trim();
                DsMain.Tables["subdetailtbl"].AcceptChanges();
            }
            BindData();
            m_obj_CustModAccUI.DsMain = DsMain;

            chkSelectAll.Enabled = true;
            chkSelectAll.Checked = false;

            chkSelectAll.Tag = "";
        }

        private void btnGenerateExe_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            Process p = new Process();
            try
            {
                btnGenerateExe.Enabled = false;
                btnGenerateExe.Refresh();
                //string currentPath = System.Environment.CurrentDirectory;
                //if (!Directory.Exists(currentPath + "\\UpdtExe\\"))
                    //throw new Exception("UpdtExe Project Folder does not exists");
                //MessageBox.Show(System.Environment.CurrentDirectory, "Before");
                //MessageBox.Show(m_obj_CustModAccUI.APath, "Before");
                if (!Directory.Exists(m_obj_CustModAccUI.APath + "CustModAccUI\\UpdtExe"))
                    throw new Exception("UpdtExe Project Folder does not exists");

                m_obj_CustModAccUI.ConvertToDBF(m_obj_CustModAccUI.Id.ToString().Trim());
                m_obj_connectVFP.DsConvert = m_obj_CustModAccUI.DsConvert;
                m_obj_connectVFP.Insert();
                //MessageBox.Show("Records inserted successfully in dbf file.", m_obj_CustModAccUI.Vumess, MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (!File.Exists(m_obj_CustModAccUI.APath + "CustModAccUI\\custconfig.fpw"))
                    throw new Exception("custconfig.fpw file does not exists in " + m_obj_CustModAccUI.APath + " path");

                if (!File.Exists(m_obj_CustModAccUI.APath + "CustModAccUI\\uecustmodaccui.prg"))
                    throw new Exception("uecustmodaccui.prg file does not exists in " + m_obj_CustModAccUI.APath + " path");

                //MessageBox.Show(m_obj_CustModAccUI.APath);
                //psi.FileName = @"C:\Program Files\Microsoft Visual FoxPro 9\vfp9.exe";
                psi.FileName = "vfp9.exe";
                if (string.IsNullOrEmpty(txt_bug.Text.ToString().Trim()))
                    txt_bug.Text = "";
                //psi.Arguments = "/C /t /nologo /c" + m_obj_CustModAccUI.APath + "CustModAccUI\\custconfig.fpw " + m_obj_CustModAccUI.APath + "CustModAccUI\\uecustmodaccui.prg " + txt_bug.Text.ToString().Trim(); //Commented by Priyanka on 02012014
                psi.Arguments = "/C /t /nologo /c" + m_obj_CustModAccUI.APath + "CustModAccUI\\custconfig.fpw "
                    + m_obj_CustModAccUI.APath + "CustModAccUI\\uecustmodaccui.prg "
                    + txt_bug.Text.ToString().Trim()
                    + " " + m_obj_CustModAccUI.APath; //Added by Priyanka on 02012014
                //MessageBox.Show(System.Environment.CurrentDirectory, "After");
                //MessageBox.Show(m_obj_CustModAccUI.APath, "After");
                //psi.Arguments = "/C dir";
                psi.UseShellExecute = false;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.CreateNoWindow = true;
                psi.RedirectStandardError = true;
                psi.RedirectStandardOutput = true;
                p.StartInfo = psi;
                p.Start();
                //MessageBox.Show(p.StandardOutput.ReadToEnd());
                tsslbl_msg.Text = "Please wait, till the process gets completed and do not click any of the buttons....";
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                p.Close();
                tsslbl_msg.Text = "";
                btnGenerateExe.Enabled = true;
            }
        }

        private void frmUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult res = MessageBox.Show("Are you sure to exit?", m_obj_CustModAccUI.Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (res == DialogResult.Yes)
                    {
                        Dispose(true);
                        Application.Exit();
                    }
                    else
                        e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void txt_apprby_Enter(object sender, EventArgs e)
        {
            if (txt_apprby.ReadOnly == false)
            {
                m_obj_CustModAccUI.AutocompleteText();
                DsSearch = m_obj_CustModAccUI.DsSearch;
                foreach (DataRow dr in DsSearch.Tables[0].Rows)
                {
                    autocomplete.Add(dr["apprby"].ToString().Trim());
                }
            }
        }

        private void txt_apprby_TextChanged(object sender, EventArgs e)
        {
            txt_apprby.AutoCompleteCustomSource = autocomplete;
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvCompany.DataSource != null && dgvCompany.Rows.Count > 0)
            {
                if (chkSelectAll.Checked == true)
                {
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                        dgvCompany.Rows[i].Cells[0].Value = true;
                    chkSelectAll.Text = "DeSelect All";
                    chkSelectAll.Tag = "";
                }
                else if (chkSelectAll.Checked == false && chkSelectAll.Tag.ToString().ToUpper() != "AUTO")
                {
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                        dgvCompany.Rows[i].Cells[0].Value = false;
                    chkSelectAll.Text = "Select All";
                }
                else if (chkSelectAll.Checked == false && chkSelectAll.Tag.ToString().ToUpper() == "AUTO")
                {
                    chkSelectAll.Checked = false;
                    chkSelectAll.Text = "Select All";
                }
            }
        }

       

       
    }
}
