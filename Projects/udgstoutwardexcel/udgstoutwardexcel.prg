&&Commented by Priyanka B on 19092019 for Bug-32872 Start
*!*	Lparameters lsdate,ledate,dsessionid
*!*	Do Form uefrmgstoutwardxls  With lsdate,ledate,dsessionid
&&Commented by Priyanka B on 19092019 for Bug-32872 End

&&Modified by Priyanka B on 19092019 for Bug-32872 Start
Lparameters lsdate,ledate,dsessionid,lValue
Do Case
	Case lValue=1
		Do Form uefrmgstoutwardxls  With lsdate,ledate,dsessionid
	Case lValue=0
		Do Form uefrmgstoutxlswefoct19  With lsdate,ledate,dsessionid
Endcase
&&Modified by Priyanka B on 19092019 for Bug-32872 End
