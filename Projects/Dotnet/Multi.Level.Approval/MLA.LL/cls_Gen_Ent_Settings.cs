using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MLA.LL
{
    public class cls_Gen_Ent_Settings
    {
        public cls_Gen_Ent_Settings()
        {
        }

        private string trantype;
        public string Trantype
        {
            get { return trantype; }
            set { trantype = value; }
        }

        private DataSet dsSettings;
        public DataSet DsSettings
        {
            get { return dsSettings; }
            set { dsSettings = value; }
        }

        private DataSet dsApplevel_Mstr;
        public DataSet DsApplevel_Mstr
        {
            get { return dsApplevel_Mstr; }
            set { dsApplevel_Mstr = value; }
        }

        private string connectionstring;
        public string Connectionstring
        {
            get { return connectionstring; }
            set { connectionstring = value; }
        }

        private string userID;
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        private string initiator;
        public string Initiator
        {
            get { return initiator; }
            set { initiator = value; }
        }

        private bool active;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
    
    }
}
