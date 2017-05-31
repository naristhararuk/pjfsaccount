INSERT INTO DbPB
(
PBCode,
CompanyID,
CompanyCode,
PettyCashLimit,
BlockPost,
Description,
Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm)

(
SELECT 
PB_Code,
ISNULL (
(	
	SELECT DbCompany.CompanyID FROM DbCompany 
	WHERE  IMPORT.dbo.dbo_MS_PB.Company_Code = DbCompany.CompanyCode
) , 342) AS Petty1,
ISNULL (
(	
	SELECT DbCompany.CompanyCode FROM DbCompany 
	WHERE  IMPORT.dbo.dbo_MS_PB.Company_Code = DbCompany.CompanyCode
) , 1570) AS Petty2,
5000 as PettyCashLimit,
'TRUE' as BlockPost,
Name1 + ' / ' + Name2 as Description,
'TRUE' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_PB
)