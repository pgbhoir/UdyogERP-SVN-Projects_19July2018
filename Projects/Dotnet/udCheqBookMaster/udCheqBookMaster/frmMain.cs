using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using ueconnect;

namespace udCheqBookMaster
{
    public partial class frmMain : uBaseForm.FrmBaseForm
    {
        DataSet vDsCommon;
        DataAccess_Net.clsDataAccess oDataAccess;
        string ServiceType = string.Empty;

        DataTable tblChqMaster, tblChqDet;
        static int refresh;
        string vMainTblNm = "ChequeBookMaster";
        string vSqlStr = "";

        String cAppPId, cAppName;
        short vTimeOut = 50;

        string vLoc_Code = string.Empty, vBook_Id = "0";
        string filepath;

        public frmMain(string[] args)
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            DataAccess_Net.clsDataAccess oDataAccess;
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Cheque Management Master";
            this.pCompId = Convert.ToInt16(args[0]);
            this.pComDbnm = args[1];
            this.pServerName = args[2];
            this.pUserId = args[3];
            this.pPassword = args[4];
            this.pPApplRange = args[5];
            this.pAppUerName = args[6];
            Icon MainIcon = new System.Drawing.Icon(args[7].Replace("<*#*>", " "));
             this.pFrmIcon = MainIcon;
            this.pPApplText = args[8].Replace("<*#*>", " ");
            this.pPApplName = args[9];
            this.pPApplPID = Convert.ToInt16(args[10]);
            this.pPApplCode = args[11];
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            DataSet vDsCommon;
            this.pAppPath = Application.ExecutablePath;
            this.pAppPath = Path.GetDirectoryName(this.pAppPath);
            this.mthControlSet();

            this.btnEdit.Enabled = false;
            this.btnExit.Enabled = false;

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.mthDsCommon();
            this.SetFormColor();
            this.SetMenuRights();
            this.mInsertProcessIdRecord();

            clsConnect oConnect;
            string startupPath = Application.StartupPath;

            oConnect = new clsConnect();
            GetInfo.iniFile ini = new GetInfo.iniFile(startupPath + "\\" + "Visudyog.ini");
            string appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
            oConnect.InitProc("'" + startupPath + "'", appfile);
            DirectoryInfo dir = new DirectoryInfo(startupPath);
            Array totalFile = dir.GetFiles();
            string registerMePath = string.Empty;
            for (int i = 0; i < totalFile.Length; i++)
            {
                string fname = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                if (Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name).ToUpper().Contains("REGISTER.ME"))
                {
                    registerMePath = Path.GetFileName(((System.IO.FileInfo[])(totalFile))[i].Name);
                    break;
                }

            }
            if (registerMePath == string.Empty)
            {
                ServiceType = "";
            }
            else
            {
                string[] objRegisterMe = (oConnect.ReadRegiValue(startupPath)).Split('^');
                ServiceType = objRegisterMe[15].ToString().Trim();
            }

