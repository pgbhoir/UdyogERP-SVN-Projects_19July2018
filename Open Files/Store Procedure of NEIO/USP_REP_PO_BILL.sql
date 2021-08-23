If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_PO_BILL')
Begin
	Drop Procedure USP_REP_PO_BILL
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[USP_REP_PO_BILL]
	@ENTRYCOND NVARCHAR(254)
	AS

Declare @SQLCOMMAND NVARCHAR(max),@TBLCON NVARCHAR(max),@SQLCOMMAND1 NVARCHAR(max),@ParmDefinition NVARCHAR(max),@SQLCOMMAND2 NVARCHAR(max)
Declare @chapno varchar(30),@eit_name  varchar(100),@mchapno varchar(250),@meit_name  varchar(250)
	
	SET @TBLCON=RTRIM(@ENTRYCOND)
			Declare @pformula varchar(100),@progcond varchar(250),@progopamt numeric(17,2)
			Declare @Entry_ty Varchar(2),@Tran_cd Numeric,@Date smalldatetime,@Progtotal Numeric(19,2),@Inv_no Varchar(20),@Progcurrtotal Numeric(19,2)

	Select Entry_ty,Tran_cd=0 Into #POMAIN from POMAIN Where 1=0
	set @sqlcommand='Insert Into #POMAIN Select POMAIN.Entry_ty,POMAIN.Tran_cd from POMAIN Where '+@TBLCON
		print @sqlcommand
		execute sp_executesql @sqlcommand
		
		select @pformula=isnull(pformula,''),@progcond=isnull(progcond,''),@progopamt=isnull(progopamt,0)  from manufact

--ruchit

declare @ITAddLess varchar(500),@NonTaxIT varchar(500),@HeaderDisc varchar(500),@HeaderTAX nvarchar(max),@HeaderNTX nvarchar(max),@ItemTaxable nvarchar(max),@ItemNTX nvarchar(max),@ItemAddl nvarchar(max),@GSTReceivable nvarchar(max)
,@CessRecv varchar(500),@CessAmt Varchar(500)
--itemwise 
SELECT 
    @ITAddLess = isnull(STUFF(
                 (SELECT Case when a_s<>'' then a_s else (case when code in( 'D','F') THEN '-' else '+' end ) end + fld_nm FROM Dcmast where  code in( 'D','F') and att_file=0 and Entry_ty='PO'  FOR XML PATH ('')), 1, 0, ''
               ),0)
--itemwise taxable and non taxable
SELECT 
    @ItemTaxable = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code='T' and att_file=0 and Entry_ty='PO' FOR XML PATH ('')), 1, 0, ''
                 --(select case when fld_nm<>'' then '+'+fld_nm else ''  end from dcmast where code='T' and att_file=0 and entry_ty='PO' and bef_aft=2  FOR XML PATH ('')), 1, 0, ''
               ),0)

 --Commented by Prajakta B. on 03-06-17   
--SELECT 
--    @ItemNTX = isnull(STUFF(
--                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=0 and Entry_ty='AR' /*and bef_aft=2*/  FOR XML PATH ('')), 1, 0, ''
--               ),0)      
                          
               
--added by Prajakta B. on 03-06-17 
--Commented Start By Prajakta B on 28082017              
--SELECT 
--    @ItemNTX = isnull(STUFF(
--                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=0 and Entry_ty='AR' and fld_nm not in ('CGSRT_AMT','SGSRT_AMT','IGSRT_AMT','FCCGSRT_AM','FCSGSRT_AM','FCIGSRT_AM') /*and bef_aft=2*/  FOR XML PATH ('')), 1, 0, ''
--               ),0)  

--Modified Start By Prajakta B on 28082017               
SELECT 
    @ItemNTX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=0 and Entry_ty='PO' and fld_nm not in ('CGSRT_AMT','SGSRT_AMT','IGSRT_AMT','Comrpcess','Compcess','FCCGSRT_AM','FCSGSRT_AM','FCIGSRT_AM') /*and bef_aft=2*/  FOR XML PATH ('')), 1, 0, ''
               ),0)      
    
--Added By Prajakta B. on 28082017  
SELECT 
	 @CessRecv = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=0 and Entry_ty='PO' and fld_nm in ('Comrpcess')/*and bef_aft=2*/  FOR XML PATH ('')), 1, 0, ''
               ),0)  
                   

--itemwise non tax

/*cgstreceivable*/
SELECT 
    @GSTReceivable = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=0 and Entry_ty='PO' and fld_nm in ('CGSRT_AMT','SGSRT_AMT','IGSRT_AMT','FCCGSRT_AM','FCSGSRT_AM','FCIGSRT_AM')/*and bef_aft=2*/  FOR XML PATH ('')), 1, 0, ''
               ),0)
