using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udTextDesignMaster
{
    static class clsMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                //args = new string[] { "1", "U011920", "AIPLLTM035\\SQLEXPRESS2014", "sa", "sa@1985r2", "^21001", "ADMIN", @"D:\UdyogERP\Bmp\Icon_VudyogGST.ico", "UdyogERPSDK", "UdyogERPSDK", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "" };
                //args = new string[] { "5", "U131920", "AIPLDTM001\\SQLEXPRESS2014", "sa", "sa1985", "^21001", "ADMIN", @"D:\UdyogERPENT2.2.0\Bmp\Icon_VudyogGST.ico", "UdyogERPENT", "UdyogERPENT", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "" };
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(args));
        }
    }
}
