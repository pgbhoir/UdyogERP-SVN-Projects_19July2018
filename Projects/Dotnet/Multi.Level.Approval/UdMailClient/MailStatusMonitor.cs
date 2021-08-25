using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using eMailClient.MailSender;

namespace UdMailClient
{
    public partial class MailStatusMonitor : Form
    {
        bool EmailConfig = false;
        public MailStatusMonitor()
        {
            InitializeComponent();
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

        #region Forms & Controls Event
        protected virtual void MailStatusMonitor_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            notifyIcon.BalloonTipText = "eMail Scheduler running.....";
            notifyIcon.BalloonTipTitle = "eMail Scheduler";
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.Text = "Email Scheduler";
            notifyIcon.Visible = false;


            ContextMenu obj_ContextMenu = new ContextMenu();

            MenuItem obj_MenuItem = new MenuItem();
            obj_ContextMenu.MenuItems.AddRange(new MenuItem[] { obj_MenuItem });
            obj_MenuItem.Index = 0;
            obj_MenuItem.Text = "Show Process";
            obj_MenuItem.Click += new EventHandler(obj_MenuItem_ShowProcessClick);

            obj_MenuItem = new MenuItem();
            obj_ContextMenu.MenuItems.AddRange(new MenuItem[] { obj_MenuItem });
            obj_MenuItem.Index = 1;
            obj_MenuItem.Text = "Exit";
            obj_MenuItem.Click += new EventHandler(obj_MenuItem_ExitClick);

            notifyIcon.ContextMenu = obj_ContextMenu;

            // Set BackGroundWorker Control Properties
            bgwkProcess.WorkerReportsProgress = true;
            bgwkProcess.WorkerSupportsCancellation = true;
            bgwkProcess.RunWorkerAsync();
        }

        private void Ticks(object sender, EventArgs e)
        {
            Timer tm = (Timer)sender;
            tm.Enabled = false;
            tm.Stop();
            timer.Start();
            timer.Enabled = true;
        }

        private void StartProcess(DoWorkEventArgs e)
        {
            try
            {
                DataTable DtMails = new DataTable();
                DtMails.ReadXml(XmlFilePath);

                cls_Gen_Mgr_MailSender m_obj_MailSender = new cls_Gen_Mgr_MailSender();
                decimal progressvalue = 100 / DtMails.Rows.Count;
                int percentageProgress = 0;

                foreach (DataRow dRow in DtMails.Rows)
                {
                    m_obj_MailSender.FromMailAddress = MailId;
                    m_obj_MailSender.ToMailAddress = Convert.ToString(dRow["ToMailAddress"]);
                    m_obj_MailSender.Body = Convert.ToString(dRow["Body"]);
                    m_obj_MailSender.Subject = Convert.ToString(dRow["Subject"]);
                    m_obj_MailSender.MailId = MailId;
                    m_obj_MailSender.MailPass = MailPass;// m_obj_MailSender.DecryptData(Convert.ToString(dtEmailSettings.Rows[0]["password"]));
                    m_obj_MailSender.SmtpHost = SmtpHost;
                    m_obj_MailSender.SmtpPort = SmtpPort;
                    m_obj_MailSender.SmtpEnableSSL = SmtpEnableSSL;

                    percentageProgress = percentageProgress + (Int32)progressvalue;
                    
                    m_obj_MailSender.ConfigMail();
                    bgwkProcess.ReportProgress(percentageProgress, "Mail sending to " + Convert.ToString(dRow["ToMailAddress"]).Trim() + ".....");

                    m_obj_MailSender.Send();
                    bgwkProcess.ReportProgress(percentageProgress, "Successfully Mail sent to " + Convert.ToString(dRow["ToMailAddress"]).Trim() + ".");
                }
            }
            catch (Exception Ex)
            {
                bgwkProcess.CancelAsync();
                if (bgwkProcess.CancellationPending)
                {
                    e.Cancel = true;
                }
                MessageBox.Show(Ex.Message,
                                "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
        }

        private void bgwkProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            StartProcess(e);
        }

   
        private void bgwkProcess_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblPercentage.Text = e.ProgressPercentage.ToString().Trim() + "%";

            progBar.Value = e.ProgressPercentage;
            progBar.Text = "Please wait, Processing is going on......";
            txtOutput.AppendText(e.UserState.ToString().Trim());
            txtOutput.AppendText(Environment.NewLine);

            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon.BalloonTipText = "Email Sender running at " + progBar.Value.ToString() + "%,\nClick here for Details.";
                notifyIcon.ShowBalloonTip(500);
            }
        }

        private void bgwkProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                txtOutput.AppendText(" Process has been cancelled !!!! ");
                txtOutput.AppendText(Environment.NewLine);
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error. Details: " + (e.Error as Exception).ToString(),"Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            {
                txtOutput.AppendText(" Task has been completed successfully !!!! ");
                txtOutput.AppendText(Environment.NewLine);
                progBar.Value = 100;
                lblPercentage.Text = 100.ToString().Trim() + "%";
                Timer tmWait = new Timer();
                tmWait.Interval = 600;
                tmWait.Enabled = true;
                tmWait.Start();
                tmWait.Tick += new EventHandler(Ticks);

             }
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > 0.5)
            {
                this.Opacity -= 0.05;
            }
            else
            {
                timer.Enabled = false;
                this.Close();
            } 
        }

        private void MailStatusMonitor_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                this.Hide();
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);
            }
            else
            {
                if (FormWindowState.Normal == WindowState)
                {
                    notifyIcon.Visible = false;
                }
            }
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        #endregion

        #region Notifier Events
        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            ShowProcess();
        }
        #endregion

        #region Context Menu Events
        private void obj_MenuItem_ExitClick(object Sender, EventArgs e)
        {
            Application.Exit();
        }

        private void obj_MenuItem_ShowProcessClick(object Sender, EventArgs e)
        {
            ShowProcess();
        }
        #endregion

        #region Private Methods
        private void ShowProcess()
        {
            if (FormWindowState.Minimized == WindowState)
            {
                this.Show();
                WindowState = FormWindowState.Normal;
                notifyIcon.Visible = false;
            }
        }
        #endregion
    }
}
