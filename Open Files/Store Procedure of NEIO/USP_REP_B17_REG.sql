DROP PROCEDURE [USP_REP_B17_REG]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		
-- Create date: 01/08/2010	
-- Description:	This Stored procedure is useful to generate B-17 Bond Register.
-- Modify date: 
-- Modified By: 
-- Remark:
-- =============================================

CREATE PROCEDURE [USP_REP_B17_REG] 
@TMPAC NVARCHAR(50),@TMPIT NVARCHAR(50),@SPLCOND VARCHAR(8000),@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60)
,@SAMT FLOAT,@EAMT FLOAT
,@SDEPT AS VARCHAR(60),@EDEPT AS VARCHAR(60)
,@SCATE AS VARCHAR(60),@ECATE AS VARCHAR(60)
,@SWARE AS VARCHAR(60),@EWARE AS VARCHAR(60)
,@SINV_SR AS VARCHAR(60),@EINV_SR AS VARCHAR(60)
,@LYN VARCHAR(20)
,@EXPARA  AS VARCHAR(60)= null

AS
 
Declare @FCON as NVARCHAR(2000),@VSAMT DECIMAL(14,2),@VEAMT DECIMAL(14,2)

EXECUTE   USP_REP_FILTCON 
@VTMPAC =@TMPAC,@VTMPIT =@TMPIT,@VSPLCOND =@SPLCOND
,@VSDATE=@SDATE
 ,@VEDATE=@EDATE
,@VSAC =@SAC,@VEAC =@EAC
,@VSIT=@SIT,@VEIT=@EIT
,@VSAMT=@SAMT,@VEAMT=@EAMT
,@VSDEPT=@SDEPT,@VEDEPT=@EDEPT
,@VSCATE =@SCATE,@VECATE =@ECATE
,@VSWARE =@SWARE,@VEWARE  =@EWARE
,@VSINV_SR =@SINV_SR,@VEINV_SR =@SINV_SR
,@VMAINFILE=NULL,@VITFILE=NULL,@VACFILE=NULL
,@VDTFLD ='DATE'
,@VLYN =NULL
,@VEXPARA=@EXPARA
,@VFCON =@FCON OUTPUT


DECLARE @SQLCOMMAND NVARCHAR(4000), @VCOND NVARCHAR(2000)
DECLARE @TBLNM VARCHAR(50),@TBLNAME1 Varchar(50)

SELECT @TBLNM = (SELECT substring(rtrim(ltrim(str(RAND( (DATEPART(mm, GETDATE()) * 100000 )
		+ (DATEPART(ss, GETDATE()) * 1000 )+ DATEPART(ms, GETDATE())) , 20,15))),3,20) as No)
		
SELECT @TBLNAME1 = '##TMP1'+@TBLNM
SET @SQLCOMMAND='select POMAIN.entry_ty,POMAIN.date,ac_name=''BALANCE WITH B17-BOND'',amount=POMAIN.BCDAMT+POMAIN.U_CESSAMT+POMAIN.U_HCESAMT+POMAIN.CCDAMT+POMAIN.HCDAMT+POMAIN.U_CVDAMT ,
				amt_ty=''DR'',POMAIN.dept,POMAIN.cate,
				POMAIN.date AS u_cldt,POMAIN.[RULE],POmain.gro_amt,POmain.inv_no as inv_no,POMAIN.inv_no as inv_no1,POmain.BONDSRNO as u_b1no,POMAIN.BOND_NO AS BOND_NO,
				POmain.tot_examt, convert(varchar(254),POmain.narr) as narr,POmain.u_sbillno as u_pinvno,POmain.u_sbilldt as u_pinvdt, 
				SPACE(50) AS ARE1NO,cast(space(10) as datetime) AS AREDATE,0 AS tot_qty,pomain.beno,pomain.bedt 
				INTO '+@TBLNAME1+' from POMAIN 
				where POMAIN.entry_ty in (''PH'',''RC'')'
EXECUTE SP_EXECUTESQL @SQLCOMMAND

