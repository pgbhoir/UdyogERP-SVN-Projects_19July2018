IF EXISTS(SELECT * FROM SYSOBJECTS WHERE XTYPE='P' AND [NAME]='Usp_Rep_GSTR1_EXCEL')
BEGIN
	DROP PROCEDURE Usp_Rep_GSTR1_EXCEL
END
GO
CREATE Procedure [dbo].[Usp_Rep_GSTR1_EXCEL]
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
As
BEGIN
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

	-------Temporary Table Creation ----
select TranId =IDENTITY(INT,1,1)
--,SerialNo=cast(0 as int)
,tabs=cast('' as varchar(10))
,lvl=cast(0 as int)
,entry_ty=cast('' as char(2))
,tran_cd=cast(0 as int)
,gstin=cast('' as varchar(15))
,recname=cast('' as varchar(100))
,invno=cast('' as varchar(16))
,invdt=cast('' as varchar(15))
,invval=cast(0.00 as decimal(18,2))
,pos=cast('' as varchar(50))
,revchg=cast('' as char(1))
,apptaxrate=cast(0.00 as decimal(4,2))
,invtype=cast('' as varchar(50))
,ecomgstin=cast('' as varchar(15))
,rate=cast(0.00 as decimal(5,2))
,taxval=cast(0.00 as decimal(18,2))
,oinvno=cast('' as varchar(16))
,oinvdt=cast('' as varchar(15))
,opos=cast('' as varchar(50))
,salefrmbndedwh=cast('' as char(1))
,typ=cast('' as varchar(2))
,finyr=cast('' as varchar(9))
,omnth=cast('' as varchar(20))
,urtype=cast('' as varchar(10))
,nrvno=cast('' as varchar(16))
,nrvdt=cast('' as varchar(15))
,doctype=cast('' as char(1))
,nrvval=cast(0.00 as decimal(18,2))
,pregst=cast('' as char(1))
,onrvno=cast('' as varchar(16))
,onrvdt=cast('' as varchar(15))
,suptype=cast('' as varchar(20))
,exptype=cast('' as varchar(5))
,portcode=cast('' as varchar(15))
,sbillno=cast('' as varchar(16))
,sbilldt=cast('' as varchar(15))
,grossadv=cast(0.00 as decimal(18,2))
,descr=cast('' as varchar(100))
,nil=cast(0.00 as decimal(18,2))
,exempt=cast(0.00 as decimal(18,2))
,nongst=cast(0.00 as decimal(18,2))
,hsn=cast('' as varchar(20))
,uqc=cast('' as varchar(50))
,totqty=cast(0.00 as decimal(9,2))
,totval=cast(0.00 as decimal(18,2))
,igstamt=cast(0.00 as decimal(18,2))
,cgstamt=cast(0.00 as decimal(18,2))
,sgstamt=cast(0.00 as decimal(18,2))
,cessamt=cast(0.00 as decimal(18,2))
,natofdoc=cast('' as varchar(100))
,frmsrno=cast('' as varchar(16))
,tosrno=cast('' as varchar(16))
,totno=cast(0 as int)
,cancld=cast(0 as int)
,it_name=cast('' as varchar(100))
into #GSTR1EXCEL
FROM  STMAIN H 
INNER JOIN STITEM D ON (H.ENTRY_TY = D .ENTRY_TY AND H.TRAN_CD = D .TRAN_CD) 
INNER JOIN IT_MAST IT ON (D .IT_CODE = IT.IT_CODE) 
LEFT OUTER JOIN shipto ON (shipto.shipto_id = h.scons_id) 
LEFT OUTER JOIN ac_mast ac ON (h.cons_id = ac.ac_id)  
WHERE 1=2

/* GSTR_VW DATA STORED IN TEMPORARY TABLE*/
SELECT DISTINCT HSN_CODE, CAST(HSN_DESC AS VARCHAR(250)) AS HSN_DESC INTO #HSN1 FROM HSN_MASTER

SELECT mEntry=case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end,
		RATE1= (case when (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0)) >0  then (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0))
					when (isnull(A.IGST_PER,0)) > 0 then (isnull(A.IGST_PER,0)) else isnull(A.gstrate,0) end)
		,igst_amt1=ISNULL(a.igst_amt,0),cgst_amt1=ISNULL(a.cgst_amt,0),sGST_AMT1=isnull(a.sGST_AMT,0),CESS_AMT1 =isnull(a.CESS_AMT,0)
		,igsrt_amt1=ISNULL(a.IGSRT_AMT,0),cgsrt_amt1=ISNULL(a.CGSRT_AMT,0),sGSrT_AMT1=isnull(a.SGSRT_AMT,0),CESSR_AMT1 =isnull(a.CessR_amt,0)
		,Ecomgstin = isnull(ac.gstin,'')
		,A.*
		,HSN_DESC=ISNULL((SELECT TOP 1 HSN_DESC FROM #HSN1 WHERE HSN_CODE = A.HSNCODE),'')
		,invtype=(case when A.supp_type='SEZ' and A.expotype='WITH IGST' then 'SEZ supplies with payment' else 
			(case when A.supp_type='SEZ' and A.expotype='WITHOUT IGST' then 'SEZ supplies without payment' else 
			(case when A.st_type in ('INTRASTATE','INTERSTATE') and A.supp_type in ('EXPORT','EOU','IMPORT') then 'Deemed Exp' else 
			(case when A.sfbwh = 1 then 'Sale from Bonded WH' else 'Regular' end) end) end) end)
		,org_date1=case when year(isnull(org_date,''))<='1900' then a.date else org_date end
INTO #GSTR1TBL 
FROM GSTR1_VW A
left join ac_mast ac on (A.ecomac_id=ac.ac_id)
inner join lcode l  on (a.entry_ty=l.entry_ty)
WHERE (A.DATE BETWEEN @SDATE AND @EDATE) and A.AMENDDATE=''


-----Amended Detail Temp table 
SELECT mEntry=case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end,
		RATE1= (case when (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0)) >0  then (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0))
					when (isnull(A.IGST_PER,0)) > 0 then (isnull(A.IGST_PER,0)) else isnull(A.gstrate,0) end)
		,igst_amt1=ISNULL(a.igst_amt,0),cgst_amt1=ISNULL(a.cgst_amt,0),sGST_AMT1=isnull(a.sGST_AMT,0),CESS_AMT1 =isnull(a.CESS_AMT,0)
		,igsrt_amt1=ISNULL(a.IGSRT_AMT,0),cgsrt_amt1=ISNULL(a.CGSRT_AMT,0),sGSrT_AMT1=isnull(a.SGSRT_AMT,0),CESSR_AMT1 =isnull(a.CessR_amt,0)
		,Ecomgstin = isnull(ac.gstin,'')
		,A.*
		,HSN_DESC=ISNULL((SELECT TOP 1 HSN_DESC FROM #HSN1 WHERE HSN_CODE = A.HSNCODE),'')
		,invtype=(case when A.supp_type='SEZ' and A.expotype='WITH IGST' then 'SEZ supplies with payment' else 
			(case when A.supp_type='SEZ' and A.expotype='WITHOUT IGST' then 'SEZ supplies without payment' else 
			(case when A.st_type in ('INTRASTATE','INTERSTATE') and A.supp_type in ('EXPORT','EOU','IMPORT') then 'Deemed Exp' else 
			(case when A.sfbwh = 1 then 'Sale from Bonded WH' else 'Regular' end) end) end) end)
		,org_date1=case when year(isnull(org_date,''))<='1900' then a.date else org_date end
INTO #GSTR1AMD
FROM GSTR1_VW A
left join ac_mast ac on (A.ecomac_id=ac.ac_id)
inner join lcode l  on (a.entry_ty=l.entry_ty)
WHERE (A.AMENDDATE BETWEEN @SDATE AND @EDATE) AND A.HSNCODE <> ''

Declare @amt1 decimal(18,2),@amt2 decimal(18,2),@amt3 decimal(18,2),@amt4 decimal(18,2),@from_srno varchar(30),@to_srno varchar(30)

--b2b
print 'b2b'

Insert Into #GSTR1EXCEL(tabs,lvl,gstin,recname,invno,invdt,invval,pos,revchg,apptaxrate,invtype,ecomgstin,rate,taxval,cessamt,entry_ty,tran_cd)
SELECT tabs,lvl,gstin,recname,invno,invdt,sum(invval),pos,revchg,apptaxrate,invtype,ecomgstin,rate,sum(taxval),sum(cessamt),entry_ty,tran_cd FROM (
select section='4A',tabs='b2b',lvl=1,gstin,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,invval=gro_amt
--,invval=net_amt
,pos=pos_std_cd+'-'+pos,revchg='N',apptaxrate=0,invtype,ecomgstin,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,entry_ty,tran_cd
from #GSTR1TBL
where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type <> 'Out of country' 
and supp_type IN ('Registered','Compounding','E-Commerce','')   
and gstin <>'' AND LineRule = 'Taxable' AND HSNCODE <> '' and Ecomgstin = ''
and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) > 0
UNION ALL
select section='4B',tabs='b2b',lvl=1,gstin,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,invval=gro_amt
--,invval=net_amt
,pos=pos_std_cd+'-'+pos
--,revchg='N'  --Commented by Priyanka B on 25082020 for Bug-33382
,revchg='Y'  --Modified by Priyanka B on 25082020 for Bug-33382
,apptaxrate=0,invtype,ecomgstin,rate=rate1,taxval=taxableamt
--,cessamt=cess_amt1  --Commented by Priyanka B on 25082020 for Bug-33382
,cessamt=cessr_amt1  --Modified by Priyanka B on 25082020 for Bug-33382
,entry_ty,tran_cd
FROM #GSTR1TBL
WHERE (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type <> 'Out of country' 
and supp_type IN('Registered','Compounding','E-Commerce')
and gstin <>'' AND LineRule = 'Taxable' AND HSNCODE <> '' and Ecomgstin = '' 
AND (SGSRT_AMT1 + CGSRT_AMT1 + IGSRT_AMT1 + cessr_amt1) > 0 
UNION ALL
select section='4C',tabs='b2b',lvl=1,gstin,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,invval=gro_amt
--,invval=net_amt
,pos=pos_std_cd+'-'+pos,revchg='N',apptaxrate=0,invtype,ecomgstin,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,entry_ty,tran_cd
from #GSTR1TBL
where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type <> 'Out of country' and Ecomgstin <> '' AND LineRule = 'Taxable' 
AND HSNCODE <> ''  and gstin not in('Unregistered') and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  
and (SGST_AMT1 + CGST_AMT1 + IGST_AMT1 + cess_amt1) > 0
UNION ALL
select section='6B',tabs='b2b',lvl=1,gstin,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,invval=gro_amt
--,invval=net_amt
,pos=pos_std_cd+'-'+pos,revchg='N',apptaxrate=0,invtype,ecomgstin,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,entry_ty,tran_cd
from #GSTR1TBL
where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type iN('INTERSTATE','INTRASTATE') and supp_type IN('SEZ') and Ecomgstin = ''
UNION ALL
select section='6C',tabs='b2b',lvl=1,gstin,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,invval=gro_amt
--,invval=net_amt
,pos=pos_std_cd+'-'+pos,revchg='N',apptaxrate=0,invtype,ecomgstin,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,entry_ty,tran_cd
from #GSTR1TBL
where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type iN('INTERSTATE','INTRASTATE') and supp_type  in('Export','EOU','IMPORT') and Ecomgstin = ''
)b2b
group by tabs,lvl,gstin,recname,invno,invdt,pos,revchg,apptaxrate,invtype,ecomgstin,rate,entry_ty,tran_cd
Order by invno,Invdt,Rate

If not exists(select * from #GSTR1EXCEL where tabs='b2b')
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,gstin,recname,invno,invdt,invval,pos,revchg,apptaxrate,invtype,ecomgstin,rate,taxval,cessamt,entry_ty,tran_cd)
	SELECT tabs='b2b',lvl=1,gstin='',recname='',invno='',invdt='',invval=0,pos='',revchg='',apptaxrate=0,invtype='',ecomgstin='',rate=0,taxval=0,cessamt=0,entry_ty='',tran_cd=0
End

--b2ba
print 'b2ba'

Insert Into #GSTR1EXCEL(tabs,lvl,gstin,recname,oinvno,oinvdt,invno,invdt,invval,pos,revchg,apptaxrate,invtype,ecomgstin,rate,taxval,cessamt,entry_ty,tran_cd)
SELECT tabs,lvl,gstin,recname,oinvno,oinvdt,invno,invdt,sum(invval),pos,revchg,apptaxrate,invtype,ecomgstin,rate,sum(taxval),sum(cessamt),entry_ty,tran_cd FROM (
select section='4A',tabs='b2ba',lvl=1,gstin,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),oinvno=rtrim(ltrim(ORG_INVNO))
,oinvdt=(cast(day(org_date) as varchar)+'-'+cast(left(datename(mm,org_date),3) as varchar)+'-'+cast(right(year(org_date),2) as varchar))
,invno=rtrim(ltrim(inv_no))
--,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))  --Commented by Priyanka B on 22072020 for AU 2.2.5
,invdt=(cast(day(amenddate) as varchar)+'-'+cast(left(datename(mm,amenddate),3) as varchar)+'-'+cast(right(year(amenddate),2) as varchar))  --Modified by Priyanka B on 22072020 for AU 2.2.5
,invval=gro_amt,pos=pos_std_cd+'-'+pos,revchg='N',apptaxrate=0,invtype,ecomgstin,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,entry_ty,tran_cd
from #GSTR1AMD
where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type <> 'Out of country' 
and supp_type IN ('Registered','Compounding','E-Commerce','')   
and gstin <>'' AND LineRule = 'Taxable' AND HSNCODE <> '' and Ecomgstin = ''
and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) > 0
UNION ALL
select section='4B',tabs='b2ba',lvl=1,gstin,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),oinvno=rtrim(ltrim(ORG_INVNO))
,oinvdt=(cast(day(org_date) as varchar)+'-'+cast(left(datename(mm,org_date),3) as varchar)+'-'+cast(right(year(org_date),2) as varchar))
,invno=rtrim(ltrim(inv_no))
--,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))  --Commented by Priyanka B on 22072020 for AU 2.2.5
,invdt=(cast(day(amenddate) as varchar)+'-'+cast(left(datename(mm,amenddate),3) as varchar)+'-'+cast(right(year(amenddate),2) as varchar))  --Modified by Priyanka B on 22072020 for AU 2.2.5
,invval=gro_amt,pos=pos_std_cd+'-'+pos
--,revchg='N'  --Commented by Priyanka B on 25082020 for Bug-33382
,revchg='Y'  --Modified by Priyanka B on 25082020 for Bug-33382
,apptaxrate=0,invtype,ecomgstin,rate=rate1,taxval=taxableamt
--,cessamt=cess_amt1  --Commented by Priyanka B on 25082020 for Bug-33382
,cessamt=cessr_amt1  --Modified by Priyanka B on 25082020 for Bug-33382
,entry_ty,tran_cd
FROM #GSTR1AMD
WHERE (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type <> 'Out of country' 
and supp_type IN('Registered','Compounding','E-Commerce')
and gstin <>'' AND LineRule = 'Taxable' AND HSNCODE <> '' and Ecomgstin = '' 
AND (SGSRT_AMT1 + CGSRT_AMT1 + IGSRT_AMT1 + cessr_amt1) > 0 
UNION ALL
select section='4C',tabs='b2ba',lvl=1,gstin,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),oinvno=rtrim(ltrim(ORG_INVNO))
,oinvdt=(cast(day(org_date) as varchar)+'-'+cast(left(datename(mm,org_date),3) as varchar)+'-'+cast(right(year(org_date),2) as varchar))
,invno=rtrim(ltrim(inv_no))
--,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))  --Commented by Priyanka B on 22072020 for AU 2.2.5
,invdt=(cast(day(amenddate) as varchar)+'-'+cast(left(datename(mm,amenddate),3) as varchar)+'-'+cast(right(year(amenddate),2) as varchar))  --Modified by Priyanka B on 22072020 for AU 2.2.5
,invval=gro_amt,pos=pos_std_cd+'-'+pos,revchg='N',apptaxrate=0,invtype,ecomgstin,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,entry_ty,tran_cd
from #GSTR1AMD
where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type <> 'Out of country' and Ecomgstin <> '' AND LineRule = 'Taxable' 
AND HSNCODE <> ''  and gstin not in('Unregistered') and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  
and (SGST_AMT1 + CGST_AMT1 + IGST_AMT1 + cess_amt1) > 0
UNION ALL
select section='6B',tabs='b2ba',lvl=1,gstin,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),oinvno=rtrim(ltrim(ORG_INVNO))
,oinvdt=(cast(day(org_date) as varchar)+'-'+cast(left(datename(mm,org_date),3) as varchar)+'-'+cast(right(year(org_date),2) as varchar))
,invno=rtrim(ltrim(inv_no))
--,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))  --Commented by Priyanka B on 22072020 for AU 2.2.5
,invdt=(cast(day(amenddate) as varchar)+'-'+cast(left(datename(mm,amenddate),3) as varchar)+'-'+cast(right(year(amenddate),2) as varchar))  --Modified by Priyanka B on 22072020 for AU 2.2.5
,invval=gro_amt,pos=pos_std_cd+'-'+pos,revchg='N',apptaxrate=0,invtype,ecomgstin,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,entry_ty,tran_cd
from #GSTR1AMD
where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type iN('INTERSTATE','INTRASTATE') and supp_type IN('SEZ') and Ecomgstin = ''
UNION ALL
select section='6C',tabs='b2ba',lvl=1,gstin,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),oinvno=rtrim(ltrim(ORG_INVNO))
,oinvdt=(cast(day(org_date) as varchar)+'-'+cast(left(datename(mm,org_date),3) as varchar)+'-'+cast(right(year(org_date),2) as varchar))
,invno=rtrim(ltrim(inv_no))
--,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))  --Commented by Priyanka B on 22072020 for AU 2.2.5
,invdt=(cast(day(amenddate) as varchar)+'-'+cast(left(datename(mm,amenddate),3) as varchar)+'-'+cast(right(year(amenddate),2) as varchar))  --Modified by Priyanka B on 22072020 for AU 2.2.5
,invval=gro_amt,pos=pos_std_cd+'-'+pos,revchg='N',apptaxrate=0,invtype,ecomgstin,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,entry_ty,tran_cd
from #GSTR1AMD
where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type iN('INTERSTATE','INTRASTATE') and supp_type  in('Export','EOU','IMPORT') and Ecomgstin = ''
)b2ba
group by tabs,lvl,gstin,recname,oinvno,oinvdt,invno,invdt,pos,revchg,apptaxrate,invtype,ecomgstin,rate,entry_ty,tran_cd
Order by invno,Invdt,Rate

