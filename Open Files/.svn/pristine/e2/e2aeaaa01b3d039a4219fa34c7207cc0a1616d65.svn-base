If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_PR_MIS')
Begin
	Drop Procedure USP_REP_PR_MIS
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[USP_REP_PR_MIS]
	(@FrmDate SMALLDATETIME,@Todate SMALLDATETIME,@FParty nvarchar(100),@TParty nvarchar(100),@FSeries nvarchar(100),@TSeries nvarchar(100),@FDept nvarchar(100),@TDept nvarchar(100),@FCategory nvarchar(100),@TCategory nvarchar(100),@FWarehouse nvarchar(100),@TWarehouse nvarchar(100))
	AS

Declare @SQLCOMMAND NVARCHAR(max),@TBLCON NVARCHAR(max),@SQLCOMMAND1 NVARCHAR(max),@ParmDefinition NVARCHAR(max),@SQLCOMMAND2 NVARCHAR(max)
Declare @chapno varchar(30),@eit_name  varchar(100),@mchapno varchar(250),@meit_name  varchar(250)
	
	
			Declare @pformula varchar(100),@progcond varchar(250),@progopamt numeric(17,2)
			Declare @Entry_ty Varchar(2),@Tran_cd Numeric,@Date smalldatetime,@Progtotal Numeric(19,2),@Inv_no Varchar(20),@Progcurrtotal Numeric(19,2)

	Select Entry_ty,Tran_cd=0 Into #PRMAIN from PRMAIN Where 1=0
	set @sqlcommand='Insert Into #PRMAIN Select PRMAIN.Entry_ty,PRMAIN.Tran_cd from PRMAIN Where PRMAIN.Entry_ty = ''PR'''
		print @sqlcommand
		execute sp_executesql @sqlcommand
		

		select @pformula=isnull(pformula,''),@progcond=isnull(progcond,''),@progopamt=isnull(progopamt,0)  from manufact

--ruchit

declare @ITAddLess varchar(500),@NonTaxIT varchar(500),@HeaderDisc varchar(500),@HeaderTAX nvarchar(max),@HeaderNTX nvarchar(max),@ItemTaxable nvarchar(max),@ItemNTX nvarchar(max),@ItemAddl nvarchar(max),@GSTReceivable nvarchar(max)
--itemwise 
SELECT 
    @ITAddLess = isnull(STUFF(
                 (SELECT Case when a_s<>'' then a_s else (case when code in( 'D','F') THEN '-' else '+' end ) end + fld_nm FROM Dcmast where  code in( 'D','F')  and att_file=0 and Entry_ty='PR'  FOR XML PATH ('')), 1, 0, ''
               ),0)
--itemwise taxable and non taxable
SELECT 
    @ItemTaxable = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code='T' and att_file=0 and Entry_ty='PR'  FOR XML PATH ('')), 1, 0, ''
                 --(select case when fld_nm<>'' then '+'+fld_nm else ''  end from dcmast where code='T' and att_file=0 and entry_ty='PR' and bef_aft=2  FOR XML PATH ('')), 1, 0, ''
               ),0)
              
         
SELECT 
    @ItemNTX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=0 and Entry_ty='PR' and fld_nm not in ('CGSRT_AMT','SGSRT_AMT','IGSRT_AMT','Comrpcess','Compcess','FCCGSRT_AM','FCSGSRT_AM','FCIGSRT_AM') /*and bef_aft=2*/  FOR XML PATH ('')), 1, 0, ''
               ),0)      
--itemwise non tax

/*cgstreceivable*/
SELECT 
    @GSTReceivable = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=0 and Entry_ty='PR' and fld_nm in ('CGSRT_AMT','SGSRT_AMT','IGSRT_AMT','FCCGSRT_AM','FCSGSRT_AM','FCIGSRT_AM')/*and bef_aft=2*/  FOR XML PATH ('')), 1, 0, ''
               ),0)
/*cgstreceivable*/

--headerwise disc
SELECT 
    @HeaderDisc = isnull(STUFF(
                 (SELECT  '-'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('D','F') and att_file=1  and Entry_ty='PR'  FOR XML PATH ('')), 1, 0, ''
               ),0)

