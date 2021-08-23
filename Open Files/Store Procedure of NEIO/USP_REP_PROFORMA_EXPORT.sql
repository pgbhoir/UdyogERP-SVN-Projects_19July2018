If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_PROFORMA_EXPORT')
Begin
	Drop Procedure USP_REP_PROFORMA_EXPORT
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ajay Jaiswal
-- Create date: 12/01/2010
-- Description:	This Stored procedure is useful to Generate data for Proforma Invoice (Export) Report.
-- =============================================

Create PROCEDURE  [dbo].[USP_REP_PROFORMA_EXPORT]
@ENTRYCOND NVARCHAR(254)
AS
	DECLARE @SQLCOMMAND AS NVARCHAR(4000),@TBLCON AS NVARCHAR(4000)
	DECLARE @CHAPNO VARCHAR(30),@EIT_NAME  VARCHAR(100),@MCHAPNO VARCHAR(250),@MEIT_NAME  VARCHAR(250)
	
--->ENTRY_TY AND TRAN_CD SEPARATION
	DECLARE @ENT VARCHAR(2),@TRN INT,@POS1 INT,@POS2 INT,@POS3 INT
		
	PRINT @ENTRYCOND
	SET @POS1=CHARINDEX('''',@ENTRYCOND,1)+1
	SET @ENT= SUBSTRING(@ENTRYCOND,@POS1,2)
	SET @POS2=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS1))+1
	SET @POS3=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS2))+1
	SET @TRN= SUBSTRING(@ENTRYCOND,@POS2,@POS2-@POS3)
	SET @TBLCON=RTRIM(@ENTRYCOND)
	
-- 	
SELECT 'REPORT HEADER' AS REP_HEAD,PIMAIN.INV_SR,PIMAIN.TRAN_CD,PIMAIN.ENTRY_TY,PIMAIN.INV_NO,PIMAIN.DATE
,PIMAIN.U_TIMEP,PIMAIN.U_TIMEP1 ,PIMAIN.U_REMOVDT,U_EXPLA='',U_EXRG23II='',U_RG2AMT=''
,EXAMT=0,U_BASDUTY=0,U_CESSPER=0,U_CESSAMT=0,U_HCESSPER=0,U_HCESAMT=0
,PIMAIN.DUE_DT,PIMAIN.U_CLDT,U_CHALNO=SPACE(1),U_CHALDT=PIMAIN.DATE,PIMAIN.U_PONO,PIMAIN.U_PODT,PIMAIN.U_LRNO,PIMAIN.U_LRDT,PIMAIN.U_DELI,PIMAIN.U_VEHNO,PIMAIN.GRO_AMT GRO_AMT1,PIMAIN.TAX_NAME,PIMAIN.TAXAMT,PIMAIN.NET_AMT,U_PLASR='',U_RG23NO='',U_RG23CNO=''
,PIITEM.U_PKNO,PIITEM.QTY,PIITEM.RATE,PIITEM.U_ASSEAMT,PIITEM.U_MRPRATE,PIITEM.U_EXPDESC,PIITEM.U_EXPMARK,PIITEM.U_EXPGWT,PIITEM.U_EXPNWT
,PIMAIN.u_fdesti,PIITEM.FCRATE--,PIMAIN.FCGRO_AMT  -- commented by Prajakta B. on 24042018	
,PIITEM.FCGRO_AMT   -- modified by Prajakta B. on 24042018	
,CURR_MAST.CURRDESC,cast(PIITEM.u_pkno as int) as U_PKNO1
,PIMAIN.U_BLNO,PIMAIN.U_bldt,PIMAIN.U_QADINV,PIMAIN.U_othref,PIMAIN.U_payment
,PIMAIN.U_countain,PIMAIN.U_COUNTAI2,PIMAIN.U_TSEAL,PIMAIN.U_TSEAL2,PIMAIN.U_PRECARRI,PIMAIN.U_RECEIPT,PIMAIN.U_LOADING,PIMAIN.U_PORT
,'India' as U_ORIGIN,PIMAIN.U_EXPDEL,IT_MAST.IT_NAME,It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'')='' THEN it_mast.it_name ELSE it_mast.it_alias END)
,MailName=(CASE WHEN ISNULL(ac_mast.MailName,'')='' THEN ac_mast.ac_name ELSE ac_mast.mailname END)	,IT_MAST.EIT_NAME,IT_MAST.CHAPNO
,IT_MAST.IDMARK,IT_MAST.RATEUNIT ,AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,S_TAX='',AC_MAST.I_TAX
,ECCNO='',AC_MAST1.ADD1 ADD11,AC_MAST1.ADD2 ADD22,AC_MAST1.ADD3 ADD33,AC_MAST1.CITY CITY1
,AC_MAST1.ZIP ZIP1,S_TAX1='',AC_MAST1.I_TAX I_TAX1,ECCNO1='',PIITEM.ITSERIAL ,AC_MAST1.AC_NAME AS CONSIGNEE
INTO #PIMAIN
FROM PIMAIN  
INNER JOIN PIITEM ON (PIMAIN.TRAN_CD=PIITEM.TRAN_CD) AND (PIMAIN.ENTRY_TY=PIITEM.ENTRY_TY)
LEFT JOIN IT_MAST ON (PIITEM.IT_CODE=IT_MAST.IT_CODE) 
LEFT JOIN CURR_MAST ON (PIMAIN.FCID = CURR_MAST.CURRENCYID)
LEFT JOIN AC_MAST ON (AC_MAST.AC_ID=PIMAIN.AC_ID) 
LEFT JOIN AC_MAST AC_MAST1 ON (AC_MAST1.AC_ID=PIMAIN.CONS_ID) 
WHERE  PIMAIN.ENTRY_TY= @ENT  AND PIMAIN.TRAN_CD=@TRN
ORDER BY PIMAIN.INV_SR
--,CAST(PIMAIN.INV_NO  AS INT)  --Commented by Priyanka B on 02092017 for Export
,PIMAIN.INV_NO  --Modified by Priyanka B on 02092017 for Export
SET @MCHAPNO=' '
SET @MEIT_NAME=' '

DECLARE CUR_STBILL CURSOR FOR SELECT DISTINCT CHAPNO FROM #PIMAIN
OPEN CUR_STBILL 
FETCH NEXT FROM CUR_STBILL INTO @CHAPNO
WHILE(@@FETCH_STATUS=0)
BEGIN
	SET @MCHAPNO=RTRIM(@MCHAPNO)+','+RTRIM(@CHAPNO)
	FETCH NEXT FROM CUR_STBILL INTO @CHAPNO
END
CLOSE CUR_STBILL
DEALLOCATE CUR_STBILL

DECLARE CUR_STBILL CURSOR FOR SELECT DISTINCT EIT_NAME FROM #PIMAIN
OPEN CUR_STBILL 
FETCH NEXT FROM CUR_STBILL INTO @EIT_NAME
WHILE(@@FETCH_STATUS=0)
BEGIN
	SET @MEIT_NAME=RTRIM(@MEIT_NAME)+','+RTRIM(@EIT_NAME)
	FETCH NEXT FROM CUR_STBILL INTO @EIT_NAME
END
CLOSE CUR_STBILL
DEALLOCATE CUR_STBILL	

SET @MCHAPNO=CASE WHEN LEN(@MCHAPNO)>1 THEN SUBSTRING(@MCHAPNO,2,LEN(@MCHAPNO)-1) ELSE '' END
SET @MEIT_NAME=CASE WHEN LEN(@MEIT_NAME)>1 THEN SUBSTRING(@MEIT_NAME,2,LEN(@MEIT_NAME)-1) ELSE '' END


--Added By Divyang P for Bug-33349 on 13032020 Start
Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50)
Select case when att_file=1 then 'M.'+RTRIM(FLD_NM) else 'I.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='P2'
union all
Select case when att_file=1 then 'M.'+RTRIM(FLD_NM) else 'I.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='P2'
union all
Select case when att_file=1 then 'M.'+RTRIM(pert_name) else 'I.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='P2'


set @QueryString = 'SELECT * 
,MCHAPNO=ISNULL(''+@MCHAPNO+'','''')
,MEIT_NAME=ISNULL(''+@MEIT_NAME+'','''') '

print @QueryString
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
SET @SQLCOMMAND = N''+@QueryString++''+N''+@Tot_flds+''+'  FROM #PIMAIN MM' 
print @SQLCOMMAND
set @sqlcommand= @SQLCOMMAND +' 
inner join PIMAIN M on (M.Tran_cd=MM.Tran_cd and M.Entry_ty=MM.Entry_ty)
inner join PIITEM I on (M.Tran_cd=I.Tran_cd and M.Entry_ty=I.Entry_ty)
 WHERE MM.ENTRY_TY='''+@ENT+''' and MM.TRAN_CD='+cast(@TRN as nvarchar(1000))+'    '
 
 execute sp_executesql @sqlcommand
--Added By Divyang P for Bug-33349 on 13032020 End


--Commented  By Divyang P for Bug-33349 on 08042020 Start
--SELECT * 
--,MCHAPNO=ISNULL(@MCHAPNO,'')
--,MEIT_NAME=ISNULL(@MEIT_NAME,'')
--FROM #PIMAIN
--Commented  By Divyang P for Bug-33349 on 08042020 End