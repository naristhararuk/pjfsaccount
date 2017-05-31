INSERT INTO DbAccount
(
	ExpenseGroupID,
	AccountCode,
	SaveAsDebtor,
	DomesticRecommend,
	ForeignRecommend,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)

(
SELECT 
(	
	SELECT ISNULL(DbExpenseGroup.ExpenseGroupID,'') FROM DbExpenseGroup 
	WHERE  IMPORT.dbo.dbo_MS_Expense_Account.Group_Code = DbExpenseGroup.ExpenseGroupCode
) AS ExpenseGroupID,
Account_Code,
'TRUE' as SaveAsDebtor,
'TRUE' as DomesticRecommend,
'TRUE' as ForeignRecommend,

'TRUE' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_Expense_Account
)