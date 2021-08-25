using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLA.ENTITY
{
    public class AppMailSetter
    {
        public AppMailSetter()
        {
        }

        private string toids;
        public string Toids
        {
            get { return toids; }
            set { toids = value; }
        }

        private string ccids;
        public string Ccids
        {
            get { return ccids; }
            set { ccids = value; }
        }

        private string bccids;
        public string Bccids
        {
            get { return bccids; }
            set { bccids = value; }
        }

        private string subject;
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string mailbody;
        public string Mailbody
        {
            get { return mailbody; }
            set { mailbody = value; }
        }

    }
}
