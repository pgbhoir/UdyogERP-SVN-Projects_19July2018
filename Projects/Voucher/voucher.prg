Para VuType,VuParty,VuSeries,VuDept,VuCate,VuCaption,Fromzoom,VuZoomUpdt,VuCond,VuRange		&&vasant160409a2
&&vasant160409a2
_VuParaCount = Parameters( )	&&This line should be after Parameter line
If _VuParaCount = 9
	VuRange = VuCond
Endif
&&vasant160409a2

&&vasant16/11/2010	Changes done for VU 10 (Standard/Professional/Enterprise)
*!*	&& Version Checking [Start] Raghu - 210509
*!*	Local lcMessage
*!*	lcMessage = "Invalid version…"
*!*	If Vartype(GlobalObj) <> "O"
*!*		Messagebox(lcMessage,64,VuMess)
*!*		Return .F.
*!*	Endif

*!*	If Upper(DEC(GlobalObj.GetPropertyVal('VersionId'))) <> Upper("Monday,27th Sept. 2010")		&&vasant280910
*!*		Messagebox(lcMessage,64,VuMess)
*!*	*!*		Return .F.
*!*	Endif
*!*	&& Version Checking [End] Raghu - 210509
* Added by Amrendra on 11-06-2011 for tkt 8121 versioning Start

****Versioning****
Local _VerValidErr,_VerRetVal,_CurrVerVal
_VerValidErr = ""
_VerRetVal  = 'NO'
Try

	_VerRetVal = AppVerChk('VOUCHER',GetFileVersion(),Justfname(Sys(16)))
Catch To _VerValidErr
	_VerRetVal  = 'NO'
Endtry
If Type("_VerRetVal")="L"
	cMsgStr="Version Error occured!"
	cMsgStr=cMsgStr+Chr(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
	Messagebox(cMsgStr,64,VuMess)
*!*		Return .F.
Endif
If _VerRetVal  = 'NO'
*!*		Return .F.
Endif
****Versioning****

* Added by Amrendra on 11-06-2011 for tkt 8121 versioning End
*!*	* Commented by Amrendra on 11-06-2011 for tkt 8121 versioning Start
*!*	LOCAL _VerValidErr,_VerRetVal
*!*	_VerValidErr = ""
*!*	_VerRetVal  = .f.
*!*	TRY
*!*		_VerRetVal = AppVerChk('VOUCHER','1st Jan.,2011')
*!*	CATCH TO _VerValidErr
*!*		_VerRetVal  = .f.
*!*	Endtry
*!*	IF _VerRetVal  = .f.
*!*		Messagebox("Invalid version…",64,VuMess)
*!*		Return .F.
*!*	ENDIF
*!*	* Commented by Amrendra on 11-06-2011 for tkt 8121 versioning  End
&&vasant16/11/2010	Changes done for VU 10 (Standard/Professional/Enterprise)
Do Form Voucher With VuType,VuParty,VuSeries,VuDept,VuCate,VuCaption,Fromzoom,VuZoomUpdt,VuCond,VuRange		&&vasant160409a2

Return

*>>>***Versioning**** Added By Amrendra On 05/07/2011
Function GetFileVersion
	Parameters lcTable
*!*		_CurrVerVal='10.2.0.0' &&[VERSIONNUMBER] &&TKT-9038 Rup Change version to 10.2 from 10.1		&& Commented by Shrikant S. on 13/04/2019 for Registration
	_CurrVerVal='2.2.0.0' &&[VERSIONNUMBER] &&TKT-9038 Rup Change version to 10.2 from 10.1		&& Added by Shrikant S. on 13/04/2019 for Registration
	If !Empty(lcTable)
		Select(lcTable)
		Append Blank
		Replace fVersion With Justfname(Sys(16))+'   '+_CurrVerVal
	Endif
	Return _CurrVerVal
	*<<<***Versioning**** Added By Amrendra On 05/07/2011
