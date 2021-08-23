IF EXISTS(SELECT * FROM SYSOBJECTS WHERE XTYPE='P' AND [NAME]='Usp_Rep_GSTR2_EXCEL')
BEGIN
	DROP PROCEDURE Usp_Rep_GSTR2_EXCEL
END
GO
CREATE Procedure [dbo].[Usp_Rep_GSTR2_EXCEL]
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
,tabs=cast('' as varchar(10))
,lvl=cast(0 as int)
,entry_ty=cast('' as char(2))
,tran_cd=cast(0 as int)
,gstin=cast('' as varchar(15))
,invno=cast('' as varchar(16))
,invdt=cast('' as varchar(15))
,invval=cast(0.00 as decimal(18,2))
,pos=cast('' as varchar(50))
,revchg=cast('' as char(1))
,invtype=cast('' as varchar(50))
,rate=cast(0.00 as decimal(5,2))
,taxval=cast(0.00 as decimal(18,2))
,igstpaid=cast(0.00 as decimal(18,2))
,cgstpaid=cast(0.00 as decimal(18,2))
,sgstpaid=cast(0.00 as decimal(18,2))
,cesspaid=cast(0.00 as decimal(18,2))
,itceligible=cast('' as varchar(50))
,igstavail=cast(0.00 as decimal(18,2))
,cgstavail=cast(0.00 as decimal(18,2))
,sgstavail=cast(0.00 as decimal(18,2))
,cessavail=cast(0.00 as decimal(18,2))
,supname=cast('' as varchar(100))
,suptype=cast('' as varchar(20))
,portcode=cast('' as varchar(15))
,boeno=cast('' as varchar(16))
,boedt=cast('' as varchar(15))
,boeval=cast(0.00 as decimal(18,2))
,doctype=cast('' as varchar(50))
,nrvno=cast('' as varchar(16))
,nrvdt=cast('' as varchar(15))
,pregst=cast('' as char(1))
,cddoctype=cast('' as char(1))
,reason=cast('' as varchar(50))
,nrvval=cast(0.00 as decimal(18,2))
,grossadv=cast(0.00 as decimal(18,2))
,cessamt=cast(0.00 as decimal(18,2))
,descr=cast('' as varchar(100))
,comptax=cast(0.00 as decimal(18,2))
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
,it_name=cast('' as varchar(100))
,addred=cast('' as varchar(50))
into #GSTR2EXCEL
FROM  PTMAIN H 
INNER JOIN PTITEM D ON (H.ENTRY_TY = D .ENTRY_TY AND H.TRAN_CD = D .TRAN_CD) 
INNER JOIN IT_MAST IT ON (D .IT_CODE = IT.IT_CODE) 
LEFT OUTER JOIN shipto ON (shipto.shipto_id = h.scons_id) 
LEFT OUTER JOIN ac_mast ac ON (h.cons_id = ac.ac_id)  
WHERE 1=2

/* GSTR2_VW DATA STORED IN TEMPORARY TABLE*/
Declare @amt1 decimal(18,2),@amt2 decimal(18,2),@amt3 decimal(18,2),@amt4 decimal(18,2)
--,@amt5 decimal(18,2),@amt6 decimal(18,2),@AgainstType varchar(50),@amenddate smalldatetime

SELECT DISTINCT HSN_CODE, CAST(HSN_DESC AS VARCHAR(250)) AS HSN_DESC INTO #HSN1 FROM HSN_MASTER

