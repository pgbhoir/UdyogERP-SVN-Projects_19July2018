-- =============================================
-- AUTHOR	  :	Anil Anthony
-- CREATE DATE: 09-11-2019
-- DESCRIPTION:	THIS STORED PROCEDURE IS used TO Delete THE RECORD FROM CF_LEDGER TABLE
-- MODIFIED BY: 
-- MODIFY DATE: 
-- REMARK:
-- =============================================

IF EXISTS (SELECT name FROM sysobjects WHERE name = N'usp_CF_LedgerDelProc' AND type = 'P')
    DROP PROCEDURE usp_CF_LedgerDelProc
GO
CREATE PROCEDURE usp_CF_LedgerDelProc
@iledID	INT
AS
SET NOCOUNT ON

BEGIN
	Delete from CF_LEDGER WHERE Led_ID=@iledID 
END
