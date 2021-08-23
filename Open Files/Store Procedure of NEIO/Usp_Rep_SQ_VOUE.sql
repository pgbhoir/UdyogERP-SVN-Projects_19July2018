If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Rep_SQ_VOUE')
Begin
	Drop Procedure Usp_Rep_SQ_VOUE
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- AUTHOR:		
-- CREATE DATE: 20/01/2011
-- DESCRIPTION:	THIS STORED PROCEDURE IS USEFUL TO GENERATE SALES QUOTATION (EXPORT) REPORT.
-- REMARK:
-- =============================================

Create PROCEDURE   [dbo].[Usp_Rep_SQ_VOUE] 
@ENTRYCOND NVARCHAR(254)
AS
BEGIN
Declare @SQLCOMMAND as NVARCHAR(4000)
	--->ENTRY_TY AND TRAN_CD SEPARATION
		--DECLARE @ENT VARCHAR(2),@TRN INT,@POS1 INT,@POS2 INT,@POS3 INT--,@ENTRYCOND NVARCHAR(254)
		--PRINT @ENTRYCOND
		--SET @POS1=CHARINDEX('''',@ENTRYCOND,1)+1
		--SET @ENT= SUBSTRING(@ENTRYCOND,@POS1,2)
		--SET @POS2=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS1))+1
		--SET @POS3=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS2))+1
		--SET @TRN= SUBSTRING(@ENTRYCOND,@POS2,@POS2-@POS3)
	---<---ENTRY_TY AND TRAN_CD SEPARATION

--Added By Divyang P for Bug-33349 on 13032020 Start
Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50)
Select case when att_file=1 then 'SQMAIN.'+RTRIM(FLD_NM) else 'SQITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='Q2'
union all
Select case when att_file=1 then 'SQMAIN.'+RTRIM(FLD_NM) else 'SQITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='Q2'
union all
Select case when att_file=1 then 'SQMAIN.'+RTRIM(pert_name) else 'SQITEM.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='Q2'
--Added By Divyang P for Bug-33349 on 13032020 End




set @QueryString='SELECT AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.FAX,AC_MAST.ADD3,AC_MAST.CITY  ,AC_MAST.ZIP,SQMAIN.INV_NO,SQMAIN.DATE,CONVERT(VARCHAR(254),SQMAIN.NARR)   AS NARR,SQMAIN.GRO_AMT AS V_GRO_AMT, 
SQMAIN.TOT_DEDUC,SQMAIN.TOT_ADD,SQMAIN.TOT_TAX,SQMAIN.TOT_NONTAX  ,
SQMAIN.NET_AMT,SQITEM.ITEM_NO, SQITEM.ITEM,SQITEM.QTY,0 AS U_LENGTH,SQITEM.RATE,
SQMAIN.GRO_AMT,IT_MAST.RATEUNIT,
IT_DESC=(CASE WHEN ISNULL(IT_MAST.IT_ALIAS,'''')='''' THEN IT_MAST.IT_NAME ELSE IT_MAST.IT_ALIAS END),
MAILNAME=(CASE WHEN ISNULL(AC_MAST.MAILNAME,'''')='''' THEN AC_MAST.AC_NAME ELSE AC_MAST.MAILNAME END),	
FCRATE=CASE WHEN CURR_MAST.CURRDESC=''INR'' THEN SQITEM.RATE ELSE SQITEM.FCRATE END
,IFCGRO_AMT=CASE WHEN CURR_MAST.CURRDESC=''INR'' THEN SQITEM.GRO_AMT ELSE SQITEM.FCGRO_AMT END
,FCGRO_AMT=CASE WHEN CURR_MAST.CURRDESC=''INR'' THEN SQMAIN.GRO_AMT ELSE SQMAIN.FCGRO_AMT END,CURR_MAST.CURRDESC,
FCNET_AMT=CASE WHEN CURR_MAST.CURRDESC=''INR'' THEN SQMAIN.NET_AMT ELSE SQMAIN.FCNET_AMT END,SQITEM.GRO_AMT AS ITEMGROAMT,
SQITREF.RINV_NO,SQITREF.RDATE'

--Added By Divyang P for Bug-33349 on 13032020 Start
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
--Added By Divyang P for Bug-33349 on 13032020 End
SET @SQLCOMMAND = N''+@QueryString++''+N''+@Tot_flds+''+' FROM SQMAIN' 
set @sqlcommand= @SQLCOMMAND +' 

INNER JOIN SQITEM ON SQMAIN.TRAN_CD=SQITEM.TRAN_CD
LEFT OUTER JOIN SQITREF ON SQMAIN.TRAN_CD=SQITREF.TRAN_CD AND SQITEM.ITSERIAL=SQITREF.ITSERIAL
INNER JOIN CURR_MAST ON SQMAIN.FCID = CURR_MAST.CURRENCYID 
INNER JOIN IT_MAST ON IT_MAST.IT_CODE=SQITEM.IT_CODE 
INNER JOIN AC_MAST ON SQMAIN.AC_ID=AC_MAST.AC_ID  WHERE '+@ENTRYCOND
PRINT @sqlcommand
execute sp_executesql @sqlcommand

END