If not exists(select * from #GSTR1EXCEL where tabs='b2ba')
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,gstin,recname,oinvno,oinvdt,invno,invdt,invval,pos,revchg,apptaxrate,invtype,ecomgstin,rate,taxval,cessamt,entry_ty,tran_cd)
	SELECT tabs='b2ba',lvl=1,gstin='',recname='',oinvno='',oinvdt='',invno='',invdt='',invval=0,pos='',revchg='',apptaxrate=0,invtype='',ecomgstin='',rate=0,taxval=0,cessamt=0,entry_ty='',tran_cd=0
End

--b2cl
print 'b2cl'

Insert Into #GSTR1EXCEL(tabs,lvl,invno,invdt,invval,pos,apptaxrate,rate,taxval,cessamt,ecomgstin,salefrmbndedwh,entry_ty,tran_cd)
select tabs,lvl,invno,invdt,sum(invval),pos,apptaxrate,rate,sum(taxval),sum(cessamt),ecomgstin,salefrmbndedwh,entry_ty,tran_cd from (
select tabs='b2cl',lvl=1,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,invval=gro_amt,pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1
,taxval=taxableamt,cessamt=cess_amt1,ecomgstin,salefrmbndedwh=SFBWH,entry_ty,tran_cd
FROM #GSTR1TBL
WHERE (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type = 'InterState' and supp_type = 'Unregistered' and gstin ='Unregistered' AND LineRule = 'Taxable' 
AND HSNCODE <> '' and Ecomgstin = '' and net_amt >250000 and (SGST_AMT + CGST_AMT1  + IGST_AMT1 + cess_amt1) > 0
UNION ALL
select tabs='b2cl',lvl=1,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,invval=gro_amt,pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1
,taxval=taxableamt,cessamt=cess_amt1,ecomgstin,salefrmbndedwh=SFBWH,entry_ty,tran_cd
from #GSTR1TBL
where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type = 'InterState' and Ecomgstin <> '' AND LineRule = 'Taxable' AND HSNCODE <> '' 
and net_amt >250000 and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) > 0
)b2cl
group by tabs,lvl,invno,invdt,pos,apptaxrate,rate,ecomgstin,salefrmbndedwh,entry_ty,tran_cd
Order by invno,Invdt,Rate

If not exists(select * from #GSTR1EXCEL where tabs='b2cl')
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,invno,invdt,invval,pos,apptaxrate,rate,taxval,cessamt,ecomgstin,salefrmbndedwh,entry_ty,tran_cd)
	SELECT tabs='b2cl',lvl=1,invno='',invdt='',invval=0,pos='',apptaxrate=0,rate=0,taxval=0,cessamt=0,ecomgstin='',salefrmbndedwh='',entry_ty='',tran_cd=0
End

--b2cla
print 'b2cla'

Insert Into #GSTR1EXCEL(tabs,lvl,oinvno,oinvdt,opos,invno,invdt,invval,apptaxrate,rate,taxval,cessamt,ecomgstin,salefrmbndedwh,entry_ty,tran_cd)
select tabs,lvl,oinvno,oinvdt,opos,invno,invdt,sum(invval),apptaxrate,rate,sum(taxval),sum(cessamt),ecomgstin,salefrmbndedwh,entry_ty,tran_cd from (
select tabs='b2cla',lvl=1,oinvno=rtrim(ltrim(ORG_INVNO))
,oinvdt=(cast(day(org_date) as varchar)+'-'+cast(left(datename(mm,org_date),3) as varchar)+'-'+cast(right(year(org_date),2) as varchar))
,opos=org_pos,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,invval=gro_amt,apptaxrate=0,rate=rate1
,taxval=taxableamt,cessamt=cess_amt1,ecomgstin,salefrmbndedwh=SFBWH,entry_ty,tran_cd
FROM #GSTR1AMD
WHERE (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type = 'InterState' and supp_type = 'Unregistered' and gstin ='Unregistered' AND LineRule = 'Taxable' 
AND HSNCODE <> '' and Ecomgstin = '' and net_amt >250000 and (SGST_AMT + CGST_AMT1  + IGST_AMT1 + cess_amt1) > 0
UNION ALL
select tabs='b2cla',lvl=1,oinvno=rtrim(ltrim(ORG_INVNO))
,oinvdt=(cast(day(org_date) as varchar)+'-'+cast(left(datename(mm,org_date),3) as varchar)+'-'+cast(right(year(org_date),2) as varchar))
,opos=org_pos,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,invval=gro_amt,apptaxrate=0,rate=rate1
,taxval=taxableamt,cessamt=cess_amt1,ecomgstin,salefrmbndedwh=SFBWH,entry_ty,tran_cd
from #GSTR1AMD
where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type = 'InterState' and Ecomgstin <> '' AND LineRule = 'Taxable' AND HSNCODE <> '' 
and net_amt >250000 and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) > 0
)b2cla
group by tabs,lvl,oinvno,oinvdt,opos,invno,invdt,apptaxrate,rate,ecomgstin,salefrmbndedwh,entry_ty,tran_cd
Order by invno,Invdt,Rate

