INSERT INTO DbInternalOrder
(
	IONumber,
	IOType,
	IOText,
	CostCenterID,
	CostCenterCode,
	CompanyID,
	CompanyCode,
	EffectiveDate,
	ExpireDate,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)

(
SELECT 
IO_Number,
IO_Type,
IO_Text,
(	
	SELECT ISNULL(DbCostCenter.CostCenterID,'') FROM DbCostCenter 
	WHERE  IMPORT.dbo.dbo_MS_IO.CostCenter = DbCostCenter.CostCenterCode
) AS Petty1,
CostCenter,
(	
	SELECT ISNULL(DbCompany.CompanyID,'') FROM DbCompany 
	WHERE  IMPORT.dbo.dbo_MS_IO.ComCode = DbCompany.CompanyCode
) AS Petty2,
ComCode,
EffectDate,
ExpireDate,
'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_IO
)