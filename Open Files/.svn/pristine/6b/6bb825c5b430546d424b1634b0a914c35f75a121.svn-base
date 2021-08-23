If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Rep_MIS_MSALERTR')
Begin
	Drop Procedure Usp_Rep_MIS_MSALERTR
End
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--Created By Prajakta B. on 14062019
Create PROCEDURE [dbo].[Usp_Rep_MIS_MSALERTR] 
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
declare @ITAddLess varchar(500),@NonTaxIT varchar(500),@HeaderDisc varchar(500),@HeaderTAX nvarchar(max),@HeaderNTX nvarchar(max),@ItemTaxable nvarchar(max),@ItemNTX nvarchar(max),@ItemAddl nvarchar(max)
--itemwise 
SELECT 
    @ITAddLess = isnull(STUFF(
                 (SELECT Case when a_s<>'' then a_s else (case when code in( 'D','F') THEN '-' else '+' end ) end + fld_nm FROM Dcmast where att_file=0 and Entry_ty='TR' and code in( 'D','F')  FOR XML PATH ('')), 1, 0, ''
               ),0)               
--itemwise taxable and non taxable
SELECT 
    @ItemTaxable = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code='T' and att_file=0 and Entry_ty='TR'  FOR XML PATH ('')), 1, 0, ''
               ),0)
SELECT 
    @ItemNTX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=0 and Entry_ty='TR' and fld_nm not in ('Compcess') /*and bef_aft=2*/  FOR XML PATH ('')), 1, 0, ''
               ),0)   
             
--itemwise non tax

--headerwise disc
SELECT 
    @HeaderDisc = isnull(STUFF(
                 (SELECT  '-'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('D','F') and att_file=1  and Entry_ty='TR'  FOR XML PATH ('')), 1, 0, ''
               ),0)

--headerwise taxable chargs
SELECT 
    @HeaderTAX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code='T' and att_file=1 and Entry_ty='TR'  FOR XML PATH ('')), 1, 0, ''
               ),0)

--headerwise non taxable chargs
SELECT 
    @HeaderNTX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=1 and Entry_ty='TR'  FOR XML PATH ('')), 1, 0, ''
               ),0)


Declare @Addtblnm Varchar(1000)
set @Addtblnm  =''
if Exists(Select Top 1 [Name] From SysObjects Where xType='U' and [Name]='TRMAINAdd')
Begin
	set @Addtblnm =' LEFT JOIN TRMAINADD ON (TRMAIN.TRAN_CD=TRMAINADD.TRAN_CD) '
end	
		
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
		set @stkl_qty= @stkl_qty +', '+'TRITEM.'+@fld_nm

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
	set @stkl_qty=',TRITEM.QTY'
End

Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds nVarchar(max),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50),@head_nm varchar(100)
Select case when att_file=1 then (CASE WHEN RTRIM(TBL_NM)='TRMAINADD' THEN 'TRMAINADD.' ELSE 'TRMAIN.' END )+RTRIM(FLD_NM) else 'TRITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm
Into #tmpFlds 
From Lother Where e_code='TR' and DISP_MIS=1
union all
Select case when att_file=1 then 'TRMAIN.'+RTRIM(FLD_NM) else 'TRITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='TR'	 and DISP_MIS=1
--Added by Divyang P on 18032020 for Bug-33349  End
union all
Select case when att_file=1 then 'TRMAIN.'+RTRIM(pert_name) else 'TRITEM.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm= (rtrim(head_nm)+'PER')  From DcMast
Where Entry_ty='TR' and DISP_MIS=1
--Added by Divyang P on 18032020 for Bug-33349  End

	SET @QueryString = ''
	SET @QueryString = 'SELECT ''REPORT HEADER'' AS REP_HEAD,TRMAIN.INV_SR,TRMAIN.TRAN_CD,TRMAIN.ENTRY_TY,TRMAIN.INV_NO,TRMAIN.DATE,tritem.qty '
	SET @QueryString =@QueryString+',TRMAIN.DUE_DT,TRITEM.GRO_AMT AS IT_GROAMT,TRMAIN.GRO_AMT GRO_AMT1,TRMAIN.NET_AMT,TRITEM.RATE,TRITEM.U_ASSEAMT, cast (TRITEM.NARR AS VARCHAR(2000)) AS NARR,TRITEM.TOT_EXAMT'
	SET @QueryString =@QueryString+',IT_MAST.IT_NAME,IT_MAST.CHAPNO,IT_MAST.RATEUNIT,HSNCODE = (CASE WHEN ISSERVICE=1 THEN IT_MAST.SERVTCODE ELSE IT_MAST.HSNCODE END),TRITEM.item_no'
	SET @QueryString =@QueryString+',It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END)'
	SET @QueryString =@QueryString+',MailName=(CASE WHEN ISNULL(ac_mast.MailName,'''')='''' THEN ac_mast.ac_name ELSE ac_mast.mailname END)'
	SET @QueryString =@QueryString+',SAC_NAME=(CASE WHEN ISNULL(S1.MailName,'''')='''' THEN (CASE WHEN ISNULL(ac_mast.MailName,'''')='''' THEN ac_mast.ac_name ELSE ac_mast.mailname END)  ELSE S1.mailname END)'	
	SET @QueryString =@QueryString+',TRITEM.ITSERIAL,item_fdisc=TRITEM.tot_fdisc,tritref.rinv_no,tritref.rdate'
	SET @QueryString =@QueryString+',TRITEM.CGST_PER,TRITEM.CGST_AMT,TRITEM.SGST_PER,TRITEM.SGST_AMT,TRITEM.IGST_PER,TRITEM.IGST_AMT'
	SET @QueryString =@QueryString+',ITADDLESS='+@ITAddLess+',ITEMTAX='+@ItemTaxable+',ITEMNTX='+@ItemNTX+',HEADERDISC='+@HeaderDisc+''
	SET @QueryString =@QueryString+',HEADERTAX='+@HeaderTAX+',HEADERNTX='+@HeaderNTX+''
	SET @QueryString =@QueryString+',TRMAIN.Party_nm,ac_mast.GSTIN,ac_mast.State,TRITEM.item,TRITEM.stkunit,Total=TRITEM.rate*TRITEM.qty'
	SET @QueryString =@QueryString+',TotalGST=TRITEM.CGST_AMT+TRITEM.SGST_AMT+TRITEM.IGST_AMT'


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
			--Set @Tot_flds=@Tot_flds+','+@fld   --Commented by Divyang P on 18032020 for Bug-33349 
			--Set @Tot_flds=@Tot_flds+','+@fld +' as '+ rtrim(@head_nm)	--Added by Divyang P on 18032020 for Bug-33349 
			Set @Tot_flds=@Tot_flds+','+@fld +' as ['+ rtrim(@head_nm) +']'	--Added by Divyang P on 18032020 for Bug-33349
		end
	End
	Fetch Next from addi_flds Into @fld,@fld_nm1,@att_file,@fld_type,@head_nm 
