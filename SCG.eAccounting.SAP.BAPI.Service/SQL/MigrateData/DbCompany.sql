INSERT INTO DbCompany
(
CompanyCode,
CompanyName,
PaymentType,
PaymentMethodPettyID,
PaymentMethodChequeID,
PaymentMethodTransferID,
AllowImportUserFromEHr,
Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm)
(
SELECT 
Company_Code,
Company_Name,
(	
	SELECT ISNULL(DbPaymentMethod.PaymentMethodID,'') FROM DbPaymentMethod 
	WHERE  IMPORT.dbo.dbo_MS_Company.Default_PayType = DbPaymentMethod.PaymentMethodCode
) AS Petty1,
(	
	SELECT ISNULL(DbPaymentMethod.PaymentMethodID,'') FROM DbPaymentMethod 
	WHERE  IMPORT.dbo.dbo_MS_Company.Default_Paymtd_Petty = DbPaymentMethod.PaymentMethodCode
) AS Petty2,
(	
	SELECT ISNULL(DbPaymentMethod.PaymentMethodID,'') FROM DbPaymentMethod 
	WHERE  IMPORT.dbo.dbo_MS_Company.Default_Paymtd_Cheque = DbPaymentMethod.PaymentMethodCode
) AS Petty3,
(	
	SELECT ISNULL(DbPaymentMethod.PaymentMethodID,'') FROM DbPaymentMethod 
	WHERE  IMPORT.dbo.dbo_MS_Company.Default_Paymtd_Tranfer = DbPaymentMethod.PaymentMethodCode
) AS Petty3,
'true' AS Active ,
'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_Company
)