SELECT mEntry=case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end,
RATE1= (case when (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0)) >0  then (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0))when (isnull(A.IGST_PER,0)) > 0 then (isnull(A.IGST_PER,0))else isnull(A.gstrate,0) end)
,iigst_amt=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((ISNULL(a.igst_amt,0)+ ISNULL(a.IGSRT_AMT,0))-ISNULL(b.iigst_amt,0)) else 0 end)
,icgst_amt=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((ISNULL(a.cgst_amt,0) + ISNULL(a.CGSRT_AMT,0)) - ISNULL(b.icgst_amt,0)) else 0 end)
,isGST_AMT=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((isnull(a.sGST_AMT,0)+ isnull(a.SGSRT_AMT,0))-isnull(b.isGST_AMT,0)) else 0 end)
,ICESS_AMT =(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((isnull(a.CESS_AMT,0)+ isnull(a.CessRT_amt,0)) - isnull(B.ICESS_AMT,0)) else 0 end)
,A.* 
,HSN_DESC=ISNULL((SELECT TOP 1 HSN_DESC FROM #HSN1 WHERE HSN_CODE = A.HSNCODE),'')
,invtype=(case when A.supp_type='SEZ' and A.imptype='WITH IGST' then 'SEZ supplies with payment' else 
			(case when A.supp_type='SEZ' and A.imptype='WITHOUT IGST' then 'SEZ supplies without payment' else 
			(case when A.st_type in ('INTRASTATE','INTERSTATE') and A.supp_type in ('EXPORT','EOU','IMPORT') then 'Deemed Exp' else 
			'Regular' end) end) end)
INTO #GSTR2TBL 
FROM GSTR2_VW A  
LEFT OUTER JOIN Epayment B ON (A.Tran_cd =B.tran_cd AND A.Entry_ty =B.entry_ty AND A.ITSERIAL =B.itserial)  
inner join lcode l  on (a.entry_ty=l.entry_ty)
WHERE (A.DATE BETWEEN @SDATE AND @EDATE) and A.AmendDate=''
		
SELECT mEntry=case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end,
RATE1= (case when (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0)) >0  then (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0))when (isnull(A.IGST_PER,0)) > 0 then (isnull(A.IGST_PER,0))else isnull(A.gstrate,0) end)
,iigst_amt=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((ISNULL(a.igst_amt,0)+ ISNULL(a.IGSRT_AMT,0))-ISNULL(b.iigst_amt,0)) else 0 end)
,icgst_amt=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((ISNULL(a.cgst_amt,0) + ISNULL(a.CGSRT_AMT,0)) - ISNULL(b.icgst_amt,0)) else 0 end)
,isGST_AMT=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((isnull(a.sGST_AMT,0)+ isnull(a.SGSRT_AMT,0))-isnull(b.isGST_AMT,0)) else 0 end)
,ICESS_AMT =(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((isnull(a.CESS_AMT,0)+ isnull(a.CessRT_amt,0)) - isnull(B.ICESS_AMT,0)) else 0 end)
,A.* 
,HSN_DESC=ISNULL((SELECT TOP 1 HSN_DESC FROM #HSN1 WHERE HSN_CODE = A.HSNCODE),'')
,invtype=(case when A.supp_type='SEZ' and A.imptype='WITH IGST' then 'SEZ supplies with payment' else 
			(case when A.supp_type='SEZ' and A.imptype='WITHOUT IGST' then 'SEZ supplies without payment' else 
			(case when A.st_type in ('INTRASTATE','INTERSTATE') and A.supp_type in ('EXPORT','EOU','IMPORT') then 'Deemed Exp' else 
			'Regular' end) end) end)
INTO #GSTR2AMD 
FROM GSTR2_VW A 
LEFT OUTER JOIN Epayment B  ON(A.Entry_ty =B.entry_ty AND A.Tran_cd=B.tran_cd AND A.ITSERIAL =B.itserial ) 
inner join lcode l  on (a.entry_ty=l.entry_ty)
WHERE (A.AmendDate BETWEEN @SDATE AND @EDATE)

/*Temporary Table for Amended and not amended records End */		


--b2b
print 'b2b'

Insert Into #GSTR2EXCEL(tabs,lvl,gstin,invno,invdt,invval,pos,revchg,invtype,rate,taxval,igstpaid,cgstpaid,sgstpaid,cesspaid,itceligible,igstavail,cgstavail,sgstavail,cessavail,entry_ty,tran_cd)
SELECT tabs,lvl,gstin,invno,invdt,invval,pos,revchg,invtype,rate,taxval,igstpaid,cgstpaid,sgstpaid,cesspaid,itceligible,igstavail,cgstavail,sgstavail,cessavail,entry_ty,tran_cd FROM (
Select tabs='b2b',lvl=1,gstin,invno=rtrim(ltrim(pinvno))
,invdt=(cast(day(pinvdt) as varchar)+'-'+cast(left(datename(mm,pinvdt),3) as varchar)+'-'+cast(right(year(pinvdt),2) as varchar))
,invval=net_amt,pos=pos_std_cd+'-'+pos,revchg='N',invtype,rate=rate1,taxval=sum(TaxableAmt)
,igstpaid=sum(igst_amt),cgstpaid=sum(cgst_amt),sgstpaid=sum(sgst_amt),cesspaid=sum(cess_amt)
,itceligible
,igstavail=sum(iigst_amt),cgstavail=sum(icgst_amt),sgstavail=sum(isgst_amt),cessavail=sum(icess_amt)
,entry_ty,tran_cd
FROM #GSTR2TBL 
where mENTRY IN ('EP','PT') 
AND ST_TYPE <> 'Out of Country' and SUPP_TYPE in('Registered','E-commerce')
and ltrim(rtrim(HSNCODE)) <>'' 
And gstin not in('Unregistered')AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) = 0 
AND (CGST_AMT + SGST_AMT + IGST_AMT) > 0  AND LineRule = 'Taxable'
GROUP BY pinvno,pinvdt,Net_amt,Rate1,gstin,pos,gstype,pos_std_cd,invtype,itceligible,entry_ty,tran_cd
)b2b ORDER BY invno,invdt,rate

If not exists(select * from #GSTR2EXCEL where tabs='b2b')
Begin
	Insert Into #GSTR2EXCEL(tabs,lvl,gstin,invno,invdt,invval,pos,revchg,invtype,rate,taxval,igstpaid,cgstpaid,sgstpaid,cesspaid,itceligible,igstavail,cgstavail,sgstavail,cessavail,entry_ty,tran_cd)
	SELECT tabs='b2b',lvl=1,gstin='',invno='',invdt='',invval=0,pos='',revchg='',invtype='',rate=0,taxval=0,igstpaid=0,cgstpaid=0,sgstpaid=0,cesspaid=0,itceligible='',igstavail=0,cgstavail=0,sgstavail=0,cessavail=0,entry_ty='',tran_cd=0
End

--b2bur
print 'b2bur'

select ENTRY_ALL=case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end,
Main_tran,ENTRY_ALL as mENTRY_ALL 
into #rcmpay 
from mainall_vw a
inner join lcode l  on (a.entry_all=l.entry_ty)
where a.entry_ty ='gb' and date<=@EDATE 
group by  Main_tran,ENTRY_ALL,bcode_nm,ext_vou,l.entry_ty

select A.*,isnull(b.main_tran,0) as pay  into #GSTR2TBL_RCM from  #GSTR2TBL A 
left outer join #rcmpay b on (a.tran_cd =b.main_tran and a.entry_ty =b.entry_all)

SELECT A.ENTRY_ALL,A.Main_tran ,SUM((CASE WHEN B.AC_NAME = 'Central GST Payable A/C '  THEN A.new_all ELSE 0 END ))  AS CGST,
SUM((CASE WHEN B.AC_NAME = 'State GST Payable A/C '  THEN A.new_all ELSE 0 END ))  AS SGST ,
SUM((CASE WHEN B.AC_NAME = 'Integrated GST Payable A/C '  THEN A.new_all ELSE 0 END ))  AS IGST 
,SUM(CASE WHEN B.AC_NAME = 'Compensation Cess Payable A/C '  THEN A.new_all ELSE 0 END )  AS CESS
INTO #RMCPAYMENT from mainall_vw A LEFT OUTER JOIN AC_MAST  B ON (A.Ac_id =B.Ac_id) where A.ENTRY_TY = 'GB' AND A.DATE BETWEEN @SDATE AND @EDATE  AND (A.ENTRY_ALL+QUOTENAME(A.Main_tran)) IN (SELECT (ENTRY_TY+QUOTENAME(Tran_cd)) FROM #GSTR2TBL_RCM )
GROUP BY A.ENTRY_ALL,A.Main_tran 
ORDER BY A.ENTRY_ALL,A.Main_tran	 

DELETE FROM #RMCPAYMENT WHERE ENTRY_ALL NOT IN('PT','P1','E1','UB','PK')

DECLARE @ENTRY_ALL VARCHAR(2),@MAIN_TRAN INT,@CGST DECIMAL(18,2),@SGST DECIMAL(18,2),@IGST DECIMAL(18,2),@CESS DECIMAL(18,2)
,@ENTRY_TY VARCHAR(2),@TRAN_CD INT,@CGST1 DECIMAL(18,2),@SGST1 DECIMAL(18,2),@IGST1 DECIMAL(18,2),@CESS1 DECIMAL(18,2),@itserial varchar(20)
,@CGST2 DECIMAL(18,2),@SGST2 DECIMAL(18,2),@IGST2 DECIMAL(18,2),@CESS2 DECIMAL(18,2)
DECLARE CUR_RCM CURSOR FOR SELECT  ENTRY_ALL,MAIN_TRAN,CGST,SGST,IGST,CESS FROM #RMCPAYMENT
OPEN CUR_RCM
FETCH CUR_RCM INTO @ENTRY_ALL,@MAIN_TRAN,@CGST,@SGST,@IGST,@CESS
WHILE(@@FETCH_STATUS =0)
BEGIN
SET @CGST2 = 0
SET @SGST2 = 0
SET @IGST2 = 0
SET @CESS2 = 0
DECLARE M_RCM CURSOR FOR SELECT Entry_ty,Tran_cd,icgst_amT,isGST_AMT,iigst_amt,ICESS_AMT,itserial FROM  #GSTR2TBL_RCM WHERE Entry_ty = @ENTRY_ALL AND Tran_cd = @MAIN_TRAN ORDER BY Entry_ty,Tran_cd,Rate1  
OPEN M_RCM
FETCH M_RCM INTO @ENTRY_TY,@TRAN_CD,@CGST1,@SGST1,@IGST1,@CESS1,@itserial
WHILE(@@FETCH_STATUS =0)
BEGIN
			
	if @CGST <> 0  OR  @CGST  = 0
		BEGIN
			SET @CGST2 = @CGST - @CGST1
			update  #GSTR2TBL_RCM set icgst_amT = CASE WHEN @CGST2 < 0  THEN (CASE WHEN @CGST <=0  THEN 0 ELSE @CGST END )  ELSE @CGST1 END WHERE Tran_cd = @TRAN_CD AND Entry_ty =@ENTRY_TY AND ITSERIAL =@itserial
			SET @CGST = @CGST- @CGST1
		END 
	if @SGST <> 0  OR  @SGST  = 0
		BEGIN
			SET @SGST2 = @SGST - @SGST1
			update  #GSTR2TBL_RCM set isGST_AMT = CASE WHEN @SGST2 < 0  THEN  (CASE WHEN @SGST <= 0  THEN 0 ELSE @SGST END ) ELSE @SGST1 END WHERE Tran_cd = @TRAN_CD AND Entry_ty =@ENTRY_TY AND ITSERIAL =@itserial 
			SET @SGST = @SGST- @SGST1
		END 
	if @IGST <> 0  OR  @IGST  = 0
		BEGIN
			SET @IGST2 = @IGST - @IGST1
			update  #GSTR2TBL_RCM set iigst_amt = CASE WHEN @IGST2 < 0  THEN (CASE WHEN @IGST  <= 0  THEN 0 ELSE @IGST  END )ELSE @IGST1 END WHERE Tran_cd = @TRAN_CD AND Entry_ty =@ENTRY_TY AND ITSERIAL =@itserial 
			SET @IGST = @IGST-@IGST1

		END 
     				
	if @CESS <> 0  OR  @CESS  = 0
		BEGIN
			SET @CESS2 = @CESS - @CESS1
			update  #GSTR2TBL_RCM set ICESS_AMT = CASE WHEN @CESS2 < 0   THEN (CASE WHEN @CESS   <= 0  THEN 0 ELSE @CESS   END )ELSE @CESS1 END WHERE Tran_cd = @TRAN_CD AND Entry_ty =@ENTRY_TY AND ITSERIAL =@itserial 
			SET @CESS = @CESS- @CESS1

		END
	FETCH  M_RCM INTO @ENTRY_TY,@TRAN_CD,@CGST1,@SGST1,@IGST1,@CESS1,@itserial
END
CLOSE M_RCM
DEALLOCATE M_RCM
FETCH CUR_RCM INTO @ENTRY_ALL,@MAIN_TRAN,@CGST,@SGST,@IGST,@CESS	
END
CLOSE CUR_RCM
DEALLOCATE CUR_RCM

Insert Into #GSTR2EXCEL(tabs,lvl,supname,invno,invdt,invval,pos,suptype,rate,taxval,igstpaid,cgstpaid,sgstpaid,cesspaid,itceligible,igstavail,cgstavail,sgstavail,cessavail,entry_ty,tran_cd)
SELECT tabs,lvl,supname,invno,invdt,invval,pos,suptype,rate,taxval,igstpaid,cgstpaid,sgstpaid,cesspaid,itceligible,igstavail,cgstavail,sgstavail,cessavail,entry_ty,tran_cd FROM (
Select tabs='b2bur',lvl=1,supname=rtrim(ltrim(cons_ac_name))
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,invval=net_amt,pos=pos_std_cd+'-'+pos
,suptype=(case when st_type='INTRASTATE' then 'Intra State' else 
		(case when st_type='INTERSTATE' then 'Inter State' else st_type end) end)
,rate=Rate1
,taxval=sum(TaxableAmt)
,igstpaid=sum(igst_amt),cgstpaid=sum(cgst_amt),sgstpaid=sum(sgst_amt),cesspaid=sum(cess_amt)
,itceligible
,igstavail=SUM(case when pay > 0 then  iigst_amt else 0 end)
,cgstavail=SUM(case when pay > 0 then  icgst_amt else 0 end)
,sgstavail=SUM(case when pay > 0 then  isGST_AMT else 0 end)
,cessavail=SUM(case when pay > 0 then  ICESS_AMT else 0 end)
,entry_ty,tran_cd
FROM #GSTR2TBL_RCM
where ENTRY_TY IN ('UB') 
GROUP BY INV_NO,DATE,Net_amt,Rate1,pos,pos_std_cd,itceligible,cons_ac_name,entry_ty,tran_cd,st_type
)b2bur ORDER BY invno,invdt,rate

If not exists(select * from #GSTR2EXCEL where tabs='b2bur')
Begin
	Insert Into #GSTR2EXCEL(tabs,lvl,supname,invno,invdt,invval,pos,suptype,rate,taxval,igstpaid,cgstpaid,sgstpaid,cesspaid,itceligible,igstavail,cgstavail,sgstavail,cessavail,entry_ty,tran_cd)
	SELECT tabs='b2bur',lvl=1,supname='',invno='',invdt='',invval=0,pos='',suptype='',rate=0,taxval=0,igstpaid=0,cgstpaid=0,sgstpaid=0,cesspaid=0,itceligible='',igstavail=0,cgstavail=0,sgstavail=0,cessavail=0,entry_ty='',tran_cd=0
End

--imps
print 'imps'

Insert Into #GSTR2EXCEL(tabs,lvl,invno,invdt,invval,pos,rate,taxval,igstpaid,cesspaid,itceligible,igstavail,cessavail,entry_ty,tran_cd)
select tabs,lvl,invno,invdt,invval,pos,rate,taxval,igstpaid,cesspaid,itceligible,igstavail,cessavail,entry_ty,tran_cd from (
Select tabs='imps',lvl=1
,invno=rtrim(ltrim(pinvno))
,invdt=(cast(day(pinvdt) as varchar)+'-'+cast(left(datename(mm,pinvdt),3) as varchar)+'-'+cast(right(year(pinvdt),2) as varchar))
,invval=net_amt,pos=pos_std_cd+'-'+pos,rate=rate1,taxval=SUM(TaxableAmt)
,igstpaid=sum(igsrt_amt),cesspaid=sum(cessrt_amt)
,itceligible
,igstavail=SUM(case when pay > 0 then  iigst_amt else 0 end)
,cessavail=SUM(case when pay > 0 then  ICESS_AMT else 0 end)
,entry_ty,tran_cd
FROM #GSTR2TBL_RCM 
where mENTRY IN ('EP','PT')
and Isservice = 'Services'
AND ST_TYPE IN('Out of Country','Interstate','Intrastate') and SUPP_TYPE in('Import','SEZ','EOU','','EXPORT')
AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) > 0
GROUP BY pinvno,pinvdt,Net_amt,Rate1,pos,pos_std_cd,itceligible,entry_ty,tran_cd
)imps ORDER BY invno,invdt,rate

If not exists(select * from #GSTR2EXCEL where tabs='imps' and lvl=1)
Begin
	Insert Into #GSTR2EXCEL(tabs,lvl,invno,invdt,invval,pos,rate,taxval,igstpaid,cesspaid,itceligible,igstavail,cessavail,entry_ty,tran_cd)
	SELECT tabs='imps',lvl=1,invno='',invdt='',invval=0,pos='',rate=0,taxval=0,igstpaid=0,cesspaid=0,itceligible='',igstavail=0,cessavail=0,entry_ty='',tran_cd=0
End

--impg
print 'impg'

Insert Into #GSTR2EXCEL(tabs,lvl,portcode,boeno,boedt,boeval,doctype,gstin,rate,taxval,igstpaid,cesspaid,itceligible,igstavail,cessavail,entry_ty,tran_cd)
select tabs,lvl,portcode,boeno,boedt,boeval,doctype,gstin,rate,taxval,igstpaid,cesspaid,itceligible,igstavail,cessavail,entry_ty,tran_cd from (
Select tabs='impg',lvl=1,portcode=isnull(portcode,''),boeno=isnull(beno,'')
,boedt=(cast(day(bedt) as varchar)+'-'+cast(left(datename(mm,bedt),3) as varchar)+'-'+cast(right(year(bedt),2) as varchar))
,boeval=Net_amt
,doctype=(case when supp_type='SEZ' then 'Received from SEZ' else 'Imports' end)
,gstin,rate=rate1,taxval=SUM(TaxableAmt)
,igstpaid=sum(igst_amt),cesspaid=sum(cess_amt)
,itceligible
,igstavail=SUM(iigst_amt)
,cessavail=SUM(ICESS_AMT)
,entry_ty,tran_cd
FROM #GSTR2TBL 
where mENTRY IN ('EP','PT')
AND ST_TYPE IN ('INTERSTATE','INTRASTATE','OUT OF COUNTRY') 
--and SUPP_TYPE in ('IMPORT','EOU','EXPORT','SEZ')  --Commented by Priyanka B on 26082020 for Bug-33493
and SUPP_TYPE in ('IMPORT','EOU','EXPORT','SEZ','')  --Modified by Priyanka B on 26082020 for Bug-33493
AND ISSERVICE='GOODS' 
GROUP BY Rate1,gstin,beno,bedt,portcode,itceligible,supp_type,entry_ty,tran_cd,Net_amt
)impg order by boeno,boedt,rate

If not exists(select * from #GSTR2EXCEL where tabs='impg' and lvl=1)
Begin
	Insert Into #GSTR2EXCEL(tabs,lvl,portcode,boeno,boedt,boeval,doctype,gstin,rate,taxval,igstpaid,cesspaid,itceligible,igstavail,cessavail,entry_ty,tran_cd)
	SELECT tabs='impg',lvl=1,portcode='',boeno='',boedt='',boeval=0,doctype='',gstin='',rate=0,taxval=0,igstpaid=0,cesspaid=0,itceligible='',igstavail=0,cessavail=0,entry_ty='',tran_cd=0
End

--cdnr
print 'cdnr'

Insert Into #GSTR2EXCEL(tabs,lvl,gstin,nrvno,nrvdt,invno,invdt,pregst,cddoctype,reason,suptype,nrvval,rate,taxval,igstpaid,cgstpaid,sgstpaid,cesspaid,itceligible,igstavail,cgstavail,sgstavail,cessavail,entry_ty,tran_cd)
select tabs,lvl,gstin,nrvno,nrvdt,invno,invdt,pregst,cddoctype,reason,suptype
--,sum(nrvval)   --Commented by Priyanka B on 26082020 for Bug-33493
,nrvval   --Modified by Priyanka B on 26082020 for Bug-33493
,rate,sum(taxval),sum(igstpaid),sum(cgstpaid),sum(sgstpaid),sum(cesspaid),itceligible,sum(igstavail),sum(cgstavail),sum(sgstavail),sum(cessavail),entry_ty,tran_cd from (
Select tabs='cdnr',lvl=1,gstin
,nrvno=rtrim(ltrim(ORG_INVNO))
,nrvdt=(cast(day(ORG_DATE) as varchar)+'-'+cast(left(datename(mm,ORG_DATE),3) as varchar)+'-'+cast(right(year(ORG_DATE),2) as varchar))
,invno=rtrim(ltrim(pinvno))
,invdt=(cast(day(pinvdt) as varchar)+'-'+cast(left(datename(mm,pinvdt),3) as varchar)+'-'+cast(right(year(pinvdt),2) as varchar))
,pregst=(case when pregst=0 then 'N' else 'Y' end)
,cddoctype,reason
,suptype=(case when st_type='INTRASTATE' then 'Intra State' else 
		(case when st_type='INTERSTATE' then 'Inter State' else st_type end) end)
,nrvval=Net_amt,rate=rate1,taxval=taxableamt
,igstpaid=(case when IGST_AMT>0 then IGST_AMT else IGSRT_AMT end)
,cgstpaid=(case when CGST_AMT>0 then CGST_AMT else CGSRT_AMT end)
,sgstpaid=(case when SGST_AMT>0 then SGST_AMT else SGSRT_AMT end)
,cesspaid=(case when CESS_AMT>0 then CESS_AMT else CESSRT_AMT end)
,itceligible
,igstavail=isnull(iigst_amt,0)
,cgstavail=isnull(icgst_amt,0)
,sgstavail=isnull(isgst_amt,0)
,cessavail=isnull(icess_amt,0)
,entry_ty,tran_cd
FROM #GSTR2TBL
where mENTRY IN ('CN','DN','PR')
and SUPP_TYPE in('IMPORT','EOU','EXPORT','')
union all
Select tabs='cdnr',lvl=1,gstin
,nrvno=rtrim(ltrim(ORG_INVNO))
,nrvdt=(cast(day(ORG_DATE) as varchar)+'-'+cast(left(datename(mm,ORG_DATE),3) as varchar)+'-'+cast(right(year(ORG_DATE),2) as varchar))
,invno=rtrim(ltrim(pinvno))
,invdt=(cast(day(pinvdt) as varchar)+'-'+cast(left(datename(mm,pinvdt),3) as varchar)+'-'+cast(right(year(pinvdt),2) as varchar))
,pregst=(case when pregst=0 then 'N' else 'Y' end)
,cddoctype,reason
,suptype=(case when st_type='INTRASTATE' then 'Intra State' else 
		(case when st_type='INTERSTATE' then 'Inter State' else st_type end) end)
,nrvval=Net_amt,rate=rate1,taxval=taxableamt
,igstpaid=(case when IGST_AMT>0 then IGST_AMT else IGSRT_AMT end)
,cgstpaid=(case when CGST_AMT>0 then CGST_AMT else CGSRT_AMT end)
,sgstpaid=(case when SGST_AMT>0 then SGST_AMT else SGSRT_AMT end)
,cesspaid=(case when CESS_AMT>0 then CESS_AMT else CESSRT_AMT end)
,itceligible
,igstavail=isnull(iigst_amt,0)
,cgstavail=isnull(icgst_amt,0)
,sgstavail=isnull(isgst_amt,0)
,cessavail=isnull(icess_amt,0)
,entry_ty,tran_cd
FROM #GSTR2TBL
where mENTRY IN ('CN','DN','PR')
and SUPP_TYPE not in('Compounding','IMPORT','EOU','EXPORT','') and LineRule in('Taxable') and HSNCODE <> '' 
)cdnr
GROUP BY tabs,lvl,gstin,nrvno,nrvdt,invno,invdt,pregst,cddoctype,reason,suptype,rate,itceligible,entry_ty,tran_cd
,nrvval   --Added by Priyanka B on 26082020 for Bug-33493
ORDER BY nrvno,nrvdt,rate

If not exists(select * from #GSTR2EXCEL where tabs='cdnr' and lvl=1)
Begin
	Insert Into #GSTR2EXCEL(tabs,lvl,gstin,nrvno,nrvdt,invno,invdt,pregst,cddoctype,reason,suptype,nrvval,rate,taxval,igstpaid,cgstpaid,sgstpaid,cesspaid,itceligible,igstavail,cgstavail,sgstavail,cessavail,entry_ty,tran_cd)
	SELECT tabs='cdnr',lvl=1,gstin='',nrvno='',nrvdt='',invno='',invdt='',pregst='',cddoctype='',reason='',suptype='',nrvval=0,rate=0,taxval=0,igstpaid=0,cgstpaid=0,sgstpaid=0,cesspaid=0,itceligible='',igstavail=0,cgstavail=0,sgstavail=0,cessavail=0,entry_ty='',tran_cd=0
End

--cdnur
print 'cdnur'

Insert Into #GSTR2EXCEL(tabs,lvl,nrvno,nrvdt,invno,invdt,pregst,cddoctype,reason,suptype,invtype,nrvval,rate,taxval,igstpaid,cgstpaid,sgstpaid,cesspaid,itceligible,igstavail,cgstavail,sgstavail,cessavail,entry_ty,tran_cd)
select tabs,lvl,nrvno,nrvdt,invno,invdt,pregst,cddoctype,reason,suptype,invtype
--,sum(nrvval)  --Commented by Priyanka B on 26082020 for Bug-33493
,nrvval   --Modified by Priyanka B on 26082020 for Bug-33493
,rate,sum(taxval),sum(igstpaid),sum(cgstpaid),sum(sgstpaid),sum(cesspaid),itceligible,sum(igstavail),sum(cgstavail),sum(sgstavail),sum(cessavail),entry_ty,tran_cd from (
Select tabs='cdnur',lvl=1
,nrvno=rtrim(ltrim(ORG_INVNO))
,nrvdt=(cast(day(ORG_DATE) as varchar)+'-'+cast(left(datename(mm,ORG_DATE),3) as varchar)+'-'+cast(right(year(ORG_DATE),2) as varchar))
,invno=rtrim(ltrim(pinvno))
,invdt=(cast(day(pinvdt) as varchar)+'-'+cast(left(datename(mm,pinvdt),3) as varchar)+'-'+cast(right(year(pinvdt),2) as varchar))
,pregst=(case when pregst=0 then 'N' else 'Y' end)
,cddoctype,reason
,suptype=(case when st_type='INTRASTATE' then 'Intra State' else 
		(case when st_type='INTERSTATE' then 'Inter State' else st_type end) end)
,invtype=(case when supp_type='Import' then 'IMPS' else 'B2BUR' end)
,nrvval=Net_amt,rate=rate1,taxval=taxableamt
,igstpaid=(case when IGST_AMT>0 then IGST_AMT else IGSRT_AMT end)
,cgstpaid=(case when CGST_AMT>0 then CGST_AMT else CGSRT_AMT end)
,sgstpaid=(case when SGST_AMT>0 then SGST_AMT else SGSRT_AMT end)
,cesspaid=(case when CESS_AMT>0 then CESS_AMT else CESSRT_AMT end)
,itceligible
,igstavail=isnull(iigst_amt,0)
,cgstavail=isnull(icgst_amt,0)
,sgstavail=isnull(isgst_amt,0)
,cessavail=isnull(icess_amt,0)
,entry_ty,tran_cd
FROM #GSTR2TBL
where mENTRY IN ('CN','DN','PR')
and SUPP_TYPE in('UNREGISTERED','IMPORT')
union all
Select tabs='cdnur',lvl=1
,nrvno=rtrim(ltrim(ORG_INVNO))
,nrvdt=(cast(day(ORG_DATE) as varchar)+'-'+cast(left(datename(mm,ORG_DATE),3) as varchar)+'-'+cast(right(year(ORG_DATE),2) as varchar))
,invno=rtrim(ltrim(pinvno))
,invdt=(cast(day(pinvdt) as varchar)+'-'+cast(left(datename(mm,pinvdt),3) as varchar)+'-'+cast(right(year(pinvdt),2) as varchar))
,pregst=(case when pregst=0 then 'N' else 'Y' end)
,cddoctype,reason
,suptype=(case when st_type='INTRASTATE' then 'Intra State' else 
		(case when st_type='INTERSTATE' then 'Inter State' else st_type end) end)
,invtype=(case when supp_type='Import' then 'IMPS' else 'B2BUR' end)
,nrvval=Net_amt,rate=rate1,taxval=taxableamt
,igstpaid=(case when IGST_AMT>0 then IGST_AMT else IGSRT_AMT end)
,cgstpaid=(case when CGST_AMT>0 then CGST_AMT else CGSRT_AMT end)
,sgstpaid=(case when SGST_AMT>0 then SGST_AMT else SGSRT_AMT end)
,cesspaid=(case when CESS_AMT>0 then CESS_AMT else CESSRT_AMT end)
,itceligible
,igstavail=isnull(iigst_amt,0)
,cgstavail=isnull(icgst_amt,0)
,sgstavail=isnull(isgst_amt,0)
,cessavail=isnull(icess_amt,0)
,entry_ty,tran_cd
FROM #GSTR2TBL
where mENTRY IN ('CN','DN','PR')
and SUPP_TYPE in('UNREGISTERED','IMPORT') and LineRule in('Taxable') and HSNCODE <> '' 
)cdnur
GROUP BY tabs,lvl,nrvno,nrvdt,invno,invdt,pregst,cddoctype,reason,suptype,rate,itceligible,entry_ty,tran_cd,invtype
,nrvval   --Added by Priyanka B on 26082020 for Bug-33493
ORDER BY nrvno,nrvdt,rate

If not exists(select * from #GSTR2EXCEL where tabs='cdnur' and lvl=1)
Begin
	Insert Into #GSTR2EXCEL(tabs,lvl,nrvno,nrvdt,invno,invdt,pregst,cddoctype,reason,suptype,invtype,nrvval,rate,taxval,igstpaid,cgstpaid,sgstpaid,cesspaid,itceligible,igstavail,cgstavail,sgstavail,cessavail,entry_ty,tran_cd)
	SELECT tabs='cdnur',lvl=1,nrvno='',nrvdt='',invno='',invdt='',pregst='',cddoctype='',reason='',suptype='',invtype='',nrvval=0,rate=0,taxval=0,igstpaid=0,cgstpaid=0,sgstpaid=0,cesspaid=0,itceligible='',igstavail=0,cgstavail=0,sgstavail=0,cessavail=0,entry_ty='',tran_cd=0
End

--at
print 'at'

/* Taxallocation details */
SELECT DISTINCT TRAN_CD,ENTRY_TY,POS,pos_std_cd,DATE,st_type,inv_no,cons_ac_name,gstin INTO #BANK_TMP FROM #GSTR2TBL
WHERE mEntry in('PT','CP','EP','BP')

select A.*,B.POS,B.POS_STD_CD,C.DATE,C.st_type,C.INV_NO,C.cons_ac_name,C.gstin INTO #TaxAlloc_tmp from TaxAllocation A INNER JOIN #BANK_TMP B ON (A.Itref_tran =B.TRAN_CD AND A.REntry_ty =B.Entry_ty)
INNER JOIN #BANK_TMP C ON (A.Entry_ty =C.Entry_ty AND A.Tran_cd =C.Tran_cd) WHERE C.DATE BETWEEN @SDATE AND @EDATE

SELECT * INTO #Tax_tmp FROM 
(select PKEY = '+', rate1,pos ,Taxableamt,CGSRT_AMT,SGSRT_AMT,IGSRT_AMT,CessRT_amt,pos_std_cd,st_type,inv_no,date,entry_ty,tran_cd,cons_ac_name,gstin 
from #GSTR2TBL 
WHERE mEntry in('BP','CP') and (CGSRT_AMT + SGSRT_AMT+IGSRT_AMT) > 0
union all 
SELECT PKEY = '-',TaxRate,POS,Taxable,CGST_AMT,SGST_AMT,IGST_AMT,COMPCESS,pos_std_cd,st_type,inv_no,date,entry_ty,tran_cd,cons_ac_name,gstin 
FROM #TaxAlloc_tmp )AA

--level=1
print 'at - Level 1'

Insert Into #GSTR2EXCEL(tabs,lvl,pos,rate,suptype,grossadv,cessamt)
select tabs,lvl,pos,rate,suptype,grossadv,cessamt from (
Select tabs='at',lvl=1,pos=pos_std_cd+'-'+pos
,rate=rate1
,suptype=(case when st_type='INTRASTATE' then 'Intra State' else 
		(case when st_type='INTERSTATE' then 'Inter State' else st_type end) end)
,grossadv = sum(CASE WHEN PKEY = '+' THEN +Taxableamt ELSE -Taxableamt END)
,cessamt = sum(CASE WHEN PKEY = '+' THEN +CessRT_amt ELSE -CessRT_amt END)
from #Tax_tmp 
--where (CGSRT_AMT + SGSRT_AMT) > 0  --Commented by Priyanka B on 28082020 for AU 2.2.5
where (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) > 0  --Modified by Priyanka B on 28082020 for AU 2.2.5
group by pos,pos_std_cd,st_type,rate1
)at  
ORDER BY pos

If not exists(select * from #GSTR2EXCEL where tabs='at' and lvl=1)
Begin
	Insert Into #GSTR2EXCEL(tabs,lvl,pos,suptype,grossadv,cessamt)
	SELECT tabs='at',lvl=1,pos='',suptype='',grossadv=0,cessamt=0
End

--level=2
print 'at - Level 2'

Insert Into #GSTR2EXCEL(tabs,lvl,pos,rate,suptype,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,supname,gstin)
select tabs,lvl,pos,rate,suptype,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,supname,gstin from (
select tabs='at',lvl=2,pos=pos_std_cd+'-'+pos
,rate=rate1
,suptype=(case when st_type='INTRASTATE' then 'Intra State' else 
		(case when st_type='INTERSTATE' then 'Inter State' else st_type end) end)
,grossadv = sum(CASE WHEN PKEY = '+' THEN +Taxableamt ELSE -Taxableamt END)
,cessamt = sum(CASE WHEN PKEY = '+' THEN +CessRT_amt ELSE -CessRT_amt END)
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,entry_ty,tran_cd,supname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #Tax_tmp 
--where (CGSRT_AMT + SGSRT_AMT) > 0  --Commented by Priyanka B on 28082020 for AU 2.2.5
where (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) > 0  --Modified by Priyanka B on 28082020 for AU 2.2.5
group by pos,pos_std_cd,st_type,entry_ty,tran_cd,inv_no,date,cons_ac_name,gstin,rate1
)at

If not exists(select * from #GSTR2EXCEL where tabs='at' and lvl=2)
Begin
	Insert Into #GSTR2EXCEL(tabs,lvl,pos,rate,suptype,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,supname,gstin)
	SELECT tabs='at',lvl=2,pos='',rate=0,suptype='',grossadv=0,cessamt=0,invno='',invdt='',entry_ty='',tran_cd=0,supname='',gstin=''
End

--atadj
print 'atadj'

SELECT A.*,b.pos,b.pos_std_cd ,b.date,b.st_type,b.INV_NO,b.cons_ac_name,b.gstin
into #TaxTemp2 FROM TaxAllocation A inner join  #BANK_TMP b ON (A.Entry_ty =B.Entry_ty AND A.Tran_cd =b.Tran_cd)  where (a.REntry_ty+QUOTENAME(a.Itref_tran) not IN(select (Entry_ty + quotename(Tran_cd))  from #BANK_TMP where Entry_ty in('cp','bp') and DATE between @SDATE and @EDATE)) and (b.date between @sdate and @edate) and a.REntry_ty  in('BP','CP')

--select * from #TaxTemp2

--level=1
print 'atadj - Level 1'

Insert Into #GSTR2EXCEL(tabs,lvl,pos,rate,suptype,grossadv,cessamt)
select tabs,lvl,pos,rate,suptype,grossadv,cessamt from (
select tabs='atadj',lvl=1,pos=pos_std_cd+'-'+pos
,rate=taxrate
,suptype=(case when st_type='INTRASTATE' then 'Intra State' else 
		(case when st_type='INTERSTATE' then 'Inter State' else st_type end) end)
,grossadv = sum(taxable),cessamt = sum(COMPCESS)
from #TaxTemp2
where (isnull(SGST_AMT,0) + isnull(CGST_AMT,0) + isnull(IGST_AMT,0)) > 0  
group by POS,pos_std_cd,st_type,taxrate
)atadj

If not exists(select * from #GSTR2EXCEL where tabs='atadj' and lvl=1)
Begin
	Insert Into #GSTR2EXCEL(tabs,lvl,pos,rate,suptype,grossadv,cessamt)
	SELECT tabs='atadj',lvl=1,pos='',rate=0,suptype='',grossadv=0,cessamt=0
End

--level=2
print 'atadj - Level 2'

Insert Into #GSTR2EXCEL(tabs,lvl,pos,rate,suptype,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,supname,gstin)
select tabs,lvl,pos,rate,suptype,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,supname,gstin from (
select tabs='atadj',lvl=2,pos=pos_std_cd+'-'+pos
,rate=taxrate
,suptype=(case when st_type='INTRASTATE' then 'Intra State' else 
		(case when st_type='INTERSTATE' then 'Inter State' else st_type end) end)
,grossadv = sum(taxable),cessamt = sum(COMPCESS)
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,entry_ty,tran_cd
,supname=substring(rtrim(ltrim(cons_ac_name)),1,15),gstin
from #TaxTemp2 
where (isnull(SGST_AMT,0) + isnull(CGST_AMT,0) + isnull(IGST_AMT,0)) > 0  
group by POS,pos_std_cd,st_type,taxrate,entry_ty,tran_cd,inv_no,date,cons_ac_name,gstin
)atadj

If not exists(select * from #GSTR2EXCEL where tabs='atadj' and lvl=2)
Begin
	Insert Into #GSTR2EXCEL(tabs,lvl,pos,rate,suptype,grossadv,cessamt,invno,invdt,entry_ty,tran_cd,supname,gstin)
	SELECT tabs='atadj',lvl=2,pos='',rate=0,suptype=0,grossadv=0,cessamt=0,invno='',invdt='',entry_ty='',tran_cd=0,supname='',gstin=''
End

--exemp
print 'exemp'

select gstin,Entry_ty,Supp_type ,St_type ,linerule
,CompAmt =isnull((case when supp_type='Compounding' then (CASE WHEN mEntry in('PT','EP','CN') THEN +GRO_AMT ELSE -GRO_AMT END) else 0.00 end),0)
,NilAmt =isnull((case when supp_type <> 'Compounding' and LINERULE in('Nill Rated','Nil Rated') AND hsncode <> '' then (CASE WHEN mEntry in('PT','EP','CN') THEN +GRO_AMT ELSE -GRO_AMT END) else 0.00 end),0)
,ExemAmt =isnull((case when supp_type <> 'Compounding' and LINERULE ='Exempted' AND hsncode <> '' then (CASE WHEN mEntry in('PT','EP','CN') THEN +GRO_AMT ELSE -GRO_AMT END) else 0.00 end),0)
,NonGstAmt =isnull((case when supp_type <> 'Compounding'  AND (hsncode = '' or LINERULE ='Non GST') and LINERULE NOT IN('Nill Rated','Nil Rated','Exempted') then (CASE WHEN mEntry in('PT','EP','CN') THEN +GRO_AMT ELSE -GRO_AMT END) else 0.00 end),0)
,mentry
into #Gstr2Tbl1
from #GSTR2TBL
where st_type in('INTERSTATE','INTRASTATE') 
AND mEntry in('PT','EP','CN','DN','PR') AND Supp_type NOT IN('SEZ','EOU','IMPORT')

--select * from #Gstr2Tbl1

---Inter-State supplies
set @AMT1 = 0.00 
set @AMT2 = 0.00
set @AMT3 = 0.00
set @AMT4 = 0.00

SELECT 
@AMT1 =sum((case when mEntry in('PT','EP','CN') THEN +(CompAmt) ELSE - (CompAmt) END )) ,
@AMT2 =sum((case when mEntry in('PT','EP','CN') THEN +(NilAmt) ELSE - (NilAmt) END )),
@AMT3 =sum((case when mEntry in('PT','EP','CN') THEN +(ExemAmt) ELSE - (ExemAmt) END )) ,
@AMT4 =sum((case when mEntry in('PT','EP','CN') THEN +(NonGstAmt) ELSE - (NonGstAmt) END )) 
FROM #Gstr2Tbl1 
WHERE st_type = 'Interstate'

Insert Into #GSTR2EXCEL(tabs,descr,comptax,nil,exempt,nongst)
select tabs,descr,comptax,nil,exempt,nongst from (
select tabs='exemp',descr='Inter-State supplies',comptax=isnull(@AMT1,0),nil=isnull(@AMT2,0),exempt=isnull(@AMT3,0),nongst=isnull(@AMT4,0)
)exemp

---Intra-State supplies
set @AMT1 = 0.00 
set @AMT2 = 0.00
set @AMT3 = 0.00
set @AMT4 = 0.00

SELECT 
@AMT1 =sum((case when mEntry in('PT','EP','CN') THEN +(CompAmt) ELSE - (CompAmt) END )) ,
@AMT2 =sum((case when mEntry in('PT','EP','CN') THEN +(NilAmt) ELSE - (NilAmt) END )),
@AMT3 =sum((case when mEntry in('PT','EP','CN') THEN +(ExemAmt) ELSE - (ExemAmt) END )) ,
@AMT4 =sum((case when mEntry in('PT','EP','CN') THEN +(NonGstAmt) ELSE - (NonGstAmt) END )) 
FROM #Gstr2Tbl1 
WHERE st_type = 'Intrastate'

Insert Into #GSTR2EXCEL(tabs,descr,comptax,nil,exempt,nongst)
select tabs,descr,comptax,nil,exempt,nongst from (
select tabs='exemp',descr='Intra-State supplies',comptax=isnull(@AMT1,0),nil=isnull(@AMT2,0),exempt=isnull(@AMT3,0),nongst=isnull(@AMT4,0)
)exemp

--itcr
SELECT * into #jvmain FROM (select tran_cd,ENTRY_TY,AGAINSTTY,RRGST,RevsType,DATE,Amenddate,CGST_AMT,SGST_AMT,IGST_AMT,COMPCESS from JVMAIN  WHERE entry_ty = 'J7'
UNION ALL 
select tran_cd,ENTRY_TY,AGAINSTTY,RRGST,RevsType,DATE,'' as Amenddate,CGST_AMT,sGST_AMT,IGST_AMT,COMPCESS from JVMAINAM  WHERE entry_ty = 'J7' )ad

Insert Into #GSTR2EXCEL(tabs,lvl,descr,addred,igstamt,cgstamt,sgstamt,cessamt)
select tabs,lvl,descr,addred,igstamt,cgstamt,sgstamt,cessamt from (
select tabs='itcr',lvl=1,descr='(a) Amount in terms of rule 37 (2)',addred='To be added'
,igstamt= isnull(sum(IGST_AMT),0),cgstamt =isnull(sum(CGST_AMT),0),sgstamt=isnull(sum(SGST_AMT),0),cessamt =isnull(sum(COMPCESS),0) 
from #jvmain
where entry_ty = 'j7' and (date between @SDATE and @EDATE ) and RevsType = 'Amount in terms of rule 37(2)'  and isnull(amenddate,'')='' 
and AGAINSTTY = 'Addition' and RRGST = 'Input Tax'
Union all
select tabs='itcr',lvl=1,descr='(b) Amount in terms of rule 42 (1) (m)',addred='To be added'
,igstamt= isnull(sum(IGST_AMT),0),cgstamt =isnull(sum(CGST_AMT),0),sgstamt=isnull(sum(SGST_AMT),0),cessamt =isnull(sum(COMPCESS),0) 
from #jvmain
where entry_ty = 'j7' and (date between @SDATE and @EDATE ) and RevsType = 'Amount in terms of rule 42 (1) (m)'  and isnull(amenddate,'')='' 
and AGAINSTTY = 'Addition' and RRGST = 'Input Tax'  
Union all
select tabs='itcr',lvl=1,descr='(c) Amount in terms of rule 43(1) (h)',addred='To be added'
,igstamt= isnull(sum(IGST_AMT),0),cgstamt =isnull(sum(CGST_AMT),0),sgstamt=isnull(sum(SGST_AMT),0),cessamt =isnull(sum(COMPCESS),0) 
from #jvmain
where entry_ty = 'j7' and ( date between @SDATE and @EDATE ) and RevsType = 'Amount in terms of rule 43(1) (h) '  and isnull(amenddate,'')='' 
and AGAINSTTY = 'Addition' and RRGST = 'Input Tax'
Union all
select tabs='itcr',lvl=1,descr='(d) Amount in terms of rule 42 (2)(a)',addred='To be added'
,igstamt= isnull(sum(IGST_AMT),0),cgstamt =isnull(sum(CGST_AMT),0),sgstamt=isnull(sum(SGST_AMT),0),cessamt =isnull(sum(COMPCESS),0) 
from #jvmain
where entry_ty = 'j7' and ( date between @SDATE and @EDATE ) and RevsType = 'Amount in terms of rule 42 (2)(a)'  and isnull(amenddate,'')='' 
and AGAINSTTY = 'Addition' and RRGST = 'Input Tax'
Union all
select tabs='itcr',lvl=1,descr='(e) Amount in terms of rule 42(2)(b)',addred='To be reduced'
,igstamt= isnull(sum(IGST_AMT),0),cgstamt =isnull(sum(CGST_AMT),0),sgstamt=isnull(sum(SGST_AMT),0),cessamt =isnull(sum(COMPCESS),0) 
from #jvmain
where entry_ty = 'j7' and ( date between @SDATE and @EDATE ) and RevsType = 'Amount in terms of rule 42(2)(b)'  and isnull(amenddate,'')='' 
and AGAINSTTY = 'Reduction' and RRGST = 'Input Tax'
Union all
select tabs='itcr',lvl=1,descr='(f) On account of amount paid subsequent to reversal of ITC',addred='To be reduced'
,igstamt= isnull(sum(IGST_AMT),0),cgstamt =isnull(sum(CGST_AMT),0),sgstamt=isnull(sum(SGST_AMT),0),cessamt =isnull(sum(COMPCESS),0) 
from #jvmain
where entry_ty = 'j7' and ( date between @SDATE and @EDATE ) and RevsType = 'On account of amount paid subsequent to reversal of ITC'  and isnull(amenddate,'')='' 
and AGAINSTTY = 'Reduction' and RRGST = 'Input Tax'
Union all
select tabs='itcr',lvl=1,descr='(g) Any other liability (Specify)',addred=(case when AGAINSTTY = 'Reduction' then 'To be reduced' else 'To be added' end) 
,igstamt= isnull(sum(IGST_AMT),0),cgstamt =isnull(sum(CGST_AMT),0),sgstamt=isnull(sum(SGST_AMT),0),cessamt =isnull(sum(COMPCESS),0) 
from #jvmain
where entry_ty = 'j7' and ( date between @SDATE and @EDATE ) and RevsType = 'Any other liability (Specify)' and isnull(amenddate,'') = '' 
and RRGST = 'Input Tax'
group by AGAINSTTY
)itcr

If not exists(select * from #GSTR2EXCEL where tabs='itcr' and lvl=1 and descr='(g) Any other liability (Specify)')
Begin
	Insert Into #GSTR2EXCEL(tabs,lvl,descr,addred,igstamt,cgstamt,sgstamt,cessamt)
	SELECT tabs='itcr',lvl=1,descr='(g) Any other liability (Specify)',addred='',igstamt=0,cgstamt=0,sgstamt=0,cessamt=0
End

--hsnsum
print 'hsnsum'

select A.*,isnull(B.SERVTCODE,'') as SERVTCODE,B.SERTY
--,C.gstr_desc 
,gstr_desc=(CASE WHEN B.UOM_DESC<>'' THEN B.UOM_DESC ELSE C.U_UOM END)
INTO #GSTRT2TBL_HSN  
from #GSTR2TBL A INNER JOIN IT_MAST B  ON (A.IT_CODE=B.IT_CODE)
Inner Join UOM C on (C.u_uom=B.rateunit)
where (A.SUPP_TYPE <> 'unregistered' 
AND A.mEntry In('PT','PR','CN','DN','EP')) OR A.Entry_ty = 'UB'
UPDATE #GSTRT2TBL_HSN SET SERVTCODE=ISNULL((select top 1 code from sertax_mast  where serty = #GSTRT2TBL_HSN.Serty),'')  where isnull(#GSTRT2TBL_HSN.SERVTCODE,'') = '' AND Isservice = 'services'


print 'hsnsum level 1'
Insert Into #GSTR2EXCEL(tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt)
select tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt from (
select tabs='hsnsum',lvl=1
,hsn=(case when Isservice = 'services' then SERVTCODE else HSNCODE end )
,descr=substring([dbo].[ReplaceASCII](hsn_desc),1,30)
,uqc=gstr_desc
,totqty=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +QTY ELSE -QTY END)
,totval=sum(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +GRO_AMT ELSE -GRO_AMT END)
,taxval=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +Taxableamt ELSE -Taxableamt END)
,igstamt=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +(ISNULL(IGST_AMT,0)+ISNULL(IGSRT_AMT,0)) ELSE -(ISNULL(IGST_AMT,0)+ISNULL(IGSRT_AMT,0)) END)
,cgstamt=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +(ISNULL(CGST_AMT,0)+ISNULL(CGSRT_AMT,0)) ELSE -(ISNULL(CGST_AMT,0)+ISNULL(CGSRT_AMT,0)) END)
,sgstamt=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +(ISNULL(SGST_AMT,0)+ISNULL(SGSRT_AMT,0)) ELSE -(ISNULL(SGST_AMT,0)+ISNULL(SGSRT_AMT,0)) END)
,cessamt=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +(ISNULL(CESS_AMT,0)+ISNULL(CessRT_amt,0)) ELSE -(ISNULL(CESS_AMT,0)+ISNULL(CessRT_amt,0)) END)
FROM #GSTRT2TBL_HSN
WHERE (mEntry IN('PT','PR','CN','DN','EP') or Entry_ty='UB')
and ltrim(rtrim((case when Isservice = 'services' then SERVTCODE else HSNCODE end ))) <>'' 
group by HSNCODE,SERVTCODE,Isservice,gstr_desc,HSN_DESC
)hsn order by hsn,uqc

print 'hsnsum level 2'
--Level=2
Insert Into #GSTR2EXCEL(tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt,it_name)
select tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt,it_name from (
select tabs='hsnsum',lvl=2
,hsn=(case when Isservice = 'services' then SERVTCODE else HSNCODE end )
,descr=substring([dbo].[ReplaceASCII](hsn_desc),1,30)
,uqc=gstr_desc
,totqty=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +QTY ELSE -QTY END)
,totval=sum(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +GRO_AMT ELSE -GRO_AMT END)
,taxval=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +Taxableamt ELSE -Taxableamt END)
,igstamt=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +(ISNULL(IGST_AMT,0)+ISNULL(IGSRT_AMT,0)) ELSE -(ISNULL(IGST_AMT,0)+ISNULL(IGSRT_AMT,0)) END)
,cgstamt=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +(ISNULL(CGST_AMT,0)+ISNULL(CGSRT_AMT,0)) ELSE -(ISNULL(CGST_AMT,0)+ISNULL(CGSRT_AMT,0)) END)
,sgstamt=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +(ISNULL(SGST_AMT,0)+ISNULL(SGSRT_AMT,0)) ELSE -(ISNULL(SGST_AMT,0)+ISNULL(SGSRT_AMT,0)) END)
,cessamt=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +(ISNULL(CESS_AMT,0)+ISNULL(CessRT_amt,0)) ELSE -(ISNULL(CESS_AMT,0)+ISNULL(CessRT_amt,0)) END)
,it_name
FROM #GSTRT2TBL_HSN
WHERE (mEntry IN('PT','PR','CN','DN','EP') or Entry_ty='UB')
and ltrim(rtrim((case when Isservice = 'services' then SERVTCODE else HSNCODE end ))) <>'' 
group by HSNCODE,gstr_desc,HSN_DESC,it_name,SERVTCODE,Isservice
)hsn order by hsn,uqc

