using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using MLA.ENTITY;

namespace MLA.DL
{
    public class cls_Approval
    {
        public cls_Approval()
        {
        }

        #region private methods,constructors,class and variable declaration
        private string connectionstring;
        public string Connectionstring
        {
            get { return connectionstring; }
            set { connectionstring = value; }
        }
        #endregion

        DataSet m_ds_settings = new DataSet();
        DataSet m_ds_applevel = new DataSet();

        public DataSet GetAllInitialData(int compid)
        {
            m_ds_settings = new DataSet();


            string sqlquery = "select code_nm,bcode_nm,entry_ty,(case when bcode_nm !='' then bcode_nm when bcode_nm ='' and ext_vou = 1 then '' else entry_ty end+'MAIN') as maintblname," +
                              "(case when bcode_nm !='' then bcode_nm when bcode_nm ='' and ext_vou = 1 then '' else entry_ty end+'ITEM') as itemtblname," +
                              "(case when bcode_nm !='' then bcode_nm when bcode_nm ='' and ext_vou = 1 then '' else entry_ty end+'ACDET') as actblname," +
                              "v_account,v_item,applevel,isnull(appmailsub,'') as appmailsub from lcode where apgenps = 1 " +
                              "select [user],[user_roles],[user_dept],[user_email],[user_name],[company] from vudyog..[user] " +
                              "select name as code_nm, code as entry_ty,applevel,isnull(appmailsub,'') as appmailsub from mastcode where applevel > 0" +
                              "select compid,co_name from vudyog..co_mast where compid = " + Convert.ToString(compid).Trim();

            m_ds_settings = cls_Sqlhelper.ExecuteDataset(Connectionstring, CommandType.Text, sqlquery);
            m_ds_settings.Tables[0].TableName = "Lcode";
            m_ds_settings.Tables[1].TableName = "User";
            m_ds_settings.Tables[2].TableName = "Mastcode";
            m_ds_settings.Tables[3].TableName = "Compmast";

            string compname = string.Empty;
            if (m_ds_settings.Tables["Compmast"] != null)
                if (m_ds_settings.Tables["Compmast"].Rows.Count > 0)
                    compname = Convert.ToString(m_ds_settings.Tables["Compmast"].Rows[0]["co_name"]).Trim();
                else
                    throw new Exception("Company ID not found in Master");
            else
                throw new Exception("Company ID not found in Master");

            foreach (DataRow dr in m_ds_settings.Tables["User"].Rows)
            {
                byte[] dbytes = (byte[])dr["company"];
                if (dbytes.Count() > 0)
                {
                    string lcompany = "0x" + cls_Common.ByteArrayToString(dbytes);
                    if (cls_Common.Decrypt(lcompany, Convert.ToString(dr["user_roles"]).Trim()).IndexOf(compname) < 0)
                        dr.Delete();
                }

            }
            m_ds_settings.Tables["User"].AcceptChanges();

            return m_ds_settings;
        }
        
        public DataSet GetAppLevelDtl(string trantype)
        {
            m_ds_applevel = new DataSet();
            string sqlquery = "select entry_ty,tran_cd,applevel,user_role,approved_remarks,rejected_remarks,levelstatus from applevel_dtl where entry_ty='"+trantype+"'";
            m_ds_applevel  = cls_Sqlhelper.ExecuteDataset(Connectionstring, CommandType.Text, sqlquery);
            m_ds_applevel.Tables[0].TableName = "AppLevel_Dtl";

            return m_ds_applevel;
        }

        public DataTable GetMasterData(string masttype,
                                       string userid)
        {
            DataSet m_ds_mast;

            DataTable dtmain = new DataTable();

            var rowuserrole = (from DataRow rowUser in m_ds_settings.Tables["user"].Rows
                               where rowUser["user"].ToString().Trim() == userid
                               select rowUser).FirstOrDefault();

            string userrole = string.Empty;

            if (rowuserrole != null)
                userrole = rowuserrole.Field<string>("User_Roles");

            string sqlquery = string.Empty;
            if (masttype == "AM")
                sqlquery = "select c.ac_id,c.ac_name,c.mailname,c.[group],c.add1,c.add2,c.add3,c.city,c.state,c.zip,c.phone,c.email," +
                           "(case when a.levelstatus = '' or a.levelstatus is null then 'Waiting for Approval' else a.levelstatus end )as apgen," +
                           "cast('' as varchar(250)) as approved_remarks,cast('' as varchar(250)) as rejected_remarks,b.bypasslevel," +
                           "(case when a.prelevel > 0 then 'Level ' + rtrim(ltrim(str(a.prelevel))) + ' ' + rtrim(ltrim(a.prelevelstatus)) else '' end) as lastlevelstatus,c.user_name," +
                           "a.* from applevel_dtl a " +
                           "inner join applevel_mstr b on a.entry_ty = b.entry_ty and a.initiator_id = b.initiator_id and a.applevel = b.applevel " +
                           "inner join ac_mast c on a.tran_cd = c.ac_id and a.initiator_id = (case when a.initiator_id = 'None' then 'None' else c.user_name end) " +
                           "where a.levelstatus not in ('Approved','Rejected') " +
                           "and a.prelevelstatus = (case when b.bypasslevel = 0 then 'Approved' else '' end) " +
                           "and b.active = 1 and b.entry_ty = 'AM' " +
                           "and b.user_role = (case when b.type = 'Role' then '" + userrole.Trim() + "' else '" + userid.Trim() + "' end) " +
                           "and c.[group] in ('Sundry Creditors','Sundry Debtors')";
            else
                if (masttype == "IM")
                    sqlquery = "select c.it_code,c.it_name,c.it_desc,c.[group],c.rate,c.rateunit,c.[type],c.nonstk,c.apgen," +
                              "(case when a.levelstatus = '' or a.levelstatus is null then 'Waiting for Approval' else a.levelstatus end )as apgen," +
                              "cast('' as varchar(250)) as approved_remarks,cast('' as varchar(250)) as rejected_remarks,b.bypasslevel," +
                              "(case when a.prelevel > 0 then 'Level ' + rtrim(ltrim(str(a.prelevel))) + ' ' + rtrim(ltrim(a.prelevelstatus)) else '' end) as lastlevelstatus,c.user_name," +
                              "a.* from applevel_dtl a " +
                              "inner join applevel_mstr b on a.entry_ty = b.entry_ty and a.initiator_id = b.initiator_id and a.applevel = b.applevel " +
                              "inner join it_mast c on a.tran_cd = c.it_code and a.initiator_id = (case when a.initiator_id = 'None' then 'None' else c.user_name end) " +
                              "where a.levelstatus not in ('Approved','Rejected') " +
                              "and a.prelevelstatus = (case when b.bypasslevel = 0 then 'Approved' else '' end) " +
                              "and b.active = 1 and b.entry_ty = 'IM' " +
                              "and b.user_role = (case when b.type = 'Role' then '" + userrole.Trim() + "' else '" + userid.Trim() + "' end) ";



            m_ds_mast = cls_Sqlhelper.ExecuteDataset(Connectionstring, CommandType.Text, sqlquery);

            if (masttype == "AM")
                m_ds_mast.Tables[0].TableName = "AMastData";
            else
                if (masttype == "IM")
                    m_ds_mast.Tables[0].TableName = "IMastData";

            return m_ds_mast.Tables[0];
        }

