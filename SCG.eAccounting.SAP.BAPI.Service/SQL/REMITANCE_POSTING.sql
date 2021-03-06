set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[REMITANCE_POSTING]
	@DOCUMENT_NO varchar(20)
AS
BEGIN

SELECT     
	[Document].DocumentTypeID									AS DocumentTypeID, 
	DocumentType.DocumentTypeName								AS DocumentTypeName, 
	[Document].DocumentID										AS RemittanceID, 
	[Document].DocumentNo										AS RemittanceNo, 
	DbCompany.CompanyCode										AS CompanyCode, 
	DbPB.PBCode													AS PBCode, 
	CONVERT(VARCHAR, [Document].CreDate,112)					AS DocDate, 
	CONVERT(VARCHAR, WorkFlowVerifyResponse.PostingDate, 112)	AS PostingDate, 
	CONVERT(VARCHAR, WorkFlowVerifyResponse.BaseLineDate, 112)	AS BaseLineDate, 
	WorkFlowVerifyResponse.PaymentMethodID						AS PaymentMethodID, 
	WorkFlowVerifyResponse.BankAccount							AS BankAccount, 
	[Document].RequesterID										AS RequesterID, 
	[Document].CreatorID										AS CreatorID, 
	[Document].ReceiverID										AS ReceiverID, 
	[Document].ApproverID										AS ApproverID, 
	[Document].Subject											AS Description, 
	AvAdvanceDocument.DocumentID								AS AdvanceID, 
	AdvanceDocument.DocumentNo									AS AdvanceNo, 
	WorkFlowVerifyResponse.BranchCode							AS BranchCode, 
	FnRemittance.Amount											AS Amount, 
	WorkFlowResponse.WorkFlowStateEventID						AS WorkFlowStateEventID
FROM         
	FnRemittance 
		INNER JOIN [Document] ON 
			FnRemittance.DocumentID = [Document].DocumentID
		INNER JOIN DocumentType ON 
			[Document].DocumentTypeID = DocumentType.DocumentTypeID 
		INNER JOIN DbCompany ON 
			[Document].CompanyID = DbCompany.CompanyID 
		INNER JOIN AvAdvanceDocument 
		INNER JOIN FnRemittanceAdvance ON 
			AvAdvanceDocument.AdvanceID = FnRemittanceAdvance.AdvanceID 
		INNER JOIN [Document] AS AdvanceDocument 
			ON AvAdvanceDocument.DocumentID = AdvanceDocument.DocumentID 
			ON FnRemittance.RemittanceID = FnRemittanceAdvance.RemittanceID 
		INNER JOIN DbPB ON 
			FnRemittance.PBID = DbPB.PBID 
		LEFT OUTER JOIN WorkFlowVerifyResponse 
		RIGHT OUTER JOIN WorkFlowResponse ON 
			WorkFlowVerifyResponse.WorkFlowResponseID = WorkFlowResponse.WorkFlowResponseID 
		RIGHT OUTER JOIN WorkFlow 
			ON WorkFlowResponse.WorkFlowID = WorkFlow.WorkFlowID 
			ON [Document].DocumentID = WorkFlow.DocumentID
WHERE     
	( (WorkFlowResponse.WorkFlowStateEventID = 18) OR (WorkFlowResponse.WorkFlowStateEventID = 21) ) AND
	[Document].DocumentNo LIKE @DOCUMENT_NO + '%'
END










