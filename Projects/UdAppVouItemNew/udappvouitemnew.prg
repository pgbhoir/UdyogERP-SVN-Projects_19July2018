Local tcol
tcol=0

&& Added by Shrikant S. on 01/06/2010 Start
Select dcmast_vw
*!*	Replace All Code With "A" For Inlist(fld_nm,'CCDAMT','HCDAMT','BCDAMT') And entry_ty='P1'		&& Changed by Shrikant S.on 30/05/2017 for GST

Replace All Code With "A" For Inlist(fld_nm,'U_CUSTAMT','CCDAMT','HCDAMT') And Inlist(entry_ty,'EI','ST') && Added by Ajay Jaiswal on 23/02/2012 for EXIM
&& Added by Shrikant S. on 01/06/2010 End
*!*	Replace All Code With "A" For Inlist(fld_nm,'BCDAMT','INSAMT','FREAMT') And entry_ty='P1'		&& Added by Shrikant S. on 04/10/2016 For GST		&& Commented by Shrikant S.on 30/05/2017 for GST

* Birendra : Start - Changes for TKT-9537 on 20 sept 2011
*!*	IF INLIST(.pcvtype,'P1') AND (([vuexc] $ vchkprod))			&& Commented by Shrikant S. on 03/01/2017 for GST
&& Commented by Shrikant S. on 03/01/2017 for GST		&&Start
*!*	If Inlist(.pcvtype,'P1') And (([vuexc] $ vchkprod) Or ([vugst] $ vchkprod))		&& Commented by Shrikant S. on 03/01/2017 for GST
*!*		tcol1=.voupage.page1.grditem.ColumnCount
*!*		tcol2 = 1
*!*		tcol  = 0
*!*		For tcol2 = 1 To tcol1
*!*			If Inlist(Upper(.voupage.page1.grditem.Columns(tcol2).ControlSource),'ITEM_VW.CCDAMT','ITEM_VW.HCDAMT','ITEM_VW.CCDPER','ITEM_VW.HCDPER','ITEM_VW.BCDAMT','ITEM_VW.BCDPER')
*!*				.voupage.page1.grditem.Columns(tcol2).Tag=Strtran(.voupage.page1.grditem.Columns(tcol2).Tag,"#E#","#A#",1,1)
*!*			Endif
*!*		Endfor
*!*	Endif
&& Commented by Shrikant S. on 03/01/2017 for GST		&&end

* Birendra : End

&& Added by Ajay Jaiswal on 23/02/2012 for EXIM --> Start
If Inlist(.pcvtype,'EI','ST') And (([vuexc] $ vchkprod))
	tcol1=.voupage.page1.grditem.ColumnCount
	tcol2 = 1
	tcol  = 0
	For tcol2 = 1 To tcol1
		If Inlist(Upper(.voupage.page1.grditem.Columns(tcol2).ControlSource),'ITEM_VW.U_CUSTAMT','ITEM_VW.CCDAMT','ITEM_VW.HCDAMT','ITEM_VW.U_CUSTPER','ITEM_VW.CCDPER','ITEM_VW.HCDPER')
			.voupage.page1.grditem.Columns(tcol2).Tag=Strtran(.voupage.page1.grditem.Columns(tcol2).Tag,"#E#","#A#",1,1)
		Endif
	Endfor
Endif
&& Added by Ajay Jaiswal on 23/02/2012 for EXIM --> End


*!*	If Inlist(.pcvtype,'WK','ST','DC') And (([vuexc] $ vchkprod)) &&  Or ([vuinv] $ vchkprod) Comment By Hetal Dt 240310  &&Commented by Priyanka B on 14012020 for Bug-33160
*!*	If Inlist(.pcvtype,'WK','ST','DC','II','LI','SS','I5','IL','NG','RU','WI') And (([vuexc] $ vchkprod)) && Comment By Anil on 12042020 for Bug 33328 &&  Or ([vuinv] $ vchkprod) Comment By Hetal Dt 240310  &&Modified by Priyanka B on 14012020 for Bug-33160
*!*	If Inlist(.pcvtype,'WK','ST','DC','II','LI','SS','I5','IL','NG','RU','WI','BS','R4') And (([vuexc] $ vchkprod)) && Commented by Anil on 04082020 for Bug-33328 Au-2.2.5 -31072020 && Modified By Anil on 12042020 for Bug 33328
	If Inlist(.pcvtype,'WK','ST','DC','II','LI','SS','I5','IL','NG','RU','WI') And (([vuexc] $ vchkprod)) && Modified by Anil on 04082020 for Bug-33328 Au-2.2.5 -31072020
	tcol1=.voupage.page1.grditem.ColumnCount
	tcol2 = 1
	tcol  = 0
	For tcol2 = 1 To tcol1
		If Upper(.voupage.page1.grditem.Columns(tcol2).ControlSource) = 'ITEM_VW.U_BOMDET'
			tcol = tcol2
			Exit
		Endif
	Endfor
	If tcol = 0
		tcol = tcol1 + 1
		.voupage.page1.grditem.ColumnCount=tcol
	Endif
	.voupage.page1.grditem.Columns(tcol).AddObject('cmdBom','VouClass.cmdBom')
	.voupage.page1.grditem.Columns(tcol).cmdbom.Picture = apath+Iif(.pcvtype='ST','bmp\finish_item.gif','bmp\raw_material.gif')
	*!*		.voupage.page1.grditem.Columns(tcol).cmdbom.Caption=	Iif(Inlist(.pcvtype,'ST','DC'),'Receipt','BOM')  &&Commented by Priyanka B on 14012020 for Bug-33160
