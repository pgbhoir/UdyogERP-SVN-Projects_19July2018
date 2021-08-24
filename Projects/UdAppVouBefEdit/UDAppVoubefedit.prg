If 'trnamend' $ vChkprod
	Do VouBefEdit In MainPrg
Endif

&& Added by Shrikant S. on 27/06/2014 for Bug-23280		&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('pharmaind')
		_curvouobj = _Screen.ActiveForm
		If _curvouobj.itempage Or Inlist(main_vw.entry_ty,"AR","OS")
			If !Used('BatchTbl_Vw')
				etsql_str = "Select * From BatchGenTbl Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[BatchTbl_Vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con < 1 Or !Used("BatchTbl_Vw")
					etsql_con = 0
				Else
					Select BatchTbl_Vw
					Index On itserial Tag itserial
				Endif
			Endif
		Endif

		If _curvouobj.itempage Or Inlist(main_vw.entry_ty,"WK")
			If !Used('wkrmdet_vw')
				etsql_str = "Select * From WKRMDET Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[wkrmdet_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con < 1 Or !Used("wkrmdet_vw")
					etsql_con = 0
				Endif
			Endif
		Endif
	Endif
Endif
&& Added by Shrikant S. on 27/06/2014 for Bug-23280		&& End

&& Added by Sachin N. S. on 30/10/2017 for Bug-30782 -- Start
*!*	If oGlblPrdFeat.UdChkProd('vugst')   &&Commented by Priyanka B on 22032019 for Bug-32067
If oGlblPrdFeat.UdChkProd('vugst') Or oGlblPrdFeat.UdChkProd('vuisd') Or oGlblPrdFeat.UdChkProd('isdkgen')   &&Modified by Priyanka B on 22032019 for Bug-32067
	cdAmendDt = Iif(Type('Main_vw.AmendDate')='T','Main_vw.AmendDate',Iif(Type('Lmc_vw.AmendDate')='T','Lmc_vw.AmendDate',Iif(Type('MainAdd_vw.AmendDate')='T','MainAdd_vw.AmendDate','')))
	If !Empty(cdAmendDt)
		If !Empty(Evaluate(cdAmendDt)) And Evaluate(cdAmendDt)!=Ctod('01/01/1900')
			=Messagebox("This record is already Amended cannot change...!!!",0+16,vumess)
			Return .F.
		Endif
	Endif
Endif
&& Added by Sachin N. S. on 30/10/2017 for Bug-30782 -- End

&&Added by Priyanka B on 06082019 for Bug-32747 Start
If Vartype(oGlblPrdFeat)='O'
	If oGlblPrdFeat.UdChkProd('efabric')
		_curvouobj = _Screen.ActiveForm
		If Inlist(main_vw.entry_ty,"W1","ID","RD","W2","IC","AF")
			If !Used('_tbl_1')
				etsql_str = ""
				msg = ""
				Do Case
					Case main_vw.entry_ty = "W1"
						etsql_str = "select top 1 * from main where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and flotno<>''"
						msg = "Once selected Lot No. in the transaction cannot be change/modify the entry!!"

					Case main_vw.entry_ty = "ID"
*!*							etsql_str = "Declare @mflotno varchar(100),@sqlcommand nvarchar(max)"
*!*							etsql_str = etsql_str + " Select @mflotno = cast(stuff((select ','+char(39)+rtrim(flotno)+char(39) from iiitem where entry_ty=?Main_vw.Entry_ty "
*!*							etsql_str = etsql_str + " and tran_cd=?Main_vw.Tran_cd order by flotno for xml path('')),1,1,'') as varchar)"
*!*							etsql_str = etsql_str + " set @sqlcommand='Select top 1 b.code_nm as tran_name,a.flotno from main a inner join lcode b on (a.entry_ty=b.entry_ty) where flotno in ('+@mflotno+') and flotno<>'''''"
*!*							etsql_str = etsql_str + " execute sp_executesql @sqlcommand"

						etsql_str = "select a.entry_ty tran_type, c.code_nm as tran_name,i.flotno into #a from armain a inner join iimain b on (a.linked_with = rtrim(b.entry_ty)+ltrim(str(b.tran_cd))+rtrim(b.l_yn) "
						etsql_str = etsql_str + " and b.linked_with = rtrim(a.entry_ty)+ltrim(str(a.tran_cd))+rtrim(a.l_yn)) inner join iiitem i on (b.entry_ty=i.entry_ty and b.tran_cd=i.tran_cd) "
						etsql_str = etsql_str + " inner join lcode c on (b.entry_ty=c.entry_ty) where b.entry_ty=?Main_vw.entry_ty and b.tran_cd=?Main_vw.tran_cd"
						etsql_str = etsql_str + " select distinct tran_name from ("
						etsql_str = etsql_str + " Select m.entry_ty tran_type,c.code_nm as tran_name,m.flotno from main m inner join lcode c on (m.entry_ty=c.entry_ty) "
						etsql_str = etsql_str + " union all Select i.entry_ty tran_type,c.code_nm as tran_name,i.flotno from iritem i inner join lcode c on (i.entry_ty=c.entry_ty) "
						etsql_str = etsql_str + " union all Select i.entry_ty tran_type,c.code_nm as tran_name,i.flotno from ipitem i inner join lcode c on (i.entry_ty=c.entry_ty) "
						etsql_str = etsql_str + " union all Select i.entry_ty tran_type,c.code_nm as tran_name,i.flotno from opitem i inner join lcode c on (i.entry_ty=c.entry_ty) )aa where flotno in (select flotno from #a)"
