If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Rep_EQ_VOUE')
Begin
	Drop Procedure Usp_Rep_EQ_VOUE
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- CREATE 
-- DATE: 20/01/2011
-- DESCRIPTION:	THIS STORED PROCEDURE IS USEFUL TO GENERATE Sales Enquiry (Export) Report.
-- REMARK:
-- =============================================

Create PROCEDURE  [dbo].[Usp_Rep_EQ_VOUE] 
@ENTRYCOND NVARCHAR(254)
AS
BEGIN
Declare @SQLCOMMAND as NVARCHAR(4000)
	-->ENTRY_TY AND TRAN_CD SEPARATION
		DECLARE @ENT VARCHAR(2),@TRN INT,@POS1 INT,@POS2 INT,@POS3 INT--,@ENTRYCOND NVARCHAR(254)
		PRINT @ENTRYCOND
		SET @POS1=CHARINDEX('''',@ENTRYCOND,1)+1
		SET @ENT= SUBSTRING(@ENTRYCOND,@POS1,2)
		SET @POS2=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS1))+1
		SET @POS3=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS2))+1
		SET @TRN= SUBSTRING(@ENTRYCOND,@POS2,@POS2-@POS3)
		PRINT @TRN
	--<---ENTRY_TY AND TRAN_CD SEPARATION

--Added By Divyang P for Bug-33349 on 07042020 Start
Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50)
Select case when att_file=1 then 'M.'+RTRIM(FLD_NM) else 'I.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='Q1'
union all
Select case when att_file=1 then 'M.'+RTRIM(FLD_NM) else 'I.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='Q1'
union all
Select case when att_file=1 then 'M.'+RTRIM(pert_name) else 'I.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='Q1'
--Added By Divyang P for Bug-33349 on 07042020 End



--set @sqlcommand='SELECT AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,EQMAIN.INV_NO,EQMAIN.DATE  --commented By Divyang P for Bug-33349 on 07042020 
set @QueryString = 'SELECT AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,M.INV_NO,M.DATE  --Modified By Divyang P for Bug-33349 on 07042020 
,V_GRO_AMT=CASE WHEN CURR_MAST.CURRDESC=''INR'' THEN M.GRO_AMT ELSE M.FCGRO_AMT END 
,NET_AMT=CASE WHEN CURR_MAST.CURRDESC=''INR'' THEN M.NET_AMT ELSE M.FCNET_AMT END
,I.ITEM_NO,I.ITEM,I.QTY
,RATE=CASE WHEN CURR_MAST.CURRDESC=''INR'' THEN I.RATE ELSE I.FCRATE END
,GRO_AMT=CASE WHEN CURR_MAST.CURRDESC=''INR'' THEN I.GRO_AMT ELSE I.FCGRO_AMT END ,IT_MAST.RATEUNIT
,It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END)
,MailName=(CASE WHEN ISNULL(ac_mast.MailName,'''')='''' THEN ac_mast.ac_name ELSE ac_mast.mailname END) 
,CURR_MAST.CURRDESC'

--Added By Divyang P for Bug-33349 on 07042020 Start
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
SET @SQLCOMMAND = N''+@QueryString++''+N''+@Tot_flds+''+'  FROM EQMAIN M ' 
print @SQLCOMMAND
set @sqlcommand= @SQLCOMMAND +' 
INNER JOIN EQITEM I ON M.TRAN_CD=I.TRAN_CD 
LEFT JOIN IT_MAST ON IT_MAST.IT_CODE=I.IT_CODE 
INNER JOIN CURR_MAST ON (M.FCID = CURR_MAST.CURRENCYID)
INNER JOIN AC_MAST ON M.AC_ID=AC_MAST.AC_ID 
WHERE M.ENTRY_TY='''+@ENT+''' and M.TRAN_CD='+cast(@TRN as nvarchar(1000))+'  '
print @SQLCOMMAND
execute sp_executesql @sqlcommand

--Added By Divyang P for Bug-33349 on 07042020 End


--Commented By Divyang P for Bug-33349 on 07042020 Start
--FROM EQMAIN 
--INNER JOIN EQITEM ON EQMAIN.TRAN_CD=EQITEM.TRAN_CD LEFT JOIN IT_MAST ON IT_MAST.IT_CODE=EQITEM.IT_CODE 
--INNER JOIN CURR_MAST ON (EQMAIN.FCID = CURR_MAST.CURRENCYID)
--INNER JOIN AC_MAST ON EQMAIN.AC_ID=AC_MAST.AC_ID AND EQMAIN.ENTRY_TY=''Q1'' WHERE '+@ENTRYCOND
--execute sp_executesql @sqlcommand
--Commented By Divyang P for Bug-33349 on 07042020 End

--set @sqlcommand='SELECT AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,EQMAIN.INV_NO,EQMAIN.DATE,EQMAIN.GRO_AMT AS V_GRO_AMT,
--EQMAIN.TAX_NAME,EQMAIN.TAXAMT,EQMAIN.TOT_DEDUC,EQMAIN.TOT_ADD,EQMAIN.TOT_TAX,EQMAIN.TOT_NONTAX,
--EQMAIN.NET_AMT,EQITEM.ITEM_NO,EQITEM.ITEM,EQITEM.QTY,EQITEM.RATE,EQITEM.GRO_AMT,IT_MAST.RATEUNIT,
--IT_DESC=(CASE WHEN ISNULL(IT_MAST.IT_ALIAS,'''')='''' THEN IT_MAST.IT_NAME ELSE IT_MAST.IT_ALIAS END),
--MAILNAME=(CASE WHEN ISNULL(AC_MAST.MAILNAME,'''')='''' THEN AC_MAST.AC_NAME ELSE AC_MAST.MAILNAME END),
--EQITEM.FCRATE,EQitem.FCGRO_AMT as IFCGRO_AMT ,EQMAIN.FCGRO_AMT,CURR_MAST.CURRDESC,EQMAIN.FCTAXAMT,EQMAIN.FCnet_amt		
--FROM EQMAIN 
--INNER JOIN EQITEM ON EQMAIN.TRAN_CD=EQITEM.TRAN_CD 

--LEFT JOIN IT_MAST ON IT_MAST.IT_CODE=EQITEM.IT_CODE 
--INNER JOIN AC_MAST ON EQMAIN.AC_ID=AC_MAST.AC_ID AND EQMAIN.ENTRY_TY=''Q1'' WHERE '+@ENTRYCOND
--EQITEM.FCRATE,EQitem.FCGRO_AMT as IFCGRO_AMT ,EQMAIN.FCGRO_AMT,CURR_MAST.CURRDESC,EQMAIN.FCTAXAMT,EQMAIN.FCnet_amt		--Commented by Shrikant S. on 23/04/2018 for Installer 
--execute sp_executesql @sqlcommand


END