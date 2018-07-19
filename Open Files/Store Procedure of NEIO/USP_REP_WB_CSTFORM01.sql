IF EXISTS(SELECT XTYPE FROM SYSOBJECTS WHERE XTYPE='P' AND name ='USP_REP_WB_CSTFORM01')
BEGIN
 DROP PROCEDURE USP_REP_WB_CSTFORM01
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXECUTE USP_REP_WB_CSTFORM01'','','','04/01/2013','03/31/2016','','','','',0,0,'','','','','','','','','2015-2016',''

-- =============================================
 -- Author:  G.Prashanth Reddy
 -- Create date: 28/07/2012 
 -- Description: This Stored procedure is useful to generate WB CST Form1
 -- Modify date: 
 -- Modified By: Sumit gavate for Bug - 27186
 -- Modify date: 05-01-2016
 -- =============================================
 CREATE PROCEDURE [dbo].[USP_REP_WB_CSTFORM01]
 @TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
 ,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
 ,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
 ,@SAMT FLOAT,@EAMT FLOAT
 ,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
 ,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
 ,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
 ,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
 ,@LYN VARCHAR(20)
 ,@EXPARA  AS VARCHAR(60)= null
 AS
 BEGIN
 Declare @FCON as NVARCHAR(2000),@VSAMT DECIMAL(14,2),@VEAMT DECIMAL(14,2)
 EXECUTE   USP_REP_FILTCON 
 @VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
 ,@VSDATE=NULL
 ,@VEDATE=@EDATE
 ,@VSAC =@SAC,@VEAC =@EAC
 ,@VSIT=@SIT,@VEIT=@EIT
 ,@VSAMT=@SAMT,@VEAMT=@EAMT
 ,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
 ,@VSCATE =@SCATE,@VECATE =@ECATE
 ,@VSWARE =@SWARE,@VEWARE  =@EWARE
 ,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
 ,@VMAINFILE='M',@VITFILE=NULL,@VACFILE=NULL
 ,@VDTFLD ='DATE'
 ,@VLYN=NULL
 ,@VEXPARA=@EXPARA
 ,@VFCON =@FCON OUTPUT
 
 DECLARE @SQLCOMMAND NVARCHAR(4000)
 DECLARE @AMTA1 NUMERIC(12,2),@AMTB1 NUMERIC(12,2),@AMTC1 NUMERIC(12,2),@AMTD1 NUMERIC(12,2),@AMTE1 NUMERIC(12,2),@AMTF1 NUMERIC(12,2),@AMTG1 NUMERIC(12,2),@AMTH1 NUMERIC(12,2),@AMTI1 NUMERIC(12,2),@AMTJ1 NUMERIC(12,2),@AMTK1 NUMERIC(12,2),@AMTL1 NUMERIC(12,2),@AMTM1 NUMERIC(12,2),@AMTN1 NUMERIC(12,2),@AMTO1 NUMERIC(12,2)
 DECLARE @AMTA2 NUMERIC(12,2),@AMTB2 NUMERIC(12,2),@AMTC2 NUMERIC(12,2),@AMTD2 NUMERIC(12,2),@AMTE2 NUMERIC(12,2),@AMTF2 NUMERIC(12,2),@AMTG2 NUMERIC(12,2),@AMTH2 NUMERIC(12,2),@AMTI2 NUMERIC(12,2),@AMTJ2 NUMERIC(12,2),@AMTK2 NUMERIC(12,2),@AMTL2 NUMERIC(12,2),@AMTM2 NUMERIC(12,2),@AMTN2 NUMERIC(12,2),@AMTO2 NUMERIC(12,2)
 DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@CHAR INT
 DECLARE @AMTA11 NUMERIC(12,2),@AMTA111 NUMERIC(12,2),@AMTA22 NUMERIC(12,2),@AMTA33 NUMERIC(12,2),@AMTA222 NUMERIC(12,2),@AMTA333 NUMERIC(12,2),@AMTA3 NUMERIC(12,2),@AMTA4 NUMERIC(12,2),@AMTA44 NUMERIC(12,2)
SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
INSERT INTO #VATAC_MAST SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
 

--Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)
----Temporary Cursor1
--SELECT BHENT='PT',M.INV_NO,M.Date,A.AC_NAME,A.AMT_TY,STM.TAX_NAME,SET_APP=ISNULL(SET_APP,0),STM.ST_TYPE,M.NET_AMT,M.GRO_AMT,TAXONAMT=M.GRO_AMT+M.TOT_DEDUC+M.TOT_TAX+M.TOT_EXAMT+M.TOT_ADD,PER=STM.LEVEL1,MTAXAMT=M.TAXAMT,TAXAMT=A.AMOUNT,STM.FORM_NM,PARTY_NM=AC1.AC_NAME,AC1.S_TAX,M.U_IMPORM
--,ADDRESS=LTRIM(AC1.ADD1)+ ' ' + LTRIM(AC1.ADD2) + ' ' + LTRIM(AC1.ADD3),M.TRAN_CD,VATONAMT=99999999999.99,Dbname=space(20),ItemType=space(1),It_code=999999999999999999-999999999999999999,ItSerial=Space(5)
--INTO #FORM221_1
--FROM PTACDET A 
--INNER JOIN PTMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
--INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
--INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
--INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
--WHERE 1=2 --A.AC_NAME IN ( SELECT AC_NAME FROM #VATAC_MAST)

--alter table #form221_1 add recno int identity

---Temporary Cursor2
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),STM.FORM_NM,AC1.S_TAX,AMT4=M.TaxAmt
INTO #FORM221
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2

Declare @MultiCo	VarChar(3)
Declare @MCON as NVARCHAR(2000)
IF Exists(Select A.ID From SysObjects A Inner Join SysColumns B On(A.ID = B.ID) Where A.[Name] = 'STMAIN' And B.[Name] = 'DBNAME')
	Begin	------Fetch Records from Multi Co. Data
		 Set @MultiCo = 'YES'
		 EXECUTE USP_REP_MULTI_CO_DATA
		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		 ,@MFCON = @MCON OUTPUT

		--SET @SQLCOMMAND='Select * from '+@MCON
		---EXECUTE SP_EXECUTESQL @SQLCOMMAND
		SET @SQLCOMMAND='Insert InTo  #FORM221_1 Select * from '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		---Drop Temp Table 
		SET @SQLCOMMAND='Drop Table '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
	End
else
	Begin ------Fetch Single Co. Data
		 Set @MultiCo = 'NO'
		 EXECUTE USP_REP_SINGLE_CO_DATA_VAT
		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		 ,@MFCON = @MCON OUTPUT

		--SET @SQLCOMMAND='Select * from '+@MCON
		---EXECUTE SP_EXECUTESQL @SQLCOMMAND
		--SET @SQLCOMMAND='Insert InTo  #FORM221_1 Select * from '+@MCON
		--EXECUTE SP_EXECUTESQL @SQLCOMMAND
		---Drop Temp Table 
		--SET @SQLCOMMAND='Drop Table '+@MCON
		--EXECUTE SP_EXECUTESQL @SQLCOMMAND
	End
-----
-----SELECT * from #form221_1 where (Date Between @Sdate and @Edate) and Bhent in('EP','PT','CN') and TAX_NAME In('','NO-TAX') and U_imporm = ''
-----

--Gross Sales
-- 1. Transfer of goods otherwise than by way of sales as referred to in section 6A the Act
--1) (a) Value of goods transferred to the assessee's place of principal in other States[Details to be furnished in Annexure A]
Select @AMTA1 = 0
Select @AMTA1 = ISNULL(Sum(VATONAMT),0) From VATTBL where (Date Between @Sdate and @Edate) And Bhent = 'ST' AND
 U_imporm in('Branch Transfer') AND ST_TYPE='OUT OF STATE'
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'1','A',0,@AMTA1,0,0,'')

--1) (b) Value of goods transferred to the assessee's agent or principal in other States (Details to be furnished in Annexure A )
Select @AMTA1 = 0
Select @AMTA1 = ISNULL(Sum(VATONAMT),0) From VATTBL where (Date Between @Sdate and @Edate) And Bhent = 'ST' AND
 U_imporm in('Consignment Transfer') AND ST_TYPE='OUT OF STATE'
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'1','B',0,@AMTA1,0,0,'')

