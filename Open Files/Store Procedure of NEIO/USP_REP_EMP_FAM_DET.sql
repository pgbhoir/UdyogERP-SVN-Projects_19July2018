DROP PROCEDURE [USP_REP_EMP_FAM_DET]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Ramya
-- Create date: 05/03/2012
-- Description:	This is useful for Employee Holiday Master Report
-- Modify date: 
-- Remark:
-- =============================================

CREATE PROCEDURE [USP_REP_EMP_FAM_DET]
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(1000)
AS

Declare @FCON as NVARCHAR(2000)
BEGIN
select EmpName=isnull(EmployeeMast.EmployeeName,''),Emp_Family_Details.*
from Emp_Family_Details  
Left JOIN EmployeeMast ON Emp_Family_Details.EmployeeCode=EmployeeMast.EmployeeCode
--select LOC_DESC=isnull(LOC_MASTER.LOC_DESC,''),EmployeeMast.* 
--from EmployeeMast
--Left JOIN LOC_MASTER ON EmployeeMast.LOC_CODE=LOC_MASTER.LOC_CODE

END
--select * from Emp_Family_Details
GO
