set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

 -- =============================================
 -- Author:  Hetal L. Patel
 -- Create date: 16/05/2007
 -- Description: This Stored procedure is useful to generate RJ VAT FORM 13
 -- Modify date: 16/05/2007 
 -- Modified By: Madhavi Penumalli
 -- Modify date: 23/11/2009 
 -- =============================================
 -- Re-Modification
 -- Modified By: Rakesh varma
 -- Modify date: 05/Feb/2010
 -- =============================================
ALTER PROCEDURE [dbo].[USP_REP_RJ_FORM_13]
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
 ,@VSDATE=@SDATE
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

 DECLARE @RATE NUMERIC(12,2),@AMTA1 NUMERIC(12,2),@AMTB1 NUMERIC(12,2),@AMTC1 NUMERIC(12,2),
         @AMTD1 NUMERIC(12,2),@AMTE1 NUMERIC(12,2),@AMTF1 NUMERIC(12,2),@AMTG1 NUMERIC(12,2),
         @AMTH1 NUMERIC(12,2),@AMTI1 NUMERIC(12,2),@AMTJ1 NUMERIC(12,2),@AMTK1 NUMERIC(12,2),
         @AMTL1 NUMERIC(12,2),@AMTM1 NUMERIC(12,2),@AMTN1 NUMERIC(12,2),@AMTO1 NUMERIC(12,2)

 DECLARE @AMTA2 NUMERIC(12,2),@AMTB2 NUMERIC(12,2),@AMTC2 NUMERIC(12,2),@AMTD2 NUMERIC(12,2),
         @AMTE2 NUMERIC(12,2),@AMTF2 NUMERIC(12,2),@AMTG2 NUMERIC(12,2),@AMTH2 NUMERIC(12,2),
         @AMTI2 NUMERIC(12,2),@AMTJ2 NUMERIC(12,2),@AMTK2 NUMERIC(12,2),@AMTL2 NUMERIC(12,2),
         @AMTM2 NUMERIC(12,2),@AMTN2 NUMERIC(12,2),@AMTO2 NUMERIC(12,2)

 DECLARE @PER NUMERIC(12,2),@TAXAMT NUMERIC(12,2),@CHAR INT,@LEVEL NUMERIC(12,2)
 
 SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1) 
 INTO #VATAC_MAST
 FROM STAX_MAS
 WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''

 INSERT INTO #VATAC_MAST
 SELECT DISTINCT AC_NAME=SUBSTRING(AC_NAME1,2,CHARINDEX('"',SUBSTRING(AC_NAME1,2,100))-1)
 FROM STAX_MAS
 WHERE AC_NAME1 NOT IN ('"SALES"','"PURCHASES"') AND ISNULL(AC_NAME1,'')<>''
 
 Declare @NetEff as numeric (12,2), @NetTax as numeric (12,2)

