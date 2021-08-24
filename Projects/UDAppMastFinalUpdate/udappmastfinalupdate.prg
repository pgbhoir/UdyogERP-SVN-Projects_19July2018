
If Type("_Screen.ActiveForm.Mainalias")<>"U"
	If oglblprdfeat.udchkprod('AutoTran')
		If Inlist(Upper(Alltrim(_Screen.ActiveForm.Mainalias)),"IT_MAST_VW","AC_MAST_VW","ITEM_GROUP_VW","AC_GROUP_MAST_VW","DEPARTMENT_VW","CATEGORY_VW","WAREHOUSE_VW","SERIES_VW","_STMAST","_DCMAST","_LOTHER")
			=uedataexport("INIT","M",Upper(Alltrim(_Screen.ActiveForm.maintbl)))
			=uedataexport("PROCESS")
		Endif
	Endif
Endif

DO VouFinalUpdate IN uemastamendment.app

&& Added by Shrikant S. on 280/06/2018 for Bug-30904		&& Start
*!*	** Start : Added by Uday on 29/04/2014 for Bug-21965		
_mMLA = .f.                              
_mMLA = oglblprdfeat.udchkprod('mulvlappr') 
IF _mMLA = .t.
	_curmastobj = _Screen.ActiveForm

	IF (ALLTRIM(UPPER(_curmastobj.maintbl)) = "AC_MAST")
		IF !INLIST(ALLTRIM(UPPER(ac_mast_vw.group)),"SUNDRY CREDITORS","SUNDRY DEBTORS")
			RETURN .t.
		ENDIF 
	ENDIF 
	
	IF (ALLTRIM(UPPER(_curmastobj.maintbl)) = "AC_MAST")
		Sq1 = "SELECT applevel,applevel_init from mastcode " +;
	  		  "where code = 'AM' and applevel > 0"
	ELSE IF (ALLTRIM(UPPER(_curmastobj.maintbl)) = "IT_MAST")
		Sq1 = "SELECT applevel,applevel_init from mastcode " +;
	  		  "where code = 'IM' and applevel > 0"		
	ENDIF 

	nretval = _curmastobj.sqlconobj.dataconn("EXE",company.dbname,Sq1,"curmastcode","_curmastobj.nhandle",_curmastobj.DataSessionId)	  
	IF nretval<=0
		WAIT CLEAR
		MESSAGEBOX("Unable to Delete Data..!!",16,vumess)
		RETURN .F.
	ENDIF
	
	STORE .f. TO Isfindmastcode 
	IF USED("curmastcode")
		IF RECCOUNT("curmastcode") > 0
			Isfindmastcode = .t.
		ENDIF
	ENDIF 
	
	IF Isfindmastcode = .t.
		STORE .T. TO isSave
		DO CASE
		CASE _curmastobj.addmode = .F. AND _curmastobj.editmode = .F.
			isSave = ProcMLADelete()  && Multi level approval records get deleted
		CASE _curmastobj.addmode = .T. or _curmastobj.editmode = .T.
			isSave = ProcMLASave()  && Multi level approval records get generated
		ENDCASE
		
		IF isSave = .F. &&AND isCheck = .T.
			RETURN .F.
		ENDIF
	ENDIF 
ENDIF 

*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
* Procedure ProcMLASave
* Calling 2 procedures for the Account master & Item master
* It Generates records of Approval and Reject Approval 
*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

