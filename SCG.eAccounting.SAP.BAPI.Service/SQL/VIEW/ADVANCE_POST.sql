USE [eAccounting]
GO
/****** Object:  View [dbo].[ADVANCE_POST]    Script Date: 04/20/2009 16:34:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ADVANCE_POST]
AS
SELECT     dbo.[Document].DocumentID, dbo.[Document].DocumentNo, dbo.AvAdvanceDocument.AdvanceType, dbo.DbCompany.CompanyCode AS COMP_CODE, 
                      CONVERT(VARCHAR, dbo.[Document].DocumentDate, 112) AS DOC_DATE, CONVERT(VARCHAR, dbo.[Document].PostingDate, 112) AS PostingDate, 
                      CONVERT(VARCHAR, dbo.[Document].BaseLineDate, 112) AS BaseLineDate, dbo.[Document].BankAccount, CONVERT(VARCHAR, 
                      dbo.AvAdvanceDocument.RequestDateOfRemittance, 112) AS DueDate, dbo.[Document].BranchCode, dbo.DbPB.PBCode, dbo.[Document].RequesterID, 
                      dbo.[Document].ReceiverID, dbo.[Document].CreatorID, dbo.[Document].ApproverID, dbo.[Document].Subject AS Description, 
                      dbo.DbPaymentMethod.PaymentMethodCode AS PaymentMethod, dbo.AvAdvanceItem.PaymentType, ISNULL(dbo.AvAdvanceDocument.Amount, 0) 
                      AS Amount
FROM         dbo.[Document] INNER JOIN
                      dbo.AvAdvanceDocument ON dbo.[Document].DocumentID = dbo.AvAdvanceDocument.DocumentID INNER JOIN
                      dbo.AvAdvanceItem ON dbo.AvAdvanceDocument.AdvanceID = dbo.AvAdvanceItem.AdvanceID LEFT OUTER JOIN
                      dbo.DbPB ON dbo.AvAdvanceDocument.PBID = dbo.DbPB.PBID LEFT OUTER JOIN
                      dbo.DbCompany ON dbo.[Document].CompanyID = dbo.DbCompany.CompanyID LEFT OUTER JOIN
                      dbo.DbPaymentMethod ON dbo.[Document].PaymentMethodID = dbo.DbPaymentMethod.PaymentMethodID

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[44] 4[8] 2[12] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
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
         Top = -288
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Document"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 226
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "AvAdvanceDocument"
            Begin Extent = 
               Top = 0
               Left = 267
               Bottom = 288
               Right = 477
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AvAdvanceItem"
            Begin Extent = 
               Top = 0
               Left = 769
               Bottom = 304
               Right = 925
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPB"
            Begin Extent = 
               Top = 133
               Left = 508
               Bottom = 402
               Right = 660
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 289
               Left = 238
               Bottom = 404
               Right = 449
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPaymentMethod"
            Begin Extent = 
               Top = 411
               Left = 239
               Bottom = 526
               Right = 425
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
      Begin ColumnWidths = 21
         Width = 284
         Width = 1500
  ' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'ADVANCE_POST'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'       Width = 2745
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
         Width = 2520
         Width = 2715
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 6615
         Alias = 3600
         Table = 3105
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'ADVANCE_POST'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'ADVANCE_POST'
