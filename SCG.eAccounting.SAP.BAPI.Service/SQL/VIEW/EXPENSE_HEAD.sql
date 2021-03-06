USE [eAccounting]
GO
/****** Object:  View [dbo].[EXPENSE_HEAD]    Script Date: 04/20/2009 16:35:37 ******/
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
                      ISNULL(dbo.FnExpenseInvoice.WHTAmount2, 0) AS WHTAmount2, COALESCE (NULLIF (dbo.FnExpenseInvoice.BranchCode, ''), NULLIF (dbo.[Document].BranchCode, ''), '') AS BranchCode, dbo.DbPaymentMethod.PaymentMethodCode, 
                      dbo.[Document].BankAccount, CONVERT(VARCHAR, dbo.[Document].PostingDate, 112) AS PostingDate, CONVERT(VARCHAR, 
                      dbo.[Document].BaseLineDate, 112) AS BaseLineDate, COALESCE (NULLIF (dbo.FnExpenseInvoice.Description, ''), NULLIF (dbo.[Document].Subject, ''), 
                      '') AS Description
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
         Configuration = "(H (1[85] 4[4] 2[4] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[17] 2[58] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1[71] 3) )"
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
         Configuration = "(H (1[44] 4) )"
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
      ActivePaneConfig = 9
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
               Top = 28
               Left = 493
               Bottom = 326
               Right = 722
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseInvoice"
            Begin Extent = 
               Top = 13
               Left = 794
               Bottom = 190
               Right = 982
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DocumentType"
            Begin Extent = 
               Top = 185
               Left = 237
               Bottom = 300
               Right = 439
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 58
               Left = 238
               Bottom = 173
               Right = 449
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPB"
            Begin Extent = 
               Top = 197
               Left = 758
               Bottom = 312
               Right = 910
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPaymentMethod"
            Begin Extent = 
               Top = 345
               Left = 238
               Bottom = 460
               Right = 424
            End
 ' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_HEAD'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'           DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbTax"
            Begin Extent = 
               Top = 246
               Left = 1011
               Bottom = 361
               Right = 1168
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
      PaneHidden = 
   End
   Begin DataPane = 
      PaneHidden = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 52
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 6555
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
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_HEAD'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_HEAD'
