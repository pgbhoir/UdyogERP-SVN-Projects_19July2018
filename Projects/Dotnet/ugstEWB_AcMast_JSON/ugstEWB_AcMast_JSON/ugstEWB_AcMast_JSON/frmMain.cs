using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.Diagnostics;
using System.IO;
using uBaseForm;
using System.Collections;
using udSelectPop;
using DataAccess_Net;
using udclsDGVNumericColumn;
using System.Reflection;
using System.Globalization;

namespace ugstEWB_AcMast_JSON
{
    public partial class frmMain : uBaseForm.FrmBaseForm
    {
        DataSet vDsCommon;
        DataAccess_Net.clsDataAccess oDataAccess;
        string vSqlStr;
        short vTimeOut = 50;
        String cAppPId, cAppName;
        int selectedReccnt = 0;  //Added by Priyanka B on 01022018 for Bug-31242

        DataTable tblAcMast;
        public frmMain(string[] args)
        {

            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "e Way Bill Customer / Supplier json file generation";
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
            //Added by Priyanka B on 14022018 for Bug-31242 Start
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            //Added by Priyanka B on 14022018 for Bug-31242 End

            this.pAppPath = Application.ExecutablePath;
            this.pAppPath = Path.GetDirectoryName(this.pAppPath);

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            this.mthControlSet();

            this.mthDsCommon();
            this.SetFormColor();
            this.mInsertProcessIdRecord();
            this.rbCustomer.Checked = true;
            //this.mthView();
        }
        private void btnJson_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication();
            if (this.txtJsonFile.Text == "")
            {
                //MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                MessageBox.Show("Please select the json File Path", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.btnJsonFile_Click(sender, e);
                //this.btnJson_Click(sender, e);    //Commented by Priyanka B on 07022018 for Bug-31242
            }
            else
            {
                if (selectedReccnt > 0)  //Added by Priyanka B on 01022018 for Bug-31242
                {
                    string vIgnoreColumn = "<Sel><State_Name><GrpNm>";
                    string v = "";
                    int vSelectedRec = 0;
                    string vLastCol = "<approxDistance>";
                    StringBuilder JsonString = new StringBuilder();

                    JsonString.Append("{ \n \"gstin\": " + "\"" + vDsCommon.Tables["company"].Rows[0]["GSTIN"].ToString().Trim() + "\"");
                    if (rbSupplier.Checked == true)
                    {
                        JsonString.Append(",\n \"supplier_details\" ");
                    }
                    else
                    {
                        JsonString.Append(",\n \"client_details\" ");
                    }

                    JsonString.Append(" :[ ");
                    for (int i = 0; i < dgvItDet.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dgvItDet.Rows[i].Cells["colSelect"].Value.ToString()) != false)  //Added by Priyanka B on 01022018 for Bug-31242
                        {
                            JsonString.Append("\n {");

                            for (int j = 0; j < dgvItDet.Columns.Count; j++)
                            {
                                string vColName = tblAcMast.Columns[j].ColumnName.ToString();
                                this.mthChkColumnName(ref vColName);
                                if (vIgnoreColumn.IndexOf("<" + tblAcMast.Columns[j].ColumnName.ToString() + ">") == -1)
                                {

                                    if (j < tblAcMast.Columns.Count - 1)
                                    {
                                        JsonString.Append("\n \"" + vColName + "\":" + "\"" + tblAcMast.Rows[i][j].ToString().Trim() + "\"");
                                    }
                                    else if (j == tblAcMast.Columns.Count - 1)
                                    {
                                        JsonString.Append("\n \"" + vColName + "\":" + "\"" + tblAcMast.Rows[i][j].ToString().Trim() + "\"");
                                    }

                                    if (vLastCol.IndexOf("<" + vColName + ">") == -1)
                                    {
                                        JsonString.Append(" ,");
                                    }
                                    else
                                    {
                                        JsonString.Append(" } ");
                                    }

                                }
                            }
                            if (i < tblAcMast.Rows.Count - 1)
                            {
                                JsonString.Append(" , ");
                            }
                        }
                    }
                    JsonString.Append("\n ] \n } ");
                    v = JsonString.ToString();
                    func_ascii.func_ascii f = new func_ascii.func_ascii();

