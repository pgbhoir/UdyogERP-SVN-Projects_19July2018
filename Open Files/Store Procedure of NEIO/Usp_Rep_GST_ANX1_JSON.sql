IF EXISTS(SELECT * FROM SYSOBJECTS WHERE XTYPE='P' AND [NAME]='Usp_Rep_GST_ANX1_JSON')
BEGIN
	DROP PROCEDURE [Usp_Rep_GST_ANX1_JSON]
END
GO
/****** Object:  StoredProcedure [dbo].[Usp_Rep_GST_ANX1_JSON]    Script Date: 14-12-2019 14:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Created by Prajakta B. 
-- Created Date 27-12-2019
-- EXECUTE Usp_Rep_GST_ANX1_JSON '04/01/2019','03/31/2020','ALL','Normal',0
-- =============================================

Create PROCEDURE [dbo].[Usp_Rep_GST_ANX1_JSON] 
	@SDATE SMALLDATETIME,@EDATE SMALLDATETIME
	,@SECCODE VARCHAR(8),@Annx varchar(20),@Summary bit
AS

SELECT PART=0,'AAAAA' as PARTSR,
cast('' as varchar(15)) as ctin,
cast('' as varchar(25)) as doctyp,
cast('' as varchar(2)) as sec7act,
cast('' as varchar(2)) as rfndelg,
cast('' as varchar(2)) as pos,
cast('' as varchar(16)) as num,
cast('' as datetime) as dt,
cast(0 as decimal(17,2)) as val,  
cast('' as varchar(15)) as hsn,
cast(0 as decimal(18,0)) as txval,
cast(0 as decimal(18,0)) as igst,
cast(0 as decimal(18,0)) as cgst,
cast(0 as decimal(18,0)) as sgst,
cast(0 as decimal(18,0)) as cess,
cast(0 as decimal(18,2)) as rate,
cast('' as datetime) as sbdt,
cast('' as varchar(50)) as sbnum,
cast('' as varchar(50)) as pcode,
cast('' as varchar(10)) as [type],
cast('' as varchar(20)) as Ecomgstin,
cast(0 as bit) as clmrfnd,
cast(0 as decimal(18,0)) as sup,
cast(0 as decimal(18,0)) as supr,
cast('' as varchar(10)) as seccode
into #GSTANNX1
FROM GSTR1_VW i 
WHERE 1=2

SELECT DISTINCT HSN_CODE, CAST(HSN_DESC AS VARCHAR(250)) AS HSN_DESC INTO #HSN FROM HSN_MASTER

SELECT A.entry_ty,A.ST_TYPE,A.gstin,A.SUPP_TYPE, '' as sec7act, '' as rfndelg,a.pos_std_cd,A.INV_NO,A.[DATE],A.GRO_AMT,
	   A.hsncode,A.Taxableamt,A.IGST_AMT,A.CGST_AMT,A.SGST_AMT,A.cess_amt,
	   rate= (case when (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0)) >0  then (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0))
					when (isnull(A.IGST_PER,0)) > 0 then (isnull(A.IGST_PER,0)) else isnull(A.gstrate,0) end),
		LineRule,SGSRT_AMT,CGSRT_AMT,IGSRT_AMT,cessr_amt,A.SBBILLNO,A.SBDATE,a.portcode,Ecomgstin = isnull(ac.gstin,''),a.isaugstref
INTO #GSTJSONANX1TBL 
FROM [GSTR1_VW] A
left join ac_mast ac on (A.ecomac_id=ac.ac_id)
inner join lcode l  on (a.entry_ty=l.entry_ty)
WHERE (A.DATE BETWEEN @SDATE AND @EDATE) and A.AMENDDATE=''	
print 1
SELECT A.entry_ty,A.ST_TYPE,A.gstin,A.SUPP_TYPE, '' as sec7act, '' as rfndelg,a.pos_std_cd,A.INV_NO,A.[DATE],A.GRO_AMT,
	   A.hsncode,A.Taxableamt,A.IGST_AMT,A.CGST_AMT,A.SGST_AMT,A.cess_amt,
	   rate= (case when (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0)) >0  then (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0))
					when (isnull(A.IGST_PER,0)) > 0 then (isnull(A.IGST_PER,0)) else isnull(A.gstrate,0) end),
		LineRule,SGSRT_AMT,CGSRT_AMT,IGSRT_AMT,cessr_amt,A.SBBILLNO,A.SBDATE,a.portcode,Ecomgstin = isnull(ac.gstin,''),a.isaugstref
INTO #GSTJSONANX1AMD 
FROM [GSTR1_VW] A
left join ac_mast ac on (A.ecomac_id=ac.ac_id)
inner join lcode l  on (a.entry_ty=l.entry_ty)
WHERE (A.AMENDDATE BETWEEN @SDATE AND @EDATE) AND A.HSNCODE <> ''
print 3
/*3A.  Supplies made to consumers and un-registered persons (Net of debit / credit notes ) */
IF @SECCODE = 'B2C' or  @SECCODE = 'All'
BEGIN
Insert Into #GSTANNX1(PART,PARTSR,ctin,doctyp,sec7act,rfndelg,pos,num,dt,val,hsn,txval,igst,cgst,sgst,cess,rate,sbdt,sbnum,pcode,[type],Ecomgstin,clmrfnd,sup,supr,seccode)
Select * 
from (
		select PART=3 ,'3A' AS PARTSR,gstin as ctin,SUPP_TYPE as doctyp,'N' as sec7act,
		'Y' as rfndelg,pos_std_cd as POS,INV_NO as num,[date] as dt,sum(GRO_AMT) as val,hsncode as hsn,txval =sum(taxableamt),
		igst = sum(IGST_AMT),cgst = sum(CGST_AMT),sgst = sum(SGST_AMT),cess_amt = sum(cess_amt),rate,SBDATE as sbdt,SBBILLNO as sbnum,
		portcode as pcode,'' as [type],'' as Ecomgstin,isaugstref as clmrfnd,0 as sup,0 as supr,'B2C' as seccode
		from #GSTJSONANX1TBL  
		where (Entry_ty in ('ST','SB','CN','DN','SR') and entry_ty<>'UB')
		and supp_type IN ('UnRegistered','')  and st_type <> 'Out of country' 
		group by pos_std_cd,rate,inv_no,[date],gstin,hsncode,Entry_ty,SUPP_TYPE,SBDATE,SBBILLNO,portcode,isaugstref
	) aa

