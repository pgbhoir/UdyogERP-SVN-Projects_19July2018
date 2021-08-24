Parameters tcWherestr As String
tcWherestr=Strtran(tcWherestr,Upper("this"),"lcValue")
If !Empty(tcWherestr)
	tcWherestr=Evaluate(tcWherestr)
	If !Empty(tcWherestr)
		tcWherestr=Transform(Evaluate(tcWherestr))
	Endif
Endif
Return tcWherestr