                    if (rbSupplier.Checked == true)
                    {
                        string vv = "client_details";
                        vv.Replace("client_details", "supplierType");
                        v.Replace("client_details", "supplierType");
                    }
                    //MessageBox.Show(v);
                    //string fName = this.txtJsonFile.Text; //@"D:\UdyogGSTSDK\pdf_report\abc.json";  //Commented by Priyanka B on 07022018 for Bug-31242

                    //Modified by Priyanka B on 07022018 for Bug-31242 Start
                    string fName = (rbCustomer.Checked == true ? @"\ugstEWB_Cust_Mast" : @"\ugstEWB_Supp_Mast") + "_" + DateTime.Now.Date.ToString("dd/MM/yy") + "_" + DateTime.Now.ToString("hh:mm:ss tt");
                    fName = fName.Replace(".", "").Replace("-", "").Replace("/", "").Replace(":", "").Replace(" ", "") + ".Json";
                    fName = this.txtJsonFile.Text + fName;
                    //Modified by Priyanka B on 07022018 for Bug-31242 End

                    System.IO.File.WriteAllText(fName, v);
                    //Added by Priyanka B on 01022018 for Bug-31242 Start
                    MessageBox.Show("JSON file has been generated at the below location : \n" + fName.ToString(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    selectedReccnt = 0;
                    this.mthView();
                    //Added by Priyanka B on 01022018 for Bug-31242 End
                }
                //Added by Priyanka B on 01022018 for Bug-31242 Start
                else
                {
                    MessageBox.Show("Please select atleast one row of data to generate JSON file.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                //Added by Priyanka B on 01022018 for Bug-31242 End
            }
        }
        private void mthChkColumnName(ref string ColName)
        {
            if (rbSupplier.Checked == true)
            {
                switch (ColName)
                {
                    case "client_type":
                        ColName = "supplierType";
                        break;
                    case "clientGstin":
                        ColName = "supplierGstin";
                        break;
                    case "clientName":
                        ColName = "supplierName";
                        break;
                    case "clientBusAddr1":
                        ColName = "supplierBusAddr1";
                        break;
                    case "clientBusAddr2":
                        ColName = "supplierBusAddr2";
                        break;
                    case "clientBusPlace":
                        ColName = "supplierBusPlace";
                        break;
                    case "clientBusPincode":
                        ColName = "supplierBusPincode";
                        break;
                    case "clientMob":
                        ColName = "supplierMob";
                        break;
                    case "clientEmail":
                        ColName = "supplierEmail";
                        break;
                    case "clientStateCode":
                        ColName = "supplierStateCode";
                        break;
                    case "approxDistance":
                        ColName = "approxDistance";
                        break;
                }
            }
        }
        private void btnJsonFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vFolderBrowserDialog1 = new FolderBrowserDialog();
            vFolderBrowserDialog1.ShowDialog();
            //Commented by Priyanka B on 07022018 for Bug-31242 Start
            //txtJsonFile.Text = (rbCustomer.Checked == true ? @"\ugstEWB_Cust_Mast" : @"\ugstEWB_Supp_Mast") + "_" + DateTime.Now.Date.ToString("dd/MM/yy") + "_" + DateTime.Now.ToString("hh:mm:ss tt");
            //txtJsonFile.Text = txtJsonFile.Text.Replace(".", "").Replace("-", "").Replace("/", "").Replace(":", "").Replace(" ", "") + ".Json";
            //Commented by Priyanka B on 07022018 for Bug-31242 End
            txtJsonFile.Text = "";  //Added by Priyanka B on 07022018 for Bug-31242
            txtJsonFile.Text = vFolderBrowserDialog1.SelectedPath + txtJsonFile.Text;
        }
        private void mthView()
        {
            dgvItDet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            //string vGrpNm = (txtGrpNm.Text.Trim() != "" ? "('" + txtGrpNm.Text.Trim() + "')" : "");  //Commented by Priyanka B on 01022018 for Bug-31242
            string vGrpNm = (txtGrpNm.Text.Trim() != "" ? "(''" + txtGrpNm.Text.Trim() + "'')" : "");  //Modified by Priyanka B on 01022018 for Bug-31242
            string vCS = (chkCusSupp.Checked == true ? "CS" : (rbCustomer.Checked == true ? "C" : "S"));
            vSqlStr = "Execute [USP_Ent_eWayBill_ACMast_json] " + (this.txtAcNm.Text.Trim() != "" ? "'" + this.txtAcNm.Text.Trim() + "'" : "''") + (vGrpNm != "" ? ",'" + vGrpNm + "'" : ",''") + (vCS != "" ? ",'" + vCS + "'" : ",''");
            //MessageBox.Show(vSqlStr);
            tblAcMast = oDataAccess.GetDataTable(vSqlStr, null, 25);

            this.dgvItDet.Columns.Clear();
            this.dgvItDet.DataSource = this.tblAcMast;
            this.dgvItDet.Columns.Clear();


            System.Windows.Forms.DataGridViewCheckBoxColumn colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            colSelect.HeaderText = "Select";
            colSelect.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSelect.Width = 40;
            colSelect.Name = "colSelect";
            colSelect.ReadOnly = false;
            this.dgvItDet.Columns.Add(colSelect);


            System.Windows.Forms.DataGridViewTextBoxColumn client_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            client_type.HeaderText = "Reg/UnReg";
            client_type.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            client_type.Width = 100;
            client_type.Name = "client_type";
            client_type.ReadOnly = true;
            this.dgvItDet.Columns.Add(client_type);


            System.Windows.Forms.DataGridViewTextBoxColumn clientGstin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            clientGstin.HeaderText = "GSTIN";
            clientGstin.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            clientGstin.Width = 100;
            clientGstin.Name = "clientGstin";
            clientGstin.ReadOnly = true;
            this.dgvItDet.Columns.Add(clientGstin);

            System.Windows.Forms.DataGridViewTextBoxColumn clientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            clientName.HeaderText = "Legal Name";
            clientName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            clientName.Width = 90;
            clientName.Name = "clientName";
            clientName.ReadOnly = true;
            this.dgvItDet.Columns.Add(clientName);

            System.Windows.Forms.DataGridViewTextBoxColumn clientBusAddr1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            clientBusAddr1.HeaderText = "Address1";
            clientBusAddr1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            clientBusAddr1.Width = 100;
            clientBusAddr1.Name = "clientBusAddr1";
            clientBusAddr1.ReadOnly = true;
            this.dgvItDet.Columns.Add(clientBusAddr1);

            System.Windows.Forms.DataGridViewTextBoxColumn clientBusAddr2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            clientBusAddr2.HeaderText = "Address2";
            clientBusAddr2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            clientBusAddr2.Width = 100;
            clientBusAddr2.Name = "clientBusAddr2";
            clientBusAddr2.ReadOnly = false;
            this.dgvItDet.Columns.Add(clientBusAddr2);

            System.Windows.Forms.DataGridViewTextBoxColumn clientBusPlace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            clientBusPlace.HeaderText = "Place";
            clientBusPlace.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            clientBusPlace.Width = 100;
            clientBusPlace.Name = "clientBusPlace";
            clientBusPlace.ReadOnly = false;
            this.dgvItDet.Columns.Add(clientBusPlace);

            System.Windows.Forms.DataGridViewTextBoxColumn clientBusPincode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            clientBusPincode.HeaderText = "Pin Code";
            clientBusPincode.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            clientBusPincode.Width = 100;
            clientBusPincode.Name = "clientBusPincode";
            clientBusPincode.ReadOnly = false;
            this.dgvItDet.Columns.Add(clientBusPincode);

            System.Windows.Forms.DataGridViewTextBoxColumn clientMob = new System.Windows.Forms.DataGridViewTextBoxColumn();
            clientMob.HeaderText = "Mobile No";
            clientMob.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            clientMob.Width = 100;
            clientMob.Name = "clientMob";
            clientMob.ReadOnly = false;
            this.dgvItDet.Columns.Add(clientMob);

            System.Windows.Forms.DataGridViewTextBoxColumn clientEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            clientEmail.HeaderText = "Email Id";
            clientEmail.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            clientEmail.Width = 100;
            clientEmail.Name = "clientEmail";
            clientEmail.ReadOnly = true;
            this.dgvItDet.Columns.Add(clientEmail);

            System.Windows.Forms.DataGridViewTextBoxColumn StateNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            StateNm.HeaderText = "State Name";
            StateNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            StateNm.Width = 100;
            StateNm.Name = "StateNm";
            StateNm.ReadOnly = false;
            this.dgvItDet.Columns.Add(StateNm);

            //Commented by Priyanka B on 09022018 for Bug-31242 Start
            //udclsDGVNumericColumn.CNumEditDataGridViewColumn clientStateCode = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            //clientStateCode.HeaderText = "State Code";
            //clientStateCode.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            //clientStateCode.Width = 100;
            //clientStateCode.Name = "clientStateCode";
            //clientStateCode.DecimalLength = 0;
            //clientStateCode.ReadOnly = false;
            //this.dgvItDet.Columns.Add(clientStateCode);
            //Commented by Priyanka B on 09022018 for Bug-31242 End

            //Modified by Priyanka B on 09022018 for Bug-31242 Start
            System.Windows.Forms.DataGridViewTextBoxColumn clientStateCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            clientStateCode.HeaderText = "State Code";
            clientStateCode.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            clientStateCode.Width = 100;
            clientStateCode.Name = "clientStateCode";
            clientStateCode.ReadOnly = true;
            this.dgvItDet.Columns.Add(clientStateCode);
            //Modified by Priyanka B on 09022018 for Bug-31242 End

            udclsDGVNumericColumn.CNumEditDataGridViewColumn approxDistance = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            approxDistance.HeaderText = "Approximate Distance";
            approxDistance.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            approxDistance.Width = 100;
            approxDistance.Name = "approxDistance";
            approxDistance.DecimalLength = 0;
            approxDistance.ReadOnly = false;
            this.dgvItDet.Columns.Add(approxDistance);

            System.Windows.Forms.DataGridViewTextBoxColumn GrpNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //GrpNm.HeaderText = "Item Group";  //Commented by Priyanka B on 08022018 for Bug-31242
            GrpNm.HeaderText = "Account Group";  //Modified by Priyanka B on 08022018 for Bug-31242
            GrpNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            GrpNm.Width = 120;
            GrpNm.Name = "GrpNm";
            GrpNm.ReadOnly = true;
            this.dgvItDet.Columns.Add(GrpNm);


            dgvItDet.Columns["colSelect"].DataPropertyName = "Sel";
            dgvItDet.Columns["client_type"].DataPropertyName = "client_type";
            dgvItDet.Columns["clientGstin"].DataPropertyName = "clientGstin";
            dgvItDet.Columns["clientName"].DataPropertyName = "clientName";
            dgvItDet.Columns["clientBusAddr1"].DataPropertyName = "clientBusAddr1";
            dgvItDet.Columns["clientBusAddr2"].DataPropertyName = "clientBusAddr2";
            dgvItDet.Columns["clientBusPlace"].DataPropertyName = "clientBusPlace";
            dgvItDet.Columns["clientBusPincode"].DataPropertyName = "clientBusPincode";
            dgvItDet.Columns["clientMob"].DataPropertyName = "clientMob";
            dgvItDet.Columns["clientEmail"].DataPropertyName = "clientEmail";
            dgvItDet.Columns["clientStateCode"].DataPropertyName = "clientStateCode";
            dgvItDet.Columns["StateNm"].DataPropertyName = "State_Name";
            dgvItDet.Columns["approxDistance"].DataPropertyName = "approxDistance";
            dgvItDet.Columns["GrpNm"].DataPropertyName = "GrpNm";

            //dgvItDet.AutoResizeColumns();

        }
        private void mthControlSet()
        {
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnAcNm.Image = Image.FromFile(fName);
                this.btnGrpNm.Image = Image.FromFile(fName);
            }
            fName = this.pAppPath + @"\bmp\loc-on.gif";
            if (File.Exists(fName) == true)
            {
                this.btnJson.Image = Image.FromFile(fName);
            }
            fName = this.pAppPath + @"\bmp\report_rtf.gif";
            if (File.Exists(fName) == true)
            {
                this.btnJsonFile.Image = Image.FromFile(fName);
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
        private void mcheckCallingApplication()
        {
            Process pProc;
            Boolean procExists = true;
            try
            {
                pProc = Process.GetProcessById(Convert.ToInt16(this.pPApplPID));
                String pName = pProc.ProcessName;
                string pName1 = this.pPApplName.Substring(0, this.pPApplName.IndexOf("."));
                if (pName.ToUpper() != pName1.ToUpper())
                {
                    procExists = false;
                }
            }
            catch (Exception)
            {
                procExists = false;

            }
            if (procExists == false)
            {
                MessageBox.Show("Can't proceed,Main Application " + this.pPApplText + " is closed", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //Application.Exit();
            }
        }
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "ugstEWB_AcMast_JSON.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            //sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";  //Commented by Priyanka B on 14022018 for Bug-31242
            sqlstr = "Set Dateformat DMY insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";  //Modified by Priyanka B on 14022018 for Bug-31242
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
        }

        private void mDeleteProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            if (string.IsNullOrEmpty(this.pPApplName) || this.pPApplPID == 0 || string.IsNullOrEmpty(this.cAppName) || string.IsNullOrEmpty(this.cAppPId))
            {
                return;
            }
            DataSet dsData = new DataSet();
            string sqlstr;
            sqlstr = " Delete from vudyog..ExtApplLog where pApplNm='" + this.pPApplName + "' and pApplId=" + this.pPApplPID + " and cApplNm= '" + cAppName + "' and cApplId= " + cAppPId;
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
        }

