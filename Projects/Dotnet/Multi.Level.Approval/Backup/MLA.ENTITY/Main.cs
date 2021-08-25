using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLA.ENTITY
{
    public class Main
    {
        private string entry_ty;
        public string Entry_ty
        {
            get { return entry_ty; }
            set { entry_ty = value; }
        }

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        private string inv_no;
        public string Inv_no
        {
            get { return inv_no; }
            set { inv_no = value; }
        }
        private string inv_sr;
        public string Inv_sr
        {
            get { return inv_sr; }
            set { inv_sr = value; }
        }
        private string dept;
        public string Dept
        {
            get { return dept; }
            set { dept = value; }
        }
        private string cate;
        public string Cate
        {
            get { return cate; }
            set { cate = value; }
        }
        private string party_nm;
        public string Party_nm
        {
            get { return party_nm; }
            set { party_nm = value; }
        }
        private decimal net_amt;
        public decimal Net_amt
        {
            get { return net_amt; }
            set { net_amt = value; }
        }
        private string apgenby;
        public string Apgenby
        {
            get { return apgenby; }
            set { apgenby = value; }
        }
        private string apgen;
        public string Apgen
        {
            get { return apgen; }
            set { apgen = value; }
        }
        private string u_remarks;
        public string U_remarks
        {
            get { return u_remarks; }
            set { u_remarks = value; }
        }
        private int tran_cd;
        public int Tran_cd
        {
            get { return tran_cd; }
            set { tran_cd = value; }
        }
        private string user_name;
        public string User_name
        {
            get { return user_name; }
            set { user_name = value; }
        }

        private string levelstatus;
        public string Levelstatus
        {
            get { return levelstatus; }
            set { levelstatus = value; }
        }

        private int lastlevel;
        public int Lastlevel
        {
            get { return lastlevel; }
            set { lastlevel = value; }
        }

        private bool ischanged;
        public bool Ischanged
        {
            get { return ischanged; }
            set { ischanged = value; }
        }

        private int currlevel;
        public int Currlevel
        {
            get { return currlevel; }
            set { currlevel = value; }
        }

        private string loguser;
        public string Loguser
        {
            get { return loguser; }
            set { loguser = value; }
        }

        private bool approve;
        public bool Approve
        {
            get { return approve; }
            set { approve = value; }
        }

        private string approved_Remarks;
        public string Approved_Remarks
        {
            get { return approved_Remarks; }
            set { approved_Remarks = value; }
        }

        private string rejected_Remarks;
        public string Rejected_Remarks
        {
            get { return rejected_Remarks; }
            set { rejected_Remarks = value; }
        }

    }
}