            this.mthView();
            button1.Enabled = false;
            lblProgress.Visible = false;
            progressBar1.Visible = false;

        }
        private void mthView()
        {
            this.pAddMode = false;
            this.pEditMode = false;

            if (ServiceType.ToUpper() == "VIEWER VERSION")
            {
                this.btnNew.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCancel.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPreview.Enabled = false;
                this.btnPrint.Enabled = false;

            }
            else
            {

                mthEnableDisableFormControls();
                this.mthGrdRefresh();
                this.mthChkNavigationButton();
            }

            this.dgvChequeDet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));



        }
        private void mthGrdRefresh()
        {

            vSqlStr = "Execute [USP_Ent_ChequeBook_Master] " + "'" + this.txtLocNm.Text + "','" + this.txtBankName.Text.Trim() + "'," + (this.chkClosedBook.Checked == true ? "'Y'" : "'N'");

            tblChqMaster = oDataAccess.GetDataTable(vSqlStr, null, 25);

            //RRG
            int i = 0;
            foreach (DataRow dr in tblChqMaster.Rows)
            {
                DataRow[] dr1;
                int Book_id = Convert.ToInt32(dr["Book_id"].ToString());
                string sqls = "Execute [USP_Ent_Cheque_Det] " + Book_id;
                DataTable dt_temp = new DataTable();
                dt_temp = oDataAccess.GetDataTable(sqls, null, 25);

                dr1 = dt_temp.Select("Status='Available'");
                int AvailableCount = dr1.Length;
                dr1 = dt_temp.Select("Status='Dishonor/Cancelled'");
                int CancelCount = dr1.Length;
                dr1 = dt_temp.Select("Status='Issued'");
                int IssuedCount = dr1.Length;
                dr1 = dt_temp.Select("Status='PDC'");
                int PDCCount = dr1.Length;
                dr1 = dt_temp.Select("Status='Reconciled'");
                int ReconciledCount = dr1.Length;
                dr1 = dt_temp.Select("Status='Reco.Pending'");
                int unRecoCount = dr1.Length;
                int totalCount = dt_temp.Rows.Count;

                tblChqMaster.Rows[i]["Available"] = AvailableCount;
                tblChqMaster.Rows[i]["Cancelled"] = CancelCount;
                tblChqMaster.Rows[i]["Issue"] = IssuedCount;
                tblChqMaster.Rows[i]["PDC"] = PDCCount;
                tblChqMaster.Rows[i]["Reco"] = ReconciledCount;
                tblChqMaster.Rows[i]["UnReco"] = unRecoCount;
                tblChqMaster.Rows[i]["TotalChq"] = totalCount;


                i++;
            }

            if (refresh == 1)
            {
                if (chkClosedBook.Checked == true)
                {
                    DataRow[] result = tblChqMaster.Select("Available > 0", "Book_id ASC");
                    if (result.Length > 0)
                    {
                        tblChqMaster = result.CopyToDataTable();
                    }
                }

            }

            //RRG

            this.dgvChequeDet.Columns.Clear();
            this.dgvChequeDet.DataSource = this.tblChqMaster;
            this.dgvChequeDet.Columns.Clear();

            System.Windows.Forms.DataGridViewCheckBoxColumn colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            //this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            colSelect.HeaderText = "";
            colSelect.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSelect.Width = 40;
            colSelect.Name = "colSelect";
            colSelect.ReadOnly = false;
            this.dgvChequeDet.Columns.Add(colSelect);

            System.Windows.Forms.DataGridViewTextBoxColumn colBankNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBankNm.HeaderText = "Bank Name";
            colBankNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colBankNm.Width = 100;
            colBankNm.Name = "colBankNm";
            colBankNm.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colBankNm);

            System.Windows.Forms.DataGridViewTextBoxColumn colChqFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colChqFrom.HeaderText = "Cheque From";
            colChqFrom.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colChqFrom.Width = 100;
            colChqFrom.Name = "colChqFrom";
            colChqFrom.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colChqFrom);

            System.Windows.Forms.DataGridViewTextBoxColumn colChqto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colChqto.HeaderText = "Cheque To";
            colChqto.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colChqto.Width = 100;
            colChqto.Name = "colChqto";
            colChqto.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colChqto);

            //System.Windows.Forms.DataGridViewTextBoxColumn colBookActDt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colBookActDt.HeaderText = "Book Activate From";
            //colBookActDt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            //colBookActDt.Width = 130;
            //colBookActDt.Name = "colBookActDt";
            //colBookActDt.ReadOnly = true;
            //this.dgvChequeDet.Columns.Add(colBookActDt);

            //System.Windows.Forms.DataGridViewTextBoxColumn colBookDeActDt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colBookDeActDt.HeaderText = "Book DeActivate From";
            //colBookDeActDt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            //colBookDeActDt.Width = 140;
            //colBookDeActDt.Name = "colBookDeActDt";
            //colBookDeActDt.ReadOnly = true;
            //this.dgvChequeDet.Columns.Add(colBookDeActDt);

            System.Windows.Forms.DataGridViewTextBoxColumn colAva = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colAva.HeaderText = "Available";
            colAva.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colAva.Width = 100;
            colAva.Name = "colAva";
            colAva.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colAva);

            System.Windows.Forms.DataGridViewTextBoxColumn colTotalChq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTotalChq.HeaderText = "Total Cheque";
            colTotalChq.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colTotalChq.Width = 100;
            colTotalChq.Name = "colTotalChq";
            colTotalChq.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colTotalChq);

            System.Windows.Forms.DataGridViewTextBoxColumn colIssue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colIssue.HeaderText = "Issued";
            colIssue.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colIssue.Width = 100;
            colIssue.Name = "colIssue";
            colIssue.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colIssue);

            System.Windows.Forms.DataGridViewTextBoxColumn colCancelled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCancelled.HeaderText = "Dishonor/Cancelled";
            colCancelled.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colCancelled.Width = 100;
            colCancelled.Name = "colCancelled";
            colCancelled.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colCancelled);

            System.Windows.Forms.DataGridViewTextBoxColumn colReco = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colReco.HeaderText = "Reconciled";
            colReco.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colReco.Width = 100;
            colReco.Name = "colReco";
            colReco.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colReco);

            //System.Windows.Forms.DataGridViewTextBoxColumn ColUnReco = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //ColUnReco.HeaderText = "Reco.Pending";
            //ColUnReco.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            //ColUnReco.Width = 100;
            //ColUnReco.Name = "ColUnReco";
            //ColUnReco.ReadOnly = true;
            //this.dgvChequeDet.Columns.Add(ColUnReco);

            System.Windows.Forms.DataGridViewTextBoxColumn colPDC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPDC.HeaderText = "PDC";
            colPDC.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colPDC.Width = 100;
            colPDC.Name = "colPDC";
            colPDC.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colPDC);

            System.Windows.Forms.DataGridViewTextBoxColumn colTrnSeries = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTrnSeries.HeaderText = "Transaction Series";
            colTrnSeries.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colTrnSeries.Width = 150;
            colTrnSeries.Name = "colTrnSeries";
            colTrnSeries.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colTrnSeries);

            System.Windows.Forms.DataGridViewTextBoxColumn colLoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLoc.HeaderText = "Location";
            colLoc.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colLoc.Width = 100;
            colLoc.Name = "colLoc";
            colLoc.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colLoc);

            System.Windows.Forms.DataGridViewTextBoxColumn colBookId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBookId.HeaderText = "Book_Id";
            colBookId.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colBookId.Width = 100;
            colBookId.Name = "colBookId";
            colBookId.ReadOnly = true;
            colBookId.Visible = false;
            this.dgvChequeDet.Columns.Add(colBookId);

            System.Windows.Forms.DataGridViewTextBoxColumn colBookActDt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBookActDt.HeaderText = "Book Activate From";
            colBookActDt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colBookActDt.Width = 130;
            colBookActDt.Name = "colBookActDt";
            colBookActDt.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colBookActDt);

            System.Windows.Forms.DataGridViewTextBoxColumn colBookDeActDt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBookDeActDt.HeaderText = "Book DeActivate From";
            colBookDeActDt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colBookDeActDt.Width = 140;
            colBookDeActDt.Name = "colBookDeActDt";
            colBookDeActDt.ReadOnly = true;
            this.dgvChequeDet.Columns.Add(colBookDeActDt);


            dgvChequeDet.Columns["colSelect"].DataPropertyName = "sel";
            dgvChequeDet.Columns["colBankNm"].DataPropertyName = "Bank_Nm";
            dgvChequeDet.Columns["colChqFrom"].DataPropertyName = "Chq_from";
            dgvChequeDet.Columns["colChqTo"].DataPropertyName = "Chq_to";
            dgvChequeDet.Columns["colBookActDt"].DataPropertyName = "Book_Act_Dt";
            dgvChequeDet.Columns["colBookDeActDt"].DataPropertyName = "Book_Deact_Dt";
            dgvChequeDet.Columns["colTotalChq"].DataPropertyName = "TotalChq";
            dgvChequeDet.Columns["colAva"].DataPropertyName = "Available";
            dgvChequeDet.Columns["colIssue"].DataPropertyName = "Issue";
            dgvChequeDet.Columns["colCancelled"].DataPropertyName = "Cancelled";
            dgvChequeDet.Columns["colReco"].DataPropertyName = "Reco";
            // dgvChequeDet.Columns["colunReco"].DataPropertyName = "UnReco";
            dgvChequeDet.Columns["colPDC"].DataPropertyName = "PDC";
            dgvChequeDet.Columns["colTrnSeries"].DataPropertyName = "Inv_Sr";
            dgvChequeDet.Columns["colBookId"].DataPropertyName = "Book_Id";
            dgvChequeDet.Columns["colLoc"].DataPropertyName = "Location";

            dgvChequeDet.Refresh();

            dgvChequeDet.Columns[0].Frozen = true;

            //rrg
            Rectangle rect = dgvChequeDet.GetCellDisplayRectangle(0, -1, true);
            rect.Y = 4;
            rect.X = 16;
            CheckBox checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";

            checkboxHeader.Size = new Size(15, 15);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);
            dgvChequeDet.Controls.Add(checkboxHeader);

            //rrg
            refresh = 0;
        }



        private void btnLocNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            vSqlStr = "Select Loc_Code,Loc_Desc From Loc_Master   union Select '' , '' Order By Loc_Desc";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 20);
            DataView dvw = tblPop.DefaultView;

            VForText = "Select Location Name";
            vSearchCol = "Location Name";
            vDisplayColumnList = "Loc_Desc:Location Name, Loc_Code:Location Code";
            vReturnCol = "Loc_Desc,Loc_Code";
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
                this.txtLocNm.Text = oSelectPop.pReturnArray[0];
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    this.mthView();
                }
            }
        }


        private void mthEnableDisableFormControls()
        {
            Boolean vEnabled = false;
            if (this.pAddMode || this.pEditMode)
            {
                vEnabled = true;
            }
            this.txtLeaftcnt.Enabled = vEnabled;
            this.txtStartNo.Enabled = vEnabled;
            this.dtpActDt.Enabled = vEnabled;
            this.dtpDeAct.Enabled = vEnabled;
            this.btnTrnSeries.Enabled = vEnabled;

        }
        private void mthChkNavigationButton()
        {
            this.btnNew.Enabled = false;
            this.btnEdit.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;

            if (ServiceType.ToUpper() != "VIEWER VERSION")
            {
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    if (this.pAddButton)
                    {
                        this.btnNew.Enabled = true;
                    }
                    if (this.pEditButton)
                    {
                        if (this.txtBankName.Text == "")
                        {
                            this.btnEdit.Enabled = false;
                        }
                        else
                        {
                            this.btnEdit.Enabled = true;
                        }

                    }
                    if (this.pDeleteButton)
                    {
                        //this.btnDelete.Enabled = true;
                    }
                    btnLogout.Enabled = true;

                }
                else
                {
                    this.btnSave.Enabled = true;
                    this.btnCancel.Enabled = true;
                    btnLogout.Enabled = false;
                }
                //if (this.dsGrd.Tables[0].Rows.Count == 0 && this.pAddMode == false && this.pEditMode == false)
                //{
                //    this.btnEdit.Enabled = false;
                //    this.btnDelete.Enabled = false;
                //}
            }
        }
        private void mthControlSet()
        {
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnLocNm.Image = Image.FromFile(fName);
                this.btnBankName.Image = Image.FromFile(fName);
            }


        }

        private void mthDsCommon()
        {
            string vL_Yn = "";
            vDsCommon = new DataSet();
            DataTable company = new DataTable();
            company.TableName = "company";
            vSqlStr = "Select * From vudyog..Co_Mast where CompId=" + this.pCompId.ToString();
            company = oDataAccess.GetDataTable(vSqlStr, null, 25);
            if (vL_Yn == "")
            {
                vL_Yn = ((DateTime)company.Rows[0]["Sta_Dt"]).Year.ToString().Trim() + "-" + ((DateTime)company.Rows[0]["End_Dt"]).Year.ToString().Trim();
            }
            vDsCommon.Tables.Add(company);
            vDsCommon.Tables[0].TableName = "company";
            DataTable tblCoAdditional = new DataTable();
            tblCoAdditional.TableName = "coadditional";
            vSqlStr = "Select Top 1 * From Manufact";
            tblCoAdditional = oDataAccess.GetDataTable(vSqlStr, null, 25);
            vDsCommon.Tables.Add(tblCoAdditional);
            vDsCommon.Tables[1].TableName = "coadditional";

        }
        private void SetFormColor()
        {
            string colorCode = string.Empty;
            Color myColor = Color.Coral;

            if (vDsCommon.Tables["company"].Rows.Count > 0)
            {
                colorCode = vDsCommon.Tables["company"].Rows[0]["vcolor"].ToString();
            }

            if (vDsCommon.Tables["company"].Rows.Count > 0)
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                myColor = Color.FromArgb(Convert.ToInt32(colorCode.Trim()));
            }
            this.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);
            foreach (Control ctrl in this.Controls)
            {
                //ctrl.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);
            }


        }

        private void dgvChequeDet_DoubleClick(object sender, EventArgs e)
        {
            //Add RRG 
            this.vBook_Id = "0";
            tblChqDet = null;

            this.pAddMode = false;
            this.pEditMode = false;

            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();
            button1.Enabled = true;
            //End RRG 

            if (this.pAddMode == true || dgvChequeDet.Rows.Count == 0)
            {
                return;
            }

            //MessageBox.Show(this.dgvChequeDet.CurrentCell.Value.ToString());
            vBook_Id = dgvChequeDet.Rows[this.dgvChequeDet.CurrentCell.RowIndex].Cells["colBookId"].Value.ToString();
            vBook_Id = dgvChequeDet.Rows[this.dgvChequeDet.CurrentCell.RowIndex].Cells["colBookId"].Value.ToString();
            //MessageBox.Show(vBook_Id);
            this.txtBankName.Text = dgvChequeDet.Rows[this.dgvChequeDet.CurrentCell.RowIndex].Cells["colBankNm"].Value.ToString();
            this.txtStartNo.Text = dgvChequeDet.Rows[this.dgvChequeDet.CurrentCell.RowIndex].Cells["colChqFrom"].Value.ToString();
            this.txtEndNo.Text = dgvChequeDet.Rows[this.dgvChequeDet.CurrentCell.RowIndex].Cells["colChqTo"].Value.ToString();
            this.txtLeaftcnt.Text = dgvChequeDet.Rows[this.dgvChequeDet.CurrentCell.RowIndex].Cells["colTotalChq"].Value.ToString();//Convert.ToString(Convert.ToInt16(this.txtEndNo.Text) - Convert.ToInt16(this.txtStartNo.Text));
            this.dtpActDt.Text = dgvChequeDet.Rows[this.dgvChequeDet.CurrentCell.RowIndex].Cells["colBookActDt"].Value.ToString();
            this.dtpDeAct.Text = dgvChequeDet.Rows[this.dgvChequeDet.CurrentCell.RowIndex].Cells["colBookDeActDt"].Value.ToString();
            this.txtTrnSeries.Text = dgvChequeDet.Rows[this.dgvChequeDet.CurrentCell.RowIndex].Cells["colTrnSeries"].Value.ToString();
            this.txtLocNm.Text = dgvChequeDet.Rows[this.dgvChequeDet.CurrentCell.RowIndex].Cells["colLoc"].Value.ToString();
            Int32 vENo = Convert.ToInt32(this.txtStartNo.Text);
            Int32 vLNo = Convert.ToInt32(this.txtEndNo.pIntValue);

            this.txtLeaftcnt.Text = (vLNo - vENo + 1).ToString();

            tblChqDet = null;


            mthChkNavigationButton();
        }

        private void btnTrnSeries_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            vSqlStr = "Select Inv_sr From SERIES a,LCode b where (b.Entry_Ty='BP' or b.BCode_Nm='BP') and CHARINDEX(b.Entry_Ty,a.Validity)>1 ";

            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 20);
            DataView dvw = tblPop.DefaultView;

            VForText = "Select Transaction Series";
            vSearchCol = "Inv_sr";
            vDisplayColumnList = "Inv_sr:Transaction Series";
            vReturnCol = "Inv_Sr";
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
                this.txtTrnSeries.Text = oSelectPop.pReturnArray[0];
                // this.mthView();

            }
        }

        private void btnBankName_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            vSqlStr = "Select Ac_Name, Ac_Id From Ac_Mast Where Typ = 'Bank' Order By Ac_Name";

            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 20);
            DataView dvw = tblPop.DefaultView;

            VForText = "Select Bank Name";
            vSearchCol = "Ac_Name";
            vDisplayColumnList = "Ac_Name:Bank Name";
            vReturnCol = "Ac_Name,Ac_Id";
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
                this.txtBankName.Text = oSelectPop.pReturnArray[0];
                if (this.pAddMode == false && this.pEditMode == false)
                {
                    this.mthView();
                }

            }
        }

        private void chkClosedBook_CheckedChanged(object sender, EventArgs e)
        {
            //mthGrdRefresh();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtLocNm.Focus();
            vBook_Id = "0";
            this.pAddMode = true;
            this.pEditMode = false;
            this.mthNew(sender, e);
            btnLogout.Enabled = false;
            this.mthChkNavigationButton();
            button1.Enabled = false;
            dtpActDt.Text = DateTime.Now.ToString();
            dtpDeAct.Text = DateTime.Now.AddYears(1).ToString();

        }
        private void mthNew(object sender, EventArgs e)
        {
            this.dtpDeAct.Value = DateTime.Now.AddYears(1);
            this.mthBindClear();
            this.mthEnableDisableFormControls();
            tblChqDet = null;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            this.mthBindClear();
            this.vBook_Id = "0";
            tblChqDet = null;
            dtpActDt.Text = DateTime.Now.ToString();
            dtpDeAct.Text = DateTime.Now.AddYears(1).ToString();
            //if (string.IsNullOrEmpty(vYear))
            //{
            //    this.txtYear.Text = vYear;
            //}
            //if (string.IsNullOrEmpty(vLocNm))
            //{
            //    this.txtLocNm.Text = vLocNm;
            //}


            this.pAddMode = false;
            this.pEditMode = false;
            //this.chkIsClosed.Enabled = false;
            this.mthGrdRefresh();
            //if (this.dgvChequeDet.Rows.Count > 0)
            //{
            //    this.mthFldRefresh(0);
            //}
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            string vErrMsg = "";
            this.mthValidation(ref vErrMsg);
            if (vErrMsg != "")
            {
                MessageBox.Show(vErrMsg, this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            try
            {
                oDataAccess.BeginTransaction();
                string vSaveString = string.Empty;
                if (this.pAddMode)
                {
                    DataTable tblBook_Id = new DataTable();

                    Int32 vENo = Convert.ToInt32(this.txtStartNo.Text);
                    Int32 vLNo = Convert.ToInt32(this.txtLeaftcnt.pIntValue);
                    this.txtEndNo.Text = Convert.ToString(vENo + vLNo - 1);
                    this.mAddSaveString(ref vSaveString);
                    oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
                    vSqlStr = "Select  isnull(Max(Book_Id),0) From ChequeBookMaster ";
                    tblBook_Id = oDataAccess.GetDataTable(vSqlStr, null, 25);
                    vBook_Id = tblBook_Id.Rows[0][0].ToString();
                    vSaveString = "Execute USP_Ent_Ins_Cheque_Det 'A'," + vBook_Id + "," + this.txtStartNo.Text + "," + this.txtEndNo.Text + ",'" + this.txtBankName.Text + "'";
                    oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
                    this.pAddMode = false;
                }
                if (this.pEditMode)
                {
                    Int32 vENo = Convert.ToInt32(this.txtStartNo.Text);
                    Int32 vLNo = Convert.ToInt32(this.txtLeaftcnt.pIntValue);

                    this.txtEndNo.Text = Convert.ToString(vENo + vLNo - 1);

                    this.mEditSaveString(ref vSaveString);
                    oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
                    vSaveString = "Execute USP_Ent_Ins_Cheque_Det 'A'," + vBook_Id + "," + this.txtStartNo.Text + "," + this.txtEndNo.Text + ",'" + this.txtBankName.Text + "'";
                    oDataAccess.ExecuteSQLStatement(vSaveString, null, vTimeOut, true);
                    this.mUpdChqDet();
                    this.pEditMode = false;
                }
                oDataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.pPApplName);
                oDataAccess.RollbackTransaction();
            }
            tblChqDet = null;
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();
            this.mthBindClear();
            this.mthView();
            button1.Enabled = false;

            dtpActDt.Text = DateTime.Now.ToString();
            dtpDeAct.Text = DateTime.Now.AddYears(1).ToString();

        }

        private void mAddSaveString(ref string vSaveString)
        {

            string vfldList = string.Empty;
            string vfldValList = string.Empty;

            if (string.IsNullOrEmpty(this.txtLocNm.Text) == false)
            {
                vSqlStr = "Select Loc_Code from Loc_Master where Loc_Desc='" + this.txtLocNm.Text.Trim() + "'";
                DataSet tds = new DataSet();
                tds = oDataAccess.GetDataSet(vSqlStr, null, vTimeOut);
                vLoc_Code = tds.Tables[0].Rows[0]["Loc_Code"].ToString().Trim();
            }
            else
            {
                vLoc_Code = string.Empty;
            }

            if (string.IsNullOrEmpty(vLoc_Code) == true) { vLoc_Code = ""; }

            if (this.pAddMode == true)
            {

                vSaveString = " Set DateFormat dmy insert into " + vMainTblNm;
                vfldList = "(Bank_Nm,Chq_From,Chq_To,Book_Act_Dt,Book_Deact_Dt,Inv_Sr,Loc_Code)";
                vfldValList = "'" + this.txtBankName.Text.Trim() + "'";
                vfldValList = vfldValList + "," + this.txtStartNo.Text;
                vfldValList = vfldValList + "," + this.txtEndNo.Text;
                vfldValList = vfldValList + ",'" + this.dtpActDt.Value.ToString("dd-MM-yyyy") + "'";
                vfldValList = vfldValList + ",'" + this.dtpDeAct.Value.ToString("dd-MM-yyyy") + "'";
                vfldValList = vfldValList + ",'" + this.txtTrnSeries.Text + "'";
                vfldValList = vfldValList + ",'" + vLoc_Code + "'";
                vSaveString = vSaveString + vfldList + " Values( " + vfldValList + ")";
            }



        }
        private void mEditSaveString(ref string vSaveString)
        {

            string vfldList = string.Empty;
            string vfldValList = string.Empty;

            if (string.IsNullOrEmpty(this.txtLocNm.Text) == false)
            {
                vSqlStr = "Select Loc_Code from Loc_Master where Loc_Desc='" + this.txtLocNm.Text.Trim() + "'";
                DataSet tds = new DataSet();
                tds = oDataAccess.GetDataSet(vSqlStr, null, vTimeOut);
                vLoc_Code = tds.Tables[0].Rows[0]["Loc_Code"].ToString().Trim();
            }
            else
            {
                vLoc_Code = string.Empty;
            }

            if (string.IsNullOrEmpty(vLoc_Code) == true) { vLoc_Code = ""; }


            if (this.pEditMode == true)
            {
                vSaveString = " Set DateFormat dmy Update " + vMainTblNm + " Set ";
                // vfldList = "(Bank_Nm,Chq_From,Chq_To,Book_Act_Dt,Book_Deact_Dt,Inv_Sr,Loc_Code)";
                vfldValList = "Bank_Nm='" + this.txtBankName.Text.Trim() + "'";
                vfldValList = vfldValList + ",Chq_From=" + this.txtStartNo.Text.Trim();
                vfldValList = vfldValList + ",Chq_To=" + this.txtEndNo.Text.Trim();
                vfldValList = vfldValList + ",Book_Act_Dt='" + this.dtpActDt.Text.Trim() + "'";
                vfldValList = vfldValList + ",Book_Deact_Dt='" + this.dtpDeAct.Text.Trim() + "'";
                vfldValList = vfldValList + ",Inv_Sr='" + this.txtTrnSeries.Text.Trim() + "'";
                vfldValList = vfldValList + ",Loc_Code='" + vLoc_Code + "'";
                vSaveString = vSaveString + vfldValList;
                vSaveString = vSaveString + " " + " Where Book_Id=" + vBook_Id;
            }


        }
        private void mUpdChqDet()
        {
            if (tblChqDet == null)
            {
                return;
            }
            foreach (DataRow dr in tblChqDet.Rows)
            {
                if ((Boolean)dr["sel"] == true)
                {
                    vSqlStr = "Update Cheque_Details Set Status='" + dr["Status"].ToString().Trim() + "' Where Book_id=" + vBook_Id + " and Chq_No='" + dr["Chq_No"].ToString().Trim() + "'";
                    oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);
                }
            }
        }

        private void mthValidation(ref string vErrMsg)
        {

            if (this.pAddMode == true)
            {
                if (this.txtBankName.Text == "")
                {
                    vErrMsg = "Please enter Bank name";
                    this.txtBankName.Focus();
                }
                if (this.txtLeaftcnt.pIntValue <= 0)
                {
                    vErrMsg = "Please enter valid leaflet value";
                    this.txtLeaftcnt.Focus();
                }
                if (this.txtStartNo.pIntValue <= 0)
                {
                    vErrMsg = "Please enter valid Start value";
                    this.txtStartNo.Focus();
                }

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            txtLocNm.Focus();
            if (vBook_Id == "")
            {
                MessageBox.Show("Please Select Cheque Book by double clicking in below List", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            //this.mcheckCallingApplication();
            this.mthEdit();
            //    button1.Enabled = false;
        }
        private void mthEdit()
        {
            this.pAddMode = false;
            this.pEditMode = true;
            this.mthEnableDisableFormControls();
            this.mthChkNavigationButton();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            vSqlStr = "Execute [USP_Ent_Cheque_Det] " + vBook_Id;
            if (tblChqDet == null)
            {
                tblChqDet = new DataTable();
                tblChqDet = oDataAccess.GetDataTable(vSqlStr, null, 25);
            }

            if (tblChqDet.Rows.Count > 0)
            {
                frmChqDet oFrmChqDet = new frmChqDet();
                oFrmChqDet.pTblMain = tblChqDet;
                oFrmChqDet.pPApplPID = this.pPApplPID;
                oFrmChqDet.pPara = this.pPara;
                oFrmChqDet.pFrmCaption = "Cheque Details";
                oFrmChqDet.pCompId = this.pCompId;
                oFrmChqDet.pComDbnm = this.pComDbnm;

                oFrmChqDet.pServerName = this.pServerName;
                oFrmChqDet.pUserId = this.pUserId;
                oFrmChqDet.pPassword = this.pPassword;
                oFrmChqDet.pPApplRange = this.pPApplRange;
                oFrmChqDet.pAppUerName = this.pAppUerName;
                oFrmChqDet.pFrmIcon = this.pFrmIcon;
                oFrmChqDet.pPApplText = this.pPApplText;
                oFrmChqDet.pPApplName = this.pPApplName;
                oFrmChqDet.pPApplPID = this.pPApplPID;
                oFrmChqDet.pPApplCode = this.pPApplCode;
                oFrmChqDet.pBook_Id = vBook_Id;
                oFrmChqDet.pEditMode = this.pEditMode;

                oFrmChqDet.Show();
            }


        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            tblChqDet = null;
            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();
            this.mthBindClear();
           // this.mthView();
            button1.Enabled = false;

            dtpActDt.Text = DateTime.Now.ToString();
            dtpDeAct.Text = DateTime.Now.AddYears(1).ToString();


            //Add RRG 
            this.vBook_Id = "0";
            tblChqDet = null;

            this.pAddMode = false;
            this.pEditMode = false;

            this.mthChkNavigationButton();
            this.mthEnableDisableFormControls();

            //End RRG 

            refresh = 1;
            mthGrdRefresh();
            //this.ExportToExcel(tblChqMaster);
            //DataTable tblChqMastertemp = new DataTable();
            //tblChqMastertemp = tblChqMaster;
            //DataRow[] result = tblChqMastertemp.Select("Available > 0");
            //tblChqMastertemp = result.CopyToDataTable();

            //this.dgvChequeDet.DataSource = tblChqMastertemp;

        }

        private void txtLeaftcnt_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtLeaftcnt.Text.ToString() != "0")
            {
                txtEndNo.Text = (Int32.Parse(txtStartNo.Text) + Int32.Parse(txtLeaftcnt.Text) - 1).ToString();
            }
            else
            {
                txtEndNo.Text = txtStartNo.Text;
            }
        }

        private void txtStartNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtLeaftcnt.Text.ToString() != "0")
            {
                txtEndNo.Text = (Int32.Parse(txtStartNo.Text) + Int32.Parse(txtLeaftcnt.Text) - 1).ToString();
            }
            else
            {
                txtEndNo.Text = txtStartNo.Text;
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        public string getFileName(string fname)
        {
            string DfilePath, filename, FilePath;
            string date = DateTime.Now.ToString("dd/MM/yy") + "_" + DateTime.Now.ToString("hh:mm:ss tt");

            FolderBrowserDialog vFolderBrowserDialog1 = new FolderBrowserDialog();
            vFolderBrowserDialog1.ShowDialog();
            DfilePath = vFolderBrowserDialog1.SelectedPath;

            progressBar1.Value = 0;
            progressBar1.Value = 10;
            lblProgress.Visible = true;
            progressBar1.Visible = true;

            filename = fname + date;
            filename = filename.Replace(".", "").Replace("-", "").Replace("/", "").Replace(":", "").Replace("__", "_").Replace(" ", "");
            //  FilePath = DfilePath + filename+".xls";
            FilePath = DfilePath + filename;
            FilePath = FilePath.Replace("\\\\", "\\");
            return FilePath;
        }

        public void ExportToExcel(DataTable dt_record, string excelFilename)
        {
            try
            {
                lblProgress.Text = "Opening Excel Application.....";
                progressBar1.Value = 30;
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

                progressBar1.Value = 40;
                Microsoft.Office.Interop.Excel.Workbook wBook;

                progressBar1.Value = 50;
                Microsoft.Office.Interop.Excel.Worksheet wSheet;

                progressBar1.Value = 60;
                wBook = excel.Workbooks.Add(System.Reflection.Missing.Value);

                progressBar1.Value = 70;
                wSheet = (Microsoft.Office.Interop.Excel.Worksheet)wBook.ActiveSheet;

                progressBar1.Value = 80;
                System.Data.DataTable dt = dt_record;
                System.Data.DataColumn dc = new DataColumn();

                progressBar1.Value = 90;
                int colIndex = 0;
                float rowIndex = 1;
                progressBar1.Value = 100;

                foreach (DataColumn dcol in dt.Columns)
                {
                    colIndex = colIndex + 1;
                    excel.Cells[rowIndex, colIndex] = dcol.ColumnName;
                }

                progressBar1.Value = 0;
                rowIndex += 1;
                float nrow = 1;
                excel.Columns.NumberFormat = "@";
                foreach (DataRow drow in dt.Rows)
                {
                    colIndex = 0;
                    foreach (DataColumn dcol in dt.Columns)
                    {
                        colIndex = colIndex + 1;
                        excel.Cells[rowIndex, colIndex] = drow[dcol.ColumnName].ToString();

                    }
                    float aa = nrow == dt.Rows.Count ? 100.00f : (nrow / dt.Rows.Count) * 100;

                    progressBar1.Value = (Int32)aa;
                    progressBar1.Refresh();
                    progressBar1.Refresh();
                    lblProgress.Text = "Read Records: " + nrow.ToString() + "/" + dt.Rows.Count;

                    nrow = nrow + 1;
                    rowIndex = rowIndex + 1;
                }
                progressBar1.Value = 100;
                progressBar1.Refresh();



                object misValue = System.Reflection.Missing.Value;
                excel.Columns.AutoFit();
                excel.ActiveWorkbook.SaveAs(excelFilename + ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                excel.ActiveWorkbook.Saved = true;

                progressBar1.Visible = false;
                lblProgress.Visible = false;

                MessageBox.Show("Your excel file exported successfully at " + excelFilename + ".xls", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                MessageBox.Show("Excel Application not available", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                lblProgress.Visible = false;
            }
        }

        public DataTable getGridRecord()
        {
            string[] selectedColumns = new[] { "Bank_Nm", "Chq_From", "Chq_To", "TotalChq", "Available", "Issue", "Cancelled", "Reco", "PDC", "Inv_Sr", "Location", "Book_Act_Dt", "Book_Deact_Dt" };

            DataTable dtMainRecord = tblChqMaster;
            DataRow[] result = dtMainRecord.Select("Sel=1");

            if (result.Length > 0)
            {
                dtMainRecord = result.CopyToDataTable();
                DataTable dt = new DataView(dtMainRecord).ToTable(false, selectedColumns);

                dt.Columns["Bank_Nm"].ColumnName = "Bank Name";
                dt.Columns["Chq_From"].ColumnName = "Cheque From";
                dt.Columns["Chq_To"].ColumnName = "Cheque To";
                dt.Columns["Book_Act_Dt"].ColumnName = "Book Activate From";
                dt.Columns["Book_Deact_Dt"].ColumnName = "Book Deactivate From";
                dt.Columns["TotalChq"].ColumnName = "Total Cheque";
                dt.Columns["Available"].ColumnName = "Available";
                dt.Columns["Issue"].ColumnName = "Issue";
                dt.Columns["Cancelled"].ColumnName = "Cancelled";
                dt.Columns["Reco"].ColumnName = "Reconciled";
                dt.Columns["PDC"].ColumnName = "PDC";
                dt.Columns["Inv_Sr"].ColumnName = "Transaction Series";
                dt.Columns["Location"].ColumnName = "Location";
                return dt;
            }
            else
            {
                return null;
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            int count = 0;
            CheckBox headerBox = ((CheckBox)dgvChequeDet.Controls.Find("checkboxHeader", true)[0]);

            string status = headerBox.CheckState.ToString();

            if (status == "Checked")
            {
                foreach (DataRow row in tblChqMaster.Rows)
                {
                    if (row["Sel"].ToString() == "0")
                    {
                        row["sel"] = headerBox.Checked;
                    }
                }
            }
            else
            {
                foreach (DataRow row in tblChqMaster.Rows)
                {
                    if (row["Sel"].ToString() == "1")
                    {
                        row["sel"] = headerBox.Checked;
                    }
                }
            }

        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            int count = dgvChequeDet.RowCount;
            string file_path;

            if (count != 0)
            {
                DataTable dtRecord = getGridRecord();
                if (dtRecord != null)
                {
                    file_path = getFileName("\\ChequeBook_");
                    progressBar1.Value = 20;
                    ExportToExcel(dtRecord, file_path);


                }
                else
                {
                    MessageBox.Show("Please select atleast one record.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }

            }
            else
            {
                MessageBox.Show("Record not found to export", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void dtpActDt_Leave(object sender, EventArgs e)
        {
            DateTime dd = DateTime.Parse(dtpActDt.Text.ToString());
            dtpDeAct.Text = dd.AddYears(1).ToString();
          
        }

        private void dtpActDt_KeyUp(object sender, KeyEventArgs e)
        {
            //DateTime dd = DateTime.Parse(dtpActDt.Text.ToString());
            //dtpDeAct.Text = dd.AddYears(1).ToString();
        }

        private void mthBindClear()
        {
            this.txtBankName.Text = ""; /*Ramya 29/10/2012*/
            this.txtLocNm.Text = "";
            this.txtLeaftcnt.Text = "";
            this.txtStartNo.Text = "";
            this.txtEndNo.Text = "";
            this.txtTrnSeries.Text = "";

        }
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string vSqlStr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udCheueManagementMaster.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            vSqlStr = "Set Dateformat DMY insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";  //Modified by Priyanka B on 14022018 for Bug-31240
            oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);
        }
        private void SetMenuRights()
        {
            DataSet dsMenu = new DataSet();
            DataSet dsRights = new DataSet();
            this.pPApplRange = this.pPApplRange.Replace("^", "");
            string strSQL = "select padname,barname,range from com_menu where range =" + this.pPApplRange;
            dsMenu = oDataAccess.GetDataSet(strSQL, null, vTimeOut);
            if (dsMenu != null)
            {
                if (dsMenu.Tables[0].Rows.Count > 0)
                {
                    string padName = "";
                    string barName = "";
                    padName = dsMenu.Tables[0].Rows[0]["padname"].ToString();
                    barName = dsMenu.Tables[0].Rows[0]["barname"].ToString();
                    strSQL = "select padname,barname,dbo.func_decoder(rights,'F') as rights from ";
                    strSQL += "userrights where padname ='" + padName.Trim() + "' and barname ='" + barName + "' and range = " + this.pPApplRange;
                    strSQL += "and dbo.func_decoder([user],'T') ='" + this.pAppUerName.Trim() + "'";

                }
            }
            dsRights = oDataAccess.GetDataSet(strSQL, null, vTimeOut);


            if (dsRights != null)
            {
                string rights = dsRights.Tables[0].Rows[0]["rights"].ToString();
                int len = rights.Length;
                string newString = "";
                ArrayList rArray = new ArrayList();

                while (len > 2)
                {
                    newString = rights.Substring(0, 2);
                    rights = rights.Substring(2);
                    rArray.Add(newString);
                    len = rights.Length;
                }
                rArray.Add(rights);

                this.pAddButton = (rArray[0].ToString().Trim() == "IY" ? true : false);
                this.pEditButton = (rArray[1].ToString().Trim() == "CY" ? true : false);
                this.pDeleteButton = (rArray[2].ToString().Trim() == "DY" ? true : false);
                this.pPrintButton = (rArray[3].ToString().Trim() == "PY" ? true : false);
            }

        }




    }
}
