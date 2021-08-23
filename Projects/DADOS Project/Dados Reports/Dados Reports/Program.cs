using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DadosReports;

namespace DadosReports
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
            //Testing Archana 
            if (args.Length < 15)
            {
                args = new string[] { "000506|[UserName=Admin][Domain=Adaequare]|[FParty=][TParty=work order][FItem=][TItem=WIRE ROPE]||[FrmDate=04/01/2018][Todate=03/31/2019]|Data Source=AIPLDTM019\\SQLEXPRESS;Persist Security Info=True;Password=sa1985;User ID=sa;Initial Catalog=U051819", "9", "U051819", @"AIPLDTM019\SQLEXPRESS", "sa", "sa1985", "^19031", "ADMIN", @"E:\U3\UdyogERPSDK\Bmp\Icon_VudyogGST.ico", "UdyogERPSDK", "UdyogERPSDK.EXE", "4764", "udPID4764DTM20111213125821", "", "", "", "01-04-2018", "31-03-2019" };
                //args = new string[] { "9", "U051819", @"AIPLDTM019\SQLEXPRESS", "sa", "sa1985", "^19031", "ADMIN", @"E:\U3\UdyogERPSDK\Bmp\Icon_VudyogGST.ico", "UdyogERPSDK", "UdyogERPSDK.EXE", "4764", "udPID4764DTM20111213125821", "", "01-04-2018", "31-03-2019" };
            }
            if (args.Length != 0)
            {
                Loading.ShowSplashScreen();
                Application.Run(new ReportsMain(args));
            }
            else
            {

                MessageBox.Show("Please Supply the argument with this exe!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Application.Exit();
            }
            
        }
    }
}