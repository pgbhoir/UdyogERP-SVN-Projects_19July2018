If Exists(Select [Name] From SysObjects Where xType='P' and [Name]='usp_Ent_GST_RFD01')
Begin
	DROP PROCEDURE [usp_Ent_GST_RFD01]
end
GO
/****** Object:  StoredProcedure [dbo].[usp_Ent_GST_RFD01]    Script Date: 27-05-2020 17:24:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[usp_Ent_GST_RFD01]
@Entry_ty Varchar(2),@Tran_cd Numeric(10),@sDate Smalldatetime,@eDate Smalldatetime
as
Begin
Declare @FCON as NVARCHAR(4000),@SQLCOMMAND as NVARCHAR(4000),@FINYR VARCHAR(50)
print @sDate
	Declare @OPENTRIES as VARCHAR(50),@OPENTRY_TY as VARCHAR(50)
	Declare @TBLNM as VARCHAR(50),@TBLNAME1 as VARCHAR(50),@TBLNAME2 as VARCHAR(50),@TBLNAME3 as VARCHAR(50),@TBLNAME4 as VARCHAR(50),@TBLNAME5 as VARCHAR(50)
	declare @TBLNAME6 as VARCHAR(50),@TBLNAME7 as VARCHAR(50),@TBLNAME8 as VARCHAR(50)
	Declare @CGST_AMT decimal(18,2),@SGST_AMT decimal(18,2),@IGST_AMT decimal(18,2) ,@CESS_AMT decimal(18,2)
	Set @OPENTRY_TY = '''OB'''
	Set @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
					+ (DATEPART(ss, GETDATE()) * 1000 )
					+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
	Set @TBLNAME1 = '##TMP1'+@TBLNM
	Set @TBLNAME2 = '##TMP2'+@TBLNM

SELECT * INTO #rfd01TBL1 FROM GSTR3B_VW WHERE (case when year(u_cldt)>1900 then u_cldt else DATE end) BETWEEN @SDATE AND @EDATE	

SELECT mEntry=case when l.Bcode_nm<>'' then l.Bcode_nm else (case when l.ext_vou=1 then '' else l.entry_ty end) end
,ADJ_TAXABLE =(Taxableamt * RIO),ADJ_CGST_AMT =(CGST_AMT * RIO),ADJ_SGST_AMT =(SGST_AMT * RIO),ADJ_IGST_AMT =(IGST_AMT * RIO)
,ADJ_CESS_AMT =(cess_amt * RIO),gb.* 
INTO #rfd01TBL FROM #rfd01TBL1 gb inner join lcode l  on (gb.entry_ty=l.entry_ty)		
WHERE (case when year(u_cldt)>1900 then u_cldt else DATE end) BETWEEN @SDATE AND @EDATE

UPDATE #rfd01TBL SET SUPP_TYPE = '' WHERE ST_TYPE = 'OUT OF COUNTRY' 

SELECT taxvalue_a =U_ASSEAMT,taxvalue_z=U_ASSEAMT,cgst_itc=cgst_amt,sgst_itc=SGST_AMT,igst_itc=IGST_AMT,ccess_itc=COMPCESS
,NetITC_ISC=igst_amt,NetITC_CES=igst_amt,cgst_itc1=cgst_amt,sgst_itc1=sgst_amt,igst_itc1=IGST_AMT,cess_itc1=CGST_AMT,cgst_itc2=CGST_AMT
,sgst_itc2=SGST_AMT,igst_itc2=igst_amt,cess_itc2=COMPCESS into #finalreftbl from ptitem where 1=2

select part=entry_ty,igst_amt,sgst_amt,cgst_amt,compcess into #NetITC from ptitem where 1=2

SET @CGST_AMT =0.00
SET @SGST_AMT =0.00
SET @IGST_AMT =0.00
SET @CESS_AMT =0.00
SELECT @IGST_AMT =SUM(Case when mEntry in('PT','CN') then +IGST_AMT else -(IGST_AMT ) end),@CGST_AMT =SUM(Case when mEntry in('PT','CN') then +CGST_AMT else -(CGST_AMT ) end)
,@SGST_AMT =SUM(Case when mEntry in('PT','CN') then +SGST_AMT else -(SGST_AMT) end),@CESS_AMT =SUM(Case when mEntry in('PT','CN') then +CESS_AMT else -(CESS_AMT) end)
FROM #rfd01TBL WHERE mEntry IN('PT','PR','CN','DN') AND Isservice  ='GOODS' AND ((st_type ='OUT OF COUNTRY' AND SUPP_TYPE IN('IMPORT','')) OR ( st_type IN('INTRASTATE','INTERSTATE') 
AND SUPP_TYPE IN('SEZ','IMPORT','EOU','EXPORT'))) AND AGAINSTGS in('','PURCHASES','GOODS') and isnull(ITCSEC,'') = ''

insert into #NetITC values('A',@IGST_AMT,@SGST_AMT,@CGST_AMT,@CESS_AMT)

SELECT *  INTO #PTEPRCMTBL FROM (select entry_ty,tran_cd,scons_id,DATE,u_cldt  from EPMAIN  union all select entry_ty,tran_cd,scons_id,DATE,u_cldt from PtMAIN )A  where entry_ty in('PT','P1','E1') AND u_cldt <= @EDATE
SELECT *  INTO #BPRCM FROM (select entry_ty,tran_cd,AC_ID,DATE,u_cldt  from CPMAIN union all select entry_ty,tran_cd,Ac_id, DATE,u_cldt from BPMAIN )A  where entry_ty in('CP','BP') AND u_cldt <= @EDATE

SET @CGST_AMT =0.00
SET @SGST_AMT =0.00
SET @IGST_AMT =0.00
SET @CESS_AMT =0.00
select @IGST_AMT = isnull(sum((case when b.typ = 'IGST Payable' then a.new_all else 0.00 end)),0)
,@CESS_AMT = isnull(sum((case when b.typ = 'COMP CESS PAYABLE' then a.new_all else 0.00 end)),0)
from mainall_vw  a left outer join AC_MAST b on a.Ac_id = b.Ac_id  
left outer join bpmain c on (a.entry_ty=c.entry_ty and a.tran_cd=c.tran_cd) where a.entry_ty = 'GB' 
and c.u_cldt between @SDATE and @EDATE
and b.typ in('CGST Payable','SGST Payable','IGST Payable','COMP CESS PAYABLE') 
and ((a.ENTRY_ALL+ quotename(a.Main_tran))  in (select  a.entry_ty+ quotename(a.Tran_cd)  from #PTEPRCMTBL a LEFT OUTER JOIN shipto c ON (c.shipto_id = a.scons_id) where c.Supp_type in('import','sez','Eou','','Export')) or  (a.ENTRY_ALL+ quotename(a.Main_tran)) in (select  a.entry_ty+ quotename(a.Tran_cd)  from #BPRCM a LEFT OUTER JOIN AC_MAST  c ON (c.Ac_id  = a.Ac_id ) where c.Supp_type in('import','sez','Eou','','Export')))

insert into #NetITC values('A',@IGST_AMT,@SGST_AMT,@CGST_AMT,@CESS_AMT)

SET @CGST_AMT =0.00
SET @SGST_AMT =0.00
SET @IGST_AMT =0.00
SET @CESS_AMT =0.00
select @IGST_AMT = isnull(sum((case when b.typ = 'IGST Payable' then a.new_all else 0.00 end)),0)
,@SGST_AMT = isnull(sum((case when b.typ = 'SGST Payable' then a.new_all else 0.00 end)),0) 
,@CGST_AMT = isnull(sum((case when b.typ = 'CGST Payable' then a.new_all else 0.00 end)),0) 
,@CESS_AMT = isnull(sum((case when b.typ = 'COMP CESS PAYABLE' then a.new_all else 0.00 end)),0)
from mainall_vw  a left outer join AC_MAST b on a.Ac_id = b.Ac_id  
left outer join bpmain c on (a.entry_ty=c.entry_ty and a.tran_cd=c.tran_cd) 
where a.entry_ty = 'GB' 
and c.u_cldt between @SDATE and @EDATE
and b.typ in('CGST Payable','SGST Payable','IGST Payable','COMP CESS PAYABLE') 
and ((a.ENTRY_ALL+ quotename(a.Main_tran)) not in (select  a.entry_ty+ quotename(a.Tran_cd)  from #PTEPRCMTBL a LEFT OUTER JOIN shipto c ON (c.shipto_id = a.scons_id) where c.Supp_type in('import','sez','Eou','','Export')) and (a.ENTRY_ALL+ quotename(a.Main_tran)) not in (select  a.entry_ty+ quotename(a.Tran_cd)  from #BPRCM a LEFT OUTER JOIN AC_MAST  c ON (c.Ac_id  = a.Ac_id ) where c.Supp_type in('import','sez','Eou','','Export')))

insert into #NetITC values('A',@IGST_AMT,@SGST_AMT,@CGST_AMT,@CESS_AMT)

SET @CGST_AMT =0.00
SET @SGST_AMT =0.00
SET @IGST_AMT =0.00
SET @CESS_AMT =0.00
select @CGST_AMT = ISNULL(SUM(A.CGST_AMT),0),@SGST_AMT=ISNULL(SUM(A.SGST_AMT),0),@IGST_AMT=ISNULL(SUM(A.IGST_AMT),0),@CESS_AMT=ISNULL(SUM(A.COMPCESS),0)
from JVITEM a left outer join JVMAIN b on (a.entry_ty =b.entry_ty and a.Tran_cd =b.Tran_cd) WHERE A.entry_ty IN('J6' ,'J8') AND  (A.date BETWEEN @SDATE AND @EDATE)

insert into #NetITC values('A',@IGST_AMT,@SGST_AMT,@CGST_AMT,@CESS_AMT)

SET @CGST_AMT =0.00
SET @SGST_AMT =0.00
SET @IGST_AMT =0.00
SET @CESS_AMT =0.00
			
SELECT @IGST_AMT =(ISNULL(SUM(Case when A.mEntry in ('EP','PT','CN') then +IGST_AMT else -(IGST_AMT ) end),0)
				- ISNULL(SUM(Case when A.mEntry in ('EP','PT','CN') then +IIGST_AMT else -(IIGST_AMT ) end),0))
,@CGST_AMT =(ISNULL(SUM(Case when A.mEntry in ('EP','PT','CN') then +CGST_AMT else -(CGST_AMT ) end),0)
				- ISNULL(SUM(Case when A.mEntry in ('EP','PT','CN') then +ICGST_AMT else -(ICGST_AMT ) end),0))
,@SGST_AMT =(ISNULL(SUM(Case when A.mEntry in ('EP','PT','CN') then +SGST_AMT else -(SGST_AMT ) end),0)
				- ISNULL(SUM(Case when A.mEntry in ('EP','PT','CN') then +ISGST_AMT else -(ISGST_AMT ) end),0))
,@CESS_AMT =(ISNULL(SUM(Case when A.mEntry in ('EP','PT','CN') then +CESS_AMT else -(CESS_AMT ) end),0)
				- ISNULL(SUM(Case when A.mEntry in ('EP','PT','CN') then +ICESS_AMT else -(ICESS_AMT ) end),0))
FROM #rfd01TBL A LEFT OUTER JOIN EPAYMENT B ON (A.ENTRY_TY=B.ENTRY_TY AND A.TRAN_CD=B.TRAN_CD AND A.ITSERIAL=B.ITSERIAL)
WHERE A.mEntry IN ('EP','PT','CN','pr','dn') AND st_type IN('INTRASTATE','INTERSTATE') AND SUPP_TYPE in('Registered','Compounding','E-commerce') 
AND AGAINSTGS in('','PURCHASES','SERVICE PURCHASE BILL') and LineRule not in('Nill Rated','Nil Rated','Exempted') AND (CGSRT_AMT +SGSRT_AMT + IGSRT_AMT) = 0 
		 	
Select @CGST_AMT = @CGST_AMT + ISNULL(SUM(A.CGST_AMT),0),@SGST_AMT = @SGST_AMT + ISNULL(SUM(A.SGST_AMT),0)
,@IGST_AMT = @IGST_AMT + ISNULL(SUM(A.IGST_AMT),0),@CESS_AMT = @CESS_AMT + ISNULL(SUM(A.COMPCESS),0)
From JVMAIN A WHERE A.entry_ty ='J7' AND  A.AGAINSTTY in ('Excess','Addition') AND  (A.date BETWEEN @SDATE AND @EDATE)
  
insert into #NetITC values('A',@IGST_AMT,@SGST_AMT,@CGST_AMT,@CESS_AMT)

SET @CGST_AMT =0.00
SET @SGST_AMT =0.00
SET @IGST_AMT =0.00
SET @CESS_AMT =0.00
select @CGST_AMT = ISNULL(SUM(A.CGST_AMT),0),@SGST_AMT=ISNULL(SUM(A.SGST_AMT),0)
,@IGST_AMT=ISNULL(SUM(A.IGST_AMT),0),@CESS_AMT=ISNULL(SUM(A.COMPCESS),0) 
from JVMAIN A WHERE A.entry_ty ='J7' AND  (A.date BETWEEN @SDATE AND @EDATE) and A.RevsType in ('GST Rules 42 & 43','Others')  
AND  A.AGAINSTTY in ('Shortage','Reduction')

insert into #NetITC values('B',@IGST_AMT,@SGST_AMT,@CGST_AMT,@CESS_AMT)		

SET @CGST_AMT =0.00
SET @SGST_AMT =0.00
SET @IGST_AMT =0.00
SET @CESS_AMT =0.00
SELECT @CGST_AMT =isnull(sum(case when part = 'A' then +CGST_AMT else -CGST_AMT end),0)
,@SGST_AMT =isnull(sum(case when part = 'A' then +SGST_AMT else -SGST_AMT end),0)
,@IGST_AMT =isnull(sum(case when part = 'A' then +IGST_AMT else -IGST_AMT end),0)   
,@CESS_AMT =isnull(sum(case when part = 'A' then +compcess else -compcess end),0)
FROM #NetITC WHERE PART IN ('A','B') 

insert into #finalreftbl values(0.00,0.00,@CGST_AMT,@SGST_AMT,@IGST_AMT,@CESS_AMT,@IGST_AMT+@SGST_AMT+@CGST_AMT,@CESS_AMT,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00)
print 1	   
update #finalreftbl set taxvalue_a=(SELECT SUM((Case when mEntry in ('BR','CR','ST','DN','SB','ST') then +isnull(Taxableamt,0.00) else -isnull(Taxableamt,0.00) end) - isnull(ADJ_TAXABLE,0.00))
									FROM #rfd01TBL WHERE (mEntry IN ('BR','CR','ST','DN','SB','ST','SR','CN') or entry_ty in ('RV'))
		AND st_type IN('INTRASTATE','INTERSTATE') AND SUPP_TYPE not IN('SEZ','EXPORT','EOU','Import') 
		and LineRule not in('Nill Rated','Nil Rated','Exempted','Non GST') and AGAINSTGS in('','SALES','SERVICE INVOICE') 
		AND (IGST_AMT + CGST_AMT + SGST_AMT ) > 0 and entry_ty<>'UB' )
print 2
update #finalreftbl set taxvalue_Z=(SELECT SUM((Case when mEntry in ('BR','CR','ST','DN','SB','ST') then +isnull(Taxableamt,0.00) else -isnull(Taxableamt,0.00) end) - isnull(ADJ_TAXABLE,0.00))
		FROM #rfd01TBL WHERE mEntry IN ('BR','CR','ST','DN','SB','ST','SR','CN') and st_type IN('OUT OF COUNTRY','INTERSTATE','INTRASTATE')
		 and SUPP_TYPE IN('Export','SEZ','EOU','Import','') and AGAINSTGS IN('SALES', 'SERVICE INVOICE','')	and LineRule in ('Nill Rated','Nil Rated') 
		)

	--Fetching Ledger entries till End Date
	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'SELECT MVW.TRAN_CD,MVW.ENTRY_TY,MVW.DATE,AVW.AMOUNT,AVW.AMT_TY,AVW.ACSERIAL,'
	SET @SQLCOMMAND =@SQLCOMMAND+' MVW.INV_NO,LCODE.IOTRANS,MVW.U_CLDT ,' 
	SET @SQLCOMMAND =@SQLCOMMAND+'	AC_MAST.AC_ID,AC_MAST.AC_NAME,'
	SET @SQLCOMMAND =@SQLCOMMAND+'	AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,'
	SET @SQLCOMMAND =@SQLCOMMAND++	'AC_MAST.i_TAX,AC_MAST.[TYPE],AC_MAST.POSTING,OAC_NAME=SUBSTRING(AVW.OAC_NAME,1,4000),MVW.PINVNO,LCODE.SERIAL,LCODE.CODE_NM ,AC_TYPE_MAST.TYP ' 
	SET @SQLCOMMAND =@SQLCOMMAND+' ,MVW.AGAINSTGS'
	SET @SQLCOMMAND =@SQLCOMMAND+'	INTO '+@TBLNAME1+' FROM LAC_VW AVW (NOLOCK) '
	SET @SQLCOMMAND =@SQLCOMMAND+'	INNER JOIN AC_MAST (NOLOCK) ON AVW.AC_ID = AC_MAST.AC_ID '
	SET @SQLCOMMAND =@SQLCOMMAND+'	INNER JOIN LCODE (NOLOCK) ON AVW.ENTRY_TY = LCODE.ENTRY_TY '
	SET @SQLCOMMAND =@SQLCOMMAND+'	INNER JOIN VW_GST_MAIN MVW (NOLOCK) '  
	SET @SQLCOMMAND =@SQLCOMMAND+'	ON AVW.TRAN_CD = MVW.TRAN_CD AND AVW.ENTRY_TY = MVW.ENTRY_TY  '
	SET @SQLCOMMAND =@SQLCOMMAND+'	INNER JOIN AC_TYPE_MAST ON (AC_MAST.TYP=AC_TYPE_MAST.TYP) '
	SET @SQLCOMMAND =@SQLCOMMAND+'	LEFT JOIN AC_MAST ACM ON (ACM.AC_ID=MVW.AC_ID) '
	SET @SQLCOMMAND =@SQLCOMMAND+'	WHERE  '
	SET @SQLCOMMAND =@SQLCOMMAND+'	AC_TYPE_MAST.TYP IN (''CGST Payable'',''SGST Payable'',''IGST Payable'',''CGST Provisional'',''SGST Provisional'',''IGST Provisional'',''IGST ITC'',''CGST ITC'',''SGST ITC'',''COMP CESS PAYABLE'',''CCess Provisional'',''CCess ITC'')  '
	SET @SQLCOMMAND =@SQLCOMMAND+'  AND (CASE WHEN YEAR(MVW.U_CLDT)>2000 THEN MVW.U_CLDT ELSE MVW.DATE END) <= '+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39)

	PRINT @SQLCOMMAND	
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'SELECT 
					    CGST_ITC1=Isnull(Sum(Case When TVW.TYP IN(''CGST ITC'',''CGST Provisional'',''Input CGST'') and TVW.Amt_Ty =''DR'' Then TVW.AMOUNT When TVW.TYP IN(''CGST ITC'',''CGST Provisional'',''Input CGST'')  AND TVW.Amt_Ty=''CR'' Then -TVW.AMOUNT Else 0 end),0) 
						,SGST_ITC1=Isnull(Sum(Case When TVW.TYP IN(''SGST ITC'',''SGST Provisional'',''Input SGST'') and TVW.Amt_Ty =''DR'' Then TVW.AMOUNT When TVW.TYP IN(''SGST ITC'',''SGST Provisional'',''Input SGST'')  AND TVW.Amt_Ty=''CR'' Then -TVW.AMOUNT Else 0 end),0) 
						,IGST_ITC1=Isnull(Sum(Case When TVW.TYP IN(''IGST Provisional'',''IGST ITC'',''Input IGST'') and TVW.Amt_Ty =''DR'' Then TVW.AMOUNT When TVW.TYP IN(''IGST Provisional'',''IGST ITC'',''Input IGST'') AND TVW.Amt_Ty=''CR'' Then - TVW.AMOUNT Else 0 end),0)  
						,CCESS_ITC1=Isnull(Sum(Case When TVW.TYP IN(''CCess Provisional'',''CCess ITC'',''Input COMP CESS'') and TVW.Amt_Ty =''DR'' Then TVW.AMOUNT When TVW.TYP IN(''CCess Provisional'',''CCess ITC'',''Input COMP CESS'') AND TVW.Amt_Ty=''CR'' Then - TVW.AMOUNT Else 0 end),0)      
	     into '+@TBLNAME2+' FROM '+@TBLNAME1+' TVW  '	
		 PRINT @SQLCOMMAND	
	     EXECUTE SP_EXECUTESQL @SQLCOMMAND


	SET @SQLCOMMAND = 'select isnull(taxvalue_z,0.00) as taxvalue_z,isnull(taxvalue_a,0.00) as taxvalue_a,igst_itc,sgst_itc,cgst_itc,ccess_itc,NetITC_ISC,NetITC_CES'
	SET @SQLCOMMAND =@SQLCOMMAND+' ,cgst_itc1=(select sum(CGST_ITC1) from '+@TBLNAME2+' ) '
	SET @SQLCOMMAND =@SQLCOMMAND+' ,sgst_itc1=(select sum(SGST_ITC1) from '+@TBLNAME2+' ) '
	SET @SQLCOMMAND =@SQLCOMMAND+' ,igst_itc1=(select sum(IGST_ITC1) from '+@TBLNAME2+' ) '
	SET @SQLCOMMAND =@SQLCOMMAND+' ,cess_itc1=(select sum(CCESS_ITC1) from '+@TBLNAME2+' ) '
	SET @SQLCOMMAND =@SQLCOMMAND+' ,cgst_itc2=(select sum(CGST_ITC1) from '+@TBLNAME2+' ) '
	SET @SQLCOMMAND =@SQLCOMMAND+' ,sgst_itc2=(select sum(SGST_ITC1) from '+@TBLNAME2+' ) '
	SET @SQLCOMMAND =@SQLCOMMAND+' ,igst_itc2=(select sum(IGST_ITC1) from '+@TBLNAME2+' ) '
	SET @SQLCOMMAND =@SQLCOMMAND+' ,cess_itc2=(select sum(CCESS_ITC1) from '+@TBLNAME2+' ) '
	SET @SQLCOMMAND =@SQLCOMMAND+' FROM #finalreftbl '
	print @SQLCOMMAND
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	
	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME2
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
	
drop table #rfd01TBL1
drop table #rfd01TBL
drop table #finalreftbl
drop table #NetITC
drop table #PTEPRCMTBL
drop table #BPRCM		
End



