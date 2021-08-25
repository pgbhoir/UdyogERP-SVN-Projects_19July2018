
If Upper(MVU_USER_ROLES) != 'ADMINISTRATOR'
	=Messagebox("Only User's having Administrator Rights can Run this Update",64,vumess)
	Do ExitUpdt
	Return  .F.
Endif

#Include AutoUpdater.h
Public mudprodcode,usquarepass,_mproddesc,_mupdtmonth,_mIntVersion,_mlastupdtmonth,_mFldrName,_mDataBackFldrName,_mManualUpdateFldrName,_mFinalUpdateFldrName,_ErrMsg,_mmachine,_mlogip,_mErrHtmlName,_GenFreshProdDetail,_AutoMainPath,_mDocFldrName,_mlastupdtversion	&&vasant280312
Public _mCheckUpdtHistTable,_mUpdateUpdtHistTable,_mAutoUpdaterCaption

mudprodcode 	= ''
usquarepass		= ''
_mproddesc 		= ''
_mupdtmonth		= {}
_mIntVersion	= ''
_mlastupdtmonth = {}
_mlastupdtversion = ''		&&vasant280312
_mFldrName 		= ''
_mDataBackFldrName		= ''
_mManualUpdateFldrName	= ''
_mFinalUpdateFldrName	= ''
_ErrMsg			= ''
_mmachine 		= ''
_mlogip   		= ''
_mErrHtmlName	= ''
_AutoMainPath	= ''
_mDocFldrName	= ''
_mCheckUpdtHistTable	= .T.
_mUpdateUpdtHistTable	= .T.
_mAutoUpdaterCaption	= ''

_AutoMainPath	= Addbs(Apath)
_mFldrName    	= _AutoMainPath+'Monthly Updates\'
_mErrHtmlName	= _mFldrName+'Update Log '+Strtran(Strtran(Ttoc(Datetime()),'/','-'),':','-')+'.html'
_GenFreshProdDetail = .F.
_varIntVersion	= ''

mudprodcode = dec(NewDecry(GlobalObj.getPropertyval("UdProdCode"),'Ud*yog+1993'))

usquarepass = Upper(dec(NewDecry(GlobalObj.getPropertyval('EncryptId'),'Ud*_yog*\+1993')))

_mproddesc  = GlobalObj.getPropertyval("ProductTitle")

_mlastupdtmonth 	= Ctod(_DefineLastUpdtMonth)

_mlastupdtversion	= _DefineLastUpdtVersion

_GenFreshProdDetail = _DefineGenFreshProdDetail

_mCheckUpdtHistTable	= _DefineCheckUpdtHistTable

_mUpdateUpdtHistTable	= _DefineUpdateUpdtHistTable

_mAutoUpdaterCaption	= _DefineAutoUpdaterCaption

nretval=0
nhandle=0
nhandle_master=0
sqlconobj=Newobject('sqlconnudobj',"sqlconnection",xapps)

msqlstr = "select name from sysobjects where xtype = 'U' and name = 'UPDTHIST'"
nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpUpdtHist","nHandle")
If nretval <= 0 Or !Used('_tmpUpdtHist')
	Do ExitUpdt
	Return  .F.
Endif

msqlstr = "select top 1 a.name from syscolumns a,sysobjects b ;
	where a.id = b.id and a.name = 'updtdetail'  and b.name = 'updthist'"
nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")
If nretval <= 0 Or !Used('_tmpCoList')
	Do ExitUpdt
	Return  .F.
Else
	If Reccount('_tmpCoList') <= 0
		msqlstr = "Alter Table UPDTHIST Add UPDTDETAIL VarBinary(8000) Default CAST('' as varbinary(1)) With Values"
		nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"","nHandle")
		If nretval <= 0
			Do ExitUpdt
			Return  .F.
		Endif
	Endif
Endif

msqlstr = "select top 1 co_name, dbname from co_mast where 1=2"
nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_tmpCoList","nHandle")

Insert Into _tmpCoList Values('Main Folder','Vudyog')


msqlstr = "select Distinct co_name, dbname from co_mast where com_type<>'M' Order by co_name"
nretval=sqlconobj.dataconn('EXE','Vudyog',msqlstr,"_CoList","nHandle")

Select _tmpCoList
Append From Dbf('_CoList')

