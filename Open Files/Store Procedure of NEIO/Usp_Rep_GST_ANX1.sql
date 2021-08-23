If Exists(Select [Name] From SysObjects Where xType='P' and [Name]='Usp_Rep_GST_ANX1')
Begin
	Drop procedure Usp_Rep_GST_ANX1
End
Go
Create Procedure [dbo].[Usp_Rep_GST_ANX1]
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
	,@XtraPara AS VARCHAR(60)
	,@Zoomin as Varchar(30)
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

		print 'XtraPara'
	print @XtraPara
	-------Temporary Table Creation ----
SELECT  TableRowId =IDENTITY(INT,1,1),PART=0,PARTSR='AAAA',SRNO= SPACE(2),INV_NO=SPACE(40), H.DATE,ORG_INVNO=SPACE(40)
	, H.DATE AS ORG_DATE, D.QTY, d.u_asseamt AS Taxableamt, d.CGST_PER AS RATE, d.CGST_PER, D.CGST_AMT, d.SGST_PER, D.SGST_AMT
	, d.IGST_PER,D.IGST_AMT,D.IGST_AMT as Cess_Amt,D.IGST_AMT as Cessr_Amt	,D.GRO_AMT, IT.IT_NAME
	, cast(IT.IT_DESC as varchar(250)) as IT_DESC , Isservice=SPACE(150), IT.HSNCODE
	,HSN_DESC = IT.SERTY
	,ac_name = cast((CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.Mailname, '') ELSE isnull(ac.ac_name, '') END) as varchar(150))
	, gstin = space(30), location = (CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.state, '') ELSE isnull(ac.state, '') END)
	,SUPP_TYPE = SPACE(100),st_type= SPACE(100),StateCode=space(5),Ecomgstin =space(30),from_srno =space(30),to_srno =space(30)
	,ORG_GSTIN =space(30) ,SBBILLNO=space(50),SBDATE=H.DATE,rptmonth=SPACE(15),rptyear =SPACE(15) ,Amenddate
	,Inputtype=space(80),Av_CGST_AMT=D.IGST_AMT,Av_sGST_AMT=D.IGST_AMT,Av_iGST_AMT=D.IGST_AMT,av_CESS_AMT=D.IGST_AMT,net_amt=D.GRO_AMT
	,h.Entry_ty,Tran_cd=cast(0 as int) --Rup
	,mEntry=h.Entry_ty  --Added by Priyanka B on 19112019 for Bug-32948
	,xPara=cast('' as varchar(20))  --Added by Priyanka B on 12022020 for Bug-33268
	into #SHANX1
	FROM  STMAIN H 
	INNER JOIN STITEM D ON (H.ENTRY_TY = D .ENTRY_TY AND H.TRAN_CD = D .TRAN_CD) 
	INNER JOIN IT_MAST IT ON (D .IT_CODE = IT.IT_CODE) 
	LEFT OUTER JOIN shipto ON (shipto.shipto_id = h.scons_id) 
	LEFT OUTER JOIN ac_mast ac ON (h.cons_id = ac.ac_id)  
	WHERE 1=2

/* GSTR_VW DATA STORED IN TEMPORARY TABLE*/
SELECT DISTINCT HSN_CODE, CAST(HSN_DESC AS VARCHAR(250)) AS HSN_DESC INTO #HSN FROM HSN_MASTER

