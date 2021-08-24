 Parameters oControl
*!*	Set Step On
sql_con = 0
oactive=_Screen.ActiveForm
tsrno = 0
If Type('oactive.pcvtype')<>'U'
	*!*		If Inlist(oactive.pcvtype,"RD","OC","W1","W2","W3","R2","W4","R3")  && Date : 03-02-2020 Bug 33252 Added - W3,R2  &&Commented by Priyanka B on 15022020 for Bug-33252
	If Inlist(oactive.pcvtype,"RD","OC","W1","W2","W3","R2","W4","R3","O6")  && Date : 03-02-2020 Bug 33252 Added - W3,R2  &&Modified by Priyanka B on 15022020 for Bug-33252
		*!*			Messagebox("txtflotno_f2key 1")
		If Type('oControl')='C'
			lcfld=oControl
		Else
			lcfld=Justext(oControl.ControlSource)
		Endif

		If Used('Itref_vw')
			If Reccount('Itref_vw')>0 And !Deleted()
				Return
			Endif
		Endif


		If (oactive.addmode Or oactive.editmode) And Upper(Alltrim(lcfld))=="FLOTNO"
			*!*				Messagebox("txtflotno_f2key 2")
			*!*				If Inlist(oactive.pcvtype,"RD","OC","W1","W2","W3","R2","W4","R3")  && Date : 03-02-2020 Bug 33252 Added - W3,R2,W4,R3  &&Commented by Priyanka B on 15022020 for Bug-33252
			If Inlist(oactive.pcvtype,"RD","OC","W1","W2","W3","R2","W4","R3","O6")  && Date : 03-02-2020 Bug 33252 Added - W3,R2,W4,R3  &&Modified by Priyanka B on 15022020 for Bug-33252
				*!*					Messagebox("txtflotno_f2key 3")
				Do Case
					Case Inlist(oactive.pcvtype,"W1")
						sq1 = "Execute USP_GetLot_Details ?main_vw.entry_ty,?main_vw.party_nm,'','',0.00,?main_vw.tran_cd,''"
						*!*						Case Inlist(oactive.pcvtype,"RD")  &&Commented by Priyanka B on 15022020 for Bug-33252
					Case Inlist(oactive.pcvtype,"RD","O6")  &&Modified by Priyanka B on 15022020 for Bug-33252
						*!*							Messagebox("txtflotno_f2key 4")
						sq1 = "Execute USP_GetLot_Details ?main_vw.entry_ty,?main_vw.party_nm,'','',0.00,?main_vw.tran_cd,?item_vw.itserial"
					Case Inlist(oactive.pcvtype,"OC")
						sq1 = "Execute USP_GetLot_Details ?main_vw.entry_ty,'Use For Production','','',0.00,?main_vw.tran_cd,?item_vw.itserial"
					Case Inlist(oactive.pcvtype,"W2")
				
						sq1 = "Execute USP_GetLot_Details ?main_vw.entry_ty,'Use For Production','','',0.00,?main_vw.tran_cd,''"
						*--Date : 03-02-2020 Bug 33252 Added - W3,R2,W4,R3 Start
					Case Inlist(oactive.pcvtype,"W3")
						sq1 = "Execute USP_GetLot_Details ?main_vw.entry_ty,?main_vw.party_nm,'','',0.00,?main_vw.tran_cd,''"
					Case Inlist(oactive.pcvtype,"R2")
						sq1 = "Execute USP_GetLot_Details ?main_vw.entry_ty,?main_vw.party_nm,'','',0.00,?main_vw.tran_cd,?item_vw.itserial"
					Case Inlist(oactive.pcvtype,"W4")
						sq1 = "Execute USP_GetLot_Details ?main_vw.entry_ty,?main_vw.party_nm,'','',0.00,?main_vw.tran_cd,''"
					Case Inlist(oactive.pcvtype,"R3")
						sq1 = "Execute USP_GetLot_Details ?main_vw.entry_ty,?main_vw.party_nm,'','',0.00,?main_vw.tran_cd,?item_vw.itserial"
						*--Date : 03-02-2020 Bug 33252  End

				ENDCASE

				nretval = oactive.sqlconobj.dataconn([EXE],company.dbname,sq1,"_updtlot","oactive.nHandle",oactive.DataSessionId)
				If nretval <=0
					Return .F.
				Endif
				*!*					Select _updtlot
				*!*					Copy To "D:\_updtl.dbf"
				*!*					Messagebox("txtflotno_f2key 5")
				If Used("_updtlot")
					If Reccount('_updtlot') > 0
						*!*						If oactive.addmode
						*!*							If Inlist(oactive.pcvtype,"RD","OC")  &&Commented by Priyanka B on 14022020 for Bug-33252
						*!*							Messagebox("txtflotno_f2key 6")
						If Inlist(oactive.pcvtype,"RD","OC","R2","R3","O6")  &&Modified by Priyanka B on 14022020 for Bug-33252
							*!*								Messagebox("txtflotno_f2key 7")
							Select item_vw
