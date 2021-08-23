If Exists(Select [name] From SysObjects Where xType='P' and [name]='USP_REP_PO_MIS')
Begin
	Drop Procedure USP_REP_PO_MIS
End
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create procedure [dbo].[USP_REP_PO_MIS]
@SDATE  SMALLDATETIME,@EDATE SMALLDATETIME
,@SAC AS VARCHAR(60),@EAC AS VARCHAR(60)
,@SIT AS VARCHAR(60),@EIT AS VARCHAR(60),@FSeries nvarchar(100),@TSeries nvarchar(100),@FDept nvarchar(100),@TDept nvarchar(100),@FCategory nvarchar(100),@TCategory nvarchar(100),@FWarehouse nvarchar(100),@TWarehouse nvarchar(100)
AS
BEGIN
DECLARE @FCON AS NVARCHAR(2000)
Declare @SQLCOMMAND as NVARCHAR(max)

--Added by Divyang P on 18032020 for Bug-33349  Start
Declare @fld Varchar(50),@fld_nm1 Varchar(50),@att_file bit,@Tot_flds Varchar(4000),@QueryString NVarchar(max),@fld_type  varchar(15),@tempsql varchar(50),@head_nm varchar(100)
Select case when att_file=1 then 'POMAIN.'+RTRIM(FLD_NM) else 'POITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty,head_nm 
Into #tmpFlds 
From Lother Where e_code='PO' and DISP_MIS=1
union all
Select case when att_file=1 then 'POMAIN.'+RTRIM(FLD_NM) else 'POITEM.'+RTRIM(FLD_NM) end as flds,FLD_NM=RTRIM(FLD_NM),att_file,data_ty='',head_nm  From DcMast
Where Entry_ty='PO' and DISP_MIS=1
union all
Select case when att_file=1 then 'POMAIN.'+RTRIM(pert_name) else 'POITEM.'+RTRIM(pert_name) end as flds,FLD_NM=RTRIM(pert_name),att_file,data_ty='',head_nm= (rtrim(head_nm)+'PER')  From DcMast
Where Entry_ty='PO' and DISP_MIS=1
--Added by Divyang P on 18032020 for Bug-33349  End

