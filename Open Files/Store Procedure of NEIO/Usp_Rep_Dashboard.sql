If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Rep_Dashboard')
Begin
	Drop Procedure Usp_Rep_Dashboard
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[Usp_Rep_Dashboard]
@CompCd Varchar(6),@Grp Varchar(10),@vPeriod Varchar(300)
As
Begin
	--Execute[Usp_Rep_Dashboard] 1,'STL2',' MnthNo in (202005,202004,202003,202002,202001,201912)'
	Declare @sqlcommand nvarchar(4000),@OBMonth varchar(10)	--Divyang
	Set @sqlcommand=''
	if @Grp='ST'
	Begin
		set @sqlcommand='Select bCode_Nm=''ST'',Inv_val=Sum(Case When (bCode_Nm in (''SR'',''CN'')) Then -Inv_Val Else Inv_Val End ) From DSB_Trn_Tot_L0 Where  (bCode_Nm in (''ST'',''SR'')  or substring(entry_ty,3,2)=''ST'') and '+@vPeriod --Group By bCode_Nm
	End
	if @Grp='STL1'
	Begin
		set @sqlcommand='Select Entry_Ty=''ST'',Mnth,MnthNo,Qty=Sum(Case When (bCode_Nm in (''SR'',''CN'')) Then -Qty Else Qty End ),[Value]=Sum(Case When (bCode_Nm in (''SR'',''CN'')) Then -Inv_Val Else Inv_Val End ),LastUpdtOn From DSB_Trn_Tot_L0 Where  (bCode_Nm in (''ST'',''SR'')  or substring(entry_ty,3,2)=''ST'') and '+@vPeriod +' Group By Mnth,MnthNo,LastUpdtOn  Order By MnthNo'
	End
	if @Grp='STL2'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'STL2', 'MnthNo=202001'
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd From DSB_ST Where ' + @vPeriod 
		set @sqlcommand=@sqlcommand +' union Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd From DSB_SR Where ' + @vPeriod 
		set @sqlcommand=@sqlcommand +' union Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd From DSB_DN Where ' + @vPeriod + ' and substring(entry_ty,3,2)=''ST'''
		set @sqlcommand=@sqlcommand +' union Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd From DSB_CN Where ' + @vPeriod + ' and substring(entry_ty,3,2)=''ST'' Order By MnthNo,Inv_Dt,Inv_No,Code_Nm'
	End
	if @Grp='PT'
	Begin
		set @sqlcommand='Select bCode_Nm=''PT'',Inv_val=Sum(Case When (bCode_Nm in (''PR'',''DN'')) Then -Inv_Val Else Inv_Val End ) From DSB_Trn_Tot_L0 Where  (bCode_Nm in (''PT'',''PR'')  or substring(entry_ty,3,2)=''PT'') and '+@vPeriod --Group By bCode_Nm
	End
	if @Grp='PTL1'
	Begin
		set @sqlcommand='Select Entry_Ty=''PT'',Mnth,MnthNo,Qty=Sum(Case When (bCode_Nm in (''PR'',''DN'')) Then -Qty Else Qty End ),[Value]=Sum(Case When (bCode_Nm in (''PR'',''DN'')) Then -Inv_Val Else Inv_Val End ),LastUpdtOn From DSB_Trn_Tot_L0 Where  (bCode_Nm in (''PT'',''PR'')  or substring(entry_ty,3,2)=''PT'') and '+@vPeriod +' Group By Mnth,MnthNo,LastUpdtOn  Order By MnthNo'
	End
	if @Grp='PTL2'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'PTL2', 'MnthNo=202001'
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Bill_No,Bill_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd From DSB_PT Where ' + @vPeriod 
		set @sqlcommand=@sqlcommand +' union Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Bill_No=Inv_No,Bill_Dt=Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd From DSB_PR Where ' + @vPeriod 
		set @sqlcommand=@sqlcommand +' union Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Bill_No=Inv_No,Bill_Dt=Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd From DSB_DN Where ' + @vPeriod + ' and substring(entry_ty,3,2)=''PT'''
		set @sqlcommand=@sqlcommand +' union Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Bill_No=Inv_No,Bill_Dt=Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd From DSB_CN Where ' + @vPeriod + ' and substring(entry_ty,3,2)=''PT'' Order By MnthNo,Inv_Dt,Inv_No,Code_Nm'
	End
	if @Grp='SR'
	Begin
		set @sqlcommand='Select bCode_Nm=''SR'',Inv_val=Sum(Inv_Val) From DSB_Trn_Tot_L0 Where  (bCode_Nm in (''SR'')  or (substring(entry_ty,3,2)=''ST'' and bCode_Nm=''CN'')  ) and '+@vPeriod --Group By bCode_Nm
	End
	if @Grp='SRL1'
	Begin
		set @sqlcommand='Select Entry_Ty=''SR'',Mnth,MnthNo,Qty=Sum(QTY),[Value]=Sum(Inv_Val),LastUpdtOn From DSB_Trn_Tot_L0 Where  (bCode_Nm in (''SR'')  or substring(entry_ty,3,2)=''ST'') and '+@vPeriod +' Group By Mnth,MnthNo,LastUpdtOn  Order By MnthNo'
	End
	if @Grp='SRL2'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'SRL2', 'MnthNo=202001'
		--Execute[Usp_Rep_Dashboard] 1,'SRL2', 'MnthNo=202002'
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,Reason From DSB_SR Where ' + @vPeriod 
		set @sqlcommand=@sqlcommand +' union Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,Reason From DSB_DN Where ' + @vPeriod + ' and substring(entry_ty,3,2)=''ST'''
		set @sqlcommand=@sqlcommand +' union Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,Reason From DSB_CN Where ' + @vPeriod + ' and substring(entry_ty,3,2)=''ST'' Order By MnthNo,Inv_Dt,Inv_No,Code_Nm'
	End
	if @Grp='PR'
	Begin
		set @sqlcommand='Select bCode_Nm=''PR'',Inv_val=Sum(Inv_Val) From DSB_Trn_Tot_L0 Where  (bCode_Nm in (''PR'')  or (substring(entry_ty,3,2)=''PT'' and bCode_Nm=''DN'')  ) and '+@vPeriod --Group By bCode_Nm
	End
	if @Grp='PRL1'
	Begin
		set @sqlcommand='Select Entry_Ty=''PR'',Mnth,MnthNo,Qty=Sum(Qty),[Value]=Sum(Inv_Val),LastUpdtOn From DSB_Trn_Tot_L0 Where  (bCode_Nm in (''PR'')  or substring(entry_ty,3,2)=''PT'') and '+@vPeriod +' Group By Mnth,MnthNo,LastUpdtOn  Order By MnthNo'
	End
	if @Grp='PRL2'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'PRL2', 'MnthNo=202001'
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,Reason From DSB_PR Where ' + @vPeriod 
		set @sqlcommand=@sqlcommand +' union Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,Reason From DSB_DN Where ' + @vPeriod + ' and substring(entry_ty,3,2)=''PT'''
		set @sqlcommand=@sqlcommand +' union Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,Reason From DSB_CN Where ' + @vPeriod + ' and substring(entry_ty,3,2)=''PT'' Order By MnthNo,Inv_Dt,Inv_No,Code_Nm'
	End
	if (@sqlcommand='SO')
	Begin
		set @sqlcommand='Select bCode_Nm='+char(39)+@Grp+char(39)+',Inv_val=Sum(Inv_val) From DSB_Trn_Tot_L0 Where  (bCode_Nm in (''SO'') ) and '+@vPeriod --Group By bCode_Nm
	End
	if @Grp='SOL1'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'POL1', 'MnthNo=202001'
		--set @sqlcommand='Select Mnth,Code_Nm,[Qty]=Qty,[Issue Qty]=Iss_Qty,[Bal Qty]=Bal_Qty,[Value]=Inv_Val,LastUpdtOn,MnthNo From DSB_Mnth_ENt_Tot_L1 Where (bCode_Nm in (''SO'') ) and '+@vPeriod  +' Order by MnthNo';--Comment RRG
		set @sqlcommand='Select Mnth,Code_Nm,[Qty]=sum(Qty),[Issue Qty]=sum(Iss_Qty),[Bal Qty]=sum(Bal_Qty),[Value]=sum(Inv_Val),LastUpdtOn,MnthNo From DSB_Mnth_ENt_Tot_L1 Where (bCode_Nm in (''SO'') ) and '+@vPeriod  +' Group By Code_Nm,Mnth,MnthNo,LastUpdtOn Order by MnthNo';--Add Rupesh G
	End
	if @Grp='SOL2'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'POL1', 'MnthNo=202001'
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Iss_Qty,Bal_Qty,Inv_Val,Ord_Status,LastUpdtOn,Entry_TY,Tran_Cd From DSB_SO Where ' + @vPeriod + ' Order By MnthNo,Code_Nm,Inv_Dt,Inv_No';
	End
	if (@Grp='SOP')
	Begin
		Print 'R1'
		--Execute[Usp_Rep_Dashboard] 1,'SOP',' MnthNo in (202005,202004,202003,202002,202001,201912)'
		set @sqlcommand='Select bCode_Nm='+char(39)+@Grp+char(39)+',Bal_It_Tot=Sum(Bal_It_Tot) From DSB_Trn_Tot_L0 Where  (bCode_Nm in (''SO'') ) and Bal_Qty>0 and '+@vPeriod --Group By bCode_Nm
	End
	if @Grp='SOPL1'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'POL1', 'MnthNo=202001'
		set @sqlcommand='Select Mnth,Code_Nm,[Bal Qty]=sum(Bal_Qty),[Value]=sum(Bal_It_Tot),LastUpdtOn,MnthNo From DSB_Mnth_ENt_Tot_L1 Where (bCode_Nm in (''SO'') ) and Bal_Qty>0  and '+@vPeriod  +' Group By Code_Nm,Mnth,MnthNo,LastUpdtOn Order by MnthNo';--RRG
		--set @sqlcommand='Select Mnth,Code_Nm,[Bal Qty]=Bal_Qty,[Value]=Bal_It_Tot,LastUpdtOn,MnthNo From DSB_Mnth_ENt_Tot_L1 Where (bCode_Nm in (''SO'') ) and Bal_Qty>0  and '+@vPeriod  +'  Order by MnthNo';--RRG
	End
	Print @sqlcommand
	if @Grp='SOPL2'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'POL1', 'MnthNo=202001'
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Iss_Qty,Bal_Qty,Inv_Val,It_Tot,Bal_It_Tot,Ord_Status,LastUpdtOn,Entry_TY,Tran_Cd From DSB_SO Where Bal_Qty>0 and ' + @vPeriod + ' Order By MnthNo,Code_Nm,Inv_Dt,Inv_No';
	End
	if (@Grp='PO')
	Begin
		set @sqlcommand='Select bCode_Nm='+char(39)+@Grp+char(39)+',Inv_val=Sum(Inv_val) From DSB_Trn_Tot_L0 Where  (bCode_Nm in (''PO'') ) and '+@vPeriod --Group By bCode_Nm
	End
	if @Grp='POL1'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'POL1', 'MnthNo=202001'
		set @sqlcommand='Select Mnth,Code_Nm,[Qty]=sum(Qty),[Issue Qty]=sum(Iss_Qty),[Bal Qty]=sum(Bal_Qty),[Value]=sum(Inv_Val),LastUpdtOn,MnthNo From DSB_Mnth_ENt_Tot_L1 Where (bCode_Nm in (''PO'') ) and '+@vPeriod  +' Group By Code_Nm,Mnth,MnthNo,LastUpdtOn Order by MnthNo';--RRG
		--set @sqlcommand='Select Mnth,Code_Nm,[Qty]=Qty,[Issue Qty]=Iss_Qty,[Bal Qty]=Bal_Qty,[Value]=Inv_Val,LastUpdtOn,MnthNo From DSB_Mnth_ENt_Tot_L1 Where (bCode_Nm in (''PO'') ) and '+@vPeriod  +' Order by MnthNo';--RRG
	End
	if @Grp='POL2'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'POL2', 'MnthNo=202001'
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Iss_Qty,Bal_Qty,Inv_Val,Ord_Status,LastUpdtOn,Entry_TY,Tran_Cd From DSB_PO Where ' + @vPeriod + ' Order By MnthNo,Code_Nm,Inv_Dt,Inv_No';
	End
	if (@Grp='POP')
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'POL2', 'MnthNo=202001'
		set @sqlcommand='Select bCode_Nm='+char(39)+@Grp+char(39)+',Bal_It_Tot=Sum(Bal_It_Tot) From DSB_Trn_Tot_L0 Where  (bCode_Nm in (''PO'') ) and Bal_Qty>0 and '+@vPeriod --Group By bCode_Nm
	End
	if @Grp='POPL1'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'POL1', 'MnthNo=202001'
		set @sqlcommand='Select Mnth,Code_Nm,[Bal Qty]=sum(Bal_Qty),[Value]=sum(Bal_It_Tot),LastUpdtOn,MnthNo From DSB_Mnth_ENt_Tot_L1 Where (bCode_Nm in (''PO'') ) and Bal_Qty>0 and '+@vPeriod  +' Group By Code_Nm,Mnth,MnthNo,LastUpdtOn Order by MnthNo';
	End
	if @Grp='POPL2'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'POL2', 'MnthNo=202001'
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Iss_Qty,Bal_Qty,Inv_Val,It_Tot,Bal_It_Tot,Ord_Status,LastUpdtOn,Entry_TY,Tran_Cd From DSB_PO Where Bal_Qty>0 and ' + @vPeriod + ' Order By MnthNo,Code_Nm,Inv_Dt,Inv_No';
	End
	if @Grp='BPL1'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'POL1', 'MnthNo=202001'
		
		--set @sqlcommand='Select Mnth,MnthNo,Code_Nm,[Qty]=Qty,[Value]=Inv_Val,LastUpdtOn From DSB_Mnth_ENt_Tot_L1 Where ' + @vPeriod + ' and  bCode_Nm=''BP''  Order by MnthNo';--RRG
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,[Qty]=sum(Qty),[Value]=sum(Inv_Val),LastUpdtOn From DSB_Mnth_ENt_Tot_L1 Where ' + @vPeriod + ' and  bCode_Nm=''BP'' Group By Code_Nm,Mnth,MnthNo,LastUpdtOn  Order by MnthNo';--RRG
	End
	if @Grp='BPL2'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'BPL2', 'MnthNo=202001'
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,Bank_Nm,Cheq_No,u_ChqDt From DSB_BP Where ' + @vPeriod + '   Order By MnthNo,Code_Nm,Inv_Dt,Inv_No';
	End
	if @Grp='BRL1'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'POL1', 'MnthNo=202001'
		--set @sqlcommand='Select Mnth,MnthNo,Code_Nm,[Qty]=Qty,[Value]=Inv_Val,LastUpdtOn From DSB_Mnth_ENt_Tot_L1 Where ' + @vPeriod + ' and  bCode_Nm=''BR'' Order by MnthNo';--RRG
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,[Qty]=Sum(Qty),[Value]=sum(Inv_Val),LastUpdtOn From DSB_Mnth_ENt_Tot_L1 Where ' + @vPeriod + ' and  bCode_Nm=''BR'' Group By Code_Nm,Mnth,MnthNo,LastUpdtOn Order by MnthNo';
	End
	if @Grp='BRL2'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'BRL2', 'MnthNo=202001'
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,Bank_Nm,Cheq_No,u_ChqDt From DSB_BR Where ' + @vPeriod + ' Order By MnthNo,Code_Nm,Inv_Dt,Inv_No';
	End

	print 'divyang'
		print @vPeriod
		set @OBMonth = right(@vPeriod,7) 
		set @OBMonth = left (@OBMonth,6)
		print @OBMonth

	if @Grp='STO'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'STO', 'MnthNo=202001'
		--set @sqlcommand='Select bCode_Nm=''ST'',[Outs Amt]=Sum(OutsAmt) From DSB_Trn_Tot_L0 Where  bCode_Nm in (''ST'') and OutsAmt>0 and '+@vPeriod --Group By bCode_Nm
		set @sqlcommand='Select bCode_Nm=''ST'',[Outs Amt]=Sum(OutsAmt) - (Select isnull(Sum(OutsAmt),0) From DSB_Trn_Tot_L0 Where ACGRP=''SD'' and  bCode_Nm in (''PT'') and OutsAmt>0 and '+@vPeriod+' ) -
						(Select isnull(SUM(OutsAmt),0) From DSB_JV Where ACGRP=''SD'' and AMT_TY in (''CR'') and OutsAmt>0 and '+@vPeriod+' )	
						From DSB_Trn_Tot_L0 Where ACGRP=''SD'' and  bCode_Nm in (''ST'',''JV'',''OB'') and OutsAmt>0 and '+@vPeriod --Group By bCode_Nm		--Divyang 27072020
	End
	if @Grp='STOL1'
	Begin
		
		--Execute[Usp_Rep_Dashboard] 1,'STOL1', 'MnthNo=202001'
		--set @sqlcommand='Select Entry_Ty=''ST'',Mnth,MnthNo,[Outs Amt]=Sum(OutsAmt),LastUpdtOn From DSB_Trn_Tot_L0 Where  bCode_Nm in (''ST'') and OutsAmt>0  and '+@vPeriod +' Group By Mnth,MnthNo,LastUpdtOn  Order By MnthNo'				--Comm Divyang
		set @sqlcommand='Select Entry_Ty=''ST'',Mnth,MnthNo,[Outs Amt]=sum ( case when bcode_nm in (''ST'',''OB'',''JV'') then OutsAmt when bCode_Nm in (''PT'') then  -(OutsAmt) else 0 end),LastUpdtOn 
						From DSB_Trn_Tot_L0 Where ACGRP=''SD'' and  bCode_Nm in (''ST'',''OB'',''JV'') and OutsAmt>0  and '+@vPeriod +'																	
						Group By Mnth,MnthNo,LastUpdtOn  
						union all
						Select Entry_Ty=''OB'',Mnth=''Opening Balance'',MnthNo=0,[Outs Amt]=sum ( case when bcode_nm in (''ST'',''OB'',''JV'') then OutsAmt when bCode_Nm in (''PT'') then  -(OutsAmt) else 0 end),LastUpdtOn=cast(LastUpdtOn as date) From DSB_Trn_Tot_L0 
						Where ACGRP=''SD'' and bCode_Nm in (''ST'',''JV'',''OB'') and OutsAmt>0  and MnthNo < '+@OBMonth+' 
						Group By cast(LastUpdtOn as date  )  
						Order By MnthNo'					-- added by Divyang
	End
	if @Grp='STOL2'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'STOL2', 'MnthNo=202001'
		--set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,[Outs Amt]=OutsAmt From DSB_ST Where OutsAmt>0 and ' + @vPeriod	--Divyang 27072020
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,[Outs Amt]=OutsAmt 
						From DSB_ST Where ACGRP=''SD'' and OutsAmt>0 and ' + @vPeriod+ ' 
						union all
						Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,[Outs Amt]=-OutsAmt 
						From DSB_PT Where ACGRP=''SD'' and OutsAmt>0 and ' + @vPeriod+ '
						union all
						Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,[Outs Amt]=OutsAmt 
						From DSB_OB Where ACGRP=''SD'' and OutsAmt>0 and ' + @vPeriod+ '
						union all
						Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,[Outs Amt]=(case when amt_ty=''DR'' then OutsAmt else  -OutsAmt end)
						From DSB_JV Where ACGRP=''SD'' and OutsAmt>0 and ' + @vPeriod+ '		'
		
		
	End
	if @Grp='PTO'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'PTO', 'MnthNo=202001'
		--set @sqlcommand='Select bCode_Nm=''PT'',[Outs Amt]=Sum(OutsAmt) From DSB_Trn_Tot_L0 Where  bCode_Nm in (''PT'') and OutsAmt>0 and '+@vPeriod --Group By bCode_Nm		
		set @sqlcommand='Select bCode_Nm=''PT'',[Outs Amt]=Sum(OutsAmt) - (Select isnull(Sum(OutsAmt),0) From DSB_Trn_Tot_L0 Where ACGRP=''SC'' and  bCode_Nm in (''ST'') and OutsAmt>0 and '+@vPeriod+' )-
						(Select isnull(SUM(OutsAmt),0) From DSB_JV Where ACGRP=''SC'' and AMT_TY in (''DR'') and OutsAmt>0 and '+@vPeriod+' )	
						From DSB_Trn_Tot_L0 Where ACGRP=''SC'' and  bCode_Nm in (''PT'',''JV'',''OB'') and OutsAmt>0 and '+@vPeriod --Group By bCode_Nm		--Divyang 27072020
	End
	if @Grp='PTOL1'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'PTOL1', 'MnthNo=202001'
		--set @sqlcommand='Select Entry_Ty=''PT'',Mnth,MnthNo,[Outs Amt]=Sum(OutsAmt),LastUpdtOn From DSB_Trn_Tot_L0 Where  bCode_Nm in (''PT'') and OutsAmt>0  and '+@vPeriod +' Group By Mnth,MnthNo,LastUpdtOn  Order By MnthNo'
		set @sqlcommand='Select Entry_Ty=''PT'',Mnth,MnthNo,[Outs Amt]=sum ( case when bcode_nm in (''PT'',''OB'',''JV'') then isnull(OutsAmt,0) when bCode_Nm in (''ST'') then  -(isnull(OutsAmt,0)) else 0 end),LastUpdtOn 
						From DSB_Trn_Tot_L0 Where ACGRP=''SC'' and  bCode_Nm in (''PT'',''OB'',''JV'') and OutsAmt>0  and '+@vPeriod +'																	
						Group By Mnth,MnthNo,LastUpdtOn  
						union all
						Select Entry_Ty=''OB'',Mnth=''Opening Balance'',MnthNo=0,[Outs Amt]=sum ( case when bcode_nm in (''PT'',''OB'',''JV'') then isnull(OutsAmt,0) when bCode_Nm in (''ST'') then  -(isnull(OutsAmt,0)) else 0 end),LastUpdtOn=cast(LastUpdtOn as date) From DSB_Trn_Tot_L0 
						Where ACGRP=''SC'' and bCode_Nm in (''PT'',''JV'',''OB'') and OutsAmt>0  and MnthNo < '+@OBMonth+' 
						Group By cast(LastUpdtOn as date  )  
						Order By MnthNo'					-- added by Divyang 27072020
	End
	if @Grp='PTOL2'
	Begin
		--Execute[Usp_Rep_Dashboard] 1,'PTOL2', 'MnthNo=202001'
		--set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Bill_No,Bill_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,[Outs Amt]=OutsAmt From DSB_PT Where OutsAmt>0 and ' + @vPeriod 
		set @sqlcommand='Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Bill_No,Bill_Dt,Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,[Outs Amt]=isnull(OutsAmt,0) 
						From DSB_PT Where ACGRP=''SC'' and OutsAmt>0 and ' + @vPeriod+ ' 
						union all
						Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Bill_No='''',Bill_Dt='''',Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,[Outs Amt]=-isnull(OutsAmt,0) 
						From DSB_ST Where ACGRP=''SC'' and OutsAmt>0 and ' + @vPeriod+ '
						union all
						Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Bill_No='''',Bill_Dt='''',Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,[Outs Amt]=isnull(OutsAmt,0) 
						From DSB_OB Where ACGRP=''SC'' and OutsAmt>0 and ' + @vPeriod+ '
						union all
						Select Mnth,MnthNo,Code_Nm,Party_Nm,Inv_No,Inv_Dt,Bill_No='''',Bill_Dt='''',Qty,Inv_Val,LastUpdtOn,Entry_TY,Tran_Cd,[Outs Amt]=isnull(OutsAmt,0) 
						From DSB_JV Where ACGRP=''SC'' and OutsAmt>0 and ' + @vPeriod+ '		'		--Divyang 27072020
	End

	if (@sqlcommand='')
	Begin
		set @sqlcommand='Select bCode_Nm='+char(39)+@Grp+char(39)+',Inv_val=Sum(Inv_val) From DSB_Trn_Tot_L0 Where  (bCode_Nm in ('+char(39)+@Grp+char(39)+') ) and '+@vPeriod --Group By bCode_Nm
	End
	Print @sqlcommand
	EXEC SP_EXECUTESQL  @sqlcommand
End
