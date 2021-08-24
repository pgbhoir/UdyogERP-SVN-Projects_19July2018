using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAccess_Net;

namespace UdyogMaterialRequirementPlanning
{
    public partial class ueFrmAddlInfo : Form
    {
        public string cEntTy = string.Empty;
        public int iTranCd = 0;
        public string cItSerial = string.Empty;

        clsDataAccess oDataAccess;

        public ueFrmAddlInfo()
        {
            InitializeComponent();
        }

        private void ueFrmAddlInfo_Load(object sender, EventArgs e)
        {
            clsDataAccess._databaseName = clsCommon.DbName;
            clsDataAccess._serverName = clsCommon.ServerName;
            clsDataAccess._userID = clsCommon.User;
            clsDataAccess._password = clsCommon.Password;

            oDataAccess = new clsDataAccess();
            string cSql = "Select * from Lother Where e_code='"+cEntTy+"' order by case when att_file=1 then 1 else 2 end, Serial, SubSerial ";
            DataTable _dtLother = oDataAccess.GetDataTable(cSql, null, 30);

            string csvString = String.Join(",", _dtLother.AsEnumerable().Select(x => (x.Field<string>("tbl_nm").ToString().Trim().ToUpper().Contains("MAIN") || x.Field<string>("tbl_nm").ToString().Trim().ToUpper().Contains("LMC") ? "A." : "B.") + "[" + x.Field<string>("fld_nm").ToString().Trim() + "]").ToArray());

            cSql = "Select "+ csvString + " from somain a Inner join soitem b on a.Entry_ty=b.Entry_ty and a.Tran_cd=b.Tran_cd where a.Entry_ty='" + cEntTy + "' and a.Tran_cd="+iTranCd.ToString()+" and b.itserial='"+cItSerial+"' ";
            DataTable _dtLother1 = oDataAccess.GetDataTable(cSql, null, 30);


            dgvAddlInfo.DataSource = _dtLother1;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
