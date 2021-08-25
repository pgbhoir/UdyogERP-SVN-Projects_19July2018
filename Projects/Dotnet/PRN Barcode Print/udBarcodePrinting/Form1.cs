using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udBarcodePrinting
{
    public partial class Form1 : uBaseForm.FrmBaseForm
    {
        string SqlStr = string.Empty, tblname, behaviour;
        string[] parts;
        string FldName = "";
        string prn;
        string prnString;
        DataAccess_Net.clsDataAccess oDataAccess;
        DataTable dtControls;
        DataTable dtRecord;
        DataTable dtSharePrintName;
        string barcodeName;
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
            //this.pFrmCaption = "Barcode Printing";   //Commented by Priyanka B on 29122018 for Bug-32062
            this.pFrmCaption = "Label-Barcode Printing";  //Modified by Priyanka B on 29122018 for Bug-32062
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
            btnPrint.Enabled = false;
            this.Height = this.Height - groupBox2.Height - 7;
            try
            {
                DataTable dtPrinter = new DataTable();
                dtPrinter = getPrinterName();
                if (dtPrinter.Rows.Count > 0)
                {
                    DataRow[] result = dtPrinter.Select("Sharename <> ''");

                    dtSharePrintName = new DataTable();
                    dtSharePrintName = result.CopyToDataTable();

                    foreach (DataRow dr in dtSharePrintName.Rows)
                    {
                        ddlPrinterName.Items.Add(dr[0].ToString());
                    }

                    DataRow[] result1 = dtSharePrintName.Select("Default=True");
                    if (result1.Length > 0)
                    {

                        ddlPrinterName.SelectedItem = result1[0][0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            this.mInsertProcessIdRecord();

        }

        private DataTable getPrinterName()
        {
            DataTable dtPrinter = new DataTable();
            dtPrinter.Columns.Add("name");
            dtPrinter.Columns.Add("Status");
            dtPrinter.Columns.Add("Default");
            dtPrinter.Columns.Add("Network");
            dtPrinter.Columns.Add("Sharename");

            var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
            foreach (var printer in printerQuery.Get())
            {
                try
                {
                    var name = printer.GetPropertyValue("Name");
                    var status = printer.GetPropertyValue("Status");
                    var isDefault = printer.GetPropertyValue("Default");
                    var isNetworkPrinter = printer.GetPropertyValue("Network");
                    var sname = printer.GetPropertyValue("Sharename");

                    dtPrinter.Rows.Add(name, status, isDefault, isNetworkPrinter, sname);


                }
                catch (Exception ee)
                {

                }
            }
            return dtPrinter;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {


                string s1 = "";
                string s = "";
                int i = 0;
                string field = "";
                DataTable dd = dtControls;

                //if (dd.Rows.Count > 0)
                //{


                //var distinctRows = (from DataRow dRow in dd.Rows
                //                    select dRow["tblname"]).Distinct();

                //var distinctRows1 = (from DataRow dRow in dd.Rows
                //                     select dRow["behaviour"]).Distinct();

                string[] tablename = new string[2];
                if (behaviour != "" && behaviour != "IM")
                {
                    tablename[0] = behaviour + "MAIN";
                    tablename[1] = behaviour + "ITEM";
                }
                else
                {
                    tablename[0] = "IT_MAST";

                }
                string[] distinctRows1 = new string[1];
                distinctRows1[0] = behaviour;

                ////
                foreach (string str in tablename)
                {
                    string sQuery1;

                    sQuery1 = "SELECT COLUMN_NAME=('" + str + ".['+COLUMN_NAME+']'),alise=('" + str + "_'+COLUMN_NAME)  FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + str + "'";
                    DataTable dtFld1 = new DataTable();
                    dtFld1 = oDataAccess.GetDataTable(sQuery1, null, 20);
                    foreach (DataRow dr in dtFld1.Rows)
                    {
                        field = field + "," + dr[0].ToString() + " as " + dr[1].ToString();
                    }
                }

                field = field.Substring(1);
                ////


                string sQuery = " set dateformat dmy select " + field + " from ";
                int i1 = 0;
                foreach (string str in tablename)
                {
                    if (str != null)
                    {
                        if (i1 == 0)
                        {
                            sQuery = sQuery + str;
                        }
                        else
                        {
                            sQuery = sQuery + " inner join " + str;
                        }

                        i1++;
                    }

                }

                foreach (string str in distinctRows1)
                {
                    if (str != "" && str != "IM" && i1 > 1)
                    {
                        sQuery = sQuery + " on " + behaviour + "MAIN.entry_ty=" + behaviour + "ITEM.entry_ty and " + behaviour + "MAIN.tran_cd=" + behaviour + "ITEM.Tran_cd ";
                    }

                }


                foreach (DataRow dr in dd.Rows)
                {

                    if (dr[1].ToString() != "button" && dr[1].ToString() != "chekbox")
                    {
                        Control[] ctls = this.Controls.Find(dr[0].ToString(), true);
                        if (ctls[0].Text.ToString() != "")
                        {
                            if (i == 0)
                            {
                                if (dr[3].ToString() == "varchar" || dr[3].ToString() == "datetime")
                                {
                                    s = dr[4].ToString() + "." + dr[2].ToString() + "='" + ctls[0].Text.ToString() + "'";
                                }
                                else
                                {
                                    s = dr[4].ToString() + "." + dr[2].ToString() + "=" + ctls[0].Text.ToString();
                                }

                            }
                            else
                            {
                                s = s + " and " + dr[4].ToString() + "." + dr[2].ToString() + "='" + ctls[0].Text.ToString() + "'";
                            }
                        }

                    }
                    else if (dr[1].ToString() == "chekbox")
                    {
                        Control[] ctls = this.Controls.Find(dr[0].ToString(), true);
                        CheckBox checkBox = ctls[0] as CheckBox;

                        if (i == 0)
                        {
                            s = dr[4].ToString() + "." + dr[2].ToString() + "=" + checkBox.Checked.ToString();
                        }
                        else
                        {
                            s = s + " and " + dr[4].ToString() + "." + dr[2].ToString() + "=" + checkBox.Checked.ToString() + "";
                        }
                    }
                    i++;

                }

                if (s.Length > 0)
                {
                    sQuery = sQuery + " where " + s;
                }

                dtRecord = new DataTable();
                dtRecord = oDataAccess.GetDataTable(sQuery, null, 20);
                //   }


                ////////////////***********************************
                genratePRN();
                ///////////////************************************

            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
            }
        }
        private void genratePRN()
        {

            DataRow[] printerShareName = dtSharePrintName.Select("name='" + ddlPrinterName.SelectedItem.ToString().Trim() + "'");

            if (printerShareName.Length > 0)
            {
                string printSName = printerShareName[0][4].ToString();
                if (dtRecord.Rows.Count == 0)
                {
                    MessageBox.Show("Records not found...!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    try
                    {
                        string fldname, rl;
                        int sp, ep;
                        string txtPrn = "";

                        //  string[] delimiters = new string[] { "<<", ">>" };
                        //parts = ffld.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                        foreach (DataRow dr in dtRecord.Rows)
                        {

                            string prn1 = prn;

                            string[] var = prn.Split('\n');

                            foreach (string str in var)
                            {
                                string[] delimiters = new string[] { "@@" };
                                parts = str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                                foreach (string str1 in parts)
                                {

                                    if (str1.StartsWith("<<") && str1.EndsWith(">>"))
                                    {
                                        string[] delimiters1 = new string[] { "<<", ">>" };
                                        parts = str1.Split(delimiters1, StringSplitOptions.RemoveEmptyEntries);

                                        fldname = parts[0].ToString();

                                        string[] delimiters12 = fldname.Split('#');

                                        if (delimiters12[0] == "H")
                                        {
                                            fldname = dr[behaviour + "Main_" + delimiters12[1].ToString()].ToString();
                                        }
                                        else if (delimiters12[0] == "L")
                                        {
                                            fldname = dr[behaviour + "Item_" + delimiters12[1].ToString()].ToString();
                                        }
                                        else if (delimiters12[0] == "I")
                                        {
                                            // MessageBox.Show(delimiters12[1].ToString());
                                            //     MessageBox.Show(dr["IT_MAST_" + delimiters12[1].ToString()].ToString());
                                            fldname = dr["IT_MAST_" + delimiters12[1].ToString()].ToString();
                                        }
                                   
                                        rl = parts[1].ToString();

                                        sp = Int32.Parse(parts[2].ToString()) - 1;

                                        ep = Int32.Parse(parts[3].ToString());


                                        fldname = fldname.Trim();
                                        if (rl == "R")
                                        {
                                            char[] array = fldname.ToCharArray();
                                            Array.Reverse(array);
                                            fldname = new string(array);

                                        }

                                        int i = fldname.Length - sp;
                                        if (i >= ep)
                                        {
                                            fldname = fldname.Substring(sp, ep);
                                        }

                                        else
                                        {
                                            fldname = fldname.Substring(sp, i);
                                        }
                                      
                                        prn1 = prn1.Replace("@@" + str1 + "@@", fldname);

                                    }
                                }
                            }
                            txtPrn = txtPrn + prn1 + "\n";
                        }

                        string fname = getFileName(txtBarcode.Text.ToString());

                        string filepath = Application.StartupPath + "\\_BAT\\_PRN";

                        using (FileStream stream = new FileStream(filepath + "\\" + fname + ".prn", FileMode.Create))
                        using (TextWriter writer1 = new StreamWriter(stream))
                        {
                            writer1.WriteLine("");
                        }

                        FileStream fs1 = new FileStream(filepath + "\\" + fname + ".prn", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter writer = new StreamWriter(fs1);

                        writer.Write(txtPrn);
                        writer.Close();

                        ////Code genrate batch file
                        string filepathBat = Application.StartupPath;
                        string txtBat = "";

                        //txtBat = txtBat + "set MyPath=%cd%" + Environment.NewLine;
                        //txtBat = txtBat + "echo %MyPath%" + Environment.NewLine;
                        //txtBat = txtBat + "set PCName=%ComputerName%" + Environment.NewLine + Environment.NewLine;
                        //txtBat = txtBat + "FOR /F \"tokens=2* delims==\" %%A in (" + Environment.NewLine;
                        //txtBat = txtBat + "  'wmic printer where \"default=True\" get sharename /value'" + Environment.NewLine;
                        //txtBat = txtBat + "  ) do SET DefaultPrinter=%%A" + Environment.NewLine;
                        //txtBat = txtBat + "set PrinterShareName=%DefaultPrinter%" + Environment.NewLine + Environment.NewLine;
                        //txtBat = txtBat + "Copy  \"%MyPath%\\_PRN\\" + fname + ".prn\" " + "\\" + "\\" + "%PCName%\\"+ printSName + "" + Environment.NewLine;
                        //txtBat = txtBat + "PAUSE";








                        // txtBat = txtBat + "set MyPath=%cd%" + Environment.NewLine;
                        // txtBat = txtBat + "echo %MyPath%" + Environment.NewLine;

                        txtBat = txtBat + "@ECHO OFF" + Environment.NewLine;
                        txtBat = txtBat + "SET MyPath=%~dp0" + Environment.NewLine;
                        txtBat = txtBat + "SET MyPath=%MyPath:~0,-1%" + Environment.NewLine;

                        txtBat = txtBat + "set PCName=%ComputerName%" + Environment.NewLine + Environment.NewLine;
                        txtBat = txtBat + "FOR /F \"tokens=2* delims==\" %%A in (" + Environment.NewLine;
                        txtBat = txtBat + "  'wmic printer where \"default=True\" get sharename /value'" + Environment.NewLine;
                        txtBat = txtBat + "  ) do SET DefaultPrinter=%%A" + Environment.NewLine;
                        txtBat = txtBat + "set PrinterShareName=%DefaultPrinter%" + Environment.NewLine + Environment.NewLine;
                        txtBat = txtBat + "Copy  \"%MyPath%\\_PRN\\" + fname + ".prn\" " + "\\" + "\\" + "%PCName%\\" + printSName + "" + Environment.NewLine;


                        //txtBat = txtBat + "Copy  \"%MyPath%\\_PRN\\" + fname + ".prn\" " + "\\" + "\\" + "%PCName%\\%PrinterShareName%" + Environment.NewLine;
                        FileStream fsBat = new FileStream(filepathBat + "\\_BAT\\" + fname + ".bat", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter writerBat = new StreamWriter(fsBat);

                        writerBat.Write(txtBat);
                        writerBat.Close();


                        ////Code genrate batch file
                        int printCopy = Int32.Parse(txtUpDown.Value.ToString());

                        for (int i = 1; i <= printCopy; i++)
                        {
                            string filepath1 = filepathBat + @"\_BAT\" + fname + ".bat";

                            System.Diagnostics.Process.Start(filepath1);
                        }


                        //if (File.Exists(filepathBat + "\\"+ fname + ".bat"))
                        //{
                        //    File.Delete(filepathBat + "\\"+ fname + ".bat");
                        //}
                    }
                    catch (Exception ee)
                    {
                        // MessageBox.Show(ee.ToString());
                    }

                }
            }


        }
        private string getFileName(string fname)
        {
            string filename;
            string date = DateTime.Now.ToString("dd/MM/yy") + "_" + DateTime.Now.ToString("hh:mm:ss tt");
            filename = fname + date;
            filename = filename.Replace(".", "").Replace("-", "").Replace("/", "").Replace(":", "").Replace("__", "_").Replace(" ", "");
            return filename;
        }
        private void btnBarcode_Click(object sender, EventArgs e)
        {


            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            groupBox2.Controls.Clear();
            dtControls = new DataTable();

            dtControls.Columns.Add("ControlId");
            dtControls.Columns.Add("ControlType");
            dtControls.Columns.Add("VName");
            dtControls.Columns.Add("datatype");
            dtControls.Columns.Add("tblname");
            dtControls.Columns.Add("txtID");
            dtControls.Columns.Add("behaviour");

            int height = 0;
            string ffld = "";
            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select Barcode_Nm as 'BarcodeName',M_T=(case when M_T='M' then 'Master' else 'Transaction' end) ,name as 'Name',tblname,behaviour,ffld,prn from BARCODE_SETTING order by Barcode_Nm desc";

            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select Barcode Name";
            vSearchCol = "BarcodeName";
            vDisplayColumnList = "BarcodeName:Barcode Name,M_T:Master/Transaction,Name:Name";
            vReturnCol = "BarcodeName,M_T,Name,tblname,behaviour,ffld,prn";
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
                btnPrint.Enabled = true;
                txtBarcode.Text = oSelectPop.pReturnArray[0];
                txtMT.Text = oSelectPop.pReturnArray[1];
                txtName.Text = oSelectPop.pReturnArray[2];
                tblname = oSelectPop.pReturnArray[3];
                behaviour = oSelectPop.pReturnArray[4];
                ffld = oSelectPop.pReturnArray[5];
                prn = oSelectPop.pReturnArray[6];

                

                // textBox1.Text = prn;
            }


            ////////////////////////////////////
            // string ffld = "<<entry_ty>><<date>><<party_nm>><<U_PAID_CVD>>";
            string[] delimiters = new string[] { "<<", ">>" };
            parts = ffld.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                groupBox2.Visible = false;
                groupBox2.Height = 0;
                this.Height = 173;

            }
            try
            {

                int pointX = 137;
                int pointY = 15;

                int Lpointx = 10;
                int Lpointy = 15;

                int Bpointx = 459;
                int Bpointy = 14;

                groupBox2.Controls.Clear();
                foreach (string str in parts)
                {
                    string[] var = str.Split('#');
                    string datatype = "", tblname1 = "";
                    FldName = var[1].ToString();
                    string headingName = var[2].ToString();
                    if (var[0].ToString() == "H")
                    {
                        SqlStr = "SELECT DATA_TYPE,TABLE_NAME  FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + behaviour + "MAIN' and COLUMN_NAME='" + var[1].ToString() + "'";
                    }
                    else if (var[0].ToString() == "L")
                    {
                        SqlStr = "SELECT DATA_TYPE,TABLE_NAME  FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + behaviour + "Item' and COLUMN_NAME='" + var[1].ToString() + "'";
                    }
                    else if (var[0].ToString() == "I")
                    {
                        SqlStr = "SELECT DATA_TYPE,TABLE_NAME  FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + tblname + "' and COLUMN_NAME='" + var[1].ToString() + "'";
                    }
                    DataTable dtFld = new DataTable();
                    dtFld = oDataAccess.GetDataTable(SqlStr, null, 20);
                    if (dtFld.Rows.Count > 0)
                    {
                        datatype = dtFld.Rows[0][0].ToString();
                        tblname1 = dtFld.Rows[0][1].ToString();
                    }
                    //SqlStr = "SELECT DATA_TYPE,TABLE_NAME  FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + behaviour + "MAIN' and COLUMN_NAME='" + str + "'";
                    ////SqlStr = SqlStr + "union all SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + behaviour + "MAIN' and COLUMN_NAME='" + str + "'";
                    //DataTable dtmain = new DataTable();
                    //DataTable dtItem = new DataTable();

                    //dtmain = oDataAccess.GetDataTable(SqlStr, null, 20);
                    //if (dtmain.Rows.Count > 0)
                    //{
                    //    datatype = dtmain.Rows[0][0].ToString();
                    //    tblname1 = dtmain.Rows[0][1].ToString();

                    //    //SqlStr = "select distinct "+str+" from "+tblname1 +"where "+ str;

                    //    //DataTable dt = oDataAccess.GetDataTable(SqlStr, null, 20);
                    //    //if (dt.Rows.Count > 0)
                    //    //{

                    //    //}
                    //}
                    //else
                    //{
                    //    SqlStr = "SELECT DATA_TYPE,TABLE_NAME  FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + behaviour + "Item' and COLUMN_NAME='" + str + "'";
                    //    dtItem = oDataAccess.GetDataTable(SqlStr, null, 20);
                    //    if (dtItem.Rows.Count > 0)
                    //    {
                    //        datatype = dtItem.Rows[0][0].ToString();
                    //        tblname1 = dtItem.Rows[0][1].ToString();
                    //    }
                    //}



                    if (datatype != "" && tblname1 != "")
                    {
                        if (datatype == "varchar" || datatype == "int" || datatype == "numeric" || datatype == "decimal" || datatype == "text" || datatype == "char")
                        {

                            Label a1 = new Label();
                            a1.Text = headingName;
                            a1.Width = 70;
                            a1.Location = new Point(Lpointx, Lpointy);
                            groupBox2.Controls.Add(a1);
                            Lpointy += 25;


                            TextBox a = new TextBox();
                            a.Name = "txt" + FldName;
                            a.Enabled = false;
                            //   dtControls.Rows.Add(a.Name, "textbox", str, datatype, tblname1);
                            dtControls.Rows.Add(a.Name, "textbox", FldName, datatype, tblname1, "", behaviour);
                            a.Width = 319;
                            a.Location = new Point(pointX, pointY);
                            groupBox2.Controls.Add(a);
                            pointY += 25;


                            Button a2 = new Button();
                            a2.Name = "btn" + FldName;
                            // dtControls.Rows.Add(a2.Name, "button", str, "", tblname1,a.Name);
                            dtControls.Rows.Add(a2.Name, "button", FldName, "", tblname1, a.Name, behaviour);
                            a2.Width = 24;
                            a2.Height = 20;
                            a2.Location = new Point(Bpointx, Bpointy);
                            // Add a Button Click Event handler
                            a2.Click += new EventHandler(DynamicButton_Click);
                            a2.Image = ((System.Drawing.Image)(resources.GetObject("btnBarcode.Image")));

                            groupBox2.Controls.Add(a2);
                            Bpointy += 25;
                            height += 25;


                        }
                        else if (datatype == "datetime")
                        {

                            Label a1 = new Label();
                            a1.Text = headingName;
                            a1.Width = 70;
                            a1.Location = new Point(Lpointx, Lpointy);
                            groupBox2.Controls.Add(a1);
                            Lpointy += 25;
                            //  height += 25;

                            DateTimePicker a = new DateTimePicker();
                            a.Name = "dtp" + FldName;
                            //dtControls.Rows.Add(a.Name, "datetime", str, datatype, tblname1);
                            dtControls.Rows.Add(a.Name, "datetime", FldName, datatype, tblname1, "", behaviour);
                            a.Format = DateTimePickerFormat.Custom;
                            a.CustomFormat = "dd-MM-yyyy";
                            a.Width = 319;
                            a.Location = new Point(pointX, pointY);
                            groupBox2.Controls.Add(a);
                            pointY += 25;

                            Button a2 = new Button();
                            a2.Width = 24;
                            a2.Height = 20;
                            a2.Location = new Point(Bpointx, Bpointy);
                            groupBox2.Controls.Add(a2);
                            Bpointy += 25;
                            a2.Visible = false;

                            height += 25;
                        }
                        else if (datatype == "bit")
                        {

                            Label a1 = new Label();
                            a1.Text = headingName;
                            a1.Width = 70;
                            a1.Location = new Point(Lpointx, Lpointy);
                            groupBox2.Controls.Add(a1);
                            Lpointy += 25;

                            CheckBox a = new CheckBox();
                            a.Name = "chk" + FldName;
                            // a.Checked = false;
                            //dtControls.Rows.Add(a.Name, "chekbox", str, datatype, tblname1);
                            dtControls.Rows.Add(a.Name, "chekbox", FldName, datatype, tblname1, "", behaviour);
                            a.Location = new Point(pointX, pointY);
                            groupBox2.Controls.Add(a);
                            pointY += 25;

                            Button a2 = new Button();
                            a2.Width = 24;
                            a2.Height = 20;
                            a2.Location = new Point(Bpointx, Bpointy);
                            groupBox2.Controls.Add(a2);
                            Bpointy += 25;
                            a2.Visible = false;

                            height += 25;
                        }

                        groupBox2.Height = height + 20;


                        //Button a2 = new Button();
                        //a2.Width = 24;
                        //a2.Height = 20;
                        //a2.Location = new Point(Bpointx, Bpointy);
                        //groupBox2.Controls.Add(a2);
                        //Bpointy += 25;

                        groupBox2.Show();
                    }


                }



            }
            catch (Exception ez)
            {
                //   MessageBox.Show(ez.ToString());
            }

            this.Height = gbPeriod.Height + groupBox2.Height + 90;
            /////////////////////////////////////
            panel1.Height = this.Height - 38;

        }

        private void button2_Click(object sender, EventArgs e)
        {




        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            mDeleteProcessIdRecord();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {


                if (ddlPrinterName.SelectedItem.ToString() != "")
                {
                    try
                    {


                        string s1 = "";
                        string s = "";
                        int i = 0;
                        string field = "";
                        DataTable dd = dtControls;

                        //if (dd.Rows.Count > 0)
                        //{


                        //var distinctRows = (from DataRow dRow in dd.Rows
                        //                    select dRow["tblname"]).Distinct();

                        //var distinctRows1 = (from DataRow dRow in dd.Rows
                        //                     select dRow["behaviour"]).Distinct();

                        string[] tablename = new string[2];
                        if (behaviour != "" && behaviour != "IM")
                        {
                            tablename[0] = behaviour + "MAIN";
                            tablename[1] = behaviour + "ITEM";
                        }
                        else
                        {
                            tablename[0] = "IT_MAST";

                        }
                        string[] distinctRows1 = new string[1];
                        distinctRows1[0] = behaviour;

                        ////
                        foreach (string str in tablename)
                        {
                            string sQuery1;

                            sQuery1 = "SELECT COLUMN_NAME=('" + str + ".['+COLUMN_NAME+']'),alise=('" + str + "_'+COLUMN_NAME)  FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + str + "'";
                            DataTable dtFld1 = new DataTable();
                            dtFld1 = oDataAccess.GetDataTable(sQuery1, null, 20);
                            foreach (DataRow dr in dtFld1.Rows)
                            {
                                field = field + "," + dr[0].ToString() + " as " + dr[1].ToString();
                            }
                        }

                        field = field.Substring(1);
                        ////


                        string sQuery = " set dateformat dmy select " + field + " from ";
                        int i1 = 0;
                        foreach (string str in tablename)
                        {
                            if (str != null)
                            {
                                if (i1 == 0)
                                {
                                    sQuery = sQuery + str;
                                }
                                else
                                {
                                    sQuery = sQuery + " inner join " + str;
                                }

                                i1++;
                            }

                        }

                        foreach (string str in distinctRows1)
                        {
                            if (str != "" && str != "IM" && i1 > 1)
                            {
                                sQuery = sQuery + " on " + behaviour + "MAIN.entry_ty=" + behaviour + "ITEM.entry_ty and " + behaviour + "MAIN.tran_cd=" + behaviour + "ITEM.Tran_cd ";
                            }

                        }


                        foreach (DataRow dr in dd.Rows)
                        {
                           
                            if (dr[1].ToString() != "button" && dr[1].ToString() != "chekbox")
                            {
                                Control[] ctls = this.Controls.Find(dr[0].ToString(), true);
                                if (ctls[0].Text.ToString() != "")
                                {
                                    if (i == 0 || s == "")
                                    {
                                        if (dr[3].ToString() == "varchar" || dr[3].ToString() == "datetime")
                                        {
                                            s = dr[4].ToString() + "." + dr[2].ToString() + "='" + ctls[0].Text.ToString() + "'";
                                        }
                                        else
                                        {
                                            s = dr[4].ToString() + "." + dr[2].ToString() + "=" + ctls[0].Text.ToString();
                                        }

                                    }
                                    else
                                    {
                                        s = s + " and " + dr[4].ToString() + "." + dr[2].ToString() + "='" + ctls[0].Text.ToString() + "'";
                                    }
                                }

                            }
                            else if (dr[1].ToString() == "chekbox")
                            {
                                Control[] ctls = this.Controls.Find(dr[0].ToString(), true);
                                CheckBox checkBox = ctls[0] as CheckBox;

                                if (i == 0)
                                {
                                    s = dr[4].ToString() + "." + dr[2].ToString() + "=" + checkBox.Checked.ToString();
                                }
                                else
                                {
                                    s = s + " and " + dr[4].ToString() + "." + dr[2].ToString() + "=" + checkBox.Checked.ToString() + "";
                                }
                            }
                            i++;

                        }
                        //Commented by Rupesh G. on 25/03/2019 for Bug no 32392       //Start
                        //if (s.Length > 0)
                        //{
                        //Commented by Rupesh G. on 25/03/2019 for Bug no 32392       //END

                        //Added by Shrikant S. on 29/12/2018 for Installer 2.1.0        //Start
                        //string barquery = "select Barcode_Nm,BC_HD,b.Entry_ty,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm))";
                        //    barquery = barquery + "Inner join BARCODEMAST c on(c.Entry_ty = b.Entry_ty)";

                        //Added by Rupesh G. on 25/03/2019 for Bug no 32392        //Start
                        string bname, M_T, barquery="";
                        bname= txtBarcode.Text.ToString().Trim();
                        M_T = txtMT.Text.ToString().Trim();
                        

                        if (M_T== "Master")
                        {
                            barquery = "select Barcode_Nm, BC_HD, b.Code,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a Inner Join MASTCODE b on(rtrim(a.[name]) = rtrim(b.Name))";
                            barquery = barquery + " Inner join BARCODEMAST c on(c.Entry_ty = b.Code)";
                            barquery = barquery + "where Barcode_Nm = '"+ bname + "'";

                        }
                        else if(M_T == "Transaction")
                        {
                            //Commented by Priyanka B on 05032020 for Bug-33346 Start
                            //barquery = "select Barcode_Nm,BC_HD,b.Entry_ty,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm))";
                            //barquery = barquery + "Inner join BARCODEMAST c on(c.Entry_ty = b.Entry_ty)";
                            //barquery = barquery + "where Barcode_Nm = '" + bname + "'";
                            //Commented by Priyanka B on 05032020 for Bug-33346 End

                            //Modified by Priyanka B on 05032020 for Bug-33346 Start
                            barquery = "select top 1 * from (select Barcode_Nm,BC_HD,b.Entry_ty,c.Tbl_Nm1,c.Fld_Nm1,c.Fld_Nm2 from BARCODE_SETTING a ";
                            barquery = barquery + " Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm)) ";
                            barquery = barquery + " Inner join BARCODEMAST c on(c.Entry_ty = b.Entry_ty) ";
                            barquery = barquery + " where Barcode_Nm = '" + bname + "'";
                            barquery = barquery+ " union ";
                            barquery = barquery + " select Barcode_Nm, BC_HD = 'D', b.Entry_ty,Tbl_Nm1 = (case when b.bcode_nm = '' then b.entry_ty else b.bcode_nm end +'ITEM'),d.Fld_Nm1,d.Fld_Nm2 from BARCODE_SETTING a ";
                            barquery = barquery + " Inner Join LCODE b on(rtrim(a.[name]) = rtrim(b.code_nm)) ";
                            barquery = barquery + " Inner join UOMBCMAST d on(d.Entry_ty = b.Entry_ty) ";
                            barquery = barquery + " where Barcode_Nm = '" + bname + "'";
                            barquery = barquery + ") aa";
                            //Modified by Priyanka B on 05032020 for Bug-33346 End
                        }
                        //Added by Rupesh G. on 25/03/2019 for Bug no 32392       //End

                        sQuery = sQuery.ToUpper();
                        DataTable barTbl = oDataAccess.GetDataTable(barquery, null, 20);
                            if (barTbl.Rows.Count > 0)
                            {
                            switch (barTbl.Rows[0]["BC_HD"].ToString().Trim())
                            {
                                case "H":
                                    //Commented by Rupesh G. on 25/03/2019 for Bug no 32392       //Start
                                    //sQuery = sQuery.Replace(barTbl.Rows[0]["Tbl_Nm1"].ToString().Trim().ToUpper() + ".[" + barTbl.Rows[0]["Fld_Nm1"].ToString().Trim() + "]", "Bar." + barTbl.Rows[0]["Fld_Nm2"].ToString().Trim());
                                    //sQuery = sQuery + " inner join BarcodeTran Bar on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd )";
                                    //Commented by Rupesh G. on 25/03/2019 for Bug no 32392       //END

                                    //Added by Rupesh G. on 25/03/2019 for Bug no 32392        //Start
                                    if (M_T == "Master")
                                    {
                                        string strQuery, iden_flds, tblName;
                                        strQuery = "select iden_flds,[FileName] from mastcode where code='" + behaviour + "'";
                                        DataTable dtRec = oDataAccess.GetDataTable(strQuery, null, 20);

                                        iden_flds = dtRec.Rows[0]["iden_flds"].ToString().Trim();
                                        tblName = dtRec.Rows[0]["FileName"].ToString().Trim();

                                        sQuery = sQuery.Replace(barTbl.Rows[0]["Tbl_Nm1"].ToString().Trim().ToUpper() + ".[" + barTbl.Rows[0]["Fld_Nm1"].ToString().Trim().ToUpper() + "]", "Bar." + barTbl.Rows[0]["Fld_Nm2"].ToString().Trim());
                                        sQuery = sQuery + " inner join BarcodeTran Bar on (Bar.Entry_ty= '" + behaviour + "' and Bar.tran_cd= " + tblName + "." + iden_flds + " )";

                                    }
                                    else if (M_T == "Transaction")
                                    {
                                        sQuery = sQuery.Replace(barTbl.Rows[0]["Tbl_Nm1"].ToString().Trim().ToUpper() + ".[" + barTbl.Rows[0]["Fld_Nm1"].ToString().Trim().ToUpper() + "]", "Bar." + barTbl.Rows[0]["Fld_Nm2"].ToString().Trim());
                                        sQuery = sQuery + " inner join BarcodeTran Bar on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd )";

                                    }
                                    //Added by Rupesh G. on 25/03/2019 for Bug no 32392        //END

                                    break;
                                case "D":
                                    //Commented by Priyanka B on 05032020 for Bug-33346 Start
                                    //sQuery = sQuery.Replace(barTbl.Rows[0]["Tbl_Nm1"].ToString().Trim().ToUpper() + ".[" + barTbl.Rows[0]["Fld_Nm1"].ToString().Trim().ToUpper() + "]", "Bar." + barTbl.Rows[0]["Fld_Nm2"].ToString().Trim());
                                    //sQuery = sQuery + " inner join BarcodeTran Bar on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd and Bar.Itserial= " + behaviour + "Item.Itserial)";
                                    //Commented by Priyanka B on 05032020 for Bug-33346 End

                                    //Modified by Priyanka B on 05032020 for Bug-33346 Start
                                    string sq = sQuery.Replace(barTbl.Rows[0]["Tbl_Nm1"].ToString().Trim().ToUpper() + ".[" + barTbl.Rows[0]["Fld_Nm1"].ToString().Trim().ToUpper() + "]", "Bar." + barTbl.Rows[0]["Fld_Nm2"].ToString().Trim());
                                    sQuery = sQuery.Replace(barTbl.Rows[0]["Tbl_Nm1"].ToString().Trim().ToUpper() + ".[" + barTbl.Rows[0]["Fld_Nm1"].ToString().Trim().ToUpper() + "]", "Bar." + barTbl.Rows[0]["Fld_Nm2"].ToString().Trim());
                                    sQuery = sQuery + " inner join BarcodeTran Bar on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd and Bar.Itserial= " + behaviour + "Item.Itserial)";
                                    sQuery = sQuery + " Union all ";
                                    sQuery = sQuery + sq.Replace("SET DATEFORMAT DMY", " ");
                                    sQuery = sQuery + " inner join BarcodeDet Bar on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd and Bar.Itserial= " + behaviour + "Item.Itserial)";
                                    //Modified by Priyanka B on 05032020 for Bug-33346 End
                                    break;
                                case "S":
                                    //sQuery = sQuery + " inner join BarcodeTran on (Bar.Entry_ty= " + behaviour + "Main.Entry_ty and Bar.tran_cd= " + behaviour + "Main.Tran_cd and Bar.Itserial= " + behaviour + "Item.Itserial )";
                                    break;
                                default:
                                    break;
                            }
                            }
                        //Added by Shrikant S. on 29/12/2018 for Installer 2.1.0        //End

                        //Added by Rupesh G. on 25/03/2019 for Bug no  32392      //Start

                        if (s.Length > 0)
                        {
                            //Added by Rupesh G. on 25/03/2019 for Bug no  32392      //END
                            sQuery = sQuery + " where " + s;
                        }
                       
                        dtRecord = new DataTable();
                        dtRecord = oDataAccess.GetDataTable(sQuery, null, 20);
                        //   }


                        ////////////////***********************************
                        genratePRN();
                        ///////////////************************************

                    }
                    catch (Exception ee)
                    {
                        //   MessageBox.Show("" + ee.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Select Printer name!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

            }
            catch (Exception ee)
            {
                MessageBox.Show("Select Printer name!", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnPrint.Enabled)
                toolStripButton1_Click(this.btnPrint, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnLogout, e);
        }

        private void DynamicButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string buttonId = button.Name;
            DataTable dt = dtControls;

            DataRow[] result = dt.Select("ControlId= '" + buttonId + "'");

            string fldnm = result[0][2].ToString();
            string tblname = result[0][4].ToString();
            string txtid = result[0][5].ToString();


            string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
            DataSet tDs = new DataSet();
            SqlStr = "select distinct " + fldnm + " from " + tblname;
            // SqlStr = "select entry_ty  from lcode";
            tDs = oDataAccess.GetDataSet(SqlStr, null, 20);
            DataView dvw = tDs.Tables[0].DefaultView;
            VForText = "Select " + fldnm;
            vSearchCol = fldnm;
            vDisplayColumnList = fldnm + ":" + fldnm;
            vReturnCol = fldnm;
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


                Control[] ctls = this.Controls.Find(txtid, true);
                ctls[0].Text = oSelectPop.pReturnArray[0];

            }


        }
        private void mInsertProcessIdRecord()
        {
            DataSet dsData = new DataSet();
            string sqlstr;
            int pi;
            pi = Process.GetCurrentProcess().Id;
            cAppName = "udBarcodePrinting.exe";
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
        private void button1_Click(object sender, EventArgs e)
        {


            //int n = 4;
            //TextBox[] textBoxes = new TextBox[n];
            //Label[] labels = new Label[n];

            //for (int i = 0; i < n; i++)
            //{
            //    textBoxes[i] = new TextBox();
            //    // Here you can modify the value of the textbox which is at textBoxes[i]

            //    labels[i] = new Label();
            //    labels[i].Text = "Label 1";
            //    labels[i].Location= 10;
            //    // Here you can modify the value of the label which is at labels[i]
            //}

            //// This adds the controls to the form (you will need to specify thier co-ordinates etc. first)
            //for (int i = 0; i < n; i++)
            //{
            //   // panel1.Controls.Add(textBoxes[i]);
            //    panel1.Controls.Add(labels[i]);
            //}
        }
    }
}