PROCEDURE ProcMLASave
IF _curmastobj.editmode = .T. AND _curmastobj.addmode = .F.
	IF (ALLTRIM(UPPER(_curmastobj.maintbl)) = "AC_MAST") && Account Master
		IF curmastcode.applevel_init = .t.
			Sq1= " Select id from applevel_dtl where entry_ty = 'AM' and Tran_cd = ?ac_mast_vw.ac_id and initiator_id =?ac_mast_vw.User_name"
		ELSE
			Sq1= " Select id from applevel_dtl where entry_ty = 'AM' and Tran_cd = ?ac_mast_vw.ac_id "
		ENDIF 

		nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursappleveldtl","_curmastobj.nHandle",_curmastobj.DATASESSIONID)
		IF nretval<=0
			WAIT CLEAR
			MESSAGEBOX("Unable to get Details from Database..!!",16,vumess)
			RETURN .F.
		ENDIF

		STORE .f. TO IsFound
		IF USED("cursappleveldtl")
			IF RECCOUNT("cursappleveldtl") > 0
				IsFound = .t. 
			ENDIF 
		ENDIF 

		IF USED("cursappleveldtl")
			USE IN cursappleveldtl
		ENDIF 
		
		IF IsFound = .t.
			DO ProcAcMastDoReject
		ELSE 
			DO ProcAcMastDoGenerateApproval
		ENDIF 
		
	ELSE if (ALLTRIM(UPPER(_curmastobj.maintbl)) = "IT_MAST") && Item Master
			STORE "" TO Sq1
			IF curmastcode.applevel_init = .t.
				Sq1= " Select id from applevel_dtl where entry_ty = 'IM' and Tran_cd = ?it_mast_vw.it_code and initiator_id =?it_mast_vw.User_name"
			ELSE
				Sq1= " Select id from applevel_dtl where entry_ty = 'IM' and Tran_cd = ?it_mast_vw.it_code "
			ENDIF 

			nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursappleveldtl","_curmastobj.nHandle",_curmastobj.DATASESSIONID)
			IF nretval<=0
				WAIT CLEAR
				MESSAGEBOX("Unable to get Details from Database..!!",16,vumess)
				RETURN .F.
			ENDIF

			STORE .f. TO IsFound
			IF USED("cursappleveldtl")
				IF RECCOUNT("cursappleveldtl") > 0
					IsFound = .t. 
				ENDIF 
			ENDIF 

			IF USED("cursappleveldtl")
				USE IN cursappleveldtl
			ENDIF 
			
			IF IsFound = .t.
				DO ProcItMastDoReject
			ELSE
			 	DO ProcItMastDoGenerateApproval
		 	ENDIF 
	ENDIF 
ELSE
	IF _curmastobj.addmode = .T. AND _curmastobj.editmode = .F.
		IF (ALLTRIM(UPPER(_curmastobj.maintbl)) = "AC_MAST") && Account Master
			DO ProcAcMastDoGenerateApproval
		ELSE if (ALLTRIM(UPPER(_curmastobj.maintbl)) = "IT_MAST") && Item Master
			DO ProcItMastDoGenerateApproval
		ENDIF 	
	ENDIF 
ENDIF
ENDPROC 
*-*-*-*-*-*-*-*-*-*-*-*
* End
*-*-*-*-*-*-*-*-*-*-*-*


*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
* Procedure ProcMLADelete
* Delete records from AppLevel_Dtl table for the Account Master and Item Master
*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

PROCEDURE ProcMLADelete
IF (ALLTRIM(UPPER(_curmastobj.maintbl)) = "AC_MAST") && Account Master
	Sq1 = "SELECT id FROM applevel_dtl " +;
		  "where entry_ty = 'AM'  and tran_cd = ?ac_mast_vw.ac_id and levelstatus ='Approved'"
		  
	nretval = _curmastobj.sqlconobj.dataconn("EXE",company.dbname,Sq1,"Cursdel","_curmastobj.nhandle",_curmastobj.DataSessionId)	  
	IF nretval<=0
		WAIT CLEAR
		MESSAGEBOX("Unable to Delete Data..!!",16,vumess)
		RETURN .F.
	ENDIF

	IF USED("cursdel")
		IF RECCOUNT("cursdel") <> 0
			USE IN cursdel
			MESSAGEBOX("Cannot delete this transaction, Transaction has been approved already.....",vumess,0+16)
			RETURN .f.
		ENDIF 
		USE IN cursdel
	ENDIF 
		  
	SELECT ac_mast_vw
	GO TOP
	Sq1= " delete from applevel_dtl where entry_ty = 'AM' and tran_cd = ?ac_mast_vw.ac_id " &&and itserial in ("+List_Itsr+")"
	nretval = 1
	nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"","_curmastobj.nHandle",_curmastobj.DATASESSIONID)
	IF nretval<=0
		WAIT CLEAR
		MESSAGEBOX("Unable to Delete Details from Database..!!",16,vumess)
		RETURN .F.
	ENDIF
