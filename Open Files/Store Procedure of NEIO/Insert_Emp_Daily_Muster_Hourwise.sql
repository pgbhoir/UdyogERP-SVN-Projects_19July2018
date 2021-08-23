If Exists(Select [Name] From SysObjects Where xType='P' and [Name]='Insert_Emp_Daily_Muster_Hourwise')
Begin
	DROP PROCEDURE [Insert_Emp_Daily_Muster_Hourwise]
end
GO
/****** Object:  StoredProcedure [dbo].[Update_table_column_default_value]    Script Date: 13-01-2020 11:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---Created by Prajakta B. on 13/01/2020 for Employee Shift Details
Create  procedure [dbo].[Insert_Emp_Daily_Muster_Hourwise]
@year varchar(20),@month int
as
begin
	DECLARE @StartDate DATETIME,@EndDate DATETIME
	declare @sqlcommand nvarchar(4000)
	
	select  @StartDate = dateadd(month, @month - 1, dateadd(year, @year - 1900, 0)) ,
			@EndDate = dateadd(month, @month,     dateadd(year, @year - 1900, -1))

	DECLARE @Counter INT, @TotalCount INT,@cntempcd int
			SET @Counter = 0
			SET @TotalCount = DateDiff(DD,@StartDate,@EndDate)
			set @cntempcd=0

	CREATE TABLE #Dates(Dates Datetime)

	select identity(int, 1,1) as srno, EmployeeCode,EmployeeName into #Empmast from EmployeeMast 
	
		WHILE (@Counter <=@TotalCount)
		BEGIN
			DECLARE @DateValue DATETIME
				SET  @DateValue= DATEADD(DD,@Counter,@StartDate)
				
				INSERT INTO #Dates([Dates])
				VALUES(@DateValue)
 
				SET @Counter = @Counter + 1
		END
		
		select * into #EmpDailyMusterHr from #Dates,#Empmast  order by [dates],employeecode

		insert into emp_Daily_Muster_Hourwise 
		select 0 as upload,0 as sel,srno,[dates],employeecode,EmployeeName,0.00 as intime,0.00 as outtime,0.00 as thr,0.00 as swh,0.00 as twh,0.00 as ot,0.00 as bt,@year,DateName( month , DateAdd( month , @month , 0 ) - 1 ) as pay_month,'' as Remark from #EmpDailyMusterHr
		where cast(@Year as varchar)+DateName( month , DateAdd( month , @month , 0 ) - 1 )+EmployeeCode not in 
					(Select cast(isnull(pay_year,0) as varchar)+cast(isnull(pay_month,0) as varchar)+EmployeeCode 
						from emp_Daily_Muster_Hourwise)
		--select * from emp_Daily_Muster_Hourwise
End 
drop table #EmpDailyMusterHr
drop table #dates
drop table #Empmast

--execute Insert_Emp_Daily_Muster_Hourwise '2020',2




