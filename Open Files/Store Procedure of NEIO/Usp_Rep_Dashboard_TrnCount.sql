If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Rep_Dashboard_TrnCount')
Begin
	Drop Procedure Usp_Rep_Dashboard_TrnCount
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[Usp_Rep_Dashboard_TrnCount]
@CompCd Varchar(6),@Grp Varchar(10),@vPeriod Varchar(300),@Entry_Ty Varchar(4)
As
Begin
	Declare @sqlcommand nvarchar(4000)
	Declare @bCode Varchar(2)
	Select @bCode=(Case When Ext_Vou=0 Then Entry_Ty Else bCode_Nm End) from Lcode Where Entry_ty=@Entry_Ty
	Set @sqlcommand=''

	if @Grp='TrnListL1'
	Begin
		--Execute [Usp_Rep_Dashboard_TrnCount] 1,'TrnListL1',' (202005,202004,202003,202002,202001,201912)',''
		--set @sqlcommand='Select l.Entry_TY,l.Code_Nm,NoTrn=Count(Inv_No) From [UserHist_vw] inner Join lcode l on (UserHist_vw.Entry_Ty=l.Entry_Ty) Where Cast(Year([Date]) as varchar(4))+(Case when Month([Date]) in (10,11,12) Then '''' else ''0'' End)  +Cast(Month([Date]) as Varchar(2)) in ' + @vPeriod+' Group by l.Entry_TY,l.Code_Nm Order By l.Code_Nm'
		--set @sqlcommand='Select l.Entry_Ty,l.Code_Nm,[No of Trn]=Count(Inv_No),Mnth=FORMAT([Date], ''MMM'')+''-''+Right(cast(Year([Date]) as varchar(4)),2) From [UserHist_vw] inner Join lcode l on (UserHist_vw.Entry_Ty=l.Entry_Ty) Where Cast(Year([Date]) as varchar(4))+(Case when Month([Date]) in (10,11,12) Then '''' else ''0'' End)  +Cast(Month([Date]) as Varchar(2)) in ' + @vPeriod+' Group by l.Entry_Ty,l.Code_Nm,FORMAT([Date], ''MMM'')+''-''+Right(cast(Year([Date]) as varchar(4)),2),year(Date),Month(date) Order By l.Code_Nm,Cast(Year([Date]) as varchar(4))+(Case when Month([Date]) in (10,11,12) Then '''' else ''0'' End)  +Cast(Month([Date]) as Varchar(2))'		--Commented by Divyang on 01072020
		set @sqlcommand='Select l.Entry_Ty,l.Code_Nm,[No of Trn]=Count(Inv_No),Mnth=LEFT(DATENAME(Month,[Date]),3 )+''-''+Right(cast(Year([Date]) as varchar(4)),2) From [UserHist_vw] inner Join lcode l on (UserHist_vw.Entry_Ty=l.Entry_Ty) Where Cast(Year([Date]) as varchar(4))+(Case when Month([Date]) in (10,11,12) Then '''' else ''0'' End)  +Cast(Month([Date]) as Varchar(2)) in ' + @vPeriod+' Group by l.Entry_Ty,l.Code_Nm,LEFT(DATENAME(Month,[date]),3 )+''-''+Right(cast(Year([Date]) as varchar(4)),2),year(Date),Month(date) Order By l.Code_Nm,Cast(Year([Date]) as varchar(4))+(Case when Month([Date]) in (10,11,12) Then '''' else ''0'' End)  +Cast(Month([Date]) as Varchar(2))'			--Modified by Divyang on 01072020
	End
	if @Grp='TrnListL2'
	Begin
		--Execute[Usp_Rep_Dashboard_TrnCount] 1,'TrnListL2', '''Dec-19''','BP'
		--set @sqlcommand='Select l.Code_Nm,NoTrn=Count(Inv_No),Mnth=FORMAT([Date], ''MMM'')+''-''+Right(cast(Year([Date]) as varchar(4)),2) From [UserHist_vw] inner Join lcode l on (UserHist_vw.Entry_Ty=l.Entry_Ty) Where l.Entry_Ty='+Char(39)+@Entry_Ty+Char(39)+' and Cast(Year([Date]) as varchar(4))+(Case when Month([Date]) in (10,11,12) Then '''' else ''0'' End)  +Cast(Month([Date]) as Varchar(2)) in ' + @vPeriod+' Group by l.Code_Nm,FORMAT([Date], ''MMM'')+''-''+Right(cast(Year([Date]) as varchar(4)),2),year(Date),Month(date) Order By l.Code_Nm,Cast(Year([Date]) as varchar(4))+(Case when Month([Date]) in (10,11,12) Then '''' else ''0'' End)  +Cast(Month([Date]) as Varchar(2))'
		--set @sqlcommand='Select l.Entry_Ty,l.Code_Nm,[No of Trn]=Count(Inv_No),Dy=Day(Date),Mnth='+@vPeriod+' From [UserHist_vw] inner Join lcode l on (UserHist_vw.Entry_Ty=l.Entry_Ty) Where l.Entry_Ty='+Char(39)+@Entry_Ty+Char(39)+' and FORMAT([Date], ''MMM'')+''-''+Right(cast(Year([Date]) as varchar(4)),2)='+@vPeriod+' Group by l.Entry_Ty,l.Code_Nm,Day(Date) Order By l.Code_Nm,Day(Date)'			--Commented by Divyang on 01072020
		set @sqlcommand='Select l.Entry_Ty,l.Code_Nm,[No of Trn]=Count(Inv_No),Dy=Day(Date),Mnth='+@vPeriod+' From [UserHist_vw] inner Join lcode l on (UserHist_vw.Entry_Ty=l.Entry_Ty) Where l.Entry_Ty='+Char(39)+@Entry_Ty+Char(39)+' and LEFT(DATENAME(Month,[Date]),3 )+''-''+Right(cast(Year([Date]) as varchar(4)),2)='+@vPeriod+' Group by l.Entry_Ty,l.Code_Nm,Day(Date) Order By l.Code_Nm,Day(Date)'			--Modified by Divyang on 01072020
	End
	if @Grp='TrnListL3'
	Begin
		--Execute[Usp_Rep_Dashboard_TrnCount] 1,'TrnListL3', '''Dec-19'' and Day(Date)=2','BP'
		--set @sqlcommand='Select l.Code_Nm,Party_Nm,Inv_No,[Date],Tran_Cd,Inv_Val=Net_Amt,SysDate,User_Name,Tran_Cd From '+@bCode+'Main m inner Join lcode l on (m.Entry_Ty=l.Entry_Ty) Where l.Entry_Ty='+Char(39)+@Entry_Ty+Char(39)+' and FORMAT([Date], ''MMM'')+''-''+Right(cast(Year([Date]) as varchar(4)),2)='+@vPeriod+ ' Order By [Date],Inv_No'				--Commented by Divyang on 01072020
		--set @sqlcommand='Select l.Code_Nm,Party_Nm,Inv_No,[Date],Tran_Cd,Inv_Val=Net_Amt,SysDate,User_Name,m.Entry_ty,Tran_Cd From '+@bCode+'Main m inner Join lcode l on (m.Entry_Ty=l.Entry_Ty) Where l.Entry_Ty='+Char(39)+@Entry_Ty+Char(39)+' and LEFT(DATENAME(Month,[Date]),3 )+''-''+Right(cast(Year([Date]) as varchar(4)),2)='+@vPeriod+ ' Order By [Date],Inv_No'		--Modified by Divyang on 01072020
		set @sqlcommand='Select l.Code_Nm,Party_Nm,Inv_No,[Date],Inv_Val=Net_Amt,SysDate,User_Name,m.Entry_ty,Tran_Cd From '+@bCode+'Main m inner Join lcode l on (m.Entry_Ty=l.Entry_Ty) Where l.Entry_Ty='+Char(39)+@Entry_Ty+Char(39)+' and LEFT(DATENAME(Month,[Date]),3 )+''-''+Right(cast(Year([Date]) as varchar(4)),2)='+@vPeriod+ ' Order By [Date],Inv_No'		--Modified by Divyang on 08072020
	End
	Print @sqlcommand
	EXEC SP_EXECUTESQL  @sqlcommand
End
		
	