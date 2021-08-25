using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UdMailClient
{
    public class UdMailClient
    {
        public UdMailClient()
        {
        }

        #region define variables and classes
       
        private string fromMailAddress;
        public string FromMailAddress
        {
            get { return fromMailAddress; }
            set { fromMailAddress = value; }
        }

        private string toMailAddress;
        public string ToMailAddress
        {
            get { return toMailAddress; }
            set { toMailAddress = value; }
        }

        private string body;
        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        private string subject;
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string mailId;
        public string MailId
        {
            get { return mailId; }
            set { mailId = value; }
        }

        private string mailPass;
        public string MailPass
        {
            get { return mailPass; }
            set { mailPass = value; }
        }

        private string smtpHost;
        public string SmtpHost
        {
            get { return smtpHost; }
            set { smtpHost = value; }
        }

        private int smtpPort;
        public int SmtpPort
        {
            get { return smtpPort; }
            set { smtpPort = value; }
        }

        private bool smtpEnableSSL;
        public bool SmtpEnableSSL
        {
            get { return smtpEnableSSL; }
            set { smtpEnableSSL = value; }
        }

        private string xmlFilePath;
        public string XmlFilePath
        {
            get { return xmlFilePath; }
            set { xmlFilePath = value; }
        }

        #endregion


        public void Send()
        {

        }
        
    }
}
