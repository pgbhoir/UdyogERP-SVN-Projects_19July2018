set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Ruepesh Prajapati
-- Create date: 16/05/2007
-- Description:	This Stored procedure is useful to generate Weekly Er-1 Report.
-- Modification Date/By/Reason: 04/10/2010 Rupesh Prajapati. Modified for Opening Balance Problem.
-- Remark:
-- =============================================
ALTER PROCEDURE        [dbo].[USP_REP_ER1_WEEKLY]
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

SET QUOTED_IDENTIFIER OFF
DECLARE @FCON AS NVARCHAR(2000),@VSAMT DECIMAL(14,4),@VEAMT DECIMAL(14,4)

EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=Null
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
,@VLYN=Null
,@VEXPARA=NULL
,@VFCON =@FCON OUTPUT

SELECT ENTRY_TY,BEH=(CASE WHEN EXT_VOU=1 THEN BCODE_NM ELSE ENTRY_TY END) INTO #LCODE FROM LCODE

SELECT IT_MAST.CHAPNO,IT_MAST.IT_NAME,IT_MAST.RATEUNIT,IT_MAST.[GROUP],IT_MAST.EIT_NAME,ExrateUnit 
,OPBAL=STKL_VW_ITEM.QTY
,RQTY=STKL_VW_ITEM.QTY
,STMQTY=STKL_VW_ITEM.QTY
,STNQTY=STKL_VW_ITEM.QTY
,STEXQTY=STKL_VW_ITEM.QTY
,STMAS=STKL_VW_ITEM.U_ASSEAMT
,STNAS=STKL_VW_ITEM.U_ASSEAMT
,STEXAS=STKL_VW_ITEM.U_ASSEAMT
,STMDUTY=STKL_VW_ITEM.U_ASSEAMT
,STNDUTY=STKL_VW_ITEM.U_ASSEAMT
,STEXDUTY=STKL_VW_ITEM.U_ASSEAMT
,CLBAL=STKL_VW_ITEM.U_ASSEAMT
into #er1_week
FROM STKL_VW_MAIN 
INNER JOIN STKL_VW_ITEM ON (STKL_VW_MAIN.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STKL_VW_MAIN.TRAN_CD=STKL_VW_ITEM.TRAN_CD) 
INNER JOIN IT_MAST ON (STKL_VW_ITEM.IT_CODE=IT_MAST.IT_CODE) 
INNER JOIN #LCODE L ON (L.ENTRY_TY=STKL_VW_MAIN.ENTRY_TY) 
LEFT JOIN STITEM STI ON (STI.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STI.TRAN_CD=STKL_VW_ITEM.TRAN_CD AND STI.ITSERIAL=STKL_VW_ITEM.ITSERIAL)  
WHERE 1=2