Order by ctin,dt,num,rate
end 
/*	3B. Supplies made to registered persons (other than those attracting reverse charge)(including edit/amendment)	*/
IF @SECCODE = 'B2B'  or  @SECCODE = 'All' and @Annx Not in ('SAHAJ')
BEGIN
	print '3b'
	Insert Into #GSTANNX1(PART,PARTSR,ctin,doctyp,sec7act,rfndelg,pos,num,dt,val,hsn,txval,igst,cgst,sgst,cess,rate,sbdt,sbnum,pcode,[type],Ecomgstin,clmrfnd,sup,supr,seccode)
	SELECT 3 AS PART ,'3B' AS PARTSR,gstin as ctin,
		case when Entry_ty='ST' then 'I' when Entry_ty='GC' then 'C' when Entry_ty='GD' then 'D' end as doctyp,
		'N' as sec7act,'Y' as rfndelg,pos_std_cd as POS,INV_NO as num,[date] as dt,sum(GRO_AMT) as val,hsncode as hsn,txval =sum(taxableamt),
		igst = sum(IGST_AMT),cgst = sum(CGST_AMT),sgst = sum(SGST_AMT),cess_amt = sum(cess_amt),rate,SBDATE as sbdt,SBBILLNO as sbnum,
		portcode as pcode,'' as [type],'' as Ecomgstin,isaugstref as clmrfnd,0 as sup,0 as supr,'B2B' as seccode
		from #GSTJSONANX1TBL
		where (Entry_ty in ('ST','SB') and entry_ty<>'UB') and st_type <> 'Out of country' 
		and supp_type IN ('Registered','Compounding') 
		and gstin <>'' AND LineRule = 'Taxable' AND HSNCODE <> '' 
		and (SGSRT_AMT + CGSRT_AMT  + IGSRT_AMT + cessr_amt) = 0  and (SGST_AMT + CGST_AMT  + IGST_AMT + cess_amt) > 0
		group by gstin,pos_std_cd,inv_no,date,rate,hsncode,Entry_ty,SBDATE,SBBILLNO,portcode ,isaugstref
		Order by gstin,Date,Inv_no,Rate
End 
/*	3C. Exports with payment of tax	*/
IF @SECCODE = 'EXP'  or  @SECCODE = 'All'and @Annx Not in ('SAHAJ','SUGAM')
BEGIN
	print '3c'
	Insert Into #GSTANNX1(PART,PARTSR,ctin,doctyp,sec7act,rfndelg,pos,num,dt,val,hsn,txval,igst,cgst,sgst,cess,rate,sbdt,sbnum,pcode,[type],Ecomgstin,clmrfnd,sup,supr,seccode)
	SELECT 3 AS PART ,'3C' AS PARTSR,gstin as ctin,
		case when Entry_ty='ST' then 'I' when Entry_ty='GC' then 'C' when Entry_ty='GD' then 'D' end as doctyp,
		'N' as sec7act,'Y' as rfndelg,pos_std_cd as pos,INV_NO as num,[date] as dt,sum(GRO_AMT) as val,hsncode as hsn,txval =sum(taxableamt),
		igst = sum(IGST_AMT),cgst = sum(CGST_AMT),sgst = sum(SGST_AMT),cess_amt = sum(cess_amt),rate,SBDATE as sbdt,SBBILLNO as sbnum,
		portcode as pcode,'' as [type],'' as Ecomgstin,isaugstref as clmrfnd,0 as sup,0 as supr,'expwp' as seccode
		from #GSTJSONANX1TBL
		where (Entry_ty in ('ST','SB') and entry_ty<>'UB') and st_type ='Out of country' 
		and supp_type IN ('')  
		and gstin ='' AND LineRule = 'Taxable' AND HSNCODE <> '' 
		and (SGSRT_AMT + CGSRT_AMT  + IGSRT_AMT + cessr_amt) = 0  and (SGST_AMT + CGST_AMT  + IGST_AMT + cess_amt) > 0
		group by gstin,inv_no,[date],pos_std_cd,rate,hsncode,SBBILLNO,SBDATE,Entry_ty,portcode,isaugstref 
		Order by gstin,[DATE],INV_NO,rate

