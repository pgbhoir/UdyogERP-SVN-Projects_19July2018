using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace udBankMaster
{
    public partial class Form1 : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
        string SqlStr, ac_name, acname;
        string bankname, mailname, group, typ, contact, designation, add1, add2, add3, Area, Zone, CITY, District, state, stateCode, zip, country, office, fax, phoner, cellno, email, branch, ACTYPE, ACNO, mcrno, ifsc, bsrcode,Salesman;
        int City_id, acgroupId, State_id, Country_id, comp_id, count = 0, ac_type_id, ccount = 0, ac_id;
        decimal cr_days, cr_limit;
        bool overlimit;
        String cAppPId, cAppName;
        DataTable tblAddInfo = new DataTable();//Added By Rupesh G.on 08/08/2019 for EXIM
        DataTable dtBankRec;//Added By Rupesh G.on 08/08/2019 for EXIM
        public Form1(string[] args)
        {
            this.pDisableCloseBtn = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            InitializeComponent();

            this.pPApplPID = 0;
            this.pPara = args;
            this.pFrmCaption = "Bank Master";
            this.pCompId = Convert.ToInt16(args[0]);
            comp_id = this.pCompId;
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
            this.pAddMode = false;
            this.pEditMode = false;
            getAccountName();
            bindControls(ac_name);
            setEnableDisable(false);
            setNavigation();
            this.mInsertProcessIdRecord();
            this.btnLast_Click(sender, e);      //Divyang
        }

        private void txtBankName_Leave(object sender, EventArgs e)
        {
            if (txtBankName.Text.ToString().Trim() != "")
            {
                txtMName.Text = txtBankName.Text;
            }
            //if (ccount == 0)
            //{
            //    SqlStr = "select * from ac_mast where LOWER(ac_name)='" + txtBankName.Text + "'";
            //    DataTable dt = new DataTable();
            //    dt = oDataAccess.GetDataTable(SqlStr, null, 20);
            //    if (txtBankName.Text.ToString() == "")
            //    {
            //        MessageBox.Show("Bank Name cannot be empty.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        txtBankName.Focus();
            //        return;
            //    }
            //    else if (dt.Rows.Count > 0)
            //    {
            //        MessageBox.Show("Bank Name already exists.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        txtBankName.Focus();
            //        return;
            //    }
            //    else
            //    {
            //        txtMName.Text = txtBankName.Text.ToString();
            //        txtMName.Focus();
            //        txtMName.SelectionStart = txtMName.Text.Length;
            //    }

            //}

        }

        private void txtMName_Leave(object sender, EventArgs e)
        {

            if (txtBankName.Text.ToString()!="" &&   txtMName.Text.ToString() == "")
            {
                MessageBox.Show("Mailing Name cannot be empty.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtMName.Text = txtBankName.Text;
                txtMName.Focus();
            }

        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (txtEmail.Text.Trim() != "")
            {
                Regex reg = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                if (!reg.IsMatch(txtEmail.Text))
                {
                    MessageBox.Show("Email id is not Proper.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtEmail.Focus();
                }
            }
        }

        private void txtMainGroup_Leave(object sender, EventArgs e)
        {

        }

        private void txtOffice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;
        }

        private void txtCreditDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;
        }

        private void txtCreditLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '.')
                e.Handled = true;
        }

        private void txtCreditLimit_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void btnInterestRate_Click(object sender, EventArgs e)
        {

            DataTable dtRateDet = new DataTable();
            InterestRateDetails oFrmIntDet = new InterestRateDetails();
            oFrmIntDet.pTblMain = dtRateDet;
            oFrmIntDet.pPApplPID = this.pPApplPID;
            oFrmIntDet.pPara = this.pPara;
            oFrmIntDet.pFrmCaption = "InterestRate Details";
            oFrmIntDet.pCompId = this.pCompId;
            oFrmIntDet.pComDbnm = this.pComDbnm;
            oFrmIntDet.pServerName = this.pServerName;
            oFrmIntDet.pUserId = this.pUserId;
            oFrmIntDet.pPassword = this.pPassword;
            oFrmIntDet.pPApplRange = this.pPApplRange;
            oFrmIntDet.pAppUerName = this.pAppUerName;
            oFrmIntDet.pFrmIcon = this.pFrmIcon;
            oFrmIntDet.pPApplText = this.pPApplText;
            oFrmIntDet.pPApplName = this.pPApplName;
            oFrmIntDet.pPApplPID = this.pPApplPID;
            oFrmIntDet.pPApplCode = this.pPApplCode;
            oFrmIntDet.Pac_id = ac_id;
            oFrmIntDet.pEditMode = this.pEditMode;
            oFrmIntDet.pAddMode = this.pAddMode;

            oFrmIntDet.Show();



        }

        private void txtArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F2")
            {
                string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
                DataSet tDs = new DataSet();
                SqlStr = "Select Distinct Area From Ac_Mast Where Area <> ''";

                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
                DataView dvw = tDs.Tables[0].DefaultView;
                VForText = "Select Area";
                vSearchCol = "Area";
                vDisplayColumnList = "Area:Area";
                vReturnCol = "Area";
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
                    txtArea.Text = oSelectPop.pReturnArray[0];
                }
            }
        }

        private void txtCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F2")
            {
                string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
                DataSet tDs = new DataSet();
                SqlStr = "Select distinct city from city where city is not null AND City <> '' Order By City";

                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
                DataView dvw = tDs.Tables[0].DefaultView;
                VForText = "Select city";
                vSearchCol = "city";
                vDisplayColumnList = "city:City";
                vReturnCol = "city";
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
                    txtCity.Text = oSelectPop.pReturnArray[0];
                }
            }
        }

        private void txtState_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F2")
            {
            }
        }

        private void BtnAddInfo_Click(object sender, EventArgs e)
        {
            if (tblAddInfo.Rows.Count == 0)
            {
                SqlStr = "Select Head_Nm,Fld_Nm,Data_Ty,Fld_Wid=cast(Fld_Wid as int),fld_Dec=cast(fld_Dec as int),FiltCond From Lother Where e_Code='EB' Order By Serial";
                tblAddInfo = new DataTable();
                tblAddInfo = oDataAccess.GetDataTable(SqlStr, null, 20);
            }
            if (dtBankRec.Rows.Count == 0)
            {
                DataRow dr = dtBankRec.NewRow();
                dtBankRec.Rows.Add(dr);

            }


            udAddInfo.frmAddInfo oFrmAddInfo = new udAddInfo.frmAddInfo();
            oFrmAddInfo.pTblAddInfo = tblAddInfo;
            oFrmAddInfo.pTblMain = dtBankRec;
            oFrmAddInfo.pParentForm = this;
            oFrmAddInfo.pTblMainNm = "ac_mast";
            oFrmAddInfo.ShowDialog();
        }

        private void txtCountry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F2")
            {
                string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
                DataSet tDs = new DataSet();
                SqlStr = "Select Country from Country where Country is not null and Country <> '' order by Country";

                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
                DataView dvw = tDs.Tables[0].DefaultView;
                VForText = "Select Country";
                vSearchCol = "Country";
                vDisplayColumnList = "Country:Country";
                vReturnCol = "Country";
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
                    txtCountry.Text = oSelectPop.pReturnArray[0];

                }
            }
        }

        private void txtBankName_KeyPress(object sender, KeyPressEventArgs e)
       {

        }

        private void txtResi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;
        }

        private void txtCell_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void btnMainGroup_Leave(object sender, EventArgs e)
        {
           
            //    if (txtMainGroup.Text.ToString() == "")
            //    {
            //        MessageBox.Show("Main Group cannot be empty.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);

            //        count++;

            //        btnMainGroup.Focus();
            //        return;
            //    }
           
        }

        private void getAccountName()
        {
            SqlStr = "select top 1 ac_name,ac_id from ac_mast where typ='Bank' order by ac_id desc";
            DataTable dtACName = new DataTable();
            dtACName = oDataAccess.GetDataTable(SqlStr, null, 20);
            if (dtACName.Rows.Count > 0)
            {
                ac_name = dtACName.Rows[0][0].ToString();
                ac_id = Int32.Parse(dtACName.Rows[0][1].ToString());
            }

        }

        private void bindControls(string accountname)
        {
            SqlStr = "select ac_name,mailname,[group],typ,contact,designatio,add1,add2,add3,Area,Zone,CITY,City_id,District,[state],";
            SqlStr = SqlStr + "State_id,stateCode,zip,country,Country_id,phone as office,fax,phoner,mobile as cellno,email,";
            SqlStr = SqlStr + "bankbr as branch,U_BKACTYPE as ACTYPE,bankno as ACNO,U_MICRNO as mcrno ,U_IFSCCODE as ifsc,bsrcode,cr_days,cramount,crallow,ac_id";

            //Add by Rupesh G. on 07/08/2019 for Exim Module-Start
            string sqlStr1 = "Select fld_Nm From LOTHER where e_code='EB'";
            DataTable dtRec = oDataAccess.GetDataTable(sqlStr1, null, 20);
            if (dtRec.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRec.Rows)
                {
                    SqlStr = SqlStr + "," + dr[0].ToString().Trim();
                }

            }
            //Add by Rupesh G. on 07/08/2019 for Exim Module-End

            SqlStr = SqlStr + " from ac_mast where typ='Bank' and ac_name ='" + accountname + "'";

            dtBankRec = new DataTable();
            dtBankRec = oDataAccess.GetDataTable(SqlStr, null, 20);

            if (dtBankRec.Rows.Count > 0)
            {
                txtBankName.Text = dtBankRec.Rows[0]["ac_name"].ToString();
                txtMName.Text = dtBankRec.Rows[0]["mailname"].ToString();
                txtMainGroup.Text = dtBankRec.Rows[0]["group"].ToString();
                txtBranchName.Text = dtBankRec.Rows[0]["branch"].ToString();
                txtACType.Text = dtBankRec.Rows[0]["ACTYPE"].ToString();
                txtACNo.Text = dtBankRec.Rows[0]["ACNO"].ToString();
                txtMICR.Text = dtBankRec.Rows[0]["mcrno"].ToString();
                txtIFSC.Text = dtBankRec.Rows[0]["ifsc"].ToString();
                txtBSR.Text = dtBankRec.Rows[0]["bsrcode"].ToString();
                txtContactPerson.Text = dtBankRec.Rows[0]["contact"].ToString();
                txtDesignation.Text = dtBankRec.Rows[0]["designatio"].ToString();
                txtAdd1.Text = dtBankRec.Rows[0]["add1"].ToString();
                txtAdd2.Text = dtBankRec.Rows[0]["add2"].ToString();
                txtAdd3.Text = dtBankRec.Rows[0]["add3"].ToString();
                txtArea.Text = dtBankRec.Rows[0]["Area"].ToString();
                txtZone.Text = dtBankRec.Rows[0]["Zone"].ToString();
                txtCity.Text = dtBankRec.Rows[0]["CITY"].ToString();
                txtDist.Text = dtBankRec.Rows[0]["District"].ToString();
                txtState.Text = dtBankRec.Rows[0]["state"].ToString();
                txtZip.Text = dtBankRec.Rows[0]["zip"].ToString();
                txtStateCode.Text = dtBankRec.Rows[0]["stateCode"].ToString();
                txtCountry.Text = dtBankRec.Rows[0]["country"].ToString();
                txtOffice.Text = dtBankRec.Rows[0]["office"].ToString();
                txtFax.Text = dtBankRec.Rows[0]["fax"].ToString();
                txtResi.Text = dtBankRec.Rows[0]["phoner"].ToString();
                txtCell.Text = dtBankRec.Rows[0]["cellno"].ToString();
                txtEmail.Text = dtBankRec.Rows[0]["email"].ToString();
                txtCreditDay.Text = dtBankRec.Rows[0]["cr_days"].ToString();
                txtCreditLimit.Text = dtBankRec.Rows[0]["cramount"].ToString();
                chkOverLimit.Checked = (Boolean)dtBankRec.Rows[0]["crallow"];
                ac_id = Int32.Parse(dtBankRec.Rows[0]["ac_id"].ToString());
            }
            else
            {
                txtBankName.Text = "";
                txtMName.Text = "";
                txtMainGroup.Text = "";
                btnMainGroup.Text = "";
                //  txtType.Text = "";

                txtBranchName.Text = "";
                txtACType.Text = "";
                txtACNo.Text = "";
                txtMICR.Text = "";
                txtIFSC.Text = "";
                txtBSR.Text = "";

                txtContactPerson.Text = "";
                txtDesignation.Text = "";
                txtAdd1.Text = "";
                txtAdd2.Text = "";
                txtAdd3.Text = "";
                txtArea.Text = "";
                btnArea.Text = "";
                txtZone.Text = "";
                txtCity.Text = "";
                btnCity.Text = "";
                txtDist.Text = "";
                txtState.Text = "";
                btnState.Text = "";
                txtZip.Text = "";
                txtStateCode.Text = "";
                txtCountry.Text = "";
                txtOffice.Text = "";
                txtFax.Text = "";
                txtResi.Text = "";
                txtCell.Text = "";
                txtEmail.Text = "";

                txtCreditDay.Text = "";
                txtCreditLimit.Text = "";
                chkOverLimit.Checked = false;

            }
        }

        private void setEnableDisable(bool val)
        {
            txtBankName.Enabled = val;
            txtMName.Enabled = val;
            btnMainGroup.Enabled = val;
            txtBranchName.Enabled = val;
            txtACType.Enabled = val;
            txtACNo.Enabled = val;
            txtMICR.Enabled = val;
            txtIFSC.Enabled = val;
            txtBSR.Enabled = val;
            txtContactPerson.Enabled = val;
            txtDesignation.Enabled = val;
            txtAdd1.Enabled = val;
            txtAdd2.Enabled = val;
            txtAdd3.Enabled = val;
            txtArea.Enabled = val;
            btnArea.Enabled = val;
            txtZone.Enabled = val;
            txtCity.Enabled = val;
            btnCity.Enabled = val;
            txtDist.Enabled = val;
            btnState.Enabled = val;
            txtZip.Enabled = val;
            btnCountry.Enabled = val;
            txtOffice.Enabled = val;
            txtFax.Enabled = val;
            txtResi.Enabled = val;
            txtCell.Enabled = val;
            txtEmail.Enabled = val;
            txtCreditDay.Enabled = val;
            txtCreditLimit.Enabled = val;
            chkOverLimit.Enabled = val;

            if (val == true && pAddMode == true)
            {
                txtBankName.Text = "";
                txtMName.Text = "";
                txtMainGroup.Text = "";
                btnMainGroup.Text = "";
                //  txtType.Text = "";

                txtBranchName.Text = "";
                txtACType.Text = "";
                txtACNo.Text = "";
                txtMICR.Text = "";
                txtIFSC.Text = "";
                txtBSR.Text = "";

                txtContactPerson.Text = "";
                txtDesignation.Text = "";
                txtAdd1.Text = "";
                txtAdd2.Text = "";
                txtAdd3.Text = "";
                txtArea.Text = "";
                btnArea.Text = "";
                txtZone.Text = "";
                txtCity.Text = "";
                btnCity.Text = "";
                txtDist.Text = "";
                txtState.Text = "";
                btnState.Text = "";
                txtZip.Text = "";
                txtStateCode.Text = "";
                txtCountry.Text = "";
                txtOffice.Text = "";
                txtFax.Text = "";
                txtResi.Text = "";
                txtCell.Text = "";
                txtEmail.Text = "";

                txtCreditDay.Text = "";
                txtCreditLimit.Text = "";
                chkOverLimit.Checked = false;
            }


        }

        private void setNavigation()
        {
            if (this.pAddMode == false && this.pEditMode == false)
            {
                if (txtBankName.Text == "")
                {
                    btnFirst.Enabled = false;
                    btnBack.Enabled = false;
                    btnForward.Enabled = false;
                    btnLast.Enabled = false;
                    btnNew.Enabled = true;
                    btnSave.Enabled = false;
                    btnEdit.Enabled = false;
                    btnCancel.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {
                    btnFirst.Enabled = true;
                    btnBack.Enabled = true;
                    btnForward.Enabled = true;
                    btnLast.Enabled = true;
                    btnNew.Enabled = true;
                    btnSave.Enabled = false;
                    btnEdit.Enabled = true;
                    btnCancel.Enabled = false;
                    btnDelete.Enabled = true;
                }
            }
            else if (this.pAddMode == true)
            {
                btnFirst.Enabled = false;
                btnBack.Enabled = false;
                btnForward.Enabled = false;
                btnLast.Enabled = false;
                btnNew.Enabled = false;
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnCancel.Enabled = true;
                btnDelete.Enabled = false;
            }
            else if (this.pEditMode == true)
            {
                btnFirst.Enabled = false;
                btnBack.Enabled = false;
                btnForward.Enabled = false;
                btnLast.Enabled = false;
                btnNew.Enabled = false;
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnCancel.Enabled = true;
                btnDelete.Enabled = false;
            }
        }

        public int getAcoountGroupDetails(string group)
        {
            SqlStr = "select Ac_group_id from ac_group_mast where ac_group_name = '" + group + "'";
            DataTable dtGroup = new DataTable();
            dtGroup = oDataAccess.GetDataTable(SqlStr, null, 20);
            if (dtGroup.Rows.Count > 0)
            {
                return Int32.Parse(dtGroup.Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public int getCityDetails(string city)
        {
            SqlStr = "Select City_id from city where city='" + city + "'";
            DataTable dtCity = new DataTable();
            dtCity = oDataAccess.GetDataTable(SqlStr, null, 20);
            if (dtCity.Rows.Count > 0)
            {
                return Int32.Parse(dtCity.Rows[0][0].ToString());
            }
            else
            {
                SqlStr = "INSERT INTO city (city)VALUES('" + city + "')";
                oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                return getCityDetails(city);
            }
        }

        public int getStateDetails(string stateCode)
        {
            SqlStr = "Select state_id from state where Gst_State_Code='" + stateCode + "'";
            DataTable dtState = new DataTable();
            dtState = oDataAccess.GetDataTable(SqlStr, null, 20);
            if (dtState.Rows.Count > 0)
            {
                return Int32.Parse(dtState.Rows[0][0].ToString());
            }
            else
            {
                return 37;
            }
        }

        public int getCountryDetails(string country)
        {
            SqlStr = "Select country_id from Country where Country='" + country + "'";
            DataTable dtCountry = new DataTable();
            dtCountry = oDataAccess.GetDataTable(SqlStr, null, 20);
            if (dtCountry.Rows.Count > 0)
            {
                return Int32.Parse(dtCountry.Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public int getAcTypeId()
        {
            SqlStr = "select ac_type_id from ac_type_mast where [typ]='BANK      '";
            DataTable dtCountry = new DataTable();
            dtCountry = oDataAccess.GetDataTable(SqlStr, null, 20);

            if (dtCountry.Rows.Count > 0)
            {
                return Int32.Parse(dtCountry.Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public void setValue()
        {
            bankname = txtBankName.Text.ToString().Trim().Replace("'", "''");
            mailname = txtMName.Text.ToString().Trim().Replace("'", "''");
            group = txtMainGroup.Text.ToString().Trim().Replace("'", "''");
            acgroupId = getAcoountGroupDetails(group);
            typ = txtType.Text.ToString().Trim();
            contact = txtContactPerson.Text.ToString().Trim();
            designation = txtDesignation.Text.ToString().Trim();
            add1 = txtAdd1.Text.ToString().Trim().Replace("'", "''");
            add2 = txtAdd2.Text.ToString().Trim().Replace("'", "''");
            add3 = txtAdd3.Text.ToString().Trim().Replace("'", "''");
            Area = txtArea.Text.ToString().Trim();
            Zone = txtZone.Text.ToString().Trim();
            CITY = txtCity.Text.ToString().Trim();
            City_id = getCityDetails(CITY);
            District = txtDist.Text.ToString().Trim();
            state = txtState.Text.ToString().Trim();
            stateCode = txtStateCode.Text.ToString().Trim();
            State_id = getStateDetails(stateCode);
            zip = txtZip.Text.ToString().Trim();
            country = txtCountry.Text.ToString().Trim();
            Country_id = getCountryDetails(country);
            office = txtOffice.Text.ToString().Trim();
            fax = txtFax.Text.ToString().Trim();
            phoner = txtResi.Text.ToString().Trim();
            cellno = txtCell.Text.ToString().Trim();
            email = txtEmail.Text.ToString().Trim();
            branch = txtBranchName.Text.ToString().Trim();
            ACTYPE = txtACType.Text.ToString().Trim();
            ACNO = txtACNo.Text.ToString().Trim();
            mcrno = txtMICR.Text.ToString().Trim();
            ifsc = txtIFSC.Text.ToString().Trim();
            bsrcode = txtBSR.Text.ToString().Trim();
            ac_type_id = getAcTypeId();
            if (txtCreditDay.Text == "")
            {
                txtCreditDay.Text = "0";
            }
            if (txtCreditLimit.Text == "")
            {
                txtCreditLimit.Text = "0.00";
            }
            cr_days = Math.Round(decimal.Parse(txtCreditDay.Text), 0);
            cr_limit = Math.Round(decimal.Parse(decimal.Parse(txtCreditLimit.Text).ToString("F")), 2);
            overlimit = chkOverLimit.Checked;
        }

        public void InsertUpdateRec()
        {
            try
            {
                if (this.pAddMode == true)
                {
                    oDataAccess.BeginTransaction();
                    SqlStr = " INSERT INTO Ac_mast(ac_name, mailname,[group], Ac_group_id, typ, contact, designatio, add1, add2, add3";
                    SqlStr = SqlStr + ", Area, Zone, city, City_id, District,[state], StateCode, State_id, zip, country, Country_id, phone, fax, phoner, mobile, email";
                    SqlStr = SqlStr + ", bankbr, U_BKACTYPE, bankno, U_MICRNO, U_IFSCCODE, bsrcode,[type],compid,ac_type_id,cr_days,cramount,crallow,ldeactive,salesman)";
                    SqlStr = SqlStr + " VALUES('" + bankname + "', '" + mailname + "', '" + group + "', " + acgroupId + ", '" + typ + "', '" + contact + "', '" + designation + "' ";
                    SqlStr = SqlStr + ", '" + add1 + "', '" + add2 + "', '" + add3 + "', '" + Area + "', '" + Zone + "', '" + CITY + "', " + City_id + ", '" + District + "', '" + state + "' ";
                    SqlStr = SqlStr + ", '" + stateCode + "', " + State_id + ", '" + zip + "', '" + country + "', " + Country_id + ", '" + office + "', '" + fax + "', '" + phoner + "', '" + cellno + "', '" + email + "'";
                    SqlStr = SqlStr + ", '" + branch + "', '" + ACTYPE + "', '" + ACNO + "', '" + mcrno + "', '" + ifsc + "', '" + bsrcode + "','B'," + comp_id + "," + ac_type_id + "," + cr_days + "," + cr_limit + ",'" + overlimit + "',0,'')";
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                    oDataAccess.CommitTransaction();

                }
                else if (this.pEditMode == true)
                {
                    oDataAccess.BeginTransaction();
                    SqlStr = "update Ac_mast set ";
                    SqlStr = SqlStr + "ac_name = '" + bankname + "', mailname = '" + mailname + "',[group] = '" + group + "', Ac_group_id = " + acgroupId + ",";
                    SqlStr = SqlStr + "typ = '" + typ + "', contact = '" + contact + "', designatio = '" + designation + "', add1 = '" + add1 + "', add2 = '" + add2 + "', add3 = '" + add3 + "',";
                    SqlStr = SqlStr + "Area = '" + Area + "', Zone = '" + Zone + "', city = '" + CITY + "', City_id = " + City_id + ", District = '" + District + "',[state] = '" + state + "',";
                    SqlStr = SqlStr + "StateCode = '" + stateCode + "', State_id = " + State_id + ", zip = '" + zip + "', country = '" + country + "', Country_id = " + Country_id + ",";
                    SqlStr = SqlStr + "phone = '" + office + "', fax = '" + fax + "', phoner = '" + phoner + "', mobile = '" + cellno + "', email = '" + email + "', bankbr = '" + branch + "',";
                    SqlStr = SqlStr + "U_BKACTYPE = '" + ACTYPE + "', bankno = '" + ACNO + "', U_MICRNO = '" + mcrno + "', U_IFSCCODE = '" + ifsc + "', bsrcode = '" + bsrcode + "',compid=" + comp_id + ",ac_type_id=" + ac_type_id + ",cr_days=" + cr_days + ",cramount=" + cr_limit + ",crallow='" + overlimit + "'";
                    SqlStr = SqlStr + "where ac_name = '" + bankname + "'";
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                    oDataAccess.CommitTransaction();
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.pAddMode = true;
            this.pEditMode = false;
            btnBankName.Visible = false;
            this.txtBankName.Size = new System.Drawing.Size(392, 21);
            setEnableDisable(true);
            setNavigation();
            txtBankName.Focus();
            ccount = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            SqlStr = "select * from ac_mast where LOWER(ac_name)='" + txtBankName.Text + "'";
            DataTable dt = new DataTable();
            dt = oDataAccess.GetDataTable(SqlStr, null, 20);
            if (txtBankName.Text.ToString() == "")
            {
                MessageBox.Show("Bank Name cannot be empty.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtBankName.Focus();
                return;
            }
            else if (dt.Rows.Count > 0 && this.pAddMode == true)
            {
                MessageBox.Show("Bank Name already exists.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtBankName.Focus();
                return;
            }
            else
            {
                txtMName.Text = txtBankName.Text.ToString();
                txtMName.Focus();
                txtMName.SelectionStart = txtMName.Text.Length;
            }

            if (txtMainGroup.Text.ToString() == "")
            {
                MessageBox.Show("Main Group cannot be empty.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                btnMainGroup.Focus();
                return;
            }


            string bsrcode = txtBSR.Text.ToString();
            if(bsrcode.Length!=7 && txtBSR.Text.Trim()!="")
            {
                MessageBox.Show("Enter valid 7 digit BSR Code", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtBSR.Focus();
            }
            else
            {
                setValue();
               
                //InsertUpdateRec();//Commented By Rupesh G. on 08/08/2019 for EXIM
                InsertUpdate();//Added By Rupesh G. on 08/08/2019 for EXIM

                btnBankName.Visible = true;
                this.txtBankName.Size = new System.Drawing.Size(359, 21);
                setEnableDisable(false);
                this.pAddMode = false;
                this.pEditMode = false;
                setNavigation();
                bindControls(txtBankName.Text);

                SqlStr = "select ac_id from ac_mast where ac_name='" + txtBankName.Text + "'";
                DataTable dtAc_id = new DataTable();
                dtAc_id = oDataAccess.GetDataTable(SqlStr, null, 20);
                if (dtAc_id.Rows.Count > 0)
                {
                    int acid = Int32.Parse(dtAc_id.Rows[0][0].ToString());
                    oDataAccess.BeginTransaction();
                    SqlStr = "Delete from InterestRateDetail where ac_id=" + acid + "";
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                    oDataAccess.CommitTransaction();

                    DataTable dd = new DataTable();
                    dd = InterestRateDetails.dtRate;
                    if (dd.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dd.Rows)
                        {
                            oDataAccess.BeginTransaction();
                            SqlStr = "Set Dateformat DMY  insert into InterestRateDetail values(" + acid + ",'" + dr[0] + "','" + dr[1] + "','" + dr[2] + "','" + dr[3] + "')";
                            oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                            oDataAccess.CommitTransaction();
                        }
                    }
                    else
                    {
                        SqlStr = "select sta_dt as 'from',end_dt 'to', 0.00'roi',DATEDIFF(day, sta_dt,end_dt)+1 AS pnd from vudyog..co_mast where compid=" + this.pCompId + "";
                        dd = oDataAccess.GetDataTable(SqlStr, null, 20);
                        foreach (DataRow dr in dd.Rows)
                        {
                            oDataAccess.BeginTransaction();
                            SqlStr = "Set Dateformat DMY  insert into InterestRateDetail values(" + acid + ",'" + dr[0] + "','" + dr[1] + "','" + dr[2] + "','" + dr[3] + "')";
                            oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                            oDataAccess.CommitTransaction();
                        }
                    }
                }
            }
           



        }

        //Added By Rupesh G. on 08/08/2019 for EXIM----Start

        private void InsertUpdate()
        {
            try
            {
                if (this.pAddMode == true)
                {
                    oDataAccess.BeginTransaction();
                    SqlStr = "";
                    this.mthInsertSting(ref SqlStr);
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                    oDataAccess.CommitTransaction();

                }
                else if (this.pEditMode == true)
                {
                    oDataAccess.BeginTransaction();
                    this.mthUpdateString(ref SqlStr);
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                    oDataAccess.CommitTransaction();
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.ToString());
            }
        }

        private void mthInsertSting(ref string SqlStr)
        {
            string vFld_Nm = "";
            string vData_Ty = "";
            string SqlStr1, SqlStr2;


            SqlStr1 = "Set dateformat dmy INSERT INTO Ac_mast(ac_name, mailname,[group], Ac_group_id, typ, contact, designatio, add1, add2, add3";
            SqlStr1 = SqlStr1 + ", Area, Zone, city, City_id, District,[state], StateCode, State_id, zip, country, Country_id, phone, fax, phoner, mobile, email";
            SqlStr1 = SqlStr1 + ", bankbr, U_BKACTYPE, bankno, U_MICRNO, U_IFSCCODE, bsrcode,[type],compid,ac_type_id,cr_days,cramount,crallow,ldeactive,salesman";

            SqlStr2 =  " VALUES('" + bankname + "', '" + mailname + "', '" + group + "', " + acgroupId + ", '" + typ + "', '" + contact + "', '" + designation + "' ";
            SqlStr2 = SqlStr2 + ", '" + add1 + "', '" + add2 + "', '" + add3 + "', '" + Area + "', '" + Zone + "', '" + CITY + "', " + City_id + ", '" + District + "', '" + state + "' ";
            SqlStr2 = SqlStr2 + ", '" + stateCode + "', " + State_id + ", '" + zip + "', '" + country + "', " + Country_id + ", '" + office + "', '" + fax + "', '" + phoner + "', '" + cellno + "', '" + email + "'";
            SqlStr2 = SqlStr2 + ", '" + branch + "', '" + ACTYPE + "', '" + ACNO + "', '" + mcrno + "', '" + ifsc + "', '" + bsrcode + "','B'," + comp_id + "," + ac_type_id + "," + cr_days + "," + cr_limit + ",'" + overlimit + "',0,''";


           

            foreach (DataRow dr in tblAddInfo.Rows)
            {
                vFld_Nm = dr["Fld_Nm"].ToString().Trim();
                vData_Ty = dr["Data_Ty"].ToString().Trim();
                SqlStr1 = SqlStr1 + "," + vFld_Nm;
                //vData_Ty = dr[vFld_Nm].GetType().ToString();
                SqlStr2 = SqlStr2 + "," + "'" + dtBankRec.Rows[0][vFld_Nm].ToString() + "'";
            }

            SqlStr1 = SqlStr1 + ")";
            SqlStr = SqlStr1 + " " + SqlStr2 + ")";
            SqlStr = SqlStr.Replace("'True'", "1").Replace("'False'", "0");
        }

        private void mthUpdateString(ref string SqlStr)
        {
         
            SqlStr = "set dateformat dmy update Ac_mast set ";
            SqlStr = SqlStr + "ac_name = '" + bankname + "', mailname = '" + mailname + "',[group] = '" + group + "', Ac_group_id = " + acgroupId + ",";
            SqlStr = SqlStr + "typ = '" + typ + "', contact = '" + contact + "', designatio = '" + designation + "', add1 = '" + add1 + "', add2 = '" + add2 + "', add3 = '" + add3 + "',";
            SqlStr = SqlStr + "Area = '" + Area + "', Zone = '" + Zone + "', city = '" + CITY + "', City_id = " + City_id + ", District = '" + District + "',[state] = '" + state + "',";
            SqlStr = SqlStr + "StateCode = '" + stateCode + "', State_id = " + State_id + ", zip = '" + zip + "', country = '" + country + "', Country_id = " + Country_id + ",";
            SqlStr = SqlStr + "phone = '" + office + "', fax = '" + fax + "', phoner = '" + phoner + "', mobile = '" + cellno + "', email = '" + email + "', bankbr = '" + branch + "',";
            SqlStr = SqlStr + "U_BKACTYPE = '" + ACTYPE + "', bankno = '" + ACNO + "', U_MICRNO = '" + mcrno + "', U_IFSCCODE = '" + ifsc + "', bsrcode = '" + bsrcode + "',compid=" + comp_id + ",ac_type_id=" + ac_type_id + ",cr_days=" + cr_days + ",cramount=" + cr_limit + ",crallow='" + overlimit + "'";
           

            string vFld_Nm = "";
            string vData_Ty = "";
            foreach (DataRow dr in tblAddInfo.Rows)
            {
                vFld_Nm = dr["Fld_Nm"].ToString().Trim();
                vData_Ty = dr["Data_Ty"].ToString().Trim();
                SqlStr = SqlStr + "," + vFld_Nm;
                SqlStr = SqlStr + "=" + "'" + dtBankRec.Rows[0][vFld_Nm].ToString() + "'";
            }
            SqlStr = SqlStr + " where ac_name = '" + bankname + "'";
            SqlStr = SqlStr.Replace("'True'", "1").Replace("'False'", "0");
        }

        //Added By Rupesh G. on 08/08/2019 for EXIM----END

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.pEditMode = true;
            this.pAddMode = false;
            btnBankName.Visible = false;
            this.txtBankName.Size = new System.Drawing.Size(392, 21);
            setEnableDisable(true);
            txtBankName.Enabled = false;
            setNavigation();
            ccount = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ccount = 1;
            btnBankName.Visible = true;
            this.txtBankName.Size = new System.Drawing.Size(359, 21);
            getAccountName();
            bindControls(ac_name);
            setEnableDisable(false);
            this.pAddMode = false;
            this.pEditMode = false;
            setNavigation();
            InterestRateDetails.dtRate = new DataTable();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you wish to delete this Record ?", this.pPApplText,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    oDataAccess.BeginTransaction();
                    SqlStr = "Delete from ac_mast where ac_name='" + txtBankName.Text.ToString() + "'";
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                    oDataAccess.CommitTransaction();
                    oDataAccess.BeginTransaction();
                    SqlStr = "Delete from InterestRateDetail where ac_id=" + ac_id + "";
                    oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                    oDataAccess.CommitTransaction();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(" Cannot Delete !!!Transaction has generated against this Account.", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
                this.txtBankName.Size = new System.Drawing.Size(359, 21);
                getAccountName();
                bindControls(ac_name);
                setEnableDisable(false);
                this.pAddMode = false;
                this.pEditMode = false;
                setNavigation();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            mDeleteProcessIdRecord();
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

        private void btnBankName_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select  ac_name,(case when ISNULL(MailName,space(1))=space(1) then ac_name else mailname end) as mailname,[Group]  from AC_MAST where [typ]='Bank' and type in('B') order by ac_name";

            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select Bank Name";
            vSearchCol = "ac_name";
            vDisplayColumnList = "ac_name:Bank Name,mailname:Mailing Name,Group:Account Group";
            vReturnCol = "ac_name,mailname,Group";
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
                txtBankName.Text = oSelectPop.pReturnArray[0];
                txtMName.Text = oSelectPop.pReturnArray[1];
                txtMainGroup.Text = oSelectPop.pReturnArray[2];
            }
            pAddMode = false;
            pAddMode = false;
            setNavigation();

            bindControls(txtBankName.Text);
        }

        private void btnMainGroup_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select ac_group_name as GroupName from ac_group_mast where (type in('B') and [group]='CASH & BANK BALANCES') or ac_group_name='CASH & BANK BALANCES' order by ac_group_name";

            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select Group Name";
            vSearchCol = "GroupName";
            vDisplayColumnList = "GroupName:Group Name";
            vReturnCol = "GroupName";
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
                txtMainGroup.Text = oSelectPop.pReturnArray[0];
            }
        }

        private void btnArea_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Distinct Area From Ac_Mast Where Area <> ''";

            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select Area";
            vSearchCol = "Area";
            vDisplayColumnList = "Area:Area";
            vReturnCol = "Area";
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
                txtArea.Text = oSelectPop.pReturnArray[0];
            }
        }

        private void btnCity_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select distinct city from city where city is not null AND City <> '' Order By City";

            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select city";
            vSearchCol = "city";
            vDisplayColumnList = "city:City";
            vReturnCol = "city";
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
                txtCity.Text = oSelectPop.pReturnArray[0];
            }
        }

        private void btnState_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select state,gst_state_code as scode from state where state !='' order by state";

            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select State";
            vSearchCol = "state";
            vDisplayColumnList = "state:State,scode:State Code";
            vReturnCol = "state,scode";
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
                txtState.Text = oSelectPop.pReturnArray[0];
                txtStateCode.Text = oSelectPop.pReturnArray[1];
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            SqlStr = "select top 1 ac_name from ac_mast where typ='Bank'";
            DataTable dt_ACName = new DataTable();
            dt_ACName = oDataAccess.GetDataTable(SqlStr, null, 20);

            if (dt_ACName.Rows.Count > 0)
            {
                bindControls(dt_ACName.Rows[0][0].ToString());
            }
            this.mthChkNavigationButton();  //Divyang
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            SqlStr = "select top 1 ac_name from ac_mast where typ='Bank' order by ac_id desc";
            DataTable dt_ACName = new DataTable();
            dt_ACName = oDataAccess.GetDataTable(SqlStr, null, 20);

            if (dt_ACName.Rows.Count > 0)
            {
                bindControls(dt_ACName.Rows[0][0].ToString());
            }
            this.mthChkNavigationButton();  //Divyang
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            SqlStr = "select  ac_name from ac_mast where typ='Bank' and Ac_id=(select top 1 Ac_id from ac_mast where typ='Bank' and Ac_id<(select ac_id from ac_mast where ac_name='" + txtBankName.Text + "') order by Ac_id desc )";
            DataTable dt_ACName = new DataTable();
            dt_ACName = oDataAccess.GetDataTable(SqlStr, null, 20);

            if (dt_ACName.Rows.Count > 0)
            {
                bindControls(dt_ACName.Rows[0][0].ToString());
            }
            this.mthChkNavigationButton();  //Divyang
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            SqlStr = "select  ac_name from ac_mast where typ='Bank' and Ac_id=(select top 1 Ac_id from ac_mast where typ='Bank' and Ac_id>(select ac_id from ac_mast where ac_name='" + txtBankName.Text + "') order by Ac_id  )";
            DataTable dt_ACName = new DataTable();
            dt_ACName = oDataAccess.GetDataTable(SqlStr, null, 20);

            if (dt_ACName.Rows.Count > 0)
            {
                bindControls(dt_ACName.Rows[0][0].ToString());
            }
            this.mthChkNavigationButton();  //Divyang
        }

        private void btnCountry_Click(object sender, EventArgs e)
        {
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "Select Country from Country where Country is not null and Country <> '' order by Country";

            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select Country";
            vSearchCol = "Country";
            vDisplayColumnList = "Country:Country";
            vReturnCol = "Country";
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
                txtCountry.Text = oSelectPop.pReturnArray[0];

            }
        }

        private void mthChkNavigationButton()           //Divyang 12052020
        {
            DataSet dsTemp = new DataSet();
            this.btnForward.Enabled = false;
            this.btnLast.Enabled = false;
            this.btnFirst.Enabled = false;
            this.btnBack.Enabled = false;
            //btnEdit.Enabled = false;
            Boolean vBtnAdd, vBtnEdit, vBtnDelete, vBtnPrint;
            
            if (this.pAddMode == false && this.pEditMode == false)
            {
                if (dtBankRec.Rows.Count > 0)
                {
                  
                    SqlStr = " select top 1 ac_name from ac_mast where typ='Bank' and ac_id >'"+dtBankRec.Rows[0]["Ac_id"]+"' order by ac_id desc ";

                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, 20);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnForward.Enabled = true;
                        this.btnLast.Enabled = true;
                    }
                    SqlStr = " select top 1 ac_name from ac_mast where typ='Bank' and ac_id <'" + dtBankRec.Rows[0]["Ac_id"] + "' order by ac_id desc ";

                    dsTemp = oDataAccess.GetDataSet(SqlStr, null, 20);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        this.btnBack.Enabled = true;
                        this.btnFirst.Enabled = true;
                    }
                }

            }

            //vBtnAdd = false;
            //vBtnDelete = false;
            //vBtnEdit = false;
            //vBtnPrint = false;
            //if (ServiceType.ToUpper() != "VIEWER VERSION")
            //{
            //    if (this.btnForward.Enabled == true || this.btnBack.Enabled == true || (this.pAddMode == false && this.pEditMode == false))
            //    {
            //        vBtnAdd = true;
            //        if (vTblMain.Rows.Count > 0)
            //        {
            //            vBtnDelete = true;
            //            vBtnEdit = true;
            //            vBtnPrint = true;
            //            this.btnValidIn.Enabled = false;


            //        }
            //    }
            //    this.mthChkAEDPButton(vBtnAdd, vBtnDelete, vBtnEdit, vBtnPrint);
            //}
        }

        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udBankMaster.exe";
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
    }
}
