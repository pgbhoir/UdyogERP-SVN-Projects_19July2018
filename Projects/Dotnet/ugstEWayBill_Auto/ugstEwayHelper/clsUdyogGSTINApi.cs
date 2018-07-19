using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Collections;
using System.Windows.Forms;
using Udyog.Library.Common;

namespace ugstnHelper
{

    public class clsUdyogGSTINApi
    {
        private string gspname = "ADAEQUARE_APP_01"; //"ADAEQUARE_VU";

        //private string username = "F066EA96863D4A68BB2944B2328DEB82";
        //private string password = "8C9D3661G75D3G46ADG9331G1B70E90BAA7D";
        //private string authorization = "Basic RjA2NkVBOTY4NjNENEE2OEJCMjk0NEIyMzI4REVCODI6OEM5RDM2NjFHNzVEM0c0NkFERzkzMzFHMUI3MEU5MEJBQTdE";


        public clsUdyogGSTINApi()
        {

        }
        public string GetAuthentication(string urlAddress, string gspId, string gspPwd)
        {
            string jsonData = string.Empty;
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(urlAddress);

            request.Method = "POST";
            request.ContentType = "application/json";
            //request.Headers.Add("Authorization",authorization);
            request.Headers.Add("gspappid", gspId);
            request.Headers.Add("gspappsecret", gspPwd);

            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    //string json =string.Format("{\"username\":\"{0}\"," +"\"password\":\"{1}\"}",username,password);
                    string json = "{\"gspappid\":\"" + gspId + "\"," + "\"gspappsecret\":\"" + gspPwd + "\"}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    jsonData = streamReader.ReadToEnd();
                }

                var jsonObject = JObject.Parse(jsonData);
                jsonData = (string)jsonObject.SelectToken("access_token");
                //string status = (string)jsonObject.SelectToken("status");
                //if (!string.IsNullOrEmpty(status))
                //{
                //    if (status.Trim() == "0")
                //    {
                //        throw new Exception("status : 0");
                //    }
                //}

