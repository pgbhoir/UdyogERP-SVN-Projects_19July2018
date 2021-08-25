*:*****************************************************************************
*:        Program: Main
*:         System: Udyog Software
*:         Author: 
*:  Last modified:
*:			AIM  : call Dashboard
*:*****************************************************************************
Parameters pRange
If Vartype(Company) <> "O"
	Return .F.
Endif

oWSHELL = Createobject("WScript.Shell")
If Vartype(oWSHELL) <> "O"
	Messagebox("WScript.Shell Object Creation Error...",16,VuMess)
	Return .F.
Endif

Declare Integer GetCurrentProcessId  In kernel32
tcCompId = Company.CompId
tcCompdb =Company.Dbname
tcCompNm=Company.co_name
SqlConObj = Newobject('SqlConnUdObj','SqlConnection',xapps)
&&Commented by Priyanka B on 31082018 for Bug-31746 Start
*!*	mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
*!*	mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))
&&Commented by Priyanka B on 31082018 for Bug-31746 End

&&Modified by Priyanka B on 31082018 for Bug-31746 Start
If Type('mvu_nenc')='U'
	mvu_nenc=0
Endif
If mvu_nenc=1
	mvu_user1 =SqlConObj.newdecry(Strconv(mvu_user,16),"Ud*yog+1993Uid")
	mvu_pass1 =SqlConObj.newdecry(Strconv(mvu_pass,16),"Ud*yog+1993Pwd")
Else
	mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
	mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))
Endif
&&Modified by Priyanka B on 31082018 for Bug-31746 End

&&Rup Bug-2471--->
rCount=0
nhandle     = 0
_etdatasessionid = _Screen.ActiveForm.DataSessionId
Set DataSession To _etdatasessionid

&& Added by Shrikant S. on 29/01/2019 for Bug-32155			&& Start
*!*	StrSql="SELECT top 1 Last_Updated FROM Alert_Master WHERE Last_Updated=0 GROUP BY Last_Updated HAVING COUNT(Last_Updated) = (SELECT COUNT(*) FROM Alert_Master)"
*!*	etsql_con = SqlConObj.dataconn([EXE],Company.Dbname,StrSql,[Alertrec_Vw],"nHandle",_etdatasessionid,.F.)
*!*	If etsql_con >0 And Used("Alertrec_Vw")
*!*		Select Alertrec_Vw
*!*		rCount=Reccount()
*!*		If rCount >0
*!*			StrSql="Execute Usp_Alert_Execute"
*!*			etsql_con = SqlConObj.dataconn([EXE],Company.Dbname,StrSql,[Alertrec_Vw],"nHandle",_etdatasessionid,.F.)
*!*			If etsql_con <=0 
*!*				etsql_con = 0
*!*			Endif
*!*		Endif
*!*	Endif
&& Added by Shrikant S. on 29/01/2019 for Bug-32155			&& End


*!*	StrSql="usp_alert_List '"+Alltrim(musername)+"'"
*!*	etsql_con = SqlConObj.dataconn([EXE],Company.Dbname,StrSql,[tAlert_Vw],"nHandle",_etdatasessionid,.F.)
*!*	If etsql_con >0 And Used("tAlert_Vw")
*!*		Select tAlert_Vw
*!*		rCount=Reccount()
*!*		etsql_con = 0
*!*	Endif
&&<--- Bug-2471 Rup

*!*	pRange = "^21705"
pRange = "^99999"
*!*	If (rCount>0)	
	vicopath=Strtran(icopath,' ','<*#*>')
	pApplCaption=Strtran(VuMess,' ','<*#*>')
	_ShellExec ="udDashboard.exe "+Transform(tcCompId)+" "+Alltrim(tcCompdb)+" "+Alltrim(mvu_server)+" "+Alltrim(mvu_user1)+" "+Alltrim(mvu_pass1)+" "+Alltrim(pRange)+ " "+musername+" "+Alltrim(vicopath)+" "+pApplCaption+" "+pApplName+" "+Alltrim(Str(pApplId))+" "+Alltrim(pApplCode)+" "+"ADMINISTRATOR"+" "+""
