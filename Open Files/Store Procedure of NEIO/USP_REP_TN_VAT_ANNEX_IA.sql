If Exists(Select [Name] from Sysobjects where xType='P' and Id=Object_Id(N'USP_REP_TN_VAT_ANNEX_IA'))
Begin
	Drop Procedure USP_REP_TN_VAT_ANNEX_IA
End
Go

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO


/*
EXECUTE USP_REP_TN_VAT_ANNEX_IA'','','','04/01/2011','03/31/2012','','','','',0,0,'','','','','','','','','2011-2012',''
*/

-- =============================================
-- Author     :	GAURAV R. TANNA
-- Create date: 06/05/2015 : Bug - 26172
-- Description:	This Stored procedure is useful to generate Tamilnadu VAT - FORM Annexure Report - IA.
-- Remark:
-- =============================================
CREATE Procedure [dbo].[USP_REP_TN_VAT_ANNEX_IA]
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
Declare @FCON as NVARCHAR(2000),@fld_list NVARCHAR(2000)
EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=@SDATE,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='M',@VITFILE=Null,@VACFILE='i'
,@VDTFLD ='DATE'
,@VLYN=Null
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

DECLARE @TAXABLE_AMT NUMERIC(14,2),@taxamt numeric(14,2)

set @FCON=rtrim(@FCON)

select part=5,srno1=1,acm.ac_name, ADDRESS=LTRIM(acm.ADD1)+ ' ' + LTRIM(acm.ADD2) + ' ' + LTRIM(acm.ADD3)
,acm.s_tax,commodity_code=space(100),it_desc=itm.it_name,INV_NO=M.INV_NO,p_bldt=M.u_pinvdt,po_no=m.u_pono
, po_dt=m.u_podt,netamt=999999999.99,st.level1,i.taxamt,i.gro_amt,i.qty,itm.p_unit,M.U_TRANSNM, M.U_TRANSADD, M.U_LRNO, M.U_LRDT,date=m.date
,category=space(01)
into #tn_vat_Annexure_IA
from ptitem i  
inner join ptmain m on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd) 
inner join ac_mast acm on (i.ac_id=acm.ac_id) 
inner join it_mast itm on (i.it_code=itm.it_code) 
inner join stax_mas st on (st.tax_name=i.tax_name)
where 1=2

-->Purchase
execute usp_rep_Taxable_Amount_Itemwise 'PT','i',@fld_list =@fld_list OUTPUT
set @fld_list=rtrim(@fld_list)
declare @sqlcommand nvarchar(4000)

Declare @MultiCo	VarChar(3)
Declare @MCON as NVARCHAR(2000)
IF Exists(Select A.ID From SysObjects A Inner Join SysColumns B On(A.ID = B.ID) Where A.[Name] = 'STMAIN' And B.[Name] = 'DBNAME')
	Begin	------Fetch Records from Multi Co. Data
		Set @MultiCo = 'YES'

		
	End
