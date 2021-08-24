using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UdyogMaterialRequirementPlanning
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           
            if (args.Length == 0)
            {
                MessageBox.Show("main 2");
                args = new string[16];
                //args[0] = "152";
                //args[1] = "B061718";
                //args[2] = "AIPLLTM001\\AIPLLTM001";
                //args[3] = "sa";
                //args[4] = "sa1985";
                //args[6] = "Admin";
                //args[7] = @"D:\Vudyoggst\Bmp\Icon_VudyogGST.ico";
                //args[8] = @"";
                //args[9] = @"";
                //args[10] = @"";
                //args[11] = @"";
                //args[13] = "SO:DATE";
                //args[14] = "01/04/2017";
                //args[15] = "31/03/2018";

                args[0] = "11"; // "179";
                args[1] = "U041920";   //"B051819";
                args[2] = "BAIPLLTH103\\SQLEXPRESS2014";
                args[3] = "sa";
                args[4] = "sa@1985";
                args[6] = "Admin";
                args[7] = @"E:\Divyang Panchal\Udyog\UdyogERPSDK\Bmp\Icon_VudyogGST.ico";
                args[8] = @"";
                args[9] = @"UdyogERPSDK.exe";
                args[10] = @"";
                args[11] = @"";
                args[13] = "SO:DATE";
                args[14] = "01/04/2019";
                args[15] = "31/03/2020";
            }
            Application.Run(new frmMRP(args));
        }
    }
}
