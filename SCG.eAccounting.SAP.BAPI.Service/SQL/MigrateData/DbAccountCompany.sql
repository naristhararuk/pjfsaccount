INSERT INTO DbAccountCompany
(
	AccountID,
	CompanyID,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)
(
SELECT 
	DbAccount.AccountID,
	(	
		SELECT ISNULL(DbCompany.CompanyID,'') FROM DbCompany 
		WHERE  IMPORT.dbo.dbo_MS_Expense_Account_Com.Com_Code = DbCompany.CompanyCode
	) AS COMCODE,

	'true'			AS Active ,
	1				AS aa ,
	getdate()		as date1,
	1				AS aa,
	getdate()		as date2,
	'KLA'			AS UpdPgm
FROM 
	IMPORT.dbo.dbo_MS_Expense_Account_Com,
	DbAccount
WHERE
	IMPORT.dbo.dbo_MS_Expense_Account_Com.Account_Code = DbAccount.AccountCode
)