/*	3D. Exports without payment of tax	*/
	Insert Into #GSTANNX1(PART,PARTSR,ctin,doctyp,sec7act,rfndelg,pos,num,dt,val,hsn,txval,igst,cgst,sgst,cess,rate,sbdt,sbnum,pcode,[type],Ecomgstin,clmrfnd,sup,supr,seccode)
	SELECT 3 AS PART ,'3D' AS PARTSR,gstin as ctin,
		case when Entry_ty='ST' then 'I' when Entry_ty='GC' then 'C' when Entry_ty='GD' then 'D' end as doctyp,
		'N' as sec7act,'Y' as rfndelg,pos_std_cd as pos,INV_NO as num,[date] as dt,sum(GRO_AMT) as val,hsncode as hsn,txval =sum(taxableamt),
		igst = sum(IGST_AMT),cgst = sum(CGST_AMT),sgst = sum(SGST_AMT),cess_amt = sum(cess_amt),rate,SBDATE as sbdt,SBBILLNO as sbnum,
		portcode as pcode,'' as [type],'' as Ecomgstin,isaugstref as clmrfnd,0 as sup,0 as supr,'expwop' as seccode
		from #GSTJSONANX1TBL
		where (Entry_ty in ('ST','SB') and entry_ty<>'UB') and st_type ='Out of country' 
		and supp_type IN ('') and gstin ='' AND LineRule = 'Nil Rated' AND HSNCODE <> ''  
		and (SGSRT_AMT + CGSRT_AMT  + IGSRT_AMT + cessr_amt) = 0  and (SGST_AMT + CGST_AMT  + IGST_AMT + cess_amt) = 0
		group by gstin,inv_no,[date],pos_std_cd,rate,hsncode,SBBILLNO,SBDATE,Entry_ty,portcode,isaugstref
		Order by GSTIN,[DATE],INV_NO,rate
End
/*	3E. Supplies to SEZ units/developers with payment of tax (including edit/amendment)	*/
IF @SECCODE = 'SEZ'  or  @SECCODE = 'All' and @Annx Not in ('SAHAJ','SUGAM')
BEGIN
	print '3e'
	Insert Into #GSTANNX1(PART,PARTSR,ctin,doctyp,sec7act,rfndelg,pos,num,dt,val,hsn,txval,igst,cgst,sgst,cess,rate,sbdt,sbnum,pcode,[type],Ecomgstin,clmrfnd,sup,supr,seccode)
	SELECT 3 AS PART ,'3E' AS PARTSR,gstin as ctin,
		case when Entry_ty='ST' then 'I' when Entry_ty='GC' then 'C' when Entry_ty='GD' then 'D' end as doctyp,
		'N' as sec7act,'Y' as rfndelg,pos_std_cd as pos,INV_NO as num,[date] as dt,sum(GRO_AMT) as val,hsncode as hsn,txval =sum(taxableamt),
		igst = sum(IGST_AMT),cgst = sum(CGST_AMT),sgst = sum(SGST_AMT),cess_amt = sum(cess_amt),rate,SBDATE as sbdt,SBBILLNO as sbnum,
		portcode as pcode,'' as [type],'' as Ecomgstin,isaugstref as clmrfnd,0 as sup,0 as supr,'sezwp' as seccode
		from #GSTJSONANX1TBL
		where (entry_ty in ('ST','SB') and entry_ty<>'UB') 
		and supp_type IN ('SEZ') 
		AND LineRule = 'Taxable' AND HSNCODE <> '' and st_type iN('INTERSTATE','INTRASTATE') 
		and (SGSRT_AMT + CGSRT_AMT + IGSRT_AMT + cessr_amt) = 0  and (SGST_AMT + CGST_AMT  + IGST_AMT + cess_amt) > 0
		group by gstin,inv_no,date,pos_std_cd,rate,hsncode,SBBILLNO,SBDATE,Entry_ty,portcode,isaugstref 
		Order by GSTIN,DATE,INV_NO,rate
/*	3F. Supplies to SEZ units/developers without payment of tax (including edit/amendment)	*/
	Insert Into #GSTANNX1(PART,PARTSR,ctin,doctyp,sec7act,rfndelg,pos,num,dt,val,hsn,txval,igst,cgst,sgst,cess,rate,sbdt,sbnum,pcode,[type],Ecomgstin,clmrfnd,sup,supr,seccode)
	SELECT 3 AS PART ,'3F' AS PARTSR,gstin as ctin,
		case when Entry_ty='ST' then 'I' when Entry_ty='GC' then 'C' when Entry_ty='GD' then 'D' end as doctyp,
		'N' as sec7act,'Y' as rfndelg,pos_std_cd as pos,INV_NO as num,[date] as dt,sum(GRO_AMT) as val,hsncode as hsn,txval =sum(taxableamt),
		igst = sum(IGST_AMT),cgst = sum(CGST_AMT),sgst = sum(SGST_AMT),cess_amt = sum(cess_amt),rate,SBDATE as sbdt,SBBILLNO as sbnum,
		portcode as pcode,'' as [type],'' as Ecomgstin,isaugstref as clmrfnd,0 as sup, 0 as supr,'sezwop' as seccode
		from #GSTJSONANX1TBL
		where (entry_ty in ('ST','SB') and entry_ty<>'UB') 
			and supp_type IN ('SEZ') 
			AND LineRule = 'Nil Rated' AND HSNCODE <> '' and st_type iN('INTERSTATE','INTRASTATE') 
			and (SGSRT_AMT + CGSRT_AMT + IGSRT_AMT + cessr_amt) = 0  and (SGST_AMT + CGST_AMT  + IGST_AMT + cess_amt) = 0
			group by gstin,inv_no,date,pos_std_cd,rate,hsncode,SBBILLNO,SBDATE,Entry_ty,portcode,isaugstref
			Order by GSTIN,DATE,INV_NO,rate
