using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Data;
using DataAccess_Net;

namespace ueProductUpgrade
{
    class Utilities
    {
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

        public static string GetDecoder(string value,bool flag)
        {
           // string mVal = "IYCYDYPYVY";
            string finalCode = string.Empty;
            for (int i = 0; i < value.Length; i++)
            {
                if (flag!=true)
                    finalCode = finalCode + Chr(Asc(value[i].ToString()) - 4);
                else
                    finalCode = finalCode + Chr(Asc(value[i].ToString()) + 4);
            }
            return finalCode;
        }

        //public static string GetRights()
        //{
        //    string mVal="IYCYDYPYVY";
        //    string finalCode=string.Empty;
        //    for (int i = 0; i < mVal.Length; i++)
        //    {
        //        finalCode =finalCode+ Chr(Asc(mVal[i].ToString()) - 4);
        //    }
        //    return finalCode;
        //}

        //public static string GetRoles()
        //{
        //    string mVal = "ADMINISTRATOR";
        //    string finalCode = string.Empty;
        //    for (int i = 0; i < mVal.Length; i++)
        //    {
        //        finalCode = finalCode + Chr(Asc(mVal[i].ToString()) + 4);
        //    }
        //    return finalCode;
        //}

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
            int d = 1;
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
            int d = 1;
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
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
        public static bool InList(string searchStr, string[] strList)
        {
            for (int i = 0; i < strList.Length; i++)
            {
                if (searchStr == strList[i])
                {
                    return true;
                }
            }
            return false;
        }
        public static DataRow AddNewRow(DataTable _dt)
        {
            DataRow _dr = _dt.NewRow();
            for (int i = 0; i < _dt.Columns.Count; i++)
            {
                switch (_dt.Columns[i].DataType.ToString())
                {
                    case "System.Int16":
                        _dr[i] = Convert.ToInt16(0);
                        break;
                    case "System.Decimal":
                        _dr[i] = Convert.ToDecimal(0.00);
                        break;
                    case "System.Double":
                        _dr[i] = Convert.ToDouble(0.00);
                        break;
                    case "System.String":
                        _dr[i] = Convert.ToString("");
                        break;
                    case "System.Boolean":
                        _dr[i] = Convert.ToBoolean(false);
                        break;
                    case "System.DateTime":
                        _dr[i] = Convert.ToDateTime("01/01/1900");
                        break;
                }
            }
            return _dr;
        }

        private static string GetUpdateString(DataRow _dr, string _tblName, List<clsParam> _clparam, string _xcludeFld)
        {
            DataTable _dt = _dr.Table;
            string _sql = BuildUpdateSQL(_dt, _tblName, _xcludeFld);

            clsParam _objparam;
            clsParam.pType paramType = clsParam.pType.pString;
            string _fldNm = "";
            string[] _xcludeFldArr = _xcludeFld.Split(',');
            foreach (DataColumn _dc in _dt.Columns)
            {
                _fldNm = _dc.ColumnName;
                if (Array.IndexOf(_xcludeFldArr, _fldNm.ToUpper()) != -1)
                {
                    continue;
                }

                if (_dc.DataType.Name == "Byte[]")
                    continue;

                switch (_dc.DataType.ToString().ToLower())
                {
                    case "varchar":
                        paramType = clsParam.pType.pString;
                        break;
                    case "decimal":
                        paramType = clsParam.pType.pFloat;
                        break;
                    case "bit":
                        paramType = clsParam.pType.pLong;
                        break;
                    case "datetime":
                        paramType = clsParam.pType.pDate;
                        break;
                    case "text":
                        paramType = clsParam.pType.pString;
                        break;
                    default:
                        paramType = clsParam.pType.pString;
                        break;
                }
                _objparam = new clsParam(_dc.ColumnName, _dr[_fldNm], paramType, _clparam, false, null, clsParam.pInOut.pIn);
            }
            return _sql;
        }

        private static string BuildUpdateSQL(DataTable _dt, string _tblName, string _xcludeFld)
        {
            StringBuilder _InsSql = new StringBuilder("Update " + _tblName + " ");
            StringBuilder _ValSql = new StringBuilder(" Set ");
            bool _FirstRec = true;
            string _colName = "";
            string[] _xcludeFldArr = _xcludeFld.Split(',');
            foreach (DataColumn _dc in _dt.Columns)
            {
                _colName = _dc.ColumnName.ToUpper();
                if (Array.IndexOf(_xcludeFldArr, _colName) != -1)
                {
                    continue;
                }

                if (_dc.DataType.Name == "Byte[]")
                    continue;

                if (_FirstRec)
                {
                    _FirstRec = false;
                }
                else
                {
                    _InsSql.Append(", ");
                    _ValSql.Append(", ");
                }

                _ValSql.Append("[" + _dc.ColumnName + "] = ?");
            }

            _InsSql.Append(_ValSql.ToString());
            _InsSql.Append("Where Tran_cd = " + _dt.Rows[0]["Tran_Cd"].ToString());

            return _InsSql.ToString();
        }
        //***** Added by Sachin N. S. on 20/01/2016 for Bug-27503 -- End ******//

