using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using CustModAccUI.DAL;

namespace CustModAccUI.BLL
{
    public class cls_Gen_Mgr_CustModAccUI:cls_Gen_Ent_CustModAccUI
    {
        #region variable declaration
        //string connectionString;
        SqlConnection sqlconn;
        SqlTransaction sqltran;
        #endregion

        #region Properties
        private DataRow drRow1;

        public DataRow DrRow1
        {
            get { return drRow1; }
            set { drRow1 = value; }
        }

        private DataRow drRow2;

        public DataRow DrRow2
        {
            get { return drRow2; }
            set { drRow2 = value; }
        }
        #endregion

        public cls_Gen_Mgr_CustModAccUI(string connString,string username,int range)
        {
           //connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
           ConnString = connString;
           User_name = username;
           Range = range;
           DsSearch = new DataSet();
        }

        #region Enumeration Members
        public enum CustModAccUI_RowName : int
        {
            id = 0,
            date,
            rcomp,
            prodname,
            prodver,
            rmacid,
            ccomp,
            optiontype,
            optiondesc,
            bug,
            pono,
            podate,
            poamt,
            apprby,
            remarks
        }
        #endregion        
       
        #region Database related Methods
        public void SearchById()
        {
            DataSet m_DsSearch = new DataSet();
            //m_DsSearch = cls_Sqlhelper.ExecuteDataset(connectionString, "USP_ENT_CUSTMODACCUI_SELECT_SEARCH");
            m_DsSearch = cls_Sqlhelper.ExecuteDataset(ConnString, "USP_ENT_CUSTMODACCUI_SELECT_SEARCH");
            DsSearch = m_DsSearch;
            m_DsSearch.Dispose();
        }

        public void GenerateDb()
        {
            SqlParameter[] m_spParam = { new SqlParameter("@action","New"),
                                           new SqlParameter("@id","")};
            DataSet m_DsSelect = new DataSet("DsMain");
            //m_DsSelect = cls_Sqlhelper.ExecuteDataset(connectionString, "USP_ENT_CUSTMODACCUI_SELECT", m_spParam);
            m_DsSelect = cls_Sqlhelper.ExecuteDataset(ConnString, "USP_ENT_CUSTMODACCUI_SELECT", m_spParam);
            DsMain = m_DsSelect;
            DsMain.Tables[0].TableName = "maintbl";
            DsMain.Tables[1].TableName = "detailtbl";
            DsMain.Tables[2].TableName = "subdetailtbl";
            DsMain.Tables[3].TableName = "autoidtbl";
            DsMain.Tables[4].TableName = "prodmasttbl";
            m_DsSelect.Dispose();
            DrRow2 = DsMain.Tables["autoidtbl"].Rows[0];
            Id = Convert.ToString(DrRow2["autoid"]);
        }

        public void AutocompleteText()
        {
            string sqlstr = "Select distinct apprby From Custfeature";

            DataSet m_DsSelect = new DataSet();
            m_DsSelect = cls_Sqlhelper.ExecuteDataset(ConnString, CommandType.Text, sqlstr);
            DsSearch = m_DsSelect;
            m_DsSelect.Dispose();
        }

        public void SelectUserRights()
        {
            string sqlstr = "Declare @barname varchar(50),@padname varchar(50) " +
                "Select @barname = barname,@padname = padname " +
                "From Com_menu " +
                "Where Range=" + Convert.ToString(Range.ToString()) + " " +
                "Select dbo.func_decoder(rights,'F') as Rights " +
                "From Userrights " +
                "Where Barname=@barname " +
                    "and Padname=@padname " +
                    "and Range=" + Convert.ToString(Range.ToString()) + " " +
                    "and Upper(dbo.func_decoder([User],'T'))='" + User_name.ToString().Trim() + "'";

            DataSet m_DsSelect = new DataSet("DsCustModAccUI");
            m_DsSelect = cls_Sqlhelper.ExecuteDataset(ConnString, CommandType.Text, sqlstr);
            DsSearch = m_DsSelect;
            m_DsSelect.Dispose();
        }

        public void Select()
        {
            SqlParameter[] m_spParam = { new SqlParameter("@action","Select"),
                                           new SqlParameter("@id",Id)};
            DataSet m_DsSelect = new DataSet("DsCustModAccUI");
            //m_DsSelect = cls_Sqlhelper.ExecuteDataset(connectionString, "USP_ENT_CUSTMODACCUI_SELECT", m_spParam);
            m_DsSelect = cls_Sqlhelper.ExecuteDataset(ConnString, "USP_ENT_CUSTMODACCUI_SELECT", m_spParam);
            DsMain = m_DsSelect;
            DsMain.Tables[0].TableName = "maintbl";
            DsMain.Tables[1].TableName = "detailtbl";
            DsMain.Tables[2].TableName = "subdetailtbl";
            m_DsSelect.Dispose();
            DrRow1 = DsMain.Tables["maintbl"].Rows[0];
            Binding();
        }

