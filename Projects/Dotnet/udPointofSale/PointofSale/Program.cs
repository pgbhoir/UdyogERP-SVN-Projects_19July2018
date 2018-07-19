using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PointofSale
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 16)
            {
                //args = new string[] { "2", "B011617", @"AIPLLTM001\AIPLLTM001", "sa", "sa1985", "^21289", "ADMIN", @"D:\Vudyogsdk\Bmp\icon_VUDYOGSDK.ico", "VudyogSDK", "VUDYOGSDK.EXE", "4764", "udPID4764DTM20111213125821", "udPOS", "01-04-2016", "31-03-2017", ".T." };
                args = new string[] { "1", "P011819", @"AIPLDTM002\SQLEXPRESS2008", "sa", "sa1985", "^21289", "ADMIN", @"E:\UdyogERP\Bmp\Icon_VudyogGST.ico", "VudyogGST", "VUDYOGGST.EXE", "4764", "udPID4764DTM20111213125821", "udPOS", "01-04-2018", "31-03-2019", ".T." };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new udFrmPOSLoginSettings(args));
        }
    }
}