SELECT mEntry=case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end,
		RATE1= (case when (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0)) >0  then (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0))
					when (isnull(A.IGST_PER,0)) > 0 then (isnull(A.IGST_PER,0)) else isnull(A.gstrate,0) end)
		,igst_amt1=ISNULL(a.igst_amt,0),cgst_amt1=ISNULL(a.cgst_amt,0),sGST_AMT1=isnull(a.sGST_AMT,0),CESS_AMT1 =isnull(a.CESS_AMT,0)
		,igsrt_amt1=ISNULL(a.IGSRT_AMT,0),cgsrt_amt1=ISNULL(a.CGSRT_AMT,0),sGSrT_AMT1=isnull(a.SGSRT_AMT,0),CESSR_AMT1 =isnull(a.CessR_amt,0)
		,Ecomgstin = isnull(ac.gstin,'')
		,A.AGAINSTGS,A.AmendDate,A.buyer_gstin,A.buyer_PANNO,A.buyer_pos,A.buyer_st_type,A.buyer_SUPP_TYPE,A.cess_amt,A.cessr_amt,A.cessrate,A.CGSRT_AMT
		,A.CGST_AMT,A.CGST_PER,A.Cons_ac_name,A.Cons_gstin,A.Cons_PANNO,A.Cons_pos,A.Cons_st_type,A.Cons_SUPP_TYPE,A.Consgstin,A.DATE,A.ecomac_id,A.Entry_ty
		,A.EXPOTYPE,A.GRO_AMT,A.GSTIN,A.gstrate,A.hsncode,A.IGSRT_AMT,A.IGST_AMT,A.IGST_PER,A.INV_NO,A.inv_sr,A.Isservice,A.IT_CODE,A.IT_NAME,A.ITSERIAL,A.l_yn
		,A.LineRule,A.net_amt,A.ORG_DATE,A.ORG_INVNO,A.pos,A.pos_std_cd,A.QTY,A.RevCharge,A.SBBILLNO,A.SBDATE,A.SGSRT_AMT,A.SGST_AMT,A.SGST_PER,A.ST_TYPE
		,A.SUPP_TYPE,A.Taxableamt,A.Tran_cd,A.uqc
		,HSN_DESC=ISNULL((SELECT TOP 1 HSN_DESC FROM #HSN WHERE HSN_CODE = A.HSNCODE),'')
		,xPara=cast('' as varchar(20))  --Added by Priyanka B on 12022020 for Bug-33268
INTO #SHANX1TBL 
FROM [GSTR1_VW] A
left join ac_mast ac on (A.ecomac_id=ac.ac_id)
inner join lcode l  on (a.entry_ty=l.entry_ty)
WHERE (A.DATE BETWEEN @SDATE AND @EDATE) and A.AMENDDATE=''			--Added by Shrikant S. on 15/11/2019 for AU 2.2.2

--left Join (Select Entry_ty,Tran_cd From [GSTR1_VW] Where AMENDDATE<>'' group by Entry_ty,Tran_cd ) B On(A.Entry_ty=B.Entry_ty and A.Tran_cd=B.Tran_cd)		--Commented by Shrikant S. on 15/11/2019 for AU 2.2.2
--WHERE (A.DATE BETWEEN @SDATE AND @EDATE) and A.AMENDDATE='' and isnull(B.Tran_cd,0)<=0												--Commented by Shrikant S. on 15/11/2019 for AU 2.2.2


-----Amended Detail Temp table 
--INSERT INTO #SHANX1TBL					--Commented by Shrikant S. on 15/11/2019 for AU 2.2.2
SELECT mEntry=case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end,
		RATE1= (case when (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0)) >0  then (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0))
					when (isnull(A.IGST_PER,0)) > 0 then (isnull(A.IGST_PER,0)) else isnull(A.gstrate,0) end)
		,igst_amt1=ISNULL(a.igst_amt,0),cgst_amt1=ISNULL(a.cgst_amt,0),sGST_AMT1=isnull(a.sGST_AMT,0),CESS_AMT1 =isnull(a.CESS_AMT,0)
		,igsrt_amt1=ISNULL(a.IGSRT_AMT,0),cgsrt_amt1=ISNULL(a.CGSRT_AMT,0),sGSrT_AMT1=isnull(a.SGSRT_AMT,0),CESSR_AMT1 =isnull(a.CessR_amt,0)
		,Ecomgstin = isnull(ac.gstin,'')
		,A.AGAINSTGS,A.AmendDate,A.buyer_gstin,A.buyer_PANNO,A.buyer_pos,A.buyer_st_type,A.buyer_SUPP_TYPE,A.cess_amt,A.cessr_amt,A.cessrate,A.CGSRT_AMT
		,A.CGST_AMT,A.CGST_PER,A.Cons_ac_name,A.Cons_gstin,A.Cons_PANNO,A.Cons_pos,A.Cons_st_type,A.Cons_SUPP_TYPE,A.Consgstin,A.DATE,A.ecomac_id,A.Entry_ty
		,A.EXPOTYPE,A.GRO_AMT,A.GSTIN,A.gstrate,A.hsncode,A.IGSRT_AMT,A.IGST_AMT,A.IGST_PER,A.INV_NO,A.inv_sr,A.Isservice,A.IT_CODE,A.IT_NAME,A.ITSERIAL,A.l_yn
		,A.LineRule,A.net_amt,A.ORG_DATE,A.ORG_INVNO,A.pos,A.pos_std_cd,A.QTY,A.RevCharge,A.SBBILLNO,A.SBDATE,A.SGSRT_AMT,A.SGST_AMT,A.SGST_PER,A.ST_TYPE
		,A.SUPP_TYPE,A.Taxableamt,A.Tran_cd,A.uqc
		,HSN_DESC=ISNULL((SELECT TOP 1 HSN_DESC FROM #HSN WHERE HSN_CODE = A.HSNCODE),'')
		,xPara=cast('' as varchar(20))  --Added by Priyanka B on 12022020 for Bug-33268
INTO #GSTR1AMD				--Added by Shrikant S. on 15/11/2019 for AU 2.2.2
FROM [GSTR1_VW] A
left join ac_mast ac on (A.ecomac_id=ac.ac_id)
inner join lcode l  on (a.entry_ty=l.entry_ty)
WHERE (A.AMENDDATE BETWEEN @SDATE AND @EDATE) AND A.HSNCODE <> ''



/*3A.  Supplies made to consumers and un-registered persons (Net of debit / credit notes ) */
Insert Into #SHANX1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
	,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE
	,Entry_ty,Tran_Cd --Rup
	,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
	,xPara  --Added by Priyanka B on 12022020 for Bug-33268
	)
Select * 
from (
		/*			--Commented by Shrikant S. on 08/11/2019 for AU 2.2.2		--Start
		select 3 AS PART ,'3A' AS PARTSR,'A' AS SRNO,gstin,ORG_INVNO as inv_no,ORG_DATE as date,pos as location		
		,sum((case when mEntry in('ST','SB','DN') THEN GRO_AMT else -GRO_AMT END)) as GRO_AMT,rate1
		,taxableamt =sum((case when mEntry in('ST','SB','DN') THEN +(taxableamt)ELSE - (taxableamt) END ))
		,CGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(CGST_AMT1) ELSE - (CGST_AMT1) END ))
		,SGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(SGST_AMT1) ELSE - (SGST_AMT1) END ))
		,IGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(IGST_AMT1) ELSE - (IGST_AMT1) END ))
		,cess_amt = sum((case when mEntry in('ST','SB','DN') THEN +(cess_amt1) ELSE - (cess_amt1) END ))
		,'' as AC_NAME,'Invoice'as SUPP_TYPE,'' as ST_TYPE,hsncode,SBBILLNO,SBDATE,Entry_ty,Tran_Cd--Rup
		from #SHANX1TBL  
		where (mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB')
		and supp_type IN ('UnRegistered','')  and st_type <> 'Out of country' and ecomgstin=''
		group by rate1,ORG_INVNO,ORG_DATE,gstin,pos,hsncode,SBBILLNO,SBDATE,Cons_Ac_Name,Entry_ty,Tran_Cd --Rup			
		--Commented by Shrikant S. on 08/11/2019 for AU 2.2.2		--End */

		--Added by Shrikant S. on 08/11/2019 for AU 2.2.2		--Start
		select 3 AS PART ,'3A' AS PARTSR,'A' AS SRNO,gstin,inv_no,date,pos as location		
		,sum(GRO_AMT) as GRO_AMT,rate1
		,taxableamt =sum(taxableamt)
		,CGST_AMT = sum(CGST_AMT1)
		,SGST_AMT = sum(SGST_AMT1)
		,IGST_AMT = sum(IGST_AMT1)
		,cess_amt = sum(cess_amt1)
		,'' as AC_NAME
		,SUPP_TYPE=Case  when mEntry='CN' THEN 'Credit Note' when mEntry='DN' THEN 'Debit Note' else 'Invoice' end
		,'' as ST_TYPE,hsncode,SBBILLNO,SBDATE,Entry_ty,Tran_Cd--Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		,xPara=case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
		from #SHANX1TBL  
		where (mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB')
		and supp_type IN ('UnRegistered','')  and st_type <> 'Out of country' 
		--and ecomgstin=''						--Commented by Shrikant S. on 11/11/2019 for AU 2.2.2
		group by rate1,inv_no,date,gstin,pos,hsncode,SBBILLNO,SBDATE,Cons_Ac_Name,Entry_ty,Tran_Cd,mEntry --Rup			
		--Added by Shrikant S. on 08/11/2019 for AU 2.2.2		--End

	) aa Order by GSTIN,DATE,INV_NO,rate1


IF NOT EXISTS(SELECT PART FROM #SHANX1 WHERE PARTSR = '3A' AND SRNO ='A')
BEGIN
	Insert Into #SHANX1(PART,PARTSR,SRNO,gstin,location,inv_no,date,gro_amt,taxableamt
		,SGST_PER,SGST_AMT,CGST_PER,CGST_AMT,IGST_PER,IGST_AMT,AC_NAME,SUPP_TYPE,ST_TYPE,StateCode,cess_amt
		,xPara --Added by Priyanka B on 12022020 for Bug-33268
		)
	VALUES(3,'3A','A','','','','',0,0,0,0,0,0,0,0,'','','','',0
	,case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
	)
END



/*	3B. Supplies made to registered persons (other than those attracting reverse charge)(including edit/amendment)	*/
	IF  @XtraPara Not in ('SAHAJ')
	Begin
		
		Insert Into #SHANX1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE
			,Entry_ty,Tran_Cd --Rup
			,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
		SELECT 3 AS PART ,'3B' AS PARTSR,'B' AS SRNO,gstin,inv_no,date,POS
		,gro_amt=SUM(gro_amt)  
		,rate1,sum(taxableamt)taxableamt
		,sum(CGST_AMT1)CGST_AMT ,sum(SGST_AMT1)SGST_AMT,sum(IGST_AMT1)IGST_AMT,sum(cess_amt1)cess_amt 
		--,'' as AC_NAME --Commented by Rup
		,Cons_Ac_Name as AC_NAME --Rup
		,'Invoice'as SUPP_TYPE,'' as ST_TYPE,hsncode,SBBILLNO,SBDATE,Entry_ty,Tran_Cd  --Rup 
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		,xPara=case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
		from #SHANX1TBL
		where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type <> 'Out of country' 
		and supp_type IN ('Registered','Compounding') 
		and gstin <>'' AND LineRule = 'Taxable' AND HSNCODE <> '' 
		--and ecomgstin=''						--Commented by Shrikant S. on 11/11/2019 for AU 2.2.2
		and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) > 0
		group by gstin,inv_no,date,POS,rate1,hsncode,SBBILLNO,SBDATE,Cons_Ac_Name,Entry_ty,Tran_Cd
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		Order by gstin,Date,Inv_no,Rate1


		IF NOT EXISTS(SELECT PART FROM #SHANX1 WHERE PARTSR = '3B' AND SRNO ='B') 
		BEGIN
			Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(3,'3B','B','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End

/*	3C. Exports with payment of tax	*/
	IF  @XtraPara Not in ('SAHAJ','SUGAM')
	Begin
		Insert Into #SHANX1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE
			,Entry_ty,Tran_Cd--Rup
			,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
		SELECT 3 AS PART ,'3C' AS PARTSR,'C' AS SRNO,gstin,inv_no,date,POS
		,gro_amt=SUM(GRO_AMT)  
		,rate1,sum(taxableamt)taxableamt
		,sum(CGST_AMT1)CGST_AMT ,sum(SGST_AMT1)SGST_AMT,sum(IGST_AMT1)IGST_AMT,sum(cess_amt1)cess_amt 
		--,'' as AC_NAME --Commented by Rup
		,Cons_Ac_Name as AC_NAME --Rup
		,'Invoice'as SUPP_TYPE,'' as ST_TYPE,hsncode,SBBILLNO,SBDATE ,Entry_ty,Tran_Cd --Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		,xPara=case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
		from #SHANX1TBL
		where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type ='Out of country' 
		and supp_type IN ('')  
		--and ecomgstin=''						--Commented by Shrikant S. on 11/11/2019 for AU 2.2.2
		and gstin ='' AND LineRule = 'Taxable' AND HSNCODE <> '' 
		and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) > 0
		group by gstin,inv_no,date,POS,rate1,hsncode,SBBILLNO,SBDATE,Cons_Ac_Name,Entry_ty,Tran_Cd --Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		Order by GSTIN,DATE,INV_NO,rate1


		IF NOT EXISTS(SELECT PART FROM #SHANX1 WHERE PARTSR = '3C' AND SRNO ='C') 
		BEGIN
			Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(3,'3C','C','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End

/*	3D. Exports without payment of tax	*/

	IF  @XtraPara Not in ('SAHAJ','SUGAM')
	Begin
		Insert Into #SHANX1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE
			,Entry_ty,Tran_Cd --Rup
			,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
		SELECT 3 AS PART ,'3D' AS PARTSR,'D' AS SRNO,gstin,inv_no,date,POS
		,gro_amt=SUM(gro_amt)
		,rate1,sum(taxableamt)taxableamt
		,sum(CGST_AMT1)CGST_AMT ,sum(SGST_AMT1)SGST_AMT,sum(IGST_AMT1)IGST_AMT,sum(cess_amt1)cess_amt 
		--,'' as AC_NAME --Commented by Rup
		,Cons_Ac_Name as AC_NAME --Rup
		,'Invoice'as SUPP_TYPE,'' as ST_TYPE,hsncode,SBBILLNO,SBDATE,Entry_ty,Tran_Cd --Rup 
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		,xPara=case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
		from #SHANX1TBL
		where (mEntry in ('ST','SB') and entry_ty<>'UB') and st_type ='Out of country' 
		and supp_type IN ('') 
		and gstin ='' AND LineRule = 'Nil Rated' AND HSNCODE <> ''  
		--and ecomgstin=''						--Commented by Shrikant S. on 11/11/2019 for AU 2.2.2
		and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) = 0
		group by gstin,inv_no,date,POS,rate1,hsncode,SBBILLNO,SBDATE,Cons_Ac_Name,Entry_ty,Tran_Cd --Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		Order by GSTIN,DATE,INV_NO,rate1


		IF NOT EXISTS(SELECT PART FROM #SHANX1 WHERE PARTSR = '3D' AND SRNO ='D') 
		BEGIN
			Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(3,'3D','D','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End

/*	3E. Supplies to SEZ units/developers with payment of tax (including edit/amendment)	*/
	IF  @XtraPara Not in ('SAHAJ','SUGAM')
	Begin
		Insert Into #SHANX1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE
			,Entry_ty,Tran_Cd --Rup
			,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
		SELECT 3 AS PART ,'3E' AS PARTSR,'E' AS SRNO,gstin,inv_no,date,POS
		,gro_amt=SUM(gro_amt)
		,rate1,sum(taxableamt)taxableamt
		,sum(CGST_AMT1)CGST_AMT ,sum(SGST_AMT1)SGST_AMT,sum(IGST_AMT1)IGST_AMT,sum(cess_amt1)cess_amt 
		--,'' as AC_NAME --Commented by Rup
		,Cons_Ac_Name as AC_NAME --Rup
		,'Invoice'as SUPP_TYPE,'' as ST_TYPE,hsncode,SBBILLNO,SBDATE ,Entry_ty,Tran_Cd --Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		,xPara=case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
		from #SHANX1TBL
		where (mEntry in ('ST','SB') and entry_ty<>'UB') 
		and supp_type IN ('SEZ') 
		--and ecomgstin=''						--Commented by Shrikant S. on 11/11/2019 for AU 2.2.2
		AND LineRule = 'Taxable' AND HSNCODE <> '' and st_type iN('INTERSTATE','INTRASTATE') 
		and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) > 0
		group by gstin,inv_no,date,POS,rate1,hsncode,SBBILLNO,SBDATE,Cons_Ac_Name,Entry_ty,Tran_Cd --Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		Order by GSTIN,DATE,INV_NO,rate1

		IF NOT EXISTS(SELECT PART FROM #SHANX1 WHERE PARTSR = '3E' AND SRNO ='E') 
		BEGIN
			Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(3,'3E','E','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End

/*	3F. Supplies to SEZ units/developers without payment of tax (including edit/amendment)	*/
	IF  @XtraPara Not in ('SAHAJ','SUGAM')
	Begin
		Insert Into #SHANX1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE
			,Entry_ty,Tran_Cd --Rup
			,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			SELECT 3 AS PART ,'3F' AS PARTSR,'F' AS SRNO,gstin,inv_no,date,POS
			,gro_amt=SUM(gro_amt)
			,rate1,sum(taxableamt)taxableamt
			,sum(CGST_AMT1)CGST_AMT ,sum(SGST_AMT1)SGST_AMT,sum(IGST_AMT1)IGST_AMT,sum(cess_amt1)cess_amt 
			--,'' as AC_NAME --Commented by Rup
			,Cons_Ac_Name as AC_NAME --Rup
			,'Invoice'as SUPP_TYPE,'' as ST_TYPE,hsncode,SBBILLNO,SBDATE,Entry_ty,Tran_Cd --Rup 
			,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
			,xPara=case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
			from #SHANX1TBL
			where (mEntry in ('ST','SB') and entry_ty<>'UB') 
			and supp_type IN ('SEZ') 
			--and ecomgstin=''						--Commented by Shrikant S. on 11/11/2019 for AU 2.2.2
			AND LineRule = 'Nil Rated' AND HSNCODE <> '' and st_type iN('INTERSTATE','INTRASTATE') 
			and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) = 0
			group by gstin,inv_no,date,POS,rate1,hsncode,SBBILLNO,SBDATE,Cons_Ac_Name,Entry_ty,Tran_Cd --Rup
			,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
			Order by GSTIN,DATE,INV_NO,rate1

		IF NOT EXISTS(SELECT PART FROM #SHANX1 WHERE PARTSR = '3F' AND SRNO ='F') 
		BEGIN
			Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(3,'3F','F','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End


/*	3G. Deemed exports (including edit/amendment)	*/	
	IF  @XtraPara Not in ('SAHAJ','SUGAM')
	Begin
		Insert Into #SHANX1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE
			,Entry_ty,Tran_Cd --Rup
			,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			SELECT 3 AS PART ,'3G' AS PARTSR,'G' AS SRNO,gstin,inv_no,date,POS
			,gro_amt=SUM(gro_amt)
			,rate1,sum(taxableamt)taxableamt
			,sum(CGST_AMT1)CGST_AMT ,sum(SGST_AMT1)SGST_AMT,sum(IGST_AMT1)IGST_AMT,sum(cess_amt1)cess_amt 
			--,'' as AC_NAME --Commented by Rup
			,Cons_Ac_Name as AC_NAME --Rup
			,'Invoice'as SUPP_TYPE,'' as ST_TYPE,hsncode,SBBILLNO,SBDATE ,Entry_ty,Tran_Cd --Rup
			,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
			,xPara=case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
			from #SHANX1TBL
			where (mEntry in ('ST','SB') and entry_ty<>'UB') 
			and supp_type IN ('EOU') 
			group by gstin,inv_no,date,POS,rate1,hsncode,SBBILLNO,SBDATE,Cons_Ac_Name,Entry_ty,Tran_Cd --Rup
			,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
			Order by GSTIN,DATE,INV_NO,rate1

		IF NOT EXISTS(SELECT PART FROM #SHANX1 WHERE PARTSR = '3G' AND SRNO ='G') 
		BEGIN
			Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(3,'3G','G','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End


/* 3H. Inward supplies received from a registered supplier (attracting reverse charge) */
SELECT mEntry=case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end,
	RATE1= (case when (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0)) >0  then (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0))when (isnull(A.IGST_PER,0)) > 0 then (isnull(A.IGST_PER,0))else isnull(A.gstrate,0) end)
		,iigst_amt=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((ISNULL(a.igst_amt,0)+ ISNULL(a.IGSRT_AMT,0))-ISNULL(b.iigst_amt,0)) else 0 end)
		,icgst_amt=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((ISNULL(a.cgst_amt,0) + ISNULL(a.CGSRT_AMT,0)) - ISNULL(b.icgst_amt,0)) else 0 end)
		,isGST_AMT=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((isnull(a.sGST_AMT,0)+ isnull(a.SGSRT_AMT,0))-isnull(b.isGST_AMT,0)) else 0 end)
		,ICESS_AMT =(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((isnull(a.CESS_AMT,0)+ isnull(a.CessRT_amt,0)) - isnull(B.ICESS_AMT,0)) else 0 end)
		,A.AGAINSTGS,A.AmendDate,A.AVL_ITC,A.BEDT,A.beno,A.Cess_amt,A.Cess_per,A.CessRT_amt,A.CGSRT_AMT,A.CGST_AMT,A.CGST_PER
		,A.Cons_ac_name,A.Cons_gstin,A.Cons_PANNO,A.Cons_pos,A.Cons_st_type,A.Cons_SUPP_TYPE --Rup
		,A.DATE,A.Entry_ty,A.GRO_AMT,A.GSTIN,A.gstrate,A.gstype,A.HSNCODE,A.IGSRT_AMT,A.IGST_AMT,A.IGST_PER,A.INV_NO
		,A.inv_sr,A.Isservice,A.IT_CODE,A.IT_NAME,A.ITSERIAL,A.l_yn,A.LineRule,A.net_amt,A.OLDGSTIN,A.ORG_DATE,A.ORG_INVNO,A.orgbedt,A.orgbeno,A.pinvdt,A.pinvno
		,A.portcode,A.pos,A.pos_std_cd,A.QTY,A.RevCharge
		,A.seller_gstin,A.seller_PANNO,A.seller_pos,A.seller_st_type,A.seller_SUPP_TYPE --Rup
		,A.SGSRT_AMT,A.SGST_AMT
		,A.SGST_PER,A.ST_TYPE,A.SUPP_TYPE,A.Taxableamt,A.Tran_cd,A.TRANSTATUS,A.uqc
		INTO #GSTR2TBL 
		FROM GSTR2_VW A  
		LEFT OUTER JOIN Epayment B ON (A.Tran_cd =B.tran_cd AND A.Entry_ty =B.entry_ty AND A.ITSERIAL =B.itserial)  
		inner join lcode l  on (a.entry_ty=l.entry_ty)
		WHERE (A.DATE BETWEEN @SDATE AND @EDATE) and A.AmendDate=''
			
	select 	a.Tran_cd,a.Entry_ty,a.Main_tran,a.ENTRY_ALL
	into #rcmbill
	from mainall_vw a
	where date<=@EDATE and a.Entry_all='BP'
	and a.Entry_ty<>'GB'					--Added by Shrikant S. on 11/11/2019 for AU 2.2.2
	group by  a.Tran_cd,a.Entry_ty,a.Main_tran,a.ENTRY_ALL



--	select A.* into #GSTR2TBL_RCM from #GSTR2TBL A --Commented by Rup
	select A.*,isnull(b.Tran_cd,0) as BillTran into #GSTR2TBL_RCM from  #GSTR2TBL A  --Rup
	left outer join #rcmbill b on (a.tran_cd =b.Main_tran and a.entry_ty =b.ENTRY_ALL)  --Rup
		
	
	



	select Entry_ty=b.Entry_all,Tran_cd=b.Main_tran,cgsrt_amt=sum(cgsrt_amt),sgsrt_amt=sum(sgsrt_amt),igsrt_amt=sum(igsrt_amt),CessRT_amt=sum(CessRT_amt) 
		,TaxableAmt=sum(TaxableAmt),GRO_AMT=sum(GRO_AMT)
		into #tblbill   from #GSTR2TBL a
		Inner Join #rcmbill b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.tran_Cd)
		Group by b.Entry_all,b.Main_tran


	update #GSTR2TBL_RCM set cgsrt_amt=b.cgsrt_amt - a.cgsrt_amt,sgsrt_amt=b.sgsrt_amt - a.sgsrt_amt ,igsrt_amt=b.igsrt_amt - a.igsrt_amt  
		,CessRT_amt=b.CessRT_amt-a.CessRT_amt,Taxableamt=b.TaxableAmt-a.TaxableAmt,Gro_amt=b.GRO_AMT-a.Gro_amt
		from #tblbill a Inner Join #GSTR2TBL_RCM b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.Tran_cd)
	
	
	Delete from #GSTR2TBL_RCM where cgsrt_amt=0 and sgsrt_amt=0 and igsrt_amt=0 and CessRT_amt=0 and Taxableamt=0 and GRO_AMT=0 and Entry_ty='BP'	


	


	/*			--Commented by Shrikant S. on 08/11/2019 for AU 2.2.2			--Start
	Select 3 as part,'3H' as partsr ,'H' srno,pinvno=Case when mENTRY IN ('PR','CN','DN') then ORG_INVNO else pinvno end 
	,pinvdt=Case when mENTRY IN ('PR','CN','DN') then ORG_DATE else pinvdt end,Net_amt,Rate1
	,TaxableAmt = SUM(CASE WHEN mENTRY IN ('EP','PT','CN','BP') THEN TaxableAmt ELSE -TaxableAmt END)
	,CGSRT_AMT=SUM(CASE WHEN mENTRY IN ('EP','PT','CN','BP') THEN CGSRT_AMT ELSE -CGSRT_AMT END) 
	,SGSRT_AMT=SUM(CASE WHEN mENTRY IN ('EP','PT','CN','BP') THEN SGSRT_AMT ELSE -SGSRT_AMT END)
	,IGSRT_AMT=SUM(CASE WHEN mENTRY IN ('EP','PT','CN','BP') THEN IGSRT_AMT ELSE -IGSRT_AMT END)
	,CessRt_Amt=sum(CASE WHEN mENTRY IN ('EP','PT','CN','BP') THEN CessRt_Amt ELSE -CessRt_Amt END) 
	,gstin
	,pos,gstype
	,Av_CGST_AMT=0
	,Av_sGST_AMT=0
	,Av_iGST_AMT=0
	,av_CESS_AMT = 0
	,supp_type='Bill of Supply'
	,gro_amt=sum(CASE WHEN mENTRY IN ('EP','PT','CN','BP') THEN TaxableAmt ELSE -TaxableAmt END)
	,hsncode
	,Entry_Ty,Tran_Cd,Cons_ac_name  --Rup
	into #tmp3h
	FROM #GSTR2TBL_RCM
	where mENTRY IN ('EP','PT','PR','CN','DN','BP')
	and SUPP_TYPE in ('Registered','Unregistered')
	and ltrim(rtrim(HSNCODE)) <>'' 
	AND LineRule = 'Taxable'
	AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT > 0)
	GROUP BY pinvno,pinvdt,Net_amt,Rate1,gstin,pos,gstype,oldgstin,ST_TYPE,hsncode,ORG_INVNO,ORG_DATE,mENTRY
	,Entry_Ty,Tran_Cd,Cons_ac_name  --Rup
	ORDER BY gstin,pinvdt,pinvno,Rate1  
	--Commented by Shrikant S. on 08/11/2019 for AU 2.2.2			--End	*/

	--Added by Shrikant S. on 08/11/2019 for AU 2.2.2			--Start
	Select 3 as part,'3H' as partsr ,'H' srno,pinvno=Case when mENTRY IN ('PR','CN','DN') then INV_NO else pinvno end 
	,pinvdt=Case when mENTRY IN ('PR','CN','DN') then DATE else pinvdt end,Net_amt,Rate1
	,TaxableAmt = SUM(TaxableAmt)	,CGSRT_AMT=SUM(CGSRT_AMT ) 	,SGSRT_AMT=SUM(SGSRT_AMT)	,IGSRT_AMT=SUM(IGSRT_AMT )	,CessRt_Amt=sum(CessRt_Amt) 
	,gstin	,pos,gstype	,Av_CGST_AMT=0	,Av_sGST_AMT=0	,Av_iGST_AMT=0	,av_CESS_AMT = 0
	,supp_type=Case  when mEntry='CN' THEN 'Credit Note' when mEntry='DN' THEN 'Debit Note' when mEntry='BP' THEN 'Advance Payment' else 'Bill of Supply' end
	,gro_amt=sum(TaxableAmt)
	,hsncode
	,Entry_Ty,Tran_Cd,Cons_ac_name  --Rup
	,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
	into #tmp3h
	FROM #GSTR2TBL_RCM
	where mENTRY IN ('EP','PT','PR','CN','DN','BP')
	and SUPP_TYPE in ('Registered','Unregistered')
	and ltrim(rtrim(HSNCODE)) <>'' 
	AND LineRule = 'Taxable'
	AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT > 0)
	GROUP BY pinvno,pinvdt,Net_amt,Rate1,gstin,pos,gstype,oldgstin,ST_TYPE,hsncode,inv_no,date,mENTRY
	,Entry_Ty,Tran_Cd,Cons_ac_name  --Rup
	ORDER BY gstin,pinvdt,pinvno,Rate1  
	--Added by Shrikant S. on 08/11/2019 for AU 2.2.2			--End

	Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
	CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,supp_type,gro_amt,hsncode
	,Entry_Ty,Tran_Cd,ac_name --Rup
	,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
	,xPara  --Added by Priyanka B on 12022020 for Bug-33268
	)
	select 
	part,partsr ,srno,pinvno,pinvdt,Net_amt=0,Rate1
	,TaxableAmt = SUM(TaxableAmt),CGSRT_AMT=SUM(CGSRT_AMT),SGSRT_AMT=SUM(SGSRT_AMT)
	,IGSRT_AMT=SUM(IGSRT_AMT),CessRt_Amt=sum(CessRt_Amt) 
	,gstin,pos,gstype,Av_CGST_AMT=0,Av_sGST_AMT=0,Av_iGST_AMT=0,av_CESS_AMT = 0	,supp_type
	,gro_amt=sum(gro_amt )
	,hsncode,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
	,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
	,xPara=case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
	from #tmp3h
	group by part,partsr ,srno,pinvno,pinvdt,gstin,pos,gstype,supp_type,hsncode,Rate1,Entry_Ty,Tran_Cd,Cons_ac_name--Rup 
	,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
	ORDER BY gstin,pinvdt,pinvno,Rate1  

	
	IF NOT EXISTS(SELECT PART FROM #SHANX1 WHERE PARTSR = '3H' AND SRNO ='H') 
	BEGIN
		Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
		,xPara  --Added by Priyanka B on 12022020 for Bug-33268
		)
		values(3,'3H','H','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
		,case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
		)
	END
	
	/*	Import of services (net of debit/ credit notes and advances paid, if any)		*/
	IF  @XtraPara Not in ('SAHAJ','SUGAM')
	Begin

		/*			--Commented by Shrikant S. on 08/11/2019 for AU 2.2.2		--Start
		Select 3 as part,'3I' as partsr ,'I' srno,pinvno=Case when mENTRY IN ('PR','CN','DN') then ORG_INVNO else pinvno end 
		,pinvdt=Case when mENTRY IN ('PR','CN','DN') then ORG_DATE else pinvdt end
		,Net_amt,Rate1,
		SUM(CASE WHEN mENTRY IN  ('EP','PT','CN','BP') then TaxableAmt else -TaxableAmt end)TaxableAmt 
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN','BP') then CGSRT_AMT else -CGSRT_AMT end)CGSRT_AMT
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN','BP') then SGSRT_AMT else -SGSRT_AMT end)SGSRT_AMT
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN','BP') then IGSRT_AMT else -IGSRT_AMT end)IGSRT_AMT
		,sum(CASE WHEN mENTRY IN  ('EP','PT','CN','BP') then CessRt_amt else -CessRt_amt end) as CessRt_amt
		,gstin,pos,gstype
		,Av_CGST_AMT=0
		,Av_sGST_AMT=0
		,Av_iGST_AMT=0
		,av_CESS_AMT = 0
		,supp_type=case when ST_TYPE='Out of Country' then 'Bill of Entry' else 'Bill of Supply' end
		,gro_amt=SUM(CASE WHEN mENTRY IN  ('EP','PT','CN','BP') then TaxableAmt else -TaxableAmt end)
		,hsncode
		,SBBILLNO=beno,SBDATE=bedt
		,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
		Into #tmp3I
		FROM #GSTR2TBL_RCM 
		where mENTRY IN ('EP','PT','CN','DN','BP')
		and Isservice = 'Services'
		AND ST_TYPE IN('Out of Country','Interstate','Intrastate') and SUPP_TYPE in('Import','EOU','')
		
		AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) > 0
		GROUP BY pinvno,pinvdt,Net_amt,Rate1,gstin,pos,gstype,oldgstin,ST_TYPE,hsncode,beno,bedt,ORG_INVNO,ORG_DATE,mENTRY,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
		--Commented by Shrikant S. on 08/11/2019 for AU 2.2.2		--End */





		--Added by Shrikant S. on 08/11/2019 for AU 2.2.2		--Start
		Select 3 as part,'3I' as partsr ,'I' srno,pinvno=Case when mENTRY IN ('PR','CN','DN') then Inv_no else pinvno end 
		,pinvdt=Case when mENTRY IN ('PR','CN','DN') then Date else pinvdt end
		,Net_amt,Rate1,
		SUM(TaxableAmt) as TaxableAmt,SUM(CGSRT_AMT) as CGSRT_AMT,SUM(SGSRT_AMT) as SGSRT_AMT,SUM(IGSRT_AMT) as IGSRT_AMT,sum(CessRt_amt) as CessRt_amt
		,gstin,pos,gstype,Av_CGST_AMT=0,Av_sGST_AMT=0,Av_iGST_AMT=0,av_CESS_AMT = 0		
		,supp_type=Case  when mEntry='CN' THEN 'Credit Note' when mEntry='DN' THEN 'Debit Note' when mEntry='BP' THEN 'Advance Payment' else (case when ST_TYPE='Out of Country' then 'Bill of Entry' else 'Bill of Supply' end) end
		,gro_amt=SUM(TaxableAmt)
		,hsncode
		,SBBILLNO=beno,SBDATE=bedt
		,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		Into #tmp3I
		FROM #GSTR2TBL_RCM 
		where mENTRY IN ('EP','PT','CN','DN','BP')
		and Isservice = 'Services'
		AND ST_TYPE IN('Out of Country','Interstate','Intrastate') and SUPP_TYPE in('Import','EOU','')
		AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) > 0
		GROUP BY pinvno,pinvdt,inv_no,date,Net_amt,Rate1,gstin,pos,gstype,oldgstin,ST_TYPE,hsncode,beno,bedt,ORG_INVNO,ORG_DATE,mENTRY,Entry_Ty,Tran_Cd,Cons_ac_name,mENTRY--Rup
		--Added by Shrikant S. on 08/11/2019 for AU 2.2.2		--End


		Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,supp_type,gro_amt,hsncode,SBBILLNO,SBDATE
		,Entry_Ty,Tran_Cd,Ac_Name--Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		,xPara  --Added by Priyanka B on 12022020 for Bug-33268
		)
		select 
		part,partsr ,srno,pinvno,pinvdt,Net_amt=0,Rate1
		,TaxableAmt = SUM(TaxableAmt),CGSRT_AMT=SUM(CGSRT_AMT),SGSRT_AMT=SUM(SGSRT_AMT)
		,IGSRT_AMT=SUM(IGSRT_AMT),CessRt_Amt=sum(CessRt_Amt) 
		,gstin,pos,gstype,Av_CGST_AMT=0,Av_sGST_AMT=0,Av_iGST_AMT=0,av_CESS_AMT = 0	,supp_type
		,gro_amt=sum(gro_amt )
		,hsncode
		,SBBILLNO,SBDATE
		,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		,xPara=case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
		from #tmp3I
		group by part,partsr ,srno,pinvno,pinvdt,gstin,pos,gstype,supp_type,hsncode,Rate1,SBBILLNO,SBDATE,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		ORDER BY gstin,pinvdt,pinvno,Rate1  


		IF NOT EXISTS(SELECT PART FROM #SHANX1 WHERE PARTSR = '3I' AND SRNO ='I') 
		BEGIN
			Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(3,'3I','I','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End

	--select * FROM #GSTR2TBL_RCM
	/*	Import of goods	*/
	IF  @XtraPara Not in ('SAHAJ','SUGAM')
	Begin	
		/*			--Commented by Shrikant S. on 08/11/2019 for AU 2.2.2		--Start
		Select 3 as part,'3J' as partsr ,'J' srno,pinvno,pinvdt,Net_amt,Rate1,
		SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then TaxableAmt else -TaxableAmt end)TaxableAmt 
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then CGST_AMT else -CGST_AMT end)CGST_AMT
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then SGST_AMT else -SGST_AMT end)SGST_AMT
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then IGST_AMT else -IGST_AMT end)IGST_AMT
		,sum(CASE WHEN mENTRY IN  ('EP','PT','CN') then  CESS_AMT else -CESS_AMT end) as CESS_AMT
		,gstin,pos,gstype
		--Commented by Rup Start
		--,Av_CGST_AMT=0
		--,Av_sGST_AMT=0
		--,Av_iGST_AMT=0
		--,av_CESS_AMT =0
		--Commented by Rup End
		--Rup Start
		,Av_CGST_AMT=SUM(case when BillTran > 0 then  icgst_amt else 0 end)
		,Av_sGST_AMT=SUM(case when BillTran > 0 then  isGST_AMT else 0 end)
		,Av_iGST_AMT=SUM(case when BillTran > 0 then  iigst_amt else 0 end)
		,av_CESS_AMT = SUM(case when BillTran > 0 then  ICESS_AMT else 0 end)
		--Rup End
		,supp_type=case when ST_TYPE='Out of Country' then 'Bill of Entry' else 'Bill of Supply' end
		,gro_amt=SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then gro_amt else -gro_amt end)
		,hsncode
		,SBBILLNO=beno,SBDATE=bedt
		,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
		into #tmp3j
		FROM #GSTR2TBL_RCM 
		where mENTRY IN ('EP','PT','CN','DN')
		and Isservice <> 'Services'
		AND ST_TYPE IN('Out of Country','Interstate','Intrastate') and SUPP_TYPE in('Import','EOU','')
		AND (CGST_AMT + SGST_AMT + IGST_AMT) > 0
		GROUP BY pinvno,pinvdt,Net_amt,Rate1,gstin,pos,gstype,oldgstin,ST_TYPE,hsncode ,beno,bedt,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
		--Commented by Shrikant S. on 08/11/2019 for AU 2.2.2		--End	 */

		--Added by Shrikant S. on 08/11/2019 for AU 2.2.2		--Start
		Select 3 as part,'3J' as partsr ,'J' srno,pinvno=Case when mENTRY IN ('PR','CN','DN') then Inv_no else pinvno end 
		,pinvdt=Case when mENTRY IN ('PR','CN','DN') then Date else pinvdt end,Net_amt,Rate1,
		SUM(TaxableAmt)TaxableAmt 
		,SUM(CGST_AMT)CGST_AMT
		,SUM(SGST_AMT)SGST_AMT
		,SUM(IGST_AMT)IGST_AMT
		,sum(CESS_AMT) as CESS_AMT
		,gstin,pos,gstype
		--Commented by Rup Start
		--,Av_CGST_AMT=0
		--,Av_sGST_AMT=0
		--,Av_iGST_AMT=0
		--,av_CESS_AMT =0
		--Commented by Rup End
		--Rup Start
		,Av_CGST_AMT=SUM(case when BillTran > 0 then  icgst_amt else 0 end)
		,Av_sGST_AMT=SUM(case when BillTran > 0 then  isGST_AMT else 0 end)
		,Av_iGST_AMT=SUM(case when BillTran > 0 then  iigst_amt else 0 end)
		,av_CESS_AMT = SUM(case when BillTran > 0 then  ICESS_AMT else 0 end)
		--Rup End
		,supp_type=Case  when mEntry='CN' THEN 'Credit Note' when mEntry='DN' THEN 'Debit Note' else (case when ST_TYPE='Out of Country' then 'Bill of Entry' else 'Bill of Supply' end) end 
		,gro_amt=SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then gro_amt else -gro_amt end)
		,hsncode
		,SBBILLNO=beno,SBDATE=bedt
		,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		into #tmp3j
		FROM #GSTR2TBL_RCM 
		where mENTRY IN ('EP','PT','CN','DN')
		and Isservice <> 'Services'
		AND ST_TYPE IN('Out of Country','Interstate','Intrastate') and SUPP_TYPE in('Import','EOU','')
		--AND (CGST_AMT + SGST_AMT + IGST_AMT) > 0			--Commented by Shrikant S. on 11/11/2019
		GROUP BY pinvno,pinvdt,inv_no,date,Net_amt,Rate1,gstin,pos,gstype,oldgstin,ST_TYPE,hsncode ,beno,bedt,Entry_Ty,Tran_Cd,Cons_ac_name,mENTRY--Rup
		--Added by Shrikant S. on 08/11/2019 for AU 2.2.2		--End


		Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,supp_type,gro_amt,hsncode,SBBILLNO,SBDATE
		,Entry_Ty,Tran_Cd,Ac_Name--Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		,xPara  --Added by Priyanka B on 12022020 for Bug-33268
		)
		select 
		part,partsr ,srno,pinvno,pinvdt,Net_amt=0,Rate1
		,TaxableAmt = SUM(TaxableAmt),CGSRT_AMT=SUM(CGST_AMT),SGSRT_AMT=SUM(SGST_AMT)
		,IGSRT_AMT=SUM(IGST_AMT),CessRt_Amt=sum(CESS_AMT) 
		,gstin,pos,gstype,Av_CGST_AMT=0,Av_sGST_AMT=0,Av_iGST_AMT=0,av_CESS_AMT = 0	,supp_type
		,gro_amt=sum(gro_amt )
		,hsncode
		,SBBILLNO,SBDATE,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		,xPara=case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
		from #tmp3j
		group by part,partsr ,srno,pinvno,pinvdt,gstin,pos,gstype,supp_type,hsncode,Rate1,SBBILLNO,SBDATE,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		ORDER BY gstin,pinvdt,pinvno,Rate1  
		
		
		

		IF NOT EXISTS(SELECT PART FROM #SHANX1 WHERE PARTSR = '3J' AND SRNO ='J') 
		BEGIN
			Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(3,'3J','J','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End

	/*	Import of goods from SEZ units / developers on a Bill of Entry	*/
	IF  @XtraPara Not in ('SAHAJ','SUGAM')
	Begin
		/*		--Commented by Shrikant S. on 08/11/2019 for AU 2.2.2		--Start
		Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,supp_type,gro_amt,hsncode,SBBILLNO,SBDATE,Entry_Ty,Tran_Cd,Ac_Name)--Rup
		select * from (
		Select 3 as part,'3K' as partsr ,'K' srno,pinvno,pinvdt,Net_amt,Rate1,
		SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then TaxableAmt else -TaxableAmt end)TaxableAmt 
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then CGSRT_AMT+CGST_AMT else -CGSRT_AMT-CGST_AMT end)CGSRT_AMT
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then SGSRT_AMT+SGST_AMT else -SGSRT_AMT-SGST_AMT end)SGSRT_AMT
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then IGSRT_AMT+IGST_AMT else -IGSRT_AMT-IGST_AMT end)IGSRT_AMT
		,sum(CASE WHEN mENTRY IN  ('EP','PT','CN') then CessRt_amt+CESS_AMT else -CessRt_amt-CESS_AMT end) as CessRt_amt
		,gstin,pos,gstype
		--Commented by Rup Start
		--,Av_CGST_AMT=0
		--,Av_sGST_AMT=0
		--,Av_iGST_AMT=0
		--,av_CESS_AMT =0
		--Commented by Rup End
		--Rup Start
		,Av_CGST_AMT=SUM(case when BillTran > 0 then  icgst_amt else 0 end)
		,Av_sGST_AMT=SUM(case when BillTran > 0 then  isGST_AMT else 0 end)
		,Av_iGST_AMT=SUM(case when BillTran > 0 then  iigst_amt else 0 end)
		,av_CESS_AMT = SUM(case when BillTran > 0 then  ICESS_AMT else 0 end)
		--Rup End
		,supp_type='Bill of Entry'
		,gro_amt=SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then TaxableAmt else -TaxableAmt end)
		,hsncode
		,SBBILLNO=beno,SBDATE=bedt,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
		FROM #GSTR2TBL_RCM 
		where mENTRY IN ('EP','PT','CN','DN')
		and Isservice <> 'Services'
		AND ST_TYPE IN('Interstate','Intrastate') and SUPP_TYPE in('SEZ')
		AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT+CGST_AMT+SGST_AMT+IGST_AMT) > 0
		GROUP BY pinvno,pinvdt,Net_amt,Rate1,gstin,pos,gstype,oldgstin ,hsncode,beno,bedt,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
		 
		) bb ORDER BY gstin,pinvdt,pinvno,Rate1 
		--Commented by Shrikant S. on 08/11/2019 for AU 2.2.2		--End */

		--Added by Shrikant S. on 08/11/2019 for AU 2.2.2			--Start
		Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,supp_type,gro_amt,hsncode,SBBILLNO,SBDATE
		,Entry_Ty,Tran_Cd,Ac_Name--Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		,xPara  --Added by Priyanka B on 12022020 for Bug-33268
		)
		select * from (
		Select 3 as part,'3K' as partsr ,'K' srno
		,pinvno=Case when mENTRY IN ('PR','CN','DN') then Inv_no else pinvno end 
		,pinvdt=Case when mENTRY IN ('PR','CN','DN') then Date else pinvdt end
		,Net_amt,Rate1,
		SUM(TaxableAmt)TaxableAmt 
		,SUM(CGSRT_AMT+CGST_AMT)CGSRT_AMT
		,SUM(SGSRT_AMT+SGST_AMT)SGSRT_AMT
		,SUM(IGSRT_AMT+IGST_AMT)IGSRT_AMT
		,sum(CessRt_amt+CESS_AMT) as CessRt_amt
		,gstin,pos,gstype
		--Commented by Rup Start
		--,Av_CGST_AMT=0
		--,Av_sGST_AMT=0
		--,Av_iGST_AMT=0
		--,av_CESS_AMT =0
		--Commented by Rup End
		--Rup Start
		,Av_CGST_AMT=SUM(case when BillTran > 0 then  icgst_amt else 0 end)
		,Av_sGST_AMT=SUM(case when BillTran > 0 then  isGST_AMT else 0 end)
		,Av_iGST_AMT=SUM(case when BillTran > 0 then  iigst_amt else 0 end)
		,av_CESS_AMT = SUM(case when BillTran > 0 then  ICESS_AMT else 0 end)
		--Rup End
		,supp_type='Bill of Entry'
		,gro_amt=SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then gro_amt else -gro_amt end)
		,hsncode
		,SBBILLNO=beno,SBDATE=bedt,Entry_Ty,Tran_Cd,Cons_ac_name--Rup
		,mEntry  --Added by Priyanka B on 19112019 for Bug-32948
		,xPara=case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
		FROM #GSTR2TBL_RCM 
		where mENTRY IN ('EP','PT','CN','DN')
		and Isservice <> 'Services'
		AND ST_TYPE IN('Interstate','Intrastate') and SUPP_TYPE in('SEZ')
		--AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT+CGST_AMT+SGST_AMT+IGST_AMT) > 0		--Commented by Shrikant S. on 11/11/2019 for AU 2.2.2
		GROUP BY pinvno,pinvdt,inv_no,date,Net_amt,Rate1,gstin,pos,gstype,oldgstin ,hsncode,beno,bedt,Entry_Ty,Tran_Cd,Cons_ac_name,mENTRY--Rup
		 
		) bb ORDER BY gstin,pinvdt,pinvno,Rate1 
		--Added by Shrikant S. on 08/11/2019 for AU 2.2.2			--End



		IF NOT EXISTS(SELECT PART FROM #SHANX1 WHERE PARTSR = '3K' AND SRNO ='K') 
		BEGIN
			Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(3,'3K','K','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End
	/*	Missing documents on which credit has been claimed in T-2 /T-1 (for quarter) tax period and supplier has not reported the same till the filing of return for the current tax period	*/
	IF  @XtraPara Not in ('SAHAJ','SUGAM')
	Begin
		IF NOT EXISTS(SELECT PART FROM #SHANX1 WHERE PARTSR = '3L' AND SRNO ='L') 
		BEGIN
			Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(3,'3L','L','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End

		
	IF  @XtraPara Not in ('SAHAJ','SUGAM')
	Begin
		Insert Into #SHANX1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,ecomgstin,Av_iGST_AMT,Av_CGST_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
		Select * 
		from (
				select 4 AS PART ,'4' AS PARTSR,'' AS SRNO,'' as gstin1,'' as inv_no,'' as date,'' as location
				,GRO_AMT=sum((case when mEntry in('ST','SB','DN') THEN GRO_AMT ELSE -GRO_AMT END ))
				,rate1=0
				,taxableamt = sum((case when mEntry in('ST','SB','DN') THEN +(taxableamt)ELSE - (taxableamt) END ))
				,CGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(CGST_AMT1) ELSE - (CGST_AMT1) END ))
				,SGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(SGST_AMT1) ELSE - (SGST_AMT1) END ))
				,IGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(IGST_AMT1) ELSE - (IGST_AMT1) END ))
				,cess_amt = sum((case when mEntry in('ST','SB','DN') THEN +(cess_amt1) ELSE - (cess_amt1) END ))
				,'' as AC_NAME,'' as SUPP_TYPE,'' as ST_TYPE,Ecomgstin
				,Av_iGST_AMT=sum((case when mEntry in('ST','SB','DN') THEN GRO_AMT ELSE 0 END ))
				,Av_CGST_AMT=sum((case when mEntry in('CN','SR') THEN GRO_AMT ELSE 0 END ))
				,xPara=case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
				from #SHANX1TBL 
				where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type in ('Intrastate','Interstate','Out of Country') 
				and ecomgstin<>''
				group by Ecomgstin
			)aa	order by Ecomgstin

		IF NOT EXISTS(SELECT PART FROM #SHANX1 WHERE PARTSR = '4' AND SRNO ='') 
		BEGIN
			Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(4,'4','','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' when @xtrapara='SAHAJ' then 'SAHAJ' when @xtrapara='SUGAM' then 'SUGAM' end  --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End

	--Rup Start
	If (@Zoomin='Yes')
	Begin
		/*Part - 3 Party,Rate and Supply Type wise  SrNo=1 */
		Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,supp_type,gro_amt,hsncode,SBBILLNO,SBDATE,Entry_Ty,Tran_Cd,Ac_Name)
		Select PART,PARTSR,SRNO='1',INV_NO='',DATE='',net_amt=0,RATE,sum(Taxableamt),
		sum(CGST_AMT),sum(SGST_AMT),sum(IGST_AMT),sum(CESS_AMT),gstin,location,Inputtype,sum(Av_CGST_AMT),sum(Av_sGST_AMT),sum(Av_iGST_AMT),sum(av_CESS_AMT),supp_type,sum(gro_amt),hsncode,SBBILLNO,SBDATE,Entry_Ty='',Tran_Cd=0,Ac_Name		
		From #SHANX1 Where Part='3' and Taxableamt>0
		Group by PART,PARTSR,RATE,gstin,location,Inputtype,supp_type,hsncode,SBBILLNO,SBDATE,Ac_Name

		/*Part - 3 PartSr wise SrNo=3 */
		Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype
		--,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT  --Commented by Priyanka B on 19112019 for Bug-32948
		,supp_type,gro_amt,hsncode,SBBILLNO,SBDATE,Entry_Ty,Tran_Cd,Ac_Name)
		--Commented by Priyanka B on 19112019 for Bug-32948 Start
		/*Select PART,PARTSR,SRNO='3',INV_NO='',DATE='',net_amt=0,RATE=0,sum(Taxableamt),
		sum(CGST_AMT),sum(SGST_AMT),sum(IGST_AMT),sum(CESS_AMT),gstin='',location='',Inputtype='',sum(Av_CGST_AMT),sum(Av_sGST_AMT),sum(Av_iGST_AMT),sum(av_CESS_AMT),supp_type='',sum(gro_amt),hsncode='',SBBILLNO='',SBDATE='',Entry_Ty='',Tran_Cd=0,Ac_Name=''		
		From #SHANX1 Where Part='3' and SRNO<>'1'
		Group by PART,PARTSR*/
		--Commented by Priyanka B on 19112019 for Bug-32948 End

		--Modified by Priyanka B on 19112019 for Bug-32948 Start
		Select PART,PARTSR,SRNO='3',INV_NO='',DATE='',net_amt=0,RATE=0
		,taxableamt = (case when partsr='3A' then sum((case when mEntry in('ST','SB','DN') THEN +(taxableamt)ELSE - (taxableamt) END )) 
						when partsr in ('3H','3I') then sum((case when mEntry in('PT','EP','CN','BP') THEN +(taxableamt)ELSE - (taxableamt) END ))
					else sum(taxableamt) end)
		,CGST_AMT = (case when partsr='3A' then sum((case when mEntry in('ST','SB','DN') THEN +(CGST_AMT) ELSE - (CGST_AMT) END )) 
						when partsr in ('3H','3I') then sum((case when mEntry in('PT','EP','CN','BP') THEN +(CGST_AMT)ELSE - (CGST_AMT) END ))
					else sum(CGST_AMT) end)
		,SGST_AMT = (case when partsr='3A' then sum((case when mEntry in('ST','SB','DN') THEN +(SGST_AMT) ELSE - (SGST_AMT) END )) 
						when partsr in ('3H','3I') then sum((case when mEntry in('PT','EP','CN','BP') THEN +(SGST_AMT)ELSE - (SGST_AMT) END ))
					else sum(SGST_AMT) end)
		,IGST_AMT = (case when partsr='3A' then sum((case when mEntry in('ST','SB','DN') THEN +(IGST_AMT) ELSE - (IGST_AMT) END )) 
						when partsr in ('3H','3I') then sum((case when mEntry in('PT','EP','CN','BP') THEN +(IGST_AMT)ELSE - (IGST_AMT) END ))
					else sum(IGST_AMT) end)
		,cess_amt = (case when partsr='3A' then sum((case when mEntry in('ST','SB','DN') THEN +(cess_amt) ELSE - (cess_amt) END )) 
						when partsr in ('3H','3I') then sum((case when mEntry in('PT','EP','CN','BP') THEN +(cess_amt)ELSE - (cess_amt) END ))
					else sum(cess_amt) end)	
		,gstin='',location='',Inputtype=''
		,supp_type='',sum(gro_amt),hsncode='',SBBILLNO='',SBDATE='',Entry_Ty='',Tran_Cd=0,Ac_Name=''		
		From #SHANX1 Where Part='3' and SRNO<>'1'
		Group by PART,PARTSR
		--Modified by Priyanka B on 19112019 for Bug-32948 End		

		/*Part - 4 Party,Supply Type wise  SrNo=1 */
		Insert into #SHANX1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,ecomgstin,Av_iGST_AMT,Av_CGST_AMT,Entry_TY,Tran_Cd)
		Select * 
		from (
				select 4 AS PART ,'4' AS PARTSR,'1' AS SRNO,gstin,inv_no,date,'' as location
				,GRO_AMT=sum((case when mEntry in('ST','SB','DN') THEN GRO_AMT ELSE -GRO_AMT END ))
				,rate1=0
				,taxableamt = sum((case when mEntry in('ST','SB','DN') THEN +(taxableamt)ELSE - (taxableamt) END ))
				,CGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(CGST_AMT1) ELSE - (CGST_AMT1) END ))
				,SGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(SGST_AMT1) ELSE - (SGST_AMT1) END ))
				,IGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(IGST_AMT1) ELSE - (IGST_AMT1) END ))
				,cess_amt = sum((case when mEntry in('ST','SB','DN') THEN +(cess_amt1) ELSE - (cess_amt1) END ))
				,Cons_ac_name,SUPP_TYPE,ST_TYPE,Ecomgstin
				,Av_iGST_AMT=sum((case when mEntry in('ST','SB','DN') THEN GRO_AMT ELSE 0 END ))
				,Av_CGST_AMT=sum((case when mEntry in('CN','SR') THEN GRO_AMT ELSE 0 END ))
				,Entry_TY,Tran_Cd
				from #SHANX1TBL 
				where (mEntry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type in ('Intrastate','Interstate','Out of Country') 
				and ecomgstin<>''
				group by gstin,Cons_ac_name,SUPP_TYPE,ST_TYPE,Ecomgstin,Entry_TY,Tran_Cd,Inv_No,[Date]
			)aa	order by Ecomgstin


		/*Part - 4 Summary SrNo=3*/
		Insert into #SHANX1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,supp_type,gro_amt,hsncode,SBBILLNO,SBDATE,Entry_Ty,Tran_Cd,Ac_Name)
		Select PART,PARTSR,SRNO='3',INV_NO='',DATE='',net_amt=0,RATE=0,sum(Taxableamt),
		sum(CGST_AMT),sum(SGST_AMT),sum(IGST_AMT),sum(CESS_AMT),gstin='',location='',Inputtype='',sum(Av_CGST_AMT),sum(Av_sGST_AMT),sum(Av_iGST_AMT),sum(av_CESS_AMT),supp_type='',sum(gro_amt),hsncode='',SBBILLNO='',SBDATE='',Entry_Ty='',Tran_Cd=0,Ac_Name=''		
		From #SHANX1 Where Part='4' and SRNO<>'1'
		Group by PART,PARTSR
	End


	SELECT Descr=(Case 
					When PartSr='3A' and SrNo='3' Then '3A. Supplies made to consumers and un-registered persons (Net of debit / credit notes )'
					When PartSr='3B' and SrNo='3' Then '3B. Supplies made to registered persons (other than those attracting reverse charge)(including edit/amendment)'
					When PartSr='3C' and SrNo='3' Then '3C.Exports with payment of tax'
					When PartSr='3D' and SrNo='3' Then '3D. Exports without payment of tax'
					When PartSr='3E' and SrNo='3' Then '3E. Supplies to SEZ units/developers with payment of tax (including edit/amendment)'
					When PartSr='3F' and SrNo='3' Then '3F. Supplies to SEZ units/developers without payment of tax (including edit/amendment)'
					When PartSr='3G' and SrNo='3' Then '3G. Deemed exports (including edit/amendment)'
					When PartSr='3H' and SrNo='3' Then '3H. Inward supplies attracting reverse charge (to be reported by the recipient, GSTIN wise for every supplier, net of debit / credit notes and advances paid, if any) '
					When PartSr='3I' and SrNo='3' Then '3I. Import of services (net of debit/ credit notes and advances paid, if any)'
					When PartSr='3J' and SrNo='3' Then '3J. Import of goods'
					When PartSr='3K' and SrNo='3' Then '3K. Import of goods from SEZ units / developers on a Bill of Entry'
					When PartSr='3L' and SrNo='3' Then '3L. Missing documents on which credit has been claimed in T-2 /T-1 (for quarter) tax period and supplier has not reported the same till the filing of return for the current tax period'
					When PartSr='4' and SrNo='3' Then '4. Details of the supplies made through e-commerce operators liable to collect tax under section 52 (out of any outward supplies declared in table 3)' 
					Else '' End)
	,* 
	FROM #SHANX1
	order by PART,PARTSR,TableRowId 
end
--Rup End