        public void Insert()
        {
            int m_execute = 0;
            string sql1 = "";
            try
            {
                SqlParameter[] m_spParam = { new SqlParameter("@action","Insert"),
                                       new SqlParameter("@id",Id),
                                       new SqlParameter("@date",Date),
                                       new SqlParameter("@rcomp",Rcomp),
                                       new SqlParameter("@prodname",Prodname),
                                       new SqlParameter("@prodver",Prodver),
                                       new SqlParameter("@rmacid",Rmacid),
                                       new SqlParameter("@bug",Bug),
                                       new SqlParameter("@pono",Pono),
                                       new SqlParameter("@podate",Podate),
                                       new SqlParameter("@poamt",Poamt),
                                       new SqlParameter("@apprby",Apprby),
                                       new SqlParameter("@remarks",Remarks),
                                       new SqlParameter("@username",User_name)};

                if (sqlconn == null)
                {
                    //sqlconn = new SqlConnection(connectionString);
                    sqlconn = new SqlConnection(ConnString);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                sqltran = sqlconn.BeginTransaction();

                sql1 = InsertDetail();
                
                m_execute = cls_Sqlhelper.ExecuteNonQuery(sqltran, "USP_ENT_CUSTMODACCUI_INSERTUPDATE", m_spParam);
                if (sql1 != string.Empty)
                    m_execute = cls_Sqlhelper.ExecuteNonQuery(sqltran, CommandType.Text, sql1);

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

        public void Update()
        {
            int m_execute = 0;
            string sql1 = "";
            try
            {
                SqlParameter[] m_spParam = { new SqlParameter("@action","Update"),
                                       new SqlParameter("@id",Id),
                                       new SqlParameter("@date",Date),
                                       new SqlParameter("@rcomp",Rcomp),
                                       new SqlParameter("@prodname",Prodname),
                                       new SqlParameter("@prodver",Prodver),
                                       new SqlParameter("@rmacid",Rmacid),
                                       new SqlParameter("@bug",Bug),
                                       new SqlParameter("@pono",Pono),
                                       new SqlParameter("@podate",Podate),
                                       new SqlParameter("@poamt",Poamt),
                                       new SqlParameter("@apprby",Apprby),
                                       new SqlParameter("@remarks",Remarks),
                                       new SqlParameter("@username",User_name)};

                 if (sqlconn == null)
                {
                    //sqlconn = new SqlConnection(connectionString);
                    sqlconn = new SqlConnection(ConnString);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                sqltran = sqlconn.BeginTransaction();

                sql1 = UpdateDetail();

                m_execute = cls_Sqlhelper.ExecuteNonQuery(sqltran, "USP_ENT_CUSTMODACCUI_INSERTUPDATE", m_spParam);

                if (sql1 != string.Empty)
                    m_execute = cls_Sqlhelper.ExecuteNonQuery(sqltran, CommandType.Text, sql1);

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

        public void Delete()
        {
            int m_execute = 0;
            try
            {
                SqlParameter[] m_spParam = {new SqlParameter("@id",Id)};

                if (sqlconn == null)
                {
                    //sqlconn = new SqlConnection(connectionString);
                    sqlconn = new SqlConnection(ConnString);
                    sqlconn.Open();
                }
                else
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();
                }

                sqltran = sqlconn.BeginTransaction();

                m_execute = cls_Sqlhelper.ExecuteNonQuery(sqltran, "USP_ENT_CUSTMODACCUI_DELETE", m_spParam);

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

        public void ConvertToDBF(string id)
        {
            DataSet m_DsSelect = new DataSet();
            //m_DsSelect = cls_Sqlhelper.ExecuteDataset(connectionString,"USP_ENT_INSERT_TO_DBF",id.ToString().Trim());
            m_DsSelect = cls_Sqlhelper.ExecuteDataset(ConnString, "USP_ENT_INSERT_TO_DBF", id.ToString().Trim());
            DsConvert = m_DsSelect;
            m_DsSelect.Dispose();
        }
        #endregion

        #region Private Methods
        private string InsertDetail()
        {
            string instr1 = "", sql2 = "", sql3 = "";

            if (DsMain.Tables["detailtbl"].Rows.Count > 0 || DsMain.Tables["subdetailtbl"].Rows.Count > 0)
            {
                DataTable dt1;
                instr1 = "Insert into custmnutranrptdts(fk_id,ccomp,optiontype,desc1,desc2,desc3) values(";

                Ccomp = string.Empty;
                Optiontype = string.Empty;
                Desc1 = string.Empty;
                Desc2 = string.Empty;
                Desc3 = string.Empty;

                foreach (DataRow outdr in DsMain.Tables["detailtbl"].Rows)
                {
                    Ccomp = Convert.ToString(outdr["ccomp"]).Trim();
                    dt1 = DsMain.Tables["subdetailtbl"].Copy();
                    dt1.DefaultView.RowFilter = "ccomp='" + Ccomp.ToString().Trim() + "'";
                    if (dt1.DefaultView.ToTable().Rows.Count > 0)
                    {
                        foreach (DataRow indr in dt1.DefaultView.ToTable().Rows)
                        {
                            Optiontype = Convert.ToString(indr["optiontype"]).Trim();
                            Desc1 = Convert.ToString(indr["desc1"]).Trim();
                            Desc2 = Convert.ToString(indr["desc2"]).Trim();
                            Desc3 = Convert.ToString(indr["desc3"]).Trim();
                            sql2 = instr1 + " '" + Id.ToString().Trim() + "','" + Ccomp + "','" + Optiontype + "','" + Desc1 + "','" + Desc2 + "','" + Desc3 + "')";
                            sql3 = sql3 + " " + sql2;
                        }
                    }
                    else
                    {
                        sql2 = instr1 + " '" + Id.ToString().Trim() + "','" + Ccomp + "','COMPANY','','','')";
                        sql3 = sql3 + " " + sql2;
                    }
                }
            }
            return sql3;
        }

        private string UpdateDetail()
        {
            string instr1 = "", upstr1 = "", destr1 = "", sql2 = "", sql3 = "", autoid = "";

            if (DsMain.Tables["detailtbl"].Rows.Count > 0 || DsMain.Tables["subdetailtbl"].Rows.Count > 0)
            {
                DataTable dt1;
                Ccomp = "";
                instr1 = "Insert into custmnutranrptdts(fk_id,ccomp,optiontype,desc1,desc2,desc3) values(";
                upstr1 = "Insert into custmnutranrptdts(fk_id,ccomp,optiontype,desc1,desc2,desc3) values(";

                foreach (DataRow dr in DsMain.Tables["detailtbl"].Rows)
                    Ccomp = (Ccomp == string.Empty || Ccomp == null) ? "'" + Convert.ToString(dr["ccomp"]).Trim() + "'" : Ccomp + ",'" + Convert.ToString(dr["ccomp"]).Trim() + "'";

                destr1 = "Delete from custmnutranrptdts where ccomp in (" + Ccomp.ToString().Trim() + ") and fk_id = '" + Id.ToString().Trim() + "'";

                
                foreach (DataRow outdr in DsMain.Tables["detailtbl"].Rows)
                {
                    Ccomp = Convert.ToString(outdr["ccomp"]).Trim();
                    autoid = Convert.ToString(outdr["id"]).Trim();
                    dt1 = DsMain.Tables["subdetailtbl"].Copy();
                    dt1.DefaultView.RowFilter = "ccomp='" + Ccomp.ToString().Trim() + "'";

                    if (dt1.DefaultView.ToTable().Rows.Count > 0)
                    {
                        foreach (DataRow indr in dt1.DefaultView.ToTable().Rows)
                        {
                            autoid = Convert.ToString(indr["id"]).Trim();
                            Optiontype = Convert.ToString(indr["optiontype"]).Trim();
                            Desc1 = Convert.ToString(indr["desc1"]).Trim();
                            Desc2 = Convert.ToString(indr["desc2"]).Trim();
                            Desc3 = Convert.ToString(indr["desc3"]).Trim();                          

                            if ((Convert.ToString(indr["id"]) == string.Empty) && (Convert.ToString(indr["fk_id"]) == string.Empty))
                            {
                                sql2 = instr1 + "'" + Id.ToString().Trim() + "','" + Ccomp + "','" + Optiontype + "','" + Desc1 + "','" + Desc2 + "','" + Desc3 + "')";
                                sql3 = sql3 + " " + sql2;
                            }
                            else
                            {
                                sql2 = upstr1 + "'" + Id.ToString().Trim() + "','" + Ccomp + "','" + Optiontype + "','" + Desc1 + "','" + Desc2 + "','" + Desc3 + "')";
                                sql3 = sql3 + " " + sql2;
                            }
                        }
                    }
                    else
                    {
                        sql2 = upstr1 + "'" + Id.ToString().Trim() + "','" + Ccomp + "','" + Optiontype + "','" + Desc1 + "','" + Desc2 + "','" + Desc3 + "')";
                        sql3 = sql3 + " " + sql2;
                    }
                }
            }
            return destr1 + " " + sql3;
        }

        private void Binding()
        {
            Id = Convert.ToString(DrRow1["id"]);
            Date = Convert.ToDateTime(DrRow1["date"]);
            Rcomp = Convert.ToString(DrRow1["rcomp"]);
            Prodname = Convert.ToString(DrRow1["prodname"]);
            Prodver = Convert.ToString(DrRow1["prodver"]);
            Rmacid = Convert.ToString(DrRow1["rmacid"]);
            Bug = Convert.ToString(DrRow1["bug"]);
            Pono = Convert.ToString(DrRow1["pono"]);
            Podate = Convert.ToDateTime(DrRow1["podate"]);
            Poamt = Convert.ToDecimal(DrRow1["poamt"]);
            Apprby = Convert.ToString(DrRow1["apprby"]);
            Remarks = Convert.ToString(DrRow1["remarks"]);
        }
        #endregion
    }
}
