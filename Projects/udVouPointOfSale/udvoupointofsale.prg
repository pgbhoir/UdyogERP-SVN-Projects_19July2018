Lparameters _nDataSession

If Vartype(oGlblPrdFeat)='O'
*!*		If oGlblPrdFeat.UdChkProd('pos')
	If oGlblPrdFeat.UdChkProd('pos') OR oGlblPrdFeat.UdChkProd('phrmretlr')		&& Changed by Sachin N. S. on 16/11/2018 for Bug-31943
		If Type('_screen.ActiveForm.pcvType')='C'
*!*				If _Screen.ActiveForm.pcvType = 'PS'
			If INLIST(_Screen.ActiveForm.pcvType,'PS','HS')				&& Changed by Sachin N. S. on 16/11/2018 for Bug-31943
				If !('udClsPointOfSale' $ Set("Classlib"))
					Set Classlib To udClsPointOfSale Additive
				Endif
				If !('Vouclass' $ Set("Classlib"))
					Set Classlib To Vouclass Additive
				Endif
			Endif
		Endif
	Endif
Endif
