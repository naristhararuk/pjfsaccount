set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[ADVANCE_POSTING_BACKUP]
	@DOCUMENT_NO varchar(20)
AS
BEGIN

-- ************************
-- HEADER
-- ************************
SELECT     
	[Document].DocumentNo				AS DocumentNo, 
	DbCompany.CompanyCode				AS COMP_CODE, 
	[Document].CreDate					AS DOC_DATE, 
	WorkFlowVerifyResponse.PostingDate	AS PostingDate, 
	WorkFlowVerifyResponse.BranchCode	AS BranchCode,
	AvAdvanceDocument.AdvanceType		AS AdvanceType
FROM         
	[Document] 
		INNER JOIN DbCompany ON 
			[Document].CompanyID				= DbCompany.CompanyID 
		INNER JOIN AvAdvanceDocument ON 
			[Document].DocumentID				= AvAdvanceDocument.DocumentID 
		INNER JOIN WorkFlow ON 
			[Document].DocumentID				= WorkFlow.DocumentID 
		INNER JOIN WorkFlowResponse ON 
			WorkFlow.WorkFlowID					= WorkFlowResponse.WorkFlowID 
		INNER JOIN WorkFlowVerifyResponse ON 
			WorkFlowResponse.WorkFlowResponseID = WorkFlowVerifyResponse.WorkFlowResponseID
WHERE
	[Document].DocumentNo LIKE @DOCUMENT_NO + '%'

-- ************************
-- ITEM
-- ************************
SELECT     
	[Document].DocumentNo					AS DocumentNo, 
	DbCompany.CompanyCode					AS COMP_CODE, 
	[Document].CreDate						AS DOC_DATE, 
	WorkFlowVerifyResponse.PostingDate		AS PostingDate, 
	WorkFlowVerifyResponse.BranchCode		AS BranchCode, 
	DbPB.PBCode								AS PBCode, 
	[Document].RequesterID					AS RequesterID, 
	[Document].ReceiverID					AS ReceiverID, 
	[Document].CreatorID					AS CreatorID, 
	[Document].ApproverID					AS ApproverID, 
	WorkFlowVerifyResponse.BaseLineDate		AS BaseLineDate, 
	DbPaymentMethod.PaymentMethodCode		AS PaymentMethodCode, 
	DbCurrency.Symbol						AS Currency, 
	AvAdvanceItem.Amount					AS Amount, 
	[Document].Subject						AS Description, 
	AvAdvanceDocument.DueDateOfRemittance	AS DueDate,
	AvAdvanceItem.PaymentType				AS PaymentType,
	AvAdvanceDocument.ArrivalDate			AS ArrivalDate,
	AvAdvanceDocument.AdvanceType			AS AdvanceType
FROM         
	[Document] 
		INNER JOIN DbCompany ON 
			[Document].CompanyID = DbCompany.CompanyID 
		INNER JOIN AvAdvanceDocument ON 
			[Document].DocumentID = AvAdvanceDocument.DocumentID 
		INNER JOIN WorkFlow ON 
			[Document].DocumentID = WorkFlow.DocumentID 
		INNER JOIN WorkFlowResponse ON 
			WorkFlow.WorkFlowID = WorkFlowResponse.WorkFlowID 
		INNER JOIN WorkFlowVerifyResponse ON 
			WorkFlowResponse.WorkFlowResponseID = WorkFlowVerifyResponse.WorkFlowResponseID 
		INNER JOIN AvAdvanceItem ON 
			AvAdvanceDocument.AdvanceID = AvAdvanceItem.AdvanceID 
		INNER JOIN DbPB ON 
			AvAdvanceDocument.PBID = DbPB.PBID 
		INNER JOIN DbPaymentMethod ON 
			WorkFlowVerifyResponse.PaymentMethodID = DbPaymentMethod.PaymentMethodID 
		INNER JOIN DbCurrency ON 
			AvAdvanceItem.CurrencyID = DbCurrency.CurrencyID
WHERE     
	(WorkFlowResponse.WorkFlowStateEventID = 18) OR
    (WorkFlowResponse.WorkFlowStateEventID = 21) AND
	[Document].DocumentNo LIKE @DOCUMENT_NO + '%'
END