Select _tmpCoList
Locate
Scan
	Select _tmpCoList
	_cCoName = Alltrim(_tmpCoList.co_Name)
	_cDBName = Alltrim(_tmpCoList.DbName)

	Wait Window "Updating Company : "+_cCoName Nowait

	msqlstr = "select top 1 b.name from sysobjects b where b.name = 'updthist' "
	nretval=sqlconobj.dataconn('EXE',_cDBName,msqlstr,"_tmpCoList1","nHandle")
	If nretval <= 0 Or !Used('_tmpCoList1')
		Do ExitUpdt
		Return  .F.
	Else
		msqlstr=""
		If Reccount('_tmpCoList1') <= 0
			msqlstr = msqlstr+"CREATE TABLE [dbo].[updthist]([UpdtMonth] [datetime] NULL,[UpdtVersion] [varchar](15) NULL,[UpdtDate] [datetime] NULL, "
			msqlstr = msqlstr+"	[User] [varchar](15) NULL,[Log_machine] [varchar](25) NULL,[Log_ip] [varchar](15) NULL,[UpdtId] [int] IDENTITY(1,1) NOT NULL, "
			msqlstr = msqlstr+"	[IntVersion] [varchar](15) NULL,[UpdtDone] [bit] NULL,[UPDTDETAIL] [varbinary](8000) NULL "
			msqlstr = msqlstr+") ON [PRIMARY] "
			nretval=sqlconobj.dataconn('EXE',_cDBName,msqlstr,"_tmpCoList1","nHandle")
		Endif
	Endif

	msqlstr = "select top 1 a.name from syscolumns a,sysobjects b ;
	where a.id = b.id and a.name = 'updtdetail'  and b.name = 'updthist'"
	nretval=sqlconobj.dataconn('EXE',_cDBName,msqlstr,"_tmpCoList1","nHandle")
	If nretval <= 0 Or !Used('_tmpCoList1')
		Do ExitUpdt
		Return  .F.
	Else
		If Reccount('_tmpCoList1') <= 0
			msqlstr = "Alter Table UPDTHIST Add UPDTDETAIL VarBinary(8000) Default CAST('' as varbinary(1)) With Values"
			nretval=sqlconobj.dataconn('EXE',_cDBName,msqlstr,"","nHandle")
			If nretval <= 0
				Do ExitUpdt
				Return  .F.
			Endif
		Endif
	Endif

	_cDate = Ctod('22/11/2019')
	msqlstr = " Select [UpdtMonth],[UpdtVersion],UpdtDate=RTRIM(Convert(Varchar(50),UpdtDate,121)),[User],[Log_machine],[Log_ip],[UpdtId],[IntVersion],[UpdtDone],[UPDTDETAIL] "+;
		" From UpdtHist Where UpdtMonth <= ?_cDate "
	nretval=sqlconobj.dataconn('EXE',_cDBName,msqlstr,"_CoUpdtHist","nHandle")

	If Used('_CoUpdtHist')
		Select _CoUpdtHist
		Scan
			Select _CoUpdtHist
			_EncText	= _cCoName

			_mupdtmonth = _CoUpdtHist.UpdtMonth
			_mVersion1 = _CoUpdtHist.UpdtVersion
			_mEntDate = _CoUpdtHist.UpdtDate
			mUsername1 = _CoUpdtHist.User
			_mmachine1 = _CoUpdtHist.Log_Machine
			_mlogip1 = _CoUpdtHist.Log_Ip
			UpdateError1 = _CoUpdtHist.UpdtDone

			cStr = ""
			cStr = cStr+"<~*0*~>"+Transform(_EncText)+"<~*0*~>"
			cStr = cStr+"<~*1*~>"+Transform(_mupdtmonth)+"<~*1*~>"
			cStr = cStr+"<~*2*~>"+Transform(_mVersion1)+"<~*2*~>"
			cStr = cStr+"<~*3*~>"+Transform(_mEntDate)+"<~*3*~>"
			cStr = cStr+"<~*4*~>"+Transform(mUsername1)+"<~*4*~>"
			cStr = cStr+"<~*5*~>"+Transform(_mmachine1)+"<~*5*~>"
			cStr = cStr+"<~*6*~>"+Transform(_mlogip1)+"<~*6*~>"
			cStr = cStr+"<~*7*~>"+Iif(UpdateError1 = .T.,"1","0")+"<~*7*~>"

			cStr = newencry(cStr,'Udencyogprod')

			cEnc = Cast(cStr As Blob)

			msqlstr = "Update UpdtHist set [UpdtDetail]=?cEnc where "
			msqlstr = msqlstr+" [UpdtMonth] = ?_mupdtmonth "
			msqlstr = msqlstr+" And [UpdtVersion] = ?_mVersion1 "
			msqlstr = msqlstr+" And [UpdtDate] = ?_mEntDate "
			msqlstr = msqlstr+" And ISNULL([User],'') = "+Iif(Isnull(mUsername1),"''","?mUsername1 ")
			msqlstr = msqlstr+" And ISNULL([Log_Machine],'') = "+Iif(Isnull(_mmachine1),"''","?_mmachine1 ")
			msqlstr = msqlstr+" And ISNULL([Log_Ip],'') = "+Iif(Isnull(_mlogip1),"''","?_mlogip1 ")
			msqlstr = msqlstr+" And [UpdtDone] = "+Iif(UpdateError1 = .T.,"1","0")

			nretval=sqlconobj.dataconn('EXE',_cDBName,msqlstr,"","nHandle")
			Select _CoUpdtHist
		Endscan
	Endif

	sqlconobj.sqlconnclose("nHandle")

	Select _tmpCoList
Endscan


=Messagebox("Updation of Update History is done successfully....!!!",0,vumess)

Do ExitUpdt


************************************************************************************************************************************************
************************************************************************************************************************************************

Procedure ExitUpdt
	If Type('sqlconobj') = 'O'
		nretval=sqlconobj.sqlconnclose("nHandle")
		nretval=sqlconobj.sqlconnclose("nhandle_master")
	Endif
	If Used('_tmpCoList')
		Use In _tmpCoList
	Endif
	If Used('_UpdtCoList')
		Use In _UpdtCoList
	Endif
	If Used('_tmptbl1')
		Use In _tmptbl1
	Endif
	If Used('_ZipDetail')
		Use In _ZipDetail
	Endif
	If Used('ErrLog')
		Use In ErrLog
	Endif
	Release mudprodcode,usquarepass,_mproddesc,_mupdtmonth,_mIntVersion,_mlastupdtmonth,_mFldrName,_mDataBackFldrName,_mManualUpdateFldrName,_mFinalUpdateFldrName,_ErrMsg,_mmachine,_mlogip,_mErrHtmlName,_AutoMainPath,_mDocFldrName,_mlastupdtversion	&&vasant280312
	Release _mCheckUpdtHistTable,_mUpdateUpdtHistTable,_mAutoUpdaterCaption
	&&Close All
	&&Wait Clear
	exitclick = .T.

