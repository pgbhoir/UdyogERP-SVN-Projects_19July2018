Lparameters lrepid,lrange


Local _VerValidErr,_VerRetVal,_CurrVerVal
_VerValidErr = ""
_VerRetVal  = 'NO'
_CurrVerVal='10.0.0.0'
Try
	_VerRetVal = AppVerChk('DADOSREPORT',_CurrVerVal,Justfname(Sys(16)))
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


If Used('mx2')
	Use In mx2
Endif

If Used('mx3')
	Use In mx3
Endif

nconn=0
If 'SQLCONNECTION' $ Upper(Alltrim(Set("Classlib")))
Else
	Set Classlib To sqlconnection In &xapps Additive
Endif
Local sqlconobj
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)

If Type('mvu_nenc')='U'
	mvu_nenc=0
Endif
If mvu_nenc=1
	_user=sqlconobj.newdecry(Strconv(mvu_user,16),"Ud*yog+1993Uid")
	_pass=sqlconobj.newdecry(Strconv(mvu_pass,16),"Ud*yog+1993Pwd")
Else
	_user=sqlconobj.dec(sqlconobj.ondecrypt(mvu_user))
	_pass=sqlconobj.dec(sqlconobj.ondecrypt(mvu_pass))
Endif


constr = 'Driver={SQL server};server='+mvu_server+';database='+Alltrim(company.dbname)+';uid='+_user+';pwd='+_pass+';'

con_str='"'+lrepid + "|"  + "|"  +  "|" +	'"'

nconn=Sqlstringconnect(constr)
If nconn<= 0
	nRetval=SQLDisconnect(nconn)
	If nRetval<=0
		Return .F.
	Endif
	=Messagebox("Connection Failed !!!"+Chr(13)+Chr(13)+Message(),0+64,VuMess)
	Return .F.
Endif
Local lrpttype,lrpthead
nRetval=SQLExec(nconn,'Select * From UBIReportMast Where ReportId= ?lrepid','MX2')
If nRetval<=0
	nRetval=SQLDisconnect(nconn)
	If nRetval<=0
		Return .F.
	Endif
	=Messagebox("Error in UBIReportMast table"+Chr(13)+Chr(13)+Message(),64,VuMess)
	Return .F.
Else
	If Reccount('mx2')=0
		nRetval=SQLDisconnect(nconn)
		If nRetval<=0
			Return .F.
		Endif
		=Messagebox("Report ID :"+Alltrim(lrepid)+" not found",64,VuMess)
		Return .F.
	Else
		lrpthead = Alltrim(mx2.ReportNm)
		lrpttype = "PIVOT"
	Endif
Endif




If Inlist(Alltrim(Upper(lrpttype)),"PIVOT")		&& Added by Archana for Bug-21126  && Added by Shrikant S. on 24/12/2014  for Auto Updater 11.0.8
	tcCompId = company.CompId
	tcCompdb = company.dbname
	tcCompNm = company.co_name
	vicopath  =Strtran(icopath,' ','<*#*>')
	pApplCaption=Strtran(VuMess,' ','<*#*>')
	_PassRoute1 = Alltrim(company.PassRoute1)
	appPath=Strtran(Transform(apath),' ','<*#*>')
	csetPath=Set("Path")
	SetPath=Strtran(Transform(csetPath),' ','<*#*>')
	csdate =Strtran(Transform(Dtoc(company.sta_dt)),' ','<*#*>')
	cedate =Strtran(Transform(Dtoc(company.end_dt)),' ','<*#*>')
	_prodcode =""
	pvalue = Allt(company.PassRoute)+Iif(!Empty(Alltrim(company.PassRoute1)),",","")+Alltrim(company.PassRoute1)
	For i = 1 To Len(pvalue)
		_prodcode = _prodcode + Chr(Asc(Substr(pvalue,i,1))/2)
	Next i
	_prodcode = _prodcode+Iif(Right(Alltrim(_prodcode),1)=',',"",",")

	con_str= con_str+" "+Alltrim(Transform(tcCompId))+" "+Alltrim(tcCompdb)
	con_str= con_str+" "+Alltrim(mvu_server)+" "+Alltrim(_user)+" "+Alltrim(_pass)
	con_str= con_str+" "+Alltrim(lrange)+" "+Alltrim(musername)+" "+Alltrim(vicopath)+" "+Alltrim(pApplCaption)
	con_str= con_str+" "+Alltrim(pApplName)+" "+Alltrim(Str(pApplId))  +" "+Alltrim(pApplCode)+" "+_prodcode
	con_str= con_str+" "+Alltrim(appPath)+" "+Alltrim(SetPath)+" "+Alltrim(csdate)+" "+Alltrim(cedate)
Endif



msqlStr = "select Top 1 QueryId From UBIQueryMast Where ReportId= ?lrepid and SQLQuery Like '%{@%' "


nRetval=SQLExec(nconn,msqlStr,'MX3')
If nRetval <=0
	nRetval=SQLDisconnect(nconn)
	If nRetval<=0
		Return .F.
	Endif
	=Messagebox("Error in UBIQueryMast table"+Chr(13)+Chr(13)+Message(),64,VuMess)
	Return .F.

Else
	Select mx3
	If Reccount('mx3')=0
		Do Case
		Case lrpttype = "PIVOT"
			If File('UdMISReports.exe ') 
				nRetval=SQLDisconnect(nconn)
				If nRetval<=0
					Return .F.
				Endif
				lcCommand="UdMISReports.exe "+con_str 
				oWSHELL = Createobject("WScript.Shell")
				oWSHELL.Exec(lcCommand)
				=Inkey(1.5)
			Else
				nRetval=SQLDisconnect(nconn)
				If nRetval<=0
					Return .F.
				Endif
				=Messagebox("Udyog MIS Reports Tool not found...!!!",64,VuMess)
				Return .F.
			Endif
		Endcase
	Else
		Use In mx2
		Use In mx3
		Do Form frmUBIReport With lrepid,lrpthead,lrpttype,lrange
	Endif
Endif

*!*	endif
Return
