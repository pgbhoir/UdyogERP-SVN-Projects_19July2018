IF EXISTS(SELECT XTYPE,NAME FROM SYSOBJECTS WHERE XTYPE = 'V' AND NAME ='Eou_LMain_vw')
BEGIN
	DROP VIEW Eou_LMain_vw
END

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE  VIEW [dbo].[Eou_LMain_vw]
AS
SELECT     Tran_cd, entry_ty, date, doc_no, inv_no, l_yn, Ac_id, [rule], dept, inv_sr, cate, pinvno AS u_beno, pinvdt as u_pinvdt
FROM         dbo.PTMAIN
UNION
SELECT     Tran_cd, entry_ty, date, doc_no, inv_no, l_yn, Ac_id, [rule], dept, inv_sr, cate, Pinvno AS u_beno, pinvdt as U_Pinvdt
FROM         dbo.IRMAIN
UNION
SELECT     Tran_cd, entry_ty, date, doc_no, inv_no, l_yn, Ac_id, [rule], dept, inv_sr, cate, Pinvno AS u_beno, pinvdt as U_Pinvdt
FROM         dbo.ARMAIN
UNION
SELECT     Tran_cd, entry_ty, date, doc_no, inv_no, l_yn, Ac_id, [rule], dept, inv_sr, cate, Pinvno AS u_beno, pinvdt as u_pinvdt
FROM         dbo.OSMAIN
UNION
SELECT     Tran_cd, entry_ty, date, doc_no, inv_no, l_yn, Ac_id, [rule], dept, inv_sr, cate, Pinvno AS u_beno, pinvdt as u_pinvdt
FROM         dbo.SRMAIN			

UNION
SELECT     Tran_cd, entry_ty, date, doc_no, inv_no, l_yn, Ac_id, [rule], dept, inv_sr, cate, Pinvno AS u_beno, pinvdt as u_pinvdt
FROM         dbo.OPMAIN	--Added OPMAIN table for Bug-31112 by Suraj K. on 09/01/2018

UNION
SELECT     Tran_cd, entry_ty, date, doc_no, inv_no, l_yn, Ac_id, [rule], dept, inv_sr, cate, Pinvno AS u_beno, pinvdt as u_pinvdt
FROM         dbo.ESMAIN	--Added by Sachin N. S. on 05/04/2018 for Bug-31382

GO


