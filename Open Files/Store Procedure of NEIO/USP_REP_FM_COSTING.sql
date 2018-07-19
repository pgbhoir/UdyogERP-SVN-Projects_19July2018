DROP PROCEDURE [USP_REP_FM_COSTING]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXECUTE USP_REP_FM_COSTING '','','','04/01/2016 12:00:00 AM','03/31/2017 12:00:00 AM',' ',' ',' ',' ',0,0,' ',' ',' ',' ',' ',' ',' ',' ','2016-2017',''
-- =============================================
-- Author:		Amrendra Singh
-- Create date: 28/02/2011
-- Description:	This Stored procedure is useful to generate FM Costing Report.
-- Modify date: 
-- Modified By: 
-- Modify date: 
-- Remark:
-- =============================================
Create PROCEDURE  [USP_REP_FM_COSTING]
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

SELECT A.BATCHNO,A.MFGDT,A.EXPDT,A.ENTRY_TY,A.DATE,A.TRAN_CD,C.IT_NAME,A.QTY,D.AC_NAME,C.[TYPE],A.PMKEY,B.INV_NO,A.ITSERIAL,cnt=999999999,C.RATEUNIT,A.IT_CODE,A.Rate,999999999.9999 AS U_LOSS
INTO #BATCH1
FROM ITEM A
INNER JOIN MAIN B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD)
INNER JOIN IT_MAST C ON (A.IT_CODE=C.IT_CODE)
INNER JOIN AC_MAST D ON (B.AC_ID=D.AC_ID)
WHERE 1=2
SELECT * INTO #BATCH FROM #BATCH1  

print @fcon

