DROP PROCEDURE [Usp_Rep_TDS_Limit_Alert]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- AUTHOR:		
-- CREATE DATE: 
-- DESCRIPTION:	THIS STORED PROCEDURE IS USEFUL TO GENERATE TDS Alert Limit REPORT. It will Consider only Special Limit From Account Master and actual Data. It will not Considere and Display TDS limits from TDS Master.
-- Modified BY/Date/Reason : Rupesh Prajapati. 15/03/2010. Chenged the Whole Procedure because it was giving wrong output. Make some Column Level Changes in RPT file also.
-- REMARK:
-- =============================================

create PROCEDURE  [Usp_Rep_TDS_Limit_Alert]
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
begin

SET QUOTED_IDENTIFIER OFF
DECLARE @FCON AS NVARCHAR(2000),@VSAMT DECIMAL(14,4),@VEAMT DECIMAL(14,4)
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
,@VMAINFILE='m',@VITFILE=' ',@VACFILE=' '
,@VDTFLD ='pinvdt'
,@VLYN =NULL
,@VEXPARA=NULL
,@VFCON =@FCON OUTPUT
PRINT @FCON 

/*Records that have date not between Ac_mast special Limit-->*/
select entry_ty,inv_no,tran_cd,exstat=case when pinvdt between  TDS_EX_FDT and TDS_EX_TDT then 1 else 2 end
--m.ac_id,inv_no,pinvdt,TDS_EX_FDT,TDS_EX_TDT
into #t1
from tdsmain_vw m inner JOIN AC_MAST_TDS ON (m.AC_ID=AC_MAST_TDS.AC_ID)
where date<=@edate 
order by tran_cd

select distinct entry_ty,tran_cd,exstat,inv_no into #t2 from #t1 where not entry_ty+cast(tran_cd as varchar) in (select distinct entry_ty+cast(tran_cd as varchar) from #t1 where exstat=1)

/*<--Records that have date not between Ac_mast special Limit*/

--select * from #t2
select m.entry_ty,m.tran_cd,m.ac_id,ac_mast.ac_name,m.svc_cate,pinvdt ,Tot_TDS=m.tdsamt+m.scamt+m.ecamt+m.hcamt , m.net_AMT,exstat=3
into #tbl1 
from tdsmain_Vw m  
inner JOIN AC_MAST ON (AC_MAST.AC_ID=m.AC_ID)  
WHERE 1=2


DECLARE @SQLCOMMAND NVARCHAR(4000)
SET @SQLCOMMAND=' '
SET @SQLCOMMAND=@SQLCOMMAND+' '+'insert into #tbl1 select m.entry_ty,m.tran_cd,m.ac_id,ac_mast.ac_name,m.svc_cate,pinvdt'
SET @SQLCOMMAND=@SQLCOMMAND+' '+',Tot_TDS=m.tdsamt+m.scamt+m.ecamt+m.hcamt'
SET @SQLCOMMAND=@SQLCOMMAND+' '+',m.net_AMT,exstat=isnull(#t2.exstat,0)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'from tdsmain_Vw m '
SET @SQLCOMMAND=@SQLCOMMAND+' '+'inner JOIN AC_MAST ON (AC_MAST.AC_ID=m.AC_ID) '
SET @SQLCOMMAND=@SQLCOMMAND+' '+'LEFT JOIN #t2 ON (#t2.entry_ty=m.entry_ty and #t2.tran_cd=m.tran_cd)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+@FCON 
SET @SQLCOMMAND=@SQLCOMMAND+' '+'and isnull(m.svc_cate,'''')<>'''' '
PRINT @SQLCOMMAND 
EXECUTE SP_EXECUTESQL @SQLCOMMAND

--select * from #tbl1 
select distinct ac_mast.ac_name,m.svc_cate
,TDS_EX_FDT=case when exstat=0 then ISNULL(AC_MAST_TDS.TDS_EX_FDT,'') else '' end 
,TDS_EX_TDT=case when exstat=0 then ISNULL(AC_MAST_TDS.TDS_EX_TDT,'') else '' end 
,TDS_EXEMPLIMIT=case when exstat=0 then  (CASE WHEN AC_MAST.TDS_LIGNRULE=1 THEN ISNULL(AC_MAST_TDS.TDS_EXEMPLIMIT,0) ELSE ISNULL(AC_MAST.TDS_EXEMPLIMIT,0) END) else 0 end 
,BillAmt=sum(case when pinvdt between AC_MAST_TDS.TDS_EX_FDT and AC_MAST_TDS.TDS_EX_TDT then NET_Amt else 0 end) /*Records that have date between Ac_mast special Limit*/
	+sum(case when ISNULL(AC_MAST_TDS.TDS_EX_FDT,'')='' then NET_Amt else 0 end) /*Records that have don't have any records in Ac_mast*/
	+sum(case when exstat=2 then NET_Amt else 0 end) /*Records that have date not between Ac_mast special Limit*/
