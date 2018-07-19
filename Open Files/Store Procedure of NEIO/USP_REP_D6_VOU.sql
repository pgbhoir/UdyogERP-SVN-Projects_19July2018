DROP PROCEDURE [USP_REP_D6_VOU]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shrikant S.
-- Create date: 17/10/2016
-- Description:	This Stored procedure is useful to generate Sales Invoice .
-- Remark: 
-- =============================================
CREATE PROCEDURE [USP_REP_D6_VOU]
	@ENTRYCOND NVARCHAR(254)
	AS

Declare @SQLCOMMAND NVARCHAR(max),@TBLCON NVARCHAR(max),@SQLCOMMAND1 NVARCHAR(max),@ParmDefinition NVARCHAR(max),@SQLCOMMAND2 NVARCHAR(max)
Declare @chapno varchar(30),@eit_name  varchar(100),@mchapno varchar(250),@meit_name  varchar(250)
	
SET @TBLCON=RTRIM(@ENTRYCOND)
			Declare @pformula varchar(100),@progcond varchar(250),@progopamt numeric(17,2)
			Declare @Entry_ty Varchar(2),@Tran_cd Numeric,@Date smalldatetime,@Progtotal Numeric(19,2),@Inv_no Varchar(20),@Progcurrtotal Numeric(19,2)


		select @pformula=isnull(pformula,''),@progcond=isnull(progcond,''),@progopamt=isnull(progopamt,0)  from manufact

--ruchit

declare @ITAddLess varchar(500),@NonTaxIT varchar(500),@HeaderDisc varchar(500),@HeaderTAX nvarchar(max),@HeaderNTX nvarchar(max),@ItemTaxable nvarchar(max),@ItemNTX nvarchar(max),@ItemAddl nvarchar(max)
--itemwise 
SELECT 
    @ITAddLess = isnull(STUFF(
                 (SELECT Case when a_s<>'' then a_s else (case when code in( 'D','F') THEN '-' else '+' end ) end + fld_nm FROM Dcmast where code in( 'D','F') and att_file=0 and Entry_ty='D6'  FOR XML PATH ('')), 1, 0, ''
               ),0)
--itemwise taxable and non taxable
SELECT 
    @ItemTaxable = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code='T' and att_file=0 and Entry_ty='D6' and fld_nm not in ('staxamt') FOR XML PATH ('')), 1, 0, ''
                 --(select case when fld_nm<>'' then '+'+fld_nm else ''  end from dcmast where code='T' and att_file=0 and entry_ty='PT' and bef_aft=2  FOR XML PATH ('')), 1, 0, ''
               ),0)
               
               
               
SELECT 
    @ItemNTX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=0 and Entry_ty='D6' and fld_nm not in  ('CGSRT_AMT','SGSRT_AMT','IGSRT_AMT') FOR XML PATH ('')), 1, 0, ''
               ),0)     

--and fld_nm not in ('CGSRT_AMT','SGSRT_AMT','IGSRT_AMT')           

--itemwise non tax

--headerwise disc
SELECT 
    @HeaderDisc = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('D','F') and att_file=1  and Entry_ty='D6'  FOR XML PATH ('')), 1, 0, ''
               ),0)

--headerwise taxable chargs
SELECT 
    @HeaderTAX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code='T' and att_file=1 and Entry_ty='D6'  FOR XML PATH ('')), 1, 0, ''
               ),0)


--headerwise non taxable chargs
SELECT 
    @HeaderNTX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=1 and Entry_ty='D6'  FOR XML PATH ('')), 1, 0, ''
               ),0)
--ruchit

Declare @sta_dt datetime, @end_dt datetime 
set @SQLCOMMAND1='select top 1 @sta_dt=sta_dt,@end_dt=end_dt from vudyog..co_mast where dbname=db_name() and ( select top 1 DNMAIN.date  from DNMAIN inner join DNITEM on (DNMAIN.tran_cd=DNITEM.tran_cd) where '+@TBLCON+' ) between sta_dt and end_dt '	
set @ParmDefinition =N' @sta_dt datetime Output, @end_dt datetime Output'
EXECUTE sp_executesql @SQLCOMMAND1,@ParmDefinition,@sta_dt=@sta_dt Output, @end_dt=@end_dt Output

