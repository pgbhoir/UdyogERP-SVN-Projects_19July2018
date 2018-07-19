DROP PROCEDURE [Add_Columns]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: 
-- Create date: 
-- Description:	
-- Modify date: 
-- Remark:
-- =============================================

CREATE procedure [Add_Columns] 
--@tblnm as varchar(10),
@tblnm as varchar(100), --Changed by Ajay Jaiswal on 03/04/2010
@ColumnDefination varchar(1000)
as
declare @fldnm as  varchar (100)
set @fldnm=substring(@ColumnDefination,1,CHARINDEX(space(1), rtrim(@ColumnDefination),1))
print @fldnm

declare @name varchar(100),@sqlcommand nvarchar(1000),@fld_exists bit,@id int
declare cur_name cursor for select [name],id from sysobjects where [type]='U' and [name] like @tblnm --'%acdet'
open cur_name
fetch next from cur_name into @name,@id
while (@@fetch_status=0)
begin
	set @sqlcommand=' '
	--if not exists (select * from syscolumns where id=@id and [name]=@fldnm)		--Commented by Shrikant S. on 05/05/2017 for GST		
	if Not exists (select * from syscolumns where id=@id and [name]=replace(Replace(@fldnm,'[',''),']',''))			--Added by Shrikant S. on 05/05/2017 for GST		
	begin
		set @sqlcommand='alter table '+@name+' add '+@ColumnDefination
		print @sqlcommand
		execute sp_executesql @sqlcommand
	end
	fetch next from cur_name into @name,@id
end
close cur_name
deallocate cur_name
GO
