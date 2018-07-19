IF EXISTS (SELECT XTYPE, NAME FROM SYSOBJECTS WHERE XTYPE = 'P' AND NAME = 'USP_REP_UP_VATFORM24')
BEGIN
	DROP PROCEDURE USP_REP_UP_VATFORM24
END
GO
set ANSI_NULLS ON
GO
set QUOTED_IDENTIFIER ON
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Hetal L Patel
-- Create date: 16/05/2007
-- Description:	This Stored procedure is useful to generate VAT Computation Report.
-- Modify date: May 14,2010
-- Modified By: Rakesh Varma
-- Modify date: May 14,2010
-- Modified By: Sandeep Shah 
-- Modify date: 03/07/2010 for TKT-1491
-- Modified By: Sandeep Shah 
-- Modify date: 03/10/2010 for TKT-1491
-- Modified By: GAURAV R. TANNA
-- Modify date: 27/06/2015 for Updating Rajastan VAT Reports as per new USP_REP_SINGLE_CO_DATA_VAT
---EXECUTE USP_REP_UP_VATFORM24'','','','04/01/2013','03/31/2014','','','','',0,0,'','','','','','','','','2013-2014',''
-- Remark: Part No.14 & 15 Modification of ITC Details & Net Tax done for TKT-1491  
-- Modified By: Suraj Kumawat
-- Modify date: 30/06/2016 for bug-26469 


 
-- =============================================
CREATE PROCEDURE [dbo].[USP_REP_UP_VATFORM24]
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
AS

BEGIN
DECLARE @FCON AS NVARCHAR(2000)
EXECUTE   USP_REP_FILTCON 

@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=NULL,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='M',@VITFILE=Null,@VACFILE='AC'
,@VDTFLD ='DATE'
,@VLYN=Null
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

DECLARE @SQLCOMMAND NVARCHAR(4000)
DECLARE @RATE NUMERIC(12,2),@AMTA1 NUMERIC(12,2),@AMTB1 NUMERIC(12,2),@AMTC1 NUMERIC(12,2),@AMTD1 NUMERIC(12,2),@AMTE1 NUMERIC(12,2),@AMTF1 NUMERIC(12,2),@AMTG1 NUMERIC(12,2),@AMTH1 NUMERIC(12,2),@AMTI1 NUMERIC(12,2),@AMTJ1 NUMERIC(12,2),@AMTK1 NUMERIC(12,2),@AMTL1 NUMERIC(12,2),@AMTM1 NUMERIC(12,2),@AMTN1 NUMERIC(12,2),@AMTO1 NUMERIC(12,2)
DECLARE @AMTA2 NUMERIC(12,2),@AMTB2 NUMERIC(12,2),@AMTC2 NUMERIC(12,2),@AMTD2 NUMERIC(12,2),@AMTE2 NUMERIC(12,2),@AMTF2 NUMERIC(12,2),@AMTG2 NUMERIC(12,2),@AMTH2 NUMERIC(12,2),@AMTI2 NUMERIC(12,2),@AMTJ2 NUMERIC(12,2),@AMTK2 NUMERIC(12,2),@AMTL2 NUMERIC(12,2),@AMTM2 NUMERIC(12,2),@AMTN2 NUMERIC(12,2),@AMTO2 NUMERIC(12,2),@AMTP1 NUMERIC (12,2),@AMTQ1 NUMERIC (12,2)
DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@CHAR INT,@LEVEL NUMERIC(12,2),@balamt NUMERIC(12,2)

SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) INTO #VATAC_MAST FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
INSERT INTO #VATAC_MAST SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) FROM STAX_MAS WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
 

Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)

---Temporary Cursor
SELECT PART=3,PARTSR='AAAA',SRNO='AAA',RATE=99.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.MAILNAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),STM.FORM_NM,AC1.S_TAX
INTO #FORM24
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
		
	End
else
	Begin ------Fetch Single Co. Data
		 Set @MultiCo = 'NO'
		 --EXECUTE USP_REP_SINGLE_CO_DATA_UP
		 EXECUTE USP_REP_SINGLE_CO_DATA_VAT
		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		 ,@MFCON = @MCON OUTPUT
		
	End
-----
-----SELECT * from #form221_1 where (Date Between @Sdate and @Edate) and Bhent in('EP','PT','CN') and TAX_NAME In('','NO-TAX') and U_imporm = ''
-----
--=======Data Collection Ends


SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 

--Part 7A & B
 DECLARE @BHENT VARCHAR(10),@INV_NO VARCHAR(200),@DATE VARCHAR(200),@ST_TYPE VARCHAR(200),
         @NET_AMT NUMERIC(14,2),@PARTY_NM VARCHAR(200),@ITEM VARCHAR(200),@GRO_AMT NUMERIC(14,2),
         @S_TAX VARCHAR(200),@QTY VARCHAR(200),@ADDRESS VARCHAR(500),@PINV_NO VARCHAR(200),@PDATE VARCHAR(200),
         @PQTY NUMERIC(18,4),@PNET_AMT NUMERIC (14,2), @PTAX_AMT NUMERIC (14,2), @PGRO_AMT NUMERIC(14,2),@VATTYPE VARCHAR(50),
         @TRAN_CD NUMERIC(8)


----Purchases against Tax Invoice
--SET @AMTA1=0
--SELECT @AMTA1=isnull(sum(A.GRO_AMT),0)
--FROM VATTBL A
--INNER JOIN PTITEM D ON (D.ENTRY_TY = A.BHENT AND D.TRAN_CD = A.TRAN_CD AND D.IT_CODE = A.IT_CODE AND A.ItSerial =D.itserial )
--INNER JOIN IT_MAST I ON (I.IT_CODE = D.IT_CODE)
--WHERE A.ST_TYPE='LOCAL' AND A.BHENT IN ('PT','EP') AND (A.DATE BETWEEN @SDATE AND @EDATE) 
--And A.Tax_Name like '%VAT%' and a.S_TAX <> ''

---- For this data will fetch from annexure A part -  added by suraj kumawat 08-07-2016 start
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=m.NET_AMT,AMT2=m.NET_AMT,AMT3=m.NET_AMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),
STM.FORM_NM,AC1.S_TAX,Item=space(50),qty=9999999999999999999.9999
INTO #AnnexA_vat24
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2
INSERT INTO #AnnexA_vat24 EXECUTE USP_REP_UP_ANNEXURE_A'','','',@SDATE,@EDATE,'','','','',0,0,'','','','','','','','','2013-2014',''
---- For this data will fetch from annexure A part -  added by suraj kumawat 08-07-2016 end ;
SELECT @AMTA1=isnull(SUM(amt3),0) FROM #AnnexA_vat24 where part = 1 and partsr ='1' and srno='A'
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7A','A',0,@AMTA1,0,0,'')