SET @QueryString = ''
SET @QueryString = 'Select [Financial Year]=m.l_yn,YearMonth=convert(varchar(4),year(m.date))+''-''+convert(varchar(2),m.date,101), 
[Month]=left(datename(month,m.date),3)+''-''+right(year(m.date),2),m.Net_Amt,m.inv_No,m.Date
,i.QTY,i.Rate,m.Entry_ty,m.Tran_cd,i.ItSerial
,BillTo=acm.Ac_Name,ShipTo=(case when sh.mailname<>'''' then sh.mailname else sh.ac_name end) 
,acm.GSTIN,im.It_Name,im.RateUnit,im.HsnCode,GS=(Case when im.IsService=1 Then ''Service'' Else ''Goods'' End),ig.IT_Group_Name
,STQty=Sum(isnull(ist.Qty,0))
,TrQty=(isnull(itr.RQty,0)),Location=sh.Location_Id'

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
--Added by Divyang P on 18032020 for Bug-33349  End


SET @SQLCOMMAND=''
SET @SQLCOMMAND= N''+@QueryString+''+' into '+'##t'+' FROM POMain m ' 
SET @SQLCOMMAND = @SQLCOMMAND+ 'inner Join POItem i on (m.Tran_cd=i.Tran_cd) '
SET @SQLCOMMAND = @SQLCOMMAND+ 'Inner Join It_Mast im on (i.It_Code=im.It_Code)'
SET @SQLCOMMAND = @SQLCOMMAND+ 'Inner Join Item_Group ig on (im.itgrid=ig.itgrid)'
SET @SQLCOMMAND = @SQLCOMMAND+ 'Inner Join Ac_Mast acm on (m.Ac_Id=acm.Ac_Id)'
SET @SQLCOMMAND = @SQLCOMMAND+ 'Left Join PTItRef isr on (i.Entry_Ty=isr.rEntry_Ty and i.Tran_cd=isr.itRef_Tran and i.ItSerial=isr.rItSerial) '
SET @SQLCOMMAND = @SQLCOMMAND+ 'Left Join PTItem  ist on (ist.Entry_Ty=isr.Entry_Ty  and ist.Tran_cd=isr.Tran_Cd and ist.ItSerial=isr.ItSerial)'
SET @SQLCOMMAND = @SQLCOMMAND+ 'Left Join PTMain ms on (isr.Tran_Cd=m.Tran_Cd)'
SET @SQLCOMMAND = @SQLCOMMAND+ 'Left Join ShipTo sh ON (sh.ac_id = m.Ac_id and sh.shipto_id = m.sAc_id)'
SET @SQLCOMMAND = @SQLCOMMAND+ 'Left Join TRItRef itr on (i.Entry_Ty=itr.rEntry_Ty and i.Tran_cd=itr.itRef_Tran and i.ItSerial=itr.rItSerial)'
SET @SQLCOMMAND = @SQLCOMMAND+ 'WHERE (m.Date BETWEEN '''+cast(@SDATE as varchar)+'''  AND '''+cast(@EDATE as varchar )+''') and (m.Party_nm BETWEEN '''+@SAC+''' AND '''+@EAC+''')and (im.It_Name BETWEEN '''+@SIT+''' AND '''+@EIT+''')'
SET @SQLCOMMAND =	@SQLCOMMAND+'and (m.inv_sr BETWEEN '+CHAR(39)+cast(@FSeries as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TSeries as varchar )+CHAR(39)+') and (m.Dept BETWEEN '+CHAR(39)+@FDept+CHAR(39)+' AND '+CHAR(39)+@TDept+CHAR(39)+')'
SET @SQLCOMMAND =	@SQLCOMMAND+'and (m.cate BETWEEN '+CHAR(39)+cast(@FCategory as varchar)+CHAR(39)+'  AND '+CHAR(39)+cast(@TCategory as varchar )+CHAR(39)+') and (i.ware_nm BETWEEN '+CHAR(39)+@FWarehouse+CHAR(39)+' AND '+CHAR(39)+@TWarehouse+CHAR(39)+')'
SET @SQLCOMMAND = @SQLCOMMAND+ 'Group By m.l_yn,m.Net_Amt,m.inv_No,m.Date
,i.QTY,i.Rate,m.Entry_ty,m.Tran_cd,i.ItSerial
,acm.Ac_Name,(case when sh.mailname<>'''' then sh.mailname else sh.ac_name end) 
,acm.GSTIN,im.It_Name,im.RateUnit,im.HsnCode,(Case when im.IsService=1 Then ''Service'' Else ''Goods'' End),ig.IT_Group_Name,itr.RQty,sh.Location_Id '
print @SQLCOMMAND
execute sp_executesql @SQLCOMMAND

--Commented by Divyang P on 18032020 for Bug-33349  Start
--Select [Financial Year]=m.l_yn,YearMonth=convert(varchar(4),year(m.date))+'-'+convert(varchar(2),m.date,101),  --Modified by Divyang for Tkt-33129 DT:18/12/2019
--[Month]=left(datename(month,m.date),3)+'-'+right(year(m.date),2),m.Net_Amt,m.inv_No,m.Date
--,i.QTY,i.Rate,m.Entry_ty,m.Tran_cd,i.ItSerial
--,BillTo=acm.Ac_Name,ShipTo=(case when sh.mailname<>'''' then sh.mailname else sh.ac_name end) 
--,acm.GSTIN,im.It_Name,im.RateUnit,im.HsnCode,GS=(Case when im.IsService=1 Then 'Service' Else 'Goods' End),ig.IT_Group_Name
--,STQty=Sum(ist.Qty)
--,TrQty=(isnull(itr.RQty,0)),Location=sh.Location_Id
----,BalQty=i.QTY-isnull(Sum(ist.Qty),0)-isnull((itr.RQty),0)
----,PendDayDateDiff(day,'2018/08/01',GetDate())
--into #t
--From POMain m 
--inner Join POItem i on (m.Tran_cd=i.Tran_cd)
--Inner Join It_Mast im on (i.It_Code=im.It_Code)
--Inner Join Item_Group ig on (im.itgrid=ig.itgrid)
--Inner Join Ac_Mast acm on (m.Ac_Id=acm.Ac_Id)
--Left Join PTItRef isr on (i.Entry_Ty=isr.rEntry_Ty and i.Tran_cd=isr.itRef_Tran and i.ItSerial=isr.rItSerial)
--Left Join PTItem  ist on (ist.Entry_Ty=isr.Entry_Ty  and ist.Tran_cd=isr.Tran_Cd and ist.ItSerial=isr.ItSerial)
--Left Join PTMain ms on (isr.Tran_Cd=m.Tran_Cd)
--Left Join ShipTo sh ON (sh.ac_id = m.Ac_id and sh.shipto_id = m.sAc_id)
--Left Join TRItRef itr on (i.Entry_Ty=itr.rEntry_Ty and i.Tran_cd=itr.itRef_Tran and i.ItSerial=itr.rItSerial)

--WHERE (m.Date BETWEEN cast(@SDATE as varchar)  AND cast(@EDATE as varchar )) and (m.Party_nm BETWEEN @SAC AND @EAC)and (im.It_Name BETWEEN @SIT AND @EIT)

--Group By m.l_yn,m.Net_Amt,m.inv_No,m.Date
--,i.QTY,i.Rate,m.Entry_ty,m.Tran_cd,i.ItSerial
--,acm.Ac_Name,(case when sh.mailname<>'''' then sh.mailname else sh.ac_name end) 
--,acm.GSTIN,im.It_Name,im.RateUnit,im.HsnCode,(Case when im.IsService=1 Then 'Service' Else 'Goods' End),ig.IT_Group_Name,itr.RQty,sh.Location_Id --Modified by Divyang for Tkt-33129 DT:18/12/2019

--Select [Financial Year],YearMonth,[Month],Net_Amt,inv_No,Date,QTY,Rate,BillTo,ShipTo,GSTIN,It_Name,RateUnit,HsnCode,GS,IT_Group_Name,STQty,TRQty,BalQty=Qty-STQty-TrQTY    --Modified by Divyang for Tkt-33129 DT:18/12/2019
--,PendDay=(Case When Qty-STQty-TRQTY>0 Then DateDiff(day,Date,GetDate()) Else 0 End),Location
--From #t
--Commented by Divyang P on 18032020 for Bug-33349  END



SET @SQLCOMMAND=''
SET @SQLCOMMAND ='Select [Financial Year],YearMonth,[Month],a.Net_Amt,a.inv_No,a.Date,a.QTY,a.Rate,a.BillTo,a.ShipTo,a.GSTIN,a.It_Name,a.RateUnit,a.HsnCode,a.GS,a.IT_Group_Name,a.STQty,a.TRQty,BalQty=a.Qty-a.STQty-a.TrQTY '
SET @SQLCOMMAND = @SQLCOMMAND + ' ,PendDay=(Case When a.Qty-a.STQty-a.TRQTY>0 Then DateDiff(day,a.Date,GetDate()) Else 0 End),a.Location '
SET @SQLCOMMAND = @SQLCOMMAND + N''+@Tot_flds+''+ ' from ##t a  inner join POmain on (POmain.tran_cd=a.tran_cd) inner join POitem on (POmain.tran_cd=POitem.tran_cd) '
execute sp_executesql @SQLCOMMAND
drop table ##t

--set DATEFORMAT dmy Execute USP_REP_PO_MIS N'01/01/2018',N'31/01/2019','Party 1','Party 1','Goods 1','Goods 1'
END




