-- =============================================
-- AUTHOR	  :	Anil Anthony
-- CREATE DATE: 11-11-2019
-- DESCRIPTION:	THIS STORED PROCEDURE TO DISPLAY CASH FLOW STATEMENT IN RPT
-- MODIFIED BY: 
-- MODIFY DATE: 
-- REMARK:
-- =============================================

IF EXISTS (SELECT name FROM sysobjects WHERE name = N'usp_REP_CASHFLOWSTATEMENT ' AND type = 'P')
    DROP PROCEDURE usp_REP_CASHFLOWSTATEMENT 
GO
CREATE PROCEDURE usp_REP_CASHFLOWSTATEMENT 
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)= null

AS
SET NOCOUNT ON

BEGIN
			DECLARE @vparticular char(100), @cAc_Name AS VARCHAR(50),@CF_Type	AS CHAR(1)
			DECLARE @iID AS INT,@iPID	AS	INT, @iAc_ID AS	INT, @LVL AS INT, @MCOND AS INT
			Declare @OPENTRIES as VARCHAR(50),@OPENTRY_TY as VARCHAR(50),@cParameter AS VARCHAR(50)
			DECLARE @iDebit INT, @iCredit INT,@iDebitAmt INT, @iCreditAmt INT, @iTotal INT
		
			DECLARE @exParameter VARCHAR(50), @param VARCHAR(50) ,@cExQuery VARCHAR(50)
			DECLARE @cFldValue VARCHAR(50), @Parset VARCHAR(50)
			DECLARE @iPosindex INT
			DECLARE @prevDt1 SMALLDATETIME, @prevDt2 SMALLDATETIME

			SELECT @prevDt1=prevdate1,@prevDt2=prevdate2 from dbo.udf_datefunction(@SDATE,@EDATE)
		
			UPDATE CASHFLOWTEMPLATE SET CFCUR=0,CFPREV=0 
			
			CREATE TABLE #curCFFlow (id int,Particular VARCHAR(100), dbAmount numeric (16,2),crAmount numeric (16,2),
			prvdbAmount numeric (16,2),prvcrAmount numeric (16,2))

			CREATE TABLE #curTemp (ID INT,AC_ID INT,AC_NAME varchar(100),dbAmount Decimal (18,2),crAmount Decimal (18,2))

			DECLARE cur_CashFlowTemp cursor FOR SELECT ID,Particular, PID FROM CASHFLOWTemplate WHERE IsHead <> 1
			open cur_CashFlowTemp
			fetch next from cur_CashFlowTemp into  @iID,@vparticular, @iPID
			while (@@fetch_Status=0)	
			Begin
				DECLARE cur_CashFlowDet cursor for Select ac_id,ac_name,cf_type from cf_ledger where PID = @iPID
				open cur_CashFlowDet
				fetch next from cur_CashFlowDet into @iAc_ID,@cAc_Name,@CF_Type
				while (@@FETCH_STATUS=0)
				Begin
					if @CF_Type = 'G' 
					BEGIN
							DECLARE openingentry_cursor CURSOR FOR 	SELECT entry_ty FROM lcode
											WHERE bcode_nm = 'OB'
							OPEN openingentry_cursor
							FETCH NEXT FROM openingentry_cursor into @opentries
							WHILE @@FETCH_STATUS = 0
							BEGIN
								Set @OPENTRY_TY = @OPENTRY_TY +','+CHAR(39)+@opentries+CHAR(39)
								FETCH NEXT FROM openingentry_cursor into @opentries
							END
							CLOSE openingentry_cursor
							DEALLOCATE openingentry_cursor

							CREATE TABLE #ACGRPID (GACID DECIMAL(9),LVL DECIMAL(9))
							SET @LVL=0
							INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL  FROM AC_GROUP_MAST WHERE AC_GROUP_NAME=@cAc_Name
							SET @MCOND=1
							WHILE @MCOND=1
							BEGIN
								IF EXISTS (SELECT AC_GROUP_ID FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)) --WHERE LVL=@LVL
								BEGIN
									INSERT INTO #ACGRPID SELECT AC_GROUP_ID,@LVL+1 FROM AC_GROUP_MAST WHERE GAC_ID IN (SELECT DISTINCT GACID  FROM #ACGRPID WHERE LVL=@LVL)
									SET @LVL=@LVL+1
								END
								ELSE
								BEGIN
									SET @MCOND=0	
								END
							END
							SELECT AC_ID,AC_NAME INTO #ACMAST FROM AC_MAST WHERE  AC_GROUP_ID IN (SELECT DISTINCT GACID FROM #ACGRPID)
							insert into  #curTemp SELECT @iID,AC_ID,AC_NAME,0,0 FROM #ACMAST
							drop table #ACGRPID
							drop table #ACMAST
						END
						if @CF_Type = 'L' 
						BEGIN
							insert into  #curTemp (ID ,AC_ID,AC_NAME,dbAmount,crAmount)
								VALUES(@iID,@iAc_ID,@cAc_Name,0,0)

						END
			
					fetch next from cur_CashFlowDet into @iAc_ID,@cAc_Name,@CF_Type
				
				End
			
				close cur_CashFlowDet 
				deallocate cur_CashFlowDet
				
				
				INSERT INTO #curCFFlow Select @iID,@vparticular, CASE WHEN amt_ty='DR' THEN SUM(AMOUNT) ELSE 0 END AS dbAmount,
				CASE WHEN amt_ty='CR' THEN SUM(AMOUNT) ELSE 0 END AS crAmount,0,0 FROM lac_vw with (nolock)
				WHERE lac_vw.date between @SDATE and @EDATE and ac_id in (select ac_id from #curTemp WHERE ID=@iID) group by lac_vw.amt_ty
				UNION
				Select @iID,@vparticular,0,0,CASE WHEN amt_ty='DR' THEN SUM(AMOUNT) ELSE 0 END AS prvdbAmount,
				CASE WHEN amt_ty='CR' THEN SUM(AMOUNT) ELSE 0 END AS prvcrAmount FROM lac_vw with (nolock)
				WHERE lac_vw.date between @prevDt1 and @prevDt2 and ac_id in (select ac_id from #curTemp WHERE ID=@iID) group by lac_vw.amt_ty
				
						
				fetch next from cur_CashFlowTemp into  @iID,@vparticular, @iPID
				
				end
			close cur_CashFlowTemp
			deallocate cur_CashFlowTemp
	
			UPDATE C SET C.cfcur = a.cfcur,c.cfprev=a.cfprv from CASHFLOWTemplate c
			inner join (
				select f.id,sum(f.dbamount-f.crAmount) as cfcur,sum(f.prvdbAmount-f.prvcrAmount) as cfprv
				 from  #curCFFlow f group by id) a on c.id = a.id
			DROP TABLE #curCFFlow
			DECLARE cur_CashFlowUpd cursor FOR SELECT PID,ac_name FROM CF_Ledger WHERE CF_Type='P'
			open cur_CashFlowUpd
			fetch next from cur_CashFlowUpd into  @iPID,@cFldValue
			while (@@fetch_Status=0)	
			Begin
	
				SET @exParameter =   @cFldValue
				SET @iPosindex	= CHARINDEX(CHAR(32),@cFldValue)
					SET @param = '['+SUBSTRING(@cFldValue,@iPosindex+1,LEN(@cFldValue))+']'
				SELECT @exParameter = SUBSTRING(@cFldValue,1,@iPosindex),@parset=@param
				set @cExQuery = @exParameter + @parset
				EXECUTE sp_sqlexec @cExQuery
				fetch next from cur_CashFlowUpd into @iPID, @cFldValue
			End
			close cur_CashFlowUpd 
			deallocate cur_CashFlowUpd	
		
			Select * FROM CASHFLOWTemplate order by srno
END
