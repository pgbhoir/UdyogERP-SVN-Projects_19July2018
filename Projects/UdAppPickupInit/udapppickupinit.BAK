Lparameters oFrmpara

&&Added by Priyanka B on 04122018 for Bug-31930 Start
oFrm=_Screen.ActiveForm
If (Inlist(main_vw.entry_ty,"PU") And ("phrmretlr" $ vchkprod))
	If Type('main_vw.ISEXPDSTK')="L" And main_vw.ISEXPDSTK=.T.
		If oFrm.Addmode Or oFrm.Editmode
			Select Detail
			cFiltDet=Set("Filter")
			Set Filter To
			Delete From Detail Where expdt>(Ttod(main_vw.Date)+main_vw.STKEXPGDAY) And ticked=.F.
			Create Cursor curDeleted (mentry_ty Varchar(2),mtran_cd Int)

			Set Deleted Off
			Select Detail
			Scan For Deleted()=.T.
				centry_ty=Detail.entry_ty
				ctran_cd=Detail.tran_cd
				Select entry_ty,tran_cd From Detail Where entry_ty=centry_ty And tran_cd=ctran_cd And Deleted()=.F. Into Cursor curRecord
				If Reccount("curRecord")<=0
					Append Blank In curDeleted
					Replace mentry_ty With Detail.entry_ty,mtran_cd With Detail.tran_cd In curDeleted
				Endif
			Endscan
			Set Deleted On

			Select curDeleted
			Scan
				Delete From Header Where Header.entry_ty = curDeleted.mentry_ty And Header.tran_cd = curDeleted.mtran_cd
			Endscan

			Select Detail
			If !Empty(cFiltDet)
				Set Filter To &cFiltDet
			Endif
		Endif
	Endif
	If Used("curDeleted")
		Use In curDeleted
	Endif
	If Used("curRecord")
		Use In curRecord
	Endif
Endif
&&Added by Priyanka B on 04122018 for Bug-31930 End
