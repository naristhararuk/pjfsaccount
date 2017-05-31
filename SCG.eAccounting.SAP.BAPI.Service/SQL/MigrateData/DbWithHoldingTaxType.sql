INSERT INTO DbWithHoldingTaxType
(WHTTypeCode,WHTTypeName,isPeople,Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm)

(
SELECT 
ID,
Name,
'true' AS Active1,
'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_WHT_Type
)