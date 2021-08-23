-- =============================================
-- AUTHOR	  :	Anil Anthony
-- CREATE DATE: 20-11-2019
-- DESCRIPTION:	THIS FUNCTION IS TO RETURN PREVIOUS YEAR/MONTH/DAY 
-- MODIFIED BY: 
-- MODIFY DATE: 
-- REMARK:
-- =============================================

IF EXISTS (SELECT name FROM sysobjects WHERE name = N'udf_DateFunction ')
    DROP FUNCTION udf_DateFunction 
GO
CREATE FUNCTION dbo.udf_DateFunction 
(
@SDATE SMALLDATETIME,
@EDATE SMALLDATETIME
)
RETURNS @table table (prevdate1 SMALLDATETIME, prevdate2 SMALLDATETIME)

AS


BEGIN
DECLARE @iDays INT
DECLARE @SPrvDate SMALLDATETIME
DECLARE @EPrvDate SMALLDATETIME

SET @iDays= datediff(day,@SDATE,@EDATE)

IF @iDays>=365
BEGIN
	SET @SPrvDate = DATEADD(YEAR,-1,@SDATE)
	SET @EPrvDate = DATEADD(YEAR,-1,@EDATE)
END
IF @iDays between 1 and 94
BEGIN
	SET @SPrvDate = DATEADD(MONTH,-1,@SDATE)
	SET @EPrvDate = DATEADD(MONTH,-1,@EDATE)
END

IF @iDays=0
BEGIN
	SET @SPrvDate = DATEADD(DAY,-1,@SDATE)
	SET @EPrvDate = DATEADD(DAY,-1,@EDATE)
END
Insert Into @table (prevdate1 , prevdate2 ) VALUES (@SPrvDate, @EPrvDate)
Return

END