END

IF @SECCODE = 'DE'  or  @SECCODE = 'All' and @Annx Not in ('SAHAJ','SUGAM')
BEGIN
	print '3g'
	Insert Into #GSTANNX1(PART,PARTSR,ctin,doctyp,sec7act,rfndelg,pos,num,dt,val,hsn,txval,igst,cgst,sgst,cess,rate,sbdt,sbnum,pcode,[type],Ecomgstin,clmrfnd,sup,supr,seccode)
	SELECT 3 AS PART ,'3G' AS PARTSR,gstin as ctin,
		case when Entry_ty='ST' then 'I' when Entry_ty='GC' then 'C' when Entry_ty='GD' then 'D' end as doctyp,
		'N' as sec7act,'Y' as rfndelg,pos_std_cd as pos,INV_NO as num,[date] as dt,sum(GRO_AMT) as val,hsncode as hsn,txval =sum(taxableamt),
		igst = sum(IGST_AMT),cgst = sum(CGST_AMT),sgst = sum(SGST_AMT),cess_amt = sum(cess_amt),rate,SBDATE as sbdt,SBBILLNO as sbnum,
		portcode as pcode,'' as [type],'' as Ecomgstin,isaugstref as clmrfnd,0 as sup,0 as supr,'DE' as seccode
		from #GSTJSONANX1TBL
		where (Entry_ty in ('ST','SB') and entry_ty<>'UB') 
			and supp_type IN ('EOU') 
			group by gstin,inv_no,[date],pos_std_cd,rate,hsncode,SBBILLNO,SBDATE,Entry_ty,portcode,isaugstref
			Order by GSTIN,DATE,INV_NO,rate
END
	print '3h'
/* 3H. Inward supplies received from a registered supplier (attracting reverse charge) */

SELECT mEntry=case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end,
	RATE1= (case when (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0)) >0  then (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0))when (isnull(A.IGST_PER,0)) > 0 then (isnull(A.IGST_PER,0))else isnull(A.gstrate,0) end)
		,A.AGAINSTGS,A.AmendDate,A.AVL_ITC,A.BEDT,A.beno,A.Cess_amt,A.Cess_per,A.CessRT_amt,A.CGSRT_AMT,A.CGST_AMT,A.CGST_PER
		,A.DATE,A.Entry_ty,A.GRO_AMT,A.GSTIN,A.gstrate,A.gstype,A.HSNCODE,A.IGSRT_AMT,A.IGST_AMT,A.IGST_PER,A.INV_NO
		,A.inv_sr,A.Isservice,A.IT_CODE,A.IT_NAME,A.ITSERIAL,A.l_yn,A.LineRule,A.net_amt,A.OLDGSTIN,A.ORG_DATE,A.ORG_INVNO,A.orgbedt,A.orgbeno,A.pinvdt,A.pinvno
		,A.portcode,A.pos,A.pos_std_cd,A.QTY,A.RevCharge
		,A.SGSRT_AMT,A.SGST_AMT,A.SGST_PER,A.ST_TYPE,A.SUPP_TYPE,A.Taxableamt,A.Tran_cd,A.TRANSTATUS,A.isaugstref
		INTO #GSTR2TBL 
		FROM GSTR2_VW A  
		LEFT OUTER JOIN Epayment B ON (A.Tran_cd =B.tran_cd AND A.Entry_ty =B.entry_ty AND A.ITSERIAL =B.itserial)  
		inner join lcode l  on (a.entry_ty=l.entry_ty)
		WHERE (A.DATE BETWEEN @SDATE AND @EDATE) and A.AmendDate=''
			
	select 	a.Tran_cd,a.Entry_ty,a.Main_tran,a.ENTRY_ALL
	into #rcmbill from mainall_vw a
	where date<=@EDATE and a.Entry_all='BP' and a.Entry_ty<>'GB'					
	group by  a.Tran_cd,a.Entry_ty,a.Main_tran,a.ENTRY_ALL

	select A.*,isnull(b.Tran_cd,0) as BillTran into #GSTR2TBL_RCM from  #GSTR2TBL A  
	left outer join #rcmbill b on (a.tran_cd =b.Main_tran and a.entry_ty =b.ENTRY_ALL)  
	
	select Entry_ty=b.Entry_all,Tran_cd=b.Main_tran,cgsrt_amt=sum(cgsrt_amt),sgsrt_amt=sum(sgsrt_amt),igsrt_amt=sum(igsrt_amt),
		   CessRT_amt=sum(CessRT_amt) ,TaxableAmt=sum(TaxableAmt),GRO_AMT=sum(GRO_AMT)
	into #tblbill  from #GSTR2TBL a
	Inner Join #rcmbill b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.tran_Cd)
	Group by b.Entry_all,b.Main_tran
	
	update #GSTR2TBL_RCM set cgsrt_amt=b.cgsrt_amt - a.cgsrt_amt,sgsrt_amt=b.sgsrt_amt - a.sgsrt_amt ,igsrt_amt=b.igsrt_amt - a.igsrt_amt  
		,CessRT_amt=b.CessRT_amt-a.CessRT_amt,Taxableamt=b.TaxableAmt-a.TaxableAmt,Gro_amt=b.GRO_AMT-a.Gro_amt
		from #tblbill a Inner Join #GSTR2TBL_RCM b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.Tran_cd)
		
	Delete from #GSTR2TBL_RCM where cgsrt_amt=0 and sgsrt_amt=0 and igsrt_amt=0 and CessRT_amt=0 and Taxableamt=0 and GRO_AMT=0 and Entry_ty='BP'	