*!*								Copy To "D:\item1.dbf"  && Commented By Anil on 31-03-2020 Seems it was for testing purpose
							sItserial = item_vw.itserial
							Select flotno,Sum(qty) As pendqty From item_vw With (Buffering = .T.) Where item_vw.itserial <> sItserial Group By flotno ;
								Into Cursor curPendQty

							&&Commented by Priyanka B on 15022020 for Bug-33252 Start
							*!*								Update a Set a.allocqty = (a.qty - a.baljoqty - a.balwojoqty),a.balqty =(a.balwojoqty - Iif(Isnull(b.pendqty),0,b.pendqty));
							*!*									From  _updtlot a Left Outer Join curPendQty b On (a.flotno==b.flotno)
							&&Commented by Priyanka B on 15022020 for Bug-33252 End

							&&Added by Priyanka B on 15022020 for Bug-33252 Start
							If Inlist(oactive.pcvtype,"O6")
								Update a Set a.allocqty = (a.qty - a.balwojoqty),a.balqty =(a.balwojoqty - Iif(Isnull(b.pendqty),0,b.pendqty));
									From  _updtlot a Left Outer Join curPendQty b On (a.flotno==b.flotno)
							Else
								Update a Set a.allocqty = (a.qty - a.baljoqty - a.balwojoqty),a.balqty =(a.balwojoqty - Iif(Isnull(b.pendqty),0,b.pendqty));
									From  _updtlot a Left Outer Join curPendQty b On (a.flotno==b.flotno)
							Endif
							&&Added by Priyanka B on 15022020 for Bug-33252 End


							&& Added by Anil on 30072020 for Bug 33328 Start
							SELECT _updtlot
							SUM balqty TO _mtally
							IF _mtally=0
								REPLACE ALL BALQTY WITH (BalJoQty-BalWoJoQty)
								Select * From _updtlot With (Buffering = .T.) Where balqty > 0 Into Cursor _updtlot1	
							ENDIF
							&& Added by Anil on 30072020 for Bug 33328 End
							

							Select * From _updtlot With (Buffering = .T.) Where balqty > 0 Into Cursor _updtlot1

							
							*!*								Select _updtlot1
							*!*								Copy To "D:\_updtl1.dbf"
						Else
							*!*								If Inlist(oactive.pcvtype,"W1","W2")  &&Commented by Priyanka B on 14022020 for Bug-33252
							If Inlist(oactive.pcvtype,"W1","W2","W3","W4")  &&Modified by Priyanka B on 14022020 for Bug-33252
								Update a Set a.allocqty = a.joqty,a.balqty = (a.qty - a.joqty) From _updtlot a 
								
								* Update a Set a.allocqty = a.joqty,a.balqty = (a.qty - a.balwojoqty) From _updtlot a && Modifiedby Anil on 26-06-2020 for Bug 33328
							
								&& Added by Anil on 26-06-2020 for Bug 33328 End
								&&Where a.qty=(a.balwojoqty+a.baljoqty)
								Select * From _updtlot With (Buffering = .T.) Where balqty > 0 Into Cursor _updtlot1
								*!*								Endif  *-- Commented by Anil On Date : 05-02-2020 Bug 33252
								&&Commented by Priyanka B on 14022020 for Bug-33252 Start
								*!*								Else
								*!*									*-- Added by Anil On Date : 05-02-2020 Bug 33252 Start
								*!*									If Inlist(oactive.pcvtype,"R2","W3")
								*!*										sItserial = item_vw.itserial
								*!*										Select flotno,Sum(qty) As pendqty From item_vw With (Buffering = .T.) Where item_vw.itserial <> sItserial Group By flotno ;
								*!*											Into Cursor curPendQty
								*!*										Update a Set a.allocqty = (a.qty - a.baljoqty - a.balwojoqty),a.balqty =(a.balwojoqty - Iif(Isnull(b.pendqty),0,b.pendqty));
								*!*											From  _updtlot a Left Outer Join curPendQty b On (a.flotno==b.flotno)

								*!*										Select * From _updtlot With (Buffering = .T.) Where balqty > 0 Into Cursor _updtlot1
								*!*									Else
								*!*										If Inlist(oactive.pcvtype,"R3","W4")
								*!*											sItserial = item_vw.itserial
								*!*											Select flotno,Sum(qty) As pendqty From item_vw With (Buffering = .T.) Where item_vw.itserial <> sItserial Group By flotno ;
								*!*												Into Cursor curPendQty
								*!*											Update a Set a.allocqty = (a.qty - a.baljoqty - a.balwojoqty),a.balqty =(a.balwojoqty - Iif(Isnull(b.pendqty),0,b.pendqty));
								*!*												From  _updtlot a Left Outer Join curPendQty b On (a.flotno==b.flotno)

								*!*											Select * From _updtlot With (Buffering = .T.) Where balqty > 0 Into Cursor _updtlot1
								*!*											*-- Date : 05-02-2020 Bug 33252 End
								*!*										Endif
								*!*									Endif
								&&Commented by Priyanka B on 14022020 for Bug-33252 End
							Endif
						Endif
						*!*														Messagebox("txtflotno_f2key 8")
						If nretval > 0 And Reccount('_updtlot1') > 0
							*!*														Messagebox("txtflotno_f2key 9")
							Do Case
									*!*										Case Inlist(oactive.pcvtype,"RD","W1")  &&Commented by Priyanka B on 14022020 for Bug-33252
