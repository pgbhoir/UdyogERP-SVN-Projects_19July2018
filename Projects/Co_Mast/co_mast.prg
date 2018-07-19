Parameters What,pRange
** Added By amrendra on 09/06/2011 for TKT-8121 Start
****Versioning****
Local _VerValidErr,_VerRetVal,_CurrVerVal
_VerValidErr = ""
_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
Try
	_VerRetVal = AppVerChk('CO_MAST',_CurrVerVal,Justfname(Sys(16)))
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
** Added By amrendra on 09/06/2011 for TKT-8121 End
If Vartype(VuMess) <> [C]
	_Screen.Visible = .F.
	Messagebox("Internal Application Are Not Execute Out-Side ...",16)
	Return .F.
Endif

rval = .F.

If ! "\datepicker." $ Lower(Set("Classlib"))
	Set Classlib To apath+"class\datepicker.vcx" Additive
	*!*		Set Classlib To "datepicker.vcx" Additive
Endif

Do Form co_mast With What,pRange Name rval
