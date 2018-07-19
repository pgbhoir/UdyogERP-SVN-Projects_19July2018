DROP PROCEDURE [USP_REP_118REC]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author     :	
-- Create date: 
-- Description:	This Store Procedure is used to generate 118 Receipt (Imported) & 118 Receipt Summary (Imported) Reports.
-- Modified Date : Changes done by Ajay Jaiswal for EOU on 21/09/2010.
-- Modified Remark:
-- =============================================

create PROCEDURE  [USP_REP_118REC] 
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
 

DECLARE @FCON AS NVARCHAR(2000),@VSAMT DECIMAL(14,2),@VEAMT DECIMAL(14,2)
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
,@VMAINFILE='m',@VITFILE='it',@VACFILE=''--,@VRULE ='IMPORTED'
,@VDTFLD ='DATE'
,@VLYN =NULL
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

IF @FCON IS NOT NULL OR @FCON <> ''
BEGIN
	SET @FCON = @FCON + 'AND m.[RULE] = ''IMPORTED'''
END


DECLARE @SQLCOMMAND NVARCHAR(4000), @VCOND NVARCHAR(2000)

--Changed by Ajay Jaiswal for EOU on 21/09/2010 --- Start
SET @SQLCOMMAND='SELECT it.TRAN_CD, it.ENTRY_TY, it.INV_NO, it.DATE, IT_MAST.IT_NAME, it.QTY, it.ITEM_NO, m.ARENO, m.AREDATE,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'RINV_NO=case when isnull(it.dc_no,'''') = '''' then it.inv_no else m1.inv_no end,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'RINV_DT=case when isnull(it.dc_no,'''') = '''' then it.date else m1.date end,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'m.BENO as U_BENO,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'m.BEDT as U_BEDT, m.PINVNO, m.PINVDT,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'it.BCDAMT,it.BCDPER,'--,it.U_CESSAMT ,it.U_HCESAMT ,it.CCDAMT,it.HCDAMT,it.U_CVDAMT,'
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'it.BCDPER,it.U_CESSPER ,it.U_HCESSPER ,it.CCDPER,it.HCDPER,it.U_CVDPER,' -- Changes done by Ajay jaiswal on 02/12/2010
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'it.U_CUSTAMT,it.U_CUSTPER,it.U_CESSPER ,it.U_HCESSPER ,it.CCDPER,it.HCDPER,it.U_CVDPER,'
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'it.U_FREAMT, it.U_INSAMT,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'IT_MAST.P_UNIT, m.[RULE], it.U_ASSEAMT, it.RATE,it.itserial'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'FROM PTITEM it INNER JOIN PTMAIN m ON (it.ENTRY_TY=m.ENTRY_TY AND it.TRAN_CD=m.TRAN_CD)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN IT_MAST ON (it.ITEM=IT_MAST.IT_NAME)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'LEFT JOIN PTITREF itr ON ( it.TRAN_CD=itr.TRAN_CD and itr.itserial=it.itserial)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'LEFT JOIN armain m1 ON ( itr.rentry_ty = m1.entry_ty  and itr.itref_tran=m1.tran_cd)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND it.entry_ty = ''P1'' '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'ORDER BY it.DATE,it.ITEM'
print @SQLCOMMAND
EXECUTE SP_EXECUTESQL @SQLCOMMAND

IF @@ERROR = 0BEGINPRINT 'STORE PROCEDURE SUCCESSFULLY UPDATED..!'END
--Changed by Ajay Jaiswal for EOU on 21/09/2010 --- End

-- SET @SQLCOMMAND='SELECT PTITEM.TRAN_CD,PTITEM.ENTRY_TY,PTITEM.INV_NO,PTITEM.DATE,IT_MAST.IT_NAME,PTITEM.QTY,PTITEM.ITEM_NO,'
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'PTITREF.RINV_NO,PTMAIN.BENO as U_BENO,PTMAIN.BEDT as U_BEDT , PTMAIN.PINVNO,PTMAIN.PINVDT,'
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'PTITEM.BCDAMT,PTITEM.U_CESSAMT ,PTITEM.U_HCESAMT ,PTITEM.CCDAMT,PTITEM.HCDAMT,PTITEM.U_CVDAMT,'
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'PTITEM.BCDPER,PTITEM.U_CESSPER ,PTITEM.U_HCESSPER ,PTITEM.CCDPER,PTITEM.HCDPER,PTITEM.U_CVDPER,'
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'PTITEM.U_FREAMT,PTITEM.U_INSAMT,'
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'IT_MAST.P_UNIT,PTMAIN.[RULE],ARITREF.RINV_NO,PTITEM.U_ASSEAMT,PTITEM.RATE FROM PTITEM '
---- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'PTITEM.U_CIFAMT,PTITEM.U_ACAMT,PTITEM.U_EXAMT,PTITEM.U_ACAMT1,PTITEM.U_HACAMT1,PTITEM.U_CESSAMT,PTITEM.U_HCESSAMT,'
---- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'PTITEM.U_IMPDUTY,IT_MAST.P_UNIT,PTMAIN.[RULE],ARITREF.RINV_NO,PTITEM.U_ASSEAMT FROM PTITEM '
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN PTMAIN ON(PTITEM.ENTRY_TY=PTMAIN.ENTRY_TY AND PTITEM.TRAN_CD=PTMAIN.TRAN_CD)'
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN STKL_VW_MAIN ON(PTMAIN.ENTRY_TY=STKL_VW_MAIN.ENTRY_TY AND PTMAIN.TRAN_CD=STKL_VW_MAIN.TRAN_CD)'
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'INNER JOIN IT_MAST ON (PTITEM.ITEM=IT_MAST.IT_NAME)'
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'LEFT JOIN PTITREF ON ( PTITEM.TRAN_CD=PTITREF.TRAN_CD) AND (PTITREF.DATE BETWEEN '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' AND '+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+') ' 
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'LEFT JOIN ARITEM ON (ARITEM.ENTRY_TY=PTITREF.RENTRY_TY AND ARITEM.DATE=PTITREF.DATE AND ARITEM.INV_NO=PTITREF.RINV_NO)'    
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'LEFT JOIN ARITREF ON (ARITEM.TRAN_CD=ARITREF.TRAN_CD ) AND (PTITREF.DATE BETWEEN '+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' AND '+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)+')'
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND PTMAIN.ENTRY_TY=''P1'''
-- --SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'AND STKL_VW_MAIN.[RULE]=`IMPORTED`'
-- SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'ORDER BY PTITEM.DATE,PTITEM.ITEM'
------PTITEM.U_CIFAMT,----PRINT @SQLCOMMAND----EXECUTE SP_EXECUTESQL @SQLCOMMAND
GO