*!*		.voupage.page1.grditem.Columns(tcol).cmdbom.Caption=	Iif(Inlist(.pcvtype,'ST','DC','II','LI','SS','I5','IL','NG','RU','WI'),'Receipt','BOM')  && Commented By Anil on 12-05-2020 for Bug 33328 &&Modified by Priyanka B on 14012020 for Bug-33160
	.voupage.page1.grditem.Columns(tcol).cmdbom.Caption=	Iif(Inlist(.pcvtype,'ST','DC','II','LI','SS','I5','IL','NG','RU','WI','BS','R4'),'Receipt','BOM') && Modified By Anil on 12-05-2020 for Bug 33328
	.voupage.page1.grditem.Columns(tcol).cmdbom.PicturePosition= 2
	.voupage.page1.grditem.Columns(tcol).CurrentControl='cmdBom'
	*!*		.voupage.page1.grditem.Columns(tcol).header1.Caption=Iif(!Inlist(.pcvtype,'ST','DC'),'BOM','Receipt')  &&Commented by Priyanka B on 14012020 for Bug-33160
	.voupage.page1.grditem.Columns(tcol).header1.Caption=Iif(!Inlist(.pcvtype,'ST','DC','II','LI','SS','I5','IL','NG','RU','WI'),'BOM','Receipt')  &&Modified by Priyanka B on 14012020 for Bug-33160
	.voupage.page1.grditem.Columns(tcol).header1.Alignment= 2
	.voupage.page1.grditem.Columns(tcol).cmdbom.Enabled=.T.
	.voupage.page1.grditem.Columns(tcol).cmdbom.Themes=.F.
	.voupage.page1.grditem.Columns(tcol).cmdbom.Visible=.T.
	.voupage.page1.grditem.Columns(tcol).cmdbom.Height=.voupage.page1.grditem.RowHeight
	*!*		.voupage.page1.grditem.Columns(tcol).cmdbom.ToolTipText = Iif(Inlist(.pcvtype,'ST','DC'),'Receipt Details','BOM Details')  &&Commented by Priyanka B on 14012020 for Bug-33160
*!*		.voupage.page1.grditem.Columns(tcol).cmdbom.ToolTipText = Iif(Inlist(.pcvtype,'ST','DC','II','LI','SS','I5','IL','NG','RU','WI'),'Receipt Details','BOM Details')  &&Commented by Anil on 12052020 for Bug 33328	&&Modified by Priyanka B on 14012020 for Bug-33160
	.voupage.page1.grditem.Columns(tcol).cmdbom.ToolTipText = Iif(Inlist(.pcvtype,'ST','DC','II','LI','SS','I5','IL','NG','RU','WI','BS','R4'),'Receipt Details','BOM Details') &&Modified by Anil on 12052020 for Bug 33328

	.voupage.page1.grditem.Columns(tcol).Sparse=.F.
ENDIF


** Birendra : Commented above and Added Below on 28/02/2011 for Costing ---> Start
&& Comment by Ajay Jaiswal on 23/02/2012 : This same code is also used in EXIM module
*!*	IF (.pcvtype="IP")		&& Commented By Shrikant S. on 08/01/2014		for Bug-20752
*!*	If Inlist(.pcvtype,"IP","OP")	&& Added By Shrikant S. on 08/01/2014		for Bug-20752 && Commented By Kishor A. on 30/09/2015 for Bug-27021
*!*	If Inlist(.pcvtype,"IP","OP","AR")			&& Added By Kishor A. on 30/09/2015 for Bug-27021   &&Commented by Priyanka B on 23012019 for Bug-32210
*!*	If Inlist(.pcvtype,"IP","OP","AR","Q2")			&& Added By Kishor A. on 30/09/2015 for Bug-27021   &&Modified by Priyanka B on 23012019 for Bug-32210		&& Commented by Shrikant S. on 13/09/2019 for Bug-32823
If Inlist(.pcvtype,"IP","OP","AR","Q2","LI")			&& Added By Shrikant S. on 13/09/2019 for Bug-32823
	With .voupage.page1.grditem
		For tcnt = 1 To .ColumnCount Step 1
			colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
			ccond            = &colcontrolsource
			If Alltrim(ccond) = 'ITEM_VW.U_FORPICK'
				If (Main_vw.entry_ty=="Q2")
					colwidth = ".column"+Alltrim(Str(tcnt))+".width = 70"
				Else
					colwidth = ".column"+Alltrim(Str(tcnt))+".width = 50"
				Endif
				withcol  = ".column"+Alltrim(Str(tcnt))
				&colwidth
				With &withcol
					If Type("cmdpick") = 'U'
						.AddObject("cmdpick","cmdclass")
					Endif
					.CurrentControl = "cmdpick"
					.cmdpick.Visible = .T.
					.Sparse = .F.
					&& Added By Shrikant S. on 08/01/2014		for Bug-20752		&& Start
					If (Main_vw.entry_ty=="OP")
						.cmdpick.Caption="IP Pickup"
					Endif
					&& Added By Shrikant S. on 08/01/2014		for Bug-20752		&& End
					&&Added by Priyanka B on 23012019 for Bug-32210 Start
					If (Main_vw.entry_ty=="Q2")
						.cmdpick.Caption="PQ Pickup"
					Endif
					&&Added by Priyanka B on 23012019 for Bug-32210 End
				Endwith
			Endif
		Endfor
	Endwith
Endif
** Birendra : Commented above and Added Below on 28/02/2011 for Costing ---> End

&& Commented by Shrikant S. on 27/10/2017 for GST Bug-30825			&& Start
*!*	*!*	** Birendra : Added Below on 30/09/2011 for TKT-8452 		---Start
*!*	*!*	If oglblprdfeat.udchkprod('AutoTran')
*!*	*!*		If Inlist(.pcvtype,"PT","ST")
*!*	*!*			If !lcode_vw.QC_Module
*!*	*!*				With .voupage.page1.grditem
*!*	*!*					For tcnt = 1 To .ColumnCount Step 1
*!*	*!*						colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
*!*	*!*						ccond            = &colcontrolsource
*!*	*!*						If Alltrim(ccond) = 'ITEM_VW.U_FORPICK'
*!*	*!*							colwidth = ".column"+Alltrim(Str(tcnt))+".width = 80"
*!*	*!*							withcol  = ".column"+Alltrim(Str(tcnt))
*!*	*!*							&colwidth
*!*	*!*							With &withcol
*!*	*!*								If Type("cmdpick") = 'U'
*!*	*!*									.AddObject("cmdpick","cmdclass")
*!*	*!*								Endif
*!*	*!*								.CurrentControl = "cmdpick"
*!*	*!*								.cmdpick.Caption="WorkOrder"
*!*	*!*								.cmdpick.Visible = .T.
*!*	*!*								.Sparse = .F.
*!*	*!*							Endwith
*!*	*!*						Endif
*!*	*!*					Endfor
*!*	*!*				Endwith
*!*	*!*			Endif
*!*	*!*		Endif
*!*	*!*	Endif
*!*	*!*	** Birendra : Added Below on 19/01/2013 for TKT-8436 		---Start
*!*	*!*	If !oglblprdfeat.udchkprod('AutoTran')
*!*	*!*		If Inlist(.pcvtype,"PT","ST")
*!*	*!*			With .voupage.page1.grditem
*!*	*!*				For tcnt = 1 To .ColumnCount Step 1
*!*	*!*					colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
*!*	*!*					ccond            = &colcontrolsource
*!*	*!*					If Alltrim(ccond) = 'ITEM_VW.U_FORPICK'
*!*	*!*						withcol  = ".column"+Alltrim(Str(tcnt))+".Visible = .f."
*!*	*!*						&withcol
*!*	*!*						Exit
*!*	*!*					Endif
*!*	*!*				Endfor
*!*	*!*			Endwith
*!*	*!*		Endif
*!*	*!*	Endif
&& Commented by Shrikant S. on 27/10/2017 for GST Bug-30825			&& End

