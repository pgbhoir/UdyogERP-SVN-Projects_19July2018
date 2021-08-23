If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_INSURANCE_POST')
Begin
	Drop Procedure USP_REP_INSURANCE_POST
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Priyanka Himane
-- Create date: 17/11/2011
-- Description:	This Stored procedure is useful to Generate data for Post-Shipment Insurance Declaration.
-- =============================================

Create PROCEDURE    [dbo].[USP_REP_INSURANCE_POST]
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

		SELECT STMAIN.INV_SR,STMAIN.TRAN_CD,STMAIN.ENTRY_TY,STMAIN.INV_NO,STMAIN.DATE,STMAIN.PARTY_NM,STMAIN.U_TIMEP,
		STMAIN.U_TIMEP1,STMAIN.U_REMOVDT,STMAIN.U_DELIVER,CONVERT(VARCHAR(2000),STMAIN.NARR) AS NARR,STMAIN.DUE_DT,
		STMAIN.U_PONO,STMAIN.U_PODT,STMAIN.U_LRNO,STMAIN.U_LRDT,STMAIN.U_DELI,STMAIN.U_VEHNO,STMAIN.GRO_AMT,STMAIN.u_exchange,
		STMAIN.U_CORREN,STMAIN.U_OTHREF,STMAIN.U_PRECARRI,STMAIN.U_RECEIPT,STMAIN.U_LOADING, STMAIN.u_TMODE,
		STMAIN.U_PORT,STMAIN.TAX_NAME,STMAIN.fcexrate,STMAIN.TAXAMT,STMAIN.NET_AMT,stmain.u_blno,stmain.u_bldt,
		STMAIN.U_QADINV,STMAIN.U_CONTNO,STMAIN.U_PKGNO,STMAIN.U_CSEAL,STMAIN.U_TSEAL,STMAIN.U_TSEAL2,STMAIN.u_vessel,
		AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.MAILNAME,
		AC_MAST.CITY,AC_MAST.ZIP,AC_MAST.COUNTRY,AC_MAST.PHONE,S_TAX='',AC_MAST.I_TAX,ECCNO='',AC_MAST.FAX,AC_MAST1.ADD1 ADD11,
		AC_MAST1.ADD2 ADD22,AC_MAST1.ADD3 ADD33,AC_MAST1.CITY CITY1,AC_MAST1.ZIP ZIP1,AC_MAST1.COUNTRY COUNTRY1,AC_MAST1.CONTACT contact1,
		AC_MAST1.PHONE PHONE1,S_TAX1='',AC_MAST1.I_TAX I_TAX1,
		ECCNO1='',AC_MAST1.MAILNAME MAILNAME11,CURR_MAST.CURRENCYCD,STMAIN.U_GWT,COUNTRY.COUNTRY as U_fdesti,
		STMAIN.U_COUNTAIN INTO #TBL1
		FROM STMAIN
		LEFT JOIN AC_MAST ON (AC_MAST.AC_ID=STMAIN.AC_ID) 
		LEFT JOIN AC_MAST AC_MAST1 ON (AC_MAST1.AC_ID=STMAIN.CONS_ID)
		LEFT JOIN CURR_MAST ON (STMAIN.FCID=CURR_MAST.CURRENCYID)
		LEFT JOIN COUNTRY ON STMAIN.U_FDESTI=COUNTRY.CODE
--		LEFT JOIN AC_MAST AC_MAST2 ON RTRIM(LTRIM(AC_MAST2.AC_NAME))=RTRIM(LTRIM(STMAIN.U_NOTIFY)) 
--		LEFT JOIN AC_MAST AC_MAST3 ON RTRIM(LTRIM(AC_MAST3.AC_NAME))=RTRIM(LTRIM(STMAIN.u_repre)) 
		WHERE STMAIN.ENTRY_TY= @ENT  AND STMAIN.TRAN_CD=@TRN
		ORDER BY STMAIN.INV_SR,STMAIN.INV_NO
--SELECT * FROM #TBL1

		SELECT STITEM.ENTRY_TY,STITEM.TRAN_CD,STITEM.ITEM,STITEM.RATE,STITEM.FCRATE,IT_MAST.IT_NAME,IT_MAST.EIT_NAME,
		IT_MAST.IT_ALIAS,(CAST(IT_MAST.IT_DESC AS VARCHAR(200))) AS [DESC],
		QTY = (SUM(STITEM.QTY)), GROAMT = CAST('0' AS NUMERIC(15,2)), STITEM.U_EXPNWT,IT_MAST.U_CNAME,IT_MAST.CHAPNO,
		STITEM.U_CORRU
		INTO #TBL2
		FROM STITEM
		INNER JOIN IT_MAST ON (STITEM.IT_CODE=IT_MAST.IT_CODE)
		WHERE STITEM.ENTRY_TY= @ENT  AND STITEM.TRAN_CD=@TRN
		GROUP BY STITEM.ENTRY_TY,STITEM.TRAN_CD,STITEM.ITEM,STITEM.RATE,STITEM.FCRATE,IT_MAST.IT_NAME,IT_MAST.EIT_NAME,
		IT_MAST.IT_ALIAS,(CAST(IT_MAST.IT_DESC AS VARCHAR(200))),STITEM.U_EXPNWT,IT_MAST.U_CNAME,IT_MAST.CHAPNO,STITEM.U_CORRU
		
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


set @QueryString = 'SELECT A.*,B.* '


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
	--SELECT A.*,B.*
	--FROM #TBL1 A INNER JOIN #TBL2 B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD)
	--Commenetd By Divyang P for Bug-33349 on 08042020 End

--EXECUTE USP_REP_INSURANCE_POST 'STMAIN.ENTRY_TY=''EI'' AND STMAIN.TRAN_CD=124'


