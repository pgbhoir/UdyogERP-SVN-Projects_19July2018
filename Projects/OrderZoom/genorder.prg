*:*****************************************************************************
*:        Program: GenOrder.PRG
*:         System: Udyog Software
*:         Author: RAGHU
*:  Last modified: 22/11/2006
*:			AIM  : Create Sales/Purchase Order Zoom-In Cursor
*:*****************************************************************************
SET STEP ON


Define Class Gen_Order As Custom
	SessionId = .F.
	ReportType = ''
	Pickup = ''
	levelcode = 0
	lError = .F.
	Sdate = {}
	edate = {}
	dateFilter = ''
	nHandle = 0
	lnear = ''
	lexac = ''
	ldele = ''
	MPara = ''
	xTraFlds = ''	&& Added By Sachin N. S. on 30/12/2008
	xTraFldsCap = ''	&& Added By Sachin N. S. on 30/12/2008
	xTraFldsOrd = ''
	ReportName = ''		&& Added By Sachin N. S. on 02/07/2010 for TKT-2644
	spl_condn = ''  &&Added by Priyanka B on 11022019 for Bug-32272


	Proce Exec_Order_Report
		If Type('This.SessionId') ='N'
			Set DataSession To (This.SessionId)
		Else
			Set DataSession To _Screen.ActiveForm.DataSessionId
		Endif


		This.Newobject('sqlconobj','sqlconnudobj','sqlconnection',xApps)
		LcItSel = ''
		LcAcSel = ''
		If Used("_lstITselected")
			LcItSel = "##"+Proper(Alltrim(musername))+[ITSEL]+Sys(2015)
			lcMGins = ""
			Select ITName As It_Name;
				FROM _lstITselected With (Buffering = .T.);
				INTO Cursor _lstselected
			If _Tally = 0
				LcItSel = ""
			Else
				Select _lstselected
				Scan
					lcMGins = This.sqlconobj.Genmultiinsert(LcItSel,'','','_lstselected','1','IT_Name')
					This.sqlconobj.setstring(lcMGins)
					Select _lstselected
				Endscan
				lcMGins = This.sqlconobj.Getstring()
				If ! Empty(lcMGins)
					lcMGCreate = "Create Table "+LcItSel+" (IT_Name Varchar(60))"
					sql_con=This.sqlconobj.dataconn("EXE",company.DbName,lcMGCreate,"","This.Parent.nHandle",This.SessionId)
					sql_con=This.sqlconobj.dataconn("EXE",company.DbName,lcMGins,"","This.Parent.nHandle",This.SessionId)
				Endif
			Endif
			Use In _lstselected
		Endif


		If Used("_lstACselected")
			LcAcSel = "##"+Proper(Alltrim(musername))+[ACSEL]+Sys(2015)
			lcMGins = ""
			Select ACName As Ac_Name;
				FROM _lstACselected With (Buffering = .T.);
				INTO Cursor _lstselected
			If _Tally = 0
				LcAcSel = ""
			Else
				Select _lstselected
				Scan
					lcMGins = This.sqlconobj.Genmultiinsert(LcAcSel,'','','_lstselected','1','Ac_Name')
					This.sqlconobj.setstring(lcMGins)
					Select _lstselected
				Endscan
				lcMGins = This.sqlconobj.Getstring()
				If ! Empty(lcMGins)
					lcMGCreate = "Create Table "+LcAcSel+" (Ac_Name Varchar(60)) "
					sql_con=This.sqlconobj.dataconn("EXE",company.DbName,lcMGCreate,"","This.Parent.nHandle",This.SessionId)
					sql_con=This.sqlconobj.dataconn("EXE",company.DbName,lcMGins,"","This.Parent.nHandle",This.SessionId)
				Endif
			Endif
			Use In _lstselected
		Endif

		
		xStr = stdcondition()
 