IF @SECCODE = 'REV'  or  @SECCODE = 'All'
begin
Select 3 as part,'3H' as partsr ,pinvno=Case when mENTRY IN ('PR','CN','DN') then INV_NO else pinvno end 
	,pinvdt=Case when mENTRY IN ('PR','CN','DN') then DATE else pinvdt end,Rate1
	,TaxableAmt = SUM(TaxableAmt),CGSRT_AMT=SUM(CGSRT_AMT ) ,SGSRT_AMT=SUM(SGSRT_AMT),IGSRT_AMT=SUM(IGSRT_AMT ),CessRt_Amt=sum(CessRt_Amt) 
	,gstin,pos_std_cd as pos,entry_ty	,gro_amt=sum(TaxableAmt),hsncode,portcode,isaugstref
	into #tmp3h
	FROM #GSTR2TBL_RCM
	where mENTRY IN ('EP','PT','PR','CN','DN','BP')	and SUPP_TYPE in ('Registered','Unregistered') and ltrim(rtrim(HSNCODE)) <>'' 
	AND LineRule = 'Taxable' AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT > 0)
	GROUP BY pinvno,pinvdt,Net_amt,Rate1,gstin,pos_std_cd,gstype,oldgstin,ST_TYPE,hsncode,inv_no,date,mENTRY,entry_ty,portcode,isaugstref
	ORDER BY gstin,pinvdt,pinvno,Rate1  

	Insert Into #GSTANNX1(PART,PARTSR,ctin,doctyp,sec7act,rfndelg,pos,num,dt,val,hsn,txval,igst,cgst,sgst,cess,rate,sbdt,sbnum,pcode,[type],Ecomgstin,clmrfnd,sup,supr,seccode)
	select part,partsr,gstin as ctin ,
	case when Entry_ty='ST' then 'I' when Entry_ty='GC' then 'C' when Entry_ty='GD' then 'D' end as doctyp,
	'N' as sec7act,'Y' as rfndelg,pos,pinvno as num,pinvdt as dt,sum(gro_amt) as val,hsncode as hsn,
	txval = SUM(TaxableAmt),igst=SUM(IGSRT_AMT),cgst=SUM(CGSRT_AMT),sgst=SUM(SGSRT_AMT),cess=sum(CessRt_Amt) 
	,Rate1 as rate,'' as sbdt,'' as sbnum,portcode as pcode,'G' as [type],'' as Ecomgstin,isaugstref as clmrfnd,0 as sup,0 as supr,'REV' as seccode
	from #tmp3h
	group by part,partsr,pinvno,pinvdt,gstin,pos,hsncode,Rate1,Entry_Ty,portcode,isaugstref
	ORDER BY gstin,pinvdt,pinvno,Rate1  
end
/* 3I Import of services */
IF @SECCODE = 'IMPS'  or  @SECCODE = 'All' and @Annx Not in ('SAHAJ','SUGAM')
BEGIN
		Select 3 as part,'3I' as partsr ,pinvno=Case when mENTRY IN ('PR','CN','DN') then Inv_no else pinvno end 
		,pinvdt=Case when mENTRY IN ('PR','CN','DN') then Date else pinvdt end
		,Rate1,SUM(TaxableAmt) as TaxableAmt,SUM(CGSRT_AMT) as CGSRT_AMT,SUM(SGSRT_AMT) as SGSRT_AMT,SUM(IGSRT_AMT) as IGSRT_AMT,sum(CessRt_amt) as CessRt_amt
		,gstin,pos_std_cd as pos,gro_amt=SUM(TaxableAmt),hsncode,portcode,isaugstref,Entry_Ty
		Into #tmp3I
		FROM #GSTR2TBL_RCM 
		where mENTRY IN ('EP','PT','CN','DN','BP')
		and Isservice = 'Services'	AND ST_TYPE IN('Out of Country','Interstate','Intrastate') and SUPP_TYPE in('Import','EOU','')
		AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) > 0
		GROUP BY pinvno,pinvdt,inv_no,date,Rate1,gstin,pos_std_cd,hsncode,mENTRY,Entry_Ty,portcode,isaugstref

	print '3i'
	
	Insert Into #GSTANNX1(PART,PARTSR,ctin,doctyp,sec7act,rfndelg,pos,num,dt,val,hsn,txval,igst,cgst,sgst,cess,rate,sbdt,sbnum,pcode,[type],Ecomgstin,clmrfnd,sup,supr,seccode)
	SELECT 3 AS PART ,'3I' AS PARTSR,gstin as ctin,'' as doctyp,'N' as sec7act,'Y' as rfndelg,pos,
			pinvno as num,pinvdt as dt,sum(GRO_AMT) as val,hsncode as hsn,txval =sum(taxableamt),
		igst=SUM(IGSRT_AMT),cgst=SUM(CGSRT_AMT),sgst=SUM(SGSRT_AMT),cess=sum(CessRt_Amt),rate1 as rate,'' as sbdt,'' as sbnum,
		portcode as pcode,'' as [type],'' as Ecomgstin,isaugstref as clmrfnd,0 as sup,0 as supr,'IMPS' as seccode
		from #tmp3I
		group by gstin,pos,rate1,hsncode,Entry_ty,portcode,pinvdt,pinvno,isaugstref
		Order by GSTIN,rate
