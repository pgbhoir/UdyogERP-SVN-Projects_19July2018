If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_POS_TRANWISE_PAYDETAIL')
Begin
	Drop Procedure USP_REP_POS_TRANWISE_PAYDETAIL
End
/****** Object:  StoredProcedure [dbo].[USP_REP_POS_TRANWISE_PAYDETAIL]    Script Date: 08/27/2018 12:46:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[USP_REP_POS_TRANWISE_PAYDETAIL]
@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SINV_SR AS VARCHAR(20),@EINV_SR AS VARCHAR(20)
,@SDEPT AS VARCHAR(20),@EDEPT AS VARCHAR(20)
,@SCATE AS VARCHAR(20),@ECATE AS VARCHAR(20)
,@SINV_NO AS VARCHAR(15),@EINV_NO AS VARCHAR(15)
AS

DECLARE @SQLCOMMAND NVARCHAR(4000)
--Commented by Priyanka B on 27082018 for Bug-31756 Start
/*--SET @SQLCOMMAND='SELECT DI.DATE,DI.INV_NO,DM.INV_SR,DI.ITSERIAL,DM.DEPT,DM.CATE,DI.ITEM,DI.QTY,DI.RATE,DI.U_ASSEAMT,DI.TAXPERCENT,DI.TAXAMT,'
SET @SQLCOMMAND='SELECT DI.DATE,DI.INV_NO,DM.INV_SR,DI.ITSERIAL,DM.DEPT,DM.CATE,DI.ITEM,DI.QTY,DI.RATE,DI.U_ASSEAMT,'--DI.tax_name as TAXPERCENT,DI.TAXAMT,' --added for bugno 27503 on 23/02/16
SET @SQLCOMMAND=@SQLCOMMAND+' '+'DI.GRO_AMT,PS.PAYMODE,CASHAMT=CASE WHEN PS.PAYMODE=''CASH'' THEN TOTALVALUE ELSE 0 END,'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'COUPONAMT=CASE WHEN PS.PAYMODE=''COUPON'' THEN TOTALVALUE ELSE 0 END,CHEQUEAMT=CASE WHEN PS.PAYMODE=''CHEQUE'' THEN TOTALVALUE ELSE 0 END,'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'CARDAMT=CASE WHEN PS.PAYMODE=''CARD'' THEN TOTALVALUE ELSE 0 END,DM.TOTALPAID,DM.BALAMT ,DM.ROUNDOFF,'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'DI.CGST_PER,DI.CGST_AMT,DI.SGST_PER,DI.SGST_AMT'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'FROM DCMAIN DM'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN DCITEM DI ON (DM.ENTRY_TY=DI.ENTRY_TY AND DM.TRAN_CD=DI.TRAN_CD)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN PSPAYDETAIL PS ON (DM.INV_NO=PS.INV_NO)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'WHERE DI.DATE BETWEEN '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' AND '+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+' AND DM.INV_SR BETWEEN '''+@SINV_SR+''' AND '''+@EINV_SR+''' AND DM.DEPT BETWEEN '''+@SDEPT+''' AND '''+@EDEPT+''''
SET @SQLCOMMAND=@SQLCOMMAND+' '+'AND DM.CATE BETWEEN '''+@SCATE+''' AND '''+@ECATE+''' AND DM.INV_NO BETWEEN '''+@SINV_NO+''' AND '''+@EINV_NO+''''
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND*/
--Commented by Priyanka B on 27082018 for Bug-31756 End