*!*		xStr = "EXECUTE USP_ORDER_ZOOM_IN '"+LcAcSel+"','"+LcItSel+"','',"+xStr   &&Commented by Priyanka B on 11022019 for Bug-32272
		xStr = "EXECUTE USP_ORDER_ZOOM_IN '"+LcAcSel+"','"+LcItSel+"',"+This.spl_condn+","+xStr   &&Modified by Priyanka B on 11022019 for Bug-32272

		Set Date AMERICAN
		xStr = xStr+"'"+This.ReportType+"',"+"'"+Iif(_Orstatus.zoomtype='I','I','P')+"'"+",'"+Transform(company.sta_dt)+"'"
		xStr = xStr+Iif(!Empty(This.xTraFlds),",'"+Alltrim(This.xTraFlds)+"'",",''")		&& Added By Sachin N. S. on 01/01/2008

		Set Date BRITISH
		sql_con=This.sqlconobj.dataconn("EXE",company.DbName,xStr,"_Ordzoom","This.Parent.nHandle",This.SessionId)
		If sql_con =< 0
			=Messagebox(Message(),0+16,Vumess)
			This.lError = .T.
			Return .F.
		Else
			Select _Ordzoom
			Count For ! Deleted() To m_Tot
			If m_Tot = 0
				=Messagebox("No Records Found For Display...",0+32,Vumess)
				This.lError = .T.
				Return .F.
			Endif
		Endif

		xStr = "Select [Entry_Ty],[Code_nm],PickupFrom From LCode"
		sql_con=This.sqlconobj.dataconn("EXE",company.DbName,xStr,"_LCode","This.Parent.nHandle",This.SessionId)
		If sql_con =< 0
			=Messagebox(Message(),0+16,Vumess)
			This.lError = .T.
			Return .F.
		Endif

		Select * From _LCode;
			WHERE Entry_Ty In (Select Entry_Ty From _Ordzoom);
			INTO Cursor _LCode
		xStr = "Select [Entry_Ty] From LCode Where Entry_Ty = 'TR' OR BCode_nm = 'TR'"
		sql_con=This.sqlconobj.dataconn("EXE",company.DbName,xStr,"_TRLCode","This.Parent.nHandle",This.SessionId)
		If sql_con =< 0
			=Messagebox(Message(),0+16,Vumess)
			This.lError = .T.
			Return .F.
		Endif


&&&&&ADDED BY SATISH PAL for bug-3954 dt.26/11/2012--Start
		Select _Ordzoom
		Scan
			If Entry_Ty = 'PD'
				lcSqlStr = 	" SELECT A.RENTRY_TY AS PO_ENTRY, A.ITREF_TRAN, A.RITSERIAL, A.ENTRY_TY, A.TRAN_CD, "+;
					" A.ITSERIAL, A.RQTY AS PD_RQTY, SUM(ISNULL(C.RQTY,0)) AS PO_RQTY "+;
					" FROM POITREF A "+;
					" INNER JOIN POITEM B ON (A.ENTRY_TY = B.ENTRY_TY AND A.TRAN_CD = B.TRAN_CD AND A.ITSERIAL = B.ITSERIAL)"+;
					" LEFT JOIN TRITREF C ON (C.RENTRY_TY = B.ENTRY_TY AND C.ITREF_TRAN = B.TRAN_CD AND C.RITSERIAL = B.ITSERIAL) "+;
					" WHERE A.RENTRY_TY = ?Entry_ty AND A.ITREF_TRAN = ?Tran_cd AND A.RITSERIAL = ?ItSerial "+;
					" GROUP BY A.RENTRY_TY, A.ITREF_TRAN, A.RITSERIAL, A.ENTRY_TY, A.TRAN_CD, A.ITSERIAL, A.RQTY "
				lcSqlStr = 	lcSqlStr + " UNION ALL "
				lcSqlStr = 	lcSqlStr + " SELECT A.RENTRY_TY AS PO_ENTRY, A.ITREF_TRAN, A.RITSERIAL, A.ENTRY_TY, A.TRAN_CD, "+;
					" A.ITSERIAL, 0 AS PD_RQTY, -SUM(ISNULL(A.RQTY,0)) AS PO_RQTY "+;
					" FROM TRITREF A "+;
					" WHERE A.RENTRY_TY = ?Entry_ty AND A.ITREF_TRAN = ?Tran_cd AND A.RITSERIAL = ?ItSerial "+;
					" GROUP BY A.RENTRY_TY, A.ITREF_TRAN, A.RITSERIAL, A.ENTRY_TY, A.TRAN_CD, A.ITSERIAL, A.RQTY "
				sql_con = This.sqlconobj.dataconn("EXE",company.DbName,lcSqlStr,"tmpPO_Ref","This.Parent.nHandle",This.SessionId)
				Select Sum(PD_RQTY - PO_RQTY) As RQTY From tmpPO_Ref Into Cursor CURPO_REF
				If CURPO_REF.RQTY>0
					Replace balqty With Iif(Isnull(CURPO_REF.RQTY),0,CURPO_REF.RQTY) In _Ordzoom
				Endif
				If CURPO_REF.RQTY>0
					Replace balqty With QTY-Iif(Isnull(CURPO_REF.RQTY),0,CURPO_REF.RQTY) In _Ordzoom
					Replace RQTY With QTY-Iif(Isnull(balqty),0,balqty) In _Ordzoom
				Endif
			Endif
			Select _Ordzoom
		Endscan
