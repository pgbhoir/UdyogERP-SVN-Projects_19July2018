using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ueconnect;

namespace udAttendanceIntegrationHourwise
{
    public partial class frmMain : uBaseForm.FrmBaseForm
    {
        DataAccess_Net.clsDataAccess oDataAccess;
       // DataTable vTblMain = new DataTable();
        string SqlStr = string.Empty;
      //  string vMainField = "state", vTblMainNm = "state", vMainFldVal = "";
        string vExpression = string.Empty;
        String cAppPId, cAppName;
        Boolean cValid = true;
        Boolean Iscancel = false;

        clsConnect oConnect;
        string startupPath = string.Empty;
        string ErrorMsg = string.Empty;
        string ServiceType = string.Empty;

        DataTable dtGridColumn = new DataTable();
        DataTable dtMain = new DataTable();
        DataTable dtEmpRec = new DataTable();

        DataSet tDs = new DataSet();
        string VForText = string.Empty, vSearchCol = string.Empty, strSQL = string.Empty, Vstr = string.Empty, vColExclude = string.Empty, vDisplayColumnList = string.Empty, vReturnCol = string.Empty;
        string vMonth = string.Empty, vYear = string.Empty, vFileName = string.Empty, vTran_Cd = String.Empty;


        string filePath = string.Empty;
        string fileExt = string.Empty;
        string employeeCode = "";
        private void btnEmpName_Click(object sender, EventArgs e)
        {

            SqlStr = "select EmployeeCode,EmployeeName from employeemast union all select '',''";

            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Employee";
            vSearchCol = "EmployeeName";
            vDisplayColumnList = "EmployeeName:Employee Name";
            vReturnCol = "EmployeeCode,EmployeeName";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                employeeCode = oSelectPop.pReturnArray[0].ToString().Trim();
                this.txtEmpName.Text = oSelectPop.pReturnArray[1].ToString().Trim();
            }
            filter();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGetExcel_Click(object sender, EventArgs e)
        {
            DataTable dtExcelRec;
            dtExcelRec = ReadExcel(filePath, fileExt);

            ////////////////
            if (dtMain.Rows.Count > 0)
            {

                if (dtExcelRec.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMain.Rows)
                    {
                        DataRow[] drmain = dtExcelRec.Select("Date='" + dr["Date"] + "' and EmployeeCode='" + dr["EmployeeCode"] + "'");
                        if (drmain.Length > 0)
                        {
                            dr["InTime"] = drmain[0]["InTime"].ToString();
                            dr["OutTime"] = drmain[0]["OutTime"].ToString();
                            dr["TotalHour"] = drmain[0]["TotalHour"].ToString();
                            dr["StandardWorkingHour"] = drmain[0]["StandardWorkingHour"].ToString();
                            dr["TotalWorkedHour"] = drmain[0]["TotalWorkedHour"].ToString();
                            dr["OverTime"] = drmain[0]["OverTime"].ToString();
                            dr["BreakTime"] = drmain[0]["BreakTime"].ToString();
                        }

                    }
                }
            }
            // dtMain = dtExcelRec;
            ///////////////////
            ColumnBindToGrid();
        }
        public void filterRecAdd()
        {
            dtMain.Columns.Add("Location");
            dtMain.Columns.Add("Department");
            dtMain.Columns.Add("Designation");
            dtMain.Columns.Add("Category");
            dtMain.Columns.Add("Grade");
            //dtMain.Columns.Add("sel");
            SqlStr = "select e.EmployeeCode,l.Loc_desc,e.Department,e.Designation,e.Category,e.Grade from employeemast e  inner join loc_Master l on e.Loc_Code=l.Loc_Code";
            DataTable dtRecEmp = oDataAccess.GetDataTable(SqlStr, null, 25);
            if (dtRecEmp.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dtRecEmp.Rows)
                {
                    foreach (DataRow dr in dtMain.Rows)
                    {
                        if (dr["EmployeeCode"].ToString().Trim() == dr1["EmployeeCode"].ToString().Trim())
                        {
                            //   dr["sel"] = false;
                            dr["Location"] = dr1["Loc_desc"].ToString();
                            dr["Department"] = dr1["Department"].ToString();
                            dr["Designation"] = dr1["Designation"].ToString();
                            dr["Category"] = dr1["Category"].ToString();
                            dr["Grade"] = dr1["Grade"].ToString();
                        }
                    }
                }

            }
        }
        private void sbExcelFile_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog file = new OpenFileDialog();
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = file.FileName;
                    fileExt = Path.GetExtension(filePath);
                    if (fileExt.CompareTo(".xls") == 0)
                    {
                        txtExcelFile.Text = filePath;
                        try
                        {
                            DataTable dtExcel = new DataTable();
                            dtExcel = ReadExcel(filePath, fileExt);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        MessageBox.Show("Please choose .xls  file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else

                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Sheet1$]", con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch (Exception ee)
                {

                    MessageBox.Show("" + ee.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dtexcel;
        }
        public string getFileName(string fname)
        {
            string DfilePath, filename, FilePath = "";
            string date = DateTime.Now.ToString("dd/MM/yy") + "_" + DateTime.Now.ToString("hh:mm:ss tt");

            FolderBrowserDialog vFolderBrowserDialog1 = new FolderBrowserDialog();
            if (vFolderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DfilePath = vFolderBrowserDialog1.SelectedPath;

                filename = fname + date;
                filename = filename.Replace(".", "").Replace("-", "").Replace("/", "").Replace(":", "").Replace("__", "_").Replace(" ", "");
                //  FilePath = DfilePath + filename+".xls";
                FilePath = DfilePath + filename;
                FilePath = FilePath.Replace("\\\\", "\\");
            }

            return FilePath;
        }
        public void ExportToExcel(DataTable dt_record, string excelFilename)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

                Microsoft.Office.Interop.Excel.Workbook wBook;


                Microsoft.Office.Interop.Excel.Worksheet wSheet;


                wBook = excel.Workbooks.Add(System.Reflection.Missing.Value);


                wSheet = (Microsoft.Office.Interop.Excel.Worksheet)wBook.ActiveSheet;


                System.Data.DataTable dt = dt_record;
                System.Data.DataColumn dc = new DataColumn();


                int colIndex = 0;
                float rowIndex = 1;


                foreach (DataColumn dcol in dt.Columns)
                {
                    colIndex = colIndex + 1;
                    excel.Cells[rowIndex, colIndex] = dcol.ColumnName;
                }


                rowIndex += 1;
                float nrow = 1;
                //  excel.Columns.NumberFormat = "@";




                foreach (DataRow drow in dt.Rows)
                {
                    colIndex = 0;
                    foreach (DataColumn dcol in dt.Columns)
                    {
                        colIndex = colIndex + 1;
                        excel.Cells[rowIndex, colIndex] = drow[dcol.ColumnName].ToString();

                    }
                    float aa = nrow == dt.Rows.Count ? 100.00f : (nrow / dt.Rows.Count) * 100;

                    nrow = nrow + 1;
                    rowIndex = rowIndex + 1;
                }




                object misValue = System.Reflection.Missing.Value;
                excel.Columns.AutoFit();

                excel.ActiveWorkbook.SaveAs(excelFilename + ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                excel.ActiveWorkbook.Saved = true;



                excel.ActiveWorkbook.Close();


                //MessageBox.Show("Your excel file exported successfully at " + excelFilename + ".xls", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                MessageBox.Show("Excel Application not available", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            }
        }
        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (txtProcYear.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Please select Processing Year");
                return;
            }
            else if (txtProcMonth.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Please select Processing Month");
                return;
            }
            DataTable dtRecord = new DataTable();
            dtRecord.Columns.Add("SrNo");
            dtRecord.Columns.Add("Date");
            dtRecord.Columns.Add("EmployeeCode");
            dtRecord.Columns.Add("EmployeeName");
            dtRecord.Columns.Add("InTime", typeof(int));
            dtRecord.Columns["InTime"].DefaultValue = 0;
            dtRecord.Columns.Add("OutTime", typeof(int));
            dtRecord.Columns["OutTime"].DefaultValue = 0;
            dtRecord.Columns.Add("TotalHour", typeof(int));
            dtRecord.Columns["TotalHour"].DefaultValue = 0;
            dtRecord.Columns.Add("StandardWorkingHour", typeof(int));
            dtRecord.Columns["StandardWorkingHour"].DefaultValue = 0;
            dtRecord.Columns.Add("TotalWorkedHour", typeof(int));
            dtRecord.Columns["TotalWorkedHour"].DefaultValue = 0;
            dtRecord.Columns.Add("OverTime", typeof(int));
            dtRecord.Columns["OverTime"].DefaultValue = 0;
            dtRecord.Columns.Add("BreakTime", typeof(int));
            dtRecord.Columns["BreakTime"].DefaultValue = 0;

            SqlStr = "select EmployeeCode,EmployeeName from employeemast order by EmployeeCode";
            DataTable dtEmpRec = oDataAccess.GetDataTable(SqlStr, null, 25);

            DateTime now = DateTime.Now;
            //DateTime firstDay = new DateTime(Int32.Parse(txtProcYear.Text.ToString().Trim()), Int32.Parse(txtProcMonth.ToString().Trim()), 1);
            int month = DateTime.ParseExact(txtProcMonth.Text.ToString().Trim(), "MMMM", new CultureInfo("en-US")).Month;
            DateTime firstDay = new DateTime(Int32.Parse(txtProcYear.Text.ToString().Trim()), month, 1);
            int days = DateTime.DaysInMonth(Int32.Parse(txtProcYear.Text.ToString().Trim()), month);
            for (int i = 1; i <= days; i++)
            {
                int srno = 1;
                foreach (DataRow dr in dtEmpRec.Rows)
                {
                    dtRecord.Rows.Add(srno, firstDay.ToString("dd/MM/yyyy"), dr["EmployeeCode"].ToString().Trim(), dr["EmployeeName"].ToString().Trim());
                    srno++;
                }
                firstDay = firstDay.AddDays(1);
            }

            if (dtRecord != null)
            {
                string file_path = getFileName("\\Attendance_Format_" + txtProcMonth.Text.ToString().Trim() + "" + txtProcYear.Text.ToString().Trim() + "_");
                if (file_path != "")
                {

                    ExportToExcel(dtRecord, file_path);
                    MessageBox.Show("Default format download successfully at " + file_path + ".xls", this.pPApplName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }

            }
        }

        //private void button8_Click(object sender, EventArgs e)
        //{
        //    filter();
        //}
        private void filter()
        {
            if (dtMain.Rows.Count > 0)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = dgvAttendanceGrid.DataSource;

                bs.Filter = "[EmployeeName] LIKE '%" + txtEmpName.Text.ToString().Trim() + "%' and [EmployeeCode] LIKE '%" + employeeCode.ToString().Trim() + "%' and Location LIKE '%" + txtLocation.Text.ToString().Trim() + "%'and Department LIKE '%" + txtDepartment.Text.ToString().Trim() + "%'and Designation LIKE '%" + txtDesignation.Text.ToString().Trim() + "%'and Category LIKE '%" + txtCategory.Text.ToString().Trim() + "%'and Grade LIKE '%" + txtGrade.Text.ToString().Trim() + "%' and (Date >='" + dtpfrom.Text.ToString() + "' and Date<='" + dtpto.Text.ToString() + "')";

                dgvAttendanceGrid.DataSource = bs;
            }
            setGridColour();
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            SqlStr = "select distinct l.Loc_desc from employeemast e  inner join loc_Master l on e.Loc_Code=l.Loc_Code union all select ''";

            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Location";
            vSearchCol = "Loc_desc";
            vDisplayColumnList = "Loc_desc:Location";
            vReturnCol = "Loc_desc";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {

                this.txtLocation.Text = oSelectPop.pReturnArray[0];
            }
            filter();
          
        }

        private void btnDesignation_Click(object sender, EventArgs e)
        {
            SqlStr = "select distinct Designation from employeemast where designation<>'' union all select ''";

            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Designation";
            vSearchCol = "Designation";
            vDisplayColumnList = "Designation:Designation";
            vReturnCol = "Designation";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {

                this.txtDesignation.Text = oSelectPop.pReturnArray[0];
            }
            filter();
        }

        private void btnGrade_Click(object sender, EventArgs e)
        {
            SqlStr = "select distinct Grade from employeemast where Grade<>'' union all select ''";

            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Grade";
            vSearchCol = "Grade";
            vDisplayColumnList = "Grade:Grade";
            vReturnCol = "Grade";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {

                this.txtGrade.Text = oSelectPop.pReturnArray[0];
            }
            filter();
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            SqlStr = "select distinct Department from employeemast where Department<>'' union all select ''";

            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Department";
            vSearchCol = "Department";
            vDisplayColumnList = "Department:Department";
            vReturnCol = "Department";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {

                this.txtDepartment.Text = oSelectPop.pReturnArray[0];
            }
            filter();
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            SqlStr = "select distinct Category from employeemast where Category<>'' union all select ''";

            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Category";
            vSearchCol = "Category";
            vDisplayColumnList = "Category:Category";
            vReturnCol = "Category";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {

                this.txtCategory.Text = oSelectPop.pReturnArray[0];
            }
            filter();
        }

        private void btnAddRec_Click(object sender, EventArgs e)
        {
            if (txtProcYear.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Please select Processing Year");
                return;
            }
            else if (txtProcMonth.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Please select Processing Month");
                return;
            }
            DataTable dtRecord = new DataTable();
            dtRecord.Columns.Add("sel", typeof(bool));
            dtRecord.Columns.Add("SrNo");
            dtRecord.Columns.Add("Date");
            dtRecord.Columns.Add("EmployeeCode");
            dtRecord.Columns.Add("EmployeeName");
            dtRecord.Columns.Add("InTime", typeof(decimal));
            dtRecord.Columns["InTime"].DefaultValue = 0;
            dtRecord.Columns.Add("OutTime", typeof(decimal));
            dtRecord.Columns["OutTime"].DefaultValue = 0;
            dtRecord.Columns.Add("TotalHour", typeof(decimal));
            dtRecord.Columns["TotalHour"].DefaultValue = 0;
            dtRecord.Columns.Add("StandardWorkingHour", typeof(decimal));
            dtRecord.Columns["StandardWorkingHour"].DefaultValue = 0;
            dtRecord.Columns.Add("TotalWorkedHour", typeof(decimal));
            dtRecord.Columns["TotalWorkedHour"].DefaultValue = 0;
            dtRecord.Columns.Add("OverTime", typeof(decimal));
            dtRecord.Columns["OverTime"].DefaultValue = 0;
            dtRecord.Columns.Add("BreakTime", typeof(decimal));
            dtRecord.Columns["BreakTime"].DefaultValue = 0;

            //SqlStr = "select EmployeeCode,EmployeeName from employeemast";
            SqlStr = " select e.EmployeeCode,e.EmployeeName,l.Loc_desc,e.Department,e.Designation,e.Category,e.Grade from employeemast e  inner join loc_Master l on e.Loc_Code=l.Loc_Code order by e.EmployeeCode";
            dtEmpRec = oDataAccess.GetDataTable(SqlStr, null, 25);

            if (dtEmpRec.Rows.Count > 0)
            {

                string filtcond = "";
                if (txtLocation.Text != "")
                {
                    if (filtcond == "")
                    {
                        filtcond = filtcond + " Loc_desc like '%" + txtLocation.Text.ToString().Trim() + "%'";
                    }
                    else
                    {
                        filtcond = filtcond + " and Loc_desc like '%" + txtLocation.Text.ToString().Trim() + "%'";
                    }

                }
                if (txtDepartment.Text != "")
                {
                    if (filtcond == "")
                    {
                        filtcond = filtcond + "  Department like '%" + txtDepartment.Text.ToString().Trim() + "%'";
                    }
                    else
                    {
                        filtcond = filtcond + " and Department like '%" + txtDepartment.Text.ToString().Trim() + "%'";
                    }
                }
                if (txtDesignation.Text != "")
                {
                    if (filtcond == "")
                    {
                        filtcond = filtcond + "  Designation like '%" + txtDesignation.Text.ToString().Trim() + "%'";
                    }
                    else
                    {
                        filtcond = filtcond + " and Designation like '%" + txtDesignation.Text.ToString().Trim() + "%'";
                    }
                }
                if (txtGrade.Text != "")
                {
                    if (filtcond == "")
                    {
                        filtcond = filtcond + "  Grade like '%" + txtDesignation.Text.ToString().Trim() + "%'";
                    }
                    else
                    {
                        filtcond = filtcond + " and Grade like '%" + txtGrade.Text.ToString().Trim() + "%'";
                    }
                }
                if (txtEmpName.Text != "")
                {
                    if (filtcond == "")
                    {
                        filtcond = filtcond + "  EmployeeName like '%" + txtDesignation.Text.ToString().Trim() + "%'";
                    }
                    else
                    {
                        filtcond = filtcond + " and EmployeeName like '%" + txtEmpName.Text.ToString().Trim() + "%'";
                    }
                }
                if (txtCategory.Text != "")
                {
                    if (filtcond == "")
                    {
                        filtcond = filtcond + "  Category like '%" + txtDesignation.Text.ToString().Trim() + "%'";
                    }
                    else
                    {
                        filtcond = filtcond + " and Category like '%" + txtCategory.Text.ToString().Trim() + "%'";
                    }
                }


                //DataRow [] drdtEmpRec = dtEmpRec.Select("Loc_desc like '%" + txtLocation.Text.ToString().Trim() + "%' or Department like '%" + txtDepartment.Text.ToString().Trim()+ "%' or Designation like '%" + txtDesignation.Text.ToString().Trim() + "%' or Category like '%" + txtCategory.Text.ToString().Trim() + "%' or Grade like '%" + txtGrade.Text.ToString().Trim() + "%'");
                DataRow[] drdtEmpRec = dtEmpRec.Select(filtcond);
                if (drdtEmpRec.Length > 0)
                {
                    dtEmpRec = drdtEmpRec.CopyToDataTable();



                    DateTime now = DateTime.Now;
                    //DateTime firstDay = new DateTime(Int32.Parse(txtProcYear.Text.ToString().Trim()), Int32.Parse(txtProcMonth.ToString().Trim()), 1);
                    int month = DateTime.ParseExact(txtProcMonth.Text.ToString().Trim(), "MMMM", new CultureInfo("en-US")).Month;
                    DateTime firstDay = new DateTime(Int32.Parse(txtProcYear.Text.ToString().Trim()), month, 1);
                    int days = DateTime.DaysInMonth(Int32.Parse(txtProcYear.Text.ToString().Trim()), month);
                    for (int i = 1; i <= days; i++)
                    {
                        int srno = 1;
                        foreach (DataRow dr in dtEmpRec.Rows)
                        {
                            dtRecord.Rows.Add(false, srno, firstDay.ToString("dd/MM/yyyy"), dr["EmployeeCode"].ToString().Trim(), dr["EmployeeName"].ToString().Trim());
                            srno++;
                        }
                        firstDay = firstDay.AddDays(1);
                    }
                    ////////////////
                    if (dtMain.Rows.Count > 0)
                    {

                        if (dtRecord.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtRecord.Rows)
                            {
                                DataRow[] drmain = dtMain.Select("Date='" + dr["Date"] + "' and EmployeeCode='" + dr["EmployeeCode"] + "'");
                                if (drmain.Length > 0)
                                {
                                    dr["InTime"] = drmain[0]["InTime"].ToString();
                                    dr["OutTime"] = drmain[0]["OutTime"].ToString();
                                    dr["TotalHour"] = drmain[0]["TotalHour"].ToString();
                                    dr["StandardWorkingHour"] = drmain[0]["StandardWorkingHour"].ToString();
                                    dr["TotalWorkedHour"] = drmain[0]["TotalWorkedHour"].ToString();
                                    dr["OverTime"] = drmain[0]["OverTime"].ToString();
                                    dr["BreakTime"] = drmain[0]["BreakTime"].ToString();
                                }

                            }
                        }
                    }
                    ///////////////////
                    dtMain = dtRecord;
                    filterRecAdd();

                    ColumnBindToGrid();
                }
            }
            else
            {
                MessageBox.Show("No record found in Employee Master.");
            }
        }

        private void dtpfrom_ValueChanged(object sender, EventArgs e)
        {
            filter();
        }

        private void dtpto_ValueChanged(object sender, EventArgs e)
        {
            filter();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dtMain.Rows.Count > 0)
            {
                this.pEditMode = true;
                mthControlEnable();

                chkSelectAll.Enabled = false;
                dgvAttendanceGrid.Columns["colSel"].ReadOnly = true;
                dgvAttendanceGrid.Columns["colEmpIn"].ReadOnly = false;
                dgvAttendanceGrid.Columns["colEmpOut"].ReadOnly = false;
                dgvAttendanceGrid.Columns["colEmpTotHr"].ReadOnly = false;
                dgvAttendanceGrid.Columns["colEmpSWT"].ReadOnly = false;
                dgvAttendanceGrid.Columns["colTWH"].ReadOnly = false;
                dgvAttendanceGrid.Columns["colOT"].ReadOnly = false;
                dgvAttendanceGrid.Columns["colBT"].ReadOnly = false;

            }
            else
            {
                MessageBox.Show("Record not found to edit record.\n Add record and Proceed...");
            }


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            chkSelectAll.Enabled = true;
            this.pEditMode = false;
            this.pEditMode = false;
            mthControlSet();
            mthControlEnable();

            setGridRec();
            setTableRec();
            filterRecAdd();
            ColumnBindToGrid();

        }

        private void btnProcYear_Click(object sender, EventArgs e)
        {
            SqlStr = "select distinct Pay_Year from emp_Processing_month where isclosed!=1"; // Changed By Amrendra on 03-07-2012

            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Year";
            vSearchCol = "Pay_Year";
            vDisplayColumnList = "Pay_Year:Year";
            vReturnCol = "Pay_Year";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {
                this.txtProcYear.Text = oSelectPop.pReturnArray[0];
                if (txtProcYear.Text != "" && txtProcMonth.Text != "")
                {
                    int month = DateTime.ParseExact(txtProcMonth.Text.ToString().Trim(), "MMMM", new CultureInfo("en-US")).Month;
                    DateTime firstDay = new DateTime(Int32.Parse(txtProcYear.Text.ToString().Trim()), month, 1);
                    DateTime lastDay = new DateTime(Int32.Parse(txtProcYear.Text.ToString().Trim()), month, 1).AddMonths(1).AddDays(-1);
                    dtpfrom.Text = firstDay.ToString();
                    dtpto.Text = lastDay.ToString();
                    //dtpfrom.MinDate = firstDay;
                    //dtpto.MaxDate = lastDay;
                    setTableRec();
                    filterRecAdd();
                    ColumnBindToGrid();
                }
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            chkSelectAll.Enabled = true;
            this.pEditMode = false;
            dtMain.AcceptChanges();
            string vfiledlist = string.Empty;
            string vfiledvalue = string.Empty;
            vfiledlist = "[Upload],[sel],[SrNo],[Date],[EmployeeCode],[EmployeeName],[InTime],[OutTime],[TotalHour],[StandardWorkingHour],[TotalWorkedHour],[OverTime],[BreakTime],[Pay_Year],[Pay_Month]";


            foreach (DataRow dr in dtMain.Rows)
            {
                bool sel;
                int srno;
                decimal InTime, OutTime, TotalHour, StandardWorkingHour, TotalWorkedHour, OverTime, BreakTime;
                string Date, EmployeeCode, EmployeeName;
                sel = Boolean.Parse(dr["sel"].ToString());
                srno = Int32.Parse(dr["SrNo"].ToString());
                Date = DateTime.Parse(dr["Date"].ToString()).ToString("dd/MM/yyyy");
                EmployeeCode = dr["EmployeeCode"].ToString().Trim();
                EmployeeName = dr["EmployeeName"].ToString().Trim();
                InTime = decimal.Parse(dr["InTime"].ToString().Trim());
                OutTime = decimal.Parse(dr["OutTime"].ToString().Trim());
                TotalHour = decimal.Parse(dr["TotalHour"].ToString().Trim());
                StandardWorkingHour = decimal.Parse(dr["StandardWorkingHour"].ToString().Trim());
                TotalWorkedHour = decimal.Parse(dr["TotalWorkedHour"].ToString().Trim());
                OverTime = decimal.Parse(dr["OverTime"].ToString().Trim());
                BreakTime = decimal.Parse(dr["BreakTime"].ToString().Trim());


                try
                {
                    SqlStr = "select * from Emp_Daily_Muster_Hourwise where Pay_Year='" + txtProcYear.Text.ToString().Trim() + "' and Pay_Month='" + txtProcMonth.Text.ToString().Trim() + "' and EmployeeCode='" + EmployeeCode + "' and [Date]= '" + Date + "'";
                    DataTable dtrec = new DataTable();

                    dtrec = oDataAccess.GetDataTable(SqlStr, null, vTimeOut);

                    if (dtrec.Rows.Count > 0)
                    {
                        SqlStr = "set dateformat dmy update Emp_Daily_Muster_Hourwise set [upload]='0',[SrNo]=" + srno + ",[EmployeeName]='" + EmployeeName + "',[InTime]=" + InTime + ",[OutTime]=" + OutTime + ",[TotalHour]=" + TotalHour + ",[StandardWorkingHour]=" + StandardWorkingHour + ",[TotalWorkedHour]='" + TotalWorkedHour + "',[OverTime]='" + OverTime + "',[BreakTime]='" + BreakTime + "' where Pay_Year='" + txtProcYear.Text.ToString().Trim() + "' and Pay_Month='" + txtProcMonth.Text.ToString().Trim() + "' and EmployeeCode='" + EmployeeCode + "' and [Date]= '" + Date + "'";
                        //SqlStr = "set dateformat dmy update Emp_Daily_Muster_Hourwise set [upload]='0',[sel]='" + sel + "',[SrNo]=" + srno + ",[EmployeeName]='" + EmployeeName + "',[InTime]=" + InTime + ",[OutTime]=" + OutTime + ",[TotalHour]=" + TotalHour + ",[StandardWorkingHour]=" + StandardWorkingHour + ",[TotalWorkedHour]='" + TotalWorkedHour + "',[OverTime]='" + OverTime + "',[BreakTime]='" + BreakTime + "' where Pay_Year='" + txtProcYear.Text.ToString().Trim() + "' and Pay_Month='" + txtProcMonth.Text.ToString().Trim() + "' and EmployeeCode='" + EmployeeCode + "' and [Date]= '" + Date + "'";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                    }
                    else
                    {
                        SqlStr = "set dateformat dmy insert into Emp_Daily_Muster_Hourwise ([upload],[sel],[SrNo],[Date],[EmployeeCode],[EmployeeName],[InTime],[OutTime],[TotalHour],[StandardWorkingHour],[TotalWorkedHour],[OverTime],[BreakTime],[Pay_Year],[Pay_Month])";
                        SqlStr = SqlStr + " values('0','" + sel + "'," + srno + ",'" + Date + "','" + EmployeeCode + "','" + EmployeeName + "'," + InTime + "," + OutTime + "," + TotalHour + "," + StandardWorkingHour + ",'" + TotalWorkedHour + "','" + OverTime + "','" + BreakTime + "','" + txtProcYear.Text.ToString().Trim() + "','" + txtProcMonth.Text.ToString().Trim() + "')";
                        oDataAccess.ExecuteSQLStatement(SqlStr, null, 20, true);
                    }


                }
                catch (Exception eee)
                {

                }
            }


            setTableRec();
            filterRecAdd();
            ColumnBindToGrid();
            this.mthControlSet();
            this.mthControlEnable();

        }

        short vTimeOut = 25;

        private void btnProcess_Click(object sender, EventArgs e)
        {

            int month = DateTime.ParseExact(txtProcMonth.Text.ToString().Trim(), "MMMM", new CultureInfo("en-US")).Month;
            int year = Int32.Parse(txtProcYear.Text.ToString().Trim());

            DataRow[] drSelectRec = dtMain.Select("sel='True'");
            if (drSelectRec.Length == 0)
            {
                MessageBox.Show("Select atleast one record to Process.");
                return;
            }
            else
            {
                DataTable dtProcessRec = new DataTable();
                dtProcessRec = drSelectRec.CopyToDataTable();

                DataView view = new DataView(dtProcessRec);
                DataTable dtEmployeeCode = new DataTable();
                dtEmployeeCode = view.ToTable(true, "Employeecode");

                foreach (DataRow dr in dtEmployeeCode.Rows)
                {
                    DataRow[] drProcessRec = dtProcessRec.Select("Employeecode='" + dr["Employeecode"] + "'");
                    SqlStr = "select * from Emp_Daily_Muster where upload='False' and Employeecode='" + dr["EmployeeCode"].ToString().Trim() + "' and Pay_Year='" + year + "' and Pay_Month='" + month + "'";
                    DataTable dtRecEmp = oDataAccess.GetDataTable(SqlStr, null, 25);
                    if (dtRecEmp.Rows.Count > 0)
                    {
                        decimal OTH=0,Wh=0;
                        foreach (DataRow dr1 in drProcessRec)
                        {
                            decimal InTime = decimal.Parse(dr1["InTime"].ToString().Trim());
                            int days = DateTime.Parse(dr1["Date"].ToString()).Day;
                            var ColumnName = "Day" + days;
                            if (InTime > 0)
                            {
                                dtRecEmp.Rows[0][ColumnName] = "PR";
                            }
                            else
                            {
                                dtRecEmp.Rows[0][ColumnName] = "AB";
                            }

                            OTH = OTH + decimal.Parse(dr1["OverTime"].ToString().Trim());
                            Wh = Wh + decimal.Parse(dr1["TotalWorkedHour"].ToString().Trim());
                        }

                        string decimal_places = "00";
                        var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                        if (regex.IsMatch(OTH.ToString()))
                        {
                            decimal_places = regex.Match(OTH.ToString()).Value;
                        }                        
                        int hour = decimal.ToInt32(OTH) + Int32.Parse(decimal_places) / 60;
                        int min = Int32.Parse(decimal_places) % 60;                        
                        dtRecEmp.Rows[0]["OverTime"] = hour+"."+ min.ToString("00");

                        string decimal_places1 = "00";
                        var regex1 = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                        if (regex1.IsMatch(Wh.ToString()))
                        {
                            decimal_places1 = regex.Match(Wh.ToString()).Value;
                        }
                        int hour1 = decimal.ToInt32(Wh) + Int32.Parse(decimal_places1) / 60;
                        int min1 = Int32.Parse(decimal_places1) % 60;
                        dtRecEmp.Rows[0]["WorkedHour"] = hour1 + "." + min1.ToString("00");


                        string vSaveString = string.Empty;
                        this.mSaveCommandString(ref vSaveString, "#ID#", dtRecEmp);
                        oDataAccess.ExecuteSQLStatement(vSaveString, null, 20, true);

                    }

                }
                setTableRec();
                filterRecAdd();
                ColumnBindToGrid();
                this.mthControlSet();
                this.mthControlEnable();
                MessageBox.Show("Day to Day muster updated...", this.pPApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);

            }



        }
        private void mSaveCommandString(ref string vSaveString, string vkeyField,DataTable vTblMain)
        {
            string vTblMainNm = "Emp_Daily_Muster";
            string vfldList = string.Empty;
            string vfldValList = string.Empty;
            string vIdentityFields = string.Empty, vfldVal = string.Empty, vDataType = string.Empty;

            /*Identity Columns--->*/
            DataSet dsData = new DataSet();
            string sqlstr = "select c.name as ColName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 ";
            sqlstr = sqlstr + " and o.name='" + vTblMainNm + " ' ";
            dsData = oDataAccess.GetDataSet(sqlstr, null, 20);
            foreach (DataRow dr1 in dsData.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(vIdentityFields) == false) { vIdentityFields = vIdentityFields + ","; }
                vIdentityFields = vIdentityFields + "#" + dr1["ColName"].ToString().Trim() + "#";
            }
            
                vSaveString = "Set DateFormat dmy Update " + vTblMainNm + "  Set ";
                string vWhereCondn = string.Empty;
                foreach (DataColumn dtc1 in vTblMain.Columns)
                {

                    {
                        vfldVal = vTblMain.Rows[0][dtc1.ToString().Trim()].ToString().Trim();
                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        if (dtc1.DataType.Name.ToLower() == "datetime")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        if (dtc1.DataType.Name.ToLower() == "boolean")
                        {
                            if (vfldVal.ToLower() == "true")
                            {
                                vfldVal = "1";
                            }
                            else
                            {
                                vfldVal = "0";
                            }

                        }
                    }
                    if ((vkeyField.ToLower().IndexOf("#" + dtc1.ToString().Trim().ToLower() + "#") > -1))
                    {
                        if (string.IsNullOrEmpty(vWhereCondn) == false) { vWhereCondn = vWhereCondn + " and "; } else { vWhereCondn = " Where "; }
                        vWhereCondn = vWhereCondn + dtc1.ToString().Trim() + " = ";
                        vfldVal = vTblMain.Rows[0][dtc1.ToString().Trim()].ToString().Trim();
                        if (dtc1.DataType.Name.ToLower() == "string")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        if (dtc1.DataType.Name.ToLower() == "datetime")
                        {
                            vfldVal = "'" + vfldVal + "'";
                        }
                        if (dtc1.DataType.Name.ToLower() == "boolean")
                        {
                            if (vfldVal.ToLower() == "true")
                            {
                                vfldVal = "1";
                            }
                            else
                            {
                                vfldVal = "0";
                            }

                        }
                        vWhereCondn = vWhereCondn + vfldVal;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(vfldList) == false) { vfldList = vfldList + ","; }
                        vfldList = vfldList + dtc1.ToString().Trim() + " = ";
                        vfldList = vfldList + vfldVal;
                    }
                }
                vSaveString = vSaveString + vfldList + vWhereCondn;
            
        }

        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(string[] args)
        {
            this.pDisableCloseBtn = true;
            InitializeComponent();
            this.pFrmCaption = "Attendance Integration Hourwise";
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

        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if(chkSelectAll.Checked)
            {
                foreach(DataRow dr in dtMain.Rows)
                {
                    dr["sel"] = true;
                }
            }
            else
            {
                foreach (DataRow dr in dtMain.Rows)
                {
                    dr["sel"] = false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
            CultureInfo ci = new CultureInfo("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
         
            this.btnHelp.Enabled = false;
            this.btnCalculator.Enabled = false;
            this.btnExit.Enabled = false;
        
            txtMuster.Text = "Monthly";
            DataAccess_Net.clsDataAccess._databaseName = this.pComDbnm;
            DataAccess_Net.clsDataAccess._serverName = this.pServerName;
            DataAccess_Net.clsDataAccess._userID = this.pUserId;
            DataAccess_Net.clsDataAccess._password = this.pPassword;
            oDataAccess = new DataAccess_Net.clsDataAccess();
            this.SetMenuRights();
          
            startupPath = Application.StartupPath;
            //  oConnect = new clsConnect();
            // GetInfo.iniFile ini = new GetInfo.iniFile(startupPath + "\\" + "Visudyog.ini");
            //  string appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
            //   oConnect.InitProc("'" + startupPath + "'", appfile);
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
          //  MessageBox.Show("6");
            //if (registerMePath == string.Empty)
            //{
            //    ServiceType = "";
            //}
            //else
            //{
            //    string[] objRegisterMe = (oConnect.ReadRegiValue(startupPath)).Split('^');
            //    ServiceType = objRegisterMe[15].ToString().Trim();
            //}
          //  MessageBox.Show("7");
            //   this.btnLast_Click(sender, e);


            this.mInsertProcessIdRecord();
            this.SetFormColor();


            string appPath = Application.ExecutablePath;

            appPath = Path.GetDirectoryName(appPath);
            if (string.IsNullOrEmpty(this.pAppPath))
            {
                this.pAppPath = appPath;
            }
            this.mthControlSet();
            this.mthControlEnable();

            setGridRec();
            setTableRec();
            filterRecAdd();
            ColumnBindToGrid();
            dgvAttendanceGrid.Height = this.Height - 280;
            dgvAttendanceGrid.Width = this.Width - 20;
        
        }

      

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Enabled)
                btnSave_Click(this.btnSave, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
                btnEdit_Click(this.btnEdit, e);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Enabled)
                btnCancel_Click(this.btnCancel, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.btnLogout.Enabled)
                btnLogout_Click(this.btnExit, e);
        }

        public void setTableRec()
        {
            SqlStr = "select sel,SrNo,Date,EmployeeCode,EmployeeName,InTime,OutTime,TotalHour,StandardWorkingHour,TotalWorkedHour,OverTime,BreakTime from Emp_Daily_Muster_Hourwise where  pay_year='" + txtProcYear.Text.ToString().Trim() + "' and Pay_Month='" + txtProcMonth.Text.ToString().Trim() + "' order by date";
            dtMain = oDataAccess.GetDataTable(SqlStr, null, 20);

        }
        private void mthControlEnable()
        {
            if (!pEditMode)
            {
                btnSave.Enabled = false;
                groupBox1.Enabled = false;
                //  groupBox3.Enabled = false;
                groupBox2.Enabled = true;
                btnEdit.Enabled = true;
            }
            else
            {
                btnSave.Enabled = true;
                groupBox1.Enabled = true;
                // groupBox3.Enabled = true;
                groupBox2.Enabled = false;
                btnEdit.Enabled = false;
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

            cAppName = "udAttendanceIntegrationHourwise";
            cAppPId = Convert.ToString(Process.GetCurrentProcess().Id);
            sqlstr = "Set DateFormat dmy insert into vudyog..ExtApplLog (pApplCode,CallDate,pApplNm,pApplId,pApplDesc,cApplNm,cApplId,cApplDesc) Values('" + this.pPApplCode + "','" + DateTime.Now.Date.ToString() + "','" + this.pPApplName + "'," + this.pPApplPID + ",'" + this.pPApplText + "','" + cAppName + "'," + cAppPId + ",'" + this.Text.Trim() + "')";
            oDataAccess.ExecuteSQLStatement(sqlstr, null, 20, true);
        }

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

        private void btnMonth_Click(object sender, EventArgs e)
        {
            SqlStr = "select DateName( Month , DateAdd( month , cast(Pay_Month as int), 0 )-1  ) as [Month] from emp_Processing_month where isclosed!=1";

            tDs = oDataAccess.GetDataSet(SqlStr, null, vTimeOut);

            DataView dvw = tDs.Tables[0].DefaultView;

            VForText = "Select Month";
            vSearchCol = "Month";
            vDisplayColumnList = "Month:Month";
            vReturnCol = "Month";
            udSelectPop.SELECTPOPUP oSelectPop = new udSelectPop.SELECTPOPUP();
            oSelectPop.pdataview = dvw;
            oSelectPop.pformtext = VForText;
            oSelectPop.psearchcol = vSearchCol;

            oSelectPop.pDisplayColumnList = vDisplayColumnList;
            oSelectPop.pRetcolList = vReturnCol;
            oSelectPop.ShowDialog();
            if (oSelectPop.pReturnArray != null)
            {

                this.txtProcMonth.Text = oSelectPop.pReturnArray[0];

                if (txtProcYear.Text != "" && txtProcMonth.Text != "")
                {
                    int month = DateTime.ParseExact(txtProcMonth.Text.ToString().Trim(), "MMMM", new CultureInfo("en-US")).Month;
                    DateTime firstDay = new DateTime(Int32.Parse(txtProcYear.Text.ToString().Trim()), month, 1);
                    DateTime lastDay = new DateTime(Int32.Parse(txtProcYear.Text.ToString().Trim()), month, 1).AddMonths(1).AddDays(-1);
                    dtpfrom.Text = firstDay.ToString();
                    dtpto.Text = lastDay.ToString();
                    //dtpfrom.MinDate = firstDay;
                    //dtpto.MaxDate = lastDay;

                    setTableRec();
                    filterRecAdd();
                    ColumnBindToGrid();
                }

            }

        }

        public void mthControlSet()
        {
            string fName = this.pAppPath + @"\bmp\pickup.gif";
            if (File.Exists(fName) == true)
            {

                this.btnMonth.Image = Image.FromFile(fName);
                this.btnProcYear.Image = Image.FromFile(fName);
                this.sbExcelFile.Image = Image.FromFile(fName);
                this.btnEmpName.Image = Image.FromFile(fName);
                this.btnDepartment.Image = Image.FromFile(fName);
                this.btnDesignation.Image = Image.FromFile(fName);
                this.btnCategory.Image = Image.FromFile(fName);
                this.btnGrade.Image = Image.FromFile(fName);
                this.btnLocation.Image = Image.FromFile(fName);
            }
            SqlStr = "select DateName( Month , DateAdd( month , cast(Pay_Month as int), 0 )-1  ) as [Month] from emp_Processing_month where isclosed!=1";

            DataTable dtRec = oDataAccess.GetDataTable(SqlStr, null, 25);
            if (dtRec.Rows.Count > 0)
            {
                txtProcMonth.Text = dtRec.Rows[0][0].ToString().Trim();

            }
            SqlStr = "select distinct Pay_Year from emp_Processing_month where isclosed!=1"; // Changed By Amrendra on 03-07-2012

            DataTable dtRec1 = oDataAccess.GetDataTable(SqlStr, null, 25);
            if (dtRec1.Rows.Count > 0)
            {
                txtProcYear.Text = dtRec1.Rows[0][0].ToString().Trim();
            }
            if (txtProcYear.Text != "" && txtProcMonth.Text != "")
            {
                int month = DateTime.ParseExact(txtProcMonth.Text.ToString().Trim(), "MMMM", new CultureInfo("en-US")).Month;
                DateTime firstDay = new DateTime(Int32.Parse(txtProcYear.Text.ToString().Trim()), month, 1);
                DateTime lastDay = new DateTime(Int32.Parse(txtProcYear.Text.ToString().Trim()), month, 1).AddMonths(1).AddDays(-1);
                dtpfrom.Text = firstDay.ToString();
                dtpto.Text = lastDay.ToString();

                //dtpfrom.MinDate = firstDay;
                //dtpto.MaxDate = lastDay;
            }


        }
        private void setGridRec()
        {
            dtGridColumn = new DataTable();
            dtGridColumn.Columns.Add("colHeader", typeof(string));
            dtGridColumn.Columns.Add("colName", typeof(string));
            dtGridColumn.Columns.Add("colProperty", typeof(string));
            dtGridColumn.Columns.Add("colReadonly", typeof(bool));
            dtGridColumn.Columns.Add("colVisible", typeof(bool));
            dtGridColumn.Columns.Add("colWidth", typeof(int));
            dtGridColumn.Columns.Add("colType", typeof(string));
            dtGridColumn.Columns.Add("serial", typeof(int));

            dtGridColumn.Rows.Add("", "colSel", "sel", 0, 1, "50", "Checkbox", 1);
            dtGridColumn.Rows.Add("Sr No", "colsrno", "SrNo", 1, 0, "80", "TextBox", 2);
            dtGridColumn.Rows.Add("Date", "colDate", "Date", 1, 1, "90", "Date", 3);
            dtGridColumn.Rows.Add("Employee Code", "colEmpCode", "EmployeeCode", 1, 1, "130", "TextBox", 4);
            dtGridColumn.Rows.Add("Employee Name", "colEmpName", "EmployeeName", 1, 1, "130", "TextBox", 5);
            dtGridColumn.Rows.Add("In Time", "colEmpIn", "InTime", 1, 1, "70", "number", 6);
            dtGridColumn.Rows.Add("Out Time", "colEmpOut", "OutTime", 1, 1, "100", "number", 7);
            dtGridColumn.Rows.Add("Total Hour", "colEmpTotHr", "TotalHour", 1, 1, "100", "number", 8);
            dtGridColumn.Rows.Add("Standard Working Hour", "colEmpSWT", "StandardWorkingHour", 1, 1, "150", "number", 9);
            dtGridColumn.Rows.Add("Total Worked Hour", "colTWH", "TotalWorkedHour", 1, 1, "150", "number", 10);
            dtGridColumn.Rows.Add("Over Time", "colOT", "OverTime", 1, 1, "100", "number", 11);
            dtGridColumn.Rows.Add("Break Time", "colBT", "BreakTime", 1, 1, "100", "number", 12);
        }
        private void setGridColour()
        {

            if (dtMain.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in dgvAttendanceGrid.Rows)
                {
                    if ((decimal.Parse(dr.Cells["colEmpIn"].Value.ToString().Trim()) > 0) || (decimal.Parse(dr.Cells["colEmpOut"].Value.ToString().Trim()) > 0) || (decimal.Parse(dr.Cells["colEmpTotHr"].Value.ToString().Trim()) > 0) || (decimal.Parse(dr.Cells["colTWH"].Value.ToString().Trim()) > 0) || (decimal.Parse(dr.Cells["colOT"].Value.ToString().Trim()) > 0) || (decimal.Parse(dr.Cells["colBT"].Value.ToString().Trim()) > 0) || (decimal.Parse(dr.Cells["colEmpSWT"].Value.ToString().Trim()) > 0))
                    {
                        dr.DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                    if (decimal.Parse(dr.Cells["colEmpIn"].Value.ToString().Trim()) > 0)
                    {
                        dr.Cells["colEmpIn"].Style.BackColor = Color.LightPink;
                    }
                    if (decimal.Parse(dr.Cells["colEmpOut"].Value.ToString().Trim()) > 0)
                    {
                        dr.Cells["colEmpOut"].Style.BackColor = Color.LightPink;
                    }
                    if (decimal.Parse(dr.Cells["colEmpTotHr"].Value.ToString().Trim()) > 0)
                    {
                        dr.Cells["colEmpTotHr"].Style.BackColor = Color.LightPink;
                    }

                    if (decimal.Parse(dr.Cells["colEmpSWT"].Value.ToString().Trim()) > 0)
                    {
                        dr.Cells["colEmpSWT"].Style.BackColor = Color.LightPink;
                    }
                    if (decimal.Parse(dr.Cells["colTWH"].Value.ToString().Trim()) > 0)
                    {
                        dr.Cells["colTWH"].Style.BackColor = Color.LightPink;
                    }
                    if (decimal.Parse(dr.Cells["colOT"].Value.ToString().Trim()) > 0)
                    {
                        dr.Cells["colOT"].Style.BackColor = Color.LightPink;
                    }
                    if (decimal.Parse(dr.Cells["colBT"].Value.ToString().Trim()) > 0)
                    {
                        dr.Cells["colBT"].Style.BackColor = Color.LightPink;
                    }
                }
            }
        }
        private void ColumnBindToGrid()
        {
            //    filterRecAdd();
            dgvAttendanceGrid.Columns.Clear();
            dgvAttendanceGrid.RowTemplate.Height = 18;
            dgvAttendanceGrid.AutoResizeColumns();
            dgvAttendanceGrid.Refresh();
            dgvAttendanceGrid.AllowUserToAddRows = false;
            dgvAttendanceGrid.AutoGenerateColumns = false;
            dgvAttendanceGrid.DataSource = dtMain;
            if (dtGridColumn.Rows.Count > 0)
            {
                foreach (DataRow dr in dtGridColumn.Rows)
                {
                    if (dr["colType"].ToString().ToLower() == "checkbox")
                    {

                        DataGridViewCheckBoxColumn check = new DataGridViewCheckBoxColumn();
                        check.HeaderText = dr["colHeader"].ToString().Trim();
                        check.Name = dr["colName"].ToString().Trim();
                        check.DataPropertyName = dr["colProperty"].ToString().Trim();
                        check.Width = Convert.ToInt32(dr["colWidth"].ToString().Trim());
                        check.ReadOnly = Convert.ToBoolean(dr["colReadonly"].ToString().Trim());
                        check.Visible = Convert.ToBoolean(dr["colVisible"].ToString().Trim());
                        dgvAttendanceGrid.Columns.Add(check);

                    }
                    else if (dr["colType"].ToString().ToLower() == "textbox")
                    {
                        DataGridViewTextBoxColumn check = new DataGridViewTextBoxColumn();
                        check.HeaderText = dr["colHeader"].ToString().Trim();
                        check.Name = dr["colName"].ToString().Trim();
                        check.DataPropertyName = dr["colProperty"].ToString().Trim();
                        check.Width = Convert.ToInt32(dr["colWidth"].ToString().Trim());
                        check.ReadOnly = Convert.ToBoolean(dr["colReadonly"].ToString().Trim());
                        check.Visible = Convert.ToBoolean(dr["colVisible"].ToString().Trim());
                        dgvAttendanceGrid.Columns.Add(check);
                    }
                    else if (dr["colType"].ToString().ToLower() == "date")
                    {

                        udclsDGVDateTimePicker.MicrosoftDateTimePicker check = new udclsDGVDateTimePicker.MicrosoftDateTimePicker();
                        check.HeaderText = dr["colHeader"].ToString().Trim();
                        check.Name = dr["colName"].ToString().Trim();
                        check.DataPropertyName = dr["colProperty"].ToString().Trim();
                        check.Width = Convert.ToInt32(dr["colWidth"].ToString().Trim());
                        check.ReadOnly = Convert.ToBoolean(dr["colReadonly"].ToString().Trim());
                        check.Visible = Convert.ToBoolean(dr["colVisible"].ToString().Trim());
                        dgvAttendanceGrid.Columns.Add(check);

                    }
                    else if (dr["colType"].ToString().ToLower() == "number")
                    {

                        udclsDGVNumericColumn.CNumEditDataGridViewColumn check = new udclsDGVNumericColumn.CNumEditDataGridViewColumn();
                        check.HeaderText = dr["colHeader"].ToString().Trim();
                        check.Name = dr["colName"].ToString().Trim();
                        check.DataPropertyName = dr["colProperty"].ToString().Trim();
                        check.Width = Convert.ToInt32(dr["colWidth"].ToString().Trim());
                        check.ReadOnly = Convert.ToBoolean(dr["colReadonly"].ToString().Trim());
                        check.Visible = Convert.ToBoolean(dr["colVisible"].ToString().Trim());
                        check.DecimalLength = 2;
                        dgvAttendanceGrid.Columns.Add(check);

                    }


                }
            }


            //  dgvAttendanceGrid.EnableHeadersVisualStyles = false;
            dgvAttendanceGrid.RowsDefaultCellStyle.Font = new Font("Arial", 8.0F);
            setGridColour();
        }
    }
}