** Birendra : Added Below on 19/01/2013 for TKT-8436 		---End
** Birendra : Added Below on 30/09/2011 for TKT-8452 		---End

*-- Date : 28-01-2020 Added by  Anil as per Bug No 33252 Start
*!*	If Inlist(.pcvtype,'I7','W3','R2','I8','W4','R3','AF')  &&Commented by Priyanka B on 24022020 for Bug-33252
*!*	If Inlist(.pcvtype,'AF','DF','FO','FS','I6','I7','I8','IC','ID','O6','OC','P6','PY','R2','R3','RD','SG','W1','W2','W3','W4') && Commented By Anil on 18-04-2020  &&Modified by Priyanka B on 24022020 for Bug-33252
If Inlist(.pcvtype,'AF','DF','FO','FS','I6','I7','I8','IC','ID','O6','OC','P6','PY','R2','R3','RD','SG','W1','W2','W3','W4','RB','BS','R4','R5') ; && Added By Anil on 18-04-2020  
	OR INLIST(.pcvtype,'S3','S4') OR INLIST(.pcvtype,'O1','P7','LD','W5','DR','JR','JI')
	tot_grd_col=.voupage.page1.grditem.ColumnCount
	For i = 1 To tot_grd_col
		.voupage.page1.grditem.Columns(i).Width = Iif(Inlist(Upper(.voupage.page1.grditem.Columns(i).ControlSource), ;
			'ITEM_VW.FDESIGN','ITEM_VW.FGRADE','ITEM_VW.FSHADE'),80,.voupage.page1.grditem.Columns(i).Width)
	Endfor
Endif
*-- Date : 28-01-2020 Added by  Anil as per Bug No 33252 End


If Inlist(.pcvtype,'IP','OP') And (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
	.cmdbom.Top   = (.voupage.Top - .cmdbom.Height)
Endif

****** Added By Sachin N. S. on 13/02/2012 for TKT-9411 and Bug-660 Batchwise/Serialize Inventory ****** Start
_objform = .voupage.Parent
If Vartype(_objform._batchserialstk)='O'
	._batchserialstk._uetrigvouitemnew()
Endif
****** Added By Sachin N. S. on 13/02/2012 for TKT-9411 and Bug-660 Batchwise/Serialize Inventory ****** End

**** Added by Shrikant S. on 07/08/2019 for Bug-32412			&& Start
If Type('Lcode_vw.UnitBc')='L'
	If Lcode_vw.UnitBc=.T.
		_curvouobj=.voupage.Parent
		If Type('_curvouobj._unitbcserialize')='O'
			_curvouobj._unitbcserialize._uetrigvouitemnew()
		Endif
	Endif
Endif
**** Added by Shrikant S. on 07/08/2019 for Bug-32412			&& End

&& Added By Shrikant S. on 	21/04/2015 for Bug-25878		&& Start
&& Added by Kishor A For bug-27300 on 01/12/2015 Start
Select Lcode_vw
If Type("lcode_vw.intrtrn")<>'U'
	If Lcode_vw.INTRTRN =.T.
		&& Added by Kishor A For bug-27300 on 01/12/2015 Endif
		If Inlist(Main_vw.entry_ty,"II") And Not('vutex' $ vchkprod)
			With .voupage.page1.grditem
				For tcnt = 1 To .ColumnCount Step 1
					colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
					ccond            = &colcontrolsource
					If Alltrim(ccond) = 'ITEM_VW.WARE_NM'
						withcol  = ".column"+Alltrim(Str(tcnt))+".enabled=.F."
						&withcol
					Endif
				Endfor
			Endwith
		Endif
	Endif && Added by Kishor A For bug-27300 on 01/12/2015
Endif && Added by Kishor A For bug-27300 on 01/12/2015
&& Added By Shrikant S. on 	21/04/2015 for Bug-25878		&& End



&& Added By Shrikant S. on 04/01/2013 for Bug-7269			&& Start
If Inlist(Main_vw.entry_ty,'D2','D3','D4','D5','C3','C2','C4','C5') And (_Screen.ActiveForm.addmode Or _Screen.ActiveForm.editmode)
	With _Screen.ActiveForm
		tot_grd_col=.voupage.page1.grditem.ColumnCount
		For i = 1 To tot_grd_col
			Do Case
				Case .voupage.page1.grditem.Columns(i).header1.Caption='Ass. Value'
					.voupage.page1.grditem.Columns(i).Visible=.F.

				Case .voupage.page1.grditem.Columns(i).header1.Caption='Rate'
					.voupage.page1.grditem.Columns(i).Visible=.F.

				Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
					.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(Main_vw.entry_ty,'D5','D4','C4','C5'),.T.,.F.)

				Case .voupage.page1.grditem.Columns(i).header1.Caption='Sale Bill No.'
					.voupage.page1.grditem.Columns(i).Width=70

				Case .voupage.page1.grditem.Columns(i).header1.Caption='Item Name'
					.voupage.page1.grditem.Columns(i).Width=120
					.voupage.page1.grditem.Columns(i).Visible=.F.

				Case .voupage.page1.grditem.Columns(i).header1.Caption=Iif(Inlist(Main_vw.entry_ty,'C3'),'Payment Made Date','Payment Recd. Date')
					.voupage.page1.grditem.Columns(i).header1.Caption=Iif(Inlist(Main_vw.entry_ty,'C3'),'Payment Made Date','Payment Recd. Date')
					.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(Main_vw.entry_ty,'D3','C3'),.T.,.F.)

				Case .voupage.page1.grditem.Columns(i).header1.Caption=Iif(Inlist(Main_vw.entry_ty,'C3'),'Made Amount','Recd. Amount')
					.voupage.page1.grditem.Columns(i).header1.Caption=Iif(Inlist(Main_vw.entry_ty,'C3'),'Made Amount','Recd. Amount')
					.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(Main_vw.entry_ty,'D3','C3'),.T.,.F.)

				Case .voupage.page1.grditem.Columns(i).header1.Caption='Late Days'
					.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(Main_vw.entry_ty,'D3','C3'),.T.,.F.)

				Case .voupage.page1.grditem.Columns(i).header1.Caption='Interest %'
					.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(Main_vw.entry_ty,'D3','C3'),.T.,.F.)

				Case .voupage.page1.grditem.Columns(i).header1.Caption='Interest Amount'
					.voupage.page1.grditem.Columns(i).Visible=Iif(Inlist(Main_vw.entry_ty,'D3','C3'),.T.,.F.)
					.voupage.page1.grditem.Refresh
			Endcase
		Endfor
	Endwith
