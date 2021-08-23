If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Rep_Dashboard_Ledg')
Begin
	Drop Procedure Usp_Rep_Dashboard_Ledg
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[Usp_Rep_Dashboard_Ledg]
@CompCd Varchar(6),@Grp Varchar(10),@vPeriod Varchar(300)
As
Begin
	Declare @sqlcommand nvarchar(4000)
	Set @sqlcommand=''
	if @Grp='bLdgL1'
	Begin
		--Execute[Usp_Rep_Dashboard_Ledg] 1,'bLdgL1', ''
		set @sqlcommand='Select a.Ac_Name,Dr=isNull(Dr,0),Cr=isNull(Cr,0),Bal=abs(isNull(Dr,0)-isNull(Cr,0)),BalDrCr=(Case When isNull(Dr,0)-isNull(Cr,0)>0 Then ''Dr'' else ''Cr'' End) From Ac_Bal a inner join Ac_Mast b on (a.Ac_id=b.Ac_Id) Where Typ in (''Cash'',''Bank'') and a.Ac_Name not like ''%GST%'' Order By a.Ac_Name'
	End
	if @Grp='gLdgL1'
	Begin
		--Execute[Usp_Rep_Dashboard_Ledg] 1,'gLdgL1', ''
		set @sqlcommand='Select a.Ac_Name,Dr=isNull(Dr,0),Cr=isNull(Cr,0),Bal=abs(isNull(Dr,0)-isNull(Cr,0)),BalDrCr=(Case When isNull(Dr,0)-isNull(Cr,0)>0 Then ''Dr'' else ''Cr'' End) From Ac_Bal a inner join Ac_Mast b on (a.Ac_id=b.Ac_Id) where (isNull(Dr,0)+isNull(CR,0)>0) and  (typ like ''%GST%'') Order by (Case When Typ like ''%GST Payable'' Then ''A'' When Typ like ''%GST ITC'' Then ''B''  When Typ like ''Output%'' Then ''C'' When Typ like ''Input%'' Then ''D'' When Typ like ''%GST Ineligible ITC'' Then ''E'' Else ''Z'' End), Ac_Name'
	End
	Print @sqlcommand
	EXEC SP_EXECUTESQL  @sqlcommand
End

