*PARA _mailto,_mailcc,_mailsub,_mailbody,_mailatt,_mailshow
LPARA _mailto,_mailcc,_mailsub,_mailbody,_mailatt,_mailshow,_Mailbcc &&Birendra : Bug-4800 on 26/09/2012

*Birendra : Bug-4800 on 26/09/2012:Start:
IF TYPE('_Mailbcc')<>'C'
 _Mailbcc=''
ENDIF 
*Birendra : Bug-4800 on 26/09/2012:End:

&&Changes done by vasant on 15/06/2012 as per Bug-4648 (Avery - Auto Email Testing)
IF PARA() <= 5
	_mailshow = .T.
ENDIF
&&Changes done by vasant on 15/06/2012 as per Bug-4648 (Avery - Auto Email Testing)

****Versioning**** && Added By Amrendra for TKT 8121 on 13-06-2011
	LOCAL _VerValidErr,_VerRetVal,_CurrVerVal
	_VerValidErr = ""
	_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]
	TRY
		_VerRetVal = AppVerChk('MAILING',_CurrVerVal,JUSTFNAME(SYS(16)))
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
****Versioning****
&&Changes done by vasant on 15/06/2012 as per Bug-4648 (Avery - Auto Email Testing)
*!*	IF PARA() <= 5
*!*		_mailshow = .T.
*!*	ENDIF
&&Changes done by vasant on 15/06/2012 as per Bug-4648 (Avery - Auto Email Testing)

*!*	_mailto   = Iif(!Empty(_mailto),Eval(_mailto),"")
*!*	_mailcc   = Iif(!Empty(_mailcc),Eval(_mailcc),"")


LOCAL llShowItem AS Boolean,lnVar AS INTEGER
#DEFINE olMailItem  0
*!*	loOutlook   = CREATEOBJECT('Outlook.Application')		&& Commented by Shrikant S. on 10/09/2015 for Bug-26664
&& Added by Shrikant S. on 10/09/2015 for Bug-26664		&&  Start
_curobject=_Screen.ActiveForm

Set DataSession To _curobject.DataSessionId


etsql_str="Select * From eMailSettings"	
etsql_con = _curobject.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[esetting],"_curobject.nHandle",_curobject.DataSessionId)


ans=.T.

TRY
	&& Added by Shrikant S. on 25/09/2018 for Bug-31906		&& Start
	llsmtp=.f.	
	IF TYPE('esetting.issmtpdefa')<>'U'
		IF esetting.issmtpdefa=.t.
			llsmtp=.t.
			ans=.f.
		endif	
	endif
	
	IF !llsmtp
		loOutlook   = Createobject('Outlook.Application')		
	ENDIF
	&& Added by Shrikant S. on 25/09/2018 for Bug-31906		&& End
	
	
*!*		loOutlook   = Createobject('Outlook.Application')				&& Commented by Shrikant S. on 25/09/2018 for Bug-31906

Catch To oException
*	Messagebox("Outlook setting not found.",64,VuMess)
	ans=.F.
Endtry

If ans=.F.
	lcmail=smtpmail(_mailto,_mailcc,_mailsub,_mailbody,_mailatt,_Mailbcc,_mailshow,_curobject)
	If !Empty(lcmail)
		Messagebox(lcmail,0,VuMess)
	Else
		ans=.T.
	Endif
	Return ans
Endif
&& Added by Shrikant S. on 10/09/2015 for Bug-26664		&& End


loNameSpace = loOutlook.GetNamespace("MAPI")
*!*	loNameSpace.Logon()				&& Commented by Shrikant S. on 10/09/2015 for Bug-26664
&& Added by Shrikant S. on 19/09/2015 for bug-26664		&& Start
Try

	loNameSpace.Logon("Outlook")
Catch To oEx
	Messagebox("Could not logon."+Chr(13)+"Mail will not be generated.",0,VuMess)
	Return .F.
Endtry
&& Added by Shrikant S. on 19/09/2015 for bug-26664		&& End


loMailItem  = loOutlook.CreateItem( olMailItem )   && This creates the MailItem Object
loMailItem.BodyFormat=2
llShowItem  = _mailshow		&&.T.

WITH loMailItem
	.TO      = _mailto
	.Cc      = _mailcc
	.Subject = _mailsub
*	.Body    = _mailbody           &&Comment By Amrendra

&& Raghu110809
	Tmp_mailatt = "<<"+STRTRAN(ALLTRIM(_mailatt),";",">><<")+">>"
	FOR lnVar = 1 TO OCCURS("<<",Tmp_mailatt)
		_mailatt = STREXTRACT(Tmp_mailatt,"<<",">>",lnVar)
		IF !EMPTY(_mailatt)
			IF FILE(_mailatt)
				.attachments.ADD((_mailatt))
			ENDIF
		ENDIF
	ENDFOR
&& Raghu110809

&& Commented by raghu code is not proper for multiple attachment [Start]
*!*	*!*		IF !EMPTY(_mailatt)
*!*	*!*			.attachments.ADD((_mailatt))
*!*	*!*		ENDIF
&& Commented by raghu code is not proper for multiple attachment [End]
	&&Changes done by vasant on 15/06/2012 as per Bug-4648 (Avery - Auto Email Testing)
&&Commented By Prajakta B. on 10/08/2019 for Bug 32773  --Start
*!*		IF !EMPTY(_mailbody)  
*!*			.HTMLBody=  _mailbody + .HTMLBody 
*!*		Endif					
&&Commented By Prajakta B. on 10/08/2019 for Bug 32773  --End
	&&Changes done by vasant on 15/06/2012 as per Bug-4648 (Avery - Auto Email Testing)
	IF llShowItem
		.DISPLAY()      && Shows the New Message Dialog with all details from
		&&Added By Prajakta B. on 10/08/2019 for Bug 32773  --Start
		IF !EMPTY(_mailbody)  
			.HTMLBody = _mailbody + .HTMLBody  
		ENDIF
		&&Added By Prajakta B. on 10/08/2019 for Bug 32773  --End
	ELSE
		.SEND()         && Calling this will cause a Security Dialog
	ENDIF
		*.HTMLBody=  _mailbody + .HTMLBody  &&Added By Amrendra	&&Changes done by vasant on 15/06/2012 as per Bug-4648 (Avery - Auto Email Testing)
ENDWITH

