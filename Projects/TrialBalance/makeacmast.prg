*:*****************************************************************************
*:        Program: Makeacmast.PRG
*:         System: Udyog Software
*:         Author: RAGHU
*:  Last modified: 19/09/2006
*:			AIM  : Create Account Master
*:*****************************************************************************
*!*	Parameters FRDATE,TODate,sqldatasession,mReportType,vsDept,veDept,vsCat,veCat
Parameters FRDATE,TODate,sqldatasession,mReportType



If Type('sqldatasession') ='N'
	Set DataSession To sqldatasession
Endif

&&Rup 07 June 2010 TKT-1129 --->
vsDept="''"
veDept="''"
If  _rstatus.isdept
	vsDept="'"+Alltrim(_tmpvar.sDept)+"'"
	veDept="'"+Alltrim(_tmpvar.eDept)+"'"
Endif
vsCat="''"
veCat="''"
If _rstatus.iscategory
	vsCat="'"+Alltrim(_tmpvar.Scat)+"'"
	veCat="'"+Alltrim(_tmpvar.eCat)+"'"
Endif
&&<---Rup 07 June 2010 TKT-1129


Set Deleted On
nHandle =0
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 10
Endif

*!*	&& Added by Shrikant S. on 29/11/2017 for Bug-30928		&& Start
*!*	cocentfilt=""
*!*	If Type('oGlblPrdFeat')='O'
*!*		If oGlblPrdFeat.UdChkProd('CostCent')
*!*			DO FORM frmcostcent WITH sqldatasession TO cocentfilt
*!*		Endif
*!*	ENDIF
*!*	&& Added by Shrikant S. on 29/11/2017 for Bug-30928		&& Start

Ldate = Set("Date")
Set Date AMERICAN
*!*	Collecting Debit and Credit Balance [Start]
*!*	Strdrcr = "EXEC Usp_Final_Accounts '"+Dtoc(FRDATE)+"','"+Dtoc(TODate)+"','"+Dtoc(company.Sta_Dt)+"','"+mReportType+"' " &&Rup 07/06/2010 TKT-1129
Strdrcr = "EXEC Usp_Final_Accounts '"+Dtoc(FRDATE)+"','"+Dtoc(TODate)+"','"+Dtoc(company.Sta_Dt)+"','"+mReportType+"' "+","+vsDept+","+veDept+","+vsCat+","+veCat

&& Added by Shrikant S. on 22/12/2017 for Bug-31086			&& Start
If Type('_rstatus.iscostcent')<>'U'
	If _rstatus.iscostcent
		sq1= "Execute usp_get_costcentre_list ?_tmpvar.scost,?_tmpvar.ecost"
		nRetval = sqlconobj.dataconn([EXE],company.dbname,sq1,"_Costcentre","nHandle",sqldatasession)
		If nRetval<0
			Return .F.
		Endif

		Strdrcr = Strdrcr + ",'"+_Costcentre.ccfilter+"'"
	Endif
Endif
&& Added by Shrikant S. on 22/12/2017 for Bug-31086			&& End

*!*	sql_con=sqlconobj.dataconn("EXE",company.dbname,Strdrcr,"_CTBAcMast","nHandle",sqldatasession)
sql_con=sqlconobj.dataconn("EXE",company.dbname,Strdrcr,"_CTBAcMast1","nHandle",sqldatasession)		&& Changed by Sachin N. S. on 03/04/2018 for Bug-31398
If sql_con =< 0
	Set Date &Ldate
	=Messagebox('Main cursor creation '+Chr(13)+Message(),0+16,VuMess)
	Return .F.
Endif

***** Added by Sachin N. S. on 03/04/2018 for Bug-31398 -- Start
Select *,opbal*0 opdebit,opbal*0 opcredit,clbal*0 cldebit, clbal*0 clcredit From _CTBAcMast1 Into Cursor _CTBAcMast Readwrite
Update _CTBAcMast Set opdebit = Iif(opbal>=0, opbal,0), opcredit = Iif(opbal<0, opbal,0)
***** Added by Sachin N. S. on 03/04/2018 for Bug-31398 -- End


*!*	Collecting Debit and Credit Balance [End]
Set Date &Ldate

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 30
Endif

If mReportType = 'P'
*!*		UPDATE _CTBAcMast SET ClBal = Debit-ABS(Credit)
	Update _CTBAcMast Set clbal = opbal+Debit-Abs(Credit)	&& Changed By Sachin N. S. on 18/03/2009
	Update _CTBAcMast Set cldebit = Iif(clbal>=0, clbal,0), clcredit = Iif(clbal<0, clbal,0)		&& Added by Sachin N. S. on 03/04/2018 for Bug-31398
Endif


*!* Inserting Additional Fields [START]
Select Space(1) As LevelFlg,;
	SPACE(100) As OrderLevel,;
	000000000000000 As Level,;
	000000000000000 As LevelInt,;
	a.*;
	FROM _CTBAcMast a;
	INTO Cursor _CTBAcMast Readwrite
*!* Inserting Additional Fields [END]

*!*	Close Temp Cursors [Start]
=CloseTmpCursor()
*!*	Close Temp Cursors [End]


If Inlist(mReportType,'B','P','T')		&&vasant021109		,'T'
	mShowStkfrm = 0

	Select _CTBAcMast
&&added by satish pal for bug-20309 dated 20/11/2013-start
*Locate For Allt(Ac_Name)=Iif(mReportType='P','CLOSING STOCK (P & L)','CLOSING STOCK') And MainFlg = 'L' &&vasant021109	And ClBal <> 0
*If Found()
	If Inlist(mReportType,'B','P')
&&added by satish pal for bug-20309 dated 20/11/2013-start
		mShowStkfrm = 1
	Endif

	If Inlist(mReportType,'B','P')		&&vasant021109
		Select _CTBAcMast
		Locate For Allt(Ac_Name)=Iif(mReportType='B','PROVISIONAL EXPENSES','PROVISIONAL EXPENSES (P & L)') And MainFlg = 'L' &&vasant021109	And ClBal <> 0
		If Found()
			mShowStkfrm = mShowStkfrm+1
		Endif
	Endif		&&vasant021109

	Select _CTBAcMast
&&added by satish pal for bug-20309 dated 20/11/2013-start
*Locate For Allt(Ac_Name)=Iif(mReportType='P','OPENING STOCK','OPENING STOCK') And MainFlg = 'L' &&vasant021109	And opBal <> 0
*If Found()
	If Inlist(mReportType,'B','P','T')
&&added by satish pal for bug-20309 dated 20/11/2013-start
		mShowStkfrm = mShowStkfrm+1
	Endif

*If mShowStkfrm < 2		&&vasant021109
	If mShowStkfrm > 0	&&vasant021109
		Do Form frmstkval With sqldatasession,mReportType
	Endif

Endif
Return .T.

Function CloseTmpCursor
***********************
sql_con = sqlconobj.SqlConnClose('nHandle')
If sql_con < 0
	=Messagebox(Message(),0+16,VuMess)
	Return .T.
Endif

Release sqlconobj,nHandle
