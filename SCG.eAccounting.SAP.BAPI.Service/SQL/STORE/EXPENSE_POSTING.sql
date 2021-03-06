USE [eAccounting]
GO
/****** Object:  StoredProcedure [dbo].[EXPENSE_POSTING]    Script Date: 04/20/2009 16:31:28 ******/
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
	[Document].BankAccount, 
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
	[Document].BankAccount, 
	CONVERT(VARCHAR, [Document].PostingDate, 112)		AS PostingDate, 
	CONVERT(VARCHAR, [Document].BaseLineDate, 112)		AS BaseLineDate,

	FnExpenseInvoiceItem.InvoiceItemID, 
	DbAccount.AccountCode, 
	ISNULL(DbAccount.SAPSpecialGL, '')					AS SpecialGL, 
	DbAccount.SAPSpecialGLAssignment, 
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
	AvAdvanceDocument.Amount
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
	[Document].DocumentID = @DOCUMENT_ID

END














