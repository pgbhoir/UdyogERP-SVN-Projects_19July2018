DROP PROCEDURE [USP_REP_BATCH_STATUS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ruepesh Prajapati
-- Create date: 16/05/2007
-- Description:	This Stored procedure is useful to generate Bath Status Report.
-- Modify date: 16/05/2007
-- Modified By: Birendra : Bug-5006 on 01/08/2012
-- Modified By: Birendra : Bug-5961 on 03/11/2012
-- Modified By: Birendra : Bug-7897 on 18/12/2012
-- Modify date: 
-- Remark:
-- =============================================
Create PROCEDURE  [USP_REP_BATCH_STATUS]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)= NULL
AS
DECLARE @FCON AS NVARCHAR(2000),@VSAMT DECIMAL(14,4),@VEAMT DECIMAL(14,4)

EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=@SDATE
,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='STKL_VW_MAIN',@VITFILE='STKL_VW_ITEM',@VACFILE=' '
,@VDTFLD ='DATE'
,@VLYN=null--@LYN
,@VEXPARA=NULL
,@VFCON =@FCON OUTPUT

SELECT A.BATCHNO,A.MFGDT,A.EXPDT,A.ENTRY_TY,A.DATE,A.TRAN_CD,C.IT_NAME,A.QTY,D.AC_NAME,C.[TYPE],A.PMKEY,B.INV_NO,A.ITSERIAL,cnt=999999999,C.RATEUNIT,A.IT_CODE
INTO #BATCH1
FROM ITEM A
INNER JOIN MAIN B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD)
INNER JOIN IT_MAST C ON (A.IT_CODE=C.IT_CODE)
INNER JOIN AC_MAST D ON (B.AC_ID=D.AC_ID)
WHERE 1=2
SELECT * INTO #BATCH FROM #BATCH1  

DECLARE @SQLCOMMAND NVARCHAR(4000),@VCOND NVARCHAR(1000)
--SET @SQLCOMMAND='INSERT INTO #BATCH1 SELECT STKL_VW_ITEM.BATCHNO,STKL_VW_ITEM.MFGDT,STKL_VW_ITEM.EXPDT,STKL_VW_ITEM.ENTRY_TY,STKL_VW_ITEM.DATE,STKL_VW_ITEM.TRAN_CD,IT_MAST.IT_NAME,STKL_VW_ITEM.QTY,AC_MAST.AC_NAME,IT_MAST.[TYPE],STKL_VW_ITEM.PMKEY,STKL_VW_MAIN.INV_NO,STKL_VW_ITEM.ITSERIAL,CNT=0,IT_MAST.RATEUNIT,IT_MAST.IT_CODE'
--Birendra : Bug-7897 on 24/12/2012
SET @SQLCOMMAND='INSERT INTO #BATCH1 SELECT Projectitref.BATCHNO,Projectitref.MFGDT,Projectitref.EXPDT,STKL_VW_ITEM.ENTRY_TY,STKL_VW_ITEM.DATE,STKL_VW_ITEM.TRAN_CD,IT_MAST.IT_NAME,STKL_VW_ITEM.QTY,AC_MAST.AC_NAME,IT_MAST.[TYPE],STKL_VW_ITEM.PMKEY,STKL_VW_MAIN.INV_NO,STKL_VW_ITEM.ITSERIAL,CNT=0,IT_MAST.RATEUNIT,IT_MAST.IT_CODE'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'FROM STKL_VW_ITEM '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN STKL_VW_MAIN ON (STKL_VW_ITEM.ENTRY_TY=STKL_VW_MAIN.ENTRY_TY AND STKL_VW_ITEM.TRAN_CD=STKL_VW_MAIN.TRAN_CD)'

SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN Projectitref ON (Projectitref.ENTRY_TY=STKL_VW_MAIN.ENTRY_TY AND projectitref.TRAN_CD=STKL_VW_MAIN.TRAN_CD)' --Birendra : Bug-7897

SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN IT_MAST  ON (STKL_VW_ITEM.IT_CODE=IT_MAST.IT_CODE)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN AC_MAST  ON (STKL_VW_MAIN.AC_ID=AC_MAST.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND ISNULL(STKL_VW_ITEM.BATCHNO,'+ char(39)+space(1)+char(39)+')<>'+char(39)+space(1)+char(39)
--Birendra : Bug-7897 on 24/12/2012
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND ISNULL(Projectitref.BATCHNO,'+ char(39)+space(1)+char(39)+')<>'+char(39)+space(1)+char(39)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND STKL_VW_ITEM.DC_NO ='+ char(39)+''+char(39) ----Birendra : Bug-7897 on 18/12/2012 :ADDED:
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND
SET @FCON=' '
EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=@SDATE
,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='MAIN',@VITFILE='ITEM',@VACFILE=' '
,@VDTFLD ='DATE'
,@VLYN=null--@LYN
,@VEXPARA=NULL
,@VFCON =@FCON OUTPUT

SET @SQLCOMMAND='INSERT INTO #BATCH1 SELECT ITEM.BATCHNO,ITEM.MFGDT,ITEM.EXPDT,ITEM.ENTRY_TY,ITEM.DATE,ITEM.TRAN_CD,IT_MAST.IT_NAME,ITEM.QTY,AC_MAST.AC_NAME,IT_MAST.[TYPE],ITEM.PMKEY,MAIN.INV_NO,ITEM.ITSERIAL,CNT=0,IT_MAST.RATEUNIT,IT_MAST.IT_CODE'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'FROM ITEM '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN MAIN ON (ITEM.ENTRY_TY=MAIN.ENTRY_TY AND ITEM.TRAN_CD=MAIN.TRAN_CD)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN IT_MAST  ON (ITEM.IT_CODE=IT_MAST.IT_CODE)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN AC_MAST  ON (MAIN.AC_ID=AC_MAST.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND ISNULL(ITEM.BATCHNO,'+ char(39)+space(1)+char(39)+')<>'+char(39)+space(1)+char(39)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND MAIN.ENTRY_TY='+ char(39)+'WK'+char(39)
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND

SET @FCON=' '
EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=@SDATE
,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='STMAIN',@VITFILE='STITEM',@VACFILE=' '
,@VDTFLD ='DATE'
,@VLYN=null--@LYN
,@VEXPARA=NULL
,@VFCON =@FCON OUTPUT

--SET @SQLCOMMAND='INSERT INTO #BATCH1 SELECT PROJECTITREF.BATCHNO,PROJECTITREF.MFGDT,PROJECTITREF.EXPDT,STITEM.ENTRY_TY,STITEM.DATE,STITEM.TRAN_CD,IT_MAST.IT_NAME,STITEM.QTY,AC_MAST.AC_NAME,IT_MAST.[TYPE],STITEM.PMKEY,STMAIN.INV_NO,STITEM.ITSERIAL,CNT=0,IT_MAST.RATEUNIT,IT_MAST.IT_CODE'
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'FROM STITEM '
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN STMAIN ON (STITEM.ENTRY_TY=STMAIN.ENTRY_TY AND STITEM.TRAN_CD=STMAIN.TRAN_CD)'
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN PROJECTITREF ON (STITEM.ENTRY_TY=PROJECTITREF.ENTRY_TY AND STITEM.TRAN_CD=PROJECTITREF.TRAN_CD AND STITEM.ITSERIAL=PROJECTITREF.ITSERIAL)'
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN IT_MAST  ON (STITEM.IT_CODE=IT_MAST.IT_CODE)'
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN AC_MAST  ON (STMAIN.AC_ID=AC_MAST.AC_ID)'
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND ISNULL(PROJECTITREF.BATCHNO,'+ char(39)+space(1)+char(39)+')<>'+char(39)+space(1)+char(39)
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND STMAIN.ENTRY_TY='+ char(39)+'ST'+char(39)


---- Modified By: Birendra : Bug-5961 on 03/11/2012:Start:
SET @SQLCOMMAND='INSERT INTO #BATCH1 SELECT PROJECTITREF.BATCHNO,PROJECTITREF.MFGDT,PROJECTITREF.EXPDT,STITEM.ENTRY_TY,STITEM.DATE,STITEM.TRAN_CD,IT_MAST.IT_NAME,STITEM.QTY,AC_MAST.AC_NAME,IT_MAST.[TYPE],STITEM.PMKEY,STMAIN.INV_NO,STITEM.ITSERIAL,CNT=0,IT_MAST.RATEUNIT,IT_MAST.IT_CODE'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'FROM stkl_vw_item stitem '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN stkl_vw_main STMAIN ON (STITEM.ENTRY_TY=STMAIN.ENTRY_TY AND STITEM.TRAN_CD=STMAIN.TRAN_CD)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN PROJECTITREF ON (STITEM.ENTRY_TY=PROJECTITREF.ENTRY_TY AND STITEM.TRAN_CD=PROJECTITREF.TRAN_CD AND STITEM.ITSERIAL=PROJECTITREF.ITSERIAL)'
--Birendra : Bug-7897 on 18/12/2012 :Commented:
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN (select * from projectitref where projectitref.aentry_ty + cast(projectitref.atran_cd as varchar)+projectitref.aitserial+cast(projectitref.ait_code as varchar)+ltrim(projectitref.pmkey) not in(select a.entry_ty + cast(a.tran_cd as varchar)+a.itserial+cast(a.it_code as varchar)+ltrim(a.pmkey) from projectitref a))
--										PROJECTITREF ON (STITEM.ENTRY_TY=PROJECTITREF.ENTRY_TY AND STITEM.TRAN_CD=PROJECTITREF.TRAN_CD AND STITEM.ITSERIAL=PROJECTITREF.ITSERIAL)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN IT_MAST  ON (STITEM.IT_CODE=IT_MAST.IT_CODE)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN AC_MAST  ON (STMAIN.AC_ID=AC_MAST.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND ISNULL(PROJECTITREF.BATCHNO,'+ char(39)+space(1)+char(39)+')<>'+char(39)+space(1)+char(39)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND STMAIN.ENTRY_TY In ('+ char(39)+'ST'+char(39) +','+ char(39)+'DC'+char(39)+')'
---- Modified By: Birendra : Bug-5961 on 03/11/2012 :End:

PRINT @SQLCOMMAND
--EXECUTE SP_EXECUTESQL @SQLCOMMAND --Birendra : Bug-7897 on 18/12/2012 :Commented:
print 'installer 12.0'

DECLARE @CNT NUMERIC(10)
SET @CNT=0
DECLARE @BATCHNO VARCHAR(20),@MFGDT SMALLDATETIME,@EXPDT SMALLDATETIME,@ENTRY_TY VARCHAR(2),@TRAN_CD INT ,@INV_NO VARCHAR(10),@DATE SMALLDATETIME,@IT_NAME VARCHAR(100),@QTY NUMERIC(13,3),@AC_NAME VARCHAR(100),@TYPE VARCHAR(40),@PMKEY VARCHAR(1),@ITSERIAL VARCHAR(5),@RATEUNIT VARCHAR(3),@IT_CODE INT
DECLARE CUR_BATCH CURSOR FOR 
SELECT BATCHNO,MFGDT,EXPDT,ENTRY_TY,DATE,TRAN_CD,IT_NAME,QTY,AC_NAME,[TYPE],PMKEY,INV_NO,ITSERIAL,RATEUNIT,IT_CODE
FROM #BATCH1 
WHERE ENTRY_TY='WK' --and batchno='NCI-102'
ORDER BY BATCHNO,DATE,TRAN_CD

OPEN CUR_BATCH
FETCH NEXT FROM CUR_BATCH INTO @BATCHNO,@MFGDT,@EXPDT,@ENTRY_TY,@DATE,@TRAN_CD,@IT_NAME,@QTY,@AC_NAME,@TYPE,@PMKEY,@INV_NO,@ITSERIAL,@RATEUNIT,@IT_CODE
WHILE (@@FETCH_STATUS=0)
BEGIN
	SET @CNT=@CNT+1
	print 'a'+cast(@cnt	 as varchar)
	
	INSERT INTO #BATCH (BATCHNO,MFGDT,EXPDT,ENTRY_TY,DATE,TRAN_CD,IT_NAME,QTY,AC_NAME,[TYPE],PMKEY,INV_NO,ITSERIAL,CNT,RATEUNIT,IT_CODE)
	VALUES (@BATCHNO,@MFGDT,@EXPDT,@ENTRY_TY,@DATE,@TRAN_CD,@IT_NAME,@QTY,@AC_NAME,@TYPE,@PMKEY,@INV_NO,@ITSERIAL,@CNT,@RATEUNIT,@IT_CODE)
	
	INSERT INTO #BATCH (BATCHNO,MFGDT,EXPDT,ENTRY_TY,DATE,TRAN_CD,IT_NAME,QTY,AC_NAME,[TYPE],PMKEY,INV_NO,ITSERIAL,CNT,RATEUNIT,IT_CODE)
	SELECT A.BATCHNO,A.MFGDT,A.EXPDT,B.ENTRY_TY,B.DATE,B.TRAN_CD,B.IT_NAME,A.QTY,B.AC_NAME,B.[TYPE],B.PMKEY,B.INV_NO,B.ITSERIAL,CNT=@CNT,B.RATEUNIT,B.IT_CODE
	FROM PROJECTITREF A
	INNER JOIN #BATCH1 B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL)
	WHERE A.ENTRY_TY='IP' AND AENTRY_TY=@ENTRY_TY AND ATRAN_CD=@TRAN_CD AND AITSERIAL=@ITSERIAL AND A.BATCHNO=@BATCHNO
	--ORDER BY B.BATCHNO,B.DATE,B.TRAN_CD  --commented by suraj on dated 25/02/15 for bug 25356	
	ORDER BY A.BATCHNO,A.DATE,A.TRAN_CD  --added by suraj on dated 25/02/15 for bug 25356	
		
	
	INSERT INTO #BATCH (BATCHNO,MFGDT,EXPDT,ENTRY_TY,DATE,TRAN_CD,IT_NAME,QTY,AC_NAME,[TYPE],PMKEY,INV_NO,ITSERIAL,CNT,RATEUNIT,IT_CODE)
	SELECT A.BATCHNO,A.MFGDT,A.EXPDT,B.ENTRY_TY,B.DATE,B.TRAN_CD,B.IT_NAME,A.QTY,B.AC_NAME,B.[TYPE],B.PMKEY,B.INV_NO,B.ITSERIAL,CNT=@CNT,B.RATEUNIT,B.IT_CODE
	FROM PROJECTITREF A
	INNER JOIN #BATCH1 B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL )
	WHERE A.ENTRY_TY='OP' AND AENTRY_TY=@ENTRY_TY AND ATRAN_CD=@TRAN_CD AND AITSERIAL=@ITSERIAL AND AIT_CODE=@IT_CODE AND A.BATCHNO=@BATCHNO
	--ORDER BY B.BATCHNO,B.DATE,B.TRAN_CD  --commented by suraj on dated 25/02/15 for bug 25356	
	ORDER BY a.BATCHNO,a.DATE,a.TRAN_CD  --added by suraj on dated 25/02/15 for bug 25356	
	
	INSERT INTO #BATCH (BATCHNO,MFGDT,EXPDT,ENTRY_TY,DATE,TRAN_CD,IT_NAME,QTY,AC_NAME,[TYPE],PMKEY,INV_NO,ITSERIAL,CNT,RATEUNIT,IT_CODE)
	SELECT distinct A.BATCHNO,A.MFGDT,A.EXPDT,B.ENTRY_TY,B.DATE,B.TRAN_CD,B.IT_NAME,A.QTY,B.AC_NAME,B.[TYPE],B.PMKEY,B.INV_NO,B.ITSERIAL,CNT=@CNT,B.RATEUNIT,B.IT_CODE
	FROM PROJECTITREF A
	INNER JOIN #BATCH1 B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL )
--	WHERE A.ENTRY_TY = 'ST','DC') 
	WHERE A.ENTRY_TY in ('ST','DC') 
	AND AENTRY_TY+CAST(ATRAN_CD AS VARCHAR)+AITSERIAL IN 
	(
		SELECT b.ENTRY_TY+CAST(b.TRAN_CD AS VARCHAR)+b.ITSERIAL
		FROM PROJECTITREF A
		INNER JOIN #BATCH1 B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL )
		WHERE A.ENTRY_TY='OP' AND AENTRY_TY=@ENTRY_TY AND ATRAN_CD=@TRAN_CD AND AITSERIAL=@ITSERIAL AND AIT_CODE=@IT_CODE AND A.BATCHNO=@BATCHNO
	)
	--ORDER BY A.BATCHNO,B.DATE,B.TRAN_CD --commented by suraj k on date 25-02-2015 for bug-25365
	ORDER BY A.BATCHNO,A.DATE,A.TRAN_CD --added by suraj k on date 25-02-2015 for bug-25365

	FETCH NEXT FROM CUR_BATCH INTO @BATCHNO,@MFGDT,@EXPDT,@ENTRY_TY,@DATE,@TRAN_CD,@IT_NAME,@QTY,@AC_NAME,@TYPE,@PMKEY,@INV_NO,@ITSERIAL,@RATEUNIT,@IT_CODE
END
	
CLOSE CUR_BATCH
DEALLOCATE CUR_BATCH

SELECT A.BATCHNO,A.MFGDT,A.EXPDT,A.ENTRY_TY,A.DATE,A.TRAN_CD,A.IT_NAME,A.QTY,A.AC_NAME,A.[TYPE],A.PMKEY,A.INV_NO,A.ITSERIAL,CNT=@CNT,A.RATEUNIT,A.IT_CODE
INTO #BATCH2
FROM #BATCH1 A 
--WHERE A.ENTRY_TY+CAST(A.TRAN_CD AS VARCHAR)+A.ITSERIAL NOT IN (SELECT DISTINCT  ENTRY_TY+CAST(A.TRAN_CD AS VARCHAR)+ITSERIAL FROM #BATCH1)
--Birendra : Bug-5006 on 01/08/2012
WHERE A.ENTRY_TY+CAST(A.TRAN_CD AS VARCHAR)+A.ITSERIAL NOT IN (SELECT DISTINCT  ENTRY_TY+CAST(A.TRAN_CD AS VARCHAR)+ITSERIAL FROM #BATCH)
--ORDER BY B.BATCHNO,B.DATE,B.TRAN_CD --commented by suraj k on date 25-02-2015 for bug-25365
ORDER BY a.BATCHNO,a.DATE,A.TRAN_CD --added by suraj k on date 25-02-2015 for bug-25365

INSERT INTO #BATCH (BATCHNO,MFGDT,EXPDT,ENTRY_TY,DATE,TRAN_CD,IT_NAME,QTY,AC_NAME,[TYPE],PMKEY,INV_NO,ITSERIAL,CNT,RATEUNIT,IT_CODE)
SELECT A.BATCHNO,A.MFGDT,A.EXPDT,A.ENTRY_TY,A.DATE,A.TRAN_CD,A.IT_NAME,A.QTY,A.AC_NAME,A.[TYPE],A.PMKEY,A.INV_NO,A.ITSERIAL,CNT=@CNT,RATEUNIT,A.IT_CODE
FROM #BATCH2 A 
--ORDER BY B.BATCHNO,CNT --commented by suraj k on date 25-02-2015 for bug-25365
ORDER BY a.BATCHNO,CNT --added by suraj k on date 25-02-2015 for bug-25365



SET @CNT=@CNT+1


SELECT a.*,b.code_nm 
FROM #BATCH a 
inner join lcode b on (a.entry_ty=b.entry_ty) 
--where a.entry_ty='st'
ORDER BY a.BATCHNO,a.CNT

DROP TABLE #BATCH1
DROP TABLE #BATCH2
DROP TABLE #BATCH
GO
