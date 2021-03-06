USE [eAccounting]
GO
/****** Object:  View [dbo].[EXPENSE_ITEM]    Script Date: 04/20/2009 16:35:27 ******/
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
                      NULLIF (dbo.FnExpenseInvoice.Description, ''), NULLIF (dbo.[Document].Subject, ''), '') AS Description, COALESCE (NULLIF (dbo.FnExpenseInvoice.BranchCode, ''), NULLIF (dbo.[Document].BranchCode, ''), '') AS BranchCode, 
                      dbo.[Document].BankAccount, CONVERT(VARCHAR, dbo.[Document].PostingDate, 112) AS PostingDate, CONVERT(VARCHAR, 
                      dbo.[Document].BaseLineDate, 112) AS BaseLineDate
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
         Configuration = "(H (1[49] 4[7] 2[40] 3) )"
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
         Configuration = "(H (1[28] 4[46] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1[75] 4) )"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[28] 2) )"
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
         Top = -192
         Left = -494
      End
      Begin Tables = 
         Begin Table = "Document"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 452
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbCompany"
            Begin Extent = 
               Top = 260
               Left = 238
               Bottom = 375
               Right = 449
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DocumentType"
            Begin Extent = 
               Top = 389
               Left = 238
               Bottom = 504
               Right = 440
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseDocument"
            Begin Extent = 
               Top = 32
               Left = 294
               Bottom = 251
               Right = 523
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbPB"
            Begin Extent = 
               Top = 11
               Left = 632
               Bottom = 126
               Right = 784
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseInvoice"
            Begin Extent = 
               Top = 193
               Left = 554
               Bottom = 486
               Right = 742
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "DbPaymentMethod"
            Begin Extent = 
               Top = 519
               Left = 239
               Bottom = 634
               Right = 425
         ' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'EXPENSE_ITEM'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'   End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FnExpenseInvoiceItem"
            Begin Extent = 
               Top = 238
               Left = 755
               Bottom = 481
               Right = 926
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbAccount"
            Begin Extent = 
               Top = 240
               Left = 1125
               Bottom = 355
               Right = 1325
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "DbCostCenter"
            Begin Extent = 
               Top = 629
               Left = 1076
               Bottom = 744
               Right = 1250
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbInternalOrder"
            Begin Extent = 
               Top = 458
               Left = 1106
               Bottom = 573
               Right = 1267
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DbTax"
            Begin Extent = 
               Top = 553
               Left = 520
               Bottom = 668
               Right = 677
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
      Begin ColumnWidths = 67
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
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 7050
         Alias = 2400
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
