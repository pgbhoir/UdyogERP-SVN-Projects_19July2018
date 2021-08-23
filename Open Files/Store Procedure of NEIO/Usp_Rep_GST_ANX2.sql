If Exists(Select [Name] From SysObjects Where xType='P' and [Name]='Usp_Rep_GST_ANX2')
Begin
	Drop procedure Usp_Rep_GST_ANX2
End
Go
Create Procedure [dbo].[Usp_Rep_GST_ANX2]
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
SELECT  TableRowId=IDENTITY(INT,1,1),PART=0,PARTSR='AAAA',SRNO= SPACE(2),INV_NO=SPACE(40), H.DATE,ORG_INVNO=SPACE(40)
	, H.DATE AS ORG_DATE, D.QTY, d.u_asseamt AS Taxableamt, d.CGST_PER AS RATE, d.CGST_PER, D.CGST_AMT, d.SGST_PER, D.SGST_AMT
	, d.IGST_PER,D.IGST_AMT,D.IGST_AMT as Cess_Amt,D.IGST_AMT as Cessr_Amt	,D.GRO_AMT, IT.IT_NAME
	, cast(IT.IT_DESC as varchar(250)) as IT_DESC , Isservice=SPACE(150), IT.HSNCODE
	,HSN_DESC = IT.SERTY
	,ac_name = cast((CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.Mailname, '') ELSE isnull(ac.ac_name, '') END) as varchar(150))
	, gstin = space(30), location = (CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.state, '') ELSE isnull(ac.state, '') END)
	,SUPP_TYPE = SPACE(100),st_type= SPACE(100),StateCode=space(5),Ecomgstin =space(30),from_srno =space(30),to_srno =space(30)
	,ORG_GSTIN =space(30) ,SBBILLNO=space(50),SBDATE=H.DATE,rptmonth=SPACE(15),rptyear =SPACE(15) ,Amenddate
	,Inputtype=space(80),Av_CGST_AMT=D.IGST_AMT,Av_sGST_AMT=D.IGST_AMT,Av_iGST_AMT=D.IGST_AMT,av_CESS_AMT=D.IGST_AMT,net_amt=D.GRO_AMT
	,xPara=cast('' as varchar(20)) --Added by Priyanka B on 12022020 for Bug-33268
	into #SHANX2
	FROM  STMAIN H 
	INNER JOIN STITEM D ON (H.ENTRY_TY = D .ENTRY_TY AND H.TRAN_CD = D .TRAN_CD) 
	INNER JOIN IT_MAST IT ON (D .IT_CODE = IT.IT_CODE) 
	LEFT OUTER JOIN shipto ON (shipto.shipto_id = h.scons_id) 
	LEFT OUTER JOIN ac_mast ac ON (h.cons_id = ac.ac_id)  
	WHERE 1=2


