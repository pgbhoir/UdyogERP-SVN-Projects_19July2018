If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_ITC_04_JSON')
Begin
	Drop Procedure USP_REP_ITC_04_JSON
End
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- EXECUTE USP_REP_ITC_04_JSON '04/01/2019','03/31/2020','4A'
-- =============================================

Create PROCEDURE [dbo].[USP_REP_ITC_04_JSON] 
	@SDATE SMALLDATETIME,@EDATE SMALLDATETIME
	,@SECCODE VARCHAR(5)
AS

SELECT PART=0,'AAAAA' as PARTSR,
cast('' as varchar(15)) as ctin,
cast('' as varchar(2)) as jw_stcd,
cast('' as varchar(20)) as chnum,
cast('' as datetime) as chdt,
cast('' as char) as flag,
cast('' as varchar(10)) as goods_ty,
cast('' as varchar(40)) as [desc],
cast('' as varchar(20)) as uqc,
cast(0 as decimal(18,0)) as qty,
cast(0 as decimal(18,0)) as txval,
cast(0 as decimal(18,0)) as tx_i,
cast(0 as decimal(18,0)) as tx_c,
cast(0 as decimal(18,0)) as tx_s,
cast(0 as decimal(18,0)) as tx_cs,
cast('' as varchar(20)) as o_chnum,
cast('' as datetime) as o_chdt,
cast('' as varchar(20)) as jw2_chnum,
cast('' as datetime) as jw2_chdt,
cast('' as varchar(20)) as nat_jw,
cast('' as varchar(20)) as lwuqc,
cast(0 as decimal(18,0)) as lwqty,
cast('' as varchar(20)) as inum,
cast(0 as datetime) as idt
into #ITC04
FROM IIITEM iii 
WHERE 1=2

