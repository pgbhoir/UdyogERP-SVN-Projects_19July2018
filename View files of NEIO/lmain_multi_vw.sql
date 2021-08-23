DROP VIEW [lmain_multi_vw]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create VIEW [lmain_multi_vw] AS
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.ARMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, u_nature,l_yn, due_dt, 
		[rule],tax_name,serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,
		compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,TDSPAYTYPE,u_chqdt, U_BRANCH,BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.BPMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,
		compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,TDSPAYTYPE, u_chqdt, U_BRANCH,BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.BRMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt
		,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.CNMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, u_nature,l_yn, due_dt, [rule],tax_name,
		serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,compid,cons_id=ac_id,
		scons_id=0,sac_id=0, SPACE(10) AS u_broker, TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.CPMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,compid,
		cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS BANK_NM
		,'' AS U_BRANCH
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.CRMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,  party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.DCMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt
		,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.DNMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, pinvno, pinvdt, narr, space(1) as u_nature,l_yn, due_dt, [rule],tax_name,serty,
		'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,compid,cons_id=ac_id,
		scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.EPMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,''
		AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.EQMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,''
		AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.ESMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.IIMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,
		'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM		
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.IPMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.IRMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, u_nature,l_yn, due_dt, [rule],tax_name,
		serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,compid,cons_id=ac_id,
		scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.JVMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, pinvno, pinvdt, narr, space(1) as u_nature,l_yn, due_dt, [rule],tax_name,
		space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,compid,
		cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.OBMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt
		,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.OPMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt
		,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.PCMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,  party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.POMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,  party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, pinvno, pinvdt, narr, space(1) as u_nature,  l_yn, due_dt,[rule],tax_name,
		space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC ,compid,
		cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , u_broker,0 as TDSPAYTYPE
		,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.PTMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.PRMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.SOMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,  party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt, 
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.SQMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt, 
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.SRMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt, 
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt
		,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.SSMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,  party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt, 
		[rule],tax_name,serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,compid,
		cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , u_broker,0 as TDSPAYTYPE
		,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM	
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.STMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id,  party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,TOT_FDISC,compid,
		cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),
		scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id , SPACE(10) AS u_broker,
		0 as TDSPAYTYPE,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.SBMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE
		,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.TRMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,3 as TDSPAYTYPE
		,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM
		,fcexrate,fctaxamt,fcgro_amt,fcnet_amt,fcid,currdesc --Birendra
FROM         dbo.OSMAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
UNION ALL
SELECT  Tran_cd, entry_ty, date, doc_no, ac_id, party_nm, cate, dept, inv_no, inv_sr, gro_amt, net_amt, cheq_no,
		drawn_on,date as cheq_dt, inv_no AS pinvno, date AS pinvdt, narr, space(1) as u_nature,l_yn, due_dt,
		[rule],tax_name,space(1) as serty,'' AS U_IMPORM,TOT_DEDUC,TOT_TAX,TOT_EXAMT,TOT_ADD,TAXAMT,TOT_NONTAX,
		TOT_FDISC ,compid,cons_id=ac_id,scons_id=0,sac_id=0, SPACE(10) AS u_broker,0 as TDSPAYTYPE
		,'1900-01-01 00:00:00.000' AS u_chqdt,'' AS U_BRANCH,'' AS BANK_NM	
		,0 as fcexrate,0 as fctaxamt,0 as fcgro_amt,0 as fcnet_amt,0 as fcid,currdesc --Birendra
FROM         dbo.MAIN m left join (select currdesc,Currencyid from dbo.curr_mast) curr_mast on m.fcid=curr_mast.Currencyid
GO
