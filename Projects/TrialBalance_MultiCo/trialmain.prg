*:*****************************************************************************
*:        Program: TrialMain
*:         System: Udyog Software
*:         Author: RAGHU
*:  Last modified: 27/04/2006
*:			AIM  : Trial Balance Or Group Summary Report In ZOOM IN
*:			Use	 : =TrialMain(FromDate,ToDate,'T','')	&& Trial Balance
*:				 : =TrialMain(FromDate,ToDate,'B','')	&& Balance Sheet
*:				 : =TrialMain(FromDate,ToDate,'P','')	&& Profit And Loss Account
*:				 : =TrialMain(FromDate,ToDate,'G',13)	&& Group Summary
*:*****************************************************************************
Parameters mFromDt,mTodate,mReportType,GCode,sqldatasession
*!*	mFromDt		 : From Date
*!*	mTodate		 : To Date
*!*	mReportType	 : 'T' For Trial Balance 'G' - Group Summary
*!*	GCode		 : Group Code
*!*	sqldatasession : Datasession

If Parameters() <> 5
	=Messagebox('Pass Valid Parameters',64,vuMess)
	Return .T.
Endif

If !("GRIDFIND.VCX" $ Upper(Set("Classlib")))
	Set Classlib To gridfind.vcx Additive
Endif
If !("CONFA.VCX" $ Upper(Set("Classlib")))
	Set Classlib To confa.vcx Additive
Endif

Local lnI
Set Talk Off
Set StrictDate To 0
Set Deleted On
mFromDt = Ttod(mFromDt)
mTodate = Ttod(mTodate)

_dbNames = ueTrig_MultiCoFinStmnt(sqldatasession)

llRet = MakeAcmast(mFromDt,mTodate,sqldatasession,mReportType,_dbNames)		&& Make Accounts Master

If llRet = .F.
	Return .F.
Endif

lnI=MakeLevel(mReportType)													&& Make Level

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 80
Endif

cAlias=Alias()

Select (cAlias)
Do Form finalacc With mReportType,GCode,mFromDt,mTodate,sqldatasession,lnI

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 100
	Statdesktop.ProgressBar.Visible = .F.
Endif