END
/* 3J Import of Goods */
IF @SECCODE = 'IMPG'  or  @SECCODE = 'All' and @Annx Not in ('SAHAJ','SUGAM')
BEGIN
	Select 3 as part,'3J' as partsr ,pinvno=Case when mENTRY IN ('PR','CN','DN') then Inv_no else pinvno end 
		,pinvdt=Case when mENTRY IN ('PR','CN','DN') then Date else pinvdt end,Rate1,SUM(TaxableAmt) as TaxableAmt,SUM(CGST_AMT) as CGST_AMT
		,SUM(SGST_AMT) as SGST_AMT,SUM(IGST_AMT) as IGST_AMT,sum(CESS_AMT) as CESS_AMT,gstin,pos_std_cd as pos
		,gro_amt=SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then gro_amt else -gro_amt end),hsncode,Entry_Ty,portcode,isaugstref
		into #tmp3j
		FROM #GSTR2TBL_RCM 
		where mENTRY IN ('EP','PT','CN','DN')
		and Isservice <> 'Services'
		AND ST_TYPE IN('Out of Country','Interstate','Intrastate') and SUPP_TYPE in('Import','EOU','')
		GROUP BY pos_std_cd,pinvno,pinvdt,inv_no,date,Rate1,gstin,pos,hsncode,Entry_Ty,mENTRY,portcode,isaugstref

	Insert Into #GSTANNX1(PART,PARTSR,ctin,doctyp,sec7act,rfndelg,pos,num,dt,val,hsn,txval,igst,cgst,sgst,cess,rate,sbdt,sbnum,pcode,[type],Ecomgstin,clmrfnd,sup,supr,seccode)
	SELECT 3 AS PART ,'3J' AS PARTSR,gstin as ctin,
		'B' as doctyp,' ' as sec7act,' ' as rfndelg,pos,pinvno as num,pinvdt as dt,sum(GRO_AMT) as val,hsncode as hsn,txval =sum(taxableamt),
		igst=SUM(IGST_AMT),cgst=SUM(CGST_AMT),sgst=SUM(SGST_AMT),cess=sum(CESS_AMT),rate1 as rate,'' as sbdt,'' as sbnum,
		portcode as pcode,'' as [type],'' as Ecomgstin,isaugstref as clmrfnd,0 as sup,0 as supr,'IMPG' as seccode
		from #tmp3j
		group by gstin,pos,rate1,hsncode,Entry_ty,portcode,pinvdt,pinvno,isaugstref
		Order by GSTIN,rate1
END
/* 3K Import of goods from SEZ units / developers on a Bill of Entry	*/
IF @SECCODE = 'IMPGSEZ'  or  @SECCODE = 'All' and @Annx Not in ('SAHAJ','SUGAM')
BEGIN
	Insert Into #GSTANNX1(PART,PARTSR,ctin,doctyp,sec7act,rfndelg,pos,num,dt,val,hsn,txval,igst,cgst,sgst,cess,rate,sbdt,sbnum,pcode,[type],Ecomgstin,clmrfnd,sup,supr,seccode)
	select * from (
		Select 3 as part,'3K' as partsr,gstin as ctin,'B' as doctyp,' ' as sec7act,'Y' as rfndelg,pos_std_cd as pos,
		num=Case when mENTRY IN ('PR','CN','DN') then Inv_no else pinvno end,dt=Case when mENTRY IN ('PR','CN','DN') then Date else pinvdt end,
		val=SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then gro_amt else -gro_amt end),hsncode as hsn,SUM(TaxableAmt) as txval, 
		SUM(IGSRT_AMT+IGST_AMT) as igst,SUM(CGSRT_AMT+CGST_AMT) as cgst,SUM(SGSRT_AMT+SGST_AMT) as sgst,sum(CessRt_amt+CESS_AMT) as cess,
		rate1 as rate,bedt as sbdt,beno as sbnum,portcode as pcode,'' as [type],'' as Ecomgstin,isaugstref as clmrfnd,0 as sup,0 as supr,'IMPGSEZ' as seccode
		FROM #GSTR2TBL_RCM 
		where mENTRY IN ('EP','PT','CN','DN') and Isservice <> 'Services' AND ST_TYPE IN('Interstate','Intrastate') and SUPP_TYPE in('SEZ')
		GROUP BY pinvno,pinvdt,inv_no,date,Rate1,gstin,pos_std_cd,hsncode,beno,bedt,Entry_Ty,mENTRY,portcode,isaugstref
		) bb ORDER BY ctin,dt,num,Rate

END

