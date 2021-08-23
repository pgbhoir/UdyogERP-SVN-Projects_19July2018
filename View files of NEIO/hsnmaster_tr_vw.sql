DROP VIEW [hsnmaster_tr_vw]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [hsnmaster_tr_vw] as

select a.inv_no,a.date,a.ITEM,a.it_code,a.ac_id,b.party_nm,b.sac_id,b.cons_id,b.scons_id,(select Gst_State_Code from STATE where state=c.state) state,d.hsncode from  LITEM_VW a
inner join Lmain_VW b
on a.entry_ty=b.entry_ty
and 
a.Tran_cd=b.Tran_cd
inner join AC_MAST c
on a.Ac_id=c.Ac_id
inner join it_mast d
on a.It_code=d.It_code
GO
