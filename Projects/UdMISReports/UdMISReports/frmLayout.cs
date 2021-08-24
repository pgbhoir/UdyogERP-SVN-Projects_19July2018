using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;

namespace UdMISReports
{
    public partial class frmLayout : DevExpress.XtraEditors.XtraForm
    {
        private string _connString = string.Empty;
        private string _username = string.Empty;
        private string _reportId = string.Empty;

        private string _layoutNm = string.Empty;
        public string LayoutName
        {
            get { return _layoutNm; }
            set { _layoutNm = value; }
        }

        private int _layoutCount = 0;
        public int LayoutCount
        {
            get { return _layoutCount; }
            set { _layoutCount = value; }
        }

        private bool _AddMode = false; 
        public bool AddMode
        {
            get { return _AddMode; }
            set { _AddMode = value; }
        }
        private bool _EditMode = false;
        public bool EditMode
        {
            get { return _EditMode; }
            set { _EditMode = value; }
        }
        
        private bool _IsDefault = false;
        public bool IsDefault
        {
            get
            {
                return _IsDefault;
            }

            set
            {
                _IsDefault = value;
            }
        }

        private bool _AllLayout = false;

        private DataTable _layoutDetails=new DataTable();

        #region Private Methods 
        private bool CheckLayoutExists()
        {
            bool retValue = false;
            try
            {
                SqlConnection oconn = new SqlConnection(_connString);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = oconn;
                cmd.CommandText = "Select Top 1 layoutNm From UBIRepLayout Where ReportId=@ReporId and Username=@Username and LayoutNm=@LayoutNm";
                cmd.CommandText = cmd.CommandText + " and LayoutNm=@LayoutNm";

                cmd.Parameters.Add(new SqlParameter("@ReporId", _reportId));
                cmd.Parameters.Add(new SqlParameter("@Username", _username));
                cmd.Parameters.Add(new SqlParameter("@LayoutNm", _layoutNm));
                retValue = (Convert.ToString(cmd.ExecuteScalar()) != string.Empty ? true:false) ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return retValue;            
        } 
        private void GetLayoutDetails()
        {
            SqlConnection oconn = new SqlConnection(_connString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = oconn;
            cmd.CommandText = "Select layoutNm as [Layout Name],IsDefault From UBIRepLayout Where ReportId=@ReporId and Username=@Username";
            cmd.Parameters.Add(new SqlParameter("@ReporId", _reportId));
            cmd.Parameters.Add(new SqlParameter("@Username", _username));
            if (!_AllLayout)
            {
                cmd.CommandText = cmd.CommandText + " and LayoutNm=@LayoutNm";
                cmd.Parameters.Add(new SqlParameter("@LayoutNm", _layoutNm));
            }
            SqlDataAdapter lda = new SqlDataAdapter(cmd);
            lda.Fill(_layoutDetails);
            _layoutCount = _layoutDetails.Rows.Count;
        }
        
        #endregion

        public frmLayout(string connString,string username,string reportId)
        {
            InitializeComponent();
            _connString = connString;
            _username = username;
            _reportId = reportId;
            _AllLayout = true;
            this.GetLayoutDetails();
            this.gridControl1.DataSource = _layoutDetails;
            if (!this.AddMode && !this.EditMode)
            {
                if (_layoutDetails.Rows.Count <= 0)
                {
                    MessageBox.Show("Layout not defined for this report.");
                    this.Close();
                }
            }
        }
        public frmLayout(string connString, string username, string reportId,bool addMode,bool editMode,string layoutName)
        {
            InitializeComponent();
            _connString = connString;
            _username = username;
            _reportId = reportId;
            _AddMode = addMode;
            _EditMode = editMode;
            _layoutNm = layoutName;
            this.GetLayoutDetails();
            this.gridControl1.DataSource = _layoutDetails;
            if (!this.AddMode && !this.EditMode)
            {
                if (_layoutDetails.Rows.Count <= 0)
                {
                    MessageBox.Show("Layout not defined for this report.");
                    this.Close();
                }
            }
        }
        private void frmLayout_Load(object sender, EventArgs e)
        {
            if (!this.AddMode && !this.EditMode)
            {
                this.gridView1.OptionsFind.AllowFindPanel = true;
                this.gridView1.OptionsFind.AlwaysVisible = true;
                this.gridView1.OptionsFind.ShowClearButton = false;
                this.gridView1.OptionsFind.ShowFindButton = true;
                this.gridView1.Columns[0].OptionsColumn.AllowEdit = false;
                this.gridView1.Columns[1].OptionsColumn.AllowEdit = false;
            }
            else
            {
                this.gridView1.OptionsFind.AllowFindPanel = false;
                this.gridView1.OptionsFind.AlwaysVisible = false;
                this.gridView1.OptionsFind.ShowClearButton = false;
                this.gridView1.OptionsFind.ShowFindButton = false;
                this.sbSave.Visible = true;
                this.gridView1.Columns[0].OptionsColumn.AllowEdit = true;
                this.gridView1.Columns[1].OptionsColumn.AllowEdit = true;
                if (this.AddMode)
                {
                    DataRow row = _layoutDetails.NewRow();
                    row["IsDefault"] = false;
                    _layoutDetails.Rows.Add(row);
                }
                if (this.EditMode)
                {
                    this.gridView1.Columns[0].OptionsColumn.AllowEdit = false;
                    this.gridView1.Columns[1].OptionsColumn.AllowEdit = true;
                }
            }
            
        }

        private void sbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRow && info.InRowCell)
            {
                DataRow ldr = view.GetDataRow(info.RowHandle);
                _layoutNm= ldr[ldr.Table.Columns["Layout Name"].Ordinal].ToString().Trim();
                _IsDefault =Convert.ToBoolean(ldr[ldr.Table.Columns["IsDefault"].Ordinal]);
                this.Close();
            }
        }

        private void sbSave_Click(object sender, EventArgs e)
        {
            _layoutNm = _layoutDetails.Rows[0]["Layout Name"].ToString().Trim();
            _IsDefault = Convert.ToBoolean(_layoutDetails.Rows[0]["IsDefault"]);
            this.Close();
        }

    }
}