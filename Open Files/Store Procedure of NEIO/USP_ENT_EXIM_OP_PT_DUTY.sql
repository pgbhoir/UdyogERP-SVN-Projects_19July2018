DROP PROCEDURE [USP_ENT_EXIM_OP_PT_DUTY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Uday G.
-- Create date: 29/12/2011
-- Description:	This Stored procedure is useful in OP Entry.
-- Modify date: 
-- Modified By: 
-- Modify date: 
-- Remark: 
-- =============================================

create PROCEDURE [USP_ENT_EXIM_OP_PT_DUTY] 
@ientry_ty varchar(02),
@aentry_ty varchar(02),
@atran_cd int,
@aitserial varchar(05)
AS
Declare @fldlist Varchar(8000),@sqlquery nvarchar(max)
Execute USP_ENT_GETFIELD_LIST 'ipitem', @fldlist Output 
print @fldlist
set @fldlist=@fldlist+',[Rule],[pentry_ty],[ptran_cd],[pitserial]'
select *,cast('' as varchar(50)) as [Rule],
	cast('' as varchar(02)) as pentry_ty,
	cast(0 as int) as ptran_cd,
	cast('' as varchar(05)) as pitserial
into #curIPItem from ipitem where 1=2


declare @entry_ty varchar(02),@tran_cd int,@itserial varchar(05)
---a.entry_ty,a.tran_cd,a.itserial,

Declare dtCursor cursor for  
		select a.entry_ty,a.tran_cd,a.itserial from projectitref a
			where a.entry_ty = @ientry_ty and a.aentry_ty = @aentry_ty and a.atran_cd = @atran_cd and a.aitserial = @aitserial

Open dtCursor


Fetch Next from dtCursor Into @entry_ty,@tran_cd,@itserial
while @@Fetch_status=0
begin
		set @sqlquery=@sqlquery+' '+'Set Identity_Insert #curIpItem On'
		set @sqlquery=@sqlquery+' '+'insert into #curIpItem ('+@fldlist+')'
		set @sqlquery=@sqlquery+' '+'select a.*,b.[rule],'
		set @sqlquery=@sqlquery+' '+'cast('' as varchar(02)) as pentry_ty,' 
		set @sqlquery=@sqlquery+' '+'cast(0 as int) as ptran_cd,'
		set @sqlquery=@sqlquery+' '+'cast('' as varchar(05)) as pitserial '
		set @sqlquery=@sqlquery+' '+'from ipitem a (nolock) '
		set @sqlquery=@sqlquery+' '+'inner join ipmain b (nolock)'
		set @sqlquery=@sqlquery+' '+'on a.entry_ty = b.entry_ty and'
		set @sqlquery=@sqlquery+' '+'a.tran_cd = b.tran_cd'
		set @sqlquery=@sqlquery+' '+'where a.entry_ty = '''+@entry_ty+''' and a.tran_cd  = '+Convert(Varchar(10),@tran_cd)+' and a.itserial = '''+@itserial+''''	
		set @sqlquery=@sqlquery+' '+'Set Identity_Insert #curIpItem Off'
		Execute sp_executesql 	@sqlquery										
				  
	Fetch Next from dtCursor Into @entry_ty,@tran_cd,@itserial
end
close dtCursor
deallocate dtCursor

update a set a.pentry_ty = b.rentry_ty,
			 a.ptran_cd = b.itref_tran,
			 a.pitserial = b.ritserial
from #curIpItem a 
	inner join othitref b
		on a.entry_ty = b.entry_ty and
		   a.tran_cd = b.tran_cd and
		   a.itserial = b.itserial	  
select * from #curIpItem
drop table #curIpItem

----execute USP_ENT_EXIM_OP_PT_DUTY 'IP','WK',2,'00001'

--Close dtCursor
GO
