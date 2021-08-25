using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MLA.ENTITY;
using MLA.DL;

namespace MLA.LL
{
    public class cls_Gen_Ent_Approval
    {
        public cls_Gen_Ent_Approval()
        {
        }

        private DataSet dsSettings;
        public DataSet DsSettings
        {
            get { return dsSettings; }
            set { dsSettings = value; }
        }

        private DataSet dsApplevel;
        public DataSet DsApplevel
        {
            get { return dsApplevel; }
            set { dsApplevel = value; }
        }

       
        private string connectionstring;
        public string Connectionstring
        {
            get { return connectionstring; }
            set { connectionstring = value; }
        }

        private List<AppMailSetter> listAppmailSetter;
        public List<AppMailSetter> ListAppmailSetter
        {
            get { return listAppmailSetter; }
            set { listAppmailSetter = value; }
        }

    }
}