print 'hsnsum level 3'
--Level=3
Insert Into #GSTR2EXCEL(tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt,invno,invdt,entry_ty,tran_cd,it_name)
select tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt,invno,invdt,entry_ty,tran_cd,it_name from (
select tabs='hsnsum',lvl=3
,hsn=(case when Isservice = 'services' then SERVTCODE else HSNCODE end )
,descr=substring([dbo].[ReplaceASCII](hsn_desc),1,30)
,uqc=gstr_desc
,totqty=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +QTY ELSE -QTY END)
,totval=sum(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +GRO_AMT ELSE -GRO_AMT END)
,taxval=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +Taxableamt ELSE -Taxableamt END)
,igstamt=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +(ISNULL(IGST_AMT,0)+ISNULL(IGSRT_AMT,0)) ELSE -(ISNULL(IGST_AMT,0)+ISNULL(IGSRT_AMT,0)) END)
,cgstamt=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +(ISNULL(CGST_AMT,0)+ISNULL(CGSRT_AMT,0)) ELSE -(ISNULL(CGST_AMT,0)+ISNULL(CGSRT_AMT,0)) END)
,sgstamt=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +(ISNULL(SGST_AMT,0)+ISNULL(SGSRT_AMT,0)) ELSE -(ISNULL(SGST_AMT,0)+ISNULL(SGSRT_AMT,0)) END)
,cessamt=SUM(CASE WHEN (mENTRY IN('PT','CN','EP') or Entry_ty='UB') THEN +(ISNULL(CESS_AMT,0)+ISNULL(CessRT_amt,0)) ELSE -(ISNULL(CESS_AMT,0)+ISNULL(CessRT_amt,0)) END)
,invno=rtrim(ltrim(inv_no))
,invdt=(cast(day(date) as varchar)+'-'+cast(left(datename(mm,date),3) as varchar)+'-'+cast(right(year(date),2) as varchar))
,entry_ty,tran_cd
,it_name
FROM #GSTRT2TBL_HSN
WHERE (mEntry IN('PT','PR','CN','DN','EP') or Entry_ty='UB')
and ltrim(rtrim((case when Isservice = 'services' then SERVTCODE else HSNCODE end ))) <>'' 
group by HSNCODE,gstr_desc,HSN_DESC,inv_no,date,entry_ty,tran_cd,it_name,SERVTCODE,Isservice
)hsn order by hsn,uqc,invno,invdt,entry_ty,tran_cd

