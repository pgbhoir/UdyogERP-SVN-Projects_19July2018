DROP PROCEDURE [USP_REP_DBK1]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ajay Jaiswal
-- Create date: 27/06/2011
-- Description:	This Stored procedure is useful to Generate data for DBK 1 Report.
-- =============================================

CREATE PROCEDURE [USP_REP_DBK1]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)= NULL
AS
Declare @FCON as NVARCHAR(2000),@SQLCOMMAND as NVARCHAR(4000),@DIFFDAY as numeric(5)
EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=null,@VEDATE=null
,@VSAC =null,@VEAC =null
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='',@VITFILE='M',@VACFILE=''
,@VDTFLD =''
,@VLYN=Null
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT


SELECT DISTINCT Bomid into #tmpInv FROM BOMHEAD WHERE 1=2
SET @SQLCOMMAND='INSERT INTO #tmpInv SELECT DISTINCT BOMID FROM BOMHEAD INNER JOIN IT_MAST ON (BOMHEAD.ITEMID=IT_MAST.IT_CODE) '+@FCON+' AND BOMLEVEL=0'
EXECUTE SP_EXECUTESQL @SQLCOMMAND

select bomhead.BomId,bomhead.Item,bomhead.fgqty,it_mast.rateper,it_mast.rateunit,it_desc=convert(varchar(200),it_mast.it_desc)
,it_mast.eit_name,bomdet.rmitem,bomdet.rmqty,bomdet.bomlevel,bomdet.isbom,bomdet.srno,bomdet.bomdetid,bomdet.particular
,it.rateper as rm_rateper,it.rateunit as rm_rateunit,rm_desc =CONVERT(varchar(200),it.it_desc)
,bomdet.u_drules,bomdet.u_wastir,bomdet.u_wastrec,bomdet.U_BCOPRO,bomdet.U_SALWAST,bomdet.U_SALECO
Into #tmpBom from bomhead inner join bomdet on (bomhead.bomid=bomdet.bomid and bomhead.bomlevel=bomdet.bomlevel) 
INNER JOIN #tmpInv ON (#tmpInv.BomId=bomhead.BomId)
Inner join it_mast on (it_mast.it_code=bomhead.itemid) 
Inner join it_mast it on (it.it_code=rmitemid)  
Order by bomdet.Bomid,bomdet.bomlevel



/*Add Enternal Columns [Start]*/
ALTER TABLE #tmpBom Add [Level] INT,OrderLevel VARCHAR(100)
/*Add Enternal Columns [End]*/

/*Set Initilize Values of External Columns [Start]*/
UPDATE #tmpBom SET [Level] = 0,OrderLevel = ''
/*Set Initilize Values of External Columns [End]*/

Update #tmpBom SET [Level] = 1,OrderLevel = REPLICATE('0',3-LEN(LTrim(RTrim(STR(srno)))))+LTrim(RTrim(STR(srno))) where Bomlevel=0

declare @BomId Varchar(6), @BomLevel Numeric, @OrderLevel varchar(500),@Bomdetid Numeric
declare @BomId1 Varchar(6), @BomLevel1 Numeric, @OrderLevel1 varchar(500),@Bomdetid1 Numeric,@BomId2 Varchar(6)
Declare @iLevel Int ,@uLevelId Int,@RecCount Int,@BomdId2 varchar(6),@cnt int,@tmp varchar(6)


Declare Outer_Cursor cursor for 
select distinct BomId from #tmpBom
Open Outer_Cursor
Fetch Next from Outer_Cursor Into @BomId2
while @@Fetch_status=0
Begin
	set @iLevel=1
	set @cnt=1
	--print @BomId2 
	while @cnt>0
	Begin
		--print @iLevel
		DECLARE Cur_BomId CURSOR FOR 
		SELECT distinct a.BomId,a.Bomdetid,a.Bomlevel,a.OrderLevel,a.[Level] FROM #tmpBom a
		where a.[Level]=@iLevel and a.Bomid=@BomId2 ORDER By a.Bomid,a.Bomlevel
		OPEN Cur_BomId
		FETCH NEXT FROM Cur_BomId INTO @BomId,@Bomdetid,@BomLevel,@OrderLevel,@uLevelId
		--set @BomdId2=@BomId
		WHILE @@FETCH_STATUS = 0
		BEGIN
				print @bomid2
				print @iLevel				
				Declare cur_Bomdet cursor for
				Select a.BomId,a.Bomdetid,a.Bomlevel,a.OrderLevel FROM #tmpBom a
				where a.BomId=@BomId and a.BomLevel=@Bomdetid and a.[Level]=0 ORDER By a.Bomid,a.Bomlevel
				
				Open cur_Bomdet
				Fetch Next From cur_Bomdet Into @BomId1,@Bomdetid1,@BomLevel1,@OrderLevel1  
				WHILE @@FETCH_STATUS = 0
				BEGIN
					Update #tmpBom set [level]=@iLevel+1,
					OrderLevel=RTrim(@OrderLevel)+'/'+REPLICATE('0',3-LEN(LTrim(RTrim(STR(srno)))))+LTrim(RTrim(STR(srno)))
					Where BomId=@BomId  and BomLevel=@Bomdetid 
				
				Fetch Next From cur_Bomdet Into @BomId1,@Bomdetid1,@BomLevel1,@OrderLevel1  
				END
				CLOSE cur_Bomdet
				DEALLOCATE cur_Bomdet

			FETCH NEXT FROM Cur_BomId INTO @BomId,@Bomdetid,@BomLevel,@OrderLevel,@uLevelId
		END
		CLOSE Cur_BomId
		DEALLOCATE Cur_BomId
		
		set @iLevel=@iLevel+1
		print 'shirk'+convert(varchar(10),@iLevel)
		select @cnt=count(*) from #tmpBom where [level]=@iLevel
		set @cnt=isnull(@cnt,0)
	End

Fetch Next from Outer_Cursor Into @BomId2
End
CLOSE Outer_Cursor
DEALLOCATE Outer_Cursor		

select * from #tmpBom Order by BomId,[level],OrderLevel
drop table #tmpBom
GO
