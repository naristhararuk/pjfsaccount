INSERT INTO SuUser
(
	LanguageID,
	EmployeeCode,
	CompanyID,
	CompanyCode,
	CostCenterID,
	CostCenterCode,

	UserName,
	Password,
	SetFailTime,
	FailTime,
	ChangePassword,
	PeopleID,
	EmployeeName,
	Email,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)

(
SELECT 
	1	AS LanguageID,
Employee_code,
	(	
		SELECT ISNULL(DbCompany.CompanyID,'') FROM DbCompany 
		WHERE  IMPORT.dbo.dbo_User_Profile.ComID = DbCompany.CompanyCode
	) AS Petty2,
	ComID,

	(	
		SELECT ISNULL(DbCostCenter.CostCenterID,'') FROM DbCostCenter 
		WHERE  IMPORT.dbo.dbo_User_Profile.Cost_Center = DbCostCenter.CostCenterCode
	) AS Petty1,
	Cost_Center,

	UserID,
	'21232f297a57a5a743894a0e4a801fc3'  AS Password,
	1000	AS SetFailTime,
	0		AS FailTime,
	'FALSE'	AS ChangePassword,
	Employee_code,
	UserName ,
	'test@scg.co.th' AS EMail,

'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_User_Profile
)
