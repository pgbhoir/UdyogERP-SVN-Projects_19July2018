****** Added By Sachin N. S. on 30/09/2011 for TKT-9711 ****** Start
_oForm = _Screen.ActiveForm
If ([vuexc] $ vchkprod)
	If Uppe(Allt(wTable))=Upper([Main_Vw])
		Select lother
		_nrecno=Iif(!Eof(),Recno(),0)
		Locate For Upper(Alltrim(fld_nm))='EXMCLEARTY'
		If Found()
			sq1= "SELECT ExMClearTy FROM [Rules] where [rule] = ?Main_vw.Rule "
			nRetval = _oForm.sqlconobj.dataconn([EXE],company.dbname,sq1,"_trans","thisform.nHandle",_oForm.DataSessionId)
			If nRetval<0
				Return .F.
			Endif
			nRetval = _oForm.sqlconobj.sqlconnclose("thisform.nHandle")
			If nRetval<0
				Return .F.
			Endif

			Replace filtcond With _Trans.ExMClearTy In lother
			_tblnm = Iif('LMC' $ Upper(Alltrim(lother.tbl_nm)),'LMC_VW','MAIN_VW')
			If Type(_tblnm+'.ExMClearTy')='C'
				If Empty(&_tblnm..ExMClearTy)
					Replace ExMClearTy With Left(_Trans.ExMClearTy,At(',',_Trans.ExMClearTy)-1) In (_tblnm)
				Endif
			Endif
		Endif
		Select lother
		If _nrecno!=0
			Go _nrecno
		Endif
	Endif
Endif
****** Added By Sachin N. S. on 30/09/2011 for TKT-9711 ****** End


&& Added by Shrikant S. on 13/10/2017 for GST		&& Exemption  Start Bug-30581
If Uppe(Allt(wTable))=Upper([Item_Vw]) And  (_oForm.addmode Or _oForm.editmode)
	Select lother
	_nrecno=Iif(!Eof(),Recno(),0)
	Locate For Upper(Alltrim(fld_nm))=='SEXNOTI'
	If Found()
&& Commented by Shrikant S. on 13/12/2017 for GST Auto udpater 13.0.5		&& Start
*!*			lcagainstgs=lcode_vw.isservitem
*!*			If (Inlist(_oForm.pcvtype,"BP","BR","CP","CR") Or Inlist(_oForm.behave,"BP","BR","CP","CR"))
*!*				If Type('main_vw.againstgs')<>'U'
*!*					If main_vw.againstgs="SERVICES"
*!*						lcagainstgs=.T.
*!*					Endif
*!*				Endif
*!*			Endif
&& Commented by Shrikant S. on 13/12/2017 for GST Auto udpater 13.0.5		&& End

		lcagainstgs=isservItem()			&& Added by Shrikant S. on 13/12/2017 for GST Auto udpater 13.0.5
		If lcagainstgs=.T.
			mSqlCondn=" Where ac_id=?Main_vw.ac_id and Serty=(Select top 1 a.[name] as Serty From sertax_mast a inner join it_mast b ON(a.[name]=b.[serty]) Where b.it_code=?Item_vw.It_code and ?Main_vw.Date Between Sdate and Edate )"
		Else
