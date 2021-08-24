Parameters _lnDataSession,_actform

IF EMPTY(Main_vw.Entry_ty) OR _actform.addmode OR _actform.editmode
	return
endif
*DO FORM frmcsvimp WITH _lnDataSession,_addMode,_editMode,_actform
If Messagebox("Do you want to Export CSV?",4,Vumess)=6
	sql_con = _actform.SqlConObj.DataConn([EXE],Company.DbName,"Select * From CSV_Export Where Entry_ty=?_actform.pcvtype",;
		[csvsetting],"_actform.nHandle",_actform.DataSessionId,.F.)
	Wait Window "Please wait... Generating CSV" TIMEOUT 1 

	If Used('csvsetting')
		lcspname=Alltrim(csvsetting.sp_name)
		lccsvfilepath=Alltrim(csvsetting.csv_path)

		If Empty(lccsvfilepath)
			lcDir = Getdir(aPath,"Select Path to Save CSV File...",Vumess,16+64+8)
			If Vartype(lcDir) = "C"
				lccsvfilepath=lcDir
			Endif
		Endif

		Select Main_vw
		lnFieldCount = Afields(laFields)

		lcfilename=EvalString(Strtran(Alltrim(csvsetting.file_name),"m.","MAIN_VW."))
		lcemailsub=EvalString(Strtran(Alltrim(csvsetting.emailsub),"m.","MAIN_VW."))
		lcemailbody=EvalString(STRTRAN(Strtran(Alltrim(csvsetting.emailbody),"m.","MAIN_VW."),"c.","csvsetting."))
		lcfiltcond=EvalString(Strtran(Alltrim(csvsetting.Filter_cond),"m.","MAIN_VW."))

		lcfilename=Strtran(lcfilename,"/","")
		lcfilename=Strtran(Strtran(Strtran(lcfilename,"/","_"),":","_")," ","_")+".csv"


		lcemailsub=Strtran(lcemailsub,"/","")
		lcemailbody=Strtran(lcemailbody,"/","")
		lcsign=Alltrim(csvsetting.signature)
		lcsigninit=Alltrim(csvsetting.signinit)
		lcfiltcond=Strtran(lcfiltcond,"/","")

		If !Empty(lcspname)

			lcspname=" Execute "+lcspname+" '"+lcfiltcond+"','"+Main_vw.Entry_ty+"'"
			sql_con = _actform.SqlConObj.DataConn([EXE],Company.DbName,lcspname,[csvdataexp],"_actform.nHandle",_actform.DataSessionId,.F.)

			If sql_con <=0
				Return .F.
			ENDIF
			
			Wait Window "Please wait... Writing CSV" TIMEOUT 1
			lcfilename=lccsvfilepath+Iif(Right(lccsvfilepath,1)=="\","","\")+lcfilename
			WriteCSV(lcfilename)
			If File(lcfilename)
				If Type('Lcode_vw.lccsvemail')<>'U'
					_mailatt=lcfilename

					_mailto = ''
					_actfrmcon = _actform.SqlConObj.DataConn([EXE],Company.DbName,;
						"Select email from ac_mast where ac_id = ?Main_vw.Ac_id",[TmpPrt_Vw],"_actform.nHandle",_actform.DataSessionId,.F.)
					If _actfrmcon > 0 And Used('TmpPrt_Vw')
						Select TmpPrt_Vw
						If Reccount() >= 1
							_mailto = TmpPrt_Vw.email
						Endif
					Endif


					If Lcode_vw.lccsvemail=.T.
						Wait Window "Please wait... Mailing attachment" TIMEOUT 1
						Do UeMailing With _mailto,"",lcemailsub,lcemailbody,_mailatt
					Endif
				Endif
				If Empty(csvsetting.csv_path)
					lcspname="Update CSV_Export set csv_path=?lccsvfilepath Where Entry_ty=?Main_vw.Entry_ty "
					sql_con = _actform.SqlConObj.DataConn([EXE],Company.DbName,lcspname,[],"_actform.nHandle",_actform.DataSessionId,.F.)

				Endif

			Endif
		Endif
		Wait Clear
	Endif
Endif


Procedure WriteCSV
Parameters llcfilename


Local lnFields
Select csvdataexp
lnFieldCount = Afields(laFields)
lnHandle = Fcreate(llcfilename, 0)

Assert lnHandle > 0 Message "Unable to create CSV file"

lcRow = ""
For lnFields = 1 To lnFieldCount
	lcRow = lcRow + Iif(Empty(lcRow), "", "||") + ;
		ALLTRIM(laFields[lnFields,1])
Endfor
=Fputs(lnHandle, lcRow)

Select csvdataexp
Scan
	lcRow = ""
	For lnFields = 1 To lnFieldCount
		lcRow = lcRow + Iif(Empty(lcRow), "", "||") + ;
			ALLTRIM(Transform(Evaluate(laFields[lnFields,1])))

*!*			If Inlist(laFields[lnFields,2], 'C', 'M')
*!*				lcRow = lcRow + Iif(Empty(lcRow), "", ",") + '"' + ;
*!*					STRTRAN(Evaluate(laFields[lnFields,1]),'"', '""') + '"'
*!*			Else
*!*				lcRow = lcRow + Iif(Empty(lcRow), "", ",") + ;
*!*					TRANSFORM(Evaluate(laFields[lnFields,1]))
*!*			Endif
	Endfor

	=Fputs(lnHandle, lcRow)
Endscan
Fclose(lnHandle)
Endproc


Procedure EvalString
Parameters lcevalstring

Local retstrvalue
retstrvalue=''

strcount=Alines(aFieldList, lcevalstring, "/")
xstr=''
For fld=1 To strcount
	xstr=Getwordnum(lcevalstring,fld,"/")
	xstrval=xstr
	If "MAIN_VW." $ Upper(xstr)
		xstrval=Transform(Evaluate(xstrval))
	ENDIF
	IF ("CSVSETTING." $ Upper(xstr))
		xstrval=Transform(Evaluate(xstrval))
	endif
	retstrvalue=retstrvalue+Alltrim(xstrval)+" "
Endfor

Return retstrvalue