----Purchases Unregistered Dealers
--SELECT @AMTA1=SUM(NET_AMT) from (Select Distinct Tran_cd,Net_amt FROM #FORM_24 WHERE BHENT In('PT','EP','CN') AND (DATE BETWEEN @SDATE AND @EDATE) and S_tax = '' And Tax_name <> 'Exempted') b
SET @AMTA1=0
SELECT @AMTA1=SUM(Gro_amt) from VATTBL WHERE BHENT In('PT','EP') AND (DATE BETWEEN @SDATE AND @EDATE) 
And S_TAX = '' AND VATTYPE='' 

SET @AMTA1 = CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7A','B',0,@AMTA1,0,0,'')

----Purchases Exempted
SELECT @AMTA1=SUM(Gro_amt) from VATTBL WHERE BHENT In('PT','EP') AND (DATE BETWEEN @SDATE AND @EDATE)  
and tax_name like '%Exempted%' AND VATTYPE='' --AND ITEMTYPE <> 'C'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7A','C',0,@AMTA1,0,0,'')

----Purchases EX UP
SET @AMTA1=0
SELECT @AMTA1=SUM(Gro_amt) from VATTBL WHERE BHENT In('PT','EP') AND (DATE BETWEEN @SDATE AND @EDATE)
AND ST_TYPE ='OUT OF STATE' and tax_name like '%C.S.T%' AND VATTYPE='' 
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7A','D',0,@AMTA1,0,0,'')

----Purchases In Principal A/c
SET @AMTA1 = 0
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7A','E',0,0,0,0,'')
--Below is New Code Added on May 13,2010 as UP Form 24 has been added with new sections in it
--Modified by:Rakesh Varma
--------------------------------------------------------------------------------------------------------
--7(a)---(v)(a) & (b)
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7A','F',0,0,0,0,'')

--------------------------------------------------------------------------------------------------------
SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7A','G',0,0,0,0,'')
--------------------------------------------------------------------------------------------------------
SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7A','H',0,0,0,0,'')
--------------------------------------------------------------------------------------------------------

INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7A','I',0,0,0,0,'')
--------------------------------------------------------------------------------------------------------

----Purchases Any Other
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7A','J',0,0,0,0,'')


SET @AMTB1=0
SELECT @AMTB1=SUM(AMT1) 
from #FORM24 
WHERE PARTSR = '7A' AND SRNO IN ('A','B','C','D','E','F','G','H','I','J')

INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7A','K',0,@AMTB1,0,0,'')

--7(a)--vii & viii
--Below is New Code Added on May 13,2010 as UP Form 24 has been added with new sections in it
--Modified by:Rakesh Varma
--------------------------------------------------------------------------------------------------------
SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0

SELECT @AMTA1=0,@AMTA2=0,@AMTB1=0,@AMTB2=0,@AMTC1=0,@AMTC2=0
----details will come from annexure A-1  vat goods  Addeded by Suraj Kuamwat 08-07-2016
	SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=m.NET_AMT,AMT2=m.NET_AMT,AMT3=m.NET_AMT,
	M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),
	STM.FORM_NM,AC1.S_TAX,Item=space(150),qty=9999999999999999999.9999,MAIN_TY=M.ENTRY_TY, MAIN_CD = M.TRAN_CD, MAIN_INV=M.INV_NO,  MAIN_DATE = M.DATE
	--PINV_NO = M.INV_NO, PDATE = M.DATE, Pqty=9999999999999999999.9999,
	--AMT4=NET_AMT,AMT5=M.TAXAMT,AMT6=M.TAXAMT,CNNO = M.INV_NO, CNDT = M.DATE
	INTO #AnnexA_1
	FROM PTACDET A 
	INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
	INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
	INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
	INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
	WHERE 1=2
 INSERT INTO #AnnexA_1  EXECUTE USP_REP_UP_ANNEXURE_A1'','','',@SDATE,@EDATE,'','','','',0,0,'','','','','','','','','2013-2014',''
 ----details will come from annexure A-1  vat goods  Addeded by Suraj Kuamwat 08-07-2016 End
 
 SET @AMTA1 = 0
 SELECT @AMTA1 = ISNULL(SUM(AMT3),0)  FROM #AnnexA_1 WHERE  PARTSR = '3' 
 INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7A','L',0,@AMTA1,0,0,'')

--------------------------------------------------------------------------------------------------------

SET @AMTD1=0
SELECT @AMTD1=isnull(SUM(AMT1),0)
from #FORM24 
WHERE PARTSR = '7A' AND SRNO IN ('A','B','C','D','E','F','G','H','I','J')
SET @AMTD1=ISNULL(@AMTD1,0)
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7A','M',0,@AMTD1-(@AMTA1),0,0,'')
--------------------------------------------------------------------------------------------------------

--SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0

--7(B)
--BLANK
--Below is New Code Added on May 13,2010 as UP Form 24 has been added with new sections in it
--Modified by:Rakesh Varma
--------------------------------------------------------------------------------------------------------
----Non VAT Goods Purchases
---Modified by:Sandeep s.
----Purchase from registered dealers


SET @AMTB1=0
SELECT @AMTB1=SUM(gro_amt) from VATTBL WHERE BHENT In('PT','EP') AND (DATE BETWEEN @SDATE AND @EDATE) 
And S_TAX <> '' AND VATTYPE='Non Vat' 
SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B','A',0,@AMTB1,0,0,'')

----------------------------------------------------------------------------------------------------------
----Purchases Unregistered Dealers
SET @AMTB1=0
SELECT @AMTB1=isnull(SUM(gro_amt),0) from VATTBL WHERE BHENT In('PT','EP') AND (DATE BETWEEN @SDATE AND @EDATE) 
And S_TAX = '' AND VATTYPE='Non Vat' 
SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B','B',0,@AMTB1,0,0,'')
---------------------------------------------------------------------------------------------------------------
----Purchases Exempted
SET @AMTB1=0
SELECT @AMTB1=isnull(SUM(GRO_AMT),0) from VATTBL WHERE BHENT In('PT','EP') AND (DATE BETWEEN @SDATE AND @EDATE) 
And VATTYPE='Non Vat' AND TAX_NAME like '%Exempted%' 
SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B','C',0,@AMTB1,0,0,'')
---------------------------------------------------------------------------------------------------------------

----Purchases Ex UP
SET @AMTA1=0
SELECT @AMTA1=ISNULL(SUM(Gro_amt),0) from VATTBL WHERE BHENT In('PT','EP') AND (DATE BETWEEN @SDATE AND @EDATE)
AND ST_TYPE ='OUT OF STATE' and tax_name like '%C.S.T%' AND VATTYPE='Non Vat' 
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B','D',0,@AMTA1,0,0,'')
---------------------------------------------------------------------------------------------------------------

INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B','E',0,0,0,0,'')
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B','F',0,0,0,0,'')
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B','G',0,0,0,0,'')
----Any other purchase
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B','H',0,0,0,0,'')
----Total Purchases

SELECT @AMTD1=SUM(AMT1) 
from #FORM24 
WHERE PARTSR = '7B' AND SRNO IN ('A','B','C','D','E','F','G','H')
SET @AMTD1=CASE WHEN @AMTD1 IS NULL THEN 0 ELSE @AMTD1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B','I',0,@AMTD1,0,0,'')

---Purchase Return
SELECT @AMTA1=0,@AMTA2=0,@AMTB1=0,@AMTB2=0,@AMTC1=0,@AMTC2=0
 SET @AMTA1 = 0
 SELECT @AMTA1 = ISNULL(SUM(AMT3),0)  FROM #AnnexA_1 WHERE PARTSR = '4' 
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B','J',0,@AMTA1,0,0,'')


--Net Purchase
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B','K',0,@AMTD1-(@AMTA1),0,0,'')


--Grand Total
SELECT @AMTA1=SUM(AMT1) 
from #FORM24 
WHERE PARTSR = '7A' AND SRNO IN ('M')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END

SELECT @AMTB1=SUM(AMT1) 
from #FORM24 
WHERE PARTSR = '7B' AND SRNO IN ('K')
SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END

INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B','L',0,@AMTA1+@AMTB1,0,0,'')
--------------------------------------------------------------------------------------------------------

--7.c- Capital Goods purchased from within the State
--Below is New Code Added on May 13,2010 as UP Form 24 has been added with new sections in it
--Modified by:Rakesh Varma
--------------------------------------------------------------------------------------------------------

SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0

SET @AMTA1=0
SELECT @AMTA1=SUM(GRO_AMT) from VATTBL WHERE BHENT In('PT','EP') AND (DATE BETWEEN @SDATE AND @EDATE) 
AND ITEMTYPE = 'C' AND S_TAX <> '' AND ST_TYPE in ('LOCAL','')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B2','A',0,@AMTA1,0,0,'')

--------------------------------------------------------------------------------------------------------

SET @AMTB1=0
SELECT @AMTB1=SUM(GRO_AMT) from VATTBL WHERE BHENT In('PT','EP') AND (DATE BETWEEN @SDATE AND @EDATE) 
AND ITEMTYPE = 'C' AND S_TAX = '' AND ST_TYPE in ('LOCAL', '')
SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B2','B',0,@AMTB1,0,0,'')

-- Total . . . .
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B2','C',0,@AMTA1+@AMTB1,0,0,'')
--------------------------------------------------------------------------------------------------------

--7.d- Purchases through commission agent for which certificate in form VI has been received
--Below is New Code Added on May 13,2010 as UP Form 24 has been added with new sections in it
--Modified by:Rakesh Varma
--------------------------------------------------------------------------------------------------------
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B3','A',0,0,0,0,'')
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7B3','B',0,0,0,0,'')
--------------------------------------------------------------------------------------------------------

--7A- Purchases/value of goods received from outside State against Forms of declaration / certificates

