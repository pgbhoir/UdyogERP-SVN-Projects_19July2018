If Exists(Select [Name] From SysObjects Where xType='P' and [Name]='Usp_Rep_GST_RET01')
Begin
	Drop procedure Usp_Rep_GST_RET01
End
Go
CREATE Procedure [dbo].[Usp_Rep_GST_RET01]
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
SELECT  TableRowId =IDENTITY(INT,1,1),PART=0,PARTSR='AAAA',SRNO= SPACE(2),INV_NO=SPACE(40), H.DATE,ORG_INVNO=SPACE(40)
	, H.DATE AS ORG_DATE, D.QTY, d.u_asseamt AS Taxableamt, d.CGST_PER AS RATE, d.CGST_PER, D.CGST_AMT, d.SGST_PER, D.SGST_AMT
	, d.IGST_PER,D.IGST_AMT,D.IGST_AMT as Cess_Amt,D.IGST_AMT as Cessr_Amt	,D.GRO_AMT, IT.IT_NAME
	, cast(IT.IT_DESC as varchar(250)) as IT_DESC , Isservice=SPACE(150), IT.HSNCODE
	,HSN_DESC = IT.SERTY
	,ac_name = cast((CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.Mailname, '') ELSE isnull(ac.ac_name, '') END) as varchar(150))
	, gstin = space(30), location = (CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.state, '') ELSE isnull(ac.state, '') END)
	,SUPP_TYPE = SPACE(100),st_type= SPACE(100),StateCode=space(5),Ecomgstin =space(30),from_srno =space(30),to_srno =space(30)
	,ORG_GSTIN =space(30) ,SBBILLNO=space(50),SBDATE=H.DATE,rptmonth=SPACE(15),rptyear =SPACE(15) ,Amenddate
	,Inputtype=space(80),Av_CGST_AMT=D.IGST_AMT,Av_sGST_AMT=D.IGST_AMT,Av_iGST_AMT=D.IGST_AMT,av_CESS_AMT=D.IGST_AMT,net_amt=D.GRO_AMT,partDesc=Convert(varchar(1000),'')
	,inptStatus=Space(50)
	,Amount1=D.IGST_AMT,Amount2=D.IGST_AMT,Amount3=D.IGST_AMT,Amount4=D.IGST_AMT,Amount5=D.IGST_AMT
	into #GSTRET1
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
		,A.CGST_AMT,A.CGST_PER,A.DATE,A.ecomac_id,A.Entry_ty
		,A.EXPOTYPE,A.GRO_AMT,A.GSTIN,A.gstrate,A.hsncode,A.IGSRT_AMT,A.IGST_AMT,A.IGST_PER,A.INV_NO,A.inv_sr,A.Isservice,A.IT_CODE,A.IT_NAME,A.ITSERIAL,A.l_yn
		,A.LineRule,A.net_amt,A.ORG_DATE,A.ORG_INVNO,A.pos,A.pos_std_cd,A.QTY,A.RevCharge,A.SBBILLNO,A.SBDATE,A.SGSRT_AMT,A.SGST_AMT,A.SGST_PER,A.ST_TYPE
		,A.SUPP_TYPE,A.Taxableamt,A.Tran_cd,A.uqc
		,HSN_DESC=ISNULL((SELECT TOP 1 HSN_DESC FROM #HSN WHERE HSN_CODE = A.HSNCODE),'')
INTO #GSTRET1TBL 
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
		,A.AGAINSTGS,A.AmendDate,A.buyer_gstin,A.buyer_PANNO,A.buyer_pos,A.buyer_st_type,A.buyer_SUPP_TYPE,A.cess_amt,A.cessr_amt,A.cessrate,A.CGSRT_AMT
		,A.CGST_AMT,A.CGST_PER,A.DATE,A.ecomac_id,A.Entry_ty
		,A.EXPOTYPE,A.GRO_AMT,A.GSTIN,A.gstrate,A.hsncode,A.IGSRT_AMT,A.IGST_AMT,A.IGST_PER,A.INV_NO,A.inv_sr,A.Isservice,A.IT_CODE,A.IT_NAME,A.ITSERIAL,A.l_yn
		,A.LineRule,A.net_amt,A.ORG_DATE,A.ORG_INVNO,A.pos,A.pos_std_cd,A.QTY,A.RevCharge,A.SBBILLNO,A.SBDATE,A.SGSRT_AMT,A.SGST_AMT,A.SGST_PER,A.ST_TYPE
		,A.SUPP_TYPE,A.Taxableamt,A.Tran_cd,A.uqc
		,HSN_DESC=ISNULL((SELECT TOP 1 HSN_DESC FROM #HSN WHERE HSN_CODE = A.HSNCODE),'')
INTO #GSTR1AMD
FROM GSTR1_VW A
left join ac_mast ac on (A.ecomac_id=ac.ac_id)
inner join lcode l  on (a.entry_ty=l.entry_ty)
WHERE (A.AMENDDATE BETWEEN @SDATE AND @EDATE) AND A.HSNCODE <> ''

--insert into #GSTRET1TBL select * from #GSTR1AMD				--Added by Shrikant S. on 14/11/2019 for AU 2.2.2


--SELECT * FROM #GSTRET1TBL
/*3A.  Supplies made to consumers and un-registered persons (Net of debit / credit notes ) */
Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
	,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc)
Select *
from (
		select 3 AS PART ,'3A' AS PARTSR,'1' AS SRNO,gstin='','' as inv_no,0 as date,'' as location
		,GRO_AMT=sum((case when mEntry in('ST','SB','DN') THEN GRO_AMT else -GRO_AMT END)) ,0 AS rate1
		,taxableamt =sum((case when mEntry in('ST','SB','DN') THEN +(taxableamt)ELSE - (taxableamt) END ))
		,CGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(CGST_AMT1) ELSE - (CGST_AMT1) END ))
		,SGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(SGST_AMT1) ELSE - (SGST_AMT1) END ))
		,IGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(IGST_AMT1) ELSE - (IGST_AMT1) END ))
		,cess_amt = sum((case when mEntry in('ST','SB','DN') THEN +(cess_amt1) ELSE - (cess_amt1) END ))
		,'' as AC_NAME,''as SUPP_TYPE,'' as ST_TYPE,hsncode='',SBBILLNO='',SBDATE=0
		,partdesc='Taxable supplies made to consumers and unregistered persons (B2C) [table 3A of FORM GST ANX-1]' 
		from #GSTRET1TBL  
		--where (mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB')		--26/10/2019
		where (mEntry in ('ST','SB','SR') and entry_ty<>'UB')
		and supp_type IN ('UnRegistered','')  and st_type <> 'Out of country' 
		--and ecomgstin=''						--Commented by Shrikant S. on 11/11/2019 for AU 2.2.2
	) aa Order by GSTIN,DATE,INV_NO,rate1


IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3A' AND SRNO ='1')
BEGIN
	Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,location,inv_no,date,gro_amt,taxableamt
		,SGST_PER,SGST_AMT,CGST_PER,CGST_AMT,IGST_PER,IGST_AMT,AC_NAME,SUPP_TYPE,ST_TYPE,StateCode,cess_amt,partdesc)
	VALUES(3,'3A','1','','','','',0,0,0,0,0,0,0,0,'','','','',0,'Taxable supplies made to consumers and unregistered persons (B2C) [table 3A of FORM GST ANX-1]' )
END
else
begin
	Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
End

/*	3B. Supplies made to registered persons (other than those attracting reverse charge)(including edit/amendment)	*/


		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc)
		
	Select *
	from (
		SELECT 3 AS PART ,'3A' AS PARTSR,'2' AS SRNO,'' as gstin,inv_no='',date=0,POS=''
		,GRO_AMT=sum((case when mEntry in('ST','SB','DN') THEN GRO_AMT else -GRO_AMT END))
		,0 as rate1
		,taxableamt =sum((case when mEntry in('ST','SB','DN') THEN +(taxableamt)ELSE - (taxableamt) END ))
		,CGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(CGST_AMT1) ELSE - (CGST_AMT1) END ))
		,SGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(SGST_AMT1) ELSE - (SGST_AMT1) END ))
		,IGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(IGST_AMT1) ELSE - (IGST_AMT1) END ))
		,cess_amt = sum((case when mEntry in('ST','SB','DN') THEN +(cess_amt1) ELSE - (cess_amt1) END ))
		,'' as AC_NAME,''as SUPP_TYPE,'' as ST_TYPE,hsncode='',SBBILLNO='',SBDATE=0 
		,partdesc='Taxable supplies made to registered persons(other than those attracting reverse charge)(B2B) [table 3B of FORM GST ANX-1]' 
		from #GSTRET1TBL
		where (mEntry in ('ST','SB','SR') and entry_ty<>'UB') and st_type <> 'Out of country' 
		--where (mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type <> 'Out of country'		--26/10/2019
		and supp_type IN ('Registered','Compounding') 
		and gstin <>'' AND LineRule = 'Taxable' AND HSNCODE <> '' 
		--and Ecomgstin = ''			--26/10/2019
		and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) > 0

		) bb Order by GSTIN,DATE,INV_NO,rate1

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3A' AND SRNO ='2') 
		BEGIN
			Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,location,inv_no,date,gro_amt,taxableamt
			,SGST_PER,SGST_AMT,CGST_PER,CGST_AMT,IGST_PER,IGST_AMT,AC_NAME,SUPP_TYPE,ST_TYPE,StateCode,cess_amt,partdesc)
			VALUES(3,'3A','2','','','','',0,0,0,0,0,0,0,0,'','','','',0,'Taxable supplies made to registered persons(other than those attracting reverse charge)(B2B) [table 3B of FORM GST ANX-1]' )
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End



/*	3C. Exports with payment of tax	*/
		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc)

		SElect * From (
		SELECT 3 AS PART ,'3A' AS PARTSR,'3' AS SRNO,gstin='',inv_no='',date=0,POS=''
		,GRO_AMT=sum((case when mEntry in('ST','SB','DN') THEN GRO_AMT else -GRO_AMT END))
		,rate1=0
		,taxableamt =sum((case when mEntry in('ST','SB','DN') THEN +(taxableamt)ELSE - (taxableamt) END ))
		,CGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(CGST_AMT1) ELSE - (CGST_AMT1) END ))
		,SGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(SGST_AMT1) ELSE - (SGST_AMT1) END ))
		,IGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(IGST_AMT1) ELSE - (IGST_AMT1) END ))
		,cess_amt = sum((case when mEntry in('ST','SB','DN') THEN +(cess_amt1) ELSE - (cess_amt1) END ))
		,'' as AC_NAME,''as SUPP_TYPE,'' as ST_TYPE,hsncode='',SBBILLNO='',SBDATE=0
		,partdesc='Exports with payment of tax [table 3C of FORM GST ANX-1]' 
		from #GSTRET1TBL
		--where (mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type ='Out of country'		--26/10/2019
		where (mEntry in ('ST','SB','SR') and entry_ty<>'UB') and st_type ='Out of country' 
		and supp_type IN ('','Export')  
		--and ecomgstin=''			--26/10/2019
		and gstin ='' AND LineRule = 'Taxable' AND HSNCODE <> '' 
		and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) > 0
		) cc Order by GSTIN,DATE,INV_NO,rate1


		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3A' AND SRNO ='3') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(3,'3A','3','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Exports with payment of tax [table 3C of FORM GST ANX-1]' )
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

