_curvouobj = _Screen.ActiveForm
etsql_con = 1

&& Commented by Shrikant S. on 29/09/2016 for GST		&& Start
*!*	If _curvouobj.itempage Or Inlist(main_vw.entry_ty,"RR","RP","GI","GR","HI","HR") 	&& Changed by Shrikant S. on 29/09/2010 for TKT-4021

*!*	*!*	If _curvouobj.itempage Or Inlist(main_vw.entry_ty,"RR","RP","GI","GR","HI","HR","J2","VR") 	&& Changed by Shrikant S. on 29/09/2010 for TKT-4021	&& Changed by Sachin N. S. on 22/09/2012 for Bug-5164
*!*	*!*		If _curvouobj.addmode Or _curvouobj.editmode
*!*	*!*			If Used('Gen_SrNo_Vw')
*!*	*!*				Select gen_srno_vw
*!*	*!*				Replace All tran_cd With main_vw.tran_cd,entry_ty With main_vw.entry_ty,;
*!*	*!*					DATE With main_vw.Date,compid With main_vw.compid,l_yn With main_vw.l_yn In gen_srno_vw
*!*	*!*	*npgno,cit_code,cware,cgroup,cchapno
*!*	*!*	*itserial

*!*	*!*				etsql_str  = "Select * from Gen_SrNo Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
*!*	*!*				etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
*!*	*!*					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*				If etsql_con > 0 And Used('TmpEt_Vw')
*!*	*!*					Select tmpet_vw
*!*	*!*					Scan
*!*	*!*						metdele = .F.
*!*	*!*						Select item_vw
*!*	*!*						If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmpet_vw.itserial,'Item_vw','Eddits')
*!*	*!*							metdele = .T.
*!*	*!*						Else
*!*	*!*	*!*							If TmpEt_Vw.Cit_code # Item_vw.it_code &&Or TmpEt_Vw.Cware # Item_vw.Ware_nm  	&& Commented by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*	*!*							If tmpet_vw.cit_code # item_vw.it_code  And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO")	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*	*						If tmpet_vw.cit_code # item_vw.it_code  And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO")	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		&& Changed By Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 :Modified with below one:FORM402SRNO
*!*	*!*	&&						If tmpet_vw.cit_code # item_vw.it_code  And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO") Commented by sandeep for bug-21327 on 08-02-2014
*!*	*!*							If tmpet_vw.cit_code # item_vw.it_code  And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO","FORM403SRNO") &&Added by sandeep for bug-21327 on 08-02-2014

*!*	*!*								metdele = .T.
*!*	*!*							Endif
*!*	*!*	&& Added by Shrikant S. on 01 Feb, 2010  ------------Start
*!*	*!*							If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmpet_vw.itserial,'Item_vw','Eddits')
*!*	*!*								If tmpet_vw.cit_code = item_vw.it_code Or Empty(item_vw.u_pageno)&&Or TmpEt_Vw.Cware # Item_vw.Ware_nm
*!*	*!*									metdele = .T.
*!*	*!*								Endif
*!*	*!*							Endif
*!*	*!*	&& Added by Shrikant S. on 01 Feb, 2010  ------------End
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*	*!*							If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO")		&& 280910
*!*	*!*	*						If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO")		&& 280910	&& Changed By Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 :Modified with below one:FORM402SRNO
*!*	*!*	&&						If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO")  Commented by sandeep for  bug-21327 on 08-02-2014
*!*	*!*							If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO","FORM403SRNO")&&Added by sandeep for bug-21327 on 08-02-2014

*!*	*!*								If	(Type('main_vw.u_rg23no')='C' Or Type('main_vw.u_rg23cno')='C')
*!*	*!*									If	Empty(main_vw.u_rg23no) And Empty(main_vw.u_rg23cno)
*!*	*!*										metdele =.T.
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*								If	Type('main_vw.u_plasr')='C'
*!*	*!*									If Empty(main_vw.u_plasr)
*!*	*!*										metdele=.T.
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*	****** Added By Sachin N. S. on 18/09/2012 for Bug-5164 ****** Start
*!*	*!*								If	Type('main_vw.SERVTXSRNO')='C'
*!*	*!*									If Empty(main_vw.SERVTXSRNO)
*!*	*!*										metdele=.T.
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*	****** Added By Sachin N. S. on 18/09/2012 for Bug-5164 ****** End
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 : Start:
*!*	*!*								If	Type('main_vw.U_402srno')='C'
*!*	*!*									If Empty(main_vw.U_402srno)
*!*	*!*										metdele=.T.
*!*	*!*									Endif
*!*	*!*								Else
*!*	*!*									If	Type('lmc_vw.U_402srno')='C'
*!*	*!*										If Empty(lmc_vw.U_402srno)
*!*	*!*											metdele=.T.
*!*	*!*										Endif
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 : End:
*!*	*!*	&&Added by sandeep for bug-21327 on 08-02-2014	--->Start:
*!*	*!*								If	Type('main_vw.U_403srno')='C'
*!*	*!*									If Empty(main_vw.U_403srno)
*!*	*!*										metdele=.T.
*!*	*!*									Endif
*!*	*!*								Else
*!*	*!*									If	Type('lmc_vw.U_403srno')='C'
*!*	*!*										If Empty(lmc_vw.U_403srno)
*!*	*!*											metdele=.T.
*!*	*!*										Endif
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*	&&Added by sandeep for bug-21327 on 08-02-2014	<----End


*!*	*!*							Endif
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*						Endif
*!*	*!*						If metdele = .T. And etsql_con > 0
*!*	*!*							etsql_str = _curvouobj.sqlconobj.gendelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
*!*	*!*									Entry_ty = ?Main_vw.Entry_ty And Itserial = ?TmpEt_Vw.itserial")
*!*	*!*							etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
*!*	*!*								"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*						Else
*!*	*!*							Select gen_srno_vw
*!*	*!*							If Seek(tmpet_vw.itserial,'Gen_SrNo_Vw','ItSerial') And ;
*!*	*!*									gen_srno_vw.cit_code = tmpet_vw.cit_code And  ;
*!*	*!*									gen_srno_vw.cgroup = tmpet_vw.cgroup And gen_srno_vw.cchapno = tmpet_vw.cchapno		&&Gen_SrNo_Vw.Cware = TmpEt_Vw.Cware AND
*!*	*!*								Replace npgno With '' In gen_srno_vw
*!*	*!*							Endif
*!*	*!*						Endif
*!*	*!*						Select tmpet_vw
*!*	*!*					Endscan
*!*	*!*				Else
*!*	*!*					etsql_con = 0
*!*	*!*				Endif

*!*	*!*				Select gen_srno_vw
*!*	*!*				Scan
*!*	*!*					mrecno=Recno()			&& Added by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*					Select item_vw
*!*	*!*	*!*					If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+gen_srno_vw.itserial,'Item_vw','Eddits') And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO") && Changed by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*	*				If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+gen_srno_vw.itserial,'Item_vw','Eddits') And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO") && Changed by Shrikant S. on 29/09/2010 for TKT-4021	&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 :Modified with below one:FORM402SRNO
*!*	*!*	&&				If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+gen_srno_vw.itserial,'Item_vw','Eddits') And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO")  Commented by sandeep for  bug-21327 on 08-02-2014
*!*	*!*					If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+gen_srno_vw.itserial,'Item_vw','Eddits') And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO","FORM403SRNO") &&added by sandeep for  bug-21327 on 08-02-2014

*!*	*!*						Replace npgno With '' In gen_srno_vw
*!*	*!*					Else
*!*	*!*	*!*						If Gen_SrNo_Vw.Cit_code # Item_vw.it_code &&Or Gen_SrNo_Vw.Cware # Item_vw.Ware_nm
*!*	*!*	*!*						If gen_srno_vw.cit_code # item_vw.it_code And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO") && Changed by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*	*					If gen_srno_vw.cit_code # item_vw.it_code And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO") && Changed by Shrikant S. on 29/09/2010 for TKT-4021		&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 :Modified with below one:FORM402SRNO
*!*	*!*	&&					If gen_srno_vw.cit_code # item_vw.it_code And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO") &&Commented by sandeep for  bug-21327 on 08-02-2014
*!*	*!*						If gen_srno_vw.cit_code # item_vw.it_code And !Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO","FORM403SRNO") &&added by sandeep for  bug-21327 on 08-02-2014
*!*	*!*							Replace npgno With '' In gen_srno_vw
*!*	*!*						Endif
*!*	*!*	&&Added by Shrikant S. on 06 Feb, 2010 -- Start
*!*	*!*						If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+gen_srno_vw.itserial,'Item_vw','Eddits')
*!*	*!*							If (gen_srno_vw.cit_code = item_vw.it_code And Empty(item_vw.u_pageno))
*!*	*!*								Replace npgno With '' In gen_srno_vw
*!*	*!*							Else
*!*	*!*								Replace npgno With item_vw.u_pageno In gen_srno_vw		&& Added by Shrikant S. on 29/09/2010 for TKT-4021
*!*	*!*							Endif
*!*	*!*						Endif
*!*	*!*	&&Added by Shrikant S. on 06 Feb, 2010 -- Start
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--Start
*!*	*!*	*!*						If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO")
*!*	*!*	*					If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO")		&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 :Modified with below one:FORM402SRNO
*!*	*!*	&&					If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO") &&Commented by sandeep for  bug-21327 on 08-02-2014
*!*	*!*						If	Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2","PLASRNO","SERVTXSRNO","FORM402SRNO","FORM403SRNO")	&&added by sandeep for  bug-21327 on 08-02-2014

*!*	*!*	*!*							If	(Type('main_vw.u_rg23no')='C' Or Type('main_vw.u_rg23cno')='C')
*!*	*!*							If	(Type('main_vw.u_rg23no')='C' Or Type('main_vw.u_rg23cno')='C') And Inlist(gen_srno_vw.ctype,"RGAPART2","RGCPART2")	&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								If	Empty(main_vw.u_rg23no) And Empty(main_vw.u_rg23cno)
*!*	*!*									Replace npgno With '' In gen_srno_vw
*!*	*!*								Endif
*!*	*!*							Endif
*!*	*!*	*!*							If	Type('main_vw.u_plasr')='C' And !(Type('main_vw.u_rg23no')='C' Or Type('main_vw.u_rg23cno')='C')
*!*	*!*							If	Type('main_vw.u_plasr')='C' And !(Type('main_vw.u_rg23no')='C' Or Type('main_vw.u_rg23cno')='C') And gen_srno_vw.ctype='PLASRNO'	&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								If Empty(main_vw.u_plasr)
*!*	*!*									Replace npgno With '' In gen_srno_vw
*!*	*!*								Endif
*!*	*!*							Endif
*!*	*!*	***** Added by Sachin N. S. on 18/09/2012 for Bug-5164 ***** Start
*!*	*!*							If	Type('main_vw.SERVTXSRNO')='C'	And gen_srno_vw.ctype='SERVTXSRNO'
*!*	*!*								If Empty(main_vw.SERVTXSRNO)
*!*	*!*									Replace npgno With '' In gen_srno_vw
*!*	*!*								Endif
*!*	*!*							Endif
*!*	*!*	***** Added by Sachin N. S. on 18/09/2012 for Bug-5164 ***** End
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 : Start:
*!*	*!*							If	Type('main_vw.U_402srno')='C' And gen_srno_vw.ctype="FORM402SRNO"
*!*	*!*								If Empty(main_vw.U_402srno)
*!*	*!*									Replace npgno With '' In gen_srno_vw
*!*	*!*								Endif
*!*	*!*							Else
*!*	*!*								If	Type('lmc_vw.U_402srno')='C' And gen_srno_vw.ctype="FORM402SRNO"
*!*	*!*									If Empty(lmc_vw.U_402srno)
*!*	*!*										Replace npgno With '' In gen_srno_vw
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*							Endif
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 : End:
*!*	*!*	&&added by sandeep for  bug-21327 on 08-02-2014	-->Start
*!*	*!*							If	Type('main_vw.U_403srno')='C' And gen_srno_vw.ctype="FORM403SRNO"
*!*	*!*								If Empty(main_vw.U_403srno)
*!*	*!*									Replace npgno With '' In gen_srno_vw
*!*	*!*								Endif
*!*	*!*							Else
*!*	*!*								If	Type('lmc_vw.U_403srno')='C' And gen_srno_vw.ctype="FORM403SRNO"
*!*	*!*									If Empty(lmc_vw.U_403srno)
*!*	*!*										Replace npgno With '' In gen_srno_vw
*!*	*!*									Endif
*!*	*!*								Endif
*!*	*!*							Endif
*!*	*!*	&&added by sandeep for  bug-21327 on 08-02-2014	<--End