--7(a)- Purchase against Form C / Form H / Form I (Details to be furnished in annexure C, D & E respectively)
SET @AMTA1=0
SELECT @AMTA1=ISNULL(SUM(GRO_AMT),0) from VATTBL WHERE BHENT In('PT','EP','P1') AND (DATE BETWEEN @SDATE AND @EDATE) 
and ST_TYPE in('OUT OF STATE','OUT OF COUNTRY')
AND RTRIM(LTRIM(REPLACE(REPLACE(REPLACE(REPLACE(FORM_NM,'FORM',''),'-',''),'/',''),' ',''))) in ('C', 'H', 'I') 
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7C','A',0,@AMTA1,0,0,'')

--7(b)- Value of goods received from outside State against Form F (details to be furnished in annexure F)
SET @AMTB1=0
SELECT @AMTB1=ISNULL(SUM(GRO_AMT),0) from VATTBL WHERE BHENT In('PT','EP','P1') AND (DATE BETWEEN @SDATE AND @EDATE) 
AND RTRIM(LTRIM(REPLACE(REPLACE(REPLACE(REPLACE(FORM_NM,'FORM',''),'-',''),'/',''),' ',''))) ='F' 
and ST_TYPE in('OUT OF STATE','OUT OF COUNTRY')
SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7C','B',0,@AMTB1,0,0,'')

-- Total
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'7C','C',0,@AMTA1+@AMTB1,0,0,'')

DECLARE @RCOUNT INT


---Part 8A & B Vat Purchase Rate Wise Breakup
--- commented by Suraj Kumawat for bug-26469 date on 02-07-2016 Startv :
 --SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
 --SET @CHAR=65
 --DECLARE  CUR_FORM24 CURSOR FOR 
 --select distinct level1 from stax_mas where ST_TYPE='LOCAL' AND entry_ty in ('PT', 'EP') AND level1 in (1,4,12.5) ORDER BY LEVEL1
 --OPEN CUR_FORM24
 --FETCH NEXT FROM CUR_FORM24 INTO @PER
 --WHILE (@@FETCH_STATUS=0)
 --BEGIN
	
	--Begin
	
	--	INSERT INTO #FORM24
	--	(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) 		  
	--	   SELECT 1, 
	--	   '8A', 
	--	   CASE WHEN @per = 1.00 THEN 'A' WHEN (@per = 4.00) THEN 'B' WHEN (@per = 12.50) THEN 'C' ELSE 'D'  END,
	--	   @Per, 
	--	   Sum(VATTBL.VATONAMT),
	--	   Sum(VATTBL.TAXAMT),
	--	   0,
	--	   it_mast.It_name
	--	   FROM VATTBL 
 --          Inner Join IT_MAST ON IT_MAST.It_code = VATTBL.It_code
	--	   inner join litem_vw B on (VATTBL.BHENT = B.Entry_ty And VATTBL.Tran_cd = B.Tran_cd And VATTBL.It_code = B.It_code)
 --          where VATTBL.bhent in ('PT', 'EP') And VATTBL.S_tax <> '' AND VATTBL.PER IN (1,4,12.5) AND VATTBL.PER = @PER
 --          AND VATTBL.TAX_NAME LIKE '%VAT%' AND VATTBL.TAXAMT <> 0 AND VATTYPE=''
	--	   AND (VATTBL.DATE BETWEEN @SDATE AND @EDATE)
	--	   group by IT_MAST.it_name, VATTBL.PER
		   
				   
	--	    SET @CHAR=@CHAR+1
	--End

  
 -- FETCH NEXT FROM CUR_FORM24 INTO @PER
 --END
 --CLOSE CUR_FORM24
 --DEALLOCATE CUR_FORM24
---- Commented by SurajKumawat for Bug-26469 date on 02-0-2016
---- Added by SurajKumawat for Bug-26469 date on 02-0-2016 start :
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM)  SELECT 1, '8A','A',VATTBL.PER
, ISNULL(Sum(case when VATTBL.bhent IN('pt','EP') then VATTBL.VATONAMT  else -VATTBL.VATONAMT end),0)
		   ,ISNULL(Sum(case when VATTBL.bhent IN('pt','EP') then VATTBL.TAXAMT else -VATTBL.TAXAMT end),0),
		   0,it_mast.It_name
FROM VATTBL Inner Join IT_MAST ON IT_MAST.It_code = VATTBL.It_code
left outer join litem_vw c on(VATTBL.BHENT =c.entry_ty and VATTBL.TRAN_CD =c.Tran_cd and VATTBL.ItSerial =c.itserial )
where VATTBL.bhent in ('PT', 'EP','DN','PR') And VATTBL.S_tax <> '' AND VATTBL.PER IN (1,4,12.5) 
AND VATTBL.ST_TYPE IN('','LOCAL')
AND VATTBL.TAX_NAME LIKE '%VAT%' AND VATTYPE='' AND (VATTBL.DATE BETWEEN @SDATE AND @EDATE) and c.ADVATPER1 <> 1 
group by VATTBL.PER,IT_MAST.it_name
---- Added by SurajKumawat for Bug-26469 date on 02-0-2016 End  :

SELECT @RCOUNT = 0
SET @RCOUNT = (SELECT COUNT(*) FROM #FORM24 WHERE PARTSR='8A')
IF (@RCOUNT=0)
begin
	INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'8A','A',0,0,0,0,'')
end

---- Commented by SurajKumawat for Bug-26469 date on 02-0-2016 Start :
 --SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
 --SET @CHAR=65
 --DECLARE  CUR_FORM24 CURSOR FOR 
 --select distinct level1 from stax_mas where ST_TYPE='LOCAL' AND entry_ty in ('PT', 'EP') AND level1 in (0.5,1) ORDER BY LEVEL1
 --OPEN CUR_FORM24
 --FETCH NEXT FROM CUR_FORM24 INTO @PER
 --WHILE (@@FETCH_STATUS=0)
 --BEGIN
	
	--Begin
	
		
	--	 INSERT INTO #FORM24
	--	(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) 		  
	--	   SELECT 1, 
	--	   '8A1', 
	--	   CASE WHEN @per = 0.5 THEN 'E' WHEN (@per = 1.00) THEN 'F' ELSE 'G'  END, 
	--	   @Per, 
	--	   Sum(VATTBL.VATONAMT),
	--	   IsNull(Sum(B.ADDLVAT1), 0),
	--	   0,
	--	   it_mast.It_name
	--	   FROM VATTBL 
 --          Inner Join IT_MAST ON IT_MAST.It_code = VATTBL.It_code
	--	   inner join litem_vw B on (VATTBL.BHENT = B.Entry_ty And VATTBL.Tran_cd = B.Tran_cd And VATTBL.It_code = B.It_code)
 --          where VATTBL.bhent in ('PT', 'EP') And VATTBL.S_tax <> '' AND VATTBL.PER IN (0.5, 1.00)  AND VATTBL.PER = @PER
 --          AND VATTBL.TAX_NAME LIKE '%VAT%' AND B.ADDLVAT1 <> 0 AND VATTYPE=''
	--	   AND (VATTBL.DATE BETWEEN @SDATE AND @EDATE)
	--	   group by IT_MAST.it_name, VATTBL.PER
		   
		
	--	    SET @CHAR=@CHAR+1
	--End

  
 -- FETCH NEXT FROM CUR_FORM24 INTO @PER
 --END
 --CLOSE CUR_FORM24
 --DEALLOCATE CUR_FORM24

