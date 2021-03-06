USE [eAccounting]
GO
/****** Object:  StoredProcedure [dbo].[ADVANCE_POSTING]    Script Date: 04/28/2009 11:14:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ADVANCE_POSTING]
	@DOCUMENT_ID	varchar(20)
AS
BEGIN

-- ************************
-- HEADER
-- ************************
SELECT     
	[Document].DocumentID, 
	[Document].DocumentNo, 
	AvAdvanceDocument.AdvanceType, 
	DbCompany.CompanyCode AS COMP_CODE, 
	CONVERT(VARCHAR, [Document].DocumentDate, 112)						AS DOC_DATE, 
	CONVERT(VARCHAR, [Document].PostingDate, 112)						AS PostingDate, 
	CONVERT(VARCHAR, [Document].BaseLineDate, 112)						AS BaseLineDate, 
    RIGHT( REPLICATE('0',10) + ISNULL([Document].BankAccount,'') , 10 )			AS BankAccount, 
	CONVERT(VARCHAR, AvAdvanceDocument.RequestDateOfRemittance, 112)	AS DueDate, 
	[Document].BranchCode, 
	DbPB.PBCode, 
	[Document].RequesterID, 
	[Document].ReceiverID, 
	[Document].CreatorID, 
	[Document].ApproverID, 
	[Document].Subject													AS Description, 
	DbPaymentMethod.PaymentMethodCode									AS PaymentMethod, 
	AvAdvanceItem.PaymentType, 
	ISNULL(AvAdvanceDocument.Amount, 0)									AS Amount
FROM         
	[Document] 
		INNER JOIN AvAdvanceDocument ON 
			[Document].DocumentID = AvAdvanceDocument.DocumentID 
		INNER JOIN AvAdvanceItem ON 
			AvAdvanceDocument.AdvanceID = AvAdvanceItem.AdvanceID 
		LEFT OUTER JOIN DbPB ON 
			AvAdvanceDocument.PBID = DbPB.PBID 
		LEFT OUTER JOIN DbCompany ON 
			[Document].CompanyID = DbCompany.CompanyID 
		LEFT OUTER JOIN DbPaymentMethod ON 
			[Document].PaymentMethodID = DbPaymentMethod.PaymentMethodID
WHERE     
	[Document].DocumentID = @DOCUMENT_ID
END



















GO
/****** Object:  StoredProcedure [dbo].[EXPENSE_POSTING]    Script Date: 04/28/2009 11:14:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO















-- =============================================
-- Author:		KOOKKLA
-- Create date: 03/04/2009
-- Description:	Expense Posting
-- =============================================
CREATE PROCEDURE [dbo].[EXPENSE_POSTING]
	@DOCUMENT_ID varchar(20)
AS
BEGIN

-- ######################################### 
-- EXPENSE HEAD
-- #########################################
SELECT     
	[Document].DocumentID AS DocumentID, 
	[Document].DocumentNo AS DocumentNo, 
	[Document].DocumentTypeID, 
	DocumentType.DocumentTypeName, 
	FnExpenseDocument.ExpenseType, 
	FnExpenseDocument.PaymentType, 
	DbCompany.CompanyCode, 
	DbCompany.CompanyName, 
	DbPB.PBCode, 
	[Document].RequesterID, 
	[Document].CreatorID, 
	[Document].ReceiverID, 
	[Document].ApproverID, 
	ISNULL(FnExpenseDocument.TotalExpense,0) AS TotalExpense, 
	ISNULL(FnExpenseDocument.TotalAdvance,0) AS TotalAdvance, 
	[Document].Subject AS DescriptionDocument, 
	CONVERT(VARCHAR, [Document].DocumentDate, 112) AS DocumentDate, 
	FnExpenseInvoice.InvoiceID, 
	FnExpenseInvoice.InvoiceNo, 
	FnExpenseInvoice.InvoiceDocumentType, 
	CONVERT(VARCHAR, FnExpenseInvoice.InvoiceDate, 112) AS InvoiceDate, 
	FnExpenseInvoice.VendorID, 
	ISNULL(FnExpenseInvoice.TotalAmount,0)		AS TotalAmount, 
	ISNULL(FnExpenseInvoice.VatAmount,0)		AS VatAmount, 
	ISNULL(FnExpenseInvoice.WHTAmount,0)		AS WHTAmount, 
	ISNULL(FnExpenseInvoice.NetAmount,0)		AS NetAmount,
	ISNULL(DbTax.RateNonDeduct,0)				AS RateNonDeduct,
	ISNULL(FnExpenseInvoice.NonDeductAmount,0)	AS NonDeductAmount,
	ISNULL(FnExpenseInvoice.TotalBaseAmount,0)	AS TotalBaseAmount,	
	FnExpenseInvoice.Description AS DescriptionInvoice, 
	FnExpenseInvoice.isVAT, 
	FnExpenseInvoice.isWHT, 
	DbTax.TaxCode, 
	DbTax.GL AS TaxGL, 
	ISNULL(DbTax.Rate,0)					AS Rate,
 
	FnExpenseInvoice.WHTID1, 
	FnExpenseInvoice.WHTTypeID1, 
	ISNULL(FnExpenseInvoice.WHTRate1,0)		AS WHTRate1,	
	ISNULL(FnExpenseInvoice.BaseAmount1,0)	AS BaseAmount1,	
	ISNULL(FnExpenseInvoice.WHTAmount1,0)	AS WHTAmount1,	
	
	FnExpenseInvoice.WHTID2, 
	FnExpenseInvoice.WHTTypeID2,
	ISNULL(FnExpenseInvoice.WHTRate2,0)		AS WHTRate2,	
	ISNULL(FnExpenseInvoice.BaseAmount2,0)	AS BaseAmount2,	
	ISNULL(FnExpenseInvoice.WHTAmount2,0)	AS WHTAmount2,	

	DbPaymentMethod.PaymentMethodCode, 
	COALESCE (NULLIF (dbo.FnExpenseInvoice.BranchCode, ''), NULLIF (dbo.[Document].BranchCode, ''), '') AS BranchCode, 
	RIGHT( REPLICATE('0',10) + ISNULL([Document].BankAccount,'') , 10 )			AS BankAccount, 
	CONVERT(VARCHAR, [Document].PostingDate, 112)		AS PostingDate, 
	CONVERT(VARCHAR, [Document].BaseLineDate, 112)		AS BaseLineDate,
 
	COALESCE (NULLIF (FnExpenseInvoice.Description, ''),NULLIF ([Document].Subject, ''), '') AS Description
FROM         
	[Document] 
		INNER JOIN FnExpenseDocument ON 
			[Document].DocumentID = FnExpenseDocument.DocumentID 
		INNER JOIN FnExpenseInvoice ON 
			FnExpenseDocument.ExpenseID = FnExpenseInvoice.ExpenseID 
		LEFT OUTER JOIN DocumentType ON 
			[Document].DocumentTypeID = DocumentType.DocumentTypeID 
		LEFT OUTER JOIN DbCompany ON 
			[Document].CompanyID = DbCompany.CompanyID 
		LEFT OUTER JOIN DbPB ON 
			FnExpenseDocument.PBID = DbPB.PBID 
		LEFT OUTER JOIN DbPaymentMethod ON 
			[Document].PaymentMethodID = DbPaymentMethod.PaymentMethodID 
		LEFT OUTER JOIN DbTax ON 
			FnExpenseInvoice.TaxID = DbTax.TaxID
WHERE     
	[Document].DocumentID = @DOCUMENT_ID

-- ######################################### 
-- EXPENSE ITEM
-- #########################################
SELECT     
	[Document].DocumentID, 
	[Document].DocumentNo, 
	[Document].DocumentTypeID, 
	DocumentType.DocumentTypeName, 
	FnExpenseDocument.ExpenseType, 
	FnExpenseDocument.PaymentType, 
	DbCompany.CompanyCode, 
	DbCompany.CompanyName, 
	DbPB.PBCode, 
	[Document].RequesterID, 
	[Document].CreatorID, 
	[Document].ReceiverID, 
	[Document].ApproverID, 
	ISNULL(FnExpenseDocument.TotalExpense, 0)			AS TotalExpense, 
	ISNULL(FnExpenseDocument.TotalAdvance, 0)			AS TotalAdvance, 
	[Document].Subject									AS DescriptionDocument, 
	CONVERT(VARCHAR, [Document].DocumentDate, 112)		AS DocumentDate, 
	FnExpenseInvoice.InvoiceID, 
	FnExpenseInvoice.InvoiceNo, 
	FnExpenseInvoice.InvoiceDocumentType, 
	CONVERT(VARCHAR, FnExpenseInvoice.InvoiceDate, 112) AS InvoiceDate, 
	FnExpenseInvoice.VendorID, 
	ISNULL(FnExpenseInvoice.TotalAmount, 0)				AS TotalAmount, 
	ISNULL(FnExpenseInvoice.VatAmount, 0)				AS VatAmount, 
	ISNULL(FnExpenseInvoice.WHTAmount, 0)				AS WHTAmount, 
	ISNULL(FnExpenseInvoice.NetAmount, 0)				AS NetAmount, 
	ISNULL(DbTax.RateNonDeduct, 0)						AS RateNonDeduct, 
	ISNULL(FnExpenseInvoice.NonDeductAmount, 0)			AS NonDeductAmount, 
	ISNULL(FnExpenseInvoice.TotalBaseAmount, 0)			AS TotalBaseAmount, 
	FnExpenseInvoice.Description						AS DescriptionInvoice, 
	FnExpenseInvoice.isVAT, FnExpenseInvoice.isWHT, 
	DbTax.TaxCode, 
	DbTax.GL											AS TaxGL, 
	ISNULL(DbTax.Rate, 0)								AS Rate, 
	FnExpenseInvoice.WHTID1, 
	FnExpenseInvoice.WHTTypeID1, 
	ISNULL(FnExpenseInvoice.WHTRate1, 0)				AS WHTRate1, 
	ISNULL(FnExpenseInvoice.BaseAmount1, 0)				AS BaseAmount1, 
	ISNULL(FnExpenseInvoice.WHTAmount1, 0)				AS WHTAmount1, 
	FnExpenseInvoice.WHTID2, FnExpenseInvoice.WHTTypeID2, 
	ISNULL(FnExpenseInvoice.WHTRate2, 0)				AS WHTRate2, 
	ISNULL(FnExpenseInvoice.BaseAmount2, 0)				AS BaseAmount2, 
	ISNULL(FnExpenseInvoice.WHTAmount2, 0)				AS WHTAmount2, 

	DbPaymentMethod.PaymentMethodCode, 
	COALESCE (NULLIF (dbo.FnExpenseInvoice.BranchCode, ''), NULLIF (dbo.[Document].BranchCode, ''), '') AS BranchCode, 
	RIGHT( REPLICATE('0',10) + ISNULL([Document].BankAccount,'') , 10 )			AS BankAccount, 
	CONVERT(VARCHAR, [Document].PostingDate, 112)		AS PostingDate, 
	CONVERT(VARCHAR, [Document].BaseLineDate, 112)		AS BaseLineDate,

	FnExpenseInvoiceItem.InvoiceItemID, 
	DbAccount.AccountCode, 
	ISNULL(DbAccount.SAPSpecialGL, '')					AS SpecialGL, 
	DbAccount.SAPSpecialGLAssignment					AS Assignment, 
	DbInternalOrder.IONumber							AS OrderNo, 
	DbCostCenter.CostCenterCode, 
	FnExpenseInvoiceItem.SaleOrder, 
	FnExpenseInvoiceItem.SaleItem, 
	ISNULL(FnExpenseInvoiceItem.ExchangeRate, 0)		AS ExchangeRate, 
	FnExpenseInvoiceItem.Description					AS DescriptionInvoiceItem, 
	ISNULL(FnExpenseInvoiceItem.Amount, 0)				AS AmountItem, 
	ISNULL(FnExpenseInvoiceItem.NonDeductAmount, 0)		AS NonDeductAmountItem, 
	ISNULL(FnExpenseInvoiceItem.Amount, 0) + ISNULL(FnExpenseInvoiceItem.NonDeductAmount, 0)	AS TotalBaseAmountItem, 
	COALESCE (NULLIF (FnExpenseInvoiceItem.Description, ''), NULLIF (FnExpenseInvoice.Description, ''), NULLIF ([Document].Subject, ''), '') AS Description
	
FROM         
	[Document] 
		INNER JOIN FnExpenseDocument ON 
			[Document].DocumentID = FnExpenseDocument.DocumentID 
		INNER JOIN FnExpenseInvoice ON 
			FnExpenseDocument.ExpenseID = FnExpenseInvoice.ExpenseID 
		INNER JOIN FnExpenseInvoiceItem ON 
			FnExpenseInvoice.InvoiceID = FnExpenseInvoiceItem.InvoiceID 
		LEFT OUTER JOIN DbAccount ON 
			FnExpenseInvoiceItem.AccountID = DbAccount.AccountID 
		LEFT OUTER JOIN DbInternalOrder ON 
			FnExpenseInvoiceItem.IOID = DbInternalOrder.IOID 
		LEFT OUTER JOIN DbCostCenter ON 
			FnExpenseInvoiceItem.CostCenterID = DbCostCenter.CostCenterID 
		LEFT OUTER JOIN DbPB ON 
			FnExpenseDocument.PBID = DbPB.PBID 
		LEFT OUTER JOIN DbPaymentMethod ON 
			[Document].PaymentMethodID = DbPaymentMethod.PaymentMethodID 
		LEFT OUTER JOIN DbCompany ON 
			[Document].CompanyID = DbCompany.CompanyID 
		LEFT OUTER JOIN DocumentType ON 
			[Document].DocumentTypeID = DocumentType.DocumentTypeID 
		LEFT OUTER JOIN DbTax ON 
			FnExpenseInvoice.TaxID = DbTax.TaxID
WHERE     
	[Document].DocumentID = @DOCUMENT_ID

-- ######################################### 
-- EXPENSE ADVANCE
-- #########################################
SELECT     
	[Document].DocumentID AS DocumentID, 
	[Document].DocumentNo AS DocumentNo, 
	FnExpenseDocument.ExpenseType, 
	FnExpenseDocument.PaymentType, 
	FnExpenseDocument.TotalExpense, 
	FnExpenseDocument.TotalAdvance, 
	DocumentType.DocumentTypeName, 
	DbCompany.CompanyCode, 
	DbCompany.CompanyName, 
	DbPB.PBCode, 
	[Document].RequesterID, 
	[Document].CreatorID, 
	[Document].ReceiverID, 
	[Document].ApproverID, 
	[Document].Subject AS DescriptionDocument, 
	CONVERT(VARCHAR, [Document].DocumentDate, 112) AS DocumentDate, 
	FnExpenseAdvance.AdvanceID, 
	DocumentAdvance.DocumentNo AS AdvanceNo, 
	DocumentAdvance.Subject AS DescriptionAdvance,
	AvAdvanceDocument.Amount,
	AvAdvanceDocument.RemittanceAmount, 
	AvAdvanceDocument.ExpenseAmount
FROM         
	[Document] 
		INNER JOIN FnExpenseDocument ON 
			[Document].DocumentID = FnExpenseDocument.DocumentID 
		INNER JOIN FnExpenseAdvance ON 
			FnExpenseDocument.ExpenseID = FnExpenseAdvance.ExpenseID 
		INNER JOIN AvAdvanceDocument ON 
			FnExpenseAdvance.AdvanceID = AvAdvanceDocument.AdvanceID 
		INNER JOIN [Document] AS DocumentAdvance ON 
			AvAdvanceDocument.DocumentID = DocumentAdvance.DocumentID 
		LEFT OUTER JOIN DbPB ON 
			FnExpenseDocument.PBID = DbPB.PBID 
		LEFT OUTER JOIN DocumentType ON 
			[Document].DocumentTypeID = DocumentType.DocumentTypeID 
		LEFT OUTER JOIN DbCompany ON 
			[Document].CompanyID = DbCompany.CompanyID
WHERE 
	(dbo.AvAdvanceDocument.ExpenseAmount > 0) AND    
	[Document].DocumentID = @DOCUMENT_ID

END




















GO
/****** Object:  StoredProcedure [dbo].[EXPENSE_REMITTANCE_POSTING]    Script Date: 04/28/2009 11:14:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EXPENSE_REMITTANCE_POSTING]
	@DOCUMENT_ID varchar(20)
AS
BEGIN

SELECT     
	[Document].DocumentID, 
	[Document].DocumentNo, 
	[Document].DocumentTypeID, 
	DocumentType.DocumentTypeName, 
	FnExpenseDocument.ExpenseType, 
	FnExpenseDocument.PaymentType, 
	DbCompany.CompanyCode, 
	DbCompany.CompanyName, 
	DbPB.PBCode, 
	[Document].RequesterID, 
	[Document].CreatorID, 
	[Document].ReceiverID, 
	[Document].ApproverID, 
	[Document].BranchCode,
	[Document].Subject													AS Description, 
	ISNULL(FnExpenseDocument.TotalExpense, 0)							AS TotalExpense, 
	ISNULL(FnExpenseDocument.TotalAdvance, 0)							AS TotalAdvance, 
	ISNULL(FnExpenseDocument.DifferenceAmount, 0)						AS DifferenceAmount, 
	ISNULL(FnExpenseDocument.TotalRemittance, 0) 						AS TotalRemittance, 
	[Document].Subject													AS DescriptionDocument, 
	CONVERT(VARCHAR, [Document].DocumentDate, 112)						AS DocumentDate, 
	DbPaymentMethod.PaymentMethodCode, 
	RIGHT(REPLICATE('0', 10) + ISNULL([Document].BankAccount, ''), 10)	AS BankAccount, 
	CONVERT(VARCHAR, [Document].PostingDate, 112)						AS PostingDate, 
	CONVERT(VARCHAR, [Document].BaseLineDate, 112)						AS BaseLineDate, 
	AdvanceDocument.DocumentNo											AS AdvanceNo, 
	ISNULL(AvAdvanceDocument.RemittanceAmount, 0)						AS RemittanceAmount, 
	ISNULL(AvAdvanceDocument.ExpenseAmount, 0)							AS ExpenseAmount
FROM         
	[Document] 
		INNER JOIN FnExpenseDocument ON 
			[Document].DocumentID = FnExpenseDocument.DocumentID 
		INNER JOIN FnExpenseAdvance ON 
			FnExpenseDocument.ExpenseID = FnExpenseAdvance.ExpenseID 
		INNER JOIN AvAdvanceDocument ON 
			FnExpenseAdvance.AdvanceID = AvAdvanceDocument.AdvanceID 
		INNER JOIN [Document] AS AdvanceDocument ON 
			AvAdvanceDocument.DocumentID = AdvanceDocument.DocumentID	
		LEFT OUTER JOIN DbCompany ON 
			[Document].CompanyID = DbCompany.CompanyID 
		LEFT OUTER JOIN DocumentType ON 
			[Document].DocumentTypeID = DocumentType.DocumentTypeID 
		LEFT OUTER JOIN DbPB ON 
			FnExpenseDocument.PBID = DbPB.PBID 
		LEFT OUTER JOIN DbPaymentMethod ON 
			[Document].PaymentMethodID = DbPaymentMethod.PaymentMethodID
WHERE     
	(ISNULL(AvAdvanceDocument.RemittanceAmount, 0) > 0) AND
	[Document].DocumentID = @DOCUMENT_ID
END






















GO
/****** Object:  StoredProcedure [dbo].[REMITANCE_POSTING]    Script Date: 04/28/2009 11:14:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[REMITANCE_POSTING]
	@DOCUMENT_ID varchar(20)
AS
BEGIN

SELECT     
	[Document].DocumentID, 
	[Document].DocumentNo, 
	[Document].DocumentTypeID, 
	DocumentType.DocumentTypeName, 
	DbCompany.CompanyCode, 
	DbPB.PBCode, 
	CONVERT(VARCHAR, [Document].DocumentDate, 112) AS DocDate, 
	CONVERT(VARCHAR, [Document].PostingDate, 112) AS PostingDate, 
	CONVERT(VARCHAR, [Document].BaseLineDate, 112) AS BaseLineDate, 
	[Document].PaymentMethodID, 
	RIGHT( REPLICATE('0',10) + ISNULL([Document].BankAccount,'') , 10 )			AS BankAccount, 
	[Document].RequesterID, 
	[Document].CreatorID, 
	[Document].ReceiverID, 
	[Document].ApproverID, 
	[Document].Subject AS Description, 
	[Document].BranchCode, 
	ISNULL(FnRemittance.TotalAmount, 0) AS Amount, 
	AvAdvanceDocument.DocumentID AS AdvanceID, 
	AdvanceDocument.DocumentNo AS AdvanceNo, 
	ISNULL(AvAdvanceDocument.Amount, 0) AS AdvanceAmount,
	ISNULL(dbo.AvAdvanceDocument.RemittanceAmount, 0) AS RemittanceAmount, 
	ISNULL(dbo.AvAdvanceDocument.ExpenseAmount, 0) AS ExpenseAmount

FROM         
	FnRemittance INNER JOIN
	[Document] ON FnRemittance.DocumentID = [Document].DocumentID LEFT OUTER JOIN
	AvAdvanceDocument INNER JOIN
	[Document] AS AdvanceDocument ON AvAdvanceDocument.DocumentID = AdvanceDocument.DocumentID RIGHT OUTER JOIN
	FnRemittanceAdvance ON AvAdvanceDocument.AdvanceID = FnRemittanceAdvance.AdvanceID ON 
	FnRemittance.RemittanceID = FnRemittanceAdvance.RemittanceID LEFT OUTER JOIN
	DbPB ON FnRemittance.PBID = DbPB.PBID LEFT OUTER JOIN
	DbCompany ON [Document].CompanyID = DbCompany.CompanyID LEFT OUTER JOIN
	DocumentType ON [Document].DocumentTypeID = DocumentType.DocumentTypeID
WHERE     
	(ISNULL(dbo.AvAdvanceDocument.RemittanceAmount, 0) > 0) AND
	[Document].DocumentID = @DOCUMENT_ID
END





















GO
/****** Object:  StoredProcedure [dbo].[VIEW_POSTING]    Script Date: 04/28/2009 11:14:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
















-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[VIEW_POSTING]
	@DOCUMENT_ID varchar(20),
	@DOCUMENT_KIND varchar(50)
AS
BEGIN

-- #####################################
-- Head SEQ = M
-- #####################################
SELECT     BAPIACHE09.DOC_KIND, BAPIACHE09.DOC_ID, BAPIACHE09.DOC_SEQ, BAPIACHE09.FI_DOC, RIGHT(BAPIACHE09.DOC_DATE, 2)
                       + '.' + SUBSTRING(BAPIACHE09.DOC_DATE, 5, 2) + '.' + LEFT(BAPIACHE09.DOC_DATE, 4) AS DOC_DATE, 
                      RIGHT(BAPIACHE09.PSTNG_DATE, 2) + '.' + SUBSTRING(BAPIACHE09.PSTNG_DATE, 5, 2) + '.' + LEFT(BAPIACHE09.PSTNG_DATE, 4) 
                      AS POST_DATE, BAPIACHE09.DOC_TYPE, BAPIACHE09.COMP_CODE, DbCompany.CompanyName, 
                      SUBSTRING(BAPIACHE09.DOC_DATE, 5, 2) AS PERIOD, LEFT(BAPIACHE09.DOC_DATE, 4) AS YEAR, BAPIACEXTC.FIELD2 AS BRNCH, 
                      'THB' AS Currency, BAPIACHE09.REF_DOC_NO, dbo.BAPIACHE09.HEADER_TXT AS HEADERTXT
FROM         BAPIACHE09 INNER JOIN
                      BAPIACEXTC ON BAPIACHE09.DOC_ID = BAPIACEXTC.DOC_ID AND BAPIACHE09.DOC_SEQ = BAPIACEXTC.DOC_SEQ AND 
                      BAPIACHE09.DOC_KIND = BAPIACEXTC.DOC_KIND INNER JOIN
                      DbCompany ON BAPIACHE09.COMP_CODE = DbCompany.CompanyCode
WHERE     
	(BAPIACEXTC.FIELD1 = N'BRNCH') AND 
	BAPIACHE09.DOC_SEQ = 'M' AND 
	BAPIACHE09.DOC_ID = @DOCUMENT_ID AND
	BAPIACHE09.DOC_KIND = @DOCUMENT_KIND

-- #####################################
-- Head SEQ = NOT M
-- #####################################
SELECT     BAPIACHE09.DOC_KIND, BAPIACHE09.DOC_ID, BAPIACHE09.DOC_SEQ, BAPIACHE09.FI_DOC, RIGHT(BAPIACHE09.DOC_DATE, 2)
                       + '.' + SUBSTRING(BAPIACHE09.DOC_DATE, 5, 2) + '.' + LEFT(BAPIACHE09.DOC_DATE, 4) AS DOC_DATE, 
                      RIGHT(BAPIACHE09.PSTNG_DATE, 2) + '.' + SUBSTRING(BAPIACHE09.PSTNG_DATE, 5, 2) + '.' + LEFT(BAPIACHE09.PSTNG_DATE, 4) 
                      AS POST_DATE, BAPIACHE09.DOC_TYPE, BAPIACHE09.COMP_CODE, DbCompany.CompanyName, 
                      SUBSTRING(BAPIACHE09.DOC_DATE, 5, 2) AS PERIOD, LEFT(BAPIACHE09.DOC_DATE, 4) AS YEAR, BAPIACEXTC.FIELD2 AS BRNCH, 
                      'THB' AS Currency, BAPIACHE09.REF_DOC_NO, dbo.BAPIACHE09.HEADER_TXT AS HEADERTXT
FROM         BAPIACHE09 INNER JOIN
                      BAPIACEXTC ON BAPIACHE09.DOC_ID = BAPIACEXTC.DOC_ID AND BAPIACHE09.DOC_SEQ = BAPIACEXTC.DOC_SEQ AND 
                      BAPIACHE09.DOC_KIND = BAPIACEXTC.DOC_KIND INNER JOIN
                      DbCompany ON BAPIACHE09.COMP_CODE = DbCompany.CompanyCode
WHERE     
	(BAPIACEXTC.FIELD1 = N'BRNCH') AND 
	BAPIACHE09.DOC_SEQ <> 'M' AND 
	BAPIACHE09.DOC_ID = @DOCUMENT_ID AND
	BAPIACHE09.DOC_KIND = @DOCUMENT_KIND

-- #####################################
-- Detail
-- #####################################
SELECT * 
INTO #VIEW_TEMP
FROM 
(

(
SELECT     BAPIACAP09.DOC_KIND, BAPIACAP09.DOC_ID, BAPIACAP09.DOC_SEQ, BAPIACAP09.ITEMNO_ACC, 
                      BAPIACAP09.VENDOR_NO AS Account, ISNULL(BAPIACAP09.TAX_CODE, N'NV') AS TAX_CODE, RIGHT(BAPIACAP09.BLINE_DATE, 2) 
                      + '/' + SUBSTRING(BAPIACAP09.BLINE_DATE, 3, 2) + '/' + LEFT(BAPIACAP09.BLINE_DATE, 4) AS BaseDate, BAPIACAP09.PMNTTRMS, 
                      BAPIACAP09.PYMT_METH, BAPIACCR09.CURRENCY, CONVERT(VARCHAR,CAST(dbo.BAPIACCR09.AMT_DOCCUR AS MONEY),1) AS AMT_DOCCUR , '' AS CostCenter, '' AS InterOrder, 
                      CASE WHEN BAPIACAP09.VENDOR_NO = 'PCADVCL' THEN ISNULL
                          ((SELECT     FIELD3
                              FROM         BAPIACEXTC
                              WHERE     DOC_ID = BAPIACAP09.DOC_ID AND DOC_SEQ = BAPIACAP09.DOC_SEQ AND FIELD1 = 'WTH1'), '') ELSE '' END AS WHTCode1, 
                      CASE WHEN BAPIACAP09.VENDOR_NO = 'PCADVCL' THEN ISNULL
                          ((SELECT     FIELD4
                              FROM         BAPIACEXTC
                              WHERE     DOC_ID = BAPIACAP09.DOC_ID AND DOC_SEQ = BAPIACAP09.DOC_SEQ AND FIELD1 = 'WTH1'), '') ELSE '' END AS WHTBase1, 
                      CASE WHEN BAPIACAP09.VENDOR_NO = 'PCADVCL' THEN ISNULL
                          ((SELECT     FIELD3
                              FROM         BAPIACEXTC
                              WHERE     DOC_ID = BAPIACAP09.DOC_ID AND DOC_SEQ = BAPIACAP09.DOC_SEQ AND FIELD1 = 'WTH2'), '') ELSE '' END AS WHTCode2, 
                      CASE WHEN BAPIACAP09.VENDOR_NO = 'PCADVCL' THEN ISNULL
                          ((SELECT     FIELD4
                              FROM         BAPIACEXTC
                              WHERE     DOC_ID = BAPIACAP09.DOC_ID AND DOC_SEQ = BAPIACAP09.DOC_SEQ AND FIELD1 = 'WTH2'), '') ELSE '' END AS WHTBase2, 
                      BAPIACAP09.ALLOC_NMBR, BAPIACAP09.ITEM_TEXT, CASE WHEN ISNULL(BAPIACAP09.SP_GL_IND, '') = '' AND 
                      BAPIACCR09.AMT_DOCCUR >= 0 THEN '21' WHEN ISNULL(BAPIACAP09.SP_GL_IND, '') = '' AND 
                      BAPIACCR09.AMT_DOCCUR < 0 THEN '31' WHEN ISNULL(BAPIACAP09.SP_GL_IND, '') <> '' AND 
                      BAPIACCR09.AMT_DOCCUR >= 0 THEN '29E' WHEN ISNULL(BAPIACAP09.SP_GL_IND, '') <> '' AND 
                      BAPIACCR09.AMT_DOCCUR < 0 THEN '39E' END AS PK
FROM         BAPIACAP09 INNER JOIN
                      BAPIACCR09 ON BAPIACAP09.DOC_ID = BAPIACCR09.DOC_ID AND BAPIACAP09.DOC_SEQ = BAPIACCR09.DOC_SEQ AND 
                      BAPIACAP09.ITEMNO_ACC = BAPIACCR09.ITEMNO_ACC AND BAPIACAP09.DOC_KIND = BAPIACCR09.DOC_KIND
)
UNION
(
SELECT     BAPIACAR09.DOC_KIND, BAPIACAR09.DOC_ID, BAPIACAR09.DOC_SEQ, BAPIACAR09.ITEMNO_ACC, 
                      BAPIACAR09.CUSTOMER AS Account, BAPIACAR09.TAX_CODE, BAPIACAR09.BLINE_DATE AS BaseDate, '' AS PMNTTRMS, 
                      '' AS PYMT_METH, '' AS CURRENCY, CONVERT(VARCHAR,CAST(dbo.BAPIACCR09.AMT_DOCCUR AS MONEY),1) AS AMT_DOCCUR, '' AS CostCenter, '' AS InterOrder, '' AS WHTCode1, '' AS WHTBase1, 
                      '' AS WHTCode2, '' AS WHTBase2, BAPIACAR09.ALLOC_NMBR, BAPIACAR09.ITEM_TEXT, CASE WHEN ISNULL(BAPIACAR09.SP_GL_IND, '') 
                      = '' AND BAPIACCR09.AMT_DOCCUR >= 0 THEN '01' WHEN ISNULL(BAPIACAR09.SP_GL_IND, '') = '' AND 
                      BAPIACCR09.AMT_DOCCUR < 0 THEN '11' WHEN ISNULL(BAPIACAR09.SP_GL_IND, '') <> '' AND 
                      BAPIACCR09.AMT_DOCCUR >= 0 THEN '09' + BAPIACAR09.SP_GL_IND WHEN ISNULL(BAPIACAR09.SP_GL_IND, '') <> '' AND 
                      BAPIACCR09.AMT_DOCCUR < 0 THEN '19' + BAPIACAR09.SP_GL_IND END AS PK
FROM         BAPIACAR09 INNER JOIN
                      BAPIACCR09 ON BAPIACAR09.DOC_ID = BAPIACCR09.DOC_ID AND BAPIACAR09.DOC_SEQ = BAPIACCR09.DOC_SEQ AND 
                      BAPIACAR09.ITEMNO_ACC = BAPIACCR09.ITEMNO_ACC AND BAPIACAR09.DOC_KIND = BAPIACCR09.DOC_KIND
)
UNION
(
SELECT     BAPIACGL09.DOC_KIND, BAPIACGL09.DOC_ID, BAPIACGL09.DOC_SEQ, BAPIACGL09.ITEMNO_ACC, 
                      BAPIACGL09.GL_ACCOUNT AS Account, ISNULL(BAPIACGL09.TAX_CODE, N'NV') AS TAX_CODE, '' AS BaseDate, '' AS PMNTTRMS, 
                      '' AS PYMT_METH, '' AS CURRENCY, CONVERT(VARCHAR,CAST(dbo.BAPIACCR09.AMT_DOCCUR AS MONEY),1) AS AMT_DOCCUR, BAPIACGL09.COSTCENTER, BAPIACGL09.ORDERID AS InterOrder, 
                      '' AS WHTCode1, '' AS WHTBase1, '' AS WHTCode2, '' AS WHTBase2, BAPIACGL09.ALLOC_NMBR, BAPIACGL09.ITEM_TEXT, 
                      CASE WHEN BAPIACCR09.AMT_DOCCUR >= 0 THEN '40' WHEN BAPIACCR09.AMT_DOCCUR < 0 THEN '50' END AS PK
FROM         BAPIACCR09 INNER JOIN
                      BAPIACGL09 ON BAPIACCR09.DOC_ID = BAPIACGL09.DOC_ID AND BAPIACCR09.DOC_SEQ = BAPIACGL09.DOC_SEQ AND 
                      BAPIACCR09.ITEMNO_ACC = BAPIACGL09.ITEMNO_ACC AND BAPIACCR09.DOC_KIND = BAPIACGL09.DOC_KIND
)
UNION
(
SELECT     BAPIACTX09.DOC_KIND, BAPIACTX09.DOC_ID, BAPIACTX09.DOC_SEQ, BAPIACTX09.ITEMNO_ACC, 
                      BAPIACTX09.GL_ACCOUNT AS Account, BAPIACTX09.TAX_CODE, '' AS BaseDate, '' AS PMNTTRMS, '' AS PYMT_METH, '' AS CURRENCY, 
                      CONVERT(VARCHAR,CAST(dbo.BAPIACCR09.AMT_DOCCUR AS MONEY),1) AS AMT_DOCCUR, '' AS CostCenter, '' AS InterOrder, '' AS WHTCode1, '' AS WHTBase1, '' AS WHTCode2, '' AS WHTBase2, 
                      '' AS ALLOC_NMBR, '' AS ITEM_TEXT, 
                      CASE WHEN BAPIACCR09.AMT_DOCCUR >= 0 THEN '40' WHEN BAPIACCR09.AMT_DOCCUR < 0 THEN '50' END AS PK
FROM         BAPIACTX09 INNER JOIN
                      BAPIACCR09 ON BAPIACTX09.DOC_ID = BAPIACCR09.DOC_ID AND BAPIACTX09.DOC_SEQ = BAPIACCR09.DOC_SEQ AND 
                      BAPIACTX09.ITEMNO_ACC = BAPIACCR09.ITEMNO_ACC AND BAPIACTX09.DOC_KIND = BAPIACCR09.DOC_KIND
)

) AS POSTING
WHERE DOC_ID = @DOCUMENT_ID AND DOC_KIND = @DOCUMENT_KIND
ORDER BY DOC_ID,DOC_SEQ,ITEMNO_ACC

-- #####################################
-- Detail Main
-- #####################################
SELECT * 
FROM #VIEW_TEMP 
WHERE DOC_SEQ = 'M'
ORDER BY DOC_ID,DOC_SEQ,ITEMNO_ACC
-- #####################################
-- Detail Detail
-- #####################################
SELECT * 
FROM #VIEW_TEMP 
WHERE DOC_SEQ <> 'M'
ORDER BY DOC_ID,DOC_SEQ,ITEMNO_ACC

DROP TABLE #VIEW_TEMP

END









