using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

using CustModAccUI.DAL;

namespace CustModAccUI.BLL
{
    public class cls_Gen_Mgr_Upload_Script:cls_Gen_Ent_Upload_Script
    {
        #region variable declaration
        //string connectionString;
        private SqlConnection oSqlcon; //Added by Priyanka on 14012014
        #endregion

        #region Properties
        private DataSet dsMain;

        public DataSet DsMain
        {
            get { return dsMain; }
            set { dsMain = value; }
        }

        private string ccomp;

        public string Ccomp
        {
            get { return ccomp; }
            set { ccomp = value; }
        }

        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string warMsg;

        public string WarMsg
        {
            get { return warMsg; }
            set { warMsg = value; }
        }
        #endregion

        //public cls_Gen_Mgr_Upload_Script(string connString)  //Commented by Priyanka on 31122013
        public cls_Gen_Mgr_Upload_Script(string connString, string vumess)  //Added by Priyanka on 31122013
        {
            //connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            ConnString = connString;
            Vumess = vumess;  //Added by Priyanka on 31122013
            DsExecute = new DataSet();
            DsMain = new DataSet();
        }

        #region Public Methods
        public void Select(string script)
        {
            ////Added by Priyanka on 14012014 start
            //SqlCommand oSqlCmd = new SqlCommand();
            //SqlTransaction oSqlTrans;
            //this.oSqlcon = new SqlConnection(ConnString);
            //this.oSqlcon.Open();
            //oSqlTrans = null;
            //oSqlTrans = this.oSqlcon.BeginTransaction();
            //oSqlCmd.Connection = this.oSqlcon;
            //oSqlCmd.Transaction = oSqlTrans;
            //oSqlCmd.CommandText = script;
            ////Added by Priyanka on 14012014 end

            DataSet m_DsSelect = new DataSet("DsCustModAccUI");
            //m_DsSelect = cls_Sqlhelper.ExecuteDataset(connectionString, CommandType.Text, script);
            m_DsSelect = cls_Sqlhelper.ExecuteDataset(ConnString, CommandType.Text, script); //Commented by Priyanka on 14012014
            //m_DsSelect = cls_Sqlhelper.ExecuteDataset(oSqlTrans, CommandType.Text, oSqlCmd.CommandText); //Added by Priyanka on 14012014
            DsExecute = m_DsSelect;
            DsExecute.Tables[0].TableName = "cust_com_menu";
            DsExecute.Tables[1].TableName = "cust_lcode";
            DsExecute.Tables[2].TableName = "cust_r_status";
            bool isFound = true;
            int i = 1;
            string[] ccomplist = Ccomp.Split(',');
            WarMsg = "";
            foreach (string ccomp in ccomplist)
            {
                foreach (DataRow dr in DsExecute.Tables["cust_com_menu"].Rows)
                {
                    foreach (DataRow dr1 in DsMain.Tables["subdetailtbl"].Select("optiontype='MENU' and ccomp='" + ccomp.ToString().ToUpper().Trim() + "'"))
                    {
                        if (dr1.RowState != DataRowState.Deleted)
                        {
                            if (dr1["desc1"].ToString().ToUpper().Trim() == dr["padname"].ToString().ToUpper().Trim() && dr1["desc2"].ToString().ToUpper().Trim() == dr["barname"].ToString().ToUpper().Trim())
                                isFound = false;
                        }
                    }
                    foreach (DataRow dr1 in DsMain.Tables["subdetailtbl"].Select("optiontype='COMPANY' and ccomp='" + ccomp.ToString().ToUpper().Trim() + "'"))
                    {
                        if (dr1["optiontype"].ToString().Trim().ToUpper() == "COMPANY")
                            dr1.Delete();
                    }
                    if (isFound == true)
                    {
                        DataRow newRow = DsMain.Tables["subdetailtbl"].NewRow();
                        newRow["srno"] = i;
                        newRow["fk_id"] = Id.ToString().Trim();
                        newRow["ccomp"] = ccomp.ToString().ToUpper().Trim();
                        newRow["optiontype"] = "MENU";
                        newRow["desc1"] = Convert.ToString(dr["padname"]).ToUpper().Trim();
                        newRow["desc2"] = Convert.ToString(dr["barname"]).ToUpper().Trim();
                        DsMain.Tables["subdetailtbl"].Rows.Add(newRow);
                        i++;
                    }
                    else
                        isFound = true;
                }

                foreach (DataRow dr in DsExecute.Tables["cust_lcode"].Rows)
                {
                    foreach (DataRow dr1 in DsMain.Tables["subdetailtbl"].Select("optiontype='TRAN' and ccomp='" + ccomp.ToString().ToUpper().Trim() + "'"))
                    {
                        if (dr1.RowState != DataRowState.Deleted)
                        {
                            if (dr1["desc1"].ToString().ToUpper().Trim() == dr["entry_ty"].ToString().ToUpper().Trim() && dr1["desc2"].ToString().ToUpper().Trim() == dr["code_nm"].ToString().ToUpper().Trim())
                                isFound = false;
                        }
                    }
                    foreach (DataRow dr1 in DsMain.Tables["subdetailtbl"].Select("optiontype='COMPANY' and ccomp='" + ccomp.ToString().ToUpper().Trim() + "'"))
                    {
                        if (dr1["optiontype"].ToString().Trim().ToUpper() == "COMPANY")
                            dr1.Delete();
                    }
                    if (isFound == true)
                    {
                        if (Convert.ToString(dr["entry_ty"]).ToUpper().Trim().StartsWith("X") || Convert.ToString(dr["entry_ty"]).ToUpper().Trim().StartsWith("Y") || Convert.ToString(dr["entry_ty"]).ToUpper().Trim().StartsWith("Z"))
                        {
                            DataRow newRow = DsMain.Tables["subdetailtbl"].NewRow();
                            newRow["srno"] = i;
                            newRow["fk_id"] = Id.ToString().Trim();
                            newRow["ccomp"] = ccomp.ToString().ToUpper().Trim();
                            newRow["optiontype"] = "TRAN";
                            newRow["desc1"] = Convert.ToString(dr["entry_ty"]).ToUpper().Trim();
                            newRow["desc2"] = Convert.ToString(dr["code_nm"]).ToUpper().Trim();
                            DsMain.Tables["subdetailtbl"].Rows.Add(newRow);
                            i++;
                        }
                        //Commented by Priyanka on 31122013 start
                        //else
                        //  WarMsg = "Warning : Transaction Type found invalid.";
                        //Commented by Priyanka on 31122013 end

                        //Added by Priyanka on 31122013 start
                        else
                        {
                            WarMsg = "Want to continue with the transaction type enetered by you?";
                            DialogResult x;
                            x = MessageBox.Show(WarMsg.ToString().Trim(), Vumess, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (x == DialogResult.No)
                                continue;
                            else
                            {
                                DataRow newRow = DsMain.Tables["subdetailtbl"].NewRow();
                                newRow["srno"] = i;
                                newRow["fk_id"] = Id.ToString().Trim();
                                newRow["ccomp"] = ccomp.ToString().ToUpper().Trim();
                                newRow["optiontype"] = "TRAN";
                                newRow["desc1"] = Convert.ToString(dr["entry_ty"]).ToUpper().Trim();
                                newRow["desc2"] = Convert.ToString(dr["code_nm"]).ToUpper().Trim();
                                DsMain.Tables["subdetailtbl"].Rows.Add(newRow);
                                i++;
                            }
                        }
                        //Added by Priyanka on 31122013 end
                    }
                    else
                        isFound = true;
                }
                foreach (DataRow dr in DsExecute.Tables["cust_r_status"].Rows)
                {
                    foreach (DataRow dr1 in DsMain.Tables["subdetailtbl"].Select("optiontype='REPORT' and ccomp='" + ccomp.ToString().ToUpper().Trim() + "'"))
                    {
                        if (dr1.RowState != DataRowState.Deleted)
                        {
                            if (dr1["desc1"].ToString().ToUpper().Trim() == dr["group"].ToString().ToUpper().Trim() && dr1["desc2"].ToString().ToUpper().Trim() == dr["desc"].ToString().ToUpper().Trim() && dr1["desc3"].ToString().ToUpper().Trim() == dr["rep_nm"].ToString().ToUpper().Trim())
                                isFound = false;
                        }
                    }
                    foreach (DataRow dr1 in DsMain.Tables["subdetailtbl"].Select("optiontype='COMPANY' and ccomp='" + ccomp.ToString().ToUpper().Trim() + "'"))
                    {
                        if (dr1["optiontype"].ToString().Trim().ToUpper() == "COMPANY")
                            dr1.Delete();
                    }
                    if (isFound == true)
                    {
                        DataRow newRow = DsMain.Tables["subdetailtbl"].NewRow();
                        newRow["srno"] = i;
                        newRow["fk_id"] = Id.ToString().Trim();
                        newRow["ccomp"] = ccomp.ToString().ToUpper().Trim();
                        newRow["optiontype"] = "REPORT";
                        newRow["desc1"] = Convert.ToString(dr["group"]).ToUpper().Trim();
                        newRow["desc2"] = Convert.ToString(dr["desc"]).ToUpper().Trim();
                        newRow["desc3"] = Convert.ToString(dr["rep_nm"]).ToUpper().Trim();
                        DsMain.Tables["subdetailtbl"].Rows.Add(newRow);
                        i++;
                    }
                    else
                        isFound = true;
                }
            }            
            m_DsSelect.Dispose();
        }
        #endregion
    }
}
