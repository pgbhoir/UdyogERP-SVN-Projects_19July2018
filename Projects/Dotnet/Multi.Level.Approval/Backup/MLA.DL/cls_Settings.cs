using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MLA.DL
{
    public class cls_Settings
    {
        #region private methods,constructors,class and variable declaration

        private string connectionstring;
        public string Connectionstring
        {
            get { return connectionstring; }
            set { connectionstring = value; }
        }


        public cls_Settings() 
        {
        }

        DataSet m_ds_settings = new DataSet();
        DataSet m_ds_Applevel_Mstr = new DataSet();

        public DataSet GetAllInitialData(int compid)
        {
            m_ds_settings = new DataSet();

            string sqlquery = "select code_nm,bcode_nm,entry_ty,applevel,applevel_init from lcode where apgenps = 1 " +
                //"select '' as type,' -------- Select Role / User --------- ' as user_role,'' as user_name,'' as designation,'' as dept,'' as email " +
                //"union all " +
                            "select 'Role' as type, user_roles as user_role,'' as user_name, '' as designation,'' as dept,'' as email,company,user_roles from vudyog..userroles " +
                            "union all " +
                            "select 'User' as type,[user] as user_role,isnull(user_name,'') as user_name, isnull(user_desig,'') as designation,isnull(user_dept,'') as dept,isnull(user_email,'') as email,company,user_roles from vudyog..[user] " +
                            "select [user],user_name,user_roles,company,isnull(user_desig,'') as designation,isnull(user_dept,'') as dept,isnull(user_email,'') as email from vudyog..[user] " +
                            "select 0 as id,cast('' as varchar(02)) as entry_ty where 1=2" +
                            "select name as code_nm, code as entry_ty,applevel,applevel_init from mastcode where applevel > 0" +
                            "select compid,co_name from vudyog..co_mast where compid = " + Convert.ToString(compid).Trim() +
                            "select 'None' as [user],'' as user_name,'' as user_roles,cast('' as varbinary(max)) as company,'' as designation,'' as dept,'' as email ";




            m_ds_settings = cls_Sqlhelper.ExecuteDataset(Connectionstring, CommandType.Text, sqlquery);
            m_ds_settings.Tables[0].TableName = "Lcode";
            m_ds_settings.Tables[1].TableName = "Usersroles";
            m_ds_settings.Tables[2].TableName = "User_mstr";
            m_ds_settings.Tables[3].TableName = "Recdelstat";
            m_ds_settings.Tables[4].TableName = "Mastcode";
            m_ds_settings.Tables[5].TableName = "Compmast";
            m_ds_settings.Tables[6].TableName = "User_Init";

            // Check Company rights for User 

            string compname = string.Empty;
            if (m_ds_settings.Tables["Compmast"] != null)
                if (m_ds_settings.Tables["Compmast"].Rows.Count > 0)
                    compname = Convert.ToString(m_ds_settings.Tables["Compmast"].Rows[0]["co_name"]).Trim();
                else
                    throw new Exception("Company ID not found in Master");
            else
                throw new Exception("Company ID not found in Master");

            foreach (DataRow dr in m_ds_settings.Tables["user_mstr"].Rows)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dr["company"])))
                {
                    byte[] dbytes = (byte[])dr["company"];
                    if (dbytes.Count() > 0)
                    {
                        string lcompany = "0x" + cls_Common.ByteArrayToString(dbytes);
                        if (cls_Common.Decrypt(lcompany, Convert.ToString(dr["user_roles"]).Trim()).IndexOf(compname) < 0)
                            dr.Delete();
                    }
                }

            }
            m_ds_settings.Tables["user_mstr"].AcceptChanges();

            // Check Company rights for User 
            foreach (DataRow dr in m_ds_settings.Tables["Usersroles"].Rows)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dr["company"])))
                {
                    byte[] dbytes = (byte[])dr["company"];
                    string lcompany = "0x" + cls_Common.ByteArrayToString(dbytes);
                    if (cls_Common.Decrypt(lcompany, Convert.ToString(dr["user_roles"]).Trim()).IndexOf(compname) < 0)
                        dr.Delete();
                }
            }

            m_ds_settings.Tables["Usersroles"].AcceptChanges();


            return m_ds_settings;
        }

      
        public DataSet GetAllAppLevel_Mstr(string trantype,
                                           string initiator)
        {
            m_ds_Applevel_Mstr = new DataSet();

            string sqlquery = "select isnull(id,0) as id,initiator_id,'Approval Level '+ltrim(rtrim(str(applevel))) as applevel,user_role, [type], entry_ty," +
                              "'U' as recstatus,isnull(bypasslevel,0) as bypasslevel,active  from applevel_mstr " +
                              " where entry_ty ='" + trantype + "' and initiator_id ='" + initiator + "'";


            m_ds_Applevel_Mstr = cls_Sqlhelper.ExecuteDataset(Connectionstring, CommandType.Text, sqlquery);
            m_ds_Applevel_Mstr.Tables[0].TableName = "Applevel_mstr";
            

            return m_ds_Applevel_Mstr;
        }

        public void Save(string userid,
                         bool active)
        {
            string sqlinsertstring = " Insert into Applevel_mstr(initiator_id,entry_ty,applevel,user_role,type,bypasslevel,add_user,add_dt) values('";
            string sqlupdatestring = " update Applevel_mstr set ";
            string sqldeletestring = " delete from Applevel_mstr where id =";
            string sqlupdateactivestatusstring = " update applevel_mstr set active =" + (active == true ? "1" : "0") + " where ";
            string finalstring = string.Empty;

            // Generate delete string if records found in table
            foreach (DataRow drdel in m_ds_settings.Tables["recdelstat"].Rows)
            {
                finalstring = finalstring + sqldeletestring + Convert.ToInt32(drdel["id"]);
            }

            foreach (DataRow dr in m_ds_Applevel_Mstr.Tables["Applevel_mstr"].Rows)
            {
                int applevel = Convert.ToInt32(Convert.ToString(dr["applevel"]).Trim().Substring(Convert.ToString(dr["applevel"]).Trim().Length-1,1));

                switch (Convert.ToString(dr["recstatus"]).Trim())
                {
                    case "I":
                        finalstring = finalstring + sqlinsertstring + Convert.ToString(dr["initiator_id"]).Trim() + "','" +
                                                        Convert.ToString(dr["entry_ty"]).Trim() + "'," +
                                                        Convert.ToString(applevel).Trim() + ",'" +
                                                        Convert.ToString(dr["user_role"]).Trim() + "','" +
                                                        Convert.ToString(dr["type"]).Trim() + "'," +
                                                        (dr["bypasslevel"] != DBNull.Value ? (Convert.ToBoolean(dr["bypasslevel"]) == true ? "1" : "0") : "0") + ",'" +
                                                        userid.Trim() + "','" +
                                                        DateTime.Now.ToString().Trim() +"')";
                        break;
                    case "U":
                        finalstring = finalstring + sqlupdatestring + "user_role ='" + Convert.ToString(dr["user_role"]).Trim() + "'," +
                                                                      "type='" + Convert.ToString(dr["type"]).Trim() + "'," +
                                                                      "bypasslevel=" + (dr["bypasslevel"] != DBNull.Value ? (Convert.ToBoolean(dr["bypasslevel"]) == true ? "1" : "0") : "0") + "," +
                                                                      "edit_user='" + userid.Trim() +"'," +
                                                                      "edit_dt='" + DateTime.Now.ToString().Trim() +"'" +
                                                                      " where id =" + Convert.ToInt32(dr["id"]);
                        break;
                }

                finalstring = finalstring + sqlupdateactivestatusstring + " entry_ty ='" + Convert.ToString(dr["entry_ty"]).Trim() +
                                                                            "' and initiator_id ='" + Convert.ToString(dr["initiator_id"]).Trim() + "' ";
                                                                            
            }

            finalstring = " set dateformat dmy " + finalstring;

            // final post to data
            SqlConnection sqlconn = null;
            SqlTransaction sqltran = null;
            try
            {
                if (sqlconn == null)
                {
                    sqlconn = new SqlConnection(Connectionstring);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                sqltran = sqlconn.BeginTransaction();

                cls_Sqlhelper.ExecuteNonQuery(Connectionstring, CommandType.Text, finalstring);
                sqltran.Commit();
            }
            catch (Exception ex)
            {
                if (sqltran != null)
                {
                    sqltran.Rollback();
                }
                throw ex;
            }
            finally
            {
                sqlconn.Close();
            }
        }
        #endregion


    }
}
