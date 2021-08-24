*:*****************************************************************************
*:        Program: Main
*:         System: Udyog Software
*:         Author: Rupesh
*:  Last modified:
*:			AIM  : Call Item Master Rate Update
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
StrSql="usp_alert_List '"+Alltrim(musername)+"'"
etsql_con = SqlConObj.dataconn([EXE],Company.Dbname,StrSql,[tAlert_Vw],"nHandle",_etdatasessionid,.F.)
If etsql_con >0 And Used("tAlert_Vw")
	Select tAlert_Vw
	rCount=Reccount()
	etsql_con = 0
Endif
&&<--- Bug-2471 Rup


If (rCount>0)
	vicopath=Strtran(icopath,' ','<*#*>')
	pApplCaption=Strtran(VuMess,' ','<*#*>')
	_ShellExec ="UdAlert.exe "+Transform(tcCompId)+" "+Alltrim(tcCompdb)+" "+Alltrim(mvu_server)+" "+Alltrim(mvu_user1)+" "+Alltrim(mvu_pass1)+" "+musername+" "+Alltrim(vicopath)+" "+pApplCaption+" "+pApplName+" "+Alltrim(Str(pApplId))  +" "+Alltrim(pApplCode)++" "+Alltrim(mvu_User_Roles)
*!*		oWSHELL.Run(_ShellExec,1,.T.)   &&Commented by Priyanka B on 01092018 for Bug-31746
	oWSHELL.Exec(_ShellExec)  &&Modified by Priyanka B on 01092018 for Bug-31746
Endif
SqlConObj = Null
mvu_user1 = Null
mvu_pass1 = Null
Release SqlConObj,mvu_user1,mvu_pass1