*!*	*!*						Endif
*!*	*!*					Endif
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End
*!*	*!*					If etsql_con > 0 And !Empty(gen_srno_vw.npgno)
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--Start
*!*	*!*						Do Case
*!*	*!*						Case gen_srno_vw.ctype="RGAPART2"
*!*	*!*							Do While (dup_no("U_rg23no",gen_srno_vw.npgno,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)
*!*	*!*								newsrno=gen_no("U_rg23no","Stkl_vw_Main",.T.,"_curvouobj.nHandle")
*!*	*!*								Replace npgno With newsrno In gen_srno_vw
*!*	*!*								Replace u_rg23no With newsrno In main_vw
*!*	*!*							Enddo
*!*	*!*						Case gen_srno_vw.ctype="RGCPART2"
*!*	*!*	*!*							Do While (dup_no("U_rg23cno",main_vw.u_rg23cno,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)
*!*	*!*							Do While (dup_no("U_rg23cno",gen_srno_vw.npgno,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)	&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								newsrno=gen_no("U_rg23cno","Stkl_vw_Main",.T.,"_curvouobj.nHandle")
*!*	*!*								Replace npgno With newsrno In gen_srno_vw
*!*	*!*								Replace u_rg23cno With newsrno In main_vw
*!*	*!*							Enddo
*!*	*!*	*					Case gen_srno_vw.ctype="RGPART1"
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 :Modified with below one:FORM402SRNO
*!*	*!*						Case Inlist(gen_srno_vw.ctype,"RGPART1","RGPAGE")
*!*	*!*							Do While (chk_pageno(.T.,"_curvouobj.nHandle")=.F.)
*!*	*!*								Replace u_pageno With '' In item_vw
*!*	*!*								newpgno=gen_pageno()
*!*	*!*								If Betw(mrecno,1,Reccount())
*!*	*!*									Go mrecno
*!*	*!*									Replace npgno With newpgno In gen_srno_vw
*!*	*!*									Replace u_pageno With newpgno In item_vw
*!*	*!*								Endif
*!*	*!*							Enddo
*!*	*!*						Case gen_srno_vw.ctype="PLASRNO"
*!*	*!*	*!*							Do While (dup_no("u_plasr",main_vw.u_plasr,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)
*!*	*!*							Do While (dup_no("u_plasr",gen_srno_vw.npgno,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)		&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								newsrno=gen_no("u_plasr","Stkl_vw_Main",.T.,"_curvouobj.nHandle")
*!*	*!*								Replace npgno With newsrno In gen_srno_vw
*!*	*!*								Replace u_plasr With newsrno In main_vw
*!*	*!*							Enddo
*!*	*!*	***** Added by Sachin N. S. on 18/09/2012 for Bug-5164 ***** Start
*!*	*!*						Case gen_srno_vw.ctype="SERVTXSRNO"
*!*	*!*							Do While (Dup_ServTxSrNo("SERVTXSRNO",gen_srno_vw.npgno,gen_srno_vw.itserial,gen_srno_vw.SerTy,.T.,"_curvouobj.nHandle")=.F.)		&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								newsrno=Gen_ServTxSrno("SERVTXSRNO",.T.,"_curvouobj.nHandle")
*!*	*!*								Replace npgno With newsrno In gen_srno_vw
*!*	*!*								Replace SERVTXSRNO With newsrno In main_vw
*!*	*!*							Enddo
*!*	*!*	***** Added by Sachin N. S. on 18/09/2012 for Bug-5164 ***** End
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 : Start:
*!*	*!*						Case gen_srno_vw.ctype="FORM402SRNO"
*!*	*!*							Do While (dup_no("u_402srno",gen_srno_vw.npgno,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)		&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								newsrno=gen_no("u_402srno","Stkl_vw_Main",.T.,"_curvouobj.nHandle")
*!*	*!*								Replace npgno With newsrno In gen_srno_vw
*!*	*!*								Replace U_402srno With newsrno In main_vw
*!*	*!*							Enddo
*!*	*!*	*Birendra: Bug-19986 on 11/10/2013 : End:
*!*	*!*	&&added by sandeep for  bug-21327 on 08-02-2014	-->Start
*!*	*!*						Case gen_srno_vw.ctype="FORM403SRNO"
*!*	*!*							Do While (dup_no("u_403srno",gen_srno_vw.npgno,"Stkl_vw_Main",.T.,"_curvouobj.nHandle")=.F.)		&& Changed by Sachin N. S. on 18/09/2012 for Bug-5164
*!*	*!*								newsrno=gen_no("u_403srno","Stkl_vw_Main",.T.,"_curvouobj.nHandle")
*!*	*!*								Replace npgno With newsrno In gen_srno_vw
*!*	*!*								Replace U_403srno With newsrno In main_vw
*!*	*!*							Enddo
*!*	*!*	&&added by sandeep for  bug-21327 on 08-02-2014	<--End


*!*	*!*						Endcase
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End
*!*	*!*						etsql_str  = _curvouobj.sqlconobj.geninsert("Gen_SrNo","","","Gen_SrNo_Vw",mvu_backend)
*!*	*!*						etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
*!*	*!*							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*					Endif
*!*	*!*					Select gen_srno_vw
*!*	*!*				Endscan
*!*	*!*			Endif

*!*	*!*		Else
*!*	*!*			etsql_str  = "Select Top 1 Tran_cd from Gen_SrNo Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
*!*	*!*			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
*!*	*!*				"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*			If etsql_con > 0 And Used('TmpEt_Vw')
*!*	*!*				Select tmpet_vw
*!*	*!*				If Reccount() > 0
*!*	*!*					etsql_str = _curvouobj.sqlconobj.gendelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
*!*	*!*							Entry_ty = ?Main_vw.Entry_ty")
*!*	*!*					etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
*!*	*!*						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*				Endif
*!*	*!*			Endif
*!*	*!*		Endif

*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--Start
*!*	*!*		If(!Inlist(main_vw.Rule,"NON-MODVATABLE","ANNEXURE V") Or Inlist(main_vw.entry_ty,"RR","RP","GI","GR","HI","HR"))
*!*	*!*			If	(Type('Main_vw.U_rg23no')='C' Or Type('Main_vw.U_rg23cno')='C')
*!*	*!*				Do Case
*!*	*!*				Case !Empty(main_vw.u_rg23no)
*!*	*!*					etsql_str  = "Update "+_curvouobj.entry_tbl+"Main set U_rg23no='"+main_vw.u_rg23no+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_str  = etsql_str +" "+"Update "+_curvouobj.entry_tbl+"Acdet set U_rg23no='"+main_vw.u_rg23no+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_tpageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*				Case !Empty(main_vw.u_rg23cno)
*!*	*!*					etsql_str  = "Update "+_curvouobj.entry_tbl+"Main set U_rg23Cno='"+main_vw.u_rg23cno+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_str  = etsql_str +" "+"Update "+_curvouobj.entry_tbl+"Acdet set U_rg23Cno='"+main_vw.u_rg23cno+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_tpageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*				Endcase
*!*	*!*				If Used('_tpageno')
*!*	*!*					Use In _tpageno
*!*	*!*				Endif
*!*	*!*			Endif
*!*	*!*			If Type('Main_vw.u_plasr')='C'
*!*	*!*				If !Empty(main_vw.u_plasr)
*!*	*!*					etsql_str  = "Update "+_curvouobj.entry_tbl+"Main set U_Plasr='"+main_vw.u_plasr+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_str  = etsql_str +" "+"Update "+_curvouobj.entry_tbl+"Acdet set U_Plasr='"+main_vw.u_plasr+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_tpageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*					If Used('_tpageno')
*!*	*!*						Use In _tpageno
*!*	*!*					Endif
*!*	*!*				Endif
*!*	*!*			Endif

*!*	*!*	****** Added by Sachin N. S. on 18/09/2012 for Bug-5164 ****** Start
*!*	*!*			If Type('Main_vw.SERVTXSRNO')='C'
*!*	*!*	&&			If !Empty(main_vw.SERVTXSRNO)  &commented by sandeep for the bug-13869 on 20-MAy-2013
*!*	*!*				If !Empty(Nvl((main_vw.SERVTXSRNO),'')) && added by sandeep for bug-13869 on 20-MAy-2013
*!*	*!*					etsql_str  = "Update "+_curvouobj.entry_tbl+"Main set SERVTXSRNO='"+main_vw.SERVTXSRNO+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_str  = etsql_str +" "+"Update "+_curvouobj.entry_tbl+"Acdet set SERVTXSRNO='"+main_vw.SERVTXSRNO+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
*!*	*!*					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_tpageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*					If Used('_tpageno')
*!*	*!*						Use In _tpageno
*!*	*!*					Endif
*!*	*!*				Endif
*!*	*!*			Endif
*!*	*!*	****** Added by Sachin N. S. on 18/09/2012 for Bug-5164 ****** End

*!*	*!*			Select item_vw

*!*	*!*			Scan
*!*	*!*				If Type('Item_vw.u_pageno')='C'
*!*	*!*					If(!Empty(item_vw.u_pageno))
*!*	*!*						etsql_str  = "Update "+_curvouobj.entry_tbl+"Item set u_pageno='"+item_vw.u_pageno+"' Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and Itserial=?Item_vw.Itserial"
*!*	*!*						etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_temppageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*	*!*						If Used('_temppageno')
*!*	*!*							Use In _temppageno
*!*	*!*						Endif
*!*	*!*					Endif
*!*	*!*				Endif
*!*	*!*				Select item_vw
*!*	*!*			Endscan
*!*	*!*		Endif
*!*	*!*	&& Added by Shrikant S. on 29/09/2010 for TKT-4021		--End

*!*	*!*		If Used('TmpEt_Vw')
*!*	*!*			Use In tmpet_vw
*!*	*!*		Endif
*!*	*!*	Endif
&& Commented by Shrikant S. on 29/09/2016 for GST		&& End

&&-->Ipop(Rup)

_malias 	= Alias()
_mrecno	= Recno()

If ([vuexc] $ vchkprod)
	If(_curvouobj.addmode=.T. Or _curvouobj.editmode=.T.)
		If Type('_curvouobj.PCVTYPE')='C'
			*			If (Inlist(_curvouobj.pcvtype,'IP','ST','OP','DC')) And Used('projectitref_vw')
			*Birendra Bug-4543 on 31/07/2012 : Commented and modified with Below one:
			*!*				If (Inlist(_curvouobj.pcvtype,'IP','ST','OP','DC','WI','WO')) And Used('projectitref_vw')		&& Commented by Shrikant S. on 23/06/2017 for GST Bug-28289
			*!*				If (Inlist(_curvouobj.pcvtype,'IP','ST','OP','DC','WI','WO','RN')) And Used('projectitref_vw')			&& Added by Shrikant S. on 23/06/2017 for GST Bug-28289  &&Commented by Priyanka B on 14012020 for Bug-33160
			If (Inlist(_curvouobj.pcvtype,'IP','ST','OP','DC','WI','WO','RN','II','LI','SS','I5','IL','NG','RU','WI')) And Used('projectitref_vw')			&& Added by Shrikant S. on 23/06/2017 for GST Bug-28289  &&Modified by Priyanka B on 14012020 for Bug-33160
				Set DataSession To _Screen.ActiveForm.DataSessionId
				If (_curvouobj.editmode) &&Delete existing record from projectitref
					etsql_str  = "delete  from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_delBom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If Used('_delBom')
						Use In _delbom
					Endif
				Endif

				Select projectitref_vw
				If Reccount()>0 &&Insert records into projectitref
					Replace All tran_cd With main_vw.tran_cd In projectitref_vw
					Scan
						msqlstr  =  _curvouobj.sqlconobj.geninsert ("projectitref","'SERIALNO'","","projectitref_vw",mvu_backend)
						etsql_con1  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					Endscan
				Endif
				*Birendra : Bug-8378 on 24/01/2013:Commented:Start:
				*!*					IF ! oglblprdfeat.udchkprod('AutoTran') &&Birendra for Bug-4426 on 08/06/2012	&& Commented By Shrikant S. on 29/12/2012 for Bug-2267
				*!*					If ! oglblprdfeat.udchkprod('AutoTran') Or  ! oglblprdfeat.udchkprod('stkresrvtn')	&& Added By Shrikant S. on 29/12/2012 for Bug-2267
				*!*						If Used('projectitref_vw')			&& Added By Shrikant S. on 18/01/2013 for Bug-8377
				*!*							Use In projectitref_vw
				*!*						Endif								&& Added By Shrikant S. on 18/01/2013 for Bug-8377
				*!*					Endif
				*Birendra : Bug-8378 on 24/01/2013:Commented:End:

				&& Commented by Shrikant S. on 27/12/2017 for Bug-31082		&& Start
				*!*	*!*	&& Added by Shrikant S. on 19/07/2016 for Bug-28289			&& Start
				*!*	*!*					If Inlist(_curvouobj.pcvtype,'MR') And Used('Itref_vw')
				*!*	*!*						If (_curvouobj.editmode) 		&&Delete existing record from projectitref
				*!*	*!*							etsql_str  = "delete  from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				*!*	*!*							etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_delBom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				*!*	*!*							If Used('_delBom')
				*!*	*!*								Use In _delbom
				*!*	*!*							Endif
				*!*	*!*						Endif

				*!*	*!*						Select Itref_vw
				*!*	*!*						Scan
				*!*	*!*							msqlstr ="Execute Usp_Ent_GetWkDetails ?Itref_vw.REntry_ty, ?Itref_vw.Itref_tran, ?Itref_vw.RItserial , ?Itref_vw.It_code"

				*!*	*!*							etsql_con1  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[tmppitref],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				*!*	*!*							If Reccount('tmppitref')>0
				*!*	*!*								Select tmppitref
				*!*	*!*								Locate
				*!*	*!*								Replace entry_ty With Itref_vw.entry_ty, tran_cd With Itref_vw.tran_cd,itserial With Itref_vw.itserial,Qty With (Itref_vw.RQty * (-1)) In tmppitref

				*!*	*!*								msqlstr  =  _curvouobj.sqlconobj.geninsert ("projectitref","'SERIALNO'","","tmppitref",mvu_backend)
				*!*	*!*								etsql_con1  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],;
				*!*	*!*									"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

				*!*	*!*								Use In 	tmppitref
				*!*	*!*							Endif
				*!*	*!*							Select Itref_vw
				*!*	*!*						Endscan
				*!*	*!*					Endif
				*!*	*!*	&& Added by Shrikant S. on 19/07/2016 for Bug-28289			&& End
				&& Commented by Shrikant S. on 27/12/2017 for Bug-31082		&& End

			Endif
			&& Added by Shrikant S. on 27/12/2017 for Bug-31082			&& Start
			If Inlist(_curvouobj.pcvtype,'MR') And Used('Itref_vw')
				If (_curvouobj.editmode) 		&&Delete existing record from projectitref
					etsql_str  = "delete  from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_delBom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If Used('_delBom')
						Use In _delbom
					Endif
				Endif

				Select Itref_vw
				Scan
					msqlstr ="Execute Usp_Ent_GetWkDetails ?Itref_vw.REntry_ty, ?Itref_vw.Itref_tran, ?Itref_vw.RItserial , ?Itref_vw.It_code"

					etsql_con1  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[tmppitref],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If Reccount('tmppitref')>0
						Select tmppitref
						Locate
						Replace entry_ty With Itref_vw.entry_ty, tran_cd With Itref_vw.tran_cd,itserial With Itref_vw.itserial,Qty With (Itref_vw.RQty * (-1)) In tmppitref

						msqlstr  =  _curvouobj.sqlconobj.geninsert ("projectitref","'SERIALNO'","","tmppitref",mvu_backend)
						etsql_con1  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

						Use In 	tmppitref
					Endif
					Select Itref_vw
				Endscan
			Endif
			&& Added by Shrikant S. on 27/12/2017 for Bug-31082			&& End

		Endif
		&& Added By Kishor A. for Bug-26932 on 10/09/2015 Start
		If Upper(Alltrim(main_vw.party_nm)) = 'CANCELLED'
			etsql_str  = "delete  from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
			etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_delBom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If Used('_delBom')
				Use In _delbom
			Endif
		Endif
		&& Added By Kishor A. for Bug-26932 on 10/09/2015 End

	Else &&Delete Button
		If  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) And Type('_curvouobj.PCVTYPE')='C' &&Check Existing Records
			etsql_str  = "select top 1 entry_ty from projectitref Where aTran_cd = ?Main_vw.Tran_cd And aEntry_ty = ?Main_vw.Entry_ty"
			etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_chkbom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If Used('_chkbom')
				If Reccount()>0
					Select _chkbom
					=Messagebox("Entry Passed Against /"+_chkbom.entry_ty+" Entry Can not be Deleted",16,vumess)
					Use In _chkbom
					Return .F.
				Endif
				Use In _chkbom
			Endif
		Endif
		etsql_str  = "delete  from projectitref Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
		etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_delBom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If Used('_delBom')
			Use In _delbom
		Endif
	Endif
Endif


If !Empty(_malias)
	Select &_malias
Endif
If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif

If _curvouobj.addmode = .T. Or _curvouobj.editmode = .T.
	If Used('_uploadcursor')
		If Reccount('_uploadcursor') > 0
			objupload =Createobject("Udyog.iTAX.FileUpload.Any.Format")
			ServerName=mvu_server

			*!*				username  =_curvouobj.sqlconobj.dec(_curvouobj.sqlconobj.ondecrypt(mvu_user))		&& Commented by Shrikant S. on 12/05/2018 for installer 1.0.0
			*!*				spassword =_curvouobj.sqlconobj.dec(_curvouobj.sqlconobj.ondecrypt(mvu_pass))		&& Commented by Shrikant S. on 12/05/2018 for installer 1.0.0

			&& Added by Shrikant S. on 26/04/2018 for Bug-31488		&& Start
			If mvu_nenc=1
				username  =_curvouobj.sqlconobj.newdecry(Strconv(mvu_user,16),"Ud*yog+1993Uid")
				spassword =_curvouobj.sqlconobj.newdecry(Strconv(mvu_pass,16),"Ud*yog+1993Pwd")
			Else
				username  = _curvouobj.sqlconobj.dec(_curvouobj.sqlconobj.ondecrypt(mvu_user))
				spassword = _curvouobj.sqlconobj.dec(_curvouobj.sqlconobj.ondecrypt(mvu_pass))
			Endif
			&& Added by Shrikant S. on 26/04/2018 for Bug-31488		&& End


			connectionstring = "Data Source="+ServerName+";Initial Catalog="+company.dbname+";User ID="+username+";password="+spassword

			Select _uploadcursor
			Replace All tr_id With main_vw.tran_cd In _uploadcursor
			Scan &&While !Eof()
				If _uploadcursor.pwhat = "U" Or _uploadcursor.pwhat = "I"
					If Empty(_uploadcursor.objsave)
						objupload.saveimage(_uploadcursor.tr_type,_uploadcursor.tr_id,_uploadcursor.tr_serial,_uploadcursor.filename,_uploadcursor.extension,_uploadcursor.objpath,connectionstring,_uploadcursor.pwhat,.F.,Null,_uploadcursor.tr_itserial)
					Else
						objupload.saveimage(_uploadcursor.tr_type,_uploadcursor.tr_id,_uploadcursor.tr_serial,_uploadcursor.filename,_uploadcursor.extension,_uploadcursor.objsave,connectionstring,_uploadcursor.pwhat,.T.,Alltrim(_uploadcursor.objsource),_uploadcursor.tr_itserial)
					Endif
				Else
					*********** Added By Sachin N. S. on 23/09/2010 for TKT-3982 *********** Start
					*!*						If _uploadcursor.pWhat = "D"
					*!*							objUpload.DeleteImage(_uploadcursor.tr_type,_uploadcursor.TR_ID,_uploadcursor.tr_serial,ConnectionString,_uploadcursor.DatabaseSave,_uploadcursor.tr_itSerial)		&& Changed By Sachin N. S. on 23/09/2010 for TKT-3982
					*!*						Endif
					*********** Added By Sachin N. S. on 23/09/2010 for TKT-3982 *********** End
					*Birendra : For Bug-661 on 10/11/2011:Start: (Due to above code asking deleted item confirmation)
					If _uploadcursor.pwhat = "D"
						msqlstr = "delete from uploadfiles where tr_type = '" + main_vw.entry_ty + "' and tr_id = " + Str(main_vw.tran_cd)
						msqlstr = msqlstr+" and tr_serial= "+Str(_uploadcursor.tr_serial) + " and tr_itserial = '" + _uploadcursor.tr_itserial+"'"
						nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						If nretval<=0
							Return .F.
						Endif
					Endif
					*Birendra : For Bug-661 on 10/11/2011:End:

					*!*						objUpload.DeleteImage(_uploadcursor.tr_type,_uploadcursor.TR_ID,_uploadcursor.tr_serial,ConnectionString,Iif(Empty(_uploadcursor.objSave),.F.,.T.),_uploadcursor.tr_itSerial)	&& Commented By Sachin N. S. on 23/09/2010 for TKT-3982

				Endif
				Select _uploadcursor
			Endscan
			objupload.connectionclose()
			objupload.dispose()
		Endif
		Use In _uploadcursor &&Birendra: Bug-661 on 10/12/2011
	Endif
Else
	If _curvouobj.addmode = .F. And _curvouobj.editmode = .F.
		msqlstr = "delete from uploadfiles where tr_type = '" + main_vw.entry_ty + "' and tr_id = " + Str(main_vw.tran_cd)
		nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
			"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If nretval<=0
			Return .F.
		Endif

	Endif
Endif

******** Added By Sachin N. S. on 20/12/2010 for New Installer ******** Start
*Birendra: Bug-19986 on 22/10/2013 : Start::Commented:
*!*	If _curvouobj.itempage	And ([vutex] $ vchkprod) And etsql_con > 0
*!*		If _curvouobj.addmode Or _curvouobj.editmode
*!*			If Used('Gen_SrNo_Vw')
*!*				Select gen_srno_vw
*!*				Replace All tran_cd With main_vw.tran_cd,entry_ty With main_vw.entry_ty,;
*!*					DATE With main_vw.Date,compid With main_vw.compid,l_yn With main_vw.l_yn In gen_srno_vw
*!*	*npgno,cit_code,cware,cgroup,cchapno
*!*	*itserial

*!*				etsql_str  = "Select * from Gen_SrNo Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
*!*				etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
*!*					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*				If etsql_con > 0 And Used('TmpEt_Vw')
*!*					Select tmpet_vw
*!*					Scan
*!*						metdele = .F.
*!*						Select item_vw
*!*						If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmpet_vw.itserial,'Item_vw','Eddits')
*!*							metdele = .T.
*!*						Else
*!*							If tmpet_vw.cit_code # item_vw.it_code &&Or TmpEt_Vw.Cware # Item_vw.Ware_nm
*!*								metdele = .T.
*!*							Endif
*!*						Endif
*!*						If metdele = .T. And etsql_con > 0
*!*							etsql_str = _curvouobj.sqlconobj.gendelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
*!*									Entry_ty = ?Main_vw.Entry_ty And Itserial = ?TmpEt_Vw.itserial")
*!*							etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
*!*								"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*						Else
*!*							Select gen_srno_vw
*!*							If Seek(tmpet_vw.itserial,'Gen_SrNo_Vw','ItSerial') And ;
*!*									gen_srno_vw.cit_code = tmpet_vw.cit_code And  ;
*!*									gen_srno_vw.cgroup = tmpet_vw.cgroup And gen_srno_vw.cchapno = tmpet_vw.cchapno		&&Gen_SrNo_Vw.Cware = TmpEt_Vw.Cware AND
*!*								Replace npgno With '' In gen_srno_vw
*!*							Endif
*!*						Endif
*!*						Select tmpet_vw
*!*					Endscan
*!*				Else
*!*					etsql_con = 0
*!*				Endif

*!*				Select gen_srno_vw
*!*				Scan
*!*					Select item_vw
*!*					If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+gen_srno_vw.itserial,'Item_vw','Eddits')
*!*						Replace npgno With '' In gen_srno_vw
*!*					Else
*!*						If gen_srno_vw.cit_code # item_vw.it_code &&Or Gen_SrNo_Vw.Cware # Item_vw.Ware_nm
*!*							Replace npgno With '' In gen_srno_vw
*!*						Endif
*!*					Endif
*!*					If etsql_con > 0 And !Empty(gen_srno_vw.npgno)
*!*						etsql_str  = _curvouobj.sqlconobj.geninsert("Gen_SrNo","","","Gen_SrNo_Vw",mvu_backend)
*!*						etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
*!*							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*					Endif
*!*					Select gen_srno_vw
*!*				Endscan
*!*			Endif
*!*		Else
*!*			etsql_str  = "Select Top 1 Tran_cd from Gen_SrNo Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
*!*			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[TmpEt_Vw],;
*!*				"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*			If etsql_con > 0 And Used('TmpEt_Vw')
*!*				Select tmpet_vw
*!*				If Reccount() > 0
*!*					etsql_str = _curvouobj.sqlconobj.gendelete("Gen_SrNo","Tran_cd = ?Main_vw.Tran_cd And ;
*!*							Entry_ty = ?Main_vw.Entry_ty")
*!*					etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
*!*						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*				Endif
*!*			Endif
*!*		Endif
*!*		If Used('TmpEt_Vw')
*!*			Use In tmpet_vw
*!*		Endif
*!*	Endif
******** Added By Sachin N. S. on 20/12/2010 for New Installer ******** End
*Birendra: Bug-19986 on 22/10/2013 : End::Commented:

&& Rup--->12/08/2009
If Used('Gen_SrNo_Vw')
	Use In gen_srno_vw
Endif
&&<---Rup 12/08/2009

*!*	IF _curvouobj.Addmode = .t. OR _curvouobj.EditMode = .t.
*!*		IF USED('_uploadcursor')
*!*			IF RECCOUNT('_uploadcursor') > 0
*!*				objUpload =CREATEOBJECT("Udyog.iTAX.FileUpload.Any.Format")
*!*				ServerName=mvu_server
*!*				UserName  =_curvouobj.SqlConObj.dec(_curvouobj.sqlconObj.ondecrypt(mvu_user))
*!*				SPassword =_curvouobj.SqlConObj.dec(_curvouobj.sqlconObj.ondecrypt(mvu_Pass))
*!*				ConnectionString = "Data Source="+ServerName+";Initial Catalog="+Company.dbname+";User ID="+UserName+";password="+SPassWord

*!*				SELECT _uploadcursor
*!*				GO top
*!*				SCAN WHILE !EOF()
*!*					IF _uploadcursor.pWhat = "U" OR _uploadcursor.pWhat = "I"
*!*						IF EMPTY(_uploadcursor.objSave)
*!*							objUpload.SaveImage(_uploadcursor.tr_type,_uploadcursor.tr_id,_uploadcursor.tr_serial,_uploadcursor.filename,_uploadcursor.Extension,_uploadcursor.objPath,ConnectionString,_uploadcursor.pWhat,.f.,null)
*!*						ELSE
*!*							objUpload.SaveImage(_uploadcursor.tr_type,_uploadcursor.tr_id,_uploadcursor.tr_serial,_uploadcursor.filename,_uploadcursor.Extension,_uploadcursor.objSave,ConnectionString,_uploadcursor.pWhat,.t.,ALLTRIM(_uploadcursor.objSource))
*!*						ENDIF
*!*					ENDIF
*!*				ENDSCAN
*!*				objUpload.ConnectionClose()
*!*				objUpload.Dispose()
*!*			ENDIF
*!*		ENDIF
*!*	ELSE
*!*		IF _curvouobj.addmode = .f. AND _curvouobj.editmode = .f.
*!*			mSqlStr = "delete from uploadfiles where tr_type = '" + main_vw.entry_ty + "' and tr_id = " + STR(main_vw.tran_cd)
*!*			nRetval = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,mSqlstr,"",;
*!*								"_curvouobj.nHandle",_curvouobj.DataSessionId,.t.)
*!*			IF nretVal<=0
*!*				RETURN .f.
*!*			ENDIF

*!*		ENDIF
*!*	ENDIF

******By Shrikant S. on  27/01/2011 ---Start for TKT-5814
If _curvouobj.addmode = .T. Or _curvouobj.editmode = .T.
	If Used('ItRef_vw')
		Select Itref_vw
		Scan
			&&--->Commented by Rup 18/04/2011 TKT-6627,6628,6829
			*!*				etsql_str = "Select Top 1 Tran_cd from  "+ItRef_vw.REntry_ty+"item where Tran_cd = ?ItRef_vw.Itref_tran And Itserial = ?ItRef_vw.Ritserial  and re_qty>qty"
			*!*				etsql_con1  = _curvouobj.SqlConObj.DataConn([EXE],Company.DbName,etsql_str,;
			*!*					[tmpreqty_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

			*!*				If etsql_con1 > 0 And Used('tmpreqty_vw')
			*!*					Select tmpreqty_vw
			*!*					If Reccount()>0
			*!*						sql_errmess = Chr(13)+" Unable to Pickup. Since it is already picked."+Chr(13)
			*!*						Return .F.
			*!*					Endif
			*!*					If Used('tmpreqty_vw')
			*!*						Select tmpreqty_vw
			*!*						Use In tmpreqty_vw
			*!*					Endif
			*!*				Endif
			&&<---Commented by Rup 18/04/2011 TKT-6627,6628,6829
			&&--->Added by Rup 18/04/2011 TKT-6627,6628,6829
			etsql_str = "select bcode_nm=case when ext_vou=1 then bcode_nm else entry_ty end,uom_desc  from lcode where entry_ty='"+Itref_vw.rentry_ty+"'"
			etsql_con1  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,;
				"llcode","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			etsql_str = "Select  * from  "+llcode.bcode_nm+"item where Tran_cd = ?ItRef_vw.Itref_tran And Itserial = ?ItRef_vw.Ritserial "
			etsql_con1  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,;
				[tmpreqty_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			mansln =0
			Select llcode
			If !Empty(llcode.uom_desc) And !Empty(lcode_vw.uom_desc)
				_muom_desc2 = llcode.uom_desc
				For i1 = 1 To company.uom_no
					If Type('_curvouobj.multi_qty(i1,1)') = 'C'
						vq= Iif(At(';',_muom_desc2)	> 0,Substr(_muom_desc2,1,At(';',_muom_desc2)-1),_muom_desc2)
						vqty= Iif(At(':',vq) > 0,Substr(vq,1,At(':',vq)-1),vq)
						v2 = Iif(At(':',vq) > 0,Substr(vq,At(':',vq)+1),'')
						vsoft = Substr(v2 ,1,3)
						vhard = Substr(v2 ,5,3)
						If vsoft=".f." And vhard =".f."
							vhard =".t."
						Endif
						x="tmpreqty_vw.re_"+vqty+">tmpreqty_vw."+vqty
						If &x
							vx=0
							x="tmpreqty_vw."+vqty
							vx=&x
							mmess =vqty+" Value Exceeds Balance Value for "+Alltrim(Str(vx))
							If 	Lower(vsoft) ='.t.'
								If _curvouobj.showmessagebox(mmess+Chr(13)+'Proceed ?',4,vumess,1) # 6		&&vasant051209
									sql_errmess = Chr(13)+" Unable to Pickup. Since it is already picked."+Chr(13)
									Return .F.
								Endif
							Else
								If 	Lower(vhard) ='.t.'
									sql_errmess = Chr(13)+" Unable to Pickup. Since it is already picked."+Chr(13)
									Return .F.
								Endif
							Endif
						Endif
					Endi
				Endfor
			Else
				If etsql_con1 > 0 And Used('tmpreqty_vw')
					Select tmpreqty_vw
					If Reccount()>0
						If tmpreqty_vw.re_qty>tmpreqty_vw.Qty
							sql_errmess = Chr(13)+" Unable to Pickup. Since it is already picked."+Chr(13)
							Return .F.
						Endif
					Endif
					If Used('tmpreqty_vw')
						Select tmpreqty_vw
						Use In tmpreqty_vw
					Endif
				Endif
			Endif
			Select Itref_vw
			&&<---Added by Rup 18/04/2011 TKT-6627,6628,6829
		Endscan
	Endif
Endif
******By Shrikant S. on  27/01/2011 ---End

If 'trnamend' $ vchkprod
	Do voufinalupdate In mainprg &&Birendra :(TKT-2386) 22 Mar 2011 for Order Amendment
Endif

******** Added By Sachin N. S. on 13/02/2012 for TKT-9411 And BUG-660 Batchwise/Serialize Inventory ******** Start
If _curvouobj.itempage
	If Type('_curvouobj._BatchSerialStk')='O'
		If etsql_con>0
			etsql_con=_curvouobj._batchserialstk._uetrigvoufinalupdate()
			&& added by Shrikant S. on 11/07/2015 for Bug-25878			&& Start
			If etsql_con <=0
				Return .F.
			Endif
			&& added by Shrikant S. on 11/07/2015 for Bug-25878			&& End
		Endif
	Endif
Endif
******** Added By Sachin N. S. on 13/02/2012 for TKT-9411 And BUG-660 Batchwise/Serialize Inventory ******** End

**** Added by Shrikant S. on 09/08/2019 for Bug-32412			&& Start
If Type('_curvouobj._unitbcserialize')='O'
	If etsql_con >0
		etsql_con = _curvouobj._unitbcserialize._uetrigvoufinalupdate()
		If etsql_con <=0
			Return .F.
		Endif
	Endif
Endif
**** Added by Shrikant S. on 09/08/2019 for Bug-32412			&& End

*!*	** Start : Added by Uday on 26/12/2011 for Exim
_mexim = .F.                              && Added by Ajay Jaiswal on 22/02/2012 for EXIM
_mdbk = .F.                               && Added by Ajay Jaiswal on 03/04/2012 for DBK
_mexim = oglblprdfeat.udchkprod('exim')   && Added by Ajay Jaiswal on 22/02/2012 for EXIM
_mdbk = oglblprdfeat.udchkprod('dbk')     && Added by Ajay Jaiswal on 03/04/2012 for DBK
If _mexim  Or _mdbk                                 && Added by Ajay Jaiswal on 21/02/2012 for EXIM & DBK
	If _curvouobj.pcvtype = "OP"
		If _curvouobj.addmode = .T. Or _curvouobj.editmode = .T.
			If Used("OP_Pttax_vw")
				Update op_pttax_vw Set tran_cd = main_vw.tran_cd ;
					WHERE tran_cd = 0

				msqlstr = "delete from op_pttaxdet where entry_ty = '" + main_vw.entry_ty + "' and tran_cd = " + Str(main_vw.tran_cd)
				nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If nretval<=0
					Return .F.
				Endif
				Go Top In op_pttax_vw
				Store "" To msqlstr
				Do While !Eof("Op_Pttax_vw")
					msqlstr = _curvouobj.sqlconobj.geninsert("op_pttaxdet","'item'","","op_pttax_vw",mvu_backend)

					nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If nretval<=0
						Return .F.
					Endif

					Skip In op_pttax_vw
				Enddo

			Endif
		Else
			If Used("OP_Pttax_vw")
				msqlstr = "delete from op_pttaxdet where entry_ty = '" + main_vw.entry_ty + "' and tran_cd = " + Str(main_vw.tran_cd)
				nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If nretval<=0
					_curvouobj.showmessagebox("Error while Delete Record from Op_PTtaxDet table",32,vumess)
					Return .F.
				Endif

				malias = Alias()
				Select op_pttax_vw
				Delete All
				Select (malias)

				With _curvouobj.voupage.pgappduties.grdappduties
					.Refresh()
				Endwith

			Endif
		Endif
	Endif
Endif
*!*	** End : Added by Uday on 26/12/2011 for Exim

*!*	** Start : Added by Uday on 29/04/2014 for Bug-21965		&& Bug-30904
_mMLA = .F.
_mMLA = oglblprdfeat.udchkprod('mulvlappr')
If _mMLA = .T.
	If lcode_vw.apgenps = .T.
		_currObj= _Screen.ActiveForm

		Store .T. To isSave
		Do Case
			Case _currObj.addmode = .F. And _currObj.editmode = .F.
				isSave = ProcMLADelete()  && Multi level approval records get deleted
			Case _currObj.addmode = .T. Or _currObj.editmode = .T.
				isSave = ProcMLASave()  && Multi level approval records get generated
		Endcase

		If isSave = .F. &&AND isCheck = .T.
			Return .F.
		Endif
	Endif
Endif

*!*	** End : Added by Uday on 29/04/2014 for Bug-21965			&& Bug-30904


** 	Added By Shrikant S. on 31/05/2012 for Bug-3248		&& Start
If _curvouobj.addmode = .F. And _curvouobj.editmode = .F.
	If Inlist(main_vw.entry_ty,"J2","J3")
		msqlstr = "delete from SertaxCrRef where rentry_ty = '" + main_vw.entry_ty + "' and rtran_cd = " + Str(main_vw.tran_cd)
		nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
			"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If nretval<=0
			Return .F.
		Endif
	Endif
Endif
** 	Added By Shrikant S. on 31/05/2012 for Bug-3248		&& End
** 	Bug-4885 Rup--->
If Inli(main_vw.entry_ty,"PP","TH","FH","EH","RH")
	If _curvouobj.addmode = .F. And _curvouobj.editmode = .F.
		msqlstr = "Update Emp_Monthly_Payroll Set "+main_vw.entry_ty+"_Ent_Ty='',"+main_vw.entry_ty+"_Trn_Cd=0 where "+main_vw.entry_ty+"_Ent_Ty='"+main_vw.entry_ty+"' and "+main_vw.entry_ty+"_Trn_Cd=" + Str(main_vw.tran_cd)
		nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
			"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If nretval<=0
			Return .F.
		Endif
	Else
		If Used("PayRoll_vw")
			Select PayRoll_vw
			Go Top
			Scan For sel
				msqlstr = "Update Emp_Monthly_Payroll Set "+main_vw.entry_ty+"_Ent_Ty='"+main_vw.entry_ty+"',"+main_vw.entry_ty+"_Trn_Cd=" + Alltrim(Str(main_vw.tran_cd))+" Where Pay_Year='"+Alltrim(PayRoll_vw.Pay_Year)+"' and Pay_Month="+Alltrim(Str(PayRoll_vw.Pay_Month))+" and EmployeeCode='"+Alltrim(PayRoll_vw.EmployeeCode)+"'"
				nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"",;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If nretval<=0
					Return .F.
				Endif
			Endscan
			Use In PayRoll_vw
		Endif
	Endif
Endif
**<---Bug-4885 Rup

&&Added By Shrikant S. on 29/12/2012 for Bug-2267 		&& Start &&vasant030412
If _curvouobj.addmode = .T. Or _curvouobj.editmode = .T.
	_mstkresrvtn = .F.
	_mstkresrvtn = oglblprdfeat.udchkprod('stkresrvtn')
	If _mstkresrvtn = .T.
		If etsql_con > 0
			etsql_str  = "Delete from StkResrvDet Where Entry_ty = ?Main_vw.Entry_ty and Tran_cd = ?Main_vw.Tran_cd"
			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
				"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

			etsql_str  = "Delete from StkResrvDet Where REntry_ty = ?Main_vw.Entry_ty and RTran_cd = ?Main_vw.Tran_cd"
			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
				"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		Endif

		*!*			If (lcode_vw.entry_ty ='OP' Or lcode_vw.bcode_nm ='OP') 							&& Commented by Shrikant S. on 18/01/2013 for Bug-8377
		If (lcode_vw.entry_ty ='OP' Or lcode_vw.bcode_nm ='OP') And Used('projectitref_vw')		&& Added by Shrikant S. on 18/01/2013 for Bug-8377
			Select projectitref_vw
			Scan
				If etsql_con > 0
					_mQty1 = projectitref_vw.Qty
					_mQty2 = 0
					_mAllocQty = 0

					etsql_str  = "Select SUM(Qty) As Qty from ProjectItRef Where AEntry_ty = ?ProjectItRef_vw.AEntry_ty and ATran_cd = ?ProjectItRef_vw.ATran_cd and AItSerial = ?ProjectItRef_vw.AItSerial"
					etsql_str  = etsql_str + " and Entry_ty = 'OP' "
					etsql_str  = etsql_str + " and Entry_ty + CAST(Tran_cd as varchar(10)) + ItSerial != ?ProjectItRef_vw.Entry_ty + CAST(?ProjectItRef_vw.Tran_cd as varchar(10)) + ?ProjectItRef_vw.ItSerial "
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[chktbl_vw],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con > 0 And Used('chktbl_vw')
						_mQty2 = Iif(Isnull(chktbl_vw.Qty)=.F.,chktbl_vw.Qty,0)
					Endif

					etsql_str  = "Select * from StkResrvDet Where Entry_ty = ?ProjectItRef_vw.AEntry_ty and Tran_cd = ?ProjectItRef_vw.ATran_cd and ItSerial = ?ProjectItRef_vw.AItSerial"
					etsql_str  = etsql_str + " And REntry_ty = 'SO'"
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[chktbl_vw],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con > 0 And Used('chktbl_vw')
						Select chktbl_vw
						Scan
							If _mQty2 > chktbl_vw.AllocQty
								Replace AllocQty With 0 In chktbl_vw
								_mQty2 = _mQty2 - chktbl_vw.AllocQty
							Else
								Replace AllocQty With AllocQty - _mQty2 In chktbl_vw
								_mQty2 = 0
							Endif
							If _mQty2 = 0
								If _mQty1 > chktbl_vw.AllocQty
									_mAllocQty = chktbl_vw.AllocQty
									_mQty1 = _mQty1 - chktbl_vw.AllocQty
								Else
									_mAllocQty = _mQty1
									Replace AllocQty With _mQty1 In chktbl_vw
									_mQty1 = 0
								Endif
								If _mAllocQty > 0
									etsql_str  = "Insert Into StkResrvDet (Entry_ty,Tran_cd,ItSerial,It_code,AllocQty,REntry_ty,RTran_cd,RItSerial,RCTran_cd)"
									etsql_str  = etsql_str + "Values (?ProjectItRef_vw.Entry_ty,?ProjectItRef_vw.Tran_cd,?ProjectItRef_vw.ItSerial,?ProjectItRef_vw.It_code,?_mAllocQty,?chktbl_vw.REntry_ty,?chktbl_vw.RTran_cd,?chktbl_vw.RItSerial,?chktbl_vw.CTran_cd)"
									etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
										"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

									etsql_str  = "Insert Into StkResrvDet (Entry_ty,Tran_cd,ItSerial,It_code,AllocQty,REntry_ty,RTran_cd,RItSerial,RCTran_cd)"
									etsql_str  = etsql_str + "Values (?ProjectItRef_vw.AEntry_ty,?ProjectItRef_vw.ATran_cd,?ProjectItRef_vw.AItSerial,?ProjectItRef_vw.It_code,?_mAllocQty,?ProjectItRef_vw.Entry_ty,?ProjectItRef_vw.Tran_cd,?ProjectItRef_vw.ItSerial,?chktbl_vw.CTran_cd)"
									etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
										"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
								Endif
							Endif
							Select chktbl_vw
						Endscan
					Endif
				Endif
				Select projectitref_vw
			Endscan
		Endif

		If Used('ItRef_vw')
			Select Itref_vw
			Scan
				If etsql_con > 0
					_mQty1 = Itref_vw.RQty
					_mQty2 = 0
					_mAllocQty = 0
					*					etsql_str  = "Select SUM(Qty) As Qty from "+_curvouobj.entry_tbl+"ItRef Where REntry_ty = ?ItRef_vw.REntry_ty and ItRef_Tran = ?ItRef_vw.ItRef_Tran and RItSerial = ?ItRef_vw.RItSerial"
					*Birendra : Bug-8378 on 24/01/2013 :Commented above changes:
					etsql_str  = "Select SUM(RQty) As Qty from "+_curvouobj.entry_tbl+"ItRef Where REntry_ty = ?ItRef_vw.REntry_ty and ItRef_Tran = ?ItRef_vw.ItRef_Tran and RItSerial = ?ItRef_vw.RItSerial"
					etsql_str  = etsql_str + " and Entry_ty + CAST(Tran_cd as varchar(10)) + ItSerial != ?ItRef_vw.Entry_ty + CAST(?ItRef_vw.Tran_cd as varchar(10)) + ?ItRef_vw.ItSerial "
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[chktbl_vw],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con > 0 And Used('chktbl_vw')
						_mQty2 = Iif(Isnull(chktbl_vw.Qty)=.F.,chktbl_vw.Qty,0)
					Endif
					etsql_str  = "Select * from StkResrvDet Where Entry_ty = ?ItRef_vw.REntry_ty and Tran_cd = ?ItRef_vw.ItRef_Tran and ItSerial = ?ItRef_vw.RItSerial"
					etsql_str  = etsql_str + " And REntry_ty = 'SO'"
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[chktbl_vw],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con > 0 And Used('chktbl_vw')
						Select chktbl_vw
						Scan
							If _mQty2 > chktbl_vw.AllocQty
								Replace AllocQty With 0 In chktbl_vw
								_mQty2 = _mQty2 - chktbl_vw.AllocQty
							Else
								Replace AllocQty With AllocQty - _mQty2 In chktbl_vw
								_mQty2 = 0
							Endif
							If _mQty2 = 0
								If _mQty1 > chktbl_vw.AllocQty
									_mAllocQty = chktbl_vw.AllocQty
									_mQty1 = _mQty1 - chktbl_vw.AllocQty
								Else
									_mAllocQty = _mQty1
									Replace AllocQty With _mQty1 In chktbl_vw
									_mQty1 = 0
								Endif
								If _mAllocQty > 0
									etsql_str  = "Insert Into StkResrvDet (Entry_ty,Tran_cd,ItSerial,It_code,AllocQty,REntry_ty,RTran_cd,RItSerial,RCTran_cd)"
									etsql_str  = etsql_str + "Values (?ItRef_vw.Entry_ty,?ItRef_vw.Tran_cd,?ItRef_vw.ItSerial,?ItRef_vw.It_code,?_mAllocQty,?chktbl_vw.REntry_ty,?chktbl_vw.RTran_cd,?chktbl_vw.RItSerial,?chktbl_vw.CTran_cd)"
									etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
										"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

									etsql_str  = "Insert Into StkResrvDet (Entry_ty,Tran_cd,ItSerial,It_code,AllocQty,REntry_ty,RTran_cd,RItSerial,RCTran_cd)"
									etsql_str  = etsql_str + "Values (?ItRef_vw.REntry_ty,?ItRef_vw.ItRef_Tran,?ItRef_vw.RItSerial,?ItRef_vw.It_code,?_mAllocQty,?ItRef_vw.Entry_ty,?ItRef_vw.Tran_cd,?ItRef_vw.ItSerial,?chktbl_vw.CTran_cd)"
									etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
										"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
								Endif
							Endif
							Select chktbl_vw
						Endscan
					Endif
				Endif
				Select Itref_vw
			Endscan
		Endif
	Endif
Endif

&& Added By Shrikant S. on 08/12/2012 for Bug-2267			&& Start
If _curvouobj.addmode = .F. And _curvouobj.editmode = .F.
	_mstkresrvtn = .F.
	_mstkresrvtn = oglblprdfeat.udchkprod('stkresrvtn')
	If _mstkresrvtn = .T.
		If (lcode_vw.entry_ty ='SO' Or lcode_vw.bcode_nm ='SO')
			If etsql_con >0
				etsql_str  = "Delete from Stkresrvsum Where Entry_ty = ?Main_vw.Entry_ty and Tran_cd = ?Main_vw.Tran_cd"
				etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			Endif
		Endif
	Endif
Endif

&& Added By Shrikant S. on 08/12/2012 for Bug-2267			&& End
&&Added By Shrikant S. on 29/12/2012 for Bug-2267 		&& End &&vasant030412
************ Added By Shrikant S. on 30/10/2012	FOR BUG-6867	****************** Start
If !_curvouobj.addmode And !_curvouobj.editmode
	If Vartype(oglblprdfeat)='O'
		If oglblprdfeat.udchkprod('openord')
			If Used("SpDiff_vw")
				etsql_str = _curvouobj.sqlconobj.gendelete("SpDiff","Tran_cd = ?Main_vw.Tran_cd And ;
						Entry_ty = ?Main_vw.Entry_ty")
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con<0
					Return .F.
				Endif
			Endif
			If _curvouobj.pcvtype="OO"
				etsql_str = _curvouobj.sqlconobj.gendelete("Amend_Detail","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty")
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con<0
					Return .F.
				Endif
			Endif
			If _curvouobj.pcvtype="ST"
				etsql_str = _curvouobj.sqlconobj.gendelete("SpDiff","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty")
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con<0
					Return .F.
				Endif
			Endif

		Endif
	Endif
Endif

************ Added By Shrikant S. on 30/10/2012	FOR BUG-6867	****************** Start
&& Added By Shrikant S. on 01/02/2014 for Bug-13926		&& Start
If !_curvouobj.addmode And !_curvouobj.editmode
	If Vartype(oglblprdfeat)='O'
		If oglblprdfeat.udchkprod('mrp')
			etsql_str = "Select [Name] From SysObjects Where Xtype='U' and [Name]='MRPLOG'"
			etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"tmpCur",;
				"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If etsql_con<0
				Return .F.
			Endif
			If Reccount('tmpCur')>0
				etsql_str = _curvouobj.sqlconobj.gendelete("MRPLOG","ItRef_Tran= ?Main_vw.Tran_cd And ;
						REntry_ty = ?Main_vw.Entry_ty")
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con<0
					Return .F.
				Endif
			Endif
			If Used('tmpCur')
				Use In tmpCur
			Endif
		Endif
	Endif
Endif

&& Added By Shrikant S. on 01/02/2014 for Bug-13926		&& End

&& Added by Shrikant S. on 29/09/2014 for Bug-23879		&& Start
If _curvouobj.addmode Or _curvouobj.editmode
	If Vartype(oglblprdfeat)='O'
		If oglblprdfeat.udchkprod('qctrl')
			If _curvouobj.pcvtype='OS'
				Select item_vw
				lnrecno=Iif(!Eof(),Recno(),0)
				Scan
					Replace qcaceptqty With Qty, qcholdqty With 0 In item_vw

					etsql_str  = "Update "+_curvouobj.entry_tbl+"Item set qcaceptqty=qty,qcholdqty=0 Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and Itserial=?Item_vw.Itserial"
					etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

					Select item_vw
				Endscan
				If lnrecno>0
					Select item_vw
					Go lnrecno
				Endif
			Endif
		Endif
	Endif
Else
	If Vartype(oglblprdfeat)='O'
		If oglblprdfeat.udchkprod('qctrl') And Inlist(lcode_vw.inv_stk,'+')
			etsql_str="Select Insp_id From qc_inspection_master Where Entry_ty=?Main_vw.Entry_ty and Tran_cd=?Main_vw.Tran_cd "
			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[qcvw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If etsql_con>0 And Reccount('qcvw')>0
				Select qcvw
				Locate
				etsql_str  = "Delete From qc_inspection_item Where Insp_Id=?qcvw.insp_Id "
				etsql_str  = etsql_str  +" Delete from QC_INSPECTION_PARAMETER Where Insp_Id=?qcvw.insp_Id "
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			Endif
		Endif
	Endif
Endif

&& Added by Shrikant S. on 29/09/2014 for Bug-23879		&& End

&& Added by Shrikant S. on 03/06/2017 for GST		&& Start
If Vartype(oglblindfeat)='O'
	*	If oglblindfeat.udchkind('vugst')
	If _curvouobj.addmode Or _curvouobj.editmode
		If Inlist(_curvouobj.pcvtype,"PT","P1","E1","GC","GD","C6","D6") And Type('main_vw.u_cldt')<>'U'
			Do Case
				Case _curvouobj.pcvtype="P1"
					*!*						Replace u_cldt With pinvdt,tranStatus With 1 In main_vw  &&Commented by Priyanka B on 19062019 for Bug-32621
					Replace u_cldt With Iif(Year(u_cldt)>1900,u_cldt,pinvdt),tranStatus With 1 In main_vw  &&Modified by Priyanka B on 19062019 for Bug-32621
					*!*				Case Inlist(_curvouobj.pcvtype,"PT","E1") And (_curvouobj.taxapplarea="OUT OF COUNTRY" Or _curvouobj.accregistatus="UNREGISTERED")		&& Commented by Shrikant S. on 12/06/2017 for GST
					*!*					Case Inlist(_curvouobj.pcvtype,"PT","E1") And _curvouobj.taxapplarea="OUT OF COUNTRY"			&& Added by Shrikant S. on 12/06/2017 for GST	&& Commented by Shrikant S. on 18/12/2017 for Auto updater 13.0.5
				Case Inlist(_curvouobj.pcvtype,"PT","E1")
					&& Added by Shrikant S. on 12/06/2017 for GST Auto updater 13.0.5		&& Start
					If isunderImport()
						*!*							Replace u_cldt With pinvdt,tranStatus With 1 In main_vw  &&Commented by Priyanka B on 19062019 for Bug-32621
						Replace u_cldt With Iif(Year(u_cldt)>1900,u_cldt,pinvdt),tranStatus With 1 In main_vw  &&Modified by Priyanka B on 19062019 for Bug-32621
					Else
						*!*							Replace u_cldt With pinvdt In main_vw  &&Commented by Priyanka B on 19062019 for Bug-32621
						Replace u_cldt With Iif(Year(u_cldt)>1900,u_cldt,pinvdt) In main_vw  &&Modified by Priyanka B on 19062019 for Bug-32621
					Endif
					&& Added by Shrikant S. on 12/06/2017 for GST Auto updater 13.0.5		&& End

					*!*				Case Inlist(_curvouobj.pcvtype,"C6","D6","GC","GD") And Inlist(main_vw.againstgs,"PURCHASES","SERVICE PURCHASE BILL") And (_curvouobj.taxapplarea="OUT OF COUNTRY" Or _curvouobj.accregistatus="UNREGISTERED")		&& Commented by Shrikant S. on 12/06/2017 for GST
				Case Inlist(_curvouobj.pcvtype,"C6","D6","GC","GD") And Inlist(main_vw.againstgs,"PURCHASES","SERVICE PURCHASE BILL") And _curvouobj.taxapplarea="OUT OF COUNTRY"		&& Added by Shrikant S. on 12/06/2017 for GST
					Replace u_cldt With pinvdt,tranStatus With 1 In main_vw
			Endcase
			If Type('Acdet_vw.u_cldt')<>'U'  &&Added by Priyanka B on 27082020 for Bug-33586
				Replace All AcDet_vw.u_cldt With main_vw.u_cldt In AcDet_vw  &&Added by Priyanka B on 25072020 for Bug-33586
			Endif  &&Added by Priyanka B on 27082020 for Bug-33586

			etsql_str  = "Update "+_curvouobj.entry_tbl+"Main set u_cldt=?main_vw.u_cldt,tranStatus =?main_vw.tranStatus Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
			etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

			&&Added by Priyanka B on 25072020 for Bug-33586 Start
			If etsql_con >0
				Select AcDet_vw
				Go Top
				If Type('Acdet_vw.u_cldt')<>'U'  &&Added by Priyanka B on 27082020 for Bug-33586
					etsql_str  = "Update "+_curvouobj.entry_tbl+"Acdet set u_cldt=?main_vw.u_cldt Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
					etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				Endif  &&Added by Priyanka B on 27082020 for Bug-33586
			Endif
			&&Added by Priyanka B on 25072020 for Bug-33586 End
		Endif
	Endif
	*	Endif
Endif

&& Added by Shrikant S. on 03/06/2017 for GST		&& End


&& Added by Shrikant S. on 23/06/2017 for GST		&& Start

If Inlist(_curvouobj.pcvtype,"P1","PT","ST","S1","E1")
	If !_curvouobj.addmode And  !_curvouobj.editmode
		etsql_str  = "Select TOP 1 Entry_ty from Othitref Where Itref_tran = ?Main_vw.Tran_cd And REntry_ty = ?Main_vw.Entry_ty"
		etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[RecExts],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If Used('RecExts')
			If Reccount()>0
				Select RecExts
				=Messagebox("Entry Passed Against /"+RecExts.entry_ty+" Entry Can not be Deleted",16,vumess)
				Use In RecExts
				Return .F.
			Endif
			Use In RecExts
		Endif
	Endif
Endif

&& Added by Shrikant S. on 23/06/2017 for GST		&& End



&& Added by Shrikant S. on 12/08/2017 for GST		&& Start
If Inlist(_curvouobj.pcvtype,"BP","BR","CP","CR")
	If !_curvouobj.addmode And  !_curvouobj.editmode
		etsql_str  = "Select TOP 1 Entry_ty from BPMAIN Where PaymentNo=?Main_vw.Inv_no and Entry_ty='RV'"
		etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[RecExts],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		If Used('RecExts')
			If Reccount()>0
				Select RecExts
				=Messagebox("Entry Passed Against /"+RecExts.entry_ty+" Entry Can not be Deleted",16,vumess)
				Use In RecExts
				Return .F.
			Endif
			Use In RecExts
		Endif
	Endif
Endif
&& Added by Shrikant S. on 12/08/2017 for GST		&& End


&& Added by Anil on 21052020 for Bug-33469 Start
_curvouobj = _Screen.ActiveForm

If ("efabric" $ vchkprod)

	*!*		If _curvouobj.itempage And Inlist(main_vw.entry_ty,"AF","PY","W1","FO","RD","IC","W2","I6","RD","O6","OC")  && ,"W3","I6","O6","ID","RD")  && Commented by Anil on 30062020 for Bug-33469
	*!*		If _curvouobj.itempage And Inlist(main_vw.entry_ty,"AF","PY","W1","FO","RD","IC","W2","I6","RD","O6","OC","RB","S3","S4","JI","JR")  && Commented by Anil on 08072020 for Bug 333328 && ,"W3","I6","O6","ID","RD")  && Modified by Anil on 30062020 for Bug-33469
	*!*		If _curvouobj.itempage And Inlist(main_vw.entry_ty,"AF","PY","W1","FO","RD","IC","W2","I6","RD","O6","OC","RB","S3","S4","JI","JR","W3","W4","BS") && Commented by Anil on 04082020 for Bug-33328 Au-2.2.5 -31072020 && Modified by Anil on 08072020 for Bug 333328
	If _curvouobj.itempage And Inlist(main_vw.entry_ty,"AF","PY","W1","IC","W2","I6","RD","O6","OC","W3","W4") && Modified by Anil on 04082020 for Bug-33328 Au-2.2.5 -31072020 && Modified by Anil on 08072020 for Bug 333328
		If _curvouobj.addmode Or _curvouobj.editmode
			If Used('BatchTbl_Vw')
				Select BatchTbl_Vw
				Replace All tran_cd With main_vw.tran_cd,entry_ty With main_vw.entry_ty,;
					DATE With main_vw.Date,compid With main_vw.compid,l_yn With main_vw.l_yn In BatchTbl_Vw
				etsql_str  = "Select * from BatchGenTbl Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tmprunno_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con > 0 And Used('tmprunno_vw')
					Select tmprunno_vw
					Scan
						metdele = .F.
						Select item_vw
						If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmprunno_vw.itserial,'Item_vw','Eddits')
							metdele = .T.
						Else
							If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmprunno_vw.itserial,'Item_vw','Eddits')
								If Empty(item_vw.batchno)
									metdele = .T.
								Endif
							Endif
						Endif
						If metdele = .T. And etsql_con > 0
							etsql_str = _curvouobj.sqlconobj.gendelete("BatchGenTbl","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty And Itserial = ?tmprunno_vw.itserial")
							etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						Else
							Select BatchTbl_Vw
							If Seek(tmprunno_vw.itserial,'BatchTbl_Vw','ItSerial')
								Replace batchno With '',runno With '' In BatchTbl_Vw
							Endif
						Endif
						Select tmprunno_vw
					Endscan
				Else
					etsql_con = 0
				Endif

				Select BatchTbl_Vw
				Scan
					mrecno=Recno()
					Select item_vw
					If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+BatchTbl_Vw.itserial,'Item_vw','Eddits')
						Replace batchno With '',runno With '' In BatchTbl_Vw
					Else
						If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+BatchTbl_Vw.itserial,'Item_vw','Eddits')
							If Empty(item_vw.batchno)
								Replace batchno With '',runno With '' In BatchTbl_Vw
							Else
								Replace batchno With item_vw.batchno In BatchTbl_Vw
							Endif
						Endif
					Endif
					If etsql_con > 0 And !Empty(BatchTbl_Vw.runno)
						If coadditional.batchrun   &&Added by Priyanka B on 26072017 for Pharma
							Do While (CheckBatchNo(.T.,"_curvouobj.nHandle")=.F.)
								Replace batchno With '' In item_vw
								newbatchno=GenBatchNo()
								If Betw(mrecno,1,Reccount())
									Go mrecno
									If _item.IsRunning=.T.
										If Alltrim(_item.BatchSfix)<>""
											_mrunno=Val(Left(Right(Alltrim(newbatchno),6+Len(Alltrim(BatchSfix))),6))
										Else
											_mrunno=Val(Right(Alltrim(newbatchno),6))
										Endif
									Else
										_mrunno=0
									Endif


									Replace batchno With newbatchno,runno With Str(_mrunno) In BatchTbl_Vw
									Replace batchno With newbatchno In item_vw
								Endif
							Enddo
						Endif  &&Added by Priyanka B on 26072017 for Pharma
						etsql_str  = _curvouobj.sqlconobj.geninsert("BatchGenTbl","","","BatchTbl_Vw",mvu_backend)
						etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					Endif
					Select BatchTbl_Vw
				Endscan
				Use In BatchTbl_Vw
			Endif
		Else

			etsql_str  = "Select Top 1 Tran_cd from BatchGenTbl Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tmprunno_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If etsql_con > 0 And Used('tmprunno_vw')
				Select tmprunno_vw
				If Reccount() > 0
					etsql_str = _curvouobj.sqlconobj.gendelete("BatchGenTbl","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty")
					etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				Endif
			Endif
		Endif

		Select item_vw
		Scan
			If Type('Item_vw.batchno')='C'
				If(!Empty(item_vw.batchno))
					etsql_str  = "Update "+_curvouobj.entry_tbl+"Item set batchno=?item_vw.batchno Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and Itserial=?Item_vw.Itserial"
					etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_temppageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If Used('_temppageno')
						Use In _temppageno
					Endif
				Endif
			Endif
			Select item_vw
		Endscan

		If Used('tmprunno_vw')
			Use In tmprunno_vw
		Endif
	Endif

Endif

&& Added by Anil on 21052020 for Bug-33469 End



&& Added by Shrikant S. on 30/06/2014 for Bug-23280		&& Start
If Vartype(oglblindfeat)='O'
	If oglblindfeat.udchkind('pharmaind')
		If _curvouobj.itempage And Inlist(main_vw.entry_ty,"AR","WK","OS")
			If _curvouobj.addmode Or _curvouobj.editmode
				If Used('BatchTbl_Vw')
					Select BatchTbl_Vw
					Replace All tran_cd With main_vw.tran_cd,entry_ty With main_vw.entry_ty,;
						DATE With main_vw.Date,compid With main_vw.compid,l_yn With main_vw.l_yn In BatchTbl_Vw

					etsql_str  = "Select * from BatchGenTbl Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tmprunno_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con > 0 And Used('tmprunno_vw')
						Select tmprunno_vw
						Scan
							metdele = .F.
							Select item_vw
							If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmprunno_vw.itserial,'Item_vw','Eddits')
								metdele = .T.
							Else
								If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmprunno_vw.itserial,'Item_vw','Eddits')
									If Empty(item_vw.batchno)
										metdele = .T.
									Endif
								Endif
							Endif
							If metdele = .T. And etsql_con > 0
								etsql_str = _curvouobj.sqlconobj.gendelete("BatchGenTbl","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty And Itserial = ?tmprunno_vw.itserial")
								etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							Else
								Select BatchTbl_Vw
								If Seek(tmprunno_vw.itserial,'BatchTbl_Vw','ItSerial')
									Replace batchno With '',runno With '' In BatchTbl_Vw
								Endif
							Endif
							Select tmprunno_vw
						Endscan
					Else
						etsql_con = 0
					Endif

					Select BatchTbl_Vw
					Scan
						mrecno=Recno()
						Select item_vw
						If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+BatchTbl_Vw.itserial,'Item_vw','Eddits')
							Replace batchno With '',runno With '' In BatchTbl_Vw
						Else
							If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+BatchTbl_Vw.itserial,'Item_vw','Eddits')
								If Empty(item_vw.batchno)
									Replace batchno With '',runno With '' In BatchTbl_Vw
								Else
									Replace batchno With item_vw.batchno In BatchTbl_Vw
								Endif
							Endif
						Endif
						If etsql_con > 0 And !Empty(BatchTbl_Vw.runno)
							If coadditional.batchrun   &&Added by Priyanka B on 26072017 for Pharma
								Do While (CheckBatchNo(.T.,"_curvouobj.nHandle")=.F.)
									Replace batchno With '' In item_vw
									newbatchno=GenBatchNo()
									If Betw(mrecno,1,Reccount())
										Go mrecno
										If _item.IsRunning=.T.
											If Alltrim(_item.BatchSfix)<>""
												_mrunno=Val(Left(Right(Alltrim(newbatchno),6+Len(Alltrim(BatchSfix))),6))
											Else
												_mrunno=Val(Right(Alltrim(newbatchno),6))
											Endif
										Else
											_mrunno=0
										Endif


										Replace batchno With newbatchno,runno With Str(_mrunno) In BatchTbl_Vw
										Replace batchno With newbatchno In item_vw
									Endif
								Enddo
							Endif  &&Added by Priyanka B on 26072017 for Pharma
							etsql_str  = _curvouobj.sqlconobj.geninsert("BatchGenTbl","","","BatchTbl_Vw",mvu_backend)
							etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						Endif
						Select BatchTbl_Vw
					Endscan
					Use In BatchTbl_Vw
				Endif
			Else
				etsql_str  = "Select Top 1 Tran_cd from BatchGenTbl Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tmprunno_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con > 0 And Used('tmprunno_vw')
					Select tmprunno_vw
					If Reccount() > 0
						etsql_str = _curvouobj.sqlconobj.gendelete("BatchGenTbl","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty")
						etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					Endif
				Endif
			Endif


			Select item_vw
			Scan
				If Type('Item_vw.batchno')='C'
					If(!Empty(item_vw.batchno))
						etsql_str  = "Update "+_curvouobj.entry_tbl+"Item set batchno=?item_vw.batchno Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and Itserial=?Item_vw.Itserial"
						etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_temppageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						If Used('_temppageno')
							Use In _temppageno
						Endif
					Endif
				Endif
				Select item_vw
			Endscan

			If Used('tmprunno_vw')
				Use In tmprunno_vw
			Endif

			If (_curvouobj.pcvtype="WK")
				If (_curvouobj.addmode Or _curvouobj.editmode) And Used("wkrmdet_vw")
					If (_curvouobj.editmode) &&Delete existing record from projectitref
						etsql_str  = "delete from WKRMDET Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
						etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						If etsql_con<=0
							Return .F.
						Endif
					Endif

					Select wkrmdet_vw
					Count For !Deleted() To wkcnt

					If wkcnt>0 &&Insert records into projectitref
						Replace All tran_cd With main_vw.tran_cd In wkrmdet_vw
						Scan
							msqlstr  =  _curvouobj.sqlconobj.geninsert ("WKRMDET","'ID'","","wkrmdet_vw",mvu_backend)
							etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							If etsql_con<=0
								Return .F.
							Endif
						Endscan
						Use In wkrmdet_vw
					Else
						_curvouobj.showmessagebox("Work order cannot save without BOM allocation",32,vumess)
						Return .F.
					Endif
				Else
					etsql_str  = "delete  from WKRMDET Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
					etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If etsql_con<=0
						Return .F.
					Endif
				Endif
			Endif
		Endif

		_malias 	= Alias()
		_mrecno	= Recno()

		If ([vuexc] $ vchkprod)
			If(_curvouobj.addmode=.T. Or _curvouobj.editmode=.T.)
				If Type('_curvouobj.PCVTYPE')='C'
					If (Inlist(_curvouobj.pcvtype,'SS')) And Used('batchalloc_vw')
						Set DataSession To _Screen.ActiveForm.DataSessionId
						If (_curvouobj.editmode) &&Delete existing record from projectitref
							etsql_str  = "delete  from batch_alloc Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
							etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							If etsql_con <=0
								Return .F.
							Endif
						Endif

						Select batchalloc_vw
						If Reccount()>0 &&Insert records into batch_alloc
							Replace All tran_cd With main_vw.tran_cd In batchalloc_vw
							Scan
								msqlstr  =  _curvouobj.sqlconobj.geninsert ("batch_alloc","","","batchalloc_vw",mvu_backend)
								etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							Endscan
						Endif

					Endif
				Endif
			Else &&Delete Button
				etsql_str  = "delete  from batch_alloc Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If etsql_con <=0
					Return .F.
				Endif
			Endif
		Endif

		If ([vuexc] $ vchkprod) And Type('_curvouobj.PCVTYPE')='C'
			If(!_curvouobj.addmode And !_curvouobj.editmode)
				If _curvouobj.pcvtype="AR"
					etsql_str  = "select top 1 entry_ty from wkrmdet Where rTran_cd = ?Main_vw.Tran_cd And rEntry_ty = ?Main_vw.Entry_ty"
					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_chkbom],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If Used('_chkbom')
						If Reccount()>0
							Select _chkbom
							=Messagebox("Entry Passed Against work order. Entry Can not be Deleted",16,vumess)
							Use In _chkbom
							Return .F.
						Endif
						Use In _chkbom
					Endif
				Endif
				*!*				Else
				*!*					If _curvouobj.pcvtype="IP"
				*!*						If !Used("ProjectItRef_vw")
				*!*							=Messagebox("Please select the batch from pickup button of the Item.",16,vumess)
				*!*							etsql_con =0
				*!*						Endif
				*!*						If Used("ProjectItRef_vw")
				*!*							Select item_vw
				*!*							Scan
				*!*								If Type('Item_vw.batchno')='C'
				*!*									If(!Empty(item_vw.batchno))
				*!*										Select projectitref_vw
				*!*										Locate For entry_ty=item_vw.entry_ty And tran_cd=item_vw.tran_cd And itserial=item_vw.itserial
				*!*										If !Found()
				*!*											=Messagebox("Please select the batch from pickup button of the Item."+(item_vw.item),16,vumess)
				*!*											etsql_con =0
				*!*											exit
				*!*										Endi
				*!*									Endif
				*!*								Endif
				*!*								Select item_vw
				*!*							Endscan
				*!*						Endif
				*!*					Endif
			Endif
		Endif

		If !Empty(_malias)
			Select &_malias
		Endif
		If Betw(_mrecno,1,Reccount())
			Go _mrecno
		Endif

	Endif
