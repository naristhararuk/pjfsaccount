INSERT INTO DbAccountLang
(
	AccountID,
	LanguageID,
	AccountName,
	Comment,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)

(
SELECT 
(	
	SELECT DbAccount.AccountID FROM DbAccount 
	WHERE  IMPORT.dbo.dbo_MS_Expense_Account.Account_Code = DbAccount.AccountCode
and DbAccount.ExpenseGroupID = 
(
	SELECT ISNULL(DbExpenseGroup.ExpenseGroupID,'') FROM DbExpenseGroup 
	WHERE  IMPORT.dbo.dbo_MS_Expense_Account.Group_Code = DbExpenseGroup.ExpenseGroupCode
)

) AS PBLanga,

1 AS LanguageID,
Account_Name_Th AS Description,
Account_Name_Th AS Comment,
'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_Expense_Account
)


INSERT INTO DbAccountLang
(
	AccountID,
	LanguageID,
	AccountName,
	Comment,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)

(
SELECT 
(	
	SELECT DbAccount.AccountID FROM DbAccount 
	WHERE  IMPORT.dbo.dbo_MS_Expense_Account.Account_Code = DbAccount.AccountCode
and DbAccount.ExpenseGroupID = 
(
	SELECT ISNULL(DbExpenseGroup.ExpenseGroupID,'') FROM DbExpenseGroup 
	WHERE  IMPORT.dbo.dbo_MS_Expense_Account.Group_Code = DbExpenseGroup.ExpenseGroupCode
)

) AS PBLanga,

2 AS LanguageID,
Account_Name_En AS Description,
Account_Name_En AS Comment,
'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_Expense_Account
)