/*	3D. Exports without payment of tax	*/
		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc)
		select * from (
		SELECT 3 AS PART ,'3A' AS PARTSR,'4' AS SRNO,gstin='',inv_no='',date=0,POS=''
		,GRO_AMT=sum((case when mEntry in('ST','SB','DN') THEN GRO_AMT else -GRO_AMT END))
		,rate1=0
		,taxableamt =sum((case when mEntry in('ST','SB','DN') THEN +(taxableamt)ELSE - (taxableamt) END ))
		,CGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(CGST_AMT1) ELSE - (CGST_AMT1) END ))
		,SGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(SGST_AMT1) ELSE - (SGST_AMT1) END ))
		,IGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(IGST_AMT1) ELSE - (IGST_AMT1) END ))
		,cess_amt = sum((case when mEntry in('ST','SB','DN') THEN +(cess_amt1) ELSE - (cess_amt1) END ))
		,'' as AC_NAME,''as SUPP_TYPE,'' as ST_TYPE,hsncode='',SBBILLNO='',SBDATE=0 
		,partdesc='Exports without payment of tax [table 3D of FORM GST ANX-1]' 
		from #GSTRET1TBL
		--where (mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type ='Out of country'		--26/10/2019
		where (mEntry in ('ST','SB','SR') and entry_ty<>'UB') and st_type ='Out of country' 
		and supp_type IN ('') 
		and gstin ='' AND LineRule = 'Nil Rated' AND HSNCODE <> '' 
		--and ecomgstin=''						--Commented by Shrikant S. on 11/11/2019 for AU 2.2.2
		and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) = 0
		) dd  Order by GSTIN,DATE,INV_NO,rate1


		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3A' AND SRNO ='4') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(3,'3A','4','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Exports without payment of tax [table 3D of FORM GST ANX-1]' )
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

/*	3E. Supplies to SEZ units/developers with payment of tax (including edit/amendment)	*/
		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc)
		SELECT * FROM (
		SELECT 3 AS PART ,'3A' AS PARTSR,'5' AS SRNO,gstin='',inv_no='',date=0,POS=''
		,GRO_AMT=sum((case when mEntry in('ST','SB','DN') THEN GRO_AMT else -GRO_AMT END))
		,rate1=0
		,taxableamt =sum((case when mEntry in('ST','SB','DN') THEN +(taxableamt)ELSE - (taxableamt) END ))
		,CGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(CGST_AMT1) ELSE - (CGST_AMT1) END ))
		,SGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(SGST_AMT1) ELSE - (SGST_AMT1) END ))
		,IGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(IGST_AMT1) ELSE - (IGST_AMT1) END ))
		,cess_amt = sum((case when mEntry in('ST','SB','DN') THEN +(cess_amt1) ELSE - (cess_amt1) END ))
		,'' as AC_NAME,''as SUPP_TYPE,'' as ST_TYPE,hsncode='',SBBILLNO='',SBDATE=0
		,partdesc='Supplies to SEZ units/developers with payment of tax [table 3E of FORM GST ANX-1]' 
		from #GSTRET1TBL
		--where mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB'				--26/10/2019
		where mEntry in ('ST','SB','SR') and entry_ty<>'UB' 
		and supp_type IN ('SEZ') 
		AND LineRule = 'Taxable' AND HSNCODE <> '' and st_type iN('INTERSTATE','INTRASTATE') 
		and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) > 0

		) ee

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3A' AND SRNO ='5') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(3,'3A','5','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Supplies to SEZ units/developers with payment of tax [table 3E of FORM GST ANX-1]' )
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End	
	
/*	3F. Supplies to SEZ units/developers without payment of tax (including edit/amendment)	*/
		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc)
			SELECT * FROM (
			SELECT 3 AS PART ,'3A' AS PARTSR,'6' AS SRNO,gstin='',inv_no='',date=0,POS=''
			,GRO_AMT=sum((case when mEntry in('ST','SB','DN') THEN GRO_AMT else -GRO_AMT END))
			,rate1=0
			,taxableamt =sum((case when mEntry in('ST','SB','DN') THEN +(taxableamt)ELSE - (taxableamt) END ))
			,CGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(CGST_AMT1) ELSE - (CGST_AMT1) END ))
			,SGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(SGST_AMT1) ELSE - (SGST_AMT1) END ))
			,IGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(IGST_AMT1) ELSE - (IGST_AMT1) END ))
			,cess_amt = sum((case when mEntry in('ST','SB','DN') THEN +(cess_amt1) ELSE - (cess_amt1) END ))
			,'' as AC_NAME,''as SUPP_TYPE,'' as ST_TYPE,hsncode='',SBBILLNO='',SBDATE=0
			,partdesc='Supplies to SEZ units / developers without payment of tax [table 3F of FORM GST ANX-1]' 
			from #GSTRET1TBL
			--where ( mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB')		--26/10/2019
			where ( mEntry in ('ST','SB','SR') and entry_ty<>'UB') 
			and supp_type IN ('SEZ') 
			--and ecomgstin=''						--Commented by Shrikant S. on 11/11/2019 for AU 2.2.2
			AND LineRule = 'Nil Rated' AND HSNCODE <> '' and st_type iN('INTERSTATE','INTRASTATE') 
			and (SGSRT_AMT1 + CGSRT_AMT1  + IGSRT_AMT1 + cessr_amt1) = 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) = 0
			) ff

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3A' AND SRNO ='6') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(3,'3A','6','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Supplies to SEZ units / developers without payment of tax [table 3F of FORM GST ANX-1]')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

/*	3G. Deemed exports (including edit/amendment)	*/	
		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc)
			SELECT * FROM (
			SELECT 3 AS PART ,'3A' AS PARTSR,'7' AS SRNO,gstin='',inv_no='',date=0,POS=''
			,GRO_AMT=sum((case when mEntry in('ST','SB','DN') THEN GRO_AMT else -GRO_AMT END))
			,rate1=0
			,taxableamt =sum((case when mEntry in('ST','SB','DN') THEN +(taxableamt)ELSE - (taxableamt) END ))
			,CGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(CGST_AMT1) ELSE - (CGST_AMT1) END ))
			,SGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(SGST_AMT1) ELSE - (SGST_AMT1) END ))
			,IGST_AMT = sum((case when mEntry in('ST','SB','DN') THEN +(IGST_AMT1) ELSE - (IGST_AMT1) END ))
			,cess_amt = sum((case when mEntry in('ST','SB','DN') THEN +(cess_amt1) ELSE - (cess_amt1) END ))
			,'' as AC_NAME,''as SUPP_TYPE,'' as ST_TYPE,hsncode='',SBBILLNO='',SBDATE=0 
			,partdesc='Deemed exports [table 3G of FORM GST ANX-1]' 
			from #GSTRET1TBL
			--where ( mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB')			--26/10/2019
			where ( mEntry in ('ST','SB','SR') and entry_ty<>'UB')			
			and supp_type IN ('EOU') 
			) gg

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3A' AND SRNO ='7') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(3,'3A','7','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Deemed exports [table 3G of FORM GST ANX-1]' )
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End	

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
			sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
			,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='3A' AND SRNO='8'

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3A' AND SRNO ='8') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(3,'3A','8','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Liabilities relating to the period prior to the introduction of current return filing system and any other liability to be paid','U-3,4,5,6,7' )
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3A' AND SRNO ='9') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,partdesc,Location,inptStatus)
			SELECT 3,'3A','9',CGST_AMT=SUM(ISNULL(CGST_AMT,0)),SGST_AMT=SUM(ISNULL(SGST_AMT,0)),IGST_AMT=SUM(ISNULL(IGST_AMT,0)),CESS_AMT=SUM(ISNULL(CESS_AMT,0)),GRO_AMT=SUM(ISNULL(GRO_AMT,0))
			,'Sub-total (A) [sum of 1 to 8]','' as loc,'C'
			FROM #GSTRET1 
			wHERE PARTSR='3A'
		END