Else
	&&Added & Commented By Kishor A to resolved whether Container_det tables Exsits Start	&& Shrikant S. on 25/01/2017 for GST
	etsql_str  = "SELECT * FROM SYS.objects WHERE NAME='BatchGenTbl'"
	etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_TblExists],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	Select _TblExists
	If Reccount() >0
		etsql_str  = "delete from BatchGenTbl Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
		etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	Endif
	If Used('_TblExists')
		Use In _TblExists
	Endif
	&&Added & Commented By Kishor A to resolved whether Container_det tables Exsits End			&& Shrikant S. on 25/01/2017 for GST
Endif

&& Added by Shrikant S. on 30/06/2014 for Bug-23280		&& End

&& changes by EBS team on 07/03/14 for Bug-21466,21467,21468 Start
* Changes done as per --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference
* Changes done by EBS Product Team
_mexim = oglblprdfeat.udchkprod('exim')
_currObj = _Screen.ActiveForm
If _mexim
	If main_vw.entry_ty='SI'
		lOldArea = Select()
		Store .T. To isCheck
		Do Case
			Case !Empty(main_vw.lc_no) And Upper(Alltrim(main_vw.party_nm)) = 'CANCELLED'
				isCheck = CancelPartyProc() && Executes When Transaction is Cancelled.
			Case !Empty(main_vw.lc_no) And (_currObj.addmode = .T. Or _currObj.editmode = .T.)
				isCheck = LCBalUpdt_AddEdtProc() && Updates LC Balance Amount in Add or Edit Mode
			Case !Empty(main_vw.lc_no) And (_currObj.addmode = .F. And _currObj.editmode = .F.)
				isCheck = LCBalUpdt_DeleteProc() && Updates LC Balance Amount in Delete Mode
			Case Empty(main_vw.lc_no) And (_currObj.addmode = .F. And _currObj.editmode = .T.)
				isCheck = RemoveLCProc() && Updates LC Balance Amount after Removing LC No.
		Endcase

		If isCheck = .F.
			Messagebox("Issues found in customization " +Chr(13) +;
				"Please refer Customization related this CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference",16,vumess)
		Endif

		If Used("cur_lcno")
			Use In cur_lcno
		Endif
		Wait Clear
		* End --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference


		* Changes done as per --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
		* Date : 08/11/2012
		* Changes done by EBS Product Team


		Wait Window + 'Updating Pre Shipment Info. details, Please wait....' Nowait
		Store .T. To isSave
		Do Case
			Case _currObj.addmode = .T. Or _currObj.editmode = .T.
				isSave = ProcSave()
			Case _currObj.addmode = .F. And _currObj.editmode = .F.
				isSave = ProcDelete()
		Endcase
		Wait Clear

		If isSave = .F.
			Messagebox("Issues found in customization " +Chr(13) +;
				"Please refer Customization related this CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info",16,vumess)
		Endif

		If isSave = .T. And isCheck = .T.
			&&Return .T. &&&& Added By Pankaj B. on 01-04-2015 for Bug-25746
		Else
			Return .F.
		Endif

		Select(lOldArea)
		Wait Clear
		&&Return isCheck && && Commented By Pankaj B. on 01-04-2015 for Bug-25746
		*End --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
	Endif
	* End ------->* Changes done by EBS Product Team
Endif
* End --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference

* EPCG Changes done as per --> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO
* Changes done by EBS Product Team on 05022013
_mepcg = oglblprdfeat.udchkprod('epcg')
_meximaa = oglblprdfeat.udchkprod('exim_aa')
_curscrobj= _Screen.ActiveForm
If _mepcg
	If main_vw.entry_ty = 'P1' And !Empty(Iif(Type("main_vw.licen_no")<>"U",Alltrim(main_vw.licen_no),Alltrim(lmc_vw.licen_no)))
		lOldArea = Select()
		Store .T. To isCheck

		If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
			If main_vw.Date < curepcgmast.issue_dt
				Messagebox("Selected license no. issue date should not be less than transaction date...",16,vumess)
				_curscrobj.EPCGctrlObjName1.Value = ""
				_curscrobj.EPCGctrlObjName1.SetFocus()
				Return .F.
			Endif

			If main_vw.Date > curepcgmast.expiry_dt
				Messagebox("Selected License no. expiry date cannot be Less than Transaction date..!!",16,vumess)
				_curscrobj.EPCGctrlObjName1.Value = ""
				_curscrobj.EPCGctrlObjName1.SetFocus()
				Return .F.
			Endif
			isCheck = EPCGAddEditProc()
		Else
			isCheck = EPCGDeleteProc()
		Endif

		If isCheck = .F.
			Return
		Endif
	Endif
Endif
* End ---> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO

* AA Changes done as per --> PR_AA_00001_Import_Purchase_AA
* Changes done by EBS Product Team on 01012013
If _meximaa
	If Inlist(main_vw.entry_ty,"P1","EI") And !Empty(Iif(Type("main_vw.aalic_no")<>"U",Alltrim(main_vw.aalic_no),Alltrim(lmc_vw.aalic_no)))
		_curscrobj = _Screen.ActiveForm

		*!*		MESSAGEBOX("main_vw.tot_examt = "+STR(main_vw.tot_examt,12,2)+CHR(13)+"_curscrobj.oTranItemDtySaved = "+STR(_curscrobj.oTranItemDtySaved,12,2)+;
		*!*			CHR(13)+"_curscrobj.oTranDtySaved = "+STR(_curscrobj.oTranDtySaved,12,2)+;
		*!*			CHR(13)+"main_vw.aa_duty = "+STR(IIF(TYPE("main_vw.aa_duty")<>"U",main_vw.aa_duty,lmc_vw.aa_duty),12,2))

		*!*		MESSAGEBOX("main_vw.net_amt = "+STR(main_vw.net_amt,12,2)+CHR(13)+"_curscrobj.oTranItemInvAmt = "+STR(_curscrobj.oTranItemInvAmt,12,2)+;
		*!*			CHR(13)+"_curscrobj.oTranTotInvAmt = "+STR(_curscrobj.oTranTotInvAmt,12,2)+;
		*!*			CHR(13)+"main_vw.AA_INVAMT = "+STR(IIF(TYPE("main_vw.AA_INVAMT")<>"U",main_vw.aa_invamt,lmc_vw.aa_invamt),12,2)+;
		*!*			CHR(13)+"_curscrobj.oAAEOInvAmt = "+STR(_curscrobj.oAAEOInvAmt,12,2)+;
		*!*			CHR(13)+"curaamast.act_invamt = "+STR(curaamast.act_invamt,12,2))

		*!*		CHR(13)+"EOInvAmt = "+STR(EOInvAmt,12,2)+
		lOldArea = Select()
		Store .T. To isCheck

		If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
			If main_vw.Date < curaamast.issue_dt
				Messagebox("Selected License No. issue date cannot be greater than transaction date...!!",0+16,vumess)
				_curscrobj.AActrlObjName1.Value = ""
				_curscrobj.AActrlObjName1.SetFocus()
				Return .F.
			Endif

			If main_vw.Date > curaamast.expiry_dt
				Messagebox("Selected License No. expiry date cannot be less than transaction date...!!",0+16,vumess)
				_curscrobj.AActrlObjName1.Value = ""
				_curscrobj.AActrlObjName1.SetFocus()
				Return .F.
			Endif
			isCheck = AAAddEditProc()
		Else
			isCheck = AADeleteProc()
		Endif
		If isCheck = .F.
			Return
		Endif
	Endif
Endif
* End ---> PR_AA_00001_Import_Purchase_AA

&& Added by Shrikant S. on 13/01/2015 for Bug-24872		&& Start
_malias 	= Alias()
_mrecno	= Recno()

If  (([vuexc] $ vchkprod) Or ([vuinv] $ vchkprod)) And Type('_curvouobj.PCVTYPE')='C' &&Check Existing Records
	etsql_str  = "Select Top 1 dbname From Vudyog..Co_mast Where Sta_dt< (Select sta_dt from Vudyog..Co_mast where compid=?Company.CompId) and co_name=?Company.co_name and dbname<>?Company.dbname Order by sta_dt desc"
	etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[chkprevrec],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	If Used('chkprevrec')
		If Reccount('chkprevrec')>0
			Locate
			etsql_str  = "Delete From "+Alltrim(chkprevrec.dbname)+"..newyeartran Where nTran_cd = ?Main_vw.Tran_cd And nEntry_ty = ?Main_vw.Entry_ty"
			etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		Endif
		Use In chkprevrec
	Endif
Endif


If !Empty(_malias)
	Select &_malias
Endif
If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif
&& Added by Shrikant S. on 13/01/2015 for Bug-24872		&& End

&& Added By Pankaj B. on 01-04-2015 for Bug-25746 Start
_malias 	= Alias()
_mrecno	= Recno()

If(_curvouobj.addmode=.T. Or _curvouobj.editmode=.T.)
	If Type('_curvouobj.PCVTYPE')='C'
		If Used('Container_vw')
			*Set DataSession To _curvouobj.DataSessionId
			If (_curvouobj.editmode) &&Delete existing record from udcost_center
				etsql_str  = "delete from Container_det Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			Endif

			Select Container_vw
			If etsql_con > 0 &&Insert records into udcost_center
				Replace All tran_cd With main_vw.tran_cd In Container_vw
				Replace All entry_ty With main_vw.entry_ty In Container_vw
				Scan
					If etsql_con > 0
						etsql_str  = _curvouobj.sqlconobj.geninsert ("Container_det","","","Container_vw","")
						etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						Select Container_vw
					Endif
				Endscan
			Endif
			Use In Container_vw
		Endif
	Endif
Else &&Delete Button
	etsql_str  = "delete from Container_det Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
	etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
Endif

If !Empty(_malias)
	Select &_malias
Endif
If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif
&& Added By Pankaj B. on 01-04-2015 for Bug-25746 End

&& Added By Kishor A. For Bug-26677 On 06/08/2015 Start.
_malias 	= Alias()
_mrecno	= Recno()

If(_curvouobj.addmode=.T. Or _curvouobj.editmode=.T.)
	If Type('_curvouobj.PCVTYPE')='C'
		If Used('PayTermsdet_vw')
			If (_curvouobj.editmode) &&Delete existing record from PayTermsdet
				etsql_str  = "delete from PayTermsdet Where tran_cd = ?Main_vw.tran_cd And Entry_ty = ?Main_vw.Entry_ty"
				etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			Endif

			Select PayTermsdet_vw
			Replace All tran_cd With main_vw.tran_cd,entry_ty With main_vw.entry_ty In PayTermsdet_vw

			Select PayTermsdet_vw
			Scan
				If etsql_con > 0 &&Insert records into PayTermsdet
					etsql_str  = _curvouobj.sqlconobj.geninsert ("PayTermsdet","","","PayTermsdet_vw","")
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				Endif
			Endscan
			Use In PayTermsdet_vw
		Endif
	Endif
Else &&Delete Button
	etsql_str  = "delete from PayTermsdet Where tran_cd = ?Main_vw.tran_cd And Entry_ty = ?Main_vw.Entry_ty"
	etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
Endif

If !Empty(_malias)
	Select &_malias
Endif
If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif

If Used("PayTerms_vw")
	Use In PayTerms_vw
Endif

If Used("PayTermsdet_vw")
	Use In PayTermsdet_vw
Endif

&& Added By Kishor A. For Bug-26677 On 06/08/2015 End.


&& Added by Shrikant S. on 05/10/2016 for GST 		&&Start
_malias 	= Alias()
_mrecno	= Recno()