---- Commented by SurajKumawat for Bug-26469 date on 02-0-2016 End :
-----  Added by SurajKumawat for Bug-26469 date on 02-0-2016 Start :
INSERT INTO #FORM24(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) 		  
SELECT 1,  '8A1', 'A' as Srno,B.ADVATPER1
,ISNULL(SUM(case when VATTBL.bhent IN('pt','EP') then VATTBL.VATONAMT else -VATTBL.VATONAMT  end ),0)
,IsNull(SUM(case when VATTBL.bhent IN('pt','EP') then B.ADDLVAT1 else -B.ADDLVAT1 end ), 0),
0,it_mast.It_name
FROM VATTBL Inner Join IT_MAST ON IT_MAST.It_code = VATTBL.It_code
left outer join litem_vw B on (VATTBL.BHENT = B.Entry_ty And VATTBL.Tran_cd = B.Tran_cd And VATTBL.itserial= B.itserial )
where VATTBL.bhent in ('PT', 'EP','dn','PR') and b.ADVATPER1 = 1 and b.ADDLVAT1 <> 0 AND VATTBL.ST_TYPE IN('','LOCAL')
AND VATTBL.VATTYPE='' AND VATTBL.TAX_NAME LIKE '%VAT%' AND  VATTBL.S_tax <> ''
AND (VATTBL.DATE BETWEEN @SDATE AND @EDATE)  group by B.ADVATPER1,IT_MAST.it_name
-----  Added by SurajKumawat for Bug-26469 date on 02-0-2016 eND :
SELECT @RCOUNT = 0
SET @RCOUNT = (SELECT COUNT(*) FROM #FORM24 WHERE PARTSR='8A1')
IF (@RCOUNT=0)
BEGIN
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'8A1','A',0,0,0,0,'')
END 

-----  Commented by SurajKumawat for Bug-26469 date on 02-0-2016 Start :
 --SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
 --SET @CHAR=65
 --DECLARE  CUR_FORM24 CURSOR FOR 
 --select distinct level1 from stax_mas where ST_TYPE='LOCAL' AND entry_ty in ('PT', 'EP') --AND level1 in (1,4,12.5)
 --OPEN CUR_FORM24
 --FETCH NEXT FROM CUR_FORM24 INTO @PER
 --WHILE (@@FETCH_STATUS=0)
 --BEGIN
	
	--Begin
	
		   
	--	 INSERT INTO #FORM24
	--	(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) 		  
	--	   SELECT 1, 
	--	   '8B', 
	--	   'H', 
	--	   @Per, 
	--	   Sum(VATTBL.VATONAMT),
	--	   Sum(VATTBL.TAXAMT),
	--	   0,
	--	   it_mast.It_name
	--	   FROM VATTBL 
 --          Inner Join IT_MAST ON IT_MAST.It_code = VATTBL.It_code
	--	   inner join litem_vw B on (VATTBL.BHENT = B.Entry_ty And VATTBL.Tran_cd = B.Tran_cd And VATTBL.It_code = B.It_code)
 --          where VATTBL.bhent in ('PT', 'EP')  AND VATTBL.PER = @PER
 --          AND VATTYPE='Non Vat'
	--	   AND (VATTBL.DATE BETWEEN @SDATE AND @EDATE)
	--	   group by IT_MAST.it_name, VATTBL.PER
		   
	--	    SET @CHAR=@CHAR+1
	--End

  
 -- FETCH NEXT FROM CUR_FORM24 INTO @PER
 --END
 --CLOSE CUR_FORM24
 --DEALLOCATE CUR_FORM24
 -----  Commented by SurajKumawat for Bug-26469 date on 02-0-2016 End :
----------------------------------------------------------------------------------------------------------
 -----  Added by SurajKumawat for Bug-26469 date on 02-0-2016 Start :
INSERT INTO #FORM24(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM)
SELECT 1, '8B', 'A',VATTBL.Per
, isnull(Sum(case when VATTBL.bhent IN('pt','EP') then VATTBL.VATONAMT else -VATTBL.VATONAMT end ),0)
,ISNULL( Sum(case when VATTBL.bhent IN('pt','EP') then VATTBL.TAXAMT else -VATTBL.TAXAMT end ),0),
		   0,it_mast.It_name  FROM VATTBL   Inner Join IT_MAST ON IT_MAST.It_code = VATTBL.It_code
           where VATTBL.bhent in ('PT', 'EP','DN','PR')  AND VATTBL.PER IN(1,4,12.5) AND VATTBL.ST_TYPE IN('','LOCAL')
           AND VATTYPE='Non Vat'  AND VATTBL.TAX_NAME LIKE '%VAT%' AND (VATTBL.DATE BETWEEN @SDATE AND @EDATE) AND  VATTBL.S_tax <> ''
		   group by VATTBL.PER,IT_MAST.it_name
-----  Added by SurajKumawat for Bug-26469 date on 02-0-2016 End:
----------------------------------------------------------------------------------------------------------

SELECT @RCOUNT = 0
SET @RCOUNT = (SELECT COUNT(*) FROM #FORM24 WHERE PARTSR='8B')
IF (@RCOUNT=0)
begin
	INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'8B','A',0,0,0,0,'')
end

--SELECT @RCOUNT = 0
--SET @RCOUNT = (SELECT COUNT(*) FROM #FORM24 WHERE PARTSR='8A' AND RATE = 1)
--IF (@RCOUNT=0)
--INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'8A','A',1,0,0,0,'')

--SELECT @RCOUNT = 0
--SET @RCOUNT = (SELECT COUNT(*) FROM #FORM24 WHERE PARTSR='8A' AND RATE = 4)
--IF (@RCOUNT=0)
--INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'8A','B',4,0,0,0,'')


--SELECT @RCOUNT = 0
--SET @RCOUNT = (SELECT COUNT(*) FROM #FORM24 WHERE PARTSR='8A' AND RATE = 12.5)
--IF (@RCOUNT=0)
--INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'8A','C',12.5,0,0,0,'')

--SELECT @RCOUNT = 0
--SET @RCOUNT = (SELECT COUNT(*) FROM #FORM24 WHERE PARTSR='8A1'  AND RATE = 0.5)
--IF (@RCOUNT=0)
--INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'8A1','E',0.5,0,0,0,'')

--SELECT @RCOUNT = 0
--SET @RCOUNT = (SELECT COUNT(*) FROM #FORM24 WHERE PARTSR='8A1'  AND RATE = 1)
--IF (@RCOUNT=0)
--INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'8A1','F',1,0,0,0,'')

--DECLARE @RCOUNT1 INT
--SELECT @RCOUNT1 = 0
--SET @RCOUNT1 = (SELECT COUNT(*) FROM #FORM24 WHERE PARTSR='8B')
--IF (@RCOUNT1=0)
--INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'8B','H',0,0,0,0,'')
--------------------------------------------------------------------------------------------------------
 
--Part 9A & B
----Sales Against Tax Invoice
-------------- ANNEXURE B 1 VALUE  START
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=M.NET_AMT,AMT2=M.NET_AMT,AMT3=M.NET_AMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.MAILNAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),
STM.FORM_NM,AC1.S_TAX,Item=space(50),Qty=9999999999999999999.9999
INTO #AnnexB_24
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2
INSERT INTO #AnnexB_24 EXECUTE USP_REP_UP_ANNEXURE_B'','','',@SDATE,@EDATE,'','','','',0,0,'','','','','','','','','2013-2014',''

-------------- ANNEXURE B 1 VALUE  END
SET @AMTA1=0
SELECT @AMTA1=ISNULL(SUM(AMT3),0) FROM #AnnexB_24 WHERE PART = 1 AND PARTSR = 1
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','A',0,@AMTA1,0,0,'') 

----Sales Other than Tax Invoice
SET @AMTA1=0
SELECT @AMTA1=SUM(GRO_AMT ) from VATTBL WHERE BHENT In('ST') AND (DATE BETWEEN @SDATE AND @EDATE) 
AND VATTYPE='' and (TAX_NAME not LIKE '%VAT%' and  TAX_NAME not LIKE '%EXEMPTED%' ) And ST_TYPE IN ('local','')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','B',0,@AMTA1,0,0,'')

----Sales Exempted
SET @AMTA1=0
SELECT @AMTA1=SUM(gRO_AMT) from VATTBL WHERE BHENT In('ST') AND (DATE BETWEEN @SDATE AND @EDATE) 
And VATTYPE='' AND TAX_NAME like'%Exempted%'  And ST_TYPE IN ('LOCAL','')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','C',0,@AMTA1,0,0,'')

--Below is New Code Added on May 13,2010 as UP Form 24 has been added with new sections in it
--Modified by:Rakesh Varma
----------------------------------------------------------------------------------------------------------
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','C1',0,0,0,0,'')
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','C2',0,0,0,0,'')
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','C3',0,0,0,0,'')
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','C4',0,0,0,0,'')
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','C5',0,0,0,0,'')
----------------------------------------------------------------------------------------------------------
--and ac_name not like '%Rece%' 
 --and taxamt=0 St_type = 'Out of State' and tax_name <> '')
 --and ac_name not like '%Rece%' and St_type = 'Out of State' And tax_name<> ''

