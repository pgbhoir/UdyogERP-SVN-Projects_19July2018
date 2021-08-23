&&Changes done by Vasant on 30/04/2013 as per Bug 7303(Barcode Printing Details).
Parameter lmainfrm,lrectype
Local _VerValidErr,_VerRetVal,_CurrVerVal
_VerValidErr = ""
_VerRetVal  = 'NO'
Try
	_VerRetVal = AppVerChk('BARCODETRAN',GetFileVersion(),Justfname(Sys(16)))
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

Sele BarCodeMast_vw
Do Form frmBarCodeTran With lmainfrm,lrectype


Function GetFileVersion
	Parameters lcTable
	_CurrVerVal='10.0.0.0'
	If !Empty(lcTable)
		Select(lcTable)
		Append Blank
		Replace fVersion With Justfname(Sys(16))+'   '+_CurrVerVal
	Endif
	Return _CurrVerVal
