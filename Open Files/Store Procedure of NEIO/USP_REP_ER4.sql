if exists (select [name] from sysobjects where [name]='USP_REP_ER4' and xtype='P')
drop procedure USP_REP_ER4

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ruepesh Prajapati
-- Create date: 16/05/2007
-- Description:	This is useful for Excise Er-1 report.
-- Modified By: Ruepesh Prajapati
-- Modify date: 30/06/2008
-- Modified By: Satish Pal for bug-7248
-- Modify date: 26/03/2013
-- Remark:For Notification
-- Modified By: Satish Pal for bug-17636
-- Modify date: 05/93/2013
-- Modified By: BIRENDRA for bug-21792
-- Modify date: 12/03/2014
-- Modified By: Shrikant for bug-25365
-- Modify date: 25/02/2015

-- =============================================

CREATE PROCEDURE [dbo].[USP_REP_ER4] 
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
--,@SFLDNM VARCHAR(10)
AS
Declare @FCON as NVARCHAR(2000),@VSAMT DECIMAL(14,2),@VEAMT DECIMAL(14,2)
DECLARE  @ENTRY_TY CHAR(2),@DATE SMALLDATETIME,@DOC_NO VARCHAR(10),@AC_ID INT,@IT_CODE NUMERIC(10),@ITSERIAL VARCHAR(5),@RULE VARCHAR(20),@IT_NAME VARCHAR(100),@CHAPNO VARCHAR(10),@RATEUNIT VARCHAR(20),@EIT_NAME VARCHAR(100),@ITGRID INT,@TRAN_CD INT,@CHAR1 NUMERIC(2),@INV_NO VARCHAR(10),@DEPT VARCHAR(20),@CATE VARCHAR(20),@INV_SR VARCHAR(20),@AC_NAME VARCHAR(100),@CNT_CER1_3_2 INT,@VTYPE VARCHAR(40),@PMKEY VARCHAR(1)
DECLARE @CDATE SMALLDATETIME,@E_DUTY NUMERIC(12,3),@E_NAME VARCHAR(100),@E_PER NUMERIC(5,3),@SRNO CHAR(1),@PAC_ID INT,@PARTY_NM VARCHAR(100),@FDATE VARCHAR(10)
DECLARE @SRNO1 INT,@PER1 CHAR(20),@DUTY1 CHAR(20),@DESC1 CHAR(50),@EPER CHAR(20),@VVEAMT CHAR(20),@SHORTNM VARCHAR(40),@U_ARREARS VARCHAR(40),@CHEQ_NO VARCHAR(40),@CNT INT,@OBACID INT,@CRAC BIT ,@CURAC BIT
DECLARE @VCOND NVARCHAR(1000),@SQLCOMMAND NVARCHAR(4000),@u_asseamt decimal(20,4)

--SELECT @FDATE=CASE WHEN DBDATE=1 THEN 'DATE' ELSE 'U_CLDT' END FROM MANUFACT
EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=@SDATE --null
,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE='STKL_VW_MAIN',@VITFILE='',@VACFILE='EX_VW_ACDET '
,@VDTFLD =@FDATE
,@VLYN =NULL
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT

-->for part 5
SET @VCOND=@SPLCOND

select Rg23Adr=amount,Rg23Acr=amount,Rg23Cdr=amount,Rg23Ccr=amount,Serdr=amount,Sercr=amount into #er4_1 from ex_vw_acdet ac
where 1=2
------<--for part 5

--select  srno1=cast(0 as int),srno2=cast(0 as int),srno3='A',srno4='A',Rate,amt1=gro_amt,amt2=gro_amt,amt3=gro_amt,OPBAL=0,BALANCE=0, itemname=@IT_NAME,chapno=0,rateunit=@IT_NAME,u_asseamt_OS=u_asseamt,u_asseamt_Prdn=u_asseamt ,u_asseamt_csmn=u_asseamt   into #er4 from stitem where 1=2
--Birendra : Bug-21792 on 12/03/2014
select  srno1=cast(0 as int),srno2=cast(0 as int),srno3='A',srno4='A',Rate,amt1=@u_asseamt,amt2=@u_asseamt,amt3=gro_amt,OPBAL=0,BALANCE=0, itemname=@IT_NAME,chapno=@CHAPNO,rateunit=@IT_NAME,u_asseamt_OS=@u_asseamt,u_asseamt_Prdn=@u_asseamt ,u_asseamt_csmn=@u_asseamt   into #er4 from stitem where 1=2
---===================================================================================================
---===================================================================================================
--->Part-3--i-Start
--->Part-3--i-a--start
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT 3,1,'a','',0,(SUM(STKL_VW_ITEM.u_asseamt)/100000) as total ,0,0,0,0,'','',''
--FROM STKL_VW_ITEM INNER JOIN STKL_VW_MAIN ON (STKL_VW_MAIN.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STKL_VW_MAIN.TRAN_CD=STKL_VW_ITEM.TRAN_CD)
--INNER JOIN AC_MAST ON (STKL_VW_MAIN.AC_ID=AC_MAST.AC_ID)
--LEFT JOIN STITEM ON (STKL_VW_ITEM.ENTRY_TY=STITEM.ENTRY_TY AND STKL_VW_ITEM.TRAN_CD=STITEM.TRAN_CD AND STKL_VW_ITEM.ITSERIAL=STITEM.ITSERIAL)
--INNER JOIN IT_MAST ON (STKL_VW_ITEM.IT_CODE=IT_MAST.IT_CODE)and STKL_VW_MAIN.[RULE] not in ('ANNEXURE V','NON-MODVATABLE') and STKL_VW_MAIN.entry_ty IN('PT','P1')

