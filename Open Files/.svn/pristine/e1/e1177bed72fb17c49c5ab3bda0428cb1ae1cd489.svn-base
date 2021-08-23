If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Rep_P5_VOU')
Begin
	Drop Procedure Usp_Rep_P5_VOU
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- CREATE : Divyang Panchal
-- DATE: 13/03/2020
-- DESCRIPTION:	THIS STORED PROCEDURE IS USEFUL TO GENERATE Purchase Order(Import) from Purchase Order(Import) transaction.
-- REMARK:
-- =============================================

Create PROCEDURE  [dbo].[Usp_Rep_P5_VOU] 
@ENTRYCOND NVARCHAR(254)
AS
BEGIN
Declare @SQLCOMMAND as NVARCHAR(max)
	--->ENTRY_TY AND TRAN_CD SEPARATION
		DECLARE @ENT VARCHAR(2),@TRN INT,@POS1 INT,@POS2 INT,@POS3 INT--,@ENTRYCOND NVARCHAR(254)
		PRINT @ENTRYCOND
		SET @POS1=CHARINDEX('''',@ENTRYCOND,1)+1
		SET @ENT= SUBSTRING(@ENTRYCOND,@POS1,2)
		SET @POS2=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS1))+1
		SET @POS3=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS2))+1
		SET @TRN= SUBSTRING(@ENTRYCOND,@POS2,@POS2-@POS3)
	---<---ENTRY_TY AND TRAN_CD SEPARATION


Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50)
Select case when att_file=1 then 'POMAIN.'+RTRIM(FLD_NM) else 'POITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='P5'
union all
Select case when att_file=1 then 'POMAIN.'+RTRIM(FLD_NM) else 'POITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='P5'
union all
Select case when att_file=1 then 'POMAIN.'+RTRIM(pert_name) else 'POITEM.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='P5'



set @QueryString ='SELECT AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.PHONE,AC_MAST.FAX,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,POMAIN.INV_NO,POMAIN.PINVNO,POMAIN.DATE,
CONVERT(VARCHAR(254)  ,POMAIN.NARR) AS NARR,'''' AS U_BROKER,POMAIN.GRO_AMT AS V_GRO_AMT,POMAIN.TAX_NAME,'''' AS U_OCT,POMAIN.TAXAMT,POMAIN.TOT_DEDUC,POMAIN.TOT_ADD,
POMAIN.TOT_TAX,POMAIN.TOT_NONTAX,POMAIN.NET_AMT,'''' AS U_OTHTRM,'''' AS U_PAYTERMS,'''' AS U_FREINTC,POITEM.ITEM_NO,POITEM.ITEM,CONVERT(VARCHAR(254),POITEM.NARR) AS NARR,
POITEM.QTY,POITEM.RATE,POITEM.GRO_AMT,IT_MAST.RATEUNIT,It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END),
MailName=(CASE WHEN ISNULL(MailName,'''')='''' THEN ac_name ELSE mailname END),CURR_MAST.CURRDESC,POITEM.FCRATE,POITEM.FCGRO_AMT '


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
SET @SQLCOMMAND = N''+@QueryString++''+N''+@Tot_flds+''+'  FROM POMAIN ' 
print @SQLCOMMAND
set @sqlcommand= @SQLCOMMAND +' 
INNER JOIN POITEM ON POMAIN.TRAN_CD=POITEM.TRAN_CD 
INNER JOIN IT_MAST ON IT_MAST.IT_CODE=POITEM.IT_CODE 
INNER JOIN AC_MAST ON POMAIN.AC_ID=AC_MAST.AC_ID 
INNER JOIN CURR_MAST ON CURR_MAST.CURRENCYID=POMAIN.FCID 
 WHERE POMAIN.ENTRY_TY='''+@ENT+''' and POMAIN.TRAN_CD='+cast(@TRN as nvarchar(1000))+' '
 
 execute sp_executesql @sqlcommand


END