ELSE
	IF (ALLTRIM(UPPER(_curmastobj.maintbl)) = "IT_MAST") && Item Master
		Sq1 = "SELECT id FROM applevel_dtl " +;
			  "where entry_ty = 'IM'  and tran_cd = ?it_mast_vw.it_code and levelstatus ='Approved'"
			  
		nretval = _curmastobj.sqlconobj.dataconn("EXE",company.dbname,Sq1,"Cursdel","_curmastobj.nhandle",_curmastobj.DataSessionId)	  
		IF nretval<=0
			WAIT CLEAR
			MESSAGEBOX("Unable to Delete Data..!!",16,vumess)
			RETURN .F.
		ENDIF

		IF USED("cursdel")
			IF RECCOUNT("cursdel") <> 0
				USE IN cursdel
				MESSAGEBOX("Cannot delete this transaction, Transaction has been approved already.....",vumess,0+16)
				RETURN .f.
			ENDIF 
			USE IN cursdel
		ENDIF 
			  
		SELECT it_mast_vw
		GO TOP
		Sq1= " delete from applevel_dtl where entry_ty = 'IM' and tran_cd = ?it_mast_vw.it_code " &&and itserial in ("+List_Itsr+")"
		nretval = 1
		nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"","_curmastobj.nHandle",_curmastobj.DATASESSIONID)
		IF nretval<=0
			WAIT CLEAR
			MESSAGEBOX("Unable to Delete Details from Database..!!",16,vumess)
			RETURN .F.
		ENDIF
	ENDIF 
ENDIF 
WAIT CLEAR
RETURN .T.
ENDPROC
*-*-*-*-*-*-*-*-*-*-*-*
* End
*-*-*-*-*-*-*-*-*-*-*-*


*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
* Procedure ProcAcMastDoGenerateApproval
* Generate Approval records into AppLevel_Dtl table for the Account Master 
*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

PROCEDURE ProcAcMastDoGenerateApproval
STORE "" TO Sq1
IF curmastcode.applevel_init = .t.
	Sq1= " Select id from applevel_dtl where entry_ty = 'AM' and Tran_cd = ?ac_mast_vw.ac_id and initiator_id =?ac_mast_vw.User_name"
ELSE
	Sq1= " Select id from applevel_dtl where entry_ty = 'AM' and Tran_cd = ?ac_mast_vw.ac_id "
ENDIF 

nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursappleveldtl","_curmastobj.nHandle",_curmastobj.DATASESSIONID)
IF nretval<=0
	WAIT CLEAR
	MESSAGEBOX("Unable to get Details from Database..!!",16,vumess)
	RETURN .F.
ENDIF

STORE .f. TO IsFound
IF USED("cursappleveldtl")
	IF RECCOUNT("cursappleveldtl") > 0
		IsFound = .t. 
	ENDIF 
ENDIF 

IF USED("cursappleveldtl")
	USE IN cursappleveldtl
ENDIF 

IF IsFound = .F.
	&& Customization 
	STORE "" TO mlacond
	IF FILE("udappvoufinalupdate_mlacond.fxp")
		mlacond = udappvoufinalupdate_mlacond()   && Customization in conditions
	ENDIF 

	STORE "" TO Sq1
	IF curmastcode.applevel_init = .t.
		Sq1= " Select entry_ty,applevel,initiator_id from applevel_mstr where entry_ty = 'AM' and initiator_id =?ac_mast_vw.User_name "+IIF(!EMPTY(mlacond)," AND " + mlacond + "","")+" order by applevel desc"
	ELSE
		Sq1= " Select entry_ty,applevel,initiator_id from applevel_mstr where entry_ty = 'AM' "+IIF(!EMPTY(mlacond)," AND " + mlacond +"","")+" order by applevel desc"
	ENDIF 
	
	nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursapplevelmstr","_curmastobj.nHandle",_curmastobj.DATASESSIONID)
	IF nretval<=0
		WAIT CLEAR
		MESSAGEBOX("Unable to get Details from Database..!!",16,vumess)
		RETURN .F.
	ENDIF

	CREATE CURSOR Tbl_SqlString(cmdInsStr MEMO,cmdDelStr MEMO,cmdFinStr MEMO)   && Create temporary cursor for string
	APPEND BLANK IN Tbl_SqlString
	
	STORE 0 TO fstlevel,m_applevel
	DO WHILE !EOF("cursapplevelmstr")
		IF fstlevel = 0
			fstlevel = cursapplevelmstr.applevel
		ENDIF 

		IF (cursapplevelmstr.applevel == m_applevel)
			SKIP IN cursapplevelmstr
			LOOP 
		ENDIF 
	
		Replace cmdInsStr WITH cmdInsStr +; 
							  "insert into applevel_dtl(entry_ty,tran_cd,applevel,initiator_id,user_role,levelstatus,prelevelstatus,add_user,add_dt) values ('" +;
							  cursapplevelmstr.entry_ty + "'," +;
							  STR(ac_mast_vw.ac_id) + "," +;
							  STR(cursapplevelmstr.applevel) + ",'" +;
							  ALLTRIM(cursapplevelmstr.initiator_id) + "'," +;
							  "''," +;
							  "'','" +;
							  IIF(cursapplevelmstr.applevel = fstlevel,"Approved","") + "','" +;
							  ALLTRIM(musername) +"','" +;
							  ac_mast_vw.sysdate + "')" IN Tbl_SqlString
							  
		m_applevel =  cursapplevelmstr.applevel		
		SKIP IN cursapplevelmstr
	ENDDO 
	
	IF !EMPTY(Tbl_SqlString.cmdInsStr)
		REPLACE cmdFinStr WITH "set dateformat dmy "+cmdInsStr IN Tbl_SqlString
		
		nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Tbl_SqlString.cmdFinStr,"","_curmastobj.nHandle",_curmastobj.DATASESSIONID,.T.)
		IF nretval<=0
			WAIT CLEAR
			MESSAGEBOX("Unable to update details in database..!!",16,vumess)
			RETURN .F.
		ENDIF
	ENDIF 
	
	IF USED("cursapplevelmstr")
		USE IN cursapplevelmstr
	ENDIF
	
	IF USED("Tbl_SqlString")
		USE IN Tbl_SqlString
	ENDIF
