Lparameters tnrange As Integer
*!*	PARAMETERS vDataSessionId

If Vartype(VuMess) <> 'C'
	Messagebox('Internal Application Not Run Directly...',0+48,[])
	Quit
	Return .F.
Endif

Do Form frmgstr9report With tnrange
