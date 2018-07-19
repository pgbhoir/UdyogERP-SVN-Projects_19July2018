using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

using CustModAccUI.DAL;

namespace CustModAccUI.BLL
{
    public class cls_Gen_Mgr_Read_XML:cls_Gen_Ent_Read_XML
    {
        //string sqlstr = "";

        #region Properties
        private DataSet dsRead;

        public DataSet DsRead
        {
            get { return dsRead; }
            set { dsRead = value; }
        }
        //Added by Priyanka on 14012014 start
        private string warMsg;
        public string WarMsg
        {
            get { return warMsg; }
            set { warMsg = value; }
        }
        //Added by Priyanka on 14012014 end
        #endregion

        #region Public Methods
        public void Readxml()
        {
            DataSet m_DsRead = new DataSet();
            try
            {
                if (File.Exists(Xmlpath.ToString()))
                {
                    m_DsRead.ReadXml(Xmlpath.ToString());
                    DataRow DrRead = m_DsRead.Tables[0].Rows[0];
                    DsRead = m_DsRead;
                    Binding(DrRead);
                    DBFill(DrRead);
                }
                else
                {
                    throw new Exception("File does not exists");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string ReadSQLFile(string sqlfile)
        {
            string sqlstr = string.Empty, line = string.Empty;
            string[] strfn = new string[3];
            WarMsg = "";
            try
            {
                FileInfo file = new FileInfo(sqlfile);
                string sqlscript = file.OpenText().ReadToEnd();
                bool cmenu_contains = Regex.Match(sqlscript, "COM_MENU", RegexOptions.IgnoreCase).Success;
                bool lcode_contains = Regex.Match(sqlscript, "LCODE", RegexOptions.IgnoreCase).Success;
                bool rpt_contains = Regex.Match(sqlscript, "R_STATUS", RegexOptions.IgnoreCase).Success;
                if (cmenu_contains == true)
                {
                    //sqlstr = ReplaceString(sqlscript, "INSERT INTO COM_MENU", "INSERT INTO #CUST_COM_MENU", StringComparison.OrdinalIgnoreCase); //Commented by Priyanka on 14012014
                    //Added by Priyanka on 14012014 start
                    if (sqlscript.Trim().ToUpper().Contains("INSERT INTO COM_MENU"))
                        sqlstr = ReplaceString(sqlscript, "INSERT INTO COM_MENU", "INSERT INTO #CUSTCOMMENU", StringComparison.OrdinalIgnoreCase);
                    else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO [COM_MENU]"))
                        sqlstr = ReplaceString(sqlscript, "INSERT INTO [COM_MENU]", "INSERT INTO #CUSTCOMMENU", StringComparison.OrdinalIgnoreCase);
                    else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO [DBO].[COM_MENU]"))
                        sqlstr = ReplaceString(sqlscript, "INSERT INTO [DBO].[COM_MENU]", "INSERT INTO #CUSTCOMMENU", StringComparison.OrdinalIgnoreCase);
                    else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO DBO.COM_MENU"))
                        sqlstr = ReplaceString(sqlscript, "INSERT INTO DBO.COM_MENU", "INSERT INTO #CUSTCOMMENU", StringComparison.OrdinalIgnoreCase);
                    else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO [DBO].COM_MENU"))
                        sqlstr = ReplaceString(sqlscript, "INSERT INTO [DBO].COM_MENU", "INSERT INTO #CUSTCOMMENU", StringComparison.OrdinalIgnoreCase);
                    else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO DBO.[COM_MENU]"))
                        sqlstr = ReplaceString(sqlscript, "INSERT INTO DBO.[COM_MENU]", "INSERT INTO #CUSTCOMMENU", StringComparison.OrdinalIgnoreCase);

                    strfn[0] = Path.GetTempPath() + Convert.ToString(DateTime.Now.ToFileTime()) + ".txt";
                    File.WriteAllText(strfn[0], sqlstr);
                    using (StreamReader strdr = new StreamReader(strfn[0]))
                    {
                        while ((line = strdr.ReadLine()) != null)
                        {
                            //line = strdr.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                if (!line.ToUpper().Contains("SELECT") && !line.ToUpper().Contains("--"))
                                {
                                    if (line.ToUpper().Contains("COM_MENU"))
                                    {
                                        WarMsg = !string.IsNullOrEmpty(WarMsg)
                                            ? WarMsg + "Check the script whether the Insert format is different from below:\n"
                                            + "'INSERT INTO COM_MENU'\n'INSERT INTO [COM_MENU]'\n'INSERT INTO [DBO].[COM_MENU]'\n'INSERT INTO DBO.COM_MENU'\n"
                                            + "'INSERT INTO [DBO].COM_MENU'\n'INSERT INTO DBO.[COM_MENU]'\n\n"
                                            + "Please correct it and then upload the script."
                                            : "Check the script whether the Insert format is different from below:\n"
                                            + "'INSERT INTO COM_MENU'\n'INSERT INTO [COM_MENU]'\n'INSERT INTO [DBO].[COM_MENU]'\n'INSERT INTO DBO.COM_MENU'\n"
                                            + "'INSERT INTO [DBO].COM_MENU'\n'INSERT INTO DBO.[COM_MENU]'\n\n"
                                            + "Please correct it and then upload the script.";
                                        strdr.Close();
                                        return sqlstr;
                                    }
                                }
                            }
                        }
                    }
                    //Added by Priyanka on 14012014 end
                }
                if (lcode_contains == true)
                {
                    if (string.IsNullOrEmpty(sqlstr))
                    {
                        //sqlstr = ReplaceString(sqlscript, "INSERT INTO LCODE", "INSERT INTO #CUST_LCODE", StringComparison.OrdinalIgnoreCase); //Commented by Priyanka on 14012014
                        //Added by Priyanka on 14012014 start
                        if (sqlscript.Trim().ToUpper().Contains("INSERT INTO LCODE"))
                            sqlstr = ReplaceString(sqlscript, "INSERT INTO LCODE", "INSERT INTO #CUSTTRANCODE", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO [LCODE]"))
                            sqlstr = ReplaceString(sqlscript, "INSERT INTO [LCODE]", "INSERT INTO #CUSTTRANCODE", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO [DBO].[LCODE]"))
                            sqlstr = ReplaceString(sqlscript, "INSERT INTO [DBO].[LCODE]", "INSERT INTO #CUSTTRANCODE", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO DBO.LCODE"))
                            sqlstr = ReplaceString(sqlscript, "INSERT INTO DBO.LCODE", "INSERT INTO #CUSTTRANCODE", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO [DBO].LCODE"))
                            sqlstr = ReplaceString(sqlscript, "INSERT INTO [DBO].LCODE", "INSERT INTO #CUSTTRANCODE", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO DBO.[LCODE]"))
                            sqlstr = ReplaceString(sqlscript, "INSERT INTO DBO.[LCODE]", "INSERT INTO #CUSTTRANCODE", StringComparison.OrdinalIgnoreCase);
                        //Added by Priyanka on 14012014 end
                    }
                    else
                    {
                        //sqlstr = ReplaceString(sqlstr, "INSERT INTO LCODE", "INSERT INTO #CUST_LCODE", StringComparison.OrdinalIgnoreCase); //Commented by Priyanka on 14012014
                        //Added by Priyanka on 14012014 start
                        if (sqlstr.Trim().ToUpper().Contains("INSERT INTO LCODE"))
                            sqlstr = ReplaceString(sqlstr, "INSERT INTO LCODE", "INSERT INTO #CUSTTRANCODE", StringComparison.OrdinalIgnoreCase);
                        else if (sqlstr.Trim().ToUpper().Contains("INSERT INTO [LCODE]"))
                            sqlstr = ReplaceString(sqlstr, "INSERT INTO [LCODE]", "INSERT INTO #CUSTTRANCODE", StringComparison.OrdinalIgnoreCase);
                        else if (sqlstr.Trim().ToUpper().Contains("INSERT INTO [DBO].[LCODE]"))
                            sqlstr = ReplaceString(sqlstr, "INSERT INTO [DBO].[LCODE]", "INSERT INTO #CUSTTRANCODE", StringComparison.OrdinalIgnoreCase);
                        else if (sqlstr.Trim().ToUpper().Contains("INSERT INTO DBO.LCODE"))
                            sqlstr = ReplaceString(sqlstr, "INSERT INTO DBO.LCODE", "INSERT INTO #CUSTTRANCODE", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO [DBO].LCODE"))
                            sqlstr = ReplaceString(sqlstr, "INSERT INTO [DBO].LCODE", "INSERT INTO #CUSTTRANCODE", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO DBO.[LCODE]"))
                            sqlstr = ReplaceString(sqlstr, "INSERT INTO DBO.[LCODE]", "INSERT INTO #CUSTTRANCODE", StringComparison.OrdinalIgnoreCase);
                        //Added by Priyanka on 14012014 end
                    }
                    //Added by Priyanka on 14012014 start
                    strfn[1] = Path.GetTempPath() + Convert.ToString(DateTime.Now.ToFileTime()) + ".txt";
                    File.WriteAllText(strfn[1], sqlstr);
                    using (StreamReader strdr = new StreamReader(strfn[1]))
                    {
                        while ((line = strdr.ReadLine()) != null)
                        {
                            //line = strdr.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                if (!line.ToUpper().Contains("SELECT") && !line.ToUpper().Contains("--"))
                                {
                                    if (line.ToUpper().Contains("LCODE"))
                                    {
                                        WarMsg = !string.IsNullOrEmpty(WarMsg)
                                            ? WarMsg + "Check the script whether the Insert format is different from below:\n"
                                            + "'INSERT INTO LCODE'\n'INSERT INTO [LCODE]'\n'INSERT INTO [DBO].[LCODE]'\n'INSERT INTO DBO.LCODE'\n"
                                            + "'INSERT INTO [DBO].LCODE'\n'INSERT INTO DBO.[LCODE]'\n\n"
                                            + "Please correct it and then upload the script."
                                            : "Check the script whether the Insert format is different from below:\n"
                                            + "'INSERT INTO LCODE'\n'INSERT INTO [LCODE]'\n'INSERT INTO [DBO].[LCODE]'\n'INSERT INTO DBO.LCODE'\n"
                                            + "'INSERT INTO [DBO].LCODE'\n'INSERT INTO DBO.[LCODE]'\n\n"
                                            + "Please correct it and then upload the script.";
                                        strdr.Close();
                                        return sqlstr;
                                    }
                                }
                            }
                        }
                    }
                    //Added by Priyanka on 14012014 end
                }
                if (rpt_contains == true)
                {
                    if (string.IsNullOrEmpty(sqlstr))
                    {
                        //sqlstr = ReplaceString(sqlscript, "INSERT INTO R_STATUS", "INSERT INTO #CUST_R_STATUS", StringComparison.OrdinalIgnoreCase); //Commented by Priyanka on 14012014
                        //Added by Priyanka on 14012014 start
                        if (sqlscript.Trim().ToUpper().Contains("INSERT INTO R_STATUS"))
                            sqlstr = ReplaceString(sqlscript, "INSERT INTO R_STATUS", "INSERT INTO #CUSTRSTATUS", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO [R_STATUS]"))
                            sqlstr = ReplaceString(sqlscript, "INSERT INTO [R_STATUS]", "INSERT INTO #CUSTRSTATUS", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO [DBO].[R_STATUS]"))
                            sqlstr = ReplaceString(sqlscript, "INSERT INTO [DBO].[R_STATUS]", "INSERT INTO #CUSTRSTATUS", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO DBO.R_STATUS"))
                            sqlstr = ReplaceString(sqlscript, "INSERT INTO DBO.R_STATUS", "INSERT INTO #CUSTRSTATUS", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO [DBO].R_STATUS"))
                            sqlstr = ReplaceString(sqlscript, "INSERT INTO [DBO].R_STATUS", "INSERT INTO #CUSTRSTATUS", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO DBO.[R_STATUS]"))
                            sqlstr = ReplaceString(sqlscript, "INSERT INTO DBO.[R_STATUS]", "INSERT INTO #CUSTRSTATUS", StringComparison.OrdinalIgnoreCase);
                        //Added by Priyanka on 14012014 end
                    }
                    else
                    {
                        //sqlstr = ReplaceString(sqlstr, "INSERT INTO R_STATUS", "INSERT INTO #CUST_R_STATUS", StringComparison.OrdinalIgnoreCase); //Commented by Priyanka on 14012014
                        //Added by Priyanka on 14012014 start
                        if (sqlstr.Trim().ToUpper().Contains("INSERT INTO R_STATUS"))
                            sqlstr = ReplaceString(sqlstr, "INSERT INTO R_STATUS", "INSERT INTO #CUSTRSTATUS", StringComparison.OrdinalIgnoreCase);
                        else if (sqlstr.Trim().ToUpper().Contains("INSERT INTO [R_STATUS]"))
                            sqlstr = ReplaceString(sqlstr, "INSERT INTO [R_STATUS]", "INSERT INTO #CUSTRSTATUS", StringComparison.OrdinalIgnoreCase);
                        else if (sqlstr.Trim().ToUpper().Contains("INSERT INTO [DBO].[R_STATUS]"))
                            sqlstr = ReplaceString(sqlstr, "INSERT INTO [DBO].[R_STATUS]", "INSERT INTO #CUSTRSTATUS", StringComparison.OrdinalIgnoreCase);
                        else if (sqlstr.Trim().ToUpper().Contains("INSERT INTO DBO.R_STATUS"))
                            sqlstr = ReplaceString(sqlstr, "INSERT INTO DBO.R_STATUS", "INSERT INTO #CUSTRSTATUS", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO [DBO].R_STATUS"))
                            sqlstr = ReplaceString(sqlstr, "INSERT INTO [DBO].R_STATUS", "INSERT INTO #CUSTRSTATUS", StringComparison.OrdinalIgnoreCase);
                        else if (sqlscript.Trim().ToUpper().Contains("INSERT INTO DBO.[R_STATUS]"))
                            sqlstr = ReplaceString(sqlstr, "INSERT INTO DBO.[R_STATUS]", "INSERT INTO #CUSTRSTATUS", StringComparison.OrdinalIgnoreCase);
                        //Added by Priyanka on 14012014 end
                    }
                    //Added by Priyanka on 14012014 start
                    strfn[2] = Path.GetTempPath() + Convert.ToString(DateTime.Now.ToFileTime()) + ".txt";
                    File.WriteAllText(strfn[2], sqlstr);
                    using (StreamReader strdr = new StreamReader(strfn[2]))
                    {
                        while ((line = strdr.ReadLine()) != null)
                        {
                            //line = strdr.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                if (!line.ToUpper().Contains("SELECT") && !line.ToUpper().Contains("--"))
                                {
                                    if (line.ToUpper().Contains("R_STATUS"))
                                    {
                                        WarMsg = !string.IsNullOrEmpty(WarMsg)
                                            ? WarMsg + "Check the script whether the Insert format is different from below:\n"
                                            + "'INSERT INTO R_STATUS'\n'INSERT INTO [R_STATUS]'\n'INSERT INTO [DBO].[R_STATUS]'\n'INSERT INTO DBO.R_STATUS'\n"
                                            + "'INSERT INTO [DBO].R_STATUS'\n'INSERT INTO DBO.[R_STATUS]'\n\n"
                                            + "Please correct it and then upload the script."
                                            : "Check the script whether the Insert format is different from below:\n"
                                            + "'INSERT INTO R_STATUS'\n'INSERT INTO [R_STATUS]'\n'INSERT INTO [DBO].[R_STATUS]'\n'INSERT INTO DBO.R_STATUS'\n"
                                            + "'INSERT INTO [DBO].R_STATUS'\n'INSERT INTO DBO.[R_STATUS]'\n\n"
                                            + "Please correct it and then upload the script.";
                                        strdr.Close();
                                        return sqlstr;
                                    }
                                }
                            }
                        }
                    }
                    //Added by Priyanka on 14012014 end
                }
            }
            //Added by Priyanka on 14012014 start
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                foreach (string s in strfn)
                {
                    if (File.Exists(s))
                        File.Delete(s);
                }
            }
            //Added by Priyanka on 14012014 end
            return sqlstr;
        }

        public static string ReplaceString(string str, string oldValue, string newValue, StringComparison comparison)
        {
            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }
        #endregion

        #region Private Methods
        private void Binding(DataRow DrRead)
        {
            Regcomp = DrRead["co_name"].ToString();

            // Search Product code in Prodmast Table
            try
            {
                string[] cTempFld = { };
                cTempFld = DrRead["product"].ToString().Split(' ');
                DataRow prodRow = DsMain.Tables["prodmasttbl"].Select("regprodcd like '" + cTempFld[0].ToString() + "%'")[0];
                
                /*string prod = DrRead["product"].ToString();
                if (DrRead["product"].ToString().EndsWith(","))
                    prod = prod.Substring(0, prod.Length - 1);
                */
                
                //DataRow prodRow = DsMain.Tables["prodmasttbl"].Select("regprodcd ='" + DrRead["product"].ToString() + "'")[0];
                
                Prodname = prodRow["productnm"].ToString().Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Product code not found in master " + ex.Message.ToString().Trim()); 
            }
 
            Prodver = DrRead["version"].ToString();
            Macid = DrRead["macid"].ToString();
        }

        private void DBFill(DataRow DrRead)
        {
            DataRow drow;
            int cnt = 0;
            //bool isEmpty = true;

            drow = DsMain.Tables["detailtbl"].NewRow();
            drow["srno"] = 1;
            drow["ccomp"] = Convert.ToString(DrRead["co_name"]).Trim();
            drow["select"] = false;
            DsMain.Tables["detailtbl"].Rows.Add(drow);

            foreach (DataColumn dcol in DsRead.Tables[0].Columns)
            {
                if (dcol.ColumnName.ToUpper().StartsWith("COMPANY"))
                    cnt++;
            }
            for (int i = 1; i <= cnt; i++)
            {
                if (Convert.ToString(DrRead["company" + i.ToString().Trim()]).Trim() != "")
                {
                    drow = DsMain.Tables["detailtbl"].NewRow();
                    drow["srno"] = i + 1;
                    drow["ccomp"] = Convert.ToString(DrRead["company" + i.ToString().Trim()]).Trim();
                    drow["select"] = false;
                    DsMain.Tables["detailtbl"].Rows.Add(drow);
                    //isEmpty = false;
                }
            }

            //if (isEmpty == true)
            //{
            //    drow = DsMain.Tables["detailtbl"].NewRow();
            //    drow["srno"] = 1;
            //    drow["ccomp"] = Convert.ToString(DrRead["co_name"]).Trim();
            //    drow["select"] = false;
            //    DsMain.Tables["detailtbl"].Rows.Add(drow);
            //}
        }
        #endregion
    }
}