_curvouobj = _Screen.ActiveForm
If(_curvouobj.addmode=.T. Or _curvouobj.editmode=.T.)
	If Used("GridCurs")
		Select entry_ty,Date,tran_cd,CGST_Act As ACGST,SGST_Act As ASGST,IGST_Act As AIGST,main_vw.entry_ty As Refentry_ty,main_vw.tran_cd As Reftran_cd From GridCurs ;
			INTO Cursor FinalGstCurs Readwrite Where Date #{} && And CGST_Act > 0 Or SGST_Act > 0 Or IGST_Act > 0

		If (_curvouobj.editmode)=.T.
			etsql_str  = "delete  from gstalloc Where RefTran_cd = ?Main_vw.Tran_cd And RefEntry_ty = ?Main_vw.Entry_ty"
			nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If nretval <=0
				Return .F.
			Endif
		Endif

		Select FinalGstCurs
		Go Top
		Scan
			Select FinalGstCurs
			lcSqlStr = _curvouobj.sqlconobj.geninsert([GSTALLOC],[],[],[FinalGstCurs],mvu_backend)
			lcSqlStr="set dateformat dmy "+lcSqlStr
			nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,lcSqlStr,"","thisform.nHandle",_curvouobj.DataSessionId,.T.)
			If nretval<=0 Or Empty(FinalGstCurs.Date)
				=Messagebox("Error in Update Statement. Please retry later",64,vumess)
				nRollback=_curvouobj .sqlconobj._sqlrollback("thisform.nHandle")
				If nRollback<=0
					=Messagebox("Not saved properly. Delete this entry and re-enter",64,vumess)
				Endif
				nretval=Thisform.sqlconobj.sqlconnclose("thisform.nHandle")
				Exit
			Endif
			Select FinalGstCurs
		Endscan
		If Used('FinalGstCurs')
			Use In FinalGstCurs
		Endif
		etsql_con =nretval  &&Added by Priyanka B on 15072017 for Export
	Endif
Else
	etsql_str  = "delete  from gstalloc Where RefTran_cd = ?Main_vw.Tran_cd And RefEntry_ty = ?Main_vw.Entry_ty"
	nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	etsql_con =nretval  &&Added by Priyanka B on 15072017 for Export
Endif
&&etsql_con =nretval  &&Commented by Priyanka B on 15072017 for Export

If !Empty(_malias)
	Select &_malias
Endif
If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif

&& Added by Shrikant S. on 05/10/2016 for GST 		&&End



&& Added by Shrikant S. on 19/06/2017 for GST 		&&Start
_malias 	= Alias()
_mrecno	= Recno()

_curvouobj = _Screen.ActiveForm
If _curvouobj.pcvtype='UB'
	If(_curvouobj.addmode=.T. Or _curvouobj.editmode=.T.)
		If Used("rcmdet_vw")

			If Reccount('rcmdet_vw') <=0
				_curvouobj.showmessagebox("Self Invoice cannot save without Unregistered Dealer bills allocation.",32,vumess)
				Return .F.
			Endif

			etsql_str  = "delete  from rcmdetail Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
			nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)


			Select rcmdet_vw
			Replace All tran_cd With main_vw.tran_cd,entry_ty With main_vw.entry_ty In rcmdet_vw
			Scan
				lcSqlStr = _curvouobj.sqlconobj.geninsert([rcmdetail],[],[],[rcmdet_vw],mvu_backend)
				nretval = _curvouobj.sqlconobj.dataconn("EXE",company.dbname,lcSqlStr,"","thisform.nHandle",_curvouobj.DataSessionId,.T.)
				If nretval <=0
					Return .F.
				Endif
				Select rcmdet_vw
			Endscan
			If Used('rcmdet_vw')
				Use In rcmdet_vw
			Endif
		Else
			_curvouobj.showmessagebox("Self Invoice cannot save without Unregistered Dealer bills allocation.",32,vumess)
			Return .F.
		Endif
	Else
		etsql_str  = "delete  from rcmdetail Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
		nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	Endif
	etsql_con =nretval
Endif
If !Empty(_malias)
	Select &_malias
Endif
If Betw(_mrecno,1,Reccount())
	Go _mrecno
Endif

If _curvouobj.pcvtype='GB'
	If Used('DEBIT_VW')
		Use In DEBIT_VW
	Endif
	If Used('GSTPaym_vw')
		Use In GSTPaym_vw
	Endif
	If Used('GSTPay_vw1')
		Use In GSTPay_vw1
	Endif
	If Used('GSTPay_vw2')
		Use In GSTPay_vw2
	Endif
Endif
&& Added by Shrikant S. on 19/06/2017 for GST 		&&End


&& Added by Kishor A. for Bug-28461 on 02-09-2016 Start


If _curvouobj.addmode Or _curvouobj.editmode
	_malias 	= Alias()
	_mrecno	= Recno()
	If _curvouobj.itempage And Inlist(main_vw.entry_ty,"AR","OS","PT","OP","ES")
		If Vartype(oglblindfeat)='O'					&& Added by Rupesh G on 27/07/2018 for Bug-31626
			If !oglblindfeat.udchkind('pharmaind')
				If oglblprdfeat.udchkprod('exmfgbp')
					If Used('BatchTbl_Vw')
						Select BatchTbl_Vw
						Replace All tran_cd With main_vw.tran_cd,entry_ty With main_vw.entry_ty,;
							DATE With main_vw.Date,compid With main_vw.compid,l_yn With main_vw.l_yn In BatchTbl_Vw

						etsql_str  = "Select * from BatchGenTbl Where l_yn = ?main_vw.l_yn and Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
						etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[tmprunno_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						If etsql_con > 0 And Used('tmprunno_vw')
							Select tmprunno_vw
							Scan
								metdele = .F.
								Select item_vw
								If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmprunno_vw.itserial,'Item_vw','Eddits')
									metdele = .T.
								Else
									If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+tmprunno_vw.itserial,'Item_vw','Eddits')
										If Empty(item_vw.batchno)
											metdele = .T.
										Endif
									Endif
								Endif
								If metdele = .T. And etsql_con > 0
									etsql_str = _curvouobj.sqlconobj.gendelete("BatchGenTbl","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty And Itserial = ?tmprunno_vw.itserial")
									etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
								Else
									Select BatchTbl_Vw
									If Seek(tmprunno_vw.itserial,'BatchTbl_Vw','ItSerial')
										Replace batchno With '',runno With '' In BatchTbl_Vw
									Endif
								Endif
								Select tmprunno_vw
							Endscan
						Else
							etsql_con = 0
						Endif

						Select BatchTbl_Vw
						Scan
							mrecno=Recno()
							Select item_vw
							If !Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+BatchTbl_Vw.itserial,'Item_vw','Eddits')
								Replace batchno With '',runno With '' In BatchTbl_Vw
							Else
								If Seek(main_vw.entry_ty+Dtos(main_vw.Date)+main_vw.doc_no+BatchTbl_Vw.itserial,'Item_vw','Eddits')
									If Empty(item_vw.batchno)
										Replace batchno With '',runno With '' In BatchTbl_Vw
									Else
										Replace batchno With item_vw.batchno In BatchTbl_Vw
									Endif
								Endif
							Endif
							If etsql_con > 0 And !Empty(BatchTbl_Vw.runno)
								*!*								Do While (CheckBatchNo(.T.,"_curvouobj.nHandle",_curvouobj)=.F.)		&& Commented by Shrikant S.on 17/05/2017 for GST
								Do While (CheckBatchNo(.T.,"_curvouobj.nHandle")=.F.)			&& Added by Shrikant S.on 17/05/2017 for GST
									Replace batchno With '' In item_vw
									newbatchno=GenBatchNo()
									If Betw(mrecno,1,Reccount())
										Go mrecno
										If _item.IsRunning=.T.
											If Alltrim(_item.BatchSfix)<>""
												_mrunno=Val(Left(Right(Alltrim(newbatchno),6+Len(Alltrim(BatchSfix))),6))
											Else
												_mrunno=Val(Right(Alltrim(newbatchno),6))
											Endif
										Else
											_mrunno=0
										Endif
										Replace batchno With newbatchno,runno With Str(_mrunno) In BatchTbl_Vw
										Replace batchno With newbatchno In item_vw
									Endif
								Enddo
								etsql_str  = _curvouobj.sqlconobj.geninsert("BatchGenTbl","","","BatchTbl_Vw",mvu_backend)
								etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							Endif
							Select BatchTbl_Vw
						Endscan
						*!*						Use In BatchTbl_Vw

						etsql_str  = "SELECT * FROM BatchGenTbl Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
						etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_TblBatchGen],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						Select	_TblBatchGen
						Index On itserial+Str(it_code) Tag _ItserCode

						Select * From item_vw Into Cursor tmpItemvw_Vw
						Index On itserial+Str(it_code) Tag _ItserCode

						Select _TblBatchGen
						Scan
							Select _TblBatchGen
							lcitserial= itserial
							lcTran_cd=tran_cd
							lcEntry_ty=entry_ty
							lcIt_Code=Str(it_code)
							lcBatchNo=batchno
							Select tmpItemvw_Vw
							If Seek(lcitserial+lcIt_Code)
								*!*								etsql_str  = "Update batchgentbl set batchno=?tmpItemvw_Vw.batchno Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and Itserial=?Item_vw.Itserial and IT_code=?Item_vw.It_Code"		&& Commented by Shrikant S. on 17/05/2017 for GST
								etsql_str  = "Update "+_curvouobj.entry_tbl+"Item set batchno=?tmpItemvw_Vw.batchno Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and Itserial=?Item_vw.Itserial and IT_code=?Item_vw.It_Code"			&& Added by Shrikant S. on 17/05/2017 for GST
								etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_temppageno],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							Endif
							Select _TblBatchGen
						Endscan

						If Used('tmpItemvw_Vw')
							Use In tmpItemvw_Vw
						Endif
						If Used('_TblBatchGen')
							Use In _TblBatchGen
						Endif
						If Used('_TblExists')
							Use In _TblExists
						Endif
					Else
						etsql_str  = "SELECT * FROM SYS.objects WHERE NAME='BatchGenTbl'"
						etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_TblExists],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						Select _TblExists
						If Reccount() >0
							etsql_str  = "SELECT top 1 * FROM BatchGenTbl Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
							etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_TblBatchGen],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							Select	_TblBatchGen
							Index On itserial+Str(it_code) Tag _ItserCode

							&& Added by Shrikant S. on 17/05/2017 for GST		&& Start
							If etsql_con1 > 0 And Used('_TblBatchGen')
								Select _TblBatchGen
								If Reccount() > 0
									etsql_str = _curvouobj.sqlconobj.gendelete("BatchGenTbl","Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty")
									etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
								Endif
							Endif
							&& Added by Shrikant S. on 17/05/2017 for GST		&& End

							&& Commented by Shrikant S. on 17/05/2017 for GST 	&& Start
							*!*							Select * From item_vw Into Cursor tmpItemvw_Vw
							*!*							Index On itserial+Str(it_code) Tag _ItserCode
							*!*							Select _TblBatchGen
							*!*							Scan
							*!*								Select _TblBatchGen
							*!*								lcitserial= itserial
							*!*								lcTran_cd=tran_cd
							*!*								lcEntry_ty=entry_ty
							*!*								lcIt_Code=Str(it_code)
							*!*								Select tmpItemvw_Vw
							*!*								If !Seek(lcitserial+lcIt_Code)
							*!*									etsql_str  = "delete from BatchGenTbl Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty And ItSerial= ?lcitserial"
							*!*									etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							*!*								Endif
							*!*								Select _TblBatchGen
							*!*							Endscan
							&& Commented by Shrikant S. on 17/05/2017 for GST 	&& End

							If Used('tmpItemvw_Vw')
								Use In tmpItemvw_Vw
							Endif
							If Used('_TblBatchGen')
								Use In _TblBatchGen
							Endif
							If Used('_TblExists')
								Use In _TblExists
							Endif

						Endif
					Endif
				Endif
			Endif
		Endif		&& Added by Rupesh G on 27/07/2018 for Bug-31626
	Endif
	If !Empty(_malias)
		Select &_malias
	Endif
	If Betw(_mrecno,1,Reccount())
		Go _mrecno
	Endif
	If Used('BatchTbl_Vw')
		Use In BatchTbl_Vw
	Endif

Else
	etsql_str  = "SELECT * FROM SYS.objects WHERE NAME='BatchGenTbl'"
	etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[_TblExists],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	Select _TblExists
	If Reccount() >0
		etsql_str  = "delete from BatchGenTbl Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty"
		etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	Endif
	If Used('_TblExists')
		Use In _TblExists
	Endif
Endif
&& Added by Kishor A. for Bug-28461 on 02-09-2016 End

&&Added by Priyanka B on 06072017 for Self Invoice GST Start
_curvouobj = _Screen.ActiveForm
If Vartype(oglblprdfeat)='O'
	If oglblprdfeat.udchkprod('vugst')
		msql_str = " "
		msql_str1 = " "
		mFnd=.F.
		If (_curvouobj.addmode=.F. And _curvouobj.editmode=.F.)
			msql_str="Select * from Rcmdetail Where pentry_ty=" + Chr(39) + Alltrim(main_vw.entry_ty) + Chr(39) + " and ptran_cd=" + Alltrim(Str(main_vw.tran_cd))

			If !Empty(msql_str)
				etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msql_str,"_chkentry","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If Used("_chkentry")
					If Reccount("_chkentry")>0
						Select _chkentry
						=Messagebox("Entry Passed Against Self Invoice. Entry cannot be Deleted",16,vumess)
						Use In _chkentry
						Return .F.
					Endif
					Use In _chkentry
				Endif
			Endif
		Else
			If (_curvouobj.editmode=.T.)
				msql_str1="Select * from Rcmdetail Where pentry_ty=" + Chr(39) + Alltrim(main_vw.entry_ty) + Chr(39) + " and ptran_cd=" + Alltrim(Str(main_vw.tran_cd))
				*!*				+ " and pitserial=" + Chr(39) + Alltrim(item_vw.itserial) + Chr(39)
				If !Empty(msql_str1)
					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msql_str1,"_chkdtl","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If Reccount("_chkdtl")>0
						Select item_vw
						Go Top
						Scan
							Select _chkdtl
							Scan For !Deleted()
								If (Alltrim(_chkdtl.pentry_ty)+Alltrim(Str(_chkdtl.ptran_cd))+Alltrim(_chkdtl.pitserial)) <> (Alltrim(item_vw.entry_ty)+Alltrim(Str(item_vw.tran_cd))+Alltrim(item_vw.itserial))
									mFnd=.T.
									Exit
								Endif
							Endscan
						Endscan
						If mFnd
							If Used("_chkdtl")
								If Reccount("_chkdtl")>0
									Select _chkdtl
									=Messagebox("Entry Passed Against Self Invoice. Entry cannot be Modified",16,vumess)
									Use In _chkdtl
									Return .F.
								Endif
								Use In _chkdtl
							Endif
						Endif
					Endif
				Endif
			Endif
		Endif
	Endif
Endif
&&Added by Priyanka B on 06072017 for Self Invoice GST End

&& Commented by Shrikant S. on 05/01/2018 for Bug-30971		&& Start
*!*	&&Added by Shrikant S. on 28/08/2017 for GST 		&& Start
*!*	_curvouobj = _Screen.ActiveForm
*!*	If _curvouobj.addmode=.T.  And _curvouobj.pcvtype="D6"
*!*		If main_vw.againstgs="SERVICE PURCHASE BILL"
*!*			If Used('OthItRef_vw')
*!*				Select OthItRef_vw
*!*				Scan
*!*					msql_str="Select Top 1 tran_cd from mainall_vw Where Entry_all=" + Chr(39) + Alltrim(OthItRef_vw.rentry_ty) + Chr(39) + " and Main_tran=" + Alltrim(Str(OthItRef_vw.Itref_tran))+" And Entry_ty='GB'"
*!*					etsql_con1 = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msql_str,"_chkentry","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*					If Used("_chkentry")
*!*						If Reccount("_chkentry")>0
*!*							Select _chkentry
*!*							=Messagebox("Entry Passed Against GST Payment. Entry cannot be Saved.",16,vumess)
*!*							Use In _chkentry
*!*							Return .F.
*!*						Endif
*!*						Use In _chkentry
*!*					Endif
*!*				Endscan
*!*			Endif
*!*		Endif
*!*	Endif
*!*	&&Added by Shrikant S. on 28/08/2017 for GST 		&& End
&& Commented by Shrikant S. on 05/01/2018 for Bug-30971		&& End

_curvouobj = _Screen.ActiveForm
*!*	If "vugst" $ vchkprod  &&Commented by Priyanka B on 09042019 for Bug-32067
If "vugst" $ vchkprod Or "vuisd" $ vchkprod Or "isdkgen" $ vchkprod   &&Modified by Priyanka B on 09042019 for Bug-32067
	If _curvouobj.itempage && And Inlist(main_vw.entry_ty,"PT","E1")		&& Commented by Shrikant S. on 22/08/2017 for GST
		If Type('item_Vw.ecredit')<>'U'  And Type('Item_vw.Linerule')<>'U' And (_curvouobj.addmode=.T. Or _curvouobj.editmode=.T.)
			creditval=0
			Select item_vw
			Locate
			Replace All  ecredit With .T. For Linerule='Taxable' In item_vw &&FOR (item_vw.CGST_AMT+item_vw.SGST_AMT+item_vw.IGST_AMT)>0
			etsql_str  = "Update "+_curvouobj.entry_tbl+"ITEM set ecredit = case when Linerule='Taxable' then 1 else 0 end Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty  "				&& AND (CGST_AMT+SGST_AMT+IGST_AMT)>0
			etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			*!*				Scan
			*!*					creditval=creditval+item_vw.CGST_AMT+item_vw.SGST_AMT+item_vw.IGST_AMT
			*!*					If creditval >0
			*!*						Replace ecredit With .T. In main_vw
			*!*
			*!*					Endif
			*!*				Endscan
		Endif
	Endif
Endif

&& Added by Sachin N. S. on 30/10/2017 for Bug-30782 -- Start
*!*	If oglblprdfeat.udchkprod('vugst')     &&Commented by Priyanka B on 19032019 for Bug-32067
If oglblprdfeat.udchkprod('vugst') Or oglblprdfeat.udchkprod('vuisd') Or oglblprdfeat.udchkprod('isdkgen')   &&Modified by Priyanka B on 19032019 for Bug-32067
	If _curvouobj.editmode=.T.
		If etsql_con > 0
			cdAmendDt = Iif(Type('Main_vw.AmendDate')='T','Main_vw.AmendDate',Iif(Type('Lmc_vw.AmendDate')='T','Lmc_vw.AmendDate',Iif(Type('MainAdd_vw.AmendDate')='T','MainAdd_vw.AmendDate','')))
			If !Empty(cdAmendDt)
				If !Empty(Evaluate(cdAmendDt)) And Ttod(Evaluate(cdAmendDt))!=Ctod('01/01/1900')
					msqlstr = "Select ISNULL(MAX(AmendNo),0)+1 as AmendNo from "+_curvouobj.entry_tbl+"MainAm where Entry_ty = '"+main_vw.entry_ty+"' and Tran_cd = "+Transform(main_vw.tran_cd)
					etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[_curTemp],;
						"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					nAmendNo = _curTemp.AmendNo
					If Used('MAINAmend_VW')
						If Type('MainAmend_vw.AmendNo')='N'
							Update MainAmend_vw Set AmendNo = nAmendNo
							Select MainAmend_vw
							Go Top
						Endif
						msqlstr  = _curvouobj.sqlconobj.geninsert(_curvouobj.entry_tbl+'MainAm',"","","MAINAmend_VW",mvu_backend)
						etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],;
							"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					Endif
					If Used('LMCAmend_VW')
						If etsql_con > 0
							If Type('LMCAmend_VW.AmendNo')='N'
								Update LMCAmend_VW Set AmendNo = nAmendNo
								Select LMCAmend_VW
								Go Top
							Endif

							etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,[Select ]+_curvouobj.LmcFldsList+[ From ]+_curvouobj.entry_tbl+'MainAm'+[ where ;
						Tran_cd = ?Main_vw.Tran_cd],[tmptbl_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							If etsql_con > 0 And Used('tmptbl_vw')
								_curvouobj.ChkNUpdate('tmptbl_vw','LMCAmend_VW',_curvouobj.entry_tbl+'MainAm','Tran_cd = ?Main_vw.Tran_cd')
							Endif
						Endif
					Endif
					If Used('MAINADDAmend_VW')
						If etsql_con > 0
							If Type('MAINADDAmend_VW.AmendNo')='N'
								Update MAINADDAmend_VW Set AmendNo = nAmendNo
								Select MAINADDAmend_VW
								Go Top
							Endif
							&& Added by Suraj Kumawat on 22-12-2017 for Bug-31091  Start..
							Select MAINADDAmend_VW
							Update MAINADDAmend_VW Set entry_ty =main_vw.entry_ty,tran_cd = main_vw.tran_cd
							Go Top
							&& Added by Suraj Kumawat on 22-12-2017 for Bug-31091  end..
							msqlstr  = _curvouobj.sqlconobj.geninsert(_curvouobj.entry_tbl+'MainAddAm',"","","MAINADDAmend_VW",mvu_backend)
							etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],;
								"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						Endif
					Endif
					If Used('ITEMAmend_VW')
						If etsql_con > 0
							If Type('ITEMAmend_VW.AmendNo')='N'
								Update ITEMAmend_VW Set AmendNo = nAmendNo
							Endif
							Select ITEMAmend_VW
							Scan
								Select ITEMAmend_VW
								msqlstr  = _curvouobj.sqlconobj.geninsert(_curvouobj.entry_tbl+'ItemAm',"","","ITEMAmend_VW",mvu_backend)
								etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],;
									"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
								Select ITEMAmend_VW
							Endscan
						Endif
					Endif
					If Used('ITREFAmend_VW')
						If etsql_con > 0
							If Type('ITREFAmend_VW.AmendNo')='N'
								Update ITREFAmend_VW Set AmendNo = nAmendNo
							Endif

							Select ITREFAmend_VW
							Scan
								Select ITREFAmend_VW

								msqlstr  = _curvouobj.sqlconobj.geninsert(_curvouobj.entry_tbl+'ItrefAm',"","","ITREFAmend_VW",mvu_backend)
								etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],;
									"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
								Select ITREFAmend_VW
							Endscan

						Endif
					Endif
					If Used('ACDETAmend_VW')
						If etsql_con > 0
							If Type('ACDETAmend_VW.AmendNo')='N'
								Update ACDETAmend_VW Set AmendNo = nAmendNo
							Endif

							Select ACDETAmend_VW
							Scan
								Select ACDETAmend_VW

								msqlstr  = _curvouobj.sqlconobj.geninsert(_curvouobj.entry_tbl+'AcDetAm',"","","ACDETAmend_VW",mvu_backend)
								etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],;
									"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
								Select ACDETAmend_VW
							Endscan
						Endif
					Endif
					If Used('MALLAmend_VW')
						If etsql_con > 0
							If Type('MALLAmend_VW.AmendNo')='N'
								Update MALLAmend_VW Set AmendNo = nAmendNo
							Endif

							Select MALLAmend_VW
							Scan
								Select MALLAmend_VW
								If MALLAmend_VW.entry_ty=MainAmend_vw.entry_ty And MALLAmend_VW.tran_cd=MainAmend_vw.tran_cd
									msqlstr  = _curvouobj.sqlconobj.geninsert(_curvouobj.entry_tbl+'MallAm',"","","MALLAmend_VW",mvu_backend)
									etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],;
										"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
								Endif
								Select MALLAmend_VW
							Endscan
						Endif
					Endif
					If Used('CALLAmend_VW')
						If etsql_con > 0
							If Type('CALLAmend_VW.AmendNo')='N'
								Update CALLAmend_VW Set AmendNo = nAmendNo
							Endif

							Select CALLAmend_VW
							Scan
								Select CALLAmend_VW

								msqlstr  = _curvouobj.sqlconobj.geninsert(_curvouobj.entry_tbl+'CallAm',"","","CALLAmend_VW",mvu_backend)
								etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[],;
									"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
								Select CALLAmend_VW
							Endscan
						Endif
					Endif
				Endif
			Endif
		Endif
	Endif
Endif
&& Added by Sachin N. S. on 30/10/2017 for Bug-30782 -- End

&&Added by Priyanka B on 02042018 for Bug-30832 Start
_curscrobj = _Screen.ActiveForm
If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
	If Inlist(main_vw.entry_ty,"PT","P1","UB","E1","PR","GC","C6","GD","D6")
		If Empty(main_vw.U_ITCDATE)
			Replace main_vw.U_ITCDATE With Ttod(main_vw.Date) In main_vw
			etsql_str  = "Update "+_curvouobj.entry_tbl+"Main set u_itcdate=?main_vw.date Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty "
			etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		Endif
	Endif
Endif
&&Added by Priyanka B on 02042018 for Bug-30832 End

&& Added by Shrikant S. on 01/06/2018 for Bug-31591		&& Start
_curscrobj = _Screen.ActiveForm
If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
	If Inlist(main_vw.entry_ty,"ST","PT","AR","DC","LI","IL","LR","RL","P1","EI") And Type('main_vw.ewbqrcode')<>'U'
		etsql_str  = "Update "+_curvouobj.entry_tbl+"Main set ewbqrcode=0x Where Tran_cd = ?Main_vw.Tran_cd And Entry_ty = ?Main_vw.Entry_ty and RTRIM(ewbn)=''"
		etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	Endif
Endif
&& Added by Shrikant S. on 01/06/2018 for Bug-31591		&& End

&&Changes done for Bug-31690 Start
&&Rup
*!*	If (_curscrobj.addmode = .T. Or _curscrobj.editmode = .T.) And Inlist(main_vw.entry_ty,"BP")  &&Commented by Priyanka B on 25082018 for Bug-31756
If (_curscrobj.addmode = .T. Or _curscrobj.editmode = .T.) And Inlist(main_vw.entry_ty,"BP","RV","CO","GS")  &&Modified by Priyanka B on 25082018 for Bug-31756
	vChqErrMsg=""
	*!*		If !Empty(main_vw.cheq_no) 		&& Commented by Shrikant S. on 27/07/2018 for Sprint 1.0.3
	If !Empty(main_vw.cheq_no) And !Isnull(main_vw.cheq_no)			&& Added by Shrikant S. on 27/07/2018 for Sprint 1.0.3
		If Alltrim(Upper(main_vw.party_nm)) <> "CANCELLED."  &&Added by Priyanka B on 21062018 for Bug-31690
			&&Commented by Priyanka B on 25082018 for Bug-31756 Start
			*!*				msql_str1="Select  BCode_Nm=isnull(l.BCode_Nm,''),Inv_No=isnull(m.Inv_No,''),Status,Iss_Entry_Ty,Iss_Tran_cd From Cheque_Details cd"
			*!*				msql_str2=" inner join ChequeBookMaster cb on (cb.Book_Id=cd.Book_Id) Left join Lcode l on (l.Entry_Ty=cd.Iss_Entry_Ty ) Left Join BpMain m on (cd.Iss_Tran_Cd=m.Tran_cd)"
			*!*				msql_str3=" Where cb.Bank_Nm='"+Alltrim(main_vw.Bank_nm)+"' and cd.Chq_No='"+Alltrim(main_vw.cheq_no)+"'"
			*!*				msql_str3=msql_str3+" and ?main_vw.date between cb.book_act_dt and cb.book_deact_dt"  &&Added by Priyanka B on 31072018 for Bug-31690
			&&Commented by Priyanka B on 25082018 for Bug-31756 End

			&&Modified by Priyanka B on 25082018 for Bug-31756 Start
			msql_str1="select top 1 * From (Select  BCode_Nm=isnull(l.BCode_Nm,''),Inv_No=isnull(m.Inv_No,''),Status,Iss_Entry_Ty,Iss_Tran_cd From Cheque_Details cd"
			msql_str2=" inner join ChequeBookMaster cb on (cb.Book_Id=cd.Book_Id) Left join Lcode l on (l.Entry_Ty=cd.Iss_Entry_Ty ) Left Join BpMain m on (cd.Iss_Tran_Cd=m.Tran_cd)"
			*!*				msql_str3=" Where cb.Bank_Nm='"+Alltrim(main_vw.Bank_nm)+"' and cd.Chq_No='"+Alltrim(main_vw.cheq_no)+"'"  &&Commented by Priyanka B on 27122019 for Bug-33165
			msql_str3=" Where cb.Bank_Nm=?main_vw.Bank_nm and cd.Chq_No='"+Alltrim(main_vw.cheq_no)+"'"  &&Modified by Priyanka B on 27122019 for Bug-33165
			msql_str3=msql_str3+" and ?main_vw.date between cb.book_act_dt and cb.book_deact_dt"
			msql_str3=msql_str3+" union Select BCode_Nm=isnull(l.BCode_Nm,''),Inv_No=isnull(m.Inv_No,''),Status,Iss_Entry_Ty,Iss_Tran_cd From Cheque_Details cd"+;
				" inner join ChequeBookMaster cb on (cb.Book_Id=cd.Book_Id) Left join Lcode l on (l.Entry_Ty=cd.Iss_Entry_Ty ) Left Join BrMain m on (cd.Iss_Tran_Cd=m.Tran_cd)"+;
				" Where cb.Bank_Nm=?main_vw.Bank_nm and cd.Chq_No='"+Alltrim(main_vw.cheq_no)+"' and ?main_vw.date between cb.book_act_dt and cb.book_deact_dt)aa order by bcode_nm desc"  &&Modified by Priyanka B on 27122019 for Bug-33165
			*!*					" Where cb.Bank_Nm='"+Alltrim(main_vw.Bank_nm)+"' and cd.Chq_No='"+Alltrim(main_vw.cheq_no)+"' and ?main_vw.date between cb.book_act_dt and cb.book_deact_dt)aa order by bcode_nm desc"  &&Commented by Priyanka B on 27122019 for Bug-33165
			&&Modified by Priyanka B on 25082018 for Bug-31756 End

			etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msql_str1+msql_str2+msql_str3,"_chkChqNo","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If etsql_con > 0 And Used("_chkChqNo")
				Select _chkChqNo
				If Reccount("_chkChqNo")>0
					Select _chkChqNo
					If (_chkChqNo.Status<>"Available" And _curscrobj.addmode = .T.) Or (!(_chkChqNo.Iss_Entry_Ty=main_vw.entry_ty And _chkChqNo.Iss_Tran_cd =main_vw.tran_cd) And _curscrobj.editmode = .T. ;
							AND !Empty(_chkChqNo.Iss_Entry_Ty))  &&Added by Priyanka B on 30072018 for Bug-31690
						vChqErrMsg=	"Cheque is Issued"
						If (_chkChqNo.bcode_nm<>"")
							vChqErrMsg = vChqErrMsg + " Against Transaction='"+Alltrim(_chkChqNo.bcode_nm)+"'"
						Endif
						If (_chkChqNo.Inv_No<>"")
							vChqErrMsg = vChqErrMsg + " And Transaction Number='"+Alltrim(_chkChqNo.Inv_No)+"'"
						Endif
					Endif
					Use In _chkChqNo
					If !Empty(vChqErrMsg)
						=Messagebox(vChqErrMsg,16,vumess)
						Return .F.
					Else
						msql_str1= "Update cd set Status='Available',Iss_Entry_Ty='',Iss_Tran_cd='0' from Cheque_Details cd"
						*!*							msql_str2= " Left Join Lcode l on (l.Entry_Ty=cd.Iss_Entry_Ty ) Left Join BpMain m on (cd.Iss_Tran_Cd=m.Tran_cd)"  &&Commented by Priyanka B on 25082018 for Bug-31756
						msql_str2= " Left Join Lcode l on (l.Entry_Ty=cd.Iss_Entry_Ty ) Left Join "+Alltrim(lcode_vw.bcode_nm)+"Main m on (cd.Iss_Tran_Cd=m.Tran_cd)"  &&Modified by Priyanka B on 25082018 for Bug-31756
						*!*							msql_str3= " Where cd.Bank_Nm='"+Alltrim(main_vw.Bank_nm)+"' and Iss_Entry_Ty='"+main_vw.entry_ty+"' and Iss_Tran_cd="+Alltrim(Str(main_vw.tran_cd))+" and cd.Status='Issued'"  &&Commented by Priyanka B on 27122019 for Bug-33165
						msql_str3= " Where cd.Bank_Nm=?main_vw.Bank_nm and Iss_Entry_Ty='"+main_vw.entry_ty+"' and Iss_Tran_cd="+Alltrim(Str(main_vw.tran_cd))+" and cd.Status='Issued'"  &&Modified by Priyanka B on 27122019 for Bug-33165
						etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msql_str1+msql_str2+msql_str3,[_tChqNo],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

						If etsql_con > 0 And Used('_tChqNo')
							Use In _tChqNo
						Endif
						msql_str1= "Update cd set Status='"+Iif(!Empty(main_vw.u_Chqdt) And main_vw.u_Chqdt> main_vw.Date,'PDC','Issued')+"',Iss_Entry_Ty='"+Alltrim(main_vw.entry_ty)+"',Iss_Tran_cd="+Alltrim(Str(main_vw.tran_cd))+" from Cheque_Details cd"
						*!*							msql_str2= " Where cd.Bank_Nm='"+Alltrim(main_vw.Bank_nm)+"' and cd.Chq_No='"+Alltrim(main_vw.cheq_no)+"' and cd.Status in ('Available','PDC','Dishonor/Cancelled')"  &&Commented by Priyanka B on 27122019 for Bug-33165
						msql_str2= " Where cd.Bank_Nm=?main_vw.Bank_nm and cd.Chq_No='"+Alltrim(main_vw.cheq_no)+"' and cd.Status in ('Available','PDC','Dishonor/Cancelled')"  &&Modified by Priyanka B on 27122019 for Bug-33165
						etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msql_str1+msql_str2,[_tChqNo],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						If etsql_con > 0 And Used('_tChqNo')
							Use In _tChqNo
						Endif

					Endif
				Endif
			Endif
			&&Added by Priyanka B on 21062018 for Bug-31690 Start
		Else
			If Alltrim(Upper(main_vw.party_nm)) = "CANCELLED."
				msql_str1= "Update cd set Status='Dishonor/Cancelled' from Cheque_Details cd"
				*!*					msql_str2= " Left Join Lcode l on (l.Entry_Ty=cd.Iss_Entry_Ty) Left Join BpMain m on (cd.Iss_Tran_Cd=m.Tran_cd)"  &&Commented by Priyanka B on 25082018 for Bug-31756
				msql_str2= " Left Join Lcode l on (l.Entry_Ty=cd.Iss_Entry_Ty) Left Join "+Alltrim(lcode_vw.bcode_nm)+"Main m on (cd.Iss_Tran_Cd=m.Tran_cd)"  &&Modified by Priyanka B on 25082018 for Bug-31756
				*!*					msql_str3= " Where cd.Bank_Nm='"+Alltrim(main_vw.Bank_nm)+"' and Iss_Entry_Ty='"+main_vw.entry_ty+"' and Iss_Tran_cd="+Alltrim(Str(main_vw.tran_cd))  &&Commented by Priyanka B on 27122019 for Bug-33165
				msql_str3= " Where cd.Bank_Nm=?main_vw.Bank_nm and Iss_Entry_Ty='"+main_vw.entry_ty+"' and Iss_Tran_cd="+Alltrim(Str(main_vw.tran_cd))  &&Modified by Priyanka B on 27122019 for Bug-33165
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msql_str1+msql_str2+msql_str3,[_tChqNo],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

				If etsql_con > 0 And Used('_tChqNo')
					Use In _tChqNo
				Endif
			Endif
		Endif
	Else
		msql_str1= "Update cd set Status='Available',Iss_Entry_Ty='',Iss_Tran_cd='0' from Cheque_Details cd"
		*!*			msql_str2= " Left Join Lcode l on (l.Entry_Ty=cd.Iss_Entry_Ty ) Left Join BpMain m on (cd.Iss_Tran_Cd=m.Tran_cd)"  &&Commented by Priyanka B on 25082018 for Bug-31756
		msql_str2= " Left Join Lcode l on (l.Entry_Ty=cd.Iss_Entry_Ty ) Left Join "+Alltrim(lcode_vw.bcode_nm)+"Main m on (cd.Iss_Tran_Cd=m.Tran_cd)"  &&Modified by Priyanka B on 25082018 for Bug-31756
		*!*			msql_str3= " Where cd.Bank_Nm='"+Alltrim(main_vw.Bank_nm)+"' and Iss_Entry_Ty='"+main_vw.entry_ty+"' and Iss_Tran_cd="+Alltrim(Str(main_vw.tran_cd))  &&Commented by Priyanka B on 27122019 for Bug-33165
		msql_str3= " Where cd.Bank_Nm=?main_vw.Bank_nm and Iss_Entry_Ty='"+main_vw.entry_ty+"' and Iss_Tran_cd="+Alltrim(Str(main_vw.tran_cd))  &&Modified by Priyanka B on 27122019 for Bug-33165
		etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msql_str1+msql_str2+msql_str3,[_tChqNo],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

		If etsql_con > 0 And Used('_tChqNo')
			Use In _tChqNo
		Endif
	Endif
	&&Added by Priyanka B on 21062018 for Bug-31690 End