----Sales Inter State against 'C' Form
SET @AMTA1=0
SELECT @AMTA1=SUM(GRO_AMT) from VATTBL WHERE BHENT In('ST') AND (DATE BETWEEN @SDATE AND @EDATE) 
And ST_TYPE = 'OUT OF STATE' AND VATTYPE='' 
AND RTRIM(LTRIM(REPLACE(REPLACE(REPLACE(REPLACE(rFORM_NM,'FORM',''),'-',''),'/',''),' ',''))) ='C'  
AND TAX_NAME NOT LIKE '%EXEMPTED%' AND U_IMPORM not IN ('Branch Transfer', 'Consignment Transfer')

SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','D',0,@AMTA1,0,0,'')

----Sales Inter State Without against 'C' Form
SET @AMTA1=0
SELECT @AMTA1=SUM(GRO_AMT) from VATTBL WHERE BHENT In('ST') AND (DATE BETWEEN @SDATE AND @EDATE) 
And ST_TYPE = 'OUT OF STATE' AND VATTYPE='' 
AND RTRIM(LTRIM(REPLACE(REPLACE(REPLACE(REPLACE(rFORM_NM,'FORM',''),'-',''),'/',''),' ',''))) NOT IN('C','F') AND RFORM_NM <> ''
AND TAX_NAME NOT LIKE '%EXEMPTED%' AND U_IMPORM not IN ('Branch Transfer', 'Consignment Transfer')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','E',0,@AMTA1,0,0,'')

----Sales Export
SET @AMTA1 = 0
SELECT @AMTA1=ISNULL(SUM(a.GRO_AMT),0) from VATTBL A inner join stmain b  on(a.BHENT=b.entry_ty and a.TRAN_CD =b.Tran_cd) 
WHERE a.BHENT In('ST') AND (a.DATE BETWEEN @SDATE AND @EDATE) 
And a.ST_TYPE = 'OUT OF COUNTRY' AND a.VATTYPE='' AND a.U_IMPORM IN('Export Out of India','High Sea Sales','Direct Exports')
and b.VATMTYPE <> 'Sale in the course of import into India'  and LTRIM(REPLACE(REPLACE(REPLACE(REPLACE(A.RFORM_NM,'FORM',''),'-',''),'/',''),' ',''))='H' 
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','F',0,@AMTA1,0,0,'')

----Sales in course of Import 
SET @AMTA1 = 0
SELECT @AMTA1=ISNULL(SUM(a.GRO_AMT),0) from VATTBL A inner join stmain b  on(a.BHENT=b.entry_ty and a.TRAN_CD =b.Tran_cd) 
WHERE a.BHENT In('ST') AND (a.DATE BETWEEN @SDATE AND @EDATE) 
And a.ST_TYPE = 'OUT OF COUNTRY' AND a.VATTYPE='' AND a.U_IMPORM not IN('Export Out of India','High Sea Sales','Direct Exports')
and b.VATMTYPE ='Sale in the course of import into India'  and LTRIM(REPLACE(REPLACE(REPLACE(REPLACE(A.RFORM_NM,'FORM',''),'-',''),'/',''),' ',''))='H' 
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','G',0,@AMTA1,0,0,'')

----Sales Outside State
SET @AMTA1 = 0
SELECT @AMTA1=SUM(GRO_AMT) from VATTBL WHERE BHENT In('ST') AND (DATE BETWEEN @SDATE AND @EDATE)
And ST_TYPE = 'OUT OF STATE' AND VATTYPE=''  and RFORM_NM = '' 
AND TAX_NAME NOT LIKE '%EXEMPTED%' AND U_IMPORM not IN ('Branch Transfer', 'Consignment Transfer')

INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','H',0,@AMTA1,0,0,'')

----Sales Consignment
SET @AMTA1 = 0
SELECT @AMTA1=isnull(SUM(GRO_AMT),0) from VATTBL WHERE BHENT In('ST') AND (DATE BETWEEN @SDATE AND @EDATE) 
And VATTYPE='' AND ( U_IMPORM IN ('Branch Transfer', 'Consignment Transfer') and RTRIM(LTRIM(REPLACE(REPLACE(REPLACE(REPLACE(RFORM_NM,'FORM',''),'-',''),'/',''),' ','')))='F') 
AND ST_TYPE in('OUT OF STATE','','Local')  AND TAX_NAME NOT LIKE '%EXEMPTED%' 
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','I',0,@AMTA1,0,0,'')

----Sales Any Other
SET @AMTA1 = 0
SELECT @AMTA1=isnull(SUM(GRO_AMT),0) from VATTBL WHERE BHENT In('ST') AND (DATE BETWEEN @SDATE AND @EDATE) 
And VATTYPE='' AND TAX_NAME LIKE '%EXEMPTED%' AND ST_TYPE not in('','Local')
AND U_IMPORM not IN ('Branch Transfer', 'Consignment Transfer')
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','J',0,@AMTA1,0,0,'')



SET @AMTB1=0
SELECT @AMTB1=SUM(AMT1) 
from #FORM24 
WHERE PARTSR = '9A' AND SRNO IN ('A','B','C','D','E','F','G','H','I','J')

INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','K',0,@AMTB1,0,0,'')
---------------------------------------------------------------------------------------------------

SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0
-----Sales return
 ----ANNEXURE B - 1 PART A DETAILS
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=m.NET_AMT,AMT2=m.NET_AMT,AMT3=m.NET_AMT,
M.INV_NO,M.DATE,PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),
STM.FORM_NM,AC1.S_TAX,Item=space(150),qty=9999999999999999999.9999,MAIN_TY=M.ENTRY_TY, MAIN_CD = M.TRAN_CD, MAIN_INV=M.INV_NO,  MAIN_DATE = M.DATE
INTO #AnnexA_anx_dt_fetch
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2
insert into #AnnexA_anx_dt_fetch  EXECUTE USP_REP_UP_ANNEXURE_B1'','','',@sdate,@edate,'','','','',0,0,'','','','','','','','','2013-2014',''
SET @AMTA1 = 0
select @AMTA1=ISNULL(SUM(AMT3),0) from #AnnexA_anx_dt_fetch WHERE  PARTSR = '3'
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','L',0,@AMTA1,0,0,'')
---------------------------------------------------------------------------------------------------

-----xiii. Net amount of sales
SELECT @AMTB1=SUM(AMT1) 
from #FORM24 
WHERE PARTSR = '9A' AND SRNO IN ('A','B','C','C1','C2','C3','C4','C5','D','E','F','G','H','I','J')
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A','M',0,@AMTB1-@AMTA1,0,0,'')
---------------------------------------------------------------------------------------------------

-----Non Vat Sales
----Taxable turnover of sale
SET @AMTB1=0
SELECT @AMTB1=isnull(SUM(Gro_amt),0) from VATTBL WHERE BHENT In('ST') AND (DATE BETWEEN @SDATE AND @EDATE)
AND VATTYPE='Non Vat' AND (TAX_NAME LIKE '%VAT%') 
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9B','A',0,@AMTB1,0,0,'') --@AMTB1
---------------------------------------------------------------------------------------------------

---Exempted turnover of sale
SET @AMTB1=0
SELECT @AMTB1=SUM(VATONAMT) from VATTBL WHERE BHENT In('ST') AND (DATE BETWEEN @SDATE AND @EDATE) 
And  VATTYPE='Non Vat' AND TAX_NAME like'%Exempted%'
SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9B','B',0,@AMTB1,0,0,'')
---------------------------------------------------------------------------------------------------

---Tax paid turnover of goods
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9B','C',0,0,0,0,'')

---Sale in Principal's A/c -
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9B','D',0,0,0,0,'')

---(a) U.P. principal
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9B','E',0,0,0,0,'')

--(b) Ex. U.P. principal
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9B','F',0,0,0,0,'')

--Any other sale
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9B','G',0,0,0,0,'')

--Total
SET @AMTB1=0
SELECT @AMTB1=SUM(AMT1) 
from #FORM24 
WHERE PARTSR = '9B' AND SRNO IN ('A','B','C','D','E','F','G')
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9B','H',0,@AMTB1,0,0,'')



