USE [eAccounting]
GO
/****** Object:  StoredProcedure [dbo].[REMITANCE_POSTING]    Script Date: 04/20/2009 16:31:42 ******/
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
	[Document].BankAccount, 
	[Document].RequesterID, 
	[Document].CreatorID, 
	[Document].ReceiverID, 
	[Document].ApproverID, 
	[Document].Subject AS Description, 
	[Document].BranchCode, 
	ISNULL(FnRemittance.TotalAmount, 0) AS Amount, 
	AvAdvanceDocument.DocumentID AS AdvanceID, 
	AdvanceDocument.DocumentNo AS AdvanceNo, 
	ISNULL(AvAdvanceDocument.Amount, 0) AS AdvanceAmount
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
	[Document].DocumentID = @DOCUMENT_ID
END
















