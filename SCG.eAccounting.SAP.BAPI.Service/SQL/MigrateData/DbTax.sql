INSERT INTO DbTax
(TaxCode,TaxName,GL,Rate,RateNonDeduct,IsDefault,Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm)

(
SELECT 
Tax_Code,
Description,
ISNULL(GL_Account,'') AS Gl_account,
Rate_Vat,
Rate_Non_Deduct,
'true' AS Active1,
'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_Tax_Code
)