INSERT INTO DbCostCenter
(
	CompanyID,
	CompanyCode,
	CostCenterCode,
	Valid,
	Expire,
	Description,
	ActualPrimaryCosts,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)

(
	SELECT
	(	
		SELECT ISNULL(DbCompany.CompanyID,'') FROM DbCompany 
		WHERE  IMPORT.dbo.dbo_MS_Cost_Center.ComCode = DbCompany.CompanyCode
	) AS Petty1,
 
ComCode,
CostCenter,
ValidFromDate,
ExpireDate,
Description,
'TRUE' as ActualPrimaryCosts,
'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_Cost_Center
WHERE IMPORT.dbo.dbo_MS_Cost_Center.CostCenter<>''
)