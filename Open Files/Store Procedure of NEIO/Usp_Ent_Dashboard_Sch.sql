If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Ent_Dashboard_Sch')
Begin
	Drop Procedure Usp_Ent_Dashboard_Sch
End
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Rupesh
-- Create date: 
-- Description:This SP used to Update Dashbord
-- Modify date: 
-- Remark:
-- =============================================
Create Procedure [dbo].[Usp_Ent_Dashboard_Sch]
@CompCd Varchar(6),@bCode_Nm Varchar(4),@MnthNo Varchar(300)
As
Begin
Declare @sqlcommand nvarchar(4000)

--execute Usp_Ent_Dashboard_Sch '6','SO','201910,201911,201912,202001,202002,202003'
--execute Usp_Ent_Dashboard_Sch '6','ST','201910,201911,201912,202001,202002,202003'
--execute Usp_Ent_Dashboard_Sch '6','BR','201910,201911,201912,202001,202002,202003'
--execute Usp_Ent_Dashboard_Sch '6','BP','201910,201911,201912,202001,202002,202003'
--execute Usp_Ent_Dashboard_Sch '6','PO','201910,201911,201912,202001,202002,202003'
--execute Usp_Ent_Dashboard_Sch '6','PT','201910,201911,201912,202001,202002,202003'
--execute Usp_Ent_Dashboard_Sch '6','SR','201910,201911,201912,202001,202002,202003'
--execute Usp_Ent_Dashboard_Sch '6','PR','201910,201911,201912,202001,202002,202003'
--execute Usp_Ent_Dashboard_Sch '6','CNST','201910,201911,201912,202001,202002,202003,202005'
--execute Usp_Ent_Dashboard_Sch '6','DNST','201910,201911,201912,202001,202002,202003'
--execute Usp_Ent_Dashboard_Sch '6','CNPT','201910,201911,201912,202001,202002,202003'
--execute Usp_Ent_Dashboard_Sch '6','DNPT','201910,201911,201912,202001,202002,202003'

--L2
set @sqlcommand='Delete From DSB_'+left(@bCode_Nm,2)+' Where MnthNo in ('+@MnthNo+') and bCode_Nm='+char(39)+left(@bCode_Nm,2)+char(39)
print @sqlcommand
print @MnthNo
EXEC SP_EXECUTESQL  @sqlcommand
Print 'R0-1'
set @sqlcommand='Insert Into DSB_'+Left(@bCode_Nm,2)+' (CompCd,Mnth,Tran_Cd,Entry_Ty,Code_Nm,bCode_Nm,bCode_Name,Party_Nm,Inv_No,Inv_Dt,Qty,It_Tot'
if (@bCode_Nm ='ST' or @bCode_Nm ='PT' or @bCode_Nm='JV' or @bCode_Nm='OB')		--Divyang 27072020
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',OutsAmt'
End
if (@bCode_Nm='SO' or @bCode_Nm='PO')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',Iss_Qty,Bal_Qty,Ord_Status,Bal_It_Tot'
End
if (@bCode_Nm='PT')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',Bill_No,Bill_Dt'
End
if (@bCode_Nm ='DNST' or @bCode_Nm ='CNST' or @bCode_Nm ='DNPT' or @bCode_Nm ='CNPT')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',[Reason]'
End
if (@bCode_Nm='JV')		--Divyang 27072020
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',AMT_TY'
End

set @sqlcommand=rtrim(@sqlcommand)+' '+',Inv_Val,LastUpdtOn,MnthNo,ACGRP)'			--Divyang 27072020
--set @sqlcommand=rtrim(@sqlcommand)+' '+'Select '+Char(39)+@CompCd+Char(39)+',Mnth=FORMAT(m.[Date], ''MMM'')+''-''+Right(cast(Year(m.[Date]) as varchar(4)),2)'	--Commented by Divyang on 01072020
set @sqlcommand=rtrim(@sqlcommand)+' '+'Select '+Char(39)+@CompCd+Char(39)+',Mnth=LEFT(DATENAME(Month,m.[date]),3 )+''-''+Right(cast(Year(m.[Date]) as varchar(4)),2)'		-- modified by Divyang on 01072020
set @sqlcommand=rtrim(@sqlcommand)+' '+',m.Tran_Cd'

