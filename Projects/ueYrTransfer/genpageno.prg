Lparameters fldnm,tblnm,ooSqlConObj,ooHandle,nDataSessionId,vcDbName,vdSta_Dt,vdEnd_Dt,lcType
Local VRNO, nHandle
*!*	nHandle=0			&& Commented by Shrikant S. on 20/05/2010 for TKT-1476
msqlstr="SELECT MAX(CAST("+Alltrim(fldnm)+" AS INT)) AS RNO  FROM "+ALLTRIM(vcDbName)+".."+Alltrim(tblnm)+" WHERE ISNUMERIC( "+Alltrim(fldnm)+" )=1 and ctype='"+ALLTRIM(lcType)+"'"
mRetval = ooSqlConObj.dataconn([EXE],vcdbname,mSqlStr,"EXCUR",ooHandle,nDataSessionId,.T.)
If mRetval<0
	Return .F.
Endif
Select EXCUR
VRNO=Alltrim(Str(Iif(Isnull(EXCUR.RNO),1,(EXCUR.RNO)+1)))
If Used("EXCUR")
	Use In EXCUR
Endif
*sele(mAlias)
Return VRNO
