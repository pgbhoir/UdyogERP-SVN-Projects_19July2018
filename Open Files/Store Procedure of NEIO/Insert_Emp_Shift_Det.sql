If Exists(Select [Name] From SysObjects Where xType='P' and [Name]='Insert_EMP_Shift_DET')
Begin
	DROP PROCEDURE [Insert_EMP_Shift_DET]
end
GO
/****** Object:  StoredProcedure [dbo].[Update_table_column_default_value]    Script Date: 13-01-2020 11:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---Created by Prajakta B. on 13/01/2020 for Employee Shift Details
Create  procedure [dbo].[Insert_EMP_Shift_DET]
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

	select EmployeeCode,shift_code into #Empmast from EmployeeMast 
	
		WHILE (@Counter <=@TotalCount)
		BEGIN
			DECLARE @DateValue DATETIME
				SET  @DateValue= DATEADD(DD,@Counter,@StartDate)
				
				INSERT INTO #Dates([Dates])
				VALUES(@DateValue)
 
				SET @Counter = @Counter + 1
		END
		print @month
		select * into #empshiftdet from #Dates,#Empmast order by [dates],employeecode
		select * from #empshiftdet
		insert into emp_shift_det ([date],emp_code,shift_code,Pay_year,pay_month)
		select [dates],employeecode,shift_code,@year,DateName( month , DateAdd( month , @month , 0 ) - 1 ) as pay_month from #empshiftdet
		where cast(@Year as varchar)+DateName( month , DateAdd( month , @month , 0 ) - 1 )+EmployeeCode not in 
					(Select cast(isnull(pay_year,0) as varchar)+cast(isnull(pay_month,0) as varchar)+EmployeeCode 
					from emp_shift_det)

End 
drop table #empshiftdet
drop table #dates
drop table #Empmast

--execute Insert_EMP_Shift_DET '2020',2