*!*									Case Inlist(oactive.pcvtype,"RD","W1","R2","W3","R3","W4")  &&Modified by Priyanka B on 14022020 for Bug-33252 && Commented by Anil on 31-03-2020 for Bug 33328 
									*!*																Messagebox("txtflotno_f2key 10")
									Case Inlist(oactive.pcvtype,"W3","R3","W4")  && Modified by Anil on 31-03-2020 for Bug No 33328 &&Modified by Priyanka B on 14022020 for Bug-33252  
									lcTitle = "Select Lot No..."
									lcSrcFld  = [flotno]
*!*										lcFldList = [flotno,qty,allocqty,balqty] && Commented by Anil on 22-06-2020 for Bug 33328
									lcFldList = [flotno,item,qty,allocqty,balqty] && Modified by Anil on 22-06-2020 for Bug 33328		
*!*									lcFldCapt = [flotno:Lot No.,qty:Quantity,allocqty:Alloc. Job Order Qty.,balqty:Balance Qty.] && Commented by Anil on 22-06-2020 for Bug 33328
									lcFldCapt = [flotno:Lot No.,item:Item,qty:Quantity,allocqty:Alloc. Job Order Qty.,balqty:Balance Qty.] && Modified by Anil on 22-06-2020 for Bug 33328
									lcFldExcl = []
*!*										lcFldRtrn = [flotno,qty,allocqty,balqty] && Commented by Anil on 22-06-2020 for Bug 33328
									lcFldRtrn = [flotno,item,qty,allocqty,balqty] && Modified by Anil on 22-06-2020 for Bug 33328
								Case Inlist(oactive.pcvtype,"OC","W2")
									lcTitle = "Select Cutting Lot No..."
									lcSrcFld  = [flotno+item]
									lcFldList = [flotno,item,allocqty,qty,balqty,fshade,fgrade,fdesign]
