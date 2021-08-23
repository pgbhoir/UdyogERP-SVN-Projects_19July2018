Parameters _mailto,_mailcc,_mailsub,_mailbody,_mailatt,_Mailbcc,_showform,_curobject

_curvouobj = _Screen.ActiveForm
Set DataSession To _curvouobj.DataSessionId
If !Used('esetting')		&& Added by Shrikant S. on 25/09/2018 for Bug-31906
	etsql_str="Select * From eMailSettings"
	etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[esetting],"_curvouobj.nHandle",_curvouobj.DataSessionId)
ENDIF						&& Added by Shrikant S. on 25/09/2018 for Bug-31906

If Reccount('esetting')<=0
	Return "Outlook/SMTP setting not found for email."
Endif

Select esetting
Locate

Local loConfig As CDO.Configuration, loFlds As Object, loMsg As CDO.Message
loConfig = Createobject("CDO.Configuration")

loFlds = loConfig.Fields
lcSchema = "http://schemas.microsoft.com/cdo/configuration/"

With loFlds
*- Set the CDOSYS configuration fields to use port 25 on the SMTP server.
	.Item(lcSchema+"sendusing") = 2

*- Enter name or IP address of remote SMTP server.
	.Item(lcSchema+"smtpserver") = Alltrim(esetting.Host)
	.Item(lcSchema+"smtpserverport") =esetting.port

*- Assign timeout in seconds
*	.Item(lcSchema+"smtpconnectiontimeout") = 10
	.Item(lcSchema+"smtpusessl") = esetting.enablessl

*- Commit changes to the object
	.Item(lcSchema+"sendusername") = Alltrim(esetting.username)
	.Item(lcSchema+"sendpassword") = Strconv(Alltrim(esetting.Password),14)
	.Item(lcSchema+"smtpauthenticate") = 1
&& .item(http://schemas.microsoft.com/cdo/configuration/cdoURLProxyServer") = "smtp.mail.yahoo.com"

	.Update()
Endwith
If Empty(_mailbody)
	_mailbody="Dear Sir/Madam,"+Chr(13)+"Please find the attachment. "+Chr(13)+Chr(13)+Chr(13)+"Thanking you,"+Chr(13)+Alltrim(esetting.yourname)
Endif


*!*	CREATE CURSOR bodyCur(bodytxt memo)
*!*	APPEND BLANK
*!*	replace bodytxt  WITH _mailbody IN bodyCur

errmsg=""
_showform=.T.
_sendmail=.T.
If _showform=.T.
	tbrDesktop.Enabled=.F.

&& Added by Shrikant S. on 	14/08/2018 for Bug-31764		&& Start
	_mailbody=Strtran(_mailbody,"<html>","")
	_mailbody=Strtran(_mailbody,"</html>","")
	_mailbody=Strtran(_mailbody,"<body>","")
	_mailbody=Strtran(_mailbody,"</body>","")
	_mailbody=Strtran(_mailbody,"<br>",Chr(13))
	_mailbody=Strtran(_mailbody,"</br>",Chr(13))
	_mailbody=Strtran(_mailbody,"<b>","")
	_mailbody=Strtran(_mailbody,"</b>","")
&& Added by Shrikant S. on 	14/08/2018 for Bug-31764		&& End

	_Mailbcc = Iif(Lower(Alltrim(esetting.username)) $ _Mailbcc,_Mailbcc,Iif(!Empty(_Mailbcc),_Mailbcc+','+Alltrim(esetting.username),Alltrim(esetting.username)))	&& Added by Sachin N. S. on 18/09/2015 for Bug-26664
	Do Form frmsendmail With _mailto,_mailcc,_mailsub,_mailbody,_mailatt,_Mailbcc,Alltrim(esetting.username),Alltrim(esetting.yourname),loConfig,_curobject To loMsg
	tbrDesktop.Enabled=.T.
	If Type('loMsg')='L'
		_sendmail=.F.
	Endif
Endif



If Type('loMsg')<>'O' And _sendmail=.T.
	loMsg = Createobject("CDO.Message")

	With loMsg
		.Configuration = loConfig
		.To = _mailto
		.CC=_mailcc
		.BCC=_Mailbcc
		.From = Alltrim(esetting.username)
		.Subject = _mailsub

		Tmp_mailatt = "<<"+Strtran(Alltrim(_mailatt),";",">><<")+">>"
		For lnVar = 1 To Occurs("<<",Tmp_mailatt)
			_mailatt = Strextract(Tmp_mailatt,"<<",">>",lnVar)

			If !Empty(_mailatt)
				If File(_mailatt)
					.addattachment(_mailatt)
				Endif
			Endif
		Endfor

		If !Empty(_mailbody)
			.TextBody =  _mailbody
		Else
			.TextBody =  ""
		Endif

	Endwith
Endif
If _sendmail=.T.
	Try
		loMsg.Send()
		Messagebox("Mail sent successfully",0,vumess,2)
	Catch To oerr
		errmsg="The message could not be sent to the SMTP Server. Please check the SMTP setting."
	Endtry
Endi

*- Set priority to HIGH if needed
*!* IF tlUrgent
*!* .Fields("Priority").Value = 1 && -1=Low, 0=Normal, 1=High
*!* .Fields.Update()
*!* ENDIF


Return errmsg