        private string GetTranTblname(string trantype, string type)
        {
            DataRow lcoderow = m_ds_settings.Tables["lcode"].Select("entry_ty='" + trantype + "'")[0];
            string tblname = string.Empty;

            if (type == "Main")
                tblname = Convert.ToString(lcoderow["maintblname"]).Trim();
            else if (type == "Item")
                tblname = Convert.ToString(lcoderow["itemtblname"]).Trim();
            else if (type == "Acdet")
                tblname = Convert.ToString(lcoderow["actblname"]).Trim();

            return tblname;
        }

        public DataTable GetApplevelMstr(string trantype)
        {
            string sqlquery = "select id,initiator_id,entry_ty,applevel,user_role,[type],bypasslevel,active from applevel_mstr " +
                            "where entry_ty ='" + trantype.Trim() + "' and active = 1 ";
            
            DataSet dsApplevelMstr = cls_Sqlhelper.ExecuteDataset(Connectionstring, CommandType.Text, sqlquery);
            dsApplevelMstr.Tables[0].TableName = "AppLevel_Mstr";
            return dsApplevelMstr.Tables[0];
        }


        public DataTable GetMainList(string trantype, DateTime startdt,
                                      DateTime enddt, string userid)
        {

            string tblname = string.Empty;
            tblname = GetTranTblname(trantype,"Main");

            DataTable dtmain = new DataTable();

            var rowuserrole = (from DataRow rowUser in m_ds_settings.Tables["user"].Rows
                               where rowUser["user"].ToString().Trim() == userid
                               select rowUser).FirstOrDefault();

            string userrole = string.Empty;

            if (rowuserrole != null)
                userrole = rowuserrole.Field<string>("User_Roles");

            string sqlquery = "set dateformat dmy " +
                              "select c.entry_ty,c.date,c.inv_no,c.inv_sr,c.dept,c.cate,c.party_nm,d.ac_name,d.mailname,c.net_amt," +
                              "(case when levelstatus = '' or a.levelstatus is null then 'Waiting for Approval' else levelstatus end )as apgen," +
                              "cast('' as varchar(250)) as approved_remarks,cast('' as varchar(250)) as rejected_remarks,b.bypasslevel," +
                              "(case when a.prelevel > 0 then 'Level ' + rtrim(ltrim(str(a.prelevel))) + ' ' + rtrim(ltrim(a.prelevelstatus)) else '' end) as lastlevelstatus,c.user_name," +
                              "a.* from applevel_dtl a " +
                              "inner join applevel_mstr b on a.entry_ty = b.entry_ty and a.initiator_id = b.initiator_id and a.applevel = b.applevel " +
                              "inner join " + tblname.Trim() + " c on a.entry_ty = c.entry_ty and a.tran_cd = c.tran_cd and a.initiator_id = (case when a.initiator_id = 'None' then 'None' else c.user_name end) " +
                              "inner join ac_mast d on c.ac_id = d.ac_id " +
                              "where a.levelstatus not in ('Approved','Rejected')  " + 
                              "and a.prelevelstatus = (case when b.bypasslevel = 0 then 'Approved' else '' end) " +
                              "and b.active = 1 " +
                              "and b.user_role = (case when b.type = 'Role' then '" + userrole.Trim() + "' else '" + userid.Trim() + 
                              "' end) and c.entry_ty ='" + trantype.Trim() + "' and c.date between '" + startdt.ToString().Trim() + "' and '" + enddt.ToString().Trim() + "'";

            DataSet dsMain = cls_Sqlhelper.ExecuteDataset(Connectionstring, CommandType.Text, sqlquery);
            dsMain.Tables[0].TableName = "Main";
            dtmain = dsMain.Tables[0];

           
    //        string sqlquery = "set dateformat dmy  select entry_ty,date,inv_no,inv_sr,dept,cate,party_nm,net_amt,apgenby,'Waiting for Approval' as apgen," +
    //                          "u_remarks,tran_cd,cast('' as varchar(100)) as levelstatus,cast(0 as int) as lastlevel," +
    //                          "cast(0 as int) as currlevel,convert(bit,0) as approve,user_name  from " + tblname + " where " +
    //                          "entry_ty='" + trantype + "' and date between '" + startdt.ToString().Trim() + 
    //                          "' and '" + enddt.ToString().Trim() + "' order by inv_no ";


            //SqlParameter[] spparam = new SqlParameter[5];
            //spparam[0] = new SqlParameter("@ptblname", SqlDbType.VarChar, 20);
            //spparam[0].Direction = ParameterDirection.Input;
            //spparam[0].Value = tblname;

            //spparam[1] = new SqlParameter("@ptrantype", SqlDbType.VarChar, 2);
            //spparam[1].Direction = ParameterDirection.Input;
            //spparam[1].Value = trantype;


            //spparam[2] = new SqlParameter("@pstdate", SqlDbType.DateTime);
            //spparam[2].Direction = ParameterDirection.Input;
            //spparam[2].Value = startdt;


            //spparam[3] = new SqlParameter("@pendate", SqlDbType.DateTime);
            //spparam[3].Direction = ParameterDirection.Input;
            //spparam[3].Value = enddt;


            //spparam[4] = new SqlParameter("@puser", SqlDbType.VarChar, 50);
            //spparam[4].Direction = ParameterDirection.Input;
            //spparam[4].Value =userid;


            //DataSet dsMain = cls_Sqlhelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "USP_Ent_GetMainTransactionData", spparam);
            //dsMain.Tables[0].TableName = "Main";
            //dtmain = dsMain.Tables[0];
            return dtmain;
        }

