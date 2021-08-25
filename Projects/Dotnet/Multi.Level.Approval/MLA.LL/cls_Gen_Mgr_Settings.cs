using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MLA.DL;

namespace MLA.LL
{
    public class cls_Gen_Mgr_Settings : cls_Gen_Ent_Settings
    {
        public cls_Gen_Mgr_Settings(string connectionstring)
        {
            m_obj_settings.Connectionstring = connectionstring;
        }

        cls_Settings m_obj_settings = new cls_Settings();
       
        public DataSet GetAllInitialData(int compid)
        {
            DsSettings = m_obj_settings.GetAllInitialData(compid);
            return DsSettings;
        }

        public DataSet GetAppLevel_Mstr()
        {
            DsApplevel_Mstr = m_obj_settings.GetAllAppLevel_Mstr(Trantype,Initiator);
            return DsApplevel_Mstr;
        }

        public void Save()
        {
            m_obj_settings.Save(UserID,Active);
        }


    }
}
