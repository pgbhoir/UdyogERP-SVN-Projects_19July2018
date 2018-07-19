DROP PROCEDURE [USP_REP_GETSUPP_BILLS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [USP_REP_GETSUPP_BILLS]
@TMPAC NVARCHAR(60),@TMPIT NVARCHAR(60),@SPLCOND NVARCHAR(500),
@SDATE SMALLDATETIME,@EDATE SMALLDATETIME,
@SNAME NVARCHAR(60),@ENAME NVARCHAR(60),
@SITEM NVARCHAR(60),@EITEM NVARCHAR(60),
@SAMT NUMERIC,@EAMT NUMERIC,
@SDEPT NVARCHAR(60),@EDEPT NVARCHAR(60),
@SCAT NVARCHAR(60),@ECAT NVARCHAR(60),
@SWARE NVARCHAR(60),@EWARE NVARCHAR(60),
@SINVSR NVARCHAR(60),@EINVSR NVARCHAR(60),
@FINYR NVARCHAR(20), @EXTPAR NVARCHAR(60)
AS 
Declare @FCON as NVARCHAR(4000),@SQLCOMMAND as NVARCHAR(4000)

EXECUTE USP_REP_FILTCON 
		@VTMPAC=@TMPAC,@VTMPIT=null,@VSPLCOND=@SPLCOND,
		@VSDATE=null,@VEDATE=@EDATE,
		@VSAC =@SNAME,@VEAC =@ENAME,
		@VSIT=null,@VEIT=null,
		@VSAMT=@SAMT,@VEAMT=@EAMT,
		@VSDEPT=@SDEPT,@VEDEPT=@EDEPT,
		@VSCATE =@SCAT,@VECATE =@ECAT,
		@VSWARE =null,@VEWARE  =null,
		@VSINV_SR =@SINVSR,@VEINV_SR =@EINVSR,
		@VMAINFILE='m1',@VITFILE='it1',@VACFILE='',
		@VDTFLD = 'DATE',@VLYN=null,@VEXPARA=@EXTPAR,
		@VFCON =@FCON OUTPUT

set @SQLCOMMAND=''
set @SQLCOMMAND=@SQLCOMMAND+' '+'Select sEntry_ty=m1.Entry_ty,sTran_cd=m1.Tran_cd,sdate=m1.Date,sInv_Sr=m1.Inv_Sr,sInv_no=m1.Inv_no,sNet_amt=m1.Net_amt,sparty_nm=m1.Party_nm'
set @SQLCOMMAND=@SQLCOMMAND+' '+',sItem=it1.Item,sRate=it1.Rate,sGro_amt=it1.Gro_amt,sItserial=it1.Itserial,m2.Entry_ty,m2.Tran_cd,m2.Date,m2.Inv_Sr,m2.Party_nm'
set @SQLCOMMAND=@SQLCOMMAND+' '+',m2.Inv_no,m2.Net_amt,it2.Item,it2.Rate,it2.Gro_amt,it2.Itserial'
set @SQLCOMMAND=@SQLCOMMAND+' '+'From StItem it1'
set @SQLCOMMAND=@SQLCOMMAND+' '+'Inner Join Stmain m1 on (m1.Tran_cd=it1.Tran_cd and m1.Entry_ty=it1.Entry_ty)'
set @SQLCOMMAND=@SQLCOMMAND+' '+'Inner Join SpDiff on (SpDiff.Tran_cd=it1.Tran_cd and SpDiff.Entry_ty=it1.Entry_ty and SpDiff.Itserial=it1.Itserial)'
set @SQLCOMMAND=@SQLCOMMAND+' '+'Inner Join Stitem it2 on (it2.Tran_cd=SpDiff.pTran_cd and it2.Entry_ty=SpDiff.pEntry_ty and it2.itserial=SpDiff.pItserial)'
set @SQLCOMMAND=@SQLCOMMAND+' '+'Inner Join Stmain m2 on (m2.Tran_cd=it2.Tran_cd and m2.Entry_ty=it2.Entry_ty)'
set @SQLCOMMAND=@SQLCOMMAND+' '+'Inner Join Ac_Mast on (Ac_Mast.Ac_id=m1.Ac_Id)'
set @SQLCOMMAND=@SQLCOMMAND+' '+'Inner Join It_mast on (It_Mast.It_Code=It1.It_code)'
set @SQLCOMMAND=@SQLCOMMAND+' '+@FCON
set @SQLCOMMAND=@SQLCOMMAND+' '+'Order by m1.date,m1.inv_sr,m1.Inv_no,it1.Itserial,m2.date,m2.inv_sr,m2.Inv_no,it2.Itserial'
Execute sp_ExecuteSql @SQLCOMMAND
GO
