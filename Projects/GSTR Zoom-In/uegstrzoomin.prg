*:*****************************************************************************
*:        Program: GSTR Zoom-In
*:         System: Udyog Software
*:         Author: SACHIN N. SAPALIGA
*:  Last modified: 02/08/2017
*:			AIM  : GSTR ZOOM-IN REPORT
*:			Use	 : =ueGSTZoomIn(FromDate,ToDate,'GSTR1','')	&& GSTR1 ZOOM-IN
*:*****************************************************************************
Parameters mFromDt,mTodate,mReportType,sqldatasession
*!*	mFromDt		 : From Date
*!*	mTodate		 : To Date
*!*	sqldatasession : Datasession

If Parameters() <> 4
	=Messagebox('Pass Valid Parameters')
	Return .T.
Endif
Local _VerValidErr,_VerRetVal,_CurrVerVal
_VerValidErr = ""
_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0'
*!*	Try
*!*		_VerRetVal = AppVerChk('UEGSTRZOOMIN',_CurrVerVal,Justfname(Sys(16)))
*!*	Catch To _VerValidErr
*!*		_VerRetVal  = 'NO'
*!*	Endtry
*!*	If Type("_VerRetVal")="L"
*!*		cMsgStr="Version Error occured!"
*!*		cMsgStr=cMsgStr+Chr(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
*!*		Messagebox(cMsgStr,64,VuMess)
*!*		Return .F.
*!*	Endif
*!*	If _VerRetVal  = 'NO'
*!*		Return .F.
*!*	Endif
****Versioning****

Local lnI
Set Talk Off
Set StrictDate To 0
Set Deleted On
mFromDt = mFromDt
mTodate = mTodate

llRet = GETGSTRDATA(mFromDt,mTodate,mReportType,sqldatasession)
If llRet = .F.
	Return .F.
Endif

Do Form FRMGSTRZOOMIN With mFromDt,mTodate,mReportType,sqldatasession

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 100
	Statdesktop.ProgressBar.Visible = .F.
Endif
