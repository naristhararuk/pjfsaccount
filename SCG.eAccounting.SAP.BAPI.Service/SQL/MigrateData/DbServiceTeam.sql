INSERT INTO DbServiceTeam
(
	ServiceTeamCode,
	Description,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)
(
SELECT 
	CompanyCode + '-BS' AS servicecode,
	'สำนักงานใหญ่บางซื่อ' AS Descri ,
	'true'			AS Active ,
	1				AS aa ,
	getdate()		as date1,
	1				AS aa,
	getdate()		as date2,
	'KLA'			AS UpdPgm
FROM 
	DbCompany
WHERE 
	DbCompany.CompanyID>272
)