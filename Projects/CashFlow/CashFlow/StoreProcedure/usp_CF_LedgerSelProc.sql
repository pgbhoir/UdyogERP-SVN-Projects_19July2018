-- =============================================
-- AUTHOR	  :	Anil Anthony
-- CREATE DATE: 05-11-2019
-- DESCRIPTION:	THIS STORED PROCEDURE IS used TO FETCH DATA FROM CF_LEDGER TABLE
-- MODIFIED BY: 
-- MODIFY DATE: 
-- REMARK:
-- =============================================

IF EXISTS (SELECT name FROM sysobjects WHERE name = N'usp_CF_LedgerSelProc' AND type = 'P')
    DROP PROCEDURE usp_CF_LedgerSelProc
GO
CREATE PROCEDURE usp_CF_LedgerSelProc
@ipId	INT
AS
SET NOCOUNT ON

BEGIN
	SELECT *  from CF_LEDGER  WITH (NOLOCK) WHERE PID=@ipId 
END