/* 3H. Inward supplies received from a registered supplier (attracting reverse charge) */
SELECT mEntry=case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end,
	RATE1= (case when (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0)) >0  then (isnull(A.CGST_PER,0)+isnull(A.SGST_PER,0))when (isnull(A.IGST_PER,0)) > 0 then (isnull(A.IGST_PER,0))else isnull(A.gstrate,0) end)
		,iigst_amt=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((ISNULL(a.igst_amt,0)+ ISNULL(a.IGSRT_AMT,0))-ISNULL(b.iigst_amt,0)) else 0 end)
		,icgst_amt=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((ISNULL(a.cgst_amt,0) + ISNULL(a.CGSRT_AMT,0)) - ISNULL(b.icgst_amt,0)) else 0 end)
		,isGST_AMT=(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((isnull(a.sGST_AMT,0)+ isnull(a.SGSRT_AMT,0))-isnull(b.isGST_AMT,0)) else 0 end)
		,ICESS_AMT =(case when gstype <> 'Ineligible for ITC' and avl_itc = 1 then ((isnull(a.CESS_AMT,0)+ isnull(a.CessRT_amt,0)) - isnull(B.ICESS_AMT,0)) else 0 end)
		,A.AGAINSTGS,A.AmendDate,A.AVL_ITC,A.BEDT,A.beno,A.Cess_amt,A.Cess_per,A.CessRT_amt,A.CGSRT_AMT,A.CGST_AMT,A.CGST_PER,A.Cons_ac_name,A.Cons_gstin,A.Cons_PANNO
		,A.Cons_pos,A.Cons_st_type,A.Cons_SUPP_TYPE,A.DATE,A.Entry_ty,A.GRO_AMT,A.GSTIN,A.gstrate,A.gstype,A.HSNCODE,A.IGSRT_AMT,A.IGST_AMT,A.IGST_PER,A.INV_NO
		,A.inv_sr,A.Isservice,A.IT_CODE,A.IT_NAME,A.ITSERIAL,A.l_yn,A.LineRule,A.net_amt,A.OLDGSTIN,A.ORG_DATE,A.ORG_INVNO,A.orgbedt,A.orgbeno,A.pinvdt,A.pinvno
		,A.portcode,A.pos,A.pos_std_cd,A.QTY,A.RevCharge,A.seller_gstin,A.seller_PANNO,A.seller_pos,A.seller_st_type,A.seller_SUPP_TYPE,A.SGSRT_AMT,A.SGST_AMT
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
	group by  a.Tran_cd,a.Entry_ty,a.Main_tran,a.ENTRY_ALL

	select A.* into #GSTR2TBL_RCM from  #GSTR2TBL A 

	select Entry_ty=b.Entry_all,Tran_cd=b.Main_tran,cgsrt_amt=sum(cgsrt_amt),sgsrt_amt=sum(sgsrt_amt),igsrt_amt=sum(igsrt_amt),CessRT_amt=sum(CessRT_amt) 
		,TaxableAmt=sum(TaxableAmt),GRO_AMT=sum(GRO_AMT)
		into #tblbill   from #GSTR2TBL a
		Inner Join #rcmbill b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.tran_Cd)
		Group by b.Entry_all,b.Main_tran


	update #GSTR2TBL_RCM set cgsrt_amt=b.cgsrt_amt - a.cgsrt_amt,sgsrt_amt=b.sgsrt_amt - a.sgsrt_amt ,igsrt_amt=b.igsrt_amt - a.igsrt_amt  
		,CessRT_amt=b.CessRT_amt-a.CessRT_amt,Taxableamt=b.TaxableAmt-a.TaxableAmt,Gro_amt=b.GRO_AMT-a.Gro_amt
		from #tblbill a Inner Join #GSTR2TBL_RCM b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.Tran_cd)
	
	
	Delete from #GSTR2TBL_RCM where cgsrt_amt=0 and sgsrt_amt=0 and igsrt_amt=0 and CessRT_amt=0 and Taxableamt=0 and GRO_AMT=0 and Entry_ty='BP'	 

	Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
	CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,gro_amt,partdesc)

	select * from (
	Select 3 as part,'3B' as partsr ,'1' srno,pinvno='',pinvdt=0,Net_amt=0,Rate1=0
	,TaxableAmt = SUM(CASE WHEN mENTRY IN ('EP','PT','CN','BP') THEN TaxableAmt ELSE -TaxableAmt END)
	,CGSRT_AMT=SUM(CASE WHEN mENTRY IN ('EP','PT','CN','BP') THEN CGSRT_AMT ELSE -CGSRT_AMT END) 
	,SGSRT_AMT=SUM(CASE WHEN mENTRY IN ('EP','PT','CN','BP') THEN SGSRT_AMT ELSE -SGSRT_AMT END)
	,IGSRT_AMT=SUM(CASE WHEN mENTRY IN ('EP','PT','CN','BP') THEN IGSRT_AMT ELSE -IGSRT_AMT END)
	,CessRt_Amt=sum(CASE WHEN mENTRY IN ('EP','PT','CN','BP') THEN CessRt_Amt ELSE -CessRt_Amt END) 
	,gstin=''
	,pos='',gstype=''
	,Av_CGST_AMT=0
	,Av_sGST_AMT=0
	,Av_iGST_AMT=0
	,av_CESS_AMT = 0
	,gro_amt=sum(CASE WHEN mENTRY IN ('EP','PT','CN','BP') THEN TaxableAmt ELSE -TaxableAmt END)
	,partdesc='Inward supplies attracting reverse charge (net of debit / credit notes and advances paid, if any) [table 3H of FORM GST ANX-1]'
	FROM #GSTR2TBL_RCM
	where mENTRY IN ('EP','PT','PR','CN','DN','BP')
	and SUPP_TYPE in ('Registered','Unregistered')
	--AND ST_TYPE <> 'Out of Country'
	--and SUPP_TYPE Not in ('SEZ')
	and ltrim(rtrim(HSNCODE)) <>'' 
	AND LineRule = 'Taxable'
	AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT > 0)
	) hh
	

	
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3B' AND SRNO ='1') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(3,'3B','1','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Inward supplies attracting reverse charge (net of debit / credit notes and advances paid, if any) [table 3H of FORM GST ANX-1]')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End	

		
		
	/*	3B. 2. Import of services (net of debit/ credit notes and advances paid, if any)		*/
		Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,gro_amt,partdesc)
		select * from 
		(
		Select 3 as part,'3B' as partsr ,'2' srno,pinvno='',pinvdt=0,Net_amt=0,Rate1=0,
		SUM(CASE WHEN mENTRY IN  ('EP','PT','CN','BP') then TaxableAmt else -TaxableAmt end)TaxableAmt 
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN','BP') then CGSRT_AMT else -CGSRT_AMT end)CGSRT_AMT
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN','BP') then SGSRT_AMT else -SGSRT_AMT end)SGSRT_AMT
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN','BP') then IGSRT_AMT else -IGSRT_AMT end)IGSRT_AMT
		,sum(CASE WHEN mENTRY IN  ('EP','PT','CN','BP') then CessRt_amt else -CessRt_amt end) as CessRt_amt
		,gstin='',pos='',gstype=''
		,Av_CGST_AMT=0
		,Av_sGST_AMT=0
		,Av_iGST_AMT=0
		,av_CESS_AMT = 0
		,gro_amt=SUM(CASE WHEN mENTRY IN  ('EP','PT','CN','BP') then TaxableAmt else -TaxableAmt end)
		,partdesc='Import of services (net of debit / credit notes and advances paid, if any) [table 3I of FORM GST ANX-1]'
		FROM #GSTR2TBL_RCM 
		where mENTRY IN ('EP','PT','PR','CN','DN','BP')
		and Isservice = 'Services'
		--AND ST_TYPE IN('Out of Country','Interstate','Intrastate') 
		AND ST_TYPE ='Out of Country'
		AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) > 0

		) bb

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3B' AND SRNO ='2') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(3,'3B','2','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Import of services (net of debit / credit notes and advances paid, if any) [table 3I of FORM GST ANX-1]')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		/*	3B. 3. Sub-total (B) [sum of 1 & 2]		*/
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3B' AND SRNO ='3') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,partdesc,Location,inptStatus)
			SELECT 3,'3B','3',CGST_AMT=SUM(ISNULL(CGST_AMT,0)),SGST_AMT=SUM(ISNULL(SGST_AMT,0)),IGST_AMT=SUM(ISNULL(IGST_AMT,0)),CESS_AMT=SUM(ISNULL(CESS_AMT,0)),GRO_AMT=SUM(ISNULL(GRO_AMT,0))
			,'Sub-total (B) [sum of 1 & 2]','' as loc,'C'
			FROM #GSTRET1 
			wHERE PARTSR='3B'
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End
		
		/*	3C. 1. Debit notes issued (FORM GST ANX-1) (Other than those attracting reverse charge)		*/
		Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,gro_amt,partdesc)
		select * from 
		(
		Select 3 as part,'3C' as partsr ,'1' srno,pinvno='',pinvdt=0,Net_amt=0,Rate1=0,
		SUM(TaxableAmt)TaxableAmt 
		,SUM(CGST_AMT)CGST_AMT
		,SUM(SGST_AMT)SGST_AMT
		,SUM(IGST_AMT)IGST_AMT,sum(CESS_AMT) as CESS_AMT
		,gstin='',pos='',gstype=''
		,Av_CGST_AMT=0
		,Av_sGST_AMT=0
		,Av_iGST_AMT=0
		,av_CESS_AMT = 0
		,gro_amt=sum(gro_amt)
		,partdesc='Debit notes issued (FORM GST ANX-1)(Other than those attracting reverse charge)'
		FROM #GSTRET1TBL 
		where mENTRY='DN'
		AND ST_TYPE IN('Out of Country','Interstate','Intrastate') 
		AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) <= 0

		) bb


		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3C' AND SRNO ='1') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(3,'3C','1','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Debit notes issued (FORM GST ANX-1)(Other than those attracting reverse charge)')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		/* 3C. 2.	Credit notes issued (FORM GST ANX-1)(Other than those attracting reverse charge)	*/
		Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,gro_amt,partdesc)
		select * from 
		(
		Select 3 as part,'3C' as partsr ,'2' srno,pinvno='',pinvdt=0,Net_amt=0,Rate1=0,
		SUM(TaxableAmt)TaxableAmt 
		,SUM(CGST_AMT)CGST_AMT
		,SUM(SGST_AMT)SGST_AMT
		,SUM(IGST_AMT)IGST_AMT,sum(CESS_AMT) as CESS_AMT
		,gstin='',pos='',gstype=''
		,Av_CGST_AMT=0
		,Av_sGST_AMT=0
		,Av_iGST_AMT=0
		,av_CESS_AMT = 0
		,gro_amt=sum(gro_amt)
		,partdesc='Credit notes issued (FORM GST ANX-1)(Other than those attracting reverse charge)'
		FROM #GSTRET1TBL 
		where mENTRY='CN'
		AND ST_TYPE IN('Out of Country','Interstate','Intrastate') 
		AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) <= 0
		 
		) bb

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3C' AND SRNO ='2') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(3,'3C','2','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Credit notes issued (FORM GST ANX-1)(Other than those attracting reverse charge)')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		/* 3C. 3.	Advances received (net of refund vouchers and including adjustments on account of wrong reporting of advances earlier)	*/
		if Exists(Select PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus='U-3,4,5,6,7' from GSTRET01 
				WHERE PARTSR = '3C' AND SRNO ='3')
		Begin
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
				CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
				Select PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
				CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus='U-3,4,5,6,7' from GSTRET01 
					WHERE PARTSR = '3C' AND SRNO ='3'
		End
		Else
		Begin
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,GRO_AMT,partdesc,inptStatus)
			select * from 
			(
			Select 3 as part,'3C' as partsr ,'3' srno,pinvno='',pinvdt=0,Net_amt=0,Rate1=0,
			SUM( TaxableAmt)TaxableAmt 
			,SUM(CGST_AMT)CGST_AMT
			,SUM(SGST_AMT)SGST_AMT
			,SUM(IGST_AMT)IGST_AMT,sum(CESS_AMT) as CESS_AMT
			,gstin='',pos='',gstype=''
			,Av_CGST_AMT=0
			,Av_sGST_AMT=0
			,Av_iGST_AMT=0
			,av_CESS_AMT = 0
			,GRO_AMT=SUM(GRO_AMT)
			,partdesc='Advances received (net of refund vouchers and including adjustments on account of wrong reporting of advances earlier)'
			,inptStatus='U-3,4,5,6,7'
			FROM #GSTRET1TBL 
			where mENTRY IN ('BR','RV')
			AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) <= 0

			) bb
		End
		
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3C' AND SRNO ='3') 
		BEGIN
			
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(3,'3C','3','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Advances received (net of refund vouchers and including adjustments on account of wrong reporting of advances earlier)','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		/* 3C. 4.	Advance adjusted		*/
		if Exists(Select PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus='U-3,4,5,6,7' from GSTRET01 
				WHERE PARTSR = '3C' AND SRNO ='4')
		Begin
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
				CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
				Select PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
				CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus='U-3,4,5,6,7' from GSTRET01 
					WHERE PARTSR = '3C' AND SRNO ='4'
		End
		Else
		Begin

			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,GRO_AMT,partdesc,inptStatus)
			select * from 
			(
			Select 3 as part,'3C' as partsr ,'4' srno,pinvno='',pinvdt=0,Net_amt=0,Rate1=0,
			isnull(SUM(TaxableAmt),0)TaxableAmt 
			,isnull(SUM(B.CGST_AMT),0) CGST_AMT
			,isnull(SUM(B.SGST_AMT),0) SGST_AMT
			,isnull(SUM(B.IGST_AMT),0) IGST_AMT,isnull(sum(B.COMPCESS),0) as CESS_AMT
			,gstin='',pos='',gstype=''
			,Av_CGST_AMT=0
			,Av_sGST_AMT=0
			,Av_iGST_AMT=0
			,av_CESS_AMT = 0
			,GRO_AMT=isnull(SUM(GRO_AMT),0)
			,partdesc='Advances adjusted'
			,inptStatus='U-3,4,5,6,7'
			FROM #GSTRET1TBL A 
			INNER JOIN TAXALLOCATION B ON  (A.ENTRY_TY=B.rENTRY_ty AND A.TRAN_CD=B.itref_tran)
			where mENTRY IN ('BR')
			AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) <= 0

			) bb
		End
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3C' AND SRNO ='4') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(3,'3C','4','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Advances adjusted','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			Select PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus='U-3,4,5,6,7' from GSTRET01 
				WHERE PARTSR = '3C' AND SRNO ='5'
		
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3C' AND SRNO ='5') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(3,'3C','5','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Reduction in output tax liability on account of transition from composition levy to normal levy, if any or any other reduction in liability','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3C' AND SRNO ='6') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,partdesc,Location,inptStatus)
			SELECT 3,'3C','6',CGST_AMT=SUM(case when SRNO IN ('1','3') then ISNULL(CGST_AMT,0) else -ISNULL(CGST_AMT,0) end)
			,SGST_AMT=SUM(case when SRNO IN ('1','3') then ISNULL(SGST_AMT,0) else -ISNULL(SGST_AMT,0) end)
			,IGST_AMT=SUM(case when SRNO IN ('1','3') then ISNULL(IGST_AMT,0) else -ISNULL(IGST_AMT,0) end)
			,CESS_AMT=SUM(case when SRNO IN ('1','3') then ISNULL(CESS_AMT,0) else -ISNULL(CESS_AMT,0) end)
			,GRO_AMT=SUM(case when SRNO IN ('1','3') then ISNULL(GRO_AMT,0) else -ISNULL(GRO_AMT,0) end)
			,'Sub-total (C) [1-2+3-4-5]','' as loc,inptStatus='C'
			FROM #GSTRET1 
			wHERE PARTSR='3C'
		END

		/*	3D. 1. Exempt and Nil rated supplies		*/
		Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,GRO_AMT,partdesc)
		select * from 
		(
		Select 3 as part,'3D' as partsr ,'1' srno,pinvno='',pinvdt=0,Net_amt=0,Rate1=0,
		SUM( TaxableAmt)TaxableAmt 
		--,SUM(case when mentry in('ST','SB','DN') THEN CGST_AMT ELSE -CGST_AMT END)CGST_AMT			--30/10/2019
		--,SUM(case when mentry in('ST','SB','DN') THEN SGST_AMT ELSE -SGST_AMT END)SGST_AMT
		--,SUM(case when mentry in('ST','SB','DN') THEN IGST_AMT ELSE -IGST_AMT END)IGST_AMT
		--,sum(case when mentry in('ST','SB','DN') THEN CESS_AMT ELSE -CESS_AMT END) as CESS_AMT
		,CGST_AMT=0
		,SGST_AMT=0
		,IGST_AMT=0
		,CESS_AMT=0
		,gstin='',pos='',gstype=''
		,Av_CGST_AMT=0
		,Av_sGST_AMT=0
		,Av_iGST_AMT=0
		,av_CESS_AMT = 0
		,GRO_AMT=SUM(GRO_AMT)
		,partdesc='Exempt and Nil rated supplies'
		FROM #GSTRET1TBL A 
		where (lineRule in('Nil Rated','Exempted') )
			--and st_type in('INTERSTATE','INTRASTATE') 
			AND (mentry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') 
			--AND Supp_type NOT IN('EXPORT','SEZ','IMPORT','EOU')

		) bb

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3D' AND SRNO ='1') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(3,'3D','1','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Exempt and Nil rated supplies')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		/*	3D. 2. Non-GST supplies (including No Supply / Schedule III supplies)	*/
		Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,net_amt,RATE,Taxableamt,
		CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,gstin,location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,GRO_AMT,partdesc)
		select * from 
		(
		Select 3 as part,'3D' as partsr ,'2' srno,pinvno='',pinvdt=0,Net_amt=0,Rate1=0,
		SUM( TaxableAmt)TaxableAmt 
		--,SUM(case when mentry in('ST','SB','DN') THEN CGST_AMT ELSE -CGST_AMT END)CGST_AMT			--30/10/2019
		--,SUM(case when mentry in('ST','SB','DN') THEN SGST_AMT ELSE -SGST_AMT END)SGST_AMT
		--,SUM(case when mentry in('ST','SB','DN') THEN IGST_AMT ELSE -IGST_AMT END)IGST_AMT
		--,sum(case when mentry in('ST','SB','DN') THEN CESS_AMT ELSE -CESS_AMT END) as CESS_AMT
		,CGST_AMT=0
		,SGST_AMT=0
		,IGST_AMT=0
		,CESS_AMT=0
		,gstin='',pos='',gstype=''
		,Av_CGST_AMT=0
		,Av_sGST_AMT=0
		,Av_iGST_AMT=0
		,av_CESS_AMT = 0
		,GRO_AMT=SUM(GRO_AMT)
		,partdesc='Non-GST supplies (including No Supply / Schedule III supplies)'
		FROM #GSTRET1TBL A 
		where (lineRule in('Non GST') or HSNCODE = '' )
			--and st_type in('INTERSTATE','INTRASTATE') 
			AND (mentry in('ST','SB','CN','DN','SR') and entry_ty<>'UB') 
			--AND Supp_type NOT IN('EXPORT','SEZ','IMPORT','EOU')

		) bb
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3D' AND SRNO ='2') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(3,'3D','2','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Non-GST supplies (including No Supply / Schedule III supplies)')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

