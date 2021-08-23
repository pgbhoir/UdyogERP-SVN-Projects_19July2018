If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Rep_MIS_MSALERPI')
Begin
	Drop Procedure Usp_Rep_MIS_MSALERPI
End
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--Created By Prajakta B. on 14062019
Create PROCEDURE [dbo].[Usp_Rep_MIS_MSALERPI] 
	(@FrmDate SMALLDATETIME,@Todate SMALLDATETIME,@FParty nvarchar(100),@TParty nvarchar(100),@FSeries nvarchar(100),@TSeries nvarchar(100),@FDept nvarchar(100),@TDept nvarchar(100),@FCategory nvarchar(100),@TCategory nvarchar(100),@FWarehouse nvarchar(100),@TWarehouse nvarchar(100)) 	
	AS
Declare @SQLCOMMAND NVARCHAR(max),@TBLCON NVARCHAR(max),@SQLCOMMAND1 NVARCHAR(max),
		@ParmDefinition NVARCHAR(max),@SQLCOMMAND2 NVARCHAR(max)
Declare @chapno varchar(30),@eit_name  varchar(100),
		@mchapno varchar(250),@meit_name  varchar(250)
Declare @pformula varchar(100),@progcond varchar(250),@progopamt numeric(17,2)
Declare @Entry_ty Varchar(2),@Tran_cd Numeric,@Date smalldatetime,@Progtotal Numeric(19,2),
		@Inv_no Varchar(20),@Progcurrtotal Numeric(19,2)
select @pformula=isnull(pformula,''),@progcond=isnull(progcond,''),@progopamt=isnull(progopamt,0)  from manufact
declare @ITAddLess varchar(500),@NonTaxIT varchar(500),@HeaderDisc varchar(500),@HeaderTAX nvarchar(max),@HeaderNTX nvarchar(max),@ItemTaxable nvarchar(max),@ItemNTX nvarchar(max),@ItemAddl nvarchar(max)
--itemwise 
SELECT 
    @ITAddLess = isnull(STUFF(
                 (SELECT Case when a_s<>'' then a_s else (case when code in( 'D','F') THEN '-' else '+' end ) end + fld_nm FROM Dcmast where att_file=0 and Entry_ty='pi' and code in( 'D','F')  FOR XML PATH ('')), 1, 0, ''
               ),0)               
--itemwise taxable and non taxable
SELECT 
    @ItemTaxable = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code='T' and att_file=0 and Entry_ty='pi'  FOR XML PATH ('')), 1, 0, ''
                 --(select case when fld_nm<>'' then '+'+fld_nm else ''  end from dcmast where code='T' and att_file=0 and entry_ty='ST' and bef_aft=2  FOR XML PATH ('')), 1, 0, ''
               ),0)
SELECT 
    @ItemNTX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=0 and Entry_ty='pi' and fld_nm not in ('Compcess') /*and bef_aft=2*/  FOR XML PATH ('')), 1, 0, ''
               ),0)   
             
--itemwise non tax

--headerwise disc
SELECT 
    @HeaderDisc = isnull(STUFF(
                 (SELECT  '-'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('D','F') and att_file=1  and Entry_ty='pi'  FOR XML PATH ('')), 1, 0, ''
               ),0)

--headerwise taxable chargs
SELECT 
    @HeaderTAX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code='T' and att_file=1 and Entry_ty='pi'  FOR XML PATH ('')), 1, 0, ''
               ),0)

--headerwise non taxable chargs
SELECT 
    @HeaderNTX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=1 and Entry_ty='pi'  FOR XML PATH ('')), 1, 0, ''
               ),0)


Declare @Addtblnm Varchar(1000)
set @Addtblnm  =''
if Exists(Select Top 1 [Name] From SysObjects Where xType='U' and [Name]='pimainAdd')
Begin
	set @Addtblnm =' LEFT JOIN pimainADD ON (pimain.TRAN_CD=pimainADD.TRAN_CD) '
end	
		
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
		set @stkl_qty= @stkl_qty +', '+'piitem.'+@fld_nm

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
	set @stkl_qty=',piitem.QTY'