DECLARE @SQLCOMMAND NVARCHAR(4000),@VCOND NVARCHAR(1000)
SET @SQLCOMMAND='insert into #er1_week SELECT IT_MAST.CHAPNO ,IT_MAST.IT_NAME,IT_MAST.RATEUNIT,IT_MAST.[GROUP],EIT_NAME=(CASE WHEN IT_MAST.EIT_NAME<>'+CHAR(39)+' '+CHAR(39)+ ' THEN IT_MAST.EIT_NAME ELSE IT_MAST.IT_NAME END),ExrateUnit'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ',OPBAL=SUM(CASE WHEN IT_MAST.[TYPE] LIKE `%FINISHED%` THEN   (CASE WHEN (L.BEH=`OS` OR STKL_VW_ITEM.DATE<='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') THEN   (CASE WHEN STKL_VW_ITEM.PMKEY=`+` THEN STKL_VW_ITEM.QTY ELSE -STKL_VW_ITEM.QTY END) ELSE 0 END)    ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ',RQTY=SUM(CASE WHEN STKL_VW_ITEM.PMKEY=`+` and not (L.BEH=`OS` OR STKL_VW_ITEM.DATE<'+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') THEN STKL_VW_ITEM.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ',STMQTY=SUM(CASE WHEN STKL_VW_ITEM.PMKEY=`-`  AND ISNULL(STI.EXAMT,0)<>0 and not (L.BEH=`OS` OR STKL_VW_ITEM.DATE<'+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') THEN STKL_VW_ITEM.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ',STNQTY=SUM(CASE WHEN STKL_VW_ITEM.PMKEY=`-` AND ISNULL(STI.EXAMT,0)=0 and not (L.BEH=`OS` OR STKL_VW_ITEM.DATE<'+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') AND STKL_VW_MAIN.[RULE] NOT IN (`CT-1`,`CT-3`,`UT-1`,`UT-3`) THEN STKL_VW_ITEM.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ',STEXQTY=SUM(CASE WHEN STKL_VW_ITEM.PMKEY=`-` AND ISNULL(STI.EXAMT,0)=0 and not (L.BEH=`OS` OR STKL_VW_ITEM.DATE<'+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') AND STKL_VW_MAIN.[RULE] IN (`CT-1`,`CT-3`,`UT-1`,`UT-3`) THEN STKL_VW_ITEM.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ',STMAS=SUM(CASE WHEN STKL_VW_ITEM.PMKEY=`-`  and not (L.BEH=`OS` OR STKL_VW_ITEM.DATE<'+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') AND ISNULL(STI.EXAMT,0)<>0 THEN STKL_VW_ITEM.U_ASSEAMT ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ',STNAS=SUM(CASE WHEN STKL_VW_ITEM.PMKEY=`-` and not (L.BEH=`OS` OR STKL_VW_ITEM.DATE<'+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') AND ISNULL(STI.EXAMT,0)=0 AND STKL_VW_MAIN.[RULE] NOT IN (`CT-1`,`CT-3`,`UT-1`,`UT-3`) THEN STKL_VW_ITEM.U_ASSEAMT ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ',STEXAS=SUM(CASE WHEN STKL_VW_ITEM.PMKEY=`-` and not (L.BEH=`OS` OR STKL_VW_ITEM.DATE<'+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') AND ISNULL(STI.EXAMT,0)=0 AND STKL_VW_MAIN.[RULE] IN (`CT-1`,`CT-3`,`UT-1`,`UT-3`) THEN STKL_VW_ITEM.U_ASSEAMT ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ',STMDUTY=SUM(CASE WHEN STKL_VW_ITEM.PMKEY=`-` and not (L.BEH=`OS` OR STKL_VW_ITEM.DATE<'+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') AND ISNULL(STI.EXAMT,0)<>0 THEN STI.EXAMT+STI.U_CESSAMT+STI.U_HCESAMT ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ',STNDUTY=SUM(CASE WHEN STKL_VW_ITEM.PMKEY=`-` and not (L.BEH=`OS` OR STKL_VW_ITEM.DATE<'+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') AND ISNULL(STI.EXAMT,0)=0 AND STKL_VW_MAIN.[RULE] NOT IN (`CT-1`,`CT-3`,`UT-1`,`UT-3`) THEN STI.EXAMT+STI.U_CESSAMT+STI.U_HCESAMT ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ',STEXDUTY=SUM(CASE WHEN STKL_VW_ITEM.PMKEY=`-` and not (L.BEH=`OS` OR STKL_VW_ITEM.DATE<'+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') AND ISNULL(STI.EXAMT,0)=0 AND STKL_VW_MAIN.[RULE] IN (`CT-1`,`CT-3`,`UT-1`,`UT-3`) THEN STI.EXAMT+STI.U_CESSAMT+STI.U_HCESAMT ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ',CLBAL=SUM(CASE WHEN IT_MAST.[TYPE] LIKE `%FINISHED%` THEN   (CASE WHEN STKL_VW_ITEM.PMKEY=`+` THEN STKL_VW_ITEM.QTY ELSE -STKL_VW_ITEM.QTY END) ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'FROM STKL_VW_MAIN '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN STKL_VW_ITEM ON (STKL_VW_MAIN.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STKL_VW_MAIN.TRAN_CD=STKL_VW_ITEM.TRAN_CD)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN IT_MAST ON (STKL_VW_ITEM.IT_CODE=IT_MAST.IT_CODE)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'INNER JOIN #LCODE L ON (L.ENTRY_TY=STKL_VW_MAIN.ENTRY_TY)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'LEFT JOIN STITEM STI ON (STI.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STI.TRAN_CD=STKL_VW_ITEM.TRAN_CD AND STI.ITSERIAL=STKL_VW_ITEM.ITSERIAL)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+  RTRIM(@FCON)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'GROUP BY CHAPNO,IT_MAST.IT_NAME,IT_MAST.RATEUNIT,IT_MAST.[GROUP],IT_MAST.EIT_NAME,ExrateUnit'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ 'ORDER BY CHAPNO,IT_MAST.EIT_NAME,IT_MAST.ExrateUnit,IT_MAST.RATEUNIT,IT_MAST.[GROUP],IT_MAST.IT_NAME'
SET @SQLCOMMAND=REPLACE(@SQLCOMMAND, '`',CHAR(39))
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND

delete from #er1_week where ((rqty=0) and (STMQTY+STNQTY+STEXQTY=0) and (STMAS+STNAS+STEXAS=0))

select * from #er1_week
--EXECUTE USP_REP_ER1_WEEKLY '','','( ((STKL_VW_ITEM.ENTRY_TY IN (`PT`,`II`,`IR`,`LR`,`GR`,`HR`,`OS`,`OP`,`ST`,`LI`,`GI`,`HI`,`IP`) AND IT_MAST.TYPE IN (`FINISHED`)) OR (STKL_VW_ITEM.ENTRY_TY IN (`ST`,`PR`) AND IT_MAST.TYPE NOT IN (`FINISHED`)) ) AND STKL_VW_MAIN.[RULE]!=`NON-MODVATABLE`) AND STKL_VW_MAIN.U_GCSSR=0','04/01/2008','03/31/2009','','','','',0,0,'','','','','','','','','2008-2009',''
--CHAPNO,EIT_NAME,ExrateUnit