If not exists(select * from #GSTR1EXCEL where tabs='b2cla')
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,oinvno,oinvdt,opos,invno,invdt,invval,apptaxrate,rate,taxval,cessamt,ecomgstin,salefrmbndedwh,entry_ty,tran_cd)
	SELECT tabs='b2cla',lvl=1,oinvno='',oinvdt='',opos='',invno='',invdt='',invval=0,apptaxrate=0,rate=0,taxval=0,cessamt=0,ecomgstin='',salefrmbndedwh='',entry_ty='',tran_cd=0
End

--b2cs
print 'b2cs'

SELECT * 
INTO #tempTbl1 
FROM (
	select A.entry_ty,A.Tran_cd,A.Itserial,A.rentry_ty,A.Itref_tran,A.Ritserial,B.net_amt,B.INV_NO 
	from SRITREF A inner JOIN #GSTR1TBL B ON (A.rentry_ty =B.ENTRY_TY AND A.Itref_tran =B.Tran_cd  AND A.Ritserial =B.ITSERIAL)
	UNION ALL
	select A.entry_ty,A.Tran_cd,A.Itserial,A.rentry_ty,A.Itref_tran,A.Ritserial,B.net_amt,B.INV_NO
	from OTHITREF A inner JOIN #GSTR1TBL B ON (A.rentry_ty =B.ENTRY_TY AND A.Itref_tran =B.Tran_cd  AND A.Ritserial =B.ITSERIAL) 
)A 
	
select * INTO #TEMP1 
FROM (
	SELECT  *
	from #GSTR1TBL  
	where  mEntry in('CN','DN','SR')
	and (Entry_ty + QUOTENAME(Tran_cd)) in(select (Entry_ty + QUOTENAME(Tran_cd)) from #tempTbl1 where net_amt <=250000)
	UNION ALL 
	select *
	from #GSTR1TBL
	where  (mEntry in('SB','ST') and entry_ty<>'UB') AND net_amt <=250000 
) AA

--select * from #TEMP1
--Level=1
print 'b2cs - Level 1'

Insert Into #GSTR1EXCEL(tabs,lvl,typ,pos,apptaxrate,rate,taxval,cessamt,ecomgstin)
select tabs,lvl,typ,pos,apptaxrate,rate,taxval,cessamt,ecomgstin from (
select tabs='b2cs',lvl=1,typ='OE',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1),ecomgstin=''
from #GSTR1TBL  
where (mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type ='Intrastate' and supp_type ='Unregistered' 
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0 and Ecomgstin=''
group by pos_std_cd,pos,rate1
union all
select tabs='b2cs',lvl=1,typ='E',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1),ecomgstin
from #GSTR1TBL 
where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type = 'Intrastate' and gstin = 'Unregistered'	and Ecomgstin <> ''
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0
group by pos_std_cd,pos,rate1,ecomgstin
union all
select tabs='b2cs',lvl=1,typ='OE',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1),ecomgstin=''
from #TEMP1 
where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type = 'Interstate' and gstin = 'Unregistered'	and Ecomgstin = ''
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0
group by pos_std_cd,pos,rate1
union all
select tabs='b2cs',lvl=1,typ='E',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1),ecomgstin
from #TEMP1 
where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type = 'Interstate' and gstin = 'Unregistered'	and Ecomgstin <> ''
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0
group by pos_std_cd,pos,rate1,ecomgstin
)b2cs order by pos,rate

If not exists(select * from #GSTR1EXCEL where tabs='b2cs' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,typ,pos,apptaxrate,rate,taxval,cessamt,ecomgstin)
	SELECT tabs='b2cs',lvl=1,typ='',pos='',apptaxrate=0,rate=0,taxval=0,cessamt=0,ecomgstin=''
End

--Level=2
print 'b2cs - Level 2'

Insert Into #GSTR1EXCEL(tabs,lvl,typ,pos,apptaxrate,rate,taxval,cessamt,ecomgstin,entry_ty,tran_cd,invno,invdt,recname,gstin)
select tabs,lvl,typ,pos,apptaxrate,rate,taxval,cessamt,ecomgstin,entry_ty,tran_cd,invno,invdt,recname,gstin from (
select tabs='b2cs',lvl=2,typ='OE',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,ecomgstin=''
,entry_ty,tran_cd,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #GSTR1TBL  
where (mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type ='Intrastate' and supp_type ='Unregistered' 
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0 and Ecomgstin=''
--group by pos_std_cd,pos,rate1
union all
select tabs='b2cs',lvl=2,typ='E',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,ecomgstin
,entry_ty,tran_cd,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #GSTR1TBL 
where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type = 'Intrastate' and gstin = 'Unregistered' and Ecomgstin <> ''
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0
--group by pos_std_cd,pos,rate1,ecomgstin
union all
select tabs='b2cs',lvl=2,typ='OE',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,ecomgstin=''
,entry_ty,tran_cd,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #TEMP1 
where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type = 'Interstate' and gstin = 'Unregistered' and Ecomgstin = ''
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0
--group by pos_std_cd,pos,rate1
union all
select tabs='b2cs',lvl=2,typ='E',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,ecomgstin
,entry_ty,tran_cd,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #TEMP1 
where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type = 'Interstate' and gstin = 'Unregistered' and Ecomgstin <> ''
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0
--group by pos_std_cd,pos,rate1,ecomgstin
)b2cs order by invno,invdt

If not exists(select * from #GSTR1EXCEL where tabs='b2cs' and lvl=2)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,typ,pos,apptaxrate,rate,taxval,cessamt,ecomgstin,entry_ty,tran_cd,invno,invdt,recname)
	SELECT tabs='b2cs',lvl=2,typ='',pos='',apptaxrate=0,rate=0,taxval=0,cessamt=0,ecomgstin='',entry_ty='',tran_cd=0,invno='',invdt='',cons_ac_name=''
End

--b2csa
print 'b2csa'

SELECT * 
INTO #tempTbl1a
FROM (
	select A.entry_ty,A.Tran_cd,A.Itserial,A.rentry_ty,A.Itref_tran,A.Ritserial,B.net_amt,B.INV_NO 
	from SRITREF A inner JOIN #GSTR1AMD B ON (A.rentry_ty =B.ENTRY_TY AND A.Itref_tran =B.Tran_cd  AND A.Ritserial =B.ITSERIAL)
	UNION ALL
	select A.entry_ty,A.Tran_cd,A.Itserial,A.rentry_ty,A.Itref_tran,A.Ritserial,B.net_amt,B.INV_NO
	from OTHITREF A inner JOIN #GSTR1AMD B ON (A.rentry_ty =B.ENTRY_TY AND A.Itref_tran =B.Tran_cd  AND A.Ritserial =B.ITSERIAL) 
)A 
	
select * INTO #TEMP1a
FROM (
	SELECT  *
	from #GSTR1AMD  
	where  mEntry in('CN','DN','SR')
	and (Entry_ty + QUOTENAME(Tran_cd)) in(select (Entry_ty + QUOTENAME(Tran_cd)) from #tempTbl1 where net_amt <=250000)
	UNION ALL 
	select *
	from #GSTR1AMD
	where  (mEntry in('SB','ST') and entry_ty<>'UB') AND net_amt <=250000 
) AA

--select * from #TEMP1
--Level=1
print 'b2csa - Level 1'

Insert Into #GSTR1EXCEL(tabs,lvl,typ,pos,apptaxrate,rate,taxval,cessamt,ecomgstin,finyr,omnth)
select tabs,lvl,typ,pos,apptaxrate,rate,taxval,cessamt,ecomgstin,finyr,omnth from (
select tabs='b2csa',lvl=1,typ='OE',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1),ecomgstin=''
,finyr=(substring(@lyn,1,5)+right(@lyn,2)),omnth=datename(mm,@SDATE)
from #GSTR1AMD  
where (mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type ='Intrastate' and supp_type ='Unregistered' 
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0 and Ecomgstin=''
group by pos_std_cd,pos,rate1
union all
select tabs='b2csa',lvl=1,typ='E',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1),ecomgstin
,finyr=(substring(@lyn,1,5)+right(@lyn,2)),omnth=datename(mm,@SDATE)
from #GSTR1AMD 
where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type = 'Intrastate' and gstin = 'Unregistered'	and Ecomgstin <> ''
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0
group by pos_std_cd,pos,rate1,ecomgstin
union all
select tabs='b2csa',lvl=1,typ='OE',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1),ecomgstin=''
,finyr=(substring(@lyn,1,5)+right(@lyn,2)),omnth=datename(mm,@SDATE)
from #TEMP1a 
where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type = 'Interstate' and gstin = 'Unregistered'	and Ecomgstin = ''
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0
group by pos_std_cd,pos,rate1
union all
select tabs='b2csa',lvl=1,typ='E',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1),ecomgstin
,finyr=(substring(@lyn,1,5)+right(@lyn,2)),omnth=datename(mm,@SDATE)
from #TEMP1a 
where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type = 'Interstate' and gstin = 'Unregistered'	and Ecomgstin <> ''
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0
group by pos_std_cd,pos,rate1,ecomgstin
)b2csa  order by pos,rate

If not exists(select * from #GSTR1EXCEL where tabs='b2csa' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,typ,pos,apptaxrate,rate,taxval,cessamt,ecomgstin,finyr,omnth)
	SELECT tabs='b2csa',lvl=1,typ='',pos='',apptaxrate=0,rate=0,taxval=0,cessamt=0,ecomgstin='',finyr='',omnth=''
End

--Level=2
print 'b2csa - Level 2'

Insert Into #GSTR1EXCEL(tabs,lvl,typ,pos,apptaxrate,rate,taxval,cessamt,ecomgstin,entry_ty,tran_cd,invno,invdt,finyr,omnth,recname,gstin)
select tabs,lvl,typ,pos,apptaxrate,rate,taxval,cessamt,ecomgstin,entry_ty,tran_cd,invno,invdt,finyr,omnth,recname,gstin from (
select tabs='b2csa',lvl=2,typ='OE',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,ecomgstin=''
,entry_ty,tran_cd,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,finyr=(substring(@lyn,1,5)+right(@lyn,2)),omnth=datename(mm,@SDATE)
,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #GSTR1AMD  
where (mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type ='Intrastate' and supp_type ='Unregistered' 
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0 and Ecomgstin=''
--group by pos_std_cd,pos,rate1
union all
select tabs='b2csa',lvl=2,typ='E',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,ecomgstin
,entry_ty,tran_cd,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,finyr=(substring(@lyn,1,5)+right(@lyn,2)),omnth=datename(mm,@SDATE)
,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #GSTR1AMD 
where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type = 'Intrastate' and gstin = 'Unregistered'	and Ecomgstin <> ''
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0
--group by pos_std_cd,pos,rate1,ecomgstin
union all
select tabs='b2csa',lvl=2,typ='OE',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,ecomgstin=''
,entry_ty,tran_cd,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,finyr=(substring(@lyn,1,5)+right(@lyn,2)),omnth=datename(mm,@SDATE)
,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #TEMP1a
where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type = 'Interstate' and gstin = 'Unregistered'	and Ecomgstin = ''
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0
--group by pos_std_cd,pos,rate1
union all
select tabs='b2csa',lvl=2,typ='E',pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1,taxval=taxableamt,cessamt=cess_amt1,ecomgstin
,entry_ty,tran_cd,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,finyr=(substring(@lyn,1,5)+right(@lyn,2)),omnth=datename(mm,@SDATE)
,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #TEMP1a 
where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type = 'Interstate' and gstin = 'Unregistered'	and Ecomgstin <> ''
AND (CGST_AMT1+SGST_AMT1+IGST_AMT1+cess_amt1) > 0
--group by pos_std_cd,pos,rate1,ecomgstin
)b2csa  order by invno,invdt

If not exists(select * from #GSTR1EXCEL where tabs='b2csa' and lvl=2)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,typ,pos,apptaxrate,rate,taxval,cessamt,ecomgstin,entry_ty,tran_cd,invno,invdt,finyr,omnth,recname,gstin)
	SELECT tabs='b2csa',lvl=2,typ='',pos='',apptaxrate=0,rate=0,taxval=0,cessamt=0,ecomgstin='',entry_ty='',tran_cd=0,invno='',invdt='',finyr='',omnth='',recname='',gstin=''
End

--cdnr
print 'cdnr'

Insert Into #GSTR1EXCEL(tabs,lvl,gstin,recname,invno,invdt,nrvno,nrvdt,doctype,pos,nrvval,apptaxrate,rate,taxval,cessamt,pregst,entry_ty,tran_cd)
select tabs,lvl,gstin,recname,invno,invdt,nrvno,nrvdt,doctype,pos,nrvval,apptaxrate,rate,taxval,cessamt,pregst,entry_ty,tran_cd from (
select tabs='cdnr',lvl=1
,gstin=(CASE WHEN isnull(ltrim(rtrim(CONSGSTIN)),'') = '' THEN (case when isnull(ltrim(rtrim(GSTIN)),'') <> '' then ltrim(rtrim(GSTIN)) 
			else '' end) ELSE ltrim(rtrim(CONSGSTIN)) END)
,recname=substring(rtrim(ltrim(cons_ac_name)),1,15)
,invno=rtrim(ltrim(ORG_INVNO))
,invdt=(cast(day(org_date1) as varchar)+'-'+cast(left(datename(mm,org_date1),3) as varchar)+'-'+cast(right(year(org_date1),2) as varchar))
,nrvno=rtrim(ltrim(inv_no))
,nrvdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,doctype=doctype,pos=pos_std_cd+'-'+pos
,nrvval=net_amt,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1)
,pregst=(case when pregst=0 then 'N' else 'Y' end)
,entry_ty,tran_cd
from #GSTR1TBL
where entry_ty ='RV' AND HSNCODE <> '' AND LineRule = 'Taxable'	and SUPP_TYPE <> 'Unregistered'
GROUP BY ORG_INVNO,ORG_DATE1,Consgstin,INV_NO,DATE,pos,rate1,net_amt,gstin,cons_ac_name,doctype,pos_std_cd,pregst,entry_ty,tran_cd
union all
select tabs='cdnr',lvl=1
,gstin=(CASE WHEN isnull(ltrim(rtrim(CONSGSTIN)),'') = '' THEN (case when isnull(ltrim(rtrim(GSTIN)),'') <> '' then ltrim(rtrim(GSTIN)) 
			else '' end) ELSE ltrim(rtrim(CONSGSTIN)) END)
,recname=substring(rtrim(ltrim(cons_ac_name)),1,15)
,invno=rtrim(ltrim(ORG_INVNO))
,invdt=(cast(day(org_date1) as varchar)+'-'+cast(left(datename(mm,org_date1),3) as varchar)+'-'+cast(right(year(org_date1),2) as varchar))
,nrvno=rtrim(ltrim(inv_no))
,nrvdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,doctype=doctype,pos=pos_std_cd+'-'+pos
,nrvval=net_amt,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1)
,pregst=(case when pregst=0 then 'N' else 'Y' end)
,entry_ty,tran_cd
from #GSTR1TBL 
where mentry in('CN','DN','SR') and SUPP_TYPE <> 'Unregistered' 
AND HSNCODE <> '' AND LineRule = 'Taxable'
GROUP BY ORG_INVNO,ORG_DATE1,Consgstin,INV_NO,DATE,pos,rate1,net_amt,gstin,cons_ac_name,doctype,pos_std_cd,pregst,entry_ty,tran_cd
)cdnr

If not exists(select * from #GSTR1EXCEL where tabs='cdnr' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,gstin,recname,invno,invdt,nrvno,nrvdt,doctype,pos,nrvval,apptaxrate,rate,taxval,cessamt,pregst,entry_ty,tran_cd)
	SELECT tabs='cdnr',lvl=1,gstin='',recname='',invno='',invdt='',nrvno='',nrvdt='',doctype='',pos='',nrvval=0,apptaxrate=0,rate=0,taxval=0,cessamt=0,pregst='',entry_ty='',tran_cd=0
End

--cdnra
print 'cdnra'

Insert Into #GSTR1EXCEL(tabs,lvl,gstin,recname,onrvno,onrvdt,oinvno,oinvdt,nrvno,nrvdt,doctype,suptype,nrvval,apptaxrate,rate,taxval,cessamt,pregst,entry_ty,tran_cd)
select tabs,lvl,gstin,recname,onrvno,onrvdt,oinvno,oinvdt,nrvno,nrvdt,doctype,suptype,nrvval,apptaxrate,rate,taxval,cessamt,pregst,entry_ty,tran_cd from (
select tabs='cdnra',lvl=1
,gstin=CONSGSTIN,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),onrvno=rtrim(ltrim(org_invno))
,onrvdt=(cast(day(org_date1) as varchar)+'-'+cast(left(datename(mm,org_date1),3) as varchar)+'-'+cast(right(year(org_date1),2) as varchar))
,oinvno=rtrim(ltrim(inv_no))
,oinvdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,nrvno=rtrim(ltrim(org_invno))
,nrvdt=(cast(day(org_date1) as varchar)+'-'+cast(left(datename(mm,org_date1),3) as varchar)+'-'+cast(right(year(org_date1),2) as varchar))
,doctype=doctype
,suptype=(case when st_type='INTRASTATE' then 'Intra State' else 
			(case when st_type='INTERSTATE' then 'Inter State' else st_type end) end)
,nrvval=net_amt,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1)
,pregst=(case when pregst=0 then 'N' else 'Y' end)
,entry_ty,tran_cd
from #GSTR1AMD
where entry_ty ='RV' AND HSNCODE <> '' AND LineRule = 'Taxable'	and SUPP_TYPE <> 'Unregistered'
GROUP BY ORG_INVNO,ORG_DATE1,Consgstin,INV_NO,DATE,rate1,net_amt,cons_ac_name,doctype,pregst,cons_ac_name,st_type,entry_ty,tran_cd
union all
select tabs='cdnra',lvl=1
,gstin=CONSGSTIN,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),onrvno=rtrim(ltrim(org_invno))
,onrvdt=(cast(day(org_date1) as varchar)+'-'+cast(left(datename(mm,org_date1),3) as varchar)+'-'+cast(right(year(org_date1),2) as varchar))
,oinvno=rtrim(ltrim(inv_no))
,oinvdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,nrvno=rtrim(ltrim(org_invno))
,nrvdt=(cast(day(org_date1) as varchar)+'-'+cast(left(datename(mm,org_date1),3) as varchar)+'-'+cast(right(year(org_date1),2) as varchar))
,doctype=doctype
,suptype=(case when st_type='INTRASTATE' then 'Intra State' else 
			(case when st_type='INTERSTATE' then 'Inter State' else st_type end) end)
,nrvval=net_amt,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1)
,pregst=(case when pregst=0 then 'N' else 'Y' end)
,entry_ty,tran_cd
from #GSTR1AMD 
where mentry in('CN','DN','SR') and SUPP_TYPE <> 'Unregistered' 
AND HSNCODE <> '' AND LineRule = 'Taxable'
GROUP BY ORG_INVNO,ORG_DATE1,Consgstin,INV_NO,DATE,rate1,net_amt,cons_ac_name,doctype,pregst,cons_ac_name,st_type,entry_ty,tran_cd
)cdnra

