USE [eAccounting]
GO
/****** Object:  StoredProcedure [dbo].[EXPENSE_REMITANCE_POSTING]    Script Date: 04/20/2009 16:31:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EXPENSE_REMITANCE_POSTING]
	@DOCUMENT_ID varchar(20)
AS
BEGIN

SELECT     Document_1.DocumentID, Document_1.DocumentNo, dbo.[Document].DocumentID AS RemitanceDocumentID, 
                      dbo.[Document].DocumentNo AS RemitanceDocumentNo, dbo.[Document].DocumentTypeID, dbo.DocumentType.DocumentTypeName, 
                      dbo.DbCompany.CompanyCode, dbo.DbPB.PBCode, CONVERT(VARCHAR, dbo.[Document].DocumentDate, 112) AS DocDate, CONVERT(VARCHAR, 
                      dbo.WorkFlowVerifyResponse.PostingDate, 112) AS PostingDate, CONVERT(VARCHAR, dbo.WorkFlowVerifyResponse.BaseLineDate, 112) 
                      AS BaseLineDate, dbo.WorkFlowVerifyResponse.PaymentMethodID, dbo.WorkFlowVerifyResponse.BankAccount, dbo.[Document].RequesterID, 
                      dbo.[Document].CreatorID, dbo.[Document].ReceiverID, dbo.[Document].ApproverID, dbo.[Document].Subject AS Description, 
                      dbo.AvAdvanceDocument.DocumentID AS AdvanceID, AdvanceDocument.DocumentNo AS AdvanceNo, dbo.WorkFlowVerifyResponse.BranchCode, 
                      ISNULL(dbo.FnRemittance.TotalAmount, 0) AS AmountOld, dbo.WorkFlowResponse.WorkFlowStateEventID, 
                      ISNULL(dbo.FnExpenseDocument.DifferenceAmount, 0) AS Amount
FROM         dbo.FnRemittance INNER JOIN
                      dbo.[Document] INNER JOIN
                      dbo.DocumentType ON dbo.[Document].DocumentTypeID = dbo.DocumentType.DocumentTypeID INNER JOIN
                      dbo.DbCompany ON dbo.[Document].CompanyID = dbo.DbCompany.CompanyID ON 
                      dbo.FnRemittance.DocumentID = dbo.[Document].DocumentID INNER JOIN
                      dbo.AvAdvanceDocument INNER JOIN
                      dbo.FnRemittanceAdvance ON dbo.AvAdvanceDocument.AdvanceID = dbo.FnRemittanceAdvance.AdvanceID INNER JOIN
                      dbo.[Document] AS AdvanceDocument ON dbo.AvAdvanceDocument.DocumentID = AdvanceDocument.DocumentID ON 
                      dbo.FnRemittance.RemittanceID = dbo.FnRemittanceAdvance.RemittanceID INNER JOIN
                      dbo.DbPB ON dbo.FnRemittance.PBID = dbo.DbPB.PBID INNER JOIN
                      dbo.FnExpenseRemittance ON dbo.FnRemittance.RemittanceID = dbo.FnExpenseRemittance.RemittanceID INNER JOIN
                      dbo.FnExpenseDocument ON dbo.FnExpenseRemittance.ExpenseID = dbo.FnExpenseDocument.ExpenseID INNER JOIN
                      dbo.[Document] AS Document_1 ON dbo.FnExpenseDocument.DocumentID = Document_1.DocumentID LEFT OUTER JOIN
                      dbo.WorkFlowVerifyResponse INNER JOIN
                      dbo.WorkFlowResponse ON dbo.WorkFlowVerifyResponse.WorkFlowResponseID = dbo.WorkFlowResponse.WorkFlowResponseID INNER JOIN
                      dbo.WorkFlow ON dbo.WorkFlowResponse.WorkFlowID = dbo.WorkFlow.WorkFlowID ON dbo.[Document].DocumentID = dbo.WorkFlow.DocumentID
WHERE
	--( (WorkFlowResponse.WorkFlowStateEventID = 18) OR (WorkFlowResponse.WorkFlowStateEventID = 21) ) AND
	Document_1.DocumentID = @DOCUMENT_ID
END














