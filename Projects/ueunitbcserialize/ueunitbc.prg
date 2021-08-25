Lparameters _cEntTyp,_nDataSession,_actform


If Type('Lcode_vw.unitbc')='L'
	If Lcode_vw.unitbc=.T.
		If !(UPPER('ueunitbcserialize') $ UPPER(Set("Classlib")))
			Set Classlib To ueunitbcserialize Additive
		ENDIF
		If !('Vouclass' $ Set("Classlib"))
			Set Classlib To Vouclass Additive
		Endif
		
		Do Form uefrmunitbc WITH _screen.ActiveForm
*		Read Events
	Endif

Endif
