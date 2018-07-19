IF 'trnamend' $ vChkprod
	DO VouBefEdit IN MainPrg
ENDIF 

&& Added by Shrikant S. on 27/06/2014 for Bug-23280		&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('pharmaind')
		_curvouobj = _Screen.ActiveForm
		If _curvouobj.itempage Or Inlist(main_vw.entry_ty,"AR","OS")
			If !Used('BatchTbl_Vw')
				etsql_str = "Select * From BatchGenTbl Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[BatchTbl_Vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con < 1 Or !Used("BatchTbl_Vw")
					etsql_con = 0
				Else
					Select BatchTbl_Vw
					Index On itserial Tag itserial
				Endif
			Endif
		Endif

		If _curvouobj.itempage Or Inlist(main_vw.entry_ty,"WK")
			If !Used('wkrmdet_vw')
				etsql_str = "Select * From WKRMDET Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[wkrmdet_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con < 1 Or !Used("wkrmdet_vw")
					etsql_con = 0
				Endif
			Endif
		Endif
	Endif
Endif
&& Added by Shrikant S. on 27/06/2014 for Bug-23280		&& End

&& Added by Sachin N. S. on 30/10/2017 for Bug-30782 -- Start
If oGlblPrdFeat.UdChkProd('vugst')
	cdAmendDt = Iif(Type('Main_vw.AmendDate')='T','Main_vw.AmendDate',Iif(Type('Lmc_vw.AmendDate')='T','Lmc_vw.AmendDate',Iif(Type('MainAdd_vw.AmendDate')='T','MainAdd_vw.AmendDate','')))
	If !Empty(cdAmendDt)
		If !Empty(Evaluate(cdAmendDt)) AND Evaluate(cdAmendDt)!=CTOD('01/01/1900')
			=Messagebox("This record is already Amended cannot change...!!!",0+16,vumess)
			Return .F.
		Endif
	Endif
Endif
&& Added by Sachin N. S. on 30/10/2017 for Bug-30782 -- End
