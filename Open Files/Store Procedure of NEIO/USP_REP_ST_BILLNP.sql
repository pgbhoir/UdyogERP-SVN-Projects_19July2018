If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_ST_BILLNP')
Begin
	Drop Procedure USP_REP_ST_BILLNP
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		
-- Create date: 
-- Description:	This Stored procedure is useful to generate Sales Nepal Invoice 
-- Modification Date/By/Reason: 28/09/2011 Shrikant S. for TKT-9486
-- Modification Date/By/Reason: 10/01/2012 Shrikant S. for Bug-1460(Multi UOM qty fields are require in default)
-- Remark:
-- =============================================
Create PROCEDURE [dbo].[USP_REP_ST_BILLNP]
	@ENTRYCOND NVARCHAR(254)
	AS
Declare @SQLCOMMAND as NVARCHAR(4000),@TBLCON as NVARCHAR(4000)
Declare @chapno varchar(30),@eit_name  varchar(100),@mchapno varchar(250),@meit_name  varchar(250)
	
--	--->Entry_Ty and Tran_Cd Separation --Commented by Shrikant S. on 22/04/10 for TKT-6
--		declare @ent varchar(2),@trn int,@pos1 int,@pos2 int,@pos3 int--,@ENTRYCOND NVARCHAR(254)
--		
--		print @ENTRYCOND
--		set @pos1=charindex('''',@ENTRYCOND,1)+1
--		set @ent= substring(@ENTRYCOND,@pos1,2)
--		set @pos2=charindex('=',@ENTRYCOND,charindex('''',@ENTRYCOND,@pos1))+1
--		set @pos3=charindex('=',@ENTRYCOND,charindex('''',@ENTRYCOND,@pos2))+1
--		set @trn= substring(@ENTRYCOND,@pos2,@pos2-@pos3)
--		--select * from bpmain where entry_ty=@ent and tran_cd=@trn
--	---<---Entry_Ty and Tran_Cd Separation
	SET @TBLCON=RTRIM(@ENTRYCOND)
	
	
		Select Entry_ty,Tran_cd=0,Date,inv_no,itserial=space(6) Into #stmain from stmain Where 1=0
		Create Clustered Index Idx_tmpStmain On #stmain (Entry_ty asc, Tran_cd Asc, Itserial asc)

		set @SQLCOMMAND='Insert Into #stmain Select stmain.Entry_ty,stmain.Tran_cd,stmain.date,stmain.inv_no,stitem.itserial from stmain Inner Join stitem on (stmain.Entry_ty=stitem.Entry_ty and stmain.Tran_cd=stitem.Tran_cd) Where '+@TBLCON
		PRINT @SQLCOMMAND
		execute sp_executesql @SQLCOMMAND


-- Added By Shrikant S. on 10/01/2012 for Bug-1460		--Start
Declare @uom_desc as Varchar(100),@len int,@fld_nm varchar(10),@fld_desc Varchar(10),@count int,@stkl_qty Varchar(100)

select @uom_desc=isnull(uom_desc,'') from vudyog..co_mast where dbname =rtrim(db_name())
Create Table #qty_desc (fld_nm varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS, fld_desc varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS)
set @len=len(@uom_desc)
set @stkl_qty=''
If @len>0 
Begin
	while @len>0
	Begin
		set @fld_nm=substring(@uom_desc,1,charindex(':',@uom_desc)-1)
		set @uom_desc=substring(@uom_desc,charindex(':',@uom_desc)+1,@len)
		--set @stkl_qty= @stkl_qty +', '+'STITEM.'+@fld_nm	--Commented By Divyang P for Bug-33349 on 08042020
		set @stkl_qty= @stkl_qty +', '+'I.'+@fld_nm			--mODIFIED By Divyang P for Bug-33349 on 08042020

		if @len>0 and charindex(';',@uom_desc)=0
		begin
			set @uom_desc=@uom_desc
			set @fld_desc=@uom_desc
			SET @len=0
		End
		else
		begin
				set @fld_desc=substring(@uom_desc,1,charindex(';',@uom_desc)-1)
				set @uom_desc=substring(@uom_desc,charindex(';',@uom_desc)+1,@len)
				set @len=len(@uom_desc)
		End
		insert into #qty_desc values (@fld_nm,@fld_desc)
	End
End
Else
Begin
	--set @stkl_qty=',STITEM.QTY'	--Commented By Divyang P for Bug-33349 on 08042020
	set @stkl_qty=',I.QTY'			--mODIFIED By Divyang P for Bug-33349 on 08042020
End

--Added By Divyang P for Bug-33349 on 08042020 Start
Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50)
Select case when att_file=1 then (CASE WHEN RTRIM(TBL_NM)='STMAINADD' THEN 'MA.' ELSE 'M.' END )+RTRIM(FLD_NM) else 'I.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='EI' 
union all
Select case when att_file=1 then 'M.'+RTRIM(FLD_NM) else 'I.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='EI' 
union all
Select case when att_file=1 then 'M.'+RTRIM(pert_name) else 'I.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='EI' and pert_name<>''
--Added By Divyang P for Bug-33349 on 08042020 End


set @QueryString='	SELECT ''REPORT HEADER'' AS REP_HEAD,M.INV_SR,M.TRAN_CD,M.ENTRY_TY,M.INV_NO,M.DATE'
set @QueryString=@QueryString+' '+',M.U_TIMEP,M.U_TIMEP1 ,M.U_REMOVDT'
--set @sqlcommand=@sqlcommand+' '+',I.EXAMT,I.U_BASDUTY,I.U_CESSPER,I.U_CESSAMT,I.U_HCESSPER,I.U_HCESAMT'
set @QueryString=@QueryString+' '+',M.U_DELIVER,M.DUE_DT,M.U_CLDT,U_CHALNO=SPACE(1),U_CHALDT=M.DATE,M.U_PONO,M.U_PODT,M.U_LRNO,M.U_LRDT,M.U_DELI,M.U_VEHNO,M.GRO_AMT GRO_AMT1,M.TAX_NAME,M.TAXAMT,M.NET_AMT'
set @QueryString=@QueryString+' '+',I.U_PKNO'+@stkl_qty+',I.RATE,I.U_ASSEAMT,I.U_MRPRATE,I.U_EXPDESC,I.U_EXPMARK,I.U_EXPGWT '
set @QueryString=@QueryString+' '+',IT_MAST.IT_NAME ,IT_MAST.EIT_NAME,IT_MAST.CHAPNO,IT_MAST.IDMARK,IT_MAST.RATEUNIT '
set @sqlcommand=@QueryString+' '+',AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,AC_MAST.I_TAX'
set @QueryString=@QueryString+' '+',AC_MAST.GSTIN ,AC_MAST1.ADD1 ADD11,AC_MAST1.ADD2 ADD22,AC_MAST1.ADD3 ADD33,AC_MAST1.CITY CITY1'
set @QueryString=@QueryString+' '+',AC_MAST1.ZIP ZIP1,AC_MAST1.I_TAX I_TAX1,AC_MAST1.GSTIN GSTIN1,I.ITSERIAL'
set @QueryString=@QueryString+' '+',It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END)'
set @QueryString=@QueryString+' '+',MailName=(CASE WHEN ISNULL(ac_mast.MailName,'''')='''' THEN ac_mast.ac_name ELSE ac_mast.mailname END)'

--Added By Divyang P for Bug-33349 on 08042020 Start
set @Tot_flds =''

Declare addi_flds cursor for
Select flds,fld_nm,att_file,data_ty from #tmpFlds
Open addi_flds
Fetch Next from addi_flds Into @fld,@fld_nm1,@att_file,@fld_type
While @@Fetch_Status=0
Begin
	if  charindex(@fld,@QueryString)=0
	begin
		if  charindex(@fld_type,'text')<>0
			begin
			 Set @Tot_flds=@Tot_flds+','+'CONVERT(VARCHAR(500),'+@fld+') AS '+substring(@fld,charindex('.',@fld)+1,len(@fld))  
			end
		else
		begin
			Set @Tot_flds=@Tot_flds+','+@fld   
		end
	End
	Fetch Next from addi_flds Into @fld,@fld_nm1,@att_file,@fld_type 
End
Close addi_flds 
Deallocate addi_flds 
declare @sql as nvarchar(max)
set @sql= cAST(REPLICATE(@Tot_flds,4000) as nvarchar(100))

SET @SQLCOMMAND=''
SET @SQLCOMMAND = N''+@QueryString++''+N''+@Tot_flds+''+'  FROM STMAIN M' 
print @SQLCOMMAND
--Added By Divyang P for Bug-33349 on 08042020 END

set @sqlcommand=@sqlcommand+' '+ 'INNER JOIN STITEM I ON (m.TRAN_CD=i.TRAN_CD) '
set @sqlcommand=@sqlcommand+' '+ 'INNER JOIN STMAINADD MA ON (M.TRAN_CD=MA.TRAN_CD) '
set @sqlcommand=@sqlcommand+' '+'INNER JOIN #stmain ON (I.TRAN_CD=#stmain.TRAN_CD and I.Entry_ty=#stmain.entry_ty and I.ITSERIAL=#stmain.itserial) '
set @sqlcommand=@sqlcommand+' '+'INNER JOIN IT_MAST ON (I.IT_CODE=IT_MAST.IT_CODE) '
set @sqlcommand=@sqlcommand+' '+'INNER JOIN AC_MAST ON (AC_MAST.AC_ID=M.AC_ID) '
set @sqlcommand=@sqlcommand+' '+'LEFT JOIN AC_MAST AC_MAST1 ON (AC_MAST1.AC_NAME=M.U_DELIVER) '
set @sqlcommand=@sqlcommand+' '+'ORDER BY M.INV_SR,M.INV_NO'
PRINT @sqlcommand
execute sp_executesql @sqlcommand

-- Added By Shrikant S. on 10/01/2012 for Bug-1460		--End

/*		Commented By Shrikant S. on 10/01/2012 for Bug-1460		--Start
	SELECT 'REPORT HEADER' AS REP_HEAD,STMAIN.INV_SR,STMAIN.TRAN_CD,STMAIN.ENTRY_TY,STMAIN.INV_NO,STMAIN.DATE
	,STMAIN.U_TIMEP,STMAIN.U_TIMEP1 ,STMAIN.U_REMOVDT,STMAIN.U_EXPLA,STMAIN.U_EXRG23II,STMAIN.U_RG2AMT
	,STITEM.EXAMT,STITEM.U_BASDUTY,STITEM.U_CESSPER,STITEM.U_CESSAMT,STITEM.U_HCESSPER,STITEM.U_HCESAMT
	,STMAIN.U_DELIVER,STMAIN.DUE_DT,STMAIN.U_CLDT,U_CHALNO=SPACE(1),U_CHALDT=STMAIN.DATE,STMAIN.U_PONO,STMAIN.U_PODT,STMAIN.U_LRNO,STMAIN.U_LRDT,STMAIN.U_DELI,STMAIN.U_VEHNO,STMAIN.GRO_AMT GRO_AMT1,STMAIN.TAX_NAME,STMAIN.TAXAMT,STMAIN.NET_AMT,STMAIN.U_PLASR,STMAIN.U_RG23NO,STMAIN.U_RG23CNO
	,STITEM.U_PKNO,STITEM.QTY,STITEM.RATE,STITEM.U_ASSEAMT,STITEM.U_MRPRATE,STITEM.U_EXPDESC,STITEM.U_EXPMARK,STITEM.U_EXPGWT 
	,IT_MAST.IT_NAME 
	,IT_MAST.EIT_NAME
	,IT_MAST.CHAPNO
	,IT_MAST.IDMARK,IT_MAST.RATEUNIT 
	,AC_MAST.AC_NAME,AC_MAST.ADD1,AC_MAST.ADD2,AC_MAST.ADD3,AC_MAST.CITY,AC_MAST.ZIP,AC_MAST.S_TAX,AC_MAST.I_TAX
	,AC_MAST.ECCNO ,AC_MAST1.ADD1 ADD11,AC_MAST1.ADD2 ADD22,AC_MAST1.ADD3 ADD33,AC_MAST1.CITY CITY1
	,AC_MAST1.ZIP ZIP1,AC_MAST1.S_TAX S_TAX1,AC_MAST1.I_TAX I_TAX1,AC_MAST1.ECCNO ECCNO1,STITEM.ITSERIAL
	,It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'')='' THEN it_mast.it_name ELSE it_mast.it_alias END)
	,MailName=(CASE WHEN ISNULL(ac_mast.MailName,'')='' THEN ac_mast.ac_name ELSE ac_mast.mailname END)
	into #stmain1
	FROM STMAIN  INNER JOIN STITEM ON (STMAIN.TRAN_CD=STITEM.TRAN_CD) 
	INNER JOIN #stmain ON (STITEM.TRAN_CD=#stmain.TRAN_CD and STITEM.Entry_ty=#stmain.entry_ty and STITEM.ITSERIAL=#stmain.itserial) --Added by Shrikant S. on 22/04/2010 for TKT-6 
	INNER JOIN IT_MAST ON (STITEM.IT_CODE=IT_MAST.IT_CODE) 
	INNER JOIN AC_MAST ON (AC_MAST.AC_ID=STMAIN.AC_ID) 
	LEFT JOIN AC_MAST AC_MAST1 ON (AC_MAST1.AC_NAME=STMAIN.U_DELIVER) 
	--WHERE  STMAIN.ENTRY_TY= @ent  and stmain.tran_cd=@trn
	ORDER BY STMAIN.INV_SR,STMAIN.INV_NO					-- Added By Shrikant S. on 28/09/2011 for TKT-9486  
	--ORDER BY STMAIN.INV_SR,CAST(STMAIN.INV_NO  AS INT)	-- Commented By Shrikant S. on 28/09/2011 for TKT-9486

	set @mchapno=' '
	set @meit_name=' '

	Select * from #stmain1
Commented By Shrikant S. on 10/01/2012 for Bug-1460		--End			*/


/*	
	declare cur_stbill cursor for select distinct CHAPNO from #stmain
	open cur_stbill 
	fetch next from cur_stbill into @chapno
	while(@@fetch_status=0)
	begin
		set @mchapno=rtrim(@mchapno)+','+rtrim(@chapno)
		fetch next from cur_stbill into @chapno
	end
	close cur_stbill
	deallocate cur_stbill
	
	declare cur_stbill cursor for select distinct eit_name from #stmain
	open cur_stbill 
	fetch next from cur_stbill into @eit_name
	while(@@fetch_status=0)
	begin
		set @meit_name=rtrim(@meit_name)+','+rtrim(@eit_name)
		fetch next from cur_stbill into @eit_name
	end
	close cur_stbill
	deallocate cur_stbill	
	
	set @mChapno=case when len(@mChapno)>1 then substring(@mChapno,2,len(@mChapno)-1) else '' end
	set @mEIT_NAME=case when len(@mEIT_NAME)>1 then substring(@mEIT_NAME,2,len(@mEIT_NAME)-1) else '' end
	SELECT * 
	,mChapno=ISNULL(@mChapno,'')
	,mEIT_NAME=ISNULL(@mEIT_NAME,'')
	FROM #STMAIN

*/