--Modified by Priyanka B on 27082018 for Bug-31756 Start
SET @SQLCOMMAND='SELECT DI.DATE,DI.INV_NO,DM.INV_SR,DI.ITSERIAL,DM.DEPT,DM.CATE,DI.ITEM,DI.QTY,DI.RATE,DI.U_ASSEAMT,'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'DI.GRO_AMT,PS.PAYMODE,SUM(TOTALVALUE) AS TOTALVALUE,CASHAMT=CASE WHEN PS.PAYMODE=''CASH'' THEN SUM(TOTALVALUE) ELSE 0 END,'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'COUPONAMT=CASE WHEN PS.PAYMODE=''COUPON'' THEN SUM(TOTALVALUE) ELSE 0 END,CHEQUEAMT=CASE WHEN PS.PAYMODE=''CHEQUE'' THEN SUM(TOTALVALUE) ELSE 0 END,'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'CARDAMT=CASE WHEN PS.PAYMODE=''CARD'' THEN SUM(TOTALVALUE) ELSE 0 END,DM.TOTALPAID,DM.BALAMT ,DM.ROUNDOFF,'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'DI.CGST_PER,DI.CGST_AMT,DI.SGST_PER,DI.SGST_AMT'
SET @SQLCOMMAND=@SQLCOMMAND+' '+' into #temptbl_1'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'FROM DCMAIN DM'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN DCITEM DI ON (DM.ENTRY_TY=DI.ENTRY_TY AND DM.TRAN_CD=DI.TRAN_CD)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN PSPAYDETAIL PS ON (DM.INV_NO=PS.INV_NO)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'WHERE DI.DATE BETWEEN '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' AND '+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+' AND DM.INV_SR BETWEEN '''+@SINV_SR+''' AND '''+@EINV_SR+''' AND DM.DEPT BETWEEN '''+@SDEPT+''' AND '''+@EDEPT+''''
SET @SQLCOMMAND=@SQLCOMMAND+' '+'AND DM.CATE BETWEEN '''+@SCATE+''' AND '''+@ECATE+''' AND DM.INV_NO BETWEEN '''+@SINV_NO+''' AND '''+@EINV_NO+''''
SET @SQLCOMMAND=@SQLCOMMAND+' '+'group by DI.DATE,DI.INV_NO,DM.INV_SR,DI.ITSERIAL,DM.DEPT,DM.CATE,DI.ITEM,DI.QTY,DI.RATE,DI.U_ASSEAMT, DI.GRO_AMT,PS.PAYMODE
								,DM.TOTALPAID,DM.BALAMT ,DM.ROUNDOFF, DI.CGST_PER,DI.CGST_AMT,DI.SGST_PER,DI.SGST_AMT '
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND

SET @SQLCOMMAND=@SQLCOMMAND+' '+'
;WITH DUPLICATEREC AS
(
SELECT RECID=ROW_NUMBER() OVER (PARTITION BY INV_NO,TOTALVALUE
ORDER BY INV_NO,ITSERIAL),*
FROM #TEMPTBL_1
) UPDATE DUPLICATEREC SET CASHAMT=0, COUPONAMT=0 , CHEQUEAMT=0 , CARDAMT=0,TOTALPAID=0,BALAMT=0, ROUNDOFF=0 WHERE RECID>1'

SET @SQLCOMMAND=@SQLCOMMAND+' '+'SELECT DATE,INV_NO,INV_SR,ITSERIAL,DEPT,CATE,ITEM,QTY,RATE,U_ASSEAMT, GRO_AMT
,CASHAMT = SUM(CASHAMT), CHEQUEAMT = SUM(CHEQUEAMT) ,COUPONAMT = SUM(COUPONAMT) ,CARDAMT = SUM(CARDAMT),TOTALPAID,BALAMT,ROUNDOFF, CGST_PER,CGST_AMT,SGST_PER,SGST_AMT 
FROM #TEMPTBL_1 GROUP BY DATE,INV_NO,INV_SR,ITSERIAL,DEPT,CATE,ITEM,QTY,RATE,U_ASSEAMT, GRO_AMT,TOTALPAID,BALAMT ,ROUNDOFF, CGST_PER,CGST_AMT,SGST_PER,SGST_AMT 
ORDER BY INV_NO,ITSERIAL'
PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND
--Added by Priyanka B on 27082018 for Bug-31756 End