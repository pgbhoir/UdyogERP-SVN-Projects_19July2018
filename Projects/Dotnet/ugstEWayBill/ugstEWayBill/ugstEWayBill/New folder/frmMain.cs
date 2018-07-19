using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using uBaseForm;
using System.Collections;
using udSelectPop;
using DataAccess_Net;
using System.Diagnostics;

//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;

namespace ugstEWayBill
{
    public partial class frmMain : uBaseForm.FrmBaseForm
    {
        DataSet vDsCommon;
        DataAccess_Net.clsDataAccess oDataAccess;
        DataTable tblGSTIN;
        string vSqlStr = "";
        DataTable jsonDataTable = new DataTable();
        //Added by Priyanka B on 01022018 for Bug-31240 Start
        int selectedReccnt = 0; 
        String cAppPId, cAppName;
        short vTimeOut = 50;
        //Added by Priyanka B on 01022018 for Bug-31240 End

        public frmMain(string[] args)
        {
            InitializeComponent();
            DataAccess_Net.clsDataAccess oDataAccess;

            //this.pDisableCloseBtn = true;  /* close disable  */

            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "e Way Bill Generation";
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

            //this.pCompId = 3;//Convert.ToInt16(args[0]);
            //this.pComDbnm = "U031718";// args[1];
            //this.pServerName = @"AIPLLTM035\SQLEXPRESS2012";// args[2];
            //this.pUserId = "sa";// args[3];
            //this.pPassword = "sa@1985r2";// args[4];
            //this.pPApplRange = "13004"; //args[5];
            //this.pAppUerName = "r";// args[6];
            //this.pFrmCaption = "GSTIN Validation";
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
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
            this.txtTrnName.Text = "Sales";
            //this.mthView();
        }
        private void mthView()
        {
            dgvGSTIN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));

            vSqlStr = "Execute [USP_Ent_eWayBill_Bulk] " + (this.txtGSTIN.Text.Trim() != "" ? "'" + this.txtGSTIN.Text.Trim() + "'" : "''") + (this.txtPartyNm.Text.Trim() != "" ? ",'" + this.txtPartyNm.Text.Trim() + "'" : ",''") ;
            vSqlStr = vSqlStr  +","+(this.txtTrnName.Text.Trim()!="" ?  "'" + this.txtTrnName.Text.Trim() + "'" : "''");
            //+",'','',''";
            tblGSTIN = oDataAccess.GetDataTable(vSqlStr, null, 25);

            this.dgvGSTIN.Columns.Clear();
            this.dgvGSTIN.DataSource = this.tblGSTIN;
            this.dgvGSTIN.Columns.Clear();

            System.Windows.Forms.DataGridViewCheckBoxColumn colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            //this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            colSelect.HeaderText = "Select";
            colSelect.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSelect.Width = 40;
            colSelect.Name = "colSelect";
            colSelect.ReadOnly = false;
            this.dgvGSTIN.Columns.Add(colSelect);

            //System.Windows.Forms.DataGridViewButtonColumn colGenEWAYBN = new System.Windows.Forms.DataGridViewButtonColumn();
            //colGenEWAYBN.HeaderText = "Generate e Way Bill ";
            //colGenEWAYBN.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            //colGenEWAYBN.Width = 145;
            //colGenEWAYBN.Name = "colGenEWAYBN";
            //colGenEWAYBN.ReadOnly = true;
            //this.dgvGSTIN.Columns.Add(colGenEWAYBN);