--9(b)--vi & vii
----Less – sales return (annexure B-1)
----------------------------------------------------------------------------------------------------------
SET @AMTA1=0
select @AMTA1=ISNULL(SUM(AMT3),0) from #AnnexA_anx_dt_fetch WHERE PARTSR = '4'
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9B','I',0,@AMTA1,0,0,'')
----------------------------------------------------------------------------------------------------------

-- Net Amount of Sale
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9B','J',0,@AMTB1-@AMTA1,0,0,'')
----------------------------------------------------------------------------------------------------------




--Grand Total
SELECT @AMTA1=0,@AMTB1=0

SELECT @AMTA1=SUM(AMT1) 
from #FORM24 
WHERE PARTSR = '9A' AND SRNO IN ('M')
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END

SELECT @AMTB1=SUM(AMT1) 
from #FORM24 
WHERE PARTSR = '9B' AND SRNO IN ('J')
SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END

INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9B','K',0,@AMTA1+@AMTB1,0,0,'')


--9(c)
--Below is New Code Added on May 13,2010 as UP Form 24 has been added with new sections in it
--Modified by:Rakesh Varma
-- Sales through Commission Agent
----------------------------------------------------------------------------------------------------------
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9B2','A',0,0,0,0,'')
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9B2','B',0,0,0,0,'')
----------------------------------------------------------------------------------------------------------

--Part 10A & B
---Vat Rate Wise Sales Breakup

 ----- Commented by Suraj Kumawat for bug-26469 and date ; 02-07-2016  Start
 --SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
 --SET @CHAR=65
 --DECLARE  CUR_FORM24 CURSOR FOR 
 --select distinct level1 from stax_mas where ST_TYPE='LOCAL' AND entry_ty in ('ST') AND level1 in (1,4,12.5)
 --OPEN CUR_FORM24
 --FETCH NEXT FROM CUR_FORM24 INTO @PER
 --WHILE (@@FETCH_STATUS=0)
 --BEGIN
	
	--Begin
	
	--	INSERT INTO #FORM24
	--	(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) 		  
	--	   SELECT 1, 
	--	   '10A', 
	--	   CASE WHEN @per = 1.00 THEN 'A' WHEN (@per = 4.00) THEN 'B' WHEN (@per = 12.50) THEN 'C' ELSE 'D'  END, 
	--	   @Per, 
	--	   Sum(VATTBL.VATONAMT),
	--	   Sum(VATTBL.TAXAMT),
	--	   0,
	--	   it_mast.It_name
	--	   FROM VATTBL 
 --          Inner Join IT_MAST ON IT_MAST.It_code = VATTBL.It_code
	--	   inner join litem_vw B on (VATTBL.BHENT = B.Entry_ty And VATTBL.Tran_cd = B.Tran_cd And VATTBL.It_code = B.It_code)
 --          where VATTBL.bhent in ('ST') And VATTBL.S_tax <> '' AND VATTBL.PER IN (1,4,12.5) 
 --          AND VATTBL.TAX_NAME LIKE '%VAT%' AND VATTBL.TAXAMT <> 0 AND VATTYPE='' AND VATTBL.PER = @PER
	--	   AND (VATTBL.DATE BETWEEN @SDATE AND @EDATE)
	--	   group by IT_MAST.it_name, VATTBL.PER
		   
	--       SET @CHAR=@CHAR+1
	--End

 -- FETCH NEXT FROM CUR_FORM24 INTO @PER
 --END
 --CLOSE CUR_FORM24
 --DEALLOCATE CUR_FORM24

----- Commented by Suraj Kumawat for bug-26469 and date ; 02-07-2016  END ;
----- Added by Suraj Kumawat for bug-26469 and date ; 02-07-2016  Start;
		INSERT INTO #FORM24(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) 	SELECT 1,'10A', 'A'
,VATTBL.per,isnull(Sum(CASE WHEN VATTBL.BHENT ='ST' THEN VATTBL.VATONAMT ELSE -VATTBL.VATONAMT END),0),
isnull(Sum(CASE WHEN VATTBL.BHENT ='ST' THEN VATTBL.TAXAMT ELSE -VATTBL.TAXAMT END),0), 0, it_mast.It_name
FROM VATTBL   Inner Join IT_MAST ON IT_MAST.It_code = VATTBL.It_code  
inner join litem_vw B on (VATTBL.BHENT = B.Entry_ty And VATTBL.Tran_cd = B.Tran_cd And VATTBL.itserial = B.itserial)
 where VATTBL.bhent in ('ST','SR','CN')   AND ST_TYPE IN('','LOCAL')  AND VATTBL.PER IN (1,4,12.5) 
   AND VATTBL.TAX_NAME LIKE '%VAT%'  AND VATTYPE=''  and b.ADVATPER1  <> 1
AND (VATTBL.DATE BETWEEN @SDATE AND @EDATE)
group by VATTBL.PER,IT_MAST.it_name 


----- Added by Suraj Kumawat for bug-26469 and date ; 02-07-2016  End; 
 
----- Commented by Suraj Kumawat for bug-26469 and date ; 02-07-2016  Start; 
 -- SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
 --SET @CHAR=65
 --DECLARE  CUR_FORM24 CURSOR FOR 
 --select distinct level1 from stax_mas where ST_TYPE='LOCAL' AND entry_ty in ('ST') AND level1 in (0.5, 1.00)
 --OPEN CUR_FORM24
 --FETCH NEXT FROM CUR_FORM24 INTO @PER
 --WHILE (@@FETCH_STATUS=0)
 --BEGIN
	
	--Begin
	
	--	 INSERT INTO #FORM24
	--	(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) 		  
	--	   SELECT 1, 
	--	   '10A1', 
	--	   CASE WHEN @per = 0.5 THEN 'E' WHEN (@per = 1.00) THEN 'F' ELSE 'G'  END, 
	--	   @Per, 
	--	   Sum(VATTBL.VATONAMT),
	--	   IsNull(Sum(B.ADDLVAT1), 0),
	--	   0,
	--	   it_mast.It_name
	--	   FROM VATTBL 
 --          Inner Join IT_MAST ON IT_MAST.It_code = VATTBL.It_code
	--	   inner join litem_vw B on (VATTBL.BHENT = B.Entry_ty And VATTBL.Tran_cd = B.Tran_cd And VATTBL.It_code = B.It_code)
 --          where VATTBL.bhent in ('ST') And VATTBL.S_tax <> '' AND VATTBL.PER IN (0.5, 1.00) 
 --          AND VATTBL.TAX_NAME LIKE '%VAT%' AND B.ADDLVAT1 <> 0 AND VATTYPE=''  AND VATTBL.PER = @PER
	--	   AND (VATTBL.DATE BETWEEN @SDATE AND @EDATE)
	--	   group by IT_MAST.it_name, VATTBL.PER
	
	--	    SET @CHAR=@CHAR+1
	--End

 -- FETCH NEXT FROM CUR_FORM24 INTO @PER
 --END
 --CLOSE CUR_FORM24
 --DEALLOCATE CUR_FORM24
 ----- Commented by Suraj Kumawat for bug-26469 and date ; 02-07-2016  End; 
 ---
 --Commented by Suraj Kumawat for bug-26469 and date ; 02-07-2016  Start; 
		 INSERT INTO #FORM24(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) 		  
		   SELECT 1, '10A1','A',b.ADVATPER1
		   ,isnull(Sum(CASE WHEN VATTBL.BHENT='ST' THEN VATTBL.VATONAMT ELSE -VATTBL.VATONAMT END ),0)
		   ,IsNull(Sum(CASE WHEN VATTBL.BHENT='ST' THEN  B.ADDLVAT1 ELSE -  B.ADDLVAT1 END), 0),
		   0,it_mast.It_name  FROM VATTBL   Inner Join IT_MAST ON IT_MAST.It_code = VATTBL.It_code
		   inner join litem_vw B on (VATTBL.BHENT = B.Entry_ty And VATTBL.Tran_cd = B.Tran_cd And VATTBL.itserial = B.itserial)
           where VATTBL.bhent in ('ST','SR','CN')  AND b.ADVATPER1 = 1  and VATTBL.ST_TYPE IN('','LOCAL')
           AND VATTBL.TAX_NAME LIKE '%VAT%' AND VATTYPE=''   AND (VATTBL.DATE BETWEEN @SDATE AND @EDATE)
		   group by  B.ADVATPER1 ,IT_MAST.it_name
 
 ---aDDED by Suraj Kumawat for bug-26469 and date ; 02-07-2016  Start; 
  ---Commented by Suraj Kumawat for bug-26469 and date ; 02-07-2016  End; 
 --SELECT @AMTA1=0,@AMTB1=0,@AMTC1=0,@AMTD1=0,@AMTE1=0,@AMTF1=0,@AMTG1=0,@AMTH1=0,@AMTI1=0,@AMTJ1=0,@AMTK1=0,@AMTL1=0,@AMTM1=0,@AMTN1=0,@AMTO1=0 
 --SET @CHAR=65
 --DECLARE  CUR_FORM24 CURSOR FOR 
 --select distinct level1 from stax_mas where ST_TYPE='LOCAL' AND entry_ty in ('ST') --AND level1 in (1,4,12.5)
 --OPEN CUR_FORM24
 --FETCH NEXT FROM CUR_FORM24 INTO @PER
 --WHILE (@@FETCH_STATUS=0)
 --BEGIN
	
	--Begin
	
	--	 INSERT INTO #FORM24
	--	(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM) 		  
	--	   SELECT 1, 
	--	   '10B', 
	--	   'H', 
	--	   @Per, 
	--	   Sum(VATTBL.VATONAMT),
	--	   Sum(VATTBL.TAXAMT),
	--	   0,
	--	   it_mast.It_name
	--	   FROM VATTBL 
 --          Inner Join IT_MAST ON IT_MAST.It_code = VATTBL.It_code
	--	   inner join litem_vw B on (VATTBL.BHENT = B.Entry_ty And VATTBL.Tran_cd = B.Tran_cd And VATTBL.It_code = B.It_code)
 --          where VATTBL.bhent in ('ST') 
 --          AND VATTYPE='Non Vat' AND VATTBL.PER = @PER
	--	   AND (VATTBL.DATE BETWEEN @SDATE AND @EDATE)
	--	   group by IT_MAST.it_name, VATTBL.PER
		   
	--	    SET @CHAR=@CHAR+1
	--End

 -- FETCH NEXT FROM CUR_FORM24 INTO @PER
 --END
 --CLOSE CUR_FORM24
 --DEALLOCATE CUR_FORM24
 
 ---aDDED by Suraj Kumawat for bug-26469 and date ; 02-07-2016  End; 
