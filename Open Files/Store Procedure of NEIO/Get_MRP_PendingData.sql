
/****** Object:  StoredProcedure [dbo].[Get_MRP_PendingData]    Script Date: 05-09-2020 04:52:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--exec sp_executesql N'Execute Get_MRP_PendingData @Entry_ty,@Sdate,@Edate,@DateOn,@Warehouse,@Wharehouseapplicable',N'@Entry_ty nvarchar(2),@Sdate datetime,@Edate datetime,@DateOn nvarchar(4),@Warehouse nvarchar(4000),@Wharehouseapplicable int',@Entry_ty=N'SO',@Sdate='2019-04-01 00:00:00',@Edate='2020-03-31 00:00:00',@DateOn=N'DATE',@Warehouse=N'',@Wharehouseapplicable=0

ALTER Procedure [dbo].[Get_MRP_PendingData]
@EntryType Varchar(2),@Sdate SmallDateTime,@Edate SmallDateTime,@DateOn Varchar(10),@WareHouse Varchar(100),@Wharehouseapplicable int 
as 
Declare @EntryCode Varchar(2),@Validity Varchar(250),@replVal Varchar(5),@SqlCmd Nvarchar(4000),@TBLNM VARCHAR(10) 
Declare @Due_dt Varchar(20),@lotherfield Varchar(250),@entry_tys2 varchar(2) ,@iswareappl integer 

set @lotherfield=''

----select @iswareappl =isnull(iswareappl,0) from  lcode  where entry_ty='so' -- Added By Suraj Kumawat for Bug-29249 
Select @iswareappl = isnull(@Wharehouseapplicable,0)
--Select  @EntryCode=Case when Bcode_nm<>'' then BCode_nm Else Entry_ty End,@Validity=MRPValidity From LCODE where Entry_ty=@EntryType
Select  @EntryCode=Case when Bcode_nm<>'' then BCode_nm Else (case when ext_vou=1 then '' else Entry_ty End )End 
	From LCODE where Entry_ty=@EntryType

--Added by Priyanka B on 01022020 for Bug-33215 Start
declare @EntryTypes varchar(50)
set @EntryTypes = (select distinct ','+char(39)+entry_ty+char(39)
From LCODE 
	where (bcode_nm=@EntryType or entry_ty=@EntryType) and mrpvalidity=1	-- Changed by Sachin N. S. on 07/09/2020 for Installer 2.3.0
--where (bcode_nm='SO' or entry_ty='SO') and mrpvalidity=1 
FOR XML PATH(''))
select @EntryTypes=substring(@EntryTypes,2,len(@EntryTypes))
--Added by Priyanka B on 01022020 for Bug-33215 End

--set @replVal=char(39)+','+CHAR(39)
--SET @Validity=REPLACE(@Validity,' ',@replVal)
--SET @Validity='('+CHAR(39)+@Validity+char(39)+')'
set @TBLNM=@EntryCode+'MAIN'
print @EntryCode
print @TBLNM

--Added by Shrikant S. on 06/06/2019		--Start		
Declare @RTBLNM as VARCHAR(50),@TBLNAME1 as VARCHAR(50)

	Set @RTBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
					+ (DATEPART(ss, GETDATE()) * 1000 )
					+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
	Set @TBLNAME1 = '##TMP1'+@RTBLNM
--Added by Shrikant S. on 06/06/2019		--End
	

If EXISTS(SELECT B.[NAME] FROM SYSOBJECTS A INNER JOIN SYSCOLUMNS B ON (B.ID=A.ID) WHERE B.[NAME]='Due_dt' AND A.[NAME]=@TBLNM)
BEGIN 
	SET @Due_dt=' b.Due_dt '
END
ELSE
BEGIN
	SET @Due_dt=' 0 as Due_dt '
END

--Select @lotherfield=@lotherfield+isnull(substring((Select ',b.' +fld_nm From LOTHER Where E_code =@EntryType  For XML Path('')),1,250),'')
--Print @lotherfield

--Commented by Priyanka B on 01022020 for Bug-33215 Start
-- Added by Sachin N. S. on 24/10/2019 for Bug-32914 -- Start
/*DECLARE @CSTR VARCHAR(500)  
select @CSTR=''
Select @CSTR=@CSTR+(CASE WHEN ATT_FILE=0 THEN 'A.' ELSE 'B.' END)+'['+RTRIM(FLD_NM)+']' +','
	from LOTHER WHERE E_CODE=@EntryType ORDER BY (CASE WHEN ATT_FILE=1 THEN 1 ELSE 2 END), SERIAL, SUBSERIAL
SELECT @CSTR=CASE WHEN @CSTR='' THEN '' ELSE ','+substring(@CSTR,1,len(@CSTR)-1) END
*/
-- Added by Sachin N. S. on 24/10/2019 for Bug-32914 -- End
--Commented by Priyanka B on 01022020 for Bug-33215 End

