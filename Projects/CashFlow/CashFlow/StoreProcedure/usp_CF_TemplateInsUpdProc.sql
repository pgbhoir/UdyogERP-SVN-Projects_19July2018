/****** Object:  StoredProcedure [dbo].[usp_CF_TemplateInsUpdProc]    Script Date: 05-11-2019 15:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- AUTHOR	  :	Anil Anthony
-- CREATE DATE: 08-11-2019
-- DESCRIPTION:	THIS STORED PROCEDURE IS used INSERT/UPDATE RECORDS IN CF_Template/CF_Ledger TABLE
-- MODIFIED BY: 
-- MODIFY DATE: 
-- REMARK:
-- =============================================

IF EXISTS (SELECT name FROM sysobjects WHERE name = N'usp_CF_TemplateInsUpdProc' AND type = 'P')
    DROP PROCEDURE usp_CF_TemplateInsUpdProc
GO
CREATE PROCEDURE usp_CF_TemplateInsUpdProc
@iPID			INT,
@vparticular	varchar(100),
@iLed_Id		INT,
@iSrNo			INT,
@ac_id			INT,
@ac_name		varchar(50),
@cf_type		char(1)
AS
SET NOCOUNT ON


SET IDENTITY_INSERT dbo.CASHFLOWTemplate  OFF
BEGIN

DECLARE @id INT
SELECT @id = 0
--IF NOT EXISTS(SELECT particular FROM dbo.CashFlowTemplate WITH (NOLOCK)
--	WHERE particular = @vparticular)
--BEGIN
--	INSERT INTO CashFlowTemplate(srno, pid, particular)
--						VALUES (@iSrNo,@iPID, @vparticular)
--	SELECT @id  = @@IDENTITY
--END

IF NOT EXISTS(SELECT particular FROM dbo.CashFlowTemplate WITH (NOLOCK)
	WHERE srno = @iSrNo)
BEGIN
	INSERT INTO CashFlowTemplate(srno, pid, particular)
						VALUES (@iSrNo,@iPID, @vparticular)
	SELECT @id  = @@IDENTITY
END


IF @ac_name <> ''
BEGIN

	IF NOT EXISTS(SELECT ac_name FROM CF_LEDGER WITH (NOLOCK)
		WHERE ac_name = @ac_name AND Pid = @iPID)
	BEGIN
		INSERT INTO CF_LEDGER(Pid, ac_id, Particular, ac_name, CF_TYPE)
				VALUES (@iPID, @ac_id, @vparticular, @ac_name,@cf_type)

	END
	ELSE
	BEGIN
		UPDATE CF_LEDGER SET  Pid = @iPID, ac_id = @ac_id, particular = @vparticular,
		ac_name = @ac_name, CF_Type = @cf_type where Led_Id = @iLed_Id AND Pid = @iPID

	END
END
SELECT  ID = @id

END

