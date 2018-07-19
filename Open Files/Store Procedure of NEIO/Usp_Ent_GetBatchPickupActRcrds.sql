DROP PROCEDURE [Usp_Ent_GetBatchPickupActRcrds]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		: Sachin .N. Sapaliga
-- Create date	: 20/03/2012
-- Description	: This Stored procedure is used in populating Batchwise/Serialize Split Quantity for Trading pickup. Bug-3196
-- Modification Date/By/Reason: Sachin N. S. on 08/03/2014 for Bug-21381
-- Modification Date/By/Reason: Sachin N. S. on 12/06/2014 for Bug-22228
-- Remark		:
-- =============================================
CREATE PROCEDURE [Usp_Ent_GetBatchPickupActRcrds]
@ITCODE INT, @ENTRY_TY  VARCHAR(2), @DATE SMALLDATETIME, @TRAN_CD INT, @ITSERIAL VARCHAR(5), 
@RENTRY_TY VARCHAR(2), @ITREF_TRAN INT, @RITSERIAL VARCHAR(5)
AS
DECLARE @SQLCOMMAND NVARCHAR(4000)


------ Added by Sachin N. S. on 12/06/2014 for Bug-22228 -- Start
;WITH _TMPIT_SRTRN_B (ENTRY_TY, TRAN_CD, ITSERIAL, RENTRY_TY, RTRAN_CD, RINV_NO, RITSERIAL, ITRAN_CD, RQTY) AS 
(
	SELECT A.ENTRY_TY, A.TRAN_CD, A.ITSERIAL, A.RENTRY_TY, A.RTRAN_CD, A.RINV_NO, A.RITSERIAL, A.ITRAN_CD, CASE WHEN A.PMKEY='+' THEN +(ISNULL(A.RQTY,0)) ELSE -(ISNULL(A.RQTY,0)) END AS RQTY
		FROM IT_SRTRN A 
	WHERE A.RENTRY_TY=@RENTRY_TY AND A.RTRAN_CD=@ITREF_TRAN AND A.RITSERIAL=@RITSERIAL 
		AND A.IT_CODE=@ITCODE AND A.ENTRY_TY!='IR' AND ISNULL(A.DC_NO,'')=''
	UNION ALL
	SELECT A.ENTRY_TY, A.TRAN_CD, A.ITSERIAL, A.RENTRY_TY, A.RTRAN_CD, A.RINV_NO, A.RITSERIAL, A.ITRAN_CD, CASE WHEN A.PMKEY='+' THEN +(ISNULL(A.RQTY,0)) ELSE -(ISNULL(A.RQTY,0)) END AS RQTY
		FROM IT_SRTRN A 
		INNER JOIN _TMPIT_SRTRN_B B ON A.RENTRY_TY=B.ENTRY_TY AND A.RTRAN_CD=B.TRAN_CD AND A.RITSERIAL=B.ITSERIAL AND A.ITRAN_CD=B.ITRAN_CD
	WHERE NOT (A.ENTRY_TY=@ENTRY_TY AND A.TRAN_CD=@TRAN_CD) AND A.ENTRY_TY!='IR' AND ISNULL(A.DC_NO,'')=''
)
SELECT A.RENTRY_TY AS ENTRY_TY, A.RDATE AS DATE, A.RTRAN_CD AS TRAN_CD, A.RINV_NO AS INV_NO, A.RITSERIAL AS ITSERIAL, 
	A.ENTRY_TY AS RENTRY_TY, A.DATE AS RDATE, A.TRAN_CD AS RTRAN_CD, A.ITSERIAL AS RITSERIAL, A.INV_NO AS RINV_NO, 
	A.DC_NO, A.RQTY AS RRQTY, B.*, SPACE(1) AS PMKEY, 'A' As Mode, A.QTY*0 AS RQTY, ABS(CASE WHEN PMKEY='+' THEN A.RQTY ELSE -A.RQTY END+ISNULL(C.RQTY,0)) AS BALQTY, 
	DTG_RQTY=A.QTY*0, A.QTY AS IN_QTY, D.RQTY as STKRSRVQTY, D.RQTY as ORGSTKRSRV, D.RDESPQTY AS RDESPQTY 
	FROM IT_SRTRN A
		INNER JOIN IT_SRSTK B ON A.ITRAN_CD = B.ITRAN_CD 
		LEFT JOIN (SELECT ITRAN_CD, SUM(RQTY) AS RQTY FROM _TMPIT_SRTRN_B GROUP BY ITRAN_CD) C ON C.ITRAN_CD=A.ITRAN_CD
		LEFT JOIN (SELECT A.ITRAN_CD, SUM(A.RQTY) - SUM(ISNULL(C.RQTY,0)) AS RQTY, SUM(ISNULL(C.RQTY,0)) AS RDESPQTY 
			FROM STKRESRVSRTRN A
				LEFT JOIN IT_SRTRN C ON A.ENTRY_TY=C.RENTRY_TY AND A.TRAN_CD=C.RTRAN_CD AND A.ITSERIAL=C.RITSERIAL AND A.ITRAN_CD=C.ITRAN_CD
			GROUP BY A.ITRAN_CD) D ON A.ITRAN_CD=D.ITRAN_CD
		WHERE A.ENTRY_TY=@RENTRY_TY AND A.TRAN_CD=@ITREF_TRAN AND A.ITSERIAL=@RITSERIAL AND 
			A.IT_CODE=@ITCODE

