set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:  Hetal L. Patel
-- Create date: 01/06/2009
-- Description:	This Stored procedure is useful to generate RJ VAT FORM 50
-- Modify date: 10/06/2009
-- Modified By: Dinesh
-- Modify date: 
-- Remark:
-- =============================================
ALTER PROCEDURE [dbo].[USP_REP_RJVAT50]
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
BEGIN
DECLARE @FCON AS NVARCHAR(2000)
EXECUTE   USP_REP_FILTCON 

@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=@SDATE,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='STMAIN',@VITFILE='STITEM',@VACFILE='AC'
,@VDTFLD ='DATE'
,@VLYN=Null
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT


DECLARE @SQLCOMMAND NVARCHAR(4000)

Declare @MultiCo	VarChar(3)
Declare @MCON as NVARCHAR(2000)
IF Exists(Select A.ID From SysObjects A Inner Join SysColumns B On(A.ID = B.ID) Where A.[Name] = 'STMAIN' And B.[Name] = 'DBNAME')
	Begin	------Fetch Records from Multi Co. Data
		Set @MultiCo = 'YES'
		SET @SQLCOMMAND='SELECT STMAIN.ENTRY_TY,STMAIN.TRAN_CD,STITEM.ITSERIAL,STMAIN.INV_NO,STMAIN.DATE,STITEM.ITEM,STITEM.QTY,STITEM.RATE,STITEM.GRO_AMT,STMAIN.FORM_NO,STMAIN.FORMIDT,STMAIN.FORMRDT,'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'STMAIN.U_PINVNO,STMAIN.U_PINVDT,STITEM.TAX_NAME,STITEM.TAXAMT,AC_MAST.AC_ID,AC_MAST.AC_NAME,AC_MAST.MAILNAME,AC_MAST.S_TAX,AC_MAST.CITY,'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'IT_MAST.[IT_DESC],IT_MAST.IT_CODE'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'FROM STMAIN'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN STITEM ON (STMAIN.ENTRY_TY=STITEM.ENTRY_TY AND STMAIN.TRAN_CD=STITEM.TRAN_CD AND STMAIN.DBNAME=STITEM.DBNAME)'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN AC_MAST ON (STMAIN.AC_ID=AC_MAST.AC_ID AND STMAIN.DBNAME=AC_MAST.DBNAME)'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN IT_MAST ON (IT_MAST.IT_CODE=STITEM.IT_CODE AND IT_MAST.DBNAME=STITEM.DBNAME)'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+@FCON
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'AND AC_MAST.ST_TYPE = ''OUT OF STATE'''
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'ORDER BY STMAIN.DATE,AC_MAST.AC_NAME,IT_MAST.IT_NAME'
	End
Else
	Begin	------Fetch Records from Single Co. Data
		Set @MultiCo = 'NO'
		SET @SQLCOMMAND='SELECT STMAIN.ENTRY_TY,STMAIN.TRAN_CD,STITEM.ITSERIAL,STMAIN.INV_NO,STMAIN.DATE,STITEM.ITEM,STITEM.QTY,STITEM.RATE,STITEM.GRO_AMT,STMAIN.FORM_NO,STMAIN.FORMIDT,STMAIN.FORMRDT,'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'STMAIN.U_PINVNO,STMAIN.U_PINVDT,STITEM.TAX_NAME,STITEM.TAXAMT,AC_MAST.AC_ID,AC_MAST.AC_NAME,AC_MAST.MAILNAME,AC_MAST.S_TAX,AC_MAST.CITY,'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'IT_MAST.[IT_DESC],IT_MAST.IT_CODE'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'FROM STMAIN'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN STITEM ON (STMAIN.ENTRY_TY=STITEM.ENTRY_TY AND STMAIN.TRAN_CD=STITEM.TRAN_CD)'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN AC_MAST ON (STMAIN.AC_ID=AC_MAST.AC_ID)'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN IT_MAST ON (IT_MAST.IT_CODE=STITEM.IT_CODE)'
		SET @SQLCOMMAND=@SQLCOMMAND+' '+@FCON
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'AND AC_MAST.ST_TYPE = ''OUT OF STATE'''
		SET @SQLCOMMAND=@SQLCOMMAND+' '+'ORDER BY STMAIN.DATE,AC_MAST.AC_NAME,IT_MAST.IT_NAME'
	End

PRINT @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND

END
--Print 'RJ VAT FORM 50'
--Go
-------
--Print 'RJ STORED PROCEDURE UPDATION COMPLETED'
--Go
