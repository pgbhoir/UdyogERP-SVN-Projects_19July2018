using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CustModAccUI.BLL
{
    public class cls_Gen_Ent_ConnectVFP
    {
        private DataSet dsConvert;

        public DataSet DsConvert
        {
            get { return dsConvert; }
            set { dsConvert = value; }
        }

        private string connString;

        public string ConnString
        {
            get { return connString; }
            set { connString = value; }
        }
    }
}
