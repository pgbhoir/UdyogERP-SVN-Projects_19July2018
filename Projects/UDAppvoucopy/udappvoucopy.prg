Lparameters Lparm
If Upper(Lparm) = "AFTER"
	_curvouobj = _Screen.ActiveForm
	Set DataSession To _curvouobj.DataSessionId
	Set DataSession To _Screen.ActiveForm.DataSessionId
	If Type('_curvouobj.PcvType') = 'C' And Used('MAIN_VW')
		If _curvouobj.EditMode = .T. Or  _curvouobj.AddMode = .T.
			If Type("MAIN_VW.AMENDDATE") = "T"
				Replace MAIN_VW.AMENDDATE With {//} In MAIN_VW
			Endif
&&Added by Priyanka B on 07032018 for Bug-31207 Start
			If Type("MAIN_VW.U_ITCDATE") = "T"
				Replace MAIN_VW.U_ITCDATE With {//} In MAIN_VW
			Endif
&&Added by Priyanka B on 07032018 for Bug-31207 End
		Endif
	Endif
Endif
