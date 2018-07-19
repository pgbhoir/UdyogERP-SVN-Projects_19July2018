using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CustModAccUI.UI
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

            //args = new string[6];
            ////args[0] = "A031314";//dbname //Avery2008R2
            //args[0] = "A051314";//dbname //Vudyogsdk
            //args[1] = "ADMIN"; //user login           
            ////args[2] = @"E:\iTAX\";//Apath //Avery2008R2
            //args[2] = @"D:\Vudyogsdk\";//Apath //Vudyogsdk
            //args[3] = "13008";//"21158"; //range
            //args[4] = @"E:\iTAX\Bmp\ueicon.ico";//icon path
            //args[5] = "iTAX";// "Visual Udyog SDK";//vumess

            string connectionstring = string.Empty, vfpconnectionstring = string.Empty, DbName = string.Empty, username = string.Empty;
            int range = 0;
            string APath = string.Empty, IcoPath = string.Empty, Vumess = string.Empty;

            try
            {
                if (args.Length > 0)
                {
                    //MessageBox.Show(args[0].ToString().Trim(), "dbname");
                    //MessageBox.Show(args[1].ToString().Trim(), "username");
                    //MessageBox.Show(args[2].ToString().Trim(), "path");
                    //MessageBox.Show(args[3].ToString().Trim(), "range");

                    if (string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[1]) || string.IsNullOrEmpty(args[2]) || string.IsNullOrEmpty(args[3]) || string.IsNullOrEmpty(args[4]) || string.IsNullOrEmpty(args[5]))
                    {
                        MessageBox.Show("Parameters cannot be empty or null..!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    DbName = args[0].ToString().Trim();
                    username = args[1].ToString().Trim().ToUpper();
                    APath = args[2].ToString().Trim();
                    range = Convert.ToInt32(args[3].ToString().Trim());
                    IcoPath = args[4].ToString().Trim();
                    Vumess = args[5].ToString().Trim();
                    
                    try
                    {
                        connectionstring = EvaluateConnectionString(DbName, APath);
                        vfpconnectionstring = EvaluateVFPConnectionString(APath);
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show("found error while evaluating connection string from INI  \n" +
                                             "Message : " + Ex.Message +
                                             "\nSource : " + Ex.Source +
                                             "\nTargetSite : " + Ex.TargetSite, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (connectionstring == string.Empty)
                    {
                        MessageBox.Show("Connection string found empty...!!!, please check INI File", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (vfpconnectionstring == string.Empty)
                    {
                        MessageBox.Show("As Feature.dbc database not found in the " + System.Environment.CurrentDirectory + "\\UpdtExe path.\n" + "Connection string found empty...!!!, please check the database.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    try
                    {
                        Application.Run(new frmUI(connectionstring, vfpconnectionstring, username, range, APath, IcoPath, Vumess));
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show("Message : " + Ex.Message +
                                             "\nSource : " + Ex.Source +
                                             "\nTargetSite : " + Ex.TargetSite, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    if (connectionstring == string.Empty)
                    {
                        MessageBox.Show("Connection string found empty...!!!, please check INI File", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (vfpconnectionstring == string.Empty)
                    {
                        MessageBox.Show("As Feature.dbc database not found in the " + System.Environment.CurrentDirectory + "\\UpdtExe path.\n" + "Connection string found empty...!!!, please check the database.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    try
                    {
                        Application.Run(new frmUI(connectionstring, vfpconnectionstring, username, range, APath, IcoPath, Vumess));
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show("Message : " + Ex.Message +
                                             "\nSource : " + Ex.Source +
                                             "\nTargetSite : " + Ex.TargetSite, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private static string EvaluateConnectionString(string database,string APath)
        {
            string apptitle = string.Empty;
            string appfile = string.Empty;
            string server = string.Empty;
            string user = string.Empty;
            string pass = string.Empty;
            string connectionstring = string.Empty;
                        
            //GetInfo.iniFile ini = new GetInfo.iniFile(APath + "\\" + "Visudyog.ini");
            GetInfo.iniFile ini = new GetInfo.iniFile(APath + "Visudyog.ini");


            GetInfo.EncDec eObject = new GetInfo.EncDec();
            apptitle = ini.IniReadValue("Settings", "Title");
            appfile = ini.IniReadValue("Settings", "xfile").Substring(0, ini.IniReadValue("Settings", "xfile").Length - 4);
            server = ini.IniReadValue("DataServer", "Name");
            user = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "User")));
            pass = ini.IniReadValue("DataServer", eObject.OnEncrypt("myName", eObject.Enc("myName", "Pass")));

            user = eObject.Dec("myName", eObject.OnDecrypt("myName", user));
            pass = eObject.Dec("myName", eObject.OnDecrypt("myName", pass));

            connectionstring = "Data Source=" + server.Trim() +
                               ";Initial Catalog=" + database.Trim() +
                               ";UID=" + user.Trim() +
                               ";Pwd=" + pass.Trim() +
                               ";Min Pool Size=5;Max Pool Size=60;Connect Timeout=10";

            return connectionstring;
        }

        private static string EvaluateVFPConnectionString(string APath)
        {
            string vfpconnectionstring = string.Empty;

            vfpconnectionstring = "Provider=vfpoledb;Data Source=" +
                 APath + "CustModAccUI\\UpdtExe\\feature.dbc;Collating Sequence=machine;" +
                ";providerName=System.Data.OleDb.OleDbConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            return vfpconnectionstring;
        }

        //public class CallMessageBox
        //{
        //    static public void Show(string ExcepMessage, string Mess)
        //    {
        //        const char dbquote = '"';
        //        MessageBox.Show(Mess + ExcepMessage
        //                + "\n\n"
        //                + "Parameters Examples are below :-"
        //                + "\n\n"
        //                + "For Customised Modules Access Interface : " + "\n"
        //                + "CustModAccUI2.UI.exe " + dbquote + "<DBNAME>" + dbquote + " " + dbquote + "<USERLOGINNAME>" + " " + dbquote + "<APPLICATIONPATH>" 
        //                + " " + dbquote + "<RANGE>" + " " + dbquote + "<ICONPATH>" + " " + dbquote + "<VUMESS>" + dbquote,                       
        //                "ITAX", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}
    }
}

