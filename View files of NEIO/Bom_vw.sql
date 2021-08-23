DROP VIEW [Bom_vw]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [Bom_vw]
as 
Select 'H' as Bomtype,0 as BomdetId,a.BomId,a.BomLevel,a.FGQty,b.It_name From Bomhead a Join It_Mast b On a.ItemId = b.It_Code
Union all
Select 'D' as Bomtype,a.BomdetID,a.BomId,a.BomLevel,a.RMQty,b.It_name From Bomdet a Join It_Mast b On a.RMItemId = b.It_Code
GO