IF @SECCODE = 'ECOM'  or  @SECCODE = 'All' and @Annx Not in ('SAHAJ','SUGAM')
BEGIN
	Insert Into #GSTANNX1(PART,PARTSR,ctin,doctyp,sec7act,rfndelg,pos,num,dt,val,hsn,txval,igst,cgst,sgst,cess,rate,sbdt,sbnum,pcode,[type],Ecomgstin,clmrfnd,sup,supr,seccode)
	Select * 
		from (
				select 4 AS PART ,'4' AS PARTSR,'' as ctin,'' as doctyp,'' as sec7act,'' as rfndelg,'' as pos,
				'' as num,'' as dt,	val=sum((case when entry_ty in('ST','SB','DN') THEN GRO_AMT ELSE -GRO_AMT END )),
				hsncode as hsn,txval = sum((case when Entry_ty in('ST','SB','DN') THEN +(taxableamt)ELSE - (taxableamt) END )),
				IGST_AMT = sum((case when Entry_ty in('ST','SB','DN') THEN +(IGST_AMT) ELSE - (IGST_AMT) END )),
				CGST_AMT = sum((case when Entry_ty in('ST','SB','DN') THEN +(CGST_AMT) ELSE - (CGST_AMT) END )),
				SGST_AMT = sum((case when Entry_ty in('ST','SB','DN') THEN +(SGST_AMT) ELSE - (SGST_AMT) END )),
				cess = sum((case when Entry_ty in('ST','SB','DN') THEN +(cess_amt) ELSE - (cess_amt) END )),rate ,
				'' as sbdt,'' as sbnum,'' as pcode,'' as [type],Ecomgstin as Ecomgstin,isaugstref as clmrfnd,
				sup=sum((case when Entry_ty in('ST','SB','DN') THEN GRO_AMT ELSE 0 END )),
				supr=sum((case when Entry_ty in('CN','SR') THEN GRO_AMT ELSE 0 END )),'ECOM' as seccode
				from #GSTJSONANX1TBL 
				where (Entry_ty in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type in ('Intrastate','Interstate','Out of Country') 
				and ecomgstin<>''
				group by Ecomgstin,hsncode,rate,isaugstref
			)aa	order by Ecomgstin
END

if(@Summary=0)
begin
	select * from #GSTANNX1 order by part,partsr
	--drop table #GSTANNX1 
end
else
begin
	--Select TaxYear=Year(@EDATE),TaxMonth=Month(@EDATE),SrNo=cast(0 as Int),Part=Cast('' as varchar(10)),Sec=Cast('' as varchar(10)),RecCount=cast(0 as Int),Val=cast(0 as Decimal(13,2)),txval=cast(0 as Decimal(13,2)),camt=cast(0 as Decimal(13,2)),samt=cast(0 as Decimal(13,2)),iamt=cast(0 as Decimal(13,2)),csamt=cast(0 as Decimal(13,2))
	Select SrNo=cast(0 as Int),Sec=Cast('' as varchar(10)),PartSr=Cast('' as varchar(10)),RecCount=cast(0 as Int),Val=cast(0 as Decimal(13,2)),txval=cast(0 as Decimal(13,2)),camt=cast(0 as Decimal(13,2)),samt=cast(0 as Decimal(13,2)),iamt=cast(0 as Decimal(13,2)),csamt=cast(0 as Decimal(13,2)),TaxYear=Year(@EDATE),TaxMonth=Month(@EDATE)
		into #GSTANNX1_Sum
		From #GSTANNX1
		Where 1=2

	Select seccode,PartSr,cntSec=Count(Seccode),(Val),TxVal=Sum(TxVal),CAmt=Sum(cgst),SAmt=Sum(sgst),IAmt=Sum(igst),CSAmt=Sum(cess),TaxYear=Year(@EDATE),TaxMonth=Month(@EDATE)
			into #TblSum
			From #GSTANNX1
			Group By seccode,PartSr,val

			

	If @SecCode='B2C' or @SecCode='All' 
	Begin
		Insert into #GSTANNX1_Sum (SrNo,Sec,PartSr,RecCount,Val,TxVal,CAmt,SAmt,IAmt,CSAmt,TaxYear,TaxMonth) 
			Select 1 as srno,Sec='B2C',PartSr='3A',Count(cntSec),sum(Val),Sum(TxVal),Sum(CAmt),Sum(SAmt),Sum(IAmt),Sum(CSAmt),TaxYear=Year(@EDATE),TaxMonth=Month(@EDATE)
			From #TblSum
			Where PartSr in ('3A')
	End	
	If @SecCode='B2B' or @SecCode='All' and @Annx Not in ('SAHAJ')
	Begin
		Insert into #GSTANNX1_Sum (SrNo,Sec,PartSr,RecCount,Val,TxVal,CAmt,SAmt,IAmt,CSAmt,TaxYear,TaxMonth) 
			Select 2,Sec='B2B',PartSr='3B',Count(cntSec),sum(Val),Sum(TxVal),Sum(CAmt),Sum(SAmt),Sum(IAmt),Sum(CSAmt),TaxYear=Year(@EDATE),TaxMonth=Month(@EDATE)
			From #TblSum
			Where PartSr in ('3B')
	End	
	IF @SECCODE = 'EXP'  or  @SECCODE = 'All'and @Annx Not in ('SAHAJ','SUGAM')
	Begin
		Insert into #GSTANNX1_Sum (SrNo,Sec,PartSr,RecCount,Val,TxVal,CAmt,SAmt,IAmt,CSAmt,TaxYear,TaxMonth) 
			Select 3,Sec='EXP',PartSr='3C3D',Count(cntSec),sum(Val),Sum(TxVal),Sum(CAmt),Sum(SAmt),Sum(IAmt),Sum(CSAmt),TaxYear=Year(@EDATE),TaxMonth=Month(@EDATE)
			From #TblSum
			Where PartSr in ('3C','3D')
	End	
	IF @SECCODE = 'SEZ'  or  @SECCODE = 'All'and @Annx Not in ('SAHAJ','SUGAM')
	Begin
		Insert into #GSTANNX1_Sum (SrNo,Sec,PartSr,RecCount,Val,TxVal,CAmt,SAmt,IAmt,CSAmt,TaxYear,TaxMonth) 
			Select 4,Sec='SEZ',PartSr='3E3F',Count(cntSec),sum(Val),Sum(TxVal),Sum(CAmt),Sum(SAmt),Sum(IAmt),Sum(CSAmt),TaxYear=Year(@EDATE),TaxMonth=Month(@EDATE)
			From #TblSum
			Where PartSr in ('3E','3F')
	End	
	IF @SECCODE = 'DE'  or  @SECCODE = 'All'and @Annx Not in ('SAHAJ','SUGAM')
	Begin
		Insert into #GSTANNX1_Sum (SrNo,Sec,PartSr,RecCount,Val,TxVal,CAmt,SAmt,IAmt,CSAmt,TaxYear,TaxMonth) 
			Select 5,Sec='DE',PartSr='3G',Count(cntSec),sum(Val),Sum(TxVal),Sum(CAmt),Sum(SAmt),Sum(IAmt),Sum(CSAmt),TaxYear=Year(@EDATE),TaxMonth=Month(@EDATE)
			From #TblSum
			Where PartSr in ('3G')
	End	
	IF @SECCODE = 'REV'  or  @SECCODE = 'All'
	Begin
		Insert into #GSTANNX1_Sum (SrNo,Sec,PartSr,RecCount,Val,TxVal,CAmt,SAmt,IAmt,CSAmt,TaxYear,TaxMonth) 
			Select 6,Sec='REV',PartSr='3H',Count(cntSec),sum(Val),Sum(TxVal),Sum(CAmt),Sum(SAmt),Sum(IAmt),Sum(CSAmt),TaxYear=Year(@EDATE),TaxMonth=Month(@EDATE)
			From #TblSum
			Where PartSr in ('3H')
	End	
	IF @SECCODE = 'IMPS'  or  @SECCODE = 'All' and @Annx Not in ('SAHAJ','SUGAM')
	Begin
		Insert into #GSTANNX1_Sum (SrNo,Sec,PartSr,RecCount,Val,TxVal,CAmt,SAmt,IAmt,CSAmt,TaxYear,TaxMonth) 
			Select 7,Sec='IMPS',PartSr='3I',Count(cntSec),sum(Val),Sum(TxVal),Sum(CAmt),Sum(SAmt),Sum(IAmt),Sum(CSAmt),TaxYear=Year(@EDATE),TaxMonth=Month(@EDATE)
			From #TblSum
			Where PartSr in ('3I')
	End	
	IF @SECCODE = 'IMPG'  or  @SECCODE = 'All' and @Annx Not in ('SAHAJ','SUGAM')
	Begin
		Insert into #GSTANNX1_Sum (SrNo,Sec,PartSr,RecCount,Val,TxVal,CAmt,SAmt,IAmt,CSAmt,TaxYear,TaxMonth) 
			Select 8,Sec='IMPG',PartSr='3J',Count(cntSec),sum(Val),Sum(TxVal),Sum(CAmt),Sum(SAmt),Sum(IAmt),Sum(CSAmt),TaxYear=Year(@EDATE),TaxMonth=Month(@EDATE)
			From #TblSum
			Where PartSr in ('3J')
	End	
	IF @SECCODE = 'IMPGSEZ'  or  @SECCODE = 'All' and @Annx Not in ('SAHAJ','SUGAM')
	Begin
		Insert into #GSTANNX1_Sum (SrNo,Sec,PartSr,RecCount,Val,TxVal,CAmt,SAmt,IAmt,CSAmt,TaxYear,TaxMonth) 
			Select 9,Sec='IMPGSEZ','3K' as PartSr,Count(cntSec),sum(Val),Sum(TxVal),Sum(CAmt),Sum(SAmt),Sum(IAmt),Sum(CSAmt),TaxYear=Year(@EDATE),TaxMonth=Month(@EDATE)
			From #TblSum
			Where PartSr in ('3K')
	End	
	IF @SECCODE = 'ECOM'  or  @SECCODE = 'All' and @Annx Not in ('SAHAJ','SUGAM')
	Begin
		Insert into #GSTANNX1_Sum (SrNo,Sec,PartSr,RecCount,Val,TxVal,CAmt,SAmt,IAmt,CSAmt,TaxYear,TaxMonth) 
			Select 10,Sec='ECOM',PartSr='4',Count(cntSec),sum(Val),Sum(TxVal),Sum(CAmt),Sum(SAmt),Sum(IAmt),Sum(CSAmt),TaxYear=Year(@EDATE),TaxMonth=Month(@EDATE)
			From #TblSum
			Where PartSr in ('4')
	End	
	select * from #GSTANNX1_Sum order by SrNo
end

--select * from #GSTANNX1
drop table #GSTANNX1 
--EXECUTE Usp_Rep_GST_ANX1_JSON '04/01/2019','03/31/2020','all','Normal',0


