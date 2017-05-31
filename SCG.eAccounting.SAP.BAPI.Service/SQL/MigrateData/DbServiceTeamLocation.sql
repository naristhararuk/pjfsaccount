INSERT INTO DbServiceTeamLocation
(
	ServiceTeamID,
	LocationID,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)
(
SELECT 
	(
		SELECT
			ServiceTeamID
		FROM 
			DbServiceTeam
		WHERE 
			LEFT(DbServiceTeam.ServiceTeamCode,4) = DbLocation.CompanyCode
	) AS ServiceTeamID,
	DbLocation.LocationID ,
	'true' AS Active,
	1				AS aa ,
	getdate()		as date1,
	1				AS aa,
	getdate()		as date2,
	'KLA'			AS UpdPgm
FROM 
	DbLocation
WHERE 
	DbLocation.LocationID > 26
)