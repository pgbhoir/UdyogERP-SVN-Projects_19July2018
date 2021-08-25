using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using uBaseForm;
using System.Collections;
using ueconnect;
using GetInfo;
using System.Diagnostics;
using DataAccess_Net;
using System.Globalization;
using System.Threading;
using udSelectPop;


namespace udTextLotNoUpdate
{
    public partial class frmMain : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;
        String cAppPId, cAppName;
        short vTimeOut = 20;

        string SqlStr;
        DataTable tblMain;

        bool isChanged = false;
        DataTable dtOldRecords = new DataTable();
        CheckBox checkboxHeader;


        //Added by Priyanka B on 05102019 for Bug-32747 Start
        bool isValid = true;
        string sMsg1 = "";
        //Added by Priyanka B on 05102019 for Bug-32747 End

        public frmMain(string[] args)
        {
            this.pDisableCloseBtn = true;

            InitializeComponent();
            this.pFrmCaption = "Lot Number Update"; //Rup
            this.pPApplPID = 0;

            this.pPara = args;
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

            this.KeyPreview = true;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.SetMenuRights();
            startupPath = Application.StartupPath;
            //startupPath = "E:\\Product Installed\\UdyogERPSDK_2.2.0";
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
            this.mInsertProcessIdRecord();
            this.SetFormColor();


            string appPath = Application.ExecutablePath;
            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }
            this.mthChkNavigationButton();
            this.mthControlSet();

            this.mthGrdRefresh(false);