If not exists(select * from #GSTR1EXCEL where tabs='cdnra' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,gstin,recname,onrvno,onrvdt,oinvno,oinvdt,nrvno,nrvdt,doctype,suptype,nrvval,apptaxrate,rate,taxval,cessamt,pregst,entry_ty,tran_cd)
	SELECT tabs='cdnra',lvl=1,gstin='',recname='',onrvno='',onrvdt='',oinvno='',oinvdt='',nrvno='',nrvdt='',doctype='',suptype='',nrvval=0,apptaxrate=0,rate=0,taxval=0,cessamt=0,pregst='',entry_ty='',tran_cd=0
End

--cdnur
print 'cdnur'

Insert Into #GSTR1EXCEL(tabs,lvl,urtype,nrvno,nrvdt,doctype,invno,invdt,pos,nrvval,apptaxrate,rate,taxval,cessamt,pregst,entry_ty,tran_cd)
select tabs,lvl,urtype,nrvno,nrvdt,doctype,invno,invdt,pos,nrvval,apptaxrate,rate,taxval,cessamt,pregst,entry_ty,tran_cd from (
select tabs='cdnur',lvl=1
,urtype=(case when supp_type='Export' and exptyp='WITH IGST' then 'EXPWP' else
		(case when supp_type='Export' and exptyp='WITHOUT IGST' then 'EXPWOP' else 'B2CL' end) end)
,nrvno=rtrim(ltrim(inv_no))
,nrvdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,doctype=doctype,invno=rtrim(ltrim(ORG_INVNO))
,invdt=(cast(day(org_date1) as varchar)+'-'+cast(left(datename(mm,org_date1),3) as varchar)+'-'+cast(right(year(org_date1),2) as varchar))
,pos=pos_std_cd+'-'+pos
,nrvval=net_amt,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1)
,pregst=(case when pregst=0 then 'N' else 'Y' end)
,entry_ty,tran_cd
from #GSTR1TBL
where entry_ty ='RV' AND HSNCODE <> '' AND LineRule = 'Taxable'	and SUPP_TYPE in ('Unregistered','Export','')
and net_amt >250000
GROUP BY ORG_INVNO,ORG_DATE1,Consgstin,INV_NO,DATE,pos,rate1,net_amt,gstin,cons_ac_name,doctype,pos_std_cd,pregst,supp_type,exptyp,entry_ty,tran_cd
union all	
select tabs='cdnur',lvl=1
,urtype=(case when supp_type='Export' and exptyp='WITH IGST' then 'EXPWP' else
		(case when supp_type='Export' and exptyp='WITHOUT IGST' then 'EXPWOP' else 'B2CL' end) end)
,nrvno=rtrim(ltrim(inv_no))
,nrvdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,doctype=doctype,invno=rtrim(ltrim(ORG_INVNO))
,invdt=(cast(day(org_date1) as varchar)+'-'+cast(left(datename(mm,org_date1),3) as varchar)+'-'+cast(right(year(org_date1),2) as varchar))
,pos=pos_std_cd+'-'+pos
,nrvval=net_amt,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1)
,pregst=(case when pregst=0 then 'N' else 'Y' end)
,entry_ty,tran_cd
from #GSTR1TBL  
where mentry in('CN','DN','SR') 
--and ST_TYPE = 'INTERSTATE' 
AND SUPP_TYPE in ('Unregistered','Export','')
and (Entry_ty + QUOTENAME(Tran_cd)) in(select (Entry_ty + QUOTENAME(Tran_cd)) from #tempTbl1 where net_amt > 250000)
AND HSNCODE <> '' AND LineRule = 'Taxable'
GROUP BY ORG_INVNO,ORG_DATE1,Consgstin,INV_NO,DATE,pos,rate1,net_amt,gstin,cons_ac_name,doctype,pos_std_cd,pregst,supp_type,exptyp,entry_ty,tran_cd
)cdnur

