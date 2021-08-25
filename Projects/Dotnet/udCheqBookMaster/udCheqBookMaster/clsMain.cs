using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udCheqBookMaster
{
    static class clsMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                args = new string[] { "1", "U041819", "AIPLDTM024\\SQLExpress", "sa", "sa1985", "^21001", "ADMIN", @"D:\UdyogERP\Bmp\Icon_VudyogGST.ico", "UdyogERPSDK", "UdyogERPSDK", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "Cheque Management" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(args));
            return 1;
        }
    }
}
