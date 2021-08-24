*:*****************************************************************************
*:        Program: Makeacmast.PRG
*:         System: Udyog Software
*:         Author: RAGHU
*:  Last modified: 19/09/2006
*:			AIM  : Create Account Master
*:*****************************************************************************

Parameters FrDate,TODate,sqldatasession,mReportType,_cDbName
If Type('sqldatasession') = 'N'
	Set DataSession To sqldatasession
Endif
Set Deleted On
nHandle =0
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 10
Endif

Ldate = Set("Date")
Set Date AMERICAN
*!*	Collecting Debit and Credit Balance [Start]
Strdrcr = " EXEC Usp_Multi_Co_Final_Accounts '"+Dtoc(FrDate)+"','"+Dtoc(TODate)+"','"+Dtoc(Company.Sta_Dt)+"','"+mReportType+"', '"+_cDbName+"'"
sql_Con=sqlconobj.dataConn("EXE",Company.DbName,Strdrcr,"_CTBAcMast","nHandle",sqldatasession)
If sql_Con =< 0
	Set Date &Ldate
	=Messagebox('Main cursor creation '+Chr(13)+Message(),0+16,VuMess)
	Return .F.
Endif

*!*	Collecting Debit and Credit Balance [End]
Set Date &Ldate

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 30
Endif

If mReportType = 'P'
	Update _CTBAcMast Set ClBal = opBal+Debit-Abs(Credit)
Endif

Set ENGINEBEHAVIOR 70
Select Space(1) As LevelFlg, ;
	SPACE(100) As OrderLevel, ;
	00000000000000000000 As Level, ;
	00000000000000000000 As LevelInt, ;
	a.Updown, a.MainFlg, a.Ac_Id, a.Ac_Group_Id, a.Ac_Name, a.Group, ;
	SUM(a.opBal) As opBal, Sum(a.Debit) As Debit, Sum(Abs(a.Credit)) As Credit, ;
	SUM((a.opBal+a.Debit-Abs(a.Credit))) As ClBal, Sum(a.OpBalAmt) As OpBalAmt, a.DbName, a.Co_Name, 000000000000000 As MaxLevel ;
	FROM _CTBAcMast a Group By Ac_Name,Group,DbName ;
	INTO Cursor _CTBAcMast Readwrite
Set ENGINEBEHAVIOR 90

*!*	Close Temp Cursors [Start]
=CloseTmpCursor()
*!*	Close Temp Cursors [End]

If Inlist(mReportType,'B','P','T')
	mShowStkfrm = 0

	Select _CTBAcMast
	If Inlist(mReportType,'B','P')
		mShowStkfrm = 1
	Endif

	If Inlist(mReportType,'B','P')
		Select _CTBAcMast
		Locate For Allt(Ac_Name)=Iif(mReportType='B','PROVISIONAL EXPENSES','PROVISIONAL EXPENSES (P & L)') And MainFlg = 'L'
		If Found()
			mShowStkfrm = mShowStkfrm+1
		Endif
	Endif

	Select _CTBAcMast
	If Inlist(mReportType,'B','P','T')
		mShowStkfrm = mShowStkfrm+1
	Endif

	If mShowStkfrm > 0
		Do Form frmstkval With sqldatasession,mReportType,_cDbName
	Endif
Endif

Select _CTBAcMast
Return .T.


Function CloseTmpCursor
***********************
sql_Con = sqlconobj.SqlConnClose('nHandle')
If sql_Con < 0
	=Messagebox(Message(),0+16,VuMess)
	Return .T.
Endif

Release sqlconobj,nHandle