/*cgstreceivable*/       


--headerwise disc
SELECT 
    @HeaderDisc = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('D','F') and att_file=1  and Entry_ty='PO'  FOR XML PATH ('')), 1, 0, ''
               ),0)

--headerwise taxable chargs
SELECT 
    @HeaderTAX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code='T' and att_file=1 and Entry_ty='PO'  FOR XML PATH ('')), 1, 0, ''
               ),0)


--headerwise non taxable chargs
SELECT 
    @HeaderNTX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=1 and Entry_ty='PO'  FOR XML PATH ('')), 1, 0, ''
               ),0)
--ruchit


Declare @sta_dt datetime, @end_dt datetime 
set @SQLCOMMAND1='select top 1 @sta_dt=sta_dt,@end_dt=end_dt from vudyog..co_mast where dbname=db_name() and ( select top 1 POMAIN.date  from POMAIN inner join POITEM on (POMAIN.tran_cd=POITEM.tran_cd) where '+@TBLCON+' ) between sta_dt and end_dt '	
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
		set @stkl_qty= @stkl_qty +', '+'POITEM.'+@fld_nm

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
	set @stkl_qty=',POITEM.QTY'
End

Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50)
Select case when att_file=1 then 'POMAIN.'+RTRIM(FLD_NM) else 'POITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='PO'
union all
Select case when att_file=1 then 'POMAIN.'+RTRIM(FLD_NM) else 'POITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='PO'
--Added By Divyang P for Bug-33349 on 13032020 Start
union all
Select case when att_file=1 then 'POMAIN.'+RTRIM(pert_name) else 'POITEM.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='PO'
--Added By Divyang P for Bug-33349 on 13032020 End



