INSERT INTO DbExpenseGroup
(
	ExpenseGroupCode,
	ExpenseGroupType,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)

(
SELECT 
Group_Code,
0 AS ExpenseGroupType,
'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_Expense
)

-- //////////////// THAI
INSERT INTO DbExpenseGroupLang
(
	ExpenseGroupID,
	LanguageID,
	Description,
	Comment,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)

(
SELECT 
(	
	SELECT ISNULL(DbExpenseGroup.ExpenseGroupID,'') FROM DbExpenseGroup 
	WHERE  IMPORT.dbo.dbo_MS_Expense.Group_Code = DbExpenseGroup.ExpenseGroupCode
) AS ExpenseGroupID,

1 AS LanguageID,
Group_Name_Th AS Description,
Group_Name_Th AS Comment,
'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_Expense
)

-- //////////////// ENGLISH
INSERT INTO DbExpenseGroupLang
(
	ExpenseGroupID,
	LanguageID,
	Description,
	Comment,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)

(
SELECT 
(	
	SELECT ISNULL(DbExpenseGroup.ExpenseGroupID,'') FROM DbExpenseGroup 
	WHERE  IMPORT.dbo.dbo_MS_Expense.Group_Code = DbExpenseGroup.ExpenseGroupCode
) AS ExpenseGroupID,

2 AS LanguageID,
Group_Name_En AS Description,
Group_Name_En AS Comment,
'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_Expense
)