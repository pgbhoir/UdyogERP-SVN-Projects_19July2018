Lparameters _mFromDt,_mTodate,_mReportType,_sqldatasession

nHandle =0
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)

Ldate = Set("Date")

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 10
Endif

Do Case
	Case _mReportType='GSTR1_Z'
		Strdrcr = "Set Dateformat DMY EXECUTE USP_GETGSTR1DATA '"+Dtoc(_mFromDt)+"','"+Dtoc(_mTodate)+"' "
	Case _mReportType='GSTR2_Z'
		Strdrcr = "Set Dateformat DMY EXECUTE USP_GETGSTR2DATA '"+Dtoc(_mFromDt)+"','"+Dtoc(_mTodate)+"' "
Endcase


sql_con=sqlconobj.dataconn("EXE",company.DbName,Strdrcr,"_GSTRData","nHandle",_sqldatasession)
If sql_con =< 0
	Set Date &Ldate
	=Messagebox('Main cursor creation '+Chr(13)+Message(),0+16,VuMess)
	Return .F.
Endif

sqlconobj.sqlConnClose("nHandle")

Select _GSTRData
Go Top
