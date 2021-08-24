using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UdMISReports
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length <=0 )
            {
                args = new string[] { "MSALER|[FParty=][TParty=work order]||[FDate=04/01/2018][TDate=04/01/2018]", "10", "A011819", @"AIPLDTM019\SQLEXPRESS", "sa", "sa1985", "^19031", "ADMIN", @"E:\U6\UdyogERP\Bmp\Icon_VudyogGST.ico", "UdyogERPENT", "UdyogERPENT.EXE", "4764", "udPID4764DTM20111213125821", "", "", "E:\\u3\\UdyogERPSDK\\", "01-04-2018", "31-03-2019" };
                //args = new string[] { "9", "U051819", @"AIPLDTM019\SQLEXPRESS", "sa", "sa1985", "^19031", "ADMIN", @"E:\U3\UdyogERPSDK\Bmp\Icon_VudyogGST.ico", "UdyogERPSDK", "UdyogERPSDK.EXE", "4764", "udPID4764DTM20111213125821", "", "01-04-2018", "31-03-2019" };
            }
            if (args.Length != 0)
            {
                Application.Run(new rbnMainform(args));
            }
            else
            {

                MessageBox.Show("Please Supply the argument with this exe!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Application.Exit();
            }
        }
    }
}
