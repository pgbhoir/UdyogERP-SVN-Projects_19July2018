If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_USCUSTINV')
Begin
	Drop Procedure USP_REP_USCUSTINV
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Priyanka Himane
-- Create date: 16/12/2011
-- Description:	This Stored procedure is useful to Generate data for US CUSTOMS INVOICE Report.
-- Modified By: 
-- Modified date: 
-- =============================================

Create PROCEDURE    [dbo].[USP_REP_USCUSTINV]
@ENTRYCOND NVARCHAR(254)
	AS
DECLARE @SQLCOMMAND AS NVARCHAR(4000),@TBLCON AS NVARCHAR(4000)

	
	--->ENTRY_TY AND TRAN_CD SEPARATION
		DECLARE @ENT VARCHAR(2),@TRN INT,@POS1 INT,@POS2 INT,@POS3 INT
		
		PRINT @ENTRYCOND
		SET @POS1=CHARINDEX('''',@ENTRYCOND,1)+1
		SET @ENT= SUBSTRING(@ENTRYCOND,@POS1,2)
		SET @POS2=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS1))+1
		SET @POS3=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS2))+1
		SET @TRN= SUBSTRING(@ENTRYCOND,@POS2,@POS2-@POS3)
		SET @TBLCON=RTRIM(@ENTRYCOND)
PRINT @ENT
PRINT @TRN

		SELECT STMAIN.TRAN_CD,STMAIN.ENTRY_TY,STMAIN.INV_NO,STMAIN.DATE,STMAIN.U_PONO,STMAIN.U_PODT,STMAIN.fcexrate,
		STITEM.FCINS,STITEM.FCFRT,
		STMAIN.U_CONTNO,STMAIN.U_PKGNO,STMAIN.u_vessel,STMAIN.U_CUSMATNM,STMAIN.U_CUSMATNO,STMAIN.U_SCHEDULE,
		AC_MAST.ADD1 ADD1,AC_MAST.ADD2 ADD2,AC_MAST.ADD3 ADD3,AC_MAST.MAILNAME,AC_MAST.COUNTRY,AC_MAST1.ADD1 ADD11,
		AC_MAST1.ADD2 ADD22,AC_MAST1.ADD3 ADD33,AC_MAST1.COUNTRY COUNTRY1,AC_MAST1.MAILNAME MAILNAME11,CURR_MAST.CURRENCYCD,
		AC_MAST2.MAILNAME MAILNAME91,AC_MAST2.ADD1 ADD91, AC_MAST2.ADD2 ADD92 ,AC_MAST2.ADD3 ADD93 ,
		AC_MAST2.CITY CITY9 ,AC_MAST2.COUNTRY COUNTRY9 ,AC_MAST2.PHONE PHONE91,
		STMAIN.U_GWT,STMAIN.U_COUNTAIN,STMAIN.U_PAYMENT1,STMAIN.U_TERMS
		INTO #TBL1
		FROM STMAIN
		LEFT JOIN STITEM ON (STMAIN.ENTRY_TY=STITEM.ENTRY_TY)
		LEFT JOIN AC_MAST ON (AC_MAST.AC_ID=STMAIN.CONS_ID)
		LEFT JOIN AC_MAST AC_MAST1 ON (AC_MAST1.AC_ID=STMAIN.CONS_ID)
		LEFT JOIN CURR_MAST ON (STMAIN.FCID=CURR_MAST.CURRENCYID)
		LEFT JOIN COUNTRY ON STMAIN.U_FDESTI=COUNTRY.CODE
		LEFT JOIN AC_MAST AC_MAST2 ON RTRIM(LTRIM(AC_MAST2.AC_NAME))=RTRIM(LTRIM(STMAIN.U_NOTIFY)) 
		WHERE STMAIN.ENTRY_TY= @ENT  AND STMAIN.TRAN_CD=@TRN
		ORDER BY STMAIN.INV_SR,STMAIN.INV_NO


		SELECT STITEM.ENTRY_TY,STITEM.TRAN_CD,STITEM.ITEM,STITEM.RATE,STITEM.FCRATE,IT_MAST.IT_NAME,IT_MAST.EIT_NAME,
		IT_MAST.IT_ALIAS,(CAST(IT_MAST.IT_DESC AS VARCHAR(200))) AS [DESC],
		QTY = (SUM(STITEM.QTY)), GROAMT = CAST('0' AS NUMERIC(15,2)), STITEM.U_EXPNWT,
		IT_MAST.U_CNAME,STITEM.U_CORRU
		INTO #TBL2
		FROM STITEM
		INNER JOIN IT_MAST ON (STITEM.IT_CODE=IT_MAST.IT_CODE)
		WHERE STITEM.ENTRY_TY= @ENT  AND STITEM.TRAN_CD=@TRN
		GROUP BY STITEM.ENTRY_TY,STITEM.TRAN_CD,STITEM.ITEM,STITEM.RATE,STITEM.FCRATE,IT_MAST.IT_NAME,IT_MAST.EIT_NAME,
		IT_MAST.IT_ALIAS,(CAST(IT_MAST.IT_DESC AS VARCHAR(200))),
		STITEM.U_EXPNWT,IT_MAST.U_CNAME,STITEM.U_CORRU
		
		UPDATE #TBL2 SET GROAMT = QTY*FCRATE

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


set @QueryString = 'SELECT A.*,B.IT_NAME,B.[DESC],B.RATE,B.FCRATE,B.QTY,B.U_EXPNWT,B.U_CNAME,B.U_CORRU '


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
SET @SQLCOMMAND = N''+@QueryString++''+N''+@Tot_flds+''+'  FROM #TBL1 A ' 
print @SQLCOMMAND
set @sqlcommand= @SQLCOMMAND +' 
INNER JOIN #TBL2 B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD)
inner join STMAIN M on (M.Tran_cd=A.Tran_cd and M.Entry_ty=A.Entry_ty)
inner join STMAINADD MA on (MA.Tran_cd=M.Tran_cd)
inner join STITEM I on (M.Tran_cd=I.Tran_cd and M.Entry_ty=I.Entry_ty)
 WHERE A.ENTRY_TY='''+@ENT+''' and A.TRAN_CD='+cast(@TRN as nvarchar(1000))+'    '
 print @SQLCOMMAND
 execute sp_executesql @sqlcommand
--Added By Divyang P for Bug-33349 on 08042020 End


	--Commenetd By Divyang P for Bug-33349 on 08042020 Start
	--SELECT A.*,B.IT_NAME,B.[DESC],B.RATE,B.FCRATE,B.QTY,B.U_EXPNWT,B.U_CNAME,B.U_CORRU
	--FROM #TBL1 A INNER JOIN #TBL2 B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD)
	--Commenetd By Divyang P for Bug-33349 on 08042020 End

--EXECUTE USP_REP_USCUSTINV 'STMAIN.ENTRY_TY=''EI'' AND STMAIN.TRAN_CD=0'