--2) Gross amount payable to the dealer as consideration for the sales of goods made during the above period in respect to
-- 2) (a) Sales in the course of Inter-State Trade or Commerce including all sales u/s 2(g)
Select @AMTA1 = 0,@PER = 0
SELECT @AMTA1 = ISNULL(Sum(gro_Amt),0) FROM VATTBL A INNER JOIN IT_MAST I ON (A.It_code = I.It_code) WHERE A.Bhent in('ST') AND I.It_Name <> 'ATF'
AND (A.DATE BETWEEN @SDATE AND @EDATE) AND A.ST_TYPE in('OUT OF STATE') AND A.U_imporm in('Sales in the course of trade or commerce u/s 2(g)')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'2','A',0,@AMTA1,0,0,'')  

-- 2) (b) Sales of goods outside the State ( as provided in section 4 of the Act )
Select @AMTA1 = 0
SELECT @AMTA1 = ISNULL(Sum(gro_Amt),0) FROM VATTBL A INNER JOIN IT_MAST I ON (A.It_code = I.It_code) WHERE 
A.Bhent in('ST') AND (A.DATE BETWEEN @SDATE AND @EDATE) AND I.It_Name <> 'ATF' AND 
A.ST_TYPE in('OUT OF STATE') AND A.U_imporm not in('Consignment Transfer','Branch Transfer','Sales in the course of trade or commerce u/s 2(g)','Under Section 8(2)')
AND A.U_imporm NOT in('Sales of tax free goods u/s 21','Under Section 21','Under Section 24','Under Section 8(1)','Under Section 8(5)')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'2','B',0,@AMTA1,0,0,'')  

--2) (c) Sales of goods in the course of export outside India [ as provided in section 5(1) of the Act ]
Select @AMTA1 = 0
Select @AMTA1 = ISNULL(Sum(A.Gro_Amt),0) From VATTBL A INNER JOIN IT_MAST IT ON (A.It_code = IT.It_code) 
INNER JOIN STmain m on (m.entry_ty = A.bhent and m.tran_cd = A.tran_cd) where (A.Date Between @Sdate and @Edate)
And A.Bhent in('ST') And A.St_Type = 'OUT OF COUNTRY' And A.U_IMPORM IN('Export Out of India','Direct Exports','High Sea Sales') 
AND LTRIM(RTRIM(REPLACE(REPLACE(A.RFORM_NM,'-',''),'FORM',''))) = 'H' AND m.VATMTYPE NOT like '%Sale in the course of import into India%'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'2','C',0,@AMTA1,0,0,'') 
 
--2) (d) Sales of goods in the course of import into India [ as provided in section 5(2) of the Act ]
SET @AMTA1 = 0
Select @AMTA1 = ISNULL(Sum(i.gro_Amt),0) From VATTBL i INNER JOIN STmain m on (m.entry_ty=i.bhent and m.tran_cd=i.tran_cd) 
INNER JOIN IT_MAST IT ON (i.It_code = IT.It_code) where (i.Date Between @Sdate and @Edate) And i.Bhent in('ST') And i.St_Type = 'OUT OF COUNTRY'
AND IT.It_Name <> 'ATF' And m.VATMTYPE like '%Sale in the course of import into India%' AND LTRIM(RTRIM(REPLACE(REPLACE(i.RFORM_NM,'-',''),'FORM',''))) = 'H'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'2','D',0,@AMTA1,0,0,'') 
 
--2) (e) Last sales of goods preceding the sales occasioning the export of those goods outside
--   India [ as provided in section 5(3) of the Act ] ( Details to be furnished in annexure B )
SET @AMTA1 = 0
SELECT @AMTA1 = ISNULL(Sum(A.VATONAMT),0) FROM VATTBL A WHERE A.ST_TYPE='OUT OF STATE' AND 
A.BHENT='ST' AND LTRIM(RTRIM(REPLACE(REPLACE(RFORM_NM,'-',''),'FORM','')))='H' AND (A.Date Between @Sdate and @Edate)
--Select @AMTA1 = ISNULL(Sum(i.Gro_Amt),0) From VATTBL i INNER JOIN STmain m on (m.entry_ty=i.bhent and m.tran_cd=i.tran_cd)
--INNER JOIN IT_MAST IT ON (i.It_code = IT.It_code) where (i.Date Between @Sdate and @Edate) And i.Bhent in('ST') AND IT.It_Name <> 'ATF'
--And i.St_Type = 'OUT OF COUNTRY' And m.VATMTYPE like '%Sale occasioning the export of goods outside India%'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'2','E',0,@AMTA1,0,0,'') 

--2) (f) Sales of Aviation Turbine Fuel to designated Indian Carriers [ as provided in section 5(5) of the Act ]
Select @AMTA1 = 0
Select @AMTA1 = ISNULL(Sum(A.gro_Amt),0) From VATTBL A INNER JOIN IT_MAST I ON (A.It_code = I.It_code) where (A.Date Between @Sdate and @Edate)
And A.Bhent in('ST') AND I.It_Name = 'ATF' AND A.U_imporm not in('Consignment Transfer','Branch Transfer') AND ST_TYPE <> 'LOCAL'
AND A.U_imporm NOT IN('Sales of tax free goods u/s 21','Under Section 21','Under Section 24','Under Section 8(1)','Under Section 8(5)','Under Section 8(2)')
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'2','F',0,@AMTA1,0,0,'')

--2) (g) Sales effected at place inside West Bengal
Select @AMTA1 = 0
SELECT @AMTA1 = ISNULL(Sum(A.gro_Amt),0) From VATTBL A WHERE  A.Bhent in('ST') AND
(A.DATE BETWEEN @SDATE AND @EDATE) AND A.ST_TYPE in('LOCAL')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'2','G',0,@AMTA1,0,0,'')  

--2) Total : 2( (a) + (b) + (c) + (d) + (e) + (f) + (g) )
Select @AMTA1 = 0,@AMTA2 = 0
SELECT @AMTA1 = ISNULL(SUM(AMT1),0),@AMTA2 = ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '2' and SRNO In ('A','B','C','D','E','F','G')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'2','H',0,@AMTA1,@AMTA2,0,'')  

--Part-3
--3) Gross amount payable to the dealer as consideration for the sales of goods made in the
--   course of Inter-State Trade or Commerce as per SL. No. 2(a) during the above period
Select @AMTA1 = 0
SELECT @AMTA1 = ISNULL(Sum(A.gro_Amt),0) From VATTBL A inner join it_mast d on (a.it_code=d.it_code)
WHERE (A.BHENT in('ST')) AND (A.DATE BETWEEN @SDATE AND @EDATE) and a.ST_TYPE='OUT OF STATE' AND D.It_name <> 'ATF' AND
a.U_imporm in('Sales in the course of trade or commerce u/s 2(g)')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'3','A',0,@AMTA1,0,0,'')  

--Part-4
--4) Less :
-- (a) Cost of freight,delivery or installation separately charged on customers and includend in SL. No. 3
SELECT @AMTA1 = 0
SELECT @AMTA1 = ISNULL(Sum(A.U_FRTAMT),0) FROM STITEM A WHERE A.Date Between @SDATE And @EDATE
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'4','A',0,@AMTA1,0,0,'')

--(b) Cash discount allowed according to the practice normally prevailing in the trade and included in SL. No. 3
SELECT @AMTA1 = 0
SELECT @AMTA1 = ISNULL(Sum(A.ITDISCAMT),0) FROM STITEM A WHERE A.Date Between @SDATE And @EDATE
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'4','B',0,@AMTA1,0,0,'')

--(c) Sale price refunded to purchasers in respect of goods returned by them according to
--   section 8A(1)(b) of the Act.[Statement to be furnished in Annexure Sales Return (CST)]
Select @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(A.VATONAMT),0) FROM 
(SELECT Date,INV_no,BHENT,Tran_cd,VATONAMT,TAXAMT  from VATTBL where St_Type = 'OUT OF STATE') A INNER JOIN JVMAIN B
ON (A.BHENT = B.Entry_ty AND A.tran_Cd=b.tran_cd ) WHERE B.VAT_ADJ ='Sale price refunded to purchasers' AND A.BHENT='J4'
AND (A.DATE BETWEEN @SDATE AND @EDATE)
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'4','C',0,@AMTA1,0,0,'')