*!*							msg = "Lot No. already in use..!!"+Chr(13)+"You cannot modify this transaction!!"

					Case main_vw.entry_ty = "RD"
						etsql_str = "select top 1 * from IRRMDET where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
						msg = "Allocation is already done against Fabric Labour Job Issue (IV)."+Chr(13)+"Entry cannot be modified!!"

					Case main_vw.entry_ty = "W2"
						etsql_str = "select top 1 * from main where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and flotno<>''"
						msg = "Once selected Cutting Lot No. in this transaction cannot be change/modify the entry!!"

					Case main_vw.entry_ty = "IC"
						etsql_str = "Declare @mflotno varchar(100),@sqlcommand nvarchar(max)"
						etsql_str = etsql_str + " Select @mflotno = cast(stuff((select ','+char(39)+rtrim(flotno)+char(39) from ipitem where entry_ty=?Main_vw.Entry_ty "
						etsql_str = etsql_str + " and tran_cd=?Main_vw.Tran_cd order by flotno for xml path('')),1,1,'') as varchar)"
						etsql_str = etsql_str + " set @sqlcommand='Select top 1 b.code_nm as tran_name,a.flotno from main a inner join lcode b on (a.entry_ty=b.entry_ty) where flotno in ('+@mflotno+') and flotno<>'''''"
						etsql_str = etsql_str + " execute sp_executesql @sqlcommand"
						msg = "Cutting Lot No. already in use..!!"+Chr(13)+"You cannot modify this transaction!!"

					Case main_vw.entry_ty = "AF"
						etsql_str = "select a.entry_ty tran_type, c.code_nm as tran_name,i.flotno into #a from armain a inner join iimain b on (a.linked_with = rtrim(b.entry_ty)+ltrim(str(b.tran_cd))+rtrim(b.l_yn) "
						etsql_str = etsql_str + " and b.linked_with = rtrim(a.entry_ty)+ltrim(str(a.tran_cd))+rtrim(a.l_yn)) inner join iiitem i on (b.entry_ty=i.entry_ty and b.tran_cd=i.tran_cd) "
						etsql_str = etsql_str + " inner join lcode c on (b.entry_ty=c.entry_ty) where a.entry_ty=?Main_vw.entry_ty and a.tran_cd=?Main_vw.tran_cd"
						etsql_str = etsql_str + " select distinct tran_name from ("
						etsql_str = etsql_str + " Select m.entry_ty tran_type,c.code_nm as tran_name,m.flotno from main m inner join lcode c on (m.entry_ty=c.entry_ty) "
						etsql_str = etsql_str + " union all Select i.entry_ty tran_type,c.code_nm as tran_name,i.flotno from iritem i inner join lcode c on (i.entry_ty=c.entry_ty) "
						etsql_str = etsql_str + " union all Select i.entry_ty tran_type,c.code_nm as tran_name,i.flotno from ipitem i inner join lcode c on (i.entry_ty=c.entry_ty) "
						etsql_str = etsql_str + " union all Select i.entry_ty tran_type,c.code_nm as tran_name,i.flotno from opitem i inner join lcode c on (i.entry_ty=c.entry_ty) )aa where flotno in (select flotno from #a)"
&&						msg = "Transaction is already made against this!!"+CHR(13)+"You cannot modify this transaction!!"
					Otherwise
				Endcase
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,'_tbl_1',"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If Reccount('_tbl_1')>0
					If Inlist(main_vw.entry_ty,"AF","ID")
						tname= ""
						Select _tbl_1
						Scan
							tname = Iif(Empty(tname),Alltrim(_tbl_1.tran_name),tname+ "," + Alltrim(_tbl_1.tran_name))
							Select _tbl_1
						Endscan
						If Reccount('_tbl_1')>1
							Messagebox(Alltrim(tname) +" entries are passed against this transaction."+Chr(13)+"You cannot modify this transaction!!",0+16,vumess)
						Else
							Messagebox(Alltrim(tname) +" entry is passed against this transaction."+Chr(13)+"You cannot modify this transaction!!",0+16,vumess)
						Endif
					Else
						=Messagebox(msg,0+16,vumess)
					Endif


					If Used('_tbl_1')
						Use In _tbl_1
					Endif
					Return .F.
				Endif
			Endif
			If Used('_tbl_1')
				Use In _tbl_1
			Endif
		Endif
	Endif
Endif
&&Added by Priyanka B on 06082019 for Bug-32747 End
