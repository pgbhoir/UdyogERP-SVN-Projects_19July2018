
Parameters cApp,vEntryty,vTrancd,pRange
&&Parameters cApp,vEntryty,vTrancd,vItserial,vAcGrp,pRange
If Vartype(Company) <> "O"
	Return .F.
ENDIF
cApp=Upper(cApp)

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

IF mvu_nenc=1
	mvu_user1 =SqlConObj.newdecry(strconv(mvu_user,16),"Ud*yog+1993Uid")
	mvu_pass1 =SqlConObj.newdecry(STRCONV(mvu_pass,16),"Ud*yog+1993Pwd")
else
	mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
	mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))
ENDIF


vicopath=Strtran(icopath,' ','<*#*>')
pApplCaption=Strtran(VuMess,' ','<*#*>')
appNm=""



Do Case
	Case cApp=="BARCODEPRINTING"
		appNm="udBarcodeDataTran.exe"
		vEntryty=Strtran(vEntryty,' ','<*#*>')
		vTrancd=Strtran(vTrancd,' ','<*#*>')

	Otherwise
		appNm=""
ENDCASE

If !Empty(appNm)
*!*		_ShellExec =appNm+" "+Transform(tcCompId)+" "+Alltrim(tcCompdb)+" "+Alltrim(mvu_server)+" "+Alltrim(mvu_user1)+" "+Alltrim(mvu_pass1) +" "+Alltrim(pRange)+" "+Alltrim(musername)+" "+Alltrim(vicopath)+" "+Alltrim(pApplCaption)+" "+Alltrim(pApplName)+" "+Alltrim(Str(pApplId))  +" "+Alltrim(pApplCode)+" "
		_ShellExec =appNm+" "+Transform(tcCompId)+" "+Alltrim(tcCompdb)+" "+Alltrim(mvu_server)+" "+Alltrim(mvu_user1)+" "+Alltrim(mvu_pass1) +" "+Alltrim(pRange)+" "+Alltrim(musername)+" "+Alltrim(vicopath)+" "+Alltrim(pApplCaption)+" "+Alltrim(pApplName)+" "+Alltrim(Str(pApplId))  +" "+Alltrim(pApplCode)+" "+Alltrim(vEntryty)+" "+Alltrim(vTrancd)+"  "
		
*!*			MESSAGEBOX(_ShellExec)
	oWSHELL.Exec(_ShellExec)
Else
	Messagebox(cApp+" not found",16,VuMess)
Endif
SqlConObj = Null
mvu_user1 = Null
mvu_pass1 = Null
Release SqlConObj,mvu_user1,mvu_pass1