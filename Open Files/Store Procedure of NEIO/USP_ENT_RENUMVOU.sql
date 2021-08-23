If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_ENT_RENUMVOU')
Begin
	Drop Procedure USP_ENT_RENUMVOU
End
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--set dateformat dmy execute [USP_ENT_RENUMVOU] 'JV','JOURNAL VOUCHER','01/08/2018','30/09/2018','A','0000001','Admin'
Create Procedure [dbo].[USP_ENT_RENUMVOU]
--@Entry_ty Varchar(2),@Code_nm Varchar(50),@SDate smalldatetime,@EDate smalldatetime,@Series Varchar(15),@startno Varchar(10),@Username Varchar(20),@bcode_nm Varchar(2)			--Commented by Shrikant S. on 16/05/2019 
@Entry_ty Varchar(2),@Code_nm Varchar(50),@SDate smalldatetime,@EDate smalldatetime,@Series Varchar(15),@startno Varchar(20),@Username Varchar(20),@bcode_nm Varchar(2)				--Added by Shrikant S. on 16/05/2019 	
AS
Declare @SQLCOMMAND NVARCHAR(500),@SQLCOMMAND1 NVARCHAR(4000),@ParmDefinition NVARCHAR(500),@SQLCOMMAND2 NVARCHAR(4000)
Declare @tran_cd Numeric,@Date smalldatetime,@Inv_No Varchar(20),@s_type Varchar(50),@i_prefix Varchar(10),@i_suffix Varchar(10),@invno_size int
Declare @tmpInvNo Varchar(20),@cnt Numeric,@UpdateOrNot Bit,@monthformat Varchar(500),@tmpInvNo1 Varchar(20),@l_yn Varchar(10),@genInvCond Varchar(500)			--Added by Shrikant S. on 16/05/2019 
--Declare @tmpInvNo Varchar(10),@cnt Numeric,@UpdateOrNot Bit,@monthformat Varchar(500),@tmpInvNo1 Varchar(20),@l_yn Varchar(10),@genInvCond Varchar(500)		--Commented by Shrikant S. on 16/05/2019 
Declare @tblType Varchar(50),@tbl_nm Varchar(20),@Fld_nm Varchar(20), @Link_cond Varchar(500), @Link_cond1 Varchar(500),@day Varchar(10),@mcon Varchar(50),@date1 smalldatetime,@mon1 Varchar(10),@tmp Varchar(20)

Declare @GenInvNo Varchar(20)

Create Table #tmpRENUMVOU(Entry_ty Varchar(2),Code_nm Varchar(50),Series Varchar(15), PrevInvno Varchar(20),NewInvno Varchar(20),Cond Varchar(200),Tbl_nm Varchar(20),ExeState Varchar(2000),Filtcond Varchar(200),l_yn Varchar(10),invdt smalldatetime,tran_cd Numeric)

Select * into #tmpLcode from Lcode where Entry_ty=@Entry_ty
select * into #tmpSeries from series  where INV_SR=@Series

Select @monthformat=isnull(MnthFormat,'') from #tmpSeries  
if @monthformat=''
begin
	select @monthformat=(select frmSQLEval from MonthFormat where MnthFrmt='MMYY')
end
else
begin
	Select @monthformat=isnull(frmSQLEval,'') from #tmpSeries left join monthformat on (#tmpSeries.mnthformat=monthformat.mnthfrmt) 
