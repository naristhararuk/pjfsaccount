INSERT INTO DbWithHoldingTax
(WHTCode,WHTName,Rate,Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm)

(
SELECT 
WHT_Code,
Description,
Rate,
'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_WHT_Code
)