Endif
&&Changes done for Bug-31690 End

&&Added by Priyanka B on 01092018 for Bug-31791 Start
If (_curscrobj.addmode = .T. Or _curscrobj.editmode = .T.)
	If Alltrim(Upper(main_vw.party_nm)) = "CANCELLED."
		If main_vw.Date > Date()
			Messagebox("Transaction Date is greater than System date. You cannot cancel this transaction!!",0+16,vumess)
			Return .F.
		Endif
	Endif
Endif
&&Added by Priyanka B on 01092018 for Bug-31791 End

&& Commented by Shrikant S. on 14/12/2018 for Bug-32062		&& Start
*!*	&& Added by Shrikant S. on 16/11/2018 for Auto updater 2.0.1		&& Start
*!*	If (_curvouobj.addmode = .T. Or _curvouobj.editmode = .T.) And  ('phrmretlr' $ vchkprod )
*!*	*!*		If Inlist(_curvouobj.pcvtype,'PK','IE','PX','MO')			&& Commented by Shrikant S. on 26/11/2018
*!*		If Inlist(_curvouobj.pcvtype,'PK','MO')							&& Added by Shrikant S. on 26/11/2018
*!*			Select item_vw
*!*			llnrecno=Iif(!Eof(),Recno(),0)
*!*			Scan
*!*				If Type('Item_vw.batchno')='C'
*!*					If(!Empty(item_vw.batchno))
*!*						etsql_str  = "Select Batchno,batchRate,prate,srate,ptr,pts From pretbatchdetails Where It_code=?Item_vw.It_code and Batchno=?Item_vw.Batchno and StoreNm=?Item_vw.Ware_nm"
*!*						etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[chkbat],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*						Select chkbat
*!*						If Reccount('chkbat')>0
*!*							Locate
*!*							rateupd=""
*!*							mmess="Difference found in batch master and current transaction for Rate for Item :"+Alltrim(item_vw.Item)
*!*							If Messagebox(mmess+Chr(13)+'Do you want to update new Rate?',4,vumess) = 6
*!*								rateupd=",batchRate= ((?item_vw.mrprate * ?item_vw.gpqty) /?item_vw.Qty)"
*!*								rateupd=rateupd+",pRate= ?item_vw.rate"
*!*								rateupd=rateupd+",sRate= ((?item_vw.srate * ?item_vw.gpqty) /?item_vw.Qty)"
*!*								rateupd=rateupd+",ptr= ((?item_vw.ptr * ?item_vw.gpqty) /?item_vw.Qty)"
*!*								rateupd=rateupd+",pts= ((?item_vw.pts * ?item_vw.gpqty) /?item_vw.Qty)"
*!*								etsql_str  = " Update pretbatchdetails set Qty=Qty "+Iif(lcode_vw.inv_stk='-',"-?Item_vw.Qty","+?Item_vw.Qty")+Iif(Len(rateupd)>0,rateupd,"")+ " Where It_code=?Item_vw.It_code and Batchno=?Item_vw.Batchno and Storenm=?Item_vw.Ware_nm"
*!*								etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*							Endif

*!*						Else
*!*							lnmrpval= ((item_vw.mrprate * item_vw.gpqty)/item_vw.Qty)
*!*							lnpval=item_vw.rate
*!*							lnsval=((item_vw.srate * item_vw.gpqty)/item_vw.Qty)
*!*							lnptrval=((item_vw.ptr * item_vw.gpqty)/item_vw.Qty)
*!*							lnptsval=((item_vw.pts * item_vw.gpqty)/item_vw.Qty)
*!*							etsql_str  = " Insert Into pretbatchdetails(It_code,Batchno,BatchRate,Inc_gst,BatchOnhold,mfgdt,expdt,prate,srate,ptr,pts,storenm,Qty)"
*!*							etsql_str  = etsql_str  +" Values(?Item_vw.It_code,?Item_vw.Batchno,?lnmrpval,?Item_vw.gstincl,0,?Item_vw.Mfgdt,?Item_vw.Expdt,?lnpval,?lnsval,?lnptrval,?lnptsval,?Item_vw.Ware_nm,"+Iif(lcode_vw.inv_stk='-',"-?Item_vw.Qty","?Item_vw.Qty")+")"
*!*							etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
*!*						Endif
*!*						Use In chkbat
*!*					Endif
*!*				Endif
*!*				Select item_vw
*!*			Endscan
*!*			If llnrecno >0
*!*				Select item_vw
*!*				Go llnrecno
*!*			Endif
*!*		Else
*!*			Select item_vw
*!*			llnrecno=Iif(!Eof(),Recno(),0)
*!*			Scan
*!*				If Type('Item_vw.batchno')='C'
*!*					If(!Empty(item_vw.batchno))
*!*						etsql_str  = " Update pretbatchdetails set Qty=Qty "+Iif(lcode_vw.inv_stk='-',"?Item_vw.Qty","-?Item_vw.Qty")+ " Where It_code=?Item_vw.It_code and Batchno=?Item_vw.Batchno and Storenm=?Item_vw.Ware_nm"
*!*						etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

*!*					Endif
*!*				Endif
*!*				Select item_vw
*!*			Endscan

*!*		Endif


*!*	Endif
*!*	&& Added by Shrikant S. on 16/11/2018 for Auto updater 2.0.1		&& End
&& Commented by Shrikant S. on 14/12/2018 for Bug-32062		&& End

&&Added by Priyanka B on 14122018 for Bug-32062 Start
_curvouobj = _Screen.ActiveForm
If (_curvouobj.addmode = .T. Or _curvouobj.editmode = .T.) And ('phrmretlr' $ vchkprod )
	If Inlist(_curvouobj.pcvtype,'SK')
		etsql_str="SELECT gstin FROM ac_mast where ac_name=?main_vw.party_nm"
		etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"_chkgstin","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
		Select _chkgstin
		If Reccount()>0
			If Alltrim(Upper(_chkgstin.gstin)) <> "UNREGISTERED"
				Select item_vw
				Scan
					etsql_str="SELECT top 1 schedule FROM it_mast it inner join pRetSalt_Master ps on (it.prSalt=ps.Salt) where it_code=?item_vw.it_code and prsalt<>'' and schedule<>''"
					etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"_chkDrug","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					Select _chkDrug
					If Reccount()>0
						If Empty(mainadd_vw.patientnm) Or Empty(mainadd_vw.drname) Or Empty(mainadd_vw.drprescno)
							Messagebox("Patient name, Doctor name and Prescription No. are mandatory for scheduled drugs.",0+16,vumess)
							etsql_con=0
							Exit
						Endif
					Endif
					Select item_vw
				Endscan
			Endif
		Endif
	Endif
Endif
&&Added by Priyanka B on 14122018 for Bug-32062 End

&&Added by Shrikant S. on 28012019 for Bug-32210 Start
_curvouobj = _Screen.ActiveForm
etsql_con = 1

&&Added by Priyanka B on 05032019 for Bug-32210 Start

&&Sales Order Export direct saving Start
If (_curvouobj.addmode Or _curvouobj.editmode) And !Empty(lcode_vw.iautotran) And !Empty(lcode_vw.autotranon) And !Used('vendorselect')
	If Inlist(_curvouobj.pcvtype,'S2')
		If Used("Itref_vw")
			msqlstr = "select cast('1' as bit) as lselect,m.party_nm as ac_name,CAST('' as varchar(2)) as entry_ty,CAST(0 as int) as tran_cd,CAST('' as varchar(5)) as itserial,o.it_code,o.rqty as qty,i.rate,m.ac_id from othitref o"
			msqlstr =msqlstr + " inner join pqitem i on (o.rentry_ty=i.entry_ty and o.itref_tran=i.tran_cd and o.ritserial=i.itserial)"
			msqlstr =msqlstr + " inner join pqmain m on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd)"
			msqlstr =msqlstr + " where 1=2"
			etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"curOthItref","_curvouobj.nHandle",_curvouobj.DataSessionId)
			If etsql_con <= 0
				Return .F.
			Endif

			Select item_vw
			lnrecno=Iif(!Eof(),Recno(),0)

			Scan
				Select Itref_vw
				Go Top
				Scan For Itref_vw.entry_ty=item_vw.entry_ty And Itref_vw.tran_cd=item_vw.tran_cd And Itref_vw.itserial=item_vw.itserial
					msqlstr = "select cast('1' as bit) as lselect,m.party_nm as ac_name,entry_ty=?main_vw.entry_ty,tran_cd=?main_vw.tran_cd,itserial=?item_vw.itserial,o.it_code,o.rqty as qty,i.rate,m.ac_id from othitref o"
					msqlstr =msqlstr + " inner join pqitem i on (o.rentry_ty=i.entry_ty and o.itref_tran=i.tran_cd and o.ritserial=i.itserial)"
					msqlstr =msqlstr + " inner join pqmain m on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd)"
					msqlstr =msqlstr + " where o.entry_ty=?itref_vw.rentry_ty and o.tran_cd=?itref_vw.itref_tran and o.itserial=?itref_vw.ritserial"

					etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"curRef1","_curvouobj.nHandle",_curvouobj.DataSessionId)
					If etsql_con <= 0
						Return .F.
					Endif

					Select curOthitref
					If Used('curRef1')
						Append From Dbf('curRef1')
					Endif
					Select Itref_vw
				Endscan
				Select item_vw
			Endscan
			If lnrecno>0
				Select item_vw
				Go lnrecno
			Endif
		Endif
	Endif

	If Used("curOthitref")
		Select * From curOthitref Where lselect=.T. And Qty>0.00 Into Cursor vendorselect Readwrite
	Endif
Endif
&&Sales Order Export direct saving End

If (_curvouobj.addmode Or _curvouobj.editmode) And !Empty(lcode_vw.iautotran) And !Empty(lcode_vw.autotranon) And Used('vendorselect')
	lcQtyMismatch=.F.

	Select vendorselect
	Replace All tran_cd With main_vw.tran_cd In vendorselect
	*!*		Select curVenddtls
	*!*		Replace All tran_cd With main_vw.tran_cd In curVenddtls

	Select item_vw.item_no,vendorselect.Qty As vendqty,item_vw.Qty From item_vw Left Join vendorselect On (item_vw.entry_ty=vendorselect.entry_ty And item_vw.tran_cd=vendorselect.tran_cd And item_vw.itserial=vendorselect.itserial);
		WHERE (vendorselect.Qty > item_vw.Qty) Into Cursor curtotqty
	**		group By item_vw.itserial,item_vw.Qty,item_vw.item_no Having (Sum(vendorselect.Qty) <> item_vw.Qty) Into Cursor curtotqty

	Select curtotqty
	If Reccount() > 0
		lineitems = ""
		Select curtotqty
		Scan
			If !Inlist(lineitems,Alltrim(curtotqty.item_no))
				lineitems = lineitems + Alltrim(curtotqty.item_no) + ","
			Endif
			Select curtotqty
		Endscan
		lineitems = Left(lineitems,Len(lineitems)-1)

		Messagebox("Supply Qty has been changed"+Chr(13)+"Please change vendor qty for below line items!!"+Chr(13)+lineitems,0+64,vumess)
		Return .F.
	Endif
Endif
&&Added by Priyanka B on 05032019 for Bug-32210 End

If (_curvouobj.addmode Or _curvouobj.editmode) And !Empty(lcode_vw.iautotran) And !Empty(lcode_vw.autotranon) And Used('vendorselect')

	etsql_str="Select top 1 * From AutoTranRef Where Entry_ty=?Main_vw.Entry_ty and Tran_cd=?Main_vw.Tran_cd "
	etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"autorefvw","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	If etsql_con <=0
		Return .F.
	Endif

	If Reccount('autorefvw')>0
		llgenEntry=.F.
	Else
		llgenEntry=.T.
	Endif
	Delete All In autorefvw

	nFieldCount = Alines(aFieldList, Alltrim(lcode_vw.autotranon), ",")

	If llgenEntry=.T.
		lcGenEntry=Alltrim(lcode_vw.iautotran)
		lcgenon=","+Alltrim(lcode_vw.autotranon)
		lcfiltcond=""
		For i=1 To nFieldCount
			lcfiltcond= lcfiltcond +  " and "+Alltrim(aFieldList[i])+"="+"venlist."+Alltrim(aFieldList[i])
		Next

		Wait Window "Generating Transaction : "+lcGenEntry Nowait
		Select entry_ty,tran_cd,ac_name &lcgenon From vendorselect Where lselect=.T. Group By entry_ty,tran_cd,ac_name &lcgenon Into Cursor venlist Readwrite

		Select * From vendorselect Where lselect=.T. Into Cursor vendData Readwrite


		If Reccount('venlist')>0

			etsql_str="Select top 1 * From Lcode Where Entry_ty=?lcGenEntry "
			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"tmLcode","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If etsql_con <=0
				Return .F.
			Endif
			lcGenEntryTbl=Iif(tmLcode.bcode_nm<>"",tmLcode.bcode_nm,Iif(tmLcode.Ext_vou=.T.,"",tmLcode.entry_ty))

			etsql_str="Select * From Dcmast Where Entry_ty=?lcGenEntry "
			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"tmpChg","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If etsql_con <=0
				Return .F.
			Endif


			etsql_str="Select top 1 * From "+lcGenEntryTbl+"main Where 1=2 "
			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"exMain_vw","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If etsql_con <=0
				Return .F.
			Endif

			etsql_str="Select top 1 * From "+lcGenEntryTbl+"item Where 1=2 "
			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"exItem_vw","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If etsql_con <=0
				Return .F.
			Endif

			Wait Window "Generating Header" Nowait

			Select venlist
			Scan
				sumGross=0
				sumNetAmt=0
				genitserial=1

				Select main_vw
				Scatter Memvar Memo

				Select exMain_vw
				Append Blank
				Gather Memvar Memo

				*			Replace Ac_id With venlist.Ac_id,Cons_id With venlist.Ac_id,Entry_ty With lcGenEntry,Tran_cd With 0 In exMain_vw
				lcupdt=Strtran(Strtran(lcfiltcond,"="," with "),"and",",")

				Replace entry_ty With lcGenEntry,tran_cd With 0,party_nm With venlist.ac_name &lcupdt In exMain_vw

				&&				macname= getAccountName(exMain_vw.ac_id,_curvouobj)
				macid= getAccountId(exMain_vw.party_nm,_curvouobj)


				Wait Window "Generating Transaction No." Nowait
				Minv_no=GetInvNo(lcGenEntry, exMain_vw.inv_sr, '', exMain_vw.Date, '', '',tmLcode.invno_size, _curvouobj.sqlconobj,_curvouobj.DataSessionId,lcGenEntryTbl,"_curvouobj.nHandle",company.dbname,company.sta_dt,company.end_Dt,company.dbname)

				Wait Window "Generating Document No." Nowait
				MDocumentNo=GetDocNo(lcGenEntry,exMain_vw.Date,lcGenEntryTbl,_curvouobj.sqlconobj,_curvouobj.DataSessionId,"_curvouobj.nHandle",company.dbname,company.sta_dt,company.end_Dt,company.dbname)


				msysdate = Dtoc(Date())+' '+Time()
				sql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,[select GetDate() As Curdate],[tmptbl_vw],;
					"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If sql_con > 0 And Used('tmptbl_vw')
					Select tmptbl_vw
					If Reccount() > 0
						msysdate = Ttoc(tmptbl_vw.Curdate)
					Endif
				Endif

				Select exMain_vw
				Replace doc_no With MDocumentNo,Inv_No With Minv_no,ac_id With macid,cons_id With ac_id In exMain_vw
				If tmLcode.apgenps=.F.				&&General Approval
					Replace Apgen With 'YES',Apgenby With mUsername,Apgentime With msysdate In exMain_vw
				Else
					Replace Apgen With [PENDING],Apgenby With [],Apgentime With [] In exMain_vw
				Endif
				If tmLcode.apledps=.F. 				&&Posting Approval
					Replace Apled With 'YES',Apledby With mUsername,Apledtime With msysdate In exMain_vw
				Else
					Replace Apled With [PENDING],Apledby With [],Apledtime With [] In exMain_vw
				Endif
				Replace user_name With mUsername,sysdate With msysdate,compid With company.compid In exMain_vw

				Select vendData
				Locate
				Scan For Alltrim(entry_ty)==Alltrim(venlist.entry_ty) And tran_cd=venlist.tran_cd And lselect=.T. And ac_name=venlist.ac_name &lcfiltcond
					lcserial=vendData.itserial

					Wait Window "Generating Detail" Nowait
					Select item_vw
					Locate
					Locate For itserial=lcserial
					If Found()

						Scatter Memvar Memo

						Select exItem_vw
						Append Blank
						Gather Memvar Memo
						Replace ac_id With exMain_vw.ac_id,party_nm With exMain_vw.party_nm,Qty With vendData.Qty;
							,entry_ty With lcGenEntry,tran_cd With 0,doc_no With exMain_vw.doc_no,Inv_No With exMain_vw.Inv_No;
							,item_no With Str(genitserial,5),itserial With Padl(Allt(Str(genitserial)),5,"0") In exItem_vw

						Replace u_asseamt With Qty * Rate In exItem_vw
						If Type('exItem_vw.u_asseamt')='N'
							mAValue =exItem_vw.u_asseamt
							mamount =exItem_vw.u_asseamt
						Endif

						Wait Window "Calculating Charges Detailwise" Nowait

						Select tmpChg
						Set Filter To att_file=.F.
						Scan
							mfld_nm="exItem_vw."+Allt(tmpChg.fld_nm)

							lcpert_name = Alltrim(tmpChg.pert_name)

							If !Empty(lcpert_name)
								Select exItem_vw
								If Vartype(lcpert_name) = "U" Or Vartype(lcFld_nm) = "U"
									Loop
								Endif

								mpert = Evaluate(lcpert_name)
								mcalamt = 0
								If !Empty(dcmast_vw.AmtExpr)
									lcfldexpr=Strtran(Strtran(Lower(Allt(tmpChg.AmtExpr)),"item_vw.","ExItem_vw."),"main_vw.","ExMain_vw.")
									mcalamt = Evaluate(lcfldexpr)
								Endif
								If mcalamt <> 0
									If dcmast_vw.disp_sign==[%]
										mamt = mcalamt * (mpert/100)
									Endif
								Endif

								Replace (lcFld_nm) With mamt In exItem_vw
							Endif

							Do Case
								Case !Empty(tmpChg.a_s)
									ma_s = tmpChg.a_s
								Case Inlist(tmpChg.Code,"D","F")
									ma_s = '-'
								Otherwise
									ma_s = '+'
							Endcase
							If tmpChg.Bef_Aft  = 1 And Empty(tmpChg.Excl_Gross)
								mAValue = mAValue + Evaluate(ma_s+mfld_nm)
							Endif
							If !Inlist(tmpChg.Excl_Gross,'C','A','E','G')
								mamount = mamount + Evaluate(ma_s+mfld_nm)
								Do Case
									Case dcmast_vw.Code = 'D'
										Replace Tot_deduc With exItem_vw.Tot_deduc + Evaluate(ma_s+mfld_nm)  In exItem_vw
									Case dcmast_vw.Code = 'T'
										Replace Tot_tax With exItem_vw.Tot_tax + Evaluate(ma_s+mfld_nm) In exItem_vw
									Case dcmast_vw.Code = 'E'
										Replace Tot_examt With exItem_vw.Tot_examt + Evaluate(ma_s+mfld_nm)  In exItem_vw
									Case dcmast_vw.Code = 'A'
										Replace Tot_add With exItem_vw.Tot_add + Evaluate(ma_s+mfld_nm)  In exItem_vw
									Case dcmast_vw.Code = 'N'
										Replace Tot_nontax With exItem_vw.Tot_nontax + Evaluate(ma_s+mfld_nm)  In exItem_vw
									Case dcmast_vw.Code = 'F'
										Replace Tot_fdisc With exItem_vw.Tot_fdisc + Evaluate(ma_s+mfld_nm)  In exItem_vw
								Endcase
							Endif

						Endscan


						Wait Window "Calculating Charges Headerwise" Nowait

						Select exMain_vw
						Scatter Name oMain_vw Fields Tot_nontax,Tot_deduc,Tot_tax,Tot_add,Tot_fdisc Blank
						Select exItem_vw
						vrec = Iif(!Eof(),Recno(),0)
						Select exMain_vw
						Replace Tot_examt With 0 In exMain_vw
						Replace Tot_nontax With 0,Tot_deduc With 0,Tot_tax With 0,Tot_add With 0,Tot_fdisc With 0 In exMain_vw

						Sele tmpChg
						Set Filter To
						Set Filter To Inlist(Code,"E") And att_file = .F.
						Scan
							a = Alltrim(fld_nm)
							If Alltrim(tmpChg.Code) = "E"
								If Type('exMain_vw.&a') = 'N'
									Replace &a With 0 In exMain_vw
									Select exItem_vw
									Scan
										Replace &a With exMain_vw.&a + exItem_vw.&a In exMain_vw
									Endscan
									Replace Tot_examt With exMain_vw.Tot_examt + exMain_vw.&a In exMain_vw
								Endif
							Endif
							Sele tmpChg
						Endscan
						Sele tmpChg
						Set Filter To

						Select exItem_vw
						Sum Tot_nontax,Tot_deduc,Tot_tax,Tot_add,Tot_fdisc To oMain_vw.Tot_nontax,oMain_vw.Tot_deduc,oMain_vw.Tot_tax,oMain_vw.Tot_add,oMain_vw.Tot_fdisc For !Deleted()

						Select exMain_vw
						Replace Tot_nontax With oMain_vw.Tot_nontax,Tot_deduc With oMain_vw.Tot_deduc,Tot_tax With oMain_vw.Tot_tax ;
							,Tot_add With oMain_vw.Tot_add ,Tot_fdisc With oMain_vw.Tot_fdisc In exMain_vw

						Select exItem_vw
						If vrec > 0
							Go vrec
						Endif

						Replace Gro_amt With mamount In exItem_vw
						sumNetAmt=sumNetAmt+mamount

						Wait Window "Updating Stock " Nowait

						If !update_itbal(lcGenEntryTbl,_curvouobj)
							Return .F.
						Endif
						*!*							Wait Window "updating itbalw" Nowait
						If !update_itbalw(lcGenEntryTbl,_curvouobj)
							Return .F.
						Endif

						Select autorefvw
						Append Blank In autorefvw
						Replace entry_ty With main_vw.entry_ty, tran_cd With main_vw.tran_cd,itserial With item_vw.itserial,rentry_ty With lcGenEntry,RItserial With exItem_vw.itserial  In autorefvw
					Endif
					genitserial=genitserial+1
					Replace net_amt With sumNetAmt In exMain_vw
					Select vendData
				Endscan


				lcSqlStr = _curvouobj.sqlconobj.geninsert(lcGenEntryTbl+"MAIN","'Tran_cd'","","exMain_vw",1)
				sql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
				If sql_con <0
					Return .F.
				Endif

				mtran_cd=0
				If nretval > 0 And mtran_cd <= 0
					nretval = _curvouobj.sqlconobj.dataconn([EXE],company.dbname," Select IDENT_CURRENT('"+lcGenEntryTbl+"Main"+"') as Tran_cd ",[tmptbl_vw],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
					If nretval > 0 And Used('tmptbl_vw')
						Select tmptbl_vw
						If Reccount() > 0
							mtran_cd = tmptbl_vw.tran_cd
						Endif
						Select exMain_vw
						Replace tran_cd With mtran_cd In exMain_vw
						Select exItem_vw
						Replace All tran_cd With mtran_cd In exItem_vw
						Scan
							lcSqlStr = _curvouobj.sqlconobj.geninsert(lcGenEntryTbl+"ITEM","","","exItem_vw",1)
							nretval= _curvouobj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							If nretval<0
								Return .F.
							Endif
							Select exItem_vw
						Endscan
						Select autorefvw
						Replace All RTran_cd With mtran_cd In autorefvw
						Scan
							lcSqlStr = _curvouobj.sqlconobj.geninsert("autotranref","","","autorefvw",1)
							nretval= _curvouobj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
							If nretval<0
								Return .F.
							Endif
							Select autorefvw
						Endscan
					Endif
					Select exItem_vw
					Zap In exItem_vw
					Select exMain_vw
					Zap In exMain_vw
					Select autorefvw
					Zap In autorefvw
				Endif
			Endscan
		Endif
	Endif
	If  Used("curVenddtls")
		Use In curVenddtls
	Endif

Else
	If ! _curvouobj.addmode And  ! _curvouobj.editmode
		etsql_str="Delete From AutoTranRef Where Entry_ty=?Main_vw.Entry_ty and Tran_cd=?Main_vw.Tran_cd "
		etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
	Endif
	If  Used("curVenddtls")
		Use In curVenddtls
	Endif
Endif
&&Added by Shrikant S. on 28012019 for Bug-32210 End

&&Added by Priyanka B on 13082019 for Bug-32747 Start
_curvouobj = _Screen.ActiveForm

If (Inlist(_curvouobj.pcvtype,"IC","ID","DF") And ("efabric" $ vchkprod))

	lcOldalias = Alias()

	If (_curvouobj.addmode Or _curvouobj.editmode)
		If Inlist(_curvouobj.pcvtype,"IC")
			If Used('Itref_vw')
				Select Itref_vw
				mrecno=Iif(!Eof(),Recno(),0)
				Scan For Inlist(Itref_vw.rentry_ty,"RD","FO")

					Select item_vw
					Scan For itserial==Itref_vw.itserial

						nsqlstr="Update Barcodedet set Rentry_ty=?main_vw.entry_ty,Rtran_cd=?main_vw.tran_cd,Ritserial=?item_vw.itserial,reason='This Barcode is already scanned. Cannot be scanned again.' ";
							+" where RTRIM(Entry_ty)+LTRIM(STR(Tran_cd))+RTRIM(itserial)='"+Alltrim(Itref_vw.rentry_ty)+Alltrim(Str(Itref_vw.Itref_tran))+Alltrim(Itref_vw.RItserial)+"'"
						etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,nsqlstr,[],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						If etsql_con <= 0
							Return .F.
						Endif
					Endscan
					Select Itref_vw
				Endscan

				If mrecno>0
					Select Itref_vw
					Go mrecno
				Endif
			Endif
		Endif
	Else
		If ! _curvouobj.addmode And  ! _curvouobj.editmode
			etsql_str="Update Barcodedet set Rentry_ty='',Rtran_cd=0,Ritserial='',reason='' ";
				+" where RTRIM(REntry_ty)+LTRIM(STR(RTran_cd))='"+Alltrim(main_vw.entry_ty)+Alltrim(Str(main_vw.tran_cd))+"'"
			etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
			If etsql_con <= 0
				Return .F.
			Endif
		Endif
	Endif

	If !Empty(lcOldalias)
		Select (lcOldalias)
	Endif
Endif
&&Added by Priyanka B on 13082019 for Bug-32747 End

&&Added by Priyanka B on 19092019 for Bug-32828 Start
_curvouobj = _Screen.ActiveForm

If _curvouobj.addmode Or _curvouobj.editmode
	If Type('Item_vw.ware_nm')<>'U'
		lcOldalias = Alias()
		&&Commented by Divyang for Bug-33372 on 17/04/2020  Start
		*!*			etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,[Select Top 1 Ware_nm From Warehouse Where ;
		*!*				RTRIM(LTRIM(Ware_nm)) != '' And Validity Like '%]+_curvouobj.pcvtype+[%'  Order by Ware_nm],[_warenm],;
		*!*				"_curvouobj.nHandle",_curvouobj.DataSessionId,.F.)
		&&Commented by Divyang for Bug-33372 on 17/04/2020 	End

		&&Added by Divyang for Bug-33372 on 17/04/2020 Start
		etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,[Select Top 1 Ware_nm From Warehouse Where ;
				RTRIM(LTRIM(Ware_nm)) != '' And Validity Like '%]+_curvouobj.pcvtype+[%' And (actdeact=0 or (actdeact=1 And cast(deactivefrom as date) > ?MAIN_VW.DATE)) Order by Ware_nm],[_warenm],;
			"_curvouobj.nHandle",_curvouobj.DataSessionId,.F.)
		&&Added by Divyang for Bug-33372 on 17/04/2020 End

		If etsql_con > 0 And Used('_warenm')
			Select _warenm
			If Reccount() > 0
				Select item_vw
				Scan
					If Empty(item_vw.Ware_nm)
						Messagebox(Iif(Inlist(_curvouobj.pcvtype,'SV','PK','SK','PG','PX','IE','MO','IU','PU','SU','HS','RA','CA','M2','M3','M4','M6','M7'),"Store Name","Warehouse Name")+" not found in Master!",0+32,vumess)
						etsql_con=0
						Exit
					Endif
					Select item_vw
				Endscan
			Endif
		Endif

		If !Empty(lcOldalias)
			Select (lcOldalias)
		Endif
	Endif
