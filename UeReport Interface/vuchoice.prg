Para ucaption1,ucond1
*!*	&&vasant16/11/2010	Changes done for VU 10 (Standard/Professional/Enterprise)
****Versioning****  Added By Amrendra On 01/06/2011
Local _VerValidErr,_VerRetVal,_CurrVerVal
_VerValidErr = ""
_VerRetVal  = 'NO'
*!*		_CurrVerVal='10.0.0.0' &&[VERSIONNUMBER]			&& Commented by Shrikant S. on 13/04/2019 for Registration
_CurrVerVal='2.2.0.0' &&[VERSIONNUMBER]			&& Added by Shrikant S. on 13/04/2019 for Registration
Try
	_VerRetVal = AppVerChk('REPORTINTERFACE',_CurrVerVal,Justfname(Sys(16)))
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

*!*	&&vasant16/11/2010	Changes done for VU 10 (Standard/Professional/Enterprise)

If ! "datepicker"  $ Lower(Set("Classlib"))
	Set Classlib To APath+"\Class\datepicker.vcx" Additive
Endif
&&Commented by Prajakta B. on 31/08/2019 for Bug 32807  &&Start
*!*	If !("GRIDFIND.VCX" $ Upper(Set("Classlib"))) &&Rup 19/02/2010 TKT-110
*!*		Set Classlib To gridfind.vcx Additive
&&Commented by Prajakta B. on 31/08/2019 for Bug 32807  &&End

If !("GRIDFIND_rep.VCX" $ Upper(Set("Classlib"))) &&Modified by Prajakta B. on 31/08/2019 for Bug 32807
	Set Classlib To gridfind_rep.vcx Additive  &&Modified by Prajakta B. on 31/08/2019 for Bug 32807
Endif

&& Added by Shrikant S. on 23/06/2013 for Bug-16293		&& Start
If ! Pemstatus(Company,"copyname",5)
	AddProperty(Company,"copyname","")
Endif
&& Added by Shrikant S. on 23/06/2013 for Bug-16293		&& End

Do Form vureport With ucaption1,ucond1
Return
