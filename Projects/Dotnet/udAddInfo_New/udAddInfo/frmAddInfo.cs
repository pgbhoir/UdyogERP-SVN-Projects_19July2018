using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.IO;



namespace udAddInfo
{
    public partial class frmAddInfo : uBaseForm.FrmBaseForm
    {
        DataTable vTblAddInfo, vTblMain;
        uBaseForm.FrmBaseForm vParentForm;
        DataAccess_Net.clsDataAccess oDataAccess;
        string SqlStr, vTblMainNm;
        public int rowId=0;//Add Rupesh G.
        public frmAddInfo()
        {
            InitializeComponent();
           
        }
        
        private void frmAddInfo_Load(object sender, EventArgs e)
        {
          
            this.KeyPreview = true;

            this.pAddMode = this.pParentForm.pAddMode;
            this.pEditMode = this.pParentForm.pEditMode;
            this.Icon = this.pParentForm.Icon;


            this.pDisableCloseBtn = this.pParentForm.pDisableCloseBtn;
            this.FormBorderStyle = this.pParentForm.FormBorderStyle;
            this.MaximizeBox = this.pParentForm.MaximizeBox;

            this.pPApplPID = this.pParentForm.pPApplPID;
            this.pPara = this.pParentForm.pPara;
            this.Text = "Additional Information";
            this.pCompId = this.pParentForm.pCompId; //Rup 25/07/2109
            this.pComDbnm = this.pParentForm.pComDbnm;
            this.pServerName = this.pParentForm.pServerName;
            this.pUserId = this.pParentForm.pUserId;
            this.pPassword = this.pParentForm.pPassword;
            this.pPApplRange = this.pParentForm.pPApplRange;
            this.pAppUerName = this.pParentForm.pAppUerName;
            this.Icon = this.pParentForm.Icon;
            this.pPApplText = this.pParentForm.pPApplText;
            this.pPApplName = this.pParentForm.pPApplName;
            this.pPApplPID = this.pParentForm.pPApplPID;
            this.pPApplCode = this.pParentForm.pPApplCode;
            this.pAppPath = this.pParentForm.pAppPath;

            this.mthAddControls();

            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            this.Text = "Additional Information";
        }
        
