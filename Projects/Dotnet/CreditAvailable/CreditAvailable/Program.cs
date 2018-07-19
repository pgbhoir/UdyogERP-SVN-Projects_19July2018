using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CreditAvailable
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
           
            if (args.Length <= 0)
            {
       //  args = new string[] { "1", "T021819", @"AIPLDTM024\SQLExpress", "sa", "sa1985", "^21380", "ADMIN", @"D:\UdyogERP\Bmp\Icon_VudyogGST.ico", "VudyogGST", "VudyogGST.EXE", "5852", "udPID5852DTM20161110115010" };
                //  args = new string[] { "1", "T011617", @"AIPLDTM024\SQLExpress", "sa", "sa1985", "^21380", "ADMIN", @"C:\VudyogGST\Bmp\Icon_VudyogGST.ico", "VudyogGST", "VudyogGST.EXE", "5852", "udPID5852DTM20161110115010" };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(args));
        }
    }
}