End

Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds nVarchar(max),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50),@head_nm varchar(100)
Select case when att_file=1 then (CASE WHEN RTRIM(TBL_NM)='pimainADD' THEN 'pimainADD.' ELSE 'pimain.' END )+RTRIM(FLD_NM) else 'piitem.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm
Into #tmpFlds 
From Lother Where e_code='pi' and DISP_MIS=1
union all
Select case when att_file=1 then 'pimain.'+RTRIM(FLD_NM) else 'piitem.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='pi'	and DISP_MIS=1
--Added by Divyang P on 17032020 for Bug-33349  Start
union all
Select case when att_file=1 then 'pimain.'+RTRIM(pert_name) else 'piitem.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm= (rtrim(head_nm)+'PER')  From DcMast
Where Entry_ty='IP' and DISP_MIS=1
--Added by Divyang P on 17032020 for Bug-33349  End


	SET @QueryString = ''
	SET @QueryString = 'SELECT ''REPORT HEADER'' AS REP_HEAD,pimain.INV_SR,pimain.TRAN_CD,pimain.ENTRY_TY,pimain.INV_NO,pimain.DATE ,pimain.U_REMOVDT'
	SET @QueryString =@QueryString+',pimain.U_DELIVER,pimain.DUE_DT,pimain.U_CLDT,pimain.U_CHALNO,pimain.U_CHALDT,pimain.U_PONO,pimain.U_PODT,pimain.U_LRNO,pimain.U_LRDT,pimain.U_DELI,pimain.U_VEHNO,piitem.GRO_AMT AS IT_GROAMT,pimain.GRO_AMT GRO_AMT1,pimain.TAX_NAME,piitem.TAX_NAME AS IT_TAXNAME,pimain.TAXAMT,piitem.TAXAMT AS IT_TAXAMT,pimain.NET_AMT,piitem.U_PKNO'+@stkl_qty+',piitem.RATE,piitem.U_ASSEAMT,piitem.U_MRPRATE,piitem.U_EXPDESC,piitem.U_APPACK, cast (piitem.NARR AS VARCHAR(2000)) AS NARR,piitem.TOT_EXAMT'
	SET @QueryString =@QueryString+',IT_MAST.IT_NAME,IT_MAST.EIT_NAME,IT_MAST.CHAPNO,IT_MAST.IDMARK,IT_MAST.RATEUNIT
	,HSNCODE = (CASE WHEN ISSERVICE=1 THEN IT_MAST.SERVTCODE ELSE IT_MAST.HSNCODE END)  
	,IT_mast.U_ITPARTCD,piitem.item_no'
	SET @QueryString =@QueryString+',It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END)'
	SET @QueryString =@QueryString+',MailName=(CASE WHEN ISNULL(ac_mast.MailName,'''')='''' THEN ac_mast.ac_name ELSE ac_mast.mailname END)'
	SET @QueryString =@QueryString+',SAC_NAME=(CASE WHEN ISNULL(S1.MailName,'''')='''' THEN (CASE WHEN ISNULL(ac_mast.MailName,'''')='''' THEN ac_mast.ac_name ELSE ac_mast.mailname END)  ELSE S1.mailname END)'--added by Ruchit for GST
	SET @QueryString =@QueryString+',SADD1=CASE WHEN pimain.SAC_ID >0 THEN S1.ADD1 ELSE AC_MAST.ADD1 END'
	SET @QueryString =@QueryString+',SADD2=CASE WHEN pimain.SAC_ID >0 THEN S1.ADD2 ELSE AC_MAST.ADD2 END'
	SET @QueryString =@QueryString+',SADD3=CASE WHEN pimain.SAC_ID >0 THEN S1.ADD3 ELSE AC_MAST.ADD3 END'
	SET @QueryString =@QueryString+',SCITY=CASE WHEN pimain.SAC_ID >0 THEN S1.CITY ELSE AC_MAST.CITY END'
	SET @QueryString =@QueryString+',SSTATE=CASE WHEN pimain.SAC_ID >0 THEN S1.STATE ELSE AC_MAST.STATE END'
	SET @QueryString =@QueryString+',SZIP=CASE WHEN pimain.SAC_ID >0 THEN S1.ZIP ELSE AC_MAST.ZIP END'
	SET @QueryString =@QueryString+',SPHONE=CASE WHEN pimain.SAC_ID >0 THEN S1.PHONE ELSE AC_MAST.PHONE END'
	SET @QueryString =@QueryString+',SUNIQUEID=CASE WHEN pimain.SAC_ID >0 THEN (CASE WHEN S1.UID<>'''' THEN S1.UID ELSE S1.GSTIN END) ELSE (CASE WHEN AC_MAST.UID<>'''' THEN AC_MAST.UID ELSE AC_MAST.GSTIN END) END'
	SET @QueryString =@QueryString+',SSTATECODE=(SELECT TOP 1 GST_STATE_CODE FROM STATE WHERE STATE=CASE WHEN pimain.SAC_ID >0 THEN S1.STATE ELSE AC_MAST.STATE END)'	
	SET @QueryString =@QueryString+',SCOUNTRY=CASE WHEN pimain.SAC_ID >0 THEN S1.COUNTRY ELSE AC_MAST.COUNTRY END'	
	SET @QueryString =@QueryString+',CAC_NAME=(CASE WHEN ISNULL(S2.MailName,'''')='''' THEN (CASE WHEN ISNULL(ac.MailName,'''')='''' THEN ac.ac_name ELSE ac.mailname END)  ELSE S2.mailname END)'--added by Ruchit for GST
	SET @QueryString =@QueryString+',CADD1=CASE WHEN pimain.SCONS_ID >0 THEN S2.ADD1 ELSE AC.ADD1 END'
	SET @QueryString =@QueryString+',CADD2=CASE WHEN pimain.SCONS_ID >0 THEN S2.ADD2 ELSE AC.ADD2 END'
	SET @QueryString =@QueryString+',CADD3=CASE WHEN pimain.SCONS_ID >0 THEN S2.ADD3 ELSE AC.ADD3 END'
	SET @QueryString =@QueryString+',CCITY=CASE WHEN pimain.SCONS_ID >0 THEN S2.CITY ELSE AC.CITY END'
	SET @QueryString =@QueryString+',CSTATE=CASE WHEN pimain.SCONS_ID >0 THEN S2.STATE ELSE AC.STATE END'
	SET @QueryString =@QueryString+',CZIP=CASE WHEN pimain.SCONS_ID >0 THEN S2.ZIP ELSE AC.ZIP END'
	SET @QueryString =@QueryString+',CPHONE=CASE WHEN pimain.SCONS_ID >0 THEN S2.PHONE ELSE AC.PHONE END'
	SET @QueryString =@QueryString+',CUNIQUEID=CASE WHEN pimain.SCONS_ID >0 THEN (CASE WHEN S2.UID<>'''' THEN S2.UID ELSE S2.GSTIN END) ELSE (CASE WHEN AC.UID<>'''' THEN AC.UID ELSE AC.GSTIN END) END'	
	SET @QueryString =@QueryString+',CSTATECODE=(SELECT TOP 1 GST_STATE_CODE FROM STATE WHERE STATE=CASE WHEN pimain.SCONS_ID >0 THEN S2.STATE ELSE AC.STATE END)'	
	SET @QueryString =@QueryString+',CCOUNTRY=CASE WHEN pimain.SCONS_ID >0 THEN S2.COUNTRY ELSE AC.COUNTRY END'
	SET @QueryString =@QueryString+',GSTSTATE,ST_TYPE=CASE WHEN pimain.SCONS_ID >0 THEN S2.ST_TYPE ELSE AC.ST_TYPE END'			
	SET @QueryString =@QueryString+',piitem.ITSERIAL,item_fdisc=piitem.tot_fdisc'
	SET @QueryString =@QueryString+',piitem.CGST_PER,piitem.CGST_AMT,piitem.SGST_PER,piitem.SGST_AMT,piitem.IGST_PER,piitem.IGST_AMT'
	SET @QueryString =@QueryString+',piitem.Compcess,piitem.CCESSRATE'
	SET @QueryString =@QueryString+',ITADDLESS='+@ITAddLess+',ITEMTAX='+@ItemTaxable+',ITEMNTX='+@ItemNTX+',HEADERDISC='+@HeaderDisc+''
	SET @QueryString =@QueryString+',HEADERTAX='+@HeaderTAX+',HEADERNTX='+@HeaderNTX+''
	SET @QueryString =@QueryString+',pimain.Party_nm,ac_mast.GSTIN,ac_mast.State,piitem.item,piitem.stkunit,Total=piitem.rate*piitem.qty'
	SET @QueryString =@QueryString+',TotalGST=piitem.CGST_AMT+piitem.SGST_AMT+piitem.IGST_AMT,pimain.u_deli as transname'


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
			--Set @Tot_flds=@Tot_flds+','+@fld   --Commented by Divyang P on 17032020 for Bug-33349  End
			--Set @Tot_flds=@Tot_flds+','+@fld +' as '+ rtrim(@head_nm) 
			Set @Tot_flds=@Tot_flds+','+@fld +' as ['+ rtrim(@head_nm) +']'	--Added by Divyang P on 18032020 for Bug-33349 
		end
	End
	Fetch Next from addi_flds Into @fld,@fld_nm1,@att_file,@fld_type,@head_nm
