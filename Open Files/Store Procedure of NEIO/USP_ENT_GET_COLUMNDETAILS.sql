IF EXISTS(SELECT * FROM SYSOBJECTS WHERE XTYPE='P' AND [NAME]='USP_ENT_GET_COLUMNDETAILS')
BEGIN
	DROP PROCEDURE [USP_ENT_GET_COLUMNDETAILS]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [USP_ENT_GET_COLUMNDETAILS]
@tblnm Varchar(50)
AS
Select 
[COL_NAME]=b.[name],
col_id=b.colid,
col_typename=c.[name],
col_len=b.[length],
--col_prec=b.xprec,  --Commented by Priyanka B on 03032020 for Bug-32474
col_prec=b.prec,  --Modified by Priyanka B on 03032020 for Bug-32474
col_scale=b.xscale,
col_null=b.isnullable,
col_identity=b.colstat,
col_dridefname=d.[name],
col_Default=convert(varchar(50),d.[definition]),
b.collation,
b.colorder
From
SysObjects a 
Inner join syscolumns b on (a.id=b.id)
--inner Join sys.types c on (c.system_type_id=b.xtype)  --Commented by Priyanka B on 26022020 for Bug-32474
inner Join sys.types c on (c.user_type_id=b.xtype)  --Modified by Priyanka B on 26022020 for Bug-32474
left Join sys.default_constraints d on (a.id=d.parent_object_id and b.colid=d.parent_column_id)
Where a.[name]=@tblnm
Order by b.colorder
GO
