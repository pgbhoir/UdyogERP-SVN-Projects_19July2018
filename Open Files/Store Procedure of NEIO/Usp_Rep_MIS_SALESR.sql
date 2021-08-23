If Exists(Select [name] From SysObjects Where xType='P' and [name]='Usp_Rep_MIS_SALESR')
Begin
	Drop Procedure Usp_Rep_MIS_SALESR
End
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Usp_Rep_MIS_SALESR] 
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
                 (SELECT Case when a_s<>'' then a_s else (case when code in( 'D','F') THEN '-' else '+' end ) end + fld_nm FROM Dcmast where att_file=0 and Entry_ty='st' and code in( 'D','F')  FOR XML PATH ('')), 1, 0, ''
               ),0)               
--itemwise taxable and non taxable
SELECT 
    @ItemTaxable = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code='T' and att_file=0 and Entry_ty='st'  FOR XML PATH ('')), 1, 0, ''
                 --(select case when fld_nm<>'' then '+'+fld_nm else ''  end from dcmast where code='T' and att_file=0 and entry_ty='ST' and bef_aft=2  FOR XML PATH ('')), 1, 0, ''
               ),0)
SELECT 
    @ItemNTX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=0 and Entry_ty='ST' and fld_nm not in ('Compcess','k_cess') /*and bef_aft=2*/  FOR XML PATH ('')), 1, 0, ''
               ),0)   
             
--itemwise non tax

--headerwise disc
SELECT 
    @HeaderDisc = isnull(STUFF(
                 (SELECT  '-'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('D','F') and att_file=1  and Entry_ty='st'  FOR XML PATH ('')), 1, 0, ''
               ),0)

--headerwise taxable chargs
SELECT 
    @HeaderTAX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code='T' and att_file=1 and Entry_ty='st'  FOR XML PATH ('')), 1, 0, ''
               ),0)

--headerwise non taxable chargs
SELECT 
    @HeaderNTX = isnull(STUFF(
                 (SELECT  '+'+ltrim(rtrim(fld_nm)) FROM Dcmast where code in ('N','A') and att_file=1 and Entry_ty='st'  FOR XML PATH ('')), 1, 0, ''
               ),0)


Declare @Addtblnm Varchar(1000)
set @Addtblnm  =''
if Exists(Select Top 1 [Name] From SysObjects Where xType='U' and [Name]='StmainAdd')
Begin
	set @Addtblnm =' LEFT JOIN STMAINADD ON (STMAIN.TRAN_CD=STMAINADD.TRAN_CD) '
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
		set @stkl_qty= @stkl_qty +', '+'STITEM.'+@fld_nm

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
	set @stkl_qty=',STITEM.QTY'
