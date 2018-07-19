using System;
using System.Collections.Generic;
using System.Text;

namespace Udyog.Library.Common
{
    public class VU_UDFS
    {
        public VU_UDFS()
        {

        }
        public static string NewENCRY(string data, string key)
        {
            var s = new List<int>();
            var j = 0;

            var x = 0;
            var res = string.Empty;

            for (var i = 0; i < 256; i++)
            {
                s.Add(i);
            }

            for (var i = 0; i < 256; i++)
            {
                var AsciiVal = Asc(key.Substring((i % key.Length), 1));

                j = (j + s[i] + AsciiVal) % 256;
                x = s[i];
                s[i] = s[j];
                s[j] = x;
            }

            var f = 0;
            j = 0;
            for (var y = 0; y < data.Length; y++)
            {
                f = (f + 1) % 256;
                j = (j + s[f]) % 256;
                x = s[f];
                s[f] = s[j];
                s[j] = x;

                var AsciiVal = Asc(data.Substring(y, 1));
                var convStringOperationApplied = AsciiVal ^ s[(s[f] + s[j]) % 256];

                var charX = Chr(convStringOperationApplied);
                var val = charX.ToString();
                res += val;

            }

            return res;
        }

        public static string NewDECRY(string data, string key)
        {
            return NewENCRY(data, key);
        }

        /// <summary>
        /// This method is used for getting the character of the value
        /// </summary>
        /// <param name="intByte"></param>
        /// <returns></returns>
        public static string Chr(int intByte)
        {
            byte[] bytBuffer = new byte[] { (byte)intByte };
            return Encoding.GetEncoding(1252).GetString(bytBuffer);
        }
        /// <summary>
        /// This method is used for getting the ascii value of the character
        /// </summary>
        /// <param name="strChar"></param>
        /// <returns></returns>
        public static int Asc(string strChar)
        {
            char[] chrBuffer = { Convert.ToChar(strChar) };
            byte[] bytBuffer = Encoding.GetEncoding(1252).GetBytes(chrBuffer);
            return (int)bytBuffer[0];
        }
        public static string enc(string strToenc)
        {
            int d = 0;
            int F = strToenc.Length;
            string Repl = string.Empty;
            string r, two;
            int rep = 0, Change;
            while (F > 0)
            {
                r = strToenc.Substring(d, 1);
                Change = Asc(r) + rep;
                two = Chr(Change);
                Repl = Repl + two;
                d = d + 01;
                rep = rep + 1;
                F = F - 1;
            }
            return Repl;
        }
        public static string dec(string strTodec)
        {
            int d = 0;
            int F = strTodec.Length;
            string Repl = string.Empty;
            string r, two = "";
            int rep = 0, Change;
            while (F > 0)
            {
                r = strTodec.Substring(d, 1);
                Change = Asc(r) - rep;
                if (Change > 0)
                    two = Chr(Change);
                Repl = Repl + two;
                d = d + 01;
                F = F - 1;
                rep = rep + 1;
            }
            return Repl;
        }
        public static string onencrypt(string strToOnEnc)
        {
            string lcreturn = string.Empty;
            for (int i = 0; i < strToOnEnc.Length; i++)
            {
                lcreturn = lcreturn + Chr(Asc(strToOnEnc[i].ToString())) + Asc(strToOnEnc[i].ToString());
            }
            return lcreturn;
        }
        public static string ondecrypt(string strToOnDec)
        {
            string lcreturn = string.Empty;
            for (int i = 0; i < strToOnDec.Length; i++)
            {
                lcreturn = lcreturn + Chr(Asc(strToOnDec[i].ToString()) / 2);
            }
            return lcreturn;
        }

        //Added by Shrikant S. on 26/04/2018 for Bug-31488      //Start
        public static string HexDecimalToASCII(string hexString)
        {
            string res = String.Empty;
            for (int a = 0; a < hexString.Length; a = a + 2)
            {
                string Char2Convert = hexString.Substring(a, 2);
                int n = Convert.ToInt32(Char2Convert, 16);
                //char c = (char)n;
                //res += c.ToString();
                res += Chr(n);
            }
            return res;
        }