--------------------------------------------------------------------------------------------------------
INSERT INTO #FORM24(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,PARTY_NM)  SELECT 1, 
		   '10B', 'A',VATTBL.Per, isnull(Sum(CASE WHEN VATTBL.BHENT='ST' THEN VATTBL.VATONAMT ELSE -VATTBL.VATONAMT END),0)
		   , isnull(Sum(CASE WHEN VATTBL.BHENT='ST' THEN VATTBL.TAXAMT ELSE -VATTBL.TAXAMT END),0), 0, it_mast.It_name
		   FROM VATTBL 
           Inner Join IT_MAST ON IT_MAST.It_code = VATTBL.It_code
		   inner join litem_vw B on (VATTBL.BHENT = B.Entry_ty And VATTBL.Tran_cd = B.Tran_cd And VATTBL.ItSerial = B.ItSerial)
           where VATTBL.bhent in ('ST','SR','CN')  AND VATTYPE='Non Vat' AND VATTBL.PER IN(1,4,12.5) and VATTBL.ST_TYPE IN('','LOCAL')
		   AND (VATTBL.DATE BETWEEN @SDATE AND @EDATE)
		   group by VATTBL.PER,IT_MAST.it_name 

SELECT @RCOUNT = 0
SET @RCOUNT = (SELECT COUNT(*) FROM #FORM24 WHERE PARTSR='10A')
IF (@RCOUNT=0)
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10A','A',0,0,0,0,'')

SELECT @RCOUNT = 0
SET @RCOUNT = (SELECT COUNT(*) FROM #FORM24 WHERE PARTSR='10A1')
IF (@RCOUNT=0)
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10A1','A',0,0,0,0,'')

SELECT @RCOUNT = 0
SET @RCOUNT = (SELECT COUNT(*) FROM #FORM24 WHERE PARTSR='10B')
IF (@RCOUNT=0)
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10B','A',0,0,0,0,'')
--------------------------------------------------------------------------------------------------------
 ---11. Installment of compounding scheme, if any
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10B1','A',0,0,0,0,'')

 ---12. Amount of T.D.S.
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10B2','A',0,0,0,0,'')

--Part 13
----Total Tax on Purchases (purchase without registration)
SET @AMTA1=0
SELECT @AMTA1=SUM(TAXAMT) from VATTBL WHERE BHENT In('PT','EP') AND (DATE BETWEEN @SDATE AND @EDATE)
AND S_TAX = '' and ST_TYPE in('','Local') and TAX_NAME like '%vat%'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10C','A',0,@AMTA1,0,0,'')

--INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'9A1','A',0,@AMTA1,0,0,'')
----Total Tax on Sales
--SELECT @AMTA1=-(SUM(TAXAMT)+@AMTO1) FROM  #form_24 where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and Tax_name <>'' --and ac_name not like '%Rece%' 
SET @AMTA1=0
SELECT @AMTA1=SUM(TAXAMT) from VATTBL WHERE BHENT In('ST') AND (DATE BETWEEN @SDATE AND @EDATE) 
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10C','B',0,@AMTA1,0,0,'')

----Installment on Compound Scheme
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10C','C',0,0,0,0,'')

----TDS Amount
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10C','D',0,0,0,0,'')

--Part 14
--i) 
----ITC brought forward from previous tax period
set @balamt = 0
SET @AMTA1 = 0
SELECT @AMTA1=ISNULL(SUM(C.amount),0) FROM  JVMAIN A  
inner JOIN JVACDET C on (A.entry_ty = C.ENTRY_TY AND A.TRAN_CD = C.TRAN_CD AND C.amt_ty ='CR')
where A.ENTRY_TY in('J4')  and A.VAT_ADJ='ITC brought forward from previous tax period'  AND A.party_nm ='VAT PAYABLE' AND ( A.date BETWEEN @SDATE AND @EDATE)
set @balamt =  @balamt +  @AMTA1
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','A',0,@AMTA1,0,0,'')

----ITC earned during the tax period
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','B',0,0,0,0,'')

----ITC earned during the Year
set @AMTA1=0
--ii)
SELECT @AMTA1=SUM(TAXAMT) from VATTBL WHERE BHENT In('PT','EP') AND (DATE BETWEEN @SDATE AND @EDATE)
AND  S_tax <> ''  and ST_TYPE in('','Local') and TAX_NAME like '%vat%'
set @balamt =  @balamt +  @AMTA1
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','C',0,@AMTA1,0,0,'')

--(b) On purchases made through purchasing commission agent against certificate in form VI
set @AMTA1 = 0
set @balamt =  @balamt +  @AMTA1
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','D',0,0,0,0,'')

--Total
SELECT @AMTG1=0
SELECT @AMTG1=SUM(AMT1) FROM #FORM24 WHERE PARTSR = '10D' AND SRNO IN ('C','D')
SET @AMTG1=CASE WHEN @AMTG1 IS NULL THEN 0 ELSE @AMTG1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','E',0,@AMTG1,0,0,'')

--(c) Installment of ITC on opening stock due in the tax period
set @AMTA1 = 0
set @balamt =  @balamt +  @AMTA1
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','F',0,0,0,0,'')

--(d) Installment of ITC on capital goods due in the tax period
SELECT @AMTA2=0, @AMTB2=0, @AMTC2=0
set @AMTA1 = 0
set @balamt =  @balamt +  @AMTA1
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','G',0,0,0,0,'')

--Total
SELECT @AMTG2=0
SELECT @AMTG2=SUM(AMT1) FROM #FORM24 WHERE PARTSR = '10D' AND SRNO IN ('F','G')
SET @AMTG2=CASE WHEN @AMTG2 IS NULL THEN 0 ELSE @AMTG2 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','H',0,@AMTG2,0,0,'')