--select * 
--from #GSTRET1TBL
--		where (mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type <> 'Out of country' 
--		and supp_type IN ('Registered','Compounding') 
--		and gstin <>'' AND LineRule = 'Taxable' AND HSNCODE <> '' and Ecomgstin = ''
--		and (isnull(SGSRT_AMT1,0) + isnull(CGSRT_AMT1,0)  + isnull(IGSRT_AMT1,0) + isnull(cessr_amt1,0)) > 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) = 0

	/* 3D. 3. 	Outward supplies attracting reverse charge (net of debit/ credit notes)	*/

		Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
		Select *
		from (
		SELECT 3 AS PART ,'3D' AS PARTSR,'3' AS SRNO,inv_no='',date=0,ORG_INVNO='',ORG_DATE=0,QTY=0
		,0 as rate1
		,taxableamt =sum(case when mEntry in('ST','SB','DN') THEN +isnull(taxableamt,0)ELSE - isnull(taxableamt,0) END )
		--,CGST_AMT = isnull(sum(case when mEntry in('ST','SB','DN') THEN +isnull(CGSRT_AMT1,0) ELSE - isnull(CGSRT_AMT1,0) END ),0)		--30/10/2019 
		--,SGST_AMT = isnull(sum(case when mEntry in('ST','SB','DN') THEN +isnull(SGSRT_AMT1,0) ELSE - isnull(SGSRT_AMT1,0) END ),0)
		--,IGST_AMT = isnull(sum(case when mEntry in('ST','SB','DN') THEN +isnull(IGSRT_AMT1,0) ELSE - isnull(IGSRT_AMT1,0) END ),0)
		--,cess_amt = isnull(sum(case when mEntry in('ST','SB','DN') THEN +isnull(cessr_amt1,0) ELSE - isnull(cessr_amt1,0) END ),0)

		,CGST_AMT=0
		,SGST_AMT=0
		,IGST_AMT=0
		,CESS_AMT=0

		,gro_amt=isnull(sum(case when mEntry in('ST','SB','DN') THEN isnull(gro_amt,0) else -isnull(gro_amt,0) end),0)
		,'' as gstin,'' As Location,''as SUPP_TYPE,Av_CGST_AMT=0,Av_sGST_AMT=0,Av_iGST_AMT=0,av_CESS_AMT=0
		,partdesc='Outward supplies attracting reverse charge(net of debit/ credit notes)' 
		from #GSTRET1TBL
		where (mEntry in ('ST','SB','CN','DN','SR') and entry_ty<>'UB') and st_type <> 'Out of country' 
		and supp_type IN ('Registered','Compounding') 
		and gstin <>'' AND LineRule = 'Taxable' AND HSNCODE <> '' 
		--and ecomgstin=''						--Commented by Shrikant S. on 11/11/2019 for AU 2.2.2
		and (isnull(SGSRT_AMT1,0) + isnull(CGSRT_AMT1,0)  + isnull(IGSRT_AMT1,0) + isnull(cessr_amt1,0)) > 0  and (SGST_AMT1 + CGST_AMT1  + IGST_AMT1 + cess_amt1) = 0
		) bb

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3D' AND SRNO ='3') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(3,'3D','3','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Outward supplies attracting reverse charge(net of debit/ credit notes)')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End




		/*	Supply of goods by a SEZ unit / developer to DTA on a Bill of Entry		*/
		Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
		select * from (
		Select 3 as part,'3D' as partsr ,'4' srno,pinvno='',pinvdt=0,ORG_INVNO='',ORG_DATE=0,Net_amt=0,Rate1=0
		,isnull(SUM(TaxableAmt),0) TaxableAmt  
		--,isnull(SUM(CGSRT_AMT),0) CGSRT_AMT,isnull(SUM(SGSRT_AMT),0) SGSRT_AMT,isnull(SUM(IGSRT_AMT),0) IGSRT_AMT			--30/10/2019
		--,isnull(sum(CessRt_Amt),0) as CessRt_Amt
		,CGST_AMT=0
		,SGST_AMT=0
		,IGST_AMT=0
		,CESS_AMT=0

		,gro_amt=isnull(sum(gro_amt),0),gstin=''
		,pos='',gstype=''
		,Av_CGST_AMT=0
		,Av_sGST_AMT=0
		,Av_iGST_AMT=0
		,av_CESS_AMT = 0
		,partdesc='Supply of goods by a SEZ unit / developer to DTA on a Bill of Entry'
		FROM #GSTR2TBL_RCM
		where mENTRY IN ('EP','PT','CN','DN')
		and Isservice <> 'Services'
		AND ST_TYPE <> 'Out of Country' and SUPP_TYPE = 'SEZ'
		and ltrim(rtrim(HSNCODE)) <>'' 
		--And gstin not in('Unregistered') 
		--AND LineRule = 'Nil Rated'
		--AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT > 0)
		) hh

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3D' AND SRNO ='4') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(3,'3D','4','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Supply of goods by a SEZ unit / developer to DTA on a Bill of Entry')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3D' AND SRNO ='5') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,partdesc,Location,inptStatus)
			SELECT 3,'3D','5',CGST_AMT=SUM(ISNULL(CGST_AMT,0)),SGST_AMT=SUM(ISNULL(SGST_AMT,0)),IGST_AMT=SUM(ISNULL(IGST_AMT,0)),CESS_AMT=SUM(ISNULL(CESS_AMT,0)),GRO_AMT=SUM(ISNULL(GRO_AMT,0))
			,'Sub-total (D) [sum of 1 to 4]','' as loc,inptStatus='C'
			FROM #GSTRET1 
			wHERE PARTSR='3D'
		END

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '3E' AND SRNO ='') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,partdesc,Location,inptStatus)
			SELECT 3,'3E','',CGST_AMT=SUM(ISNULL(CGST_AMT,0)),SGST_AMT=SUM(ISNULL(SGST_AMT,0)),IGST_AMT=SUM(ISNULL(IGST_AMT,0)),CESS_AMT=SUM(ISNULL(CESS_AMT,0)),GRO_AMT=SUM(ISNULL(GRO_AMT,0))
			,'Total value and tax liability (A+B+C+D)','' as loc,inptStatus='C'
			FROM #GSTRET1 
			wHERE (PARTSR='3A' AND SRNO='9') OR (PARTSR='3B' AND SRNO='3') OR (PARTSR='3C' AND SRNO='6') OR (PARTSR='3D' AND SRNO='5') 
		END


		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4A' AND SRNO='1'

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4A' AND SRNO ='1') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4A','1','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Credit on all documents which have been rejected in FORM GST ANX-2(net of debit /credit notes)','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4A' AND SRNO='2'

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4A' AND SRNO ='2') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4A','2','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Credit on all documents which have been kept pending in FORM GST ANX-2(net of debit /credit notes)','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4A' AND SRNO='3'

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4A' AND SRNO ='3') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4A','3','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Credit on all documents which have been accepted (including deemed accepted) in FORM GST ANX-2(net of debit/credit notes)','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4A' AND SRNO='4'

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4A' AND SRNO ='4') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4A','4','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Eligible credit (after 1st July, 2017) not availed prior to the introduction of this return but admissible as per Law(transition to new return system)','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		/*	4A. 5.	Inward supplies attracting reverse charge (net of debit/credit notes and advances paid, if any) [table 3H of FORM GST ANX-1]	*/
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4A' AND SRNO ='5') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			select 4,'4A','5',INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT
			,'Inward supplies attracting reverse charge (net of debit/credit notes and advances paid, if any) [table 3H of FORM GST ANX-1]' 
			from #GSTRET1
			where partsr='3B' AND SRNO='1'
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		/* 4A. 6.	Import of services (net of debit /credit notes and advances paid, if any and excluding services received from SEZ units) [table 3I of FORM GST ANX-1]	*/
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4A' AND SRNO ='6') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			--values(4,'4A','F','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Import of services (net of debit /credit notes and advances paid, if any and excluding services received from SEZ units)')
			select 4,'4A','6',INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,'Import of services (net of debit /credit notes and advances paid, if any and excluding services received from SEZ units) [table 3I of FORM GST ANX-1]' 
			from #GSTRET1
				where partsr='3B' AND SRNO='2'
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		/* 4A. 7.	Import of goods [table 3J of FORM GST ANX-1]	*/
		Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
		select * from (
		Select 4 as part,'4A' as partsr ,'7' srno,pinvno='',pinvdt=0,ORG_INVNO='',ORG_DATE=0,Net_amt=0,Rate1=0,
		SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then TaxableAmt else -TaxableAmt end)TaxableAmt 
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then CGST_AMT else -CGST_AMT end)CGST_AMT
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then SGST_AMT else -SGST_AMT end)SGST_AMT
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then IGST_AMT else -IGST_AMT end)IGST_AMT,sum(CASE WHEN mENTRY IN  ('EP','PT','CN') then CESS_AMT else -CESS_AMT end) as CESS_AMT
		,GRO_AMT=SUM(ISNULL(GRO_AMT,0)),gstin='',pos='',gstype=''
		,Av_CGST_AMT=0
		,Av_sGST_AMT=0
		,Av_iGST_AMT=0
		,av_CESS_AMT = 0
		,partdesc='Import of goods [table 3J of FORM GST ANX-1]'
		FROM #GSTR2TBL
		where mENTRY IN ('EP','PT','CN','DN')
		and Isservice <> 'Services'
		AND ST_TYPE IN('Out of Country','Interstate','Intrastate') and SUPP_TYPE in('Import','EOU','')
		AND (CGST_AMT + SGST_AMT + IGST_AMT) > 0
		) bb

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4A' AND SRNO ='7') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(4,'4A','7','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Import of goods [table 3J of FORM GST ANX-1]')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		/* 4A. 8.	Import of goods from SEZ units / developers [table 3K of FORM GST ANX-1]	*/
		Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
		select * from (
		Select 4 as part,'4A' as partsr ,'8' srno,pinvno='',pinvdt=0,ORG_INVNO='',ORG_DATE=0,Net_amt=0,Rate1=0,
		SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then TaxableAmt else -TaxableAmt end)TaxableAmt 
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then CGSRT_AMT else -CGSRT_AMT end)CGST_AMT
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then SGSRT_AMT else -SGSRT_AMT end)SGST_AMT
		,SUM(CASE WHEN mENTRY IN  ('EP','PT','CN') then IGSRT_AMT else -IGSRT_AMT end)IGST_AMT
		,sum(CASE WHEN mENTRY IN  ('EP','PT','CN') then CessRt_Amt else -CessRt_Amt end) as CESS_AMT
		,GRO_AMT=SUM(ISNULL(GRO_AMT,0)),gstin='',pos='',gstype=''
		,Av_CGST_AMT=0
		,Av_sGST_AMT=0
		,Av_iGST_AMT=0
		,av_CESS_AMT = 0
		,partdesc='Import of goods from SEZ units / developers [table 3K of FORM GST ANX-1]'
		FROM #GSTR2TBL
		where mENTRY IN ('EP','PT','CN','DN')
		and Isservice <> 'Services'
		AND ST_TYPE IN('Interstate','Intrastate') and SUPP_TYPE in('SEZ')
		--AND (CGSRT_AMT + SGSRT_AMT + IGSRT_AMT) > 0





		) bb
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4A' AND SRNO ='8') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(4,'4A','8','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Import of goods from SEZ units / developers [table 3K of FORM GST ANX-1]')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		/*	4A. 9. ISD Credit (net of ISD credit notes) [table 5 of FORM GST ANX-2]	*/
		Select 4 as part,'4A' as partsr ,'9' srno,pinvno='',pinvdt=0,0.00 as Net_amt,0 as Rate1,SUM(TaxableAmt) as TaxableAmt 
			,SUM(CGST_AMT)as CGST_AMT,SUM(SGST_AMT) as SGST_AMT,SUM(IGST_AMT)as IGST_AMT,sum(Cess_Amt) as Cess_Amt
			,gstin='','' as location,'' as gstype
			,Av_CGST_AMT=SUM(icgst_amt)
			,Av_sGST_AMT=SUM(isGST_AMT)
			,Av_iGST_AMT=SUM(iigst_amt)
			,av_CESS_AMT = SUM(ICESS_AMT),'Invoice' AS SUPP_TYPE,GRO_AMT=SUM(GRO_AMT)
			INTO #SEC5
			FROM #GSTR2TBL
			where ENTRY_TY ='J6'
			union all
			Select 4 as part,'4A' as partsr ,'9' srno
			,inv_no='',[DATE]=0
			,0.00 as Net_amt,0 as Rate1,SUM(TaxableAmt) as TaxableAmt 
			,SUM(CGST_AMT)as CGST_AMT,SUM(SGST_AMT) as SGST_AMT,SUM(IGST_AMT)as IGST_AMT,sum(Cess_Amt) as Cess_Amt 
			,gstin='','' as location,'' as gstype
			,Av_CGST_AMT=SUM(icgst_amt)
			,Av_sGST_AMT=SUM(isGST_AMT)
			,Av_iGST_AMT=SUM(iigst_amt)
			,av_CESS_AMT =SUM(ICESS_AMT),'Credit Note' AS SUPP_TYPE,GRO_AMT=SUM(GRO_AMT)
			FROM #GSTR2TBL 
			where ENTRY_TY ='J8'
			

			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			SELECT part,partsr,srno
			,pinvno='',pinvdt='',ORG_INVNO='',ORG_DATE=0,Net_amt=0,Rate1=0,SUM(TaxableAmt) as TaxableAmt 
			,SUM(CGST_AMT)as CGST_AMT,SUM(SGST_AMT) as SGST_AMT,SUM(IGST_AMT)as IGST_AMT,sum(Cess_Amt) as Cess_Amt,gro_amt=SUM(ISNULL(gro_amt,0))
			,gstin='','' as location,'' as gstype
			,Av_CGST_AMT=0
			,Av_sGST_AMT=0
			,Av_iGST_AMT=0
			,av_CESS_AMT =0
			,partdesc='ISD Credit (net of ISD credit notes) [table 5 of FORM GST ANX-2]'
			FROM #SEC5
			GROUP BY  part,partsr,srno

			drop table #SEC5

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4A' AND SRNO ='9') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc)
			values(4,'4A','9','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'ISD Credit (net of ISD credit notes) [table 5 of FORM GST ANX-2]')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4A' AND SRNO='10'

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4A' AND SRNO ='10') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4A','10','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Provisional input tax credit on documents not uploaded by the suppliers[net of ineligible credit]','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4A' AND SRNO='11'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4A' AND SRNO ='11') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4A','11','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Upward adjustment in input tax credit due to receipt of credit notes and all other adjustments and reclaims','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		/*	4A. 12. Sub-total (A) [sum of 3 to 11]	*/
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4A' AND SRNO ='12') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			--values(4,'4A','L','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Sub-total (A) [sum of 3 to 11]')
			sELECT 4,'4A','12','',0,'',0,0,0,0,
			CGST_AMT=SUM(ISNULL(CGST_AMT,0)),SGST_AMT=SUM(ISNULL(SGST_AMT,0)),IGST_AMT=SUM(ISNULL(IGST_AMT,0)),CESS_AMT=SUM(ISNULL(CESS_AMT,0))
			,GRO_AMT=SUM(GRO_AMT),gstin='', location='',Inputtype='',Av_CGST_AMT=0,Av_sGST_AMT=0,Av_iGST_AMT=0,av_CESS_AMT=0,partdesc='Sub-total (A) [sum of 3 to 11]'
			,inptStatus='C'
				FROM 
				#GSTRET1 wHERE PARTSR='4A' AND CONVERT(INT,SRNO) BETWEEN 3 AND 11
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End	
		
		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4B' AND SRNO='1'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4B' AND SRNO ='1') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4B','1','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Credit on documents which have been accepted in previous returns but rejected in current tax period (net of debit/ credit notes)','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4B' AND SRNO='2'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4B' AND SRNO ='2') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4B','2','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Supplies not eligible for credit (including ISD credit)[out of net credit available in table 4A above]','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4B' AND SRNO='3'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4B' AND SRNO ='3') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4B','3','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Reversal of credit in respect of supplies on which provisional credit has already been claimed in the previous tax periods but documents have been uploaded by the supplier in the current tax period(net of ineligible credit)','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4B' AND SRNO='4'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4B' AND SRNO ='4') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4B','4','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Reversal of input tax credit as per law(Rule 37, 39, 42 & 43)','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4B' AND SRNO='5'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4B' AND SRNO ='5') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4B','5','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Other reversals including downward adjustment of ITC on account of transition from composition levy to normal levy, If any','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4B' AND SRNO ='6') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			--values(4,'4B','F','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Sub-total (B) [sum of 1 to 5]')

			sELECT 4,'4B','6','',0,'',0,0,0,0,
			CGST_AMT=SUM(ISNULL(CGST_AMT,0)),SGST_AMT=SUM(ISNULL(SGST_AMT,0)),IGST_AMT=SUM(ISNULL(IGST_AMT,0)),CESS_AMT=SUM(ISNULL(CESS_AMT,0))
			,GRO_AMT=SUM(GRO_AMT),gstin='', location='',Inputtype='',Av_CGST_AMT=0,Av_sGST_AMT=0,Av_iGST_AMT=0,av_CESS_AMT=0,partdesc='Sub-total (B) [sum of 1 to 5]'
			,inptStatus='C'
				FROM 
				#GSTRET1 wHERE PARTSR='4B' 
		END

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4C' AND SRNO ='') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)

			sELECT 4,'4C','','',0,'',0,0,0,0,
			CGST_AMT=SUM(CASE WHEN PARTSR='4A' THEN ISNULL(CGST_AMT,0) ELSE -ISNULL(CGST_AMT,0) END)
			,SGST_AMT=SUM(CASE WHEN PARTSR='4A' THEN ISNULL(SGST_AMT,0) ELSE -ISNULL(SGST_AMT,0) END)
			,IGST_AMT=SUM(CASE WHEN PARTSR='4A' THEN ISNULL(IGST_AMT,0) ELSE -ISNULL(IGST_AMT,0) END)
			,CESS_AMT=SUM(CASE WHEN PARTSR='4A' THEN ISNULL(CESS_AMT,0) ELSE -ISNULL(CESS_AMT,0) END)
			,GRO_AMT=SUM(CASE WHEN PARTSR='4A' THEN GRO_AMT ELSE -GRO_AMT END),gstin='', location=''
			,Inputtype='',Av_CGST_AMT=0,Av_sGST_AMT=0,Av_iGST_AMT=0,av_CESS_AMT=0,partdesc='ITC available (net of reversals) (A- B)'
			,inptStatus='C'
				FROM 
				#GSTRET1 wHERE (PARTSR ='4A' and srno='12')  or (partsr='4B' and srno='6')
		END

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4D' AND SRNO='1'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4D' AND SRNO ='1') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4D','1','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'First month','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4D' AND SRNO='2'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4D' AND SRNO ='2') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4D','2','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Second month','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4D' AND SRNO ='3') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			sELECT 4,'4D','3','',0,'',0,0,0,0,
			CGST_AMT=SUM(ISNULL(CGST_AMT,0)),SGST_AMT=SUM(ISNULL(SGST_AMT,0)),IGST_AMT=SUM(ISNULL(IGST_AMT,0)),CESS_AMT=SUM(ISNULL(CESS_AMT,0))
			,GRO_AMT=SUM(GRO_AMT),gstin='', location='',Inputtype='',Av_CGST_AMT=0,Av_sGST_AMT=0,Av_iGST_AMT=0,av_CESS_AMT=0,partdesc='Sub-total (D) [sum of 1& 2]'
			,inptStatus='C'
				FROM 
				#GSTRET1 wHERE PARTSR='4D' 

		END


		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4E' AND SRNO ='') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			--values(4,'4E','','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Net ITC available (C-D)','C')

			sELECT 4,'4E','','',0,'',0,0,0,0,
			CGST_AMT=SUM(Case when PARTSR='4C' then ISNULL(CGST_AMT,0) else -ISNULL(CGST_AMT,0) End)
			,SGST_AMT=SUM(Case when PARTSR='4C' then ISNULL(SGST_AMT,0) else -ISNULL(SGST_AMT,0) end)
			,IGST_AMT=SUM(Case when PARTSR='4C' then ISNULL(IGST_AMT,0) else -ISNULL(IGST_AMT,0) end)
			,CESS_AMT=SUM(Case when PARTSR='4C' then ISNULL(CESS_AMT,0) else ISNULL(CESS_AMT,0) end)
			,GRO_AMT=SUM(Case when PARTSR='4C' then GRO_AMT else -GRO_AMT end)
			,gstin='', location='',Inputtype='',Av_CGST_AMT=0,Av_sGST_AMT=0,Av_iGST_AMT=0,av_CESS_AMT=0,partdesc='Net ITC available (C-D)'
			,inptStatus='C'
				FROM 
				#GSTRET1 wHERE (PARTSR='4D' and srno='3') or (PARTSR='4C' and SRNO='')

		END


		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4F' AND SRNO=''
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4F' AND SRNO ='') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4F','','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Input tax credit on capital goods (out of C)','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7' FROM GSTRET01 Where PARTSR='4G' AND SRNO=''

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '4G' AND SRNO ='') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(4,'4G','','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Input tax credit on services (out of C)','U-3,4,5,6,7')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U' FROM GSTRET01 Where PARTSR='5' AND SRNO='1'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '5' AND SRNO ='1') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(5,'5','1','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'TDS','U')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U' FROM GSTRET01 Where PARTSR='5' AND SRNO='2'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '5' AND SRNO ='2') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(5,'5','2','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'TCS','U')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '5' AND SRNO ='3') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			--values(5,'5A','C','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Total')
			sELECT 5,'5','3','',0,'',0,0,0,0,
			CGST_AMT=SUM(ISNULL(CGST_AMT,0)),SGST_AMT=SUM(ISNULL(SGST_AMT,0)),IGST_AMT=SUM(ISNULL(IGST_AMT,0)),CESS_AMT=SUM(ISNULL(CESS_AMT,0))
			,GRO_AMT=SUM(GRO_AMT),gstin='', location='',Inputtype='',Av_CGST_AMT=0,Av_sGST_AMT=0,Av_iGST_AMT=0,av_CESS_AMT=0,partdesc='Total'
			,inptStatus='C'
				FROM 
				#GSTRET1 wHERE PARTSR='5' 
		END

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7,8' FROM GSTRET01 Where PARTSR='6A' AND SRNO='1'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '6A' AND SRNO ='1') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(6,'6A','1','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Interest and late fee due to late filing of return (including late reporting of invoices of previous tax periods, rejection of accepted documents by the recipient) (to be computed by the system)','U-3,4,5,6,7,8')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7,8' FROM GSTRET01 Where PARTSR='6A' AND SRNO='2'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '6A' AND SRNO ='2') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(6,'6A','2','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Interest on account of reversal of input tax credit (to be calculated by the taxpayer)','U-3,4,5,6,7,8')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7,8' FROM GSTRET01 Where PARTSR='6A' AND SRNO='3'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '6A' AND SRNO ='3') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(6,'6A','3','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Interest on account of late reporting of supplies attracting reverse charge(to be calculated by the taxpayer)','U-3,4,5,6,7,8')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U-3,4,5,6,7,8' FROM GSTRET01 Where PARTSR='6A' AND SRNO='4'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '6A' AND SRNO ='4') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(6,'6A','4','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Other interest liability (to be specified)(to be calculated by the taxpayer)','U-3,4,5,6,7,8')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End


		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '6A' AND SRNO ='5') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			--values(6,'6A','E','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Total')
			sELECT 6,'6A','5','',0,'',0,0,0,0,
			CGST_AMT=SUM(ISNULL(CGST_AMT,0)),SGST_AMT=SUM(ISNULL(SGST_AMT,0)),IGST_AMT=SUM(ISNULL(IGST_AMT,0)),CESS_AMT=SUM(ISNULL(CESS_AMT,0))
			,GRO_AMT=SUM(GRO_AMT),gstin='', location='',Inputtype='',Av_CGST_AMT=0,Av_sGST_AMT=0,Av_iGST_AMT=0,av_CESS_AMT=0,partdesc='Total'
			,inptStatus='C'
				FROM 
				#GSTRET1 wHERE PARTSR='6A' 
		END
		
		
		/*	Payment of tax	*/
		--Reverse charge		
   	select mv.date as malldate,mv.entry_all,mv.main_tran,mv.acseri_all,new_all=sum(mv.new_all),b.u_cldt 
	into #mall3B 
	from mainall_vw mv
		inner join ac_mast a on (a.ac_id=mv.ac_id)	
		left outer join bpmain b on (mv.entry_ty=b.entry_ty and mv.tran_cd=b.tran_cd)
		where (a.typ Like '%GST Pay%' or a.typ Like  '%Cess payable%')
		 and b.u_cldt <= @edate
		 group by mv.Date,mv.entry_all,mv.main_tran,mv.acseri_all,b.u_cldt

	select sel=cast(0 as bit),m.entry_ty,m.tran_cd,ac.acserial,ac_name=ac_mast.ac_name,ac.amount,new_all=isnull(ac.amount,0),ac.amt_ty,ac_mast.typ
	,ac.Serty,party_nm=ac_mast.mailName,m.inv_no,m.date,tpayment=cast(0 as decimal(17,2)),m.l_yn,ac.ac_id,m.inv_sr,isused=0,m.net_amt,compid=0
	,TrnType=L.code_nm	Into #tmpsdata3B 	from SerTaxMain_vw m	INNER JOIN SerTaxAcDet_vw ac on (m.entry_ty=ac.entry_ty and m.tran_cd=ac.tran_cd)
	inner join ac_mast on (ac_mast.ac_id=m.ac_id) 	inner join lcode l on (l.entry_ty=m.entry_ty) 	WHERE 1=2
		
	select sel=cast(0 as bit),m.entry_ty,m.tran_cd,ac.acserial,a.ac_name,ac.amount,new_all=isnull(mall.new_all,0),ac.amt_ty,a.typ
	,ac.Serty,party_nm=ac_mast.mailName,m.inv_no,m.date,tpayment=cast(0 as decimal(17,2)),m.l_yn,ac.ac_id,m.inv_sr,isused=0,m.net_amt,compid=0
	,TrnType=l.code_nm
	into #taxpay
	from SerTaxMain_vw m
	INNER JOIN SerTaxAcDet_vw ac on (m.entry_ty=ac.entry_ty and m.tran_cd=ac.tran_cd)
	inner join ac_mast on (ac_mast.ac_id=m.ac_id)
	inner join ac_mast a on (a.ac_id=ac.ac_id)
	inner join lcode l on (l.entry_ty=m.entry_ty)
	left join #mall3B mall on(ac.entry_ty=mall.entry_all and ac.tran_cd=main_tran and ac.acserial=acseri_all)
	where (a.typ  Like '%GST Pay%' or a.typ Like  '%Cess payable%')
	and ac.amt_ty='CR'
	and ac.date <= @edate		
	and (l.entry_ty in ('BP','CP','B1','C1','IF','OF') or l.bcode_nm in ('BP','CP','B1','C1','IF','OF'))
	AND AC.amount-(CASE WHEN MALL.U_CLDT BETWEEN @SDATE AND @EDATE THEN 0 ELSE ISNULL(MALL.NEW_ALL,0) END) > 0
	order by m.date,inv_no
	
	insert Into #taxpay
	select sel=cast(0 as bit),rm.entry_ty,rm.tran_cd,ac.acserial,a.ac_name,ac.amount,new_all=isnull(mall.new_all,0),ac.amt_ty,a.typ
	,Serty=ac.serty,party_nm=ac_mast.mailName,rm.inv_no,rm.date,tpayment=cast(0 as decimal(17,2)),l_yn='',ac.ac_id,rm.inv_sr,isused=0,rm.net_amt,compid=0
	,TrnType=l.code_nm
	from RevMain_vw rm 
	INNER JOIN SerTaxAcDet_vw ac on (rm.entry_ty=ac.entry_ty and rm.tran_cd=ac.tran_cd)
	inner join ac_mast on (ac_mast.ac_id=rm.ac_id)
	inner join ac_mast a on (a.ac_id=ac.ac_id)
	inner join lcode l on (l.entry_ty=rm.entry_ty)
	left join #mall3B mall on(ac.entry_ty=mall.entry_all and ac.tran_cd=main_tran and ac.acserial=acseri_all)
	where (a.typ  Like '%GST Pay%' or a.typ Like  '%Cess payable%')
	and ac.amt_ty='CR'
	and ac.date <=@edate						
	and (l.entry_ty in ('PT','P1','E1','UB') or l.bcode_nm in ('PT','P1','E1'))
	AND AC.amount-(CASE WHEN MALL.U_CLDT BETWEEN @SDATE AND @EDATE THEN 0 ELSE ISNULL(MALL.NEW_ALL,0) END) > 0
	order by rm.date,rm.inv_no
	
	insert Into #taxpay
	select sel=cast(0 as bit),rm.entry_ty,rm.tran_cd,ac.acserial,a.ac_name,ac.amount,new_all=isnull(mall.new_all,0),ac.amt_ty,a.typ
	,Serty=ac.serty,party_nm=ac_mast.mailName,rm.inv_no,rm.date,tpayment=cast(0 as decimal(17,2)),l_yn='',ac.ac_id,rm.inv_sr,isused=0,rm.net_amt,compid=0
	,TrnType=l.code_nm
	from cnmain rm 
	INNER JOIN cnacdet ac on (rm.entry_ty=ac.entry_ty and rm.tran_cd=ac.tran_cd)
	inner join ac_mast on (ac_mast.ac_id=rm.ac_id)
	inner join ac_mast a on (a.ac_id=ac.ac_id)
	inner join lcode l on (l.entry_ty=rm.entry_ty)
	left join #mall3B mall on(ac.entry_ty=mall.entry_all and ac.tran_cd=main_tran and ac.acserial=acseri_all)
	where (a.typ  Like '%GST Pay%' or a.typ Like  '%Cess payable%')
	and ac.amt_ty='CR'
	and ac.date <=@edate						
	and l.entry_ty in ('C6','GC')
	and rm.AGAINSTGS in ('SERVICE PURCHASE BILL','PURCHASES')
	AND AC.amount-(CASE WHEN MALL.U_CLDT BETWEEN @SDATE AND @EDATE THEN 0 ELSE ISNULL(MALL.NEW_ALL,0) END) > 0
	order by rm.date,rm.inv_no
	
	Select Entry_ty,Tran_cd,Rentry_ty,Itref_tran Into #Othref3B from Othitref group by Entry_ty,Tran_cd,Rentry_ty,Itref_tran 

	select sel=cast(0 as bit),rm.entry_ty,rm.tran_cd,ac.acserial,a.ac_name,ac.amount,new_all=isnull(mall.new_all,0),ac.amt_ty,a.typ
	,Serty=ac.serty,party_nm=ac_mast.mailName,rm.inv_no,rm.date,tpayment=cast(0 as decimal(17,2)),l_yn='',ac.ac_id,rm.inv_sr,isused=0,rm.net_amt,compid=0
	,TrnType=l.code_nm
	,o.itref_tran,o.rentry_ty
	Into #debit3B
	from Dnmain rm 
	INNER JOIN Dnacdet ac on (rm.entry_ty=ac.entry_ty and rm.tran_cd=ac.tran_cd)		
	inner join ac_mast on (ac_mast.ac_id=rm.ac_id)
	inner join ac_mast a on (a.ac_id=ac.ac_id)
	inner join lcode l on (l.entry_ty=rm.entry_ty)
	Inner Join #Othref3B o on (o.entry_ty=rm.entry_ty and o.Tran_cd=rm.Tran_cd)
	left join #mall3B mall on(ac.entry_ty=mall.entry_all and ac.tran_cd=main_tran and ac.acserial=acseri_all and mall.malldate<@sdate)
	where (a.typ  Like '%GST Pay%' or a.typ Like  '%Cess payable%')
	and ac.amt_ty='DR'
	and ac.date <=@edate						
	and l.entry_ty in ('D6','GD')
	and rm.AGAINSTGS in ('SERVICE PURCHASE BILL','PURCHASES')
	order by rm.date,rm.inv_no
		
	Update #taxpay set amount=a.amount-b.amount 
		from #taxpay a 
		inner join #debit3B b on (a.entry_ty=b.rentry_ty and a.tran_cd=b.itref_tran and a.ac_id=b.ac_id and a.serty=b.serty)
		
 	Delete from #taxpay where amount<=0
	
	Declare @IGST_PAY Numeric(18,2),@CGST_PAY Numeric(18,2),@SGST_PAY  Numeric(18,2),@cess_PAY  Numeric(18,2),@IGST_TAX Numeric(18,2),@SGST_TAX Numeric(18,2),@CGST_TAX Numeric(18,2),@CCESS_TAX Numeric(18,2)
	Declare @IGST_PAY_Rev Numeric(18,2),@CGST_PAY_Rev Numeric(18,2),@SGST_PAY_Rev  Numeric(18,2),@cess_PAY_Rev  Numeric(18,2),@IGST_TAX_paid Numeric(18,2),@SGST_TAX_paid Numeric(18,2),@CGST_TAX_paid Numeric(18,2),@CCESS_TAX_paid Numeric(18,2)
	SET @IGST_PAY = 0
	SET @CGST_PAY = 0
	SET @SGST_PAY = 0
	set @cess_PAY = 0
	
	select 
	@IGST_PAY_Rev = sum((case when typ = 'IGST Payable'  then (amount) else 0 end)) ,
	@CGST_PAY_Rev = sum((case when typ = 'CGST Payable'  then  (amount) else 0 end)) ,
	@SGST_PAY_Rev = sum((case when typ = 'SGST Payable'  then  (amount) else 0 end)) ,
	@cess_PAY_Rev = sum((case when typ = 'COMP CESS PAYABLE'  then  (amount) else 0 end)) 
	from #taxpay
	WHERE DATE <= @EDATE

	--Added by Shrikant S. on 12/11/2019 for AU 2.2.2		--Start
	
	Select sum(b.Amount) as DebitAmount,b.typ 
	Into #DebitNote
	from #debit3B b 
	where b.Date Between @Sdate and @edate 
	and  b.rentry_ty+quotename(b.itref_tran)+quotename(b.ac_id)+rtrim(b.serty) Not In (select a.entry_ty+quotename(a.tran_cd)+quotename(a.ac_id)+rtrim(a.serty) from #taxpay a)
	group by b.typ
	
	select 
	@IGST_PAY_Rev = @IGST_PAY_Rev + isnull(sum((case when typ = 'IGST Payable'  then -DebitAmount else 0 end)),0) ,
	@CGST_PAY_Rev = @CGST_PAY_Rev + isnull(sum((case when typ = 'CGST Payable'  then  -DebitAmount else 0 end)),0) ,
	@SGST_PAY_Rev = @SGST_PAY_Rev + isnull(sum((case when typ = 'SGST Payable'  then  -DebitAmount else 0 end)),0) ,
	@cess_PAY_Rev = @cess_PAY_Rev + isnull(sum((case when typ = 'COMP CESS PAYABLE'  then  -DebitAmount else 0 end)),0) 
	from #DebitNote
	--Added by Shrikant S. on 12/11/2019 for AU 2.2.2		--End

	set @IGST_TAX_paid =0
	set @SGST_TAX_paid =0
	set @CGST_TAX_paid =0
	SET @CCESS_TAX_paid=0 
		
--Other than reverse charge
SELECT a=1,a.ac_id,a.ac_name,a.amount,a.amt_ty
		,a.entry_ty,a.date,a.u_cldt,a.inv_sr,a.Tran_cd,a.acserial
				iNTO #TBL1
				FROM lac_vw a
				inner join lmain_vw b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.tran_cd)
				left outer join mainall_vw mv on (b.entry_ty=mv.entry_ty and b.tran_cd=mv.tran_cd)
				left join lcode l on (a.entry_ty=l.entry_ty)
				WHERE a.ac_name IN('Central GST Payable A/C','State GST Payable A/C','Integrated GST Payable A/C','Compensation Cess Payable A/C')
				and ((case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end) not in ('GA','GB','PT','BP','CP') or a.entry_ty in ('RV','UB'))
				and l.entry_ty<>'GA'  
				AND (Case when YEAR(a.u_cldt)>2000 then a.u_cldt else a.date end) <= @EDATE		
				and (a.entry_ty+QUOTENAME(a.tran_cd)) not in (select (mv.entry_all+quotename(mv.main_tran)) from mainall_vw mv inner join bpmain b on (b.entry_ty=mv.entry_ty and b.tran_cd=mv.tran_cd))

		UNION ALL
			SELECT a=2,a.ac_id,a.ac_name,a.amount,a.amt_ty
			,a.entry_ty,a.date,a.u_cldt,b.inv_sr,a.Tran_cd,a.acserial
				FROM jvacdet a
				inner join jvmain b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.tran_cd)
				WHERE a.ac_name IN('Central GST Payable A/C','State GST Payable A/C','Integrated GST Payable A/C','Compensation Cess Payable A/C') 
				and b.entry_ty IN ('GA')
				AND b.U_CLDT <  @SDATE AND YEAR(b.U_CLDT) > 2000
		UNION ALL
			SELECT a=3,a.ac_id,a.ac_name,a.amount,a.amt_ty
			,a.entry_ty,a.date,a.u_cldt,b.inv_sr,a.Tran_cd,a.acserial
				FROM bpacdet a
				inner join bpmain b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.tran_cd)
				WHERE a.ac_name IN('Central GST Payable A/C','State GST Payable A/C','Integrated GST Payable A/C','Compensation Cess Payable A/C') 
				and b.entry_ty IN ('GB')
				AND b.date <= @EDATE		
				and (b.entry_ty+QUOTENAME(b.tran_cd)) not in (select (mv.entry_ty+quotename(mv.tran_cd)) from mainall_vw mv inner join bpmain b on (b.entry_ty=mv.entry_ty and b.tran_cd=mv.tran_cd))
	
	Delete from #TBL1 Where Entry_ty in ('GC','GD') AND INV_SR<>'SALES'

	Delete from #TBL1 Where Entry_ty in ('UB') AND entry_ty +QUOTENAME(Tran_cd)+ACSERIAL Not in (select (entry_ALL +QUOTENAME(MAIN_TRAN)+ACSERI_ALL) from  Mainall_vw 
				where  DATE_all <= @EDATE )
	
	Delete from #TBL1 Where Entry_ty in ('UB')			--Commented by Shrikant S. on 13/11/2019 for AU 2.2.2
	--Delete from #TBL1 Where Entry_ty in ('UB','GB')			--Added by Shrikant S. on 13/11/2019 for AU 2.2.2
	
		SELECT @IGST_PAY= SUM(CASE WHEN ac_name ='Integrated GST Payable A/C' THEN (case when amt_ty ='cr'  then + AMOUNT else -AMOUNT  end)  ELSE 0.00 END)
				,@SGST_PAY= SUM(CASE WHEN ac_name ='State GST Payable A/C' THEN (case when amt_ty ='cr'  then + AMOUNT else -AMOUNT  end) ELSE 0.00 END)
				,@CGST_PAY= SUM(CASE WHEN ac_name ='Central GST Payable A/C' THEN (case when amt_ty ='cr'  then + AMOUNT else -AMOUNT  end) ELSE 0.00 END)
				,@cess_PAY= SUM(CASE WHEN ac_name ='Compensation Cess Payable A/C' THEN (case when amt_ty ='cr'  then + AMOUNT else -AMOUNT  end) ELSE 0.00 END)
				FROM #TBL1 