Select Entry_ty,Tran_cd=0,Date,inv_no,itserial=space(6) Into #DNMAIN from DNMAIN Where 1=0
	Create NonClustered Index Idx_tmpDNMAIN On #DNMAIN (Entry_ty asc, Tran_cd Asc, Itserial asc)

		set @sqlcommand='Insert Into #DNMAIN Select DNMAIN.Entry_ty,DNMAIN.Tran_cd,DNMAIN.date,DNMAIN.inv_no,DNITEM.itserial from DNMAIN Inner Join DNITEM on (DNMAIN.Entry_ty=DNITEM.Entry_ty and DNMAIN.Tran_cd=DNITEM.Tran_cd) Where '+@TBLCON
		print @sqlcommand
		execute sp_executesql @sqlcommand


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
		set @stkl_qty= @stkl_qty +', '+'DNITEM.'+@fld_nm

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
	set @stkl_qty=',DNITEM.QTY'
End
Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50)
Select case when att_file=1 then 'DNMAIN.'+RTRIM(FLD_NM) else 'DNITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty 
Into #tmpFlds 
From Lother Where e_code='D6'
union all
Select case when att_file=1 then 'DNMAIN.'+RTRIM(FLD_NM) else 'DNITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty=''  From DcMast
Where Entry_ty='D6'

	SET @QueryString = ''
	SET @QueryString = 'SELECT ''REPORT HEADER'' AS REP_HEAD,DNMAIN.INV_SR,DNMAIN.TRAN_CD,DNMAIN.ENTRY_TY,DNMAIN.INV_NO,DNMAIN.DATE'
	SET @QueryString =@QueryString+',DNMAIN.DUE_DT,DNITEM.GRO_AMT AS IT_GROAMT,DNMAIN.GRO_AMT GRO_AMT1,DNMAIN.TAX_NAME,DNITEM.TAX_NAME AS IT_TAXNAME,DNMAIN.TAXAMT,DNITEM.TAXAMT AS IT_TAXAMT,DNMAIN.NET_AMT'+@stkl_qty+',RATE=ACDETALLOC.AMOUNT,DISCOUNT=ACDETALLOC.SABTAMT,EXPENSES=ACDETALLOC.SEXPAMT,U_ASSEAMT=STAXAMT, cast (DNITEM.NARR AS VARCHAR(2000)) AS NARR,DNITEM.TOT_EXAMT'
	SET @QueryString =@QueryString+',IT_MAST.IT_NAME,IT_MAST.EIT_NAME,IT_MAST.CHAPNO,IT_MAST.IDMARK,IT_MAST.RATEUNIT,IT_MAST.HSNCODE,IT_mast.U_ITPARTCD'
	SET @QueryString =@QueryString+',It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END)'
	SET @QueryString =@QueryString+',MailName=(CASE WHEN ISNULL(ac_mast.MailName,'''')='''' THEN ac_mast.ac_name ELSE ac_mast.mailname END)'
	SET @QueryString =@QueryString+',SAC_NAME=(CASE WHEN ISNULL(ac_mast.MailName,'''')='''' THEN ac_mast.ac_name ELSE ac_mast.mailname END)'
	SET @QueryString =@QueryString+',SADD1=CASE WHEN DNMAIN.SAC_ID >0 THEN (CASE WHEN ISNULL(S1.add1,'''')='''' THEN S1.add1 ELSE S1.ADD1 END) ELSE AC_MAST.ADD1 END'
	SET @QueryString =@QueryString+',SADD2=CASE WHEN DNMAIN.SAC_ID >0 THEN (CASE WHEN ISNULL(S1.ADD2,'''')='''' THEN S1.ADD2 ELSE S1.ADD2 END) ELSE AC_MAST.ADD2 END'
	SET @QueryString =@QueryString+',SADD3=CASE WHEN DNMAIN.SAC_ID >0 THEN (CASE WHEN ISNULL(S1.ADD3,'''')='''' THEN S1.ADD3 ELSE S1.ADD3 END) ELSE AC_MAST.ADD3 END'
	SET @QueryString =@QueryString+',SCITY=CASE WHEN DNMAIN.SAC_ID >0 THEN (CASE WHEN ISNULL(S1.CITY,'''')='''' THEN S1.ADD3 ELSE S1.CITY END) ELSE AC_MAST.CITY END'
	SET @QueryString =@QueryString+',SSTATE=CASE WHEN DNMAIN.SAC_ID >0 THEN S1.STATE ELSE AC_MAST.STATE END'
	SET @QueryString =@QueryString+',SCOUNTRY=CASE WHEN DNMAIN.SAC_ID >0 THEN S1.COUNTRY ELSE AC_MAST.COUNTRY END'
	SET @QueryString =@QueryString+',SZIP=CASE WHEN DNMAIN.SAC_ID >0 THEN S1.ZIP ELSE AC_MAST.ZIP END'
	SET @QueryString =@QueryString+',SPHONE=CASE WHEN DNMAIN.SAC_ID >0 THEN S1.PHONE ELSE AC_MAST.PHONE END'
	SET @QueryString =@QueryString+',SUNIQUEID=CASE WHEN DNMAIN.SAC_ID >0 THEN (CASE WHEN S1.UID<>'''' THEN S1.UID ELSE S1.GSTIN END) ELSE (CASE WHEN AC_MAST.UID<>'''' THEN AC_MAST.UID ELSE AC_MAST.GSTIN END) END'
	SET @QueryString =@QueryString+',SSTATECODE=(SELECT TOP 1 GST_STATE_CODE FROM STATE WHERE STATE=CASE WHEN DNMAIN.SAC_ID >0 THEN S1.STATE ELSE AC_MAST.STATE END)'	
	SET @QueryString =@QueryString+',CAC_NAME=CASE WHEN DNMAIN.SCONS_ID >0 THEN (CASE WHEN ISNULL(S2.MailName,'''')='''' THEN AC.MailName ELSE S2.MailName END) ELSE AC.MailName END'
	SET @QueryString =@QueryString+',CADD1=CASE WHEN DNMAIN.SCONS_ID >0 THEN (CASE WHEN ISNULL(S2.add1,'''')='''' THEN AC.add1 ELSE S2.ADD1 END) ELSE AC.ADD1 END'
	SET @QueryString =@QueryString+',CADD2=CASE WHEN DNMAIN.SCONS_ID >0 THEN (CASE WHEN ISNULL(S2.add2,'''')='''' THEN AC.add2 ELSE S2.ADD2 END) ELSE AC.ADD2 END'
	SET @QueryString =@QueryString+',CADD3=CASE WHEN DNMAIN.SCONS_ID >0 THEN (CASE WHEN ISNULL(S2.add3,'''')='''' THEN AC.add3 ELSE S2.ADD3 END) ELSE AC.ADD3 END'
	SET @QueryString =@QueryString+',CCITY=CASE WHEN DNMAIN.SCONS_ID >0 THEN (CASE WHEN ISNULL(S2.CITY,'''')='''' THEN AC.CITY ELSE S2.CITY END) ELSE AC.CITY END'
	SET @QueryString =@QueryString+',CSTATE=CASE WHEN DNMAIN.SCONS_ID >0 THEN S2.STATE ELSE AC.STATE END'
	SET @QueryString =@QueryString+',CZIP=CASE WHEN DNMAIN.SCONS_ID >0 THEN (CASE WHEN ISNULL(S2.ZIP,'''')='''' THEN AC.ZIP ELSE S2.ZIP END) ELSE AC.ZIP END'
	SET @QueryString =@QueryString+',CPHONE=CASE WHEN DNMAIN.SCONS_ID >0 THEN (CASE WHEN ISNULL(S2.PHONE,'''')='''' THEN AC.PHONE ELSE S2.PHONE END) ELSE AC.PHONE END'
	SET @QueryString =@QueryString+',CUNIQUEID=CASE WHEN DNMAIN.SCONS_ID >0 THEN (CASE WHEN S2.UID<>'''' THEN S2.UID ELSE S2.GSTIN END) ELSE (CASE WHEN AC.UID<>'''' THEN AC.UID ELSE AC.GSTIN END) END'	
	SET @QueryString =@QueryString+',CSTATECODE=(SELECT TOP 1 GST_STATE_CODE FROM STATE WHERE STATE=CASE WHEN DNMAIN.SCONS_ID >0 THEN S2.STATE ELSE AC.STATE END)'	
	SET @QueryString =@QueryString+',CCOUNTRY=CASE WHEN DNMAIN.SCONS_ID >0 THEN S2.COUNTRY ELSE AC.COUNTRY END'
	SET @QueryString =@QueryString+',DNMAIN.GSTSTATE,ST_TYPE=CASE WHEN DNMAIN.SCONS_ID >0 THEN S2.ST_TYPE ELSE AC.ST_TYPE END'	
	SET @QueryString =@QueryString+',DNITEM.ITSERIAL'
	SET @QueryString =@QueryString+',DNITEM.Tariff,DNMAIN.roundoff'
	SET @QueryString =@QueryString+',DNITEM.CGST_PER,DNITEM.CGST_AMT,DNITEM.SGST_PER,DNITEM.SGST_AMT,DNITEM.IGST_PER,DNITEM.IGST_AMT'
	SET @QueryString =@QueryString+',ITADDLESS='+@ITAddLess+',ITEMTAX='+@ItemTaxable+',ITEMNTX='+@ItemNTX+',HEADERDISC='+@HeaderDisc+''--ruchit
	SET @QueryString =@QueryString+',HEADERTAX='+@HeaderTAX+',HEADERNTX='+@HeaderNTX+''

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