            System.Windows.Forms.DataGridViewTextBoxColumn colEWAYN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEWAYN.HeaderText = "e WAY Bill No.";
            colEWAYN.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colEWAYN.Width = 100;
            colEWAYN.Name = "colEWAYN";
            colEWAYN.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colEWAYN);


            System.Windows.Forms.DataGridViewTextBoxColumn colEWAYDt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEWAYDt.HeaderText = "e WAY Bill Dt.";
            colEWAYDt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colEWAYDt.Width = 100;
            colEWAYDt.Name = "colEWAYDt";
            colEWAYDt.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colEWAYDt);

            System.Windows.Forms.DataGridViewTextBoxColumn colSuppTyp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSuppTyp.HeaderText = "Supply Type";
            colSuppTyp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSuppTyp.Width = 90;
            colSuppTyp.Name = "colSuppTyp";
            colSuppTyp.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colSuppTyp);

            System.Windows.Forms.DataGridViewTextBoxColumn colSubTyp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSubTyp.HeaderText = "Sub Type";
            colSubTyp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colSubTyp.Width = 100;
            colSubTyp.Name = "colSubTyp";
            colSubTyp.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colSubTyp);

            System.Windows.Forms.DataGridViewTextBoxColumn colDocTyp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDocTyp.HeaderText = "Doc. Type";
            colDocTyp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colDocTyp.Width = 100;
            colDocTyp.Name = "colDocTyp";
            colDocTyp.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colDocTyp);

            System.Windows.Forms.DataGridViewTextBoxColumn colDocNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDocNo.HeaderText = "Doc No.";
            colDocNo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colDocNo.Width = 120;
            colDocNo.Name = "colDocNo";
            colDocNo.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colDocNo);

            System.Windows.Forms.DataGridViewTextBoxColumn colDocDt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDocDt.HeaderText = "Doc. Dt.";
            colDocDt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colDocDt.Width = 100;
            colDocDt.Name = "colDocDt";
            colDocDt.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colDocDt);


            System.Windows.Forms.DataGridViewTextBoxColumn colGSTIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colGSTIN.HeaderText = "GSTIN";
            colGSTIN.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colGSTIN.Width = 120;
            colGSTIN.Name = "colGSTIN";
            colGSTIN.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colGSTIN);

            System.Windows.Forms.DataGridViewTextBoxColumn colLgNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLgNm.HeaderText = "Legal Name of Business";
            colLgNm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colLgNm.Width = 145;
            colLgNm.Name = "colLgNm";
            colLgNm.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colLgNm);

            System.Windows.Forms.DataGridViewTextBoxColumn colConCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colConCity.HeaderText = "Place of Consignor";
            colConCity.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colConCity.Width = 100;
            colConCity.Name = "colConCity";
            colConCity.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colConCity);

            System.Windows.Forms.DataGridViewTextBoxColumn colConState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colConState.HeaderText = "State of Consignor";
            colConState.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colConState.Width = 145;
            colConState.Name = "colConState";
            colConState.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colConState);

            System.Windows.Forms.DataGridViewTextBoxColumn colTranDocNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTranDocNo.HeaderText = "Trans. Doc (LR) No.";
            colTranDocNo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colTranDocNo.Width = 140;
            colTranDocNo.Name = "colTranDocNo";
            colTranDocNo.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colTranDocNo);

            System.Windows.Forms.DataGridViewTextBoxColumn colTranDocDt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTranDocDt.HeaderText = "Doc. Dt.";
            colTranDocDt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colTranDocDt.Width = 100;
            colTranDocDt.Name = "colTranDocDt";
            colTranDocDt.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colTranDocDt);

            System.Windows.Forms.DataGridViewTextBoxColumn colTranMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTranMode.HeaderText = "Trans. Mode";
            colTranMode.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colTranMode.Width = 100;
            colTranMode.Name = "colTranMode";
            colTranMode.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colTranMode);

            System.Windows.Forms.DataGridViewTextBoxColumn colAproxDis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colAproxDis.HeaderText = "Approx Distance";
            colAproxDis.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            colAproxDis.Width = 145;
            colAproxDis.Name = "colAproxDis";
            colAproxDis.ReadOnly = true;
            this.dgvGSTIN.Columns.Add(colAproxDis);

           

            dgvGSTIN.Columns["colSelect"].DataPropertyName = "sel";
            dgvGSTIN.Columns["colSuppTyp"].DataPropertyName = "SuppTyp";
            dgvGSTIN.Columns["colSubTyp"].DataPropertyName = "Sub Type";
            dgvGSTIN.Columns["colDocTyp"].DataPropertyName = "DocTyp";
            dgvGSTIN.Columns["colDocNo"].DataPropertyName = "DocNo";
            dgvGSTIN.Columns["colDocDt"].DataPropertyName = "DocDt";
            dgvGSTIN.Columns["colGSTIN"].DataPropertyName = "GSTIN";
            dgvGSTIN.Columns["colLgNm"].DataPropertyName = "LgNm";
            dgvGSTIN.Columns["colConCity"].DataPropertyName = "ConCity";
            dgvGSTIN.Columns["colConState"].DataPropertyName = "ConState";
            dgvGSTIN.Columns["colTranDocNo"].DataPropertyName = "TranDocNo";
            dgvGSTIN.Columns["colTranDocDt"].DataPropertyName = "TranDocDt";
            dgvGSTIN.Columns["colTranMode"].DataPropertyName = "TranMode";
            dgvGSTIN.Columns["colAproxDis"].DataPropertyName = "AproxDis";
            dgvGSTIN.Columns["colEWAYN"].DataPropertyName = "eWAYN";
            dgvGSTIN.Columns["colEWAYDt"].DataPropertyName = "eWAYDt";
        }

        private void mthControlSet()
        {
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {
                this.btnPartyNm.Image = Image.FromFile(fName);
                this.btnGSTIN.Image = Image.FromFile(fName);
                this.btnTrnName.Image = Image.FromFile(fName);
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

        private void btnGSTIN_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            vSqlStr = "Select GSTIN='' ,'' union Select Distinct GSTIN,Ac_Name From AC_Mast where isnull(GSTIN,'')<>''";
            //vSqlStr = (this.txtGroup.Text.Trim() != "" ? vSqlStr + " and [Group]='" + this.txtGroup.Text.Trim() + "'" : vSqlStr);
            vSqlStr = vSqlStr + " order by GSTIN";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 20);


            DataView dvw = tblPop.DefaultView;// tDs.Tables[0].DefaultView;

            VForText = "Select GSTIN";
            vSearchCol = "GSTIN";
            vDisplayColumnList = "GSTIN:GSTIN,AC_Name:Party Name";
            vReturnCol = "GSTIN";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.Icon = this.pFrmIcon;  //Added by Priyanka B on 02022018 for Bug-31240
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtGSTIN.Text = oSelectPop.pReturnArray[0];
                this.mthView();
            }
        }

        private void btnPartyNm_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            //this.txtGSTIN.Text = "";
            vSqlStr = "Select AC_Name=''  union Select AC_Name=Ac_Name From AC_Mast where 1=1";
            //vSqlStr = (this.txtGroup.Text.Trim() != "" ? vSqlStr + " and [Group]='" + this.txtGroup.Text.Trim() + "'" : vSqlStr);
            vSqlStr = vSqlStr + " order by Ac_Name";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 20);


            DataView dvw = tblPop.DefaultView;// tDs.Tables[0].DefaultView;

            VForText = "Select Party Name";
            //vSearchCol = "GSTIN"; //Commented by Priyanka B on 02022018 for Bug-31240
            vSearchCol = "AC_Name";  //Commented by Priyanka B on 02022018 for Bug-31240
            vDisplayColumnList = "AC_Name:Party Name";
            vReturnCol = "AC_Name";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.Icon = this.pFrmIcon;  //Added by Priyanka B on 02022018 for Bug-31240
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtPartyNm.Text = oSelectPop.pReturnArray[0];
                this.mthView();
            }

        }

        private void btnJson_Click(object sender, EventArgs e)
        {
            mcheckCallingApplication();  //Added by Priyanka B on 02022018 for Bug-31240
            if (this.txtJsonFile.Text == "")
            {
                //MessageBox.Show("Please select the json File Path");  //Commented by Priyanka B on 02022018 for Bug-31240
                //Added by Priyanka B on 02022018 for Bug-31240 Start
                MessageBox.Show("Please select the json File Path", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.btnJsonFile_Click(sender, e);
                this.btnJson_Click(sender, e);
                //Added by Priyanka B on 02022018 for Bug-31240 End
            }
            else
            {
                if (selectedReccnt > 0)  //Added by Priyanka B on 02022018 for Bug-31240
                {
                    string v = "";
                    int vSelectedRec = 0;
                    StringBuilder JsonString = new StringBuilder();

                    JsonString.Append("{ \"billLists\": ");
                    JsonString.Append(" [ ");
                    foreach (DataRow dr in tblGSTIN.Rows)
                    {
                        if ((Boolean)dr["Sel"] == true)
                        {
                            JsonString.Append("\n {");  //Added by Priyanka B on 02022018 for Bug-31240

                            //Commented by Priyanka B on 02022018 for Bug-31240 Start
                            //if (vSelectedRec == 0)
                            //{
                            //    JsonString.Append("{");
                            //}
                            //else
                            //{
                            //    JsonString.Append(", { ");
                            //}
                            //vSelectedRec = vSelectedRec + 1;
                            //this.mthGenJson(dr["Entry_Ty"].ToString(), dr["DocNo"].ToString(), ref JsonString);
                            //Commented by Priyanka B on 02022018 for Bug-31240 End

                            this.mthGenJson(this.txtTrnName.Text.ToString().Trim(), dr["DocNo"].ToString(), ref JsonString);  //Modified by Priyanka B on 02022018 for Bug-31240
                            JsonString.Append(" } ");
                        }
                    }
                    JsonString.Append(" ] }");
                    v = JsonString.ToString();
                    //MessageBox.Show(v);
                    string fName = this.txtJsonFile.Text; //@"D:\UdyogGSTSDK\pdf_report\abc.json";
                    System.IO.File.WriteAllText(fName, v);
                    //Added by Priyanka B on 02022018 for Bug-31240 Start
                    MessageBox.Show("JSON file has been generated at the below location : \n" + this.txtJsonFile.Text.ToString(), this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.mthView();
                    selectedReccnt = 0;
                    //Added by Priyanka B on 02022018 for Bug-31240 End
                }
                //Added by Priyanka B on 02022018 for Bug-31240 Start
                else
                {
                    MessageBox.Show("Please select atleast one row of data to generate JSON file.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                //Added by Priyanka B on 02022018 for Bug-31240 End

            }
        }
        private void mthGenJson(string vEntry_Ty,string vInv_No, ref StringBuilder JsonString)
        {
            DataTable tblMain;
            vSqlStr = "Execute [USP_Ent_eWayBill_JsonDet_Main] '" + vEntry_Ty + "','" + vInv_No.Trim() + "'";
            tblMain = oDataAccess.GetDataTable(vSqlStr, null, 25);
            
            if (tblMain.Rows.Count > 0)
            {              
                
                 //v =v+ DataTableToJsonObj(ref JsonString, tblMain);

                
                for (int j = 0; j < tblMain.Columns.Count; j++)
                {
                    if (j < tblMain.Columns.Count - 1)
                    {
                        JsonString.Append("\"" + tblMain.Columns[j].ColumnName.ToString() + "\":" + "\"" + tblMain.Rows[0][j].ToString().Trim() + "\",");
                    }
                    else if (j == tblMain.Columns.Count - 1)
                    {
                        JsonString.Append("\"" + tblMain.Columns[j].ColumnName.ToString() + "\":" + "\"" + tblMain.Rows[0][j].ToString().Trim() + "\"");
                    }
                }
            }
            
            DataTable tblItem;
            vSqlStr = "Execute [USP_Ent_eWayBill_JsonDet_Item] '" + vEntry_Ty + "','" + vInv_No.Trim() + "'";
            tblItem = oDataAccess.GetDataTable(vSqlStr, null, 25);

            JsonString.Append(",   \"itemList\": ");

            if (tblItem.Rows.Count > 0)
            {
                JsonString.Append(" [ { ");
                for (int j = 0; j < tblItem.Columns.Count; j++)
                {
                    if (tblItem.Columns[j].ColumnName.ToString()=="cessRate")
                    { 
                        //MessageBox.Show("Abc");
                    }
                    if (j < tblItem.Columns.Count - 1)
                    {
                        JsonString.Append("\"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + "\"" + tblItem.Rows[0][j].ToString().Trim() + "\",");
                    }
                    else if (j == tblItem.Columns.Count - 1)
                    {
                        JsonString.Append("\"" + tblItem.Columns[j].ColumnName.ToString() + "\":" + "\"" + tblItem.Rows[0][j].ToString().Trim() + "\"");
                    }
                }
                JsonString.Append(" } ] ");

            }




        }
        public string DataTableToJsonObj(ref StringBuilder JsonString, DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            //StringBuilder JsonString = new StringBuilder();
            //JsonString.Append("{\"billLists\": ");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString().Trim() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString().Trim() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                JsonString.Append("}");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }

        public string DataTableToJsonObj_tbl(ref StringBuilder JsonString, DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            //StringBuilder JsonString = new StringBuilder();
            //JsonString.Append("{\"billLists\": ");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString().Trim() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString().Trim() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                JsonString.Append("}");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }
        public string DataTableToJSON(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //return serializer.Serialize(list);
            return "";
        }

        private void btnJsonFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vFolderBrowserDialog1 = new FolderBrowserDialog();
            vFolderBrowserDialog1.ShowDialog();
            txtJsonFile.Text = @"\ugstEWayBill"+"_"+DateTime.Now.Date.ToString("dd/MM/yy")+"_"+ DateTime.Now.ToString("hh:mm:ss tt") ;
            txtJsonFile.Text = txtJsonFile.Text.Replace(".","").Replace("-","").Replace("/","").Replace(":","").Replace(" ","") + ".Json";
            txtJsonFile.Text = vFolderBrowserDialog1.SelectedPath + txtJsonFile.Text;
        }

        private void btnTrnName_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataTable tblPop;
            //this.txtGSTIN.Text = "";  //Commented by Priyanka B on 02022018 for Bug-31240
            vSqlStr = "Select Code_Nm='Sales' Union Select Code_Nm='Purchase' Union Select Code_Nm='LABOUR JOB ISSUE[IV]' Union Select Code_Nm='LABOUR JOB ISSUE[V]' Union Select Code_Nm='LABOUR JOB RECEIPT[IV]' Union Select Code_Nm='LABOUR JOB RECEIPT[V]' Union Select Code_Nm='CREDIT NOTE' Union Select Code_Nm='Sales Return' Union Select Code_Nm='Purchase' Union Select Code_Nm='Purchase' Union Select Code_Nm='Purchase' Union Select Code_Nm='Purchase'";
            tblPop = oDataAccess.GetDataTable(vSqlStr, null, 20);


            DataView dvw = tblPop.DefaultView;// tDs.Tables[0].DefaultView;

            VForText = "Select Transaction Name";
            vSearchCol = "Code_Nm";
            vDisplayColumnList = "Code_Nm:Transaction Name";
            vReturnCol = "Code_Nm";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.Icon = this.pFrmIcon;  //Added by Priyanka B on 02022018 for Bug-31240
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtTrnName.Text = oSelectPop.pReturnArray[0];
                //this.mthView();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void txtTrnName_TextChanged(object sender, EventArgs e)
        {
            this.mthView();
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {

        }

        //Added by Priyanka B on 02022018 for Bug-31240 Start
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
        private void mInsertProcessIdRecord()
        {

            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "ugstEWB_ItMast_JSON.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = " insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, vTimeOut, true);
        }

        private void btnSelctAll_Click(object sender, EventArgs e)
        {
            //Added by Priyanka B on 02022018 for Bug-31240 Start
            if (dgvGSTIN.Rows.Count > 0)
            {
                if (btnSelctAll.Text == "Select All")
                {
                    for (int i = 0; i < dgvGSTIN.Rows.Count; i++)
                    {
                        dgvGSTIN.Rows[i].Cells["colSelect"].Value = true;
                    }
                    btnSelctAll.Text = "DeSelect All";
                }
                else
                {
                    for (int i = 0; i < dgvGSTIN.Rows.Count; i++)
                    {
                        dgvGSTIN.Rows[i].Cells["colSelect"].Value = false;
                    }
                    btnSelctAll.Text = "Select All";
                }
            }
            //Added by Priyanka B on 02022018 for Bug-31240 End
        }

        private void dgvGSTIN_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Added by Priyanka B on 02022018 for Bug-31240 Start
            if (dgvGSTIN.Rows.Count > 0)
            {
                if (e.ColumnIndex == 0)
                {
                    if (Convert.ToBoolean(dgvGSTIN.Rows[e.RowIndex].Cells["colSelect"].Value.ToString()) == true)
                        selectedReccnt++;
                    else
                        selectedReccnt--;
                }
            }
            //Added by Priyanka B on 02022018 for Bug-31240 End
        }

        private void mDeleteProcessIdRecord()
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
        //Added by Priyanka B on 02022018 for Bug-31240 End
    }
}