*!*									lcFldCapt = [flotno:Cutting Lot No.,qty:Quantity,allocqty:Alloc. Job Order Qty.,balqty:Balance Qty.,item:Goods Name,fshade:Shade,fgrade:Grade,fdesign:Design] && Commented by Anil on 06042020 for Bug 33328
									lcFldCapt = [flotno:Cutting Lot No.,item:Sent Item,qty:Quantity,allocqty:Alloc. Job Order Qty.,balqty:Balance Qty.,fshade:Shade,fgrade:Grade,fdesign:Design] && Modified by Anil on 06042020 for Bug 33328
									lcFldExcl = []
									lcFldRtrn = [flotno,item,qty,allocqty,balqty,fshade,fgrade,fdesign]
									&&Commented by Priyanka B on 14022020 for Bug-33252 Start
									*!*											*-- Added by Anil On Date : 05-02-2020 Bug 33252 Start
									*!*										Case Inlist(oactive.pcvtype,"R2","W3")
									*!*											lcTitle = "Select Lot No..."
									*!*											lcSrcFld  = [flotno]
									*!*											lcFldList = [flotno,qty,allocqty,balqty]
									*!*											lcFldCapt = [flotno:Lot No.,qty:Quantity,allocqty:Alloc. Job Order Qty.,balqty:Balance Qty.]
									*!*											lcFldExcl = []
									*!*											lcFldRtrn = [flotno,qty,allocqty,balqty]
									*!*										Case Inlist(oactive.pcvtype,"R3","W4")
									*!*											lcTitle = "Select Lot No..."
									*!*											lcSrcFld  = [flotno]
									*!*											lcFldList = [flotno,qty,allocqty,balqty]
									*!*											lcFldCapt = [flotno:Lot No.,qty:Quantity,allocqty:Alloc. Job Order Qty.,balqty:Balance Qty.]
									*!*											lcFldExcl = []
									*!*											lcFldRtrn = [flotno,qty,allocqty,balqty]
									*!*											*-- Added by Anil On Date : 05-02-2020 Bug 33252 End
									&&Commented by Priyanka B on 14022020 for Bug-33252 End
									&&Added by Priyanka B on 15022020 for Bug-33252 Start
								Case Inlist(oactive.pcvtype,"O6")
									lcTitle = "Select Lot No..."
									lcSrcFld  = [flotno+item]
									lcFldList = [flotno,allocqty,qty,balqty,item,fshade,fgrade,fdesign]
									lcFldCapt = [flotno:Lot No.,qty:Quantity,allocqty:Alloc. Qty.,balqty:Balance Qty.,item:Goods Name,fshade:Shade,fgrade:Grade,fdesign:Design]
									lcFldExcl = []
									lcFldRtrn = [flotno,qty,allocqty,balqty,item,fshade,fgrade,fdesign]
									&&Added by Priyanka B on 15022020 for Bug-33252 End
								Case Inlist(oactive.pcvtype,"RD","W1","R2") && Added by Anil on 31-03-2020 for Bug No 33328 
									lcTitle = "Select Lot No..."
									lcSrcFld  = [flotno]
									lcFldList = [flotno,item,qty,allocqty,balqty]
									lcFldCapt = [flotno:Lot No.,item:Item,qty:Quantity,allocqty:Alloc. Job Order Qty.,balqty:Balance Qty.]
									lcFldExcl = []
									lcFldRtrn = [flotno,item,qty,allocqty,balqty]
							Endcase
							*!*								Messagebox("txtflotno_f2key 11")
							If Type('oControl')='O'
								lcstr = oControl.Value
							Else
								lcstr =""
							ENDIF
							Select 	_updtlot1

							RetItem=Uegetpop([_updtlot1],lcTitle,lcSrcFld,lcFldList,lcstr,[],[],[],.F.,[],lcFldRtrn,lcFldCapt,lcFldExcl)

							If Vartype(RetItem)="O"
								If Type('oControl')='O'
									oControl.Value = RetItem.flotno
								Endif
								Replace item_vw.flotno With RetItem.flotno In item_vw
								&&Commented by Priyanka B on 14022020 for Bug-33252 Start
								*!*									*-- Added by Anil On Date : 05-02-2020 Bug 33252 Start

								*!*									If Inlist(oactive.pcvtype,'W3','W4')
								*!*										Replace main_vw.fsentqty With RetItem.balqty In main_vw
								*!*									Endif
								*!*									*-- Added by Anil On Date : 05-02-2020 Bug 33252 End
								&&Commented by Priyanka B on 14022020 for Bug-33252 End

								If oactive.pcvtype="W2"
									Replace main_vw.fSentItem With RetItem.Item In main_vw
									Replace main_vw.fsentqty WITH RetItem.balqty In main_vw
									Select item_vw
									Delete All
									Append Blank
									tsrno = tsrno+1
									Replace item_no With Str(tsrno,5),itserial With Padl(Allt(Str(tsrno)),5,"0"),Item With RetItem.Item,flotno With RetItem.flotno;
										,fshade With RetItem.fshade,fgrade With RetItem.fgrade,fdesign With RetItem.fdesign;
										,entry_ty With main_vw.entry_ty,Date With main_vw.Date,Doc_no With main_vw.Doc_no In item_vw
								Endif
								If Inlist(oactive.pcvtype,"OC")
									Replace Item With RetItem.Item, fshade With RetItem.fshade;
										,fgrade With RetItem.fgrade,fdesign With RetItem.fdesign In item_vw
								Endif
							Endif
						Else
							sql_con = oactive.sqlconobj.sqlconnclose("oactive.nHandle")
							If Used('_updtlot')
								Use In _updtlot
							Endif
							Messagebox("No Records found!! ",0+64,vumess)
							Return .T.
						Endif
					Else
						Messagebox("No Records found!! ",0+64,vumess)
					Endif
				Endif
				sql_con = oactive.sqlconobj.sqlconnclose("oactive.nHandle")
				If Used('_updtlot')
					Use In _updtlot
				Endif
			Endif
		Endif
	Endif
Endif


 