--(e) ITC reversed during the tax period
set @AMTA1  = 0
SELECT @AMTA1 =SUM(TAXAMT) from VATTBL WHERE BHENT In('PR','DN') AND (DATE BETWEEN @SDATE AND @EDATE) 
AND  S_tax <> ''  and ST_TYPE in('','Local') and TAX_NAME like '%vat%'
set @balamt =  @balamt -  @AMTA1
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','I',0,@AMTA1 ,0,0,'')

--(f) Admissible ITC in the tax period (a+b+c+d-e)
set @AMTA1 = 0 
select @AMTA1 = isnull(sum(CASE WHEN SRNO <> 'I' THEN amt1 ELSE - amt1 END),0) from #FORM24 WHERE PARTSR ='10D' AND SRNO IN('C','D','G','I')
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','J',0,@AMTA1,0,0,'')
---INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','J',0,@AMTA1+@AMTA2-(@AMTB1+@AMTC1+@AMTB2+@AMTC2),0,0,'')

--iii(a) Adjustment of ITC against tax payable

--SELECT @AMTA1=SUM(TAXAMT) from VATTBL WHERE BHENT In('PT','EP') AND (DATE BETWEEN @SDATE AND @EDATE) 
--AND  S_tax <> '' AND st_type = 'LOCAL'

--SELECT @AMTB1=SUM(TAXAMT) from VATTBL WHERE BHENT In('PR') AND (DATE BETWEEN @SDATE AND @EDATE) 
--AND  S_tax <> '' AND  st_type = 'LOCAL'

--SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
--SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
--SET @AMTC1=CASE WHEN @AMTC1 IS NULL THEN 0 ELSE @AMTC1 END
SET @AMTA1 = 0
SELECT @AMTA1=ISNULL(SUM(C.amount),0) FROM  JVMAIN A  
inner JOIN JVACDET C on (A.entry_ty = C.ENTRY_TY AND A.TRAN_CD = C.TRAN_CD AND C.amt_ty ='CR')
where A.ENTRY_TY in('J4')  and A.VAT_ADJ='Adjustment of ITC against tax payable'  AND A.party_nm ='VAT PAYABLE' AND ( A.date BETWEEN @SDATE AND @EDATE) 
set @balamt =  @balamt + @AMTA1
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','K',0,@AMTA1,0,0,'')


--iii(b) Adjustment of ITC against CST
SET @AMTA1 = 0

SELECT @AMTA1=ISNULL(SUM(C.amount),0) FROM  JVMAIN A  
inner JOIN JVACDET C on (A.entry_ty = C.ENTRY_TY AND A.TRAN_CD = C.TRAN_CD AND C.amt_ty ='CR')
where A.ENTRY_TY in('J4')  and A.VAT_ADJ='Input Vat Adjustment Against CST'  AND A.party_nm ='VAT PAYABLE' AND ( A.date BETWEEN @SDATE AND @EDATE) 
set @balamt =  @balamt + @AMTA1
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','L',0,@AMTA1,0,0,'')

--iv. ITC carried forward to the next tax period, if any
SET @AMTA1 = 0
SELECT @AMTA1=ISNULL(SUM(C.amount),0) FROM  JVMAIN A  
inner JOIN JVACDET C on (A.entry_ty = C.ENTRY_TY AND A.TRAN_CD = C.TRAN_CD AND C.amt_ty ='CR')
where A.ENTRY_TY in('J4')  and A.VAT_ADJ='ITC carried forward to the next tax period'  AND A.party_nm ='VAT PAYABLE' AND ( A.date BETWEEN @SDATE AND @EDATE)
set @balamt =  @balamt + @AMTA1
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','M',0,@AMTA1,0,0,'')

--- Total 
SET @AMTA1 = 0
SET @AMTA1 = @balamt 
--select @AMTA1 =ISNULL(SUM(AMT1),0) FROM  #FORM24 WHERE PART = 1 AND PARTSR ='10D' AND SRNO IN('A','C','D','F','G','I','J','K','L','M')
--SET @AMTG1=CASE WHEN @AMTG1 IS NULL THEN 0 ELSE @AMTG1 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10D','N',0,@AMTA1,0,0,'')
----------------------------------------------------------------------------------------------------------

--Part 15
----Total Tax Payable [13]
SELECT @AMTA1=0
SELECT @AMTA1=ISNULL(SUM(AMT1),0) FROM  #form24 where Partsr = '10C' 
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10E','A',0,@AMTA1,0,0,'')

---Total ITC Adjustment [14]
SELECT @AMTA1=0,@AMTA2=0
SELECT @AMTA1=ISNULL(SUM(AMT1),0) FROM  #form24 where Partsr = '10D'AND SRNO='K'
SELECT @AMTA2=ISNULL(SUM(AMT1),0) FROM  #form24 where Partsr = '10D'AND SRNO='L'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10E','B',0,@AMTA1+@AMTA2,0,0,'')

---Net tax
SELECT @AMTA1=0,@AMTA2=0
SELECT @AMTA1=SUM(AMT1) FROM  #form24 where Partsr = '10E'AND SRNO='A'
SELECT @AMTA2=SUM(AMT1) FROM  #form24 where Partsr = '10E'AND SRNO='B'
SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10E','C',0,@AMTA1-@AMTA2,0,0,'')

--A- Tax deposited in Bank / Treasury
--Part 16
----Bank Payment Details
Declare @TAXONAMT as numeric(12,2),@TAXAMT1 as numeric(12,2),@ITEMAMT as numeric(12,2),@FORM_NM as varchar(30)
SELECT @TAXONAMT=0,@TAXAMT =0,@ITEMAMT =0,@INV_NO ='',@DATE ='',@PARTY_NM ='',@ADDRESS ='',@ITEM ='',@FORM_NM='',@S_TAX ='',@QTY=0

SET @CHAR=65

SET @PER = 0
declare Cur_VatPay cursor  for

select B.bank_nm, b.u_chalno, B.Date, b.net_amt from VATTBL A
Inner join Bpmain B on (A.Bhent = B.Entry_ty and A.Tran_cd = B.Tran_cd)
where BHENT = 'BP' And B.Date Between @sdate and @edate And B.Party_nm like '%VAT%'
open Cur_VatPay
FETCH NEXT FROM Cur_VatPay INTO @PARTY_NM,@INV_NO,@DATE,@TAXONAMT
	WHILE (@@FETCH_STATUS=0)
	BEGIN

	SET @PARTY_NM=CASE WHEN @PARTY_NM IS NULL THEN '' ELSE @PARTY_NM END
	SET @INV_NO=CASE WHEN @INV_NO IS NULL THEN '' ELSE @INV_NO END
	SET @DATE=CASE WHEN @DATE IS NULL THEN '' ELSE @DATE END
	SET @TAXONAMT=CASE WHEN @TAXONAMT IS NULL THEN 0 ELSE @TAXONAMT END

	INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,INV_NO,DATE,PARTY_NM) VALUES (1,'10F','A',0,@TAXONAMT,0,0,@INV_NO,@DATE,@PARTY_NM)
	SET @CHAR=@CHAR+1
	FETCH NEXT FROM CUR_VatPay INTO @PARTY_NM,@INV_NO,@DATE,@TAXONAMT
END
CLOSE CUR_VatPay
DEALLOCATE CUR_VatPay

--------------------------------------------------------------------------------------------------------

SELECT @RCOUNT = 0

SET @RCOUNT = (SELECT COUNT(*) FROM #FORM24 WHERE PARTSR='10F')

IF (@RCOUNT=0)
begin
	INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10F','A',0,0,0,0,'')
end 
--------------------------------------------------------------------------------------------------------

--B- By adjustment against adjustment vouchers

SELECT @AMTA1=0
SELECT @AMTA2   =  ISNULL(SUM(AMT1),0)  FROM #FORM24 WHERE PARTSR='10F' AND PART = 1 AND SRNO ='A'
INSERT INTO #FORM24 (PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,party_nm) VALUES (1,'10G','A',0,0,@AMTA2,0,'')


Update #FORM24 set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
		            RATE = isnull(RATE,0), AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0), 
					AMT3 = isnull(AMT3,0), INV_NO = isnull(INV_NO,''), DATE = isnull(Date,''), 
					PARTY_NM = isnull(Party_nm,''), ADDRESS = isnull(Address,''),
					FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,'')
SELECT * FROM #FORM24 order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int)--, SRNO


END
--Print 'UP VAT FORM 24'

