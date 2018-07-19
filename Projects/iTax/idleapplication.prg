*============================================================
* Developed by Uday on dated 05/03/2014 - Bug ID 21693
* This class detects the activitiy, due to inactivity if application will idle
* then message will be popup and prompt user to continue session or close session
*============================================================

Define Class IdleApplication As InactivityTimer
	Procedure Init(tnTimeOutInMinutes,tnTimeInterval)
	DoDefault(tnTimeOutInMinutes,tnTimeInterval)

	Procedure eventTimeout
	ans = Messagebox([Due of inactivity, your session is about to expire, if you are still using this application,] + Chr(13) +;
		[Please click "OK" to continue the session otherwise your session will expire within 2 min. and you will need to restart application again];		&& Added by Shrikant S. on 07/06/2018 for Bug-31413
		,0+64+0,"Session timeout",120000)																													&& Added by Shrikant S. on 07/06/2018 for Bug-31413
		
*!*			[Please click "OK" otherwise your session will expire within 2 min. and you will need to restart application again],1+64+256,"Session timeout",120000)	&& Commented by Shrikant S. on 07/06/2018 for Bug-31413

*!*			If ans = 2 Or ans = -1															&& Commented by Shrikant S. on 28/05/2018 for Bug-31413
*!*				statdesktop.Message = "Application will be Shutdown within 2 min........"	&& Commented by Shrikant S. on 28/05/2018 for Bug-31413
	If ans = -1			&& Added by Shrikant S. on 28/05/2018 for Bug-31413
		statdesktop.Message = "Application is shutting down........"
*!*			shutOffTimer.Enabled = .T.										&& Commented by Shrikant S. on 28/05/2018 for Bug-31413

		&& Added by Shrikant S. on 28/05/2018 for Bug-31413			&& Start
		exitclick = .T.
		applicationshutoff.appshutoff
		If Type('GlobalObj') = 'O'
			Local m_LicenseServiceValidate
			m_LicenseServiceValidate = ''
			If Type('pemstatus(GlobalObj,"LicenseClose",3)') = 'C'	&&vasant16/11/2010	Changes done for VU 10 (Standard/Professional/Enterprise)
				m_LicenseServiceValidate = GlobalObj.LicenseClose()
			Endif
			If Type('m_LicenseServiceValidate') <> 'C'
				m_LicenseServiceValidate = ''
			Endif
			Release m_LicenseServiceValidate
		Endif
&& Added by Shrikant S. on 28/05/2018 for Bug-31413			&& End

	Endif
Enddefine

*============================================================
* If user has click on cancel, this timer will be activated and shutoff application will be 2 minutes.
*============================================================

Define Class shutOffTimer As Timer
	Interval = 120000
	Enabled = .F.


	Procedure Timer
	This.eventShutTimeout()
	Endproc

	Procedure eventShutTimeout
	exitclick = .T.
	applicationshutoff.appshutoff
	Endproc
Enddefine

*============================================================
* Detects user activity and fires an event after the
* specified period of inactivity.
*============================================================
Define Class InactivityTimer As Timer

*----------------------------------------------------------
* API constants
*----------------------------------------------------------
	#Define WM_KEYUP                        0x0101
	#Define WM_SYSKEYUP                     0x0105
	#Define WM_MOUSEMOVE                    0x0200
	#Define GWL_WNDPROC         (-4)

*----------------------------------------------------------
* internal properties
*----------------------------------------------------------
	nTimeOutInMinutes = 0
	tLastActivity = {/:}
	nOldProc = 0

*----------------------------------------------------------
* Timer configuration
*----------------------------------------------------------
* Interval = 30
	Enabled = .T.

*------------------------------------------------------------
* Listen to API events when the form starts. You can pass
* the timeout as a parameter.
*------------------------------------------------------------
	Procedure Init(tnTimeOutInMinutes,tnTimeInterval)
	Declare Integer GetWindowLong In WIN32API ;
		integer HWnd, ;
		integer nIndex
	Declare Integer CallWindowProc In WIN32API ;
		integer lpPrevWndFunc, ;
		integer HWnd,Integer Msg,;
		integer wParam,;
		integer Lparam
	This.Interval = tnTimeInterval
	This.nOldProc=GetWindowLong(_vfp.HWnd,GWL_WNDPROC)
	If Vartype(m.tnTimeOutInMinutes) == "N"
		This.nTimeOutInMinutes = m.tnTimeOutInMinutes
	Endif
	This.tLastActivity = Datetime()
	Bindevent(0,WM_KEYUP,This,"WndProc")
	Bindevent(0,WM_MOUSEMOVE,This,"WndProc")
	Endproc

*------------------------------------------------------------
* Stop listening
*------------------------------------------------------------
	Procedure Unload
	Unbindevents(0,WM_KEYUP)
	Unbindevents(0,WM_MOUSEMOVE)
	Endproc

*------------------------------------------------------------
* Every event counts as activity
*------------------------------------------------------------
	Procedure WndProc( ;
		hWnd As Long,Msg As Long,wParam As Long,Lparam As Long )
	This.tLastActivity = Datetime()
	_Screen.Caption = Str(Val(_Screen.Caption)+1)
*statdesktop.MESSAGE = statdesktop.MESSAGE + " Last Activity :" + DTOC(DATE()) + " " + TIME()
	Return CallWindowProc(This.nOldProc,HWnd,Msg,wParam,Lparam)

*------------------------------------------------------------
* Check last activity against time out
*------------------------------------------------------------


	Procedure Timer
	Local ltFireEvent
	ltFireEvent = This.tLastActivity + 60*This.nTimeOutInMinutes
	If Datetime() > m.ltFireEvent
		This.eventTimeout()
	Endif
	Endproc

*------------------------------------------------------------
* Override this event or bind to it to respond to user
* inactivity. You can change the nTimeOutInMinutes to offer
* multiple stages of timeouts.
*------------------------------------------------------------
	Procedure eventTimeout

Enddefine
