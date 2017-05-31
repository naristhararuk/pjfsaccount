SELECT TOP(3)     ID, UserID, Password, ChangeDate, Active, CreBy, CreDate, UpdBy, UpdDate, UpdPgm, RowVersion
FROM         SuPasswordHistory
WHERE     (UserID = '29')
ORDER BY ChangeDate DESC