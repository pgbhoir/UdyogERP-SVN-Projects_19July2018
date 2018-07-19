DROP PROCEDURE [USP_REP_PLAPayment]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [USP_REP_PLAPayment]
	@ENTRYCOND NVARCHAR(254)
	AS
Declare @SQLCOMMAND as NVARCHAR(4000),@TBLCON as NVARCHAR(4000)

	SET @TBLCON=RTRIM(@ENTRYCOND)
	
	Declare @TBLNM as VARCHAR(50),@TBLNAME1 as VARCHAR(50),@TBLNAME2 as VARCHAR(50),@TBLNAME3 as VARCHAR(50)
	
	Set @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
					+ (DATEPART(ss, GETDATE()) * 1000 )
					+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
	Set @TBLNAME1 = '##TMP1'+@TBLNM
	Set @TBLNAME2 = '##TMP2'+@TBLNM
	Set @TBLNAME3 = '##TMP3'+@TBLNM

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'SELECT 1 as GRP,A.TRAN_CD,A.ENTRY_TY,A.DATE,A.INV_NO,A.INV_SR,A.CHEQ_NO,A.NET_AMT,
		CONVERT(VARCHAR(254),A.NARR) AS NARR, B.AC_NAME,B.AMT_TY, B.AMOUNT, C.AC_ID ,C.ADD1,C.ADD2,C.ADD3,C.CITY,C.ZIP,
		MailName=(CASE WHEN ISNULL(c.MailName,'' '')='' '' THEN c.ac_name ELSE c.mailname END)
		INTO '+@TBLNAME1+' FROM BPMAIN A 
		LEFT JOIN BPACDET B ON A.ENTRY_TY = B.ENTRY_TY AND A.TRAN_CD = B.TRAN_CD 
		LEFT JOIN AC_MAST C ON B.AC_ID=C.AC_ID WHERE '+RTRIM(@TBLCON)
	EXECUTE SP_EXECUTESQL @SQLCOMMAND


	SET @SQLCOMMAND=' '
	SET @SQLCOMMAND= 'SELECT 2 as GRP,A.TRAN_CD,A.ENTRY_TY,
		A.DATE,B.INV_NO,
		INV_SR='' '',CHEQ_NO='' '',A.NET_AMT,NARR='' '',AC_NAME='' '',AMT_TY='' '',
		AMOUNT=ISNULL(B.NEW_ALL,0)+ISNULL(B.TDS,0)+ISNULL(B.DISC,0),AC_ID=0,ADD1='' '', ADD2='' '',ADD3='' '',CITY='' '',ZIP='' '',Mailname='' ''
	    INTO '+@TBLNAME2 +' FROM '+@TBLNAME1+' A 
	    INNER JOIN MAINALL_VW B ON (A.entry_ty=B.entry_all and A.tran_cd =B.main_tran and A.AC_ID=B.AC_ID) '
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND=' '
	SET @SQLCOMMAND= 'SELECT 2 as GRP,A.TRAN_CD,A.ENTRY_TY,
		A.DATE,B.INV_NO,
		INV_SR='' '',CHEQ_NO='' '',A.NET_AMT,NARR='' '',AC_NAME='' '',AMT_TY='' '',
		AMOUNT=ISNULL(B.NEW_ALL,0),AC_ID=0,ADD1='' '', ADD2='' '',ADD3='' '',CITY='' '',ZIP='' '',Mailname='' ''
	    INTO '+@TBLNAME3 +' FROM '+@TBLNAME1+' A 
	    INNER JOIN MAINALL_VW B ON (A.entry_ty=B.entry_ty and A.tran_cd =B.tran_cd and A.AC_ID=B.AC_ID) '
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	
	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'SELECT * FROM '+@TBLNAME1+' A UNION ALL SELECT * FROM '+@TBLNAME2+' B  UNION ALL SELECT * FROM '+@TBLNAME3+' C  ORDER BY DATE,TRAN_CD,GRP'
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME1
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME2
	EXECUTE SP_EXECUTESQL @SQLCOMMAND

	SET @SQLCOMMAND = 'DROP TABLE '+@TBLNAME3
	EXECUTE SP_EXECUTESQL @SQLCOMMAND
GO
