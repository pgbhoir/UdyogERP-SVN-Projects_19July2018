Parameters oControl

sql_con = 0
oactive=_Screen.ActiveForm
tsrno = 0
If Type('oactive.pcvtype')<>'U'
	&&If Inlist(oactive.pcvtype,"AF","PY","ID","FO")  &&Commented by Prajakta B. on 08/01/2020 for Bug 33074
*!*		If Inlist(oactive.pcvtype,"AF","PY","ID","FO","P6")  &&Modified by Prajakta B. on 08/01/2020 for Bug 33074 && Commmented by Anil on 01-12-2020 for Bug 33252
	If Inlist(oactive.pcvtype,"AF","PY","ID","FO","P6","I7","I8") && Modified by Anil A on 07-02-2020 for Bug 33252
		If Type('oControl')='C'
			lcfld=oControl
		Else
			lcfld=Justext(oControl.ControlSource)
		Endif

		sq1 = ""
		If (oactive.addmode Or oactive.editmode) And Upper(Alltrim(lcfld))=="FDESIGN"
			sq1 = "Select [type] as ittype from it_mast where it_code=?item_vw.it_code"
			nretval = oactive.sqlconobj.dataconn([EXE],company.dbname,sq1,"_it_type","oactive.nHandle",oactive.DataSessionId)
			If Used("_it_type")
				Select _it_type
				If !Empty(Alltrim(_it_type.ittype)) And Inlist(Alltrim(Upper(_it_type.ittype)),"FINISHED","SEMI FINISHED")

					*!*						sq1= "SELECT DISTINCT [NAME] AS FDESIGN FROM TEXT_DESIGN_MASTER"  &&Commented by Priyanka B on 30112019 for Bug-33074
					sq1= "SELECT DISTINCT [NAME] AS FDESIGN FROM TEXT_DESIGN_MASTER	WHERE (DEACTIVE=0 OR (DEACTIVE=1 AND CAST(DEACTIVEFROM AS DATE) > ?MAIN_VW.DATE))"  &&Modified by Priyanka B on 30112019 for Bug-33074

					nretval = oactive.sqlconobj.dataconn([EXE],company.dbname,sq1,"curfdesign","oactive.nHandle",oactive.DataSessionId)
					If nretval <=0
						Return .F.
					Endif
					If Used("curfdesign")

						If nretval > 0 And Reccount('curfdesign') > 0
							lcTitle = "Select Fabric Design..."
							lcSrcFld  = [FDESIGN]
							lcFldList = [FDESIGN]
							lcFldCapt = [FDESIGN:Design]
							lcFldExcl = []
							lcFldRtrn = [FDESIGN]

							If Type('oControl')='O'
								lcstr = oControl.Value
							Else
								lcstr =""
							Endif

							RetItem=Uegetpop([curfdesign],lcTitle,lcSrcFld,lcFldList,lcstr,[],[],[],.F.,[],lcFldRtrn,lcFldCapt,lcFldExcl)

							If Vartype(RetItem)<>"U"
								If Type('oControl')='O'
									oControl.Value = RetItem
								Endif
*!*									Replace item_vw.FDESIGN With RetItem In item_vw  &&Commented by Priyanka B on 30112019 for Bug-33074
								Replace item_vw.FDESIGN With Iif(Vartype(RetItem)="L",lcstr,RetItem) In item_vw  &&Modified by Priyanka B on 30112019 for Bug-33074
							Endif
						Else
							sql_con = oactive.sqlconobj.sqlconnclose("oactive.nHandle")
							If Used('curfdesign')
								Use In curfdesign
							Endif
							Return .F.
						Endif
					Else
						Messagebox("No Records found!!",0+64,vumess)
					Endif
				Else
					Replace item_vw.FDESIGN With "" In item_vw
				Endif
				sql_con = oactive.sqlconobj.sqlconnclose("oactive.nHandle")
				If Used('curfdesign')
					Use In curfdesign
				Endif
			Endif
		Endif
	Endif
ENDIF


