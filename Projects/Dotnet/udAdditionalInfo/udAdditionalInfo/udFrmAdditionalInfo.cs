using DataAccess_Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace udAdditionalInfo
{

    public partial class udFrmAdditionalInfo : Form
    {
        DataTable _dtLother;
        public string _HdrDtl;
        public string _TblName;
        public DataSet _commonDs;
        public bool AddMode = false, EditMode = false;

        clsDataAccess _oDataAccess;
        private DataView currentView;
        private DataTable _sourceTable;
        private string _filterid;
        public udFrmAdditionalInfo()
        {
            InitializeComponent();

        }
        public udFrmAdditionalInfo(DataTable _dtLother, string _HdrDtl, string _TblName, DataSet _commonDs,string FilterRowId)
        {
            InitializeComponent();
            this._dtLother = _dtLother;
            this._HdrDtl = _HdrDtl;
            this._TblName = _TblName;
            this._commonDs = _commonDs;
            _filterid = FilterRowId;
            //currentView = _commonDs.Tables[_TblName].DefaultView;
            //if (_HdrDtl=="D")
            //    currentView.RowFilter="ItemRowId="+ FilterRowId;
            //else
            //    currentView.RowFilter = "";

            _sourceTable = _commonDs.Tables[_TblName].Clone();


            DataRow[] sourcerows;
            if (_HdrDtl=="D")
                sourcerows = _commonDs.Tables[_TblName].Select("ItemRowId=" + FilterRowId);
            else
                sourcerows = _commonDs.Tables[_TblName].Select();

            for (int i = 0; i < sourcerows.Count(); i++)
            {
                _sourceTable.Rows.Add(sourcerows[i].ItemArray);
            }
        }

        public udFrmAdditionalInfo(DataTable _dtLother, string _HdrDtl, string _TblName, DataSet _commonDs)
        {
            InitializeComponent();
            this._dtLother = _dtLother;
            this._HdrDtl = _HdrDtl;
            this._TblName = _TblName;
            this._commonDs = _commonDs;
        }

        private void udFrmAdditionalInfo_Load(object sender, EventArgs e)
        {
            int _lblPosLeft = 9, _lblPosRight = 338, _lblPosTop = 15, _cntrlPosLeft = 80, _cntrlPosRight = 409, _cntrlPosTop = 12;
            int _lblMaxWidth = 67, _lblMaxHght = 15, _cntrlMaxWidth = 215, _cntrlMaxHght = 20;
            bool _lChngLine = false, _lLeftSide = true, _lLblHght = false;
            Label _lblCntrl;
            TextBox _txtCntrl;
            _oDataAccess = new clsDataAccess();     //Added by Shrikant S. on 06/10/2018 

            uNumericTextBox.cNumericTextBox _cNumericTextBox;

            foreach (DataRow _dr in _dtLother.Rows)
            {
                if (_dr["Inter_Use"].ToString().ToUpper() == ".T." || Convert.ToBoolean(_dr["ingrid"]) == true)
                {
                    continue;
                }
                
                _lblCntrl = new Label();
                _lblCntrl.Location = new System.Drawing.Point(_lLeftSide == true ? _lblPosLeft : _lblPosRight, _lblPosTop);
                _lblCntrl.Name = "lbl" + _dr["Fld_Nm"].ToString().Trim();
                _lblCntrl.Text = _dr["Head_Nm"].ToString().Trim();
                _lblCntrl.Width = _lblMaxWidth;
                _lblCntrl.Height = _lblMaxHght;
                if (_lblCntrl.Text.Length > 11)
                {
                    _lblCntrl.Height *= 2;
                    _cntrlPosTop = _lLeftSide == true ? _cntrlPosTop + 7 : _cntrlPosTop;
                    _lLblHght = true;
                }
                _lblCntrl.BackColor = System.Drawing.Color.Transparent;
                this.pnlUpper.Controls.Add(_lblCntrl);

                switch (_dr["Data_Ty"].ToString().Trim())
                {
                    case "Decimal":
                        _cNumericTextBox = new uNumericTextBox.cNumericTextBox();
                        _cNumericTextBox.Location = new System.Drawing.Point(_lLeftSide == true ? _cntrlPosLeft : _cntrlPosRight, _cntrlPosTop);
                        _cNumericTextBox.Name = "txt" + _dr["Fld_Nm"].ToString().Trim();
                        _cNumericTextBox.Width = _cntrlMaxWidth;
                        //_cNumericTextBox.DataBindings.Add("Text", _commonDs, _TblName + "." + _dr["Fld_Nm"].ToString().Trim());       //Commented by Shrikant S. on 09/10/2018 
                        _cNumericTextBox.DataBindings.Add("Text", _sourceTable,  _dr["Fld_Nm"].ToString().Trim());      //Added by Shrikant S. on 09/10/2018 
                        _cNumericTextBox.pDecimalLength = Convert.ToInt16(_dr["Fld_Dec"]);
                        this.pnlUpper.Controls.Add(_cNumericTextBox);
                        
                        break;
                    //Added by Shrikant S. on 05/10/2018    //Start
                    case "Datetime":
                    case "datetime":
                    case "dateTime":
                        DateTimePicker dpk = new DateTimePicker();
                        dpk.Name = "dpk" + _dr["Fld_Nm"].ToString().Trim(); ;
                        dpk.Width = 100;
                        dpk.CustomFormat = " ";
                        dpk.Format = DateTimePickerFormat.Custom;
                        dpk.ValueChanged += new System.EventHandler(this.dpk_ValueChanged);
                        dpk.Location = new System.Drawing.Point(_lLeftSide == true ? _cntrlPosLeft : _cntrlPosRight, _cntrlPosTop);
                        dpk.Checked = false;
                        //Binding b = new Binding("Value", _commonDs, _TblName + "." + _dr["Fld_Nm"].ToString().Trim(), true);          //Commented by Shrikant S. on 09/10/2018 
                        Binding b = new Binding("Value", _sourceTable, _dr["Fld_Nm"].ToString().Trim(), true);         //Added by Shrikant S. on 09/10/2018 
                        b.Format += new ConvertEventHandler(dpk_Format);
                        b.Parse += new ConvertEventHandler(dpk_Parse);
                        dpk.DataBindings.Add(b);

                        this.pnlUpper.Controls.Add(dpk);
                        break;
                    //Added by Shrikant S. on 05/10/2018    //End
                    default:
                        _txtCntrl = new TextBox();
                        _txtCntrl.Location = new System.Drawing.Point(_lLeftSide == true ? _cntrlPosLeft : _cntrlPosRight, _cntrlPosTop);
                        _txtCntrl.Name = "txt" + _dr["Fld_Nm"].ToString().Trim();
                        _txtCntrl.Width = _cntrlMaxWidth;
                        //_txtCntrl.DataBindings.Add("Text", _commonDs, _TblName + "." + _dr["Fld_Nm"].ToString().Trim());      //Commented by Shrikant S. on 09/10/2018 
                        _txtCntrl.DataBindings.Add("Text", _sourceTable, _dr["Fld_Nm"].ToString().Trim());        //Added by Shrikant S. on 09/10/2018 
                        this.pnlUpper.Controls.Add(_txtCntrl);

                        string filtcond = Convert.ToString(_dr["filtcond"]).Trim();
                        if (filtcond.Length > 0)
                        {
                            Button btn = new Button();
                            btn.Name = "btn" + _dr["Fld_Nm"].ToString().Trim();
                            btn.Text = "...";
                            btn.Left = _txtCntrl.Left + _txtCntrl.Width + 2;
                            //btn.Top = top + 2;
                            btn.Tag = filtcond;
                            btn.Width = 30;
                            btn.Location = new System.Drawing.Point(_lLeftSide == true ? _cntrlPosLeft + _cntrlMaxWidth + 5 : _cntrlPosRight + _cntrlMaxWidth + 5, _cntrlPosTop);
                            this.pnlUpper.Controls.Add(btn);
                            btn.Click += new EventHandler(popupbtn_clicked);
                        }

                        break;
                }

                if (_lLeftSide == true)
                {
                    _lLeftSide = false;
                    _lChngLine = true;
                }
                else
                {
                    if (_lChngLine == true)
                    {
                        _lLeftSide = true;
                        _lChngLine = false;
                        _lblPosTop += 25;
                        _cntrlPosTop += 23;
                        if (_lLblHght == true)
                        {
                            _lLblHght = false;
                            _lblPosTop += 5;
                        }
                    }
                }
            }
            this.pnlUpper.Height = _cntrlPosTop + _cntrlMaxHght + 10;
            this.pnlLower.Top = this.pnlUpper.Top + this.pnlUpper.Height + 2;
            this.Height = this.pnlLower.Top + this.pnlLower.Height + 40;        //30    01/10/2018 Shrikant S.
            this.Refresh();
        }
        //Added by Shrikant S. on 05/10/2018        //Start
        public void dpk_ValueChanged(object sender, EventArgs e)
        {
            if (((DateTimePicker)sender).ShowCheckBox == true)
            {
                if (((DateTimePicker)sender).Checked == false)
                {
                    ((DateTimePicker)sender).CustomFormat = " ";
                    ((DateTimePicker)sender).Format = DateTimePickerFormat.Custom;
                    ((DateTimePicker)sender).Checked = false;
                    ((DateTimePicker)sender).Refresh();
                }
                else
                {
                    ((DateTimePicker)sender).CustomFormat = "dd/MM/yyyy";
                    ((DateTimePicker)sender).Format = DateTimePickerFormat.Custom;
                    ((DateTimePicker)sender).Checked = true;
                }
            }
            else
            {
                ((DateTimePicker)sender).CustomFormat = "dd/MM/yyyy";
                ((DateTimePicker)sender).Format = DateTimePickerFormat.Custom;
                ((DateTimePicker)sender).Checked = true;
            }
        }
        void dpk_Format(object sender, ConvertEventArgs e)
        {
            // e.Value is the object value, we format it to be what we want to show up in the control 
            Binding b = sender as Binding;
            if (b != null)
            {
                DateTimePicker dtp = (b.Control as DateTimePicker);
                if (dtp != null)
                {
                    if (e.Value == null || e.Value == System.DBNull.Value || Convert.ToDateTime(e.Value).Year <= 1900)
                    {
                        dtp.ShowCheckBox = true;
                        dtp.Checked = false;
                        e.Value = dtp.Value;
                    }
                    else
                    {
                        dtp.ShowCheckBox = true;
                        if (Convert.ToDateTime(e.Value).Year <= 1900)
                        {
                            dtp.Checked = false;
                            dtp.Format = DateTimePickerFormat.Custom;

                        }
                        else
                            dtp.Checked = true;
                    }
                }

            }
        }

        void dpk_Parse(object sender, ConvertEventArgs e)
        {
            // e.value is the formatted value coming from the control.   
            // we change it to be the value we want to stuff in the object. 
            Binding b = sender as Binding;
            if (b != null)
            {
                DateTimePicker dtp = (b.Control as DateTimePicker);
                if (dtp != null)
                {
                    if (dtp.Checked == false)
                    {
                        dtp.ShowCheckBox = true;
                        dtp.Checked = false;
                        e.Value = System.DBNull.Value;
                    }
                    else
                    {
                        DateTime val = Convert.ToDateTime(e.Value);
                        e.Value = val;
                    }
                }
            }
        }
        private void popupbtn_clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string filtcond = btn.Tag.ToString().Trim();
            if (filtcond.ToString().Length > 0)
            {
                DataTable searchTbl = new DataTable();

                udSelectPop.SELECTPOPUP _oSelPop = new udSelectPop.SELECTPOPUP();
                string cFrmCap = "Select ", cSrchCol = "", cDispCol = "", cRetCol = "";
                string fldnm = btn.Name.Substring(3);
                Control[] ctllbl = this.pnlUpper.Controls.Find("lbl" + fldnm, false);
                Label lbl = (Label)ctllbl[0];

                if (filtcond.ToLower().Contains("execute ") || filtcond.ToLower().Contains("select ") || filtcond.ToLower().Contains("exec "))
                {
                    cFrmCap = cFrmCap + lbl.Text;
                    string csql = (filtcond.Trim().IndexOf("{") > 0 ? filtcond.Trim().Substring(0, filtcond.Trim().IndexOf("{") - 1) : filtcond.Trim());
                    searchTbl = _oDataAccess.GetDataTable(csql, null, 25);

                    cSrchCol = searchTbl.Columns[0].ColumnName;
                    cRetCol = searchTbl.Columns[0].ColumnName;
                    cDispCol = searchTbl.Columns[0].ColumnName;

                    if (filtcond.Trim().IndexOf("{") > 0)
                    {
                        filtcond = filtcond.Substring(filtcond.Trim().IndexOf("{") + 1, (filtcond.Trim().Length - filtcond.Trim().IndexOf("{") - 2));
                        string[] vals = filtcond.Trim().Split('#');
                        switch (vals.Length)
                        {
                            case 1:
                                cSrchCol = vals[0];
                                break;
                            case 2:
                                cSrchCol = vals[0];
                                cRetCol = vals[1];
                                break;
                            case 4:
                                cSrchCol = vals[0];
                                cRetCol = vals[1];
                                cDispCol = vals[3];
                                break;
                            default:
                                break;
                        }
                    }
                    _oSelPop.pformtext = cFrmCap;
                    _oSelPop.psearchcol = cSrchCol;
                    _oSelPop.pDisplayColumnList = cDispCol;
                    _oSelPop.pRetcolList = cRetCol;
                }
                else
                {
                    string[] valList = filtcond.Trim().Split(',');
                    searchTbl.Columns.Add(new DataColumn("searchfld", typeof(string)));
                    foreach (var item in valList)
                    {
                        DataRow row = searchTbl.NewRow();
                        row[0] = item;
                        searchTbl.Rows.Add(row);
                    }
                    cSrchCol = searchTbl.Columns[0].ColumnName;
                    cRetCol = searchTbl.Columns[0].ColumnName;
                    cDispCol = searchTbl.Columns[0].ColumnName.Trim()+":"+lbl.Text;
                    _oSelPop.pformtext = cFrmCap;
                    _oSelPop.psearchcol = cSrchCol;
                    _oSelPop.pDisplayColumnList = cDispCol;
                    _oSelPop.pRetcolList = cRetCol;
                }
                DataView _dvw = searchTbl.DefaultView;
                _oSelPop.pdataview = _dvw;
                _oSelPop.Icon = this.Icon;
                _oSelPop.ShowDialog();

                if (_oSelPop.pReturnArray != null)
                {
                    //_commonDs.Tables[_TblName].Rows[this.BindingContext[_commonDs, _TblName].Position][fldnm] = _oSelPop.pReturnArray[0].ToString();      //Commented by Shrikant S. on 09/10/2018 
                    currentView[0][fldnm] = _oSelPop.pReturnArray[0].ToString();                                                  //Added by Shrikant S. on 09/10/2018 
                    Control[] ctl = this.pnlUpper.Controls.Find("txt" + fldnm, false);
                    TextBox txt = (TextBox)ctl[0];
                    txt.Text = _oSelPop.pReturnArray[0].ToString();
                }
            }
        }
        //Added by Shrikant S. on 05/10/2018        //End
        private void btnDone_Click(object sender, EventArgs e)
        {
            //currentView.RowFilter = "";

            DataRow[] sourcerows=null ;
            if (this._HdrDtl=="D")
                sourcerows = _commonDs.Tables[_TblName].Select("ItemRowId=" + _filterid);
            else
                sourcerows = _commonDs.Tables[_TblName].Select();

            for (int i = 0; i < sourcerows.Count(); i++)
            {
                foreach (DataRow _dr in _dtLother.Rows)
                {
                    if (_dr["Inter_Use"].ToString().ToUpper() == ".T." || Convert.ToBoolean(_dr["ingrid"]) == true)
                    {
                        continue;
                    }
                    sourcerows[i][_dr["fld_nm"].ToString().Trim()] = _sourceTable.Rows[i][_dr["fld_nm"].ToString().Trim()];
                }
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _commonDs.Tables[_TblName].RejectChanges();           
            this.Close();
        }

    }
}