,Tot_TDS=sum(case when pinvdt between AC_MAST_TDS.TDS_EX_FDT and AC_MAST_TDS.TDS_EX_TDT then Tot_TDS else 0 end)
	+sum(case when ISNULL(AC_MAST_TDS.TDS_EX_FDT,'')='' then Tot_TDS else 0 end)
	+sum(case when exstat=2 then Tot_TDS else 0 end)
from #tbl1 m
inner JOIN AC_MAST ON (AC_MAST.AC_ID=m.AC_ID) 
LEFT JOIN AC_MAST_TDS ON (m.AC_ID=AC_MAST_TDS.AC_ID)
group by ac_mast.ac_name,m.svc_cate,exstat
,AC_MAST_TDS.Tds_Ex_FDt,AC_MAST_TDS.Tds_Ex_TDt,AC_MAST.Tds_lIgnRule,AC_MAST_TDS.TDS_ExempLimit,AC_MAST.TDS_ExempLimit


drop table #t1
drop table #t2

--,case when exstat=0 then ISNULL(AC_MAST_TDS.TDS_EX_FDT,'') else '' end 
--,case when exstat=0 then ISNULL(AC_MAST_TDS.TDS_EX_TDT,'') else '' end 
--,case when exstat=0 then  (CASE WHEN AC_MAST.TDS_LIGNRULE=1 THEN ISNULL(AC_MAST_TDS.TDS_EXEMPLIMIT,0) ELSE ISNULL(AC_MAST.TDS_EXEMPLIMIT,0) END) else 0 end 

/*SET @SQLCOMMAND=@SQLCOMMAND+' '+'select ac_mast.ac_name,m.svc_cate'
SET @SQLCOMMAND=@SQLCOMMAND+' '+',TDS_EX_FDT=case when isnull(#t2.exstat,0)=0 then ISNULL(AC_MAST_TDS.TDS_EX_FDT,'''') else '''' end '
SET @SQLCOMMAND=@SQLCOMMAND+' '+',TDS_EX_TDT=case when isnull(#t2.exstat,0)=0 then ISNULL(AC_MAST_TDS.TDS_EX_TDT,'''') else '''' end '
--SET @SQLCOMMAND=@SQLCOMMAND+' '+',TDS_EX_TDT=ISNULL(AC_MAST_TDS.TDS_EX_TDT,0)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+',TDS_EXEMPLIMIT=case when isnull(#t2.exstat,0)=0 then  CASE WHEN AC_MAST.TDS_LIGNRULE=1 THEN ISNULL(AC_MAST_TDS.TDS_EXEMPLIMIT,0) ELSE ISNULL(AC_MAST.TDS_EXEMPLIMIT,0) END else 0 end '
SET @SQLCOMMAND=@SQLCOMMAND+' '+',Tot_TDS=sum(case when pinvdt between AC_MAST_TDS.TDS_EX_FDT and AC_MAST_TDS.TDS_EX_TDT then (m.tdsamt+m.scamt+m.scamt+.m.hcamt+m.ecamt) else 0 end)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'         +sum(case when ISNULL(AC_MAST_TDS.TDS_EX_FDT,'''')='''' then (m.tdsamt+m.scamt+m.scamt+.m.hcamt+m.ecamt) else 0 end)'
--,TDSONAMT=SUM(case when pinvdt between AC_MAST_TDS.TDS_EX_FDT and AC_MAST_TDS.TDS_EX_TDT then m.TDSONAMT else 0 end)
--,TDSONAMT1=SUM(case when ISNULL(AC_MAST_TDS.TDS_EX_FDT,'')='' then m.TDSONAMT else 0 end)
SET @SQLCOMMAND=@SQLCOMMAND+' '+',TDSONAMT=SUM(case when pinvdt between AC_MAST_TDS.TDS_EX_FDT and AC_MAST_TDS.TDS_EX_TDT then m.net_AMT else 0 end)
          +SUM(case when ISNULL(AC_MAST_TDS.TDS_EX_FDT,'''')='''' then m.net_AMT else 0 end)'
--SET @SQLCOMMAND=@SQLCOMMAND+' '+',exstat=isnull(#t2.exstat,0)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'from tdsmain_Vw m '
SET @SQLCOMMAND=@SQLCOMMAND+' '+'inner JOIN AC_MAST ON (AC_MAST.AC_ID=m.AC_ID) '
SET @SQLCOMMAND=@SQLCOMMAND+' '+'LEFT JOIN AC_MAST_TDS ON (AC_MAST.AC_ID=AC_MAST_TDS.AC_ID)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'LEFT JOIN #t2 ON (#t2.entry_ty=m.entry_ty and #t2.tran_cd=m.tran_cd)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+@FCON 
SET @SQLCOMMAND=@SQLCOMMAND+' '+'and isnull(m.svc_cate,'''')<>'''' '
SET @SQLCOMMAND=@SQLCOMMAND+' '+'group by '
SET @SQLCOMMAND=@SQLCOMMAND+' '+'ac_mast.ac_name,m.svc_cate,AC_MAST.Tds_lIgnRule,AC_MAST_TDS.TDS_ExempLimit,AC_MAST.TDS_ExempLimit, isnull(#t2.exstat,0) '
SET @SQLCOMMAND=@SQLCOMMAND+' '+',ISNULL(AC_MAST_TDS.TDS_EX_FDT,'''')'
SET @SQLCOMMAND=@SQLCOMMAND+' '+',ISNULL(AC_MAST_TDS.TDS_EX_TDT,'''') '
SET @SQLCOMMAND=@SQLCOMMAND+' '+'order by ac_mast.ac_name'*/


