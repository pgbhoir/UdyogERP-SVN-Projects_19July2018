using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace ugstEWB_AcMast_JSON
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
                //args = new string[] { "3", "U031718", "AIPLLTM035\\SQLEXPRESS2012", "sa", "sa@1985r2", "^21001", "ADMIN", @"D:\UdyogGSTSDK\Bmp\Icon_VudyogGST.ico", "uGST SDK", "UdyogGSTSDK", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "eWAY BIll Generation" };// "Employee Master Updation" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
                //args = new string[] { "9", "U111718", "AIPLDTM001\\SQLEXPRESS", "sa", "sa1985", "^21001", "ADMIN", @"D:\UdyogGSTSDK\Bmp\Icon_VudyogGST.ico", "uGST sdk", "UdyogGSTSDK", "1", "udpid6096DTM20110307112715", "ADMINISTRATOR", "eWAY BIll Generation" };// "Employee Master Updation" };/*Rup Add ICO Path,Parent Appl Caption,Parent Appl. Name,Parent Appl PId,Application Log Table*/
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(args));
            return 1;
        }
    }
}
