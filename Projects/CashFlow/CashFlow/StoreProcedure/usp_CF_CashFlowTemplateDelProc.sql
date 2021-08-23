-- =============================================
-- AUTHOR	  :	Anil Anthony
-- CREATE DATE: 09-11-2019
-- DESCRIPTION:	THIS STORED PROCEDURE IS used TO Delete THE RECORD FROM CASHFLOWTEMPLATE AND ITS CORRESPONDING RECORDS FROM CF_LEDGER TABLE
-- MODIFIED BY: 
-- MODIFY DATE: 
-- REMARK:
-- =============================================

IF EXISTS (SELECT name FROM sysobjects WHERE name = N'usp_CF_CashFlowTemplateDelProc' AND type = 'P')
    DROP PROCEDURE usp_CF_CashFlowTemplateDelProc
GO
CREATE PROCEDURE usp_CF_CashFlowTemplateDelProc
@iID	INT,
@iPID	INT,
@iSrNo	INT
AS
SET NOCOUNT ON

BEGIN
	DELETE FROM CF_LEDGER WHERE pid  = @iPID
	Delete from CASHFLOWTemplate WHERE srno = @iSrNo
END
