using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace ugstEWayBill
{
    class csEWAYBillJson
    {
        public string userGstin { get; set; }
        public string supplyType { get; set; }
        public string subSupplyType { get; set; }
        public string docType { get; set; }
        public string docNo { get; set; }
        public string docDate { get; set; }
        public string fromGstin { get; set; }
        public string fromTrdName { get; set; }
        public string fromAddr1 { get; set; }
        public string fromAddr2 { get; set; }
        public string fromPlace { get; set; }
        public string fromPincode { get; set; }
        public string fromStateCode { get; set; }
        public string toGstin { get; set; }
        public string toTrdName { get; set; }
        public string toAddr1 { get; set; }
        public string toAddr2 { get; set; }
        public string toPlace { get; set; }
        public string toPincode { get; set; }
        public string toStateCode { get; set; }
        public string totalValue { get; set; }
        public string cgstValue { get; set; }
        public string sgstValue { get; set; }
        public string igstValue { get; set; }
        public string cessValue { get; set; }
        public string transMode { get; set; }
        public string transDistance { get; set; }
        public string transporterName { get; set; }
        public string transporterId { get; set; }
        public string transDocNo { get; set; }
        public string transDocDate { get; set; }
        public string vehicleNo { get; set; }
        public clsItemList itemList { get; set; }
    }
    public class clsItemList
    {
        public int  itemNo { get; set; }
        public string productName { get; set; }
        public string productDesc { get; set; }
        public string hsnCode { get; set; }
        public decimal quantity { get; set; }
        public decimal qtyUnit { get; set; }
        public decimal taxableAmount { get; set; }
        public decimal sgstRate { get; set; }
        public decimal cgstRate { get; set; }
        public decimal igstRate { get; set; }
        public decimal cessRate { get; set; }
    }

}
