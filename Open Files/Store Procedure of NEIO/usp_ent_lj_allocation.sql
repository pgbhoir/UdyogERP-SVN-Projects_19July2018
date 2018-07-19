iF exists(Select [Name] From SysObjects Where xType='P' and [Name]='usp_ent_lj_allocation')
Begin
	DROP PROCEDURE [usp_ent_lj_allocation]
end
GO
-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 09/05/2009
-- Description:	This Stored procedure is useful in Labour Job form uefrm_lj_allocation.SCX.
-- Modification Date/By/Reason: 11/08/2009 Rupesh Prajapati. Modified for Party Name Filteration.
-- Modification Date/By/Reason: 11/08/2009 Rupesh Prajapati. Modified for Date Filteration.
-- Modification Date/By/Reason: 16/07/2010 Shrikant S. Modified for L2S-56(Apostrophe)
-- Modification Date/By/Reason: 16/08/2010 Shrikant S. Modified for TKT-3223
-- Remark:		
-- =============================================
create procedure [usp_ent_lj_allocation]
@party_nm varchar(100),@entry_ty varchar(2),@tran_cd int, @itserial varchar(5),@dt smalldatetime
as
declare @sqlcommand nvarchar(4000),@bcode_nm varchar(2)

select @bcode_nm=(case when ext_vou=1 then bcode_nm else entry_ty end) from lcode where entry_ty=@entry_ty
--select bcode_nm=(case when ext_vou=1 then bcode_nm else entry_ty end) from lcode where entry_ty=@entry_ty

--select lientry_ty,li_itser,li_tran_cd,qty_used=sum(qty_used),wastage=sum(wastage)		--Commented by Shrikant S. on16/08/2010 for TKT-3223 
select lientry_ty,li_itser,li_tran_cd,qty_used=sum(qty_used),wastage=sum(wastage),procwaste=sum(procwaste)	--Added by Shrikant S. on16/08/2010 for TKT-3223 
into #rmdet
from irrmdet 
where 1=2
group by lientry_ty,li_itser,li_tran_cd

--select b.u_pinvno,b.u_pinvdt,a.inv_no,a.date,item=i1.it_name,i1.it_code,a.qty,qty_used=a.qty,aqty=a.qty,adjqty=a.qty,wastage=a.qty	--Commented by Shrikant S. on16/08/2010 for TKT-3223 
select b.pinvno,b.pinvdt,a.inv_no,a.date,item=i1.it_name,i1.it_code,a.qty,qty_used=a.qty,aqty=a.qty,adjqty=a.qty,wastage=a.qty,procwaste=a.qty	--Added by Shrikant S. on16/08/2010 for TKT-3223 
,days=999
,days180=999
,a.entry_ty,a.tran_cd,a.itserial,a.doc_no
into #lj
from iiitem a
inner join iimain b on (a.entry_ty=b.entry_ty and a.tran_cd=b.tran_cd)
inner join it_mast i1 on (a.it_code=i1.it_code)
where 1=2

if charindex('''',@party_nm)>0
Begin
	set @party_nm=replace(@party_nm,'''','''''')
end

set @sqlcommand='insert into #rmdet select lientry_ty,li_itser,li_tran_cd,qty_used=sum(qty_used),wastage=sum(wastage),procwaste=sum(procwaste)'		--Changed by Shrikant S. on16/08/2010 for TKT-3223 
set @sqlcommand=rtrim(@sqlcommand)+' '+' from '+(case when (@bcode_nm='IR') then 'IR' else 'II' end)+'rmdet '
set @sqlcommand=rtrim(@sqlcommand)+' '+' where not (entry_ty='+char(39)+@entry_ty+char(39)+' and tran_cd='+ltrim( rtrim(cast(@tran_cd as varchar)) )+')' --and itserial=@itserial)'
set @sqlcommand=rtrim(@sqlcommand)+' '+' group by lientry_ty,li_itser,li_tran_cd'
print @sqlcommand
execute sp_executesql @sqlcommand

--select * from #rmdet

---- set @sqlcommand='insert into #lj select a.u_pinvno,a.u_pinvdt,c.inv_no,c.date,item=i1.it_name,i1.it_code,a.qty,qty_used=0,aqty=a.qty,adjqty=0,wastage=0,procwaste=0' ---- Commented by suraj kuamwat date on 22-04-2017 for GST Changed by Shrikant S. on16/08/2010 for TKT-3223 
set @sqlcommand='insert into #lj select a.pinvno,a.pinvdt,c.inv_no,c.date,item=i1.it_name,i1.it_code,a.qty,qty_used=0,aqty=a.qty,adjqty=0,wastage=0,procwaste=0'		 ---Changed by suraj Kumawat date on 22-04-2017 for gst
set @sqlcommand=rtrim(@sqlcommand)+' '+' ,days=(  case when (datediff(day,a.date,'+char(39)+cast(@dt as varchar)+char(39)+'))<=180 then (datediff(day,a.date,'+char(39)+cast(@dt as varchar)+char(39)+')) else 0 end  )'
set @sqlcommand=rtrim(@sqlcommand)+' '+' ,days180=(  case when (datediff(day,a.date,'+char(39)+cast(@dt as varchar)+char(39)+'))>180 then (datediff(day,a.date,'+char(39)+cast(@dt as varchar)+char(39)+')) else 0 end  )'
set @sqlcommand=rtrim(@sqlcommand)+' '+' ,a.entry_ty,a.tran_cd,a.itserial,a.doc_no'
set @sqlcommand=rtrim(@sqlcommand)+' '+' from '+(case when (@bcode_nm='IR') then 'II' else 'IR' end)+'item a'
set @sqlcommand=rtrim(@sqlcommand)+' '+' inner join '+(case when (@bcode_nm='IR') then 'II' else 'IR' end)+'main c on (a.entry_ty=c.entry_ty and a.tran_cd=c.tran_cd)'
set @sqlcommand=rtrim(@sqlcommand)+' '+' inner join it_mast i1 on (a.it_code=i1.it_code)'
set @sqlcommand=rtrim(@sqlcommand)+' '+' inner join ac_mast  on (ac_mast.ac_id=c.ac_id)'
set @sqlcommand=rtrim(@sqlcommand)+' '+' where a.entry_ty='+char(39)+(case when (@bcode_nm='IR') then 'LI' else 'RL' end)+char(39)
set @sqlcommand=rtrim(@sqlcommand)+' '+' and ac_mast.ac_name='+char(39)+rtrim(@party_nm)+char(39)
set @sqlcommand=rtrim(@sqlcommand)+' '+' and a.date<='+char(39)+cast(@dt as varchar)+char(39)
print @sqlcommand
execute sp_executesql @sqlcommand

--select * from #lj
--select * from #rmdet

update a set a.qty_used=b.qty_used+b.wastage+b.procwaste,a.aqty=a.aqty-(b.qty_used+b.wastage+b.procwaste),adjqty=0,wastage=0,procwaste=0	--Changed by Shrikant S. on16/08/2010 for TKT-3223 
from #lj a 
inner join #rmdet b on (a.entry_ty=b.lientry_ty and a.tran_cd=b.li_tran_cd and a.itserial=b.li_itser)
--update #lj a set aqty=,adjqty=0,wastage=0,days180=0

delete from #lj where aqty=0
select * from #lj Order by date

--QTY_USED=consumed qty
GO