        private void mthAddControls()
        {
           
            int cnt = 0;
            string vHead_Nm, vFld_Nm, vData_Ty, VFiltCond, vCtrlNm;
            int vFld_Wid, vFld_Dec;
            int vTop = 10, vLeft = 0, vLblWidth = 150, vTxtwidth = 200, vDtpWidh = 100, vHeight = 20;
            bool vEnabled = ((this.pAddMode == true || this.pEditMode == true) ? true : false);
            bool vReadonly = ((this.pAddMode == false && this.pEditMode == false) ? true : false);
            foreach (DataRow dr in this.pTblAddInfo.Rows)
            {
                cnt = cnt + 1;
                if (cnt % 2 == 0)
                {
                    vLeft = 10 + vLblWidth + 20 + vTxtwidth + 30;

                }
                else
                {
                    vLeft = 10;
                }
                vHead_Nm = dr["Head_Nm"].ToString().Trim();
                vFld_Nm = dr["Fld_Nm"].ToString().Trim();
                vData_Ty = dr["Data_Ty"].ToString().Trim().ToUpper();
                vFld_Wid = (int)dr["Fld_Wid"];
                vFld_Dec = (int)dr["Fld_Dec"];
                VFiltCond = dr["FiltCond"].ToString().Trim();
                if (vData_Ty != "BIT")
                {
                    Label vLabel = new Label();
                    vLabel.Text = vHead_Nm;
                    vLabel.Tag = vFld_Nm;
                    vLabel.Height = vHeight;
                    vLabel.Width = vLblWidth;
                    vLabel.Top = vTop;
                    vLabel.Left = vLeft;
                    vLabel.Name = "lbl" + "_" + vFld_Nm;
                    this.Controls.Add(vLabel);
                    if (vData_Ty == "VARCHAR")
                    {
                        TextBox vTextBox = new TextBox();

                        vTextBox.Tag = vFld_Nm;
                        vTextBox.Size = new System.Drawing.Size(vTxtwidth, vHeight);
                        vTextBox.Top = vTop;
                        vTextBox.Left = vLabel.Left + vLabel.Width + 30;
                        vTextBox.Name = "txt" + "_" + vFld_Nm;
                        if (this.pTblMain.Rows.Count>0)
                        {
                            //if (this.pTblMain.Rows[0][vFld_Nm]!=null)
                            //{
                          //  vTextBox.Text = this.pTblMain.Rows[0][vFld_Nm].ToString();Commented by rupesh G 
                            vTextBox.Text = this.pTblMain.Rows[rowId][vFld_Nm].ToString();//Added by reupesh G
                            //}
                        }
                        
                        
                        
                        if (string.IsNullOrEmpty(VFiltCond) ==false)
                        {                            
                            vTextBox.Width = vTextBox.Width - 20 - 10;
                            Button vHelpButton = new Button();
                            vHelpButton.Tag = VFiltCond;
                            vHelpButton.Height = vHeight;
                            vHelpButton.Width = 20;
                            vHelpButton.Top = vTop;
                            vHelpButton.Left = vTextBox.Left + vTextBox.Width + 10;
                            vHelpButton.Visible = true;
                            vHelpButton.Name = "hbtn" + "_" + vFld_Nm;
                            string fName = this.pAppPath + @"\bmp\loc-on.gif";
                            if (File.Exists(fName) == true)
                            {
                                vHelpButton.Image = Image.FromFile(fName);
                            }
                            vTextBox.ReadOnly = true;
                            vTextBox.Tag = VFiltCond;
                            vTextBox.KeyDown += new KeyEventHandler(vTextBox_KeyDown);
                            vHelpButton.Click += new System.EventHandler(this.vHelpButton_Click); //Rup 25/07/2109
                            this.Controls.Add(vTextBox);
                            this.Controls.Add(vHelpButton);
                        }
                        else
                        {
                            vTextBox.ReadOnly = vReadonly;
                            vTextBox.KeyDown += new KeyEventHandler(vTextBox_KeyDown);
                            this.Controls.Add(vTextBox);
                        }


                    }
                    if (vData_Ty == "DECIMAL")
                    {
                        uNumericTextBox.cNumericTextBox vTextBox = new uNumericTextBox.cNumericTextBox();
                        vTextBox.Tag = vFld_Nm;
                        vTextBox.Height = vHeight;
                        vTextBox.Width = vTxtwidth;
                        vTextBox.Top = vTop;
                        vTextBox.Left = vLabel.Left + vLabel.Width + 30;
                        vTextBox.Name = "txt" + "_" + vFld_Nm;
                        vTextBox.pAllowNegative = false;
                        vTextBox.pDecimalLength = vFld_Dec;
                        vTextBox.MaxLength = vFld_Wid;
                       
                        vTextBox.Enabled = vEnabled;
                        if (this.pTblMain.Rows.Count > 0)
                        {
                           
                                //vTextBox.Text = this.pTblMain.Rows[0][vFld_Nm].ToString();//Rupesh G
                            vTextBox.Text = this.pTblMain.Rows[rowId][vFld_Nm].ToString();
                        }
                        this.Controls.Add(vTextBox);
                    }
                    if (vData_Ty == "DATETIME")
                    {
                        DateTimePicker vDateTimePicker = new DateTimePicker();
                        vDateTimePicker.Format = DateTimePickerFormat.Custom;
                        vDateTimePicker.CustomFormat = "dd/MM/yyyy";
                        vDateTimePicker.Tag = vFld_Nm;
                        vDateTimePicker.Height = vHeight;
                        vDateTimePicker.Width = vDtpWidh;
                        vDateTimePicker.Top = vTop;
                        vDateTimePicker.Left = vLabel.Left + vLabel.Width + 30;
                        vDateTimePicker.Name = "dtp" + "_" + vFld_Nm;
                        if (this.pTblMain.Rows.Count > 0)
                        {
                            
                                 //vDateTimePicker.Value = (DateTime)this.pTblMain.Rows[0][vFld_Nm];//Rupesh G
                            vDateTimePicker.Value = (DateTime)this.pTblMain.Rows[rowId][vFld_Nm];
                        }
                        vDateTimePicker.Enabled = vEnabled;
                        this.Controls.Add(vDateTimePicker);
                    }

                }
                else //(vData_Ty != "Bit"
                {
                    CheckBox vCheckBox = new CheckBox();
                    vCheckBox.Text = vHead_Nm;
                    vCheckBox.Tag = vFld_Nm;
                    vCheckBox.Height = vHeight;
                    vCheckBox.Width = vLblWidth;
                    vCheckBox.Top = vTop;
                    vCheckBox.Left = vLeft;
                    vCheckBox.Name = "chk" + "_" + vFld_Nm;
                    if (this.pTblMain.Rows.Count > 0)
                    {
                        if (((bool)this.pTblMain.Rows[rowId][vFld_Nm]).ToString() == "")
                        {
                            vCheckBox.Checked = false;
                        }
                        else
                        {
                            vCheckBox.Checked = (bool)this.pTblMain.Rows[rowId][vFld_Nm]; //Rup 25/07/2109
                            
                           // vCheckBox.Checked = (bool)this.pTblMain.Rows[0][vFld_Nm]; //Rup 25/07/2109
                        }
                    }
                    vCheckBox.Enabled = vEnabled;
                    this.Controls.Add(vCheckBox);
                }
                if (cnt % 2 == 0)
                {
                    vTop = vTop + vHeight + 7;
                }
            } // foreach (DataRow dr in this.pTblAddInfo.Rows)
            this.Height = this.Height + vTop;

        }
        private void vHelpButton_Click(object sender, EventArgs e)
        {
            if (this.pAddMode == false && this.pEditMode == false)
            {
                return;
            }
            string vFormText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            Control vCntrl = new Control();
            vCntrl = ((Control)sender);
            if (vCntrl.Name.Substring(0, 5) == "hbtn_")
            {
                var t1 = this.Controls.Find("$$$", false);
                var t2 = this.Controls.Find("$$$", false);
                foreach (Control vcnt in this.Controls)
                {

                    if (vcnt.Name == vCntrl.Name.Replace("hbtn_", "lbl_"))
                    {

                        t2 = this.Controls.Find(vcnt.Name, false);
                        if (t2 != null)
                        {
                            vFormText = "Select " + t2[0].Text;
                        }

                    }
                    if (vcnt.Name == vCntrl.Name.Replace("hbtn_", "txt_"))
                    {

                        t1 = this.Controls.Find(vcnt.Name, false);

                    }

                }
                if (t1[0].Text == "$$$")
                {
                    return;
                }
                //Call Selectpop--->

                strSQL = vCntrl.Tag.ToString();

                SqlStr = strSQL.Substring(0, strSQL.IndexOf("{"));
                strSQL = strSQL.Replace(SqlStr, "");
                DataSet tDs = new DataSet();
                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
                DataView dvw = tDs.Tables[0].DefaultView;



                vSearchCol = strSQL.Substring(0, strSQL.IndexOf("#") + 1);
                strSQL = strSQL.Replace(vSearchCol, "");
                vSearchCol = vSearchCol.Replace("{", "").Replace("#", "").Replace("+", ",");

                vReturnCol = strSQL.Substring(0, strSQL.IndexOf("#") + 1).Replace("+", ",");
                strSQL = strSQL.Replace(vReturnCol, "");
                vReturnCol = vReturnCol.Replace("#", "");

                vColExclude = strSQL.Substring(0, strSQL.IndexOf("#") + 1);
                strSQL = strSQL.Replace(vColExclude, "");
                vColExclude = vColExclude.Replace("#", "").Replace("+", ",");

                vDisplayColumnList = strSQL.Replace("}", "");


                udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
                oSelectPop.pdataview = dvw;
                oSelectPop.pformtext = vFormText;
                oSelectPop.psearchcol = vSearchCol;
                oSelectPop.pDisplayColumnList = vDisplayColumnList;
                oSelectPop.pRetcolList = vReturnCol;

                oSelectPop.ShowDialog();
                if (oSelectPop.pReturnArray != null)
                {
                    t1[0].Text = oSelectPop.pReturnArray[0];
                }

                //<---Call Selectpop



            }
        }
        private void vHelpButton_F2(object sender, EventArgs e)
        {
            if (this.pAddMode == false && this.pEditMode == false)
            {
                return;
            }
            string vFormText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            Control vCntrl = new Control();
            vCntrl = ((Control)sender);
            if (vCntrl.Name.Substring(0, 4) == "txt_")
            {
                var t1 = vCntrl;
                var t2 = this.Controls.Find("$$$", false);
                foreach (Control vcnt in this.Controls)
                {

                    if (vcnt.Name == vCntrl.Name.Replace("txt_", "lbl_"))
                    {

                        t2 = this.Controls.Find(vcnt.Name, false);
                        if (t2 != null)
                        {
                            vFormText = "Select " + t2[0].Text;
                        }

                    }
                    
                }
                
                //Call Selectpop--->

                strSQL = vCntrl.Tag.ToString();

                SqlStr = "Select Distinct "+t2[0].Name.Replace("lbl_","")+" From "+this.pTblMainNm + " Order By "+t2[0].Name.Replace("lbl_", "");
                MessageBox.Show(SqlStr);
                DataSet tDs = new DataSet();
                tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
                DataView dvw = tDs.Tables[0].DefaultView;



                vSearchCol = t2[0].Name.Replace("lbl_", "");

                vReturnCol = t2[0].Name.Replace("lbl_", "");

                vColExclude = "";
                
                vDisplayColumnList = t1.Name.Replace("txt_","")+":"+ t2[0].Text;
                MessageBox.Show(vSearchCol);
                MessageBox.Show(vReturnCol);
                MessageBox.Show(vColExclude);

                udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
                oSelectPop.pdataview = dvw;
                oSelectPop.pformtext = vFormText;
                oSelectPop.psearchcol = vSearchCol;
                oSelectPop.pDisplayColumnList = vDisplayColumnList;
                oSelectPop.pRetcolList = vReturnCol;

                oSelectPop.ShowDialog();
                if (oSelectPop.pReturnArray != null)
                {
                    t1.Text = oSelectPop.pReturnArray[0];
                }

                //<---Call Selectpop



            }
        }
        private void vTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.pAddMode == false && this.pEditMode == false)
            {
                return;
            }
            if (e.KeyCode == Keys.F2)
            {
                // MessageBox.Show(((TextBox)sender).Tag.ToString());
                var t2 = this.Controls.Find(((TextBox)sender).Name.Replace("txt_", "hbtn_"), false);
                
                if (t2.Length>0)
                {
                  //  MessageBox.Show("Yes");
                    this.vHelpButton_Click(t2[0], e);
                }
                else
                {
                 //   MessageBox.Show("No");
                    this.vHelpButton_F2(sender, e);
                }
                
            }


        }
        private void btnDone_Click(object sender, EventArgs e)
        {
           
            string vFld_Nm, vData_Ty, vVCtrlNm;
            foreach (DataRow dr in this.pTblAddInfo.Rows)
            {

                vFld_Nm = dr["Fld_Nm"].ToString().Trim();
              
                vData_Ty = dr["Data_Ty"].ToString().Trim().ToUpper();
               
                if (vData_Ty == "VARCHAR" || vData_Ty == "DECIMAL")
                {
                    vVCtrlNm = "txt_" + vFld_Nm;
                    this.pTblMain.Rows[rowId][vFld_Nm] = this.Controls[vVCtrlNm].Text.Trim();
                  //  this.pTblMain.Rows[0][vFld_Nm] = this.Controls[vVCtrlNm].Text.Trim();Rupesh G
                   
                }
                else
                {
                    if (vData_Ty == "DATETIME")
                    {
                      
                        vVCtrlNm = "dtp_" + vFld_Nm;
                        this.pTblMain.Rows[rowId][vFld_Nm] = ((DateTimePicker)this.Controls[vVCtrlNm]).Value;
                        //this.pTblMain.Rows[0][vFld_Nm] = ((DateTimePicker)this.Controls[vVCtrlNm]).Value;//Rupesh G
                    }
                    else
                    {
                      
                        vVCtrlNm = "chk_" + vFld_Nm;
                        this.pTblMain.Rows[rowId][vFld_Nm] = ((CheckBox)this.Controls[vVCtrlNm]).Checked;//Rupesh G
                        //this.pTblMain.Rows[0][vFld_Nm] = ((CheckBox)this.Controls[vVCtrlNm]).Checked;
                    }
                }

                this.Close();
            }
            this.pTblMain.AcceptChanges();
            //vFld_Nm = "U_AA";
            //MessageBox.Show(this.pTblMain.Rows[0][vFld_Nm].ToString());
           
            this.Close();
        }

        public DataTable pTblAddInfo
        {

            get
            {
                return vTblAddInfo;
            }
            set
            {
                vTblAddInfo = value;
            }
        }

        private void frmAddInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.D)      
            {
                this.btnDone_Click(sender, e);
                e.SuppressKeyPress = true;  // Stops other controls on the form receiving event.
            }
        }

        public DataTable pTblMain
        {

            get
            {
                return vTblMain;
            }
            set
            {
                vTblMain = value;
            }
        }
        public string pTblMainNm
        {
            get
            {
                return vTblMainNm;
            }
            set
            {
                vTblMainNm = value;
            }
        }
        public uBaseForm.FrmBaseForm pParentForm
        {

            get
            {
                return vParentForm;
            }
            set
            {
                vParentForm = value;
            }
        }

    }
}