If not exists(select * from #GSTR1EXCEL where tabs='cdnur' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,urtype,nrvno,nrvdt,doctype,invno,invdt,pos,nrvval,apptaxrate,rate,taxval,cessamt,pregst,entry_ty,tran_cd)
	SELECT tabs='cdnur',lvl=1,urtype='',nrvno='',nrvdt='',doctype='',invno='',invdt='',pos='',nrvval=0,apptaxrate=0,rate=0,taxval=0,cessamt=0,pregst='',entry_ty='',tran_cd=0
End

--cdnura
print 'cdnura'

Insert Into #GSTR1EXCEL(tabs,lvl,urtype,onrvno,onrvdt,oinvno,oinvdt,nrvno,nrvdt,doctype,suptype,nrvval,apptaxrate,rate,taxval,cessamt,pregst,entry_ty,tran_cd)
select tabs,lvl,urtype,onrvno,onrvdt,oinvno,oinvdt,nrvno,nrvdt,doctype,suptype,nrvval,apptaxrate,rate,taxval,cessamt,pregst,entry_ty,tran_cd from (
select tabs='cdnura',lvl=1
,urtype=(case when supp_type='Export' and exptyp='WITH IGST' then 'EXPWP' else
		(case when supp_type='Export' and exptyp='WITHOUT IGST' then 'EXPWOP' else 'B2CL' end) end)
,onrvno=rtrim(ltrim(org_invno))
,onrvdt=(cast(day(org_date1) as varchar)+'-'+cast(left(datename(mm,org_date1),3) as varchar)+'-'+cast(right(year(org_date1),2) as varchar))
,oinvno=rtrim(ltrim(inv_no))
,oinvdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,nrvno=rtrim(ltrim(org_invno))
,nrvdt=(cast(day(org_date1) as varchar)+'-'+cast(left(datename(mm,org_date1),3) as varchar)+'-'+cast(right(year(org_date1),2) as varchar))
,doctype=doctype,suptype=supp_type,nrvval=net_amt,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1)
,pregst=(case when pregst=0 then 'N' else 'Y' end)
,entry_ty,tran_cd
from #GSTR1AMD
where entry_ty ='RV' AND HSNCODE <> '' AND LineRule = 'Taxable'	and SUPP_TYPE in ('Unregistered','Export','')
and net_amt >250000
GROUP BY ORG_INVNO,ORG_DATE1,INV_NO,DATE,rate1,net_amt,doctype,pregst,supp_type,exptyp,entry_ty,tran_cd
union all
select tabs='cdnura',lvl=1
,urtype=(case when supp_type='Export' and exptyp='WITH IGST' then 'EXPWP' else
		(case when supp_type='Export' and exptyp='WITHOUT IGST' then 'EXPWOP' else 'B2CL' end) end)
,onrvno=rtrim(ltrim(org_invno))
,onrvdt=(cast(day(org_date1) as varchar)+'-'+cast(left(datename(mm,org_date1),3) as varchar)+'-'+cast(right(year(org_date1),2) as varchar))
,oinvno=rtrim(ltrim(inv_no))
,oinvdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,nrvno=rtrim(ltrim(org_invno))
,nrvdt=(cast(day(org_date1) as varchar)+'-'+cast(left(datename(mm,org_date1),3) as varchar)+'-'+cast(right(year(org_date1),2) as varchar))
,doctype=doctype,suptype=supp_type,nrvval=net_amt,apptaxrate=0,rate=rate1,taxval=sum(taxableamt),cessamt=sum(cess_amt1)
,pregst=(case when pregst=0 then 'N' else 'Y' end)
,entry_ty,tran_cd
from #GSTR1AMD
where mentry in('CN','DN','SR') 
--and ST_TYPE = 'INTERSTATE' 
AND SUPP_TYPE in ('Unregistered','Export','')
and (Entry_ty + QUOTENAME(Tran_cd)) in(select (Entry_ty + QUOTENAME(Tran_cd)) from #tempTbl1 where net_amt > 250000)
AND HSNCODE <> '' AND LineRule = 'Taxable'
GROUP BY ORG_INVNO,ORG_DATE1,INV_NO,DATE,rate1,net_amt,doctype,pregst,supp_type,exptyp,entry_ty,tran_cd
)cdnura

If not exists(select * from #GSTR1EXCEL where tabs='cdnura' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,urtype,onrvno,onrvdt,oinvno,oinvdt,nrvno,nrvdt,doctype,suptype,nrvval,apptaxrate,rate,taxval,cessamt,pregst,entry_ty,tran_cd)
	SELECT tabs='cdnura',lvl=1,urtype='',onrvno='',onrvdt='',oinvno='',oinvdt='',nrvno='',nrvdt='',doctype='',suptype='',nrvval=0,apptaxrate=0,rate=0,taxval=0,cessamt=0,pregst='',entry_ty='',tran_cd=0
End

--exp
print 'exp'

Insert Into #GSTR1EXCEL(tabs,lvl,exptype,invno,invdt,invval,portcode,sbillno,sbilldt,apptaxrate,rate,taxval,cessamt,entry_ty,tran_cd)
select tabs,lvl,exptype,invno,invdt,invval,portcode,sbillno,sbilldt,apptaxrate,rate,taxval,cessamt,entry_ty,tran_cd from (
select tabs='exp',lvl=1
,exptype=(case when expotype='WITH IGST' then 'WPAY' else 'WOPAY' end)
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,invval=net_amt,portcode
--,sbillno=sbbillno  --Commented by Priyanka B on 12062020 for Bug-33382
,sbillno=substring(rtrim(ltrim(sbbillno)),1,7)  --Modified by Priyanka B on 12062020 for Bug-33382
,sbilldt=(cast(day(sbdate) as varchar)+'-'+cast(left(datename(mm,sbdate),3) as varchar)+'-'+cast(right(year(sbdate),2) as varchar))
,apptaxrate=0,rate=rate1
,taxval=sum(taxableamt),cessamt=sum(cess_amt1),entry_ty,tran_cd
from #GSTR1TBL
where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type ='OUT OF COUNTRY' and Ecomgstin = ''
group by expotype,inv_no,date,net_amt,portcode,sbbillno,sbdate,rate1,entry_ty,tran_cd
)exp

If not exists(select * from #GSTR1EXCEL where tabs='exp' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,exptype,invno,invdt,invval,portcode,sbillno,sbilldt,apptaxrate,rate,taxval,cessamt,entry_ty,tran_cd)
	SELECT tabs='exp',lvl=1,exptype='',invno='',invdt='',invval=0,portcode='',sbillno='',sbilldt='',apptaxrate=0,rate=0,taxval=0,cessamt=0,entry_ty='',tran_cd=0
End

--expa
print 'expa'

Insert Into #GSTR1EXCEL(tabs,lvl,exptype,oinvno,oinvdt,invno,invdt,invval,portcode,sbillno,sbilldt,apptaxrate,rate,taxval,cessamt,entry_ty,tran_cd)
select tabs,lvl,exptype,oinvno,oinvdt,invno,invdt,invval,portcode,sbillno,sbilldt,apptaxrate,rate,taxval,cessamt,entry_ty,tran_cd from (
select tabs='expa',lvl=1
,exptype=(case when expotype='WITH IGST' then 'WPAY' else 'WOPAY' end)
,oinvno=rtrim(ltrim(ORG_INVNO))
,oinvdt=(cast(day(org_date) as varchar)+'-'+cast(left(datename(mm,org_date),3) as varchar)+'-'+cast(right(year(org_date),2) as varchar))
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,invval=net_amt,portcode
--,sbillno=sbbillno  --Commented by Priyanka B on 12062020 for Bug-33382
,sbillno=substring(rtrim(ltrim(sbbillno)),1,7)  --Modified by Priyanka B on 12062020 for Bug-33382
,sbilldt=(cast(day(sbdate) as varchar)+'-'+cast(left(datename(mm,sbdate),3) as varchar)+'-'+cast(right(year(sbdate),2) as varchar))
,apptaxrate=0,rate=rate1
,taxval=sum(taxableamt),cessamt=sum(cess_amt1),entry_ty,tran_cd
from #GSTR1AMD
where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type ='OUT OF COUNTRY' and Ecomgstin = ''
group by expotype,inv_no,date,net_amt,portcode,sbbillno,sbdate,rate1,org_invno,org_date,entry_ty,tran_cd
)expa

If not exists(select * from #GSTR1EXCEL where tabs='expa' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,exptype,oinvno,oinvdt,invno,invdt,invval,portcode,sbillno,sbilldt,apptaxrate,rate,taxval,cessamt,entry_ty,tran_cd)
	SELECT tabs='expa',lvl=1,exptype='',oinvno='',oinvdt='',invno='',invdt='',invval=0,portcode='',sbillno='',sbilldt='',apptaxrate=0,rate=0,taxval=0,cessamt=0,entry_ty='',tran_cd=0
End

--at
print 'at'

SELECT * 
into #BANK_TMP 
FROM (
		SELECT DISTINCT TRAN_CD,ENTRY_TY,POS,pos_std_cd,DATE,INV_NO,cons_ac_name,gstin
		FROM #GSTR1TBL 
		WHERE (mentry IN('CR','BR','ST','SB') and entry_ty<>'UB')
	)bb

--select '#BANK_TMP',* from #BANK_TMP

select A.*,B.POS,B.POS_STD_CD,C.DATE,C.INV_NO,C.cons_ac_name,C.gstin
INTO #TaxAlloc_tmp 
from TaxAllocation A INNER JOIN #BANK_TMP B ON (A.Itref_tran =B.TRAN_CD AND A.REntry_ty =B.Entry_ty)
INNER JOIN #BANK_TMP C ON (A.Entry_ty =C.Entry_ty AND A.Tran_cd =C.Tran_cd) 
WHERE C.DATE BETWEEN @SDATE AND @EDATE

--select '#TaxAlloc_tmp',* from #TaxAlloc_tmp

SELECT * 
INTO #Tax_tmp 
FROM (
		select PKEY = '+', rate1,pos ,Taxableamt,CGST_AMT1,SGST_AMT1,IGST_AMT1,Cess_amt1,POS_STD_CD,inv_no,date,entry_ty,tran_cd,cons_ac_name,gstin
		from #GSTR1TBL 
		where mentry in('BR','CR') and (CGST_AMT1 + SGST_AMT1+IGST_AMT1) > 0
		union all 
		SELECT PKEY = '-',TaxRate,POS,Taxable,CGST_AMT,SGST_AMT,IGST_AMT,COMPCESS,POS_STD_CD,inv_no,date,entry_ty,tran_cd,cons_ac_name,gstin
		--inv_no,date
		FROM #TaxAlloc_tmp
	)AA

--select '#Tax_tmp',* from #Tax_tmp
--return
--level=1
print 'at - Level 1'

Insert Into #GSTR1EXCEL(tabs,lvl,pos,apptaxrate,rate,grossadv,cessamt)
select tabs,lvl,pos,apptaxrate,rate,grossadv,cessamt from (
select tabs='at',lvl=1,pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1
,grossadv = sum(case when pkey ='+' then +Taxableamt else -Taxableamt end)
,cessamt = sum(case when pkey ='+' then +CESS_AMT1 else -CESS_AMT1 end )
from #Tax_tmp 
where (isnull(SGST_AMT1,0) + isnull(CGST_AMT1,0) + isnull(IGST_AMT1,0)) > 0  
group by rate1,POS,pos_std_cd
)at order by pos,rate

If not exists(select * from #GSTR1EXCEL where tabs='at' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,pos,apptaxrate,rate,grossadv,cessamt)
	SELECT tabs='at',lvl=1,pos='',apptaxrate=0,rate=0,grossadv=0,cessamt=0
End

--level=2
print 'at - Level 2'

Insert Into #GSTR1EXCEL(tabs,lvl,pos,apptaxrate,rate,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,recname,gstin)
select tabs,lvl,pos,apptaxrate,rate,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,recname,gstin from (
select tabs='at',lvl=2,pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=rate1
,grossadv = sum(case when pkey ='+' then +Taxableamt else -Taxableamt end)
,cessamt = sum(case when pkey ='+' then +CESS_AMT1 else -CESS_AMT1 end )
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,entry_ty,tran_cd,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #Tax_tmp 
where (isnull(SGST_AMT1,0) + isnull(CGST_AMT1,0) + isnull(IGST_AMT1,0)) > 0  
group by rate1,POS,pos_std_cd,entry_ty,tran_cd,inv_no,date,cons_ac_name,gstin
)at order by invno,invdt

If not exists(select * from #GSTR1EXCEL where tabs='at' and lvl=2)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,pos,apptaxrate,rate,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,recname,gstin)
	SELECT tabs='at',lvl=2,pos='',apptaxrate=0,rate=0,grossadv=0,cessamt=0,invno='',invdt='',entry_ty='',tran_cd=0,recname='',gstin=''
End

--ata
print 'ata'

SELECT * 
into #BANK_TMP1
FROM (
		SELECT DISTINCT TRAN_CD,ENTRY_TY,POS,pos_std_cd,AMENDDATE as DATE,INV_NO,ORG_POS,cons_ac_name,gstin
		FROM #GSTR1AMD 
		WHERE (mentry IN('CR','BR','ST','SB') and entry_ty<>'UB')
	)bb

--select * from #BANK_TMP1

select A.*,B.POS,B.POS_STD_CD,C.DATE,C.INV_NO,C.ORG_POS,C.cons_ac_name,C.gstin
INTO #TaxAlloc_tmp1
from TaxAllocation A INNER JOIN #BANK_TMP1 B ON (A.Itref_tran =B.TRAN_CD AND A.REntry_ty =B.Entry_ty)
INNER JOIN #BANK_TMP1 C ON (A.Entry_ty =C.Entry_ty AND A.Tran_cd =C.Tran_cd) 
WHERE C.DATE BETWEEN @SDATE AND @EDATE

--select * from #TaxAlloc_tmp1

SELECT * 
INTO #Tax_tmp1
FROM (
		select PKEY = '+', rate1,pos ,Taxableamt,CGST_AMT1,SGST_AMT1,IGST_AMT1,Cess_amt1,POS_STD_CD,inv_no,Amenddate as date,entry_ty,tran_cd,ORG_POS,cons_ac_name,gstin
		from #GSTR1AMD 
		where mentry in('BR','CR') and (CGST_AMT1 + SGST_AMT1+IGST_AMT1) > 0
		union all 
		SELECT PKEY = '-',TaxRate,POS,Taxable,CGST_AMT,SGST_AMT,IGST_AMT,COMPCESS,POS_STD_CD,inv_no,date,entry_ty,tran_cd,ORG_POS,cons_ac_name,gstin
		FROM #TaxAlloc_tmp1
	)AA

--select * from #Tax_tmp1
--return
--level=1
print 'ata - Level 1'

Insert Into #GSTR1EXCEL(tabs,lvl,finyr,omnth,opos,apptaxrate,rate,grossadv,cessamt)
select tabs,lvl,finyr,omnth,opos,apptaxrate,rate,grossadv,cessamt from (
select tabs='ata',lvl=1,finyr=(substring(@lyn,1,5)+right(@lyn,2)),omnth=datename(mm,@SDATE)
,opos=ORG_POS,apptaxrate=0,rate=rate1
,grossadv = sum(case when pkey ='+' then +Taxableamt else -Taxableamt end)
,cessamt = sum(case when pkey ='+' then +CESS_AMT1 else -CESS_AMT1 end )
from #Tax_tmp1 
where (isnull(SGST_AMT1,0) + isnull(CGST_AMT1,0) + isnull(IGST_AMT1,0)) > 0  
group by rate1,ORG_POS
)ata order by opos,rate

If not exists(select * from #GSTR1EXCEL where tabs='ata' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,finyr,omnth,opos,apptaxrate,rate,grossadv,cessamt)
	SELECT tabs='ata',lvl=1,finyr='',omnth='',opos='',apptaxrate=0,rate=0,grossadv=0,cessamt=0
End

--level=2
print 'ata - Level 2'

Insert Into #GSTR1EXCEL(tabs,lvl,finyr,omnth,opos,apptaxrate,rate,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,recname,gstin)
select tabs,lvl,finyr,omnth,opos,apptaxrate,rate,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,recname,gstin from (
select tabs='ata',lvl=2,finyr=(substring(@lyn,1,5)+right(@lyn,2)),omnth=datename(mm,@SDATE)
,opos=ORG_POS,apptaxrate=0,rate=rate1
,grossadv = sum(case when pkey ='+' then +Taxableamt else -Taxableamt end)
,cessamt = sum(case when pkey ='+' then +CESS_AMT1 else -CESS_AMT1 end )
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,entry_ty,tran_cd,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #Tax_tmp1 
where (isnull(SGST_AMT1,0) + isnull(CGST_AMT1,0) + isnull(IGST_AMT1,0)) > 0  
group by rate1,ORG_POS,entry_ty,tran_cd,inv_no,date,cons_ac_name,gstin
)ata order by invno,invdt

If not exists(select * from #GSTR1EXCEL where tabs='ata' and lvl=2)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,finyr,omnth,opos,apptaxrate,rate,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,recname,gstin)
	SELECT tabs='ata',lvl=2,finyr='',omnth='',opos='',apptaxrate=0,rate=0,grossadv=0,cessamt=0,invno='',invdt='',entry_ty='',tran_cd=0,recname='',gstin=''
End

--atadj
print 'atadj'

SELECT A.*,b.pos,b.pos_std_cd,b.inv_no,b.date,cons_ac_name,gstin
into #TaxTemp2 
FROM TaxAllocation A 
inner join  #BANK_TMP b ON (A.Entry_ty =B.Entry_ty AND A.Tran_cd =b.Tran_cd)  
where (a.REntry_ty+QUOTENAME(a.Itref_tran) not IN(select (Entry_ty + quotename(Tran_cd))  from #BANK_TMP 
													where Entry_ty in('BR','CR') 
													and DATE between @SDATE and @EDATE)) 
and (b.date between @sdate and @edate) and a.REntry_ty  in('BR','CR')

--select * from #TaxTemp2

--level=1
print 'atadj - Level 1'

Insert Into #GSTR1EXCEL(tabs,lvl,pos,apptaxrate,rate,grossadv,cessamt)
select tabs,lvl,pos,apptaxrate,rate,grossadv,cessamt from (
select tabs='atadj',lvl=1,pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=taxrate
,grossadv = sum(taxable),cessamt = sum(COMPCESS)
from #TaxTemp2
where (isnull(SGST_AMT,0) + isnull(CGST_AMT,0) + isnull(IGST_AMT,0)) > 0  
group by taxrate,POS,pos_std_cd
)atadj order by pos,rate

If not exists(select * from #GSTR1EXCEL where tabs='atadj' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,pos,apptaxrate,rate,grossadv,cessamt)
	SELECT tabs='atadj',lvl=1,pos='',apptaxrate=0,rate=0,grossadv=0,cessamt=0
End

--level=2
print 'atadj - Level 2'

Insert Into #GSTR1EXCEL(tabs,lvl,pos,apptaxrate,rate,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,recname,gstin)
select tabs,lvl,pos,apptaxrate,rate,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,recname,gstin from (
select tabs='atadj',lvl=2,pos=pos_std_cd+'-'+pos,apptaxrate=0,rate=taxrate
,grossadv = sum(taxable),cessamt = sum(COMPCESS)
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,entry_ty,tran_cd
,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #TaxTemp2 
where (isnull(SGST_AMT,0) + isnull(CGST_AMT,0) + isnull(IGST_AMT,0)) > 0  
group by taxrate,POS,pos_std_cd,entry_ty,tran_cd,inv_no,date,cons_ac_name,gstin
)atadj order by invno,invdt

If not exists(select * from #GSTR1EXCEL where tabs='atadj' and lvl=2)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,pos,apptaxrate,rate,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,recname,gstin)
	SELECT tabs='atadj',lvl=2,pos='',apptaxrate=0,rate=0,grossadv=0,cessamt=0,invno='',invdt='',entry_ty='',tran_cd=0,recname='',gstin=''
End

--atadja
print 'atadja'

SELECT A.*,b.pos,b.pos_std_cd,b.inv_no,b.date,b.ORG_POS,cons_ac_name,gstin
into #TaxTemp3 
FROM TaxAllocation A 
inner join  #BANK_TMP1 b ON (A.Entry_ty =B.Entry_ty AND A.Tran_cd =b.Tran_cd)  
where (a.REntry_ty+QUOTENAME(a.Itref_tran) not IN(select (Entry_ty + quotename(Tran_cd))  from #BANK_TMP1 
													where Entry_ty in('BR','CR') 
													and DATE between @SDATE and @EDATE)) 
and (b.date between @sdate and @edate) and a.REntry_ty  in('BR','CR')

--select * from #TaxTemp3

--level=1
print 'atadja - Level 1'

Insert Into #GSTR1EXCEL(tabs,lvl,finyr,omnth,opos,apptaxrate,rate,grossadv,cessamt)
select tabs,lvl,finyr,omnth,opos,apptaxrate,rate,grossadv,cessamt from (
select tabs='atadja',lvl=1,finyr=(substring(@lyn,1,5)+right(@lyn,2)),omnth=datename(mm,@SDATE)
,opos=ORG_POS,apptaxrate=0,rate=taxrate
,grossadv = sum(taxable),cessamt = sum(COMPCESS)
from #TaxTemp3
where (isnull(SGST_AMT,0) + isnull(CGST_AMT,0) + isnull(IGST_AMT,0)) > 0  
group by taxrate,ORG_POS
)atadja order by opos,rate

If not exists(select * from #GSTR1EXCEL where tabs='atadja' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,finyr,omnth,opos,apptaxrate,rate,grossadv,cessamt)
	SELECT tabs='atadja',lvl=1,finyr='',omnth='',opos='',apptaxrate=0,rate=0,grossadv=0,cessamt=0
End


--level=2
print 'atadja - Level 2'

Insert Into #GSTR1EXCEL(tabs,lvl,finyr,omnth,opos,apptaxrate,rate,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,recname,gstin)
select tabs,lvl,finyr,omnth,opos,apptaxrate,rate,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,recname,gstin from (
select tabs='atadja',lvl=2,finyr=(substring(@lyn,1,5)+right(@lyn,2)),omnth=datename(mm,@SDATE)
,opos=ORG_POS,apptaxrate=0,rate=taxrate
,grossadv = sum(taxable),cessamt = sum(COMPCESS)
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,entry_ty,tran_cd
,recname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #TaxTemp3
where (isnull(SGST_AMT,0) + isnull(CGST_AMT,0) + isnull(IGST_AMT,0)) > 0  
group by taxrate,ORG_POS,entry_ty,tran_cd,inv_no,date,cons_ac_name,gstin
)atadja order by invno,invdt

If not exists(select * from #GSTR1EXCEL where tabs='atadja' and lvl=2)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,finyr,omnth,opos,apptaxrate,rate,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,recname,gstin)
	SELECT tabs='atadja',lvl=2,finyr='',omnth='',opos='',apptaxrate=0,rate=0,grossadv=0,cessamt=0,invno='',invdt='',entry_ty='',tran_cd=0,recname='',gstin=''
End

--exemp
print 'exemp'

select gstin,Entry_ty,Supp_type ,St_type ,linerule,NilAmt =(case when lineRule ='Nil Rated'  then GRO_AMT else 0.00 end)
,ExemAmt =(case when lineRule ='Exempted'  then GRO_AMT else 0.00 end),NonGstAmt =(case when (HSNCODE ='' or LineRule ='Non GST')   then GRO_AMT else 0.00 end) 
,mentry
into #Gstr1Tbl1
from #GSTR1TBL
where (lineRule in('Nil Rated','Exempted','Non GST') or HSNCODE = '' )
and st_type in('INTERSTATE','INTRASTATE') 
AND (mentry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') AND Supp_type NOT IN('EXPORT','SEZ','IMPORT','EOU')

--select * from #Gstr1Tbl1

---Inter-State supplies to registered persons
set @AMT1 = 0.00 
set @AMT2 = 0.00
set @AMT3 = 0.00
SELECT @AMT1 =sum((case when mentry in('ST','SB','DN') THEN +(NilAmt) ELSE - (NilAmt) END )),
@AMT2 =sum((case when mentry in('ST','SB','DN') THEN +(ExemAmt) ELSE - (ExemAmt) END )) ,
@AMT3 =sum((case when mentry in('ST','SB','DN') THEN +(NonGstAmt) ELSE - (NonGstAmt) END )) 
FROM #Gstr1Tbl1 
WHERE st_type = 'Interstate' and gstin <>'Unregistered'

Insert Into #GSTR1EXCEL(tabs,descr,nil,exempt,nongst)
select tabs,descr,nil,exempt,nongst from (
select tabs='exemp',descr='Inter-State supplies to registered persons',nil=isnull(@AMT1,0),exempt=isnull(@AMT2,0),nongst=isnull(@AMT3,0)
)exemp

---Intra- State supplies to registered persons
set @AMT1 = 0.00 
set @AMT2 = 0.00
set @AMT3 = 0.00
SELECT @AMT1 =sum((case when mentry in('ST','SB','DN') THEN +(NilAmt) ELSE - (NilAmt) END )),
@AMT2 =sum((case when mentry in('ST','SB','DN') THEN +(ExemAmt) ELSE - (ExemAmt) END )) ,
@AMT3 =sum((case when mentry in('ST','SB','DN') THEN +(NonGstAmt) ELSE - (NonGstAmt) END )) 
FROM #Gstr1Tbl1 WHERE st_type = 'Intrastate' and gstin <>'Unregistered'

Insert Into #GSTR1EXCEL(tabs,descr,nil,exempt,nongst)
select tabs,descr,nil,exempt,nongst from (
select tabs='exemp',descr='Intra-State supplies to registered persons',nil=isnull(@AMT1,0),exempt=isnull(@AMT2,0),nongst=isnull(@AMT3,0)
)exemp

---Inter-State supplies to Unregistered persons
set @AMT1 = 0.00 
set @AMT2 = 0.00
set @AMT3 = 0.00
SELECT @AMT1 =sum((case when mentry in('ST','SB','DN') THEN +(NilAmt) ELSE - (NilAmt) END )),
@AMT2 =sum((case when mentry in('ST','SB','DN') THEN +(ExemAmt) ELSE - (ExemAmt) END )) ,
@AMT3 =sum((case when mentry in('ST','SB','DN') THEN +(NonGstAmt) ELSE - (NonGstAmt) END )) 
FROM #Gstr1Tbl1 
WHERE st_type = 'Interstate' and gstin ='Unregistered'

Insert Into #GSTR1EXCEL(tabs,descr,nil,exempt,nongst)
select tabs,descr,nil,exempt,nongst from (
select tabs='exemp',descr='Inter-State supplies to unregistered persons',nil=isnull(@AMT1,0),exempt=isnull(@AMT2,0),nongst=isnull(@AMT3,0)
)exemp

---Intra-State supplies to Unregistered persons
set @AMT1 = 0.00 
set @AMT2 = 0.00
set @AMT3 = 0.00
SELECT @AMT1 =sum((case when mentry in('ST','SB','DN') THEN +(NilAmt) ELSE - (NilAmt) END )),
@AMT2 =sum((case when mentry in('ST','SB','DN') THEN +(ExemAmt) ELSE - (ExemAmt) END )) ,
@AMT3 =sum((case when mentry in('ST','SB','DN') THEN +(NonGstAmt) ELSE - (NonGstAmt) END )) 
FROM #Gstr1Tbl1 
WHERE st_type = 'Intrastate' and gstin ='Unregistered'

Insert Into #GSTR1EXCEL(tabs,descr,nil,exempt,nongst)
select tabs,descr,nil,exempt,nongst from (
select tabs='exemp',descr='Intra-State supplies to unregistered persons',nil=isnull(@AMT1,0),exempt=isnull(@AMT2,0),nongst=isnull(@AMT3,0)
)exemp


--HSN
print 'hsn'

select A.*,isnull(B.SERVTCODE,'') as SERVTCODE,B.SERTY
--,C.gstr_desc
,gstr_desc=(CASE WHEN B.UOM_DESC<>'' THEN B.UOM_DESC ELSE C.U_UOM END)
INTO #GSTRT1TBL_HSN  
from #GSTR1TBL A INNER JOIN IT_MAST B  ON (A.IT_CODE=B.IT_CODE)
Inner Join UOM C on (C.u_uom=B.rateunit)
where  (A.mentry In('ST','SR','CN','DN','SB')  and entry_ty<>'UB')
UPDATE #GSTRT1TBL_HSN SET SERVTCODE=ISNULL((select top 1 code from sertax_mast  where serty = #GSTRT1TBL_HSN.Serty),'')  where isnull(#GSTRT1TBL_HSN.SERVTCODE,'') = '' AND Isservice = 'services'

--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt)
select tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt from (
select tabs='hsn',lvl=1,hsn=HSNCODE
--,descr=substring(HSN_DESC,1,30)
,descr=substring([dbo].[ReplaceASCII](hsn_desc),1,30)
,uqc=gstr_desc
,totqty=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +QTY ELSE -QTY END)
--,totval=sum(CASE WHEN mentry IN('ST','DN','SB') THEN +NET_AMT ELSE -NET_AMT END)  --Commented by Priyanka B on 31072020 for Bug-33382
,totval=sum(CASE WHEN mentry IN('ST','DN','SB') THEN +GRO_AMT ELSE -GRO_AMT END)  --Modified by Priyanka B on 31072020 for Bug-33382
,taxval=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +Taxableamt ELSE -Taxableamt END)
,igstamt=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +(ISNULL(IGST_AMT,0)) ELSE -(ISNULL(IGST_AMT,0)) END)
,cgstamt=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +(ISNULL(CGST_AMT,0)) ELSE -(ISNULL(CGST_AMT,0)) END)
,sgstamt=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +(ISNULL(SGST_AMT,0)) ELSE -(ISNULL(SGST_AMT,0)) END)
,cessamt=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +(ISNULL(CESS_AMT,0)) ELSE -(ISNULL(CESS_AMT,0)) END)
FROM #GSTRT1TBL_HSN
WHERE (mentry In('ST','SR','CN','DN','SB') and entry_ty<>'UB')
and HSNCODE <>'' 
group by HSNCODE,gstr_desc,HSN_DESC
)hsn order by hsn,uqc

--select * from #GSTRT1TBL_HSN

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt,it_name)
select tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt,it_name from (
select tabs='hsn',lvl=2,hsn=HSNCODE
--,descr=substring(HSN_DESC,1,30)
,descr=substring([dbo].[ReplaceASCII](hsn_desc),1,30)
,uqc=gstr_desc
,totqty=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +QTY ELSE -QTY END)
--,totval=sum(CASE WHEN mentry IN('ST','DN','SB') THEN +NET_AMT ELSE -NET_AMT END)   --Commented by Priyanka B on 31072020 for Bug-33382
,totval=sum(CASE WHEN mentry IN('ST','DN','SB') THEN +GRO_AMT ELSE -GRO_AMT END)  --Modified by Priyanka B on 31072020 for Bug-33382
,taxval=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +Taxableamt ELSE -Taxableamt END)
,igstamt=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +(ISNULL(IGST_AMT,0)) ELSE -(ISNULL(IGST_AMT,0)) END)
,cgstamt=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +(ISNULL(CGST_AMT,0)) ELSE -(ISNULL(CGST_AMT,0)) END)
,sgstamt=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +(ISNULL(SGST_AMT,0)) ELSE -(ISNULL(SGST_AMT,0)) END)
,cessamt=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +(ISNULL(CESS_AMT,0)) ELSE -(ISNULL(CESS_AMT,0)) END)
,it_name
FROM #GSTRT1TBL_HSN
WHERE (mentry In('ST','SR','CN','DN','SB') and entry_ty<>'UB')
and HSNCODE <>'' 
group by HSNCODE,gstr_desc,HSN_DESC,it_name
)hsn order by hsn,uqc

