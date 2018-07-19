DROP PROCEDURE [USP_ADD_MISSING_FLDS_AMENDTBL]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [USP_ADD_MISSING_FLDS_AMENDTBL]
(@TBLTYPE VARCHAR(2) )
AS
BEGIN
	DECLARE @SQLSTR NVARCHAR(MAX)
	DECLARE @TBLNM VARCHAR(20), @FLDNM VARCHAR(100), @FLDTYP VARCHAR(100), @MAX_LENGTH NUMERIC(10), @PRECISION NUMERIC(5), @SCALE NUMERIC(5), @FLDSTRU VARCHAR(200)
	SET @SQLSTR = 'DECLARE ADDFLDLIST CURSOR FOR 
			SELECT A.[name]+''AM'' AS TBLNM, B.[name] AS FLDNM, C.[NAME] AS FLDTYP, B.[max_length], B.[precision], B.[SCALE],
					FLDSTRU=B.[name]+'' ''+
					CASE 
						WHEN C.[NAME]=''TEXT'' THEN
							C.[NAME]+'' DEFAULT '''''''' WITH VALUES ''
						WHEN C.[NAME]=''INT'' THEN
							C.[NAME]+'' DEFAULT 0 WITH VALUES ''
						WHEN C.[NAME]=''TINYINT'' THEN
							C.[NAME]+'' DEFAULT 0 WITH VALUES ''
						WHEN C.[NAME]=''SMALLDATETIME'' THEN
							C.[NAME]+'' DEFAULT '''''''' WITH VALUES ''
						WHEN C.[NAME]=''DATETIME'' THEN
							C.[NAME]+'' DEFAULT '''''''' WITH VALUES ''
						WHEN C.[NAME]=''BIT'' THEN
							C.[NAME]+'' DEFAULT 0 WITH VALUES ''
						WHEN C.[NAME]=''DECIMAL'' THEN
							C.[NAME]+''(''+CAST(B.[precision] AS VARCHAR)+'',''+CAST(B.[SCALE] AS VARCHAR)+'') DEFAULT 0 WITH VALUES ''
						WHEN C.[NAME]=''NUMERIC'' THEN
							C.[NAME]+''(''+CAST(B.[precision] AS VARCHAR)+'',''+CAST(B.[SCALE] AS VARCHAR)+'') DEFAULT 0 WITH VALUES ''
						WHEN C.[NAME]=''VARCHAR'' THEN
							C.[NAME]+''(''+CAST(B.[MAX_LENGTH] AS VARCHAR)+'') DEFAULT '''''''' WITH VALUES ''
						WHEN C.[NAME]=''CHAR'' THEN
							C.[NAME]+''(''+CAST(B.[MAX_LENGTH] AS VARCHAR)+'') DEFAULT '''''''' WITH VALUES ''
					END				FROM SYS.TABLES A
					INNER JOIN SYS.COLUMNS B ON A.OBJECT_ID=B.OBJECT_ID
					INNER JOIN SYS.TYPES C ON B.SYSTEM_TYPE_ID=C.SYSTEM_TYPE_ID
				WHERE A.[name] IN ('+CHAR(39)+@TBLTYPE+'MAIN'+CHAR(39)+','+CHAR(39)+@TBLTYPE+'PTITEM'+CHAR(39)+','+CHAR(39)+@TBLTYPE+'PTACDET'+CHAR(39)+','+CHAR(39)+@TBLTYPE+'PTITREF'+CHAR(39)+','+CHAR(39)+@TBLTYPE+'PTMALL'+CHAR(39)+','+CHAR(39)+@TBLTYPE+'PTCALL'+CHAR(39)+') AND
					RTRIM(A.[NAME])+''AM''+RTRIM(B.NAME) NOT IN 
						( SELECT RTRIM(A1.[NAME])+RTRIM(B1.NAME) 
							FROM SYS.TABLES A1
								INNER JOIN SYS.COLUMNS B1 ON A1.OBJECT_ID=B1.OBJECT_ID
							WHERE A1.[name] IN ('+CHAR(39)+@TBLTYPE+'MAINAM'+CHAR(39)+','+CHAR(39)+@TBLTYPE+'PTITEMAM'+CHAR(39)+','+CHAR(39)+@TBLTYPE+'PTACDETAM'+CHAR(39)+','+CHAR(39)+@TBLTYPE+'PTITREFAM'+CHAR(39)+','+CHAR(39)+@TBLTYPE+'PTMALLAM'+CHAR(39)+','+CHAR(39)+@TBLTYPE+'PTCALLAM'+CHAR(39)+')
						)'

	--PRINT @SQLSTR

	EXECUTE SP_EXECUTESQL @SQLSTR
	
	OPEN ADDFLDLIST
	FETCH NEXT FROM ADDFLDLIST INTO @TBLNM, @FLDNM, @FLDTYP, @MAX_LENGTH, @PRECISION, @SCALE, @FLDSTRU
	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXECUTE Add_Columns @TBLNM, @FLDSTRU
		FETCH NEXT FROM ADDFLDLIST INTO @TBLNM, @FLDNM, @FLDTYP, @MAX_LENGTH, @PRECISION, @SCALE, @FLDSTRU
	END
	
	CLOSE ADDFLDLIST
	DEALLOCATE ADDFLDLIST
	
END

--SELECT * FROM SYS.TYPES
GO