        private void rbCustomer_CheckedChanged(object sender, EventArgs e)
        {
            this.txtJsonFile.Text = "";
            this.mthView();
        }

        private void rbSupplier_CheckedChanged(object sender, EventArgs e)
        {
            this.txtJsonFile.Text = "";

            //Added by Priyanka B on 08022018 for Bug-31242 Start
            this.txtAcNm.Text = "";
            this.txtGrpNm.Text = "";
            //Added by Priyanka B on 08022018 for Bug-31242 End
            this.mthView();
        }

        private void chkCusSupp_CheckedChanged(object sender, EventArgs e)
        {
            this.txtAcNm.Text = "";
            this.txtGrpNm.Text = "";
            //Added by Priyanka B on 07022018 for Bug-31242 Start
            if (chkCusSupp.Checked)
            {
                this.rbCustomer.Checked = false;
                this.rbSupplier.Checked = false;
                this.rbCustomer.Enabled = false;
                this.rbSupplier.Enabled = false;
            }
            else
            {
                this.rbCustomer.Checked = true;
                this.rbSupplier.Checked = false;
                this.rbCustomer.Enabled = true;
                this.rbSupplier.Enabled = true;
            }
            //Added by Priyanka B on 07022018 for Bug-31242 End
            this.mthView();
        }