----Temporary Cursor1
SELECT BHENT='PT',M.INV_NO,M.Date,A.AC_NAME,A.AMT_TY,STM.TAX_NAME,SET_APP=ISNULL(SET_APP,0),
STM.ST_TYPE,M.NET_AMT,M.GRO_AMT,TAXONAMT=M.GRO_AMT+M.TOT_DEDUC+M.TOT_TAX+M.TOT_EXAMT+M.TOT_ADD,
PER=STM.LEVEL1,MTAXAMT=M.TAXAMT,TAXAMT=A.AMOUNT,STM.FORM_NM,PARTY_NM=AC1.AC_NAME,AC1.S_TAX,M.U_IMPORM,
ADDRESS=LTRIM(AC1.ADD1)+ ' ' + LTRIM(AC1.ADD2) + ' ' + LTRIM(AC1.ADD3),M.TRAN_CD,VATONAMT=99999999999.99,
Dbname=space(20),ItemType=space(1),It_code=999999999999999999-999999999999999999,ItSerial=Space(5)
INTO #FORM_RJ_13
FROM PTACDET A 
INNER JOIN PTMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2 --A.AC_NAME IN ( SELECT AC_NAME FROM #VATAC_MAST)

alter table #FORM_RJ_13 add recno int identity

---Temporary Cursor2
SELECT PART=3,PARTSR='AAA',SRNO='AAA',RATE=99.999,AMT1=NET_AMT,AMT2=M.TAXAMT,AMT3=M.TAXAMT,M.INV_NO,M.DATE,
PARTY_NM=AC1.AC_NAME,ADDRESS=Ltrim(AC1.Add1)+' '+Ltrim(AC1.Add2)+' '+Ltrim(AC1.Add3),STM.FORM_NM,AC1.S_TAX
INTO #FORM_RJ13
FROM PTACDET A 
INNER JOIN STMAIN M ON (A.ENTRY_TY=M.ENTRY_TY AND A.TRAN_CD=M.TRAN_CD)
INNER JOIN STAX_MAS STM ON (M.TAX_NAME=STM.TAX_NAME)
INNER JOIN AC_MAST AC ON (A.AC_NAME=AC.AC_NAME)
INNER JOIN AC_MAST AC1 ON (M.AC_ID=AC1.AC_ID)
WHERE 1=2

Declare @MultiCo	VarChar(3)
Declare @MCON as NVARCHAR(2000)
IF Exists(Select A.ID From SysObjects A Inner Join SysColumns B On(A.ID = B.ID) 
          Where A.[Name] = 'STMAIN' And B.[Name] = 'DBNAME')
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
		SET @SQLCOMMAND='Insert InTo #FORM_RJ_13 Select * from '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		---Drop Temp Table 
		SET @SQLCOMMAND='Drop Table '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
	End
else
	Begin ------Fetch Single Co. Data
		 Set @MultiCo = 'NO'
		 EXECUTE USP_REP_SINGLE_CO_DATA
		  @TMPAC, @TMPIT, @SPLCOND, @SDATE, @EDATE
		 ,@SAC, @EAC, @SIT, @EIT, @SAMT, @EAMT
		 ,@SDEPT, @EDEPT, @SCATE, @ECATE,@SWARE
		 ,@EWARE, @SINV_SR, @EINV_SR, @LYN, @EXPARA
		 ,@MFCON = @MCON OUTPUT

		--SET @SQLCOMMAND='Select * from '+@MCON
		---EXECUTE SP_EXECUTESQL @SQLCOMMAND
		SET @SQLCOMMAND='Insert InTo #FORM_RJ_13 Select * from '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
		---Drop Temp Table 
		SET @SQLCOMMAND='Drop Table '+@MCON
		EXECUTE SP_EXECUTESQL @SQLCOMMAND
        
	End


--->PART 1-5 
 
--------------------------------------------------------------------------------------------
 --1

 SELECT @AMTA1=0,@AMTA2=0,@AMTB1=0        

 SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #FORM_RJ_13 
 WHERE BHENT in ('ST')  AND (DATE BETWEEN @SDATE AND @EDATE)  and ac_name not like '%Rece%'

 Select @AMTA2=Round(SUM(NET_AMT),0) From #FORM_RJ_13 
 Where bhent = 'ST' AND (DATE BETWEEN @SDATE AND @EDATE) and ac_name not like '%Rece%' and 
 U_imporm = 'Purchase Return'

 SET @AMTA1=CASE WHEN @AMTA1 IS NULL THEN 0 ELSE @AMTA1 END
 SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
 SET @AMTB1=@AMTA1-@AMTA2

 INSERT INTO #FORM_RJ13(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
 VALUES(1,'1','A',0,@AMTB1,0,0,'')
--------------------------------------------------------------------------------------------
 --2

 SELECT @AMTA1=0

 SELECT @AMTA1=Round(SUM(NET_AMT),0) FROM #FORM_RJ_13 
 WHERE BHENT in ('SR','CN')  AND (DATE BETWEEN @SDATE AND @EDATE)

 INSERT INTO #FORM_RJ13(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
 VALUES(1,'1','B',0,@AMTA1,0,0,'')
--------------------------------------------------------------------------------------------
 --3

 SELECT @AMTB1=0,@AMTB2=0,@AMTA2=0

 SELECT @AMTB1=round(sum(TAXAMT),0) FROM #FORM_RJ_13
 WHERE  BHENT in('ST') And Tax_name like '%VAT%' AND (DATE BETWEEN @SDATE AND @EDATE) And 
 S_tax <> '' and ac_name not like '%Rece%' and U_imporm <> 'Purchase Return'

 SELECT @AMTB2=round(sum(TAXAMT),0) FROM #FORM_RJ_13  
 WHERE  BHENT in('SR') And  Tax_name like '%VAT%' AND (DATE BETWEEN @SDATE AND @EDATE) And S_tax <> '' 

 SET @AMTB1=CASE WHEN @AMTB1 IS NULL THEN 0 ELSE @AMTB1 END
 SET @AMTB2=CASE WHEN @AMTB2 IS NULL THEN 0 ELSE @AMTB2 END

 SET @AMTA2= @AMTB1 - @AMTB2
 SET @AMTA2=CASE WHEN @AMTA2 IS NULL THEN 0 ELSE @AMTA2 END
 
 INSERT INTO #FORM_RJ13(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
 VALUES(1,'1','C',0,@AMTA2,0,0,'')

--------------------------------------------------------------------------------------------
--4 

SELECT @AMTA1=0

SELECT @AMTA1=Round(SUM(TAXAMT),0) FROM #FORM_RJ_13 
WHERE BHENT in ('PT','EP','CN')  AND (DATE BETWEEN @SDATE AND @EDATE) 

INSERT INTO #FORM_RJ13(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
VALUES(1,'1','D',0,@AMTA1,0,0,'')
--------------------------------------------------------------------------------------------
--5

 SELECT @AMTA1=0 

 SELECT @AMTA1=Round(SUM(TAXAMT),0) FROM #FORM_RJ_13 
 WHERE BHENT in ('PR','DN')  AND (DATE BETWEEN @SDATE AND @EDATE) 

 INSERT INTO #FORM_RJ13(PART,PARTSR,SRNO,RATE,AMT1,AMT2,AMT3,Party_nm)
 VALUES (1,'1','E',0,@AMTA1,0,0,'')
--------------------------------------------------------------------------------------------
 
 Update #FORM_RJ13 
 set  PART = isnull(Part,0) , Partsr = isnull(PARTSR,''), SRNO = isnull(SRNO,''),
      RATE = isnull(RATE,0), AMT1 = isnull(AMT1,0), AMT2 = isnull(AMT2,0), 
      AMT3 = isnull(AMT3,0), INV_NO = isnull(INV_NO,''), DATE = isnull(Date,''), 
	  PARTY_NM = isnull(Party_nm,''), ADDRESS = isnull(Address,''),
	  FORM_NM = isnull(form_nm,''), S_TAX = isnull(S_tax,'')

 SELECT * FROM #FORM_RJ13
 order by cast(substring(partsr,1,case when (isnumeric(substring(partsr,1,2))=1) then 2 else 1 end) as int),
 partsr,SRNO
 
 END

set ANSI_NULLS OFF
--Print 'RJ VAT FORM 13'