        public static string ASCIIToHexDecimal(string asciiString)
        {
            StringBuilder sb = new StringBuilder();
            //byte[] inputBytes = Encoding.UTF8.GetBytes(asciiString);
            //            foreach (byte b in inputBytes)
            //{
            //    sb.Append(string.Format("{0:x2}", b));
            //}
            foreach (char c in asciiString)
            {
                sb.Append(String.Format("{0:X}", Asc(c.ToString())));
            }
            return sb.ToString();
        }
        //Added by Shrikant S. on 26/04/2018 for Bug-31488      //End

        /// <summary>
        /// This method is used for getting product code from Company Master
        /// </summary>
        /// <param name="code">String to decrypt</param>
        /// <returns></returns>
        public static string GetDecProductCode(string code)
        {
            string decryptedStr = string.Empty;
            for (int i = 0; i < code.Length; i++)
            {
                decryptedStr = decryptedStr + Chr(Asc(code[i].ToString()) / 2);
            }
            return decryptedStr;
        }

        public static string GetEncProductCode(string ProdCode)
        {
            //ProdCode = ProdCode.Replace(",", "");
            string retProd = string.Empty;
            for (int i = 0; i < ProdCode.Length; i++)
            {
                retProd = retProd + Chr(Asc(ProdCode[i].ToString()) * 2);
            }
            return retProd;
        }

        public static string RetAppEnc(string appName)
        {
            string retValue = string.Empty;
            retValue = NewENCRY(enc(appName), "Ud*yog+1993");
            return retValue;
        }
        public static string RetModuleDec(string moduleName)
        {
            string retValue = string.Empty;
            retValue = NewDECRY(moduleName, "Udyog!Module!Mast");
            return retValue;
        }
        public static string RetFeatureDec(string featureName)
        {
            string retValue = string.Empty;
            retValue = NewDECRY(featureName, "Udencyogprod");
            return retValue;
        }
        public static string RetVersion(string appName)
        {
            string retValue = string.Empty;
            retValue = "10.0.0.0";
            return retValue;
        }
        public static string RetEncValue(string encString, string keyValue)
        {
            string retValue = string.Empty;
            retValue = NewENCRY(encString, keyValue);
            return retValue;
        }
        public static string RetShortCode(string appName)
        {
            string retValue = string.Empty;
            retValue = RetProdProperty(appName, "UdProdShortCode");
            return retValue;
        }
        public static string RetProduct(string appName)
        {
            string retValue = string.Empty;
            retValue = RetProdProperty(appName, "MainProduct");
            return retValue;
        }

        public static string RetProdProperty(string appName, string appProperty)
        {
            string retValue = string.Empty;

            int retIndex = 0;
            switch (appProperty)
            {
                case "UdProdShortCode":
                    retIndex = 1;
                    break;
                case "MainProduct":
                    retIndex = 2;
                    break;
                default:
                    break;
            }
            string[,] prodArray = new string[,] { 
                                    { "USquare", "U5","" }, 
                                    { "iTax", "IT","" }, 
                                    { "VudyogTRD", "UV","" }, 
                                    { "VudyogMFG", "UV","" }, 
                                    { "VudyogServiceTax", "VS","" }, 
                                    { "VudyogSDK", "U9","U9 SDK" }, 
                                    { "VudyogSTD", "U8" ,"U8 STD"} ,
                                    {"VudyogPRO","U7","U7 PRO"},
                                    {"VudyogENT","U6","U6 ENT"},
                                    {"10USquare","U5","U5 USQ"},
                                    {"10iTax","I5","I5 ITX"}    
                                    };

            for (int i = 0; i < prodArray.GetUpperBound(0); i++)
            {
                if (appName == prodArray[i, 0])
                {
                    retValue = prodArray[i, retIndex];
                    break;
                }
            }

            return retValue;
        }
    }
}
