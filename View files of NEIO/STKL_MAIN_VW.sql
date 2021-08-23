DROP VIEW [STKL_MAIN_VW]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [STKL_MAIN_VW]
AS
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.STMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.SBMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.PTMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.ARMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.OBMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.BPMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.BRMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.CNMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.CPMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.IIMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.PCMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.POMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.SQMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.SRMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.DCMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.CRMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.DNMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.EPMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.ESMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.IPMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.IRMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.JVMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.OPMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.PRMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.SSMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.EQMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.TRMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.OSMAIN
UNION ALL
SELECT     Tran_cd, date, entry_ty, doc_no, Ac_id, net_amt, gro_amt, dept, cate, inv_sr, [rule], Pinvno, Pinvdt, inv_no, cheq_no, due_dt
FROM         dbo.MAIN
GO
