Lparameters _vouitemassfrmname		&&vasant100111
_curvouobj = _Screen.ActiveForm &&&Added by satish pal for bug-7678
&&&Added by satish pal for bug-4180 dt.18/05/2012--start

&& Commented by Shrikant S. on 28/05/2013 for Bug-12163		&& Start
*!*	IF TYPE("_SCREEN.ACTIVEFORM.mainAlias") <> "C"
*!*		RETURN 0
*!*	ENDIF
&& Commented by Shrikant S. on 28/05/2013 for Bug-12163		&& End
&&&Added by satish pal for bug-7678 dt.09/12/2012-Start
*!*	IF TYPE('_curvouobj.PcvType') <> 'C'
*!*		RETURN 0
*!*	ENDIF

If Type('_curvouobj.addmode')='U' Or Type('_curvouobj.editmode')='U'
	Return 0
Endif
If _curvouobj.addmode=.T. Or _curvouobj.editmode=.T.
	&&&Added by satish pal for bug-7678 dt.09/12/2012-End
	If Type("_SCREEN.ACTIVEFORM.mainAlias") = "C"
		If Upper(Alltrim(_Screen.ActiveForm.mainAlias)) <> "MAIN_VW"
			Return 0  &&&Changed by satish pal for bug-4180 dt.02/08/2012
		Endif
		*!*		SET  DATASESSIONID TO _SCREEN.ACTIVEFORM.DATASESSIONID
		If !Used("item_vw")
			Return 0
		Endif
	Endif
	&&&Added by satish pal for bug-4180 dt.18/05/2012--end

	*Birendra:On 12 oct 2011 for Bug-60 :start:
	If Type('_vouitemassfrmname')='L'
		_vouitemassfrmname=_Screen.ActiveForm
	Endif
	*Birendra:On 12 oct 2011 for Bug-60 :End:

	&& Added by Shrikant S. on 04/01/2017 for GST		&& Start
	objform=_curvouobj
	If Type('_curvouobj.ofrmfrom')<>'U'
		objform=_curvouobj.ofrmfrom
	Endif
	&& Added by Shrikant S. on 04/01/2017 for GST		&& End

	massamt  = 0
	_mrprate = 0
	_mabtper = 0
	If Type('item_vw.u_mrprate') = 'N'
		_mrprate = item_vw.u_mrprate
		_mabtper = item_vw.abtper
	Endif
	*Birendra:On 12 oct 2011 for Bug-60 :start:
	If Type('Main_vw.IncExcise')<>'U'
		If main_vw.IncExcise=.F.
			Select DcMast_vw
			Replace All excl_gross With "A" For Inlist(fld_nm,'CCDAMT','HCDAMT','U_CUSTAMT') And Entry_ty='P1' In DcMast_vw
		Else
			Select DcMast_vw
			Replace All excl_gross With "" For Inlist(fld_nm,'CCDAMT','HCDAMT','U_CUSTAMT') And Entry_ty='P1' In DcMast_vw
		Endif
	Endif
	*Birendra:On 12 oct 2011 for Bug-60 :End:

	&& Added by sandeep on 22/02/12 for bug-2373 -Start
	malias = Alias()
	Local vrate,vrateper
	mrateper = 1
	sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)
	nhandle=0

	*!*		sq1=" select top 1 rateper from it_mast where It_code = ?Item_vw.It_code"				&& Commented by Shrikant S. on 22/12/2016 for GST
	*!*		sq1=" select top 1 "+Iif(Inlist(objform.pcvtype,"PT","P1","PR"),'Prate','RatePer')+" as rateper from it_mast where It_code = ?Item_vw.It_code"	&& Commented by Shrikant S. on 22/12/2016 for GST (TO be rectify in stock valuation)
	*!*		sq1=" select top 1 "+Iif(Inlist(main_vw.Entry_ty,"PT","P1","PR"),'Prate','RatePer')+" as rateper from it_mast where It_code = ?Item_vw.It_code"	&& Added by Shrikant S. on 22/12/2016 for GST (TO be rectify in stock valuation)
	sq1=" select top 1 Prate,RatePer from it_mast where It_code = ?Item_vw.It_code"	&& Added by Shrikant S. on 22/12/2016 for GST (TO be rectify in stock valuation)
	nretval = sqlconobj.dataconn([EXE],company.dbname,sq1,"EXCUR","nHandle",_Screen.ActiveForm.DataSessionId)

	If nretval<0
		Return .F.
	Endif

	&& Added by Shrikant S. on 17/03/2017 for GST		&& Start
	If Type('Lcode_vw.IoTrans')<>'U'
		Do Case
			Case Lcode_vw.IoTrans=1
				If EXCUR.Prate # 0
					mrateper = EXCUR.Prate
				Endif
			Case Lcode_vw.IoTrans=2
				If EXCUR.rateper # 0
					mrateper = EXCUR.rateper
				Endif
		Endcase
	Endif
	&& Added by Shrikant S. on 17/03/2017 for GST		&& End

	*!*		vrateper=Iif(!Isnull(excur.rateper),excur.rateper,0)		&& Commented by Shriaknt S. on 20/03/2017 for GST

	=sqlconobj.sqlconnclose("nHandle")
	If Used("EXCUR")
		Use In EXCUR
	Endif

	&& Commented by Shriaknt S. on 20/03/2017 for GST 		&&Start
	*!*		If  vrateper # 0
	*!*			mrateper=vrateper
	*!*		Endif
	&& Commented by Shriaknt S. on 20/03/2017 for GST 		&&End

	&& Added by sandeep on 22/02/12 for bug-2373 -End

	&& Added by Shrikant S. on 26/10/2018 for Bug-31942		&& Start
	llfreeqty=.F.
	_lnfreeqty=0				&& Added by Shrikant S. on 14/12/2018	for Installer 2.1.0
	If Type('Item_vw.FreeQTy')<>'U'
		llfreeqty=Iif(item_vw.FreeQTy >0 ,.T.,.F.)
		&& Added by Shrikant S. on 14/12/2018 for Installer 2.1.0		&& Start
		If Type('Lcode_vw.gpuomapp')<>'U'
			If Lcode_vw.gpuomapp=.T.
				&& Added by Shrikant S. on 24/12/2018 for Installer 2.1.0	&& Start
				_llnitemqty=item_vw.qty
				If Type('Item_vw.looseqty')='N'
					_llnitemqty=_llnitemqty - item_vw.looseqty
				Endif
				&& Added by Shrikant S. on 24/12/2018 for Installer 2.1.0	&& End
				*!*					IF llfreeqty=.t.			&& Commented by Shrikant S. on 21/12/2018 for Installer 2.1.0
				If llfreeqty=.T. And item_vw.gpQty >0				&& Added by Shrikant S. on 21/12/2018 for Installer 2.1.0
					&& Commented by Shrikant S. on 24/12/2018 for Installer 2.1.0	&& Start
					*!*						_llnitemqty=Item_vw.Qty
					*!*						IF TYPE('Item_vw.looseqty')='N'
					*!*							_llnitemqty=_llnitemqty - Item_vw.looseqty
					*!*						endif
					&& Commented by Shrikant S. on 24/12/2018 for Installer 2.1.0	&& End
					_lnfreeqty= (_llnitemqty / item_vw.gpQty) * item_vw.FreeQTy
				Endif
			Endif
		Endif
		&& Added by Shrikant S. on 14/12/2018 for Installer 2.1.0		&& End

	Endif
	&& Added by Shrikant S. on 26/10/2018 for Bug-31942		&& End


	Sele item_vw
	If _mrprate#0
		If _mabtper#0
			&&massamt = Round((QTY*_mrprate)-(QTY*_mrprate*_mabtper)/100,2)
			*!*				massamt=QTY*Round(_mrprate-(_mabtper*_mrprate)/100,company.RateDeci) &&&Added by satish pal for bug-3466 dt.12/04/2012		&& Commented by Shrikant S. on 26/10/2018 for Bug-31942
			*!*				massamt=Iif(llfreeqty,Qty-FreeQTy,Qty)*Round(_mrprate-(_mabtper*_mrprate)/100,company.RateDeci)		&& Added by Shrikant S. on 26/10/2018 for Bug-31942		&& Commented by Shrikant S. on 14/12/2018 for Installer 2.1.0
			massamt=Iif(llfreeqty,qty-_lnfreeqty,qty)*Round(_mrprate-(_mabtper*_mrprate)/100,company.RateDeci)		&& Added by Shrikant S. on 14/12/2018 for Installer 2.1.0
		Else
			&&massamt = Round((QTY*_mrprate),2)
			*!*				massamt = Round((QTY*_mrprate),company.RateDeci) &&&Added by satish pal for bug-3466 dt.12/04/2012			&& Commented by Shrikant S. on 26/10/2018 for Bug-31942
			*!*				massamt = Round((Iif(llfreeqty,Qty-FreeQTy,Qty)*_mrprate),company.RateDeci) 			&& Added by Shrikant S. on 26/10/2018 for Bug-31942			&& Commented by Shrikant S. on 14/12/2018 for Installer 2.1.0
			massamt = Round((Iif(llfreeqty,qty-_lnfreeqty,qty)*_mrprate),company.RateDeci) 			&& Added by Shrikant S. on 14/12/2018 for Installer 2.1.0
		Endif
	Else
		****** Changed By Sachin N. S. on 28/06/2010 for TKT-2669 ****** Start
		If _vouitemassfrmname.Multi_Cur = .T.		&&vasant100111
			If Upper(Alltrim(main1_vw.Fcname)) != Upper(Alltrim(company.Currency)) And !Empty(main1_vw.Fcname)		&&vasant300609	&&vasant300609
				&&			massamt = Round(QTY*FCRATE,2)
				*!*					massamt = Round(QTY*FCRATE,company.RateDeci) &&&Added by satish pal for bug-3466 dt.12/04/2012			&& Commented by Shrikant S. on 26/10/2018 for Bug-31942
				*!*					massamt = Round(Iif(llfreeqty,Qty-FreeQTy,Qty)*FCRATE,company.RateDeci) 				&& Added by Shrikant S. on 26/10/2018 for Bug-31942			&& Commented by Shrikant S. on 14/12/2018 for Installer 2.1.0
				*!*					massamt = Round(Iif(llfreeqty,qty-_lnfreeqty,qty)*FCRATE,company.RateDeci) 				&& Added by Shrikant S. on 14/12/2018 for Installer 2.1.0	&& Commented by Shrikant S. on 24/08/2019 for AU 2.2.1
				massamt = Round((Iif(llfreeqty,qty-_lnfreeqty,qty)*FCRATE)/mrateper,company.RateDeci) 				&& Added by Shrikant S. on 24/08/2019 for AU 2.2.1
			Else
				&&			massamt = Round((QTY*RATE)/mrateper,2)  &&change by sandeep on 22/02/12 for bug 2373
				*!*					massamt = Round((QTY*RATE)/mrateper,company.RateDeci) &&&Added by satish pal for bug-3466 dt.12/04/2012			&& Commented by Shrikant S. on 26/10/2018 for Bug-31942
				*!*					massamt = Round((Iif(llfreeqty,Qty-FreeQTy,Qty)*RATE)/mrateper,company.RateDeci) 					&& Added by Shrikant S. on 26/10/2018 for Bug-31942		&& Commented by Shrikant S. on 14/12/2018 for Installer 2.1.0
				massamt = Round((Iif(llfreeqty,qty-_lnfreeqty,qty)*RATE)/mrateper,company.RateDeci) 					&& Added by Shrikant S. on 14/12/2018 for Installer 2.1.0
			Endif
		Else
			&&		massamt = Round((QTY*RATE)/mrateper,2)  &&change by sandeep on 22/02/12 for bug 2373
			*!*				massamt = Round((QTY*RATE)/mrateper,company.RateDeci) &&&Added by satish pal for bug-3466 dt.12/04/2012				&& Commented by Shrikant S. on 26/10/2018 for Bug-31942
			*!*				massamt = Round((Iif(llfreeqty,Qty-FreeQTy,Qty)*RATE)/mrateper,company.RateDeci) &&&Added by satish pal for bug-3466 dt.12/04/2012					&& Added by Shrikant S. on 26/10/2018 for Bug-31942		&& Commented by Shrikant S. on 14/12/2018 for Installer 2.1.0
			massamt = Round((Iif(llfreeqty,qty-_lnfreeqty,qty)*RATE)/mrateper,company.RateDeci) &&&Added by satish pal for bug-3466 dt.12/04/2012					&& Added by Shrikant S. on 14/12/2018 for Installer 2.1.0
		Endif
		*!*	massamt = Round(QTY*RATE,2)
		****** Changed By Sachin N. S. on 28/06/2010 for TKT-2669 ****** End
	Endif
	&&&Added by satish pal for bug-7678-Start


	&&Added by Priyanka B on 13022020 for Bug-33252 Start
	If [efabric] $ vchkprod
		If Type('item_vw.fRatePer') <> 'U'
			If item_vw.fRatePer = "MTR"
				massamt =item_vw.qty * item_vw.RATE
			Else
				massamt =item_vw.fpcs * item_vw.RATE
			Endif
		Endif
	Endif
	&&Added by Priyanka B on 13022020 for Bug-33252 End
