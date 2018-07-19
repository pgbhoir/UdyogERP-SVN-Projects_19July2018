If Exists(Select [name] From SysObjects Where xtype='P' and [Name]='USP_REP_UA_FORMVAT3_ANNEXURE14A_DADOS')
Begin
	Drop Procedure USP_REP_UA_FORMVAT3_ANNEXURE14A_DADOS
End
Go
      
CREATE PROCEDURE [dbo].[USP_REP_UA_FORMVAT3_ANNEXURE14A_DADOS]
@SDATE SMALLDATETIME,@EDATE SMALLDATETIME            
AS        
BEGIN           
--Details of sale (taxable) return within the State(within six month)
SELECT A.PARTY_NM,AC.S_TAX,STINV=ISNULL(ISNULL(B.RINV_NO,'')+' DT.'+CONVERT(VARCHAR(10),CASE WHEN YEAR(B.RDATE)<=1900 THEN '' ELSE B.RDATE END,110),''),A.DATE,CHALAN=A.INV_NO+' DT.'+CONVERT(VARCHAR(10),A.DATE,110),CREDIT='',A.TAXAMT,IT.EIT_NAME FROM SRITEM A
LEFT OUTER JOIN SRITREF B ON(A.TRAN_CD=B.TRAN_CD)
INNER JOIN AC_MAST AC ON (A.AC_ID=AC.AC_ID) 
INNER JOIN IT_MAST IT ON(A.IT_CODE=IT.IT_CODE)
WHERE AC.ST_TYPE in ('LOCAL') AND AC.S_TAX<>'' and (A.DATE BETWEEN @SDATE AND @EDATE)      


END  
