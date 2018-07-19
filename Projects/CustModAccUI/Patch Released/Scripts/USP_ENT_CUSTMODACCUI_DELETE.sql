IF EXISTS(SELECT * FROM SYSOBJECTS WHERE [NAME]='USP_ENT_CUSTMODACCUI_DELETE' AND XTYPE='P')
BEGIN
	DROP PROCEDURE USP_ENT_CUSTMODACCUI_DELETE
END
GO
Create Procedure [dbo].[USP_ENT_CUSTMODACCUI_DELETE]
@id varchar(20)
As
Begin
	Delete From custfeature Where id=@id
	Delete From custmnutranrptdts Where fk_id=@id
End