End

Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds nVarchar(max),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50),@head_nm varchar(100)
Select case when att_file=1 then (CASE WHEN RTRIM(TBL_NM)='STMAINADD' THEN 'STMAINADD.' ELSE 'STMAIN.' END )+RTRIM(FLD_NM) else 'STITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm
Into #tmpFlds 
From Lother Where e_code='ST' and DISP_MIS=1
union all
Select case when att_file=1 then 'STMAIN.'+RTRIM(FLD_NM) else 'STITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='ST'	 and DISP_MIS=1
--Added by Divyang P on 17032020 for Bug-33349  Start
union all
Select case when att_file=1 then 'STMAIN.'+RTRIM(pert_name) else 'STITEM.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm= (rtrim(head_nm)+'PER')  From DcMast
Where Entry_ty='ST' and DISP_MIS=1
--Added by Divyang P on 17032020 for Bug-33349  End

	SET @QueryString = ''
	SET @QueryString = 'SELECT ''REPORT HEADER'' AS REP_HEAD,STMAIN.INV_SR,STMAIN.TRAN_CD,STMAIN.ENTRY_TY,STMAIN.INV_NO,STMAIN.DATE,STMAIN.U_TIMEP,STMAIN.U_TIMEP1 ,STMAIN.U_REMOVDT'
	SET @QueryString =@QueryString+',[Financial Year]=STMAIN.l_yn,YearMonth=convert(varchar(4),year(STMAIN.date))+''-''+convert(varchar(2),STMAIN.date,101),[Month]=left(datename(month,STMAIN.date),3)+''-''+right(year(STMAIN.date),2) '   --Modified by Divyang for Tkt-33129 DT:18/12/2019   fld:l_yn,YearMonth,Month,Location
	SET @QueryString =@QueryString+',STMAIN.U_DELIVER,STMAIN.DUE_DT,STMAIN.U_CLDT,STMAIN.U_CHALNO,STMAIN.U_CHALDT,STMAIN.U_PONO,STMAIN.U_PODT,STMAIN.U_LRNO,STMAIN.U_LRDT,STMAIN.U_DELI,STMAIN.U_VEHNO,STITEM.GRO_AMT AS IT_GROAMT,STMAIN.GRO_AMT GRO_AMT1,STMAIN.TAX_NAME,STITEM.TAX_NAME AS IT_TAXNAME,STMAIN.TAXAMT,STITEM.TAXAMT AS IT_TAXAMT,STMAIN.NET_AMT,STITEM.U_PKNO'+@stkl_qty+',STITEM.RATE,STITEM.U_ASSEAMT,STITEM.U_MRPRATE,STITEM.U_EXPDESC,STITEM.U_APPACK,STITEM.PACKSIZE1, cast (STITEM.NARR AS VARCHAR(2000)) AS NARR,STITEM.TOT_EXAMT'
	SET @QueryString =@QueryString+',IT_MAST.IT_NAME,IT_MAST.EIT_NAME,IT_MAST.CHAPNO,IT_MAST.IDMARK,IT_MAST.RATEUNIT
	--,IT_MAST.HSNCODE  --Commented by Priyanka B on 09122017 for AU 13.0.5
	,HSNCODE = (CASE WHEN ISSERVICE=1 THEN IT_MAST.SERVTCODE ELSE IT_MAST.HSNCODE END)  --Modified by Priyanka B on 09122017 for AU 13.0.5
	,IT_mast.U_ITPARTCD,stitem.item_no'
	SET @QueryString =@QueryString+',It_Desc=(CASE WHEN ISNULL(it_mast.it_alias,'''')='''' THEN it_mast.it_name ELSE it_mast.it_alias END)'
	SET @QueryString =@QueryString+',MailName=(CASE WHEN ISNULL(ac_mast.MailName,'''')='''' THEN ac_mast.ac_name ELSE ac_mast.mailname END)'
	SET @QueryString =@QueryString+',SAC_NAME=(CASE WHEN ISNULL(S1.MailName,'''')='''' THEN (CASE WHEN ISNULL(ac_mast.MailName,'''')='''' THEN ac_mast.ac_name ELSE ac_mast.mailname END)  ELSE S1.mailname END)'--added by Ruchit for GST
	SET @QueryString =@QueryString+',SADD1=CASE WHEN STMAIN.SAC_ID >0 THEN S1.ADD1 ELSE AC_MAST.ADD1 END'
	SET @QueryString =@QueryString+',SADD2=CASE WHEN STMAIN.SAC_ID >0 THEN S1.ADD2 ELSE AC_MAST.ADD2 END'
	SET @QueryString =@QueryString+',SADD3=CASE WHEN STMAIN.SAC_ID >0 THEN S1.ADD3 ELSE AC_MAST.ADD3 END'
	SET @QueryString =@QueryString+',SCITY=CASE WHEN STMAIN.SAC_ID >0 THEN S1.CITY ELSE AC_MAST.CITY END'
	SET @QueryString =@QueryString+',SSTATE=CASE WHEN STMAIN.SAC_ID >0 THEN S1.STATE ELSE AC_MAST.STATE END'
	SET @QueryString =@QueryString+',SZIP=CASE WHEN STMAIN.SAC_ID >0 THEN S1.ZIP ELSE AC_MAST.ZIP END'
	SET @QueryString =@QueryString+',SPHONE=CASE WHEN STMAIN.SAC_ID >0 THEN S1.PHONE ELSE AC_MAST.PHONE END'
	SET @QueryString =@QueryString+',SUNIQUEID=CASE WHEN STMAIN.SAC_ID >0 THEN (CASE WHEN S1.UID<>'''' THEN S1.UID ELSE S1.GSTIN END) ELSE (CASE WHEN AC_MAST.UID<>'''' THEN AC_MAST.UID ELSE AC_MAST.GSTIN END) END'
	SET @QueryString =@QueryString+',SSTATECODE=(SELECT TOP 1 GST_STATE_CODE FROM STATE WHERE STATE=CASE WHEN STMAIN.SAC_ID >0 THEN S1.STATE ELSE AC_MAST.STATE END)'	
	SET @QueryString =@QueryString+',SCOUNTRY=CASE WHEN STMAIN.SAC_ID >0 THEN S1.COUNTRY ELSE AC_MAST.COUNTRY END'	
	SET @QueryString =@QueryString+',CAC_NAME=(CASE WHEN ISNULL(S2.MailName,'''')='''' THEN (CASE WHEN ISNULL(ac.MailName,'''')='''' THEN ac.ac_name ELSE ac.mailname END)  ELSE S2.mailname END)'--added by Ruchit for GST
	SET @QueryString =@QueryString+',CADD1=CASE WHEN STMAIN.SCONS_ID >0 THEN S2.ADD1 ELSE AC.ADD1 END'
	SET @QueryString =@QueryString+',CADD2=CASE WHEN STMAIN.SCONS_ID >0 THEN S2.ADD2 ELSE AC.ADD2 END'
	SET @QueryString =@QueryString+',CADD3=CASE WHEN STMAIN.SCONS_ID >0 THEN S2.ADD3 ELSE AC.ADD3 END'
	SET @QueryString =@QueryString+',CCITY=CASE WHEN STMAIN.SCONS_ID >0 THEN S2.CITY ELSE AC.CITY END'
	SET @QueryString =@QueryString+',CSTATE=CASE WHEN STMAIN.SCONS_ID >0 THEN S2.STATE ELSE AC.STATE END'
	SET @QueryString =@QueryString+',CZIP=CASE WHEN STMAIN.SCONS_ID >0 THEN S2.ZIP ELSE AC.ZIP END'
	SET @QueryString =@QueryString+',CPHONE=CASE WHEN STMAIN.SCONS_ID >0 THEN S2.PHONE ELSE AC.PHONE END'
	SET @QueryString =@QueryString+',CUNIQUEID=CASE WHEN STMAIN.SCONS_ID >0 THEN (CASE WHEN S2.UID<>'''' THEN S2.UID ELSE S2.GSTIN END) ELSE (CASE WHEN AC.UID<>'''' THEN AC.UID ELSE AC.GSTIN END) END'	
	SET @QueryString =@QueryString+',CSTATECODE=(SELECT TOP 1 GST_STATE_CODE FROM STATE WHERE STATE=CASE WHEN STMAIN.SCONS_ID >0 THEN S2.STATE ELSE AC.STATE END)'	
	SET @QueryString =@QueryString+',CCOUNTRY=CASE WHEN STMAIN.SCONS_ID >0 THEN S2.COUNTRY ELSE AC.COUNTRY END'
	SET @QueryString =@QueryString+',GSTSTATE,ST_TYPE=CASE WHEN STMAIN.SCONS_ID >0 THEN S2.ST_TYPE ELSE AC.ST_TYPE END'			
	SET @QueryString =@QueryString+',STITEM.ITSERIAL,item_fdisc=stitem.tot_fdisc'
	SET @QueryString =@QueryString+',STMAIN.ctdsamt,STMAIN.stdsamt,STMAIN.itdsamt,Stitem.Tariff,STMAIN.roundoff'		--Added by Shrikant S. on 25/04/2017 for GST
	SET @QueryString =@QueryString+',STITEM.CGST_PER,STITEM.CGST_AMT,STITEM.SGST_PER,STITEM.SGST_AMT,STITEM.IGST_PER,STITEM.IGST_AMT'
	SET @QueryString =@QueryString+',STITEM.Compcess,stitem.CCESSRATE'-- Added By Prajakta B. On 28082017
	SET @QueryString =@QueryString+',ITADDLESS='+@ITAddLess+',ITEMTAX='+@ItemTaxable+',ITEMNTX='+@ItemNTX+',HEADERDISC='+@HeaderDisc+''--ruchit, NONTAXIT='+@NonTaxIT+'
	SET @QueryString =@QueryString+',HEADERTAX='+@HeaderTAX+',HEADERNTX='+@HeaderNTX+''
	SET @QueryString =@QueryString+',STMAIN.Party_nm,ac_mast.GSTIN,ac_mast.State,STITEM.item,STITEM.stkunit,Total=STITEM.rate*STITEM.qty '
	SET @QueryString =@QueryString+',TotalGST=STITEM.CGST_AMT+STITEM.SGST_AMT+STITEM.IGST_AMT,STMAIN.EWBN,STMAIN.EWBDT,STMAINadd.EWBDIST '
	SET @QueryString =@QueryString+',STMAIN.u_deli as transname,STMAIN.EWBVTD,STITEM.LINERULE,Location=s1.location_id,stmain.U_FREIGHT,stitem.PKGFRWD'
