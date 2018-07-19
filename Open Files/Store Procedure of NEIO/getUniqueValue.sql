DROP PROCEDURE [getUniqueValue]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE procedure [getUniqueValue]
as
declare @UniqueNo varchar(20)
declare curUniqueVal cursor for 
SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
+ (DATEPART(ss, GETDATE()) * 1000 )
+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as no
open curUniqueVal
fetch next from curUniqueVal into @UniqueNo
while @@fetch_status=0
begin
	print rtrim(@UniqueNo)
	fetch next from curUniqueVal into @UniqueNo
end
close curUniqueVal
deallocate curUniqueVal
GO
