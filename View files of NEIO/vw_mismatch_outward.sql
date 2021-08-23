DROP VIEW [vw_mismatch_outward]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [vw_mismatch_outward] AS
SELECT    H.entry_ty,
		  H.Tran_cd,	
		  GSTN = (CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.gstin, '')  ELSE isnull(ac.gstin, '') END)  ,
		  Party_Name = (CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.Mailname, '') ELSE isnull(ac.ac_name, '') END),
		  H.inv_no AS 'Bill/Trans. No.',
		  H.date AS 'Bill/Trans. Date',
		 IT.HSNCODE AS 'HSN/SAC',
		  D.u_asseamt AS 'Taxable Value',
		  --D.IGST_PER AS 'IGST Rate',
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then 0 else d.GSTRATE end else D.IGST_PER end AS 'IGST Rate',
		  D.IGST_AMT AS 'IGST Amount',
		  --D.CGST_PER AS 'CGST Rate', 
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then d.GSTRATE/2 else 0 end else D.CGST_PER end AS 'CGST Rate',
		  D.CGST_AMT AS 'CGST Amount',
		  --D.SGST_PER AS 'SGST Rate',
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then d.GSTRATE/2 else 0 end else D.SGST_PER end AS 'SGST Rate',
		  D.SGST_AMT AS 'SGST Amount',
		  H.transtatus,
		  H.GSTSTATE,
		  ac1.State,
		  D.LineRule,
		  D.GSTRATE
		  
		  	   
FROM         STMAIN H INNER JOIN
                      STITEM D ON (H.ENTRY_TY = D.ENTRY_TY AND H.TRAN_CD = D.TRAN_CD) INNER JOIN
                      IT_MAST IT ON (D.IT_CODE = IT.IT_CODE) INNER JOIN
                       ac_mast ac1 ON (h.ac_id = ac1.ac_id)  LEFT OUTER JOIN
                      shipto ON (shipto.shipto_id = h.scons_id) LEFT OUTER JOIN
                      ac_mast ac ON (h.cons_id = ac.ac_id)        
		WHERE H.ENTRY_TY IN ('ST')                      
union all
SELECT	  H.entry_ty,
		  H.Tran_cd,
		  GSTN = (CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.gstin, '')  ELSE isnull(ac.gstin, '') END)  ,
		  Party_Name = (CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.Mailname, '') ELSE isnull(ac.ac_name, '') END),
		  H.inv_no AS 'Bill/Trans. No.',
		  H.date AS 'Bill/Trans. Date',
		 IT.HSNCODE AS 'HSN/SAC',
		  D.u_asseamt AS 'Taxable Value',
		  --D.IGST_PER AS 'IGST Rate',
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then 0 else d.GSTRATE end else D.IGST_PER end AS 'IGST Rate',
		  D.IGST_AMT AS 'IGST Amount',
		  --D.CGST_PER AS 'CGST Rate', 
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then d.GSTRATE/2 else 0 end else D.CGST_PER end AS 'CGST Rate',
		  D.CGST_AMT AS 'CGST Amount',
		  --D.SGST_PER AS 'SGST Rate',
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then d.GSTRATE/2 else 0 end else D.SGST_PER end AS 'SGST Rate',
		  D.SGST_AMT AS 'SGST Amount',
		  H.transtatus,
		  H.GSTSTATE,
		  ac1.State,
		  D.LineRule,
		  D.GSTRATE
		  	   
FROM         SBMAIN H INNER JOIN
                      SBITEM D ON (H.ENTRY_TY = D.ENTRY_TY AND H.TRAN_CD = D.TRAN_CD) INNER JOIN
                      IT_MAST IT ON (D.IT_CODE = IT.IT_CODE) INNER JOIN
                       ac_mast ac1 ON (h.ac_id = ac1.ac_id)  LEFT OUTER JOIN
                      shipto ON (shipto.shipto_id = h.scons_id) LEFT OUTER JOIN
                      ac_mast ac ON (h.cons_id = ac.ac_id) 
			WHERE H.ENTRY_TY='S1'                      
union all
SELECT	  H.entry_ty,
		  H.Tran_cd,
		  GSTN = (CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.gstin, '')  ELSE isnull(ac.gstin, '') END)  ,
		  Party_Name = (CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.Mailname, '') ELSE isnull(ac.ac_name, '') END),
		  H.inv_no AS 'Bill/Trans. No.',
		  H.date AS 'Bill/Trans. Date',
		  IT.HSNCODE AS 'HSN/SAC',
		  D.u_asseamt AS 'Taxable Value',
		  --D.IGST_PER AS 'IGST Rate',
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then 0 else d.GSTRATE end else D.IGST_PER end AS 'IGST Rate',
		  D.IGST_AMT AS 'IGST Amount',
		  --D.CGST_PER AS 'CGST Rate', 
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then d.GSTRATE/2 else 0 end else D.CGST_PER end AS 'CGST Rate',
		  D.CGST_AMT AS 'CGST Amount',
		  --D.SGST_PER AS 'SGST Rate',
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then d.GSTRATE/2 else 0 end else D.SGST_PER end AS 'SGST Rate',
		  D.SGST_AMT AS 'SGST Amount',
		  H.transtatus,
		  H.GSTSTATE,
		  ac1.State,
		  D.LineRule,
		  D.GSTRATE
		  	   