/* 3A.  Inward supplies received from a registered person (other than the supplies attracting reverse charge) */
	SELECT mEntry=case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end,
	RATE1= (case when (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0)) >0  then (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0))when (isnull(A.IGST_PER,0)) > 0 then (isnull(A.IGST_PER,0))else isnull(A.gstrate,0) end)
		,iigst_amt=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((ISNULL(a.igst_amt,0)+ ISNULL(a.IGSRT_AMT,0))-ISNULL(b.iigst_amt,0)) else 0 end)
		,icgst_amt=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((ISNULL(a.cgst_amt,0) + ISNULL(a.CGSRT_AMT,0)) - ISNULL(b.icgst_amt,0)) else 0 end)
		,isGST_AMT=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((isnull(a.sGST_AMT,0)+ isnull(a.SGSRT_AMT,0))-isnull(b.isGST_AMT,0)) else 0 end)
		,ICESS_AMT =(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((isnull(a.CESS_AMT,0)+ isnull(a.CessRT_amt,0)) - isnull(B.ICESS_AMT,0)) else 0 end)
		,A.AGAINSTGS,A.AmendDate,A.AVL_ITC,A.BEDT,A.beno,A.Cess_amt,A.Cess_per,A.CessRT_amt,A.CGSRT_AMT,A.CGST_AMT,A.CGST_PER
		,A.DATE,A.Entry_ty,A.GRO_AMT,A.GSTIN,A.gstrate,A.gstype,A.HSNCODE,A.IGSRT_AMT,A.IGST_AMT,A.IGST_PER,A.INV_NO
		,A.inv_sr,A.Isservice,A.IT_CODE,A.IT_NAME,A.ITSERIAL,A.l_yn,A.LineRule,A.net_amt,A.OLDGSTIN,A.ORG_DATE,A.ORG_INVNO,A.orgbedt,A.orgbeno,A.pinvdt,A.pinvno
		,A.portcode,A.pos,A.pos_std_cd,A.QTY,A.RevCharge,A.SGSRT_AMT,A.SGST_AMT
		,A.SGST_PER,A.ST_TYPE,A.SUPP_TYPE,A.Taxableamt,A.Tran_cd,A.TRANSTATUS,A.uqc,A.cons_ac_name
		INTO #GSTR2TBL 
		FROM GSTR2_VW A  
		LEFT OUTER JOIN Epayment B ON (A.Tran_cd =B.tran_cd AND A.Entry_ty =B.entry_ty AND A.ITSERIAL =B.itserial)  
		inner join lcode l  on (a.entry_ty=l.entry_ty)
		WHERE (A.DATE BETWEEN @SDATE AND @EDATE) and A.AmendDate=''

	
		--select * From #GSTR2TBL
		--Added by Priyanka B on 18022020 for Bug-32860 Start
		IF  @XtraPara Not in ('SAHAJ','SUGAM')
		Begin
		--Added by Priyanka B on 18022020 for Bug-32860 End
			Insert into #SHANX2(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,ac_name,supp_type,gro_amt,hsncode
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			Select 3 as part,'3A' as partsr ,'A' srno,pinvno,pinvdt,Net_amt,Rate1
			,SUM(TaxableAmt)TaxableAmt
			,SUM(CGST_AMT)CGST_AMT,SUM(SGST_AMT)SGST_AMT,SUM(IGST_AMT)IGST_AMT,SUM(Cess_amt)Cess_amt
			,gstin,pos,gstype
			,Av_CGST_AMT=SUM(icgst_amt)
			,Av_sGST_AMT=SUM(isGST_AMT)
			,Av_iGST_AMT=SUM(iigst_amt)
			,av_CESS_AMT = SUM(ICESS_AMT)
			,cons_ac_name
			,supp_type='Bill of Supply'
			,gro_amt=sum(gro_amt)
			,hsncode
			,xPara=case when @xtrapara='' then 'NORMAL' else @xtrapara end  --Added by Priyanka B on 12022020 for Bug-33268
			FROM #GSTR2TBL 
			where mENTRY IN ('EP','PT') 
			--AND ST_TYPE <> 'Out of Country' and SUPP_TYPE in('Registered','E-commerce','SEZ')		--Commented by Shrikant S. on 08/11/2019 for AU 2.2.2
			--AND ST_TYPE <> 'Out of Country' and ( SUPP_TYPE in('Registered','E-commerce','Compounding')	or (SUPP_TYPE ='SEZ' AND ISSERVICE='SERVICES'))	--Commented by Shrikant S. on 11/11/2019 for AU 2.2.2		
			AND ST_TYPE <> 'Out of Country' and ( SUPP_TYPE in('Registered','E-commerce')	or (SUPP_TYPE ='SEZ' AND ISSERVICE='SERVICES'))	--Added by Shrikant S. on 08/11/2019 for AU 2.2.2
			and ltrim(rtrim(HSNCODE)) <>'' 
			And gstin not in('Unregistered') 
			AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) = 0										--Added by Shrikant S. on 11/11/2019 for AU 2.2.2
			--AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) = 0  AND (CGST_AMT + SGST_AMT + IGST_AMT) > 0  AND LineRule = 'Taxable'			--Commented by Shrikant S. on 08/11/2019 for AU 2.2.2
			GROUP BY pinvno,pinvdt,Net_amt,Rate1,gstin,pos,gstype,cons_ac_name,oldgstin,hsncode
			ORDER BY gstin,pinvdt,pinvno,Rate1  
		--Added by Priyanka B on 18022020 for Bug-32860 Start
		End 
		Else
		Begin
			Insert into #SHANX2(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,ac_name,supp_type,gro_amt,hsncode
			,xPara
			)
			Select 3 as part,'3A' as partsr ,'A' srno,pinvno,pinvdt,Net_amt,Rate1
			,SUM(TaxableAmt)TaxableAmt
			,SUM(CGST_AMT)CGST_AMT,SUM(SGST_AMT)SGST_AMT,SUM(IGST_AMT)IGST_AMT,SUM(Cess_amt)Cess_amt
			,gstin,pos,gstype
			,Av_CGST_AMT=SUM(icgst_amt)
			,Av_sGST_AMT=SUM(isGST_AMT)
			,Av_iGST_AMT=SUM(iigst_amt)
			,av_CESS_AMT = SUM(ICESS_AMT)
			,cons_ac_name
			,supp_type='Bill of Supply'
			,gro_amt=sum(gro_amt)
			,hsncode
			,xPara=case when @xtrapara='' then 'NORMAL' else @xtrapara end
			FROM #GSTR2TBL 
			where mENTRY IN ('EP','PT') 
			AND ST_TYPE <> 'Out of Country' and  SUPP_TYPE in('Registered','E-commerce')
			and ltrim(rtrim(HSNCODE)) <>'' 
			And gstin not in('Unregistered') 
			AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) = 0			
			GROUP BY pinvno,pinvdt,Net_amt,Rate1,gstin,pos,gstype,cons_ac_name,oldgstin,hsncode
			ORDER BY gstin,pinvdt,pinvno,Rate1  
		End
		--Added by Priyanka B on 18022020 for Bug-32860 End

	IF NOT EXISTS(SELECT PART FROM #SHANX2 WHERE PARTSR = '3A' AND SRNO ='A')
	BEGIN
		Insert into #SHANX2(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
		,xPara  --Added by Priyanka B on 12022020 for Bug-33268
		)
		values(3,'3A','A','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
		,case when @xtrapara='' then 'NORMAL' else @xtrapara end   --Added by Priyanka B on 12022020 for Bug-33268
		)
	END

	/*	3B. Import of goods from SEZ units / developers on Bill of Entry */
	IF  @XtraPara Not in ('SAHAJ','SUGAM')
	Begin

		Insert into #SHANX2(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,ac_name,supp_type,gro_amt,hsncode
		,xPara  --Added by Priyanka B on 12022020 for Bug-33268
		)
		Select 3 as part,'3B' as partsr ,'B' srno,beno=case when beno<>'' then beno else pinvno end,bedt=case when beno<>'' then bedt else pinvdt end ,Net_amt,Rate1
		--,SUM(TaxableAmt)TaxableAmt,SUM(CGSRT_AMT)CGST_AMT,SUM(SGSRT_AMT)SGST_AMT,SUM(IGSRT_AMT)IGST_AMT		--Commented by Shrikant S. on 11/11/2019 for AU 2.2.2
		,SUM(TaxableAmt)TaxableAmt,SUM(CGST_AMT)CGST_AMT,SUM(SGST_AMT)SGST_AMT,SUM(IGST_AMT)IGST_AMT			--Added by Shrikant S. on 11/11/2019 for AU 2.2.2
		,sum(Cess_amt) AS Cess_amt
		,gstin,pos,gstype
		,Av_CGST_AMT=SUM(icgst_amt)
		,Av_sGST_AMT=SUM(isGST_AMT)
		,Av_iGST_AMT=SUM(iigst_amt)
		,av_CESS_AMT = SUM(ICESS_AMT)
		,cons_ac_name
		,supp_type='Bill of Entry'
		,gro_amt=sum(gro_amt)
		,hsncode
		,xPara=case when @xtrapara='' then 'NORMAL' else @xtrapara end  --Added by Priyanka B on 12022020 for Bug-33268
		FROM #GSTR2TBL 
		where mENTRY IN ('EP','PT')
		AND ST_TYPE IN('Interstate','Intrastate') and SUPP_TYPE ='SEZ'
		AND ISSERVICE='GOODS'
		GROUP BY gstin,pinvdt,pinvno,Rate1,bedt,beno,Net_amt,pos,gstype,oldgstin,cons_ac_name ,portcode,hsncode 
		order by gstin,case when beno<>'' then bedt else pinvdt end,case when beno<>'' then beno else pinvno end,Rate1


		IF NOT EXISTS(SELECT PART FROM #SHANX2 WHERE PARTSR = '3B' AND SRNO ='B')
		BEGIN
			Insert into #SHANX2(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(3,'3B','B','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' else @xtrapara end   --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End

	/*		3C. Import of goods from overseas on Bill of Entry		*/
	IF  @XtraPara Not in ('SAHAJ','SUGAM')
	Begin
		Insert into #SHANX2(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,ac_name,supp_type,gro_amt,hsncode
		,xPara  --Added by Priyanka B on 12022020 for Bug-33268
		)
		Select 3 as part,'3C' as partsr ,'C' srno,beno=case when beno<>'' then beno else pinvno end,bedt=case when beno<>'' then bedt else pinvdt end
		,Net_amt,Rate1,SUM(TaxableAmt)TaxableAmt ,SUM(CGST_AMT)CGST_AMT,SUM(SGST_AMT)SGST_AMT,SUM(IGST_AMT)IGST_AMT
		,sum(Cess_amt)
		,gstin,pos,gstype
		,Av_CGST_AMT=SUM(icgst_amt)
		,Av_sGST_AMT=SUM(isGST_AMT)
		,Av_iGST_AMT=SUM(iigst_amt)
		,av_CESS_AMT = SUM(ICESS_AMT)
		,cons_ac_name
		,supp_type='Bill of Entry'
		,gro_amt=sum(gro_amt)
		,hsncode
		,xPara=case when @xtrapara='' then 'NORMAL' else @xtrapara end  --Added by Priyanka B on 12022020 for Bug-33268
		FROM #GSTR2TBL 
		where mENTRY IN ('EP','PT')
		AND ISSERVICE='GOODS'
		AND ((ST_TYPE ='Out of Country') OR (ST_TYPE IN('INTERSTATE','INTRASTATE') and SUPP_TYPE in('IMPORT','EOU')))
		Group by beno,pinvno,pinvdt,bedt,Net_amt,Rate1,gstin,pos,gstype,hsncode,cons_ac_name
		ORDER BY gstin,case when beno<>'' then bedt else pinvdt end,case when beno<>'' then beno else pinvno end,Rate1

		IF NOT EXISTS(SELECT PART FROM #SHANX2 WHERE PARTSR = '3C' AND SRNO ='C')
		BEGIN
			Insert into #SHANX2(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(3,'3C','C','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' else @xtrapara end   --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End

	/*	Credit on all documents which have been rejected(net of debit /credit notes)	*/
	IF NOT EXISTS(SELECT PART FROM #SHANX2 WHERE PARTSR = '4' AND SRNO ='A')
	BEGIN
		Insert into #SHANX2(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
		,xPara  --Added by Priyanka B on 12022020 for Bug-33268
		)
		values(4,'4','A','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
		,case when @xtrapara='' then 'NORMAL' else @xtrapara end  --Added by Priyanka B on 12022020 for Bug-33268
		)
	END

	/*	Credit on all documents which have been kept pending (net of debit /credit notes)	*/
	IF NOT EXISTS(SELECT PART FROM #SHANX2 WHERE PARTSR = '4' AND SRNO ='B')
	BEGIN
		Insert into #SHANX2(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
		,xPara  --Added by Priyanka B on 12022020 for Bug-33268
		)
		values(4,'4','B','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
		,case when @xtrapara='' then 'NORMAL' else @xtrapara end  --Added by Priyanka B on 12022020 for Bug-33268
		)
	END

	/*	Credit on all documents which have been accepted (including deemed accepted) (net of debit/credit notes)	*/
	IF NOT EXISTS(SELECT PART FROM #SHANX2 WHERE PARTSR = '4' AND SRNO ='C')
	BEGIN
		Insert into #SHANX2(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
		,xPara  --Added by Priyanka B on 12022020 for Bug-33268
		)
		values(4,'4','C','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
		,case when @xtrapara='' then 'NORMAL' else @xtrapara end  --Added by Priyanka B on 12022020 for Bug-33268
		)
	END


	/*	5. ISD credits received (eligible credit only)	*/
	IF  @XtraPara Not in ('SAHAJ','SUGAM')
	Begin
		
		Select 5 as part,'5' as partsr ,'' srno,pinvno,pinvdt,0.00 as Net_amt,0 as Rate1,SUM(TaxableAmt) as TaxableAmt 
			,SUM(CGST_AMT)as CGST_AMT,SUM(SGST_AMT) as SGST_AMT,SUM(IGST_AMT)as IGST_AMT,sum(Cess_Amt) as Cess_Amt
			,gstin,'' as location,'' as gstype
			,Av_CGST_AMT=SUM(icgst_amt)
			,Av_sGST_AMT=SUM(isGST_AMT)
			,Av_iGST_AMT=SUM(iigst_amt)
			,av_CESS_AMT = SUM(ICESS_AMT),'Invoice' AS SUPP_TYPE
			INTO #SEC5
			FROM #GSTR2TBL
			where ENTRY_TY ='J6'
			GROUP BY pinvno,pinvdt,gstin
			union all
			Select 5 as part,'5' as partsr ,'' srno
			,inv_no,[DATE]
			,0.00 as Net_amt,0 as Rate1,SUM(TaxableAmt) as TaxableAmt 
			,SUM(CGST_AMT)as CGST_AMT,SUM(SGST_AMT) as SGST_AMT,SUM(IGST_AMT)as IGST_AMT,sum(Cess_Amt) as Cess_Amt 
			,gstin,'' as location,'' as gstype
			,Av_CGST_AMT=SUM(icgst_amt)
			,Av_sGST_AMT=SUM(isGST_AMT)
			,Av_iGST_AMT=SUM(iigst_amt)
			,av_CESS_AMT =SUM(ICESS_AMT),'Credit Note' AS SUPP_TYPE
			FROM #GSTR2TBL 
			where ENTRY_TY ='J8'
			GROUP BY 
			INV_NO,[Date],gstin

			Insert into #SHANX2(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,SUPP_TYPE
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			SELECT part,partsr,srno
			,pinvno,pinvdt,Net_amt,Rate1,SUM(TaxableAmt) as TaxableAmt 
			,SUM(CGST_AMT)as CGST_AMT,SUM(SGST_AMT) as SGST_AMT,SUM(IGST_AMT)as IGST_AMT,sum(Cess_Amt) as Cess_Amt 
			,gstin,'' as location,'' as gstype
			,Av_CGST_AMT=0
			,Av_sGST_AMT=0
			,Av_iGST_AMT=0
			,av_CESS_AMT =0
			,SUPP_TYPE
			,xPara=case when @xtrapara='' then 'NORMAL' else @xtrapara end  --Added by Priyanka B on 12022020 for Bug-33268
			FROM #SEC5
			GROUP BY  part,partsr,srno,pinvno,pinvdt,Net_amt,Rate1,SUPP_TYPE,GSTIN
			order by gstin,pinvdt,pinvno

			drop table #SEC5
		IF NOT EXISTS(SELECT PART FROM #SHANX2 WHERE PARTSR = '5' AND SRNO ='')
		BEGIN
			Insert into #SHANX2(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,xPara  --Added by Priyanka B on 12022020 for Bug-33268
			)
			values(5,'5','','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0
			,case when @xtrapara='' then 'NORMAL' else @xtrapara end  --Added by Priyanka B on 12022020 for Bug-33268
			)
		END
	End

SELECT * FROM #SHANX2
	order by PART,PARTSR,TableRowId 
end