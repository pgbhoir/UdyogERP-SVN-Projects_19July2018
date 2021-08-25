using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;
using System.Windows.Forms;
using Udyog.Library.Common;
using System.Data;

namespace ueProductUpgrade
{
    class GetMachineDetails
    {
        public static string MachineName()
        {
            return System.Net.Dns.GetHostName().ToUpper();
        }
        public static string IpAddress()
        {
            string retVal = string.Empty;
            ManagementObjectSearcher objSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");
            ManagementObjectCollection objCollection = objSearcher.Get();
            foreach (ManagementObject obj in objCollection)
            {
                string[] AddressList = (string[])obj["IPAddress"];
                retVal = AddressList[0].ToString();
                break;
            }
            if (retVal.Length == 0)
                retVal = "127.0.0.1";
            return retVal;
        }
        public static string ProcessorId()
        {
            string processorID = string.Empty;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * FROM WIN32_Processor");
            ManagementObjectCollection mObject = searcher.Get();

            foreach (ManagementObject obj in mObject)
            {
                processorID = obj["ProcessorId"].ToString();
                break;
            }
            return processorID;
        }
    }
    public class ReadRegisterMefile
    {
        public ReadRegisterMefile(){
        }

        public string[,] ReadFile(string fileName)
        {
            string _RegMe = "";
            StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding(1252));
            _RegMe = sr.ReadToEnd();
            sr.Close();