--Modified by Priyanka B on 01022020 for Bug-33215 Start
declare @sql nvarchar(Max)
DECLARE @CSTR VARCHAR(max)
select @CSTR=''''
set @sql='DECLARE @CSTR1 VARCHAR(max)
SELECT @CSTR1=''''
SELECT DISTINCT RTRIM(FLD_NM) AS FLD_NM,max(cast(ATT_FILE as int)) as ATT_FILE,max(SERIAL) as SERIAL,max(SUBSERIAL) as SUBSERIAL
INTO #A FROM LOTHER WHERE E_CODE IN ('+@EntryTypes+') GROUP BY FLD_NM

SELECT @CSTR1=@CSTR1+(CASE WHEN ATT_FILE=0 THEN ''A.'' ELSE ''B.'' END)+''[''+RTRIM(FLD_NM)+'']'' +'',''
FROM #A ORDER BY ATT_FILE--,SERIAL, SUBSERIAL
SELECT @CSTR1=CASE WHEN @CSTR1='''' THEN '''' ELSE '',''+substring(@CSTR1,1,len(@CSTR1)-1) END
select cstr = @cstr1 into temp_1
drop table #a'

print @sql
exec sp_executesql @sql
set @cstr=(select cstr from temp_1)
drop table temp_1
--Modified by Priyanka B on 01022020 for Bug-33215 End

--set @SqlCmd='Select Sel=cast(0 as bit),b.Inv_no,b.Date,'+@Due_dt+',a.item,a.qty,a.Qty as AdjustQty,a.ware_nm,a.It_code '--+@lotherfield  --Commented by Prajakta B. on 27/03/2020 For Bug 32929
set @SqlCmd='Select Sel=cast(0 as bit),b.Inv_no,b.Date,'+@Due_dt+',a.item,it.it_desc,a.qty,a.Qty as AdjustQty,a.ware_nm,a.It_code '--+@lotherfield  --Modified by Prajakta B. on 27/03/2020 For Bug 32929
set @SqlCmd=@SqlCmd+' '+',a.Entry_ty,a.Tran_cd,a.itserial'
set @SqlCmd=@SqlCmd+' '+',(select top 1 code_nm from lcode where entry_ty =a.entry_ty ) as [Transaction]'

--Added by Priyanka B on 27042018 for Bugs 31390 & 31306 Start
Declare @TBLNM_ITEM varchar(10)
set @TBLNM_ITEM=@EntryCode+'ITEM'



IF EXISTS(SELECT * FROM SYSCOLUMNS C INNER JOIN SYSOBJECTS O ON (C.ID=O.ID) WHERE O.XTYPE='U' AND O.NAME=@TBLNM_ITEM AND C.[NAME] = 'U_BOMID1')
BEGIN 
	print 1
	set @SqlCmd=@SqlCmd+' '+',BomId = (Case when isnull(a.u_bomid1,'''')<>'''' then a.u_bomid1 else it.u_bomid1 end)' 
	set @SqlCmd=@SqlCmd+' '+',ItemBom = rtrim(ltrim(a.Item)) + ''-'' + rtrim(ltrim((Case when isnull(a.u_bomid1,'''')<>'''' then a.u_bomid1 else it.u_bomid1 end)))' 
	print @SqlCmd
END
ELSE 
BEGIN 
	print 2
	set @SqlCmd=@SqlCmd+' '+',Isnull(it.u_bomid1,'''') as BomId' 
	set @SqlCmd=@SqlCmd+' '+',ItemBom = rtrim(ltrim(a.Item)) + ''-'' + rtrim(ltrim(Isnull(it.u_bomid1,'''')))' 
	print @SqlCmd
END
--Added by Priyanka B on 27042018 for Bugs 31390 & 31306 End
print 3
set @SqlCmd=@SqlCmd+@CSTR		-- Added by Sachin N. S. on 24/10/2019 for Bug-32914

set @SqlCmd=@SqlCmd+' INTO '+ @TBLNAME1							--Added by Shrikant S. on 06/06/2019		
set @SqlCmd=@SqlCmd+' '+'From '+@EntryCode+'main b'
set @SqlCmd=@SqlCmd+' '+'Inner Join '+@EntryCode+'Item a on (a.Tran_cd=b.Tran_cd)'

set @SqlCmd=@SqlCmd+' '+'Inner Join It_mast it on (a.it_code=it.it_code) '  --Added by Priyanka B on 27042018 for Bugs 31390 & 31306

