IF EXISTS(SELECT [NAME] FROM SYS.PROCEDURES WHERE [NAME]='Usp_Final_Accounts_Schedule')
BEGIN
	DROP PROCEDURE Usp_Final_Accounts_Schedule
END
/****** Object:  StoredProcedure [dbo].[Usp_Final_Accounts]    Script Date: 03/10/2018 15:43:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Usp_Final_Accounts_Schedule]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)

As
Declare @FDate as smalldatetime, @TDate as smalldatetime, @C_St_Date as smalldatetime
Set @FDate=@SDATE
set @TDate =@EDATE
set @C_St_Date=@EDATE

If @FDate IS NULL OR @TDate IS NULL OR @C_St_Date IS NULL
Begin
	RAISERROR ('Please pass valid parameters..',16,1)
	Return 
End
If @FDate = '' OR @TDate = '' OR @C_St_Date = ''
Begin
	RAISERROR ('Please pass valid parameters..',16,1)
	Return 
End

if (@SDEPT is null OR @EDEPT IS NULL OR @SCATE IS NULL OR @ECATE IS NULL) /*TKT-1129*/
Begin
	RAISERROR ('Please pass valid parameters..',16,1)
	Return 
End

-- Added by Sachin N. S. on 04/03/2020 for Bug-32672 -- Start
DECLARE @COND NVARCHAR(500), @SCHDCOND1 NVARCHAR(20),@SCHDCOND2 NVARCHAR(20),@SCHDCOND3 NVARCHAR(20),@TMPCOSTCENT NVARCHAR(200), 
	@COMPID INT
--SET @SCHDCOND1 = SUBSTRING(@EXPARA,',','')

SELECT @COND=@EXPARA

SELECT @SCHDCOND1=SUBSTRING(@COND,1,CHARINDEX(':',@COND)-1)
SELECT @COND=REPLACE(@COND,@SCHDCOND1+':','')
SELECT @SCHDCOND1=SUBSTRING(@SCHDCOND1,CHARINDEX('=',@SCHDCOND1)+1,LEN(@SCHDCOND1)-CHARINDEX('=',@SCHDCOND1))
SELECT @TMPCOSTCENT=@SCHDCOND1

SELECT @SCHDCOND2=SUBSTRING(@COND,1,CHARINDEX(':',@COND)-1)
SELECT @COND=REPLACE(@COND,@SCHDCOND2+':','')
SELECT @SCHDCOND2=SUBSTRING(@SCHDCOND2,CHARINDEX('=',@SCHDCOND2)+1,LEN(@SCHDCOND2)-CHARINDEX('=',@SCHDCOND2))

SELECT @SCHDCOND3=@COND
SELECT @SCHDCOND3=SUBSTRING(@SCHDCOND3,CHARINDEX('=',@SCHDCOND3)+1,LEN(@SCHDCOND3)-CHARINDEX('=',@SCHDCOND3))
SELECT @COMPID=CAST(@SCHDCOND3 AS INT)


--SELECT @SCHDCOND1=SUBSTRING(@EXPARA,1,CHARINDEX(':',@EXPARA,1)-1)
--SELECT @SCHDCOND2=SUBSTRING(@EXPARA,CHARINDEX(':',@EXPARA,1)+1,CHARINDEX(':',@EXPARA,2)-CHARINDEX(':',@EXPARA,1)-1)
--SELECT @SCHDCOND3=SUBSTRING(@EXPARA,CHARINDEX(':',@EXPARA,2)+1,LEN(@EXPARA)-CHARINDEX(':',@EXPARA,2))
--SELECT @SCHDCOND2=SUBSTRING(@SCHDCOND2,CHARINDEX('=',@SCHDCOND2)+1,LEN(@SCHDCOND2)-CHARINDEX('=',@SCHDCOND2))
--SELECT @COMPID=CAST(SUBSTRING(@SCHDCOND3,CHARINDEX('=',@SCHDCOND3)+1,LEN(@SCHDCOND3)-CHARINDEX('=',@SCHDCOND3)) AS INT)
--SELECT @TMPCOSTCENT=SUBSTRING(@SCHDCOND1,CHARINDEX('=',@SCHDCOND1)+1,LEN(@SCHDCOND1)-CHARINDEX('=',@SCHDCOND1))
-- Added by Sachin N. S. on 04/03/2020 for Bug-32672 -- End

/* Internale Variable declaration and Assigning [Start] */
Declare @Balance Numeric(17,2),@TBLNM VARCHAR(50),@TBLNAME1 Varchar(50),
	@TBLNAME2 Varchar(50),@TBLNAME3 Varchar(50),@TBLNAME4 Varchar(50),
	@SQLCOMMAND as NVARCHAR(4000),@TIME SMALLDATETIME

Declare @TBLNAME5 Varchar(50)
Declare @TBLNAME6 Varchar(50)
Declare @TBLNAME7 Varchar(50)
Declare @TBLNAME8 Varchar(50)
Declare @TBLNAME9 Varchar(50)		-- Added by Sachin N. S. on 05/03/2020 for Bug-32627 

Select @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
		+ (DATEPART(ss, GETDATE()) * 1000 )+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No),
		@Balance = 0,@SQLCOMMAND = ''

Select @TBLNAME1 = '##TMP1'+@TBLNM,@TBLNAME2 = '##TMP2'+@TBLNM
Select @TBLNAME3 = '##TMP3'+@TBLNM,@TBLNAME4 = '##TMP4'+@TBLNM
Select @TBLNAME5 = '##TMP5'+@TBLNM
Select @TBLNAME6 = '##TMP6'+@TBLNM
Select @TBLNAME7 = '##TMP7'+@TBLNM
Select @TBLNAME8 = '##TMP8'+@TBLNM
Select @TBLNAME9 = '##TMP9'+@TBLNM		-- Added by Sachin N. S. on 05/03/2020 for Bug-32627 
/* Internale Variable declaration and Assigning [End] */

