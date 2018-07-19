PARA cAcGroup,mType,pRange

IF ! "\datepicker." $ LOWER(SET("Classlib"))
	SET CLASSLIB TO apath+"class\datepicker.vcx" ADDITIVE
ENDIF

IF ! "\vouclass." $ LOWER(SET("Classlib"))
	SET CLASSLIB TO apath+"class\vouclass.vcx" ADDITIVE
ENDIF

IF ! "\UITools." $ LOWER(SET("Classlib"))
	SET CLASSLIB TO UITools.vcx ADDITIVE
ENDIF

****Versioning**** Added By Amrendra On 01/06/2011 TKT 8473 
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
	TRY
		_VerRetVal = AppVerChk('ACMAST',GetFileVersion(),JUSTFNAME(SYS(16)))
	CATCH TO _VerValidErr
		_VerRetVal  = 'NO'
	Endtry	
	IF TYPE("_VerRetVal")="L"
		cMsgStr="Version Error occured!"
		cMsgStr=cMsgStr+CHR(13)+"Kindly update latest version of "+GlobalObj.getPropertyval("ProductTitle")
		Messagebox(cMsgStr,64,VuMess)
		Return .F. 
	ENDIF
	IF _VerRetVal  = 'NO' 
		Return .F.
	Endif
*!*	****Versioning****  TKT 8473 



mGroup=ALLTRIM(cAcGroup)

&& Added by Shrikant S. on 26/04/2017 for GST		Start
lcFilter=""
If ("#pid=" $ mType)
	lcval=Strextract(mType,"#pid=","#")
	If Len(lcval) >0
		mType=Strtran(mType,"#pid="+lcval+"#","")
	Endif
ENDIF
&& Added by Shrikant S. on 26/04/2017 for GST		End

IF EMPTY(mGroup)
	If Type('lcval')='C'
		lcFilter="Ac_id="+Transform(lcval)		&& Added by Shrikant S. on 26/04/2017 for GST
	ENDIF
	DO FORM acmast WITH cAcGroup,mType,pRange,lcFilter		&& Changed by Shrikant S. on 26/04/2017 for GST
ENDIF
IF UPPER(mGroup)="GROUP"
	If Type('lcval')='C'
		lcFilter="Ac_group_id="+Transform(lcval)	&& Added by Shrikant S. on 26/04/2017 for GST
	Endif
	DO FORM acmast WITH cAcGroup,mType,pRange,lcFilter		&& Changed by Shrikant S. on 26/04/2017 for GST
ENDIF
IF UPPER(mGroup)="GL"
	If Type('lcval')='C'
		lcFilter="Ac_id="+Transform(lcval)		&& Added by Shrikant S. on 26/04/2017 for GST
	Endif
	DO FORM frmgl WITH mType,pRange,lcFilter		&& Changed by Shrikant S. on 26/04/2017 for GST
ENDIF


*>>>***Versioning**** Added By Amrendra On 05/07/2011 TKT 8543
FUNCTION GetFileVersion
PARAMETERS lcTable
	_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	IF !EMPTY(lcTable)
		SELECT(lcTable)
		APPEND BLANK 
		replace fVersion WITH JUSTFNAME(SYS(16))+'   '+_CurrVerVal
	ENDIF 
RETURN _CurrVerVal
*<<<***Versioning**** Added By Amrendra On 05/07/2011  TKT 8543