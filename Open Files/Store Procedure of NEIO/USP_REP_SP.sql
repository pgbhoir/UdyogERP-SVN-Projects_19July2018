If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_SP')
Begin
	Drop Procedure USP_REP_SP
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nilesh Yadav on dated 26/02/2015 for Bug 25365
-- EXECUTE USP_REP_SP "main.Entry_ty = 'SP' And main.Tran_cd = 0"
-- =============================================

Create PROCEDURE  [dbo].[USP_REP_SP]
@ENTRYCOND NVARCHAR(254)
AS
Declare @SQLCOMMAND as NVARCHAR(4000),@TBLCON as NVARCHAR(4000)
SET @TBLCON=RTRIM(@ENTRYCOND)

	DECLARE @ENT VARCHAR(2),@TRN INT,@POS1 INT,@POS2 INT,@POS3 INT
		
		PRINT @ENTRYCOND
		SET @POS1=CHARINDEX('''',@ENTRYCOND,1)+1
		SET @ENT= SUBSTRING(@ENTRYCOND,@POS1,2)
		SET @POS2=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS1))+1
		SET @POS3=CHARINDEX('=',@ENTRYCOND,CHARINDEX('''',@ENTRYCOND,@POS2))+1
		SET @TRN= SUBSTRING(@ENTRYCOND,@POS2,@POS2-@POS3)
		SET @TBLCON=RTRIM(@ENTRYCOND)

Select Entry_ty,Tran_cd=0,Date,inv_no Into #main from main Where 1=0
Create Clustered Index Idx_tmpmain On #main (Entry_ty asc, Tran_cd Asc)

set @sqlcommand='Insert Into #main Select main.Entry_ty,main.Tran_cd,main.date,main.inv_no
from main 
Where '+@TBLCON
execute sp_executesql @sqlcommand

--Added By Divyang P for Bug-33349 on 08042020 Start
Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50)
Select case when att_file=1 then (CASE WHEN RTRIM(TBL_NM)='STMAINADD' THEN 'MA.' ELSE 'M.' END )+RTRIM(FLD_NM) else 'I.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='SP' 
union all
Select case when att_file=1 then 'M.'+RTRIM(FLD_NM) else 'I.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='SP' 
union all
Select case when att_file=1 then 'M.'+RTRIM(pert_name) else 'I.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='SP' and pert_name<>''


set @QueryString='select m.inv_no,m.date,I.item_no,I.item,I.SP_partynm,I.sp_segment,I.qty,it.rateunit,I.rate,I.gro_amt,m.salesman,'
SET @QueryString=@QueryString+' '+ 'ac.ac_name,ac.add1,ac.add2,ac.add3,ac.city,ac.mailname '


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
SET @SQLCOMMAND = N''+@QueryString++''+N''+@Tot_flds+''+'  FROM main M ' 
print @SQLCOMMAND
set @sqlcommand= @SQLCOMMAND +' 
inner join item I on (M.entry_ty=I.entry_ty and M.tran_cd=I.tran_cd)
INNER JOIN #main ON (I.TRAN_CD=#main.TRAN_CD and I.Entry_ty=#main.entry_ty)
inner join ac_mast ac on (ac.ac_id=m.ac_id)
INNER JOIN IT_MAST it ON (I.IT_CODE=it.IT_CODE)
 WHERE M.ENTRY_TY='''+@ENT+''' and M.TRAN_CD='+cast(@TRN as nvarchar(1000))+'    '
 print @SQLCOMMAND
 execute sp_executesql @sqlcommand
--Added By Divyang P for Bug-33349 on 08042020 End
	



--Commenetd By Divyang P for Bug-33349 on 08042020 Start
--set @SqlCommand='select m.inv_no,m.date,s.item_no,s.item,s.SP_partynm,s.sp_segment,s.qty,it.rateunit,s.rate,s.gro_amt,m.salesman,'
--SET @SQLCOMMAND=@SQLCOMMAND+' '+ 'ac.ac_name,ac.add1,ac.add2,ac.add3,ac.city,ac.mailname'
--SET @SQLCOMMAND=@SQLCOMMAND+' '+'from main m'
--SET @SQLCOMMAND=@SQLCOMMAND+' '+'inner join item s on (m.entry_ty=s.entry_ty and m.tran_cd=s.tran_cd)'
--set @sqlcommand=@sqlcommand+' '+'INNER JOIN #main ON (s.TRAN_CD=#main.TRAN_CD and s.Entry_ty=#main.entry_ty)'
--SET @SQLCOMMAND=@SQLCOMMAND+' '+'inner join ac_mast ac on (ac.ac_id=m.ac_id)'
--SET @SQLCOMMAND=@SQLCOMMAND+' '+'INNER JOIN IT_MAST it ON (s.IT_CODE=it.IT_CODE)'
--SET @SQLCOMMAND=@SQLCOMMAND+' '+'where m.entry_ty=''sp'''
--EXECUTE SP_EXECUTESQL @SQLCOMMAND
--PRINT @SQLCOMMAND
--Commenetd By Divyang P for Bug-33349 on 08042020 End



