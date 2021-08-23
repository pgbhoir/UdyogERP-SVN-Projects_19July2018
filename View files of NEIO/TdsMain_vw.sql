DROP VIEW [TdsMain_vw]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*-- =============================================
-- Author:		
-- Create date: 
-- Description:	This View is useful for TDS and TCS Entries\Reports.
-- Modification Date\By\Reason: 21/01/2011. Rupesh. Add TCS TKT-5692.
-- Modification Date\By\Reason: 17/02/2011. Prasanth. Add UTN TKT-5692.
-- Modification Date\By\Reason: 26/06/2012. Sandeep 'SERBAMT ,SERCAMT, SERHAMT' Added columns for  bug-4838.
-- Modification Date\By\Reason: 17/06/2013 Shrikant S. for Bug-14220
-- Modification Date\By\Reason: 18/07/2013 Shrikant S. for Bug-17628 for fields  ,TDSRATEACT,NAT_REM,ACK15CA,u_nature
-- Modification Date\By\Reason: 06/10/2016 Shrikant S. for GST -- Removed SERBAMT ,SERCAMT, SERHAMT 
-- Modification Date\By\Reason: 20/04/2017 Shrikant S. for GST -- Removed tdsamt,scamt,ecamt,hcamt,tds_tp,sc_tp,ec_tp,hc_tp and Added CTDS_PER,STDS_PER,ITDS_PER,CTDSAMT,STDSAMT,ITDSAMT
-- Remark:
-- =============================================*/
CREATE view [TdsMain_vw] as
select entry_ty,tran_cd,date,date as Pinvdt,inv_no,inv_no as Pinvno,ac_id,tdsonamt,svc_cate,cheq_no,u_chalno,u_bsrcode as bsrcode,u_chaldt,net_amt,u_cldt,u_arrears,tdspaytype,l_yn,tot_examt,utn
,space(1) as TDSRATEACT,space(1) as NAT_REM,space(1) as ACK15CA,u_nature,CTDS_PER,STDS_PER,ITDS_PER,CTDSAMT,STDSAMT,ITDSAMT,tdsamt,scamt,ecamt,hcamt,tds_tp,sc_tp,ec_tp,HC_TP,0 as snet_amt,0 as stot_examt 
from bpmain     
union all
select entry_ty,tran_cd,date,date as Pinvdt,inv_no,inv_no as Pinvno,ac_id,tdsonamt,svc_cate,cheq_no,u_chalno,u_bsrcode as bsrcode,u_chaldt,net_amt,u_cldt,u_arrears,tdspaytype,l_yn,tot_examt,utn='' 
,space(1) as TDSRATEACT,space(1) as NAT_REM,space(1) as ACK15CA,space(1) as u_nature,CTDS_PER,STDS_PER,ITDS_PER,CTDSAMT,STDSAMT,ITDSAMT,tdsamt,scamt,ecamt,hcamt,tds_tp,sc_tp,ec_tp,HC_TP,0 as snet_amt,0 as stot_examt
from cpmain 
union all
select entry_ty,tran_cd,date,Pinvdt,inv_no,Pinvno,ac_id,tdsonamt,svc_cate,space(1) as cheq_no,space(1) as u_chalno,'' as bsrcode,getdate() as u_chaldt,net_amt,date as u_cldt,'' as u_arrears ,0 as tdspaytype,l_yn,tot_examt,utn=''
--,TDSRATEACT,NAT_REM,ACK15CA,space(1) as u_nature,CTDS_PER,STDS_PER,ITDS_PER,CTDSAMT,STDSAMT,ITDSAMT,tdsamt,scamt,ecamt,hcamt,tds_tp,sc_tp,ec_tp,HC_TP,0 as snet_amt,0 as stot_examt		--Commented by Shrikant S. on 21/08/2018 for Installer 2.0.0
,TDSRATEACT,NAT_REM,ACK15CA,space(1) as u_nature,CTDS_PER,STDS_PER,ITDS_PER,CTDSAMT,STDSAMT,ITDSAMT,tdsamt,scamt,ecamt,hcamt,tds_tp,sc_tp,ec_tp,HC_TP,snet_amt,stot_examt					--Added by Shrikant S. on 21/08/2018 for Installer 2.0.0
from epmain
union all
select entry_ty,tran_cd,date,date as Pinvdt,inv_no,inv_no as Pinvno,ac_id,0 as tdsonamt,svc_cate,cheq_no,u_chalno,'' as bsrcode,u_chaldt,net_amt,u_cldt,'' as u_arrears,tdspaytype,l_yn,tot_examt,utn='' 
,space(1) as TDSRATEACT,space(1) as NAT_REM,space(1) as ACK15CA,space(1) as u_nature,CTDS_PER,STDS_PER,ITDS_PER,CTDSAMT,STDSAMT,ITDSAMT,tcsamt as tdsamt,stcsamt as scamt,etcsamt as ecamt,htcsamt as hcamt,tds_tp,sc_tp,ec_tp,HC_TP,0 as snet_amt,0 as stot_examt
from brmain     
union all
select entry_ty,tran_cd,date,date as Pinvdt,inv_no,inv_no as Pinvno,ac_id,0 as tdsonamt,svc_cate,cheq_no,'' as u_chalno,'' as  bsrcode,'' as u_chaldt,net_amt,date as u_cldt,'' as u_arrears,tdspaytype,l_yn,tot_examt,utn='' 
,space(1) as TDSRATEACT,space(1) as NAT_REM,space(1) as ACK15CA,space(1) as u_nature,CTDS_PER,STDS_PER,ITDS_PER,CTDSAMT,STDSAMT,ITDSAMT,tcsamt as tdsamt,stcsamt as scamt,etcsamt as ecamt,htcsamt as hcamt,tds_tp,sc_tp,ec_tp,HC_TP,0 as snet_amt,0 as stot_examt
from crmain 
union all
select entry_ty,tran_cd,date,'' as Pinvdt,inv_no,'' as Pinvno,ac_id,tdsonamt=gro_amt+tot_add+tot_tax,svc_cate,space(1) as cheq_no,space(1) as u_chalno,'' as bsrcode,getdate() as u_chaldt,net_amt,date as u_cldt,'' as u_arrears ,0 as tdspaytype,l_yn,tot_examt,utn=''
,space(1) as TDSRATEACT,space(1) as NAT_REM,space(1) as ACK15CA,space(1) as u_nature,CTDS_PER,STDS_PER,ITDS_PER,CTDSAMT,STDSAMT,ITDSAMT,tcsamt as tdsamt,stcsamt as scamt,0 as ecamt,0 as hcamt,tds_tp,sc_tp,ec_tp,HC_TP,0 as snet_amt,0 as stot_examt
from stmain
union all
select entry_ty,tran_cd,date,Pinvdt,inv_no,Pinvno,ac_id,tdsonamt,svc_cate,space(1) as cheq_no,space(1) as u_chalno,'' as bsrcode,getdate() as u_chaldt,net_amt,date as u_cldt,'' as u_arrears ,0 as tdspaytype,l_yn,tot_examt,utn=''
,space(1) as TDSRATEACT,space(1) as NAT_REM,space(1) as ACK15CA,space(1) as u_nature,0 AS CTDS_PER,0 AS STDS_PER,0 AS ITDS_PER,0 AS CTDSAMT,0 AS STDSAMT,0 AS ITDSAMT,tdsamt,scamt,ecamt,hcamt,tds_tp,sc_tp,ec_tp,HC_TP,snet_amt,stot_examt
from PTmain
GO
