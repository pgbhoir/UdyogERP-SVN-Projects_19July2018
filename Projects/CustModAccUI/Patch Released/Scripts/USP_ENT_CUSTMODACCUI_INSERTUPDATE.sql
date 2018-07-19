IF EXISTS(SELECT * FROM SYSOBJECTS WHERE [NAME]='USP_ENT_CUSTMODACCUI_INSERTUPDATE' AND XTYPE='P')
BEGIN
	DROP PROCEDURE USP_ENT_CUSTMODACCUI_INSERTUPDATE
END
GO
Create Procedure [dbo].[USP_ENT_CUSTMODACCUI_INSERTUPDATE]
@action varchar(10),
@id varchar(5)='',
@date smalldatetime='',
@rcomp varchar(100)='',
@prodname varchar(50)='',
@prodver varchar(15)='',
@rmacid varchar(100)='',
--@ccomp varchar(100)='',
--@optiontype varchar(10)='',
--@desc1 varchar(150)='',
--@desc2 varchar(150)='',
----@detail custmnutranrptdts_type readonly,
@bug varchar(50)='',
@pono varchar(20)='',
@podate smalldatetime='',
@poamt numeric(12,2)=0,
@apprby varchar(30)='',
@remarks varchar(100)='',
@username varchar(30)=''
As
Begin
	If rtrim(upper(@action)) = 'INSERT'
	Begin
		Insert into custfeature(id,[date],rcomp,prodname,prodver,rmacid,bug,pono,podate,poamt,apprby,remarks,[user_name])
		values(@id,@date,@rcomp,@prodname,@prodver,@rmacid,@bug,@pono,@podate,@poamt,@apprby,@remarks,@username)
		
		--Insert into custmnutranrptdts(fk_id,ccomp,optiontype,desc1,desc2)
		----select fk_id,ccomp,optiontype,desc1,desc2 from @detail
		--values(@id,@ccomp,@optiontype,@desc1,@desc2)
	End
	
	If rtrim(upper(@action)) = 'UPDATE'
	Begin
		Update custfeature set [date]=@date,rcomp=@rcomp,prodname=@prodname,prodver=@prodver,rmacid=@rmacid,
		bug=@bug,pono=@pono,podate=@podate,poamt=@poamt,apprby=@apprby,remarks=@remarks,[user_name]=@username Where id=@id
		
		--Update custmnutranrptdts set ccomp=@ccomp,optiontype=@optiontype,desc1=@desc1,desc2=@desc2
		--Where fk_id=@id
	End
End