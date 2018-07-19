using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

using CustModAccUI.DAL;

namespace CustModAccUI.BLL
{
    public class cls_Gen_Mgr_ConnectVFP:cls_Gen_Ent_ConnectVFP
    {
        #region variable declaration
        //string connectionString;
        OleDbConnection oledbconn;
        //OleDbTransaction oledbtran;
        #endregion
       
        public cls_Gen_Mgr_ConnectVFP(string vfpconnString)
        {
            //connectionString = ConfigurationManager.ConnectionStrings["vfpconnstr"].ConnectionString;
            ConnString = vfpconnString;
        }

        public void Insert()
        {
            try
            {
                if (oledbconn == null)
                {
                    //oledbconn = new OleDbConnection(connectionString);
                    oledbconn = new OleDbConnection(ConnString);
                    oledbconn.Open();
                }
                else
                {
                    if (oledbconn.State == ConnectionState.Closed)
                        oledbconn.Open();
                }

                //oledbtran = oledbconn.BeginTransaction();

                InsertToDBF();
                
                //oledbtran.Commit();
            }
            catch (Exception ex)
            {
                //if (oledbtran != null)
                //{
                //    oledbtran.Rollback();
                //}
                throw ex;
            }
            finally
            {
                oledbconn.Close();
                //oledbtran.Connection.Close();
            }
        }

        private void CmdExecute(string str)
        {
            OleDbCommand oComm;
            int m_execute = 0;
            oComm = new OleDbCommand(str, oledbconn);
            m_execute = oComm.ExecuteNonQuery();
        }

        private void InsertToDBF()
        {
            string instr1 = "", sql2 = "", delstr1 = "";
           
            if (DsConvert.Tables[0].Rows.Count > 0)
            {
                delstr1 = "Delete from custfeature";
                CmdExecute(delstr1);
                delstr1 = "USE feature!custfeature EXCLUSIVE";
                CmdExecute(delstr1);
                delstr1 = "PACK feature!custfeature";
                CmdExecute(delstr1);
                
                instr1 = "Insert into custfeature([rcomp],[rmacid],[ccomp],[optiontype],[optiondesc],[bug]) values(";

                foreach (DataRow outdr in DsConvert.Tables[0].Rows)
                {
                    sql2 = instr1 + " '" + outdr["rcomp"].ToString().Trim() + "','" + outdr["rmacid"].ToString().Trim() + "','" + outdr["ccomp"].ToString().Trim() + "','" + outdr["optiontype"].ToString().Trim() + "','" + outdr["optiondesc"].ToString().Trim() + "','" + outdr["bug"].ToString().Trim() + "')";
                    CmdExecute(sql2);
                }
            }
        }
    }
}
