using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CustModAccUI.BLL
{
    public class cls_Gen_Ent_Read_XML
    {
        #region Properties
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string xmlpath;

        public string Xmlpath
        {
            get { return xmlpath; }
            set { xmlpath = value; }
        }

        private string regcomp;

        public string Regcomp
        {
            get { return regcomp; }
            set { regcomp = value; }
        }

        private string date;

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        private string prodname;

        public string Prodname
        {
            get { return prodname; }
            set { prodname = value; }
        }

        private string prodver;

        public string Prodver
        {
            get { return prodver; }
            set { prodver = value; }
        }

        private string macid;

        public string Macid
        {
            get { return macid; }
            set { macid = value; }
        }

        private DataSet dsMain;

        public DataSet DsMain
        {
            get { return dsMain; }
            set { dsMain = value; }
        }
        #endregion
    }
}
