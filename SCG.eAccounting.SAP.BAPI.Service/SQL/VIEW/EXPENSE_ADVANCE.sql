USE [eAccounting]
GO
/****** Object:  View [dbo].[EXPENSE_ADVANCE]    Script Date: 04/20/2009 16:35:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[EXPENSE_ADVANCE]
AS
SELECT     dbo.[Document].DocumentID AS ExpenseID, dbo.[Document].DocumentNo AS ExpenseNo, dbo.FnExpenseDocument.ExpenseType, 
                      dbo.FnExpenseDocument.PaymentType, dbo.FnExpenseDocument.TotalExpense, dbo.FnExpenseDocument.TotalAdvance, 
                      dbo.DocumentType.DocumentTypeName, dbo.DbCompany.CompanyCode, dbo.DbCompany.CompanyName, dbo.DbPB.PBCode, 
                      dbo.[Document].RequesterID, dbo.[Document].CreatorID, dbo.[Document].ReceiverID, dbo.[Document].ApproverID, 
                      dbo.[Document].Subject AS DescriptionDocument, CONVERT(VARCHAR, dbo.[Document].DocumentDate, 112) AS DocumentDate, 
                      dbo.FnExpenseAdvance.AdvanceID, DocumentAdvance.DocumentNo AS AdvanceNo, DocumentAdvance.Subject AS DescriptionAdvance, 
                      dbo.AvAdvanceDocument.Amount
FROM         dbo.[Document] INNER JOIN
                      dbo.FnExpenseDocument ON dbo.[Document].DocumentID = dbo.FnExpenseDocument.DocumentID INNER JOIN
                      dbo.FnExpenseAdvance ON dbo.FnExpenseDocument.ExpenseID = dbo.FnExpenseAdvance.ExpenseID INNER JOIN
                      dbo.AvAdvanceDocument ON dbo.FnExpenseAdvance.AdvanceID = dbo.AvAdvanceDocument.AdvanceID INNER JOIN
                      dbo.[Document] AS DocumentAdvance ON dbo.AvAdvanceDocument.DocumentID = DocumentAdvance.DocumentID LEFT OUTER JOIN
                      dbo.DbPB ON dbo.FnExpenseDocument.PBID = dbo.DbPB.PBID LEFT OUTER JOIN
                      dbo.DocumentType ON dbo.[Document].DocumentTypeID = dbo.DocumentType.DocumentTypeID LEFT OUTER JOIN
                      dbo.DbCompany ON dbo.[Document].CompanyID = dbo.DbCompany.CompanyID

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[29] 4[4] 2[49] 3) )"
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
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Document"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 277
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 232
               Left = 239
               Bottom = 347
               Right = 450
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DocumentType"
            Begin Extent = 
               Top = 374
               Left = 238
               Bottom = 489
               Right = 440
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseDocument"
            Begin Extent = 
               Top = 21
               Left = 325
               Bottom = 136
               Right = 554
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPB"
            Begin Extent = 
               Top = 169
               Left = 585
               Bottom = 284
               Right = 737
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseAdvance"
            Begin Extent = 
               Top = 7
               Left = 627
               Bottom = 122
               Right = 814
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AvAdvanceDocument"
            Begin Extent = 
               Top = 100
               Left = 843
               Bottom = 215
               Right = 1053
            E' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_ADVANCE'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'nd
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DocumentAdvance"
            Begin Extent = 
               Top = 229
               Left = 1082
               Bottom = 344
               Right = 1343
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
      Begin ColumnWidths = 11
         Width = 284
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_ADVANCE'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_ADVANCE'