Endif
&& Added By Shrikant S. on 04/01/2013 for Bug-7269			&& End


&& Added By Shrikant S. on 26/06/2012 for Bug-4744		&& Start
If Vartype(oglblprdfeat)='O'
	If oglblprdfeat.udchkprod('openord')
		If Type('lcode_vw.allowzeroqty')!='U'
			If (.pcvtype='OO') And Lcode_vw.allowzeroqty=.T.
				With .voupage.page1.grditem
					For tcnt = 1 To .ColumnCount Step 1
						colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
						ccond            = &colcontrolsource
						If Alltrim(ccond) = 'ITEM_VW.QTY'
							colVisible= ".column"+Alltrim(Str(tcnt))+".VISIBLE = .F."
							&colVisible
						Endif
					Endfor
				Endwith
			Endif
		Endif
	Endif
Endif
&& Added By Shrikant S. on 26/06/2012 for Bug-4744		&& End

&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('pharmaind')
		If Inlist(.pcvtype,"AR")
			tot_grd_col=.voupage.page1.grditem.ColumnCount
			For i = 1 To tot_grd_col
				Do Case
					Case .voupage.page1.grditem.Columns(i).header1.Caption ='Quantity'
						lnqtycol=i
					Case .voupage.page1.grditem.Columns(i).header1.Caption ='Rate'
						lnratecol=i
					Case .voupage.page1.grditem.Columns(i).header1.Caption ='Conv. Qty'
						lnqty1col=i
					Case .voupage.page1.grditem.Columns(i).header1.Caption ='Conv. Unit'
						lnunitcol=i
					Case .voupage.page1.grditem.Columns(i).header1.Caption ='Conv. Rate'
						lncrate=i
					Case .voupage.page1.grditem.Columns(i).header1.Caption ='Amount'
						lnamount=i
				Endcase

			Endfor
			lntmpOrder=.voupage.page1.grditem.Columns(lnqtycol).ColumnOrder
			.voupage.page1.grditem.Columns(lnqtycol).ColumnOrder=.voupage.page1.grditem.Columns(lnqty1col).ColumnOrder
			.voupage.page1.grditem.Columns(lnqty1col).ColumnOrder=lntmpOrder

			lntmpOrder=.voupage.page1.grditem.Columns(lnratecol).ColumnOrder
			.voupage.page1.grditem.Columns(lnratecol).ColumnOrder=.voupage.page1.grditem.Columns(lncrate).ColumnOrder
			.voupage.page1.grditem.Columns(lncrate).ColumnOrder=lntmpOrder

		Endif

		If Inlist(.pcvtype,"IP")
			tot_grd_col=.voupage.page1.grditem.ColumnCount
			For i = 1 To tot_grd_col
				Do Case
					Case .voupage.page1.grditem.Columns(i).header1.Caption ='Pickup'
						.voupage.page1.grditem.Columns(i).cmdpick.Caption ='Select'
						Exit
				Endcase
			Endfor
		Endif
		If Inlist(.pcvtype,'ST') And (([vuexc] $ vchkprod))
			tcol1=.voupage.page1.grditem.ColumnCount
			tcol2 = 1
			tcol  = 0
			For tcol2 = 1 To tcol1
				If Alltrim(Upper(.voupage.page1.grditem.Columns(tcol2).header1.Caption)) == 'RECEIPT'
					tcol = tcol2
					Exit
				Endif
			Endfor
			If tcol >0
				.voupage.page1.grditem.Columns(tcol).ColumnOrder=9
			Endif
		Endif

		If Inlist(.pcvtype,'SS')
			With .voupage.page1.grditem
				For tcnt = 1 To .ColumnCount Step 1
					colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
					ccond            = &colcontrolsource
					If Alltrim(ccond) = 'ITEM_VW.U_FORPICK'
						colwidth = ".column"+Alltrim(Str(tcnt))+".width = 80"
						withcol  = ".column"+Alltrim(Str(tcnt))
						&colwidth
						With &withcol
							If Type("cmdbtn") = 'U'
								.AddObject("cmdbtn","cmdbtncls")
							Endif
							.CurrentControl = "cmdbtn"
							.cmdbtn.Caption="Click Here"
							.cmdbtn.Visible = .T.
							.Sparse = .F.
						Endwith
					Endif
				Endfor
			Endwith

		Endif
	Endif
Endif
&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& End



&& Added by Shrikant S. on 06/10/2016 for GST			&& Start