ENDIF 
RETURN .t.
ENDPROC 
*-*-*-*-*-*-*-*-*-*-*-*
* End
*-*-*-*-*-*-*-*-*-*-*-*


*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
* Procedure ProcItMastDoGenerateApproval
* Generate Approval records into AppLevel_Dtl table for the Item Master 
*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

PROCEDURE ProcItMastDoGenerateApproval
STORE "" TO Sq1
IF curmastcode.applevel_init = .t.
	Sq1= " Select id from applevel_dtl where entry_ty = 'IM' and Tran_cd = ?it_mast_vw.it_code and initiator_id =?it_mast_vw.User_name"
ELSE
	Sq1= " Select id from applevel_dtl where entry_ty = 'IM' and Tran_cd = ?it_mast_vw.it_code "
ENDIF 

nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursappleveldtl","_curmastobj.nHandle",_curmastobj.DATASESSIONID)
IF nretval<=0
	WAIT CLEAR
	MESSAGEBOX("Unable to get Details from Database..!!",16,vumess)
	RETURN .F.
ENDIF

STORE .f. TO IsFound
IF USED("cursappleveldtl")
	IF RECCOUNT("cursappleveldtl") > 0
		IsFound = .t. 
	ENDIF 
ENDIF 

IF USED("cursappleveldtl")
	USE IN cursappleveldtl
ENDIF 

IF IsFound = .F.
	&& Customization 
	STORE "" TO mlacond
	IF FILE("udappvoufinalupdate_mlacond.fxp")
		mlacond = udappvoufinalupdate_mlacond()   && Customization in conditions
	ENDIF 

	STORE "" TO Sq1
	IF curmastcode.applevel_init = .t.
		Sq1= " Select entry_ty,applevel,initiator_id from applevel_mstr where entry_ty = 'IM' and initiator_id =?it_mast_vw.User_name "+IIF(!EMPTY(mlacond)," AND " + mlacond + "","")+" order by applevel desc"
	ELSE
		Sq1= " Select entry_ty,applevel,initiator_id from applevel_mstr where entry_ty = 'IM' "+IIF(!EMPTY(mlacond)," AND " + mlacond +"","")+" order by applevel desc"
	ENDIF 
	
	nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursapplevelmstr","_curmastobj.nHandle",_curmastobj.DATASESSIONID)
	IF nretval<=0
		WAIT CLEAR
		MESSAGEBOX("Unable to get Details from Database..!!",16,vumess)
		RETURN .F.
	ENDIF

	CREATE CURSOR Tbl_SqlString(cmdInsStr MEMO,cmdDelStr MEMO,cmdFinStr MEMO)   && Create temporary cursor for string
	APPEND BLANK IN Tbl_SqlString
	
	STORE 0 TO fstlevel,m_applevel
	DO WHILE !EOF("cursapplevelmstr")
		IF fstlevel = 0
			fstlevel = cursapplevelmstr.applevel
		ENDIF 

		IF (cursapplevelmstr.applevel == m_applevel)
			SKIP IN cursapplevelmstr
			LOOP 
		ENDIF 
	
		Replace cmdInsStr WITH cmdInsStr +; 
							  "insert into applevel_dtl(entry_ty,tran_cd,applevel,initiator_id,user_role,levelstatus,prelevelstatus,add_user,add_dt) values ('" +;
							  cursapplevelmstr.entry_ty + "'," +;
							  STR(it_mast_vw.it_code) + "," +;
							  STR(cursapplevelmstr.applevel) + ",'" +;
							  ALLTRIM(cursapplevelmstr.initiator_id) + "'," +;
							  "''," +;
							  "'','" +;
							  IIF(cursapplevelmstr.applevel = fstlevel,"Approved","") + "','" +;
							  ALLTRIM(musername) +"','" +;
							  it_mast_vw.sysdate + "')" IN Tbl_SqlString
							  
		m_applevel =  cursapplevelmstr.applevel		
		SKIP IN cursapplevelmstr
	ENDDO 
	
	IF !EMPTY(Tbl_SqlString.cmdInsStr)
		REPLACE cmdFinStr WITH "set dateformat dmy "+cmdInsStr IN Tbl_SqlString
		
		nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Tbl_SqlString.cmdFinStr,"","_curmastobj.nHandle",_curmastobj.DATASESSIONID,.T.)
		IF nretval<=0
			WAIT CLEAR
			MESSAGEBOX("Unable to update details in database..!!",16,vumess)
			RETURN .F.
		ENDIF
	ENDIF 
	
	IF USED("cursapplevelmstr")
		USE IN cursapplevelmstr
	ENDIF
	
	IF USED("Tbl_SqlString")
		USE IN Tbl_SqlString
	ENDIF