End
Close addi_flds 
Deallocate addi_flds 
declare @sql as nvarchar(max)
set @sql= cAST(REPLICATE(@Tot_flds,4000) as nvarchar(100))

SET @SQLCOMMAND=''
SET @SQLCOMMAND = N''+@QueryString+''+N''+@Tot_flds+''+' into '+'##main11'+' FROM TRMAIN' 


 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN TRITEM ON (TRMAIN.TRAN_CD=TRITEM.TRAN_CD AND TRMAIN.ENTRY_TY=TRITEM.ENTRY_TY)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN IT_MAST ON (TRITEM.IT_CODE=IT_MAST.IT_CODE)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN AC_MAST ON (AC_MAST.AC_ID=TRMAIN.AC_ID)'
  SET @SQLCOMMAND =	@SQLCOMMAND+' LEFT JOIN TRITREF ON TRITEM.ENTRY_TY=TRITREF.ENTRY_TY AND TRITEM.TRAN_CD=TRITREF.TRAN_CD AND TRITEM.ITSERIAL=TRITREF.ITSERIAL'
 SET @SQLCOMMAND =	@SQLCOMMAND+' LEFT JOIN SHIPTO S1 ON (TRMAIN.AC_ID=S1.AC_ID AND TRMAIN.SAC_ID=S1.SHIPTO_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' WHERE (TRMAIN.date BETWEEN '+CHAR(39)+cast(@FrmDate as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@Todate as varchar )+CHAR(39)+') and (TRMAIN.Party_nm BETWEEN '+CHAR(39)+@FParty+CHAR(39)+' AND '+CHAR(39)+@TParty+CHAR(39)+')'
 SET @SQLCOMMAND =	@SQLCOMMAND+'and (TRMAIN.inv_sr BETWEEN '+CHAR(39)+cast(@FSeries as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TSeries as varchar )+CHAR(39)+') and (TRMAIN.Dept BETWEEN '+CHAR(39)+@FDept+CHAR(39)+' AND '+CHAR(39)+@TDept+CHAR(39)+')'
 SET @SQLCOMMAND =	@SQLCOMMAND+'and (TRMAIN.cate BETWEEN '+CHAR(39)+cast(@FCategory as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TCategory as varchar )+CHAR(39)+') and (TRitem.ware_nm BETWEEN '+CHAR(39)+@FWarehouse+CHAR(39)+' AND '+CHAR(39)+@TWarehouse+CHAR(39)+')'
 SET @SQLCOMMAND =	@SQLCOMMAND+' ORDER BY TRMAIN.INV_SR,TRMAIN.INV_NO'
 print @sqlcommand
execute sp_executesql @SQLCOMMAND


--SET @SQLCOMMAND ='select Date,rdate,inv_no,rinv_no,Party_nm,GSTIN,State,item,HSNCODE,qty,stkunit,rate,Total,ITEMTAX,u_asseamt,CGST_PER,CGST_AMT,SGST_PER,SGST_AMT,IGST_PER,IGST_AMT,TotalGST,IT_GROAMT,ITEMNTX,net_amt,ITADDLESS  from ##main11'		--Commented by Divyang P on 18032020 for Bug-33349 

--Added by Divyang P on 18032020 for Bug-33349  Start
SET @SQLCOMMAND=''
SET @SQLCOMMAND ='select a.Date,a.rdate,a.inv_no,a.rinv_no,a.Party_nm,a.GSTIN,a.State,a.item,a.HSNCODE,a.qty,a.stkunit,a.rate,a.Total,a.ITEMTAX,a.u_asseamt,a.CGST_PER,a.CGST_AMT,a.SGST_PER,a.SGST_AMT,a.IGST_PER,a.IGST_AMT,a.TotalGST,a.IT_GROAMT,a.ITEMNTX,a.net_amt,a.ITADDLESS '
SET @SQLCOMMAND = @SQLCOMMAND + N''+@Tot_flds+''+ ' from ##main11 a inner join TRMAIN on (TRMAIN.tran_cd=a.tran_cd) inner join TRITEM on (TRITEM.tran_cd=TRMAIN.tran_cd and TRITEM.item=a.it_name) '
--Added by Divyang P on 18032020 for Bug-33349  END
print @sqlcommand
execute sp_executesql @SQLCOMMAND
drop table ##main11