If Type('_Screen.ActiveForm.pcvtype')<>'U'

	*!*		If Inlist(_Screen.ActiveForm.pcvtype,'E1','S1','IB','J6','BP','BR','CP','CR','C6','D6','RV','J8') And (_Screen.ActiveForm.addmode Or _Screen.ActiveForm.editmode)    && Added by Shrikant S. on 06/02/2017 for GST  &&Commented by Priyanka B on 13042018 for AU 13.0.6
	*!*		If Inlist(.pcvtype,'E1','S1','IB','J6','BP','BR','CP','CR','C6','D6','RV','J8') And (_Screen.ActiveForm.addmode Or _Screen.ActiveForm.editmode)    && Added by Shrikant S. on 06/02/2017 for GST  &&Modified by Priyanka B on 13042018 for AU 13.0.6
	If Inlist(.pcvtype,'E1','S1','IB','J6','BP','BR','CP','CR','C6','D6','RV','J8') And (_Screen.ActiveForm.addmode Or _Screen.ActiveForm.editmode) And (_Screen.ActiveForm.pcvtype=Main_vw.entry_ty)   && Added by Shrikant S. on 11/05/2018 for Installer 1.0.0
		*!*		If Inlist(_Screen.ActiveForm.pcvtype,'E1','S1','IB','J6','BP','BR','CP','CR','C6','D6','RV','J8','P7','P8','S7','S8','S3','S4','S5','S9')And (_Screen.ActiveForm.addmode Or _Screen.ActiveForm.editmode)				 && Added by Prajakta B. on 24/07/2017 for GST
		*!*	If Inlist(Main_vw.Entry_ty,'E1','S1','IB','J6','BP','BR','CP','CR','GC','GD','C6','D6') And (_Screen.ActiveForm.addmode Or _Screen.ActiveForm.editmode)				&& Added by Shrikant S. on 06/02/2017 for GST
		If Type('_Screen.ActiveForm.voupage.page1.grditem')<>'U'  &&Added by Priyanka B on 17042018 for AU 13.0.6
			With _Screen.ActiveForm
				tot_grd_col=.voupage.page1.grditem.ColumnCount
				For i = 1 To tot_grd_col
					Do Case
						Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'
							.voupage.page1.grditem.Columns(i).Visible=.F.
							*!*					IF Inlist(Main_vw.Entry_ty,'GC','GD','C6','D6')
							*!*						.voupage.page1.grditem.Columns(i).Visible=.T.
							*!*						.voupage.page1.grditem.Columns(i).Enabled=.F.
							*!*					endif
						Case .voupage.page1.grditem.Columns(i).header1.Caption='Rate'
							.voupage.page1.grditem.Columns(i).Visible=.F.
							*!*					IF Inlist(Main_vw.Entry_ty,'GC','GD','C6','D6')
							*!*						.voupage.page1.grditem.Columns(i).Visible=.T.
							*!*					endif

						Case Upper(.voupage.page1.grditem.Columns(i).ControlSource)='ITEM_VW.U_ASSEAMT'
							*!*					If Inlist(_Screen.ActiveForm.pcvtype,"IB","J6","E1","S1","C6","D6")		&& Commented by Shrikant S. on 06/02/2017 for GST
							If Inlist(_Screen.ActiveForm.pcvtype,"IB","J6","J8")		&& Added by Shrikant S. on 06/02/2017 for GST
								*!*					If Inlist(_Screen.ActiveForm.pcvtype,"IB","J6","E1","S1")					&& Added by Shrikant S. on 06/02/2017 for GST
								.voupage.page1.grditem.Columns(i).Visible=.F.
							Endif
							If Inlist(_Screen.ActiveForm.pcvtype,"C6","D6")
								.voupage.page1.grditem.Columns(i).header1.Caption="Diff. Taxable Value"
								.voupage.page1.grditem.Columns(i).ReadOnly=.T.
								.voupage.page1.grditem.Columns(i).Width=110
							Endif
							If Inlist(_Screen.ActiveForm.pcvtype,"S1","E1")
								.voupage.page1.grditem.Columns(i).ReadOnly=.T.
							Endif

						Case Upper(.voupage.page1.grditem.Columns(i).ControlSource)="ITEM_VW.STAXAMT"
							.voupage.page1.grditem.Columns(i).ReadOnly=.T.
							&& Added by Shrikant S. on 13/07/2018 for Bug-31518		&& Start
						Case Alltrim(Upper(.voupage.page1.grditem.Columns(i).ControlSource))="ITEM_VW.FCRATE"
							If Inlist(_Screen.ActiveForm.pcvtype,'BP','BR','CP','CR')
								.voupage.page1.grditem.Columns(i).Visible=.F.
							Endif
							&& Added by Shrikant S. on 13/07/2018 for Bug-31518		&& End

					Endcase
				Endfor
			Endwith
		Endif  &&Added by Priyanka B on 17042018 for AU 13.0.6
	Endif
Endif
&& Added by Shrikant S. on 06/10/2016 for GST			&& End

&& Added by Shrikant S. on 23/02/2017 for GST		&& Start
If Type('_Screen.ActiveForm.pcvtype')<>'U'
	*!*		If Inlist(_Screen.ActiveForm.pcvtype,'GD','GC')  &&Commented by Priyanka B on 13042018 for AU 13.0.6
	If Inlist(.pcvtype,'GD','GC')  &&Modified by Priyanka B on 13042018 for AU 13.0.6
		If Type('_Screen.ActiveForm.voupage.page1.grditem')<>'U'  &&Added by Priyanka B on 17042018 for AU 13.0.6
			With _Screen.ActiveForm
				tot_grd_col=.voupage.page1.grditem.ColumnCount
				For i = 1 To tot_grd_col
					Do Case
						Case Upper(.voupage.page1.grditem.Columns(i).ControlSource)='ITEM_VW.RATE'
							.voupage.page1.grditem.Columns(i).header1.Caption='Diff. Rate'
							&& Commented by Suraj K. Date on 22-01-2018 for Bug-30639  STart
							*!*					Case .voupage.page1.grditem.Columns(i).header1.Caption='Quantity'			&& Added by Shrikant S. on 12/08/2017 for GST
							*!*						.voupage.page1.grditem.Columns(i).Enabled=.F.							&& Added by Shrikant S. on 12/08/2017 for GST
							&& Commented by Suraj K. Date on 22-01-2018 for Bug-30639   End
					Endcase
				Endfor
			Endwith
		Endif  &&Added by Priyanka B on 17042018 for AU 13.0.6
	Endif
Endif
&& Added by Shrikant S. on 23/02/2017 for GST		&& End

