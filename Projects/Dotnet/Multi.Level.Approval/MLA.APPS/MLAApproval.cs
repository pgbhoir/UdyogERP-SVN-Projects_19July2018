using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MLA.LL;
using MLA.ENTITY;
using eMailClient.MailSender;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using MLA.DL;
using vunettofx;

namespace MLA.APPS
{
    public partial class MLAApproval : Form
    {

        public MLAApproval()
        {
            InitializeComponent();
        }


        public static string Connectionstring { get; set; }
        public string UserID { get; set; }
        public DateTime Startdt { get; set; }
        public DateTime Enddt { get; set; }
        public string IconPath { get; set; }
        public int CompId { get; set; }
        public string DbName { get; set; }
        public string DefaPath { get; set; }
        public string AppPath { get; set; }
        public string ProdCode { get; set; }
        private System.Data.DataSet applevel_dtl;

        bool paneltoggle = false;
        int panelwidth = 0;
        bool isAccount = false;
        bool isItem = false;
        bool isActivate = false;
        DataTable DtMainvw;
        DataTable DtMastData;
        DataTable DtApplevel_Mstr;
        string prevtrantype = string.Empty;
        private string formstatus = string.Empty;

        private void timer_Tick(object sender, EventArgs e)
        {
            if (paneltoggle == true)
            {
                if (panelwidth < 245)
                {
                    panelwidth += 5;
                }
                else
                {
                    panSideBar.Refresh();
                    timer.Enabled = false;
                }
            }
            else
            {
                if (paneltoggle == false)
                {
                    if (panelwidth > -55)
                    {
                        panelwidth -= 10;
                    }
                    else
                    {
                        panSideBar.Refresh();
                        timer.Enabled = false;
                    }
                }
            }

            panSideBar.Location = new System.Drawing.Point(-245 + panelwidth, 1);
        }

        private void panSideBar_MouseLeave(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void tsBtnSideBar_Click(object sender, EventArgs e)
        {
            if (paneltoggle == true)
                paneltoggle = false;
            else if (paneltoggle == false)
                paneltoggle = true;

            timer.Enabled = true;
        }


        private void picBox_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            paneltoggle = false;
        }

        cls_Gen_Mgr_Approval m_obj_Approval = new cls_Gen_Mgr_Approval(Connectionstring);


        private void MLAApproval_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(IconPath))
                {
                    Icon ico = new Icon(IconPath);
                    this.Icon = ico;
                }

                m_obj_Approval.GetAllInitialData(CompId);
                GenerateEntitiesNode();
                ControlsBinding();
                tsLogUser.Text = "Log User : " + UserID.Trim().ToUpper();
                grid.Height = btnSave.Top - 20;

                string format = @"dd\/MM\/yyyy";

                dtPickFrom.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
                dtPickFrom.Properties.Mask.EditMask = format;
                dtPickFrom.Properties.Mask.UseMaskAsDisplayFormat = true;
                dtPickFrom.Text = Startdt.ToString("dd/MM/yyyy");

                dtPickTo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
                dtPickTo.Properties.Mask.EditMask = format;
                dtPickTo.Properties.Mask.UseMaskAsDisplayFormat = true;
                dtPickTo.Text = Enddt.ToString("dd/MM/yyyy");
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

        private void GenerateEntitiesNode()
        {

            treevwEntities.BeginUpdate();
            string[] caption = { "Entities List" };
            treevwEntities.Columns.Add();
            treevwEntities.Columns[0].Caption = caption[0];
            treevwEntities.Columns[0].VisibleIndex = 0;
            treevwEntities.Columns[0].FieldName = caption[0];
            treevwEntities.Columns[0].Name = caption[0];
            treevwEntities.EndUpdate();

            foreach (DataRow row in m_obj_Approval.DsSettings.Tables["mastcode"].Rows)
            {
                treevwEntities.BeginUnboundLoad();
                // Create a root node
                TreeListNode parentForRootNodes = null;
                parentForRootNodes = treevwEntities.AppendNode(new object[] { Convert.ToString(row["code_nm"]).Trim().ToUpper() }, parentForRootNodes);
                parentForRootNodes.Tag = Convert.ToString(row["entry_ty"]).Trim() + "," + Convert.ToString(row["applevel"]).Trim();
                parentForRootNodes.SelectImageIndex = 1;
                parentForRootNodes.ImageIndex = 1;
                //   treevwEntities.SetNodeIndex(parentForRootNodes, 2);
                treevwEntities.EndUnboundLoad();
            }

            foreach (DataRow row in m_obj_Approval.DsSettings.Tables["lcode"].Rows)
            {
                treevwEntities.BeginUnboundLoad();
                // Create a root node
                TreeListNode parentForRootNodes = null;
                parentForRootNodes = treevwEntities.AppendNode(new object[] { Convert.ToString(row["code_nm"]).Trim().ToUpper() }, parentForRootNodes);
                parentForRootNodes.Tag = Convert.ToString(row["entry_ty"]).Trim() + "," + Convert.ToString(row["applevel"]).Trim();
                parentForRootNodes.ImageIndex = 1;
                parentForRootNodes.SelectImageIndex = 1;
                // treevwEntities.SetNodeIndex(parentForRootNodes, 2);
                treevwEntities.EndUnboundLoad();
            }


        }

        private void ControlsBinding()
        {
            ImageComboBoxItem item = new ImageComboBoxItem();
            item.Description = "All";
            item.ImageIndex = 7;
            item.Value = "All";
            cboStatus.Properties.Items.Add(item);

            item = new ImageComboBoxItem();
            item.Description = "Approved";
            item.ImageIndex = 5;
            item.Value = "Approved";
            cboStatus.Properties.Items.Add(item);

            item = new ImageComboBoxItem();
            item.Description = "Rejected";
            item.ImageIndex = 4;
            item.Value = "Rejected";
            cboStatus.Properties.Items.Add(item);

            cboStatus.Properties.SmallImages = imagelist;
            cboStatus.SelectedIndex = 0;
        }