--Birendra : Bug-21792 on 11/03/2014
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT 3,1,'a','',0,(SUM(STKL_VW_ITEM.u_asseamt)/100000) as total ,0,0,0,0,'','',''
FROM STKL_VW_ITEM INNER JOIN STKL_VW_MAIN ON (STKL_VW_MAIN.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STKL_VW_MAIN.TRAN_CD=STKL_VW_ITEM.TRAN_CD)
where STKL_VW_MAIN.[RULE] not in ('ANNEXURE V','NON-MODVATABLE') and STKL_VW_MAIN.entry_ty IN('PT','P1')
and STKL_VW_ITEM.date between @SDATE AND @EDATE 

--->Part-3--i-a--End
--->Part-3--i-b--start
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit)  SELECT 3,1,'b','',0,(SUM(STKL_VW_ITEM.u_asseamt)/100000 )as total ,0,0,0,0,'','',''
--FROM STKL_VW_ITEM INNER JOIN STKL_VW_MAIN ON (STKL_VW_MAIN.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STKL_VW_MAIN.TRAN_CD=STKL_VW_ITEM.TRAN_CD)
--INNER JOIN AC_MAST ON (STKL_VW_MAIN.AC_ID=AC_MAST.AC_ID)
--LEFT JOIN STITEM ON (STKL_VW_ITEM.ENTRY_TY=STITEM.ENTRY_TY AND STKL_VW_ITEM.TRAN_CD=STITEM.TRAN_CD AND STKL_VW_ITEM.ITSERIAL=STITEM.ITSERIAL)
--INNER JOIN IT_MAST ON (STKL_VW_ITEM.IT_CODE=IT_MAST.IT_CODE)and STKL_VW_MAIN.[RULE] ='NON-MODVATABLE' and STKL_VW_MAIN.entry_ty IN('PT','P1')

--Birendra : Bug-21792 on 11/03/2014
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit)  SELECT 3,1,'b','',0,(SUM(STKL_VW_ITEM.u_asseamt)/100000 )as total ,0,0,0,0,'','',''
FROM STKL_VW_ITEM INNER JOIN STKL_VW_MAIN ON (STKL_VW_MAIN.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STKL_VW_MAIN.TRAN_CD=STKL_VW_ITEM.TRAN_CD)
where STKL_VW_MAIN.[RULE] ='NON-MODVATABLE' and STKL_VW_MAIN.entry_ty IN('PT','P1')
and STKL_VW_ITEM.date between @SDATE AND @EDATE 
--->Part-3--i-b--End
--->Part-3--i-c--Start
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT 3,1,'c','',0,0 ,0,0,0,0,'','',''
--Birendra : Bug-21792 on 11/03/2014
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT 3,1,'c','',0,(select SUM(amt1)from #er4) ,0,0,0,0,'','',''
--->Part-3--i-c--End
--->Part-3--i--End
---===================================================================================================
---===================================================================================================
--->Part-3--ii--Start
--->Part-3--ii--a-Start

SELECT  STKL_VW_ITEM.ENTRY_TY,BEH='  ',STKL_VW_ITEM.DATE,STKL_VW_ITEM.DOC_NO,STKL_VW_ITEM.AC_ID,STKL_VW_ITEM.IT_CODE,STKL_VW_ITEM.QTY,STKL_VW_ITEM.U_LQTY,STKL_VW_ITEM.ITSERIAL,STKL_VW_ITEM.PMKEY,IT_MAST.ITGRID,IT_MAST.[GROUP],IT_MAST.IT_NAME,IT_MAST.CHAPNO,IT_MAST.TYPE,IT_MAST.RATEUNIT
,STKL_VW_ITEM.rate
,STKL_VW_ITEM.u_asseamt ----Birendra : Bug-21792 on 12/03/2014
INTO #TITEM FROM STKL_VW_ITEM 
INNER JOIN STKL_VW_MAIN  ON (STKL_VW_ITEM.TRAN_CD=STKL_VW_MAIN.TRAN_CD )
INNER JOIN IT_MAST  ON (IT_MAST.IT_CODE=STKL_VW_ITEM.IT_CODE)
INNER JOIN AC_MAST  ON (AC_MAST.AC_ID=STKL_VW_MAIN.AC_ID)
INNER JOIN LCODE  ON (STKL_VW_ITEM.ENTRY_TY=LCODE.ENTRY_TY)
WHERE 1=2
declare @RuleCondition varchar(1000)
set @RuleCondition=''
--SET @SQLCOMMAND='INSERT INTO  #TITEM SELECT IVW.ENTRY_TY,BEH=(CASE WHEN LCODE.EXT_VOU=1 THEN LCODE.BCODE_NM ELSE LCODE.ENTRY_TY END),IVW.DATE,IVW.DOC_NO,IVW.AC_ID,IVW.IT_CODE,IVW.QTY,IVW.U_LQTY,IVW.ITSERIAL,IVW.PMKEY,IT_MAST.ITGRID,IT_MAST.[GROUP],IT_MAST.IT_NAME,IT_MAST.CHAPNO,IT_MAST.TYPE,IT_MAST.RATEUNIT,IVW.rate '
--Birendra : Bug-21792 on 12/03/2014
SET @SQLCOMMAND='INSERT INTO  #TITEM SELECT IVW.ENTRY_TY,BEH=(CASE WHEN LCODE.EXT_VOU=1 THEN LCODE.BCODE_NM ELSE LCODE.ENTRY_TY END),IVW.DATE,IVW.DOC_NO,IVW.AC_ID,IVW.IT_CODE,IVW.QTY,IVW.U_LQTY,IVW.ITSERIAL,IVW.PMKEY,IT_MAST.ITGRID,IT_MAST.[GROUP],
				case when len(ltrim(rtrim(IT_MAST.eit_name)))>0 then IT_MAST.eit_name else IT_MAST.IT_NAME end
				,IT_MAST.CHAPNO,IT_MAST.TYPE,IT_MAST.RATEUNIT,IVW.rate,IVW.u_asseamt '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+'  FROM STKL_VW_ITEM IVW'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN STKL_VW_MAIN MVW ON (IVW.TRAN_CD=MVW.TRAN_CD AND IVW.ENTRY_TY=MVW.ENTRY_TY)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN IT_MAST  ON (IT_MAST.IT_CODE=IVW.IT_CODE)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN AC_MAST  ON (AC_MAST.AC_ID=MVW.AC_ID)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' INNER JOIN LCODE  ON (IVW.ENTRY_TY=LCODE.ENTRY_TY)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND MVW.APGEN='+CHAR(39)+'YES'+CHAR(39)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND IVW.DC_NO='+CHAR(39)+' '+CHAR(39)
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' AND IVW.Date<='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39) --Birendra : Bug-21792 on 13/03/2014
EXECUTE SP_EXECUTESQL @SQLCOMMAND
print @SQLCOMMAND