            lblProgress.Visible = false;
            progressBar1.Visible = false;
            this.pEditMode = false;
        }
        //Added by Priyanka B on 05102019 for Bug-32747 Start

        private void mthGrdValidate(DataTable dtMain)
        {
            isValid = true;
            SqlStr = "";
            string sInvNo1 = "", sInvNo2 = "";
            string invno = "";
            sMsg1 = "";
            
            foreach (DataGridViewRow dr in dgvMain.Rows)
            {
                if ((bool)dr.Cells["colSelect"].Value == true)
                {
                    DataRow[] dr1 = dtMain.Select("Inv_No<>'" + dr.Cells["colInvNo"].Value + "' and lotno='" + dr.Cells["colLotNo"].Value + "'");

                    SqlStr = "select top 1 * from (Select entry_ty,inv_no,fLotNo From OSITEM union all Select entry_ty,inv_no,fLotNo From ARITEM ) a where flotno<>'' and flotno='" + dr.Cells["colLotNo"].Value.ToString().Trim() + "'";
                    DataTable dtFound = new DataTable();
                    dtFound = oDataAccess.GetDataTable(SqlStr, null, 20);

                    DataRow[] dr2 = dtFound.Select("flotno='" + dr.Cells["colLotNo"].Value + "'");

                    if (dr1.Length > 0)
                    {
                        isValid = false;

                        int count = 0;
                        foreach (DataRow drRec in dr1)
                        {
                            if (invno.Contains(drRec["Inv_No"].ToString().Trim()) == false)
                            {
                                if (count == 0)
                                {
                                    invno = drRec["Inv_No"].ToString().Trim();
                                }
                                else
                                {
                                    invno = invno + "," + drRec["Inv_No"].ToString().Trim();
                                }
                                count++;
                            }
                        }
                        if (sInvNo1.Contains(invno) == false)
                        {
                            sInvNo1 = sInvNo1 == "" ? invno : sInvNo1 + "," + invno;
                        }
                    }
                    if (dr2.Length > 0)
                    {
                        isValid = false;
                        sInvNo2 = sInvNo2 == "" ? "Invoice No. '" + dtFound.Rows[0]["inv_no"].ToString().Trim() +
                        "' having Lot No. '" + dtFound.Rows[0]["flotno"].ToString().Trim() + "'"
                        : sInvNo2 + ",\n" + "Invoice No. '" + dtFound.Rows[0]["inv_no"].ToString().Trim() +
                        "' having Lot No. '" + dtFound.Rows[0]["flotno"].ToString().Trim() + "'";
                    }
                }
            }
            if (sInvNo1 != string.Empty)
            {
                List<string> invnos = sInvNo1.Split(',').ToList<string>();
                invnos.Sort();
                string sInv = "";
                foreach (string n in invnos)
                {
                    sInv = sInv == "" ? n : sInv + "," + n;
                }

                sMsg1 = "Duplicate Lot No. found in below invoices of Fabric Labour Job Issue (IV) : \n" + sInv;
            }
            if (sInvNo2 != string.Empty)
            {
                sMsg1 = sMsg1 + "\n\n" + sInvNo2 + " which is already in use!!";
            }
        }
        //Added by Priyanka B on 05102019 for Bug-32747 End

        private void btnSave_Click(object sender, EventArgs e)
        {
            dgvMain.EndEdit();

            DataTable dtMainRecord = this.tblMain;
            
            dtMainRecord.AcceptChanges();
            DataRow[] res = dtMainRecord.Select("Sel=1");
            //Added by Prajakta B. on 23/01/2020 for Bug 33252  --Start
            if (txtbulklot.Text.ToString().Trim()!="")
            {
                string bulklotno = txtbulklot.Text.ToString().Trim();
                foreach (DataRow row in res)
                {
                    row["lotno"] = bulklotno;
                }
                dtMainRecord.AcceptChanges();
            }
            //Added by Prajakta B. on 23/01/2020 for Bug 33252  --End
            if (res.Length == 0)
            {
                MessageBox.Show("Please select atleast one record for which you want to update Lot No.!!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            //Added by Priyanka B on 05102019 for Bug-32747 Start
            this.mthGrdValidate(dtMainRecord);

            if (!isValid)
            {
                if (sMsg1 != string.Empty)
                {
                    MessageBox.Show(sMsg1.ToString(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            //Added by Priyanka B on 05102019 for Bug-32747 End

            DataTable dtRecFound;
            string sInvNo = "", sLotNo = "", sMsg = "";
            foreach (DataGridViewRow dr in dgvMain.Rows)
            {
                if ((bool)dr.Cells["colSelect"].Value == true)
                {
                    DataRow[] s = dtOldRecords.Select("Inv_no='" + dr.Cells["colInvNo"].Value.ToString() + "' and ItSerial='" + dr.Cells["colItSerial"].Value.ToString() + "'");
                    //SqlStr = "select top 1 * from (Select entry_ty,fLotNo From ARITEM Union all Select entry_ty,fLotNo From IIMAIN Union all Select entry_ty,fLotNo From IPITEM Union all Select entry_ty,fLotNo From IRITEM Union all Select entry_ty,fCutLotNo From ITEM Union all Select entry_ty,fLotNo From ITEM Union all Select entry_ty,fCutLotNo From MAIN Union all Select entry_ty,fLotNo From MAIN Union all Select entry_ty,fLotNo From OPITEM ) a where flotno<>'' and flotno='" + s[0][3].ToString().Trim() + "'";
                    SqlStr = "select top 1 * from (Select entry_ty,fLotNo From IPITEM Union all Select entry_ty,fLotNo From IRITEM Union all Select entry_ty,fLotNo From ITEM Union all Select entry_ty,fLotNo From MAIN Union all Select entry_ty,fLotNo From OPITEM ) a where flotno<>'' and flotno='" + s[0][3].ToString().Trim() + "'";
                    dtRecFound = new DataTable();
                    dtRecFound = oDataAccess.GetDataTable(SqlStr, null, 20);
                    if (dtRecFound.Rows.Count > 0)
                    {
                        sInvNo = sInvNo == "" ? "Invoice No. '" + dr.Cells["colInvNo"].Value.ToString().Trim() +
                        "' having '" + dr.Cells["colItNm"].Value.ToString().Trim() + "' goods " +
                        " with Lot No. '" + s[0][3].ToString().Trim() + "'"
                        : sInvNo + ",\n" + "Invoice No. '" + dr.Cells["colInvNo"].Value.ToString().Trim() +
                        "' having '" + dr.Cells["colItNm"].Value.ToString().Trim() + "' goods "
                        + " with Lot No. '" + s[0][3].ToString().Trim() + "'";
                    }
                    else
                    {
                        SqlStr = "Update IiItem Set fLotNo='" + dr.Cells["colLotNo"].Value.ToString().Trim() + "' Where Tran_Cd=" + dr.Cells["colTranCd"].Value.ToString().Trim() + " and ItSerial='" + dr.Cells["colItSerial"].Value.ToString().Trim() + "'";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
                        dr.Cells["colSelect"].Value = false;
                        sLotNo = sLotNo == "" ? "Invoice No. '" + dr.Cells["colInvNo"].Value.ToString().Trim() +
                            "' having '" + dr.Cells["colItNm"].Value.ToString().Trim() + "' goods " +
                            " with Lot No. '" + dr.Cells["colLotNo"].Value.ToString().Trim() + "'"
                            : sLotNo + ",\n" + "Invoice No. '" + dr.Cells["colInvNo"].Value.ToString().Trim() +
                            "' having '" + dr.Cells["colItNm"].Value.ToString().Trim() + "' goods " +
                            " with Lot No. '" + dr.Cells["colLotNo"].Value.ToString().Trim() + "'";
                    }
                }
            }
            if (sInvNo != string.Empty && sLotNo != string.Empty)
            {
                sMsg = sInvNo + "\nRecords already in use.Cannot be updated!!" + "\nAND\n" + sLotNo + "\nRecords updated successfully!!!";
            }
            else if (sInvNo != string.Empty)
            {
                sMsg = sInvNo + "\nRecords already in use.Cannot be updated!!";
            }
            else
            {
                sMsg = sLotNo + "\nRecords updated successfully!!!";
            }
            this.mthGrdRefresh(false);
            MessageBox.Show(sMsg.ToString(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);


            //foreach (DataGridViewRow dr in dgvMain.Rows)
            //{
            //    if ((bool)dr.Cells["colSelect"].Value == true)
            //    {
            //        SqlStr = "Update IiItem Set fLotNo='" + dr.Cells["colLotNo"].Value.ToString().Trim() + "' Where Tran_Cd=" + dr.Cells["colTranCd"].Value.ToString().Trim() + " and ItSerial='" + dr.Cells["colItSerial"].Value.ToString().Trim() + "'";
            //        oDataAccess.ExecuteSQLStatement(SqlStr, null, vTimeOut, true);
            //        dr.Cells["colSelect"].Value = false;
            //    }
            //}
            this.pEditMode = false;
            this.mthChkNavigationButton();
            this.btnRefresh.Focus();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.mthGrdRefresh(false);
        }
        private void mthGrdRefresh(bool dtRefresh)
        {
            //SqlStr = "Set Dateformat DMY Select Sel=cast(0 as bit),itm.It_Name,i.Qty,LotNo=i.fLotNo,acm.Ac_Name,i.Inv_No,i.[Date],i.Tran_Cd,i.ItSerial  From  IiItem i inner Join It_Mast itm on (i.It_Code=itm.It_Code) inner Join Ac_Mast acm on (i.Ac_Id=acm.Ac_Id) Where i.Entry_Ty='ID' ";  //Commented by Prajakta B. on 23/01/2020 for Bug 33252
            //Modified by Prajakta B. on 23/01/2020   for Bug 33252--Start
            SqlStr = "Set Dateformat DMY Select Sel=cast(0 as bit),itm.It_Name,i.Qty,LotNo=i.fLotNo,acm.Ac_Name,aci.ac_name as dyer_name,i.Inv_No,i.[Date],i.Tran_Cd,i.ItSerial  From  IiItem i " +
                     " left join iimain m on i.entry_ty=m.entry_ty and i.tran_cd = m.tran_cd inner Join It_Mast itm on (i.It_Code=itm.It_Code) inner Join Ac_Mast acm on (m.cons_id=acm.Ac_Id) "+
                     " inner join ac_mast aci on (m.Ac_id=aci.Ac_id) Where i.Entry_Ty='ID' ";
            //Modified by Prajakta B. on 23/01/2020 for Bug 33252  --End
            if (this.chkBlankLot.Checked == false)
            {
                //SqlStr = SqlStr + " Where (i.fLotNo<>'' or i.flotno='')";
                SqlStr = SqlStr + " and (i.fLotNo<>'' or i.flotno='')";
            }
            else
            {
                SqlStr = SqlStr + " and i.fLotNo='' ";
            }

            if (this.txtPartyNm.Text != "")
            {
                //SqlStr = SqlStr + " and aci.Ac_Name='" + this.txtDyerNm.Text.Trim() + "'"; // Commented by Prajakta B. on 25/01/2020 for Bug 33252
                SqlStr = SqlStr + " and acm.Ac_Name='" + this.txtPartyNm.Text.Trim() + "'"; //Modified by Prajakta B. on 25 / 01 / 2020 for Bug 33252
            }
            //Added by Prajakta B. on 25/01/2020 for Bug 33252  --Start
            if (this.txtDyerNm.Text != "")
            {
                SqlStr = SqlStr + " and aci.Ac_Name='" + this.txtDyerNm.Text.Trim() + "'";
            }
            //Added by Prajakta B. on 25/01/2020 for Bug 33252  --End
            if (this.txtItNm.Text != "")
            {
                SqlStr = SqlStr + " and itm.It_Name='" + this.txtItNm.Text.Trim() + "'";
            }
            if (this.txtTranNo.Text != "")
            {
                SqlStr = SqlStr + " and i.Inv_No='" + this.txtTranNo.Text.Trim() + "'";
            }
            if (this.txtLotNo.Text != "")
            {
                SqlStr = SqlStr + " and i.fLotNo='" + this.txtLotNo.Text.Trim() + "'";
            }
            //if (dtRefresh == true)
            if (isChanged == true)
            {
                SqlStr = SqlStr + " and i.[Date]='" + this.dtpTranDt.Text.Trim() + "'";
            }
            SqlStr = SqlStr + " order by i.inv_no";
            //MessageBox.Show(SqlStr.ToString());
            tblMain = new DataTable();
            tblMain = oDataAccess.GetDataTable(SqlStr, null, 20);

            this.dgvMain.Columns.Clear();
            this.dgvMain.DataSource = this.tblMain;
            this.dgvMain.Columns.Clear();

            System.Windows.Forms.DataGridViewCheckBoxColumn colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            //this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            colSelect.HeaderText = "";
            colSelect.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSelect.Width = 20;
            colSelect.Name = "colSelect";
            //Prajakta B Start
            if (this.pEditMode == true)
                colSelect.ReadOnly = false;
            else
                colSelect.ReadOnly = true;
            //Prajakta B end
            //colSelect.ReadOnly = false;
            this.dgvMain.Columns.Add(colSelect);

            System.Windows.Forms.DataGridViewTextBoxColumn colItNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItNm.HeaderText = "Item Name";
            colItNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colItNm.Width = 270;
            colItNm.Name = "colItNm";
            colItNm.ReadOnly = true;
            this.dgvMain.Columns.Add(colItNm);

            System.Windows.Forms.DataGridViewTextBoxColumn colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colQty.HeaderText = "Quantity";
            colQty.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colQty.Width = 100;
            colQty.Name = "colQty";
            colQty.ReadOnly = true;
            this.dgvMain.Columns.Add(colQty);

            System.Windows.Forms.DataGridViewTextBoxColumn colLotNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLotNo.HeaderText = "Lot No.";
            colLotNo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colLotNo.Width = 120;
            colLotNo.Name = "colLotNo";
            if (this.pEditMode == true)
                colLotNo.ReadOnly = false;
            else
                colLotNo.ReadOnly = true;
            this.dgvMain.Columns.Add(colLotNo);

            System.Windows.Forms.DataGridViewTextBoxColumn colPartyNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPartyNm.HeaderText = "Party Name";
            colPartyNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colPartyNm.Width = 270;
            colPartyNm.Name = "colPartyNm";
            colPartyNm.ReadOnly = true;
            this.dgvMain.Columns.Add(colPartyNm);

            //Added by Prajakta B. on 25/01/2020 for Bug 33252  --Start
            System.Windows.Forms.DataGridViewTextBoxColumn colDyerNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDyerNm.HeaderText = "Dyer Name";
            colDyerNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colDyerNm.Width = 270;
            colDyerNm.Name = "colDyerNm";
            colDyerNm.ReadOnly = true;
            this.dgvMain.Columns.Add(colDyerNm);
            //Added by Prajakta B. on 25/01/2020 for Bug 33252  --End

            System.Windows.Forms.DataGridViewTextBoxColumn colInvNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colInvNo.HeaderText = "Tran. Ref. No.";
            colInvNo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colInvNo.Width = 100;
            colInvNo.Name = "colInvNo";
            colInvNo.ReadOnly = true;
            this.dgvMain.Columns.Add(colInvNo);

            System.Windows.Forms.DataGridViewTextBoxColumn colTranCd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTranCd.HeaderText = "Tran. Code";
            colTranCd.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colTranCd.Width = 100;
            colTranCd.Name = "colTranCd";
            colTranCd.ReadOnly = true;
            colTranCd.Visible = false;
            this.dgvMain.Columns.Add(colTranCd);

            System.Windows.Forms.DataGridViewTextBoxColumn colItSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItSerial.HeaderText = "Item Serial";
            colItSerial.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colItSerial.Width = 100;
            colItSerial.Name = "colItSerial";
            colItSerial.ReadOnly = true;
            colItSerial.Visible = false;
            this.dgvMain.Columns.Add(colItSerial);


            dgvMain.Columns["colSelect"].DataPropertyName = "sel";
            dgvMain.Columns["colItNm"].DataPropertyName = "It_Name";
            dgvMain.Columns["colQty"].DataPropertyName = "Qty";
            dgvMain.Columns["colLotNo"].DataPropertyName = "LotNo";
            dgvMain.Columns["colPartyNm"].DataPropertyName = "Ac_Name";//Commented by Prajakta B. on 25/01/2020 for Bug 33252
            dgvMain.Columns["colDyerNm"].DataPropertyName = "dyer_name";  //Added by Prajakta B. on 25/01/2020 for Bug 33252
            dgvMain.Columns["colInvNo"].DataPropertyName = "Inv_No";
            dgvMain.Columns["colTranCd"].DataPropertyName = "Tran_Cd";
            dgvMain.Columns["colItSerial"].DataPropertyName = "ItSerial";

            Rectangle rect = dgvMain.GetCellDisplayRectangle(0, -1, true);
            rect.Y = 4;
            rect.X = 6;
            checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";

            checkboxHeader.Size = new Size(15, 15);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);
            dgvMain.Controls.Add(checkboxHeader);

            dgvMain.Refresh();


            //" + "'" + this.txtLocNm.Text + "','" + this.txtBankName.Text.Trim() + "'," + (this.chkClosedBook.Checked == true ? "'Y'" : "'N'");
            dtOldRecords = oDataAccess.GetDataTable(SqlStr, null, 20);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.pEditMode == true)
            {
                if (this.checkboxHeader.Checked == true)
                {
                    foreach (DataGridViewRow row1 in dgvMain.Rows)
                    {
                        row1.Cells["colSelect"].Value = true;
                    }
                }
                else
                {
                    foreach (DataGridViewRow row1 in dgvMain.Rows)
                    {
                        row1.Cells["colSelect"].Value = false;
                    }
                }
                //foreach (DataGridViewRow row1 in dgvMain.Rows)
                //{

                //    var chk = row1.Cells["colSelect"].Value.ToString().Trim();
                //    if (chk == "False")
                //    {

                //        row1.Cells["colSelect"].Value = true;

                //    }
                //    else if (chk == "True")
                //    {
                //        if (chk == "True")
                //        {
                //            row1.Cells["colSelect"].Value = false;
                //        }
                //    }
                //}
                // dataGridView1.RefreshEdit();
                dgvMain.EndEdit();
            }
            else
            {
                this.checkboxHeader.Checked = false;
            }
        }

        public void mthControlSet()
        {
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnPartyNm.Image = Image.FromFile(fName);
                this.btnItNm.Image = Image.FromFile(fName);
                this.btnTranNo.Image = Image.FromFile(fName);
                this.btnLotNo.Image = Image.FromFile(fName);  //Added by Priyanka B on 22112019 for Bug-32747
                this.btnDyer.Image = Image.FromFile(fName);  //Added by Prajakta B, on 25/01/2020 for Bug 33252
            }
            fName = this.pAppPath + @"\bmp\Excel_1.ico";
            //Excel_Exp.ico";
            if (File.Exists(fName) == true)
            {
                this.btnExportExcel.Image = Image.FromFile(fName);
            }
            this.chkBlankLot.Checked = false;
        }

        private void btnPartyNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            //SqlStr = "Select acm.Ac_Name From  IiItem i inner Join Ac_Mast acm on (i.Ac_Id=acm.Ac_Id)";  //Commented by Prajakta B. on 25/01/2020 for Bug 33252
            SqlStr = "select ac_mast.ac_name from iimain inner join ac_mast on iimain.cons_id=ac_mast.ac_id";  //Modified by Prajakta B. on 25/01/2020 for Bug 33252
            if (this.chkBlankLot.Checked == true)
            {
                SqlStr = SqlStr + " Where i.fLotNo<>'' ";
            }

            SqlStr = SqlStr + " union Select ''  Order By Ac_Name";
            tblPop = oDataAccess.GetDataTable(SqlStr, null, 20);
            DataView dvw = tblPop.DefaultView;

            VForText = "Select Party Name";
            vSearchCol = "Ac_Name";
            vDisplayColumnList = "Ac_Name:Party Name";
            vReturnCol = "Ac_Name";
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
                this.txtPartyNm.Text = oSelectPop.pReturnArray[0];
            }
        }

        private void btnItNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            SqlStr = "Select itm.It_Name From  IiItem i inner Join It_Mast itm on (i.It_Code=itm.It_Code) inner Join Ac_Mast acm on (i.Ac_Id=acm.Ac_Id) ";
            if (this.chkBlankLot.Checked == true)
            {
                SqlStr = SqlStr + " Where i.fLotNo<>'' ";
            }
            if (this.txtPartyNm.Text != "")
            {
                SqlStr = SqlStr + " and acm.Ac_Name='" + this.txtPartyNm.Text.Trim() + "'";
            }
            SqlStr = SqlStr + " union Select ''  Order By It_Name";

            tblPop = oDataAccess.GetDataTable(SqlStr, null, 20);
            DataView dvw = tblPop.DefaultView;

            VForText = "Select Item Name";
            vSearchCol = "It_Name";
            vDisplayColumnList = "It_Name:Item Name";
            vReturnCol = "It_Name";
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
                this.txtItNm.Text = oSelectPop.pReturnArray[0];
            }
        }
        private void btnTranNo_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            SqlStr = "Select i.Inv_No From  IiItem i inner Join It_Mast itm on (i.It_Code=itm.It_Code) inner Join Ac_Mast acm on (i.Ac_Id=acm.Ac_Id) ";
            if (this.chkBlankLot.Checked == true)
            {
                SqlStr = SqlStr + " Where i.fLotNo<>'' ";
            }

            if (this.txtPartyNm.Text != "")
            {
                SqlStr = SqlStr + " and acm.Ac_Name='" + this.txtPartyNm.Text.Trim() + "'";
            }
            if (this.txtItNm.Text != "")
            {
                SqlStr = SqlStr + " and itm.It_Name='" + this.txtItNm.Text.Trim() + "'";
            }
            SqlStr = SqlStr + " union Select ''  Order By i.Inv_No";
            tblPop = oDataAccess.GetDataTable(SqlStr, null, 20);
            DataView dvw = tblPop.DefaultView;

            VForText = "Select Transaction No.";
            vSearchCol = "It_Name";
            vDisplayColumnList = "Inv_No:Transaction No.";
            vReturnCol = "Inv_No";
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
                this.txtTranNo.Text = oSelectPop.pReturnArray[0];
            }

        }
        private void btnLotNo_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            SqlStr = "Select i.fLotNo From  IiItem i inner Join It_Mast itm on (i.It_Code=itm.It_Code) inner Join Ac_Mast acm on (i.Ac_Id=acm.Ac_Id) ";
            if (this.chkBlankLot.Checked == true)
            {
                SqlStr = SqlStr + " Where i.fLotNo<>'' ";
            }
            if (this.txtPartyNm.Text != "")
            {
                SqlStr = SqlStr + " and acm.Ac_Name='" + this.txtPartyNm.Text.Trim() + "'";
            }
            if (this.txtItNm.Text != "")
            {
                SqlStr = SqlStr + " and itm.It_Name='" + this.txtItNm.Text.Trim() + "'";
            }
            if (this.txtTranNo.Text != "")
            {
                SqlStr = SqlStr + " and i.Inv_No='" + this.txtTranNo.Text.Trim() + "'";
            }
            SqlStr = SqlStr + " union Select ''  Order By i.fLotNo";
            tblPop = oDataAccess.GetDataTable(SqlStr, null, 20);
            DataView dvw = tblPop.DefaultView;

            VForText = "Select Lot No.";
            vSearchCol = "fLotNo";
            vDisplayColumnList = "fLotNo:Lot No.";
            vReturnCol = "fLotNo";
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
                this.txtLotNo.Text = oSelectPop.pReturnArray[0];
            }
        }
        private void mthChkNavigationButton()
        {
            if (this.pEditMode == false)
            {
                this.btnEdit.Enabled = true;
                this.btnSave.Enabled = false;
                this.btnCancel.Enabled = false;
                this.btnExit.Enabled = true;
                this.txtbulklot.ReadOnly = true;
                this.txtbulklot.Text = "";
            }
            else
            {
                this.btnEdit.Enabled = false;
                this.btnSave.Enabled = true;
                this.btnCancel.Enabled = true;
                this.btnExit.Enabled = false;
                this.txtbulklot.ReadOnly = false;
                this.txtbulklot.Text = "";
            }
        }
        private void SetMenuRights()
        {
            DataSet dsMenu = new DataSet();
            DataSet dsRights = new DataSet();
            this.pPApplRange = this.pPApplRange.Replace("^", "");
            string strSQL = "select padname,barname,range from com_menu where range =" + this.pPApplRange;
            dsMenu = oDataAccess.GetDataSet(strSQL, null, 20);
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
            dsRights = oDataAccess.GetDataSet(strSQL, null, 20);


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
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;

            cAppName = "udTextLotNoUpdate.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.pEditMode = true;
            dgvMain.Columns["colLotNo"].ReadOnly = false;
            dgvMain.Columns["colSelect"].ReadOnly = false;
            this.mthChkNavigationButton();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.pEditMode = false;
            this.mthChkNavigationButton();
            this.mthGrdRefresh(false);

            dgvMain.EndEdit();
            foreach (DataGridViewRow dr in dgvMain.Rows)
            {
                dr.Cells["colSelect"].Value = false;
            }

        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            int count = dgvMain.RowCount;
            string file_path;

            if (count != 0)
            {
                DataTable dtRecord = getGridRecord();
                if (dtRecord != null)
                {
                    file_path = getFileName("\\Lot Details_");
                    if (file_path != "")
                    {
                        progressBar1.Value = 20;
                        ExportToExcel(dtRecord, file_path);
                    }
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

                MessageBox.Show("Your excel file exported successfully at " + excelFilename + ".xls", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                MessageBox.Show("Excel Application not available", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                lblProgress.Visible = false;
            }
        }
        public DataTable getGridRecord()
        {
            //string[] selectedColumns = new[] { "It_Name", "Qty", "LotNo", "Ac_Name", "Inv_No" };
            string[] selectedColumns = new[] { "It_Name", "Qty", "LotNo", "Ac_Name","Dyer_Name", "Inv_No" };

            DataTable dtMainRecord = this.tblMain;
            DataRow[] result = dtMainRecord.Select("Sel=1");

            if (result.Length > 0)
            {
                dtMainRecord = result.CopyToDataTable();
                DataTable dt = new DataView(dtMainRecord).ToTable(false, selectedColumns);

                dt.Columns["It_Name"].ColumnName = "Item Name";
                dt.Columns["Qty"].ColumnName = "Quantity";
                dt.Columns["LotNo"].ColumnName = "Lot No.";
                dt.Columns["Ac_Name"].ColumnName = "Party Name";
                dt.Columns["Dyer_Name"].ColumnName = "Dyer Name";
                dt.Columns["Inv_No"].ColumnName = "Trans. Ref. No.";

                return dt;
            }
            else
            {
                return null;
            }

        }
        public string getFileName(string fname)
        {
            string DfilePath, filename, FilePath = "";
            string date = DateTime.Now.ToString("dd/MM/yy") + "_" + DateTime.Now.ToString("hh:mm:ss tt");

            FolderBrowserDialog vFolderBrowserDialog1 = new FolderBrowserDialog();
            if (vFolderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
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
            }
            return FilePath;
        }

        private void dtpTranDt_ValueChanged(object sender, EventArgs e)
        {
            isChanged = true;
        }


        //Added by Priyanka B on 09102019 for Bug-32747 Start

        private void dgvMain_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            int colIndex = this.dgvMain.CurrentCell.ColumnIndex;
            int rowIndex = this.dgvMain.CurrentCell.RowIndex;

            if (dgvMain.Rows[rowIndex].Cells[0].Value.ToString().ToLower() != "true")
            {
                dgvMain.Rows[rowIndex].Cells[0].Value = true;
            }
        }

        private void dgvMain_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                string lotno = dgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Trim();
                if (lotno == "")
                {
                    //dgvMain.Rows[e.RowIndex].Cells[0].Value = false;
                }
            }
        }
        //Added by Priyanka B on 22112019 for Bug-32747 Start
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnExit_Click(this.btnExit, e);
        }
        //Added by Prajakta B. on 25/01/2020 for Bug 33252  --Start
        private void btnDyer_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            SqlStr = "Select acm.Ac_Name From  IiItem i inner Join Ac_Mast acm on (i.Ac_Id=acm.Ac_Id)";
            if (this.chkBlankLot.Checked == true)
            {
                SqlStr = SqlStr + " Where i.fLotNo<>'' ";
            }

            SqlStr = SqlStr + " union Select ''  Order By Ac_Name";
            tblPop = oDataAccess.GetDataTable(SqlStr, null, 20);
            DataView dvw = tblPop.DefaultView;

            VForText = "Select Dyer Name";
            vSearchCol = "Ac_Name";
            vDisplayColumnList = "Ac_Name:Dyer Name";
            vReturnCol = "Ac_Name";
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
                this.txtDyerNm.Text = oSelectPop.pReturnArray[0];
            }
        }
        //Added by Prajakta B. on 25/01/2020 for Bug 33252  --End
        //Added by Prajakta B. on 17/03/2020 for Bug 33252  --Start
        private void chkBlankLot_CheckedChanged(object sender, EventArgs e)
        {
            if(chkBlankLot.Checked==true)
            {
                txtLotNo.Text = "";
            }
        }

        private void txtLotNo_TextChanged(object sender, EventArgs e)
        {
            if(txtLotNo.Text=="")
            {
                chkBlankLot.Enabled = true;
            }
            else
            {
                chkBlankLot.Enabled = false;
                chkBlankLot.Checked = false;
            }
        }

        //Added by Prajakta B. on 17/03/2020 for Bug 33252  --End

        //Added by Priyanka B on 22112019 for Bug-32747 End

        //Added by Priyanka B on 09102019 for Bug-32747 End

        private void SetFormColor()
        {
            DataSet dsColor = new DataSet();
            Color myColor = Color.Coral;
            string strSQL;
            string colorCode = string.Empty;
            strSQL = "select vcolor from Vudyog..co_mast where compid =" + this.pCompId;
            dsColor = oDataAccess.GetDataSet(strSQL, null, 20);
            if (dsColor != null)
            {
                if (dsColor.Tables.Count > 0)
                {
                    dsColor.Tables[0].TableName = "ColorInfo";
                    colorCode = dsColor.Tables["ColorInfo"].Rows[0]["vcolor"].ToString();
                }
            }

            if (!string.IsNullOrEmpty(colorCode))
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                myColor = Color.FromArgb(Convert.ToInt32(colorCode.Trim()));
            }
            this.BackColor = Color.FromArgb(myColor.R, myColor.R, myColor.G, myColor.B);

        }
    }
}