&& Added by Shrikant S. on 19/06/2017 for GST		&& Start
If Type('_Screen.ActiveForm.pcvtype')<>'U'
	*!*		If Inlist(_Screen.ActiveForm.pcvtype,'UB')  &&Commented by Priyanka B on 13042018 for AU 13.0.6
	If Inlist(.pcvtype,'UB')  &&Modified by Priyanka B on 13042018 for AU 13.0.6
		If Type('_Screen.ActiveForm.voupage.page1.grditem')<>'U'  &&Added by Priyanka B on 17042018 for AU 13.0.6
			With _Screen.ActiveForm
				tot_grd_col=.voupage.page1.grditem.ColumnCount
				For i = 1 To tot_grd_col
					Do Case
						Case Upper(.voupage.page1.grditem.Columns(i).ControlSource)='ITEM_VW.SELFDISC'
							.voupage.page1.grditem.Columns(i).Visible=.F.
					Endcase
				Endfor
			Endwith
		Endif
	Endif  &&Added by Priyanka B on 17042018 for AU 13.0.6
Endif
&& Added by Shrikant S. on 19/06/2017 for GST		&& End

&& Added by Shrikatn S. on 19/07/2016 for Bug-28289		&& Start
If Inlist(.pcvtype,'RN') And ([vuexc] $ vchkprod)
	.cmdSelectBom.Top = .cmdnarration.Top+.cmdnarration.Height+5
	.cmdSelectBom.Left = .cmdnarration.Left
	*		.cmdbom.TOP   = (.voupage.TOP - .cmdbom.HEIGHT)
Endif
&& Added by Shrikatn S. on 19/07/2016 for Bug-28289		&& End

&& Added by Shrikant S. on 19/10/2018 Bug-31942		&& Start
*!*	If Inlist(.pcvtype,'PK') 			&& Commented by Shrikant S. on 24/11/2018 for Auto updater 2.0.1
If Inlist(.pcvtype,'PK') 				&& Added by Shrikant S. on 24/11/2018 for Auto updater 2.0.1
	.cmdcsv.Top = .txtInvoiceNo.Top
	.cmdcsv.Left = .txtInvoiceNo.Left + .txtInvoiceNo.Width +20
Endif
If Inlist(.pcvtype,'SK') 				&& Added by Shrikant S. on 24/11/2018 for Auto updater 2.0.1
	.cmdcsv.Top = .txtDate.Top
	.cmdcsv.Left = .txtDate.Left + .txtDate.Width +20
Endif

&& Added by Shrikant S. on 19/10/2018 Bug-31942		&& End

** Birendra : Commented above and Added Below on 28/02/2011 for Costing 		---Start
&& Comment by Ajay Jaiswal on 23/02/2012 : This same code is also used in EXIM module
*!*	DEFINE CLASS cmdclass AS COMMANDBUTTON
*!*		CAPTION = "Pickup"
*!*		FONTSIZE = 8
*!*		FONTBOLD = .T.
*!*		FORECOLOR = RGB(255,0,0)

*!*		PROCEDURE CLICK()
*!*		DO FORM uefrm_itempickup WITH THISFORM.addmode,THISFORM.editmode,THISFORM.DATASESSIONID
*!*		RETURN
*!*	ENDDEFINE

*changes by EBS team on 07/03/14 for Bug-21466,21467,21468 start
* Changes done as per --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
* Date : 08/11/2012
* Changes done by EBS Product Team
_mexim = oglblprdfeat.udchkprod('exim')
If _mexim
	If Inlist(.pcvtype,'SI')
		With .voupage.page1.grditem
			For tcnt = 1 To .ColumnCount Step 1
				colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
				ccond            = &colcontrolsource
				If Alltrim(ccond) = 'ITEM_VW.U_FORINFO'
					colwidth = ".column"+Alltrim(Str(tcnt))+".width = 120"
					withcol  = ".column"+Alltrim(Str(tcnt))
					&colwidth
					With &withcol
						If Type("cmdPreShipInfo") = 'U'
							.AddObject("cmdPreShipInfo","cmdButton")
						Endif
						.CurrentControl = "cmdPreShipInfo"
						.cmdPreShipInfo.Visible = .T.
						.Sparse = .F.
						.header1.Caption = ""
						.header1.Alignment = 2
						.cmdPreShipInfo.Picture = apath+'bmp\additional_info.gif'
						.cmdPreShipInfo.DisabledPicture = apath+'bmp\additional_info_off.gif'
						.cmdPreShipInfo.Caption=	'  Pre Shipment Info.'
						.cmdPreShipInfo.ToolTipText = '  Pre Shipment Info.'
						.cmdPreShipInfo.FontSize= 8
						.cmdPreShipInfo.PicturePosition= 2
					Endwith
				Endif
			Endfor
		Endwith
	Endif
Endif
* End --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info


***** Added by Sachin N. S. on 01/04/2016 for Bug-27503 -- Start
_curvouobj = _Screen.ActiveForm
***** Changed by Sachin N. S. on 24/11/2018 for Bug-31943 -- Start
If Type('_objform._udClsPointOfSale')='O'
	_objform._udClsPointOfSale._uetrigvouitemnew()
Endif
*!*	If Type('_curvouobj._udClsPointOfSale')='O'
*!*		_curvouobj._udClsPointOfSale._uetrigvouitemnew()
*!*	Endif
***** Changed by Sachin N. S. on 24/11/2018 for Bug-31943 -- End
***** Added by Sachin N. S. on 01/04/2016 for Bug-27503 -- End

** Added By rupesh G. on 28/01/2019 for bug no. 32194----start
If Type('_Screen.ActiveForm.pcvtype')<>'U'
	If Inlist(.pcvtype,"M4")
		With .voupage.page1.grditem
			For tcnt = 1 To .ColumnCount Step 1
				colcontrolsource = "upper(alltrim(.column"+Alltrim(Str(tcnt))+".controlsource))"
				ccond            = &colcontrolsource
				If Alltrim(ccond) = 'ITEM_VW.FORPICK'
					colwidth = ".column"+Alltrim(Str(tcnt))+".width = 110"
					withcol  = ".column"+Alltrim(Str(tcnt))
					&colwidth
					With &withcol
						If Type("cmdpick") = 'U'
							.AddObject("cmdpick","cmdclass")
						Endif
						.CurrentControl = "cmdpick"
						.cmdpick.Visible = .T.
						.Sparse = .F.
					Endwith
					Do Case
						Case .Columns(tcnt).header1.Caption ='Pickup'
							.Columns(tcnt).cmdpick.Caption ='AutomobilePickup'
							Exit
					Endcase
				Endif
			Endfor
		Endwith
	Endif
