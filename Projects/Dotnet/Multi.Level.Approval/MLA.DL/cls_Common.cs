using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLA.DL
{
    public class cls_Common
    {
        public cls_Common()
        {
        }

        #region CompanyDecryption Methods
        public static string ByteArrayToString(byte[] byteArray)
        {
            var hex = new StringBuilder(byteArray.Length * 1);
            foreach (var b in byteArray)
                hex.AppendFormat("{0:X}", b);
            return hex.ToString();
        }

        public static string Decrypt(string cExpression, string userrole)
        {
            string lExpression = cExpression;
            lExpression = lExpression.Substring(2, lExpression.Length - 2);
            int w, m, tot;
            string n;
            int i;
            string ins1;
            ins1 = "";
            int[] comarry = new int[(lExpression.Length / 2)];
            i = 0;

            string padstr1;
            padstr1 = Replicate(userrole.ToUpper(), ((lExpression.Length / 2) / userrole.Length) + 1);
            padstr1 = padstr1.Substring(0, (lExpression.Length / 2) - userrole.Length);
            padstr1 = padstr1 + userrole.ToUpper();

            for (w = 0; w < lExpression.Length; w = w + 2)
            {

                n = lExpression.Substring(w, 2);
                n = "0x" + n;
                comarry[i] = System.Convert.ToInt32(n, 16);
                m = Convert.ToChar(padstr1.Substring(i, 1));
                tot = comarry[i] - m;
                if (tot > 0)
                {
                    ins1 = ins1 + Convert.ToChar(tot);
                }
                i = i + 1;
            }

            return ins1;
        }


        public static string Replicate(string cExpression, int nTimes)
        {
            //Create a stringBuilder
            StringBuilder sb = new StringBuilder();
            //Insert the expression into the StringBuilder for nTimes
            sb.Insert(0, cExpression, nTimes);
            //Convert it to a string and return it back
            return sb.ToString();
        }
        #endregion
    }
}