        public Main PopulateMain(IDataRecord dr)
        {
            Main main = new Main();

            if (dr["entry_ty"] != DBNull.Value)
                main.Entry_ty = Convert.ToString(dr["entry_ty"]);

            if (dr["date"] != DBNull.Value)
                main.Date = Convert.ToDateTime(dr["date"]);

            if (dr["inv_no"] != DBNull.Value)
                main.Inv_no = Convert.ToString(dr["inv_no"]);

            if (dr["inv_sr"] != DBNull.Value)
                main.Inv_sr = Convert.ToString(dr["inv_sr"]);

            if (dr["party_nm"] != DBNull.Value)
                main.Party_nm = Convert.ToString(dr["party_nm"]);

            if (dr["cate"] != DBNull.Value)
                main.Cate = Convert.ToString(dr["cate"]);

            if (dr["dept"] != DBNull.Value)
                main.Dept = Convert.ToString(dr["dept"]);

            if (dr["net_amt"] != DBNull.Value)
                main.Net_amt = Convert.ToDecimal(dr["net_amt"]);

            if (dr["apgenby"] != DBNull.Value)
                main.Apgenby = Convert.ToString(dr["apgenby"]);

            if (dr["apgen"] != DBNull.Value)
                main.Apgen = Convert.ToString(dr["apgen"]);

            //if (dr["u_remarks"] != DBNull.Value)
            //    main.U_remarks = Convert.ToString(dr["u_remarks"]);

            if (dr["tran_cd"] != DBNull.Value)
                main.Tran_cd = Convert.ToInt32(dr["tran_cd"]);

            if (dr["levelstatus"] != DBNull.Value)
                main.Levelstatus = Convert.ToString(dr["levelstatus"]);

            if (dr["lastlevel"] != DBNull.Value)
                main.Lastlevel = Convert.ToInt32(dr["lastlevel"]);

            if (dr["currlevel"] != DBNull.Value)
                main.Currlevel = Convert.ToInt32(dr["currlevel"]);

            if (dr["approve"] != DBNull.Value)
                main.Approve = Convert.ToBoolean(dr["approve"]);

            if (dr["user_name"] != DBNull.Value)
                main.User_name = Convert.ToString(dr["user_name"]);

            main.Approved_Remarks = string.Empty;
            main.Rejected_Remarks = string.Empty;

            return main;

        }


        public List<ItemDet> GetItemDataList(string trantype, int tran_cd)
        {
            string tblname = string.Empty;
            tblname = GetTranTblname(trantype,"Item");

            List<ItemDet> item = new List<ItemDet>();

            string sqlquery = "select item_no,item,qty,rate,gro_amt,tran_cd,re_qty from " + tblname +
                                  " where entry_ty='" + trantype + "' and tran_cd =" + tran_cd + " order by inv_no ";
            SqlDataReader dr = cls_Sqlhelper.ExecuteReader(Connectionstring, CommandType.Text, sqlquery);

            while (dr.Read())
            {
                item.Add(PopulateItem(dr));
            }

            return item;
        }

        public ItemDet PopulateItem(IDataRecord dr)
        {
            ItemDet item = new ItemDet();

            if (dr["item_no"] != DBNull.Value)
                item.Item_no = Convert.ToInt32(dr["item_no"]);

            if (dr["item"] != DBNull.Value)
                item.Item = Convert.ToString(dr["item"]);

            if (dr["qty"] != DBNull.Value)
                item.Qty = Convert.ToDecimal(dr["qty"]);

            if (dr["rate"] != DBNull.Value)
                item.Rate = Convert.ToDecimal(dr["rate"]);

            if (dr["gro_amt"] != DBNull.Value)
                item.Gro_amt = Convert.ToDecimal(dr["gro_amt"]);

            if (dr["tran_cd"] != DBNull.Value)
                item.Tran_cd = Convert.ToInt32(dr["tran_cd"]);

            if (dr["re_qty"] != DBNull.Value)
                item.Re_qty = Convert.ToDecimal(dr["re_qty"]);

            return item;
        }

        public List<AcDet> GetAccountDataList(string trantype, int tran_cd)
        {
            string tblname = string.Empty;
            tblname = GetTranTblname(trantype, "Acdet");

            List<AcDet> acdet = new List<AcDet>();

            string sqlquery = "select  ac_name,amount,amt_ty,tran_cd from " + tblname +
                                  " where entry_ty='" + trantype + "' and tran_cd =" + tran_cd + " order by inv_no ";
            SqlDataReader dr = cls_Sqlhelper.ExecuteReader(Connectionstring, CommandType.Text, sqlquery);

            while (dr.Read())
            {
                acdet.Add(PopulateAccount(dr));
            }

            return acdet;
        }

        public AcDet PopulateAccount(IDataRecord dr)
        {
            AcDet acdet = new AcDet();

            if (dr["tran_cd"] != DBNull.Value)
                acdet.Tran_cd = Convert.ToInt32(dr["tran_cd"]);

            if (dr["ac_name"] != DBNull.Value)
                acdet.Ac_name = Convert.ToString(dr["ac_name"]);

            if (dr["amount"] != DBNull.Value)
                acdet.Amount = Convert.ToDecimal(dr["amount"]);

            if (dr["amt_ty"] != DBNull.Value)
                acdet.Amt_ty = Convert.ToString(dr["amt_ty"]);

            return acdet;
        }


