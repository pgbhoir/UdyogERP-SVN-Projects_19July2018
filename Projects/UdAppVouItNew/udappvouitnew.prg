
&&--->TKT-8320 GTA
If Inlist(.pcVtype,'IF','OF') &&And (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
	Replace qty With 1 In item_vw
Endif
&&<---TKT-8320 GTA

&& Added by Shrikant S. on 21/06/2012 for Bug-4744		&& Start
If Vartype(oGlblPrdFeat)='O'
	If oGlblPrdFeat.UdChkProd('openord')
		If Type('lcode_vw.allowzeroqty')!='U'
			If Inlist(.pcVtype,'OO') And Lcode_vw.allowzeroqty=.T.
				Replace qty With 1 In item_vw
			Endif
		Endif
	Endif
Endif
&& Added by Shrikant S. on 21/06/2012 for Bug-4744		&& End

*Birendra : TKT-9363, on 19/09/2011 for Auto Debit/Credit Note :Start:
If Inlist(.pcVtype,'D2','D3','D4','D5','C2','C3','C4','C5') &&,'CI')
	_curfrmobj = _Screen.ActiveForm

	nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,"select it_code from it_mast where it_name='Interest'",[tmp_itcode],;
		"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.F.)
	If nRet <= 0
		Return .F.
	Endif

	Select item_vw
	Replace item_vw.Item With 'Interest' In item_vw
	Replace item_vw.qty With 1 In item_vw
	Replace item_vw.it_code With tmp_itcode.it_code In item_vw
	If Used('tmp_itcode')
		Use In tmp_itcode
	Endif
	Select item_vw
Endif
*Birendra : TKT-9363, on 19/09/2011 for Auto Debit/Credit Note :End:

&& Added by Shrikant S. on 28/06/2014 	for Bug-23280		&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('pharmaind')
		If Type('.pcvtype')='C'
			If Inlist(.pcVtype,"WK")
				_malias=Alias()
				_mrecno=Iif(!Eof(),Recno(),0)

				Select item_vw
				itemRec=Iif(!Eof(),Recno(),0)
				Count For !Deleted() To itemcnt

				If itemcnt>1
					Select item_vw
					If itemRec>0
						Go itemRec
						Messagebox("Only one Finished/Semi Finished item is allowed.",0,vumess)
						Delete In item_vw
					Endif
					If !Empty(_malias)
						Select &_malias
						Go _mrecno
					Endif
					Return .F.
				Endif

			Endif
		Endif
	Endif
Endif
&& Added by Shrikant S. on 28/06/2014 	for Bug-23280		&& End

&& Added by Shrikant S. on 21/04/2015 for bug-25878		&& Start

If Not('vutex' $ vchkprod)
&&Added by Priyanka B on 01122018 for Pharma Retails Installer Start
	If Inlist(main_vw.Entry_ty,'PK','SK')
		If Empty(item_vw.Item)
			_curfrmobj = _Screen.ActiveForm

			SQLQuery="select top 1 RTRIM(ware_nm) as Ware_nm from warehouse where rtrim(ltrim(ware_nm)) != '' and validity like '%"+main_vw.Entry_ty+"%'";
				+" and (actdeact=0 or (actdeact=1 and cast(deactivefrom as date) > ?main_vw.date)) order by case when ware_nm='Common Store' then 'a' else 'b' end"
			nRet = _curfrmobj.sqlconobj.DataConn([EXE],Company.DbName,SQLQuery,[tmp_warenm],"_curfrmobj.nHandle",_curfrmobj.DataSessionId,.F.)
			If nRet <= 0
				Return .F.
			Endif
			Select tmp_warenm
			If Reccount("tmp_warenm")>0
				Replace ware_nm With tmp_warenm.ware_nm In item_vw
			Endif
			Use In tmp_warenm
		Endif
	Endif
&&Added by Priyanka B on 01122018 for Pharma Retails Installer End

*!*		If Type('lmc_vw.fware')<>'U' Or Type('main_vw.fware')<>'U'		&& Commented  by Shrikant S. on 24/04/2017 for GST
&&Commented by Priyanka B on 16102018 for Bug-31944 & Bug-31920 Start
*!*		If Type('lmc_vw.fware')<>'U' And Type('main_vw.fware')<>'U'			&& Added  by Shrikant S. on 24/04/2017 for GST
*!*			Replace item_vw.ware_nm With lmc_vw.fware In item_vw
*!*		Endif
&&Commented by Priyanka B on 16102018 for Bug-31944 & Bug-31920 End

&&Modified by Priyanka B on 16102018 for Bug-31944 & Bug-31920 Start
	If Type('lmc_vw.fware')<>'U' Or Type('main_vw.fware')<>'U'			&& Added  by Shrikant S. on 24/04/2017 for GST
		Replace item_vw.ware_nm With Iif(Type('main_vw.fware')<>'U',main_vw.fware,lmc_vw.fware) In item_vw
	Endif
&&Modified by Priyanka B on 16102018 for Bug-31944 & Bug-31920 End
Endif

