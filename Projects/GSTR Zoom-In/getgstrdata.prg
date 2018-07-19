Lparameters _mFromDt,_mTodate,_sqldatasession

nHandle =0
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)

Ldate = Set("Date")

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 10
Endif

Strdrcr = "Set Dateformat DMY Select * from GSTR1_VW where Date between '"+Dtoc(_mFromDt)+"' and '"+Dtoc(_mTodate)+"' "
sql_con=sqlconobj.dataconn("EXE",company.DbName,Strdrcr,"_GSTRData","nHandle",_sqldatasession)
If sql_con =< 0
	Set Date &Ldate
	=Messagebox('Main cursor creation '+Chr(13)+Message(),0+16,VuMess)
	Return .F.
Endif

Strdrcr = "Set Dateformat DMY Select * from GSTR1_VW where AmendDate between '"+Dtoc(_mFromDt)+"' and '"+Dtoc(_mTodate)+"' AND HSNCODE <> '' "
sql_con=sqlconobj.dataconn("EXE",company.DbName,Strdrcr,"_GSTRDataAmend","nHandle",_sqldatasession)
If sql_con =< 0
	Set Date &Ldate
	=Messagebox('Main cursor creation '+Chr(13)+Message(),0+16,VuMess)
	Return .F.
Endif


Strdrcr = "Set Dateformat DMY "+;
	"SELECT ENTRY_TY, TRAN_CD, DATE, DATE_ALL, ENTRY_ALL, MAIN_TRAN FROM STMALL where Date between '"+Dtoc(_mFromDt)+"' and '"+Dtoc(_mTodate)+"' AND ENTRY_TY='ST' "+;
	"UNION ALL "+;
	"SELECT ENTRY_TY, TRAN_CD, DATE, DATE_ALL, ENTRY_ALL, MAIN_TRAN FROM SBMALL where Date between '"+Dtoc(_mFromDt)+"' and '"+Dtoc(_mTodate)+"' AND ENTRY_TY='S1' "
sql_con=sqlconobj.dataconn("EXE",company.DbName,Strdrcr,"_BkRecDet","nHandle",_sqldatasession)
If sql_con =< 0
	Set Date &Ldate
	=Messagebox('Main cursor creation '+Chr(13)+Message(),0+16,VuMess)
	Return .F.
Endif

Strdrcr = "Set Dateformat DMY "+;
	"SELECT H.Entry_ty, H.Tran_cd, H.INV_NO, H.DATE, AC.GSTIN, d.AMTEXCGST AS Taxableamt, d.CGST_PER, D.CGST_AMT, d.SGST_PER, "+;
	"D.SGST_AMT, D.IGST_PER, D.IGST_AMT, D.GRO_AMT, pos = h.GSTState, ac.SUPP_TYPE, st_type= ac.st_type, h.net_amt, "+;
	"D.CGSRT_AMT, D.SGSRT_AMT, D.IGSRT_AMT,0 as CESS_AMT "+;
	"FROM BRMAIN H  "+;
	"INNER JOIN BRITEM D ON (H.ENTRY_TY = D .ENTRY_TY AND H.TRAN_CD = D .TRAN_CD) "+;
	"INNER JOIN IT_MAST IT ON (D .IT_CODE = IT.IT_CODE) "+;
	"LEFT OUTER JOIN  ac_mast ac ON (h.Ac_id = ac.ac_id) "+;
	"WHERE H.TDSPAYTYPE = 2 AND H.ENTRY_TY ='BR' "+;
	"union all "+;
	"SELECT H.Entry_ty, H.Tran_cd, H.INV_NO, H.DATE, AC.GSTIN, d.AMTEXCGST AS Taxableamt, d.CGST_PER, D.CGST_AMT, d.SGST_PER, "+;
	"D.SGST_AMT, D.IGST_PER, D.IGST_AMT, D.GRO_AMT, pos = h.GSTState, ac.SUPP_TYPE, st_type=ac.st_type, h.net_amt, "+;
	"D.CGSRT_AMT, D.SGSRT_AMT, D.IGSRT_AMT,0 as CESS_AMT "+;
	"FROM CRMAIN H "+;
	"INNER JOIN CRITEM D ON (H.ENTRY_TY = D .ENTRY_TY AND H.TRAN_CD = D .TRAN_CD) "+;
	"INNER JOIN IT_MAST IT ON (D .IT_CODE = IT.IT_CODE) "+;
	"LEFT OUTER JOIN Ac_mast ac ON (h.Ac_id = ac.ac_id) "+;
	"WHERE H.TDSPAYTYPE = 2 AND H.ENTRY_TY ='CR' "
sql_con=sqlconobj.dataconn("EXE",company.DbName,Strdrcr,"_EarlierBkDtl","nHandle",_sqldatasession)
If sql_con =< 0
	Set Date &Ldate
	=Messagebox('Main cursor creation '+Chr(13)+Message(),0+16,VuMess)
	Return .F.
Endif
sqlconobj.sqlConnClose("nHandle")


Select A.* From _EarlierBkDtl A ;
	INNER Join _BkRecDet B On A.ENTRY_TY=B.ENTRY_ALL And A.TRAN_CD=B.MAIN_TRAN ;
	WHERE B.DATE_ALL < _mFromDt And Inlist(ENTRY_ALL,'BR','CR') ;
	INTO Cursor _EarlierBkDtl

Select _GSTRData
Go Top
