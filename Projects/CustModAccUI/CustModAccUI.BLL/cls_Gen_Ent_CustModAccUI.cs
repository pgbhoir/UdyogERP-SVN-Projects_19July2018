using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CustModAccUI.BLL
{
    public class cls_Gen_Ent_CustModAccUI
    {
        #region Properties
        private string connString;

        public string ConnString
        {
            get { return connString; }
            set { connString = value; }
        }

        private DataSet dsSearch;

        public DataSet DsSearch
        {
            get { return dsSearch; }
            set { dsSearch = value; }
        }

        private DataSet dsMain;

        public DataSet DsMain
        {
            get { return dsMain; }
            set { dsMain = value; }
        }

        private DataSet dsDetail;

        public DataSet DsDetail
        {
            get { return dsDetail; }
            set { dsDetail = value; }
        }

        private DataSet dsConvert;

        public DataSet DsConvert
        {
            get { return dsConvert; }
            set { dsConvert = value; }
        }

        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private string rcomp;

        public string Rcomp
        {
            get { return rcomp; }
            set { rcomp = value; }
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

        private string rmacid;

        public string Rmacid
        {
            get { return rmacid; }
            set { rmacid = value; }
        }

        private string ccomp;

        public string Ccomp
        {
            get { return ccomp; }
            set { ccomp = value; }
        }

        private string optiontype;

        public string Optiontype
        {
            get { return optiontype; }
            set { optiontype = value; }
        }

        private string desc1;

        public string Desc1
        {
            get { return desc1; }
            set { desc1 = value; }
        }

        private string desc2;

        public string Desc2
        {
            get { return desc2; }
            set { desc2 = value; }
        }

        private string desc3;

        public string Desc3
        {
            get { return desc3; }
            set { desc3 = value; }
        }

        private string bug;

        public string Bug
        {
            get { return bug; }
            set { bug = value; }
        }

        private string pono;

        public string Pono
        {
            get { return pono; }
            set { pono = value; }
        }

        private DateTime podate;

        public DateTime Podate
        {
            get { return podate; }
            set { podate = value; }
        }

        private decimal poamt;

        public decimal Poamt
        {
            get { return poamt; }
            set { poamt = value; }
        }

        private string apprby;

        public string Apprby
        {
            get { return apprby; }
            set { apprby = value; }
        }

        private string remarks;

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        private string user_name;

        public string User_name
        {
            get { return user_name; }
            set { user_name = value; }
        }

        private int range;

        public int Range
        {
            get { return range; }
            set { range = value; }
        }

        private string aPath;

        public string APath
        {
            get { return aPath; }
            set { aPath = value; }
        }

        private string icoPath;

        public string IcoPath
        {
            get { return icoPath; }
            set { icoPath = value; }
        }

        private string vumess;

        public string Vumess
        {
            get { return vumess; }
            set { vumess = value; }
        }
        #endregion
    }
}