--Modify by Prajakta B. on 06-07-2017
SET @QueryString ='SELECT POMAIN.INV_SR,POMAIN.TRAN_CD,POMAIN.ENTRY_TY,POMAIN.INV_NO,POMAIN.DATE,POMAIN.DUE_DT,
POMAIN.GRO_AMT GRO_AMT1,POMAIN.TAX_NAME,POMAIN.TAXAMT,POMAIN.NET_AMT,POMAIN.SLIPNO,POMAIN.TOT_NONTAX,POMAIN.U_PAYMENT,POMAIN.U_TERMSD,POMAIN.U_REMARKS1,POMAIN.U_DISPATCH,
POMAIN.TOT_TAX,POMAIN.TOT_DEDUC,POITEM.CGST_AMT,POITEM.SGST_AMT,POITEM.IGST_AMT,POMAIN.ROUNDOFF,
CONVERT(VARCHAR(254),POMAIN.NARR) AS NARR,CONVERT(VARCHAR(254),POITEM.NARR) AS ITNARR,POMAIN.USER_NAME,POITEM.GRO_AMT,POITEM.ITEM_NO,POITEM.CGST_PER,POITEM.SGST_PER,POITEM.IGST_PER,
POITEM.QTY,POITEM.RATE,POITEM.U_ASSEAMT,IT_MAST.IT_NAME AS ITEM,
It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END),
MailName=(CASE WHEN ISNULL(ac_mast.MailName,'''')='''' THEN ac_mast.ac_name ELSE SHIPTO.mailname END),
MailName1=(CASE WHEN ISNULL(ac_mast1.MailName,'''')='''' THEN ac_mast1.ac_name ELSE SHIPTO1.mailname END),
IT_MAST.[GROUP],IT_MAST.EIT_NAME,IT_MAST.CHAPNO,IT_MAST.RATEUNIT,it_mast.HSNCODE
,ac_name=(CASE WHEN POMAIN.sac_id> 0 THEN SHIPTO.ac_name ELSE AC_MAST.ac_name END)
,AC_MAST.i_tax,AC_MAST.supp_type,AC_MAST.gstin,AC_MAST.[state],AC_MAST.statecode
,ADD1=AC_MAST.ADD1
,ADD2=AC_MAST.ADD2
,ADD3=AC_MAST.ADD3
,City=AC_MAST.city
,Zip=AC_MAST.zip
,contact=AC_MAST.contact
,phone=AC_MAST.phone
,phoner=AC_MAST.phoner
,email=AC_MAST.email
,i_tax=AC_MAST.i_tax

,add11=(case when POMAIN.scons_id> 0 then shipto1.add1 else AC_MAST1.ADD1 end)
,add22=(case when POMAIN.scons_id> 0 then shipto1.add2 else AC_MAST1.ADD2 end)
,add33=(case when POMAIN.scons_id> 0 then shipto1.add3 else AC_MAST1.ADD3 end)
,city1=(case when POMAIN.scons_id> 0 then shipto1.city else AC_MAST1.city end)
,zip1=(case when POMAIN.scons_id> 0 then shipto1.zip else AC_MAST1.zip end)
,contact1=(case when POMAIN.scons_id> 0 then shipto1.contact else AC_MAST1.contact end)
,phone1=(case when POMAIN.scons_id> 0 then shipto1.phone else AC_MAST1.phone end)
,phoner1=(case when POMAIN.scons_id> 0 then shipto1.phoner else AC_MAST1.phoner end)
,email1=(case when POMAIN.scons_id> 0 then shipto1.email else AC_MAST1.email end)
,i_tax1=(case when POMAIN.scons_id> 0 then shipto1.I_tax else AC_MAST1.i_tax end)
,supp_type1=(case when POMAIN.scons_id> 0 then shipto1.supp_type else AC_MAST1.supp_type end)
,gstin1=(case when POMAIN.scons_id> 0 then shipto1.gstin else AC_MAST1.gstin end)
,state1=(case when POMAIN.scons_id> 0 then shipto1.state else AC_MAST1.state end)
,statecode1=(case when POMAIN.scons_id> 0 then shipto1.statecode else AC_MAST1.statecode end)
,POMAIN.Ac_id,POMAIN.CONS_ID,POMAIN.SAC_ID,POMAIN.SCONS_ID
,poitem.compcess,poitem.comrpcess'
SET @QueryString =@QueryString+',ITADDLESS='+@ITAddLess+',ITEMTAX='+@ItemTaxable+',ITEMNTX='+@ItemNTX+',HEADERDISC='+@HeaderDisc+''--ruchit
SET @QueryString =@QueryString+',HEADERTAX='+@HeaderTAX+',HEADERNTX='+@HeaderNTX++',GSTReceivable='+@GSTReceivable+',Cessrecv='+@CessRecv+''
set @Tot_flds =''

Declare addi_flds cursor for
Select flds,fld_nm,att_file,data_ty from #tmpFlds
Open addi_flds
Fetch Next from addi_flds Into @fld,@fld_nm1,@att_file,@fld_type
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
			Set @Tot_flds=@Tot_flds+','+@fld   
		end
	End
	Fetch Next from addi_flds Into @fld,@fld_nm1,@att_file,@fld_type 
End
Close addi_flds 
Deallocate addi_flds 
declare @sql as nvarchar(max)
set @sql= cAST(REPLICATE(@Tot_flds,4000) as nvarchar(100))
--set @Tot_flds =''						--Commented By Divyang P for Bug-33349 on 13032020 
SET @SQLCOMMAND=''
SET @SQLCOMMAND = N''+@QueryString+''+N''+@Tot_flds+''+' FROM POMAIN' 
SET @SQLCOMMAND= @SQLCOMMAND+'
INNER JOIN POITEM ON (POMAIN.TRAN_CD=POITEM.TRAN_CD) '
--INNER JOIN #POMAIN ON (POITEM.TRAN_CD=#POMAIN.TRAN_CD and POITEM.Entry_ty=#POMAIN.entry_ty and POITEM.ITSERIAL=#POMAIN.itserial)
SET @SQLCOMMAND= @SQLCOMMAND+'INNER JOIN IT_MAST ON (POITEM.IT_CODE=IT_MAST.IT_CODE) 
INNER JOIN AC_MAST ON (AC_MAST.AC_ID=POMAIN.AC_ID) 
INNER JOIN AC_MAST AC_MAST1 ON (AC_MAST1.AC_ID=POMAIN.CONS_ID)
INNER JOIN #POMAIN ON (POMAIN.TRAN_CD=#POMAIN.TRAN_CD and POMAIN.Entry_ty=#POMAIN.entry_ty ) 
LEFT JOIN SHIPTO ON (SHIPTO.AC_ID=POMAIN.Ac_id AND SHIPTO.Shipto_id=POMAIN.sac_id)
LEFT JOIN SHIPTO SHIPTO1 ON(SHIPTO1.AC_ID=POMAIN.CONS_id AND SHIPTO1.Shipto_id =POMAIN.scons_id)
ORDER BY POMAIN.DATE,POMAIN.INV_SR,POMAIN.INV_NO'
--WHERE  POMAIN.ENTRY_TY=''SO''

execute sp_executesql @sqlcommand