        public List<AppMailSetter> Save(DataTable dtView,
                         DataTable dtAppLevel_Mstr,
                         string trantype,
                         string userid,
                         string keyfld)
        {

            string tblname = string.Empty;
            switch (trantype)
            {
                case "IM":
                    tblname = "It_Mast";
                    break;
                case "AM":
                    tblname = "Ac_Mast";
                    break;
                default:
                    tblname = GetTranTblname(trantype, "Main");
                    break;
            }

            string sqlupdatestring = " Update applevel_dtl set user_role ='" + userid.Trim() + "',levelstatus='";
            string sqlupdateprelevelstring = " Update applevel_dtl set prelevelstatus ='";
            string updatefinalappstring = " update " + tblname + " set ";

            string finalstring = string.Empty;
            SqlDataReader dr = null;

            List<AppMailSetter> ListAppMailSetter = new List<AppMailSetter>();
            AppMailSetter appmailsetter;

            foreach (DataRow drow in dtView.Rows)
            {
                int applevel = Convert.ToInt32(drow["applevel"]);

                if (Convert.ToString(drow["levelstatus"]).Trim() == "Approved" ||
                    Convert.ToString(drow["levelstatus"]).Trim() == "Rejected")
                {

                    appmailsetter = new AppMailSetter();

                    string sqlquery = string.Empty;
                    if (Convert.ToBoolean(drow["bypasslevel"]) == true)
                    {
                        sqlquery = "select applevel from applevel_dtl where entry_ty ='" + trantype.Trim() +"' and tran_cd =" + Convert.ToInt32(drow[keyfld]) +
                                   " and initiator_id ='" + Convert.ToString(drow["initiator_id"]).Trim() + "'" +
                                   " and levelstatus != 'Approved' and applevel > " + applevel;

                        dr = cls_Sqlhelper.ExecuteReader(Connectionstring, CommandType.Text, sqlquery);

                        while (dr.Read())
                        {
                            if (Convert.ToString(drow["levelstatus"]) == "Approved")
                                finalstring = finalstring + sqlupdatestring + "Approved" + "',prelevelstatus='Approved'" +
                                                   ",approved_remarks ='Approved by Level " + Convert.ToString(applevel).Trim() + "',rejected_remarks =''" +
                                                   " where " +
                                                   " entry_ty ='" + Convert.ToString(drow["entry_ty"]).Trim() + "'" +
                                                   " and tran_cd = " + Convert.ToInt32(drow[keyfld]) + " and applevel =" + Convert.ToString(dr["applevel"]).Trim();
                            else if (Convert.ToString(drow["levelstatus"]) == "Rejected")
                                finalstring = finalstring + sqlupdatestring + "Rejected" + "',prelevelstatus='Rejected'" +
                                               ",approved_remarks ='',rejected_remarks =" + "'Rejected by Level " + Convert.ToString(applevel).Trim() + "'" +
                                               " where " +
                                               " entry_ty ='" + Convert.ToString(drow["entry_ty"]).Trim() + "'" +
                                               " and tran_cd = " + Convert.ToInt32(drow[keyfld]) + " and applevel =" + Convert.ToString(dr["applevel"]).Trim();

                        }
                        dr.Close();

                        finalstring = finalstring + sqlupdatestring + Convert.ToString(drow["levelstatus"]).Trim() + "',prelevelstatus='" + Convert.ToString(drow["levelstatus"]).Trim() + "'" +
                        //finalstring = finalstring + sqlupdatestring + Convert.ToString(drow["levelstatus"]).Trim() + "',prelevstatus='" + Convert.ToString(drow["levelstatus"]).Trim() + "'" +
                                ",approved_remarks ='" + Convert.ToString(drow["approved_remarks"]).Trim() + "',rejected_remarks ='" +
                                Convert.ToString(drow["rejected_remarks"]).Trim() + "'" +
                                " where " +
                                " entry_ty ='" + Convert.ToString(drow["entry_ty"]).Trim() + "'" +
                                " and tran_cd = " + Convert.ToInt32(drow[keyfld]) + " and applevel =" + applevel;
                       

                    }
                    else
                    {

                        finalstring = finalstring + sqlupdatestring + Convert.ToString(drow["levelstatus"]).Trim() + "'" +
                                                    ",approved_remarks ='" + Convert.ToString(drow["approved_remarks"]).Trim() + "',rejected_remarks ='" +
                                                    Convert.ToString(drow["rejected_remarks"]).Trim() + "'" +
                                                    " where " +
                                                    " entry_ty ='" + Convert.ToString(drow["entry_ty"]).Trim() + "'" +
                                                    " and tran_cd = " + Convert.ToInt32(drow[keyfld]) + " and applevel =" + applevel;

                    }

                   sqlquery = "select top 1 applevel from applevel_dtl where entry_ty ='" + Convert.ToString(drow["entry_ty"]).Trim() + "' and tran_cd=" +
                               + Convert.ToInt32(drow[keyfld]) +
                               " and initiator_id ='" + Convert.ToString(drow["initiator_id"]).Trim() + "' and applevel < " + applevel;

                    int prelevel = 0;
                    dr = cls_Sqlhelper.ExecuteReader(Connectionstring, CommandType.Text, sqlquery);
                    while (dr.Read())
                    {
                        if (dr["applevel"] !=DBNull.Value)
                            prelevel = Convert.ToInt32(dr["applevel"]);
                    }
                    dr.Close();
                    dr.Dispose();

                    if (prelevel > 0)
                        finalstring = finalstring + sqlupdateprelevelstring + Convert.ToString(drow["levelstatus"]).Trim() + 
                           "',prelevel=" + Convert.ToString(applevel).Trim() + 
                           " where " +
                           " entry_ty ='" + Convert.ToString(drow["entry_ty"]).Trim() + "'" +
                           " and tran_cd = " + Convert.ToInt32(drow[keyfld]) + " and applevel =" + prelevel;
                    else
                    {
                        string remarks = string.Empty;
                        string appstatus = "No";

                        if (Convert.ToString(drow["levelstatus"]).Trim() == "Approved")
                        {
                            remarks = Convert.ToString(drow["approved_remarks"]).Trim();
                            appstatus = "Yes";
                        }
                        else if (Convert.ToString(drow["levelstatus"]).Trim() == "Rejected")
                        {
                            remarks = Convert.ToString(drow["rejected_remarks"]).Trim();
                            appstatus = "No";
                        }

                        finalstring = finalstring + updatefinalappstring + " apgen = '" + appstatus + "'" + 
                                       ",u_remarks ='" + remarks + "'" +
                                       ",apgenby='" + userid.Trim() + "' " +
                                       "where " + keyfld.Trim() + " =" + Convert.ToString(drow[keyfld]).Trim() + " ";
                    }

                    // Adding mail details

                    DataRow drLcode = null;
                    if (trantype == "IM" || trantype == "AM")
                        drLcode = (from DataRow rowLcode in m_ds_settings.Tables["Mastcode"].Rows
                                   where Convert.ToString(rowLcode["entry_ty"]).Trim() == Convert.ToString(drow["entry_ty"]).Trim()
                                   select rowLcode).FirstOrDefault();
                    else
                        drLcode = (from DataRow rowLcode in m_ds_settings.Tables["Lcode"].Rows
                                   where Convert.ToString(rowLcode["entry_ty"]).Trim() == Convert.ToString(drow["entry_ty"]).Trim()
                                   select rowLcode).FirstOrDefault();


                    string codename = string.Empty;
                    string appmailsub = string.Empty;

                 
                    codename = Convert.ToString(drLcode["code_nm"]).Trim();
                    appmailsub = Convert.ToString(drLcode["appmailsub"]).Trim();
                    

                    DataRow drLogUser = (from DataRow rowUser in m_ds_settings.Tables["User"].Rows
                                         where Convert.ToString(rowUser["user"]).ToUpper().Trim() == Convert.ToString(userid).Trim().ToUpper()
                                         select rowUser).FirstOrDefault();

                    // if the Level has been rejected, mail has to send on Initiator ID
                    if (prelevel > 0 || prelevel == 0)
                    {
                        if (Convert.ToString(drow["levelstatus"]).Trim() == "Rejected")
                        {
                            // If initiator has not selected
                            if (Convert.ToString(drow["Initiator_id"]).Trim() == "None")
                            {
                                IEnumerable<DataRow> drapplevelMstr = (from DataRow row in dtAppLevel_Mstr.Rows
                                                                       where Convert.ToString(row["initiator_id"]) == "None"
                                                                       select row);

                                foreach (DataRow rowAppLvlMstr in drapplevelMstr)
                                {
                                    if (Convert.ToString(rowAppLvlMstr["type"]) == "Role") // If type is role then mail has sent to all users which are under Role
                                    {
                                        IEnumerable<DataRow> drUser = (from DataRow rowUser in m_ds_settings.Tables["User"].Rows
                                                                       where Convert.ToString(rowUser["user_roles"]).ToUpper().Trim() == Convert.ToString(rowAppLvlMstr["user_role"]).Trim().ToUpper()
                                                                       select rowUser);

                                        foreach (DataRow drowRoles in drUser)
                                        {
                                            if (drowRoles == null)
                                                throw new Exception("Initiator Id. not found in master..");

                                            if (Convert.ToString(drowRoles["user_email"]).Trim() == string.Empty)
                                                throw new Exception("User email Id. not found in User master of this user :" + Convert.ToString(drowRoles["user"]).Trim());

                                            AddListRejectMails(trantype, codename, appmailsub, userid, drow, drLogUser, drowRoles, ListAppMailSetter);
                                        }
                                    }
                                    else
                                    {
                                        // Mail has to sent to user
                                        DataRow drUser = (from DataRow rowUser in m_ds_settings.Tables["User"].Rows
                                                          where Convert.ToString(rowUser["user"]).ToUpper().Trim() == Convert.ToString(rowAppLvlMstr["user_role"]).Trim().ToUpper()
                                                          select rowUser).FirstOrDefault();

                                        if (drUser == null)
                                            throw new Exception("Initiator Id. not found in master..");

                                        if (Convert.ToString(drUser["user_email"]).Trim() == string.Empty)
                                            throw new Exception("User email Id. not found in User master of this user :" + Convert.ToString(drUser["user"]).Trim());

                                        AddListRejectMails(trantype, codename, appmailsub, userid, drow, drLogUser, drUser, ListAppMailSetter);
                                    }
                                }
                            }
                            else
                            {
                                // If Initiator Id has selected then mail has sent to Initiator
                                 DataRow drUser = (from DataRow rowUser in m_ds_settings.Tables["User"].Rows
                                                  where Convert.ToString(rowUser["user"]).ToUpper().Trim() == Convert.ToString(drow["user_name"]).Trim().ToUpper()
                                                  select rowUser).FirstOrDefault();

                                 if (drUser == null)
                                    throw new Exception("Initiator Id. not found in master..");

                                 if (Convert.ToString(drUser["user_email"]).Trim() == string.Empty)
                                     throw new Exception("User email Id. not found in User master of this user :" + Convert.ToString(drUser["user"]).Trim());

                                AddListRejectMails(trantype, codename, appmailsub, userid, drow, drLogUser, drUser, ListAppMailSetter);
                            }
                           
                        }
                    }


                    if (prelevel > 0)
                    {
                        if (Convert.ToString(drow["levelstatus"]).Trim() == "Approved")
                        {
                            IEnumerable<DataRow> drapplevelMstr = (from DataRow row in dtAppLevel_Mstr.Rows
                                                                   where Convert.ToInt32(row["applevel"]) == prelevel &&
                                                                         Convert.ToString(row["initiator_id"]) == Convert.ToString(drow["initiator_id"]).Trim()
                                                                   select row);

                            foreach (DataRow rowApplevel in drapplevelMstr)
                            {
                                
                                // Generate mail line according Transaction and Master
                                string strmailline = string.Empty;
                                if (trantype == "IM")
                                    strmailline = codename + "<b> Item Name :- " + Convert.ToString(drow["it_name"]).Trim() + "</b>";
                                else if (trantype == "AM")
                                    strmailline = codename + "<b> A/c. Name :- " + Convert.ToString(drow["ac_name"]).Trim() + "</b>";
                                else
                                    strmailline = codename + "<b> Inv. No. :- " + Convert.ToString(drow["inv_no"]).Trim() + "</b>";


                                // if in Approval level master type has selected User
                                if (Convert.ToString(rowApplevel["type"]) == "User")
                                {
                                    DataRow drUser = (from DataRow rowUser in m_ds_settings.Tables["User"].Rows
                                                      where Convert.ToString(rowUser["user"]).ToUpper().Trim() == Convert.ToString(rowApplevel["user_role"]).Trim().ToUpper()
                                                      select rowUser).FirstOrDefault();

                                    if (drUser == null)
                                        throw new Exception("User not found in master..");

                                    
                                    if (Convert.ToString(drUser["user_email"]).Trim() == string.Empty)
                                        throw new Exception("User email Id. not found in User master of this user :" + Convert.ToString(drUser["user"]).Trim());


                                    AddListNextApproveMails(trantype, codename, appmailsub, userid, strmailline, drow, drLogUser, drUser, ListAppMailSetter);

                                    //StringBuilder stbMailBody = new StringBuilder();
                                    //stbMailBody.Append("Dear Sir / Madam, \n\n");
                                    //stbMailBody.Append(strmailline + " is pending for your approval,\n");
                                    //stbMailBody.Append("kindly approve the same. \n\n");

                                    //if (Convert.ToString(drow["Approved_remarks"]).Trim() != string.Empty)
                                    //    stbMailBody.Append("Approver Comments :" + Convert.ToString(drow["Approved_remarks"]).Trim() + "\n\n\n");

                                    //stbMailBody.Append("Thanks & Regards,\n");
                                    //stbMailBody.Append((Convert.ToString(drLogUser["user_name"]).Trim() == string.Empty ? userid.Trim() : Convert.ToString(drLogUser["user_name"]).Trim()) + "\n\n\n");
                                    //stbMailBody.Append("Note : This is system Generated Mail, please do not reply to this mail..");

                                }
                                else
                                {
                                    /* if in Approval level master type has selected Role then
                                     * send alert mails to all users which is under this role */

                                    if (Convert.ToString(rowApplevel["type"]) == "Role")
                                    {
                                        IEnumerable<DataRow> drUser = (from DataRow rowUser in m_ds_settings.Tables["User"].Rows
                                                                       where Convert.ToString(rowUser["user_roles"]).ToUpper().Trim() == Convert.ToString(rowApplevel["user_role"]).Trim().ToUpper()
                                                                       select rowUser);

                                        foreach (DataRow drowRoles in drUser)
                                        {
                                            if (Convert.ToString(drowRoles["user_email"]).Trim() == string.Empty)
                                                throw new Exception("User email Id. not found in User master of this user :" + Convert.ToString(drowRoles["user"]).Trim());


                                            AddListNextApproveMails(trantype, codename, appmailsub, userid, strmailline, drow, drLogUser, drowRoles, ListAppMailSetter);

                                            
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else if (prelevel == 0) // Final Approval
                    {
                        if (Convert.ToString(drow["levelstatus"]).Trim() == "Approved")
                        {
                            //IEnumerable<DataRow> drapplevelMstr = null;

                            // If initiator has not selected
                            if (Convert.ToString(drow["Initiator_id"]).Trim() == "None")
                            {
                                IEnumerable<DataRow> drapplevelMstr = (from DataRow row in dtAppLevel_Mstr.Rows
                                                                       where Convert.ToString(row["initiator_id"]) == "None"
                                                                       select row);

                                foreach (DataRow rowAppLvlMstr in drapplevelMstr)
                                {
                                    // In Approval level master role has selected then mail has to sent all respected users
                                    if (Convert.ToString(rowAppLvlMstr["type"]) == "Role")
                                    {
                                        IEnumerable<DataRow> drUser = (from DataRow rowUser in m_ds_settings.Tables["User"].Rows
                                                                       where Convert.ToString(rowUser["user_roles"]).ToUpper().Trim() == Convert.ToString(rowAppLvlMstr["user_role"]).Trim().ToUpper()
                                                                       select rowUser);

                                        foreach (DataRow drowRoles in drUser)
                                        {
                                            if (drowRoles == null)
                                                throw new Exception("Initiator Id. not found in master..");

                                            if (Convert.ToString(drowRoles["user_email"]).Trim() == string.Empty)
                                                throw new Exception("User email Id. not found in User master of this user :" + Convert.ToString(drowRoles["user"]).Trim());

                                            AddListApproveMails(trantype, codename, appmailsub, userid, drow, drLogUser, drowRoles, ListAppMailSetter);
                                        }
                                    }
                                    else
                                    {
                                        // Mail has to sent all users
                                        DataRow drUser = (from DataRow rowUser in m_ds_settings.Tables["User"].Rows
                                                          where Convert.ToString(rowUser["user"]).ToUpper().Trim() == Convert.ToString(rowAppLvlMstr["user_role"]).Trim().ToUpper()
                                                          select rowUser).FirstOrDefault();

                                        if (drUser == null)
                                            throw new Exception("Initiator Id. not found in master..");

                                        if (Convert.ToString(drUser["user_email"]).Trim() == string.Empty)
                                            throw new Exception("User email Id. not found in User master of this user :" + Convert.ToString(drUser["user"]).Trim());

                                        AddListApproveMails(trantype, codename, appmailsub, userid, drow, drLogUser, drUser, ListAppMailSetter);
                                    }
                                }

                            }
                            else
                            {
                                // If top level has been approved then mail has to be sent to Initiator ( if Initiator has been selected in master )
                                DataRow drUser = (from DataRow rowUser in m_ds_settings.Tables["User"].Rows
                                                  where Convert.ToString(rowUser["user"]).ToUpper().Trim() == Convert.ToString(drow["user_name"]).Trim().ToUpper()
                                                  select rowUser).FirstOrDefault();

                                if (drUser == null)
                                    throw new Exception("Initiator Id. not found in master..");

                                if (Convert.ToString(drUser["user_email"]).Trim() == string.Empty)
                                    throw new Exception("User email Id. not found in User master of this user :" + Convert.ToString(drUser["user"]).Trim());

                                AddListApproveMails(trantype, codename, appmailsub, userid, drow, drLogUser, drUser, ListAppMailSetter);
                            }
                        }
                    }


                }
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

            return ListAppMailSetter;
        }

        private void AddListNextApproveMails(string trantype,
                                         string codename,
                                         string appmailsub,
                                         string userid,
                                         string strmailline,
                                         DataRow drow,
                                         DataRow drLogUser,
                                         DataRow drUser,
                                         List<AppMailSetter> ListAppMailSetter)
        {
            StringBuilder stbMailBody = new StringBuilder();
            stbMailBody.Append("<font face='Courier New'>");
            stbMailBody.Append("<p>Dear Sir / Madam,</p>");
            stbMailBody.Append("<p>" + strmailline + " is pending for your approval,</p>");
            stbMailBody.Append("kindly approve the same.");
            stbMailBody.Append("<br>");
            stbMailBody.Append("<br>");

            if (Convert.ToString(drow["Approved_remarks"]).Trim() != string.Empty)
            {
                stbMailBody.Append("<table border='1'>");
                stbMailBody.Append("<tr style='background-color:#C5C8C8'><td><b><font face='Courier New'>    Approver Comments    </font></b></td></tr>");
                stbMailBody.Append("<tr><td><b><font face='Courier New'>" + Convert.ToString(drow["Approved_remarks"]).Trim() + "</font></b></td></tr>");
                stbMailBody.Append("</table>");
            }

            stbMailBody.Append("<br>");
            stbMailBody.Append("Thanks & Regards,<br>");
            stbMailBody.Append((Convert.ToString(drLogUser["user_name"]).Trim() == string.Empty ? userid.Trim() : Convert.ToString(drLogUser["user_name"]).Trim()));
            stbMailBody.Append("<br>");
            stbMailBody.Append("<br>");
            stbMailBody.Append("<p><font size='2'><b><i>Note : This is system Generated Mail, please do not reply to this mail..</i></b></font></p>");
            stbMailBody.Append("</font>");


            ListAppMailSetter.Add(DoAddMails(Convert.ToString(drUser["user_email"]).Trim(),
                                             codename + " " +
                                             (appmailsub == string.Empty ? "Approval notification " :
                                             appmailsub),
                                             stbMailBody.ToString()));
        }

        private void AddListApproveMails(string trantype,
                                         string codename,
                                         string appmailsub,
                                         string userid,
                                         DataRow drow,
                                         DataRow drLogUser,
                                         DataRow drUser,
                                         List<AppMailSetter> ListAppMailSetter)
        {
            StringBuilder stbMailBody = new StringBuilder();
            stbMailBody.Append("<font face='Courier New'>");
            stbMailBody.Append("<p>Dear Sir / Madam,</p>");
            stbMailBody.Append("<p>Please note that <b>" + codename + "</b> has been approved. Details are below.</p>");
            stbMailBody.Append("<br>");

            stbMailBody.Append("<table border='1'>");
            if (trantype == "AM")
                stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  A/c Name   </font></b></td><td><font face='Courier New'>" + Convert.ToString(drow["ac_name"]).Trim() + "</font></td></tr>");
            else if (trantype == "IM")
                stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  Item Name  </font></b></td><td><font face='Courier New'>" + Convert.ToString(drow["it_name"]).Trim() + "</font></td></tr>");
            else
            {
                stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  Inv. No.   </font></b></td><td><font face='Courier New'>" + Convert.ToString(drow["inv_no"]).Trim() + "</font></td></tr>");
                //stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  Party Name   </font></b></td><td><font face='Courier New'>" + Convert.ToString(drow["party_nm"]).Trim() + "</font></td></tr>");
                stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  Party Name   </font></b></td><td><font face='Courier New'>" + Convert.ToString(drow["mailname"]).Trim() + "</font></td></tr>");
            }

            stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  Approved Date   </font></b></td><td><font face='Courier New'>" + DateTime.Now.Date.ToString("dd/MM/yyyy").Trim() + "</font></td></tr>");

            if (Convert.ToString(drow["approved_remarks"]).Trim() != string.Empty)
                stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  Approved Comments   </font></b></td><td><b><font face='Courier New'>" + Convert.ToString(drow["approved_remarks"]).Trim() + "</font></b></td></tr>");

            
            stbMailBody.Append("</table>");

            stbMailBody.Append("<br>");
            stbMailBody.Append("<br>");
            stbMailBody.Append("Thanks & Regards,<br>");
            stbMailBody.Append((Convert.ToString(drLogUser["user_name"]).Trim() == string.Empty ? userid.Trim() : Convert.ToString(drLogUser["user_name"]).Trim()));
            stbMailBody.Append("<br>");
            stbMailBody.Append("<br>");
            stbMailBody.Append("<p><font size='2'><b><i>Note : This is system Generated Mail, please do not reply to this mail..</i></b></font></p>");
            stbMailBody.Append("</font>");
            //StringBuilder stbMailBody = new StringBuilder();
            //stbMailBody.Append("Dear Sir / Madam, \n\n");
            //stbMailBody.Append("Please note that " + codename + " has been approved. Details are below.\n\n");

            //if (trantype == "AM")
            //    stbMailBody.Append("A/c Name :" + Convert.ToString(drow["ac_name"]).Trim() + "\n");
            //else if (trantype == "IM")
            //    stbMailBody.Append("Item Name :" + Convert.ToString(drow["it_name"]).Trim() + "\n");
            //else
            //{
            //    stbMailBody.Append("Inv. No. :" + Convert.ToString(drow["inv_no"]).Trim() + "\n");
            //    stbMailBody.Append("Party Name :" + Convert.ToString(drow["party_nm"]).Trim() + "\n");
            //}

            //stbMailBody.Append("Approved Date :" + DateTime.Now.Date.ToString("dd/MM/yyyy").Trim() + "\n");

            //if (Convert.ToString(drow["approved_remarks"]).Trim() !=string.Empty)
            //    stbMailBody.Append("Approved Comments :" + Convert.ToString(drow["approved_remarks"]).Trim() + "\n\n");
            

            //stbMailBody.Append("Thanks & Regards,\n");
            //stbMailBody.Append((Convert.ToString(drLogUser["user_name"]).Trim() == string.Empty ? userid.Trim() : Convert.ToString(drLogUser["user_name"]).Trim()) + "\n\n\n");
            //stbMailBody.Append("Note : This is system Generated Mail, please do not reply to this mail..");

            ListAppMailSetter.Add(DoAddMails(Convert.ToString(drUser["user_email"]).Trim(),
                                             codename + " " +
                                             (appmailsub == string.Empty ? "Approval notification " :
                                             appmailsub),
                                             stbMailBody.ToString()));
        }

        private void AddListRejectMails(string trantype,
                                      string codename,
                                      string appmailsub,
                                      string userid,
                                      DataRow drow,
                                      DataRow drLogUser,
                                      DataRow drUser,
                                      List<AppMailSetter> ListAppMailSetter)
        {

            StringBuilder stbMailBody = new StringBuilder();
            stbMailBody.Append("<font face='Courier New'>");
            stbMailBody.Append("<p>Dear Sir / Madam,</p>");
            stbMailBody.Append("<p>Please note that " + codename + " has been rejected. Details are below.</p>");
            stbMailBody.Append("<br>");

            stbMailBody.Append("<table border='1'>");
            if (trantype == "AM")
                stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  A/c Name   </font></b></td><td><font face='Courier New'>" + Convert.ToString(drow["ac_name"]).Trim() + "</font></td></tr>");
            else if (trantype == "IM")
                stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  Item Name   </font></b></td><td><font face='Courier New'>" + Convert.ToString(drow["it_name"]).Trim() + "</font></td></tr>");
            else
            {
                stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  Inv. No.   </font></b></td><td><font face='Courier New'>" + Convert.ToString(drow["inv_no"]).Trim() + "</font></td></tr>");
                //stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  Party Name   </font></b></td><td><font face='Courier New'>" + Convert.ToString(drow["party_nm"]).Trim() + "</font></td></tr>");
                stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  Party Name   </font></b></td><td><font face='Courier New'>" + Convert.ToString(drow["mailname"]).Trim() + "</font></td></tr>");
            }

            stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  Rejected Date   </font></b></td><td><font face='Courier New'>" + DateTime.Now.Date.ToString("dd/MM/yyyy").Trim() + "</font></td></tr>");

            if (Convert.ToString(drow["rejected_remarks"]).Trim() != string.Empty)
                stbMailBody.Append("<tr><td style='background-color:#C5C8C8'><b><font face='Courier New'>  Rejected Comments   </font></b></td><td><b><font face='Courier New'>" + Convert.ToString(drow["rejected_remarks"]).Trim() + "</font></b></td></tr>");
            stbMailBody.Append("</table>");

            stbMailBody.Append("<br>");
            stbMailBody.Append("<br>");
            stbMailBody.Append("Thanks & Regards,<br>");
            stbMailBody.Append((Convert.ToString(drLogUser["user_name"]).Trim() == string.Empty ? userid.Trim() : Convert.ToString(drLogUser["user_name"]).Trim()));
            stbMailBody.Append("<br>");
            stbMailBody.Append("<br>");
            stbMailBody.Append("<p><font size='2'><b><i>Note : This is system Generated Mail, please do not reply to this mail..</i></b></font></p>");
            stbMailBody.Append("</font>");

            //StringBuilder stbMailBody = new StringBuilder();
            //stbMailBody.Append("Dear Sir / Madam, \n\n");
            //stbMailBody.Append("Please note that " + codename + "  has been Rejected. Details are below.\n\n");

            //if (trantype == "AM")
            //    stbMailBody.Append("A/c Name :" + Convert.ToString(drow["ac_name"]).Trim() + "\n");
            //else if (trantype == "IM")
            //    stbMailBody.Append("Item Name :" + Convert.ToString(drow["it_name"]).Trim() + "\n");
            //else
            //{
            //    stbMailBody.Append("Inv. No. :" + Convert.ToString(drow["inv_no"]).Trim() + "\n");
            //    stbMailBody.Append("Party Name :" + Convert.ToString(drow["party_nm"]).Trim() + "\n");
            //}
            //stbMailBody.Append("Rejected Date :" + DateTime.Now.Date.ToString("dd/MM/yyyy").Trim() + "\n");
            //stbMailBody.Append("Rejected Comments :" + Convert.ToString(drow["rejected_remarks"]).Trim() + "\n\n");

            //stbMailBody.Append("Thanks & Regards,\n");
            //stbMailBody.Append((Convert.ToString(drLogUser["user_name"]).Trim() == string.Empty ? userid.Trim() : Convert.ToString(drLogUser["user_name"]).Trim()) + "\n\n\n");
            //stbMailBody.Append("Note : This is system Generated Mail, please do not reply to this mail.");

            ListAppMailSetter.Add(DoAddMails(Convert.ToString(drUser["user_email"]).Trim(),
                                    codename + " " +
                                    (appmailsub == string.Empty ? "Approval notification " :
                                    appmailsub),
                                    stbMailBody.ToString()));
        }

        private AppMailSetter DoAddMails(string toids,
                                string subject,
                                string mailbody)
        {
            AppMailSetter appmailsetter = new AppMailSetter();
            appmailsetter.Toids = toids;
            appmailsetter.Subject = subject;
            appmailsetter.Mailbody = mailbody;

            return appmailsetter;
        }


        public DataTable GetEmailCredentials()
        {
            string sqlquery = "select * from emailsettings ";

            DataSet dsEmailSettings = cls_Sqlhelper.ExecuteDataset(Connectionstring, CommandType.Text, sqlquery);
            dsEmailSettings.Tables[0].TableName = "emailSettings";
            return dsEmailSettings.Tables[0];
        }


        public void Save(DataTable dtMast,
                       string trantype,
                       string keyfld,
                       string userid)
        {
            string sqlinsertstring = "Insert into Applevel_Dtl(entry_ty,tran_cd,applevel,user_role,add_user,add_dt) values('";
            
            string tblname = string.Empty;
            if (trantype == "IM")
                tblname = "It_mast";
            else
                tblname = "Ac_Mast";

            string updatefinalappstring = "update " + tblname.Trim() + " set apgen = 'Yes' ";
            string deletestring = "delete from applevel_dtl where entry_ty ='";
            string finalstring = string.Empty;
            foreach (DataRow mastRow in dtMast.Select("Ischanged = 1"))
            {
                if (Convert.ToBoolean(mastRow["Approve"]) == false)
                {
                    DataTable dtAppLevelDtl = m_ds_applevel.Tables["AppLevel_Dtl"];
                    var rowappleveldtl = (from DataRow rowAppleveldtl in dtAppLevelDtl.Rows
                                          where rowAppleveldtl["entry_ty"].ToString().Trim() == trantype &&
                                                Convert.ToInt32(rowAppleveldtl["tran_cd"]) == Convert.ToInt32(mastRow[keyfld]) &&
                                                 Convert.ToInt32(rowAppleveldtl["applevel"]) == Convert.ToInt32(mastRow["applevel"])
                                          select rowAppleveldtl).FirstOrDefault();

                    if (rowappleveldtl != null)
                    {
                        finalstring = finalstring + deletestring + trantype + "' and tran_cd =" + Convert.ToInt32(mastRow[keyfld]) + " ";
                    }

                    continue;

                }

                if (Convert.ToInt32(mastRow["Currlevel"]) == 1)
                    finalstring = finalstring + updatefinalappstring +
                        ",u_remarks ='" + Convert.ToString(mastRow["U_remarks"]).Trim() + "'" +
                        ",apgenby='" + Convert.ToString(mastRow["Loguser"]) + "' " +
                        "where " + keyfld.Trim() +"=" + Convert.ToInt32(mastRow[keyfld]) + " ";

                finalstring = finalstring + sqlinsertstring + trantype + "'," +
                                                              Convert.ToInt32(mastRow[keyfld]) + "," +
                                                              Convert.ToInt32(mastRow["Currlevel"]) + ",'" +
                                                              Convert.ToString(mastRow["Loguser"]) + "','" +
                                                              userid + "','" +
                                                              DateTime.Now.ToString().Trim() + "')";

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

    }
}