--II-
SET @SQLCOMMAND='INSERT INTO  '+@TBLNAME1+' 
				select lac_vw.entry_ty,lac_vw.date,lac_vw.ac_name,lac_vw.amount,lac_vw.amt_ty,lac_vw.dept,lac_vw.cate,
				lac_vw.u_cldt,IIMAIN.[RULE],IIMAIN.gro_amt,IIMAIN.inv_no as inv_no,lac_vw.inv_no as inv_no1,IIMAIN.BONDSRNO as u_b1no,SPACE(50) AS BOND_NO,
				IIMAIN.tot_examt as tot_examt,convert(varchar(254),IIMAIN.narr) as narr,IIMAIN.u_pinvno,IIMAIN.u_pinvdt, 
				SPACE(50) AS  ARE1NO,SPACE(50) AS AREDATE,0 AS tot_qty,space(50) as beno, space(50) as bedt
				from lac_vw 
				LEFT join IIMAIN on (lac_vw.tran_cd=IIMAIN.tran_cd)
				where lac_vw.ac_name =''INTER DEPT TRANSFER'' and lac_vw.entry_ty=''II'''EXECUTE SP_EXECUTESQL @SQLCOMMAND--IR--SET @SQLCOMMAND='INSERT INTO  '+@TBLNAME1+' 
				select lac_vw.entry_ty,lac_vw.date,lac_vw.ac_name,lac_vw.amount,lac_vw.amt_ty,lac_vw.dept,lac_vw.cate,
				lac_vw.u_cldt,IRMAIN.[RULE],IRMAIN.gro_amt,IRMAIN.inv_no as inv_no,lac_vw.inv_no as inv_no1,IRMAIN.BONDSRNO as u_b1no,SPACE(50) AS BOND_NO,
				IRMAIN.tot_examt as tot_examt,convert(varchar(254),IRMAIN.narr) as narr,IRMAIN.u_pinvno,IRMAIN.u_pinvdt, 
				SPACE(50) AS  ARE1NO,SPACE(50) AS AREDATE,0 AS tot_qty,space(50) as beno, space(50) as bedt
				from lac_vw 
				LEFT join IRMAIN on (lac_vw.tran_cd=IRMAIN.tran_cd)
				where lac_vw.ac_name =''BALANCE WITH B17-BOND'' and lac_vw.entry_ty=''IR'''EXECUTE SP_EXECUTESQL @SQLCOMMAND--BC-