&&&&&ADDED BY SATISH PAL for bug-3954 dt.26/11/2012--End


		Select _TRLCode
		sql_con=This.sqlconobj.Sqlconnclose("This.Parent.nHandle")


		This.AssignPickup()

		This.FindMaxLevel()

		This.UnderLevel()

		This.Cur_ColorCode()

		Select _Ordzoom
		Select * From _Ordzoom a;
			ORDER By a.RDate,a.UnderLevel;
			INTO Cursor _Ordzoom


		Select _Ordzoom
		Go Top
	Endproc

	Function FindMaxLevel
		Select Max(a.levelcode) As MaxL From _Ordzoom a;
			INTO Cursor MaxLevel
		If _Tally <> 0
			This.levelcode = MaxLevel.MaxL+1
		Else
			This.levelcode = 1
		Endif

	Function UnderLevel
		Update _Ordzoom Set UnderLevel = Alltrim(ETI),RepType = Iif(balqty<=0,'E','P');
			WHERE RFETI = ETI
		lcPickup = This.Pickup

*	WAIT WINDOW THIS.Pickup
******** Commented By Sachin N. S. on 30/06/2010 for TKT-2324 ******** Start
*!*		IF ! EMPTY(lcPickup)
*!*			UPDATE _Ordzoom SET Balqty = 0;
*!*				WHERE INLIST(Entry_Ty,&lcPickup) = .F.
*!*		ENDIF
******** Commented By Sachin N. S. on 30/06/2010 for TKT-2324 ******** End

		For I = 1 To 50 Step 1	&&	UPDATE NLevel Group Value [Start]
			Select a.RFETI,a.ETI,a.UnderLevel,RepType,RDate;
				FROM _Ordzoom a With (Buffering = .T.);
				WHERE a.levelcode = I;
				INTO Cursor CurTopLevel
			If _Tally <> 0
				Select CurTopLevel
				Scan
					This.FindUnderGroup(CurTopLevel.ETI,CurTopLevel.UnderLevel,CurTopLevel.RepType,CurTopLevel.RDate)
					Select CurTopLevel
				Endscan
			Else
				Exit
			Endif
			Use In CurTopLevel
		Endfor	&& UPDATE NLevel Group Value [End]
		Select _Ordzoom
		= Tableupdate(.T.)
	Endfunc

	Function FindUnderGroup
		Parameters lcETI,lcUndfld,lcRepType,ldRDate
		Update _Ordzoom Set UnderLevel = Alltrim(lcUndfld)+Iif(!Empty(lcUndfld),":","")+Alltrim(ETI),RepType=lcRepType,RDate=ldRDate;
			WHERE RFETI == lcETI And RFETI <> ETI

	Function Cur_ColorCode
		Update _Ordzoom Set ColorCode = Iif(levelcode = 1,'Rgb(244,244,234)',;
			IIF(levelcode = 2,'Rgb(235,237,254)',;
			IIF(levelcode = 3,'Rgb(240,255,240)',;
			IIF(levelcode = 4,'Rgb(255,223,223)',;
			IIF(levelcode = 5,'Rgb(255,225,255)',;
			IIF(levelcode = 6,'Rgb(210,255,210)',;
			IIF(levelcode = 7,'Rgb(213,255,255)',;
			IIF(levelcode = 8,'Rgb(255,225,240)',;
			IIF(levelcode = 9,'Rgb(201,209,209)',;
			IIF(levelcode = 10,'Rgb(230,197,185)',''))))))))))
	Endfunc

	Function AssignPickup
		Select _LCode
		Scan
			If ! Empty(PickupFrom)
				lcPickupFrom = This.SetPickFrom(Alltrim(PickupFrom))
				This.Pickup = This.Pickup+lcPickupFrom
			Endif
		Endscan
		If ! Empty(This.Pickup)
			This.Pickup = Left(This.Pickup,Len(This.Pickup)-1)
		Endif
	Endfunc

	Function SetPickFrom
		Lparameters tcPickupFrom As String
		Store "" To lcPFrom,lcEntry
		tcPickupFrom = Strtran(Strtran(tcPickupFrom,"/",""),",","")
		lnTotcd = Len(tcPickupFrom)
		For lnInt = 1 To lnTotcd Step 1
			lcEntry = Substr(tcPickupFrom,lnInt-1,2)
			If !Empty(lcEntry)
				lcPFrom = [']+lcEntry+[',]
			Endif
		Endfor
		Return lcPFrom
	Endfunc

Enddefine