Else
	massamt=0
Endif
&&&Added by satish pal for bug-7678-End

&& Added by Shrikant S. on 13/10/2018 for GST Incl		&& Start
If Type('Item_vw.gstIncl')='L' And Type('Lcode_vw.gstIncl')='L'
	If Lcode_vw.gstIncl=.T.
		If item_vw.gstIncl=.T.
			massamt=Iif(Type('Item_vw.gstrate')='N',Iif(item_vw.gstrate > 0 And massamt >0,(massamt/(1+(item_vw.gstrate/100))),massamt),massamt)
		Endif
	Endif
Endif
&& Added by Shrikant S. on 13/10/2018 for GST Incl		&& End

&& Added by Shrikant S. on 04/10/2016 for GST		&& Start
If Type('Item_vw.GTAXABLAMT')<>'U'
	Replace item_vw.GTAXABLAMT With massamt In item_vw
Endif
&& Added by Shrikant S. on 04/10/2016 for GST		&& End

&& added by Shrikant S. on 08/09/2017 for GST		&& Start
If main_vw.Entry_ty="RV"
	Replace RATE With U_ASSEAMT In item_vw
Endif
&& added by Shrikant S. on 08/09/2017 for GST		&& End

&& added by Shrikant S. on 12/10/2017 for GST		&& Start
If Type('Item_vw.staxamt')<>'U'
	Replace staxamt With massamt In item_vw