if (@bCode_Nm ='DNST' or @bCode_Nm ='CNST' or @bCode_Nm ='DNPT' or @bCode_Nm ='CNPT')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',Entry_Ty=l.Entry_Ty+(Case When AgainstGS=''Sales'' Then ''ST'' Else ''PT'' End)' 
End
Else
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',l.Entry_Ty'
End
set @sqlcommand=rtrim(@sqlcommand)+' '+',l.Code_Nm,bCode_Nm=(Case When l.Ext_Vou=1 Then l.BCode_Nm Else l.Entry_Ty End),bCode_Name=(Case When l.Ext_Vou=1 Then l1.Code_Nm Else l.Code_Nm End)'
if (@bCode_Nm='JV' )    --Divyang 27072020
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',lac_vw.Ac_Name'
END
ELSE
BEGIN
	set @sqlcommand=rtrim(@sqlcommand)+' '+',acm.Ac_Name'
END
set @sqlcommand=rtrim(@sqlcommand)+' '+',m.Inv_No,m.[Date],Qty=Sum(i.Qty),It_Tot=Sum(i.Gro_Amt)'
if (@bCode_Nm='ST' or @bCode_Nm='PT'  or @bCode_Nm='OB')    --Divyang 27072020
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',OutsAmt=(m.Net_Amt-isNull(ac.Re_All,0))'
End
if (@bCode_Nm='JV' )    --Divyang 27072020
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',OutsAmt=(m.Net_Amt-isNull(ac.Re_All,0))'
End
if (@bCode_Nm='SO' or @bCode_Nm='PO')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',Iss_Qty=Sum(Re_Qty),Bal_Qty=Sum(i.Qty-Re_Qty),Ord_Status=(Case When sum(i.Qty-Re_Qty)>0 Then ''Pending'' Else ''Closed'' End),Bal_It_Tot=Sum((i.Gro_Amt/i.Qty)*(i.Qty-Re_Qty))'
End
if (@bCode_Nm='PT')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',Bill_No=pInvNo,Bill_Dt=pInvDt'
End
if (@bCode_Nm ='DNST' or @bCode_Nm ='CNST' or @bCode_Nm ='DNPT' or @bCode_Nm ='CNPT')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',[Reason]=cast(u_GPrice as varchar(20))'
End
if (@bCode_Nm='JV')		--Divyang 27072020
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',lac_vw.amt_ty'
End
set @sqlcommand=rtrim(@sqlcommand)+' '+',m.Net_Amt,LastUpdtOn=Cast(getDate() as Varchar(20)),MnthNo=Cast(Year(m.[Date]) as varchar(4))+(Case when Month(m.[Date]) in (10,11,12) Then '''' else ''0'' End)  +Cast(Month(m.[Date]) as Varchar(2))'
--set @sqlcommand=rtrim(@sqlcommand)+' '+',ACGRP=(case when acm.[group]=''SUNDRY DEBTORS'' then ''SD'' when acm.[group]=''SUNDRY CREDITORS'' then ''SC'' else '''' END)    '		--Divyang 27072020
set @sqlcommand=rtrim(@sqlcommand)+' '+',ACGRP=(case when acmg.[group]=''SUNDRY DEBTORS'' then ''SD'' when acmg.[group]=''SUNDRY CREDITORS'' then ''SC'' when acm.[group]=''SUNDRY DEBTORS'' then ''SD'' when acm.[group]=''SUNDRY CREDITORS'' then ''SC'' else '''' END)    '		--Divyang 10082020
set @sqlcommand=rtrim(@sqlcommand)+' '+'From '+Left(@bCode_Nm,2)+'Main m inner join Ac_Mast acm on (m.Ac_Id=acm.Ac_Id) Left Join '+Left(@bCode_Nm,2)+'Item i on (m.Tran_cd=i.Tran_Cd)' 
set @sqlcommand=rtrim(@sqlcommand)+' '+'left join ac_group_mast acmg on (acmg.ac_group_id=acm.ac_group_id)  '		--Divyang 10082020
set @sqlcommand=rtrim(@sqlcommand)+' '+'Left Join It_Mast itm on (i.It_Code=itm.It_Code)' 
set @sqlcommand=rtrim(@sqlcommand)+' '+'Inner Join lCode l on (l.Entry_Ty=m.Entry_Ty)'
set @sqlcommand=rtrim(@sqlcommand)+' '+'left join lCode l1 on (l.bCode_Nm=l1.Entry_Ty)'
if (@bCode_Nm ='ST' or @bCode_Nm ='PT' or @bCode_Nm='JV' or @bCode_Nm='OB')   --Divayng 27072020
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+' Left Join '+@bCode_Nm+'AcDet ac on (m.Tran_Cd=ac.Tran_cd and m.Party_Nm=ac.Ac_Name)'	--Divyang
End
if ( @bCode_Nm='JV' )   --Divayng 27072020
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+' inner Join lac_vw  on (m.Tran_Cd=lac_vw.Tran_cd)  '	
End


set @sqlcommand=rtrim(@sqlcommand)+' '+'Where  Cast(Year(m.[Date]) as varchar(4))+(Case when Month(m.[Date]) in (10,11,12) Then '''' else ''0'' End)  +Cast(Month(m.[Date]) as Varchar(2)) in ('+@MnthNo+' ) and ((Case When l.Ext_Vou=1 Then l.BCode_Nm Else l.Entry_Ty End)='+Char(39)+Left(@bCode_Nm,2)+Char(39)+')'
set @sqlcommand=rtrim(@sqlcommand)+' '+'And (Case When l.Ext_Vou=1 Then l.BCode_Nm Else l.Entry_Ty End)='+Char(39)+Left(@bCode_Nm,2)+Char(39)

