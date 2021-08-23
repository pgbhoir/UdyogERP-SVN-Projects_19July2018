-- =============================================
-- AUTHOR	  :	Anil Anthony
-- CREATE DATE: 19-11-2019
-- DESCRIPTION:	THIS STORED PROCEDURE IS used TO INSERT/UPDATE SRNO IN CASHFLOW TABLE
-- MODIFIED BY: 
-- MODIFY DATE: 
-- REMARK:
-- =============================================

IF EXISTS (SELECT name FROM sysobjects WHERE name = N'usp_CF_InsUpdSrnoProc' AND type = 'P')
    DROP PROCEDURE usp_CF_InsUpdSrnoProc 
GO
CREATE PROCEDURE usp_CF_InsUpdSrnoProc 
@iId		INT,
@iSrno		INT,
@iPID		INT,
@vparticular	varchar(100),
@bIsHead	bit,
@bIsTotal	bit

AS
SET NOCOUNT ON

BEGIN
	DECLARE @cid INT
	SELECT @cid = 0
--	IF @iId <> 0
--	BEGIN
--		UPDATE CashFlowTemplate SET SrNo = @iSrno,ishead = @bIsHead, istotal=@bIsTotal where ID = @iId
--	END
--	ELSE
--	BEGIN
--		INSERT INTO CashFlowTemplate (srno,pid,particular,ishead,istotal) values (@iSrno, @iPID,'',@bIsHead,@bIsTotal)
--		SELECT @cid  = @@IDENTITY
--	END
--	SELECT  ID = @cid
--END

	IF  EXISTS(SELECT particular FROM dbo.CashFlowTemplate WITH (NOLOCK)
		WHERE srno = @iSrNo)
	BEGIN
		UPDATE CashFlowTemplate SET ishead = @bIsHead, istotal=@bIsTotal,particular=@vparticular where SrNo = @iSrno
	END
	ELSE
	BEGIN
			INSERT INTO CashFlowTemplate (srno,pid,particular,ishead,istotal) values (@iSrno, @iPID,'',@bIsHead,@bIsTotal)
			SELECT @cid  = @@IDENTITY
	END
	SELECT  ID = @cid
END