*!*	If Inlist(main_vw.Entry_ty,'II') And Not('vutex' $ vchkprod)  &&Commented by Priyanka B on 23102018 for Bug-31944
If Inlist(main_vw.Entry_ty,'II','SV') And Not('vutex' $ vchkprod)  &&Modified by Priyanka B on 23102018 for Bug-31944
*!*		Replace item_vw.ware_nm With lmc_vw.fware In item_vw
	Select item_vw
	mrecno=Iif(!Eof(),Recno(),0)

	Select Distinct item_vw.tWare From item_vw With (Buffering=.T.) Where !Empty(item_vw.tWare) Into Cursor curware

	Select curware
	If Reccount('curware')>0
		Locate
		If mrecno>0
			Select item_vw
		Endif
		Replace item_vw.tWare With curware.tWare In item_vw

		With .Voupage.Page1.Grditem
			For tcnt = 1 To .ColumnCount Step 1
				colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
				cCond            = &colcontrolsource
				If Alltrim(cCond) = 'ITEM_VW.TWARE'
					withcol  = ".column"+Alltrim(Str(tcnt))+".enabled=.F."
					&withcol
				Endif
			Endfor
		Endwith
	Else
		With .Voupage.Page1.Grditem
			For tcnt = 1 To .ColumnCount Step 1
				colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
				cCond            = &colcontrolsource
				If Alltrim(cCond) = 'ITEM_VW.TWARE'
					withcol  = ".column"+Alltrim(Str(tcnt))+".enabled=.T."
					&withcol
				Endif
			Endfor
		Endwith
	Endif
	If Used('curware')
		Use In curware
	Endif
Endif
&& Added by Shrikant S. on 21/04/2015 for bug-25878		&& End


&& Added by Shrikant S. on 04/10/2016 for GST		&& Start
If Type('ITEM_VW.IGST_AMT')<>'U'
	Do Case
		Case Inlist(Alltrim(.taxapplarea),"INTRASTATE") Or Empty(.taxapplarea)
			If Type('item_vw.IGST_AMT')<>'U'
				Replace IGST_AMT With 0,IGST_PER With 0 In item_vw
			Endif
		Case Inlist(Alltrim(.taxapplarea),"INTERSTATE","OUT OF COUNTRY")
			If Type('item_vw.CGST_AMT')<>'U'
				Replace CGST_AMT With 0,CGST_PER With 0 In item_vw				&& Changed by Shrikant S. on 12/12/2017 just added "in item_vw" for auto updater 13.0.5
			Endif
			If Type('item_vw.SGST_AMT')<>'U'
				Replace SGST_AMT With 0,SGST_PER With 0 In item_vw
			Endif
	Endcase
Endif
*!*	IF INLIST(_curfrmobj.pcvtype,'E1','S1','IB','J6','BP','BR')
*!* If Inlist(.pcVtype,'E1','S1','IB','J7','BP','BR','CP','CR','C6','D6') && Commented by Prajakta B. on 24/07/2017 for GST
If Inlist(.pcVtype,'E1','S1','IB','J7','BP','BR','CP','CR','C6','D6','P7','P8','S7','S8','S4','S5','S9','S3')  && Added by Prajakta B. on 24/07/2017 for GST
	Select item_vw
	Replace item_vw.qty With 1,item_vw.rate With 1 In item_vw

&&Added by Priyanka B on 30032019 for Bug-32067 Start
	If .pcVtype='IB'
		Replace item_vw.qty With 0.01,item_vw.rate With 0.01 In item_vw
	Endif
&&Added by Priyanka B on 30032019 for Bug-32067 End

*	,Sr_Sr With Subs(Item_vw.Sr_Sr,1,2)+'TTT' In item_vw
Endif
&& Added by Shrikant S. on 04/10/2016 for GST		&& End

If Inlist(.pcVtype,'J6','J8')
	Select item_vw
	Replace item_vw.qty With 0.01,item_vw.rate With 0.01,Linerule With 'Taxable',ecredit With .T.  In item_vw
Endif

&&Added by Priyanka B on 10092019 for Bug-32747 Start
_curvouobj = _Screen.ActiveForm

If (Inlist(_curvouobj.pcVtype,"W1","RD") And ("efabric" $ vchkprod))
	lcOldalias = Alias()
	If (_curvouobj.addmode Or _curvouobj.editmode)
		If Inlist(_curvouobj.pcVtype,"W1")
			Replace item_vw.flotno With main_vw.flotno In item_vw
		Endif
*!*			If Inlist(_curvouobj.pcVtype,"RD")
*!*				If Used('Itref_vw')
*!*					Select Itref_vw
*!*					mrecno=Iif(!Eof(),Recno(),0)
*!*					If Reccount('Itref_vw')>0
*!*	*!*						Messagebox(Itref_vw.flotno)
*!*	*!*						Replace item_vw.flotno With itref_vw.flotno In item_vw
*!*					Endif
*!*					If mrecno>0
*!*						Select Itref_vw
*!*						Go mrecno
*!*					Endif
*!*				Endif
*!*			Endif
	Endif
	If !Empty(lcOldalias)
		Select (lcOldalias)
	Endif
Endif
&&Added by Priyanka B on 10092019 for Bug-32747 End


&& Added by Anil on 13-05-2020 for Bug-2020 Start
*!*	If (Inlist(_curvouobj.pcvtype,'RB','BS','R4','R5','S3','S4') And ("efabric" $ vchkprod)) && Commented by Anil on 20052020 for Bug 33402
*!*	If (Inlist(_curvouobj.pcvtype,'RB','BS','R4','S3','S4') And ("efabric" $ vchkprod)) && Commented by Anil on 09072020 for Bug 33328 && Modified by Anil on 20052020 for Bug 33402
	If (Inlist(_curvouobj.pcvtype,'RB','BS','R4') And ("efabric" $ vchkprod)) && Modified by Anil on 09072020 for Bug 33328
		nBaleCol=0
		tot_grd_col=.voupage.page1.grditem.ColumnCount
		For icnt = 1 To tot_grd_col
				IF .voupage.page1.grditem.Columns(icnt).header1.Caption='Bale No'
					nBaleCol = icnt
				ENDIF
		ENDFOR
		IF nBaleCol <> 0
			ncol = "Column"+ALLTRIM(STR(nBaleCol))+".Text1"
			.ItemColNm = ncol
		ENDIF
ENDIF
&& Added by Anil on 13-05-2020 for Bug-2020 End
