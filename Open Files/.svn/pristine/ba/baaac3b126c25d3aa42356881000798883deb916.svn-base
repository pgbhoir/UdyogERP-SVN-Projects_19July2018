If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Rep_MIS_PD')
Begin
	Drop Procedure Usp_Rep_MIS_PD
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Create By Prajakta B. on 22/07/2019
Create PROCEDURE [dbo].[Usp_Rep_MIS_PD] 
	(@FrmDate SMALLDATETIME,@Todate SMALLDATETIME,@FParty nvarchar(100),@TParty nvarchar(100),@FSeries nvarchar(100),@TSeries nvarchar(100),@FDept nvarchar(100),@TDept nvarchar(100),@FCategory nvarchar(100),@TCategory nvarchar(100),@FWarehouse nvarchar(100),@TWarehouse nvarchar(100)) 	
	AS
	
	Declare @SQLCOMMAND NVARCHAR(max),@SQLCOMMAND1 NVARCHAR(max),@ParmDefinition NVARCHAR(max),@SQLCOMMAND2 NVARCHAR(max),@QueryString nvarchar(max)
Declare @chapno varchar(30),@eit_name  varchar(100),@mchapno varchar(250),@meit_name  varchar(250)
			Declare @pformula varchar(100),@progcond varchar(250),@progopamt numeric(17,2)
			Declare @Entry_ty Varchar(2),@Tran_cd Numeric,@Date smalldatetime,@Progtotal Numeric(19,2),@Inv_no Varchar(20),@Progcurrtotal Numeric(19,2)

--Added by Divyang P on 18032020 for Bug-33349  Start
Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@fld_type  varchar(15),@tempsql varchar(50),@head_nm varchar(100)
Select case when att_file=1 then 'EQMAIN.'+RTRIM(FLD_NM) else 'EQITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='PD' and DISP_MIS=1
union all
Select case when att_file=1 then 'EQMAIN.'+RTRIM(FLD_NM) else 'EQITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='PD' and DISP_MIS=1
union all
Select case when att_file=1 then 'EQMAIN.'+RTRIM(pert_name) else 'EQITEM.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm= (rtrim(head_nm)+'PER')  From DcMast
Where Entry_ty='PD' and DISP_MIS=1
--Added by Divyang P on 18032020 for Bug-33349  End


SET @QueryString ='Select EQMAIN.[date],EQMAIN.ENTRY_TY,EQMAIN.TRan_cd,EQMAIN.inv_no,EQMAIN.party_nm,EQMAIN.net_amt,EQITEM.item_no,EQITEM.item,cast(EQMAIN.narr as varchar(400)) AS narr,
EQITEM.qty,EQITEM.rate,it_mast.rateunit,It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END)
,EQITEM.qty*EQITEM.rate as U_ASSEAMT,AC_MAST.gstin,AC_MAST.state,HSNCODE = (CASE WHEN ISSERVICE=1 THEN IT_MAST.SERVTCODE ELSE IT_MAST.HSNCODE END)'

--Added by Divyang P on 18032020 for Bug-33349  Start
		set @Tot_flds =''
	Declare addi_flds cursor for
	Select flds,fld_nm,att_file,data_ty,head_nm from #tmpFlds
	Open addi_flds
	Fetch Next from addi_flds Into @fld,@fld_nm1,@att_file,@fld_type,@head_nm
	While @@Fetch_Status=0
	Begin
		if  charindex(@fld,@QueryString)=0
		begin
			if  charindex(@fld_type,'text')<>0
				begin
				 Set @Tot_flds=@Tot_flds+','+'CONVERT(VARCHAR(500),'+@fld+') AS '+substring(@fld,charindex('.',@fld)+1,len(@fld))  
				end
			else
			begin
				print @fld
				--Set @Tot_flds=@Tot_flds+','+@fld +' as '+ rtrim(@head_nm)
				Set @Tot_flds=@Tot_flds+','+@fld +' as ['+ rtrim(@head_nm) +']'	
			end
		End
		Fetch Next from addi_flds Into @fld,@fld_nm1,@att_file,@fld_type,@head_nm
	End
	Close addi_flds 
	Deallocate addi_flds 
	declare @sql as nvarchar(max)
	set @sql= cAST(REPLICATE(@Tot_flds,4000) as nvarchar(100))
	print @Tot_flds
	--Added by Divyang P on 18032020 for Bug-33349  END

	SET @SQLCOMMAND=''
	SET @SQLCOMMAND = N''+@QueryString+''+N''+@Tot_flds+''+' into '+'##ipmain11'+' FROM ipmain' 
	