-- Total of 4(a), 4(b) and 4(c) :
Select @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PARTSR = '4' and SRNO In ('A','B','C')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'4','D',0,@AMTA1,0,0,'')

--5) Balance (3-4): total turnover or inter-State sales.
Select @AMTA1 = 0,@AMTA2 = 0,@AMTA11=0,@AMTA22 = 0
SELECT @AMTA1 = ISNULL(SUM(AMT1),0),@AMTA11 = ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '3' and SRNO In ('A')
SELECT @AMTA2 = ISNULL(SUM(AMT1),0),@AMTA22 = ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '4' and SRNO In ('D')
SET @AMTA111 = ISNULL(@AMTA1-@AMTA2,0)
SET @AMTA222 = ISNULL(@AMTA11-@AMTA22,0)
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'4','E',0,@AMTA111,@AMTA222,0,'')  

--Part-5
-- 6) Deduct :
--(a) Sale prices received or receivable by the dealer in respect of subsequent sales under
--   section 6(2) of the Act against prescribed certificate(s) and included in SL. No 5
--   [Details to be furnished in Annexure E & Annexure G]
Select @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(A.VATONAMT),0) FROM VATTBL A WHERE A.ST_TYPE='OUT OF STATE' AND A.BHENT='ST' AND 
(A.Tax_name like '%C.S.T%' OR A.Tax_name like '%CST%') AND A.U_IMPORM in ('Under Section 6(2)','U/S 6(2)')
AND (A.DATE BETWEEN @SDATE AND @EDATE)
--SELECT @AMTA1 = ISNULL(SUM(GRO_AMT),0) FROM VATTBL A WHERE (A.BHENT in ('ST')) AND 
--(A.DATE BETWEEN @SDATE AND @EDATE) AND a.U_Imporm = 'Under Section 6(2)' AND A.St_Type = 'OUT OF STATE'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'5','A',0,@AMTA1,0,0,'') 

--(b) Sale prices received or receivable by the dealer in respect of sales to any official,
--    personnel, consular or diplomatic agent of any foreign diplomatic mission or consulate
--    in India or the United Nations or any other similar international body under section 6(3)
--    of the Act and included in Sl. No. 5
Select @AMTA1 = 0
Select @AMTA1 = ISNULL(Sum(GRO_AMT),0) From VATTBL where (Date Between @Sdate and @Edate) And Bhent = 'ST' And
St_Type = 'OUT OF STATE' AND U_Imporm = 'Under Section 6(3)' AND LTRIM(RTRIM(REPLACE(REPLACE(RFORM_NM,'-',''),'FORM',''))) = 'J'
AND (DATE BETWEEN @SDATE AND @EDATE)
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'5','B',0,@AMTA1,0,0,'')

--(c) Sale prices received or receivable by the dealer in respect of sales made to a registered
--   dealer of special economic zone under section 8(6) of the Act and included in Sl. No. 5
--  ( Details to be furnished in Annexure C )
SELECT @AMTA1 = ISNULL(SUM(e.gro_amt),0) FROM VATTBL A
Inner Join VATITEM_VW e On(A.Bhent = e.Entry_ty And A.Tran_cd = e.Tran_cd and a.it_code=e.it_code and a.itserial=e.itserial)
INNER JOIN VATMAIN_VW H ON A.BHENT=H.ENTRY_TY AND A.TRAN_CD=H.TRAN_cD INNER JOIN AC_MAST SAC ON SAC.AC_ID =H.AC_ID 
WHERE SAC.ST_TYPE='OUT OF STATE' AND A.BHENT='ST'  AND SAC.C_TAX <> '' AND (A.DATE BETWEEN @SDATE AND @EDATE) 
AND [RULE] IN ('REBATE','CT-3','CT-1','LUT') AND LTRIM(RTRIM(REPLACE(REPLACE(A.RFORM_NM,'-',''),'FORM','')))='I'
--Select @AMTA1 = ISNULL(Sum(GRO_AMT),0) From VATTBL where (Date Between @Sdate and @Edate) And Bhent = 'ST' And St_Type = 'OUT OF STATE'
--AND U_Imporm = 'Special economic zone under section 8(6)' AND S_TAX <> '' AND LTRIM(RTRIM(REPLACE(REPLACE(RFORM_NM,'-',''),'FORM',''))) = 'I'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'5','C',0,@AMTA1,0,0,'')

--(d) Sale prices received or receivable by the dealer in respect of sales exempt under section
--    8(5) of the Act and included in Sl. No. 5 [Details to be furnished in Annexure E & Annexure G]
--Select @AMTA1 = 0
--SELECT @AMTA1 = ISNULL(SUM(A.VATONAMT),0) FROM VATTBL A INNER JOIN IT_MAST ON (A.IT_CODE=IT_MAST.IT_CODE) INNER JOIN AC_MAST SAC ON SAC.AC_ID =A.AC_ID
--inner join STITEM ST on (A.Bhent = ST.Entry_ty And A.Tran_cd = ST.Tran_cd and a.it_code=ST.it_code and a.itserial=ST.itserial)  
--WHERE SAC.ST_TYPE='OUT OF STATE' AND A.BHENT='ST' AND (A.Tax_name like '%C.S.T%' OR A.Tax_name like '%CST%') 
--AND A.U_IMPORM in ('Under Section 6(2)','U/S 6(2)') AND (A.DATE BETWEEN @SDATE AND @EDATE)
SELECT @AMTA1 = ISNULL(SUM(GRO_AMT),0) FROM VATTBL WHERE (BHENT='ST') AND (DATE BETWEEN @SDATE AND @EDATE) AND
ST_TYPE='OUT OF STATE' AND U_Imporm = 'Sales exempt under section 8(5)' and tax_name IN('EXEMPTED','')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'5','D',0,@AMTA1,0,0,'')

--(e) Any other sales eligible for exemption and included in SL. No 5
Select @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(GRO_AMT),0) FROM VATTBL WHERE (BHENT='ST') AND 
(DATE BETWEEN @SDATE AND @EDATE) and ST_TYPE='OUT OF STATE' AND U_Imporm = 'Sales exempt under section 8(5)' and tax_name = ''
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'5','E',0,@AMTA1,0,0,'')

-- (f) Total : 6 [ (a) + (b) + (c) + (d) + (e) ]
Select @AMTA1 = 0,@AMTA2 = 0
SELECT @AMTA1 = ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PARTSR = '5' and SRNO In ('A','B','C','D','E')
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'5','F',0,@AMTA1,0,0,'')  

--7) Balance 'X' :- [ (5) - (6)(f)]
Select @AMTA1 = 0,@AMTA2 = 0,@AMTA11=0,@AMTA22=0
SELECT @AMTA1 = ISNULL(SUM(AMT1),0),@AMTA11 = ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '4' and SRNO In ('E')
SELECT @AMTA2 = ISNULL(SUM(AMT1),0),@AMTA22 = ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '5' and SRNO In ('F')
SET @AMTA111 = ISNULL(@AMTA1-@AMTA2,0)
SET @AMTA222 = ISNULL(@AMTA11-@AMTA22,0)
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'5','G',0,@AMTA111,@AMTA222,0,'')  

--Part-6
--8)(a) Sales in the course of inter-State trade or commerce of goods, the sales of which are free of tax, under section 21 of
--the West Bengal Value Added Tax Act, 2003 or under section 24 of the West Bengal Sales Tax Act, 1994
Select @AMTA1 = 0,@AMTA2 = 0
SELECT @AMTA1=ISNULL(SUM(VATONAMT),0),@AMTA2=ISNULL(SUM(TAXAMT),0) FROM VATTBL A WHERE (A.BHENT='ST') AND (A.DATE BETWEEN @SDATE AND @EDATE) AND
A.ST_TYPE='OUT OF STATE'  AND A.U_imporm in('Sales of tax free goods u/s 21','Under Section 21','Under Section 24') AND
A.Tax_name = ''
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'6','A',0,@AMTA1,@AMTA2,0,'') 

