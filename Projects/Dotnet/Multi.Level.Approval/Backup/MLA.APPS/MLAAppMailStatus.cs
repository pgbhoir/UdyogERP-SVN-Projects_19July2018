using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using eMailClient.MailSender;
using MLA.ENTITY;
using MLA.LL;

//using vunettofx;

namespace MLA.APPS
{
    public partial class MLAAppMailStatus : Form
    {
        
        #region define variables
        private List<AppMailSetter> listAppMailSetter;
        public List<AppMailSetter> ListAppMailSetter
        {
            get { return listAppMailSetter; }
            set { listAppMailSetter = value; }
        }

        private string connectionstring;
        public string Connectionstring
        {
            get { return connectionstring; }
            set { connectionstring = value; }
        }

        #endregion


        #region Forms & Controls Event
        public MLAAppMailStatus()
        {
            InitializeComponent();
        }

        protected virtual void MLAAppMailStatus_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            notifyIcon.BalloonTipText = "eMail Sender running.....";
            notifyIcon.BalloonTipTitle = "eMail Sender";
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.Text = "eMail Sender";
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
                cls_Gen_Mgr_Approval m_obj_Approval = new cls_Gen_Mgr_Approval(Connectionstring);

                DataTable dtEmailSettings = m_obj_Approval.GetEmailCredentials();
                if (dtEmailSettings.Rows.Count == 0)
                {
                    MessageBox.Show("Cannot proceed, Email configuration not found....!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                cls_Gen_Mgr_MailSender m_obj_MailSender = new cls_Gen_Mgr_MailSender();
                decimal progressvalue  = 100 / ListAppMailSetter.Count();
                int percentageProgress = 0;

                foreach (AppMailSetter appmailsetter in ListAppMailSetter)
                {
                    m_obj_MailSender.FromMailAddress = Convert.ToString(dtEmailSettings.Rows[0]["username"]);
                    m_obj_MailSender.ToMailAddress = appmailsetter.Toids;
                    m_obj_MailSender.Body = appmailsetter.Mailbody;
                    m_obj_MailSender.Subject = appmailsetter.Subject;
                    m_obj_MailSender.MailId = Convert.ToString(dtEmailSettings.Rows[0]["username"]);
                    m_obj_MailSender.MailPass = m_obj_MailSender.DecryptData(Convert.ToString(dtEmailSettings.Rows[0]["password"]));
                    m_obj_MailSender.SmtpHost = Convert.ToString(dtEmailSettings.Rows[0]["host"]);
                    m_obj_MailSender.SmtpPort = Convert.ToInt32(dtEmailSettings.Rows[0]["port"]);
                    m_obj_MailSender.SmtpEnableSSL = Convert.ToBoolean(dtEmailSettings.Rows[0]["enablessl"]);
                    m_obj_MailSender.IsBodyHtml = true;
                   

                    percentageProgress = percentageProgress + (Int32)progressvalue;
                    
                    m_obj_MailSender.ConfigMail();
                    bgwkProcess.ReportProgress(percentageProgress, "Mail sending to " + appmailsetter.Toids.Trim() + ".....");

                    m_obj_MailSender.Send();
                    bgwkProcess.ReportProgress(percentageProgress, "Successfully Mail sent to " + appmailsetter.Toids.Trim() + ".");
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
                                "ITAX", MessageBoxButtons.OK,
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
                notifyIcon.BalloonTipText = "ITAX eMail Sender running at " + progBar.Value.ToString() + "%,\nClick here for Details.";
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
                MessageBox.Show("Error. Details: " + (e.Error as Exception).ToString(),"ITAX", MessageBoxButtons.OK,
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

        private void MLAAppMailStatus_Resize(object sender, EventArgs e)
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
