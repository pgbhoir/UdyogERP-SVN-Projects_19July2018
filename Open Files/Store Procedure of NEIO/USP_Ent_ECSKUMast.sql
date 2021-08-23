IF EXISTS(SELECT * FROM SYSOBJECTS WHERE XTYPE='P' AND NAME='USP_Ent_ECSKUMast')
BEGIN
	DROP PROCEDURE USP_Ent_ECSKUMast
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  Procedure [dbo].[USP_Ent_ECSKUMast]
(
	@sku Varchar(30),@ItName Varchar(100)
)
As
Begin
	Declare @SqlCommand nvarchar(4000)

	declare @sqlcmd nvarchar(max)
	set @sqlcmd=''
	set @sqlcmd = @sqlcmd + ' select sel=0,a.Id,a.sku,a.it_code ,b.it_name,b.it_desc,b.[type],b.[group] from ecskumast a left join it_mast b on (a.it_code=b.it_code)'
	/*if @sku <> ''  and @ItName <>''
	begin
		set @sqlcmd = @sqlcmd + ' where sku='+char(39)+@sku +char(39)+' and it_name='+char(39)+@ItName+char(39)
	end	
	*/
	print @sqlcmd
	exec sp_executesql @sqlcmd

End