if (@bCode_Nm ='DNST' or @bCode_Nm ='CNST')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+' And m.AgainstGS='+char(39)+'Sales'+Char(39)
End
if (@bCode_Nm ='DNPT' or @bCode_Nm ='CNPT')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+' And m.AgainstGS='+char(39)+'Purchase'+Char(39)
End
if ( @bCode_Nm='JV' )   --Divayng 27072020
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+' and lac_vw.entry_ty=''JV'' '	
End

set @sqlcommand=rtrim(@sqlcommand)+' '+'Group By m.Tran_Cd,l.Entry_Ty,l.Ext_Vou,l.code_nm,l1.code_nm,l.bCode_Nm,m.Party_Nm,m.Inv_No,m.Date,acm.Ac_Name,m.net_amt,acm.[group],acmg.[group]'
if (@bCode_Nm ='ST' or @bCode_Nm ='PT' or @bCode_Nm='JV' or @bCode_Nm='OB')  --Divyang 27072020
Begin
	set @sqlcommand=rtrim(@sqlcommand)+',ac.Re_All'
End
if (@bCode_Nm='PT')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',pInvNo,pInvDt'
End
if (@bCode_Nm ='DNST' or @bCode_Nm ='CNST' or @bCode_Nm ='DNPT' or @bCode_Nm ='CNPT')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',m.AgainstGS,U_GPRICE' 
End
if ( @bCode_Nm='JV' )   --Divayng 27072020
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+' ,lac_vw.ac_name,lac_vw.entry_ty,lac_vw.amt_ty'	
End


