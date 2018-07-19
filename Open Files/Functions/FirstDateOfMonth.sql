DROP FUNCTION [FirstDateOfMonth]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Function [FirstDateOfMonth](@Date smalldatetime)
Returns Smalldatetime
as
Begin
	Return convert(smalldatetime,substring(convert(varchar(50),@date,112),5,2)+'/01/'+substring(convert(varchar(50),@date,112),1,4))
End
GO
