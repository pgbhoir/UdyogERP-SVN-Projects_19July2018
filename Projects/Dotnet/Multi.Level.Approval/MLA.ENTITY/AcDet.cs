using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLA.ENTITY
{
    public class AcDet
    {
        private string entry_ty;
        public string Entry_ty
        {
            get { return entry_ty; }
            set { entry_ty = value; }
        }

        private int tran_cd;
        public int Tran_cd
        {
            get { return tran_cd; }
            set { tran_cd = value; }
        }

        private string ac_name;
        public string Ac_name
        {
            get { return ac_name; }
            set { ac_name = value; }
        }

        private string amt_ty;
        public string Amt_ty
        {
            get { return amt_ty; }
            set { amt_ty = value; }
        }

        private decimal amount;
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
    }
}