*!*			MESSAGEBOX(Transform(tcCompId))
*!*			MESSAGEBOX(Alltrim(tcCompdb))		
*!*			MESSAGEBOX(Alltrim(mvu_server))	
*!*			MESSAGEBOX(Alltrim(mvu_user1))
*!*			MESSAGEBOX(Alltrim(mvu_pass1))
*!*			MESSAGEBOX(Alltrim(pRange))
*!*			MESSAGEBOX(musername)
*!*			MESSAGEBOX(Alltrim(vicopath))
*!*			MESSAGEBOX(pApplCaption)
*!*			MESSAGEBOX(pApplName)
*!*			MESSAGEBOX(Alltrim(Str(pApplId)))
*!*			MESSAGEBOX(Alltrim(pApplCode))
*!*			_ShellExec ="udDashboard.exe "+Transform(tcCompId)+" "+Alltrim(tcCompdb)+" "+Alltrim(mvu_server)+" "+Alltrim(mvu_user1)+" "+Alltrim(mvu_pass1)+ " "+musername+" "+Alltrim(vicopath)+" "+pApplCaption+" "+pApplName+" "+Alltrim(Str(pApplId))  +" "+Alltrim(pApplCode)
*!*		_ShellExec =appNm+" "+Transform(tcCompId)+" "+Alltrim(tcCompdb)+" "+Alltrim(mvu_server)+" "+Alltrim(mvu_user1)+" "+Alltrim(mvu_pass1) +" "+Alltrim(pRange)+" "+Alltrim(musername)+" "+Alltrim(vicopath)+" "+Alltrim(pApplCaption)+" "+Alltrim(pApplName)+" "+Alltrim(Str(pApplId))  +" "+Alltrim(pApplCode)+" "
*!*		oWSHELL.Run(_ShellExec,1,.T.)   &&Commented by Priyanka B on 01092018 for Bug-31746
*!*	MESSAGEBOX("1")
	oWSHELL.Exec(_ShellExec)  &&Modified by Priyanka B on 01092018 for Bug-31746
*!*		MESSAGEBOX("2")
*!*	Endif
SqlConObj = Null
mvu_user1 = Null
mvu_pass1 = Null
Release SqlConObj,mvu_user1,mvu_pass1




&&-------------------------------------------------------------------------------------

*!*	Parameters cApp,vEntryty,vTrancd,pRange
*!*	&&Parameters cApp,vEntryty,vTrancd,vItserial,vAcGrp,pRange
*!*	If Vartype(Company) <> "O"
*!*		Return .F.
*!*	ENDIF
*!*	cApp=Upper(cApp)

*!*	oWSHELL = Createobject("WScript.Shell")
*!*	If Vartype(oWSHELL) <> "O"
*!*		Messagebox("WScript.Shell Object Creation Error...",16,VuMess)
*!*		Return .F.
*!*	Endif

*!*	Declare Integer GetCurrentProcessId  In kernel32

*!*	tcCompId = Company.CompId
*!*	tcCompdb =Company.Dbname
*!*	tcCompNm=Company.co_name
*!*	SqlConObj = Newobject('SqlConnUdObj','SqlConnection',xapps)

*!*	IF mvu_nenc=1
*!*		mvu_user1 =SqlConObj.newdecry(strconv(mvu_user,16),"Ud*yog+1993Uid")
*!*		mvu_pass1 =SqlConObj.newdecry(STRCONV(mvu_pass,16),"Ud*yog+1993Pwd")
*!*	else
*!*		mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
*!*		mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))
*!*	ENDIF


*!*	vicopath=Strtran(icopath,' ','<*#*>')
*!*	pApplCaption=Strtran(VuMess,' ','<*#*>')
*!*	appNm=""



*!*	Do Case
*!*		Case cApp=="DASHBOARD"
*!*			appNm="udDashboard.exe"
*!*	*!*			vEntryty=Strtran(vEntryty,' ','<*#*>')
*!*	*!*			vTrancd=Strtran(vTrancd,' ','<*#*>')

*!*		Otherwise
*!*			appNm=""
*!*	ENDCASE

*!*	If !Empty(appNm)
*!*			_ShellExec =appNm+" "+Transform(tcCompId)+" "+Alltrim(tcCompdb)+" "+Alltrim(mvu_server)+" "+Alltrim(mvu_user1)+" "+Alltrim(mvu_pass1) +" "+Alltrim(pRange)+" "+Alltrim(musername)+" "+Alltrim(vicopath)+" "+Alltrim(pApplCaption)+" "+Alltrim(pApplName)+" "+Alltrim(Str(pApplId))  +" "+Alltrim(pApplCode)+" "
*!*	*!*			_ShellExec =appNm+" "+Transform(tcCompId)+" "+Alltrim(tcCompdb)+" "+Alltrim(mvu_server)+" "+Alltrim(mvu_user1)+" "+Alltrim(mvu_pass1) +" "+Alltrim(pRange)+" "+Alltrim(musername)+" "+Alltrim(vicopath)+" "+Alltrim(pApplCaption)+" "+Alltrim(pApplName)+" "+Alltrim(Str(pApplId))  +" "+Alltrim(pApplCode)+" "+Alltrim(vEntryty)+" "+Alltrim(vTrancd)+"  "
*!*			
*!*				MESSAGEBOX(_ShellExec)
*!*		oWSHELL.Exec(_ShellExec)
*!*	Else
*!*		Messagebox(cApp+" not found",16,VuMess)
*!*	Endif
*!*	SqlConObj = Null
*!*	mvu_user1 = Null
*!*	mvu_pass1 = Null
*!*	Release SqlConObj,mvu_user1,mvu_pass1