set @SqlCmd=@SqlCmd+' '+'Where a.QTY<>a.RE_QTY  and cast(a.narr as varchar) = '''' and (b.'+@DateOn+' Between '''+Convert(Varchar(50),@Sdate)+''' and '''+Convert(Varchar(50),@Edate)+''') '
---set @SqlCmd=@SqlCmd+' '+' and b.Entry_ty= '''+@EntryType+''' ' --- Commented for Bug-29000
---set @SqlCmd=@SqlCmd+' '+' and b.Entry_ty= '''+@EntryType+''''+  ' and b.tran_cd not in(select tran_cd from mrplog where entry_ty =b.entry_ty and tran_cd=b.tran_cd and itserial=a.itserial)'  ------ Added for Bug-29000 -- 29252
--set @SqlCmd=@SqlCmd+' '+' and a.Entry_ty+Convert(Varchar(10),a.Tran_cd)+a.Itserial Not in (Select Entry_ty+Convert(Varchar(10),Tran_cd)+Itserial From MRPLog) ' ------ Added for Bug-29000
set @SqlCmd=@SqlCmd+' '+' and b.Entry_ty in (select (case when bcode_nm = '''' then entry_ty else entry_ty end)  from lcode where  (case when bcode_nm = '''' then entry_ty else bcode_nm end ) = '+''''+@EntryType+''''+ ' AND ( CASE WHEN (case when bcode_nm = '''' then entry_ty else entry_ty end)= ''SO'' THEN 1 ELSE ISNULL(MRPVALIDITY,0)  END ) = 1)  '
--and b.tran_cd not in(select tran_cd from mrplog where entry_ty =b.entry_ty and tran_cd=b.tran_cd and itserial=a.itserial)'  ---add by Suraj Kumawat for bug-29252 date on 12-12-2016		--commented by Shrikant S. on 06/06/2019 
----set @SqlCmd=@SqlCmd+' '+ Case When @WareHouse<>'' then ' and a.ware_nm='''+@WareHouse+''' ' Else '' End   -- commended by suraj kumawat for bug-29249  
--added by suraj kumawat for bug-29249 
print 4
if ( @iswareappl = 1 )
	begin
		set @SqlCmd=@SqlCmd+' '+ ' and a.ware_nm='''+@WareHouse+''' '
	end
else
	begin
		set @SqlCmd=@SqlCmd+' '+ Case When @WareHouse<>'' then ' and a.ware_nm='''+@WareHouse+''' ' Else '' End 
	end
	
set @SqlCmd=@SqlCmd+' '+ 'Order by b.date,b.inv_no'   --Added by Priyanka B on 01062018 for Bug-31390

Print @SqlCmd
print 5
Execute Sp_ExecuteSql @SqlCmd
print 6
--set @SqlCmd='SELECT * FROM '+@TBLNAME1
--Execute Sp_ExecuteSql @SqlCmd




--Added by Shrikant S. on 06/06/2019		--Start
set @SqlCmd='Delete FROM '+@TBLNAME1+' Where entry_ty+quotename(tran_cd)+itserial in  (select a.entry_ty+quotename(a.tran_cd)+a.itserial from '+@TBLNAME1+' a Inner Join (Select Entry_ty,Tran_cd,Itserial from MrpLog Where rEntry_ty=''WK'' group by Entry_ty,Tran_cd,Itserial) b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.Tran_cd and a.Itserial=b.Itserial) '
set @SqlCmd=@SqlCmd+' '+' Inner Join (Select Entry_ty,Tran_cd,Itserial from MrpLog Where rEntry_ty=''PD''  group by Entry_ty,Tran_cd,Itserial) c on (a.Entry_ty=c.Entry_ty and a.Tran_cd=c.Tran_cd and a.Itserial=c.Itserial) ) '
Execute Sp_ExecuteSql @SqlCmd


set @SqlCmd='Update '+@TBLNAME1+' Set AdjustQty=AdjustQty-Isnull(a.rQty,0) from (Select  Entry_ty,Tran_cd,Itserial,Rqty from MrpLog Where REntry_ty=''WK'') a Inner Join '+@TBLNAME1+' b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.Tran_cd and a.Itserial=b.Itserial)'
Execute Sp_ExecuteSql @SqlCmd

set @SqlCmd='SELECT * FROM '+@TBLNAME1
Execute Sp_ExecuteSql @SqlCmd
--Added by Shrikant S. on 06/06/2019		--End


---select (case when bcode_nm = ' ' then entry_ty else entry_ty end)  from lcode where  (case when bcode_nm = '' then entry_ty else bcode_nm end ) =  'SO'  AND ( CASE WHEN (case when bcode_nm = ' ' then entry_ty else entry_ty end)= 'SO' THEN 1 ELSE ISNULL(MRPVALIDITY,0)  END ) = 1

--Select Sel=cast(0 as bit),b.inv_no,a.date,b.due_dt,a.item,a.qty,b.u_custpo,a.ware_nm
--from somain b 
--left outer join soitem a on(a.tran_cd=b.tran_cd) 
--where a.item+a.inv_no not in (select item+orderno from mrplog) and a.DATE >=  '" + frm + "' and a.date <= '" + todt1 + "' 
--AND a.QTY<>a.RE_QTY  and a.entry_ty in ('" + mrp4 + "') and ware_nm='" + cmbWareHouse.Text + "' order by a.inv_no 

--Execute Get_MRP_PendingData 'SO','04/01/2013','03/31/2017','Due_dt',''


