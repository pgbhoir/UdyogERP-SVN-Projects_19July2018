
/****** Object:  StoredProcedure [dbo].[USP_MRP_PLANNING]    Script Date: 27-03-2020 10:56:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************************************/
-- Change History
-- Changed by/on/for : Sachin N. S. on 07/07/2020 for Bug-33526
/******************************************************************************************************/
ALTER PROCEDURE [dbo].[USP_MRP_PLANNING]
@ItName AS Varchar(50),@Qty AS Numeric(18,4),@Ware_nm as varchar(50),@Wharehouseapplicable int 
,@BomId varchar(10)  --Added by Priyanka B on 27042018 for Bugs 31390 & 31306
AS
set Nocount on
Declare @Entry_Ty Varchar(2),@Bcode_nm Varchar(2),@EntryTbl Varchar(2),@SqlCmd NVarchar(4000)
Declare @It_code Numeric(10),@Spl_Cond Varchar(500)
Declare @Itemid Numeric(10),@Item Varchar(100),@FgQty Numeric(18,4), @RmitemId Numeric(10),@Rmitem Varchar(100),@Rmqty Numeric(18,4),@nLevel Int
declare @it_desc varchar(400),@rmit_desc varchar(400)
Declare @ReOrder Numeric(15,4),@Item_Type Varchar(50),@mIt_code Numeric(10)
Declare @PEND_WO Numeric(18,4),@pend_ord Numeric(18,4),@stock_avl Numeric(18,4),@stock_Req Numeric(18,4)

Declare 
--@BomId Varchar(10), --Commented by Priyanka B on 27042018 for Bugs 31390 & 31306
@childCount int,@srno  Int ,@iswareappl integer 