End
Close addi_flds 
Deallocate addi_flds 
declare @sql as nvarchar(max)
set @sql= cAST(REPLICATE(@Tot_flds,4000) as nvarchar(100))

SET @SQLCOMMAND=''
SET @SQLCOMMAND = N''+@QueryString+''+N''+@Tot_flds+''+' into '+'##main11'+' FROM pimain' 
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN piitem ON (pimain.TRAN_CD=piitem.TRAN_CD AND pimain.ENTRY_TY=piitem.ENTRY_TY)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN IT_MAST ON (piitem.IT_CODE=IT_MAST.IT_CODE)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN AC_MAST ON (AC_MAST.AC_ID=pimain.AC_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+ CASE WHEN @Addtblnm<>'' THEN @Addtblnm ELSE '' END
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN AC_MAST AC ON (AC.AC_ID=pimain.CONS_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' LEFT JOIN SHIPTO S1 ON (pimain.AC_ID=S1.AC_ID AND pimain.SAC_ID=S1.SHIPTO_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' LEFT JOIN SHIPTO S2 ON (pimain.CONS_ID=S2.AC_ID AND pimain.SCONS_ID=S2.SHIPTO_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+'WHERE (pimain.date BETWEEN '+CHAR(39)+cast(@FrmDate as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@Todate as varchar )+CHAR(39)+') and (pimain.Party_nm BETWEEN '+CHAR(39)+@FParty+CHAR(39)+' AND '+CHAR(39)+@TParty+CHAR(39)+')'
 SET @SQLCOMMAND =	@SQLCOMMAND+'and (pimain.inv_sr BETWEEN '+CHAR(39)+cast(@FSeries as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TSeries as varchar )+CHAR(39)+') and (pimain.Dept BETWEEN '+CHAR(39)+@FDept+CHAR(39)+' AND '+CHAR(39)+@TDept+CHAR(39)+')'
 SET @SQLCOMMAND =	@SQLCOMMAND+'and (pimain.cate BETWEEN '+CHAR(39)+cast(@FCategory as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TCategory as varchar )+CHAR(39)+') and (piitem.ware_nm BETWEEN '+CHAR(39)+@FWarehouse+CHAR(39)+' AND '+CHAR(39)+@TWarehouse+CHAR(39)+')'
 SET @SQLCOMMAND =	@SQLCOMMAND+' ORDER BY pimain.INV_SR,pimain.INV_NO'
 print @sqlcommand
execute sp_executesql @SQLCOMMAND


--SET @SQLCOMMAND ='select Date,inv_no,Party_nm,GSTIN,State,st_type,item,HSNCODE,qty,stkunit,rate,Total,ITEMTAX,u_asseamt,CGST_PER,CGST_AMT,SGST_PER,SGST_AMT,IGST_PER,IGST_AMT,TotalGST,IT_GROAMT,ITEMNTX,net_amt,ITADDLESS,transname,u_vehno,U_LRDT,u_lrno,CCESSRATE=case when CCESSRATE=''NO-CESS'' then '''' else CCESSRATE end,COMPCESS from ##main11'    --Commented by Divyang P on 17032020 for Bug-33349

--Added by Divyang P on 17032020 for Bug-33349  Start
SET @SQLCOMMAND=''
SET @SQLCOMMAND ='select a.Date,a.inv_no,a.Party_nm,a.GSTIN,a.State,a.st_type,a.item,a.HSNCODE,a.qty,a.stkunit,a.rate,a.Total,a.ITEMTAX,a.u_asseamt,a.CGST_PER,a.CGST_AMT,a.SGST_PER,a.SGST_AMT,a.IGST_PER,a.IGST_AMT,a.TotalGST,a.IT_GROAMT,a.ITEMNTX,a.net_amt,a.ITADDLESS,a.transname,a.u_vehno,a.U_LRDT,a.u_lrno,CCESSRATE=case when a.CCESSRATE=''NO-CESS'' then '''' else a.CCESSRATE end,a.COMPCESS  '
SET @SQLCOMMAND = @SQLCOMMAND + N''+@Tot_flds+''+ ' from ##main11 a inner join PIMAIN on (PIMAIN.tran_cd=a.tran_cd) inner join PIITEM on (PIITEM.tran_cd=PIMAIN.tran_cd)  '
--Added by Divyang P on 17032020 for Bug-33349  END

execute sp_executesql @SQLCOMMAND
drop table ##main11