        private void treevwEntities_Click(object sender, EventArgs e)
        {
            if (treevwEntities.Nodes.Count <= 0)        //Added by Shrikant S. on 23/08/2018 for Installer 2.0.0
                return;                                 //Added by Shrikant S. on 23/08/2018 for Installer 2.0.0

            string[] nodetag = Convert.ToString(treevwEntities.FocusedNode.Tag).Trim().Split(',');
            string trantype = nodetag[0].Trim();


            if (DtMainvw != null)
            {
                if (DtMainvw.DataSet != null)
                {
                    if (DtMainvw.DataSet.HasChanges() == true)
                    {
                        if (MessageBox.Show("Changes has found..., Do you want to Save the Changes ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            DoSave(prevtrantype);
                        else
                            DtMainvw.DataSet.RejectChanges();
                    }
                }
            }

            if (DtMastData != null)
            {
                if (DtMainvw != null)
                {
                    if (DtMainvw.DataSet != null)
                    {
                        if (DtMastData.DataSet.HasChanges() == true)
                        {
                            if (MessageBox.Show("Changes has found..., Do you want to Save the Changes ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                DoSave(prevtrantype);
                            //btnMainSave.PerformClick();
                            else
                                DtMastData.RejectChanges();
                        }
                    }
                }
            }

            if (trantype == "IM")
            {
                BindingMasterGrid(trantype, "it_code", grdIM, false);
                grdIM.Location = new Point(0, 0);
                grdIM.Height = btnSave.Top - 20;
                grdIM.Visible = true;
                grdIM.Width = grdAM.Width;

                grid.Visible = false;
                grdAM.Visible = false;

                grdIM.Visible = true;

                tbPage.Text = "Approve Details - Item Master";
                this.DtMainvw = null;

                this.grdAM.DataSource = null;           //Added by Shrikant S. on 03/08/2018 for Bug-30904
                this.grid.DataSource = null;            //Added by Shrikant S. on 03/08/2018 for Bug-30904

            }
            else if (trantype == "AM")
            {
                BindingMasterGrid(trantype, "ac_id", grdAM, false);
                grdAM.Location = new Point(0, 0);
                grdAM.Height = btnSave.Top - 20;
                grdAM.Visible = true;

                grdIM.Visible = false;
                grid.Visible = false;

                tbPage.Text = "Approve Details - Account Master";

                this.grdIM.DataSource = null;           //Added by Shrikant S. on 03/08/2018 for Bug-30904
                this.grid.DataSource = null;            //Added by Shrikant S. on 03/08/2018 for Bug-30904
            }
            else
            {

                BindingTransactionGrid(trantype, false);
                grdAM.Height = btnSave.Top - 20;
                grdAM.Visible = false;
                grdIM.Visible = false;
                grid.Visible = true;

                DataRow lcoderow = m_obj_Approval.DsSettings.Tables["lcode"].Select("entry_ty='" + trantype + "'")[0];
                tbPage.Text = "Approve Details - " + Convert.ToString(lcoderow["code_nm"]).Trim();

                this.grdIM.DataSource = null;           //Added by Shrikant S. on 03/08/2018 for Bug-30904
                this.grdAM.DataSource = null;            //Added by Shrikant S. on 03/08/2018 for Bug-30904
            }

            SetFilter();
            prevtrantype = trantype;
        }

        private void BindingMasterGrid(string trantype,
                                       string keyfld,
                                       GridControl grdcontrol,
                                       bool isInit)
        {
            DtApplevel_Mstr = new DataTable();

            DtApplevel_Mstr = m_obj_Approval.GetApplevelMstr(trantype);

            if (DtApplevel_Mstr.Rows.Count == 0)
            {
                MessageBox.Show("Approval levels not defined for selected Master in Approval workflow Master,\nPlease create first approval workflow in Master",
                                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DtMastData = m_obj_Approval.GetMasterData(trantype, UserID);

            if (DtMastData.Rows.Count == 0)
            {
                if (isInit == false)
                {
                    tsMessage.Text = string.Empty;
                    MessageBox.Show("No records found for approval...!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                    tsMessage.Text = "No records found for approval....!!";
            }
            else
                tsMessage.Text = string.Empty;

            grdcontrol.DataSource = DtMastData;
            paneltoggle = false;
            timer.Enabled = true;
        }


        private void BindingTransactionGrid(string trantype, bool isInit)
        {
            try
            {
                DtApplevel_Mstr = new DataTable();
                DtMainvw = new DataTable();

                DtApplevel_Mstr = m_obj_Approval.GetApplevelMstr(trantype);

                if (DtApplevel_Mstr.Rows.Count == 0)
                {
                    MessageBox.Show("Approval levels not defined for selected transaction in Approval workflow Master,\nPlease create first approval workflow in Master",
                                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DtMainvw = m_obj_Approval.GetMainList(trantype, Convert.ToDateTime(dtPickFrom.Text.Trim()), Convert.ToDateTime(dtPickTo.Text.Trim()), UserID);


                var dtlcode = (from DataRow row in m_obj_Approval.DsSettings.Tables["lcode"].Rows
                               where row["entry_ty"].ToString().Trim() == trantype
                               select row).Single();

                isAccount = dtlcode.Field<bool>("v_account");
                isItem = dtlcode.Field<bool>("v_item");

                int listcount = DtMainvw.Rows.Count;

                if (isAccount == true && isItem == true)
                {
                    relations = new object[listcount, 2];
                    for (int l = 0; l < listcount; l++)
                    {
                        relations[l, 0] = "Item Details";
                        relations[l, 1] = "Account Details";
                    }
                }
                else
                {
                    if (isAccount == true && isItem == false)
                    {
                        relations = new object[listcount, 1];
                        for (int l = 0; l < listcount; l++)
                        {
                            relations[l, 0] = "Account Details";
                        }
                    }
                    else
                    {
                        if (isAccount == false && isItem == true)
                        {
                            relations = new object[listcount, 1];
                            for (int l = 0; l < listcount; l++)
                            {
                                relations[l, 0] = "Item Details";
                            }
                        }
                    }

                }

                grid.DataSource = DtMainvw;
                paneltoggle = false;
                timer.Enabled = true;


                // Hide columns on conditions
                int recNosinvSr = (from DataRow row in DtMainvw.Rows
                                   where row["inv_sr"].ToString().Trim() != string.Empty
                                   select row).Count();

                int recNosdept = (from DataRow row in DtMainvw.Rows
                                  where row["dept"].ToString().Trim() != string.Empty
                                  select row).Count();

                int recNoscate = (from DataRow row in DtMainvw.Rows
                                  where row["cate"].ToString().Trim() != string.Empty
                                  select row).Count();

                if (recNosinvSr == 0)
                    grdvwMain.Columns[6].Visible = false;
                else
                    grdvwMain.Columns[6].Visible = true;

                if (recNoscate == 0)
                    grdvwMain.Columns[7].Visible = false;
                else
                    grdvwMain.Columns[7].Visible = true;

                if (recNosdept == 0)
                    grdvwMain.Columns[8].Visible = false;
                else
                    grdvwMain.Columns[8].Visible = true;

                if (DtMainvw.Rows.Count == 0)
                {
                    if (isInit == false)
                    {
                        tsMessage.Text = string.Empty;
                        MessageBox.Show("No records found for approval...!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                        tsMessage.Text = "No records found for approval....!!";
                }
                else
                    tsMessage.Text = string.Empty;

            }
            catch (Exception ex)
            {
                string error = "\n\n found error while getting records  \n\n" +
                        "Message : " + ex.Message +
                        "\nSource : " + ex.Source +
                        "\nTargetSite : " + ex.TargetSite;

                MessageBox.Show(error, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #region GridControls Events
        private object[,] relations =
        {
            {"Item Details","Account Details"},
            {"Item Details","Account Details"}
        };

        //Specifies the maximum number of details 
        private void grdvwMain_MasterRowGetRelationCount(object sender,
        MasterRowGetRelationCountEventArgs e)
        {
            if (isAccount == true && isItem == true)
                e.RelationCount = 2;
            else
                if (isAccount == true || isItem == true)
                e.RelationCount = 1;
            else
                e.RelationCount = 2;
        }

        //Specifies whether a specific detail contains data 
        //The detail should not be displayed if the corresponding relations element is null 
        private void grdvwMain_MasterRowEmpty(object sender, MasterRowEmptyEventArgs e)
        {
            e.IsEmpty = IsRelationEmpty(e.RowHandle, e.RelationIndex);
        }

        bool IsRelationEmpty(int rowHandle, int relationIndex)
        {
            return rowHandle == GridControl.InvalidRowHandle || relations[rowHandle,
                relationIndex] == null;
        }


        //Get child data for specific relations 
        //The ChildRecordsProducts, ChildRecordsCustomers, ChildRecordsShippers and Records  
        //are custom classes derived from ArrayList 
        private void grdvwMain_MasterRowGetChildList(object sender,
            MasterRowGetChildListEventArgs e)
        {
            if (IsRelationEmpty(e.RowHandle, e.RelationIndex)) return;

            int tran_cd = Convert.ToInt32(grdvwMain.GetRowCellValue(e.RowHandle, "tran_cd"));
            string[] nodetag = Convert.ToString(treevwEntities.FocusedNode.Tag).Trim().Split(',');
            string trantype = nodetag[0].Trim();

            string s = relations[e.RowHandle, e.RelationIndex].ToString();
            switch (s)
            {
                case "Item Details":
                    e.ChildList = m_obj_Approval.GetItemDataList(trantype, tran_cd);
                    break;
                case "Account Details":
                    e.ChildList = m_obj_Approval.GetAccountDataList(trantype, tran_cd);
                    break;
            }
        }



        //Provide names for relations 
        //The names are used to determine pattern Views for representing detail data 
        private void grdvwMain_MasterRowGetRelationName(object sender,
            MasterRowGetRelationNameEventArgs e)
        {
            if (IsRelationEmpty(e.RowHandle, e.RelationIndex))
                e.RelationName = "";
            else
                e.RelationName = relations[e.RowHandle, e.RelationIndex].ToString();
        }
        #endregion

        private void approveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            string a = "";
            string text = "";
            string text2 = "";
            if (this.grdIMvw.DataSource != null) //Added By Kishor A. for Bug-21965 on-08-11-2016
            {
                if (System.Convert.ToString(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "entry_ty").ToString().Trim()) == "IM")
                {
                    text = System.Convert.ToString(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "entry_ty"));
                    num = System.Convert.ToInt32(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "tran_cd"));
                    text2 = string.Concat(new object[]
                    {
                "select entry_ty,tran_cd,applevel,prelevel,levelstatus from applevel_dtl where levelstatus='Approved' and prelevelstatus ='Approved' and prelevel<>0 and entry_ty='",
                text,
                "' and tran_cd=",
                num,
                " order by tran_cd"
                    });
                    this.applevel_dtl = cls_Sqlhelper.ExecuteDataset(MLAApproval.Connectionstring, System.Data.CommandType.Text, text2);

                    num2 = System.Convert.ToInt32(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "applevel"));
                    num3 = System.Convert.ToInt32(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "prelevel"));
                    a = System.Convert.ToString(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "levelstatus"));
                }
            } //Added By Kishor A. for Bug-21965 on-08-11-2016
            else if (this.grdAMvw.DataSource != null) //Added By Kishor A. for Bug-21965 on-08-11-2016
            {
                if (System.Convert.ToString(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "entry_ty").ToString().Trim()) == "AM")
                {
                    text = System.Convert.ToString(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "entry_ty"));
                    num = System.Convert.ToInt32(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "tran_cd"));
                    text2 = string.Concat(new object[]
                    {
                "select entry_ty,tran_cd,applevel,prelevel,levelstatus from applevel_dtl where levelstatus='Approved' and prelevelstatus ='Approved' and prelevel<>0 and entry_ty='",
                text,
                "' and tran_cd=",
                num,
                " order by tran_cd"
                    });
                    this.applevel_dtl = cls_Sqlhelper.ExecuteDataset(MLAApproval.Connectionstring, System.Data.CommandType.Text, text2);

                    num2 = System.Convert.ToInt32(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "applevel"));
                    num3 = System.Convert.ToInt32(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "prelevel"));
                    a = System.Convert.ToString(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "levelstatus"));
                }
            } //Added By Kishor A. for Bug-21965 on-08-11-2016
            else
            {
                text = System.Convert.ToString(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "entry_ty"));
                num = System.Convert.ToInt32(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "tran_cd"));
                text2 = string.Concat(new object[]
                {
                "select entry_ty,tran_cd,applevel,prelevel,levelstatus from applevel_dtl where levelstatus='Approved' and prelevelstatus ='Approved' and prelevel<>0 and entry_ty='",
                text,
                "' and tran_cd=",
                num,
                " order by tran_cd"
                });
                this.applevel_dtl = cls_Sqlhelper.ExecuteDataset(MLAApproval.Connectionstring, System.Data.CommandType.Text, text2);

                num2 = System.Convert.ToInt32(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "applevel"));
                num3 = System.Convert.ToInt32(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "prelevel"));
                a = System.Convert.ToString(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "levelstatus"));
            }
            foreach (System.Data.DataRow dataRow in this.applevel_dtl.Tables[0].Rows)
            {
                if (text == dataRow["entry_ty"].ToString() && num == System.Convert.ToInt32(dataRow["tran_cd"]) && num2 > System.Convert.ToInt32(dataRow["applevel"]) && a == "Approved")
                {
                    System.Windows.Forms.MessageBox.Show("This transaction is already has been approved by upper level " + System.Environment.NewLine + "You can not change status..!!", "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return;
                }
            }
            string[] array = System.Convert.ToString(this.treevwEntities.FocusedNode.Tag).Trim().Split(new char[]
            {
                ','
            });
            string text3 = array[0].Trim();
            if (text3 == "IM")
            {
                this.DoApproveMaster(text3, "it_code", this.grdIMvw);
            }
            else if (text3 == "AM")
            {
                this.DoApproveMaster(text3, "ac_id", this.grdAMvw);
            }
            else
            {
                this.DoApproveTransaction();
            }
        }


        #region Approve and Reject Transaction
        private void DoApproveTransaction()
        {
            string entry_ty = Convert.ToString(grdvwMain.GetRowCellValue(grdvwMain.FocusedRowHandle, "entry_ty"));
            int tran_cd = Convert.ToInt32(grdvwMain.GetRowCellValue(grdvwMain.FocusedRowHandle, "tran_cd"));

            DataRow drMainRow = (from DataRow row in DtMainvw.Rows
                                 where Convert.ToInt32(row["tran_cd"]) == tran_cd
                                 select row).FirstOrDefault();

            drMainRow["Apgen"] = "Approved";
            drMainRow["LevelStatus"] = "Approved";


            Size maxWindowTrackSize = SystemInformation.MaxWindowTrackSize;
            Point Pt = grid.PointToClient(Control.MousePosition);
            MLAAppRemarks AppRemarks = new MLAAppRemarks();
            int mX = 0;
            if ((maxWindowTrackSize.Width + AppRemarks.Width) > Pt.X)
                mX = maxWindowTrackSize.Width - AppRemarks.Width - 50;

            AppRemarks.DesktopLocation = new Point(mX, Pt.Y);
            AppRemarks.Text = "Approval Comments";
            AppRemarks.Remarks = Convert.ToString(drMainRow["Approved_Remarks"]).Trim();

            if (AppRemarks.ShowDialog() == DialogResult.OK)
            {
                drMainRow["Approved_Remarks"] = AppRemarks.Remarks;
                drMainRow["Rejected_Remarks"] = string.Empty;
            }

            grdvwMain.RefreshData();

        }


        private void DoRejectTransaction()
        {
            string entry_ty = Convert.ToString(grdvwMain.GetRowCellValue(grdvwMain.FocusedRowHandle, "entry_ty"));
            int tran_cd = Convert.ToInt32(grdvwMain.GetRowCellValue(grdvwMain.FocusedRowHandle, "tran_cd"));

            DataRow drMainRow = (from DataRow row in DtMainvw.Rows
                                 where Convert.ToInt32(row["tran_cd"]) == tran_cd
                                 select row).FirstOrDefault();

            drMainRow["Apgen"] = "Rejected";
            drMainRow["LevelStatus"] = "Rejected";

            Size maxWindowTrackSize = SystemInformation.MaxWindowTrackSize;
            Point Pt = grid.PointToClient(Control.MousePosition);
            MLAAppRemarks AppRemarks = new MLAAppRemarks();
            int mX = 0;
            if ((maxWindowTrackSize.Width + AppRemarks.Width) > Pt.X)
                mX = maxWindowTrackSize.Width - AppRemarks.Width - 50;

            AppRemarks.DesktopLocation = new Point(mX, Pt.Y);
            AppRemarks.Text = "Rejected Comments";
            AppRemarks.Remarks = Convert.ToString(drMainRow["Rejected_Remarks"]).Trim();

            if (AppRemarks.ShowDialog() == DialogResult.OK)
            {
                drMainRow["Rejected_Remarks"] = AppRemarks.Remarks;
                drMainRow["Approved_Remarks"] = string.Empty;
            }

            grdvwMain.RefreshData();

        }
        #endregion


        #region Approve and Reject Masters
        private void DoApproveMaster(string trantype,
                                    string keyfld,
                                    GridView grdvw)
        {
            int keyvalue = Convert.ToInt32(grdvw.GetRowCellValue(grdvw.FocusedRowHandle, keyfld));

            DataRow drRow = (from DataRow row in DtMastData.Rows
                             where Convert.ToInt32(row[keyfld]) == keyvalue
                             select row).FirstOrDefault();

            drRow["Apgen"] = "Approved";
            drRow["LevelStatus"] = "Approved";


            Size maxWindowTrackSize = SystemInformation.MaxWindowTrackSize;
            Point Pt = grid.PointToClient(Control.MousePosition);
            MLAAppRemarks AppRemarks = new MLAAppRemarks();
            int mX = 0;
            if ((maxWindowTrackSize.Width + AppRemarks.Width) > Pt.X)
                mX = maxWindowTrackSize.Width - AppRemarks.Width - 50;

            AppRemarks.DesktopLocation = new Point(mX, Pt.Y);
            AppRemarks.Text = "Approval Comments";
            AppRemarks.Remarks = Convert.ToString(drRow["Approved_Remarks"]).Trim();

            if (AppRemarks.ShowDialog() == DialogResult.OK)
            {
                drRow["Approved_Remarks"] = AppRemarks.Remarks;
                drRow["Rejected_Remarks"] = string.Empty;
            }

            grdvw.RefreshData();

        }

        private void DoRejectMaster(string trantype,
                                    string keyfld,
                                    GridView grdvw)
        {
            int keyvalue = Convert.ToInt32(grdvw.GetRowCellValue(grdvw.FocusedRowHandle, keyfld));

            DataRow drRow = (from DataRow row in DtMastData.Rows
                             where Convert.ToInt32(row["tran_cd"]) == keyvalue
                             select row).FirstOrDefault();

            drRow["Apgen"] = "Rejected";
            drRow["LevelStatus"] = "Rejected";

            Size maxWindowTrackSize = SystemInformation.MaxWindowTrackSize;
            Point Pt = grid.PointToClient(Control.MousePosition);
            MLAAppRemarks AppRemarks = new MLAAppRemarks();
            int mX = 0;
            if ((maxWindowTrackSize.Width + AppRemarks.Width) > Pt.X)
                mX = maxWindowTrackSize.Width - AppRemarks.Width - 50;

            AppRemarks.DesktopLocation = new Point(mX, Pt.Y);
            AppRemarks.Text = "Rejected Comments";
            AppRemarks.Remarks = Convert.ToString(drRow["Rejected_Remarks"]).Trim();

            if (AppRemarks.ShowDialog() == DialogResult.OK)
            {
                drRow["Rejected_Remarks"] = AppRemarks.Remarks;
                drRow["Approved_Remarks"] = string.Empty;
            }

            grdvw.RefreshData();

        }
        #endregion



        private void notApproveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            string a = "";
            string text = "";
            string text2 = "";
            if (this.grdIMvw.DataSource != null)
                if (System.Convert.ToString(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "entry_ty").ToString().Trim()) == "IM")
                {
                    text = System.Convert.ToString(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "entry_ty"));
                    num = System.Convert.ToInt32(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "tran_cd"));
                    text2 = string.Concat(new object[]
                    {
                "select entry_ty,tran_cd,applevel,prelevel,levelstatus from applevel_dtl where levelstatus='Approved' and prelevelstatus ='Approved' and prelevel<>0 and entry_ty='",
                text,
                "' and tran_cd=",
                num,
                " order by tran_cd"
                    });
                    this.applevel_dtl = cls_Sqlhelper.ExecuteDataset(MLAApproval.Connectionstring, System.Data.CommandType.Text, text2);

                    num2 = System.Convert.ToInt32(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "applevel"));
                    num3 = System.Convert.ToInt32(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "prelevel"));
                    a = System.Convert.ToString(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "levelstatus"));
                }
            if (this.grdAMvw.DataSource != null)
            {
                if (System.Convert.ToString(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "entry_ty").ToString().Trim()) == "AM")
                {
                    text = System.Convert.ToString(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "entry_ty"));
                    num = System.Convert.ToInt32(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "tran_cd"));
                    text2 = string.Concat(new object[]
                    {
                "select entry_ty,tran_cd,applevel,prelevel,levelstatus from applevel_dtl where levelstatus='Approved' and prelevelstatus ='Approved' and prelevel<>0 and entry_ty='",
                text,
                "' and tran_cd=",
                num,
                " order by tran_cd"
                    });
                    this.applevel_dtl = cls_Sqlhelper.ExecuteDataset(MLAApproval.Connectionstring, System.Data.CommandType.Text, text2);

                    num2 = System.Convert.ToInt32(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "applevel"));
                    num3 = System.Convert.ToInt32(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "prelevel"));
                    a = System.Convert.ToString(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "levelstatus"));
                }
            }
            else
            {
                text = System.Convert.ToString(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "entry_ty"));
                num = System.Convert.ToInt32(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "tran_cd"));
                text2 = string.Concat(new object[]
                {
                "select entry_ty,tran_cd,applevel,prelevel,levelstatus from applevel_dtl where levelstatus='Approved' and prelevelstatus ='Approved' and prelevel<>0 and entry_ty='",
                text,
                "' and tran_cd=",
                num,
                " order by tran_cd"
                });
                this.applevel_dtl = cls_Sqlhelper.ExecuteDataset(MLAApproval.Connectionstring, System.Data.CommandType.Text, text2);

                num2 = System.Convert.ToInt32(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "applevel"));
                num3 = System.Convert.ToInt32(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "prelevel"));
                a = System.Convert.ToString(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "levelstatus"));
            }
            foreach (System.Data.DataRow dataRow in this.applevel_dtl.Tables[0].Rows)
            {
                if (text == dataRow["entry_ty"].ToString() && num == System.Convert.ToInt32(dataRow["tran_cd"]) && num2 > System.Convert.ToInt32(dataRow["applevel"]) && a == "Approved")
                {
                    System.Windows.Forms.MessageBox.Show("This transaction is already has been approved by upper level " + System.Environment.NewLine + "You can not change status..!!", "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return;
                }
            }
            string[] array = System.Convert.ToString(this.treevwEntities.FocusedNode.Tag).Trim().Split(new char[]
            {
                ','
            });
            string text3 = array[0].Trim();
            if (text3 == "IM")
            {
                this.DoRejectMaster(text3, "it_code", this.grdIMvw);
            }
            else if (text3 == "AM")
            {
                this.DoRejectMaster(text3, "ac_id", this.grdAMvw);
            }
            else
            {
                this.DoRejectTransaction();
            }
        }

        private void commentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            string a = "";
            string text = "";
            string text2 = "";
            if (this.grdIMvw.DataSource != null) //Added By Shrikant S. on 26/06/2018 for Bug-30904
            {
                if (System.Convert.ToString(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "entry_ty").ToString().Trim()) == "IM")
                {
                    text = System.Convert.ToString(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "entry_ty"));
                    num = System.Convert.ToInt32(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "tran_cd"));
                    text2 = string.Concat(new object[]
                    {
                "select entry_ty,tran_cd,applevel,prelevel,levelstatus from applevel_dtl where levelstatus='Approved' and prelevelstatus ='Approved' and prelevel<>0 and entry_ty='",
                text,
                "' and tran_cd=",
                num,
                " order by tran_cd"
                    });
                    this.applevel_dtl = cls_Sqlhelper.ExecuteDataset(MLAApproval.Connectionstring, System.Data.CommandType.Text, text2);

                    num2 = System.Convert.ToInt32(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "applevel"));
                    num3 = System.Convert.ToInt32(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "prelevel"));
                    a = System.Convert.ToString(this.grdIMvw.GetRowCellValue(this.grdIMvw.FocusedRowHandle, "levelstatus"));
                    a = "IM";           //Added By Shrikant S. on 03/08/2018 for Bug-30904
                }

            }       //Added By Shrikant S. on 26/06/2018 for Bug-30904
            if (this.grdAMvw.DataSource != null) //Added By Shrikant S. on 26/06/2018 for Bug-30904
            {
                if (System.Convert.ToString(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "entry_ty").ToString().Trim()) == "AM")            //added by Shrikant S. on 26/06/2018 for Bug-30904
                //  else if (System.Convert.ToString(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "entry_ty").ToString().Trim()) == "AM")        //Commented by Shrikant S. on 26/06/2018 for Bug-30904
                {
                    text = System.Convert.ToString(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "entry_ty"));
                    num = System.Convert.ToInt32(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "tran_cd"));
                    text2 = string.Concat(new object[]
                    {
                "select entry_ty,tran_cd,applevel,prelevel,levelstatus from applevel_dtl where levelstatus='Approved' and prelevelstatus ='Approved' and prelevel<>0 and entry_ty='",
                text,
                "' and tran_cd=",
                num,
                " order by tran_cd"
                    });
                    this.applevel_dtl = cls_Sqlhelper.ExecuteDataset(MLAApproval.Connectionstring, System.Data.CommandType.Text, text2);

                    num2 = System.Convert.ToInt32(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "applevel"));
                    num3 = System.Convert.ToInt32(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "prelevel"));
                    a = System.Convert.ToString(this.grdAMvw.GetRowCellValue(this.grdAMvw.FocusedRowHandle, "levelstatus"));
                    a = "AM";           //Added By Shrikant S. on 03/08/2018 for Bug-30904
                }
            }   //Added By Shrikant S. on 26/06/2018 for Bug-30904
            else
            {
                text = System.Convert.ToString(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "entry_ty"));
                num = System.Convert.ToInt32(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "tran_cd"));
                text2 = string.Concat(new object[]
                {
                "select entry_ty,tran_cd,applevel,prelevel,levelstatus from applevel_dtl where levelstatus='Approved' and prelevelstatus ='Approved' and prelevel<>0 and entry_ty='",
                text,
                "' and tran_cd=",
                num,
                " order by tran_cd"
                });
                this.applevel_dtl = cls_Sqlhelper.ExecuteDataset(MLAApproval.Connectionstring, System.Data.CommandType.Text, text2);

                num2 = System.Convert.ToInt32(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "applevel"));
                num3 = System.Convert.ToInt32(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "prelevel"));
                a = System.Convert.ToString(this.grdvwMain.GetRowCellValue(this.grdvwMain.FocusedRowHandle, "levelstatus"));
            }
            foreach (System.Data.DataRow dataRow in this.applevel_dtl.Tables[0].Rows)
            {
                if (text == dataRow["entry_ty"].ToString() && num == System.Convert.ToInt32(dataRow["tran_cd"]) && num2 > System.Convert.ToInt32(dataRow["applevel"]) && a == "Approved")
                {
                    System.Windows.Forms.MessageBox.Show("This transaction is already has been approved by upper level " + System.Environment.NewLine + "You can not change status..!!", "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return;
                }
            }
            string[] array = System.Convert.ToString(this.treevwEntities.FocusedNode.Tag).Trim().Split(new char[]
            {
                ','
            });
            if (a == "IM")
            {
                this.DoComment(this.grdIMvw, "it_code", this.DtMastData);
            }
            else if (a == "AM")
            {
                this.DoComment(this.grdAMvw, "ac_id", this.DtMastData);
            }
            else
            {
                this.DoComment(this.grdvwMain, "tran_cd", this.DtMainvw);
            }
        }

        private void DoComment(GridView grdview,
                               string keyfld,
                               DataTable dtView)
        {
            int keyvalue = Convert.ToInt32(grdview.GetRowCellValue(grdview.FocusedRowHandle, keyfld));
            DataRow drRow = (from DataRow row in dtView.Rows
                             where Convert.ToInt32(row[keyfld]) == keyvalue
                             select row).FirstOrDefault();
            if (Convert.ToString(drRow["Apgen"]).Trim() != "Approved" && Convert.ToString(drRow["Apgen"]).Trim() != "Rejected")
            {
                //MessageBox.Show("Please do approve or reject transaction first...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);     //Commented by Shrikant S. on 03/0/2018 for Bug-30904
                MessageBox.Show("Please do approve or reject record first...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);        //Added by Shrikant S. on 03/0/2018 for Bug-30904
                return;
            }
            string caption = string.Empty;
            string remarks = string.Empty;

            if (Convert.ToString(drRow["Apgen"]).Trim() == "Approved")
            {
                caption = "Approval Comments";
                remarks = Convert.ToString(drRow["Approved_Remarks"]).Trim();
            }
            else if (Convert.ToString(drRow["Apgen"]).Trim() == "Rejected")
            {
                caption = "Rejected Comments";
                remarks = Convert.ToString(drRow["Rejected_Remarks"]).Trim();
            }
            Size maxWindowTrackSize = SystemInformation.MaxWindowTrackSize;
            Point Pt = grid.PointToClient(Control.MousePosition);
            MLAAppRemarks AppRemarks = new MLAAppRemarks();
            int mX = 0;
            if ((maxWindowTrackSize.Width + AppRemarks.Width) > Pt.X)
                mX = maxWindowTrackSize.Width - AppRemarks.Width - 50;
            AppRemarks.DesktopLocation = new Point(mX, Pt.Y);
            AppRemarks.Text = caption;
            AppRemarks.Remarks = remarks;
            AppRemarks.ShowDialog();

            if (Convert.ToString(drRow["Apgen"]).Trim() == "Approved")
            {
                drRow["Approved_Remarks"] = AppRemarks.Remarks;
                drRow["Rejected_Remarks"] = string.Empty;
            }
            else if (Convert.ToString(drRow["Apgen"]).Trim() == "Rejected")
            {
                drRow["Rejected_Remarks"] = AppRemarks.Remarks;
                drRow["Approved_Remarks"] = string.Empty;
            }
            grdview.RefreshData();
        }


        private void DoSave(string trantype)
        {
            if (treevwEntities.Nodes.Count <= 0)            //Added by Shrikant S. on 23/08/2018 for Installer 2.0.0            
                return;                                     //Added by Shrikant S. on 23/08/2018 for Installer 2.0.0    

            try
            {
                if (trantype == "IM" || trantype == "AM")
                {
                    if (DtMastData.DataSet.HasChanges() == false)
                    {
                        MessageBox.Show("Changes not found for saving....!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                if (trantype == "IM")
                {
                    m_obj_Approval.Save(DtMastData, DtApplevel_Mstr, trantype, UserID);
                    DtMastData.DataSet.AcceptChanges();
                    BindingMasterGrid(trantype, "it_code", grdIM, true);
                }
                else if (trantype == "AM")
                {
                    m_obj_Approval.Save(DtMastData, DtApplevel_Mstr, trantype, UserID);
                    DtMastData.DataSet.AcceptChanges();
                    BindingMasterGrid(trantype, "ac_id", grdAM, true);
                }
                else
                {
                    if (DtMainvw.DataSet.HasChanges() == false)
                    {
                        MessageBox.Show("Changes not found for saving....!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    m_obj_Approval.Save(DtMainvw, DtApplevel_Mstr, trantype, UserID);
                    DtMainvw.DataSet.AcceptChanges();
                    BindingTransactionGrid(trantype, true);
                }
                MessageBox.Show("Successfully saved data...!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (m_obj_Approval.PvEmailExists)  //Added By Kishor A. for Bug-21965 on-08-11-2016
                {
                    if (m_obj_Approval.ListAppmailSetter.Count > 0)
                    {
                        MLAAppMailStatus AppMailStatus = new MLAAppMailStatus();
                        AppMailStatus.FrmIcon = IconPath; //Added By Kishor A. for Bug-21965 on-08-11-2016
                        AppMailStatus.Connectionstring = Connectionstring;
                        AppMailStatus.ListAppMailSetter = m_obj_Approval.ListAppmailSetter;
                        AppMailStatus.Show();
                    }
                }  //Added By Kishor A. for Bug-21965 on-08-11-2016
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            DoSave(GetSelectedNodeTranType());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            formstatus = "cancel";
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (treevwEntities.Nodes.Count <= 0)            //Added by Shrikant S. on 23/08/2018 for Installer 2.0.0            
                return;                                     //Added by Shrikant S. on 23/08/2018 for Installer 2.0.0    

            if (formstatus == "cancel" )              
            {
                string[] nodetag = Convert.ToString(treevwEntities.FocusedNode.Tag).Trim().Split(',');
                string trantype = nodetag[0].Trim();
                if (trantype != "AM" || trantype != "IM")
                {
                    if (DtMainvw != null)
                    {
                        if (DtMainvw.Rows.Count > 0)            //Added by Shrikant S. on 09/08/2018 for Bug-30904
                        {
                            if (DtMainvw.DataSet.HasChanges() == true)
                            {
                                if (MessageBox.Show("Changes has found..., Do you want to Save the Changes ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    DoSave(prevtrantype);
                                else
                                    DtMainvw.DataSet.RejectChanges();
                            }
                        }
                    }
                }

                var dr = MessageBox.Show("Want to Exit ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void MLAApproval_Activated(object sender, EventArgs e)
        {
            if (isActivate == false)
            {
                paneltoggle = true;
                timer.Enabled = true;
                isActivate = true;
            }
        }

        private void dtPickFrom_Validated(object sender, EventArgs e)
        {
            if (dtPickFrom.Text == string.Empty || dtPickTo.Text == string.Empty)
                return;

            if (GetSelectedNodeTranType() == "IM" || GetSelectedNodeTranType() == "AM")
                return;

            DateTime dtFrom = Convert.ToDateTime(dtPickFrom.Text);
            DateTime dtTo = Convert.ToDateTime(dtPickTo.Text);

            if (dtFrom > dtTo)
            {
                errorProvider.SetError(dtPickFrom, "From date cannot be Greater than To Date");
                return;
            }

            treevwEntities_Click(sender, e);
        }

        private void dtPickTo_Validated(object sender, EventArgs e)
        {
            if (dtPickFrom.Text == string.Empty || dtPickTo.Text == string.Empty)
                return;

            if (GetSelectedNodeTranType() == "IM" || GetSelectedNodeTranType() == "AM")
                return;

            DateTime dtFrom = Convert.ToDateTime(dtPickFrom.Text);
            DateTime dtTo = Convert.ToDateTime(dtPickTo.Text);

            if (dtTo < dtFrom)
            {
                errorProvider.SetError(dtPickTo, "To date cannot be Less than From Date");
                return;
            }

            treevwEntities_Click(sender, e);
        }

        private string GetSelectedNodeTranType()
        {
            if (treevwEntities.Nodes.Count <= 0)            //added by Shrikant S. on 23/08/2018 for Installer 2.0.0
                return string.Empty;                        //added by Shrikant S. on 23/08/2018 for Installer 2.0.0

            string[] nodetag = Convert.ToString(treevwEntities.FocusedNode.Tag).Trim().Split(',');
            string trantype = nodetag[0].Trim();
            return trantype;
        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFilter();
        }

        private void SetFilter()
        {
            string trantype = GetSelectedNodeTranType();
            string status = cboStatus.SelectedItem.ToString().Trim();

            if (status == "All")
                status = "Waiting for Approval'";

            if (trantype == "AM")
            {
                grdAMvw.Columns["apgen"].FilterInfo = new ColumnFilterInfo("[apgen] = '" + status + "'");
            }
            else if (trantype == "IM")
            {
                grdIMvw.Columns["apgen"].FilterInfo = new ColumnFilterInfo("[apgen] = '" + status + "'");
            }
            else
            {
                if (cboStatus.SelectedItem.ToString().Trim() == "All")
                    status = "Waiting for Approval'";

                grdvwMain.Columns["apgen"].FilterInfo = new ColumnFilterInfo("[apgen] = '" + status + "'");

            }


            //if (status == "All")
            //    status = "Waiting for Approval' or [apgen] = 'Approved' or [apgen] = 'Rejected";

            //if (trantype == "AM")
            //{
            //    grdAMvw.Columns["apgen"].FilterInfo = new ColumnFilterInfo("[apgen] = '" + status + "'");
            //}
            //else if (trantype == "IM")
            //{
            //    grdIMvw.Columns["apgen"].FilterInfo = new ColumnFilterInfo("[apgen] = '" + status + "'");
            //}
            //else
            //{
            //    if (cboStatus.SelectedItem.ToString().Trim() == "All")
            //        status = "Waiting for Approval' or [apgen] = 'Approved' or [apgen] = 'Rejected";

            //    grdvwMain.Columns["apgen"].FilterInfo = new ColumnFilterInfo("[apgen] = '" + status + "'");
            //}
        }

        private void grdvw_customdrawcell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

            if (e.Column.FieldName.ToLower().Trim() == "apgen")
            {
                e.Handled = true;
                Image img = null;
                GridView grdvw = sender as GridView;

                string value = CheckCondition(e.RowHandle, e.Column.FieldName, grdvw);

                if (value.Trim() == "Approved")
                    img = Properties.Resources.Ok_icon;
                else if (value.Trim() == "Waiting for Approval")
                    img = Properties.Resources.alert_icon;
                else if (value.Trim() == "Rejected")
                    img = Properties.Resources.Action_cancel_icon;

                if (img != null)
                {
                    e.Graphics.DrawImage(img, new Rectangle(e.Bounds.X, e.Bounds.Y, 17, 17));
                    e.Appearance.DrawString(e.Cache, e.DisplayText, new Rectangle(e.Bounds.X + 30, e.Bounds.Y, e.Bounds.Width - 20, e.Bounds.Height));
                }

            }
            else
            {
                if (e.Column.FieldName.ToLower().Trim() == "lastlevelstatus")
                {
                    e.Handled = true;
                    Image img = null;
                    GridView grdvw = sender as GridView;

                    string value = CheckCondition(e.RowHandle, e.Column.FieldName, grdvw);

                    if (value.Trim().IndexOf("Approved") >= 0)
                        img = Properties.Resources.Ok_icon;
                    else if (value.Trim().IndexOf("Rejected") >= 0)
                        img = Properties.Resources.Action_cancel_icon;

                    if (img != null)
                    {
                        e.Graphics.DrawImage(img, new Rectangle(e.Bounds.X, e.Bounds.Y, 17, 17));
                        e.Appearance.DrawString(e.Cache, e.DisplayText, new Rectangle(e.Bounds.X + 30, e.Bounds.Y, e.Bounds.Width - 20, e.Bounds.Height));
                    }

                }

            }



        }

        Point CalcPosition(RowCellCustomDrawEventArgs e, Image img)
        {
            Point p = new Point();
            p.X = e.Bounds.Location.X + (e.Bounds.Width - img.Width) / 2;
            p.Y = e.Bounds.Location.Y + (e.Bounds.Height - img.Height) / 2;
            return p;
        }


        private string CheckCondition(int row, string colName, GridView grdvw)
        {
            string value = Convert.ToString(grdvw.GetRowCellValue(row, colName));
            return value;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            paneltoggle = false;
        }


        private void grdvwMain_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            string capplevel = Convert.ToString(grdvwMain.GetRowCellValue(e.FocusedRowHandle, "applevel"));
            string cinitiator = Convert.ToString(grdvwMain.GetRowCellValue(e.FocusedRowHandle, "initiator_id"));

            tsLevel.Text = "User Approval Level : " + capplevel;
            tsUser.Text = "Initiator :" + cinitiator;
        }

        private void zoomInTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] nodetag = Convert.ToString(treevwEntities.FocusedNode.Tag).Trim().Split(',');
            string trantype = nodetag[0].Trim();

            if (trantype == "IM")
                ZoomInTransaction(grdIMvw, trantype, "it_code");
            else if (trantype == "AM")
                ZoomInTransaction(grdAMvw, trantype, "ac_id");
            else
                ZoomInTransaction(grdvwMain, trantype, "tran_cd");

        }

        private void ZoomInTransaction(GridView grdvw, string trantype, string keyfld)
        {
            //int keyvalue = 0;
            //keyvalue = Convert.ToInt32(grdvw.GetRowCellValue(grdvw.FocusedRowHandle, keyfld));
            //vunettofx.ClNetToFxClass netToFx = new vunettofx.ClNetToFxClass();
            ////this.WindowState = FormWindowState.Minimized;
            ////netToFx.PrNetToFx(DbName, Startdt.ToString("dd/MM/yyyy"), Enddt.ToString("dd/MM/yyyy"), UserID, AppPath, DefaPath, IconPath, ProdCode, "DO ToUeVoucher WITH '" + trantype + "'," + Convert.ToString(keyvalue).Trim() + ",Thisform.DataSessionId,.T.");
            //object callvfpobject = new object();
            //callvfpobject = new vunettofx.ClNetToFxClass().PrNetToFx(DbName, Startdt.ToString("dd/MM/yyyy"), Enddt.ToString("dd/MM/yyyy"), UserID, AppPath, DefaPath, IconPath, ProdCode, "DO ToUeVoucher WITH '" + trantype + "'," + Convert.ToString(keyvalue).Trim() + ",Thisform.DataSessionId,.T.");

            var thread = new System.Threading.Thread(ShowUI);
            thread.Name = "ZoomIn";

            thread.Start();


        }

        private void ShowUI()
        {
            string[] nodetag = Convert.ToString(treevwEntities.FocusedNode.Tag).Trim().Split(',');
            string trantype = nodetag[0].Trim();
            int keyvalue = 0;
            keyvalue = Convert.ToInt32(grdvwMain.GetRowCellValue(grdvwMain.FocusedRowHandle, "tran_cd"));
            object callvfpobject = new object();
            callvfpobject = new vunettofx.ClNetToFx().PrNetToFx(DbName, Startdt.ToString("dd/MM/yyyy"), Enddt.ToString("dd/MM/yyyy"), UserID, AppPath, DefaPath, IconPath, ProdCode, "DO ToUeVoucher WITH '" + trantype + "'," + Convert.ToString(keyvalue).Trim() + ",Thisform.DataSessionId,.T.");
        }

        private void conTxtMenu_Opening(object sender, CancelEventArgs e)
        {
            if (treevwEntities.Nodes.Count <= 0)        //Added by Shrikant S. on 23/08/2018 for Installer 2.0.0
                return;                                 //Added by Shrikant S. on 23/08/2018 for Installer 2.0.0

            string[] nodetag = Convert.ToString(treevwEntities.FocusedNode.Tag).Trim().Split(',');
            string trantype = nodetag[0].Trim();

            if (trantype == "AM" || trantype == "IM")
                zoomInTransactionToolStripMenuItem.Visible = false;
            else
                zoomInTransactionToolStripMenuItem.Visible = true;
        }

        private void btnSave_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