If not exists(select * from #GSTR2EXCEL where tabs='hsnsum' and lvl=1)
Begin
	Insert Into #GSTR2EXCEL(tabs,lvl,hsn,descr,uqc,totqty,totval,taxval,igstamt,cgstamt,sgstamt,cessamt)
	SELECT tabs='hsnsum',lvl=1,hsn='',descr='',uqc='',totqty=0,totval=0,taxval=0,igstamt=0,cgstamt=0,sgstamt=0,cessamt=0
End

Update #GSTR2EXCEL set invdt=case when invdt='1-Jan-00' then '' else invdt end
,nrvdt=case when nrvdt='1-Jan-00' then '' else nrvdt end


--SELECT RANK() OVER (PARTITION BY tabs ORDER BY tranid) AS RANK

/*Update a set a.SerialNo=isnull(a.SerialNo,0)+1 
from #GSTR1EXCEL a, #GSTR1EXCEL b 
where a.tabs=b.tabs
*/

select RANK() OVER (PARTITION BY tabs,lvl ORDER BY tranid) AS SerialNo
,* from #GSTR2EXCEL
--where tabs='b2b'
order by tranid

--select distinct tabs,'case "'+tabs+'":'+char(13)+'break;',tranid from #GSTR1EXCEL
--where lvl=2
------group by tabs,tranid
--order by tranid

END
go
--set dateformat dmy EXECUTE Usp_Rep_GSTR2_EXCEL'','','','01/04/2019 ','31/03/2020','','','','',0,0,'','','','','','','','','2020-2021',''