set @IGST_TAX_paid =0
	set @SGST_TAX_paid =0
	set @CGST_TAX_paid =0
	SET @CCESS_TAX_paid=0 
	SELECT @IGST_TAX_paid=SUM(case when a.ac_name ='Integrated GST Payable A/C' then ISNULL(a.amount,0) else 0 end)
			,@CGST_TAX_paid=SUM(case when a.ac_name ='Central GST Payable A/C' then ISNULL(a.amount,0) else 0 end)
			,@SGST_TAX_paid=SUM(case when a.ac_name ='State GST Payable A/C' then ISNULL(a.amount,0) else 0 end)
			,@CCESS_TAX_paid=SUM(case when a.ac_name ='Compensation Cess Payable A/C' then ISNULL(a.amount,0) else 0 end)
				FROM jvacdet a
				inner join jvmain b on (a.Entry_ty=b.Entry_ty and a.Tran_cd=b.tran_cd)
				WHERE a.ac_name IN('Central GST Payable A/C','State GST Payable A/C','Integrated GST Payable A/C','Compensation Cess Payable A/C') 
				and b.entry_ty IN ('GA')
				AND b.U_CLDT <=  @EDATE AND YEAR(b.U_CLDT) > 2000


 DECLARE @IGST_INT DECIMAL(18,2),@SGST_INT DECIMAL(18,2),@CGST_INT DECIMAL(18,2),@CESS_INT DECIMAL(18,2),@IGST_FEE DECIMAL(18,2),@SGST_FEE DECIMAL(18,2),@CGST_FEE DECIMAL(18,2),@CESS_FEE DECIMAL(18,2)
	  ----Interest Payable A/c
	  set @IGST_INT = 0
	  set @SGST_INT = 0
	  set @CGST_INT = 0
	  select @IGST_INT = isnull(sum(a.amount),0)  from lac_vw a INNER JOIN BPMAIN B ON(A.entry_ty =B.entry_ty AND A.Tran_cd=B.Tran_cd) where a.ac_name ='Integrated GST Interest Payable A/C' and a.entry_ty ='GB' AND (b.u_cldt BETWEEN @SDATE AND @EDATE)
	  select @CGST_INT = isnull(sum(a.amount),0)  from lac_vw a INNER JOIN BPMAIN B ON(A.entry_ty =B.entry_ty AND A.Tran_cd=B.Tran_cd) where a.ac_name ='Central GST Interest Payable A/C' and a.entry_ty ='GB' AND (b.u_cldt BETWEEN @SDATE AND @EDATE) 
	  select @SGST_INT = isnull(sum(a.amount),0)  from lac_vw a INNER JOIN BPMAIN B ON(A.entry_ty =B.entry_ty AND A.Tran_cd=B.Tran_cd) where a.ac_name ='State GST Interest Payable A/C' and a.entry_ty ='GB' AND (b.u_cldt BETWEEN @SDATE AND @EDATE) 
	  select @CESS_INT = isnull(sum(a.amount),0)  from lac_vw a INNER JOIN BPMAIN B ON(A.entry_ty =B.entry_ty AND A.Tran_cd=B.Tran_cd) where a.ac_name ='Compensation Cess Interest Payable A/C' and a.entry_ty ='GB' AND (b.u_cldt BETWEEN @SDATE AND @EDATE) 
	  
	  ----Late fee Payable A/c
	  set @IGST_FEE = 0
	  set @SGST_FEE = 0
	  set @CGST_FEE = 0
	  select @IGST_FEE = isnull(sum(a.amount),0)  from lac_vw a INNER JOIN BPMAIN B ON(A.entry_ty =B.entry_ty AND A.Tran_cd=B.Tran_cd) where a.ac_name ='Integrated GST Late Fee Payable A/C' and a.entry_ty ='GB' AND (B.u_cldt BETWEEN @SDATE AND @EDATE) 
	  select @CGST_FEE = isnull(sum(a.amount),0)  from lac_vw a INNER JOIN BPMAIN B ON(A.entry_ty =B.entry_ty AND A.Tran_cd=B.Tran_cd) where a.ac_name ='Central GST Late Fee Payable A/C' and a.entry_ty ='GB' AND (B.u_cldt BETWEEN @SDATE AND @EDATE) 
	  select @SGST_FEE = isnull(sum(a.amount),0)  from lac_vw a INNER JOIN BPMAIN B ON(A.entry_ty =B.entry_ty AND A.Tran_cd=B.Tran_cd) where a.ac_name ='State GST Late Fee Payable A/C' and a.entry_ty ='GB' AND (b.u_cldt BETWEEN @SDATE AND @EDATE) 
	  select @CESS_FEE= isnull(sum(a.amount),0)  from lac_vw a INNER JOIN BPMAIN B ON(A.entry_ty =B.entry_ty AND A.Tran_cd=B.Tran_cd) where a.ac_name ='Compensation Cess Late Fee Payable A/C' and a.entry_ty ='GB' AND (b.u_cldt BETWEEN @SDATE AND @EDATE) 

	 ---TAX paid cess  
	  set @IGST_TAX = 0
	  set @SGST_TAX = 0
	  set @CGST_TAX = 0
	  SET @CCESS_TAX = 0
	  
	  select @IGST_TAX = isnull(sum(a.amount),0)  from lac_vw a INNER JOIN BPMAIN B ON(A.entry_ty =B.entry_ty AND A.Tran_cd=B.Tran_cd) where a.ac_name ='Integrated GST Payable A/C' and a.entry_ty ='GB' AND (B.u_cldt BETWEEN @SDATE AND @EDATE) 
	  and (b.entry_ty+QUOTENAME(b.tran_cd)) not in (select (mv.entry_ty+quotename(mv.tran_cd)) from mainall_vw mv inner join bpmain b on (b.entry_ty=mv.entry_ty and b.tran_cd=mv.tran_cd))
	  select @CGST_TAX = isnull(sum(a.amount),0) from lac_vw a INNER JOIN BPMAIN B ON(A.entry_ty =B.entry_ty AND A.Tran_cd=B.Tran_cd) where a.ac_name ='Central GST Payable A/C' and a.entry_ty ='GB' AND (B.u_cldt BETWEEN @SDATE AND @EDATE) 
	  and (b.entry_ty+QUOTENAME(b.tran_cd)) not in (select (mv.entry_ty+quotename(mv.tran_cd)) from mainall_vw mv inner join bpmain b on (b.entry_ty=mv.entry_ty and b.tran_cd=mv.tran_cd))
	  select @SGST_TAX = isnull(sum(a.amount),0)  from lac_vw a INNER JOIN BPMAIN B ON(A.entry_ty =B.entry_ty AND A.Tran_cd=B.Tran_cd) where a.ac_name ='State GST Payable A/C' and a.entry_ty ='GB' AND (B.u_cldt BETWEEN @SDATE AND @EDATE) 
	  and (b.entry_ty+QUOTENAME(b.tran_cd)) not in (select (mv.entry_ty+quotename(mv.tran_cd)) from mainall_vw mv inner join bpmain b on (b.entry_ty=mv.entry_ty and b.tran_cd=mv.tran_cd))
	  select @CCESS_TAX = isnull(sum(a.amount),0)  from lac_vw a INNER JOIN BPMAIN B ON(A.entry_ty =B.entry_ty AND A.Tran_cd=B.Tran_cd) where a.ac_name ='Compensation Cess Payable A/C' and a.entry_ty ='GB' AND (B.u_cldt BETWEEN @SDATE AND @EDATE) 
	  and (b.entry_ty+QUOTENAME(b.tran_cd)) not in (select (mv.entry_ty+quotename(mv.tran_cd)) from mainall_vw mv inner join bpmain b on (b.entry_ty=mv.entry_ty and b.tran_cd=mv.tran_cd))
	  
	  
	--Added by Priyanka B on 25022020 for Bug-32885 Start
	select 
	@IGST_PAY_Rev = IGST_AMT,
	@CGST_PAY_Rev = CGST_AMT,
	@SGST_PAY_Rev = SGST_AMT,
	@cess_PAY_Rev = CESS_AMT 
	from #GSTRET1 WHERE PARTSR='3B' AND SRNO='3'

	select 
	@IGST_PAY = SUM(case when partsr='3E' and srno='' then IGST_AMT else -IGST_AMT end),
	@CGST_PAY = SUM(case when partsr='3E' and srno='' then CGST_AMT else -CGST_AMT end),
	@SGST_PAY = SUM(case when partsr='3E' and srno='' then SGST_AMT else -SGST_AMT end),
	@cess_PAY = SUM(case when partsr='3E' and srno='' then CESS_AMT else -CESS_AMT end) 
	from #GSTRET1 WHERE (PARTSR ='3B' AND SRNO = '3') OR (PARTSR ='3E' AND SRNO = '')
	--Added by Priyanka B on 25022020 for Bug-32885 End

	  Declare @Amount1 Numeric(18,2), @Amount2 Numeric(18,2), @Amount3 Numeric(18,2), @Amount4 Numeric(18,2), @Amount5 Numeric(18,2)

	  select @Amount1=0, @Amount2=0, @Amount3=0, @Amount4=0,@Amount5=0
	  select @Amount1=Amount1, @Amount2=Amount2, @Amount3=Amount3, @Amount4 =Amount4 From GSTRET01 Where StartDt=@Sdate and enddt=@EDATE and partsr='7A' and srno='1'

	  
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '7A' AND SRNO ='1') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,Amount1,Amount2,Amount3,Amount4,Amount5,inptStatus)
			values(7,'7A','1','','','','',0,0,@IGST_PAY,0,0,@IGST_TAX_paid,0,@IGST_PAY_Rev,'','','',@IGST_INT,@IGST_FEE,@IGST_TAX,0,'Integrated tax',@Amount1, @Amount2, @Amount3, @Amount4,@Amount5,'U-5,6,7,8')
		END

		select @Amount1=0, @Amount2=0, @Amount3=0, @Amount4=0,@Amount5=0
		select @Amount1=Amount1, @Amount2=Amount2, @Amount3=Amount3, @Amount4 =Amount4 From GSTRET01 Where StartDt=@Sdate and enddt=@EDATE and partsr='7A' and srno='2'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '7A' AND SRNO ='2') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,Amount1,Amount2,Amount3,Amount4,Amount5,inptStatus)
			values(7,'7A','2','','','','',0,0,@CGST_PAY,@CGST_TAX_paid,0,0,0,@CGST_PAY_Rev,'','','',@CGST_INT,@CGST_FEE,@CGST_TAX,0,'Central tax',@Amount1, @Amount2, @Amount3, @Amount4,@Amount5,'U-5,6,7,8')
		END

		select @Amount1=0, @Amount2=0, @Amount3=0, @Amount4=0,@Amount5=0
	  select @Amount1=Amount1, @Amount2=Amount2, @Amount3=Amount3, @Amount4 =Amount4 From GSTRET01 Where StartDt=@Sdate and enddt=@EDATE and partsr='7A' and srno='3'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '7A' AND SRNO ='3') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,Amount1,Amount2,Amount3,Amount4,Amount5,inptStatus)
			values(7,'7A','3','','','','',0,0,@SGST_PAY,0,@SGST_TAX_paid,0,0,@SGST_PAY_Rev,'','','',@SGST_INT,@SGST_FEE,@SGST_TAX,0,'State/UT tax',@Amount1, @Amount2, @Amount3, @Amount4,@Amount5,'U-5,6,7,8')
		END

		select @Amount1=0, @Amount2=0, @Amount3=0, @Amount4=0,@Amount5=0
		select @Amount1=Amount1, @Amount2=Amount2, @Amount3=Amount3, @Amount4 =Amount4 From GSTRET01 Where StartDt=@Sdate and enddt=@EDATE and partsr='7A' and srno='4'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '7A' AND SRNO ='4') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,Amount1,Amount2,Amount3,Amount4,Amount5,inptStatus)
			values(7,'7A','4','','','','',0,0,@cess_PAY,0,0,0,@CCESS_TAX_paid,@cess_PAY_Rev,'','','',@CESS_INT,@CESS_FEE,@CCESS_TAX,0,'Cess',@Amount1, @Amount2, @Amount3, @Amount4,@Amount5,'U-5,6,7,8')
		END

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '7A' AND SRNO ='5') 
		BEGIN
			
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus,Amount1,Amount2,Amount3,Amount4,Amount5)
			--values(7,'7A','E','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Total')
			Select 7,'7A','5','',0,'',0,0,0,Taxableamt=sum(Isnull(Taxableamt,0)),
			CGST_AMT=SUM(isnull(CGST_AMT,0)),SGST_AMT=SUM(isnull(SGST_AMT,0)),IGST_AMT=SUM(isnull(IGST_AMT,0)),CESS_AMT=SUM(isnull(CESS_AMT,0))
			,GRO_AMT=SUM(isnull(GRO_AMT,0)),gstin='',location='',Inputtype='',Av_CGST_AMT=sum(isnull(Av_CGST_AMT,0)),Av_sGST_AMT=sum(isnull(Av_sGST_AMT,0))
			,Av_iGST_AMT=sum(isnull(Av_iGST_AMT,0)),av_CESS_AMT=sum(isnull(av_CESS_AMT,0))
			,partdesc='Total',inptStatus='C',Amount1=Sum(amount1),Amount2=Sum(amount2),Amount3=Sum(amount3),Amount4=Sum(amount4),Amount5=Sum(amount5)
			from #GSTRET1
			Where PARTSR='7A'

		END


		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U' FROM GSTRET01 Where PARTSR='8A' AND SRNO='1'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '8A' AND SRNO ='1') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(8,'8A','1','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Integrated tax','U')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U' FROM GSTRET01 Where PARTSR='8A' AND SRNO='2'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '8A' AND SRNO ='2') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(8,'8A','2','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Central tax','U')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U' FROM GSTRET01 Where PARTSR='8A' AND SRNO='3'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '8A' AND SRNO ='3') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(8,'8A','3','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'State/UT tax','U')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		Insert Into #GSTRET1(PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus)
		sELECT PART,PARTSR,SRNO,gstin,inv_no,date,location,gro_amt,rate,taxableamt
		,CGST_AMT,SGST_AMT,IGST_AMT,cess_amt,AC_NAME,SUPP_TYPE,ST_TYPE,hsncode,SBBILLNO,SBDATE,partdesc,inptStatus='U' FROM GSTRET01 Where PARTSR='8A' AND SRNO='4'
		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '8A' AND SRNO ='4') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
			values(8,'8A','4','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Cess','U')
		END
		else
		begin
			Update #GSTRET1 set GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)
		End

		IF NOT EXISTS(SELECT PART FROM #GSTRET1 WHERE PARTSR = '8A' AND SRNO ='') 
		BEGIN
			Insert into #GSTRET1(PART,PARTSR,SRNO,INV_NO,DATE,ORG_INVNO,ORG_DATE,QTY,RATE,Taxableamt,
			CGST_AMT,SGST_AMT,IGST_AMT,CESS_AMT,GRO_AMT,gstin, location,Inputtype,Av_CGST_AMT,Av_sGST_AMT,Av_iGST_AMT,av_CESS_AMT,partdesc,inptStatus)
