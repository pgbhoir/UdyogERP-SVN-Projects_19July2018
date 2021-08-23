IF EXISTS(SELECT * FROM SYSOBJECTS WHERE XTYPE='P' AND [NAME]='USP_ENT_LabourJob_III_Pending')
BEGIN
	DROP PROCEDURE [USP_ENT_LabourJob_III_Pending]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Priyanka B.
-- Create date: 30/03/2020
-- =============================================
Create PROCEDURE [USP_ENT_LabourJob_III_Pending]
@DbName varchar(10),@EDATE SMALLDATETIME
AS
SELECT * INTO ##LabourJov_III
FROM (SELECT IIITEM.IT_CODE,IIITEM.ITSERIAL,IIITEM.ENTRY_TY,IIITEM.TRAN_CD LITRAN_CD,IIITEM.QTY,IIITEM.AC_ID
	,  QTY_USED=CASE WHEN IRRMDET.DATE<=CAST(@EDATE AS VARCHAR) THEN isnull(IRRMDET.QTY_USED,0) ELSE 0 END
	,  It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'')='' THEN it_mast.it_name ELSE it_mast.it_alias END)
	,  IRRMDET.TRAN_CD LRTRAN_CD,IRITEM.IT_CODE LR_ITCODE,LR_QTY=CASE WHEN IRRMDET.DATE<=CAST(@EDATE AS VARCHAR)
	THEN ISNULL(IRITEM.QTY,0) ELSE 0 END,  BALQTY=IIITEM.QTY-(CASE WHEN IRRMDET.DATE<=CAST(@EDATE AS VARCHAR)
	THEN isnull(IRRMDET.QTY_USED,0) ELSE 0 END+CASE WHEN IRRMDET.DATE<=CAST(@EDATE AS VARCHAR)
	THEN isnull(IRRMDET.WASTAGE,0) ELSE 0 END),  FITEM=(CASE WHEN ISNULL(LR_ITMAST.it_alias,'')='' 
	THEN LR_ITMAST.it_name ELSE LR_ITMAST.it_alias END),  FQTY=CASE WHEN IRRMDET.DATE<=CAST(@EDATE AS VARCHAR)
	THEN ISNULL(IRITEM.QTY,0) ELSE 0 END  
	,LRITSERIAL=IRRMDET.ITSERIAL
	FROM IIITEM  
	INNER JOIN IIMAIN ON  (IIITEM.TRAN_CD=IIMAIN.TRAN_CD)  
	INNER JOIN IT_MAST ON (IIITEM.IT_CODE=IT_MAST.IT_CODE)  
	LEFT JOIN IRRMDET ON (IIITEM.TRAN_CD=IRRMDET.LI_TRAN_CD AND IIITEM.ENTRY_TY=IRRMDET.LIENTRY_TY 
		AND IIITEM.ITSERIAL=IRRMDET.LI_ITSER AND IRRMDET.DATE<=CAST(@EDATE AS VARCHAR))  
	LEFT JOIN IRMAIN ON (IRRMDET.TRAN_CD=IRMAIN.TRAN_CD)  
	LEFT JOIN IRITEM ON (IRRMDET.TRAN_CD=IRITEM.TRAN_CD AND IRITEM.ENTRY_TY=IRRMDET.ENTRY_TY 
		AND IRRMDET.ITSERIAL=IRITEM.ITSERIAL)  
	LEFT JOIN IT_MAST LR_ITMAST ON (LR_ITMAST.IT_CODE=IRITEM.IT_CODE)  
	WHERE (IIMAIN.DATE< =CAST(@EDATE AS VARCHAR)) AND IIITEM.ENTRY_TY='I5' AND IIMAIN.[RULE]='ANNEXURE III') A 
Where 1=2

