*:*****************************************************************************
*:        Program: Main
*:         System: Udyog Software
*:         Author: Birendra Prasad
*:  Last modified: 
*:			AIM  : Call Cost centre master exe
*:*****************************************************************************
PARAMETERS pRange
IF VARTYPE(Company) <> "O"
	RETURN .F.
ENDIF
*!*	IF PARAMETERS() < 1
*!*		=MESSAGEBOX('Pass Valid Parameters',0,"")
*!*		RETURN .T.
*!*	ENDIF

oWSHELL = CREATEOBJECT("WScript.Shell")
IF VARTYPE(oWSHELL) <> "O"
	MESSAGEBOX("WScript.Shell Object Creation Error...",16,VuMess)
	RETURN .F.
ENDIF

DECLARE INTEGER GetCurrentProcessId  IN kernel32
pid=GetCurrentProcessId()
tcCompId = Company.CompId
tcCompdb =Company.Dbname
tcCompNm=Company.co_name
SqlConObj = NEWOBJECT('SqlConnUdObj','SqlConnection',xapps)
*!*	mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))		&& Commented by Shrikant S. on 26/04/2018 for Bug-31488
*!*	mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))		&& Commented by Shrikant S. on 26/04/2018 for Bug-31488

&& Added by Shrikant S. on 26/04/2018 for Bug-31488		&& Start
IF mvu_nenc=1
	mvu_user1 =SqlConObj.newdecry(strconv(mvu_user,16),"Ud*yog+1993Uid")
	mvu_pass1 =SqlConObj.newdecry(STRCONV(mvu_pass,16),"Ud*yog+1993Pwd")
else
	mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
	mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))
ENDIF
&& Added by Shrikant S. on 26/04/2018 for Bug-31488		&& End


vicopath=STRTRAN(icopath,' ','<*#*>')
pApplCaption=STRTRAN(vumess,' ','<*#*>')
*!*	pApplName=justfname(SYS(16))
*!*	_ShellExec ="udDynamicMaster.exe "+TRANSFORM(tcCompId)+" "+ALLTRIM(tcCompdb)+" "+ALLTRIM(mvu_server)+" "+ALLTRIM(mvu_user1)+" "+ALLTRIM(mvu_pass1)+" "+pRange+" "+alltrim(MastCode)+" "+musername+" "+ALLTRIM(vicopath)+" "+pApplCaption+" "+pApplName+" "+ALLTRIM(STR(pid))+" "+rExtAppTbl 
*_ShellExec ="UeCost_Cen_master.exe "
_ShellExec ="UeCost_Cen_master.exe "+TRANSFORM(tcCompId)+" "+ALLTRIM(tcCompdb)+" "+ALLTRIM(mvu_server)+" "+ALLTRIM(mvu_user1)+" "+ALLTRIM(mvu_pass1)+" "+pRange+" "+"DEG"+" "+musername +" "+ALLTRIM(vicopath)+" "+pApplCaption+" "+pApplName+" "+ALLTRIM(STR(pid))+" "+pApplCode

oWSHELL.EXEC(_ShellExec)
SqlConObj = NULL
mvu_user1 = NULL
mvu_pass1 = NULL
RELEASE SqlConObj,mvu_user1,mvu_pass1