Select DESCRIPTION = IT_NAME,RATEUNIT,CHAPNO,[TYPE],OPBAL = QTY,RQTY_PT = QTY,RRate_PT = Rate,RQTY_OP = QTY,RQTY_IR = QTY,RQTY_SR = QTY,RQTY_AR = QTY,RQTY_OT = QTY,IQTY_ST = QTY,IRate_ST=rate,IQTY_DC = QTY,IQTY_IP = QTY,IQTY_II = QTY,IQTY_PR = QTY,IQTY_OT = QTY,CLBAL = QTY
,u_asseamt_OS=u_asseamt --Birendra : Bug-21792 on 12/03/2014
,u_asseamt_Prdn=u_asseamt --Birendra : Bug-21792 on 12/03/2014
,u_asseamt_csmn=u_asseamt --Birendra : Bug-21792 on 12/03/2014
 into #TITEM2 from #TITEM where 1=2 
SET @SQLCOMMAND='Insert into #TITEM2 SELECT DESCRIPTION=A.it_NAME,A.RATEUNIT,A.CHAPNO,A.TYPE'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',OPBAL =SUM(CASE WHEN (A.BEH ='+'''OS'''+' OR A.DATE<'+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') THEN (CASE WHEN A.PMKEY='+'''+'''+' THEN A.QTY ELSE -A.QTY END) ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',RQTY_PT = SUM(CASE WHEN A.BEH ='+'''PT'''+' AND A.PMKEY='+'''+'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' THEN A.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',RRate_PT = SUM(CASE WHEN A.BEH ='+'''PT'''+' AND A.PMKEY='+'''+'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' THEN A.Rate ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',RQTY_OP = SUM(CASE WHEN A.BEH ='+'''OP'''+' AND A.PMKEY='+'''+'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' THEN A.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',RQTY_IR = SUM(CASE WHEN A.BEH ='+'''IR'''+' AND A.PMKEY='+'''+'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' THEN A.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',RQTY_SR = SUM(CASE WHEN A.BEH ='+'''SR'''+' AND A.PMKEY='+'''+'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' THEN A.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',RQTY_AR=SUM(CASE WHEN A.BEH ='+'''AR'''+' AND A.PMKEY='+'''+'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' THEN A.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',RQTY_OT=SUM(CASE WHEN A.BEH NOT IN ('+'''OS'''+','+'''PT'''+','+'''OP'''+','+'''IR'''+','+'''SR'''+','+'''AR'''+') AND A.PMKEY='+'''+'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' THEN A.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',IQTY_ST  =SUM(CASE WHEN A.BEH ='+'''ST'''+' AND A.PMKEY='+'''-'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39) +'THEN A.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',IRate_ST  =SUM(CASE WHEN A.BEH ='+'''ST'''+' AND A.PMKEY='+'''-'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39) +'THEN A.Rate ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',IQTY_DC  =SUM(CASE WHEN A.BEH ='+'''DC'''+' AND A.PMKEY='+'''-'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' THEN A.QTY ELSE 0 END)' --Added By Hetal TKT-616 Dt 10032010
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',IQTY_IP  =SUM(CASE WHEN A.BEH ='+'''IP'''+' AND A.PMKEY='+'''-'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' THEN A.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',IQTY_II  =SUM(CASE WHEN A.BEH ='+'''II'''+' AND A.PMKEY='+'''-'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' THEN A.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',IQTY_PR  =SUM(CASE WHEN A.BEH ='+'''PR'''+' AND A.PMKEY='+'''-'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' THEN A.QTY ELSE 0 END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',IQTY_OT  =SUM(CASE WHEN A.BEH NOT IN ('+'''ST'''+','+'''IP'''+','+'''II'''+','+'''PR'''+','+'''DC'''+') AND A.PMKEY='+'''-'''+' AND A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+' THEN A.QTY ELSE 0 END)' --Added By Hetal TKT-616 Dt 10032010
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',CLBAL    =SUM(CASE WHEN  A.PMKEY='+'''+'''+' THEN A.QTY ELSE -A.QTY END)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',u_asseamt_OS=SUM(CASE WHEN (A.BEH ='+'''OS'''+' OR A.DATE<'+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') THEN (Case when A.PMKEY='+'''+'''+' then a.u_asseamt ELSE -a.u_asseamt end) else 0 END)' --Birendra : Bug-21792 on 12/03/2014
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',u_asseamt_Prdn=SUM(CASE WHEN (A.PMKEY='+'''+'''+' and A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') THEN a.u_asseamt ELSE 0 END)' --Birendra : Bug-21792 on 12/03/2014
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+',u_asseamt_Csmn=SUM(CASE WHEN (A.PMKEY='+'''-'''+' and A.DATE>='+CHAR(39)+CAST(@SDATE AS VARCHAR)+CHAR(39)+') THEN a.u_asseamt ELSE 0 END)' --Birendra : Bug-21792 on 12/03/2014
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' FROM #TITEM A'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' GROUP BY A.it_NAME,A.RATEUNIT,A.TYPE,A.CHAPNO'
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' ORDER BY A.it_NAME,A.RATEUNIT,A.TYPEA.CHAPNO'		--Commented by Shrikant S. on 25/02/2015 for Bug-25365
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+' ORDER BY A.it_NAME,A.RATEUNIT,A.TYPE,A.CHAPNO'			--Added by Shrikant S. on 25/02/2015 for Bug-25365
EXECUTE SP_EXECUTESQL @SQLCOMMAND

IF  CHARINDEX('ANS=NO', @EXPARA)<>0 
BEGIN
	print @EXPARA
	delete from #TITEM2 where clbal=0
END

--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT 3,2,'a','a',RRate_PT/100000,IQTY_IP,OPBAL+rqty_pt-clbal,rqty_pt,OPBAL,clbal,[DESCRIPTION],chapno,rateunit from #TITEM2 where [TYPE]='Raw Material'
--Birendra : Bug-21792 on 12/03/2014
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit,u_asseamt_OS,u_asseamt_Prdn,u_asseamt_csmn ) 
SELECT 3,2,'a','a'
,RRate_PT/100000
,IQTY_IP,OPBAL+rqty_pt-clbal,rqty_pt,OPBAL,clbal,[DESCRIPTION],chapno,rateunit
,isnull(u_asseamt_OS,0)/100000
,isnull(u_asseamt_Prdn,0)/100000
,isnull(u_asseamt_csmn,0)/100000
 from #TITEM2 where [TYPE]='Raw Material'
---update #er4 set amt2=CAST(amt2 AS numeric)-CAST(BALANCE AS numeric) where srno1=3 and srno2=2 and srno3='a' and srno4='a'
IF not EXISTS(select * from #er4 where srno1=3 and srno2=2 and srno3='a' and srno4='a') 
begin
	insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit,u_asseamt_OS,u_asseamt_Prdn,u_asseamt_csmn) values (3,2,'a','a',0,0,0,0,0,0,'','','',0,0,0)
end
--->Part-3--ii--a-End
--->Part-3--ii--b-Start



--->Part-3--ii--b-End
---===================
--->Part-3--ii--End
---===================================================================================================
---===================================================================================================
--->Part-3--iii--Start
--->Part-3--iii-a--Start
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit)  SELECT 3,3,'a','',0,SUM(qty)as total ,0,0,0,0,'','',''
--FROM EPITEM WHERE EPITEM.entry_ty='IF' group by EPITEM.entry_ty
--Birendra : Bug-21792 on 11/03/2014 :Start:
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit)  SELECT 3,3,'a','',0,isnull(SUM(case when a.amt_ty='DR' then a.amount else case when a.amt_ty='CR' then -a.amount else 0 end end ),0)/100000 as total ,0,0,0,0,'','',''
from lac_vw a
inner join AC_MAST b on a.AC_ID=b.Ac_id
where b.typ='Inward Freight' and a.date between @SDATE and @EDATE

--Birendra : Bug-21792 on 11/03/2014 :End:

IF not EXISTS(select * from #er4 where srno1=3 and srno2=3 and srno3='a' and srno4='') 
begin
	insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'a','',0,0,0,0,0,0,'','','')
end
--->Part-3--iii-a--End
--->Part-3--i-b--Start
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit)  SELECT 3,3,'b','',0,SUM(qty)as total ,0,0,0,0,'','',''
--Birendra : Bug-21792 on 11/03/2014 :Start:
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit)  SELECT 3,3,'b','',0,isnull(SUM(case when a.amt_ty='DR' then a.amount else case when a.amt_ty='CR' then -a.amount else 0 end end ),0)/100000 as total ,0,0,0,0,'','',''
from lac_vw a
inner join AC_MAST b on a.AC_ID=b.Ac_id
where b.typ='Outward Freight' and a.date between @SDATE and @EDATE
--Birendra : Bug-21792 on 11/03/2014 :End:
IF not EXISTS(select * from #er4 where srno1=3 and srno2=3 and srno3='b' and srno4='') 
begin
	insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'b','',0,0,0,0,0,0,'','','')
end
--->Part-3--iii-b--End
--->Part-3--iii--End
---===================================================================================================
---===================================================================================================
--->Part-3--iii--Start
---===================================================================================================
---===================================================================================================
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,2,'a','e',0,0,0,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,2,'b','',0,0,0,0,0,0,'','','')
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'c','',0,0,0,0,0,0,'','','')
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'d','',0,0,0,0,0,0,'','','')
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'e','',0,0,0,0,0,0,'','','')
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'f','',0,0,0,0,0,0,'','','')
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'g','',0,0,0,0,0,0,'','','')
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'h','',0,0,0,0,0,0,'','','')
--Birendra : Bug-21792 on 11/03/2014 :Start:
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT 3,3,'c','',0,isnull(SUM(case when a.amt_ty='DR' then a.amount else case when a.amt_ty='CR' then -a.amount else 0 end end ),0)/100000 as total ,0,0,0,0,'','',''
from lac_vw a
inner join AC_MAST b on a.AC_ID=b.Ac_id
where b.typ='Sales Promotion' and a.date between @SDATE and @EDATE

IF not EXISTS(select * from #er4 where srno1=3 and srno2=3 and srno3='c' and srno4='') 
begin
	insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'c','',0,0,0,0,0,0,'','','')
end
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT 3,3,'d','',0,isnull(SUM(case when a.amt_ty='DR' then a.amount else case when a.amt_ty='CR' then -a.amount else 0 end end ),0)/100000 as total ,0,0,0,0,'','',''
from lac_vw a
inner join AC_MAST b on a.AC_ID=b.Ac_id
where b.typ='Commission for Sales' and a.date between @SDATE and @EDATE
IF not EXISTS(select * from #er4 where srno1=3 and srno2=3 and srno3='d' and srno4='') 
begin
	insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'d','',0,0,0,0,0,0,'','','')
end
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT 3,3,'e','',0,isnull(SUM(case when a.amt_ty='DR' then a.amount else case when a.amt_ty='CR' then -a.amount else 0 end end ),0)/100000 as total ,0,0,0,0,'','',''
from lac_vw a
inner join AC_MAST b on a.AC_ID=b.Ac_id
where b.typ='R & D Expenditure' and a.date between @SDATE and @EDATE
IF not EXISTS(select * from #er4 where srno1=3 and srno2=3 and srno3='e' and srno4='') 
begin
	insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'e','',0,0,0,0,0,0,'','','')
end
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT 3,3,'f','',0,isnull(SUM(case when a.amt_ty='DR' then a.amount else case when a.amt_ty='CR' then -a.amount else 0 end end ),0)/100000 as total ,0,0,0,0,'','',''
from lac_vw a
inner join AC_MAST b on a.AC_ID=b.Ac_id
where b.typ='Wages' and a.date between @SDATE and @EDATE
IF not EXISTS(select * from #er4 where srno1=3 and srno2=3 and srno3='f' and srno4='') 
begin
	insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'f','',0,0,0,0,0,0,'','','')
end
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT 3,3,'g','',0,isnull(SUM(case when a.amt_ty='DR' then a.amount else case when a.amt_ty='CR' then -a.amount else 0 end end ),0)/100000 as total ,0,0,0,0,'','',''
from lac_vw a
inner join AC_MAST b on a.AC_ID=b.Ac_id
where b.typ='Power and Fuel' and a.date between @SDATE and @EDATE
IF not EXISTS(select * from #er4 where srno1=3 and srno2=3 and srno3='g' and srno4='') 
begin
	insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'g','',0,0,0,0,0,0,'','','')
end
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT 3,3,'h','',0,isnull(SUM(case when a.amt_ty='DR' then a.amount else case when a.amt_ty='CR' then -a.amount else 0 end end ),0)/100000 as total ,0,0,0,0,'','',''
from lac_vw a
inner join AC_MAST b on a.AC_ID=b.Ac_id
where b.typ='Other Expenses' and a.date between @SDATE and @EDATE
IF not EXISTS(select * from #er4 where srno1=3 and srno2=3 and srno3='h' and srno4='') 
begin
	insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,3,'h','',0,0,0,0,0,0,'','','')
end
--Birendra : Bug-21792 on 11/03/2014 :End:

insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,4,'a','',0,0,0,0,0,0,'No','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,4,'b','',0,0,0,0,0,0,'No','','')

IF EXISTS(select entry_ty from IRITEM where entry_ty='LR')
begin
	Update #er4 set itemname='Yes' where srno1=3 and srno2=4 and srno3='a' and srno4=''
	Update #er4 set itemname='Yes' where srno1=3 and srno2=4 and srno3='b' and srno4=''
end
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname) values (3,4,'a','',0,0,0,0,0,0,'')
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname) values (3,4,'b','',0,0,0,0,0,0,'')


insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,4,'c','',0,0,0,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (3,4,'d','',0,0,0,0,0,0,'','','')
------------<--Part-3
------------->Part-4
--->Part-4--i--Start
----insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname) values (4,1,'','',0,0,0,0,0,0,'')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT 4,1,'','',0,(SUM(STKL_VW_ITEM.gro_amt)/100000) as total ,0,0,0,0,'','',''
FROM STKL_VW_ITEM INNER JOIN STKL_VW_MAIN ON (STKL_VW_MAIN.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STKL_VW_MAIN.TRAN_CD=STKL_VW_ITEM.TRAN_CD)
INNER JOIN AC_MAST ON (STKL_VW_MAIN.AC_ID=AC_MAST.AC_ID) LEFT JOIN STITEM ON (STKL_VW_ITEM.ENTRY_TY=STITEM.ENTRY_TY AND STKL_VW_ITEM.TRAN_CD=STITEM.TRAN_CD AND STKL_VW_ITEM.ITSERIAL=STITEM.ITSERIAL)
INNER JOIN IT_MAST ON (STKL_VW_ITEM.IT_CODE=IT_MAST.IT_CODE)and STKL_VW_MAIN.entry_ty='ST'
and STKL_VW_ITEM.date between @SDATE AND @EDATE  --Birendra : Bug-21792 on 13/03/2014
--->Part-4--i--End
--->Part-4--ii--Start

--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT  4,2,'a','a',IRate_ST/100000,0,IQTY_ST,RQTY_OP,OPBAL,CLBAL,[DESCRIPTION],chapno,rateunit  FROM #TITEM2 where [TYPE] IN ('Finished','Semi Finish') 

--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) SELECT  4,3,'a','a',RRate_PT/100000,0,IQTY_ST,RQTY_PT,OPBAL,CLBAL,[DESCRIPTION],chapno,rateunit  FROM #TITEM2 where [TYPE]='Trading'
--Birendra : Bug-21792 on 11/03/2014 :Start:
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit,u_asseamt_OS,u_asseamt_Prdn,u_asseamt_csmn) SELECT  4,2,'a','a',IRate_ST/100000,0,IQTY_ST,RQTY_OP+RQTY_PT,OPBAL,CLBAL,[DESCRIPTION],chapno,rateunit
,isnull(u_asseamt_OS,0)/100000 
,isnull(u_asseamt_Prdn,0)/100000 
,isnull(u_asseamt_csmn,0)/100000 
  FROM #TITEM2 where [TYPE] IN ('Finished','Semi Finish') 

insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit,u_asseamt_OS,u_asseamt_Prdn,u_asseamt_csmn) SELECT  4,3,'a','a',RRate_PT/100000,0,IQTY_ST,RQTY_PT,OPBAL,CLBAL,[DESCRIPTION],chapno,rateunit
,isnull(u_asseamt_OS,0)/100000 
,isnull(u_asseamt_Prdn,0)/100000 
,isnull(u_asseamt_csmn,0)/100000 
  FROM #TITEM2 where [TYPE]='Trading'
--Birendra : Bug-21792 on 11/03/2014 :End:

IF not EXISTS(select * from #er4 where srno1=4 and srno2=2 and srno3='a' and srno4='a') 
begin
	insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit,u_asseamt_OS,u_asseamt_Prdn,u_asseamt_csmn) values (4,2,'a','a',0,0,0,0,0,0,'','','',0,0,0)
end
IF not EXISTS(select * from #er4 where srno1=4 and srno2=3 and srno3='a' and srno4='a') 
begin
	insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit,u_asseamt_OS,u_asseamt_Prdn,u_asseamt_csmn) values (4,3,'a','a',0,0,0,0,0,0,'','','',0,0,0)
end

insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit)  SELECT 4,4,'','',0,isnull(SUM(STKL_VW_ITEM.u_asseamt),0)/100000 as total ,0,0,0,0,'','',''
FROM STKL_VW_ITEM INNER JOIN STKL_VW_MAIN ON (STKL_VW_MAIN.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STKL_VW_MAIN.TRAN_CD=STKL_VW_ITEM.TRAN_CD)
INNER JOIN AC_MAST ON (STKL_VW_MAIN.AC_ID=AC_MAST.AC_ID)
LEFT JOIN STITEM ON (STKL_VW_ITEM.ENTRY_TY=STITEM.ENTRY_TY AND STKL_VW_ITEM.TRAN_CD=STITEM.TRAN_CD AND STKL_VW_ITEM.ITSERIAL=STITEM.ITSERIAL)
INNER JOIN IT_MAST ON (STKL_VW_ITEM.IT_CODE=IT_MAST.IT_CODE)and STKL_VW_MAIN.[RULE] ='NON-MODVATABLE' and STKL_VW_MAIN.entry_ty='ST'
and STKL_VW_ITEM.date between @sdate and @EDATE --Birendra : Bug-21792 on 13/03/2014



insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit)  SELECT 4,5,'','',0,isnull(SUM(STKL_VW_ITEM.u_asseamt),0)/100000 as total ,0,0,0,0,'','',''
FROM STKL_VW_ITEM INNER JOIN STKL_VW_MAIN ON (STKL_VW_MAIN.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STKL_VW_MAIN.TRAN_CD=STKL_VW_ITEM.TRAN_CD)
INNER JOIN AC_MAST ON (STKL_VW_MAIN.AC_ID=AC_MAST.AC_ID)
LEFT JOIN STITEM ON (STKL_VW_ITEM.ENTRY_TY=STITEM.ENTRY_TY AND STKL_VW_ITEM.TRAN_CD=STITEM.TRAN_CD AND STKL_VW_ITEM.ITSERIAL=STITEM.ITSERIAL)
INNER JOIN IT_MAST ON (STKL_VW_ITEM.IT_CODE=IT_MAST.IT_CODE)and STKL_VW_MAIN.[RULE] in ('CT-1','CT-3','EOU EXPORT','IMPORTED','UT-1','UT-3') and STKL_VW_MAIN.entry_ty='ST'
and STKL_VW_ITEM.date between @sdate and @EDATE --Birendra : Bug-21792 on 13/03/2014


insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit)  SELECT 4,6,'','',0,isnull(SUM(STKL_VW_ITEM.u_asseamt),0)/100000 as total ,0,0,0,0,'','',''
FROM STKL_VW_ITEM INNER JOIN STKL_VW_MAIN ON (STKL_VW_MAIN.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STKL_VW_MAIN.TRAN_CD=STKL_VW_ITEM.TRAN_CD)
INNER JOIN AC_MAST ON (STKL_VW_MAIN.AC_ID=AC_MAST.AC_ID)
LEFT JOIN STITEM ON (STKL_VW_ITEM.ENTRY_TY=STITEM.ENTRY_TY AND STKL_VW_ITEM.TRAN_CD=STITEM.TRAN_CD AND STKL_VW_ITEM.ITSERIAL=STITEM.ITSERIAL)
INNER JOIN IT_MAST ON (STKL_VW_ITEM.IT_CODE=IT_MAST.IT_CODE)and STKL_VW_MAIN.[RULE] ='REBATE' and STKL_VW_MAIN.entry_ty='ST'
and STKL_VW_ITEM.date between @sdate and @EDATE --Birendra : Bug-21792 on 13/03/2014

insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit)  SELECT 4,7,'','',0,isnull(SUM(STKL_VW_ITEM.u_asseamt),0)/100000 as total ,0,0,0,0,'','',''
FROM STKL_VW_ITEM INNER JOIN STKL_VW_MAIN ON (STKL_VW_MAIN.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STKL_VW_MAIN.TRAN_CD=STKL_VW_ITEM.TRAN_CD)
INNER JOIN AC_MAST ON (STKL_VW_MAIN.AC_ID=AC_MAST.AC_ID)
LEFT JOIN STITEM ON (STKL_VW_ITEM.ENTRY_TY=STITEM.ENTRY_TY AND STKL_VW_ITEM.TRAN_CD=STITEM.TRAN_CD AND STKL_VW_ITEM.ITSERIAL=STITEM.ITSERIAL)
INNER JOIN IT_MAST ON (STKL_VW_ITEM.IT_CODE=IT_MAST.IT_CODE and (IT_MAST.it_name like '%waste%' or IT_MAST.it_name like '%Scrap%' ))and STKL_VW_MAIN.entry_ty='ST'
and STKL_VW_ITEM.date between @sdate and @EDATE --Birendra : Bug-21792 on 13/03/2014
--Birendra : Bug-21792 on 13/03/2014:Start:
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,7,'','',0,0,0,0,0,0,'','','')
IF not EXISTS(select * from #er4 where srno1=4 and srno2=7 and srno3='' and srno4='') 
begin
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,7,'','',0,0,0,0,0,0,'','','')
end
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit)  SELECT 4,8,'a','',0,isnull(SUM(STKL_VW_ITEM.u_asseamt),0)/100000 as total ,0,0,0,0,'','',''
FROM STKL_VW_ITEM INNER JOIN STKL_VW_MAIN ON (STKL_VW_MAIN.ENTRY_TY=STKL_VW_ITEM.ENTRY_TY AND STKL_VW_MAIN.TRAN_CD=STKL_VW_ITEM.TRAN_CD)
and STKL_VW_MAIN.entry_ty='ST' and STKL_VW_MAIN.u_imporm in ('Raw Material Sale','Branch Transfer','Purchase Return')
and STKL_VW_ITEM.date between @sdate and @EDATE --Birendra : Bug-21792 on 13/03/2014

--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,8,'a','',0,0,0,0,0,0,'','','')
IF not EXISTS(select * from #er4 where srno1=4 and srno2=8 and srno3='a' and srno4='') 
begin
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,8,'a','',0,0,0,0,0,0,'','','')
end

insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,8,'b','',0,0,0,0,0,0,'','','')
--Birendra : Bug-21792 on 13/03/2014:End:

insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,10,'a','',0,0,0,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,10,'b','',0,0,0,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,10,'c','',0,0,0,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,10,'d','',0,0,0,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,10,'e','',0,0,0,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,10,'f','',0,0,0,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,10,'g','',0,0,0,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,10,'h','',0,0,0,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,10,'i','',0,0,0,0,0,0,'','','')

insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,11,'','',0,0,0,0,0,0,'','','')

insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,12,'a','',0,0,0,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,12,'b','',0,0,0,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (4,12,'c','',0,0,0,0,0,0,'','','')

--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname) values (4,12,'d','',0,0,0,0,0,0,'')
--select taxamt=sum(taxamt) into #stmain from stmain
--Birendra : Bug-21792 on 11/04/2014
select taxamt=sum(taxamt/100000) into #stmain from stmain where date between @SDATE and @EDATE 
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) select 4,9,'','',0,taxamt,0,0,0,0,'','','' from #stmain

------------<--Part-4
------------<--Part-5 & 6
SET @SQLCOMMAND=' insert into #er4_1 select '
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' Rg23Adr=sum(case when ac.amt_ty='+'''dr'''+' and ac_mast.ac_name like '+'''%rg23a%'''+' then amount else 0 end)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' ,Rg23Acr=sum(case when ac.amt_ty='+'''cr'''+' and ac_mast.ac_name like '+'''%rg23a%'''+' then amount else 0 end)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' ,Rg23Cdr=sum(case when ac.amt_ty='+'''dr'''+' and ac_mast.ac_name like '+'''%rg23c%'''+' then amount else 0 end)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' ,Rg23Ccr=sum(case when ac.amt_ty='+'''cr'''+' and ac_mast.ac_name like '+'''%rg23c%'''+' then amount else 0 end)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' ,Serdr=sum(case when ac.amt_ty='+'''dr'''+' and ac_mast.ac_name like '+'''%service%'''+' then amount else 0 end)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' ,Sercr=sum(case when ac.amt_ty='+'''cr'''+' and ac_mast.ac_name like '+'''%service%'''+' then amount else 0 end)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' from ex_vw_acdet ac'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' inner join stkl_vw_main on (stkl_vw_main.entry_ty=AC.entry_ty and stkl_vw_main.tran_cd=ac.tran_cd)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' inner join ac_mast on (ac_mast.ac_id=ac.ac_id)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' inner join er_excise ex on (ac_mast.ac_name=ex.ac_name)'
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' where YEAR(ac.U_CLDT) <> ''1900'''+' and ex.AC_NAME<>'' '''
--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' and ac.U_CLDT <='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39) --Birendra : Bug-21792 on 13/03/2014
SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+ ' and ac.date <='+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39) --Birendra : Bug-21792 on 13/03/2014

