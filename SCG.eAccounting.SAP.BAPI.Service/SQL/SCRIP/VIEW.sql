USE [eAccounting]
GO
/****** Object:  View [dbo].[ADVANCE_POST]    Script Date: 04/28/2009 11:14:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ADVANCE_POST]
AS
SELECT     dbo.[Document].DocumentID, dbo.[Document].DocumentNo, dbo.AvAdvanceDocument.AdvanceType, dbo.DbCompany.CompanyCode AS COMP_CODE, 
                      CONVERT(VARCHAR, dbo.[Document].DocumentDate, 112) AS DOC_DATE, CONVERT(VARCHAR, dbo.[Document].PostingDate, 112) AS PostingDate, 
                      CONVERT(VARCHAR, dbo.[Document].BaseLineDate, 112) AS BaseLineDate, RIGHT(REPLICATE('0', 10) + ISNULL(dbo.[Document].BankAccount, ''), 10) 
                      AS BankAccount, CONVERT(VARCHAR, dbo.AvAdvanceDocument.RequestDateOfRemittance, 112) AS DueDate, dbo.[Document].BranchCode, 
                      dbo.DbPB.PBCode, dbo.[Document].RequesterID, dbo.[Document].ReceiverID, dbo.[Document].CreatorID, dbo.[Document].ApproverID, 
                      dbo.[Document].Subject AS Description, dbo.DbPaymentMethod.PaymentMethodCode AS PaymentMethod, dbo.AvAdvanceItem.PaymentType, 
                      ISNULL(dbo.AvAdvanceDocument.Amount, 0) AS Amount
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
         Configuration = "(H (1[41] 4[41] 2[4] 3) )"
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
               Bottom = 121
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AvAdvanceDocument"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 121
               Right = 456
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AvAdvanceItem"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 241
               Right = 194
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPB"
            Begin Extent = 
               Top = 126
               Left = 232
               Bottom = 241
               Right = 384
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 246
               Left = 38
               Bottom = 361
               Right = 249
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPaymentMethod"
            Begin Extent = 
               Top = 246
               Left = 287
               Bottom = 361
               Right = 473
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
     ' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'ADVANCE_POST'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'    Width = 1500
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
         Column = 4110
         Alias = 2310
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'ADVANCE_POST'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'ADVANCE_POST'

GO
/****** Object:  View [dbo].[EXPENSE_ADVANCE]    Script Date: 04/28/2009 11:14:30 ******/
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
                      dbo.AvAdvanceDocument.Amount, dbo.AvAdvanceDocument.RemittanceAmount, dbo.AvAdvanceDocument.ExpenseAmount
FROM         dbo.[Document] INNER JOIN
                      dbo.FnExpenseDocument ON dbo.[Document].DocumentID = dbo.FnExpenseDocument.DocumentID INNER JOIN
                      dbo.FnExpenseAdvance ON dbo.FnExpenseDocument.ExpenseID = dbo.FnExpenseAdvance.ExpenseID INNER JOIN
                      dbo.AvAdvanceDocument ON dbo.FnExpenseAdvance.AdvanceID = dbo.AvAdvanceDocument.AdvanceID INNER JOIN
                      dbo.[Document] AS DocumentAdvance ON dbo.AvAdvanceDocument.DocumentID = DocumentAdvance.DocumentID LEFT OUTER JOIN
                      dbo.DbPB ON dbo.FnExpenseDocument.PBID = dbo.DbPB.PBID LEFT OUTER JOIN
                      dbo.DocumentType ON dbo.[Document].DocumentTypeID = dbo.DocumentType.DocumentTypeID LEFT OUTER JOIN
                      dbo.DbCompany ON dbo.[Document].CompanyID = dbo.DbCompany.CompanyID
WHERE     (dbo.AvAdvanceDocument.ExpenseAmount > 0)


GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[55] 4[6] 2[20] 3) )"
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
               Bottom = 121
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseDocument"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 121
               Right = 475
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseAdvance"
            Begin Extent = 
               Top = 6
               Left = 513
               Bottom = 121
               Right = 700
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AvAdvanceDocument"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 241
               Right = 248
            End
            DisplayFlags = 280
            TopColumn = 18
         End
         Begin Table = "DocumentAdvance"
            Begin Extent = 
               Top = 126
               Left = 286
               Bottom = 241
               Right = 456
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPB"
            Begin Extent = 
               Top = 6
               Left = 738
               Bottom = 121
               Right = 890
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DocumentType"
            Begin Extent = 
               Top = 126
               Left = 494
               Bottom = 241
               Right = 696
           ' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_ADVANCE'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 246
               Left = 38
               Bottom = 361
               Right = 249
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
      Begin ColumnWidths = 23
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

