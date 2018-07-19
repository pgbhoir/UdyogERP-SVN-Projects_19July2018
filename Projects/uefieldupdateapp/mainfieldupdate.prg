Parameter modulname, prange

If Vartype(company) <> "O"
	Return .F.
Endif
owshell = Createobject("WScript.Shell")
If Vartype(owshell) <> "O"
	Messagebox( ;
		"WScript.Shell Object Creation Error...",  ;
		16, vumess)
	Return .F.
Endif
Declare Integer GetCurrentProcessId In 	kernel32

tccompid = company.compid
tccompdb = company.dbname
tccompnm = company.co_name
sqlconobj = Newobject('SqlConnUdObj', 'SqlConnection', xapps)

*!*	mvu_user1 = sqlconobj.dec(sqlconobj.ondecrypt(mvu_user))		&& Commented by Shrikant S. on 26/04/2018 for Bug-31488
*!*	mvu_pass1 = sqlconobj.dec(sqlconobj.ondecrypt(mvu_pass))		&& Commented by Shrikant S. on 26/04/2018 for Bug-31488

&& Added by Shrikant S. on 26/04/2018 for Bug-31488		&& Start
IF mvu_nenc=1
	mvu_user1 =SqlConObj.newdecry(strconv(mvu_user,16),"Ud*yog+1993Uid")
	mvu_pass1 =SqlConObj.newdecry(STRCONV(mvu_pass,16),"Ud*yog+1993Pwd")
else
	mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
	mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))
ENDIF
&& Added by Shrikant S. on 26/04/2018 for Bug-31488		&& End


vicopath = Strtran(icopath, ' ','<*#*>')
papplcaption = Strtran(vumess,' ','<*#*>')
modulname = Strtran(modulname,' ','<*#*>')
_shellexec = "ueTblFields.exe " + TRANSFORM(tccompid)+" "+ ALLTRIM(tccompdb) + " " +ALLTRIM(mvu_server) +" " +ALLTRIM(mvu_user1)+" " +ALLTRIM(mvu_pass1) +" " + musername +	" " +  ;
	ALLTRIM(vicopath) + " " + papplcaption + " " + papplname +" " +ALLTRIM(Str(papplid)) +" " +	ALLTRIM(papplcode) +" " +ALLTRIM(mvu_user_roles) + " " + prange + " " +	ALLTRIM(modulname)
	
owshell.Exec(_shellexec)
sqlconobj = null
mvu_user1 = null
mvu_pass1 = null
Release sqlconobj, mvu_user1,mvu_pass1

