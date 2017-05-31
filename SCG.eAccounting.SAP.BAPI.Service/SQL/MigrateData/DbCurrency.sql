INSERT INTO DbCurrency
(Symbol,Comment,UpdBy,UpdDate,CreBy,CreDate,UpdPgm,Active)

(
SELECT 
ID,
[Name],
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm,
'true' AS Active 
FROM IMPORT.dbo.Currency
)

-- ///
INSERT INTO DbCurrencyLang
(CurrencyID,LanguageID,Description,Comment,UpdBy,UpdDate,CreBy,CreDate,UpdPgm,Active)

(
SELECT 
(	
	SELECT DbCurrency.CurrencyID FROM DbCurrency 
	WHERE  IMPORT.dbo.Currency.ID = DbCurrency.Symbol
) AS Petty1,
1 AS LanguageID ,
[NAME],
[NAME],
1 as upddate,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm,
'true' AS Active 
FROM IMPORT.dbo.Currency
)

-- //
INSERT INTO DbCurrencyLang
(CurrencyID,LanguageID,Description,Comment,UpdBy,UpdDate,CreBy,CreDate,UpdPgm,Active)

(
SELECT 
(	
	SELECT DbCurrency.CurrencyID FROM DbCurrency 
	WHERE  IMPORT.dbo.Currency.ID = DbCurrency.Symbol
) AS Petty1,
2 AS LanguageID ,
[NAME],
[NAME],
1 as upddate,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm,
'true' AS Active 
FROM IMPORT.dbo.Currency
)