GO
/****** Object:  View [dbo].[EXPENSE_HEAD]    Script Date: 04/28/2009 11:14:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[EXPENSE_HEAD]
AS
SELECT     dbo.[Document].DocumentID, dbo.[Document].DocumentNo, dbo.[Document].DocumentTypeID, dbo.DocumentType.DocumentTypeName, 
                      dbo.FnExpenseDocument.ExpenseType, dbo.FnExpenseDocument.PaymentType, dbo.DbCompany.CompanyCode, dbo.DbCompany.CompanyName, 
                      dbo.DbPB.PBCode, dbo.[Document].RequesterID, dbo.[Document].CreatorID, dbo.[Document].ReceiverID, dbo.[Document].ApproverID, 
                      ISNULL(dbo.FnExpenseDocument.TotalExpense, 0) AS TotalExpense, ISNULL(dbo.FnExpenseDocument.TotalAdvance, 0) AS TotalAdvance, 
                      dbo.[Document].Subject AS DescriptionDocument, CONVERT(VARCHAR, dbo.[Document].DocumentDate, 112) AS DocumentDate, 
                      dbo.FnExpenseInvoice.InvoiceID, dbo.FnExpenseInvoice.InvoiceNo, dbo.FnExpenseInvoice.InvoiceDocumentType, CONVERT(VARCHAR, 
                      dbo.FnExpenseInvoice.InvoiceDate, 112) AS InvoiceDate, dbo.FnExpenseInvoice.VendorID, ISNULL(dbo.FnExpenseInvoice.TotalAmount, 0) 
                      AS TotalAmount, ISNULL(dbo.FnExpenseInvoice.VatAmount, 0) AS VatAmount, ISNULL(dbo.FnExpenseInvoice.WHTAmount, 0) AS WHTAmount, 
                      ISNULL(dbo.FnExpenseInvoice.NetAmount, 0) AS NetAmount, ISNULL(dbo.DbTax.RateNonDeduct, 0) AS RateNonDeduct, 
                      ISNULL(dbo.FnExpenseInvoice.NonDeductAmount, 0) AS NonDeductAmount, ISNULL(dbo.FnExpenseInvoice.TotalBaseAmount, 0) AS TotalBaseAmount, 
                      dbo.FnExpenseInvoice.Description AS DescriptionInvoice, dbo.FnExpenseInvoice.isVAT, dbo.FnExpenseInvoice.isWHT, dbo.DbTax.TaxCode, 
                      dbo.DbTax.GL AS TaxGL, ISNULL(dbo.DbTax.Rate, 0) AS Rate, dbo.FnExpenseInvoice.WHTID1, dbo.FnExpenseInvoice.WHTTypeID1, 
                      ISNULL(dbo.FnExpenseInvoice.WHTRate1, 0) AS WHTRate1, ISNULL(dbo.FnExpenseInvoice.BaseAmount1, 0) AS BaseAmount1, 
                      ISNULL(dbo.FnExpenseInvoice.WHTAmount1, 0) AS WHTAmount1, dbo.FnExpenseInvoice.WHTID2, dbo.FnExpenseInvoice.WHTTypeID2, 
                      ISNULL(dbo.FnExpenseInvoice.WHTRate2, 0) AS WHTRate2, ISNULL(dbo.FnExpenseInvoice.BaseAmount2, 0) AS BaseAmount2, 
                      ISNULL(dbo.FnExpenseInvoice.WHTAmount2, 0) AS WHTAmount2, COALESCE (NULLIF (dbo.FnExpenseInvoice.BranchCode, ''), 
                      NULLIF (dbo.[Document].BranchCode, ''), '') AS BranchCode, dbo.DbPaymentMethod.PaymentMethodCode, RIGHT(REPLICATE('0', 10) 
                      + ISNULL(dbo.[Document].BankAccount, ''), 10) AS BankAccount, CONVERT(VARCHAR, dbo.[Document].PostingDate, 112) AS PostingDate, 
                      CONVERT(VARCHAR, dbo.[Document].BaseLineDate, 112) AS BaseLineDate, COALESCE (NULLIF (dbo.FnExpenseInvoice.Description, ''), 
                      NULLIF (dbo.[Document].Subject, ''), '') AS Description
FROM         dbo.[Document] INNER JOIN
                      dbo.FnExpenseDocument ON dbo.[Document].DocumentID = dbo.FnExpenseDocument.DocumentID INNER JOIN
                      dbo.FnExpenseInvoice ON dbo.FnExpenseDocument.ExpenseID = dbo.FnExpenseInvoice.ExpenseID LEFT OUTER JOIN
                      dbo.DocumentType ON dbo.[Document].DocumentTypeID = dbo.DocumentType.DocumentTypeID LEFT OUTER JOIN
                      dbo.DbCompany ON dbo.[Document].CompanyID = dbo.DbCompany.CompanyID LEFT OUTER JOIN
                      dbo.DbPB ON dbo.FnExpenseDocument.PBID = dbo.DbPB.PBID LEFT OUTER JOIN
                      dbo.DbPaymentMethod ON dbo.[Document].PaymentMethodID = dbo.DbPaymentMethod.PaymentMethodID LEFT OUTER JOIN
                      dbo.DbTax ON dbo.FnExpenseInvoice.TaxID = dbo.DbTax.TaxID

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[51] 4[41] 2[4] 3) )"
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
         Top = -96
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Document"
            Begin Extent = 
               Top = 102
               Left = 38
               Bottom = 217
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseDocument"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 121
               Right = 475
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseInvoice"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 241
               Right = 226
            End
            DisplayFlags = 280
            TopColumn = 37
         End
         Begin Table = "DocumentType"
            Begin Extent = 
               Top = 126
               Left = 264
               Bottom = 241
               Right = 466
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 246
               Left = 38
               Bottom = 361
               Right = 249
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPB"
            Begin Extent = 
               Top = 246
               Left = 287
               Bottom = 361
               Right = 439
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPaymentMethod"
            Begin Extent = 
               Top = 366
               Left = 38
               Bottom = 481
               Right = 224
            E' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_HEAD'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'nd
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbTax"
            Begin Extent = 
               Top = 366
               Left = 262
               Bottom = 481
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
      Begin ColumnWidths = 53
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4665
         Alias = 2955
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_HEAD'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_HEAD'

GO
/****** Object:  View [dbo].[EXPENSE_ITEM]    Script Date: 04/28/2009 11:14:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[EXPENSE_ITEM]
AS
SELECT     dbo.[Document].DocumentID, dbo.[Document].DocumentNo, dbo.[Document].DocumentTypeID, dbo.DocumentType.DocumentTypeName, 
                      dbo.FnExpenseDocument.ExpenseType, dbo.FnExpenseDocument.PaymentType, dbo.DbCompany.CompanyCode, dbo.DbCompany.CompanyName, 
                      dbo.DbPB.PBCode, dbo.[Document].RequesterID, dbo.[Document].CreatorID, dbo.[Document].ReceiverID, dbo.[Document].ApproverID, 
                      ISNULL(dbo.FnExpenseDocument.TotalExpense, 0) AS TotalExpense, ISNULL(dbo.FnExpenseDocument.TotalAdvance, 0) AS TotalAdvance, 
                      dbo.[Document].Subject AS DescriptionDocument, CONVERT(VARCHAR, dbo.[Document].DocumentDate, 112) AS DocumentDate, 
                      dbo.FnExpenseInvoice.InvoiceID, dbo.FnExpenseInvoice.InvoiceNo, dbo.FnExpenseInvoice.InvoiceDocumentType, CONVERT(VARCHAR, 
                      dbo.FnExpenseInvoice.InvoiceDate, 112) AS InvoiceDate, dbo.FnExpenseInvoice.VendorID, ISNULL(dbo.FnExpenseInvoice.TotalAmount, 0) 
                      AS TotalAmount, ISNULL(dbo.FnExpenseInvoice.VatAmount, 0) AS VatAmount, ISNULL(dbo.FnExpenseInvoice.WHTAmount, 0) AS WHTAmount, 
                      ISNULL(dbo.FnExpenseInvoice.NetAmount, 0) AS NetAmount, ISNULL(dbo.DbTax.RateNonDeduct, 0) AS RateNonDeduct, 
                      ISNULL(dbo.FnExpenseInvoice.NonDeductAmount, 0) AS NonDeductAmount, ISNULL(dbo.FnExpenseInvoice.TotalBaseAmount, 0) AS TotalBaseAmount, 
                      dbo.FnExpenseInvoice.Description AS DescriptionInvoice, dbo.FnExpenseInvoice.isVAT, dbo.FnExpenseInvoice.isWHT, dbo.DbTax.TaxCode, 
                      dbo.DbTax.GL AS TaxGL, ISNULL(dbo.DbTax.Rate, 0) AS Rate, dbo.FnExpenseInvoice.WHTID1, dbo.FnExpenseInvoice.WHTTypeID1, 
                      ISNULL(dbo.FnExpenseInvoice.WHTRate1, 0) AS WHTRate1, ISNULL(dbo.FnExpenseInvoice.BaseAmount1, 0) AS BaseAmount1, 
                      ISNULL(dbo.FnExpenseInvoice.WHTAmount1, 0) AS WHTAmount1, dbo.FnExpenseInvoice.WHTID2, dbo.FnExpenseInvoice.WHTTypeID2, 
                      ISNULL(dbo.FnExpenseInvoice.WHTRate2, 0) AS WHTRate2, ISNULL(dbo.FnExpenseInvoice.BaseAmount2, 0) AS BaseAmount2, 
                      ISNULL(dbo.FnExpenseInvoice.WHTAmount2, 0) AS WHTAmount2, dbo.DbPaymentMethod.PaymentMethodCode, 
                      dbo.FnExpenseInvoiceItem.InvoiceItemID, dbo.DbAccount.AccountCode, ISNULL(dbo.DbAccount.SAPSpecialGL, '') AS SpecialGL, 
                      dbo.DbAccount.SAPSpecialGLAssignment, dbo.DbInternalOrder.IONumber AS OrderNo, dbo.DbCostCenter.CostCenterCode, 
                      dbo.FnExpenseInvoiceItem.SaleOrder, dbo.FnExpenseInvoiceItem.SaleItem, ISNULL(dbo.FnExpenseInvoiceItem.ExchangeRate, 0) AS ExchangeRate, 
                      dbo.FnExpenseInvoiceItem.Description AS DescriptionInvoiceItem, ISNULL(dbo.FnExpenseInvoiceItem.Amount, 0) AS AmountItem, 
                      ISNULL(dbo.FnExpenseInvoiceItem.NonDeductAmount, 0) AS NonDeductAmountItem, ISNULL(dbo.FnExpenseInvoiceItem.Amount, 0) 
                      + ISNULL(dbo.FnExpenseInvoiceItem.NonDeductAmount, 0) AS TotalBaseAmountItem, COALESCE (NULLIF (dbo.FnExpenseInvoiceItem.Description, ''), 
                      NULLIF (dbo.FnExpenseInvoice.Description, ''), NULLIF (dbo.[Document].Subject, ''), '') AS Description, 
                      COALESCE (NULLIF (dbo.FnExpenseInvoice.BranchCode, ''), NULLIF (dbo.[Document].BranchCode, ''), '') AS BranchCode, RIGHT(REPLICATE('0', 10) 
                      + ISNULL(dbo.[Document].BankAccount, ''), 10) AS BankAccount, CONVERT(VARCHAR, dbo.[Document].PostingDate, 112) AS PostingDate, 
                      CONVERT(VARCHAR, dbo.[Document].BaseLineDate, 112) AS BaseLineDate
FROM         dbo.[Document] INNER JOIN
                      dbo.FnExpenseDocument ON dbo.[Document].DocumentID = dbo.FnExpenseDocument.DocumentID INNER JOIN
                      dbo.FnExpenseInvoice ON dbo.FnExpenseDocument.ExpenseID = dbo.FnExpenseInvoice.ExpenseID INNER JOIN
                      dbo.FnExpenseInvoiceItem ON dbo.FnExpenseInvoice.InvoiceID = dbo.FnExpenseInvoiceItem.InvoiceID LEFT OUTER JOIN
                      dbo.DbAccount ON dbo.FnExpenseInvoiceItem.AccountID = dbo.DbAccount.AccountID LEFT OUTER JOIN
                      dbo.DbInternalOrder ON dbo.FnExpenseInvoiceItem.IOID = dbo.DbInternalOrder.IOID LEFT OUTER JOIN
                      dbo.DbCostCenter ON dbo.FnExpenseInvoiceItem.CostCenterID = dbo.DbCostCenter.CostCenterID LEFT OUTER JOIN
                      dbo.DbPB ON dbo.FnExpenseDocument.PBID = dbo.DbPB.PBID LEFT OUTER JOIN
                      dbo.DbPaymentMethod ON dbo.[Document].PaymentMethodID = dbo.DbPaymentMethod.PaymentMethodID LEFT OUTER JOIN
                      dbo.DbCompany ON dbo.[Document].CompanyID = dbo.DbCompany.CompanyID LEFT OUTER JOIN
                      dbo.DocumentType ON dbo.[Document].DocumentTypeID = dbo.DocumentType.DocumentTypeID LEFT OUTER JOIN
                      dbo.DbTax ON dbo.FnExpenseInvoice.TaxID = dbo.DbTax.TaxID


GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[38] 2[3] 3) )"
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
               Bottom = 121
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseDocument"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 121
               Right = 475
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseInvoice"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 241
               Right = 226
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseInvoiceItem"
            Begin Extent = 
               Top = 126
               Left = 264
               Bottom = 241
               Right = 435
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbAccount"
            Begin Extent = 
               Top = 246
               Left = 38
               Bottom = 361
               Right = 238
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbInternalOrder"
            Begin Extent = 
               Top = 246
               Left = 276
               Bottom = 361
               Right = 437
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCostCenter"
            Begin Extent = 
               Top = 366
               Left = 38
               Bottom = 481
               Right = 212
  ' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_ITEM'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'          End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPB"
            Begin Extent = 
               Top = 366
               Left = 250
               Bottom = 481
               Right = 402
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPaymentMethod"
            Begin Extent = 
               Top = 486
               Left = 38
               Bottom = 601
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 486
               Left = 262
               Bottom = 601
               Right = 473
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DocumentType"
            Begin Extent = 
               Top = 606
               Left = 38
               Bottom = 721
               Right = 240
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbTax"
            Begin Extent = 
               Top = 606
               Left = 278
               Bottom = 721
               Right = 435
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
      Begin ColumnWidths = 65
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
         Column = 4470
         Alias = 2040
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_ITEM'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_ITEM'

GO
/****** Object:  View [dbo].[EXPENSE_REMITTANCE_POST]    Script Date: 04/28/2009 11:14:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[EXPENSE_REMITTANCE_POST]
AS
SELECT     dbo.[Document].DocumentID, dbo.[Document].DocumentNo, dbo.[Document].DocumentTypeID, dbo.DocumentType.DocumentTypeName, 
                      dbo.FnExpenseDocument.ExpenseType, dbo.FnExpenseDocument.PaymentType, dbo.DbCompany.CompanyCode, dbo.DbCompany.CompanyName, 
                      dbo.DbPB.PBCode, dbo.[Document].RequesterID, dbo.[Document].CreatorID, dbo.[Document].ReceiverID, dbo.[Document].ApproverID, 
                      ISNULL(dbo.FnExpenseDocument.TotalExpense, 0) AS TotalExpense, ISNULL(dbo.FnExpenseDocument.TotalAdvance, 0) AS TotalAdvance, 
                      ISNULL(dbo.FnExpenseDocument.DifferenceAmount, 0) AS DifferenceAmount, ISNULL(dbo.FnExpenseDocument.TotalRemittance, 0) 
                      AS TotalRemittance, dbo.[Document].Subject AS DescriptionDocument, CONVERT(VARCHAR, dbo.[Document].DocumentDate, 112) AS DocumentDate, 
                      dbo.DbPaymentMethod.PaymentMethodCode, RIGHT(REPLICATE('0', 10) + ISNULL(dbo.[Document].BankAccount, ''), 10) AS BankAccount, 
                      CONVERT(VARCHAR, dbo.[Document].PostingDate, 112) AS PostingDate, CONVERT(VARCHAR, dbo.[Document].BaseLineDate, 112) AS BaseLineDate, 
                      AdvanceDocument.DocumentNo AS AdvanceDocumentNo, ISNULL(dbo.AvAdvanceDocument.RemittanceAmount, 0) AS RemittanceAmount, 
                      ISNULL(dbo.AvAdvanceDocument.ExpenseAmount, 0) AS ExpenseAmount
FROM         dbo.[Document] INNER JOIN
                      dbo.FnExpenseDocument ON dbo.[Document].DocumentID = dbo.FnExpenseDocument.DocumentID INNER JOIN
                      dbo.FnExpenseAdvance ON dbo.FnExpenseDocument.ExpenseID = dbo.FnExpenseAdvance.ExpenseID INNER JOIN
                      dbo.AvAdvanceDocument ON dbo.FnExpenseAdvance.AdvanceID = dbo.AvAdvanceDocument.AdvanceID INNER JOIN
                      dbo.[Document] AS AdvanceDocument ON dbo.AvAdvanceDocument.DocumentID = AdvanceDocument.DocumentID LEFT OUTER JOIN
                      dbo.DbCompany ON dbo.[Document].CompanyID = dbo.DbCompany.CompanyID LEFT OUTER JOIN
                      dbo.DocumentType ON dbo.[Document].DocumentTypeID = dbo.DocumentType.DocumentTypeID LEFT OUTER JOIN
                      dbo.DbPB ON dbo.FnExpenseDocument.PBID = dbo.DbPB.PBID LEFT OUTER JOIN
                      dbo.DbPaymentMethod ON dbo.[Document].PaymentMethodID = dbo.DbPaymentMethod.PaymentMethodID
WHERE     (ISNULL(dbo.AvAdvanceDocument.RemittanceAmount, 0) > 0)


GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[18] 4[17] 2[55] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[42] 4[54] 3) )"
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
         Configuration = "(H (1[81] 3) )"
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
               Top = 21
               Left = 7
               Bottom = 387
               Right = 177
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseDocument"
            Begin Extent = 
               Top = 0
               Left = 312
               Bottom = 416
               Right = 541
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DocumentType"
            Begin Extent = 
               Top = 551
               Left = 226
               Bottom = 666
               Right = 428
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 370
               Left = 299
               Bottom = 485
               Right = 510
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPB"
            Begin Extent = 
               Top = 261
               Left = 596
               Bottom = 376
               Right = 748
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPaymentMethod"
            Begin Extent = 
               Top = 89
               Left = 315
               Bottom = 204
               Right = 501
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseAdvance"
            Begin Extent = 
               Top = 100
               Left = 571
               Bottom = 352
               Right = 758
            End' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_REMITTANCE_POST'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AvAdvanceDocument"
            Begin Extent = 
               Top = 79
               Left = 636
               Bottom = 485
               Right = 846
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AdvanceDocument"
            Begin Extent = 
               Top = 6
               Left = 796
               Bottom = 459
               Right = 966
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
      Begin ColumnWidths = 52
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2820
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
         Width = 1875
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1110
         Width = 960
         Width = 2520
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 6945
         Alias = 2280
         Table = 3225
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_REMITTANCE_POST'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_REMITTANCE_POST'

GO
/****** Object:  View [dbo].[PostingDate]    Script Date: 04/28/2009 11:14:30 ******/
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