-- Added by Suraj Kumawat for Bug-29249 date on 30-12-2016 Start
 --- Select @Spl_Cond=Case when @Ware_nm<>'' Then 'And i.Ware_nm='''+@Ware_nm+''' ' Else '' End  -- Commented by Suraj for bug-29249
----select @iswareappl =isnull(iswareappl,0) from  lcode  where entry_ty='so' -- Added By Suraj Kumawat for Bug-29249 
Select @iswareappl = isnull(@Wharehouseapplicable,0) -- Added By Suraj Kumawat for Bug-29249 
if ( @iswareappl = 1 )
	begin
		Select @Spl_Cond=' And i.Ware_nm='''+@Ware_nm+''' ' 
	end
else
	begin
		Select @Spl_Cond=Case when @Ware_nm<>'' Then 'And i.Ware_nm='''+@Ware_nm+''' ' Else '' End 
	end

-- Added by Suraj Kumawat for Bug-29249 date on 30-12-2016 End

SELECT IT_CODE,QTY,WARE_NM  INTO #PENDWO FROM ITEM WHERE 1=2

--Select @It_code=It_code,@BomId=u_Bomid1 From It_mast Where It_name=@ItName  --Commented by Priyanka B on 27042018 for Bugs 31390 & 31306

/*		Retrieving Stock Entries	 Start		*/
---select i.It_code,SQtym=i.Qty,SQty=i.Qty,SQtyLabr=i.Qty into #GetStock  from STITEM i  where 1=2  commented for bug-29249 
select i.It_code,i.ware_nm ,SQtym=i.Qty,SQty=i.Qty,SQtyLabr=i.Qty into #GetStock  from STITEM i  where 1=2  -- added for bug-29249 

Insert Into #GetStock Execute USP_MRP_GETSTOCK @It_code,@Spl_Cond
--Fetching Pending work orders

INSERT INTO #PENDWO EXEC USP_MRP_GET_PEND_WO 

print @It_code			--Don't comment the print statement

/*		Fetching all related raw materials		Start	*/

--commented by Shrikant S. on 07/06/2019 for Bug-32556			--Start
-- ;WITH BomDetails AS 
--( 
----initialization 
--Select y.Itemid,y.Item,y.FgQty,x.RmitemId,x.Rmitem,x.Rmqty,0 AS nLevel,x.Srno
--FROM BomDet x
--Inner Join Bomhead y on (x.BomId=y.BomId and x.bomlevel=y.bomlevel)
--WHERE x.BomId =@BomId and x.bomlevel=0
--UNION ALL 
----recursive execution 
--SELECT m.Itemid,m.Item,m.FgQty,d.RmitemId,d.Rmitem,d.Rmqty,nLevel+1,d.Srno
--FROM BomDet d 
--Inner join Bomhead m on (m.BomId=d.BomId and d.Bomlevel=m.Bomlevel)
--Inner Join BomDetails b ON b.RmitemId = m.itemId
--) 
--SELECT * Into #Bom FROM BomDetails Order by nLevel,srno
--commented by Shrikant S. on 07/06/2019 for Bug-32556			--End

--Added by Shrikant S. on 07/06/2019 for Bug-32556			--Start
--Select y.Itemid,y.Item,y.FgQty,x.RmitemId,x.Rmitem,x.Rmqty,0 AS nLevel,x.Srno,x.Bomid,x.Bomlevel,x.bomdetid
Select y.Itemid,y.Item
,im.it_desc    --Added by Prajakta B. on 27/03/2020 for Bug 32929 
,y.FgQty,x.RmitemId,x.Rmitem
,im1.it_desc as rmit_desc    --Added by Prajakta B. on 27/03/2020 for Bug 32929 
,x.Rmqty,0 AS nLevel,x.Srno,x.Bomid,x.Bomlevel,x.bomdetid
,x.subbomid		-- Added by Sachin N. S. on 06/07/2020 for Bug-33526
Into #Bom
FROM BomDet x
Inner Join Bomhead y on (x.BomId=y.BomId and x.bomlevel=y.bomlevel)
inner join it_mast im on (im.it_code=y.ItemId)
inner join it_mast im1 on (im1.It_code=x.RmitemId)
WHERE 1=2

Insert Into #Bom
Select y.Itemid,y.Item
	,im.it_desc   --Added by Prajakta B. on 27/03/2020 for Bug 32929 
	,y.FgQty,x.RmitemId,x.Rmitem
	,im1.it_desc as rmit_desc   --Added by Prajakta B. on 27/03/2020 for Bug 32929 
	,x.Rmqty,0 AS nLevel,x.Srno,x.Bomid,x.Bomlevel,x.bomdetid
	,x.subbomid		-- Changed by Sachin N. S. on 06/07/2020 for Bug-33526	
FROM BomDet x
Inner Join Bomhead y on (x.BomId=y.BomId and x.bomlevel=y.bomlevel)
inner join it_mast im on (im.it_code=y.ItemId)  --Added by Prajakta B. on 27/03/2020 for Bug 32929 
inner join it_mast im1 on (im1.It_code=x.RmitemId)   --Added by Prajakta B. on 27/03/2020 for Bug 32929 
WHERE x.BomId =@BomId and x.bomlevel=0


Declare @LVL Int, @MCOND INT
SELECT @LVL=0,@MCOND=0 
WHILE @MCOND=0
BEGIN
	IF EXISTS (SELECT BomId FROM Bomhead WHERE quotename(BomId)+quotename(bomlevel)+quotename(itemId) IN (SELECT DISTINCT quotename(BomId)+quotename(bomdetid)+quotename(rmitemId)  FROM #Bom WHERE nLevel=@LVL)) --WHERE LVL=@LVL
	BEGIN
		PRINT @LVL

		--Insert Into #Bom (Itemid,Item,FgQty,RmitemId,Rmitem,Rmqty,nLevel,Srno,Bomid,Bomlevel,bomdetid)--Commented by Prajakta B. on 27-03-2020 for Bug 32929
		--Insert Into #Bom (Itemid,Item,it_desc,FgQty,RmitemId,Rmitem,rmit_desc,Rmqty,nLevel,Srno,Bomid,Bomlevel,bomdetid) --Modified by Prajakta B. on 27-03-2020 for Bug 32929
		--Select x.Itemid,x.Item,im1.it_desc,x.FgQty,y.RmitemId,y.Rmitem,im.it_desc as rmit_desc,y.Rmqty,@LVL+1 AS nLevel,y.Srno,y.Bomid,y.Bomlevel,y.Bomdetid
		Insert Into #Bom (Itemid,Item,it_desc,FgQty,RmitemId,Rmitem,rmit_desc,Rmqty,nLevel,Srno,Bomid,Bomlevel,bomdetid,SubBomId)				-- Changed by Sachin N. S. on 06/07/2020 for Bug-33526	
		Select x.Itemid,x.Item,im1.it_desc,x.FgQty,y.RmitemId,y.Rmitem,im.it_desc as rmit_desc,y.Rmqty,@LVL+1 AS nLevel,y.Srno,
			y.Bomid,y.Bomlevel,y.Bomdetid,y.SubBomId		-- Changed by Sachin N. S. on 06/07/2020 for Bug-33526	
		FROM BomDet y 
			Inner join Bomhead x on (y.BomId=x.BomId and y.Bomlevel=x.Bomlevel)
			inner join it_mast im on (im.it_code=x.ItemId)  --Added by Prajakta B. on 27/03/2020 for Bug 32929 
			inner join it_mast im1 on (im1.It_code=y.RmitemId)   --Added by Prajakta B. on 27/03/2020 for Bug 32929 
		Where quotename(x.BomId)+quotename(x.bomlevel)+quotename(x.itemId) 
		in (Select quotename(BomId)+quotename(bomdetid)+quotename(rmitemId)  from #Bom Where nLevel=@LVL)

		SELECT @LVL=@LVL+1
		print @LVL
	END
	ELSE
	BEGIN
		SELECT @MCOND=1
	END
END
--Added by Shrikant S. on 07/06/2019 for Bug-32556			--End

-- Added by Sachin N. S. on 06/07/2020 for Bug-33526 -- Start

--select * from #bom

;with cte_subbomdet as
(
	Select x.Itemid,x.Item,im1.it_desc,x.FgQty,y.RmitemId,y.Rmitem,im.it_desc as rmit_desc,cast((z.RmQty/z.FgQty)*y.Rmqty as numeric(18,4)) as RmQty,
		z.nLevel+1 AS nLevel,y.Srno, z.Bomid,y.Bomlevel,y.Bomdetid,y.SubBomId
		FROM BomDet y 
			Inner join Bomhead x on (y.BomId=x.BomId and y.Bomlevel=x.Bomlevel)
			inner join it_mast im on (im.it_code=x.ItemId)  
			inner join it_mast im1 on (im1.It_code=y.RmitemId)
			Inner join #Bom z on x.Bomid=z.SUBBOMID and x.BomLevel=0
			 where isnull(z.subbomid,'')<>''
	union all
	Select x.Itemid,x.Item,im1.it_desc,x.FgQty,y.RmitemId,y.Rmitem,im.it_desc as rmit_desc,cast((z.RmQty/z.FgQty)*y.Rmqty as numeric(18,4)) as RmQty,
		z.nLevel+1 AS nLevel,y.Srno, Z.Bomid,y.Bomlevel,y.Bomdetid,y.SubBomId
		From BomDet y 
			Inner join Bomhead x on (y.BomId=x.BomId and y.Bomlevel=x.Bomlevel)
			inner join it_mast im on (im.it_code=x.ItemId)  
			inner join it_mast im1 on (im1.It_code=y.RmitemId)
			inner join cte_subbomdet z on z.SubBomid=x.BomId and x.BomLevel=0
	union all
	Select x.Itemid,x.Item,im1.it_desc,x.FgQty,y.RmitemId,y.Rmitem,im.it_desc as rmit_desc,cast((z.RmQty/z.FgQty)*y.Rmqty as numeric(18,4)) as RmQty,
		z.nLevel+1 AS nLevel,y.Srno,y.Bomid,y.Bomlevel,y.Bomdetid,y.SubBomId
		From BomDet y 
			Inner join Bomhead x on (y.BomId=x.BomId and y.Bomlevel=x.Bomlevel)
			inner join it_mast im on (im.it_code=x.ItemId)  
			inner join it_mast im1 on (im1.It_code=y.RmitemId)
			inner join cte_subbomdet z on z.SubBomid=x.BomId and x.BomLevel=z.BomLevel+1
)
Insert into #Bom
select * from cte_subbomdet
-- Added by Sachin N. S. on 06/07/2020 for Bug-33526 -- End

--select * from #bom

-- Commented by Sachin N. S. on 07/08/2020 for Bug-33526 -- Start
--Added by Prajakta B. on 22/01/2020 for Bug 33202 --Start
--select a.ItemId,a.item
--	,a.it_desc  --Added by Prajakta B. on 27/03/2020 for Bug 32929
--	,b.fgqty,a.rmitemid,a.rmitem
--	,a.rmit_desc  --Added by Prajakta B. on 27/03/2020 for Bug 32929
--	,(a.rmqty/a.FgQty)*b.rmqty as rmqty,a.bomid,a.bomlevel
--	into #bom1
--from #bom a,#bom b
--where a.item=b.Rmitem and a.itemid=b.rmitemid

--select * from #bom1

--update #bom set #bom.Rmqty=b.rmqty
--from #bom 
--	left join #bom1 b on #bom.ItemId=b.ItemId and #bom.item=b.item	
--where #bom.item=b.Item and #bom.itemid=b.ItemId and #bom.RmitemId=b.RmitemId 
--Added by Prajakta B. on 22/01/2020 for Bug 33202  --End
-- Commented by Sachin N. S. on 07/08/2020 for Bug-33526 -- End


/*		Fetching all related raw materials		End		*/
--select * from #bom1

--Select * Into #inventory from inventory Where 1=2

--create table #inventory(item varchar(50) ,order_qty decimal(15,2),rmitem varchar(50),req_qty decimal(15,2),req1_qty decimal(15,2),stock_avl decimal(15,2),pend_order decimal(15,2),reorder decimal(15,2),pending_wo decimal(15,2),indent_qty decimal(15,2),mtype varchar(20))
print 'a'
Select a.Item
	,b.it_desc  --Added by Prajakta B. on 27/03/2020 for Bug 32929
	,a.qty as order_qty,a.Item as rmitem
	,b.it_desc as rmit_desc  --Added by Prajakta B. on 27/03/2020 for Bug 32929
	,a.qty as req_qty,a.qty as stock_avl,a.qty as pend_order,b.reorder
	,a.qty as pending_wo,a.qty as indent_qty,b.[type] as mtype,b.It_code,IsSubBOM =CONVERT(Bit,0),a.Item as pItem,a.it_code as pit_code
	,a.ware_nm --- Added for bug-29249
	,ItemBom=(rtrim(ltrim(a.Item)) + '-' + @BomId)  --Added by Priyanka B on 27042018 for Bugs 31390 & 31306
	Into #inventory 
From Stitem a 
	Inner Join IT_MAST b on (a.It_code=b.It_code) where 1=2

print'b'
print @It_code
--Insert Into #inventory values(@itname,@qty,'',0,0,0,0,0,0,'',@It_code)

print'c'

--Select Itemid,Item,FgQty,RmitemId,Rmitem,Rmqty,nLevel From #Bom Order by nLevel,srno

/*		Retrieving pending order, work order and stock of raw materials		Start	*/

Declare BomdetCur Cursor for
	Select Itemid,Item,it_desc,FgQty,RmitemId,Rmitem,rmit_desc,Rmqty,nLevel,srno From #Bom Order by nLevel,srno  --Modified by Prajakta B. on 27/03/2020 for Bug 32929
	
Open BomdetCur	
Fetch Next From BomdetCur Into @Itemid,@Item,@it_desc,@FgQty,@RmitemId,@Rmitem,@rmit_desc,@Rmqty,@nLevel,@srno  --Modified by Prajakta B. on 27/03/2020 for Bug 32929
While @@Fetch_Status=0
Begin
		
	Select @childCount=COUNT(Itemid) From #Bom Where Item=@Rmitem
		
	--- Added by by Suraj Kumawat for Bug-29249 STart
		--- Select @ReOrder=ReOrder,@Item_Type=[type] FROM IT_MAST WHERE It_code=@RmitemId  --- commented by Suraj Kumawat for Bug-29249
	if ( @iswareappl = 1 )
		begin
			Select @ReOrder= (case when (select re_ord_qty from  ITEM_Warehouse_ReOrder WHERE IT_CODE = @RmitemId AND Ware_Nm =@Ware_nm )  > 0  THEN (select re_ord_qty from  ITEM_Warehouse_ReOrder WHERE IT_CODE = @RmitemId AND Ware_Nm =@Ware_nm )  ELSE   ReOrder END )  ,@Item_Type=[type] FROM IT_MAST WHERE It_code=@RmitemId 
			SELECT @PEND_WO=Isnull(SUM(QTY),0) FROM #PENDWO WHERE it_code=@RmitemId and WARE_NM=@Ware_nm
			select @pend_ord=Isnull(sum(qty-re_qty),0) from poitem where item=@Rmitem  and WARE_NM=@Ware_nm  
		end
	else
		begin
			Select @ReOrder=ReOrder,@Item_Type=[type] FROM IT_MAST WHERE It_code=@RmitemId  
			SELECT @PEND_WO=Isnull(SUM(QTY),0) FROM #PENDWO WHERE it_code=@RmitemId 
			select @pend_ord=Isnull(sum(qty-re_qty),0) from poitem where item=@Rmitem  
		end	
	---- Added by by Suraj Kumawat for Bug-29249 STart		
	print '30122016'
	print @ReOrder
	
	/* Commented by Suraj Kumawat for bug-29249
		SELECT @PEND_WO=Isnull(SUM(QTY),0) FROM #PENDWO WHERE it_code=@RmitemId 
		select @pend_ord=Isnull(sum(qty-re_qty),0) from poitem where item=@Rmitem  
	*/

	Insert Into #GetStock Execute USP_MRP_GETSTOCK @RmitemId,@Spl_Cond
	---select @stock_avl=Isnull(sum(sqtym+sqty+sqtylabr),0) from #GetStock where it_code=@RmitemId  Commented by Suraj Kumawat for Bug-29249
	select @stock_avl=Isnull(sum(sqtym+sqty+sqtylabr),0) from #GetStock where it_code=@RmitemId  AND  Ware_nm=@Ware_nm  -- added by Suraj Kumawat for Bug-29249
	
	Set @stock_Req=Case when @stock_avl < (@Rmqty * @Qty) then (@Rmqty * @Qty)- @stock_avl Else (@Rmqty * @Qty) End 
	print 'a'
	print @Rmqty
	print @Qty
	print @FgQty
	--insert into #inventory values(@ItName,@Qty,@Rmitem, (@Rmqty * @Qty)/@FgQty,@stock_avl,@pend_ord,@reorder,@PEND_WO,0,@Item_Type,@RmitemId,case when @childCount>0 then 1 else 0 end, @Item,@Itemid,@Ware_nm)  --Commented by Priyanka B on 27042018 for Bugs 31390 & 31306
	--insert into #inventory values(@ItName,@Qty,@Rmitem, (@Rmqty * @Qty)/@FgQty,@stock_avl,@pend_ord,@reorder,@PEND_WO,0,@Item_Type,@RmitemId,case when @childCount>0 then 1 else 0 end, @Item,@Itemid,@Ware_nm,@ItName+'-'+@BomId)  --Modified by Priyanka B on 27042018 for Bugs 31390 & 31306  --Commented  by Prajakta B. on 27/03/2020 for Bug 32929
	insert into #inventory values(@ItName,@it_desc,@Qty,@Rmitem,@rmit_desc, (@Rmqty * @Qty)/@FgQty,@stock_avl,@pend_ord,@reorder,@PEND_WO,0,@Item_Type,@RmitemId,case when @childCount>0 then 1 else 0 end, @Item,@Itemid,@Ware_nm,@ItName+'-'+@BomId)  --Modified by Priyanka B on 27042018 for Bugs 31390 & 31306  --Modified  by Prajakta B. on 27/03/2020 for Bug 32929
	print 'b'
	Fetch Next From BomdetCur Into @Itemid,@Item,@it_desc,@FgQty,@RmitemId,@Rmitem,@rmit_desc,@Rmqty,@nLevel,@srno  --Modified  by Prajakta B. on 27/03/2020 for Bug 32929
End
Close BomdetCur
Deallocate BomdetCur
/*		Retrieving pending order, work order and stock of raw materials		End	*/


print 'cc'
--Select * From #inventory
--Update #inventory Set indent_qty=req_qty-pend_order-stock_avl+pending_wo+reorder		--Commented by Shrikant S. on 06/06/2019 for Bug-32507
Update #inventory Set indent_qty=req_qty-pend_order-stock_avl+reorder		--Added by Shrikant S. on 06/06/2019 for Bug-32507
Update #inventory Set indent_qty=0 where indent_qty<0

--select * from #Bom Order by nLevel,srno

print 'dd'
Select Item
,it_desc  --Added  by Prajakta B. on 27/03/2020 for Bug 32929
,rmitem
,rmit_desc --Added  by Prajakta B. on 27/03/2020 for Bug 32929
,req_qty,stock_avl,pend_order,reorder ,pending_wo,indent_qty,mtype,It_code,IsSubBOM,pItem,order_qty,ware_nm=@ware_nm,pit_code 
,ItemBom  --Added by Priyanka B on 27042018 for Bugs 31390 & 31306
from #inventory



Drop Table #GetStock
Drop table #inventory