Endif
&& added by Shrikant S. on 12/10/2017 for GST		&& End
&&Added by Prajakta B. on 30/11/2019 for Bug 32783 &&Start
If(main_vw.Entry_ty='ST') Or (main_vw.Entry_ty='GC' And main_vw.againstgs='SALES') Or (main_vw.Entry_ty='GD' And main_vw.againstgs='SALES')
	cDate="01/08/2019"
	mdate={}
	Do Case
		Case main_vw.Entry_ty='ST'
			mdate = main_vw.Date
		Case (main_vw.Entry_ty='GC' And main_vw.againstgs='SALES') Or (main_vw.Entry_ty='GD' And main_vw.againstgs='SALES')
			If Used('othitref_vw')
				msqlstr = "SELECT [date] FROM STMAIN WHERE entry_ty=?othitref_vw.rentry_ty and tran_cd=?othitref_vw.itref_tran"
				nretval = sqlconobj.dataconn([EXE],company.dbname,msqlstr,"stmain_cur","nHandle",_Screen.ActiveForm.DataSessionId)
				If nretval <=0
					Return .F.
				Endif
				mdate = stmain_cur.Date
			Endif
	Endcase

	If Between(Ttod(mdate),Ctod(cDate),Gomonth(Ctod(cDate),24))
		msqlstr = "SELECT * FROM ac_mast WHERE ac_id=?main_vw.cons_id"
		nretval = sqlconobj.dataconn([EXE],company.dbname,msqlstr,"ac_mast_cur","nHandle",_Screen.ActiveForm.DataSessionId)
		If nretval <=0
			Return .F.
		Endif
		If ((ac_mast_cur.supp_type='Unregistered' And main_vw.gststate='Kerala' And ac_mast_cur.st_type='INTRASTATE') Or (ac_mast_cur.supp_type='Registered' And main_vw.gststate='Kerala' And ac_mast_cur.st_type='INTERSTATE'))
			If (item_vw.gstrate=3.00)
				Replace item_vw.K_CESSPER With 0.25 In item_vw
			Else
				If (item_vw.gstrate=0.00)
					Replace item_vw.K_CESSPER With 0.00 In item_vw
				Else
					Replace item_vw.K_CESSPER With 1.00 In item_vw
				Endif
			Endif
		Endif
	Endif
Endif
&&Added by Prajakta B. on 30/11/2019 for Bug 32783 &&End

Return massamt
