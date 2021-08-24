curObj=_Screen.ActiveForm
If curObj.addmode Or curObj.editmode
	If Empty(curObj.cwhat)
		Select it_mast_vw
		If Type('it_mast_vw.APIEXPORT')<>'U'
			Replace APIEXPORT With {} In it_mast_vw
		Endif

		If Type('it_mast_vw.APIIMPORT')<>'U'
			Replace APIIMPORT With {} In it_mast_vw
		Endif

	Endif
Endif
