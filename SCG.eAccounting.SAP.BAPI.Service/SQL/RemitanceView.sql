USE [eAccounting]
GO
/****** Object:  View [dbo].[Remitance]    Script Date: 03/30/2009 12:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Remitance]
AS
SELECT     dbo.[Document].DocumentTypeID, dbo.DocumentType.DocumentTypeName, dbo.[Document].DocumentID AS RemittanceID, 
                      dbo.[Document].DocumentNo AS RemittanceNo, dbo.DbCompany.CompanyCode, dbo.DbPB.PBCode, CONVERT(VARCHAR, dbo.[Document].CreDate, 
                      112) AS DocDate, CONVERT(VARCHAR, dbo.WorkFlowVerifyResponse.PostingDate, 112) AS PostingDate, CONVERT(VARCHAR, 
                      dbo.WorkFlowVerifyResponse.BaseLineDate, 112) AS BaseLineDate, dbo.WorkFlowVerifyResponse.PaymentMethodID, 
                      dbo.WorkFlowVerifyResponse.BankAccount, dbo.[Document].RequesterID, dbo.[Document].CreatorID, dbo.[Document].ReceiverID, 
                      dbo.[Document].ApproverID, dbo.[Document].Subject AS Description, dbo.AvAdvanceDocument.DocumentID AS AdvanceID, 
                      AdvanceDocument.DocumentNo AS AdvanceNo, dbo.WorkFlowVerifyResponse.BranchCode, dbo.FnRemittance.Amount, 
                      dbo.WorkFlowResponse.WorkFlowStateEventID
FROM         dbo.FnRemittance INNER JOIN
                      dbo.[Document] INNER JOIN
                      dbo.DocumentType ON dbo.[Document].DocumentTypeID = dbo.DocumentType.DocumentTypeID INNER JOIN
                      dbo.DbCompany ON dbo.[Document].CompanyID = dbo.DbCompany.CompanyID ON 
                      dbo.FnRemittance.DocumentID = dbo.[Document].DocumentID INNER JOIN
                      dbo.AvAdvanceDocument INNER JOIN
                      dbo.FnRemittanceAdvance ON dbo.AvAdvanceDocument.AdvanceID = dbo.FnRemittanceAdvance.AdvanceID INNER JOIN
                      dbo.[Document] AS AdvanceDocument ON dbo.AvAdvanceDocument.DocumentID = AdvanceDocument.DocumentID ON 
                      dbo.FnRemittance.RemittanceID = dbo.FnRemittanceAdvance.RemittanceID INNER JOIN
                      dbo.DbPB ON dbo.FnRemittance.PBID = dbo.DbPB.PBID LEFT OUTER JOIN
                      dbo.WorkFlowVerifyResponse INNER JOIN
                      dbo.WorkFlowResponse ON dbo.WorkFlowVerifyResponse.WorkFlowResponseID = dbo.WorkFlowResponse.WorkFlowResponseID INNER JOIN
                      dbo.WorkFlow ON dbo.WorkFlowResponse.WorkFlowID = dbo.WorkFlow.WorkFlowID ON dbo.[Document].DocumentID = dbo.WorkFlow.DocumentID

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[6] 2[29] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[44] 4[43] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[29] 2[46] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1[81] 3) )"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4[50] 3) )"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3) )"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1[45] 4) )"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 2
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "FnRemittance"
            Begin Extent = 
               Top = 0
               Left = 216
               Bottom = 291
               Right = 369
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Document"
            Begin Extent = 
               Top = 56
               Left = 21
               Bottom = 382
               Right = 185
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DocumentType"
            Begin Extent = 
               Top = 365
               Left = 401
               Bottom = 463
               Right = 581
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 366
               Left = 215
               Bottom = 539
               Right = 389
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AvAdvanceDocument"
            Begin Extent = 
               Top = 16
               Left = 615
               Bottom = 263
               Right = 825
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnRemittanceAdvance"
            Begin Extent = 
               Top = 20
               Left = 398
               Bottom = 118
               Right = 585
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AdvanceDocument"
            Begin Extent = 
               Top = 31
               Left = 874
               Bottom = 234
               Right = 1086
      ' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'Remitance'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'      End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPB"
            Begin Extent = 
               Top = 219
               Left = 400
               Bottom = 334
               Right = 552
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlowVerifyResponse"
            Begin Extent = 
               Top = 320
               Left = 1029
               Bottom = 477
               Right = 1243
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlowResponse"
            Begin Extent = 
               Top = 286
               Left = 807
               Bottom = 428
               Right = 1000
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkFlow"
            Begin Extent = 
               Top = 266
               Left = 615
               Bottom = 393
               Right = 778
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 22
         Width = 284
         Width = 1500
         Width = 2250
         Width = 1500
         Width = 3540
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2025
         Width = 1500
         Width = 1110
         Width = 1875
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      PaneHidden = 
      Begin ColumnWidths = 11
         Column = 5565
         Alias = 2745
         Table = 4170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'Remitance'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'Remitance'
