DROP PROCEDURE [USP_STKL_VW_ITEM]
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [USP_STKL_VW_ITEM] AS


DECLARE @SQLCOMMAND NVARCHAR(4000)
DECLARE @WHCOND NVARCHAR(250)
SET @WHCOND = ' WHERE (PMKEY='+CHAR(39)+'+' +CHAR(39)+ 'OR PMKEY='+CHAR(39)+'-'+CHAR(39)+') AND DC_NO='+CHAR(39)+' ' +CHAR(39)

SET @SQLCOMMAND='CREATE VIEW STKL_VW_ITEM AS '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM STITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM PTITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM ARITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM OBITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM BPITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM BRITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
---SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM CNITEM ' + RTRIM(@WHCOND) 
--SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM CPITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM IIITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM PCITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM POITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM SOITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM SQITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM SRITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM DCITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM CRITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
--SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM DNITEM ' + RTRIM(@WHCOND) 
--SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM EPITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM ESITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM IPITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM IRITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
--SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM JVITEM ' + RTRIM(@WHCOND) 
--SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM OPITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM PRITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM SSITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM EQITEM ' + RTRIM(@WHCOND) 
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND) +' UNION '
SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' SELECT ENTRY_TY,DATE,DOC_NO,AC_ID,IT_CODE,QTY,U_LQTY,ITSERIAL,PMKEY,GRO_AMT,WARE_NM,TRAN_CD FROM TRITEM ' + RTRIM(@WHCOND) 

EXEC SP_EXECUTESQL @SQLCOMMAND
PRINT  @SQLCOMMAND
GO