ENDIF 
RETURN .t.
ENDPROC 
*-*-*-*-*-*-*-*-*-*-*-*
* End
*-*-*-*-*-*-*-*-*-*-*-*


*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
* Procedure ProcAcMastDoReject
* Generate Reject flow into AppLevel_Dtl table for the Account Master
* Move all previous records into AppLevel_Hist table and reset the Applevel_dtl table in new mode. 
*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

PROCEDURE ProcAcMastDoReject()
	STORE "" TO Sq1
	IF curmastcode.applevel_init = .t.
		Sq1= " Select id from applevel_dtl where entry_ty = 'AM' and Tran_cd = ?Ac_Mast_vw.ac_id and initiator_id =?Ac_Mast_vw.User_name "+;
			 " and levelstatus ='Rejected'"
	ELSE
		Sq1= " Select id from applevel_dtl where entry_ty = 'AM' and Tran_cd = ?Ac_Mast_vw.ac_id "+;
			 " and levelstatus ='Rejected'"
	ENDIF 	
			 
	nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursrejappleveldtl","_curmastobj.nHandle",_curmastobj.DATASESSIONID)
	IF nretval<=0
		WAIT CLEAR
		MESSAGEBOX("Unable to get Details from Database..!!",16,vumess)
		RETURN .F.
	ENDIF

	STORE .f. TO IsFound
	IF USED("cursrejappleveldtl")
		IF RECCOUNT("cursrejappleveldtl") > 0
			IsFound = .t. 
		ENDIF 
	ENDIF 
	
	IF IsFound = .f.	
		USE IN cursrejappleveldtl
		RETURN
	ENDIF 
	
	STORE "" TO Sq1
	IF curmastcode.applevel_init = .t.
		Sq1= " Select * from applevel_dtl where entry_ty = 'AM' and Tran_cd = ?Ac_Mast_vw.Ac_id  and initiator_id =?Ac_Mast_vw.User_name "
	ELSE
		Sq1= " Select * from applevel_dtl where entry_ty = 'AM' and Tran_cd = ?Ac_Mast_vw.Ac_id "
	ENDIF 
		 
	nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursappleveldtl","_curmastobj.nHandle",_curmastobj.DATASESSIONID)
	IF nretval<=0
		WAIT CLEAR
		MESSAGEBOX("Unable to get Details from Database..!!",16,vumess)
		RETURN .F.
	ENDIF

	CREATE CURSOR Tbl_SqlString(cmdInsStr MEMO,cmdUpdateStr MEMO,cmdFinStr MEMO)   && Create temporary cursor for string
	APPEND BLANK IN Tbl_SqlString
	
	GO TOP IN cursappleveldtl
	DO WHILE !EOF("cursappleveldtl")
		Replace cmdInsStr WITH cmdInsStr +; 
							  " insert into applevel_hist(entry_ty,tran_cd,applevel,initiator_id,user_role,levelstatus,prelevelstatus,add_user,add_dt) values ('" +;
							  cursappleveldtl.entry_ty + "'," +;
							  STR(Ac_mast_vw.ac_id) + "," +;
							  STR(cursappleveldtl.applevel) + ",'" +;
							  ALLTRIM(cursappleveldtl.initiator_id) + "','" +;
							  ALLTRIM(cursappleveldtl.user_role) + "','" + ;
							  ALLTRIM(cursappleveldtl.levelstatus) + "','" +;
							  ALLTRIM(cursappleveldtl.prelevelstatus) + "','" +;
							  ALLTRIM(musername) +"','" +;
							  Ac_mast_vw.sysdate + "')" IN Tbl_SqlString
							  
		IF (cursappleveldtl.levelstatus = "Rejected")
			EXIT 
		ENDIF 
							  
		SKIP IN cursappleveldtl		
	ENDDO 
	
	IF !EMPTY(Tbl_SqlString.cmdInsStr)
		REPLACE cmdFinStr WITH "set dateformat dmy "+cmdInsStr IN Tbl_SqlString
		
		nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Tbl_SqlString.cmdFinStr,"","_curmastobj.nHandle",_curmastobj.DATASESSIONID,.T.)
		IF nretval<=0
			WAIT CLEAR
			MESSAGEBOX("Unable to update details in database..!!",16,vumess)
			RETURN .F.
		ENDIF
	ENDIF 
	
	Replace cmdInsStr WITH "",;
			cmdUpdateStr with "",;
			cmdFinStr WITH "" in Tbl_SqlString
	
	IF lcode_vw.applevel_init = .t.
		Replace cmdUpdateStr WITH cmdUpdateStr + "update applevel_dtl set approved_remarks ='',rejected_remarks='',levelstatus='',user_role='' where entry_ty='AM' and " +; 
								  "tran_cd=" + ALLTRIM(STR(ac_mast_vw.ac_id)) + " and " +;
								  "initiator_id='" + ALLTRIM(ac_mast_vw.user_name) + "'" IN Tbl_SqlString
								  
		Replace cmdUpdateStr WITH cmdUpdateStr + "update applevel_dtl set prelevelstatus ='',prelevel=0 where entry_ty='AM' and " +; 
								  "tran_cd=" + ALLTRIM(STR(ac_mast_vw.ac_id)) + " and " +;
								  "initiator_id='" + ALLTRIM(ac_mast_vw.user_name) + "' and prelevel !=0" IN Tbl_SqlString

		Replace cmdUpdateStr WITH cmdUpdateStr + "update applevel_dtl set edit_user ='"+ALLTRIM(musername)+"',edit_dt='"+ac_mast_vw.sysdate+"' where entry_ty='AM' and " +; 
								  "tran_cd=" + ALLTRIM(STR(ac_mast_vw.ac_id)) + " and " +;
								  "initiator_id='" + ALLTRIM(ac_mast_vw.user_name) + "'" IN Tbl_SqlString							  
								  
	ELSE
		Replace cmdUpdateStr WITH cmdUpdateStr + "update applevel_dtl set approved_remarks ='',rejected_remarks='',levelstatus='',user_role='' where entry_ty='AM' and " +; 
								  "tran_cd=" + ALLTRIM(STR(ac_mast_vw.ac_id)) IN Tbl_SqlString
								  
		Replace cmdUpdateStr WITH cmdUpdateStr + "update applevel_dtl set prelevelstatus ='',prelevel=0 where entry_ty='AM' and " +; 
								  "tran_cd=" + ALLTRIM(STR(ac_mast_vw.ac_id)) + " and " +;
								  "initiator_id='" + ALLTRIM(ac_mast_vw.user_name) + "' and prelevel !=0" IN Tbl_SqlString

		Replace cmdUpdateStr WITH cmdUpdateStr + "update applevel_dtl set edit_user ='"+ALLTRIM(musername)+"',edit_dt='"+ac_mast_vw.sysdate+"' where entry_ty='AM' and " +; 
								  "tran_cd=" + ALLTRIM(STR(ac_mast_vw.ac_id)) + " and " +;
								  "initiator_id='" + ALLTRIM(ac_mast_vw.user_name) + "'" IN Tbl_SqlString				
	ENDIF
	 
	IF !EMPTY(Tbl_SqlString.cmdUpdateStr)
		REPLACE cmdFinStr WITH "set dateformat dmy "+cmdUpdateStr IN Tbl_SqlString
		
		nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,Tbl_SqlString.cmdFinStr,"","_currObj.nHandle",_currObj.DATASESSIONID,.T.)
		IF nretval<=0
			WAIT CLEAR
			MESSAGEBOX("Unable to update details in database..!!",16,vumess)
			RETURN .F.
		ENDIF
	ENDIF
	
	IF USED("cursrejappleveldtl")
		USE IN cursrejappleveldtl
	ENDIF 
	
	IF USED("cursappleveldtl")
		USE IN cursappleveldtl
	ENDIF 
	
	IF USED("Tbl_SqlString")
		USE IN Tbl_SqlString
	ENDIF 
