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


namespace ugstEWB_ItMast_JSON
{
    public partial class frmMain : uBaseForm.FrmBaseForm
    {
        DataSet vDsCommon;
        DataAccess_Net.clsDataAccess oDataAccess;
        string vSqlStr;
        short vTimeOut = 50;
        String cAppPId, cAppName;
        int selectedReccnt = 0;  //Added by Priyanka B on 01022018 for Bug-31241

        DataTable tblItMast;
        public frmMain(string[] args)
        {
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            //this.pFrmCaption = "e Way Bill Item Master json file generation";  //Commented by Priyanka B on 05022018 for Bug-31241
            this.pFrmCaption = "e Way Bill Product Master json file generation";  //Modified by Priyanka B on 05022018 for Bug-31241
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
            //Added by Priyanka B on 14022018 for Bug-31241 Start
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            //Added by Priyanka B on 14022018 for Bug-31241 End

            this.pAppPath = Application.ExecutablePath;
            this.pAppPath = Path.GetDirectoryName(this.pAppPath);
            this.mthControlSet();


            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.mthDsCommon();
            this.SetFormColor();
            this.mInsertProcessIdRecord();
            try
            {
                this.mthView();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        private void mthView()
        {
            dgvItDet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));

            vSqlStr = "Execute [USP_Ent_eWayBill_ItMast_json] " + (this.txtItNm.Text.Trim() != "" ? "'" + this.txtItNm.Text.Trim() + "'" : "''") + (this.txtGrpNm.Text.Trim() != "" ? ",'" + this.txtGrpNm.Text.Trim() + "'" : ",''") + (this.txtItTyp.Text.Trim() != "" ? ",'" + this.txtItTyp.Text.Trim() + "'" : ",''") ;
            tblItMast = oDataAccess.GetDataTable(vSqlStr, null, 25);

            this.dgvItDet.Columns.Clear();
            this.dgvItDet.DataSource = this.tblItMast;
            this.dgvItDet.Columns.Clear();

            System.Windows.Forms.DataGridViewCheckBoxColumn colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            //this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            colSelect.HeaderText = "Select";
            colSelect.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSelect.Width = 40;
            colSelect.Name = "colSelect";
            colSelect.ReadOnly = false;
            this.dgvItDet.Columns.Add(colSelect);

            System.Windows.Forms.DataGridViewTextBoxColumn productName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            productName.HeaderText = "Product Name";
            productName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            productName.Width = 100;
            productName.Name = "productName";
            productName.ReadOnly = true;
            this.dgvItDet.Columns.Add(productName);


            System.Windows.Forms.DataGridViewTextBoxColumn measuringUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            measuringUnit.HeaderText = "Measuring Unit";
            measuringUnit.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            measuringUnit.Width = 100;
            measuringUnit.Name = "measuringUnit";
            measuringUnit.ReadOnly = true;
            this.dgvItDet.Columns.Add(measuringUnit);

            System.Windows.Forms.DataGridViewTextBoxColumn description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            description.HeaderText = "Description";
            description.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            description.Width = 90;
            description.Name = "description";
            description.ReadOnly = true;
            this.dgvItDet.Columns.Add(description);

            System.Windows.Forms.DataGridViewTextBoxColumn hsnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            hsnCode.HeaderText = "HSN Code";
            hsnCode.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            hsnCode.Width = 100;
            hsnCode.Name = "hsnCode";
            hsnCode.ReadOnly = true;
            this.dgvItDet.Columns.Add(hsnCode);

            udclsDGVNumericColumn.CNumEditDataGridViewColumn cgst = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            cgst.HeaderText = "CGST %";
            cgst.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            cgst.Width = 100;
            cgst.Name = "cgst";
            cgst.ReadOnly = false ;
            cgst.DecimalLength = 3;
            this.dgvItDet.Columns.Add(cgst);

            udclsDGVNumericColumn.CNumEditDataGridViewColumn sgst = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            sgst.HeaderText = "SGST %";
            sgst.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            sgst.Width = 100;
            sgst.Name = "sgst";
            sgst.ReadOnly = false;
            sgst.DecimalLength = 3;
            this.dgvItDet.Columns.Add(sgst);

            udclsDGVNumericColumn.CNumEditDataGridViewColumn igst = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            igst.HeaderText = "IGST %";
            igst.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            igst.Width = 100;
            igst.Name = "igst";
            igst.ReadOnly =false;
            igst.DecimalLength = 3;
            this.dgvItDet.Columns.Add(igst);

            //udclsDGVNumericColumn.CNumEditDataGridViewColumn cess = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();   //Commented by Priyanka B on 05022018 for Bug-31241
            System.Windows.Forms.DataGridViewTextBoxColumn cess = new System.Windows.Forms.DataGridViewTextBoxColumn();  //Modified by Priyanka B on 05022018 for Bug-31241
            cess.HeaderText = "Cess %";
            cess.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            cess.Width = 100;
            cess.Name = "cess";
            //cess.DecimalLength=3;  //Commented by Priyanka B on 05022018 for Bug-31241
            cess.ReadOnly = false;
            this.dgvItDet.Columns.Add(cess);

            udclsDGVNumericColumn.CNumEditDataGridViewColumn cessAdvol = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
            cessAdvol.HeaderText = "Cess Advol %";
            cessAdvol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            cessAdvol.Width = 100;
            cessAdvol.Name = "cessAdvol";
            cessAdvol.DecimalLength = 3;
            cessAdvol.ReadOnly = false;
            this.dgvItDet.Columns.Add(cessAdvol);


            System.Windows.Forms.DataGridViewTextBoxColumn ColItGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //ColItGroup.HeaderText = "Item Group";  //Commented by Priyanka B on 05022018 for Bug-31241
            ColItGroup.HeaderText = "Product Group";  //Modified by Priyanka B on 05022018 for Bug-31241
            ColItGroup.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            ColItGroup.Width = 120;
            ColItGroup.Name = "ColItGroup";
            ColItGroup.ReadOnly = true;
            this.dgvItDet.Columns.Add(ColItGroup);

            System.Windows.Forms.DataGridViewTextBoxColumn colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colType.HeaderText = "Type";
            colType.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colType.Width = 100;
            colType.Name = "colType";
            colType.ReadOnly = true;
            this.dgvItDet.Columns.Add(colType);
            
            dgvItDet.Columns["colSelect"].DataPropertyName = "Sel";
            dgvItDet.Columns["productName"].DataPropertyName = "productName";
            dgvItDet.Columns["measuringUnit"].DataPropertyName = "measuringUnit";
            dgvItDet.Columns["description"].DataPropertyName = "description";
            dgvItDet.Columns["hsnCode"].DataPropertyName = "hsnCode";
            dgvItDet.Columns["cgst"].DataPropertyName = "cgst";
            dgvItDet.Columns["sgst"].DataPropertyName = "sgst";
            dgvItDet.Columns["igst"].DataPropertyName = "igst";
            dgvItDet.Columns["cess"].DataPropertyName = "cess";
            dgvItDet.Columns["cessAdvol"].DataPropertyName = "cessAdvol";
            //Commented by Priyanka B on 05022018 for Bug-31241 Start
            //dgvItDet.Columns["ColItGroup"].DataPropertyName = "ColItGroup"; 
            //dgvItDet.Columns["colType"].DataPropertyName = "colType";
            //Commented by Priyanka B on 05022018 for Bug-31241 End

            //Modified by Priyanka B on 05022018 for Bug-31241 Start
            dgvItDet.Columns["ColItGroup"].DataPropertyName = "Group";
            dgvItDet.Columns["colType"].DataPropertyName = "Type";
            //Modified by Priyanka B on 05022018 for Bug-31241 End

        }
        private void mthControlSet()
        {
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnItNm.Image = Image.FromFile(fName);
                this.btnGrpNm.Image = Image.FromFile(fName);
                this.btnItTyp.Image = Image.FromFile(fName);
                this.btnStatus.Image = Image.FromFile(fName);
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

        private void btnJson_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication();
            if (this.txtJsonFile.Text == "")
            {
                //MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                MessageBox.Show("Please select the json File Path", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.btnJsonFile_Click(sender, e);
                //this.btnJson_Click(sender, e);  //Commented by Priyanka B on 07022018 for Bug-31241
            }
            else
            {
                if (selectedReccnt > 0)  //Added by Priyanka B on 01022018 for Bug-31241
                {
                    string vIgnoreColumn = "<Sel><It_Code><Group>,<type>";
                    string v = "";
                    int vSelectedRec = 0;
                    string vLastCol = "<cessAdvol>";
                    StringBuilder JsonString = new StringBuilder();

                    JsonString.Append("{ \"gstin\": " + "\"" + vDsCommon.Tables["company"].Rows[0]["GSTIN"].ToString().Trim() + "\"");
                    JsonString.Append(",\n \"product_details\" ");
                    JsonString.Append(" :[ ");
                    for (int i = 0; i < tblItMast.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(tblItMast.Rows[i]["Sel"].ToString()) != false)  //Added by Priyanka B on 01022018 for Bug-31241
                        {
                            JsonString.Append("\n {");

                            for (int j = 0; j < tblItMast.Columns.Count; j++)
                            {

                                if (vIgnoreColumn.IndexOf("<" + tblItMast.Columns[j].ColumnName.ToString() + ">") == -1)
                                {

                                    if (j < tblItMast.Columns.Count - 1)
                                    {
                                        JsonString.Append("\n \"" + tblItMast.Columns[j].ColumnName.ToString() + "\":" + "\"" + tblItMast.Rows[i][j].ToString().Trim() + "\"");
                                    }
                                    else if (j == tblItMast.Columns.Count - 1)
                                    {
                                        JsonString.Append("\n \"" + tblItMast.Columns[j].ColumnName.ToString() + "\":" + "\"" + tblItMast.Rows[i][j].ToString().Trim() + "\"");
                                    }

                                    if (vLastCol.IndexOf("<" + tblItMast.Columns[j].ColumnName.ToString() + ">") == -1)
                                    {
                                        JsonString.Append(" ,");
                                    }
                                    else
                                    {
                                        JsonString.Append(" }");
                                    }

                                }
                            }
                            if (i < tblItMast.Rows.Count - 1)
                            {
                                JsonString.Append(" ,");
                            }
                        }

                    }
                    JsonString.Append("\n ] \n } "); //Added by Priyanka B on 01022018 for Bug-31241
                    v = JsonString.ToString();

                    //MessageBox.Show(v);
                    //string fName = this.txtJsonFile.Text; //@"D:\UdyogGSTSDK\pdf_report\abc.json";  //Commented by Priyanka B on 07022018 for Bug-31241

                    //Modified by Priyanka B on 07022018 for Bug-31241 Start
                    string fName = @"\ugstEWB_It_Mast" + "_" + DateTime.Now.Date.ToString("dd/MM/yy") + "_" + DateTime.Now.ToString("hh:mm:ss tt");
                    fName = fName.Replace(".", "").Replace("-", "").Replace("/", "").Replace(":", "").Replace(" ", "") + ".Json";
                    fName = this.txtJsonFile.Text + fName;
                    //Modified by Priyanka B on 07022018 for Bug-31241 End

                    System.IO.File.WriteAllText(fName, v);
                    //Added by Priyanka B on 01022018 for Bug-31241 Start
                    MessageBox.Show("JSON file has been generated at the below location : \n" + fName.ToString(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.mthView();
                    selectedReccnt = 0;
                    //Added by Priyanka B on 01022018 for Bug-31241 End
                }
                //Added by Priyanka B on 01022018 for Bug-31241 Start
                else
                {
                    MessageBox.Show("Please select atleast one row of data to generate JSON file.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                //Added by Priyanka B on 01022018 for Bug-31241 End
            }
        }

        private void btnJsonFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vFolderBrowserDialog1 = new FolderBrowserDialog();
            vFolderBrowserDialog1.ShowDialog();
            //Commented by Priyanka B on 07022018 for Bug-31241 Start
            //txtJsonFile.Text = @"\ugstEWB_It_Mast" + "_" + DateTime.Now.Date.ToString("dd/MM/yy") + "_" + DateTime.Now.ToString("hh:mm:ss tt");
            //txtJsonFile.Text = txtJsonFile.Text.Replace(".", "").Replace("-", "").Replace("/", "").Replace(":", "").Replace(" ", "") + ".Json";
            //Commented by Priyanka B on 07022018 for Bug-31241 End
            txtJsonFile.Text = "";  //Added by Priyanka B on 07022018 for Bug-31241
            txtJsonFile.Text = vFolderBrowserDialog1.SelectedPath + txtJsonFile.Text;
        }

        private void gbxFilter_Enter(object sender, EventArgs e)
        {

        }

        private void btnItTyp_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            //this.txtItTyp.Text = "";  //Commented by Priyanka B on 08022018 for Bug-31241
            vSqlStr = "Select [Type]=''  union  Select distinct [Type] From ITEM_TYPE Order by [Type]";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 20);

            DataView dvw = tblPop.DefaultView;// tDs.Tables[0].DefaultView;

            //VForText = "Select Item Type";  //Commented by Priyanka B on 01022018 for Bug-31241
            VForText = "Select Product Type";  //Modified by Priyanka B on 01022018 for Bug-31241
            vSearchCol = "Type";
            //vDisplayColumnList = "Type:Item Type";  //Commented by Priyanka B on 01022018 for Bug-31241
            vDisplayColumnList = "Type:Product Type";  //Modified by Priyanka B on 01022018 for Bug-31241
            vReturnCol = "Type";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.Icon = this.pFrmIcon;  //Added by Priyanka B on 01022018 for Bug-31241
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtItTyp.Text = oSelectPop.pReturnArray[0];
                //Modified by Priyanka B on 08022018 for Bug-31241 Start
                selectedReccnt = 0;
                btnSelctAll.Text = "Select All";
                this.txtItNm.Text = "";
                this.txtGrpNm.Text = "";
                //Modified by Priyanka B on 08022018 for Bug-31241 End
                this.mthView();
            }
            ////Added by Priyanka B on 01022018 for Bug-31241 Start
            //if (dgvItDet.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dgvItDet.Rows.Count; i++)
            //    {
            //        dgvItDet.Rows[i].Cells["colSelect"].Value = false;
            //    }
            //    btnSelctAll.Text = "Select All";
            //}
            ////Added by Priyanka B on 01022018 for Bug-31241 End
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
                Application.Exit();
            }
        }
        private void mInsertProcessIdRecord()/*Added Rup 07/03/2011*/
        {
            
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "ugstEWB_ItMast_JSON.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            //sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";  //Commented by Priyanka B on 14022018 for Bug-31241
            sqlstr = "Set Dateformat DMY insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";  //Modified by Priyanka B on 14022018 for Bug-31241
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
        private void btnGrpNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            this.txtGrpNm.Text = "";
            vSqlStr = "Select [It_Group_Name]=''  union  Select [It_Group_Name] From ITEM_GROUP ";
            if (this.txtItTyp.Text !="")
            {
                vSqlStr = vSqlStr + " Where [Type]='" + this.txtItTyp.Text.Trim() + "'";
            }
            vSqlStr = vSqlStr + "  Order by [It_Group_Name]";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 20);


            DataView dvw = tblPop.DefaultView;// tDs.Tables[0].DefaultView;

            //VForText = "Select Item Group";  //Commented by Priyanka B on 01022018 for Bug-31241
            VForText = "Select Product Group"; //Modified by Priyanka B on 01022018 for Bug-31241
            vSearchCol = "It_Group_Name";
            //vDisplayColumnList = "It_Group_Name:Item Group";  //Commented by Priyanka B on 01022018 for Bug-31241
            vDisplayColumnList = "It_Group_Name:Product Group"; //Modified by Priyanka B on 01022018 for Bug-31241
            vReturnCol = "It_Group_Name";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.Icon = this.pFrmIcon;  //Added by Priyanka B on 01022018 for Bug-31241
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtGrpNm.Text = oSelectPop.pReturnArray[0];
                this.mthView();
                //Modified by Priyanka B on 08022018 for Bug-31241 Start
                selectedReccnt = 0;
                btnSelctAll.Text = "Select All";
                //Modified by Priyanka B on 08022018 for Bug-31241 End
            }
            ////Added by Priyanka B on 01022018 for Bug-31241 Start
            //if (dgvItDet.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dgvItDet.Rows.Count; i++)
            //    {
            //        dgvItDet.Rows[i].Cells["colSelect"].Value = false;
            //    }
            //    btnSelctAll.Text = "Select All";
            //}
            ////Added by Priyanka B on 01022018 for Bug-31241 End
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDeleteProcessIdRecord();
        }

        private void btnItNm_Click(object sender, EventArgs e)
        {
            //Added by Priyanka B on 01022018 for Bug-31241 Start
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            //Commented by Priyanka B on 08022018 for Bug-31241 Start
            //this.txtItNm.Text = "";
            //vSqlStr = "Select [It_Name]=''  union  Select [It_Name] From IT_MAST ";
            //vSqlStr = vSqlStr + "  Order by [It_Name]";
            //tblPop = oDataAccess.GetDataTable(vSqlStr, null, 20);
            //Commented by Priyanka B on 08022018 for Bug-31241 End

            //Modified by Priyanka B on 08022018 for Bug-31241 Start
            //vSqlStr = "Execute [USP_Ent_eWayBill_Filter] '" + this.txtItTyp.Text.ToString().Trim() + "','',''";  //Commented by Priyanka B on 15022018 for Bug-31241
            vSqlStr = "Execute [USP_Ent_eWayBill_Filter] '" + this.txtItTyp.Text.ToString().Trim() + "'";  //Modified by Priyanka B on 15022018 for Bug-31241
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 25);
            //Modified by Priyanka B on 08022018 for Bug-31241 End

            DataView dvw = tblPop.DefaultView;// tDs.Tables[0].DefaultView;
            dvw.Sort = "it_name";  //Added by Priyanka B on 08022018 for Bug-31241

            VForText = "Select Product Name";
            vSearchCol = "It_Name";
            vDisplayColumnList = "It_Name:Product Name";
            vReturnCol = "It_Name";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.Icon = this.pFrmIcon;  //Added by Priyanka B on 01022018 for Bug-31241
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtItNm.Text = oSelectPop.pReturnArray[0];
                this.mthView();
                //Modified by Priyanka B on 08022018 for Bug-31241 Start
                selectedReccnt = 0;
                btnSelctAll.Text = "Select All";
                //Modified by Priyanka B on 08022018 for Bug-31241 End
            }
            //if (dgvItDet.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dgvItDet.Rows.Count; i++)
            //    {
            //        dgvItDet.Rows[i].Cells["colSelect"].Value = false;
            //    }
            //    btnSelctAll.Text = "Select All";
            //}
            //Added by Priyanka B on 01022018 for Bug-31241 End
        }

        private void btnSelctAll_Click(object sender, EventArgs e)
        {
            //Added by Priyanka B on 01022018 for Bug-31241 Start
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
            //Added by Priyanka B on 01022018 for Bug-31241 End
        }

        private void dgvItDet_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Added by Priyanka B on 01022018 for Bug-31241 Start
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
            //Added by Priyanka B on 01022018 for Bug-31241 End
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dgvItDet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
