DROP VIEW [STKL_VW_MAIN]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [STKL_VW_MAIN]
as
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], u_gcssr, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, inv_no, 
                      0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT,  U_IMPORM,
                       SPACE(1) AS U_TR6, apgen, l_yn,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id, '01/01/1900' AS U_TR6DT, EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,U_LRNO,U_LRDT,U_NT,U_DELI,U_VEHNO,TAX_NAME,TAXAMT,U_TRANDT,U_DRVNAME,U_DRVADD,U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.STMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 As u_gcssr, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, inv_no, 
                      0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT,  U_IMPORM,
                       SPACE(1) AS U_TR6, apgen, l_yn,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.SBMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 As u_gcssr, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, inv_no, 
                      0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT,  U_IMPORM,
                       SPACE(1) AS U_TR6, apgen, l_yn,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.SDMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, Pinvno, Pinvdt, inv_no, 0 AS U_INT, SPACE(1) 
                      AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT,  U_IMPORM, 
                      SPACE(1) AS U_TR6, apgen, l_yn,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.PTMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.ARMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT,  
                      SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.OBMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, U_ARREARS, cheq_no, U_CHALNO, U_CHALDT, 
                       SPACE(1) AS U_IMPORM, u_tr6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.BPMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, U_ARREARS, cheq_no, U_CHALNO, U_CHALDT, 
                       SPACE(1) AS U_IMPORM, u_tr6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.BRMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                      SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.CNMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                      SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.CPMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, Pinvno, Pinvdt, inv_no,0 as u_int, u_arrears, 
                      cheq_no, u_chalno, u_chaldt,  SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,U_LRNO,U_LRDT,U_NT,U_DELI,U_VEHNO,TAX_NAME,TAXAMT,U_TRANDT,U_DRVNAME,U_DRVADD,U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.IIMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT,
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.PCMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT,
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.POMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.SOMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.SQMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule],U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT,  
                      SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.SRMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.DCMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.CRMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.DNMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.EPMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.ESMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule],U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.IPMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, Pinvno, Pinvdt, 
                      inv_no, 0 AS U_INT, U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT,  
                      SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.IRMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT,
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.JVMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.OPMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=(case when isnull(cons_id,0)=0 then ac_id else cons_id end),scons_id=(case when isnull(scons_id,0)=0 then sac_id else scons_id end),sac_id, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.PRMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.SSMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.EQMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       SPACE(1) AS U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.TRMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.OSMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode	--Birendra: Bug-4426(auto transaction) on 14/06/2012
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN  -- Birendra : Bug-18417 on 15/10/2013
FROM         dbo.MAIN
--Added by Priyanka B on 28032019 for Bug-32067 Start
UNION ALL
SELECT     Tran_cd, date, entry_ty, narr, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], 0 AS U_GCSSR, SPACE(1) AS Pinvno, '01/01/1900' AS Pinvdt, 
                      inv_no, 0 AS U_INT, SPACE(1) AS U_ARREARS, cheq_no, SPACE(1) AS U_CHALNO, '01/01/1900' AS U_CHALDT, 
                       U_IMPORM, SPACE(1) AS U_TR6, apgen, l_yn,cons_id=ac_id,scons_id=0,sac_id=0, '01/01/1900' AS U_TR6DT, SPACE(20) AS EXMCLEARTY
					,linked_with,SPACE(1) as BSRCode
					,'' as U_LRNO,'' as U_LRDT,'' as U_NT,'' as U_DELI,'' as U_VEHNO,'' as TAX_NAME,0 as TAXAMT,'' as U_TRANDT,'' as U_DRVNAME,'' as U_DRVADD,'' as U_DRVLICEN
FROM         dbo.IBMAIN
--Added by Priyanka B on 28032019 for Bug-32067 End
GO
