If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_ITC_04')
Begin
	Drop Procedure USP_REP_ITC_04
End
GO
/****** Object:  StoredProcedure [dbo].[USP_REP_ITC_04]    Script Date: 2019-03-07 14:38:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- EXECUTE USP_REP_ITC_04 '','','','04/01/2018','03/31/2019','','','','',0,0,'','','','','','','','','2018-2019',''
-- Author: Prajakta B.
-- Create date:30/12/2017
-- Description:	This Stored procedure is useful to generate Form GST ITC-04.
-- Remark:
-- Modified by : Priyanka B on 05122019 for Bug-33056
-- =============================================

Create PROCEDURE [dbo].[USP_REP_ITC_04] 
@TMPAC NVARCHAR(60),@TMPIT NVARCHAR(60),@SPLCOND NVARCHAR(500),
	@SDATE SMALLDATETIME,@EDATE SMALLDATETIME,
	@SNAME NVARCHAR(60),@ENAME NVARCHAR(60),
	@SITEM NVARCHAR(60),@EITEM NVARCHAR(60),
	@SAMT NUMERIC,@EAMT NUMERIC,
	@SDEPT NVARCHAR(60),@EDEPT NVARCHAR(60),
	@SCAT NVARCHAR(60),@ECAT NVARCHAR(60),
	@SWARE NVARCHAR(60),@EWARE NVARCHAR(60),
	@SINVSR NVARCHAR(60),@EINVSR NVARCHAR(60),
	@FINYR NVARCHAR(20),@expara NVARCHAR(1000)
AS

Declare @FCON as NVARCHAR(2000),@VSAMT DECIMAL(14,4),@VEAMT DECIMAL(14,4),@tmpDate smalldatetime
EXECUTE   USP_REP_FILTCON 
		@VTMPAC=@TMPAC,@VTMPIT=@TMPIT,@VSPLCOND=@SPLCOND, 
		@VSDATE=@SDATE,@VEDATE=@EDATE,    
		@VSAC =@SNAME,@VEAC =@ENAME, 
		@VSIT=@SITEM,@VEIT=@EITEM,
		@VSAMT=@SAMT,@VEAMT=@EAMT,  
		@VSDEPT=@SDEPT,@VEDEPT=@EDEPT,
		@VSCATE =@SCAT,@VECATE =@ECAT,
		@VSWARE =@SWARE,@VEWARE  =@EWARE,
		@VSINV_SR =@SINVSR,@VEINV_SR =@EINVSR,
		@VMAINFILE='iim',@VITFILE='iii',@VACFILE='ac_mast',
		@VDTFLD = 'DATE',@VLYN=null,@VEXPARA=@expara,
		@VFCON =@FCON OUTPUT

PRINT @FCON
PRINT @EXPARA
DECLARE @SQLCOMMAND NVARCHAR(4000),@VCOND NVARCHAR(1000)

-- added by Prajakta B. on 05092017 Start
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
-- added by Prajakta B. on 05092017 end

-- added by Prajakta B. on 01122017 for Bug 30972 end
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
-- added by Prajakta B. on 01122017 for Bug 30972 end

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

SELECT PART=0,'AAAAA' as PARTSR,CAST('' AS VARCHAR(20)) as ii_gstin,cast('' as varchar(50)) as ii_state,
cast('' as varchar(40)) as ii_inv_no,cast('' as varchar(20)) as iichallanno,cast('' as DATETIME) as iichallandt,
cast('' as DATETIME) as iidate,cast('' as varchar(4000)) as ii_it_desc,cast('' as varchar(2000)) as ii_itname,
cast('' as varchar(20)) as ii_p_unit,ii_qty=CAST(0 AS DECIMAL(18,2)),ii_taxamt=CAST(0 AS DECIMAL(18,2)),cast('' as varchar(50)) as type,
cgst_amt=CAST(0 AS DECIMAL(18,2)),sgst_amt=CAST(0 AS DECIMAL(18,2)),igst_amt=CAST(0 AS DECIMAL(18,2)),cess_amt=CAST(0 AS DECIMAL(18,2)),
cgst_per=CAST(0 AS DECIMAL(18,2)),sgst_per=CAST(0 AS DECIMAL(18,2)),igst_per=CAST(0 AS DECIMAL(18,2)),cess_per=CONVERT (VARCHAR(80),0),
cast('' as varchar(20)) as ir_gstin,cast('' as varchar(50)) as ir_state,cast('' as varchar(2000)) as irname,cast('' as varchar(1000)) as ir_invno,
cast('' as DATETIME) as ir_date,cast('' as varchar(20)) as chno_jw,cast('' AS DATETIME) as chdt_jw,cast('' as varchar(50)) as chgstin_jw,
cast('' as varchar(50)) as chstate_jw,cast('' as varchar(50)) as invno_sjw,cast('' as DATETIME) as date_sjw,cast('' as varchar(4000)) as ir_It_Desc,
cast('' as varchar(2000)) as ir_IT_NAME,cast('' as varchar(20)) as ir_p_unit,ir_qty=CAST(0 AS DECIMAL(18,2)),ir_taxamt=CAST(0 AS DECIMAL(18,2))
,ir_wastage=CAST(0 AS DECIMAL(18,2))
,cast('' as varchar(20)) as nat_jw,cast('' as varchar(20)) as lwuqc,cast(0 as decimal(18,0)) as lwqty  --Added by Priyanka B on 05122019 for Bug-33056
into #TmpAnnexure4
FROM IIITEM iii 
WHERE 1=2

SET @SQLCOMMAND = 'Insert Into #TmpAnnexure4 (PART,PARTSR,ii_gstin,ii_state,ii_inv_no,iichallanno,iichallandt,iidate,ii_it_desc,ii_itname,
				   ii_p_unit,ii_qty,ii_taxamt,type,cgst_amt,sgst_amt,igst_amt,cess_amt,cgst_per,sgst_per,igst_per,cess_per,
				   ir_gstin,ir_state,irname,ir_invno,ir_date,chno_jw,chdt_jw,chgstin_jw,chstate_jw,invno_sjw,date_sjw,ir_It_Desc,ir_IT_NAME,
				   ir_p_unit,ir_qty,ir_taxamt,ir_wastage)'
				   
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+'SELECT PART=4,''4A'' as PARTSR,ii_gstin=ac_mast.GSTIN,ii_state=ac_mast.state,ii_inv_no=iim.INV_NO,iichallanno=iii.Pinvno,iichallandt=iii.Pinvdt,iidate=iii.DATE,
									ii_It_Desc=(CASE WHEN ISNULL(IT_MAST.it_alias,'''')='''' THEN IT_MAST.it_name ELSE IT_MAST.it_alias END),ii_itname=IT_MAST.IT_NAME, 
									ii_p_unit= case when iii.QTY>0 then IT_MAST.P_UNIT else '''' end,ii_qty=iii.QTY,ii_taxamt=iii.u_asseamt,IT_MAST.type,'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' '+@CGST_AMT+','+@SGST_AMT+','+@IGST_AMT+','+@CESS_AMT+''
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' ,'+@CGST_PER+','+@SGST_PER+','+@IGST_PER+','+@CESS_PER+''
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' ,'''' as ir_gstin,'''' as ir_state,'''' as irname,'''' as ir_invno,'''' as ir_date,'''' as chno_jw
										,'''' as chdt_jw, '''' as chgstin_jw,'''' as chstate_jw,'''' as invno_sjw,'''' as date_sjw,
										'''' as ir_It_Desc,'''' as ir_IT_NAME,'''' as ir_p_unit,ir_qty=0,ir_taxamt=0,ir_wastage=0'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM IIITEM iii'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN IIMAIN iim ON  (iii.TRAN_CD=iim.TRAN_CD and iii.entry_ty=iim.entry_ty) '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN AC_MAST ac_mast ON (ac_mast.AC_ID=iii.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN IT_MAST IT_MAST ON (iii.IT_CODE=IT_MAST.IT_CODE)  '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)+' and iim.entry_ty in (''LI'',''I5'')'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' ORDER BY iii.DATE,iii.INV_NO,iii.ITSERIAL'				   
print @sqlcommand
EXEC SP_EXECUTESQL  @SQLCOMMAND

IF NOT EXISTS(SELECT PART FROM #TmpAnnexure4  WHERE PART=4 and PARTSR = '4A')
BEGIN
	Insert Into #TmpAnnexure4 (PART,PARTSR,ii_gstin,ii_state,ii_inv_no,iichallanno,iichallandt,iidate,ii_it_desc,ii_itname,
				   ii_p_unit,ii_qty,ii_taxamt,type,cgst_amt,sgst_amt,igst_amt,cess_amt,cgst_per,sgst_per,igst_per,cess_per,
				   ir_gstin,ir_state,irname,ir_invno,ir_date,chno_jw,chdt_jw,chgstin_jw,chstate_jw,invno_sjw,date_sjw,ir_It_Desc,ir_IT_NAME,
				   ir_p_unit,ir_qty,ir_taxamt,ir_wastage)
	VALUES(4,'4A','','','','','','','','','',0,0,'',0,0,0,0,0,0,0,0,'','','','','','','','','','','','','','',0,0,0)
END
 
 
SET @SQLCOMMAND = 'Insert Into #TmpAnnexure4 (PART,PARTSR,ii_gstin,ii_state,ii_inv_no,iichallanno,iichallandt,iidate,ii_it_desc,ii_itname,
				   ii_p_unit,ii_qty,ii_taxamt,type,cgst_amt,sgst_amt,igst_amt,cess_amt,cgst_per,sgst_per,igst_per,cess_per,
				   ir_gstin,ir_state,irname,ir_invno,ir_date,chno_jw,chdt_jw,chgstin_jw,chstate_jw,invno_sjw,date_sjw,ir_It_Desc,ir_IT_NAME,
				   ir_p_unit,ir_qty,ir_taxamt,ir_wastage
				   ,nat_jw,lwuqc,lwqty)'  --Added by Priyanka B on 05122019 for Bug-33056
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+'SELECT  Distinct PART=4,''4A'' as PARTSR,'''' as ii_gstin,'''' as ii_state   --Commented by Prajakta B. on 20072018 for bug 31705 
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+'SELECT distinct PART=5,''5A'' as PARTSR,'''' as ii_gstin,'''' as ii_state    --Modified by Prajakta B. on 20072018 for bug 31705 
,'''' as ii_inv_no,'''' as iichallanno,'''' as iichallandt,'''' as iidate
,'''' as ii_It_Desc,'''' as ii_itname,'''' as ii_p_unit,ii_qty=0,ii_taxamt=0,'''' as type,
CGST_AMT=0,SGST_AMT=0,IGST_AMT=0,CESS_AMT=0,CGST_PER=0,SGST_PER=0,IGST_PER=0,CESS_PER=0,
ir_gstin=ac_mast.GSTIN,ir_state=ac_mast.state,irname=''Received Back'',
ir_invno=case when iiitem.pinvno='''' then iiitem.inv_no else IIITEM.Pinvno end,
ir_date=case when iiitem.pinvdt='''' then iiitem.date else IIITEM.pinvdt end
--,'''' as chno_jw,'''' as chdt_jw  --Commented by Priyanka B on 05122019 for Bug-33056
,iii.inv_no as chno_jw,iii.date as chdt_jw  --Modified by Priyanka B on 05122019 for Bug-33056
, '''' as chgstin_jw,'''' as chstate_jw,'''' as invno_sjw,'''' as date_sjw,
ir_It_Desc=(CASE WHEN ISNULL(IT_MAST.it_alias,'''')='''' THEN IT_MAST.it_name ELSE IT_MAST.it_alias END),ir_IT_NAME=IT_MAST.it_name,
ir_p_unit=case when iii.QTY>0 then IT_MAST.P_UNIT else '''' end,ir_qty=iii.qty,ir_taxamt=iii.U_ASSEAMT,ir_wastage=irrmdet.wastage
,nat_jw=iim.u_nopro,lwuqc=case when iii.QTY>0 then IT_MAST.P_UNIT else '''' end,lwqty=irrmdet.wastage  --Added by Priyanka B on 05122019 for Bug-33056
FROM iritem iii
INNER JOIN irmain iim ON  (iii.TRAN_CD=iim.TRAN_CD and iii.entry_ty=iim.entry_ty) 
LEFT JOIN IRRMDET ON (iii.TRAN_CD=IRRMDET.TRAN_CD AND iii.ENTRY_TY=IRRMDET.ENTRY_TY AND iii.ITSERIAL=IRRMDET.itserial)
left Join IIITEM on(IIITEM.entry_ty=IRRMDET.lientry_ty and IIITEM.Tran_cd=IRRMDET.li_Tran_cd AND IIITEM.ITSERIAL=IRRMDET.LI_ITSER  )
INNER JOIN AC_MAST ac_mast ON (ac_mast.AC_ID=iii.AC_ID)
INNER JOIN IT_MAST IT_MAST ON (iii.IT_CODE=IT_MAST.IT_CODE)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)+' and iim.entry_ty=''LR'''	
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'Order By ac_mast.GSTIN,ac_mast.state,
case when iiitem.pinvno='''' then iiitem.inv_no else IIITEM.Pinvno end,
case when iiitem.pinvdt='''' then iiitem.date else IIITEM.pinvdt end
,(CASE WHEN ISNULL(IT_MAST.it_alias,'''')='''' THEN IT_MAST.it_name ELSE IT_MAST.it_alias END),IT_MAST.it_name
,case when iii.QTY>0 then IT_MAST.P_UNIT else '''' end,iii.qty,iii.U_ASSEAMT,irrmdet.wastage'
print @SQLCOMMAND
EXEC SP_EXECUTESQL  @SQLCOMMAND

IF NOT EXISTS(SELECT PART FROM #TmpAnnexure4  WHERE PART=5 and PARTSR = '5A')
BEGIN
	Insert Into #TmpAnnexure4 (PART,PARTSR,ii_gstin,ii_state,ii_inv_no,iichallanno,iichallandt,iidate,ii_it_desc,ii_itname,
				   ii_p_unit,ii_qty,ii_taxamt,type,cgst_amt,sgst_amt,igst_amt,cess_amt,cgst_per,sgst_per,igst_per,cess_per,
				   ir_gstin,ir_state,irname,ir_invno,ir_date,chno_jw,chdt_jw,chgstin_jw,chstate_jw,invno_sjw,date_sjw,ir_It_Desc,ir_IT_NAME,
				   ir_p_unit,ir_qty,ir_taxamt,ir_wastage)
	VALUES(5,'5A','','','','','','','','','',0,0,'',0,0,0,0,0,0,0,0,'','','','','','','','','','','','','','',0,0,0)
END			   


SET @SQLCOMMAND = 'Insert Into #TmpAnnexure4 (PART,PARTSR,ii_gstin,ii_state,ii_inv_no,iichallanno,iichallandt,iidate,ii_it_desc,ii_itname,
				   ii_p_unit,ii_qty,ii_taxamt,type,cgst_amt,sgst_amt,igst_amt,cess_amt,cgst_per,sgst_per,igst_per,cess_per,
				   ir_gstin,ir_state,irname,ir_invno,ir_date,chno_jw,chdt_jw,chgstin_jw,chstate_jw,invno_sjw,date_sjw,ir_It_Desc,ir_IT_NAME,
				   ir_p_unit,ir_qty,ir_taxamt,ir_wastage
				   ,nat_jw,lwuqc,lwqty)'  --Added by Priyanka B on 05122019 for Bug-33056
				   
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+'SELECT distinct PART=5,''5B'' as PARTSR,'''' as ii_gstin,'''' as ii_state
,'''' as ii_inv_no,'''' as iichallanno,'''' as iichallandt,'''' as iidate,
'''' as ii_It_Desc,'''' as ii_itname,'''' as ii_p_unit,ii_qty=0,ii_taxamt=0,'''' as type,
CGST_AMT=0,SGST_AMT=0,IGST_AMT=0,CESS_AMT=0,CGST_PER=0,SGST_PER=0,IGST_PER=0,CESS_PER=0,
--'''' as ir_gstin,'''' as ir_state
ir_gstin=ac_mast.GSTIN,ir_state=ac_mast.state
,irname=''Sent out to another Job Worker''
--,'''' as ir_invno,'''' as ir_date,  --Commented by Priyanka B on 05122019 for Bug-33056
,IIITEM.Pinvno as ir_invno,IIITEM.Pinvdt as ir_date,  --Modified by Priyanka B on 05122019 for Bug-33056
--chno_jw=case when IIITEM.pinvno='''' then IIITEM.inv_no else IIITEM.pinvno end,chdt_jw=case when IIITEM.pinvdt='''' then IIITEM.date else IIITEM.Pinvdt end,  --Commented by Priyanka B on 05122019 for Bug-33056
chno_jw=III.inv_no,chdt_jw=III.date,  --Modified by Priyanka B on 05122019 for Bug-33056
chgstin_jw=ac_mast.GSTIN,chstate_jw=ac_mast.state,'''' as invno_sjw,'''' as date_sjw,
ir_It_Desc=(CASE WHEN ISNULL(IT_MAST.it_alias,'''')='''' THEN IT_MAST.it_name ELSE IT_MAST.it_alias END),ir_IT_NAME=IT_MAST.it_name,
ir_p_unit=case when iii.QTY>0 then IT_MAST.P_UNIT else '''' end,ir_qty=iii.qty,ir_taxamt=iii.U_ASSEAMT,ir_wastage=irrmdet.wastage
,nat_jw=iim.u_nopro,lwuqc=case when iii.QTY>0 then IT_MAST.P_UNIT else '''' end,lwqty=irrmdet.wastage  --Added by Priyanka B on 05122019 for Bug-33056
 FROM iritem iii
INNER JOIN irmain iim ON  (iii.TRAN_CD=iim.TRAN_CD and iii.entry_ty=iim.entry_ty) 
LEFT JOIN IRRMDET ON (iii.TRAN_CD=IRRMDET.TRAN_CD AND iii.ENTRY_TY=IRRMDET.ENTRY_TY AND iii.ITSERIAL=IRRMDET.itserial)
left Join IIITEM on(IIITEM.entry_ty=IRRMDET.lientry_ty and IIITEM.Tran_cd=IRRMDET.li_Tran_cd AND IIITEM.ITSERIAL=IRRMDET.LI_ITSER  )
INNER JOIN AC_MAST ac_mast ON (ac_mast.AC_ID=iii.AC_ID)
INNER JOIN IT_MAST IT_MAST ON (iii.IT_CODE=IT_MAST.IT_CODE) '	
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)+' and iim.entry_ty=''R1'''	
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'Order By ac_mast.GSTIN,ac_mast.state,
--case when iiitem.pinvno='''' then iiitem.inv_no else IIITEM.Pinvno end,case when iiitem.pinvdt='''' then iiitem.date else IIITEM.pinvdt end  --Commented by Priyanka B on 05122019 for Bug-33056
III.inv_no,III.date  --Modified by Priyanka B on 05122019 for Bug-33056
,(CASE WHEN ISNULL(IT_MAST.it_alias,'''')='''' THEN IT_MAST.it_name ELSE IT_MAST.it_alias END),IT_MAST.it_name
,case when iii.QTY>0 then IT_MAST.P_UNIT else '''' end,iii.qty,iii.U_ASSEAMT,irrmdet.wastage'

EXEC SP_EXECUTESQL  @SQLCOMMAND

IF NOT EXISTS(SELECT PART FROM #TmpAnnexure4  WHERE PART=5 and PARTSR = '5B')
BEGIN
	Insert Into #TmpAnnexure4 (PART,PARTSR,ii_gstin,ii_state,ii_inv_no,iichallanno,iichallandt,iidate,ii_it_desc,ii_itname,
				   ii_p_unit,ii_qty,ii_taxamt,type,cgst_amt,sgst_amt,igst_amt,cess_amt,cgst_per,sgst_per,igst_per,cess_per,
				   ir_gstin,ir_state,irname,ir_invno,ir_date,chno_jw,chdt_jw,chgstin_jw,chstate_jw,invno_sjw,date_sjw,ir_It_Desc,ir_IT_NAME,
				   ir_p_unit,ir_qty,ir_taxamt,ir_wastage)
	VALUES(5,'5B','','','','','','','','','',0,0,'',0,0,0,0,0,0,0,0,'','','','','','','','','','','','','','',0,0,0)
END			   
			   

SET @SQLCOMMAND = 'Insert Into #TmpAnnexure4 (PART,PARTSR,ii_gstin,ii_state,ii_inv_no,iichallanno,iichallandt,iidate,ii_it_desc,ii_itname,
				   ii_p_unit,ii_qty,ii_taxamt,type,cgst_per,cgst_amt,sgst_per,sgst_amt,igst_per,igst_amt,cess_per,cess_amt,
				   ir_gstin,ir_state,irname,ir_invno,ir_date,chno_jw,chdt_jw,chgstin_jw,chstate_jw,invno_sjw,date_sjw,ir_It_Desc,ir_IT_NAME,
				   ir_p_unit,ir_qty,ir_taxamt,ir_wastage
				   ,nat_jw,lwuqc,lwqty)'  --Added by Priyanka B on 05122019 for Bug-33056				   
				   
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+'SELECT distinct PART=5,''5C'' as PARTSR,'''' as ii_gstin,'''' as ii_state
,'''' as ii_inv_no,'''' as iichallanno,'''' as iichallandt,'''' as iidate
,'''' as ii_It_Desc,'''' as ii_itname,'''' as ii_p_unit,ii_qty=0,ii_taxamt=0,'''' as type,
CGST_AMT=0,SGST_AMT=0,IGST_AMT=0,CESS_AMT=0,CGST_PER=0,SGST_PER=0,IGST_PER=0,CESS_PER=0,
--'''' as ir_gstin,'''' as ir_state
ir_gstin=ac_mast.GSTIN,ir_state=ac_mast.state
,irname=''Supplied from premises of Job Worker''
--,'''' as ir_invno,'''' as ir_date,'''' as chno_jw,'''' as chdt_jw  --Commented by Priyanka B on 05122019 for Bug-33056
,III.u_invnopsp as ir_invno,III.u_invdtpsp as ir_date,III.inv_no as chno_jw,III.date as chdt_jw  --Modified by Priyanka B on 05122019 for Bug-33056
, '''' as chgstin_jw,'''' as chstate_jw,
invno_sjw=iim.inv_no,date_sjw=iim.date,
ir_It_Desc=(CASE WHEN ISNULL(IT_MAST.it_alias,'''')='''' THEN IT_MAST.it_name ELSE IT_MAST.it_alias END),ir_IT_NAME=IT_MAST.it_name,
ir_p_unit=case when iii.QTY>0 then IT_MAST.P_UNIT else '''' end,ir_qty=iii.qty,ir_taxamt=iii.U_ASSEAMT,ir_wastage=0
,nat_jw=iii.u_nopro,lwuqc=case when iii.QTY>0 then IT_MAST.P_UNIT else '''' end,lwqty=0  --Added by Priyanka B on 05122019 for Bug-33056
FROM stitem iii
INNER JOIN stmain iim ON  (iii.TRAN_CD=iim.TRAN_CD and iii.entry_ty=iim.entry_ty) 
INNER JOIN AC_MAST ac_mast ON (ac_mast.AC_ID=iii.AC_ID)
INNER JOIN IT_MAST IT_MAST ON (iii.IT_CODE=IT_MAST.IT_CODE)	'	
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)+' and iim.entry_ty=''ST'' and iii.u_invnopsp<>'''' and iii.u_invdtpsp<>'''' '	
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'Order By iim.inv_no,iim.date,
(CASE WHEN ISNULL(IT_MAST.it_alias,'''')='''' THEN IT_MAST.it_name ELSE IT_MAST.it_alias END),IT_MAST.it_name,
case when iii.QTY>0 then IT_MAST.P_UNIT else '''' end,iii.qty,iii.U_ASSEAMT'
		   
EXEC SP_EXECUTESQL  @SQLCOMMAND

IF NOT EXISTS(SELECT PART FROM #TmpAnnexure4  WHERE PART=5 and PARTSR = '5C')
BEGIN
	Insert Into #TmpAnnexure4 (PART,PARTSR,ii_gstin,ii_state,ii_inv_no,iichallanno,iichallandt,iidate,ii_it_desc,ii_itname,
				   ii_p_unit,ii_qty,ii_taxamt,type,cgst_amt,sgst_amt,igst_amt,cess_amt,cgst_per,sgst_per,igst_per,cess_per,
				   ir_gstin,ir_state,irname,ir_invno,ir_date,chno_jw,chdt_jw,chgstin_jw,chstate_jw,invno_sjw,date_sjw,ir_It_Desc,ir_IT_NAME,
				   ir_p_unit,ir_qty,ir_taxamt,ir_wastage)
	VALUES(5,'5C','','','','','','','','','',0,0,'',0,0,0,0,0,0,0,0,'','','','','','','','','','','','','','',0,0,0)
END			   
					   
select * from #TmpAnnexure4

 
drop table #TmpAnnexure4 

--EXECUTE USP_REP_ITC_04 '','','','04/01/2018','04/30/2019','','','','',0,0,'','','','','','','','','2018-2019',''