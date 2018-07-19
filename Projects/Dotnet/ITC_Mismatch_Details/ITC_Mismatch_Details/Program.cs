using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITC_Mismatch_Details
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length <= 0)
            {
                // args = new string[] { "1", "I011718", @"AIPLDTM024\SQLExpress", "sa", "sa1985", "^21380", "ADMIN", @"C:\VudyogGST\Bmp\Icon_VudyogGST.ico", "VudyogGST", "VudyogGST.EXE", "5852", "udPID5852DTM20161110115010" };
                args = new string[] { "1", "U061718", @"AIPLDTM024\SQLExpress", "sa", "sa1985", "^21380", "ADMIN", @"D:\UdyogGST\Bmp\Icon_VudyogGST.ico", "UdyogGST", "UdyogGST.EXE", "5852", "udPID5852DTM20161110115010","","","","","", "Haryana" };
                
                  //args = new string[] { "1", "D011617", @"AIPLDTM024\SQLExpress", "sa", "sa1985", "^21380", "ADMIN", @"C:\VudyogGST\Bmp\Icon_VudyogGST.ico", "VudyogGST", "VudyogGST.EXE", "5852", "udPID5852DTM20161110115010" };
            }
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
             Application.Run(new frmSelectFile(args));
             // Application.Run(new frmMismatchDetails());
            return 1;
        }
    }
}