--Level=3
Insert Into #GSTR1EXCEL(tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt,invno,invdt,entry_ty,tran_cd,it_name)
select tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt,invno,invdt,entry_ty,tran_cd,it_name from (
select tabs='hsn',lvl=3,hsn=HSNCODE
--,descr=substring(HSN_DESC,1,30)
,descr=substring([dbo].[ReplaceASCII](hsn_desc),1,30)
,uqc=gstr_desc
,totqty=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +QTY ELSE -QTY END)
--,totval=sum(CASE WHEN mentry IN('ST','DN','SB') THEN +NET_AMT ELSE -NET_AMT END)   --Commented by Priyanka B on 31072020 for Bug-33382
,totval=sum(CASE WHEN mentry IN('ST','DN','SB') THEN +GRO_AMT ELSE -GRO_AMT END)  --Modified by Priyanka B on 31072020 for Bug-33382
,taxval=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +Taxableamt ELSE -Taxableamt END)
,igstamt=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +(ISNULL(IGST_AMT,0)) ELSE -(ISNULL(IGST_AMT,0)) END)
,cgstamt=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +(ISNULL(CGST_AMT,0)) ELSE -(ISNULL(CGST_AMT,0)) END)
,sgstamt=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +(ISNULL(SGST_AMT,0)) ELSE -(ISNULL(SGST_AMT,0)) END)
,cessamt=SUM(CASE WHEN mentry IN('ST','DN','SB') THEN +(ISNULL(CESS_AMT,0)) ELSE -(ISNULL(CESS_AMT,0)) END)
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,entry_ty,tran_cd
,it_name
FROM #GSTRT1TBL_HSN
WHERE (mentry In('ST','SR','CN','DN','SB') and entry_ty<>'UB')
and HSNCODE <>'' 
group by HSNCODE,gstr_desc,HSN_DESC,inv_no,date,entry_ty,tran_cd,it_name
)hsn order by hsn,uqc,invno,invdt,entry_ty,tran_cd