--8)(b) Sales of goods referred to in sub-section (1) of section 8 of the Act to registered dealers against
--     prescribed declaration forms (Details to the furnished in Annexure D)
Select @AMTA1 = 0,@AMTA2 = 0
SELECT @AMTA1 = ISNULL(SUM(A.gro_amt),0) FROM VATTBL A
Inner Join VATITEM_VW e On(A.Bhent = e.Entry_ty And A.Tran_cd = e.Tran_cd and a.it_code=e.it_code and a.itserial=e.itserial)
INNER JOIN VATMAIN_VW H ON A.BHENT=H.ENTRY_TY AND A.TRAN_CD=H.TRAN_cD INNER JOIN AC_MAST SAC ON SAC.AC_ID = H.AC_ID WHERE
SAC.ST_TYPE='OUT OF STATE' AND A.BHENT='ST' AND SAC.C_TAX <> '' AND LTRIM(RTRIM(REPLACE(REPLACE(A.RFORM_NM,'-',''),'FORM','')))='C'
AND (A.DATE BETWEEN @SDATE AND @EDATE)
--Select @AMTA1=ISNULL(SUM(VATONAMT),0),@AMTA2=ISNULL(SUM(TAXAMT),0) From VATTBL A where (Date Between @Sdate and @Edate) And 
--Bhent = 'ST' And s_tax <> '' and st_type = 'OUT OF STATE' AND U_imporm = 'Under Section 8(1)'
--AND LTRIM(RTRIM(REPLACE(REPLACE(RFORM_NM,'-',''),'FORM','')))='C'
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'6','B',0,@AMTA1,@AMTA2,0,'')

-- 8)(b)(i) Taxable at the rate of per centum
SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
SET @CHAR=65
DECLARE  CUR_FORM01 CURSOR FOR 
select distinct level1 from stax_mas where ST_TYPE='Out of State'--CHARINDEX('VAT',TAX_NAME)>0
OPEN CUR_FORM01
FETCH NEXT FROM CUR_FORM01 INTO @PER
WHILE (@@FETCH_STATUS=0)
BEGIN
	if @per = 0
		SELECT @AMTA1 = ISNULL(SUM(A.VATONAMT),0),@AMTB1=ISNULL(SUM(A.TAXAMT),0) FROM VATTBL A 
        Inner Join VATMAIN_VW c On(A.Bhent = c.Entry_Ty And A.Tran_cd = c.Tran_cd)
        INNER JOIN AC_MAST AC ON (AC.AC_ID=c.AC_ID) 
        WHERE AC.ST_TYPE='Out of State' AND A.BHENT='ST' AND A.PER=@PER AND AC.C_TAX <> '' AND 
        (A.DATE BETWEEN @SDATE AND @EDATE) and A.Tax_name like '%Margin%' AND A.U_Imporm = 'Under Section 8(1)'
	else
		SELECT @AMTA1=ISNULL(SUM(A.VATONAMT),0),@AMTB1=ISNULL(SUM(A.TAXAMT),0) FROM VATTBL A 
        Inner Join VATMAIN_VW c On(A.Bhent = c.Entry_Ty And A.Tran_cd = c.Tran_cd)
        INNER JOIN AC_MAST AC ON (AC.AC_ID=c.AC_ID)
        WHERE AC.ST_TYPE='Out of State' AND A.U_Imporm = 'Under Section 8(1)' AND A.BHENT='ST' AND  
        A.PER=@PER AND AC.C_TAX <> '' AND (A.DATE BETWEEN @SDATE AND @EDATE)
        AND LTRIM(RTRIM(REPLACE(REPLACE(A.RFORM_NM,'-',''),'FORM','')))='C'
    if @AMTA1 <> 0
    BEGIN
		INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) 
		VALUES(1,'7',CHAR(@CHAR),@PER,@AMTA1,@AMTB1,0,'Taxable at the rate of') 
	
		SET @AMTJ1 = ISNULL(@AMTJ1+@AMTA1,0) --TOTAL TAXABLE AMOUNT
		SET @AMTK1 = ISNULL(@AMTK1+@AMTB1,0) --TOTAL TAX
	End
	
	SET @CHAR=@CHAR+1
	FETCH NEXT FROM CUR_FORM01 INTO @PER
END
CLOSE CUR_FORM01
DEALLOCATE CUR_FORM01
SET @AMTJ1=ISNULL(@AMTJ1,0)
SET @AMTK1=ISNULL(@AMTK1,0)

Select @AMTA1 = 0,@AMTA2 = 0
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES (1,'7','Z',0,ISNULL(@AMTJ1,0),@AMTK1,0,'Total')

--SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
--SET @CHAR=65
--DECLARE  CUR_FORM01 CURSOR FOR 
--select distinct level1 from stax_mas where ST_TYPE='Out of State'--CHARINDEX('VAT',TAX_NAME)>0
--OPEN CUR_FORM01
--FETCH NEXT FROM CUR_FORM01 INTO @PER
--WHILE (@@FETCH_STATUS=0)
--BEGIN
--	if @per = 0
--		SELECT @AMTA1=SUM(NET_AMT),@AMTB1=SUM(TAXAMT) FROM #Form221_1 WHERE ST_TYPE='Out of State' AND BHENT in ('ST') AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%'
--	else
--		SELECT @AMTA1=SUM(Net_AMT),@AMTB1=SUM(TAXAMT) FROM #Form221_1 WHERE ST_TYPE='Out of State' AND BHENT in ('ST')  AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE)
	
--	SET @AMTA1=ISNULL(@AMTA1,0)
--	SET @AMTB1=ISNULL(@AMTB1,0)

--	INSERT INTO #FORM221
--	(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--	(1,'7',CHAR(@CHAR),@PER,@AMTA1-@AMTB1,@AMTB1,0,'Taxable at the rate of') 
	
--	SET @AMTJ1=@AMTJ1+@AMTA1 --TOTAL TAXABLE AMOUNT
--	SET @AMTK1=@AMTK1+@AMTB1 --TOTAL TAX
	
--	SET @CHAR=@CHAR+1
--	FETCH NEXT FROM CUR_FORM01 INTO @PER
--END
--CLOSE CUR_FORM01
--DEALLOCATE CUR_FORM01
--SET @AMTJ1=ISNULL(@AMTJ1,0)
--SET @AMTK1=ISNULL(@AMTK1,0)


--INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES (1,'7','Z',0,@AMTJ1-@AMTK1,@AMTK1,0,'Total')
--Part-8
--Select @AMTA1=Round(Sum(Net_Amt),0) From #Form221_1 where (Date Between @Sdate and @Edate) And Bhent = 'ST' And St_Type = 'Out of Country' 
--Set @AMTA1 = ISNULL(@AMTA1,0)
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'8','',0,@AMTA1,0,0,'')

--8)(c) Sales of goods notified under sub-section (5) of section 8 of
--    the Act not included in any other item-
-- 8)(c)(i) Taxable at the rate of per centum
SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
SET @CHAR=65
DECLARE  CUR_FORM01 CURSOR FOR 
select distinct level1 from stax_mas where ST_TYPE='Out of State'--CHARINDEX('VAT',TAX_NAME)>0
OPEN CUR_FORM01
FETCH NEXT FROM CUR_FORM01 INTO @PER
WHILE (@@FETCH_STATUS=0)
BEGIN
	if @per = 0
		SELECT @AMTA1=ISNULL(Sum(VATONAMT),0),@AMTB1=ISNULL(SUM(TAXAMT),0) FROM VATTBL WHERE ST_TYPE='Out of State' AND
		BHENT='ST' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' AND U_Imporm = 'Under Section 8(5)'
	else
		SELECT @AMTA1=ISNULL(Sum(VATONAMT),0),@AMTB1=ISNULL(SUM(TAXAMT),0) FROM VATTBL WHERE ST_TYPE='Out of State' AND
		BHENT='ST' AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) AND U_Imporm = 'Under Section 8(5)'
	if @AMTA1 <> 0
    BEGIN
		INSERT INTO #FORM221
		(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
		(1,'8',CHAR(@CHAR),@PER,@AMTA1,@AMTB1,0,'Taxable at the rate of') 
		
		SET @AMTJ1 = ISNULL(@AMTJ1+@AMTA1,0) --TOTAL TAXABLE AMOUNT
		SET @AMTK1 = ISNULL(@AMTK1+@AMTB1,0) --TOTAL TAX
	END
	SET @CHAR=@CHAR+1
	FETCH NEXT FROM CUR_FORM01 INTO @PER
END
CLOSE CUR_FORM01
DEALLOCATE CUR_FORM01
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES (1,'8','Z',0,@AMTJ1,@AMTK1,0,'Total')
---------------------------------------------------------------------------------------------------------------------
--Part-9
--Select @AMTA1=Round(Sum(Net_Amt),0) From #Form221_1 where (Date Between @Sdate and @Edate) And Bhent = 'ST' And St_Type = 'Out of Country' 
--Set @AMTA1 = ISNULL(@AMTA1,0)
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'9','',0,@AMTA1,0,0,'')
--Select @AMTA1=Round(Sum(Net_Amt),0) From #Form221_1 where (Date Between @Sdate and @Edate) And Bhent = 'ST' And St_Type = 'Out of Country' 
--Set @AMTA1 = ISNULL(@AMTA1,0)
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'9','',0,@AMTA1,0,0,'')