--headerwise taxable chargs
SELECT 
    @HeaderTAX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code='T' and att_file=1 and Entry_ty='PR'  FOR XML PATH ('')), 1, 0, ''
               ),0)


--headerwise non taxable chargs
SELECT 
    @HeaderNTX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=1 and Entry_ty='PR'  FOR XML PATH ('')), 1, 0, ''
               ),0)
--ruchit


Declare @sta_dt datetime, @end_dt datetime 
set @SQLCOMMAND1='select top 1 @sta_dt=sta_dt,@end_dt=end_dt from vudyog..co_mast where dbname=db_name() and ( select top 1 PRMAIN.date  from PRMAIN inner join PRITEM on (PRMAIN.tran_cd=PRITEM.tran_cd) where PRMAIN.Entry_ty = ''PR'' ) between sta_dt and end_dt '	
set @ParmDefinition =N' @sta_dt datetime Output, @end_dt datetime Output'
EXECUTE sp_executesql @SQLCOMMAND1,@ParmDefinition,@sta_dt=@sta_dt Output, @end_dt=@end_dt Output


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
		set @stkl_qty= @stkl_qty +', '+'PRITEM.'+@fld_nm

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
	set @stkl_qty=',PRITEM.QTY'
End

Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50),@head_nm varchar(100)
Select case when att_file=1 then 'PRMAIN.'+RTRIM(FLD_NM) else 'PRITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='PR'	and DISP_MIS=1
union all
Select case when att_file=1 then 'PRMAIN.'+RTRIM(FLD_NM) else 'PRITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='PR'	and DISP_MIS=1
--Added by Divyang P on 18032020 for Bug-33349  Start
union all
Select case when att_file=1 then 'PRMAIN.'+RTRIM(pert_name) else 'PRITEM.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm= (rtrim(head_nm)+'PER')  From DcMast
Where Entry_ty='PR' and DISP_MIS=1
--Added by Divyang P on 18032020 for Bug-33349  End



