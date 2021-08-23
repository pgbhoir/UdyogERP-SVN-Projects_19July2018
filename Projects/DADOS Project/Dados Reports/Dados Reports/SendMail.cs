using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using System.Net;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using LogicalLibrary;

namespace DadosReports
{
    public partial class SendMail : DevExpress.XtraEditors.XtraForm
    {
        #region Local Veriables

        string AttachmentFilePath = string.Empty;
        string AttachmentFile = string.Empty;
        string AttachmentFileXSL = string.Empty;
        string AttachmentFilePathXSL = string.Empty;
        string[] ToMails = null;
        string[] CCMails = null;
        string[] BCCMails = null;
        string[] FormMails = null;
        bool validMailIDs = false;
        string ReportsName = string.Empty;
        string ReportLoginUser = string.Empty;
        private DataTable _CredentialDetail;            //Added by Shrikant S. on 30/01/2019    for Bug-32212
        private string sqlconstr = string.Empty;        //Added by Shrikant S. on 30/01/2019    for Bug-32212
        CommonInfo commonInfo = new CommonInfo();

        #endregion

        #region Constructor
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public SendMail(string AttachmentPath, string AttachmentFileName, string ReportName, string LoginUser,string SqlConStr)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            if (AttachmentFileName.Contains(".xml"))
            {
                AttachmentFilePath = AttachmentPath;
                AttachmentFile = AttachmentFileName;
                AttachmentFilePathXSL = AttachmentPath;
                AttachmentFileXSL = AttachmentFileName;

                AttachmentFilePathXSL = AttachmentFilePathXSL.Replace("xml", "xsl");
                AttachmentFileXSL = AttachmentFileXSL.Replace("xml", "xsl");
                StringBuilder fileNames = new StringBuilder();
                fileNames.AppendLine(AttachmentFile + " ;");
                fileNames.AppendLine(AttachmentFileXSL);
                lblAttachmentFileName.Text = fileNames.ToString();// AttachmentFile + " ;  " + AttachmentFileXSL;

            }
            else
            {
                AttachmentFilePath = AttachmentPath;
                AttachmentFile = AttachmentFileName;
                lblAttachmentFileName.Text = AttachmentFile;
            }
            ReportsName = ReportName;
            ReportLoginUser = LoginUser;
            sqlconstr = SqlConStr;          //Added by Shrikant S. on 30/01/2019 for Bug-32212
            //lblAttachmentFileName.Text=
            // TODO: Add any constructor code after InitializeComponent call
        }
        #endregion

        #region Form Events

        private void SendMail_Load(object sender, EventArgs e)
        {
            _CredentialDetail = this.GetCredentials();

            //string strFromMailAdd = ConfigurationManager.AppSettings["NetworkCredentialUserName"].ToString();

            txtFrom.Text = _CredentialDetail.Rows[0]["username"].ToString(); 
            txtSubject.Text = "Dados Reports : " + ReportsName;
            StringBuilder mailbody = new StringBuilder();

            mailbody.AppendLine("Dear");
            mailbody.AppendLine();
            mailbody.AppendLine("Please Find the Attached Dados Report: " + ReportsName);
            mailbody.AppendLine();
            mailbody.AppendLine("Thanks & Regards");
            mailbody.AppendLine(ReportLoginUser);

            txtMailBody.Text = mailbody.ToString();
            
        }

