IF EXISTS(SELECT * FROM SYSOBJECTS WHERE XTYPE='P' AND [NAME]='USP_ENT_BOMDET')
BEGIN
		DROP PROCEDURE USP_ENT_BOMDET
END
GO
-- =============================================
-- Author:		Ruepesh Prajapati.
-- Create date: 17/04/2009
-- Description:	This Stored procedure is useful In Work Order Entry.
-- Modify date: 13/04/2010
-- Modified By: Hetal L Patel
-- Modify date: 
-- Remark: TKT-946 Bom Close Validation
-- =============================================
CREATE PROCEDURE [dbo].[USP_ENT_BOMDET] 
@qty numeric(18,4)
AS
--select i.bomid,i.bomlevel,i.rmitemid,i.rmitem,i.particular,i.rmqty,req_qty=i.rmqty  -- Commented by Prajakta B. on 10/02/2020 for Bug 32929
select i.bomid,i.bomlevel,i.rmitemid,i.rmitem,cast(im.it_desc as varchar(100)) as it_desc,i.particular,i.rmqty,req_qty=i.rmqty -- Modified by Prajakta B. on 10/02/2020 for Bug 32929
into #bomdet
from bomdet i
inner join bomhead m on (m.bomid=i.bomid and m.bomlevel=i.bomlevel)
left join it_mast im on (im.It_code=i.RmitemId and im.it_name=i.Rmitem) -- Added by Prajakta B. on 10/02/2020 for Bug 32929
where 1=2

-- Commented by Prajakta B. on 10/02/2020 for Bug 32929  Start
--insert into #bomdet (bomid,bomlevel,rmitemid,rmitem,particular,rmqty,req_qty)
--select i.bomid,i.bomlevel,i.rmitemid,i.rmitem,i.particular,i.rmqty,req_qty=(@qty*i.rmqty)/m.fgqty
-- Commented by Prajakta B. on 10/02/2020 for Bug 32929  End
-- Modified by Prajakta B. on 10/02/2020 for Bug 32929  start
insert into #bomdet (bomid,bomlevel,rmitemid,rmitem,it_desc,particular,rmqty,req_qty)
select i.bomid,i.bomlevel,i.rmitemid,i.rmitem,cast(im.it_desc as varchar(100)) as it_desc,i.particular,i.rmqty,req_qty=(@qty*i.rmqty)/m.fgqty
-- Modified by Prajakta B. on 10/02/2020 for Bug 32929  End
from bomdet i
inner join bomhead m on (m.bomid=i.bomid and m.bomlevel=i.bomlevel)
left join it_mast im on (im.It_code=i.RmitemId and im.it_name=i.Rmitem) -- Added by Prajakta B. on 10/02/2020 for Bug 32929
--Where M.Bomclose = 0 /*TKT-946 Hetal Dt 130410*/ --Commented by Priyanka B on 26032020 for Bug-33332

select * from #bomdet
drop table #bomdet