        private static string BuildInsertSQL(DataTable _dt, string _tblName, string _xcludeFld)
        {
            StringBuilder _InsSql = new StringBuilder("Insert into " + _tblName + " ( ");
            StringBuilder _ValSql = new StringBuilder(" Values (");
            bool _FirstRec = true;
            bool _isIdentity = false;
            string _IdentityType = "";
            string _colName = "";
            string[] _xcludeFldArr = _xcludeFld.Split(',');
            foreach (DataColumn _dc in _dt.Columns)
            {
                _colName = _dc.ColumnName.ToUpper();

                if (Array.IndexOf(_xcludeFldArr, _colName) != -1)
                {
                    continue;
                }

                if (_dc.DataType.Name == "Byte[]")
                    continue;

                if (_dc.AutoIncrement)
                {
                    _isIdentity = true;

                    switch (_dc.DataType.Name)
                    {
                        case "Int16":
                            _IdentityType = "smallint";
                            break;
                        case "SByte":
                            _IdentityType = "tinyint";
                            break;
                        case "Int64":
                            _IdentityType = "bigint";
                            break;
                        case "Decimal":
                            _IdentityType = "decimal";
                            break;
                        default:
                            _IdentityType = "int";
                            break;
                    }
                }
                else
                {
                    if (_FirstRec)
                    {
                        _FirstRec = false;
                    }
                    else
                    {
                        _InsSql.Append(", ");
                        _ValSql.Append(", ");
                    }

                    _InsSql.Append("[" + _dc.ColumnName + "]");
                    _ValSql.Append("?");
                    //_ValSql.Append("@");
                    //_ValSql.Append(_dc.ColumnName);
                }
            }

            _InsSql.Append(")");
            _ValSql.Append(")");
            _InsSql.Append(_ValSql.ToString());

            if (_isIdentity)
            {
                _InsSql.Append("; Select cast(scope_identity() as ");
                _InsSql.Append(_IdentityType);
                _InsSql.Append(")");
            }

            return _InsSql.ToString();
        }
        private static string GetInsertString(DataRow _dr, string _tblName, List<clsParam> _clparam, string _xcludeFld)
        {
            DataTable _dt = _dr.Table;
            string _sql = BuildInsertSQL(_dt, _tblName, _xcludeFld);

            clsParam _objparam;
            clsParam.pType paramType = clsParam.pType.pString;
            string _fldNm = "";
            string[] _xcludeFldArr = _xcludeFld.Split(',');
            foreach (DataColumn _dc in _dt.Columns)
            {
                _fldNm = _dc.ColumnName;
                if (Array.IndexOf(_xcludeFldArr, _fldNm.ToUpper()) != -1)
                {
                    continue;
                }

                if (_dc.DataType.Name == "Byte[]")
                    continue;

                switch (_dc.DataType.ToString().ToLower())
                {
                    case "varchar":
                        paramType = clsParam.pType.pString;
                        break;
                    case "decimal":
                        paramType = clsParam.pType.pFloat;
                        break;
                    case "bit":
                        paramType = clsParam.pType.pLong;
                        break;
                    case "datetime":
                        paramType = clsParam.pType.pDate;
                        break;
                    case "text":
                        paramType = clsParam.pType.pString;
                        break;
                    default:
                        paramType = clsParam.pType.pString;
                        break;
                }
                _objparam = new clsParam(_dc.ColumnName, _dr[_fldNm], paramType, _clparam, false, null, clsParam.pInOut.pIn);
            }

            //_clparam.Add(new clsParam { FieldName = "@Entry_ty1", Value = _dc });
            //_clparam.Add(new clsParam { FieldName = "@Tran_cd", Value = 0 });

            return _sql;
        }
    }

    public static class Extensions
    {
        public static String Left(this string input, int length)
        {
            var result = "";
            if ((input.Length <= 0)) return result;
            if ((length > input.Length))
            {
                length = input.Length;
            }
            result = input.Substring(0, length);
            return result;
        }
 
        public static String Mid(this string input, int start, int length)
        {
            var result = "";
            if (((input.Length <= 0) || (start >= input.Length))) return result;
            if ((start + length > input.Length))
            {
                length = (input.Length - start);
            }
            result = input.Substring(start, length);
            return result;
        }
 
        public static String Right(this string input, int length)
        {
            var result = "";
            if ((input.Length <= 0)) return result;
            if ((length > input.Length))
            {
                length = input.Length;
            }
            result = input.Substring((input.Length - length), length);
            return result;
        }
    }
}