--SET @SQLCOMMAND=RTRIM(@SQLCOMMAND)+' '+RTRIM(@FCON)
PRINT @SQLCOMMAND
EXEC SP_EXECUTESQL  @SQLCOMMAND
--<--for part 5
------------select * from #er4_1
declare @Rg23Adr numeric(12,2),@Rg23Acr numeric(12,2),@Rg23Cdr numeric(12,2),@Rg23Ccr numeric(12,2),@serdr numeric(12,2),@sercr numeric(12,2)
select @Rg23Adr=Rg23Adr,@Rg23Acr=Rg23Acr,@Rg23Cdr=Rg23Cdr,@Rg23Ccr=Rg23Ccr,@serdr=serdr,@sercr=sercr from #er4_1 

--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (5,2,'a','',0,@Rg23Adr,@Rg23Acr,0,0,0,'','','')
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (5,2,'b','',0,@Rg23Cdr,@Rg23Ccr,0,0,0,'','','')
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (5,2,'c','',0,@serdr,@sercr,0,0,0,'','','')
--Birendra : Bug-21792 on 11/04/2014
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (5,2,'a','',0,@Rg23Adr/100000,@Rg23Acr/100000,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (5,2,'b','',0,@Rg23Cdr/100000,@Rg23Ccr/100000,0,0,0,'','','')
insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname,chapno,rateunit) values (5,2,'c','',0,@serdr/100000,@sercr/100000,0,0,0,'','','')

--delete from #er4 where amt1+amt2+OPBAL+BALANCE =0 and ((srno1=3 or srno1=4)and srno2=2 and srno3='a' and srno4='a')--Biru
--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname) values (6,0,'','',0,0,0,0,0,0,'')
--<--Part-5 & 6

--insert into #er4 (srno1,srno2,srno3,srno4,Rate,amt1,amt2,amt3,OPBAL,BALANCE,itemname) values (3,3,'c','',0,0,0,0,'')

select * from #er4 order by srno1,srno2,srno3
drop table #er4
DROP TABLE #TITEM
DROP TABLE #TITEM2
DROP TABLE #stmain