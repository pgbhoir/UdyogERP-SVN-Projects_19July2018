using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udCountryMast
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
                //args = new string[] { "1", "U011920", "AIPLDTM024\\SQLExpress", "sa", "sa1985", "^21001", "ADMIN", @"D:\UdyogERP\Bmp\Icon_VudyogGST.ico", "UdyogERPSDK", "UdyogERPSDK", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "" };
                args = new string[] { "11", "U041920", "BAIPLLTH103\\SQLEXPRESS2014", "sa", "sa@1985", "^21001", "ADMIN", @"E:\Divyang Panchal\Udyog\UdyogERPSDK\Bmp\Icon_VudyogGST.ico", "UdyogERPSDK", "UdyogERPSDK", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(args));
        }
    }
}
