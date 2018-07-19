DROP PROCEDURE [USP_REP_EMP_PF_FORM_6A]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- AUTHOR:		RUEPESH PRAJAPATI.
-- CREATE DATE: 06/12/2012
-- DESCRIPTION:	This Stroed Procedure is useful for PF Form 6A Report
-- Modification Date/By/Reason: 
-- Remark: 
-- =============================================
Create PROCEDURE    [USP_REP_EMP_PF_FORM_6A]
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
Begin
	DECLARE @FCON AS NVARCHAR(2000),@SQLCOMMAND NVARCHAR(4000)
	EXECUTE   USP_REP_FILTCON 
	@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
	,@VSDATE=@SDATE
	,@VEDATE=@EDATE
	,@VSAC =@SAC,@VEAC =@EAC
	,@VSIT=@SIT,@VEIT=@EIT
	,@VSAMT=@SAMT,@VEAMT=@EAMT
	,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
	,@VSCATE =@SCATE,@VECATE =@ECATE
	,@VSWARE =@SWARE,@VEWARE  =@EWARE
	,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
	,@VMAINFILE='p',@VITFILE='',@VACFILE=''
	,@VDTFLD ='MnthLastDt'
	,@VLYN =NULL
	,@VEXPARA=@EXPARA
	,@VFCON =@FCON OUTPUT
	
	Declare @tDate SmallDateTime
	Select Part=1,em.EmployeeCode,em.pMailName,em.PFNO,lm.pf_code,p.BasicAmt,p.PFEmpE,p.PFEmpR,p.EPSAmt,Refund=p.EPSAmt,p.VEPFAmt,Pay_Year=2012,Pay_Month=3,p.PFAdChg,p.EDLIContr,p.EDLIAdChg into #PF6A From Emp_Monthly_Payroll p inner join EmployeeMast em on (em.EmployeeCode=p.EmployeeCode)   Left Join loc_master lm on (em.loc_code=lm.loc_code) Where 1=2


	Insert into #PF6A
	Select Part=1,em.EmployeeCode,pMailName=(Case when isnull(em.pMailName,'')='' Then em.EmployeeName Else em.pMailName End),em.PFNO,lm.pf_code,sum(p.BasicAmt),sum(p.PFEmpE),sum(p.PFEmpR),sum(p.EPSAmt),Refund=0,Sum(p.VEPFAmt),Pay_Year=0,Pay_Month=0,Sum(p.PFAdChg),Sum(p.EDLIContr),Sum(p.EDLIAdChg)
	From Emp_Monthly_Payroll p inner join EmployeeMast em on (em.EmployeeCode=p.EmployeeCode) 
    Left Join loc_master lm on (em.loc_code=lm.loc_code)  /*Ramya for Pf_Code*/
	Where (p.MnthLastDt BetWeen @sDate and @eDate) and p.PFEmpE+p.VEPFAmt<>0
	Group By em.EmployeeCode,(Case when isnull(em.pMailName,'')='' Then em.EmployeeName Else em.pMailName End),em.PFNO,lm.pf_code
	--EXECUTE USP_REP_EMP_PF_FORM_6A'','','','04/01/2012','03/31/2013','','','','',0,0,'','','','','','','','','2012-2013',''
	Set @tDate=@sDate
	While (@tDate<=@eDate)
	Begin
		Print @tDate
		--Set @tDate=DateAdd(month,1,@tDate)
		Insert into #PF6A

		Select Part=2,EmployeeCode='',pMailName='',PFNO='',lm.PF_Code,BasicAmt=0,sum(p.PFEmpE),sum(p.PFEmpR),sum(p.EPSAmt),Refund=0,Sum(p.VEPFAmt),Year(dateadd(mm,1,m.u_cldt)),Pay_Month =Month(dateadd(mm,1,m.u_cldt)),Sum(p.PFAdChg),Sum(p.EDLIContr),Sum(p	.EDLIAdChg)
		From BpMain m
		inner Join Emp_Monthly_Payroll p on (m.Entry_Ty=p.FH_Ent_Ty and m.Tran_cd=p.FH_Trn_Cd)
		inner join EmployeeMast em on (em.EmployeeCode=p.EmployeeCode) /*Ramya for Pf_Code*/
		Left Join loc_master lm on (em.loc_code=lm.loc_code)  /*Ramya for Pf_Code*/
		Where (Month(@tDate)=Month(dateadd(mm,1,m.u_cldt))) and (Year(@tDate)=Year(dateadd(mm,1,m.u_cldt))) and  isnull(m.PFEmpE,0)+isnull(m.VEPFAmt,0)<>0
		Group By m.u_cldt,lm.PF_Code
		Set @tDate=DateAdd(Month,1,@tDate)
	End

	Select Distinct PF_Code into #pf6A1 From #PF6a
	

	Insert into #PF6A
	Select Part=2,EmployeeCode='',pMailName='',PFNO='',PF_Code,BasicAmt=0,0,0,0,Refund=0,0,9999,Pay_Month =1,0,0,0
	From #PF6A1
		

	Select * From #PF6A Order by pf_code,Part,pMailName,Pay_Year,pay_Month 
	Drop Table #PF6A
End
GO