Print @sqlcommand
Print 'R0-2'
--Select Party_Nm,Inv_No,Inv_Dt,It_Name,Ord_Qty,Iss_Qty,Bal_Qty,Ord_Status From MB_Sales_Order_Staus
EXEC SP_EXECUTESQL  @sqlcommand
--if (@bCode_Nm ='ST' or @bCode_Nm ='PT')
--Begin
--	set @sqlcommand='Update a Set a.OutsAmt=b.Amount-b.Re_All From DSB_'+@bCode_Nm+' a inner join '+@bCode_Nm+'Acdet b on (a.Tran_Cd=b.Tran_cd and a.Party_Nm=b.Ac_Name)  Where MnthNo in('+@MnthNo+')'
--	EXEC SP_EXECUTESQL  @sqlcommand
--End



----L1
--Select Distinct Entry_Ty,MnthNo,Tran_Cd,Inv_Val into #InvVal From DSB_DN Where 1=2
--Select Entry_Ty,MnthNo,Inv_Val=Inv_Val into #InvVal1 From #InvVal Where 1=2
--set @sqlcommand='Insert into #InvVal Select Distinct Entry_Ty,MnthNo,Tran_Cd,Inv_Val From DSB_'+@bCode_Nm +' Where MnthNo in('+@MnthNo+')'
--EXEC SP_EXECUTESQL  @sqlcommand
--Select * from #InvVal
Print 'R1'
--set @sqlcommand='Insert Into #InvVal1 Select Entry_Ty,MnthNo,Inv_Val=Sum(Inv_Val) From #InvVal Group By Entry_Ty,MnthNo'
--EXEC SP_EXECUTESQL  @sqlcommand
--Print @sqlcommand
--Select * from #InvVal1
set @sqlcommand='Delete From DSB_Mnth_ENt_Tot_L1 Where MnthNo in('+@MnthNo+') and bCode_Nm='+char(39)+Left(@bCode_Nm,2)+char(39)
EXEC SP_EXECUTESQL  @sqlcommand
set @sqlcommand='Insert Into DSB_Mnth_ENt_Tot_L1 (CompCd,Mnth,Entry_Ty,Code_Nm,bCode_Nm,bCode_Name,Qty,It_Tot,Iss_Qty,Bal_Qty,Bal_It_Tot,OutsAmt,Inv_Val,LastUpdtOn,MnthNo,ACGRP)'		--Divyang 27072020
set @sqlcommand=rtrim(@sqlcommand)+' '+'SELECT CompCd,Mnth,Entry_Ty,Code_Nm,bCode_Nm,bCode_Name,Qty=Sum(Qty),It_Tot=Sum(It_Tot)'
if (@bCode_Nm='SO' or @bCode_Nm='PO')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',Iss_Qty=sum(Iss_Qty),Bal_Qty=sum(Bal_Qty),Bal_It_Tot=Sum(Bal_It_Tot)'
End
else
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',Iss_Qty=0,Bal_Qty=0,Bal_It_Tot=0'
End
if (@bCode_Nm='ST' or @bCode_Nm='PT' or @bCode_Nm='JV' or @bCode_Nm='OB')   --Divyang 27072020
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',OutsAmt=sum(OutsAmt)'
End
else
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',OutsAmt=0'
End