--			values(8,'8A','E','','','','',0,0,0,0,0,0,0,0,'','','',0,0,0,0,'Total')
			Select 8,'8A','','',0,'',0,0,0,Taxableamt=sum(Isnull(Taxableamt,0)),
			CGST_AMT=SUM(isnull(CGST_AMT,0)),SGST_AMT=SUM(isnull(SGST_AMT,0)),IGST_AMT=SUM(isnull(IGST_AMT,0)),CESS_AMT=SUM(isnull(CESS_AMT,0))
			,GRO_AMT=SUM(isnull(GRO_AMT,0)),gstin='',location='',Inputtype='',Av_CGST_AMT=sum(isnull(Av_CGST_AMT,0)),Av_sGST_AMT=sum(isnull(Av_sGST_AMT,0))
			,Av_iGST_AMT=sum(isnull(Av_iGST_AMT,0)),av_CESS_AMT=sum(isnull(av_CESS_AMT,0))
			,partdesc='Total',inptStatus='C'
			from #GSTRET1
			Where PARTSR='8A'
		END

Update #GSTRET1 set inptstatus=isnull(inptstatus,''),Amount1=isnull(Amount1,0),Amount2=isnull(Amount2,0),Amount3=isnull(Amount3,0),Amount4=isnull(Amount4,0),Amount5=isnull(Amount5,0)
	,Av_CGST_AMT=isnull(Av_CGST_AMT,0),Av_SGST_AMT=isnull(Av_SGST_AMT,0),Av_IGST_AMT=isnull(Av_IGST_AMT,0),av_CESS_AMT=isnull(av_CESS_AMT,0),GRO_AMT=isnull(GRO_AMT,0),taxableamt=isnull(taxableamt,0),CGST_AMT=isnull(CGST_AMT,0),SGST_AMT=isnull(SGST_AMT,0),IGST_AMT=isnull(IGST_AMT,0),cess_amt=isnull(cess_amt,0)

		

SELECT *,StartDt=@SDATE,EndDt=@EDATE FROM #GSTRET1
	order by PART,PARTSR,TableRowId 
end