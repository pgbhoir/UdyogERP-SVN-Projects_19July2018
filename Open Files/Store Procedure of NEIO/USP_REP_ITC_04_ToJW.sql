If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_ITC_04_ToJW')
Begin
	Drop Procedure USP_REP_ITC_04_ToJW
End
/****** Object:  StoredProcedure [dbo].[USP_REP_ITC_04_ToJW]    Script Date: 05/07/2018 11:55:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- EXECUTE USP_REP_ITC_04_ToJW '04/01/2018','03/31/2019'
-- Author: Prajakta B.
-- Create date:05-07-2018
-- Description:	This Stored procedure is useful to generate Form GST ITC-04 Excel.
-- Remark:
-- =============================================

Create PROCEDURE [dbo].[USP_REP_ITC_04_ToJW] 
@LSTARTDATE  SMALLDATETIME,@LENDDATE SMALLDATETIME
AS

Declare @CGST_AMT varchar(MAX),@SGST_AMT varchar(MAX),@IGST_AMT varchar(MAX),@CESS_AMT varchar(MAX)

Declare @CGST_PER varchar(MAX),@SGST_PER varchar(MAX),@IGST_PER varchar(MAX),@CESS_PER varchar(MAX)

IF EXISTS(SELECT * FROM SYSOBJECTS O INNER JOIN SYSCOLUMNS C ON (O.ID=C.ID) WHERE O.NAME='iiitem' AND C.NAME IN ('CGST_AMT','SGST_AMT','IGST_AMT'))
BEGIN
 SET @CGST_AMT = 'iii.CGST_AMT'
 SET @SGST_AMT = 'iii.SGST_AMT'
 SET @IGST_AMT = 'iii.IGST_AMT'
END
ELSE
BEGIN
 SET @CGST_AMT = '0 as CGST_AMT'
 SET @SGST_AMT = '0 as SGST_AMT' 
 SET @IGST_AMT = '0 as IGST_AMT' 
END

IF EXISTS(SELECT * FROM SYSOBJECTS O INNER JOIN SYSCOLUMNS C ON (O.ID=C.ID) WHERE O.NAME='iiitem' AND C.NAME IN ('CGST_PER','SGST_PER','IGST_PER'))
BEGIN
 SET @CGST_PER = 'iii.CGST_PER'
 SET @SGST_PER = 'iii.SGST_PER'
 SET @IGST_PER = 'iii.IGST_PER'
END
ELSE
BEGIN
 SET @CGST_PER = '0 as CGST_PER'
 SET @SGST_PER = '0 as SGST_PER' 
 SET @IGST_PER = '0 as IGST_PER' 
END

IF EXISTS(SELECT * FROM SYSOBJECTS O INNER JOIN SYSCOLUMNS C ON (O.ID=C.ID) WHERE O.NAME='iiitem' AND C.NAME IN ('COMPCESS'))
BEGIN
 SET @CESS_AMT = 'iii.COMPCESS'
END
ELSE
BEGIN
 SET @CESS_AMT = '0 as COMPCESS'
END

IF EXISTS(SELECT * FROM SYSOBJECTS O INNER JOIN SYSCOLUMNS C ON (O.ID=C.ID) WHERE O.NAME='iiitem' AND C.NAME IN ('CCESSRATE'))
BEGIN
 SET @CESS_PER = 'iii.CCESSRATE'
END
ELSE
BEGIN
 SET @CESS_PER = '0 as CCESSRATE'
END

DECLARE @SQLCOMMAND NVARCHAR(4000)
SET @SQLCOMMAND ='SELECT GSTIN=case when ac_mast.GSTIN=''UNREGISTERED'' then '''' else ac_mast.GSTIN end,State=state.LetrCode,
						 Challan_no= case when iii.Pinvno='''' then iim.INV_NO else iii.Pinvno end, 
						 Challan_dt= case when year(iii.Pinvdt)>1900 then iii.Pinvdt else iii.[DATE] end,	
						 TypeofGoods= case when IT_MAST.[type]=''Machinery/Stores'' then ''Capital'' else ''Input'' end,
						 It_Desc=(CASE WHEN ISNULL(IT_MAST.it_alias,'''')='''' THEN IT_MAST.it_name ELSE IT_MAST.it_alias END), 
						 p_unit= case when iii.QTY>0 then IT_MAST.P_UNIT else '''' end,
						 Quantity=iii.QTY,TaxAmt=iii.u_asseamt,supp_type=case when ac_mast.supp_type=''SEZ'' then ''SEZ'' else ''Non SEZ'' end,
					   	 CGST_AMT='+@CGST_AMT+',SGST_AMT='+@SGST_AMT+',IGST_AMT='+@IGST_AMT+',CESS_AMT='+@CESS_AMT+',
						 CGST_PER='+@CGST_PER+',SGST_PER='+@SGST_PER+',IGST_PER='+@IGST_PER+',CESS_PER='+@CESS_PER+'
				  FROM IIITEM iii
				  INNER JOIN IIMAIN iim ON  (iii.TRAN_CD=iim.TRAN_CD and iii.entry_ty=iim.entry_ty) 
				  INNER JOIN AC_MAST ac_mast ON (ac_mast.AC_ID=iii.AC_ID)
				  INNER JOIN STATE state on (ac_mast.state_id=state.state_id)	
				  INNER JOIN IT_MAST IT_MAST ON (iii.IT_CODE=IT_MAST.IT_CODE)  
				  where iim.entry_ty in (''LI'',''I5'') and (iim.DATE BETWEEN '+CHAR(39)+CAST(@LSTARTDATE AS VARCHAR)+CHAR(39)+' AND '+CHAR(39)+CAST(@LENDDATE AS VARCHAR)+CHAR(39)+')
				  ORDER BY iii.DATE,iii.INV_NO,iii.ITSERIAL'				   
print @SQLCOMMAND
EXEC SP_EXECUTESQL  @SQLCOMMAND
	
--EXECUTE USP_REP_ITC_04_ToJW '04/01/2018','04/30/2019'