end
--Select @monthformat=isnull(frmSQLEval,'') from #tmpSeries  
--left join monthformat on (#tmpSeries.mnthformat=monthformat.mnthfrmt) 

select @invno_size=invno_size from  #tmpLcode			--Added by Shrikant S. on 17/5/2019			

print @Entry_ty+@Code_nm+@Series+@startno+@Username

Create Table #Update_tables(tblType Varchar(50),tbl_nm Varchar(20),Fld_nm Varchar(20), Link_cond Varchar(500),Link_cond1 Varchar(500))

Insert Into #Update_tables Values('',@bcode_nm+'MAIN','Inv_no','Entry_ty+convert(Varchar(10),Tran_cd)','')
Insert Into #Update_tables Values('',@bcode_nm+'ITEM','Inv_no','Entry_ty+convert(Varchar(10),Tran_cd)','')
Insert Into #Update_tables Values('',@bcode_nm+'ACDET','Inv_no','Entry_ty+convert(Varchar(10),Tran_cd)','')
Insert Into #Update_tables Values('','GEN_MISS','Inv_no','','rtrim(Entry_ty)+RTRIM(inv_sr)+convert(Varchar(20),Inv_no)')
Insert Into #Update_tables Values('','GEN_INV','Inv_no','','rtrim(Entry_ty)+RTRIM(inv_sr)')
INSERT INTO #Update_tables SELECT *,' ' as Link_cond1 FROM Upd_tbl



set @cnt=convert(Numeric,Case when @Series='' then right(@startno,@invno_size) else @startno end )			--Changed by Shrikant S. on 16/05/2019
set @tmpInvNo=Case when @Series='' then right(@startno,@invno_size) else @startno end						--Changed by Shrikant S. on 16/05/2019


--Select a.Entry_ty,a.tran_cd,a.Date,a.Inv_no,a.inv_sr,s_type=isnull(b.s_type,''),i_prefix=case when a.inv_sr='' then @Entry_ty+'/'+ substring(a.l_yn,3,2)+substring(a.l_yn,8,2)+'/' else isnull(b.i_prefix,'') end,i_suffix =isnull(b.i_suffix,''),a.l_yn		--Added by Shrikant S. on 16/05/2019 
--from Lmain_vw a Inner Join Series b on (a.inv_sr=b.inv_sr) 
--where a.Entry_ty=@Entry_ty AND a.inv_sr=@Series and a.date between @SDATE and @EDate
--Order by Date,Inv_no

Declare Upd_cur Cursor for
--Select a.Entry_ty,a.tran_cd,a.Date,a.Inv_no,a.inv_sr,s_type=isnull(b.s_type,''),i_prefix=isnull(b.i_prefix,''),i_suffix =isnull(b.i_suffix,''),a.l_yn		--Commented by Shrikant S. on 16/05/2019 
Select a.Entry_ty,a.tran_cd,a.Date,a.Inv_no,a.inv_sr,s_type=isnull(b.s_type,''),i_prefix=case when a.inv_sr='' then @Entry_ty+'/'+ substring(a.l_yn,3,2)+substring(a.l_yn,8,2)+'/' else isnull(b.i_prefix,'') end,i_suffix =isnull(b.i_suffix,''),a.l_yn		--Added by Shrikant S. on 16/05/2019 
from Lmain_vw a Inner Join Series b on (a.inv_sr=b.inv_sr) 
where a.Entry_ty=@Entry_ty AND a.inv_sr=@Series and a.date between @SDATE and @EDate
--Order by Inv_no				--Commented by Shrikant S. on 16/05/2019 
Order by Date,Inv_no			--Added by Shrikant S. on 16/05/2019 
Open Upd_cur
Fetch Next From Upd_cur Into @Entry_ty,@tran_cd,@Date,@Inv_no,@Series,@s_type,@i_prefix,@i_suffix,@l_yn
set @date1=@Date

While @@Fetch_Status=0
Begin
	--Print @Entry_ty+convert(varchar(10),@tran_cd)
		if rtrim(@s_type)=''
		Begin
		--Added by Divyang for Bug-33358 on 25/04/2020 Start
			Set @tmpInvNo1=case when rtrim(@i_prefix)<>'' and rtrim(@i_suffix)<>'' then replace(rtrim(@i_prefix),'"','')+@tmpInvNo+replace(rtrim(@i_suffix),'"','') else 
						(case when rtrim(@i_prefix)<>'' then replace(rtrim(@i_prefix),'"','')+@tmpInvNo  else   
						(case when rtrim(@i_suffix)<>'' then @tmpInvNo+replace(rtrim(@i_suffix),'"','') else @tmpInvNo end) end) end  
		--Added by Divyang for Bug-33358 on 25/04/2020 End
		--Commented by Divyang for Bug-33358 on 25/04/2020 Start
			--Set @tmpInvNo1=case when rtrim(@i_prefix)<>'' then replace(rtrim(@i_prefix),'''','')+@tmpInvNo else 
			--		(case when rtrim(@i_suffix)<>'' then @tmpInvNo+replace(rtrim(@i_suffix),'''','') else @tmpInvNo end) end
		--Commented by Divyang for Bug-33358 on 25/04/2020 End
			
			set @genInvCond=''
		End

		
		if rtrim(@s_type)='DAYWISE'
		Begin
			set @day=substring(convert(varchar(10),@date,103),1,2)+substring(convert(varchar(10),@date,103),4,2)+substring(convert(varchar(10),@date,103),9,2)
			--Added by Divyang for Bug-33358 on 25/04/2020 Start
			Set @tmpInvNo1=case when rtrim(@i_prefix)<>'' and rtrim(@i_suffix)<>'' then replace(rtrim(@i_prefix),'"','')+@day+@tmpInvNo+replace(rtrim(@i_suffix),'"','') else 
			(case when rtrim(@i_prefix)<>'' then replace(rtrim(@i_prefix),'"','')+@day+@tmpInvNo else   
			(case when rtrim(@i_suffix)<>'' then @day+@tmpInvNo+replace(rtrim(@i_suffix),'"','') else @day+@tmpInvNo end) end) end	
			--Added by Divyang for Bug-33358 on 25/04/2020 End
			--Commented by Divyang for Bug-33358 on 25/04/2020 Start
			--Set @tmpInvNo1=case when rtrim(@i_prefix)<>'' then replace(rtrim(@i_prefix),'"','')+@day+@tmpInvNo else  
			--(case when rtrim(@i_suffix)<>'' then @day+@tmpInvNo+replace(rtrim(@i_suffix),'"','') else @day+@tmpInvNo end) end			
			--Commented by Divyang for Bug-33358 on 25/04/2020 End
			set @genInvCond=''
			SET @genInvCond= ' and  inv_dt='''''+convert(Varchar(50),@Date)+''''''
			
		End

		if rtrim(@s_type)='MONTHWISE'
		Begin
		print 'Rupesh g'+@monthformat
			if rtrim(@monthformat)<>''
			Begin
				set @SQLCOMMAND1=N'Select @monCond='+@monthformat
				set @ParmDefinition =N' @date smalldatetime,@monCond Varchar(50) Output'
				--print @SQLCOMMAND1
				EXECUTE sp_executesql @SQLCOMMAND1,@ParmDefinition,@date=@Date,@monCond=@mcon OUTPUT
				set @mon1=@mcon
			end
			--Added by Divyang for Bug-33358 on 25/04/2020 Start
			Set @tmpInvNo1=case when rtrim(@i_prefix)<>'' and rtrim(@i_suffix)<>'' then replace(rtrim(@i_prefix),'"','')+rtrim(@mcon)+@tmpInvNo+replace(rtrim(@i_suffix),'"','') else 
				(case when rtrim(@i_prefix)<>'' then replace(rtrim(@i_prefix),'"','')+rtrim(@mcon)+@tmpInvNo else  
				(case when rtrim(@i_suffix)<>'' then rtrim(@mcon)+@tmpInvNo+replace(rtrim(@i_suffix),'"','') else rtrim(@mcon)+@tmpInvNo end) end)end	 
			--Added by Divyang for Bug-33358 on 25/04/2020 End
			--Commented by Divyang for Bug-33358 on 25/04/2020 Start
				--Set @tmpInvNo1=case when rtrim(@i_prefix)<>'' then replace(rtrim(@i_prefix),'"','')+rtrim(@mcon)+@tmpInvNo else  
				--(case when rtrim(@i_suffix)<>'' then rtrim(@mcon)+@tmpInvNo+replace(rtrim(@i_suffix),'"','') else rtrim(@mcon)+@tmpInvNo end) end
			--Commented by Divyang for Bug-33358 on 25/04/2020 End
				set @genInvCond=''
				SET @genInvCond= ' and  convert(varchar(2),month(INV_DT))+convert(varchar(4),year(INV_DT))='''''+convert(Varchar(2),month(@Date))+convert(Varchar(4),year(@Date))+''''''

		End

		Select @tmp=Case rtrim(@s_type)
			When 'DAYWISE' Then (Case When rtrim(@i_prefix)<>'' Then replace(rtrim(@i_prefix),'"','') Else '' End)+'******'   --Divyang
			When 'MONTHWISE' Then (Case when rtrim(@monthformat)<>'' then (Case When rtrim(@i_prefix)<>'' Then replace(rtrim(@i_prefix),'"','') Else '' End)+rtrim(@mon1)  else (Case When rtrim(@i_prefix)<>'' Then replace(rtrim(@i_prefix),'"','') Else '' End)+'****' end)   --Divyang
			else ''+(Case When rtrim(@i_prefix)<>'' Then replace(rtrim(@i_prefix),'"','') Else '' End) End --Divyang
		--set @tmp=Case When len(@tmp)>0 Then Substring(rtrim(@Inv_No),len(@tmp)+1,6) Else rtrim(@Inv_No) End			--Commented by Shrikant S. on 16/05/2019
		set @tmp=Case When len(@tmp)>0 Then Substring(rtrim(@Inv_No),len(@tmp)+1,@invno_size) Else rtrim(@Inv_No) End
		--print @tmp+'shrikant'+@tmpInvNo1
	set @UpdateOrNot=0	

	Declare Condtbl_cur Cursor for
	Select tblType,tbl_nm,fld_nm,link_cond,Link_cond1 From #Update_tables
	Open Condtbl_cur 
	Fetch next From Condtbl_cur Into @tblType,@tbl_nm,@fld_nm,@Link_cond,@Link_cond1
	While @@Fetch_Status=0
	Begin
		--PRINT @tbl_nm
		--Commented by Shrikant S. on 17/5/2019			--Start
--			if rtrim(@Link_cond)<>'' and CharIndex('MAIN',@tbl_nm)>0
--			Begin
--				set @SQLCOMMAND1=N'If Not Exists(Select '+@fld_nm+' from '+@tbl_nm+' Where '+@Link_cond+'='''+(@Entry_ty+convert(Varchar(10),@tran_cd))+''' And ' +@fld_nm+'='+' '''+@tmpInvNo1+''' )'
----			Else
----				set @SQLCOMMAND1=N'If Not Exists(Select '+@fld_nm+' from '+@tbl_nm+' Where '+@Link_cond1+'='''+(rtrim(@Entry_ty)+rtrim(@Series)+rtrim(convert(Varchar(10),convert(numeric,@Inv_no))))+''' And ' +@fld_nm+'='+convert(Varchar(10),convert(Numeric,@tmpInvNo))+' )'
--				set @SQLCOMMAND1=@SQLCOMMAND1+' Begin set @Upd=1  End Else Begin set @Upd=0 End'
--				set @ParmDefinition =N' @Upd Bit Output'
--				print @SQLCOMMAND1
--				EXECUTE sp_executesql @SQLCOMMAND1,@ParmDefinition,@Upd=@UpdateOrNot OUTPUT
--			End
			--Commented by Shrikant S. on 17/5/2019			--End

			SET @UpdateOrNot=1					--Added by Shrikant S. on 17/5/2019			--Start
			if @UpdateOrNot<>0
			Begin
				if rtrim(@Link_cond)<>''
				Begin
					set @SQLCOMMAND2=''
					set @SQLCOMMAND2=' insert into #tmpRENUMVOU values('''+@Entry_ty+''',''' +@Code_nm+''','''+rtrim(@Series)+''','''+@Inv_no+''','''+@tmpInvNo1+''','''+@Entry_ty+convert(Varchar(10),@tran_cd)+''','''+@tbl_nm+''','''+' Update '+@tbl_nm+' with (TABLOCKX) set '+@fld_nm+'='+' '''''+@tmpInvNo1+'''''  Where '+@Link_cond+'= '''''+(@Entry_ty+convert(Varchar(10),@tran_cd))+''''' '+''','''+rtrim(@Link_cond)+''','''+rtrim(@l_yn)+''','''+convert(varchar(50),@Date)+''','+convert(varchar(10),@tran_cd)+' )'
					print @SQLCOMMAND2
					EXECUTE sp_executesql @SQLCOMMAND2
				End
				else
				Begin 
					if rtrim(@tbl_nm)<>'GEN_INV' 
					Begin
						set @GenInvNo=replace(replace( @tmpInvNo,@i_prefix,''),@i_suffix,'')
						set @SQLCOMMAND2=''
						--set	@SQLCOMMAND2='insert into #tmpRENUMVOU values('''+@Entry_ty+''',''' +@Code_nm+''','''+rtrim(@Series)+''', '''+CONVERT(VARCHAR(10),CONVERT(NUMERIC,@tmp))+''','''+convert(Varchar(10),convert(Numeric,@tmpInvNo))+''','''+(rtrim(@Entry_ty)+rtrim(@Series)+rtrim(@Inv_no))+''','''+@tbl_nm+''','''+' Update '+@tbl_nm+' with (TABLOCKX) set '+@fld_nm+'='+convert(Varchar(10),convert(Numeric,@tmpInvNo))+' Where '+@Link_cond1+'= '''''+(rtrim(@Entry_ty)+rtrim(@Series)+rtrim(convert(Varchar(10),convert(Numeric,@tmp))))+''''' '+''' ,'''+rtrim(@Link_cond1)+''','''+rtrim(@l_yn)+''','''+convert(varchar(50),@Date)+''','+convert(varchar(10),@tran_cd) +')'
						set	@SQLCOMMAND2='insert into #tmpRENUMVOU values('''+@Entry_ty+''',''' +@Code_nm+''','''+rtrim(@Series)+''', '''+CONVERT(VARCHAR(10),CONVERT(NUMERIC,@tmp))+''','''+convert(Varchar(20),convert(Numeric,@GenInvNo))+''','''+(rtrim(@Entry_ty)+rtrim(@Series)+rtrim(@Inv_no))+''','''+@tbl_nm+''','''+' Update '+@tbl_nm+' with (TABLOCKX) set '+@fld_nm+'='+convert(Varchar(10),convert(Numeric,@GenInvNo))+' Where '+@Link_cond1+'= '''''+(rtrim(@Entry_ty)+rtrim(@Series)+rtrim(convert(Varchar(10),convert(Numeric,@tmp))))+''''' '+''' ,'''+rtrim(@Link_cond1)+''','''+rtrim(@l_yn)+''','''+convert(varchar(50),@Date)+''','+convert(varchar(10),@tran_cd) +')'
						print @SQLCOMMAND2
						EXECUTE sp_executesql @SQLCOMMAND2
					End
					else
					Begin
						set @GenInvNo=replace(replace( @tmpInvNo,@i_prefix,''),@i_suffix,'')
						set @SQLCOMMAND2=''
						--set	@SQLCOMMAND2='insert into #tmpRENUMVOU values('''+@Entry_ty+''',''' +@Code_nm+''','''+rtrim(@Series)+''', '''+CONVERT(VARCHAR(10),CONVERT(NUMERIC,@tmp))+''','''+convert(Varchar(10),convert(Numeric,@tmpInvNo))+''','''+(rtrim(@Entry_ty)+rtrim(@Series)+rtrim(@Inv_no))+''','''+@tbl_nm+''','''+' Update '+@tbl_nm+' with (TABLOCKX) set '+@fld_nm+'='+convert(Varchar(10),convert(Numeric,@tmpInvNo))+' Where '+@Link_cond1+'= '''''+(rtrim(@Entry_ty)+rtrim(@Series))+''''' and inv_dt='''''+convert(varchar(50),@Date)+''''''+''','''+rtrim(@Link_cond1)+''','''+rtrim(@l_yn)+''','''+convert(varchar(50),@Date)+''','+convert(varchar(10),@tran_cd) +')'
						
						--set	@SQLCOMMAND2='insert into #tmpRENUMVOU values('''+@Entry_ty+''',''' +@Code_nm+''','''+rtrim(@Series)+''', '''+CONVERT(VARCHAR(10),CONVERT(NUMERIC,@tmp))+''','''+convert(Varchar(10),convert(Numeric,@tmpInvNo))+''','''+(rtrim(@Entry_ty)+rtrim(@Series)+rtrim(@Inv_no))+''','''+@tbl_nm+''','''+' Update '+@tbl_nm+' with (TABLOCKX) set '+@fld_nm+'='+convert(Varchar(10),convert(Numeric,@tmpInvNo))+' Where '+@Link_cond1+'= '''''+(rtrim(@Entry_ty)+rtrim(@Series))+''''' '+@genInvCond+''','''+rtrim(@Link_cond1)+''','''+rtrim(@l_yn)+''','''+convert(varchar(50),@Date)+''','+convert(varchar(10),@tran_cd) +')'		--Commented by Shrikant S. on 16/05/2019
						set	@SQLCOMMAND2='insert into #tmpRENUMVOU values('''+@Entry_ty+''',''' +@Code_nm+''','''+rtrim(@Series)+''', '''+CONVERT(VARCHAR(10),CONVERT(NUMERIC,@tmp))+''','''+convert(Varchar(10),convert(Numeric,@GenInvNo))+''','''+(rtrim(@Entry_ty)+rtrim(@Series)+rtrim(@Inv_no))+''','''+@tbl_nm+''','''+' Update '+@tbl_nm+' with (TABLOCKX) set '+@fld_nm+'='+convert(Varchar(10),convert(Numeric,@GenInvNo))+' Where '+@Link_cond1+'= '''''+(rtrim(@Entry_ty)+rtrim(@Series))+''''' '+@genInvCond+''','''+rtrim(@Link_cond1)+''','''+rtrim(@l_yn)+''','''+convert(varchar(50),@Date)+''','+convert(varchar(10),@tran_cd) +')'			--Added by Shrikant S. on 16/05/2019
						print @SQLCOMMAND2
						EXECUTE sp_executesql @SQLCOMMAND2
					End
				End
				
				print 'insert '+@monthformat
				print 'insert '+@mon1
				print 'insert '+@mcon

				print 'insert start'
			--	Select right(cast(@Inv_no as varchar), len(@Inv_no)-len(@mon1))
				
				--update GEN_MISS set flag='N' where inv_no=right(cast(@Inv_no as varchar), len(@Inv_no)-len(@mon1))   --Commented by Divyang for Bug-33358 on 25/04/2020 
				update GEN_MISS set flag='N' where inv_no=right(cast(@Inv_no as varchar), len(@Inv_no)-len(@mon1)-LEN(replace((@i_prefix),'"','')))	and inv_sr=@Series	--Added by Divyang for Bug-33358 on 25/04/2020 
				
				Insert into RenumVou values(@Entry_ty,@tran_cd,@tbl_nm,@Series,@Inv_no,@tmpInvNo1,case when rtrim(@Link_cond)<>'' then rtrim(@Link_cond) else rtrim(@Link_cond1) end,@l_yn,@SDATE,@EDATE,@date,Getdate(),@UserName)
				
			End 
			Fetch next From Condtbl_cur Into @tblType,@tbl_nm,@fld_nm,@Link_cond,@Link_cond1
	End
	Close Condtbl_cur
	Deallocate Condtbl_cur

	

	Fetch Next From Upd_cur Into @Entry_ty,@tran_cd,@Date,@Inv_no,@Series,@s_type,@i_prefix,@i_suffix,@l_yn
	set @cnt=@cnt+1
	set @tmpInvNo=replicate('0',len(@tmpInvNo)-len(convert(Varchar(5),convert(Numeric,@cnt))))+convert(varchar(10),@cnt)
	if rtrim(@s_type)='DAYWISE'
	Begin
		if @Date<>@date1 
			Begin
				--set @cnt=convert(Numeric,@startno)			--Commented by Shrikant S. on 16/05/2019
				set @cnt=convert(Numeric,Case when @Series='' then right(@startno,@invno_size) else @startno end )		--Added by Shrikant S. on 16/05/2019

				--set @tmpInvNo=@startno			--Commented by Shrikant S. on 16/05/2019
				set @tmpInvNo=Case when @Series='' then right(@startno,@invno_size) else @startno end		--Added by Shrikant S. on 16/05/2019
				set @date1 =@Date
			End
		End	
		if rtrim(@s_type)='MONTHWISE'
		Begin
		print @monthformat
		print @mon1
		print @mcon

			
			print 'RUP'+@monthformat
			if rtrim(@monthformat)<>''
			Begin
				set @SQLCOMMAND1=N'Select @monCond='+@monthformat
				set @ParmDefinition =N' @date smalldatetime,@monCond Varchar(50) Output'
				--print @SQLCOMMAND1
				EXECUTE sp_executesql @SQLCOMMAND1,@ParmDefinition,@date=@Date,@monCond=@mcon OUTPUT
			end
			if @mon1<>@mcon 
			Begin
				--set @cnt=convert(Numeric,@startno)			--Commented by Shrikant S. on 16/05/2019
				set @cnt=convert(Numeric,Case when @Series='' then right(@startno,@invno_size) else @startno end )		--Added by Shrikant S. on 16/05/2019

				--set @tmpInvNo=@startno			--Commented by Shrikant S. on 16/05/2019
				set @tmpInvNo=Case when @Series='' then right(@startno,@invno_size) else @startno end		--Added by Shrikant S. on 16/05/2019
				set @mon1=@mcon
			End
		End			
