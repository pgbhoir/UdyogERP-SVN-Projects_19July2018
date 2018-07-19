DROP PROCEDURE [USP_REP_DNREG_SERVICE]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	Prajakta B.
-- Create date: 27/03/2017
-- Description:	This Strored procedure is useful for Debit Note Service Register
-- Remark:
-- =============================================

Create PROCEDURE [USP_REP_DNREG_SERVICE]
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

Declare @FCON as NVARCHAR(4000),@SQLCOMMAND as NVARCHAR(4000)

EXECUTE USP_REP_FILTCON 
		@VTMPAC=@TMPAC,@VTMPIT=@TMPIT,@VSPLCOND=@SPLCOND, 
		@VSDATE=@SDATE,@VEDATE=@EDATE,    --Added @sdate para. by sandeep for bug-18638 on 26-Aug-13
		@VSAC =@SNAME,@VEAC =@ENAME, --Added @SNAME & @ENAME para. by sandeep for bug-18638 on 26-Aug-13  
		@VSIT=@SITEM,@VEIT=@EITEM,
		@VSAMT=@SAMT,@VEAMT=@EAMT, --Added @SAMT & @EAMT para. by sandeep for bug-18638 on 26-Aug-13  
		@VSDEPT=@SDEPT,@VEDEPT=@EDEPT,
		@VSCATE =@SCAT,@VECATE =@ECAT,
		@VSWARE =@SWARE,@VEWARE  =@EWARE,
		@VSINV_SR =@SINVSR,@VEINV_SR =@EINVSR,
		@VMAINFILE='DNMAIN',@VITFILE=null,@VACFILE=null,
		@VDTFLD = 'DATE',@VLYN=null,@VEXPARA=@expara,
		@VFCON =@FCON OUTPUT

	declare @RuleCondition varchar(1000)
	set @RuleCondition=''
	if charindex('$>Rule',@expara)<>0
	begin
		set @RuleCondition=@expara
		SET @RuleCondition=REPLACE(@RuleCondition, '`','''')
		set @RuleCondition=substring(@RuleCondition,charindex('$>Rule',@RuleCondition)+6,len(@RuleCondition)-(charindex('$>Rule',@RuleCondition)+5))
		set @RuleCondition=substring(@RuleCondition,1,charindex('<$Rule',@RuleCondition)-1)
	end

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'SELECT DNITEM.[DATE],DNITEM.inv_no,DNITEM.ITEM,DNITEM.qty,DNITEM.rate,DNITEM.U_ASSEAMT,DNITEM.tot_add,DNITEM.tot_deduc,DNITEM.tot_tax,DNITEM.tot_examt,DNITEM.tot_nontax,DNITEM.tot_fdisc
	,DNITEM.SBILLNO,DNITEM.SBDATE,DNITEM.CGST_AMT,DNITEM.SGST_AMT,DNITEM.IGST_AMT,DNITEM.GRO_AMT,DNITEM.PARTY_NM,DNMAIN.AGAINSTGS,DNITEM.cgst_per,DNITEM.sgst_per,DNITEM.igst_per,acdetalloc.amount 
	,dnitem.cgsrt_amt,dnitem.sgsrt_amt,dnitem.igsrt_amt FROM DNMAIN
	LEFT JOIN DNITEM ON DNMAIN.TRAN_CD=DNITEM.TRAN_CD
	inner join AcdetAlloc on (DNITEM.entry_ty=AcdetAlloc.Entry_ty and DNITEM.Tran_cd=AcdetAlloc.Tran_cd and DNITEM.itserial=AcdetAlloc.itserial)
	INNER JOIN AC_MAST ON DNMAIN.AC_ID=AC_MAST.AC_ID'
	SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)+' '+@RuleCondition +'AND DNITEM.entry_ty=''D6'' '
	SET @SQLCOMMAND =RTRIM(@SQLCOMMAND)+' ORDER BY DNITEM.DATE,DNITEM.INV_NO'
	PRINT @SQLCOMMAND
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
GO
