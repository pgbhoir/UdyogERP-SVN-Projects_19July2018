using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Udyog.Library.Common;
using System.Windows;
using System.Windows.Forms;
using udEInvoiceJson;
using System.Drawing;
using Udyog.QRCode.Codec;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;


namespace udAdqGSPHelper
{

    public static class clsAdqGSPHelper
    {
        
        static void Main(string[] args)
        {

            //System.Windows.Forms.MessageBox.Show("Hi");
            //System.Windows.Forms.MessageBox.Show(args[0].ToString());
            if (args.Length < 1)
            {
                args = new string[] { "GSTIN_SEARCH", "3", "U011920", @"AIPLLTM035\SQLEXPRESS2014", "sa", "sa@1985r2", "GSTIN", "9", "^21001", "ADMIN", @"D:\UdyogERP\Bmp\Icon_VudyogGST.ico", "UdyogERPSDK", "UdyogERPSDK", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "Work Order Generation", "2018-19" };
            }
            string vComp_Id, vdsDbNm, vdsDbServNm, vDbUserId, vDbPass, vGSTIN, vAc_Id, vModuleNm, vAppUerName, vApplText, vFin_Year, vTranCd, vEntryTy,vIRN, vBcode_nm; //30122020
            short vTimeOut = 15;
            vComp_Id = args[0];
            vdsDbNm = args[1];
            vdsDbServNm = args[2];
            vDbUserId = args[3];
            vDbPass = args[4];
            vGSTIN = args[5];
            vAc_Id = args[6];
            vModuleNm = args[7];
            vAppUerName = args[8];
            //vApplText = args[9];
            vApplText = args[9].Replace("<*#*>", " ");
            int i = 0;

           

            //Arg10=null
            System.Windows.Forms.ToolStripLabel vlbl = new System.Windows.Forms.ToolStripLabel();
            //System.Windows.Forms.MessageBox.Show(vApplText);
            //System.Windows.Forms.ToolStripLabel vlbl;

            if (vModuleNm.ToUpper() == "GSTIN_SEARCH".ToUpper())
            {
                clsGSTINSearch vGSTINSearch = new clsGSTINSearch();
                vGSTINSearch.mthCheckGSTIN(vComp_Id, vdsDbNm, vdsDbServNm, vDbUserId, vDbPass, vGSTIN, vAc_Id, vModuleNm, vAppUerName, vTimeOut, vApplText, vlbl);
                //clsGSTINSearch vGSTINSearch = new clsGSTINSearch();
                //vGSTINSearch.mthCheckGSTIN(args[2],args[3],args[4],args[5],args[6], args[7],args[0],args[9],"coName","uGSTIN",15,args[10],"err","nicid","nicpwd","apppath","lblnull");
            }
            if (vModuleNm.ToUpper() == "GSTR_STATUS".ToUpper())
            {
                vFin_Year = args[11];
                //MessageBox.Show(vFin_Year);
                clsGSTRStatus vGSTRStatus = new clsGSTRStatus();
                vGSTRStatus.mthCheckGSTRStatus(vFin_Year, vAc_Id, vComp_Id, vdsDbNm, vdsDbServNm, vDbUserId, vDbPass, vGSTIN, vModuleNm, vAppUerName, vTimeOut, vApplText, vlbl);

            }
            if (vModuleNm.ToUpper() == "EINVOICE_GEN".ToUpper())
            {

                vTranCd = args[11];
                vEntryTy = args[12];
                vBcode_nm = args[14];
                //MessageBox.Show(vFin_Year);
                clseInvoice veInvoice = new clseInvoice();
                veInvoice.mthGeneInvoice(vComp_Id, vdsDbNm, vdsDbServNm, vDbUserId, vDbPass, vEntryTy, vTranCd, vBcode_nm, vModuleNm, vAppUerName, vTimeOut, vApplText, vlbl);


            }
        }
    }
    public class clsMain
    {
        public string mthGetData(string vApiURL, string tokenvalue, string vAutho_Token, DataAccess_Net.clsDataAccess oDataAccess, short vTimeOut, string vApplText, string vGSTIN, string vAppUerName, string vRequestId, string vModuleNm, System.Windows.Forms.ToolStripLabel vlbl)
        {
            string jsonData = string.Empty;

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(vApiURL);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", tokenvalue);
            try
            {
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
            if (jsonData.Contains("Issue occured, "))
            {
                clsMain vClsMain = new clsMain();
                string issueStr = jsonData.Replace("Issue occured, ", "");
                if (issueStr.Contains("Invalid Token") || issueStr.Contains("Token Expired"))
                {
                    vAutho_Token = string.Empty;
                    //vErrMsg = issueStr;
                    //return;
                }
                if (issueStr.Substring(issueStr.Length - 1, 1) == ",")
                {
                    issueStr = issueStr.Substring(0, issueStr.Length - 1);
                    issueStr = vClsMain.mthGetErrorList(issueStr, oDataAccess, vTimeOut, vApplText);
                }

                vClsMain.mthUpdate_ApiLog("GSTIN Search-Err Response", DateTime.Now, vAppUerName, issueStr, vRequestId, vModuleNm, oDataAccess);
                //vErrMsg = issueStr;
                System.Windows.Forms.MessageBox.Show(vGSTIN + " - " + issueStr, vApplText, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

                vlbl.Text = "Ready";
                vAutho_Token = string.Empty;
                //return;
            }
            return jsonData;
        }

       
        //Added By Rupesh G on 27/01/2020 for bug no.---Start
        public string mthPostData(string vApiURL, string tokenvalue, string vAutho_Token, DataAccess_Net.clsDataAccess oDataAccess, short vTimeOut, string vApplText, string vGSTIN, string vAppUerName, string vRequestId, string vModuleNm, System.Windows.Forms.ToolStripLabel vlbl, string username, string password, string gstin, string bodyString)
        {
            vlbl = new ToolStripLabel();
            string requestId = gstin.Trim() + DateTime.Now.ToString("yyyyMMddHHmmssFFF").Trim();

            string jsonData = string.Empty;

            string vDupIrn = string.Empty;      //Divyang 09102020
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(vApiURL);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", tokenvalue);
            //gstin = "01AMBPG7773M002";   //Comm Divyang
            // username = "adqgspjkusr1";
            request.Headers.Add("user_name", username);
            request.Headers.Add("password", password);
            request.Headers.Add("gstin", gstin.Trim());
            request.Headers.Add("requestid", requestId);
            request.Headers.Add("Authorization", tokenvalue);
            request.Headers.Add("Cache-Control", "no-cache");
            MessageBox.Show(vApiURL.ToString());
            MessageBox.Show(username.ToString());
            MessageBox.Show(password.ToString());
            MessageBox.Show(gstin.ToString());
            MessageBox.Show(requestId.ToString());
            MessageBox.Show(tokenvalue.ToString());

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
                    //JArray jsonArray = JArray.Parse(jsonData);

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
                                    if (jsonObject.Count > 2)       //Divyang
                                    {
                                        if ((string)jsonObject.SelectToken("result").ToString() == null)
                                        {
                                            status = jsonObject.SelectToken("message").Value<string>();
                                            status = status.Replace("\"", "").Replace("errorCodes:", "").Replace("{", "").Replace("}", "");
                                            throw new Exception(status);
                                        }
                                        else
                                        {
                                            status = jsonObject.SelectToken("message").Value<string>();

                                            if (status == "2150 : Duplicate IRN")
                                           {
                                                //JObject jsonObject1 = JObject.Parse(jsonData);
                                                JArray ResultArray = JArray.Parse(jsonObject["result"].ToString());
                                                string DescData = ResultArray[0]["Desc"].ToString();
                                                JObject jsonObject1 = JObject.Parse(DescData);
                                                vDupIrn = jsonObject1["Irn"].ToString();
                                                throw new Exception(vDupIrn);
                                            }

                                            //var userGuid = Convert.ToString(jsonObject["result"]);
                                            //var uu = JObject.Parse(userGuid);
                                            //var userGuid1 = Convert.ToString(uu["Desc"]);
                                            //var obj = jsonObject.SelectToken("result");

                                            //JArray jsonArray = JArray.Parse(jsonObject.ToString());
                                            //var userGuid = Convert.ToString(jsonObject["result"]["Desc"]["Irn"]);


                                            throw new Exception(status);

                                            //if (status == "2150 : Duplicate IRN")           //Divyang 08102020
                                            //{
                                            //    clsAuthentication vclsAuthentication = new clsAuthentication();

                                            //    string vLastValDt = DateTime.Now.Date.ToString();

                                            //    vclsAuthentication.mthGenerateToken(vGSTIN, ref vApiURL,vComp_Id, vTimeOut, ref vAutho_Token, vModuleNm, vAppUerName, ref vRequestId, vlbl, oDataAccess);

                                            //    this.mthGetDataEInv(vApiURL,tokenvalue,vAutho_Token, oDataAccess,vTimeOut,vApplText,vGSTIN,vAppUerName,vRequestId, "EINVOICE_RET", vlbl,username,password,gstin,bodyString);
                                            //}
                                            //else
                                            //{
                                            //    throw new Exception(status);
                                            // }
                                        }
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
                MessageBox.Show(ex.ToString());
                jsonData = "Issue occured, " + ex.Message;
            }
            
            if (vDupIrn !="")       //Divyang 09102020
            {
                jsonData = vDupIrn;
                return jsonData;
            }


            if (jsonData.Contains("Issue occured, "))
            {
                clsMain vClsMain = new clsMain();
                string issueStr = jsonData.Replace("Issue occured, ", "");
                if (issueStr.Contains("Invalid Token") || issueStr.Contains("Token Expired"))
                {
                    vAutho_Token = string.Empty;
                    //vErrMsg = issueStr;
                    //return;
                }
                if (issueStr.Substring(issueStr.Length - 1, 1) == ",")
                {
                    issueStr = issueStr.Substring(0, issueStr.Length - 1);
                    issueStr = vClsMain.mthGetErrorList(issueStr, oDataAccess, vTimeOut, vApplText);
                }

                vClsMain.mthUpdate_ApiLog("E-Invoice Response", DateTime.Now, vAppUerName, issueStr, vRequestId, vModuleNm, oDataAccess);
                //vErrMsg = issueStr;
                if (issueStr.Contains("2230"))
                {
                    issueStr = "2230 : This IRN cannot be cancelled because  e-way Bill has been generated. Please cancel e-way Bill through 'e-way Bill Generation' in UERP or NIC Portal manually and then try cancelling IRN.";
                    System.Windows.Forms.MessageBox.Show(gstin + " - " + issueStr, vApplText, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(gstin + " - " + issueStr, vApplText, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                }

                vlbl.Text = "Ready";
                vAutho_Token = string.Empty;
                //return;
            }
            return jsonData;
        }
        //Added By Rupesh G on 27/01/2020 for bug no.----End

        //Added by Divyang on 08102020 Start
        public string mthGetDataEInv(string vApiURL, string tokenvalue, string vAutho_Token, DataAccess_Net.clsDataAccess oDataAccess, short vTimeOut, string vApplText, string vGSTIN, string vAppUerName, string vRequestId, string vModuleNm, System.Windows.Forms.ToolStripLabel vlbl, string username, string password, string gstin, string bodyString)
        {
            string requestId = gstin.Trim() + DateTime.Now.ToString("yyyyMMddHHmmssFFF").Trim();

            string jsonData = string.Empty;

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(vApiURL);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", tokenvalue);
            //gstin = "01AMBPG7773M002";   //Comm Divyang
            // username = "adqgspjkusr1";
            request.Headers.Add("user_name", username);
            request.Headers.Add("password", password);
            request.Headers.Add("gstin", gstin.Trim());
            request.Headers.Add("requestid", requestId);
            request.Headers.Add("Authorization", tokenvalue);
            request.Headers.Add("Cache-Control", "no-cache");
            try
            {
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
            if (jsonData.Contains("Issue occured, "))
            {
                clsMain vClsMain = new clsMain();
                string issueStr = jsonData.Replace("Issue occured, ", "");
                if (issueStr.Contains("Invalid Token") || issueStr.Contains("Token Expired"))
                {
                    vAutho_Token = string.Empty;
                    //vErrMsg = issueStr;
                    //return;
                }
                if (issueStr.Substring(issueStr.Length - 1, 1) == ",")
                {
                    issueStr = issueStr.Substring(0, issueStr.Length - 1);
                    issueStr = vClsMain.mthGetErrorList(issueStr, oDataAccess, vTimeOut, vApplText);
                }

                vClsMain.mthUpdate_ApiLog("E-Invoice Response", DateTime.Now, vAppUerName, issueStr, vRequestId, vModuleNm, oDataAccess);
                //vErrMsg = issueStr;
                System.Windows.Forms.MessageBox.Show(gstin + " - " + issueStr, vApplText, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

                vlbl.Text = "Ready";
                vAutho_Token = string.Empty;
                //return;
            }
            return jsonData;
        }
        //Added by Divyang on 08102020 End


        public void mthUpdate_ApiLog(string logType, DateTime loggedTime, string loggedUser, string logText, string requestID, string vModuleNm, DataAccess_Net.clsDataAccess oDataAccess)
        {
            string vSqlStr;
            vSqlStr = "set DateFormat dmy Insert Into Adq_GSP_API_Log(logType,loggedTime,loggedUser,respStatus,requestId,ModuleNm) Values ('" + logType + "','" + loggedTime + "','" + loggedUser + "','" + logText.Replace("'", "") + "','" + requestID + "','" + vModuleNm + "')"; //Rup 14/11/2019
            oDataAccess.ExecuteSQLStatement(vSqlStr, null, 0, true);
        }
        public string mthGetErrorList(string errorCode, DataAccess_Net.clsDataAccess oDataAccess, short vTimeOut, string vApplText)
        {
            string errorDesc = string.Empty;
            string vSqlStr = "";

            try
            {
                vSqlStr = "Select Errorcode,ErrorDesc From Adq_GSP_API_ErrList Where ErrorCode in (" + errorCode + ")";
                System.Data.DataTable tblErrors = new System.Data.DataTable();
                oDataAccess.GetDataTable(vSqlStr, null, vTimeOut);
                for (int i = 0; i < tblErrors.Rows.Count; i++)
                {
                    errorDesc = errorDesc + tblErrors.Rows[i]["Errorcode"].ToString() + ":" + tblErrors.Rows[i]["ErrorDesc"].ToString() + "\n";
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, vApplText, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return errorDesc;
        }
    }
    public class clsAuthentication
    {
        public string NicId, NiCPwd, GSTIN;
        private string gspname = "gsp.adaequare.com";
        //string _gspid = "79D0D4E854FE4A308E1604184592D98B";    //Rup  
        //string _gspPwd = "018851BEG188BG4891GB129GB2213A70367A";
        public void mthGenerateToken(ref string vUserGSTIN, ref string vApiURL, string vComp_Id, short vTimeOut, ref string vAutho_Token, string vModuleNm, string vAppUerName, ref string vRequestId, System.Windows.Forms.ToolStripLabel vlbl, DataAccess_Net.clsDataAccess oDataAccess)
        {

            clsMain vClsMain = new clsMain();
            string vAuthoURL = "", vAppPath;
            string[] vApiLicenseFile = null;
            string vSqlStr = "";
            string vGSPId = "", vGSPPwd = "";
            string vDsNicId, vDsNiCPwd, vCo_Name;

            oDataAccess = new DataAccess_Net.clsDataAccess();
            vSqlStr = "Select Co_Name,GSTIN,nicuid,nicpwd,IRNID,IRNPWD From vudyog..Co_Mast Where CompId=" + vComp_Id;
            System.Data.DataTable tblCoMast = new System.Data.DataTable();
            tblCoMast = oDataAccess.GetDataTable(vSqlStr, null, vTimeOut);
            vCo_Name = tblCoMast.Rows[0]["Co_Name"].ToString();
            //vCo_Name = "OM VINYLS PVT. LTD.";

            //vCo_Name = "Udyog Softwre Test Product API";

            

            vUserGSTIN = tblCoMast.Rows[0]["GSTIN"].ToString();
            if (vModuleNm == "EINVOICE_GEN" || vModuleNm == "EINVOICE_CAN" || vModuleNm == "EINVOICE_EWB_CAN" || vModuleNm == "EINVOICE_RET")
            {

                vDsNicId = tblCoMast.Rows[0]["IRNID"].ToString();
                vDsNiCPwd = tblCoMast.Rows[0]["IRNPWD"].ToString();

            }
            else
            {
                vDsNicId = tblCoMast.Rows[0]["nicuid"].ToString();
                vDsNiCPwd = tblCoMast.Rows[0]["nicpwd"].ToString();

            }

            NicId = vDsNicId;//RRG
            NiCPwd = VU_UDFS.NewDECRY(VU_UDFS.HexDecimalToASCII(vDsNiCPwd.Trim()), "Ud*yog+1993NIC"); //RRG
            MessageBox.Show(NiCPwd.ToString());
            //NiCPwd = "Gsp@1234";
            GSTIN = vUserGSTIN;//RRG
            
            vRequestId = vUserGSTIN.Trim() + DateTime.Now.ToString("yyyyMMddHHmmssFFF").Trim();

            vAppPath = Application.ExecutablePath;
            vAppPath = Path.GetDirectoryName(vAppPath);

            
            
            clsAdqApiLicenseValidation vclsAdqApiLicenseValidation = new clsAdqApiLicenseValidation();
            vclsAdqApiLicenseValidation.mthReadUrlSettings(vModuleNm, ref vAuthoURL, ref vApiURL, ref vApiLicenseFile, vUserGSTIN, vDsNicId, vDsNiCPwd, vAppPath, vlbl);
            if (vUserGSTIN.Trim().Length <= 0)  //Added by Divyang
            {
                vlbl.Text = "NCO_GSTIN";
                return;
            }
            
            vclsAdqApiLicenseValidation.mthReadClientLicense(vModuleNm, vCo_Name, ref vGSPId, ref vGSPPwd, vDsNicId, vDsNiCPwd, vAppPath, vlbl);
            
            if (vDsNicId.Trim().Length <= 0 || vDsNiCPwd.Trim().Length <= 0)   //added by Divyang 
            {
                vlbl.Text = "NO NICID";
                return;
            }
            if (vGSPId == null || vGSPPwd == null || vGSPId == string.Empty || vGSPPwd == string.Empty)   //Added by Divyang
            {
                vlbl.Text = "NO_Lic_Val";
                return;
            }
            
            udAdqGSPHelper.clsAuthentication vAutho = new udAdqGSPHelper.clsAuthentication();
            if (vAutho_Token == string.Empty)
            {
                vlbl.Text = "Authenticating...";
                vAutho_Token = vAutho.mthGetAuthentication(vAuthoURL, vGSPId, vGSPPwd);
            
                if (vAutho_Token.Contains("Issue occured,"))
                {
                    string issueStr = vAutho_Token.Replace("Issue occured, ", "");
                    System.Windows.Forms.MessageBox.Show(issueStr);

                    vClsMain.mthUpdate_ApiLog("Authentication", DateTime.Now, vAppUerName, issueStr, string.Empty, vModuleNm, oDataAccess);
                    vlbl.Text = "Ready";
                    vAutho_Token = string.Empty;
                    //return;
                }
                else
                {
                    vClsMain.mthUpdate_ApiLog("Authentication", DateTime.Now, vAppUerName, "Successful", string.Empty, vModuleNm, oDataAccess);
                }
                vlbl.Text = "Requesting GSTIN Search for requestid: " + vRequestId;
            }

        }
        public string mthGetAuthentication(string urlAddress, string gspId, string gspPwd)
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
                    string json = "{\"gspappid\":\"" + gspId + "\"," + "\"gspappsecret\":\"" + gspPwd + "\"}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (System.Net.HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    jsonData = streamReader.ReadToEnd();
                }

                var jsonObject = JObject.Parse(jsonData);
                jsonData = (string)jsonObject.SelectToken("access_token");

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


    }
    public class clsAdqApiLicenseValidation
    {
        public void mthReadUrlSettings(string vURLName, ref string vAuthoURL, ref string vApiURL, ref string[] vApiLicenseFile, string vUserGSTIN, string vDsNicId, string vDsNiCPwd, string pAppPath, System.Windows.Forms.ToolStripLabel vlbl)
        {
            string vDupIRN = vApiURL.Trim();
           
            
            string vNiCPwd;
            if (vUserGSTIN.Trim().Length <= 0)
            {
                System.Windows.Forms.MessageBox.Show("GSTIN of Company cannot be Empty...!!!");
                vlbl.Text = "Ready";
                return;
            }
            
            vApiLicenseFile = Directory.GetFiles(Path.Combine(pAppPath, ""), "*API Lic.Licx");
            if (vApiLicenseFile.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("License file of GSP is not found. Cannot continue...!!!");
                vlbl.Text = "Ready";
                return;
            }
            vlbl.Text = "Reading url Setting...";
            
            vAuthoURL = System.Configuration.ConfigurationManager.AppSettings["GSPAuth"].ToString();
            vApiURL = System.Configuration.ConfigurationManager.AppSettings[vURLName].ToString(); //Rup 14/11/2019
            
            if (vApiURL.Contains("##IRN##"))
            {
                vApiURL = vApiURL.Replace("##IRN##", vDupIRN);
            }
            
            //  vNiCPwd = VU_UDFS.NewDECRY(VU_UDFS.HexDecimalToASCII(vDsNicId.Trim()), "Ud*yog+1993NIC");
            vNiCPwd = VU_UDFS.NewDECRY(VU_UDFS.HexDecimalToASCII(vDsNiCPwd.Trim()), "Ud*yog+1993NIC");  //Comm Divyang
            //vNiCPwd = "Gsp@1234";
        }
        public void mthReadClientLicense(string vModuleNm, string vCo_Name, ref string vGSPId, ref string vGSPPwd, string vDsNicId, string vNiCPwd, string pAppPath, System.Windows.Forms.ToolStripLabel vlbl)
        {
            vlbl = new ToolStripLabel();
            vlbl.Text = "Verifying License...";
            clsAdqApiLicenseValidation _LicReader = new clsAdqApiLicenseValidation();
            //vCo_Name = "Udyog Softwre Test Product API";
            // vCo_Name = "OM VINYLS PVT. LTD.";
            string[] _licread = _LicReader.mthChkClientLicense(pAppPath, vCo_Name.Trim(), "udewaygen");
            vGSPId = _licread[0].ToString();
            vGSPPwd = _licread[1].ToString();
            if (vGSPId == null || vGSPPwd == null || vGSPId == string.Empty || vGSPPwd == string.Empty)
            {
                //System.Windows.Forms.MessageBox.Show("License file of "+ vModuleNm + " is not valid. Cannot continue...!!!");
                System.Windows.Forms.MessageBox.Show("License file of API is not valid. Cannot continue...!!!");  //modified by Divyang Panchal for Tkt-33066 on 16/12/2019
                //vlbl.Text = "License File is Not Valid";
                vlbl.Text = "Ready";       //Divyang 14032020 
                return;
            }
            if (vDsNicId.Trim().Length <= 0 || vNiCPwd.Trim().Length <= 0)
            {
                System.Windows.Forms.MessageBox.Show("Please enter GSP User Id/Password in NIC credentials in Control Centre.");
                vlbl.Text = "Ready";
                return;
            }
        }
        public string[] mthChkClientLicense(string _appPath, string _compNm, string _module)
        {
            string[] ClientCred = new string[2];
            ClientCred[0] = "";
            ClientCred[1] = "";
            //MessageBox.Show("L1" + _appPath);
            string[] ApiLicenseFile = Directory.GetFiles(_appPath + "\\", "*API Lic.Licx");
            //MessageBox.Show("L2" + ApiLicenseFile.Length.ToString());
            if (ApiLicenseFile.Length == 0)
            {
                return ClientCred;
            }

            string _Filepath = ApiLicenseFile[0];
            //MessageBox.Show("L3" + _Filepath);
            if (!File.Exists(_Filepath))
            {
                return ClientCred;
            }
            FileStream stream = File.Open(_Filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1252));
            //MessageBox.Show("L6" + _Filepath);
            string fileText = reader.ReadToEnd();
            //MessageBox.Show("L6" + fileText);
            reader.Close();
            stream.Close();

            string _PartyNm = fileText.Substring(0, 50).Trim();
            int _lens = 1;
            int _lens1 = 0;
            string _licenseStr = "";


            for (_lens = 50; _lens < fileText.Length;)
            {
                _lens1 = fileText.Length - _lens >= 50 ? 50 : fileText.Length - _lens;
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
                    string _cn = VU_UDFS.dec(VU_UDFS.NewDECRY(_compDet[0].Replace("<<e1~>>", "").Replace("CN:", ""), _PartyNm));
                    if (_compNm.Trim() == _cn.Trim())
                    {
                        string _mi = VU_UDFS.dec(VU_UDFS.NewDECRY(_compDet[1].Replace("<<e1~>>", "").Replace("MI:", ""), _PartyNm));
                        _mi = _mi.Trim();
                        string _pv = VU_UDFS.dec(VU_UDFS.NewDECRY(_compDet[2].Replace("<<e1~>>", "").Replace("PV:", ""), _mi));

                        string[] _md = _compDet[3].Replace("<<e1~>>", "").Split(new[] { "<<~2s>>" }, StringSplitOptions.RemoveEmptyEntries);
                        string[] _mdlst;
                        bool lexit = false;

                        foreach (string _str2 in _md)
                        {
                            _mdlst = _str2.Replace("<<e2~>>", "").Split(new[] { "<<~3s>>" }, StringSplitOptions.RemoveEmptyEntries);
                            if (_mdlst.Length == 3)
                            {
                                string _mn = VU_UDFS.NewDECRY(_mdlst[0].Replace("<<e3~>>", "").Replace("PC:", ""), _mi);

                                if (_module.Trim() == _mn.Trim())
                                {
                                    ClientCred[0] = VU_UDFS.NewDECRY(_mdlst[1].Replace("<<e3~>>", "").Replace("UI:", ""), _mi);
                                    ClientCred[1] = VU_UDFS.NewDECRY(_mdlst[2].Replace("<<e3~>>", "").Replace("PW:", ""), _mi);
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

    public class clsGSTINSearch
    {
        public void mthCheckGSTIN(string vComp_Id, string vdsDbNm, string vdsDbServNm, string vDbUserId, string vDbPass, string vGSTIN, string vAc_Id, string vModuleNm, string vAppUerName, short vTimeOut, string vApplText, System.Windows.Forms.ToolStripLabel vlbl)
        {
            vModuleNm = vModuleNm.ToUpper();
            string vSqlStr = "";
            string vApiURL = "";
            string vUserGSTIN = "", vErrMsg = "";
            DataAccess_Net.clsDataAccess oDataAccess;
            DataAccess_Net.clsDataAccess._databaseName = vdsDbNm;
            DataAccess_Net.clsDataAccess._serverName = vdsDbServNm;
            DataAccess_Net.clsDataAccess._userID = vDbUserId;
            DataAccess_Net.clsDataAccess._password = vDbPass;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            System.Windows.Forms.Button sender = new System.Windows.Forms.Button();
            EventArgs e = new EventArgs();
            string vRequestId = "";
            string vAutho_Token = "";

            clsMain vClsMain = new clsMain();
            clsAdqApiLicenseValidation vclsAdqApiLicenseValidation = new clsAdqApiLicenseValidation();
            clsAuthentication vclsAuthentication = new clsAuthentication();
            string vLastValDt = DateTime.Now.Date.ToString();



            int vCnt = 0;

            clsGSTINSeachSchema vGSTINSeachSchema = null;
            clsGSTINSeachSchema.pradr vGSTINSeachSchema_pradr = null;
            clsGSTINSeachSchema.pradr.addr vGSTINSeachSchema_pradr_addr = null;
            vclsAuthentication.mthGenerateToken(ref vUserGSTIN, ref vApiURL, vComp_Id, vTimeOut, ref vAutho_Token, vModuleNm, vAppUerName, ref vRequestId, vlbl, oDataAccess);

            if (vlbl.Text == "NCO_GSTIN") //Added by Divyang
            {
                return;
            }
            if (vlbl.Text == "NO_Lic_Val")   //Added by Divyang
            {
                return;
            }
            if (vlbl.Text == "NO NICID")  //Added by Divyang
            {
                return;
            }
            vApiURL = vApiURL + "TP&gstin=" + vGSTIN;
            string vTokenvalue = "Bearer " + vAutho_Token;
            string vGSTNSearchDetail = vClsMain.mthGetData(vApiURL, vTokenvalue, vAutho_Token, oDataAccess, vTimeOut, vApplText, vGSTIN, vAppUerName, vRequestId, vModuleNm, vlbl);
            if (vGSTNSearchDetail.Contains("Issue occured, "))
            {
                vErrMsg = vGSTNSearchDetail.Replace("Issue occured, ", "");
            }
            else
            {
                vlbl.Text = "Validate";
                System.Data.DataTable dtGSTINdetail = new System.Data.DataTable();
                JObject GSTINdetails = JObject.Parse(vGSTNSearchDetail);
                vGSTINSeachSchema = JsonConvert.DeserializeObject<clsGSTINSeachSchema>((GSTINdetails).SelectToken("result").ToString());
                vGSTINSeachSchema_pradr = JsonConvert.DeserializeObject<clsGSTINSeachSchema.pradr>((GSTINdetails).SelectToken("result").SelectToken("pradr").ToString());
                vGSTINSeachSchema_pradr_addr = JsonConvert.DeserializeObject<clsGSTINSeachSchema.pradr.addr>((GSTINdetails).SelectToken("result").SelectToken("pradr").SelectToken("addr").ToString());
            }

            if (vAc_Id != "0")
            {
                vSqlStr = "Delete From Ac_Mast_GSTIN Where Ac_Id=" + vAc_Id;
                oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);
            }
            else
            {
                vSqlStr = "Delete From Ac_Mast_GSTIN Where Ac_Id=" + vAc_Id + " and GSTIN='" + vGSTIN + "'";
                oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);
            }
            vSqlStr = "";
            if (vGSTINSeachSchema != null)
            {
                vSqlStr = "set dateformat dmy Insert Into Ac_Mast_GSTIN (Ac_Id,GSTIN,LastUpdated,RegDt,GSTIN_Stat,LastValDt,Trade_Nm,Legal_Nm,GSTIN_Match,Legal_Nm_Match,Trade_Nm_Match";
                vSqlStr = vSqlStr + ",stjCd,stj,dty,cxdt,ctb,bnm,loc,st,bno,stcd,dst,city,flno,lt,pncd,lg,ctjCd,ctj) Values(";
                vSqlStr = vSqlStr + vAc_Id + ",'" + vGSTIN + "','" + vGSTINSeachSchema.lstupdt + "','" + vGSTINSeachSchema.rgdt + "','" + vGSTINSeachSchema.sts + "','" + vLastValDt + "','" + vGSTINSeachSchema.tradeNam + "','" + vGSTINSeachSchema.lgnm + "','','',''";
                vSqlStr = vSqlStr + ",'" + vGSTINSeachSchema.stjCd + "','" + vGSTINSeachSchema.stj + "','" + vGSTINSeachSchema.dty + "','" + vGSTINSeachSchema.cxdt + "','" + vGSTINSeachSchema.ctb + "','" + vGSTINSeachSchema_pradr_addr.bnm + "','" + vGSTINSeachSchema_pradr_addr.loc + "','" + vGSTINSeachSchema_pradr_addr.st + "','" + vGSTINSeachSchema_pradr_addr.bno + "','" + vGSTINSeachSchema_pradr_addr.stcd + "','" + vGSTINSeachSchema_pradr_addr.dst + "','" + vGSTINSeachSchema_pradr_addr.city + "','" + vGSTINSeachSchema_pradr_addr.flno + "','" + vGSTINSeachSchema_pradr_addr.lt + "','" + vGSTINSeachSchema_pradr_addr.pncd + "','" + vGSTINSeachSchema_pradr_addr.lg + "','" + vGSTINSeachSchema.ctjCd + "','" + vGSTINSeachSchema.ctj + "')";
                oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);
                vCnt = vCnt + 1;
                vClsMain.mthUpdate_ApiLog("GSTIN Search Response", DateTime.Now, vAppUerName, vGSTINSeachSchema.ToString(), vRequestId, vModuleNm, oDataAccess);
            }
            if (vErrMsg != "")
            {
                vSqlStr = "set dateformat dmy Insert Into Ac_Mast_GSTIN (Ac_Id,GSTIN,LastUpdated,RegDt,GSTIN_Stat,LastValDt,Trade_Nm,Legal_Nm,GSTIN_Match,Legal_Nm_Match,Trade_Nm_Match) Values( ";
                vSqlStr = vSqlStr + vAc_Id + ",'" + vGSTIN + "','" + "" + "','" + "" + "','" + vErrMsg.Replace("'", "") + "','" + vLastValDt + "','" + "" + "','" + "" + "','','','')";
                oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);
                vCnt = vCnt + 1;
            }
            if (vCnt > 0)
            {
                vSqlStr = "update  a Set a.GSTIN_Match=(case when (a.GSTIN=ac.GSTIN) and (a.Legal_Nm=ac.Ac_Name) and (a.Trade_Nm=ac.MailName) Then 1 Else 0 End),a.Legal_Nm_Match=(case when a.Legal_Nm=ac.Ac_Name Then 1 Else 0 End),a.Trade_Nm_Match=(case when a.Trade_Nm=ac.MailName Then 1 Else 0 End) From Ac_Mast_GSTIN  a Inner Join Ac_Mast ac on (a.Ac_Id=ac.Ac_Id) Where LastValDt='" + vLastValDt + "'";
                oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);
            }

            vlbl.Text = "Received " + vModuleNm + " response for requestid: " + vRequestId;

        }

        public class clsGSTINSeachSchema
        {
            public string stjCd { get; set; }
            public string lgnm { get; set; }
            public string stj { get; set; }
            public string dty { get; set; }
            public string cxdt { get; set; }
            //public string nba { get; set; }
            public string gstin { get; set; }
            public string lstupdt { get; set; }
            public string rgdt { get; set; }
            public string ctb { get; set; }
            public class pradr
            {
                public class addr
                {
                    public string bnm { get; set; }
                    public string loc { get; set; }
                    public string st { get; set; }
                    public string bno { get; set; }
                    public string stcd { get; set; }
                    public string dst { get; set; }
                    public string city { get; set; }
                    public string flno { get; set; }
                    public string lt { get; set; }
                    public string pncd { get; set; }
                    public string lg { get; set; }
                }
                //public string ntr { get; set; }
            }
            public string tradeNam { get; set; }
            public string sts { get; set; }

            public string ctjCd { get; set; }
            public string ctj { get; set; }
        }
    }
    public class clsGSTRStatus
    {
        public void mthCheckGSTRStatus(string vFinyear, string vAc_Id, string vComp_Id, string vdsDbNm, string vdsDbServNm, string vDbUserId, string vDbPass, string vGSTIN, string vModuleNm, string vAppUerName, short vTimeOut, string vApplText, System.Windows.Forms.ToolStripLabel vlbl)
        {
            vModuleNm = vModuleNm.ToUpper();

            string vSqlStr = "";
            string vApiURL = "";
            string vUserGSTIN = "", vErrMsg = "";
            DataAccess_Net.clsDataAccess oDataAccess;
            DataAccess_Net.clsDataAccess._databaseName = vdsDbNm;
            DataAccess_Net.clsDataAccess._serverName = vdsDbServNm;
            DataAccess_Net.clsDataAccess._userID = vDbUserId;
            DataAccess_Net.clsDataAccess._password = vDbPass;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            System.Windows.Forms.Button sender = new System.Windows.Forms.Button();
            EventArgs e = new EventArgs();
            string vRequestId = "";
            string vAutho_Token = "";

            clsMain vClsMain = new clsMain();
            clsAdqApiLicenseValidation vclsAdqApiLicenseValidation = new clsAdqApiLicenseValidation();
            clsAuthentication vclsAuthentication = new clsAuthentication();

            string vLastValDt = DateTime.Now.Date.ToString();

            vclsAuthentication.mthGenerateToken(ref vUserGSTIN, ref vApiURL, vComp_Id, vTimeOut, ref vAutho_Token, vModuleNm, vAppUerName, ref vRequestId, vlbl, oDataAccess);

            if (vAutho_Token == "")
            {
                return;
            }

            vApiURL = vApiURL + "RETTRACK&gstin=" + vGSTIN.Trim() + "&fy=" + vFinyear;
            string vTokenvalue = "Bearer " + vAutho_Token;
            string vGSTRStatus = vClsMain.mthGetData(vApiURL, vTokenvalue, vAutho_Token, oDataAccess, vTimeOut, vApplText, vGSTIN, vAppUerName, vRequestId, vModuleNm, vlbl);

            if (vGSTRStatus.Contains("Issue occured, "))
            {
                vErrMsg = vGSTRStatus.Replace("Issue occured, ", "");
            }
            else
            {
                vlbl.Text = "Validate";
                System.Data.DataTable dtGSTRStatus = new System.Data.DataTable();
                JObject GSTRdetails = JObject.Parse(vGSTRStatus);
                clsGSTRStatusSchema vGSTReturns = JsonConvert.DeserializeObject<clsGSTRStatusSchema>((GSTRdetails).SelectToken("result").ToString());

                vSqlStr = "Delete From Ac_Mast_GSTR Where ac_id=" + vAc_Id + " and GSTIN='" + vGSTIN + "'";
                oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);


                for (int i = 0; i <= vGSTReturns.EFiledlist.Count - 1; i++)
                {
                    string vvalid;
                    try
                    {
                        vvalid = vGSTReturns.EFiledlist[i].valid.ToString().Trim();
                    }
                    catch (Exception)
                    {
                        vvalid = "";
                    }


                    vSqlStr = "Set Dateformat dmy Insert Into Ac_Mast_GSTR (ac_id,GSTIN,valid,mof,dof,ret_prd,rtntype,arn,[status],Fin_Year,LastValDt) Values(";
                    vSqlStr = vSqlStr + "'" + vAc_Id + "','" + vGSTIN + "','" + vvalid + "','" + vGSTReturns.EFiledlist[i].mof.ToString().Trim() + "','" + vGSTReturns.EFiledlist[i].dof.ToString().Trim() + "','" + vGSTReturns.EFiledlist[i].ret_prd.ToString().Trim() + "','" + vGSTReturns.EFiledlist[i].rtntype.ToString().Trim() + "','" + vGSTReturns.EFiledlist[i].arn.ToString().Trim() + "','" + vGSTReturns.EFiledlist[i].status.ToString().Trim() + "','" + vFinyear + "','" + vLastValDt + "')";
                    oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);

                }

            }

        }
        public class clsGSTRStatusSchema
        {
            public List<lstEFiledlist> EFiledlist { get; set; }
        }
        public class lstEFiledlist
        {
            public string valid { get; set; }
            public string mof { get; set; }
            public string dof { get; set; }
            public string rtntype { get; set; }
            public string ret_prd { get; set; }
            public string arn { get; set; }
            public string status { get; set; }
        }


    }

    public class clseInvoice
    {
        public string E_INV_NO = "";
        public void mthGeneInvoice(string vComp_Id, string vdsDbNm, string vdsDbServNm, string vDbUserId, string vDbPass, string vEntry_Ty, string vTran_cd, string vBcode_nm, string vModuleNm, string vAppUerName, short vTimeOut, string vApplText, System.Windows.Forms.ToolStripLabel vlbl)
        {
            E_INV_NO = "";
            string bodyString;
            vModuleNm = vModuleNm.ToUpper();
            string vSqlStr = "";
            string vApiURL = "";
            string vUserGSTIN = "", vErrMsg = "";
            DataAccess_Net.clsDataAccess oDataAccess;
            DataAccess_Net.clsDataAccess._databaseName = vdsDbNm;
            DataAccess_Net.clsDataAccess._serverName = vdsDbServNm;
            DataAccess_Net.clsDataAccess._userID = vDbUserId;
            DataAccess_Net.clsDataAccess._password = vDbPass;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            System.Windows.Forms.Button sender = new System.Windows.Forms.Button();
            EventArgs e = new EventArgs();
            string vRequestId = "";
            string vAutho_Token = "";

            clsEInvoiceJsonGen vclsEInvoiceJsonGen = new clsEInvoiceJsonGen();
            bodyString = vclsEInvoiceJsonGen.mthGeneInvoiceJson(vComp_Id, vdsDbNm, vdsDbServNm, vDbUserId, vDbPass, vEntry_Ty, vTran_cd, vBcode_nm, vModuleNm, vAppUerName, vTimeOut, vApplText);

            if (vclsEInvoiceJsonGen.vErrMsg != "")
            {
                MessageBox.Show("Please rectify the below points and then generate the e-Invoice." + Environment.NewLine + vclsEInvoiceJsonGen.vErrMsg, vApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                vlbl.Text = "Data Entry Validation Failed";
                return;
            }
            clsMain vClsMain = new clsMain();
            clsAdqApiLicenseValidation vclsAdqApiLicenseValidation = new clsAdqApiLicenseValidation();
            clsAuthentication vclsAuthentication = new clsAuthentication();
            string vLastValDt = DateTime.Now.Date.ToString();

            int vCnt = 0;

            clseInvoiceSchema veInvoiceSchema = null;

            vclsAuthentication.mthGenerateToken(ref vUserGSTIN, ref vApiURL, vComp_Id, vTimeOut, ref vAutho_Token, vModuleNm, vAppUerName, ref vRequestId, vlbl, oDataAccess);

            if (vlbl.Text == "NCO_GSTIN") //Added by Divyang
            {
                return;
            }
            if (vlbl.Text == "NO_Lic_Val")   //Added by Divyang
            {
                return;
            }
            if (vlbl.Text == "NO NICID")  //Added by Divyang
            {
                return;
            }
            //  vApiURL = vApiURL + "TP&gstin=" + vGSTIN;
            string vTokenvalue = "Bearer " + vAutho_Token;
            string veInvoiceDetail = vClsMain.mthPostData(vApiURL, vTokenvalue, vAutho_Token, oDataAccess, vTimeOut, vApplText, "", vAppUerName, vRequestId, vModuleNm, vlbl, vclsAuthentication.NicId, vclsAuthentication.NiCPwd, vclsAuthentication.GSTIN, bodyString);


            if (veInvoiceDetail.Contains("Issue occured, "))
            {

                return;
                //vErrMsg = veInvoiceDetail.Replace("Issue occured, ", "");

                //vclsAuthentication.mthGenerateToken(ref vUserGSTIN, ref vApiURL, vComp_Id, vTimeOut, ref vAutho_Token, "EINVOICE_RET", vAppUerName, ref vRequestId, vlbl, oDataAccess);
                //vTokenvalue = "Bearer " + vAutho_Token;
                //string veInvoiceDetail1 = vClsMain.mthGetDataEInv(vApiURL, vTokenvalue, vAutho_Token, oDataAccess, vTimeOut, vApplText, "", vAppUerName, vRequestId, "EINVOICE_RET", vlbl, vclsAuthentication.NicId, vclsAuthentication.NiCPwd, vclsAuthentication.GSTIN, bodyString);

            }
            if (veInvoiceDetail.Length == 64)       //Divyang 09102020
            {
                vApiURL = veInvoiceDetail.Trim();
                vModuleNm = "EINVOICE_RET";
                vclsAuthentication.mthGenerateToken(ref vUserGSTIN, ref vApiURL, vComp_Id, vTimeOut, ref vAutho_Token, vModuleNm, vAppUerName, ref vRequestId, vlbl, oDataAccess);
                //vclsAuthentication.mthGenerateToken(ref vUserGSTIN, ref vApiURL, vComp_Id, vTimeOut, ref vAutho_Token, "EINVOICE_RET", vAppUerName, ref vRequestId, vlbl, oDataAccess);
                vTokenvalue = "Bearer " + vAutho_Token;
                string veInvoiceDetail1 = vClsMain.mthGetDataEInv(vApiURL, vTokenvalue, vAutho_Token, oDataAccess, vTimeOut, vApplText, "", vAppUerName, vRequestId, "EINVOICE_RET", vlbl, vclsAuthentication.NicId, vclsAuthentication.NiCPwd, vclsAuthentication.GSTIN, bodyString);

                
                vlbl.Text = "E-Invoice fetched successfully";
                System.Data.DataTable dteInvoicedetail = new System.Data.DataTable();
                JObject eInvoicedetails = JObject.Parse(veInvoiceDetail1);
                veInvoiceSchema = JsonConvert.DeserializeObject<clseInvoiceSchema>((eInvoicedetails).SelectToken("result").ToString());
                E_INV_NO = veInvoiceSchema.Irn;
                
                string vtblMain = "";           //Divyang
                if (vEntry_Ty == "ST")
                {
                    vtblMain = "STMAINADD";
                }
                if (vEntry_Ty == "EI")      //Divyang 28082020
                {
                    vtblMain = "STMAINADD";
                }
                if (vEntry_Ty == "GC")
                {
                    vtblMain = "CNMAIN";
                }
                if (vEntry_Ty == "GD")
                {
                    vtblMain = "DNMAIN";
                }
                if (vEntry_Ty == "SR")
                {
                    vtblMain = "SRMAIN";
                }
                if (vEntry_Ty == "PR")
                {
                    vtblMain = "PRMAIN";
                }
                if (vBcode_nm == "ST")      //30122020
                {
                    vtblMain = "STMAINADD";
                }

                

                if (vEntry_Ty != "" && vTran_cd != "" && E_INV_NO != "")
                {
                    //vSqlStr = "set dateformat dmy update stmainadd set E_INV_NO='" + veInvoiceSchema.Irn + "' ,AckDt=cast('" + veInvoiceSchema.AckDt + "' as date),AckNo='" + veInvoiceSchema.AckNo + "',SignedInvoice='" + veInvoiceSchema.SignedInvoice + "',SignedQRCode='" + veInvoiceSchema.SignedQRCode + "', Status='" + veInvoiceSchema.Status + "'";
                    //vSqlStr = "set dateformat dmy update " + vtblMain + " set E_INV_NO='" + veInvoiceSchema.Irn + "' ,AckDt=cast('" + veInvoiceSchema.AckDt + "' as datetime),AckNo='" + veInvoiceSchema.AckNo + "',SignedInvoice='" + veInvoiceSchema.SignedInvoice + "',SignedQRCode='" + veInvoiceSchema.SignedQRCode + "', Status='" + veInvoiceSchema.Status + "'";
                    vSqlStr = "set dateformat dmy update " + vtblMain + " set E_INV_NO='" + veInvoiceSchema.Irn + "' ,AckDt=convert(datetime,'" + veInvoiceSchema.AckDt + "',120),AckNo='" + veInvoiceSchema.AckNo + "',SignedInvoice='" + veInvoiceSchema.SignedInvoice + "',SignedQRCode='" + veInvoiceSchema.SignedQRCode + "', Status='" + veInvoiceSchema.Status + "'";
                    vSqlStr = vSqlStr + " where entry_ty='" + vEntry_Ty + "' and tran_cd='" + vTran_cd + "'";
                    oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);


                    if (veInvoiceSchema.EwbNo != null)
                    {
                        if (vtblMain == "STMAINADD") { vtblMain = "STMAIN"; }
                        vSqlStr = " set dateformat dmy update " + vtblMain + " set ewbn='" + veInvoiceSchema.EwbNo + "',ewbdt=cast('" + veInvoiceSchema.EwbDt + "' as date),EWBVFD=cast('" + veInvoiceSchema.EwbDt + "' as date),EWBVFT=left(cast('" + veInvoiceSchema.EwbDt + "' as time),8),EWBVTD=cast('" + veInvoiceSchema.EwbValidTill + "' as date),EWBVTT=left(cast('" + veInvoiceSchema.EwbValidTill + "' as time),8) ";
                        vSqlStr = vSqlStr + " where entry_ty='" + vEntry_Ty + "' and tran_cd='" + vTran_cd + "'";
                        oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);

                        if (vtblMain == "STMAIN") { vtblMain = "STMAINADD"; }
                        vSqlStr = " set dateformat dmy update " + vtblMain + " set IrnEWBStatus='ACT' ";
                        vSqlStr = vSqlStr + " where entry_ty='" + vEntry_Ty + "' and tran_cd='" + vTran_cd + "'";
                        oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);

                        //// EWB QRCode 02012020 Start

                        QRCodeEncoder oQRcode = new QRCodeEncoder();
                        oQRcode.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                        oQRcode.QRCodeScale = 4;
                        oQRcode.QRCodeVersion = 7;
                        oQRcode.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                        //if (vDsCommon.Tables[2].Columns.Contains("BC_Size") && vDsCommon.Tables[2].Rows.Count > 0)
                        //{
                        //    oQRcode.QRCodeScale = Convert.ToInt16(vDsCommon.Tables[2].Rows[0]["BC_Size"]);
                        //}

                        Bitmap bmp = oQRcode.Encode(Convert.ToString(veInvoiceSchema.EwbNo + "/" + vUserGSTIN + "/" + Convert.ToString(veInvoiceSchema.EwbDt)));
                        byte[] bytes = (byte[])TypeDescriptor.GetConverter(bmp).ConvertTo(bmp, typeof(byte[]));


                        vSqlStr = "set dateformat dmy Execute Usp_ent_Update_EWBQR @EWBNQR,@Entry_ty,@Tran_cd,@Bcode_nm";

                        string sqlconnstr = "Data Source=" + vdsDbServNm + ";Initial Catalog=" + vdsDbNm + ";Uid=" + vDbUserId + ";Pwd=" + vDbPass;

                        SqlConnection conn = new SqlConnection(sqlconnstr);
                        SqlCommand cmd = new SqlCommand(vSqlStr, conn);


                        cmd.Parameters.Add("@EWBNQR", SqlDbType.Image).Value = bytes;
                        cmd.Parameters.Add("@Entry_ty", SqlDbType.VarChar).Value = vEntry_Ty;            // 01/06/2018 
                        cmd.Parameters.Add("@Tran_cd", SqlDbType.NChar).Value = vTran_cd;
                        cmd.Parameters.Add("@BCode_nm", SqlDbType.VarChar).Value = vBcode_nm;            // 01/06/2018 

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();




                        //// EWB QRCode 02012020 End


                    }

                    vclsEInvoiceJsonGen.mthQRCode(vEntry_Ty, vTran_cd, vBcode_nm);  //30122020

                }

            }
            else
            {

                vlbl.Text = "Validate";
                System.Data.DataTable dteInvoicedetail = new System.Data.DataTable();
                JObject eInvoicedetails = JObject.Parse(veInvoiceDetail);
                veInvoiceSchema = JsonConvert.DeserializeObject<clseInvoiceSchema>((eInvoicedetails).SelectToken("result").ToString());
                E_INV_NO = veInvoiceSchema.Irn;

                string vtblMain = "";           //Divyang
                if (vEntry_Ty=="ST")
                {
                    vtblMain = "STMAINADD";
                }
                if (vEntry_Ty == "EI")      //Divyang 28082020
                {
                    vtblMain = "STMAINADD";
                }
                if (vEntry_Ty == "GC")
                {
                    vtblMain = "CNMAIN";
                }
                if (vEntry_Ty == "GD")
                {
                    vtblMain = "DNMAIN";
                }
                if (vEntry_Ty == "SR")
                {
                    vtblMain = "SRMAIN";
                }
                if (vEntry_Ty == "PR")
                {
                    vtblMain = "PRMAIN";
                }
                if (vBcode_nm == "ST")      //30122020
                {
                    vtblMain = "STMAINADD";
                }


                if (vEntry_Ty != "" && vTran_cd != "" && E_INV_NO != "")
                {
                    //vSqlStr = "set dateformat dmy update stmainadd set E_INV_NO='" + veInvoiceSchema.Irn + "' ,AckDt=cast('" + veInvoiceSchema.AckDt + "' as date),AckNo='" + veInvoiceSchema.AckNo + "',SignedInvoice='" + veInvoiceSchema.SignedInvoice + "',SignedQRCode='" + veInvoiceSchema.SignedQRCode + "', Status='" + veInvoiceSchema.Status + "'";
                    //vSqlStr = "set dateformat dmy update "+vtblMain+" set E_INV_NO='" + veInvoiceSchema.Irn + "' ,AckDt=cast('" + veInvoiceSchema.AckDt + "' as datetime),AckNo='" + veInvoiceSchema.AckNo + "',SignedInvoice='" + veInvoiceSchema.SignedInvoice + "',SignedQRCode='" + veInvoiceSchema.SignedQRCode + "', Status='" + veInvoiceSchema.Status + "'";
                    vSqlStr = "set dateformat dmy update " + vtblMain + " set E_INV_NO='" + veInvoiceSchema.Irn + "' ,AckDt=convert(datetime,'" + veInvoiceSchema.AckDt + "',120),AckNo='" + veInvoiceSchema.AckNo + "',SignedInvoice='" + veInvoiceSchema.SignedInvoice + "',SignedQRCode='" + veInvoiceSchema.SignedQRCode + "', Status='" + veInvoiceSchema.Status + "'";
                    vSqlStr = vSqlStr + " where entry_ty='" + vEntry_Ty + "' and tran_cd='" + vTran_cd + "'";
                    oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);

                   
                    if (veInvoiceSchema.EwbNo != null)
                    {
                        if (vtblMain == "STMAINADD") { vtblMain = "STMAIN"; }
                        vSqlStr = " set dateformat dmy update " + vtblMain + " set ewbn='" + veInvoiceSchema.EwbNo + "',ewbdt=cast('" + veInvoiceSchema.EwbDt + "' as date),EWBVFD=cast('" + veInvoiceSchema.EwbDt + "' as date),EWBVFT=left(cast('" + veInvoiceSchema.EwbDt + "' as time),8),EWBVTD=cast('" + veInvoiceSchema.EwbValidTill + "' as date),EWBVTT=left(cast('" + veInvoiceSchema.EwbValidTill + "' as time),8) ";
                        vSqlStr = vSqlStr + " where entry_ty='" + vEntry_Ty + "' and tran_cd='" + vTran_cd + "'";
                        oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);

                        if (vtblMain == "STMAIN") { vtblMain = "STMAINADD"; }
                        vSqlStr = " set dateformat dmy update " + vtblMain + " set IrnEWBStatus='ACT' ";
                        vSqlStr = vSqlStr + " where entry_ty='" + vEntry_Ty + "' and tran_cd='" + vTran_cd + "'";
                        oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);



                        //// EWB QRCode 02012020 Start

                        QRCodeEncoder oQRcode = new QRCodeEncoder();
                        oQRcode.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                        oQRcode.QRCodeScale = 4;
                        oQRcode.QRCodeVersion = 7;
                        oQRcode.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                        //if (vDsCommon.Tables[2].Columns.Contains("BC_Size") && vDsCommon.Tables[2].Rows.Count > 0)
                        //{
                        //    oQRcode.QRCodeScale = Convert.ToInt16(vDsCommon.Tables[2].Rows[0]["BC_Size"]);
                        //}

                        Bitmap bmp = oQRcode.Encode(Convert.ToString(veInvoiceSchema.EwbNo + "/"+vUserGSTIN+"/" + Convert.ToString(veInvoiceSchema.EwbDt)));
                        byte[] bytes = (byte[])TypeDescriptor.GetConverter(bmp).ConvertTo(bmp, typeof(byte[]));


                        vSqlStr = "set dateformat dmy Execute Usp_ent_Update_EWBQR @EWBNQR,@Entry_ty,@Tran_cd,@Bcode_nm";

                        string sqlconnstr = "Data Source=" + vdsDbServNm + ";Initial Catalog=" + vdsDbNm + ";Uid=" + vDbUserId + ";Pwd=" + vDbPass;    

                        SqlConnection conn = new SqlConnection(sqlconnstr);
                        SqlCommand cmd = new SqlCommand(vSqlStr, conn);

                        
                        cmd.Parameters.Add("@EWBNQR", SqlDbType.Image).Value = bytes;
                        cmd.Parameters.Add("@Entry_ty", SqlDbType.VarChar).Value = vEntry_Ty;            // 01/06/2018 
                        cmd.Parameters.Add("@Tran_cd", SqlDbType.NChar).Value = vTran_cd;
                        cmd.Parameters.Add("@BCode_nm", SqlDbType.VarChar).Value = vBcode_nm;            // 01/06/2018 

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();




                        //// EWB QRCode 02012020 End






                    }

                    vclsEInvoiceJsonGen.mthQRCode(vEntry_Ty, vTran_cd, vBcode_nm);          //30122020

                }

            }

            vlbl.Text = "Received " + vModuleNm + " response for requestid: " + vRequestId;


        }

        public class clseInvoiceSchema
        {
            public string AckDt { get; set; }
            public string AckNo { get; set; }
            public string Irn { get; set; }
            public string SignedInvoice { get; set; }
            public string SignedQRCode { get; set; }
            public string Status { get; set; }
            public string EwbDt { get; set; }
            public string EwbNo { get; set; }
            public string EwbValidTill { get; set; }

        }
    }

