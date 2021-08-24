*:*****************************************************************************
*:        Program: UpdateDebitCredit.PRG
*:         System: Udyog Software
*:         Author: RAGHU
*:  Last modified: 19/09/2006
*:			AIM  : Update OrderLevel For Account Master
*:*****************************************************************************
Lparameters mReportType

Local lnI
lnI = 1
Set Deleted On

If mReportType = 'B'
	LnLiabilityId = findgroupFlg('LIABILITIES','ID')
	Replace Ac_group_id With LnLiabilityId For Allt(Ac_name)='NET PROFIT & LOSS' And MainFlg = 'L' And Debit = 0 And Credit = 0 In _CTBAcMast
	Replace Group With 'LIABILITIES' For Allt(Ac_name)='NET PROFIT & LOSS' And MainFlg = 'L' And Debit = 0 And Credit = 0 In _CTBAcMast
Endif

ASCIIVAL = 64
*!*	Update _CTBAcMast Set Level = 1 , MaxLevel=1, OrderLevel = IncAFunction(Ac_name) Where MainFlg = 'G' And Ac_Id = Ac_group_id
=UpdtOrderLevel('_CTBAcMast')

Select _CTBAcMast
*!*	INDEX ON MainFlg+STR(Updown)+Ac_name TAG MF_Updown
Index On Str(Updown)+Ac_name Tag MF_Updown

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 40
Endif

*!*	ORDER BY a.MainFlg,a.Updown