Endif

** Added By rupesh G. on 28/01/2019 for bug no. 32194----End

&&Commented by Priyanka B on 17032020 for Bug-32210,33268 & 33193 Start
*!*	&&Added by Priyanka B on 24012019 for Bug-32210 Start
*!*	If Type('_Screen.ActiveForm.pcvtype')<>'U'
*!*		Select Lcode_vw
*!*		*!*		If Type("lcode_vw.iautotran")<>'U'
*!*		If Type("lcode_vw.vendor")<>'U'
*!*			*!*			If !Empty(lcode_vw.iautotran)
*!*			If !Empty(Lcode_vw.vendor)
*!*				** Date : 01-02-2020  Added by Anil for Bug No 33193 Start
*!*				If WarehouseExists=.F.
*!*					tcol = 4
*!*				Else
*!*					tcol = tcol + 1
*!*				Endif
*!*				** Date : 01-02-2020  Added by Anil for Bug No 33193 End
*!*				.voupage.page1.grditem.Columns(tcol+1).AddObject('cmdBom','cmdVendDtls')
*!*				*!*			.Voupage.Page1.Grditem.Columns(tcol+1).cmdBom.Picture = apath+Iif(.pcVtype='ST' Or .pcVtype='IP','bmp\finish_item.gif','bmp\raw_material.gif')
*!*				.voupage.page1.grditem.Columns(tcol+1).cmdbom.Picture = apath+'bmp\raw_material.gif'
*!*				.voupage.page1.grditem.Columns(tcol+1).cmdbom.Caption=	"Select"
*!*				.voupage.page1.grditem.Columns(tcol+1).cmdbom.PicturePosition= 2
*!*				.voupage.page1.grditem.Columns(tcol+1).CurrentControl='cmdBom'
*!*				.voupage.page1.grditem.Columns(tcol+1).header1.Caption="Vendor Details"
*!*				.voupage.page1.grditem.Columns(tcol+1).header1.Alignment= 2
*!*				.voupage.page1.grditem.Columns(tcol+1).cmdbom.Enabled=.T.
*!*				.voupage.page1.grditem.Columns(tcol+1).cmdbom.Themes=.F.
*!*				.voupage.page1.grditem.Columns(tcol+1).cmdbom.Visible=.T.
*!*				.voupage.page1.grditem.Columns(tcol+1).cmdbom.Height=.voupage.page1.grditem.RowHeight
*!*				.voupage.page1.grditem.Columns(tcol+1).cmdbom.ToolTipText = "Select"
*!*				.voupage.page1.grditem.Columns(tcol+1).Sparse=.F.
*!*				.voupage.page1.grditem.Columns(tcol+1).Width = 100
*!*			Endif
*!*		Endif
*!*	Endif
*!*	&&Added by Priyanka B on 24012019 for Bug-32210 End
&&Commented by Priyanka B on 17032020 for Bug-32210,33268 & 33193 End

&&Added by Rupesh G on 03022020 for Bug-33181 Start
If Inlist(.pcvtype,'ST') And [udewaygen] $ vchkprod And Upper(Alltrim(.cmdGen.Caption))=="GENERATE E-INVOICE"
	.cmdGen.Enabled=Iif((!.addmode And !.editmode),.T.,.F.)
Endif
&&Added by Rupesh G on 03022020 for Bug-33181 End

&&Added by Divyang on 09062020 for Bug-33510 Start
If lcode_vw.BC_Tran = .T. Or lcode_vw.BC_Item = .T. Or lcode_vw.BC_SerInv = .T.
	If Inlist(.pcvtype,'OP','AR','PT','ST','OS') And [barcode] $ vchkprod And Upper(Alltrim(.cmdPrintBar.Caption))=="BARCODE PRINT"
		.cmdPrintBar.Enabled=Iif((!.addmode And !.editmode),.T.,.F.)
	ENDIF
ENDIF
&&Added by Divyang on 09062020 for Bug-33510 End


* Changes done as per --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
* Date : 08/11/2012
* Changes done by EBS Product Team
Define Class cmdButton As CommandButton
	Procedure Click
		curObj = _Screen.ActiveForm
		If !Used("Tbl_PreShipment_Vw")
			lcStr = "Select CAST(0 as int) as serial,PreShipInfo.* From [PreShipInfo] ;
					 Where Entry_TY = ?Item_vw.Entry_Ty And Tran_cd = ?Item_vw.Tran_Cd Order by Itserial"

			nretval = curObj.sqlconobj.dataconn("EXE",company.dbname,lcStr,"Tbl_PreShipment_Vw","curObj.nhandle",curObj.DataSessionId)
			If nretval < 0
				Return .F.
			Endif
			nretval=curObj.sqlconobj.sqlconnclose("curObj.nHandle")
		Endif

		Select Tbl_PreShipment_Vw
		Go Top
		Do Form frm_PreShipmentInfo With curObj.addmode,curObj.editmode,curObj.DataSessionId
		Select Item_vw
	Endproc
Enddefine

* End --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
*changes by EBS team on 07/03/14 for Bug-21466,21467,21468 end

