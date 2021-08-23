If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Rep_MIS_MREV5')
Begin
	Drop Procedure Usp_Rep_MIS_MREV5
End
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--Created By Prajakta B. on 14062019
Create PROCEDURE [dbo].[Usp_Rep_MIS_MREV5] 
	(@FrmDate SMALLDATETIME,@Todate SMALLDATETIME,@FParty nvarchar(100),@TParty nvarchar(100),@FSeries nvarchar(100),@TSeries nvarchar(100),@FDept nvarchar(100),@TDept nvarchar(100),@FCategory nvarchar(100),@TCategory nvarchar(100),@FWarehouse nvarchar(100),@TWarehouse nvarchar(100)) 	
	AS
Declare @SQLCOMMAND NVARCHAR(max),@TBLCON NVARCHAR(max),@QueryString NVARCHAR(max),
		@ParmDefinition NVARCHAR(max)
Declare @chapno varchar(30),@eit_name  varchar(100),
		@mchapno varchar(250),@meit_name  varchar(250)
Declare @pformula varchar(100),@progcond varchar(250),@progopamt numeric(17,2)
Declare @Entry_ty Varchar(2),@Tran_cd Numeric,@Date smalldatetime,@Progtotal Numeric(19,2),
		@Inv_no Varchar(20),@Progcurrtotal Numeric(19,2)
select @pformula=isnull(pformula,''),@progcond=isnull(progcond,''),@progopamt=isnull(progopamt,0)  from manufact

--Added by Divyang P on 18032020 for Bug-33349  Start
Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@fld_type  varchar(15),@tempsql varchar(50),@head_nm varchar(100)
Select case when att_file=1 then 'irmain.'+RTRIM(FLD_NM) else 'iritem.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='RL' and DISP_MIS=1
union all
Select case when att_file=1 then 'irmain.'+RTRIM(FLD_NM) else 'iritem.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='RL' and DISP_MIS=1
union all
Select case when att_file=1 then 'irmain.'+RTRIM(pert_name) else 'iritem.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm= (rtrim(head_nm)+'PER')  From DcMast
Where Entry_ty='RL' and DISP_MIS=1
--Added by Divyang P on 18032020 for Bug-33349  End


	SET @QueryString = ''
	SET @QueryString = 'SELECT ''REPORT HEADER'' AS REP_HEAD,irmain.TRAN_CD,irmain.ENTRY_TY,irmain.INV_NO,irmain.DATE'
	SET @QueryString =@QueryString+',irmain.prodtype,irmain.DUE_DT,irmain.U_LRNO,irmain.U_LRDT,irmain.U_DELI,irmain.U_VEHNO,iritem.GRO_AMT AS IT_GROAMT,irmain.GRO_AMT GRO_AMT1,irmain.TAX_NAME,iritem.TAX_NAME AS IT_TAXNAME,irmain.TAXAMT,iritem.TAXAMT AS IT_TAXAMT,irmain.NET_AMT,iritem.RATE,iritem.U_ASSEAMT, cast (iritem.NARR AS VARCHAR(2000)) AS NARR,iritem.TOT_EXAMT'
	SET @QueryString =@QueryString+',IT_MAST.IT_NAME,IT_MAST.EIT_NAME,IT_MAST.CHAPNO,IT_MAST.RATEUNIT
	,HSNCODE = (CASE WHEN ISSERVICE=1 THEN IT_MAST.SERVTCODE ELSE IT_MAST.HSNCODE END) ,IT_mast.U_ITPARTCD,iritem.item_no'
	SET @QueryString =@QueryString+',It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END)'
	SET @QueryString =@QueryString+',ST_TYPE=CASE WHEN irmain.SCONS_ID >0 THEN S2.ST_TYPE ELSE AC.ST_TYPE END'			
	SET @QueryString =@QueryString+',iritem.ITSERIAL,item_fdisc=iritem.tot_fdisc,iritem.qty'	
	SET @QueryString =@QueryString+',iritem.CGST_PER,iritem.CGST_AMT,iritem.SGST_PER,iritem.SGST_AMT,iritem.IGST_PER,iritem.IGST_AMT'
	SET @QueryString =@QueryString+',iritem.Compcess,iritem.CCESSRATE'
	SET @QueryString =@QueryString+',irmain.Party_nm,ac_mast.GSTIN,ac_mast.State,iritem.item,iritem.stkunit,Total=iritem.rate*iritem.qty'
	SET @QueryString =@QueryString+',TotalGST=iritem.CGST_AMT+iritem.SGST_AMT+iritem.IGST_AMT,irmain.EWBN,irmain.EWBDT,irmain.EWBDIST'
	SET @QueryString =@QueryString+',irmain.u_deli as transname,irmain.EWBVTD,iritem.u_rule,iritem.pinvno,iritem.pinvdt'

