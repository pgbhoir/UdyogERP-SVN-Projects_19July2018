using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CustModAccUI.BLL
{
    public class cls_Gen_Ent_Upload_Script
    {
        #region Properties
        private DataSet dsExecute;

        public DataSet DsExecute
        {
            get { return dsExecute; }
            set { dsExecute = value; }
        }

        private string connString;

        public string ConnString
        {
            get { return connString; }
            set { connString = value; }
        }
        //Added by Priyanka on 31122013 start
        private string vumess;

        public string Vumess
        {
            get { return vumess; }
            set { vumess = value; }
        }
        //Added by Priyanka on 31122013 end
        #endregion
    }
}