--(d) Other sales of goods [ not included in SL.No. 8 (a), SL.No.8(b) and Sl.No. 8(c) ], referred to in subsection
--    (2) of section 8, taxable at the rate applicable to sale or purchase of such goods inside
--    the State under the sales tax law of the State -
SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
SET @CHAR=65
Select @AMTA1 = 0,@AMTA2 = 0,@PER=0
DECLARE  CUR_FORM01 CURSOR FOR 
select distinct level1 from stax_mas where ST_TYPE='Local'--CHARINDEX('VAT',TAX_NAME)>0
OPEN CUR_FORM01
FETCH NEXT FROM CUR_FORM01 INTO @PER
WHILE (@@FETCH_STATUS=0)
BEGIN
	if @per = 0
		SELECT @AMTA1=ISNULL(SUM(VATONAMT),0),@AMTB1=ISNULL(SUM(TAXAMT),0) FROM VATTBL WHERE ST_TYPE='Local' AND BHENT in ('ST','PT') AND PER=@PER
		AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' AND U_Imporm = 'Under Section 8(2)'
	else
		SELECT @AMTA1=ISNULL(SUM(VATONAMT),0),@AMTB1=ISNULL(SUM(TAXAMT),0) FROM VATTBL WHERE ST_TYPE='Local' AND BHENT in ('ST','PT')  
		AND PER=@PER AND (DATE BETWEEN @SDATE AND @EDATE) AND U_Imporm = 'Under Section 8(2)'

	SET @AMTA1=ISNULL(@AMTA1,0)
	SET @AMTB1=ISNULL(@AMTB1,0)
	if @AMTA1 <> 0
    BEGIN
		INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) 
		VALUES(1,'9',CHAR(@CHAR),@PER,@AMTA1,@AMTB1,0,'Taxable at the rate of') 
		
		SET @AMTJ1 = ISNULL(@AMTJ1+@AMTA1,0) --TOTAL TAXABLE AMOUNT
		SET @AMTK1 = ISNULL(@AMTK1+@AMTB1,0) --TOTAL TAX
	END
	SET @CHAR=@CHAR+1
	FETCH NEXT FROM CUR_FORM01 INTO @PER
END
CLOSE CUR_FORM01
DEALLOCATE CUR_FORM01
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES (1,'9','Y',0,@AMTJ1,@AMTK1,0,'Total')


-- (e) Total : [Total of columns of (a), (b), (c), & (d) ]
SELECT @AMTA1=ISNULL(SUM(AMT1),0),@AMTA11=ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '6' and SRNO In ('A')
SELECT @AMTA2=ISNULL(SUM(AMT1),0),@AMTA22=ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '7' and SRNO In ('Z')
SELECT @AMTA3=ISNULL(SUM(AMT1),0),@AMTA33=ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '8' and SRNO In ('Z')
SELECT @AMTA4=ISNULL(SUM(AMT1),0),@AMTA44=ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '9' and SRNO In ('Y')
SET @AMTA111 = @AMTA1+@AMTA2+@AMTA3+@AMTA4
SET @AMTA222 = @AMTA11+@AMTA22+@AMTA33+@AMTA44
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) 
VALUES  (1,'9','Z',0,ISNULL(@AMTA111,0),ISNULL(@AMTA222,0),0,'(e) Total:[Total of columns of (a),(b),(c),&(d)]')  


--9) (a) Total tax payable as in column (4) of 8(e)
SELECT @AMTA1=ISNULL(SUM(AMT1),0),@AMTA2=ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '9' and SRNO In ('Z')
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM)
VALUES(1,'10','A',0,@AMTA1,@AMTA2,0,'')

--9) (b) Adjustment, if any, in respect of tax payable on account of sales return
SELECT @AMTA2 = ISNULL(SUM(A.TAXAMT),0) FROM VATTBL a WHERE a.BHENT='SR' AND a.St_Type = 'OUT OF STATE' AND (A.DATE BETWEEN @SDATE AND @EDATE) 
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'10','B',0,0,@AMTA2,0,'')  

--9) (c) Adjustment, if any, in respect of tax payable other than sales return
SELECT @AMTA2 = ISNULL(SUM(A.TAXAMT),0) FROM VATTBL a WHERE a.BHENT='CN' AND a.St_Type = 'OUT OF STATE' AND (A.DATE BETWEEN @SDATE AND @EDATE) 
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'10','C',0,0,@AMTA2,0,'')  

--9) (d) Net amount of tax liability [(a)-(b)+/-(c)]
SELECT @AMTA1=ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '10' and SRNO In ('A')
SELECT @AMTA2=ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '10' and SRNO In ('B')
SELECT @AMTA11=ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '10' and SRNO In ('C')
SET @AMTA111 = ISNULL((@AMTA1-@AMTA2)+ @AMTA11,0)
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'10','D',0,0,@AMTA111,0,'')  

--10) (a) Break up of total tax payable as in column (2) of 9(c)
Select @AMTA1 = 0
Select @AMTA1 = ISNULL(SUM(AMT2),0) FROM #FORM221 WHERE PARTSR = '10' and SRNO In ('C')
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'11','A',0,@AMTA1,0,0,'')

--10) (b) Less: amount of tax deferred / remitted, if any
Select @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(A.VATONAMT),0) FROM (SELECT Date,INV_no,BHENT,Tran_cd,VATONAMT,TAXAMT FROM VATTBL WHERE St_Type = 'OUT OF STATE') A 
INNER JOIN JVMAIN B ON (A.BHENT = B.Entry_ty AND A.tran_Cd=b.tran_cd ) WHERE B.VAT_ADJ = 'Less tax deferred' AND A.BHENT='J4'
AND (A.DATE BETWEEN @SDATE AND @EDATE)
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'11','B',0,@AMTA1,0,0,'')

--10) (c) Less: adjustment of input tax credit or input tax rebate as referred to in rule 8D of the CST (WB) Rules, 1958
Select @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(A.VATONAMT),0) FROM 
(SELECT Date,INV_no,BHENT,Tran_cd,VATONAMT,TAXAMT FROM VATTBL WHERE St_Type = 'OUT OF STATE') A INNER JOIN JVMAIN B ON 
(A.BHENT = B.Entry_ty AND A.tran_Cd=b.tran_cd) WHERE (A.DATE BETWEEN @SDATE AND @EDATE) AND
 B.VAT_ADJ in('Input tax credit referred to in rule 8D','Input tax rebate referred to in rule 8D') AND A.BHENT='J4'
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'11','C',0,@AMTA1,0,0,'')  

--10) (d) Add: amount of deferred tax payable, if any [Only for dealers whose deferment of tax has expired]
Select @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(A.VATONAMT),0) FROM (SELECT Date,INV_no,BHENT,Tran_cd,VATONAMT,TAXAMT FROM VATTBL WHERE St_Type = 'OUT OF STATE') A 
INNER JOIN JVMAIN B ON (A.BHENT = B.Entry_ty AND A.tran_Cd=b.tran_cd ) WHERE B.VAT_ADJ = 'Add tax deferred' AND A.BHENT='J4'
AND (A.DATE BETWEEN @SDATE AND @EDATE)
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'11','D',0,@AMTA1,0,0,'')

