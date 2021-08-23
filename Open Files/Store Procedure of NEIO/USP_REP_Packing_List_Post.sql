If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_Packing_List_Post')
Begin
	Drop Procedure USP_REP_Packing_List_Post
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ajay Jaiswal
-- Create date: 12/01/2010
-- Description:	This Stored procedure is useful to Generate data for Packing List Transation.
-- Modified By:  Archana Khade
-- Modified Date:14/02/2014 for Bug-21456
-- =============================================

Create PROCEDURE  [dbo].[USP_REP_Packing_List_Post]
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
SELECT 'REPORT HEADER' AS REP_HEAD,STMAIN.INV_SR,STMAIN.TRAN_CD,STMAIN.PARTY_NM,STMAIN.ENTRY_TY,STMAIN.INV_NO,STMAIN.DATE
,STMAIN.U_TIMEP,STMAIN.U_TIMEP1 ,STMAIN.U_REMOVDT,U_EXPLA='',U_EXRG23II='',U_RG2AMT=''
,EXAMT=0,U_BASDUTY=0,U_CESSPER=0,U_CESSAMT=0,U_HCESSPER=0,U_HCESAMT=0
,STMAIN.U_DELIVER,STMAIN.DUE_DT,STMAIN.U_CLDT,U_CHALNO=SPACE(1),U_CHALDT=STMAIN.DATE,STMAIN.U_PONO,STMAIN.U_PODT,STMAIN.U_LRNO,STMAIN.U_LRDT,STMAIN.U_DELI,STMAIN.U_VEHNO,STMAIN.GRO_AMT GRO_AMT1,STMAIN.TAX_NAME,STMAIN.TAXAMT,STMAIN.NET_AMT,U_PLASR='',U_RG23NO='',U_RG23CNO=''
,STITEM.U_PKNO,STITEM.QTY,STITEM.RATE,STITEM.U_ASSEAMT,STITEM.U_MRPRATE,STITEM.U_EXPDESC,STITEM.U_EXPMARK,STITEM.U_EXPGWT,STITEM.U_EXPNWT
,STMAIN.u_fdesti
,MailName=(CASE WHEN ISNULL(ac_mast.MailName,'')='' THEN ac_mast.ac_name ELSE ac_mast.mailname END)	
,cast(STITEM.u_pkno as int) as U_PKNO1
,STMAIN.U_BLNO,STMAIN.U_BLDT,STMAIN.U_VESSEL,STMAIN.U_TMODE,STITEM.U_TWEIGHT
,STMAIN.U_countain,STMAIN.U_COUNTAI2,STMAIN.U_OTHREF
,STMAIN.U_TSEAL,STMAIN.U_CSEAL
,STMAIN.U_PRECARRI
,STMAIN.U_RECEIPT
,STMAIN.U_LOADING
,STMAIN.U_PORT
,'India' as U_ORIGIN
,STMAIN.U_EXPDEL
,IT_MAST.IT_NAME
,CAST(IT_MAST.IT_DESC AS VARCHAR(4000)) AS IT_DESC 
,IT_MAST.EIT_NAME
,IT_MAST.CHAPNO
,IT_MAST.IDMARK,IT_MAST.RATEUNIT 
,AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,S_TAX='',AC_MAST.I_TAX
,ECCNO='' ,AC_MAST1.ADD1 ADD11,AC_MAST1.ADD2 ADD22,AC_MAST1.ADD3 ADD33,AC_MAST1.CITY CITY1
,AC_MAST1.ZIP ZIP1,S_TAX1='',AC_MAST1.I_TAX I_TAX1,ECCNO1='',STITEM.ITSERIAL  
,STMAIN.U_CONTNO,STMAIN.U_PKGNO --Added by Archana K. on 14/02/14 for Bug-21456
INTO #STMAIN
FROM STMAIN  INNER JOIN STITEM ON (STMAIN.TRAN_CD=STITEM.TRAN_CD) 
INNER JOIN IT_MAST ON (STITEM.IT_CODE=IT_MAST.IT_CODE) 
INNER JOIN AC_MAST ON (AC_MAST.AC_ID=STMAIN.AC_ID) 
LEFT JOIN AC_MAST AC_MAST1 ON (AC_MAST1.AC_NAME=STMAIN.U_DELIVER) 
WHERE  STMAIN.ENTRY_TY= @ENT  AND STMAIN.TRAN_CD=@TRN
ORDER BY STMAIN.INV_SR,CAST(STMAIN.INV_NO  AS INT)
SET @MCHAPNO=' '
SET @MEIT_NAME=' '
	
DECLARE CUR_STBILL CURSOR FOR SELECT DISTINCT CHAPNO FROM #STMAIN
OPEN CUR_STBILL 
FETCH NEXT FROM CUR_STBILL INTO @CHAPNO
WHILE(@@FETCH_STATUS=0)
BEGIN
	SET @MCHAPNO=RTRIM(@MCHAPNO)+','+RTRIM(@CHAPNO)
	FETCH NEXT FROM CUR_STBILL INTO @CHAPNO
END
CLOSE CUR_STBILL
DEALLOCATE CUR_STBILL

DECLARE CUR_STBILL CURSOR FOR SELECT DISTINCT EIT_NAME FROM #STMAIN
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

--Added By Divyang P for Bug-33349 on 08042020 Start
Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50)
Select case when att_file=1 then (CASE WHEN RTRIM(TBL_NM)='STMAINADD' THEN 'MA.' ELSE 'M.' END )+RTRIM(FLD_NM) else 'I.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='EI' 
union all
Select case when att_file=1 then 'M.'+RTRIM(FLD_NM) else 'I.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='EI' 
union all
Select case when att_file=1 then 'M.'+RTRIM(pert_name) else 'I.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='EI' and pert_name<>''


set @QueryString = 'SELECT * 
,MCHAPNO=ISNULL(''+@MCHAPNO+'','''')
,MEIT_NAME=ISNULL(''+@MEIT_NAME+'','''') '


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
SET @SQLCOMMAND = N''+@QueryString++''+N''+@Tot_flds+''+'  FROM #STMAIN MM' 
print @SQLCOMMAND
set @sqlcommand= @SQLCOMMAND +' 
inner join STMAIN M on (M.Tran_cd=MM.Tran_cd and M.Entry_ty=MM.Entry_ty)
inner join STMAINADD MA on (MA.Tran_cd=M.Tran_cd)
inner join STITEM I on (M.Tran_cd=I.Tran_cd and M.Entry_ty=I.Entry_ty)
 WHERE MM.ENTRY_TY='''+@ENT+''' and MM.TRAN_CD='+cast(@TRN as nvarchar(1000))+'    '
 print @SQLCOMMAND
 execute sp_executesql @sqlcommand
--Added By Divyang P for Bug-33349 on 08042020 End



--Commenetd By Divyang P for Bug-33349 on 08042020 Start
--SELECT * 
--,MCHAPNO=ISNULL(@MCHAPNO,'')
--,MEIT_NAME=ISNULL(@MEIT_NAME,'')
--FROM #STMAIN
--Commenetd By Divyang P for Bug-33349 on 08042020 End