SET @QueryString ='SELECT [Financial Year]=prmain.l_yn,YearMonth=convert(varchar(4),year(prmain.date))+''-''+convert(varchar(2),prmain.date,101),[Month]=left(datename(month,prmain.date),3)+''-''+right(year(prmain.date),2),    --Modified by Divyang for Tkt-33129 DT:18/12/2019   fld:l_yn,YearMonth,Month,Location
AC_NAME=(CASE WHEN PRMAIN.sac_id> 0 THEN (CASE WHEN ISNULL(SHIPTO.MailName,'''')='''' THEN AC_MAST.mailname ELSE SHIPTO.mailname END) ELSE AC_MAST.mailname END)
,ADD1=(CASE WHEN PRMAIN.sac_id> 0 THEN (CASE WHEN ISNULL(SHIPTO.add1,'''')='''' THEN AC_MAST.add1 ELSE SHIPTO.ADD1 END) ELSE AC_MAST.ADD1 END)
,ADD2=(CASE WHEN PRMAIN.sac_id> 0 THEN (CASE WHEN ISNULL(SHIPTO.add2,'''')='''' THEN AC_MAST.add2 ELSE SHIPTO.add2 END) ELSE AC_MAST.ADD2 END)
,ADD3=(CASE WHEN PRMAIN.sac_id> 0 THEN (CASE WHEN ISNULL(SHIPTO.add3,'''')='''' THEN AC_MAST.add3 ELSE SHIPTO.add3 END) ELSE AC_MAST.ADD3 END)
,City=(CASE WHEN PRMAIN.sac_id> 0 THEN (CASE WHEN ISNULL(SHIPTO.city,'''')='''' THEN AC_MAST.city ELSE SHIPTO.city END) ELSE AC_MAST.city END)
,Zip=(CASE WHEN PRMAIN.sac_id> 0 THEN (CASE WHEN ISNULL(SHIPTO.zip,'''')='''' THEN AC_MAST.zip ELSE SHIPTO.zip END) ELSE AC_MAST.zip END),PRMAIN.INV_NO,PRMAIN.DATE,PRMAIN.GRO_AMT AS V_GRO_AMT,
PRITEM.CGST_AMT,PRITEM.SGST_AMT,PRITEM.IGST_AMT,PRMAIN.TOT_DEDUC,PRMAIN.TOT_ADD,PRMAIN.TOT_TAX,PRMAIN.TOT_NONTAX,PRMAIN.NET_AMT,PRITEM.ITEM_NO,
PRITEM.ITEM,CAST(PRMAIN.NARR AS VARCHAR(400)) AS NARR,PRITEM.QTY,PRITEM.RATE,PRITEM.GRO_AMT,IT_MAST.RATEUNIT,PRMAIN.Tran_cd,PRITEM.U_ASSEAMT,
It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END),
MailName=(CASE WHEN PRMAIN.sac_id> 0 THEN (CASE WHEN ISNULL(SHIPTO.MailName,'''')='''' THEN AC_MAST.mailname ELSE SHIPTO.mailname END) ELSE AC_MAST.mailname END)
,U_DELIVER=(CASE WHEN PRMAIN.scons_id > 0 THEN (CASE WHEN ISNULL(SHIPTO1.MailName,'''')='''' THEN AC_MAST1.ac_name ELSE SHIPTO1.mailname END) ELSE AC_MAST1.AC_NAME END)
,ADD11=(CASE WHEN PRMAIN.scons_id> 0 THEN (CASE WHEN ISNULL(SHIPTO1.add1,'''')='''' THEN AC_MAST1.add1 ELSE SHIPTO1.ADD1 END) ELSE AC_MAST.ADD1 END)
,ADD22=(CASE WHEN PRMAIN.scons_id> 0 THEN (CASE WHEN ISNULL(SHIPTO1.add2,'''')='''' THEN AC_MAST1.add2 ELSE SHIPTO1.add2 END) ELSE AC_MAST.ADD2 END)
,ADD33=(CASE WHEN PRMAIN.scons_id> 0 THEN (CASE WHEN ISNULL(SHIPTO1.add3,'''')='''' THEN AC_MAST1.add3 ELSE SHIPTO1.add3 END) ELSE AC_MAST1.ADD3 END)
,City1=(CASE WHEN PRMAIN.scons_id> 0 THEN (CASE WHEN ISNULL(SHIPTO1.city,'''')='''' THEN AC_MAST1.city ELSE SHIPTO1.city END) ELSE AC_MAST1.city END)
,Zip1=(CASE WHEN PRMAIN.scons_id> 0 THEN (CASE WHEN ISNULL(SHIPTO1.zip,'''')='''' THEN AC_MAST1.zip ELSE SHIPTO1.zip END) ELSE AC_MAST1.zip END),item_fdisc=pritem.tot_fdisc
 ,pritem.compcess,pritem.comrpcess,GSTState as Placeofsupply,PRitref.rinv_no as PurchaseBillNo,PRitref.rdate PurchaseBillDate
 ,hsncode=(case when it_mast.isservice=1 then it_mast.servtcode else it_mast.HSNCODE end)
 ,AC_MAST.gstin,Location=shipto.location_id' 
SET @QueryString =@QueryString+',ITADDLESS='+@ITAddLess+',ITEMTAX='+@ItemTaxable+',ITEMNTX='+@ItemNTX+',HEADERDISC='+@HeaderDisc+''--ruchit
SET @QueryString =@QueryString+',HEADERTAX='+@HeaderTAX+',HEADERNTX='+@HeaderNTX+',GSTReceivable='+@GSTReceivable+''
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
			--Set @Tot_flds=@Tot_flds+','+@fld     --Commented by Divyang P on 18032020 for Bug-33349
			--Set @Tot_flds=@Tot_flds+','+@fld +' as '+ rtrim(@head_nm)		 --Added by Divyang P on 18032020 for Bug-33349
			Set @Tot_flds=@Tot_flds+','+@fld +' as ['+ rtrim(@head_nm) +']'	
		end
	End
	Fetch Next from addi_flds Into @fld,@fld_nm1,@att_file,@fld_type,@head_nm
End
Close addi_flds 
Deallocate addi_flds 
declare @sql as nvarchar(max)
set @sql= cAST(REPLICATE(@Tot_flds,4000) as nvarchar(100))
--set @Tot_flds =''    --Commented by Divyang P on 18032020 for Bug-33349
SET @SQLCOMMAND=''
SET @SQLCOMMAND = N''+@QueryString+''+N''+@Tot_flds+''+' into ##main11 FROM PRMAIN ' 
SET @SQLCOMMAND= @SQLCOMMAND+'
INNER JOIN PRITEM ON PRMAIN.TRAN_CD=PRITEM.TRAN_CD 
INNER JOIN PRitref ON (PRMAIN.TRAN_CD=PRitref.TRAN_CD and PRMAIN.Entry_ty=PRitref.entry_ty)

LEFT JOIN IT_MAST ON IT_MAST.IT_CODE=PRITEM.IT_CODE 
INNER JOIN AC_MAST ON PRMAIN.AC_ID=AC_MAST.AC_ID 
INNER JOIN AC_MAST AC_MAST1 ON (AC_MAST1.AC_ID=PRMAIN.CONS_ID)
LEFT JOIN SHIPTO ON (SHIPTO.AC_ID=PRMAIN.Ac_id AND SHIPTO.Shipto_id=PRMAIN.sac_id) 
LEFT JOIN SHIPTO SHIPTO1 ON(SHIPTO1.AC_ID=PRMAIN.CONS_id AND SHIPTO1.Shipto_id =PRMAIN.scons_id)
  

INNER JOIN #PRMAIN ON (PRMAIN.TRAN_CD=#PRMAIN.TRAN_CD and PRMAIN.Entry_ty=#PRMAIN.entry_ty ) 
'
SET @SQLCOMMAND =	@SQLCOMMAND+'where  (PRMAIN.inv_sr BETWEEN '+CHAR(39)+cast(@FSeries as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TSeries as varchar )+CHAR(39)+') and (PRMAIN.Dept BETWEEN '+CHAR(39)+@FDept+CHAR(39)+' AND '+CHAR(39)+@TDept+CHAR(39)+')'
SET @SQLCOMMAND =	@SQLCOMMAND+'and (PRMAIN.cate BETWEEN '+CHAR(39)+cast(@FCategory as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TCategory as varchar )+CHAR(39)+') and (PRitem.ware_nm BETWEEN '+CHAR(39)+@FWarehouse+CHAR(39)+' AND '+CHAR(39)+@TWarehouse+CHAR(39)+')'

execute sp_executesql @sqlcommand

--SET @SQLCOMMAND='select [Financial Year],YearMonth,[Month],Date,MailName as Seller,INV_NO as TransactionNo,Placeofsupply,QTY,Rate,PurchaseBillNo,PurchaseBillDate,CGST_AMT,SGST_AMT,IGST_AMT,NET_AMT,ReasonforReturn=''01-Purchase Return'',hsncode,gstin,Location,IT_Desc from ##main11'         --Modified by Divyang for Tkt-33129 DT:18/12/2019   fld:l_yn,YearMonth,Month,Location   --Commented by Divyang P on 18032020 for Bug-33349

--Added by Divyang P on 18032020 for Bug-33349  Start
SET @SQLCOMMAND=''
SET @SQLCOMMAND ='select [Financial Year],YearMonth,[Month],a.Date,a.MailName as Seller,a.INV_NO as TransactionNo,a.Placeofsupply,a.QTY,a.Rate,a.PurchaseBillNo,a.PurchaseBillDate,a.CGST_AMT,a.SGST_AMT,a.IGST_AMT,a.NET_AMT,ReasonforReturn=''01-Purchase Return'',a.hsncode,a.gstin,a.Location,a.IT_Desc  '
SET @SQLCOMMAND = @SQLCOMMAND + N''+@Tot_flds+''+ ' from ##main11 a inner join PRMAIN on (PRMAIN.tran_cd=a.tran_cd) inner join PRITEM on (PRITEM.tran_cd=PRMAIN.tran_cd) '
--Added by Divyang P on 18032020 for Bug-33349  END

execute sp_executesql @SQLCOMMAND
drop table ##main11

--set DATEFORMAT dmy EXECUTE USP_REP_PR_MIS N'01/01/2018',N'31/01/2019','Party 1',N'Party 1'
--select PRitref.rinv_no as PurchaseBillNo,PRitref.rdate PurchaseBillDate,* from PRitref