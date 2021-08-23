Lparameters ccValue,nType
Local cPattern,oRE

oRE = Createobject("VBScript.RegExp")
Do Case
	Case nType = 1 		&& Pattern for Email Address
		cPattern = '^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$'

&&Added by Priyanka B on 11062018 for Bug-31628 Start
	Case nType = 2  && Pattern for GSTIN
		cPattern = '^[0-9]{2}[a-zA-Z]{4}[0-9a-zA-Z]{1}[0-9]{4}[a-zA-Z]{1}[1-9A-Za-z]{1}[Zz1-9A-Ja-j]{1}[0-9a-zA-Z]{1}$'
&&Added by Priyanka B on 11062018 for Bug-31628 End
Endcase
oRE.Pattern = cPattern
*!*	Return oRE.test(ccValue)  &&Commented by Priyanka B on 13022019 for Bug-31890 & AU 2.1.1

&&Modified by Priyanka B on 13022019 for Bug-31890 & AU 2.1.1 Start
llValid = oRE.test(ccValue)
&&oRE.Release()   &&Commented by Prajakta B. on 20022019 for AU 2.1.1
RELEASE oRE		  &&Modified by Prajakta B. on 20022019 for AU 2.1.1
Return llValid
&&Modified by Priyanka B on 13022019 for Bug-31890 & AU 2.1.1 End
