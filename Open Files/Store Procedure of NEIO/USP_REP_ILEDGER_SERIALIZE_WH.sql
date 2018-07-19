DROP PROCEDURE [USP_REP_ILEDGER_SERIALIZE_WH]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author	  :	Sachin N. S.
-- Create date: 27/04/2012
-- Description:	This Stored procedure is useful for Stock ledger Report of Split Quantity Warehouse wise.
-- Modify By :	Archana K.
--Modify Date : 29/04/13 for Bug-13141(Serial No. Selection Require).
-- Remark	  :
-- =============================================

CREATE PROCEDURE [USP_REP_ILEDGER_SERIALIZE_WH]
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
	Declare @OPENTRIES as VARCHAR(50),@OPENTRY_TY as VARCHAR(50)
	Declare @TBLNM as VARCHAR(50),@TBLNAME1 as VARCHAR(50),@TBLNAME2 as VARCHAR(50)
	
	Set @OPENTRY_TY = '''OS'''
	Set @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
					+ (DATEPART(ss, GETDATE()) * 1000 )
					+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
	Set @TBLNAME1 = '##TMP1'+@TBLNM
	Set @TBLNAME2 = '##TMP2'+@TBLNM
	
	DECLARE openingentry_cursor CURSOR FOR
		SELECT entry_ty FROM lcode
		WHERE bcode_nm = 'OS'
	OPEN openingentry_cursor
	FETCH NEXT FROM openingentry_cursor into @opentries
	WHILE @@FETCH_STATUS = 0
	BEGIN
	   Set @OPENTRY_TY = @OPENTRY_TY +','''+@opentries+''''
	   FETCH NEXT FROM openingentry_cursor into @opentries
	END
	CLOSE openingentry_cursor
	DEALLOCATE openingentry_cursor

	EXECUTE USP_REP_FILTCON 
		@VTMPAC=null,@VTMPIT=@TMPIT,@VSPLCOND=@SPLCOND,
		@VSDATE=null,@VEDATE=@EDATE,
		@VSAC =null,@VEAC =null,
		@VSIT=@SITEM,@VEIT=@EITEM,
		@VSAMT=null,@VEAMT=null,
		@VSDEPT=@SDEPT,@VEDEPT=@EDEPT,
		@VSCATE =@SCAT,@VECATE =@ECAT,
		@VSWARE =@SWARE,@VEWARE  =@EWARE,
		@VSINV_SR =@SINVSR,@VEINV_SR =@EINVSR,
		@VMAINFILE='MVW',@VITFILE='IVW',@VACFILE=null,
		@VDTFLD = 'DATE',@VLYN=null,@VEXPARA=@expara,
		@VFCON =@FCON OUTPUT

		/*Archana 27/04/2013 Bug-13141--->*/
			declare @RuleCondition varchar(1000)
			set @RuleCondition=''
			
			if charindex('$>Rule',@expara)<>0
			begin
				set @RuleCondition=@expara
				SET @RuleCondition=REPLACE(@RuleCondition, '`','''')
				set @RuleCondition=substring(@RuleCondition,charindex('$>Rule',@RuleCondition)+6,len(@RuleCondition)-(charindex('$>Rule',@RuleCondition)+5))
				set @RuleCondition=substring(@RuleCondition,1,charindex('<$Rule',@RuleCondition)-1)
			end

	/*Archana 27/04/2013 Bug-13141--->*/

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'SELECT IVW.TRAN_CD,IVW.ENTRY_TY,IVW.DATE,IVW.ITSERIAL,
		IVW.QTY,IVW.DC_NO,IVW.WARE_NM,
		MVW.INV_NO,LCODE.INV_STK,
		AC_MAST.AC_ID,AC_MAST.AC_NAME,
		IT_MAST.IT_CODE,IT_MAST.IT_NAME,IT_MAST.RATEUNIT, 
		SRW.RENTRY_TY, SRW.RTRAN_CD, SRW.RITSERIAL, SRW.RQTY, SRW.ITRAN_CD, SRW.WARE_NMFR	
		INTO '+@TBLNAME1+' FROM STKL_VW_ITEM IVW (NOLOCK)
		INNER JOIN AC_MAST (NOLOCK) ON IVW.AC_ID = AC_MAST.AC_ID
		INNER JOIN IT_MAST (NOLOCK) ON IVW.IT_CODE = IT_MAST.IT_CODE
		INNER JOIN STKL_VW_MAIN MVW (NOLOCK) 
			ON IVW.TRAN_CD = MVW.TRAN_CD AND IVW.ENTRY_TY = MVW.ENTRY_TY
		INNER JOIN IT_SRTRN SRW ON IVW.ENTRY_TY=SRW.ENTRY_TY AND IVW.TRAN_CD=SRW.TRAN_CD AND IVW.ITSERIAL=SRW.ITSERIAL
		INNER JOIN LCODE (NOLOCK) 
			ON IVW.ENTRY_TY = LCODE.ENTRY_TY AND LCODE.INV_STK IN (''+'',''-'')'+RTRIM(@FCON)
		--SRW.RENTRY_TY, SRW.RTRAN_CD, SRW.RITSERIAL, SRW.RQTY, SRW.ITRAN_CD, SRW.WARE_NMFR		-- Changed By Sachin N. S. on 27/03/2012 for Bug-3196 Version 1.1
		--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND IT_MAST.NONSTK='+CHAR(39)+'Stockable'+CHAR(39) check it from Special Condition
		SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND MVW.APGEN='+CHAR(39)+'YES'+CHAR(39)
		SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND IVW.DC_NO='+CHAR(39)+' '+CHAR(39)
		--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+@RuleCondition /*Rup 09/06/2010 TKT-1690*/

	PRINT @SQLCOMMAND
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'SELECT TRAN_CD=0,ENTRY_TY='' '',
		DATE=CONVERT(SMALLDATETIME,'''+CONVERT(VARCHAR(50),@SDATE)+'''),
		QTY=IsNull(sum(CASE WHEN TVW.INV_STK = ''+'' AND IsNull(TVW.DC_NO,'' '') = '' '' THEN TVW.RQTY END),0)
		   -IsNull(sum(CASE WHEN TVW.INV_STK = ''-'' AND IsNull(TVW.DC_NO,'' '') = '' '' THEN TVW.RQTY END),0),
		ITSERIAL='' '',TVW.WARE_NM,INV_NO='' '',AC_ID=0,AC_NAME=''Balance B/f'',INV_STK='' '',
		TVW.IT_CODE,TVW.IT_NAME,TVW.RATEUNIT,TVW.ITRAN_CD, WARE_NMFR='' ''
		INTO '+@TBLNAME2+' FROM '+@TBLNAME1+' TVW
		WHERE (TVW.DATE < '''+CONVERT(VARCHAR(50),@SDATE)+''' OR TVW.ENTRY_TY IN ('+@OPENTRY_TY+')) 
		GROUP BY TVW.IT_CODE,TVW.IT_NAME,TVW.RATEUNIT,TVW.ITRAN_CD, TVW.WARE_NM
		UNION ALL
		SELECT TVW.TRAN_CD,TVW.ENTRY_TY,TVW.DATE,
		QTY=IsNull(CASE WHEN TVW.INV_STK = ''+'' AND IsNull(TVW.DC_NO,'' '') = '' '' THEN TVW.RQTY END,0)
		   -IsNull(CASE WHEN TVW.INV_STK = ''-'' AND IsNull(TVW.DC_NO,'' '') = '' '' THEN TVW.RQTY END,0),
		TVW.ITSERIAL,TVW.WARE_NM,TVW.INV_NO,TVW.AC_ID,TVW.AC_NAME,TVW.INV_STK,
		TVW.IT_CODE,TVW.IT_NAME,TVW.RATEUNIT,TVW.ITRAN_CD, TVW.WARE_NMFR
		FROM '+@TBLNAME1+' TVW
		WHERE (TVW.DATE BETWEEN '''+CONVERT(VARCHAR(50),@SDATE)+''' AND '''+CONVERT(VARCHAR(50),@EDATE)+''' AND 
			TVW.ENTRY_TY NOT IN ('+@OPENTRY_TY+'))'
--		TVW.IT_CODE,TVW.IT_NAME,TVW.RATEUNIT,TVW.ITRAN_CD	-- Changed By Sachin N. S. on 27/03/2012 for Bug-3196 Version 1.1
--		TVW.IT_CODE,TVW.IT_NAME,TVW.RATEUNIT,TVW.ITRAN_CD	-- Changed By Sachin N. S. on 27/03/2012 for Bug-3196 Version 1.1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'SELECT TVW.*, STW.* FROM '+@TBLNAME2+' TVW
		INNER JOIN IT_SRSTK STW ON TVW.ITRAN_CD=STW.ITRAN_CD
		WHERE TVW.QTY <> 0 '+@expara+--added filter condition by Archana K. on 27/04/13 for Bug-13141
		'ORDER BY TVW.IT_NAME,TVW.ITRAN_CD,TVW.WARE_NM,TVW.DATE,TVW.INV_STK,TVW.INV_NO,TVW.ITSERIAL'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME2
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
GO
