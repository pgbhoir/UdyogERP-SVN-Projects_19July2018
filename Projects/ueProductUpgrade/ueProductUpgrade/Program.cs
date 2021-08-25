using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ueProductUpgrade
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new frmMain());
        //}
        //  added by sandeep for bug-18141 on 14-sep-13 -->start
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                args = new string[] { "3", "U011920", "AIPLLTM001\\AIPLLTM001", "sa", "sa1985", "^18010", "ADMIN", @"C:\UdyogERP\Bmp\icon_VudyogGST.ico", "Udyog ERP", "UdyogERP.exe", "1", "udpid6096DTM20110307112715" };/*added by sandeep for bug-18141 on 14-sep-13 Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
                //args = new string[] { "1", "U011920", "AIPLDTM019\\SQL2014", "sa", "sa1985", "^18010", "ADMIN", @"d:\UdyogERP\Bmp\icon_VudyogGST.ico", "Udyog ERPENT", "UdyogERPENT.exe", "1", "udpid6096DTM20110307112715" };/*added by sandeep for bug-18141 on 14-sep-13 Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
                //args = new string[] { "67", "U011718", "AIPLDTM019\\SQLEXPRESS", "sa", "sa1985", "^18010", "ADMIN", @"e:\u3\VudyogGST\Bmp\icon_VudyogGSSDK.ico", "Udyog GST", "UdyogGST.exe", "1", "udpid6096DTM20110307112715" };/*added by sandeep for bug-18141 on 14-sep-13 Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //          Application.Run(new frmMain()); // Commented added by sandeep for bug-18141 on 14-sep-13
            Application.Run(new frmMain(args)); //added by sandeep for bug-18141 on 14-sep-13
            return 1;
            //  added by sandeep for bug-18141 on 14-sep-13 -->End
        }
    }
}