--10) (e) Net amount of tax payable [ (a)-(b)-(c)+(d) ]
SELECT @AMTA1=ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PARTSR = '11' and SRNO In ('A')
SELECT @AMTA2=ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PARTSR = '11' and SRNO In ('B')
SELECT @AMTA11=ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PARTSR = '11' and SRNO In ('C')
SELECT @AMTA22=ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PARTSR = '11' and SRNO In ('D')
SET @AMTA111= ISNULL(((@AMTA1-@AMTA2)-@AMTA11),0)
SET @AMTA111 = @AMTA111 + @AMTA22
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'11','E',0,@AMTA111,0,0,'')  

--11) (a) Tax paid in excess earlier within the year, now adjusted
SELECT @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(Gro_amt),0) FROM BPMAIN WHERE (Entry_ty='BP') AND Party_nm = 'CST PAYABLE' AND U_NATURE ='Tax Paid in Excess'
AND Date Between @Sdate and @Edate
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'12','A',0,@AMTA1,0,0,'')

--11) (b) Tax paid for the period into government treasury
SELECT @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(Gro_amt),0) FROM BPMAIN WHERE (Entry_ty='BP') AND Party_nm = 'CST PAYABLE' AND U_NATURE <> 'Tax Paid in Excess'
AND Date Between @Sdate and @Edate
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'12','B',0,@AMTA1,0,0,'')

--11) (c) Amount credited under Refund Adjustment Order
SELECT @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(VATONAMT),0) FROM (select A.tran_cd,A.bhent,A.VATONAMT,dbname e FROM VATTBL A 
INNER JOIN VATMAIN_VW C ON A.TRAN_cD=C.TRAN_cD WHERE (A.BHENT='J4') AND C.VAT_ADJ ='Refund Adjustment order'
AND (A.Date Between @Sdate and @Edate) AND A.St_type = 'OUT OF STATE')B
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'12','C',0,@AMTA1,0,0,'')

--11) (d) Tax short paid / Tax paid in excess. [10(e) -11(a) -11(b) -11(c)] : [ use ( -) for excess ]
SELECT @AMTA1 = 0,@AMTA11 = 0,@AMTA2 = 0,@AMTA22 = 0,@AMTB1 = 0
SELECT @AMTA1=ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PARTSR = '11' and SRNO In ('E')
SELECT @AMTA11=ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PARTSR = '12' and SRNO In ('A')
SELECT @AMTA2=ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PARTSR = '12' and SRNO In ('B')
SELECT @AMTA22=ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PARTSR = '12' and SRNO In ('C')
SET @AMTB1 = @AMTA1 - @AMTA11 - @AMTA2 - @AMTA22
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM)
VALUES  (1,'12','D',0,@AMTB1,0,0,'(e) Total:[Total of columns of (a),(b),(c),&(d)]')  

--12) (a) Interest payable as referred to in section 9(2B) of the Act read with section 33 of the
--        West Bengal Value Added Tax Act, 2003 or section 31 of the West Bengal Sales Tax Act, 1994.
SELECT @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(VATONAMT),0) FROM (select A.tran_cd,A.bhent,A.VATONAMT,dbname e FROM VATTBL A INNER JOIN VATMAIN_VW C ON 
A.TRAN_cD=C.TRAN_cD WHERE (A.BHENT='J4') AND C.VAT_ADJ in ('Interest Payable Under Column 33','Interest Payable Under Section 31')
AND (A.Date Between @Sdate and @Edate) AND A.St_type = 'OUT OF STATE')B
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'13','A',0,@AMTA1,0,0,'')

--12) (b) Interest paid for the period into government treasury
SELECT @AMTA1 = 0
SELECT @AMTA1=ISNULL(SUM(VATONAMT),0) FROM (select A.tran_cd,A.bhent,A.VATONAMT,dbname e FROM VATTBL A 
INNER JOIN VATMAIN_VW C ON A.TRAN_cD=C.TRAN_cD WHERE (A.BHENT='BP') AND C.U_NATURE ='INTEREST'
AND (A.Date Between @Sdate and @Edate) AND C.Party_nm like '%CST%')B 
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'13','B',0,@AMTA1,0,0,'')

--12) (c) Interest short paid / paid in excess : [ (a) - (b) ] [ use ( -) for excess]
SELECT @AMTA1 = 0,@AMTA11 = 0,@AMTA111 = 0
SELECT @AMTA1=ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PARTSR = '13' and SRNO In ('A')
SELECT @AMTA11=ISNULL(SUM(AMT1),0) FROM #FORM221 WHERE PARTSR = '13' and SRNO In ('B')
SET @AMTA111 = ISNULL(@AMTA1-@AMTA11,0)
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'13','C',0,@AMTA111,@AMTA222,0,'')

--Part-14
--13)(a) Late fee, referred to in section 32(2) of the West Bengal Value Added Tax Act, 2003,
--       payable for late submission of return (For Extended period set payable value to 0 (zero))
SELECT @AMTA1=0
SELECT @AMTA1=ISNULL(SUM(VATONAMT),0) FROM (select A.tran_cd,A.bhent,A.VATONAMT,dbname e FROM VATTBL A 
INNER JOIN VATMAIN_VW C ON A.TRAN_cD=C.TRAN_cD WHERE (A.BHENT='J4') AND C.VAT_ADJ ='Late Fees Payable' AND A.St_Type = 'OUT OF STATE'
AND (A.Date Between @Sdate and @Edate))B 
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'14','A',0,@AMTA1,0,0,'')

--13.(b) Late fee, referred to in section 32(2) of the West Bengal Value Added Tax Act, 2003, paid into government treasury
SELECT @AMTA1=0
SELECT @AMTA1=ISNULL(SUM(VATONAMT),0) FROM (select A.tran_cd,A.bhent,A.VATONAMT,dbname e FROM VATTBL A 
INNER JOIN VATMAIN_VW C ON A.TRAN_cD=C.TRAN_cD WHERE (A.BHENT='BP') AND C.U_NATURE ='Late Fees'
AND (A.Date Between @Sdate and @Edate) AND C.Party_nm = 'CST PAYABLE')B
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES(1,'14','B',0,@AMTA1,0,0,'')

--Select @AMTA1=SUM(NET_AMT),@AMTA2=SUM(TAXAMT) From #Form221_1 A where (Date Between @Sdate and @Edate) And Bhent = 'PT'  and  st_type = 'Out Of State'
--Set @AMTA1 = ISNULL(@AMTA1,0)
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'14','A',0,@AMTA1,@AMTA2,0,'')
--Select @AMTA1=Round(Sum(Net_Amt),0) From #Form221_1 where (Date Between @Sdate and @Edate) And Bhent = 'ST' And St_Type = 'Out of Country' 
--Set @AMTA1 = ISNULL(@AMTA1,0)
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'14','B',0,@AMTA1,0,0,'')

