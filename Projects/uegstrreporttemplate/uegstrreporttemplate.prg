LPARAMETERS LRPT_PRD,LRANGE
*!*	LParameters LRPT_PRD,LREP_NM,
*!*	_lOPTCOMPO = UPPER(coadditional.optCompo)
*!*	IF UPPER(LREP_NM)='GSTRXLSDATA' AND _lOPTCOMPO = 'YES'
*!*		=MESSAGEBOX("Dealer under compostion Scheme See the GSTR-4 Quarterly Return Report ",16,vUmess)
*!*		RETURN .F.
*!*	ENDIF 

*!*	IF UPPER(LREP_NM) ='GSTR3'  AND _lOPTCOMPO = 'YES'
*!*		=MESSAGEBOX("Dealer under compostion Scheme See the GSTR-4 Quarterly Return Report ",16,vUmess)
*!*		RETURN .F.
*!*	ENDIF 
*!*	IF UPPER(LREP_NM) ='GSTR4'  AND _lOPTCOMPO = 'NO'
*!*		=MESSAGEBOX("Dealer not under compostion Scheme See the GSTR-3 Monthly Return Report ",16,vUmess)
*!*		RETURN .F.
*!*	ENDIF 
*!*	do form uegstreport With LRPT_PRD,UPPER(LREP_NM),LRANGE
do form uegstrreporttemplate With LRPT_PRD,LRANGE
