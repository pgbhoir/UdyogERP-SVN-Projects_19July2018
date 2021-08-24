***************************************************Auto Transaction*************************************

*Birendra : On 5july2011 TKT - 8452 :Start:
_curvouobj = _Screen.ActiveForm
*Birendra : on 23 Sept 2011 for Auto IP/OP BOM :Start:
ztmpalias = ''
ztmpalias = Alias()
*Birendra : on 23 Sept 2011 for Auto IP/OP BOM :End:

*!*	IF (oglblprdfeat.udchkprod('AutoTran') OR oglblprdfeat.udchkprod('procinv')) AND !lcode_vw.QC_Module  &&Commented by Priyanka B on 04092019 for Bug-32747
If (oglblprdfeat.udchkprod('AutoTran') Or oglblprdfeat.udchkprod('procinv') Or oglblprdfeat.udchkprod('efabric')) And !lcode_vw.QC_Module  &&Modified by Priyanka B on 04092019 for Bug-32747
	If File('uedataexport.app')
		lcSqlstr = "select * from sysobjects where [name] = 'Tbl_DataExport_Mast'"
		mretval = _curvouobj.SqlConObj.dataconn("EXE",company.dbname,lcSqlstr,[Tbl_DbExport_vw],"_curvouobj.nhandle",_curvouobj.DataSessionId,.T.)
		If mretval <= 0
			Return .F.
		Endif
		If Used('Tbl_DbExport_vw')
			lcSqlstr = "SELECT TOP 1 * FROM Tbl_DataExport_Mast WHERE cMastcode = '"+Main_Vw.Entry_ty+"' AND cType = 'T'"
			mretval = _curvouobj.SqlConObj.dataconn("EXE",company.dbname,lcSqlstr,[Tbl_DbExport_vw],"_curvouobj.nhandle",_curvouobj.DataSessionId,.T.)
			If mretval <= 0
				Return .F.
			Endif
			If Reccount("Tbl_DbExport_vw") = 0
				Select Tbl_DbExport_vw
				Use In Tbl_DbExport_vw
			Else
				Select Tbl_DbExport_vw
*!*					Use In Tbl_DbExport_vw  &&Commented by Priyanka B on 01082019 for Bug-32747
				If Type("_Screen.ActiveForm.Mainalias")<>"U"
&&Commented by Priyanka B on 01082019 for Bug-32747 Start
*!*						=uedataexport("INIT","T",Main_Vw.Entry_ty)
*!*						=uedataexport("PROCESS")
&&Commented by Priyanka B on 01082019 for Bug-32747 End

&&Added by Priyanka B on 01082019 for Bug-32747 Start
					If Type('Tbl_DbExport_vw.partycond')<>'U' And !Empty(Tbl_DbExport_vw.partycond)
						If Evaluate(Tbl_DbExport_vw.partycond)
							=uedataexport("INIT","T",Main_Vw.Entry_ty)
							=uedataexport("PROCESS")
						Else
							Return .T.
						Endif
					Else
						=uedataexport("INIT","T",Main_Vw.Entry_ty)
						=uedataexport("PROCESS")
					Endif
&&Added by Priyanka B on 01082019 for Bug-32747 End
				Endif
				Use In Tbl_DbExport_vw  &&Added by Priyanka B on 01082019 for Bug-32747
			Endif
		Endif
	Endif
Endif
*Birendra : on 23 Sept 2011 for Auto IP/OP BOM :Start:
If Used("projectitref_vw")
	Use In projectitref_vw
Endif
If Not Empty(ztmpalias)
	Select &ztmpalias
Endif
*Birendra : on 23 Sept 2011 for Auto IP/OP BOM :End:

*Birendra : On 5july2011 TKT - 8452 :End:
*****************************************************************************************
