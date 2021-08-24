*:********************************************************************************************************************
*:        Program: Main
*:         System: Udyog Software
*:		 Added by: Vasant on 27/04/12
*:		  Purpose: To call any external program of USquare/VU10 Products
*:		Parameter details : 1st Paramets is EXE Name,last parameter is Range,others will be as per user requirement
*:********************************************************************************************************************
Lparameters Para1,Para2,Para3,Para4,Para5,Para6,Para7,Para8,Para9,Para10,Para11,Para12,Para13,Para14,Para15
_LParaCnt = Parameters()
Set Date To BRITISH		&& Added by Sachin N. S. on 31/08/2013 for Bug-14538
_LError	= .F.
_Lparas = ''
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
tcCompdb = Company.Dbname
tcCompNm = Company.co_name

SqlConObj = Newobject('SqlConnUdObj','SqlConnection',xapps)
*!*	mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))		&& Commented by Shrikant S. on 26/04/2018 for Bug-31488
*!*	mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))		&& Commented by Shrikant S. on 26/04/2018 for Bug-31488

&& Added by Shrikant S. on 26/04/2018 for Bug-31488		&& Start
If mvu_nenc=1
	mvu_user1 =SqlConObj.newdecry(Strconv(mvu_user,16),"Ud*yog+1993Uid")
	mvu_pass1 =SqlConObj.newdecry(Strconv(mvu_pass,16),"Ud*yog+1993Pwd")
Else
	mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
	mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))
Endif
&& Added by Shrikant S. on 26/04/2018 for Bug-31488		&& End


vicopath  =Strtran(icopath,' ','<*#*>')
pApplCaption=Strtran(VuMess,' ','<*#*>')
_prodcode 	= ''
_prodcode1 	= ''

_PassRoute1 = Alltrim(Company.PassRoute)
For i = 1 To Len(_PassRoute1)
	_prodcode1 = _prodcode1 + Chr(Asc(Substr(_PassRoute1,i,1))/2)
Next i

*!*	For i = 1 To Len(_PassRoute1) STEP 5
*!*		_prodcode = _prodcode + SUBSTR(_prodcode1,i,5) +','
*!*	Endfor

_PassRoute1 = Alltrim(Company.PassRoute1)
For i = 1 To Len(_PassRoute1)
	_prodcode = _prodcode + Chr(Asc(Substr(_PassRoute1,i,1))/2)
Next i
_prodcode = _prodcode+','

_prodcode = vChkProd		&& Added by Sachin N. S. on 04/09/2019 for AU 2.2.1

appNm		=""
pRange		=""
For i1 = 1 To _LParaCnt
	_ParaVal = Evaluate('Para'+Alltrim(Str(i1)))
	Do Case
		Case i1 = 1
			If Type('_ParaVal') = 'C'
				appNm = _ParaVal
			Endif
		Case i1 = _LParaCnt		
			If Type('_ParaVal') = 'C'
				pRange = _ParaVal
			Endif
		Otherwise
			_Lparas	= _Lparas+" "+Strtran(Transform(_ParaVal),' ','<*#*>')
	Endcase
Endfor

If Empty(appNm)
	=Messagebox("Check first parameter of the program",16,VuMess)
	_LError	= .T.
Endif
If Empty(pRange)
	=Messagebox("Check last parameter of the program",16,VuMess)
	_LError	= .T.
Endif

***** Added by Sachin N. S. on 04/10/2019 for Bug-32814 -- Start
_varname1 = '_ProdCode'+Sys(3)
&_varname1 = dec(NewDecry(GlobalObj.getPropertyVal("UdProdCode"),'Ud*yog+1993'))
***** Added by Sachin N. S. on 04/10/2019 for Bug-32814 -- End

If _LError	= .F.
	Try
		_ShellExec = appNm+" "+Transform(tcCompId)+" "+Alltrim(tcCompdb)
		_ShellExec = _ShellExec+" "+Alltrim(mvu_server)+" "+Alltrim(mvu_user1)+" "+Alltrim(mvu_pass1)
		_ShellExec = _ShellExec+" "+Alltrim(pRange)+" "+Alltrim(musername)+" "+Alltrim(vicopath)+" "+Alltrim(pApplCaption)
		_ShellExec = _ShellExec+" "+Alltrim(pApplName)+" "+Alltrim(Str(pApplId))  +" "+Alltrim(pApplCode)+" "+_prodcode+" "+EVALUATE(_varname1)+" "+_Lparas		&& Changed by Sachin N. S. on 11/03/2020 for Bug-32814
		oWSHELL.Exec(_ShellExec)
		
	Catch To _ErrMsg
		=Messagebox(_ErrMsg.Message,16,VuMess)
	Endtry
Endif

SqlConObj = Null
mvu_user1 = Null
mvu_pass1 = Null
_prodcode = Null
Release SqlConObj,mvu_user1,mvu_pass1
