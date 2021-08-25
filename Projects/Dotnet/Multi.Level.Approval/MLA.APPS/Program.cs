using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;


namespace MLA.APPS
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
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-IN");

            //args = new string[16];
            //args[13] = "T";
            //args[1] = "U161819";
            //args[0] = "18";
            //args[14] = "01/04/2018";
            //args[15] = "31/03/2019";
            //args[6] = "B";
            //args[7] = @"E:\U3\Vudyogsdk\Bmp\icon_VudyogGSSDK.ico";

            try
            {
                if (args.Length > 0)
                {
                    string module = string.Empty;
                    string dbname = string.Empty;
                    string userid = string.Empty;
                    string iconpath = string.Empty;
                    int compid = 0;
                    string defapath = string.Empty;
                    string apppath = string.Empty;
                    string prodcode = string.Empty;

                    DateTime startDt = DateTime.Now;
                    DateTime endDt = DateTime.Now;


                    //for (int i = 0; i < args.Count(); i++)
                    //{
                    //    MessageBox.Show("Argument No. " + i.ToString().Trim() + " Value : " + args[i].Replace("<*#*>", " "));
                    //}


                    userid = args[6].Replace("<*#*>", " ");
                    iconpath = args[7].Replace("<*#*>", " ");

                    dbname = Convert.ToString(args[1]);
                    module = Convert.ToString(args[13]);
                    compid = Convert.ToInt32(args[0]);
                    prodcode = Convert.ToString(args[12]);

                    if (args.Count() > 14)
                    {
                        startDt = Convert.ToDateTime(args[14].Replace("<*#*>", " "));
                        endDt = Convert.ToDateTime(args[15].Replace("<*#*>", " "));

                        if (args.Count() > 16)
                        {
                            defapath = Convert.ToString(args[16]);
                            apppath = Convert.ToString(args[17]);
                        }
                    }
                    if (module.Trim().ToUpper() == "M")
                    {
                        MLAAppLevelSettings.ConnectionString = EvaluateConnectionString(dbname);
                        MLAAppLevelSettings m_obj_settings = new MLAAppLevelSettings();
                        m_obj_settings.UserID = userid;
                        m_obj_settings.IconPath = iconpath;
                        m_obj_settings.CompId = compid;

                        Application.Run(m_obj_settings);
                    }
                    else if (module.Trim().ToUpper() == "T")
                    {
                        MLAApproval.Connectionstring = EvaluateConnectionString(dbname);
                        MLAApproval m_obj_approval = new MLAApproval();
                        m_obj_approval.CompId = compid;
                        m_obj_approval.UserID = userid;
                        m_obj_approval.Startdt = startDt;
                        m_obj_approval.Enddt = endDt;
                        m_obj_approval.IconPath = iconpath;
                        m_obj_approval.DbName = dbname;
                        m_obj_approval.DefaPath = defapath;
                        m_obj_approval.AppPath = apppath;
                        m_obj_approval.ProdCode = prodcode;
                        Application.Run(m_obj_approval);
                    }
                    else if (module.Trim().ToUpper() == "S")
                    {
                        MLAEmailSettings m_obj_emailsettings = new MLAEmailSettings(EvaluateConnectionString(dbname));
                        m_obj_emailsettings.IconPath = iconpath;
                        Application.Run(m_obj_emailsettings);
                    }

                }
            }
            catch (Exception Ex)
            {
                string error = "\n\n Parameters may not correct..  \n\n" +
                          "Message : " + Ex.Message +
                          "\nSource : " + Ex.Source +
                          "\nTargetSite : " + Ex.TargetSite;

                MessageBox.Show(error, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private static string EvaluateConnectionString(string database)
        {
            string apptitle = string.Empty;
            string appfile = string.Empty;
            string server = string.Empty;
            string user = string.Empty;
            string pass = string.Empty;
            string connectionstring = string.Empty;
            string _nenc = string.Empty;        //Added by Shrikant S. on 28/07/2018 for Bug-30904


            //GetInfo.iniFile ini = new GetInfo.iniFile(@"E:\U3\Udyogerpsdk\\" + "\\" + "Visudyog.ini");

            GetInfo.iniFile ini = new GetInfo.iniFile(Application.StartupPath + "\\" + "Visudyog.ini");


            GetInfo.EncDec eObject = new GetInfo.EncDec();
            apptitle = ini.IniReadValue("Settings", "Title");
            appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
            server = ini.IniReadValue("DataServer", "Name");
            user = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "User")));
            pass = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "Pass")));
            
            //Added by Shrikant S. on 28/07/2018 for Bug-30904          // Start
            _nenc = ini.IniReadValue("Settings", "NENC");
            if (_nenc.Trim() == "1")
            {
                user = Udyog.Library.Common.VU_UDFS.NewDECRY(Udyog.Library.Common.VU_UDFS.HexDecimalToASCII(user), "Ud*yog+1993Uid");
                pass = Udyog.Library.Common.VU_UDFS.NewDECRY(Udyog.Library.Common.VU_UDFS.HexDecimalToASCII(pass), "Ud*yog+1993Pwd");
            }
            else
            {
                user = eObject.Dec("myName", eObject.OnDecrypt("myName", user));
                pass = eObject.Dec("myName", eObject.OnDecrypt("myName", pass));
            }
            //Added by Shrikant S. on 28/07/2018 for Bug-30904          // End

            //Commented by Shrikant S. on 28/07/2018 for Bug-30904          // Start
            //user = eObject.Dec("myName", eObject.OnDecrypt("myName", user));
            //pass = eObject.Dec("myName", eObject.OnDecrypt("myName", pass));
            //Commented by Shrikant S. on 28/07/2018 for Bug-30904          // End

            connectionstring = "Data Source=" + server.Trim() +
                               ";Initial Catalog=" + database.Trim() +
                               ";UID=" + user.Trim() +
                               ";Pwd=" + pass.Trim() +
                               ";Min Pool Size=5;Max Pool Size=60;Connect Timeout=10";

            return connectionstring;
        }
    }
}