declare @sqlcommand Nvarchar(max)
SET @SQLCOMMAND='Insert into ##LabourJov_III'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+ ' SELECT * FROM (SELECT IIITEM.IT_CODE,IIITEM.ITSERIAL,IIITEM.ENTRY_TY,IIITEM.TRAN_CD LITRAN_CD,IIITEM.QTY,IIITEM.AC_ID,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' QTY_USED=CASE WHEN IRRMDET.DATE<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+' THEN isnull(IRRMDET.QTY_USED,0) ELSE 0 END,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END),'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' IRRMDET.TRAN_CD LRTRAN_CD,IRITEM.IT_CODE LR_ITCODE,LR_QTY=CASE WHEN IRRMDET.DATE<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+' THEN ISNULL(IRITEM.QTY,0) ELSE 0 END,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' BALQTY=IIITEM.QTY-(CASE WHEN IRRMDET.DATE<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+' THEN isnull(IRRMDET.QTY_USED,0) ELSE 0 END+CASE WHEN IRRMDET.DATE<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+' THEN isnull(IRRMDET.WASTAGE,0) ELSE 0 END),'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FITEM=(CASE WHEN ISNULL(LR_ITMAST.it_alias,'''')='''' THEN LR_ITMAST.it_name ELSE LR_ITMAST.it_alias END),'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FQTY=CASE WHEN IRRMDET.DATE<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+' THEN ISNULL(IRITEM.QTY,0) ELSE 0 END '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' ,IRRMDET.ITSERIAL AS LRITSERIAL'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM IIITEM '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN IIMAIN ON  (IIITEM.TRAN_CD=IIMAIN.TRAN_CD) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN IT_MAST ON (IIITEM.IT_CODE=IT_MAST.IT_CODE)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN IRRMDET ON (IIITEM.TRAN_CD=IRRMDET.LI_TRAN_CD AND IIITEM.ENTRY_TY=IRRMDET.LIENTRY_TY AND IIITEM.ITSERIAL=IRRMDET.LI_ITSER AND IRRMDET.DATE<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+') '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN IRMAIN ON (IRRMDET.TRAN_CD=IRMAIN.TRAN_CD) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN IRITEM ON (IRRMDET.TRAN_CD=IRITEM.TRAN_CD AND IRITEM.ENTRY_TY=IRRMDET.ENTRY_TY AND IRRMDET.ITSERIAL=IRITEM.ITSERIAL)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' LEFT JOIN IT_MAST LR_ITMAST ON (LR_ITMAST.IT_CODE=IRITEM.IT_CODE)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' WHERE (IIMAIN.DATE< ='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+') AND IIITEM.ENTRY_TY=''I5'' 
AND IIMAIN.[PRODTYPE]=''ANNEXURE III'')
A Where ISNULL(BALQTY,0)>0'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' ORDER BY ITSERIAL ' 
print @SQLCOMMAND
EXEC SP_EXECUTESQL @SQLCOMMAND

--SELECT * FROM ##LabourJov_III

select LRTRAN_CD,LITRAN_CD,ITSERIAL,IT_CODE,it_desc,FITEM,LR_ITCODE,LRITSERIAL INTO #TempRecord from ##LabourJov_III

--SELECT * FROM #TempRecord
--return