    public class clseInvoiceCancel
    {
        public string E_INV_NO = "";
        public void mthCaneInvoice(string vComp_Id, string vdsDbNm, string vdsDbServNm, string vDbUserId, string vDbPass, string vEntry_Ty, string vTran_cd, String vBcode_nm, string vModuleNm, string vAppUerName, short vTimeOut, string vApplText, System.Windows.Forms.ToolStripLabel vlbl)
        {
            E_INV_NO = "";
            string bodyString;
            vModuleNm = vModuleNm.ToUpper();
            string vSqlStr = "";
            string vApiURL = "";
            string vUserGSTIN = "", vErrMsg = "";
            DataAccess_Net.clsDataAccess oDataAccess;
            DataAccess_Net.clsDataAccess._databaseName = vdsDbNm;
            DataAccess_Net.clsDataAccess._serverName = vdsDbServNm;
            DataAccess_Net.clsDataAccess._userID = vDbUserId;
            DataAccess_Net.clsDataAccess._password = vDbPass;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            // System.Windows.Forms.Button sender = new System.Windows.Forms.Button();
            // EventArgs e = new EventArgs();

            string vRequestId = "";
            string vAutho_Token = "";

            clsEInvoiceJsonGen vclsEInvoiceJsonGen = new clsEInvoiceJsonGen();
            bodyString = vclsEInvoiceJsonGen.mthCaneInvoiceJson(vComp_Id, vdsDbNm, vdsDbServNm, vDbUserId, vDbPass, vEntry_Ty, vTran_cd, vBcode_nm, vModuleNm, vAppUerName, vTimeOut, vApplText);

            //if (vclsEInvoiceJsonGen.vErrMsg != "")
            //{
            //    MessageBox.Show("Please rectify the below points and then generate the e-Invoice." + Environment.NewLine + vclsEInvoiceJsonGen.vErrMsg, vApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    vlbl.Text = "Data Entry Validation Failed";
            //    return;
            //}
            clsMain vClsMain = new clsMain();
            clsAdqApiLicenseValidation vclsAdqApiLicenseValidation = new clsAdqApiLicenseValidation();
            clsAuthentication vclsAuthentication = new clsAuthentication();
            string vLastValDt = DateTime.Now.Date.ToString();
            int vCnt = 0;
            clsCaneInvoiceSchema vCaneInvoiceSchema = null;
            vclsAuthentication.mthGenerateToken(ref vUserGSTIN, ref vApiURL, vComp_Id, vTimeOut, ref vAutho_Token, vModuleNm, vAppUerName, ref vRequestId, vlbl, oDataAccess);
            if (vlbl.Text == "NCO_GSTIN") //Added by Divyang
            {
                return;
            }
            if (vlbl.Text == "NO_Lic_Val")   //Added by Divyang
            {
                return;
            }
            if (vlbl.Text == "NO NICID")  //Added by Divyang
            {
                return;
            }
            //  vApiURL = vApiURL + "TP&gstin=" + vGSTIN;
            string vTokenvalue = "Bearer " + vAutho_Token;
            string vCanceleInvoiceDetail = vClsMain.mthPostData(vApiURL, vTokenvalue, vAutho_Token, oDataAccess, vTimeOut, vApplText, "", vAppUerName, vRequestId, vModuleNm, vlbl, vclsAuthentication.NicId, vclsAuthentication.NiCPwd, vclsAuthentication.GSTIN, bodyString);
            if (vCanceleInvoiceDetail.Contains("Issue occured, "))
            {
                vErrMsg = vCanceleInvoiceDetail.Replace("Issue occured, ", "");
            }
            else
            {
                vlbl.Text = "e-Invoice canceled";
                System.Data.DataTable dteInvoicedetail = new System.Data.DataTable();
                JObject eInvoicedetails = JObject.Parse(vCanceleInvoiceDetail);
                vCaneInvoiceSchema = JsonConvert.DeserializeObject<clsCaneInvoiceSchema>((eInvoicedetails).SelectToken("result").ToString());
                E_INV_NO = vCaneInvoiceSchema.Irn;

                string vtblMain = "";           //Divyang
                if (vEntry_Ty == "ST")
                {
                    vtblMain = "STMAINADD";
                }
                if (vEntry_Ty == "EI")
                {
                    vtblMain = "STMAINADD";
                }
                if (vEntry_Ty == "GC")
                {
                    vtblMain = "CNMAIN";
                }
                if (vEntry_Ty == "GD")
                {
                    vtblMain = "DNMAIN";
                }
                if (vEntry_Ty == "SR")
                {
                    vtblMain = "SRMAIN";
                }
                if (vEntry_Ty == "PR")
                {
                    vtblMain = "PRMAIN";
                }
                if (vBcode_nm == "ST")      //30122020
                {
                    vtblMain = "STMAINADD";
                }

                if (vEntry_Ty != "" && vTran_cd != "" && E_INV_NO != "")
                {
                    vSqlStr = "set dateformat dmy update "+vtblMain+" set IrnCanDt=cast('" + vCaneInvoiceSchema.CancelDate + "' as date), Status='Cancelled'";
                    vSqlStr = vSqlStr + " where entry_ty='" + vEntry_Ty + "' and tran_cd='" + vTran_cd + "'";
                    oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);
             

                }
            }
            if (vErrMsg != "")
            {
                vlbl.Text = "vErrMsg";
            }
            else
            {
                vlbl.Text = "Received " + vModuleNm + " response for requestid: " + vRequestId;
            }
        }