Endif
&&Added by Priyanka B on 19092019 for Bug-32828 End

&&Added by Priyanka B on 15022020 for Bug-33252 Start
_curvouobj = _Screen.ActiveForm

If ("efabric" $ vchkprod)
	lcOldalias = Alias()

	If (_curvouobj.addmode Or _curvouobj.editmode)
		If Inlist(_curvouobj.pcvtype,"RD")
			Select item_vw
			mrecno=Iif(!Eof(),Recno(),0)
			Go Top In item_vw
			Scan
				nsqlstr="Select Bc_val1 from barcodedet where entry_ty='RD' and tran_cd=?item_vw.tran_cd and itserial=?item_vw.itserial"
				etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,nsqlstr,[_barcdval1],"_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)

				If Reccount()>0
					If Type('Item_vw.fbarval')<>'U'
						etsql_str="Update Iritem set fbarval=?_barcdval1.bc_val1 where entry_ty='RD' and tran_cd=?item_vw.tran_cd and itserial=?item_vw.itserial"
						etsql_con  = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,etsql_str,"","_curvouobj.nHandle",_curvouobj.DataSessionId,.T.)
						If etsql_con <= 0
							Return .F.
						Endif
					Endif
				Endif
				Select item_vw
			Endscan

			If mrecno>0
				Select item_vw
				Go mrecno
			Endif
		Endif
	Endif


	If !Empty(lcOldalias)
		Select (lcOldalias)
	Endif
Endif
&&Added by Priyanka B on 15022020 for Bug-33252 End

******** Added By Sachin N. S. on 20/05/2020 FOR Bug-32115 ******** Start
If Inlist(_curvouobj.pcvtype,'SK','ST')
	If Type('_curvouobj._udPOSAutoBRCR')='O'
		If _curvouobj.addmode
			If etsql_con>0
				etsql_con=_curvouobj._udPOSAutoBRCR._udtrigvoufinalupdate()
			Endif
		Endif
	Endif
Endif
******** Added By Sachin N. S. on 20/05/2020 FOR Bug-32115 ******** End

If Type('lcode_vw.m_ASubInfo') <> 'U' And Type('lcode_vw.i_ASubInfo') <> 'U'  && Added by Anil on 04082020
	******** Added By Sachin N. S. on 20/07/2020 for Bug-33615 ******** Start
	If lcode_vw.m_ASubInfo Or lcode_vw.i_ASubInfo
		If Type('_curvouobj.oUdAddSubInfo')='O'
			If etsql_con>0
				etsql_con=_curvouobj.oUdAddSubInfo.udtrigvoufinalupdate()
				If etsql_con <=0
					Return .F.
				Endif
			Endif
		Endif
	Endif
	******** Added By Sachin N. S. on 20/07/2020 for Bug-33615 ******** End
Endif

Return Iif(etsql_con < 1,.F.,.T.)			&& Added By Shrikant S. on 27/06/2014 for Bug-23280 && Please don't comment or remov the line and paste your code in above this line







* AA Changes done as per --> PR_AA_00001_Import_Purchase_AA
* Changes done by EBS Product Team on 01012013
Procedure AAAddEditProc
	_curscrobj = _Screen.ActiveForm

	If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
		lcSqlStr = ""
		EOInvAmt = 0

		* AA Changes done as per --> PR_AA_00001_Import_Purchase_AA
		* Changes done by EBS Product Team on 29122012

		If main_vw.entry_ty = "P1"
			* Changes done by EBS Product Team on 27022013
			Do Case
				Case (_curscrobj.addmode = .T. And _curscrobj.editmode = .F.) Or ((_curscrobj.addmode = .F. And _curscrobj.editmode = .T.);
						And (_curscrobj.oAATranDtySaved = 0 Or _curscrobj.oAATranTotInvAmt = 0))
					lcSqlStr = "Update aa_mast set act_duty = act_duty + " + Alltrim(Str(lmc_vw.aa_duty,12,2)) + ",act_invamt = act_invamt + " + Alltrim(Str(main_vw.aa_invamt,12,2)) +;
						" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

				Case (_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) And (_curscrobj.oAATranDtySaved > 0 Or _curscrobj.oAATranTotInvAmt > 0)
					&& Update old LC Balance amount, when user changes LC no. in Edit mode.

					Do Case
						Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) <> Alltrim(lmc_vw.aalic_no);
								And  (_curscrobj.oAATranDtySaved = main_vw.Tot_examt Or _curscrobj.oAATranTotInvAmt = main_vw.net_amt)
							lcSqlStr = " Update aa_mast set act_duty = act_duty - " + Alltrim(Str(main_vw.Tot_examt,12,2)) + ",act_invamt = act_invamt - " + Alltrim(Str(main_vw.net_amt,12,2)) +;
								" Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

							lcSqlStr = lcSqlStr + " Update aa_mast set act_duty = act_duty + " + Alltrim(Str(main_vw.Tot_examt,12,2)) + ",act_invamt = act_invamt + " + Alltrim(Str(main_vw.net_amt,12,2)) +;
								" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

						Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) = Alltrim(lmc_vw.aalic_no) And (_curscrobj.oAATranItemDtySaved <> main_vw.Tot_examt Or _curscrobj.oAATranItemInvAmt <> main_vw.net_amt)
							lcSqlStr = " Update aa_mast set act_duty = act_duty - " + Alltrim(Str(_curscrobj.oAATranItemDtySaved,12,2)) + ",act_invamt = act_invamt - " + Alltrim(Str(_curscrobj.oAATranItemInvAmt,12,2)) +;
								" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

							lcSqlStr = lcSqlStr + " Update aa_mast set act_duty = act_duty + " + Alltrim(Str(main_vw.Tot_examt,11,2)) + ",act_invamt = act_invamt + " + Alltrim(Str(main_vw.net_amt,11,2)) +;
								" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

						Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) <> Alltrim(lmc_vw.aalic_no) And (_curscrobj.oAATranDtySaved <> main_vw.Tot_examt Or _curscrobj.oAATranTotInvAmt <> main_vw.net_amt)
							lcSqlStr = " Update aa_mast set act_duty = act_duty - " + Alltrim(Str(_curscrobj.oAATranDtySaved,12,2)) + ",act_invamt = act_invamt - " + Alltrim(Str(_curscrobj.oAATranTotInvAmt,12,2)) +;
								" Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

							lcSqlStr = lcSqlStr + " Update aa_mast set act_duty = act_duty + " + Alltrim(Str(main_vw.Tot_examt,12,2)) + ",act_invamt = act_invamt + " + Alltrim(Str(main_vw.net_amt,12,2)) +;
								" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

					Endcase
			Endcase

			*!*			Messagebox("_curscrobj.oAAEOInvAmt = "+Str(_curscrobj.oAAEOInvAmt,12,2)+;
			*!*				CHR(13)+"main_vw.aa_eoinvamt = "+Str(main_vw.aa_eoinvamt,12,2)+;
			*!*				CHR(13)+"curaamast.eo_invamt = "+Str(curaamast.eo_invamt,12,2))
			Do Case
				Case (_curscrobj.addmode = .T. And _curscrobj.editmode = .F.) Or ((_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) And _curscrobj.oAAEOInvAmt = 0)
					*!*				Wait Window "Case 1"
					EOInvAmt = curaamast.eo_invamt + main_vw.Gro_amt + (main_vw.Gro_amt * curaamast.va_per / 100)
					lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = " + Alltrim(Str(EOInvAmt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
						", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

				Case (_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) And _curscrobj.oAAEOInvAmt > 0
					*!*				Wait Window "Case 2"
					Do Case
						Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) <> Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAAEOInvAmt = main_vw.aa_eoinvamt
							*!*					Wait Window "Case 2.1"
							lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = eo_invamt - " + Alltrim(Str(main_vw.aa_eoinvamt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
								", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

							lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = eo_invamt + " + Alltrim(Str(main_vw.aa_eoinvamt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
								", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

						Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) <> Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAAEOInvAmt <> main_vw.aa_eoinvamt
							*!*					Wait Window "Case 2.2"
							lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = eo_invamt - " + Alltrim(Str(_curscrobj.oAAEOInvAmt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
								", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

							lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = eo_invamt + " + Alltrim(Str(main_vw.aa_eoinvamt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
								", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

						Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) = Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAAEOInvAmt <> main_vw.aa_eoinvamt
							*!*					Wait Window "Case 2.3"
							lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = eo_invamt - " + Alltrim(Str(_curscrobj.oAAEOInvAmt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
								", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

							lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = eo_invamt + " + Alltrim(Str(main_vw.aa_eoinvamt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
								", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

						Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) = Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAAEOInvAmt = main_vw.aa_eoinvamt
							*!*					Wait Window "Case 2.4"
							lcSqlStr = lcSqlStr + " Update aa_mast set eo_invamt = " + Alltrim(Str(main_vw.aa_eoinvamt,12,2)) + ",act_duty_fc = act_duty * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
								", fcid = " + Alltrim(Str(main_vw.fcid)) + " Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

					Endcase
			Endcase
			* End ---> PR_AA_00001_Import_Purchase_AA
		Else
			If main_vw.entry_ty = "EI"
				Do Case
					Case (_curscrobj.addmode = .T. And _curscrobj.editmode = .F.) Or ((_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) And _curscrobj.oAATranTotInvAmt = 0)
						lcSqlStr = "Update aa_mast set exp_invamt = exp_invamt + " + Alltrim(Str(lmc_vw.aa_invamt,12,2)) +;
							" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

					Case (_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) And _curscrobj.oAATranTotInvAmt > 0
						&& Update old LC Balance amount, when user changes LC no. in Edit mode.

						Do Case
							Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) <> Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAATranTotInvAmt <> main_vw.net_amt
								lcSqlStr = " Update aa_mast set exp_invamt = exp_invamt - " + Alltrim(Str(_curscrobj.oAATranTotInvAmt,12,2)) +;
									" Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

								lcSqlStr = lcSqlStr + " Update aa_mast set exp_invamt = exp_invamt + " + Alltrim(Str(main_vw.net_amt,12,2)) +;
									" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

							Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) = Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAATranItemInvAmt <> main_vw.net_amt
								lcSqlStr = " Update aa_mast set exp_invamt = exp_invamt - " + Alltrim(Str(_curscrobj.oAATranItemInvAmt,12,2)) +;
									" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

								lcSqlStr = lcSqlStr + " Update aa_mast set exp_invamt = exp_invamt + " + Alltrim(Str(main_vw.net_amt,11,2)) +;
									" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"

							Case !Empty(_curscrobj.oAALicenseNo) And Alltrim(_curscrobj.oAALicenseNo) <> Alltrim(lmc_vw.aalic_no) And  _curscrobj.oAATranTotInvAmt = main_vw.net_amt
								lcSqlStr = " Update aa_mast set exp_invamt = exp_invamt - " + Alltrim(Str(main_vw.net_amt,12,2)) +;
									" Where licen_no = '" + Alltrim(_curscrobj.oAALicenseNo) + "'"

								lcSqlStr = lcSqlStr + " Update aa_mast set exp_invamt = exp_invamt + " + Alltrim(Str(main_vw.net_amt,12,2)) +;
									" Where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"
						Endcase
				Endcase
			Endif
		Endif
		* Changes done by EBS Product Team on 27022013
		* End ---> PR_AA_00002_Export_Sales_EO_AA

		If !Empty(lcSqlStr)
			*!*			IF main_vw.entry_ty = "P1"
			*!*				lcSqlStr = lcSqlStr + " Update aa_mast set act_duty_fc = act_duty * " + STR(main_vw.fcexrate,12,2) + ;
			*!*					", fcid = " + STR(main_vw.fcid) + " where licen_no = '" + IIF(TYPE("main_vw.aalic_no")<>"U",ALLTRIM(main_vw.aalic_no),ALLTRIM(lmc_vw.aalic_no)) + "'"
			*!*			ENDIF
			nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_curscrobj.nHandle",_curscrobj.DataSessionId,.T.)
			If nretval < 0
				nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
				Select(lOldArea)
				Return .F.
			Endif
		Endif
	Endif
	Return .T.
Endproc

Procedure AADeleteProc
	_curscrobj = _Screen.ActiveForm

	lcSqlStr = ""
	* AA Changes done as per --> PR_AA_00001_Import_Purchase_AA
	* Changes done by EBS Product Team on 29122012

	If main_vw.entry_ty = "P1"
		* Changes done by EBS Product Team on 27022013
		If Type("main_vw.aa_duty") <> 'U' And Type("main_vw.aalic_no") <> 'U' And Type("main_vw.aa_invamt") <> 'U'
			lcSqlStr = "Update aa_mast set act_duty = act_duty - " + Alltrim(Str(main_vw.Tot_examt,12,2)) + ;
				",act_invamt = act_invamt - " + Alltrim(Str(main_vw.net_amt,12,2)) + ", eo_invamt = eo_invamt - " + Alltrim(Str(main_vw.aa_eoinvamt,12,2))+;
				" where licen_no = '" + Alltrim(main_vw.aalic_no) + "'"
		Else
			If Type("lmc_vw.aa_duty") <> 'U' And Type("lmc_vw.aalic_no") <> 'U' And Type("main_vw.aa_invamt") <> 'U'
				lcSqlStr = "Update aa_mast set act_duty = act_duty - " + Alltrim(Str(main_vw.Tot_examt,12,2)) + ;
					",act_invamt = act_invamt - " + Alltrim(Str(main_vw.net_amt,12,2)) + ", eo_invamt = eo_invamt - " + Alltrim(Str(main_vw.aa_eoinvamt,12,2))+;
					" where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"
			Endif
		Endif
		* Changes done by EBS Product Team on 27022013
		* End ---> PR_AA_00001_Import_Purchase_AA
	Else

		* AA Changes done as per --> PR_AA_00002_Export_Sales_EO_AA
		* Changes done by EBS Product Team on 04022013

		If main_vw.entry_ty = "EI"
			* Changes done by EBS Product Team on 27022013
			If Type("main_vw.aa_invamt") <> 'U' And Type("main_vw.aalic_no") <> 'U'
				lcSqlStr = "Update aa_mast set exp_invamt = exp_invamt - " + Alltrim(Str(main_vw.net_amt,12,2)) +;
					" where licen_no = '" + Alltrim(main_vw.aalic_no) + "'"
			Else
				If Type("lmc_vw.aa_invamt") <> 'U' And Type("lmc_vw.aalic_no") <> 'U'
					lcSqlStr = "Update aa_mast set exp_invamt = exp_invamt - " + Alltrim(Str(main_vw.net_amt,12,2)) +;
						" where licen_no = '" + Alltrim(lmc_vw.aalic_no) + "'"
				Endif
			Endif
		Endif
		* Changes done by EBS Product Team on 27022013
		* End ---> PR_AA_00002_Export_Sales_EO_AA
	Endif
	If !Empty(lcSqlStr)
		nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_curscrobj.nHandle",_curscrobj.DataSessionId,.T.)
		If nretval < 0
			nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
			Select(lOldArea)
			Return .F.
		Endif
	Endif
	Return .T.
Endproc

* End ---> PR_AA_00001_Import_Purchase_AA

* EPCG Changes done as per --> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO
* Changes done by EBS Product Team on 05022013
Procedure EPCGAddEditProc
	_curscrobj = _Screen.ActiveForm
	If _curscrobj.addmode = .T. Or _curscrobj.editmode = .T.
		lcSqlStr = ""
		Store 0 To mDutySaved,mTotEO
		Select Sum(basic_eo) As sumbasiceo,Sum(duty_saved) As sumdutysaved From item_vw With (Buffering = .T.) ;
			INTO Cursor cursumitem_vw

		*!*		MESSAGEBOX("sumbasiceo = "+STR(cursumitem_vw.sumbasiceo,12,2)+CHR(13)+"sumdutysaved = "+STR(cursumitem_vw.sumdutysaved,12,2)+;
		*!*			CHR(13)+"_curscrobj.oTranDtySaved = "+STR(_curscrobj.oTranDtySaved,12,2)+;
		*!*			CHR(13)+"lmc_vw.duty_saved = "+STR(IIF(TYPE("main_vw.duty_saved")<>"U",main_vw.duty_saved,lmc_vw.duty_saved),12,2)+;
		*!*			CHR(13)+"lmc_vw.tot_eo = "+STR(IIF(TYPE("main_vw.tot_eo")<>"U",main_vw.tot_eo,lmc_vw.tot_eo),12,2)+;
		*!*			CHR(13)+"_curscrobj.oTranItemDtySaved = "+STR(_curscrobj.oTranItemDtySaved,12,2)+;
		*!*			CHR(13)+"_curscrobj.oTranItemTotEO = "+STR(_curscrobj.oTranItemTotEO,12,2)+;
		*!*			CHR(13)+"_curscrobj.oTranTotEO = "+STR(_curscrobj.oTranTotEO,12,2))

		Do Case
			Case (_curscrobj.addmode = .T. And _curscrobj.editmode = .F.) Or ((_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) ;
					AND _curscrobj.oEPCGTranDtySaved = 0 And _curscrobj.oEPCGTranTotEO = 0)

				lcSqlStr = "Update epcg_mast set duty_saved = duty_saved + " + Alltrim(Str(lmc_vw.duty_saved,12,2)) +;
					", tot_eo = tot_eo + " + Alltrim(Str(lmc_vw.tot_eo,12,2)) + " Where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"

			Case (_curscrobj.addmode = .F. And _curscrobj.editmode = .T.) And _curscrobj.oEPCGTranDtySaved > 0 And _curscrobj.oEPCGTranTotEO > 0

				Do Case
					Case !Empty(_curscrobj.oEPCGLicenseNo) And Alltrim(_curscrobj.oEPCGLicenseNo) <> Alltrim(lmc_vw.licen_no) ;
							AND _curscrobj.oEPCGTranDtySaved = cursumitem_vw.sumdutysaved And _curscrobj.oEPCGTranTotEO = cursumitem_vw.sumbasiceo

						lcSqlStr = " Update epcg_mast set duty_saved = duty_saved - " + Alltrim(Str(lmc_vw.duty_saved,12,2)) +;
							", tot_eo = tot_eo - " + Alltrim(Str(lmc_vw.tot_eo,12,2)) + " Where licen_no = '" + Alltrim(_curscrobj.oEPCGLicenseNo) + "'"

						lcSqlStr = lcSqlStr + " Update epcg_mast set duty_saved = duty_saved + " + Alltrim(Str(lmc_vw.duty_saved,12,2)) +;
							", tot_eo = tot_eo + " + Alltrim(Str(lmc_vw.tot_eo,12,2)) + " Where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"

					Case !Empty(_curscrobj.oEPCGLicenseNo) And Alltrim(_curscrobj.oEPCGLicenseNo) = Alltrim(lmc_vw.licen_no) ;
							AND _curscrobj.oEPCGTranItemDtySaved <> cursumitem_vw.sumdutysaved And _curscrobj.oEPCGTranItemTotEO <> cursumitem_vw.sumbasiceo

						lcSqlStr = " Update epcg_mast set duty_saved = duty_saved - " + Alltrim(Str(_curscrobj.oEPCGTranItemDtySaved,12,2)) +;
							", tot_eo = tot_eo - " + Alltrim(Str(_curscrobj.oEPCGTranItemTotEO,12,2)) + " Where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"

						lcSqlStr = lcSqlStr + " Update epcg_mast set duty_saved = duty_saved + " + Alltrim(Str(lmc_vw.duty_saved,11,2)) +;
							", tot_eo = tot_eo + " + Alltrim(Str(lmc_vw.tot_eo,12,2)) + " Where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"

					Case !Empty(_curscrobj.oEPCGLicenseNo) And Alltrim(_curscrobj.oEPCGLicenseNo) <> Alltrim(lmc_vw.licen_no) ;
							AND _curscrobj.oEPCGTranDtySaved <> cursumitem_vw.sumdutysaved And _curscrobj.oEPCGTranTotEO <> cursumitem_vw.sumbasiceo

						lcSqlStr = " Update epcg_mast set duty_saved = duty_saved - " + Alltrim(Str(_curscrobj.oEPCGTranDtySaved,12,2)) +;
							", tot_eo = tot_eo - " + Alltrim(Str(_curscrobj.oEPCGTranTotEO,12,2)) + " Where licen_no = '" + Alltrim(_curscrobj.oEPCGLicenseNo) + "'"

						lcSqlStr = lcSqlStr + " Update epcg_mast set duty_saved = duty_saved + " + Alltrim(Str(lmc_vw.duty_saved,12,2)) +;
							", tot_eo = tot_eo + " + Alltrim(Str(lmc_vw.tot_eo,12,2)) + " Where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"

				Endcase
		Endcase

		If !Empty(lcSqlStr)
			lcSqlStr = lcSqlStr + " Update epcg_mast set duty_saved_fc = duty_saved * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ;
				", tot_eo_fc = tot_eo * " + Alltrim(Str(main_vw.fcexrate,12,2)) + ", fcid = " + Alltrim(Str(main_vw.fcid)) +;
				" where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"
		Endif

		nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_curscrobj.nHandle",_curscrobj.DataSessionId,.T.)
		If nretval < 0
			nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
			Select(lOldArea)
			Return .F.
		Endif
	Endif

	Return .T.
Endproc

Procedure EPCGDeleteProc
	_curscrobj = _Screen.ActiveForm
	lcSqlStr = ""

	If !Used("cursumitem_vw")
		Select Sum(basic_eo) As sumbasiceo,Sum(duty_saved) As sumdutysaved From item_vw With (Buffering = .T.) ;
			INTO Cursor cursumitem_vw
	Endif

	If Type("main_vw.tot_eo") <> 'U' And Type("main_vw.duty_saved") <> 'U' And Type("main_vw.licen_no") <> 'U'
		lcSqlStr = "update epcg_mast set duty_saved = duty_saved - " + Alltrim(Str(cursumitem_vw.sumdutysaved,12,2)) +;
			", tot_eo = tot_eo - " + Alltrim(Str(cursumitem_vw.sumbasiceo,12,2)) + " where licen_no = '" + Alltrim(main_vw.licen_no) + "'"
	Else
		If Type("lmc_vw.tot_eo") <> 'U' And Type("lmc_vw.duty_saved") <> 'U' And Type("lmc_vw.licen_no") <> 'U'
			lcSqlStr = "update epcg_mast set duty_saved = duty_saved -" + Alltrim(Str(cursumitem_vw.sumdutysaved,12,2)) +;
				", tot_eo = tot_eo -" + Alltrim(Str(cursumitem_vw.sumbasiceo,12,2)) + " where licen_no = '" + Alltrim(lmc_vw.licen_no) + "'"
		Endif
	Endif

	nretval = _curscrobj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_curscrobj.nHandle",_curscrobj.DataSessionId,.T.)
	If nretval < 0
		nretval = _curscrobj.sqlconobj.sqlconnclose("_curscrobj.nHandle")
		Select(lOldArea)
		Return .F.
	Endif
	Return .T.
Endproc

* End ---> PR_EPCG_00001_Import_Purchase_Duty_Saved_EO

* Changes done as per --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference
* Changes done by EBS Product Team
Procedure CancelPartyProc
	_currObj = _Screen.ActiveForm
	lcSqlStr = "Update Export_LC_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(_currObj.oPrevInvAmt,11,2)) +;
		" Where LC_No = '" + Alltrim(_currObj.oLCNo) + "'"

	nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_currObj.nHandle",_currObj.DataSessionId,.T.)
	If nretval < 0
		nretval = _currObj.sqlconobj.sqlconnclose("_currObj.nHandle")
		Select(lOldArea)
		Return .F.
	Endif
	Return .T.
Endproc

Procedure LCBalUpdt_AddEdtProc
	_currObj = _Screen.ActiveForm
	If _currObj.addmode = .T. Or _currObj.editmode = .T.
		&& Get LC Balance Details for checking
		Wait Window "Checking LC Balance Amount..." Nowait
		If Type("main_vw.LC_No") <> 'U'
			lcSqlStr = "Select LC_No, LC_BalAmt from export_lc_mast where lc_no=?main_vw.lc_no"
		Else
			If Type("lmc_vw.LC_No") <> 'U'
				lcSqlStr = "Select LC_No, LC_BalAmt from export_lc_mast where lc_no=?lmc_vw.lc_no"
			Endif
		Endif

		nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"cur_lcno","_currObj.nHandle",_currObj.DataSessionId)
		If nretval < 0
			nretval = _currObj.sqlconobj.sqlconnclose("_currObj.nHandle")
			Select(lOldArea)
			Return .F.
		Endif

		Store .T. To lcheck
		If _currObj.addmode = .T.
			If main_vw.fcnet_amt > 0
				If (cur_lcno.LC_BalAmt < main_vw.fcnet_amt)
					lcheck = .F.
				Endif
			Endif
		Else
			If _currObj.editmode = .T.
				Store 0 To totBal
				If _currObj.oPrevInvAmt > 0
					Do Case
						Case !Empty(_currObj.oLCNo) And Alltrim(_currObj.oLCNo) <> Alltrim(main_vw.lc_no)
							totBal = cur_lcno.LC_BalAmt

						Case Empty(_currObj.oLCNo) And Alltrim(_currObj.oLCNo) <> Alltrim(main_vw.lc_no)
							totBal = cur_lcno.LC_BalAmt
						Otherwise
							totBal = cur_lcno.LC_BalAmt + _currObj.oPrevInvAmt
					Endcase
					If main_vw.fcnet_amt <> _currObj.oPrevInvAmt
						If (totBal < main_vw.fcnet_amt)
							lcheck = .F.
						Endif
					Else
						If (totBal < main_vw.fcnet_amt)
							lcheck = .F.
						Endif
					Endif
				Else
					If (cur_lcno.LC_BalAmt < main_vw.fcnet_amt)
						lcheck = .F.
					Endif
				Endif
			Endif
		Endif

		If lcheck = .F.
			Messagebox("Invoice Amount found more than LC Balance Amount."+Chr(13)+"Transaction could not be saved....",0+64,vumess)
			Select(lOldArea)
			Return .F.
		Endif

		Wait Window "Updating LC Balance Amount..." Nowait
		lcSqlStr = ""
		Do Case
			Case (_currObj.addmode = .T. And _currObj.editmode = .F.) Or ;
					((_currObj.addmode = .F. And _currObj.editmode = .T.) And _currObj.oPrevInvAmt = 0)
				lcSqlStr = " Update Export_LC_Mast set LC_BalAmt = LC_BalAmt - " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
					" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

			Case (_currObj.addmode = .F. And _currObj.editmode = .T.) And _currObj.oPrevInvAmt > 0
				&& Update old LC Balance amount, when user changes LC no. in Edit mode.
				Do Case
					Case !Empty(_currObj.oLCNo) And Alltrim(_currObj.oLCNo) <> Alltrim(main_vw.lc_no) And  _currObj.oPrevInvAmt <> main_vw.fcnet_amt
						lcSqlStr = " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(_currObj.oPrevInvAmt,11,2)) +;
							" Where LC_No = '" + Alltrim(_currObj.oLCNo) + "'"

						lcSqlStr = lcSqlStr + " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt - " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
							" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

					Case !Empty(_currObj.oLCNo) And Alltrim(_currObj.oLCNo) = Alltrim(main_vw.lc_no) And  _currObj.oPrevInvAmt <> main_vw.fcnet_amt

						lcSqlStr = " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(_currObj.oPrevInvAmt,11,2)) +;
							" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

						lcSqlStr = lcSqlStr + " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt - " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
							" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

					Case Empty(_currObj.oLCNo) And (_currObj.oPrevInvAmt = main_vw.fcnet_amt Or _currObj.oPrevInvAmt <> main_vw.fcnet_amt)
						lcSqlStr = lcSqlStr + " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt - " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
							" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

					Case !Empty(_currObj.oLCNo) And Alltrim(_currObj.oLCNo) <> Alltrim(main_vw.lc_no) And  _currObj.oPrevInvAmt = main_vw.fcnet_amt
						lcSqlStr = " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(_currObj.oPrevInvAmt,11,2)) +;
							" Where LC_No = '" + Alltrim(_currObj.oLCNo) + "'"

						lcSqlStr = lcSqlStr + " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt - " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
							" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

					Otherwise
						lcSqlStr = " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(_currObj.oPrevInvAmt,11,2)) +;
							" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

						lcSqlStr = lcSqlStr + " Update Export_Lc_Mast set LC_BalAmt = LC_BalAmt - " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
							" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"
				Endcase

		Endcase

		nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_currObj.nHandle",_currObj.DataSessionId,.T.)
		If nretval < 0
			nretval = _currObj.sqlconobj.sqlconnclose("_currObj.nHandle")
			Select(lOldArea)
			Return .F.
		Endif
	Endif
	Return .T.
Endproc

Procedure LCBalUpdt_DeleteProc
	_currObj = _Screen.ActiveForm
	lcSqlStr = "Update Export_LC_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(main_vw.fcnet_amt,11,2)) +;
		" Where LC_No = '" + Alltrim(main_vw.lc_no) + "'"

	nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_currObj.nHandle",_currObj.DataSessionId,.T.)
	If nretval < 0
		nretval = _currObj.sqlconobj.sqlconnclose("_currObj.nHandle")
		Select(lOldArea)
		Return .F.
	Endif
	Return .T.
Endproc

