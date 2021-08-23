If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Rep_RN_VOU')
Begin
	Drop Procedure Usp_Rep_RN_VOU
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- CREATE : Divyang Panchal
-- DATE: 13/03/2020
-- DESCRIPTION:	THIS STORED PROCEDURE IS USEFUL TO GENERATE Material Requisition Note from Material Requisition Note transaction.
-- REMARK:
-- =============================================

Create PROCEDURE  [dbo].[Usp_Rep_RN_VOU] 
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
Select case when att_file=1 then 'EQMAIN.'+RTRIM(FLD_NM) else 'EQITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='RN'
union all
Select case when att_file=1 then 'EQMAIN.'+RTRIM(FLD_NM) else 'EQITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='RN'
union all
Select case when att_file=1 then 'EQMAIN.'+RTRIM(pert_name) else 'EQITEM.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='RN'



set @QueryString ='SELECT EQMAIN.INV_SR,EQMAIN.ENTRY_TY,EQMAIN.TRAN_CD,EQMAIN.INV_NO,EQMAIN.DATE,EQITEM.QTY,
EQITEM.RATE,EQITEM.GRO_AMT,EQITEM.ITEM_NO,IT_DESC=(CASE WHEN ISNULL(IT_MAST.IT_ALIAS,'''')='''' 
THEN IT_MAST.IT_NAME ELSE IT_MAST.IT_ALIAS END),IT_MAST.RATEUNIT '


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
SET @SQLCOMMAND = N''+@QueryString++''+N''+@Tot_flds+''+'  FROM EQMAIN ' 
print @SQLCOMMAND
set @sqlcommand= @SQLCOMMAND +' 
INNER JOIN EQITEM ON (EQMAIN.TRAN_CD=EQITEM.TRAN_CD) 
INNER JOIN IT_MAST ON (EQITEM.IT_CODE=IT_MAST.IT_CODE) 
 WHERE EQMAIN.ENTRY_TY='''+@ENT+''' and EQMAIN.TRAN_CD='+cast(@TRN as nvarchar(1000))+' '
 
 execute sp_executesql @sqlcommand


END