----Local Sale
----Select @AMTA1=Round(Sum(Net_Amt),0) From #Form221_1 where (Date Between @Sdate and @Edate) And Bhent = 'ST' And St_Type = 'Local' and tax_name <> ''
--Select @AMTA1=Sum(AMT1) From #Form221 Where PartSr = '1' and SrNo = 'A'
--Select @AMTB1=Sum(AMT1) From #Form221 Where PartSr = '1' and SrNo In('BA','BB')
--Set @AMTA1 = ISNULL(@AMTA1,0)
--Set @AMTB1 = ISNULL(@AMTB1,0)
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'1','C',0,@AMTA1-@AMTB1,0,0,'')
--Select @AMTA1=Round(Sum(Net_Amt),0) From #Form221_1 where (Date Between @Sdate and @Edate) And Bhent = 'ST' And St_Type = 'Local' and tax_name <> '' and U_imporm <> 'Purchase Return'
--Set @AMTA1 = ISNULL(@AMTA1,0)
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'1','CA',0,@AMTA1,0,0,'')
--Select @AMTA1=Sum(AMT1) From #form221 where Partsr = '1' And Srno = 'C'
--Select @AMTA2=Sum(AMT1) From #form221 where Partsr = '1' And Srno = 'CA'
--Set @AMTA1 = ISNULL(@AMTA1,0)
--Set @AMTA2 = ISNULL(@AMTA2,0)
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'1','D',0,@AMTA1-@AMTA2,0,0,'')
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'1','DA',0,0,0,0,'')
--
--
----Local Sale
--Select @AMTA1=Round(Sum(Net_Amt),0) From #Form221_1 where (Date Between @Sdate and @Edate) And Bhent = 'ST' And St_Type = 'Local' and tax_name = ''
--Set @AMTA1 = ISNULL(@AMTA1,0)
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'1','E',0,@AMTA1,0,0,'')
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'1','F',0,0,0,0,'')
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'1','FA',0,0,0,0,'')
--Set @AMTA1 = 0
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'1','FB',0,@AMTA1,0,0,'')
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'1','FC',0,@AMTA1,0,0,'')
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'1','FD',0,0,0,0,'')
----Sales to Register Dealer's
--Select @AMTA1=Round(Sum(Net_Amt),0) From #Form221_1 where (Date Between @Sdate and @Edate) And Bhent = 'ST' And s_tax <> '' and U_imporm Not In('Purchase Return','Branch Transfer') and st_type = 'Out Of State'
--Set @AMTA1 = ISNULL(@AMTA1,0)
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'1','FE',0,@AMTA1,0,0,'')
----Sales to Un-Register Dealer's
--Select @AMTA1=Round(Sum(Net_Amt),0) From #Form221_1 where (Date Between @Sdate and @Edate) And Bhent = 'ST' And s_tax = '' and U_imporm Not In('Purchase Return','Branch Transfer') and st_type = 'Out Of State'
--Set @AMTA1 = ISNULL(@AMTA1,0)
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'1','FF',0,@AMTA1,0,0,'')
--Select @AMTA1=Sum(Amt1) from #Form221 Where Partsr = '1' and srno in('FB','FC','FD','FE','FF')
--INSERT INTO #FORM221
--(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES
--(1,'1','FG',0,@AMTA1,0,0,'')

 -->---PART 2

-----Tax & Taxable Amount of Sales for the period
-- SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
-- SET @CHAR=65
-- DECLARE  CUR_FORM221 CURSOR FOR 
-- select distinct level1 from stax_mas where ST_TYPE='OUT OF STATE'--CHARINDEX('VAT',TAX_NAME)>0
-- OPEN CUR_FORM221
-- FETCH NEXT FROM CUR_FORM221 INTO @PER
-- WHILE (@@FETCH_STATUS=0)
-- BEGIN
--	if @per = 0
--		begin
--			SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #form221_1 where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' AND PER=@PER And ac_name not like '%Rece%' and U_imporm <> 'Purchase Return' And St_Type = 'Out Of State' And U_Imporm <> 'Branch Transfer'
--			SELECT @AMTB1=Round(SUM(TAXAMT),0)  FROM #form221_1 where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' AND PER=@PER And ac_name not like '%Rece%' and U_imporm <> 'Purchase Return' And St_Type = 'Out Of State' And U_Imporm <> 'Branch Transfer'
--			SELECT @AMTC1=Round(SUM(NET_AMT),0) FROM #form221_1 where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' AND PER=@PER And St_Type = 'Out Of State' And U_Imporm <> 'Branch Transfer' 
--			SELECT @AMTD1=Round(SUM(TAXAMT),0)  FROM #form221_1 where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name like '%Margin%' And S_tax <> '' AND PER=@PER And St_Type = 'Out Of State' And U_Imporm <> 'Branch Transfer'
--		end
--	else
--		begin
--			SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #form221_1 where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and ac_name not like '%Rece%' And S_tax <> '' AND PER=@PER and U_imporm <> 'Purchase Return' And St_Type = 'Out Of State' And U_Imporm <> 'Branch Transfer'
--			SELECT @AMTB1=Round(SUM(TAXAMT),0)  FROM #form221_1 where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and ac_name not like '%Rece%' And S_tax <> '' AND PER=@PER and U_imporm <> 'Purchase Return' And St_Type = 'Out Of State' And U_Imporm <> 'Branch Transfer'
--			SELECT @AMTC1=Round(SUM(NET_AMT),0) FROM #form221_1 where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' AND PER=@PER And St_Type = 'Out Of State' And U_Imporm <> 'Branch Transfer'
--			SELECT @AMTD1=Round(SUM(TAXAMT),0)  FROM #form221_1 where bhent = 'SR' AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' AND PER=@PER And St_Type = 'Out Of State' And U_Imporm <> 'Branch Transfer'
--		end
--	
--  --Sales Invoices
--  SET @AMTA1=ISNULL(@AMTA1,0)
--  SET @AMTB1=ISNULL(@AMTB1,0)
-- 
--  --Return Invoices
--  SET @AMTC1=ISNULL(@AMTC1,0)
--  SET @AMTD1=ISNULL(@AMTD1,0)
--
--  --Net Effect
--  Set @NetEFF = @AMTA1-(@AMTB1+(@AMTC1-@AMTD1))
--  --Set @NetEFF = (@AMTA1-@AMTB1)-(@AMTC1-@AMTD1)
--  Set @NetTAX = (@AMTB1)-(@AMTD1)
--
--  if @nettax <> 0
--	  begin
--		  INSERT INTO #FORM221
--		  (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
--		  (1,'15',CHAR(@CHAR),@PER,@NETEFF,@NETTAX,0,'')
--		--  (1,'6',CHAR(@CHAR),@PER,@AMTA1-@AMTB1,@AMTB1,0)
--		  
--		--  SET @AMTJ1=@AMTJ1+@AMTA1 --TOTAL TAXABLE AMOUNT
--		--  SET @AMTK1=@AMTK1+@AMTB1 --TOTAL TAX
--		  SET @AMTJ1=@AMTJ1+@NETEFF --TOTAL TAXABLE AMOUNT
--		  SET @AMTK1=@AMTK1+@NETTAX --TOTAL TAX
--		  SET @CHAR=@CHAR+1
--	  end
--
--  FETCH NEXT FROM CUR_FORM221 INTO @PER
-- END
-- CLOSE CUR_FORM221
-- DEALLOCATE CUR_FORM221
--
-----Total of Tax & Taxable Amount of Sales for the period
-- INSERT INTO #FORM221
-- (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm) VALUES
-- (1,'15','Z',0,@AMTJ1,@AMTK1,0,'')
---- (1,'6','Z',0,@AMTJ1-@AMTK1,@AMTK1,0)


--Part 15
--A- Tax deposited in Bank / Treasury
----Bank Payment Details
Declare @TAXONAMT as numeric(12,2),@TAXONAMT1 as numeric(12,2),@TAXAMT1 as numeric(12,2),@ITEMAMT as numeric(12,2),@INV_NO as varchar(10),@DATE as smalldatetime,@PARTY_NM as varchar(50),@ADDRESS as varchar(100),@ITEM as varchar(50),@FORM_NM as varchar(30),@S_TAX as varchar(30),@QTY as numeric(18,4)
SELECT @TAXONAMT=0,@TAXONAMT1=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0
SET @CHAR=65
SET @PER = 0
declare Cur_VatPay cursor  for
select A.Vatonamt,A.Gro_amt,A.taxamt,A.INV_NO,B.Date,B.Bank_nm as PARTY_NM,Address=AC.S_Tax,
Form_nm = substring(datename(month,B.date),1,3)+' '+CAST(year(B.Date) AS VARCHAR(10)),S_Tax = B.U_Nature
from VATTBL A
Inner join Bpmain B on (A.Bhent = B.Entry_ty and A.Tran_cd = B.Tran_cd)
INNER JOIN AC_MAST AC ON (AC.ac_name=B.Bank_nm)
where BHENT = 'BP' And B.Date Between @sdate and @edate and B.Party_nm like '%CST%' AND B.U_Nature IN ('INTEREST','Late Fees','CST')
open Cur_VatPay
FETCH NEXT FROM Cur_VatPay INTO @TAXONAMT,@ITEMAMT,@TAXAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX
	WHILE (@@FETCH_STATUS=0)
	BEGIN
		SET @TAXONAMT1 =  @TAXONAMT
		SET @ITEMAMT   =  CASE WHEN UPPER(@S_TAX) = 'INTEREST' THEN ISNULL(@TAXONAMT,0) ELSE 0 END
		SET @TAXAMT    =  CASE WHEN @S_TAX = 'CST' THEN ISNULL(@TAXONAMT,0) ELSE 0 END
		SET @TAXONAMT  =  CASE WHEN @S_TAX = 'Late Fees' THEN ISNULL(@TAXONAMT,0) ELSE 0 END
		INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,AMT4,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX)
		VALUES (1,'15',CHAR(@CHAR),ISNULL(@PER,0),ISNULL(@TAXONAMT,0),ISNULL(@TAXAMT,0),ISNULL(@ITEMAMT,0),ISNULL(@TAXONAMT1,0),ISNULL(@INV_NO,''),
				ISNULL(@DATE,''),ISNULL(@PARTY_NM,''),ISNULL(@ADDRESS,''),ISNULL(@FORM_NM,''),ISNULL(@S_TAX,''))

		SET @CHAR=@CHAR+1
	FETCH NEXT FROM Cur_VatPay INTO @TAXONAMT,@ITEMAMT,@TAXAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX--,@ITEM,@QTY
END
CLOSE Cur_VatPay
DEALLOCATE Cur_VatPay

--Declare @TAXONAMT as numeric(12,2),@TAXAMT1 as numeric(12,2),@ITEMAMT as numeric(12,2),@INV_NO as varchar(10),@DATE as smalldatetime,@PARTY_NM as varchar(50),@ADDRESS as varchar(100),@ITEM as varchar(50),@FORM_NM as varchar(30),@S_TAX as varchar(30),@QTY as numeric(18,4)
--SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0

--SET @CHAR=65
--SET @PER = 0
--declare Cur_VatPay cursor  for
--select A.Vatonamt,A.Gro_amt,A.taxamt,INV_NO=b.u_chalno,Date=b.u_chaldt,A.Party_nm,Address=c.U_BSRCODE,Form_nm=substring(datename(month,c.date),1,3)+' '+CAST(year(c.Date) AS VARCHAR(10)),S_tax = ac.S_TAX
--from VATTBL A
--INNER JOIN VATMAIN_VW c On(A.Bhent = c.Entry_Ty And A.Tran_cd = c.Tran_cd)
--INNER JOIN AC_MAST AC ON (AC.AC_ID=c.AC_ID) 
--INNER JOIN Bpmain B on (A.Bhent = B.Entry_ty and A.Tran_cd = B.Tran_cd)
--where BHENT = 'BP' And B.Date Between @sdate and @edate And B.Party_nm like '%CST%'and B.u_nature Like ('late fees')
--open Cur_VatPay
--FETCH NEXT FROM Cur_VatPay INTO @TAXONAMT,@ITEMAMT,@TAXAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX
--	WHILE (@@FETCH_STATUS=0)
--	BEGIN

--	SET @PER=CASE WHEN @PER IS NULL THEN 0 ELSE @PER END
--	SET @TAXONAMT=CASE WHEN @TAXONAMT IS NULL THEN 0 ELSE @TAXONAMT END
--	SET @TAXAMT=CASE WHEN @TAXAMT IS NULL THEN 0 ELSE @TAXAMT END
--	SET @ITEMAMT=CASE WHEN @ITEMAMT IS NULL THEN 0 ELSE @ITEMAMT END
--	SET @QTY=CASE WHEN @QTY IS NULL THEN 0 ELSE @QTY END
--	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
--	SET @INV_NO=CASE WHEN @INV_NO IS NULL THEN '' ELSE @INV_NO END
--	SET @DATE=CASE WHEN @DATE IS NULL THEN '' ELSE @DATE END
--	SET @ADDRESS=CASE WHEN @ADDRESS IS NULL THEN '' ELSE @ADDRESS END
--	SET @ITEM=CASE WHEN @ITEM IS NULL THEN '' ELSE @ITEM END
--	SET @S_TAX=CASE WHEN @S_TAX IS NULL THEN '' ELSE @S_TAX END
--	SET @FORM_NM=CASE WHEN @FORM_NM IS NULL THEN '' ELSE @FORM_NM END
	
--	INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) VALUES (1,'15',CHAR(@CHAR),@PER,@TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX)