DECLARE @SQLCOMMAND NVARCHAR(4000),@VCOND NVARCHAR(1000)
SET @SQLCOMMAND='INSERT INTO #BATCH1 SELECT STKL_VW_ITEM.BATCHNO,STKL_VW_ITEM.MFGDT,STKL_VW_ITEM.EXPDT,STKL_VW_ITEM.ENTRY_TY,STKL_VW_ITEM.DATE,STKL_VW_ITEM.TRAN_CD,IT_MAST.IT_NAME,STKL_VW_ITEM.QTY,AC_MAST.AC_NAME,IT_MAST.[TYPE],STKL_VW_ITEM.PMKEY,STKL_VW_MAIN.INV_NO,STKL_VW_ITEM.ITSERIAL,CNT=0,IT_MAST.RATEUNIT,IT_MAST.IT_CODE,STKL_VW_ITEM.RATE,STKL_VW_ITEM.U_LOSS'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'FROM STKL_VW_ITEM '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN STKL_VW_MAIN ON (STKL_VW_ITEM.ENTRY_TY=STKL_VW_MAIN.ENTRY_TY AND STKL_VW_ITEM.TRAN_CD=STKL_VW_MAIN.TRAN_CD)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN IT_MAST  ON (STKL_VW_ITEM.IT_CODE=IT_MAST.IT_CODE)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN AC_MAST  ON (STKL_VW_MAIN.AC_ID=AC_MAST.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND ISNULL(STKL_VW_ITEM.BATCHNO,'+ char(39)+space(1)+char(39)+')<>'+char(39)+space(1)+char(39)
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

SET @SQLCOMMAND='INSERT INTO #BATCH1 SELECT ITEM.BATCHNO,ITEM.MFGDT,ITEM.EXPDT,ITEM.ENTRY_TY,ITEM.DATE,ITEM.TRAN_CD,IT_MAST.IT_NAME,ITEM.QTY,AC_MAST.AC_NAME,IT_MAST.[TYPE],ITEM.PMKEY,MAIN.INV_NO,ITEM.ITSERIAL,CNT=0,IT_MAST.RATEUNIT,IT_MAST.IT_CODE,ITEM.RATE,0'
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

SET @SQLCOMMAND='INSERT INTO #BATCH1 SELECT PROJECTITREF.BATCHNO,PROJECTITREF.MFGDT,PROJECTITREF.EXPDT,STITEM.ENTRY_TY,STITEM.DATE,STITEM.TRAN_CD,IT_MAST.IT_NAME,STITEM.QTY,AC_MAST.AC_NAME,IT_MAST.[TYPE],STITEM.PMKEY,STMAIN.INV_NO,STITEM.ITSERIAL,CNT=0,IT_MAST.RATEUNIT,IT_MAST.IT_CODE,STITEM.RATE,0'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'FROM STITEM '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN STMAIN ON (STITEM.ENTRY_TY=STMAIN.ENTRY_TY AND STITEM.TRAN_CD=STMAIN.TRAN_CD)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN PROJECTITREF ON (STITEM.ENTRY_TY=PROJECTITREF.ENTRY_TY AND STITEM.TRAN_CD=PROJECTITREF.TRAN_CD AND STITEM.ITSERIAL=PROJECTITREF.ITSERIAL)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN IT_MAST  ON (STITEM.IT_CODE=IT_MAST.IT_CODE)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN AC_MAST  ON (STMAIN.AC_ID=AC_MAST.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND ISNULL(PROJECTITREF.BATCHNO,'+ char(39)+space(1)+char(39)+')<>'+char(39)+space(1)+char(39)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND STMAIN.ENTRY_TY='+ char(39)+'ST'+char(39)
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND



DECLARE @CNT NUMERIC(10)
SET @CNT=0
DECLARE @BATCHNO VARCHAR(20),@MFGDT SMALLDATETIME,@EXPDT SMALLDATETIME,@ENTRY_TY VARCHAR(2),@TRAN_CD INT ,@INV_NO VARCHAR(10),@DATE SMALLDATETIME,@IT_NAME VARCHAR(100),@QTY NUMERIC(13,3),@AC_NAME VARCHAR(100),@TYPE VARCHAR(40),@PMKEY VARCHAR(1),@ITSERIAL VARCHAR(5),@RATEUNIT VARCHAR(3),@IT_CODE INT,@RATE NUMERIC(12,2),@U_LOSS NUMERIC(10,3)
DECLARE CUR_BATCH CURSOR FOR 
SELECT BATCHNO,MFGDT,EXPDT,ENTRY_TY,DATE,TRAN_CD,IT_NAME,QTY,AC_NAME,[TYPE],PMKEY,INV_NO,ITSERIAL,RATEUNIT,IT_CODE,rate*0,u_loss
FROM #BATCH1 
WHERE ENTRY_TY='WK' --and batchno='NCI-102'
ORDER BY BATCHNO,DATE,TRAN_CD

OPEN CUR_BATCH
FETCH NEXT FROM CUR_BATCH INTO @BATCHNO,@MFGDT,@EXPDT,@ENTRY_TY,@DATE,@TRAN_CD,@IT_NAME,@QTY,@AC_NAME,@TYPE,@PMKEY,@INV_NO,@ITSERIAL,@RATEUNIT,@IT_CODE,@RATE,@U_LOSS
WHILE (@@FETCH_STATUS=0)
BEGIN
	SET @CNT=@CNT+1
	print 'a'+cast(@cnt	 as varchar)
	
	INSERT INTO #BATCH (BATCHNO,MFGDT,EXPDT,ENTRY_TY,DATE,TRAN_CD,IT_NAME,QTY,AC_NAME,[TYPE],PMKEY,INV_NO,ITSERIAL,CNT,RATEUNIT,IT_CODE,RATE,U_LOSS)
	VALUES (@BATCHNO,@MFGDT,@EXPDT,@ENTRY_TY,@DATE,@TRAN_CD,@IT_NAME,@QTY,@AC_NAME,@TYPE,@PMKEY,@INV_NO,@ITSERIAL,@CNT,@RATEUNIT,@IT_CODE,@RATE,@U_LOSS)
	
	INSERT INTO #BATCH (BATCHNO,MFGDT,EXPDT,ENTRY_TY,DATE,TRAN_CD,IT_NAME,QTY,AC_NAME,[TYPE],PMKEY,INV_NO,ITSERIAL,CNT,RATEUNIT,IT_CODE,Rate,U_LOSS)
	SELECT A.BATCHNO,A.MFGDT,A.EXPDT,B.ENTRY_TY,B.DATE,B.TRAN_CD,B.IT_NAME,A.QTY,B.AC_NAME,B.[TYPE],B.PMKEY,B.INV_NO,B.ITSERIAL,CNT=@CNT,B.RATEUNIT,B.IT_CODE,B.RATE,B.U_LOSS
	FROM PROJECTITREF A
	INNER JOIN #BATCH1 B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL)
	WHERE A.ENTRY_TY='IP' AND AENTRY_TY=@ENTRY_TY AND ATRAN_CD=@TRAN_CD AND AITSERIAL=@ITSERIAL AND A.BATCHNO=@BATCHNO
	ORDER BY B.BATCHNO,B.DATE,B.TRAN_CD 
	
	INSERT INTO #BATCH (BATCHNO,MFGDT,EXPDT,ENTRY_TY,DATE,TRAN_CD,IT_NAME,QTY,AC_NAME,[TYPE],PMKEY,INV_NO,ITSERIAL,CNT,RATEUNIT,IT_CODE,Rate,U_LOSS)
	SELECT A.BATCHNO,A.MFGDT,A.EXPDT,B.ENTRY_TY,B.DATE,B.TRAN_CD,B.IT_NAME,A.QTY,B.AC_NAME,B.[TYPE],B.PMKEY,B.INV_NO,B.ITSERIAL,CNT=@CNT,B.RATEUNIT,B.IT_CODE,B.Rate,B.U_LOSS
	FROM PROJECTITREF A
	INNER JOIN #BATCH1 B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL )
	WHERE A.ENTRY_TY='OP' AND AENTRY_TY=@ENTRY_TY AND ATRAN_CD=@TRAN_CD AND AITSERIAL=@ITSERIAL AND AIT_CODE=@IT_CODE AND A.BATCHNO=@BATCHNO
	ORDER BY B.BATCHNO,B.DATE,B.TRAN_CD
-----------------------------
	INSERT INTO #BATCH (BATCHNO,MFGDT,EXPDT,ENTRY_TY,DATE,TRAN_CD,IT_NAME,QTY,AC_NAME,[TYPE],PMKEY,INV_NO,ITSERIAL,CNT,RATEUNIT,IT_CODE,Rate,U_LOSS)
	SELECT A.BATCHNO,A.MFGDT,A.EXPDT,B.ENTRY_TY,B.DATE,B.TRAN_CD,B.IT_NAME,A.QTY,B.AC_NAME,B.[TYPE],B.PMKEY,B.INV_NO,B.ITSERIAL,CNT=@CNT,B.RATEUNIT,B.IT_CODE,B.Rate,B.U_LOSS
	FROM PROJECTITREF A
	INNER JOIN #BATCH1 B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL )
	WHERE A.ENTRY_TY='WI' and [type]='Raw Material' AND AENTRY_TY=@ENTRY_TY AND ATRAN_CD=@TRAN_CD AND AITSERIAL=@ITSERIAL AND AIT_CODE=@IT_CODE AND A.BATCHNO=@BATCHNO
	ORDER BY B.BATCHNO,B.DATE,B.TRAN_CD
-----------------------------
	
	INSERT INTO #BATCH (BATCHNO,MFGDT,EXPDT,ENTRY_TY,DATE,TRAN_CD,IT_NAME,QTY,AC_NAME,[TYPE],PMKEY,INV_NO,ITSERIAL,CNT,RATEUNIT,IT_CODE,Rate,U_LOSS)
	SELECT distinct A.BATCHNO,A.MFGDT,A.EXPDT,B.ENTRY_TY,B.DATE,B.TRAN_CD,B.IT_NAME,A.QTY,B.AC_NAME,B.[TYPE],B.PMKEY,B.INV_NO,B.ITSERIAL,CNT=@CNT,B.RATEUNIT,B.IT_CODE,B.Rate,B.U_LOSS
	FROM PROJECTITREF A
	INNER JOIN #BATCH1 B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL )
	WHERE A.ENTRY_TY='ST' 
	AND AENTRY_TY+CAST(ATRAN_CD AS VARCHAR)+AITSERIAL IN 
	(
		SELECT b.ENTRY_TY+CAST(b.TRAN_CD AS VARCHAR)+b.ITSERIAL
		FROM PROJECTITREF A
		INNER JOIN #BATCH1 B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL )
		WHERE A.ENTRY_TY='OP' AND AENTRY_TY=@ENTRY_TY AND ATRAN_CD=@TRAN_CD AND AITSERIAL=@ITSERIAL AND AIT_CODE=@IT_CODE AND A.BATCHNO=@BATCHNO
	)
	ORDER BY A.BATCHNO,B.DATE,B.TRAN_CD
   
--Added By Prajakta B. on 11102017 for Bug 30658 Start

  update bt
		set batchno=bt.BATCHNO,MFGDT=bt.MFGDT,EXPDT=bt.EXPDT,ENTRY_TY=Bt.ENTRY_TY,[DATE]=Bt.DATE,TRAN_CD=Bt.TRAN_CD,IT_NAME=Bt.IT_NAME
		,qty=qty-ir.rqty,AC_NAME=bt.AC_NAME,[TYPE]=bt.[TYPE],PMKEY=bt.PMKEY,INV_NO=Bt.INV_NO,ITSERIAL=Bt.ITSERIAL,cnt=bt.CNT
		,rateunit=Bt.RATEUNIT,it_code=Bt.IT_CODE,rate=Bt.RATE,u_loss=Bt.U_LOSS
  from #BATCH bt
  inner join (select rentry_ty,itref_tran,ritserial,sum(rqty) as rqty from IRITREF group by rentry_ty,itref_tran,ritserial )IR on 
			 (bt.entry_ty=ir.rentry_ty and bt.itserial=ir.ritserial and bt.tran_cd=ir.itref_tran)	
	
--Added By Prajakta B. on 11102017 for Bug 30658 End  
  
	FETCH NEXT FROM CUR_BATCH INTO @BATCHNO,@MFGDT,@EXPDT,@ENTRY_TY,@DATE,@TRAN_CD,@IT_NAME,@QTY,@AC_NAME,@TYPE,@PMKEY,@INV_NO,@ITSERIAL,@RATEUNIT,@IT_CODE,@rate,@U_LOSS
END
	
CLOSE CUR_BATCH
DEALLOCATE CUR_BATCH

SELECT A.BATCHNO,A.MFGDT,A.EXPDT,A.ENTRY_TY,A.DATE,A.TRAN_CD,A.IT_NAME,A.QTY,A.AC_NAME,A.[TYPE],A.PMKEY,A.INV_NO,A.ITSERIAL,CNT=@CNT,A.RATEUNIT,A.IT_CODE,A.Rate,A.U_LOSS
INTO #BATCH2
FROM #BATCH1 A 
WHERE A.ENTRY_TY+CAST(A.TRAN_CD AS VARCHAR)+A.ITSERIAL NOT IN (SELECT DISTINCT  ENTRY_TY+CAST(A.TRAN_CD AS VARCHAR)+ITSERIAL FROM #BATCH1)
--ORDER BY B.BATCHNO,B.DATE,B.TRAN_CD --commented by suraj k on date 25-02-2015 for bug-25365
 ORDER BY A.BATCHNO,A.DATE,A.TRAN_CD --ADDED by suraj k on date 25-02-2015 for bug-25365


INSERT INTO #BATCH (BATCHNO,MFGDT,EXPDT,ENTRY_TY,DATE,TRAN_CD,IT_NAME,QTY,AC_NAME,[TYPE],PMKEY,INV_NO,ITSERIAL,CNT,RATEUNIT,IT_CODE,Rate,U_LOSS)
SELECT A.BATCHNO,A.MFGDT,A.EXPDT,A.ENTRY_TY,A.DATE,A.TRAN_CD,A.IT_NAME,A.QTY,A.AC_NAME,A.[TYPE],A.PMKEY,A.INV_NO,A.ITSERIAL,CNT=@CNT,RATEUNIT,A.IT_CODE,A.Rate,A.U_LOSS
FROM #BATCH2 A 
--ORDER BY B.BATCHNO,CNT  --commented by suraj k on date 25-02-2015 for bug-25365
ORDER BY a.BATCHNO,CNT  --ADDED by suraj k on date 25-02-2015 for bug-25365


SET @CNT=@CNT+1
-- Added By amrendra on 21_04_2011  *** Start
alter table #batch add FinMat varchar(50),FinQty numeric(9,3) 
declare @MCNT as int,@MFMName as varchar(50),@MFQty as numeric(9,3)
declare new_cursor1 cursor FOR 
select cnt,IT_name from #batch a inner join lcode b on (a.entry_ty=b.entry_ty) where a.entry_ty!='WK' and upper(type)='FINISHED' ORDER BY a.BATCHNO,a.CNT,a.pmkey 

open new_cursor1
fetch next from new_cursor1 into @MCNT,@MFMName
while (@@FETCH_STATUS=0)
BEGIN

select @MFQty=sum(qty) from #batch where CNT=@MCNT and upper(type)='FINISHED' and pmkey ='+'


UPDATE #BATCH SET FINMAT=@MFMName,FinQty=@MFQty WHERE CNT=@MCNT
fetch next from new_cursor1 into @MCNT,@MFMName
END
CLOSE NEW_CURSOR1
DEALLOCATE NEW_CURSOR1
-- Added By amrendra on 21_04_2011 *** End

SELECT a.*,b.code_nm
FROM #BATCH a 
inner join lcode b on (a.entry_ty=b.entry_ty) 
where a.entry_ty!='WK' and a.qty>0
ORDER BY a.BATCHNO,A.[TYPE],a.CNT,a.pmkey


DROP TABLE #BATCH1
DROP TABLE #BATCH2
DROP TABLE #BATCH
GO
