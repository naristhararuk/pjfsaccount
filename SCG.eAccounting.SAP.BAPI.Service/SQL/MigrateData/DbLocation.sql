INSERT INTO DbLocation
(
	LocationCode,
	CompanyID,
	CompanyCode,
	Description,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)
(
SELECT 
	IMPORT.dbo.Location.Location,
	DbCompany.CompanyID,
	DbCompany.CompanyCode,
	IMPORT.dbo.Location.Description,
	'true'			AS Active ,
	1				AS aa ,
	getdate()		as date1,
	1				AS aa,
	getdate()		as date2,
	'KLA'			AS UpdPgm
FROM 
	IMPORT.dbo.Location,
	DbCompany
WHERE 
	(LEFT(IMPORT.dbo.Location.Location,3)+'0') = DbCompany.CompanyCode
)