End
Close Upd_cur
Deallocate Upd_cur

--select * from #tmpRENUMVOU

--select * from #tmpRENUMVOU	where Tbl_nm='PTMAIN'--where rtrim(entry_ty)=@Entry_ty and rtrim(series)=@Series and tbl_nm='gen_miss'


DELETE FROM Gen_miss 
FROM Gen_miss AS a
    INNER JOIN #tmpRENUMVOU AS b
    ON ( a.inv_no=convert(Numeric,b.prevInvno) 
and convert(smalldatetime,a.inv_dt)=convert(smalldatetime,b.invdt) )
WHERE rtrim(a.entry_ty)=@Entry_ty and rtrim(a.inv_sr)=@Series and b.tbl_nm='gen_miss'


Declare @exeState Varchar(2000),@cond Varchar(50),@filtcond Varchar (200)
Declare Renumcur Cursor for
Select exeState,tbl_nm,cond,filtcond,Entry_ty,PrevInvno,Newinvno,l_yn,series,invdt,tran_cd from #tmpRENUMVOU
Open Renumcur
Fetch Next from Renumcur Into @exeState,@tbl_nm,@cond,@filtcond,@Entry_ty,@tmpInvNo,@Inv_no,@l_yn,@Series,@date,@tran_cd
if rtrim(@tbl_nm)='GEN_MISS'
	Begin
		set @SQLCOMMAND='Insert Into GEN_MISS (Entry_ty,inv_sr,Inv_no,Flag,l_yn,inv_dt,user_name,compid) Values ('''+@Entry_ty+''','''+rtrim(@Series)+''','''+@Inv_no+''',''Y'','''+@l_yn+''','''+convert(varchar(50),@date)+''','''',0)' 
	End
