Parameters _lnDataSession,_addMode,_editMode,_actform

_LParaCnt = Parameters()

If _LParaCnt=4
	If _addMode =.T. Or _editMode=.T.
		Do Form frmcsvimp With _lnDataSession,_addMode,_editMode,_actform
	ENDIF
ELSE
	DO FORM frmfldmap WITH _lnDataSession
Endif