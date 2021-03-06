USE [eAccounting]
GO
/****** Object:  View [dbo].[REMITTANCE_POST]    Script Date: 04/20/2009 16:36:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[REMITTANCE_POST]
AS
SELECT     dbo.[Document].DocumentID, dbo.[Document].DocumentNo, dbo.[Document].DocumentTypeID, dbo.DocumentType.DocumentTypeName, 
                      dbo.DbCompany.CompanyCode, dbo.DbPB.PBCode, CONVERT(VARCHAR, dbo.[Document].DocumentDate, 112) AS DocDate, CONVERT(VARCHAR, 
                      dbo.[Document].PostingDate, 112) AS PostingDate, CONVERT(VARCHAR, dbo.[Document].BaseLineDate, 112) AS BaseLineDate, 
                      dbo.[Document].PaymentMethodID, dbo.[Document].BankAccount, dbo.[Document].RequesterID, dbo.[Document].CreatorID, dbo.[Document].ReceiverID, 
                      dbo.[Document].ApproverID, dbo.[Document].Subject AS Description, dbo.[Document].BranchCode, ISNULL(dbo.FnRemittance.TotalAmount, 0) 
                      AS Amount, dbo.AvAdvanceDocument.DocumentID AS AdvanceID, AdvanceDocument.DocumentNo AS AdvanceNo, 
                      ISNULL(dbo.AvAdvanceDocument.Amount, 0) AS AdvanceAmount
FROM         dbo.FnRemittance INNER JOIN
                      dbo.[Document] ON dbo.FnRemittance.DocumentID = dbo.[Document].DocumentID LEFT OUTER JOIN
                      dbo.AvAdvanceDocument INNER JOIN
                      dbo.[Document] AS AdvanceDocument ON dbo.AvAdvanceDocument.DocumentID = AdvanceDocument.DocumentID RIGHT OUTER JOIN
                      dbo.FnRemittanceAdvance ON dbo.AvAdvanceDocument.AdvanceID = dbo.FnRemittanceAdvance.AdvanceID ON 
                      dbo.FnRemittance.RemittanceID = dbo.FnRemittanceAdvance.RemittanceID LEFT OUTER JOIN
                      dbo.DbPB ON dbo.FnRemittance.PBID = dbo.DbPB.PBID LEFT OUTER JOIN
                      dbo.DbCompany ON dbo.[Document].CompanyID = dbo.DbCompany.CompanyID LEFT OUTER JOIN
                      dbo.DocumentType ON dbo.[Document].DocumentTypeID = dbo.DocumentType.DocumentTypeID

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[34] 4[4] 2[44] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[32] 4[40] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[50] 2[25] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1[74] 3) )"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
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
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = -192
      End
      Begin Tables = 
         Begin Table = "FnRemittance"
            Begin Extent = 
               Top = 7
               Left = 484
               Bottom = 265
               Right = 637
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Document"
            Begin Extent = 
               Top = 6
               Left = 230
               Bottom = 121
               Right = 400
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AvAdvanceDocument"
            Begin Extent = 
               Top = 131
               Left = 960
               Bottom = 308
               Right = 1170
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "AdvanceDocument"
            Begin Extent = 
               Top = 274
               Left = 1199
               Bottom = 389
               Right = 1369
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "FnRemittanceAdvance"
            Begin Extent = 
               Top = 4
               Left = 743
               Bottom = 248
               Right = 930
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPB"
            Begin Extent = 
               Top = 277
               Left = 709
               Bottom = 383
               Right = 861
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 92
               Left = 218
               Bottom = 343
               Right = 429
          ' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'REMITTANCE_POST'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'  End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DocumentType"
            Begin Extent = 
               Top = 345
               Left = 217
               Bottom = 601
               Right = 419
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
         Width = 1500
         Width = 1500
         Width = 2790
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2010
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 5115
         Alias = 2775
         Table = 3915
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'REMITTANCE_POST'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'REMITTANCE_POST'
