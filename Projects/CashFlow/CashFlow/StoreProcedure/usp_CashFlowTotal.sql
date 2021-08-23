-- =============================================
-- AUTHOR	  :	Anil Anthony
-- CREATE DATE: 18-11-2019
-- DESCRIPTION:	THIS STORED PROCEDURE IS used TO update data in Cash Flow Table
-- MODIFIED BY: 
-- MODIFY DATE: 
-- REMARK:
-- =============================================

IF EXISTS (SELECT name FROM sysobjects WHERE name = N'usp_CashFlowTotal' AND type = 'P')
    DROP PROCEDURE usp_CashFlowTotal 
GO
CREATE PROCEDURE usp_CashFlowTotal 
@vParaList VARCHAR(200)

AS
SET NOCOUNT ON

BEGIN
	DECLARE @iStart INT,@iEnd INT, @iSrNo INT, @iCFcur NUMERIC (16,2),@iCFPrv NUMERIC (16,2)

	SET @vParaList = REPLACE(@vParaList,',','.')
	
	SET @iSrNo   = PARSENAME(@vParaList,1)
	SET @iEnd	 = PARSENAME(@vParaList,2)
	SET @iStart  = PARSENAME(@vParaList,3)

	SELECT @iCFcur = SUM(ISNULL(CFCur,0)),@iCFPrv = SUM(ISNULL(CFPrev,0)) FROM CASHFLOWTemplate WITH (NOLOCK) WHERE SRNO BETWEEN @iStart AND @iEnd 


	UPDATE CASHFLOWTemplate SET CFCur = @iCFcur,CFPrev= @iCFPrv WHERE SRNO = @iSrNo
END