_morderlevel = ''
If mReportType <> 'P'
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('LIABILITIES','ORDERLEVEL'),1)+[']
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('ASSETS','ORDERLEVEL'),1)+[']
Endif
If mReportType <> 'B'
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('TRADING INCOME','ORDERLEVEL'),1)+[']
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('TRADING EXPENSES','ORDERLEVEL'),1)+[']
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('MANUFACTURING INCOME','ORDERLEVEL'),1)+[']
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('MANUFACTURING EXPENSES','ORDERLEVEL'),1)+[']
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('EXPENSE','ORDERLEVEL'),1)+[']
	_morderlevel = _morderlevel+[']+Left(findgroupFlg('INCOME','ORDERLEVEL'),1)+[']
Endif
_morderlevel = Strtran(_morderlevel,[''],[','])


For I = 1 To 50 Step 1
	*!*	UPDATE NLevel Group Value [Start]
	*!*		Select a.Ac_Id,a.Ac_group_id,OrderLevel,Ac_Name,Group;		&& Added Ac_Name,Group field By Sachin N. S. on 11/07/2009
	*!*			FROM _CTBAcMast a;
	*!*			ORDER By a.MainFlg,a.Updown;
	*!*			WHERE a.Level = I And MainFlg = 'G';
	*!*			INTO Cursor CurTopLevel

	Select a.MainFlg,a.Updown,a.OrderLevel,a.Ac_name ;		&& Added Ac_Name,Group field By Sachin N. S. on 11/07/2009
	From _CTBAcMast a ;
		group By a.MainFlg,a.Updown,a.OrderLevel,a.Ac_name ;
		ORDER By a.MainFlg,a.Updown,a.OrderLevel,a.Ac_name ;
		WHERE a.Level = I And MainFlg = 'G';
		INTO Cursor CurTopLevel

	If _Tally <> 0
		Select CurTopLevel
		Scan
			*!*				=FindUnderGroup(CurTopLevel.Ac_Id,I+1,OrderLevel)	&& Commented By Sachin N. S. on 11/07/2009
			Select CurTopLevel
			=FindUnderGroup(CurTopLevel.Ac_name,I+1,CurTopLevel.OrderLevel)
			Select a.Ac_name From _CTBAcMast a ;
				Where a.Group=CurTopLevel.Ac_name And a.OrderLevel=Alltrim(CurTopLevel.OrderLevel) ;
				Into Cursor _curtemp
			If _Tally>0
				=FindUpperGroup(CurTopLevel.Ac_name,I+1,CurTopLevel.OrderLevel)
			Endif
			Select CurTopLevel
		Endscan
	Else
		lnI = Iif(I > 1,I-1,I)
		Exit
	Endif
Endfor
*!*	UPDATE NLevel Group Value [End]

If Used("CurTopLevel")
	Use In CurTopLevel
Endif

Select _CTBAcMast
Delete For Isnull(OrderLevel)

Update _CTBAcMast Set LevelFlg = Left(Alltrim(OrderLevel),1),;
	LevelINT = Val(Right(Strtran(Alltrim(OrderLevel),"/",''),Len(Alltrim(Strtran(Alltrim(OrderLevel),"/",'')))-1))

If Type('Statdesktop') = 'O'
	Statdesktop.ProgressBar.Value = 50
Endif

If mReportType = 'B'
	=FindProfitOrLoss()
Endif

=UpdateDebitCredit()

Select _CTBAcMast
Delete For (Empty(Opbal) And Empty(Debit) And Empty(Credit))

If Inlist(mReportType,'P','B')
	Update _CTBAcMast Set Level = Level - 1, MaxLevel = MaxLevel - 1 Where Level > 0
Endif

_Tally = 0
Select Distinct Level From _CTBAcMast Where Level > 0 And Inlist(Left(OrderLevel,1),&_morderlevel) Into Cursor _TBAcMast
lnI = _Tally

Select Iif(a.Level<>1,Space(a.Level*2)+a.Ac_name,a.Ac_name) As Ac_Name2, ;
	Iif(a.Level<>1,Space((a.Level+1)*2),Space(a.Level*2))+Alltrim(a.Co_Name)+Space(150) As Ac_Name3, ;
	a.* ;
	FROM _CTBAcMast a ;
	ORDER By a.LevelFlg,a.OrderLevel,a.Ac_group_id ;
	INTO Cursor _TBAcMast

Select _TBAcMast
Go Top

Return lnI


************ Changed By Sachin N. S. on 11/07/2009 ************ Start
*!*	Function FindUnderGroup
*!*	***********************
*!*		Parameters gAc_Id,m_Level,mOrderLevel
*!*		IVAL = 0
*!*		Select _CTBAcMast
*!*		Set Filter To Ac_group_id = gAc_Id And Level = 0
*!*		Set Order To MF_Updown
*!*		Scan
*!*			Replace Level With m_Level
*!*			Replace OrderLevel With IncNFunction(mOrderLevel)
*!*			Select _CTBAcMast
*!*		Endscan
*!*		Select _CTBAcMast
*!*		Set Filter To


Function FindUnderGroup
	***********************
	Parameters gAc_Name,m_Level,mOrderLevel
	IVAL = 0
	Select _CTBAcMast
	*!*		Set Filter To Group = gAc_Name And Level = 0
	Set Order To MF_Updown
	Scan For Group = gAc_Name And Level = 0
		Select _CTBAcMast
		nRecno=Iif(!Eof(),Recno(),0)
		cAcName = _CTBAcMast.Ac_name
		If _CTBAcMast.Level=0
			_morderlevel1=IncNFunction(mOrderLevel)
			Replace OrderLevel With _morderlevel1 For Ac_name=cAcName And Group = gAc_Name In _CTBAcMast
			Replace Level With m_Level, MaxLevel With m_Level For Ac_name=cAcName And Group = gAc_Name In _CTBAcMast
		Endif
		If nRecno!=0
			Go nRecno
		Endif
		Select _CTBAcMast
	Endscan
	Select _CTBAcMast
	Set Filter To
EndFunc

Function FindUpperGroup
	***********************
	Parameters gAc_Name,m_Level,mOrderLevel
	IVAL = 0
	mOrderLevel=Alltrim(mOrderLevel)
	Select _CTBAcMast
	Set Order To MF_Updown
	Do While .T.
		Select _CTBAcMast
		Go Top
		Replace MaxLevel With Iif(MaxLevel>=m_Level,MaxLevel,MaxLevel+1) For OrderLevel=mOrderLevel+Replicate(' ',100-Len(mOrderLevel)) In _CTBAcMast
		_morderlevel1 = At('/',mOrderLevel)
		If _morderlevel1>0
			mOrderLevel = Left(mOrderLevel,_morderlevel1-1)
		Else
			Replace MaxLevel With Iif(MaxLevel>=m_Level,MaxLevel,MaxLevel+1) For OrderLevel=mOrderLevel+Replicate(' ',100-Len(mOrderLevel)) In _CTBAcMast
			Exit
		Endif
	Enddo
	Select _CTBAcMast
	Set Filter To
EndFunc
	************ Changed By Sachin N. S. on 11/07/2009 ************ End

Function IncAFunction
	********************
	Lparameters lcAc_Name
	*!*	*!*	ASCIIVAL = ASCIIVAL + 1
	Do Case
		Case Upper(lcAc_Name) = 'LIABILITIES'
			ASCIIVAL = 65
		Case Upper(lcAc_Name) = 'ASSETS'
			ASCIIVAL = 66
		Case Upper(lcAc_Name) = 'INCOME'
			ASCIIVAL = 67
		Case Upper(lcAc_Name) = 'EXPENSE'
			ASCIIVAL = 68
		Case Upper(lcAc_Name) = 'TRADING INCOME'
			ASCIIVAL = 69
		Case Upper(lcAc_Name) = 'TRADING EXPENSES'
			ASCIIVAL = 70
		Case Upper(lcAc_Name) = 'MANUFACTURING INCOME'
			ASCIIVAL = 71
		Case Upper(lcAc_Name) = 'MANUFACTURING EXPENSES'
			ASCIIVAL = 72
	Endcase
	Return Chr(ASCIIVAL)
EndFunc

Function IncNFunction
	*********************
	Parameters mOrderLevel
	IVAL = IVAL + 1
	IncFun = Alltrim(mOrderLevel)+'/'+Replicate('0',7-Len(+Alltrim(Str(IVAL))))+Alltrim(Str(IVAL))
	Return IncFun
EndFunc

Function FindProfitOrLoss
	*************************
	Mtincomeflg  = findgroupFlg('TRADING INCOME','FLAG')
	Mtexpenseflg = findgroupFlg('TRADING EXPENSES','FLAG')
	Mexpenseflg  = findgroupFlg('EXPENSE','FLAG')
	Mincomeflg   = findgroupFlg('INCOME','FLAG')
	Mmincomeflg  = findgroupFlg('MANUFACTURING INCOME','FLAG')
	Mmexpenseflg = findgroupFlg('MANUFACTURING EXPENSES','FLAG')

	Store 0 To PandLGp,mTrdIncome,mTrdExpense,mExpense,mIncome,mMfgIncome,mMfgExpense

	&& Find GP
	Select a.DbName,Sum(Iif(Inlist(a.LevelFlg,Mtexpenseflg,Mmexpenseflg,Mexpenseflg),Iif(Isnull(a.ClBal),0,a.ClBal),a.ClBal*0)) As Expense,;
		Sum(Iif(Inlist(a.LevelFlg,Mtincomeflg,Mmincomeflg,Mincomeflg),Iif(Isnull(a.ClBal),0,-a.ClBal),a.ClBal*0)) As Income ;
		FROM _CTBAcMast a;
		WHERE Inlist(a.LevelFlg,Mtexpenseflg,Mtincomeflg,Mmexpenseflg,Mmincomeflg,Mexpenseflg,Mincomeflg);
		GROUP By a.DbName ;
		INTO Cursor CurTrdIncome

	Update a Set ;
		a.Debit  = Iif(b.Expense>b.Income,b.Expense-b.Income,0), ;
		a.Credit = Iif(b.Income>b.Expense,Abs(b.Income-b.Expense),0), ;
		a.ClBal  = Iif(b.Expense>b.Income,b.Expense-b.Income,0)-Iif(b.Income>b.Expense,Abs(b.Income-b.Expense),0) ;
		From _CTBAcMast a ;
		inner Join CurTrdIncome b On a.DbName=b.DbName ;
		WHERE Allt(a.Ac_name)=='NET PROFIT & LOSS' And a.MainFlg = 'L'
	Select _CTBAcMast
	Go Top
EndFunc

Function findgroupFlg
	********************
	Parameters mAc_Name,tcType

	_MlcLevelFld = tcType
	Do Case
		Case tcType='ID'
			_MlcLevelFld = 'Ac_id'
		Case tcType='FLAG'
			_MlcLevelFld = 'LevelFlg'
	Endcase
	Select &_MlcLevelFld ;
		FROM _CTBAcMast ;
		WHERE Ac_name = mAc_Name;
		INTO Cursor FindLevelFlg
	Go Top
	lcLevelFlg = &_MlcLevelFld

	If Used('FindLevelFlg')
		Use In FindLevelFlg
	Endif
	Return lcLevelFlg
EndFunc

Function UpdtOrderLevel
	Lparameters _lTblAcmast
	Set ENGINEBEHAVIOR 70
	Select Ac_name,Ac_Id, Updown, Iif(Upper(Ac_name)='LIABILITIES','A',Iif(Upper(Ac_name)='ASSETS','B',Iif(Upper(Ac_name)='INCOME','C', ;
		IIF(Upper(Ac_name)='EXPENSE','D',Iif(Upper(Ac_name)='TRADING INCOME','E',Iif(Upper(Ac_name)='TRADING EXPENSES','F', ;
		IIF(Upper(Ac_name)='MANUFACTURING INCOME','G',Iif(Upper(Ac_name)='MANUFACTURING EXPENSES','H','I')))))))) As LevelOrd ;
	From (_lTblAcmast) ;
			Where MainFlg = 'G' And Ac_Id = Ac_group_id And Empty(OrderLevel) ;
		GROUP By Ac_name ;
		Order By Updown, LevelOrd Into Cursor _curr1
	Set ENGINEBEHAVIOR 90

	ASCIIVAL = 65
	Select _curr1
	Scan
		Select _curr1
		Update (_lTblAcmast) Set Level = 1 , OrderLevel = Chr(ASCIIVAL) Where MainFlg = 'G' And Ac_name=_curr1.Ac_name

		ASCIIVAL = ASCIIVAL+1
		Select _curr1
	Endscan

Endfunc