/*SET @SQLCOMMAND=@SQLCOMMAND+' '+'SELECT ac_mast.ac_name,TDSONAMT=SUM(tds_vw.TDSONAMT),AC_MAST.I_TAX,tds_vw.TDS_TP AS EP_TDS_TP,tds_vw.SC_TP AS EP_SC_TP,tds_vw.HC_TP AS EP_HC_TP,tds_vw.EC_TP AS EP_EC_TP,SUM((tds_vw.TDSAMT+tds_vw.SCAMT+tds_vw.HCAMT+tds_vw.ECAMT)) AS TDSAMT,AC_MAST.SVC_CATE,'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'CASE WHEN AC_MAST.TDS_LIGNRULE=1 THEN ISNULL(AC_MAST_TDS.TDS_EXEMPLIMIT,0) ELSE ISNULL(AC_MAST.TDS_EXEMPLIMIT,0) END TDS_EXEMPLIMIT,TDS_EX_FDT=ISNULL(AC_MAST_TDS.TDS_EX_FDT,0),TDS_EX_TDT=ISNULL(AC_MAST_TDS.TDS_EX_TDT,0),T.SC_APLIMIT'
SET @SQLCOMMAND=@SQLCOMMAND+' '+' FROM tdsmain_Vw tds_vw LEFT JOIN AC_MAST ON (AC_MAST.AC_ID=tds_vw.AC_ID)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'LEFT JOIN AC_MAST_TDS ON (AC_MAST.AC_ID=AC_MAST_TDS.AC_ID)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'LEFT JOIN AC_MAST_TDS TDS ON(tds_vw.TDS_TP=TDS.TDS_TP AND tds_vw.SC_TP=TDS.TDS_SC_TP AND tds_vw.HC_TP=TDS.TDS_HC_TP AND tds_vw.EC_TP=TDS.TDS_EC_TP)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'LEFT JOIN TDSMASTER T ON (T.TDS_CODE=AC_MAST.TDS_CODE AND T.DED_TYPE=AC_MAST.DED_TYPE)'
SET @SQLCOMMAND=@SQLCOMMAND+' '+@FCON 
SET @SQLCOMMAND=@SQLCOMMAND+' '+' AND AC_MAST.SVC_CATE<>'' '' AND tds_vw.TDSAMT<>0'
SET @SQLCOMMAND=@SQLCOMMAND+' '+'GROUP BY ac_mast.ac_name,AC_MAST.I_TAX,tds_vw.TDS_TP,tds_vw.SC_TP,tds_vw.HC_TP,tds_vw.EC_TP,AC_MAST.TDS_LIGNRULE,AC_MAST.TDS_EXEMPLIMIT,AC_MAST_TDS.TDS_EXEMPLIMIT '
SET @SQLCOMMAND=@SQLCOMMAND+' '+',AC_MAST_TDS.TDS_TP ,AC_MAST_TDS.TDS_SC_TP,AC_MAST_TDS.TDS_HC_TP,AC_MAST_TDS.TDS_EC_TP ,AC_MAST.TDS_TP   ,AC_MAST.TDS_SC_TP,AC_MAST.TDS_EC_TP,AC_MAST.TDS_EX_FDT,AC_MAST.TDS_EX_TDT,AC_MAST_TDS.TDS_EX_FDT,AC_MAST_TDS.TDS_EX_TDT,T.SC_APLIMIT ,AC_MAST.SVC_CATE'
PRINT @SQLCOMMAND 
EXECUTE SP_EXECUTESQL @SQLCOMMAND*/
end
GO
