using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;

using MLA.LL;

namespace MLA.APPS
{
    public partial class MLAAppLevelSettings : Form
    {
        public MLAAppLevelSettings()
        {
            InitializeComponent();
        }

        #region Define variables and class

        public static string ConnectionString { get; set; }
        public string UserID { get; set; }
        public string IconPath { get; set; }
        public int CompId { get; set; }


        cls_Gen_Mgr_Settings m_obj_Settings = new cls_Gen_Mgr_Settings(ConnectionString);
        private DataView Dgv;
        private int RowId = 0;
        private string m_applevel = string.Empty;
        private string m_userrole = string.Empty;
        private string m_entrytype = string.Empty;
        private bool activeclick = false;
        private bool applevelInit = false;
        #endregion

        #region form events 
        private void MLAAppLevelSettings_Load(object sender, EventArgs e)
        {
            try
            {

                if (File.Exists(IconPath))
                {
                    Icon ico = new Icon(IconPath);
                    this.Icon = ico;
                }

                m_obj_Settings.UserID = UserID;
                m_obj_Settings.GetAllInitialData(CompId);

                GenerateTranListNodes();
               // GenerateUserRolesList();

                groupBox1.ForeColor = Color.Blue;
                foreach (Control ctrl in groupBox1.Controls)
                    ctrl.ForeColor = SystemColors.ControlText;

                groupBox2.ForeColor = Color.Purple;
                foreach (Control ctrl in groupBox2.Controls)
                    ctrl.ForeColor = SystemColors.ControlText;

            }
            catch (Exception Ex)
            {
                string error = "\n\n found error while collecting data  \n\n" +
                          "Message : " + Ex.Message +
                          "\nSource : " + Ex.Source +
                          "\nTargetSite : " + Ex.TargetSite;

                MessageBox.Show(error, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool iserror = false;
            errorProvider.Clear();

            if (cboAppLevel.SelectedIndex == 0)
            {
                errorProvider.SetError(cboAppLevel, "This field is mandatory");
                iserror = true;
            }

            if (lkeditUserRoles.EditValue == null || lkeditUserRoles.EditValue.ToString() == string.Empty)
            {
                errorProvider.SetError(lkeditUserRoles, "This field is mandatory");
                iserror = true;
            }
           
            if (iserror == true)
                return;

            if (btnUpdate.Tag.ToString().Trim() == "Add")
                AddLevel();
            else if (btnUpdate.Tag.ToString().Trim() == "Update")
                ModifyLevel();
            
            dtgrdvwAppLevel.Refresh();

            SetGridImages();

            GenerateLevelNodes();

            // Reset controls

            cboAppLevel.SelectedIndex = 0;
            lkeditUserRoles.EditValue = string.Empty;
          //  cboRoleUser.SelectedIndex = 0;
            chkPasslevel.Checked = false;

            btnCancel.PerformClick();
        }

        #endregion

        private void AddLevel()
        {
            var findrows = (from DataRow row in m_obj_Settings.DsApplevel_Mstr.Tables["Applevel_mstr"].Rows
                            where row["user_role"].ToString().Trim() == lkeditUserRoles.EditValue.ToString().Trim()
                            select row).ToList();

            if (findrows.Count() > 0)
            {
                MessageBox.Show("Role / User has already assigned to Approval Level, Cannot assign again to another Approval Level", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow[] dtrows = (from DataRow row in m_obj_Settings.DsApplevel_Mstr.Tables["Applevel_mstr"].Rows
                                where row["Applevel"].ToString().Trim() == cboAppLevel.SelectedItem.ToString().Trim()
                                select row).ToArray();

            bool isRole = false, isUser = false;

            foreach (DataRow dtapplevel in dtrows)
            {
                DataRow[] dtusermstrrows = null;
                if (Convert.ToString(dtapplevel["type"]).Trim().ToUpper() == "ROLE")
                {
                    dtusermstrrows = (from DataRow row in m_obj_Settings.DsSettings.Tables["User_mstr"].Rows
                                      where row["user"].ToString().Trim() == lkeditUserRoles.EditValue.ToString().Trim()
                                      select row).ToArray();
                }
                else
                {
                    if (Convert.ToString(dtapplevel["type"]).Trim().ToUpper() == "USER")
                    {
                        dtusermstrrows = (from DataRow row in m_obj_Settings.DsSettings.Tables["User_mstr"].Rows
                                          where row["user_roles"].ToString().Trim() == lkeditUserRoles.EditValue.ToString().Trim()
                                          select row).ToArray();
                    }
                }


                foreach (DataRow druser in dtusermstrrows)
                {
                    if (Convert.ToString(dtapplevel["type"]).Trim().ToUpper() == "ROLE")
                    {
                        if (Convert.ToString(dtapplevel["user_role"]).ToUpper().Trim() ==
                               Convert.ToString(druser["user_roles"]).ToUpper().Trim())
                        {
                            isRole = true;
                            break;
                        }
                    }
                    else
                    {
                        if (Convert.ToString(dtapplevel["type"]).Trim().ToUpper() == "USER")
                        {
                            if (Convert.ToString(dtapplevel["user_role"]).ToUpper().Trim() ==
                                   Convert.ToString(druser["user"]).ToUpper().Trim())
                            {
                                isUser = true;
                                break;
                            }
                        }
                    }
                }

                if (isRole == true)
                {
                    MessageBox.Show("User has already belong to this Role " + Convert.ToString(dtapplevel["user_role"]).ToUpper().Trim() + " and " +
                                    "this Role has already assigned to " + cboAppLevel.SelectedItem.ToString().Trim() + ", you cannot assign again \n\n" +
                                    "If you want assign this user, please remove assigned Role from the Approval Level and then try the same"
                            , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                else
                {
                    if (isUser == true)
                    {
                        MessageBox.Show(Convert.ToString(dtapplevel["user_role"]).ToUpper().Trim() + " user has already belong to this Role, and " +
                                    "User has already assigned to " + cboAppLevel.SelectedItem.ToString().Trim() + ", you cannot assign again \n\n" +
                                    "If you want assign this Role, please remove assigned User from the Approval Level and then try the same"
                            , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            }

            if (isRole == true || isUser == true)
                return;

            int maxid = Convert.ToInt32(m_obj_Settings.DsApplevel_Mstr.Tables["Applevel_mstr"].AsEnumerable()
                        .Max(row => row["id"]));

            DataRow dr = m_obj_Settings.DsApplevel_Mstr.Tables["Applevel_mstr"].NewRow();
            dr["id"] = maxid + 1;
            dr["initiator_id"] = lkeditInitiator.EditValue.ToString().Trim();
            dr["applevel"] = cboAppLevel.SelectedItem.ToString().Trim();
            dr["user_role"] = lkeditUserRoles.EditValue.ToString().Trim();
            dr["entry_ty"] = treevwNodes.SelectedNode.Tag.ToString().Trim();
            dr["bypasslevel"] = chkPasslevel.Checked;
            
            DataRow findrow = m_obj_Settings.DsSettings.Tables["Usersroles"].Select("user_role ='" + lkeditUserRoles.EditValue.ToString().Trim() + "'")[0];
            if (findrow != null)
            {
                dr["type"] = Convert.ToString(findrow["type"]).Trim();
            }
            dr["recstatus"] = "I";

            m_obj_Settings.DsApplevel_Mstr.Tables["AppLevel_mstr"].Rows.Add(dr);
       //     m_obj_Settings.DsApplevel_Mstr.Tables["Applevel_mstr"].AcceptChanges();
        }

        private void ModifyLevel()
        {
            var findrows = (from DataRow row in m_obj_Settings.DsApplevel_Mstr.Tables["Applevel_mstr"].Rows
                            where row["user_role"].ToString().Trim() == lkeditUserRoles.EditValue.ToString().Trim() 
                            && row["applevel"].ToString().Trim() == cboAppLevel.SelectedItem.ToString().Trim()
                            && (row["bypasslevel"] == DBNull.Value ? false :  Convert.ToBoolean(row["bypasslevel"])) == chkPasslevel.Checked
                            select row).ToList();

            if (findrows.Count() > 0)
            {
                MessageBox.Show("Role / User has already assigned to Approval Level, Cannot assign again to another Approval Level", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow[] dtrows = (from DataRow row in m_obj_Settings.DsApplevel_Mstr.Tables["Applevel_mstr"].Rows
                                where row["Applevel"].ToString().Trim() == m_applevel && 
                                row["User_role"].ToString().Trim() != m_userrole
                                select row).ToArray();

            bool isRole = false, isUser = false;

            foreach (DataRow dtapplevel in dtrows)
            {
                DataRow[] dtusermstrrows = null;
                if (Convert.ToString(dtapplevel["type"]).Trim().ToUpper() == "ROLE")
                {
                    dtusermstrrows = (from DataRow row in m_obj_Settings.DsSettings.Tables["User_mstr"].Rows
                                      where row["user"].ToString().Trim() == lkeditUserRoles.EditValue.ToString().Trim()
                                      select row).ToArray();
                }
                else
                {
                    if (Convert.ToString(dtapplevel["type"]).Trim().ToUpper() == "USER")
                    {
                        dtusermstrrows = (from DataRow row in m_obj_Settings.DsSettings.Tables["User_mstr"].Rows
                                          where row["user_roles"].ToString().Trim() == lkeditUserRoles.EditValue.ToString().Trim()
                                          select row).ToArray();
                    }
                }


                foreach (DataRow druser in dtusermstrrows)
                {
                    if (Convert.ToString(dtapplevel["type"]).Trim().ToUpper() == "ROLE")
                    {
                        if (Convert.ToString(dtapplevel["user_role"]).ToUpper().Trim() ==
                               Convert.ToString(druser["user_roles"]).ToUpper().Trim())
                        {
                            isRole = true;
                            break;
                        }
                    }
                    else
                    {
                        if (Convert.ToString(dtapplevel["type"]).Trim().ToUpper() == "USER")
                        {
                            if (Convert.ToString(dtapplevel["user_role"]).ToUpper().Trim() ==
                                   Convert.ToString(druser["user"]).ToUpper().Trim())
                            {
                                isUser = true;
                                break;
                            }
                        }
                    }
                }

                if (isRole == true)
                {
                    MessageBox.Show("User has already belong to this Role " + Convert.ToString(dtapplevel["user_role"]).ToUpper().Trim() + " and " +
                                    "this Role has already assigned to " + cboAppLevel.SelectedItem.ToString().Trim() + ", you cannot assign again \n\n" +
                                    "If you want assign this user, please remove assigned Role from the Approval Level and then try the same"
                            , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                else
                {
                    if (isUser == true)
                    {
                        MessageBox.Show(Convert.ToString(dtapplevel["user_role"]).ToUpper().Trim() + " user has already belong to this Role, and " +
                                    "User has already assigned to " + cboAppLevel.SelectedItem.ToString().Trim() + ", you cannot assign again \n\n" +
                                    "If you want assign this Role, please remove assigned User from the Approval Level and then try the same"
                            , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            }

            if (isRole == true || isUser == true)
                return;

            DataRow dr = m_obj_Settings.DsApplevel_Mstr.Tables["AppLevel_mstr"].Select("id =" + RowId)[0];

            dr["applevel"] = cboAppLevel.SelectedItem.ToString().Trim();
            dr["user_role"] = lkeditUserRoles.EditValue.ToString().Trim();
            dr["bypasslevel"] = chkPasslevel.Checked;

            DataRow findrow = m_obj_Settings.DsSettings.Tables["Usersroles"].Select("user_role ='" + lkeditUserRoles.EditValue.ToString().Trim() + "'")[0];
            if (findrow != null)
            {
                dr["type"] = Convert.ToString(findrow["type"]).Trim();
            }
            dr["recstatus"] =  Convert.ToInt32(dr["id"]) == 0 ? "I" : "U";

          //  m_obj_Settings.DsApplevel_Mstr.Tables["AppLevel_mstr"].AcceptChanges();


        }

        private void SetGridImages()
        {
            for (int r = 0; r < dtgrdvwAppLevel.Rows.Count; r++)
            {
                DataGridViewImageCell cell = (DataGridViewImageCell)dtgrdvwAppLevel.Rows[r].Cells[2];
                DataGridViewTextBoxCell celltype = (DataGridViewTextBoxCell)dtgrdvwAppLevel.Rows[r].Cells[3];

                switch (celltype.Value.ToString().Trim().ToUpper())
                {
                    case "USER":
                        cell.Value = imagelist.Images[0];
                        break;
                    case "ROLE":
                        cell.Value = imagelist.Images[2];
                        break;
                }
            }
        }
        private void GenerateTranListNodes()
        {
           
            treevwNodes.Nodes.Clear();
            treevwNodes.ImageList = imagelist;
            foreach (DataRow dr in m_obj_Settings.DsSettings.Tables["mastcode"].Rows)
            {
                TreeNode trnode = new TreeNode();
                trnode.Name = "Menu";
                trnode.Text = Convert.ToString(dr["code_nm"]).Trim().ToUpper();
                trnode.ImageIndex = 1;
                trnode.SelectedImageIndex = 1;
                trnode.Tag = Convert.ToString(dr["entry_ty"]).Trim().ToUpper();
                treevwNodes.Nodes.Add(trnode);
            }

            foreach (DataRow dr in m_obj_Settings.DsSettings.Tables["lcode"].Rows)
            {
                TreeNode trnode = new TreeNode();
                trnode.Name = "Menu";
                trnode.Text = Convert.ToString(dr["code_nm"]).Trim().ToUpper();
                trnode.ImageIndex = 1;
                trnode.SelectedImageIndex = 1;
                trnode.Tag = Convert.ToString(dr["entry_ty"]).Trim();
                treevwNodes.Nodes.Add(trnode);
            }
        }

        //private void GenerateUserRolesList()
        //{
          
        //    cboRoleUser.DataSource = m_obj_Settings.DsSettings.Tables["usersroles"];
        //    cboRoleUser.DisplayMember = "user_role";
        //    cboRoleUser.ValueMember = "user_role";

        //    //cboRoleUser.Items.Insert(0, "--------------Select Role User------------");
        //}

        private void GenerateLevelNodes()
        {
            treevwLevels.ImageList = imagelist;
            treevwLevels.Nodes.Clear();

            
            //IEnumerable<DataRow> applevel = (from AppLevel_Mstr in m_obj_Settings.DsApplevel_Mstr.Tables["AppLevel_Mstr"].AsEnumerable()
            //                                 orderby AppLevel_Mstr.Field<string>("AppLevel")
            //                                 select AppLevel_Mstr);

//            DataView Dv = m_obj_Settings.DsApplevel_Mstr.Tables["AppLevel_Mstr"].DefaultView;
//            Dv.Sort = "Applevel asc";

  //          DataTable dt = Dv.ToTable();

          //  DataViewRowState dvrs = DataViewRowState.OriginalRows;
            DataRow[] rows = m_obj_Settings.DsApplevel_Mstr.Tables["AppLevel_Mstr"].Select("", "AppLevel", DataViewRowState.CurrentRows);

            for (int i = 0; i < rows.Length; i++)
            {
                var result = from TreeNode node in treevwLevels.Nodes
                             where node.Text.Contains(Convert.ToString(Convert.ToString(rows[i]["AppLevel"]).Trim()))
                             select node.Index;

                TreeNode trnode;
                if (result.Count() == 0)
                {
                    trnode = new TreeNode();
                    trnode.Name = "Level";
                    trnode.Text = Convert.ToString(rows[i]["AppLevel"]).Trim();
                    trnode.Tag = "Level";

                    trnode.ImageIndex = 3;
                    trnode.SelectedImageIndex = 3;
                    treevwLevels.Nodes.Add(trnode);

                    GenerateUserNodes(trnode, Convert.ToString(rows[i]["AppLevel"]).Trim());
                }

            }

            //foreach (DataRow dr in applevel)
            //{
            //   var result = from TreeNode node in treevwLevels.Nodes
            //                 where node.Text.Contains(Convert.ToString(Convert.ToString(dr["AppLevel"]).Trim()))
            //                 select node.Index;

            //   TreeNode trnode;
            //    if (result.Count() == 0)
            //   {
            //        trnode = new TreeNode();
            //        trnode.Name = "Level";
            //        trnode.Text = Convert.ToString(dr["AppLevel"]).Trim();
            //        trnode.Tag = "Level";
                   
            //        trnode.ImageIndex = 3;
            //        trnode.SelectedImageIndex = 3;
            //        treevwLevels.Nodes.Add(trnode);

            //        GenerateUserNodes(trnode, Convert.ToString(dr["AppLevel"]).Trim());
            //   }

                
               
            //}
        }

        private void GenerateUserNodes(TreeNode trnode, string text)
        {
            foreach (DataRow dr in Dgv.Table.Select("Applevel ='" + text.Trim() +"'"))
            {
                TreeNode trchildnode;
                trchildnode = new TreeNode();
                trchildnode.Name = "User";
                trchildnode.Text = Convert.ToString(dr["User_role"]).Trim();
                trchildnode.Tag = "User";
                if (Convert.ToString(dr["type"]).Trim().ToUpper() == "ROLE")
                {
                    trchildnode.ImageIndex = 2;
                    trchildnode.SelectedImageIndex = 2;
                }
                else if (Convert.ToString(dr["type"]).Trim().ToUpper() == "USER")
                {
                    trchildnode.ImageIndex = 0;
                    trchildnode.SelectedImageIndex = 0;
                }
                trnode.Nodes.Add(trchildnode);
            }

            treevwLevels.ExpandAll();
        }

        private void GenerateLevelList(int cntApplevel)
        {
            cboAppLevel.Items.Clear();
            for (int l = 1; l <= cntApplevel; l++)
            {
                cboAppLevel.Items.Add("Approval Level " + l.ToString().Trim());
            }
            cboAppLevel.Items.Insert(0, " ---------------- Select Approval Level ---------------- ");
            cboAppLevel.SelectedIndex = 0;
        }

        private void ControlBinding()
        {
            Dgv = m_obj_Settings.DsApplevel_Mstr.Tables["Applevel_mstr"].DefaultView;
            Dgv.Sort = "Applevel Asc";
          
            dtgrdvwAppLevel.AutoGenerateColumns = false;
            dtgrdvwAppLevel.DataSource = Dgv;
        }

        private void DoEditAppLevel(int rowindex)
        {
            string applevel = Convert.ToString(dtgrdvwAppLevel.Rows[rowindex].Cells[0].Value);
            string userrole = Convert.ToString(dtgrdvwAppLevel.Rows[rowindex].Cells[1].Value);
            bool passlevel = Convert.ToBoolean(dtgrdvwAppLevel.Rows[rowindex].Cells[4].Value);
            int id = Convert.ToInt32(dtgrdvwAppLevel.Rows[rowindex].Cells[5].Value);

            cboAppLevel.SelectedItem = applevel;
           // lkeditUserRoles.EditValue = lkeditUserRoles.Properties.GetDataSourceValue("user_role", rowindex);
            lkeditUserRoles.Text = userrole;
         //   lkeditUserRoles.Properties.poup
            chkPasslevel.Checked = passlevel;

            m_applevel = applevel;
            m_userrole = userrole;



            btnUpdate.Tag = "Update";
            RowId = id;

            btnCancel.Visible = true;
            btnDelete.Visible = true;
            cboAppLevel.Enabled = false;
            errorProvider.Clear();

            SetGridImages();
            GenerateLevelNodes();

            lkeditUserRoles.Focus();
        }

        private void DoDeleteAppLevel(int id)
        {
            if (MessageBox.Show("Want to Delete ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataRow drdel = m_obj_Settings.DsApplevel_Mstr.Tables["Applevel_mstr"].Select("id =" + id)[0];

                // storing delete records in temp table for reference
                if (drdel["id"] != System.DBNull.Value)
                {
                    if (Convert.ToInt32(drdel["id"]) != 0)
                    {
                        DataRow drrecdelstat = m_obj_Settings.DsSettings.Tables["Recdelstat"].NewRow();
                        drrecdelstat["id"] = Convert.ToInt32(drdel["id"]);
                        drrecdelstat["entry_ty"] = Convert.ToString(drdel["entry_ty"]).Trim();
                        m_obj_Settings.DsSettings.Tables["recdelstat"].Rows.Add(drrecdelstat);
                     //   m_obj_Settings.DsSettings.Tables["recdelstat"].AcceptChanges();
                    }
                }
                //end

                drdel.Delete();
            //    m_obj_Settings.DsApplevel_Mstr.Tables["Applevel_mstr"].AcceptChanges();

                SetGridImages();
                GenerateLevelNodes();
                btnCancel.PerformClick();
            }
        }

        private void dtgrdvwAppLevel_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgrdvwAppLevel.Rows.Count > 0)
            {
                DoEditAppLevel(e.RowIndex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_applevel = string.Empty;
            m_userrole = string.Empty;
            RowId = 0;
            btnCancel.Visible = false;
            btnDelete.Visible = false;
            btnUpdate.Tag = " Add ";

            cboAppLevel.Enabled = true;
            cboAppLevel.SelectedIndex = 0;
            lkeditUserRoles.EditValue = string.Empty;
           // cboRoleUser.SelectedIndex = 0;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dtgrdvwAppLevel.Rows.Count > 0)
            {
                DoEditAppLevel(dtgrdvwAppLevel.SelectedCells[0].RowIndex);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dtgrdvwAppLevel.Rows.Count > 0)
            {
                int id = Convert.ToInt32(dtgrdvwAppLevel.Rows[dtgrdvwAppLevel.SelectedCells[0].RowIndex].Cells[5].Value);
                DoDeleteAppLevel(id);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DoDeleteAppLevel(RowId);
        }

        private void treevwNodes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            GetFillDataInLookUpEditor(e.Node.Tag.ToString().Trim());

            if (lkeditInitiator.EditValue == null || lkeditInitiator.EditValue.ToString().Trim() == string.Empty)
            {
                Dgv = null;
                dtgrdvwAppLevel.DataSource = Dgv;
                dtgrdvwAppLevel.Refresh();
                return;
            }

            GetAppLevelDetails(e.Node.Tag.ToString().Trim(), lkeditInitiator.EditValue.ToString().Trim());
        }

        private void GetFillDataInLookUpEditor(string trantype)
        {
            string tblname = string.Empty;

            if (trantype == "IM" || trantype == "AM")
                tblname = "mastcode";
            else
                tblname = "lcode";

            var rowlcode = (from DataRow rowLcode in m_obj_Settings.DsSettings.Tables[tblname].Rows
                        where rowLcode["entry_ty"].ToString().Trim() == trantype
                        select rowLcode).FirstOrDefault();

            DataView dvusers = null;
            if (rowlcode != null)
                applevelInit = rowlcode["applevel_init"] != DBNull.Value ? Convert.ToBoolean(rowlcode["applevel_init"]) : false;
            else
                return;

            if (applevelInit == true)
            {
                dvusers = m_obj_Settings.DsSettings.Tables["User_mstr"].DefaultView;
                lkeditInitiator.Enabled = true;
                btnGetData.Enabled = true;
            }
            else
            {
                dvusers = m_obj_Settings.DsSettings.Tables["User_init"].DefaultView;
                lkeditInitiator.Enabled = false;
                btnGetData.Enabled = false;
            }


            lkeditInitiator.Properties.DataSource = dvusers;

            lkeditInitiator.Properties.DisplayMember = "user";
            lkeditInitiator.Properties.ValueMember = "user";
            lkeditInitiator.Properties.PopupFormWidth = 400;
            lkeditInitiator.Properties.PopupWidth = 500;
            lkeditInitiator.Properties.NullText = string.Empty;
            lkeditInitiator.EditValue = string.Empty;


            if (applevelInit == false)
                lkeditInitiator.ItemIndex = 0;
            else
                lkeditInitiator.ItemIndex = -1;

            DataView dvusersroles = m_obj_Settings.DsSettings.Tables["Usersroles"].DefaultView;
            lkeditUserRoles.Properties.DataSource = dvusersroles;

            lkeditUserRoles.Properties.DisplayMember = "user_role";
            lkeditUserRoles.Properties.ValueMember = "user_role";


            lkeditUserRoles.Properties.PopupWidth = 500;
            lkeditUserRoles.Properties.NullText = string.Empty;

        }

        private void btnMainSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Save Dataset changes
                m_obj_Settings.DsApplevel_Mstr.Tables["AppLevel_Mstr"].AcceptChanges();
                m_obj_Settings.DsSettings.Tables["recdelstat"].AcceptChanges();

                statusLabel.Text = "Please wait,Saving data.......";
                m_obj_Settings.Active = chkActive.Checked;
                m_obj_Settings.Save();
                GetRefreshData();
                statusLabel.Text = string.Empty;

                MessageBox.Show("Records has been Saved succesfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex)
            {
                string error = "\n\n found error while saving record  \n\n" +
                        "Message : " + Ex.Message +
                        "\nSource : " + Ex.Source +
                        "\nTargetSite : " + Ex.TargetSite;

                MessageBox.Show(error, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetRefreshData()
        {
            treevwNodes.Select();
            m_entrytype = treevwNodes.SelectedNode.Tag.ToString().Trim();
            m_obj_Settings.Trantype = m_entrytype;
            m_obj_Settings.GetAppLevel_Mstr();

            ControlBinding();
            SetGridImages();
            GenerateLevelNodes();
            
        }

        private void btnMainCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MLAAppLevelSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_obj_Settings.DsApplevel_Mstr != null)
            {
                if (m_obj_Settings.DsApplevel_Mstr.HasChanges() == true)
                {
                    if (MessageBox.Show("Discard changes ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        private void dtgrdvwAppLevel_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SetGridImages();
        }

        private void cboAppLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAppLevel.SelectedIndex == 0)
                return;

            DataRow row = null;
            if (m_entrytype == "IM" || m_entrytype == "AM")
                row = m_obj_Settings.DsSettings.Tables["mastcode"].Select("entry_ty='" + m_entrytype + "'")[0];
            else
                row = m_obj_Settings.DsSettings.Tables["lcode"].Select("entry_ty='" + m_entrytype + "'")[0];

            int applevel = row["applevel"] != DBNull.Value ? Convert.ToInt32(row["applevel"]) : 0;

            string currlevel = cboAppLevel.SelectedItem.ToString().Trim();
            int sellevel = Convert.ToInt32(currlevel.Substring(currlevel.Length - 1, 1));

            if (sellevel == applevel)
               chkPasslevel.Enabled = false;
            else
                chkPasslevel.Enabled = true;
        }

        private void GetAppLevelDetails(string trantype,
                                        string initiator)
        {
            if (m_obj_Settings.DsApplevel_Mstr != null)
            {
                if (m_obj_Settings.DsApplevel_Mstr.HasChanges() == true)
                {
                    if (MessageBox.Show("Changes has found..., Do you want to Save the Changes ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        btnMainSave.PerformClick();
                    else
                    {
                        m_obj_Settings.DsApplevel_Mstr.Tables["AppLevel_Mstr"].RejectChanges();
                        m_obj_Settings.DsSettings.Tables["recdelstat"].RejectChanges();
                    }
                }
            }

            statusLabel.Text = "Please wait,getting Data.......";

            m_entrytype = trantype;
            groupBox2.Text = " Approval Level Input Details - " + treevwNodes.SelectedNode.Text.Trim();

            m_obj_Settings.Trantype = m_entrytype;
            m_obj_Settings.Initiator = initiator;
            m_obj_Settings.GetAppLevel_Mstr();

            ControlBinding();
            SetGridImages();
            GenerateLevelNodes();
            statusLabel.Text = string.Empty;

            // Check Active status
            var rowforcheckactive = (from DataRow rowApplevel_mstr in  m_obj_Settings.DsApplevel_Mstr.Tables["Applevel_mstr"].Rows
                                      where rowApplevel_mstr["entry_ty"].ToString().Trim() == m_entrytype &&
                                            rowApplevel_mstr["initiator_id"].ToString().Trim() == initiator &&
                                            (rowApplevel_mstr["active"] == DBNull.Value ? false : Convert.ToBoolean(rowApplevel_mstr["active"])) == true 
                                      select rowApplevel_mstr).FirstOrDefault();

            if (rowforcheckactive == null)
                chkActive.Checked = false;
            else
                chkActive.Checked = true;

            // if the records not found for this intitator then it will consider add status
            if (m_obj_Settings.DsApplevel_Mstr.Tables["Applevel_mstr"].Rows.Count == 0)
                chkActive.Checked = true;

            DataRow row = null;
            if (m_entrytype == "IM" || m_entrytype == "AM")
                row = m_obj_Settings.DsSettings.Tables["mastcode"].Select("entry_ty='" + m_entrytype + "'")[0];
            else
                row = m_obj_Settings.DsSettings.Tables["lcode"].Select("entry_ty='" + m_entrytype + "'")[0];

            int applevel = row["applevel"] != DBNull.Value ? Convert.ToInt32(row["applevel"]) : 0;

            GenerateLevelList(applevel);

            // Reset all settings
            cboAppLevel.SelectedIndex = 0;
            cboAppLevel.Enabled = true;

            //cboRoleUser.SelectedIndex = 0;
            if (applevelInit == true)
            {
                lkeditUserRoles.EditValue = string.Empty;
                lkeditInitiator.Enabled = true;
                btnGetData.Enabled = true;
            }
            else
            {
                lkeditInitiator.Enabled = false;
                btnGetData.Enabled = false;
            }

            //cboRoleUser.Enabled = true;

            chkPasslevel.Enabled = true;

            btnDelete.Visible = false;
            btnCancel.Visible = false;
        }


        private void lkeditInitiator_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (lkeditInitiator.EditValue == null)
                    return;

                GetAppLevelDetails(treevwNodes.SelectedNode.Tag.ToString().Trim(), lkeditInitiator.EditValue.ToString().Trim());
            }

        }

        //private void lkeditInitiator_Validated(object sender, EventArgs e)
        //{
        //    if (lkeditInitiator.EditValue == null)
        //        return;

        //    GetAppLevelDetails(treevwNodes.SelectedNode.Tag.ToString().Trim(), lkeditInitiator.EditValue.ToString().Trim());
        //}

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (chkActive.Checked == false && activeclick == true)
            {
                if (MessageBox.Show("Deactive Level Workflow ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    chkActive.Checked = false;
                else
                    chkActive.Checked = true;

                activeclick = false;
            }
        }

        private void dtgrdvwAppLevel_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataTable dt = m_obj_Settings.DsSettings.Tables["user_mstr"];

                if (dtgrdvwAppLevel.Rows[e.RowIndex].Cells[3].Value.ToString().ToUpper() == "USER")
                {
                     DataRow dr = m_obj_Settings.DsSettings.Tables["user_mstr"].Select("user ='" + dtgrdvwAppLevel.Rows[e.RowIndex].Cells[1].Value.ToString().Trim() +"'")[0];
                     e.ToolTipText = "Name :" + Convert.ToString(dr["user_name"]).Trim() + "\n" +
                                     "Designation :" + Convert.ToString(dr["designation"]).Trim() + "\n" +
                                     "Department :" + Convert.ToString(dr["dept"]).Trim() + "\n" +
                                     "Email Id.:" + Convert.ToString(dr["email"]).Trim() + "\n\n\n" +
                                     "Press Double click or Right click for update and delete details";

                }
            }
        }

        private void chkActive_Click(object sender, EventArgs e)
        {
            activeclick = true;
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (lkeditInitiator.EditValue == null)
                return;

            GetAppLevelDetails(treevwNodes.SelectedNode.Tag.ToString().Trim(), lkeditInitiator.EditValue.ToString().Trim());
        }

        private void lkeditInitiator_Validated(object sender, EventArgs e)
        {
            if (lkeditInitiator.EditValue == null)
                return;

            GetAppLevelDetails(treevwNodes.SelectedNode.Tag.ToString().Trim(), lkeditInitiator.EditValue.ToString().Trim());
        }

      
        //private void lkeditInitiator_Validating(object sender, CancelEventArgs e)
        //{
        //    GetAppLevelDetails(treevwNodes.SelectedNode.Tag.ToString().Trim(), lkeditInitiator.EditValue.ToString().Trim());
        //}
            
     
    }
}
