using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace ugstEWayBill
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
                //args = new string[] { "67", "U011718", "AIPLDTM019\\SQLEXPRESS", "sa", "sa1985", "^21001", "ADMIN", @"E:\U3\VudyogGST\Bmp\Icon_VudyogGST.ico", "uGST SDK", "UdyogGST", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "eWAY BIll Generation" };
                args = new string[] { "1", "U011819", "AIPLDTM019\\SQLEXPRESS", "sa", "ShrikantShedekar", "^21468", "ADMIN", @"E:\U3\UdyogERPSDK\Bmp\icon_VudyogGSSDK.ico", "UdyogERPSDK", "UdyogERPSDK", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "eWAY BIll Generation" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(args));
            return 1;
        }
    }
}
