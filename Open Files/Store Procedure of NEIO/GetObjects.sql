DROP PROCEDURE [GetObjects]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GetObjects]
as
Create Table #tmpObjects (oType int, oObjName Varchar(75),oOwner Varchar(10),oSequence int)

insert Into #tmpObjects Execute sys.sp_MSdependencies

Select * from #tmpObjects Order By (case when oType =8 then 'a' else (case when oType =1 then 'b' else (case when oType =4 then 'c' else (case when oType =16 then 'd' else 'e' end) end)end)end),oSequence,oObjName

Drop Table #tmpObjects
GO