        private void btnGrpNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            this.txtGrpNm.Text = "";
            vSqlStr = "Select [Ac_Group_Name]=''  union  Select [AC_Group_Name] From AC_GROUP_MAST Order by [AC_Group_Name]";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 20);


            DataView dvw = tblPop.DefaultView;// tDs.Tables[0].DefaultView;

            VForText = "Select Group Name";
            vSearchCol = "AC_Group_Name";
            vDisplayColumnList = "AC_Group_Name:Group Name";
            vReturnCol = "AC_Group_Name";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.Icon = this.pFrmIcon;  //Added by Priyanka B on 01022018 for Bug-31242
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtGrpNm.Text = oSelectPop.pReturnArray[0];
                this.mthView();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnSelctAll_Click(object sender, EventArgs e)
        {
            //Added by Priyanka B on 01022018 for Bug-31242 Start
            if (dgvItDet.Rows.Count > 0)
            {
                if (btnSelctAll.Text == "Select All")
                {
                    for (int i = 0; i < dgvItDet.Rows.Count; i++)
                    {
                        dgvItDet.Rows[i].Cells["colSelect"].Value = true;
                    }
                    btnSelctAll.Text = "DeSelect All";
                }
                else
                {
                    for (int i = 0; i < dgvItDet.Rows.Count; i++)
                    {
                        dgvItDet.Rows[i].Cells["colSelect"].Value = false;
                    }
                    btnSelctAll.Text = "Select All";
                }
            }
            //Added by Priyanka B on 01022018 for Bug-31242 End
        }

