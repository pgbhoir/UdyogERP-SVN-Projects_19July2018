IF EXISTS(SELECT * FROM SYSOBJECTS WHERE XTYPE='P' AND NAME='USP_Del_ECSKUMast')
BEGIN
	DROP PROCEDURE USP_Del_ECSKUMast
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  Procedure [dbo].[USP_Del_ECSKUMast]
(
	@sku Varchar(100)
)
As
Begin
	Declare @SqlCommand nvarchar(4000)
	set @sku=replace(@sku,',',''',''')
	print @sku
	declare @sqlcmd nvarchar(max)
	set @sqlcmd=''
	--set @sqlcmd = 'select distinct tblnm from (select it_name as item,ecsku,tblnm=''ITEM MASTER'' from it_mast where ecsku in ('+rtrim(ltrim(@sku))+')'
	--set @sqlcmd = @sqlcmd + ' ' + 'union all select item,ecsku,''SALES ORDER'' from soitem where ecsku in ('+rtrim(ltrim(@sku))+') ) aa order by tblnm'

	set @sqlcmd = 'select tblnm=isnull(stuff((select distinct '',''+tblnm from (select it_name as item,ecsku,tblnm=''ITEM MASTER'' from it_mast where ecsku in ('+rtrim(ltrim(@sku))+')'
	set @sqlcmd = @sqlcmd + ' ' + 'union all select item,ecsku,''SALES ORDER'' from soitem where ecsku in ('+rtrim(ltrim(@sku))+') ) aa for xml path('''')),1,1,''''),'''') order by tblnm'
		
	--set @sqlcmd = 'select distinct tblnm=isnull(stuff((select distinct '',''+tblnm from (select it_name as item,ecsku,tblnm=''ITEM MASTER'' from it_mast where ecsku in ('+rtrim(ltrim(@sku))+')'
	--set @sqlcmd = @sqlcmd + ' ' + 'union all select item,ecsku,''SALES ORDER'' from soitem where ecsku in ('+rtrim(ltrim(@sku))+') ) aa for xml path('''')),1,1,''''),'''') order by tblnm'

	print @sqlcmd
	exec sp_executesql @sqlcmd
End