Else
	Begin
		set @SQLCOMMAND=@exeState
	End
	--PRINT @SQLCOMMAND
	EXECUTE sp_executesql @SQLCOMMAND	
While @@Fetch_Status=0
Begin
	Fetch Next from Renumcur Into @exeState,@tbl_nm,@cond,@filtcond,@Entry_ty,@tmpInvNo,@Inv_no,@l_yn,@Series,@date,@tran_cd
	if rtrim(@tbl_nm)='GEN_MISS'
	Begin
		set @SQLCOMMAND='Insert Into GEN_MISS (Entry_ty,inv_sr,Inv_no,Flag,l_yn,inv_dt,user_name,compid) Values ('''+@Entry_ty+''','''+rtrim(@Series)+''','''+@Inv_no+''',''Y'','''+@l_yn+''','''+convert(varchar(50),@date)+''','''',0)' 
	End
	Else
	Begin
		set @SQLCOMMAND=@exeState
	End
	--PRINT @SQLCOMMAND
	EXECUTE sp_executesql @SQLCOMMAND	
End
Close Renumcur
Deallocate Renumcur


Select a.entry_ty,b.Code_nm,a.Inv_sr,a.PrevInvno,a.NewInvno from RenumVou a 
Inner join #tmpLcode b on (a.entry_ty=b.entry_ty) 
where a.FromDate between @sdate and @edate and Inv_sr=@Series and a.Entry_ty=@Entry_ty
Group by a.entry_ty,b.Code_nm,a.Inv_sr,a.PrevInvno,a.NewInvno
Order by a.entry_ty,b.Code_nm,a.Inv_sr,a.PrevInvno,a.NewInvno

Drop Table #tmpRENUMVOU
Drop table #tmpLcode 
Drop table #tmpSeries
Drop table #Update_tables




