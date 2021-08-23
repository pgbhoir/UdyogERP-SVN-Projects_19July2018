If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_ImportSummary_MIS')
Begin
	Drop Procedure USP_REP_ImportSummary_MIS
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[USP_REP_ImportSummary_MIS]
	(@FrmDate SMALLDATETIME,@Todate SMALLDATETIME,@FParty nvarchar(100),@TParty nvarchar(100),@FITNM AS VARCHAR(100),@TITNM AS VARCHAR(100)) 	
	AS

Declare @SQLCOMMAND NVARCHAR(max),@TBLCON NVARCHAR(max),@SQLCOMMAND1 NVARCHAR(max),@ParmDefinition NVARCHAR(max),@SQLCOMMAND2 NVARCHAR(max)
Declare @chapno varchar(30),@eit_name  varchar(100),@mchapno varchar(250),@meit_name  varchar(250)
	
	
			Declare @pformula varchar(100),@progcond varchar(250),@progopamt numeric(17,2)
			Declare @Entry_ty Varchar(2),@Tran_cd Numeric,@Date smalldatetime,@Progtotal Numeric(19,2),@Inv_no Varchar(20),@Progcurrtotal Numeric(19,2)

	Select Entry_ty,Tran_cd=0 Into #PTMAIN from PTMAIN Where 1=0
	set @sqlcommand='Insert Into #PTMAIN Select PTMAIN.Entry_ty,PTMAIN.Tran_cd from PTMAIN Where PTMAIN.Entry_ty in (''PT'',''P1'')'
		print @sqlcommand
		execute sp_executesql @sqlcommand

		select @pformula=isnull(pformula,''),@progcond=isnull(progcond,''),@progopamt=isnull(progopamt,0)  from manufact

Declare @sta_dt datetime, @end_dt datetime 
set @SQLCOMMAND1='select top 1 @sta_dt=sta_dt,@end_dt=end_dt from vudyog..co_mast where dbname=db_name() and ( select top 1 ptmain.date  from ptmain inner join ptitem on (ptmain.tran_cd=ptitem.tran_cd) where ptMAIN.Entry_ty in(''PT'')) between sta_dt and end_dt '	
set @ParmDefinition =N' @sta_dt datetime Output, @end_dt datetime Output'
print @SQLCOMMAND1
EXECUTE sp_executesql @SQLCOMMAND1,@ParmDefinition,@sta_dt=@sta_dt Output, @end_dt=@end_dt Output


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
		set @stkl_qty= @stkl_qty +', '+'ptitem.'+@fld_nm

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
	set @stkl_qty=',ptitem.QTY'
End

Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50),@head_nm varchar(100)
Select case when att_file=1 then 'ptmain.'+RTRIM(FLD_NM) else 'ptitem.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds
From Lother Where e_code in('pt','p1') and DISP_MIS=1
union all
Select case when att_file=1 then 'ptmain.'+RTRIM(FLD_NM) else 'ptitem.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty in('pt','p1') and DISP_MIS=1
--Added by Divyang P on 18032020 for Bug-33349  Start
union all
Select case when att_file=1 then 'ptmain.'+RTRIM(pert_name) else 'ptitem.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm= (rtrim(head_nm)+'PER')  From DcMast
Where Entry_ty in('pt','p1') and DISP_MIS=1
--Added by Divyang P on 18032020 for Bug-33349  End

SET @QueryString ='SELECT ptmain.entry_ty,ptmain.tran_cd,ptitem.item as prod_nm,ptitem.party_nm as suppl_nm,ptmain.pinvno as bill_no
,ptmain.pinvdt as bill_dt,ptitem.qty*ptitem.rate as pur_cost,ptitem.gro_amt as cost_price,sum(COGSALL.allocamt) as led_amt'
--SET @QueryString =@QueryString+',ITADDLESS='+@ITAddLess+',ITEMTAX='+@ItemTaxable+',ITEMNTX='+@ItemNTX+',HEADERDISC='+@HeaderDisc+''
--SET @QueryString =@QueryString+',HEADERTAX='+@HeaderTAX+',HEADERNTX='+@HeaderNTX+''
set @Tot_flds =''