--	SET @CHAR=@CHAR+1
--	FETCH NEXT FROM CUR_VatPay INTO @TAXONAMT,@TAXAMT,@ITEMAMT,@INV_NO,@DATE,@PARTY_NM,@ADDRESS,@FORM_NM,@S_TAX--,@ITEM,@QTY
--END
--CLOSE CUR_VatPay
--DEALLOCATE CUR_VatPay

DECLARE @CNT NUMERIC (4,0)
SET @CNT = 0

SELECT @CNT=COUNT(PARTSR) FROM #FORM221 WHERE PARTSR = '15'
IF @CNT = 0
 BEGIN
	INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM,ADDRESS,FORM_NM,S_TAX) 
	VALUES (1,'15','A',0,0,0,0,'','','','','','')
 END

--Part-16
--14. Goods purchased within the meaning of section 3 of the Central Sales Tax Act, 1956 and for which Form C is required.
--    [Details to be furnished in Annexure E]
SET @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(A.VATONAMT),0) FROM VATTBL A WHERE A.ST_TYPE='OUT OF STATE' 
AND A.BHENT='PT'  AND  A.S_Tax <> '' AND LTRIM(RTRIM(REPLACE(REPLACE(A.FORM_NM,'-',''),'FORM','')))='C' 
AND (A.DATE BETWEEN @SDATE AND @EDATE)
--SELECT @AMTA1 = ISNULL(SUM(GRO_AMT),0) FROM (SELECT A.GRO_AMT,A.TAXAMT,DBNAME FROM VATTBL A 
--INNER JOIN VATMAIN_VW C ON A.TRAN_CD=C.TRAN_CD AND A.BHENT=C.ENTRY_TY
--INNER JOIN AC_MAST SAC ON SAC.AC_ID =C.AC_ID WHERE (A.BHENT in ('PT')) AND (A.DATE BETWEEN @SDATE AND @EDATE) 
--and a.ST_TYPE='OUT OF STATE' AND LTRIM(RTRIM(REPLACE(REPLACE(ISNULL(A.Form_Nm,''),'-',''),'FORM',''))) = 'C'
--AND SAC.C_TAX <> '' AND A.U_Imporm = 'Under Section 3') B
INSERT INTO #FORM221 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES  (1,'16','A',0,@AMTA1,0,0,'') 

--15. Receipt of goods on stock transfer from other States within the meaning of section 6A of Central Sales Tax Act,1956 
--      and for which Form F is required. [Details to be furnished in Annexure F]
SET @AMTA1 = 0
SELECT @AMTA1 = ISNULL(SUM(VATONAMT),0) FROM VATTBL A WHERE A.ST_TYPE='OUT OF STATE' AND A.U_IMPORM in ('Consignment Transfer','BRANCH TRANSFER')
AND A.BHENT='ST' AND A.S_TAX <> '' AND LTRIM(RTRIM(REPLACE(REPLACE(A.RFORM_NM,'-',''),'FORM','')))='F'
AND (A.DATE BETWEEN @SDATE AND @EDATE)

--SELECT @AMTA1 = ISNULL(SUM(GRO_AMT),0) FROM (SELECT A.GRO_AMT,A.TAXAMT,DBNAME FROM VATTBL A 
--INNER JOIN VATMAIN_VW C ON A.TRAN_CD=C.TRAN_CD AND A.BHENT=C.ENTRY_TY
--INNER JOIN AC_MAST SAC ON SAC.AC_ID =C.AC_ID WHERE (A.BHENT in ('PT')) AND (A.DATE BETWEEN @SDATE AND @EDATE) 
--AND a.ST_TYPE='OUT OF STATE' AND LTRIM(RTRIM(REPLACE(REPLACE(ISNULL(A.Form_Nm,''),'-',''),'FORM',''))) = 'F'
--AND A.U_imporm in('Branch Transfer')) B
INSERT INTO #FORM221(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) VALUES (1,'16','B',0,@AMTA1,0,0,'')

--Updating Null Records  
Update #form221 set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
		             RATE = isnull(RATE,0), AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0),AMT4 =  isnull(AMT2,0),
					 AMT3 = isnull(AMT3,0), INV_NO = isnull(INV_NO,''), DATE = isnull(Date,''), 
					 PARTY_NM = isnull(Party_nm,''), ADDRESS = isnull(Address,''),
					 FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,'')--, Qty = isnull(Qty,0),  ITEM =isnull(item,''),

 SELECT * FROM #FORM221 order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int)
 --SELECT * FROM #FORM221_1 --order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int)
 
 END
--Print 'WB VAT FORM 01'

