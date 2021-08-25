using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLA.ENTITY
{
    public class ItemDet
    {
        private int item_no;
        public int Item_no
        {
            get { return item_no; }
            set { item_no = value; }
        }

        private string item;
        public string Item
        {
            get { return item; }
            set { item = value; }
        }

        private decimal qty;
        public decimal Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        private decimal rate;
        public decimal Rate
        {
            get { return rate; }
            set { rate = value; }
        }

        private decimal gro_amt;
        public decimal Gro_amt
        {
            get { return gro_amt; }
            set { gro_amt = value; }
        }

        private int tran_cd;
        public int Tran_cd
        {
            get { return tran_cd; }
            set { tran_cd = value; }
        }

        private decimal re_qty;
        public decimal Re_qty
        {
            get { return re_qty; }
            set { re_qty = value; }
        }
    }
}