GO
/****** Object:  View [dbo].[REMITTANCE_POST]    Script Date: 04/28/2009 11:14:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[REMITTANCE_POST]
AS
SELECT     dbo.[Document].DocumentID, dbo.[Document].DocumentNo, dbo.[Document].DocumentTypeID, dbo.DocumentType.DocumentTypeName, 
                      dbo.DbCompany.CompanyCode, dbo.DbPB.PBCode, CONVERT(VARCHAR, dbo.[Document].DocumentDate, 112) AS DocDate, CONVERT(VARCHAR, 
                      dbo.[Document].PostingDate, 112) AS PostingDate, CONVERT(VARCHAR, dbo.[Document].BaseLineDate, 112) AS BaseLineDate, 
                      dbo.[Document].PaymentMethodID, RIGHT(REPLICATE('0', 10) + ISNULL(dbo.[Document].BankAccount, ''), 10) AS BankAccount, 
                      dbo.[Document].RequesterID, dbo.[Document].CreatorID, dbo.[Document].ReceiverID, dbo.[Document].ApproverID, 
                      dbo.[Document].Subject AS Description, dbo.[Document].BranchCode, ISNULL(dbo.FnRemittance.TotalAmount, 0) AS Amount, 
                      dbo.AvAdvanceDocument.DocumentID AS AdvanceID, AdvanceDocument.DocumentNo AS AdvanceNo, ISNULL(dbo.AvAdvanceDocument.Amount, 0) 
                      AS AdvanceAmount, ISNULL(dbo.AvAdvanceDocument.RemittanceAmount, 0) AS RemittanceAmount, ISNULL(dbo.AvAdvanceDocument.ExpenseAmount, 
                      0) AS ExpenseAmount
FROM         dbo.FnRemittance INNER JOIN
                      dbo.[Document] ON dbo.FnRemittance.DocumentID = dbo.[Document].DocumentID LEFT OUTER JOIN
                      dbo.AvAdvanceDocument INNER JOIN
                      dbo.[Document] AS AdvanceDocument ON dbo.AvAdvanceDocument.DocumentID = AdvanceDocument.DocumentID INNER JOIN
                      dbo.FnRemittanceAdvance ON dbo.AvAdvanceDocument.AdvanceID = dbo.FnRemittanceAdvance.AdvanceID ON 
                      dbo.FnRemittance.RemittanceID = dbo.FnRemittanceAdvance.RemittanceID LEFT OUTER JOIN
                      dbo.DbPB ON dbo.FnRemittance.PBID = dbo.DbPB.PBID LEFT OUTER JOIN
                      dbo.DbCompany ON dbo.[Document].CompanyID = dbo.DbCompany.CompanyID LEFT OUTER JOIN
                      dbo.DocumentType ON dbo.[Document].DocumentTypeID = dbo.DocumentType.DocumentTypeID
WHERE     (ISNULL(dbo.AvAdvanceDocument.RemittanceAmount, 0) > 0)


GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[28] 4[4] 2[65] 3) )"
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
         Begin Table = "FnRemittance"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 191
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Document"
            Begin Extent = 
               Top = 6
               Left = 229
               Bottom = 121
               Right = 399
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AvAdvanceDocument"
            Begin Extent = 
               Top = 54
               Left = 827
               Bottom = 392
               Right = 1037
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AdvanceDocument"
            Begin Extent = 
               Top = 143
               Left = 1196
               Bottom = 258
               Right = 1366
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnRemittanceAdvance"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 241
               Right = 226
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPB"
            Begin Extent = 
               Top = 126
               Left = 264
               Bottom = 241
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 256
               Left = 322
               Bottom = 371
               Right = 533
           ' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'REMITTANCE_POST'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DocumentType"
            Begin Extent = 
               Top = 250
               Left = 525
               Bottom = 365
               Right = 727
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
      Begin ColumnWidths = 24
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 3675
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
         Width = 1500
         Width = 1500
         Width = 1725
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4815
         Alias = 1920
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'REMITTANCE_POST'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'REMITTANCE_POST'

GO
/****** Object:  View [dbo].[VIEW_POST_AP]    Script Date: 04/28/2009 11:14:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VIEW_POST_AP]
AS
SELECT     dbo.BAPIACAP09.DOC_KIND, dbo.BAPIACAP09.DOC_ID, dbo.BAPIACAP09.DOC_SEQ, dbo.BAPIACAP09.ITEMNO_ACC, 
                      dbo.BAPIACAP09.VENDOR_NO AS Account, ISNULL(dbo.BAPIACAP09.TAX_CODE, N'NV') AS TAX_CODE, RIGHT(dbo.BAPIACAP09.BLINE_DATE, 2) 
                      + '/' + SUBSTRING(dbo.BAPIACAP09.BLINE_DATE, 3, 2) + '/' + LEFT(dbo.BAPIACAP09.BLINE_DATE, 4) AS BaseDate, dbo.BAPIACAP09.PMNTTRMS, 
                      dbo.BAPIACAP09.PYMT_METH, dbo.BAPIACCR09.CURRENCY, CONVERT(VARCHAR, CAST(dbo.BAPIACCR09.AMT_DOCCUR AS MONEY), 1) 
                      AS AMT_DOCCUR, '' AS CostCenter, '' AS InterOrder, CASE WHEN BAPIACAP09.VENDOR_NO = 'PCADVCL' THEN ISNULL
                          ((SELECT     FIELD3
                              FROM         BAPIACEXTC
                              WHERE     DOC_ID = BAPIACAP09.DOC_ID AND DOC_SEQ = BAPIACAP09.DOC_SEQ AND FIELD1 = 'WTH1'), '') ELSE '' END AS WHTCode1, 
                      CASE WHEN BAPIACAP09.VENDOR_NO = 'PCADVCL' THEN ISNULL
                          ((SELECT     FIELD4
                              FROM         BAPIACEXTC
                              WHERE     DOC_ID = BAPIACAP09.DOC_ID AND DOC_SEQ = BAPIACAP09.DOC_SEQ AND FIELD1 = 'WTH1'), '') ELSE '' END AS WHTBase1, 
                      CASE WHEN BAPIACAP09.VENDOR_NO = 'PCADVCL' THEN ISNULL
                          ((SELECT     FIELD3
                              FROM         BAPIACEXTC
                              WHERE     DOC_ID = BAPIACAP09.DOC_ID AND DOC_SEQ = BAPIACAP09.DOC_SEQ AND FIELD1 = 'WTH2'), '') ELSE '' END AS WHTCode2, 
                      CASE WHEN BAPIACAP09.VENDOR_NO = 'PCADVCL' THEN ISNULL
                          ((SELECT     FIELD4
                              FROM         BAPIACEXTC
                              WHERE     DOC_ID = BAPIACAP09.DOC_ID AND DOC_SEQ = BAPIACAP09.DOC_SEQ AND FIELD1 = 'WTH2'), '') ELSE '' END AS WHTBase2, 
                      dbo.BAPIACAP09.ALLOC_NMBR, dbo.BAPIACAP09.ITEM_TEXT, CASE WHEN ISNULL(BAPIACAP09.SP_GL_IND, '') = '' AND 
                      BAPIACCR09.AMT_DOCCUR >= 0 THEN '21' WHEN ISNULL(BAPIACAP09.SP_GL_IND, '') = '' AND 
                      BAPIACCR09.AMT_DOCCUR < 0 THEN '31' WHEN ISNULL(BAPIACAP09.SP_GL_IND, '') <> '' AND 
                      BAPIACCR09.AMT_DOCCUR >= 0 THEN '29E' WHEN ISNULL(BAPIACAP09.SP_GL_IND, '') <> '' AND 
                      BAPIACCR09.AMT_DOCCUR < 0 THEN '39E' END AS PK
FROM         dbo.BAPIACAP09 INNER JOIN
                      dbo.BAPIACCR09 ON dbo.BAPIACAP09.DOC_ID = dbo.BAPIACCR09.DOC_ID AND dbo.BAPIACAP09.DOC_SEQ = dbo.BAPIACCR09.DOC_SEQ AND 
                      dbo.BAPIACAP09.ITEMNO_ACC = dbo.BAPIACCR09.ITEMNO_ACC AND dbo.BAPIACAP09.DOC_KIND = dbo.BAPIACCR09.DOC_KIND


GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[20] 4[57] 2[4] 3) )"
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
         Begin Table = "BAPIACAP09"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 210
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "BAPIACCR09"
            Begin Extent = 
               Top = 6
               Left = 248
               Bottom = 121
               Right = 407
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
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 6330
         Alias = 3060
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'VIEW_POST_AP'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'VIEW_POST_AP'

GO
/****** Object:  View [dbo].[VIEW_POST_AR]    Script Date: 04/28/2009 11:14:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VIEW_POST_AR]
AS
SELECT     dbo.BAPIACAR09.DOC_KIND, dbo.BAPIACAR09.DOC_ID, dbo.BAPIACAR09.DOC_SEQ, dbo.BAPIACAR09.ITEMNO_ACC, 
                      dbo.BAPIACAR09.CUSTOMER AS Account, dbo.BAPIACAR09.TAX_CODE, dbo.BAPIACAR09.BLINE_DATE AS BaseDate, '' AS PMNTTRMS, 
                      '' AS PYMT_METH, '' AS CURRENCY, CONVERT(VARCHAR, CAST(dbo.BAPIACCR09.AMT_DOCCUR AS MONEY), 1) AS AMT_DOCCUR, '' AS CostCenter, 
                      '' AS InterOrder, '' AS WHTCode1, '' AS WHTBase1, '' AS WHTCode2, '' AS WHTBase2, dbo.BAPIACAR09.ALLOC_NMBR, dbo.BAPIACAR09.ITEM_TEXT, 
                      CASE WHEN ISNULL(BAPIACAR09.SP_GL_IND, '') = '' AND BAPIACCR09.AMT_DOCCUR >= 0 THEN '01' WHEN ISNULL(BAPIACAR09.SP_GL_IND, '') 
                      = '' AND BAPIACCR09.AMT_DOCCUR < 0 THEN '11' WHEN ISNULL(BAPIACAR09.SP_GL_IND, '') <> '' AND 
                      BAPIACCR09.AMT_DOCCUR >= 0 THEN '09' + BAPIACAR09.SP_GL_IND WHEN ISNULL(BAPIACAR09.SP_GL_IND, '') <> '' AND 
                      BAPIACCR09.AMT_DOCCUR < 0 THEN '19' + BAPIACAR09.SP_GL_IND END AS PK
FROM         dbo.BAPIACAR09 INNER JOIN
                      dbo.BAPIACCR09 ON dbo.BAPIACAR09.DOC_ID = dbo.BAPIACCR09.DOC_ID AND dbo.BAPIACAR09.DOC_SEQ = dbo.BAPIACCR09.DOC_SEQ AND 
                      dbo.BAPIACAR09.ITEMNO_ACC = dbo.BAPIACCR09.ITEMNO_ACC AND dbo.BAPIACAR09.DOC_KIND = dbo.BAPIACCR09.DOC_KIND


GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[19] 4[43] 2[12] 3) )"
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
         Begin Table = "BAPIACAR09"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 210
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "BAPIACCR09"
            Begin Extent = 
               Top = 6
               Left = 248
               Bottom = 121
               Right = 407
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
      Begin ColumnWidths = 9
         Width = 284
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
         Column = 4725
         Alias = 3480
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'VIEW_POST_AR'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'VIEW_POST_AR'

GO
/****** Object:  View [dbo].[VIEW_POST_GL]    Script Date: 04/28/2009 11:14:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VIEW_POST_GL]
AS
SELECT     dbo.BAPIACGL09.DOC_KIND, dbo.BAPIACGL09.DOC_ID, dbo.BAPIACGL09.DOC_SEQ, dbo.BAPIACGL09.ITEMNO_ACC, 
                      dbo.BAPIACGL09.GL_ACCOUNT AS Account, ISNULL(dbo.BAPIACGL09.TAX_CODE, N'NV') AS TAX_CODE, '' AS BaseDate, '' AS PMNTTRMS, 
                      '' AS PYMT_METH, '' AS CURRENCY, CONVERT(VARCHAR, CAST(dbo.BAPIACCR09.AMT_DOCCUR AS MONEY), 1) AS AMT_DOCCUR, 
                      dbo.BAPIACGL09.COSTCENTER, dbo.BAPIACGL09.ORDERID AS InterOrder, '' AS WHTCode1, '' AS WHTBase1, '' AS WHTCode2, '' AS WHTBase2, 
                      dbo.BAPIACGL09.ALLOC_NMBR, dbo.BAPIACGL09.ITEM_TEXT, 
                      CASE WHEN BAPIACCR09.AMT_DOCCUR >= 0 THEN '40' WHEN BAPIACCR09.AMT_DOCCUR < 0 THEN '50' END AS PK
FROM         dbo.BAPIACCR09 INNER JOIN
                      dbo.BAPIACGL09 ON dbo.BAPIACCR09.DOC_ID = dbo.BAPIACGL09.DOC_ID AND dbo.BAPIACCR09.DOC_SEQ = dbo.BAPIACGL09.DOC_SEQ AND 
                      dbo.BAPIACCR09.ITEMNO_ACC = dbo.BAPIACGL09.ITEMNO_ACC AND dbo.BAPIACCR09.DOC_KIND = dbo.BAPIACGL09.DOC_KIND


GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[26] 4[35] 2[20] 3) )"
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
         Begin Table = "BAPIACCR09"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 197
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "BAPIACGL09"
            Begin Extent = 
               Top = 6
               Left = 235
               Bottom = 121
               Right = 409
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
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4245
         Alias = 2430
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'VIEW_POST_GL'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'VIEW_POST_GL'

GO
/****** Object:  View [dbo].[VIEW_POST_HEAD]    Script Date: 04/28/2009 11:14:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VIEW_POST_HEAD]
AS
SELECT     dbo.BAPIACHE09.DOC_KIND, dbo.BAPIACHE09.DOC_ID, dbo.BAPIACHE09.DOC_SEQ, dbo.BAPIACHE09.FI_DOC, RIGHT(dbo.BAPIACHE09.DOC_DATE, 2)
                       + '.' + SUBSTRING(dbo.BAPIACHE09.DOC_DATE, 5, 2) + '.' + LEFT(dbo.BAPIACHE09.DOC_DATE, 4) AS DOC_DATE, 
                      RIGHT(dbo.BAPIACHE09.PSTNG_DATE, 2) + '.' + SUBSTRING(dbo.BAPIACHE09.PSTNG_DATE, 5, 2) + '.' + LEFT(dbo.BAPIACHE09.PSTNG_DATE, 4) 
                      AS POST_DATE, dbo.BAPIACHE09.DOC_TYPE, dbo.BAPIACHE09.COMP_CODE, dbo.DbCompany.CompanyName, 
                      SUBSTRING(dbo.BAPIACHE09.DOC_DATE, 3, 2) AS PERIOD, LEFT(dbo.BAPIACHE09.DOC_DATE, 4) AS YEAR, dbo.BAPIACEXTC.FIELD2 AS BRNCH, 
                      'THB' AS Currency, dbo.BAPIACHE09.REF_DOC_NO, dbo.BAPIACHE09.HEADER_TXT AS HEADERTXT
FROM         dbo.BAPIACHE09 INNER JOIN
                      dbo.BAPIACEXTC ON dbo.BAPIACHE09.DOC_ID = dbo.BAPIACEXTC.DOC_ID AND dbo.BAPIACHE09.DOC_SEQ = dbo.BAPIACEXTC.DOC_SEQ AND 
                      dbo.BAPIACHE09.DOC_KIND = dbo.BAPIACEXTC.DOC_KIND INNER JOIN
                      dbo.DbCompany ON dbo.BAPIACHE09.COMP_CODE = dbo.DbCompany.CompanyCode
WHERE     (dbo.BAPIACEXTC.FIELD1 = N'BRNCH')


GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[79] 4[4] 2[4] 3) )"
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
         Begin Table = "BAPIACHE09"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 335
               Right = 220
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "BAPIACEXTC"
            Begin Extent = 
               Top = 8
               Left = 336
               Bottom = 279
               Right = 488
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 107
               Left = 551
               Bottom = 222
               Right = 762
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
      Begin ColumnWidths = 16
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'VIEW_POST_HEAD'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'VIEW_POST_HEAD'

GO
/****** Object:  View [dbo].[VIEW_POST_TAX]    Script Date: 04/28/2009 11:14:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VIEW_POST_TAX]
AS
SELECT     dbo.BAPIACTX09.DOC_KIND, dbo.BAPIACTX09.DOC_ID, dbo.BAPIACTX09.DOC_SEQ, dbo.BAPIACTX09.ITEMNO_ACC, 
                      dbo.BAPIACTX09.GL_ACCOUNT AS Account, dbo.BAPIACTX09.TAX_CODE, '' AS BaseDate, '' AS PMNTTRMS, '' AS PYMT_METH, '' AS CURRENCY, 
                      CONVERT(VARCHAR, CAST(dbo.BAPIACCR09.AMT_DOCCUR AS MONEY), 1) AS AMT_DOCCUR, '' AS CostCenter, '' AS InterOrder, '' AS WHTCode1, 
                      '' AS WHTBase1, '' AS WHTCode2, '' AS WHTBase2, '' AS ALLOC_NMBR, '' AS ITEM_TEXT, 
                      CASE WHEN BAPIACCR09.AMT_DOCCUR >= 0 THEN '40' WHEN BAPIACCR09.AMT_DOCCUR < 0 THEN '50' END AS PK
FROM         dbo.BAPIACTX09 INNER JOIN
                      dbo.BAPIACCR09 ON dbo.BAPIACTX09.DOC_ID = dbo.BAPIACCR09.DOC_ID AND dbo.BAPIACTX09.DOC_SEQ = dbo.BAPIACCR09.DOC_SEQ AND 
                      dbo.BAPIACTX09.ITEMNO_ACC = dbo.BAPIACCR09.ITEMNO_ACC AND dbo.BAPIACTX09.DOC_KIND = dbo.BAPIACCR09.DOC_KIND


GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[10] 2[4] 3) )"
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
         Begin Table = "BAPIACTX09"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 219
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "BAPIACCR09"
            Begin Extent = 
               Top = 6
               Left = 257
               Bottom = 121
               Right = 416
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
      Begin ColumnWidths = 15
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
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 5565
         Alias = 3000
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'VIEW_POST_TAX'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'VIEW_POST_TAX'