Declare addi_flds cursor for
Select flds,fld_nm,att_file,data_ty,head_nm from #tmpFlds
Open addi_flds
Fetch Next from addi_flds Into @fld,@fld_nm1,@att_file,@fld_type,@head_nm
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
			Set @Tot_flds=@Tot_flds+','+@fld +' as ['+ rtrim(@head_nm) +']'	
		end
	End
	Fetch Next from addi_flds Into @fld,@fld_nm1,@att_file,@fld_type,@head_nm
End
Close addi_flds 
Deallocate addi_flds 
declare @sql as nvarchar(max)
set @sql= cAST(REPLICATE(@Tot_flds,4000) as nvarchar(100))
SET @SQLCOMMAND=''
--SET @SQLCOMMAND = N''+@QueryString+''+N''+@Tot_flds+''+'  FROM PTMAIN ' 

SET @SQLCOMMAND= @QueryString+' into ##ptmain1 from ptmain INNER JOIN PTITEM  ON PTMAIN.TRAN_CD=PTITEM.TRAN_CD LEFT JOIN IT_MAST ON IT_MAST.IT_CODE=PTITEM.IT_CODE INNER JOIN AC_MAST ON PTMAIN.AC_ID=AC_MAST.AC_ID '
SET @SQLCOMMAND= @SQLCOMMAND+' INNER JOIN #PTMAIN ON (PTMAIN.TRAN_CD=#PTMAIN.TRAN_CD and PTMAIN.Entry_ty=#PTMAIN.entry_ty ) '
SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN AC_MAST AC ON (AC.AC_ID=PTMAIN.CONS_ID)'
SET @SQLCOMMAND =	@SQLCOMMAND+' LEFT JOIN SHIPTO S1 ON (PTMAIN.AC_ID=S1.AC_ID AND PTMAIN.SAC_ID=S1.SHIPTO_ID)'
SET @SQLCOMMAND =	@SQLCOMMAND+' LEFT JOIN SHIPTO S2 ON (PTMAIN.CONS_ID=S2.AC_ID AND PTMAIN.SCONS_ID=S2.SHIPTO_ID)'
SET @SQLCOMMAND =	@SQLCOMMAND+' inner join COGSALL on COGSALL.entry_all = PTITEM.entry_ty and COGSALL.Main_tran=PTITEM.Tran_cd and COGSALL.itseri_all=PTITEM.itserial'
SET @SQLCOMMAND =	@SQLCOMMAND+' WHERE PTMAIN.entry_ty in(''PT'',''P1'') and (PTMAIN.date BETWEEN '+CHAR(39)+cast(@FrmDate as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@Todate as varchar )+CHAR(39)+') and (PTMAIN.Party_nm BETWEEN '+CHAR(39)+@FParty+CHAR(39)+' AND '+CHAR(39)+@TParty+CHAR(39)+')'
SET @SQLCOMMAND =	@SQLCOMMAND+'  and (ptitem.item BETWEEN '+CHAR(39)+@FITnm+CHAR(39)+' AND '+CHAR(39)+@TITnm+CHAR(39)+')'
SET @SQLCOMMAND =	@SQLCOMMAND+' group by ptmain.entry_ty,ptmain.tran_cd,ptitem.item,ptitem.party_nm,ptmain.pinvno,ptmain.pinvdt,ptitem.qty,ptitem.rate,ptitem.gro_amt'
execute sp_executesql @sqlcommand
print @sqlcommand
set @SQLCOMMAND=''
set @SQLCOMMAND=' SELECT a.entry_ty,a.prod_nm,a.suppl_nm,a.bill_no,a.bill_dt,a.pur_cost,a.cost_price,a.led_amt'
SET @SQLCOMMAND = N''+@SQLCOMMAND+''+N''+@Tot_flds+''+'   from ##ptmain1 a  ' 
SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN PTITEM ON a.TRAN_CD=PTITEM.TRAN_CD and a.entry_ty=PTITEM.entry_ty'
execute sp_executesql @sqlcommand

drop table ##ptmain1
--set dateformat dmy
--execute USP_REP_ImportDetail_MIS '01/06/2020','30/06/2020','','pt test','',''






