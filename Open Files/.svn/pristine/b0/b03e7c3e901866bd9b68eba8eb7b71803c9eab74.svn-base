IF EXISTS(SELECT * FROM SYSOBJECTS WHERE XTYPE='FN' AND [NAME]='REPLACEASCII')
BEGIN
	DROP FUNCTION REPLACEASCII
END
go
CREATE FUNCTION [dbo].[ReplaceASCII](@inputString VARCHAR(8000))
RETURNS VARCHAR(8000)
AS
     BEGIN
         DECLARE @badStrings VARCHAR(100);
         DECLARE @increment INT= 1;
         WHILE @increment <= DATALENGTH(@inputString)
             BEGIN
                 IF((ASCII(SUBSTRING(@inputString, @increment, 1)) not between 65 and 90)
				 and (ASCII(SUBSTRING(@inputString, @increment, 1)) not between 97 and 122) and (ASCII(SUBSTRING(@inputString, @increment, 1)) <> 32))
                 BEGIN
                    SET @badStrings = CHAR(ASCII(SUBSTRING(@inputString, @increment, 1)));
                    SET @inputString = replace(REPLACE(@inputString, @badStrings, ' '),'  ',' ')
                 END
                 SET @increment = @increment + 1
             END
         RETURN @inputString
     END
GO