FROM         CNMAIN H INNER JOIN
                      CNITEM D ON (H.ENTRY_TY = D.ENTRY_TY AND H.TRAN_CD = D.TRAN_CD) INNER JOIN
                      IT_MAST IT ON (D.IT_CODE = IT.IT_CODE) INNER JOIN
                       ac_mast ac1 ON (h.ac_id = ac1.ac_id)  LEFT OUTER JOIN
                      shipto ON (shipto.shipto_id = h.scons_id) LEFT OUTER JOIN
                      ac_mast ac ON (h.cons_id = ac.ac_id) 
        Where H.againstgs in ('SALES','SERVICE INVOICE')   AND H.entry_ty IN ('GC','C6')           
union all
SELECT	  H.entry_ty,
		  H.Tran_cd,
		  GSTN = (CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.gstin, '')  ELSE isnull(ac.gstin, '') END)  ,
		  Party_Name = (CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.Mailname, '') ELSE isnull(ac.ac_name, '') END),
		  H.inv_no AS 'Bill/Trans. No.',
		  H.date AS 'Bill/Trans. Date',
		 IT.HSNCODE AS 'HSN/SAC',
		  D.u_asseamt AS 'Taxable Value',
		  --D.IGST_PER AS 'IGST Rate',
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then 0 else d.GSTRATE end else D.IGST_PER end AS 'IGST Rate',
		  D.IGST_AMT AS 'IGST Amount',
		  --D.CGST_PER AS 'CGST Rate', 
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then d.GSTRATE/2 else 0 end else D.CGST_PER end AS 'CGST Rate',
		  D.CGST_AMT AS 'CGST Amount',
		  --D.SGST_PER AS 'SGST Rate',
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then d.GSTRATE/2 else 0 end else D.SGST_PER end AS 'SGST Rate',
		  D.SGST_AMT AS 'SGST Amount',
		  H.transtatus,
		  H.GSTSTATE,
		  ac1.State,
		  D.LineRule,
		  D.GSTRATE
		  	   
FROM         DNMAIN H INNER JOIN
                      DNITEM D ON (H.ENTRY_TY = D.ENTRY_TY AND H.TRAN_CD = D.TRAN_CD) INNER JOIN
                      IT_MAST IT ON (D.IT_CODE = IT.IT_CODE) INNER JOIN
                       ac_mast ac1 ON (h.ac_id = ac1.ac_id)  LEFT OUTER JOIN
                      shipto ON (shipto.shipto_id = h.scons_id) LEFT OUTER JOIN
                      ac_mast ac ON (h.cons_id = ac.ac_id) 
        Where H.againstgs in ('SALES','SERVICE INVOICE')    AND H.entry_ty IN ('GD','D6')                      
union all
SELECT	  H.entry_ty,
		  H.Tran_cd,
		  GSTN = (CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.gstin, '')  ELSE isnull(ac.gstin, '') END)  ,
		  Party_Name = (CASE WHEN isnull(H.scons_id, 0) > 0 THEN isnull(shipto.Mailname, '') ELSE isnull(ac.ac_name, '') END),
		  H.inv_no AS 'Bill/Trans. No.',
		  H.date AS 'Bill/Trans. Date',
		 IT.HSNCODE AS 'HSN/SAC',
		  D.u_asseamt AS 'Taxable Value',
		  --D.IGST_PER AS 'IGST Rate',
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then 0 else d.GSTRATE end else D.IGST_PER end AS 'IGST Rate',
		  D.IGST_AMT AS 'IGST Amount',
		  --D.CGST_PER AS 'CGST Rate', 
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then d.GSTRATE/2 else 0 end else D.CGST_PER end AS 'CGST Rate',
		  D.CGST_AMT AS 'CGST Amount',
		  --D.SGST_PER AS 'SGST Rate',
		  case when D.linerule='exempted' then case when h.GSTState=ac1.State then d.GSTRATE/2 else 0 end else D.SGST_PER end AS 'SGST Rate',
		  D.SGST_AMT AS 'SGST Amount',
		  H.transtatus,
		  H.GSTSTATE,
		  ac1.State,
		  D.LineRule,
		  D.GSTRATE
		  	   
FROM         SRMAIN H INNER JOIN
                      SRITEM D ON (H.ENTRY_TY = D.ENTRY_TY AND H.TRAN_CD = D.TRAN_CD) INNER JOIN
                      IT_MAST IT ON (D.IT_CODE = IT.IT_CODE) INNER JOIN
                       ac_mast ac1 ON (h.ac_id = ac1.ac_id)  LEFT OUTER JOIN
                      shipto ON (shipto.shipto_id = h.scons_id) LEFT OUTER JOIN
                      ac_mast ac ON (h.cons_id = ac.ac_id) 
		WHERE H.ENTRY_TY='SR'
GO
