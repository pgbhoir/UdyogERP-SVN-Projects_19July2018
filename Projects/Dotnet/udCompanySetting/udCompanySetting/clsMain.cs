using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udCompanySetting
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
                //args = new string[] { "6", "U011920", @"AIPLLTM035\SQLEXPRESS2014", "sa", "sa@1985r2", "^13015", "ADMIN", @"D:\UdyogERPSDK\Bmp\Icon_VudyogGST.ico", "UdyogERP SDK", "UdyogERPSDK", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "" };
                //args = new string[] { "11", "U041920", @"BAIPLLTH103\SQLEXPRESS2014", "sa", "sa@1985", "^13015", "ADMIN", @"E:\Divyang Panchal\Udyog\UdyogERPSDK\Bmp\Icon_VudyogGST.ico", "UdyogERP SDK", "UdyogERPSDK", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "" };
                args = new string[] { "289", "B062021", @"AIPLLTM001\AIPLLTM001", "sa", "sa1985", "^13015", "ADMIN", @"D:\VudyogGST\Bmp\Icon_VudyogGST.ico", "VUdyogGST", "VudyogGST", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "" };
                //args = new string[] { "1", "U011920", @"BAIPLLTH103\SQLEXPRESS2014", "sa", "sa@1985", "^13015", "ADMIN", @"E:\Divyang Panchal\Udyog\UdyogERPSDK\Bmp\Icon_VudyogGST.ico", "UdyogERP SDK", "UdyogERPSDK", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(args));
        }
    }
}