        private void txtCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTO.Text != "")
                {
                    ToMails = txtTO.Text.Split(new char[] { ';' });

                    foreach (string Tomail in ToMails)
                    {
                        if (commonInfo.IsValidEmail(Tomail))
                        {
                            validMailIDs = true;
                        }
                        else
                        {
                            MessageBox.Show("Please Enter Valid To Mail ID ! \r\n may be one or more mail id format is wronge", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            validMailIDs = false;
                            txtTO.Focus();
                            break;
                        }
                    }
                    if (validMailIDs)
                    {
                        validMailIDs = false;
                        if (txtFrom.Text != "")
                        {
                            FormMails = txtFrom.Text.Split(new char[] { ';' });
                            if (FormMails.Length > 1)
                            {
                                MessageBox.Show("Please Enter only one From Mail ID !", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                validMailIDs = false;
                                txtFrom.Focus();
                            }
                            foreach (string Formmail in FormMails)
                            {
                                if (commonInfo.IsValidEmail(Formmail))
                                {
                                    validMailIDs = true;
                                }
                                else
                                {
                                    MessageBox.Show("Please Enter Valid From Mail ID ! \r\n may be one or more mail id format is wronge", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                    validMailIDs = false;
                                    txtFrom.Focus();
                                    break;
                                }
                            }
                            if (validMailIDs)
                            {
                                if (txtSubject.Text != "")
                                {
                                    bool verificetion = false;
                                    if (txtCC.Text != "")
                                    {
                                        CCMails = txtCC.Text.Split(new char[] { ';' });

                                        foreach (string CCmail in CCMails)
                                        {
                                            if (commonInfo.IsValidEmail(CCmail))
                                            {
                                                validMailIDs = true;
                                            }
                                            else
                                            {
                                                MessageBox.Show("Please Enter Valid CC Mail ID ! \r\n may be one or more mail id format is wronge", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                                validMailIDs = false;
                                                txtCC.Focus();
                                                break;
                                            }
                                        }
                                        if (validMailIDs)
                                        {
                                            verificetion = true;
                                        }
                                        else
                                        {
                                            MessageBox.Show("PLease Enter Valid CC Mail ID !", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                            txtCC.Text = "";
                                            txtCC.Focus();
                                        }
                                    }

                                    if (txtBCC.Text != "")
                                    {
                                        BCCMails = txtBCC.Text.Split(new char[] { ';' });

                                        foreach (string BCCmail in BCCMails)
                                        {
                                            if (commonInfo.IsValidEmail(BCCmail))
                                            {
                                                validMailIDs = true;
                                            }
                                            else
                                            {
                                                MessageBox.Show("Please Enter Valid BCC Mail ID ! \r\n may be one or more mail id format is wronge", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                                validMailIDs = false;
                                                txtBCC.Focus();
                                                break;
                                            }
                                        }
                                        if (validMailIDs)
                                        {
                                            verificetion = true;
                                        }
                                        else
                                        {
                                            MessageBox.Show("PLease Enter Valid BCC Mail ID !", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                            txtBCC.Text = "";
                                            txtBCC.Focus();
                                        }
                                    }

                                    if (txtCC.Text == "")
                                    {
                                        verificetion = true;
                                    }

                                    if (txtBCC.Text == "")
                                    {
                                        verificetion = true;
                                    }

                                    if (verificetion == true)
                                    {
                                        if (txtMailBody.Text == "")
                                        {
                                            DialogResult DR = MessageBox.Show("Mail Body Is Empty, Are You Sure You Want To Send Mail ??", "Information - Send Mail", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                            if (DR == DialogResult.Yes)
                                            {
                                                try
                                                {
                                                    //Commented by Shrikant S. on 30/01/2019 for Bug-32212      //Start
                                                    //string strSmptHost = ConfigurationManager.AppSettings["SMPTHost"].ToString();
                                                    //string strSmptPort = ConfigurationManager.AppSettings["SMPTPort"].ToString();
                                                    //string struserName = ConfigurationManager.AppSettings["NetworkCredentialUserName"].ToString();
                                                    //string strPassword = ConfigurationManager.AppSettings["NetworkCredentialPassword"].ToString();
                                                    //Commented by Shrikant S. on 30/01/2019 for Bug-32212      //End

                                                    //Added by Shrikant S. on 30/01/2019 for Bug-32212      //Start
                                                    string strSmptHost = _CredentialDetail.Rows[0]["Host"].ToString();
                                                    string strSmptPort = _CredentialDetail.Rows[0]["Port"].ToString();
                                                    string struserName = _CredentialDetail.Rows[0]["username"].ToString();
                                                    string strPassword = this.GetPassWord(_CredentialDetail.Rows[0]["password"].ToString());
                                                    bool enableSSL = Convert.ToBoolean(_CredentialDetail.Rows[0]["enablessl"]);
                                                    //Added by Shrikant S. on 30/01/2019 for Bug-32212      //End


                                                    if (commonInfo.sendMail(ToMails, txtFrom.Text, CCMails, BCCMails, txtSubject.Text, txtMailBody.Text, AttachmentFilePath, AttachmentFilePathXSL,strSmptHost,strSmptPort,struserName,strPassword, enableSSL))
                                                    {
                                                        MessageBox.Show("Mail Sent Sucsessfully", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                                        this.Close();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show("Mail Not Sent Due to internal Error :  " + "\r\n" + ex.Message + "\r\n" + "please Send once again", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                                }
                                            }
                                            else
                                            {
                                                txtMailBody.Text = "";
                                                txtMailBody.Focus();
                                            }

                                        }
                                        else
                                        {
                                            try
                                            {
                                                //Commented by Shrikant S. on 30/01/2019 for Bug-32212      //Start
                                                //string strSmptHost = ConfigurationManager.AppSettings["SMPTHost"].ToString();
                                                //string strSmptPort = ConfigurationManager.AppSettings["SMPTPort"].ToString();
                                                //string struserName = ConfigurationManager.AppSettings["NetworkCredentialUserName"].ToString();
                                                //string strPassword = ConfigurationManager.AppSettings["NetworkCredentialPassword"].ToString();
                                                //Commented by Shrikant S. on 30/01/2019 for Bug-32212      //End

                                                //Added by Shrikant S. on 30/01/2019 for Bug-32212      //Start
                                                string strSmptHost = _CredentialDetail.Rows[0]["Host"].ToString();
                                                string strSmptPort = _CredentialDetail.Rows[0]["Port"].ToString();
                                                string struserName = _CredentialDetail.Rows[0]["username"].ToString();
                                                string strPassword = this.GetPassWord(_CredentialDetail.Rows[0]["password"].ToString());
                                                bool enableSSL = Convert.ToBoolean(_CredentialDetail.Rows[0]["enablessl"]);
                                                //Added by Shrikant S. on 30/01/2019 for Bug-32212      //End

                                                if (commonInfo.sendMail(ToMails, txtFrom.Text, CCMails, BCCMails, txtSubject.Text, txtMailBody.Text, AttachmentFilePath, AttachmentFilePathXSL, strSmptHost, strSmptPort, struserName, strPassword, enableSSL))
                                                {
                                                    MessageBox.Show("Mail Sent Sucsessfully", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                                    this.Close();
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("Mail Not Sent Due to internal Error :  " + "\r\n" + ex.Message + "\r\n" + "please Send once again", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("PLease Enter The To Subject Of The Mail !", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                    txtSubject.Text = "";
                                    txtSubject.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("PLease Enter Valid From Mail ID !", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                txtFrom.Text = "";
                                txtFrom.Focus();
                            }


                        }
                        else
                        {
                            MessageBox.Show("PLease Enter The From Mail ID !", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            txtFrom.Text = "";
                            txtFrom.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("PLease Enter Valid To Mail ID !", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        txtTO.Text = "";
                        txtTO.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("PLease Enter The To Mail ID !", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    txtTO.Text = "";
                    txtTO.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :  " + ex.Message, "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        #endregion

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        //Added by Shrikant S. on 30/01/2019 for Bug-32212      //Start       
        private DataTable GetCredentials()
        {
            DataTable ldt = new DataTable();
            SqlConnection oconn = new SqlConnection(sqlconstr);
            SqlDataAdapter lda = new SqlDataAdapter("Select * From eMailSettings", oconn);
            lda.Fill(ldt);
            return ldt;
        }
        private string GetPassWord(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }
        //Added by Shrikant S. on 30/01/2019 for Bug-32212      //End

    }
}