Procedure RemoveLCProc
	_currObj = _Screen.ActiveForm
	If !Empty(_currObj.oLCNo)
		lcSqlStr = "Update Export_LC_Mast set LC_BalAmt = LC_BalAmt + " + Alltrim(Str(_currObj.oPrevInvAmt,11,2)) +;
			" Where LC_No = '" + Alltrim(_currObj.oLCNo) + "'"

		nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,"","_currObj.nHandle",_currObj.DataSessionId,.T.)
		If nretval < 0
			nretval = _currObj.sqlconobj.sqlconnclose("_currObj.nHandle")
			Select(lOldArea)
			Return .F.
		Endif

		If Type("main_vw.FrmDocCr") <> 'U'
			Replace main_vw.frmdoccr With '' In main_vw
		Else
			If Type("lmc_vw.FrmDocCr") <> 'U'
				Replace lmc_vw.frmdoccr With '' In lmc_vw
			Endif
		Endif

		If Type("main_vw.DocCrNo") <> 'U'
			Replace main_vw.doccrno With '' In main_vw
		Else
			If Type("lmc_vw.DocCrNo") <> 'U'
				Replace lmc_vw.doccrno With '' In lmc_vw
			Endif
		Endif

		If Type("main_vw.PerInv_No") <> 'U'
			Replace main_vw.perinv_no With '' In main_vw
		Else
			If Type("lmc_vw.PerInv_No") <> 'U'
				Replace lmc_vw.perinv_no With '' In lmc_vw
			Endif
		Endif

		If Type("main_vw.PerInv_Dt") <> 'U'
			Replace main_vw.perinv_dt With {} In main_vw
		Else
			If Type("lmc_vw.PerInv_Dt") <> 'U'
				Replace lmc_vw.perinv_dt With {} In lmc_vw
			Endif
		Endif

		If Type("main_vw.DraftsAt") <> 'U'
			Replace main_vw.draftsat With '' In main_vw
		Else
			If Type("lmc_vw.DraftsAt") <> 'U'
				Replace lmc_vw.draftsat With '' In lmc_vw
			Endif
		Endif

		If Type("main_vw.u_bank") <> 'U'
			Replace main_vw.u_bank With '' In main_vw
		Else
			If Type("lmc_vw.u_bank") <> 'U'
				Replace lmc_vw.u_bank With '' In lmc_vw
			Endif
		Endif

		If Type("main_vw.u_throu1") <> 'U'
			Replace main_vw.u_throu1 With '' In main_vw
		Else
			If Type("lmc_vw.u_throu1") <> 'U'
				Replace lmc_vw.u_throu1 With '' In lmc_vw
			Endif
		Endif

		If Type("main_vw.u_throu2") <> 'U'
			Replace main_vw.u_throu2 With '' In main_vw
		Else
			If Type("lmc_vw.u_throu2") <> 'U'
				Replace lmc_vw.u_throu2 With '' In lmc_vw
			Endif
		Endif

		If Type("main_vw.u_throu3") <> 'U'
			Replace main_vw.u_throu3 With '' In main_vw
		Else
			If Type("lmc_vw.u_throu3") <> 'U'
				Replace lmc_vw.u_throu3 With '' In lmc_vw
			Endif
		Endif

		&&added by priyanka_28052013
		If Type("main_vw.u_terms") <> 'U'
			Replace main_vw.u_terms With '' In main_vw
		Else
			If Type("lmc_vw.u_terms") <> 'U'
				Replace lmc_vw.u_terms With '' In lmc_vw
			Endif
		Endif
		&&added by priyanka_28052013
	Endif
	Return .T.
Endproc
* End --> CR_KOEL_0002_Export_Sales_Transaction_&_Export_LC_Master_Cross_Reference


* Changes done as per --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
* Date : 08/11/2012
* Changes done by EBS Product Team

Procedure ProcDelete
	_cursobj = _Screen.ActiveForm
	Wait Window + 'Checking existing Pre Shipment Information Details.' Nowait
	Select main_vw
	Go Top
	Sq1= " Delete From PreShipInfo Where Entry_Ty = ?Main_Vw.Entry_Ty and Tran_cd = ?Main_Vw.Tran_Cd " &&and itserial in ("+List_Itsr+")"
	nretval = 1
	nretval = _cursobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"","_cursobj.nHandle",_cursobj.DataSessionId)
	If nretval<=0
		Wait Clear
		Messagebox("Unable to Delete Pre Shipment Information Details from Database..!!",16,vumess)
		Return .F.
	Endif
	Wait Clear
	Return .T.
Endproc

Procedure ProcSave
	_cursobj = _Screen.ActiveForm
	Wait Window + 'Updating Pre shipment Info. Details, Please wait' Nowait
	If Used('Tbl_PreShipment_Vw')
		_Tally=0
		Select * From Tbl_PreShipment_Vw Into Cursor cursRec
		If _Tally > 0
			Replace All entry_ty With main_vw.entry_ty,;
				tran_cd	With main_vw.tran_cd In Tbl_PreShipment_Vw

			Create Cursor Tbl_SqlString(cmdInsStr Memo,cmdDelStr Memo,cmdFinStr Memo)   && Create temporary cursor for string
			Append Blank In Tbl_SqlString

			Store "" To InsStr,DelStr

			Select item_vw
			Go Top
			Do While !Eof("Item_vw")
				Ds_Itsr = item_vw.itserial

				Replace cmdDelStr With cmdDelStr +;
					" Delete from PreShipInfo where entry_ty ='" + Alltrim(item_vw.entry_ty) +;
					"' and tran_cd =" + Alltrim(Str(item_vw.tran_cd)) + " and itserial ='" + Alltrim(item_vw.itserial) +"'" ;
					IN Tbl_SqlString


				Select Tbl_PreShipment_Vw
				Go Top
				Scan For itserial = Ds_Itsr
					Wait Window + "Updating data for Serial:- "+Transform(Tbl_PreShipment_Vw.Serial) Nowait

					For fieldcount = 1 To Fcount("Tbl_PreShipment_Vw") Step 1
						If Inlist(Field(fieldcount,"Tbl_PreShipment_Vw"),"SRNO","KVA","ENGINECD","CASENO","DIMENSION","LORRYNO","CONTAINNO") ;
								OR ;
								INLIST(Field(fieldcount,"Tbl_PreShipment_Vw"),"CONTAINTYPE","LINESEALNO","EXCSEALNO")
							fxStr = "Replace " + Field(fieldcount,"Tbl_PreShipment_Vw") + " WITH [" + RepQuotes(Evaluate(Field(fieldcount,"Tbl_PreShipment_Vw"))) + ;
								"] IN Tbl_PreShipment_Vw "

							&fxStr
						Endif
					Endfor


					Replace cmdInsStr With cmdInsStr + " Insert into PreShipInfo (entry_ty,tran_cd,itserial,srno,kva,enginecd,caseno,pckqty,nocase,Dimension,grsweight,netweight,lorryno,containno,containtype,linesealno,excsealno,dispadvno,remark) values('" +;
						ALLTRIM(Tbl_PreShipment_Vw.entry_ty) + "',"+;
						ALLTRIM(Str(Tbl_PreShipment_Vw.tran_cd)) + ",'"+;
						ALLTRIM(Tbl_PreShipment_Vw.itserial) + "','"+;
						ALLTRIM(Tbl_PreShipment_Vw.srno) + "','"+;
						ALLTRIM(Tbl_PreShipment_Vw.kva) + "','"+;
						ALLTRIM(Tbl_PreShipment_Vw.enginecd) + "','"+;
						ALLTRIM(Tbl_PreShipment_Vw.caseno) + "',"+;
						ALLTRIM(Str(Tbl_PreShipment_Vw.pckqty,18,3))+","+;
						ALLTRIM(Str(Tbl_PreShipment_Vw.nocase))+",'"+;
						ALLTRIM(Tbl_PreShipment_Vw.Dimension)+"',"+;
						ALLTRIM(Str(Tbl_PreShipment_Vw.grsweight,18,3))+","+;
						ALLTRIM(Str(Tbl_PreShipment_Vw.netweight,18,3))+",'"+;
						ALLTRIM(Tbl_PreShipment_Vw.lorryno) + "','"+;
						ALLTRIM(Tbl_PreShipment_Vw.containno) + "','"+;
						ALLTRIM(Tbl_PreShipment_Vw.containtype) + "','"+;
						ALLTRIM(Tbl_PreShipment_Vw.linesealno) + "','"+;
						ALLTRIM(Tbl_PreShipment_Vw.excsealno) + "','"+;
						ALLTRIM(Tbl_PreShipment_Vw.dispadvno) + "','"+;
						ALLTRIM(Tbl_PreShipment_Vw.remark)+"')";   && Changes done as per --> CR_KOEL_00005A_Form_To_Record_Pre_Shipment_Info_1.2 on Dated 06/03/2013
					In Tbl_SqlString

				Endscan
				Select item_vw
				Skip
			Enddo

			Replace cmdFinStr With cmdDelStr + cmdInsStr In Tbl_SqlString

			nretval = _cursobj.sqlconobj.dataconn([EXE],company.dbname,Tbl_SqlString.cmdFinStr,"","_cursobj.nHandle",_cursobj.DataSessionId,.T.)
			If nretval<=0
				Wait Clear
				Messagebox("Unable to update Pre shipment information details in database..!!",16,vumess)
				Return .F.
			Endif
		Endif
		Use In Tbl_PreShipment_Vw

		If Used("Tbl_SqlString")
			Use In  Tbl_SqlString
		Endif

		If Used("cursRec")
			Use In cursRec
		Endif
		Wait Clear
		Return .T.
	Endif
Endproc

Procedure RepQuotes
	Parameters sString
	sString = Iif(Isnull(sString),"",sString)
	Return Alltrim(Strtran(sString,"'","''"))
Endproc
*End --> CR_KOEL_0005A_Form_To_Record_Pre_Shipment_Info
&& changes by EBS team on 07/03/14 for Bug-21466,21467,21468 end

*!*	Return Iif(etsql_con < 1,.F.,.T.)			&& Commented by shrikant S. on 29/09/2014 for Bug-23879

*!*	** Start : Added by Uday on 29/04/2014 for Bug-21965			&& Bug-30904
Procedure ProcMLASave

	If _currObj.editmode = .T. And _currObj.addmode = .F.
		Do ProcDoReject
	Else
		If _currObj.addmode = .T. And _currObj.editmode = .F.
			Do ProcDoGenerateApproval
		Endif
	Endif
Endproc

Procedure ProcMLADelete
	_cursobj = _Screen.ActiveForm

	Sq1 = "SELECT id FROM applevel_dtl " +;
		"where entry_ty = ?Main_Vw.Entry_Ty  and tran_cd = ?Main_Vw.Tran_Cd and levelstatus ='Approved'"

	nretval = _cursobj.sqlconobj.dataconn("EXE",company.dbname,Sq1,"Cursdel","_cursobj.nhandle",_cursobj.DataSessionId)
	If nretval<=0
		Wait Clear
		Messagebox("Unable to Delete Data..!!",16,vumess)
		Return .F.
	Endif

	If Used("cursdel")
		If Reccount("cursdel") <> 0
			Use In cursdel
			Messagebox("Cannot delete this transaction, Transaction has been approved already.....",vumess,0+16)
			Return .F.
		Endif
		Use In cursdel
	Endif

	Select main_vw
	Go Top
	Sq1= " delete from applevel_dtl where entry_ty = ?main_vw.entry_ty and tran_cd = ?main_vw.tran_cd " &&and itserial in ("+List_Itsr+")"
	nretval = 1
	nretval = _cursobj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"","_cursobj.nHandle",_cursobj.DataSessionId)
	If nretval<=0
		Wait Clear
		Messagebox("Unable to Delete Details from Database..!!",16,vumess)
		Return .F.
	Endif
	Wait Clear
	Return .T.
Endproc


Procedure ProcDoReject()
	Store "" To Sq1
	If lcode_vw.applevel_init = .T.
		Sq1= " Select id from applevel_dtl where entry_ty = ?Main_vw.Entry_ty and Tran_cd = ?Main_Vw.Tran_Cd and initiator_id =?Main_vw.User_name "+;
			" and levelstatus ='Rejected'"
	Else
		Sq1= " Select id from applevel_dtl where entry_ty = ?Main_vw.Entry_ty and Tran_cd = ?Main_Vw.Tran_Cd "+;
			" and levelstatus ='Rejected'"
	Endif

	nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursrejappleveldtl","_currObj.nHandle",_currObj.DataSessionId)
	If nretval<=0
		Wait Clear
		Messagebox("Unable to get Details from Database..!!",16,vumess)
		Return .F.
	Endif

	Store .F. To IsFound
	If Used("cursrejappleveldtl")
		If Reccount("cursrejappleveldtl") > 0
			IsFound = .T.
		Endif
	Endif

	If IsFound = .F.
		Use In cursrejappleveldtl
		Return
	Endif

	Store "" To Sq1
	If lcode_vw.applevel_init = .T.
		Sq1= " Select * from applevel_dtl where entry_ty = ?Main_vw.Entry_ty and Tran_cd = ?Main_Vw.Tran_Cd and initiator_id =?Main_vw.User_name "
	Else
		Sq1= " Select * from applevel_dtl where entry_ty = ?Main_vw.Entry_ty and Tran_cd = ?Main_Vw.Tran_Cd "
	Endif

	nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursappleveldtl","_currObj.nHandle",_currObj.DataSessionId)
	If nretval<=0
		Wait Clear
		Messagebox("Unable to get Details from Database..!!",16,vumess)
		Return .F.
	Endif

	Create Cursor Tbl_SqlString(cmdInsStr Memo,cmdUpdateStr Memo,cmdFinStr Memo)   && Create temporary cursor for string
	Append Blank In Tbl_SqlString

	Go Top In cursappleveldtl
	Do While !Eof("cursappleveldtl")
		Replace cmdInsStr With cmdInsStr +;
			" insert into applevel_hist(entry_ty,tran_cd,applevel,initiator_id,user_role,levelstatus,prelevelstatus,add_user,add_dt) values ('" +;
			cursappleveldtl.entry_ty + "'," +;
			STR(main_vw.tran_cd) + "," +;
			STR(cursappleveldtl.applevel) + ",'" +;
			ALLTRIM(cursappleveldtl.initiator_id) + "','" +;
			ALLTRIM(cursappleveldtl.user_role) + "','" + ;
			ALLTRIM(cursappleveldtl.levelstatus) + "','" +;
			ALLTRIM(cursappleveldtl.prelevelstatus) + "','" +;
			ALLTRIM(mUsername) +"','" +;
			main_vw.sysdate + "')" In Tbl_SqlString

		If (cursappleveldtl.levelstatus = "Rejected")
			Exit
		Endif

		Skip In cursappleveldtl
	Enddo

	If !Empty(Tbl_SqlString.cmdInsStr)
		Replace cmdFinStr With "set dateformat dmy "+cmdInsStr In Tbl_SqlString

		nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,Tbl_SqlString.cmdFinStr,"","_currObj.nHandle",_currObj.DataSessionId,.T.)
		If nretval<=0
			Wait Clear
			Messagebox("Unable to update details in database..!!",16,vumess)
			Return .F.
		Endif
	Endif

	Replace cmdInsStr With "",;
		cmdUpdateStr With "",;
		cmdFinStr With "" In Tbl_SqlString

	If lcode_vw.applevel_init = .T.
		Replace cmdUpdateStr With cmdUpdateStr + "update applevel_dtl set approved_remarks ='',rejected_remarks='',levelstatus='',user_role='' where entry_ty='" + Alltrim(main_vw.entry_ty) + "' and " +;
			"tran_cd=" + Alltrim(Str(main_vw.tran_cd)) + " and " +;
			"initiator_id='" + Alltrim(main_vw.user_name) + "'" In Tbl_SqlString

		Replace cmdUpdateStr With cmdUpdateStr + "update applevel_dtl set prelevelstatus ='',prelevel=0 where entry_ty='" + Alltrim(main_vw.entry_ty) + "' and " +;
			"tran_cd=" + Alltrim(Str(main_vw.tran_cd)) + " and " +;
			"initiator_id='" + Alltrim(main_vw.user_name) + "' and prelevel !=0" In Tbl_SqlString

		Replace cmdUpdateStr With cmdUpdateStr + "update applevel_dtl set edit_user ='"+Alltrim(mUsername)+"',edit_dt='"+main_vw.sysdate+"' where entry_ty='" + Alltrim(main_vw.entry_ty) + "' and " +;
			"tran_cd=" + Alltrim(Str(main_vw.tran_cd)) + " and " +;
			"initiator_id='" + Alltrim(main_vw.user_name) + "'" In Tbl_SqlString

	Else
		Replace cmdUpdateStr With cmdUpdateStr + "update applevel_dtl set approved_remarks ='',rejected_remarks='',levelstatus='',user_role='' where entry_ty='" + Alltrim(main_vw.entry_ty) + "' and " +;
			"tran_cd=" + Alltrim(Str(main_vw.tran_cd)) In Tbl_SqlString

		Replace cmdUpdateStr With cmdUpdateStr + "update applevel_dtl set prelevelstatus ='',prelevel=0 where entry_ty='" + Alltrim(main_vw.entry_ty) + "' and " +;
			"tran_cd=" + Alltrim(Str(main_vw.tran_cd)) + " and " +;
			"initiator_id='" + Alltrim(main_vw.user_name) + "' and prelevel !=0" In Tbl_SqlString

		Replace cmdUpdateStr With cmdUpdateStr + "update applevel_dtl set edit_user ='"+Alltrim(mUsername)+"',edit_dt='"+main_vw.sysdate+"' where entry_ty='" + Alltrim(main_vw.entry_ty) + "' and " +;
			"tran_cd=" + Alltrim(Str(main_vw.tran_cd)) + " and " +;
			"initiator_id='" + Alltrim(main_vw.user_name) + "'" In Tbl_SqlString
	Endif

	If !Empty(Tbl_SqlString.cmdUpdateStr)
		Replace cmdFinStr With "set dateformat dmy "+cmdUpdateStr In Tbl_SqlString

		nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,Tbl_SqlString.cmdFinStr,"","_currObj.nHandle",_currObj.DataSessionId,.T.)
		If nretval<=0
			Wait Clear
			Messagebox("Unable to update details in database..!!",16,vumess)
			Return .F.
		Endif
	Endif

	If Used("cursrejappleveldtl")
		Use In cursrejappleveldtl
	Endif

	If Used("cursappleveldtl")
		Use In cursappleveldtl
	Endif

	If Used("Tbl_SqlString")
		Use In Tbl_SqlString
	Endif
Endproc


Procedure ProcDoGenerateApproval

	Store "" To Sq1
	If lcode_vw.applevel_init = .T.
		Sq1= " Select id from applevel_dtl where entry_ty = ?Main_vw.Entry_ty and Tran_cd = ?Main_Vw.Tran_Cd and initiator_id =?Main_vw.User_name"
	Else
		Sq1= " Select id from applevel_dtl where entry_ty = ?Main_vw.Entry_ty and Tran_cd = ?Main_Vw.Tran_Cd "
	Endif

	nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursappleveldtl","_currObj.nHandle",_currObj.DataSessionId)
	If nretval<=0
		Wait Clear
		Messagebox("Unable to get Details from Database..!!",16,vumess)
		Return .F.
	Endif

	Store .F. To IsFound
	If Used("cursappleveldtl")
		If Reccount("cursappleveldtl") > 0
			IsFound = .T.
		Endif
	Endif

	If Used("cursappleveldtl")
		Use In cursappleveldtl
	Endif

	If IsFound = .F.
		&& Customization
		Store "" To mlacond
		If File("udappvoufinalupdate_mlacond.fxp")
			mlacond = udappvoufinalupdate_mlacond()   && Customization in conditions
		Endif

		*!*		STORE "" TO clevel
		*!*		DO case
		*!*		CASE lmc_vw.u_mattype = "Returnable" AND lmc_vw.u_excisety = "Non-Excisable"
		*!*			clevel = "4,1"
		*!*		CASE lmc_vw.u_mattype = "Returnable" AND lmc_vw.u_excisety = "Excisable"
		*!*			clevel = "4,2,1"
		*!*		CASE lmc_vw.u_mattype = "Non-Returnable" AND lmc_vw.u_excisety = "Non-Excisable"
		*!*			clevel = "4,3,1"
		*!*		CASE lmc_vw.u_mattype = "Non-Returnable" AND lmc_vw.u_excisety = "Excisable"
		*!*			clevel = "4,3,2,1"
		*!*		ENDCASE
		* End
		* Sq1= " Select entry_ty,applevel,initiator_id from applevel_mstr where entry_ty = ?Main_vw.Entry_ty and initiator_id =?Main_vw.User_name "+IIF(!EMPTY(CLEVEL)," AND APPLEVEL IN ("+CLEVEL+")","")+" order by applevel desc"

		Store "" To Sq1
		If lcode_vw.applevel_init = .T.
			Sq1= " Select entry_ty,applevel,initiator_id from applevel_mstr where entry_ty = ?Main_vw.Entry_ty and initiator_id =?Main_vw.User_name "+Iif(!Empty(mlacond)," AND " + mlacond + "","")+" order by applevel desc"
		Else
			Sq1= " Select entry_ty,applevel,initiator_id from applevel_mstr where entry_ty = ?Main_vw.Entry_ty "+Iif(!Empty(mlacond)," AND " + mlacond +"","")+" order by applevel desc"
		Endif

		nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,Sq1,"cursapplevelmstr","_currObj.nHandle",_currObj.DataSessionId)
		If nretval<=0
			Wait Clear
			Messagebox("Unable to get Details from Database..!!",16,vumess)
			Return .F.
		Endif

		Create Cursor Tbl_SqlString(cmdInsStr Memo,cmdDelStr Memo,cmdFinStr Memo)   && Create temporary cursor for string
		Append Blank In Tbl_SqlString

		Store 0 To fstlevel,m_applevel
		Do While !Eof("cursapplevelmstr")
			If fstlevel = 0
				fstlevel = cursapplevelmstr.applevel
			Endif

			If (cursapplevelmstr.applevel == m_applevel)
				Skip In cursapplevelmstr
				Loop
			Endif

			Replace cmdInsStr With cmdInsStr +;
				"insert into applevel_dtl(entry_ty,tran_cd,applevel,initiator_id,user_role,levelstatus,prelevelstatus,add_user,add_dt) values ('" +;
				cursapplevelmstr.entry_ty + "'," +;
				STR(main_vw.tran_cd) + "," +;
				STR(cursapplevelmstr.applevel) + ",'" +;
				ALLTRIM(cursapplevelmstr.initiator_id) + "'," +;
				"''," +;
				"'','" +;
				IIF(cursapplevelmstr.applevel = fstlevel,"Approved","") + "','" +;
				ALLTRIM(mUsername) +"','" +;
				main_vw.sysdate + "')" In Tbl_SqlString

			m_applevel =  cursapplevelmstr.applevel
			Skip In cursapplevelmstr
		Enddo

		If !Empty(Tbl_SqlString.cmdInsStr)
			Replace cmdFinStr With "set dateformat dmy "+cmdInsStr In Tbl_SqlString

			nretval = _currObj.sqlconobj.dataconn([EXE],company.dbname,Tbl_SqlString.cmdFinStr,"","_currObj.nHandle",_currObj.DataSessionId,.T.)
			If nretval<=0
				Wait Clear
				Messagebox("Unable to update details in database..!!",16,vumess)
				Return .F.
			Endif
		Endif

		If Used("cursapplevelmstr")
			Use In cursapplevelmstr
		Endif

		If Used("Tbl_SqlString")
			Use In Tbl_SqlString
		Endif
	Endif
	Return .T.
Endproc
*!*	** End : Added by Uday on 29/04/2014 for Bug-21965			&& Bug-30904

&&Added by Shrikant S. on 28012019 for Bug-32210 Start
Procedure getAccountName
	Parameters _acid,oactform

	msqlstr="SELECT TOP 1 Ac_name FROM Ac_mast WHERE Ac_id = ?_acid"
	mretval = oactform.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"__acname","oactform.nhandle",oactform.DataSessionId,.T.)
	If mretval<=0
		Return .F.
	Endif
	Return __acname.ac_name

Procedure getAccountId
	Parameters _acname,oactform

	msqlstr="SELECT TOP 1 Ac_id FROM Ac_mast WHERE Ac_name=?_acname"
	mretval = oactform.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"__acid","oactform.nhandle",oactform.DataSessionId,.T.)
	If mretval<=0
		Return .F.
	Endif
	Return __acid.ac_id


Procedure update_itbal
	Parameters lcgenEnTbl,oactform

	mconditbal='It_code = ?ExItem_vw.It_code'
	msqlstr="SELECT TOP 1 * FROM It_bal WHERE It_code = ?ExItem_vw.it_code"
	mretval = oactform.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"It_bal_vw","oactform.nhandle",oactform.DataSessionId,.T.)
	If mretval<=0
		Return .F.
	Endif
	Select It_bal_vw
	edit_it_bal=.T.
	If Reccount('It_bal_vw')=0
		edit_it_bal = .F.
		Append Blank
		Replace it_code With exItem_vw.it_code In It_bal_vw
	Endif
	Go Top In It_bal_vw
	lcQtyfld = lcgenEnTbl+"Qty"
	lnOldqty = &lcQtyfld
	Replace (lcQtyfld) With lnOldqty+exItem_vw.Qty In It_bal_vw

	If !edit_it_bal
		mfldupdt = "It_code,"+lcgenEnTbl+"qty"
	Else
		mfldupdt = lcgenEnTbl+"Qty"
	Endif
	If !edit_it_bal
		msqlstr = oactform.sqlconobj.geninsert("it_bal","","","It_bal_vw",mvu_backend,mfldupdt)
	Else
		msqlstr = oactform.sqlconobj.GenUpdate("it_bal","'It_code'","","it_bal_vw",mvu_backend,mconditbal,mfldupdt)
	Endif
	mretval = oactform.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","oactform.nhandle",oactform.DataSessionId,.T.)
	If mretval<=0
		Return .F.
	Endif
	Return .T.
Endproc

Procedure update_itbalw
	Parameters lcgenEnTbl,oactform

	Go Top In exMain_vw
	mconditbal="It_code=?ExItem_vw.It_code AND Entry_ty=?ExMain_vw.Entry_ty AND Ware_nm = ?ExItem_vw.Ware_nm "
	mconditbal=mconditbal+" AND Date = ?ExMain_vw.Date AND [RULE] =''"
	msqlstr="SELECT Top 1 * FROM It_balw WHERE "+mconditbal
	mretval = oactform.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"It_balw_vw","oactform.nhandle",oactform.DataSessionId,.T.)
	If mretval<=0
		Return .F.
	Endif
	Select It_balw_vw
	edit_it_bal=.T.
	If Reccount('It_balw_vw')=0
		edit_it_bal = .F.
		Append Blank
		Replace it_code With exItem_vw.it_code In It_balw_vw
	Endif
	Go Top In It_balw_vw
	Replace entry_ty With exMain_vw.entry_ty In It_balw_vw
	Replace Ware_nm With exItem_vw.Ware_nm In It_balw_vw
	If It_balw_vw.Date < exMain_vw.Date
		Replace Date With exMain_vw.Date In It_balw_vw
	Endif
	If Type("It_balw_vw.Rule") <> "U"
		If !Empty(exMain_vw.Rule)
			Replace Rule With exMain_vw.Rule In It_balw_vw
		Endif
	Endif
	Replace Qty With It_balw_vw.Qty+exItem_vw.Qty In It_balw_vw
	If !edit_it_bal
		msqlstr = oactform.sqlconobj.geninsert("it_balw","","","It_balw_vw",mvu_backend)
	Else
		msqlstr = oactform.sqlconobj.GenUpdate("it_balw","'It_code'",,"It_balw_vw",mvu_backend,mconditbal)
	Endif
	mretval = oactform.sqlconobj.dataconn("EXE",company.dbname,msqlstr,"","oactform.nhandle",oactform.DataSessionId,.T.)
	If mretval<=0
		Return .F.
	Endif
	Return .T.

Endproc