print @SQLCOMMAND
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
			--Set @Tot_flds=@Tot_flds+','+@fld    -- Commented By Divyang P on 17032020 for Bug-33349
			--Set @Tot_flds=@Tot_flds+','+@fld +' as '+ rtrim(@head_nm)	-- Added By Divyang P on 17032020 for Bug-33349
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
SET @SQLCOMMAND = N''+@QueryString+''+N''+@Tot_flds+''+' into '+'##main111'+' FROM STMAIN' 

 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN STITEM ON (STMAIN.TRAN_CD=STITEM.TRAN_CD AND STMAIN.ENTRY_TY=STITEM.ENTRY_TY)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN IT_MAST ON (STITEM.IT_CODE=IT_MAST.IT_CODE)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN AC_MAST ON (AC_MAST.AC_ID=STMAIN.AC_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+CASE WHEN @Addtblnm<>'' THEN @Addtblnm ELSE '' END
 SET @SQLCOMMAND =	@SQLCOMMAND+' INNER JOIN AC_MAST AC ON (AC.AC_ID=STMAIN.CONS_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' LEFT JOIN SHIPTO S1 ON (STMAIN.AC_ID=S1.AC_ID AND STMAIN.SAC_ID=S1.SHIPTO_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+' LEFT JOIN SHIPTO S2 ON (STMAIN.CONS_ID=S2.AC_ID AND STMAIN.SCONS_ID=S2.SHIPTO_ID)'
 SET @SQLCOMMAND =	@SQLCOMMAND+'WHERE (STMAIN.date BETWEEN '+CHAR(39)+cast(@FrmDate as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@Todate as varchar )+CHAR(39)+') and (STMAIN.Party_nm BETWEEN '+CHAR(39)+@FParty+CHAR(39)+' AND '+CHAR(39)+@TParty+CHAR(39)+') '
 SET @SQLCOMMAND =	@SQLCOMMAND+'and (STMAIN.inv_sr BETWEEN '+CHAR(39)+cast(@FSeries as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TSeries as varchar )+CHAR(39)+') and (STMAIN.Dept BETWEEN '+CHAR(39)+@FDept+CHAR(39)+' AND '+CHAR(39)+@TDept+CHAR(39)+')'
SET @SQLCOMMAND =	@SQLCOMMAND+'and (STMAIN.cate BETWEEN '+CHAR(39)+cast(@FCategory as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TCategory as varchar )+CHAR(39)+') and (STitem.ware_nm BETWEEN '+CHAR(39)+@FWarehouse+CHAR(39)+' AND '+CHAR(39)+@TWarehouse+CHAR(39)+')'
 SET @SQLCOMMAND =	@SQLCOMMAND+' ORDER BY STMAIN.INV_SR,STMAIN.INV_NO'
 print @SQLCOMMAND
execute sp_executesql @SQLCOMMAND

--SET @SQLCOMMAND ='select [Financial Year],YearMonth,[Month],Date,inv_no,Party_nm,GSTIN,State,st_type,item,HSNCODE,qty,stkunit,rate,Total,ITEMTAX,u_asseamt,CGST_PER,CGST_AMT,SGST_PER,SGST_AMT,IGST_PER,IGST_AMT,TotalGST,EWBN,EWBDT,EWBDIST,roundoff,IT_GROAMT,ITEMNTX,net_amt,ITADDLESS,transname,u_vehno,U_LRDT,u_lrno,EWBVTD,U_FREIGHT,LINERULE,PKGFRWD,CCESSRATE=case when CCESSRATE=''NO-CESS'' then '''' else CCESSRATE end,COMPCESS,Location from ##main11'    --Modified by Divyang for Tkt-33129 DT:18/12/2019   fld:l_yn,YearMonth,Month,Location        --Commented by Divyang P on 17032020 forBug-33349

--Added by Divyang P on 17032020 for Bug-33349  Start
SET @SQLCOMMAND=''
SET @SQLCOMMAND ='select [Financial Year],YearMonth,[Month],a.Date,a.inv_no,a.Party_nm,a.GSTIN,a.State,st_type,a.item,HSNCODE,a.qty,a.stkunit,a.rate,a.Total,a.ITEMTAX,a.u_asseamt,a.CGST_PER,a.CGST_AMT,a.SGST_PER,a.SGST_AMT,a.IGST_PER,a.IGST_AMT,a.TotalGST,a.EWBN,a.EWBDT,a.EWBDIST,a.roundoff,a.IT_GROAMT,a.ITEMNTX,a.net_amt,a.ITADDLESS,a.transname,a.u_vehno,a.U_LRDT,a.u_lrno,a.EWBVTD,a.U_FREIGHT,a.LINERULE,a.PKGFRWD,CCESSRATE=case when a.CCESSRATE=''NO-CESS'' then '''' else a.CCESSRATE end,a.COMPCESS,a.Location '
SET @SQLCOMMAND = @SQLCOMMAND + N''+@Tot_flds+''+ ' from ##main111 a inner join STMAIN on (STMAIN.tran_cd=a.tran_cd) inner join STITEM on (STITEM.tran_cd=STMAIN.tran_cd and stitem.item_no=a.item_no) inner join STMAINADD on (STMAINADD.Tran_cd=STMAIN.Tran_cd)'
--Added by Divyang P on 17032020 for Bug-33349  END
print @SQLCOMMAND
execute sp_executesql @SQLCOMMAND
drop table ##main111