Else
	Begin	------Fetch Records from Single Co. Data
		Set @MultiCo = 'NO'
		
		-->Purchase
		
		set @sqlcommand='insert into #tn_vat_Annexure_IA select part=5,srno1=1'
		set @sqlcommand=@sqlcommand+' '+',acm.ac_name,ADDRESS=RTRIM(LTRIM(acm.ADD1))+ '' '' + RTRIM(LTRIM(acm.ADD2)) + '' '' + RTRIM(LTRIM(acm.ADD3))'
		set @sqlcommand=@sqlcommand+' '+',acm.s_tax,commodity_code=itm.hsncode,itm.it_name,m.inv_no,M.u_pinvdt,m.u_pono,m.u_podt'
		set @sqlcommand=@sqlcommand+' '+',netamt=(i.gro_amt - i.taxamt),st.level1,i.taxamt,i.gro_amt,i.qty,itm.p_unit'
		set @sqlcommand=@sqlcommand+' '+',U_TRANSNM=ISNULL(M.U_TRANSNM,''''), U_TRANSADD=ISNULL(M.U_TRANSADD,''''), M.U_LRNO, M.U_LRDT,date=m.date,category=''J'''
		set @sqlcommand=@sqlcommand+' '+'from ptitem i '
		set @sqlcommand=@sqlcommand+' '+'inner join ptmain m on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join ac_mast acm on (i.ac_id=acm.ac_id)'
		set @sqlcommand=@sqlcommand+' '+'inner join it_mast itm on (i.it_code=itm.it_code)'
		set @sqlcommand=@sqlcommand+' '+'left outer  join stax_mas st on (st.tax_name=i.tax_name And St.Entry_ty = I.Entry_Ty)'
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		set @sqlcommand=@sqlcommand+' '+'and isnull(acm.st_type,'''')=''OUT OF STATE''' +' and st.form_nm in('+'''FORM C'',''C FORM'''+')'
		--set @sqlcommand=@sqlcommand+' '+'group by acm.ac_name,acm.s_tax,st.st_type,itm.hsncode,itm.vatcap,i.tax_name,st.level1,m.u_imporm'
		--set @sqlcommand=@sqlcommand+' '+'order by acm.ac_name,acm.s_tax,st.st_type,itm.hsncode,itm.vatcap,i.tax_name,st.level1,m.u_imporm'
		
		print @sqlcommand
		execute sp_executesql @sqlcommand
		
		set @sqlcommand='insert into #tn_vat_Annexure_IA select part=5,srno1=2'
		set @sqlcommand=@sqlcommand+' '+',acm.ac_name,ADDRESS=RTRIM(LTRIM(acm.ADD1))+ '' '' + RTRIM(LTRIM(acm.ADD2)) + '' '' + RTRIM(LTRIM(acm.ADD3))'
		set @sqlcommand=@sqlcommand+' '+',acm.s_tax,commodity_code=itm.hsncode,itm.it_name,m.inv_no,M.u_pinvdt,m.u_pono,m.u_podt'
		set @sqlcommand=@sqlcommand+' '+',netamt=(i.gro_amt - i.taxamt),st.level1,i.taxamt,i.gro_amt,i.qty,itm.p_unit'
		set @sqlcommand=@sqlcommand+' '+',U_TRANSNM=ISNULL(M.U_TRANSNM,''''), U_TRANSADD=ISNULL(M.U_TRANSADD,''''), M.U_LRNO, M.U_LRDT,date=m.date,category=''S'''
		set @sqlcommand=@sqlcommand+' '+'from ptitem i '
		set @sqlcommand=@sqlcommand+' '+'inner join ptmain m on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd)'
		set @sqlcommand=@sqlcommand+' '+'inner join ac_mast acm on (i.ac_id=acm.ac_id)'
		set @sqlcommand=@sqlcommand+' '+'inner join it_mast itm on (i.it_code=itm.it_code)'
		set @sqlcommand=@sqlcommand+' '+'left outer  join stax_mas st on (st.tax_name=i.tax_name And St.Entry_ty = I.Entry_Ty)'
		set @sqlcommand=@sqlcommand+' '+rtrim(@fcon)
		set @sqlcommand=@sqlcommand+' '+'and isnull(acm.st_type,'''')=''OUT OF STATE''' +' and m.u_imporm in('+'''Stock receipts from Head office / branches / principals outside the state'''+')'
		--set @sqlcommand=@sqlcommand+' '+'group by acm.ac_name,acm.s_tax,st.st_type,itm.hsncode,itm.vatcap,i.tax_name,st.level1,m.u_imporm'
		--set @sqlcommand=@sqlcommand+' '+'order by acm.ac_name,acm.s_tax,st.st_type,itm.hsncode,itm.vatcap,i.tax_name,st.level1,m.u_imporm'
		
		print @sqlcommand
		execute sp_executesql @sqlcommand
		
		
	End
select  * from #tn_vat_Annexure_IA  order by date,srno1