Procedure Get_gstrate
	Parameters _Entry,_macid,_sacid,_consid,_sconsid,_state,_statecode,_itcode,_datasessionid

	If Alltrim(coadditional.optCompo)="Yes"
		Return
	Endif

	mac_id=_macid
	mstateid=""
	mhsnid=0

	lcSqlStr="select top 1 state,gstin,st_type,supp_type,Statecode,Country from ac_mast where Ac_id=?mac_id"

	If Type('_sacid')<>'U'
		If _sacid >0
			lcSqlStr="select top 1 state,gstin,st_type,supp_type,Statecode,Country from shipto where Ac_id=?mac_id and Shipto_id=?_sacid"
		Else
			lcSqlStr="select top 1 state,gstin,st_type,supp_type,Statecode,Country from ac_mast where Ac_id=?mac_id"
		Endif
	Endif

	If Type('_sconsid')<>'U'
		If _consid > 0
			mac_id =_consid
			If _consid >0
				lcSqlStr="select top 1 state,gstin,st_type,supp_type,Statecode,Country from shipto where Ac_id=?mac_id and Shipto_id=?_sconsid"
			Else
				lcSqlStr="select top 1 state,gstin,st_type,supp_type,Statecode,Country from ac_mast where Ac_id=?mac_id "
			Endif
		Endif
	Endif

	llthirdparty=.F.
	If Type('_consid')<>'U' And lcode_vw.IoTrans=2
		If _consid > 0
			If _macid <> _consid
				llthirdparty=CheckThirdParty_Exists(_macid,_consid)
				If llthirdparty=.T.
					If Type('_sacid')<>'U'
						If _sacid >0
							lcSqlStr="select top 1 state,gstin,st_type,supp_type,Statecode,Country from shipto where Ac_id=?_macid and Shipto_id=?_sacid"
						Else
							lcSqlStr="select top 1 state,gstin,st_type,supp_type,Statecode,Country from ac_mast where Ac_id=?_macid"
						Endif
					Else
						lcSqlStr="select top 1 state,gstin,st_type,supp_type,Statecode,Country from ac_mast where Ac_id=?_macid"
					Endif
				Endif
			Endif
		Endif
	Endif

	nRet = _curform.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,[tmpgst],"_curform.nHandle",_curform.DataSessionId,.T.)
	If nRet <= 0
		Return .F.
	Endif

	If llthirdparty
		Replace statecode With tmpgst.statecode In exMain_vw
	Else
		If !Empty(_state)
			Replace state With _state,statecode With _statecode In tmpgst
		Endif
	Endif

	If Alltrim(tmpgst.country)<>Alltrim(company.country)
		Replace st_type With "OUT OF COUNTRY" In tmpgst
	Else
		Do Case
			Case Alltrim(tmpgst.state) != Allt(company.state) And Alltrim(tmpgst.country)=Alltrim(company.country)
				Replace st_type With "INTERSTATE" In tmpgst
			Case Alltrim(tmpgst.state) = Alltrim(company.state) And Alltrim(tmpgst.country)=Alltrim(company.country)
				Replace st_type With "INTRASTATE" In tmpgst
		Endcase
	Endif


	If Alltrim(tmpgst.st_type)=="OUT OF COUNTRY"
		Select tmpgst
		Replace gstin With company.gstin,state With company.state In tmpgst
	Endif

	*!*	Thisform.accregistatus=Alltrim(tmpgst.supp_type) IN ExMain_Vw
	*!*	Thisform.taxapplarea=Alltrim(tmpgst.st_type)

	*!*	lcsqlstr='Select top 1 * From Lcode Where Entry_ty=?_Entry'
	*!*	nRet = _curform.sqlconobj.dataconn([EXE],company.dbname,lcsqlstr,[tmpLcode],"_curform.nHandle",_curform.DataSessionId,.F.)
	*!*	If nRet <= 0
	*!*		Return .F.
	*!*	Endif


	If tmLcode.IoTrans=1
		lcSqlStr="Select top 1 gst_state_code as stcode from State where state=?company.state"
		nRet = _curform.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,[tmpscode],"thisform.nHandle",_curform.DataSessionId,.F.)
		If nRet <= 0
			Return .F.
		Endif
		mstateid=tmpscode.stcode
		Use In tmpscode
	Else
		mstateid=tmpgst.statecode
	Endif

	lcSqlStr="Select top 1 hsncode,isservice,hsn_id,serty from it_mast where it_code=?_itcode"
	nRet = _curform.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,[tmpItemdet],"_curform.nHandle",_curform.DataSessionId,.F.)
	If nRet <= 0
		Return .F.
	Endif

	mhsnid=tmpItemdet.hsn_id
	mhsncode=tmpItemdet.hsncode
	misservice=tmpItemdet.isservice
	mdate=Ttod(exMain_vw.Date)+1

	If !Empty(mhsncode) And misservice<>.T.
		lcSqlStr="If Exists(Select top 1 sgstper from hsncodemast Where hsnid=?mhsnid and state_code=?mstateid and Activefrom < ?mdate) "
		lcSqlStr=lcSqlStr+	" Begin "
		lcSqlStr=lcSqlStr+	" Select sgstper,cgstper,igstper,exempted from hsncodemast Where hsnid=?mhsnid and state_code=?mstateid "
		lcSqlStr=lcSqlStr+	" and Activefrom < ?mdate order by Activefrom desc "
		lcSqlStr=lcSqlStr+	" End "
		lcSqlStr=lcSqlStr+	" Else "
		lcSqlStr=lcSqlStr+	" Begin "
		lcSqlStr=lcSqlStr+	" Select sgstper,cgstper,igstper,exempted from hsncodemast Where hsnid=?mhsnid and state_code='00'"
		lcSqlStr=lcSqlStr+	" and Activefrom < ?mdate order by Activefrom desc "
		lcSqlStr=lcSqlStr+	" End "

		nRet = _curform.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,[tmphsndet],"_curform.nHandle",_curform.DataSessionId,.F.)
		If nRet <= 0
			Return .F.
		Endif

		hsncount=Reccount('tmphsndet')
		If hsncount > 0
			Select tmphsndet
			Locate
			If tmphsndet.exempted
				Replace sgstper With 0, cgstper With 0,igstper With 0 In tmphsndet
			Endif
		Else

		Endif
		Select sgstper,cgstper,igstper,tmpgst.supp_type As RegiStatus,tmpgst.st_type As SuppType,exempted,Space(20) As Linerule   From tmphsndet Into Cursor gstRates Readwrite
	Endif
	If !Empty(tmpItemdet.hsncode) And tmpItemdet.isservice=.T.
		msqlstr="select Name,Abt_per,CGST_PER,IGST_PER,SGST_PER,exempted=Convert(Bit,0) From SerTax_Mast where [name]=?tmpItemdet.serty and (?Exmain_vw.date between sdate and edate) and charindex(?Exmain_vw.entry_ty,validity)>0 "
		nRet = _curform.sqlconobj.dataconn([EXE],company.dbname,msqlstr,[tmphsn],"_curform.nHandle",_curform.DataSessionId,.F.)
		If nRet <= 0
			Return .F.
		Endif
		Select SGST_PER As sgstper,CGST_PER As cgstper,IGST_PER As igstper,tmpgst.supp_type As RegiStatus,tmpgst.st_type As SuppType,exempted,Space(20) As Linerule From tmphsn Into Cursor gstRates Readwrite
		If Reccount('tmphsn')<=0
			Replace Linerule With "Non-GST" In gstRates
		Endif
	Endif

	If Used('gstRates')
		Do Case
			Case gstRates.exempted
				Replace Linerule With "Exempted" In gstRates
			Case (gstRates.sgstper + gstRates.cgstper + gstRates.igstper)<=0
				Replace Linerule With "Nill Rated" In gstRates
			Case (gstRates.sgstper + gstRates.cgstper + gstRates.igstper)>0
				Replace Linerule With "Taxable" In gstRates
		Endcase

		Do Case
			Case Inlist(Alltrim(tmpgst.supp_type),"EOU","SEZ","Export","Import")
				Replace sgstper With 0,cgstper With 0 In gstRates
			Case Inlist(Alltrim(tmpgst.st_type),"INTRASTATE")
				Replace igstper With 0 In gstRates
			Case Inlist(Alltrim(tmpgst.st_type),"INTERSTATE","OUT OF COUNTRY")
				Replace sgstper With 0,cgstper With 0 In gstRates
		Endcase

		If Upper(Alltrim(tmpgst.supp_type))="COMPOUNDING" And (tmLcode.IoTrans=1 Or Inlist(_Entry,"E1"))
			Replace sgstper With 0,cgstper With 0,igstper With 0  In gstRates
		Endif

		If Upper(Alltrim(tmpgst.supp_type))="COMPOUNDING" And Inlist(_Entry,"BP","CP")
			If exMain_vw.tdspaytype=2
				Replace sgstper With 0,cgstper With 0,igstper With 0  In gstRates
			Endif
		Endif

		If Type('Lmc_vw.ExpoType')<>'U' And (Inlist(tmLcode.entry_ty,"ST","S1","DC","PI","BR") Or Inlist(tmLcode.Behave,"ST","SB","DC","PI","BR") Or tmLcode.IoTrans=2)
			If (Alltrim(tmpgst.st_type)="OUT OF COUNTRY" Or Inlist(Alltrim(tmpgst.supp_type),"SEZ","EOU","Export")) And Upper(Alltrim(lmc_vw.ExpoType))="WITHOUT IGST"
				Replace sgstper With 0,cgstper With 0,igstper With 0  In gstRates
			Endif
		Endif
		Select exItem_vw
		Replace CGST_PER With gstRates.cgstper,SGST_PER With gstRates.sgstper,IGST_PER With gstRates.igstper In exItem_vw

	Endif
Endproc

Procedure get_cessrate
	Parameters _item

	_curform=_Screen.ActiveForm
	_datasessionid=_Screen.ActiveForm.DataSessionId
	Set DataSession To _datasessionid


	lcSqlStr="Select top 1 hsncode,isservice,hsn_id,serty,ServTCode from it_mast where it_name=?_Item"

	nRet = _curform.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,[tmpItemdet],"_curform.nHandle",_curform.DataSessionId,.F.)
	If nRet <= 0
		Return .F.
	Endif

	mhsnid=tmpItemdet.hsn_id
	mhsncode=Iif(tmpItemdet.isservice,tmpItemdet.servtcode,tmpItemdet.hsncode)
	misservice=tmpItemdet.isservice
	mdate= Ttod(exMain_vw.Date)+1
	mstateid=exMain_vw.gstscode

	lcSqlStr=[Execute usp_ent_get_cessrate ?mhsncode,?mstateid,?mdate]
	nRet = _curform.sqlconobj.dataconn([EXE],company.dbname,lcSqlStr,[_cessrate],"_curform.nHandle",_curform.DataSessionId,.F.)
	If nRet <= 0
		Return .F.
	Endif
Endproc

Procedure GetDocNo
	Parameter ventryType, vInvoiceDate,Tbl_entry,_cursqlconobj,nDatasessionid,vnHandle,vcDbName,vdSta_Dt,vdEnd_Dt,ndbname


	If Empty(ventryType)
		Retu ''
	Endif
	If Len(ventryType)>2
		Retu ''
	Endif
	If Len(ventryType)=1
		ventryType=ventryType+[ ]
	Endif
	If Empty(vInvoiceDate)
		Retu ''
	Endif
	If Used('Gen_doc_vw')
		Select Gen_doc_vw
		Use In Gen_doc_vw
	Endif
	mdoc_no = ''

	sql_con = _cursqlconobj.dataconn([EXE],vcDbName," Select * from "+Alltrim(ndbname)+"..Gen_doc with (NOLOCK) where 1=0 ",[Gen_doc_vw],;
		vnHandle,nDatasessionid,.T.)

	If sql_con > 0 And Used('Gen_doc_vw')
		Select Gen_doc_vw
		Append Blank In Gen_doc_vw
		Replace entry_ty With ventryType,Date With vInvoiceDate In Gen_doc_vw
		Wait Clear
		mrollback = .T.
		Do While .T.
			sql_con = _cursqlconobj.dataconn([EXE],vcDbName," Select Top 1 Doc_no from "+Alltrim(ndbname)+"..Gen_doc with (TABLOCKX) ;
			where Entry_ty = ?Gen_doc_vw.Entry_ty And Date = ?Gen_doc_vw.Date ",[tmptbl_vw],;
				vnHandle,nDatasessionid,.T.)

			msqlstr = []
			If sql_con > 0 And Used('tmptbl_vw')
				Select tmptbl_vw
				If Reccount() <= 0
					Select Gen_doc_vw
					Replace doc_no With 1 In Gen_doc_vw
					msqlstr = _cursqlconobj.geninsert("gen_doc","","","Gen_doc_vw",mvu_backend)
					msqlstr =Strtran(msqlstr,"gen_doc(",Alltrim(ndbname)+"..gen_doc(")
				Else
					Select Gen_doc_vw
					Replace doc_no With tmptbl_vw.doc_no + 1 In Gen_doc_vw
					mCond = "Entry_ty = ?Gen_doc_vw.Entry_ty And Date = ?Gen_doc_vw.Date"
					msqlstr = _cursqlconobj.GenUpdate("gen_doc","","","Gen_doc_vw",mvu_backend,mCond,"doc_no")
					msqlstr =Strtran(msqlstr,"gen_doc ",Alltrim(ndbname)+"..gen_doc ")
				Endif

				sql_con =_cursqlconobj.dataconn([EXE],vcDbName,msqlstr,[],vnHandle,nDatasessionid,.T.)
				If sql_con > 0
					*!*					sql_con = _curvouobj._SqlCommit(vnHandle)			&& 23/01/2019
					If sql_con > 0
						sql_main = Tbl_entry+'Main'
						Select Gen_doc_vw
						mdoc_no = Padl(Alltrim(Str(Gen_doc_vw.doc_no)),Len(exMain_vw.doc_no),"0")
						sql_con = _cursqlconobj.dataconn([EXE],vcDbName," Select Top 1 Entry_ty from "+Alltrim(ndbname)+".."+sql_main+;
							" where Entry_ty = ?Gen_doc_vw.Entry_ty And Date = ?Gen_doc_vw.Date And Doc_no = ?mdoc_no",;
							[tmptbl_vw],vnHandle,nDatasessionid,.T.)
						If sql_con > 0 And Used('tmptbl_vw')
							Select tmptbl_vw
							If Reccount() <= 0
								mrollback = .F.
								Exit
							Endif
						Endif
					Endif
				Endif
			Endif
		Enddo
		If mrollback = .T.
			sql_con =_cursqlconobj._sqlrollback(vnHandle)
		Endif
	Endif
	If Used('Gen_doc_vw')
		Select Gen_doc_vw
		Use In Gen_doc_vw
	Endif
	Wait Clear
	Return mdoc_no


Procedure GetInvNo
	Lparameter ventryType, vInvoiceSeries, vInvoiceNo,VentDate, voldInvoiceSeries, voldInvoiceNo,vinv_size,_cursqlconobj, nDatasessionid, entry_tbl, vnHandle,vcDbName,vdSta_Dt,vdEnd_Dt,ndbname

	If Empty(ventryType)
		Retu ''
	Endif
	If Len(ventryType)>2
		Retu ''
	Endif
	If Len(ventryType)=1
		ventryType=ventryType+[ ]
	Endif
	If Empty(vInvoiceSeries)
		vInvoiceSeries=Space(Len(exMain_vw.inv_sr))
	Endif
	If Empty(voldInvoiceSeries)
		voldInvoiceSeries=Space(Len(exMain_vw.inv_sr))
	Endif

	If Type('vinv_size') # 'N'
		vinv_size = 1
	Endif

	Minv_no     = ''
	v_i_s_type  = ''
	v_i_prefix  = ''
	v_i_suffix  = ''
	v_i_middle  = ''
	_vInvoiceEs = voldInvoiceSeries
	_vInvoiceEn = voldInvoiceNo

	v_i_prefix  = ''
	v_i_suffix  = ''
	v_i_s_type  = ''
	v_i_middle  = ''
	_vInvoiceSr = vInvoiceSeries
	_vInvoiceNo = Allt(vInvoiceNo)

	sql_con = _cursqlconobj.dataconn([EXE],vcDbName,[Select Top 1 * from Series where Inv_sr = ?_vInvoiceSr ];
		,[tmptbl_vw],vnHandle,nDatasessionid,.T.)

	If sql_con > 0 And Used('tmptbl_vw')
		Select tmptbl_vw
		If Reccount() > 0
			v_i_s_type = tmptbl_vw.S_type
			v_i_MnthFormat = tmptbl_vw.MnthFormat
			If !Empty(tmptbl_vw.I_prefix)
				v_i_prefix = Allt(Eval(tmptbl_vw.I_prefix))
			Else
				If Empty(_vInvoiceEs)
					v_i_prefix = GetPrefix(ventryType)
				Endif

			Endif
			If !Empty(tmptbl_vw.I_suffix)
				v_i_suffix = Allt(Eval(tmptbl_vw.I_suffix))
			Endif
			Do Case
				Case v_i_s_type  = 'DAYWISE'
					v_i_middle   = Dtos(VentDate)
					v_i_middle   = Subs(v_i_middle,7,2)+Subs(v_i_middle,5,2)+Subs(v_i_middle,3,2)
				Case v_i_s_type  = 'MONTHWISE'
					v_i_middle   = genMnthWiseFormat(VentDate,v_i_MnthFormat)
					v_i_middle   = v_i_middle
			Endcase
			_vInvoiceNo = Subs(_vInvoiceNo,Len(v_i_prefix+v_i_middle)+1)
			_vInvoiceNo = Subs(_vInvoiceNo,1,Len(_vInvoiceNo)-Len(v_i_suffix))
		Endif
	Endif
	_vInvoiceNo = Val(_vInvoiceNo)
	_vInvoiceNo = Iif(_vInvoiceNo = 0,1,_vInvoiceNo)

	If v_i_s_type = 'MONTHWISE'
		VentDate1 = '01/'+Str(Month(VentDate),2)+'/'+Str(Year(VentDate),4)
		VentDate  = Ctod(VentDate1)
	Endif

	vctrYear=[]
	If Between(VentDate,vdSta_Dt,vdEnd_Dt)
		vctrYear=Allt(Str(Year(vdSta_Dt)))+[-]+Allt(Str(Year(vdEnd_Dt)))
	Else
		endDate=[]
		For i = 1 To Month(vdEnd_Dt)
			If Empty(endDate)
				endDate=Allt(Str(i))
			Else
				endDate=endDate+[,]+Allt(Str(i))
			Endif
		Endfor

		startDate=[]
		For i = Month(vdSta_Dt) To 12
			If Empty(startDate)
				startDate=Allt(Str(i))
			Else
				startDate=startDate+[,]+Allt(Str(i))
			Endif
		Endfor
		Do Case
			Case Inlist(Month(VentDate),&endDate)
				vctrYear=Allt(Str(Year(VentDate)-1))+[-]+Allt(Str(Year(VentDate)))
			Case Inlist(Month(VentDate),&startDate)
				vctrYear=Allt(Str(Year(VentDate)))+[-]+Allt(Str(Year(VentDate)+1))
		Endcase
	Endif

	If Used('Gen_inv_vw')
		Select Gen_inv_vw
		Use In Gen_inv_vw
	Endif
	If Used('Gen_miss_vw')
		Select Gen_miss_vw
		Use In Gen_miss_vw
	Endif
	sql_con = _cursqlconobj.dataconn([EXE],vcDbName," Select * from "+Alltrim(ndbname)+"..Gen_inv with (NOLOCK) where 1=0 ",;
		[Gen_inv_vw],vnHandle,nDatasessionid,.T.)
	If sql_con > 0
		sql_con = _cursqlconobj.dataconn([EXE],vcDbName," Select * from "+Alltrim(ndbname)+"..Gen_miss with (NOLOCK) where 1=0 ",;
			[Gen_Miss_vw],vnHandle,nDatasessionid,.T.)
	Endif
	If sql_con > 0 And Used('Gen_inv_vw') And Used('Gen_miss_vw')
		Select Gen_inv_vw
		Append Blank In Gen_inv_vw
		Replace entry_ty With ventryType,Inv_dt With VentDate,inv_sr With vInvoiceSeries,;
			Inv_No With _vInvoiceNo,l_yn With vctrYear In Gen_inv_vw

		Select Gen_miss_vw
		Append Blank In Gen_miss_vw
		Replace entry_ty With Gen_inv_vw.entry_ty,Inv_dt With Gen_inv_vw.Inv_dt,inv_sr With Gen_inv_vw.inv_sr,;
			Inv_No With Gen_inv_vw.Inv_No,l_yn With Gen_inv_vw.l_yn,Flag With 'Y' In Gen_miss_vw

		Wait Clear
		mrollback = .T.
		Do While .T.
			Do Case
				Case v_i_s_type = 'DAYWISE'
					sql_con = _cursqlconobj.dataconn([EXE],vcDbName," Select Top 1 Inv_no from "+Alltrim(ndbname)+"..Gen_inv with (TABLOCKX) ;
				where Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And Inv_dt = ?Gen_inv_vw.Inv_dt ",;
						[tmptbl_vw],vnHandle,nDatasessionid,.T.)
				Case v_i_s_type = 'MONTHWISE'
					sql_con = _cursqlconobj.dataconn([EXE],vcDbName," Select Top 1 Inv_no from "+Alltrim(ndbname)+"..Gen_inv with (TABLOCKX) ;
				where Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And ;
				MONTH(Inv_dt) = ?MONTH(Gen_inv_vw.Inv_dt) And Year(Inv_dt) = ?Year(Gen_inv_vw.Inv_dt) ",[tmptbl_vw],;
						vnHandle,nDatasessionid,.T.)
				Otherwise
					sql_con = _cursqlconobj.dataconn([EXE],vcDbName," Select Top 1 Inv_no from "+Alltrim(ndbname)+"..Gen_inv with (TABLOCKX) ;
				where Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And L_yn = ?Gen_inv_vw.L_yn ",;
						[tmptbl_vw],vnHandle,nDatasessionid,.T.)
			Endcase
			msqlstr = []
			If sql_con > 0 And Used('tmptbl_vw')
				msqlstr = ' '
				Select tmptbl_vw
				If Reccount() <= 0
					Select Gen_inv_vw
					msqlstr = _cursqlconobj.geninsert("gen_inv","","","Gen_inv_vw",mvu_backend)
					msqlstr=Strtran(msqlstr,"gen_inv(",Alltrim(ndbname)+"..gen_inv(")
				Else
					If tmptbl_vw.Inv_No < Gen_inv_vw.Inv_No
						Select Gen_inv_vw
						Do Case
							Case v_i_s_type = 'DAYWISE'
								mCond = "Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And Inv_dt = ?Gen_inv_vw.Inv_dt"
							Case v_i_s_type = 'MONTHWISE'
								mCond = "Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And MONTH(Inv_dt) = ?MONTH(Gen_inv_vw.Inv_dt) And Year(Inv_dt) = ?Year(Gen_inv_vw.Inv_dt)"
							Otherwise
								mCond = "Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And L_yn = ?Gen_inv_vw.L_yn"
						Endcase

						msqlstr = _cursqlconobj.GenUpdate("gen_inv","","","Gen_inv_vw",mvu_backend,mCond,"inv_no")
						msqlstr=Strtran(msqlstr,"gen_inv ",Alltrim(ndbname)+"..gen_inv ")
					Endif
				Endif

				If !Empty(msqlstr)
					sql_con = _cursqlconobj.dataconn([EXE],vcDbName,msqlstr,[],;
						vnHandle,nDatasessionid,.T.)
				Endif
				If sql_con > 0
					Do Case
						Case v_i_s_type = 'DAYWISE'
							sql_con = _cursqlconobj.dataconn([EXE],vcDbName," Select Top 1 Flag from "+Alltrim(ndbname)+"..Gen_miss where ;
						Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And Inv_no = ?Gen_inv_vw.Inv_no And Inv_dt = ?Gen_inv_vw.Inv_dt ",[tmptbl_vw],;
								vnHandle,nDatasessionid,.T.)
						Case v_i_s_type = 'MONTHWISE'
							sql_con = _cursqlconobj.dataconn([EXE],vcDbName," Select Top 1 Flag from "+Alltrim(ndbname)+"..Gen_miss where ;
						Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And ;
						Inv_no = ?Gen_inv_vw.Inv_no And MONTH(Inv_dt) = ?MONTH(Gen_inv_vw.Inv_dt) And ;
						Year(Inv_dt) = ?Year(Gen_inv_vw.Inv_dt) ",[tmptbl_vw],;
								vnHandle,nDatasessionid,.T.)
						Otherwise
							sql_con = _cursqlconobj.dataconn([EXE],vcDbName," Select Top 1 Flag from "+Alltrim(ndbname)+"..Gen_miss where ;
						Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And ;
						Inv_no = ?Gen_inv_vw.Inv_no And L_yn = ?Gen_inv_vw.L_yn ",[tmptbl_vw],;
								vnHandle,nDatasessionid,.T.)
					Endcase
					vFoundInMiss='Y'
					msqlstr = []
					If sql_con > 0 And Used('tmptbl_vw')
						Select tmptbl_vw
						If Reccount() <= 0
							vFoundInMiss='N'
							Select Gen_miss_vw
							msqlstr = _cursqlconobj.geninsert("gen_miss","","","Gen_miss_vw",mvu_backend)
							msqlstr=Strtran(msqlstr,"gen_miss(",Alltrim(ndbname)+"..gen_miss(")
							sql_con = _cursqlconobj.dataconn([EXE],vcDbName,msqlstr,[],;
								vnHandle,nDatasessionid,.T.)
						Else
							vFoundInMiss=tmptbl_vw.Flag
							If vFoundInMiss = 'N'
								Select 	Gen_miss_vw
								Do Case
									Case v_i_s_type = 'DAYWISE'
										mCond = "Entry_ty = ?Gen_miss_vw.Entry_ty And Inv_sr = ?Gen_miss_vw.Inv_sr And ;
									Inv_no = ?Gen_miss_vw.Inv_no And Inv_dt = ?Gen_miss_vw.Inv_dt"
									Case v_i_s_type = 'MONTHWISE'
										mCond = "Entry_ty = ?Gen_miss_vw.Entry_ty And Inv_sr = ?Gen_miss_vw.Inv_sr And ;
									Inv_no = ?Gen_miss_vw.Inv_no And MONTH(Inv_dt) = ?MONTH(Gen_miss_vw.Inv_dt) And ;
									Year(Inv_dt) = ?Year(Gen_miss_vw.Inv_dt)"
									Otherwise
										mCond = "Entry_ty = ?Gen_miss_vw.Entry_ty And Inv_sr = ?Gen_miss_vw.Inv_sr And ;
									Inv_no = ?Gen_miss_vw.Inv_no And L_yn = ?Gen_miss_vw.L_yn"
								Endcase
								msqlstr = _cursqlconobj.GenUpdate("gen_miss","","","Gen_miss_vw",;
									mvu_backend,mCond,"inv_no,flag")		&&03/01/2008
								msqlstr=Strtran(msqlstr,"gen_miss ",Alltrim(ndbname)+"..gen_miss ")
								sql_con = _cursqlconobj.dataconn([EXE],vcDbName,msqlstr,[],;
									vnHandle,nDatasessionid,.T.)
							Endif
						Endif
					Endif
					If vFoundInMiss='N'
						*!*						sql_con = _curvouobj._SqlCommit(lnhandle)				&& 23/01/2019
						If sql_con > 0
							sql_main = entry_tbl+'Main'
							Select Gen_inv_vw
							Minv_no = Padl(Alltrim(Str(Gen_inv_vw.Inv_No)),vinv_size,"0")	&&test_z vinv_size
							Minv_no = Padr(v_i_prefix+v_i_middle+Minv_no+v_i_suffix,Len(exMain_vw.Inv_No),' ')
							sql_con = _cursqlconobj.dataconn([EXE],vcDbName,[ Select Top 1 Entry_ty from ]+;
								sql_main+[ where Entry_ty = ?Gen_inv_vw.Entry_ty And Inv_sr = ?Gen_inv_vw.Inv_sr And ;
							Inv_no = ?minv_no And L_yn = ?Gen_inv_vw.L_yn ],[tmptbl_vw],;
								vnHandle,nDatasessionid,.T.)
							If sql_con > 0 And Used('tmptbl_vw')
								Select tmptbl_vw
								If Reccount() <= 0
									mrollback = .F.
									Exit
								Else
									vFoundInMiss='Y'
								Endif
							Endif
						Endif
					Endif
					If vFoundInMiss='Y'
						Select Gen_inv_vw
						Replace Inv_No With Inv_No + 1 In Gen_inv_vw

						Select Gen_miss_vw
						Replace Inv_No With Gen_inv_vw.Inv_No In Gen_miss_vw
					Endif
				Endif
			Endif
		Enddo
		msqlstr = []
		If mrollback = .F.
			_vInvoiceEn = Iif(Type('_vInvoiceEn') # 'N',Val(_vInvoiceEn),_vInvoiceEn)
			Select Gen_inv_vw
			Replace entry_ty With ventryType,Inv_dt With VentDate,inv_sr With _vInvoiceEs,;
				Inv_No With _vInvoiceEn,l_yn With vctrYear In Gen_inv_vw
			Select Gen_miss_vw
			Replace entry_ty With Gen_inv_vw.entry_ty,Inv_dt With Gen_inv_vw.Inv_dt,inv_sr With Gen_inv_vw.inv_sr,;
				Inv_No With Gen_inv_vw.Inv_No,l_yn With Gen_inv_vw.l_yn,Flag With 'N' In Gen_miss_vw

			Do Case
				Case v_i_s_type = 'daywise'
					mCond = "entry_ty = ?gen_miss_vw.entry_ty and inv_sr = ?gen_miss_vw.inv_sr and ;
				inv_no = ?gen_miss_vw.inv_no and inv_dt = ?gen_miss_vw.inv_dt"
				Case v_i_s_type = 'monthwise'
					mCond = "entry_ty = ?gen_miss_vw.entry_ty and inv_sr = ?gen_miss_vw.inv_sr and ;
				inv_no = ?gen_miss_vw.inv_no and month(inv_dt) = ?month(gen_miss_vw.inv_dt) and ;
				year(inv_dt) = ?year(gen_miss_vw.inv_dt)"
				Otherwise
					mCond = "entry_ty = ?gen_miss_vw.entry_ty and inv_sr = ?gen_miss_vw.inv_sr and ;
				inv_no = ?gen_miss_vw.inv_no and l_yn = ?gen_miss_vw.l_yn"
			Endcase
			msqlstr = _cursqlconobj.GenUpdate("gen_miss","","","gen_miss_vw",mvu_backend,mCond,"flag")
			msqlstr=Strtran(msqlstr,"gen_miss ",Alltrim(ndbname)+"..gen_miss ")
			sql_con = _cursqlconobj.dataconn([exe],vcDbName,msqlstr,[],;
				vnHandle,nDatasessionid,.T.)
			If sql_con < 0
				mrollback = .T.
			Endif
		Endif
		If mrollback = .T.
			sql_con = _cursqlconobj._sqlrollback(lnhandle)
		Endif
	Endif

	If Used('Gen_inv_vw')
		Select Gen_inv_vw
		Use In Gen_inv_vw
	Endif
	If Used('Gen_miss_vw')

		Select Gen_miss_vw
		Use In Gen_miss_vw
	Endif

	Wait Clear
	Return Minv_no

	&&Added by Shrikant S. on 28012019 for Bug-32210 End

	&&Added by Priyanka B on 06032019 for Bug-32210 Start
Procedure GetColumnStructure
	Lparameters lcfld
	nFieldCount = Alines(aFieldList, lcfld, ",")
	lcfieldstr =""

	For i=1 To nFieldCount
		For ix = 1 To Afields(laFieldInfo,'Main_vw')
			If Upper(Alltrim(aFieldList[i]))==Upper(Alltrim(laFieldInfo[m.ix,1]))
				Do Case
					Case Inlist(Alltrim(laFieldInfo[m.ix,2]),"L","D","T","I")
						If Alltrim(laFieldInfo[m.ix,2])="T"
							lcFldDataType = "D"
						Else
							lcFldDataType = Alltrim(laFieldInfo[m.ix,2])
						Endif
					Case Alltrim(laFieldInfo[m.ix,2])="M"
						lcFldDataType = "C(250)"
					Case Inlist(Alltrim(laFieldInfo[m.ix,2]),"N","F")
						lcFldDataType = Alltrim(laFieldInfo[m.ix,2]) + "("+Alltrim(Str(laFieldInfo[m.ix,3]))+","+Alltrim(Str(laFieldInfo[m.ix,4]))+")"
					Otherwise
						lcFldDataType = Alltrim(laFieldInfo[m.ix,2]) + "("+Alltrim(Str(laFieldInfo[m.ix,3]))+")"
				Endcase
				lcfieldstr = Iif(Empty(lcfieldstr),"",lcfieldstr+",") + Alltrim(laFieldInfo[m.ix,1]) + " " + lcFldDataType
			Endif
		Endfor
	Endfor
	Return lcfieldstr
Endproc


Procedure getrefdetails
	_curvouobj = _Screen.ActiveForm

	If Used("Itref_vw")
		msqlstr = "select cast('1' as bit) as lselect,m.party_nm as ac_name,o.rentry_ty,o.itref_tran,o.ritserial,o.it_code,o.rqty as qty,i.rate,m.ac_id from othitref o"
		msqlstr =msqlstr + " inner join pqitem i on (o.rentry_ty=i.entry_ty and o.itref_tran=i.tran_cd and o.ritserial=i.itserial)"
		msqlstr =msqlstr + " inner join pqmain m on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd)"
		msqlstr =msqlstr + " where 1=2"
		etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"curOthItref","_curvouobj.nHandle",_curvouobj.DataSessionId)
		If etsql_con <= 0
			Return .F.
		Endif

		*!*			Select item_vw
		*!*			Go Top

		Select Itref_vw
		Go Top
		Scan For Itref_vw.entry_ty=item_vw.entry_ty And Itref_vw.tran_cd=item_vw.tran_cd And Itref_vw.itserial=item_vw.itserial
			msqlstr = "select cast('1' as bit) as lselect,m.party_nm as ac_name,o.rentry_ty,o.itref_tran,o.ritserial,o.it_code,o.rqty as qty,i.rate,m.ac_id from othitref o"
			msqlstr =msqlstr + " inner join pqitem i on (o.rentry_ty=i.entry_ty and o.itref_tran=i.tran_cd and o.ritserial=i.itserial)"
			msqlstr =msqlstr + " inner join pqmain m on (m.entry_ty=i.entry_ty and m.tran_cd=i.tran_cd)"
			msqlstr =msqlstr + " where o.entry_ty=?itref_vw.rentry_ty and o.tran_cd=?itref_vw.itref_tran and o.itserial=?itref_vw.ritserial"

			etsql_con = _curvouobj.sqlconobj.dataconn([EXE],company.dbname,msqlstr,"curRef1","_curvouobj.nHandle",_curvouobj.DataSessionId)
			If etsql_con <= 0
				Return .F.
			Endif
			etsql_con = _curvouobj.sqlconobj.sqlconnclose("_curvouobj.nhandle")

			Select curOthitref
			If Used('curRef1')
				Append From Dbf('curRef1')
			Endif
			Select Itref_vw
		Endscan
	Endif

	If Used("curRef1")
		Use In curRef1
	Endif

Endproc
&&Added by Priyanka B on 06032019 for Bug-32210 End
