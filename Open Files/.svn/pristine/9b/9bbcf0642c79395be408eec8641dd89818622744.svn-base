If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Rep_MIS_MWK')
Begin
	Drop Procedure Usp_Rep_MIS_MWK
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--Created By Prajakta B. on 14062019
Create PROCEDURE [dbo].[Usp_Rep_MIS_MWK] 
	(@FrmDate SMALLDATETIME,@Todate SMALLDATETIME,@FParty nvarchar(100),@TParty nvarchar(100),@FSeries nvarchar(100),@TSeries nvarchar(100),@FDept nvarchar(100),@TDept nvarchar(100),@FCategory nvarchar(100),@TCategory nvarchar(100),@FWarehouse nvarchar(100),@TWarehouse nvarchar(100)) 	
	AS
Declare @SQLCOMMAND NVARCHAR(max),@TBLCON NVARCHAR(max),@SQLCOMMAND1 NVARCHAR(max),
		@ParmDefinition NVARCHAR(max),@SQLCOMMAND2 NVARCHAR(max)
Declare @chapno varchar(30),@eit_name  varchar(100),
		@mchapno varchar(250),@meit_name  varchar(250)
Declare @pformula varchar(100),@progcond varchar(250),@progopamt numeric(17,2)
Declare @Entry_ty Varchar(2),@Tran_cd Numeric,@Date smalldatetime,@Progtotal Numeric(19,2),
		@Inv_no Varchar(20),@Progcurrtotal Numeric(19,2)
select @pformula=isnull(pformula,''),@progcond=isnull(progcond,''),@progopamt=isnull(progopamt,0)  from manufact
		
Declare @uom_desc as Varchar(100),@len int,@fld_nm varchar(10),@fld_desc Varchar(10),@count int,@stkl_qty Varchar(100)

select @uom_desc=isnull(uom_desc,'') from vudyog..co_mast where dbname =rtrim(db_name())
Create Table #qty_desc (fld_nm varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS, fld_desc varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS)
set @len=len(@uom_desc)
set @stkl_qty=''
If @len>0 
Begin
	while @len>0
	Begin
		set @fld_nm=substring(@uom_desc,1,charindex(':',@uom_desc)-1)
		set @uom_desc=substring(@uom_desc,charindex(':',@uom_desc)+1,@len)
		set @stkl_qty= @stkl_qty +', '+'item.'+@fld_nm

		if @len>0 and charindex(';',@uom_desc)=0
		begin
			set @uom_desc=@uom_desc
			set @fld_desc=@uom_desc
			SET @len=0
		End
		else
		begin
				set @fld_desc=substring(@uom_desc,1,charindex(';',@uom_desc)-1)
				set @uom_desc=substring(@uom_desc,charindex(';',@uom_desc)+1,@len)
				set @len=len(@uom_desc)
		End
		insert into #qty_desc values (@fld_nm,@fld_desc)
	End
End
Else
Begin
	set @stkl_qty=',item.QTY'
End


--Added by Divyang P on 18032020 for Bug-33349  Start
Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@fld_type  varchar(15),@tempsql varchar(50),@head_nm varchar(100)
Select case when att_file=1 then 'MAIN.'+RTRIM(FLD_NM) else 'ITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='WK' and DISP_MIS=1
union all
Select case when att_file=1 then 'MAIN.'+RTRIM(FLD_NM) else 'ITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='WK' and DISP_MIS=1
union all
Select case when att_file=1 then 'MAIN.'+RTRIM(pert_name) else 'ITEM.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm= (rtrim(head_nm)+'PER')  From DcMast
Where Entry_ty='WK' and DISP_MIS=1
--Added by Divyang P on 18032020 for Bug-33349  End