--SELECT A.RENTRY_TY AS ENTRY_TY, A.RDATE AS DATE, A.RTRAN_CD AS TRAN_CD, A.RINV_NO AS INV_NO, A.RITSERIAL AS ITSERIAL, 
--	A.ENTRY_TY AS RENTRY_TY, A.DATE AS RDATE, A.TRAN_CD AS RTRAN_CD, A.ITSERIAL AS RITSERIAL, A.INV_NO AS RINV_NO, 
--	A.DC_NO, A.RQTY AS RRQTY, B.* INTO #_TMPIT_SRTRN_A
--	FROM IT_SRTRN A
--		INNER JOIN IT_SRSTK B ON A.ITRAN_CD = B.ITRAN_CD 
--		WHERE A.ENTRY_TY=@RENTRY_TY AND A.TRAN_CD=@ITREF_TRAN AND A.ITSERIAL=@RITSERIAL AND 
--			A.IT_CODE=@ITCODE
--
--
--SELECT A.RENTRY_TY, A.RTRAN_CD, A.RINV_NO, A.RITSERIAL, A.ITRAN_CD, SUM(ISNULL(A.RQTY,0)) AS RQTY INTO #_TMPIT_SRTRN_B
--		FROM IT_SRTRN A 
--			WHERE A.RENTRY_TY=@RENTRY_TY AND A.RTRAN_CD=@ITREF_TRAN AND A.RITSERIAL=@RITSERIAL 
--			AND NOT (A.ENTRY_TY=@ENTRY_TY AND A.TRAN_CD=@TRAN_CD) AND A.ENTRY_TY!='IR'	
--			GROUP BY A.RENTRY_TY, A.RTRAN_CD, A.RINV_NO, A.RITSERIAL, A.ITRAN_CD
--
----**** Added By Sachin N. S. on 07/03/2014 for Bug-21381 -- Start
--SELECT A.ITRAN_CD, SUM(A.RQTY) - SUM(ISNULL(C.RQTY,0)) AS RQTY, 
--	SUM(ISNULL(C.RQTY,0)) AS RDESPQTY INTO #_TMPIT_STKRSRVSRTRN
--	FROM STKRESRVSRTRN A
--		INNER JOIN #_TMPIT_SRTRN_A B ON A.ITRAN_CD=B.ITRAN_CD 
--		LEFT JOIN IT_SRTRN C ON A.ENTRY_TY=C.RENTRY_TY AND A.TRAN_CD=C.RTRAN_CD AND A.ITSERIAL=C.RITSERIAL AND A.ITRAN_CD=C.ITRAN_CD
--	GROUP BY A.ITRAN_CD
--
----**** Added By Sachin N. S. on 07/03/2014 for Bug-21381 -- End
--
--
--SELECT A.*, SPACE(1) AS PMKEY, 'A' As Mode, A.QTY*0 AS RQTY, A.RRQTY-ISNULL(B.RQTY,0) AS BALQTY, DTG_RQTY=A.QTY*0, A.QTY AS IN_QTY, 
--	D.RQTY as STKRSRVQTY, D.RQTY as ORGSTKRSRV, D.RDESPQTY AS RDESPQTY		-- Changed By Sachin N. S. on 07/03/2014 for Bug-21381 
--	FROM #_TMPIT_SRTRN_A A
--		LEFT JOIN #_TMPIT_SRTRN_B B ON A.RENTRY_TY=B.RENTRY_TY AND A.RTRAN_CD=B.RTRAN_CD AND 
--			A.RINV_NO=B.RINV_NO AND A.RITSERIAL=B.RITSERIAL AND A.ITRAN_CD=B.ITRAN_CD	
--		LEFT JOIN #_TMPIT_STKRSRVSRTRN D ON A.ITRAN_CD=D.ITRAN_CD	-- Added By Sachin N. S. on 07/03/2014 for Bug-21381 
--	ORDER BY A.RDATE,A.ITRAN_CD
--
----SELECT A.*, SPACE(1) AS PMKEY, 'A' As Mode, A.QTY*0 AS RQTY, A.RRQTY-ISNULL(B.RQTY,0) AS BALQTY, DTG_RQTY=A.QTY*0, A.QTY AS IN_QTY		-- Changed By Sachin N. S. on 07/03/2014 for Bug-21381 
--
--
--
------ Added by Sachin N. S. on 12/06/2014 for Bug-22228 -- End
GO