DECLARE @TblName Varchar(20),@fldlist Varchar(4000)
--**************************************** II TABLES TRANSFER **********************************************
	
	Set @sqlcommand ='DELETE FROM '+RTRIM(LTRIM(@DbName))+'..IIITEM WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) IN (SELECT ''I5''+Convert(Varchar(10),LITRAN_CD)+itserial+Convert(Varchar(10),it_code) FROM #TempRecord) AND ENTRY_TY=''I5'''
	Execute sp_Executesql @sqlcommand
	
	Set @sqlcommand ='DELETE FROM '+RTRIM(LTRIM(@DbName))+'..IIMAIN WHERE TRAN_CD IN (SELECT LITRAN_CD FROM #TempRecord) AND ENTRY_TY=''I5'''	
	Execute sp_Executesql @sqlcommand
	
	set @TblName='IIMAIN'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
		
	Set @sqlcommand = 'Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IIMAIN ON INSERT INTO '+RTRIM(LTRIM(@DbName))+'..IIMAIN ('+@fldlist+') 
	SELECT '+@fldlist+' FROM IIMAIN WHERE TRAN_CD IN (SELECT LITRAN_CD FROM #TempRecord) AND TRAN_CD NOT IN (SELECT TRAN_CD FROM '+RTRIM(LTRIM(@DbName))+'..IIMAIN)
	AND ENTRY_TY=''I5''
    Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IIMAIN OFF'
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand	
	
	set @TblName='IIITEM'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
	
	Set @sqlcommand = 'Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IIITEM ON INSERT INTO '+RTRIM(LTRIM(@DbName))+'..IIITEM ('+@fldlist+') 
	SELECT '+@fldlist+' FROM IIITEM WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) IN (SELECT ''I5''+Convert(Varchar(10),LITRAN_CD)+itserial+Convert(Varchar(10),it_code) FROM #TempRecord) AND entry_ty+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) 
	NOT IN (SELECT entry_ty+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) FROM '+RTRIM(LTRIM(@DbName))+'..IIITEM)
	AND ENTRY_TY=''I5''
	Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IIITEM OFF'
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand
	
	Set @sqlcommand = 'UPDATE '+RTRIM(LTRIM(@DbName))+'..IIITEM  SET DC_NO=''YE'' 
	WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) 
	IN (SELECT ''I5''+Convert(Varchar(10),LITRAN_CD)+itserial+Convert(Varchar(10),it_code) FROM #TempRecord) AND ENTRY_TY=''I5'''
	Execute sp_Executesql @sqlcommand
	
--*****************************************************************************************************************

--**************************************** IRRMDET TABLE TRANSFER **********************************************
	Set @sqlcommand = 'DELETE FROM '+RTRIM(LTRIM(@DbName))+'..IRRMDET WHERE Convert(Varchar(10),LI_TRAN_CD)+li_item 
	IN (SELECT Convert(Varchar(10),LITran_cd)+it_desc FROM #TempRecord) and lientry_ty =''I5'''
	print @sqlcommand
	Execute sp_Executesql @sqlcommand
	
	set @TblName='IRRMDET'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
	
	Set @sqlcommand = 'INSERT INTO '+RTRIM(LTRIM(@DbName))+'..IRRMDET ('+@fldlist+') 
	SELECT '+@fldlist+' FROM IRRMDET WHERE Convert(Varchar(10),LI_TRAN_CD) IN (SELECT Convert(Varchar(10),LITran_cd) 
	FROM #TempRecord) AND Convert(Varchar(10),LI_TRAN_CD) NOT IN (SELECT Convert(Varchar(10),LI_TRAN_CD)
	FROM '+RTRIM(LTRIM(@DbName))+'..IRRMDET) and lientry_ty =''I5''' 
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand

--*****************************************************************************************************************

--**************************************** IR TABLES TRANSFER **********************************************
	Set @sqlcommand ='DELETE FROM '+RTRIM(LTRIM(@DbName))+'..IRITEM WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd)+itserial+Convert(Varchar(10),it_code) IN (SELECT ''LR''+Convert(Varchar(10),LITRAN_CD)+LRitserial+Convert(Varchar(10),LR_itcode) FROM #TempRecord) AND ENTRY_TY=''R1'''
	print @sqlcommand
	Execute sp_Executesql @sqlcommand
	
	Set @sqlcommand ='DELETE FROM '+RTRIM(LTRIM(@DbName))+'..IRMAIN WHERE TRAN_CD IN (SELECT LITRAN_CD FROM #TempRecord) AND ENTRY_TY=''R1'''	
	print @sqlcommand
	Execute sp_Executesql @sqlcommand
	
	SELECT  * FROM IRMAIN WHERE TRAN_CD IN (SELECT LRTRAN_CD FROM #TempRecord) AND TRAN_CD NOT IN (SELECT TRAN_CD FROM U072021..IRMAIN)
	AND ENTRY_TY='R1'

	set @TblName='IRMAIN'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
		
	Set @sqlcommand = 'Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IRMAIN ON INSERT INTO '+RTRIM(LTRIM(@DbName))+'..IRMAIN ('+@fldlist+') 
	SELECT '+@fldlist+' FROM IRMAIN WHERE TRAN_CD IN (SELECT LRTRAN_CD FROM #TempRecord) AND TRAN_CD NOT IN (SELECT TRAN_CD FROM '+RTRIM(LTRIM(@DbName))+'..IRMAIN)
	AND ENTRY_TY=''R1''
    Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IRMAIN OFF'
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand	
	
	set @TblName='IRITEM'
	Execute USP_ENT_GETFIELD_LIST @TblName, @fldlist Output 
	
	Set @sqlcommand = 'Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IRITEM ON INSERT INTO '+RTRIM(LTRIM(@DbName))+'..IRITEM ('+@fldlist+') 
	SELECT '+@fldlist+' FROM IRITEM WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd)
	IN (SELECT ''R1''+Convert(Varchar(10),LRTRAN_CD) FROM #TempRecord) AND entry_ty+Convert(Varchar(10),Tran_cd)
	NOT IN (SELECT entry_ty+Convert(Varchar(10),Tran_cd) FROM '+RTRIM(LTRIM(@DbName))+'..IRITEM)
	AND ENTRY_TY=''R1''
	Set Identity_Insert '+RTRIM(LTRIM(@DbName))+'..IRITEM OFF'
	PRINT @sqlcommand
	Execute sp_Executesql @sqlcommand
	
	Set @sqlcommand = 'UPDATE '+RTRIM(LTRIM(@DbName))+'..IRITEM  SET DC_NO=''YE'' WHERE ENTRY_TY+Convert(Varchar(10),Tran_cd)
	IN (SELECT ''R1''+Convert(Varchar(10),LRTRAN_CD) FROM #TempRecord) 	AND ENTRY_TY=''R1'''
	Execute sp_Executesql @sqlcommand
	
--*****************************************************************************************************************

DROP TABLE ##LabourJov_III
DROP TABLE #TempRecord
GO
