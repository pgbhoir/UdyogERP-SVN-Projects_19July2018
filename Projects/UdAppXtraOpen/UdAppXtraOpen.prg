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
		_transcnt= RECCOUNT('_trans')
		SELECT _trans
		locate
		if _transcnt=1
			replace defa_val WITH _trans.NotiSrNo IN Lother
		endif
		
		lcaddicond=""
		Select _Trans
		Scan
			lcaddicond=lcaddicond+","+Alltrim(_Trans.NotiSrNo)
		Endscan
			
		Replace filtcond With lcaddicond In lother
		IF !EMPTY(lcaddicond)
			Replace mandatory With '.t.',val_err WITH '"'+ALLTRIM(Lother.head_nm) +' cannot be empty."' In lother
		ELSE
			Replace whn_con With '.f.' In lother
		endif
	Endif
	Select lother
	Locate For Upper(Alltrim(fld_nm))=='SEXNOTISL'
	If Found()
		IF EMPTY(Item_vw.sexnotisl)
			Replace filtcond With " " IN Lother
		ELSE
			Replace filtcond With Item_vw.sexnotisl IN Lother
		ENDIF
		replace mandatory WITH "IIF(!EMPTY(Item_vw.SEXNOTI),.t.,.f.)",val_err WITH '"'+ALLTRIM(Lother.head_nm) +' cannot be empty."' In lother
	Endif
	Select lother
	If _nrecno!=0
		Go _nrecno
	Endif
Endif
&& Added by Shrikant S. on 13/10/2017 for GST		&& Exemption  End	Bug-30581