If not exists(select * from #GSTR1EXCEL where tabs='hsn' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt)
	SELECT tabs='hsn',lvl=1,hsn='',descr='',uqc='',totqty=0,totval=0,taxval=0,igstamt=0,cgstamt=0,sgstamt=0,cessamt=0
End

--docs
print 'docs'
--Invoices for outward supply
--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Invoices for outward supply'
,frmsrno=min(inv_no)
,tosrno=max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM STMAIN a
inner join lcode l on (a.entry_ty=l.entry_ty)
WHERE a.entry_ty='ST'
--(case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end) IN ('ST') and a.entry_ty<>'UB'
and ( date between @sdate and @edate ) 
GROUP BY INV_SR
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Invoices for outward supply'
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM STMAIN a
inner join lcode l on (a.entry_ty=l.entry_ty)
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE a.entry_ty='ST'
and ( date between @sdate and @edate ) 
)docs order by invno,invdt


If not exists(select * from #GSTR1EXCEL where tabs='docs' and natofdoc='Invoices for outward supply' and lvl=1)
Begin
	Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
	SELECT tabs='docs',lvl=1,natofdoc='Invoices for outward supply',frmsrno='',tosrno='',totno=0,cancld=0
End

--Level=1
IF EXISTS(SELECT * FROM STMAIN WHERE ENTRY_TY='EI' and ( date between @sdate and @edate ))
BEGIN
	Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
	select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
	select tabs='docs',lvl=1,natofdoc='Invoices for outward supply'
	,frmsrno=min(inv_no)
	,tosrno=max(inv_no)
	,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
	,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
	FROM STMAIN a
	inner join lcode l on (a.entry_ty=l.entry_ty)
	WHERE a.entry_ty='EI'
	--(case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end) IN ('ST') and a.entry_ty<>'UB'
	and ( date between @sdate and @edate ) 
	GROUP BY INV_SR
	)docs
END

--Level=2
IF EXISTS(SELECT * FROM STMAIN WHERE ENTRY_TY='EI' and ( date between @sdate and @edate ))
BEGIN
	Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
	select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
	select tabs='docs',lvl=2,natofdoc='Invoices for outward supply'
	,invno=rtrim(ltrim(inv_no))
	,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
	,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
	FROM STMAIN a
	inner join lcode l on (a.entry_ty=l.entry_ty)
	inner join ac_mast b on (a.ac_id=b.ac_id)
	WHERE a.entry_ty='EI'
	and ( date between @sdate and @edate ) 
	)docs order by invno,invdt
END

--Level=1
IF EXISTS(SELECT * FROM SBMAIN WHERE ENTRY_TY='S1' and ( date between @sdate and @edate ))
BEGIN
	Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
	select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
	select tabs='docs',lvl=1,natofdoc='Invoices for outward supply'
	,frmsrno=min(inv_no)
	,tosrno=max(inv_no)
	,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
	,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
	FROM SBMAIN 
	WHERE ENTRY_TY='S1' and ( date between @sdate and @edate )
	GROUP BY INV_SR
	)docs
