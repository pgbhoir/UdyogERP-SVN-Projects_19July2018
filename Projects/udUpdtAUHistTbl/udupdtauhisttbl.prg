_Screen.Visible = .F.

Declare Integer GetPrivateProfileString In Win32API As GetPrivStr ;
	STRING cSection, String cKey, String cDefault, String @cBuffer, ;
	INTEGER nBufferSize, String cINIFile

Local iniFilePath,lcExeName,ueapath,uexapps
Public _mVersion

_mVersion   = ''
ueapath     = Allt(Sys(5) + Curd())
iniFilePath = ueapath+"visudyog.ini"

If !File(iniFilePath)
	Messagebox("Configuration File Not found",16,'Udyog Admin')
	Retu .F.
Endif

mvu_one = Space(2000)
mvu_two = 0
mvu_two	= GetPrivStr([Settings],"XFile", "", @mvu_one, Len(mvu_one), ueapath + "visudyog.ini")
uexapps = Left(mvu_one,mvu_two)

If Vartype(uexapps) <> 'C' Or Empty(uexapps)
	=Messagebox('In Configuration file Xfile Path cannot be empty',16,[Udyog])
	Return .F.
Else
	If !File(uexapps)
		=Messagebox('In Configuration file Specify application file is not found',16,[Udyog])
		Return .F.
	Endif

	lcExeName = Allt(uexapps)
	=Agetfileversion(_VersionArr,uexapps)
	If Type('_VersionArr') = 'C'
		_mVersion = _VersionArr(1,4)
	Endif
	If Occurs('.',_mVersion) = 2
		_mVersion = _mVersion + '.0'
	Endif

	Do &lcExeName With [U]
Endif






Define Class CustSqlConnUdObj As SqlConnUdObj

	Procedure ShowError
		Lparameters pmsg As String,_sqlconhandle,_logUser
		If Type('_ShowErrMsgOrRetVal') != 'L'
			_ShowErrMsgOrRetVal = .F.
		Endif
		_ShowErrMsgVal = ''

		Local mret,merrmsg
		merrmsg = Message()
		mret = SQLExec(&_sqlconhandle,"select @@error as Num", "_Error")
		If !Used("_Error") Or mret <= 0
			Return .F.
		Endif
		Sele _error
		Do Case
			Case _error.num = 547
				If !Empty(pmsg)
					If _ShowErrMsgOrRetVal = .F.
						=Messagebox(pmsg + Chr(13) + Chr(13) + Alltr(merrmsg),64,vumess)
					Else
						_ShowErrMsgVal = pmsg + Chr(13) + Chr(13) + Alltr(merrmsg)
					Endif
				Else
					If _ShowErrMsgOrRetVal = .F.
						=Messagebox("Constraint violation Error" + Chr(13) + Chr(13) + Alltr(merrmsg),64,vumess)
					Else
						_ShowErrMsgVal = "Constraint violation Error" + Chr(13) + Chr(13) + Alltr(merrmsg)
					Endif
				Endif
			Case _error.num = 2714 And 	_logUser = .T.
				If _ShowErrMsgOrRetVal = .F.
					=Messagebox(Alltrim(pmsg),64,vumess)
				Else
					_ShowErrMsgVal = Alltrim(pmsg)
				Endif
			Otherwise
				If !Empty(pmsg)
					pmsg = pmsg + Iif(!Empty(pmsg), Chr(13) + Chr(13),"") + Alltr(merrmsg)
				Else
					pmsg = Alltr(merrmsg)
				Endif
				If _ShowErrMsgOrRetVal = .F.
					=Messagebox(pmsg,64,vumess)
				Else
					_ShowErrMsgVal = Alltrim(pmsg)
				Endif
				If Type('statdesktop')='O'
					statdesktop.progressbar.Visible = .F.
				Endif
		Endcase
		Use
		Return

Enddefine
