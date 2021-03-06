USE [eAccounting]
GO
/****** Object:  StoredProcedure [dbo].[ADVANCE_POSTING]    Script Date: 04/20/2009 16:31:19 ******/
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
	[Document].BankAccount, 
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