                string status = string.Empty;
                foreach (var pair in jsonObject)
                {
                    switch (pair.Key)
                    {
                        case "error":
                            status = (string)jsonObject.SelectToken("error_description");
                            throw new Exception(status);
                        case "status":
                            status = (string)jsonObject.SelectToken("status");
                            if (status == "0")
                            {
                                throw new Exception("status : 0");
                            }
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                jsonData = "Issue occured, " + ex.Message;
            }

            return jsonData;
        }
        public string GetEwayBillDetails(string _username, string _password, string urlAddress, string tokenvalue, string gstin, string requestId, string bodyString)
        {
            string jsonData = string.Empty;

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(urlAddress);
            request.Method = "POST";
            request.ContentType = "application/json";
            //request.Headers.Add("gspname", gspname);
            request.Headers.Add("username", _username);
            request.Headers.Add("password", _password);
            request.Headers.Add("gstin", gstin.Trim());
            request.Headers.Add("requestid", requestId);
            request.Headers.Add("Authorization", tokenvalue);
            request.Headers.Add("Cache-Control", "no-cache");
            try
            {
                using (Stream streamWriter = request.GetRequestStream())
                {
                    //var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(bodyString);
                    //streamWriter.Write(json);

                    Encoding encoding = new UTF8Encoding();
                    byte[] data = encoding.GetBytes(bodyString);
                    streamWriter.Write(data, 0, data.Length);

                    streamWriter.Flush();
                    streamWriter.Close();
                }

                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                    jsonData = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();
                    var jsonObject = JObject.Parse(jsonData);

                    //MessageBox.Show(jsonData);

                    string status = string.Empty;
                    foreach (var pair in jsonObject)
                    {
                        switch (pair.Key)
                        {
                            case "error":
                                status = (string)jsonObject.SelectToken("error_description");
                                throw new Exception(status);
                            case "success":
                                status = (string)jsonObject.SelectToken("success");
                                if (status == "False")
                                {
                                    if ((string)jsonObject.SelectToken("result") == null)
                                    {
                                        status = jsonObject.SelectToken("message").Value<string>();
                                        status = status.Replace("\"", "").Replace("errorCodes:", "").Replace("{", "").Replace("}", "");
                                        throw new Exception(status);
                                    }
                                    else
                                    {
                                        status = jsonObject.SelectToken("message").Value<string>();
                                        throw new Exception(status);
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                jsonData = "Issue occured, " + ex.Message;
            }

            return jsonData;
        }

        public string CancelEwayBills(string _username, string _password, string urlAddress, string tokenvalue, string gstin, string requestId, string bodyString)
        {
            string jsonData = string.Empty;

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(urlAddress);
            request.Method = "POST";
            request.ContentType = "application/json";
            //request.Headers.Add("gspname", gspname);
            request.Headers.Add("username", _username);
            request.Headers.Add("password", _password);
            request.Headers.Add("gstin", gstin);
            request.Headers.Add("requestid", requestId);
            request.Headers.Add("Authorization", tokenvalue);

            try
            {
                using (Stream streamWriter = request.GetRequestStream())
                {
                    Encoding encoding = new UTF8Encoding();
                    byte[] data = encoding.GetBytes(bodyString);
                    streamWriter.Write(data, 0, data.Length);

                    streamWriter.Flush();
                    streamWriter.Close();
                }


                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                    jsonData = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();

                    var jsonObject = JObject.Parse(jsonData);
                    //IList<JToken> childList=jsonObject.Children().ToList();
                    string status = string.Empty;
                    foreach (var pair in jsonObject)
                    {
                        switch (pair.Key)
                        {
                            case "error":
                                status = (string)jsonObject.SelectToken("error_description");
                                throw new Exception(status);
                            case "success":
                                status = (string)jsonObject.SelectToken("success");
                                if (status == "False")
                                {
                                    if ((string)jsonObject.SelectToken("result") == null)
                                    {
                                        status = jsonObject.SelectToken("message").Value<string>();
                                        status = status.Replace("\"", "").Replace("errorCodes:", "").Replace("{", "").Replace("}", "");
                                        throw new Exception(status);
                                    }
                                    else
                                    {
                                        status = jsonObject.SelectToken("message").Value<string>();
                                        throw new Exception(status);
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                jsonData = "Issue occured, " + ex.Message;
            }

            return jsonData;
        }

    }

    // Added by Sachin N. S. on 06/04/2018 -- Start
    public class clsUdyogGSTApiLicChker
    {
        public clsUdyogGSTApiLicChker()
        {
        }

        public string[] ChkClientLicense(string _appPath, string _compNm, string _module)
        {
            string[] ClientCred = new string[2];
            ClientCred[0] = "";
            ClientCred[1] = "";

            //_appPath = @"D:\VudyogGST\UdyogGST\Udyog Software License Testing company 000001";       // to be Removed
            //_appPath = @"D:\VudyogGST\UdyogGST\Udyog Software Pvt. Ltd 000001";       // to be Removed
            string[] ApiLicenseFile = Directory.GetFiles(_appPath+"\\", "*API Lic.Licx");
            if (ApiLicenseFile.Length == 0)
                return ClientCred;

            string _Filepath = ApiLicenseFile[0];
            if (!File.Exists(_Filepath))
            {
                return ClientCred;
            }
            
            FileStream stream = File.Open(_Filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1252));

            string fileText = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            
            string _PartyNm = fileText.Substring(0, 50).Trim();
            int _lens = 1;
            int _lens1 = 0;
            string _licenseStr = "";

            
            for (_lens = 50; _lens < fileText.Length; )
            {
                _lens1 = fileText.Length - _lens >= 50 ? 50 : fileText.Length - _lens;
                //_licenseStr += VU_UDFS.dec(VU_UDFS.dec(fileText.Substring(_lens, _lens1)));
                _licenseStr += VU_UDFS.NewDECRY(fileText.Substring(_lens, _lens1), _PartyNm);
                _lens += fileText.Length - _lens >= 50 ? 50 : fileText.Length - _lens;
            }
            string[] _compLst = _licenseStr.Split(new[] { "<<~0s>>" }, StringSplitOptions.RemoveEmptyEntries);
            string[] _compDet;

            foreach (string _str1 in _compLst)
            {
                _compDet = _str1.Replace("<<e0~>>", "").Split(new[] { "<<~1s>>" }, StringSplitOptions.RemoveEmptyEntries);
                if (_compDet.Length == 4)
                {
                    //string _cn = VU_UDFS.dec(VU_UDFS.dec(_compDet[0].Replace("<<e1~>>", "").Replace("CN:", "")));
                    string _cn = VU_UDFS.dec(VU_UDFS.NewDECRY(_compDet[0].Replace("<<e1~>>", "").Replace("CN:", ""),_PartyNm));
                    if (_compNm.Trim() == _cn.Trim())
                    {
                        //string _mi = VU_UDFS.dec(VU_UDFS.dec(_compDet[1].Replace("<<e1~>>", "").Replace("MI:", "")));
                        //string _pv = VU_UDFS.dec(VU_UDFS.dec(_compDet[2].Replace("<<e1~>>", "").Replace("PV:", "")));

                        string _mi = VU_UDFS.dec(VU_UDFS.NewDECRY(_compDet[1].Replace("<<e1~>>", "").Replace("MI:", ""),_PartyNm));
                        _mi = _mi.Trim();
                        string _pv = VU_UDFS.dec(VU_UDFS.NewDECRY(_compDet[2].Replace("<<e1~>>", "").Replace("PV:", ""),_mi));

                        string[] _md = _compDet[3].Replace("<<e1~>>", "").Split(new[] { "<<~2s>>" }, StringSplitOptions.RemoveEmptyEntries);
                        string[] _mdlst;
                        bool lexit = false;

                        foreach (string _str2 in _md)
                        {
                            _mdlst = _str2.Replace("<<e2~>>", "").Split(new[] { "<<~3s>>" }, StringSplitOptions.RemoveEmptyEntries);
                            if (_mdlst.Length == 3)
                            {
                                //string _mn = VU_UDFS.dec(_mdlst[0].Replace("<<e3~>>", "").Replace("PC:", ""));
                                string _mn = VU_UDFS.NewDECRY(_mdlst[0].Replace("<<e3~>>", "").Replace("PC:", ""),_mi);
                                
                                if (_module.Trim() == _mn.Trim())
                                {
                                    //ClientCred[0] = VU_UDFS.dec(_mdlst[1].Replace("<<e3~>>", "").Replace("UI:", ""));
                                    //ClientCred[1] = VU_UDFS.dec(_mdlst[2].Replace("<<e3~>>", "").Replace("PW:", ""));

                                    ClientCred[0] = VU_UDFS.NewDECRY(_mdlst[1].Replace("<<e3~>>", "").Replace("UI:", ""),_mi);
                                    ClientCred[1] = VU_UDFS.NewDECRY(_mdlst[2].Replace("<<e3~>>", "").Replace("PW:", ""),_mi);

                                    lexit = true;
                                    break;
                                }
                            }
                        }
                        if (lexit == true)
                            break;
                    }
                }
            }
            return ClientCred;
        }
    }
    // Added by Sachin N. S. on 06/04/2018 -- End
}