*!*				mSqlCondn=" Where ac_id=?Main_vw.ac_id and Serty='GOODS' and Exists(Select validity from ServiceTaxNotifications  Where AGAINSTGS='GOODS' OR VALIDITY= (Select TOP 1 hsncode from it_mast where it_code=?Item_vw.It_code)) "		&& Commented by Shrikant S. on 17/11/2017 for GST Bug-30581
			mSqlCondn=" Where ac_id=?Main_vw.ac_id and Serty='GOODS' and  Notisrno in (Select Noti_no from ServiceTaxNotifications  Where AGAINSTGS='GOODS' AND (VALIDITY='Any Chapter' or VALIDITY= (Select TOP 1 hsncode from it_mast where it_code=?Item_vw.It_code)) ) "		&& Added by Shrikant S. on 17/11/2017 for GST Bug-30581
		Endif

		Do Case
			Case Inlist(Alltrim(_oForm.accregistatus),"EOU","SEZ","Export","Import")
				mSqlCondn=mSqlCondn+ " and Notisrno Not like '%Central Tax%'"
			Case _oForm.taxapplarea="INTRASTATE"
				mSqlCondn=mSqlCondn+ " and Notisrno like '%Central Tax%'"
			Case Inlist(_oForm.taxapplarea,"INTERSTATE","OUT OF COUNTRY")
				mSqlCondn=mSqlCondn+ " and Notisrno Not like '%Central Tax%'"
		Endcase

		sq1= " Select NotiSrNo From Ac_mast_Serv "+mSqlCondn+" Group by NotiSrNo Order by NotiSrNo "

		nRetval = _oForm.sqlconobj.dataconn([EXE],company.dbname,sq1,"_trans","thisform.nHandle",_oForm.DataSessionId)
		If nRetval<0
			Return .F.
		Endif
		nRetval = _oForm.sqlconobj.sqlconnclose("thisform.nHandle")
		If nRetval<0
			Return .F.
		Endif
		_transcnt= Reccount('_trans')
		Select _Trans
		Locate
		If _transcnt=1
			Replace defa_val With _Trans.NotiSrNo In lother
		Endif

		lcaddicond=""
		Select _Trans
		Scan
			lcaddicond=lcaddicond+","+Alltrim(_Trans.NotiSrNo)
		Endscan

		Replace filtcond With lcaddicond In lother
		If !Empty(lcaddicond)
			Replace mandatory With '.t.',val_err With '"'+Alltrim(lother.head_nm) +' cannot be empty."' In lother
		Else
			Replace whn_con With '.f.' In lother
		Endif
	Endif
	Select lother
	Locate For Upper(Alltrim(fld_nm))=='SEXNOTISL'
	If Found()
		If Empty(Item_vw.sexnotisl)
			Replace filtcond With " " In lother
		Else
			Replace filtcond With Item_vw.sexnotisl In lother
		Endif
		Replace mandatory With "IIF(!EMPTY(Item_vw.SEXNOTI),.t.,.f.)",val_err With '"'+Alltrim(lother.head_nm) +' cannot be empty."' In lother
	Endif
	Select lother
	If _nrecno!=0
		Go _nrecno
	Endif
Endif
&& Added by Shrikant S. on 13/10/2017 for GST		&& Exemption  End	Bug-30581

&&Added by Priyanka B on 04042019 for Bug-32067 Start
If Vartype(oGlblPrdFeat)='O'
	If oGlblPrdFeat.UdChkProd('vuisd') Or oGlblPrdFeat.UdChkProd('isdkgen')
		If Used("Lother") And Uppe(Allt(wTable))=Upper([Main_Vw])
			Select lother
			Do Case
				Case Inlist(main_vw.entry_ty,"GC","GD")
					If Alltrim(Upper(main_vw.againstgs))="PURCHASES"
						Update lother Set filtcond=',Domestic Purchases,Branch Transfer,SEZ Developer Purchase,SEZ Unit Purchase';
							Where e_code In ('GC','GD') And Alltrim(Lower(fld_nm))='u_imporm'
					Else
						If Alltrim(Upper(main_vw.againstgs))="ISD INVOICE PASSING"
							Update lother Set filtcond=',Invoice Credit Mismatch,Redistribution of ITC distributed to a wrong recipient';
								Where e_code In ('GC','GD') And Alltrim(Lower(fld_nm))='u_imporm'
						Endif
					Endif

				Case main_vw.entry_ty="IB"
					Update lother Set filtcond=',Invoice Credit Mismatch,Redistribution of ITC distributed to a wrong recipient';
						Where e_code='IB' And Alltrim(Lower(fld_nm))='u_imporm'

				Case main_vw.entry_ty="PT"
					Update lother Set filtcond=',Branch Transfer,Consignment Transfer,Domestic Purchases,SEZ Developer Purchase,SEZ Unit Purchase';
						Where e_code='PT' And Alltrim(Lower(fld_nm))='u_imporm'
			Endcase
			Select lother
			Go Top
		Endif
	Endif
Endif
&&Added by Priyanka B on 04042019 for Bug-32067 End
