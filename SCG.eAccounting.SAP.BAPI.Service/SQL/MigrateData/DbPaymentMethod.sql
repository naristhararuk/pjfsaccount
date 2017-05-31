INSERT INTO DbPaymentMethod
(PaymentMethodCode,PaymentMethodName,Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm)

(
SELECT 
PmtID,
Description,
'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_Payment_Method
)