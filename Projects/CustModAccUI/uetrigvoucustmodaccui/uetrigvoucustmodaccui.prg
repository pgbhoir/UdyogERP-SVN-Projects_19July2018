Parameters nRange &&,nIcoPath,nVumess

Declare Integer ShellExecute In shell32.Dll;
	INTEGER hndwin,;
	STRING cAction,;
	STRING cFileName,;
	STRING cParams,;
	STRING cDir,;
	INTEGER nShowWin

cAction = "open"
cFileName = APath + "CustModAccUI2.UI.exe"
*!*	Messagebox(APath)
*!*	Messagebox(icopath)
*!*	Messagebox(vumess)
nRange = Substr(nRange,2,Len(nRange))
cParams = '"'+Alltrim(Company.dbname)+'" "'+Alltrim(mUserName)+'" "'+Alltrim(APath)+'\" '+Alltrim(nRange)+' "'+alltrim(icopath)+'" "'+alltrim(vumess)+'"'
*!*	cParams = '"'+Alltrim(nDbname)+'" "'+Alltrim(nUserName)+'" '+Alltrim(nRange)
*!*	Messagebox(cParams)
cDir = ""
mRtrn = ShellExecute(0,cAction,cFileName,cParams,cDir,1)
Do Case
Case mRtrn = 2
	mMsg = "Bad Association"
Case mRtrn = 29
	mMsg = "Failure to load appication"
Case mRtrn = 30
	mMsg = "Application is busy"
Case mRtrn = 31
	mMsg = "No application association"
Otherwise
	mMsg = ""
Endcase

If !Empty(mMsg)
	Messagebox(mMsg,48,vumess)
Endif

Clear Dlls ShellExecute
