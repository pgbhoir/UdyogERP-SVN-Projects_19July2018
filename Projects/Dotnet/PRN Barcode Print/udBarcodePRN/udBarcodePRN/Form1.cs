using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udBarcodePRN
{
    public partial class Form1 : uBaseForm.FrmBaseForm
    {
        string SqlStr = string.Empty;
        DataAccess_Net.clsDataAccess oDataAccess;
        DataTable dtTranDetail;
        DataTable dtFieldDetail;
        DataTable dtBARCODEDet;
        static public DataTable dtFilterField;
        string queryString;
        string Barcode_Nm, M_T, name, prn, ffld, tblname, behaviour;
        string _stblname;
        String cAppPId, cAppName;
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(string[] args)
        {
            this.pDisableCloseBtn = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            InitializeComponent();
            this.pPApplPID = 0;
            this.pPara = args;
            //this.pFrmCaption = "Barcode Setting";  //Commented by Priyanka B on 29122018 for Bug-32062
            this.pFrmCaption = "Label-Barcode Setting";  //Modified by Priyanka B on 29122018 for Bug-32062
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

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string brname;
            setEnableDisable();
            setTranName();

            SqlStr = "select top 1 Barcode_Nm from BARCODE_SETTING order by Barcode_Nm desc";
            dtBARCODEDet = new DataTable();
            dtBARCODEDet = oDataAccess.GetDataTable(SqlStr, null, 20);
            if (dtBARCODEDet.Rows.Count == 0)
            {
                brname = "";
            }
            else
            {
                brname = dtBARCODEDet.Rows[0][0].ToString();
            }

            BindControl(brname);

            setEnableDisable(false);
            setNavigation();
            this.mInsertProcessIdRecord();
        }

        private void setEnableDisable()
        {
            ddlCategory.SelectedIndex = 0;
        }

        private void BindControl(string brname)
        {
            // setTranName();

            SqlStr = "select top 1 * from BARCODE_SETTING where Barcode_Nm='" + brname + "'";
            dtBARCODEDet = new DataTable();
            dtBARCODEDet = oDataAccess.GetDataTable(SqlStr, null, 20);

            if (dtBARCODEDet.Rows.Count > 0)
            {
                txtBarcodeName.Text = dtBARCODEDet.Rows[0]["Barcode_Nm"].ToString();
                if (dtBARCODEDet.Rows[0]["M_T"].ToString() == "M")
                {
                    ddlCategory.SelectedIndex = 0;
                }
                else
                {
                    ddlCategory.SelectedIndex = 1;
                }
                ddlName.SelectedItem = dtBARCODEDet.Rows[0]["name"].ToString();
                txtPRN.Text = dtBARCODEDet.Rows[0]["prn"].ToString();
                txtFFld.Text = dtBARCODEDet.Rows[0]["ffld"].ToString();
            }
            else
            {
                txtBarcodeName.Text = "";
                ddlCategory.SelectedIndex = 0;
                txtPRN.Text = "";
                txtFFld.Text = "";
                ddlFieldName.Text = "";
                rdbRight.Checked = false;
                rdbLeft.Checked = false;
                txtPosition.Text = "";
                txtLength.Text = "";
            }

        }

        private void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            setTranName();


        }

        private void setTranName()
        {
            bool val;
            DataRow[] result;
            SqlStr = "select M_T='T',code_nm,behaviour=(case when isnull(bcode_nm,'')='' then entry_ty else bcode_nm end),TBL_NM='' from LCODE where isnull(barcode,0)=1";
            SqlStr = SqlStr + "union select M_T='M',name,code,FILENAME from Mastcode where isnull(barcode,0)=1";
            SqlStr = SqlStr + "order by code_nm";
            dtTranDetail = new DataTable();
            dtTranDetail = oDataAccess.GetDataTable(SqlStr, null, 20);

            if (ddlCategory.SelectedIndex == 0)
            {
                result = dtTranDetail.Select("M_T='M'");
                dtTranDetail = result.CopyToDataTable();
                val = false;
                //Commented by Priyanka B on 05032020 for Bug-33346 Start
                //rdbHeader.Checked = false;
                //rdbLineitem.Checked = false;
                //rdbBoth.Checked = false;
                //Commented by Priyanka B on 05032020 for Bug-33346 End

                //Modified by Priyanka B on 05032020 for Bug-33346 Start
                chkHdr.Checked = false;
                chkLineItem.Checked = false;
                chkBCdet.Checked = false;
                //Modified by Priyanka B on 05032020 for Bug-33346 End

            }
            else
            {
                result = dtTranDetail.Select("M_T='T'");
                dtTranDetail = result.CopyToDataTable();
                val = true;
                //rdbHeader.Checked = true;  //Commented by Priyanka B on 05032020 for Bug-33346
                chkHdr.Checked = true;  //Modified by Priyanka B on 05032020 for Bug-33346
            }
            //Commented by Priyanka B on 05032020 for Bug-33346 Start
            //rdbBoth.Enabled = val;
            //rdbHeader.Enabled = val;
            //rdbLineitem.Enabled = val;
            //Commented by Priyanka B on 05032020 for Bug-33346 End

            //Modified by Priyanka B on 05032020 for Bug-33346 Start
            chkHdr.Enabled = val;
            chkLineItem.Enabled = val;
            chkBCdet.Enabled = val;
            //Modified by Priyanka B on 05032020 for Bug-33346 End

            ddlName.Items.Clear();
            if (result.Length > 0)
            {
                foreach (DataRow dr in result)
                {
                    ddlName.Items.Add(dr[1].ToString());
                }
                ddlName.SelectedIndex = 0;
            }

            //ddlName.DataSource = new BindingSource(dtTranDetail, null);
            //ddlName.DisplayMember = "code_nm";
            //ddlName.ValueMember = "code_nm";
        }

        private void setFieldName()
        {
            string hl = "It_Mast";
            string h2 = "";  //Added by Priyanka B on 05032020 for Bug-33346
            DataRow[] result = dtTranDetail.Select("code_nm='" + ddlName.SelectedItem.ToString() + "'");
            string tblname, behaviour;
            if (result.Length > 0)
            {
                behaviour = result[0][2].ToString();
                tblname = result[0][3].ToString();

                this.tblname = tblname;
                this.behaviour = behaviour;
                if (ddlCategory.SelectedIndex != 0)
                {
                    //Commented by Priyanka B on 05032020 for Bug-33346 Start
                    //if (rdbHeader.Checked == true)
                    //{
                    //    tblname = behaviour + "MAIN";
                    //    hl = "Header";
                    //}
                    //else if (rdbLineitem.Checked == true)
                    //{
                    //    tblname = behaviour + "ITEM";
                    //    hl = "LineItem";
                    //}
                    //else if (rdbBoth.Checked == true)
                    //{
                    //    tblname = "Both";
                    //}
                    //Commented by Priyanka B on 05032020 for Bug-33346 End

                    //Modified by Priyanka B on 05032020 for Bug-33346 Start
                    if (chkHdr.Checked == true)
                    {
                        tblname = behaviour + "MAIN";
                        hl = "Header";
                    }
                    if (chkLineItem.Checked == true)
                    {
                        tblname = behaviour + "ITEM";
                        h2 = "LineItem";
                    }
                    if (chkBCdet.Checked == true)
                    {
                        tblname = "Barcodedet";
                    }
                    //Modified by Priyanka B on 05032020 for Bug-33346 End
                }


                //Commented by Priyanka B on 05032020 for Bug-33346 Start
                //if (tblname == "Both")
                //{
                //    SqlStr = "select  c.name,c.column_id,dis=(case when isnull(value,'')='' then  c.name else value end),table1='Header'";
                //    SqlStr = SqlStr + "FROM sys.columns c ";
                //    SqlStr = SqlStr + "INNER JOIN sys.types t ON (c.user_type_id = t.user_type_id)";
                //    SqlStr = SqlStr + "inner join sys.tables o on (o.object_id=c.object_id)";
                //    SqlStr = SqlStr + "left outer join sys.extended_properties s on (s.minor_id=c.column_id and s.major_id=o.object_id)";
                //    SqlStr = SqlStr + "where o.name='" + behaviour + "MAIN'";
                //    SqlStr = SqlStr + "union all";
                //    SqlStr = SqlStr + " select  c.name,c.column_id,dis=(case when isnull(value,'')='' then  c.name else value end),table1='LineItem'";
                //    SqlStr = SqlStr + "FROM sys.columns c ";
                //    SqlStr = SqlStr + "INNER JOIN sys.types t ON (c.user_type_id = t.user_type_id)";
                //    SqlStr = SqlStr + "inner join sys.tables o on (o.object_id=c.object_id)";
                //    SqlStr = SqlStr + "left outer join sys.extended_properties s on (s.minor_id=c.column_id and s.major_id=o.object_id)";
                //    SqlStr = SqlStr + "where o.name='" + behaviour + "ITEM' order by table1,column_id";

                //    // SqlStr = "SELECT name, column_id,table1='Header' FROM sys.columns WHERE object_id = OBJECT_ID('" + behaviour + "MAIN')";
                //    //  SqlStr = SqlStr + " union SELECT name, column_id,table1='LineItem' FROM sys.columns WHERE object_id = OBJECT_ID('" + behaviour + "ITEM')";
                //    dtFieldDetail = new DataTable();
                //    dtFieldDetail = oDataAccess.GetDataTable(SqlStr, null, 20);
                //}
                //else
                //{
                //    SqlStr = "select  c.name,c.column_id,dis=(case when isnull(value,'')='' then  c.name else value end),table1='" + hl + "'";
                //    SqlStr = SqlStr + "FROM sys.columns c ";
                //    SqlStr = SqlStr + "INNER JOIN sys.types t ON (c.user_type_id = t.user_type_id)";
                //    SqlStr = SqlStr + "inner join sys.tables o on (o.object_id=c.object_id)";
                //    SqlStr = SqlStr + "left outer join sys.extended_properties s on (s.minor_id=c.column_id and s.major_id=o.object_id)";
                //    SqlStr = SqlStr + "where o.name='" + tblname + "'  order by table1,column_id";
                //    //SqlStr = "SELECT name, column_id,table1='" + hl + "' FROM sys.columns WHERE object_id = OBJECT_ID('" + tblname + "')";
                //    dtFieldDetail = new DataTable();
                //    dtFieldDetail = oDataAccess.GetDataTable(SqlStr, null, 20);
                //}
                //Commented by Priyanka B on 05032020 for Bug-33346 End

                //Modified by Priyanka B on 05032020 for Bug-33346 Start
                SqlStr = "";
                if (hl == "Header")
                {
                    SqlStr = "select  c.name,c.column_id,dis=(case when isnull(value,'')='' then  c.name else value end),table1='Header'";
                    SqlStr = SqlStr + "FROM sys.columns c ";
                    SqlStr = SqlStr + "INNER JOIN sys.types t ON (c.user_type_id = t.user_type_id)";
                    SqlStr = SqlStr + "inner join sys.tables o on (o.object_id=c.object_id)";
                    SqlStr = SqlStr + "left outer join sys.extended_properties s on (s.minor_id=c.column_id and s.major_id=o.object_id)";
                    SqlStr = SqlStr + "where o.name='" + behaviour + "MAIN'";
                }
                if(h2=="LineItem")
                {
                    if (!string.IsNullOrEmpty(SqlStr))
                    {
                        SqlStr = SqlStr + " Union all ";
                        SqlStr = SqlStr + " select  c.name,c.column_id,dis=(case when isnull(value,'')='' then  c.name else value end),table1='LineItem'";
                        SqlStr = SqlStr + "FROM sys.columns c ";
                        SqlStr = SqlStr + "INNER JOIN sys.types t ON (c.user_type_id = t.user_type_id)";
                        SqlStr = SqlStr + "inner join sys.tables o on (o.object_id=c.object_id)";
                        SqlStr = SqlStr + "left outer join sys.extended_properties s on (s.minor_id=c.column_id and s.major_id=o.object_id)";
                        SqlStr = SqlStr + "where o.name='" + behaviour + "ITEM'";
                    }
                    else
                    {
                        SqlStr = " select  c.name,c.column_id,dis=(case when isnull(value,'')='' then  c.name else value end),table1='LineItem'";
                        SqlStr = SqlStr + "FROM sys.columns c ";
                        SqlStr = SqlStr + "INNER JOIN sys.types t ON (c.user_type_id = t.user_type_id)";
                        SqlStr = SqlStr + "inner join sys.tables o on (o.object_id=c.object_id)";
                        SqlStr = SqlStr + "left outer join sys.extended_properties s on (s.minor_id=c.column_id and s.major_id=o.object_id)";
                        SqlStr = SqlStr + "where o.name='" + behaviour + "ITEM'";
                    }
                }
                if (tblname == "Barcodedet")
                {
                    if (!string.IsNullOrEmpty(SqlStr))
                    {
                        SqlStr = SqlStr + " union all ";
                        SqlStr = SqlStr + " select c.name,c.column_id,dis=(case when isnull(value,'')='' then  c.name else value end),table1='" + tblname + "'";
                        SqlStr = SqlStr + " FROM sys.columns c ";
                        SqlStr = SqlStr + " INNER JOIN sys.types t ON (c.user_type_id = t.user_type_id)";
                        SqlStr = SqlStr + " inner join sys.tables o on (o.object_id=c.object_id)";
                        SqlStr = SqlStr + " left outer join sys.extended_properties s on (s.minor_id=c.column_id and s.major_id=o.object_id)";
                        SqlStr = SqlStr + " where o.name='" + tblname + "'";
                    }
                    else
                    {
                        SqlStr = " select c.name,c.column_id,dis=(case when isnull(value,'')='' then  c.name else value end),table1='" + tblname + "'";
                        SqlStr = SqlStr + " FROM sys.columns c ";
                        SqlStr = SqlStr + " INNER JOIN sys.types t ON (c.user_type_id = t.user_type_id)";
                        SqlStr = SqlStr + " inner join sys.tables o on (o.object_id=c.object_id)";
                        SqlStr = SqlStr + " left outer join sys.extended_properties s on (s.minor_id=c.column_id and s.major_id=o.object_id)";
                        SqlStr = SqlStr + " where o.name='" + tblname + "'";
                    }
                }
                if (!string.IsNullOrEmpty(SqlStr))
                {
                    SqlStr = SqlStr + " order by table1,column_id";
                    dtFieldDetail = new DataTable();
                    dtFieldDetail = oDataAccess.GetDataTable(SqlStr, null, 20);
                }
                //Modified by Priyanka B on 05032020 for Bug-33346 End


                //if (dtFieldDetail.Rows.Count > 0)
                //{
                //    ddlFieldName1.Items.Clear();
                //    foreach (DataRow dr in dtFieldDetail.Rows)
                //    {
                //        ddlFieldName1.Items.Add(dr[0].ToString());
                //    }
                //    ddlFieldName1.SelectedIndex = 0;
                //}
            }



        }

        private void ddlName_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFieldName();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file  
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {
                try
                {
                    string filePath = file.FileName; //get the path of the file  
                    string fileExt = Path.GetExtension(filePath); //get the file extension  
                    if (fileExt.CompareTo(".txt") == 0 || fileExt.CompareTo(".prn") == 0)
                    {
                        string text;
                        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                        {
                            text = streamReader.ReadToEnd();
                        }

                        txtPRN.Text = text;
                    }
                    else
                    {
                        MessageBox.Show("Please choose .txt or .prn  file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (Exception ee)
                {

                }
            }


        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            mDeleteProcessIdRecord();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtPRN.Text = "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void rdbHeader_CheckedChanged(object sender, EventArgs e)
        {
            setFieldName();
        }

        private void rdbLineitem_CheckedChanged(object sender, EventArgs e)
        {
            setFieldName();
        }

        private void rdbBoth_CheckedChanged(object sender, EventArgs e)
        {
            setFieldName();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            int p, l;
            if (txtPosition.Text.ToString() != "" && txtLength.Text.ToString() != "")
            {
                p = Int32.Parse(txtPosition.Text.ToString());
                l = Int32.Parse(txtLength.Text.ToString());
                if (p > l)
                {
                    MessageBox.Show("Position value cannot be less than length value.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtLength.Focus();
                }
            }


            if (ddlFieldName.Text.ToString() == "")
            {
                MessageBox.Show("Field name cannot be empty.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                btnSearch.Focus();
            }
            else if (rdbRight.Checked == false && rdbLeft.Checked == false)
            {
                MessageBox.Show("String to be taken cannot be empty.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else if (txtPosition.Text.ToString() == "")
            {
                MessageBox.Show("Position cannot be empty.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtPosition.Focus();
            }

            else if (txtLength.Text.ToString() == "")
            {
                MessageBox.Show("Length cannot be empty.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtLength.Focus();
            }


            else
            {
                try
                {
                    string PRN = txtPRN.Text;
                    string ss = txtPRN.SelectedText.ToString();
                    //<< field_name >><< L / R >><< SP >><< EP >>
                    string fieldName, LR = "", SP, EP;
                    fieldName = ddlFieldName.Text.ToString().Trim();
                    //  fieldName = ddlFieldName1.SelectedItem.ToString().Trim();
                    if (rdbLeft.Checked == true)
                    {
                        LR = "L";
                    }
                    else if (rdbRight.Checked == true)
                    {
                        LR = "R";
                    }
                    SP = txtPosition.Text.ToString().Trim();
                    EP = txtLength.Text.ToString().Trim();

                    string _tblprfix = "";
                    if (_stblname == "Header")
                    {
                        _tblprfix = "H";
                    }
                    else if (_stblname == "LineItem")
                    {
                        _tblprfix = "L";
                    }
                    else if (_stblname == "It_Mast")
                    {
                        _tblprfix = "I";
                    }
                    queryString = "@@<<" + _tblprfix + "#" + fieldName + ">><<" + LR + ">><<" + SP + ">><<" + EP + ">>@@";

                    string output = PRN.Replace(ss, queryString);
                    txtPRN.Text = "";
                    txtPRN.Text = output;
                }
                catch (Exception ee)
                {

                }
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            dtFilterField = dtFieldDetail;
            FilterForm fu = new FilterForm();
            fu.pPApplPID = this.pPApplPID;
            fu.pPara = this.pPara;
            fu.pFrmCaption = "Filter Condition";
            fu.pCompId = this.pCompId;
            fu.pComDbnm = this.pComDbnm;
            fu.pServerName = this.pServerName;
            fu.pUserId = this.pUserId;
            fu.pPassword = this.pPassword;
            fu.pPApplRange = this.pPApplRange;
            fu.pAppUerName = this.pAppUerName;
            fu.pFrmIcon = this.pFrmIcon;
            fu.pPApplText = this.pPApplText;
            fu.pPApplName = this.pPApplName;
            fu.pPApplPID = this.pPApplPID;
            fu.pPApplCode = this.pPApplCode;
            fu.pEditMode = this.pEditMode;
            fu.pAddMode = this.pAddMode;


            fu.ShowDialog();
            if (FilterForm.s1 != "")
            {
                txtFFld.Text = "";
                txtFFld.Text = FilterForm.s1;
            }



        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            DataTable dtCopy = dtFieldDetail.Copy();

            tDs.Tables.Add(dtCopy);
            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Field";
            vSearchCol = "name ";
            vDisplayColumnList = "name:Field Name,dis:Heading Name,table1:Table";
            vReturnCol = "name,table1";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.Icon = pFrmIcon;
            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();

            if (oSelectPop.pReturnArray != null)
            {
                ddlFieldName.Text = oSelectPop.pReturnArray[0];
                _stblname = oSelectPop.pReturnArray[1];
                //Added by Priyanka B on 05032020 for Bug-33346 Start
                if(_stblname.ToLower()=="barcodedet")
                {
                    _stblname = "LineItem";
                }
                //Added by Priyanka B on 05032020 for Bug-33346 End
            }
        }

        public void setEnableDisable(bool val)
        {
            txtBarcodeName.Enabled = val;
            ddlCategory.Enabled = val;
            ddlName.Enabled = val;
            groupBox1.Enabled = val;
            groupBox2.Enabled = val;
            txtPosition.Enabled = val;
            txtLength.Enabled = val;
            btnApply.Enabled = val;
            txtPRN.Enabled = val;
            btnLoad.Enabled = val;
            button1.Enabled = val;
            button2.Enabled = val;
            btnSearchBarcode.Enabled = true;
            if (val == true && pAddMode == true)
            {
                btnSearchBarcode.Enabled = false;
                txtBarcodeName.Focus();

                txtBarcodeName.Text = "";
                ddlCategory.SelectedIndex = 0;
                txtPRN.Text = "";
                txtFFld.Text = "";
                ddlFieldName.Text = "";
                rdbRight.Checked = false;
                rdbLeft.Checked = false;
                txtPosition.Text = "";
                txtLength.Text = "";

            }
            else if (val == true && pEditMode == true)
            {
                btnSearchBarcode.Enabled = false;
                txtBarcodeName.Enabled = false;
                ddlCategory.Focus();
            }
        }

        private void setNavigation()
        {
            if (this.pAddMode == false && this.pEditMode == false)
            {
                if (txtBarcodeName.Text == "")
                {
                    //btnFirst.Enabled = false;
                    //btnBack.Enabled = false;
                    //btnForward.Enabled = false;
                    //btnLast.Enabled = false;
                    btnNew.Enabled = true;
                    btnSave.Enabled = false;
                    btnEdit.Enabled = false;
                    btnCancel.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {
                    //btnFirst.Enabled = true;
                    //btnBack.Enabled = true;
                    //btnForward.Enabled = true;
                    //btnLast.Enabled = true;
                    btnNew.Enabled = true;
                    btnSave.Enabled = false;
                    btnEdit.Enabled = true;
                    btnCancel.Enabled = false;
                    btnDelete.Enabled = true;
                }
            }
            else if (this.pAddMode == true)
            {
                //btnFirst.Enabled = false;
                //btnBack.Enabled = false;
                //btnForward.Enabled = false;
                //btnLast.Enabled = false;
                btnNew.Enabled = false;
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnCancel.Enabled = true;
                btnDelete.Enabled = false;
            }
            else if (this.pEditMode == true)
            {
                //btnFirst.Enabled = false;
                //btnBack.Enabled = false;
                //btnForward.Enabled = false;
                //btnLast.Enabled = false;
                btnNew.Enabled = false;
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnCancel.Enabled = true;
                btnDelete.Enabled = false;
            }
        }

        private void setval()
        {
            Barcode_Nm = txtBarcodeName.Text.ToString().Trim().Replace("'", "''");
            M_T = ddlCategory.SelectedItem.ToString().Trim().Replace("'", "''");
            if (M_T == "Transaction")
            {
                M_T = "T";
            }
            else
            {
                M_T = "M";
            }
            name = ddlName.SelectedItem.ToString().Trim().Replace("'", "''");
            prn = txtPRN.Text.ToString().Trim().Replace("'", "''");
            ffld = txtFFld.Text.ToString().Trim().Replace("'", "''");


        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            this.pAddMode = true;
            setEnableDisable(true);
            setNavigation();
        }

        private void txtPosition_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if (!char.IsDigit(c) && c != '\b')
            {
                e.Handled = true;
            }
            else if (c == '0' && txtPosition.Text == "")
            {
                e.Handled = true;
            }
            else if (char.IsDigit(c))
            {
                if (Int32.Parse(c.ToString()) < 0)
                {
                    e.Handled = true;
                }
            }


        }

        private void txtLength_KeyPress(object sender, KeyPressEventArgs e)
        {

            char c = e.KeyChar;

            if (!char.IsDigit(c) && c != '\b')
            {
                e.Handled = true;
            }
            else if (c == '0' && txtLength.Text == "")
            {
                e.Handled = true;
            }
            else if (char.IsDigit(c))
            {
                if (Int32.Parse(c.ToString()) < 0)
                {
                    e.Handled = true;
                }
            }



        }

        private void txtLength_Leave(object sender, EventArgs e)
        {

        }

        private void txtPosition_Enter(object sender, EventArgs e)
        {
            if (txtPosition.Text == "")
            {
                txtLength.Text = "";
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnNew.Enabled)
                btnNew_Click(this.btnNew, e);
        }

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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnDelete.Enabled)
                btnDelete_Click(this.btnDelete, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnLogout, e);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.pEditMode = true;
            this.pAddMode = false;
            setEnableDisable(true);
            setNavigation();
        }

        //Added by Priyanka B on 05032020 for Bug-33346 Start
        private void chkHdr_CheckedChanged(object sender, EventArgs e)
        {
            setFieldName();
        }

        private void chkLineItem_CheckedChanged(object sender, EventArgs e)
        {
            setFieldName();
        }

        private void chkBCdet_CheckedChanged(object sender, EventArgs e)
        {
            setFieldName();
        }
        //Added by Priyanka B on 05032020 for Bug-33346 End

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
              MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    oDataAccess.BeginTransaction();
                    SqlStr = "Delete from BARCODE_SETTING where Barcode_Nm='" + txtBarcodeName.Text.ToString().Trim().Replace("'", "''") + "'";
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                    oDataAccess.CommitTransaction();

                    string brname;
                    this.pAddMode = false;
                    this.pEditMode = false;
                    setEnableDisable();
                    setTranName();
                    SqlStr = "select top 1 Barcode_Nm from BARCODE_SETTING order by Barcode_Nm desc";
                    dtBARCODEDet = new DataTable();
                    dtBARCODEDet = oDataAccess.GetDataTable(SqlStr, null, 20);
                    if (dtBARCODEDet.Rows.Count == 0)
                    {
                        brname = "";
                    }
                    else
                    {
                        brname = dtBARCODEDet.Rows[0][0].ToString();
                    }

                    BindControl(brname);

                    setEnableDisable(false);
                    setNavigation();
                }
                catch (Exception ee)
                {
                    //  MessageBox.Show(" Cannot Delete !!!Transaction has generated against this Account.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string brname;
            this.pAddMode = false;
            this.pEditMode = false;
            setEnableDisable();
            setTranName();
            SqlStr = "select top 1 Barcode_Nm from BARCODE_SETTING order by Barcode_Nm desc";
            dtBARCODEDet = new DataTable();
            dtBARCODEDet = oDataAccess.GetDataTable(SqlStr, null, 20);
            if (dtBARCODEDet.Rows.Count == 0)
            {
                brname = "";
            }
            else
            {
                brname = dtBARCODEDet.Rows[0][0].ToString();
            }

            BindControl(brname);

            setEnableDisable(false);
            setNavigation();
        }

        private void btnSearchBarcode_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select Barcode_Nm as 'BarcodeName',name as 'Name' from BARCODE_SETTING order by Barcode_Nm desc";

            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select Barcode Name";
            vSearchCol = "BarcodeName";
            vDisplayColumnList = "BarcodeName:Barcode Name,Name:Name";
            vReturnCol = "BarcodeName,Name";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.Icon = pFrmIcon;
            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();

            if (oSelectPop.pReturnArray != null)
            {
                txtBarcodeName.Text = oSelectPop.pReturnArray[0];

            }

            pAddMode = false;
            pAddMode = false;

            BindControl(txtBarcodeName.Text.ToString().Trim().Replace("'", "''"));

            setEnableDisable(false);
            setNavigation();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlStr = "select  Barcode_Nm from BARCODE_SETTING where Barcode_Nm='" + txtBarcodeName.Text.ToString().Trim().Replace("'", "''")+"'"; 
            DataTable dttemp = new DataTable();
            dttemp = oDataAccess.GetDataTable(SqlStr, null, 20);

            if (txtBarcodeName.Text.ToString() == "")
            {
                MessageBox.Show("Barcode Name cannot be empty.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtBarcodeName.Focus();

            }
            else if ((dttemp.Rows.Count>0) && this.pAddMode == true)
            {
                MessageBox.Show("Barcode Name already exist.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtBarcodeName.Focus();
            }
            else
            {
                setval();
                InsertUpdate();
                this.pAddMode = false;
                this.pEditMode = false;

                this.pAddMode = false;
                this.pEditMode = false;
                setEnableDisable();
                //setTranName();
                SqlStr = "select top 1 Barcode_Nm from BARCODE_SETTING order by Barcode_Nm desc";
                dtBARCODEDet = new DataTable();
                dtBARCODEDet = oDataAccess.GetDataTable(SqlStr, null, 20);
                string brname = dtBARCODEDet.Rows[0][0].ToString();
                BindControl(brname);

                setEnableDisable(false);
                setNavigation();
            }



        }
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udBarcodePRN.exe";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
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
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }
        private void InsertUpdate()
        {
            try
            {
                if (this.pAddMode == true)
                {
                    oDataAccess.BeginTransaction();
                    SqlStr = "set dateformat dmy INSERT INTO BARCODE_SETTING(Barcode_Nm,M_T,name,prn,ffld,tblname,behaviour)";
                    SqlStr = SqlStr + " VALUES('" + Barcode_Nm + "','" + M_T + "','" + name + "','" + prn + "','" + ffld + "','" + tblname + "','" + behaviour + "')";
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                    oDataAccess.CommitTransaction();

                }
                else if (this.pEditMode == true)
                {
                    oDataAccess.BeginTransaction();
                    SqlStr = "set dateformat dmy update BARCODE_SETTING set ";
                    SqlStr = SqlStr + "M_T='" + M_T + "',name='" + name + "',prn='" + prn + "',ffld='" + ffld + "',tblname='" + tblname + "',behaviour='" + behaviour + "' where Barcode_Nm='" + Barcode_Nm + "'";
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                    oDataAccess.CommitTransaction();
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.ToString());
            }
        }

    }
}
