using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udTextGradeMaster
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
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(args));
        }
    }
}
