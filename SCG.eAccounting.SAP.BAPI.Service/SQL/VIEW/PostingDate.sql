USE [eAccounting]
GO
/****** Object:  View [dbo].[PostingDate]    Script Date: 04/20/2009 16:35:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[PostingDate]
AS
SELECT     dbo.WorkFlowResponse.WorkFlowStateEventID, dbo.WorkFlowVerifyResponse.PostingDate, dbo.WorkFlowVerifyResponse.BranchCode
FROM         dbo.WorkFlow INNER JOIN
                      dbo.WorkFlowResponse ON dbo.WorkFlow.WorkFlowID = dbo.WorkFlowResponse.WorkFlowID INNER JOIN
                      dbo.WorkFlowVerifyResponse ON dbo.WorkFlowResponse.WorkFlowResponseID = dbo.WorkFlowVerifyResponse.WorkFlowResponseID
WHERE     (dbo.WorkFlowResponse.WorkFlowStateEventID = 18) OR
                      (dbo.WorkFlowResponse.WorkFlowStateEventID = 21)

