INSERT INTO DbVendor
(
	VendorCode,
	VendorName1,
	VendorName2,
	Street,
	City,
	District,
	Country,
	PostalCode,
	TaxNo1,
	TaxNo2,
	BlockDelete,
	BlockPost,

	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)

(
SELECT 
	
	Vendor_Code,
	Name1,
	Name2,
	Street_House_No,
	City,
	District,
	Country,
	Postal_Code,
	TaxNo1,
	TaxNo2,
	'TRUE' AS BlockDelete,
	'TRUE' AS BlockPost,
	
'true' AS Active ,
1 AS aa ,
getdate() as date1,
1 AS aa,
getdate() as date2,
'KLA' AS UpdPgm

FROM IMPORT.dbo.dbo_MS_Vendor
)
