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
*!*	mReportType	 : 'T' For Trial Balanace 'G' - Group Summary
*!*	GCode		 : Group Code
*!*	sqldatasession : Datasession

If Parameters() <> 5
	=Messagebox('Pass Valid Parameters')
	Return .T.
Endif
****Versioning**** && Added By Amrendra for TKT 8121 on 13-06-2011
Local _VerValidErr,_VerRetVal,_CurrVerVal
_VerValidErr = ""
_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
Try
	_VerRetVal = AppVerChk('TRIALBALANCE',_CurrVerVal,Justfname(Sys(16)))
Catch To _VerValidErr
	_VerRetVal  = 'NO'
Endtry
If Type("_VerRetVal")="L"
	cMsgStr="Version Error occured!"
	cMsgStr=cMsgStr+Chr(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
	Messagebox(cMsgStr,64,VuMess)
	Return .F.
Endif
If _VerRetVal  = 'NO'
	Return .F.
Endif
****Versioning****

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

llRet = MakeAcmast(mFromDt,mTodate,sqldatasession,mReportType)		&& Make Accounts Master
If llRet = .F.
	Return .F.
Endif

lnI=MakeLevel(mReportType)												&& Make Level
If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 80
Endif
&&vasant041109
*!*	IF INLIST(mReportType,'P','B')
*!*		SELECT _TBAcMast
*!*		REPLACE ALL level WITH level - 1 IN _TBAcMast
*!*		GO top
*!*	Endif
&&vasant041109

Select _TBAcMast
*!*	COPY TO "d:\_TBAcMast.dbf"
Go Top

Do Form finalacc With mReportType,GCode,mFromDt,mTodate,sqldatasession,lnI
If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 100
	Statdesktop.ProgressBar.Visible = .F.
Endif