Define Class cmdclass As CommandButton
	Caption = "Pickup"
	FontSize = 8
	FontBold = .T.
	ForeColor = Rgb(0,0,0)

	Procedure Click()
		&& Added By Pankaj B. on 14-03-2015 for Bug-25542 Start
		curObj = _Screen.ActiveForm

		lcStr = "Select type From it_mast where it_code= ?Item_vw.it_code"
		nretval = curObj.sqlconobj.dataconn("EXE",company.dbname,lcStr,"ipmast_vw","curObj.nhandle",curObj.DataSessionId)
		If nretval < 0
			Return .F.
		Endif
		nretval=curObj.sqlconobj.sqlconnclose("curObj.nHandle")
		**Added By Rupesh G. on 28/01/2019 for bug no 32194--Start
		If Main_vw.entry_ty="M4" And This.Caption="AutomobilePickup"
			Do Form uefrm_itempickup_m4 With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId
		Endif
		**Added By Rupesh G. on 28/01/2019 for bug no 32194--End
		&&Added by Priyanka B on 23012019 for Bug-32210 Start
		If Main_vw.entry_ty="Q2" And This.Caption="PQ Pickup"
			Do Form uefrm_itempickupQ2_PQ With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId
		Endif
		&&Added by Priyanka B on 23012019 for Bug-32210 End

		*!*		IF this.Caption="Pickup" And !Inlist(UPPER(ipmast_vw.Type),"FINISHED","SEMI FINISHED") && Commented Kishor A. for bug-27021 on 30/09/2015
		If This.Caption="Pickup" And !Inlist(Upper(ipmast_vw.Type),"FINISHED","SEMI FINISHED") And !Main_vw.entry_ty="AR" && Added by Kishor A. for bug-27021
			Do Form uefrm_itempickup With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId
		Endif

		&& Added by Kishor A. for bug-27021 on 30/09/2015 Start..
		If Inlist(Main_vw.entry_ty,'AR')
			Do Form uefrm_itpopickup With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId
			Thisform.ItemGrdBefCalc(4)
		Endif
		&& Added by Kishor A. for bug-27021 on 30/09/2015 End..

		*!*		IF this.Caption="Pickup" And Inlist(UPPER(ipmast_vw.Type),"FINISHED","SEMI FINISHED") && Commented Kishor A. for bug-27021 on 30/09/2015
		If This.Caption="Pickup" And Inlist(Upper(ipmast_vw.Type),"FINISHED","SEMI FINISHED") And !Main_vw.entry_ty="AR" && Added by Kishor A. for bug-27021
			*!*			Do Form uefrm_OpStItemAllocation With Thisform.DataSessionId,Thisform.addmode,Thisform.editmode,Thisform			&& Commented by Shrikant S. on 02/02/2017 for GST
			Do Form uefrm_itempickup With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId						&& Added by Shrikant S. on 02/02/2017 for GST
		Endif
		&& Added By Pankaj B. on 14-03-2015 for Bug-25542 End

		&& Comented By Pankaj B. on 14-03-2015 for Bug-25542 Start
		*!*		IF this.Caption="Pickup"
		*!*		Do Form uefrm_itempickup With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId
		*!*		ENDIF
		&& Comented By Pankaj B. on 14-03-2015 for Bug-25542 End

		** Birendra : Added Below on 14/14/2011 for TKT-8452 		---Start
		If This.Caption="WorkOrder" And oglblprdfeat.udchkprod('AutoTran')
			formObj=_Screen.ActiveForm
			_malias 	= Alias()
			If Inlist(Main_vw.entry_ty,'PT') And (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
				Do Form uefrm_bomdetailsIP_AUTO With formObj.DataSessionId,formObj.addmode,formObj.editmode,formObj
			Endif
			If Inlist(Main_vw.entry_ty,'ST') And (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod))
				Do Form uefrm_bomdetailsop_AUTO With formObj.DataSessionId,formObj.addmode,formObj.editmode,formObj
			Endif
			If !Empty(_malias)
				Select &_malias
			Endif
		Endif
		** Birendra : Added Below on 14/14/2011 for TKT-8452 		---End
		&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& Start && Comented By Pankaj B. on 14-03-2015 for Bug-25542 Start
		*!*		IF this.Caption="Select"
		*!*			Do Form uefrm_itempickup_pharmaind With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId
		*!*		ENDIF
		&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& End && Comented By Pankaj B. on 14-03-2015 for Bug-25542 End

		&& Added By Pankaj B. on 14-03-2015 for Bug-25542 Start
		If This.Caption="Select" And !Inlist(Upper(ipmast_vw.Type),"FINISHED","SEMI FINISHED")
			Do Form uefrm_itempickup_pharmaind With Thisform.addmode,Thisform.editmode,Thisform.DataSessionId
		Endif
		If This.Caption="Select" And Inlist(Upper(ipmast_vw.Type),"FINISHED","SEMI FINISHED")
			Do Form uefrm_OpStItemAllocation_pharmaind With Thisform.DataSessionId,Thisform.addmode,Thisform.editmode,Thisform
		Endif
		&& Added By Pankaj B. on 14-03-2015 for Bug-25542 End

		&& Added By Shrikant S. on 08/01/2014		for Bug-20752		&& Start
		If This.Caption=="IP Pickup"
			Do Form UEFRM_IP_ALLOCATION With _Screen.ActiveForm.DataSessionId,_Screen.ActiveForm.addmode,_Screen.ActiveForm.editmode
		Endif
		&& Added By Shrikant S. on 08/01/2014		for Bug-20752		&& End

		Return
Enddefine

** Added by Shrikant S. on 10/08/2010 for EOU 		---End


&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& Start
Define Class cmdbtncls As CommandButton
	Procedure Click
		_malias 	= Alias()
		_mRecNo	= Recno()
		Do Form uefrm_batchselect With _Screen.ActiveForm.addmode,_Screen.ActiveForm.editmode,_Screen.ActiveForm.DataSessionId,Thisform
		If !Empty(_malias)
			Select &_malias
		Endif
		If Betw(_mRecNo,1,Reccount())
			Go _mRecNo
		Endif
	Endproc
Enddefine
&& Added by Shrikant S. on 28/06/2014 for Bug-23280		&& End

&&Commented by Priyanka B on 17032020 for Bug-32210,33268 & 33193 Start
*!*	&&Added by Priyanka B on 24012019 for Bug-32210 Start
*!*	Define Class cmdVendDtls As CommandButton
*!*		Procedure Click
*!*			_malias 	= Alias()
*!*			_mRecNo	= Recno()
*!*			Do udAutoTranPickup.App With _Screen.ActiveForm.addmode,_Screen.ActiveForm.editmode,_Screen.ActiveForm.DataSessionId,Thisform
*!*			If !Empty(_malias)
*!*				Select &_malias
*!*			Endif
*!*			If Betw(_mRecNo,1,Reccount())
*!*				Go _mRecNo
*!*			Endif
*!*		Endproc
*!*	Enddefine
*!*	&&Added by Priyanka B on 24012019 for Bug-32210 End
&&Commented by Priyanka B on 17032020 for Bug-32210,33268 & 33193 End