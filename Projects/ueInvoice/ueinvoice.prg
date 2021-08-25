Lparameters Para1,Para2,Para3,Para4,Para5,Para6,Para7,Para8,Para9,Para10,Para11,Para12,Para13,Para14,Para15
_LParaCnt = Parameters()
Set Date To BRITISH
_LError	= .F.
_Lparas = ''

_curform = _Screen.ActiveForm
_etdatasessionid = _curform.DataSessionId
_curform.nhandle=0

vE_INV_NO=mainadd_vw.E_INV_NO
If(!Empty(vE_INV_NO))
	Messagebox("E Invoice No. already exist...",16,VuMess)
	Return .F.
Endif


If Vartype(Company) <> "O"
	Return .F.
Endif
cApp="EINVOICE_GEN"
oWSHELL = Createobject("WScript.Shell")
If Vartype(oWSHELL) <> "O"
	Return .F.
Endif

Declare Integer GetCurrentProcessId  In kernel32

tcCompId = Company.CompId
tcCompdb =Company.Dbname
tcCompNm=Company.co_name
SqlConObj = Newobject('SqlConnUdObj','SqlConnection',xapps)

If mvu_nenc=1
	mvu_user1 =SqlConObj.newdecry(Strconv(mvu_user,16),"Ud*yog+1993Uid")
	mvu_pass1 =SqlConObj.newdecry(Strconv(mvu_pass,16),"Ud*yog+1993Pwd")
Else
	mvu_user1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_user))
	mvu_pass1 = SqlConObj.dec(SqlConObj.ondecrypt(mvu_pass))
Endif

vicopath=Strtran(icopath,' ','<*#*>')
pApplCaption=Strtran(VuMess,' ','<*#*>')
appNm=""
vGSTIN=Company.GSTIN
vAc_Id=Alltrim(Str(main_vw.ac_id))
vEntry_ty=Alltrim(main_vw.Entry_ty)
vTran_cd=Alltrim(Str(main_vw.Tran_cd))
vTimeOut="15"
Do Case
	Case cApp=="EINVOICE_GEN"
		appNm="udAdqGSPHelper.exe"
	Otherwise
		appNm=""
Endcase
If !Empty(appNm)

	_ShellExec =appNm+" "+Transform(tcCompId)+" "+Alltrim(tcCompdb)+" "+Alltrim(mvu_server)+" "+Alltrim(mvu_user1)+" "+Alltrim(mvu_pass1) +" "+Alltrim(vGSTIN)+" "+Alltrim(vAc_Id)+" "+Alltrim(cApp)+" "+Alltrim(musername)+" "+Alltrim(pApplCaption)+" "+Alltrim("null")+" "+Alltrim(vTran_cd)+" "+Alltrim(vEntry_ty)+""
	oWSHELL.Exec(_ShellExec)
Else
	Messagebox(cApp+" not found",16,VuMess)
Endif

Wait "E Invoice No. process...." Window  Timeout 15

vSq1Str= "select E_inv_no from stmainadd where ENTRY_TY=?main_vw.Entry_ty and TRAN_CD=?main_vw.Tran_cd"
nretval =_curform.SqlConObj.dataconn([EXE],Company.Dbname,vSq1Str,"curInvNo","_curform.nHandle",_curform.DataSessionId)
If nretval<=0
	Messagebox("Unable to get E Invoice Details from Database..!!",16,VuMess)
	Return .F.
Endif
Messagebox('E Invoice No. generated successfully....',0+64,VuMess)
Select curInvNo

Replace mainadd_vw.E_INV_NO With curInvNo.E_INV_NO  In mainadd_vw
_Screen.ActiveForm.Refresh