SET @SQLCOMMAND='INSERT INTO  '+@TBLNAME1+' 
				select lac_vw.entry_ty,lac_vw.date,lac_vw.ac_name,lac_vw.amount,lac_vw.amt_ty,lac_vw.dept,lac_vw.cate,
				lac_vw.u_cldt,BPMAIN.[RULE],BPMAIN.gro_amt,BPMAIN.inv_no as inv_no,lac_vw.inv_no as inv_no1,BPMAIN.BONDSRNO as u_b1no,BPMAIN.BOND_NO AS BOND_NO,
				lac_vw.amount as tot_examt,convert(varchar(254),BPMAIN.narr) as narr,BPMAIN.u_pinvno,BPMAIN.u_pinvdt,
				 SPACE(50) AS ARE1NO,SPACE(50) AS AREDATE,0 AS tot_qty,space(50) as beno, space(50) as bedt
				from lac_vw 
				LEFT join BPMAIN on (lac_vw.tran_cd=BPMAIN.tran_cd)
				where lac_vw.ac_name =''BALANCE WITH B17-BOND'' and lac_vw.entry_ty=''BC'''EXECUTE SP_EXECUTESQL @SQLCOMMAND--BD-
SET @SQLCOMMAND='INSERT INTO  '+@TBLNAME1+' 
				select lac_vw.entry_ty,lac_vw.date,lac_vw.ac_name,lac_vw.amount,lac_vw.amt_ty,lac_vw.dept,lac_vw.cate,
				lac_vw.u_cldt,BRMAIN.[RULE],BRMAIN.gro_amt,BRMAIN.inv_no as inv_no,lac_vw.inv_no as inv_no1,BRMAIN.BONDSRNO as u_b1no,BRMAIN.BOND_NO AS BOND_NO,
				lac_vw.amount as tot_examt,convert(varchar(254),BRMAIN.narr) as narr,BRMAIN.u_pinvno,BRMAIN.u_pinvdt,
				SPACE(50) AS ARE1NO,SPACE(50) AS AREDATE,0 AS tot_qty,space(50) as beno, space(50) as bedt
				from lac_vw 
				LEFT join BRMAIN on (lac_vw.tran_cd=BRMAIN.tran_cd)
				where lac_vw.ac_name =''BALANCE WITH B17-BOND'' and lac_vw.entry_ty=''BD'''EXECUTE SP_EXECUTESQL @SQLCOMMAND--OB--SET @SQLCOMMAND='INSERT INTO  '+@TBLNAME1+' 
				select lac_vw.entry_ty,lac_vw.date,lac_vw.ac_name,lac_vw.amount,lac_vw.amt_ty,lac_vw.dept,lac_vw.cate,
				lac_vw.u_cldt,OBMAIN.[RULE],OBMAIN.gro_amt,OBMAIN.inv_no as inv_no,lac_vw.inv_no as inv_no1,OBMAIN.BONDSRNO as u_b1no,OBMAIN.BOND_NO AS BOND_NO,
				OBMAIN.tot_examt as tot_examt,convert(varchar(254),OBMAIN.narr)as narr,OBMAIN.u_pinvno,OBMAIN.u_pinvdt, 
				SPACE(50) AS  ARE1NO,SPACE(50) AS AREDATE,0 AS tot_qty,space(50) as beno, space(50) as bedt
				from lac_vw 
				LEFT join OBMAIN on (lac_vw.tran_cd=OBMAIN.tran_cd)
				where lac_vw.ac_name =''BALANCE WITH B17-BOND'' and lac_vw.entry_ty=''BB'''EXECUTE SP_EXECUTESQL @SQLCOMMAND--EI--SET @SQLCOMMAND=' INSERT INTO  '+@TBLNAME1+' 
				select EIACDET.entry_ty,EIACDET.date,EIACDET.ac_name,EIACDET.amount,EIACDET.amt_ty,EIACDET.dept,EIACDET.cate,
				EIACDET.u_cldt,EIMAIN.[RULE],EIMAIN.gro_amt,EIMAIN.inv_no as inv_no,EIACDET.inv_no as inv_no1,EIMAIN.BONDSRNO as u_b1no,EIMAIN.BOND_NO AS BOND_NO,
				EIMAIN.tot_examt as tot_examt,convert(varchar(254),EIMAIN.narr)as narr,EIMAIN.u_pinvno,EIMAIN.u_pinvdt,EIMAIN.ARENO AS ARE1NO,EIMAIN.AREDATE,
				tot_qty=(select sum(qty) from eiitem  where eiitem.tran_cd=eiacdet.tran_cd),space(50) as beno, space(50) as bedt
				from EIACDET 
				LEFT join EIMAIN on (EIACDET.TRAN_CD=EIMAIN.tran_cd)
				WHERE EIACDET.ac_id=EIMAIN.ac_id 'EXECUTE SP_EXECUTESQL @SQLCOMMAND--Added by Ajay Jaiswal on 18/08/2010 --- Start--ST--SET @SQLCOMMAND=' INSERT INTO  '+@TBLNAME1+' 
				select STACDET.entry_ty,STACDET.date,STACDET.ac_name,STACDET.amount,STACDET.amt_ty,STACDET.dept,STACDET.cate,
				STACDET.u_cldt,STMAIN.[RULE],STMAIN.gro_amt,STMAIN.inv_no as inv_no,STACDET.inv_no as inv_no1,STMAIN.BONDSRNO as u_b1no,STMAIN.BOND_NO AS BOND_NO,
				STMAIN.tot_examt as tot_examt,convert(varchar(254),STMAIN.narr)as narr,STMAIN.u_pinvno,STMAIN.u_pinvdt,STMAIN.ARENO AS ARE1NO,STMAIN.AREDATE AS AREDATE,
				tot_qty=(select sum(qty) from STitem  where STitem.tran_cd=STacdet.tran_cd),space(50) as beno, space(50) as bedt
				from STACDET 
				LEFT join STMAIN on (STACDET.TRAN_CD=STMAIN.tran_cd)
				WHERE stacdet.ac_name =''BALANCE WITH B17-BOND'' and stacdet.entry_ty=''ST'''--Added by Ajay Jaiswal on 18/08/2010 --- EndEXECUTE SP_EXECUTESQL @SQLCOMMANDSET @SQLCOMMAND=' SELECT * FROM '+@TBLNAME1+' where (u_b1no <> '''' or entry_ty = ''BB'') and date between'+char(39)+cast(@SDATE as varchar) +char(39)+' and'+CHAR(39)+CAST(@EDATE AS VARCHAR)+CHAR(39) SET @SQLCOMMAND=@SQLCOMMAND+' order by u_b1no,date'EXECUTE SP_EXECUTESQL @SQLCOMMANDSET @SQLCOMMAND=' DROP TABLE '+@TBLNAME1EXECUTE SP_EXECUTESQL @SQLCOMMAND
GO
