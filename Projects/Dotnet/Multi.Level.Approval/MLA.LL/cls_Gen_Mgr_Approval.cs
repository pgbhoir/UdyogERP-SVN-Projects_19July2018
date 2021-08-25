using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MLA.DL;
using MLA.ENTITY;

namespace MLA.LL
{
    public class cls_Gen_Mgr_Approval : cls_Gen_Ent_Approval
    {
        public bool PvEmailExists = true;  //Added By Kishor A. for Bug-21965 on-08-11-2016
        cls_Approval m_obj_Approval = new cls_Approval();        
        public cls_Gen_Mgr_Approval(string connectionstring)
        {
            m_obj_Approval.Connectionstring = connectionstring;            
        }

        public DataSet GetAllInitialData(int compid)
        {
            DsSettings = m_obj_Approval.GetAllInitialData(compid);
            return DsSettings;
        }

        public DataTable GetMainList(string trantype,
                                       DateTime startdate,
                                       DateTime enddate,
                                       string userid)
        {
            return m_obj_Approval.GetMainList(trantype, startdate, enddate, userid);
        }

        public DataTable GetApplevelMstr(string trantype)
        {
            return m_obj_Approval.GetApplevelMstr(trantype);
        }


        public List<ItemDet> GetItemDataList(string trantype, int tran_cd)
        {
           return m_obj_Approval.GetItemDataList(trantype, tran_cd);
        }

        public List<AcDet> GetAccountDataList(string trantype, int tran_cd)
        {
            return m_obj_Approval.GetAccountDataList(trantype, tran_cd);
        }

        public DataSet GetAppLevelDtl(string trantype)
        {
            DsApplevel = m_obj_Approval.GetAppLevelDtl(trantype);
            return DsApplevel;
        }

        public void GetFilterDataAsPerInitiator(List<Main> lstmainlist,
                                                DataTable dtApplevelMstr)
        {
            string strtran_cd = string.Empty;
            foreach (Main main in lstmainlist)
            {
                // search initiator id in Applevel Master 
                var applevelMstr = (from applevel_mstr in dtApplevelMstr.AsEnumerable()
                                                    where Convert.ToString(applevel_mstr["entry_ty"]).Trim() == main.Entry_ty.Trim() &&
                                                          Convert.ToString(applevel_mstr["initiator_id"]).Trim().ToUpper() == main.User_name.Trim().ToUpper()
                                                          select applevel_mstr).FirstOrDefault();


                 if (applevelMstr == null)
                     strtran_cd += (strtran_cd == string.Empty ? "" : ",") + main.Tran_cd.ToString().Trim();
            }

            if (strtran_cd != string.Empty)
            {
                foreach (string str in strtran_cd.Split(','))
                {
                    lstmainlist.RemoveAll(m => m.Tran_cd == Convert.ToInt32(str));
                }
            }
        }

        public void GetFilterDataAsPerLevel(string userid,
                                            List<Main> lstmainlist)
        {
            DataRow userMstr = (from UserMstr in DsSettings.Tables["User"].AsEnumerable()
                                where Convert.ToString(UserMstr["user"]).Trim().ToUpper() ==
                                userid.Trim().ToUpper()
                                select UserMstr).FirstOrDefault();

            string UserRoles = string.Empty;
            if (userMstr != null)
                UserRoles = userMstr.Field<string>("User_Roles").ToUpper();


            string strtran_cd = string.Empty;
            foreach (Main main in lstmainlist)
            {
                DataRow rowappLevel = (from DataRow drowmstr in DsSettings.Tables["AppLevel_Mstr"].Rows
                                       where Convert.ToString(drowmstr["entry_ty"]).Trim() == main.Entry_ty &&
                                            Convert.ToString(drowmstr["initiator_id"]).Trim() == main.User_name.Trim() &&
                                            Convert.ToString(drowmstr["user_role"]).ToUpper().Trim() == userid.ToUpper().Trim()
                                       select drowmstr).FirstOrDefault();

                if (rowappLevel == null)
                {
                    rowappLevel = (from DataRow drowmstr in DsSettings.Tables["AppLevel_Mstr"].Rows
                                   where Convert.ToString(drowmstr["entry_ty"]).Trim() == main.Entry_ty &&
                                        Convert.ToString(drowmstr["initiator_id"]).Trim() == main.User_name.Trim() &&
                                        Convert.ToString(drowmstr["user_role"]) == UserRoles.Trim()
                                   select drowmstr).FirstOrDefault();
                }

                int currloguserlevel = 0;
                if (rowappLevel != null)
                    currloguserlevel = Convert.ToInt32(rowappLevel["applevel"]);

                if (currloguserlevel == 0)
                    strtran_cd += (strtran_cd == string.Empty ? "" : ",") + main.Tran_cd.ToString().Trim();

            }

            if (strtran_cd != string.Empty)
            {
                foreach (string str in strtran_cd.Split(','))
                {
                    lstmainlist.RemoveAll(m => m.Tran_cd == Convert.ToInt32(str));
                }
            }        
        }

        public DataTable GetMasterData(string trantype, string userid)
        {
            return m_obj_Approval.GetMasterData(trantype, userid);
        }

        public DataTable GetEmailCredentials()
        {
            return m_obj_Approval.GetEmailCredentials();
        }

        public void Save(DataTable dtView,
                         DataTable dtApplevelMstr,
                         string trantype,
                         string userid)
        {
            ListAppmailSetter = m_obj_Approval.Save(dtView, dtApplevelMstr, trantype, userid,
               (trantype == "IM" ? "it_code" : trantype == "AM" ? "Ac_id" : "tran_cd"));
            PvEmailExists = m_obj_Approval.PvEmailExists;  //Added By Kishor A. for Bug-21965 on-08-11-2016
        }

   
    }
}