SET @SQLCOMMAND=''
--SET @SQLCOMMAND = N''+@QueryString+''+N''+' into ##main11 FROM EQMAIN '     --Commented by Divyang P on 18032020 for Bug-3339
SET @SQLCOMMAND = N''+@QueryString+''+N''+@Tot_flds+''+' into ##main11 FROM EQMAIN ' 
SET @SQLCOMMAND= @SQLCOMMAND+'
inner join EQITEM on EQMAIN.tran_cd=EQITEM.tran_cd 
left join it_mast on it_mast.it_code=EQITEM.it_code 
inner join ac_mast on EQMAIN.ac_id=ac_mast.ac_id'
SET @SQLCOMMAND =	@SQLCOMMAND+' WHERE EQMAIN.entry_ty=''PD'' and (EQMAIN.date BETWEEN '+CHAR(39)+cast(@FrmDate as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@Todate as varchar )+CHAR(39)+') and (EQMAIN.Party_nm BETWEEN '+CHAR(39)+@FParty+CHAR(39)+' AND '+CHAR(39)+@TParty+CHAR(39)+')'
SET @SQLCOMMAND =	@SQLCOMMAND+'and (EQMAIN.inv_sr BETWEEN '+CHAR(39)+cast(@FSeries as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TSeries as varchar )+CHAR(39)+') and (EQMAIN.Dept BETWEEN '+CHAR(39)+@FDept+CHAR(39)+' AND '+CHAR(39)+@TDept+CHAR(39)+')'
SET @SQLCOMMAND =	@SQLCOMMAND+'and (EQMAIN.cate BETWEEN '+CHAR(39)+cast(@FCategory as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TCategory as varchar )+CHAR(39)+') and (EQitem.ware_nm BETWEEN '+CHAR(39)+@FWarehouse+CHAR(39)+' AND '+CHAR(39)+@TWarehouse+CHAR(39)+')'
SET @SQLCOMMAND= @SQLCOMMAND+' Order by item_no' 
print @sqlcommand
execute sp_executesql @sqlcommand



SET @SQLCOMMAND ='select a.date,a.inv_no,a.party_nm,a.GSTIN,a.state,a.item,a.HSNCODE,a.qty,a.rateunit,a.rate,a.U_ASSEAMT,a.net_amt,a.narr'
--SET @SQLCOMMAND=@SQLCOMMAND+' from ##main11'	--Commented by Divyang P on 18032020 for Bug-3339
SET @SQLCOMMAND = @SQLCOMMAND + N''+@Tot_flds+''+ ' from ##main11 a inner join EQMAIN on (EQMAIN.tran_cd=a.tran_cd and a.entry_ty=EQMAIN.entry_ty) inner join EQITEM on (EQITEM.tran_cd=EQMAIN.tran_cd and EQITEM.entry_ty=EQMAIN.entry_ty) '
execute sp_executesql @SQLCOMMAND
drop table ##main11
	

--exec sp_executesql N'execute Usp_Rep_MIS_PD @FDate,@TDate,@FParty,@TParty',N'@FParty nvarchar(4000),@TParty nvarchar(10),@FDate nvarchar(10),@TDate nvarchar(10)',@FParty=N'',@TParty=N'WORK ORDER',@FDate=N'04/01/2019',@TDate=N'03/31/2020'
--go