SET @SQLCOMMAND=''
SET @SQLCOMMAND = N''+@QueryString+''+N''+@Tot_flds+''+' FROM DNMAIN' 
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN DNITEM ON (DNMAIN.TRAN_CD=DNITEM.TRAN_CD AND DNMAIN.ENTRY_TY=DNITEM.ENTRY_TY)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN #DNMAIN ON (DNITEM.TRAN_CD=#DNMAIN.TRAN_CD and DNITEM.Entry_ty=#DNMAIN.entry_ty and DNITEM.ITSERIAL=#DNMAIN.itserial) '
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN IT_MAST ON (DNITEM.IT_CODE=IT_MAST.IT_CODE)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN AC_MAST ON (AC_MAST.AC_ID=DNMAIN.AC_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN AC_MAST AC ON (AC.AC_ID=DNMAIN.CONS_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN ACDETALLOC ON (ACDETALLOC.ENTRY_TY=DNMAIN.ENTRY_TY AND ACDETALLOC.TRAN_CD=DNMAIN.TRAN_CD AND ACDETALLOC.ITSERIAL=DNITEM.ITSERIAL)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' LEFT JOIN SHIPTO S1 ON (DNMAIN.AC_ID=S1.AC_ID AND DNMAIN.SAC_ID=S1.SHIPTO_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' LEFT JOIN SHIPTO S2 ON (DNMAIN.CONS_ID=S2.AC_ID AND DNMAIN.SCONS_ID=S2.SHIPTO_ID)'
 
SET @SQLCOMMAND =	@SQLCOMMAND+' ORDER BY DNMAIN.INV_SR,DNMAIN.INV_NO'
print @sqlcommand
execute sp_executesql @sqlcommand
GO
