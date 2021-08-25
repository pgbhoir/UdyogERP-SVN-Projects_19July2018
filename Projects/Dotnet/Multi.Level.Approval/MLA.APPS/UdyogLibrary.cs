using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Udyog.Library.Common;
namespace MLA.DL
{
    public class UdyogLibrary
    {
        public UdyogLibrary()
        { }

        public string GetDecrytedValue(string key, string value)
        {
            return Udyog.Library.Common.VU_UDFS.NewDECRY(Udyog.Library.Common.VU_UDFS.HexDecimalToASCII(key), value);
        }

    }
}