        private void dgvItDet_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Added by Priyanka B on 01022018 for Bug-31242 Start
            if (dgvItDet.Rows.Count > 0)
            {
                if (e.ColumnIndex == 0)
                {
                    if (Convert.ToBoolean(dgvItDet.Rows[e.RowIndex].Cells["colSelect"].Value.ToString()) == true)
                        selectedReccnt++;
                    else
                        selectedReccnt--;
                }
            }
            //Added by Priyanka B on 01022018 for Bug-31242 End
        }

        private void btnAcNm_Click(object sender, EventArgs e)
        {
            //Added by Priyanka B on 01022018 for Bug-31242 Start
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            this.txtAcNm.Text = "";
            //Commented by Priyanka B on 07022018 for Bug-31242 Start
            //vSqlStr = "Select [Ac_Name]=''  union  Select [AC_Name] From AC_MAST Order by [AC_Name]";
            //tblPop = oDataAccess.GetDataTable(vSqlStr, null, 20);
            //Commented by Priyanka B on 07022018 for Bug-31242 Start

            //Modified by Priyanka B on 07022018 for Bug-31242 Start
            string vCS = "";
            vCS = (chkCusSupp.Checked == true ? "CS" : (rbCustomer.Checked == true ? "C" : "S"));
            //vSqlStr = "Execute [USP_Ent_eWayBill_Filter] '" + vCS.ToString().Trim() + "','',''";   //Commented by Priyanka B on 15022018 for Bug-31242
            vSqlStr = "Execute [USP_Ent_eWayBill_Filter] '" + vCS.ToString().Trim() + "'";  //Modified by Priyanka B on 15022018 for Bug-31242
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);
            //Modified by Priyanka B on 07022018 for Bug-31242 End

            DataView dvw = tblPop.DefaultView;// tDs.Tables[0].DefaultView;
            dvw.Sort = "ac_name";  //Added by Priyanka B on 07022018 for Bug-31242

            VForText = "Select Account Name";
            vSearchCol = "AC_Name";
            vDisplayColumnList = "AC_Name:Account Name";
            vReturnCol = "AC_Name";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.Icon = this.pFrmIcon;  //Added by Priyanka B on 01022018 for Bug-31242
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtAcNm.Text = oSelectPop.pReturnArray[0];
                this.mthView();
            }
            //Added by Priyanka B on 01022018 for Bug-31242 End
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

    }
}
