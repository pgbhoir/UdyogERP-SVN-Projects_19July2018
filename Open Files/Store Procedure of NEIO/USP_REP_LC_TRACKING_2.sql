DROP PROCEDURE [USP_REP_LC_TRACKING_2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [USP_REP_LC_TRACKING_2]
 @TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000)
,@SDATE SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60),@SAMT FLOAT
,@EAMT FLOAT,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@XTRAFLDS int
AS

Create Table #LC_TRACKING (Srno Int,Sub_order Varchar(10)
	,Date_val1 SmallDateTime,Date_val2 SmallDateTime,Date_val3 SmallDateTime,Date_val4 SmallDateTime
	,Num_Val1 Numeric(14,2),Num_Val2 Numeric(14,2),Num_Val3 Numeric(14,2),Num_Val4 Numeric(14,2)
	,Str_Val1 Varchar(100),Str_Val2 Varchar(100),Str_Val3 Varchar(100),Str_Val4 Varchar(100)
	,Txt_Val1 Text
	)

Insert into #LC_TRACKING (SRNO,SUB_ORDER,NUM_VAL1,NUM_VAL2,NUM_VAL3,NUM_VAL4,STR_VAL1,STR_VAL2,STR_VAL3)
select 1,'',LC_CUSTCODE, count(lc_no) LC_Count, sum(LC_AMT+LC_AMTEXT)SUM_LC_AMT, sum(LC_balamt)SUM_LC_BALAMT,AC_MAST.AC_NAME,AC_MAST.MAILNAME,CURR_MAST.CURRENCYCD
from EXPORT_LC_MAST
inner join AC_MAST on (EXPORT_LC_MAST.LC_CUSTCODE=AC_MAST.AC_ID)
inner Join CURR_MAST ON (EXPORT_LC_MAST.LC_CURRENCY=CURR_MAST.CURRENCYID)
where AC_MAST.AC_NAME between rtrim(ltrim(@SAC)) and rtrim(ltrim(@EAC))
group by LC_CUSTCODE,AC_MAST.AC_NAME,AC_MAST.MAILNAME,CURR_MAST.CURRENCYCD


select EXPORT_LC_MAST.LC_NO,EXPORT_LC_MAST.LC_DATE,EXPORT_LC_MAST.LC_EXPIRYEXTENDATE,(EXPORT_LC_MAST.LC_AMT+EXPORT_LC_MAST.LC_AMTEXT) AS LC_AMT,STMAIN.FCNET_AMT
,EXPORT_LC_MAST.LC_BALAMT,AC_MAST.AC_NAME,AC_MAST.MAILNAME,STMAIN.INV_NO,cast('' as varchar(100)) as inv_no1, count(inv_no) as inv_no_count 
into #temp
from EXPORT_LC_MAST 
left join STMAIN on (EXPORT_LC_MAST.LC_NO=STMAIN.LC_NO and EXPORT_LC_MAST.LC_CUSTCODE=STMAIN.AC_ID)
left join AC_MAST on (EXPORT_LC_MAST.LC_CUSTCODE = AC_MAST.AC_ID)
where AC_MAST.AC_NAME between rtrim(ltrim(@SAC)) and rtrim(ltrim(@EAC))
group by EXPORT_LC_MAST.LC_NO,EXPORT_LC_MAST.LC_DATE,EXPORT_LC_MAST.LC_EXPIRYEXTENDATE,EXPORT_LC_MAST.LC_AMT,EXPORT_LC_MAST.LC_AMTEXT,STMAIN.FCNET_AMT,EXPORT_LC_MAST.LC_BALAMT,AC_MAST.AC_NAME,AC_MAST.MAILNAME,STMAIN.INV_NO

alter table #temp add recid int identity
--select * from #temp
declare @lc_no varchar(20),@lc_custcode int,@inv_no varchar(50),@recid int
declare @str varchar(1000),@key_fld varchar(100)
select @str='',@key_fld=''

declare cur_temp cursor for 
select lc_no,inv_no,recid from #temp
open cur_temp
fetch next from cur_temp into @lc_no,@inv_no,@recid
while @@fetch_status = 0
begin
	if @key_fld <> @inv_no
	begin
		select @str='',@key_fld=@inv_no
	end
	if @recid >= 1 and @str=''
	begin		
		select @str = coalesce(rtrim(@str)+',','')+rtrim(inv_no)
		from #temp a
		where AC_NAME between @SAC and @EAC and a.lc_no=@lc_no
		group by a.lc_no,a.lc_date,a.lc_amt,a.lc_balamt,a.lc_expiryextendate,a.inv_no
		select @str = right(@str,len(@str)-1)
		update #temp set inv_no1=@str from stmain inner join #temp on (stmain.lc_no=#temp.lc_no) 
		where AC_NAME between @SAC and @EAC and stmain.lc_no=@lc_no and recid=@recid
		set @recid=@recid+1
	end
fetch next from cur_temp into @lc_no,@inv_no,@recid
end
close cur_temp
deallocate cur_temp

Insert into #LC_TRACKING (SRNO,SUB_ORDER,STR_VAL4,DATE_VAL1,DATE_VAL2,NUM_VAL1,NUM_VAL2,NUM_VAL3,STR_VAL1,STR_VAL2,TXT_VAL1,NUM_VAL4)
select 3,'',lc_no,lc_date,lc_expiryextendate,lc_amt,sum(fcnet_amt) as fcnet_amt,lc_balamt,AC_NAME,MAILNAME,inv_no1 as inv_no, count(inv_no) as inv_no_count 
from #temp
group by lc_no,lc_date,lc_expiryextendate,lc_amt,lc_balamt,AC_NAME,MAILNAME,inv_no1

--select 3,'',EXPORT_LC_MAST.LC_NO,EXPORT_LC_MAST.LC_DATE,EXPORT_LC_MAST.LC_EXPIRYEXTENDATE,EXPORT_LC_MAST.LC_AMT,STMAIN.FCNET_AMT
--,EXPORT_LC_MAST.LC_BALAMT,AC_MAST.AC_NAME,AC_MAST.MAILNAME,STMAIN.INV_NO 
--from EXPORT_LC_MAST 
--left join STMAIN on (EXPORT_LC_MAST.LC_NO=STMAIN.LC_NO and EXPORT_LC_MAST.LC_CUSTCODE=STMAIN.AC_ID)
--left join AC_MAST on (EXPORT_LC_MAST.LC_CUSTCODE = AC_MAST.AC_ID)
--where AC_MAST.AC_NAME between rtrim(ltrim(@SAC)) and rtrim(ltrim(@EAC))
--group by EXPORT_LC_MAST.LC_NO,EXPORT_LC_MAST.LC_DATE,EXPORT_LC_MAST.LC_EXPIRYEXTENDATE,EXPORT_LC_MAST.LC_AMT,STMAIN.FCNET_AMT,EXPORT_LC_MAST.LC_BALAMT,AC_MAST.AC_NAME,AC_MAST.MAILNAME,STMAIN.INV_NO

Select * from #LC_TRACKING
order by str_val2,srno
GO
