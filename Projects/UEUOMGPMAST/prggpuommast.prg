Lparameters tnrange

If !'\DATEPICKER.' $ Upper(Set('Class'))
	Set Classlib To datepicker Additive
Endif
If !'\VOUCLASS.' $ Upper(Set('Class'))
	Set Classlib To VOUCLASS Additive
Endif

Do Form frmgpuommast With tnrange
