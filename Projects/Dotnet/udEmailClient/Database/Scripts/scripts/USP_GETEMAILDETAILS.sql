IF EXISTS(SELECT * FROM SYSOBJECTS WHERE [NAME]='USP_GETEMAILDETAILS' AND XTYPE='P')
BEGIN
	DROP PROCEDURE USP_GETEMAILDETAILS
END
GO
Create PROCEDURE USP_GETEMAILDETAILS
@ACID INT
AS
SELECT EMAIL FROM AC_MAST WHERE AC_ID=@ACID