set @sqlcommand=rtrim(@sqlcommand)+' '+',Inv_Val,LastUpdtOn,MnthNo,ACGRP'		--Divyang 27072020
set @sqlcommand=rtrim(@sqlcommand)+' '+'From DSB_'+Left(@bCode_Nm,2) 
set @sqlcommand=rtrim(@sqlcommand)+' '+'Where MnthNo in('+@MnthNo+') and bCode_Nm='+char(39)+Left(@bCode_Nm,2)+char(39)
set @sqlcommand=rtrim(@sqlcommand)+' '+'Group By CompCd,Mnth,Entry_Ty,Code_Nm,bCode_Nm,bCode_Name,LastUpdtOn,MnthNo,Inv_Val,ACGRP'		--Divyang 27072020
Print 'R2'
Print 'r 1'+@sqlcommand
EXEC SP_EXECUTESQL  @sqlcommand
--update a set a.Inv_Val=b.Inv_Val From DSB_Mnth_ENt_Tot_L1 a inner Join #InvVal1 b on (a.MnthNo=b.MnthNo and a.Entry_Ty=b.Entry_Ty)
Print 'R3'
------L0
set @sqlcommand='Delete From DSB_Trn_Tot_L0 Where MnthNo in('+@MnthNo+') and bCode_Nm='+char(39)+Left(@bCode_Nm,2)+char(39)
Print 'R4'
EXEC SP_EXECUTESQL  @sqlcommand
Print 'R6'
set @sqlcommand='Insert Into DSB_Trn_Tot_L0 (CompCd,Mnth,Entry_Ty,Code_Nm,bCode_Nm,bCode_Name,Qty,Inv_Val,It_Tot,OutsAmt'
if (@bCode_Nm='SO' or @bCode_Nm='PO')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',Bal_Qty,Bal_It_Tot'
End
--if (@bCode_Nm='JV')				--Divyang 27072020
--Begin
--	set @sqlcommand=rtrim(@sqlcommand)+' '+',AMT_TY'
--End
set @sqlcommand=rtrim(@sqlcommand)+' '+',LastUpdtOn,MnthNo,ACGRP)'    --Divyang 27072020
set @sqlcommand=rtrim(@sqlcommand)+' '+'Select CompCd,Mnth,Entry_Ty,Code_Nm,bCode_Nm,bCode_Name,Sum(Qty),Inv_Val=Sum(Inv_Val),It_Tot=Sum(It_Tot)'

if (@bCode_Nm='ST' or @bCode_Nm='PT' or @bCode_Nm='JV' or @bCode_Nm='OB') Begin set @sqlcommand=rtrim(@sqlcommand)+' '+',OutsAmt=Sum(OutsAmt)' End else Begin set @sqlcommand=rtrim(@sqlcommand)+' '+',OutsAmt=0' End			--Divyang 27072020

if (@bCode_Nm='SO' or @bCode_Nm='PO')
Begin
	set @sqlcommand=rtrim(@sqlcommand)+' '+',Bal_Qty=Sum(Bal_Qty),Bal_It_Tot=Sum(Bal_It_Tot)' 
End
--if (@bCode_Nm='JV')				--Divyang 27072020
--Begin
--	set @sqlcommand=rtrim(@sqlcommand)+' '+',amt_ty'
--End
set @sqlcommand=rtrim(@sqlcommand)+' '+',LastUpdtOn,MnthNo,ACGRP'		--Divyang 27072020
set @sqlcommand=rtrim(@sqlcommand)+' '+'From DSB_'+Left(@bCode_Nm,2) 
set @sqlcommand=rtrim(@sqlcommand)+' '+'Where MnthNo in('+@MnthNo+') and bCode_Nm='+char(39)+Left(@bCode_Nm,2)+char(39)
set @sqlcommand=rtrim(@sqlcommand)+' '+'Group By CompCd,Mnth,Entry_Ty,Code_Nm,bCode_Nm,bCode_Name,LastUpdtOn,MnthNo,ACGRP'			--Divyang 27072020
--if (@bCode_Nm='JV')				--Divyang 27072020
--Begin
--	set @sqlcommand=rtrim(@sqlcommand)+' '+',amt_ty'
--End
Print 'R7'
Print @sqlcommand
EXEC SP_EXECUTESQL  @sqlcommand

--update a set a.Inv_Val=b.Inv_Val From DSB_Trn_Tot_L0 a inner Join #InvVal1 b on (a.MnthNo=b.MnthNo and a.Entry_Ty=b.Entry_Ty) 

--Select * from DSB_Trn_Tot_L0
--Select * from DSB_Mnth_ENt_Tot_L1
--Select * from DSB_BR -- L2
--Select * from DSB_DN -- L2
--Select * from DSB_BR -- L2
End


