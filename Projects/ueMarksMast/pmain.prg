Parameters _addmode,_editmode,_datasessionId,_parent,lctype,lcatt_file


If lctype=="T"
	Do Form frmproplist WITH _addmode,_editmode,_datasessionId,_parent,lctype,lcatt_file
	Read Events
Else
	If lctype="M"
		Do Form frmpropMast
	Endif
Endif

