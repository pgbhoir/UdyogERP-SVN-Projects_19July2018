IF EXISTS(SELECT [NAME] FROM SYS.PROCEDURES WHERE [NAME]='USP_GET_SCHEDULEWISE_RECORDS')
BEGIN
	DROP PROCEDURE USP_GET_SCHEDULEWISE_RECORDS
END
GO

--***************************************************************--
-- Procedure Name : USP_GET_SCHEDULEWISE_RECORDS
-- Procedure Desc : This procedure is called in Scheduler Ledger Mapping Master
--					Used to get the records from Schedule Master
-- Created by\on  : Sachin N. S.
-- Changed by\on  : Sachin N. S. on 07/03/2020 for Bug-32672
--***************************************************************--
CREATE PROCEDURE [USP_GET_SCHEDULEWISE_RECORDS]
@SCHDCODE VARCHAR(10), @SCHDTYPE VARCHAR(5)
AS
BEGIN
	;WITH CTE AS
	(
		Select 'SN' as [Type],SchdCode,SchdType,SchdId,SchdName,SchdParId,SerialNo,SchdNtNo, 
			'SN'+'_'+Cast(SchdParId as Varchar) as ParCode, cast('SN_'+cast(SchdId as varchar) as varchar(200)) as [OrderPath] 
			From schdhdtbl 
				Where SchdCode=@SCHDCODE and SchdType=@SCHDTYPE AND SCHDPARID=0
					AND CFORMULA=''			-- Added by Sachin N. S. on 07/03/2020 for Bug-32672
		UNION ALL
		Select 'SN' as [Type],A.SchdCode,A.SchdType,A.SchdId,A.SchdName,A.SchdParId,A.SerialNo,A.SchdNtNo, 
			'SN'+'_'+Cast(B.SchdId as Varchar) as ParCode,
			cast(B.[OrderPath]+'/'+'SN_'+cast(A.SchdId as varchar) as varchar(200)) as [OrderPath] 
			FROM SCHDHDTBL A
				INNER JOIN CTE B ON A.SCHDPARID=B.SCHDID
					Where A.SchdCode=@SCHDCODE and A.SchdType=@SCHDTYPE
						AND A.CFORMULA=''			-- Added by Sachin N. S. on 07/03/2020 for Bug-32672
	)SELECT * INTO ##tmpcte1 FROM CTE
	
			
	;With cte2 as
	(
		Select cast(rtrim(a.GrporAc)+'U' as varchar(2)) as [Type], 
			b.SchdCode, b.SchdType, a.GrpOrAcId, Cast(a.GrpOrAcNm as varchar(50)) as GrpOrAcNm, a.SchdId,
			'' as SerialNo, 0 as SchdNtNo, B.[Type]+'_'+Cast(B.SchdId as Varchar) as ParCode,
			cast(b.[OrderPath]+'/'+cast(rtrim(a.GrporAc)+'U' as varchar(2))+'_'+cast(a.GrpOrAcId as varchar) as Varchar(200)) as [OrderPath]
			from schddttbl a
				inner join ##tmpcte1 b on a.SchdId=b.SchdId
		Union all
		Select 'GN' as [Type], b.SchdCode,b.SchdType,cast(a.Ac_group_id as Int),cast(a.ac_group_name as varchar(50)),cast(a.gac_id as int), '',0,
			b.[Type]+'_'+Cast(a.Gac_Id as Varchar) as ParCode,
			cast(b.[OrderPath]+'/'+'GN_'+Cast(a.ac_group_id as varchar) as Varchar(200)) as [OrderPath]
			from ac_group_mast a
				inner join cte2 b on a.Gac_id = b.GrpOrAcId and left(b.[Type],1)='G'
	)
	,cte3 as
	(
		Select * from cte2
		Union all 
		Select 'AN',b.SchdCode,b.SchdType,a.Ac_id,cast(a.Ac_name as varchar(50)),a.ac_group_id,'',0,
			b.[Type]+'_'+Cast(a.ac_group_Id as Varchar) as ParCode,
			Cast(b.[OrderPath]+'/'+'AN'+'_'+Cast(a.ac_id as varchar) as Varchar(200)) as [OrderPath]
			from ac_mast a
				inner join cte2 b on a.Ac_group_id =b.grporacid and left(b.[Type],1)='G'
	)
	Select *,'E' as AddEdit from ##tmpcte1
	Union all
	Select *,'E' as AddEdit from cte3
	ORDER BY OrderPath
		
	Drop Table ##tmpcte1
END