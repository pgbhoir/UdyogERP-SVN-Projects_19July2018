using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Threading;

namespace UdMISReports
{
    public partial class frmSendMail : DevExpress.XtraEditors.XtraForm
    {
        private string _connString;
        private string _reportName;
        private string _userName;
        private string _fileName;
        Stream _fileStream;
        private DataTable _CredentialDetail;
        
        public frmSendMail(string connString, string fileName, string reportName, string userName)
        {
            InitializeComponent();
            _connString = connString;
            _reportName = reportName;
            _fileName = fileName;
            _userName = userName;
            lblAttachmentFileName.Text = Path.GetFileName(_fileName);
        }
        public frmSendMail(string connString,string fileName, Stream fileStream, string reportName, string userName)
        {
            InitializeComponent();
            _connString = connString;
            _reportName = reportName;
            _fileName = fileName;
            _fileStream = fileStream;
            _userName = userName;
            lblAttachmentFileName.Text = Path.GetFileName(_fileName);
        }
        private void frmSendMail_Load(object sender, EventArgs e)
        {
            _CredentialDetail = this.GetCredentials();
            if (_CredentialDetail.Rows.Count <= 0)
            {
                MessageBox.Show("Auto Email SMTP setting not defined. Please update...", "Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }
            txtFrom.Text = _CredentialDetail.Rows[0]["username"].ToString();
            txtSubject.Text = "Reports : " + _reportName;
            StringBuilder mailbody = new StringBuilder();

            mailbody.AppendLine("Dear Sir / Madam,");
            mailbody.AppendLine();
            mailbody.AppendLine("Please Find the Attached Report: " + _reportName);
            mailbody.AppendLine();
            mailbody.AppendLine("Thanks & Regards");
            mailbody.AppendLine(_userName);

            txtMailBody.Text = mailbody.ToString();
        }

        private void txtSend_Click(object sender, EventArgs e)
        {
            string[] toList = new string[] { };
            string[] ccList = new string[] { };
            string[] bccList = new string[] { };

            #region validations
            if (this.txtTO.Text.Trim().Length <= 0)
            {
                MessageBox.Show("Please give atlease one To e-mail ", "Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            else
            {
                if (!this.ValidateEmailIds(this.txtTO.Text.Trim()))
                {
                    MessageBox.Show("Please check Valid TO e-mail id.\r\n May be one or more mail id format is wrong", "Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.txtTO.Focus();
                    return;
                }
                toList = this.txtTO.Text.Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (this.txtCC.Text.Trim().Length > 0)
            {
                if (!this.ValidateEmailIds(this.txtCC.Text.Trim()))
                {
                    MessageBox.Show("Please check Valid CC e-mail id.\r\n May be one or more mail id format is wrong", "Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.txtCC.Focus();
                    return;
                }
                ccList = this.txtCC.Text.Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (this.txtBCC.Text.Trim().Length > 0)
            {
                if (!this.ValidateEmailIds(this.txtBCC.Text.Trim()))
                {
                    MessageBox.Show("Please check Valid BCC e-mail id.\r\n May be one or more mail id format is wrong", "Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.txtBCC.Focus();
                    return;
                }
                bccList = this.txtBCC.Text.Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (this.txtFrom.Text.Trim().Length <= 0)
            {
                MessageBox.Show("Please check Valid From e-Mail id.\r\n May be one or more mail id format is wrong", "Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            else
            {
                if (!this.ValidateEmailIds(this.txtFrom.Text.Trim()))
                {
                    this.txtFrom.Focus();
                    return;
                }
            }

            #endregion

            if (this.txtSubject.Text.Trim().Length <= 0 || this.txtMailBody.Text.Trim().Length <= 0)
            {
                DialogResult result = MessageBox.Show("Send this message without a subject or text in the body?", "Information - Send Mail", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (DialogResult.Cancel == result)
                {
                    return;
                }
            }
            try
            {

                string strSmptHost = _CredentialDetail.Rows[0]["Host"].ToString();
                string strSmptPort = _CredentialDetail.Rows[0]["Port"].ToString();
                string struserName = _CredentialDetail.Rows[0]["username"].ToString();
                string strPassword = this.GetPassWord(_CredentialDetail.Rows[0]["password"].ToString());
                bool enableSSL = Convert.ToBoolean(_CredentialDetail.Rows[0]["enablessl"]);
                if (sendMail(toList, txtFrom.Text, ccList, bccList, txtSubject.Text, txtMailBody.Text, _fileName, strSmptHost, strSmptPort, struserName, strPassword, enableSSL))
                {
                    MessageBox.Show("Mail Sent Sucsessfully", "Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mail Not Sent Due to internal Error :  " + "\r\n" + ex.Message + "\r\n" + "please Send once again", "Information - Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                
            }

        }

        private void txtCancel_Click(object sender, EventArgs e)
        {
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }
            this.Close();
        }
        private DataTable GetCredentials()
        {
            DataTable ldt = new DataTable();
            SqlConnection oconn = new SqlConnection(_connString);
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
        private bool ValidateEmailIds(string emailIdList)
        {
            if (emailIdList.Length <= 0)
                return false;

            //Regex regex = new Regex(@"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.
            //(com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|co.in|co|in|[a-zA-Z]{2})$");
            //Regex regex = new Regex(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;,.]{0,1}\s*)+$");     //Separated by comma and Semicolon
            //string[] emails = emailIdList.Split(new char[] { ';' },StringSplitOptions.RemoveEmptyEntries);
            //foreach (var email in emails)
            //{
            //    if (regex.IsMatch(email) == false)
            //        return false;
            //}
            //return true;

            Regex regex = new Regex(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$");     //Separated by Semicolon
            return regex.IsMatch(emailIdList);
        }

        public bool sendMail(string[] strToMailID, string strFromMailID, string[] strCCMailID, string[] strBCCMailID, string strSubject, string strMailBody, string AttachmentLocation, string SMPTHost, string SMPTPort, string NetworkCredentialUserName, string NetworkCredentialPassword,bool enableSSL)
        {
            #region Configuring SMTP Client

            string strSmptHost = SMPTHost;
            string strSmptPort = SMPTPort;
            string struserName = NetworkCredentialUserName;
            string strPassword = NetworkCredentialPassword;
            string strFromAddress = strFromMailID;
            string strMailSubject = strSubject;
            string stringMailBody = strMailBody;
            string[] ToMailIDs = strToMailID;
            SmtpClient client = new SmtpClient();
            //SmtpClient client = new SmtpClient(Properties.Settings.Default.SMTPAddress);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = enableSSL;
            client.Host = strSmptHost;
            client.Port = Convert.ToInt32(strSmptPort);

            // setup Smtp authentication
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(struserName, strPassword);
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(strFromAddress);
            if (ToMailIDs != null)
            {
                foreach (string tomailid in ToMailIDs)
                {
                    msg.To.Add(new MailAddress(tomailid));
                }
            }

            if (strCCMailID != null)
            {
                foreach (string ccmailid in strCCMailID)
                {
                    msg.CC.Add(new MailAddress(ccmailid));
                }
            }

            if (strBCCMailID != null)
            {
                foreach (string bccmailid in strBCCMailID)
                {
                    msg.Bcc.Add(new MailAddress(bccmailid));
                }
            }

            #endregion

            #region Adding Mail Subject And Body

            msg.Subject = strMailSubject;
            msg.IsBodyHtml = false;
            msg.Body = strMailBody;

            #endregion

            #region Adding Attachment for mail

            //Attachment attachment = new Attachment(_fileStream, _fileName, System.Net.Mime.MediaTypeNames.Application.Octet);
            //msg.Attachments.Add(attachment);

            Attachment attachment = new Attachment(AttachmentLocation, System.Net.Mime.MediaTypeNames.Application.Octet);
            msg.Attachments.Add(attachment);
            this.lblAttachmentFileName.Text = attachment.Name;


            //if (XSLAttachmentLocation != "" && XSLAttachmentLocation != null)
            //{
            //    Attachment attachment1 = new Attachment(AttachmentLocation, System.Net.Mime.MediaTypeNames.Application.Octet);
            //    //Attachment attachment2 = new Attachment(XSLAttachmentLocation, System.Net.Mime.MediaTypeNames.Application.Octet);
            //    msg.Attachments.Add(attachment1);
            //    //msg.Attachments.Add(attachment2);
            //}
            //else
            //{
            //    Attachment attachment = new Attachment(AttachmentLocation, System.Net.Mime.MediaTypeNames.Application.Octet);
            //    msg.Attachments.Add(attachment);
            //}

            #endregion

            #region Sending Mail
            try
            {
                client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Mail Not Sent Due to internal Error :  " + "\r\n" + ex.Message + "\r\n" + "please Send once again");
            }
            finally
            {
                client.Dispose();
            }
            #endregion
        }
    }
}