END

--Level=2
IF EXISTS(SELECT * FROM SBMAIN WHERE ENTRY_TY='S1' and ( date between @sdate and @edate ))
BEGIN
	Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
	select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
	select tabs='docs',lvl=2,natofdoc='Invoices for outward supply'
	,invno=rtrim(ltrim(inv_no))
	,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
	,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
	FROM SBMAIN a
	inner join ac_mast b on (a.ac_id=b.ac_id)
	WHERE ENTRY_TY='S1' and ( date between @sdate and @edate )
	)docs order by invno,invdt
END

--Invoices for inward supply from unregistered person
--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Invoices for inward supply from unregistered person'
,frmsrno=min(inv_no)
,tosrno=max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM STMAIN a
inner join lcode l on (a.entry_ty=l.entry_ty)
WHERE a.entry_ty='UB'
--(case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end) IN ('ST') and a.entry_ty<>'UB'
and ( date between @sdate and @edate ) 
GROUP BY INV_SR
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Invoices for inward supply from unregistered person'
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM STMAIN a
inner join lcode l on (a.entry_ty=l.entry_ty)
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE a.entry_ty='UB'
--(case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end) IN ('ST') and a.entry_ty<>'UB'
and ( date between @sdate and @edate ) 
)docs order by invno,invdt

--Revised Invoice
SELECT DISTINCT CONS_AC_NAME as PARTY_NM,AMENDDATE,INV_NO,ENTRY_TY,TRAN_CD INTO #REVINVNO FROM #GSTR1AMD

--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Revised Invoice'
,frmsrno=min(inv_no)
,tosrno=max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM #REVINVNO a
left join lcode l on (a.entry_ty=l.entry_ty)
WHERE (amenddate between @sdate and @edate ) 
GROUP BY a.entry_ty,l.code_nm
--Order by 'Revised Invoice ('+ltrim(rtrim(upper(b.code_nm)))+')'
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Revised Invoice'
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(AMENDDATE) as varchar)+'-'+cast(left(datename(mm,AMENDDATE),3) as varchar)+'-'+cast(right(year(AMENDDATE),2) as varchar))
,recname=rtrim(ltrim(PARTY_NM)),b.gstin,a.entry_ty,a.tran_cd
FROM #REVINVNO a
left join lcode l on (a.entry_ty=l.entry_ty)
inner join ac_mast b on (a.PARTY_NM=b.ac_name)
WHERE (amenddate between @sdate and @edate ) 
)docs order by invno,invdt

--Debit Note
--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Debit Note'
,frmsrno=min(inv_no)
,tosrno=max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM DNMAIN 
WHERE ENTRY_TY IN('GD') and ( date between @sdate and @edate )  AND AGAINSTGS ='SALES'  
GROUP BY INV_SR
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Debit Note'
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM DNMAIN a
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE ENTRY_TY IN('GD') and ( date between @sdate and @edate )  AND AGAINSTGS ='SALES'  
)docs order by invno,invdt

--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Debit Note'
,frmsrno=min(inv_no)
,tosrno=max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM DNMAIN 
WHERE ENTRY_TY IN('D6') and ( date between @sdate and @edate )  AND AGAINSTGS ='SALES'  
GROUP BY INV_SR
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Debit Note'
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM DNMAIN a
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE ENTRY_TY IN('D6') and ( date between @sdate and @edate )  AND AGAINSTGS ='SALES'  
)docs order by invno,invdt

--Credit Note
--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Credit Note'
,frmsrno=min(inv_no)
,tosrno=max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM CNMAIN 
WHERE ENTRY_TY IN('GC') and ( date between @sdate and @edate )  AND AGAINSTGS ='SALES'  
GROUP BY INV_SR
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Credit Note'
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM CNMAIN a
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE ENTRY_TY IN('GC') and ( date between @sdate and @edate )  AND AGAINSTGS ='SALES'  
)docs order by invno,invdt

--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Credit Note'
,frmsrno=min(inv_no)
,tosrno=max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM CNMAIN 
WHERE ENTRY_TY IN('C6') and ( date between @sdate and @edate )  AND AGAINSTGS ='SALES'  
GROUP BY INV_SR
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Credit Note'
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM CNMAIN a
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE ENTRY_TY IN('C6') and ( date between @sdate and @edate )  AND AGAINSTGS ='SALES'  
)docs order by invno,invdt

--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Credit Note'
,frmsrno=min(inv_no)
,tosrno=max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM SRMAIN 
WHERE ENTRY_TY IN('SR') and ( date between @sdate and @edate )
GROUP BY INV_SR
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Credit Note'
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM SRMAIN a
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE ENTRY_TY IN('SR') and ( date between @sdate and @edate )
)docs order by invno,invdt

--Receipt voucher
--Bank Receipt
--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Receipt Voucher'
,frmsrno=min(inv_no)
,tosrno=max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM BRMAIN 
WHERE ENTRY_TY IN('BR') and ( date between @sdate and @edate ) 
GROUP BY inv_sr 
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Receipt Voucher'
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM BRMAIN a
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE ENTRY_TY IN('BR') and ( date between @sdate and @edate ) 
)docs order by invno,invdt


--Cash Receipt 
--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Receipt Voucher'
,frmsrno=min(inv_no)
,tosrno=max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM CRMAIN 
WHERE ENTRY_TY IN('CR') and ( date between @sdate and @edate ) 
GROUP BY inv_sr
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Receipt Voucher'
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM CRMAIN a
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE ENTRY_TY IN('CR') and ( date between @sdate and @edate ) 
)docs order by invno,invdt


--Payment Voucher
--Bank Payment
--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Payment Voucher'
,frmsrno=min(inv_no)
,tosrno=max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM BPMAIN 
WHERE ENTRY_TY IN('BP') and ( date between @sdate and @edate ) 
GROUP BY inv_sr
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Payment Voucher'
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM BPMAIN a
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE ENTRY_TY IN('BP') and ( date between @sdate and @edate ) 
)docs order by invno,invdt
	
--Cash Payment
--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Payment Voucher'
,frmsrno=min(inv_no)
,tosrno=max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM CPMAIN 
WHERE ENTRY_TY IN('CP') and ( date between @sdate and @edate ) 
GROUP BY inv_sr
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Payment Voucher'
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM CPMAIN a
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE ENTRY_TY IN('CP') and ( date between @sdate and @edate ) 
)docs order by invno,invdt


--Refund voucher
--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Refund Voucher'
,frmsrno=min(inv_no)
,tosrno=max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM BPMAIN
WHERE ENTRY_TY IN('RV') and ( date between @sdate and @edate ) 
GROUP BY inv_sr
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Refund Voucher'
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM BPMAIN a
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE ENTRY_TY IN('RV') and ( date between @sdate and @edate ) 
)docs order by invno,invdt


--Delivery Challan for job work
--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Delivery Challan for job work'
,frmsrno=entry_ty+'/'+min(inv_no)
,tosrno=entry_ty+'/'+max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM IIMAIN
WHERE ENTRY_TY IN('LI') and ( date between @sdate and @edate ) 
GROUP BY inv_sr,entry_ty
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Delivery Challan for job work'
,invno=entry_ty+'/'+rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM IIMAIN a
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE ENTRY_TY IN('LI') and ( date between @sdate and @edate ) 
)docs order by invno,invdt

--Delivery Challan for supply on approval
--Insert Into #GSTR1EXCEL(tabs,natofdoc,frmsrno,tosrno,totno,cancld)
--select tabs,natofdoc,frmsrno,tosrno,totno,cancld from (
--select tabs='docs',natofdoc='Delivery Challan for supply on approval'
--,frmsrno='',tosrno='',totno=0,cancld=0
--)docs

--Delivery Challan in case of liquid gas
--Insert Into #GSTR1EXCEL(tabs,natofdoc,frmsrno,tosrno,totno,cancld)
--select tabs,natofdoc,frmsrno,tosrno,totno,cancld from (
--select tabs='docs',natofdoc='Delivery Challan in case of liquid gas'
--,frmsrno='',tosrno='',totno=0,cancld=0
--)docs

--Delivery Challan in cases other than by way of supply (excluding at S no. 9 to 11)
--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Delivery Challan in cases other than by way of supply (excluding at S no. 9 to 11)'
,frmsrno=entry_ty+'/'+min(inv_no)
,tosrno=entry_ty+'/'+max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM IIMAIN 
WHERE ENTRY_TY IN('IL') and ( date between @sdate and @edate )
GROUP BY inv_sr	,entry_ty
)docs

--Level=2
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=1,natofdoc='Delivery Challan in cases other than by way of supply (excluding at S no. 9 to 11)'
,invno=entry_ty+'/'+rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM IIMAIN a
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE ENTRY_TY IN('IL') and ( date between @sdate and @edate )
)docs order by invno,invdt

--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld)
select tabs,lvl,natofdoc,frmsrno,tosrno,totno,cancld from (
select tabs='docs',lvl=1,natofdoc='Delivery Challan in cases other than by way of supply (excluding at S no. 9 to 11)'
,frmsrno=entry_ty+'/'+min(inv_no)
,tosrno=entry_ty+'/'+max(inv_no)
,totno=(sum((Case when PARTY_NM !='' then 1 else 0 end)))
,cancld=(sum((Case when PARTY_NM ='CANCELLED.' then 1 else 0 end)))
FROM DCMAIN 
WHERE ENTRY_TY IN('DC') and ( date between @sdate and @edate ) and ISNULL(MTRNTYPE,'')='Branch Transfer'  
GROUP BY inv_sr	,entry_ty  
)docs

--Level=1
Insert Into #GSTR1EXCEL(tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd)
select tabs,lvl,natofdoc,invno,invdt,recname,gstin,entry_ty,tran_cd from (
select tabs='docs',lvl=2,natofdoc='Delivery Challan in cases other than by way of supply (excluding at S no. 9 to 11)'
,invno=entry_ty+'/'+rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,recname=rtrim(ltrim(party_nm)),b.gstin,a.entry_ty,a.tran_cd
FROM DCMAIN a
inner join ac_mast b on (a.ac_id=b.ac_id)
WHERE ENTRY_TY IN('DC') and ( date between @sdate and @edate ) and ISNULL(MTRNTYPE,'')='Branch Transfer'  
)docs order by invno,invdt

Update #GSTR1EXCEL set invdt=case when invdt='1-Jan-00' then '' else invdt end
,oinvdt=case when oinvdt='1-Jan-00' then '' else oinvdt end
,nrvdt=case when nrvdt='1-Jan-00' then '' else nrvdt end
,onrvdt=case when onrvdt='1-Jan-00' then '' else onrvdt end
,sbilldt=case when sbilldt='1-Jan-00' then '' else sbilldt end
,ecomgstin=''

--SELECT RANK() OVER (PARTITION BY tabs ORDER BY tranid) AS RANK

/*Update a set a.SerialNo=isnull(a.SerialNo,0)+1 
from #GSTR1EXCEL a, #GSTR1EXCEL b 
where a.tabs=b.tabs
*/

select RANK() OVER (PARTITION BY tabs,lvl
--,(case when tabs='docs' then descr else '' end) 
ORDER BY tranid) AS SerialNo
,* from #GSTR1EXCEL
--where tabs='hsn' --and lvl=1
order by tranid

/*
select RANK() OVER (PARTITION BY tabs,lvl ORDER BY tranid) AS SerialNo
,* from #GSTR1EXCEL
where tabs='hsn' and lvl=2
order by tranid


select RANK() OVER (PARTITION BY tabs,lvl ORDER BY tranid) AS SerialNo
,* from #GSTR1EXCEL
where tabs='hsn' and lvl=3
order by tranid
*/
--select distinct tabs,'case "'+tabs+'":'+char(13)+'break;',tranid from #GSTR1EXCEL
--where lvl=2
------group by tabs,tranid
--order by tranid

END
go
--set dateformat dmy EXECUTE Usp_Rep_GSTR1_EXCEL'','','','01/04/2019 ','31/03/2020','','','','',0,0,'','','','','','','','','2020-2021',''