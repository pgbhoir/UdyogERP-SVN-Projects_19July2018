If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_Barcode_Data')
Begin
	Drop Procedure USP_REP_Barcode_Data
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:	Divyang Panchal
-- Create date: 01/06/2020
-- Description:	This Strored procedure is useful for Generation of Grid Data of Barcode Printing at Transaction level.
-- Remark:
-- =============================================

Create PROCEDURE [dbo].[USP_REP_Barcode_Data]
	@Entry_ty varchar(2),@Tran_cd nvarchar(10)
	AS

Declare @SQLCOMMAND as NVARCHAR(4000),@TBLNAME varchar(50)

	SET @SQLCOMMAND = ''
	SET @SQLCOMMAND = 'select 1 as sel,m.party_nm,m.inv_no,item_no='''',ItemName='''',code=m.ac_id,m.Tran_cd,itserial='''',stran_cd=0,SERIALNO='''',Header=''Headerwise''
						from '+@Entry_ty+'MAIN M 
						inner join BARCODEMAST B on (B.Entry_ty=m.entry_ty and BC_HD=''H'')
						where M.entry_ty='''+@Entry_ty+''' and m.Tran_cd='+@Tran_cd+'
						union
						select 1 as sel,i.party_nm,i.inv_no,i.item_no,ItemName=i.item,code=i.It_code,i.Tran_cd,i.itserial,stran_cd=0,SERIALNO='''',Header=''Itemwise''
						from '+@Entry_ty+'ITEM I
						inner join BARCODEMAST B on (B.Entry_ty=I.entry_ty and BC_HD=''D'')
						where I.entry_ty='''+@Entry_ty+''' and i.Tran_cd='+@Tran_cd+'
						union
						select 1 as sel,party_nm='''',inv_no=s.InInv_No,item_no=i.item_no,ItemName=s.It_Name,code=s.It_Code,Tran_cd=s.InTran_cd,itserial=s.InItserial,STran_cd=s.iTran_cd,S.SERIALNO,Header=''Serialize''
						from It_SrStk S
						inner join '+@Entry_ty+'ITEM I on (i.Tran_cd=s.InTran_cd and i.entry_ty=s.InEntry_ty and i.itserial=s.InItserial)
						inner join BARCODEMAST B on (B.Entry_ty=S.InEntry_ty and BC_HD=''S'')
						where S.InEntry_ty='''+@Entry_ty+''' and s.InTran_cd='+@Tran_cd+'  '
	PRINT @SQLCOMMAND
	EXECUTE SP_EXECUTESQL @SQLCOMMAND