--Added by Divyang P on 18032020 for Bug-33349  Start
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
				print @fld
				--Set @Tot_flds=@Tot_flds+','+@fld +' as '+ rtrim(@head_nm)
				Set @Tot_flds=@Tot_flds+','+@fld +' as ['+ rtrim(@head_nm) +']'	
			end
		End
		Fetch Next from addi_flds Into @fld,@fld_nm1,@att_file,@fld_type,@head_nm
	End
	Close addi_flds 
	Deallocate addi_flds 
	declare @sql as nvarchar(max)
	set @sql= cAST(REPLICATE(@Tot_flds,4000) as nvarchar(100))
	print @Tot_flds

	--Added by Divyang P on 18032020 for Bug-33349  END



SET @SQLCOMMAND=''
--SET @SQLCOMMAND = N''+@QueryString+''+N''+' into '+'##main11'+' FROM irmain'	--Commented by Divyang P on 18032020 for Bug-33349  END
SET @SQLCOMMAND = N''+@QueryString+''+N''+@Tot_flds+''+' into '+'##main11'+' FROM irmain'	--Added by Divyang P on 18032020 for Bug-33349  END
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN iritem ON (irmain.TRAN_CD=iritem.TRAN_CD AND irmain.ENTRY_TY=iritem.ENTRY_TY)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN IT_MAST ON (iritem.IT_CODE=IT_MAST.IT_CODE)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN AC_MAST ON (AC_MAST.AC_ID=irmain.AC_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN AC_MAST AC ON (AC.AC_ID=irmain.CONS_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' LEFT JOIN SHIPTO S1 ON (irmain.AC_ID=S1.AC_ID AND irmain.SAC_ID=S1.SHIPTO_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' LEFT JOIN SHIPTO S2 ON (irmain.CONS_ID=S2.AC_ID AND irmain.SCONS_ID=S2.SHIPTO_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+'WHERE irmain.entry_ty=''RL'' and (irmain.date BETWEEN '+CHAR(39)+cast(@FrmDate as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@Todate as varchar )+CHAR(39)+') and (irmain.Party_nm BETWEEN '+CHAR(39)+@FParty+CHAR(39)+' AND '+CHAR(39)+@TParty+CHAR(39)+')'
 SET @SQLCOMMAND =	@SQLCOMMAND+'and (irmain.inv_sr BETWEEN '+CHAR(39)+cast(@FSeries as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TSeries as varchar )+CHAR(39)+') and (irmain.Dept BETWEEN '+CHAR(39)+@FDept+CHAR(39)+' AND '+CHAR(39)+@TDept+CHAR(39)+')'
 SET @SQLCOMMAND =	@SQLCOMMAND+'and (irmain.cate BETWEEN '+CHAR(39)+cast(@FCategory as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TCategory as varchar )+CHAR(39)+') and (iritem.ware_nm BETWEEN '+CHAR(39)+@FWarehouse+CHAR(39)+' AND '+CHAR(39)+@TWarehouse+CHAR(39)+')'
 SET @SQLCOMMAND =	@SQLCOMMAND+' ORDER BY irmain.INV_SR,irmain.INV_NO'
 print @sqlcommand
execute sp_executesql @SQLCOMMAND

SET @SQLCOMMAND ='select a.Date,a.inv_no,a.Party_nm,a.GSTIN,a.State,a.st_type,a.item,a.HSNCODE,a.qty,a.stkunit,a.rate,a.Total,a.u_asseamt,a.CGST_PER,a.CGST_AMT,a.SGST_PER,a.SGST_AMT,a.IGST_PER,a.IGST_AMT,a.TotalGST,a.EWBN,a.EWBDT,a.EWBDIST,a.IT_GROAMT,a.net_amt,a.transname,a.u_vehno,a.U_LRDT,a.u_lrno,a.EWBVTD,CCESSRATE=case when a.CCESSRATE=''NO-CESS'' then '''' else a.CCESSRATE end,a.COMPCESS
					,a.prodtype,a.u_rule,a.pinvno,a.pinvdt '
SET @SQLCOMMAND = @SQLCOMMAND + N''+@Tot_flds+''+ ' from ##main11 a inner join irmain on (irmain.tran_cd=a.tran_cd) inner join iritem on (iritem.tran_cd=irmain.tran_cd) '
execute sp_executesql @SQLCOMMAND
drop table ##main11

--exec sp_executesql N'execute Usp_Rep_MIS_MREV5 @FDate,@TDate,@FParty,@TParty',N'@FParty nvarchar(4000),@TParty nvarchar(10),@FDate nvarchar(10),@TDate nvarchar(10)',@FParty=N'',@TParty=N'WORK ORDER',@FDate=N'04/01/2019',@TDate=N'03/31/2020'
--go