            string[,] retval = new string[,]
            {
                    {"r_compn",VU_UDFS.dec(_RegMe.Substring(0, 50)) },
                    { "r_comp",VU_UDFS.dec(_RegMe.Substring(0, 50))},
                    { "r_add",VU_UDFS.dec(_RegMe.Substring(50, 200))},
                    { "r_srvtype",VU_UDFS.dec(_RegMe.Substring(1500, 50)) },
                    {"r_regtype",VU_UDFS.dec(_RegMe.Substring(1550, 50)) },
                    {"r_instdate",VU_UDFS.dec(_RegMe.Substring(1600, 10)) },
                    {"r_Business",VU_UDFS.dec(_RegMe.Substring(1660, 100)) },
                    {"r_ProdManu",VU_UDFS.dec(_RegMe.Substring(1760, 100)) },
                    {"xvalue",VU_UDFS.dec(_RegMe.Substring(1860, 200)) },
                    {"r_idno",VU_UDFS.dec(_RegMe.Substring(2060, 50)) },
                    {"r_clientid",VU_UDFS.dec(_RegMe.Substring(2110, 15)) },
                    {"r_ecode",VU_UDFS.dec(_RegMe.Substring(2125, 50)) },
                    {"r_ename",VU_UDFS.dec(_RegMe.Substring(2175, 50)) },
                    {"r_svccode",VU_UDFS.dec(_RegMe.Substring(2225, 50)) },
                    { "r_svcname",VU_UDFS.dec(_RegMe.Substring(2275, 50)) },
                    {"r_coof",Convert.ToUInt16(VU_UDFS.dec(VU_UDFS.dec(_RegMe.Substring(2325, 10))).Trim().Replace("COOF", "").Replace(Convert.ToChar(30).ToString(), "")).ToString() },
                    {"r_noof",Convert.ToUInt16(VU_UDFS.dec(VU_UDFS.dec(_RegMe.Substring(2335, 10))).ToString().Trim().Replace("USOF", "")).ToString() },
                    {"r_pid",VU_UDFS.dec(_RegMe.Substring(2345, 10)) },
                    {"r_dbsrvname",VU_UDFS.dec(_RegMe.Substring(2355, 50)) },
                    {"r_dbsrvIp",VU_UDFS.dec(_RegMe.Substring(2405, 20)) },
                    {"r_Apsrvname",VU_UDFS.dec(_RegMe.Substring(2425, 50)) },
                    {"r_ApsrvIp",VU_UDFS.dec(_RegMe.Substring(2475, 20)) },
                    {"r_ExpDt",VU_UDFS.dec(_RegMe.Substring(2495, 25)) },
                    {"r_MACId",VU_UDFS.dec(_RegMe.Substring(2520, 50)) },
                    {"r_AMCStDt",VU_UDFS.dec(_RegMe.Substring(2570, 25)) },
                    {"r_AMCEdDt",VU_UDFS.dec(_RegMe.Substring(2595, 25)) },
                    {"reg_date", (_RegMe.Length > 2620?VU_UDFS.dec(_RegMe.Substring(2620, 10)):"" ) },
                    { "reg_value",(_RegMe.Length > 2620?VU_UDFS.dec(_RegMe.Substring(2630, 8)):"NOT DONE" )}
            };
            return retval;
        }

    }
    public class ReadInfFile
    {
        public ReadInfFile() { }
        public DataTable ReadFile(string fileName)
        {
            DataTable RetTable = new DataTable();
            RetTable.Columns.Add(new DataColumn("clientnm", typeof(string)));
            RetTable.Columns.Add(new DataColumn("macid", typeof(string)));
            RetTable.Columns.Add(new DataColumn("prodnm", typeof(string)));
            RetTable.Columns.Add(new DataColumn("mainprodcd", typeof(string)));
            RetTable.Columns.Add(new DataColumn("prodcd", typeof(string)));
            RetTable.Columns.Add(new DataColumn("zip", typeof(string)));
            RetTable.Columns.Add(new DataColumn("featureid", typeof(string)));
            RetTable.Columns.Add(new DataColumn("vatstates", typeof(string)));
            RetTable.Columns.Add(new DataColumn("addprodcd", typeof(string)));

            string Inftext = string.Empty;
            byte[] binaryContent = System.IO.File.ReadAllBytes(fileName);
            for (int i = 0; i < binaryContent.Length; i++)
            {
                Inftext = Inftext + Utilities.Chr(binaryContent[i]);
            }
            string _partynm = Inftext.Trim().Substring(0, 50).Trim();
            string dec_cldet = _partynm;
            string _macid = string.Empty;

            for (int i = 50; i < Inftext.Length; i=i+50)
            {
                string _mname = string.Empty;
                if (i+50 > Inftext.Length)
                    _mname= VU_UDFS.NewDECRY(Inftext.Trim().Substring(i, Inftext.Length- i), _partynm);
                else
                    _mname = VU_UDFS.NewDECRY(Inftext.Trim().Substring(i, 50), _partynm);
                dec_cldet = dec_cldet + _mname;
            }
            string _partyz = dec_cldet;
            while (true)
            {
                int _party1 = _partyz.IndexOf("<<~0s>>");
                int _party2 = _partyz.IndexOf("<<e0~>>");
                if (_party1 >= 0 && _party2 > 0)
                {
                    string _partya = _partyz.Substring(_party1, _party2 - _party1);
                    _macid = "";
                    DataRow row = RetTable.NewRow();
                    while (true)
                    {
                        int _party3 = _partya.IndexOf("<<~1s>>");
                        int _party4 = _partya.IndexOf("<<e1~>>");
                        if (_party3 >= 0 && _party4 > 0)
                        {
                            string _partyd = _partya.Substring(_party3, _party4 - _party3);
                            switch (_partyd.Substring(7,2))
                            {
                                case "CN":
                                    row["clientnm"] = VU_UDFS.dec(VU_UDFS.NewDECRY(_partyd.Substring(10), _partynm)).Trim();
                                    break;
                                case "MI":
                                    _macid= VU_UDFS.dec(VU_UDFS.NewDECRY(_partyd.Substring(10), _partynm)).Trim();
                                    row["macid"] = _macid;
                                    break;
                                case "PV":
                                    row["prodnm"] = VU_UDFS.dec(VU_UDFS.NewDECRY(_partyd.Substring(10), _macid)).Trim();
                                    break;
                                case "MP":
                                    row["mainprodcd"] = VU_UDFS.NewDECRY(_partyd.Substring(10), _macid).Trim();
                                    break;
                                case "PC":
                                    string _cPrdCode1 = string.Empty;
                                    string _cPrdCode2 = string.Empty;
                                    string _cPrdCode3 = string.Empty;
                                    string _cPrdCode= VU_UDFS.NewDECRY(_partyd.Substring(10), _macid).Trim()+",";
                                    while (true)
                                    {
                                        _cPrdCode1 = _cPrdCode.Left(_cPrdCode.IndexOf(","));
                                        if (Utilities.InList(_cPrdCode1, new string[] { "vuent", "vupro", "vuexc", "vuexp", "vuinv", "vuord", "vubil", "vutex", "vuser", "vuisd", "vumcu", "vutds", "vugst" }))
                                        {
                                            _cPrdCode2 = _cPrdCode2 + (_cPrdCode2.Trim().Length <= 0 ? "" : ",") + _cPrdCode1.Trim();
                                        }
                                        else
                                        {
                                            _cPrdCode3 = _cPrdCode3 + (_cPrdCode3.Trim().Length <= 0 ? "" : ",") + _cPrdCode1.Trim();
                                        }
                                        _cPrdCode = _cPrdCode.Substring(_cPrdCode.IndexOf(",") + 1);
                                        if (_cPrdCode.Trim().Length <= 0)
                                        {
                                            break;
                                        }
                                    }
                                    row["prodcd"] = _cPrdCode2.Trim();
                                    row["addprodcd"] = _cPrdCode3.Trim();
                                    break;
                                case "ZP":
                                    row["zip"] = VU_UDFS.NewDECRY(_partyd.Substring(10), _macid).Trim();
                                    break;
                                case "FI":
                                    row["featureid"] = VU_UDFS.NewDECRY(_partyd.Substring(10),_partynm+ _macid).Trim();
                                    break;
                                case "VS":
                                    row["vatstates"] = VU_UDFS.NewDECRY(_partyd.Substring(10), _macid).Trim();
                                    break;
                            }
                            _partya = _partya.Substring(_party4 + 7);
                        }
                        else
                        {
                            _partya = string.Empty;
                        }
                        if (_partya == string.Empty)
                            break;
                    }
                    _partyz = _partyz.Substring(_party2 + 7);
                    RetTable.Rows.Add(row);
                }
                else
                {
                    _partyz = string.Empty;
                }
                if (_partyz == string.Empty)
                    break;
            }

            return RetTable;
        }  

    }
}
