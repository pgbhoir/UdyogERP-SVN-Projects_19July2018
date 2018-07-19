using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ITC_Mismatch_DetailsProperties
{
    public class PubClassProp
    {
        private DataTable _dtExcelData;

        public DataTable DtExcelData
        {
            get
            {
                return _dtExcelData;
            }

            set
            {
                _dtExcelData = value;
            }
        }
    }
}