Declare @QueryString NVarchar(max)

	SET @QueryString = ''
	SET @QueryString = 'SELECT ''REPORT HEADER'' AS REP_HEAD,main.INV_SR,main.TRAN_CD,main.ENTRY_TY,main.INV_NO,main.DATE,item.qty'
	SET @QueryString =@QueryString+',item.GRO_AMT AS IT_GROAMT,main.GRO_AMT GRO_AMT1,main.NET_AMT,item.RATE,item.U_ASSEAMT, cast (item.NARR AS VARCHAR(2000)) AS NARR'
	SET @QueryString =@QueryString+',IT_MAST.IT_NAME,IT_MAST.RATEUNIT,HSNCODE = (CASE WHEN ISSERVICE=1 THEN IT_MAST.SERVTCODE ELSE IT_MAST.HSNCODE END),item.item_no'
	SET @QueryString =@QueryString+',It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END)'
	SET @QueryString =@QueryString+',main.Party_nm,ac_mast.GSTIN,ac_mast.State,item.item,item.stkunit as Unit,Total=item.rate*item.qty'
	SET @QueryString =@QueryString+',item.ITSERIAL,item.proc_id,item.mfgdt,item.expdt,item.batchno,item.bomid,item.bomlevel'
	
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
	--SET @SQLCOMMAND = N''+@QueryString+''+' into '+'##main11'+' FROM main'	--Commented by Divyang P on 18032020 for Bug-33349
	SET @SQLCOMMAND = N''+@QueryString+''+N''+@Tot_flds+''+' into '+'##main11'+' FROM main'		--Added by Divyang P on 18032020 for Bug-33349
	SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN item ON (main.TRAN_CD=item.TRAN_CD AND main.ENTRY_TY=item.ENTRY_TY)'
	SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN IT_MAST ON (item.IT_CODE=IT_MAST.IT_CODE)'
	SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN AC_MAST ON (AC_MAST.AC_ID=main.AC_ID)'
	SET @SQLCOMMAND =	@SQLCOMMAND+' LEFT JOIN SHIPTO S1 ON (main.AC_ID=S1.AC_ID)'
	SET @SQLCOMMAND =	@SQLCOMMAND+'WHERE (main.date BETWEEN '+CHAR(39)+cast(@FrmDate as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@Todate as varchar )+CHAR(39)+') and (main.Party_nm BETWEEN '+CHAR(39)+@FParty+CHAR(39)+' AND '+CHAR(39)+@TParty+CHAR(39)+')'
	SET @SQLCOMMAND =	@SQLCOMMAND+'and (main.inv_sr BETWEEN '+CHAR(39)+cast(@FSeries as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TSeries as varchar )+CHAR(39)+') and (main.Dept BETWEEN '+CHAR(39)+@FDept+CHAR(39)+' AND '+CHAR(39)+@TDept+CHAR(39)+')'
    SET @SQLCOMMAND =	@SQLCOMMAND+'and (main.cate BETWEEN '+CHAR(39)+cast(@FCategory as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TCategory as varchar )+CHAR(39)+') and (item.ware_nm BETWEEN '+CHAR(39)+@FWarehouse+CHAR(39)+' AND '+CHAR(39)+@TWarehouse+CHAR(39)+')'
	SET @SQLCOMMAND =	@SQLCOMMAND+' ORDER BY main.INV_SR,main.INV_NO'
	print @sqlcommand
	execute sp_executesql @SQLCOMMAND

SET @SQLCOMMAND ='select a.Date,a.inv_no,a.Party_nm,a.item,a.HSNCODE,a.qty,a.Unit,a.rate,a.Total,a.u_asseamt,a.IT_GROAMT,a.net_amt,a.proc_id,a.mfgdt,a.expdt,a.batchno,a.bomid,a.bomlevel '
SET @SQLCOMMAND = @SQLCOMMAND + N''+@Tot_flds+''+ ' from ##main11 a inner join main on (main.tran_cd=a.tran_cd) inner join item on (item.tran_cd=main.tran_cd) '	--Added by Divyang P on 18032020 for Bug-33349
execute sp_executesql @SQLCOMMAND
drop table ##main11