-- Changed by Sachin N. S. on 13/05/2020 for Bug-33477 -- Start
--Select * into ##STKVALConfig from StkValConfig		--Bug20309
select [BalPTBook],[BalSTBook],[PalPTBook],[PalSTBook],[OP_AcName],[ClP_AcName],[ClB_AcName],[TOp_AcName],[TClP_AcName],[TClB_AcName]
 into ##STKVALConfig from Vudyog..co_mast where Compid=@CompID	
 -- Changed by Sachin N. S. on 13/05/2020 for Bug-33477 -- End

/* Collecting Data from accounts details and create table [Start] */
SET @SQLCOMMAND = 'SELECT AVW.TRAN_CD,AVW.ENTRY_TY,AVW.DATE,AVW.AMOUNT,AVW.AMT_TY,
		MVW.INV_NO,AC_MAST.AC_ID,AC_MAST.[TYPE],AC_MAST.AC_NAME,AVW.ACSERIAL
		INTO '+@TBLNAME1+' FROM LAC_VW AVW (NOLOCK)
		INNER JOIN AC_MAST (NOLOCK) ON AVW.AC_ID = AC_MAST.AC_ID
		INNER JOIN LMAIN_VW MVW (NOLOCK) 
		ON AVW.TRAN_CD = MVW.TRAN_CD AND AVW.ENTRY_TY = MVW.ENTRY_TY'
		--WHERE (MVW.DATE < = '''+CONVERT(VARCHAR(50),@TDate)+''') '		--Commented by Shrikant S. on 29/11/2017 for Bug-30928
		
--Added by Shrikant S. on 30/11/2017 for Bug-30928		--Start		
if @TMPCOSTCENT<>''
Begin
	SET @SQLCOMMAND =@SQLCOMMAND+' '+'LEFT JOIN MAINCALL_VW CVW(NOLOCK) ON (CVW.ENTRY_TY=AVW.ENTRY_TY AND CVW.TRAN_CD=AVW.TRAN_CD AND CVW.ACSERIAL=AVW.ACSERIAL AND CVW.AC_ID=AVW.AC_ID)'
	SET @SQLCOMMAND =@SQLCOMMAND+' '+'INNER JOIN '+@TMPCOSTCENT+' CC ON (CC.COST_CEN_NAME=CVW.COST_CEN_NAME)'
End
SET @SQLCOMMAND =@SQLCOMMAND+' '+'WHERE (MVW.DATE < = '''+CONVERT(VARCHAR(50),@TDate)+''') '				
--Added by Shrikant S. on 30/11/2017 for Bug-30928		--End

If (@EDEPT<>'')
Begin
	SET @SQLCOMMAND =rtrim(@SQLCOMMAND)+' '+' and ('+'MVW.DEPT between '''+@SDEPT+''' and '''+@EDEPT+''')'
End

If (@ECATE<>'')
Begin
	SET @SQLCOMMAND =rtrim(@SQLCOMMAND)+' '+' and ('+'MVW.Cate between '''+@SCATE+''' and '''+@ECATE+''')'
End

EXECUTE sp_executesql @SQLCOMMAND
/* Collecting Data from accounts details and create table [End] */

print 'a1'

Declare @Stk_OpAccounts Varchar(100),@Stk_ClAccounts Varchar(100)  
Set @Stk_ClAccounts = ''
DECLARE CSTKVAL CURSOR FOR 
SELECT ClB_AcName FROM ##STKVALConfig
OPEN CSTKVAL
FETCH NEXT FROM CSTKVAL INTO @Stk_ClAccounts
WHILE @@FETCH_STATUS=0
BEGIN

	SET @SQLCOMMAND = 'DELETE FROM '+@TBLNAME1+' WHERE AC_NAME = '''+@Stk_ClAccounts+''' AND [DATE] < '''+CONVERT(VARCHAR(50),@C_St_Date-1)+''' '	
	EXECUTE sp_executesql @SQLCOMMAND 

	FETCH NEXT FROM CSTKVAL INTO @Stk_ClAccounts
END
CLOSE CSTKVAL
DEALLOCATE CSTKVAL

print 'a2'

/*Remove Trading and Profit loss Previous Entry [Start]*/
SET @SQLCOMMAND = 'DELETE FROM '+@TBLNAME1+' WHERE CONVERT(VARCHAR(20),TRAN_CD)+''-''+ENTRY_TY IN 
	(SELECT CONVERT(VARCHAR(20),TRAN_CD)+''-''+ENTRY_TY AS COMEID FROM '+@TBLNAME1+' WHERE [TYPE] IN (''T'',''P'') 
	AND [DATE] NOT BETWEEN '''+CONVERT(VARCHAR(50),@FDate)+''' AND '''+CONVERT(VARCHAR(50),@TDate)+''') AND [TYPE] IN (''T'',''P'') '
EXECUTE sp_executesql @SQLCOMMAND
/*Remove Trading and Profit loss Previous Entry [End]*/

print 'a3'

-- Changed by Sachin N. S. on 05/03/2020 for Bug-32672 -- Start
SET @SQLCOMMAND = 'SELECT TRAN_CD,AC_NAME,DATE INTO '+@TBLNAME9+' FROM '+@TBLNAME1+' WHERE 
			ENTRY_TY IN (''OB'') '
EXECUTE sp_executesql @SQLCOMMAND			

SET @SQLCOMMAND = 'DELETE A FROM '+@TBLNAME1+' A 
						INNER JOIN '+@TBLNAME9+' B ON A.AC_NAME=B.AC_NAME 
					WHERE A.DATE<B.DATE '
EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME9
EXECUTE sp_executesql @SQLCOMMAND

--SET @SQLCOMMAND = 'DECLARE @OPTRAN_CD as INT,@OPDATE as DATETIME,@OPACNAME as varchar(250), @NCNT INT
--	SET @NCNT=1
--	DECLARE openingentry_cursor CURSOR FOR
--		SELECT TRAN_CD,AC_NAME,DATE FROM '+@TBLNAME1+' WHERE 
--			ENTRY_TY IN (''OB'') 
--	OPEN openingentry_cursor
--	FETCH NEXT FROM openingentry_cursor into @OPTRAN_CD,@OPACNAME,@OPDATE
--	WHILE @@FETCH_STATUS = 0
--	BEGIN
--		PRINT @NCNT
--		PRINT @OPACNAME
--	   DELETE FROM '+@TBLNAME1+' WHERE DATE < @OPDATE		--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).
--			AND AC_NAME IN (SELECT AC_NAME FROM '+@TBLNAME1+
--			' WHERE AC_NAME = @OPACNAME AND ENTRY_TY IN (''OB'') AND TRAN_CD = @OPTRAN_CD )		--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).
--			SET @NCNT=@NCNT+1
--	   FETCH NEXT FROM openingentry_cursor into @OPTRAN_CD,@OPACNAME,@OPDATE
--	END
--CLOSE openingentry_cursor
--DEALLOCATE openingentry_cursor'
--EXECUTE SP_EXECUTESQL @SQLCOMMAND

-- Changed by Sachin N. S. on 05/03/2020 for Bug-32672 -- End 
 
print 'a4' 
	
SET @SQLCOMMAND = 'IF EXISTS(SELECT TOP 1 A.DATE FROM LITEM_VW A,LCODE B,LMAIN_VW C WHERE A.ENTRY_TY = B.ENTRY_TY AND A.ENTRY_TY = C.ENTRY_TY AND A.TRAN_CD = C.TRAN_CD AND A.DATE < '''+CONVERT(VARCHAR(50),@C_St_Date)+''' AND B.INV_STK<>'' '' AND A.DC_NO='''' AND C.[RULE] NOT IN (''EXCISE'',''NON-EXCISE''))  '		--Changes done by vasant on 05/05/2012 as per Bug 3450 (Balance sheet report Problem).	--Added By Shrikant S. on 10/06/2013 for Bug-548
SET @SQLCOMMAND = @SQLCOMMAND + ' DELETE FROM '+@TBLNAME1+' WHERE ENTRY_TY IN (SELECT (CASE WHEN EXT_VOU=1 THEN BCODE_NM ELSE ENTRY_TY END) AS BHENT FROM LCODE WHERE ENTRY_TY = ''OS'' OR BCODE_NM = ''OS'') '
EXECUTE sp_executesql @SQLCOMMAND

