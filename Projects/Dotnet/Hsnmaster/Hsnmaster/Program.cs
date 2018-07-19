using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hsnmaster
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            //if (args.Length < 8)
            if (args.Length <=0 )
            {
                //args = new string[] { "1", "U191718", @"AIPLDTM019\SQLExpress", "sa", "sa1985", "^21380", "ADMIN", @"E:\U3\UdyogGSTSDK\Bmp\ueicon.ICO", "UdyogGSTSDK.EXE", "UdyogGSTSDK.EXE", "2448", "udPID5852DTM20161110115010" };
                //  args = new string[] { "1", "T011617", @"AIPLDTM024\SQLExpress", "sa", "sa1985", "^21380", "ADMIN", @"C:\VudyogGST\Bmp\Icon_VudyogGST.ico", "VudyogGST", "VudyogGST.EXE", "5852", "udPID5852DTM20161110115010" };
                //args = new string[] { "119", "B041718", @"AIPLDTM001\SQLEXPRESS", "sa", "sa1985", "^21380", "ADMIN", @"D:\UdyogGST\Bmp\Icon_VudyogGST.ico", "UdyogGST", "UdyogGST.EXE", "5852", "udPID5852DTM20161110115010" };
                //args = new string[] { "137", "B091718", @"AIPLDTM001\SQLEXPRESS", "sa", "sa1985", "^21380", "ADMIN", @"D:\UdyogGST\Bmp\Icon_VudyogGST.ico", "UdyogGST", "UdyogGST.EXE", "5852", "udPID5852DTM20161110115010" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmHSNCodeMast(args));
            return 1;
        }
    }
}
