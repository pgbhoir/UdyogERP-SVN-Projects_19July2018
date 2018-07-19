DROP PROCEDURE [USP_SELECT_AUTO_OP_HEAD]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author      : Raghavendra Joshi
-- Create date : 04/01/2012
-- Description :	
-- =============================================

CREATE Procedure [USP_SELECT_AUTO_OP_HEAD]
(
@PrimaryId Int = 0
)
As
SELECT TOP 1 a.*
--	,b.Ac_name
--	,b.Mailname
	,c.It_name
	,d.Bomid,d.Bomlevel
	FROM Auto_op_head a
--	Inner Join Ac_mast b ON (a.Ac_Id = b.Ac_Id)
	Inner Join It_Mast c ON (a.It_code = c.It_code)
	Inner Join Bomhead d ON (a.FkBomid = d.PrimaryId)
	WHERE a.PrimaryId = @PrimaryId
GO