print 'a5'

Set @Stk_OpAccounts = ''
DECLARE CSTKVAL CURSOR FOR 
SELECT Op_AcName FROM ##STKVALConfig
OPEN CSTKVAL
FETCH NEXT FROM CSTKVAL INTO @Stk_OpAccounts
WHILE @@FETCH_STATUS=0
BEGIN

	SET @SQLCOMMAND = 'IF EXISTS(SELECT TOP 1 A.DATE FROM ARMAIN A WHERE A.[RULE] IN (''EXCISE'',''NON-EXCISE'') AND A.DATE < '''+CONVERT(VARCHAR(50),@C_St_Date)+''')' 
	SET @SQLCOMMAND = @SQLCOMMAND + 'Update '+@TBLNAME1+' Set Amount = 0 Where AC_NAME in ('''+@Stk_OpAccounts+''') '	
	EXECUTE sp_executesql @SQLCOMMAND

	FETCH NEXT FROM CSTKVAL INTO @Stk_OpAccounts
END
CLOSE CSTKVAL
DEALLOCATE CSTKVAL


print 'a6'

Set @Stk_OpAccounts = ''
Set @Stk_ClAccounts = ''
DECLARE CSTKVAL CURSOR FOR 
SELECT Op_AcName,ClB_AcName FROM ##STKVALConfig
OPEN CSTKVAL
FETCH NEXT FROM CSTKVAL INTO @Stk_OpAccounts,@Stk_ClAccounts
WHILE @@FETCH_STATUS=0
BEGIN

	SET @SQLCOMMAND = 'UPDATE '+@TBLNAME1+' SET AC_NAME = '''+@Stk_OpAccounts+''', 
		AC_ID=(SELECT AC_ID FROM AC_MAST WHERE AC_NAME = '''+@Stk_OpAccounts+''') 
		WHERE AC_NAME = '''+@Stk_ClAccounts+''' AND [DATE] = '''+CONVERT(VARCHAR(50),@C_St_Date-1)+''' '	
	EXECUTE sp_executesql @SQLCOMMAND 

	FETCH NEXT FROM CSTKVAL INTO @Stk_OpAccounts,@Stk_ClAccounts
END
CLOSE CSTKVAL
DEALLOCATE CSTKVAL

print 'a7'

Drop table ##STKVALConfig

SET @SQLCOMMAND = 'SELECT TRAN_CD=0,ENTRY_TY='' '',
	DATE = '''+CONVERT(VARCHAR(50),@FDate)+''',
	AMOUNT=ISNULL(SUM(CASE WHEN TVW.AMT_TY = ''DR'' THEN TVW.AMOUNT ELSE - TVW.AMOUNT END),0),
	TVW.AC_ID,TVW.AC_NAME,TVW.ACSERIAL,AMT_TY=''A'',INV_NO='' ''
	INTO '+@TBLNAME2+' FROM '+@TBLNAME1+' TVW
	WHERE (TVW.DATE < '''+CONVERT(VARCHAR(50),@FDate)+'''
	OR TVW.ENTRY_TY IN (Select Entry_Ty From LCode Where bCode_Nm = ''OB'' OR Entry_Ty = ''OB'' OR bCode_Nm = ''OS'' OR Entry_Ty = ''OS'')) 
	GROUP BY TVW.AC_ID,TVW.AC_NAME,TVW.ACSERIAL
	UNION
SELECT TVW.TRAN_CD,TVW.ENTRY_TY,TVW.DATE,
	AMOUNT=(CASE WHEN TVW.AMT_TY=''DR'' THEN TVW.AMOUNT ELSE -TVW.AMOUNT END),
	TVW.AC_ID,TVW.AC_NAME,TVW.ACSERIAL,TVW.AMT_TY,TVW.INV_NO
	FROM '+@TBLNAME1+' TVW
	LEFT JOIN LAC_VW LVW (NOLOCK) 
	ON TVW.TRAN_CD = LVW.TRAN_CD AND TVW.ENTRY_TY = LVW.ENTRY_TY AND TVW.AC_ID != LVW.AC_ID
	WHERE (TVW.DATE BETWEEN '''+CONVERT(VARCHAR(50),@FDate)+''' AND '''+CONVERT(VARCHAR(50),@TDate)+''' AND 
	TVW.ENTRY_TY NOT IN (Select Entry_Ty From LCode Where bCode_Nm = ''OB'' OR Entry_Ty = ''OB'' OR bCode_Nm = ''OS'' OR Entry_Ty = ''OS''))'
EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 'SELECT a.Ac_id,
	Opening = isnull(CASE Amt_Ty WHEN ''A'' THEN SUM(a.Amount)END,0),
	Debit = isnull(CASE Amt_Ty WHEN ''DR'' THEN SUM(a.Amount)END,0),
	Credit = isnull(CASE Amt_Ty WHEN ''CR'' THEN SUM(a.Amount) END,0)
	Into '+@TBLNAME3+' from '+@TBLNAME2+' a
	group by a.Ac_id,a.amt_ty'
EXECUTE sp_executesql @SQLCOMMAND


print 'a8'

SET @SQLCOMMAND = 'SELECT b.Ac_id,Sum(IsNull(a.Opening,0)) as OpBal,Sum(IsNull(a.Debit,0)) as Debit,
	Sum(IsNull(a.Credit,0)) as Credit,CAST(0 AS Numeric(17,2)) As ClBal
	Into '+@TBLNAME4+' from '+@TBLNAME3+' a Right Join Ac_Mast b 
	ON (b.Ac_id = a.Ac_id) group by b.Ac_id'
EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 'Update '+@TBLNAME4+' SET OPbal = (CASE WHEN OpBal IS NULL THEN 0 ELSE OPBAL END),
	Debit = (CASE WHEN Debit IS NULL THEN 0 ELSE Debit END),
	Credit = (CASE WHEN Credit IS NULL THEN 0 ELSE Credit END),
	Clbal = (CASE WHEN Clbal IS NULL THEN 0 ELSE Clbal END)'
EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 'UPDATE '+@TBLNAME4+' SET ClBal = (OpBal+Debit-ABS(Credit)) '
EXECUTE sp_executesql @SQLCOMMAND 

print 'a9'

/* Combined Groups And Ledgers with Opening,Debit,Credit[Start] */
SET @SQLCOMMAND = 'Select Updown,''G'' As MainFlg,Ac_Group_Id as Ac_Id,gAC_id as Ac_Group_Id,
	AC_GROUP_NAME+space(100) as Ac_Name,[Group], CAST(0 AS Numeric(17,2)) As OpBal,CAST(0 AS Numeric(17,2)) As Debit,
	CAST(0 AS Numeric(17,2)) As Credit,CAST(0 AS Numeric(17,2)) As ClBal
		INTO '+@TBLNAME5+' From Ac_Group_Mast
Union All 
Select Updown,''L'' As MainFlg,b.Ac_Id,b.Ac_Group_Id,b.Ac_Name+space(100), b.[Group],
	a.OpBal,a.Debit,ABS(a.Credit),a.ClBal
	From '+@TBLNAME4+' a Right Join Ac_Mast b ON (b.Ac_id = a.Ac_id)'
EXECUTE sp_executesql @SQLCOMMAND
/* Combined Groups And Ledgers [End] */

print 'a10'

/* Updating the Alternate group in case of Negative Balance Sheet[Start] */	
If Exists(Select b.[name] From sysobjects a inner join syscolumns b on (a.id=b.id) where a.[name]='ac_mast' and b.[name]='agrp_id')
Begin
	Select Ac_id,aGrp_Id Into #pGrpid from Ac_mast Where isnull(AGRP_ID,0)<>0
	SELECT AC_ID,ACTYPE=space(1) INTO #ACMAST FROM PTMAIN WHERE  1=2
	
	DECLARE @MCOND AS BIT,@LVL  AS INT
	
	CREATE TABLE #ACGRPID (GACID DECIMAL(9),LVL DECIMAL(9))
	SET @LVL=0
	
	INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL FROM AC_GROUP_MAST WHERE AC_GROUP_NAME ='LIABILITIES'
	SET @MCOND=1
	WHILE @MCOND=1
	BEGIN
		IF EXISTS (SELECT AC_GROUP_ID FROM AC_GROUP_MAST WHERE AC_GROUP_ID!=GAC_ID AND GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)) --WHERE LVL=@LVL
		BEGIN
			INSERT INTO #ACGRPID 
				SELECT AC_GROUP_ID,@LVL+1 FROM AC_GROUP_MAST WHERE AC_GROUP_ID!=GAC_ID AND 
					GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)
			SET @LVL=@LVL+1
		END
		ELSE
		BEGIN
			SET @MCOND=0	
		END
	END
	INSERT INTO #ACMAST SELECT AC_ID,'L' FROM AC_MAST WHERE  AC_GROUP_ID IN (SELECT DISTINCT GACID FROM #ACGRPID)
	DELETE FROM #ACGRPID
	

	INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL  FROM AC_GROUP_MAST WHERE AC_GROUP_NAME ='ASSETS'
	SET @LVL=0
	SET @MCOND=1
	WHILE @MCOND=1
	BEGIN
		IF EXISTS (SELECT AC_GROUP_ID FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)) --WHERE LVL=@LVL
		BEGIN
			INSERT INTO #ACGRPID 
				SELECT AC_GROUP_ID,@LVL+1 FROM AC_GROUP_MAST 
					WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)
			SET @LVL=@LVL+1
		END
		ELSE
		BEGIN
			SET @MCOND=0	
		END
	END
	INSERT INTO #ACMAST SELECT AC_ID,'A' FROM AC_MAST WHERE  AC_GROUP_ID IN (SELECT DISTINCT GACID FROM #ACGRPID)
	DELETE FROM #ACGRPID

	SET @SQLCOMMAND = 'Update '	+@TBLNAME5+' set Ac_group_Id=b.aGrp_id From '+@TBLNAME5+' a '
	SET @SQLCOMMAND = @SQLCOMMAND+' '+'inner join #pGrpid b On (a.Ac_Id=b.Ac_Id) Left Join #ACMAST c on (a.ac_id=c.ac_id)'
	SET @SQLCOMMAND = @SQLCOMMAND+' '+'Where (c.AcType=''L'' and a.ClBal>0) Or ((c.AcType IS NULL OR c.AcType=''A'')AND a.ClBal<0)'
	EXECUTE sp_executesql @SQLCOMMAND
End
/* Updating the Alternate group in case of Negative Balance Sheet[End] */

print 'a11'

Declare @OpbalAmt Decimal(18,2),@ParmDefinition NVARCHAR(500)
set @OpbalAmt=0
SET @ParmDefinition=N'@parmOUT Decimal(18,2) Output'
SET @SQLCOMMAND = 'Select @parmOUT=Isnull(Clbal,0) from '+@TBLNAME5+' Where AC_NAME in (''OPENING BALANCES'')'		
EXECUTE sp_executesql @SQLCOMMAND,@ParmDefinition,@parmOUT=@OpbalAmt Output

SET @SQLCOMMAND = 'Update '+@TBLNAME5+' Set Opbal=0,Debit=0,Credit=0,ClBal=0 Where AC_NAME in (''OPENING BALANCES'')'		
EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 'Select *,'+convert(Varchar(20),@OpbalAmt)+' as OpBalAmt into '+@TBLNAME6+' From '+@TBLNAME5
EXECUTE sp_executesql @SQLCOMMAND

-- Get Scheduled wise Data -- Start

--SET @SQLCOMMAND = 'Select * from '+@TBLNAME6
--EXECUTE sp_executesql @SQLCOMMAND
--Print '1'

print 'a12'


SET @SQLCOMMAND = ';with cte_schedule as
(
	Select SchdId, cast(GrpOrAc as varchar(1)) as GrpOrAc, a.GrpOrAcId, B.Ac_group_id as ParId, 
		GrpOrAcNm=cast(GrpOrAcNm as varchar(50)) from SchdDtTbl a
		Inner join '+@TBLNAME6+' b on a.GrpOrAcId=b.Ac_id and CASE WHEN a.GrpOrAc=''G'' THEN ''G'' ELSE ''L'' END=b.MainFlg
			WHERE A.SCHDCODE=''III'' AND A.SCHDTYPE='''+@SCHDCOND2+'''		-- Added by Sachin N. S. on 05/03/2020 for Bug-32672
	Union all
	Select b.SchdId, ''G'', cast(a.Ac_id as int), a.Ac_group_id, cast(a.Ac_name as varchar(50)) from '+@TBLNAME6+' a
		Inner join cte_schedule b on a.Ac_id=b.ParId and a.MainFlg=''G'' and a.Ac_id<>b.GrpOrAcId
)
--Select Distinct * from cte_schedule '
--EXECUTE sp_executesql @SQLCOMMAND

--Select Distinct * from cte_schedule
--return 
--print 'a8'

--SET @SQLCOMMAND = ';with '
SET @SQLCOMMAND = @SQLCOMMAND  + '
,cte_schedule1 as
(
	Select a.SchdId, cast(a.GrpOrAc as varchar(1)) as GrpOrAc, a.GrpOrAcId, c.ParId as ParId, 
		GrpOrAcNm=cast(a.GrpOrAcNm as varchar(50)) 
	from schddttbl a
		Inner join '+@TBLNAME6+' b on a.GrpOrAcId=b.Ac_id and a.GrpOrAc=b.MainFlg
		Inner join cte_schedule c on a.SchdId = c.SchdId and a.GrpOrAcId=c.GrpOrAcId
		Where a.GrpOrAc=''G'' 
			and A.SCHDCODE=''III'' AND A.SCHDTYPE='''+@SCHDCOND2+'''		-- Added by Sachin N. S. on 05/03/2020 for Bug-32672
	Union all
	Select b.SchdId, a.MainFlg, 
		cast(a.Ac_id as Int), a.Ac_group_id, cast(a.Ac_name as varchar(50)) 
	from '+@TBLNAME6+' a
		Inner join cte_schedule1 b on a.Ac_GROUP_id=b.GrpOrAcId and b.GrpOrAc=''G''
)
,cte_schedule2 as 
(
	Select * From cte_schedule
	Union 
	Select * From cte_schedule1
)
Select * into '+@TBLNAME7+' from cte_schedule2 Order by SchdId '
EXECUTE sp_executesql @SQLCOMMAND

print 'a13'

--Print '2'
--return
--SET @SQLCOMMAND = 'Select * from '+@TBLNAME7
--EXECUTE sp_executesql @SQLCOMMAND


--SET @SQLCOMMAND = ';With cte_schedule (SchdId, SchdName, SchdParId, SerialNo, SchdNtNo, SchdType, OrderBy) as
--(
--	Select SchdId, SchdName, SchdParId, SerialNo, SchdNtNo, ''S'' as SchdType, cast(SerialNo as varchar(100)) as OrderBy 
--	From SchdHdTbl
--		Where SchdParId=0 and SchdCode=''III'' and SchdType=''BS''
--	union all
--	Select a.SchdId, a.SchdName, a.SchdParId, a.SerialNo, a.SchdNtNo, ''S'' as SchdType, cast(OrderBy+''/''+a.SerialNo as varchar(100)) 
--	From SchdHdTbl a
--		Inner join cte_schedule b on a.SchdParId=b.SchdId
--	Where a.SchdCode=''III'' and a.SchdType=''BS''
--),
--Cte_schedule1 (SchdId, GrporAcID, SchdName, SchdParId, SerialNo, SchdNtNo, SchdType, OrderBy) as 
--(
--	Select SchdId, 0, SchdName, SchdParId, SerialNo, SchdNtNo, SchdType, cast(OrderBy as varchar(100)) as OrderBy 
--	from cte_schedule 
--	union all
--	Select 0, a.GrpOrAcId, cast(GrpOrAcNm as varchar(200)), b.SchdId, cast(ROW_NUMBER() over(Order by a.SchdId) as varchar(5)), 
--		b.SchdNtNo, cast((case when GrpOrAc=''A'' then ''L'' else GrpOrAc end) as varchar(1)), CAST(OrderBy+''/''+cast(ROW_NUMBER() over(Order by a.SchdId) as varchar(5)) as varchar(100)) 
--	from SchdDtTbl a 
--		inner join Cte_schedule1 b on a.SchdId = b.SchdId
--)
--Select 00 as Updown,SchdType,GrporAcID,SchdId,SchdParId,SchdName,SerialNo, SchdNtNo, OrderBy, b.OpBal OpBal, b.ClBal as ClBal into '+@TBLNAME6+' from cte_schedule1 a
--	Left Outer join '+@TBLNAME5+' b on a.GrporAcID=b.Ac_id and a.SchdType=b.MainFlg
--Order by OrderBy '


--print 'a10'


SET @SQLCOMMAND = 
--;With cte_schedule (SchdId, SchdName, SchdParId, SerialNo, SchdNtNo, SchdType, OrderBy, OrderByPar, Level) as
';With cte_schedule (SchdId, SchdName, SchdParId, SerialNo, SchdNtNo, SchdType, OrderBy, OrderByPar, Level, Formula, SupressSum) as		-- Changed by Sachin N. S. on 06/03/2020 for Bug-32672
(
	Select SchdId, SchdName=cast(SchdName as Varchar(200)), SchdParId, SerialNo=cast(SerialNo as varchar(5)), 
		SchdNtNo, cast(''S'' as varchar(2)) as SchdType, 
		--cast(SerialNo as varchar(100)) as OrderBy, cast(SerialNo as varchar(100)) as OrderByPar,
		cast(SchdId as varchar(100)) as OrderBy, cast(SchdId as varchar(100)) as OrderByPar,		-- Changed by Sachin N. S. on 06/03/2020 for Bug-32672
		cast(1 as numeric(2)) Level, Cformula as Formula, SupressSum						-- Changed by Sachin N. S. on 06/03/2020 for Bug-32672
	From SchdHdTbl
		Where SchdParId=0 and SchdCode=''III'' and SchdType='''+@SCHDCOND2+'''
	union all
	Select a.SchdId, SchdName=cast(a.SchdName as Varchar(200)), a.SchdParId, SerialNo=cast(a.SerialNo as varchar(5)), 
		a.SchdNtNo, cast(''S'' as varchar(2)) as SchdType, 
		--cast(OrderBy+''/''+a.SerialNo as varchar(100)), cast(OrderBy+''/'' as varchar(100)) , 
		cast(OrderBy+''/''+cast(a.SchdId as varchar) as varchar(100)), cast(OrderBy+''/'' as varchar(100)) ,			-- Changed by Sachin N. S. on 06/03/2020 for Bug-32672
		cast(b.Level+1 as numeric(2)), a.Cformula as Formula, a.SupressSum						-- Changed by Sachin N. S. on 06/03/2020 for Bug-32672
	From SchdHdTbl a
		Inner join cte_schedule b on a.SchdParId=b.SchdId
	Where a.SchdCode=''III'' and a.SchdType='''+@SCHDCOND2+'''
),
--Cte_schedule1 (SchdId, GrporAcID, GrpParId, SchdName, SchdParId, SerialNo, SchdNtNo, SchdType, OrderBy, OrderByPar, Level) as 
Cte_schedule1 (SchdId, GrporAcID, GrpParId, SchdName, SchdParId, SerialNo, SchdNtNo, SchdType, OrderBy, OrderByPar, Level, Formula, SupressSum) as			-- Changed by Sachin N. S. on 06/03/2020 for Bug-32672
(
	Select a.SchdId, a.SchdId, a.SchdParId, a.SchdName, a.SchdParId, a.SerialNo, a.SchdNtNo, a.SchdType, 
		cast(a.OrderBy as varchar(100)) as OrderBy, cast(a.OrderByPar as varchar(100)) as OrderByPar, a.Level
		, cast(a.Formula as varchar(250)), cast(a.SupressSum as bit)		-- Added by Sachin N. S. on 06/03/2020 for Bug-32672  
	from cte_schedule a
	union all
	Select 0, a.GrpOrAcId, b.SchdId, cast(GrpOrAcNm as varchar(200)), b.SchdId, cast(ROW_NUMBER() over(Order by a.SchdId) as varchar(5)), 
		b.SchdNtNo, cast((case when GrpOrAc=''A'' then ''L'' else GrpOrAc end) as varchar(2)), CAST(OrderBy+''/''+cast(ROW_NUMBER() over(Order by a.SchdId) as varchar(5)) as varchar(100)) 
		, cast(b.OrderBy+''/'' as varchar(100)) as OrderByPar , b.Level 
		, cast('''' as varchar(250)) as Formula, cast(0 as bit) as SupressSum		-- Added by Sachin N. S. on 06/03/2020 for Bug-32672
	from '+@TBLNAME7+' a 
		inner join Cte_schedule1 b on a.SchdId = b.SchdId 
	Where a.ParId=a.GrporAcID
	union all
	Select 0, a.GrpOrAcId, b.GrpOrAcId, cast(GrpOrAcNm as varchar(200)), b.SchdParId, cast(ROW_NUMBER() over(Order by a.SchdId) as varchar(5)), 
		b.SchdNtNo, cast((case when GrpOrAc=''A'' then ''L'' else GrpOrAc end) as varchar(2)), CAST(OrderBy+''/''+cast(ROW_NUMBER() over(Order by a.SchdId) as varchar(5)) as varchar(100)) 
		, cast(b.OrderBy+''/'' as varchar(100)) as OrderByPar , b.Level 
		, cast('''' as varchar(250)), cast(0 as bit) as SupressSum		-- Added by Sachin N. S. on 06/03/2020 for Bug-32672
	from '+@TBLNAME7+' a 
		inner join Cte_schedule1 b on a.SchdId = b.SchdParId and a.ParId=b.GrporAcID and b.SchdType<>''S''
	Where a.ParId<>a.GrporAcID
)
Select 00 as Updown,SchdType,GrporAcID,GrpParId,SchdId,SchdParId,SchdName,SerialNo, SchdNtNo, OrderBy, OrderByPar, 
		a.Level, ISNULL(b.OpBal,0) OpBal, ISNULL(b.ClBal,0) as ClBal
		, a.Formula, a.SupressSum				-- Added by Sachin N. S. on 06/03/2020 for Bug-32672
		 into '+@TBLNAME8+' from cte_schedule1 a
	Left Outer join '+@TBLNAME6+' b on a.GrporAcID=b.Ac_id and a.SchdType=b.MainFlg
Order by OrderBy '

EXECUTE sp_executesql @SQLCOMMAND

Print 'a14'

--SET @SQLCOMMAND = 'Update a set a.OpBal=B.OpBal,A.ClBal=B.ClBal, SchdType=''SC'' from '+@TBLNAME8+' a 
--		Inner Join (Select SchdParId, Sum(OpBal) as OpBal, Sum(ClBal) as ClBal From '+@TBLNAME8+' Where SchdType<>''S'' and GrporAcID<>0 Group by SchdParId) b on a.SchdId=b.SchdParId
--	Where SchdType=''S'' '
--EXECUTE sp_executesql @SQLCOMMAND


SET @SQLCOMMAND = 
'
DECLARE @MLOOP INT
DECLARE @SCHDTYPE VARCHAR(2), @ORDERBYPAR VARCHAR(100), @OPBAL NUMERIC(18,2), @CLBAL NUMERIC(18,2)
DECLARE SCHDLOOP CURSOR FOR 
SELECT SCHDTYPE, ORDERBYPAR, SUM(OPBAL) OPBAL, SUM(CLBAL) CLBAL FROM '+@TBLNAME8+' 
	WHERE RTRIM(ORDERBY)+''/'' NOT IN (SELECT DISTINCT ORDERBYPAR FROM '+@TBLNAME8+') 
	Group by SCHDTYPE, ORDERBYPAR
OPEN SCHDLOOP
FETCH NEXT FROM SCHDLOOP INTO @SCHDTYPE,@ORDERBYPAR,@OPBAL,@CLBAL
WHILE @@FETCH_STATUS=0
BEGIN
	PRINT @ORDERBYPAR
	SET @MLOOP=1
	WHILE @MLOOP=1
	BEGIN
		UPDATE '+@TBLNAME8+' SET OPBAL=OPBAL+@OPBAL, CLBAL=CLBAL+@CLBAL, 
			SCHDTYPE=CASE WHEN LEFT(SCHDTYPE,1)=''S'' AND LEFT(@SCHDTYPE,1)<>''S'' THEN ''SC'' ELSE SCHDTYPE END 
		WHERE RTRIM(ORDERBY)+''/'' = @ORDERBYPAR
		IF EXISTS(SELECT SCHDTYPE, (CASE WHEN ORDERBYPAR=ORDERBY THEN '''' ELSE ORDERBYPAR END) FROM '+@TBLNAME8+
			' WHERE RTRIM(ORDERBY)+''/'' = @ORDERBYPAR)
			BEGIN
				SELECT @SCHDTYPE=SCHDTYPE, @ORDERBYPAR=(CASE WHEN ORDERBYPAR=ORDERBY THEN '''' ELSE ORDERBYPAR END) FROM '+@TBLNAME8+
					' WHERE RTRIM(ORDERBY)+''/'' = @ORDERBYPAR
				IF @ORDERBYPAR=''''
				BEGIN
					SET @MLOOP=0
				END
			END
		ELSE
			BEGIN
				SET @MLOOP=0
			END
		--PRINT @ORDERBYPAR
	END
	FETCH NEXT FROM SCHDLOOP INTO @SCHDTYPE,@ORDERBYPAR,@OPBAL,@CLBAL
END
CLOSE SCHDLOOP
DEALLOCATE SCHDLOOP
'
--PRINT @SQLCOMMAND
EXECUTE sp_executesql @SQLCOMMAND
--SELECT * FROM SCHDHDTBL WHERE CFORMULA<>''
--print 'a15'
--SET @SQLCOMMAND = 'Select *,RTRIM(CAST(SCHDID AS VARCHAR))+''+'' from '+@TBLNAME8+' Order by OrderBy '
--EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 
'DECLARE @CSPLITFRM VARCHAR(500), @SCHDID int,@OPBAL NUMERIC(18,2), @CFORMULA VARCHAR(500), @CLBAL NUMERIC(18,2),@SCHDID1 INT
DECLARE SCHDLOOP CURSOR FOR
	SELECT SCHDID,CFORMULA FROM SCHDHDTBL A
		WHERE SCHDCODE=''III'' AND SCHDTYPE='''+@SCHDCOND2+''' AND CFORMULA<>''''
OPEN SCHDLOOP
FETCH NEXT FROM SCHDLOOP INTO @SCHDID,@CFORMULA
WHILE @@FETCH_STATUS=0
BEGIN
	PRINT @SCHDID
	PRINT @CFORMULA
	
	--SELECT SCHDID,SCHDNAME, OPBAL,CLBAL, CHARINDEX(RTRIM(CAST(SCHDID AS VARCHAR))+''+'',@CFORMULA),CHARINDEX(''+''+RTRIM(CAST(SCHDID AS VARCHAR)),@CFORMULA) 
	--		FROM '+@TBLNAME8+' WHERE SCHDTYPE IN (''S'')
			
	DECLARE SCHDLOOP1 CURSOR FOR
		SELECT SCHDID,CASE WHEN CHARINDEX(''-''+RTRIM(CAST(SCHDID AS VARCHAR)),@CFORMULA)>0 THEN -OPBAL ELSE OPBAL END AS OPBAL,
			CASE WHEN CHARINDEX(''-''+RTRIM(CAST(SCHDID AS VARCHAR)),@CFORMULA)>0 THEN -CLBAL ELSE CLBAL END AS CLBAL 
		FROM '+@TBLNAME8+' A
			WHERE SCHDTYPE IN (''S'',''SC'') AND 
			(
				(CHARINDEX(RTRIM(CAST(SCHDID AS VARCHAR))+''+'',@CFORMULA)>0 OR CHARINDEX(''+''+RTRIM(CAST(SCHDID AS VARCHAR)),@CFORMULA)>0) or
				(CHARINDEX(RTRIM(CAST(SCHDID AS VARCHAR))+''-'',@CFORMULA)>0 OR CHARINDEX(''-''+RTRIM(CAST(SCHDID AS VARCHAR)),@CFORMULA)>0)
			)
												
	OPEN SCHDLOOP1
	FETCH NEXT FROM SCHDLOOP1 INTO @SCHDID1,@OPBAL,@CLBAL
	WHILE @@FETCH_STATUS=0
	BEGIN
		PRINT @SCHDID1	
		UPDATE '+@TBLNAME8+' SET OPBAL=OPBAL+@OPBAL, CLBAL=CLBAL+@CLBAL
			WHERE SCHDID=@SCHDID AND SCHDTYPE IN (''S'',''SC'')
		
		FETCH NEXT FROM SCHDLOOP1 INTO @SCHDID1,@OPBAL,@CLBAL
	END
	CLOSE SCHDLOOP1
	DEALLOCATE SCHDLOOP1
	
	FETCH NEXT FROM SCHDLOOP INTO @SCHDID,@CFORMULA
END
CLOSE SCHDLOOP
DEALLOCATE SCHDLOOP
'		
PRINT @SQLCOMMAND
EXECUTE sp_executesql @SQLCOMMAND

--SET @SQLCOMMAND = 'select * from '+@TBLNAME8
--EXECUTE sp_executesql @SQLCOMMAND

--	SET @SQLCOMMAND = 'Select OrderByPar, Sum(OpBal), Sum(ClBal), max(OrderBy)+''.''+cast(row_number() over (Order by OrderByPar) as varchar) as OrderBy
--	from '+@TBLNAME8+' 
--	Where Orderby+''/'' not in (Select distinct OrderByPar From '+@TBLNAME8+' ) 
--		and Formula=''''			-- Added by Sachin N. S. on 07/03/2020 for Bug-32672
--		Group by OrderByPar
--		'
--EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = ';with Cte_schedule (OrderByPar, OpBal, ClBal, OrderBy) as
(
	Select OrderByPar, Sum(OpBal), Sum(ClBal), max(OrderBy)+''.''+cast(row_number() over (Order by OrderByPar) as varchar) as OrderBy
	from '+@TBLNAME8+' 
	Where Orderby+''/'' not in (Select distinct OrderByPar From '+@TBLNAME8+' ) 
		and Formula=''''			-- Added by Sachin N. S. on 07/03/2020 for Bug-32672
	Group by OrderByPar
	union all
	Select a.OrderByPar, (isnull(b.OpBal,0)), (isnull(b.ClBal,0)), cast(b.OrderBy as varchar(100))+''.''+cast(row_number() over (Order by b.OrderByPar) as varchar) as OrderBy
		From '+@TBLNAME8+' a
			Inner Join Cte_schedule b on rtrim(a.OrderBy)+''/''=b.OrderByPar
			where Formula=''''			-- Added by Sachin N. S. on 07/03/2020 for Bug-32672
)
Insert Into '+@TBLNAME8+'
select a.Updown, a.SchdType, a.GrporAcID, a.GrpParId, a.SchdId,a.SchdParId,''Total of : ''+a.SchdName SchdName, a.SerialNo, 
	a.SchdNtNo, case when isnull(b.OrderBy,'''')='''' then a.OrderBy else b.OrderBy end OrderBy, a.OrderByPar, a.Level, 
	ISNULL(b.OpBal,0) OpBal, ISNULL(b.ClBal,0) as ClBal, a.Formula, a.SupressSum
From '+@TBLNAME8+' a
	Inner Join (Select OrderByPar, Sum(OpBal) OpBal, Sum(ClBal) ClBal, Max(OrderBy) OrderBy from Cte_schedule Group by OrderByPar) b on rtrim(a.Orderby)+''/'' = b.OrderByPar
Order by OrderBy
'
EXECUTE sp_executesql @SQLCOMMAND

print 'a16'

SET @SQLCOMMAND = ';with cte_schedule (OrderBy,SchdNtNo) as 
(
	Select OrderBy,row_number() over(Order by OrderBy) as SchdNtNo From '+@TBLNAME8+' Where (OpBal!=0 or ClBal!=0) and SchdType=''SC'' and charindex(''.'',OrderBy)=0
	union all
	select a.OrderBy, b.SchdNtNo From '+@TBLNAME8+' a
		inner join cte_schedule b on a.OrderByPar=b.OrderBy+''/''
)
Update a set a.SchdNtNo=b.SchdNtNo from '+@TBLNAME8+' a
	inner join cte_schedule b on a.Orderby=b.OrderBy 
'
EXECUTE sp_executesql @SQLCOMMAND
Print 'a17'

SET @SQLCOMMAND = 'Select * from '+@TBLNAME8+' Order by OrderBy '
EXECUTE sp_executesql @SQLCOMMAND


Print 'a18'
-- Get Scheduled wise Data -- End



SET @SQLCOMMAND = 'Drop table '+@TBLNAME5
EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 'Drop table '+@TBLNAME6
EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 'Drop table '+@TBLNAME7
EXECUTE sp_executesql @SQLCOMMAND

SET @SQLCOMMAND = 'Drop table '+@TBLNAME8
EXECUTE sp_executesql @SQLCOMMAND


/* Droping Temp tables [Start] */
SET @SQLCOMMAND = 'Drop table '+@TBLNAME1
EXECUTE sp_executesql @SQLCOMMAND
SET @SQLCOMMAND = 'Drop table '+@TBLNAME2
EXECUTE sp_executesql @SQLCOMMAND
SET @SQLCOMMAND = 'Drop table '+@TBLNAME3
EXECUTE sp_executesql @SQLCOMMAND
SET @SQLCOMMAND = 'Drop table '+@TBLNAME4
EXECUTE sp_executesql @SQLCOMMAND
/* Droping Temp tables [End] */