        public class clsCaneInvoiceSchema
        {         
            public string CancelDate { get; set; }
            public string Irn { get; set; }           
        }
    }


    public class clseInvoiceEwbCancel              //Divyang 05092020
    {
        public string E_WAY_NO = "";
        public void mthCaneInvoiceEwb(string vComp_Id, string vdsDbNm, string vdsDbServNm, string vDbUserId, string vDbPass, string vEntry_Ty, string vTran_cd, string vBcode_nm, string vModuleNm, string vAppUerName, short vTimeOut, string vApplText, System.Windows.Forms.ToolStripLabel vlbl)
        {
            E_WAY_NO = "";
            string bodyString;
            vModuleNm = vModuleNm.ToUpper();
            string vSqlStr = "";
            string vApiURL = "";
            string vUserGSTIN = "", vErrMsg = "";
            DataAccess_Net.clsDataAccess oDataAccess;
            DataAccess_Net.clsDataAccess._databaseName = vdsDbNm;
            DataAccess_Net.clsDataAccess._serverName = vdsDbServNm;
            DataAccess_Net.clsDataAccess._userID = vDbUserId;
            DataAccess_Net.clsDataAccess._password = vDbPass;
            oDataAccess = new DataAccess_Net.clsDataAccess();

            // System.Windows.Forms.Button sender = new System.Windows.Forms.Button();
            // EventArgs e = new EventArgs();

            string vRequestId = "";
            string vAutho_Token = "";

            clsEInvoiceJsonGen vclsEInvoiceEwbJsonGen = new clsEInvoiceJsonGen();
            bodyString = vclsEInvoiceEwbJsonGen.mthCaneInvoiceEwbJson(vComp_Id, vdsDbNm, vdsDbServNm, vDbUserId, vDbPass, vEntry_Ty, vTran_cd, vBcode_nm, vModuleNm, vAppUerName, vTimeOut, vApplText);

            //if (vclsEInvoiceJsonGen.vErrMsg != "")
            //{
            //    MessageBox.Show("Please rectify the below points and then generate the e-Invoice." + Environment.NewLine + vclsEInvoiceJsonGen.vErrMsg, vApplText, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    vlbl.Text = "Data Entry Validation Failed";
            //    return;
            //}
            clsMain vClsMain = new clsMain();
            clsAdqApiLicenseValidation vclsAdqApiLicenseValidation = new clsAdqApiLicenseValidation();
            clsAuthentication vclsAuthentication = new clsAuthentication();
            string vLastValDt = DateTime.Now.Date.ToString();
            int vCnt = 0;
            clsCaneInvoiceEwbSchema vCaneInvoiceEwbSchema = null;
            vclsAuthentication.mthGenerateToken(ref vUserGSTIN, ref vApiURL, vComp_Id, vTimeOut, ref vAutho_Token, vModuleNm, vAppUerName, ref vRequestId, vlbl, oDataAccess);
            if (vlbl.Text == "NCO_GSTIN") //Added by Divyang
            {
                return;
            }
            if (vlbl.Text == "NO_Lic_Val")   //Added by Divyang
            {
                return;
            }
            if (vlbl.Text == "NO NICID")  //Added by Divyang
            {
                return;
            }
            //  vApiURL = vApiURL + "TP&gstin=" + vGSTIN;
            string vTokenvalue = "Bearer " + vAutho_Token;
            string vCanceleInvoiceEwbDetail = vClsMain.mthPostData(vApiURL, vTokenvalue, vAutho_Token, oDataAccess, vTimeOut, vApplText, "", vAppUerName, vRequestId, vModuleNm, vlbl, vclsAuthentication.NicId, vclsAuthentication.NiCPwd, vclsAuthentication.GSTIN, bodyString);
            if (vCanceleInvoiceEwbDetail.Contains("Issue occured, "))
            {
                vErrMsg = vCanceleInvoiceEwbDetail.Replace("Issue occured, ", "");
            }
            else
            {
                vlbl.Text = "e-Way Bill is cancelled";
                System.Data.DataTable dteInvoiceEwbdetail = new System.Data.DataTable();
                JObject eInvoiceEwbdetails = JObject.Parse(vCanceleInvoiceEwbDetail);
                vCaneInvoiceEwbSchema = JsonConvert.DeserializeObject<clsCaneInvoiceEwbSchema>((eInvoiceEwbdetails).SelectToken("result").ToString());
                E_WAY_NO = vCaneInvoiceEwbSchema.ewayBillNo;

                string vtblMain = "";           //Divyang
                if (vEntry_Ty == "ST")
                {
                    vtblMain = "STMAINADD";
                }
                if (vEntry_Ty == "EI")
                {
                    vtblMain = "STMAINADD";
                }
                if (vEntry_Ty == "GC")
                {
                    vtblMain = "CNMAIN";
                }
                if (vEntry_Ty == "GD")
                {
                    vtblMain = "DNMAIN";
                }
                if (vEntry_Ty == "SR")
                {
                    vtblMain = "SRMAIN";
                }
                if (vEntry_Ty == "PR")
                {
                    vtblMain = "PRMAIN";
                }
                if (vBcode_nm == "ST")      //30122020
                {
                    vtblMain = "STMAINADD";
                }

                if (vEntry_Ty != "" && vTran_cd != "" && E_WAY_NO != "")
                {
                    vSqlStr = "set dateformat dmy update " + vtblMain + " set IrnEWBCanDt=cast('" + vCaneInvoiceEwbSchema.CancelDate + "' as date), IrnEWBStatus='Cancelled'";
                    vSqlStr = vSqlStr + " where entry_ty='" + vEntry_Ty + "' and tran_cd='" + vTran_cd + "'";
                    oDataAccess.ExecuteSQLStatement(vSqlStr, null, vTimeOut, true);


                }
            }
            if (vErrMsg != "")
            {
                vlbl.Text = "vErrMsg";
            }
            else
            {
                vlbl.Text = "Received " + vModuleNm + " response for requestid: " + vRequestId;
            }
        }

        public class clsCaneInvoiceEwbSchema
        {
            public string CancelDate { get; set; }
            public string ewayBillNo { get; set; }
        }
    }


}
