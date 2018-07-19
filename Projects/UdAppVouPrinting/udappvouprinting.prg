Lparameters _prinpara
_actfrm = _Screen.ActiveForm
_actprintflag = 0
If Type('printflag') = 'N'
	_actprintflag = printflag
Endif

If _prinpara = 'AFTER' And _actprintflag = 4
	nHandle = 0
	_mailto = ''
	_actfrmcon = _actfrm.SqlConObj.DataConn([EXE],Company.DbName,;
		"Select email from ac_mast where ac_id = ?Main_vw.Ac_id",[TmpPrt_Vw],"nHandle",_actfrm.DataSessionId,.F.)
	If _actfrmcon > 0 And Used('TmpPrt_Vw')
		Select TmpPrt_Vw
		If Reccount() >= 1
			_mailto = TmpPrt_Vw.email
		Endif
	Endif
	_mins = 1
	_mins1 = 1 && Add Rupesh G for bug no.30878
	_mailatt =''
	
	&& Comment start Rupesh G for bug no.30878
	&&For _mins = 1 To _mine
*!*	&&Commented by Amrendra on 21-06-2011 for TKT 7054       Start
*!*			mdoreponm = Alltrim(Iif(!Empty(aprintarr(_mins,6)),Evaluate(aprintarr(_mins,6)),Strtran(Upper(mdorepo),'.RPT','')))+'.PDF'
*!*			_mailatt   = Alltrim(coadditional.pdf_path)+'\'+mdoreponm
*!*	&&Commented by Amrendra on 21-06-2011 for TKT 7054       End
&&Added by Amrendra on 21-06-2011 for TKT 7054 Start
		&&mdoreponm = Alltrim(aprintarr(_mins,2))+'.PDF'
		&&_mailatt   =_mailatt  + Iif(Empty(mdoreponm),'',Iif(!Empty(_mailatt),';','')+Alltrim(coadditional.pdf_path)+'\'+mdoreponm)
&&Added by Amrendra on 21-06-2011 for TKT 7054  End
	&&	_mailsub =  mdoreponm
	&&	_mailbody = ''
*_mailbody = "Test Mail"+chr(13)+chr(13)+;
"Thanks,"+chr(13)+chr(13)+mUsername
*	Do UeMailing With _mailto,"",_mailsub,_mailbody,_mailatt		 &&Commented by Amrendra on 21-06-2011 for TKT 7054
	&&ENDFOR
	&& Comment End Rupesh G for bug no.30878

***** Added by Sachin N. S. on 18/09/2015 for Bug-26664 -- Start
	llExit=.F.

	msqlstr="Delete from vudyog..ExtApplLog where calldate+7<getdate()"
	nretval=_actfrm.SqlConObj.DataConn('EXE',Company.DbName,msqlstr,"_rExtAppTbl","nHandle1",_actfrm.DataSessionId)
	If nretval<0
		Return .F.
	Endif
	
*!*		MESSAGEBOX("s1")
*!*		MESSAGEBOX(pApplCode)
	Do While .T.
		msqlstr="select cApplNm,cApplId,cApplDesc from vudyog..ExtApplLog where pApplCode='"+pApplCode+"'"
		nretval=_actfrm.SqlConObj.DataConn('EXE',Company.DbName,msqlstr,"_rExtAppTbl","nHandle1",_actfrm.DataSessionId)
		If nretval<0
			Return .F.
		ENDIF
		llExit=.F.
		nretval=_actfrm.SqlConObj.sqlconnclose("nHandle1")
		oWMI = Getobject('winmgmts://')
		If Used("_rExtAppTbl")
			Select _rExtAppTbl
			Go Top
			Locate
			SCAN
*!*					MESSAGEBOX(Alltrim(Str(_rExtAppTbl.cApplId)))
				cQuery = "select * from win32_process where ProcessId="+Alltrim(Str(_rExtAppTbl.cApplId))+" and Name='udPdfSignature.exe'"
				oResult = oWMI.ExecQuery(cQuery)
				If oResult.Count>0
*!*						statdesktop.Message = [Busy Mode....]
*!*						=beep(600,200)
*!*						statdesktop.Message = [User :]+musername
					llExit=.T.
					Exit
				Else
					llExit=.F.
				Endif
			Endscan
		Endif
		If !llExit
			Exit
		Endif
	Enddo
***** Added by Sachin N. S. on 18/09/2015 for Bug-26664 -- End

	&& Add Rupesh G. Bug no.30878--Start

	mailMode=''
	If _mine > 1
		eMessageTitle = 'My Application'
		eMessageText = "Do you want to Send PDF's as a single mail or as different mails?"+Chr(13)+"Click 'Yes' to send single mail or 'No' to send as different mail."
		nDialogType = 4 + 32
		*  4 = Yes and No buttons
		*  16 = Stop sign icon
		nAnswer = Messagebox(eMessageText, nDialogType, eMessageTitle)
		Do Case
			Case nAnswer = 6
				mailMode='1'
			Case nAnswer = 7
				mailMode='2'
		Endcase
	Else
		mailMode='2'
	Endif

	For _mins = 1 To _mine
		mdoreponm = Alltrim(Iif(!Empty(aprintarr(_mins,6)),Evaluate(aprintarr(_mins,6)),Strtran(Upper(mdorepo),'.RPT','')))

		&&Rupesh G Start
		mdoreponm =Strtran(mdoreponm ,"/","_")
		mdoreponm =Strtran(mdoreponm ,"\","_")
		mdoreponm =Strtran(mdoreponm ,":","_")
		mdoreponm =Strtran(mdoreponm ,"*","_")
		mdoreponm =Strtran(mdoreponm ,"?","_")
		mdoreponm =Strtran(mdoreponm ,"<","_")
		mdoreponm =Strtran(mdoreponm ,">","_")
		mdoreponm =Strtran(mdoreponm ,"|","_")

		If mailMode=='1'
			For  _mins1 = 1 To _mine1
				If Alltrim(aprintarr(_mins,2))= Alltrim(copyfilearr(_mins1,1))
					mdoreponm=copyfilearr(_mins1,2)
					_mailatt   =_mailatt+Iif(Empty(mdoreponm),'',Iif(!Empty(_mailatt),';','')+Alltrim(coadditional.pdf_path)+Alltrim(mdoreponm))+".pdf"
					vEmailSub=Alltrim(Iif(!Empty(aprintarr(1,7)),aprintarr(1,7),"")) 
					vEmailBody=Alltrim(Iif(!Empty(aprintarr(1,8)),aprintarr(1,8),"")) 
				Endif
			ENDFOR
			
		Else
			For  _mins1 = 1 To _mine1
				If Alltrim(aprintarr(_mins,2))= Alltrim(copyfilearr(_mins1,1))
					mdoreponm=copyfilearr(_mins1,2)
					_mailatt   =_mailatt+Iif(Empty(mdoreponm),'',Iif(!Empty(_mailatt),';','')+Alltrim(coadditional.pdf_path)+Alltrim(mdoreponm))+".pdf"
					vEmailSub=Alltrim(Iif(!Empty(aprintarr(_mins,7)),aprintarr(_mins,7),"")) 
					vEmailBody=Alltrim(Iif(!Empty(aprintarr(_mins,8)),aprintarr(_mins,8),"")) 
					ENDIF
					
				ENDFOR
				

			ENDIF
			

			vEmailSub=Iif(!Empty(vEmailSub),Evaluate(vEmailSub),"")
			_mailsub = vEmailSub

			vEmailBody=Iif(!Empty(vEmailBody),Evaluate(vEmailBody),"") 
			_mailbody = vEmailBody
			
			
			If mailMode=='2'			
				Do UeMailing With _mailto,"",_mailsub,_mailbody,_mailatt		
				_mailatt=''
			Endif
		ENDFOR		

		If mailMode=='1'
			
			Do UeMailing With _mailto,"",_mailsub,_mailbody,_mailatt		
		Endif
		
		&& Add Rupesh G. Bug no.30878--End
		
	&&Do UeMailing With _mailto,"",_mailsub,_mailbody,_mailatt		&&Added by Amrendra on 21-06-2011 for TKT 7054 && Comment Rupesh G. for bug no 30878
Endif
