Parameters oObject
&&Added by Priyanka B on 13022020 for Bug-33252 Start
_etdatasessionid = oObject.DataSessionId
lnamt = 0
If [efabric] $ vchkprod
	If Inlist(main_vw.entry_ty,'AF','BS','DF','FO','FS','I6','I7','I8','IC','ID','O6','OC') Or Inlist(main_vw.entry_ty,'P6','PY','R2','R3','R4','R5','RB','RD','S3','S4','SG','W1','W2','W3','W4')   &&Added by Priyanka B on 02062020 for Bug-33448
		If Type('item_vw.fRatePer') <> 'U'
			If item_vw.fRatePer = "MTR"
				lnamt =item_vw.qty * item_vw.rate
			Else
				lnamt =item_vw.fpcs * item_vw.rate
			Endif
			Replace item_vw.u_asseamt With lnamt In item_vw
		Endif
	Endif   &&Added by Priyanka B on 02062020 for Bug-33448
Endif
&&Added by Priyanka B on 13022020 for Bug-33252 End

Return lnamt
