INSERT INTO DbCompanyPaymentMethod
(
	CompanyID,
	CompanyCode,
	PaymentMethodID,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)
(
SELECT 
	DbCompany.CompanyID,
	DbCompany.CompanyCode,
	DbPaymentMethod.PaymentMethodID,

	'true'			AS Active ,
	1				AS aa ,
	getdate()		as date1,
	1				AS aa,
	getdate()		as date2,
	'KLA'			AS UpdPgm
FROM 
	IMPORT.dbo.dbo_Payment_Method_By_Com,
	DbCompany,
	DbPaymentMethod
WHERE
	IMPORT.dbo.dbo_Payment_Method_By_Com.ComID = DbCompany.CompanyCode AND
	IMPORT.dbo.dbo_Payment_Method_By_Com.PmtID = DbPaymentMethod.PaymentMethodCode
)