ENDPROC
*-*-*-*-*-*-*-*-*-*-*-*
* End
*-*-*-*-*-*-*-*-*-*-*-*


*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
* Procedure ProcItMastDoReject
* Generate Reject flow into AppLevel_Dtl table for the Item Master
* Move all previous records into AppLevel_Hist table and reset the Applevel_dtl table in new mode. 
*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

PROCEDURE ProcItMastDoReject()
	STORE "" TO Sq1
	IF curmastcode.applevel_init = .t.
		Sq1= " Select id from applevel_dtl where entry_ty = 'IM' and Tran_cd = ?It_Mast_vw.it_code and initiator_id =?It_Mast_vw.User_name "+;
			 " and levelstatus ='Rejected'"
	ELSE
		Sq1= " Select id from applevel_dtl where entry_ty = 'IM' and Tran_cd = ?It_Mast_vw.it_code "+;
			 " and levelstatus ='Rejected'"
	ENDIF 	
			 
	nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursrejappleveldtl","_curmastobj.nHandle",_curmastobj.DATASESSIONID)
	IF nretval<=0
		WAIT CLEAR
		MESSAGEBOX("Unable to get Details from Database..!!",16,vumess)
		RETURN .F.
	ENDIF

	STORE .f. TO IsFound
	IF USED("cursrejappleveldtl")
		IF RECCOUNT("cursrejappleveldtl") > 0
			IsFound = .t. 
		ENDIF 
	ENDIF 
	
	IF IsFound = .f.	
		USE IN cursrejappleveldtl
		RETURN
	ENDIF 
	
	STORE "" TO Sq1
	IF curmastcode.applevel_init = .t.
		Sq1= " Select * from applevel_dtl where entry_ty = 'IM' and Tran_cd = ?It_Mast_vw.It_code  and initiator_id =?It_Mast_vw.User_name "
	ELSE
		Sq1= " Select * from applevel_dtl where entry_ty = 'IM' and Tran_cd = ?It_Mast_vw.It_code "
	ENDIF 
		 
	nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursappleveldtl","_curmastobj.nHandle",_curmastobj.DATASESSIONID)
	IF nretval<=0
		WAIT CLEAR
		MESSAGEBOX("Unable to get Details from Database..!!",16,vumess)
		RETURN .F.
	ENDIF

	CREATE CURSOR Tbl_SqlString(cmdInsStr MEMO,cmdUpdateStr MEMO,cmdFinStr MEMO)   && Create temporary cursor for string
	APPEND BLANK IN Tbl_SqlString
	
	GO TOP IN cursappleveldtl
	DO WHILE !EOF("cursappleveldtl")
		Replace cmdInsStr WITH cmdInsStr +; 
							  " insert into applevel_hist(entry_ty,tran_cd,applevel,initiator_id,user_role,levelstatus,prelevelstatus,add_user,add_dt) values ('" +;
							  cursappleveldtl.entry_ty + "'," +;
							  STR(It_Mast_vw.it_code) + "," +;
							  STR(cursappleveldtl.applevel) + ",'" +;
							  ALLTRIM(cursappleveldtl.initiator_id) + "','" +;
							  ALLTRIM(cursappleveldtl.user_role) + "','" + ;
							  ALLTRIM(cursappleveldtl.levelstatus) + "','" +;
							  ALLTRIM(cursappleveldtl.prelevelstatus) + "','" +;
							  ALLTRIM(musername) +"','" +;
							  It_Mast_vw.sysdate + "')" IN Tbl_SqlString
							  
		IF (cursappleveldtl.levelstatus = "Rejected")
			EXIT 
		ENDIF 
							  
		SKIP IN cursappleveldtl		
	ENDDO 
	
	IF !EMPTY(Tbl_SqlString.cmdInsStr)
		REPLACE cmdFinStr WITH "set dateformat dmy "+cmdInsStr IN Tbl_SqlString
		
		nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Tbl_SqlString.cmdFinStr,"","_curmastobj.nHandle",_curmastobj.DATASESSIONID,.T.)
		IF nretval<=0
			WAIT CLEAR
			MESSAGEBOX("Unable to update details in database..!!",16,vumess)
			RETURN .F.
		ENDIF
	ENDIF 
	
	Replace cmdInsStr WITH "",;
			cmdUpdateStr with "",;
			cmdFinStr WITH "" in Tbl_SqlString
	
	IF lcode_vw.applevel_init = .t.
		Replace cmdUpdateStr WITH cmdUpdateStr + "update applevel_dtl set approved_remarks ='',rejected_remarks='',levelstatus='',user_role='' where entry_ty='IM' and " +; 
								  "tran_cd=" + ALLTRIM(STR(it_Mast_vw.it_code)) + " and " +;
								  "initiator_id='" + ALLTRIM(it_mast_vw.user_name) + "'" IN Tbl_SqlString
								  
		Replace cmdUpdateStr WITH cmdUpdateStr + "update applevel_dtl set prelevelstatus ='',prelevel=0 where entry_ty='IM' and " +; 
								  "tran_cd=" + ALLTRIM(STR(it_mast_vw.it_code)) + " and " +;
								  "initiator_id='" + ALLTRIM(it_mast_vw.user_name) + "' and prelevel !=0" IN Tbl_SqlString

		Replace cmdUpdateStr WITH cmdUpdateStr + "update applevel_dtl set edit_user ='"+ALLTRIM(musername)+"',edit_dt='"+it_mast_vw.sysdate+"' where entry_ty='IM' and " +; 
								  "tran_cd=" + ALLTRIM(STR(it_mast_vw.it_code)) + " and " +;
								  "initiator_id='" + ALLTRIM(it_mast_vw.user_name) + "'" IN Tbl_SqlString							  
								  
	ELSE
		Replace cmdUpdateStr WITH cmdUpdateStr + "update applevel_dtl set approved_remarks ='',rejected_remarks='',levelstatus='',user_role='' where entry_ty='IM' and " +; 
								  "tran_cd=" + ALLTRIM(STR(it_mast_vw.it_code)) IN Tbl_SqlString
								  
		Replace cmdUpdateStr WITH cmdUpdateStr + "update applevel_dtl set prelevelstatus ='',prelevel=0 where entry_ty='IM' and " +; 
								  "tran_cd=" + ALLTRIM(STR(it_mast_vw.it_code)) + " and " +;
								  "initiator_id='" + ALLTRIM(it_mast_vw.user_name) + "' and prelevel !=0" IN Tbl_SqlString

		Replace cmdUpdateStr WITH cmdUpdateStr + "update applevel_dtl set edit_user ='"+ALLTRIM(musername)+"',edit_dt='"+ac_mast_vw.sysdate+"' where entry_ty='AM' and " +; 
								  "tran_cd=" + ALLTRIM(STR(ac_mast_vw.ac_id)) + " and " +;
								  "initiator_id='" + ALLTRIM(ac_mast_vw.user_name) + "'" IN Tbl_SqlString				
	ENDIF
	 
	IF !EMPTY(Tbl_SqlString.cmdUpdateStr)
		REPLACE cmdFinStr WITH "set dateformat dmy "+cmdUpdateStr IN Tbl_SqlString
		
		nretval = _curmastobj.sqlconobj.dataconn([EXE],company.dbname,Tbl_SqlString.cmdFinStr,"","_curmastobj.nHandle",_curmastobj.DATASESSIONID,.T.)
		IF nretval<=0
			WAIT CLEAR
			MESSAGEBOX("Unable to update details in database..!!",16,vumess)
			RETURN .F.
		ENDIF
	ENDIF
	
	IF USED("cursrejappleveldtl")
		USE IN cursrejappleveldtl
	ENDIF 
	
	IF USED("cursappleveldtl")
		USE IN cursappleveldtl
	ENDIF 
	
	IF USED("Tbl_SqlString")
		USE IN Tbl_SqlString
	ENDIF 
ENDPROC
*!*	** Added by Uday on 29/04/2014 for Bug-21965		&& End
&& Added by Shrikant S. on 280/06/2018 for Bug-30904		&& End