IF @SECCODE = '4A'
BEGIN
	Insert Into #ITC04 (PART,PARTSR,ctin,jw_stcd,chnum,chdt,flag,goods_ty,[desc],uqc,qty,txval,tx_i,tx_c,tx_s,tx_cs)
	SELECT PART=4,'4A' as PARTSR,
	ctin=ac_mast.GSTIN
	,jw_stcd=ac_mast.statecode,
	chnum=iii.Pinvno,
	chdt=iii.Pinvdt,
	flag='N',
	--goods_ty=(case when it_mast.isservice=0 and it_mast.type <>'Machinery/Stores' then 'Inputs' else 'Capital Goods' end),  --Commented by Priyanka B on 07112019 for Bug-32782
	goods_ty=(case when it_mast.isservice=0 and it_mast.type <>'Machinery/Stores' then '8b' else '7b' end),  --Modified by Priyanka B on 07112019 for Bug-32782
	[desc]=(CASE WHEN ISNULL(IT_MAST.it_alias,'')='' THEN IT_MAST.it_name ELSE IT_MAST.it_alias END),
	uqc=case when iii.QTY>0 then IT_MAST.P_UNIT else '' end,
	qty=iii.QTY,
	txval=iii.u_asseamt,
	--tx_i=iii.igst_amt,	tx_c=iii.cgst_amt,	tx_s=iii.sgst_amt,	tx_cs=iii.compcess  --Commented by Priyanka B on 07112019 for Bug-32782
	tx_i=iii.igst_per,	tx_c=iii.cgst_per,	tx_s=iii.sgst_per,	tx_cs=case when iii.ccessrate='NO-CESS' then 0 else cast(iii.ccessrate as int) end  --Modified by Priyanka B on 07112019 for Bug-32782
	FROM IIITEM iii
	INNER JOIN IIMAIN iim ON  (iii.TRAN_CD=iim.TRAN_CD and iii.entry_ty=iim.entry_ty) 
	INNER JOIN AC_MAST ac_mast ON (ac_mast.AC_ID=iii.AC_ID)
	INNER JOIN IT_MAST IT_MAST ON (iii.IT_CODE=IT_MAST.IT_CODE)
	WHERE (iim.DATE BETWEEN @sdate and @edate)
	and iim.entry_ty in ('LI','I5')
	ORDER BY iii.DATE,iii.INV_NO,iii.ITSERIAL

	--Commented by Priyanka B on 07112019 for Bug-32782 Start
	--IF NOT EXISTS(SELECT PART FROM #ITC04 WHERE PART=4 and PARTSR = '4A')
	--BEGIN
	--	Insert Into #ITC04 (PART,PARTSR,ctin,jw_stcd,chnum,chdt,flag,goods_ty,[desc],uqc,qty,txval,tx_i,tx_c,tx_s,tx_cs)
	--	VALUES(4,'4A','','','','','N','','','',0,0,0,0,0,0)
	--END
	--Commented by Priyanka B on 07112019 for Bug-32782 End
END

IF @SECCODE = '5A'
BEGIN
	Insert Into #ITC04 (PART,PARTSR,ctin,jw_stcd,flag,o_chnum,o_chdt,jw2_chnum,jw2_chdt,nat_jw,[desc],uqc,qty,lwuqc,lwqty)
	SELECT distinct PART=5,'5A' as PARTSR,
	ctin=ac_mast.GSTIN,
	jw_stcd=ac_mast.statecode,
	flag='N',
	o_chnum=IIITEM.Pinvno,
	o_chdt=IIITEM.Pinvdt,
	jw2_chnum=III.inv_no,
	jw2_chdt=III.date,
	--nat_jw='', --Commented by Priyanka B on 22112019 for Bug-32782
	nat_jw=iim.u_nopro,  --Modified by Priyanka B on 22112019 for Bug-32782
	[desc]=(CASE WHEN ISNULL(IT_MAST.it_alias,'')='' THEN IT_MAST.it_name ELSE IT_MAST.it_alias END),
	uqc=case when iii.QTY>0 then IT_MAST.P_UNIT else '' end,
	qty=iii.QTY,
	lwuqc=case when iii.QTY>0 then IT_MAST.P_UNIT else '' end,
	lwqty=irrmdet.wastage
	FROM iritem iii
	INNER JOIN irmain iim ON  (iii.TRAN_CD=iim.TRAN_CD and iii.entry_ty=iim.entry_ty) 
	LEFT JOIN IRRMDET ON (iii.TRAN_CD=IRRMDET.TRAN_CD AND iii.ENTRY_TY=IRRMDET.ENTRY_TY AND iii.ITSERIAL=IRRMDET.itserial)
	left Join IIITEM on(IIITEM.entry_ty=IRRMDET.lientry_ty and IIITEM.Tran_cd=IRRMDET.li_Tran_cd AND IIITEM.ITSERIAL=IRRMDET.LI_ITSER  )
	INNER JOIN AC_MAST ac_mast ON (ac_mast.AC_ID=iii.AC_ID)
	INNER JOIN IT_MAST IT_MAST ON (iii.IT_CODE=IT_MAST.IT_CODE)
	WHERE (iim.DATE BETWEEN @sdate and @edate)
	and iim.entry_ty='LR'
	--ORDER BY iii.DATE,iii.INV_NO,iii.ITSERIAL

	--Commented by Priyanka B on 07112019 for Bug-32782 Start
	--IF NOT EXISTS(SELECT PART FROM #ITC04 WHERE PART=5 and PARTSR = '5A')
	--BEGIN
	--	Insert Into #ITC04 (PART,PARTSR,ctin,jw_stcd,flag,o_chnum,o_chdt,jw2_chnum,jw2_chdt,nat_jw,[desc],uqc,qty,lwuqc,lwqty)
	--	VALUES(5,'5A','','','N','','','','','','','',0,'',0)
	--END
	--Commented by Priyanka B on 07112019 for Bug-32782 End
END

IF @SECCODE = '5B'
BEGIN
	Insert Into #ITC04 (PART,PARTSR,ctin,jw_stcd,flag,o_chnum,o_chdt,jw2_chnum,jw2_chdt,nat_jw,[desc],uqc,qty,lwuqc,lwqty)
	SELECT distinct PART=5,'5B' as PARTSR,
	ctin=ac_mast.GSTIN,
	jw_stcd=ac_mast.statecode,
	flag='N',
	o_chnum=IIITEM.Pinvno,
	o_chdt=IIITEM.Pinvdt,
	jw2_chnum=III.inv_no,
	jw2_chdt=III.date,
	--nat_jw='',  --Commented by Priyanka B on 22112019 for Bug-32782
	nat_jw=iim.u_nopro,  --Modified by Priyanka B on 22112019 for Bug-32782
	[desc]=(CASE WHEN ISNULL(IT_MAST.it_alias,'')='' THEN IT_MAST.it_name ELSE IT_MAST.it_alias END),
	uqc=case when iii.QTY>0 then IT_MAST.P_UNIT else '' end,
	qty=iii.QTY,
	lwuqc=case when iii.QTY>0 then IT_MAST.P_UNIT else '' end,
	lwqty=irrmdet.wastage
	FROM iritem iii
	INNER JOIN irmain iim ON  (iii.TRAN_CD=iim.TRAN_CD and iii.entry_ty=iim.entry_ty) 
	LEFT JOIN IRRMDET ON (iii.TRAN_CD=IRRMDET.TRAN_CD AND iii.ENTRY_TY=IRRMDET.ENTRY_TY AND iii.ITSERIAL=IRRMDET.itserial)
	left Join IIITEM on(IIITEM.entry_ty=IRRMDET.lientry_ty and IIITEM.Tran_cd=IRRMDET.li_Tran_cd AND IIITEM.ITSERIAL=IRRMDET.LI_ITSER  )
	INNER JOIN AC_MAST ac_mast ON (ac_mast.AC_ID=iii.AC_ID)
	INNER JOIN IT_MAST IT_MAST ON (iii.IT_CODE=IT_MAST.IT_CODE) 
	WHERE (iim.DATE BETWEEN @sdate and @edate)
	and iim.entry_ty='R1'

	--Commented by Priyanka B on 07112019 for Bug-32782 Start
	--IF NOT EXISTS(SELECT PART FROM #ITC04 WHERE PART=5 and PARTSR = '5B')
	--BEGIN
	--	Insert Into #ITC04 (PART,PARTSR,ctin,jw_stcd,flag,o_chnum,o_chdt,jw2_chnum,jw2_chdt,nat_jw,[desc],uqc,qty,lwuqc,lwqty)
	--	VALUES(5,'5B','','','N','','','','','','','',0,'',0)
	--END
	--Commented by Priyanka B on 07112019 for Bug-32782 End
END

IF @SECCODE = '5C'
BEGIN
	Insert Into #ITC04 (PART,PARTSR,ctin,jw_stcd,flag,o_chnum,o_chdt,inum,idt,nat_jw,[desc],uqc,qty,lwuqc,lwqty)
	SELECT distinct PART=5,'5C' as PARTSR,
	ctin=ac_mast.GSTIN,
	jw_stcd=ac_mast.statecode,
	flag='N',
	o_chnum=III.u_invnopsp,
	o_chdt=III.u_invdtpsp,
	inum=III.inv_no,
	idt=III.date,
	--nat_jw='',   --Commented by Priyanka B on 22112019 for Bug-32782
	nat_jw=iii.u_nopro,  --Modified by Priyanka B on 22112019 for Bug-32782
	[desc]=(CASE WHEN ISNULL(IT_MAST.it_alias,'')='' THEN IT_MAST.it_name ELSE IT_MAST.it_alias END),
	uqc=case when iii.QTY>0 then IT_MAST.P_UNIT else '' end,
	qty=iii.QTY,
	lwuqc=case when iii.QTY>0 then IT_MAST.P_UNIT else '' end,
	lwqty=0
	FROM stitem iii
	INNER JOIN stmain iim ON  (iii.TRAN_CD=iim.TRAN_CD and iii.entry_ty=iim.entry_ty) 
	INNER JOIN AC_MAST ac_mast ON (ac_mast.AC_ID=iii.AC_ID)
	INNER JOIN IT_MAST IT_MAST ON (iii.IT_CODE=IT_MAST.IT_CODE)
	WHERE (iim.DATE BETWEEN @sdate and @edate)
	and iim.entry_ty='ST' and iii.u_invnopsp<>'' and iii.u_invdtpsp<>''
	
	--Commented by Priyanka B on 07112019 for Bug-32782 Start
	--IF NOT EXISTS(SELECT PART FROM #ITC04 WHERE PART=5 and PARTSR = '5C')
	--BEGIN
	--	Insert Into #ITC04 (PART,PARTSR,ctin,jw_stcd,flag,o_chnum,o_chdt,inum,idt,nat_jw,[desc],uqc,qty,lwuqc,lwqty)
	--	VALUES(5,'5C','','','N','','','','','','','',0,'',0)
	--END
	--Commented by Priyanka B on 07112019 for Bug-32782 End
END

select * from #ITC04
drop table #ITC04